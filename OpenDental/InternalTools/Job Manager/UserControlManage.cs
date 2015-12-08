using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Linq;


namespace OpenDental {
	public partial class UserControlManage:UserControl {
		private List<Userod> _listUsers;
		private List<JobStatus> _listStatuses;
		
		public UserControlManage() {
			InitializeComponent();
			ODEvent.Fired+=ODEvent_Fired;
		}

		private void UserControlManage_Load(object sender,EventArgs e) {
			if(Security.CurUser==null) {
				return;
			}
			_listUsers=Userods.GetUsersForJobs();
			comboBoxMultiExpert.Items.Add("All");
			comboBoxMultiOwner.Items.Add("All");
			for(int i=0;i<_listUsers.Count;i++) {
				comboBoxMultiExpert.Items.Add(_listUsers[i].UserName);
				comboBoxMultiOwner.Items.Add(_listUsers[i].UserName);
			}
			comboBoxMultiStatus.Items.Add("All");
			_listStatuses=Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();
			foreach(JobStatus status in _listStatuses) {
				comboBoxMultiStatus.Items.Add(status.ToString());
			}
			comboBoxMultiExpert.SetSelected(0,true);
			comboBoxMultiOwner.SetSelected(0,true);
			comboBoxMultiStatus.SetSelected(0,true);
			FillGrid();
		}

		private void FillGrid() {
			List<string> listExpertNums=new List<string>();
			if(!comboBoxMultiExpert.SelectedIndices.Contains(0)) {//Combo Box does not have All selected 
				for(int i=0;i<comboBoxMultiExpert.SelectedIndices.Count;i++) {//Add the specific experts
					listExpertNums.Add(_listUsers[(int)comboBoxMultiExpert.SelectedIndices[i]-1].UserNum.ToString());
				}
			}
			List<string> listOwnerNums=new List<string>();
			if(!comboBoxMultiOwner.SelectedIndices.Contains(0)) {//Combo Box does not have All selected
				for(int i=0;i<comboBoxMultiOwner.SelectedIndices.Count;i++) {//Add the specific owners
					listOwnerNums.Add(_listUsers[(int)comboBoxMultiOwner.SelectedIndices[i]-1].UserNum.ToString());
				}
			}
			List<string> listJobStatuses=new List<string>();
			if(!comboBoxMultiStatus.SelectedIndices.Contains(0)) {//Combo Box does not have All selected
				for(int i=0;i<comboBoxMultiStatus.SelectedIndices.Count;i++) {//Add the specific statuses
					listJobStatuses.Add(((int)_listStatuses[(int)comboBoxMultiStatus.SelectedIndices[i]-1]).ToString());
				}
			}
			DataTable table=Jobs.GetForJobManager(listExpertNums,listOwnerNums,listJobStatuses);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("JobNum",50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Expert",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Owner",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",230);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Job Title",470);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["JobNum"].ToString());
				long expertNum=PIn.Long(table.Rows[i]["Expert"].ToString());
				if(expertNum!=0) {
					row.Cells.Add(_listUsers.Find(x => x.UserNum==expertNum).UserName);//Expert
				}
				else {
					row.Cells.Add("None");
				}
				long ownerNum=PIn.Long(table.Rows[i]["Owner"].ToString());
				if(ownerNum!=0) {
					row.Cells.Add(_listUsers.Find(x => x.UserNum==ownerNum).UserName);//Owner
				}
				else {
					row.Cells.Add("None");
				}
				row.Cells.Add(Enum.GetName(typeof(JobStatus),PIn.Long(table.Rows[i]["Status"].ToString())));//JobStatus
				row.Cells.Add(PIn.DateT(table.Rows[i]["DateTimeEntry"].ToString()).ToShortDateString());//Date
				if(table.Rows[i]["Title"].ToString().Length>=50) {
					row.Cells.Add(table.Rows[i]["Title"].ToString().Substring(0,50)+"...");//Title
				}
				else {
					row.Cells.Add(table.Rows[i]["Title"].ToString());
				}
				row.Tag=PIn.Long(table.Rows[i]["JobNum"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			FillGrid();
		}
		
		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			FormJobEdit FormJE=new FormJobEdit((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			FormJE.ShowDialog();
			if(FormJE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			Job job=Jobs.GetOne((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			DataTable ownerSummaryText=Jobs.GetSummaryForOwner(job.Owner);
			DataTable expertSummaryText=Jobs.GetSummaryForExpert(job.Expert);
			labelExpertHrs.Text=PIn.Int(expertSummaryText.Rows[0]["numEstHours"].ToString()).ToString();
			labelExpertJobs.Text=PIn.Int(expertSummaryText.Rows[0]["numJobs"].ToString()).ToString();
			labelOwnerHrs.Text=PIn.Int(ownerSummaryText.Rows[0]["numEstHours"].ToString()).ToString();
			labelOwnerJobs.Text=PIn.Int(ownerSummaryText.Rows[0]["numJobs"].ToString()).ToString();
			labelEstHrs.Text=job.HoursEstimate.ToString();
			labelActualHrs.Text=job.HoursActual.ToString();
		}

		private void comboBoxMultiExpert_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboBoxMultiExpert.SelectedIndices.Count==0) {
				comboBoxMultiExpert.SetSelected(0,true);
			}
		}

		private void comboBoxMultiStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboBoxMultiStatus.SelectedIndices.Count==0) {
				comboBoxMultiStatus.SetSelected(0,true);
			}
		}

		private void comboBoxMultiOwner_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboBoxMultiOwner.SelectedIndices.Count==0) {
				comboBoxMultiOwner.SetSelected(0,true);
			}
		}

	}
}
