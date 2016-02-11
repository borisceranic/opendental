namespace OpenDental{
	partial class FormInsVerificationSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsVerificationSetup));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.label23 = new System.Windows.Forms.Label();
			this.textInsBenefitEligibilityDays = new OpenDental.ValidNumber();
			this.label29 = new System.Windows.Forms.Label();
			this.textScheduledAppointmentDays = new OpenDental.ValidNumber();
			this.textPatientEnrollmentDays = new OpenDental.ValidNumber();
			this.label30 = new System.Windows.Forms.Label();
			this.groupBox9.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(117, 129);
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
			this.butCancel.Location = new System.Drawing.Point(198, 129);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.label23);
			this.groupBox9.Controls.Add(this.textInsBenefitEligibilityDays);
			this.groupBox9.Controls.Add(this.label29);
			this.groupBox9.Controls.Add(this.textScheduledAppointmentDays);
			this.groupBox9.Controls.Add(this.textPatientEnrollmentDays);
			this.groupBox9.Controls.Add(this.label30);
			this.groupBox9.Location = new System.Drawing.Point(12, 12);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(261, 101);
			this.groupBox9.TabIndex = 90;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Number of days before verification is needed";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(0, 19);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(192, 20);
			this.label23.TabIndex = 75;
			this.label23.Text = "Insurance benefits";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsBenefitEligibilityDays
			// 
			this.textInsBenefitEligibilityDays.Location = new System.Drawing.Point(198, 19);
			this.textInsBenefitEligibilityDays.MaxVal = 255;
			this.textInsBenefitEligibilityDays.MinVal = 0;
			this.textInsBenefitEligibilityDays.Name = "textInsBenefitEligibilityDays";
			this.textInsBenefitEligibilityDays.Size = new System.Drawing.Size(51, 20);
			this.textInsBenefitEligibilityDays.TabIndex = 76;
			this.textInsBenefitEligibilityDays.Text = "0";
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(0, 45);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(192, 20);
			this.label29.TabIndex = 83;
			this.label29.Text = "Patient eligibility:";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textScheduledAppointmentDays
			// 
			this.textScheduledAppointmentDays.Location = new System.Drawing.Point(198, 71);
			this.textScheduledAppointmentDays.MaxVal = 255;
			this.textScheduledAppointmentDays.MinVal = 0;
			this.textScheduledAppointmentDays.Name = "textScheduledAppointmentDays";
			this.textScheduledAppointmentDays.Size = new System.Drawing.Size(51, 20);
			this.textScheduledAppointmentDays.TabIndex = 86;
			this.textScheduledAppointmentDays.Text = "0";
			// 
			// textPatientEnrollmentDays
			// 
			this.textPatientEnrollmentDays.Location = new System.Drawing.Point(198, 45);
			this.textPatientEnrollmentDays.MaxVal = 255;
			this.textPatientEnrollmentDays.MinVal = 0;
			this.textPatientEnrollmentDays.Name = "textPatientEnrollmentDays";
			this.textPatientEnrollmentDays.Size = new System.Drawing.Size(51, 20);
			this.textPatientEnrollmentDays.TabIndex = 84;
			this.textPatientEnrollmentDays.Text = "0";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(0, 71);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(192, 20);
			this.label30.TabIndex = 85;
			this.label30.Text = "Scheduled appointment:";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormInsVerificationSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(285, 165);
			this.Controls.Add(this.groupBox9);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormInsVerificationSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Insurance Verification Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInsVerificationSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormInsVerificationSetup_Load);
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label23;
        private ValidNumber textInsBenefitEligibilityDays;
        private System.Windows.Forms.Label label29;
        private ValidNumber textScheduledAppointmentDays;
        private ValidNumber textPatientEnrollmentDays;
        private System.Windows.Forms.Label label30;
    }
}