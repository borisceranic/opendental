namespace OpenDental{
	partial class FormEmailInbox {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailInbox));
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.labelInboxComputerName = new System.Windows.Forms.Label();
			this.labelThisComputer = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textComputerName = new OpenDental.ODtextBox();
			this.textComputerNameReceive = new OpenDental.ODtextBox();
			this.splitContainerNoFlicker = new OpenDental.SplitContainerNoFlicker();
			this.gridEmailMessages = new OpenDental.UI.ODGrid();
			this.emailPreview = new OpenDental.EmailPreviewControl();
			this.butDelete = new OpenDental.UI.Button();
			this.butChangePat = new OpenDental.UI.Button();
			this.butMarkUnread = new OpenDental.UI.Button();
			this.butMarkRead = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.odToolBarButton1 = new OpenDental.UI.ODToolBarButton();
			this.comboEmailAddress = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker)).BeginInit();
			this.splitContainerNoFlicker.Panel1.SuspendLayout();
			this.splitContainerNoFlicker.Panel2.SuspendLayout();
			this.splitContainerNoFlicker.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
			// 
			// labelInboxComputerName
			// 
			this.labelInboxComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelInboxComputerName.Location = new System.Drawing.Point(410, 635);
			this.labelInboxComputerName.Name = "labelInboxComputerName";
			this.labelInboxComputerName.Size = new System.Drawing.Size(304, 16);
			this.labelInboxComputerName.TabIndex = 144;
			this.labelInboxComputerName.Text = "Computer Name Where New Email Is Received:";
			this.labelInboxComputerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelThisComputer
			// 
			this.labelThisComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelThisComputer.Location = new System.Drawing.Point(407, 651);
			this.labelThisComputer.Name = "labelThisComputer";
			this.labelThisComputer.Size = new System.Drawing.Size(307, 16);
			this.labelThisComputer.TabIndex = 145;
			this.labelThisComputer.Text = "This Computer Name:";
			this.labelThisComputer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, -2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(638, 16);
			this.label1.TabIndex = 148;
			this.label1.Text = "Email Inbox is limited and should not be used as your primary email solution yet." +
    "  Work in progress.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textComputerName
			// 
			this.textComputerName.AcceptsTab = true;
			this.textComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textComputerName.BackColor = System.Drawing.SystemColors.Control;
			this.textComputerName.DetectLinksEnabled = false;
			this.textComputerName.DetectUrls = false;
			this.textComputerName.Location = new System.Drawing.Point(714, 651);
			this.textComputerName.Multiline = false;
			this.textComputerName.Name = "textComputerName";
			this.textComputerName.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textComputerName.ReadOnly = true;
			this.textComputerName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textComputerName.Size = new System.Drawing.Size(142, 18);
			this.textComputerName.SpellCheckIsEnabled = false;
			this.textComputerName.TabIndex = 150;
			this.textComputerName.Text = "";
			// 
			// textComputerNameReceive
			// 
			this.textComputerNameReceive.AcceptsTab = true;
			this.textComputerNameReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textComputerNameReceive.BackColor = System.Drawing.SystemColors.Control;
			this.textComputerNameReceive.DetectLinksEnabled = false;
			this.textComputerNameReceive.DetectUrls = false;
			this.textComputerNameReceive.Location = new System.Drawing.Point(714, 635);
			this.textComputerNameReceive.Multiline = false;
			this.textComputerNameReceive.Name = "textComputerNameReceive";
			this.textComputerNameReceive.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textComputerNameReceive.ReadOnly = true;
			this.textComputerNameReceive.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textComputerNameReceive.Size = new System.Drawing.Size(142, 18);
			this.textComputerNameReceive.SpellCheckIsEnabled = false;
			this.textComputerNameReceive.TabIndex = 149;
			this.textComputerNameReceive.Text = "";
			// 
			// splitContainerNoFlicker
			// 
			this.splitContainerNoFlicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerNoFlicker.Location = new System.Drawing.Point(12, 42);
			this.splitContainerNoFlicker.Name = "splitContainerNoFlicker";
			this.splitContainerNoFlicker.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerNoFlicker.Panel1
			// 
			this.splitContainerNoFlicker.Panel1.Controls.Add(this.gridEmailMessages);
			this.splitContainerNoFlicker.Panel1MinSize = 200;
			// 
			// splitContainerNoFlicker.Panel2
			// 
			this.splitContainerNoFlicker.Panel2.Controls.Add(this.emailPreview);
			this.splitContainerNoFlicker.Panel2Collapsed = true;
			this.splitContainerNoFlicker.Panel2MinSize = 200;
			this.splitContainerNoFlicker.Size = new System.Drawing.Size(950, 592);
			this.splitContainerNoFlicker.SplitterDistance = 200;
			this.splitContainerNoFlicker.TabIndex = 0;
			this.splitContainerNoFlicker.TabStop = false;
			// 
			// gridEmailMessages
			// 
			this.gridEmailMessages.AllowSortingByColumn = true;
			this.gridEmailMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridEmailMessages.HasAddButton = false;
			this.gridEmailMessages.HasMultilineHeaders = false;
			this.gridEmailMessages.HScrollVisible = false;
			this.gridEmailMessages.Location = new System.Drawing.Point(0, 0);
			this.gridEmailMessages.Name = "gridEmailMessages";
			this.gridEmailMessages.ScrollValue = 0;
			this.gridEmailMessages.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridEmailMessages.Size = new System.Drawing.Size(950, 592);
			this.gridEmailMessages.TabIndex = 140;
			this.gridEmailMessages.Title = "Email Messages";
			this.gridEmailMessages.TranslationName = "TableApptProcs";
			this.gridEmailMessages.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEmailMessages_CellDoubleClick);
			this.gridEmailMessages.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEmailMessages_CellClick);
			// 
			// emailPreview
			// 
			this.emailPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailPreview.BccAddress = "";
			this.emailPreview.BodyText = "";
			this.emailPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.emailPreview.CcAddress = "";
			this.emailPreview.Location = new System.Drawing.Point(0, 0);
			this.emailPreview.Name = "emailPreview";
			this.emailPreview.Size = new System.Drawing.Size(937, 405);
			this.emailPreview.Subject = "";
			this.emailPreview.TabIndex = 1;
			this.emailPreview.ToAddress = "";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 640);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(83, 24);
			this.butDelete.TabIndex = 147;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butChangePat
			// 
			this.butChangePat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butChangePat.Autosize = true;
			this.butChangePat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePat.CornerRadius = 4F;
			this.butChangePat.Location = new System.Drawing.Point(733, 16);
			this.butChangePat.Name = "butChangePat";
			this.butChangePat.Size = new System.Drawing.Size(75, 24);
			this.butChangePat.TabIndex = 146;
			this.butChangePat.Text = "Change Pat";
			this.butChangePat.Click += new System.EventHandler(this.butChangePat_Click);
			// 
			// butMarkUnread
			// 
			this.butMarkUnread.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkUnread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMarkUnread.Autosize = true;
			this.butMarkUnread.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkUnread.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkUnread.CornerRadius = 4F;
			this.butMarkUnread.Location = new System.Drawing.Point(810, 16);
			this.butMarkUnread.Name = "butMarkUnread";
			this.butMarkUnread.Size = new System.Drawing.Size(75, 24);
			this.butMarkUnread.TabIndex = 143;
			this.butMarkUnread.Text = "Mark Unread";
			this.butMarkUnread.Click += new System.EventHandler(this.butMarkUnread_Click);
			// 
			// butMarkRead
			// 
			this.butMarkRead.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMarkRead.Autosize = true;
			this.butMarkRead.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkRead.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkRead.CornerRadius = 4F;
			this.butMarkRead.Location = new System.Drawing.Point(887, 16);
			this.butMarkRead.Name = "butMarkRead";
			this.butMarkRead.Size = new System.Drawing.Size(75, 24);
			this.butMarkRead.TabIndex = 142;
			this.butMarkRead.Text = "Mark Read";
			this.butMarkRead.Click += new System.EventHandler(this.butMarkRead_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(656, 16);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 141;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 640);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// odToolBarButton1
			// 
			this.odToolBarButton1.Bounds = new System.Drawing.Rectangle(0, 0, 0, 0);
			this.odToolBarButton1.DropDownMenu = null;
			this.odToolBarButton1.Enabled = true;
			this.odToolBarButton1.ImageIndex = -1;
			this.odToolBarButton1.Pushed = false;
			this.odToolBarButton1.State = OpenDental.UI.ToolBarButtonState.Normal;
			this.odToolBarButton1.Style = OpenDental.UI.ODToolBarButtonStyle.PushButton;
			this.odToolBarButton1.Tag = "";
			this.odToolBarButton1.Text = "";
			this.odToolBarButton1.ToolTipText = "";
			// 
			// comboEmailAddress
			// 
			this.comboEmailAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailAddress.Location = new System.Drawing.Point(13, 19);
			this.comboEmailAddress.MaxDropDownItems = 40;
			this.comboEmailAddress.Name = "comboEmailAddress";
			this.comboEmailAddress.Size = new System.Drawing.Size(233, 21);
			this.comboEmailAddress.TabIndex = 141;
			this.comboEmailAddress.SelectionChangeCommitted += new System.EventHandler(this.butRefresh_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(97, 644);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(304, 16);
			this.label2.TabIndex = 151;
			this.label2.Text = "Hides the email if a patient is attached.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormEmailInbox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 675);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboEmailAddress);
			this.Controls.Add(this.textComputerName);
			this.Controls.Add(this.textComputerNameReceive);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.splitContainerNoFlicker);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butChangePat);
			this.Controls.Add(this.labelThisComputer);
			this.Controls.Add(this.labelInboxComputerName);
			this.Controls.Add(this.butMarkUnread);
			this.Controls.Add(this.butMarkRead);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(990, 713);
			this.Name = "FormEmailInbox";
			this.Text = "Email Inbox";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormEmailInbox_Load);
			this.Resize += new System.EventHandler(this.FormEmailInbox_Resize);
			this.splitContainerNoFlicker.Panel1.ResumeLayout(false);
			this.splitContainerNoFlicker.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker)).EndInit();
			this.splitContainerNoFlicker.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private UI.ODGrid gridEmailMessages;
		private UI.ODToolBarButton odToolBarButton1;
		private UI.Button butRefresh;
		private UI.Button butMarkRead;
		private UI.Button butMarkUnread;
		private System.Windows.Forms.Label labelInboxComputerName;
		private System.Windows.Forms.Label labelThisComputer;
		private UI.Button butChangePat;
		private UI.Button butDelete;
		private EmailPreviewControl emailPreview;
		private SplitContainerNoFlicker splitContainerNoFlicker;
		private System.Windows.Forms.Label label1;
		private ODtextBox textComputerNameReceive;
		private ODtextBox textComputerName;
		private System.Windows.Forms.ComboBox comboEmailAddress;
		private System.Windows.Forms.Label label2;
	}
}