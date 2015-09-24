namespace OpenDental{
	partial class FormJobEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobEdit));
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textExpert = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textProject = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textVersion = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textEstHours = new System.Windows.Forms.TextBox();
			this.textActualHours = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textDateEntry = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textOwner = new System.Windows.Forms.TextBox();
			this.groupAddLink = new System.Windows.Forms.GroupBox();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.textNotes = new OpenDental.ODtextBox();
			this.textDescription = new OpenDental.ODtextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.butPickOwner = new OpenDental.UI.Button();
			this.butHistory = new OpenDental.UI.Button();
			this.butLinkBug = new OpenDental.UI.Button();
			this.butLinkFeatReq = new OpenDental.UI.Button();
			this.butLinkTask = new OpenDental.UI.Button();
			this.butPickProject = new OpenDental.UI.Button();
			this.butPickExpert = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupAddLink.SuspendLayout();
			this.SuspendLayout();
			// 
			// textJobNum
			// 
			this.textJobNum.Location = new System.Drawing.Point(95, 12);
			this.textJobNum.MaxLength = 100;
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.ReadOnly = true;
			this.textJobNum.Size = new System.Drawing.Size(183, 20);
			this.textJobNum.TabIndex = 0;
			this.textJobNum.TabStop = false;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(11, 12);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(83, 20);
			this.label19.TabIndex = 0;
			this.label19.Text = "JobNum";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Expert";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textExpert
			// 
			this.textExpert.Location = new System.Drawing.Point(95, 56);
			this.textExpert.MaxLength = 100;
			this.textExpert.Name = "textExpert";
			this.textExpert.ReadOnly = true;
			this.textExpert.Size = new System.Drawing.Size(159, 20);
			this.textExpert.TabIndex = 0;
			this.textExpert.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(289, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Project";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProject
			// 
			this.textProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textProject.Location = new System.Drawing.Point(373, 12);
			this.textProject.MaxLength = 100;
			this.textProject.Name = "textProject";
			this.textProject.ReadOnly = true;
			this.textProject.Size = new System.Drawing.Size(373, 20);
			this.textProject.TabIndex = 0;
			this.textProject.TabStop = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Priority";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 146);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(83, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 123);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 20);
			this.label5.TabIndex = 0;
			this.label5.Text = "Status";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 169);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 20);
			this.label6.TabIndex = 0;
			this.label6.Text = "Version";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVersion
			// 
			this.textVersion.Location = new System.Drawing.Point(95, 169);
			this.textVersion.MaxLength = 100;
			this.textVersion.Name = "textVersion";
			this.textVersion.Size = new System.Drawing.Size(183, 20);
			this.textVersion.TabIndex = 6;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(11, 191);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(83, 20);
			this.label7.TabIndex = 0;
			this.label7.Text = "Est Hours";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEstHours
			// 
			this.textEstHours.Location = new System.Drawing.Point(95, 191);
			this.textEstHours.MaxLength = 100;
			this.textEstHours.Name = "textEstHours";
			this.textEstHours.ReadOnly = true;
			this.textEstHours.Size = new System.Drawing.Size(183, 20);
			this.textEstHours.TabIndex = 7;
			// 
			// textActualHours
			// 
			this.textActualHours.Location = new System.Drawing.Point(95, 213);
			this.textActualHours.MaxLength = 100;
			this.textActualHours.Name = "textActualHours";
			this.textActualHours.Size = new System.Drawing.Size(183, 20);
			this.textActualHours.TabIndex = 8;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(12, 213);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 20);
			this.label8.TabIndex = 0;
			this.label8.Text = "Actual Hours";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(289, 57);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(83, 20);
			this.label9.TabIndex = 0;
			this.label9.Text = "Description";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(11, 34);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(83, 20);
			this.label10.TabIndex = 0;
			this.label10.Text = "Date Entry";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(95, 34);
			this.textDateEntry.MaxLength = 100;
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(183, 20);
			this.textDateEntry.TabIndex = 0;
			this.textDateEntry.TabStop = false;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(11, 78);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(83, 20);
			this.label11.TabIndex = 0;
			this.label11.Text = "Owner";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOwner
			// 
			this.textOwner.Location = new System.Drawing.Point(95, 78);
			this.textOwner.MaxLength = 100;
			this.textOwner.Name = "textOwner";
			this.textOwner.ReadOnly = true;
			this.textOwner.Size = new System.Drawing.Size(159, 20);
			this.textOwner.TabIndex = 0;
			this.textOwner.TabStop = false;
			// 
			// groupAddLink
			// 
			this.groupAddLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupAddLink.Controls.Add(this.butLinkBug);
			this.groupAddLink.Controls.Add(this.butLinkFeatReq);
			this.groupAddLink.Controls.Add(this.butLinkTask);
			this.groupAddLink.Location = new System.Drawing.Point(95, 341);
			this.groupAddLink.Name = "groupAddLink";
			this.groupAddLink.Size = new System.Drawing.Size(224, 46);
			this.groupAddLink.TabIndex = 13;
			this.groupAddLink.TabStop = false;
			this.groupAddLink.Text = "Add Link";
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.FormattingEnabled = true;
			this.comboPriority.Location = new System.Drawing.Point(95, 100);
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(183, 21);
			this.comboPriority.TabIndex = 3;
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.FormattingEnabled = true;
			this.comboStatus.Location = new System.Drawing.Point(95, 123);
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(183, 21);
			this.comboStatus.TabIndex = 4;
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.FormattingEnabled = true;
			this.comboType.Location = new System.Drawing.Point(95, 146);
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(183, 21);
			this.comboType.TabIndex = 5;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(289, 34);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(83, 20);
			this.label12.TabIndex = 15;
			this.label12.Text = "Title";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTitle
			// 
			this.textTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textTitle.Location = new System.Drawing.Point(373, 34);
			this.textTitle.MaxLength = 100;
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(397, 20);
			this.textTitle.TabIndex = 10;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(11, 236);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(83, 20);
			this.label13.TabIndex = 0;
			this.label13.Text = "Notes";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNotes
			// 
			this.textNotes.AcceptsTab = true;
			this.textNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.textNotes.DetectUrls = false;
			this.textNotes.Location = new System.Drawing.Point(95, 235);
			this.textNotes.Name = "textNotes";
			this.textNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.CommLog;
			this.textNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNotes.Size = new System.Drawing.Size(272, 104);
			this.textNotes.TabIndex = 12;
			this.textNotes.Text = "";
			// 
			// textDescription
			// 
			this.textDescription.AcceptsTab = true;
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.DetectUrls = false;
			this.textDescription.Location = new System.Drawing.Point(373, 56);
			this.textDescription.Name = "textDescription";
			this.textDescription.QuickPasteType = OpenDentBusiness.QuickPasteType.CommLog;
			this.textDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(397, 283);
			this.textDescription.TabIndex = 11;
			this.textDescription.Text = "";
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
			this.butDelete.Location = new System.Drawing.Point(12, 360);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 17;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butPickOwner
			// 
			this.butPickOwner.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickOwner.Autosize = false;
			this.butPickOwner.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickOwner.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickOwner.CornerRadius = 2F;
			this.butPickOwner.Location = new System.Drawing.Point(255, 77);
			this.butPickOwner.Name = "butPickOwner";
			this.butPickOwner.Size = new System.Drawing.Size(23, 21);
			this.butPickOwner.TabIndex = 2;
			this.butPickOwner.Text = "...";
			this.butPickOwner.Click += new System.EventHandler(this.butPickOwner_Click);
			// 
			// butHistory
			// 
			this.butHistory.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHistory.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butHistory.Autosize = true;
			this.butHistory.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHistory.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHistory.CornerRadius = 4F;
			this.butHistory.Location = new System.Drawing.Point(373, 358);
			this.butHistory.Name = "butHistory";
			this.butHistory.Size = new System.Drawing.Size(75, 24);
			this.butHistory.TabIndex = 14;
			this.butHistory.Text = "History";
			this.butHistory.Click += new System.EventHandler(this.butHistory_Click);
			// 
			// butLinkBug
			// 
			this.butLinkBug.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLinkBug.Autosize = true;
			this.butLinkBug.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLinkBug.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLinkBug.CornerRadius = 4F;
			this.butLinkBug.Location = new System.Drawing.Point(150, 18);
			this.butLinkBug.Name = "butLinkBug";
			this.butLinkBug.Size = new System.Drawing.Size(65, 22);
			this.butLinkBug.TabIndex = 2;
			this.butLinkBug.Text = "Bug";
			this.butLinkBug.Click += new System.EventHandler(this.butLinkBug_Click);
			// 
			// butLinkFeatReq
			// 
			this.butLinkFeatReq.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLinkFeatReq.Autosize = true;
			this.butLinkFeatReq.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLinkFeatReq.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLinkFeatReq.CornerRadius = 4F;
			this.butLinkFeatReq.Location = new System.Drawing.Point(79, 18);
			this.butLinkFeatReq.Name = "butLinkFeatReq";
			this.butLinkFeatReq.Size = new System.Drawing.Size(65, 22);
			this.butLinkFeatReq.TabIndex = 1;
			this.butLinkFeatReq.Text = "Feat. Req.";
			this.butLinkFeatReq.Click += new System.EventHandler(this.butLinkFeatReq_Click);
			// 
			// butLinkTask
			// 
			this.butLinkTask.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLinkTask.Autosize = true;
			this.butLinkTask.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLinkTask.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLinkTask.CornerRadius = 4F;
			this.butLinkTask.Location = new System.Drawing.Point(8, 18);
			this.butLinkTask.Name = "butLinkTask";
			this.butLinkTask.Size = new System.Drawing.Size(65, 22);
			this.butLinkTask.TabIndex = 0;
			this.butLinkTask.Text = "Task";
			this.butLinkTask.Click += new System.EventHandler(this.butLinkTask_Click);
			// 
			// butPickProject
			// 
			this.butPickProject.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPickProject.Autosize = false;
			this.butPickProject.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProject.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProject.CornerRadius = 2F;
			this.butPickProject.Location = new System.Drawing.Point(747, 11);
			this.butPickProject.Name = "butPickProject";
			this.butPickProject.Size = new System.Drawing.Size(23, 21);
			this.butPickProject.TabIndex = 9;
			this.butPickProject.Text = "...";
			this.butPickProject.Click += new System.EventHandler(this.butPickProject_Click);
			// 
			// butPickExpert
			// 
			this.butPickExpert.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickExpert.Autosize = false;
			this.butPickExpert.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickExpert.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickExpert.CornerRadius = 2F;
			this.butPickExpert.Location = new System.Drawing.Point(255, 55);
			this.butPickExpert.Name = "butPickExpert";
			this.butPickExpert.Size = new System.Drawing.Size(23, 21);
			this.butPickExpert.TabIndex = 1;
			this.butPickExpert.Text = "...";
			this.butPickExpert.Click += new System.EventHandler(this.butPickExpert_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(616, 360);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(697, 360);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 16;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormJobEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 396);
			this.Controls.Add(this.textNotes);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butPickOwner);
			this.Controls.Add(this.comboType);
			this.Controls.Add(this.comboStatus);
			this.Controls.Add(this.comboPriority);
			this.Controls.Add(this.butHistory);
			this.Controls.Add(this.groupAddLink);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.textOwner);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textActualHours);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textEstHours);
			this.Controls.Add(this.textVersion);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butPickProject);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textProject);
			this.Controls.Add(this.butPickExpert);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textExpert);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.textJobNum);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(650, 333);
			this.Name = "FormJobEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Job Edit";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormJobEdit_Load);
			this.groupAddLink.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textJobNum;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textExpert;
		private UI.Button butPickExpert;
		private UI.Button butPickProject;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textProject;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textVersion;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textEstHours;
		private System.Windows.Forms.TextBox textActualHours;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textDateEntry;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textOwner;
		private System.Windows.Forms.GroupBox groupAddLink;
		private UI.Button butLinkBug;
		private UI.Button butLinkFeatReq;
		private UI.Button butLinkTask;
		private UI.Button butHistory;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.ComboBox comboStatus;
		private System.Windows.Forms.ComboBox comboType;
		private UI.Button butPickOwner;
		private UI.Button butDelete;
		private ODtextBox textDescription;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textTitle;
		private ODtextBox textNotes;
		private System.Windows.Forms.Label label13;
	}
}