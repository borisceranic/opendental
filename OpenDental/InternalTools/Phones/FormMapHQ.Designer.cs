namespace OpenDental {
	partial class FormMapHQ {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMapHQ));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.escalationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleTriageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboRoom = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.labelCurrentTime = new OpenDental.MapAreaRoomControl();
			this.labelTriageCoordinator = new System.Windows.Forms.Label();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.eServiceMetricsControl = new OpenDental.EServiceMetricsControl();
			this.officesDownView = new OpenDental.EscalationViewControl();
			this.label3 = new System.Windows.Forms.Label();
			this.escalationView = new OpenDental.EscalationViewControl();
			this.label2 = new System.Windows.Forms.Label();
			this.labelTriageOpsStaff = new OpenDental.MapAreaRoomControl();
			this.groupPhoneMetrics = new System.Windows.Forms.GroupBox();
			this.labelTriageTimeSpan = new OpenDental.MapAreaRoomControl();
			this.labelTriageRedCalls = new OpenDental.MapAreaRoomControl();
			this.label1 = new System.Windows.Forms.Label();
			this.labelTriageRedTimeSpan = new OpenDental.MapAreaRoomControl();
			this.label4 = new System.Windows.Forms.Label();
			this.labelVoicemailTimeSpan = new OpenDental.MapAreaRoomControl();
			this.label11 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.labelTriageCalls = new OpenDental.MapAreaRoomControl();
			this.labelVoicemailCalls = new OpenDental.MapAreaRoomControl();
			this.label14 = new System.Windows.Forms.Label();
			this.mapAreaPanelHQ = new OpenDental.MapAreaPanel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.openNewMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.groupPhoneMetrics.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.menuStrip);
			this.splitContainer1.Panel1.Controls.Add(this.comboRoom);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.labelCurrentTime);
			this.splitContainer1.Panel1.Controls.Add(this.labelTriageCoordinator);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(1884, 1042);
			this.splitContainer1.SplitterDistance = 84;
			this.splitContainer1.TabIndex = 0;
			// 
			// menuStrip
			// 
			this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this.toggleTriageToolStripMenuItem,
            this.openNewMapToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(6, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(447, 24);
			this.menuStrip.TabIndex = 71;
			this.menuStrip.Text = "menuStrip1";
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem,
            this.escalationToolStripMenuItem});
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.setupToolStripMenuItem.Text = "Setup";
			// 
			// mapToolStripMenuItem
			// 
			this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
			this.mapToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.mapToolStripMenuItem.Text = "Map";
			this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
			// 
			// escalationToolStripMenuItem
			// 
			this.escalationToolStripMenuItem.Name = "escalationToolStripMenuItem";
			this.escalationToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.escalationToolStripMenuItem.Text = "Escalation";
			this.escalationToolStripMenuItem.Click += new System.EventHandler(this.escalationToolStripMenuItem_Click);
			// 
			// fullScreenToolStripMenuItem
			// 
			this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
			this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
			this.fullScreenToolStripMenuItem.Text = "Full Screen";
			this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
			// 
			// toggleTriageToolStripMenuItem
			// 
			this.toggleTriageToolStripMenuItem.Name = "toggleTriageToolStripMenuItem";
			this.toggleTriageToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
			this.toggleTriageToolStripMenuItem.Text = "Toggle Triage View";
			this.toggleTriageToolStripMenuItem.Click += new System.EventHandler(this.toggleTriageToolStripMenuItem_Click);
			// 
			// comboRoom
			// 
			this.comboRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboRoom.ForeColor = System.Drawing.Color.Blue;
			this.comboRoom.ItemHeight = 40;
			this.comboRoom.Location = new System.Drawing.Point(136, 33);
			this.comboRoom.MaxDropDownItems = 30;
			this.comboRoom.Name = "comboRoom";
			this.comboRoom.Size = new System.Drawing.Size(258, 48);
			this.comboRoom.TabIndex = 70;
			this.comboRoom.SelectionChangeCommitted += new System.EventHandler(this.comboRoom_SelectionChangeCommitted);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(10, 35);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 46);
			this.label5.TabIndex = 67;
			this.label5.Text = "Room:";
			// 
			// labelCurrentTime
			// 
			this.labelCurrentTime.AllowDragging = false;
			this.labelCurrentTime.AllowEdit = false;
			this.labelCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCurrentTime.BorderThickness = 2;
			this.labelCurrentTime.Elapsed = null;
			this.labelCurrentTime.EmployeeName = null;
			this.labelCurrentTime.EmployeeNum = ((long)(0));
			this.labelCurrentTime.Empty = false;
			this.labelCurrentTime.Extension = null;
			this.labelCurrentTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F);
			this.labelCurrentTime.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCurrentTime.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelCurrentTime.InnerColor = System.Drawing.SystemColors.Control;
			this.labelCurrentTime.Location = new System.Drawing.Point(1690, 12);
			this.labelCurrentTime.Name = "labelCurrentTime";
			this.labelCurrentTime.OuterColor = System.Drawing.SystemColors.Control;
			this.labelCurrentTime.PhoneImage = null;
			this.labelCurrentTime.Size = new System.Drawing.Size(182, 61);
			this.labelCurrentTime.Status = null;
			this.labelCurrentTime.TabIndex = 66;
			this.labelCurrentTime.Text = "12:45 PM";
			// 
			// labelTriageCoordinator
			// 
			this.labelTriageCoordinator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTriageCoordinator.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageCoordinator.Location = new System.Drawing.Point(400, 7);
			this.labelTriageCoordinator.Name = "labelTriageCoordinator";
			this.labelTriageCoordinator.Size = new System.Drawing.Size(1305, 72);
			this.labelTriageCoordinator.TabIndex = 65;
			this.labelTriageCoordinator.Text = "Call Center Status Map - Triage Coordinator - Jim Smith";
			this.labelTriageCoordinator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.eServiceMetricsControl);
			this.splitContainer2.Panel1.Controls.Add(this.officesDownView);
			this.splitContainer2.Panel1.Controls.Add(this.label3);
			this.splitContainer2.Panel1.Controls.Add(this.escalationView);
			this.splitContainer2.Panel1.Controls.Add(this.label2);
			this.splitContainer2.Panel1.Controls.Add(this.labelTriageOpsStaff);
			this.splitContainer2.Panel1.Controls.Add(this.groupPhoneMetrics);
			this.splitContainer2.Panel1.Controls.Add(this.label14);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.mapAreaPanelHQ);
			this.splitContainer2.Size = new System.Drawing.Size(1884, 954);
			this.splitContainer2.SplitterDistance = 513;
			this.splitContainer2.TabIndex = 34;
			// 
			// eServiceMetricsControl
			// 
			this.eServiceMetricsControl.AccountBalance = 562F;
			this.eServiceMetricsControl.AlertColor = System.Drawing.Color.Blue;
			this.eServiceMetricsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.eServiceMetricsControl.Location = new System.Drawing.Point(180, 884);
			this.eServiceMetricsControl.Name = "eServiceMetricsControl";
			this.eServiceMetricsControl.Size = new System.Drawing.Size(317, 62);
			this.eServiceMetricsControl.TabIndex = 88;
			// 
			// officesDownView
			// 
			this.officesDownView.BackColor = System.Drawing.Color.White;
			this.officesDownView.BorderThickness = 1;
			this.officesDownView.FadeAlphaIncrement = 0;
			this.officesDownView.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.officesDownView.Items = ((System.ComponentModel.BindingList<string>)(resources.GetObject("officesDownView.Items")));
			this.officesDownView.LinePadding = -6;
			this.officesDownView.Location = new System.Drawing.Point(180, 742);
			this.officesDownView.MinAlpha = 60;
			this.officesDownView.Name = "officesDownView";
			this.officesDownView.OuterColor = System.Drawing.Color.Black;
			this.officesDownView.Size = new System.Drawing.Size(317, 140);
			this.officesDownView.StartFadeIndex = 0;
			this.officesDownView.TabIndex = 87;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(2, 743);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 111);
			this.label3.TabIndex = 86;
			this.label3.Text = "Offices\r\nDown";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// escalationView
			// 
			this.escalationView.BackColor = System.Drawing.Color.White;
			this.escalationView.BorderThickness = 1;
			this.escalationView.FadeAlphaIncrement = 20;
			this.escalationView.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.escalationView.Items = ((System.ComponentModel.BindingList<string>)(resources.GetObject("escalationView.Items")));
			this.escalationView.LinePadding = -6;
			this.escalationView.Location = new System.Drawing.Point(180, 430);
			this.escalationView.MinAlpha = 60;
			this.escalationView.Name = "escalationView";
			this.escalationView.OuterColor = System.Drawing.Color.Black;
			this.escalationView.Size = new System.Drawing.Size(317, 306);
			this.escalationView.StartFadeIndex = 0;
			this.escalationView.TabIndex = 85;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(1, 431);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 46);
			this.label2.TabIndex = 84;
			this.label2.Text = "Escalation";
			// 
			// labelTriageOpsStaff
			// 
			this.labelTriageOpsStaff.AllowDragging = false;
			this.labelTriageOpsStaff.AllowEdit = false;
			this.labelTriageOpsStaff.BorderThickness = 1;
			this.labelTriageOpsStaff.Elapsed = null;
			this.labelTriageOpsStaff.EmployeeName = null;
			this.labelTriageOpsStaff.EmployeeNum = ((long)(0));
			this.labelTriageOpsStaff.Empty = false;
			this.labelTriageOpsStaff.Extension = null;
			this.labelTriageOpsStaff.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageOpsStaff.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageOpsStaff.InnerColor = System.Drawing.Color.LightCyan;
			this.labelTriageOpsStaff.Location = new System.Drawing.Point(182, 351);
			this.labelTriageOpsStaff.Name = "labelTriageOpsStaff";
			this.labelTriageOpsStaff.OuterColor = System.Drawing.Color.Blue;
			this.labelTriageOpsStaff.PhoneImage = null;
			this.labelTriageOpsStaff.Size = new System.Drawing.Size(107, 70);
			this.labelTriageOpsStaff.Status = null;
			this.labelTriageOpsStaff.TabIndex = 83;
			this.labelTriageOpsStaff.Text = "0";
			// 
			// groupPhoneMetrics
			// 
			this.groupPhoneMetrics.Controls.Add(this.labelTriageTimeSpan);
			this.groupPhoneMetrics.Controls.Add(this.labelTriageRedCalls);
			this.groupPhoneMetrics.Controls.Add(this.label1);
			this.groupPhoneMetrics.Controls.Add(this.labelTriageRedTimeSpan);
			this.groupPhoneMetrics.Controls.Add(this.label4);
			this.groupPhoneMetrics.Controls.Add(this.labelVoicemailTimeSpan);
			this.groupPhoneMetrics.Controls.Add(this.label11);
			this.groupPhoneMetrics.Controls.Add(this.label6);
			this.groupPhoneMetrics.Controls.Add(this.label10);
			this.groupPhoneMetrics.Controls.Add(this.labelTriageCalls);
			this.groupPhoneMetrics.Controls.Add(this.labelVoicemailCalls);
			this.groupPhoneMetrics.Location = new System.Drawing.Point(4, 0);
			this.groupPhoneMetrics.Name = "groupPhoneMetrics";
			this.groupPhoneMetrics.Size = new System.Drawing.Size(499, 339);
			this.groupPhoneMetrics.TabIndex = 82;
			this.groupPhoneMetrics.TabStop = false;
			// 
			// labelTriageTimeSpan
			// 
			this.labelTriageTimeSpan.AllowDragging = false;
			this.labelTriageTimeSpan.AllowEdit = false;
			this.labelTriageTimeSpan.BorderThickness = 1;
			this.labelTriageTimeSpan.Elapsed = null;
			this.labelTriageTimeSpan.EmployeeName = null;
			this.labelTriageTimeSpan.EmployeeNum = ((long)(0));
			this.labelTriageTimeSpan.Empty = false;
			this.labelTriageTimeSpan.Extension = null;
			this.labelTriageTimeSpan.Font = new System.Drawing.Font("Calibri", 56F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageTimeSpan.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageTimeSpan.InnerColor = System.Drawing.Color.White;
			this.labelTriageTimeSpan.Location = new System.Drawing.Point(291, 262);
			this.labelTriageTimeSpan.Name = "labelTriageTimeSpan";
			this.labelTriageTimeSpan.OuterColor = System.Drawing.Color.Black;
			this.labelTriageTimeSpan.PhoneImage = null;
			this.labelTriageTimeSpan.Size = new System.Drawing.Size(202, 70);
			this.labelTriageTimeSpan.Status = null;
			this.labelTriageTimeSpan.TabIndex = 33;
			this.labelTriageTimeSpan.Text = "0000";
			// 
			// labelTriageRedCalls
			// 
			this.labelTriageRedCalls.AllowDragging = false;
			this.labelTriageRedCalls.AllowEdit = false;
			this.labelTriageRedCalls.BackColor = System.Drawing.Color.White;
			this.labelTriageRedCalls.BorderThickness = 1;
			this.labelTriageRedCalls.Elapsed = null;
			this.labelTriageRedCalls.EmployeeName = null;
			this.labelTriageRedCalls.EmployeeNum = ((long)(0));
			this.labelTriageRedCalls.Empty = false;
			this.labelTriageRedCalls.Extension = null;
			this.labelTriageRedCalls.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageRedCalls.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageRedCalls.InnerColor = System.Drawing.Color.White;
			this.labelTriageRedCalls.Location = new System.Drawing.Point(178, 64);
			this.labelTriageRedCalls.Name = "labelTriageRedCalls";
			this.labelTriageRedCalls.OuterColor = System.Drawing.Color.Black;
			this.labelTriageRedCalls.PhoneImage = null;
			this.labelTriageRedCalls.Size = new System.Drawing.Size(107, 70);
			this.labelTriageRedCalls.Status = null;
			this.labelTriageRedCalls.TabIndex = 12;
			this.labelTriageRedCalls.Text = "0";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 76);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 46);
			this.label1.TabIndex = 6;
			this.label1.Text = "Triage Red";
			// 
			// labelTriageRedTimeSpan
			// 
			this.labelTriageRedTimeSpan.AllowDragging = false;
			this.labelTriageRedTimeSpan.AllowEdit = false;
			this.labelTriageRedTimeSpan.BackColor = System.Drawing.Color.White;
			this.labelTriageRedTimeSpan.BorderThickness = 1;
			this.labelTriageRedTimeSpan.Elapsed = null;
			this.labelTriageRedTimeSpan.EmployeeName = null;
			this.labelTriageRedTimeSpan.EmployeeNum = ((long)(0));
			this.labelTriageRedTimeSpan.Empty = false;
			this.labelTriageRedTimeSpan.Extension = null;
			this.labelTriageRedTimeSpan.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageRedTimeSpan.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageRedTimeSpan.InnerColor = System.Drawing.Color.White;
			this.labelTriageRedTimeSpan.Location = new System.Drawing.Point(291, 64);
			this.labelTriageRedTimeSpan.Name = "labelTriageRedTimeSpan";
			this.labelTriageRedTimeSpan.OuterColor = System.Drawing.Color.Black;
			this.labelTriageRedTimeSpan.PhoneImage = null;
			this.labelTriageRedTimeSpan.Size = new System.Drawing.Size(203, 70);
			this.labelTriageRedTimeSpan.Status = null;
			this.labelTriageRedTimeSpan.TabIndex = 7;
			this.labelTriageRedTimeSpan.Text = "00:00";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(4, 175);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(170, 46);
			this.label4.TabIndex = 8;
			this.label4.Text = "Voicemail";
			// 
			// labelVoicemailTimeSpan
			// 
			this.labelVoicemailTimeSpan.AllowDragging = false;
			this.labelVoicemailTimeSpan.AllowEdit = false;
			this.labelVoicemailTimeSpan.BackColor = System.Drawing.Color.White;
			this.labelVoicemailTimeSpan.BorderThickness = 1;
			this.labelVoicemailTimeSpan.Elapsed = null;
			this.labelVoicemailTimeSpan.EmployeeName = null;
			this.labelVoicemailTimeSpan.EmployeeNum = ((long)(0));
			this.labelVoicemailTimeSpan.Empty = false;
			this.labelVoicemailTimeSpan.Extension = null;
			this.labelVoicemailTimeSpan.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVoicemailTimeSpan.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVoicemailTimeSpan.InnerColor = System.Drawing.Color.White;
			this.labelVoicemailTimeSpan.Location = new System.Drawing.Point(291, 163);
			this.labelVoicemailTimeSpan.Name = "labelVoicemailTimeSpan";
			this.labelVoicemailTimeSpan.OuterColor = System.Drawing.Color.Black;
			this.labelVoicemailTimeSpan.PhoneImage = null;
			this.labelVoicemailTimeSpan.Size = new System.Drawing.Size(202, 70);
			this.labelVoicemailTimeSpan.Status = null;
			this.labelVoicemailTimeSpan.TabIndex = 9;
			this.labelVoicemailTimeSpan.Text = "00:00";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(343, 15);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(97, 46);
			this.label11.TabIndex = 16;
			this.label11.Text = "Time";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(4, 274);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(114, 46);
			this.label6.TabIndex = 10;
			this.label6.Text = "Triage";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(172, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(119, 46);
			this.label10.TabIndex = 15;
			this.label10.Text = "# Calls";
			// 
			// labelTriageCalls
			// 
			this.labelTriageCalls.AllowDragging = false;
			this.labelTriageCalls.AllowEdit = false;
			this.labelTriageCalls.BackColor = System.Drawing.Color.White;
			this.labelTriageCalls.BorderThickness = 1;
			this.labelTriageCalls.Elapsed = null;
			this.labelTriageCalls.EmployeeName = null;
			this.labelTriageCalls.EmployeeNum = ((long)(0));
			this.labelTriageCalls.Empty = false;
			this.labelTriageCalls.Extension = null;
			this.labelTriageCalls.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageCalls.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriageCalls.InnerColor = System.Drawing.Color.White;
			this.labelTriageCalls.Location = new System.Drawing.Point(178, 262);
			this.labelTriageCalls.Name = "labelTriageCalls";
			this.labelTriageCalls.OuterColor = System.Drawing.Color.Black;
			this.labelTriageCalls.PhoneImage = null;
			this.labelTriageCalls.Size = new System.Drawing.Size(107, 70);
			this.labelTriageCalls.Status = null;
			this.labelTriageCalls.TabIndex = 14;
			this.labelTriageCalls.Text = "0";
			// 
			// labelVoicemailCalls
			// 
			this.labelVoicemailCalls.AllowDragging = false;
			this.labelVoicemailCalls.AllowEdit = false;
			this.labelVoicemailCalls.BackColor = System.Drawing.Color.White;
			this.labelVoicemailCalls.BorderThickness = 1;
			this.labelVoicemailCalls.Elapsed = null;
			this.labelVoicemailCalls.EmployeeName = null;
			this.labelVoicemailCalls.EmployeeNum = ((long)(0));
			this.labelVoicemailCalls.Empty = false;
			this.labelVoicemailCalls.Extension = null;
			this.labelVoicemailCalls.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVoicemailCalls.FontHeader = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVoicemailCalls.InnerColor = System.Drawing.Color.White;
			this.labelVoicemailCalls.Location = new System.Drawing.Point(178, 163);
			this.labelVoicemailCalls.Name = "labelVoicemailCalls";
			this.labelVoicemailCalls.OuterColor = System.Drawing.Color.Black;
			this.labelVoicemailCalls.PhoneImage = null;
			this.labelVoicemailCalls.Size = new System.Drawing.Size(107, 70);
			this.labelVoicemailCalls.Status = null;
			this.labelVoicemailCalls.TabIndex = 13;
			this.labelVoicemailCalls.Text = "0";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(-2, 363);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(183, 46);
			this.label14.TabIndex = 81;
			this.label14.Text = "Triage Ops";
			// 
			// mapAreaPanelHQ
			// 
			this.mapAreaPanelHQ.AllowDragging = false;
			this.mapAreaPanelHQ.AllowEditing = false;
			this.mapAreaPanelHQ.AutoScroll = true;
			this.mapAreaPanelHQ.AutoScrollMinSize = new System.Drawing.Size(1326, 935);
			this.mapAreaPanelHQ.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapAreaPanelHQ.FloorColor = System.Drawing.Color.White;
			this.mapAreaPanelHQ.FloorHeightFeet = 55;
			this.mapAreaPanelHQ.FloorWidthFeet = 78;
			this.mapAreaPanelHQ.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapAreaPanelHQ.FontCubicle = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapAreaPanelHQ.FontCubicleHeader = new System.Drawing.Font("Calibri", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapAreaPanelHQ.FontLabel = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapAreaPanelHQ.GridColor = System.Drawing.Color.LightGray;
			this.mapAreaPanelHQ.Location = new System.Drawing.Point(0, 0);
			this.mapAreaPanelHQ.Name = "mapAreaPanelHQ";
			this.mapAreaPanelHQ.PixelsPerFoot = 17;
			this.mapAreaPanelHQ.ShowGrid = false;
			this.mapAreaPanelHQ.ShowOutline = true;
			this.mapAreaPanelHQ.Size = new System.Drawing.Size(1367, 954);
			this.mapAreaPanelHQ.TabIndex = 71;
			this.mapAreaPanelHQ.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mapAreaPanelHQ_Scroll);
			// 
			// timer1
			// 
			this.timer1.Interval = 250;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// openNewMapToolStripMenuItem
			// 
			this.openNewMapToolStripMenuItem.Name = "openNewMapToolStripMenuItem";
			this.openNewMapToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
			this.openNewMapToolStripMenuItem.Text = "Open New Map";
			this.openNewMapToolStripMenuItem.Click += new System.EventHandler(this.openNewMapToolStripMenuItem_Click);
			// 
			// FormMapHQ
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1884, 1042);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMapHQ";
			this.Text = "Call Center Status Map";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMapHQ_FormClosed);
			this.Load += new System.EventHandler(this.FormMapHQ_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.groupPhoneMetrics.ResumeLayout(false);
			this.groupPhoneMetrics.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem escalationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboRoom;
		private System.Windows.Forms.Label label5;
		private MapAreaRoomControl labelCurrentTime;
		private System.Windows.Forms.Label labelTriageCoordinator;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private EServiceMetricsControl eServiceMetricsControl;
		private EscalationViewControl officesDownView;
		private System.Windows.Forms.Label label3;
		private EscalationViewControl escalationView;
		private System.Windows.Forms.Label label2;
		private MapAreaRoomControl labelTriageOpsStaff;
		private System.Windows.Forms.GroupBox groupPhoneMetrics;
		private MapAreaRoomControl labelTriageTimeSpan;
		private MapAreaRoomControl labelTriageRedCalls;
		private System.Windows.Forms.Label label1;
		private MapAreaRoomControl labelTriageRedTimeSpan;
		private System.Windows.Forms.Label label4;
		private MapAreaRoomControl labelVoicemailTimeSpan;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label10;
		private MapAreaRoomControl labelTriageCalls;
		private MapAreaRoomControl labelVoicemailCalls;
		private System.Windows.Forms.Label label14;
		private MapAreaPanel mapAreaPanelHQ;
		private System.Windows.Forms.ToolStripMenuItem toggleTriageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openNewMapToolStripMenuItem;
	}
}