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
		private DataTable _tableReviews;
		private DataTable _tableMyJobs;
		private List<JobLink> _jobLinks;
		
		public UserControlManage() {
			InitializeComponent();
			JobHandler.JobFired+=ODEvent_Fired;
		}

		private void UserControlManage_Load(object sender,EventArgs e) {
			if(Security.CurUser==null) {
				return;
			}
			labelMyJobsCreator.Text="";
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
			#region MyJobsTab
			tabPageMyJobs.Text=tabPageMyJobs.Text.Replace("My",Security.CurUser.UserName);
			gridMyJobs.Title=gridMyJobs.Title.Replace("My",Security.CurUser.UserName);
			FillGridMyJobs();
			FillGridReviews();
			#endregion
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
					row.Cells.Add("");
				}
				long ownerNum=PIn.Long(table.Rows[i]["Owner"].ToString());
				if(ownerNum!=0) {
					row.Cells.Add(_listUsers.Find(x => x.UserNum==ownerNum).UserName);//Owner
				}
				else {
					row.Cells.Add("");
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

		#region MyJobs Tab
		private void FillGridMyJobs() {
			_tableMyJobs=Jobs.GetMyJobsTable(checkShowCreated.Checked,checkShowCompleted.Checked);
			gridMyJobs.BeginUpdate();
			gridMyJobs.Columns.Clear();
			ODGridColumn col=new ODGridColumn("JobNum",50,GridSortingStrategy.AmountParse);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Priority",50);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Date",70,GridSortingStrategy.DateParse);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Expert",55);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Owner",55);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Status",150);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Category",80);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Title",600);
			gridMyJobs.Columns.Add(col);
			col=new ODGridColumn("Project",200);
			gridMyJobs.Columns.Add(col);
			gridMyJobs.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableMyJobs.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_tableMyJobs.Rows[i]["JobNum"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["priority"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["date"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["expert"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["owner"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["status"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["category"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["Title"].ToString());
				row.Cells.Add(_tableMyJobs.Rows[i]["project"].ToString());
				row.Tag=PIn.Long(_tableMyJobs.Rows[i]["JobNum"].ToString());
				gridMyJobs.Rows.Add(row);
			}
			gridMyJobs.EndUpdate();
		}

		private void gridMyJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			FormJobEdit FormJE=new FormJobEdit((long)gridMyJobs.Rows[gridMyJobs.GetSelectedIndex()].Tag);
			FormJE.Show();
		}

		private void gridMyJobs_CellClick(object sender,ODGridClickEventArgs e) {
			DataRow dataRow=_tableMyJobs.Rows.Find(gridMyJobs.Rows[gridMyJobs.GetSelectedIndex()].Tag);
			string originator=dataRow["originator"].ToString();
			string estHours=dataRow["EstimatedHours"].ToString();
			labelMyJobsCreator.Text=originator;
			labelMyJobsEstHours.Text=estHours;
			FillGridLink();
		}

		private void butRefreshMyJobs_Click(object sender,EventArgs e) {
			FillGridMyJobs();
		}

		#region Reviews
		private void FillGridReviews() {
			long selectedReviewNum=0;
			if(gridReviews.GetSelectedIndex()!=-1) {
				selectedReviewNum=(long)gridReviews.Rows[gridReviews.GetSelectedIndex()].Tag;
			}
			_tableReviews=JobReviews.GetOutstandingForUser(Security.CurUser.UserNum);
			gridReviews.BeginUpdate();
			gridReviews.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Owner",55);
			gridReviews.Columns.Add(col);
			col=new ODGridColumn("Date",70);
			gridReviews.Columns.Add(col);
			col=new ODGridColumn("Status",75);
			gridReviews.Columns.Add(col);
			col=new ODGridColumn("Job Title",115);
			gridReviews.Columns.Add(col);
			gridReviews.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableReviews.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Userods.GetName(PIn.Long(_tableReviews.Rows[i]["Owner"].ToString())));
				row.Cells.Add(PIn.DateT(_tableReviews.Rows[i]["DateTStamp"].ToString()).ToShortDateString());
				row.Cells.Add(Enum.GetName(typeof(JobReviewStatus),_tableReviews.Rows[i]["ReviewStatus"])); //if null returns blank
				row.Cells.Add(_tableReviews.Rows[i]["Title"].ToString());
				row.Tag=long.Parse(_tableReviews.Rows[i]["JobReviewNum"].ToString());
				gridReviews.Rows.Add(row);
			}
			gridReviews.EndUpdate();
			for(int i=0;i<gridReviews.Rows.Count;i++) {
				if((long)gridReviews.Rows[i].Tag==selectedReviewNum) {
					gridReviews.SetSelected(i,true);
				}
			}
		}

		private void setSeenToolStripMenuItem_Click(object sender,EventArgs e) {
			if(gridReviews.GetSelectedIndex()==-1) {
				return;
			}
			JobReviews.SetSeen(PIn.Long(_tableReviews.Rows[gridReviews.GetSelectedIndex()]["JobReviewNum"].ToString()));
			DataValid.SetInvalid(InvalidType.Jobs);
			FillGridReviews();
		}

		private void gridReviews_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			long jobNum=PIn.Long(_tableReviews.Rows[e.Row]["JobNum"].ToString()); //every job must have a jobNum associated with it, so no need for try-catch.
			FormJobEdit FormJE=new FormJobEdit(jobNum);
			FormJE.Show();
		}
		#endregion

		#region Links
		private void FillGridLink() {
			long jobNum=(long)gridMyJobs.Rows[gridMyJobs.GetSelectedIndex()].Tag;
			_jobLinks=JobLinks.GetJobLinks(jobNum);
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
		#endregion
		#endregion

		private void butAddJob_Click(object sender,EventArgs e) {
			if(JobRoles.IsAuthorized(JobRoleType.Concept)) {
				FormJobEdit FormJE=new FormJobEdit();
				FormJE.Show();
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			FillGrid();
			FillGridMyJobs();
			FillGridReviews();
		}
		
		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) { //open FormJobEdit
			FormJobEdit FormJE=new FormJobEdit((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			FormJE.Show();
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

		protected override void OnHandleDestroyed(EventArgs e) {
			base.OnHandleDestroyed(e);
			JobHandler.JobFired-=ODEvent_Fired;
		}

	}
}
