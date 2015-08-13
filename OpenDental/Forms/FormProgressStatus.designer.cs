namespace OpenDental{
	partial class FormProgressStatus {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgressStatus));
			this.labelMsg = new System.Windows.Forms.Label();
			this.progressBarMarquee = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// labelMsg
			// 
			this.labelMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMsg.Location = new System.Drawing.Point(52, 11);
			this.labelMsg.Name = "labelMsg";
			this.labelMsg.Size = new System.Drawing.Size(366, 36);
			this.labelMsg.TabIndex = 1;
			this.labelMsg.Text = "Please Wait...";
			this.labelMsg.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// progressBarMarquee
			// 
			this.progressBarMarquee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarMarquee.Location = new System.Drawing.Point(55, 50);
			this.progressBarMarquee.MarqueeAnimationSpeed = 50;
			this.progressBarMarquee.Name = "progressBarMarquee";
			this.progressBarMarquee.Size = new System.Drawing.Size(363, 23);
			this.progressBarMarquee.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBarMarquee.TabIndex = 2;
			// 
			// FormProgressStatus
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ClientSize = new System.Drawing.Size(469, 104);
			this.Controls.Add(this.progressBarMarquee);
			this.Controls.Add(this.labelMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormProgressStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelMsg;
		private System.Windows.Forms.ProgressBar progressBarMarquee;

	}
}