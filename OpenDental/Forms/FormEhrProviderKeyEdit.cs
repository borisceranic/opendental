using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrProviderKeyEdit:Form {
		private EhrProvKey _keyCur;
		private Provider _provCur;

		///<summary>Only used from FormEhrProviderKeys.  keyCur can be a blank new key.  provCur and keyCur cannot be null.</summary>
		public FormEhrProviderKeyEdit(Provider provCur, EhrProvKey keyCur) {
			InitializeComponent();
			Lan.F(this);
			_provCur=provCur;
			_keyCur=keyCur;
		}

		private void FormEhrProviderKeyEdit_Load(object sender,EventArgs e) {
			textYear.Text=_keyCur.YearValue.ToString();
			textKey.Text=_keyCur.ProvKey;
			textLName.Text=_provCur.LName;
			textFName.Text=_provCur.FName;
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
			if(!FormEHR.ProvKeyIsValid(_provCur.LName,_provCur.FName,PIn.Int(textYear.Text),textKey.Text)) {
				MsgBox.Show(this,"Invalid provider key");
				return;
			}
			_keyCur.LName=textLName.Text;
			_keyCur.FName=textFName.Text;
			_keyCur.YearValue=PIn.Int(textYear.Text);
			_keyCur.ProvKey=textKey.Text;
			_keyCur.ProvNum=_provCur.ProvNum;
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