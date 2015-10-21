using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormJobEdit:Form {
		private Job _job;
		///<summary>The current mode that this Job Edit window is to be displayed in.</summary>
		private EditMode _editMode;
		private List<JobNote> _jobNotes;
		private bool _isOverridden=false;

		///<summary>Creates a new job.</summary>
		public FormJobEdit():this(0,0) {
			
		}

		///<summary>Pass in the jobNum for existing jobs, or 0 for new jobs.</summary>
		public FormJobEdit(long jobNum):this(jobNum,0) {

		}

		///<summary>Pass in the jobNum for existing jobs, or 0 for new jobs.  Pass in a projectNum for a new job if you want to set it by default.</summary>
		public FormJobEdit(long jobNum,long projectNum) {
			JobHandler.JobFired+=ODEvent_Fired;
			if(jobNum==0) {
				_job=new Job();
				_job.ProjectNum=projectNum;
				_job.Owner=Security.CurUser.UserNum;
				_job.Priority=JobPriority.Medium;
				_job.Status=JobStatus.Concept;
				Jobs.Insert(_job);
				_job.IsNew=true;
			}
			else {
				_job=Jobs.GetOne(jobNum);
			}
			InitializeComponent();
			Lan.F(this);
		}

		public Job JobCur {
			get {
				return _job;
			}
		}

		private void FormJobEdit_Load(object sender,EventArgs e) {
			#region Set Mode
			switch(_job.Status) {
				case JobStatus.Concept:
					_editMode=EditMode.Concept;
					break;
				case JobStatus.NeedsApproval:
					_editMode=EditMode.Approval;
					break;
				case JobStatus.NeedsExpertDefinition:
				case JobStatus.UnderConstruction:
				case JobStatus.ReadyToAssign:
					_editMode=EditMode.Expert;
					break;
				case JobStatus.Assigned:
				case JobStatus.InProgress:
				case JobStatus.ReadyForReview:
				case JobStatus.OnHold:
					_editMode=EditMode.Engineer;
					break;
				case JobStatus.ReadyToBeDocumented:
				case JobStatus.QuestionForEngineers:
				case JobStatus.QuestionForManager:
					_editMode=EditMode.Documentation;
					break;
				case JobStatus.Documented:
					_editMode=EditMode.NotifyCustomer;
					break;
				case JobStatus.Rescinded:
				case JobStatus.Done:
				case JobStatus.Deleted:
					_editMode=EditMode.Done;
					break;
				default:
					_editMode=EditMode.ReadOnly;
					break;
			}
			#endregion
			#region Fill Controls
			_jobNotes=JobNotes.GetForJob(_job.JobNum);
			FillGrid();
			textJobNum.Text=_job.JobNum.ToString();	//set JobNum
			//load comboboxes with enums
			for(int i=0;i<Enum.GetNames(typeof(JobPriority)).Length;i++) {
				comboPriority.Items.Add(Lan.g("enumJobPriority",Enum.GetNames(typeof(JobPriority))[i]));
			}
			comboPriority.SelectedIndex=(int)_job.Priority;
			for(int i=0;i<Enum.GetNames(typeof(JobCategory)).Length;i++) {
				comboCategory.Items.Add(Lan.g("enumJobType",Enum.GetNames(typeof(JobCategory))[i]));
			}
			comboCategory.SelectedIndex=(int)_job.Category;
			for(int i=0;i<Enum.GetNames(typeof(JobStatus)).Length;i++) {
				comboStatus.Items.Add(Lan.g("enumJobStatus",Enum.GetNames(typeof(JobStatus))[i]));
			}
			comboStatus.SelectedIndex=(int)_job.Status;
			JobProject project=JobProjects.GetOne(_job.ProjectNum);
			if(project!=null) {
				textProject.Text=PIn.String(project.Title); //project
			}
			if(!_job.IsNew) { //load Job information. Skip if job is new.
				Userod expert=Userods.GetUser(_job.Expert);
				Userod owner=Userods.GetUser(_job.Owner);
				if(expert!=null) {
					textExpert.Text=expert.UserName;
				}
				if(owner!=null) {
					textOwner.Text=owner.UserName;
				}
				textVersion.Text=_job.JobVersion;	//version
				textEstHours.Text=_job.HoursEstimate.ToString(); //est hours
				textActualHours.Text=_job.HoursActual.ToString(); //actual hours
				textDateEntry.Text=_job.DateTimeEntry.ToShortDateString(); //date entry
				textTitle.Text=_job.Title.ToString(); //title
				textDescription.Text=_job.Description.ToString(); //description
			}
			#endregion
			#region Evaluate Permissions
			if(JobRoles.IsAuthorized(JobRoleType.Override,true)) {
				butOverride.Visible=true;
			}
			//Concept Edit Mode
			if(_editMode==EditMode.Concept
				&& JobRoles.IsAuthorized(JobRoleType.Concept,true)) 
			{
				butAction1.Text="Send for Approval";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butReviews.Enabled=false;
				butHistory.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//Approval Edit Mode
			else if(_editMode==EditMode.Approval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				butAction1.Text="Send to Expert";
				butAction2.Text="Send for Re-write";
				butAction3.Visible=false;
				butAction4.Visible=false;
				butReviews.Enabled=false;
				textVersion.ReadOnly=true;
				comboCategory.Enabled=false;
				textActualHours.ReadOnly=true;
			}
			//Expert Edit Mode
			else if(_editMode==EditMode.Expert
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				butAction1.Text="Under Construction";
				butAction2.Text="Ready to Assign";
				butAction3.Text="Send to Engineer";
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				butReviews.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//Engineer Edit Mode
			else if(_editMode==EditMode.Engineer && _job.Owner==Security.CurUser.UserNum) {
				butAction1.Text="In Progress";
				butAction2.Text="On Hold";
				butAction3.Text="Ready For Review";
				butAction4.Text="Send to Documentation";
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				if(_job.Expert!=Security.CurUser.UserNum) {
					textTitle.ReadOnly=true;
					textEstHours.ReadOnly=true;
					textDescription.ReadOnly=true;
				}
			}
			//Engineer Edit Mode by Expert
			else if(_editMode==EditMode.Engineer && _job.Expert==Security.CurUser.UserNum) {
				groupActions.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
			}
			//Documentation Edit Mode
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				butAction1.Text="Question For Engineers";
				butAction2.Text="Question For Manager";
				butAction3.Text="Documented";
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
			}
			else if(_editMode==EditMode.Documentation && !JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				butAction1.Text="Send to Documentation";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
			}
			//Notify the Customer mode
			else if(_editMode==EditMode.NotifyCustomer
				&& JobRoles.IsAuthorized(JobRoleType.NotifyCustomer,true)) 
			{
				butAction1.Text="Customer Notified";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
				butOK.Enabled=false;
			}
			//Done Edit Mode
			else if(_editMode==EditMode.Done && !JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				groupActions.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
				butLinks.Enabled=false;
				butAddNote.Enabled=false;
			}
			else if(_editMode==EditMode.Done && JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				butAction1.Text="Unlock Job";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddNote.Enabled=false;
			}
			//Read Only Mode
			else {
				groupActions.Visible=false;
				_editMode=EditMode.ReadOnly;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
				butLinks.Enabled=false;
				butAddNote.Enabled=false;
				butOK.Enabled=false;
			}
			if(!_job.IsNew && !JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				butDelete.Enabled=false;
			}
			#endregion
		}

		public void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Time"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"User"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),400);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_jobNotes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobNotes[i].DateTimeNote.ToShortDateString()+" "+_jobNotes[i].DateTimeNote.ToShortTimeString());
				row.Cells.Add(Userods.GetName(_jobNotes[i].UserNum));
				row.Cells.Add(_jobNotes[i].Note);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}

		private void butAddNote_Click(object sender,EventArgs e) {
			JobNote jobNote=new JobNote();
			jobNote.IsNew=true;
			jobNote.JobNum=_job.JobNum;
			jobNote.UserNum=Security.CurUser.UserNum;
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			if(FormJNE.ShowDialog()==DialogResult.OK) {
				_jobNotes.Add(FormJNE.JobNoteCur);
				FillGrid();
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			JobNote jobNote=_jobNotes[gridMain.GetSelectedIndex()];
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			if(FormJNE.ShowDialog()==DialogResult.OK) {
				if(FormJNE.JobNoteCur==null) {
					_jobNotes.RemoveAt(gridMain.GetSelectedIndex());
				}
				FillGrid();
			}
		}

		private void butProjectPick_Click(object sender,EventArgs e) {
			FormJobProjectSelect FormJPS=new FormJobProjectSelect();
			if(FormJPS.ShowDialog()==DialogResult.OK) {
				_job.ProjectNum=FormJPS.SelectedProject.JobProjectNum; //project 
				textProject.Text=FormJPS.SelectedProject.Title; //project 
			}
		}

		private void butLinks_Click(object sender,EventArgs e) {
			FormJobLinks FormJL=new FormJobLinks(_job.JobNum);
			FormJL.ShowDialog();
		}

		private void butReviews_Click(object sender,EventArgs e) {
			FormJobReviews FormJR=new FormJobReviews(_job.JobNum);
			FormJR.ShowDialog();
		}

		private void butHistory_Click(object sender,EventArgs e) {
			FormJobHistory FormJH=new FormJobHistory(_job.JobNum);
			FormJH.ShowDialog();
		}

		private void PrepareForAction() {
			_job.Priority=(JobPriority)comboPriority.SelectedIndex; //priority
			_job.Category=(JobCategory)comboCategory.SelectedIndex; //type
			_job.JobVersion=textVersion.Text; //version
			try {//in case the user enters letters or symbols into the text box.
				_job.HoursEstimate=PIn.Int(textEstHours.Text); //est hours
			}
			catch {
				MsgBox.Show(this,"You have entered an invalid number of estimated hours. Please correct this before continuing."); //est hours
				return;
			}
			try {//in case the user enters letters or symbols into the text box.
				_job.HoursActual=PIn.Int(textActualHours.Text); //actual hours
			}
			catch {
				MsgBox.Show(this,"You have entered an invalid number of actual hours. Please correct this before continuing."); //actual hours
				return;
			}
			_job.Title=textTitle.Text; //title
			_job.Description=textDescription.Text; //description
		}

		private void butAction1_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send For Approval
			if(_editMode==EditMode.Concept) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Approval,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				_job.HoursEstimate=PIn.Int(textEstHours.Text);
				Jobs.SetStatus(_job,JobStatus.NeedsApproval,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Send To Expert
			else if(_editMode==EditMode.Approval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				_job.Expert=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsExpertDefinition,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Under Construction
			else if(_editMode==EditMode.Expert
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.UnderConstruction,_job.Owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region In Progress
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.InProgress,_job.Owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Questions For Engineers
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Engineer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.QuestionForEngineers,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Send To Documentation
			else if(_editMode==EditMode.Documentation && !JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Documentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Customer Notified
			if(_editMode==EditMode.NotifyCustomer
				&& JobRoles.IsAuthorized(JobRoleType.NotifyCustomer,true)) 
			{
				Jobs.SetStatus(_job,JobStatus.Done,_job.Owner);
				DialogResult=DialogResult.OK;
			}
			#endregion
			#region Unlock Job
			else if(_editMode==EditMode.Done && JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				MsgBox.Show(this,"Not Yet Implemented");
				return;
			}
			#endregion
		}

		private void butAction2_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send for Re-write
			if(_editMode==EditMode.Approval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Concept,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.Concept,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Ready To Assign
			if(_editMode==EditMode.Expert
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.ReadyToAssign,_job.Owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Ready For Review
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.OnHold,_job.Owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Question For Manager
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Approval,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.QuestionForManager,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
		}

		private void butAction3_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send To Engineer
			if(_editMode==EditMode.Expert
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) {
					listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Engineer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.Assigned,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Ready For Review
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.ReadyForReview,_job.Owner);
				DialogResult=DialogResult.OK;
			}
			#endregion
			#region Documented
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.NotifyCustomer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.Documented,owner);
				DialogResult=DialogResult.OK;
			}
			#endregion
		}

		private void butAction4_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send To Documentation
			if(_editMode==EditMode.Engineer) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Documentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented,owner);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
		}

		private void butOwnerPick_Click(object sender,EventArgs e) {
			List<Userod> listUsersForPicker=Userods.GetUsers(false);
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUser=listUsersForPicker;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_job.Owner=FormUP.SelectedUserNum;
			textOwner.Text=Userods.GetName(_job.Owner);
		}

		private void butExpertPick_Click(object sender,EventArgs e) {
			List<Userod> listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUser=listUsersForPicker;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_job.Expert=FormUP.SelectedUserNum;
			textExpert.Text=Userods.GetName(_job.Expert);
		}

		private void butOverride_Click(object sender,EventArgs e) {
			groupActions.Visible=false;
			comboStatus.Enabled=true;
			comboPriority.Enabled=true;
			comboCategory.Enabled=true;
			butProjectPick.Enabled=true;
			butExpertPick.Visible=true;
			butOwnerPick.Visible=true;
			textTitle.ReadOnly=false;
			textDescription.ReadOnly=false;
			textActualHours.ReadOnly=false;
			textEstHours.ReadOnly=false;
			textVersion.ReadOnly=false;
			butHistory.Enabled=true;
			butLinks.Enabled=true;
			butReviews.Enabled=true;
			butAddNote.Enabled=true;
			butOK.Enabled=true;
			butHistory.Enabled=true;
			butLinks.Enabled=true;
			butReviews.Enabled=true;
			_isOverridden=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this job will delete all JobEvents"
				+" and pointers to other tasks, features, and bugs. Are you sure you want to continue?")) {
				try { //Jobs.Delete will throw an application exception if there are any reviews associated with this job.
					Jobs.Delete(_job.JobNum);
					_job=null;
					DataValid.SetInvalid(InvalidType.Jobs);
					DialogResult=DialogResult.OK;
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//send Job information to database. No need to send jobNum, as it has already been set.
			if(textEstHours.errorProvider1.GetError(textEstHours)!="") {
				MsgBox.Show(this,"You have entered an invalid integer of estimated hours. Please correct this before continuing."); //est hours
				return;
			}
			if(textActualHours.errorProvider1.GetError(textActualHours)!="") {
				MsgBox.Show(this,"You have entered an invalid integer of actual hours. Please correct this before continuing."); //actual hours
				return;
			}
			_job.Priority=(JobPriority)comboPriority.SelectedIndex; //priority
			_job.Category=(JobCategory)comboCategory.SelectedIndex; //Category
			_job.JobVersion=textVersion.Text; //version
			_job.Title=textTitle.Text; //title
			_job.Description=textDescription.Text; //description
			_job.HoursEstimate=PIn.Int(textEstHours.Text);
			_job.HoursActual=PIn.Int(textActualHours.Text);
			if(_isOverridden) {
				_job.Status=(JobStatus)comboStatus.SelectedIndex;
			}
			Jobs.Update(_job);
			if(_isOverridden) {
				JobEvent jobEventCur=new JobEvent();
				jobEventCur.Description="THIS JOB WAS MANUALLY OVERRIDDEN BY "+Security.CurUser.UserName+":\r\n"+_job.Description;
				jobEventCur.JobNum=_job.JobNum;
				jobEventCur.Status=_job.Status;
				jobEventCur.Owner=_job.Owner;
				JobEvents.Insert(jobEventCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}

		private void FormJobEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult!=DialogResult.OK) { //to account for when the form is closed using the toolbar buttons.
				if(_job.IsNew) {
					try {
						Jobs.Delete(_job.JobNum);
					}
					catch(Exception ex) { //unlikely. this would only happen if someone created a job and then immediately attached reviews to it.
						MessageBox.Show(ex.Message);
						e.Cancel=true;
					}
				}
			}
			else {
				DataValid.SetInvalid(InvalidType.Jobs);
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			_jobNotes=JobNotes.GetForJob(_job.JobNum);
			FillGrid();
		}

		///<summary>The many different modes that the Job Edit window can be put in.</summary>
		private enum EditMode {
			///<summary>0 - An idea for a job with a rough or refined definition that needs approval.</summary>
			Concept,
			///<summary>1 - All jobs that need the attention of a user that has job approval access.</summary>
			Approval,
			///<summary>2 - Pending jobs for experts.  They can be anything from still writing up to ready to assign.</summary>
			Expert,
			///<summary>3 - Jobs that are in queue or are being actively worked on by an engineer.</summary>
			Engineer,
			///<summary>4 - In the process of being documented or in limbo with additional information needed for documenting purposes.</summary>
			Documentation,
			///<summary>5 - Final step is to notify the customer of the changes made.</summary>
			NotifyCustomer,
			///<summary>6 - Jobs that have been finished, documented, and all customers notified.  Can also mean "deleted".</summary>
			Done,
			///<summary>7 - Typically a user without the correct permission simply viewing an old job.</summary>
			ReadOnly
		}
	}
}