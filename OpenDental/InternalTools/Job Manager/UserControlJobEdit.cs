using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental.InternalTools.Job_Manager {


	
	
	public partial class UserControlJobEdit:UserControl {
		//FIELDS
		public bool IsNew;
		private Job _jobOld;
		private Job _jobCur;
		///<summary>Private member for IsChanged Property. Private setter, public getter.</summary>
		private bool _isChanged;
		private bool _isOverride;

		///<summary>Occurs whenever this control saves changes to DB, after the control has redrawn itself. 
		/// Usually connected to either a form close or refresh.</summary>
		[Category("Action"),Description("Whenever this control saves changes to DB, after the control has redrawn itself. Usually connected to either a form close or refresh.")]
		public event EventHandler SaveClick=null;

		public delegate void UpdateTitleEvent(object sender,string title);
		public event UpdateTitleEvent TitleChanged=null;

		public delegate void JobOverrideEvent(object sender,bool isOverride);
		public event JobOverrideEvent JobOverride=null;

		//PROPERTIES
		public bool IsChanged {
			get { return _isChanged; }
			private set {
				_isChanged=value;
				butSave.Enabled=_isChanged;
			}
		}

		public bool IsOverride {
			get {return _isOverride;}
			set {
				_isOverride=value;
				CheckPermissions();
			}
		}

		//FUNCTIONS
		public UserControlJobEdit() {
			InitializeComponent();
			Enum.GetNames(typeof(JobPriority)).ToList().ForEach(x=>comboPriority.Items.Add(x));
			Enum.GetNames(typeof(JobStat)).ToList().ForEach(x=>comboStatus.Items.Add(x));
			Enum.GetNames(typeof(JobCategory)).ToList().ForEach(x=>comboCategory.Items.Add(x));
		}

		///<summary>Not a property so that this is compatible with the VS designer.</summary>
		public Job GetJob() {
			return _jobCur;
		}

		///<summary>Should only be called once when new job should be loaded into control. If called again, changes will be lost.</summary>
		public void LoadJob(Job job) {
			this.Enabled=false;//disable control while it is filled.
			_isOverride=false;
			if(job==null) {
				_jobCur=new Job();
			}
			else {
				_jobCur=job.Copy();
			}
			_jobOld=_jobCur.Copy();//cannot be null
			textTitle.Text=_jobCur.Title;
			textJobNum.Text=_jobCur.JobNum>0?_jobCur.JobNum.ToString():Lan.g("Jobs","New Job");
			comboPriority.SelectedIndex=(int)_jobCur.Priority;
			comboStatus.SelectedIndex=(int)_jobCur.JobStatus;
			comboCategory.SelectedIndex=(int)_jobCur.Category;
			textDateEntry.Text=_jobCur.DateTimeEntry.Year>1880?_jobCur.DateTimeEntry.ToShortDateString():"";
			textVersion.Text=_jobCur.JobVersion;
			try {
				textEditorMain.MainRtf=_jobCur.Description;//This is here to convert our old job descriptions to the new RTF descriptions.
			}
			catch {
				textEditorMain.MainText=_jobCur.Description;
			}
			textExpert.Text="";
			Userod expert=UserodC.Listt.FirstOrDefault(x=>x.UserNum==_jobCur.ExpertNum);
			if(expert!=null) {
				textExpert.Text=expert.UserName;
			}
			textOwner.Text="";
			Userod owner=UserodC.Listt.FirstOrDefault(x=>x.UserNum==_jobCur.OwnerNum);
			if(owner!=null) {
				textOwner.Text=owner.UserName;
			}
			textPrevOwner.Text=JobEvents.GetPrevOwnerName(_jobCur.JobNum);
			textEstHours.Text=_jobCur.HoursEstimate.ToString();
			textActualHours.Text=_jobCur.HoursActual.ToString();
			Job parent=Jobs.GetOne(_jobCur.ParentNum);
			textParent.Text=parent!=null?parent.ToString():"";
			FillAllGrids();
			CheckPermissions();
			IsChanged=false;
			if(job!=null) {//re-enable control after we have loaded the job.
				this.Enabled=true;
			}
		}

		private void FillAllGrids() {
			FillGridWatchers();
			FillGridCustQuote();
			FillGridTasks();
			FillGridFeatuerReq();
			FillGridBugs();
			FillGridNote();
			FillGridReviews();
			FillGridHistory();
		}

		private void FillGridWatchers() {
			gridWatchers.BeginUpdate();
			gridWatchers.Columns.Clear();
			gridWatchers.Columns.Add(new ODGridColumn("",50));
			gridWatchers.Rows.Clear();
			List<Userod> listWatchers=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Watcher).Select(x => UserodC.Listt.FirstOrDefault(y => y.UserNum==x.FKey)).ToList();
			foreach(Userod user in listWatchers.FindAll(x => x!=null)){
				ODGridRow row=new ODGridRow() { Tag =user };
				row.Cells.Add(user.UserName);
				gridWatchers.Rows.Add(row);
			}
			gridWatchers.EndUpdate();
		}

		private void FillGridCustQuote() {
			gridCustomerQuotes.BeginUpdate();
			gridCustomerQuotes.Columns.Clear();
			gridCustomerQuotes.Columns.Add(new ODGridColumn("PatNum",75));
			gridCustomerQuotes.Columns.Add(new ODGridColumn("Quote",50));
			gridCustomerQuotes.Rows.Clear();
			foreach(JobQuote jobQuote in _jobCur.ListJobQuotes){
				ODGridRow row=new ODGridRow() { Tag=jobQuote };//JobQuote
				row.Cells.Add(jobQuote.PatNum.ToString());
				row.Cells.Add(jobQuote.Amount);
				gridCustomerQuotes.Rows.Add(row);
			}
			gridCustomerQuotes.EndUpdate();
		}

		private void FillGridTasks() {
			gridTasks.BeginUpdate();
			gridTasks.Columns.Clear();
			gridTasks.Columns.Add(new ODGridColumn("Date",70));
			gridTasks.Columns.Add(new ODGridColumn("TaskList",100));
			gridTasks.Columns.Add(new ODGridColumn("Done",40) { TextAlign=HorizontalAlignment.Center });
			gridTasks.NoteSpanStart=0;
			gridTasks.NoteSpanStop=2;
			gridTasks.Rows.Clear();
			List<Task> listTasks=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Task).Select(x => Tasks.GetOne(x.FKey)).ToList();
			foreach(Task task in listTasks.FindAll(x => x!=null)){
				ODGridRow row=new ODGridRow() { Tag=task.TaskNum };//taskNum
				row.Cells.Add(task.DateTimeEntry.ToShortDateString());
				row.Cells.Add(TaskLists.GetOne(task.TaskListNum).Descript);
				row.Cells.Add(task.DateTimeFinished.Year>1880?"X":"");
				row.Note=task.Descript.PadRight(100).Substring(0,100).Trim();
				gridTasks.Rows.Add(row);
			}
			gridTasks.EndUpdate();
		}

		private void FillGridFeatuerReq() {
			gridFeatureReq.BeginUpdate();
			gridFeatureReq.Columns.Clear();
			gridFeatureReq.Columns.Add(new ODGridColumn("Feat Req Num",150));
			//todo: add status of FR. Difficult because FR dataset comes from webservice.
			//gridFeatureReq.Columns.Add(new ODGridColumn("Status",50){TextAlign=HorizontalAlignment.Center});
			gridFeatureReq.Rows.Clear();
			List<long> listReqNums=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Request).Select(x => x.FKey).ToList();
			foreach(long reqNum in listReqNums){
				ODGridRow row=new ODGridRow() { Tag=reqNum };//FR Num
				row.Cells.Add(reqNum.ToString());
				//todo: add status of FR. Difficult because FR dataset comes from webservice.
				gridFeatureReq.Rows.Add(row);
			}
			gridFeatureReq.EndUpdate();
		}

		private void FillGridBugs() {
			gridBugs.BeginUpdate();
			gridBugs.Columns.Clear();
			gridBugs.Columns.Add(new ODGridColumn("Bug Num (From JRMT)",50));
			gridBugs.Rows.Clear();
			List<long> listBugNums=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Bug).Select(x => x.FKey).ToList();
			foreach(Bug bug in listBugNums.Select(Bugs.GetOne).Where(x=>x!=null)){
				ODGridRow row=new ODGridRow() { Tag=bug.BugId };//bugNum
				row.Cells.Add(bug.Description);
				gridBugs.Rows.Add(row);
			}
			gridBugs.EndUpdate();
		}

		public void FillGridNote() {
			gridNotes.BeginUpdate();
			gridNotes.Columns.Clear();
			gridNotes.Columns.Add(new ODGridColumn(Lan.g(this,"Date Time"),120));
			gridNotes.Columns.Add(new ODGridColumn(Lan.g(this,"User"),80));
			gridNotes.Columns.Add(new ODGridColumn(Lan.g(this,"Note"),400));
			gridNotes.Rows.Clear();
			ODGridRow row;
			foreach(JobNote jobNote in _jobCur.ListJobNotes) {
				row=new ODGridRow();
				row.Cells.Add(jobNote.DateTimeNote.ToShortDateString()+" "+jobNote.DateTimeNote.ToShortTimeString());
				row.Cells.Add(Userods.GetName(jobNote.UserNum));
				row.Cells.Add(jobNote.Note);
				row.Tag=jobNote;
				gridNotes.Rows.Add(row);
			}
			gridNotes.EndUpdate();
			gridNotes.ScrollToEnd();
		}

		private void FillGridReviews() {
			long selectedReviewNum=0;
			if(gridReview.GetSelectedIndex()!=-1) {
				selectedReviewNum=(long)gridReview.Rows[gridReview.GetSelectedIndex()].Tag;
			}
			gridReview.BeginUpdate();
			gridReview.Columns.Clear();
			gridReview.Columns.Add(new ODGridColumn("Date Last Edited",90));
			gridReview.Columns.Add(new ODGridColumn("Reviewer",80));
			gridReview.Columns.Add(new ODGridColumn("Status",80));
			gridReview.Columns.Add(new ODGridColumn("Description",200));
			gridReview.Rows.Clear();
			ODGridRow row;
			foreach(JobReview jobReview in _jobCur.ListJobReviews) {
				row=new ODGridRow();
				row.Cells.Add(jobReview.DateTStamp.ToShortDateString());
				row.Cells.Add(Userods.GetName(jobReview.ReviewerNum));
				row.Cells.Add(Enum.GetName(typeof(JobReviewStatus),(int)jobReview.ReviewStatus));
				if(jobReview.Description.Length>=80) {
					row.Cells.Add(jobReview.Description.Substring(0,80)+"...");
				}
				else {
					row.Cells.Add(jobReview.Description);
				}
				row.Tag=jobReview.JobReviewNum;
				gridReview.Rows.Add(row);
			}
			gridReview.EndUpdate();
			for(int i=0;i<gridReview.Rows.Count;i++) {
				if((long)gridReview.Rows[i].Tag==selectedReviewNum) {
					gridReview.SetSelected(i,true);
					break;
				}
			}
		}

		private void FillGridHistory() {
			long selectedEventNum=0;
			if(gridHistory.GetSelectedIndex()!=-1) {
				selectedEventNum=(long)gridHistory.Rows[gridHistory.GetSelectedIndex()].Tag;
			}
			gridHistory.BeginUpdate();
			gridHistory.Columns.Clear();
			gridHistory.Columns.Add(new ODGridColumn("Date",140));
			gridHistory.Columns.Add(new ODGridColumn("Owner",100));
			gridHistory.Columns.Add(new ODGridColumn("Status",100));
			gridHistory.Rows.Clear();
			ODGridRow row;
			foreach(JobEvent jobEvent in _jobCur.ListJobEvents) {
				row=new ODGridRow();
				row.Cells.Add(jobEvent.DateTimeEntry.ToShortDateString()+" "+jobEvent.DateTimeEntry.ToShortTimeString());
				row.Cells.Add(Userods.GetName(jobEvent.OwnerNum));
				row.Cells.Add(Enum.GetName(typeof(JobStat),(int)jobEvent.JobStatus));
				row.Tag=jobEvent.JobEventNum;
				gridHistory.Rows.Add(row);
			}
			gridHistory.EndUpdate();
			for(int i=0;i<gridHistory.Rows.Count;i++) {
				if((long)gridHistory.Rows[i].Tag==selectedEventNum) {
					gridHistory.SetSelected(i,true);
					break;
				}
			}
		}

		///<summary>Based on job status, category, and user role, this will enable or disable various controls.</summary>
		private void CheckPermissions() {
			//disable various controls and re-enable them below depending on permissions.
			textTitle.ReadOnly=true;
			comboPriority.Enabled=false;
			comboStatus.Enabled=false;
			comboCategory.Enabled=false;
			textVersion.ReadOnly=true;
			textEstHours.Enabled=false;
			textActualHours.Enabled=false;
			butParentPick.Visible=false;
			butParentRemove.Visible=false;
			butExpertPick.Visible=false;
			butOwnerPick.Visible=false;
			gridCustomerQuotes.HasAddButton=false;//Quote permission only
			textEditorMain.ReadOnly=true;
			if(_jobCur==null) {
				return;
			}
			if(JobPermissions.IsAuthorized(JobPerm.Quote,true) && _jobOld.JobStatus!=JobStat.Complete && _jobOld.JobStatus!=JobStat.Deleted) {
				gridCustomerQuotes.HasAddButton=true;
			}
			switch(_jobCur.JobStatus) {
				case JobStat.Concept:
					if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
						break;
					}
					comboPriority.Enabled=true;
					comboCategory.Enabled=true;
					textEstHours.ReadOnly=false;
					butParentPick.Enabled=true;
					butParentRemove.Enabled=true;
					textEditorMain.ReadOnly=false;
					break;
				case JobStat.NeedsConceptApproval:
				case JobStat.NeedsJobApproval:
					if(!JobPermissions.IsAuthorized(JobPerm.Approval,true)) {
						break;
					}
					comboPriority.Enabled=true;
					comboCategory.Enabled=true;
					textEstHours.ReadOnly=false;
					butParentPick.Enabled=true;
					butParentRemove.Enabled=true;
					textEditorMain.ReadOnly=false;
					break;
				case JobStat.ConceptApproved:
				case JobStat.CurrentlyWriting:
				case JobStat.NeedsJobClarification:
					if(!JobPermissions.IsAuthorized(JobPerm.Writeup,true) || _jobOld.ExpertNum!=Security.CurUser.UserNum) {
						break;
					}
					comboPriority.Enabled=true;
					comboCategory.Enabled=true;
					textEstHours.ReadOnly=false;
					butParentPick.Enabled=true;
					butParentRemove.Enabled=true;
					textEditorMain.ReadOnly=false;
					break;
				case JobStat.JobApproved:
				case JobStat.ReadyToAssign:
					if(!JobPermissions.IsAuthorized(JobPerm.Writeup,true) || (_jobOld.ExpertNum!=Security.CurUser.UserNum && _jobOld.ExpertNum!=0)) {
						break;
					}
					comboPriority.Enabled=true;
					comboCategory.Enabled=true;
					textEstHours.ReadOnly=false;
					butParentPick.Enabled=true;
					butParentRemove.Enabled=true;
					if(_jobOld.ExpertNum==0) {
						butExpertPick.Enabled=true;
					}
					butOwnerPick.Enabled=true;
					break;
				case JobStat.Assigned:
				case JobStat.CurrentlyWorkingOn:
				case JobStat.OnHoldExpert:
				case JobStat.ReadyForReview:
				case JobStat.OnHoldEngineer:
					if(_jobOld.OwnerNum!=Security.CurUser.UserNum && _jobOld.ExpertNum!=Security.CurUser.UserNum) {
						break;
					}
					textActualHours.Enabled=true;
					gridReview.HasAddButton=true;
					break;
				case JobStat.ReadyToBeDocumented:
				case JobStat.NeedsDocumentationClarification:
				case JobStat.NotifyCustomer:
					//Not sure what needs to be edited here.
					break;
				case JobStat.Rescinded:
				case JobStat.Complete:
				case JobStat.Deleted:
					gridTasks.HasAddButton=false;
					gridFeatureReq.HasAddButton=false;
					gridBugs.HasAddButton=false;
					gridReview.HasAddButton=false;
					//nothing enabled.
					break;
				default:
					MessageBox.Show("Unsupported job status. Add to UserControlJobEdit.CheckPermissions()");
					break;
			}
			if(_isOverride) {//Enable everything and make everything visible
				textTitle.ReadOnly=false;
				comboPriority.Enabled=true;
				comboStatus.Enabled=true;
				comboCategory.Enabled=true;
				textVersion.ReadOnly=false;
				textEstHours.Enabled=true;
				textActualHours.Enabled=true;
				butParentPick.Visible=true;
				butParentRemove.Visible=true;
				butExpertPick.Visible=true;
				butOwnerPick.Visible=true;
				gridCustomerQuotes.HasAddButton=true;
				textEditorMain.ReadOnly=false;
			}
		}

		///<summary>Resizes Link Grids in group box.</summary>
		private void groupLinks_Resize(object sender,EventArgs e) {
			List<ODGrid> grids=groupLinks.Controls.OfType<ODGrid>().OrderBy(x => x.Top).ToList();
			int padding=4;
			int topMost=grids.Min(x=>x.Top);
			int sizeEach=(groupLinks.Height-topMost-(padding*grids.Count))/grids.Count;
			for(int i=0;i<grids.Count;i++) {
				grids[i].Top=topMost+(i*(sizeEach+padding));
				grids[i].Height=sizeEach;
			}
		}

		private void butActions_Click(object sender,EventArgs e) {
			bool perm=false;
			ContextMenu actionMenu=new System.Windows.Forms.ContextMenu();
			switch(_jobCur.JobStatus) {
				case JobStat.Concept:
					perm=JobPermissions.IsAuthorized(JobPerm.Concept,true);
					actionMenu.MenuItems.Add(new MenuItem("Send for Approval",actionMenu_ApprovalClick) { Enabled=perm });
					break;
				case JobStat.NeedsConceptApproval:
					perm=JobPermissions.IsAuthorized(JobPerm.Approval,true);
					actionMenu.MenuItems.Add(new MenuItem("Approve Concept",actionMenu_ApproveConceptClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Ask For Clarification",actionMenu_AskClarificationClick) { Enabled=perm });
					break;
				case JobStat.ConceptApproved:
				case JobStat.CurrentlyWriting:
					perm=JobPermissions.IsAuthorized(JobPerm.Writeup,true) && _jobCur.ExpertNum==Security.CurUser.UserNum;
					actionMenu.MenuItems.Add(new MenuItem("Send for Approval",actionMenu_ReApprovalClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Currently Writing",actionMenu_CurrentlyWritingClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Ask For Clarification",actionMenu_AskClarificationClick) { Enabled=perm });
					break;
				case JobStat.NeedsJobApproval:
					perm=JobPermissions.IsAuthorized(JobPerm.Approval,true);
					actionMenu.MenuItems.Add(new MenuItem("Approve Job",actionMenu_ApproveJobClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Assign to Engineer",actionMenu_AssignEngineerClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Ask For Clarification",actionMenu_AskJobClarificationClick) { Enabled=perm });
					break;
				case JobStat.NeedsJobClarification:
					perm=JobPermissions.IsAuthorized(JobPerm.Writeup,true);
					actionMenu.MenuItems.Add(new MenuItem("Send Back To "+JobEvents.GetPrevOwnerName(_jobCur.JobNum),actionMenu_SendBackToClick) { Enabled=perm });
					perm=perm && _jobCur.ExpertNum==Security.CurUser.UserNum;//assigned expert only
					actionMenu.MenuItems.Add(new MenuItem("Change To Edit Mode",actionMenu_EditModeClick) { Enabled=perm });
					break;
				case JobStat.JobApproved:
				case JobStat.OnHoldExpert:
				case JobStat.ReadyToAssign:
					perm=JobPermissions.IsAuthorized(JobPerm.Writeup,true) && (_jobCur.JobStatus==JobStat.ReadyToAssign || _jobCur.ExpertNum==Security.CurUser.UserNum);
					actionMenu.MenuItems.Add(new MenuItem("Assign to Engineer",actionMenu_AssignEngineerClick) {Enabled=perm});//WU + (Expert or ReadyToAssign)
					perm=JobPermissions.IsAuthorized(JobPerm.Writeup,true) && _jobCur.ExpertNum==Security.CurUser.UserNum;//assigned expert only
					actionMenu.MenuItems.Add(new MenuItem("Put On Hold",actionMenu_OnHoldExpertClick) {Enabled=perm});
					actionMenu.MenuItems.Add(new MenuItem("Ready to Assign",actionMenu_AssignClick) {Enabled=perm});
					break;
				case JobStat.Assigned:
				case JobStat.CurrentlyWorkingOn:
				case JobStat.ReadyForReview:
				case JobStat.OnHoldEngineer:
					perm=JobPermissions.IsAuthorized(JobPerm.Engineer,true) && _jobCur.OwnerNum==Security.CurUser.UserNum;
					actionMenu.MenuItems.Add(new MenuItem("Send To Tech Writer",actionMenu_TechWriterSendClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Currently Working On",actionMenu_CurrentlyWorkingOnClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Put On Hold",actionMenu_OnHoldEngineerClick) {Enabled=perm});
					actionMenu.MenuItems.Add(new MenuItem("Ready For Review",actionMenu_ReviewClick) { Enabled=perm });
					break;
				case JobStat.ReadyToBeDocumented:
					perm=JobPermissions.IsAuthorized(JobPerm.Documentation,true);
					actionMenu.MenuItems.Add(new MenuItem("Send To Customer Notify",actionMenu_SendCustomerNotifyClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Ask For Clarification",actionMenu_AskClarificationDocumentaionClick) { Enabled=perm });
					break;
				case JobStat.NeedsDocumentationClarification:
					perm=!JobPermissions.IsAuthorized(JobPerm.Documentation,true);
					actionMenu.MenuItems.Add(new MenuItem("Send to Documentation",actionMenu_TechWriterSendClick) { Enabled=perm });
					break;
				case JobStat.NotifyCustomer:
					perm=JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true);
					actionMenu.MenuItems.Add(new MenuItem("Mark Complete",actionMenu_MarkCompleteClick) { Enabled=perm });
					break;
				case JobStat.Rescinded:
				case JobStat.Complete:
				case JobStat.Deleted:
					perm=JobPermissions.IsAuthorized(JobPerm.Approval,true);
					actionMenu.MenuItems.Add(new MenuItem("Unlock Job",actionMenu_UnlockClick) { Enabled=perm });
					break;
				default:
					break;
			}
			if(JobPermissions.IsAuthorized(JobPerm.Approval,true)) {
				actionMenu.MenuItems.Add("-");
				actionMenu.MenuItems.Add("Delete",actionMenu_DeleteClick);
			}
			if(JobPermissions.IsAuthorized(JobPerm.Override,true)) {
				actionMenu.MenuItems.Add("-");
				actionMenu.MenuItems.Add("Override",actionMenu_OverrideClick);
			}
			if(actionMenu.MenuItems.Count>0 && actionMenu.MenuItems[0].Text=="-") {
				actionMenu.MenuItems.RemoveAt(0);
			}
			if(actionMenu.MenuItems.Count==0) {
				actionMenu.MenuItems.Add(new MenuItem("No Actions Available"){Enabled=false});
			}
			butActions.ContextMenu=actionMenu;
			butActions.ContextMenu.Show(butActions,new Point(0,butActions.Height));
		}

		#region Action Menu Item Event Handlers

		private void actionMenu_ApprovalClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Approval,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NeedsConceptApproval;
			SaveJob(_jobCur);
		}

		private void actionMenu_ApproveConceptClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Writeup,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.ExpertNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.ConceptApproved;
			SaveJob(_jobCur);
		}

		private void actionMenu_AskClarificationClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Concept,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.Concept;
			SaveJob(_jobCur);
		}

		private void actionMenu_AskJobClarificationClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Writeup,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NeedsJobClarification;
			SaveJob(_jobCur);
		}

		private void actionMenu_ReApprovalClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Approval,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NeedsJobApproval;
			SaveJob(_jobCur);
		}

		private void actionMenu_CurrentlyWritingClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.CurrentlyWriting;
			SaveJob(_jobCur);
		}

		private void actionMenu_ApproveJobClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Writeup,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.JobApproved;
			SaveJob(_jobCur);
		}

		private void actionMenu_AssignClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.ReadyToAssign;
			SaveJob(_jobCur);
		}

		private void actionMenu_AssignEngineerClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Engineer,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.Assigned;
			SaveJob(_jobCur);
		}

		private void actionMenu_SendBackToClick(object sender,EventArgs e) {
			long userNum=JobEvents.GetMostRecent(_jobCur.JobNum).OwnerNum;
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NeedsJobApproval;
			SaveJob(_jobCur);
		}

		private void actionMenu_EditModeClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.ConceptApproved;
			SaveJob(_jobCur);
		}

		private void actionMenu_CurrentlyWorkingOnClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.CurrentlyWorkingOn;
			SaveJob(_jobCur);
		}

		private void actionMenu_OnHoldExpertClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.OnHoldExpert;
			SaveJob(_jobCur);
		}

		private void actionMenu_OnHoldEngineerClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.OnHoldEngineer;
			SaveJob(_jobCur);
		}

		private void actionMenu_ReviewClick(object sender,EventArgs e) {
			//Todo: launch job review window, edit, and save.
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.ReadyForReview;
			SaveJob(_jobCur);
		}

		private void actionMenu_AskClarificationDocumentaionClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Concept,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NeedsDocumentationClarification;
			SaveJob(_jobCur);
		}

		private void actionMenu_TechWriterSendClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Documentation,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.ReadyToBeDocumented;
			SaveJob(_jobCur);
		}

		private void actionMenu_SendCustomerNotifyClick(object sender,EventArgs e) {
			long userNum=0;
			if(!PickUsersForRole(JobPerm.NotifyCustomer,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=userNum;
			comboStatus.SelectedIndex=(int)JobStat.NotifyCustomer;
			SaveJob(_jobCur);
		}

		private void actionMenu_MarkCompleteClick(object sender,EventArgs e) {
			IsChanged=true;
			comboStatus.SelectedIndex=(int)JobStat.Complete;
			SaveJob(_jobCur);
		}

		private void actionMenu_UnlockClick(object sender,EventArgs e) {
			MsgBox.Show(this,"Not Yet Implemented");
		}

		private void actionMenu_DeleteClick(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this job will delete all JobEvents"
				+" and pointers to other tasks, features, and bugs. Are you sure you want to continue?")) 
			{
				return;
			}
			try { //Jobs.Delete will throw an application exception if there are any reviews associated with this job.
				Jobs.Delete(_jobCur.JobNum);
				LoadJob(null);
				if(SaveClick!=null) {
					SaveClick(this,new EventArgs());
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void actionMenu_OverrideClick(object sender,EventArgs e) {
			_isOverride=true;
			if(this.JobOverride!=null) {
				JobOverride(this,true);
			}
			CheckPermissions();
		}

		///<summary>Returns false if user cancels. Out variable is the selected usernum.</summary>
		private bool PickUsersForRole(JobPerm permType,bool showHidden, out long userNum) {
			userNum=0;
			List<Userod> listUsersForPicker=Userods.GetUsersByJobRole(permType,showHidden);
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUser=listUsersForPicker;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return false;
			}
			userNum=FormUP.SelectedUserNum;
			return true;
		}

		#endregion

		private void textTitle_TextChanged(object sender,EventArgs e) {
			_jobCur.Title=textTitle.Text;
			IsChanged=true;
			if(TitleChanged!=null) {
				TitleChanged(this,textTitle.Text);
			}
		}

		private void butParentRemove_Click(object sender,EventArgs e) {
			IsChanged=true;
			_jobCur.ParentNum=0;
			textParent.Text="";
		}

		private void butParentPick_Click(object sender,EventArgs e) {
			InputBox inBox=new InputBox("Input parent job number.");
			inBox.ShowDialog();
			if(inBox.DialogResult!=DialogResult.OK) {
				return;
			}
			long jobNum=0;
			long.TryParse(new string(inBox.textResult.Text.Where(char.IsDigit).ToArray()),out jobNum);
			Job job=Jobs.GetOne(jobNum);
			if(job==null) {
				return;
			}
			if(Jobs.CheckForLoop(_jobCur.JobNum,jobNum)) {
				MsgBox.Show(this,"Invalid parent job, would create an infinite loop.");
				return;
			}
			IsChanged=true;
			_jobCur.ParentNum=job.JobNum;
			textParent.Text=job.ToString();
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(string.IsNullOrWhiteSpace(textTitle.Text)) {
				MsgBox.Show(this,"Invalid job title.");
				return;
			}
			SaveJob(_jobCur);
		}

		///<summary>Job must have all in memory fields filled. Eg. Job.ListJobLinks, Job.ListJobNotes, etc.</summary>
		private void SaveJob(Job job) {
			job.Description=textEditorMain.MainRtf;
			job.HoursActual=PIn.Int(textActualHours.Text);
			job.HoursEstimate=PIn.Int(textEstHours.Text);
			job.Priority=(JobPriority)comboPriority.SelectedIndex;
			job.JobStatus=(JobStat)comboStatus.SelectedIndex;
			job.Category=(JobCategory)comboCategory.SelectedIndex;
			//All other fields should have been maintained while editing the job in the form.
			if(job.JobNum==0 || IsNew) {
				Jobs.Insert(job);
				job.ListJobLinks.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobNotes.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobReviews.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobQuotes.ForEach(x=>x.JobNum=job.JobNum);
				//job.ListJobEvents.ForEach(x=>x.JobNum=job.JobNum);//do not sync
			}
			else {
				Jobs.Update(job);
			}
			JobLinks.Sync(job.ListJobLinks,job.JobNum);
			JobNotes.Sync(job.ListJobNotes,job.JobNum);
			JobReviews.Sync(job.ListJobReviews,job.JobNum);
			JobQuotes.Sync(job.ListJobQuotes,job.JobNum);
			//JobEvents.Sync();//do not sync
			if(job.OwnerNum!=_jobOld.OwnerNum || job.JobStatus!=_jobOld.JobStatus || job.Description!=_jobOld.Description) {
				JobEvent jobEventCur=new JobEvent();
				//Must do text manipulation inside the RichTextbox to preserve RTF Formatting.
				try {
					textEditorMain.MainRtf=_jobOld.Description;
				}
				catch {
					textEditorMain.MainText=_jobOld.Description;
				}
				if(_isOverride) {
					textEditorMain.MainText=textEditorMain.MainText.Insert(0,"THIS JOB WAS MANUALLY OVERRIDDEN BY "+Security.CurUser.UserName+":\r\n");
				}
				jobEventCur.Description=textEditorMain.MainRtf;
				jobEventCur.JobNum=_jobOld.JobNum;
				jobEventCur.JobStatus=_jobOld.JobStatus;
				jobEventCur.OwnerNum=_jobOld.OwnerNum;
				JobEvents.Insert(jobEventCur);
				job.ListJobEvents.Add(JobEvents.GetOne(jobEventCur.JobEventNum));//to get correct time stamp
			}
			LoadJob(job);
			if(SaveClick!=null) {
				SaveClick(this,new EventArgs());
			}
		}

		//Allows save to be called from outside this control.
		public void ForceSave() {
			if(_jobCur==null || IsChanged==false) {
				return;
			}
			SaveJob(_jobCur);
		}

		private void comboPriority_SelectionChangeCommitted(object sender,EventArgs e) {
			IsChanged=true;
		}

		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			IsChanged=true;
		}

		private void comboCategory_SelectionChangeCommitted(object sender,EventArgs e) {
			IsChanged=true;
		}

		private void butExpertPick_Click(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Engineer,false,out userNum)) {
				return;
			}
			IsChanged=true;
			_jobCur.ExpertNum=userNum;
			textExpert.Text=Userods.GetName(_jobCur.ExpertNum);
		}

		private void butOwnerPick_Click(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			List<Userod> listUsersForPicker=Userods.GetUsersByJobRole(JobPerm.Engineer,false);
			FormUserPick FormUP=new FormUserPick {
				IsSelectionmode=true,
				ListUser=listUsersForPicker
			};
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			IsChanged=true;
			_jobCur.OwnerNum=FormUP.SelectedUserNum;
			textOwner.Text=Userods.GetName(_jobCur.OwnerNum);
		}

		private void gridWatchers_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			FormUserPick FormUP=new FormUserPick();
			if(_jobCur.ListJobLinks.FindAll(x=>x.LinkType==JobLinkType.Watcher).All(x=>x.FKey!=Security.CurUser.UserNum)) {
				FormUP.SuggestedUserNum=Security.CurUser.UserNum;
			}
			FormUP.IsSelectionmode=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink=new JobLink() {
				FKey=FormUP.SelectedUserNum,
				JobNum=_jobCur.JobNum,
				LinkType=JobLinkType.Watcher
			};
			//"Save" to memory only.
			_jobCur.ListJobLinks.Add(jobLink);
			IsChanged=true;
			FillGridWatchers();
		}

		private void gridCustomerQuotes_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(new JobQuote() {JobNum=_jobCur.JobNum,IsNew=true});
			FormJQE.ShowDialog();
			if(FormJQE.DialogResult!=DialogResult.OK || FormJQE.JobQuoteCur==null) {
				return;
			}
			IsChanged=true;
			_jobCur.ListJobQuotes.Add(FormJQE.JobQuoteCur);
			FillGridCustQuote();
		}

		private void gridTasks_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			FormTaskSearch FormTS=new FormTaskSearch() {IsSelectionMode=true};
			FormTS.ShowDialog();
			if(FormTS.DialogResult!=DialogResult.OK) {
				return;
			}
			IsChanged=true;
			JobLink jobLink=new JobLink();
			jobLink.JobNum=_jobCur.JobNum;
			jobLink.FKey=FormTS.SelectedTaskNum;
			jobLink.LinkType=JobLinkType.Task;
			_jobCur.ListJobLinks.Add(jobLink);
			FillGridTasks();
		}

		private void gridFeatureReq_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			FormFeatureRequest FormFR=new FormFeatureRequest() {IsSelectionMode=true};
			FormFR.ShowDialog();
			if(FormFR.DialogResult!=DialogResult.OK) {
				return;
			}
			IsChanged=true;
			JobLink jobLink=new JobLink();
			jobLink.JobNum=_jobCur.JobNum;
			jobLink.FKey=FormFR.SelectedFeatureNum;
			jobLink.LinkType=JobLinkType.Request;
			_jobCur.ListJobLinks.Add(jobLink);
			FillGridFeatuerReq();
		}

		private void gridBugs_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			FormBugSearch FormBS=new FormBugSearch();
			FormBS.ShowDialog();
			if(FormBS.DialogResult!=DialogResult.OK || FormBS.BugCur==null) {
				return;
			}
			IsChanged=true;
			JobLink jobLink=new JobLink();
			jobLink.JobNum=_jobCur.JobNum;
			jobLink.FKey=FormBS.BugCur.BugId;
			jobLink.LinkType=JobLinkType.Bug;
			_jobCur.ListJobLinks.Add(jobLink);
			FillGridBugs();
		}

		private void gridReview_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			long userNum=0;
			if(!PickUsersForRole(JobPerm.Writeup,false,out userNum)) {
				return;
			}
			FormJobReviewEdit FormJRE=new FormJobReviewEdit(new JobReview { ReviewerNum=userNum,JobNum=_jobCur.JobNum,IsNew=true },false);
			FormJRE.ShowDialog();
			if(FormJRE.DialogResult!=DialogResult.OK || FormJRE.JobReviewCur==null) {
				return;
			}
			IsChanged=true;
			_jobCur.ListJobReviews.Add(FormJRE.JobReviewCur);
			FillGridReviews();
		}

		private void gridNotes_TitleAddClick(object sender,EventArgs e) {
			if(_jobCur==null) {
				return;//should never happen
			}
			JobNote jobNote=new JobNote() {
				DateTimeNote=MiscData.GetNowDateTime(),
				IsNew=true,
				JobNum=_jobCur.JobNum,
				UserNum=Security.CurUser.UserNum
			};
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			FormJNE.ShowDialog();
			if(FormJNE.DialogResult!=DialogResult.OK || FormJNE.JobNoteCur==null) {
				return;
			}
			IsChanged=true;
			_jobCur.ListJobNotes.Add(FormJNE.JobNoteCur);
			FillGridNote();
		}

		private void gridCustomerQuotes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridCustomerQuotes.Rows[e.Row].Tag is JobQuote)) {
				return;//should never happen
			}
			JobQuote jq=(JobQuote)gridCustomerQuotes.Rows[e.Row].Tag;
			FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(jq);
			FormJQE.ShowDialog();
			if(FormJQE.DialogResult!=DialogResult.OK){
				return;
			}
			IsChanged=true;
			_jobCur.ListJobQuotes.RemoveAll(x=>x.JobQuoteNum==jq.JobQuoteNum);//should remove only one
			if(FormJQE.JobQuoteCur!=null) {//re-add altered version, iff the jobquote was modified.
				_jobCur.ListJobQuotes.Add(FormJQE.JobQuoteCur);
			}
			FillGridCustQuote();
		}

		private void gridTasks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridTasks.Rows[e.Row].Tag is long)) {
				return;//should never happen
			}
			Task task=Tasks.GetOne((long)gridTasks.Rows[e.Row].Tag);
			FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
			FormTE.ShowDialog();
			if(FormTE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGridTasks();
			//modeless, changes will not be reflected in the task grid. changes rarely occur that affect the grid however.
		}

		private void textEditorMain_OnTextEdited() {
			IsChanged=true;
		}

		private void textEstHours_TextChanged(object sender,EventArgs e) {
			IsChanged=true;
		}

		private void textActualHours_TextChanged(object sender,EventArgs e) {
			IsChanged=true;
		}

		private void gridFeatureReq_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//Todo. Open Feature request editor... this is tedious due to the fact that the feature request data set comes form the web service and is not a proper table.
		}

		private void gridHistory_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormJobHistoryView FormJHV=new FormJobHistoryView((long)gridHistory.Rows[e.Row].Tag);
			FormJHV.ShowDialog();
		}

		private void gridNotes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridNotes.Rows[e.Row].Tag is JobNote)) {
				return;//should never happen.
			}
			JobNote jobNote=(JobNote)gridNotes.Rows[e.Row].Tag;
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			FormJNE.ShowDialog();
			if(FormJNE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormJNE._jobNote==null) {//delete from in memory list.
				_jobCur.ListJobNotes.RemoveAt(e.Row);
			}
			else {//update in memory list
				_jobCur.ListJobNotes[e.Row]=FormJNE._jobNote;
			}
			FillGridNote();
		}



	}

}
	

