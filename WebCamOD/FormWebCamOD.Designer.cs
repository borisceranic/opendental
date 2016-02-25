namespace WebCamOD {
	partial class FormWebCamOD {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebCamOD));
			this.timerWebCamSnapshots = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
			this.SuspendLayout();
			// 
			// timerWebCamSnapshots
			// 
			this.timerWebCamSnapshots.Interval = 1600;
			this.timerWebCamSnapshots.Tick += new System.EventHandler(this.timerWebCamSnapshots_Tick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(362, 106);
			this.label1.TabIndex = 2;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// videoSourcePlayer
			// 
			this.videoSourcePlayer.BackColor = System.Drawing.SystemColors.ControlDark;
			this.videoSourcePlayer.ForeColor = System.Drawing.Color.DarkRed;
			this.videoSourcePlayer.Location = new System.Drawing.Point(152, 116);
			this.videoSourcePlayer.Name = "videoSourcePlayer";
			this.videoSourcePlayer.Size = new System.Drawing.Size(64, 48);
			this.videoSourcePlayer.TabIndex = 4;
			this.videoSourcePlayer.VideoSource = null;
			// 
			// FormWebCamOD
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 177);
			this.Controls.Add(this.videoSourcePlayer);
			this.Controls.Add(this.label1);
			this.Name = "FormWebCamOD";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WebCamOD";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWebCamOD_FormClosing);
			this.Load += new System.EventHandler(this.FormWebCamOD_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Timer timerWebCamSnapshots;
		private System.Windows.Forms.Label label1;
		private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
	}
}

