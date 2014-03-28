using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrProvKey:Form {
		public Provider ProvCur;

		public FormEhrProvKey() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEhrProvKey_Load(object sender,EventArgs e) {
			textLName.Text=ProvCur.LName;
			textFName.Text=ProvCur.FName;
			textEhrKey.Text=ProvCur.EhrKey;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(ProvCur.EhrKey=="") {
				DialogResult=DialogResult.Cancel;//Nothing to delete.
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			ProvCur.EhrKey="";
			DialogResult=DialogResult.OK;//So that the change will be made in the provider edit window (parent window).
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!FormEHR.ProvKeyIsValid(ProvCur.LName,ProvCur.FName,14,textEhrKey.Text)){
				MsgBox.Show(this,"Invalid provider key.  Check capitalization, exact spelling, and report access status.");
				return;
			}
			ProvCur.EhrKey=textEhrKey.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}