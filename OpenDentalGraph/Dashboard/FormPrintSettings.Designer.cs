namespace OpenDentalGraph {
	partial class FormPrintSettings {
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
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkLandscape = new System.Windows.Forms.CheckBox();
			this.textWidth = new System.Windows.Forms.TextBox();
			this.textHeight = new System.Windows.Forms.TextBox();
			this.labelHeight = new System.Windows.Forms.Label();
			this.labelWidth = new System.Windows.Forms.Label();
			this.butPrintPreview = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textMarginWidth = new System.Windows.Forms.TextBox();
			this.textMarginHeight = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textXPos = new System.Windows.Forms.TextBox();
			this.textYPos = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butExport = new System.Windows.Forms.Button();
			this.butPrint = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkLandscape
			// 
			this.checkLandscape.Checked = true;
			this.checkLandscape.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkLandscape.Location = new System.Drawing.Point(167, 89);
			this.checkLandscape.Name = "checkLandscape";
			this.checkLandscape.Size = new System.Drawing.Size(138, 17);
			this.checkLandscape.TabIndex = 0;
			this.checkLandscape.Text = "Landscape";
			this.checkLandscape.UseVisualStyleBackColor = true;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(76, 19);
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(53, 20);
			this.textWidth.TabIndex = 1;
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(76, 41);
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(53, 20);
			this.textHeight.TabIndex = 2;
			// 
			// labelHeight
			// 
			this.labelHeight.Location = new System.Drawing.Point(7, 44);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(68, 13);
			this.labelHeight.TabIndex = 3;
			this.labelHeight.Text = "Height:";
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelWidth
			// 
			this.labelWidth.Location = new System.Drawing.Point(7, 22);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(68, 13);
			this.labelWidth.TabIndex = 4;
			this.labelWidth.Text = "Width:";
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPrintPreview
			// 
			this.butPrintPreview.Location = new System.Drawing.Point(128, 158);
			this.butPrintPreview.Name = "butPrintPreview";
			this.butPrintPreview.Size = new System.Drawing.Size(83, 23);
			this.butPrintPreview.TabIndex = 5;
			this.butPrintPreview.Text = "Print Preview";
			this.butPrintPreview.UseVisualStyleBackColor = true;
			this.butPrintPreview.Click += new System.EventHandler(this.butPrintPreview_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textWidth);
			this.groupBox1.Controls.Add(this.textHeight);
			this.groupBox1.Controls.Add(this.labelHeight);
			this.groupBox1.Controls.Add(this.labelWidth);
			this.groupBox1.Location = new System.Drawing.Point(10, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(142, 68);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Chart Size";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textMarginWidth);
			this.groupBox2.Controls.Add(this.textMarginHeight);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Location = new System.Drawing.Point(164, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(141, 68);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Margins";
			// 
			// textMarginWidth
			// 
			this.textMarginWidth.Location = new System.Drawing.Point(76, 19);
			this.textMarginWidth.Name = "textMarginWidth";
			this.textMarginWidth.Size = new System.Drawing.Size(53, 20);
			this.textMarginWidth.TabIndex = 1;
			// 
			// textMarginHeight
			// 
			this.textMarginHeight.Location = new System.Drawing.Point(76, 41);
			this.textMarginHeight.Name = "textMarginHeight";
			this.textMarginHeight.Size = new System.Drawing.Size(53, 20);
			this.textMarginHeight.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Height:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 22);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(68, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Width:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.textXPos);
			this.groupBox3.Controls.Add(this.textYPos);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(10, 83);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(142, 68);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Chart Position";
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(76, 19);
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(53, 20);
			this.textXPos.TabIndex = 1;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(76, 41);
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(53, 20);
			this.textYPos.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "y";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "x";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butExport
			// 
			this.butExport.Location = new System.Drawing.Point(39, 158);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(83, 23);
			this.butExport.TabIndex = 12;
			this.butExport.Text = "Export";
			this.butExport.UseVisualStyleBackColor = true;
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butPrint
			// 
			this.butPrint.Location = new System.Drawing.Point(215, 158);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(83, 23);
			this.butPrint.TabIndex = 13;
			this.butPrint.Text = "Print";
			this.butPrint.UseVisualStyleBackColor = true;
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// FormPrintSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 187);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkLandscape);
			this.Controls.Add(this.butPrintPreview);
			this.Name = "FormPrintSettings";
			this.Text = "Print Settings";
			this.Load += new System.EventHandler(this.FormPrintSettings_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox checkLandscape;
		private System.Windows.Forms.TextBox textWidth;
		private System.Windows.Forms.TextBox textHeight;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.Button butPrintPreview;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textMarginWidth;
		private System.Windows.Forms.TextBox textMarginHeight;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textYPos;
		private System.Windows.Forms.TextBox textXPos;
		private System.Windows.Forms.Button butExport;
		private System.Windows.Forms.Button butPrint;
	}
}