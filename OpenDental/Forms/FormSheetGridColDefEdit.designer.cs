namespace OpenDental{
	partial class FormSheetGridColDefEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetGridColDefEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textNameInternal = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textWidth = new OpenDental.ValidNum();
			this.label5 = new System.Windows.Forms.Label();
			this.textNameDisplay = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboTextAlign = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
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
			this.butOK.Location = new System.Drawing.Point(461, 79);
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
			this.butCancel.Location = new System.Drawing.Point(461, 109);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textNameInternal
			// 
			this.textNameInternal.Enabled = false;
			this.textNameInternal.Location = new System.Drawing.Point(193, 12);
			this.textNameInternal.Name = "textNameInternal";
			this.textNameInternal.Size = new System.Drawing.Size(197, 20);
			this.textNameInternal.TabIndex = 133;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(60, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(133, 16);
			this.label3.TabIndex = 132;
			this.label3.Text = "Internal Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(193, 66);
			this.textWidth.MaxVal = 1100;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 129;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(57, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 16);
			this.label5.TabIndex = 128;
			this.label5.Text = "Column Width";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNameDisplay
			// 
			this.textNameDisplay.Location = new System.Drawing.Point(193, 38);
			this.textNameDisplay.Name = "textNameDisplay";
			this.textNameDisplay.Size = new System.Drawing.Size(197, 20);
			this.textNameDisplay.TabIndex = 135;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(57, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 134;
			this.label1.Text = "Display Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTextAlign
			// 
			this.comboTextAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTextAlign.FormattingEnabled = true;
			this.comboTextAlign.Location = new System.Drawing.Point(193, 92);
			this.comboTextAlign.Name = "comboTextAlign";
			this.comboTextAlign.Size = new System.Drawing.Size(191, 21);
			this.comboTextAlign.TabIndex = 137;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(60, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(134, 16);
			this.label2.TabIndex = 136;
			this.label2.Text = "Text Alignment";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormSheetGridColDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(548, 145);
			this.Controls.Add(this.comboTextAlign);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textNameDisplay);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textNameInternal);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormSheetGridColDefEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Column";
			this.Load += new System.EventHandler(this.FormSheetGridColDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textNameInternal;
		private System.Windows.Forms.Label label3;
		private ValidNum textWidth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textNameDisplay;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboTextAlign;
		private System.Windows.Forms.Label label2;
	}
}