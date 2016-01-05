using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormBugSearch:Form {
		private List<Bug> _listBugsAll=null;

		public Bug BugCur;

		public FormBugSearch() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormBugSearch_Load(object sender,EventArgs e) {
			LoadDataAsync();
			FillGridMain();
		}

		private void LoadDataAsync() {
			ODThread thread=new ODThread((o) => {
				_listBugsAll=Bugs.GetAll();
				this.BeginInvoke((Action)(FillGridMain));
			});
			thread.AddExceptionHandler((ex) => {
				try {
					this.BeginInvoke((Action)(() => {
						MessageBox.Show(ex.Message);
					}));
				}
				catch { }
			});
			thread.Start();
		}

		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn("BugId",50));
			gridMain.Columns.Add(new ODGridColumn("Date",75){SortingStrategy=GridSortingStrategy.DateParse});
			gridMain.Columns.Add(new ODGridColumn("Status",75){TextAlign=HorizontalAlignment.Center});
			gridMain.Columns.Add(new ODGridColumn("Pri",50) { TextAlign=HorizontalAlignment.Center });
			gridMain.Columns.Add(new ODGridColumn("Vers. Found",75));
			gridMain.Columns.Add(new ODGridColumn("Vers. Fixed",75));
			gridMain.Columns.Add(new ODGridColumn("Description",50));
			gridMain.Rows.Clear();
			if(_listBugsAll==null) {
				gridMain.EndUpdate();
				return;//have not returned from DB yet OR no filter set.
			}
			List<string> searchTokens=new List<string>();
			if(checkToken.Checked) {
				searchTokens=textFilter.Text.ToLower().Split(' ').ToList();
			}
			else {
				searchTokens.Add(textFilter.Text.ToLower());
			}
			//listBugsFiltered contains any row that contains all tokens (tokens can appear in any column)
			List<Bug> listBugsFiltered=_listBugsAll.FindAll(x => (checkShow.Checked || new[]{BugStatus.Verified,BugStatus.Accepted}.Contains(x.Status_))
				&& searchTokens.All(y => x.Description.ToLower().Contains(y) || x.VersionsFound.ToLower().Contains(y) || x.VersionsFixed.ToLower().Contains(y)));
			foreach(Bug bug in listBugsFiltered) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(bug.BugId.ToString());
				row.Cells.Add(bug.CreationDate.ToShortDateString());
				row.Cells.Add(bug.Status_.ToString());
				row.Cells.Add(bug.PriorityLevel.ToString());
				row.Cells.Add(bug.VersionsFound.Replace(";","\r\n"));
				row.Cells.Add(bug.VersionsFixed.Replace(";","\r\n"));
				row.Cells.Add(bug.Description);
				row.Tag=bug;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row>=gridMain.Rows.Count) {
				return;
			}
			BugCur=(Bug)gridMain.Rows[e.Row].Tag;
		}

		private void textFilter_TextChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void checkToken_CheckedChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void checkShow_CheckedChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			LoadDataAsync();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			BugCur=(Bug)gridMain.Rows[e.Row].Tag;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(BugCur==null) {
				MsgBox.Show(this,"Select a bug first.");
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			BugCur=null;
			DialogResult=DialogResult.Cancel;
		}

	}



}