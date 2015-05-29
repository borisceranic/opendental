namespace OpenDental {
	partial class FormMedLabs {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedLabs));
			this.checkIncludeNoPat = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkGroupBySpec = new System.Windows.Forms.CheckBox();
			this.butCurrent = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.butFind = new OpenDental.UI.Button();
			this.textDateEnd = new OpenDental.ValidDate();
			this.textDateStart = new OpenDental.ValidDate();
			this.labelEndDate = new System.Windows.Forms.Label();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.labelPatient = new System.Windows.Forms.Label();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkIncludeNoPat
			// 
			this.checkIncludeNoPat.Location = new System.Drawing.Point(534, 13);
			this.checkIncludeNoPat.Name = "checkIncludeNoPat";
			this.checkIncludeNoPat.Size = new System.Drawing.Size(215, 18);
			this.checkIncludeNoPat.TabIndex = 336;
			this.checkIncludeNoPat.Text = "Include labs not attached to a patient";
			this.checkIncludeNoPat.UseVisualStyleBackColor = true;
			this.checkIncludeNoPat.Click += new System.EventHandler(this.checkIncludeNoPat_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkGroupBySpec);
			this.groupBox1.Controls.Add(this.butCurrent);
			this.groupBox1.Controls.Add(this.checkIncludeNoPat);
			this.groupBox1.Controls.Add(this.butRefresh);
			this.groupBox1.Controls.Add(this.butAll);
			this.groupBox1.Controls.Add(this.butFind);
			this.groupBox1.Controls.Add(this.textDateEnd);
			this.groupBox1.Controls.Add(this.textDateStart);
			this.groupBox1.Controls.Add(this.labelEndDate);
			this.groupBox1.Controls.Add(this.textPatient);
			this.groupBox1.Controls.Add(this.labelPatient);
			this.groupBox1.Controls.Add(this.labelStartDate);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(860, 62);
			this.groupBox1.TabIndex = 337;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View";
			// 
			// checkGroupBySpec
			// 
			this.checkGroupBySpec.Location = new System.Drawing.Point(534, 38);
			this.checkGroupBySpec.Name = "checkGroupBySpec";
			this.checkGroupBySpec.Size = new System.Drawing.Size(215, 18);
			this.checkGroupBySpec.TabIndex = 337;
			this.checkGroupBySpec.Text = "One row per specimen";
			this.checkGroupBySpec.UseVisualStyleBackColor = true;
			this.checkGroupBySpec.Click += new System.EventHandler(this.checkGroupBySpec_Click);
			// 
			// butCurrent
			// 
			this.butCurrent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCurrent.Autosize = true;
			this.butCurrent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCurrent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCurrent.CornerRadius = 4F;
			this.butCurrent.Location = new System.Drawing.Point(266, 34);
			this.butCurrent.Name = "butCurrent";
			this.butCurrent.Size = new System.Drawing.Size(64, 24);
			this.butCurrent.TabIndex = 2;
			this.butCurrent.Text = "Current";
			this.butCurrent.Click += new System.EventHandler(this.butCurrent_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(779, 12);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 6;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(418, 34);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(64, 24);
			this.butAll.TabIndex = 4;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// butFind
			// 
			this.butFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFind.Autosize = true;
			this.butFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFind.CornerRadius = 4F;
			this.butFind.Location = new System.Drawing.Point(342, 34);
			this.butFind.Name = "butFind";
			this.butFind.Size = new System.Drawing.Size(64, 24);
			this.butFind.TabIndex = 3;
			this.butFind.Text = "Find";
			this.butFind.Click += new System.EventHandler(this.butFind_Click);
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(102, 35);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(77, 20);
			this.textDateEnd.TabIndex = 1;
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(102, 12);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 0;
			// 
			// labelEndDate
			// 
			this.labelEndDate.Location = new System.Drawing.Point(19, 35);
			this.labelEndDate.Name = "labelEndDate";
			this.labelEndDate.Size = new System.Drawing.Size(80, 18);
			this.labelEndDate.TabIndex = 8;
			this.labelEndDate.Text = "End Date";
			this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatient
			// 
			this.textPatient.BackColor = System.Drawing.SystemColors.Window;
			this.textPatient.Location = new System.Drawing.Point(266, 12);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(216, 20);
			this.textPatient.TabIndex = 8;
			this.textPatient.TabStop = false;
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(184, 12);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(80, 18);
			this.labelPatient.TabIndex = 8;
			this.labelPatient.Text = "Patient";
			this.labelPatient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(19, 12);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(80, 18);
			this.labelStartDate.TabIndex = 8;
			this.labelStartDate.Text = "Start Date";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(797, 411);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 335;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 80);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(860, 321);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Labs";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// FormMedLabs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 446);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(900, 258);
			this.Name = "FormMedLabs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medical Labs";
			this.Load += new System.EventHandler(this.FormMedLabs_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private UI.Button butClose;
		private System.Windows.Forms.CheckBox checkIncludeNoPat;
		private System.Windows.Forms.GroupBox groupBox1;
		private UI.Button butCurrent;
		private UI.Button butAll;
		private UI.Button butFind;
		private ValidDate textDateEnd;
		private ValidDate textDateStart;
		private System.Windows.Forms.Label labelEndDate;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label labelPatient;
		private System.Windows.Forms.Label labelStartDate;
		private UI.Button butRefresh;
		private System.Windows.Forms.CheckBox checkGroupBySpec;
	}
}