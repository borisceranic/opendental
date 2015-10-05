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
			ODEvent.Fired+=ODEvent_Fired;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobHistory_Load(object sender,EventArgs e) {
			_jobEvents=JobEvents.GetForJob(_jobNum);
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Owner",200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobEvents.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobEvents[i].DateTimeEntry.ToShortDateString());
				row.Cells.Add(Userods.GetName(_jobEvents[i].Owner));
				row.Cells.Add(Enum.GetName(typeof(JobStatus),(int)_jobEvents[i].JobStatus));
				if(_jobEvents[i].Description.Length>=80) {
					row.Cells.Add(_jobEvents[i].Description.Substring(0,80)+"...");
				}
				else {
					row.Cells.Add(_jobEvents[i].Description);
				}
				row.Tag=_jobEvents[i].JobEventNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
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