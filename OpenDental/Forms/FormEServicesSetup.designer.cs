namespace OpenDental{
	partial class FormEServicesSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEServicesSetup));
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textOpenDentalUrlPatientPortal = new System.Windows.Forms.TextBox();
			this.textBoxNotificationSubject = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxNotificationBody = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBoxNotification = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butGetUrlPatientPortal = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.textRedirectUrlPatientPortal = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabListenerService = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkAllowEConnectorComm = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textListenerPort = new OpenDental.ValidNum();
			this.label10 = new System.Windows.Forms.Label();
			this.butSaveListenerPort = new OpenDental.UI.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butInstallEConnector = new OpenDental.UI.Button();
			this.labelListenerServiceAck = new System.Windows.Forms.Label();
			this.butListenerServiceAck = new OpenDental.UI.Button();
			this.label27 = new System.Windows.Forms.Label();
			this.butListenerServiceHistoryRefresh = new OpenDental.UI.Button();
			this.label26 = new System.Windows.Forms.Label();
			this.gridListenerServiceStatusHistory = new OpenDental.UI.ODGrid();
			this.butStartListenerService = new OpenDental.UI.Button();
			this.label24 = new System.Windows.Forms.Label();
			this.labelListenerStatus = new System.Windows.Forms.Label();
			this.butListenerAlertsOff = new OpenDental.UI.Button();
			this.textListenerServiceStatus = new System.Windows.Forms.TextBox();
			this.tabMobileOld = new System.Windows.Forms.TabPage();
			this.checkTroubleshooting = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textDateTimeLastRun = new System.Windows.Forms.Label();
			this.groupPreferences = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.textMobileUserName = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butCurrentWorkstation = new OpenDental.UI.Button();
			this.textMobilePassword = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.textMobileSynchWorkStation = new System.Windows.Forms.TextBox();
			this.textSynchMinutes = new OpenDental.ValidNumber();
			this.label18 = new System.Windows.Forms.Label();
			this.butSaveMobileSynch = new OpenDental.UI.Button();
			this.textDateBefore = new OpenDental.ValidDate();
			this.labelMobileSynchURL = new System.Windows.Forms.Label();
			this.textMobileSyncServerURL = new System.Windows.Forms.TextBox();
			this.labelMinutesBetweenSynch = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.butFullSync = new OpenDental.UI.Button();
			this.butSync = new OpenDental.UI.Button();
			this.tabMobileNew = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butGetUrlMobileWeb = new OpenDental.UI.Button();
			this.textOpenDentalUrlMobileWeb = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.tabPatientPortal = new System.Windows.Forms.TabPage();
			this.butSavePatientPortal = new OpenDental.UI.Button();
			this.tabWebSched = new System.Windows.Forms.TabPage();
			this.butSignUp = new OpenDental.UI.Button();
			this.groupWebSchedPreview = new System.Windows.Forms.GroupBox();
			this.butWebSchedPickClinic = new OpenDental.UI.Button();
			this.butWebSchedPickProv = new OpenDental.UI.Button();
			this.label22 = new System.Windows.Forms.Label();
			this.comboWebSchedProviders = new System.Windows.Forms.ComboBox();
			this.butWebSchedToday = new OpenDental.UI.Button();
			this.gridWebSchedTimeSlots = new OpenDental.UI.ODGrid();
			this.textWebSchedDateStart = new OpenDental.ValidDate();
			this.labelWebSchedClinic = new System.Windows.Forms.Label();
			this.labelWebSchedRecallTypes = new System.Windows.Forms.Label();
			this.comboWebSchedClinic = new System.Windows.Forms.ComboBox();
			this.comboWebSchedRecallTypes = new System.Windows.Forms.ComboBox();
			this.groupRecallSetup = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.listBoxWebSchedProviderPref = new System.Windows.Forms.ListBox();
			this.gridWebSchedRecallTypes = new OpenDental.UI.ODGrid();
			this.gridWebSchedOperatories = new OpenDental.UI.ODGrid();
			this.label20 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.butRecallSchedSetup = new OpenDental.UI.Button();
			this.butWebSchedEnable = new OpenDental.UI.Button();
			this.labelWebSchedEnable = new System.Windows.Forms.Label();
			this.labelWebSchedDesc = new System.Windows.Forms.Label();
			this.tabSmsServices = new System.Windows.Forms.TabPage();
			this.butBackMonth = new OpenDental.UI.Button();
			this.dateTimePickerSms = new System.Windows.Forms.DateTimePicker();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.textCountryCode = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.checkSmsAgree = new System.Windows.Forms.CheckBox();
			this.comboClinicSms = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textSmsLimit = new System.Windows.Forms.TextBox();
			this.butSmsUnsubscribe = new OpenDental.UI.Button();
			this.butSmsCancel = new OpenDental.UI.Button();
			this.label28 = new System.Windows.Forms.Label();
			this.butSmsSubmit = new OpenDental.UI.Button();
			this.gridSmsSummary = new OpenDental.UI.ODGrid();
			this.gridClinics = new OpenDental.UI.ODGrid();
			this.butFwdMonth = new OpenDental.UI.Button();
			this.butToday = new OpenDental.UI.Button();
			this.label32 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button1 = new OpenDental.UI.Button();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.button2 = new OpenDental.UI.Button();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label33 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label34 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button3 = new OpenDental.UI.Button();
			this.button4 = new OpenDental.UI.Button();
			this.label36 = new System.Windows.Forms.Label();
			this.button5 = new OpenDental.UI.Button();
			this.odGrid1 = new OpenDental.UI.ODGrid();
			this.odGrid2 = new OpenDental.UI.ODGrid();
			this.button6 = new OpenDental.UI.Button();
			this.button7 = new OpenDental.UI.Button();
			this.label37 = new System.Windows.Forms.Label();
			this.groupBoxNotification.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabListenerService.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabMobileOld.SuspendLayout();
			this.groupPreferences.SuspendLayout();
			this.tabMobileNew.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPatientPortal.SuspendLayout();
			this.tabWebSched.SuspendLayout();
			this.groupWebSchedPreview.SuspendLayout();
			this.groupRecallSetup.SuspendLayout();
			this.tabSmsServices.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 17);
			this.label2.TabIndex = 40;
			this.label2.Text = "Hosted URL";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(39, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(863, 26);
			this.label3.TabIndex = 42;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// textOpenDentalUrlPatientPortal
			// 
			this.textOpenDentalUrlPatientPortal.Location = new System.Drawing.Point(144, 49);
			this.textOpenDentalUrlPatientPortal.Name = "textOpenDentalUrlPatientPortal";
			this.textOpenDentalUrlPatientPortal.Size = new System.Drawing.Size(349, 20);
			this.textOpenDentalUrlPatientPortal.TabIndex = 43;
			this.textOpenDentalUrlPatientPortal.Text = "Click \'Get URL\'";
			// 
			// textBoxNotificationSubject
			// 
			this.textBoxNotificationSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNotificationSubject.Location = new System.Drawing.Point(93, 72);
			this.textBoxNotificationSubject.Name = "textBoxNotificationSubject";
			this.textBoxNotificationSubject.Size = new System.Drawing.Size(798, 20);
			this.textBoxNotificationSubject.TabIndex = 45;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 17);
			this.label4.TabIndex = 44;
			this.label4.Text = "Subject";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxNotificationBody
			// 
			this.textBoxNotificationBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNotificationBody.Location = new System.Drawing.Point(93, 117);
			this.textBoxNotificationBody.Multiline = true;
			this.textBoxNotificationBody.Name = "textBoxNotificationBody";
			this.textBoxNotificationBody.Size = new System.Drawing.Size(798, 112);
			this.textBoxNotificationBody.TabIndex = 46;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(9, 115);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(75, 17);
			this.label6.TabIndex = 47;
			this.label6.Text = "Body";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxNotification
			// 
			this.groupBoxNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxNotification.Controls.Add(this.label9);
			this.groupBoxNotification.Controls.Add(this.label7);
			this.groupBoxNotification.Controls.Add(this.textBoxNotificationSubject);
			this.groupBoxNotification.Controls.Add(this.label6);
			this.groupBoxNotification.Controls.Add(this.label4);
			this.groupBoxNotification.Controls.Add(this.textBoxNotificationBody);
			this.groupBoxNotification.Location = new System.Drawing.Point(10, 309);
			this.groupBoxNotification.Name = "groupBoxNotification";
			this.groupBoxNotification.Size = new System.Drawing.Size(908, 240);
			this.groupBoxNotification.TabIndex = 48;
			this.groupBoxNotification.TabStop = false;
			this.groupBoxNotification.Text = "Notification Email";
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.Location = new System.Drawing.Point(39, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(852, 53);
			this.label9.TabIndex = 52;
			this.label9.Text = resources.GetString("label9.Text");
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(90, 95);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(573, 17);
			this.label7.TabIndex = 48;
			this.label7.Text = "[URL] will be replaced with the value of \'Patient Facing URL\' as entered above.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.butGetUrlPatientPortal);
			this.groupBox1.Controls.Add(this.textOpenDentalUrlPatientPortal);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(10, 7);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(908, 84);
			this.groupBox1.TabIndex = 49;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Open Dental Hosted";
			// 
			// butGetUrlPatientPortal
			// 
			this.butGetUrlPatientPortal.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGetUrlPatientPortal.Autosize = true;
			this.butGetUrlPatientPortal.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGetUrlPatientPortal.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGetUrlPatientPortal.CornerRadius = 4F;
			this.butGetUrlPatientPortal.Location = new System.Drawing.Point(499, 47);
			this.butGetUrlPatientPortal.Name = "butGetUrlPatientPortal";
			this.butGetUrlPatientPortal.Size = new System.Drawing.Size(75, 23);
			this.butGetUrlPatientPortal.TabIndex = 55;
			this.butGetUrlPatientPortal.Text = "Get URL";
			this.butGetUrlPatientPortal.UseVisualStyleBackColor = true;
			this.butGetUrlPatientPortal.Click += new System.EventHandler(this.butGetUrlPatientPortal_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(19, 101);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(129, 17);
			this.label8.TabIndex = 52;
			this.label8.Text = "Patient Facing URL";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRedirectUrlPatientPortal
			// 
			this.textRedirectUrlPatientPortal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textRedirectUrlPatientPortal.Location = new System.Drawing.Point(154, 99);
			this.textRedirectUrlPatientPortal.Name = "textRedirectUrlPatientPortal";
			this.textRedirectUrlPatientPortal.Size = new System.Drawing.Size(747, 20);
			this.textRedirectUrlPatientPortal.TabIndex = 50;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(49, 122);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(869, 184);
			this.label1.TabIndex = 51;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabListenerService);
			this.tabControl.Controls.Add(this.tabMobileOld);
			this.tabControl.Controls.Add(this.tabMobileNew);
			this.tabControl.Controls.Add(this.tabPatientPortal);
			this.tabControl.Controls.Add(this.tabWebSched);
			this.tabControl.Controls.Add(this.tabSmsServices);
			this.tabControl.Location = new System.Drawing.Point(12, 40);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(952, 614);
			this.tabControl.TabIndex = 53;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabListenerService
			// 
			this.tabListenerService.BackColor = System.Drawing.SystemColors.Control;
			this.tabListenerService.Controls.Add(this.groupBox4);
			this.tabListenerService.Controls.Add(this.label25);
			this.tabListenerService.Controls.Add(this.groupBox3);
			this.tabListenerService.Location = new System.Drawing.Point(4, 22);
			this.tabListenerService.Name = "tabListenerService";
			this.tabListenerService.Padding = new System.Windows.Forms.Padding(3);
			this.tabListenerService.Size = new System.Drawing.Size(944, 588);
			this.tabListenerService.TabIndex = 4;
			this.tabListenerService.Text = "eConnector Service";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.checkAllowEConnectorComm);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.textListenerPort);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.butSaveListenerPort);
			this.groupBox4.Location = new System.Drawing.Point(117, 445);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(711, 137);
			this.groupBox4.TabIndex = 252;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "eConnector Service Settings";
			// 
			// checkAllowEConnectorComm
			// 
			this.checkAllowEConnectorComm.AutoSize = true;
			this.checkAllowEConnectorComm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEConnectorComm.Location = new System.Drawing.Point(130, 77);
			this.checkAllowEConnectorComm.Name = "checkAllowEConnectorComm";
			this.checkAllowEConnectorComm.Size = new System.Drawing.Size(252, 17);
			this.checkAllowEConnectorComm.TabIndex = 244;
			this.checkAllowEConnectorComm.Text = "Allow eConnector to communicate for eServices";
			this.checkAllowEConnectorComm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEConnectorComm.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Location = new System.Drawing.Point(7, 18);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(698, 35);
			this.label11.TabIndex = 56;
			this.label11.Text = "The eConnector Port is the same for all eServices hosted by Open Dental and must " +
    "be forwarded by your router to the computer that is running the eConnector servi" +
    "ce.";
			// 
			// textListenerPort
			// 
			this.textListenerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textListenerPort.Location = new System.Drawing.Point(282, 54);
			this.textListenerPort.MaxVal = 65535;
			this.textListenerPort.MinVal = 0;
			this.textListenerPort.Name = "textListenerPort";
			this.textListenerPort.Size = new System.Drawing.Size(100, 20);
			this.textListenerPort.TabIndex = 51;
			this.textListenerPort.Text = "0";
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(91, 55);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(185, 17);
			this.label10.TabIndex = 57;
			this.label10.Text = "eConnector Port";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSaveListenerPort
			// 
			this.butSaveListenerPort.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveListenerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSaveListenerPort.Autosize = true;
			this.butSaveListenerPort.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveListenerPort.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveListenerPort.CornerRadius = 4F;
			this.butSaveListenerPort.Location = new System.Drawing.Point(323, 104);
			this.butSaveListenerPort.Name = "butSaveListenerPort";
			this.butSaveListenerPort.Size = new System.Drawing.Size(61, 24);
			this.butSaveListenerPort.TabIndex = 243;
			this.butSaveListenerPort.Text = "Save";
			this.butSaveListenerPort.Click += new System.EventHandler(this.butSaveListenerPort_Click);
			// 
			// label25
			// 
			this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label25.Location = new System.Drawing.Point(6, 8);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(932, 84);
			this.label25.TabIndex = 251;
			this.label25.Text = resources.GetString("label25.Text");
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.butInstallEConnector);
			this.groupBox3.Controls.Add(this.labelListenerServiceAck);
			this.groupBox3.Controls.Add(this.butListenerServiceAck);
			this.groupBox3.Controls.Add(this.label27);
			this.groupBox3.Controls.Add(this.butListenerServiceHistoryRefresh);
			this.groupBox3.Controls.Add(this.label26);
			this.groupBox3.Controls.Add(this.gridListenerServiceStatusHistory);
			this.groupBox3.Controls.Add(this.butStartListenerService);
			this.groupBox3.Controls.Add(this.label24);
			this.groupBox3.Controls.Add(this.labelListenerStatus);
			this.groupBox3.Controls.Add(this.butListenerAlertsOff);
			this.groupBox3.Controls.Add(this.textListenerServiceStatus);
			this.groupBox3.Location = new System.Drawing.Point(9, 100);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(929, 339);
			this.groupBox3.TabIndex = 249;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "eConnector Service Monitor";
			// 
			// butInstallEConnector
			// 
			this.butInstallEConnector.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInstallEConnector.Autosize = true;
			this.butInstallEConnector.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInstallEConnector.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInstallEConnector.CornerRadius = 4F;
			this.butInstallEConnector.Location = new System.Drawing.Point(594, 45);
			this.butInstallEConnector.Name = "butInstallEConnector";
			this.butInstallEConnector.Size = new System.Drawing.Size(61, 24);
			this.butInstallEConnector.TabIndex = 255;
			this.butInstallEConnector.Text = "Install";
			this.butInstallEConnector.Click += new System.EventHandler(this.butInstallEConnector_Click);
			// 
			// labelListenerServiceAck
			// 
			this.labelListenerServiceAck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelListenerServiceAck.Location = new System.Drawing.Point(278, 263);
			this.labelListenerServiceAck.Name = "labelListenerServiceAck";
			this.labelListenerServiceAck.Size = new System.Drawing.Size(578, 13);
			this.labelListenerServiceAck.TabIndex = 254;
			this.labelListenerServiceAck.Text = "Acknowledge all errors.  This will stop the eServices menu from showing yellow.";
			this.labelListenerServiceAck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butListenerServiceAck
			// 
			this.butListenerServiceAck.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListenerServiceAck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butListenerServiceAck.Autosize = true;
			this.butListenerServiceAck.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListenerServiceAck.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListenerServiceAck.CornerRadius = 4F;
			this.butListenerServiceAck.Location = new System.Drawing.Point(862, 257);
			this.butListenerServiceAck.Name = "butListenerServiceAck";
			this.butListenerServiceAck.Size = new System.Drawing.Size(61, 24);
			this.butListenerServiceAck.TabIndex = 253;
			this.butListenerServiceAck.Text = "Ack";
			this.butListenerServiceAck.Click += new System.EventHandler(this.butListenerServiceAck_Click);
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(7, 18);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(916, 19);
			this.label27.TabIndex = 252;
			this.label27.Text = "Open Dental monitors the status of the eConnector Service and alerts all workstat" +
    "ions when status is critical.";
			// 
			// butListenerServiceHistoryRefresh
			// 
			this.butListenerServiceHistoryRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListenerServiceHistoryRefresh.Autosize = true;
			this.butListenerServiceHistoryRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListenerServiceHistoryRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListenerServiceHistoryRefresh.CornerRadius = 4F;
			this.butListenerServiceHistoryRefresh.Location = new System.Drawing.Point(862, 87);
			this.butListenerServiceHistoryRefresh.Name = "butListenerServiceHistoryRefresh";
			this.butListenerServiceHistoryRefresh.Size = new System.Drawing.Size(61, 24);
			this.butListenerServiceHistoryRefresh.TabIndex = 251;
			this.butListenerServiceHistoryRefresh.Text = "Refresh";
			this.butListenerServiceHistoryRefresh.Click += new System.EventHandler(this.butListenerServiceHistoryRefresh_Click);
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(3, 70);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(853, 37);
			this.label26.TabIndex = 250;
			this.label26.Text = resources.GetString("label26.Text");
			this.label26.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridListenerServiceStatusHistory
			// 
			this.gridListenerServiceStatusHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridListenerServiceStatusHistory.HasMultilineHeaders = false;
			this.gridListenerServiceStatusHistory.HScrollVisible = false;
			this.gridListenerServiceStatusHistory.Location = new System.Drawing.Point(6, 117);
			this.gridListenerServiceStatusHistory.Name = "gridListenerServiceStatusHistory";
			this.gridListenerServiceStatusHistory.ScrollValue = 0;
			this.gridListenerServiceStatusHistory.Size = new System.Drawing.Size(917, 138);
			this.gridListenerServiceStatusHistory.TabIndex = 249;
			this.gridListenerServiceStatusHistory.Title = "eConnector History";
			this.gridListenerServiceStatusHistory.TranslationName = "FormEServicesSetup";
			this.gridListenerServiceStatusHistory.WrapText = false;
			// 
			// butStartListenerService
			// 
			this.butStartListenerService.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStartListenerService.Autosize = true;
			this.butStartListenerService.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStartListenerService.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStartListenerService.CornerRadius = 4F;
			this.butStartListenerService.Enabled = false;
			this.butStartListenerService.Location = new System.Drawing.Point(527, 45);
			this.butStartListenerService.Name = "butStartListenerService";
			this.butStartListenerService.Size = new System.Drawing.Size(61, 24);
			this.butStartListenerService.TabIndex = 245;
			this.butStartListenerService.Text = "Start";
			this.butStartListenerService.Click += new System.EventHandler(this.butStartListenerService_Click);
			// 
			// label24
			// 
			this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label24.Location = new System.Drawing.Point(115, 306);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(578, 29);
			this.label24.TabIndex = 248;
			this.label24.Text = "Before you stop monitoring, first uninstall the eConnector Service.\r\nMonitoring w" +
    "ill automatically resume when an active eConnector Service has been detected.";
			// 
			// labelListenerStatus
			// 
			this.labelListenerStatus.Location = new System.Drawing.Point(177, 48);
			this.labelListenerStatus.Name = "labelListenerStatus";
			this.labelListenerStatus.Size = new System.Drawing.Size(238, 17);
			this.labelListenerStatus.TabIndex = 244;
			this.labelListenerStatus.Text = "Current eConnector Service Status";
			this.labelListenerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butListenerAlertsOff
			// 
			this.butListenerAlertsOff.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListenerAlertsOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butListenerAlertsOff.Autosize = true;
			this.butListenerAlertsOff.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListenerAlertsOff.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListenerAlertsOff.CornerRadius = 4F;
			this.butListenerAlertsOff.Location = new System.Drawing.Point(9, 307);
			this.butListenerAlertsOff.Name = "butListenerAlertsOff";
			this.butListenerAlertsOff.Size = new System.Drawing.Size(100, 24);
			this.butListenerAlertsOff.TabIndex = 247;
			this.butListenerAlertsOff.Text = "Stop Monitoring";
			this.butListenerAlertsOff.Click += new System.EventHandler(this.butListenerAlertsOff_Click);
			// 
			// textListenerServiceStatus
			// 
			this.textListenerServiceStatus.Location = new System.Drawing.Point(421, 47);
			this.textListenerServiceStatus.Name = "textListenerServiceStatus";
			this.textListenerServiceStatus.ReadOnly = true;
			this.textListenerServiceStatus.Size = new System.Drawing.Size(100, 20);
			this.textListenerServiceStatus.TabIndex = 246;
			// 
			// tabMobileOld
			// 
			this.tabMobileOld.BackColor = System.Drawing.SystemColors.Control;
			this.tabMobileOld.Controls.Add(this.checkTroubleshooting);
			this.tabMobileOld.Controls.Add(this.butDelete);
			this.tabMobileOld.Controls.Add(this.textDateTimeLastRun);
			this.tabMobileOld.Controls.Add(this.groupPreferences);
			this.tabMobileOld.Controls.Add(this.label19);
			this.tabMobileOld.Controls.Add(this.butFullSync);
			this.tabMobileOld.Controls.Add(this.butSync);
			this.tabMobileOld.Location = new System.Drawing.Point(4, 22);
			this.tabMobileOld.Name = "tabMobileOld";
			this.tabMobileOld.Size = new System.Drawing.Size(944, 588);
			this.tabMobileOld.TabIndex = 2;
			this.tabMobileOld.Text = "Mobile Synch (old-style)";
			// 
			// checkTroubleshooting
			// 
			this.checkTroubleshooting.Location = new System.Drawing.Point(531, 230);
			this.checkTroubleshooting.Name = "checkTroubleshooting";
			this.checkTroubleshooting.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkTroubleshooting.Size = new System.Drawing.Size(184, 24);
			this.checkTroubleshooting.TabIndex = 254;
			this.checkTroubleshooting.Text = "Synch Troubleshooting Mode";
			this.checkTroubleshooting.UseVisualStyleBackColor = true;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Location = new System.Drawing.Point(399, 279);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(68, 24);
			this.butDelete.TabIndex = 253;
			this.butDelete.Text = "Delete All";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textDateTimeLastRun
			// 
			this.textDateTimeLastRun.Location = new System.Drawing.Point(400, 230);
			this.textDateTimeLastRun.Name = "textDateTimeLastRun";
			this.textDateTimeLastRun.Size = new System.Drawing.Size(207, 18);
			this.textDateTimeLastRun.TabIndex = 252;
			this.textDateTimeLastRun.Text = "3/4/2011 4:15 PM";
			this.textDateTimeLastRun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupPreferences
			// 
			this.groupPreferences.Controls.Add(this.label13);
			this.groupPreferences.Controls.Add(this.label14);
			this.groupPreferences.Controls.Add(this.textMobileUserName);
			this.groupPreferences.Controls.Add(this.label15);
			this.groupPreferences.Controls.Add(this.butCurrentWorkstation);
			this.groupPreferences.Controls.Add(this.textMobilePassword);
			this.groupPreferences.Controls.Add(this.label16);
			this.groupPreferences.Controls.Add(this.label17);
			this.groupPreferences.Controls.Add(this.textMobileSynchWorkStation);
			this.groupPreferences.Controls.Add(this.textSynchMinutes);
			this.groupPreferences.Controls.Add(this.label18);
			this.groupPreferences.Controls.Add(this.butSaveMobileSynch);
			this.groupPreferences.Controls.Add(this.textDateBefore);
			this.groupPreferences.Controls.Add(this.labelMobileSynchURL);
			this.groupPreferences.Controls.Add(this.textMobileSyncServerURL);
			this.groupPreferences.Controls.Add(this.labelMinutesBetweenSynch);
			this.groupPreferences.Location = new System.Drawing.Point(131, 7);
			this.groupPreferences.Name = "groupPreferences";
			this.groupPreferences.Size = new System.Drawing.Size(682, 212);
			this.groupPreferences.TabIndex = 251;
			this.groupPreferences.TabStop = false;
			this.groupPreferences.Text = "Preferences";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 183);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(575, 19);
			this.label13.TabIndex = 246;
			this.label13.Text = "To change your password, enter a new one in the box and Save.  To keep the old pa" +
    "ssword, leave the box empty.";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(222, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(343, 18);
			this.label14.TabIndex = 244;
			this.label14.Text = "Set to 0 to stop automatic Synchronization";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMobileUserName
			// 
			this.textMobileUserName.Location = new System.Drawing.Point(177, 131);
			this.textMobileUserName.Name = "textMobileUserName";
			this.textMobileUserName.Size = new System.Drawing.Size(247, 20);
			this.textMobileUserName.TabIndex = 242;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(5, 132);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(169, 19);
			this.label15.TabIndex = 243;
			this.label15.Text = "User Name";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCurrentWorkstation
			// 
			this.butCurrentWorkstation.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCurrentWorkstation.Autosize = true;
			this.butCurrentWorkstation.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCurrentWorkstation.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCurrentWorkstation.CornerRadius = 4F;
			this.butCurrentWorkstation.Location = new System.Drawing.Point(430, 101);
			this.butCurrentWorkstation.Name = "butCurrentWorkstation";
			this.butCurrentWorkstation.Size = new System.Drawing.Size(115, 24);
			this.butCurrentWorkstation.TabIndex = 247;
			this.butCurrentWorkstation.Text = "Current Workstation";
			this.butCurrentWorkstation.Click += new System.EventHandler(this.butCurrentWorkstation_Click);
			// 
			// textMobilePassword
			// 
			this.textMobilePassword.Location = new System.Drawing.Point(177, 159);
			this.textMobilePassword.Name = "textMobilePassword";
			this.textMobilePassword.PasswordChar = '*';
			this.textMobilePassword.Size = new System.Drawing.Size(247, 20);
			this.textMobilePassword.TabIndex = 243;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(4, 105);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(170, 18);
			this.label16.TabIndex = 246;
			this.label16.Text = "Workstation for Synching";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(5, 160);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(169, 19);
			this.label17.TabIndex = 244;
			this.label17.Text = "Password";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMobileSynchWorkStation
			// 
			this.textMobileSynchWorkStation.Location = new System.Drawing.Point(177, 103);
			this.textMobileSynchWorkStation.Name = "textMobileSynchWorkStation";
			this.textMobileSynchWorkStation.Size = new System.Drawing.Size(247, 20);
			this.textMobileSynchWorkStation.TabIndex = 245;
			// 
			// textSynchMinutes
			// 
			this.textSynchMinutes.Location = new System.Drawing.Point(177, 47);
			this.textSynchMinutes.MaxVal = 255;
			this.textSynchMinutes.MinVal = 0;
			this.textSynchMinutes.Name = "textSynchMinutes";
			this.textSynchMinutes.Size = new System.Drawing.Size(39, 20);
			this.textSynchMinutes.TabIndex = 241;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(5, 76);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(170, 18);
			this.label18.TabIndex = 85;
			this.label18.Text = "Exclude Appointments Before";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSaveMobileSynch
			// 
			this.butSaveMobileSynch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveMobileSynch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveMobileSynch.Autosize = true;
			this.butSaveMobileSynch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveMobileSynch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveMobileSynch.CornerRadius = 4F;
			this.butSaveMobileSynch.Location = new System.Drawing.Point(615, 182);
			this.butSaveMobileSynch.Name = "butSaveMobileSynch";
			this.butSaveMobileSynch.Size = new System.Drawing.Size(61, 24);
			this.butSaveMobileSynch.TabIndex = 240;
			this.butSaveMobileSynch.Text = "Save";
			this.butSaveMobileSynch.Click += new System.EventHandler(this.butSaveMobileSynch_Click);
			// 
			// textDateBefore
			// 
			this.textDateBefore.Location = new System.Drawing.Point(177, 75);
			this.textDateBefore.Name = "textDateBefore";
			this.textDateBefore.Size = new System.Drawing.Size(100, 20);
			this.textDateBefore.TabIndex = 84;
			// 
			// labelMobileSynchURL
			// 
			this.labelMobileSynchURL.Location = new System.Drawing.Point(6, 20);
			this.labelMobileSynchURL.Name = "labelMobileSynchURL";
			this.labelMobileSynchURL.Size = new System.Drawing.Size(169, 19);
			this.labelMobileSynchURL.TabIndex = 76;
			this.labelMobileSynchURL.Text = "Host Server Address";
			this.labelMobileSynchURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMobileSyncServerURL
			// 
			this.textMobileSyncServerURL.Location = new System.Drawing.Point(177, 19);
			this.textMobileSyncServerURL.Name = "textMobileSyncServerURL";
			this.textMobileSyncServerURL.Size = new System.Drawing.Size(445, 20);
			this.textMobileSyncServerURL.TabIndex = 75;
			// 
			// labelMinutesBetweenSynch
			// 
			this.labelMinutesBetweenSynch.Location = new System.Drawing.Point(6, 48);
			this.labelMinutesBetweenSynch.Name = "labelMinutesBetweenSynch";
			this.labelMinutesBetweenSynch.Size = new System.Drawing.Size(169, 19);
			this.labelMinutesBetweenSynch.TabIndex = 79;
			this.labelMinutesBetweenSynch.Text = "Minutes Between Synch";
			this.labelMinutesBetweenSynch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(230, 230);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(167, 18);
			this.label19.TabIndex = 250;
			this.label19.Text = "Date/time of last sync";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butFullSync
			// 
			this.butFullSync.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFullSync.Autosize = true;
			this.butFullSync.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFullSync.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFullSync.CornerRadius = 4F;
			this.butFullSync.Location = new System.Drawing.Point(473, 279);
			this.butFullSync.Name = "butFullSync";
			this.butFullSync.Size = new System.Drawing.Size(68, 24);
			this.butFullSync.TabIndex = 249;
			this.butFullSync.Text = "Full Synch";
			this.butFullSync.Click += new System.EventHandler(this.butFullSync_Click);
			// 
			// butSync
			// 
			this.butSync.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSync.Autosize = true;
			this.butSync.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSync.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSync.CornerRadius = 4F;
			this.butSync.Location = new System.Drawing.Point(547, 279);
			this.butSync.Name = "butSync";
			this.butSync.Size = new System.Drawing.Size(68, 24);
			this.butSync.TabIndex = 248;
			this.butSync.Text = "Synch";
			this.butSync.Click += new System.EventHandler(this.butSync_Click);
			// 
			// tabMobileNew
			// 
			this.tabMobileNew.BackColor = System.Drawing.SystemColors.Control;
			this.tabMobileNew.Controls.Add(this.groupBox2);
			this.tabMobileNew.Location = new System.Drawing.Point(4, 22);
			this.tabMobileNew.Name = "tabMobileNew";
			this.tabMobileNew.Padding = new System.Windows.Forms.Padding(3);
			this.tabMobileNew.Size = new System.Drawing.Size(944, 588);
			this.tabMobileNew.TabIndex = 0;
			this.tabMobileNew.Text = "Mobile Web (new-style)";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.butGetUrlMobileWeb);
			this.groupBox2.Controls.Add(this.textOpenDentalUrlMobileWeb);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Location = new System.Drawing.Point(10, 7);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(908, 84);
			this.groupBox2.TabIndex = 50;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Open Dental Hosted";
			// 
			// butGetUrlMobileWeb
			// 
			this.butGetUrlMobileWeb.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGetUrlMobileWeb.Autosize = true;
			this.butGetUrlMobileWeb.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGetUrlMobileWeb.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGetUrlMobileWeb.CornerRadius = 4F;
			this.butGetUrlMobileWeb.Location = new System.Drawing.Point(499, 47);
			this.butGetUrlMobileWeb.Name = "butGetUrlMobileWeb";
			this.butGetUrlMobileWeb.Size = new System.Drawing.Size(75, 23);
			this.butGetUrlMobileWeb.TabIndex = 55;
			this.butGetUrlMobileWeb.Text = "Get URL";
			this.butGetUrlMobileWeb.UseVisualStyleBackColor = true;
			this.butGetUrlMobileWeb.Click += new System.EventHandler(this.butGetUrlMobileWeb_Click);
			// 
			// textOpenDentalUrlMobileWeb
			// 
			this.textOpenDentalUrlMobileWeb.Location = new System.Drawing.Point(144, 49);
			this.textOpenDentalUrlMobileWeb.Name = "textOpenDentalUrlMobileWeb";
			this.textOpenDentalUrlMobileWeb.Size = new System.Drawing.Size(349, 20);
			this.textOpenDentalUrlMobileWeb.TabIndex = 43;
			this.textOpenDentalUrlMobileWeb.Text = "Click \'Get URL\'";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 51);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(126, 17);
			this.label5.TabIndex = 40;
			this.label5.Text = "Hosted URL";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label12.Location = new System.Drawing.Point(39, 18);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(863, 26);
			this.label12.TabIndex = 42;
			this.label12.Text = resources.GetString("label12.Text");
			// 
			// tabPatientPortal
			// 
			this.tabPatientPortal.BackColor = System.Drawing.SystemColors.Control;
			this.tabPatientPortal.Controls.Add(this.butSavePatientPortal);
			this.tabPatientPortal.Controls.Add(this.label1);
			this.tabPatientPortal.Controls.Add(this.label8);
			this.tabPatientPortal.Controls.Add(this.groupBoxNotification);
			this.tabPatientPortal.Controls.Add(this.textRedirectUrlPatientPortal);
			this.tabPatientPortal.Controls.Add(this.groupBox1);
			this.tabPatientPortal.Location = new System.Drawing.Point(4, 22);
			this.tabPatientPortal.Name = "tabPatientPortal";
			this.tabPatientPortal.Padding = new System.Windows.Forms.Padding(3);
			this.tabPatientPortal.Size = new System.Drawing.Size(944, 588);
			this.tabPatientPortal.TabIndex = 1;
			this.tabPatientPortal.Text = "Patient Portal";
			// 
			// butSavePatientPortal
			// 
			this.butSavePatientPortal.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSavePatientPortal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butSavePatientPortal.Autosize = true;
			this.butSavePatientPortal.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSavePatientPortal.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSavePatientPortal.CornerRadius = 4F;
			this.butSavePatientPortal.Location = new System.Drawing.Point(442, 555);
			this.butSavePatientPortal.Name = "butSavePatientPortal";
			this.butSavePatientPortal.Size = new System.Drawing.Size(61, 24);
			this.butSavePatientPortal.TabIndex = 241;
			this.butSavePatientPortal.Text = "Save";
			this.butSavePatientPortal.Click += new System.EventHandler(this.butSavePatientPortal_Click);
			// 
			// tabWebSched
			// 
			this.tabWebSched.BackColor = System.Drawing.SystemColors.Control;
			this.tabWebSched.Controls.Add(this.butSignUp);
			this.tabWebSched.Controls.Add(this.groupWebSchedPreview);
			this.tabWebSched.Controls.Add(this.groupRecallSetup);
			this.tabWebSched.Controls.Add(this.butWebSchedEnable);
			this.tabWebSched.Controls.Add(this.labelWebSchedEnable);
			this.tabWebSched.Controls.Add(this.labelWebSchedDesc);
			this.tabWebSched.Location = new System.Drawing.Point(4, 22);
			this.tabWebSched.Name = "tabWebSched";
			this.tabWebSched.Size = new System.Drawing.Size(944, 588);
			this.tabWebSched.TabIndex = 3;
			this.tabWebSched.Text = "Web Sched";
			// 
			// butSignUp
			// 
			this.butSignUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSignUp.Autosize = true;
			this.butSignUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSignUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSignUp.CornerRadius = 4F;
			this.butSignUp.Location = new System.Drawing.Point(818, 98);
			this.butSignUp.Name = "butSignUp";
			this.butSignUp.Size = new System.Drawing.Size(103, 24);
			this.butSignUp.TabIndex = 301;
			this.butSignUp.Text = "Sign Up";
			this.butSignUp.Click += new System.EventHandler(this.butSignUp_Click);
			// 
			// groupWebSchedPreview
			// 
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickClinic);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedPickProv);
			this.groupWebSchedPreview.Controls.Add(this.label22);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedProviders);
			this.groupWebSchedPreview.Controls.Add(this.butWebSchedToday);
			this.groupWebSchedPreview.Controls.Add(this.gridWebSchedTimeSlots);
			this.groupWebSchedPreview.Controls.Add(this.textWebSchedDateStart);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.labelWebSchedRecallTypes);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedClinic);
			this.groupWebSchedPreview.Controls.Add(this.comboWebSchedRecallTypes);
			this.groupWebSchedPreview.Location = new System.Drawing.Point(253, 387);
			this.groupWebSchedPreview.Name = "groupWebSchedPreview";
			this.groupWebSchedPreview.Size = new System.Drawing.Size(439, 194);
			this.groupWebSchedPreview.TabIndex = 252;
			this.groupWebSchedPreview.TabStop = false;
			this.groupWebSchedPreview.Text = "Available Times For Patients";
			// 
			// butWebSchedPickClinic
			// 
			this.butWebSchedPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebSchedPickClinic.Autosize = false;
			this.butWebSchedPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebSchedPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebSchedPickClinic.CornerRadius = 2F;
			this.butWebSchedPickClinic.Location = new System.Drawing.Point(414, 159);
			this.butWebSchedPickClinic.Name = "butWebSchedPickClinic";
			this.butWebSchedPickClinic.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickClinic.TabIndex = 313;
			this.butWebSchedPickClinic.Text = "...";
			this.butWebSchedPickClinic.Click += new System.EventHandler(this.butWebSchedPickClinic_Click);
			// 
			// butWebSchedPickProv
			// 
			this.butWebSchedPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebSchedPickProv.Autosize = false;
			this.butWebSchedPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebSchedPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebSchedPickProv.CornerRadius = 2F;
			this.butWebSchedPickProv.Location = new System.Drawing.Point(414, 118);
			this.butWebSchedPickProv.Name = "butWebSchedPickProv";
			this.butWebSchedPickProv.Size = new System.Drawing.Size(18, 21);
			this.butWebSchedPickProv.TabIndex = 312;
			this.butWebSchedPickProv.Text = "...";
			this.butWebSchedPickProv.Click += new System.EventHandler(this.butWebSchedPickProv_Click);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(200, 101);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(182, 14);
			this.label22.TabIndex = 310;
			this.label22.Text = "Provider";
			this.label22.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedProviders
			// 
			this.comboWebSchedProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedProviders.Location = new System.Drawing.Point(200, 118);
			this.comboWebSchedProviders.MaxDropDownItems = 30;
			this.comboWebSchedProviders.Name = "comboWebSchedProviders";
			this.comboWebSchedProviders.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedProviders.TabIndex = 311;
			this.comboWebSchedProviders.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedProviders_SelectionChangeCommitted);
			// 
			// butWebSchedToday
			// 
			this.butWebSchedToday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebSchedToday.Autosize = true;
			this.butWebSchedToday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebSchedToday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebSchedToday.CornerRadius = 4F;
			this.butWebSchedToday.Location = new System.Drawing.Point(334, 36);
			this.butWebSchedToday.Name = "butWebSchedToday";
			this.butWebSchedToday.Size = new System.Drawing.Size(75, 21);
			this.butWebSchedToday.TabIndex = 309;
			this.butWebSchedToday.Text = "Today";
			this.butWebSchedToday.Click += new System.EventHandler(this.butWebSchedToday_Click);
			// 
			// gridWebSchedTimeSlots
			// 
			this.gridWebSchedTimeSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridWebSchedTimeSlots.HasMultilineHeaders = false;
			this.gridWebSchedTimeSlots.HScrollVisible = false;
			this.gridWebSchedTimeSlots.Location = new System.Drawing.Point(18, 19);
			this.gridWebSchedTimeSlots.Name = "gridWebSchedTimeSlots";
			this.gridWebSchedTimeSlots.ScrollValue = 0;
			this.gridWebSchedTimeSlots.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedTimeSlots.Size = new System.Drawing.Size(174, 169);
			this.gridWebSchedTimeSlots.TabIndex = 302;
			this.gridWebSchedTimeSlots.Title = "Time Slots";
			this.gridWebSchedTimeSlots.TranslationName = "FormEServicesSetup";
			this.gridWebSchedTimeSlots.WrapText = false;
			// 
			// textWebSchedDateStart
			// 
			this.textWebSchedDateStart.Location = new System.Drawing.Point(203, 36);
			this.textWebSchedDateStart.Name = "textWebSchedDateStart";
			this.textWebSchedDateStart.Size = new System.Drawing.Size(90, 20);
			this.textWebSchedDateStart.TabIndex = 303;
			this.textWebSchedDateStart.Text = "07/08/2015";
			this.textWebSchedDateStart.TextChanged += new System.EventHandler(this.textWebSchedDateStart_TextChanged);
			// 
			// labelWebSchedClinic
			// 
			this.labelWebSchedClinic.Location = new System.Drawing.Point(200, 142);
			this.labelWebSchedClinic.Name = "labelWebSchedClinic";
			this.labelWebSchedClinic.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedClinic.TabIndex = 264;
			this.labelWebSchedClinic.Text = "Clinic";
			this.labelWebSchedClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelWebSchedRecallTypes
			// 
			this.labelWebSchedRecallTypes.Location = new System.Drawing.Point(200, 60);
			this.labelWebSchedRecallTypes.Name = "labelWebSchedRecallTypes";
			this.labelWebSchedRecallTypes.Size = new System.Drawing.Size(182, 14);
			this.labelWebSchedRecallTypes.TabIndex = 254;
			this.labelWebSchedRecallTypes.Text = "Recall Type";
			this.labelWebSchedRecallTypes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboWebSchedClinic
			// 
			this.comboWebSchedClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedClinic.Location = new System.Drawing.Point(200, 159);
			this.comboWebSchedClinic.MaxDropDownItems = 30;
			this.comboWebSchedClinic.Name = "comboWebSchedClinic";
			this.comboWebSchedClinic.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedClinic.TabIndex = 305;
			this.comboWebSchedClinic.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedClinic_SelectionChangeCommitted);
			// 
			// comboWebSchedRecallTypes
			// 
			this.comboWebSchedRecallTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboWebSchedRecallTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWebSchedRecallTypes.Location = new System.Drawing.Point(200, 77);
			this.comboWebSchedRecallTypes.MaxDropDownItems = 30;
			this.comboWebSchedRecallTypes.Name = "comboWebSchedRecallTypes";
			this.comboWebSchedRecallTypes.Size = new System.Drawing.Size(209, 21);
			this.comboWebSchedRecallTypes.TabIndex = 304;
			this.comboWebSchedRecallTypes.SelectionChangeCommitted += new System.EventHandler(this.comboWebSchedRecallTypes_SelectionChangeCommitted);
			// 
			// groupRecallSetup
			// 
			this.groupRecallSetup.Controls.Add(this.label21);
			this.groupRecallSetup.Controls.Add(this.listBoxWebSchedProviderPref);
			this.groupRecallSetup.Controls.Add(this.gridWebSchedRecallTypes);
			this.groupRecallSetup.Controls.Add(this.gridWebSchedOperatories);
			this.groupRecallSetup.Controls.Add(this.label20);
			this.groupRecallSetup.Controls.Add(this.label35);
			this.groupRecallSetup.Controls.Add(this.label31);
			this.groupRecallSetup.Controls.Add(this.butRecallSchedSetup);
			this.groupRecallSetup.Location = new System.Drawing.Point(6, 128);
			this.groupRecallSetup.Name = "groupRecallSetup";
			this.groupRecallSetup.Size = new System.Drawing.Size(935, 247);
			this.groupRecallSetup.TabIndex = 247;
			this.groupRecallSetup.TabStop = false;
			this.groupRecallSetup.Text = "Web Sched Settings";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(144, 184);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(406, 56);
			this.label21.TabIndex = 310;
			this.label21.Text = resources.GetString("label21.Text");
			// 
			// listBoxWebSchedProviderPref
			// 
			this.listBoxWebSchedProviderPref.FormattingEnabled = true;
			this.listBoxWebSchedProviderPref.Items.AddRange(new object[] {
            "First Available",
            "Primary Provider",
            "Secondary Provider",
            "Last Seen Hygienist"});
			this.listBoxWebSchedProviderPref.Location = new System.Drawing.Point(18, 184);
			this.listBoxWebSchedProviderPref.Name = "listBoxWebSchedProviderPref";
			this.listBoxWebSchedProviderPref.Size = new System.Drawing.Size(120, 56);
			this.listBoxWebSchedProviderPref.TabIndex = 309;
			this.listBoxWebSchedProviderPref.SelectedIndexChanged += new System.EventHandler(this.listBoxWebSchedProviderPref_SelectedIndexChanged);
			// 
			// gridWebSchedRecallTypes
			// 
			this.gridWebSchedRecallTypes.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gridWebSchedRecallTypes.HasMultilineHeaders = false;
			this.gridWebSchedRecallTypes.HScrollVisible = false;
			this.gridWebSchedRecallTypes.Location = new System.Drawing.Point(573, 31);
			this.gridWebSchedRecallTypes.Name = "gridWebSchedRecallTypes";
			this.gridWebSchedRecallTypes.ScrollValue = 0;
			this.gridWebSchedRecallTypes.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedRecallTypes.Size = new System.Drawing.Size(342, 147);
			this.gridWebSchedRecallTypes.TabIndex = 307;
			this.gridWebSchedRecallTypes.Title = "Recall Types";
			this.gridWebSchedRecallTypes.TranslationName = "FormEServicesSetup";
			this.gridWebSchedRecallTypes.WrapText = false;
			this.gridWebSchedRecallTypes.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWebSchedRecallTypes_CellDoubleClick);
			// 
			// gridWebSchedOperatories
			// 
			this.gridWebSchedOperatories.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gridWebSchedOperatories.HasMultilineHeaders = false;
			this.gridWebSchedOperatories.HScrollVisible = false;
			this.gridWebSchedOperatories.Location = new System.Drawing.Point(18, 31);
			this.gridWebSchedOperatories.Name = "gridWebSchedOperatories";
			this.gridWebSchedOperatories.ScrollValue = 0;
			this.gridWebSchedOperatories.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSchedOperatories.Size = new System.Drawing.Size(532, 147);
			this.gridWebSchedOperatories.TabIndex = 307;
			this.gridWebSchedOperatories.Title = "Operatories Considered";
			this.gridWebSchedOperatories.TranslationName = "FormEServicesSetup";
			this.gridWebSchedOperatories.WrapText = false;
			this.gridWebSchedOperatories.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWebSchedOperatories_CellDoubleClick);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(573, 197);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(233, 28);
			this.label20.TabIndex = 247;
			this.label20.Text = "Customize the notification message that will be sent to the patient.";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(570, 13);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(345, 15);
			this.label35.TabIndex = 254;
			this.label35.Text = "Double click to edit.";
			this.label35.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(293, 13);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(257, 15);
			this.label31.TabIndex = 254;
			this.label31.Text = "Double click to edit.";
			this.label31.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butRecallSchedSetup
			// 
			this.butRecallSchedSetup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallSchedSetup.Autosize = true;
			this.butRecallSchedSetup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallSchedSetup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallSchedSetup.CornerRadius = 4F;
			this.butRecallSchedSetup.Location = new System.Drawing.Point(812, 199);
			this.butRecallSchedSetup.Name = "butRecallSchedSetup";
			this.butRecallSchedSetup.Size = new System.Drawing.Size(103, 24);
			this.butRecallSchedSetup.TabIndex = 308;
			this.butRecallSchedSetup.Text = "Recall Setup";
			this.butRecallSchedSetup.Click += new System.EventHandler(this.butWebSchedSetup_Click);
			// 
			// butWebSchedEnable
			// 
			this.butWebSchedEnable.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebSchedEnable.Autosize = true;
			this.butWebSchedEnable.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebSchedEnable.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebSchedEnable.CornerRadius = 4F;
			this.butWebSchedEnable.Location = new System.Drawing.Point(24, 98);
			this.butWebSchedEnable.Name = "butWebSchedEnable";
			this.butWebSchedEnable.Size = new System.Drawing.Size(102, 24);
			this.butWebSchedEnable.TabIndex = 300;
			this.butWebSchedEnable.Text = "Enable";
			this.butWebSchedEnable.Click += new System.EventHandler(this.butWebSchedEnable_Click);
			// 
			// labelWebSchedEnable
			// 
			this.labelWebSchedEnable.Location = new System.Drawing.Point(129, 104);
			this.labelWebSchedEnable.Name = "labelWebSchedEnable";
			this.labelWebSchedEnable.Size = new System.Drawing.Size(526, 17);
			this.labelWebSchedEnable.TabIndex = 245;
			this.labelWebSchedEnable.Text = "labelWebSchedEnable";
			// 
			// labelWebSchedDesc
			// 
			this.labelWebSchedDesc.Location = new System.Drawing.Point(6, 12);
			this.labelWebSchedDesc.Name = "labelWebSchedDesc";
			this.labelWebSchedDesc.Size = new System.Drawing.Size(935, 83);
			this.labelWebSchedDesc.TabIndex = 52;
			this.labelWebSchedDesc.Text = resources.GetString("labelWebSchedDesc.Text");
			// 
			// tabSmsServices
			// 
			this.tabSmsServices.BackColor = System.Drawing.SystemColors.Control;
			this.tabSmsServices.Controls.Add(this.butBackMonth);
			this.tabSmsServices.Controls.Add(this.dateTimePickerSms);
			this.tabSmsServices.Controls.Add(this.groupBox5);
			this.tabSmsServices.Controls.Add(this.gridSmsSummary);
			this.tabSmsServices.Controls.Add(this.gridClinics);
			this.tabSmsServices.Controls.Add(this.butFwdMonth);
			this.tabSmsServices.Controls.Add(this.butToday);
			this.tabSmsServices.Location = new System.Drawing.Point(4, 22);
			this.tabSmsServices.Name = "tabSmsServices";
			this.tabSmsServices.Padding = new System.Windows.Forms.Padding(3);
			this.tabSmsServices.Size = new System.Drawing.Size(944, 588);
			this.tabSmsServices.TabIndex = 6;
			this.tabSmsServices.Text = "Texting Services";
			// 
			// butBackMonth
			// 
			this.butBackMonth.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butBackMonth.Autosize = true;
			this.butBackMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBackMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBackMonth.CornerRadius = 4F;
			this.butBackMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBackMonth.Image = ((System.Drawing.Image)(resources.GetObject("butBackMonth.Image")));
			this.butBackMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBackMonth.Location = new System.Drawing.Point(553, 480);
			this.butBackMonth.Name = "butBackMonth";
			this.butBackMonth.Size = new System.Drawing.Size(32, 22);
			this.butBackMonth.TabIndex = 268;
			this.butBackMonth.Text = "M";
			this.butBackMonth.Click += new System.EventHandler(this.butBackMonth_Click);
			// 
			// dateTimePickerSms
			// 
			this.dateTimePickerSms.CustomFormat = "MMM yyyy";
			this.dateTimePickerSms.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerSms.Location = new System.Drawing.Point(585, 481);
			this.dateTimePickerSms.Name = "dateTimePickerSms";
			this.dateTimePickerSms.Size = new System.Drawing.Size(113, 20);
			this.dateTimePickerSms.TabIndex = 258;
			this.dateTimePickerSms.ValueChanged += new System.EventHandler(this.dateTimePickerSms_ValueChanged);
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox5.Controls.Add(this.textCountryCode);
			this.groupBox5.Controls.Add(this.label30);
			this.groupBox5.Controls.Add(this.label29);
			this.groupBox5.Controls.Add(this.checkSmsAgree);
			this.groupBox5.Controls.Add(this.comboClinicSms);
			this.groupBox5.Controls.Add(this.labelClinic);
			this.groupBox5.Controls.Add(this.textSmsLimit);
			this.groupBox5.Controls.Add(this.butSmsUnsubscribe);
			this.groupBox5.Controls.Add(this.butSmsCancel);
			this.groupBox5.Controls.Add(this.label28);
			this.groupBox5.Controls.Add(this.butSmsSubmit);
			this.groupBox5.Location = new System.Drawing.Point(9, 243);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(326, 283);
			this.groupBox5.TabIndex = 257;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Service Acknowledgement";
			// 
			// textCountryCode
			// 
			this.textCountryCode.Enabled = false;
			this.textCountryCode.Location = new System.Drawing.Point(100, 44);
			this.textCountryCode.Name = "textCountryCode";
			this.textCountryCode.Size = new System.Drawing.Size(38, 20);
			this.textCountryCode.TabIndex = 261;
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(10, 47);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(89, 14);
			this.label30.TabIndex = 260;
			this.label30.Text = "Country Code";
			this.label30.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(6, 67);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(314, 134);
			this.label29.TabIndex = 56;
			this.label29.Text = resources.GetString("label29.Text");
			// 
			// checkSmsAgree
			// 
			this.checkSmsAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkSmsAgree.AutoSize = true;
			this.checkSmsAgree.Location = new System.Drawing.Point(9, 204);
			this.checkSmsAgree.Name = "checkSmsAgree";
			this.checkSmsAgree.Size = new System.Drawing.Size(271, 17);
			this.checkSmsAgree.TabIndex = 250;
			this.checkSmsAgree.Text = "I understand and acknowledge the terms of service.";
			this.checkSmsAgree.UseVisualStyleBackColor = true;
			this.checkSmsAgree.CheckedChanged += new System.EventHandler(this.checkSmsAgree_CheckedChanged);
			// 
			// comboClinicSms
			// 
			this.comboClinicSms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinicSms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinicSms.Location = new System.Drawing.Point(100, 19);
			this.comboClinicSms.MaxDropDownItems = 30;
			this.comboClinicSms.Name = "comboClinicSms";
			this.comboClinicSms.Size = new System.Drawing.Size(220, 21);
			this.comboClinicSms.TabIndex = 259;
			this.comboClinicSms.SelectedIndexChanged += new System.EventHandler(this.comboClinicSms_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(9, 22);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(89, 14);
			this.labelClinic.TabIndex = 258;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSmsLimit
			// 
			this.textSmsLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSmsLimit.Location = new System.Drawing.Point(9, 227);
			this.textSmsLimit.Name = "textSmsLimit";
			this.textSmsLimit.Size = new System.Drawing.Size(148, 20);
			this.textSmsLimit.TabIndex = 252;
			this.textSmsLimit.TextChanged += new System.EventHandler(this.textSmsLimit_TextChanged);
			this.textSmsLimit.Leave += new System.EventHandler(this.textSmsLimit_Leave);
			// 
			// butSmsUnsubscribe
			// 
			this.butSmsUnsubscribe.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSmsUnsubscribe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSmsUnsubscribe.Autosize = true;
			this.butSmsUnsubscribe.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSmsUnsubscribe.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSmsUnsubscribe.CornerRadius = 4F;
			this.butSmsUnsubscribe.Location = new System.Drawing.Point(9, 253);
			this.butSmsUnsubscribe.Name = "butSmsUnsubscribe";
			this.butSmsUnsubscribe.Size = new System.Drawing.Size(75, 23);
			this.butSmsUnsubscribe.TabIndex = 254;
			this.butSmsUnsubscribe.Text = "Unsubscribe";
			this.butSmsUnsubscribe.UseVisualStyleBackColor = true;
			this.butSmsUnsubscribe.Click += new System.EventHandler(this.butSmsUnsubscribe_Click);
			// 
			// butSmsCancel
			// 
			this.butSmsCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSmsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSmsCancel.Autosize = true;
			this.butSmsCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSmsCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSmsCancel.CornerRadius = 4F;
			this.butSmsCancel.Location = new System.Drawing.Point(245, 253);
			this.butSmsCancel.Name = "butSmsCancel";
			this.butSmsCancel.Size = new System.Drawing.Size(75, 23);
			this.butSmsCancel.TabIndex = 245;
			this.butSmsCancel.Text = "Cancel";
			this.butSmsCancel.UseVisualStyleBackColor = true;
			this.butSmsCancel.Click += new System.EventHandler(this.butSmsCancel_Click);
			// 
			// label28
			// 
			this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label28.Location = new System.Drawing.Point(160, 228);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(160, 17);
			this.label28.TabIndex = 253;
			this.label28.Text = "Monthly Limit in USD";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSmsSubmit
			// 
			this.butSmsSubmit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSmsSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSmsSubmit.Autosize = true;
			this.butSmsSubmit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSmsSubmit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSmsSubmit.CornerRadius = 4F;
			this.butSmsSubmit.Location = new System.Drawing.Point(165, 253);
			this.butSmsSubmit.Name = "butSmsSubmit";
			this.butSmsSubmit.Size = new System.Drawing.Size(75, 23);
			this.butSmsSubmit.TabIndex = 251;
			this.butSmsSubmit.Text = "Subcribe";
			this.butSmsSubmit.UseVisualStyleBackColor = true;
			this.butSmsSubmit.Click += new System.EventHandler(this.butSmsSubmit_Click);
			// 
			// gridSmsSummary
			// 
			this.gridSmsSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSmsSummary.HasMultilineHeaders = true;
			this.gridSmsSummary.HScrollVisible = false;
			this.gridSmsSummary.Location = new System.Drawing.Point(343, 6);
			this.gridSmsSummary.Name = "gridSmsSummary";
			this.gridSmsSummary.ScrollValue = 0;
			this.gridSmsSummary.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridSmsSummary.Size = new System.Drawing.Size(597, 471);
			this.gridSmsSummary.TabIndex = 252;
			this.gridSmsSummary.Title = "Text Messaging Phone Number and Usage Summary";
			this.gridSmsSummary.TranslationName = "FormEServicesSetup";
			this.gridSmsSummary.WrapText = false;
			// 
			// gridClinics
			// 
			this.gridClinics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridClinics.HasMultilineHeaders = false;
			this.gridClinics.HScrollVisible = false;
			this.gridClinics.Location = new System.Drawing.Point(13, 6);
			this.gridClinics.Name = "gridClinics";
			this.gridClinics.ScrollValue = 0;
			this.gridClinics.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridClinics.Size = new System.Drawing.Size(322, 231);
			this.gridClinics.TabIndex = 249;
			this.gridClinics.Title = "Subscription Information";
			this.gridClinics.TranslationName = "FormEServicesSetup";
			this.gridClinics.WrapText = false;
			this.gridClinics.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridClinics_CellClick);
			// 
			// butFwdMonth
			// 
			this.butFwdMonth.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butFwdMonth.Autosize = false;
			this.butFwdMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwdMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwdMonth.CornerRadius = 4F;
			this.butFwdMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butFwdMonth.Image = ((System.Drawing.Image)(resources.GetObject("butFwdMonth.Image")));
			this.butFwdMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butFwdMonth.Location = new System.Drawing.Point(698, 480);
			this.butFwdMonth.Name = "butFwdMonth";
			this.butFwdMonth.Size = new System.Drawing.Size(29, 22);
			this.butFwdMonth.TabIndex = 267;
			this.butFwdMonth.Text = "M";
			this.butFwdMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFwdMonth.Click += new System.EventHandler(this.butFwdMonth_Click);
			// 
			// butToday
			// 
			this.butToday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butToday.Autosize = false;
			this.butToday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butToday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butToday.CornerRadius = 4F;
			this.butToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butToday.Location = new System.Drawing.Point(614, 504);
			this.butToday.Name = "butToday";
			this.butToday.Size = new System.Drawing.Size(54, 22);
			this.butToday.TabIndex = 262;
			this.butToday.Text = "Today";
			this.butToday.Click += new System.EventHandler(this.butToday_Click);
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(10, 47);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(89, 14);
			this.label32.TabIndex = 260;
			this.label32.Text = "Country Code";
			this.label32.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label23
			// 
			this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label23.Location = new System.Drawing.Point(13, 9);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(949, 28);
			this.label23.TabIndex = 244;
			this.label23.Text = "eServices refer to Open Dental features that can be delivered electronically via " +
    "the internet.  All eServices hosted by Open Dental use the eConnector Service.";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 23);
			this.butClose.TabIndex = 500;
			this.butClose.Text = "Close";
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.dateTimePicker1);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.groupBox6);
			this.tabPage1.Controls.Add(this.odGrid1);
			this.tabPage1.Controls.Add(this.odGrid2);
			this.tabPage1.Controls.Add(this.button6);
			this.tabPage1.Controls.Add(this.button7);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(944, 588);
			this.tabPage1.TabIndex = 7;
			this.tabPage1.Text = "Texting Services";
			// 
			// button1
			// 
			this.button1.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Autosize = true;
			this.button1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button1.CornerRadius = 4F;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.Location = new System.Drawing.Point(1294, 965);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 22);
			this.button1.TabIndex = 268;
			this.button1.Text = "M";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker1.CustomFormat = "MMM yyyy";
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new System.Drawing.Point(1326, 966);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(113, 20);
			this.dateTimePicker1.TabIndex = 258;
			// 
			// button2
			// 
			this.button2.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Autosize = false;
			this.button2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button2.CornerRadius = 4F;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
			this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button2.Location = new System.Drawing.Point(1439, 965);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(29, 22);
			this.button2.TabIndex = 267;
			this.button2.Text = "M";
			this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox6.Controls.Add(this.textBox1);
			this.groupBox6.Controls.Add(this.label32);
			this.groupBox6.Controls.Add(this.label33);
			this.groupBox6.Controls.Add(this.checkBox1);
			this.groupBox6.Controls.Add(this.comboBox1);
			this.groupBox6.Controls.Add(this.label34);
			this.groupBox6.Controls.Add(this.textBox2);
			this.groupBox6.Controls.Add(this.button3);
			this.groupBox6.Controls.Add(this.button4);
			this.groupBox6.Controls.Add(this.label36);
			this.groupBox6.Controls.Add(this.button5);
			this.groupBox6.Location = new System.Drawing.Point(12, 728);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(326, 283);
			this.groupBox6.TabIndex = 257;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Service Acknowledgement";
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(100, 44);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(38, 20);
			this.textBox1.TabIndex = 261;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(6, 67);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(314, 134);
			this.label33.TabIndex = 56;
			this.label33.Text = resources.GetString("label33.Text");
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(9, 387);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(271, 17);
			this.checkBox1.TabIndex = 250;
			this.checkBox1.Text = "I understand and acknowledge the terms of service.";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Location = new System.Drawing.Point(100, 19);
			this.comboBox1.MaxDropDownItems = 30;
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(346, 21);
			this.comboBox1.TabIndex = 259;
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(9, 22);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(89, 14);
			this.label34.TabIndex = 258;
			this.label34.Text = "Clinic";
			this.label34.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBox2.Location = new System.Drawing.Point(9, 410);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(148, 20);
			this.textBox2.TabIndex = 252;
			// 
			// button3
			// 
			this.button3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.Autosize = true;
			this.button3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button3.CornerRadius = 4F;
			this.button3.Location = new System.Drawing.Point(9, 436);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 254;
			this.button3.Text = "Unsubscribe";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.Autosize = true;
			this.button4.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button4.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button4.CornerRadius = 4F;
			this.button4.Location = new System.Drawing.Point(371, 436);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 245;
			this.button4.Text = "Cancel";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// label36
			// 
			this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label36.Location = new System.Drawing.Point(160, 411);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(286, 17);
			this.label36.TabIndex = 253;
			this.label36.Text = "Monthly Limit in USD";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button5
			// 
			this.button5.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button5.Autosize = true;
			this.button5.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button5.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button5.CornerRadius = 4F;
			this.button5.Location = new System.Drawing.Point(291, 436);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 251;
			this.button5.Text = "Subcribe";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// odGrid1
			// 
			this.odGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.odGrid1.HasMultilineHeaders = true;
			this.odGrid1.HScrollVisible = false;
			this.odGrid1.Location = new System.Drawing.Point(346, 9);
			this.odGrid1.Name = "odGrid1";
			this.odGrid1.ScrollValue = 0;
			this.odGrid1.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.odGrid1.Size = new System.Drawing.Size(1335, 953);
			this.odGrid1.TabIndex = 252;
			this.odGrid1.Title = "Text Messaging Phone Number and Usage Summary";
			this.odGrid1.TranslationName = "FormEServicesSetup";
			this.odGrid1.WrapText = false;
			// 
			// odGrid2
			// 
			this.odGrid2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.odGrid2.HasMultilineHeaders = false;
			this.odGrid2.HScrollVisible = false;
			this.odGrid2.Location = new System.Drawing.Point(16, 9);
			this.odGrid2.Name = "odGrid2";
			this.odGrid2.ScrollValue = 0;
			this.odGrid2.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.odGrid2.Size = new System.Drawing.Size(322, 713);
			this.odGrid2.TabIndex = 249;
			this.odGrid2.Title = "Subscription Information";
			this.odGrid2.TranslationName = "FormEServicesSetup";
			this.odGrid2.WrapText = false;
			// 
			// button6
			// 
			this.button6.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button6.Autosize = false;
			this.button6.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button6.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button6.CornerRadius = 4F;
			this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button6.Location = new System.Drawing.Point(1355, 989);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(54, 22);
			this.button6.TabIndex = 262;
			this.button6.Text = "Today";
			// 
			// button7
			// 
			this.button7.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button7.Autosize = true;
			this.button7.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button7.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button7.CornerRadius = 4F;
			this.button7.Location = new System.Drawing.Point(237, 1017);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(95, 23);
			this.button7.TabIndex = 245;
			this.button7.Text = "Create Test Data";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Visible = false;
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(0, 0);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(100, 23);
			this.label37.TabIndex = 0;
			// 
			// FormEServicesSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 692);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.tabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormEServicesSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "eServices Setup";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPatientPortalSetup_FormClosed);
			this.Load += new System.EventHandler(this.FormEServicesSetup_Load);
			this.groupBoxNotification.ResumeLayout(false);
			this.groupBoxNotification.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabListenerService.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabMobileOld.ResumeLayout(false);
			this.groupPreferences.ResumeLayout(false);
			this.groupPreferences.PerformLayout();
			this.tabMobileNew.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPatientPortal.ResumeLayout(false);
			this.tabPatientPortal.PerformLayout();
			this.tabWebSched.ResumeLayout(false);
			this.groupWebSchedPreview.ResumeLayout(false);
			this.groupWebSchedPreview.PerformLayout();
			this.groupRecallSetup.ResumeLayout(false);
			this.tabSmsServices.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textOpenDentalUrlPatientPortal;
		private System.Windows.Forms.TextBox textBoxNotificationSubject;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxNotificationBody;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBoxNotification;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textRedirectUrlPatientPortal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private ValidNum textListenerPort;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabMobileNew;
		private System.Windows.Forms.TabPage tabPatientPortal;
		private UI.Button butClose;
		private UI.Button butGetUrlPatientPortal;
		private UI.Button butSavePatientPortal;
		private System.Windows.Forms.TabPage tabMobileOld;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.GroupBox groupBox2;
		private UI.Button butGetUrlMobileWeb;
		private System.Windows.Forms.TextBox textOpenDentalUrlMobileWeb;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label12;
		private UI.Button butSaveListenerPort;
		private System.Windows.Forms.CheckBox checkTroubleshooting;
		private UI.Button butDelete;
		private System.Windows.Forms.Label textDateTimeLastRun;
		private System.Windows.Forms.GroupBox groupPreferences;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textMobileUserName;
		private System.Windows.Forms.Label label15;
		private UI.Button butCurrentWorkstation;
		private System.Windows.Forms.TextBox textMobilePassword;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TextBox textMobileSynchWorkStation;
		private ValidNumber textSynchMinutes;
		private System.Windows.Forms.Label label18;
		private UI.Button butSaveMobileSynch;
		private ValidDate textDateBefore;
		private System.Windows.Forms.Label labelMobileSynchURL;
		private System.Windows.Forms.TextBox textMobileSyncServerURL;
		private System.Windows.Forms.Label labelMinutesBetweenSynch;
		private System.Windows.Forms.Label label19;
		private UI.Button butFullSync;
		private UI.Button butSync;
		private System.Windows.Forms.TabPage tabWebSched;
		private System.Windows.Forms.Label labelWebSchedDesc;
		private UI.Button butRecallSchedSetup;
		private System.Windows.Forms.Label labelWebSchedEnable;
		private UI.Button butWebSchedEnable;
		private System.Windows.Forms.GroupBox groupRecallSetup;
		private System.Windows.Forms.Label label20;
		private UI.Button butSignUp;
		private System.Windows.Forms.TabPage tabListenerService;
		private System.Windows.Forms.GroupBox groupBox3;
		private UI.Button butStartListenerService;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label labelListenerStatus;
		private UI.Button butListenerAlertsOff;
		private System.Windows.Forms.TextBox textListenerServiceStatus;
		private System.Windows.Forms.Label label23;
		private UI.ODGrid gridListenerServiceStatusHistory;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private UI.Button butListenerServiceHistoryRefresh;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label labelListenerServiceAck;
		private UI.Button butListenerServiceAck;
		private System.Windows.Forms.TabPage tabSmsServices;
		private UI.ODGrid gridSmsSummary;
		private UI.Button butSmsSubmit;
		private System.Windows.Forms.Label label28;
		private UI.Button butSmsCancel;
		private System.Windows.Forms.TextBox textSmsLimit;
		private System.Windows.Forms.CheckBox checkSmsAgree;
		private UI.ODGrid gridClinics;
		private System.Windows.Forms.Label label29;
		private UI.Button butSmsUnsubscribe;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ComboBox comboClinicSms;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.TextBox textCountryCode;
		private System.Windows.Forms.DateTimePicker dateTimePickerSms;
		private UI.Button butBackMonth;
		private UI.Button butFwdMonth;
		private UI.Button butToday;
		private UI.ODGrid gridWebSchedRecallTypes;
		private System.Windows.Forms.Label label35;
		private UI.ODGrid gridWebSchedTimeSlots;
		private System.Windows.Forms.GroupBox groupWebSchedPreview;
		private System.Windows.Forms.Label labelWebSchedClinic;
		private System.Windows.Forms.Label labelWebSchedRecallTypes;
		private System.Windows.Forms.ComboBox comboWebSchedClinic;
		private System.Windows.Forms.ComboBox comboWebSchedRecallTypes;
		private System.Windows.Forms.Label label32;
		private ValidDate textWebSchedDateStart;
		private UI.Button butWebSchedToday;
		private UI.ODGrid gridWebSchedOperatories;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.ListBox listBoxWebSchedProviderPref;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.ComboBox comboWebSchedProviders;
		private UI.Button butWebSchedPickClinic;
		private UI.Button butWebSchedPickProv;
		private System.Windows.Forms.TabPage tabPage1;
		private UI.Button button1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private UI.Button button2;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.TextBox textBox2;
		private UI.Button button3;
		private UI.Button button4;
		private System.Windows.Forms.Label label36;
		private UI.Button button5;
		private UI.ODGrid odGrid1;
		private UI.ODGrid odGrid2;
		private UI.Button button6;
		private UI.Button button7;
		private UI.Button butInstallEConnector;
		private System.Windows.Forms.CheckBox checkAllowEConnectorComm;

	}
}