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
			this.tabPatientPortal = new System.Windows.Forms.TabPage();
			this.butSavePatientPortal = new OpenDental.UI.Button();
			this.tabMobileNew = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butGetUrlMobileWeb = new OpenDental.UI.Button();
			this.textOpenDentalUrlMobileWeb = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
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
			this.tabWebSched = new System.Windows.Forms.TabPage();
			this.butSignUp = new OpenDental.UI.Button();
			this.groupRecallSetup = new System.Windows.Forms.GroupBox();
			this.label22 = new System.Windows.Forms.Label();
			this.butOperatories = new OpenDental.UI.Button();
			this.label21 = new System.Windows.Forms.Label();
			this.butRecallTypes = new OpenDental.UI.Button();
			this.label20 = new System.Windows.Forms.Label();
			this.labelRecallMessage = new System.Windows.Forms.Label();
			this.butRecallSchedSetup = new OpenDental.UI.Button();
			this.butWebSchedEnable = new OpenDental.UI.Button();
			this.labelWebSchedEnable = new System.Windows.Forms.Label();
			this.labelWebSchedDesc = new System.Windows.Forms.Label();
			this.tabListenerService = new System.Windows.Forms.TabPage();
			this.butClose = new OpenDental.UI.Button();
			this.label23 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textListenerPort = new OpenDental.ValidNum();
			this.label10 = new System.Windows.Forms.Label();
			this.butSaveListenerPort = new OpenDental.UI.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label27 = new System.Windows.Forms.Label();
			this.butListenerServiceHistoryRefresh = new OpenDental.UI.Button();
			this.label26 = new System.Windows.Forms.Label();
			this.gridListenerServiceStatusHistory = new OpenDental.UI.ODGrid();
			this.butStartListenerService = new OpenDental.UI.Button();
			this.label24 = new System.Windows.Forms.Label();
			this.labelListenerStatus = new System.Windows.Forms.Label();
			this.butListenerAlertsOff = new OpenDental.UI.Button();
			this.textListenerServiceStatus = new System.Windows.Forms.TextBox();
			this.groupBoxNotification.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabPatientPortal.SuspendLayout();
			this.tabMobileNew.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabMobileOld.SuspendLayout();
			this.groupPreferences.SuspendLayout();
			this.tabWebSched.SuspendLayout();
			this.groupRecallSetup.SuspendLayout();
			this.tabListenerService.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
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
			this.tabControl.Controls.Add(this.tabPatientPortal);
			this.tabControl.Controls.Add(this.tabMobileNew);
			this.tabControl.Controls.Add(this.tabMobileOld);
			this.tabControl.Controls.Add(this.tabWebSched);
			this.tabControl.Location = new System.Drawing.Point(12, 40);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(952, 614);
			this.tabControl.TabIndex = 53;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
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
			// tabWebSched
			// 
			this.tabWebSched.BackColor = System.Drawing.SystemColors.Control;
			this.tabWebSched.Controls.Add(this.butSignUp);
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
			this.butSignUp.Location = new System.Drawing.Point(742, 116);
			this.butSignUp.Name = "butSignUp";
			this.butSignUp.Size = new System.Drawing.Size(103, 24);
			this.butSignUp.TabIndex = 248;
			this.butSignUp.Text = "Sign Up";
			this.butSignUp.Click += new System.EventHandler(this.butSignUp_Click);
			// 
			// groupRecallSetup
			// 
			this.groupRecallSetup.Controls.Add(this.label22);
			this.groupRecallSetup.Controls.Add(this.butOperatories);
			this.groupRecallSetup.Controls.Add(this.label21);
			this.groupRecallSetup.Controls.Add(this.butRecallTypes);
			this.groupRecallSetup.Controls.Add(this.label20);
			this.groupRecallSetup.Controls.Add(this.labelRecallMessage);
			this.groupRecallSetup.Controls.Add(this.butRecallSchedSetup);
			this.groupRecallSetup.Location = new System.Drawing.Point(93, 187);
			this.groupRecallSetup.Name = "groupRecallSetup";
			this.groupRecallSetup.Size = new System.Drawing.Size(758, 215);
			this.groupRecallSetup.TabIndex = 247;
			this.groupRecallSetup.TabStop = false;
			this.groupRecallSetup.Text = "Web Sched Settings";
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(120, 173);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(632, 18);
			this.label22.TabIndex = 251;
			this.label22.Text = "Operatories must be set up correctly in order for the patient to be able to see o" +
    "penings in your schedule.";
			// 
			// butOperatories
			// 
			this.butOperatories.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOperatories.Autosize = true;
			this.butOperatories.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOperatories.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOperatories.CornerRadius = 4F;
			this.butOperatories.Location = new System.Drawing.Point(9, 167);
			this.butOperatories.Name = "butOperatories";
			this.butOperatories.Size = new System.Drawing.Size(103, 24);
			this.butOperatories.TabIndex = 250;
			this.butOperatories.Text = "Operatories";
			this.butOperatories.Click += new System.EventHandler(this.butOperatories_Click);
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(119, 123);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(632, 29);
			this.label21.TabIndex = 249;
			this.label21.Text = "Recall Types is used to determine the length of the recall appointments that will" +
    " be scheduled by the patient.  One hour maximum.";
			// 
			// butRecallTypes
			// 
			this.butRecallTypes.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallTypes.Autosize = true;
			this.butRecallTypes.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallTypes.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallTypes.CornerRadius = 4F;
			this.butRecallTypes.Location = new System.Drawing.Point(8, 117);
			this.butRecallTypes.Name = "butRecallTypes";
			this.butRecallTypes.Size = new System.Drawing.Size(103, 24);
			this.butRecallTypes.TabIndex = 248;
			this.butRecallTypes.Text = "Recall Types";
			this.butRecallTypes.Click += new System.EventHandler(this.butRecallTypes_Click);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(120, 73);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(632, 18);
			this.label20.TabIndex = 247;
			this.label20.Text = "Recall Setup is used to customize the notification messages that will be sent to " +
    "the patient.";
			// 
			// labelRecallMessage
			// 
			this.labelRecallMessage.Location = new System.Drawing.Point(6, 21);
			this.labelRecallMessage.Name = "labelRecallMessage";
			this.labelRecallMessage.Size = new System.Drawing.Size(746, 43);
			this.labelRecallMessage.TabIndex = 246;
			this.labelRecallMessage.Text = resources.GetString("labelRecallMessage.Text");
			// 
			// butRecallSchedSetup
			// 
			this.butRecallSchedSetup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallSchedSetup.Autosize = true;
			this.butRecallSchedSetup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallSchedSetup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallSchedSetup.CornerRadius = 4F;
			this.butRecallSchedSetup.Location = new System.Drawing.Point(9, 67);
			this.butRecallSchedSetup.Name = "butRecallSchedSetup";
			this.butRecallSchedSetup.Size = new System.Drawing.Size(103, 24);
			this.butRecallSchedSetup.TabIndex = 243;
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
			this.butWebSchedEnable.Location = new System.Drawing.Point(102, 116);
			this.butWebSchedEnable.Name = "butWebSchedEnable";
			this.butWebSchedEnable.Size = new System.Drawing.Size(102, 24);
			this.butWebSchedEnable.TabIndex = 246;
			this.butWebSchedEnable.Text = "Enable";
			this.butWebSchedEnable.Click += new System.EventHandler(this.butWebSchedEnable_Click);
			// 
			// labelWebSchedEnable
			// 
			this.labelWebSchedEnable.Location = new System.Drawing.Point(93, 143);
			this.labelWebSchedEnable.Name = "labelWebSchedEnable";
			this.labelWebSchedEnable.Size = new System.Drawing.Size(758, 41);
			this.labelWebSchedEnable.TabIndex = 245;
			this.labelWebSchedEnable.Text = "labelWebSchedEnable";
			// 
			// labelWebSchedDesc
			// 
			this.labelWebSchedDesc.Location = new System.Drawing.Point(93, 12);
			this.labelWebSchedDesc.Name = "labelWebSchedDesc";
			this.labelWebSchedDesc.Size = new System.Drawing.Size(758, 101);
			this.labelWebSchedDesc.TabIndex = 52;
			this.labelWebSchedDesc.Text = resources.GetString("labelWebSchedDesc.Text");
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
			this.tabListenerService.Text = "Listener Service";
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
			this.butClose.TabIndex = 53;
			this.butClose.Text = "Close";
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
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
    "the Internet.  All eServices hosted by Open Dental use the Listener Service.";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.textListenerPort);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.butSaveListenerPort);
			this.groupBox4.Location = new System.Drawing.Point(117, 451);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(711, 133);
			this.groupBox4.TabIndex = 255;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Listener Service Settings";
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Location = new System.Drawing.Point(7, 18);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(698, 35);
			this.label11.TabIndex = 56;
			this.label11.Text = "The Listener Port is the same for all eServices hosted by Open Dental and must be" +
    " forwarded by your router to the computer that is running the OpenDentCustListen" +
    "er service.";
			// 
			// textListenerPort
			// 
			this.textListenerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textListenerPort.Location = new System.Drawing.Point(311, 56);
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
			this.label10.Location = new System.Drawing.Point(120, 57);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(185, 17);
			this.label10.TabIndex = 57;
			this.label10.Text = "Listener Port";
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
			this.butSaveListenerPort.Location = new System.Drawing.Point(323, 100);
			this.butSaveListenerPort.Name = "butSaveListenerPort";
			this.butSaveListenerPort.Size = new System.Drawing.Size(61, 24);
			this.butSaveListenerPort.TabIndex = 243;
			this.butSaveListenerPort.Text = "Save";
			this.butSaveListenerPort.Click += new System.EventHandler(this.butSaveListenerPort_Click);
			// 
			// label25
			// 
			this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label25.Location = new System.Drawing.Point(123, 4);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(687, 85);
			this.label25.TabIndex = 254;
			this.label25.Text = resources.GetString("label25.Text");
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label27);
			this.groupBox3.Controls.Add(this.butListenerServiceHistoryRefresh);
			this.groupBox3.Controls.Add(this.label26);
			this.groupBox3.Controls.Add(this.gridListenerServiceStatusHistory);
			this.groupBox3.Controls.Add(this.butStartListenerService);
			this.groupBox3.Controls.Add(this.label24);
			this.groupBox3.Controls.Add(this.labelListenerStatus);
			this.groupBox3.Controls.Add(this.butListenerAlertsOff);
			this.groupBox3.Controls.Add(this.textListenerServiceStatus);
			this.groupBox3.Location = new System.Drawing.Point(117, 113);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(711, 318);
			this.groupBox3.TabIndex = 253;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Listener Service Monitor";
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(7, 18);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(684, 19);
			this.label27.TabIndex = 252;
			this.label27.Text = "Open Dental monitors the status of the Listener Service and alerts all workstatio" +
    "ns when status is critical.";
			// 
			// butListenerServiceHistoryRefresh
			// 
			this.butListenerServiceHistoryRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListenerServiceHistoryRefresh.Autosize = true;
			this.butListenerServiceHistoryRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListenerServiceHistoryRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListenerServiceHistoryRefresh.CornerRadius = 4F;
			this.butListenerServiceHistoryRefresh.Location = new System.Drawing.Point(644, 87);
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
			this.label26.Size = new System.Drawing.Size(620, 37);
			this.label26.TabIndex = 250;
			this.label26.Text = resources.GetString("label26.Text");
			this.label26.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridListenerServiceStatusHistory
			// 
			this.gridListenerServiceStatusHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridListenerServiceStatusHistory.HScrollVisible = false;
			this.gridListenerServiceStatusHistory.Location = new System.Drawing.Point(6, 117);
			this.gridListenerServiceStatusHistory.Name = "gridListenerServiceStatusHistory";
			this.gridListenerServiceStatusHistory.ScrollValue = 0;
			this.gridListenerServiceStatusHistory.Size = new System.Drawing.Size(699, 138);
			this.gridListenerServiceStatusHistory.TabIndex = 249;
			this.gridListenerServiceStatusHistory.Title = "Listener Service History";
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
			this.butStartListenerService.Location = new System.Drawing.Point(417, 45);
			this.butStartListenerService.Name = "butStartListenerService";
			this.butStartListenerService.Size = new System.Drawing.Size(61, 24);
			this.butStartListenerService.TabIndex = 245;
			this.butStartListenerService.Text = "Start";
			this.butStartListenerService.Click += new System.EventHandler(this.butStartListenerService_Click);
			// 
			// label24
			// 
			this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label24.Location = new System.Drawing.Point(115, 285);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(578, 29);
			this.label24.TabIndex = 248;
			this.label24.Text = "Before you stop monitoring, first uninstall the Listener Service.\r\nMonitoring wil" +
    "l automatically resume when an active Listener Service has been detected.";
			// 
			// labelListenerStatus
			// 
			this.labelListenerStatus.Location = new System.Drawing.Point(67, 48);
			this.labelListenerStatus.Name = "labelListenerStatus";
			this.labelListenerStatus.Size = new System.Drawing.Size(238, 17);
			this.labelListenerStatus.TabIndex = 244;
			this.labelListenerStatus.Text = "Current Listener Service Status";
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
			this.butListenerAlertsOff.Location = new System.Drawing.Point(9, 286);
			this.butListenerAlertsOff.Name = "butListenerAlertsOff";
			this.butListenerAlertsOff.Size = new System.Drawing.Size(100, 24);
			this.butListenerAlertsOff.TabIndex = 247;
			this.butListenerAlertsOff.Text = "Stop Monitoring";
			this.butListenerAlertsOff.Click += new System.EventHandler(this.butListenerAlertsOff_Click);
			// 
			// textListenerServiceStatus
			// 
			this.textListenerServiceStatus.Location = new System.Drawing.Point(311, 47);
			this.textListenerServiceStatus.Name = "textListenerServiceStatus";
			this.textListenerServiceStatus.ReadOnly = true;
			this.textListenerServiceStatus.Size = new System.Drawing.Size(100, 20);
			this.textListenerServiceStatus.TabIndex = 246;
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
			this.Load += new System.EventHandler(this.FormPatientPortalSetup_Load);
			this.groupBoxNotification.ResumeLayout(false);
			this.groupBoxNotification.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabPatientPortal.ResumeLayout(false);
			this.tabPatientPortal.PerformLayout();
			this.tabMobileNew.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabMobileOld.ResumeLayout(false);
			this.groupPreferences.ResumeLayout(false);
			this.groupPreferences.PerformLayout();
			this.tabWebSched.ResumeLayout(false);
			this.groupRecallSetup.ResumeLayout(false);
			this.tabListenerService.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
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
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabMobileNew;
		private System.Windows.Forms.TabPage tabPatientPortal;
		private UI.Button butClose;
		private UI.Button butGetUrlPatientPortal;
		private UI.Button butSavePatientPortal;
		private System.Windows.Forms.TabPage tabMobileOld;
		private System.Windows.Forms.GroupBox groupBox2;
		private UI.Button butGetUrlMobileWeb;
		private System.Windows.Forms.TextBox textOpenDentalUrlMobileWeb;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label12;
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
		private System.Windows.Forms.Label label22;
		private UI.Button butOperatories;
		private System.Windows.Forms.Label label21;
		private UI.Button butRecallTypes;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label labelRecallMessage;
		private UI.Button butSignUp;
		private System.Windows.Forms.TabPage tabListenerService;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label11;
		private ValidNum textListenerPort;
		private System.Windows.Forms.Label label10;
		private UI.Button butSaveListenerPort;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label27;
		private UI.Button butListenerServiceHistoryRefresh;
		private System.Windows.Forms.Label label26;
		private UI.ODGrid gridListenerServiceStatusHistory;
		private UI.Button butStartListenerService;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label labelListenerStatus;
		private UI.Button butListenerAlertsOff;
		private System.Windows.Forms.TextBox textListenerServiceStatus;

	}
}