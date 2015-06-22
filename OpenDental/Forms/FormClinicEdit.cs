using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClinicEdit : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.TextBox textZip;
		private System.Windows.Forms.TextBox textAddress2;
		private System.Windows.Forms.TextBox textAddress;
		private System.Windows.Forms.TextBox textPhone;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBankNumber;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboPlaceService;
		private GroupBox groupBox4;
		private ComboBox comboInsBillingProv;
		private RadioButton radioInsBillingProvSpecific;
		private RadioButton radioInsBillingProvTreat;
		private RadioButton radioInsBillingProvDefault;
		private TextBox textFax;
		private Label label8;
		private Label label9;
		private Label label10;
		private TextBox textEmail;
		private Clinic ClinicCur;
		private Label label12;
		private ComboBox comboDefaultProvider;
		private UI.Button butPickDefaultProv;
		private UI.Button butEmail;
		private UI.Button butPickInsBillingProv;
		private List<Provider> _listProv;
		private UI.Button butNone;
		private long _provNumDefaultSelected;
		private CheckBox checkIsMedicalOnly;
		private long _provNumBillingSelected;

		///<summary></summary>
		public FormClinicEdit(Clinic clinicCur)
		{
			//
			// Required for Windows Form Designer support
			//
			ClinicCur=clinicCur;
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinicEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.textCity = new System.Windows.Forms.TextBox();
			this.textState = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.textPhone = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBankNumber = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.comboPlaceService = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butPickInsBillingProv = new OpenDental.UI.Button();
			this.comboInsBillingProv = new System.Windows.Forms.ComboBox();
			this.radioInsBillingProvSpecific = new System.Windows.Forms.RadioButton();
			this.radioInsBillingProvTreat = new System.Windows.Forms.RadioButton();
			this.radioInsBillingProvDefault = new System.Windows.Forms.RadioButton();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textFax = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.butEmail = new OpenDental.UI.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.comboDefaultProvider = new System.Windows.Forms.ComboBox();
			this.butPickDefaultProv = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.checkIsMedicalOnly = new System.Windows.Forms.CheckBox();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(197, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Clinic Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(223, 34);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(263, 20);
			this.textDescription.TabIndex = 0;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(223, 139);
			this.textCity.MaxLength = 255;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(155, 20);
			this.textCity.TabIndex = 5;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(378, 139);
			this.textState.MaxLength = 255;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(65, 20);
			this.textState.TabIndex = 6;
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(443, 139);
			this.textZip.MaxLength = 255;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(71, 20);
			this.textZip.TabIndex = 7;
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(223, 118);
			this.textAddress2.MaxLength = 255;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(291, 20);
			this.textAddress2.TabIndex = 4;
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(223, 97);
			this.textAddress.MaxLength = 255;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(291, 20);
			this.textAddress.TabIndex = 3;
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(223, 55);
			this.textPhone.MaxLength = 255;
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(157, 20);
			this.textPhone.TabIndex = 1;
			this.textPhone.TextChanged += new System.EventHandler(this.textPhone_TextChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(9, 143);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(212, 15);
			this.label11.TabIndex = 105;
			this.label11.Text = "City, ST, Zip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(21, 122);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(200, 17);
			this.label4.TabIndex = 103;
			this.label4.Text = "Address2";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(21, 99);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 17);
			this.label3.TabIndex = 101;
			this.label3.Text = "Address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(22, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 17);
			this.label2.TabIndex = 99;
			this.label2.Text = "Phone";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBankNumber
			// 
			this.textBankNumber.Location = new System.Drawing.Point(223, 194);
			this.textBankNumber.MaxLength = 255;
			this.textBankNumber.Name = "textBankNumber";
			this.textBankNumber.Size = new System.Drawing.Size(291, 20);
			this.textBankNumber.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(71, 198);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 17);
			this.label5.TabIndex = 109;
			this.label5.Text = "Bank Account Number";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(383, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 18);
			this.label6.TabIndex = 110;
			this.label6.Text = "(###)###-####";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(19, 371);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(198, 17);
			this.label7.TabIndex = 111;
			this.label7.Text = "Default Proc Place of Service";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(223, 368);
			this.comboPlaceService.MaxDropDownItems = 30;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(212, 21);
			this.comboPlaceService.TabIndex = 8;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butPickInsBillingProv);
			this.groupBox4.Controls.Add(this.comboInsBillingProv);
			this.groupBox4.Controls.Add(this.radioInsBillingProvSpecific);
			this.groupBox4.Controls.Add(this.radioInsBillingProvTreat);
			this.groupBox4.Controls.Add(this.radioInsBillingProvDefault);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(210, 234);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(262, 101);
			this.groupBox4.TabIndex = 11;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Default Insurance Billing Provider";
			// 
			// butPickInsBillingProv
			// 
			this.butPickInsBillingProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickInsBillingProv.Autosize = false;
			this.butPickInsBillingProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickInsBillingProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickInsBillingProv.CornerRadius = 2F;
			this.butPickInsBillingProv.Location = new System.Drawing.Point(231, 72);
			this.butPickInsBillingProv.Name = "butPickInsBillingProv";
			this.butPickInsBillingProv.Size = new System.Drawing.Size(23, 21);
			this.butPickInsBillingProv.TabIndex = 161;
			this.butPickInsBillingProv.Text = "...";
			this.butPickInsBillingProv.Click += new System.EventHandler(this.butPickInsBillingProv_Click);
			// 
			// comboInsBillingProv
			// 
			this.comboInsBillingProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboInsBillingProv.Location = new System.Drawing.Point(13, 72);
			this.comboInsBillingProv.Name = "comboInsBillingProv";
			this.comboInsBillingProv.Size = new System.Drawing.Size(212, 21);
			this.comboInsBillingProv.TabIndex = 3;
			this.comboInsBillingProv.SelectionChangeCommitted += new System.EventHandler(this.comboInsBillingProv_SelectionChangeCommitted);
			// 
			// radioInsBillingProvSpecific
			// 
			this.radioInsBillingProvSpecific.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvSpecific.Location = new System.Drawing.Point(13, 52);
			this.radioInsBillingProvSpecific.Name = "radioInsBillingProvSpecific";
			this.radioInsBillingProvSpecific.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvSpecific.TabIndex = 2;
			this.radioInsBillingProvSpecific.Text = "Specific Provider:";
			// 
			// radioInsBillingProvTreat
			// 
			this.radioInsBillingProvTreat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvTreat.Location = new System.Drawing.Point(13, 34);
			this.radioInsBillingProvTreat.Name = "radioInsBillingProvTreat";
			this.radioInsBillingProvTreat.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvTreat.TabIndex = 1;
			this.radioInsBillingProvTreat.Text = "Treating Provider";
			// 
			// radioInsBillingProvDefault
			// 
			this.radioInsBillingProvDefault.Checked = true;
			this.radioInsBillingProvDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvDefault.Location = new System.Drawing.Point(13, 16);
			this.radioInsBillingProvDefault.Name = "radioInsBillingProvDefault";
			this.radioInsBillingProvDefault.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvDefault.TabIndex = 0;
			this.radioInsBillingProvDefault.TabStop = true;
			this.radioInsBillingProvDefault.Text = "Default Practice Provider";
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
			this.butDelete.Location = new System.Drawing.Point(12, 420);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 26);
			this.butDelete.TabIndex = 10;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(482, 420);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 11;
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
			this.butCancel.Location = new System.Drawing.Point(563, 420);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 12;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textFax
			// 
			this.textFax.Location = new System.Drawing.Point(223, 76);
			this.textFax.MaxLength = 255;
			this.textFax.Name = "textFax";
			this.textFax.Size = new System.Drawing.Size(157, 20);
			this.textFax.TabIndex = 2;
			this.textFax.TextChanged += new System.EventHandler(this.textFax_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(22, 79);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(200, 17);
			this.label8.TabIndex = 113;
			this.label8.Text = "Fax";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(383, 78);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 18);
			this.label9.TabIndex = 114;
			this.label9.Text = "(###)###-####";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(53, 163);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(168, 17);
			this.label10.TabIndex = 111;
			this.label10.Text = "Clinic Email Address";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textEmail
			// 
			this.textEmail.BackColor = System.Drawing.SystemColors.Window;
			this.textEmail.Location = new System.Drawing.Point(223, 160);
			this.textEmail.MaxLength = 255;
			this.textEmail.Name = "textEmail";
			this.textEmail.ReadOnly = true;
			this.textEmail.Size = new System.Drawing.Size(261, 20);
			this.textEmail.TabIndex = 8;
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Location = new System.Drawing.Point(491, 159);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(24, 21);
			this.butEmail.TabIndex = 10;
			this.butEmail.Text = "...";
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(19, 345);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(198, 17);
			this.label12.TabIndex = 115;
			this.label12.Text = "Default Clinic Provider";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboDefaultProvider
			// 
			this.comboDefaultProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultProvider.Location = new System.Drawing.Point(223, 341);
			this.comboDefaultProvider.Name = "comboDefaultProvider";
			this.comboDefaultProvider.Size = new System.Drawing.Size(212, 21);
			this.comboDefaultProvider.TabIndex = 116;
			this.comboDefaultProvider.SelectionChangeCommitted += new System.EventHandler(this.comboDefaultProvider_SelectionChangeCommitted);
			// 
			// butPickDefaultProv
			// 
			this.butPickDefaultProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickDefaultProv.Autosize = false;
			this.butPickDefaultProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickDefaultProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickDefaultProv.CornerRadius = 2F;
			this.butPickDefaultProv.Location = new System.Drawing.Point(441, 341);
			this.butPickDefaultProv.Name = "butPickDefaultProv";
			this.butPickDefaultProv.Size = new System.Drawing.Size(23, 21);
			this.butPickDefaultProv.TabIndex = 160;
			this.butPickDefaultProv.Text = "...";
			this.butPickDefaultProv.Click += new System.EventHandler(this.butPickDefaultProv_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(470, 341);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(48, 21);
			this.butNone.TabIndex = 161;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// checkIsMedicalOnly
			// 
			this.checkIsMedicalOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsMedicalOnly.Location = new System.Drawing.Point(80, 12);
			this.checkIsMedicalOnly.Name = "checkIsMedicalOnly";
			this.checkIsMedicalOnly.Size = new System.Drawing.Size(157, 16);
			this.checkIsMedicalOnly.TabIndex = 164;
			this.checkIsMedicalOnly.Text = "Clinic is Medical";
			this.checkIsMedicalOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormClinicEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(650, 458);
			this.Controls.Add(this.checkIsMedicalOnly);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butPickDefaultProv);
			this.Controls.Add(this.comboDefaultProvider);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textFax);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.comboPlaceService);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textEmail);
			this.Controls.Add(this.textBankNumber);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textCity);
			this.Controls.Add(this.textState);
			this.Controls.Add(this.textZip);
			this.Controls.Add(this.textAddress2);
			this.Controls.Add(this.textAddress);
			this.Controls.Add(this.textPhone);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butEmail);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label6);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(666, 496);
			this.Name = "FormClinicEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Clinic";
			this.Load += new System.EventHandler(this.FormClinicEdit_Load);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClinicEdit_Load(object sender, System.EventArgs e) {
			checkIsMedicalOnly.Checked=ClinicCur.IsMedicalOnly;
			if(Programs.UsingEcwTightOrFullMode()) {
				checkIsMedicalOnly.Visible=false;
			}
			textDescription.Text=ClinicCur.Description;
			string phone=ClinicCur.Phone;
			if(phone!=null && phone.Length==10 && Application.CurrentCulture.Name=="en-US"){
				textPhone.Text="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
			}
			else{
				textPhone.Text=phone;
			}
			string fax=ClinicCur.Fax;
			if(fax!=null && fax.Length==10 && Application.CurrentCulture.Name=="en-US") {
				textFax.Text="("+fax.Substring(0,3)+")"+fax.Substring(3,3)+"-"+fax.Substring(6);
			}
			else {
				textFax.Text=fax;
			}
			textAddress.Text=ClinicCur.Address;
			textAddress2.Text=ClinicCur.Address2;
			textCity.Text=ClinicCur.City;
			textState.Text=ClinicCur.State;
			textZip.Text=ClinicCur.Zip;
			textBankNumber.Text=ClinicCur.BankNumber;
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ClinicCur.DefaultPlaceService;
			_listProv=ProviderC.GetListShort();
			_provNumDefaultSelected=ClinicCur.DefaultProv;
			_provNumBillingSelected=ClinicCur.InsBillingProv;
			comboInsBillingProv.Items.Clear();
			comboDefaultProvider.Items.Clear();
			for(int i=0;i<_listProv.Count;i++) {
				comboInsBillingProv.Items.Add(_listProv[i].GetLongDesc());//Only visible provs added to combobox.
				comboDefaultProvider.Items.Add(_listProv[i].GetLongDesc());
				if(_listProv[i].ProvNum==ClinicCur.InsBillingProv) {
					comboInsBillingProv.SelectedIndex=i;
				}
				if(_listProv[i].ProvNum==ClinicCur.DefaultProv) {
					comboDefaultProvider.SelectedIndex=i;
				}
			}
			if(ClinicCur.InsBillingProv==0){
				radioInsBillingProvDefault.Checked=true;//default=0
			}
			else if(ClinicCur.InsBillingProv==-1){
				radioInsBillingProvTreat.Checked=true;//treat=-1
			}
			else{
				if(comboInsBillingProv.SelectedIndex==-1) {//The provider exists but is hidden (exclude this block of code if provider selection is optional)
					comboInsBillingProv.Text=Providers.GetLongDesc(_provNumBillingSelected);//Appends "(hidden)" to the end of the long description.
				}
				radioInsBillingProvSpecific.Checked=true;//specific=any number >0. Foreign key to ProvNum
			}
			EmailAddress emailAddress=EmailAddresses.GetOne(ClinicCur.EmailAddressNum);
			if(emailAddress!=null) {
				textEmail.Text=emailAddress.EmailUsername;
			}
		}

		private void textPhone_TextChanged(object sender,System.EventArgs e) {
			int cursor=textPhone.SelectionStart;
			int length=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			if(textPhone.Text.Length>length)
				cursor++;
			textPhone.SelectionStart=cursor;		
		}

		private void textFax_TextChanged(object sender,EventArgs e) {
			int cursor=textFax.SelectionStart;
			int length=textFax.Text.Length;
			textFax.Text=TelephoneNumbers.AutoFormat(textFax.Text);
			if(textFax.Text.Length>length)
				cursor++;
			textFax.SelectionStart=cursor;
		}

		private void butEmail_Click(object sender,EventArgs e) {
			FormEmailAddresses FormEA=new FormEmailAddresses();
			FormEA.IsSelectionMode=true;
			FormEA.ShowDialog();
			if(FormEA.DialogResult!=DialogResult.OK) {
				return;
			}
			ClinicCur.EmailAddressNum=FormEA.EmailAddressNum;
			textEmail.Text=EmailAddresses.GetOne(FormEA.EmailAddressNum).EmailUsername;
		}

		private void butPickInsBillingProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			if(comboInsBillingProv.SelectedIndex>-1) {
				FormPP.SelectedProvNum=_listProv[comboInsBillingProv.SelectedIndex].ProvNum;
			}
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboInsBillingProv.SelectedIndex=Providers.GetIndex(FormPP.SelectedProvNum);
			_provNumBillingSelected=FormPP.SelectedProvNum;
		}

		private void butPickDefaultProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			if(comboDefaultProvider.SelectedIndex>-1) {
				FormPP.SelectedProvNum=_listProv[comboDefaultProvider.SelectedIndex].ProvNum;
			}
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboDefaultProvider.SelectedIndex=Providers.GetIndex(FormPP.SelectedProvNum);
			_provNumDefaultSelected=FormPP.SelectedProvNum;
		}

		private void comboDefaultProvider_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumDefaultSelected=_listProv[comboDefaultProvider.SelectedIndex].ProvNum;
		}

		private void comboInsBillingProv_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumBillingSelected=_listProv[comboInsBillingProv.SelectedIndex].ProvNum;
		}

		private void butNone_Click(object sender,EventArgs e) {
			_provNumDefaultSelected=0;
			comboDefaultProvider.SelectedIndex=-1;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete Clinic?")) {
				return;
			}
			try{
				Clinics.Delete(ClinicCur);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDescription.Text==""){
				MessageBox.Show(Lan.g(this,"Description cannot be blank."));
				return;
			}
			if(radioInsBillingProvSpecific.Checked && comboInsBillingProv.SelectedIndex==-1){
				MsgBox.Show(this,"You must select a provider.");
				return;
			}
			string phone=textPhone.Text;
			if(Application.CurrentCulture.Name=="en-US"){
				phone=phone.Replace("(","");
				phone=phone.Replace(")","");
				phone=phone.Replace(" ","");
				phone=phone.Replace("-","");
				if(phone.Length!=0 && phone.Length!=10){
					MessageBox.Show("Invalid phone");
					return;
				}
			}
			string fax=textFax.Text;
			if(Application.CurrentCulture.Name=="en-US") {
				fax=fax.Replace("(","");
				fax=fax.Replace(")","");
				fax=fax.Replace(" ","");
				fax=fax.Replace("-","");
				if(fax.Length!=0 && fax.Length!=10) {
					MessageBox.Show("Invalid fax");
					return;
				}
			}
			ClinicCur.IsMedicalOnly=checkIsMedicalOnly.Checked;
			ClinicCur.Description=textDescription.Text;
			ClinicCur.Phone=phone;
			ClinicCur.Fax=fax;
			ClinicCur.Address=textAddress.Text;
			ClinicCur.Address2=textAddress2.Text;
			ClinicCur.City=textCity.Text;
			ClinicCur.State=textState.Text;
			ClinicCur.Zip=textZip.Text;
			ClinicCur.BankNumber=textBankNumber.Text;
			ClinicCur.DefaultPlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			if(radioInsBillingProvDefault.Checked){//default=0
				ClinicCur.InsBillingProv=0;
			}
			else if(radioInsBillingProvTreat.Checked){//treat=-1
				ClinicCur.InsBillingProv=-1;
			}
			else{
				ClinicCur.InsBillingProv=_provNumBillingSelected;
			}
			ClinicCur.DefaultProv=_provNumDefaultSelected;
			if(IsNew){
				Clinics.Insert(ClinicCur);
			}
			else{
				Clinics.Update(ClinicCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		

		

		


	}
}





















