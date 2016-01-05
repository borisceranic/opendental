namespace OpenDental.InternalTools.Job_Manager {
	partial class UserControlJobEdit {
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
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.textEditorMain = new OpenDental.OdtextEditor();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabMain = new System.Windows.Forms.TabPage();
			this.gridNotes = new OpenDental.UI.ODGrid();
			this.tabReviews = new System.Windows.Forms.TabPage();
			this.gridReview = new OpenDental.UI.ODGrid();
			this.tabHistory = new System.Windows.Forms.TabPage();
			this.gridHistory = new OpenDental.UI.ODGrid();
			this.groupLinks = new System.Windows.Forms.GroupBox();
			this.gridBugs = new OpenDental.UI.ODGrid();
			this.gridFeatureReq = new OpenDental.UI.ODGrid();
			this.gridTasks = new OpenDental.UI.ODGrid();
			this.gridCustomerQuotes = new OpenDental.UI.ODGrid();
			this.gridWatchers = new OpenDental.UI.ODGrid();
			this.label9 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.textExpert = new System.Windows.Forms.TextBox();
			this.butExpertPick = new OpenDental.UI.Button();
			this.textOwner = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.butOwnerPick = new OpenDental.UI.Button();
			this.textPrevOwner = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textVersion = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textDateEntry = new System.Windows.Forms.TextBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.comboCategory = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textParent = new System.Windows.Forms.TextBox();
			this.butParentPick = new OpenDental.UI.Button();
			this.butParentRemove = new OpenDental.UI.Button();
			this.textEstHours = new OpenDental.ValidNumber();
			this.butActions = new OpenDental.UI.Button();
			this.textActualHours = new OpenDental.ValidNumber();
			this.butSave = new OpenDental.UI.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabReviews.SuspendLayout();
			this.tabHistory.SuspendLayout();
			this.groupLinks.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer2
			// 
			this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer2.Location = new System.Drawing.Point(3, 79);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.textEditorMain);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.tabControlMain);
			this.splitContainer2.Panel2MinSize = 250;
			this.splitContainer2.Size = new System.Drawing.Size(768, 557);
			this.splitContainer2.SplitterDistance = 281;
			this.splitContainer2.TabIndex = 301;
			// 
			// textEditorMain
			// 
			this.textEditorMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorMain.HasSaveButton = true;
			this.textEditorMain.Location = new System.Drawing.Point(0, 0);
			this.textEditorMain.MainRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
			this.textEditorMain.MainText = "";
			this.textEditorMain.MinimumSize = new System.Drawing.Size(450, 120);
			this.textEditorMain.Name = "textEditorMain";
			this.textEditorMain.ReadOnly = false;
			this.textEditorMain.Size = new System.Drawing.Size(768, 281);
			this.textEditorMain.TabIndex = 260;
			this.textEditorMain.OnTextEdited += new OpenDental.OdtextEditor.textChangedEventHandler(this.textEditorMain_OnTextEdited);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlMain.Controls.Add(this.tabMain);
			this.tabControlMain.Controls.Add(this.tabReviews);
			this.tabControlMain.Controls.Add(this.tabHistory);
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(768, 269);
			this.tabControlMain.TabIndex = 261;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.gridNotes);
			this.tabMain.Location = new System.Drawing.Point(4, 22);
			this.tabMain.Name = "tabMain";
			this.tabMain.Padding = new System.Windows.Forms.Padding(3);
			this.tabMain.Size = new System.Drawing.Size(760, 243);
			this.tabMain.TabIndex = 0;
			this.tabMain.Text = "Discussion";
			this.tabMain.UseVisualStyleBackColor = true;
			// 
			// gridNotes
			// 
			this.gridNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridNotes.HasAddButton = true;
			this.gridNotes.HasMultilineHeaders = false;
			this.gridNotes.HScrollVisible = false;
			this.gridNotes.Location = new System.Drawing.Point(3, 3);
			this.gridNotes.Name = "gridNotes";
			this.gridNotes.ScrollValue = 0;
			this.gridNotes.Size = new System.Drawing.Size(754, 237);
			this.gridNotes.TabIndex = 194;
			this.gridNotes.Title = "Discussion";
			this.gridNotes.TranslationName = "FormTaskEdit";
			this.gridNotes.TitleAddClick += new System.EventHandler(this.gridNotes_TitleAddClick);
			// 
			// tabReviews
			// 
			this.tabReviews.BackColor = System.Drawing.SystemColors.Control;
			this.tabReviews.Controls.Add(this.gridReview);
			this.tabReviews.Location = new System.Drawing.Point(4, 22);
			this.tabReviews.Name = "tabReviews";
			this.tabReviews.Padding = new System.Windows.Forms.Padding(3);
			this.tabReviews.Size = new System.Drawing.Size(760, 243);
			this.tabReviews.TabIndex = 2;
			this.tabReviews.Text = "Reviews";
			// 
			// gridReview
			// 
			this.gridReview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridReview.HasAddButton = true;
			this.gridReview.HasMultilineHeaders = false;
			this.gridReview.HScrollVisible = false;
			this.gridReview.Location = new System.Drawing.Point(3, 3);
			this.gridReview.Name = "gridReview";
			this.gridReview.ScrollValue = 0;
			this.gridReview.Size = new System.Drawing.Size(754, 237);
			this.gridReview.TabIndex = 21;
			this.gridReview.TabStop = false;
			this.gridReview.Title = "Reviews";
			this.gridReview.TranslationName = null;
			this.gridReview.TitleAddClick += new System.EventHandler(this.gridReview_TitleAddClick);
			// 
			// tabHistory
			// 
			this.tabHistory.BackColor = System.Drawing.SystemColors.Control;
			this.tabHistory.Controls.Add(this.gridHistory);
			this.tabHistory.Location = new System.Drawing.Point(4, 22);
			this.tabHistory.Name = "tabHistory";
			this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
			this.tabHistory.Size = new System.Drawing.Size(760, 243);
			this.tabHistory.TabIndex = 3;
			this.tabHistory.Text = "History";
			// 
			// gridHistory
			// 
			this.gridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridHistory.HasAddButton = false;
			this.gridHistory.HasMultilineHeaders = false;
			this.gridHistory.HScrollVisible = false;
			this.gridHistory.Location = new System.Drawing.Point(3, 3);
			this.gridHistory.Name = "gridHistory";
			this.gridHistory.ScrollValue = 0;
			this.gridHistory.Size = new System.Drawing.Size(754, 237);
			this.gridHistory.TabIndex = 19;
			this.gridHistory.TabStop = false;
			this.gridHistory.Title = "History Events";
			this.gridHistory.TranslationName = null;
			this.gridHistory.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridHistory_CellDoubleClick);
			// 
			// groupLinks
			// 
			this.groupLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupLinks.Controls.Add(this.gridBugs);
			this.groupLinks.Controls.Add(this.gridFeatureReq);
			this.groupLinks.Controls.Add(this.gridTasks);
			this.groupLinks.Controls.Add(this.gridCustomerQuotes);
			this.groupLinks.Controls.Add(this.gridWatchers);
			this.groupLinks.Controls.Add(this.label9);
			this.groupLinks.Controls.Add(this.label14);
			this.groupLinks.Controls.Add(this.textExpert);
			this.groupLinks.Controls.Add(this.butExpertPick);
			this.groupLinks.Controls.Add(this.textOwner);
			this.groupLinks.Controls.Add(this.label11);
			this.groupLinks.Controls.Add(this.butOwnerPick);
			this.groupLinks.Controls.Add(this.textPrevOwner);
			this.groupLinks.Location = new System.Drawing.Point(777, 47);
			this.groupLinks.Name = "groupLinks";
			this.groupLinks.Size = new System.Drawing.Size(233, 589);
			this.groupLinks.TabIndex = 296;
			this.groupLinks.TabStop = false;
			this.groupLinks.Text = "Links";
			this.groupLinks.Resize += new System.EventHandler(this.groupLinks_Resize);
			// 
			// gridBugs
			// 
			this.gridBugs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridBugs.HasAddButton = true;
			this.gridBugs.HasMultilineHeaders = false;
			this.gridBugs.HScrollVisible = false;
			this.gridBugs.Location = new System.Drawing.Point(5, 476);
			this.gridBugs.Name = "gridBugs";
			this.gridBugs.ScrollValue = 0;
			this.gridBugs.Size = new System.Drawing.Size(223, 91);
			this.gridBugs.TabIndex = 259;
			this.gridBugs.Title = "Bugs";
			this.gridBugs.TranslationName = "FormTaskEdit";
			this.gridBugs.TitleAddClick += new System.EventHandler(this.gridBugs_TitleAddClick);
			// 
			// gridFeatureReq
			// 
			this.gridFeatureReq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridFeatureReq.HasAddButton = true;
			this.gridFeatureReq.HasMultilineHeaders = false;
			this.gridFeatureReq.HScrollVisible = false;
			this.gridFeatureReq.Location = new System.Drawing.Point(5, 379);
			this.gridFeatureReq.Name = "gridFeatureReq";
			this.gridFeatureReq.ScrollValue = 0;
			this.gridFeatureReq.Size = new System.Drawing.Size(223, 91);
			this.gridFeatureReq.TabIndex = 228;
			this.gridFeatureReq.Title = "Feature Requests";
			this.gridFeatureReq.TranslationName = "FormTaskEdit";
			this.gridFeatureReq.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFeatureReq_CellDoubleClick);
			this.gridFeatureReq.TitleAddClick += new System.EventHandler(this.gridFeatureReq_TitleAddClick);
			// 
			// gridTasks
			// 
			this.gridTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTasks.HasAddButton = true;
			this.gridTasks.HasMultilineHeaders = false;
			this.gridTasks.HScrollVisible = false;
			this.gridTasks.Location = new System.Drawing.Point(5, 282);
			this.gridTasks.Name = "gridTasks";
			this.gridTasks.ScrollValue = 0;
			this.gridTasks.Size = new System.Drawing.Size(223, 91);
			this.gridTasks.TabIndex = 227;
			this.gridTasks.Title = "Tasks";
			this.gridTasks.TranslationName = "FormTaskEdit";
			this.gridTasks.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTasks_CellDoubleClick);
			this.gridTasks.TitleAddClick += new System.EventHandler(this.gridTasks_TitleAddClick);
			// 
			// gridCustomerQuotes
			// 
			this.gridCustomerQuotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCustomerQuotes.HasAddButton = true;
			this.gridCustomerQuotes.HasMultilineHeaders = false;
			this.gridCustomerQuotes.HScrollVisible = false;
			this.gridCustomerQuotes.Location = new System.Drawing.Point(5, 185);
			this.gridCustomerQuotes.Name = "gridCustomerQuotes";
			this.gridCustomerQuotes.ScrollValue = 0;
			this.gridCustomerQuotes.Size = new System.Drawing.Size(223, 91);
			this.gridCustomerQuotes.TabIndex = 226;
			this.gridCustomerQuotes.Title = "Customers and Quotes";
			this.gridCustomerQuotes.TranslationName = "FormTaskEdit";
			this.gridCustomerQuotes.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerQuotes_CellDoubleClick);
			this.gridCustomerQuotes.TitleAddClick += new System.EventHandler(this.gridCustomerQuotes_TitleAddClick);
			// 
			// gridWatchers
			// 
			this.gridWatchers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridWatchers.HasAddButton = true;
			this.gridWatchers.HasMultilineHeaders = false;
			this.gridWatchers.HScrollVisible = false;
			this.gridWatchers.Location = new System.Drawing.Point(6, 88);
			this.gridWatchers.Name = "gridWatchers";
			this.gridWatchers.ScrollValue = 0;
			this.gridWatchers.Size = new System.Drawing.Size(223, 91);
			this.gridWatchers.TabIndex = 225;
			this.gridWatchers.Title = "Watchers";
			this.gridWatchers.TranslationName = "FormTaskEdit";
			this.gridWatchers.TitleAddClick += new System.EventHandler(this.gridWatchers_TitleAddClick);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(3, 61);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 20);
			this.label9.TabIndex = 257;
			this.label9.Text = "Prev. Owner";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 16);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(63, 20);
			this.label14.TabIndex = 193;
			this.label14.Text = "Expert";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textExpert
			// 
			this.textExpert.Location = new System.Drawing.Point(69, 16);
			this.textExpert.MaxLength = 100;
			this.textExpert.Name = "textExpert";
			this.textExpert.ReadOnly = true;
			this.textExpert.Size = new System.Drawing.Size(137, 20);
			this.textExpert.TabIndex = 192;
			this.textExpert.TabStop = false;
			// 
			// butExpertPick
			// 
			this.butExpertPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExpertPick.Autosize = true;
			this.butExpertPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExpertPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExpertPick.CornerRadius = 4F;
			this.butExpertPick.Location = new System.Drawing.Point(206, 16);
			this.butExpertPick.Name = "butExpertPick";
			this.butExpertPick.Size = new System.Drawing.Size(23, 20);
			this.butExpertPick.TabIndex = 216;
			this.butExpertPick.Text = "...";
			this.butExpertPick.Click += new System.EventHandler(this.butExpertPick_Click);
			// 
			// textOwner
			// 
			this.textOwner.Location = new System.Drawing.Point(69, 39);
			this.textOwner.MaxLength = 100;
			this.textOwner.Name = "textOwner";
			this.textOwner.ReadOnly = true;
			this.textOwner.Size = new System.Drawing.Size(137, 20);
			this.textOwner.TabIndex = 206;
			this.textOwner.TabStop = false;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 39);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(63, 20);
			this.label11.TabIndex = 205;
			this.label11.Text = "Owner";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butOwnerPick
			// 
			this.butOwnerPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOwnerPick.Autosize = true;
			this.butOwnerPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOwnerPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOwnerPick.CornerRadius = 4F;
			this.butOwnerPick.Location = new System.Drawing.Point(206, 39);
			this.butOwnerPick.Name = "butOwnerPick";
			this.butOwnerPick.Size = new System.Drawing.Size(23, 20);
			this.butOwnerPick.TabIndex = 215;
			this.butOwnerPick.Text = "...";
			this.butOwnerPick.Click += new System.EventHandler(this.butOwnerPick_Click);
			// 
			// textPrevOwner
			// 
			this.textPrevOwner.Location = new System.Drawing.Point(69, 62);
			this.textPrevOwner.MaxLength = 100;
			this.textPrevOwner.Name = "textPrevOwner";
			this.textPrevOwner.ReadOnly = true;
			this.textPrevOwner.Size = new System.Drawing.Size(137, 20);
			this.textPrevOwner.TabIndex = 258;
			this.textPrevOwner.TabStop = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(291, 51);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(65, 20);
			this.label7.TabIndex = 264;
			this.label7.Text = "Hrs. Est.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(414, 51);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 20);
			this.label8.TabIndex = 265;
			this.label8.Text = "Hrs. Act.";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(653, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(63, 20);
			this.label10.TabIndex = 291;
			this.label10.Text = "Date Entry";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textVersion
			// 
			this.textVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textVersion.Location = new System.Drawing.Point(840, 21);
			this.textVersion.MaxLength = 100;
			this.textVersion.Name = "textVersion";
			this.textVersion.Size = new System.Drawing.Size(170, 20);
			this.textVersion.TabIndex = 294;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(837, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(63, 20);
			this.label6.TabIndex = 292;
			this.label6.Text = "Version";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateEntry.Location = new System.Drawing.Point(656, 21);
			this.textDateEntry.MaxLength = 100;
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(177, 20);
			this.textDateEntry.TabIndex = 293;
			this.textDateEntry.TabStop = false;
			// 
			// comboStatus
			// 
			this.comboStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.Enabled = false;
			this.comboStatus.FormattingEnabled = true;
			this.comboStatus.Location = new System.Drawing.Point(410, 21);
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(117, 21);
			this.comboStatus.TabIndex = 290;
			// 
			// comboCategory
			// 
			this.comboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategory.FormattingEnabled = true;
			this.comboCategory.Location = new System.Drawing.Point(533, 21);
			this.comboCategory.Name = "comboCategory";
			this.comboCategory.Size = new System.Drawing.Size(117, 21);
			this.comboCategory.TabIndex = 287;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 20);
			this.label12.TabIndex = 289;
			this.label12.Text = "Title";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboPriority
			// 
			this.comboPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.FormattingEnabled = true;
			this.comboPriority.Location = new System.Drawing.Point(287, 21);
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(117, 21);
			this.comboPriority.TabIndex = 286;
			// 
			// textTitle
			// 
			this.textTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textTitle.Location = new System.Drawing.Point(3, 21);
			this.textTitle.MaxLength = 255;
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(187, 20);
			this.textTitle.TabIndex = 288;
			this.textTitle.TextChanged += new System.EventHandler(this.textTitle_TextChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(530, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(105, 20);
			this.label4.TabIndex = 282;
			this.label4.Text = "Category";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(407, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 20);
			this.label5.TabIndex = 283;
			this.label5.Text = "Status";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(284, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 20);
			this.label3.TabIndex = 281;
			this.label3.Text = "Priority";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label19
			// 
			this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label19.Location = new System.Drawing.Point(196, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(61, 20);
			this.label19.TabIndex = 284;
			this.label19.Text = "JobNum";
			this.label19.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textJobNum
			// 
			this.textJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textJobNum.Location = new System.Drawing.Point(196, 21);
			this.textJobNum.MaxLength = 100;
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.ReadOnly = true;
			this.textJobNum.Size = new System.Drawing.Size(85, 20);
			this.textJobNum.TabIndex = 285;
			this.textJobNum.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(533, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 20);
			this.label1.TabIndex = 305;
			this.label1.Text = "Parent Job";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textParent
			// 
			this.textParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textParent.Location = new System.Drawing.Point(598, 51);
			this.textParent.MaxLength = 100;
			this.textParent.Name = "textParent";
			this.textParent.ReadOnly = true;
			this.textParent.Size = new System.Drawing.Size(127, 20);
			this.textParent.TabIndex = 304;
			this.textParent.TabStop = false;
			// 
			// butParentPick
			// 
			this.butParentPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butParentPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butParentPick.Autosize = true;
			this.butParentPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butParentPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butParentPick.CornerRadius = 4F;
			this.butParentPick.Location = new System.Drawing.Point(725, 51);
			this.butParentPick.Name = "butParentPick";
			this.butParentPick.Size = new System.Drawing.Size(23, 20);
			this.butParentPick.TabIndex = 307;
			this.butParentPick.Text = "...";
			this.butParentPick.Click += new System.EventHandler(this.butParentPick_Click);
			// 
			// butParentRemove
			// 
			this.butParentRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butParentRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butParentRemove.Autosize = true;
			this.butParentRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butParentRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butParentRemove.CornerRadius = 4F;
			this.butParentRemove.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butParentRemove.Location = new System.Drawing.Point(748, 51);
			this.butParentRemove.Name = "butParentRemove";
			this.butParentRemove.Size = new System.Drawing.Size(23, 20);
			this.butParentRemove.TabIndex = 306;
			this.butParentRemove.Click += new System.EventHandler(this.butParentRemove_Click);
			// 
			// textEstHours
			// 
			this.textEstHours.Location = new System.Drawing.Point(358, 52);
			this.textEstHours.MaxVal = 255;
			this.textEstHours.MinVal = 0;
			this.textEstHours.Name = "textEstHours";
			this.textEstHours.Size = new System.Drawing.Size(46, 20);
			this.textEstHours.TabIndex = 269;
			this.textEstHours.TextChanged += new System.EventHandler(this.textEstHours_TextChanged);
			// 
			// butActions
			// 
			this.butActions.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butActions.Autosize = true;
			this.butActions.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butActions.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butActions.CornerRadius = 4F;
			this.butActions.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butActions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butActions.Location = new System.Drawing.Point(3, 49);
			this.butActions.Name = "butActions";
			this.butActions.Size = new System.Drawing.Size(136, 24);
			this.butActions.TabIndex = 303;
			this.butActions.Text = "Job Actions";
			this.butActions.Click += new System.EventHandler(this.butActions_Click);
			// 
			// textActualHours
			// 
			this.textActualHours.Location = new System.Drawing.Point(481, 52);
			this.textActualHours.MaxVal = 255;
			this.textActualHours.MinVal = 0;
			this.textActualHours.Name = "textActualHours";
			this.textActualHours.Size = new System.Drawing.Size(46, 20);
			this.textActualHours.TabIndex = 270;
			this.textActualHours.TextChanged += new System.EventHandler(this.textActualHours_TextChanged);
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Enabled = false;
			this.butSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSave.Location = new System.Drawing.Point(145, 49);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(136, 24);
			this.butSave.TabIndex = 302;
			this.butSave.Text = "Save Changes";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// UserControlJobEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.butParentPick);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textParent);
			this.Controls.Add(this.butParentRemove);
			this.Controls.Add(this.textEstHours);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.butActions);
			this.Controls.Add(this.textActualHours);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.groupLinks);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.textVersion);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.comboStatus);
			this.Controls.Add(this.comboCategory);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.comboPriority);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.textJobNum);
			this.Name = "UserControlJobEdit";
			this.Size = new System.Drawing.Size(1013, 639);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabReviews.ResumeLayout(false);
			this.tabHistory.ResumeLayout(false);
			this.groupLinks.ResumeLayout(false);
			this.groupLinks.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer2;
		private OdtextEditor textEditorMain;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabMain;
		private UI.ODGrid gridNotes;
		private System.Windows.Forms.TabPage tabReviews;
		private UI.ODGrid gridReview;
		private System.Windows.Forms.TabPage tabHistory;
		private UI.ODGrid gridHistory;
		private System.Windows.Forms.GroupBox groupLinks;
		private UI.ODGrid gridBugs;
		private UI.ODGrid gridFeatureReq;
		private UI.ODGrid gridTasks;
		private UI.ODGrid gridCustomerQuotes;
		private UI.ODGrid gridWatchers;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textExpert;
		private UI.Button butExpertPick;
		private System.Windows.Forms.TextBox textOwner;
		private System.Windows.Forms.Label label11;
		private UI.Button butOwnerPick;
		private System.Windows.Forms.TextBox textPrevOwner;
		private ValidNumber textEstHours;
		private ValidNumber textActualHours;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textVersion;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textDateEntry;
		private System.Windows.Forms.ComboBox comboStatus;
		private System.Windows.Forms.ComboBox comboCategory;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox textJobNum;
		private UI.Button butSave;
		private UI.Button butActions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textParent;
		private UI.Button butParentRemove;
		private UI.Button butParentPick;
	}
}
