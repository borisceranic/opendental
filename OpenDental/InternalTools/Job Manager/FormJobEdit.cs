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
		private JobEvent _jobEventCur;
		private JobEvent _jobEventOld;

		///<summary>Creates a new job.</summary>
		public FormJobEdit():this(0) {
			
		}

		///<summary>Pass in the jobNum for existing jobs, or 0 for new jobs.</summary>
		public FormJobEdit(long jobNum) {
			if(jobNum==0) {
				_job=new Job();
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
			_jobEventOld=JobEvents.GetMostRecent(_job.JobNum);
			_jobEventCur=new JobEvent();
			_jobEventCur.JobNum=_job.JobNum;
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
			for(int i=0;i<Enum.GetNames(typeof(JobStatus)).Length;i++) {
				comboStatus.Items.Add(Lan.g("enumJobStatus",Enum.GetNames(typeof(JobStatus))[i]));
			}
			comboStatus.SelectedIndex=(int)_job.JobStatus;
			if(!_job.IsNew) { //load Job information. Skip if job is new.
				Userod expert=Userods.GetUser(_job.Expert);
				Userod owner=Userods.GetUser(_job.Owner);
				JobProject project=JobProjects.GetOne(_job.ProjectNum);
				if(expert!=null) {
					textExpert.Text=PIn.String(expert.UserName);	//expert
				}
				if(owner!=null) {
					textOwner.Text=PIn.String(owner.UserName);
				}
				textVersion.Text=PIn.String(_job.JobVersion);	//version
				textEstHours.Text=PIn.String(_job.HoursEstimate.ToString()); //est hours
				textActualHours.Text=PIn.String(_job.HoursActual.ToString()); //actual hours
				textDateEntry.Text=PIn.String(_job.DateTimeEntry.ToShortDateString()); //date entry
				if(project!=null) {
					textProject.Text=PIn.String(project.Description); //project
				}
				textTitle.Text=PIn.String(_job.Title.ToString()); //title
				textDescription.Text=PIn.String(_job.Description.ToString()); //description
				textNotes.Text=PIn.String(_job.Notes.ToString());
			}
			//check for permission or expert status and enable text fields
			if(Security.IsAuthorized(Permissions.JobApproval,true) || Security.CurUser.UserNum==_job.Expert) {
				textEstHours.ReadOnly=false;
				textDescription.ReadOnly=false;
			}
			//JobType should not be editable once the job has been created or if the user does not have JobApproval permission.
			if(!_job.IsNew && !Security.IsAuthorized(Permissions.JobApproval,true)) {
				comboType.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void butPickExpert_Click(object sender,EventArgs e) {
			FormUserPick FormUP=new FormUserPick();
			FormUP.ShowDialog();
			if(FormUP.DialogResult==DialogResult.OK) {
				_job.Expert=FormUP.SelectedUserNum;
				textExpert.Text=Userods.GetName(FormUP.SelectedUserNum);
			}
		}

		private void butPickProject_Click(object sender,EventArgs e) {
			//TODO: Set the projectnum similar to the PickExpert button
			//FormProjects FormProj=new FormProjects();
			//if(FormProj.DialogResult==DialogResult.OK) {
			//	_job.ProjectNum=FormProj.SelectedProjectNum; //project 
			//	textProject=Projects.GetProject(FormProject.SelectedProjectNum).Title; //project 
			//}
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butPickOwner_Click(object sender,EventArgs e) {
			FormUserPick FormUP=new FormUserPick();
			FormUP.ShowDialog();
			if(FormUP.DialogResult==DialogResult.OK) {
				_jobEventCur.Owner=FormUP.SelectedUserNum; //owner
				_job.Owner=FormUP.SelectedUserNum; //owner
				textOwner.Text=Userods.GetName(FormUP.SelectedUserNum);
			}
		}

		private void butLinkBug_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butLinkTask_Click(object sender,EventArgs e) {
			//FormTaskSearch FormTS=new FormTaskSearch();
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butLinkFeatReq_Click(object sender,EventArgs e) {
			//FormFeatureRequest FormFR=new FormFeatureRequest();
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butHistory_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Deleting this job will delete all JobEvents"
				+" and pointers to other tasks, features, and bugs. Are you sure you want to continue?")) {
				try { //Jobs.Delete will throw an application exception if there are any reviews associated with this job.
					Jobs.Delete(_job.JobNum);
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
			_jobEventCur.JobStatus=(JobStatus)comboStatus.SelectedIndex;//status
			_job.JobStatus=(JobStatus)comboStatus.SelectedIndex;//status
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
			_jobEventCur.Description=textDescription.Text;
			_job.Notes=textNotes.Text;
			//create a jobEvent entry if it is a new job, the owner has changed or if its status has been updated
			if(_job.IsNew || _jobEventOld.Owner!=_jobEventCur.Owner || _jobEventOld.JobStatus!=_jobEventCur.JobStatus) {
				JobEvents.Insert(_jobEventCur);
			}
			Jobs.Update(_job);
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
	}
}