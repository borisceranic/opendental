using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobReviewEdit:Form {
		private JobReview _jobReviewCur;
		private List<Userod> _listReviewers;

		///<summary>Used for existing Reviews. Pass in the jobNum and the jobReviewNum.</summary>
		public FormJobReviewEdit(JobReview jobReview) {
			_jobReviewCur=jobReview.Copy();
			InitializeComponent();
			Lan.F(this);
		}

		///<summary>Can be null if deleted from this form.</summary>
		public JobReview JobReviewCur {
			get {
				return _jobReviewCur;
			}
		}

		private void FormJobReviewEdit_Load(object sender,EventArgs e) {
			_listReviewers=Userods.GetUsersByJobRole(JobPerm.Writeup,false);
			_listReviewers.ForEach(x => comboReviewer.Items.Add(x.UserName));
			comboReviewer.SelectedIndex=_listReviewers.FindIndex(x => x.UserNum==_jobReviewCur.ReviewerNum);
			Enum.GetNames(typeof(JobReviewStatus)).ToList().ForEach(x=>comboStatus.Items.Add(x));
			comboStatus.SelectedIndex=(int)_jobReviewCur.ReviewStatus;
			//Disable all controls
			comboReviewer.Enabled=false;
			textDescription.ReadOnly=true;
			comboStatus.Enabled=false;
			butDelete.Enabled=false;
			if(_jobReviewCur.ReviewerNum==0 && JobPermissions.IsAuthorized(JobPerm.Writeup,true)) {//allow any expert to change the expert if there is no expert.
				comboReviewer.Enabled=true;
			}
			if(_jobReviewCur.ReviewerNum==Security.CurUser.UserNum || (_jobReviewCur.ReviewerNum==0 && JobPermissions.IsAuthorized(JobPerm.Writeup,true))) { //allow current expert to edit things.
				textDescription.ReadOnly=false;
				comboStatus.Enabled=true;
				butDelete.Enabled=true;
			}
			if(new[]{JobReviewStatus.Done, JobReviewStatus.NeedsAdditionalWork}.Contains(_jobReviewCur.ReviewStatus)){
				butDelete.Enabled=false;
			}
			if(!_jobReviewCur.IsNew) {
				textDateLastEdited.Text=_jobReviewCur.DateTStamp.ToShortDateString();
			} 
			textDescription.Text=_jobReviewCur.Description;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_jobReviewCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete the current job review?")) {
				_jobReviewCur=null;
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(comboReviewer.SelectedIndex>-1) {
				_jobReviewCur.ReviewerNum=_listReviewers[comboReviewer.SelectedIndex].UserNum;
			}
			_jobReviewCur.ReviewStatus=(JobReviewStatus)comboStatus.SelectedIndex;
			_jobReviewCur.Description=textDescription.Text;
			if(_jobReviewCur.IsNew) {
				_jobReviewCur.DateTStamp=DateTime.Now;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}
	}
}