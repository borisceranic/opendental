namespace OpenDental{
	partial class FormPodiumSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPodiumSetup));
			this.checkHideButtons = new System.Windows.Forms.CheckBox();
			this.textPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textApptSetComplete = new OpenDental.ValidDouble();
			this.label1 = new System.Windows.Forms.Label();
			this.textApptTimeArrived = new OpenDental.ValidDouble();
			this.textApptTimeDismissed = new OpenDental.ValidDouble();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioTimeDismissedExistingPat = new System.Windows.Forms.RadioButton();
			this.radioTimeArrivedExistingPat = new System.Windows.Forms.RadioButton();
			this.radioSetCompleteExistingPat = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioTimeDismissedNewPat = new System.Windows.Forms.RadioButton();
			this.radioTimeArrivedNewPat = new System.Windows.Forms.RadioButton();
			this.radioSetCompleteNewPat = new System.Windows.Forms.RadioButton();
			this.textCompNameOrIP = new System.Windows.Forms.TextBox();
			this.textAPIToken = new System.Windows.Forms.TextBox();
			this.textLocationID = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.checkUseEConnector = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkHideButtons
			// 
			this.checkHideButtons.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideButtons.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideButtons.Location = new System.Drawing.Point(240, 13);
			this.checkHideButtons.Name = "checkHideButtons";
			this.checkHideButtons.Size = new System.Drawing.Size(291, 18);
			this.checkHideButtons.TabIndex = 93;
			this.checkHideButtons.Text = "Hide button in Standard Reports";
			this.checkHideButtons.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPath
			// 
			this.textPath.Location = new System.Drawing.Point(221, 34);
			this.textPath.Name = "textPath";
			this.textPath.Size = new System.Drawing.Size(312, 20);
			this.textPath.TabIndex = 81;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(25, 36);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(194, 18);
			this.label3.TabIndex = 80;
			this.label3.Text = "Path of file to open";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEnabled
			// 
			this.checkEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnabled.Location = new System.Drawing.Point(22, 13);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(212, 18);
			this.checkEnabled.TabIndex = 75;
			this.checkEnabled.Text = "Enabled";
			this.checkEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textApptSetComplete);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textApptTimeArrived);
			this.groupBox1.Controls.Add(this.textApptTimeDismissed);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(221, 281);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 103);
			this.groupBox1.TabIndex = 94;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trigger Rules";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(56, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(238, 16);
			this.label2.TabIndex = 110;
			this.label2.Text = "minutes after patient dismissed.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textApptSetComplete
			// 
			this.textApptSetComplete.Location = new System.Drawing.Point(18, 21);
			this.textApptSetComplete.MaxVal = 5000D;
			this.textApptSetComplete.MinVal = 5D;
			this.textApptSetComplete.Name = "textApptSetComplete";
			this.textApptSetComplete.Size = new System.Drawing.Size(37, 20);
			this.textApptSetComplete.TabIndex = 106;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(238, 16);
			this.label1.TabIndex = 109;
			this.label1.Text = "minutes after patient arrived.\r\n";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textApptTimeArrived
			// 
			this.textApptTimeArrived.Location = new System.Drawing.Point(18, 47);
			this.textApptTimeArrived.MaxVal = 5000D;
			this.textApptTimeArrived.MinVal = 5D;
			this.textApptTimeArrived.Name = "textApptTimeArrived";
			this.textApptTimeArrived.Size = new System.Drawing.Size(37, 20);
			this.textApptTimeArrived.TabIndex = 107;
			// 
			// textApptTimeDismissed
			// 
			this.textApptTimeDismissed.Location = new System.Drawing.Point(18, 73);
			this.textApptTimeDismissed.MaxVal = 5000D;
			this.textApptTimeDismissed.MinVal = 5D;
			this.textApptTimeDismissed.Name = "textApptTimeDismissed";
			this.textApptTimeDismissed.Size = new System.Drawing.Size(37, 20);
			this.textApptTimeDismissed.TabIndex = 108;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(56, 21);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(238, 16);
			this.label4.TabIndex = 97;
			this.label4.Text = "minutes after appointment completed.\r\n";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioTimeDismissedExistingPat);
			this.groupBox2.Controls.Add(this.radioTimeArrivedExistingPat);
			this.groupBox2.Controls.Add(this.radioSetCompleteExistingPat);
			this.groupBox2.Location = new System.Drawing.Point(221, 189);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(158, 86);
			this.groupBox2.TabIndex = 95;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Existing Patient Trigger";
			// 
			// radioTimeDismissedExistingPat
			// 
			this.radioTimeDismissedExistingPat.Location = new System.Drawing.Point(21, 61);
			this.radioTimeDismissedExistingPat.Name = "radioTimeDismissedExistingPat";
			this.radioTimeDismissedExistingPat.Size = new System.Drawing.Size(117, 17);
			this.radioTimeDismissedExistingPat.TabIndex = 2;
			this.radioTimeDismissedExistingPat.TabStop = true;
			this.radioTimeDismissedExistingPat.Text = "Time Dismissed";
			this.radioTimeDismissedExistingPat.UseVisualStyleBackColor = true;
			this.radioTimeDismissedExistingPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// radioTimeArrivedExistingPat
			// 
			this.radioTimeArrivedExistingPat.Location = new System.Drawing.Point(21, 40);
			this.radioTimeArrivedExistingPat.Name = "radioTimeArrivedExistingPat";
			this.radioTimeArrivedExistingPat.Size = new System.Drawing.Size(117, 17);
			this.radioTimeArrivedExistingPat.TabIndex = 1;
			this.radioTimeArrivedExistingPat.TabStop = true;
			this.radioTimeArrivedExistingPat.Text = "Time Arrived";
			this.radioTimeArrivedExistingPat.UseVisualStyleBackColor = true;
			this.radioTimeArrivedExistingPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// radioSetCompleteExistingPat
			// 
			this.radioSetCompleteExistingPat.Location = new System.Drawing.Point(21, 19);
			this.radioSetCompleteExistingPat.Name = "radioSetCompleteExistingPat";
			this.radioSetCompleteExistingPat.Size = new System.Drawing.Size(117, 17);
			this.radioSetCompleteExistingPat.TabIndex = 0;
			this.radioSetCompleteExistingPat.TabStop = true;
			this.radioSetCompleteExistingPat.Text = "Set complete";
			this.radioSetCompleteExistingPat.UseVisualStyleBackColor = true;
			this.radioSetCompleteExistingPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioTimeDismissedNewPat);
			this.groupBox3.Controls.Add(this.radioTimeArrivedNewPat);
			this.groupBox3.Controls.Add(this.radioSetCompleteNewPat);
			this.groupBox3.Location = new System.Drawing.Point(385, 189);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(148, 86);
			this.groupBox3.TabIndex = 96;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "New Patient Trigger";
			// 
			// radioTimeDismissedNewPat
			// 
			this.radioTimeDismissedNewPat.Location = new System.Drawing.Point(17, 61);
			this.radioTimeDismissedNewPat.Name = "radioTimeDismissedNewPat";
			this.radioTimeDismissedNewPat.Size = new System.Drawing.Size(121, 17);
			this.radioTimeDismissedNewPat.TabIndex = 5;
			this.radioTimeDismissedNewPat.TabStop = true;
			this.radioTimeDismissedNewPat.Text = "Time Dismissed";
			this.radioTimeDismissedNewPat.UseVisualStyleBackColor = true;
			this.radioTimeDismissedNewPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// radioTimeArrivedNewPat
			// 
			this.radioTimeArrivedNewPat.Location = new System.Drawing.Point(17, 40);
			this.radioTimeArrivedNewPat.Name = "radioTimeArrivedNewPat";
			this.radioTimeArrivedNewPat.Size = new System.Drawing.Size(121, 17);
			this.radioTimeArrivedNewPat.TabIndex = 4;
			this.radioTimeArrivedNewPat.TabStop = true;
			this.radioTimeArrivedNewPat.Text = "Time Arrived";
			this.radioTimeArrivedNewPat.UseVisualStyleBackColor = true;
			this.radioTimeArrivedNewPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// radioSetCompleteNewPat
			// 
			this.radioSetCompleteNewPat.Location = new System.Drawing.Point(17, 19);
			this.radioSetCompleteNewPat.Name = "radioSetCompleteNewPat";
			this.radioSetCompleteNewPat.Size = new System.Drawing.Size(121, 17);
			this.radioSetCompleteNewPat.TabIndex = 3;
			this.radioSetCompleteNewPat.TabStop = true;
			this.radioSetCompleteNewPat.Text = "Set complete";
			this.radioSetCompleteNewPat.UseVisualStyleBackColor = true;
			this.radioSetCompleteNewPat.CheckedChanged += new System.EventHandler(this.RadioButton_CheckChanged);
			// 
			// textCompNameOrIP
			// 
			this.textCompNameOrIP.Location = new System.Drawing.Point(221, 60);
			this.textCompNameOrIP.Name = "textCompNameOrIP";
			this.textCompNameOrIP.Size = new System.Drawing.Size(312, 20);
			this.textCompNameOrIP.TabIndex = 97;
			// 
			// textAPIToken
			// 
			this.textAPIToken.Location = new System.Drawing.Point(221, 86);
			this.textAPIToken.Name = "textAPIToken";
			this.textAPIToken.Size = new System.Drawing.Size(312, 20);
			this.textAPIToken.TabIndex = 98;
			// 
			// textLocationID
			// 
			this.textLocationID.Location = new System.Drawing.Point(221, 112);
			this.textLocationID.Name = "textLocationID";
			this.textLocationID.Size = new System.Drawing.Size(312, 20);
			this.textLocationID.TabIndex = 99;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(25, 61);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(194, 18);
			this.label7.TabIndex = 100;
			this.label7.Text = "Computer name or IP (required)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(25, 87);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(194, 18);
			this.label8.TabIndex = 101;
			this.label8.Text = "API Token (required)";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(25, 113);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(194, 18);
			this.label9.TabIndex = 102;
			this.label9.Text = "Location ID (required)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(25, 161);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(194, 18);
			this.labelClinic.TabIndex = 103;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(221, 160);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(158, 21);
			this.comboClinic.TabIndex = 104;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// checkUseEConnector
			// 
			this.checkUseEConnector.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseEConnector.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseEConnector.Location = new System.Drawing.Point(22, 138);
			this.checkUseEConnector.Name = "checkUseEConnector";
			this.checkUseEConnector.Size = new System.Drawing.Size(212, 18);
			this.checkUseEConnector.TabIndex = 105;
			this.checkUseEConnector.Text = "Use eConnector to send invitations";
			this.checkUseEConnector.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(550, 328);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(550, 358);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormPodiumSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(637, 394);
			this.Controls.Add(this.checkUseEConnector);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textLocationID);
			this.Controls.Add(this.textAPIToken);
			this.Controls.Add(this.textCompNameOrIP);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkHideButtons);
			this.Controls.Add(this.textPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(653, 433);
			this.Name = "FormPodiumSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Podium Setup";
			this.Load += new System.EventHandler(this.FormPodiumSetup_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkHideButtons;
		private System.Windows.Forms.TextBox textPath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkEnabled;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioTimeDismissedExistingPat;
		private System.Windows.Forms.RadioButton radioTimeArrivedExistingPat;
		private System.Windows.Forms.RadioButton radioSetCompleteExistingPat;
		private System.Windows.Forms.RadioButton radioTimeDismissedNewPat;
		private System.Windows.Forms.RadioButton radioTimeArrivedNewPat;
		private System.Windows.Forms.RadioButton radioSetCompleteNewPat;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textCompNameOrIP;
		private System.Windows.Forms.TextBox textAPIToken;
		private System.Windows.Forms.TextBox textLocationID;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.CheckBox checkUseEConnector;
		private ValidDouble textApptSetComplete;
		private ValidDouble textApptTimeArrived;
		private ValidDouble textApptTimeDismissed;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}