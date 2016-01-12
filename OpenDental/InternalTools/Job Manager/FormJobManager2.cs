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
		///<summary>Used to filter the tree by this user.</summary>
		private Userod _userFilter;

		private Userod UserFilter {
			get {return _userFilter;}
			set { 
				_userFilter=value;
				this.Text="Job Manager"+(value==null?"":" - "+value.UserName);
			}
		}
		
		public FormJobManager2() {
			InitializeComponent();
		}

		private void FormJobManager_Load(object sender,EventArgs e) {
			UserFilter=Security.CurUser;
			RefreshAndFillThreaded();
			comboCategorySearch.Items.Add("All");
			Enum.GetNames(typeof(JobCategory)).ToList().ForEach(x => comboCategorySearch.Items.Add(x));
			comboCategorySearch.SelectedIndex=0;
			Enum.GetNames(typeof(GroupJobsBy)).ToList().ForEach(x => comboGroup.Items.Add(x));
			comboGroup.SelectedIndex=(int)GroupJobsBy.User;
			if(!JobPermissions.IsAuthorized(JobPerm.Override,true)) {
				butOverride.Visible=false;
			}
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
				butAddJob.Enabled=false;
			}
			//FillTree();//Invoked above.
			//FillWorkSummary();//Invoked above.
			Signalods.Subscribe(this);
		}

		///<summary>Fills all in memory data from the DB on a seperate thread and then refills controls.</summary>
		private void RefreshAndFillThreaded() {
			ODThread thread=new ODThread((o) => {
				_listJobsAll=Jobs.GetAll();
				Jobs.FillInMemoryLists(_listJobsAll);
				_listJobPermissionsAll=JobPermissions.GetList();
				_listJobsFiltered=_listJobsAll.Select(x => x.Copy()).ToList();
				this.Invoke((Action)FillTree);
				this.Invoke((Action)FillWorkSummary);
			});
			thread.AddExceptionHandler((ex) => {/*todo, or not todo*/});
			thread.Start();
		}

		private void FillWorkSummary() {
			gridWorkSummary.BeginUpdate();
			gridWorkSummary.Columns.Clear();
			gridWorkSummary.Columns.Add(new ODGridColumn("Worker",100));
			gridWorkSummary.Columns.Add(new ODGridColumn("Jobs As Expert",100) { SortingStrategy=GridSortingStrategy.AmountParse, TextAlign=HorizontalAlignment.Right });
			gridWorkSummary.Columns.Add(new ODGridColumn("Jobs As Owner",100) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			gridWorkSummary.Columns.Add(new ODGridColumn("Est Hours",100) { SortingStrategy=GridSortingStrategy.AmountParse,TextAlign=HorizontalAlignment.Right });
			gridWorkSummary.Rows.Clear();
			ODGridRow row;
			List<long> userNums=_listJobPermissionsAll.Select(x=>x.UserNum).Distinct().ToList();
			List<Userod> listUsers=UserodC.GetListt().FindAll(x => userNums.Contains(x.UserNum)).OrderBy(x => x.UserName).ToList();
			listUsers.Add(new Userod() {UserNum=0,UserName="Unassigned"});
			foreach(Userod user in listUsers){
				row=new ODGridRow();
				row.Cells.Add(user.UserName);
				row.Cells.Add(_listJobsAll.FindAll(x => x.ExpertNum==user.UserNum).Count.ToString());
				row.Cells.Add(_listJobsAll.FindAll(x => x.OwnerNum==user.UserNum).Count.ToString());
				row.Cells.Add(_listJobsAll.FindAll(x => x.OwnerNum==user.UserNum).Sum(x => x.HoursEstimate).ToString());
				row.Tag=user;
				gridWorkSummary.Rows.Add(row);
			}
			gridWorkSummary.EndUpdate();
		}

		#region Tree Control and Filtering

		///<summary>listJobsAll must already be updated. Pass in a in leui of using Security.CurUser.</summary>
		private void FilterAndFill() {
			string filter=textSearch.Text.ToLower();
			_listJobsFiltered=_listJobsAll.Select(x => x.Copy()).ToList();
			if(UserFilter!=null) {
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>
					x.ExpertNum==UserFilter.UserNum
						|| x.OwnerNum==UserFilter.UserNum
						|| x.ListJobLinks.Any(y => y.LinkType==JobLinkType.Watcher && y.FKey==UserFilter.UserNum)
						|| x.ListJobReviews.Any(y => y.ReviewerNum==UserFilter.UserNum));
			}
			_listJobNumsHighlight=new List<long>();
			if(!checkComplete.Checked) {
				_listJobsFiltered.RemoveAll(x=>new[] {JobStat.Complete,JobStat.Deleted,JobStat.Rescinded}.Contains(x.JobStatus));
			}
			if(comboCategorySearch.SelectedIndex>0) {
				JobCategory cat=(JobCategory)(comboCategorySearch.SelectedIndex-1);
				_listJobsFiltered.RemoveAll(x=>x.Category!=cat);
			}
			if(!string.IsNullOrWhiteSpace(filter)) {
				List<Job> matches = _listJobsFiltered.FindAll(x => x.Title.ToLower().Contains(filter)||x.JobNum.ToString().Contains(filter));
				_listJobNumsHighlight=matches.Select(x=>x.JobNum).ToList();
				if(!checkHighlight.Checked) {//not highlight only, actually filter results.
					_listJobsFiltered=matches.Select(x => x.Copy()).ToList();
					if(((GroupJobsBy)comboGroup.SelectedIndex)==GroupJobsBy.Heirarchy) {//find parent if we are in heirarchy view
						List<Job> parentJobs;
						do {//This loop finds the parents of orphaned nodes so that when searching you can see results in context.
							long[] jobs,parents;
							jobs=_listJobsFiltered.Select(x=>x.JobNum).ToArray();
							parents=_listJobsFiltered.Select(x=>x.ParentNum).Distinct().ToArray();
							parentJobs=_listJobsAll.FindAll(x=>!jobs.Contains(x.JobNum) && parents.Contains(x.JobNum));
							_listJobsFiltered.AddRange(parentJobs);
						} while(parentJobs.Count>0);
					}//end heirarchy do/while
				}//end if !CheckHighlight
			}//end if filtering results
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
							BackColor=(_listJobNumsHighlight.Contains(job.JobNum)?Color.Wheat:Color.White)
						});
					}
					break;
				case GroupJobsBy.Heirarchy:
					foreach(Job job in _listJobsFiltered.Where(x=>x.ParentNum==0)) {//Add top level nodes.
						TreeNode node=GetNodeHeirarchy(job);//get child nodes for each top level node.
						treeJobs.Nodes.Add(node);
					}
					break;
				case GroupJobsBy.Status:
					foreach(JobStat status in Enum.GetValues(typeof(JobStat))) {//Add top level nodes.
						TreeNode node=new TreeNode(status.ToString()) { Tag=status };//get child nodes for each top level node.
						foreach(Job job in _listJobsFiltered.Where(x=>x.JobStatus==status)) {
							TreeNode child=new TreeNode(job.ToString()) { Tag=job };
							if(_listJobNumsHighlight.Contains(job.JobNum)) {
								child.BackColor=Color.Wheat;
							}
							node.Nodes.Add(child);
						}
						treeJobs.Nodes.Add(node);
					}
					break;
				//case GroupJobsBy.MyJobs:
				case GroupJobsBy.User:
					List<Userod> listUsers;
					if(UserFilter!=null) {
						listUsers=new List<Userod>() {
							UserFilter
						};
					}
					else{
						List<long> userNums=_listJobPermissionsAll.Select(x=>x.UserNum).Distinct().ToList();//show users with job permissions
						userNums=userNums.Union(_listJobsFiltered.SelectMany(Jobs.GetUserNums)).ToList();//show users with jobs
						listUsers=UserodC.GetListt().FindAll(x=>userNums.Contains(x.UserNum)).OrderBy(x=>x.UserName).ToList();
						listUsers.Add(new Userod() {UserName="Un-Assigned"});
					}
					foreach(Userod user in listUsers){//UserodC.Listt.FindAll(z=>_listJobsFiltered.SelectMany(x => new[] { x.Expert,x.Owner }.Union(_listJobLinksUsers.Select(y => y.FKey))).Distinct().Contains(z.UserNum)).OrderBy(x=>x.UserName)) {
						TreeNode node=new TreeNode(user.UserName) {Tag=user};
						TreeNode nodeChild=null;
						nodeChild=CreateNodeByStatus("Expert",_listJobsFiltered.FindAll(x=>x.ExpertNum==user.UserNum));
						if(nodeChild!=null){
							node.Nodes.Add(nodeChild);
						}
						nodeChild=CreateNodeByStatus("Owner",_listJobsFiltered.FindAll(x=>x.OwnerNum==user.UserNum));
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
						if(node.Nodes.Count>0 || UserFilter!=null) {//add user node even if there are no child nodes.
							treeJobs.Nodes.Add(node);
						}
					}
					break;
				case GroupJobsBy.Expert:
				case GroupJobsBy.Owner:
					List<long> expOwnNums;
					if(((GroupJobsBy)comboGroup.SelectedIndex)==GroupJobsBy.Expert) {
						expOwnNums=_listJobsFiltered.Select(x=>x.ExpertNum).ToList();
					}
					else {
						expOwnNums=_listJobsFiltered.Select(x => x.OwnerNum).ToList();
					}
					listUsers=UserodC.GetListt().FindAll(x => expOwnNums.Contains(x.UserNum)).OrderBy(x => x.UserName).ToList();
					listUsers.Add(new Userod() {UserName="Unassigned"});
					foreach(Userod user in listUsers) {//Add top level nodes.
						TreeNode node=new TreeNode(user.UserName) { Tag=user };//get child nodes for each top level node.
						node=CreateNodeByStatus(user.UserName,_listJobsFiltered.Where(x=>user.UserNum==(((GroupJobsBy)comboGroup.SelectedIndex)==GroupJobsBy.Expert?x.ExpertNum:x.OwnerNum)).ToList());
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
			foreach(JobStat status in Enum.GetValues(typeof(JobStat)).Cast<JobStat>().ToList()) {
				TreeNode nodeStatus=new TreeNode(status.ToString());
				listJobs.FindAll(x=>x.JobStatus==status).ForEach(x=>nodeStatus.Nodes.Add(new TreeNode(x.ToString()) {
					Tag=x,
					BackColor=(_listJobNumsHighlight.Contains(x.JobNum)?Color.Wheat:Color.White)
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

		///<summary>Recursive</summary>
		private TreeNode GetNodeHeirarchy(Job job) {
			TreeNode[] children=_listJobsFiltered.FindAll(x => x.ParentNum==job.JobNum).Select(GetNodeHeirarchy).ToArray();//can be enhanced by removing matches from the search set.
			TreeNode node=new TreeNode(job.ToString()) { Tag=job };
			if(children.Length>0) {
				node.Nodes.AddRange(children);
			}
			if(_listJobNumsHighlight.Contains(job.JobNum)) {
				node.BackColor=Color.Wheat;
			}
			return node;
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
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.OK:
					case DialogResult.Yes:
						userControlJobEdit.ForceSave();
						break;
					case DialogResult.No:
						//do nothing, allow new job to be laoded and lose unsaved changes.
						break;
					case DialogResult.Cancel:
						//do not load or navigate to new job.
						return;
				}
			}
			Job job=null;
			if(e.Node!=null && (e.Node.Tag is Job)) {
				job=(Job)e.Node.Tag;
			}
			userControlJobEdit.LoadJob(job);
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
				&& j1.OwnerNum!=Security.CurUser.UserNum
				&& j1.ExpertNum!=Security.CurUser.UserNum
				&& !JobPermissions.IsAuthorized(JobPerm.Approval,true)) 
			{
				return;//only expert, owner, Approver, or override can drag and drop.
			}
			//TODO: limit drag and drop based on permissions. (semi complete above)
			//Depending on tree view/grouping mode drag and drop performs different functions.
			switch((GroupJobsBy)comboGroup.SelectedIndex) {
				case GroupJobsBy.Heirarchy:
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
					break;
				case GroupJobsBy.Expert:
					TrySetExpert(j1,destinationNode);
					break;
				case GroupJobsBy.Owner:
					TrySetOwner(j1,destinationNode);
					break;
				case GroupJobsBy.Status:
					TrySetStatus(j1,destinationNode);
					break;
				case GroupJobsBy.User:
					TrySetUser(j1,destinationNode);
					break;
				default:
					return;
			}
			//Can be improved, this updates in memory list.
			Job temp=_listJobsAll.FirstOrDefault(x => x.JobNum==j1.JobNum);
			if(temp!=null) {//should never be null
				temp.ParentNum=j1.ParentNum; //update in memory list.
				temp.OwnerNum=j1.OwnerNum; //update in memory list.
				temp.ExpertNum=j1.ExpertNum; //update in memory list.
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
			return true;
		}

		private void TrySetExpert(Job j1,TreeNode destinationNode) {
			if(destinationNode==null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remove expert from job?")) {
					return;
				}
				j1.ExpertNum=0;
			}
			else if(destinationNode.Tag is Job) {
				Job j2=(Job)destinationNode.Tag;
				if(MessageBox.Show(string.Format("Change expert on job from {0} to {1}?",Userods.GetName(j1.ExpertNum),Userods.GetName(j2.ExpertNum)),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				j1.ExpertNum=j2.ExpertNum;
			}
			else if(destinationNode.Tag is Userod) {
				Userod user=(Userod)destinationNode.Tag;
				if(MessageBox.Show(string.Format("Change expert on job from {0} to {1}?",Userods.GetName(j1.ExpertNum),user.UserName),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				j1.ExpertNum=user.UserNum;
			}
			else {
				return;//no valid target;
			}
			Jobs.Update(j1);
		}

		private void TrySetOwner(Job j1,TreeNode destinationNode) {
			if(destinationNode==null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remove owner from job?")) {
					return;
				}
				j1.OwnerNum=0;
			}
			else if(destinationNode.Tag is Job) {
				Job j2=(Job)destinationNode.Tag;
				if(MessageBox.Show(string.Format("Change owner on job from {0} to {1}?",Userods.GetName(j1.OwnerNum),Userods.GetName(j2.OwnerNum)),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				j1.OwnerNum=j2.OwnerNum;
			}
			else if(destinationNode.Tag is Userod) {
				Userod user=(Userod)destinationNode.Tag;
				if(MessageBox.Show(string.Format("Change owner on job from {0} to {1}?",Userods.GetName(j1.OwnerNum),user.UserName),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				j1.OwnerNum=user.UserNum;
			}
			else {
				return;//no valid target
			}
			Jobs.Update(j1);
		}

		private void TrySetStatus(Job j1,TreeNode destinationNode) {
			if(destinationNode==null) {
				return;//no status to set. Jobs must have a status.
			}
			else if(destinationNode.Tag is Job) {
				Job j2=(Job)destinationNode.Tag;
				if(MessageBox.Show(string.Format("Change status of job from {0} to {1}?",j1.JobStatus.ToString(),j2.JobStatus.ToString()),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				j1.JobStatus=j2.JobStatus;
			}
			else {
				return;//no valid target
			}
			Jobs.Update(j1);
		}

		private void TrySetUser(Job j1,TreeNode destinationNode) {
			//TODO: this one is fairly complex
		}

		private void textFilter_TextChanged(object sender,EventArgs e) {
			FilterAndFill();
		}

		private void checkHighlight_CheckedChanged(object sender,EventArgs e) {
			FilterAndFill();
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

		private void checkMine_Click(object sender,EventArgs e) {
			if(checkMine.Checked) {
				UserFilter=Security.CurUser;
			}
			else if(UserFilter==Security.CurUser){
				UserFilter=null;//_userFilter can be something other than the current user.
			}
			FilterAndFill();
		}

		private void checkComplete_CheckedChanged(object sender,EventArgs e) {
			FilterAndFill();
		}

		#endregion

		private void gridWorkSummary_CellClick(object sender,ODGridClickEventArgs e) {
			if(!(gridWorkSummary.Rows[e.Row].Tag is Userod)) {
				return;
			}
			UserFilter=(Userod)gridWorkSummary.Rows[e.Row].Tag;
			checkMine.Checked=(UserFilter.UserNum==Security.CurUser.UserNum);
			FilterAndFill();
		}

		//private void gridWatchers_TitleAddClick(object sender,EventArgs e) {
		//	if(_jobCur==null) {
		//		return;
		//	}
		//	FormUserPick FormUP=new FormUserPick();
		//	FormUP.IsSelectionmode=true;
		//	FormUP.ShowDialog();
		//	if(FormUP.DialogResult!=DialogResult.OK) {
		//		return;
		//	}
		//	JobLink jobLink=new JobLink() {
		//		FKey=FormUP.SelectedUserNum,
		//		JobNum=_jobCur.JobNum,
		//		LinkType=JobLinkType.Watcher
		//	};
		//	JobLinks.Insert(jobLink);
		//	_listJobLinksUsers.Add(jobLink);//in memory list
		//	_listJobLinksAll.Add(jobLink);//in memory list
		//	//LoadJob(_jobCur);//cause refresh of controls
		//	Signalods.SetInvalidSignal(new Signalod2() {
		//		FKey=_jobCur.JobNum,
		//		FKeyType=FKeyTypeSig.JobNum,
		//		IType=InvalidType.Jobs
		//	});

		//}

		private void butAddJob_Click(object sender,EventArgs e) {
			FormJobNew FormJN=new FormJobNew();
			if(treeJobs.SelectedNode!=null && (treeJobs.SelectedNode.Tag is Job)) {
				Job jobSelected=(Job)treeJobs.SelectedNode.Tag;
				FormJN.JobCur.ParentNum=jobSelected.JobNum;
				FormJN.JobCur.ExpertNum=jobSelected.ExpertNum;
				FormJN.JobCur.Category=jobSelected.Category;
				FormJN.JobCur.Priority=jobSelected.Priority;
			}
			FormJN.ShowDialog();
			if(FormJN.DialogResult!=DialogResult.OK) {
				return;
			}
			//Jobs.Insert(FormJN.JobCur);
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

		///<summary>For UI only. Never saved to DB.</summary>
		private enum GroupJobsBy {
			None,
			//MyJobs,
			Heirarchy,
			User,
			Status,
			Expert,
			Owner,
			//Priority,

		}

		private void butSearch_Click(object sender,EventArgs e) {
			FormJobSearch FormJS=new FormJobSearch();
			//pass in data here to reduce calls to DB.
			FormJS.ShowDialog();
			if(FormJS.DialogResult!=DialogResult.OK) {
				return;
			}
			comboGroup.SelectedIndex=(int)GroupJobsBy.None;
			checkCollapse.Checked=false;
			checkMine.Checked=false;
			checkComplete.Checked=true;
			_listJobsFiltered=FormJS.GetSearchResults();
			FillTree();
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						userControlJobEdit.ForceSave();
						break;
					case System.Windows.Forms.DialogResult.No:
						//do nothing, load new job.
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return;
				}
			}
			userControlJobEdit.LoadJob(FormJS.SelectedJob);//can be null
		}

		private void comboCategorySearch_SelectedIndexChanged(object sender,EventArgs e) {
			FilterAndFill();
		}

		///<summary>This is a temporary solution. Once the Job Manager is programmed to use signals to refresh content dynamically this should be removed.</summary>
		private void butRefresh_Click(object sender,EventArgs e) {
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						userControlJobEdit.ForceSave();
						break;
					case System.Windows.Forms.DialogResult.No:
						//do nothing
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						return;
				}
			}
			RefreshAndFillThreaded();
		}

		private void FormJobManager2_FormClosing(object sender,FormClosingEventArgs e) {
			if(userControlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case System.Windows.Forms.DialogResult.OK:
					case System.Windows.Forms.DialogResult.Yes:
						userControlJobEdit.ForceSave();
						break;
					case System.Windows.Forms.DialogResult.No:
						//do nothing
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						e.Cancel=true;
						return;
				}
			}
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
				if(userControlJobEdit.IsChanged) {
					userControlJobEdit.LoadLinks(jobCur);//only load links from the job. Effectively merging changes.
				}
				else {//either the job was force saved above and IsChanged=false, or it was not edited to begin with.
					userControlJobEdit.LoadJob(jobCur);
				}
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

		}




	}
}
