using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CodeBase;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormJobManager2:Form, ISignalProcessor {
		///<summary>All jobs</summary>
		private List<Job> _listJobsAll=new List<Job>();
		///<summary>Jobs to be displayed in tree.</summary>
		private List<Job> _listJobsFiltered=new List<Job>();
		///<summary>Jobs to be highlighted in the tree.</summary>
		private List<long> _listJobNumsHighlight=new List<long>();
		///<summary>Cached permissions for Job Manager.</summary>
		private List<JobPermission> _listJobPermissionsAll=new List<JobPermission>();
		private bool _isOverride;

		private static Color[] statusColors = new Color[]{
					Color.FromArgb(254,235,233), //High Priority
					Color.White, //Normal Priority
					Color.FromArgb(233,233,233), //Low Priority
					Color.FromArgb(200,200,200)//On Hold
				};
		
		public FormJobManager2() {
			InitializeComponent();
		}

		private void FormJobManager_Load(object sender,EventArgs e) {
			comboUser.Items.Add("All");//All is first.
			comboUser.Items.Add("Unassigned");
			UserodC.ShortList.ForEach(x => comboUser.Items.Add(x.UserName));
			comboUser.SelectedIndex=UserodC.ShortList.FindIndex(x => x.UserNum==Security.CurUser.UserNum);
			if(comboUser.SelectedIndex>-1) {
				comboUser.SelectedIndex=comboUser.SelectedIndex+2;
			}
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
			comboCategorySearch.Items.Add("All");
			Enum.GetNames(typeof(JobCategory)).ToList().ForEach(x => comboCategorySearch.Items.Add(x));
			comboCategorySearch.SelectedIndex=0;
			Enum.GetNames(typeof(GroupJobsBy)).ToList().ForEach(x => comboGroup.Items.Add(x));
			comboGroup.SelectedIndex=(int)GroupJobsBy.Heirarchy;
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
				butAddJob.Enabled=false;
			}
			RefreshAndFillThreaded();
			Signalods.Subscribe(this);
		}

		///<summary>Fills all in memory data from the DB on a seperate thread and then refills controls.</summary>
		private void RefreshAndFillThreaded() {
			ODThread thread = new ODThread((o) => {
				_listJobsAll=Jobs.GetAll();
				Jobs.FillInMemoryLists(_listJobsAll);
				_listJobPermissionsAll=JobPermissions.GetList();
				_listJobsFiltered=_listJobsAll.Select(x => x.Copy()).ToList();
				this.Invoke((Action)FilterAndFill);
				this.Invoke((Action)FillGridWorkSummary);
				this.Invoke((Action)FillGridActions);
				this.Invoke((Action)FillGridMyJobs);
			});
			thread.AddExceptionHandler((ex) => { MessageBox.Show(ex.Message); });
			thread.Start();
		}

		///<summary>Always fills from _ListJobsAll.</summary>
		private void FillGridActions() {
			long selectedJobNum=0;
			if(userControlJobEdit.GetJob()!=null) {
				selectedJobNum=userControlJobEdit.GetJob().JobNum;
			}
			gridAction.BeginUpdate();
			gridAction.Columns.Clear();
			gridAction.Columns.Add(new ODGridColumn("Priority",50) { TextAlign=HorizontalAlignment.Center });
			gridAction.Columns.Add(new ODGridColumn("Owner",55) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridAction.Columns.Add(new ODGridColumn("",245));
			gridAction.Rows.Clear();
			ODGridRow row;
			Userod UserFilter = null;
			if(comboUser.SelectedIndex==1) {
				UserFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				UserFilter=UserodC.ShortList[comboUser.SelectedIndex-2];
			}
			//Sort jobs into action dictionary
			Dictionary<JobAction,List<Job>> dictActions = new Dictionary<JobAction,List<Job>>();
			foreach(Job job in _listJobsAll) {
				JobAction action;
				if(UserFilter==null) {
					action=job.OwnerAction;
				}
				else {
					action=job.ActionForUser(UserFilter);
				}
				if(!dictActions.ContainsKey(action)) {
					dictActions[action]=new List<Job>();
				}
				dictActions[action].Add(job);
			}
			//sort dictionary so actions will appear in same order
			dictActions=dictActions.OrderBy(x => (int)x.Key).ToDictionary(x=>x.Key,x=>x.Value);
			foreach(KeyValuePair<JobAction,List<Job>> kvp in dictActions) {
				if(kvp.Key==JobAction.Undefined || kvp.Key==JobAction.UnknownJobPhase || kvp.Key==JobAction.None) {
					//Undefined occurs when there is no action to take. 
					//UnknownJobPhase occurs when there is something wrong with the programming.
					continue;
				}
				List<Job> listJobsSorted = kvp.Value//filter
					.OrderBy(x => UserFilter==null || x.OwnerNum!=UserFilter.UserNum)//sort
					.ThenBy(x => x.OwnerNum!=0)
					.ThenBy(x => x.Priority).ToList();
				if(!checkShowUnassinged.Checked) {
					listJobsSorted.RemoveAll(x => x.Priority==JobPriority.OnHold);
					if(UserFilter!=null) {
						//Actions in this list will be filtered by checkShowUnassigned. If not in this list, items will always show if applicable 
						//(For example ApproveJob always shows if user has approval permission.)
						JobAction[] JobActionsUnassigned = new JobAction[] {
							JobAction.WriteConcept,
							JobAction.WriteJob,
							JobAction.WriteCode,
							JobAction.TakeJob,
							JobAction.ReviewCode
						};
						if(UserFilter.UserNum>0 && JobActionsUnassigned.Contains(kvp.Key)) {
							listJobsSorted.RemoveAll(x => x.OwnerNum==0 || kvp.Key==JobAction.TakeJob);//filters out passive actions if unassigned. Bug.
						}
					}
				}
				if(listJobsSorted.Count==0) {
					continue;
				}
				gridAction.Rows.Add(new ODGridRow("","",kvp.Key.ToString()) { ColorBackG=gridAction.HeaderColor,Bold=true });
				listJobsSorted.ForEach(x => gridAction.Rows.Add(
					new ODGridRow(
						new ODGridCell(x.Priority.ToString()) { CellColor=statusColors[(int)x.Priority] },
						new ODGridCell(x.OwnerNum==0 ? "-" : Userods.GetName(x.OwnerNum)),
						new ODGridCell(x.ToString()) { CellColor=(x.ToString().ToLower().Contains(textSearch.Text.ToLower())&&!string.IsNullOrWhiteSpace(textSearch.Text) ? Color.LightYellow : Color.Empty) }
						) { Tag=x }
					)
				);
			}
			gridAction.EndUpdate();
			//RESELECT JOB
			if(selectedJobNum>0) {
				for(int i = 0;i<gridAction.Rows.Count;i++) {
					if((gridAction.Rows[i].Tag is Job) && ((Job)gridAction.Rows[i].Tag).JobNum==selectedJobNum) {
						gridAction.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridMyJobs() {
			gridMyJobs.Title="Jobs"+(string.IsNullOrWhiteSpace(comboUser.Text)?"": " For "+comboUser.Text);
			gridMyJobs.BeginUpdate();
			gridMyJobs.Columns.Clear();
			gridMyJobs.Columns.Add(new ODGridColumn("JobNum",50) { TextAlign=HorizontalAlignment.Center, SortingStrategy=GridSortingStrategy.AmountParse });// X for yes
			gridMyJobs.Columns.Add(new ODGridColumn("Type",80) { TextAlign=HorizontalAlignment.Center });// X for yes
			gridMyJobs.Columns.Add(new ODGridColumn("Phase",85) { TextAlign=HorizontalAlignment.Center });// X for yes
			gridMyJobs.Columns.Add(new ODGridColumn("Priority",50) { TextAlign=HorizontalAlignment.Center });
			gridMyJobs.Columns.Add(new ODGridColumn("Description",300));// X for yes
			gridMyJobs.Columns.Add(new ODGridColumn("Current\r\nOwner",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Current\r\nAction",125) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Needs\r\nAppr.",45) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Concept",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			//gridMyJobs.Columns.Add(new ODGridColumn("Concept\r\nApprover",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Expert",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			//gridMyJobs.Columns.Add(new ODGridColumn("Writeup\r\nApprover",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			//gridMyJobs.Columns.Add(new ODGridColumn("Change\r\nApprover",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Engineer",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Reviewer(s)",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Documentor",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Customer\r\nContact",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			gridMyJobs.Columns.Add(new ODGridColumn("Watchers",75) { TextAlign=HorizontalAlignment.Center });// X for yes, - for unassigned
			if(checkShowVersion.Checked) {
				gridMyJobs.Columns.Add(new ODGridColumn("Version",75) { TextAlign=HorizontalAlignment.Center });
			}
			gridMyJobs.Rows.Clear();
			List<Job> _listJobsSorted = _listJobsAll
				.OrderBy(x => x.PhaseCur)
				.ThenBy(x => x.UserNumDocumenter)
				.ThenBy(x => x.ListJobReviews.Count)
				.ThenBy(x => x.UserNumEngineer)
				.ThenBy(x=>x.UserNumApproverChange)
				.ThenBy(x=>x.UserNumApproverJob)
				.ThenBy(x=>x.UserNumExpert)
				.ThenBy(x=>x.UserNumApproverConcept)
				.ThenBy(x=>x.UserNumConcept)
				.ThenBy(x=>x.IsApprovalNeeded)
				.ThenBy(x=>x.OwnerNum)
				.ThenBy(x => x.Priority)
				.ThenBy(x=>x.JobNum)
				.ToList();
			Userod userFilter = null;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=UserodC.ShortList[comboUser.SelectedIndex-2];
			}
			foreach(Job job in _listJobsSorted) {
				if(!checkShowComplete.Checked && new[] { JobPhase.Cancelled,JobPhase.Complete }.Contains(job.PhaseCur) && job.OwnerAction!=JobAction.ContactCustomer) {
					continue;//skip complete if check complete not checked.
				}
				if(userFilter==null //all
					|| (userFilter.UserNum==0 && job.OwnerNum==0) //unasigned
					|| (userFilter.UserNum!=0 && Jobs.GetUserNums(job).Contains(userFilter.UserNum))) //filter
				{
					gridMyJobs.Rows.Add(GetMyJobRow(job,userFilter));
				}
			}
			gridMyJobs.EndUpdate();
			if(userControlJobEdit.GetJob()!=null) {
				int idx = gridMyJobs.Rows.Cast<ODGridRow>().ToList().FindIndex(x => (x.Tag is Job) && ((Job)x.Tag).JobNum==userControlJobEdit.GetJob().JobNum);
				if(idx>-1) {
					gridMyJobs.SetSelected(idx,true);
				}
				else {
					gridMyJobs.SetSelected(false);
				}
			}
		}

		///<summary>Adds, edits, or removes jobs to the grid. jobNum must always be set, job can be null to delete from grid.</summary>
		private void UpdateGridMyJobs(long jobNum, Job job) {
			Userod userFilter = null;
			if(comboUser.SelectedIndex==1) {
				userFilter=new Userod() { UserName="Unassigned",UserNum=0 };
			}
			else if(comboUser.SelectedIndex>1) {
				userFilter=UserodC.ShortList[comboUser.SelectedIndex-2];
			}
			for(int i = 0;i<gridMyJobs.Rows.Count;i++) {
				//ODGridRow row = gridMyJobs.Rows[i];
				if(((Job)gridMyJobs.Rows[i].Tag).JobNum!=jobNum) {
					continue;
				}
				if(job==null //job deleted 
					|| (userFilter!=null && userFilter.UserNum==0 && Jobs.GetUserNums(job).Any(x=>x!=userFilter.UserNum))//unassigned selected and job is no longer unassigned
					|| (userFilter!=null && !Jobs.GetUserNums(job).Contains(userFilter.UserNum))) //job no longer associated with user.
				{
					gridMyJobs.Rows.RemoveAt(i);
				}
				else {
					gridMyJobs.Rows[i]=GetMyJobRow(job,userFilter);
				}
				gridMyJobs.Refresh();
				return;
			}
			if(job==null //job deleted 
				|| (userFilter!=null && userFilter.UserNum==0 && Jobs.GetUserNums(job).Any(x => x!=userFilter.UserNum))//unassigned selected and job is no longer unassigned
				|| (userFilter!=null && !Jobs.GetUserNums(job).Contains(userFilter.UserNum))) //job no longer associated with user.
			{
				return;
			}
			gridMyJobs.BeginUpdate();
			gridMyJobs.Rows.Add(GetMyJobRow(job,userFilter));//job newly associated with user.
			gridMyJobs.EndUpdate();
		}

		///<summary>userod is used for highlighting.</summary>
		private ODGridRow GetMyJobRow(Job job,Userod userod) {
			long userNumHighlight = -1;
			if(userod!=null) {
				userNumHighlight=userod.UserNum;
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(job.JobNum.ToString());
			row.Cells.Add(job.Category.ToString());
			row.Cells.Add(job.PhaseCur.ToString());
			row.Cells.Add(new ODGridCell(job.Priority.ToString()) { CellColor=statusColors[(int)job.Priority] });
			Color cellColor = Color.Empty;
			if(!string.IsNullOrWhiteSpace(textSearch.Text) && job.ToString().ToLower().Contains(textSearch.Text.ToLower())) {
				cellColor=Color.FromArgb(255,255,230);//light yellow for search match.
			}
			row.Cells.Add(new ODGridCell(job.Title.Left(50,true)) { CellColor=cellColor });//first 50 characters.
			Color cLightGreen = Color.FromArgb(236,255,236);//light green
			long cellLong = job.OwnerNum;
			if(job.UserNumCheckout==0) {
				row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
				row.Cells.Add(new ODGridCell(job.OwnerAction.GetDescription()) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
				row.Cells.Add(new ODGridCell(job.IsApprovalNeeded ? "X" : "") { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			}
			else {
				cellLong = job.UserNumCheckout;
				row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=Color.FromArgb(254,235,233) });
				row.Cells.Add(new ODGridCell("Job Checked Out") { CellColor= Color.FromArgb(254,235,233) });
				row.Cells.Add(new ODGridCell("") { CellColor = Color.FromArgb(254,235,233) });
			}
			cellLong = job.UserNumConcept;
			row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			cellLong = job.UserNumExpert;
			row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			cellLong = job.UserNumEngineer;
			row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			string reviewers = "";
			List<long> reviewerNums = job.ListJobReviews.FindAll(x => x.ReviewerNum>0).Select(x => x.ReviewerNum).Distinct().ToList();
			if(reviewerNums.Count>0) {
				reviewers=Userods.GetName(reviewerNums[0]);
			}
			if(reviewerNums.Count>1) {
				reviewers+=string.Format("(+{0})",reviewerNums.Count-1);
			}
			row.Cells.Add(new ODGridCell(reviewers) { CellColor=(reviewerNums.Contains(userNumHighlight) ? cLightGreen : Color.Empty) });
			cellLong = job.UserNumDocumenter;
			row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			cellLong = job.UserNumCustContact;
			row.Cells.Add(new ODGridCell(Userods.GetName(cellLong)) { CellColor=(cellLong==userNumHighlight ? cLightGreen : Color.Empty) });
			string watchers = "";
			List<long> watcherNums = job.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Watcher).Select(x => x.FKey).Distinct().ToList();
			if(watcherNums.Count>0) {
				watchers=Userods.GetName(watcherNums[0]);
			}
			if(watcherNums.Count>1) {
				watchers+=string.Format("(+{0})",watcherNums.Count-1);
			}
			row.Cells.Add(new ODGridCell(watchers) { CellColor=(watcherNums.Contains(userNumHighlight) ? cLightGreen : Color.Empty) });
			if(checkShowVersion.Checked) {
				row.Cells.Add(job.JobVersion.Replace(";","\r\n").Trim());
			}
			row.Tag=job;
			return row;
		}

		private void FillGridWorkSummary() {
			//GridWorkSummary should be a pivoted version of the ACtion Items grid. 
			gridWorkSummary.BeginUpdate();
			gridWorkSummary.Columns.Clear();
			gridWorkSummary.Columns.Add(new ODGridColumn("User\r\nName",100));
			JobAction[] actions = new[] {
				//JobAction.AnswerQuestion,
				//JobAction.ApproveConcept,
				JobAction.WriteConcept,
				//JobAction.ApproveJob,
				//JobAction.AssignWriteup,
				JobAction.WriteJob,
				//JobAction.ApproveChanges,
				//JobAction.AssignExpert,
				//JobAction.AssignEngineer,
				JobAction.ReviewCode,
				JobAction.WriteCode,
				//JobAction.TakeJob,
				JobAction.Document,
				JobAction.Advise,
				JobAction.WaitForApproval,
				JobAction.WaitForReview
			};
			foreach(JobAction action in actions) {
				gridWorkSummary.Columns.Add(new ODGridColumn(action.GetDescription().Replace(" ","\r\n"),60) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			}
			//gridWorkSummary.Columns.Add(new ODGridColumn("Info Request",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			//gridWorkSummary.Columns.Add(new ODGridColumn("Write\r\nConcept",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			//gridWorkSummary.Columns.Add(new ODGridColumn("Write\r\nJob",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			//gridWorkSummary.Columns.Add(new ODGridColumn("Advise\r\nAs Expert",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			//gridWorkSummary.Columns.Add(new ODGridColumn("Write\r\nCode",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			//gridWorkSummary.Columns.Add(new ODGridColumn("Est Hours\r\n(Write Code)",85) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			gridWorkSummary.Rows.Clear();
			ODGridRow row;
			List<long> userNums = _listJobPermissionsAll.Select(x => x.UserNum).Distinct().ToList();
			List<Userod> listUsers = UserodC.GetListt().FindAll(x => userNums.Contains(x.UserNum)).OrderBy(x => x.UserName).ToList();
			listUsers.Add(new Userod() { UserNum=0,UserName="Unassigned" });
			foreach(Userod user in listUsers) {
				row=new ODGridRow();
				Dictionary<JobAction,List<Job>> dictActions = actions.ToDictionary(x => x,x => new List<Job>());
				foreach(Job job in _listJobsAll) {
					JobAction action = job.ActionForUser(user);
					if(!dictActions.ContainsKey(action)) {
						continue;
					}
					if(job.OwnerNum==user.UserNum 
						|| action==JobAction.Advise
						|| action==JobAction.Document
						|| action==JobAction.WaitForApproval
						|| action==JobAction.WaitForReview) 
					{
						dictActions[action].Add(job);
					}
				}
				row.Cells.Add(user.UserName);
				foreach(KeyValuePair<JobAction,List<Job>> kvp in dictActions) {
					int count = kvp.Value.FindAll(x => x.Priority!=JobPriority.OnHold).Count;
					if(count==0) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(count.ToString());
					}
				}
				row.Tag=user;
				gridWorkSummary.Rows.Add(row);
			}
			gridWorkSummary.EndUpdate();
		}

		#region Tree Control and Filtering

		///<summary>listJobsAll must already be updated.</summary>
		private void FilterAndFill() {
			string filter = textSearch.Text.ToLower();
			_listJobsFiltered=_listJobsAll.Select(x => x.Copy()).ToList();
			if(GroupJobsBy.MyHeirarchy==(GroupJobsBy)comboGroup.SelectedIndex) {
				_listJobsFiltered.RemoveAll(x => !Jobs.GetUserNums(x).Contains(Security.CurUser.UserNum));
			}
			_listJobNumsHighlight=new List<long>();
			if(!checkComplete.Checked) {
				_listJobsFiltered.RemoveAll(x => new[] { JobPhase.Complete,JobPhase.Cancelled }.Contains(x.PhaseCur) && x.OwnerAction!=JobAction.ContactCustomer);
			}
			if(comboCategorySearch.SelectedIndex>0) {
				JobCategory cat = (JobCategory)(comboCategorySearch.SelectedIndex-1);
				_listJobsFiltered.RemoveAll(x => x.Category!=cat);
			}
			if(!string.IsNullOrWhiteSpace(filter)) {
				List<Job> matches = _listJobsFiltered.FindAll(x => x.Title.ToLower().Contains(filter)||x.JobNum.ToString().Contains(filter));
				_listJobNumsHighlight=matches.Select(x => x.JobNum).ToList();
				//if(!checkHighlight.Checked) {//not highlight only, actually filter results.
				_listJobsFiltered=matches.Select(x => x.Copy()).ToList();
				 //}//end if !CheckHighlight
			}//end if filtering results
			if(new[] { GroupJobsBy.Heirarchy,GroupJobsBy.MyHeirarchy }.Contains((GroupJobsBy)comboGroup.SelectedIndex)) {//find parent if we are in heirarchy view
				List<Job> parentJobs;
				do {//This loop finds the parents of orphaned nodes so that when searching you can see results in context.
					long[] jobs, parents;
					jobs=_listJobsFiltered.Select(x => x.JobNum).ToArray();
					parents=_listJobsFiltered.Select(x => x.ParentNum).Distinct().ToArray();
					parentJobs=_listJobsAll.FindAll(x => !jobs.Contains(x.JobNum) && parents.Contains(x.JobNum));
					_listJobsFiltered.AddRange(parentJobs);
				} while(parentJobs.Count>0);
			}//end heirarchy do/while
			FillTree();
		}
		
		private void FillTree() {
			treeJobs.BeginUpdate();
			treeJobs.Nodes.Clear();
			switch((GroupJobsBy)comboGroup.SelectedIndex) {
				case GroupJobsBy.None:
					foreach(Job job in _listJobsFiltered) {//Add top level nodes.
						treeJobs.Nodes.Add(new TreeNode(job.ToString()) {
							Tag=job,
							BackColor=(_listJobNumsHighlight.Contains(job.JobNum)? Color.FromArgb(255,255,230) : Color.White)
						});
					}
					break;
				case GroupJobsBy.MyHeirarchy:
				case GroupJobsBy.Heirarchy:
					foreach(Job job in _listJobsFiltered.Where(x=>x.ParentNum==0)) {//Add top level nodes.
						TreeNode node=GetNodeHeirarchyFiltered(job);//get child nodes for each top level node.
						treeJobs.Nodes.Add(node);
					}
					break;
				case GroupJobsBy.Status:
					foreach(JobPhase status in Enum.GetValues(typeof(JobPhase))) {//Add top level nodes.
						TreeNode node=new TreeNode(status.ToString()) { Tag=status };//get child nodes for each top level node.
						foreach(Job job in _listJobsFiltered.Where(x=>x.PhaseCur==status)) {
							TreeNode child=new TreeNode(job.ToString()) { Tag=job };
							if(_listJobNumsHighlight.Contains(job.JobNum)) {
								child.BackColor=Color.FromArgb(255,255,230);
							}
							node.Nodes.Add(child);
						}
						treeJobs.Nodes.Add(node);
					}
					break;
				//case GroupJobsBy.MyJobs:
				case GroupJobsBy.User:
					List<Userod> listUsers;
					//if(UserFilter!=null) {
					//	listUsers=new List<Userod>() {
					//		UserFilter
					//	};
					//}
					//else{
					List<long> userNums=_listJobPermissionsAll.Select(x=>x.UserNum).Distinct().ToList();//show users with job permissions
					userNums=userNums.Union(_listJobsFiltered.SelectMany(x=>Jobs.GetUserNums(x,true))).ToList();//show users with jobs
					listUsers=UserodC.GetListt().FindAll(x=>userNums.Contains(x.UserNum)).OrderBy(x=>x.UserName).ToList();
					listUsers.Add(new Userod() {UserName="Un-Assigned"});
					//}
					foreach(Userod user in listUsers){//UserodC.Listt.FindAll(z=>_listJobsFiltered.SelectMany(x => new[] { x.Expert,x.Owner }.Union(_listJobLinksUsers.Select(y => y.FKey))).Distinct().Contains(z.UserNum)).OrderBy(x=>x.UserName)) {
						TreeNode node=new TreeNode(user.UserName) {Tag=user};
						TreeNode nodeChild=null;
						nodeChild=CreateNodeByStatus("Expert",_listJobsFiltered.FindAll(x=>x.UserNumExpert==user.UserNum));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Engineer",_listJobsFiltered.FindAll(x=>x.UserNumEngineer==user.UserNum));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Watching",_listJobsFiltered.FindAll(x => x.ListJobLinks.Any(y => y.LinkType==JobLinkType.Watcher && y.FKey==user.UserNum)));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Reviews",_listJobsFiltered.FindAll(x=>x.ListJobReviews.Any(y=>y.ReviewerNum==user.UserNum)));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						if(node.Nodes.Count>0) {
							treeJobs.Nodes.Add(node);
						}
					}
					break;
				case GroupJobsBy.Owner:
					List<long> expOwnNums;
					expOwnNums=_listJobsFiltered.Select(x => x.OwnerNum).ToList();
					listUsers=UserodC.GetListt().FindAll(x => expOwnNums.Contains(x.UserNum)).OrderBy(x => x.UserName).ToList();
					listUsers.Add(new Userod() {UserName="Unassigned"});
					foreach(Userod user in listUsers) {//Add top level nodes.
						TreeNode node=new TreeNode(user.UserName) { Tag=user };//get child nodes for each top level node.
						node=CreateNodeByStatus(user.UserName,_listJobsFiltered.Where(x=>user.UserNum==x.OwnerNum).ToList());
						if(node!=null) {
							node.Tag=user;
							treeJobs.Nodes.Add(node);
						}
					}
					break;
			}
			treeJobs.EndUpdate();
			if(checkCollapse.Checked) {
				treeJobs.CollapseAll();
			}
			else {
				treeJobs.ExpandAll();
			}
		}

		///<summary>Returns a single node with the given name, and adds all jobs to the node with a status node in between. Returns null if no jobs in list.</summary>
		private TreeNode CreateNodeByStatus(string NodeName,List<Job> listJobs) {
			if(listJobs==null || listJobs.Count==0) {
				return null;
			}
			TreeNode node=new TreeNode(NodeName);
			foreach(JobPhase status in Enum.GetValues(typeof(JobPhase)).Cast<JobPhase>().ToList()) {
				TreeNode nodeStatus=new TreeNode(status.ToString());
				listJobs.FindAll(x=>x.PhaseCur==status).ForEach(x=>nodeStatus.Nodes.Add(new TreeNode(x.ToString()) {
					Tag=x,
					BackColor=(_listJobNumsHighlight.Contains(x.JobNum)? Color.FromArgb(255,255,230) : Color.White)
				}));
				if(nodeStatus.Nodes==null || nodeStatus.Nodes.Count==0) {
					continue;
				}
				node.Nodes.Add(nodeStatus);
			}
			if(node.Nodes==null || node.Nodes.Count==0) {
				return null;
			}
			return node;
		}

		///<summary></summary>
		private TreeNode GetNodeHeirarchyFiltered(Job job) {
			TreeNode[] children=_listJobsFiltered.FindAll(x => x.ParentNum==job.JobNum).Select(GetNodeHeirarchyFiltered).ToArray();//can be enhanced by removing matches from the search set.
			TreeNode node=new TreeNode(job.ToString()) { Tag=job };
			if(children.Length>0) {
				node.Nodes.AddRange(children);
			}
			if(_listJobNumsHighlight.Contains(job.JobNum)) {
				node.BackColor=Color.FromArgb(255,255,230);
			}
			return node;
		}
		///<summary></summary>
		private TreeNode GetNodeHeirarchyAll(Job job) {
			TreeNode[] children = _listJobsAll.FindAll(x => x.ParentNum==job.JobNum).Select(GetNodeHeirarchyAll).ToArray();//can be enhanced by removing matches from the search set.
			TreeNode node = new TreeNode(job.ToString()) { Tag=job };
			if(children.Length>0) {
				node.Nodes.AddRange(children);
			}
			return node;
		}

		///<summary>Similar to GetNodeHeirarchy, but used to build tree to be passed to job control.</summary>
		private TreeNode GetJobTree(Job job) {
			List<Job> jobHeirarchy = new List<Job> { job };
			for(int i = 0;i<jobHeirarchy.Count;i++) {
				Job j = _listJobsAll.FirstOrDefault(x => x.JobNum==jobHeirarchy[i].ParentNum);
				if(j==null) {
					break;
				}
				jobHeirarchy.Add(j);
			}
			return GetNodeHeirarchyAll(jobHeirarchy.Last());
		}

		///<summary>Check for heirarchical loops when moving a child job to a parent job. Returns true if loop is found. Example A>B>C>A would be a loop.</summary>
		private bool IsJobLoop(Job jobChild,long jobNumParent) {
			List<long> lineage=new List<long>(){jobChild.JobNum};
			Job jobCur=jobChild.Copy();
			jobCur.ParentNum=jobNumParent;
			while(jobCur.ParentNum!=0){
				if(lineage.Contains(jobCur.ParentNum)) {
					MessageBox.Show(this,"Invalid heirarchy detected. Moving the job there would create an infinite loop.");
					return true;//loop found
				}
				Job jobNext=_listJobsAll.FirstOrDefault(x=>x.JobNum==jobCur.ParentNum);
				if(jobNext==null) {
					MessageBox.Show(this,"Invalid heirarchy detected. Cannot find job "+jobCur.ParentNum);
					return true;
				}
				jobCur=jobNext;
				lineage.Add(jobCur.JobNum);
			} 
			return false;//no loop detected
		}

		private void treeJobs_NodeMouseClick(object sender,TreeNodeMouseClickEventArgs e) {
			if(JobUnsavedChangesCheck()) {
				return;
			}
			Job job=null;
			if(e.Node!=null && (e.Node.Tag is Job)) {
				job=(Job)e.Node.Tag;
			}
			userControlJobEdit.LoadJob(job,GetJobTree(job));
		}

		private void treeJobs_ItemDrag(object sender,ItemDragEventArgs e) {
			treeJobs.SelectedNode=(TreeNode)e.Item;
			DoDragDrop(e.Item,DragDropEffects.Move);
		}

		private void treeJobs_DragEnter(object sender,DragEventArgs e) {
			e.Effect=DragDropEffects.Move;
		}

		private void treeJobs_DragDrop(object sender,DragEventArgs e) {
			if(grayNode!=null) {
				grayNode.BackColor=Color.White;
			}
			if(comboGroup.SelectedIndex!=(int)GroupJobsBy.Heirarchy) {
				return;//drag and drop only applies to heirarchy view.
			}
			if(userControlJobEdit.IsChanged) {
				MessageBox.Show("You must save changes to current job before making drag and drop changes.");
				return;
			}
			if(!e.Data.GetDataPresent("System.Windows.Forms.TreeNode",false)) { 
				return; 
			}
			Point pt=((TreeView)sender).PointToClient(new Point(e.X,e.Y));
			TreeNode destinationNode=((TreeView)sender).GetNodeAt(pt);
			TreeNode sourceNode=(TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if(!(sourceNode.Tag is Job)) {//only allow move is source node was a job.
				return;//might have to set some additional variable instead of just returning.
			}
			Job j1=(Job)sourceNode.Tag;
			if(!_isOverride
				&& j1.UserNumEngineer!=Security.CurUser.UserNum
				&& j1.UserNumExpert!=Security.CurUser.UserNum
				&& !JobPermissions.IsAuthorized(JobPerm.Approval,true)
				&& !JobPermissions.IsAuthorized(JobPerm.FeatureManager)) 
			{
				return;//only expert, engineer, Approver, FeatureManager, or override can drag and drop.
			}
			if(!TryMoveJobtoJob(j1,destinationNode)) {
				return;
			}
			if(sourceNode.Parent==null) {
				treeJobs.Nodes.Remove(sourceNode);
			}
			else {
				sourceNode.Parent.Nodes.Remove(sourceNode);
			}
			if(destinationNode!=null) {
				destinationNode.Nodes.Add(sourceNode);
			}
			else {
				treeJobs.Nodes.Add(sourceNode);
			}
			//Can be improved, this updates in memory list.
			Job temp=_listJobsAll.FirstOrDefault(x => x.JobNum==j1.JobNum);
			if(temp!=null) {//should never be null
				temp.ParentNum=j1.ParentNum; //update in memory list.
				temp.UserNumEngineer=j1.UserNumEngineer; //update in memory list.
				temp.UserNumExpert=j1.UserNumExpert; //update in memory list.
			}
			FilterAndFill();//this is annoying and can be improved, but reflects the proper changes. tree will expand or collapse based on collapse all check.
		}

		private bool TryMoveJobtoJob(Job j1,TreeNode destinationNode) {
			if(destinationNode==null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Move selected job to top level?")) {
					return false;
				}
				j1.ParentNum=0;
			}
			else if(destinationNode.Tag is Job) {
				Job j2=(Job)destinationNode.Tag;
				if(j1.JobNum==j2.JobNum) {
					return false;
				}
				if(IsJobLoop(j1,j2.JobNum)) {
					return false;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Move selected job?")) {
					return false;
				}
				j1.ParentNum=j2.JobNum;
			}
			else {
				return false;//no valid target
			}
			Jobs.Update(j1);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,j1.JobNum);
			return true;
		}

		// Make sure you have the correct using clause to see DllImport:
		// using System.Runtime.InteropServices;
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd,int wMsg,int wParam,int lParam);

		private TreeNode grayNode=null;//only used in treeJobs_DragOver to reduce flickering.
		private void treeJobs_DragOver(object sender,DragEventArgs e) {
			Point p=treeJobs.PointToClient(new Point(e.X,e.Y));
			TreeNode node=treeJobs.GetNodeAt(p.X,p.Y);
			if(grayNode!=null && grayNode!=node) {
				grayNode.BackColor=Color.White;
				grayNode=null;
			}
			if(node!=null && node.BackColor!=Color.LightGray) {
				node.BackColor=Color.LightGray;
				grayNode=node;
			}
			if(p.Y<25) {
				SendMessage(treeJobs.Handle,277,0,0);//Scroll Up
			}
			else if(p.Y>treeJobs.Height-25) {
				SendMessage(treeJobs.Handle,277,1,0);//Scroll down.
			}
		}

		private void comboGroup_SelectionChangeCommitted(object sender,EventArgs e) {
			checkCollapse.Checked=true;
			FilterAndFill();
		}

		private void checkCollapse_CheckedChanged(object sender,EventArgs e) {
			if(checkCollapse.Checked) {
				treeJobs.CollapseAll();
			}
			else {
				treeJobs.ExpandAll();
			}
		}

		private void checkComplete_CheckedChanged(object sender,EventArgs e) {
			FilterAndFill();
		}

		#endregion

		private void gridWorkSummary_CellClick(object sender,ODGridClickEventArgs e) {
			if(!(gridWorkSummary.Rows[e.Row].Tag is Userod)) {
				return;
			}
			int idx = UserodC.ShortList.FindIndex(x => x.UserNum==((Userod)gridWorkSummary.Rows[e.Row].Tag).UserNum)+2;//"accidentally" works for unassigned as well
			comboUser.SelectedIndex=idx;
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
			FillGridActions();
			FillGridMyJobs();
		}

		private void butAddJob_Click(object sender,EventArgs e) {
			FormJobNew FormJN=new FormJobNew();
			FormJN.ShowDialog();
			if(FormJN.DialogResult!=DialogResult.OK) {
				return;
			}
			_listJobsAll.Add(FormJN.JobCur);
			FilterAndFill();
		}

		private void butOverride_Click(object sender,EventArgs e) {
			_isOverride=true;
			userControlJobEdit.IsOverride=true;
		}

		private void userControlJobEdit_JobOverride(object sender,bool isOverride) {
			_isOverride=isOverride;
		}

		private void userControlJobEdit_SaveClick(object sender,EventArgs e) {
			Job jobNew=userControlJobEdit.GetJob();
			Job jobStale=_listJobsAll.FirstOrDefault(x=>x.JobNum==jobNew.JobNum);
			if(jobStale==null) {
				_listJobsAll.Add(jobNew);
			}
			else {
				_listJobsAll[_listJobsAll.IndexOf(jobStale)]=jobNew;
			}
			UpdateNodes(jobNew);
			FillGridActions();
		}

		///<summary>Flat recursion. Updates any nodes displaying outdated information for the passed in job (identified by JobNum). Does not move nodes in tree, just updates job information.</summary>
		private void UpdateNodes(Job jobNew) {
			List<TreeNode> treeNodes=new List<TreeNode>(treeJobs.Nodes.Cast<TreeNode>());
			for(int i=0;i<treeNodes.Count;i++) {//flat recursion
				TreeNode nodeCur=treeNodes[i];
				if((nodeCur.Tag is Job) && ((Job)nodeCur.Tag).JobNum==jobNew.JobNum) {
					nodeCur.Text=jobNew.ToString();//update label if Title has changed.
					nodeCur.Tag=jobNew;
				}
				treeNodes.AddRange(nodeCur.Nodes.Cast<TreeNode>());
			}
		}

		public void GoToJob(long jobNum) {
			Job job=Jobs.GetOneFilled(jobNum);
			if(job==null) {
				MessageBox.Show("Job not found.");
				return;
			}
			if(JobUnsavedChangesCheck()) {
				return;//there ARE unsaved changes that the user decided not to save.
			}
			//If launching from task, and job manager is not yet open, then GetJobTree will return an empty tree
			//Wait until the data set is filled to continue.
			int loopCount = 0;
			while(_listJobsAll.Count==0 && loopCount<20) {
				System.Threading.Thread.Sleep(100);
				loopCount++;//in case the DB is empty or there is an error for some reason. do not wait more than 2 seconds. this is an arbitrary length of time.
			}
			userControlJobEdit.LoadJob(job,GetJobTree(job));
		}

		private void userControlJobEdit_RequestJob(object sender,long jobNum) {
			Job job = _listJobsAll.FirstOrDefault(x=>x.JobNum==jobNum);
			if(job==null) {
				MessageBox.Show("Job not found.");//shouldn't happen if everything is working properly.
				return;
			}
			if(JobUnsavedChangesCheck()) {
				return;//there ARE unsaved changes that the user decided not to save.
			}
			userControlJobEdit.LoadJob(job,GetJobTree(job));
		}

		///<summary>For UI only. Never saved to DB.</summary>
		private enum GroupJobsBy {
			None,
			MyHeirarchy,
			Heirarchy,
			User,
			Status,
			Owner
		}

		private void butSearch_Click(object sender,EventArgs e) {
			FormJobSearch FormJS=new FormJobSearch();
			FormJS.InitialSearchString=textSearch.Text;
			//pass in data here to reduce calls to DB.
			FormJS.ShowDialog();
			if(FormJS.DialogResult!=DialogResult.OK) {
				return;
			}
			comboGroup.SelectedIndex=(int)GroupJobsBy.None;
			checkCollapse.Checked=false;
			//checkMine.Checked=false;
			checkComplete.Checked=true;
			checkResults.Checked=true;//sets control visibility as well.
			tabControlNav.SelectedIndex=1;//tree view to see search results.
			_listJobsFiltered=FormJS.GetSearchResults();
			FillTree();
			if(JobUnsavedChangesCheck()) {
				return;
			}
			userControlJobEdit.LoadJob(FormJS.SelectedJob,GetJobTree(FormJS.SelectedJob));//can be null
		}

		private void comboCategorySearch_SelectedIndexChanged(object sender,EventArgs e) {
			FilterAndFill();
		}

		private void textSearchAction_TextChanged(object sender,EventArgs e) {
			FillGridActions();
			FillGridMyJobs();
			FilterAndFill();
		}

		private void checkShowComplete_CheckedChanged(object sender,EventArgs e) {
			FillGridMyJobs();
		}

		private void checkShowVersion_CheckedChanged(object sender,EventArgs e) {
			FillGridMyJobs();
		}

		private void butMe_Click(object sender,EventArgs e) {
			int idx = UserodC.ShortList.FindIndex(x => x.UserNum==Security.CurUser.UserNum);
			if(idx>-1) {
				idx+=2;
			}
			comboUser.SelectedIndex=idx;
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
			FillGridActions();
			FillGridMyJobs();
		}

		///<summary>This is a temporary solution. Once the Job Manager is programmed to use signals to refresh content dynamically this should be removed.</summary>
		private void butRefresh_Click(object sender,EventArgs e) {
			if(JobUnsavedChangesCheck()) {
				return;
			}
			RefreshAndFillThreaded();
		}

		private void gridMyJobs_CellClick(object sender,ODGridClickEventArgs e) {
			if(!(gridMyJobs.Rows[e.Row].Tag is Job)) {
				return;
			}
			if(JobUnsavedChangesCheck()) {
				return;
			}
			Job job = (Job)gridMyJobs.Rows[e.Row].Tag;
			userControlJobEdit.LoadJob(job,GetJobTree(job));
			FillGridActions();
		}


		public void ProcessSignals(List<Signalod> listSignals) {
			if(listSignals.All(x => !x.ITypes.Contains(((int)InvalidType.Jobs).ToString()))) {
				//no job signals;
				return;
			}
			//Get the latest jobs that have been updated by the signal.
			//Initialized to <jobNum,null>
			Dictionary<long,Job> dictNewJobs= listSignals.FindAll(x => x.ITypes.Contains(((int)InvalidType.Jobs).ToString()) && x.FKeyType==KeyType.Job).Select(x => x.FKey).Distinct().ToDictionary(x=>x,x=>(Job)null);
			List<Job> newJobs = Jobs.GetMany(dictNewJobs.Keys.ToList());
			Jobs.FillInMemoryLists(newJobs);
			newJobs.ForEach(x => dictNewJobs[x.JobNum]=x);
			//Current job loaded in UserControlJobEdit (Should now be prevented by job checkout.)
			Job jobCur = null;//job currently loaded into the UserControlJobEdit AND included in a signal.
			if(userControlJobEdit.GetJob()!=null) {//get job curently loaded
				jobCur=newJobs.FirstOrDefault(x => x.JobNum==userControlJobEdit.GetJob().JobNum);
			}
			if(jobCur!=null) {//someone updated the currently loaded job.
				userControlJobEdit.LoadMergeJob(jobCur);
			}
			//Update in memory lists.
			foreach(KeyValuePair<long,Job> kvp in dictNewJobs) {
				if(kvp.Value==null) {//deleted job
					_listJobsAll.RemoveAll(x => x.JobNum==kvp.Key);
					_listJobsFiltered.RemoveAll(x => x.JobNum==kvp.Key);
					List<TreeNode> treeNodes = new List<TreeNode>(treeJobs.Nodes.Cast<TreeNode>());
					for(int i = 0;i<treeNodes.Count;i++) {//flat recursion
						TreeNode nodeCur = treeNodes[i];
						if((nodeCur.Tag is Job) && ((Job)nodeCur.Tag).JobNum==kvp.Key) {
							nodeCur.Text="(Deleted) - "+nodeCur.Text;//update label text to indicate deleted.
							nodeCur.Tag=null;
						}
						treeNodes.AddRange(nodeCur.Nodes.Cast<TreeNode>());
					}
					continue;
				}
				//Master Job List
				Job jobOld=_listJobsAll.FirstOrDefault(x => x.JobNum==kvp.Key);
				if(jobOld==null) {//new job entirely, no need to update anything in memory, just add to jobs list.
					_listJobsAll.Add(kvp.Value);
					continue;
				}
				_listJobsAll[_listJobsAll.IndexOf(jobOld)]=kvp.Value;
				//Filtered Job List
				jobOld=_listJobsFiltered.FirstOrDefault(x => x.JobNum==kvp.Key);
				if(jobOld!=null) {//update item in filtered list.
					_listJobsFiltered[_listJobsFiltered.IndexOf(jobOld)]=kvp.Value;
				}
				//Jobs in tree
				UpdateNodes(kvp.Value);
			}
			FillGridActions();
			foreach(KeyValuePair<long,Job> kvp in dictNewJobs) {
				UpdateGridMyJobs(kvp.Key,kvp.Value);
			}
			FillGridWorkSummary();
		}

		private void gridAction_CellClick(object sender,ODGridClickEventArgs e) {
			if(JobUnsavedChangesCheck()) {
				return;
			}
			if(!(gridAction.Rows[e.Row].Tag is Job)) {
				return;
			}
			Job selectedjob = (Job)gridAction.Rows[e.Row].Tag;
			userControlJobEdit.LoadJob(selectedjob,GetJobTree(selectedjob));
			int idx=gridMyJobs.Rows.Cast<ODGridRow>().ToList().FindIndex(x => (x.Tag is Job) && ((Job)x.Tag).JobNum==selectedjob.JobNum);
			if(idx>-1) {
				gridMyJobs.SetSelected(idx,true);
			}
			else {
				gridMyJobs.SetSelected(false);
			}
		}

		private void gridAction_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridAction.Rows[e.Row].Tag is Job)) {
				return;
			}
			tabControlMain.SelectedIndex=0;
		}

		private void gridMyJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridMyJobs.Rows[e.Row].Tag is Job)) {
				return;//should never happen.
			}
			tabControlMain.SelectedIndex=0;
		}

		private void comboUser_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridActions();
			FillGridMyJobs();
			this.Text="Job Manager"+(comboUser.Text=="" ? "" : " - "+comboUser.Text);
		}

		private bool JobUnsavedChangesCheck() {
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						userControlJobEdit.ForceSave();
						break;
					case System.Windows.Forms.DialogResult.No:
						CheckinJob();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return true;
				}
			}
			return false;//no unsaved changes
		}

		private void CheckinJob() {
			Job jobCur = userControlJobEdit.GetJob();
			if(jobCur==null) {
				return;
			}
			if(jobCur.UserNumCheckout==Security.CurUser.UserNum) {
				jobCur= Jobs.GetOne(jobCur.JobNum);
				jobCur.UserNumCheckout=0;
				Jobs.Update(jobCur);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobCur.JobNum);
			}
		}

		private void checkResults_CheckedChanged(object sender,EventArgs e) {
			if(!checkResults.Checked) {
				//Unfilter results if we unchecked it.
				_listJobsFiltered=_listJobsAll.Select(x=>x.Copy()).ToList();
				FillTree();
			}
			//visible==Checked
			checkResults.Visible=checkResults.Checked;
			//visible==!Checked
			comboCategorySearch.Visible=!checkResults.Checked;
			comboGroup.Visible=!checkResults.Checked;
			checkCollapse.Visible=!checkResults.Checked;
			checkComplete.Visible=!checkResults.Checked;
			labelCategory.Visible=!checkResults.Checked;
			labelGroupBy.Visible=!checkResults.Checked;
		}

		private void FormJobManager2_FormClosing(object sender,FormClosingEventArgs e) {
			if(JobUnsavedChangesCheck()) {
				e.Cancel=true;
				return;
			}
		}

	}
}
