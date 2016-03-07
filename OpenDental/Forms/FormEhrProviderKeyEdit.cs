using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrProviderKeyEdit:ODForm {
		private EhrProvKey _keyCur;

		///<summary>Only used from FormEhrProviderKeys.  keyCur can be a blank new key.  keyCur cannot be null.</summary>
		public FormEhrProviderKeyEdit(EhrProvKey keyCur) {
			InitializeComponent();
			Lan.F(this);
			_keyCur=keyCur;
		}

		private void FormEhrProviderKeyEdit_Load(object sender,EventArgs e) {
			textYear.Text=_keyCur.YearValue.ToString();
			textKey.Text=_keyCur.ProvKey;
			textLName.Text=_keyCur.LName;
			textFName.Text=_keyCur.FName;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_keyCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			EhrProvKeys.Delete(_keyCur.EhrProvKeyNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textYear.Text==""){
				MessageBox.Show("Please enter a year.");
				return;
			}
			if(textYear.errorProvider1.GetError(textYear)!="") {
				MessageBox.Show("Invalid year, must be two digits.");
				return;
			}
			if(!FormEHR.ProvKeyIsValid(textLName.Text,textFName.Text,PIn.Int(textYear.Text),textKey.Text)) {
				MsgBox.Show(this,"Invalid provider key");
				return;
			}
			_keyCur.LName=textLName.Text;
			_keyCur.FName=textFName.Text;
			_keyCur.YearValue=PIn.Int(textYear.Text);
			_keyCur.ProvKey=textKey.Text;
			if(_keyCur.IsNew) {
				EhrProvKeys.Insert(_keyCur);
			}
			else {
				EhrProvKeys.Update(_keyCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}