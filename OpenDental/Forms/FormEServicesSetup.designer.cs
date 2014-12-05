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
			this.tabRecallScheduler = new System.Windows.Forms.TabPage();
			this.butRecallSchedEnable = new OpenDental.UI.Button();
			this.labelRecallSchedEnable = new System.Windows.Forms.Label();
			this.butRecallSchedSetup = new OpenDental.UI.Button();
			this.labelRecallSchedDesc = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.butSaveListenerPort = new OpenDental.UI.Button();
			this.textListenerPort = new OpenDental.ValidNum();
			this.butClose = new OpenDental.UI.Button();
			this.groupBoxNotification.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabPatientPortal.SuspendLayout();
			this.tabMobileNew.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabMobileOld.SuspendLayout();
			this.groupPreferences.SuspendLayout();
			this.tabRecallScheduler.SuspendLayout();
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
			this.tabControl.Controls.Add(this.tabPatientPortal);
			this.tabControl.Controls.Add(this.tabMobileNew);
			this.tabControl.Controls.Add(this.tabMobileOld);
			this.tabControl.Controls.Add(this.tabRecallScheduler);
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
			// tabRecallScheduler
			// 
			this.tabRecallScheduler.BackColor = System.Drawing.SystemColors.Control;
			this.tabRecallScheduler.Controls.Add(this.butRecallSchedEnable);
			this.tabRecallScheduler.Controls.Add(this.labelRecallSchedEnable);
			this.tabRecallScheduler.Controls.Add(this.butRecallSchedSetup);
			this.tabRecallScheduler.Controls.Add(this.labelRecallSchedDesc);
			this.tabRecallScheduler.Location = new System.Drawing.Point(4, 22);
			this.tabRecallScheduler.Name = "tabRecallScheduler";
			this.tabRecallScheduler.Size = new System.Drawing.Size(944, 588);
			this.tabRecallScheduler.TabIndex = 3;
			this.tabRecallScheduler.Text = "Recall Scheduler";
			// 
			// butRecallSchedEnable
			// 
			this.butRecallSchedEnable.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallSchedEnable.Autosize = true;
			this.butRecallSchedEnable.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallSchedEnable.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallSchedEnable.CornerRadius = 4F;
			this.butRecallSchedEnable.Location = new System.Drawing.Point(41, 80);
			this.butRecallSchedEnable.Name = "butRecallSchedEnable";
			this.butRecallSchedEnable.Size = new System.Drawing.Size(73, 24);
			this.butRecallSchedEnable.TabIndex = 246;
			this.butRecallSchedEnable.Text = "Enable";
			this.butRecallSchedEnable.Click += new System.EventHandler(this.butRecallSchedEnable_Click);
			// 
			// labelRecallSchedEnable
			// 
			this.labelRecallSchedEnable.Location = new System.Drawing.Point(38, 107);
			this.labelRecallSchedEnable.Name = "labelRecallSchedEnable";
			this.labelRecallSchedEnable.Size = new System.Drawing.Size(869, 52);
			this.labelRecallSchedEnable.TabIndex = 245;
			this.labelRecallSchedEnable.Text = "labelRecallSchedEnable";
			// 
			// butRecallSchedSetup
			// 
			this.butRecallSchedSetup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallSchedSetup.Autosize = true;
			this.butRecallSchedSetup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallSchedSetup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallSchedSetup.CornerRadius = 4F;
			this.butRecallSchedSetup.Location = new System.Drawing.Point(41, 162);
			this.butRecallSchedSetup.Name = "butRecallSchedSetup";
			this.butRecallSchedSetup.Size = new System.Drawing.Size(103, 24);
			this.butRecallSchedSetup.TabIndex = 243;
			this.butRecallSchedSetup.Text = "Recall Setup";
			this.butRecallSchedSetup.Click += new System.EventHandler(this.butRecallSchedSetup_Click);
			// 
			// labelRecallSchedDesc
			// 
			this.labelRecallSchedDesc.Location = new System.Drawing.Point(38, 12);
			this.labelRecallSchedDesc.Name = "labelRecallSchedDesc";
			this.labelRecallSchedDesc.Size = new System.Drawing.Size(869, 49);
			this.labelRecallSchedDesc.TabIndex = 52;
			this.labelRecallSchedDesc.Text = "TODO: This is where a great and fantastic description of the recall scheduler ser" +
    "vice will go.";
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Location = new System.Drawing.Point(16, 7);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(948, 28);
			this.label11.TabIndex = 56;
			this.label11.Text = resources.GetString("label11.Text");
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(695, 36);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(86, 17);
			this.label10.TabIndex = 57;
			this.label10.Text = "Listener Port";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSaveListenerPort
			// 
			this.butSaveListenerPort.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveListenerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveListenerPort.Autosize = true;
			this.butSaveListenerPort.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveListenerPort.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveListenerPort.CornerRadius = 4F;
			this.butSaveListenerPort.Location = new System.Drawing.Point(894, 33);
			this.butSaveListenerPort.Name = "butSaveListenerPort";
			this.butSaveListenerPort.Size = new System.Drawing.Size(61, 24);
			this.butSaveListenerPort.TabIndex = 243;
			this.butSaveListenerPort.Text = "Save";
			this.butSaveListenerPort.Click += new System.EventHandler(this.butSaveListenerPort_Click);
			// 
			// textListenerPort
			// 
			this.textListenerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textListenerPort.Location = new System.Drawing.Point(787, 35);
			this.textListenerPort.MaxVal = 65535;
			this.textListenerPort.MinVal = 0;
			this.textListenerPort.Name = "textListenerPort";
			this.textListenerPort.Size = new System.Drawing.Size(100, 20);
			this.textListenerPort.TabIndex = 51;
			this.textListenerPort.Text = "0";
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
			// FormEServicesSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 692);
			this.Controls.Add(this.butSaveListenerPort);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.textListenerPort);
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
			this.tabRecallScheduler.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.TabPage tabRecallScheduler;
		private System.Windows.Forms.Label labelRecallSchedDesc;
		private UI.Button butRecallSchedSetup;
		private System.Windows.Forms.Label labelRecallSchedEnable;
		private UI.Button butRecallSchedEnable;

	}
}