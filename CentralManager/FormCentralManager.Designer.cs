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
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.menuItemConnections = new System.Windows.Forms.MenuItem();
			this.menuItemGroups = new System.Windows.Forms.MenuItem();
			this.menuItemPassword = new System.Windows.Forms.MenuItem();
			this.menuItemUsers = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemAnnualPI = new System.Windows.Forms.MenuItem();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.comboConnectionGroups = new System.Windows.Forms.ComboBox();
			this.butSearch = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(508, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 17);
			this.label2.TabIndex = 212;
			this.label2.Text = "Search";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(582, 5);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(190, 20);
			this.textSearch.TabIndex = 211;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup,
            this.menuItemReports});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConnections,
            this.menuItemGroups,
            this.menuItemPassword,
            this.menuItemUsers});
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
			// menuItemPassword
			// 
			this.menuItemPassword.Index = 2;
			this.menuItemPassword.Text = "Password";
			this.menuItemPassword.Click += new System.EventHandler(this.menuPassword_Click);
			// 
			// menuItemUsers
			// 
			this.menuItemUsers.Index = 3;
			this.menuItemUsers.Text = "Users";
			this.menuItemUsers.Click += new System.EventHandler(this.menuUserEdit_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 1;
			this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAnnualPI});
			this.menuItemReports.Text = "Reports";
			// 
			// menuItemAnnualPI
			// 
			this.menuItemAnnualPI.Index = 0;
			this.menuItemAnnualPI.Text = "Annual P&&I";
			this.menuItemAnnualPI.Click += new System.EventHandler(this.menuProdInc_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 31);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(760, 480);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Connections - double click to launch";
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(94, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(177, 15);
			this.label1.TabIndex = 213;
			this.label1.Text = "Connection Groups";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboConnectionGroups
			// 
			this.comboConnectionGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConnectionGroups.FormattingEnabled = true;
			this.comboConnectionGroups.Location = new System.Drawing.Point(277, 2);
			this.comboConnectionGroups.MaxDropDownItems = 20;
			this.comboConnectionGroups.Name = "comboConnectionGroups";
			this.comboConnectionGroups.Size = new System.Drawing.Size(190, 21);
			this.comboConnectionGroups.TabIndex = 214;
			this.comboConnectionGroups.SelectionChangeCommitted += new System.EventHandler(this.comboConnectionGroups_SelectionChangeCommitted);
			// 
			// butSearch
			// 
			this.butSearch.Location = new System.Drawing.Point(13, 2);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 23);
			this.butSearch.TabIndex = 215;
			this.butSearch.Text = "Pt Search";
			this.butSearch.UseVisualStyleBackColor = true;
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// FormCentralManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 519);
			this.Controls.Add(this.butSearch);
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
		private System.Windows.Forms.MenuItem menuItemPassword;
		private System.Windows.Forms.MenuItem menuItemConnections;
		private System.Windows.Forms.MenuItem menuItemUsers;
		private System.Windows.Forms.MenuItem menuItemAnnualPI;
		private System.Windows.Forms.MenuItem menuItemGroups;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboConnectionGroups;
		private System.Windows.Forms.Button butSearch;
	}
}

