namespace OpenDental{
	partial class FormEvaluationDefEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationDefEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.labelCriterion = new System.Windows.Forms.Label();
			this.listCriterion = new System.Windows.Forms.ListBox();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butGradingScale = new OpenDental.UI.Button();
			this.textGradeScaleName = new System.Windows.Forms.TextBox();
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
			this.butOK.Location = new System.Drawing.Point(566, 492);
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
			this.butCancel.Location = new System.Drawing.Point(566, 522);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(320, 276);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 24);
			this.butRight.TabIndex = 66;
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
			this.butLeft.Location = new System.Drawing.Point(320, 236);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 24);
			this.butLeft.TabIndex = 65;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// labelCriterion
			// 
			this.labelCriterion.Location = new System.Drawing.Point(370, 53);
			this.labelCriterion.Name = "labelCriterion";
			this.labelCriterion.Size = new System.Drawing.Size(213, 17);
			this.labelCriterion.TabIndex = 64;
			this.labelCriterion.Text = "Criterion Available";
			this.labelCriterion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listCriterion
			// 
			this.listCriterion.FormattingEnabled = true;
			this.listCriterion.IntegralHeight = false;
			this.listCriterion.Location = new System.Drawing.Point(373, 73);
			this.listCriterion.Name = "listCriterion";
			this.listCriterion.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCriterion.Size = new System.Drawing.Size(158, 412);
			this.listCriterion.TabIndex = 63;
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
			this.butDown.Location = new System.Drawing.Point(109, 491);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 24);
			this.butDown.TabIndex = 62;
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
			this.butUp.Location = new System.Drawing.Point(12, 491);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 24);
			this.butUp.TabIndex = 61;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// gridMain
			// 
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 60);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292, 425);
			this.gridMain.TabIndex = 60;
			this.gridMain.Title = "Criterion Used";
			this.gridMain.TranslationName = "FormEvaluationDefEdit";
			// 
			// butGradingScale
			// 
			this.butGradingScale.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGradingScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butGradingScale.Autosize = true;
			this.butGradingScale.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGradingScale.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGradingScale.CornerRadius = 4F;
			this.butGradingScale.Location = new System.Drawing.Point(12, 31);
			this.butGradingScale.Name = "butGradingScale";
			this.butGradingScale.Size = new System.Drawing.Size(87, 24);
			this.butGradingScale.TabIndex = 68;
			this.butGradingScale.Text = "Grading Scales";
			this.butGradingScale.Click += new System.EventHandler(this.butGradingScale_Click);
			// 
			// textGradeScaleName
			// 
			this.textGradeScaleName.Location = new System.Drawing.Point(105, 34);
			this.textGradeScaleName.MaxLength = 255;
			this.textGradeScaleName.Name = "textGradeScaleName";
			this.textGradeScaleName.ReadOnly = true;
			this.textGradeScaleName.Size = new System.Drawing.Size(121, 20);
			this.textGradeScaleName.TabIndex = 112;
			// 
			// FormEvaluationDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(653, 558);
			this.Controls.Add(this.textGradeScaleName);
			this.Controls.Add(this.butGradingScale);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.labelCriterion);
			this.Controls.Add(this.listCriterion);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormEvaluationDefEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Definition Edit";
			this.Load += new System.EventHandler(this.FormEvaluationDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butRight;
		private UI.Button butLeft;
		private System.Windows.Forms.Label labelCriterion;
		private System.Windows.Forms.ListBox listCriterion;
		private UI.Button butDown;
		private UI.Button butUp;
		private UI.ODGrid gridMain;
		private UI.Button butGradingScale;
		private System.Windows.Forms.TextBox textGradeScaleName;
	}
}