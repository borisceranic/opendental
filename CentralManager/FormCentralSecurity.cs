using CodeBase;
using OpenDental;
using OpenDental.UI;
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
	public partial class FormCentralSecurity:Form {
		private List<Userod> _listCEMTUsers;
		private long _selectedGroupNum;
		private TreeNode _clickedPermNode;

		public FormCentralSecurity() {
			InitializeComponent();
		}

		private void FormCentralSecurity_Load(object sender,EventArgs e) {
			FillTreePermissionsInitial();
			FillUsers();
			FillTreePerm();
			checkEnable.Checked=PrefC.GetBool(PrefName.CentralManagerSecurityLock);
			checkAdmin.Checked=PrefC.GetBool(PrefName.SecurityLockIncludesAdmin);
			if(PrefC.GetDate(PrefName.SecurityLockDate).Year>1880) {
				textDate.Text=PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}
			if(PrefC.GetInt(PrefName.SecurityLockDays)>0) {
				textDays.Text=PrefC.GetInt(PrefName.SecurityLockDays).ToString();
			}
		}

		private void FillTreePermissionsInitial(){
			TreeNode node;
			TreeNode node2;//second level
			TreeNode node3;
			node=SetNode("Main Menu");
				node2=SetNode(Permissions.Setup);
					node3=SetNode(Permissions.ProblemEdit);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.ChooseDatabase);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.SecurityAdmin);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Schedules);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Providers);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Blockouts);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.UserQuery);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.UserQueryAdmin);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Reports);
					node3=SetNode(Permissions.ReportDashboard);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.ReportProdInc);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.RefAttachAdd);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.RefAttachDelete);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.ReferralAdd);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.AutoNoteQuickNoteEdit);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.WikiListSetup);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode("Main Toolbar");
				node2=SetNode(Permissions.CommlogEdit);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.EmailSend);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.WebmailSend);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.SheetEdit);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.TaskNoteEdit);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.AppointmentsModule);
				node2=SetNode(Permissions.AppointmentCreate);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.AppointmentMove);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.AppointmentEdit);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.EcwAppointmentRevise);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.FamilyModule);
				node2=SetNode(Permissions.InsPlanChangeAssign);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.InsPlanChangeSubsc);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.CarrierCreate);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.AccountModule);
				node2=SetNode(Permissions.ClaimSentEdit);
					node.Nodes.Add(node2);
				node2=SetNode("Insurance Payment");
					node3=SetNode(Permissions.InsPayCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.InsPayEdit);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode("Payment");
					node3=SetNode(Permissions.PaymentCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.PaymentEdit);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode("Adjustment");
					node3=SetNode(Permissions.AdjustmentCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.AdjustmentEdit);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.AdjustmentEditZero);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.TPModule);
				node2=SetNode(Permissions.TreatPlanEdit);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.ChartModule);
				node2=SetNode("Procedure");
					node3=SetNode(Permissions.ProcComplCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.ProcComplEdit);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.ProcEditShowFee);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.ProcDelete);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.ProcedureNote);
						node2.Nodes.Add(node3);
						node.Nodes.Add(node2);
				node2=SetNode("Rx");
					node3=SetNode(Permissions.RxCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.RxEdit);
						node2.Nodes.Add(node3);
						node.Nodes.Add(node2);
				node2=SetNode(Permissions.OrthoChartEdit);
						node.Nodes.Add(node2);
				node2=SetNode(Permissions.PerioEdit);
					node.Nodes.Add(node2);
				node2 = SetNode("Anesthesia");
					node3 = SetNode(Permissions.AnesthesiaIntakeMeds);
						node2.Nodes.Add(node3);
					node3 = SetNode(Permissions.AnesthesiaControlMeds);
						node2.Nodes.Add(node3);
						node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.ImagesModule);
				node2=SetNode(Permissions.ImageDelete);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode(Permissions.ManageModule);
				node2=SetNode(Permissions.Accounting);
					node3=SetNode(Permissions.AccountingCreate);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.AccountingEdit);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Billing);
					node.Nodes.Add(node2);	
				node2=SetNode(Permissions.DepositSlips);
					node.Nodes.Add(node2);
				node2=SetNode(Permissions.Backup);
					node.Nodes.Add(node2);
				node2=SetNode("Timecard");
					node3=SetNode(Permissions.TimecardsEditAll);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.TimecardDeleteEntry);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				node2=SetNode("Equipment");
					node3=SetNode(Permissions.EquipmentSetup);
						node2.Nodes.Add(node3);
					node3=SetNode(Permissions.EquipmentDelete);
						node2.Nodes.Add(node3);
					node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode("EHR");
				node2=SetNode(Permissions.EhrEmergencyAccess);
				node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			node=SetNode("Dental School");
				node2=SetNode(Permissions.AdminDentalInstructors);
				node.Nodes.Add(node2);
				node2=SetNode(Permissions.AdminDentalStudents);
				node.Nodes.Add(node2);
				node2=SetNode(Permissions.AdminDentalEvaluations);
				node.Nodes.Add(node2);
				treePermissions.Nodes.Add(node);
			treePermissions.ExpandAll();
		}

		///<summary>This just keeps FillTreePermissionsInitial looking cleaner.</summary>
		private TreeNode SetNode(Permissions perm){
			TreeNode retVal=new TreeNode();
			retVal.Text=GroupPermissions.GetDesc(perm);
			retVal.Tag=perm;
			retVal.ImageIndex=1;
			retVal.SelectedImageIndex=1;
			return retVal;
		}

		///<summary>Only called from FillTreePermissionsInitial</summary>
		private TreeNode SetNode(string text){
			TreeNode retVal=new TreeNode();
			retVal.Text=text;
			retVal.Tag=Permissions.None;
			retVal.ImageIndex=0;
			retVal.SelectedImageIndex=0;
			return retVal;
		}

		private void FillUsers() {
			Cache.Refresh(InvalidType.Security);
			_selectedGroupNum=0;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Username",90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Group",0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listCEMTUsers=UserodC.GetListt();
			string userdesc;
			for(int i=0;i<_listCEMTUsers.Count;i++) {
				row=new ODGridRow();
				userdesc=_listCEMTUsers[i].UserName;
				if(_listCEMTUsers[i].IsHidden) {
					userdesc+="(hidden)";
				}
				row.Cells.Add(userdesc);
				row.Cells.Add(UserGroups.GetGroup(_listCEMTUsers[i].UserGroupNum).Description);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void treePermissions_AfterSelect(object sender,TreeViewEventArgs e) {
			treePermissions.SelectedNode=null;
			treePermissions.EndUpdate();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			_selectedGroupNum=_listCEMTUsers[e.Row].UserGroupNum;
			for(int i=0;i<_listCEMTUsers.Count;i++) {
				if(_listCEMTUsers[i].UserGroupNum==_selectedGroupNum) {
					gridMain.Rows[i].ColorText=Color.Red;
				}
				else{
					gridMain.Rows[i].ColorText=Color.Black;
				}
			}
			gridMain.Invalidate();
			FillTreePerm();
		}

		private void FillTreePerm(){
			GroupPermissions.RefreshCache();
			if(_selectedGroupNum==0){
				//labelPerm.Text="";
				treePermissions.Enabled=false;
			}
			else{
				//labelPerm.Text="Permissions for group:  "+UserGroups.GetGroup(_selectedGroupNum).Description;
				treePermissions.Enabled=true;
			}
			for(int i=0;i<treePermissions.Nodes.Count;i++){
				FillNodes(treePermissions.Nodes[i],_selectedGroupNum);
			}
		}

		private void FillNodes(TreeNode node,long userGroupNum) {
			//first, any child nodes
			for(int i=0;i<node.Nodes.Count;i++){
				FillNodes(node.Nodes[i],userGroupNum);
			}
			//then this node
			if(node.ImageIndex==0){
				return;
			}
			node.ImageIndex=1;
			node.Text=GroupPermissions.GetDesc((Permissions)node.Tag);
			for(int i=0;i<GroupPermissionC.List.Length;i++){
				if(GroupPermissionC.List[i].UserGroupNum==userGroupNum
					&& GroupPermissionC.List[i].PermType==(Permissions)node.Tag)
				{
					node.ImageIndex=2;
					if(GroupPermissionC.List[i].NewerDate.Year>1880){
						node.Text+=" (if date newer than "+GroupPermissionC.List[i].NewerDate.ToShortDateString()+")";
					}
					else if(GroupPermissionC.List[i].NewerDays>0){
						node.Text+=" (if days newer than "+GroupPermissionC.List[i].NewerDays.ToString()+")";
					}
				}
			}
		}

		private void treePermissions_DoubleClick(object sender,EventArgs e) {
			if(_clickedPermNode==null){
				return;
			}
			Permissions permType=(Permissions)_clickedPermNode.Tag;
			if(!GroupPermissions.PermTakesDates(permType)){
				return;
			}
			GroupPermission perm=GroupPermissions.GetPerm(_selectedGroupNum,(Permissions)_clickedPermNode.Tag);
			if(perm==null){
				return;
			}
			FormCentralGroupPermEdit FormCG=new FormCentralGroupPermEdit(perm);
			FormCG.ShowDialog();
			if(FormCG.DialogResult==DialogResult.Cancel){
				return;
			}
			FillTreePerm();
		}

		private void treePermissions_MouseDown(object sender,MouseEventArgs e) {
			_clickedPermNode=treePermissions.GetNodeAt(e.X,e.Y);
			if(_clickedPermNode==null) {
				return;
			}
			if(_clickedPermNode.Parent==null) {//level 1
				if(e.X<5 || e.X>17) {
					return;
				}
			}
			else if(_clickedPermNode.Parent.Parent==null) {//level 2
				if(e.X<24 || e.X>36) {
					return;
				}
			}
			else if(_clickedPermNode.Parent.Parent.Parent==null) {//level 3
				if(e.X<43 || e.X>55) {
					return;
				}
			}
			if(_clickedPermNode.ImageIndex==1) {//unchecked, so need to add a permission
				GroupPermission perm=new GroupPermission();
				perm.PermType=(Permissions)_clickedPermNode.Tag;
				perm.UserGroupNum=_selectedGroupNum;
				if(GroupPermissions.PermTakesDates(perm.PermType)) {
					FormCentralGroupPermEdit FormCG=new FormCentralGroupPermEdit(perm);
					perm.IsNew=true;
					FormCG.ShowDialog();
					if(FormCG.DialogResult==DialogResult.Cancel) {
						treePermissions.EndUpdate();
						return;
					}
				}
				else {
					try {
						GroupPermissions.Insert(perm);
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
						return;
					}
				}
			}
			else if(_clickedPermNode.ImageIndex==2) {//checked, so need to delete the perm
				try {
					GroupPermissions.RemovePermission(_selectedGroupNum,(Permissions)_clickedPermNode.Tag);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			FillTreePerm();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Userod user=Userods.GetUser(_listCEMTUsers[e.Row].UserNum);
			FormCentralUserEdit FormU=new FormCentralUserEdit(user);
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.Cancel) {
				return;
			}
			FillUsers();
			for(int i=0;i<_listCEMTUsers.Count;i++) {
				if(_listCEMTUsers[i].UserNum==FormU._userCur.UserNum) {
					gridMain.SetSelected(i,true);
					_selectedGroupNum=FormU._userCur.UserGroupNum;
					break;
				}
			}
			FillTreePerm();
		}

		private void butAddUser_Click(object sender,EventArgs e) {
			Userod user=new Userod();
			user.UserGroupNum=_selectedGroupNum;
			user.IsNew=true;
			FormCentralUserEdit FormCU=new FormCentralUserEdit(user);
			FormCU.ShowDialog();
			if(FormCU.DialogResult==DialogResult.Cancel){
				return;
			}
			FillUsers();
			FillTreePerm();
		}

		private void butSetAll_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select user first.");
				return;
			}
			GroupPermission perm;
			for(int i=0;i<Enum.GetNames(typeof(Permissions)).Length;i++){
				perm=GroupPermissions.GetPerm(_selectedGroupNum,(Permissions)i);
				if(perm==null){
					perm=new GroupPermission();
					perm.PermType=(Permissions)i;
					perm.UserGroupNum=_selectedGroupNum;
					try{
						GroupPermissions.Insert(perm);
					}
					catch(Exception ex){
						MessageBox.Show(ex.Message);
					}
				}
			}
			FillTreePerm();
		}

		private void butEditGroup_Click(object sender,EventArgs e) {
			FormCentralUserGroups FormCUG=new FormCentralUserGroups();
			FormCUG.ShowDialog();
			FillUsers();
			FillTreePerm();
		}

		private void textDate_KeyDown(object sender,KeyEventArgs e) {
			textDays.Text="";
		}

		private void textDays_KeyDown(object sender,KeyEventArgs e) {
			textDate.Text="";
			textDate.errorProvider1.SetError(textDate,"");
		}

		private void butSync_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix error first.");
				return;
			}
			//Enter info into local DB before pushing out to others so we save it.
			int days=PIn.Int(textDays.Text);
			DateTime date=PIn.Date(textDate.Text);
			Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(date,false));
			Prefs.UpdateInt(PrefName.SecurityLockDays,days);
			Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkAdmin.Checked) ;
			Prefs.UpdateBool(PrefName.CentralManagerSecurityLock,checkEnable.Checked);
			List<CentralConnection> listChosenConns=new List<CentralConnection>();
			FormCentralChooseDatabase FormCCD=new FormCentralChooseDatabase();
			FormCCD.ShowDialog();
			if(FormCCD.DialogResult==DialogResult.OK) {
				listChosenConns=FormCCD.ListCentralConn;
			}
			else {
				return;
			}
			SyncAll(listChosenConns);
			DialogResult=DialogResult.OK;
		}

			private void SyncAll(List<CentralConnection> listConns) {
			//Get CEMT users, groups, and associated permissions
			List<CentralUserData> listCentralUserData=new List<CentralUserData>();
			for(int i=0;i<_listCEMTUsers.Count;i++) {
				bool hasUserGroup=false;
				UserGroup userGroupCEMT=UserGroups.GetGroup(_listCEMTUsers[i].UserGroupNum);//All users have a usergroup.
				for(int j=0;j<listCentralUserData.Count;j++) {
					if(listCentralUserData[j].UserGroup.Description==userGroupCEMT.Description) {
						listCentralUserData[j].ListUsers.Add(_listCEMTUsers[i]);
						hasUserGroup=true;
						break;
					}
				}
				if(!hasUserGroup) {//Add user group and permissions if they aren't already included.
					List<GroupPermission> listGroupPerms=GroupPermissions.GetPerms(userGroupCEMT.UserGroupNum);
					//New struct element with a one member list of the only person found so far in that usergroup, along with the permissions.
					listCentralUserData.Add(new CentralUserData(userGroupCEMT,new List<Userod>() { _listCEMTUsers[i] },listGroupPerms));
				}
			}
			string failedConns="";
			string nameConflicts="";
			for(int i=0;i<listConns.Count;i++) {
				if(!CentralConnectionHelper.UpdateCentralConnection(listConns[i])) {
					string serverName="";
					if(listConns[i].ServiceURI!="") {
						serverName=listConns[i].ServiceURI;
					}
					else {
						serverName=listConns[i].ServerName+", "+listConns[i].DatabaseName;
					}
					failedConns+=serverName+"\r\n";
					continue;
				}
				Prefs.RefreshCache();
				if(Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(PIn.Date(textDate.Text),false))
				| Prefs.UpdateInt(PrefName.SecurityLockDays,PIn.Int(textDays.Text))
				| Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkAdmin.Checked)  
				| Prefs.UpdateBool(PrefName.CentralManagerSecurityLock,checkEnable.Checked)) {
					Signalods.SetInvalid(InvalidType.Prefs);//Causes cache refresh on workstations
				}
				//We're connected, cache has been refreshed
				//Get remote users, usergroups, and associated permissions
				List<Userod> listRemoteUsers=UserodC.GetListt();
				List<UserGroup> listRemoteUserGroups=UserGroups.GetList();
				#region Detect Conflicts
				//User conflicts
				bool nameConflict=false;
				for(int j=0;j<_listCEMTUsers.Count;j++) {
					for(int k=0;k<listRemoteUsers.Count;k++) {
						if(listRemoteUsers[k].UserName==_listCEMTUsers[j].UserName && listRemoteUsers[k].UserNumCEMT==0) {//User doesn't belong to CEMT
							string serverName="";
							if(listConns[i].ServiceURI!="") {
								serverName=listConns[i].ServiceURI;
							}
							else {
								serverName=listConns[i].ServerName+", "+listConns[i].DatabaseName;
							}
							nameConflicts+=listRemoteUsers[j].UserName+" already exists in "+serverName+"\r\n";
							nameConflict=true;
							break;
						}
					}
				}
				if(nameConflict) {
					continue;//Skip on to the next connection.
				}
				#endregion
				List<UserGroup> listRemoteCEMTUserGroups=UserGroups.GetCEMTGroups();
				List<UserGroup> listCEMTUserGroups=new List<UserGroup>();
				for(int j=0;j<listCentralUserData.Count;j++) {
					listCEMTUserGroups.Add(listCentralUserData[j].UserGroup.Copy());
				}
				//SyncUserGroups returns the list of UserGroups for deletion so it can be used after syncing Users and GroupPermissions.
				List<UserGroup> listUserGroupsForDeletion=CentralUserGroups.Sync(listCEMTUserGroups,listRemoteCEMTUserGroups);
				UserGroups.RefreshCache();
				listRemoteCEMTUserGroups=UserGroups.GetCEMTGroups();
				for(int j=0;j<listCentralUserData.Count;j++) {
					List<GroupPermission> listGroupPerms=new List<GroupPermission>();
					for(int k=0;k<listRemoteCEMTUserGroups.Count;k++) {
						if(listCentralUserData[j].UserGroup.UserGroupNumCEMT==listRemoteCEMTUserGroups[k].UserGroupNumCEMT) {
							for(int l=0;l<listCentralUserData[j].ListUsers.Count;l++) {
								listCentralUserData[j].ListUsers[l].UserGroupNum=listRemoteCEMTUserGroups[k].UserGroupNum;
							}
							for(int l=0;l<listCentralUserData[j].ListGroupPermissions.Count;l++) {
								listCentralUserData[j].ListGroupPermissions[l].UserGroupNum=listRemoteCEMTUserGroups[k].UserGroupNum;
							}
							listGroupPerms=GroupPermissions.GetPerms(listRemoteCEMTUserGroups[k].UserGroupNum);
						}
					}
					CentralUserods.Sync(listCentralUserData[j].ListUsers,listRemoteUsers);
					CentralGroupPermissions.Sync(listCentralUserData[j].ListGroupPermissions,listGroupPerms);
				}
				for(int j=0;j<listUserGroupsForDeletion.Count;j++) {
					UserGroups.Delete(listUserGroupsForDeletion[j]);
				}
			}
			string errorText="";
			if(failedConns=="" && nameConflicts=="") {
				errorText+="Done";
			}
			if(failedConns!="") {
				errorText="Failed Connections:\r\n"+failedConns+"Please try these connections again.\r\n";
			}
			if(nameConflicts!="") {
				errorText+="Name Conflicts:\r\n"+nameConflicts+"Please rename users and try again.\r\n";
			}
			MsgBoxCopyPaste MsgBoxCopyPaste=new MsgBoxCopyPaste(errorText);
			MsgBoxCopyPaste.ShowDialog();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix error first.");
				return;
			}
			//Enter info into local DB before pushing out to others so we save it.
			int days=PIn.Int(textDays.Text);
			DateTime date=PIn.Date(textDate.Text);
			Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(date,false));
			Prefs.UpdateInt(PrefName.SecurityLockDays,days);
			Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkAdmin.Checked) ;
			Prefs.UpdateBool(PrefName.CentralManagerSecurityLock,checkEnable.Checked);
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Custom data structure for containing a UserGroup and its associated list of GroupPermissions and users.</summary>
		public struct CentralUserData {
			public UserGroup UserGroup;
			public List<Userod> ListUsers;
			public List<GroupPermission> ListGroupPermissions;
			public CentralUserData(UserGroup userGroup,List<Userod> listUsers,List<GroupPermission> listGroupPermissions) {
				UserGroup=userGroup;
				ListUsers=listUsers;
				ListGroupPermissions=listGroupPermissions;
			}
		}

	}
}
