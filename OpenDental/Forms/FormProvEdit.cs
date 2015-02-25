using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormProvEdit : System.Windows.Forms.Form{
		private System.Windows.Forms.Label labelColor;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox checkIsSecondary;
		private System.Windows.Forms.ListBox listFeeSched;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textAbbr;
		private System.Windows.Forms.TextBox textStateLicense;
		private System.Windows.Forms.TextBox textSSN;
		private System.Windows.Forms.TextBox textMI;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textDEANum;
		private System.ComponentModel.Container components = null;// Required designer variable.
		private System.Windows.Forms.CheckBox checkIsHidden;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.ListBox listSpecialty;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioSSN;
		private System.Windows.Forms.RadioButton radioTIN;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textMedicaidID;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.TableProvIdent tbProvIdent;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.CheckBox checkSigOnFile;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Button butOutlineColor;
		private System.Windows.Forms.TextBox textSuffix;
		private System.Windows.Forms.ComboBox comboSchoolClass;
		private System.Windows.Forms.Label labelSchoolClass;
		///<summary>Provider Identifiers showing in the list for this provider.</summary>
		private ProviderIdent[] ListProvIdent;
		private System.Windows.Forms.Label labelNPI;
		private System.Windows.Forms.TextBox textNationalProvID;
		private TextBox textCanadianOfficeNum;
		private Label labelCanadianOfficeNum;
		private GroupBox groupAnesthProvType;
		private Label labelAnesthProvs;
		private RadioButton radAsstCirc;
		private RadioButton radAnesthSurg;
		private RadioButton radNone;
		private Label label4;
		private TextBox textTaxonomyOverride;
		private CheckBox checkIsCDAnet;
		private TextBox textEcwID;
		private Label labelEcwID;
		private TextBox textStateRxID;
		private Label label12;
		private CheckBox checkIsNotPerson;
		private Label label15;
		private TextBox textStateWhereLicensed;
		private CheckBox checkIsInstructor;
		private GroupBox groupDentalSchools;
		private TextBox textUserName;
		private TextBox textPassword;
		private TextBox textProvNum;
		private Label label17;
		private Label label16;
		private Label label18;
		public Provider ProvCur;
		private Label labelEhrMU;
		private ComboBox comboEhrMu;
		private Label labelPassDescription;
		private Userod _existingUser;

		///<summary></summary>
		public FormProvEdit(){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			//ProvCur=provCur;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				labelNPI.Text=Lan.g(this,"CDA Number");
			}
			else{
				labelCanadianOfficeNum.Visible=false;
				textCanadianOfficeNum.Visible=false;
			}
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProvEdit));
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.labelColor = new System.Windows.Forms.Label();
			this.butColor = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.checkIsSecondary = new System.Windows.Forms.CheckBox();
			this.listFeeSched = new System.Windows.Forms.ListBox();
			this.listSpecialty = new System.Windows.Forms.ListBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textAbbr = new System.Windows.Forms.TextBox();
			this.textStateLicense = new System.Windows.Forms.TextBox();
			this.textSSN = new System.Windows.Forms.TextBox();
			this.textSuffix = new System.Windows.Forms.TextBox();
			this.textMI = new System.Windows.Forms.TextBox();
			this.textFName = new System.Windows.Forms.TextBox();
			this.textLName = new System.Windows.Forms.TextBox();
			this.textDEANum = new System.Windows.Forms.TextBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioTIN = new System.Windows.Forms.RadioButton();
			this.radioSSN = new System.Windows.Forms.RadioButton();
			this.checkSigOnFile = new System.Windows.Forms.CheckBox();
			this.textMedicaidID = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tbProvIdent = new OpenDental.TableProvIdent();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.butOutlineColor = new System.Windows.Forms.Button();
			this.comboSchoolClass = new System.Windows.Forms.ComboBox();
			this.labelSchoolClass = new System.Windows.Forms.Label();
			this.textNationalProvID = new System.Windows.Forms.TextBox();
			this.labelNPI = new System.Windows.Forms.Label();
			this.textCanadianOfficeNum = new System.Windows.Forms.TextBox();
			this.labelCanadianOfficeNum = new System.Windows.Forms.Label();
			this.groupAnesthProvType = new System.Windows.Forms.GroupBox();
			this.radAsstCirc = new System.Windows.Forms.RadioButton();
			this.radAnesthSurg = new System.Windows.Forms.RadioButton();
			this.radNone = new System.Windows.Forms.RadioButton();
			this.labelAnesthProvs = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textTaxonomyOverride = new System.Windows.Forms.TextBox();
			this.checkIsCDAnet = new System.Windows.Forms.CheckBox();
			this.textEcwID = new System.Windows.Forms.TextBox();
			this.labelEcwID = new System.Windows.Forms.Label();
			this.textStateRxID = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.checkIsNotPerson = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.textStateWhereLicensed = new System.Windows.Forms.TextBox();
			this.checkIsInstructor = new System.Windows.Forms.CheckBox();
			this.groupDentalSchools = new System.Windows.Forms.GroupBox();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.textProvNum = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.labelEhrMU = new System.Windows.Forms.Label();
			this.comboEhrMu = new System.Windows.Forms.ComboBox();
			this.labelPassDescription = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupAnesthProvType.SuspendLayout();
			this.groupDentalSchools.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(371, 435);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(158, 17);
			this.checkIsHidden.TabIndex = 22;
			this.checkIsHidden.Text = "Hidden";
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(14, 358);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(140, 16);
			this.labelColor.TabIndex = 10;
			this.labelColor.Text = "Appointment Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(155, 355);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 13;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(17, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 14);
			this.label1.TabIndex = 12;
			this.label1.Text = "Abbreviation";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 220);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(142, 14);
			this.label3.TabIndex = 14;
			this.label3.Text = "State License Number";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(526, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(116, 14);
			this.label5.TabIndex = 16;
			this.label5.Text = "Specialty";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(369, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(105, 14);
			this.label6.TabIndex = 17;
			this.label6.Text = "Fee Schedule";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(46, 92);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 14);
			this.label7.TabIndex = 18;
			this.label7.Text = "MI";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(15, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(138, 14);
			this.label8.TabIndex = 19;
			this.label8.Text = "First Name";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 112);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(147, 14);
			this.label9.TabIndex = 20;
			this.label9.Text = "Suffix (DMD,DDS,Jr,etc)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(9, 52);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(143, 14);
			this.label10.TabIndex = 21;
			this.label10.Text = "Last Name";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(15, 259);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(137, 14);
			this.label11.TabIndex = 22;
			this.label11.Text = "DEA Number";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsSecondary
			// 
			this.checkIsSecondary.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsSecondary.Location = new System.Drawing.Point(371, 381);
			this.checkIsSecondary.Name = "checkIsSecondary";
			this.checkIsSecondary.Size = new System.Drawing.Size(155, 17);
			this.checkIsSecondary.TabIndex = 19;
			this.checkIsSecondary.Text = "Secondary Provider (Hyg)";
			// 
			// listFeeSched
			// 
			this.listFeeSched.Location = new System.Drawing.Point(371, 25);
			this.listFeeSched.Name = "listFeeSched";
			this.listFeeSched.Size = new System.Drawing.Size(108, 173);
			this.listFeeSched.TabIndex = 13;
			// 
			// listSpecialty
			// 
			this.listSpecialty.Items.AddRange(new object[] {
            "Dental General Practice",
            "Dental Hygienist",
            "Endodontics",
            "Pediatric Dentistry",
            "Periodontics",
            "Prosthodontics",
            "Orthodontics",
            "Denturist",
            "Surgery, Oral & Maxillofacial",
            "Dental Assistant",
            "Dental Laboratory Technician",
            "Pathology, Oral & MaxFac",
            "Public Health",
            "Radiology"});
			this.listSpecialty.Location = new System.Drawing.Point(526, 25);
			this.listSpecialty.Name = "listSpecialty";
			this.listSpecialty.Size = new System.Drawing.Size(154, 186);
			this.listSpecialty.TabIndex = 17;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(635, 592);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 25;
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(724, 592);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 26;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textAbbr
			// 
			this.textAbbr.Location = new System.Drawing.Point(154, 28);
			this.textAbbr.MaxLength = 255;
			this.textAbbr.Name = "textAbbr";
			this.textAbbr.Size = new System.Drawing.Size(121, 20);
			this.textAbbr.TabIndex = 0;
			// 
			// textStateLicense
			// 
			this.textStateLicense.Location = new System.Drawing.Point(154, 216);
			this.textStateLicense.MaxLength = 15;
			this.textStateLicense.Name = "textStateLicense";
			this.textStateLicense.Size = new System.Drawing.Size(100, 20);
			this.textStateLicense.TabIndex = 6;
			// 
			// textSSN
			// 
			this.textSSN.Location = new System.Drawing.Point(8, 52);
			this.textSSN.Name = "textSSN";
			this.textSSN.Size = new System.Drawing.Size(100, 20);
			this.textSSN.TabIndex = 2;
			// 
			// textSuffix
			// 
			this.textSuffix.Location = new System.Drawing.Point(154, 108);
			this.textSuffix.MaxLength = 100;
			this.textSuffix.Name = "textSuffix";
			this.textSuffix.Size = new System.Drawing.Size(104, 20);
			this.textSuffix.TabIndex = 4;
			// 
			// textMI
			// 
			this.textMI.Location = new System.Drawing.Point(154, 88);
			this.textMI.MaxLength = 100;
			this.textMI.Name = "textMI";
			this.textMI.Size = new System.Drawing.Size(63, 20);
			this.textMI.TabIndex = 3;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(154, 68);
			this.textFName.MaxLength = 100;
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(161, 20);
			this.textFName.TabIndex = 2;
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(154, 48);
			this.textLName.MaxLength = 100;
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(161, 20);
			this.textLName.TabIndex = 1;
			// 
			// textDEANum
			// 
			this.textDEANum.Location = new System.Drawing.Point(154, 255);
			this.textDEANum.MaxLength = 15;
			this.textDEANum.Name = "textDEANum";
			this.textDEANum.Size = new System.Drawing.Size(100, 20);
			this.textDEANum.TabIndex = 8;
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioTIN);
			this.groupBox1.Controls.Add(this.radioSSN);
			this.groupBox1.Controls.Add(this.textSSN);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(146, 133);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(156, 80);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "SSN or TIN (no dashes)";
			// 
			// radioTIN
			// 
			this.radioTIN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioTIN.Location = new System.Drawing.Point(9, 34);
			this.radioTIN.Name = "radioTIN";
			this.radioTIN.Size = new System.Drawing.Size(135, 15);
			this.radioTIN.TabIndex = 1;
			this.radioTIN.Text = "TIN";
			this.radioTIN.Click += new System.EventHandler(this.radioTIN_Click);
			// 
			// radioSSN
			// 
			this.radioSSN.Checked = true;
			this.radioSSN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioSSN.Location = new System.Drawing.Point(9, 17);
			this.radioSSN.Name = "radioSSN";
			this.radioSSN.Size = new System.Drawing.Size(104, 14);
			this.radioSSN.TabIndex = 0;
			this.radioSSN.TabStop = true;
			this.radioSSN.Text = "SSN";
			this.radioSSN.Click += new System.EventHandler(this.radioSSN_Click);
			// 
			// checkSigOnFile
			// 
			this.checkSigOnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSigOnFile.Location = new System.Drawing.Point(371, 399);
			this.checkSigOnFile.Name = "checkSigOnFile";
			this.checkSigOnFile.Size = new System.Drawing.Size(121, 17);
			this.checkSigOnFile.TabIndex = 20;
			this.checkSigOnFile.Text = "Signature on File";
			// 
			// textMedicaidID
			// 
			this.textMedicaidID.Location = new System.Drawing.Point(154, 295);
			this.textMedicaidID.MaxLength = 20;
			this.textMedicaidID.Name = "textMedicaidID";
			this.textMedicaidID.Size = new System.Drawing.Size(100, 20);
			this.textMedicaidID.TabIndex = 10;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(12, 299);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(141, 14);
			this.label13.TabIndex = 42;
			this.label13.Text = "Medicaid ID";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tbProvIdent
			// 
			this.tbProvIdent.BackColor = System.Drawing.SystemColors.Window;
			this.tbProvIdent.Location = new System.Drawing.Point(7, 58);
			this.tbProvIdent.Name = "tbProvIdent";
			this.tbProvIdent.ScrollValue = 211;
			this.tbProvIdent.SelectedIndices = new int[0];
			this.tbProvIdent.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbProvIdent.Size = new System.Drawing.Size(319, 88);
			this.tbProvIdent.TabIndex = 43;
			this.tbProvIdent.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbProvIdent_CellDoubleClicked);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butAdd);
			this.groupBox2.Controls.Add(this.butDelete);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.tbProvIdent);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(33, 451);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(496, 157);
			this.groupBox2.TabIndex = 23;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Supplemental Provider Identifiers";
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(358, 59);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(90, 24);
			this.butAdd.TabIndex = 0;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(358, 94);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(90, 24);
			this.butDelete.TabIndex = 1;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(481, 32);
			this.label2.TabIndex = 44;
			this.label2.Text = "This is where you store provider IDs assigned by individual insurance companies, " +
    "especially BC/BS.";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(14, 378);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(140, 16);
			this.label14.TabIndex = 45;
			this.label14.Text = "Highlight Outline Color";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOutlineColor
			// 
			this.butOutlineColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butOutlineColor.Location = new System.Drawing.Point(155, 375);
			this.butOutlineColor.Name = "butOutlineColor";
			this.butOutlineColor.Size = new System.Drawing.Size(30, 20);
			this.butOutlineColor.TabIndex = 14;
			this.butOutlineColor.Click += new System.EventHandler(this.butOutlineColor_Click);
			// 
			// comboSchoolClass
			// 
			this.comboSchoolClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSchoolClass.Location = new System.Drawing.Point(101, 20);
			this.comboSchoolClass.MaxDropDownItems = 30;
			this.comboSchoolClass.Name = "comboSchoolClass";
			this.comboSchoolClass.Size = new System.Drawing.Size(158, 21);
			this.comboSchoolClass.TabIndex = 0;
			this.comboSchoolClass.Visible = false;
			// 
			// labelSchoolClass
			// 
			this.labelSchoolClass.Location = new System.Drawing.Point(8, 21);
			this.labelSchoolClass.Name = "labelSchoolClass";
			this.labelSchoolClass.Size = new System.Drawing.Size(93, 16);
			this.labelSchoolClass.TabIndex = 89;
			this.labelSchoolClass.Text = "Class";
			this.labelSchoolClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSchoolClass.Visible = false;
			// 
			// textNationalProvID
			// 
			this.textNationalProvID.Location = new System.Drawing.Point(154, 315);
			this.textNationalProvID.MaxLength = 20;
			this.textNationalProvID.Name = "textNationalProvID";
			this.textNationalProvID.Size = new System.Drawing.Size(100, 20);
			this.textNationalProvID.TabIndex = 11;
			// 
			// labelNPI
			// 
			this.labelNPI.Location = new System.Drawing.Point(12, 319);
			this.labelNPI.Name = "labelNPI";
			this.labelNPI.Size = new System.Drawing.Size(141, 14);
			this.labelNPI.TabIndex = 92;
			this.labelNPI.Text = "National Provider ID";
			this.labelNPI.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCanadianOfficeNum
			// 
			this.textCanadianOfficeNum.Location = new System.Drawing.Point(154, 335);
			this.textCanadianOfficeNum.MaxLength = 20;
			this.textCanadianOfficeNum.Name = "textCanadianOfficeNum";
			this.textCanadianOfficeNum.Size = new System.Drawing.Size(100, 20);
			this.textCanadianOfficeNum.TabIndex = 12;
			// 
			// labelCanadianOfficeNum
			// 
			this.labelCanadianOfficeNum.Location = new System.Drawing.Point(12, 339);
			this.labelCanadianOfficeNum.Name = "labelCanadianOfficeNum";
			this.labelCanadianOfficeNum.Size = new System.Drawing.Size(141, 14);
			this.labelCanadianOfficeNum.TabIndex = 94;
			this.labelCanadianOfficeNum.Text = "Office Number";
			this.labelCanadianOfficeNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupAnesthProvType
			// 
			this.groupAnesthProvType.Controls.Add(this.radAsstCirc);
			this.groupAnesthProvType.Controls.Add(this.radAnesthSurg);
			this.groupAnesthProvType.Controls.Add(this.radNone);
			this.groupAnesthProvType.Controls.Add(this.labelAnesthProvs);
			this.groupAnesthProvType.Location = new System.Drawing.Point(359, 264);
			this.groupAnesthProvType.Name = "groupAnesthProvType";
			this.groupAnesthProvType.Size = new System.Drawing.Size(347, 83);
			this.groupAnesthProvType.TabIndex = 17;
			this.groupAnesthProvType.TabStop = false;
			this.groupAnesthProvType.Text = "Anesthesia Provider Groups (optional)";
			// 
			// radAsstCirc
			// 
			this.radAsstCirc.AutoSize = true;
			this.radAsstCirc.Location = new System.Drawing.Point(12, 56);
			this.radAsstCirc.Name = "radAsstCirc";
			this.radAsstCirc.Size = new System.Drawing.Size(116, 17);
			this.radAsstCirc.TabIndex = 9;
			this.radAsstCirc.Text = "Assistant/Circulator";
			this.radAsstCirc.UseVisualStyleBackColor = true;
			// 
			// radAnesthSurg
			// 
			this.radAnesthSurg.AutoSize = true;
			this.radAnesthSurg.Location = new System.Drawing.Point(12, 37);
			this.radAnesthSurg.Name = "radAnesthSurg";
			this.radAnesthSurg.Size = new System.Drawing.Size(122, 17);
			this.radAnesthSurg.TabIndex = 8;
			this.radAnesthSurg.Text = "Anesthetist/Surgeon";
			this.radAnesthSurg.UseVisualStyleBackColor = true;
			// 
			// radNone
			// 
			this.radNone.AutoSize = true;
			this.radNone.Checked = true;
			this.radNone.Location = new System.Drawing.Point(12, 18);
			this.radNone.Name = "radNone";
			this.radNone.Size = new System.Drawing.Size(51, 17);
			this.radNone.TabIndex = 7;
			this.radNone.TabStop = true;
			this.radNone.Text = "None";
			this.radNone.UseVisualStyleBackColor = true;
			// 
			// labelAnesthProvs
			// 
			this.labelAnesthProvs.Location = new System.Drawing.Point(153, 22);
			this.labelAnesthProvs.Name = "labelAnesthProvs";
			this.labelAnesthProvs.Size = new System.Drawing.Size(188, 52);
			this.labelAnesthProvs.TabIndex = 4;
			this.labelAnesthProvs.Text = "Assign this user to a group. This will populate the corresponding dropdowns on th" +
    "e Anesthetic Record.";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(523, 215);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 14);
			this.label4.TabIndex = 96;
			this.label4.Text = "Taxonomy Code Override";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textTaxonomyOverride
			// 
			this.textTaxonomyOverride.Location = new System.Drawing.Point(526, 231);
			this.textTaxonomyOverride.MaxLength = 255;
			this.textTaxonomyOverride.Name = "textTaxonomyOverride";
			this.textTaxonomyOverride.Size = new System.Drawing.Size(154, 20);
			this.textTaxonomyOverride.TabIndex = 16;
			// 
			// checkIsCDAnet
			// 
			this.checkIsCDAnet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsCDAnet.Location = new System.Drawing.Point(371, 363);
			this.checkIsCDAnet.Name = "checkIsCDAnet";
			this.checkIsCDAnet.Size = new System.Drawing.Size(168, 17);
			this.checkIsCDAnet.TabIndex = 18;
			this.checkIsCDAnet.Text = "Is CDAnet Member";
			this.checkIsCDAnet.Visible = false;
			// 
			// textEcwID
			// 
			this.textEcwID.Location = new System.Drawing.Point(154, 8);
			this.textEcwID.MaxLength = 255;
			this.textEcwID.Name = "textEcwID";
			this.textEcwID.ReadOnly = true;
			this.textEcwID.Size = new System.Drawing.Size(121, 20);
			this.textEcwID.TabIndex = 100;
			// 
			// labelEcwID
			// 
			this.labelEcwID.Location = new System.Drawing.Point(17, 12);
			this.labelEcwID.Name = "labelEcwID";
			this.labelEcwID.Size = new System.Drawing.Size(136, 14);
			this.labelEcwID.TabIndex = 101;
			this.labelEcwID.Text = "eCW ID";
			this.labelEcwID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStateRxID
			// 
			this.textStateRxID.Location = new System.Drawing.Point(154, 275);
			this.textStateRxID.MaxLength = 15;
			this.textStateRxID.Name = "textStateRxID";
			this.textStateRxID.Size = new System.Drawing.Size(100, 20);
			this.textStateRxID.TabIndex = 9;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(15, 279);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(137, 14);
			this.label12.TabIndex = 106;
			this.label12.Text = "State Rx ID";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsNotPerson
			// 
			this.checkIsNotPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsNotPerson.Location = new System.Drawing.Point(371, 417);
			this.checkIsNotPerson.Name = "checkIsNotPerson";
			this.checkIsNotPerson.Size = new System.Drawing.Size(410, 17);
			this.checkIsNotPerson.TabIndex = 21;
			this.checkIsNotPerson.Text = "Not a Person (for example, a dummy provider representing the organization)";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(13, 239);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(139, 14);
			this.label15.TabIndex = 109;
			this.label15.Text = "State Where Licensed";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStateWhereLicensed
			// 
			this.textStateWhereLicensed.Location = new System.Drawing.Point(154, 235);
			this.textStateWhereLicensed.MaxLength = 15;
			this.textStateWhereLicensed.Name = "textStateWhereLicensed";
			this.textStateWhereLicensed.Size = new System.Drawing.Size(34, 20);
			this.textStateWhereLicensed.TabIndex = 7;
			// 
			// checkIsInstructor
			// 
			this.checkIsInstructor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsInstructor.Enabled = false;
			this.checkIsInstructor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsInstructor.Location = new System.Drawing.Point(7, 104);
			this.checkIsInstructor.Name = "checkIsInstructor";
			this.checkIsInstructor.Size = new System.Drawing.Size(107, 17);
			this.checkIsInstructor.TabIndex = 4;
			this.checkIsInstructor.Text = "Is Instructor";
			this.checkIsInstructor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupDentalSchools
			// 
			this.groupDentalSchools.Controls.Add(this.labelPassDescription);
			this.groupDentalSchools.Controls.Add(this.textUserName);
			this.groupDentalSchools.Controls.Add(this.textPassword);
			this.groupDentalSchools.Controls.Add(this.textProvNum);
			this.groupDentalSchools.Controls.Add(this.label17);
			this.groupDentalSchools.Controls.Add(this.label16);
			this.groupDentalSchools.Controls.Add(this.label18);
			this.groupDentalSchools.Controls.Add(this.labelSchoolClass);
			this.groupDentalSchools.Controls.Add(this.checkIsInstructor);
			this.groupDentalSchools.Controls.Add(this.comboSchoolClass);
			this.groupDentalSchools.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupDentalSchools.Location = new System.Drawing.Point(535, 451);
			this.groupDentalSchools.Name = "groupDentalSchools";
			this.groupDentalSchools.Size = new System.Drawing.Size(264, 135);
			this.groupDentalSchools.TabIndex = 24;
			this.groupDentalSchools.TabStop = false;
			this.groupDentalSchools.Text = "Dental Schools";
			this.groupDentalSchools.Visible = false;
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(101, 62);
			this.textUserName.MaxLength = 100;
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(157, 20);
			this.textUserName.TabIndex = 2;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(101, 82);
			this.textPassword.MaxLength = 100;
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(157, 20);
			this.textPassword.TabIndex = 3;
			// 
			// textProvNum
			// 
			this.textProvNum.Location = new System.Drawing.Point(101, 42);
			this.textProvNum.MaxLength = 15;
			this.textProvNum.Name = "textProvNum";
			this.textProvNum.ReadOnly = true;
			this.textProvNum.Size = new System.Drawing.Size(157, 20);
			this.textProvNum.TabIndex = 1;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(5, 66);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(95, 14);
			this.label17.TabIndex = 116;
			this.label17.Text = "User Name";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(31, 45);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(69, 14);
			this.label16.TabIndex = 112;
			this.label16.Text = "ProvNum";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(5, 86);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(95, 14);
			this.label18.TabIndex = 115;
			this.label18.Text = "Password";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelEhrMU
			// 
			this.labelEhrMU.Location = new System.Drawing.Point(9, 397);
			this.labelEhrMU.Name = "labelEhrMU";
			this.labelEhrMU.Size = new System.Drawing.Size(144, 16);
			this.labelEhrMU.TabIndex = 114;
			this.labelEhrMU.Text = "EHR Meaningful Use";
			this.labelEhrMU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboEhrMu
			// 
			this.comboEhrMu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEhrMu.Location = new System.Drawing.Point(155, 396);
			this.comboEhrMu.MaxDropDownItems = 30;
			this.comboEhrMu.Name = "comboEhrMu";
			this.comboEhrMu.Size = new System.Drawing.Size(100, 21);
			this.comboEhrMu.TabIndex = 15;
			// 
			// labelPassDescription
			// 
			this.labelPassDescription.Location = new System.Drawing.Point(120, 105);
			this.labelPassDescription.Name = "labelPassDescription";
			this.labelPassDescription.Size = new System.Drawing.Size(138, 27);
			this.labelPassDescription.TabIndex = 248;
			this.labelPassDescription.Text = "To keep the old password, leave the box empty.";
			this.labelPassDescription.Visible = false;
			// 
			// FormProvEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(811, 628);
			this.Controls.Add(this.labelEhrMU);
			this.Controls.Add(this.comboEhrMu);
			this.Controls.Add(this.groupDentalSchools);
			this.Controls.Add(this.textStateWhereLicensed);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.checkIsNotPerson);
			this.Controls.Add(this.textStateRxID);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textEcwID);
			this.Controls.Add(this.labelEcwID);
			this.Controls.Add(this.checkIsCDAnet);
			this.Controls.Add(this.textTaxonomyOverride);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupAnesthProvType);
			this.Controls.Add(this.textCanadianOfficeNum);
			this.Controls.Add(this.labelCanadianOfficeNum);
			this.Controls.Add(this.textNationalProvID);
			this.Controls.Add(this.labelNPI);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.butOutlineColor);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textMedicaidID);
			this.Controls.Add(this.textDEANum);
			this.Controls.Add(this.textLName);
			this.Controls.Add(this.textFName);
			this.Controls.Add(this.textMI);
			this.Controls.Add(this.textSuffix);
			this.Controls.Add(this.textStateLicense);
			this.Controls.Add(this.textAbbr);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.checkSigOnFile);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.listSpecialty);
			this.Controls.Add(this.listFeeSched);
			this.Controls.Add(this.checkIsSecondary);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.labelColor);
			this.Controls.Add(this.butColor);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(827, 666);
			this.Name = "FormProvEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Provider";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProvEdit_Closing);
			this.Load += new System.EventHandler(this.FormProvEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupAnesthProvType.ResumeLayout(false);
			this.groupAnesthProvType.PerformLayout();
			this.groupDentalSchools.ResumeLayout(false);
			this.groupDentalSchools.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormProvEdit_Load(object sender, System.EventArgs e) {
			//if(IsNew){
			//	Providers.Cur.SigOnFile=true;
			//	Providers.InsertCur();
				//one field handled from previous form
			//}
			comboEhrMu.Items.Add("Use Global");
			comboEhrMu.Items.Add("Stage 1");
			comboEhrMu.Items.Add("Stage 2");
			comboEhrMu.SelectedIndex=ProvCur.EhrMuStage;
			if(!PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				comboEhrMu.Visible=false;
				labelEhrMU.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) //Dental Schools is turned on
				&& (ProvCur.SchoolClassNum!=0 || ProvCur.IsInstructor))//Adding/Editing Students or Instructors
			{
				groupDentalSchools.Visible=true;
				if(!ProvCur.IsNew) {
					labelPassDescription.Visible=true;
					textProvNum.Text=ProvCur.ProvNum.ToString();
					List<Userod> userList=Providers.GetAttachedUsers(ProvCur.ProvNum);
					if(userList.Count>0) {
						textUserName.Text=userList[0].UserName;//Should always happen if they are a student.
						_existingUser=userList[0];
					}
				}
				else {
					textUserName.Text=Providers.GetNextAvailableProvNum().ToString();//User-names are suggested to be the ProvNum of the provider.  This can be changed at will.
				}
				for(int i=0;i<SchoolClasses.List.Length;i++) {
					comboSchoolClass.Items.Add(SchoolClasses.List[i].GradYear.ToString()+"-"+SchoolClasses.List[i].Descript);
					comboSchoolClass.SelectedIndex=0;
					if(SchoolClasses.List[i].SchoolClassNum==ProvCur.SchoolClassNum) {
						comboSchoolClass.SelectedIndex=i;
					}
				}
				if(ProvCur.SchoolClassNum!=0) {
					labelSchoolClass.Visible=true;
					comboSchoolClass.Visible=true;
				}
			}
			if(Programs.IsEnabled(ProgramName.eClinicalWorks)) {
				textEcwID.Text=ProvCur.EcwID;
			}
			else{
				labelEcwID.Visible=false;
				textEcwID.Visible=false;
			}
			List<EhrProvKey> listProvKey=EhrProvKeys.GetKeysByFLName(ProvCur.LName,ProvCur.FName);
			if(listProvKey.Count>0) {
				textLName.Enabled=false;
				textFName.Enabled=false;
			}
			else{
				textLName.Enabled=true;
				textFName.Enabled=true;
			}
			//We'll just always show the Anesthesia fields since they are part of the standard database.
			textAbbr.Text=ProvCur.Abbr;
			textLName.Text=ProvCur.LName;
			textFName.Text=ProvCur.FName;
			textMI.Text=ProvCur.MI;
			textSuffix.Text=ProvCur.Suffix;
			textSSN.Text=ProvCur.SSN;
			if(ProvCur.UsingTIN){
				radioTIN.Checked=true;
			}
			else {
				radioSSN.Checked=true;
			}
			textStateLicense.Text=ProvCur.StateLicense;
			textStateWhereLicensed.Text=ProvCur.StateWhereLicensed;
			textDEANum.Text=ProvCur.DEANum;
			textStateRxID.Text=ProvCur.StateRxID;
			//textBlueCrossID.Text=ProvCur.BlueCrossID;
			textMedicaidID.Text=ProvCur.MedicaidID;
			textNationalProvID.Text=ProvCur.NationalProvID;
			textCanadianOfficeNum.Text=ProvCur.CanadianOfficeNum;
			checkIsSecondary.Checked=ProvCur.IsSecondary;
			checkSigOnFile.Checked=ProvCur.SigOnFile;
			checkIsHidden.Checked=ProvCur.IsHidden;
			checkIsInstructor.Checked=ProvCur.IsInstructor;
			butColor.BackColor=ProvCur.ProvColor;
			butOutlineColor.BackColor=ProvCur.OutlineColor;
			for(int i=0;i<FeeSchedC.ListShort.Count;i++){
				this.listFeeSched.Items.Add(FeeSchedC.ListShort[i].Description);
				if(FeeSchedC.ListShort[i].FeeSchedNum==ProvCur.FeeSched){
					listFeeSched.SelectedIndex=i;
				}
			}
			if(listFeeSched.SelectedIndex<0){
				listFeeSched.SelectedIndex=0;
			}
			listSpecialty.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(DentalSpecialty)).Length;i++){
				listSpecialty.Items.Add(Lan.g("enumDentalSpecialty",Enum.GetNames(typeof(DentalSpecialty))[i]));
			}
			listSpecialty.SelectedIndex=(int)ProvCur.Specialty;
			textTaxonomyOverride.Text=ProvCur.TaxonomyCodeOverride;
			FillProvIdent();
			//These radio buttons are used to properly filter the provider dropdowns on FormAnetheticRecord
			if (ProvCur.AnesthProvType == 0)
				{
					radNone.Checked = true;
				}
			
			if (ProvCur.AnesthProvType == 1)
				{
					radAnesthSurg.Checked = true;
				}

			if (ProvCur.AnesthProvType == 2)
			{
				radAsstCirc.Checked = true;
			}
			checkIsCDAnet.Checked=ProvCur.IsCDAnet;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				checkIsCDAnet.Visible=true;
			}
			checkIsNotPerson.Checked=ProvCur.IsNotPerson;
		}

		private void butColor_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butOutlineColor_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butOutlineColor.BackColor;
			colorDialog1.ShowDialog();
			butOutlineColor.BackColor=colorDialog1.Color;
		}

		private void radioSSN_Click(object sender, System.EventArgs e) {
			ProvCur.UsingTIN=false;
		}

		private void radioTIN_Click(object sender, System.EventArgs e) {
			ProvCur.UsingTIN=true;
		}

		private void FillProvIdent(){
			ProviderIdents.RefreshCache();
			ListProvIdent=ProviderIdents.GetForProv(ProvCur.ProvNum);
			tbProvIdent.ResetRows(ListProvIdent.Length);
			tbProvIdent.SetGridColor(Color.Gray);
			for(int i=0;i<ListProvIdent.Length;i++){
				tbProvIdent.Cell[0,i]=ListProvIdent[i].PayorID;
				tbProvIdent.Cell[1,i]=ListProvIdent[i].SuppIDType.ToString();
				tbProvIdent.Cell[2,i]=ListProvIdent[i].IDNumber;
			}
			tbProvIdent.LayoutTables();
		}

		private void tbProvIdent_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			FormProviderIdentEdit FormP=new FormProviderIdentEdit();
			FormP.ProvIdentCur=ListProvIdent[e.Row];
			FormP.ShowDialog();
			FillProvIdent();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProviderIdentEdit FormP=new FormProviderIdentEdit();
			FormP.ProvIdentCur=new ProviderIdent();
			FormP.ProvIdentCur.ProvNum=ProvCur.ProvNum;
			FormP.IsNew=true;
			FormP.ShowDialog();
			FillProvIdent();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(tbProvIdent.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete the selected Provider Identifier?"),"",
				MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return;
			}
			ProviderIdents.Delete(ListProvIdent[tbProvIdent.SelectedRow]);
			FillProvIdent();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(textAbbr.Text=="") {
				MessageBox.Show(Lan.g(this,"Abbreviation not allowed to be blank."));
				return;
			}
			if(textSSN.Text.Contains("-")) {
				MsgBox.Show(this,"SSN/TIN not allowed to have dash.");
				return;
			}
			if(checkIsHidden.Checked && PrefC.GetLong(PrefName.PracticeDefaultProv)==ProvCur.ProvNum) {
				MsgBox.Show(this,"Not allowed to hide practice default provider.");
				return;
			}
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].ProvNum==ProvCur.ProvNum) {
					continue;
				}
				if(ProviderC.ListLong[i].Abbr==textAbbr.Text && PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					if(!MsgBox.Show(this,true,"This abbreviation is already in use by another provider.  Continue anyway?")) {
						return;
					}
				}
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && checkIsCDAnet.Checked) {
				if(textNationalProvID.Text!=Eclaims.Canadian.TidyAN(textNationalProvID.Text,9,true)) {
					MsgBox.Show(this,"CDA number must be 9 characters long and composed of numbers and letters only.");
					return;
				}
				if(textCanadianOfficeNum.Text!=Eclaims.Canadian.TidyAN(textCanadianOfficeNum.Text,4,true)) {
					MsgBox.Show(this,"Office number must be 4 characters long and composed of numbers and letters only.");
					return;
				}
			}
			if(checkIsNotPerson.Checked) {
				if(textFName.Text!="" || textMI.Text!="") {
					MsgBox.Show(this,"When the 'Not a Person' box is checked, the provider may not have a First Name or Middle Initial entered.");
					return;
				}
			}
			if(checkIsHidden.Checked) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Any future schedule for this provider will be deleted.  Continue?")) {
					return;
				}
				Providers.RemoveProvFromFutureSchedule(ProvCur.ProvNum);
			}
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) && (ProvCur.IsInstructor || ProvCur.SchoolClassNum!=0)) {//Is an Instructor or a Student
				if(textUserName.Text=="") {
					MsgBox.Show(this,"User Name is not allowed to be blank.");
					return;
				}
			}
			ProvCur.Abbr=textAbbr.Text;
			ProvCur.LName=textLName.Text;
			ProvCur.FName=textFName.Text;
			ProvCur.MI=textMI.Text;
			ProvCur.Suffix=textSuffix.Text;
			ProvCur.SSN=textSSN.Text;
			ProvCur.StateLicense=textStateLicense.Text;
			ProvCur.StateWhereLicensed=textStateWhereLicensed.Text;
			ProvCur.DEANum=textDEANum.Text;
			ProvCur.StateRxID=textStateRxID.Text;
			//ProvCur.BlueCrossID=textBlueCrossID.Text;
			ProvCur.MedicaidID=textMedicaidID.Text;
			ProvCur.NationalProvID=textNationalProvID.Text;
			ProvCur.CanadianOfficeNum=textCanadianOfficeNum.Text;
			//EhrKey and EhrHasReportAccess set when user uses the ... button
			ProvCur.IsSecondary=checkIsSecondary.Checked;
			ProvCur.SigOnFile=checkSigOnFile.Checked;
			ProvCur.IsHidden=checkIsHidden.Checked;
			ProvCur.IsCDAnet=checkIsCDAnet.Checked;
			ProvCur.ProvColor=butColor.BackColor;
			ProvCur.OutlineColor=butOutlineColor.BackColor;
			ProvCur.IsInstructor=checkIsInstructor.Checked;
			ProvCur.EhrMuStage=comboEhrMu.SelectedIndex;
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				if(ProvCur.SchoolClassNum!=0) {
					ProvCur.SchoolClassNum=SchoolClasses.List[comboSchoolClass.SelectedIndex].SchoolClassNum;
				}
			}
			if(listFeeSched.SelectedIndex!=-1) {
				ProvCur.FeeSched=FeeSchedC.ListShort[listFeeSched.SelectedIndex].FeeSchedNum;
			}
			ProvCur.Specialty=(DentalSpecialty)listSpecialty.SelectedIndex;
			ProvCur.TaxonomyCodeOverride=textTaxonomyOverride.Text;
			if(radAnesthSurg.Checked) {
				ProvCur.AnesthProvType=1;
			}
			else if(radAsstCirc.Checked) {
				ProvCur.AnesthProvType=2;
			}
			else {
				ProvCur.AnesthProvType=0;
			}
			ProvCur.IsNotPerson=checkIsNotPerson.Checked;
			if(IsNew) {
				long provNum=Providers.Insert(ProvCur);
				if(ProvCur.IsInstructor) {
					Userod user=new Userod();
					user.UserName=textUserName.Text;
					user.Password=Userods.EncryptPassword(textPassword.Text);
					user.ProvNum=provNum;
					user.UserGroupNum=PrefC.GetLong(PrefName.SecurityGroupForInstructors);
					try {
						Userods.Insert(user);
					}
					catch(Exception ex) {
						Providers.Delete(ProvCur);
						MessageBox.Show(ex.Message);
						return;
					}
				}
			}
			else {
				try {
					if(_existingUser!=null && (ProvCur.IsInstructor || ProvCur.SchoolClassNum!=0)) {
						_existingUser.UserName=textUserName.Text;
						if(textPassword.Text!="") {
							_existingUser.Password=Userods.EncryptPassword(textPassword.Text);
						}
						Userods.Update(_existingUser);
					}
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				Providers.Update(ProvCur);
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProvEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK)
				return;
			if(IsNew){
				//UserPermissions.DeleteAllForProv(Providers.Cur.ProvNum);
				ProviderIdents.DeleteAllForProv(ProvCur.ProvNum);
				Providers.Delete(ProvCur);
			}
		}

	

	

		

	

		

		


	}
}




