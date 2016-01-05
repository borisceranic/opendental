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
	public partial class UserControlJobs:UserControl {
		private List<Job> _jobList;
		
		public UserControlJobs() {
			InitializeComponent();
			long jobNum;
			try {
				jobNum=long.Parse(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				jobNum=0;
			}
			_jobList=Jobs.GetJobList(jobNum,textExpert.Text,textOwner.Text,textVersion.Text,textProject.Text,textTitle.Text,
				comboStatus.SelectedIndex-1,comboPriority.SelectedIndex-1,comboType.SelectedIndex-1,checkShowHidden.Checked);
			JobHandler.JobFired+=ODEvent_Fired;
		}

		private void UserControlJob_Load(object sender,EventArgs e) {
			if(Security.CurUser==null) {
				return;
			}
			//load the comboboxes
			comboPriority.Items.Add("");
			comboType.Items.Add("");
			comboStatus.Items.Add("");
			for(int i=0;i<Enum.GetNames(typeof(JobPriority)).Length;i++) {
				comboPriority.Items.Add(Lan.g("enumJobPriority",Enum.GetNames(typeof(JobPriority))[i]));
			}
			for(int i=0;i<Enum.GetNames(typeof(JobCategory)).Length;i++) {
				comboType.Items.Add(Lan.g("enumJobType",Enum.GetNames(typeof(JobCategory))[i]));
			}
			for(int i=0;i<Enum.GetNames(typeof(JobStat)).Length;i++) {
				comboStatus.Items.Add(Lan.g("enumJobStatus",Enum.GetNames(typeof(JobStat))[i]));

			}
			comboPriority.SelectedIndex=0; //comboboxes have no filter to start with.
			comboType.SelectedIndex=0;
			comboStatus.SelectedIndex=0;
			FillGrid();
		}

		private void FillGrid() {
			int selectedIndex=gridMain.GetSelectedIndex();
			long selectedJobNum=0;
			if(_jobList.Count!=0 && selectedIndex!=-1) {
				selectedJobNum=_jobList[gridMain.GetSelectedIndex()].JobNum;
			}
			long jobNum=0;
			try {									
				jobNum=long.Parse(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				//do nothing
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Owner",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Expert",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",135);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Priority",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Title",0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_jobList.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Userods.GetName(_jobList[i].OwnerNum));
				row.Cells.Add(Userods.GetName(_jobList[i].ExpertNum));
				row.Cells.Add(Enum.GetName(typeof(JobStat),(int)_jobList[i].JobStatus)); //if null returns blank
				row.Cells.Add(Enum.GetName(typeof(JobPriority),(int)_jobList[i].Priority)); //if null returns blank
				row.Cells.Add(_jobList[i].Title);
				row.Tag=_jobList[i].JobNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if((long)gridMain.Rows[i].Tag==selectedJobNum) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			long jobNum;
			try {
				jobNum=long.Parse(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				jobNum=0;
			}
			_jobList=Jobs.GetJobList(jobNum,textExpert.Text,textOwner.Text,textVersion.Text,textProject.Text,textTitle.Text,
				comboStatus.SelectedIndex-1,comboPriority.SelectedIndex-1,comboType.SelectedIndex-1,checkShowHidden.Checked);
			FillGrid();
		}

		/// <summary>Lessen the number of calls to the database by only refreshing the grid when the user is done typing. 
		/// (.5s after the last keystroke)</summary>
		private void timerSearch_Tick(object sender,EventArgs e) {
			timerSearch.Stop();
			long jobNum;
			try {
				jobNum=long.Parse(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				jobNum=0;
			}
			_jobList=Jobs.GetJobList(jobNum,textExpert.Text,textOwner.Text,textVersion.Text,textProject.Text,textTitle.Text,
				comboStatus.SelectedIndex-1,comboPriority.SelectedIndex-1,comboType.SelectedIndex-1,checkShowHidden.Checked);
			FillGrid();
		}
	
		private void textJobNum_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textExpert_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textOwner_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textVersion_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textProject_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void textTitle_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void comboPriority_SelectionChangeCommitted(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void comboType_SelectionChangeCommitted(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			long jobNum;
			try {
				jobNum=long.Parse(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				jobNum=0;
			}
			_jobList=Jobs.GetJobList(jobNum,textExpert.Text,textOwner.Text,textVersion.Text,textProject.Text,textTitle.Text,
				comboStatus.SelectedIndex-1,comboPriority.SelectedIndex-1,comboType.SelectedIndex-1,checkShowHidden.Checked);
			FillGrid();
		}
		
		private void butAdd_Click(object sender,EventArgs e) {
			if(JobPermissions.IsAuthorized(JobPerm.Concept)) {
				FormJobEdit FormJE=new FormJobEdit();
				FormJE.Show();
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			long jobNum=_jobList[e.Row].JobNum; //every job must have a jobNum associated with it, so no need for try-catch.
			FormJobEdit FormJE=new FormJobEdit(jobNum);
			FormJE.Show();
		}

		protected override void OnHandleDestroyed(EventArgs e) {
			base.OnHandleDestroyed(e);
			JobHandler.JobFired-=ODEvent_Fired;
		}
	}
}
