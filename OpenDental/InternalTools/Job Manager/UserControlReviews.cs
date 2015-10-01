﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;


namespace OpenDental {
	public partial class UserControlReviews:UserControl {
		private DataTable _table;
		
		public UserControlReviews() {
			InitializeComponent();
			ODEvent.Fired+=ODEvent_Fired;
		}

		private void UserControlReviews_Load(object sender,EventArgs e) {
			if(Security.CurUser==null) {
				return;
			}
			FillGrid();
		}

		private void FillGrid() {
			_table=JobReviews.GetOutstandingForUser(Security.CurUser.UserNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Owner",55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",115);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Job Title",115);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",75);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Userods.GetUser(PIn.Long(_table.Rows[i]["Owner"].ToString())).UserName);
				row.Cells.Add(PIn.DateT(_table.Rows[i]["DateTStamp"].ToString()).ToShortDateString());
				row.Cells.Add(_table.Rows[i]["Description"].ToString());
				row.Cells.Add(_table.Rows[i]["Title"].ToString());
				row.Cells.Add(Enum.GetName(typeof(JobReviewStatus),_table.Rows[i]["JobReviewStatus"])); //if null returns blank
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
			JobReviews.SetSeen(PIn.Long(_table.Rows[gridMain.GetSelectedIndex()]["JobReviewNum"].ToString()));
			Signalods.SetInvalid(InvalidType.Job);
			FillGrid();
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
