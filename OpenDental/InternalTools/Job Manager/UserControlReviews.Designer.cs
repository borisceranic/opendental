namespace OpenDental {
	partial class UserControlReviews {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.gridReviews = new OpenDental.UI.ODGrid();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.setSeenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridReviews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridReviews.ContextMenuStrip = this.contextMenuStrip;
			this.gridReviews.HasMultilineHeaders = false;
			this.gridReviews.HScrollVisible = true;
			this.gridReviews.Location = new System.Drawing.Point(4, 5);
			this.gridReviews.Name = "gridMain";
			this.gridReviews.ScrollValue = 0;
			this.gridReviews.Size = new System.Drawing.Size(545, 382);
			this.gridReviews.TabIndex = 11;
			this.gridReviews.TabStop = false;
			this.gridReviews.Title = "Reviews";
			this.gridReviews.TranslationName = null;
			this.gridReviews.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setSeenToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.Size = new System.Drawing.Size(119, 26);
			// 
			// setSeenToolStripMenuItem
			// 
			this.setSeenToolStripMenuItem.Name = "setSeenToolStripMenuItem";
			this.setSeenToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.setSeenToolStripMenuItem.Text = "Set Seen";
			this.setSeenToolStripMenuItem.Click += new System.EventHandler(this.setSeenToolStripMenuItem_Click);
			// 
			// UserControlReviews
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridReviews);
			this.MinimumSize = new System.Drawing.Size(450, 290);
			this.Name = "UserControlReviews";
			this.Size = new System.Drawing.Size(552, 390);
			this.Load += new System.EventHandler(this.UserControlReviews_Load);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridReviews;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem setSeenToolStripMenuItem;


	}
}
