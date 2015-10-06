using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormStateAbbrEdit:Form {
		private StateAbbr _stateAbbrCur;

		public FormStateAbbrEdit(StateAbbr stateAbbr) {
			_stateAbbrCur=stateAbbr;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormStateAbbrEdit_Load(object sender,EventArgs e) {
			textDescription.Text=_stateAbbrCur.Description;
			textAbbr.Text=_stateAbbrCur.Abbr;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_stateAbbrCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete State Abbr?")) {
				return;
			}
			StateAbbrs.Delete(_stateAbbrCur.StateAbbrNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDescription.Text=="") {
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			if(textAbbr.Text=="") {
				MsgBox.Show(this,"Abbrevation cannot be blank.");
				return;
			}
			_stateAbbrCur.Description=textDescription.Text;
			_stateAbbrCur.Abbr=textAbbr.Text;
			if(_stateAbbrCur.IsNew) {
				StateAbbrs.Insert(_stateAbbrCur);
			}
			else {
				StateAbbrs.Update(_stateAbbrCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}