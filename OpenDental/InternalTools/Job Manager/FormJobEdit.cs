using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobEdit:Form {
		private Job _job;
		///<summary>The current mode that this Job Edit window is to be displayed in.</summary>
		private EditMode _editMode;

		///<summary>Creates a new job.</summary>
		public FormJobEdit():this(0,0) {
			
		}

		///<summary>Pass in the jobNum for existing jobs, or 0 for new jobs.</summary>
		public FormJobEdit(long jobNum):this(jobNum,0) {

		}

		///<summary>Pass in the jobNum for existing jobs, or 0 for new jobs.  Pass in a projectNum for a new job if you want to set it by default.</summary>
		public FormJobEdit(long jobNum,long projectNum) {
			if(jobNum==0) {
				_job=new Job();
				_job.ProjectNum=projectNum;
				_job.Owner=Security.CurUser.UserNum;
				Jobs.Insert(_job);
				_job.IsNew=true;
			}
			else {
				_job=Jobs.GetOne(jobNum);
			}
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobEdit_Load(object sender,EventArgs e) {
			#region Set Mode
			switch(_job.JobStatus) {
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
				case JobStatus.Documented:
				case JobStatus.QuestionForEngineers:
				case JobStatus.QuestionForManager:
					_editMode=EditMode.Documentation;
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
			textJobNum.Text=_job.JobNum.ToString();	//set JobNum
			//load comboboxes with enums
			for(int i=0;i<Enum.GetNames(typeof(JobPriority)).Length;i++) {
				comboPriority.Items.Add(Lan.g("enumJobPriority",Enum.GetNames(typeof(JobPriority))[i]));
			}
			comboPriority.SelectedIndex=(int)_job.JobPriority;
			for(int i=0;i<Enum.GetNames(typeof(JobType)).Length;i++) {
				comboType.Items.Add(Lan.g("enumJobType",Enum.GetNames(typeof(JobType))[i]));
			}
			comboType.SelectedIndex=(int)_job.JobType;
			textStatus.Text=_job.JobStatus.ToString();
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
				textNotes.Text=_job.Notes.ToString();
			}
			#endregion
			#region Evaluate Permissions
			//Concept Edit Mode
			if(_editMode==EditMode.Concept) {
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
				&& Security.IsAuthorized(Permissions.JobApproval,true)) 
			{
				butAction1.Text="Send to Expert";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butReviews.Enabled=false;
				textVersion.ReadOnly=true;
				comboType.Enabled=false;
				textActualHours.ReadOnly=true;
			}
			//Expert Edit Mode
			else if(_editMode==EditMode.Expert
				&& Security.IsAuthorized(Permissions.JobEdit,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				butAction1.Text="Under Construction";
				butAction2.Text="Ready to Assign";
				butAction3.Text="Send to Engineer";
				butAction4.Visible=false;
				butPickProject.Enabled=false;
				butReviews.Enabled=false;
				comboType.Enabled=false;
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
				butPickProject.Enabled=false;
				comboType.Enabled=false;
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
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
			}
			//Documentation Edit Mode
			else if(_editMode==EditMode.Documentation && Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				butAction1.Text="Question For Engineers";
				butAction2.Text="Question For Manager";
				butAction3.Text="Documented";
				butAction4.Visible=false;
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
			}
			else if(_editMode==EditMode.Documentation && !Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				butAction1.Text="Send to Documentation";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
			}
			//Done Edit Mode
			else if(_editMode==EditMode.Done && !Security.IsAuthorized(Permissions.JobApproval,true)) {
				groupActions.Visible=false;
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
				butLinks.Enabled=false;
				textNotes.ReadOnly=true;
			}
			else if(_editMode==EditMode.Done && Security.IsAuthorized(Permissions.JobApproval,true)) {
				butAction1.Text="Unlock Job";
				butAction2.Visible=false;
				butAction3.Visible=false;
				butAction4.Visible=false;
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				textNotes.ReadOnly=true;
			}
			//Read Only Mode
			else {
				groupActions.Visible=false;
				_editMode=EditMode.ReadOnly;
				butPickProject.Enabled=false;
				comboType.Enabled=false;
				comboPriority.Enabled=false;
				textTitle.ReadOnly=true;
				textEstHours.ReadOnly=true;
				textDescription.ReadOnly=true;
				textActualHours.ReadOnly=true;
				textVersion.ReadOnly=true;
				butReviews.Enabled=false;
				butLinks.Enabled=false;
				textNotes.ReadOnly=true;
				butOK.Enabled=false;
			}
			if(!_job.IsNew && !Security.IsAuthorized(Permissions.JobApproval,true)) {
				butDelete.Enabled=false;
			}
			#endregion
		}

		private void butPickProject_Click(object sender,EventArgs e) {
			FormJobProjectSelect FormJPS=new FormJobProjectSelect();
			if(FormJPS.DialogResult==DialogResult.OK) {
				_job.ProjectNum=FormJPS.SelectedProject.JobProjectNum; //project 
				textProject.Text=FormJPS.SelectedProject.Title; //project 
			}
		}

		private void butLinkTask_Click(object sender,EventArgs e) {
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
			_job.JobPriority=(JobPriority)comboPriority.SelectedIndex; //priority
			_job.JobType=(JobType)comboType.SelectedIndex; //type
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
			_job.Notes=textNotes.Text;
		}

		private void butAction1_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send For Approval
			if(_editMode==EditMode.Concept) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobApproval,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsApproval);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Send To Expert
			else if(_editMode==EditMode.Approval
				&& Security.IsAuthorized(Permissions.JobApproval,true)) 
			{
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobEdit,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				_job.Expert=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsExpertDefinition);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Under Construction
			else if(_editMode==EditMode.Expert
				&& Security.IsAuthorized(Permissions.JobEdit,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.UnderConstruction);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region In Progress
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.InProgress);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Questions For Engineers
			else if(_editMode==EditMode.Documentation && Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobManager,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.QuestionForEngineers);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Send To Documentation
			else if(_editMode==EditMode.Documentation && !Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobDocumentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Unlock Job
			else if(_editMode==EditMode.Done && Security.IsAuthorized(Permissions.JobApproval,true)) {
				MsgBox.Show(this,"Not Yet Implemented");
				return;
			}
			#endregion
		}

		private void butAction2_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Ready To Assign
			if(_editMode==EditMode.Expert
				&& Security.IsAuthorized(Permissions.JobEdit,true)
				&& _job.Expert==Security.CurUser.UserNum) 
			{
				Jobs.SetStatus(_job,JobStatus.ReadyToAssign);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Ready For Review
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.OnHold);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Question For Manager
			else if(_editMode==EditMode.Documentation && Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobApproval,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.QuestionForManager);
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
				&& Security.IsAuthorized(Permissions.JobEdit,true)
				&& _job.Expert==Security.CurUser.UserNum) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobManager,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.NeedsExpertDefinition);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
			#region Ready For Review
			else if(_editMode==EditMode.Engineer) {
				Jobs.SetStatus(_job,JobStatus.ReadyForReview);
				DialogResult=DialogResult.OK;
			}
			#endregion
			#region Documented
			else if(_editMode==EditMode.Documentation && Security.IsAuthorized(Permissions.JobDocumentation,true)) {
				Jobs.SetStatus(_job,JobStatus.Documented);
				DialogResult=DialogResult.OK;
			}
			#endregion
		}

		private void butAction4_Click(object sender,EventArgs e) {
			PrepareForAction();
			List<Userod> listUsersForPicker=new List<Userod>();
			#region Send To Documentation
			if(_editMode==EditMode.Engineer) {
				listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobDocumentation,false);
				FormUserPick FormUP=new FormUserPick();
				FormUP.IsSelectionmode=true;
				FormUP.ListUser=listUsersForPicker;
				if(FormUP.ShowDialog()!=DialogResult.OK) {
					return;
				}
				_job.Owner=FormUP.SelectedUserNum;
				Jobs.SetStatus(_job,JobStatus.ReadyToBeDocumented);
				DialogResult=DialogResult.OK;
				return;
			}
			#endregion
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this job will delete all JobEvents"
				+" and pointers to other tasks, features, and bugs. Are you sure you want to continue?")) {
				try { //Jobs.Delete will throw an application exception if there are any reviews associated with this job.
					Jobs.Delete(_job.JobNum);
					Signalods.SetInvalid(InvalidType.Job);
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
			_job.JobPriority=(JobPriority)comboPriority.SelectedIndex; //priority
			_job.JobType=(JobType)comboType.SelectedIndex; //type
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
			_job.DateTimeEntry=PIn.DateT(textDateEntry.Text); //date entry
			_job.Title=textTitle.Text; //title
			_job.Description=textDescription.Text; //description
			_job.Notes=textNotes.Text;
			Jobs.Update(_job);
			Signalods.SetInvalid(InvalidType.Job);
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
			///<summary>5 - Jobs that have been finished, documented, and all customers notified.  Can also mean "deleted".</summary>
			Done,
			///<summary>6 - Typically a user without the correct permission simply viewing an old job.</summary>
			ReadOnly
		}
	}
}