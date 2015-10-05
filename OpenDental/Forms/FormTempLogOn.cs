using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTempLogOn:Form {
		private Userod _userCur;

		public FormTempLogOn(long userNum) {
			_userCur=Userods.GetUser(userNum);
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTempLogOn_Load(object sender,EventArgs e) {
			textUserName.Text=_userCur.UserName;
		}

		private void checkShow_Click(object sender,EventArgs e) {
			//char ch=textPassword.PasswordChar;
			if(checkShow.Checked) {
				textPassword.PasswordChar='\0';
			}
			else {
				textPassword.PasswordChar='*';
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!Userods.CheckTypedPassword(textPassword.Text,_userCur.Password)) {
				MsgBox.Show(this,"Login failed");
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}