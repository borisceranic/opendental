﻿namespace OpenDentalGraph {
	partial class DashboardCellCtrl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.panelPrint = new System.Windows.Forms.Panel();
			this.panelEditCell = new System.Windows.Forms.Panel();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.butDrag = new System.Windows.Forms.Button();
			this.butDeleteCell = new System.Windows.Forms.Button();
			this.butDeleteRow = new System.Windows.Forms.Button();
			this.butDeleteColumn = new System.Windows.Forms.Button();
			this.butRefresh = new System.Windows.Forms.Button();
			this.butSaveImage = new System.Windows.Forms.Button();
			this.butEdit = new System.Windows.Forms.Button();
			this.butPrint = new System.Windows.Forms.Button();
			this.butPrintPreview = new System.Windows.Forms.Button();
			this.butPageSetup = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.panelPrint.SuspendLayout();
			this.panelEditCell.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// panelPrint
			// 
			this.panelPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panelPrint.Controls.Add(this.butRefresh);
			this.panelPrint.Controls.Add(this.butSaveImage);
			this.panelPrint.Controls.Add(this.butEdit);
			this.panelPrint.Controls.Add(this.butPrint);
			this.panelPrint.Controls.Add(this.butPrintPreview);
			this.panelPrint.Controls.Add(this.butPageSetup);
			this.panelPrint.Location = new System.Drawing.Point(164, 0);
			this.panelPrint.Name = "panelPrint";
			this.panelPrint.Size = new System.Drawing.Size(210, 30);
			this.panelPrint.TabIndex = 20;
			// 
			// panelEditCell
			// 
			this.panelEditCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panelEditCell.Controls.Add(this.butDrag);
			this.panelEditCell.Controls.Add(this.butDeleteCell);
			this.panelEditCell.Controls.Add(this.butDeleteRow);
			this.panelEditCell.Controls.Add(this.butDeleteColumn);
			this.panelEditCell.Location = new System.Drawing.Point(22, 0);
			this.panelEditCell.Name = "panelEditCell";
			this.panelEditCell.Size = new System.Drawing.Size(136, 30);
			this.panelEditCell.TabIndex = 21;
			// 
			// butDrag
			// 
			this.butDrag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDrag.BackColor = System.Drawing.SystemColors.Control;
			this.butDrag.Cursor = System.Windows.Forms.Cursors.SizeAll;
			this.butDrag.Enabled = false;
			this.butDrag.Image = global::OpenDentalGraph.Properties.Resources.drag;
			this.butDrag.Location = new System.Drawing.Point(106, 0);
			this.butDrag.Name = "butDrag";
			this.butDrag.Size = new System.Drawing.Size(30, 30);
			this.butDrag.TabIndex = 4;
			this.butDrag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butDrag, "Drag/Move");
			this.butDrag.UseVisualStyleBackColor = false;
			this.butDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butDrag_MouseDown);
			this.butDrag.MouseEnter += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			this.butDrag.MouseLeave += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			// 
			// butDeleteCell
			// 
			this.butDeleteCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDeleteCell.BackColor = System.Drawing.SystemColors.Control;
			this.butDeleteCell.Enabled = false;
			this.butDeleteCell.Image = global::OpenDentalGraph.Properties.Resources.deleteX18;
			this.butDeleteCell.Location = new System.Drawing.Point(70, 0);
			this.butDeleteCell.Name = "butDeleteCell";
			this.butDeleteCell.Size = new System.Drawing.Size(30, 30);
			this.butDeleteCell.TabIndex = 2;
			this.butDeleteCell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butDeleteCell, "Delete Cell Contents");
			this.butDeleteCell.UseVisualStyleBackColor = false;
			this.butDeleteCell.Click += new System.EventHandler(this.butDeleteCell_Click);
			this.butDeleteCell.MouseEnter += new System.EventHandler(this.butDeleteCell_MouseEnter);
			this.butDeleteCell.MouseLeave += new System.EventHandler(this.butDeleteCell_MouseLeave);
			// 
			// butDeleteRow
			// 
			this.butDeleteRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDeleteRow.BackColor = System.Drawing.SystemColors.Control;
			this.butDeleteRow.Image = global::OpenDentalGraph.Properties.Resources.deleteRow;
			this.butDeleteRow.Location = new System.Drawing.Point(34, 0);
			this.butDeleteRow.Name = "butDeleteRow";
			this.butDeleteRow.Size = new System.Drawing.Size(30, 30);
			this.butDeleteRow.TabIndex = 1;
			this.butDeleteRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butDeleteRow, "Delete Row");
			this.butDeleteRow.UseVisualStyleBackColor = false;
			this.butDeleteRow.Click += new System.EventHandler(this.butDeleteRow_Click);
			this.butDeleteRow.MouseEnter += new System.EventHandler(this.butDeleteRow_MouseEnter);
			this.butDeleteRow.MouseLeave += new System.EventHandler(this.butDeleteRow_MouseLeave);
			// 
			// butDeleteColumn
			// 
			this.butDeleteColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDeleteColumn.BackColor = System.Drawing.SystemColors.Control;
			this.butDeleteColumn.Image = global::OpenDentalGraph.Properties.Resources.deleteColumn;
			this.butDeleteColumn.Location = new System.Drawing.Point(0, 0);
			this.butDeleteColumn.Name = "butDeleteColumn";
			this.butDeleteColumn.Size = new System.Drawing.Size(30, 30);
			this.butDeleteColumn.TabIndex = 0;
			this.butDeleteColumn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butDeleteColumn, "Delete Column");
			this.butDeleteColumn.UseVisualStyleBackColor = false;
			this.butDeleteColumn.Click += new System.EventHandler(this.butDeleteColumn_Click);
			this.butDeleteColumn.MouseEnter += new System.EventHandler(this.butDeleteColumn_MouseEnter);
			this.butDeleteColumn.MouseLeave += new System.EventHandler(this.butDeleteColumn_MouseLeave);
			// 
			// butRefresh
			// 
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.BackColor = System.Drawing.SystemColors.Control;
			this.butRefresh.Image = global::OpenDentalGraph.Properties.Resources.refresh;
			this.butRefresh.Location = new System.Drawing.Point(1, 0);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(30, 30);
			this.butRefresh.TabIndex = 9;
			this.butRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butRefresh, "Save Image");
			this.butRefresh.UseVisualStyleBackColor = false;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSaveImage
			// 
			this.butSaveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveImage.BackColor = System.Drawing.SystemColors.Control;
			this.butSaveImage.Image = global::OpenDentalGraph.Properties.Resources.image;
			this.butSaveImage.Location = new System.Drawing.Point(37, 0);
			this.butSaveImage.Name = "butSaveImage";
			this.butSaveImage.Size = new System.Drawing.Size(30, 30);
			this.butSaveImage.TabIndex = 8;
			this.butSaveImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butSaveImage, "Save Image");
			this.butSaveImage.UseVisualStyleBackColor = false;
			this.butSaveImage.Click += new System.EventHandler(this.butSaveImage_Click);
			// 
			// butEdit
			// 
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEdit.BackColor = System.Drawing.SystemColors.Control;
			this.butEdit.Enabled = false;
			this.butEdit.Image = global::OpenDentalGraph.Properties.Resources.editPencil;
			this.butEdit.Location = new System.Drawing.Point(180, 0);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(30, 30);
			this.butEdit.TabIndex = 3;
			this.butEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butEdit, "Edit Cell Contents");
			this.butEdit.UseVisualStyleBackColor = false;
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			this.butEdit.MouseEnter += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			this.butEdit.MouseLeave += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.BackColor = System.Drawing.SystemColors.Control;
			this.butPrint.Image = global::OpenDentalGraph.Properties.Resources.print;
			this.butPrint.Location = new System.Drawing.Point(145, 0);
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
			this.butPrintPreview.Location = new System.Drawing.Point(109, 0);
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
			this.butPageSetup.Location = new System.Drawing.Point(73, 0);
			this.butPageSetup.Name = "butPageSetup";
			this.butPageSetup.Size = new System.Drawing.Size(30, 30);
			this.butPageSetup.TabIndex = 7;
			this.butPageSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.butPageSetup, "Page Setup");
			this.butPageSetup.UseVisualStyleBackColor = false;
			this.butPageSetup.Click += new System.EventHandler(this.butPageSetup_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Image = global::OpenDentalGraph.Properties.Resources.addChart;
			this.pictureBox.Location = new System.Drawing.Point(3, 3);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(371, 139);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox.TabIndex = 18;
			this.pictureBox.TabStop = false;
			// 
			// DashboardCellCtrl
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.panelEditCell);
			this.Controls.Add(this.panelPrint);
			this.Controls.Add(this.pictureBox);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "DashboardCellCtrl";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(377, 145);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DashboardCell_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DashboardCell_DragEnter);
			this.DragLeave += new System.EventHandler(this.DashboardCell_DragLeave);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DashboardCell_Paint);
			this.MouseEnter += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			this.MouseLeave += new System.EventHandler(this.DashboardCell_MouseEnterLeave);
			this.panelPrint.ResumeLayout(false);
			this.panelEditCell.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button butDrag;
		private System.Windows.Forms.Button butDeleteColumn;
		private System.Windows.Forms.Button butDeleteCell;
		private System.Windows.Forms.Button butDeleteRow;
		private System.Windows.Forms.Button butEdit;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button butPrint;
		private System.Windows.Forms.Button butPrintPreview;
		private System.Windows.Forms.Button butPageSetup;
		private System.Windows.Forms.Panel panelPrint;
		private System.Windows.Forms.Button butSaveImage;
		private System.Windows.Forms.Panel panelEditCell;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button butRefresh;
	}
}