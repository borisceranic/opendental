using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;

namespace OpenDental {
	public partial class FormJobEdit:Form {
		private Job _job;
		///<summary>The current mode that this Job Edit window is to be displayed in.</summary>
		private EditMode _editMode;
		private List<JobNote> _jobNotes;
		private List<JobEvent> _jobEvents;
		private List<JobReview> _jobReviews;
		private List<JobLink> _jobLinks;
		private bool _isOverridden=false;
		private bool _hasChanged=false;
		private Userod _prevOwner;
		private Job _jobOld;

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
			_jobOld=_job.Copy();
			#region Set Mode
			switch(_job.Status) {
				case JobStatus.Concept:
					_editMode=EditMode.Concept;
					break;
				case JobStatus.NeedsConceptApproval:
					_editMode=EditMode.ConceptApproval;
					break;
				case JobStatus.ConceptApproved:
				case JobStatus.CurrentlyWriting:
					_editMode=EditMode.ExpertDefinition;
					break;
				case JobStatus.NeedsJobApproval:
					_editMode=EditMode.JobApproval;
					break;
				case JobStatus.NeedsJobClarification:
					_editMode=EditMode.JobClarify;
					break;
				case JobStatus.JobApproved:
				case JobStatus.OnHoldExpert:
				case JobStatus.ReadyToAssign:
					_editMode=EditMode.AssignToEngineer;
					break;
				case JobStatus.Assigned:
				case JobStatus.CurrentlyWorkingOn:
				case JobStatus.ReadyForReview:
				case JobStatus.OnHoldEngineer:
					_editMode=EditMode.Engineer;
					break;
				case JobStatus.ReadyToBeDocumented:
				case JobStatus.NeedsDocumentationClarification:
					_editMode=EditMode.Documentation;
					break;
				case JobStatus.NotifyCustomer:
					_editMode=EditMode.NotifyCustomer;
					break;
				case JobStatus.Rescinded:
				case JobStatus.Complete:
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
			_jobEvents=JobEvents.GetForJob(_job.JobNum);
			FillGridNote();
			FillGridLink();
			FillGridReviews();
			FillGridHistory();
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
				try {
					textEditorMain.MainRtf=_job.Description.ToString(); //This is here to convert our old job descriptions to the new RTF descriptions.
				}
				catch {
					textEditorMain.MainText=_job.Description.ToString();
				}
				_prevOwner=JobEvents.GetPrevOwner(_job.JobNum);
				if(_prevOwner!=null) {
					textPrevOwner.Text=_prevOwner.UserName;
				}
			}
			this.Text="Job Edit: "+textProject.Text+" - "+textTitle.Text;
			#endregion
			#region Evaluate Permissions
			if(JobRoles.IsAuthorized(JobRoleType.Override,true)) {
				butOverride.Visible=true;
			}
			//Concept Edit Mode
			if(_editMode==EditMode.Concept
				&& JobRoles.IsAuthorized(JobRoleType.Concept,true)) 
			{
				butAction1.Text="Send For Approval";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butAddReview.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//ConceptApproval Edit Mode
			else if(_editMode==EditMode.ConceptApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				butAction1.Text="Approve Concept";
				butAction2.Text="Ask For Clarification";
				butAction3.Visible=false;
				butAction4.Visible=false;
				butAddReview.Enabled=false;
				textVersion.ReadOnly=true;
				comboCategory.Enabled=false;
				textActualHours.ReadOnly=true;
			}
			//ExpertDefinition Edit Mode
			else if(_editMode==EditMode.ExpertDefinition
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				butAction1.Text="Send For Approval";
				butAction2.Text="Currently Writing";
				butAction3.Text="Send For Clarification";
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				butAddReview.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//JobApproval Edit Mode
			else if(_editMode==EditMode.JobApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				butAction1.Text="Approve Job";
				butAction2.Text="Assign To Engineer";
				butAction3.Text="Ask For Clarification";
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				butAddReview.Enabled=false;
				textVersion.ReadOnly=true;
				comboCategory.Enabled=false;
				textActualHours.ReadOnly=true;
			}
			//JobClarify Edit Mode
			else if(_editMode==EditMode.JobClarify
				&& JobRoles.IsAuthorized(JobRoleType.Concept,true)) {
				string prevOwnerName="Previous Owner";//This should never display, but I set it just in case
				if(_prevOwner!=null) {
					prevOwnerName=_prevOwner.UserName;
				}
				butAction1.Text="Send Back To "+prevOwnerName;
				if(_job.Expert==Security.CurUser.UserNum) {
					butAction2.Text="Change to Edit Mode";
				}
				else {
					butAction2.Visible=false;
				}
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
				butOK.Enabled=false;
				butRemove.Enabled=false;
				butLinkTask.Enabled=false;
				butLinkBug.Enabled=false;
				butLinkQuote.Enabled=false;
				butLinkFeatReq.Enabled=false;
			}
			//AssignToEngineer Edit Mode
			else if(_editMode==EditMode.AssignToEngineer
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				butAction1.Text="Assign To Engineer";
				butAction2.Text="Put On Hold";
				butAction3.Text="Ready To Assign";
				butAction4.Visible=false;
				if(_job.Expert!=Security.CurUser.UserNum && _job.Status==JobStatus.ReadyToAssign) {
					textEditorMain.ReadOnly=true;
					butAction2.Enabled=false;
				}
				butProjectPick.Enabled=false;
				butAddReview.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//AssignToEngineer Edit Mode (Other Engineer)
			else if(_editMode==EditMode.AssignToEngineer
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert!=Security.CurUser.UserNum 
				&& _job.Status==JobStatus.ReadyToAssign)
			{
				butAction1.Text="Assign To Engineer";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				textEditorMain.ReadOnly=true;
				butProjectPick.Enabled=false;
				butAddReview.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textVersion.ReadOnly=true;
				textActualHours.ReadOnly=true;
			}
			//Engineer Edit Mode
			else if(_editMode==EditMode.Engineer && _job.Owner==Security.CurUser.UserNum) {
				butAction1.Text="Send To Tech Writer";
				butAction2.Text="Currently Working On";
				butAction3.Text="Put On Hold";
				butAction4.Text="Ready For Review";
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				if(_job.Expert!=Security.CurUser.UserNum) {
					textTitle.ReadOnly=true;
					textEstHours.ReadOnly=true;
					textEditorMain.ReadOnly=true;
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
				butAction1.Text="Send To Customer Relat.";
				butAction2.Text="Ask For Clarification";
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
			}
			else if(_editMode==EditMode.Documentation && !JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				butAction1.Text="Send to Tech Writer";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
			}
			//Notify the Customer mode
			else if(_editMode==EditMode.NotifyCustomer
				&& JobRoles.IsAuthorized(JobRoleType.NotifyCustomer,true)) 
			{
				butAction1.Text="Mark Complete";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butProjectPick.Enabled=false;
				comboCategory.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
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
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
				butAddReview.Enabled=false;
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
				textEditorMain.ReadOnly=true;
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
				textEditorMain.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butAddReview.Enabled=false;
				butOK.Enabled=false;
				butRemove.Enabled=false;
				butLinkTask.Enabled=false;
				butLinkBug.Enabled=false;
				butLinkQuote.Enabled=false;
				butLinkFeatReq.Enabled=false;
			}
			if(!_job.IsNew && !JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				butDelete.Enabled=false;
			}
			if(_editMode==EditMode.Concept && Security.CurUser.UserNum==JobCur.Owner) {
				butDelete.Enabled=true;
			}
			#endregion
		}

		private void butProjectPick_Click(object sender,EventArgs e) {
			FormJobProjectSelect FormJPS=new FormJobProjectSelect();
			if(FormJPS.ShowDialog()==DialogResult.OK) {
				_job.ProjectNum=FormJPS.SelectedProject.JobProjectNum; //project 
				textProject.Text=FormJPS.SelectedProject.Title; //project 
			}
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
			_job.Description=textEditorMain.MainRtf; //description
		}

		private void butAction1_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker = new List<Userod>();
			#region Send For Approval
			if(_editMode==EditMode.Concept) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Approval,false);
				FormUserPick FormUP = new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select a Job Manager";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner = FormUP.SelectedUserNum;
				_job.HoursEstimate=PIn.Int(textEstHours.Text);
				Jobs.SetStatus(_job,JobStatus.NeedsConceptApproval,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Approve Concept
			else if(_editMode==EditMode.ConceptApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
				FormUserPick FormUP = new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_prevOwner.UserNum;
				FormUP.Text="Select an Expert";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner = FormUP.SelectedUserNum;
				_job.Expert=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ConceptApproved,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Send For Approval (Expert)
			else if(_editMode==EditMode.ExpertDefinition
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Approval,false);
				FormUserPick FormUP = new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select a Job Manager";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner = FormUP.SelectedUserNum;
				_job.HoursEstimate=PIn.Int(textEstHours.Text);
				Jobs.SetStatus(_job,JobStatus.NeedsJobApproval,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Approve Job
			else if(_editMode==EditMode.JobApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_job.Expert;
				FormUP.Text="Select an Expert";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				_job.Expert=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.JobApproved,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Send Back To *USERNAME*
			else if(_editMode==EditMode.JobClarify
				&& JobRoles.IsAuthorized(JobRoleType.Concept,true)) 
			{
				Jobs.SetStatus(_job,JobStatus.NeedsJobApproval,_prevOwner.UserNum);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Assign To Engineer
			else if(_editMode==EditMode.AssignToEngineer
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true))
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Engineer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select an Engineer";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner = FormUP.SelectedUserNum;
				_job.HoursEstimate=PIn.Int(textEstHours.Text);
				Jobs.SetStatus(_job,JobStatus.Assigned,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Send To Tech Writer
			else if(_editMode==EditMode.Engineer) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Documentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select a Tech Writer";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner = FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented,owner);
				_hasChanged=true;
				Close();
			}
			#endregion
			#region Send To Customer Relat.
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.NotifyCustomer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select a Customer Relations Coordinator";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NotifyCustomer,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Send To Tech Writer
			else if(_editMode==EditMode.Documentation && !JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Documentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_prevOwner.UserNum;
				FormUP.Text="Select a Tech Writer";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Mark Complete
			if(_editMode==EditMode.NotifyCustomer
				&& JobRoles.IsAuthorized(JobRoleType.NotifyCustomer,true)) 
			{
				Jobs.SetStatus(_job,JobStatus.Complete,_job.Owner);
				_hasChanged=true;
				Close();
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
			#region Ask For Clarification
			if(_editMode==EditMode.ConceptApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Concept,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(_prevOwner!=null) {
					FormUP.SelectedUserNum=_prevOwner.UserNum;
				}
				FormUP.Text="Select a Concept Writer";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.Concept,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Currently Writing
			if(_editMode==EditMode.ExpertDefinition
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.CurrentlyWriting,_job.Owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Assign to Engineer
			if(_editMode==EditMode.JobApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) {
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Engineer,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.Text="Select an Engineer";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				Jobs.SetStatus(_job,JobStatus.JobApproved,_job.Owner);
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.Assigned,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Change to Edit Mode
			if(_editMode==EditMode.JobClarify
				&& JobRoles.IsAuthorized(JobRoleType.Concept,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.ConceptApproved,_job.Owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Put On Hold
			if(_editMode==EditMode.AssignToEngineer
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) {
				Jobs.SetStatus(_job,JobStatus.OnHoldExpert,_job.Owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Currently Working On
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.CurrentlyWorkingOn,_job.Owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
			#region Ask For Clarification
			else if(_editMode==EditMode.Documentation && JobRoles.IsAuthorized(JobRoleType.Documentation,true)) {
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.SelectedUserNum=_job.Expert;
				FormUP.Text="Select a User";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsDocumentationClarification,owner);
				_hasChanged=true;
				Close();
				return;
			}
			#endregion
		}

		private void butAction3_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send For Clarification
			if(_editMode==EditMode.ExpertDefinition
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Approval,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_prevOwner.UserNum;
				FormUP.Text="Select a Job Manager";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsConceptApproval,owner);
				_hasChanged=true;
				Close();
			}
			#endregion
			#region Ask For Clarification
			else if(_editMode==EditMode.JobApproval
				&& JobRoles.IsAuthorized(JobRoleType.Approval,true)) 
			{
				listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Concept,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_job.Expert;
				FormUP.Text="Select a User";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				long owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsJobClarification,owner);
				_hasChanged=true;
				Close();
			}
			#endregion
			#region Ready To Assign
			else if(_editMode==EditMode.AssignToEngineer
				&& JobRoles.IsAuthorized(JobRoleType.Writeup,true)) 
			{
				Jobs.SetStatus(_job,JobStatus.ReadyToAssign,_job.Owner);
				_hasChanged=true;
				Close();
			}
			#endregion
			#region Put On Hold
			else if(_editMode==EditMode.Engineer&&_job.Owner==Security.CurUser.UserNum) {
				Jobs.SetStatus(_job,JobStatus.OnHoldEngineer,_job.Owner);
				_hasChanged=true;
				Close();
			}
			#endregion
		}

		private void butAction4_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Ready For Review
			if(_editMode==EditMode.Engineer && _job.Owner==Security.CurUser.UserNum) {
				listUsersForPicker = Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				FormUP.SelectedUserNum=_job.Expert;
				FormUP.Text="Select an Expert";
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				FormJobReviewEdit FormJRE=new FormJobReviewEdit(_job.JobNum,FormUP.SelectedUserNum);
				if(FormJRE.ShowDialog()==DialogResult.OK) {
					FormJRE.JobReviewCur.IsNew=false;
					_jobReviews.Add(FormJRE.JobReviewCur);
					FillGridReviews();
					Jobs.SetStatus(_job,JobStatus.ReadyForReview,_job.Owner);
					_hasChanged=true;
					Close();
				}
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
			_hasChanged=true;
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
			_hasChanged=true;
		}

		private void butOverride_Click(object sender,EventArgs e) {
			groupActions.Visible=false;
			comboStatus.Enabled=true;
			comboPriority.Enabled=true;
			comboCategory.Enabled=true;
			butProjectPick.Enabled=true;
			butExpertPick.Enabled=true;
			butOwnerPick.Enabled=true;
			textTitle.ReadOnly=false;
			textActualHours.ReadOnly=false;
			textEstHours.ReadOnly=false;
			textVersion.ReadOnly=false;
			butAddNote.Enabled=true;
			butOK.Enabled=true;
			butAddReview.Enabled=true;
			butLinkBug.Enabled=true;
			butLinkQuote.Enabled=true;
			butLinkFeatReq.Enabled=true;
			butLinkTask.Enabled=true;
			butRemove.Enabled=true;
			textEditorMain.ReadOnly=true;
			_isOverridden=true;
		}

		#region Main Tab
		public void FillGridNote() {
			gridNotes.BeginUpdate();
			gridNotes.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Time"),120);
			gridNotes.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"User"),80);
			gridNotes.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),400);
			gridNotes.Columns.Add(col);
			gridNotes.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_jobNotes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobNotes[i].DateTimeNote.ToShortDateString()+" "+_jobNotes[i].DateTimeNote.ToShortTimeString());
				row.Cells.Add(Userods.GetName(_jobNotes[i].UserNum));
				row.Cells.Add(_jobNotes[i].Note);
				gridNotes.Rows.Add(row);
			}
			gridNotes.EndUpdate();
			gridNotes.ScrollToEnd();
		}

		private void butAddNote_Click(object sender,EventArgs e) {
			JobNote jobNote=new JobNote();
			jobNote.IsNew=true;
			jobNote.JobNum=_job.JobNum;
			jobNote.UserNum=Security.CurUser.UserNum;
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			if(FormJNE.ShowDialog()==DialogResult.OK) {
				_jobNotes.Add(FormJNE.JobNoteCur);
				FillGridNote();
			}
		}

		private void gridNotes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			JobNote jobNote=_jobNotes[gridNotes.GetSelectedIndex()];
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			if(FormJNE.ShowDialog()==DialogResult.OK) {
				if(FormJNE.JobNoteCur==null) {
					_jobNotes.RemoveAt(gridNotes.GetSelectedIndex());
				}
				FillGridNote();
			}
		}

		private void textEditorMain_SaveClick(object sender,EventArgs e) {
			_jobOld.Description=textEditorMain.MainRtf;
			Jobs.Update(_jobOld); //This will save all changes made to _job, but not yet committed to the DB.
		}

		#region Links Grid
		private void FillGridLink() {
			_jobLinks=JobLinks.GetJobLinks(_job.JobNum);
			long selectedLinkNum=0;
			if(gridLinks.GetSelectedIndex()!=-1) {
				selectedLinkNum=(long)gridLinks.Rows[gridLinks.GetSelectedIndex()].Tag;
			}
			gridLinks.BeginUpdate();
			gridLinks.Columns.Clear();
			gridLinks.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Type",70);
			gridLinks.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridLinks.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobLinks.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobLinks[i].LinkType.ToString());
				if(_jobLinks[i].LinkType==JobLinkType.Task) {
					Task task=Tasks.GetOne(_jobLinks[i].FKey);
					if(task==null) {//task was deleted
						continue;
					}
					if(task.Descript.Length>=80) {
						row.Cells.Add(task.Descript.Substring(0,80)+"...");
					}
					else {
						row.Cells.Add(task.Descript);
					}
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Bug) {
					row.Cells.Add("Under Construction");
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Request) {
					row.Cells.Add("Feature Request #"+_jobLinks[i].FKey);
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Quote) {
					JobQuote quote=JobQuotes.GetOne(_jobLinks[i].FKey);
					string quoteText="Amount: "+quote.Amount;
					if(quote.PatNum!=0) {
						Patient pat=Patients.GetPat(quote.PatNum);
						quoteText+="\r\nCustomer: "+pat.LName+", "+pat.FName;
					}
					if(quote.Note!="") {
						quoteText+="\r\nNote: "+quote.Note;
					}
					row.Cells.Add(quoteText);
				}
				row.Tag=_jobLinks[i].JobLinkNum;
				gridLinks.Rows.Add(row);
			}
			gridLinks.EndUpdate();
			for(int i=0;i<gridLinks.Rows.Count;i++) {
				if((long)gridLinks.Rows[i].Tag==selectedLinkNum) {
					gridLinks.SetSelected(i,true);
				}
			}
		}

		private void gridLinks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			switch(_jobLinks[gridLinks.SelectedIndices[0]].LinkType) {
				case JobLinkType.Task:
					Task task=Tasks.GetOne(_jobLinks[gridLinks.SelectedIndices[0]].FKey);
					FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
					FormTE.ShowDialog();
					FillGridLink();
					break;
				case JobLinkType.Request:
					FormRequestEdit FormRE=new FormRequestEdit();
					FormRE.RequestId=_jobLinks[gridLinks.SelectedIndices[0]].FKey;
					FormRE.IsAdminMode=true;
					FormRE.ShowDialog();
					FillGridLink();
					break;
				case JobLinkType.Bug:
					break;
				case JobLinkType.Quote:
					JobQuote quote=JobQuotes.GetOne(_jobLinks[gridLinks.SelectedIndices[0]].FKey);
					FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(quote);
					FormJQE.JobLinkNum=_jobLinks[gridLinks.SelectedIndices[0]].JobLinkNum;//Allows deletion of the link if the quote is deleted.
					FormJQE.ShowDialog();
					FillGridLink();
					break;
			}
		}

