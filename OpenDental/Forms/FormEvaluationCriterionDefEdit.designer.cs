namespace OpenDental{
	partial class FormEvaluationCriterionDefEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationCriterionDefEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkIsCategoryName = new System.Windows.Forms.CheckBox();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textGradeScaleName = new System.Windows.Forms.TextBox();
			this.butGradingScale = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
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
			this.butOK.Location = new System.Drawing.Point(314, 75);
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
			this.butCancel.Location = new System.Drawing.Point(314, 105);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkIsCategoryName
			// 
			this.checkIsCategoryName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsCategoryName.Location = new System.Drawing.Point(118, 81);
			this.checkIsCategoryName.Name = "checkIsCategoryName";
			this.checkIsCategoryName.Size = new System.Drawing.Size(190, 17);
			this.checkIsCategoryName.TabIndex = 115;
			this.checkIsCategoryName.Text = "(This will not show a grade)";
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(118, 29);
			this.textDescript.MaxLength = 255;
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(121, 20);
			this.textDescript.TabIndex = 116;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(1, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 17);
			this.label1.TabIndex = 119;
			this.label1.Text = "Grading Scale";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGradeScaleName
			// 
			this.textGradeScaleName.Location = new System.Drawing.Point(118, 55);
			this.textGradeScaleName.MaxLength = 255;
			this.textGradeScaleName.Name = "textGradeScaleName";
			this.textGradeScaleName.ReadOnly = true;
			this.textGradeScaleName.Size = new System.Drawing.Size(121, 20);
			this.textGradeScaleName.TabIndex = 118;
			// 
			// butGradingScale
			// 
			this.butGradingScale.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGradingScale.Autosize = true;
			this.butGradingScale.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGradingScale.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGradingScale.CornerRadius = 4F;
			this.butGradingScale.Location = new System.Drawing.Point(244, 53);
			this.butGradingScale.Name = "butGradingScale";
			this.butGradingScale.Size = new System.Drawing.Size(24, 24);
			this.butGradingScale.TabIndex = 117;
			this.butGradingScale.Text = "...";
			this.butGradingScale.Click += new System.EventHandler(this.butGradingScale_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(1, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 17);
			this.label2.TabIndex = 120;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(113, 17);
			this.label3.TabIndex = 121;
			this.label3.Text = "Category Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.butDelete.Location = new System.Drawing.Point(12, 105);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 122;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// FormEvaluationCriterionDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(401, 141);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textGradeScaleName);
			this.Controls.Add(this.butGradingScale);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.checkIsCategoryName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormEvaluationCriterionDefEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Criterion Def Edit";
			this.Load += new System.EventHandler(this.FormEvaluationCriterionDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkIsCategoryName;
		private System.Windows.Forms.TextBox textDescript;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textGradeScaleName;
		private UI.Button butGradingScale;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private UI.Button butDelete;
	}
}