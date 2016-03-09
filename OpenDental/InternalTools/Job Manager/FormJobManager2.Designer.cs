namespace OpenDental {
	partial class FormJobManager2 {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobManager2));
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboUser = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControlNav = new System.Windows.Forms.TabControl();
			this.tabAction = new System.Windows.Forms.TabPage();
			this.checkShowUnassinged = new System.Windows.Forms.CheckBox();
			this.gridAction = new OpenDental.UI.ODGrid();
			this.tabTree = new System.Windows.Forms.TabPage();
			this.checkResults = new System.Windows.Forms.CheckBox();
			this.treeJobs = new System.Windows.Forms.TreeView();
			this.checkCollapse = new System.Windows.Forms.CheckBox();
			this.comboCategorySearch = new System.Windows.Forms.ComboBox();
			this.labelCategory = new System.Windows.Forms.Label();
			this.labelGroupBy = new System.Windows.Forms.Label();
			this.checkComplete = new System.Windows.Forms.CheckBox();
			this.comboGroup = new System.Windows.Forms.ComboBox();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabJobDetails = new System.Windows.Forms.TabPage();
			this.userControlJobEdit = new OpenDental.InternalTools.Job_Manager.UserControlJobEdit();
			this.tabManage = new System.Windows.Forms.TabPage();
			this.gridWorkSummary = new OpenDental.UI.ODGrid();
			this.tabMyJobs = new System.Windows.Forms.TabPage();
			this.checkShowVersion = new System.Windows.Forms.CheckBox();
			this.checkShowComplete = new System.Windows.Forms.CheckBox();
			this.gridMyJobs = new OpenDental.UI.ODGrid();
			this.butSearch = new OpenDental.UI.Button();
			this.butMe = new OpenDental.UI.Button();
			this.butAddJob = new OpenDental.UI.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControlNav.SuspendLayout();
			this.tabAction.SuspendLayout();
			this.tabTree.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabJobDetails.SuspendLayout();
			this.tabManage.SuspendLayout();
			this.tabMyJobs.SuspendLayout();
			this.SuspendLayout();
			// 
			// textSearch
			// 
			this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSearch.Location = new System.Drawing.Point(384, 13);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(181, 20);
			this.textSearch.TabIndex = 240;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearchAction_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(328, 12);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 20);
			this.label5.TabIndex = 241;
			this.label5.Text = "Search";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboUser
			// 
			this.comboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUser.FormattingEnabled = true;
			this.comboUser.Location = new System.Drawing.Point(141, 13);
			this.comboUser.Name = "comboUser";
			this.comboUser.Size = new System.Drawing.Size(153, 21);
			this.comboUser.TabIndex = 236;
			this.comboUser.SelectionChangeCommitted += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(85, 16);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 15);
			this.label4.TabIndex = 237;
			this.label4.Text = "User";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(-1, 41);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabControlNav);
			this.splitContainer1.Panel1MinSize = 250;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControlMain);
			this.splitContainer1.Panel2MinSize = 250;
			this.splitContainer1.Size = new System.Drawing.Size(1284, 672);
			this.splitContainer1.SplitterDistance = 254;
			this.splitContainer1.TabIndex = 6;
			// 
			// tabControlNav
			// 
			this.tabControlNav.Controls.Add(this.tabAction);
			this.tabControlNav.Controls.Add(this.tabTree);
			this.tabControlNav.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlNav.Location = new System.Drawing.Point(0, 0);
			this.tabControlNav.Name = "tabControlNav";
			this.tabControlNav.SelectedIndex = 0;
			this.tabControlNav.Size = new System.Drawing.Size(254, 672);
			this.tabControlNav.TabIndex = 1;
			// 
			// tabAction
			// 
			this.tabAction.Controls.Add(this.checkShowUnassinged);
			this.tabAction.Controls.Add(this.gridAction);
			this.tabAction.Location = new System.Drawing.Point(4, 22);
			this.tabAction.Name = "tabAction";
			this.tabAction.Padding = new System.Windows.Forms.Padding(3);
			this.tabAction.Size = new System.Drawing.Size(246, 646);
			this.tabAction.TabIndex = 0;
			this.tabAction.Text = "Needs Action";
			this.tabAction.UseVisualStyleBackColor = true;
			// 
			// checkShowUnassinged
			// 
			this.checkShowUnassinged.Location = new System.Drawing.Point(5, 5);
			this.checkShowUnassinged.Name = "checkShowUnassinged";
			this.checkShowUnassinged.Size = new System.Drawing.Size(184, 20);
			this.checkShowUnassinged.TabIndex = 238;
			this.checkShowUnassinged.Text = "Show OnHold/Unassigned";
			this.checkShowUnassinged.UseVisualStyleBackColor = true;
			this.checkShowUnassinged.CheckedChanged += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// gridAction
			// 
			this.gridAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAction.HasAddButton = false;
			this.gridAction.HasMultilineHeaders = true;
			this.gridAction.HScrollVisible = false;
			this.gridAction.Location = new System.Drawing.Point(3, 31);
			this.gridAction.Name = "gridAction";
			this.gridAction.ScrollValue = 0;
			this.gridAction.Size = new System.Drawing.Size(240, 612);
			this.gridAction.TabIndex = 227;
			this.gridAction.Title = "Action Items";
			this.gridAction.TranslationName = "FormTaskEdit";
			this.gridAction.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAction_CellDoubleClick);
			this.gridAction.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAction_CellClick);
			// 
			// tabTree
			// 
			this.tabTree.Controls.Add(this.checkResults);
			this.tabTree.Controls.Add(this.treeJobs);
			this.tabTree.Controls.Add(this.checkCollapse);
			this.tabTree.Controls.Add(this.comboCategorySearch);
			this.tabTree.Controls.Add(this.labelCategory);
			this.tabTree.Controls.Add(this.labelGroupBy);
			this.tabTree.Controls.Add(this.checkComplete);
			this.tabTree.Controls.Add(this.comboGroup);
			this.tabTree.Location = new System.Drawing.Point(4, 22);
			this.tabTree.Name = "tabTree";
			this.tabTree.Padding = new System.Windows.Forms.Padding(3);
			this.tabTree.Size = new System.Drawing.Size(246, 646);
			this.tabTree.TabIndex = 1;
			this.tabTree.Text = "Tree View";
			this.tabTree.UseVisualStyleBackColor = true;
			// 
			// checkResults
			// 
			this.checkResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkResults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResults.Location = new System.Drawing.Point(115, 97);
			this.checkResults.Name = "checkResults";
			this.checkResults.Size = new System.Drawing.Size(125, 20);
			this.checkResults.TabIndex = 235;
			this.checkResults.Text = "Search Results";
			this.checkResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkResults.UseVisualStyleBackColor = true;
			this.checkResults.Visible = false;
			this.checkResults.CheckedChanged += new System.EventHandler(this.checkResults_CheckedChanged);
			// 
			// treeJobs
			// 
			this.treeJobs.AllowDrop = true;
			this.treeJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeJobs.HideSelection = false;
			this.treeJobs.Indent = 9;
			this.treeJobs.Location = new System.Drawing.Point(3, 118);
			this.treeJobs.Name = "treeJobs";
			this.treeJobs.Size = new System.Drawing.Size(240, 525);
			this.treeJobs.TabIndex = 220;
			this.treeJobs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeJobs_ItemDrag);
			this.treeJobs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeJobs_NodeMouseClick);
			this.treeJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragDrop);
			this.treeJobs.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragEnter);
			this.treeJobs.DragOver += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragOver);
			// 
			// checkCollapse
			// 
			this.checkCollapse.Location = new System.Drawing.Point(6, 97);
			this.checkCollapse.Name = "checkCollapse";
			this.checkCollapse.Size = new System.Drawing.Size(103, 20);
			this.checkCollapse.TabIndex = 226;
			this.checkCollapse.Text = "Collapse All";
			this.checkCollapse.UseVisualStyleBackColor = true;
			this.checkCollapse.CheckedChanged += new System.EventHandler(this.checkCollapse_CheckedChanged);
			// 
			// comboCategorySearch
			// 
			this.comboCategorySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCategorySearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategorySearch.FormattingEnabled = true;
			this.comboCategorySearch.Location = new System.Drawing.Point(68, 6);
			this.comboCategorySearch.Name = "comboCategorySearch";
			this.comboCategorySearch.Size = new System.Drawing.Size(172, 21);
			this.comboCategorySearch.TabIndex = 234;
			this.comboCategorySearch.SelectedIndexChanged += new System.EventHandler(this.comboCategorySearch_SelectedIndexChanged);
			// 
			// labelCategory
			// 
			this.labelCategory.Location = new System.Drawing.Point(12, 7);
			this.labelCategory.Margin = new System.Windows.Forms.Padding(0);
			this.labelCategory.Name = "labelCategory";
			this.labelCategory.Size = new System.Drawing.Size(55, 20);
			this.labelCategory.TabIndex = 233;
			this.labelCategory.Text = "Category";
			this.labelCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelGroupBy
			// 
			this.labelGroupBy.Location = new System.Drawing.Point(12, 33);
			this.labelGroupBy.Margin = new System.Windows.Forms.Padding(0);
			this.labelGroupBy.Name = "labelGroupBy";
			this.labelGroupBy.Size = new System.Drawing.Size(55, 15);
			this.labelGroupBy.TabIndex = 222;
			this.labelGroupBy.Text = "Group By";
			this.labelGroupBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkComplete
			// 
			this.checkComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkComplete.Location = new System.Drawing.Point(105, 57);
			this.checkComplete.Name = "checkComplete";
			this.checkComplete.Size = new System.Drawing.Size(135, 20);
			this.checkComplete.TabIndex = 230;
			this.checkComplete.Text = "Include Complete";
			this.checkComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkComplete.UseVisualStyleBackColor = true;
			this.checkComplete.CheckedChanged += new System.EventHandler(this.checkComplete_CheckedChanged);
			// 
			// comboGroup
			// 
			this.comboGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroup.FormattingEnabled = true;
			this.comboGroup.Location = new System.Drawing.Point(68, 30);
			this.comboGroup.Name = "comboGroup";
			this.comboGroup.Size = new System.Drawing.Size(172, 21);
			this.comboGroup.TabIndex = 221;
			this.comboGroup.SelectionChangeCommitted += new System.EventHandler(this.comboGroup_SelectionChangeCommitted);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabJobDetails);
			this.tabControlMain.Controls.Add(this.tabManage);
			this.tabControlMain.Controls.Add(this.tabMyJobs);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1026, 672);
			this.tabControlMain.TabIndex = 221;
			// 
			// tabJobDetails
			// 
			this.tabJobDetails.BackColor = System.Drawing.SystemColors.Control;
			this.tabJobDetails.Controls.Add(this.userControlJobEdit);
			this.tabJobDetails.Location = new System.Drawing.Point(4, 22);
			this.tabJobDetails.Name = "tabJobDetails";
			this.tabJobDetails.Padding = new System.Windows.Forms.Padding(3);
			this.tabJobDetails.Size = new System.Drawing.Size(1018, 646);
			this.tabJobDetails.TabIndex = 2;
			this.tabJobDetails.Text = "Job";
			// 
			// userControlJobEdit
			// 
			this.userControlJobEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobEdit.Enabled = false;
			this.userControlJobEdit.IsOverride = false;
			this.userControlJobEdit.Location = new System.Drawing.Point(3, 3);
			this.userControlJobEdit.Name = "userControlJobEdit";
			this.userControlJobEdit.Size = new System.Drawing.Size(1012, 640);
			this.userControlJobEdit.TabIndex = 0;
			this.userControlJobEdit.SaveClick += new System.EventHandler(this.userControlJobEdit_SaveClick);
			this.userControlJobEdit.RequestJob += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.RequestJobEvent(this.userControlJobEdit_RequestJob);
			this.userControlJobEdit.JobOverride += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.JobOverrideEvent(this.userControlJobEdit_JobOverride);
			// 
			// tabManage
			// 
			this.tabManage.BackColor = System.Drawing.SystemColors.Control;
			this.tabManage.Controls.Add(this.gridWorkSummary);
			this.tabManage.Location = new System.Drawing.Point(4, 22);
			this.tabManage.Name = "tabManage";
			this.tabManage.Padding = new System.Windows.Forms.Padding(3);
			this.tabManage.Size = new System.Drawing.Size(1018, 646);
			this.tabManage.TabIndex = 3;
			this.tabManage.Text = "Manage";
			// 
			// gridWorkSummary
			// 
			this.gridWorkSummary.AllowSortingByColumn = true;
			this.gridWorkSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWorkSummary.HasAddButton = false;
			this.gridWorkSummary.HasMultilineHeaders = true;
			this.gridWorkSummary.HScrollVisible = false;
			this.gridWorkSummary.Location = new System.Drawing.Point(3, 3);
			this.gridWorkSummary.Name = "gridWorkSummary";
			this.gridWorkSummary.ScrollValue = 0;
			this.gridWorkSummary.Size = new System.Drawing.Size(590, 640);
			this.gridWorkSummary.TabIndex = 226;
			this.gridWorkSummary.Title = "Workload Summary";
			this.gridWorkSummary.TranslationName = "FormTaskEdit";
			this.gridWorkSummary.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWorkSummary_CellClick);
			// 
			// tabMyJobs
			// 
			this.tabMyJobs.BackColor = System.Drawing.SystemColors.Control;
			this.tabMyJobs.Controls.Add(this.checkShowVersion);
			this.tabMyJobs.Controls.Add(this.checkShowComplete);
			this.tabMyJobs.Controls.Add(this.gridMyJobs);
			this.tabMyJobs.Location = new System.Drawing.Point(4, 22);
			this.tabMyJobs.Name = "tabMyJobs";
			this.tabMyJobs.Padding = new System.Windows.Forms.Padding(3);
			this.tabMyJobs.Size = new System.Drawing.Size(1018, 646);
			this.tabMyJobs.TabIndex = 4;
			this.tabMyJobs.Text = "Jobs By User";
			// 
			// checkShowVersion
			// 
			this.checkShowVersion.Location = new System.Drawing.Point(128, 6);
			this.checkShowVersion.Name = "checkShowVersion";
			this.checkShowVersion.Size = new System.Drawing.Size(184, 20);
			this.checkShowVersion.TabIndex = 245;
			this.checkShowVersion.Text = "Show Version";
			this.checkShowVersion.UseVisualStyleBackColor = true;
			this.checkShowVersion.CheckedChanged += new System.EventHandler(this.checkShowVersion_CheckedChanged);
			// 
			// checkShowComplete
			// 
			this.checkShowComplete.Location = new System.Drawing.Point(6, 6);
			this.checkShowComplete.Name = "checkShowComplete";
			this.checkShowComplete.Size = new System.Drawing.Size(116, 20);
			this.checkShowComplete.TabIndex = 244;
			this.checkShowComplete.Text = "Show Complete";
			this.checkShowComplete.UseVisualStyleBackColor = true;
			this.checkShowComplete.CheckedChanged += new System.EventHandler(this.checkShowComplete_CheckedChanged);
			// 
			// gridMyJobs
			// 
			this.gridMyJobs.AllowSortingByColumn = true;
			this.gridMyJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMyJobs.HasAddButton = false;
			this.gridMyJobs.HasMultilineHeaders = true;
			this.gridMyJobs.HScrollVisible = false;
			this.gridMyJobs.Location = new System.Drawing.Point(3, 31);
			this.gridMyJobs.Name = "gridMyJobs";
			this.gridMyJobs.ScrollValue = 0;
			this.gridMyJobs.Size = new System.Drawing.Size(1012, 612);
			this.gridMyJobs.TabIndex = 226;
			this.gridMyJobs.Title = "My Jobs";
			this.gridMyJobs.TranslationName = "FormTaskEdit";
			this.gridMyJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMyJobs_CellDoubleClick);
			this.gridMyJobs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMyJobs_CellClick);
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(571, 11);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(80, 24);
			this.butSearch.TabIndex = 231;
			this.butSearch.Text = "Power Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// butMe
			// 
			this.butMe.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMe.Autosize = true;
			this.butMe.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMe.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMe.CornerRadius = 4F;
			this.butMe.Location = new System.Drawing.Point(294, 13);
			this.butMe.Name = "butMe";
			this.butMe.Size = new System.Drawing.Size(31, 21);
			this.butMe.TabIndex = 239;
			this.butMe.Text = "Me";
			this.butMe.Click += new System.EventHandler(this.butMe_Click);
			// 
			// butAddJob
			// 
			this.butAddJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddJob.Autosize = true;
			this.butAddJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddJob.CornerRadius = 4F;
			this.butAddJob.Location = new System.Drawing.Point(7, 11);
			this.butAddJob.Name = "butAddJob";
			this.butAddJob.Size = new System.Drawing.Size(75, 24);
			this.butAddJob.TabIndex = 227;
			this.butAddJob.Text = "Add Job";
			this.butAddJob.Click += new System.EventHandler(this.butAddJob_Click);
			// 
			// FormJobManager2
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1281, 713);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.butMe);
			this.Controls.Add(this.butAddJob);
			this.Controls.Add(this.comboUser);
			this.Controls.Add(this.label4);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobManager2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Job Manager 2";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobManager2_FormClosing);
			this.Load += new System.EventHandler(this.FormJobManager_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControlNav.ResumeLayout(false);
			this.tabAction.ResumeLayout(false);
			this.tabTree.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabJobDetails.ResumeLayout(false);
			this.tabManage.ResumeLayout(false);
			this.tabMyJobs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabJobDetails;
		private UI.Button butAddJob;
		private System.Windows.Forms.CheckBox checkCollapse;
		private System.Windows.Forms.Label labelGroupBy;
		private System.Windows.Forms.TreeView treeJobs;
		private System.Windows.Forms.ComboBox comboGroup;
		private InternalTools.Job_Manager.UserControlJobEdit userControlJobEdit;
		private System.Windows.Forms.TabPage tabManage;
		private UI.ODGrid gridWorkSummary;
		private System.Windows.Forms.CheckBox checkComplete;
		private UI.Button butSearch;
		private System.Windows.Forms.ComboBox comboCategorySearch;
		private System.Windows.Forms.Label labelCategory;
		private UI.ODGrid gridAction;
		private System.Windows.Forms.TabControl tabControlNav;
		private System.Windows.Forms.TabPage tabAction;
		private System.Windows.Forms.ComboBox comboUser;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabPage tabTree;
		private System.Windows.Forms.CheckBox checkShowUnassinged;
		private UI.Button butMe;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TabPage tabMyJobs;
		private UI.ODGrid gridMyJobs;
		private System.Windows.Forms.CheckBox checkShowComplete;
		private System.Windows.Forms.CheckBox checkResults;
		private System.Windows.Forms.CheckBox checkShowVersion;
	}
}