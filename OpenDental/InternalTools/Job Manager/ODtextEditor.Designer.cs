namespace OpenDental {
	partial class OdtextEditor {
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
			this.comboFontSize = new System.Windows.Forms.ComboBox();
			this.butHighlightSelect = new System.Windows.Forms.Button();
			this.comboFontType = new System.Windows.Forms.ComboBox();
			this.butColorSelect = new System.Windows.Forms.Button();
			this.butHighlight = new System.Windows.Forms.Button();
			this.butFont = new System.Windows.Forms.Button();
			this.butBullet = new System.Windows.Forms.Button();
			this.butStrikeout = new System.Windows.Forms.Button();
			this.butRedo = new System.Windows.Forms.Button();
			this.butColor = new System.Windows.Forms.Button();
			this.butUnderline = new System.Windows.Forms.Button();
			this.butItalics = new System.Windows.Forms.Button();
			this.butPaste = new System.Windows.Forms.Button();
			this.butBold = new System.Windows.Forms.Button();
			this.butUndo = new System.Windows.Forms.Button();
			this.butCopy = new System.Windows.Forms.Button();
			this.butCut = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.labelWarning = new System.Windows.Forms.Label();
			this.butSave = new System.Windows.Forms.Button();
			this.textDescription = new OpenDental.ODtextBox();
			this.SuspendLayout();
			// 
			// comboFontSize
			// 
			this.comboFontSize.FormattingEnabled = true;
			this.comboFontSize.Location = new System.Drawing.Point(556, 4);
			this.comboFontSize.Name = "comboFontSize";
			this.comboFontSize.Size = new System.Drawing.Size(39, 21);
			this.comboFontSize.TabIndex = 186;
			// 
			// butHighlightSelect
			// 
			this.butHighlightSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butHighlightSelect.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butHighlightSelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butHighlightSelect.Location = new System.Drawing.Point(702, 2);
			this.butHighlightSelect.Name = "butHighlightSelect";
			this.butHighlightSelect.Size = new System.Drawing.Size(10, 24);
			this.butHighlightSelect.TabIndex = 184;
			this.butHighlightSelect.Text = "Cut";
			this.butHighlightSelect.Click += new System.EventHandler(this.butHighlightSelect_Click);
			this.butHighlightSelect.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butHighlightSelect.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// comboFontType
			// 
			this.comboFontType.FormattingEnabled = true;
			this.comboFontType.Location = new System.Drawing.Point(442, 4);
			this.comboFontType.Name = "comboFontType";
			this.comboFontType.Size = new System.Drawing.Size(113, 21);
			this.comboFontType.TabIndex = 183;
			// 
			// butColorSelect
			// 
			this.butColorSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColorSelect.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butColorSelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butColorSelect.Location = new System.Drawing.Point(627, 2);
			this.butColorSelect.Name = "butColorSelect";
			this.butColorSelect.Size = new System.Drawing.Size(10, 24);
			this.butColorSelect.TabIndex = 182;
			this.butColorSelect.Text = "Cut";
			this.butColorSelect.Click += new System.EventHandler(this.butColorSelect_Click);
			this.butColorSelect.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butColorSelect.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butHighlight
			// 
			this.butHighlight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butHighlight.Location = new System.Drawing.Point(645, 2);
			this.butHighlight.Name = "butHighlight";
			this.butHighlight.Size = new System.Drawing.Size(57, 24);
			this.butHighlight.TabIndex = 181;
			this.butHighlight.Text = "Highlight";
			this.butHighlight.Click += new System.EventHandler(this.butHighlight_Click);
			this.butHighlight.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butHighlight.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butFont
			// 
			this.butFont.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butFont.Location = new System.Drawing.Point(400, 2);
			this.butFont.Name = "butFont";
			this.butFont.Size = new System.Drawing.Size(41, 24);
			this.butFont.TabIndex = 178;
			this.butFont.Text = "Font";
			this.butFont.Click += new System.EventHandler(this.butFont_Click);
			this.butFont.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butFont.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butBullet
			// 
			this.butBullet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butBullet.Location = new System.Drawing.Point(345, 2);
			this.butBullet.Name = "butBullet";
			this.butBullet.Size = new System.Drawing.Size(55, 24);
			this.butBullet.TabIndex = 179;
			this.butBullet.Text = "Bulleted";
			this.butBullet.Click += new System.EventHandler(this.butBullet_Click);
			this.butBullet.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butBullet.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butStrikeout
			// 
			this.butStrikeout.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butStrikeout.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butStrikeout.Location = new System.Drawing.Point(315, 2);
			this.butStrikeout.Name = "butStrikeout";
			this.butStrikeout.Size = new System.Drawing.Size(30, 24);
			this.butStrikeout.TabIndex = 185;
			this.butStrikeout.Text = "abc";
			this.butStrikeout.Click += new System.EventHandler(this.butStrikeout_Click);
			this.butStrikeout.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butStrikeout.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butRedo
			// 
			this.butRedo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butRedo.Location = new System.Drawing.Point(205, 2);
			this.butRedo.Name = "butRedo";
			this.butRedo.Size = new System.Drawing.Size(41, 24);
			this.butRedo.TabIndex = 177;
			this.butRedo.Text = "Redo";
			this.butRedo.Click += new System.EventHandler(this.butRedo_Click);
			this.butRedo.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butRedo.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butColor.Location = new System.Drawing.Point(601, 2);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(26, 24);
			this.butColor.TabIndex = 170;
			this.butColor.Text = "A";
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			this.butColor.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butColor.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butUnderline
			// 
			this.butUnderline.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butUnderline.Font = new System.Drawing.Font("Times New Roman", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butUnderline.Location = new System.Drawing.Point(292, 2);
			this.butUnderline.Name = "butUnderline";
			this.butUnderline.Size = new System.Drawing.Size(23, 24);
			this.butUnderline.TabIndex = 180;
			this.butUnderline.Text = "U";
			this.butUnderline.Click += new System.EventHandler(this.butUnderline_Click);
			this.butUnderline.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butUnderline.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butItalics
			// 
			this.butItalics.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butItalics.Font = new System.Drawing.Font("Times New Roman", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butItalics.Location = new System.Drawing.Point(269, 2);
			this.butItalics.Name = "butItalics";
			this.butItalics.Size = new System.Drawing.Size(23, 24);
			this.butItalics.TabIndex = 176;
			this.butItalics.Text = "I";
			this.butItalics.Click += new System.EventHandler(this.butItalics_Click);
			this.butItalics.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butItalics.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butPaste
			// 
			this.butPaste.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butPaste.Image = global::OpenDental.Properties.Resources.butPaste;
			this.butPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPaste.Location = new System.Drawing.Point(102, 2);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(62, 24);
			this.butPaste.TabIndex = 175;
			this.butPaste.Text = "Paste";
			this.butPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			this.butPaste.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butPaste.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butBold
			// 
			this.butBold.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butBold.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBold.Location = new System.Drawing.Point(246, 2);
			this.butBold.Name = "butBold";
			this.butBold.Size = new System.Drawing.Size(23, 24);
			this.butBold.TabIndex = 173;
			this.butBold.Text = "B";
			this.butBold.Click += new System.EventHandler(this.butBold_Click);
			this.butBold.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butBold.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butUndo
			// 
			this.butUndo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butUndo.Location = new System.Drawing.Point(164, 2);
			this.butUndo.Name = "butUndo";
			this.butUndo.Size = new System.Drawing.Size(41, 24);
			this.butUndo.TabIndex = 172;
			this.butUndo.Text = "Undo";
			this.butUndo.Click += new System.EventHandler(this.butUndo_Click);
			this.butUndo.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butUndo.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butCopy
			// 
			this.butCopy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butCopy.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(42, 2);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(60, 24);
			this.butCopy.TabIndex = 174;
			this.butCopy.Text = "Copy";
			this.butCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			this.butCopy.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butCopy.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// butCut
			// 
			this.butCut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butCut.Location = new System.Drawing.Point(1, 2);
			this.butCut.Name = "butCut";
			this.butCut.Size = new System.Drawing.Size(41, 24);
			this.butCut.TabIndex = 171;
			this.butCut.Text = "Cut";
			this.butCut.Click += new System.EventHandler(this.butCut_Click);
			this.butCut.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butCut.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// labelWarning
			// 
			this.labelWarning.AutoSize = true;
			this.labelWarning.Location = new System.Drawing.Point(718, 8);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(47, 13);
			this.labelWarning.TabIndex = 187;
			this.labelWarning.Text = "Warning";
			// 
			// butSave
			// 
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butSave.Location = new System.Drawing.Point(800, 2);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(55, 24);
			this.butSave.TabIndex = 188;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			this.butSave.MouseEnter += new System.EventHandler(this.HoverColorEnter);
			this.butSave.MouseLeave += new System.EventHandler(this.HoverColorLeave);
			// 
			// textDescription
			// 
			this.textDescription.AcceptsTab = true;
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.BackColor = System.Drawing.SystemColors.Window;
			this.textDescription.DetectUrls = false;
			this.textDescription.HideSelection = false;
			this.textDescription.Location = new System.Drawing.Point(0, 25);
			this.textDescription.Name = "textDescription";
			this.textDescription.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(855, 498);
			this.textDescription.TabIndex = 169;
			this.textDescription.Text = "";
			// 
			// OdtextEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.labelWarning);
			this.Controls.Add(this.comboFontSize);
			this.Controls.Add(this.butHighlightSelect);
			this.Controls.Add(this.comboFontType);
			this.Controls.Add(this.butColorSelect);
			this.Controls.Add(this.butHighlight);
			this.Controls.Add(this.butFont);
			this.Controls.Add(this.butBullet);
			this.Controls.Add(this.butStrikeout);
			this.Controls.Add(this.butRedo);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.butUnderline);
			this.Controls.Add(this.butItalics);
			this.Controls.Add(this.butPaste);
			this.Controls.Add(this.butBold);
			this.Controls.Add(this.butUndo);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.butCut);
			this.MinimumSize = new System.Drawing.Size(450, 290);
			this.Name = "OdtextEditor";
			this.Size = new System.Drawing.Size(855, 523);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.OdtextEditor_Layout);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboFontSize;
		private System.Windows.Forms.Button butHighlightSelect;
		private System.Windows.Forms.ComboBox comboFontType;
		private System.Windows.Forms.Button butColorSelect;
		private System.Windows.Forms.Button butHighlight;
		private System.Windows.Forms.Button butFont;
		private System.Windows.Forms.Button butBullet;
		private System.Windows.Forms.Button butStrikeout;
		private System.Windows.Forms.Button butRedo;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.Button butUnderline;
		private System.Windows.Forms.Button butItalics;
		private System.Windows.Forms.Button butPaste;
		private System.Windows.Forms.Button butBold;
		private System.Windows.Forms.Button butUndo;
		private ODtextBox textDescription;
		private System.Windows.Forms.Button butCopy;
		private System.Windows.Forms.Button butCut;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Label labelWarning;
		private System.Windows.Forms.Button butSave;


	}
}
