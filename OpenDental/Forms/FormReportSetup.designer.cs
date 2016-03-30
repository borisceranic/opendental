namespace OpenDental{
	partial class FormReportSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportSetup));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabDisplayOptions = new System.Windows.Forms.TabPage();
			this.checkReportsProcDate = new System.Windows.Forms.CheckBox();
			this.checkReportProdWO = new System.Windows.Forms.CheckBox();
			this.checkReportPIClinic = new System.Windows.Forms.CheckBox();
			this.checkReportsShowPatNum = new System.Windows.Forms.CheckBox();
			this.checkReportPIClinicInfo = new System.Windows.Forms.CheckBox();
			this.checkReportPrintWrapColumns = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.gridProdInc = new OpenDental.UI.ODGrid();
			this.gridDaily = new OpenDental.UI.ODGrid();
			this.labelODInternal = new System.Windows.Forms.Label();
			this.gridMonthly = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.gridLists = new OpenDental.UI.ODGrid();
			this.butDown = new OpenDental.UI.Button();
			this.gridPublicHealth = new OpenDental.UI.ODGrid();
			this.butUp = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.tabControl1.SuspendLayout();
			this.tabDisplayOptions.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabDisplayOptions);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(6, 4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(545, 608);
			this.tabControl1.TabIndex = 216;
			// 
			// tabDisplayOptions
			// 
			this.tabDisplayOptions.BackColor = System.Drawing.Color.Transparent;
			this.tabDisplayOptions.Controls.Add(this.checkReportsProcDate);
			this.tabDisplayOptions.Controls.Add(this.checkReportProdWO);
			this.tabDisplayOptions.Controls.Add(this.checkReportPIClinic);
			this.tabDisplayOptions.Controls.Add(this.checkReportsShowPatNum);
			this.tabDisplayOptions.Controls.Add(this.checkReportPIClinicInfo);
			this.tabDisplayOptions.Controls.Add(this.checkReportPrintWrapColumns);
			this.tabDisplayOptions.Location = new System.Drawing.Point(4, 22);
			this.tabDisplayOptions.Name = "tabDisplayOptions";
			this.tabDisplayOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tabDisplayOptions.Size = new System.Drawing.Size(537, 582);
			this.tabDisplayOptions.TabIndex = 0;
			this.tabDisplayOptions.Text = "Report Settings";
			// 
			// checkReportsProcDate
			// 
			this.checkReportsProcDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportsProcDate.Location = new System.Drawing.Point(20, 62);
			this.checkReportsProcDate.Name = "checkReportsProcDate";
			this.checkReportsProcDate.Size = new System.Drawing.Size(345, 17);
			this.checkReportsProcDate.TabIndex = 199;
			this.checkReportsProcDate.Text = "Default to using Proc Date for PPO writeoffs";
			// 
			// checkReportProdWO
			// 
			this.checkReportProdWO.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportProdWO.Location = new System.Drawing.Point(20, 140);
			this.checkReportProdWO.Name = "checkReportProdWO";
			this.checkReportProdWO.Size = new System.Drawing.Size(345, 17);
			this.checkReportProdWO.TabIndex = 201;
			this.checkReportProdWO.Text = "Monthly P&&I scheduled production subtracts PPO writeoffs";
			// 
			// checkReportPIClinic
			// 
			this.checkReportPIClinic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportPIClinic.Location = new System.Drawing.Point(20, 218);
			this.checkReportPIClinic.Name = "checkReportPIClinic";
			this.checkReportPIClinic.Size = new System.Drawing.Size(345, 17);
			this.checkReportPIClinic.TabIndex = 202;
			this.checkReportPIClinic.Text = "Default to showing clinic breakdown on P&&I reports.";
			// 
			// checkReportsShowPatNum
			// 
			this.checkReportsShowPatNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportsShowPatNum.Location = new System.Drawing.Point(20, 101);
			this.checkReportsShowPatNum.Name = "checkReportsShowPatNum";
			this.checkReportsShowPatNum.Size = new System.Drawing.Size(345, 17);
			this.checkReportsShowPatNum.TabIndex = 200;
			this.checkReportsShowPatNum.Text = "Show PatNum: Aging, OutstandingIns, ProcsNotBilled";
			// 
			// checkReportPIClinicInfo
			// 
			this.checkReportPIClinicInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportPIClinicInfo.Location = new System.Drawing.Point(20, 179);
			this.checkReportPIClinicInfo.Name = "checkReportPIClinicInfo";
			this.checkReportPIClinicInfo.Size = new System.Drawing.Size(345, 17);
			this.checkReportPIClinicInfo.TabIndex = 204;
			this.checkReportPIClinicInfo.Text = "Default to showing clinic info on Daily P&&I report.";
			// 
			// checkReportPrintWrapColumns
			// 
			this.checkReportPrintWrapColumns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportPrintWrapColumns.Location = new System.Drawing.Point(20, 23);
			this.checkReportPrintWrapColumns.Name = "checkReportPrintWrapColumns";
			this.checkReportPrintWrapColumns.Size = new System.Drawing.Size(345, 17);
			this.checkReportPrintWrapColumns.TabIndex = 203;
			this.checkReportPrintWrapColumns.Text = "Wrap columns when printing";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.Transparent;
			this.tabPage2.Controls.Add(this.gridProdInc);
			this.tabPage2.Controls.Add(this.gridDaily);
			this.tabPage2.Controls.Add(this.labelODInternal);
			this.tabPage2.Controls.Add(this.gridMonthly);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.gridLists);
			this.tabPage2.Controls.Add(this.butDown);
			this.tabPage2.Controls.Add(this.gridPublicHealth);
			this.tabPage2.Controls.Add(this.butUp);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(537, 582);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Display Settings";
			// 
			// gridProdInc
			// 
			this.gridProdInc.HasAddButton = false;
			this.gridProdInc.HasMultilineHeaders = false;
			this.gridProdInc.HScrollVisible = false;
			this.gridProdInc.Location = new System.Drawing.Point(7, 6);
			this.gridProdInc.Name = "gridProdInc";
			this.gridProdInc.ScrollValue = 0;
			this.gridProdInc.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridProdInc.Size = new System.Drawing.Size(255, 151);
			this.gridProdInc.TabIndex = 205;
			this.gridProdInc.Title = "Production & Income";
			this.gridProdInc.TranslationName = null;
			this.gridProdInc.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridProdInc.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// gridDaily
			// 
			this.gridDaily.HasAddButton = false;
			this.gridDaily.HasMultilineHeaders = false;
			this.gridDaily.HScrollVisible = false;
			this.gridDaily.Location = new System.Drawing.Point(7, 163);
			this.gridDaily.Name = "gridDaily";
			this.gridDaily.ScrollValue = 0;
			this.gridDaily.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridDaily.Size = new System.Drawing.Size(255, 151);
			this.gridDaily.TabIndex = 206;
			this.gridDaily.Title = "Daily";
			this.gridDaily.TranslationName = null;
			this.gridDaily.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridDaily.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// labelODInternal
			// 
			this.labelODInternal.Location = new System.Drawing.Point(315, 473);
			this.labelODInternal.Name = "labelODInternal";
			this.labelODInternal.Size = new System.Drawing.Size(161, 15);
			this.labelODInternal.TabIndex = 213;
			this.labelODInternal.Text = "None";
			this.labelODInternal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gridMonthly
			// 
			this.gridMonthly.HasAddButton = false;
			this.gridMonthly.HasMultilineHeaders = false;
			this.gridMonthly.HScrollVisible = false;
			this.gridMonthly.Location = new System.Drawing.Point(7, 317);
			this.gridMonthly.Name = "gridMonthly";
			this.gridMonthly.ScrollValue = 0;
			this.gridMonthly.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMonthly.Size = new System.Drawing.Size(255, 261);
			this.gridMonthly.TabIndex = 207;
			this.gridMonthly.Title = "Monthly";
			this.gridMonthly.TranslationName = null;
			this.gridMonthly.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridMonthly.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(268, 423);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(255, 42);
			this.label1.TabIndex = 212;
			this.label1.Text = "Move the selected item within its list.\r\nThe current selection\'s internal name is" +
    ":";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// gridLists
			// 
			this.gridLists.HasAddButton = false;
			this.gridLists.HasMultilineHeaders = false;
			this.gridLists.HScrollVisible = false;
			this.gridLists.Location = new System.Drawing.Point(271, 6);
			this.gridLists.Name = "gridLists";
			this.gridLists.ScrollValue = 0;
			this.gridLists.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridLists.Size = new System.Drawing.Size(255, 308);
			this.gridLists.TabIndex = 208;
			this.gridLists.Title = "Lists";
			this.gridLists.TranslationName = null;
			this.gridLists.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridLists.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(360, 535);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(71, 24);
			this.butDown.TabIndex = 211;
			this.butDown.Text = "Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// gridPublicHealth
			// 
			this.gridPublicHealth.HasAddButton = false;
			this.gridPublicHealth.HasMultilineHeaders = false;
			this.gridPublicHealth.HScrollVisible = false;
			this.gridPublicHealth.Location = new System.Drawing.Point(271, 317);
			this.gridPublicHealth.Name = "gridPublicHealth";
			this.gridPublicHealth.ScrollValue = 0;
			this.gridPublicHealth.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridPublicHealth.Size = new System.Drawing.Size(255, 103);
			this.gridPublicHealth.TabIndex = 209;
			this.gridPublicHealth.Title = "Public Health";
			this.gridPublicHealth.TranslationName = null;
			this.gridPublicHealth.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridPublicHealth.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(360, 505);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(71, 24);
			this.butUp.TabIndex = 210;
			this.butUp.Text = "Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(388, 619);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(469, 619);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormReportSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(556, 651);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormReportSetup";
			this.Text = "Report Setup";
			this.Load += new System.EventHandler(this.FormReportSetup_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabDisplayOptions.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkReportsProcDate;
		private System.Windows.Forms.CheckBox checkReportProdWO;
		private System.Windows.Forms.CheckBox checkReportsShowPatNum;
		private System.Windows.Forms.CheckBox checkReportPIClinic;
		private System.Windows.Forms.CheckBox checkReportPrintWrapColumns;
    private System.Windows.Forms.CheckBox checkReportPIClinicInfo;
		private UI.ODGrid gridProdInc;
		private UI.ODGrid gridDaily;
		private UI.ODGrid gridMonthly;
		private UI.ODGrid gridLists;
		private UI.ODGrid gridPublicHealth;
		private UI.Button butUp;
		private UI.Button butDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelODInternal;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDisplayOptions;
		private System.Windows.Forms.TabPage tabPage2;
	}
}