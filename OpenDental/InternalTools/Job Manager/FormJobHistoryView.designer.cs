namespace OpenDental{
	partial class FormJobHistoryView {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobHistoryView));
			this.butClose = new OpenDental.UI.Button();
			this.textStatus = new System.Windows.Forms.TextBox();
			this.textDescription = new OpenDental.ODtextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textOwner = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textDateEntry = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(309, 235);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 16;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// textStatus
			// 
			this.textStatus.Location = new System.Drawing.Point(95, 82);
			this.textStatus.MaxLength = 100;
			this.textStatus.Name = "textStatus";
			this.textStatus.ReadOnly = true;
			this.textStatus.Size = new System.Drawing.Size(183, 20);
			this.textStatus.TabIndex = 31;
			// 
			// textDescription
			// 
			this.textDescription.AcceptsTab = true;
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.DetectUrls = false;
			this.textDescription.Location = new System.Drawing.Point(95, 108);
			this.textDescription.Name = "textDescription";
			this.textDescription.QuickPasteType = OpenDentBusiness.QuickPasteType.CommLog;
			this.textDescription.ReadOnly = true;
			this.textDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(289, 121);
			this.textDescription.TabIndex = 30;
			this.textDescription.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(11, 60);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(83, 20);
			this.label11.TabIndex = 22;
			this.label11.Text = "Owner";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOwner
			// 
			this.textOwner.Location = new System.Drawing.Point(95, 61);
			this.textOwner.MaxLength = 100;
			this.textOwner.Name = "textOwner";
			this.textOwner.ReadOnly = true;
			this.textOwner.Size = new System.Drawing.Size(183, 20);
			this.textOwner.TabIndex = 23;
			this.textOwner.TabStop = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(11, 39);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(83, 20);
			this.label10.TabIndex = 24;
			this.label10.Text = "Date Entry";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(95, 39);
			this.textDateEntry.MaxLength = 100;
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(183, 20);
			this.textDateEntry.TabIndex = 25;
			this.textDateEntry.TabStop = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(12, 105);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(83, 20);
			this.label9.TabIndex = 26;
			this.label9.Text = "Description";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 81);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 20);
			this.label5.TabIndex = 27;
			this.label5.Text = "Status";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(11, 17);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(83, 20);
			this.label19.TabIndex = 28;
			this.label19.Text = "JobNum";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textJobNum
			// 
			this.textJobNum.Location = new System.Drawing.Point(95, 17);
			this.textJobNum.MaxLength = 100;
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.ReadOnly = true;
			this.textJobNum.Size = new System.Drawing.Size(183, 20);
			this.textJobNum.TabIndex = 29;
			this.textJobNum.TabStop = false;
			// 
			// FormJobHistoryView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(400, 271);
			this.Controls.Add(this.textStatus);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.textOwner);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.textJobNum);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobHistoryView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Job Event";
			this.Load += new System.EventHandler(this.FormJobHistoryView_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TextBox textStatus;
		private ODtextBox textDescription;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textOwner;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textDateEntry;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox textJobNum;
	}
}