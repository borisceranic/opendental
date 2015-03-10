using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralUserEdit:Form {
		public Userod _userCur;

		public FormCentralUserEdit(Userod user) {
			InitializeComponent();
			_userCur=user.Copy();
		}		

		private void FormCentralUserEdit_Load(object sender,EventArgs e) {
			checkIsHidden.Checked=_userCur.IsHidden;
			textUserName.Text=_userCur.UserName;
			for(int i=0;i<UserGroups.List.Length;i++){
				listUserGroup.Items.Add(UserGroups.List[i].Description);
				if(_userCur.UserGroupNum==UserGroups.List[i].UserGroupNum){
					listUserGroup.SelectedIndex=i;
				}
			}
			if(listUserGroup.SelectedIndex==-1){//never allowed to delete last group, so this won't fail
				listUserGroup.SelectedIndex=0;
			}
			if(_userCur.Password==""){
				butPassword.Text="Create Password";
			}
		}

		private void butPassword_Click(object sender,EventArgs e) {
			bool isCreate=false;
			if(_userCur.Password==null) {
				isCreate=true;
			}
			FormCentralUserPasswordEdit FormCPE=new FormCentralUserPasswordEdit(isCreate,_userCur.UserName);
			FormCPE.ShowDialog();
			if(FormCPE.DialogResult==DialogResult.Cancel){
				return;
			}
			_userCur.Password=FormCPE.HashedResult;
			if(_userCur.Password==""){
				butPassword.Text="Create Password";
			}
			else{
				butPassword.Text="Change Password";
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textUserName.Text==""){
				MessageBox.Show(this,"Please enter a username.");
				return;
			}
			_userCur.IsHidden=checkIsHidden.Checked;
			_userCur.UserName=textUserName.Text;
			_userCur.UserGroupNum=UserGroups.List[listUserGroup.SelectedIndex].UserGroupNum;
			_userCur.EmployeeNum=0;
			_userCur.ProvNum=0;
			_userCur.ClinicNum=0;
			_userCur.ClinicIsRestricted=false;
			try{
				if(_userCur.IsNew){
					long userNum=Userods.Insert(_userCur);
					_userCur.UserNumCEMT=userNum;
					Userods.Update(_userCur);//Doing this instead of making a new version of insert...
				}
				else{
					Userods.Update(_userCur);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}




	}
}
