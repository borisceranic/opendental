using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormUserEdit : ODForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		///<summary></summary>
		public Userod UserCur;
		private TabControl tabControl1;
		private TabPage tabUser;
		private TabPage tabClinics;
		private UI.Button butCancel;
		private UI.Button butOK;
		private UI.Button butPassword;
		private UI.Button butJobRoles;
		private Label labelClinic;
		private ListBox listClinic;
		private Label label1;
		private Label label3;
		private ListBox listUserGroup;
		private TextBox textUserName;
		private Label label2;
		private ListBox listEmployee;
		private Label label4;
		private Label label5;
		private ListBox listProv;
		private CheckBox checkIsHidden;
		private Label label27;
		private TextBox textUserNum;
		private ListBox listClinicMulti;
		private Label label6;
		private CheckBox checkClinicIsRestricted;
		private List<UserGroup> _listUserGroups;
		private Clinic[] _arrayClinics;

		///<summary></summary>
		public FormUserEdit(Userod userCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			UserCur=userCur.Copy();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserEdit));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabUser = new System.Windows.Forms.TabPage();
			this.textUserNum = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listEmployee = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.listUserGroup = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabClinics = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.listClinicMulti = new System.Windows.Forms.ListBox();
			this.checkClinicIsRestricted = new System.Windows.Forms.CheckBox();
			this.listClinic = new System.Windows.Forms.ListBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butJobRoles = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butPassword = new OpenDental.UI.Button();
			this.tabControl1.SuspendLayout();
			this.tabUser.SuspendLayout();
			this.tabClinics.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabUser);
			this.tabControl1.Controls.Add(this.tabClinics);
			this.tabControl1.Location = new System.Drawing.Point(12, 13);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(614, 381);
			this.tabControl1.TabIndex = 0;
			// 
			// tabUser
			// 
			this.tabUser.BackColor = System.Drawing.SystemColors.Control;
			this.tabUser.Controls.Add(this.textUserNum);
			this.tabUser.Controls.Add(this.label27);
			this.tabUser.Controls.Add(this.checkIsHidden);
			this.tabUser.Controls.Add(this.listProv);
			this.tabUser.Controls.Add(this.label5);
			this.tabUser.Controls.Add(this.label4);
			this.tabUser.Controls.Add(this.listEmployee);
			this.tabUser.Controls.Add(this.label2);
			this.tabUser.Controls.Add(this.textUserName);
			this.tabUser.Controls.Add(this.listUserGroup);
			this.tabUser.Controls.Add(this.label3);
			this.tabUser.Controls.Add(this.label1);
			this.tabUser.Location = new System.Drawing.Point(4, 22);
			this.tabUser.Name = "tabUser";
			this.tabUser.Padding = new System.Windows.Forms.Padding(3);
			this.tabUser.Size = new System.Drawing.Size(606, 355);
			this.tabUser.TabIndex = 0;
			this.tabUser.Text = "User";
			// 
			// textUserNum
			// 
			this.textUserNum.BackColor = System.Drawing.SystemColors.Control;
			this.textUserNum.Location = new System.Drawing.Point(28, 27);
			this.textUserNum.Name = "textUserNum";
			this.textUserNum.ReadOnly = true;
			this.textUserNum.Size = new System.Drawing.Size(144, 20);
			this.textUserNum.TabIndex = 165;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(28, 9);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(65, 17);
			this.label27.TabIndex = 166;
			this.label27.Text = "User ID";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.Location = new System.Drawing.Point(495, 70);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(104, 16);
			this.checkIsHidden.TabIndex = 163;
			this.checkIsHidden.Text = "Is Hidden";
			this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.UseVisualStyleBackColor = true;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(414, 110);
			this.listProv.Name = "listProv";
			this.listProv.Size = new System.Drawing.Size(185, 225);
			this.listProv.TabIndex = 160;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(414, 91);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(124, 20);
			this.label5.TabIndex = 159;
			this.label5.Text = "Provider";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(218, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(344, 23);
			this.label4.TabIndex = 158;
			this.label4.Text = "Setting employee or provider is entirely optional";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// listEmployee
			// 
			this.listEmployee.Location = new System.Drawing.Point(221, 110);
			this.listEmployee.Name = "listEmployee";
			this.listEmployee.Size = new System.Drawing.Size(185, 225);
			this.listEmployee.TabIndex = 157;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(221, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 20);
			this.label2.TabIndex = 156;
			this.label2.Text = "Employee (for timecards)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(28, 68);
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(198, 20);
			this.textUserName.TabIndex = 152;
			// 
			// listUserGroup
			// 
			this.listUserGroup.Location = new System.Drawing.Point(28, 110);
			this.listUserGroup.Name = "listUserGroup";
			this.listUserGroup.Size = new System.Drawing.Size(185, 225);
			this.listUserGroup.TabIndex = 154;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(28, 91);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(197, 20);
			this.label3.TabIndex = 153;
			this.label3.Text = "User Group";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(28, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 20);
			this.label1.TabIndex = 151;
			this.label1.Text = "Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabClinics
			// 
			this.tabClinics.BackColor = System.Drawing.SystemColors.Control;
			this.tabClinics.Controls.Add(this.label6);
			this.tabClinics.Controls.Add(this.listClinicMulti);
			this.tabClinics.Controls.Add(this.checkClinicIsRestricted);
			this.tabClinics.Controls.Add(this.listClinic);
			this.tabClinics.Controls.Add(this.labelClinic);
			this.tabClinics.Location = new System.Drawing.Point(4, 22);
			this.tabClinics.Name = "tabClinics";
			this.tabClinics.Padding = new System.Windows.Forms.Padding(3);
			this.tabClinics.Size = new System.Drawing.Size(606, 355);
			this.tabClinics.TabIndex = 1;
			this.tabClinics.Text = "Clinics";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(329, 43);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(150, 20);
			this.label6.TabIndex = 169;
			this.label6.Text = "User Restricted Clinics";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listClinicMulti
			// 
			this.listClinicMulti.Location = new System.Drawing.Point(329, 66);
			this.listClinicMulti.Name = "listClinicMulti";
			this.listClinicMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClinicMulti.Size = new System.Drawing.Size(250, 225);
			this.listClinicMulti.TabIndex = 168;
			// 
			// checkClinicIsRestricted
			// 
			this.checkClinicIsRestricted.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkClinicIsRestricted.Location = new System.Drawing.Point(329, 297);
			this.checkClinicIsRestricted.Name = "checkClinicIsRestricted";
			this.checkClinicIsRestricted.Size = new System.Drawing.Size(250, 52);
			this.checkClinicIsRestricted.TabIndex = 167;
			this.checkClinicIsRestricted.Text = "Restrict user to only see these clinics";
			this.checkClinicIsRestricted.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkClinicIsRestricted.UseVisualStyleBackColor = true;
			// 
			// listClinic
			// 
			this.listClinic.Location = new System.Drawing.Point(28, 66);
			this.listClinic.Name = "listClinic";
			this.listClinic.Size = new System.Drawing.Size(250, 225);
			this.listClinic.TabIndex = 166;
			this.listClinic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listClinic_MouseClick);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(28, 43);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(150, 20);
			this.labelClinic.TabIndex = 165;
			this.labelClinic.Text = "User Default Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butJobRoles
			// 
			this.butJobRoles.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJobRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butJobRoles.Autosize = true;
			this.butJobRoles.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJobRoles.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJobRoles.CornerRadius = 4F;
			this.butJobRoles.Location = new System.Drawing.Point(191, 400);
			this.butJobRoles.Name = "butJobRoles";
			this.butJobRoles.Size = new System.Drawing.Size(103, 26);
			this.butJobRoles.TabIndex = 167;
			this.butJobRoles.Text = "Set Job Roles";
			this.butJobRoles.Click += new System.EventHandler(this.butJobRoles_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(470, 400);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 150;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(551, 400);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 149;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butPassword
			// 
			this.butPassword.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPassword.Autosize = true;
			this.butPassword.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPassword.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPassword.CornerRadius = 4F;
			this.butPassword.Location = new System.Drawing.Point(44, 400);
			this.butPassword.Name = "butPassword";
			this.butPassword.Size = new System.Drawing.Size(103, 26);
			this.butPassword.TabIndex = 155;
			this.butPassword.Text = "Change Password";
			this.butPassword.Click += new System.EventHandler(this.butPassword_Click);
			// 
			// FormUserEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(638, 431);
			this.Controls.Add(this.butJobRoles);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butPassword);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormUserEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "User Edit";
			this.Load += new System.EventHandler(this.FormUserEdit_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabUser.ResumeLayout(false);
			this.tabUser.PerformLayout();
			this.tabClinics.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormUserEdit_Load(object sender, System.EventArgs e) {
			checkIsHidden.Checked=UserCur.IsHidden;
			if(UserCur.UserNum!=0) {
				textUserNum.Text=UserCur.UserNum.ToString();
			}
			textUserName.Text=UserCur.UserName;
			_listUserGroups=UserGroups.GetList();
			for(int i=0;i<_listUserGroups.Count;i++){
				listUserGroup.Items.Add(_listUserGroups[i].Description);
				if(UserCur.UserGroupNum==_listUserGroups[i].UserGroupNum) {
					listUserGroup.SelectedIndex=i;
				}
			}
			if(listUserGroup.SelectedIndex==-1){//never allowed to delete last group, so this won't fail
				listUserGroup.SelectedIndex=0;
			}
			listEmployee.Items.Clear();
			listEmployee.Items.Add(Lan.g(this,"none"));
			listEmployee.SelectedIndex=0;
			for(int i=0;i<Employees.ListShort.Length;i++){
				listEmployee.Items.Add(Employees.GetNameFL(Employees.ListShort[i]));
				if(UserCur.EmployeeNum==Employees.ListShort[i].EmployeeNum){
					listEmployee.SelectedIndex=i+1;
				}
			}
			listProv.Items.Clear();
			listProv.Items.Add(Lan.g(this,"none"));
			listProv.SelectedIndex=0;
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				listProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				if(UserCur.ProvNum==ProviderC.ListShort[i].ProvNum) {
					listProv.SelectedIndex=i+1;
				}
			}
			if(!PrefC.HasClinicsEnabled) {
				tabClinics.Enabled=false;//Disables all controls in the clinics tab.  Tab is still selectable.
			}
			else {
				_arrayClinics=Clinics.GetList();
				listClinic.Items.Clear();
				listClinic.Items.Add(Lan.g(this,"all"));
				if(UserCur.ClinicNum==0) {//Unrestricted
					listClinic.SetSelected(0,true);
					checkClinicIsRestricted.Enabled=false;//We don't really need this checkbox any more but it's probably better for users to keep it....
				}
				List<UserClinic> listUserClinics=UserClinics.GetForUser(UserCur.UserNum);
				for(int i=0;i<_arrayClinics.Length;i++) {
					listClinic.Items.Add(_arrayClinics[i].Description);
					listClinicMulti.Items.Add(_arrayClinics[i].Description);
					if(UserCur.ClinicNum==_arrayClinics[i].ClinicNum) {
						listClinic.SetSelected(i+1,true);
					}
					if(UserCur.ClinicNum!=0 && listUserClinics.Exists(x => x.ClinicNum==_arrayClinics[i].ClinicNum)) {
						listClinicMulti.SetSelected(i,true);//No "All" option, don't select i+1
					}
				}
				checkClinicIsRestricted.Checked=UserCur.ClinicIsRestricted;
			}
			if(UserCur.Password==""){
				butPassword.Text=Lan.g(this,"Create Password");
			}
			if(!PrefC.IsODHQ) {
				butJobRoles.Visible=false;
			}
		}

		private void listClinic_MouseClick(object sender,MouseEventArgs e) {
			int idx=listClinic.IndexFromPoint(e.Location);
			if(idx==-1){
				return;
			}
			if(idx==0){//all
				checkClinicIsRestricted.Checked=false;
				checkClinicIsRestricted.Enabled=false;
			}
			else{
				checkClinicIsRestricted.Enabled=true;
			}
		}

		private void butPassword_Click(object sender, System.EventArgs e) {
			bool isCreate=UserCur.Password=="";
			FormUserPassword FormU=new FormUserPassword(isCreate,UserCur.UserName);
			FormU.IsInSecurityWindow=true;
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.Cancel){
				return;
			}
			UserCur.Password=FormU.HashedResult;
			if(PrefC.GetBool(PrefName.PasswordsMustBeStrong)) {
				UserCur.PasswordIsStrong=true;
			}
			if(UserCur.Password==""){
				butPassword.Text=Lan.g(this,"Create Password");
			}
			else{
				butPassword.Text=Lan.g(this,"Change Password");
			}
		}

		private void butJobRoles_Click(object sender,EventArgs e) {
			FormJobPermissions FormJR=new FormJobPermissions(UserCur.UserNum);
			FormJR.ShowDialog();
		}
		private void butOK_Click(object sender, System.EventArgs e) {
			if(textUserName.Text==""){
				MsgBox.Show(this,"Please enter a username.");
				return;
			}
			List<UserClinic> listUserClinics=new List<UserClinic>();
			if(PrefC.HasClinicsEnabled && checkClinicIsRestricted.Checked) {//They want to restrict the user to certain clinics or clinics are enabled.  
				for(int i=0;i<listClinicMulti.SelectedIndices.Count;i++) {
					listUserClinics.Add(new UserClinic(_arrayClinics[listClinicMulti.SelectedIndices[i]].ClinicNum,UserCur.UserNum));
				}
				//If they set the user up with a default clinic and it's not in the restricted list, return.
				if(!listUserClinics.Exists(x => x.ClinicNum==_arrayClinics[listClinic.SelectedIndex-1].ClinicNum)) {
					MsgBox.Show(this,"User cannot have a default clinic that they are not restricted to.");
					return;
				}
			}
			if(UserClinics.Sync(listUserClinics,UserCur.UserNum)) {//Either syncs new list, or clears old list if no longer restricted.
				DataValid.SetInvalid(InvalidType.UserClinics);
			}
			if(!PrefC.HasClinicsEnabled || listClinic.SelectedIndex==0) {
				UserCur.ClinicNum=0;
			}
			else {
				UserCur.ClinicNum=_arrayClinics[listClinic.SelectedIndex-1].ClinicNum;
			}
			UserCur.ClinicIsRestricted=checkClinicIsRestricted.Checked;//This is kept in sync with their choice of "All".
			UserCur.IsHidden=checkIsHidden.Checked;
			UserCur.UserName=textUserName.Text;
			UserCur.UserGroupNum=_listUserGroups[listUserGroup.SelectedIndex].UserGroupNum;
			if(listEmployee.SelectedIndex==0){
				UserCur.EmployeeNum=0;
			}
			else{
				UserCur.EmployeeNum=Employees.ListShort[listEmployee.SelectedIndex-1].EmployeeNum;
			}
			if(listProv.SelectedIndex==0) {
				Provider prov=Providers.GetProv(UserCur.ProvNum);
				if(prov!=null) {
					prov.IsInstructor=false;//If there are more than 1 users associated to this provider, they will no longer be an instructor.
					Providers.Update(prov);	
				}
				UserCur.ProvNum=0;
			}
			else {
				Provider prov=Providers.GetProv(UserCur.ProvNum);
				if(prov!=null) {
					if(prov.ProvNum!=ProviderC.ListShort[listProv.SelectedIndex-1].ProvNum) {
						prov.IsInstructor=false;//If there are more than 1 users associated to this provider, they will no longer be an instructor.
					}
					Providers.Update(prov);
				}
				UserCur.ProvNum=ProviderC.ListShort[listProv.SelectedIndex-1].ProvNum;
			}
			try{
				if(IsNew){
					Userods.Insert(UserCur);
				}
				else{
					Userods.Update(UserCur);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















