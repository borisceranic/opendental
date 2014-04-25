namespace OpenDental{
	partial class FormPatientPortalSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientPortalSetup));
			this.butOK = new System.Windows.Forms.Button();
			this.butCancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.butGetURL = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textOpenDentalURl = new System.Windows.Forms.TextBox();
			this.textBoxNotificationSubject = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxNotificationBody = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBoxNotification = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textListenerPort = new OpenDental.ValidNum();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textPatientPortalURL = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBoxNotification.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(754, 606);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(836, 606);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 17);
			this.label2.TabIndex = 40;
			this.label2.Text = "Hosted URL";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butGetURL
			// 
			this.butGetURL.Location = new System.Drawing.Point(500, 46);
			this.butGetURL.Name = "butGetURL";
			this.butGetURL.Size = new System.Drawing.Size(94, 23);
			this.butGetURL.TabIndex = 41;
			this.butGetURL.Text = "Get URL";
			this.butGetURL.UseVisualStyleBackColor = true;
			this.butGetURL.Click += new System.EventHandler(this.butGetURL_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(39, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(851, 28);
			this.label3.TabIndex = 42;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// textOpenDentalURl
			// 
			this.textOpenDentalURl.Location = new System.Drawing.Point(144, 47);
			this.textOpenDentalURl.Name = "textOpenDentalURl";
			this.textOpenDentalURl.Size = new System.Drawing.Size(349, 20);
			this.textOpenDentalURl.TabIndex = 43;
			this.textOpenDentalURl.Text = "Click \'Get URL\'";
			// 
			// textBoxNotificationSubject
			// 
			this.textBoxNotificationSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxNotificationSubject.Location = new System.Drawing.Point(93, 72);
			this.textBoxNotificationSubject.Name = "textBoxNotificationSubject";
			this.textBoxNotificationSubject.Size = new System.Drawing.Size(797, 20);
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
			this.textBoxNotificationBody.Size = new System.Drawing.Size(797, 152);
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
			this.groupBoxNotification.Location = new System.Drawing.Point(15, 324);
			this.groupBoxNotification.Name = "groupBoxNotification";
			this.groupBoxNotification.Size = new System.Drawing.Size(896, 276);
			this.groupBoxNotification.TabIndex = 48;
			this.groupBoxNotification.TabStop = false;
			this.groupBoxNotification.Text = "Notification Email";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(39, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(851, 53);
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
			this.groupBox1.Controls.Add(this.textListenerPort);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textOpenDentalURl);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.butGetURL);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(15, 7);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(896, 100);
			this.groupBox1.TabIndex = 49;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Open Dental Hosted";
			// 
			// textListenerPort
			// 
			this.textListenerPort.Location = new System.Drawing.Point(144, 73);
			this.textListenerPort.MaxVal = 65535;
			this.textListenerPort.MinVal = 0;
			this.textListenerPort.Name = "textListenerPort";
			this.textListenerPort.Size = new System.Drawing.Size(100, 20);
			this.textListenerPort.TabIndex = 51;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(12, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(126, 17);
			this.label5.TabIndex = 48;
			this.label5.Text = "Listener Port";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(24, 116);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(129, 17);
			this.label8.TabIndex = 52;
			this.label8.Text = "Patient Facing URL";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatientPortalURL
			// 
			this.textPatientPortalURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPatientPortalURL.Location = new System.Drawing.Point(159, 114);
			this.textPatientPortalURL.Name = "textPatientPortalURL";
			this.textPatientPortalURL.Size = new System.Drawing.Size(746, 20);
			this.textPatientPortalURL.TabIndex = 50;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(54, 137);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(857, 184);
			this.label1.TabIndex = 51;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// FormPatientPortalSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(923, 641);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textPatientPortalURL);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBoxNotification);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormPatientPortalSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Patient Portal Setup";
			this.Load += new System.EventHandler(this.FormPatientPortalSetup_Load);
			this.groupBoxNotification.ResumeLayout(false);
			this.groupBoxNotification.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Button butCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button butGetURL;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textOpenDentalURl;
		private System.Windows.Forms.TextBox textBoxNotificationSubject;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxNotificationBody;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBoxNotification;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPatientPortalURL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private ValidNum textListenerPort;

	}
}