		private void butLinkBug_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butLinkTask_Click(object sender,EventArgs e) {
			FormTaskSearch FormTS=new FormTaskSearch();
			FormTS.IsSelectionMode=true;
			if(FormTS.ShowDialog()==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_job.JobNum;
				jobLink.FKey=FormTS.SelectedTaskNum;
				jobLink.LinkType=JobLinkType.Task;
				JobLinks.Insert(jobLink);
				_hasChanged=true;
				FillGridLink();
			}
		}

		private void butLinkFeatReq_Click(object sender,EventArgs e) {
			FormFeatureRequest FormFR=new FormFeatureRequest();
			FormFR.IsSelectionMode=true;
			FormFR.ShowDialog();
			if(FormFR.DialogResult==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_job.JobNum;
				jobLink.FKey=FormFR.SelectedFeatureNum;
				jobLink.LinkType=JobLinkType.Request;
				JobLinks.Insert(jobLink);
				_hasChanged=true;
				FillGridLink();
			}
		}

		private void butLinkQuote_Click(object sender,EventArgs e) {
			if(!JobRoles.IsAuthorized(JobRoleType.Quote)) {
				return;
			}
			JobQuote jobQuote=new JobQuote();
			jobQuote.IsNew=true;
			FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(jobQuote);
			if(FormJQE.ShowDialog()==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_job.JobNum;
				jobLink.FKey=FormJQE.JobQuoteCur.JobQuoteNum;
				jobLink.LinkType=JobLinkType.Quote;
				JobLinks.Insert(jobLink);
				_hasChanged=true;
				FillGridLink();
			}
		}

