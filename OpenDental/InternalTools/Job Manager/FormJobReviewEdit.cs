using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobReviewEdit:Form {
		private long _jobNum;
		private JobReview _jobReviewCur;
		private bool _isReadOnly;

		///<summary>Used for new Reviews. Pass in the jobNum.</summary>
		public FormJobReviewEdit(long jobNum,long userNum):this(jobNum,null,true) {
			_jobReviewCur=new JobReview();
			_jobReviewCur.IsNew=true;
			_jobReviewCur.Reviewer=userNum;
		}

		///<summary>Used for existing Reviews. Pass in the jobNum and the jobReviewNum.</summary>
		public FormJobReviewEdit(long jobNum,JobReview jobReview,bool isReadOnly) {
			_jobNum=jobNum;
			_jobReviewCur=jobReview;
			_isReadOnly=isReadOnly;
			InitializeComponent();
			Lan.F(this);
		}

		public JobReview JobReviewCur {
			get {
				return _jobReviewCur;
			}
		}

		private void FormJobReviewEdit_Load(object sender,EventArgs e) {
			for(int i=0;i<Enum.GetNames(typeof(JobReviewStatus)).Length;i++) {
				comboStatus.Items.Add(Lan.g("enumJobReviewStatus",Enum.GetNames(typeof(JobReviewStatus))[i]));
			}
			textReviewer.Text=Userods.GetName(_jobReviewCur.Reviewer);
			if(_isReadOnly) {
				textDescription.ReadOnly=true;
				comboStatus.Enabled=false;
			}
			if(comboStatus.SelectedIndex==(int)JobReviewStatus.Done 
				|| comboStatus.SelectedIndex==(int)JobReviewStatus.NeedsAdditionalWork) 
			{
				butDelete.Enabled=false;
			}
			comboStatus.SelectedIndex=0;
			if(!_jobReviewCur.IsNew) { //load Review information. Skip if Review is new.
				textDateLastEdited.Text=_jobReviewCur.DateTStamp.ToShortDateString();
				textDescription.Text=_jobReviewCur.Description;
				comboStatus.SelectedIndex=(int)_jobReviewCur.JobReviewStatus;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_jobReviewCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This will delete the current job review. Are you sure?")) {
				JobReviews.Delete(_jobReviewCur.JobReviewNum,JobLinkType.Review);
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			_jobReviewCur.JobReviewStatus=(JobReviewStatus)comboStatus.SelectedIndex;
			_jobReviewCur.Description=textDescription.Text;
			if(_jobReviewCur.IsNew) {
				long jobReviewNum=JobReviews.Insert(_jobReviewCur);
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_jobNum;
				jobLink.LinkType=JobLinkType.Review;
				jobLink.FKey=jobReviewNum;
				JobLinks.Insert(jobLink);
			}
			else {
				JobReviews.Update(_jobReviewCur);
			}
			Signalods.SetInvalid(InvalidType.Job);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}
	}
}