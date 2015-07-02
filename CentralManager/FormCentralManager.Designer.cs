namespace CentralManager {
	partial class FormCentralManager {
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
			this.label2 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemLogoff = new System.Windows.Forms.MenuItem();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemPassword = new System.Windows.Forms.MenuItem();
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.menuItemConnections = new System.Windows.Forms.MenuItem();
			this.menuItemGroups = new System.Windows.Forms.MenuItem();
			this.menuItemSecurity = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemAnnualPI = new System.Windows.Forms.MenuItem();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.comboConnectionGroups = new System.Windows.Forms.ComboBox();
			this.butPtSearch = new OpenDental.UI.Button();
			this.butEdit = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.groupBoxSync = new System.Windows.Forms.GroupBox();
			this.butLocks = new OpenDental.UI.Button();
			this.butUsers = new OpenDental.UI.Button();
			this.butSecurity = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBoxSync.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(793, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(169, 13);
			this.label2.TabIndex = 212;
			this.label2.Text = "Search";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(793, 28);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(169, 20);
			this.textSearch.TabIndex = 211;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogoff,
            this.menuItemFile,
            this.menuItemSetup,
            this.menuItemReports});
			// 
			// menuItemLogoff
			// 
			this.menuItemLogoff.Index = 0;
			this.menuItemLogoff.Text = "Logoff";
			this.menuItemLogoff.Click += new System.EventHandler(this.menuItemLogoff_Click);
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 1;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPassword});
			this.menuItemFile.Text = "File";
			// 
			// menuItemPassword
			// 
			this.menuItemPassword.Index = 0;
			this.menuItemPassword.Text = "Change Password";
			this.menuItemPassword.Click += new System.EventHandler(this.menuItemPassword_Click);
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 2;
			this.menuItemSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConnections,
            this.menuItemGroups,
            this.menuItemSecurity});
			this.menuItemSetup.Text = "Setup";
			// 
			// menuItemConnections
			// 
			this.menuItemConnections.Index = 0;
			this.menuItemConnections.Text = "Connections";
			this.menuItemConnections.Click += new System.EventHandler(this.menuConnSetup_Click);
			// 
			// menuItemGroups
			// 
			this.menuItemGroups.Index = 1;
			this.menuItemGroups.Text = "Groups";
			this.menuItemGroups.Click += new System.EventHandler(this.menuGroups_Click);
			// 
			// menuItemSecurity
			// 
			this.menuItemSecurity.Index = 2;
			this.menuItemSecurity.Text = "Security";
			this.menuItemSecurity.Click += new System.EventHandler(this.menuItemSecurity_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 3;
			this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAnnualPI});
			this.menuItemReports.Text = "Reports";
			// 
			// menuItemAnnualPI
			// 
			this.menuItemAnnualPI.Index = 0;
			this.menuItemAnnualPI.Text = "Production and Income";
			this.menuItemAnnualPI.Click += new System.EventHandler(this.menuProdInc_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(775, 463);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Connections - double click to launch";
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(793, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(169, 15);
			this.label1.TabIndex = 213;
			this.label1.Text = "Connection Groups";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// comboConnectionGroups
			// 
			this.comboConnectionGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConnectionGroups.FormattingEnabled = true;
			this.comboConnectionGroups.Location = new System.Drawing.Point(793, 79);
			this.comboConnectionGroups.MaxDropDownItems = 20;
			this.comboConnectionGroups.Name = "comboConnectionGroups";
			this.comboConnectionGroups.Size = new System.Drawing.Size(169, 21);
			this.comboConnectionGroups.TabIndex = 214;
			this.comboConnectionGroups.SelectionChangeCommitted += new System.EventHandler(this.comboConnectionGroups_SelectionChangeCommitted);
			// 
			// butPtSearch
			// 
			this.butPtSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPtSearch.Autosize = true;
			this.butPtSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPtSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPtSearch.CornerRadius = 4F;
			this.butPtSearch.Location = new System.Drawing.Point(12, 21);
			this.butPtSearch.Name = "butPtSearch";
			this.butPtSearch.Size = new System.Drawing.Size(75, 23);
			this.butPtSearch.TabIndex = 215;
			this.butPtSearch.Text = "Patients";
			this.butPtSearch.UseVisualStyleBackColor = true;
			this.butPtSearch.Click += new System.EventHandler(this.butPtSearch_Click);
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(93, 484);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(75, 23);
			this.butEdit.TabIndex = 217;
			this.butEdit.Text = "Edit";
			this.butEdit.UseVisualStyleBackColor = true;
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(12, 484);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 23);
			this.butAdd.TabIndex = 216;
			this.butAdd.Text = "Add";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupBoxSync
			// 
			this.groupBoxSync.Controls.Add(this.butLocks);
			this.groupBoxSync.Controls.Add(this.butUsers);
			this.groupBoxSync.Controls.Add(this.butSecurity);
			this.groupBoxSync.Location = new System.Drawing.Point(40, 138);
			this.groupBoxSync.Name = "groupBoxSync";
			this.groupBoxSync.Size = new System.Drawing.Size(98, 112);
			this.groupBoxSync.TabIndex = 218;
			this.groupBoxSync.TabStop = false;
			this.groupBoxSync.Text = "Sync";
			// 
			// butLocks
			// 
			this.butLocks.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLocks.Autosize = true;
			this.butLocks.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLocks.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLocks.CornerRadius = 4F;
			this.butLocks.Location = new System.Drawing.Point(12, 19);
			this.butLocks.Name = "butLocks";
			this.butLocks.Size = new System.Drawing.Size(75, 23);
			this.butLocks.TabIndex = 221;
			this.butLocks.Text = "Locks";
			this.butLocks.UseVisualStyleBackColor = true;
			this.butLocks.Click += new System.EventHandler(this.butLocks_Click);
			// 
			// butUsers
			// 
			this.butUsers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUsers.Autosize = true;
			this.butUsers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUsers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUsers.CornerRadius = 4F;
			this.butUsers.Location = new System.Drawing.Point(12, 76);
			this.butUsers.Name = "butUsers";
			this.butUsers.Size = new System.Drawing.Size(75, 23);
			this.butUsers.TabIndex = 220;
			this.butUsers.Text = "Users";
			this.butUsers.UseVisualStyleBackColor = true;
			this.butUsers.Click += new System.EventHandler(this.butUsers_Click);
			// 
			// butSecurity
			// 
			this.butSecurity.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSecurity.Autosize = true;
			this.butSecurity.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSecurity.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSecurity.CornerRadius = 4F;
			this.butSecurity.Location = new System.Drawing.Point(12, 48);
			this.butSecurity.Name = "butSecurity";
			this.butSecurity.Size = new System.Drawing.Size(75, 23);
			this.butSecurity.TabIndex = 219;
			this.butSecurity.Text = "Security";
			this.butSecurity.UseVisualStyleBackColor = true;
			this.butSecurity.Click += new System.EventHandler(this.butSecurity_Click);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(9, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(154, 32);
			this.label3.TabIndex = 219;
			this.label3.Text = "Select connections before running any tools";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butPtSearch);
			this.groupBox1.Location = new System.Drawing.Point(40, 58);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(98, 60);
			this.groupBox1.TabIndex = 220;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Controls.Add(this.groupBoxSync);
			this.groupBox2.Location = new System.Drawing.Point(793, 216);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(169, 259);
			this.groupBox2.TabIndex = 221;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Connection Tools";
			// 
			// FormCentralManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(974, 519);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.comboConnectionGroups);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.gridMain);
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(799, 431);
			this.Name = "FormCentralManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Central Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCentralManager_FormClosing);
			this.Load += new System.EventHandler(this.FormCentralManager_Load);
			this.groupBoxSync.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private System.Windows.Forms.MenuItem menuItemReports;
		private System.Windows.Forms.MenuItem menuItemConnections;
		private System.Windows.Forms.MenuItem menuItemSecurity;
		private System.Windows.Forms.MenuItem menuItemAnnualPI;
		private System.Windows.Forms.MenuItem menuItemGroups;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboConnectionGroups;
		private OpenDental.UI.Button butPtSearch;
		private System.Windows.Forms.MenuItem menuItemLogoff;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemPassword;
		private OpenDental.UI.Button butEdit;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.GroupBox groupBoxSync;
		private OpenDental.UI.Button butUsers;
		private OpenDental.UI.Button butSecurity;
		private OpenDental.UI.Button butLocks;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}

