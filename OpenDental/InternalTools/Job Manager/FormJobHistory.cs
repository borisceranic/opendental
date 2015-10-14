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
	public partial class FormJobHistory:Form {
		private long _jobNum;
		private List<JobEvent> _jobEvents;

		///<summary>Opens with links to the passed in JobNum.</summary>
		public FormJobHistory(long jobNum) {
			_jobNum=jobNum;
			JobHandler.JobFired+=ODEvent_Fired;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobHistory_Load(object sender,EventArgs e) {
			_jobEvents=JobEvents.GetForJob(_jobNum);
			FillGrid();
		}

		private void FillGrid() {
			long selectedEventNum=0;
			if(gridMain.GetSelectedIndex()!=-1) {
				selectedEventNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Date",140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Owner",100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",400);
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobEvents.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobEvents[i].DateTimeEntry.ToShortDateString()+" "+_jobEvents[i].DateTimeEntry.ToShortTimeString());
				row.Cells.Add(Userods.GetName(_jobEvents[i].Owner));
				row.Cells.Add(Enum.GetName(typeof(JobStatus),(int)_jobEvents[i].Status));
				string[] arrayDescriptionLines=_jobEvents[i].Description.Split('\n');
				if(arrayDescriptionLines.Length>0) {
					if(arrayDescriptionLines[0].Length>=50) {
						row.Cells.Add(arrayDescriptionLines[0].Substring(0,50)+"...");//Description
					}
					else if(arrayDescriptionLines.Length>1) {
						row.Cells.Add(arrayDescriptionLines[0]+"...");//Description
					}
					else {
						row.Cells.Add(arrayDescriptionLines[0]);
					}
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=_jobEvents[i].JobEventNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if((long)gridMain.Rows[i].Tag==selectedEventNum) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormJobHistoryView FormJHV=new FormJobHistoryView((long)gridMain.Rows[e.Row].Tag);
			FormJHV.Show();
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			_jobEvents=JobEvents.GetForJob(_jobNum);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}