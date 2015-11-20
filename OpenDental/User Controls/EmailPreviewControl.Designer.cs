namespace OpenDental {
	partial class EmailPreviewControl {
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
			this.labelSignedBy = new System.Windows.Forms.Label();
			this.textMsgDateTime = new System.Windows.Forms.TextBox();
			this.labelSent = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.contextMenuAttachments = new System.Windows.Forms.ContextMenu();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemRename = new System.Windows.Forms.MenuItem();
			this.menuItemRemove = new System.Windows.Forms.MenuItem();
			this.textSentOrReceived = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.gridAttachments = new OpenDental.UI.ODGrid();
			this.comboEmailFrom = new System.Windows.Forms.ComboBox();
			this.textBccAddress = new OpenDental.ODtextBox();
			this.textCcAddress = new OpenDental.ODtextBox();
			this.butShowImages = new OpenDental.UI.Button();
			this.textSubject = new OpenDental.ODtextBox();
			this.textSignedBy = new OpenDental.ODtextBox();
			this.textToAddress = new OpenDental.ODtextBox();
			this.textFromAddress = new OpenDental.ODtextBox();
			this.butAttach = new OpenDental.UI.Button();
			this.butSig = new OpenDental.UI.Button();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.textBodyText = new OpenDental.ODtextBox();
			this.SuspendLayout();
			// 
			// labelSignedBy
			// 
			this.labelSignedBy.Location = new System.Drawing.Point(0, 125);
			this.labelSignedBy.Name = "labelSignedBy";
			this.labelSignedBy.Size = new System.Drawing.Size(88, 14);
			this.labelSignedBy.TabIndex = 0;
			this.labelSignedBy.Text = "Signed By:";
			this.labelSignedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMsgDateTime
			// 
			this.textMsgDateTime.BackColor = System.Drawing.SystemColors.Control;
			this.textMsgDateTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textMsgDateTime.ForeColor = System.Drawing.Color.Red;
			this.textMsgDateTime.Location = new System.Drawing.Point(89, 21);
			this.textMsgDateTime.Name = "textMsgDateTime";
			this.textMsgDateTime.Size = new System.Drawing.Size(234, 13);
			this.textMsgDateTime.TabIndex = 0;
			this.textMsgDateTime.TabStop = false;
			this.textMsgDateTime.Text = "Unsent";
			// 
			// labelSent
			// 
			this.labelSent.Location = new System.Drawing.Point(0, 20);
			this.labelSent.Name = "labelSent";
			this.labelSent.Size = new System.Drawing.Size(88, 14);
			this.labelSent.TabIndex = 0;
			this.labelSent.Text = "Date / Time:";
			this.labelSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 14);
			this.label3.TabIndex = 0;
			this.label3.Text = "From:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 62);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "To:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 14);
			this.label2.TabIndex = 0;
			this.label2.Text = "Subject:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// contextMenuAttachments
			// 
			this.contextMenuAttachments.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpen,
            this.menuItemRename,
            this.menuItemRemove});
			this.contextMenuAttachments.Popup += new System.EventHandler(this.contextMenuAttachments_Popup);
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Text = "Open";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemRename
			// 
			this.menuItemRename.Index = 1;
			this.menuItemRename.Text = "Rename";
			this.menuItemRename.Click += new System.EventHandler(this.menuItemRename_Click);
			// 
			// menuItemRemove
			// 
			this.menuItemRemove.Index = 2;
			this.menuItemRemove.Text = "Remove";
			this.menuItemRemove.Click += new System.EventHandler(this.menuItemRemove_Click);
			// 
			// textSentOrReceived
			// 
			this.textSentOrReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSentOrReceived.BackColor = System.Drawing.SystemColors.Control;
			this.textSentOrReceived.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textSentOrReceived.Location = new System.Drawing.Point(89, 1);
			this.textSentOrReceived.Name = "textSentOrReceived";
			this.textSentOrReceived.Size = new System.Drawing.Size(555, 13);
			this.textSentOrReceived.TabIndex = 0;
			this.textSentOrReceived.TabStop = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(0, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 14);
			this.label5.TabIndex = 0;
			this.label5.Text = "Sent/Received:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 83);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 14);
			this.label4.TabIndex = 8;
			this.label4.Text = "CC:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0, 103);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 14);
			this.label6.TabIndex = 0;
			this.label6.Text = "BCC:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridAttachments
			// 
			this.gridAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAttachments.HasMultilineHeaders = false;
			this.gridAttachments.HScrollVisible = false;
			this.gridAttachments.Location = new System.Drawing.Point(750, 21);
			this.gridAttachments.Name = "gridAttachments";
			this.gridAttachments.ScrollValue = 0;
			this.gridAttachments.Size = new System.Drawing.Size(220, 140);
			this.gridAttachments.TabIndex = 0;
			this.gridAttachments.TabStop = false;
			this.gridAttachments.Title = "Attachments";
			this.gridAttachments.TranslationName = null;
			this.gridAttachments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAttachments_CellDoubleClick);
			this.gridAttachments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridAttachments_MouseDown);
			// 
			// comboEmailFrom
			// 
			this.comboEmailFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboEmailFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailFrom.Location = new System.Drawing.Point(391, 36);
			this.comboEmailFrom.MaxDropDownItems = 40;
			this.comboEmailFrom.Name = "comboEmailFrom";
			this.comboEmailFrom.Size = new System.Drawing.Size(356, 21);
			this.comboEmailFrom.TabIndex = 2;
			this.comboEmailFrom.SelectionChangeCommitted += new System.EventHandler(this.comboEmailFrom_SelectionChangeCommitted);
			// 
			// textBccAddress
			// 
			this.textBccAddress.AcceptsTab = true;
			this.textBccAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBccAddress.DetectUrls = false;
			this.textBccAddress.Location = new System.Drawing.Point(89, 100);
			this.textBccAddress.Multiline = false;
			this.textBccAddress.Name = "textBccAddress";
			this.textBccAddress.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textBccAddress.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textBccAddress.Size = new System.Drawing.Size(658, 20);
			this.textBccAddress.SpellCheckIsEnabled = false;
			this.textBccAddress.TabIndex = 5;
			this.textBccAddress.Text = "";
			this.textBccAddress.WordWrap = false;
			// 
			// textCcAddress
			// 
			this.textCcAddress.AcceptsTab = true;
			this.textCcAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textCcAddress.DetectUrls = false;
			this.textCcAddress.Location = new System.Drawing.Point(89, 79);
			this.textCcAddress.Multiline = false;
			this.textCcAddress.Name = "textCcAddress";
			this.textCcAddress.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textCcAddress.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textCcAddress.Size = new System.Drawing.Size(658, 20);
			this.textCcAddress.SpellCheckIsEnabled = false;
			this.textCcAddress.TabIndex = 4;
			this.textCcAddress.Text = "";
			this.textCcAddress.WordWrap = false;
			// 
			// butShowImages
			// 
			this.butShowImages.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowImages.Autosize = true;
			this.butShowImages.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowImages.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowImages.CornerRadius = 4F;
			this.butShowImages.Location = new System.Drawing.Point(8, 163);
			this.butShowImages.Name = "butShowImages";
			this.butShowImages.Size = new System.Drawing.Size(78, 22);
			this.butShowImages.TabIndex = 9;
			this.butShowImages.Text = "Show Images";
			this.butShowImages.Visible = false;
			this.butShowImages.Click += new System.EventHandler(this.butShowImages_Click);
			// 
			// textSubject
			// 
			this.textSubject.AcceptsTab = true;
			this.textSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSubject.DetectUrls = false;
			this.textSubject.Location = new System.Drawing.Point(89, 142);
			this.textSubject.Multiline = false;
			this.textSubject.Name = "textSubject";
			this.textSubject.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textSubject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textSubject.Size = new System.Drawing.Size(658, 20);
			this.textSubject.TabIndex = 7;
			this.textSubject.Text = "";
			this.textSubject.WordWrap = false;
			// 
			// textSignedBy
			// 
			this.textSignedBy.AcceptsTab = true;
			this.textSignedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSignedBy.DetectUrls = false;
			this.textSignedBy.Location = new System.Drawing.Point(89, 121);
			this.textSignedBy.Multiline = false;
			this.textSignedBy.Name = "textSignedBy";
			this.textSignedBy.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textSignedBy.ReadOnly = true;
			this.textSignedBy.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textSignedBy.Size = new System.Drawing.Size(622, 20);
			this.textSignedBy.SpellCheckIsEnabled = false;
			this.textSignedBy.TabIndex = 0;
			this.textSignedBy.TabStop = false;
			this.textSignedBy.Text = "";
			this.textSignedBy.WordWrap = false;
			// 
			// textToAddress
			// 
			this.textToAddress.AcceptsTab = true;
			this.textToAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textToAddress.DetectUrls = false;
			this.textToAddress.Location = new System.Drawing.Point(89, 58);
			this.textToAddress.Multiline = false;
			this.textToAddress.Name = "textToAddress";
			this.textToAddress.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textToAddress.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textToAddress.Size = new System.Drawing.Size(658, 20);
			this.textToAddress.SpellCheckIsEnabled = false;
			this.textToAddress.TabIndex = 3;
			this.textToAddress.Text = "";
			this.textToAddress.WordWrap = false;
			// 
			// textFromAddress
			// 
			this.textFromAddress.AcceptsTab = true;
			this.textFromAddress.DetectUrls = false;
			this.textFromAddress.Location = new System.Drawing.Point(89, 37);
			this.textFromAddress.Multiline = false;
			this.textFromAddress.Name = "textFromAddress";
			this.textFromAddress.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textFromAddress.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.textFromAddress.Size = new System.Drawing.Size(300, 20);
			this.textFromAddress.SpellCheckIsEnabled = false;
			this.textFromAddress.TabIndex = 1;
			this.textFromAddress.Text = "";
			this.textFromAddress.WordWrap = false;
			this.textFromAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textFromAddress_KeyUp);
			this.textFromAddress.Leave += new System.EventHandler(this.textFromAddress_Leave);
			// 
			// butAttach
			// 
			this.butAttach.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAttach.Autosize = true;
			this.butAttach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttach.CornerRadius = 4F;
			this.butAttach.Location = new System.Drawing.Point(750, 0);
			this.butAttach.Name = "butAttach";
			this.butAttach.Size = new System.Drawing.Size(75, 20);
			this.butAttach.TabIndex = 10;
			this.butAttach.Text = "Attach...";
			this.butAttach.Click += new System.EventHandler(this.butAttach_Click);
			// 
			// butSig
			// 
			this.butSig.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSig.Autosize = true;
			this.butSig.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSig.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSig.CornerRadius = 4F;
			this.butSig.Location = new System.Drawing.Point(714, 121);
			this.butSig.Name = "butSig";
			this.butSig.Size = new System.Drawing.Size(33, 20);
			this.butSig.TabIndex = 6;
			this.butSig.Text = "Sig";
			this.butSig.Click += new System.EventHandler(this.butSig_Click);
			// 
			// webBrowser
			// 
			this.webBrowser.AllowWebBrowserDrop = false;
			this.webBrowser.Location = new System.Drawing.Point(950, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.ScriptErrorsSuppressed = true;
			this.webBrowser.Size = new System.Drawing.Size(20, 20);
			this.webBrowser.TabIndex = 0;
			this.webBrowser.TabStop = false;
			this.webBrowser.Visible = false;
			this.webBrowser.WebBrowserShortcutsEnabled = false;
			this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
			this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
			// 
			// textBodyText
			// 
			this.textBodyText.AcceptsTab = true;
			this.textBodyText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBodyText.DetectUrls = false;
			this.textBodyText.Location = new System.Drawing.Point(89, 163);
			this.textBodyText.Name = "textBodyText";
			this.textBodyText.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textBodyText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textBodyText.Size = new System.Drawing.Size(884, 242);
			this.textBodyText.TabIndex = 8;
			this.textBodyText.Text = "";
			this.textBodyText.TextChanged += new System.EventHandler(this.textBodyText_TextChanged);
			// 
			// EmailPreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboEmailFrom);
			this.Controls.Add(this.textBccAddress);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textCcAddress);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butShowImages);
			this.Controls.Add(this.textSentOrReceived);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textSubject);
			this.Controls.Add(this.textSignedBy);
			this.Controls.Add(this.textToAddress);
			this.Controls.Add(this.textFromAddress);
			this.Controls.Add(this.butAttach);
			this.Controls.Add(this.gridAttachments);
			this.Controls.Add(this.labelSignedBy);
			this.Controls.Add(this.butSig);
			this.Controls.Add(this.webBrowser);
			this.Controls.Add(this.textBodyText);
			this.Controls.Add(this.textMsgDateTime);
			this.Controls.Add(this.labelSent);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Name = "EmailPreviewControl";
			this.Size = new System.Drawing.Size(973, 405);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenu contextMenuAttachments;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemRename;
		private System.Windows.Forms.MenuItem menuItemRemove;
		private UI.ODGrid gridAttachments;
		private UI.Button butAttach;
		private ODtextBox textFromAddress;
		private ODtextBox textToAddress;
		private ODtextBox textSignedBy;
		private ODtextBox textSubject;
		private System.Windows.Forms.Label labelSignedBy;
		private UI.Button butSig;
		private System.Windows.Forms.WebBrowser webBrowser;
		private ODtextBox textBodyText;
		private System.Windows.Forms.TextBox textMsgDateTime;
		private System.Windows.Forms.Label labelSent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSentOrReceived;
		private System.Windows.Forms.Label label5;
		private UI.Button butShowImages;
		private ODtextBox textCcAddress;
		private System.Windows.Forms.Label label4;
		private ODtextBox textBccAddress;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox comboEmailFrom;

	}
}
