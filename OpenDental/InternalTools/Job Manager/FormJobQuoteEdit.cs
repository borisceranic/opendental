using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobQuoteEdit:Form {
		private JobQuote _jobQuote;
		private Patient _patCur;
		public long JobLinkNum;

		///<summary>Used for existing Reviews. Pass in the jobNum and the jobReviewNum.</summary>
		public FormJobQuoteEdit(JobQuote jobQuote) {
			_jobQuote=jobQuote;
			InitializeComponent();
			Lan.F(this);
		}

		public JobQuote JobQuoteCur {
			get {
				return _jobQuote;
			}
		}

		private void FormJobQuoteEdit_Load(object sender,EventArgs e) {
			if(_jobQuote.PatNum!=0) {
				_patCur=Patients.GetPat(_jobQuote.PatNum);
				textPatient.Text=_patCur.LName+", "+_patCur.FName;
			}
			textNote.Text=_jobQuote.Note;
			textAmount.Text=_jobQuote.Amount;
		}

		private void butPatPicker_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect(_patCur);
			if(FormPS.ShowDialog()==DialogResult.OK) {
				_patCur=Patients.GetPat(FormPS.SelectedPatNum);
				textPatient.Text=_patCur.LName+", "+_patCur.FName;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_jobQuote.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This will delete the current job quote. Are you sure?")) {
				JobLinks.Delete(JobLinkNum);
				JobQuotes.Delete(_jobQuote.JobQuoteNum);
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//if(textAmount.Text=="") {
			//	MsgBox.Show(this,"Please enter an amount.");//We can put this in if you don't want them to have auto-entered 0.00 on blank.
			//	return;
			//}
			_jobQuote.Note=textNote.Text;
			if(_patCur!=null) {
				_jobQuote.PatNum=_patCur.PatNum;
			}
			else {
				_jobQuote.PatNum=0;
			}
			_jobQuote.Amount=PIn.Double(textAmount.Text).ToString("F");//I personally think if they enter nothing it'll put in 0.00 for them. Good?
			if(_jobQuote.IsNew) {
				JobQuotes.Insert(_jobQuote);
			}
			else {
				JobQuotes.Update(_jobQuote);
			}
			Signalods.SetInvalid(InvalidType.Jobs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}

	}
}