namespace CentralManager {
	partial class FormCentralSecurity {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralSecurity));
			this.treePermissions = new System.Windows.Forms.TreeView();
			this.imageListPerm = new System.Windows.Forms.ImageList(this.components);
			this.labelPerm = new System.Windows.Forms.Label();
			this.butSyncAll = new OpenDental.UI.Button();
			this.butSetAll = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butAddUser = new OpenDental.UI.Button();
			this.butEditGroup = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.checkAdmin = new System.Windows.Forms.CheckBox();
			this.checkEnable = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textDays = new System.Windows.Forms.TextBox();
			this.textDate = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butSyncUsers = new OpenDental.UI.Button();
			this.butSyncLocks = new OpenDental.UI.Button();
			this.textSyncCode = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treePermissions
			// 
			this.treePermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treePermissions.HideSelection = false;
			this.treePermissions.ImageIndex = 0;
			this.treePermissions.ImageList = this.imageListPerm;
			this.treePermissions.ItemHeight = 15;
			this.treePermissions.Location = new System.Drawing.Point(255, 27);
			this.treePermissions.Name = "treePermissions";
			this.treePermissions.SelectedImageIndex = 0;
			this.treePermissions.ShowPlusMinus = false;
			this.treePermissions.ShowRootLines = false;
			this.treePermissions.Size = new System.Drawing.Size(386, 305);
			this.treePermissions.TabIndex = 61;
			this.treePermissions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePermissions_AfterSelect);
			this.treePermissions.DoubleClick += new System.EventHandler(this.treePermissions_DoubleClick);
			this.treePermissions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treePermissions_MouseDown);
			// 
			// imageListPerm
			// 
			this.imageListPerm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPerm.ImageStream")));
			this.imageListPerm.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListPerm.Images.SetKeyName(0, "grayBox.gif");
			this.imageListPerm.Images.SetKeyName(1, "checkBoxUnchecked.gif");
			this.imageListPerm.Images.SetKeyName(2, "checkBoxChecked.gif");
			// 
			// labelPerm
			// 
			this.labelPerm.Location = new System.Drawing.Point(252, 9);
			this.labelPerm.Name = "labelPerm";
			this.labelPerm.Size = new System.Drawing.Size(285, 15);
			this.labelPerm.TabIndex = 66;
			this.labelPerm.Text = "Permissions for group:";
			this.labelPerm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butSyncAll
			// 
			this.butSyncAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSyncAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSyncAll.Autosize = true;
			this.butSyncAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSyncAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSyncAll.CornerRadius = 4F;
			this.butSyncAll.Location = new System.Drawing.Point(566, 443);
			this.butSyncAll.Name = "butSyncAll";
			this.butSyncAll.Size = new System.Drawing.Size(75, 24);
			this.butSyncAll.TabIndex = 67;
			this.butSyncAll.Text = "Sync All";
			this.butSyncAll.Click += new System.EventHandler(this.butSync_Click);
			// 
			// butSetAll
			// 
			this.butSetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSetAll.Autosize = true;
			this.butSetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetAll.CornerRadius = 4F;
			this.butSetAll.Location = new System.Drawing.Point(255, 338);
			this.butSetAll.Name = "butSetAll";
			this.butSetAll.Size = new System.Drawing.Size(79, 24);
			this.butSetAll.TabIndex = 64;
			this.butSetAll.Text = "Set All";
			this.butSetAll.Click += new System.EventHandler(this.butSetAll_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(566, 592);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 65;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAddUser
			// 
			this.butAddUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddUser.Autosize = true;
			this.butAddUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddUser.CornerRadius = 4F;
			this.butAddUser.Location = new System.Drawing.Point(170, 338);
			this.butAddUser.Name = "butAddUser";
			this.butAddUser.Size = new System.Drawing.Size(79, 24);
			this.butAddUser.TabIndex = 63;
			this.butAddUser.Text = "Add User";
			this.butAddUser.Click += new System.EventHandler(this.butAddUser_Click);
			// 
			// butEditGroup
			// 
			this.butEditGroup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditGroup.Autosize = true;
			this.butEditGroup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditGroup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditGroup.CornerRadius = 4F;
			this.butEditGroup.Location = new System.Drawing.Point(12, 338);
			this.butEditGroup.Name = "butEditGroup";
			this.butEditGroup.Size = new System.Drawing.Size(75, 24);
			this.butEditGroup.TabIndex = 62;
			this.butEditGroup.Text = "Edit Groups";
			this.butEditGroup.Click += new System.EventHandler(this.butEditGroup_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 9);
			this.gridMain.MinimumSize = new System.Drawing.Size(200, 0);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(237, 323);
			this.gridMain.TabIndex = 60;
			this.gridMain.Title = "Users";
			this.gridMain.TranslationName = "TableSecurity";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.checkAdmin);
			this.groupBox1.Controls.Add(this.checkEnable);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textDays);
			this.groupBox1.Controls.Add(this.textDate);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(12, 388);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(488, 228);
			this.groupBox1.TabIndex = 106;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Lock Date";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(446, 88);
			this.label1.TabIndex = 100;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(231, 194);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(251, 24);
			this.label6.TabIndex = 105;
			this.label6.Text = "(these settings editable from Central Manager only)";
			// 
			// checkAdmin
			// 
			this.checkAdmin.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAdmin.Location = new System.Drawing.Point(40, 171);
			this.checkAdmin.Name = "checkAdmin";
			this.checkAdmin.Size = new System.Drawing.Size(185, 17);
			this.checkAdmin.TabIndex = 3;
			this.checkAdmin.Text = "Lock Includes Admins";
			this.checkAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAdmin.UseVisualStyleBackColor = true;
			// 
			// checkEnable
			// 
			this.checkEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnable.Location = new System.Drawing.Point(6, 194);
			this.checkEnable.Name = "checkEnable";
			this.checkEnable.Size = new System.Drawing.Size(219, 17);
			this.checkEnable.TabIndex = 104;
			this.checkEnable.Text = "Central Manager Security Lock";
			this.checkEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnable.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(139, 146);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 16);
			this.label4.TabIndex = 66;
			this.label4.Text = "Days";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(143, 119);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 18);
			this.label3.TabIndex = 65;
			this.label3.Text = "Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDays
			// 
			this.textDays.Location = new System.Drawing.Point(210, 145);
			this.textDays.Name = "textDays";
			this.textDays.Size = new System.Drawing.Size(46, 20);
			this.textDays.TabIndex = 2;
			this.textDays.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textDays_KeyDown);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(210, 119);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(100, 20);
			this.textDate.TabIndex = 1;
			this.textDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textDate_KeyDown);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(261, 147);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 16);
			this.label2.TabIndex = 68;
			this.label2.Text = "1 means only today";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(566, 562);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 107;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butSyncUsers
			// 
			this.butSyncUsers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSyncUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSyncUsers.Autosize = true;
			this.butSyncUsers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSyncUsers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSyncUsers.CornerRadius = 4F;
			this.butSyncUsers.Location = new System.Drawing.Point(566, 473);
			this.butSyncUsers.Name = "butSyncUsers";
			this.butSyncUsers.Size = new System.Drawing.Size(75, 24);
			this.butSyncUsers.TabIndex = 108;
			this.butSyncUsers.Text = "Sync Users";
			this.butSyncUsers.Click += new System.EventHandler(this.butSyncUsers_Click);
			// 
			// butSyncLocks
			// 
			this.butSyncLocks.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSyncLocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSyncLocks.Autosize = true;
			this.butSyncLocks.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSyncLocks.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSyncLocks.CornerRadius = 4F;
			this.butSyncLocks.Location = new System.Drawing.Point(566, 503);
			this.butSyncLocks.Name = "butSyncLocks";
			this.butSyncLocks.Size = new System.Drawing.Size(75, 24);
			this.butSyncLocks.TabIndex = 109;
			this.butSyncLocks.Text = "Sync Locks";
			this.butSyncLocks.Click += new System.EventHandler(this.butSyncLocks_Click);
			// 
			// textSyncCode
			// 
			this.textSyncCode.Location = new System.Drawing.Point(519, 404);
			this.textSyncCode.Name = "textSyncCode";
			this.textSyncCode.ReadOnly = true;
			this.textSyncCode.Size = new System.Drawing.Size(122, 20);
			this.textSyncCode.TabIndex = 110;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(516, 335);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(125, 67);
			this.label5.TabIndex = 111;
			this.label5.Text = "Sync Code - All databases which can sync with this one will have the code below i" +
    "n their misc setup window.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormCentralSecurity
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 628);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textSyncCode);
			this.Controls.Add(this.butSyncLocks);
			this.Controls.Add(this.butSyncUsers);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butSyncAll);
			this.Controls.Add(this.labelPerm);
			this.Controls.Add(this.butSetAll);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAddUser);
			this.Controls.Add(this.butEditGroup);
			this.Controls.Add(this.treePermissions);
			this.Controls.Add(this.gridMain);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(593, 341);
			this.Name = "FormCentralSecurity";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Central Manager User Setup";
			this.Load += new System.EventHandler(this.FormCentralSecurity_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.TreeView treePermissions;
		private OpenDental.UI.Button butAddUser;
		private OpenDental.UI.Button butEditGroup;
		private OpenDental.UI.Button butSetAll;
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.ImageList imageListPerm;
		private System.Windows.Forms.Label labelPerm;
		private OpenDental.UI.Button butSyncAll;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkAdmin;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textDays;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkEnable;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butSyncUsers;
		private OpenDental.UI.Button butSyncLocks;
		private System.Windows.Forms.TextBox textSyncCode;
		private System.Windows.Forms.Label label5;
	}
}