namespace OpenDental{
	partial class FormReportSetup {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportSetup));
      this.checkReportsProcDate = new System.Windows.Forms.CheckBox();
      this.checkReportProdWO = new System.Windows.Forms.CheckBox();
      this.checkReportsShowPatNum = new System.Windows.Forms.CheckBox();
      this.checkReportPIClinic = new System.Windows.Forms.CheckBox();
      this.checkReportPrintWrapColumns = new System.Windows.Forms.CheckBox();
      this.butOK = new OpenDental.UI.Button();
      this.butCancel = new OpenDental.UI.Button();
      this.checkReportPIClinicInfo = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // checkReportsProcDate
      // 
      this.checkReportsProcDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportsProcDate.Location = new System.Drawing.Point(20, 19);
      this.checkReportsProcDate.Name = "checkReportsProcDate";
      this.checkReportsProcDate.Size = new System.Drawing.Size(369, 17);
      this.checkReportsProcDate.TabIndex = 199;
      this.checkReportsProcDate.Text = "Default to using Proc Date for PPO writeoffs";
      // 
      // checkReportProdWO
      // 
      this.checkReportProdWO.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportProdWO.Location = new System.Drawing.Point(20, 57);
      this.checkReportProdWO.Name = "checkReportProdWO";
      this.checkReportProdWO.Size = new System.Drawing.Size(369, 17);
      this.checkReportProdWO.TabIndex = 201;
      this.checkReportProdWO.Text = "Monthly P&&I scheduled production subtracts PPO writeoffs";
      // 
      // checkReportsShowPatNum
      // 
      this.checkReportsShowPatNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportsShowPatNum.Location = new System.Drawing.Point(20, 38);
      this.checkReportsShowPatNum.Name = "checkReportsShowPatNum";
      this.checkReportsShowPatNum.Size = new System.Drawing.Size(369, 17);
      this.checkReportsShowPatNum.TabIndex = 200;
      this.checkReportsShowPatNum.Text = "Show PatNum: Aging, OutstandingIns, ProcsNotBilled";
      // 
      // checkReportPIClinic
      // 
      this.checkReportPIClinic.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportPIClinic.Location = new System.Drawing.Point(20, 95);
      this.checkReportPIClinic.Name = "checkReportPIClinic";
      this.checkReportPIClinic.Size = new System.Drawing.Size(369, 17);
      this.checkReportPIClinic.TabIndex = 202;
      this.checkReportPIClinic.Text = "Default to showing clinic breakdown on P&&I reports.";
      // 
      // checkReportPrintWrapColumns
      // 
      this.checkReportPrintWrapColumns.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportPrintWrapColumns.Location = new System.Drawing.Point(20, 114);
      this.checkReportPrintWrapColumns.Name = "checkReportPrintWrapColumns";
      this.checkReportPrintWrapColumns.Size = new System.Drawing.Size(369, 17);
      this.checkReportPrintWrapColumns.TabIndex = 203;
      this.checkReportPrintWrapColumns.Text = "Wrap columns when printing";
      // 
      // butOK
      // 
      this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.butOK.Autosize = true;
      this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butOK.CornerRadius = 4F;
      this.butOK.Location = new System.Drawing.Point(233, 142);
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
      this.butCancel.Location = new System.Drawing.Point(314, 142);
      this.butCancel.Name = "butCancel";
      this.butCancel.Size = new System.Drawing.Size(75, 24);
      this.butCancel.TabIndex = 2;
      this.butCancel.Text = "&Cancel";
      this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
      // 
      // checkReportPIClinicInfo
      // 
      this.checkReportPIClinicInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkReportPIClinicInfo.Location = new System.Drawing.Point(20, 76);
      this.checkReportPIClinicInfo.Name = "checkReportPIClinicInfo";
      this.checkReportPIClinicInfo.Size = new System.Drawing.Size(369, 17);
      this.checkReportPIClinicInfo.TabIndex = 204;
      this.checkReportPIClinicInfo.Text = "Default to showing clinic info on Daily P&&I report.";
      // 
      // FormReportSetup
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(401, 185);
      this.Controls.Add(this.checkReportPIClinicInfo);
      this.Controls.Add(this.checkReportPrintWrapColumns);
      this.Controls.Add(this.checkReportPIClinic);
      this.Controls.Add(this.checkReportsShowPatNum);
      this.Controls.Add(this.checkReportProdWO);
      this.Controls.Add(this.checkReportsProcDate);
      this.Controls.Add(this.butOK);
      this.Controls.Add(this.butCancel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormReportSetup";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Report Setup";
      this.Load += new System.EventHandler(this.FormReportSetup_Load);
      this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkReportsProcDate;
		private System.Windows.Forms.CheckBox checkReportProdWO;
		private System.Windows.Forms.CheckBox checkReportsShowPatNum;
		private System.Windows.Forms.CheckBox checkReportPIClinic;
		private System.Windows.Forms.CheckBox checkReportPrintWrapColumns;
    private System.Windows.Forms.CheckBox checkReportPIClinicInfo;
  }
}