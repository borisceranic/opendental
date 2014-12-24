using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailCertRegister:Form {

		public FormEmailCertRegister(string emailAddress) {
			InitializeComponent();
			Lan.F(this);
			textEmailAddress.Text=emailAddress;
		}

		private void butSendCode_Click(object sender,EventArgs e) {

		}

		private void butBrowse_Click(object sender,EventArgs e) {
			if(openFileDialogCert.ShowDialog()==DialogResult.OK) {
				textCertFilePath.Text=openFileDialogCert.FileName;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!File.Exists(textCertFilePath.Text)) {
				MsgBox.Show(this,"Certificate file path is invalid.");
				return;
			}
			string ext=Path.GetExtension(textCertFilePath.Text).ToLower();
			if(ext!=".pfx" && ext!=".der" && ext!=".cer") {
				MsgBox.Show(this,"Certificate file path extension must be .pfx or .der or .cer.");
				return;
			}



			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}