namespace OpenDental{
	partial class FormGradingScaleItemEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGradingScaleItemEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.labelGradeNumber = new System.Windows.Forms.Label();
			this.textGradeShowing = new System.Windows.Forms.TextBox();
			this.labelGradeShowing = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textGradeNumber = new System.Windows.Forms.TextBox();
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
			this.butOK.Location = new System.Drawing.Point(205, 114);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
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
			this.butCancel.Location = new System.Drawing.Point(286, 114);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(131, 66);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(199, 20);
			this.textDescription.TabIndex = 3;
			// 
			// labelGradeNumber
			// 
			this.labelGradeNumber.Location = new System.Drawing.Point(6, 43);
			this.labelGradeNumber.Name = "labelGradeNumber";
			this.labelGradeNumber.Size = new System.Drawing.Size(125, 18);
			this.labelGradeNumber.TabIndex = 72;
			this.labelGradeNumber.Text = "Grade Number";
			this.labelGradeNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGradeShowing
			// 
			this.textGradeShowing.Location = new System.Drawing.Point(131, 20);
			this.textGradeShowing.Name = "textGradeShowing";
			this.textGradeShowing.Size = new System.Drawing.Size(61, 20);
			this.textGradeShowing.TabIndex = 1;
			// 
			// labelGradeShowing
			// 
			this.labelGradeShowing.Location = new System.Drawing.Point(6, 20);
			this.labelGradeShowing.Name = "labelGradeShowing";
			this.labelGradeShowing.Size = new System.Drawing.Size(125, 18);
			this.labelGradeShowing.TabIndex = 69;
			this.labelGradeShowing.Text = "Grade Showing";
			this.labelGradeShowing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDescription
			// 
			this.labelDescription.Location = new System.Drawing.Point(6, 66);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(125, 18);
			this.labelDescription.TabIndex = 70;
			this.labelDescription.Text = "Description";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(198, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 18);
			this.label1.TabIndex = 75;
			this.label1.Text = "*Required";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(198, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(125, 18);
			this.label2.TabIndex = 76;
			this.label2.Text = "*Required";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textGradeNumber
			// 
			this.textGradeNumber.Location = new System.Drawing.Point(131, 43);
			this.textGradeNumber.Name = "textGradeNumber";
			this.textGradeNumber.Size = new System.Drawing.Size(61, 20);
			this.textGradeNumber.TabIndex = 2;
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
			this.butDelete.Location = new System.Drawing.Point(6, 114);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(78, 24);
			this.butDelete.TabIndex = 4;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// FormGradingScaleItemEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(373, 150);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textGradeNumber);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.labelGradeNumber);
			this.Controls.Add(this.textGradeShowing);
			this.Controls.Add(this.labelGradeShowing);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(358, 161);
			this.Name = "FormGradingScaleItemEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Grading Scale Item Edit";
			this.Load += new System.EventHandler(this.FormGradingScaleItemEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label labelGradeNumber;
		private System.Windows.Forms.TextBox textGradeShowing;
		private System.Windows.Forms.Label labelGradeShowing;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textGradeNumber;
		private UI.Button butDelete;
	}
}