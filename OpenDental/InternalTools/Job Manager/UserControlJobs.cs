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
		private DataTable _table;
		
		public UserControlJobs() {
			InitializeComponent();
			ODEvent.Fired+=ODEvent_Fired;
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
			for(int i=0;i<Enum.GetNames(typeof(JobType)).Length;i++) {
				comboType.Items.Add(Lan.g("enumJobType",Enum.GetNames(typeof(JobType))[i]));
			}
			for(int i=0;i<Enum.GetNames(typeof(JobStatus)).Length;i++) {
				comboStatus.Items.Add(Lan.g("enumJobStatus",Enum.GetNames(typeof(JobStatus))[i]));

			}
			comboPriority.SelectedIndex=0; //comboboxes have no filter to start with.
			comboType.SelectedIndex=0;
			comboStatus.SelectedIndex=0;
			FillGrid();
		}

		private void FillGrid() {
			int selectedIndex=gridMain.GetSelectedIndex();
			long selectedJobNum=0;
			if(_table!=null && selectedIndex!=-1) {
				selectedJobNum=PIn.Long(_table.Rows[gridMain.GetSelectedIndex()]["JobNum"].ToString());
			}
			long jobNum;
			try {									
				jobNum=PIn.Long(textJobNum.Text); //in case the user enters letters or symbols into the text box.
			}
			catch {
				jobNum=0;
			}
			_table=Jobs.GetJobDataTable(jobNum,textExpert.Text,textOwner.Text,textVersion.Text,textProject.Text,textTitle.Text,
				comboStatus.SelectedIndex-1,comboPriority.SelectedIndex-1,comboType.SelectedIndex-1,checkShowHidden.Checked);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Title",100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Owner",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Expert",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",135);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Priority",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("JobNum",60);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_table.Rows[i]["Title"].ToString());
				row.Cells.Add(_table.Rows[i]["Owner"].ToString());
				row.Cells.Add(_table.Rows[i]["Expert"].ToString());
				row.Cells.Add(Enum.GetName(typeof(JobStatus),_table.Rows[i]["JobStatus"])); //if null returns blank
				row.Cells.Add(Enum.GetName(typeof(JobPriority),_table.Rows[i]["JobPriority"])); //if null returns blank
				row.Cells.Add(_table.Rows[i]["JobNum"].ToString());				
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<_table.Rows.Count;i++) { //set index to the job that it was at before the update. 
				if(selectedJobNum.ToString()==_table.Rows[i]["JobNum"].ToString()) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			FillGrid();
		}

		/// <summary>Lessen the number of calls to the database by only refreshing the grid when the user is done typing. 
		/// (.5s after the last keystroke)</summary>
		private void timerSearch_Tick(object sender,EventArgs e) {
			timerSearch.Stop();
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

		private void comboStatus_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboPriority_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboType_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butAdd_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.JobEdit)) {
				FormJobEdit FormJE=new FormJobEdit();
				FormJE.ShowDialog();
				if(FormJE.DialogResult==DialogResult.OK) {
					FillGrid();
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			long jobNum=PIn.Long(_table.Rows[e.Row]["JobNum"].ToString()); //every job must have a jobNum associated with it, so no need for try-catch.
			FormJobEdit FormJE=new FormJobEdit(jobNum);
			FormJE.ShowDialog();
			if(FormJE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}
	}
}