		private void butLinkRemove_Click(object sender,EventArgs e) {
			if(gridLinks.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Select a link to remove first.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to remove this link?")) {
				return;
			}
			JobLinks.Delete((long)gridLinks.Rows[gridLinks.GetSelectedIndex()].Tag);
			_hasChanged=true;
			FillGridLink();
		}
		#endregion

		#endregion

		#region History Tab
		private void FillGridHistory() {
			List<JobEvent> listjobEvents=JobEvents.GetForJob(_job.JobNum);
			long selectedEventNum=0;
			if(gridHistory.GetSelectedIndex()!=-1) {
				selectedEventNum=(long)gridHistory.Rows[gridHistory.GetSelectedIndex()].Tag;
			}
			gridHistory.BeginUpdate();
			gridHistory.Columns.Clear();
			gridHistory.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Date",140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn("Owner",100);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn("Status",200);
			gridHistory.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<listjobEvents.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listjobEvents[i].DateTimeEntry.ToShortDateString()+" "+listjobEvents[i].DateTimeEntry.ToShortTimeString());
				row.Cells.Add(Userods.GetName(listjobEvents[i].Owner));
				row.Cells.Add(Enum.GetName(typeof(JobStatus),(int)listjobEvents[i].Status));
				row.Tag=listjobEvents[i].JobEventNum;
				gridHistory.Rows.Add(row);
			}
			gridHistory.EndUpdate();
			for(int i=0;i<gridHistory.Rows.Count;i++) {
				if((long)gridHistory.Rows[i].Tag==selectedEventNum) {
					gridHistory.SetSelected(i,true);
				}
			}
		}

		private void gridHistory_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormJobHistoryView FormJHV=new FormJobHistoryView((long)gridHistory.Rows[e.Row].Tag);
			FormJHV.Show();
		}
		#endregion

		#region Reviews Tab
		private void FillGridReviews() {
			_jobReviews=JobReviews.GetForJob(_job.JobNum);
			long selectedReviewNum=0;
			if(gridReview.GetSelectedIndex()!=-1) {
				selectedReviewNum=(long)gridReview.Rows[gridReview.GetSelectedIndex()].Tag;
			}
			gridReview.BeginUpdate();
			gridReview.Columns.Clear();
			gridReview.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Date Last Edited",90);
			gridReview.Columns.Add(col);
			col=new ODGridColumn("Reviewer",80);
			gridReview.Columns.Add(col);
			col=new ODGridColumn("Status",80);
			gridReview.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridReview.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobReviews.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobReviews[i].DateTStamp.ToShortDateString());
				row.Cells.Add(Userods.GetName(_jobReviews[i].Reviewer));
				row.Cells.Add(Enum.GetName(typeof(JobReviewStatus),(int)_jobReviews[i].ReviewStatus));
				if(_jobReviews[i].Description.Length>=80) {
					row.Cells.Add(_jobReviews[i].Description.Substring(0,80)+"...");
				}
				else {
					row.Cells.Add(_jobReviews[i].Description);
				}
				row.Tag=_jobReviews[i].JobReviewNum;
				gridReview.Rows.Add(row);
			}
			gridReview.EndUpdate();
			for(int i=0;i<gridReview.Rows.Count;i++) {
				if((long)gridReview.Rows[i].Tag==selectedReviewNum) {
					gridReview.SetSelected(i,true);
				}
			}
		}

		private void gridReview_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			bool isReadOnly=false;
			if(_jobReviews[gridReview.GetSelectedIndex()].Reviewer!=Security.CurUser.UserNum) {
				isReadOnly=true;
				InputBox FormIB=new InputBox(Userods.GetName(_jobReviews[gridReview.GetSelectedIndex()].Reviewer));
				FormIB.setTitle("Log-in to Edit Review");
				FormIB.textResult.PasswordChar='*';
				if(FormIB.ShowDialog()==DialogResult.OK
					&& Userods.CheckTypedPassword(FormIB.textResult.Text,Userods.GetUser(_jobReviews[gridReview.GetSelectedIndex()].Reviewer).Password)) {
					isReadOnly=false;
				}
				else if(FormIB.DialogResult==DialogResult.Cancel) {
					//Do not show anything here since they simply want to see the read-only version of the review
				}
				else {
					MsgBox.Show(this,"Log-in Failed");
				}
			}
			FormJobReviewEdit FormJRE=new FormJobReviewEdit(_job.JobNum,_jobReviews[gridReview.GetSelectedIndex()],isReadOnly);
			if(FormJRE.ShowDialog()==DialogResult.OK) {
				FillGridReviews();
			}
		}

		private void butAddReview_Click(object sender,EventArgs e) {
			List<Userod> listUsersForPicker=Userods.GetUsersByJobRole(JobRoleType.Writeup,false);
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUser=listUsersForPicker;
			FormUP.SelectedUserNum=_job.Expert;
			FormUP.Text="Select an Expert";
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FormJobReviewEdit FormJRE=new FormJobReviewEdit(_job.JobNum,FormUP.SelectedUserNum);
			if(FormJRE.ShowDialog()==DialogResult.OK) {
				FormJRE.JobReviewCur.IsNew=false;
				_jobReviews.Add(FormJRE.JobReviewCur);
				FillGridReviews();
				_hasChanged=true;
			}
		}
		#endregion

		private void comboCategory_SelectionChangeCommitted(object sender,EventArgs e) {
			_hasChanged=true;
		}
		
		private void textTitle_KeyDown(object sender,KeyEventArgs e) {
			_hasChanged=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this job will delete all JobEvents"
				+" and pointers to other tasks, features, and bugs. Are you sure you want to continue?")) {
				try { //Jobs.Delete will throw an application exception if there are any reviews associated with this job.
					Jobs.Delete(_job.JobNum);
					_job=null;
					DataValid.SetInvalid(InvalidType.Jobs);
					Close();
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
			_job.Description=textEditorMain.MainRtf; //description
			_job.HoursEstimate=PIn.Int(textEstHours.Text);
			_job.HoursActual=PIn.Int(textActualHours.Text);
			if(_isOverridden) {
				_job.Status=(JobStatus)comboStatus.SelectedIndex;
			}
			Jobs.Update(_job);
			if(_isOverridden) {
				JobEvent jobEventCur=new JobEvent();
				textEditorMain.Text.Insert(0,"THIS JOB WAS MANUALLY OVERRIDDEN BY "+Security.CurUser.UserName+":\r\n");
				jobEventCur.Description=textEditorMain.MainRtf;
				jobEventCur.JobNum=_job.JobNum;
				jobEventCur.Status=_job.Status;
				jobEventCur.Owner=_job.Owner;
				JobEvents.Insert(jobEventCur);
			}
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(_job.IsNew) {
					try {
						Jobs.Delete(_job.JobNum);
					}
					catch(Exception ex) { //unlikely. this would only happen if someone created a job and then immediately attached reviews to it.
						MessageBox.Show(ex.Message);
						return;
					}
				}
			Close();
		}

		private void FormJobEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				Signalods.SetInvalidNoCache(InvalidType.Jobs);
			}
			JobHandler.JobFired-=ODEvent_Fired;
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			_jobNotes=JobNotes.GetForJob(_job.JobNum);
			FillGridNote();
		}

		///<summary>The many different modes that the Job Edit window can be put in.</summary>
		private enum EditMode {
			///<summary>0 - An idea for a job with a rough or refined definition that needs approval.</summary>
			Concept,
			///<summary>1 - All jobs that need the attention of a user that has job approval access for the intial concept review.</summary>
			ConceptApproval,
			///<summary>2 - Pending jobs for experts.  They can be anything from still writing up to ready to assign.</summary>
			ExpertDefinition,
			///<summary>3 - All jobs that need the attention of a user that has job approval access for the final review of the writeup.</summary>
			JobApproval,
			///<summary>4 - Jobs that need clarification before final approval.</summary>
			JobClarify,
			///<summary>5 - Pending jobs for experts to assign out.</summary>
			AssignToEngineer,
			///<summary>6 - Jobs that are in queue or are being actively worked on by an engineer.</summary>
			Engineer,
			///<summary>7 - In the process of being documented or in limbo with additional information needed for documenting purposes.</summary>
			Documentation,
			///<summary>8 - Final step is to notify the customer of the changes made.</summary>
			NotifyCustomer,
			///<summary>9 - Jobs that have been finished, documented, and all customers notified.  Can also mean "deleted".</summary>
			Done,
			///<summary>10 - Typically a user without the correct permission simply viewing an old job.</summary>
			ReadOnly
		}
	}
}