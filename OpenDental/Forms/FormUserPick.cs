using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormUserPick:Form {
		public List<Userod> ListUser;
		///<summary>If this form closes with OK, then this value will be filled.</summary>
		public long SelectedUserNum;
		public bool IsSelectionmode;

		public FormUserPick() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormUserPick_Load(object sender,EventArgs e) {
			if(ListUser==null) {
				ListUser=UserodC.GetListShort();
			}
			for(int i=0;i<ListUser.Count;i++) {
				listUser.Items.Add(ListUser[i]);
			}
		}

		private void listUser_DoubleClick(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true) && Userods.GetInbox(ListUser[listUser.SelectedIndex].UserNum)!=0 && !IsSelectionmode) {
				MsgBox.Show(this,"Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUser[listUser.SelectedIndex].UserNum;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				MsgBox.Show(this,"Please pick a user first.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true) && Userods.GetInbox(ListUser[listUser.SelectedIndex].UserNum)!=0 && !IsSelectionmode) {
				MsgBox.Show(this,"Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUser[listUser.SelectedIndex].UserNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}