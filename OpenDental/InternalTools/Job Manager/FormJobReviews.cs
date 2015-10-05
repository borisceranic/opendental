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
	public partial class FormJobReviews:Form {
		private long _jobNum;
		private List<JobReview> _jobReviews;

		///<summary>Pass in the jobNum for existing jobs.</summary>
		public FormJobReviews(long jobNum) {
			_jobNum=jobNum;
			ODEvent.Fired+=ODEvent_Fired;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobReviews_Load(object sender,EventArgs e) {
			_jobReviews=JobReviews.GetForJob(_jobNum);
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Date Last Edited",90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Reviewer",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobReviews.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobReviews[i].DateTStamp.ToShortDateString());
				row.Cells.Add(Userods.GetName(_jobReviews[i].Reviewer));
				row.Cells.Add(Enum.GetName(typeof(JobReviewStatus),(int)_jobReviews[i].JobReviewStatus));
				if(_jobReviews[i].Description.Length>=80) {
					row.Cells.Add(_jobReviews[i].Description.Substring(0,80)+"...");
				}
				else {
					row.Cells.Add(_jobReviews[i].Description);
				}
				row.Tag=_jobReviews[i].JobReviewNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			bool isReadOnly=false;
			if(_jobReviews[gridMain.GetSelectedIndex()].Reviewer!=Security.CurUser.UserNum) {
				isReadOnly=true;
				FormTempLogOn FormTLO=new FormTempLogOn(_jobReviews[gridMain.GetSelectedIndex()].Reviewer);
				if(FormTLO.ShowDialog()==DialogResult.OK) {
					isReadOnly=false;
				}
			}
			FormJobReviewEdit FormJRE=new FormJobReviewEdit(_jobNum,_jobReviews[gridMain.GetSelectedIndex()],isReadOnly);
			if(FormJRE.ShowDialog()==DialogResult.OK) {
				if(FormJRE.JobReviewCur==null) {
					_jobReviews.RemoveAt(gridMain.GetSelectedIndex());
				}
				FillGrid();
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			List<Userod> listUsersForPicker=Userods.GetUsersByPermission(Permissions.JobEdit,false);
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUser=listUsersForPicker;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FormJobReviewEdit FormJRE=new FormJobReviewEdit(_jobNum,FormUP.SelectedUserNum);
			if(FormJRE.ShowDialog()==DialogResult.OK) {
				_jobReviews.Add(FormJRE.JobReviewCur);
				FillGrid();
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			_jobReviews=JobReviews.GetForJob(_jobNum);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}

		private void FormJobEdit_FormClosing(object sender,FormClosingEventArgs e) {
			
		}

	}
}