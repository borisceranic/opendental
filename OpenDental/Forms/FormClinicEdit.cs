using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClinicEdit : ODForm {
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
		private System.Windows.Forms.TextBox textPhone;
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
		public Clinic ClinicCur;
		private Label label12;
		private ComboBox comboDefaultProvider;
		private UI.Button butPickDefaultProv;
		private UI.Button butEmail;
		private UI.Button butPickInsBillingProv;
		private List<Provider> _listProv;
		private UI.Button butNone;
		private CheckBox checkIsMedicalOnly;
		private TextBox textCity;
		private TextBox textState;
		private TextBox textZip;
		private TextBox textAddress2;
		private Label label11;
		private Label label4;
		private TextBox textAddress;
		private Label label3;
		private Label label17;
		private Label label13;
		private TextBox textPayToZip;
		private TextBox textPayToST;
		private TextBox textPayToCity;
		private TextBox textPayToAddress2;
		private TextBox textPayToAddress;
		private Label label14;
		private Label label15;
		private Label label18;
		private Label label16;
		private TextBox textBillingZip;
		private TextBox textBillingST;
		private TextBox textBillingCity;
		private TextBox textBillingAddress2;
		private TextBox textBillingAddress;
		private Label label19;
		private Label label20;
		private TextBox textClinicNum;
		private Label label21;
		private CheckBox checkUseBillingAddressOnClaims;
		private Label label22;
		private ComboBox comboRegion;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvBillNum;
		private TabControl tabControl1;
		private TabPage PhysicalAddress;
		private TabPage BillingAddress;
		private TabPage PayToAddress;
		private CheckBox checkExcludeFromInsVerifyList;

		///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvDefNum;

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
			this.textPhone = new System.Windows.Forms.TextBox();
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
			this.textFax = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboDefaultProvider = new System.Windows.Forms.ComboBox();
			this.checkIsMedicalOnly = new System.Windows.Forms.CheckBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textCity = new System.Windows.Forms.TextBox();
			this.textState = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.textPayToZip = new System.Windows.Forms.TextBox();
			this.textPayToST = new System.Windows.Forms.TextBox();
			this.textPayToCity = new System.Windows.Forms.TextBox();
			this.textPayToAddress2 = new System.Windows.Forms.TextBox();
			this.textPayToAddress = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.checkUseBillingAddressOnClaims = new System.Windows.Forms.CheckBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.textBillingZip = new System.Windows.Forms.TextBox();
			this.textBillingST = new System.Windows.Forms.TextBox();
			this.textBillingCity = new System.Windows.Forms.TextBox();
			this.textBillingAddress2 = new System.Windows.Forms.TextBox();
			this.textBillingAddress = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.butNone = new OpenDental.UI.Button();
			this.butPickDefaultProv = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butEmail = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textClinicNum = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.comboRegion = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.PhysicalAddress = new System.Windows.Forms.TabPage();
			this.BillingAddress = new System.Windows.Forms.TabPage();
			this.PayToAddress = new System.Windows.Forms.TabPage();
			this.checkExcludeFromInsVerifyList = new System.Windows.Forms.CheckBox();
			this.groupBox4.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.PhysicalAddress.SuspendLayout();
			this.BillingAddress.SuspendLayout();
			this.PayToAddress.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(207, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Clinic Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(223, 49);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(263, 20);
			this.textDescription.TabIndex = 2;
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(223, 70);
			this.textPhone.MaxLength = 255;
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(157, 20);
			this.textPhone.TabIndex = 3;
			this.textPhone.TextChanged += new System.EventHandler(this.textPhone_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(210, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Phone";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBankNumber
			// 
			this.textBankNumber.Location = new System.Drawing.Point(223, 365);
			this.textBankNumber.MaxLength = 255;
			this.textBankNumber.Name = "textBankNumber";
			this.textBankNumber.Size = new System.Drawing.Size(291, 20);
			this.textBankNumber.TabIndex = 10;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(71, 369);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Bank Account Number";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(383, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 18);
			this.label6.TabIndex = 0;
			this.label6.Text = "(###)###-####";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(32, 518);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(198, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Default Proc Place of Service";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(236, 515);
			this.comboPlaceService.MaxDropDownItems = 30;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(216, 21);
			this.comboPlaceService.TabIndex = 15;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butPickInsBillingProv);
			this.groupBox4.Controls.Add(this.comboInsBillingProv);
			this.groupBox4.Controls.Add(this.radioInsBillingProvSpecific);
			this.groupBox4.Controls.Add(this.radioInsBillingProvTreat);
			this.groupBox4.Controls.Add(this.radioInsBillingProvDefault);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(223, 387);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(262, 100);
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
			this.butPickInsBillingProv.Location = new System.Drawing.Point(231, 73);
			this.butPickInsBillingProv.Name = "butPickInsBillingProv";
			this.butPickInsBillingProv.Size = new System.Drawing.Size(23, 21);
			this.butPickInsBillingProv.TabIndex = 5;
			this.butPickInsBillingProv.Text = "...";
			this.butPickInsBillingProv.Click += new System.EventHandler(this.butPickInsBillingProv_Click);
			// 
			// comboInsBillingProv
			// 
			this.comboInsBillingProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboInsBillingProv.Location = new System.Drawing.Point(13, 73);
			this.comboInsBillingProv.Name = "comboInsBillingProv";
			this.comboInsBillingProv.Size = new System.Drawing.Size(216, 21);
			this.comboInsBillingProv.TabIndex = 4;
			this.comboInsBillingProv.SelectedIndexChanged += new System.EventHandler(this.comboInsBillingProv_SelectedIndexChanged);
			// 
			// radioInsBillingProvSpecific
			// 
			this.radioInsBillingProvSpecific.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvSpecific.Location = new System.Drawing.Point(13, 53);
			this.radioInsBillingProvSpecific.Name = "radioInsBillingProvSpecific";
			this.radioInsBillingProvSpecific.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvSpecific.TabIndex = 3;
			this.radioInsBillingProvSpecific.Text = "Specific Provider:";
			// 
			// radioInsBillingProvTreat
			// 
			this.radioInsBillingProvTreat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvTreat.Location = new System.Drawing.Point(13, 35);
			this.radioInsBillingProvTreat.Name = "radioInsBillingProvTreat";
			this.radioInsBillingProvTreat.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvTreat.TabIndex = 2;
			this.radioInsBillingProvTreat.Text = "Treating Provider";
			// 
			// radioInsBillingProvDefault
			// 
			this.radioInsBillingProvDefault.Checked = true;
			this.radioInsBillingProvDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvDefault.Location = new System.Drawing.Point(13, 17);
			this.radioInsBillingProvDefault.Name = "radioInsBillingProvDefault";
			this.radioInsBillingProvDefault.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvDefault.TabIndex = 1;
			this.radioInsBillingProvDefault.TabStop = true;
			this.radioInsBillingProvDefault.Text = "Default Practice Provider";
			// 
			// textFax
			// 
			this.textFax.Location = new System.Drawing.Point(223, 91);
			this.textFax.MaxLength = 255;
			this.textFax.Name = "textFax";
			this.textFax.Size = new System.Drawing.Size(157, 20);
			this.textFax.TabIndex = 4;
			this.textFax.TextChanged += new System.EventHandler(this.textFax_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(12, 94);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(210, 17);
			this.label8.TabIndex = 0;
			this.label8.Text = "Fax";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(383, 93);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 18);
			this.label9.TabIndex = 0;
			this.label9.Text = "(###)###-####";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(53, 347);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(168, 17);
			this.label10.TabIndex = 0;
			this.label10.Text = "Clinic Email Address";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textEmail
			// 
			this.textEmail.BackColor = System.Drawing.SystemColors.Window;
			this.textEmail.Location = new System.Drawing.Point(223, 344);
			this.textEmail.MaxLength = 255;
			this.textEmail.Name = "textEmail";
			this.textEmail.ReadOnly = true;
			this.textEmail.Size = new System.Drawing.Size(266, 20);
			this.textEmail.TabIndex = 8;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(32, 494);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(198, 17);
			this.label12.TabIndex = 0;
			this.label12.Text = "Default Clinic Provider";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboDefaultProvider
			// 
			this.comboDefaultProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultProvider.Location = new System.Drawing.Point(236, 490);
			this.comboDefaultProvider.Name = "comboDefaultProvider";
			this.comboDefaultProvider.Size = new System.Drawing.Size(216, 21);
			this.comboDefaultProvider.TabIndex = 12;
			this.comboDefaultProvider.SelectedIndexChanged += new System.EventHandler(this.comboDefaultProvider_SelectedIndexChanged);
			// 
			// checkIsMedicalOnly
			// 
			this.checkIsMedicalOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsMedicalOnly.Location = new System.Drawing.Point(80, 9);
			this.checkIsMedicalOnly.Name = "checkIsMedicalOnly";
			this.checkIsMedicalOnly.Size = new System.Drawing.Size(157, 16);
			this.checkIsMedicalOnly.TabIndex = 0;
			this.checkIsMedicalOnly.Text = "Clinic is Medical";
			this.checkIsMedicalOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(103, 39);
			this.textAddress.MaxLength = 255;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(291, 20);
			this.textAddress.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(7, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(103, 81);
			this.textCity.MaxLength = 255;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(155, 20);
			this.textCity.TabIndex = 3;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(257, 81);
			this.textState.MaxLength = 255;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(66, 20);
			this.textState.TabIndex = 4;
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(322, 81);
			this.textZip.MaxLength = 255;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(72, 20);
			this.textZip.TabIndex = 5;
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(103, 60);
			this.textAddress2.MaxLength = 255;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(291, 20);
			this.textAddress2.TabIndex = 2;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(7, 83);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(95, 15);
			this.label11.TabIndex = 0;
			this.label11.Text = "City, ST, Zip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(7, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Address 2";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.BackColor = System.Drawing.Color.Transparent;
			this.label17.Location = new System.Drawing.Point(105, 5);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(288, 30);
			this.label17.TabIndex = 0;
			this.label17.Text = "Optional for claims.  Can be a PO Box.  Sent in addition to treating or billing a" +
    "ddress.";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(5, 61);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(97, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "Address 2";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPayToZip
			// 
			this.textPayToZip.Location = new System.Drawing.Point(322, 81);
			this.textPayToZip.Name = "textPayToZip";
			this.textPayToZip.Size = new System.Drawing.Size(72, 20);
			this.textPayToZip.TabIndex = 5;
			// 
			// textPayToST
			// 
			this.textPayToST.Location = new System.Drawing.Point(257, 81);
			this.textPayToST.Name = "textPayToST";
			this.textPayToST.Size = new System.Drawing.Size(66, 20);
			this.textPayToST.TabIndex = 4;
			// 
			// textPayToCity
			// 
			this.textPayToCity.Location = new System.Drawing.Point(103, 81);
			this.textPayToCity.Name = "textPayToCity";
			this.textPayToCity.Size = new System.Drawing.Size(155, 20);
			this.textPayToCity.TabIndex = 3;
			// 
			// textPayToAddress2
			// 
			this.textPayToAddress2.Location = new System.Drawing.Point(103, 60);
			this.textPayToAddress2.Name = "textPayToAddress2";
			this.textPayToAddress2.Size = new System.Drawing.Size(291, 20);
			this.textPayToAddress2.TabIndex = 2;
			// 
			// textPayToAddress
			// 
			this.textPayToAddress.Location = new System.Drawing.Point(103, 39);
			this.textPayToAddress.Name = "textPayToAddress";
			this.textPayToAddress.Size = new System.Drawing.Size(291, 20);
			this.textPayToAddress.TabIndex = 1;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(4, 41);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(98, 14);
			this.label14.TabIndex = 0;
			this.label14.Text = "Address";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(4, 83);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(98, 15);
			this.label15.TabIndex = 0;
			this.label15.Text = "City, ST, Zip";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseBillingAddressOnClaims
			// 
			this.checkUseBillingAddressOnClaims.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseBillingAddressOnClaims.Location = new System.Drawing.Point(6, 23);
			this.checkUseBillingAddressOnClaims.Name = "checkUseBillingAddressOnClaims";
			this.checkUseBillingAddressOnClaims.Size = new System.Drawing.Size(111, 16);
			this.checkUseBillingAddressOnClaims.TabIndex = 1;
			this.checkUseBillingAddressOnClaims.Text = "Use on Claims";
			this.checkUseBillingAddressOnClaims.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseBillingAddressOnClaims.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(105, 5);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(280, 15);
			this.label18.TabIndex = 0;
			this.label18.Text = "Optional, for E-Claims.  Cannot be a PO Box.";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(5, 61);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(97, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "Address 2";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBillingZip
			// 
			this.textBillingZip.Location = new System.Drawing.Point(321, 81);
			this.textBillingZip.Name = "textBillingZip";
			this.textBillingZip.Size = new System.Drawing.Size(73, 20);
			this.textBillingZip.TabIndex = 6;
			// 
			// textBillingST
			// 
			this.textBillingST.Location = new System.Drawing.Point(257, 81);
			this.textBillingST.Name = "textBillingST";
			this.textBillingST.Size = new System.Drawing.Size(65, 20);
			this.textBillingST.TabIndex = 5;
			// 
			// textBillingCity
			// 
			this.textBillingCity.Location = new System.Drawing.Point(103, 81);
			this.textBillingCity.Name = "textBillingCity";
			this.textBillingCity.Size = new System.Drawing.Size(155, 20);
			this.textBillingCity.TabIndex = 4;
			// 
			// textBillingAddress2
			// 
			this.textBillingAddress2.Location = new System.Drawing.Point(103, 60);
			this.textBillingAddress2.Name = "textBillingAddress2";
			this.textBillingAddress2.Size = new System.Drawing.Size(291, 20);
			this.textBillingAddress2.TabIndex = 3;
			// 
			// textBillingAddress
			// 
			this.textBillingAddress.Location = new System.Drawing.Point(103, 39);
			this.textBillingAddress.Name = "textBillingAddress";
			this.textBillingAddress.Size = new System.Drawing.Size(291, 20);
			this.textBillingAddress.TabIndex = 2;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(4, 41);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(98, 14);
			this.label19.TabIndex = 0;
			this.label19.Text = "Address";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(4, 83);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(98, 15);
			this.label20.TabIndex = 0;
			this.label20.Text = "City, ST, Zip";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(483, 490);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(48, 21);
			this.butNone.TabIndex = 14;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butPickDefaultProv
			// 
			this.butPickDefaultProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickDefaultProv.Autosize = false;
			this.butPickDefaultProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickDefaultProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickDefaultProv.CornerRadius = 2F;
			this.butPickDefaultProv.Location = new System.Drawing.Point(454, 490);
			this.butPickDefaultProv.Name = "butPickDefaultProv";
			this.butPickDefaultProv.Size = new System.Drawing.Size(23, 21);
			this.butPickDefaultProv.TabIndex = 13;
			this.butPickDefaultProv.Text = "...";
			this.butPickDefaultProv.Click += new System.EventHandler(this.butPickDefaultProv_Click);
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
			this.butDelete.Location = new System.Drawing.Point(12, 558);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 26);
			this.butDelete.TabIndex = 18;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Location = new System.Drawing.Point(491, 343);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(24, 21);
			this.butEmail.TabIndex = 9;
			this.butEmail.Text = "...";
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(518, 558);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 16;
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
			this.butCancel.Location = new System.Drawing.Point(599, 558);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 17;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textClinicNum
			// 
			this.textClinicNum.BackColor = System.Drawing.SystemColors.Control;
			this.textClinicNum.Location = new System.Drawing.Point(223, 26);
			this.textClinicNum.Name = "textClinicNum";
			this.textClinicNum.ReadOnly = true;
			this.textClinicNum.Size = new System.Drawing.Size(157, 20);
			this.textClinicNum.TabIndex = 1;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(13, 27);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(207, 17);
			this.label21.TabIndex = 19;
			this.label21.Text = "Clinic ID";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(12, 115);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(210, 17);
			this.label22.TabIndex = 20;
			this.label22.Text = "Region";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboRegion
			// 
			this.comboRegion.FormattingEnabled = true;
			this.comboRegion.Location = new System.Drawing.Point(223, 112);
			this.comboRegion.Name = "comboRegion";
			this.comboRegion.Size = new System.Drawing.Size(157, 21);
			this.comboRegion.TabIndex = 21;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.PhysicalAddress);
			this.tabControl1.Controls.Add(this.BillingAddress);
			this.tabControl1.Controls.Add(this.PayToAddress);
			this.tabControl1.Location = new System.Drawing.Point(114, 154);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(460, 186);
			this.tabControl1.TabIndex = 22;
			// 
			// PhysicalAddress
			// 
			this.PhysicalAddress.BackColor = System.Drawing.Color.Transparent;
			this.PhysicalAddress.Controls.Add(this.textAddress);
			this.PhysicalAddress.Controls.Add(this.label3);
			this.PhysicalAddress.Controls.Add(this.label4);
			this.PhysicalAddress.Controls.Add(this.textCity);
			this.PhysicalAddress.Controls.Add(this.label11);
			this.PhysicalAddress.Controls.Add(this.textState);
			this.PhysicalAddress.Controls.Add(this.textAddress2);
			this.PhysicalAddress.Controls.Add(this.textZip);
			this.PhysicalAddress.Location = new System.Drawing.Point(4, 22);
			this.PhysicalAddress.Name = "PhysicalAddress";
			this.PhysicalAddress.Padding = new System.Windows.Forms.Padding(3);
			this.PhysicalAddress.Size = new System.Drawing.Size(452, 160);
			this.PhysicalAddress.TabIndex = 0;
			this.PhysicalAddress.Text = "Physical Treating Address";
			// 
			// BillingAddress
			// 
			this.BillingAddress.BackColor = System.Drawing.Color.Transparent;
			this.BillingAddress.Controls.Add(this.checkUseBillingAddressOnClaims);
			this.BillingAddress.Controls.Add(this.label18);
			this.BillingAddress.Controls.Add(this.label20);
			this.BillingAddress.Controls.Add(this.label16);
			this.BillingAddress.Controls.Add(this.label19);
			this.BillingAddress.Controls.Add(this.textBillingZip);
			this.BillingAddress.Controls.Add(this.textBillingAddress);
			this.BillingAddress.Controls.Add(this.textBillingST);
			this.BillingAddress.Controls.Add(this.textBillingAddress2);
			this.BillingAddress.Controls.Add(this.textBillingCity);
			this.BillingAddress.Location = new System.Drawing.Point(4, 22);
			this.BillingAddress.Name = "BillingAddress";
			this.BillingAddress.Padding = new System.Windows.Forms.Padding(3);
			this.BillingAddress.Size = new System.Drawing.Size(452, 160);
			this.BillingAddress.TabIndex = 1;
			this.BillingAddress.Text = "Billing Address";
			// 
			// PayToAddress
			// 
			this.PayToAddress.BackColor = System.Drawing.Color.Transparent;
			this.PayToAddress.Controls.Add(this.label17);
			this.PayToAddress.Controls.Add(this.label13);
			this.PayToAddress.Controls.Add(this.label15);
			this.PayToAddress.Controls.Add(this.textPayToZip);
			this.PayToAddress.Controls.Add(this.label14);
			this.PayToAddress.Controls.Add(this.textPayToST);
			this.PayToAddress.Controls.Add(this.textPayToAddress);
			this.PayToAddress.Controls.Add(this.textPayToCity);
			this.PayToAddress.Controls.Add(this.textPayToAddress2);
			this.PayToAddress.Location = new System.Drawing.Point(4, 22);
			this.PayToAddress.Name = "PayToAddress";
			this.PayToAddress.Size = new System.Drawing.Size(452, 160);
			this.PayToAddress.TabIndex = 2;
			this.PayToAddress.Text = "Pay To Address";
			// 
			// checkExcludeFromInsVerifyList
			// 
			this.checkExcludeFromInsVerifyList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeFromInsVerifyList.Location = new System.Drawing.Point(5, 135);
			this.checkExcludeFromInsVerifyList.Name = "checkExcludeFromInsVerifyList";
			this.checkExcludeFromInsVerifyList.Size = new System.Drawing.Size(232, 16);
			this.checkExcludeFromInsVerifyList.TabIndex = 23;
			this.checkExcludeFromInsVerifyList.Text = "Hide From Insurance Verification List";
			this.checkExcludeFromInsVerifyList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormClinicEdit
			// 
			this.ClientSize = new System.Drawing.Size(686, 596);
			this.Controls.Add(this.checkExcludeFromInsVerifyList);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.comboRegion);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.textClinicNum);
			this.Controls.Add(this.label21);
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
			this.Controls.Add(this.textPhone);
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
			this.Text = "Edit Clinic";
			this.Load += new System.EventHandler(this.FormClinicEdit_Load);
			this.groupBox4.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.PhysicalAddress.ResumeLayout(false);
			this.PhysicalAddress.PerformLayout();
			this.BillingAddress.ResumeLayout(false);
			this.BillingAddress.PerformLayout();
			this.PayToAddress.ResumeLayout(false);
			this.PayToAddress.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClinicEdit_Load(object sender, System.EventArgs e) {
			checkIsMedicalOnly.Checked=ClinicCur.IsMedicalOnly;
			if(Programs.UsingEcwTightOrFullMode()) {
				checkIsMedicalOnly.Visible=false;
			}
			if(ClinicCur.ClinicNum!=0) {
				textClinicNum.Text=ClinicCur.ClinicNum.ToString();
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
			checkUseBillingAddressOnClaims.Checked=ClinicCur.UseBillAddrOnClaims;
			checkExcludeFromInsVerifyList.Checked=ClinicCur.IsInsVerifyExcluded;
			textAddress.Text=ClinicCur.Address;
			textAddress2.Text=ClinicCur.Address2;
			textCity.Text=ClinicCur.City;
			textState.Text=ClinicCur.State;
			textZip.Text=ClinicCur.Zip;
			textBillingAddress.Text=ClinicCur.BillingAddress;
			textBillingAddress2.Text=ClinicCur.BillingAddress2;
			textBillingCity.Text=ClinicCur.BillingCity;
			textBillingST.Text=ClinicCur.BillingState;
			textBillingZip.Text=ClinicCur.BillingZip;
			textPayToAddress.Text=ClinicCur.PayToAddress;
			textPayToAddress2.Text=ClinicCur.PayToAddress2;
			textPayToCity.Text=ClinicCur.PayToCity;
			textPayToST.Text=ClinicCur.PayToState;
			textPayToZip.Text=ClinicCur.PayToZip;
			textBankNumber.Text=ClinicCur.BankNumber;
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ClinicCur.DefaultPlaceService;
			_selectedProvBillNum=ClinicCur.InsBillingProv;
			_selectedProvDefNum=ClinicCur.DefaultProv;
			comboDefaultProvider.SelectedIndex=-1;
			comboInsBillingProv.SelectedIndex=-1;
			_listProviders=Providers.GetProvsForClinic(ClinicCur.ClinicNum);
			fillComboProviders();
			comboRegion.Items.Clear();
			comboRegion.Items.Add(Lan.g(this,"none"));
			comboRegion.SelectedIndex=0;
			Def[] arrayRegionDefs=DefC.Short[(int)DefCat.Regions];
			for(int i=0;i<arrayRegionDefs.Length;i++) {
				comboRegion.Items.Add(arrayRegionDefs[i].ItemName);
				if(arrayRegionDefs[i].DefNum==ClinicCur.Region) {
					comboRegion.SelectedIndex=i+1;
				}
			}
			if(ClinicCur.InsBillingProv==0){
				radioInsBillingProvDefault.Checked=true;//default=0
			}
			else if(ClinicCur.InsBillingProv==-1){
				radioInsBillingProvTreat.Checked=true;//treat=-1
			}
			else{
				radioInsBillingProvSpecific.Checked=true;//specific=any number >0. Foreign key to ProvNum
			}
			EmailAddress emailAddress=EmailAddresses.GetOne(ClinicCur.EmailAddressNum);
			if(emailAddress!=null) {
				textEmail.Text=emailAddress.EmailUsername;
			}
		}

		private void comboInsBillingProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboInsBillingProv.SelectedIndex>-1) {
				_selectedProvBillNum=_listProviders[comboInsBillingProv.SelectedIndex].ProvNum;
			}
		}

		private void comboDefaultProvider_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboDefaultProvider.SelectedIndex>-1) {
				_selectedProvDefNum=_listProviders[comboDefaultProvider.SelectedIndex].ProvNum;
			}
		}

		private void butPickInsBillingProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvBillNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvBillNum=FormPP.SelectedProvNum;
			comboInsBillingProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvBillNum),() => { return Providers.GetAbbr(_selectedProvBillNum); });
		}

		private void butPickDefaultProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvDefNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvDefNum=FormPP.SelectedProvNum;
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),() => { return Providers.GetAbbr(_selectedProvDefNum); });
		}

		private void butNone_Click(object sender,EventArgs e) {
			_selectedProvDefNum=0;
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),() => { return Providers.GetAbbr(_selectedProvDefNum); });
		}

		private void fillComboProviders() {
			//Fill comboInsBillingProvider
			comboInsBillingProv.Items.Clear();
			_listProviders.ForEach(x => comboInsBillingProv.Items.Add(x.Abbr));
			comboInsBillingProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvBillNum),() => { return Providers.GetAbbr(_selectedProvBillNum); });
			//Fill comboDefaultProvider
			comboDefaultProvider.Items.Clear();
			_listProviders.ForEach(x => comboDefaultProvider.Items.Add(x.Abbr));
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),() => { return Providers.GetAbbr(_selectedProvDefNum); });
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
				ClinicCur=null;//Set ClinicCur to null so the calling form knows it was deleted.
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
			if(radioInsBillingProvSpecific.Checked && _selectedProvBillNum==0){
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
			ClinicCur.IsInsVerifyExcluded=checkExcludeFromInsVerifyList.Checked;
			ClinicCur.Description=textDescription.Text;
			ClinicCur.Phone=phone;
			ClinicCur.Fax=fax;
			ClinicCur.Address=textAddress.Text;
			ClinicCur.Address2=textAddress2.Text;
			ClinicCur.City=textCity.Text;
			ClinicCur.State=textState.Text;
			ClinicCur.Zip=textZip.Text;
			ClinicCur.BillingAddress=textBillingAddress.Text;
			ClinicCur.BillingAddress2=textBillingAddress2.Text;
			ClinicCur.BillingCity=textBillingCity.Text;
			ClinicCur.BillingState=textBillingST.Text;
			ClinicCur.BillingZip=textBillingZip.Text;
			ClinicCur.PayToAddress=textPayToAddress.Text;
			ClinicCur.PayToAddress2=textPayToAddress2.Text;
			ClinicCur.PayToCity=textPayToCity.Text;
			ClinicCur.PayToState=textPayToST.Text;
			ClinicCur.PayToZip=textPayToZip.Text;
			ClinicCur.BankNumber=textBankNumber.Text;
			ClinicCur.DefaultPlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			ClinicCur.UseBillAddrOnClaims=checkUseBillingAddressOnClaims.Checked;
			long defNumRegion=0;
			if(comboRegion.SelectedIndex>0){
				defNumRegion=DefC.Short[(int)DefCat.Regions][comboRegion.SelectedIndex-1].DefNum;
			}
			ClinicCur.Region=defNumRegion;
			if(radioInsBillingProvDefault.Checked){//default=0
				ClinicCur.InsBillingProv=0;
			}
			else if(radioInsBillingProvTreat.Checked){//treat=-1
				ClinicCur.InsBillingProv=-1;
			}
			else{
				ClinicCur.InsBillingProv=_selectedProvBillNum;
			}
			ClinicCur.DefaultProv=_selectedProvDefNum;
			if(IsNew) {
				//for every new clinic, insert a set of program properties for PayConnect with the values from
				//the 'Headquarters' or ClinicNum=0 set of properties
				List<ProgramProperty> listProps=ProgramProperties.GetListForProgramAndClinic(Programs.GetProgramNum(ProgramName.PayConnect),0);
				//Add the program props for X-Charge for the HQ or practice level clinic (ClinicNum=0) to the list of props to duplicate for the new clinic
				listProps.AddRange(ProgramProperties.GetListForProgramAndClinic(Programs.GetProgramNum(ProgramName.Xcharge),0));//doesn't include path override
				for(int i=0;i<listProps.Count;i++) {
					listProps[i].ClinicNum=ClinicCur.ClinicNum;
					ProgramProperties.Insert(listProps[i]);//copy all values from the 0 clinic except ClinicNum and the primary key, insert will assign pri key
				}
				DataValid.SetInvalid(InvalidType.Programs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















