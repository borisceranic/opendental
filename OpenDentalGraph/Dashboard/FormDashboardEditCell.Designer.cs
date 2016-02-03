namespace OpenDentalGraph {
	partial class FormDashboardEditCell {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.panelPrint = new System.Windows.Forms.Panel();
			this.butSaveImage = new System.Windows.Forms.Button();
			this.butPrint = new System.Windows.Forms.Button();
			this.butPrintPreview = new System.Windows.Forms.Button();
			this.butPageSetup = new System.Windows.Forms.Button();
			this.butCancel = new System.Windows.Forms.Button();
			this.butOk = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.panelPrint.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.IsSplitterFixed = true;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.panelPrint);
			this.splitContainer.Panel2.Controls.Add(this.butCancel);
			this.splitContainer.Panel2.Controls.Add(this.butOk);
			this.splitContainer.Size = new System.Drawing.Size(812, 500);
			this.splitContainer.SplitterDistance = 468;
			this.splitContainer.SplitterWidth = 1;
			this.splitContainer.TabIndex = 0;
			// 
			// panelPrint
			// 
			this.panelPrint.Controls.Add(this.butSaveImage);
			this.panelPrint.Controls.Add(this.butPrint);
			this.panelPrint.Controls.Add(this.butPrintPreview);
			this.panelPrint.Controls.Add(this.butPageSetup);
			this.panelPrint.Location = new System.Drawing.Point(3, 0);
			this.panelPrint.Name = "panelPrint";
			this.panelPrint.Size = new System.Drawing.Size(137, 30);
			this.panelPrint.TabIndex = 21;
			// 
			// butSaveImage
			// 
			this.butSaveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveImage.BackColor = System.Drawing.SystemColors.Control;
			this.butSaveImage.Image = global::OpenDentalGraph.Properties.Resources.image;
			this.butSaveImage.Location = new System.Drawing.Point(-1, 0);
			this.butSaveImage.Name = "butSaveImage";
			this.butSaveImage.Size = new System.Drawing.Size(30, 30);
			this.butSaveImage.TabIndex = 8;
			this.butSaveImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butSaveImage, "Save Image");
			this.butSaveImage.UseVisualStyleBackColor = false;
			this.butSaveImage.Click += new System.EventHandler(this.butSaveImage_Click);
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.BackColor = System.Drawing.SystemColors.Control;
			this.butPrint.Image = global::OpenDentalGraph.Properties.Resources.print;
			this.butPrint.Location = new System.Drawing.Point(107, 0);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(30, 30);
			this.butPrint.TabIndex = 5;
			this.butPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butPrint, "Print");
			this.butPrint.UseVisualStyleBackColor = false;
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butPrintPreview
			// 
			this.butPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrintPreview.BackColor = System.Drawing.SystemColors.Control;
			this.butPrintPreview.Image = global::OpenDentalGraph.Properties.Resources.printpreview;
			this.butPrintPreview.Location = new System.Drawing.Point(71, 0);
			this.butPrintPreview.Name = "butPrintPreview";
			this.butPrintPreview.Size = new System.Drawing.Size(30, 30);
			this.butPrintPreview.TabIndex = 6;
			this.butPrintPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butPrintPreview, "Print Preview");
			this.butPrintPreview.UseVisualStyleBackColor = false;
			this.butPrintPreview.Click += new System.EventHandler(this.butPrintPreview_Click);
			// 
			// butPageSetup
			// 
			this.butPageSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPageSetup.BackColor = System.Drawing.SystemColors.Control;
			this.butPageSetup.Image = global::OpenDentalGraph.Properties.Resources.printsetup;
			this.butPageSetup.Location = new System.Drawing.Point(35, 0);
			this.butPageSetup.Name = "butPageSetup";
			this.butPageSetup.Size = new System.Drawing.Size(30, 30);
			this.butPageSetup.TabIndex = 7;
			this.butPageSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butPageSetup, "Page Setup");
			this.butPageSetup.UseVisualStyleBackColor = false;
			this.butPageSetup.Click += new System.EventHandler(this.butPageSetup_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(734, 4);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 1;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			// 
			// butOk
			// 
			this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.butOk.Location = new System.Drawing.Point(653, 4);
			this.butOk.Name = "butOk";
			this.butOk.Size = new System.Drawing.Size(75, 23);
			this.butOk.TabIndex = 0;
			this.butOk.Text = "OK";
			this.butOk.UseVisualStyleBackColor = true;
			// 
			// FormDashboardEditCell
			// 
			this.AcceptButton = this.butOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(812, 500);
			this.Controls.Add(this.splitContainer);
			this.Name = "FormDashboardEditCell";
			this.Text = "Edit Cell";
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.panelPrint.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.Button butCancel;
		private System.Windows.Forms.Button butOk;
		private System.Windows.Forms.Panel panelPrint;
		private System.Windows.Forms.Button butSaveImage;
		private System.Windows.Forms.Button butPrint;
		private System.Windows.Forms.Button butPrintPreview;
		private System.Windows.Forms.Button butPageSetup;
		private System.Windows.Forms.ToolTip toolTip;
	}
}