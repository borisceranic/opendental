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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.comboCategorySearch = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butSearch = new OpenDental.UI.Button();
			this.checkComplete = new System.Windows.Forms.CheckBox();
			this.checkMine = new System.Windows.Forms.CheckBox();
			this.butOverride = new OpenDental.UI.Button();
			this.butAddJob = new OpenDental.UI.Button();
			this.comboGroup = new System.Windows.Forms.ComboBox();
			this.treeJobs = new System.Windows.Forms.TreeView();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkHighlight = new System.Windows.Forms.CheckBox();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.checkCollapse = new System.Windows.Forms.CheckBox();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabJobDetails = new System.Windows.Forms.TabPage();
			this.userControlJobEdit = new OpenDental.InternalTools.Job_Manager.UserControlJobEdit();
			this.tabManage = new System.Windows.Forms.TabPage();
			this.gridWorkSummary = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabJobDetails.SuspendLayout();
			this.tabManage.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.butRefresh);
			this.splitContainer1.Panel1.Controls.Add(this.comboCategorySearch);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.butSearch);
			this.splitContainer1.Panel1.Controls.Add(this.checkComplete);
			this.splitContainer1.Panel1.Controls.Add(this.checkMine);
			this.splitContainer1.Panel1.Controls.Add(this.butOverride);
			this.splitContainer1.Panel1.Controls.Add(this.butAddJob);
			this.splitContainer1.Panel1.Controls.Add(this.comboGroup);
			this.splitContainer1.Panel1.Controls.Add(this.treeJobs);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.checkHighlight);
			this.splitContainer1.Panel1.Controls.Add(this.textSearch);
			this.splitContainer1.Panel1.Controls.Add(this.checkCollapse);
			this.splitContainer1.Panel1MinSize = 250;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControlMain);
			this.splitContainer1.Panel2MinSize = 900;
			this.splitContainer1.Size = new System.Drawing.Size(1281, 713);
			this.splitContainer1.SplitterDistance = 257;
			this.splitContainer1.TabIndex = 6;
			// 
			// comboCategorySearch
			// 
			this.comboCategorySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCategorySearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategorySearch.FormattingEnabled = true;
			this.comboCategorySearch.Location = new System.Drawing.Point(69, 7);
			this.comboCategorySearch.Name = "comboCategorySearch";
			this.comboCategorySearch.Size = new System.Drawing.Size(183, 21);
			this.comboCategorySearch.TabIndex = 234;
			this.comboCategorySearch.SelectedIndexChanged += new System.EventHandler(this.comboCategorySearch_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 8);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 20);
			this.label3.TabIndex = 233;
			this.label3.Text = "Category";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(69, 75);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(80, 24);
			this.butSearch.TabIndex = 231;
			this.butSearch.Text = "Power Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// checkComplete
			// 
			this.checkComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkComplete.Location = new System.Drawing.Point(163, 99);
			this.checkComplete.Name = "checkComplete";
			this.checkComplete.Size = new System.Drawing.Size(89, 20);
			this.checkComplete.TabIndex = 230;
			this.checkComplete.Text = "Complete";
			this.checkComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkComplete.UseVisualStyleBackColor = true;
			this.checkComplete.CheckedChanged += new System.EventHandler(this.checkComplete_CheckedChanged);
			// 
			// checkMine
			// 
			this.checkMine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkMine.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMine.Checked = true;
			this.checkMine.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkMine.Location = new System.Drawing.Point(163, 77);
			this.checkMine.Name = "checkMine";
			this.checkMine.Size = new System.Drawing.Size(89, 20);
			this.checkMine.TabIndex = 229;
			this.checkMine.Text = "Mine Only";
			this.checkMine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMine.UseVisualStyleBackColor = true;
			this.checkMine.Click += new System.EventHandler(this.checkMine_Click);
			// 
			// butOverride
			// 
			this.butOverride.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butOverride.Autosize = true;
			this.butOverride.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOverride.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOverride.CornerRadius = 4F;
			this.butOverride.Location = new System.Drawing.Point(5, 683);
			this.butOverride.Name = "butOverride";
			this.butOverride.Size = new System.Drawing.Size(75, 24);
			this.butOverride.TabIndex = 228;
			this.butOverride.Text = "Override";
			this.butOverride.Click += new System.EventHandler(this.butOverride_Click);
			// 
			// butAddJob
			// 
			this.butAddJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddJob.Autosize = true;
			this.butAddJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddJob.CornerRadius = 4F;
			this.butAddJob.Location = new System.Drawing.Point(177, 683);
			this.butAddJob.Name = "butAddJob";
			this.butAddJob.Size = new System.Drawing.Size(75, 24);
			this.butAddJob.TabIndex = 227;
			this.butAddJob.Text = "Add Job";
			this.butAddJob.Click += new System.EventHandler(this.butAddJob_Click);
			// 
			// comboGroup
			// 
			this.comboGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroup.FormattingEnabled = true;
			this.comboGroup.Location = new System.Drawing.Point(69, 31);
			this.comboGroup.Name = "comboGroup";
			this.comboGroup.Size = new System.Drawing.Size(183, 21);
			this.comboGroup.TabIndex = 221;
			this.comboGroup.SelectionChangeCommitted += new System.EventHandler(this.comboGroup_SelectionChangeCommitted);
			// 
			// treeJobs
			// 
			this.treeJobs.AllowDrop = true;
			this.treeJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeJobs.Indent = 9;
			this.treeJobs.Location = new System.Drawing.Point(5, 125);
			this.treeJobs.Name = "treeJobs";
			this.treeJobs.Size = new System.Drawing.Size(248, 554);
			this.treeJobs.TabIndex = 220;
			this.treeJobs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeJobs_ItemDrag);
			this.treeJobs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeJobs_NodeMouseClick);
			this.treeJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragDrop);
			this.treeJobs.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragEnter);
			this.treeJobs.DragOver += new System.Windows.Forms.DragEventHandler(this.treeJobs_DragOver);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 54);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 20);
			this.label2.TabIndex = 224;
			this.label2.Text = "Search";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 34);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 15);
			this.label1.TabIndex = 222;
			this.label1.Text = "Group By";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkHighlight
			// 
			this.checkHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHighlight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHighlight.Location = new System.Drawing.Point(235, 55);
			this.checkHighlight.Name = "checkHighlight";
			this.checkHighlight.Size = new System.Drawing.Size(17, 20);
			this.checkHighlight.TabIndex = 225;
			this.checkHighlight.UseVisualStyleBackColor = true;
			this.checkHighlight.CheckedChanged += new System.EventHandler(this.checkHighlight_CheckedChanged);
			// 
			// textSearch
			// 
			this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSearch.Location = new System.Drawing.Point(69, 55);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(160, 20);
			this.textSearch.TabIndex = 223;
			this.textSearch.TextChanged += new System.EventHandler(this.textFilter_TextChanged);
			// 
			// checkCollapse
			// 
			this.checkCollapse.Location = new System.Drawing.Point(6, 107);
			this.checkCollapse.Name = "checkCollapse";
			this.checkCollapse.Size = new System.Drawing.Size(89, 20);
			this.checkCollapse.TabIndex = 226;
			this.checkCollapse.Text = "Collapse All";
			this.checkCollapse.UseVisualStyleBackColor = true;
			this.checkCollapse.CheckedChanged += new System.EventHandler(this.checkCollapse_CheckedChanged);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabJobDetails);
			this.tabControlMain.Controls.Add(this.tabManage);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1020, 713);
			this.tabControlMain.TabIndex = 221;
			// 
			// tabJobDetails
			// 
			this.tabJobDetails.BackColor = System.Drawing.SystemColors.Control;
			this.tabJobDetails.Controls.Add(this.userControlJobEdit);
			this.tabJobDetails.Location = new System.Drawing.Point(4, 22);
			this.tabJobDetails.Name = "tabJobDetails";
			this.tabJobDetails.Padding = new System.Windows.Forms.Padding(3);
			this.tabJobDetails.Size = new System.Drawing.Size(1012, 687);
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
			this.userControlJobEdit.Size = new System.Drawing.Size(1006, 681);
			this.userControlJobEdit.TabIndex = 0;
			this.userControlJobEdit.SaveClick += new System.EventHandler(this.userControlJobEdit_SaveClick);
			this.userControlJobEdit.JobOverride += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.JobOverrideEvent(this.userControlJobEdit_JobOverride);
			// 
			// tabManage
			// 
			this.tabManage.BackColor = System.Drawing.SystemColors.Control;
			this.tabManage.Controls.Add(this.gridWorkSummary);
			this.tabManage.Location = new System.Drawing.Point(4, 22);
			this.tabManage.Name = "tabManage";
			this.tabManage.Padding = new System.Windows.Forms.Padding(3);
			this.tabManage.Size = new System.Drawing.Size(1012, 687);
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
			this.gridWorkSummary.Location = new System.Drawing.Point(6, 6);
			this.gridWorkSummary.Name = "gridWorkSummary";
			this.gridWorkSummary.ScrollValue = 0;
			this.gridWorkSummary.Size = new System.Drawing.Size(400, 675);
			this.gridWorkSummary.TabIndex = 226;
			this.gridWorkSummary.Title = "Workload Summary";
			this.gridWorkSummary.TranslationName = "FormTaskEdit";
			this.gridWorkSummary.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWorkSummary_CellClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(11, 75);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(52, 24);
			this.butRefresh.TabIndex = 235;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// FormJobManager2
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1281, 713);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1297, 752);
			this.Name = "FormJobManager2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Job Manager 2";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobManager2_FormClosing);
			this.Load += new System.EventHandler(this.FormJobManager_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabJobDetails.ResumeLayout(false);
			this.tabManage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabJobDetails;
		private UI.Button butAddJob;
		private System.Windows.Forms.CheckBox checkCollapse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkHighlight;
		private System.Windows.Forms.TreeView treeJobs;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboGroup;
		private System.Windows.Forms.TextBox textSearch;
		private InternalTools.Job_Manager.UserControlJobEdit userControlJobEdit;
		private UI.Button butOverride;
		private System.Windows.Forms.TabPage tabManage;
		private UI.ODGrid gridWorkSummary;
		private System.Windows.Forms.CheckBox checkMine;
		private System.Windows.Forms.CheckBox checkComplete;
		private UI.Button butSearch;
		private System.Windows.Forms.ComboBox comboCategorySearch;
		private System.Windows.Forms.Label label3;
		private UI.Button butRefresh;


	}
}