namespace OpenDental{
	partial class FormSheetFieldGrid {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldGrid));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.comboGrowthBehavior = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textHeight = new OpenDental.ValidNum();
			this.label8 = new System.Windows.Forms.Label();
			this.textWidth = new OpenDental.ValidNum();
			this.label7 = new System.Windows.Forms.Label();
			this.textYPos = new OpenDental.ValidNum();
			this.label6 = new System.Windows.Forms.Label();
			this.textXPos = new OpenDental.ValidNum();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listColumns = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textGridType = new System.Windows.Forms.TextBox();
			this.checkPmtOpt = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(743, 528);
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
			this.butCancel.Location = new System.Drawing.Point(743, 558);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(15, 558);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 115;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// comboGrowthBehavior
			// 
			this.comboGrowthBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGrowthBehavior.Enabled = false;
			this.comboGrowthBehavior.FormattingEnabled = true;
			this.comboGrowthBehavior.Location = new System.Drawing.Point(619, 93);
			this.comboGrowthBehavior.Name = "comboGrowthBehavior";
			this.comboGrowthBehavior.Size = new System.Drawing.Size(197, 21);
			this.comboGrowthBehavior.TabIndex = 114;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(512, 94);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(107, 16);
			this.label9.TabIndex = 113;
			this.label9.Text = "Growth Behavior";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHeight
			// 
			this.textHeight.Enabled = false;
			this.textHeight.Location = new System.Drawing.Point(619, 199);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = -100;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 112;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(548, 200);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(71, 16);
			this.label8.TabIndex = 111;
			this.label8.Text = "Height";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Enabled = false;
			this.textWidth.Location = new System.Drawing.Point(619, 173);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = -100;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 110;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(548, 174);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(71, 16);
			this.label7.TabIndex = 109;
			this.label7.Text = "Width";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(619, 147);
			this.textYPos.MaxVal = 2000;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 108;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(548, 148);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(71, 16);
			this.label6.TabIndex = 107;
			this.label6.Text = "Y Pos";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(619, 121);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 106;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(548, 122);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 16);
			this.label5.TabIndex = 105;
			this.label5.Text = "X Pos";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(393, 33);
			this.label1.TabIndex = 103;
			this.label1.Text = "The text value for this field will be generated later from the database.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(362, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 16);
			this.label2.TabIndex = 102;
			this.label2.Text = "Columns Available";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listColumns
			// 
			this.listColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listColumns.FormattingEnabled = true;
			this.listColumns.Location = new System.Drawing.Point(354, 66);
			this.listColumns.Name = "listColumns";
			this.listColumns.Size = new System.Drawing.Size(142, 433);
			this.listColumns.TabIndex = 101;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(512, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 16);
			this.label3.TabIndex = 116;
			this.label3.Text = "Grid Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(313, 283);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 24);
			this.butRight.TabIndex = 126;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(313, 243);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 24);
			this.butLeft.TabIndex = 125;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(225, 505);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 24);
			this.butDown.TabIndex = 122;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
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
			this.butUp.Location = new System.Drawing.Point(128, 505);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 24);
			this.butUp.TabIndex = 121;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// gridMain
			// 
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(15, 67);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292, 432);
			this.gridMain.TabIndex = 120;
			this.gridMain.Title = "Fields Showing";
			this.gridMain.TranslationName = "FormDisplayFields";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// textGridType
			// 
			this.textGridType.Enabled = false;
			this.textGridType.Location = new System.Drawing.Point(619, 67);
			this.textGridType.Name = "textGridType";
			this.textGridType.Size = new System.Drawing.Size(197, 20);
			this.textGridType.TabIndex = 127;
			// 
			// checkPmtOpt
			// 
			this.checkPmtOpt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPmtOpt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPmtOpt.Location = new System.Drawing.Point(502, 225);
			this.checkPmtOpt.Name = "checkPmtOpt";
			this.checkPmtOpt.Size = new System.Drawing.Size(130, 20);
			this.checkPmtOpt.TabIndex = 235;
			this.checkPmtOpt.Text = "Is Payment Option";
			this.checkPmtOpt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormSheetFieldGrid
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(830, 594);
			this.Controls.Add(this.checkPmtOpt);
			this.Controls.Add(this.textGridType);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.comboGrowthBehavior);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listColumns);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormSheetFieldGrid";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Sheet Grid";
			this.Load += new System.EventHandler(this.FormSheetFieldGrid_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butDelete;
		private System.Windows.Forms.ComboBox comboGrowthBehavior;
		private System.Windows.Forms.Label label9;
		private ValidNum textHeight;
		private System.Windows.Forms.Label label8;
		private ValidNum textWidth;
		private System.Windows.Forms.Label label7;
		private ValidNum textYPos;
		private System.Windows.Forms.Label label6;
		private ValidNum textXPos;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listColumns;
		private System.Windows.Forms.Label label3;
		private UI.Button butRight;
		private UI.Button butLeft;
		private UI.Button butDown;
		private UI.Button butUp;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.TextBox textGridType;
		private System.Windows.Forms.CheckBox checkPmtOpt;
	}
}