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
		private DataTable _table;
		private List<Userod> _listUsers;
		private string[] _arrayStatuses;
		private List<JobStatus> _listStatuses;
		private List<string> _listExpertFilterNums;
		private List<string> _listOwnerFilterNums;
		private List<string> _listJobStatusFilters;
		
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
			_arrayStatuses=Enum.GetNames(typeof(JobStatus));
			_listStatuses=Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();
			for(int i=0;i<_arrayStatuses.Length;i++) {
				comboBoxMultiStatus.Items.Add(_arrayStatuses[i]);
			}
			comboBoxMultiExpert.SetSelected(0,true);
			comboBoxMultiOwner.SetSelected(0,true);
			comboBoxMultiStatus.SetSelected(0,true);
			FillGrid();
		}

		private void FillGrid() {
			List<string> listExpertNums=new List<string>();
			if(comboBoxMultiExpert.SelectedIndices.Contains(0)) {//Combo Box has All selected
				for(int i=0;i<_listUsers.Count;i++) {
					listExpertNums.Add(_listUsers[i].UserNum.ToString());
				}
			}
			else {
				for(int i=0;i<comboBoxMultiExpert.SelectedIndices.Count;i++) {
					listExpertNums.Add(_listUsers[(int)comboBoxMultiExpert.SelectedIndices[i]-1].UserNum.ToString());
				}
			}
			List<string> listOwnerNums=new List<string>();
			if(comboBoxMultiOwner.SelectedIndices.Contains(0)) {//Combo Box has All selected
				for(int i=0;i<_listUsers.Count;i++) {
					listOwnerNums.Add(_listUsers[i].UserNum.ToString());
				}
			}
			else {
				for(int i=0;i<comboBoxMultiOwner.SelectedIndices.Count;i++) {
					listOwnerNums.Add(_listUsers[(int)comboBoxMultiOwner.SelectedIndices[i]-1].UserNum.ToString());
				}
			}
			List<string> listJobStatuses=new List<string>();
			if(comboBoxMultiStatus.SelectedIndices.Contains(0)) {//Combo Box has All selected
				for(int i=0;i<_listStatuses.Count;i++) {
					listJobStatuses.Add(((int)_listStatuses[i]).ToString());
				}
			}
			else {
				for(int i=0;i<comboBoxMultiStatus.SelectedIndices.Count;i++) {
					listJobStatuses.Add(((int)_listStatuses[(int)comboBoxMultiStatus.SelectedIndices[i]-1]).ToString());
				}
			}
			//Set the filter variables so if a combination row is double clicked the jobs displayed will be correct even if the filters were changed in the 
			//user interface as long as the Refresh button wasn't pressed.
			_listExpertFilterNums=listExpertNums;
			_listOwnerFilterNums=listOwnerNums;
			_listJobStatusFilters=listJobStatuses;
			_table=Jobs.GetForJobManager(checkExpert.Checked,listExpertNums,checkOwner.Checked,listOwnerNums,checkStatus.Checked,listJobStatuses,checkDate.Checked);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Expert",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Owner",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",130);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Job Title",150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",220);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Count",50);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			bool isGroupBy=false;
			if(checkOwner.Checked || checkExpert.Checked || checkStatus.Checked || checkDate.Checked) {
				isGroupBy=true;
			}
			for(int i=0;i<_table.Rows.Count;i++) {
				row=new ODGridRow();
				int groupCount=1;
				if(isGroupBy) {
					groupCount=PIn.Int(_table.Rows[i]["countJobs"].ToString());
				}
				if(checkExpert.Checked || !isGroupBy || groupCount==1) {
					row.Cells.Add(Userods.GetUser(PIn.Long(_table.Rows[i]["Expert"].ToString())).UserName);//Expert
					//color yellow
					if(groupCount!=1) {
						row.ColorText=Color.OrangeRed;
					}
				}
				else {
					row.Cells.Add(" - ");
				}
				if(checkOwner.Checked || !isGroupBy || groupCount==1) {
					row.Cells.Add(Userods.GetUser(PIn.Long(_table.Rows[i]["Owner"].ToString())).UserName);//Owner
					//color brown
					if(groupCount!=1) {
						row.ColorText=Color.Brown;
					}
				}
				else {
					row.Cells.Add(" - ");
				}
				if(checkStatus.Checked || !isGroupBy || groupCount==1) {
					row.Cells.Add(Enum.GetName(typeof(JobStatus),PIn.Long(_table.Rows[i]["JobStatus"].ToString())));//JobStatus
					//color blue
					if(groupCount!=1) {
						row.ColorText=Color.Blue;
					}
				}
				else {
					row.Cells.Add(" - ");
				}
				if(checkDate.Checked || !isGroupBy || groupCount==1) {
					row.Cells.Add(PIn.DateT(_table.Rows[i]["DateTimeEntry"].ToString()).ToShortDateString());//Date
					//color green
					if(groupCount!=1) {
						row.ColorText=Color.DarkGreen;
					}
				}
				else {
					row.Cells.Add(" - ");
				}
				if(!isGroupBy || groupCount==1) {
					row.Cells.Add(_table.Rows[i]["Title"].ToString());//Title
					row.Cells.Add(_table.Rows[i]["Description"].ToString());//Description
				}
				else {
					row.Cells.Add(" - ");
					row.Cells.Add(" - ");
				}
				if(isGroupBy) {
					row.Cells.Add(_table.Rows[i]["countJobs"].ToString());//Count
				}
				else {
					row.Cells.Add("1");
				}
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

		private void setSeenToolStripMenuItem_Click(object sender,EventArgs e) {
			JobReviews.SetSeen(PIn.Long(_table.Rows[gridMain.GetSelectedIndex()]["JobNum"].ToString()));
			Signalods.SetInvalid(InvalidType.Job);
			FillGrid();
		}

		
		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			if(PIn.Int(gridMain.Rows[e.Row].Cells[gridMain.Columns.GetIndex("Count")].Text)>1) {//It's a collapsed row, let's display a job picker
				string rowExpert=gridMain.Rows[e.Row].Cells[gridMain.Columns.GetIndex("Expert")].Text;
				string rowOwner=gridMain.Rows[e.Row].Cells[gridMain.Columns.GetIndex("Owner")].Text;
				string rowStatus=gridMain.Rows[e.Row].Cells[gridMain.Columns.GetIndex("Status")].Text;
				for(int i=0;i<_listStatuses.Count;i++) {
					if(rowStatus==_listStatuses[i].ToString()) {
						rowStatus=((int)_listStatuses[i]).ToString();
						break;
					}
				}
				string rowDate=gridMain.Rows[e.Row].Cells[gridMain.Columns.GetIndex("Date")].Text;
				DataTable table=Jobs.GetForJobPicker(checkExpert.Checked,_listExpertFilterNums,rowExpert,
					checkOwner.Checked,_listOwnerFilterNums,rowOwner,
					checkStatus.Checked,_listJobStatusFilters,rowStatus,
					checkDate.Checked,rowDate);
				FormJobPicker FormJP=new FormJobPicker(table);
				if(FormJP.ShowDialog()==DialogResult.OK) {
					FormJobEdit FormJE=new FormJobEdit(FormJP.JobNum);
					if(FormJE.ShowDialog()==DialogResult.OK) {
						FillGrid();
					}					
				}
			}
			else {
				FormJobEdit FormJE=new FormJobEdit(PIn.Long(_table.Rows[e.Row]["JobNum"].ToString()));
				FormJE.ShowDialog();
				if(FormJE.DialogResult==DialogResult.OK) {
					FillGrid();
				}
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			long ownerNum=PIn.Long(_table.Rows[e.Row]["Owner"].ToString());
			long expertNum=PIn.Long(_table.Rows[e.Row]["Expert"].ToString());
			DataTable ownerSummaryText=Jobs.GetSummaryForOwner(ownerNum);
			DataTable expertSummaryText=Jobs.GetSummaryForExpert(expertNum);
			labelExpertHrs.Text=PIn.Int(expertSummaryText.Rows[0]["numEstHours"].ToString()).ToString();
			labelExpertJobs.Text=PIn.Int(expertSummaryText.Rows[0]["numJobs"].ToString()).ToString();
			labelOwnerHrs.Text=PIn.Int(ownerSummaryText.Rows[0]["numEstHours"].ToString()).ToString();
			labelOwnerJobs.Text=PIn.Int(ownerSummaryText.Rows[0]["numJobs"].ToString()).ToString();
			labelEstHrs.Text=PIn.Int(_table.Rows[e.Row]["HoursEstimate"].ToString()).ToString();
			labelActualHrs.Text=PIn.Int(_table.Rows[e.Row]["HoursActual"].ToString()).ToString();
		}

	}
}
