﻿using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralUserPasswordEdit:Form {
		public string HashedResult;
		private bool _isCreate;

		public FormCentralUserPasswordEdit(bool isCreate,string userName) {
			InitializeComponent();
			_isCreate=isCreate;
			textUserName.Text=userName;
		}

		private void FormCentralUserPasswordEdit_Load(object sender,EventArgs e) {
			if(_isCreate){
				Text="Create Password";
			}
		}

		private void checkShow_Click(object sender,EventArgs e) {
			if(checkShow.Checked) {
				textPassword.PasswordChar='\0';
			}
			else {
				textPassword.PasswordChar='*';
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!_isCreate) {
				string userPassCur=Userods.GetUserByName(textUserName.Text,false).Password;
				//If user's current password is blank we dont care what they put for the old one.
				if(userPassCur!="" && Userods.HashPassword(textCurrent.Text)!=userPassCur)	{
					MessageBox.Show(this,"Current password incorrect.");
					return;
				}
			}
			if(textPassword.Text==""){
				MessageBox.Show(this,"Passwords cannot be blank.");
				return;
			}
			else{
				HashedResult=Userods.HashPassword(textPassword.Text);
				if(Userods.GetUserByName(textUserName.Text,false).UserName==Security.CurUser.UserName) {
					Security.PasswordTyped=textPassword.Text;
					//They're updating the password for the logged in user.  Update CurUser for when they sync then attempt to log into remote DB.
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
