namespace OpenDental {
	partial class JobContainerControl {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobContainerControl));
			this.butClose = new System.Windows.Forms.Button();
			this.butDock = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.butRight = new System.Windows.Forms.Button();
			this.butLeft = new System.Windows.Forms.Button();
			this.panelHighlight = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butClose.Image = global::OpenDental.Properties.Resources.deleteX10;
			this.butClose.Location = new System.Drawing.Point(427, 3);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(15, 15);
			this.butClose.TabIndex = 0;
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butDock
			// 
			this.butDock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butDock.ImageList = this.imageList1;
			this.butDock.Location = new System.Drawing.Point(407, 3);
			this.butDock.Name = "butDock";
			this.butDock.Size = new System.Drawing.Size(15, 15);
			this.butDock.TabIndex = 1;
			this.butDock.UseVisualStyleBackColor = true;
			this.butDock.Click += new System.EventHandler(this.butDock_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageList1.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// butRight
			// 
			this.butRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(387, 3);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(15, 15);
			this.butRight.TabIndex = 2;
			this.butRight.UseVisualStyleBackColor = true;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(367, 3);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(15, 15);
			this.butLeft.TabIndex = 3;
			this.butLeft.UseVisualStyleBackColor = true;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// panelHighlight
			// 
			this.panelHighlight.BackColor = System.Drawing.SystemColors.Highlight;
			this.panelHighlight.Location = new System.Drawing.Point(0, 0);
			this.panelHighlight.Name = "panelHighlight";
			this.panelHighlight.Size = new System.Drawing.Size(448, 298);
			this.panelHighlight.TabIndex = 4;
			this.panelHighlight.Visible = false;
			// 
			// JobContainerControl
			// 
			this.AllowDragging = true;
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butDock);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.panelHighlight);
			this.DoubleBuffered = true;
			this.Name = "JobContainerControl";
			this.Size = new System.Drawing.Size(448, 298);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.JobContainerControl_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.JobContainerControl_DragEnter);
			this.DragLeave += new System.EventHandler(this.JobContainerControl_DragLeave);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JobContainerControl_MouseDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button butClose;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button butLeft;
		private System.Windows.Forms.Button butRight;
		private System.Windows.Forms.Button butDock;
		public System.Windows.Forms.Panel panelHighlight;

	}
}
