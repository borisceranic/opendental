namespace OpenDental{
	partial class FormApptTypeEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptTypeEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.butColorClear = new OpenDental.UI.Button();
			this.butColor = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
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
			this.butOK.Location = new System.Drawing.Point(306, 186);
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
			this.butCancel.Location = new System.Drawing.Point(387, 186);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(28, 65);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(113, 14);
			this.label9.TabIndex = 182;
			this.label9.Text = "Color";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butColorClear
			// 
			this.butColorClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butColorClear.Autosize = true;
			this.butColorClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butColorClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butColorClear.CornerRadius = 4F;
			this.butColorClear.Location = new System.Drawing.Point(172, 63);
			this.butColorClear.Name = "butColorClear";
			this.butColorClear.Size = new System.Drawing.Size(39, 20);
			this.butColorClear.TabIndex = 181;
			this.butColorClear.Text = "none";
			this.butColorClear.Click += new System.EventHandler(this.butColorClear_Click);
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(146, 63);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(20, 20);
			this.butColor.TabIndex = 180;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(47, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 20);
			this.label2.TabIndex = 184;
			this.label2.Text = "Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(146, 36);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(272, 20);
			this.textName.TabIndex = 183;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.Checked = true;
			this.checkIsHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIsHidden.Location = new System.Drawing.Point(51, 88);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkIsHidden.Size = new System.Drawing.Size(109, 24);
			this.checkIsHidden.TabIndex = 185;
			this.checkIsHidden.Text = "Hidden";
			this.checkIsHidden.UseVisualStyleBackColor = true;
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
			this.butDelete.Location = new System.Drawing.Point(12, 186);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 186;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// FormApptTypeEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(474, 222);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.butColorClear);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormApptTypeEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Appointment Type Edit";
			this.Load += new System.EventHandler(this.FormApptTypeEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.Button butOK;
		private UI.Button butCancel;
		private System.Windows.Forms.Label label9;
		private UI.Button butColorClear;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.CheckBox checkIsHidden;
		private UI.Button butDelete;

	}
}