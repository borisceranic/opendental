using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormTaskHist:Form {
		public long TaskNumCur;
		///<summary>Contains all TaskHists for the given TaskNumCur. Does not include the "current" revision of non-deleted tasks.</summary>
		private List<TaskHist> _listTaskAudit;

		public FormTaskHist() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTaskHist_Load(object sender,EventArgs e) {
			_listTaskAudit=TaskHists.GetArchivesForTask(TaskNumCur);
			FillGrid();
		}

		private void FillGrid() {
			gridTaskHist.BeginUpdate();
			gridTaskHist.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableTaskAudit","Create Date"),140);
			gridTaskHist.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTaskAudit","Edit Date"),140);
			gridTaskHist.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTaskAudit","Editing User"),80);
			gridTaskHist.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTaskAudit","Changes"),100);
			gridTaskHist.Columns.Add(col);
			gridTaskHist.Rows.Clear();
			ODGridRow row;
			for(int i=1;i<_listTaskAudit.Count;i++) {
				TaskHist taskHistCur=_listTaskAudit[i];
				TaskHist taskHistOld=_listTaskAudit[i-1];
				row=new ODGridRow();
				if(taskHistOld.DateTimeEntry==DateTime.MinValue) {
					row.Cells.Add(_listTaskAudit[i].DateTimeEntry.ToString());
				}
				else {
					row.Cells.Add(taskHistOld.DateTimeEntry.ToString());
				}
				row.Cells.Add(taskHistOld.DateTStamp.ToString());
				row.Cells.Add(Userods.GetUser(taskHistOld.UserNum).UserName);
				row.Cells.Add(TaskHists.GetChangesDescription(taskHistOld,taskHistCur));
				gridTaskHist.Rows.Add(row);
			}
			//Compare the current task with the last hist entry (Add the "current revision" of the task if necessary.)
			if(_listTaskAudit.Count>0) {
				TaskHist taskHistOld=_listTaskAudit[_listTaskAudit.Count-1];
				TaskHist taskHistCur=new TaskHist(Tasks.GetOne(TaskNumCur));
				row=new ODGridRow();
				if(taskHistOld.DateTimeEntry==DateTime.MinValue) {
					row.Cells.Add(taskHistCur.DateTimeEntry.ToString());
				}
				else {
					row.Cells.Add(taskHistOld.DateTimeEntry.ToString());
				}
				row.Cells.Add(taskHistOld.DateTStamp.ToString());
				row.Cells.Add(Userods.GetUser(taskHistOld.UserNum).UserName);
				row.Cells.Add(TaskHists.GetChangesDescription(taskHistOld,taskHistCur));
				gridTaskHist.Rows.Add(row);
			}
			gridTaskHist.EndUpdate();
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}

		

	}
}