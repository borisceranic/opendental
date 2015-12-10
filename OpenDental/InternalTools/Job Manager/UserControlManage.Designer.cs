namespace OpenDental {
	partial class UserControlManage {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlManage));
			this.groupDetails = new System.Windows.Forms.GroupBox();
			this.labelActualHrs = new System.Windows.Forms.Label();
			this.labelEstHrs = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupSummary = new System.Windows.Forms.GroupBox();
			this.labelOwnerJobs = new System.Windows.Forms.Label();
			this.labelOwnerHrs = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxMultiOwner = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiStatus = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiExpert = new OpenDental.UI.ComboBoxMulti();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelExpertJobs = new System.Windows.Forms.Label();
			this.labelExpertHrs = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageMyJobs = new System.Windows.Forms.TabPage();
			this.gridReviews = new OpenDental.UI.ODGrid();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.setSeenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridLinks = new OpenDental.UI.ODGrid();
			this.groupJobDetails = new System.Windows.Forms.GroupBox();
			this.labelMyJobsEstHours = new System.Windows.Forms.Label();
			this.labelMyJobsCreator = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.gridMyJobs = new OpenDental.UI.ODGrid();
			this.tabPageManager = new System.Windows.Forms.TabPage();
			this.butRefresh = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAddJob = new OpenDental.UI.Button();
			this.butAddJobMain = new OpenDental.UI.Button();
			this.groupDetails.SuspendLayout();
			this.groupSummary.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPageMyJobs.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.groupJobDetails.SuspendLayout();
			this.tabPageManager.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupDetails
			// 
			this.groupDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDetails.Controls.Add(this.labelActualHrs);
			this.groupDetails.Controls.Add(this.labelEstHrs);
			this.groupDetails.Controls.Add(this.label8);
			this.groupDetails.Controls.Add(this.label9);
			this.groupDetails.Location = new System.Drawing.Point(759, 79);
			this.groupDetails.Name = "groupDetails";
			this.groupDetails.Size = new System.Drawing.Size(223, 82);
			this.groupDetails.TabIndex = 12;
			this.groupDetails.TabStop = false;
			this.groupDetails.Text = "Job Details";
			// 
			// labelActualHrs
			// 
			this.labelActualHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelActualHrs.Location = new System.Drawing.Point(117, 43);
			this.labelActualHrs.Name = "labelActualHrs";
			this.labelActualHrs.Size = new System.Drawing.Size(100, 23);
			this.labelActualHrs.TabIndex = 9;
			this.labelActualHrs.Text = "0";
			this.labelActualHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelEstHrs
			// 
			this.labelEstHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEstHrs.Location = new System.Drawing.Point(117, 20);
			this.labelEstHrs.Name = "labelEstHrs";
			this.labelEstHrs.Size = new System.Drawing.Size(100, 23);
			this.labelEstHrs.TabIndex = 8;
			this.labelEstHrs.Text = "0";
			this.labelEstHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 43);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 23);
			this.label8.TabIndex = 7;
			this.label8.Text = "Actual Hours:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(68, 23);
			this.label9.TabIndex = 6;
			this.label9.Text = "Est Hours:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupSummary
			// 
			this.groupSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupSummary.Controls.Add(this.labelOwnerJobs);
			this.groupSummary.Controls.Add(this.labelOwnerHrs);
			this.groupSummary.Controls.Add(this.label6);
			this.groupSummary.Controls.Add(this.label7);
			this.groupSummary.Location = new System.Drawing.Point(759, 252);
			this.groupSummary.Name = "groupSummary";
			this.groupSummary.Size = new System.Drawing.Size(223, 82);
			this.groupSummary.TabIndex = 13;
			this.groupSummary.TabStop = false;
			this.groupSummary.Text = "Owner Summary";
			// 
			// labelOwnerJobs
			// 
			this.labelOwnerJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOwnerJobs.Location = new System.Drawing.Point(116, 43);
			this.labelOwnerJobs.Name = "labelOwnerJobs";
			this.labelOwnerJobs.Size = new System.Drawing.Size(100, 23);
			this.labelOwnerJobs.TabIndex = 13;
			this.labelOwnerJobs.Text = "0";
			this.labelOwnerJobs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelOwnerHrs
			// 
			this.labelOwnerHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOwnerHrs.Location = new System.Drawing.Point(116, 20);
			this.labelOwnerHrs.Name = "labelOwnerHrs";
			this.labelOwnerHrs.Size = new System.Drawing.Size(100, 23);
			this.labelOwnerHrs.TabIndex = 12;
			this.labelOwnerHrs.Text = "0";
			this.labelOwnerHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 43);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 23);
			this.label6.TabIndex = 3;
			this.label6.Text = "Incompl Jobs:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 23);
			this.label7.TabIndex = 2;
			this.label7.Text = "Incompl Job Hrs:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.comboBoxMultiOwner);
			this.groupBox2.Controls.Add(this.comboBoxMultiStatus);
			this.groupBox2.Controls.Add(this.comboBoxMultiExpert);
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(672, 73);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filter By";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(214, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 21);
			this.label3.TabIndex = 5;
			this.label3.Text = "Owner";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(414, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 21);
			this.label2.TabIndex = 4;
			this.label2.Text = "Status";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 21);
			this.label1.TabIndex = 3;
			this.label1.Text = "Expert";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBoxMultiOwner
			// 
			this.comboBoxMultiOwner.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiOwner.DroppedDown = false;
			this.comboBoxMultiOwner.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiOwner.Items")));
			this.comboBoxMultiOwner.Location = new System.Drawing.Point(288, 31);
			this.comboBoxMultiOwner.Name = "comboBoxMultiOwner";
			this.comboBoxMultiOwner.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiOwner.SelectedIndices")));
			this.comboBoxMultiOwner.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiOwner.TabIndex = 2;
			this.comboBoxMultiOwner.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiOwner_SelectionChangeCommitted);
			// 
			// comboBoxMultiStatus
			// 
			this.comboBoxMultiStatus.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiStatus.DroppedDown = false;
			this.comboBoxMultiStatus.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiStatus.Items")));
			this.comboBoxMultiStatus.Location = new System.Drawing.Point(488, 31);
			this.comboBoxMultiStatus.Name = "comboBoxMultiStatus";
			this.comboBoxMultiStatus.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiStatus.SelectedIndices")));
			this.comboBoxMultiStatus.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiStatus.TabIndex = 1;
			this.comboBoxMultiStatus.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiStatus_SelectionChangeCommitted);
			// 
			// comboBoxMultiExpert
			// 
			this.comboBoxMultiExpert.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiExpert.DroppedDown = false;
			this.comboBoxMultiExpert.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiExpert.Items")));
			this.comboBoxMultiExpert.Location = new System.Drawing.Point(88, 31);
			this.comboBoxMultiExpert.Name = "comboBoxMultiExpert";
			this.comboBoxMultiExpert.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiExpert.SelectedIndices")));
			this.comboBoxMultiExpert.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiExpert.TabIndex = 0;
			this.comboBoxMultiExpert.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiExpert_SelectionChangeCommitted);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.labelExpertJobs);
			this.groupBox3.Controls.Add(this.labelExpertHrs);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Location = new System.Drawing.Point(759, 167);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(223, 82);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Expert Summary";
			// 
			// labelExpertJobs
			// 
			this.labelExpertJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelExpertJobs.Location = new System.Drawing.Point(117, 43);
			this.labelExpertJobs.Name = "labelExpertJobs";
			this.labelExpertJobs.Size = new System.Drawing.Size(100, 23);
			this.labelExpertJobs.TabIndex = 11;
			this.labelExpertJobs.Text = "0";
			this.labelExpertJobs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelExpertHrs
			// 
			this.labelExpertHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelExpertHrs.Location = new System.Drawing.Point(117, 20);
			this.labelExpertHrs.Name = "labelExpertHrs";
			this.labelExpertHrs.Size = new System.Drawing.Size(100, 23);
			this.labelExpertHrs.TabIndex = 10;
			this.labelExpertHrs.Text = "0";
			this.labelExpertHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 43);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 23);
			this.label5.TabIndex = 1;
			this.label5.Text = "Incompl Jobs:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Incompl Job Hrs:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPageMyJobs);
			this.tabControl1.Controls.Add(this.tabPageManager);
			this.tabControl1.Location = new System.Drawing.Point(3, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(987, 734);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPageMyJobs
			// 
			this.tabPageMyJobs.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageMyJobs.Controls.Add(this.butAddJob);
			this.tabPageMyJobs.Controls.Add(this.gridReviews);
			this.tabPageMyJobs.Controls.Add(this.gridLinks);
			this.tabPageMyJobs.Controls.Add(this.groupJobDetails);
			this.tabPageMyJobs.Controls.Add(this.gridMyJobs);
			this.tabPageMyJobs.Location = new System.Drawing.Point(4, 22);
			this.tabPageMyJobs.Name = "tabPageMyJobs";
			this.tabPageMyJobs.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageMyJobs.Size = new System.Drawing.Size(979, 708);
			this.tabPageMyJobs.TabIndex = 1;
			this.tabPageMyJobs.Text = "My Jobs";
			// 
			// gridReviews
			// 
			this.gridReviews.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gridReviews.ContextMenuStrip = this.contextMenuStrip;
			this.gridReviews.HasMultilineHeaders = false;
			this.gridReviews.HScrollVisible = false;
			this.gridReviews.Location = new System.Drawing.Point(599, 36);
			this.gridReviews.Name = "gridReviews";
			this.gridReviews.ScrollValue = 0;
			this.gridReviews.Size = new System.Drawing.Size(377, 230);
			this.gridReviews.TabIndex = 156;
			this.gridReviews.TabStop = false;
			this.gridReviews.Title = "Reviews To Do";
			this.gridReviews.TranslationName = null;
			this.gridReviews.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridReviews_CellDoubleClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setSeenToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.Size = new System.Drawing.Size(119, 26);
			// 
			// setSeenToolStripMenuItem
			// 
			this.setSeenToolStripMenuItem.Name = "setSeenToolStripMenuItem";
			this.setSeenToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.setSeenToolStripMenuItem.Text = "Set Seen";
			this.setSeenToolStripMenuItem.Click += new System.EventHandler(this.setSeenToolStripMenuItem_Click);
			// 
			// gridLinks
			// 
			this.gridLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gridLinks.HasMultilineHeaders = false;
			this.gridLinks.HScrollVisible = false;
			this.gridLinks.Location = new System.Drawing.Point(599, 272);
			this.gridLinks.Name = "gridLinks";
			this.gridLinks.ScrollValue = 0;
			this.gridLinks.Size = new System.Drawing.Size(377, 199);
			this.gridLinks.TabIndex = 155;
			this.gridLinks.TabStop = false;
			this.gridLinks.Title = "Links";
			this.gridLinks.TranslationName = null;
			this.gridLinks.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridLinks_CellDoubleClick);
			// 
			// groupJobDetails
			// 
			this.groupJobDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupJobDetails.Controls.Add(this.labelMyJobsEstHours);
			this.groupJobDetails.Controls.Add(this.labelMyJobsCreator);
			this.groupJobDetails.Controls.Add(this.label16);
			this.groupJobDetails.Controls.Add(this.label17);
			this.groupJobDetails.Location = new System.Drawing.Point(599, 477);
			this.groupJobDetails.Name = "groupJobDetails";
			this.groupJobDetails.Size = new System.Drawing.Size(378, 81);
			this.groupJobDetails.TabIndex = 15;
			this.groupJobDetails.TabStop = false;
			this.groupJobDetails.Text = "Job Details";
			// 
			// labelMyJobsEstHours
			// 
			this.labelMyJobsEstHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMyJobsEstHours.Location = new System.Drawing.Point(124, 43);
			this.labelMyJobsEstHours.Name = "labelMyJobsEstHours";
			this.labelMyJobsEstHours.Size = new System.Drawing.Size(100, 23);
			this.labelMyJobsEstHours.TabIndex = 9;
			this.labelMyJobsEstHours.Text = "0";
			this.labelMyJobsEstHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelMyJobsCreator
			// 
			this.labelMyJobsCreator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMyJobsCreator.Location = new System.Drawing.Point(124, 20);
			this.labelMyJobsCreator.Name = "labelMyJobsCreator";
			this.labelMyJobsCreator.Size = new System.Drawing.Size(100, 23);
			this.labelMyJobsCreator.TabIndex = 8;
			this.labelMyJobsCreator.Text = "Creator";
			this.labelMyJobsCreator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(15, 43);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(88, 23);
			this.label16.TabIndex = 7;
			this.label16.Text = "Estimated Hours:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(15, 20);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(83, 23);
			this.label17.TabIndex = 6;
			this.label17.Text = "Original Creator:";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridMyJobs
			// 
			this.gridMyJobs.AllowSortingByColumn = true;
			this.gridMyJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMyJobs.HasMultilineHeaders = false;
			this.gridMyJobs.HScrollVisible = false;
			this.gridMyJobs.Location = new System.Drawing.Point(0, 0);
			this.gridMyJobs.Name = "gridMyJobs";
			this.gridMyJobs.ScrollValue = 0;
			this.gridMyJobs.Size = new System.Drawing.Size(596, 708);
			this.gridMyJobs.TabIndex = 12;
			this.gridMyJobs.TabStop = false;
			this.gridMyJobs.Title = "My Jobs";
			this.gridMyJobs.TranslationName = null;
			this.gridMyJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMyJobs_CellDoubleClick);
			this.gridMyJobs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMyJobs_CellClick);
			// 
			// tabPageManager
			// 
			this.tabPageManager.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageManager.Controls.Add(this.butAddJobMain);
			this.tabPageManager.Controls.Add(this.groupBox3);
			this.tabPageManager.Controls.Add(this.butRefresh);
			this.tabPageManager.Controls.Add(this.groupBox2);
			this.tabPageManager.Controls.Add(this.groupDetails);
			this.tabPageManager.Controls.Add(this.groupSummary);
			this.tabPageManager.Controls.Add(this.gridMain);
			this.tabPageManager.Location = new System.Drawing.Point(4, 22);
			this.tabPageManager.Name = "tabPageManager";
			this.tabPageManager.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageManager.Size = new System.Drawing.Size(979, 708);
			this.tabPageManager.TabIndex = 0;
			this.tabPageManager.Text = "Overview";
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(678, 50);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 16;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(0, 79);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(753, 626);
			this.gridMain.TabIndex = 11;
			this.gridMain.TabStop = false;
			this.gridMain.Title = "Manage";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// butAddJob
			// 
			this.butAddJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddJob.Autosize = true;
			this.butAddJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddJob.CornerRadius = 4F;
			this.butAddJob.Location = new System.Drawing.Point(602, 6);
			this.butAddJob.Name = "butAddJob";
			this.butAddJob.Size = new System.Drawing.Size(75, 24);
			this.butAddJob.TabIndex = 157;
			this.butAddJob.Text = "Add Job";
			this.butAddJob.Click += new System.EventHandler(this.butAddJob_Click);
			// 
			// butAddJobMain
			// 
			this.butAddJobMain.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddJobMain.Autosize = true;
			this.butAddJobMain.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddJobMain.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddJobMain.CornerRadius = 4F;
			this.butAddJobMain.Location = new System.Drawing.Point(678, 20);
			this.butAddJobMain.Name = "butAddJobMain";
			this.butAddJobMain.Size = new System.Drawing.Size(75, 24);
			this.butAddJobMain.TabIndex = 158;
			this.butAddJobMain.Text = "Add Job";
			this.butAddJobMain.Click += new System.EventHandler(this.butAddJob_Click);
			// 
			// UserControlManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.MinimumSize = new System.Drawing.Size(450, 290);
			this.Name = "UserControlManage";
			this.Size = new System.Drawing.Size(990, 734);
			this.Load += new System.EventHandler(this.UserControlManage_Load);
			this.groupDetails.ResumeLayout(false);
			this.groupSummary.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPageMyJobs.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			this.groupJobDetails.ResumeLayout(false);
			this.tabPageManager.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridMain;
		private System.Windows.Forms.GroupBox groupDetails;
		private System.Windows.Forms.GroupBox groupSummary;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private UI.ComboBoxMulti comboBoxMultiOwner;
		private UI.ComboBoxMulti comboBoxMultiStatus;
		private UI.ComboBoxMulti comboBoxMultiExpert;
		private UI.Button butRefresh;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelActualHrs;
		private System.Windows.Forms.Label labelEstHrs;
		private System.Windows.Forms.Label labelOwnerJobs;
		private System.Windows.Forms.Label labelOwnerHrs;
		private System.Windows.Forms.Label labelExpertJobs;
		private System.Windows.Forms.Label labelExpertHrs;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageManager;
		private System.Windows.Forms.TabPage tabPageMyJobs;
		private UI.ODGrid gridMyJobs;
		private System.Windows.Forms.GroupBox groupJobDetails;
		private System.Windows.Forms.Label labelMyJobsEstHours;
		private System.Windows.Forms.Label labelMyJobsCreator;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private UI.ODGrid gridLinks;
		private UI.ODGrid gridReviews;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem setSeenToolStripMenuItem;
		private UI.Button butAddJob;
		private UI.Button butAddJobMain;


	}
}
