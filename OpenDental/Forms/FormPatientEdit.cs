/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using System.Diagnostics;
using System.Linq;

namespace OpenDental{
///<summary></summary>
	public class FormPatientEdit : System.Windows.Forms.Form{
		private System.Windows.Forms.Label labelLName;
		private System.Windows.Forms.Label labelFName;
		private System.Windows.Forms.Label labelMiddleI;
		private System.Windows.Forms.Label labelPreferred;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label labelGender;
		private System.Windows.Forms.Label labelPosition;
		private System.Windows.Forms.Label labelBirthdate;
		private System.Windows.Forms.Label labelAddress;
		private System.Windows.Forms.Label labelAddress2;
		private System.Windows.Forms.Label labelHmPhone;
		private System.Windows.Forms.Label labelWkPhone;
		private System.Windows.Forms.Label labelWirelessPhone;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private IContainer components;// Required designer variable.
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textMiddleI;
		private System.Windows.Forms.TextBox textPreferred;
		private System.Windows.Forms.TextBox textSSN;
		private System.Windows.Forms.TextBox textAddress;
		private System.Windows.Forms.TextBox textAddress2;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.TextBox textState;
		private System.Windows.Forms.TextBox textHmPhone;
		private System.Windows.Forms.TextBox textWkPhone;
		private System.Windows.Forms.TextBox textWirelessPhone;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox textAge;
		private System.Windows.Forms.Label labelSalutation;
		private System.Windows.Forms.TextBox textSalutation;
		private System.Windows.Forms.TextBox textEmail;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textSchool;
		private System.Windows.Forms.RadioButton radioStudentN;
		private System.Windows.Forms.RadioButton radioStudentP;
		private System.Windows.Forms.RadioButton radioStudentF;
		private System.Windows.Forms.Label labelSchoolName;
		private System.Windows.Forms.Label labelChartNumber;
		private System.Windows.Forms.TextBox textChartNumber;
		//private OpenDental.ValidDate textBirthdate2;
		private OpenDental.ValidDate textBirthdate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkSame;
		private System.Windows.Forms.ComboBox comboZip;
		private System.Windows.Forms.TextBox textZip;
		private System.Windows.Forms.GroupBox groupNotes;
		private System.Windows.Forms.CheckBox checkNotesSame;
		private System.Windows.Forms.TextBox textPatNum;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label32;
		private OpenDental.UI.Button butAuto;
		private System.Windows.Forms.Label labelMedicaidID;
		private System.Windows.Forms.TextBox textMedicaidID;
		private System.Windows.Forms.ListBox listStatus;
		private System.Windows.Forms.ListBox listGender;
		private System.Windows.Forms.ListBox listPosition;
		private System.Windows.Forms.TextBox textEmployer;
		private System.Windows.Forms.Label labelEmployer;
		private System.Windows.Forms.Label labelSSN;
		private System.Windows.Forms.Label labelZip;
		private System.Windows.Forms.Label labelST;
		private OpenDental.UI.Button butEditZip;
		private System.Windows.Forms.Label labelCity;
		private System.Windows.Forms.Label labelRaceEthnicity;
		private System.Windows.Forms.Label labelCounty;
		private System.Windows.Forms.Label labelSite;
		private System.Windows.Forms.Label labelGradeLevel;
		private System.Windows.Forms.GroupBox groupPH;
		private System.Windows.Forms.TextBox textCounty;
		private System.Windows.Forms.Label labelUrgency;
		private System.Windows.Forms.TextBox textSite;
		private System.Windows.Forms.ComboBox comboGradeLevel;
		private System.Windows.Forms.ComboBox comboUrgency;
		///<summary>Set true if this is a new patient. Patient must have been already inserted. If users clicks cancel, this patient will be deleted.</summary>
		public bool IsNew;
		private System.Windows.Forms.ListBox listSites;//displays dropdown for Sites
		private string SiteOriginal;
		private bool mouseIsInListSites;
		private List<Site> listSitesFiltered;
		private System.Windows.Forms.ListBox listCounties;//displays dropdown for GradeSchools
		private string countyOriginal;
		private OpenDental.ValidDate textDateFirstVisit;
		private System.Windows.Forms.Label labelDateFirstVisit;
		private bool mouseIsInListCounties;
		private County[] CountiesList;
		private OpenDental.ODtextBox textAddrNotes;
		private System.Windows.Forms.Label labelPutInInsPlan;
		private System.Windows.Forms.ListBox listEmps;//displayed from within code, not designer
		private string empOriginal;//used in the emp dropdown logic
		private bool mouseIsInListEmps;
		///<summary>This is the object that is altered in this form.</summary>
		private Patient PatCur;
		//private RefAttach[] RefList;
		private Family FamCur;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private TextBox textTrophyFolder;
		private Label labelTrophyFolder;
		private TextBox textWard;
		private Label labelWard;
		private Label labelLanguage;
		private ComboBox comboLanguage;
		private Patient PatOld;
		private ComboBox comboContact;
		private Label labelContact;
		private ComboBox comboConfirm;
		private Label labelConfirm;
		private ComboBox comboRecall;
		private Label labelRecall;
		private ValidDate textAdmitDate;
		private Label labelAdmitDate;
		private TextBox textTitle;
		private Label labelTitle;
		private OpenDental.UI.Button butPickSite;
		private OpenDental.UI.Button butPickResponsParty;
		private TextBox textResponsParty;
		private Label labelResponsParty;
		private OpenDental.UI.Button butClearResponsParty;
		private Label labelCanadianEligibilityCode;
		private ComboBox comboCanadianEligibilityCode;
		private Label label41;
		private ListBox listRelationships;
		private OpenDental.UI.Button butAddGuardian;
		///<summary>Will include the languages setup in the settings, and also the language of this patient if that language is not on the selection list.</summary>
		private List<string> languageList;
		private OpenDental.UI.Button butGuardianDefaults;
		private TextBox textAskToArriveEarly;
		private Label labelAskToArriveEarly;
		private CheckBox checkArriveEarlySame;
		private Label label43;
		private Label labelTextOk;
		private ListBox listTextOk;
		private ComboBoxMulti comboBoxMultiRace;
		private ComboBox comboEthnicity;
		private Label labelEthnicity;
		private TextBox textCountry;
		private TextBox textMotherMaidenFname;
		private Label labelMotherMaidenFname;
		private TextBox textMotherMaidenLname;
		private Label labelMotherMaidenLname;
		private List<Guardian> GuardianList;
		///<summary>If the user presses cancel, use this list to revert any changes to the guardians for all family members.</summary>
		private List<Guardian> _listGuardiansForFamOld;
		private Label labelDeceased;
		private TextBox textDateDeceased;
		private EhrPatient _ehrPatientCur;
		private GroupBox groupBox3;
		private CheckBox checkEmailPhoneSame;
		private UI.Button butShowMap;
		private GroupBox groupBillProv;
		private UI.Button butPickSecondary;
		private ComboBox comboBillType;
		private UI.Button butPickPrimary;
		private Label labelBillType;
		private Label labelFeeSched;
		private TextBox textCreditType;
		private Label labelSecProv;
		private Label labelCreditType;
		private Label labelPriProv;
		private ComboBox comboPriProv;
		private ComboBox comboFeeSched;
		private ComboBox comboSecProv;
		private CheckBox checkBillProvSame;
		private Label label30;
		private Label labelEmail;
		private TextBox textReferredFrom;
		private UI.Button butReferredFrom;
		private Label labelReferredFrom;
		private List<Clinic> _listClinics;
		private ToolTip _referredFromToolTip;
		private TextBox textMedicaidState;
		private Label labelRequiredField;
		///<summary>Local cache of RefAttaches for the current patient.  Set in FillReferrals().</summary>
		private List<RefAttach> _listRefAttaches;
		private System.Windows.Forms.ListBox listMedicaidStates;//displayed from within code, not designer
		private List<RequiredField> _listRequiredFields;
		private string _medicaidStateOriginal;//used in the medicaidState dropdown logic
		private bool _mouseIsInListMedicaidStates;
		private System.Windows.Forms.ListBox listStates;//displayed from within code, not designer
		private string _stateOriginal;//used in the medicaidState dropdown logic
		private bool _mouseIsInListStates;
		private bool _isMissingRequiredFields;
		private bool _isLoad;//To keep track if ListBoxes' selected index is changed by the user
		private ErrorProvider _errorProv=new ErrorProvider();
		private CheckBox checkSuperBilling;
		private bool _isValidating=false;

		///<summary></summary>
		public FormPatientEdit(Patient patCur,Family famCur){
			InitializeComponent();// Required for Windows Form Designer support
			PatCur=patCur;
			FamCur=famCur;
			PatOld=patCur.Copy();
			listEmps=new ListBox();
			listEmps.Location=new Point(textEmployer.Left,textEmployer.Bottom);
			listEmps.Size=new Size(textEmployer.Width,100);
			listEmps.Visible=false;
			listEmps.Click += new System.EventHandler(listEmps_Click);
			listEmps.DoubleClick += new System.EventHandler(listEmps_DoubleClick);
			listEmps.MouseEnter += new System.EventHandler(listEmps_MouseEnter);
			listEmps.MouseLeave += new System.EventHandler(listEmps_MouseLeave);
			Controls.Add(listEmps);
			listEmps.BringToFront();
			listCounties=new ListBox();
			listCounties.Location=new Point(groupPH.Left+textCounty.Left,groupPH.Top+textCounty.Bottom);
			listCounties.Size=new Size(textCounty.Width,100);
			listCounties.Visible=false;
			listCounties.Click += new System.EventHandler(listCounties_Click);
			//listCounties.DoubleClick += new System.EventHandler(listCars_DoubleClick);
			listCounties.MouseEnter += new System.EventHandler(listCounties_MouseEnter);
			listCounties.MouseLeave += new System.EventHandler(listCounties_MouseLeave);
			Controls.Add(listCounties);
			listCounties.BringToFront();
			listSites=new ListBox();
			listSites.Location=new Point(groupPH.Left+textSite.Left,groupPH.Top+textSite.Bottom);
			listSites.Size=new Size(textSite.Width,100);
			listSites.Visible=false;
			listSites.Click += new System.EventHandler(listSites_Click);
			listSites.MouseEnter += new System.EventHandler(listSites_MouseEnter);
			listSites.MouseLeave += new System.EventHandler(listSites_MouseLeave);
			Controls.Add(listSites);
			listSites.BringToFront();
			Lan.F(this);
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				labelST.Text=Lan.g(this,"ST, Country");
				textCountry.Visible=true;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				labelSSN.Text=Lan.g(this,"SIN");
				labelZip.Text=Lan.g(this,"Postal Code");
				labelST.Text=Lan.g(this,"Province");
				butEditZip.Text=Lan.g(this,"Edit Postal");
				labelCanadianEligibilityCode.Visible=true;
				comboCanadianEligibilityCode.Visible=true;
				radioStudentN.Visible=false;
				radioStudentP.Visible=false;
				radioStudentF.Visible=false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("GB")){//en-GB
				//labelSSN.Text="?";
				labelZip.Text=Lan.g(this,"Postcode");
				labelST.Text="";//no such thing as state in GB
				butEditZip.Text=Lan.g(this,"Edit Postcode");
			}
			_referredFromToolTip=new ToolTip();
			_referredFromToolTip.InitialDelay=500;
			_referredFromToolTip.ReshowDelay=100;
			listMedicaidStates=new ListBox();
			listMedicaidStates.Location=new Point(textMedicaidState.Left,textMedicaidState.Bottom);
			listMedicaidStates.Size=new Size(textMedicaidState.Width,100);
			listMedicaidStates.Visible=false;
			listMedicaidStates.Click += new System.EventHandler(listMedicaidStates_Click);
			listMedicaidStates.MouseEnter += new System.EventHandler(listMedicaidStates_MouseEnter);
			listMedicaidStates.MouseLeave += new System.EventHandler(listMedicaidStates_MouseLeave);
			Controls.Add(listMedicaidStates);
			listMedicaidStates.BringToFront();
			listStates=new ListBox();
			listStates.Location=new Point(textState.Left+groupBox1.Left,textState.Bottom+groupBox1.Top);
			listStates.Size=new Size(textState.Width,100);
			listStates.Visible=false;
			listStates.Click += new System.EventHandler(listStates_Click);
			listStates.MouseEnter += new System.EventHandler(listStates_MouseEnter);
			listStates.MouseLeave += new System.EventHandler(listStates_MouseLeave);
			Controls.Add(listStates);
			listStates.BringToFront();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientEdit));
			this.labelLName = new System.Windows.Forms.Label();
			this.labelFName = new System.Windows.Forms.Label();
			this.labelMiddleI = new System.Windows.Forms.Label();
			this.labelPreferred = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelGender = new System.Windows.Forms.Label();
			this.labelPosition = new System.Windows.Forms.Label();
			this.labelBirthdate = new System.Windows.Forms.Label();
			this.labelSSN = new System.Windows.Forms.Label();
			this.labelAddress = new System.Windows.Forms.Label();
			this.labelAddress2 = new System.Windows.Forms.Label();
			this.labelCity = new System.Windows.Forms.Label();
			this.labelST = new System.Windows.Forms.Label();
			this.labelZip = new System.Windows.Forms.Label();
			this.labelHmPhone = new System.Windows.Forms.Label();
			this.labelWkPhone = new System.Windows.Forms.Label();
			this.labelWirelessPhone = new System.Windows.Forms.Label();
			this.textLName = new System.Windows.Forms.TextBox();
			this.textFName = new System.Windows.Forms.TextBox();
			this.textMiddleI = new System.Windows.Forms.TextBox();
			this.textPreferred = new System.Windows.Forms.TextBox();
			this.textSSN = new System.Windows.Forms.TextBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textCity = new System.Windows.Forms.TextBox();
			this.textState = new System.Windows.Forms.TextBox();
			this.textHmPhone = new System.Windows.Forms.TextBox();
			this.textWkPhone = new System.Windows.Forms.TextBox();
			this.textWirelessPhone = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label20 = new System.Windows.Forms.Label();
			this.textAge = new System.Windows.Forms.TextBox();
			this.textSalutation = new System.Windows.Forms.TextBox();
			this.labelSalutation = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelCanadianEligibilityCode = new System.Windows.Forms.Label();
			this.comboCanadianEligibilityCode = new System.Windows.Forms.ComboBox();
			this.textSchool = new System.Windows.Forms.TextBox();
			this.radioStudentN = new System.Windows.Forms.RadioButton();
			this.radioStudentP = new System.Windows.Forms.RadioButton();
			this.radioStudentF = new System.Windows.Forms.RadioButton();
			this.labelSchoolName = new System.Windows.Forms.Label();
			this.labelChartNumber = new System.Windows.Forms.Label();
			this.textChartNumber = new System.Windows.Forms.TextBox();
			this.textBirthdate = new OpenDental.ValidDate();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butShowMap = new OpenDental.UI.Button();
			this.butEditZip = new OpenDental.UI.Button();
			this.textZip = new System.Windows.Forms.TextBox();
			this.comboZip = new System.Windows.Forms.ComboBox();
			this.checkSame = new System.Windows.Forms.CheckBox();
			this.textCountry = new System.Windows.Forms.TextBox();
			this.groupNotes = new System.Windows.Forms.GroupBox();
			this.textAddrNotes = new OpenDental.ODtextBox();
			this.checkNotesSame = new System.Windows.Forms.CheckBox();
			this.textPatNum = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.butAuto = new OpenDental.UI.Button();
			this.textMedicaidID = new System.Windows.Forms.TextBox();
			this.labelMedicaidID = new System.Windows.Forms.Label();
			this.listStatus = new System.Windows.Forms.ListBox();
			this.listGender = new System.Windows.Forms.ListBox();
			this.listPosition = new System.Windows.Forms.ListBox();
			this.textEmployer = new System.Windows.Forms.TextBox();
			this.labelEmployer = new System.Windows.Forms.Label();
			this.groupPH = new System.Windows.Forms.GroupBox();
			this.comboBoxMultiRace = new OpenDental.UI.ComboBoxMulti();
			this.comboEthnicity = new System.Windows.Forms.ComboBox();
			this.labelEthnicity = new System.Windows.Forms.Label();
			this.butClearResponsParty = new OpenDental.UI.Button();
			this.butPickResponsParty = new OpenDental.UI.Button();
			this.textResponsParty = new System.Windows.Forms.TextBox();
			this.labelResponsParty = new System.Windows.Forms.Label();
			this.butPickSite = new OpenDental.UI.Button();
			this.comboUrgency = new System.Windows.Forms.ComboBox();
			this.comboGradeLevel = new System.Windows.Forms.ComboBox();
			this.textSite = new System.Windows.Forms.TextBox();
			this.labelUrgency = new System.Windows.Forms.Label();
			this.textCounty = new System.Windows.Forms.TextBox();
			this.labelGradeLevel = new System.Windows.Forms.Label();
			this.labelSite = new System.Windows.Forms.Label();
			this.labelCounty = new System.Windows.Forms.Label();
			this.labelRaceEthnicity = new System.Windows.Forms.Label();
			this.textDateFirstVisit = new OpenDental.ValidDate();
			this.labelDateFirstVisit = new System.Windows.Forms.Label();
			this.labelPutInInsPlan = new System.Windows.Forms.Label();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.textTrophyFolder = new System.Windows.Forms.TextBox();
			this.labelTrophyFolder = new System.Windows.Forms.Label();
			this.textWard = new System.Windows.Forms.TextBox();
			this.labelWard = new System.Windows.Forms.Label();
			this.labelLanguage = new System.Windows.Forms.Label();
			this.comboLanguage = new System.Windows.Forms.ComboBox();
			this.comboContact = new System.Windows.Forms.ComboBox();
			this.labelContact = new System.Windows.Forms.Label();
			this.comboConfirm = new System.Windows.Forms.ComboBox();
			this.labelConfirm = new System.Windows.Forms.Label();
			this.comboRecall = new System.Windows.Forms.ComboBox();
			this.labelRecall = new System.Windows.Forms.Label();
			this.textAdmitDate = new OpenDental.ValidDate();
			this.labelAdmitDate = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.labelTitle = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.listRelationships = new System.Windows.Forms.ListBox();
			this.butAddGuardian = new OpenDental.UI.Button();
			this.butGuardianDefaults = new OpenDental.UI.Button();
			this.textAskToArriveEarly = new System.Windows.Forms.TextBox();
			this.labelAskToArriveEarly = new System.Windows.Forms.Label();
			this.checkArriveEarlySame = new System.Windows.Forms.CheckBox();
			this.label43 = new System.Windows.Forms.Label();
			this.labelTextOk = new System.Windows.Forms.Label();
			this.listTextOk = new System.Windows.Forms.ListBox();
			this.textMotherMaidenFname = new System.Windows.Forms.TextBox();
			this.labelMotherMaidenFname = new System.Windows.Forms.Label();
			this.textMotherMaidenLname = new System.Windows.Forms.TextBox();
			this.labelMotherMaidenLname = new System.Windows.Forms.Label();
			this.labelDeceased = new System.Windows.Forms.Label();
			this.textDateDeceased = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label30 = new System.Windows.Forms.Label();
			this.checkEmailPhoneSame = new System.Windows.Forms.CheckBox();
			this.labelEmail = new System.Windows.Forms.Label();
			this.groupBillProv = new System.Windows.Forms.GroupBox();
			this.checkBillProvSame = new System.Windows.Forms.CheckBox();
			this.checkSuperBilling = new System.Windows.Forms.CheckBox();
			this.butPickSecondary = new OpenDental.UI.Button();
			this.comboBillType = new System.Windows.Forms.ComboBox();
			this.butPickPrimary = new OpenDental.UI.Button();
			this.labelBillType = new System.Windows.Forms.Label();
			this.labelFeeSched = new System.Windows.Forms.Label();
			this.textCreditType = new System.Windows.Forms.TextBox();
			this.labelSecProv = new System.Windows.Forms.Label();
			this.labelCreditType = new System.Windows.Forms.Label();
			this.labelPriProv = new System.Windows.Forms.Label();
			this.comboPriProv = new System.Windows.Forms.ComboBox();
			this.comboFeeSched = new System.Windows.Forms.ComboBox();
			this.comboSecProv = new System.Windows.Forms.ComboBox();
			this.textReferredFrom = new System.Windows.Forms.TextBox();
			this.butReferredFrom = new OpenDental.UI.Button();
			this.labelReferredFrom = new System.Windows.Forms.Label();
			this.textMedicaidState = new System.Windows.Forms.TextBox();
			this.labelRequiredField = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupNotes.SuspendLayout();
			this.groupPH.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBillProv.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelLName
			// 
			this.labelLName.Location = new System.Drawing.Point(5, 32);
			this.labelLName.Name = "labelLName";
			this.labelLName.Size = new System.Drawing.Size(154, 14);
			this.labelLName.TabIndex = 0;
			this.labelLName.Text = "Last Name";
			this.labelLName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFName
			// 
			this.labelFName.Location = new System.Drawing.Point(5, 52);
			this.labelFName.Name = "labelFName";
			this.labelFName.Size = new System.Drawing.Size(154, 14);
			this.labelFName.TabIndex = 0;
			this.labelFName.Text = "First Name";
			this.labelFName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelMiddleI
			// 
			this.labelMiddleI.Location = new System.Drawing.Point(5, 72);
			this.labelMiddleI.Name = "labelMiddleI";
			this.labelMiddleI.Size = new System.Drawing.Size(154, 14);
			this.labelMiddleI.TabIndex = 0;
			this.labelMiddleI.Text = "Middle Initial";
			this.labelMiddleI.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelPreferred
			// 
			this.labelPreferred.Location = new System.Drawing.Point(5, 92);
			this.labelPreferred.Name = "labelPreferred";
			this.labelPreferred.Size = new System.Drawing.Size(154, 14);
			this.labelPreferred.TabIndex = 0;
			this.labelPreferred.Text = "Preferred Name";
			this.labelPreferred.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(19, 191);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(105, 14);
			this.labelStatus.TabIndex = 0;
			this.labelStatus.Text = "Status";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelGender
			// 
			this.labelGender.Location = new System.Drawing.Point(134, 191);
			this.labelGender.Name = "labelGender";
			this.labelGender.Size = new System.Drawing.Size(105, 14);
			this.labelGender.TabIndex = 0;
			this.labelGender.Text = "Gender";
			this.labelGender.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPosition
			// 
			this.labelPosition.Location = new System.Drawing.Point(249, 191);
			this.labelPosition.Name = "labelPosition";
			this.labelPosition.Size = new System.Drawing.Size(105, 14);
			this.labelPosition.TabIndex = 0;
			this.labelPosition.Text = "Position";
			this.labelPosition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelBirthdate
			// 
			this.labelBirthdate.Location = new System.Drawing.Point(2, 282);
			this.labelBirthdate.Name = "labelBirthdate";
			this.labelBirthdate.Size = new System.Drawing.Size(156, 14);
			this.labelBirthdate.TabIndex = 0;
			this.labelBirthdate.Text = "Birthdate";
			this.labelBirthdate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelSSN
			// 
			this.labelSSN.Location = new System.Drawing.Point(5, 322);
			this.labelSSN.Name = "labelSSN";
			this.labelSSN.Size = new System.Drawing.Size(154, 14);
			this.labelSSN.TabIndex = 0;
			this.labelSSN.Text = "SS#";
			this.labelSSN.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelAddress
			// 
			this.labelAddress.Location = new System.Drawing.Point(8, 56);
			this.labelAddress.Name = "labelAddress";
			this.labelAddress.Size = new System.Drawing.Size(152, 14);
			this.labelAddress.TabIndex = 0;
			this.labelAddress.Text = "Address";
			this.labelAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelAddress2
			// 
			this.labelAddress2.Location = new System.Drawing.Point(8, 76);
			this.labelAddress2.Name = "labelAddress2";
			this.labelAddress2.Size = new System.Drawing.Size(152, 14);
			this.labelAddress2.TabIndex = 0;
			this.labelAddress2.Text = "Address2";
			this.labelAddress2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelCity
			// 
			this.labelCity.Location = new System.Drawing.Point(8, 96);
			this.labelCity.Name = "labelCity";
			this.labelCity.Size = new System.Drawing.Size(152, 14);
			this.labelCity.TabIndex = 0;
			this.labelCity.Text = "City";
			this.labelCity.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelST
			// 
			this.labelST.Location = new System.Drawing.Point(8, 116);
			this.labelST.Name = "labelST";
			this.labelST.Size = new System.Drawing.Size(152, 14);
			this.labelST.TabIndex = 0;
			this.labelST.Text = "ST";
			this.labelST.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelZip
			// 
			this.labelZip.Location = new System.Drawing.Point(8, 136);
			this.labelZip.Name = "labelZip";
			this.labelZip.Size = new System.Drawing.Size(152, 14);
			this.labelZip.TabIndex = 0;
			this.labelZip.Text = "Zip";
			this.labelZip.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelHmPhone
			// 
			this.labelHmPhone.Location = new System.Drawing.Point(8, 36);
			this.labelHmPhone.Name = "labelHmPhone";
			this.labelHmPhone.Size = new System.Drawing.Size(152, 14);
			this.labelHmPhone.TabIndex = 0;
			this.labelHmPhone.Text = "Home Phone";
			this.labelHmPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelWkPhone
			// 
			this.labelWkPhone.Location = new System.Drawing.Point(8, 56);
			this.labelWkPhone.Name = "labelWkPhone";
			this.labelWkPhone.Size = new System.Drawing.Size(152, 14);
			this.labelWkPhone.TabIndex = 0;
			this.labelWkPhone.Text = "Work Phone";
			this.labelWkPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelWirelessPhone
			// 
			this.labelWirelessPhone.Location = new System.Drawing.Point(8, 36);
			this.labelWirelessPhone.Name = "labelWirelessPhone";
			this.labelWirelessPhone.Size = new System.Drawing.Size(152, 14);
			this.labelWirelessPhone.TabIndex = 0;
			this.labelWirelessPhone.Text = "Wireless Phone";
			this.labelWirelessPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(160, 28);
			this.textLName.MaxLength = 100;
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(228, 20);
			this.textLName.TabIndex = 1;
			this.textLName.TextChanged += new System.EventHandler(this.textLName_TextChanged);
			this.textLName.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(160, 48);
			this.textFName.MaxLength = 100;
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(228, 20);
			this.textFName.TabIndex = 2;
			this.textFName.TextChanged += new System.EventHandler(this.textFName_TextChanged);
			this.textFName.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textMiddleI
			// 
			this.textMiddleI.Location = new System.Drawing.Point(160, 68);
			this.textMiddleI.MaxLength = 100;
			this.textMiddleI.Name = "textMiddleI";
			this.textMiddleI.Size = new System.Drawing.Size(96, 20);
			this.textMiddleI.TabIndex = 3;
			this.textMiddleI.TextChanged += new System.EventHandler(this.textMiddleI_TextChanged);
			this.textMiddleI.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textPreferred
			// 
			this.textPreferred.Location = new System.Drawing.Point(160, 88);
			this.textPreferred.MaxLength = 100;
			this.textPreferred.Name = "textPreferred";
			this.textPreferred.Size = new System.Drawing.Size(228, 20);
			this.textPreferred.TabIndex = 4;
			this.textPreferred.TextChanged += new System.EventHandler(this.textPreferred_TextChanged);
			this.textPreferred.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textSSN
			// 
			this.textSSN.Location = new System.Drawing.Point(160, 318);
			this.textSSN.MaxLength = 100;
			this.textSSN.Name = "textSSN";
			this.textSSN.Size = new System.Drawing.Size(96, 20);
			this.textSSN.TabIndex = 11;
			this.textSSN.Leave += new System.EventHandler(this.textBox_Leave);
			this.textSSN.Validating += new System.ComponentModel.CancelEventHandler(this.textSSN_Validating);
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(161, 52);
			this.textAddress.MaxLength = 100;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(254, 20);
			this.textAddress.TabIndex = 3;
			this.textAddress.TextChanged += new System.EventHandler(this.textAddress_TextChanged);
			this.textAddress.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(161, 72);
			this.textAddress2.MaxLength = 100;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(254, 20);
			this.textAddress2.TabIndex = 4;
			this.textAddress2.TextChanged += new System.EventHandler(this.textAddress2_TextChanged);
			this.textAddress2.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(161, 92);
			this.textCity.MaxLength = 100;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(198, 20);
			this.textCity.TabIndex = 5;
			this.textCity.TextChanged += new System.EventHandler(this.textCity_TextChanged);
			this.textCity.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(161, 112);
			this.textState.MaxLength = 100;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(61, 20);
			this.textState.TabIndex = 6;
			this.textState.TextChanged += new System.EventHandler(this.textState_TextChanged);
			this.textState.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textState_KeyUp);
			this.textState.Leave += new System.EventHandler(this.textState_Leave);
			// 
			// textHmPhone
			// 
			this.textHmPhone.Location = new System.Drawing.Point(161, 32);
			this.textHmPhone.MaxLength = 30;
			this.textHmPhone.Name = "textHmPhone";
			this.textHmPhone.Size = new System.Drawing.Size(198, 20);
			this.textHmPhone.TabIndex = 2;
			this.textHmPhone.TextChanged += new System.EventHandler(this.textAnyPhoneNumber_TextChanged);
			this.textHmPhone.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textWkPhone
			// 
			this.textWkPhone.Location = new System.Drawing.Point(161, 52);
			this.textWkPhone.MaxLength = 30;
			this.textWkPhone.Name = "textWkPhone";
			this.textWkPhone.Size = new System.Drawing.Size(135, 20);
			this.textWkPhone.TabIndex = 3;
			this.textWkPhone.TextChanged += new System.EventHandler(this.textAnyPhoneNumber_TextChanged);
			this.textWkPhone.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textWirelessPhone
			// 
			this.textWirelessPhone.Location = new System.Drawing.Point(161, 32);
			this.textWirelessPhone.MaxLength = 30;
			this.textWirelessPhone.Name = "textWirelessPhone";
			this.textWirelessPhone.Size = new System.Drawing.Size(135, 20);
			this.textWirelessPhone.TabIndex = 2;
			this.textWirelessPhone.TextChanged += new System.EventHandler(this.textAnyPhoneNumber_TextChanged);
			this.textWirelessPhone.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(801, 665);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 34;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(887, 665);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 35;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(258, 282);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(47, 14);
			this.label20.TabIndex = 0;
			this.label20.Text = "Age";
			this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textAge
			// 
			this.textAge.Location = new System.Drawing.Point(305, 278);
			this.textAge.Name = "textAge";
			this.textAge.ReadOnly = true;
			this.textAge.Size = new System.Drawing.Size(50, 20);
			this.textAge.TabIndex = 0;
			this.textAge.TabStop = false;
			// 
			// textSalutation
			// 
			this.textSalutation.Location = new System.Drawing.Point(160, 128);
			this.textSalutation.MaxLength = 100;
			this.textSalutation.Name = "textSalutation";
			this.textSalutation.Size = new System.Drawing.Size(228, 20);
			this.textSalutation.TabIndex = 6;
			this.textSalutation.TextChanged += new System.EventHandler(this.textSalutation_TextChanged);
			this.textSalutation.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelSalutation
			// 
			this.labelSalutation.Location = new System.Drawing.Point(5, 132);
			this.labelSalutation.Name = "labelSalutation";
			this.labelSalutation.Size = new System.Drawing.Size(154, 14);
			this.labelSalutation.TabIndex = 0;
			this.labelSalutation.Text = "Salutation (Dear __)";
			this.labelSalutation.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textEmail
			// 
			this.textEmail.Location = new System.Drawing.Point(161, 72);
			this.textEmail.MaxLength = 100;
			this.textEmail.Name = "textEmail";
			this.textEmail.Size = new System.Drawing.Size(254, 20);
			this.textEmail.TabIndex = 4;
			this.textEmail.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.labelCanadianEligibilityCode);
			this.groupBox2.Controls.Add(this.comboCanadianEligibilityCode);
			this.groupBox2.Controls.Add(this.textSchool);
			this.groupBox2.Controls.Add(this.radioStudentN);
			this.groupBox2.Controls.Add(this.radioStudentP);
			this.groupBox2.Controls.Add(this.radioStudentF);
			this.groupBox2.Controls.Add(this.labelSchoolName);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(12, 606);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(439, 84);
			this.groupBox2.TabIndex = 28;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Student Status if Dependent Over 19 (for Ins)";
			// 
			// labelCanadianEligibilityCode
			// 
			this.labelCanadianEligibilityCode.Location = new System.Drawing.Point(9, 61);
			this.labelCanadianEligibilityCode.Name = "labelCanadianEligibilityCode";
			this.labelCanadianEligibilityCode.Size = new System.Drawing.Size(184, 14);
			this.labelCanadianEligibilityCode.TabIndex = 0;
			this.labelCanadianEligibilityCode.Text = "Eligibility Excep. Code";
			this.labelCanadianEligibilityCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelCanadianEligibilityCode.Visible = false;
			// 
			// comboCanadianEligibilityCode
			// 
			this.comboCanadianEligibilityCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCanadianEligibilityCode.FormattingEnabled = true;
			this.comboCanadianEligibilityCode.Location = new System.Drawing.Point(194, 57);
			this.comboCanadianEligibilityCode.Name = "comboCanadianEligibilityCode";
			this.comboCanadianEligibilityCode.Size = new System.Drawing.Size(224, 21);
			this.comboCanadianEligibilityCode.TabIndex = 5;
			this.comboCanadianEligibilityCode.Visible = false;
			this.comboCanadianEligibilityCode.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// textSchool
			// 
			this.textSchool.Location = new System.Drawing.Point(194, 37);
			this.textSchool.MaxLength = 255;
			this.textSchool.Name = "textSchool";
			this.textSchool.Size = new System.Drawing.Size(224, 20);
			this.textSchool.TabIndex = 4;
			this.textSchool.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// radioStudentN
			// 
			this.radioStudentN.Location = new System.Drawing.Point(122, 18);
			this.radioStudentN.Name = "radioStudentN";
			this.radioStudentN.Size = new System.Drawing.Size(117, 16);
			this.radioStudentN.TabIndex = 1;
			this.radioStudentN.TabStop = true;
			this.radioStudentN.Text = "Nonstudent";
			this.radioStudentN.Click += new System.EventHandler(this.radioStudentN_Click);
			// 
			// radioStudentP
			// 
			this.radioStudentP.Location = new System.Drawing.Point(332, 18);
			this.radioStudentP.Name = "radioStudentP";
			this.radioStudentP.Size = new System.Drawing.Size(86, 16);
			this.radioStudentP.TabIndex = 3;
			this.radioStudentP.TabStop = true;
			this.radioStudentP.Text = "Parttime";
			this.radioStudentP.Click += new System.EventHandler(this.radioStudentP_Click);
			// 
			// radioStudentF
			// 
			this.radioStudentF.Location = new System.Drawing.Point(239, 18);
			this.radioStudentF.Name = "radioStudentF";
			this.radioStudentF.Size = new System.Drawing.Size(93, 16);
			this.radioStudentF.TabIndex = 2;
			this.radioStudentF.TabStop = true;
			this.radioStudentF.Text = "Fulltime";
			this.radioStudentF.Click += new System.EventHandler(this.radioStudentF_Click);
			// 
			// labelSchoolName
			// 
			this.labelSchoolName.Location = new System.Drawing.Point(9, 41);
			this.labelSchoolName.Name = "labelSchoolName";
			this.labelSchoolName.Size = new System.Drawing.Size(183, 14);
			this.labelSchoolName.TabIndex = 0;
			this.labelSchoolName.Text = "College Name";
			this.labelSchoolName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelChartNumber
			// 
			this.labelChartNumber.Location = new System.Drawing.Point(5, 362);
			this.labelChartNumber.Name = "labelChartNumber";
			this.labelChartNumber.Size = new System.Drawing.Size(154, 16);
			this.labelChartNumber.TabIndex = 0;
			this.labelChartNumber.Text = "ChartNumber";
			this.labelChartNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textChartNumber
			// 
			this.textChartNumber.Location = new System.Drawing.Point(160, 358);
			this.textChartNumber.MaxLength = 20;
			this.textChartNumber.Name = "textChartNumber";
			this.textChartNumber.Size = new System.Drawing.Size(96, 20);
			this.textChartNumber.TabIndex = 14;
			this.textChartNumber.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// textBirthdate
			// 
			this.textBirthdate.Location = new System.Drawing.Point(160, 278);
			this.textBirthdate.Name = "textBirthdate";
			this.textBirthdate.Size = new System.Drawing.Size(96, 20);
			this.textBirthdate.TabIndex = 9;
			this.textBirthdate.Validated += new System.EventHandler(this.textBirthdate_Validated);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butShowMap);
			this.groupBox1.Controls.Add(this.textHmPhone);
			this.groupBox1.Controls.Add(this.butEditZip);
			this.groupBox1.Controls.Add(this.textZip);
			this.groupBox1.Controls.Add(this.comboZip);
			this.groupBox1.Controls.Add(this.checkSame);
			this.groupBox1.Controls.Add(this.textCountry);
			this.groupBox1.Controls.Add(this.textState);
			this.groupBox1.Controls.Add(this.labelST);
			this.groupBox1.Controls.Add(this.textAddress);
			this.groupBox1.Controls.Add(this.labelAddress2);
			this.groupBox1.Controls.Add(this.labelCity);
			this.groupBox1.Controls.Add(this.textAddress2);
			this.groupBox1.Controls.Add(this.labelZip);
			this.groupBox1.Controls.Add(this.labelHmPhone);
			this.groupBox1.Controls.Add(this.textCity);
			this.groupBox1.Controls.Add(this.labelAddress);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(488, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(474, 160);
			this.groupBox1.TabIndex = 29;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Address and Phone";
			// 
			// butShowMap
			// 
			this.butShowMap.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowMap.Autosize = true;
			this.butShowMap.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowMap.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowMap.CornerRadius = 4F;
			this.butShowMap.Location = new System.Drawing.Point(342, 132);
			this.butShowMap.Name = "butShowMap";
			this.butShowMap.Size = new System.Drawing.Size(73, 22);
			this.butShowMap.TabIndex = 61;
			this.butShowMap.Text = "Show Map";
			this.butShowMap.Visible = false;
			this.butShowMap.Click += new System.EventHandler(this.butShowMap_Click);
			// 
			// butEditZip
			// 
			this.butEditZip.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditZip.Autosize = true;
			this.butEditZip.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditZip.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditZip.CornerRadius = 4F;
			this.butEditZip.Location = new System.Drawing.Point(269, 132);
			this.butEditZip.Name = "butEditZip";
			this.butEditZip.Size = new System.Drawing.Size(73, 22);
			this.butEditZip.TabIndex = 9;
			this.butEditZip.Text = "&Edit Zip";
			this.butEditZip.Click += new System.EventHandler(this.butEditZip_Click);
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(161, 132);
			this.textZip.MaxLength = 100;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(87, 20);
			this.textZip.TabIndex = 8;
			this.textZip.TextChanged += new System.EventHandler(this.textZip_TextChanged);
			this.textZip.Leave += new System.EventHandler(this.textBox_Leave);
			this.textZip.Validating += new System.ComponentModel.CancelEventHandler(this.textZip_Validating);
			// 
			// comboZip
			// 
			this.comboZip.DropDownWidth = 198;
			this.comboZip.Location = new System.Drawing.Point(161, 132);
			this.comboZip.MaxDropDownItems = 20;
			this.comboZip.Name = "comboZip";
			this.comboZip.Size = new System.Drawing.Size(106, 21);
			this.comboZip.TabIndex = 60;
			this.comboZip.TabStop = false;
			this.comboZip.SelectionChangeCommitted += new System.EventHandler(this.comboZip_SelectionChangeCommitted);
			// 
			// checkSame
			// 
			this.checkSame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSame.Location = new System.Drawing.Point(161, 13);
			this.checkSame.Name = "checkSame";
			this.checkSame.Size = new System.Drawing.Size(254, 17);
			this.checkSame.TabIndex = 1;
			this.checkSame.TabStop = false;
			this.checkSame.Text = "Same for entire family";
			// 
			// textCountry
			// 
			this.textCountry.Location = new System.Drawing.Point(223, 112);
			this.textCountry.MaxLength = 100;
			this.textCountry.Name = "textCountry";
			this.textCountry.Size = new System.Drawing.Size(192, 20);
			this.textCountry.TabIndex = 7;
			this.textCountry.Visible = false;
			this.textCountry.TextChanged += new System.EventHandler(this.textState_TextChanged);
			// 
			// groupNotes
			// 
			this.groupNotes.Controls.Add(this.textAddrNotes);
			this.groupNotes.Controls.Add(this.checkNotesSame);
			this.groupNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupNotes.Location = new System.Drawing.Point(488, 164);
			this.groupNotes.Name = "groupNotes";
			this.groupNotes.Size = new System.Drawing.Size(474, 84);
			this.groupNotes.TabIndex = 30;
			this.groupNotes.TabStop = false;
			this.groupNotes.Text = "Address and Phone Notes";
			// 
			// textAddrNotes
			// 
			this.textAddrNotes.AcceptsTab = true;
			this.textAddrNotes.BackColor = System.Drawing.SystemColors.Window;
			this.textAddrNotes.DetectUrls = false;
			this.textAddrNotes.Location = new System.Drawing.Point(161, 32);
			this.textAddrNotes.Name = "textAddrNotes";
			this.textAddrNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.PatAddressNote;
			this.textAddrNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textAddrNotes.Size = new System.Drawing.Size(224, 52);
			this.textAddrNotes.TabIndex = 0;
			this.textAddrNotes.TabStop = false;
			this.textAddrNotes.Text = "";
			this.textAddrNotes.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// checkNotesSame
			// 
			this.checkNotesSame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotesSame.Location = new System.Drawing.Point(161, 13);
			this.checkNotesSame.Name = "checkNotesSame";
			this.checkNotesSame.Size = new System.Drawing.Size(254, 17);
			this.checkNotesSame.TabIndex = 1;
			this.checkNotesSame.TabStop = false;
			this.checkNotesSame.Text = "Same for entire family";
			// 
			// textPatNum
			// 
			this.textPatNum.Location = new System.Drawing.Point(160, 8);
			this.textPatNum.MaxLength = 100;
			this.textPatNum.Name = "textPatNum";
			this.textPatNum.ReadOnly = true;
			this.textPatNum.Size = new System.Drawing.Size(228, 20);
			this.textPatNum.TabIndex = 0;
			this.textPatNum.TabStop = false;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(5, 12);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(154, 14);
			this.label19.TabIndex = 0;
			this.label19.Text = "Patient Number";
			this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(325, 362);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(142, 35);
			this.label32.TabIndex = 0;
			this.label32.Text = "(if used)";
			// 
			// butAuto
			// 
			this.butAuto.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAuto.Autosize = true;
			this.butAuto.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAuto.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAuto.CornerRadius = 4F;
			this.butAuto.Location = new System.Drawing.Point(258, 358);
			this.butAuto.Name = "butAuto";
			this.butAuto.Size = new System.Drawing.Size(62, 22);
			this.butAuto.TabIndex = 15;
			this.butAuto.Text = "Auto";
			this.butAuto.Click += new System.EventHandler(this.butAuto_Click);
			// 
			// textMedicaidID
			// 
			this.textMedicaidID.Location = new System.Drawing.Point(160, 338);
			this.textMedicaidID.MaxLength = 20;
			this.textMedicaidID.Name = "textMedicaidID";
			this.textMedicaidID.Size = new System.Drawing.Size(96, 20);
			this.textMedicaidID.TabIndex = 12;
			this.textMedicaidID.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelMedicaidID
			// 
			this.labelMedicaidID.Location = new System.Drawing.Point(5, 342);
			this.labelMedicaidID.Name = "labelMedicaidID";
			this.labelMedicaidID.Size = new System.Drawing.Size(154, 14);
			this.labelMedicaidID.TabIndex = 0;
			this.labelMedicaidID.Text = "Medicaid ID, State";
			this.labelMedicaidID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listStatus
			// 
			this.listStatus.Location = new System.Drawing.Point(19, 206);
			this.listStatus.Name = "listStatus";
			this.listStatus.Size = new System.Drawing.Size(105, 69);
			this.listStatus.TabIndex = 0;
			this.listStatus.TabStop = false;
			this.listStatus.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
			// 
			// listGender
			// 
			this.listGender.Location = new System.Drawing.Point(134, 206);
			this.listGender.Name = "listGender";
			this.listGender.Size = new System.Drawing.Size(105, 43);
			this.listGender.TabIndex = 0;
			this.listGender.TabStop = false;
			this.listGender.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
			// 
			// listPosition
			// 
			this.listPosition.Location = new System.Drawing.Point(249, 206);
			this.listPosition.Name = "listPosition";
			this.listPosition.Size = new System.Drawing.Size(105, 69);
			this.listPosition.TabIndex = 0;
			this.listPosition.TabStop = false;
			this.listPosition.SelectedIndexChanged += new System.EventHandler(this.listPosition_SelectedIndexChanged);
			// 
			// textEmployer
			// 
			this.textEmployer.Location = new System.Drawing.Point(160, 418);
			this.textEmployer.MaxLength = 255;
			this.textEmployer.Name = "textEmployer";
			this.textEmployer.Size = new System.Drawing.Size(226, 20);
			this.textEmployer.TabIndex = 18;
			this.textEmployer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEmployer_KeyUp);
			this.textEmployer.Leave += new System.EventHandler(this.textEmployer_Leave);
			// 
			// labelEmployer
			// 
			this.labelEmployer.Location = new System.Drawing.Point(5, 422);
			this.labelEmployer.Name = "labelEmployer";
			this.labelEmployer.Size = new System.Drawing.Size(154, 14);
			this.labelEmployer.TabIndex = 0;
			this.labelEmployer.Text = "Employer";
			this.labelEmployer.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupPH
			// 
			this.groupPH.Controls.Add(this.comboBoxMultiRace);
			this.groupPH.Controls.Add(this.comboEthnicity);
			this.groupPH.Controls.Add(this.labelEthnicity);
			this.groupPH.Controls.Add(this.butClearResponsParty);
			this.groupPH.Controls.Add(this.butPickResponsParty);
			this.groupPH.Controls.Add(this.textResponsParty);
			this.groupPH.Controls.Add(this.labelResponsParty);
			this.groupPH.Controls.Add(this.butPickSite);
			this.groupPH.Controls.Add(this.comboUrgency);
			this.groupPH.Controls.Add(this.comboGradeLevel);
			this.groupPH.Controls.Add(this.textSite);
			this.groupPH.Controls.Add(this.labelUrgency);
			this.groupPH.Controls.Add(this.textCounty);
			this.groupPH.Controls.Add(this.labelGradeLevel);
			this.groupPH.Controls.Add(this.labelSite);
			this.groupPH.Controls.Add(this.labelCounty);
			this.groupPH.Controls.Add(this.labelRaceEthnicity);
			this.groupPH.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupPH.Location = new System.Drawing.Point(488, 491);
			this.groupPH.Name = "groupPH";
			this.groupPH.Size = new System.Drawing.Size(474, 166);
			this.groupPH.TabIndex = 33;
			this.groupPH.TabStop = false;
			this.groupPH.Text = "Public Health";
			// 
			// comboBoxMultiRace
			// 
			this.comboBoxMultiRace.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiRace.DroppedDown = false;
			this.comboBoxMultiRace.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiRace.Items")));
			this.comboBoxMultiRace.Location = new System.Drawing.Point(161, 15);
			this.comboBoxMultiRace.Name = "comboBoxMultiRace";
			this.comboBoxMultiRace.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiRace.SelectedIndices")));
			this.comboBoxMultiRace.Size = new System.Drawing.Size(155, 21);
			this.comboBoxMultiRace.TabIndex = 1;
			this.comboBoxMultiRace.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiRace_SelectionChangeCommitted);
			// 
			// comboEthnicity
			// 
			this.comboEthnicity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEthnicity.Location = new System.Drawing.Point(161, 36);
			this.comboEthnicity.MaxDropDownItems = 20;
			this.comboEthnicity.Name = "comboEthnicity";
			this.comboEthnicity.Size = new System.Drawing.Size(198, 21);
			this.comboEthnicity.TabIndex = 2;
			this.comboEthnicity.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// labelEthnicity
			// 
			this.labelEthnicity.Location = new System.Drawing.Point(8, 40);
			this.labelEthnicity.Name = "labelEthnicity";
			this.labelEthnicity.Size = new System.Drawing.Size(152, 14);
			this.labelEthnicity.TabIndex = 0;
			this.labelEthnicity.Text = "Ethnicity";
			this.labelEthnicity.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butClearResponsParty
			// 
			this.butClearResponsParty.AdjustImageLocation = new System.Drawing.Point(1, 1);
			this.butClearResponsParty.Autosize = true;
			this.butClearResponsParty.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearResponsParty.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearResponsParty.CornerRadius = 4F;
			this.butClearResponsParty.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butClearResponsParty.Location = new System.Drawing.Point(384, 137);
			this.butClearResponsParty.Name = "butClearResponsParty";
			this.butClearResponsParty.Size = new System.Drawing.Size(25, 23);
			this.butClearResponsParty.TabIndex = 0;
			this.butClearResponsParty.TabStop = false;
			this.butClearResponsParty.Click += new System.EventHandler(this.butClearResponsParty_Click);
			// 
			// butPickResponsParty
			// 
			this.butPickResponsParty.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickResponsParty.Autosize = true;
			this.butPickResponsParty.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickResponsParty.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickResponsParty.CornerRadius = 4F;
			this.butPickResponsParty.Location = new System.Drawing.Point(361, 137);
			this.butPickResponsParty.Name = "butPickResponsParty";
			this.butPickResponsParty.Size = new System.Drawing.Size(23, 23);
			this.butPickResponsParty.TabIndex = 8;
			this.butPickResponsParty.Text = "...";
			this.butPickResponsParty.Click += new System.EventHandler(this.butPickResponsParty_Click);
			// 
			// textResponsParty
			// 
			this.textResponsParty.AcceptsReturn = true;
			this.textResponsParty.Location = new System.Drawing.Point(161, 139);
			this.textResponsParty.Name = "textResponsParty";
			this.textResponsParty.ReadOnly = true;
			this.textResponsParty.Size = new System.Drawing.Size(198, 20);
			this.textResponsParty.TabIndex = 0;
			this.textResponsParty.TabStop = false;
			this.textResponsParty.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelResponsParty
			// 
			this.labelResponsParty.Location = new System.Drawing.Point(8, 143);
			this.labelResponsParty.Name = "labelResponsParty";
			this.labelResponsParty.Size = new System.Drawing.Size(152, 14);
			this.labelResponsParty.TabIndex = 0;
			this.labelResponsParty.Text = "Responsible Party";
			this.labelResponsParty.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butPickSite
			// 
			this.butPickSite.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSite.Autosize = true;
			this.butPickSite.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSite.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSite.CornerRadius = 4F;
			this.butPickSite.Location = new System.Drawing.Point(361, 77);
			this.butPickSite.Name = "butPickSite";
			this.butPickSite.Size = new System.Drawing.Size(23, 20);
			this.butPickSite.TabIndex = 5;
			this.butPickSite.Text = "...";
			this.butPickSite.Click += new System.EventHandler(this.butPickSite_Click);
			// 
			// comboUrgency
			// 
			this.comboUrgency.BackColor = System.Drawing.SystemColors.Window;
			this.comboUrgency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUrgency.Location = new System.Drawing.Point(161, 118);
			this.comboUrgency.Name = "comboUrgency";
			this.comboUrgency.Size = new System.Drawing.Size(198, 21);
			this.comboUrgency.TabIndex = 7;
			this.comboUrgency.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// comboGradeLevel
			// 
			this.comboGradeLevel.BackColor = System.Drawing.SystemColors.Window;
			this.comboGradeLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGradeLevel.Location = new System.Drawing.Point(161, 97);
			this.comboGradeLevel.MaxDropDownItems = 25;
			this.comboGradeLevel.Name = "comboGradeLevel";
			this.comboGradeLevel.Size = new System.Drawing.Size(198, 21);
			this.comboGradeLevel.TabIndex = 6;
			this.comboGradeLevel.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// textSite
			// 
			this.textSite.AcceptsReturn = true;
			this.textSite.Location = new System.Drawing.Point(161, 77);
			this.textSite.Name = "textSite";
			this.textSite.Size = new System.Drawing.Size(198, 20);
			this.textSite.TabIndex = 4;
			this.textSite.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textSite_KeyUp);
			this.textSite.Leave += new System.EventHandler(this.textSite_Leave);
			// 
			// labelUrgency
			// 
			this.labelUrgency.Location = new System.Drawing.Point(8, 122);
			this.labelUrgency.Name = "labelUrgency";
			this.labelUrgency.Size = new System.Drawing.Size(152, 14);
			this.labelUrgency.TabIndex = 0;
			this.labelUrgency.Text = "Treatment Urgency";
			this.labelUrgency.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCounty
			// 
			this.textCounty.AcceptsReturn = true;
			this.textCounty.Location = new System.Drawing.Point(161, 57);
			this.textCounty.Name = "textCounty";
			this.textCounty.Size = new System.Drawing.Size(198, 20);
			this.textCounty.TabIndex = 3;
			this.textCounty.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCounty_KeyUp);
			this.textCounty.Leave += new System.EventHandler(this.textCounty_Leave);
			// 
			// labelGradeLevel
			// 
			this.labelGradeLevel.Location = new System.Drawing.Point(8, 101);
			this.labelGradeLevel.Name = "labelGradeLevel";
			this.labelGradeLevel.Size = new System.Drawing.Size(152, 14);
			this.labelGradeLevel.TabIndex = 0;
			this.labelGradeLevel.Text = "Grade Level";
			this.labelGradeLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(8, 81);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(152, 14);
			this.labelSite.TabIndex = 0;
			this.labelSite.Text = "Site (or Grade School)";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelCounty
			// 
			this.labelCounty.Location = new System.Drawing.Point(8, 61);
			this.labelCounty.Name = "labelCounty";
			this.labelCounty.Size = new System.Drawing.Size(152, 14);
			this.labelCounty.TabIndex = 0;
			this.labelCounty.Text = "County";
			this.labelCounty.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelRaceEthnicity
			// 
			this.labelRaceEthnicity.Location = new System.Drawing.Point(8, 19);
			this.labelRaceEthnicity.Name = "labelRaceEthnicity";
			this.labelRaceEthnicity.Size = new System.Drawing.Size(152, 14);
			this.labelRaceEthnicity.TabIndex = 0;
			this.labelRaceEthnicity.Text = "Race/Ethnicity";
			this.labelRaceEthnicity.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateFirstVisit
			// 
			this.textDateFirstVisit.Location = new System.Drawing.Point(160, 378);
			this.textDateFirstVisit.Name = "textDateFirstVisit";
			this.textDateFirstVisit.Size = new System.Drawing.Size(96, 20);
			this.textDateFirstVisit.TabIndex = 16;
			this.textDateFirstVisit.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelDateFirstVisit
			// 
			this.labelDateFirstVisit.Location = new System.Drawing.Point(5, 382);
			this.labelDateFirstVisit.Name = "labelDateFirstVisit";
			this.labelDateFirstVisit.Size = new System.Drawing.Size(153, 14);
			this.labelDateFirstVisit.TabIndex = 0;
			this.labelDateFirstVisit.Text = "Date of First Visit";
			this.labelDateFirstVisit.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelPutInInsPlan
			// 
			this.labelPutInInsPlan.Location = new System.Drawing.Point(320, 342);
			this.labelPutInInsPlan.Name = "labelPutInInsPlan";
			this.labelPutInInsPlan.Size = new System.Drawing.Size(165, 14);
			this.labelPutInInsPlan.TabIndex = 0;
			this.labelPutInInsPlan.Text = "(put in InsPlan too)";
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(5, 546);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(154, 14);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(160, 542);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(226, 21);
			this.comboClinic.TabIndex = 24;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// textTrophyFolder
			// 
			this.textTrophyFolder.Location = new System.Drawing.Point(160, 501);
			this.textTrophyFolder.MaxLength = 30;
			this.textTrophyFolder.Name = "textTrophyFolder";
			this.textTrophyFolder.Size = new System.Drawing.Size(226, 20);
			this.textTrophyFolder.TabIndex = 22;
			this.textTrophyFolder.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelTrophyFolder
			// 
			this.labelTrophyFolder.Location = new System.Drawing.Point(5, 505);
			this.labelTrophyFolder.Name = "labelTrophyFolder";
			this.labelTrophyFolder.Size = new System.Drawing.Size(154, 14);
			this.labelTrophyFolder.TabIndex = 0;
			this.labelTrophyFolder.Text = "Trophy Folder";
			this.labelTrophyFolder.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textWard
			// 
			this.textWard.Location = new System.Drawing.Point(160, 563);
			this.textWard.MaxLength = 100;
			this.textWard.Name = "textWard";
			this.textWard.Size = new System.Drawing.Size(50, 20);
			this.textWard.TabIndex = 25;
			this.textWard.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelWard
			// 
			this.labelWard.Location = new System.Drawing.Point(5, 567);
			this.labelWard.Name = "labelWard";
			this.labelWard.Size = new System.Drawing.Size(154, 14);
			this.labelWard.TabIndex = 0;
			this.labelWard.Text = "Ward";
			this.labelWard.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelLanguage
			// 
			this.labelLanguage.Location = new System.Drawing.Point(5, 525);
			this.labelLanguage.Name = "labelLanguage";
			this.labelLanguage.Size = new System.Drawing.Size(153, 14);
			this.labelLanguage.TabIndex = 0;
			this.labelLanguage.Text = "Language";
			this.labelLanguage.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboLanguage
			// 
			this.comboLanguage.BackColor = System.Drawing.SystemColors.Window;
			this.comboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLanguage.Location = new System.Drawing.Point(160, 521);
			this.comboLanguage.MaxDropDownItems = 25;
			this.comboLanguage.Name = "comboLanguage";
			this.comboLanguage.Size = new System.Drawing.Size(226, 21);
			this.comboLanguage.TabIndex = 23;
			this.comboLanguage.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// comboContact
			// 
			this.comboContact.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboContact.FormattingEnabled = true;
			this.comboContact.Location = new System.Drawing.Point(160, 438);
			this.comboContact.MaxDropDownItems = 30;
			this.comboContact.Name = "comboContact";
			this.comboContact.Size = new System.Drawing.Size(226, 21);
			this.comboContact.TabIndex = 19;
			this.comboContact.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// labelContact
			// 
			this.labelContact.Location = new System.Drawing.Point(5, 442);
			this.labelContact.Name = "labelContact";
			this.labelContact.Size = new System.Drawing.Size(153, 14);
			this.labelContact.TabIndex = 0;
			this.labelContact.Text = "Prefer Contact Method";
			this.labelContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboConfirm
			// 
			this.comboConfirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConfirm.FormattingEnabled = true;
			this.comboConfirm.Location = new System.Drawing.Point(160, 459);
			this.comboConfirm.MaxDropDownItems = 30;
			this.comboConfirm.Name = "comboConfirm";
			this.comboConfirm.Size = new System.Drawing.Size(226, 21);
			this.comboConfirm.TabIndex = 20;
			this.comboConfirm.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// labelConfirm
			// 
			this.labelConfirm.Location = new System.Drawing.Point(5, 463);
			this.labelConfirm.Name = "labelConfirm";
			this.labelConfirm.Size = new System.Drawing.Size(153, 14);
			this.labelConfirm.TabIndex = 0;
			this.labelConfirm.Text = "Prefer Confirm Method";
			this.labelConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboRecall
			// 
			this.comboRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRecall.FormattingEnabled = true;
			this.comboRecall.Location = new System.Drawing.Point(160, 480);
			this.comboRecall.MaxDropDownItems = 30;
			this.comboRecall.Name = "comboRecall";
			this.comboRecall.Size = new System.Drawing.Size(226, 21);
			this.comboRecall.TabIndex = 21;
			this.comboRecall.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// labelRecall
			// 
			this.labelRecall.Location = new System.Drawing.Point(5, 484);
			this.labelRecall.Name = "labelRecall";
			this.labelRecall.Size = new System.Drawing.Size(153, 14);
			this.labelRecall.TabIndex = 0;
			this.labelRecall.Text = "Prefer Recall Method";
			this.labelRecall.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAdmitDate
			// 
			this.textAdmitDate.Location = new System.Drawing.Point(290, 563);
			this.textAdmitDate.Name = "textAdmitDate";
			this.textAdmitDate.Size = new System.Drawing.Size(96, 20);
			this.textAdmitDate.TabIndex = 26;
			this.textAdmitDate.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelAdmitDate
			// 
			this.labelAdmitDate.Location = new System.Drawing.Point(212, 567);
			this.labelAdmitDate.Name = "labelAdmitDate";
			this.labelAdmitDate.Size = new System.Drawing.Size(78, 14);
			this.labelAdmitDate.TabIndex = 0;
			this.labelAdmitDate.Text = "Admit Date";
			this.labelAdmitDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(160, 108);
			this.textTitle.MaxLength = 100;
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(96, 20);
			this.textTitle.TabIndex = 5;
			this.textTitle.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelTitle
			// 
			this.labelTitle.Location = new System.Drawing.Point(5, 112);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(154, 14);
			this.labelTitle.TabIndex = 0;
			this.labelTitle.Text = "Title (Mr., Ms.)";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label41
			// 
			this.label41.Location = new System.Drawing.Point(364, 191);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(114, 14);
			this.label41.TabIndex = 0;
			this.label41.Text = "Family Relationships";
			this.label41.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listRelationships
			// 
			this.listRelationships.FormattingEnabled = true;
			this.listRelationships.Location = new System.Drawing.Point(364, 206);
			this.listRelationships.Name = "listRelationships";
			this.listRelationships.Size = new System.Drawing.Size(114, 69);
			this.listRelationships.TabIndex = 0;
			this.listRelationships.TabStop = false;
			this.listRelationships.DoubleClick += new System.EventHandler(this.listRelationships_DoubleClick);
			// 
			// butAddGuardian
			// 
			this.butAddGuardian.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddGuardian.Autosize = true;
			this.butAddGuardian.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddGuardian.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddGuardian.CornerRadius = 4F;
			this.butAddGuardian.Location = new System.Drawing.Point(364, 277);
			this.butAddGuardian.Name = "butAddGuardian";
			this.butAddGuardian.Size = new System.Drawing.Size(57, 22);
			this.butAddGuardian.TabIndex = 0;
			this.butAddGuardian.TabStop = false;
			this.butAddGuardian.Text = "Add";
			this.butAddGuardian.Click += new System.EventHandler(this.butAddGuardian_Click);
			// 
			// butGuardianDefaults
			// 
			this.butGuardianDefaults.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGuardianDefaults.Autosize = true;
			this.butGuardianDefaults.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGuardianDefaults.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGuardianDefaults.CornerRadius = 4F;
			this.butGuardianDefaults.Location = new System.Drawing.Point(421, 277);
			this.butGuardianDefaults.Name = "butGuardianDefaults";
			this.butGuardianDefaults.Size = new System.Drawing.Size(57, 22);
			this.butGuardianDefaults.TabIndex = 0;
			this.butGuardianDefaults.TabStop = false;
			this.butGuardianDefaults.Text = "Defaults";
			this.butGuardianDefaults.Click += new System.EventHandler(this.butGuardianDefaults_Click);
			// 
			// textAskToArriveEarly
			// 
			this.textAskToArriveEarly.Location = new System.Drawing.Point(160, 398);
			this.textAskToArriveEarly.MaxLength = 100;
			this.textAskToArriveEarly.Name = "textAskToArriveEarly";
			this.textAskToArriveEarly.Size = new System.Drawing.Size(36, 20);
			this.textAskToArriveEarly.TabIndex = 17;
			this.textAskToArriveEarly.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelAskToArriveEarly
			// 
			this.labelAskToArriveEarly.Location = new System.Drawing.Point(5, 402);
			this.labelAskToArriveEarly.Name = "labelAskToArriveEarly";
			this.labelAskToArriveEarly.Size = new System.Drawing.Size(154, 14);
			this.labelAskToArriveEarly.TabIndex = 0;
			this.labelAskToArriveEarly.Text = "Ask To Arrive Early";
			this.labelAskToArriveEarly.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkArriveEarlySame
			// 
			this.checkArriveEarlySame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkArriveEarlySame.Location = new System.Drawing.Point(270, 399);
			this.checkArriveEarlySame.Name = "checkArriveEarlySame";
			this.checkArriveEarlySame.Size = new System.Drawing.Size(208, 17);
			this.checkArriveEarlySame.TabIndex = 1;
			this.checkArriveEarlySame.TabStop = false;
			this.checkArriveEarlySame.Text = "Same for entire family";
			// 
			// label43
			// 
			this.label43.Location = new System.Drawing.Point(196, 402);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(68, 14);
			this.label43.TabIndex = 0;
			this.label43.Text = "(minutes)";
			// 
			// labelTextOk
			// 
			this.labelTextOk.Location = new System.Drawing.Point(297, 35);
			this.labelTextOk.Name = "labelTextOk";
			this.labelTextOk.Size = new System.Drawing.Size(64, 14);
			this.labelTextOk.TabIndex = 0;
			this.labelTextOk.Text = "Text OK";
			this.labelTextOk.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listTextOk
			// 
			this.listTextOk.ColumnWidth = 30;
			this.listTextOk.Items.AddRange(new object[] {
            "??",
            "Yes",
            "No"});
			this.listTextOk.Location = new System.Drawing.Point(361, 33);
			this.listTextOk.MultiColumn = true;
			this.listTextOk.Name = "listTextOk";
			this.listTextOk.Size = new System.Drawing.Size(95, 17);
			this.listTextOk.TabIndex = 0;
			this.listTextOk.TabStop = false;
			this.listTextOk.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
			// 
			// textMotherMaidenFname
			// 
			this.textMotherMaidenFname.Location = new System.Drawing.Point(160, 148);
			this.textMotherMaidenFname.MaxLength = 100;
			this.textMotherMaidenFname.Name = "textMotherMaidenFname";
			this.textMotherMaidenFname.Size = new System.Drawing.Size(228, 20);
			this.textMotherMaidenFname.TabIndex = 7;
			this.textMotherMaidenFname.Visible = false;
			this.textMotherMaidenFname.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelMotherMaidenFname
			// 
			this.labelMotherMaidenFname.Location = new System.Drawing.Point(5, 152);
			this.labelMotherMaidenFname.Name = "labelMotherMaidenFname";
			this.labelMotherMaidenFname.Size = new System.Drawing.Size(154, 14);
			this.labelMotherMaidenFname.TabIndex = 0;
			this.labelMotherMaidenFname.Text = "Mother\'s Maiden First Name";
			this.labelMotherMaidenFname.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelMotherMaidenFname.Visible = false;
			// 
			// textMotherMaidenLname
			// 
			this.textMotherMaidenLname.Location = new System.Drawing.Point(160, 168);
			this.textMotherMaidenLname.MaxLength = 100;
			this.textMotherMaidenLname.Name = "textMotherMaidenLname";
			this.textMotherMaidenLname.Size = new System.Drawing.Size(228, 20);
			this.textMotherMaidenLname.TabIndex = 8;
			this.textMotherMaidenLname.Visible = false;
			this.textMotherMaidenLname.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelMotherMaidenLname
			// 
			this.labelMotherMaidenLname.Location = new System.Drawing.Point(5, 172);
			this.labelMotherMaidenLname.Name = "labelMotherMaidenLname";
			this.labelMotherMaidenLname.Size = new System.Drawing.Size(154, 14);
			this.labelMotherMaidenLname.TabIndex = 0;
			this.labelMotherMaidenLname.Text = "Mother\'s Maiden Last Name";
			this.labelMotherMaidenLname.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelMotherMaidenLname.Visible = false;
			// 
			// labelDeceased
			// 
			this.labelDeceased.Location = new System.Drawing.Point(2, 302);
			this.labelDeceased.Name = "labelDeceased";
			this.labelDeceased.Size = new System.Drawing.Size(156, 14);
			this.labelDeceased.TabIndex = 0;
			this.labelDeceased.Text = "Date Time Deceased";
			this.labelDeceased.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelDeceased.Visible = false;
			// 
			// textDateDeceased
			// 
			this.textDateDeceased.Location = new System.Drawing.Point(160, 298);
			this.textDateDeceased.MaxLength = 100;
			this.textDateDeceased.Name = "textDateDeceased";
			this.textDateDeceased.Size = new System.Drawing.Size(195, 20);
			this.textDateDeceased.TabIndex = 10;
			this.textDateDeceased.Visible = false;
			this.textDateDeceased.Leave += new System.EventHandler(this.textBox_Leave);
			this.textDateDeceased.Validated += new System.EventHandler(this.textDateDeceased_Validated);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label30);
			this.groupBox3.Controls.Add(this.checkEmailPhoneSame);
			this.groupBox3.Controls.Add(this.labelWirelessPhone);
			this.groupBox3.Controls.Add(this.labelWkPhone);
			this.groupBox3.Controls.Add(this.textWkPhone);
			this.groupBox3.Controls.Add(this.labelTextOk);
			this.groupBox3.Controls.Add(this.listTextOk);
			this.groupBox3.Controls.Add(this.textWirelessPhone);
			this.groupBox3.Controls.Add(this.textEmail);
			this.groupBox3.Controls.Add(this.labelEmail);
			this.groupBox3.Location = new System.Drawing.Point(488, 392);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(474, 98);
			this.groupBox3.TabIndex = 32;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Email and Phone";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(415, 76);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(50, 14);
			this.label30.TabIndex = 7;
			this.label30.Text = "(a,b,c...)";
			// 
			// checkEmailPhoneSame
			// 
			this.checkEmailPhoneSame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEmailPhoneSame.Location = new System.Drawing.Point(161, 13);
			this.checkEmailPhoneSame.Name = "checkEmailPhoneSame";
			this.checkEmailPhoneSame.Size = new System.Drawing.Size(254, 17);
			this.checkEmailPhoneSame.TabIndex = 1;
			this.checkEmailPhoneSame.TabStop = false;
			this.checkEmailPhoneSame.Text = "Same for entire family";
			// 
			// labelEmail
			// 
			this.labelEmail.Location = new System.Drawing.Point(8, 76);
			this.labelEmail.Name = "labelEmail";
			this.labelEmail.Size = new System.Drawing.Size(152, 14);
			this.labelEmail.TabIndex = 0;
			this.labelEmail.Text = "E-mail Addresses";
			this.labelEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBillProv
			// 
			this.groupBillProv.Controls.Add(this.checkBillProvSame);
			this.groupBillProv.Controls.Add(this.checkSuperBilling);
			this.groupBillProv.Controls.Add(this.butPickSecondary);
			this.groupBillProv.Controls.Add(this.comboBillType);
			this.groupBillProv.Controls.Add(this.butPickPrimary);
			this.groupBillProv.Controls.Add(this.labelBillType);
			this.groupBillProv.Controls.Add(this.labelFeeSched);
			this.groupBillProv.Controls.Add(this.textCreditType);
			this.groupBillProv.Controls.Add(this.labelSecProv);
			this.groupBillProv.Controls.Add(this.labelCreditType);
			this.groupBillProv.Controls.Add(this.labelPriProv);
			this.groupBillProv.Controls.Add(this.comboPriProv);
			this.groupBillProv.Controls.Add(this.comboFeeSched);
			this.groupBillProv.Controls.Add(this.comboSecProv);
			this.groupBillProv.Location = new System.Drawing.Point(488, 249);
			this.groupBillProv.Name = "groupBillProv";
			this.groupBillProv.Size = new System.Drawing.Size(474, 142);
			this.groupBillProv.TabIndex = 31;
			this.groupBillProv.TabStop = false;
			this.groupBillProv.Text = "Billing and Provider(s)";
			// 
			// checkBillProvSame
			// 
			this.checkBillProvSame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBillProvSame.Location = new System.Drawing.Point(161, 13);
			this.checkBillProvSame.Name = "checkBillProvSame";
			this.checkBillProvSame.Size = new System.Drawing.Size(165, 17);
			this.checkBillProvSame.TabIndex = 1;
			this.checkBillProvSame.TabStop = false;
			this.checkBillProvSame.Text = "Same for entire family";
			// 
			// checkSuperBilling
			// 
			this.checkSuperBilling.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperBilling.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperBilling.Location = new System.Drawing.Point(183, 32);
			this.checkSuperBilling.Name = "checkSuperBilling";
			this.checkSuperBilling.Size = new System.Drawing.Size(176, 17);
			this.checkSuperBilling.TabIndex = 47;
			this.checkSuperBilling.TabStop = false;
			this.checkSuperBilling.Text = "Included in Super Family Billing";
			this.checkSuperBilling.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperBilling.Visible = false;
			this.checkSuperBilling.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkSuperBilling_MouseDown);
			// 
			// butPickSecondary
			// 
			this.butPickSecondary.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSecondary.Autosize = false;
			this.butPickSecondary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSecondary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSecondary.CornerRadius = 2F;
			this.butPickSecondary.Location = new System.Drawing.Point(361, 94);
			this.butPickSecondary.Name = "butPickSecondary";
			this.butPickSecondary.Size = new System.Drawing.Size(23, 21);
			this.butPickSecondary.TabIndex = 45;
			this.butPickSecondary.Text = "...";
			this.butPickSecondary.Click += new System.EventHandler(this.butPickSecondary_Click);
			// 
			// comboBillType
			// 
			this.comboBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillType.FormattingEnabled = true;
			this.comboBillType.Location = new System.Drawing.Point(161, 52);
			this.comboBillType.MaxDropDownItems = 30;
			this.comboBillType.Name = "comboBillType";
			this.comboBillType.Size = new System.Drawing.Size(198, 21);
			this.comboBillType.TabIndex = 41;
			this.comboBillType.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// butPickPrimary
			// 
			this.butPickPrimary.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickPrimary.Autosize = false;
			this.butPickPrimary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickPrimary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickPrimary.CornerRadius = 2F;
			this.butPickPrimary.Location = new System.Drawing.Point(361, 73);
			this.butPickPrimary.Name = "butPickPrimary";
			this.butPickPrimary.Size = new System.Drawing.Size(23, 21);
			this.butPickPrimary.TabIndex = 43;
			this.butPickPrimary.Text = "...";
			this.butPickPrimary.Click += new System.EventHandler(this.butPickPrimary_Click);
			// 
			// labelBillType
			// 
			this.labelBillType.Location = new System.Drawing.Point(8, 56);
			this.labelBillType.Name = "labelBillType";
			this.labelBillType.Size = new System.Drawing.Size(152, 14);
			this.labelBillType.TabIndex = 39;
			this.labelBillType.Text = "Billing Type";
			this.labelBillType.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFeeSched
			// 
			this.labelFeeSched.Location = new System.Drawing.Point(8, 119);
			this.labelFeeSched.Name = "labelFeeSched";
			this.labelFeeSched.Size = new System.Drawing.Size(152, 14);
			this.labelFeeSched.TabIndex = 35;
			this.labelFeeSched.Text = "Fee Schedule (rarely used)";
			this.labelFeeSched.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCreditType
			// 
			this.textCreditType.Location = new System.Drawing.Point(161, 32);
			this.textCreditType.MaxLength = 1;
			this.textCreditType.Name = "textCreditType";
			this.textCreditType.Size = new System.Drawing.Size(18, 20);
			this.textCreditType.TabIndex = 40;
			this.textCreditType.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// labelSecProv
			// 
			this.labelSecProv.Location = new System.Drawing.Point(8, 98);
			this.labelSecProv.Name = "labelSecProv";
			this.labelSecProv.Size = new System.Drawing.Size(152, 14);
			this.labelSecProv.TabIndex = 36;
			this.labelSecProv.Text = "Secondary Provider";
			this.labelSecProv.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelCreditType
			// 
			this.labelCreditType.Location = new System.Drawing.Point(8, 36);
			this.labelCreditType.Name = "labelCreditType";
			this.labelCreditType.Size = new System.Drawing.Size(152, 14);
			this.labelCreditType.TabIndex = 38;
			this.labelCreditType.Text = "Credit Type";
			this.labelCreditType.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelPriProv
			// 
			this.labelPriProv.Location = new System.Drawing.Point(8, 77);
			this.labelPriProv.Name = "labelPriProv";
			this.labelPriProv.Size = new System.Drawing.Size(152, 14);
			this.labelPriProv.TabIndex = 37;
			this.labelPriProv.Text = "Primary Provider";
			this.labelPriProv.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboPriProv
			// 
			this.comboPriProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriProv.FormattingEnabled = true;
			this.comboPriProv.Location = new System.Drawing.Point(161, 73);
			this.comboPriProv.MaxDropDownItems = 30;
			this.comboPriProv.Name = "comboPriProv";
			this.comboPriProv.Size = new System.Drawing.Size(198, 21);
			this.comboPriProv.TabIndex = 42;
			this.comboPriProv.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// comboFeeSched
			// 
			this.comboFeeSched.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched.FormattingEnabled = true;
			this.comboFeeSched.ItemHeight = 13;
			this.comboFeeSched.Location = new System.Drawing.Point(161, 115);
			this.comboFeeSched.MaxDropDownItems = 30;
			this.comboFeeSched.Name = "comboFeeSched";
			this.comboFeeSched.Size = new System.Drawing.Size(198, 21);
			this.comboFeeSched.TabIndex = 46;
			this.comboFeeSched.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// comboSecProv
			// 
			this.comboSecProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSecProv.FormattingEnabled = true;
			this.comboSecProv.ItemHeight = 13;
			this.comboSecProv.Location = new System.Drawing.Point(161, 94);
			this.comboSecProv.MaxDropDownItems = 30;
			this.comboSecProv.Name = "comboSecProv";
			this.comboSecProv.Size = new System.Drawing.Size(198, 21);
			this.comboSecProv.TabIndex = 44;
			this.comboSecProv.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_SelectionChangeCommited);
			// 
			// textReferredFrom
			// 
			this.textReferredFrom.Location = new System.Drawing.Point(160, 583);
			this.textReferredFrom.MaxLength = 30;
			this.textReferredFrom.Multiline = true;
			this.textReferredFrom.Name = "textReferredFrom";
			this.textReferredFrom.ReadOnly = true;
			this.textReferredFrom.Size = new System.Drawing.Size(226, 20);
			this.textReferredFrom.TabIndex = 36;
			this.textReferredFrom.TabStop = false;
			this.textReferredFrom.WordWrap = false;
			this.textReferredFrom.DoubleClick += new System.EventHandler(this.textReferredFrom_DoubleClick);
			// 
			// butReferredFrom
			// 
			this.butReferredFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReferredFrom.Autosize = false;
			this.butReferredFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReferredFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReferredFrom.CornerRadius = 2F;
			this.butReferredFrom.Location = new System.Drawing.Point(388, 583);
			this.butReferredFrom.Name = "butReferredFrom";
			this.butReferredFrom.Size = new System.Drawing.Size(23, 20);
			this.butReferredFrom.TabIndex = 27;
			this.butReferredFrom.Text = "...";
			this.butReferredFrom.Click += new System.EventHandler(this.butReferredFrom_Click);
			// 
			// labelReferredFrom
			// 
			this.labelReferredFrom.Location = new System.Drawing.Point(5, 587);
			this.labelReferredFrom.Name = "labelReferredFrom";
			this.labelReferredFrom.Size = new System.Drawing.Size(154, 14);
			this.labelReferredFrom.TabIndex = 50;
			this.labelReferredFrom.Text = "Referred From";
			this.labelReferredFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textMedicaidState
			// 
			this.textMedicaidState.Location = new System.Drawing.Point(257, 338);
			this.textMedicaidState.MaxLength = 100;
			this.textMedicaidState.Name = "textMedicaidState";
			this.textMedicaidState.Size = new System.Drawing.Size(61, 20);
			this.textMedicaidState.TabIndex = 13;
			this.textMedicaidState.TextChanged += new System.EventHandler(this.textMedicaidState_TextChanged);
			this.textMedicaidState.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textMedicaidState_KeyUp);
			this.textMedicaidState.Leave += new System.EventHandler(this.textMedicaidState_Leave);
			// 
			// labelRequiredField
			// 
			this.labelRequiredField.Location = new System.Drawing.Point(604, 672);
			this.labelRequiredField.Name = "labelRequiredField";
			this.labelRequiredField.Size = new System.Drawing.Size(180, 14);
			this.labelRequiredField.TabIndex = 9;
			this.labelRequiredField.Text = "* Indicates Required Field";
			this.labelRequiredField.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelRequiredField.Visible = false;
			// 
			// FormPatientEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.textMedicaidState);
			this.Controls.Add(this.labelRequiredField);
			this.Controls.Add(this.labelReferredFrom);
			this.Controls.Add(this.butReferredFrom);
			this.Controls.Add(this.textReferredFrom);
			this.Controls.Add(this.groupBillProv);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.textDateDeceased);
			this.Controls.Add(this.labelDeceased);
			this.Controls.Add(this.textMotherMaidenLname);
			this.Controls.Add(this.labelMotherMaidenLname);
			this.Controls.Add(this.textMotherMaidenFname);
			this.Controls.Add(this.labelMotherMaidenFname);
			this.Controls.Add(this.checkArriveEarlySame);
			this.Controls.Add(this.label43);
			this.Controls.Add(this.textAskToArriveEarly);
			this.Controls.Add(this.labelAskToArriveEarly);
			this.Controls.Add(this.butGuardianDefaults);
			this.Controls.Add(this.butAddGuardian);
			this.Controls.Add(this.listRelationships);
			this.Controls.Add(this.label41);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.textAdmitDate);
			this.Controls.Add(this.labelAdmitDate);
			this.Controls.Add(this.comboRecall);
			this.Controls.Add(this.labelRecall);
			this.Controls.Add(this.comboConfirm);
			this.Controls.Add(this.labelConfirm);
			this.Controls.Add(this.comboContact);
			this.Controls.Add(this.labelContact);
			this.Controls.Add(this.comboLanguage);
			this.Controls.Add(this.labelLanguage);
			this.Controls.Add(this.textWard);
			this.Controls.Add(this.labelWard);
			this.Controls.Add(this.textTrophyFolder);
			this.Controls.Add(this.labelTrophyFolder);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.labelPutInInsPlan);
			this.Controls.Add(this.textDateFirstVisit);
			this.Controls.Add(this.textEmployer);
			this.Controls.Add(this.textMedicaidID);
			this.Controls.Add(this.textPatNum);
			this.Controls.Add(this.textBirthdate);
			this.Controls.Add(this.textChartNumber);
			this.Controls.Add(this.textSalutation);
			this.Controls.Add(this.textAge);
			this.Controls.Add(this.textSSN);
			this.Controls.Add(this.textPreferred);
			this.Controls.Add(this.textMiddleI);
			this.Controls.Add(this.textFName);
			this.Controls.Add(this.textLName);
			this.Controls.Add(this.butAuto);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelDateFirstVisit);
			this.Controls.Add(this.groupPH);
			this.Controls.Add(this.labelEmployer);
			this.Controls.Add(this.labelGender);
			this.Controls.Add(this.listPosition);
			this.Controls.Add(this.listGender);
			this.Controls.Add(this.listStatus);
			this.Controls.Add(this.labelMedicaidID);
			this.Controls.Add(this.label32);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.groupNotes);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.labelChartNumber);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.labelSalutation);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.labelSSN);
			this.Controls.Add(this.labelBirthdate);
			this.Controls.Add(this.labelPosition);
			this.Controls.Add(this.labelPreferred);
			this.Controls.Add(this.labelMiddleI);
			this.Controls.Add(this.labelFName);
			this.Controls.Add(this.labelLName);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPatientEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Patient Information";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPatientEdit_Closing);
			this.Load += new System.EventHandler(this.FormPatientEdit_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupNotes.ResumeLayout(false);
			this.groupPH.ResumeLayout(false);
			this.groupPH.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBillProv.ResumeLayout(false);
			this.groupBillProv.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPatientEdit_Load(object sender, System.EventArgs e) {
			_isLoad=true;
			FillComboZip();
			checkSame.Checked=true;
			checkBillProvSame.Checked=true;
			checkNotesSame.Checked=true;
			checkEmailPhoneSame.Checked=true;
			if(PatCur!=null){
				for(int i=0;i<FamCur.ListPats.Length;i++){
					if(PatCur.HmPhone!=FamCur.ListPats[i].HmPhone
						|| PatCur.Address!=FamCur.ListPats[i].Address
						|| PatCur.Address2!=FamCur.ListPats[i].Address2
						|| PatCur.City!=FamCur.ListPats[i].City
						|| PatCur.State!=FamCur.ListPats[i].State
						|| PatCur.Zip!=FamCur.ListPats[i].Zip)
					{
						checkSame.Checked=false;
					}
					if(PatCur.CreditType!=FamCur.ListPats[i].CreditType
						|| PatCur.BillingType!=FamCur.ListPats[i].BillingType
						|| PatCur.PriProv!=FamCur.ListPats[i].PriProv
						|| PatCur.SecProv!=FamCur.ListPats[i].SecProv
						|| PatCur.FeeSched!=FamCur.ListPats[i].FeeSched)
					{
						checkBillProvSame.Checked=false;
					}
					if(PatCur.AddrNote!=FamCur.ListPats[i].AddrNote)
					{
						checkNotesSame.Checked=false;
					}
					if(PatCur.WirelessPhone!=FamCur.ListPats[i].WirelessPhone
						|| PatCur.WkPhone!=FamCur.ListPats[i].WkPhone
						|| PatCur.Email!=FamCur.ListPats[i].Email) 
					{
						checkEmailPhoneSame.Checked=false;
					}
				}
			}
			textPatNum.Text=PatCur.PatNum.ToString();
			textLName.Text=PatCur.LName;
			textFName.Text=PatCur.FName;
			textMiddleI.Text=PatCur.MiddleI;
			textPreferred.Text=PatCur.Preferred;
			textTitle.Text=PatCur.Title;
			textSalutation.Text=PatCur.Salutation;
			_ehrPatientCur=EhrPatients.Refresh(PatCur.PatNum);
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {//Show mother's maiden name UI if using EHR.
				labelMotherMaidenFname.Visible=true;
				textMotherMaidenFname.Visible=true;
				textMotherMaidenFname.Text=_ehrPatientCur.MotherMaidenFname;
				labelMotherMaidenLname.Visible=true;
				textMotherMaidenLname.Visible=true;
				textMotherMaidenLname.Text=_ehrPatientCur.MotherMaidenLname;
				labelDeceased.Visible=true;
				textDateDeceased.Visible=true;
			}
			listStatus.Items.Add(Lan.g("enumPatientStatus","Patient"));
			listStatus.Items.Add(Lan.g("enumPatientStatus","NonPatient"));
			listStatus.Items.Add(Lan.g("enumPatientStatus","Inactive"));
			listStatus.Items.Add(Lan.g("enumPatientStatus","Archived"));
			listStatus.Items.Add(Lan.g("enumPatientStatus","Deceased"));
			listStatus.Items.Add(Lan.g("enumPatientStatus","Prospective"));
			listGender.Items.Add(Lan.g("enumPatientGender","Male"));
			listGender.Items.Add(Lan.g("enumPatientGender","Female"));
			listGender.Items.Add(Lan.g("enumPatientGender","Unknown"));
			listPosition.Items.Add(Lan.g("enumPatientPosition","Single"));
			listPosition.Items.Add(Lan.g("enumPatientPosition","Married"));
			listPosition.Items.Add(Lan.g("enumPatientPosition","Child"));
			listPosition.Items.Add(Lan.g("enumPatientPosition","Widowed"));
			listPosition.Items.Add(Lan.g("enumPatientPosition","Divorced"));
			switch (PatCur.PatStatus){
				case PatientStatus.Patient : listStatus.SelectedIndex=0; break;
				case PatientStatus.NonPatient : listStatus.SelectedIndex=1; break;
				case PatientStatus.Inactive : listStatus.SelectedIndex=2; break;
				case PatientStatus.Archived : listStatus.SelectedIndex=3; break;
				case PatientStatus.Deceased : listStatus.SelectedIndex=4; break;
				case PatientStatus.Prospective : listStatus.SelectedIndex=5; break;}
			switch (PatCur.Gender){
				case PatientGender.Male : listGender.SelectedIndex=0; break;
				case PatientGender.Female : listGender.SelectedIndex=1; break;
				case PatientGender.Unknown : listGender.SelectedIndex=2; break;}
			switch (PatCur.Position){
				case PatientPosition.Single : listPosition.SelectedIndex=0; break;
				case PatientPosition.Married : listPosition.SelectedIndex=1; break;
				case PatientPosition.Child : listPosition.SelectedIndex=2; break;
				case PatientPosition.Widowed : listPosition.SelectedIndex=3; break;
				case PatientPosition.Divorced : listPosition.SelectedIndex=4; break;}
			FillGuardians();
			_listGuardiansForFamOld=FamCur.ListPats.SelectMany(x => Guardians.Refresh(x.PatNum)).ToList();
			if(PatCur.Birthdate.Year < 1880)
				textBirthdate.Text="";
			else
				textBirthdate.Text=PatCur.Birthdate.ToShortDateString();
			if(PatCur.DateTimeDeceased.Year < 1880) {
				textDateDeceased.Text="";
			}
			else {
				textDateDeceased.Text=PatCur.DateTimeDeceased.ToShortDateString()+"  "+PatCur.DateTimeDeceased.ToShortTimeString();
			}
			textAge.Text=PatientLogic.DateToAgeString(PatCur.Birthdate,PatCur.DateTimeDeceased);
			if(CultureInfo.CurrentCulture.Name=="en-US"//if USA
				&& PatCur.SSN!=null//the null catches new patients
				&& PatCur.SSN.Length==9)//and length exactly 9 (no data gets lost in formatting)
			{
				textSSN.Text=PatCur.SSN.Substring(0,3)+"-"
					+PatCur.SSN.Substring(3,2)+"-"+PatCur.SSN.Substring(5,4);
			}
			else{
				textSSN.Text=PatCur.SSN;
			}
			textMedicaidID.Text=PatCur.MedicaidID;
			textMedicaidState.Text=_ehrPatientCur.MedicaidState;
			textAddress.Text=PatCur.Address;
			textAddress2.Text=PatCur.Address2;
			textCity.Text=PatCur.City;
			textState.Text=PatCur.State;
			textCountry.Text=PatCur.Country;
			textZip.Text=PatCur.Zip;
			textHmPhone.Text=PatCur.HmPhone;
			textWkPhone.Text=PatCur.WkPhone;
			textWirelessPhone.Text=PatCur.WirelessPhone;
			listTextOk.SelectedIndex=(int)PatCur.TxtMsgOk;
			textEmail.Text=PatCur.Email;
			textCreditType.Text=PatCur.CreditType;
			if(PatCur.DateFirstVisit.Year < 1880){
				textDateFirstVisit.Text="";
			}
			else{
				textDateFirstVisit.Text=PatCur.DateFirstVisit.ToShortDateString();
			}
			if(PatCur.AskToArriveEarly>0){
				textAskToArriveEarly.Text=PatCur.AskToArriveEarly.ToString();
			}
			textChartNumber.Text=PatCur.ChartNumber;
			textEmployer.Text=Employers.GetName(PatCur.EmployerNum);
			//textEmploymentNote.Text=PatCur.EmploymentNote;
			languageList=new List<string>();
			if(PrefC.GetString(PrefName.LanguagesUsedByPatients)!="") {
				string[] lanstring=PrefC.GetString(PrefName.LanguagesUsedByPatients).Split(',');
				for(int i=0;i<lanstring.Length;i++) {
					if(lanstring[i]=="") {
						continue;
					}
					languageList.Add(lanstring[i]);
				}
			}
			if(PatCur.Language!="" && PatCur.Language!=null && !languageList.Contains(PatCur.Language)) {
				languageList.Add(PatCur.Language);
			}
			comboLanguage.Items.Add(Lan.g(this,"none"));//regardless of how many languages are listed, the first item is "none"
			comboLanguage.SelectedIndex=0;
			for(int i=0;i<languageList.Count;i++) {
				if(languageList[i]=="") {
					continue;
				}
				CultureInfo culture=CodeBase.MiscUtils.GetCultureFromThreeLetter(languageList[i]);
				if(culture==null) {//custom language
					comboLanguage.Items.Add(languageList[i]);
				}
				else {
					comboLanguage.Items.Add(culture.DisplayName);
				}
				if(PatCur.Language==languageList[i]) {
					comboLanguage.SelectedIndex=i+1;
				}
			}
			if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				comboPriProv.Items.Add(Lan.g(this,"Select Provider"));
				comboPriProv.SelectedIndex=0;//"Select Provider'
			}
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					comboPriProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				}
				else {
					comboPriProv.Items.Add(ProviderC.ListShort[i].Abbr);
				}
				if(ProviderC.ListShort[i].ProvNum==PatCur.PriProv) {
					comboPriProv.SelectedIndex=i;
					if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
						comboPriProv.SelectedIndex++;//Plus 1 to account for 'Select Provider'
					}
				}
			}
			/*Provider should not automatically change.  So may end up with no provider selected.
			if(comboPriProv.SelectedIndex==-1){
				int defaultindex=Providers.GetIndex(PrefC.GetLong(PrefName.PracticeDefaultProv));
				if(defaultindex==-1) {//default provider hidden
					comboPriProv.SelectedIndex=0;
				}
				else {
					comboPriProv.SelectedIndex=defaultindex;
				}
			}*/
			comboSecProv.Items.Clear();
			comboSecProv.Items.Add(Lan.g(this,"none"));
			comboSecProv.SelectedIndex=0;
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					comboSecProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				}
				else {
					comboSecProv.Items.Add(ProviderC.ListShort[i].Abbr);
				}
				if(ProviderC.ListShort[i].ProvNum==PatCur.SecProv)
					comboSecProv.SelectedIndex=i+1;
			}
			comboFeeSched.Items.Clear();
			comboFeeSched.Items.Add(Lan.g(this,"none"));
			comboFeeSched.SelectedIndex=0;
			for(int i=0;i<FeeSchedC.ListShort.Count;i++){
				comboFeeSched.Items.Add(FeeSchedC.ListShort[i].Description);
				if(FeeSchedC.ListShort[i].FeeSchedNum==PatCur.FeeSched){
					comboFeeSched.SelectedIndex=i+1;
				}
			}
			//MessageBox.Show(DefC.Short[(int)DefCat.BillingTypes].Length.ToString());
			for(int i=0;i<DefC.Short[(int)DefCat.BillingTypes].Length;i++){
				comboBillType.Items.Add(DefC.Short[(int)DefCat.BillingTypes][i].ItemName);
				if(DefC.Short[(int)DefCat.BillingTypes][i].DefNum==PatCur.BillingType)
					comboBillType.SelectedIndex=i;
			}
			if(comboBillType.SelectedIndex==-1){
				if(comboBillType.Items.Count==0) {
					MsgBox.Show(this,"Error.  All billing types have been hidden.  Please go to Definitions and unhide at least one.");
					DialogResult=DialogResult.Cancel;
					return;
				}
				comboBillType.SelectedIndex=0;
			}
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboClinic.Items.Clear();
			comboClinic.Items.Add(Lan.g(this,"Unassigned"));
			comboClinic.SelectedIndex=0;
			for(int i=0;i<_listClinics.Count;i++) {
				comboClinic.Items.Add(_listClinics[i].Description);
				if(_listClinics[i].ClinicNum==PatCur.ClinicNum) {
					comboClinic.SelectedIndex=i+1;
				}
			}
			switch(PatCur.StudentStatus){
				case "N"://non
				case "":
					radioStudentN.Checked=true;
					break;
				case "P"://parttime
					radioStudentP.Checked=true;
					break;
				case "F"://fulltime
					radioStudentF.Checked=true;
					break;
			}
			textSchool.Text=PatCur.SchoolName;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				labelSchoolName.Text=Lan.g(this,"Name of School");
				comboCanadianEligibilityCode.Items.Add("0 - Please Choose");
				comboCanadianEligibilityCode.Items.Add("1 - Full-time student");
				comboCanadianEligibilityCode.Items.Add("2 - Disabled");
				comboCanadianEligibilityCode.Items.Add("3 - Disabled student");
				comboCanadianEligibilityCode.Items.Add("4 - Code not applicable");
				comboCanadianEligibilityCode.SelectedIndex=PatCur.CanadianEligibilityCode;
			}
			textAddrNotes.Text=PatCur.AddrNote;
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				groupPH.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideMedicaid)){
				labelMedicaidID.Visible=false;
				labelPutInInsPlan.Visible=false;
				textMedicaidID.Visible=false;
				textMedicaidState.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)){
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","none"));//0 none option for both regular and EHR users.
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				labelRaceEthnicity.Text="Race";
				//comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Aboriginal"));//Hidden for EHR
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","AfricanAmerican"));//1
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","AmericanIndian"));//2
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Asian"));//3
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","DeclinedToSpecify"));//4
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","HawaiiOrPacIsland"));//5
				//comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Hispanic"));//Hidden for EHR. Supplemental to base 'race'. Shows in comboEthnicity instead.
				//comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Multiracial"));//Hidden for EHR
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Other"));//6
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","White"));//7
				comboEthnicity.Items.Add(Lan.g(this,"none"));//0 
				comboEthnicity.Items.Add(Lan.g(this,"DeclinedToSpecify"));//1
				comboEthnicity.Items.Add(Lan.g(this,"Not Hispanic"));//2
				comboEthnicity.Items.Add(Lan.g(this,"Hispanic"));//3
				List<int> listPatRaces=PatientRaces.GetPatRaceList(PatCur.PatNum);
				//Make sure there are no duplicates before filling.  This will get rid of the duplicates from the DB on the butOK_Click().
				for(int i=listPatRaces.Count-1;i>0;i--) {
					for(int j=i-1;j>=0;j--) {
						if(listPatRaces[i]==listPatRaces[j]) { //If there is a duplicate race
							listPatRaces.RemoveAt(i);
							break;
						}
					}
				}
				comboEthnicity.SelectedIndex=0;//none
				for(int i=0;i<listPatRaces.Count;i++) {
					PatRace race=(PatRace)listPatRaces[i];
					if(race==PatRace.AfricanAmerican) {
						comboBoxMultiRace.SetSelected(1,true);//AfricanAmerican
					}
					else if(race==PatRace.AmericanIndian) {
						comboBoxMultiRace.SetSelected(2,true);//AmericanIndian
					}
					else if(race==PatRace.Asian) {
						comboBoxMultiRace.SetSelected(3,true);//Asian
					}
					else if(race==PatRace.DeclinedToSpecifyRace) {
						comboBoxMultiRace.SetSelected(4,true);//DeclinedToSpecify
					}
					else if(race==PatRace.HawaiiOrPacIsland) {
						comboBoxMultiRace.SetSelected(5,true);//HawaiiOrPacIsland
					}
					else if(race==PatRace.Hispanic) {
						comboEthnicity.SelectedIndex=3;//Hispanic
					}
					else if(race==PatRace.NotHispanic) {
						comboEthnicity.SelectedIndex=2;//Not Hispanic
					}
					else if(race==PatRace.Other) {
						comboBoxMultiRace.SetSelected(6,true);//Other
					}
					else if(race==PatRace.White) {
						comboBoxMultiRace.SetSelected(7,true);//White
					}
					else if(race==PatRace.DeclinedToSpecifyEthnicity) {
						comboEthnicity.SelectedIndex=1;//DeclinedToSpecify
					}
				}
			}
			else {//EHR not enabled
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Aboriginal"));//1
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","AfricanAmerican"));//2
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","AmericanIndian"));//3
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Asian"));//4
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","DeclinedToSpecify"));//5
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","HawaiiOrPacIsland"));//6
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Hispanic"));//7
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Multiracial"));//8
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","Other"));//9
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","White"));//10
				comboBoxMultiRace.Items.Add(Lan.g("enumPatRace","NotHispanic"));//11
				List<int> listPatRaces=PatientRaces.GetPatRaceList(PatCur.PatNum);
				//Make sure there are no duplicates before filling.  This will get rid of the duplicates from the DB on the butOK_Click().
				for(int i=listPatRaces.Count-1;i>0;i--) {
					for(int j=i-1;j>=0;j--) {
						if(listPatRaces[i]==listPatRaces[j]) { //If there is a duplicate race
							listPatRaces.RemoveAt(i);
							break;
						}
					}
				}
				for(int i=0;i<listPatRaces.Count;i++) {
					comboBoxMultiRace.SetSelected(listPatRaces[i]+1,true);//Offset by 1 because of none option at location 0.
				}
				labelEthnicity.Visible=false;
				comboEthnicity.Visible=false;
			}
			if(comboBoxMultiRace.SelectedIndices.Count==0) {//no race set
				comboBoxMultiRace.SetSelected(0,true);//Set to none
			}
			comboBoxMultiRace.RefreshText();
			textCounty.Text=PatCur.County;
			textSite.Text=Sites.GetDescription(PatCur.SiteNum);
			string[] enumGrade=Enum.GetNames(typeof(PatientGrade));
			for(int i=0;i<enumGrade.Length;i++){
				comboGradeLevel.Items.Add(Lan.g("enumGrade",enumGrade[i]));
			}
			comboGradeLevel.SelectedIndex=(int)PatCur.GradeLevel;
			string[] enumUrg=Enum.GetNames(typeof(TreatmentUrgency));
			for(int i=0;i<enumUrg.Length;i++){
				comboUrgency.Items.Add(Lan.g("enumUrg",enumUrg[i]));
			}
			comboUrgency.SelectedIndex=(int)PatCur.Urgency;
			if(PatCur.ResponsParty!=0){
				textResponsParty.Text=Patients.GetLim(PatCur.ResponsParty).GetNameLF();
			}
			if(Programs.IsEnabled(ProgramName.TrophyEnhanced)){
				textTrophyFolder.Text=PatCur.TrophyFolder;
			}
			else{
				labelTrophyFolder.Visible=false;
				textTrophyFolder.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideHospitals)){
				textWard.Visible=false;
				labelWard.Visible=false;
				textAdmitDate.Visible=false;
				labelAdmitDate.Visible=false;
			}
			textWard.Text=PatCur.Ward;
			for(int i=0;i<Enum.GetNames(typeof(ContactMethod)).Length;i++){
				comboContact.Items.Add(Lan.g("enumContactMethod",Enum.GetNames(typeof(ContactMethod))[i]));
				comboConfirm.Items.Add(Lan.g("enumContactMethod",Enum.GetNames(typeof(ContactMethod))[i]));
				comboRecall.Items.Add(Lan.g("enumContactMethod",Enum.GetNames(typeof(ContactMethod))[i]));
			}
			comboContact.SelectedIndex=(int)PatCur.PreferContactMethod;
			comboConfirm.SelectedIndex=(int)PatCur.PreferConfirmMethod;
			comboRecall.SelectedIndex=(int)PatCur.PreferRecallMethod;
			if(PatCur.AdmitDate.Year>1880){
				textAdmitDate.Text=PatCur.AdmitDate.ToShortDateString();
			}
			FillReferrals();
			textLName.Select();
			if(HL7Defs.IsExistingHL7Enabled()) {
				if(HL7Defs.GetOneDeepEnabled().ShowDemographics==HL7ShowDemographics.Show) {//If show, then not edit so disable OK button
					butOK.Enabled=false;
				}
			}
			if(PrefC.GetBool(PrefName.ShowFeatureGoogleMaps)) {
				butShowMap.Visible=true;
			}
			_listRequiredFields=RequiredFields.Listt.FindAll(x => x.FieldType==RequiredFieldType.PatientInfo);
			//Remove the RequiredFields that are only on the Add Family window
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.InsuranceSubscriber);
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.InsuranceSubscriberID);
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.Carrier);
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.InsurancePhone);
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.GroupName);
			_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.GroupNum);
			if(!PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.MothersMaidenFirstName);
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.MothersMaidenLastName);
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.DateTimeDeceased);
			}
			if(!Programs.IsEnabled(Programs.GetProgramNum(ProgramName.TrophyEnhanced))) {
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.TrophyFolder);
			}
			if(PrefC.GetBool(PrefName.EasyHideHospitals)) {
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.Ward);
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.AdmitDate);
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) { //Canadian. en-CA or fr-CA
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.StudentStatus);
			}
			else {//Not Canadian
				_listRequiredFields.RemoveAll(x => x.FieldName==RequiredFieldName.EligibilityExceptCode);
			}
			_errorProv.BlinkStyle=ErrorBlinkStyle.NeverBlink;
			if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies) 
				&& PrefC.GetBool(PrefName.StatementsUseSheets) 
				&& PatCur.SuperFamily!=0 
				&& PatCur.Guarantor==PatCur.PatNum) 
			{
				//If the patient is a guarantor in a superfamily then enable the checkbox to opt into superfamily billing.
				checkSuperBilling.Visible=true;
				checkSuperBilling.Checked=PatCur.HasSuperBilling;
			}
			SetRequiredFields();
			_isLoad=false;
			Plugins.HookAddCode(this,"FormPatientEdit.Load_end",PatCur);
		}

		private void FillComboZip(){
			comboZip.Items.Clear();
			for(int i=0;i<ZipCodes.ALFrequent.Count;i++)
			{
				comboZip.Items.Add(((ZipCode)ZipCodes.ALFrequent[i]).ZipCodeDigits
					+" ("+((ZipCode)ZipCodes.ALFrequent[i]).City+")");
			}
		}

		private void FillGuardians(){
			GuardianList=Guardians.Refresh(PatCur.PatNum);
			listRelationships.Items.Clear();
			for(int i=0;i<GuardianList.Count;i++){
				listRelationships.Items.Add(FamCur.GetNameInFamFirst(GuardianList[i].PatNumGuardian)+" "
					+Guardians.GetGuardianRelationshipStr(GuardianList[i].Relationship));
			}
		}

		///<summary>Puts an asterisk next to the label and highlights the textbox/listbox/combobox/radiobutton background for all RequiredFields that
		///have their conditions met.</summary>
		private void SetRequiredFields() {
			_isMissingRequiredFields=false;
			bool areConditionsMet;
			for(int i=0;i<_listRequiredFields.Count;i++) {
				areConditionsMet=ConditionsAreMet(_listRequiredFields[i]);
				if(areConditionsMet) {
					labelRequiredField.Visible=true;
				}
				switch(_listRequiredFields[i].FieldName) {
					case RequiredFieldName.Address:
						SetRequiredTextBox(labelAddress,textAddress,areConditionsMet);
						break;
					case RequiredFieldName.Address2:
						SetRequiredTextBox(labelAddress2,textAddress2,areConditionsMet);
						break;
					case RequiredFieldName.AddressPhoneNotes:
						if(areConditionsMet) {
							if(!groupNotes.Text.Contains("*")) {
								groupNotes.Text+="*";
							}
							if(textAddrNotes.Text=="") {
								_isMissingRequiredFields=true;
								if(_isValidating) {
									_errorProv.SetError(textAddrNotes,Lan.g(this,"Text box cannot be blank"));
								}
							}
							else {
								_errorProv.SetError(textAddrNotes,"");
							}
						}
						else {
							if(groupNotes.Text.Contains("*")) {
								groupNotes.Text=groupNotes.Text.Replace("*","");
							}
							_errorProv.SetError(textAddrNotes,"");
						}
						break;
					case RequiredFieldName.AdmitDate:
						SetRequiredTextBox(labelAdmitDate,textAdmitDate,areConditionsMet);
						break;
					case RequiredFieldName.AskArriveEarly:
						SetRequiredTextBox(labelAskToArriveEarly,textAskToArriveEarly,areConditionsMet);
						break;
					case RequiredFieldName.BillingType:
						SetRequiredComboBox(labelBillType,comboBillType,areConditionsMet,-1,"A billing type must be selected.");
						break;
					case RequiredFieldName.Birthdate:
						SetRequiredTextBox(labelBirthdate,textBirthdate,areConditionsMet);
						break;
					case RequiredFieldName.ChartNumber:
						SetRequiredTextBox(labelChartNumber,textChartNumber,areConditionsMet);
						break;
					case RequiredFieldName.City:
						SetRequiredTextBox(labelCity,textCity,areConditionsMet);
						break;
					case RequiredFieldName.Clinic:
						SetRequiredComboBox(labelClinic,comboClinic,areConditionsMet,0,"Selection cannot be 'Unassigned'.");
						break;
					case RequiredFieldName.CollegeName:
						SetRequiredTextBox(labelSchoolName,textSchool,areConditionsMet);
						break;
					case RequiredFieldName.County:
						SetRequiredTextBox(labelCounty,textCounty,areConditionsMet);
						break;
					case RequiredFieldName.CreditType:
						SetRequiredTextBox(labelCreditType,textCreditType,areConditionsMet);
						break;
					case RequiredFieldName.DateFirstVisit:
						SetRequiredTextBox(labelDateFirstVisit,textDateFirstVisit,areConditionsMet);
						break;
					case RequiredFieldName.DateTimeDeceased:
						SetRequiredTextBox(labelDeceased,textDateDeceased,areConditionsMet);
						break;
					case RequiredFieldName.EligibilityExceptCode:
						SetRequiredComboBox(labelCanadianEligibilityCode,comboCanadianEligibilityCode,areConditionsMet,0,
							"Selection cannot be '0 - Please Choose'.");
						break;
					case RequiredFieldName.EmailAddress:
						SetRequiredTextBox(labelEmail,textEmail,areConditionsMet);
						break;
					case RequiredFieldName.Employer:
						SetRequiredTextBox(labelEmployer,textEmployer,areConditionsMet);
						break;
					case RequiredFieldName.Ethnicity:
						SetRequiredComboBox(labelEthnicity,comboEthnicity,areConditionsMet,0,"Selection cannot be 'none'.");
						break;
					case RequiredFieldName.FeeSchedule:
						SetRequiredComboBox(labelFeeSched,comboFeeSched,areConditionsMet,0,"Selection cannot be 'none'.");
						break;
					case RequiredFieldName.FirstName:
						SetRequiredTextBox(labelFName,textFName,areConditionsMet);
						break;
					case RequiredFieldName.Gender:
						labelGender.Text=labelGender.Text.Replace("*","");
						if(areConditionsMet) {
							labelGender.Text=labelGender.Text+"*";
							if(listGender.Items[listGender.SelectedIndex].ToString()==Lan.g(this,"Unknown")) {
								if(_isValidating) {
									_errorProv.SetError(listGender,Lan.g(this,"Gender cannot be 'Unknown'."));
									_errorProv.SetIconAlignment(listGender,ErrorIconAlignment.BottomRight);
								}
								_isMissingRequiredFields=true;
							}
							else {
								_errorProv.SetError(listGender,"");
							}
						}
						else {
							_errorProv.SetError(listGender,"");
						}
						break;
					case RequiredFieldName.GradeLevel:
						SetRequiredComboBox(labelGradeLevel,comboGradeLevel,areConditionsMet,0,"Selection cannot be 'Unknown'.");
						break;
					case RequiredFieldName.HomePhone:
						SetRequiredTextBox(labelHmPhone,textHmPhone,areConditionsMet);
						break;
					case RequiredFieldName.Language:
						SetRequiredComboBox(labelLanguage,comboLanguage,areConditionsMet,0,"Selection cannot be 'none'.");
						break;
					case RequiredFieldName.LastName:
						SetRequiredTextBox(labelLName,textLName,areConditionsMet);
						break;
					case RequiredFieldName.MedicaidID:
						SetRequiredTextBox(labelMedicaidID,textMedicaidID,areConditionsMet);						
						break;
					case RequiredFieldName.MedicaidState:
						SetRequiredTextBox(labelMedicaidID,textMedicaidState,areConditionsMet);
						if(textMedicaidState.Text!=""	&& !StateAbbrs.IsValidAbbr(textMedicaidState.Text)) {
							_isMissingRequiredFields=true;
							if(_isValidating) {
								_errorProv.SetError(textMedicaidState,Lan.g(this,"Invalid state abbreviation"));
							}
						}
						CheckMedicaidIDLength();						
						break;
					case RequiredFieldName.MiddleInitial:
						SetRequiredTextBox(labelMiddleI,textMiddleI,areConditionsMet);
						break;
					case RequiredFieldName.MothersMaidenFirstName:
						SetRequiredTextBox(labelMotherMaidenFname,textMotherMaidenFname,areConditionsMet);
						break;
					case RequiredFieldName.MothersMaidenLastName:
						SetRequiredTextBox(labelMotherMaidenLname,textMotherMaidenLname,areConditionsMet);
						break;
					case RequiredFieldName.PreferConfirmMethod:
						SetRequiredComboBox(labelConfirm,comboConfirm,areConditionsMet,0,"Selection cannot be 'None'.");
						break;
					case RequiredFieldName.PreferContactMethod:
						SetRequiredComboBox(labelContact,comboContact,areConditionsMet,0,"Selection cannot be 'None'.");
						break;
					case RequiredFieldName.PreferRecallMethod:
						SetRequiredComboBox(labelRecall,comboRecall,areConditionsMet,0,"Selection cannot be 'None'.");
						break;
					case RequiredFieldName.PreferredName:
						SetRequiredTextBox(labelPreferred,textPreferred,areConditionsMet);
						break;
					case RequiredFieldName.PrimaryProvider:
						if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
							SetRequiredComboBox(labelPriProv,comboPriProv,areConditionsMet,0,"Selection cannot be 'Select Provider'.");
						}
						break;
					case RequiredFieldName.Race:
						labelRaceEthnicity.Text=labelRaceEthnicity.Text.Replace("*","");
						if(areConditionsMet) {
							labelRaceEthnicity.Text=labelRaceEthnicity.Text+"*";
							if(comboBoxMultiRace.SelectedIndices.Contains(0)  //'None' is not allowed
								|| comboBoxMultiRace.SelectedIndices.Count==0) 
							{
								_isMissingRequiredFields=true;
								if(_isValidating) {
									_errorProv.SetError(comboBoxMultiRace,Lan.g(this,"Selection cannot be 'none' or blank."));
								}
							}
							else {
								_errorProv.SetError(comboBoxMultiRace,"");
							}
						}
						else {
							_errorProv.SetError(comboBoxMultiRace,"");
						}
						break;
					case RequiredFieldName.ReferredFrom:
						SetRequiredTextBox(labelReferredFrom,textReferredFrom,areConditionsMet);
						break;
					case RequiredFieldName.ResponsibleParty:
						SetRequiredTextBox(labelResponsParty,textResponsParty,areConditionsMet);
						break;
					case RequiredFieldName.Salutation:
						SetRequiredTextBox(labelSalutation,textSalutation,areConditionsMet);
						break;
					case RequiredFieldName.SecondaryProvider:
						SetRequiredComboBox(labelSecProv,comboSecProv,areConditionsMet,0,"Selection cannot be 'none'.");
						break;
					case RequiredFieldName.Site:
						SetRequiredTextBox(labelSite,textSite,areConditionsMet);
						break;
					case RequiredFieldName.SocialSecurityNumber:
						SetRequiredTextBox(labelSSN,textSSN,areConditionsMet);
						break;
					case RequiredFieldName.State:
						SetRequiredTextBox(labelST,textState,areConditionsMet);
						if(textState.Text!=""	&& !StateAbbrs.IsValidAbbr(textState.Text)) {
							_isMissingRequiredFields=true;
							if(_isValidating) {
								_errorProv.SetError(textState,Lan.g(this,"Invalid state abbreviation"));
							}
						}
						break;
					case RequiredFieldName.StudentStatus:
						radioStudentN.Text=radioStudentN.Text.Replace("*","");
						radioStudentF.Text=radioStudentF.Text.Replace("*","");
						radioStudentP.Text=radioStudentP.Text.Replace("*","");
						if(areConditionsMet) {
							radioStudentN.Text=radioStudentN.Text+"*";
							radioStudentF.Text=radioStudentF.Text+"*";
							radioStudentP.Text=radioStudentP.Text+"*";
							if(!radioStudentN.Checked && !radioStudentF.Checked && !radioStudentP.Checked) {
								_isMissingRequiredFields=true;
								if(_isValidating) {
									_errorProv.SetError(radioStudentP,"A student status must be selected.");
								}
							}
							else {
								_errorProv.SetError(radioStudentP,"");
							}
						}
						else {
							_errorProv.SetError(radioStudentP,"");
						}
						break;
					case RequiredFieldName.TextOK:
						labelTextOk.Text=labelTextOk.Text.Replace("*","");						
						if(areConditionsMet) {
							labelTextOk.Text=labelTextOk.Text+"*";
							if(listTextOk.Items[listTextOk.SelectedIndex].ToString()==Lan.g(this,"??")) {
								_isMissingRequiredFields=true;
								if(_isValidating) {
									_errorProv.SetError(listTextOk,Lan.g(this,"Selection cannot be '??'."));
								}
							}
							else {
								_errorProv.SetError(listTextOk,"");
							}
						}
						else {
							_errorProv.SetError(listTextOk,"");
						}
						break;
					case RequiredFieldName.Title:
						SetRequiredTextBox(labelTitle,textTitle,areConditionsMet);
						break;
					case RequiredFieldName.TreatmentUrgency:
						SetRequiredComboBox(labelUrgency,comboUrgency,areConditionsMet,0,"Selection cannot be 'Unknown'.");
						break;
					case RequiredFieldName.TrophyFolder:
						SetRequiredTextBox(labelTrophyFolder,textTrophyFolder,areConditionsMet);
						break;
					case RequiredFieldName.Ward:
						SetRequiredTextBox(labelWard,textWard,areConditionsMet);
						break;
					case RequiredFieldName.WirelessPhone:
						SetRequiredTextBox(labelWirelessPhone,textWirelessPhone,areConditionsMet);
						break;
					case RequiredFieldName.WorkPhone:
						SetRequiredTextBox(labelWkPhone,textWkPhone,areConditionsMet);
						break;
					case RequiredFieldName.Zip:
						SetRequiredTextBox(labelZip,textZip,areConditionsMet);
						break;
				}
			}
		}

		///<summary>Returns true if all the conditions for the RequiredField are met.</summary>
		private bool ConditionsAreMet(RequiredField reqField) {
			List<RequiredFieldCondition> listConditions=reqField.ListRequiredFieldConditions;
			if(listConditions.Count==0) {//This RequiredField is always required
				return true;
			}
			bool areConditionsMet=false;
			int previousFieldName=-1;
			for(int i=0;i<listConditions.Count;i++) {
				if(areConditionsMet && (int)listConditions[i].ConditionType==previousFieldName) {
					continue;//A condition of this type has already been met
				}
				if(!areConditionsMet && previousFieldName!=-1
					&& (int)listConditions[i].ConditionType!=previousFieldName) 
				{
					return false;//None of the conditions of the previous type were met
				}
				areConditionsMet=false;
				switch(listConditions[i].ConditionType) {
					case RequiredFieldName.AdmitDate:
						if(PrefC.GetBool(PrefName.EasyHideHospitals)) {
							areConditionsMet=true;
							break;
						}
						areConditionsMet=CheckDateConditions(textAdmitDate.Text,i,listConditions);
						break;
					case RequiredFieldName.BillingType:
						//Conditions of type BillingType store the DefNum as the ConditionValue.
						long defNumber=DefC.Short[(int)DefCat.BillingTypes][comboBillType.SelectedIndex].DefNum;
						areConditionsMet=ConditionComparerHelper(defNumber.ToString(),i,listConditions);
						break;
					case RequiredFieldName.Birthdate://But actually using Age for calculations						
						if(textAge.Text=="" || textBirthdate.errorProvider1.GetError(textBirthdate)!="") {
							areConditionsMet=false;
							break;
						}
						DateTime birthdate=PIn.Date(textBirthdate.Text);
						if(birthdate>DateTime.Today) {
							birthdate=birthdate.AddYears(-100);
						}
						int ageEntered=DateTime.Today.Year-birthdate.Year;
						if(birthdate>DateTime.Today.AddYears(-ageEntered)) {
							ageEntered--;
						}
						List<RequiredFieldCondition> listAgeConditions=listConditions.FindAll(x => x.ConditionType==RequiredFieldName.Birthdate);
						//There should be no more than 2 conditions of type Birthdate
						List<bool> listAreCondsMet=new List<bool>();
						for(int j=0;j<listAgeConditions.Count;j++) {
							listAreCondsMet.Add(CondOpComparer(ageEntered,listAgeConditions[j].Operator,PIn.Int(listAgeConditions[j].ConditionValue)));
						}
						if(listAreCondsMet.Count<2 || listAgeConditions[1].ConditionRelationship==LogicalOperator.And) {
							areConditionsMet=!listAreCondsMet.Contains(false);
							break;
						}
						areConditionsMet=listAreCondsMet.Contains(true);
						break;
					case RequiredFieldName.Clinic:
						if(!PrefC.HasClinicsEnabled) {
							areConditionsMet=true;
							break;
						}
						//Conditions of type Clinic store the ClinicNum as the ConditionValue. If that value is "0", it represents 'Unassigned'.
						if(comboClinic.SelectedIndex==0) {
							areConditionsMet=ConditionComparerHelper("0",i,listConditions);//"0" for 'Unassigned'
						}
						else {
							areConditionsMet=ConditionComparerHelper(_listClinics[comboClinic.SelectedIndex-1].ClinicNum.ToString(),//Minus 1 for 'Unassigned'
								i,listConditions);
						}		
						break;								
					case RequiredFieldName.DateTimeDeceased:
						if(!PrefC.GetBool(PrefName.ShowFeatureEhr)) {
							areConditionsMet=true;
							break;
						}
						areConditionsMet=CheckDateConditions(textDateDeceased.Text,i,listConditions);
						break;
					case RequiredFieldName.Gender:
						areConditionsMet=ConditionComparerHelper(listGender.Items[listGender.SelectedIndex].ToString(),i,listConditions);
						break;
					case RequiredFieldName.Language:
						areConditionsMet=ConditionComparerHelper(comboLanguage.Items[comboLanguage.SelectedIndex].ToString(),i,listConditions);
						break;
					case RequiredFieldName.MedicaidID:
						if(PrefC.GetBool(PrefName.EasyHideMedicaid)) {
							areConditionsMet=true;
							break;
						}
						//The only possible value for ConditionValue is 'Blank'
						if((listConditions[i].Operator==ConditionOperator.Equals && textMedicaidID.Text=="")
							|| (listConditions[i].Operator==ConditionOperator.NotEquals && textMedicaidID.Text!="")) {
							areConditionsMet=true;
						}
						break;
					case RequiredFieldName.MedicaidState:
						if(PrefC.GetBool(PrefName.EasyHideMedicaid)) {
							areConditionsMet=true;
							break;
						}
						//The only possible value for ConditionValue is 'Blank'
						if((listConditions[i].Operator==ConditionOperator.Equals && textMedicaidState.Text=="")
							|| (listConditions[i].Operator==ConditionOperator.NotEquals && textMedicaidState.Text!="")) {
							areConditionsMet=true;
						}
						break;
					case RequiredFieldName.PatientStatus:
						areConditionsMet=ConditionComparerHelper(listStatus.Items[listStatus.SelectedIndex].ToString(),i,listConditions);
						break;
					case RequiredFieldName.Position:
						areConditionsMet=ConditionComparerHelper(listPosition.Items[listPosition.SelectedIndex].ToString(),i,listConditions);
						break;
					case RequiredFieldName.PrimaryProvider:
						//Conditions of type PrimaryProvider store the ProvNum as the ConditionValue.
						int provIdx=comboPriProv.SelectedIndex;
						if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
							provIdx--;//To account for 'Select Provider'
						}
						if(provIdx<0) {
							break;
						}
						areConditionsMet=ConditionComparerHelper(ProviderC.ListShort[provIdx].ProvNum.ToString(),i,listConditions);
						break;							
					case RequiredFieldName.StudentStatus:
						areConditionsMet=CheckStudentStatusConditions(i,listConditions);
						break;
				}
				previousFieldName=(int)listConditions[i].ConditionType;
			}
			return areConditionsMet;
		}

		///<summary>Returns true if the operator is Equals and the value is in the list of conditions or if the operator is NotEquals and the value is 
		///not in the list of conditions.</summary>
		private bool ConditionComparerHelper(string val,int condCurIndex,List<RequiredFieldCondition> listConds) {
			if(listConds[condCurIndex].Operator==ConditionOperator.Equals 
				&& listConds.FindAll(x => x.ConditionType==listConds[condCurIndex].ConditionType)
						.Any(x => x.ConditionValue==val)) 
			{
				return true;
			}
			if(listConds[condCurIndex].Operator==ConditionOperator.NotEquals 
				&& !listConds.FindAll(x => x.ConditionType==listConds[condCurIndex].ConditionType)
						 .Any(x => x.ConditionValue==val)) 
			{
				return true;
			}
			return false;
		}

		///<summary>Returns true if the conditions for this date condition are true.</summary>
		private bool CheckDateConditions(string dateStr,int condCurIndex,List<RequiredFieldCondition> listConds) {
			DateTime dateCur=DateTime.MinValue;
			if(dateStr=="" || !DateTime.TryParse(dateStr,out dateCur)) {
				return false;
			}
			List<RequiredFieldCondition> listDateConditions=listConds.FindAll(x => x.ConditionType==listConds[condCurIndex].ConditionType);
			if(listDateConditions.Count<1) {
				return false;
			}
			//There should be no more than 2 conditions of a date type
			List<bool> listAreCondsMet=new List<bool>();
			for(int i=0;i<listDateConditions.Count;i++) {
				listAreCondsMet.Add(CondOpComparer(dateCur,listDateConditions[i].Operator,PIn.Date(listDateConditions[i].ConditionValue)));
			}
			if(listAreCondsMet.Count<2 || listDateConditions[1].ConditionRelationship==LogicalOperator.And) {
				return !listAreCondsMet.Contains(false);
			}
			return listAreCondsMet.Contains(true);
		}

		///<summary>Returns true if the conditions for StudentStatus are true.</summary>
		private bool CheckStudentStatusConditions(int condCurIndex,List<RequiredFieldCondition> listConds) {
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) { //Canadian. en-CA or fr-CA
				return true;
			}
			if(listConds[condCurIndex].Operator==ConditionOperator.Equals) {
				if((radioStudentN.Checked && listConds[condCurIndex].ConditionValue==Lan.g(this,"Nonstudent"))
					|| (radioStudentF.Checked && listConds[condCurIndex].ConditionValue==Lan.g(this,"Fulltime"))
					|| (radioStudentP.Checked && listConds[condCurIndex].ConditionValue==Lan.g(this,"Parttime")))
				{
					return true;
				}
				return false;
			}
			else { //condCur.Operator==ConditionOperator.NotEquals
				List<RequiredFieldCondition> listStudentConds=listConds.FindAll(x => x.ConditionType==RequiredFieldName.StudentStatus);
				if((radioStudentN.Checked && listStudentConds.Any(x => x.ConditionValue==Lan.g(this,"Nonstudent")))
					|| (radioStudentF.Checked && listStudentConds.Any(x => x.ConditionValue==Lan.g(this,"Fulltime")))
					|| (radioStudentP.Checked && listStudentConds.Any(x => x.ConditionValue==Lan.g(this,"Parttime"))))
				{
					return false;
				}
				return true;
			}
		}

		///<summary>Evaluates two dates using the provided operator.</summary>
		private bool CondOpComparer(DateTime date1,ConditionOperator oper,DateTime date2) {
			return CondOpComparer(DateTime.Compare(date1,date2),oper,0);
		}

		///<summary>Evaluates two integers using the provided operator.</summary>
		private bool CondOpComparer(int value1,ConditionOperator oper,int value2) {
			switch(oper) {
				case ConditionOperator.Equals:
					return value1==value2;
				case ConditionOperator.NotEquals:
					return value1!=value2;
				case ConditionOperator.GreaterThan:
					return value1>value2;
				case ConditionOperator.GreaterThanOrEqual:
					return value1>=value2;
				case ConditionOperator.LessThan:
					return value1<value2;
				case ConditionOperator.LessThanOrEqual:
					return value1<=value2;
			}
			return false;
		}

		///<summary>Checks to see if the Medicaid ID is the proper number of digits for the Medicaid State.</summary>
		private void CheckMedicaidIDLength() {
			int reqLength=StateAbbrs.GetMedicaidIDLength(textMedicaidState.Text);
			if(reqLength==0 || reqLength==textMedicaidID.Text.Length) {
				return;
			}
			_isMissingRequiredFields=true;
			if(_isValidating) {
				_errorProv.SetError(textMedicaidID,Lan.g(this,"Medicaid ID length must be ")+reqLength.ToString()+Lan.g(this," digits for the state of ")
					+textMedicaidState.Text+".");
			}
		}

		///<summary>Puts an asterisk next to the label if the field is required and the conditions are met. If it also blank, sets the error provider.
		///</summary>
		private void SetRequiredTextBox(Label labelCur,TextBox textBoxCur,bool areConditionsMet) {
			if(areConditionsMet) {
				labelCur.Text=labelCur.Text.Replace("*","")+"*";
				if(textBoxCur.Text=="") {
					_isMissingRequiredFields=true;
					if(_isValidating) {
						_errorProv.SetError(textBoxCur,"Text box cannot be blank.");
					}
				}
				else {
					_errorProv.SetError(textBoxCur,"");
				}
			}
			else {
				labelCur.Text=labelCur.Text.Replace("*","");
				_errorProv.SetError(textBoxCur,"");
			}
			if(textBoxCur.Name==textSite.Name || textBoxCur.Name==textReferredFrom.Name) {
				_errorProv.SetIconPadding(textBoxCur,25);//Width of the pick button
			}
			else if(textBoxCur.Name==textResponsParty.Name) {
				_errorProv.SetIconPadding(textBoxCur,50);//Width of the pick and remove buttons
			}
		}

		///<summary>Puts an asterisk next to the label if the field is required and the conditions are met. If the disallowedIdx is also selected, 
		///ets the error provider.</summary>
		private void SetRequiredComboBox(Label labelCur,ComboBox comboCur,bool areConditionsMet,int disallowedIdx,string errorMsg) {
			if(areConditionsMet) {
				labelCur.Text=labelCur.Text.Replace("*","")+"*";
				if(comboCur.SelectedIndex==disallowedIdx) {
					_isMissingRequiredFields=true;
					if(_isValidating) {
						_errorProv.SetError(comboCur,Lan.g(this,errorMsg));
					}
				}
				else {
					_errorProv.SetError(comboCur,"");
				}
			}
			else {
				labelCur.Text=labelCur.Text.Replace("*","");
				_errorProv.SetError(comboCur,"");
			} 
			if(comboCur.Name==comboPriProv.Name || comboCur.Name==comboSecProv.Name) {
				_errorProv.SetIconPadding(comboCur,25);//Width of the pick button
			}
		}			

		private void textBox_Leave(object sender,System.EventArgs e) {
			SetRequiredFields();
		}
		
		private void ListBox_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(_isLoad) {
				return;
			}
			SetRequiredFields();
		}

		private void ComboBox_SelectionChangeCommited(object sender,System.EventArgs e) {
			SetRequiredFields();
		}

		//private void butSecClear_Click(object sender, System.EventArgs e) {
		//	listSecProv.SelectedIndex=-1;
		//}

		//private void butClearFee_Click(object sender, System.EventArgs e) {
		//	listFeeSched.SelectedIndex=-1;
		//}

		private void textLName_TextChanged(object sender, System.EventArgs e) {
			if(textLName.Text.Length==1){
				textLName.Text=textLName.Text.ToUpper();
				textLName.SelectionStart=1;
			}
		}

		private void textFName_TextChanged(object sender, System.EventArgs e) {
			if(textFName.Text.Length==1){
				textFName.Text=textFName.Text.ToUpper();
				textFName.SelectionStart=1;
			}
		}

		private void textMiddleI_TextChanged(object sender, System.EventArgs e) {
			if(textMiddleI.Text.Length==1){
				textMiddleI.Text=textMiddleI.Text.ToUpper();
				textMiddleI.SelectionStart=1;
			}
		}

		private void textPreferred_TextChanged(object sender, System.EventArgs e) {
			if(textPreferred.Text.Length==1){
				textPreferred.Text=textPreferred.Text.ToUpper();
				textPreferred.SelectionStart=1;
			}
		}

		private void textSalutation_TextChanged(object sender, System.EventArgs e) {
			if(textSalutation.Text.Length==1){
				textSalutation.Text=textSalutation.Text.ToUpper();
				textSalutation.SelectionStart=1;
			}
		}

		private void textAddress_TextChanged(object sender, System.EventArgs e) {
			if(textAddress.Text.Length==1){
				textAddress.Text=textAddress.Text.ToUpper();
				textAddress.SelectionStart=1;
			}
		}

		private void textAddress2_TextChanged(object sender, System.EventArgs e) {
			if(textAddress2.Text.Length==1){
				textAddress2.Text=textAddress2.Text.ToUpper();
				textAddress2.SelectionStart=1;
			}
		}

		private void radioStudentN_Click(object sender, System.EventArgs e) {
			PatCur.StudentStatus="N";
			SetRequiredFields();
		}

		private void radioStudentF_Click(object sender, System.EventArgs e) {
			PatCur.StudentStatus="F";
			SetRequiredFields();
		}

		private void radioStudentP_Click(object sender, System.EventArgs e) {
			PatCur.StudentStatus="P";
			SetRequiredFields();
		}

		private void textSSN_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(CultureInfo.CurrentCulture.Name!="en-US"){
				return;
			}
			//only reformats if in USA and exactly 9 digits.
			if(textSSN.Text==""){
				return;
			}
			if(textSSN.Text.Length==9){//if just numbers, try to reformat.
				bool SSNisValid=true;
				for(int i=0;i<textSSN.Text.Length;i++){
					if(!Char.IsNumber(textSSN.Text,i)){
						SSNisValid=false;
					}
				}
				if(SSNisValid){
					textSSN.Text=textSSN.Text.Substring(0,3)+"-"
						+textSSN.Text.Substring(3,2)+"-"+textSSN.Text.Substring(5,4);	
				}
			}
			if(!Regex.IsMatch(textSSN.Text,@"^\d\d\d-\d\d-\d\d\d\d$")){
				if(MessageBox.Show("SSN not valid. Continue anyway?","",MessageBoxButtons.OKCancel)
					!=DialogResult.OK)
				{
					e.Cancel=true;
				}
			}		
		}

		private void textCreditType_TextChanged(object sender, System.EventArgs e) {
			textCreditType.Text=textCreditType.Text.ToUpper();
			textCreditType.SelectionStart=1;
		}

		private void textBirthdate_Validated(object sender, System.EventArgs e) {
			CalcAge();
		}

		private void textDateDeceased_Validated(object sender,EventArgs e) {
			CalcAge();
		}

		private void CalcAge() {
			textAge.Text="";
			if(textBirthdate.errorProvider1.GetError(textBirthdate)!="") {
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate.Text);
			if(birthdate>DateTime.Today) {
				birthdate=birthdate.AddYears(-100);
			}
			DateTime dateTimeTo=DateTime.Now;
			if(textDateDeceased.Text!="") {
				try {
					dateTimeTo=DateTime.Parse(textDateDeceased.Text);
				}
				catch {
					return;
				}
			}
			textAge.Text=PatientLogic.DateToAgeString(birthdate,dateTimeTo);
			SetRequiredFields();
		}

		private void textZip_TextChanged(object sender, System.EventArgs e) {
			comboZip.SelectedIndex=-1;
		}

		private void comboZip_SelectionChangeCommitted(object sender, System.EventArgs e) {
			//this happens when a zipcode is selected from the combobox of frequent zips.
			//The combo box is tucked under textZip because Microsoft makes stupid controls.
			textCity.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).City;
			textState.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).State;
			textZip.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).ZipCodeDigits;
			SetRequiredFields();
		}

		private void textZip_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			//fired as soon as control loses focus.
			//it's here to validate if zip is typed in to text box instead of picked from list.
			//if(textZip.Text=="" && (textCity.Text!="" || textState.Text!="")){
			//	if(MessageBox.Show(Lan.g(this,"Delete the City and State?"),"",MessageBoxButtons.OKCancel)
			//		==DialogResult.OK){
			//		textCity.Text="";
			//		textState.Text="";
			//	}	
			//	return;
			//}
			if(textZip.Text.Length<5){
				return;
			}
			if(comboZip.SelectedIndex!=-1){
				return;
			}
			//the autofill only works if both city and state are left blank
			if(textCity.Text!="" || textState.Text!=""){
				return;
			}
			ZipCodes.GetALMatches(textZip.Text);
			if(ZipCodes.ALMatches.Count==0){
				//No match found. Must enter info for new zipcode
				ZipCode ZipCodeCur=new ZipCode();
				ZipCodeCur.ZipCodeDigits=textZip.Text;
				FormZipCodeEdit FormZE=new FormZipCodeEdit();
				FormZE.ZipCodeCur=ZipCodeCur;
				FormZE.IsNew=true;
				FormZE.ShowDialog();
				if(FormZE.DialogResult!=DialogResult.OK){
					return;
				}
				DataValid.SetInvalid(InvalidType.ZipCodes);//FormZipCodeEdit does not contain internal refresh
				FillComboZip();
				textCity.Text=ZipCodeCur.City;
				textState.Text=ZipCodeCur.State;
				textZip.Text=ZipCodeCur.ZipCodeDigits;
			}
			else if(ZipCodes.ALMatches.Count==1){
				//only one match found.  Use it.
				textCity.Text=((ZipCode)ZipCodes.ALMatches[0]).City;
				textState.Text=((ZipCode)ZipCodes.ALMatches[0]).State;
			}
			else{
				//multiple matches found.  Pick one
				FormZipSelect FormZS=new FormZipSelect();
				FormZS.ShowDialog();
				FillComboZip();
				if(FormZS.DialogResult!=DialogResult.OK){
					return;
				}
				DataValid.SetInvalid(InvalidType.ZipCodes);
				textCity.Text=FormZS.ZipSelected.City;
				textState.Text=FormZS.ZipSelected.State;
				textZip.Text=FormZS.ZipSelected.ZipCodeDigits;
			}
			SetRequiredFields();
		}

		private void checkSuperBilling_MouseDown(object sender,MouseEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
		}

		private void butEditZip_Click(object sender, System.EventArgs e) {
			if(textZip.Text.Length==0){
				MessageBox.Show(Lan.g(this,"Please enter a zipcode first."));
				return;
			}
			ZipCodes.GetALMatches(textZip.Text);
			if(ZipCodes.ALMatches.Count==0){
				FormZipCodeEdit FormZE=new FormZipCodeEdit();
				FormZE.ZipCodeCur=new ZipCode();
				FormZE.ZipCodeCur.ZipCodeDigits=textZip.Text;
				FormZE.IsNew=true;
				FormZE.ShowDialog();
				FillComboZip();
				if(FormZE.DialogResult!=DialogResult.OK){
					return;
				}
				DataValid.SetInvalid(InvalidType.ZipCodes);
				textCity.Text=FormZE.ZipCodeCur.City;
				textState.Text=FormZE.ZipCodeCur.State;
				textZip.Text=FormZE.ZipCodeCur.ZipCodeDigits;
			}
			else{
				FormZipSelect FormZS=new FormZipSelect();
				FormZS.ShowDialog();
				FillComboZip();
				if(FormZS.DialogResult!=DialogResult.OK){
					return;
				}
				//Not needed:
				//DataValid.SetInvalid(InvalidTypes.ZipCodes);
				textCity.Text=FormZS.ZipSelected.City;
				textState.Text=FormZS.ZipSelected.State;
				textZip.Text=FormZS.ZipSelected.ZipCodeDigits;
			}
		}

		private void butShowMap_Click(object sender,EventArgs e) {
			if(textAddress.Text=="" 
				|| textCity.Text=="" 
				|| textState.Text=="") 
			{
				MsgBox.Show(this,"Please fill in Address, City, and ST before using maps.");
				return;
			}
			try {
				Process.Start("http://maps.google.com/maps?t=m&q="+textAddress.Text+" "+textAddress2.Text+" "+textCity.Text+" "+textState.Text);
			}
			catch {
				MsgBox.Show(this,"Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet then try again.");
			}
		}

		///<summary>All text boxes on this form that accept a phone number use this text changed event.</summary>
		private void textAnyPhoneNumber_TextChanged(object sender,System.EventArgs e) {
			if(sender.GetType()!=typeof(TextBox)) {
				return;
			}
			TextBox textPhone=(TextBox)sender;
			int phoneTextPosition=textPhone.SelectionStart;
			int textLength=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			int diff=textPhone.Text.Length-textLength;
			textPhone.SelectionStart=phoneTextPosition+diff;
		}

		private void butAuto_Click(object sender, System.EventArgs e) {
			try {
				textChartNumber.Text=Patients.GetNextChartNum();
				_errorProv.SetError(textChartNumber,"");
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void textCity_TextChanged(object sender, System.EventArgs e) {
			if(textCity.Text.Length==1){
				textCity.Text=textCity.Text.ToUpper();
				textCity.SelectionStart=1;
			}
		}

		private void textState_TextChanged(object sender, System.EventArgs e) {
			if(CultureInfo.CurrentCulture.Name=="en-US" //if USA or Canada, capitalize first 2 letters
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textState.Text.Length==1 || textState.Text.Length==2){
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=2;
				}
			}
			else{
				if(textState.Text.Length==1){
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=1;
				}
			}
		}

		private void textMedicaidState_TextChanged(object sender,EventArgs e) {
			if(CultureInfo.CurrentCulture.Name=="en-US" //if USA or Canada, capitalize first 2 letters
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) //Canadian. en-CA or fr-CA
			{
				if(textMedicaidState.Text.Length==1 || textMedicaidState.Text.Length==2) {
					textMedicaidState.Text=textMedicaidState.Text.ToUpper();
					textMedicaidState.SelectionStart=2;
				}
			}
			else {
				if(textMedicaidState.Text.Length==1) {
					textMedicaidState.Text=textMedicaidState.Text.ToUpper();
					textMedicaidState.SelectionStart=1;
				}
			}
		}

		///<summary>Validates there is still a last name entered and updates the last, first, middle and preferred name of PatCur to what is currently
		///typed in the text boxes.  May not match what is in the database.  Does not save the changes to the database so user can safely cancel and
		///revert the changes.</summary>
		private void UpdateLocalNameHelper() {
			if(textLName.Text=="") {
				MsgBox.Show(this,"Last Name must be entered.");
				return;
			}
			PatCur.LName=textLName.Text;
			PatCur.FName=textFName.Text;
			PatCur.MiddleI=textMiddleI.Text;
			PatCur.Preferred=textPreferred.Text;
			for(int i=0;i<FamCur.ListPats.Length;i++) {//update the Family object as well
				if(FamCur.ListPats[i].PatNum==PatCur.PatNum) {
					FamCur.ListPats[i]=PatCur.Copy();
					break;
				}
			}
			if(PatCur.ResponsParty==PatCur.PatNum) {
				textResponsParty.Text=PatCur.GetNameLF();
			}
			for(int i=0;i<GuardianList.Count;i++) {
				if(GuardianList[i].PatNumGuardian==PatCur.PatNum) {
					listRelationships.Items[i]=Patients.GetNameFirst(PatCur.FName,PatCur.Preferred)+" "
						+Guardians.GetGuardianRelationshipStr(GuardianList[i].Relationship);
					//don't break out of loop since it is possible to add multiple relationships with this patient as the PatNumGuardian
					//break;
				}
			}
		}

		#region Public Health
		
		private void textCounty_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return){
				listCounties.Visible=false;
				comboGradeLevel.Focus();
				return;
			}
			if(textCounty.Text==""){
				listCounties.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down){
				if(listCounties.Items.Count==0){
					return;
				}
				if(listCounties.SelectedIndex==-1){
					listCounties.SelectedIndex=0;
					textCounty.Text=listCounties.SelectedItem.ToString();
				}
				else if(listCounties.SelectedIndex==listCounties.Items.Count-1){
					listCounties.SelectedIndex=-1;
					textCounty.Text=countyOriginal;
				}
				else{
					listCounties.SelectedIndex++;
					textCounty.Text=listCounties.SelectedItem.ToString();
				}
				textCounty.SelectionStart=textCounty.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up){
				if(listCounties.Items.Count==0){
					return;
				}
				if(listCounties.SelectedIndex==-1){
					listCounties.SelectedIndex=listCounties.Items.Count-1;
					textCounty.Text=listCounties.SelectedItem.ToString();
				}
				else if(listCounties.SelectedIndex==0){
					listCounties.SelectedIndex=-1;
					textCounty.Text=countyOriginal;
				}
				else{
					listCounties.SelectedIndex--;
					textCounty.Text=listCounties.SelectedItem.ToString();
				}
				textCounty.SelectionStart=textCounty.Text.Length;
				return;
			}
			if(textCounty.Text.Length==1){
				textCounty.Text=textCounty.Text.ToUpper();
				textCounty.SelectionStart=1;
			}
			countyOriginal=textCounty.Text;//the original text is preserved when using up and down arrows
			listCounties.Items.Clear();
			CountiesList=Counties.Refresh(textCounty.Text);
			//similarSchools=
				//Carriers.GetSimilarNames(textCounty.Text);
			for(int i=0;i<CountiesList.Length;i++){
				listCounties.Items.Add(CountiesList[i].CountyName);
			}
			int h=13*CountiesList.Length+5;
			if(h > ClientSize.Height-listCounties.Top)
				h=ClientSize.Height-listCounties.Top;
			listCounties.Size=new Size(textCounty.Width,h);
			listCounties.Visible=true;
		}

		private void textCounty_Leave(object sender, System.EventArgs e) {
			if(mouseIsInListCounties){
				return;
			}
			//or if user clicked on a different text box.
			if(listCounties.SelectedIndex!=-1){
				textCounty.Text=CountiesList[listCounties.SelectedIndex].CountyName;
			}
			listCounties.Visible=false;
			SetRequiredFields();
		}

		private void listCounties_Click(object sender, System.EventArgs e){
			textCounty.Text=CountiesList[listCounties.SelectedIndex].CountyName;
			textCounty.Focus();
			textCounty.SelectionStart=textCounty.Text.Length;
			listCounties.Visible=false;
		}

		private void listCounties_MouseEnter(object sender, System.EventArgs e){
			mouseIsInListCounties=true;
		}

		private void listCounties_MouseLeave(object sender, System.EventArgs e){
			mouseIsInListCounties=false;
		}

		private void textSite_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				listSites.Visible=false;
				comboGradeLevel.Focus();
				return;
			}
			if(textSite.Text=="") {
				listSites.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listSites.Items.Count==0) {
					return;
				}
				if(listSites.SelectedIndex==-1) {
					listSites.SelectedIndex=0;
					textSite.Text=listSites.SelectedItem.ToString();
				}
				else if(listSites.SelectedIndex==listSites.Items.Count-1) {
					listSites.SelectedIndex=-1;
					textSite.Text=SiteOriginal;
				}
				else {
					listSites.SelectedIndex++;
					textSite.Text=listSites.SelectedItem.ToString();
				}
				textSite.SelectionStart=textSite.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listSites.Items.Count==0) {
					return;
				}
				if(listSites.SelectedIndex==-1) {
					listSites.SelectedIndex=listSites.Items.Count-1;
					textSite.Text=listSites.SelectedItem.ToString();
				}
				else if(listSites.SelectedIndex==0) {
					listSites.SelectedIndex=-1;
					textSite.Text=SiteOriginal;
				}
				else {
					listSites.SelectedIndex--;
					textSite.Text=listSites.SelectedItem.ToString();
				}
				textSite.SelectionStart=textSite.Text.Length;
				return;
			}
			if(textSite.Text.Length==1) {
				textSite.Text=textSite.Text.ToUpper();
				textSite.SelectionStart=1;
			}
			SiteOriginal=textSite.Text;//the original text is preserved when using up and down arrows
			listSites.Items.Clear();
			listSitesFiltered=Sites.GetListFiltered(textSite.Text);
			//similarSchools=
			//Carriers.GetSimilarNames(textSite.Text);
			for(int i=0;i<listSitesFiltered.Count;i++) {
				listSites.Items.Add(listSitesFiltered[i].Description);
			}
			int h=13*listSitesFiltered.Count+5;
			if(h > ClientSize.Height-listSites.Top) {
				h=ClientSize.Height-listSites.Top;
			}
			listSites.Size=new Size(textSite.Width,h);
			listSites.Visible=true;
		}

		private void textSite_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListSites) {
				return;
			}
			//or if user clicked on a different text box.
			if(listSites.SelectedIndex!=-1) {
				textSite.Text=listSitesFiltered[listSites.SelectedIndex].Description;
				PatCur.SiteNum=listSitesFiltered[listSites.SelectedIndex].SiteNum;
			}
			listSites.Visible=false;
			SetRequiredFields();
		}

		private void listSites_Click(object sender,System.EventArgs e) {
			textSite.Text=listSitesFiltered[listSites.SelectedIndex].Description;
			PatCur.SiteNum=listSitesFiltered[listSites.SelectedIndex].SiteNum;
			textSite.Focus();
			textSite.SelectionStart=textSite.Text.Length;
			listSites.Visible=false;
		}

		private void listSites_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListSites=true;
		}

		private void listSites_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListSites=false;
		}

		private void butPickSite_Click(object sender,EventArgs e) {
			FormSites FormS=new FormSites();
			FormS.IsSelectionMode=true;
			FormS.SelectedSiteNum=PatCur.SiteNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			PatCur.SiteNum=FormS.SelectedSiteNum;
			textSite.Text=Sites.GetDescription(PatCur.SiteNum);
			SetRequiredFields();
		}

		private void butPickResponsParty_Click(object sender,EventArgs e) {
			UpdateLocalNameHelper();
			FormFamilyMemberSelect FormF=new FormFamilyMemberSelect(FamCur);
			FormF.ShowDialog();
			if(FormF.DialogResult!=DialogResult.OK) {
				return;
			}
			PatCur.ResponsParty=FormF.SelectedPatNum;
			//saves a call to the db if this pat's responsible party is self and name in db could be different than local PatCur name
			if(PatCur.PatNum==PatCur.ResponsParty) {
				textResponsParty.Text=PatCur.GetNameLF();
			}
			else {
				textResponsParty.Text=Patients.GetLim(PatCur.ResponsParty).GetNameLF();
			}
			_errorProv.SetError(textResponsParty,"");
		}

		private void butClearResponsParty_Click(object sender,EventArgs e) {
			PatCur.ResponsParty=0;
			textResponsParty.Text="";
			SetRequiredFields();
		}
		#endregion

		/*private void butChangeEmp_Click(object sender, System.EventArgs e) {
			FormEmployers FormE=new FormEmployers();
			FormE.IsSelectMode=true;
			Employers.Cur=Employers.GetEmployer(PatCur.EmployerNum);
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			PatCur.EmployerNum=Employers.Cur.EmployerNum;
			textEmployer.Text=Employers.Cur.EmpName;
		}*/

		private void textEmployer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return){
				listEmps.Visible=false;
				textWirelessPhone.Focus();
				return;
			}
			if(textEmployer.Text==""){
				listEmps.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down){
				if(listEmps.Items.Count==0){
					return;
				}
				if(listEmps.SelectedIndex==-1){
					listEmps.SelectedIndex=0;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				else if(listEmps.SelectedIndex==listEmps.Items.Count-1){
					listEmps.SelectedIndex=-1;
					textEmployer.Text=empOriginal;
				}
				else{
					listEmps.SelectedIndex++;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				textEmployer.SelectionStart=textEmployer.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up){
				if(listEmps.Items.Count==0){
					return;
				}
				if(listEmps.SelectedIndex==-1){
					listEmps.SelectedIndex=listEmps.Items.Count-1;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				else if(listEmps.SelectedIndex==0){
					listEmps.SelectedIndex=-1;
					textEmployer.Text=empOriginal;
				}
				else{
					listEmps.SelectedIndex--;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				textEmployer.SelectionStart=textEmployer.Text.Length;
				return;
			}
			if(textEmployer.Text.Length==1){
				textEmployer.Text=textEmployer.Text.ToUpper();
				textEmployer.SelectionStart=1;
			}
			empOriginal=textEmployer.Text;//the original text is preserved when using up and down arrows
			listEmps.Items.Clear();
			List<Employer> similarEmps=Employers.GetSimilarNames(textEmployer.Text);
			for(int i=0;i<similarEmps.Count;i++){
				listEmps.Items.Add(similarEmps[i].EmpName);
			}
			int h=13*similarEmps.Count+5;
			if(h > ClientSize.Height-listEmps.Top){
				h=ClientSize.Height-listEmps.Top;
			}
			listEmps.Size=new Size(231,h);
			listEmps.Visible=true;
		}

		private void textEmployer_Leave(object sender, System.EventArgs e) {
			if(mouseIsInListEmps){
				return;
			}
			listEmps.Visible=false;
			SetRequiredFields();
		}

		private void listEmps_Click(object sender, System.EventArgs e){
			textEmployer.Text=listEmps.SelectedItem.ToString();
			textEmployer.Focus();
			textEmployer.SelectionStart=textEmployer.Text.Length;
			listEmps.Visible=false;
		}

		private void listEmps_DoubleClick(object sender, System.EventArgs e){
			//no longer used
			textEmployer.Text=listEmps.SelectedItem.ToString();
			textEmployer.Focus();
			textEmployer.SelectionStart=textEmployer.Text.Length;
			listEmps.Visible=false;
		}

		private void listEmps_MouseEnter(object sender, System.EventArgs e){
			mouseIsInListEmps=true;
		}

		private void listEmps_MouseLeave(object sender, System.EventArgs e){
			mouseIsInListEmps=false;
		}

		private void textMedicaidState_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listMedicaidStates.Visible=false;
				return;
			}
			if(textMedicaidState.Text=="") {
				listMedicaidStates.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listMedicaidStates.Items.Count==0) {
					return;
				}
				if(listMedicaidStates.SelectedIndex==-1) {
					listMedicaidStates.SelectedIndex=0;
					textMedicaidState.Text=listMedicaidStates.SelectedItem.ToString();
				}
				else if(listMedicaidStates.SelectedIndex==listMedicaidStates.Items.Count-1) {
					listMedicaidStates.SelectedIndex=-1;
					textMedicaidState.Text=_medicaidStateOriginal;
				}
				else {
					listMedicaidStates.SelectedIndex++;
					textMedicaidState.Text=listMedicaidStates.SelectedItem.ToString();
				}
				textMedicaidState.SelectionStart=textMedicaidState.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listMedicaidStates.Items.Count==0) {
					return;
				}
				if(listMedicaidStates.SelectedIndex==-1) {
					listMedicaidStates.SelectedIndex=listMedicaidStates.Items.Count-1;
					textMedicaidState.Text=listMedicaidStates.SelectedItem.ToString();
				}
				else if(listMedicaidStates.SelectedIndex==0) {
					listMedicaidStates.SelectedIndex=-1;
					textMedicaidState.Text=_medicaidStateOriginal;
				}
				else {
					listMedicaidStates.SelectedIndex--;
					textMedicaidState.Text=listMedicaidStates.SelectedItem.ToString();
				}
				textMedicaidState.SelectionStart=textMedicaidState.Text.Length;
				return;
			}
			if(textMedicaidState.Text.Length==1) {
				textMedicaidState.Text=textMedicaidState.Text.ToUpper();
				textMedicaidState.SelectionStart=1;
			}
			_medicaidStateOriginal=textMedicaidState.Text;//the original text is preserved when using up and down arrows
			listMedicaidStates.Items.Clear();
			List<StateAbbr> similarAbbrs=StateAbbrs.GetSimilarAbbrs(textMedicaidState.Text);
			for(int i=0;i<similarAbbrs.Count;i++) {
				listMedicaidStates.Items.Add(similarAbbrs[i].Abbr);
			}
			int h=13*similarAbbrs.Count+5;
			if(h > ClientSize.Height-listMedicaidStates.Top) {
				h=ClientSize.Height-listMedicaidStates.Top;
			}
			listMedicaidStates.Size=new Size(textMedicaidState.Width,h);
			listMedicaidStates.Visible=true;
		}

		private void textMedicaidState_Leave(object sender,System.EventArgs e) {
			if(_mouseIsInListMedicaidStates) {
				return;
			}
			listMedicaidStates.Visible=false;
			SetRequiredFields();
		}

		private void listMedicaidStates_Click(object sender,System.EventArgs e) {
			textMedicaidState.Text=listMedicaidStates.SelectedItem.ToString();
			textMedicaidState.Focus();
			textMedicaidState.SelectionStart=textMedicaidState.Text.Length;
			listMedicaidStates.Visible=false;
		}

		private void listMedicaidStates_MouseEnter(object sender,System.EventArgs e) {
			_mouseIsInListMedicaidStates=true;
		}

		private void listMedicaidStates_MouseLeave(object sender,System.EventArgs e) {
			_mouseIsInListMedicaidStates=false;
		}

		private void textState_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listStates.Visible=false;
				return;
			}
			if(textState.Text=="") {
				listStates.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listStates.Items.Count==0) {
					return;
				}
				if(listStates.SelectedIndex==-1) {
					listStates.SelectedIndex=0;
					textState.Text=listStates.SelectedItem.ToString();
				}
				else if(listStates.SelectedIndex==listStates.Items.Count-1) {
					listStates.SelectedIndex=-1;
					textState.Text=_stateOriginal;
				}
				else {
					listStates.SelectedIndex++;
					textState.Text=listStates.SelectedItem.ToString();
				}
				textState.SelectionStart=textState.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listStates.Items.Count==0) {
					return;
				}
				if(listStates.SelectedIndex==-1) {
					listStates.SelectedIndex=listStates.Items.Count-1;
					textState.Text=listStates.SelectedItem.ToString();
				}
				else if(listStates.SelectedIndex==0) {
					listStates.SelectedIndex=-1;
					textState.Text=_stateOriginal;
				}
				else {
					listStates.SelectedIndex--;
					textState.Text=listStates.SelectedItem.ToString();
				}
				textState.SelectionStart=textState.Text.Length;
				return;
			}
			if(textState.Text.Length==1) {
				textState.Text=textState.Text.ToUpper();
				textState.SelectionStart=1;
			}
			_stateOriginal=textState.Text;//the original text is preserved when using up and down arrows
			listStates.Items.Clear();
			List<StateAbbr> similarAbbrs=StateAbbrs.GetSimilarAbbrs(textState.Text);
			for(int i=0;i<similarAbbrs.Count;i++) {
				listStates.Items.Add(similarAbbrs[i].Abbr);
			}
			int h=13*similarAbbrs.Count+5;
			if(h > ClientSize.Height-listStates.Top) {
				h=ClientSize.Height-listStates.Top;
			}
			listStates.Size=new Size(textState.Width,h);
			listStates.Visible=true;
		}

		private void textState_Leave(object sender,System.EventArgs e) {
			if(_mouseIsInListStates) {
				return;
			}
			listStates.Visible=false;
			SetRequiredFields();
		}

		private void listStates_Click(object sender,System.EventArgs e) {
			textState.Text=listStates.SelectedItem.ToString();
			textState.Focus();
			textState.SelectionStart=textState.Text.Length;
			listStates.Visible=false;
		}

		private void listStates_MouseEnter(object sender,System.EventArgs e) {
			_mouseIsInListStates=true;
		}

		private void listStates_MouseLeave(object sender,System.EventArgs e) {
			_mouseIsInListStates=false;
		}

		private void listPosition_SelectedIndexChanged(object sender,EventArgs e) {
			if(!_isLoad) {
				SetRequiredFields();
			}
			//CheckGuardianUiState();
		}

		private void listRelationships_DoubleClick(object sender,EventArgs e) {
			if(listRelationships.SelectedIndex==-1) {
				return;
			}
			UpdateLocalNameHelper();
			FormGuardianEdit formG=new FormGuardianEdit(GuardianList[listRelationships.SelectedIndex],FamCur);
			if(formG.ShowDialog()==DialogResult.OK) {
				FillGuardians();
			}
		}

		private void butAddGuardian_Click(object sender,EventArgs e) {
			UpdateLocalNameHelper();
			Guardian guardian=new Guardian();
			guardian.IsNew=true;
			guardian.PatNumChild=PatCur.PatNum;
			//no patnumGuardian set
			FormGuardianEdit formG=new FormGuardianEdit(guardian,FamCur);
			if(formG.ShowDialog()==DialogResult.OK) {
				FillGuardians();
			}
		}

		private void butGuardianDefaults_Click(object sender,EventArgs e) {
			if(Guardians.ExistForFamily(PatCur.Guarantor)) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Replace existing relationships with default relationships for entire family?")) {
					return;
				}
				//don't delete existing guardians for family until we are certain we can replace them with the defaults
				//Guardians.DeleteForFamily(PatCur.Guarantor);
			}
			List<Patient> listAdults=new List<Patient>();
			List<Patient> listChildren=new List<Patient>();
			PatientPosition pos;
			for(int p=0;p<FamCur.ListPats.Length;p++){
				if(FamCur.ListPats[p].PatNum==PatCur.PatNum) {
					pos=(PatientPosition)listPosition.SelectedIndex;
				}
				else {
					pos=FamCur.ListPats[p].Position;
				}
				if(pos==PatientPosition.Child){
					listChildren.Add(FamCur.ListPats[p]);
				}
				else{
					listAdults.Add(FamCur.ListPats[p]);
				}
			}
			Patient eldestMaleAdult=null;
			Patient eldestFemaleAdult=null;
			for(int i=0;i<listAdults.Count;i++) {
				if(listAdults[i].Gender==PatientGender.Male 
					&& (eldestMaleAdult==null || listAdults[i].Age>eldestMaleAdult.Age)) 
				{
						eldestMaleAdult=listAdults[i];
				}
				if(listAdults[i].Gender==PatientGender.Female
					&& (eldestFemaleAdult==null || listAdults[i].Age>eldestFemaleAdult.Age)) 
				{
					eldestFemaleAdult=listAdults[i];
				}
				//Do not do anything for the other genders.
			}
			if(listAdults.Count<1) {
				MsgBox.Show(this,"No adults found.\r\nFamily relationships will not be changed.");
				return;
			}
			if(listChildren.Count<1) {
				MsgBox.Show(this,"No children found.\r\nFamily relationships will not be changed.");
				return;
			}
			if(eldestFemaleAdult==null && eldestMaleAdult==null) {
				MsgBox.Show(this,"No male or female adults found.\r\nFamily relationships will not be changed.");
				return;
			}
			if(Guardians.ExistForFamily(PatCur.Guarantor)) {
				//delete all guardians for the family, original family relationships are saved on load so this can be undone if the user presses cancel.
				Guardians.DeleteForFamily(PatCur.Guarantor);
			}
			for(int i=0;i<listChildren.Count;i++) {
				if(eldestFemaleAdult!=null) {
					//Create Parent=>Child relationship
					Guardian motherGuard=new Guardian();
					motherGuard.PatNumChild=eldestFemaleAdult.PatNum;
					motherGuard.PatNumGuardian=listChildren[i].PatNum;
					motherGuard.Relationship=GuardianRelationship.Child;
					Guardians.Insert(motherGuard);
					//Create Child=>Parent relationship
					Guardian childGuard=new Guardian();
					childGuard.PatNumChild=listChildren[i].PatNum;
					childGuard.PatNumGuardian=eldestFemaleAdult.PatNum;
					childGuard.Relationship=GuardianRelationship.Mother;
					childGuard.IsGuardian=true;
					Guardians.Insert(childGuard);
				}
				if(eldestMaleAdult!=null) {
					//Create Parent=>Child relationship
					Guardian fatherGuard=new Guardian();
					fatherGuard.PatNumChild=eldestMaleAdult.PatNum;
					fatherGuard.PatNumGuardian=listChildren[i].PatNum;
					fatherGuard.Relationship=GuardianRelationship.Child;
					Guardians.Insert(fatherGuard);
					//Create Child=>Parent relationship
					Guardian childGuard=new Guardian();
					childGuard.PatNumChild=listChildren[i].PatNum;
					childGuard.PatNumGuardian=eldestMaleAdult.PatNum;
					childGuard.Relationship=GuardianRelationship.Father;
					childGuard.IsGuardian=true;
					Guardians.Insert(childGuard);
				}
			}
			FillGuardians();
		}

		private void butPickPrimary_Click(object sender,EventArgs e) {
			FormProviderPick formp=new FormProviderPick();
			if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				if(comboPriProv.SelectedIndex>0) {
					formp.SelectedProvNum=ProviderC.ListShort[comboPriProv.SelectedIndex-1].ProvNum;//Minus 1 to account for 'Select Provider'
				}
			}
			else {
				if(comboPriProv.SelectedIndex>-1) {
					formp.SelectedProvNum=ProviderC.ListShort[comboPriProv.SelectedIndex].ProvNum;
				}
			}
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			int indexOffset=Providers.GetIndex(formp.SelectedProvNum);
			if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				indexOffset++;//Plus 1 to account for 'Select Provider'
			}
			comboPriProv.SelectedIndex=indexOffset;
		}

		private void butPickSecondary_Click(object sender,EventArgs e) {
			FormProviderPick formp=new FormProviderPick();
			if(comboSecProv.SelectedIndex>0) {
				formp.SelectedProvNum=ProviderC.ListShort[comboSecProv.SelectedIndex-1].ProvNum;
			}
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			comboSecProv.SelectedIndex=Providers.GetIndex(formp.SelectedProvNum)+1;
		}
		
		private void comboBoxMultiRace_SelectionChangeCommitted(object sender,EventArgs e) {
			RemoveIllogicalRaceCombinations();
			SetRequiredFields();
		}

		///<summary>Disallows the user from selecting illogical combinations such as 'DeclinedToSpecify' and 'Asian'.</summary>
		private void RemoveIllogicalRaceCombinations() {
			if(comboBoxMultiRace.ListSelectedIndices.Count<2) {
				return;
			}
			int declinedIdx;
			int otherIdx;
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				declinedIdx=4;
				otherIdx=6;
			}
			else {
				declinedIdx=5;
				otherIdx=9;
			}
			//The first selected is 'None', so unselect it.
			if(comboBoxMultiRace.ListSelectedIndices[0]==0) {
				comboBoxMultiRace.SetSelected(0,false);
				if(comboBoxMultiRace.ListSelectedIndices.Count<2) {
					return;
				}
			}
			//The first selected is 'DeclinedToSpecify', so unselect it.
			if(comboBoxMultiRace.ListSelectedIndices[0]==declinedIdx) {
				comboBoxMultiRace.SetSelected(declinedIdx,false);
				if(comboBoxMultiRace.ListSelectedIndices.Count<2) {
					return;
				}
			}
			//The first selected is 'Other', so unselect it.
			if(comboBoxMultiRace.ListSelectedIndices[0]==otherIdx) {
				comboBoxMultiRace.SetSelected(otherIdx,false);
				if(comboBoxMultiRace.ListSelectedIndices.Count<2) {
					return;
				}
			}
			//'None' is either the last one selected or in the middle of the items selected, so unselect all but 'None'.
			if(comboBoxMultiRace.ListSelectedIndices.Contains(0)) {
				comboBoxMultiRace.SelectedIndices.Clear();
				comboBoxMultiRace.SetSelected(0,true);
				return;
			}
			//'DeclinedToSpecify' is either the last one selected or in the middle of the items selected, so unselect all but 'DeclinedToSpecify'.
			if(comboBoxMultiRace.ListSelectedIndices.Contains(declinedIdx)) {
				comboBoxMultiRace.SelectedIndices.Clear();
				comboBoxMultiRace.SetSelected(declinedIdx,true);
				return;
			}
			//'Other' is either the last one selected or in the middle of the items selected, so unselect all but 'Other'.
			if(comboBoxMultiRace.ListSelectedIndices.Contains(otherIdx)) {
				comboBoxMultiRace.SelectedIndices.Clear();
				comboBoxMultiRace.SetSelected(otherIdx,true);
				return;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				return;
			}
			//Guaranteed to be at least 2 selected indices if we get here
			int hispanicIdx=7;
			int nonHispanicIdx=11;
			//The last one selected is 'Hispanic' and 'NotHispanic' is also selected.
			if(comboBoxMultiRace.ListSelectedIndices[comboBoxMultiRace.ListSelectedIndices.Count-1]==hispanicIdx
				&& comboBoxMultiRace.ListSelectedIndices.Contains(nonHispanicIdx)) 
			{
				comboBoxMultiRace.SetSelected(nonHispanicIdx,false);
			}
			//The last one selected is 'NotHispanic' and 'Hispanic' is also selected.
			else if(comboBoxMultiRace.ListSelectedIndices[comboBoxMultiRace.ListSelectedIndices.Count-1]==nonHispanicIdx
				&& comboBoxMultiRace.ListSelectedIndices.Contains(hispanicIdx)) 
			{
				comboBoxMultiRace.SetSelected(hispanicIdx,false);
			}
		}	

		///<summary>Gets an employerNum based on the name entered. Called from FillCur</summary>
		private void GetEmployerNum(){
			if(PatCur.EmployerNum==0){//no employer was previously entered.
				if(textEmployer.Text==""){
					//no change
				}
				else{
					PatCur.EmployerNum=Employers.GetEmployerNum(textEmployer.Text);
				}
			}
			else{//an employer was previously entered
				if(textEmployer.Text==""){
					PatCur.EmployerNum=0;
				}
				//if text has changed
				else if(Employers.GetName(PatCur.EmployerNum)!=textEmployer.Text){
					PatCur.EmployerNum=Employers.GetEmployerNum(textEmployer.Text);
				}
			}
		}

		private void butReferredFrom_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.RefAttachAdd)) {
				return;
			}
			Referral refCur=new Referral();
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Is the referral source an existing patient?")) {
				FormPatientSelect FormPS=new FormPatientSelect();
				FormPS.SelectionModeOnly=true;
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK) {
					return;
				}
				refCur.PatNum=FormPS.SelectedPatNum;
				bool referralIsNew=true;
				if(Referrals.List.Any(x => x.PatNum==FormPS.SelectedPatNum)) {
					refCur=Referrals.List.FirstOrDefault(x => x.PatNum==FormPS.SelectedPatNum);
					referralIsNew=false;
				}
				FormReferralEdit FormRefEdit=new FormReferralEdit(refCur);//the ReferralNum must be added here
				FormRefEdit.IsNew=referralIsNew;
				FormRefEdit.ShowDialog();
				if(FormRefEdit.DialogResult!=DialogResult.OK) {
					return;
				}
				refCur=FormRefEdit.RefCur;//not needed, but it makes it clear that we are editing the ref in FormRefEdit
			}
			else {//not a patient referral, must be a doctor or marketing/other so show the referral select window with doctor and other check boxes checked
				FormReferralSelect FormRS=new FormReferralSelect();
				FormRS.IsSelectionMode=true;
				FormRS.IsShowPat=false;
				FormRS.ShowDialog();
				if(FormRS.DialogResult!=DialogResult.OK) {
					FillReferrals();//the user may have edited a referral and then cancelled attaching to the patient, refill the text box to reflect any changes
					return;
				}
				refCur=FormRS.SelectedReferral;
			}
			RefAttach refattach=new RefAttach();
			refattach.ReferralNum=refCur.ReferralNum;
			refattach.PatNum=PatCur.PatNum;
			refattach.IsFrom=true;
			refattach.RefDate=DateTimeOD.Today;
			if(refCur.IsDoctor) {//whether using ehr or not
				refattach.IsTransitionOfCare=true;
			}
			refattach.ItemOrder=_listRefAttaches.Select(x => x.ItemOrder).DefaultIfEmpty().Max()+1;
			RefAttaches.Insert(refattach);
			SecurityLogs.MakeLogEntry(Permissions.RefAttachAdd,PatCur.PatNum,"Referred From "+Referrals.GetNameFL(refattach.ReferralNum));
			FillReferrals();
		}

		///<summary>Fills the Referred From text box with the oldest (lowest ItemOrder) referral source marked IsFrom.</summary>
		private void FillReferrals() {
			textReferredFrom.Clear();
			_listRefAttaches=RefAttaches.Refresh(PatCur.PatNum);
			string firstRefNameTypeAbbr="";
			string firstRefType="";
			string firstRefFullName="";
			RefAttach refAttach=_listRefAttaches.FirstOrDefault(x => x.IsFrom);
			if(refAttach==null) {
				return;
			}
			Referral refCur=Referrals.GetReferral(refAttach.ReferralNum);
			if(refCur==null) {
				return;
			}
			firstRefFullName=Referrals.GetNameLF(refCur.ReferralNum);
			if(refCur.PatNum>0) {
				firstRefType=" (patient)";
			}
			else if(refCur.IsDoctor) {
				firstRefType=" (doctor)";
			}
			string suffix="";
			if(_listRefAttaches.FindAll(x => x.IsFrom).Count>1) {
				suffix=" (+"+(_listRefAttaches.FindAll(x => x.IsFrom).Count-1)+" more)";
			}
			firstRefNameTypeAbbr=firstRefFullName;
			for(int i=1;i<firstRefFullName.Length+1;i++) {//i is used as the length to substring, not an index, so i<firstRefName.Length+1 is safe
				if(TextRenderer.MeasureText(firstRefFullName.Substring(0,i)+firstRefType+suffix,textReferredFrom.Font).Width<textReferredFrom.Width)	{
					continue;
				}
				firstRefNameTypeAbbr=firstRefFullName.Substring(0,i-1);
				break;
			}
			firstRefNameTypeAbbr+=firstRefType+suffix;//both firstRefType and suffix could be blank, but they will show regardless of the length of firstRefName
			textReferredFrom.Text=firstRefNameTypeAbbr;
			_referredFromToolTip.SetToolTip(textReferredFrom,firstRefFullName+firstRefType+suffix);
			//Example: Schmidt, John Jacob Jingleheimer, DDS (doctor) (+5 more) 
			//might be shortened to : Schmidt, John Jaco (doctor) (+5 more) 
			_errorProv.SetError(textReferredFrom,"");
		}

		private void textReferredFrom_DoubleClick(object sender,EventArgs e) {
			FormReferralsPatient FormRE=new FormReferralsPatient();
			FormRE.PatNum=PatCur.PatNum;
			FormRE.ShowDialog();
			FillReferrals();
			SetRequiredFields();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			bool isValid=true;
			object[] parameters=new object[] { isValid,PatCur };
			Plugins.HookAddCode(this,"FormPatientEdit.butOK_Click_beginning",parameters);
			if((bool)parameters[0]==false) {//Didn't pass plug-in validation
				return;
			}
			bool CDSinterventionCheckRequired=false;//checks selected values
			if(  textBirthdate.errorProvider1.GetError(textBirthdate)!=""
				|| textDateFirstVisit.errorProvider1.GetError(textDateFirstVisit)!=""
				|| textAdmitDate.errorProvider1.GetError(textAdmitDate)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime dateTimeDeceased=DateTime.MinValue;
			try {
				if(textDateDeceased.Text!="") {
					dateTimeDeceased=DateTime.Parse(textDateDeceased.Text);
				}
			}
			catch {
				MsgBox.Show(this,"Date time deceased is invalid.");
				return;
			}
			if(textLName.Text==""){
				MsgBox.Show(this,"Last Name must be entered.");
				return;
			}
			//see if chartNum is a duplicate
			if(textChartNumber.Text!=""){
				//the patNum will be 0 for new
				string usedBy=Patients.ChartNumUsedBy(textChartNumber.Text,PatCur.PatNum);
				if(usedBy!=""){
					MessageBox.Show(Lan.g(this,"This chart number is already in use by:")+" "+usedBy);
					return;
				}
			}
			try{
				PIn.Int(textAskToArriveEarly.Text);
			}
			catch{
				MsgBox.Show(this,"Ask To Arrive Early invalid.");
				return;
			}
			if(textCounty.Text != "" && !Counties.DoesExist(textCounty.Text)){
				MessageBox.Show(Lan.g(this,"County name invalid."));
				return;
			}
			if(textSite.Text=="") {
				PatCur.SiteNum=0;
			}
			if(textSite.Text != "" && textSite.Text != Sites.GetDescription(PatCur.SiteNum)) {
				long matchingSite=Sites.FindMatchSiteNum(textSite.Text);
				if(matchingSite==-1) {
					MessageBox.Show(Lan.g(this,"Invalid Site description."));
					return;
				}
				else {
					PatCur.SiteNum=matchingSite;
				}
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(comboCanadianEligibilityCode.SelectedIndex==1//FT student
					&& textSchool.Text=="" && PIn.Date(textBirthdate.Text).AddYears(18)<=DateTime.Today)
				{
					MsgBox.Show(this,"School should be entered if full-time student and patient is 18 or older.");
					return;
				}
			}
			//If HQ and this is a patient in a reseller family, do not allow the changing of the patient status.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) 
				&& Resellers.IsResellerFamily(PatCur.Guarantor)
				&& PatOld.PatStatus!=(PatientStatus)listStatus.SelectedIndex) 
			{
				MsgBox.Show(this,"Cannot change the status of a patient in a reseller family.");
				return;
			}
			int selectedProvIndex=comboPriProv.SelectedIndex;
			if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				selectedProvIndex--;//Minus 1 to account for 'Select Provider'
			}
			if(selectedProvIndex<0) {
				if(PrefC.GetBool(PrefName.PriProvDefaultToSelectProv) || PatCur.PriProv==0) {//selected index could be -1 if the provider was selected and then hidden
					MsgBox.Show(this,"Primary provider must be set.");
					_isValidating=true;
					SetRequiredFields();
					return;
				}
			}
			else {
				PatCur.PriProv=ProviderC.ListShort[selectedProvIndex].ProvNum;
			}
			if(_isMissingRequiredFields) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Required fields are missing or incorrect.  Click OK to save anyway or Cancel to return and "
						+"finish editing patient information.")) {
					_isValidating=true;
					SetRequiredFields();
					return;
				}
				SecurityLogs.MakeLogEntry(Permissions.RequiredFields,PatCur.PatNum,"Saved patient with required fields missing.");
			}
			PatCur.LName=textLName.Text;
			PatCur.FName=textFName.Text;
			PatCur.MiddleI=textMiddleI.Text;
			PatCur.Preferred=textPreferred.Text;
			PatCur.Title=textTitle.Text;
			PatCur.Salutation=textSalutation.Text;
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {//Mother's maiden name UI is only used when EHR is enabled.
				_ehrPatientCur.MotherMaidenFname=textMotherMaidenFname.Text;
				_ehrPatientCur.MotherMaidenLname=textMotherMaidenLname.Text;
			}
			switch(listStatus.SelectedIndex){
				case 0: PatCur.PatStatus=PatientStatus.Patient; break;
				case 1: PatCur.PatStatus=PatientStatus.NonPatient; break;
				case 2: PatCur.PatStatus=PatientStatus.Inactive; break;
				case 3: PatCur.PatStatus=PatientStatus.Archived; break;
				case 4: PatCur.PatStatus=PatientStatus.Deceased; break;
				case 5: PatCur.PatStatus=PatientStatus.Prospective; break;
			}
			switch(listGender.SelectedIndex){
				case 0: PatCur.Gender=PatientGender.Male; break;
				case 1: PatCur.Gender=PatientGender.Female; break;
				case 2: PatCur.Gender=PatientGender.Unknown; break;
			}
			switch(listPosition.SelectedIndex){
				case 0: PatCur.Position=PatientPosition.Single; break;
				case 1: PatCur.Position=PatientPosition.Married; break;
				case 2: PatCur.Position=PatientPosition.Child; break;
				case 3: PatCur.Position=PatientPosition.Widowed; break;
				case 4: PatCur.Position=PatientPosition.Divorced; break;
			}
			PatCur.Birthdate=PIn.Date(textBirthdate.Text);
			PatCur.DateTimeDeceased=dateTimeDeceased;
			if(CultureInfo.CurrentCulture.Name=="en-US"){
				if(Regex.IsMatch(textSSN.Text,@"^\d\d\d-\d\d-\d\d\d\d$")){
					PatCur.SSN=textSSN.Text.Substring(0,3)+textSSN.Text.Substring(4,2)
						+textSSN.Text.Substring(7,4);
				}
				else{
					PatCur.SSN=textSSN.Text;
				}
			}
			else{//other cultures
				PatCur.SSN=textSSN.Text;
			}
			if(IsNew) {//Check if patient already exists.
				List<Patient> patList=Patients.GetListByName(PatCur.LName,PatCur.FName,PatCur.PatNum);
				for(int i=0;i<patList.Count;i++) {
					//If dates match or aren't entered there might be a duplicate patient.
					if(patList[i].Birthdate==PatCur.Birthdate
					|| patList[i].Birthdate.Year<1880
					|| PatCur.Birthdate.Year<1880) {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This patient might already exist.  Continue anyway?")) {
							return;
						}
						break;
					}
				}
			}
			PatCur.MedicaidID=textMedicaidID.Text;
			_ehrPatientCur.MedicaidState=textMedicaidState.Text;
			EhrPatients.Update(_ehrPatientCur);
			PatCur.WkPhone=textWkPhone.Text;
			PatCur.WirelessPhone=textWirelessPhone.Text;
			PatCur.TxtMsgOk=(YN)listTextOk.SelectedIndex;
			PatCur.Email=textEmail.Text;
			//PatCur.RecallInterval=PIn.PInt(textRecall.Text);
			PatCur.ChartNumber=textChartNumber.Text;
			PatCur.SchoolName=textSchool.Text;
			//address:
			PatCur.HmPhone=textHmPhone.Text;
			PatCur.Address=textAddress.Text;
			PatCur.Address2=textAddress2.Text;
			PatCur.City=textCity.Text;
			PatCur.State=textState.Text;
			PatCur.Country=textCountry.Text;
			PatCur.Zip=textZip.Text;
			PatCur.CreditType=textCreditType.Text;
			PatCur.HasSuperBilling=checkSuperBilling.Checked;
			GetEmployerNum();
			//PatCur.EmploymentNote=textEmploymentNote.Text;
			if(comboLanguage.SelectedIndex==0){
				PatCur.Language="";
			}
			else{
				PatCur.Language=languageList[comboLanguage.SelectedIndex-1];
			}
			PatCur.AddrNote=textAddrNotes.Text;
			PatCur.DateFirstVisit=PIn.Date(textDateFirstVisit.Text);
			PatCur.AskToArriveEarly=PIn.Int(textAskToArriveEarly.Text);
			if(comboSecProv.SelectedIndex==0){
				PatCur.SecProv=0;
			}
			else{
				PatCur.SecProv=ProviderC.ListShort[comboSecProv.SelectedIndex-1].ProvNum;
			}
			if(comboFeeSched.SelectedIndex==0){
				PatCur.FeeSched=0;
			}
			else{
				PatCur.FeeSched=FeeSchedC.ListShort[comboFeeSched.SelectedIndex-1].FeeSchedNum;
			}
			if(comboBillType.SelectedIndex!=-1){
				PatCur.BillingType=DefC.Short[(int)DefCat.BillingTypes][comboBillType.SelectedIndex].DefNum;
			}
			if(comboClinic.SelectedIndex==0) {
				PatCur.ClinicNum=0;
			}
			else {
				PatCur.ClinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			List<PatRace> listPatRaces=new List<PatRace>();
			for(int i=0;i<comboBoxMultiRace.SelectedIndices.Count;i++) {
				int selectedIdx=(int)comboBoxMultiRace.SelectedIndices[i];
				if(selectedIdx==0) {//If the none option was chosen, then ensure that no other race information is saved.
					listPatRaces.Clear();
					break;
				}
				if(!PrefC.GetBool(PrefName.ShowFeatureEhr)) {//If not using EHR, then the comboBoxMultiRace item locations are the same as the PatRace enum locations plus 1.
					listPatRaces.Add((PatRace)(selectedIdx-1));
					continue;
				}
				//EHR
				if(selectedIdx==1) {
					listPatRaces.Add(PatRace.AfricanAmerican);
				}
				else if(selectedIdx==2) {
					listPatRaces.Add(PatRace.AmericanIndian);
				}
				else if(selectedIdx==3) {
					listPatRaces.Add(PatRace.Asian);
				}
				else if(selectedIdx==4) {
					listPatRaces.Add(PatRace.DeclinedToSpecifyRace);
				}
				else if(selectedIdx==5) {
					listPatRaces.Add(PatRace.HawaiiOrPacIsland);
				}
				else if(selectedIdx==6) {
					listPatRaces.Add(PatRace.Other);
				}
				else if(selectedIdx==7) {
					listPatRaces.Add(PatRace.White);
				}
			}
			if(listPatRaces.Contains(PatRace.DeclinedToSpecifyRace)) {//If DeclinedToSpecify was chosen, then ensure that no other races are saved.
				listPatRaces.Clear();
				listPatRaces.Add(PatRace.DeclinedToSpecifyRace);
			}
			else if(listPatRaces.Contains(PatRace.Other)) {//If Other was chosen, then ensure that no other races are saved.
				listPatRaces.Clear();
				listPatRaces.Add(PatRace.Other);
			}
			//In order to pass EHR G2 MU testing you must be able to have an ethnicity without a race, or a race without an ethnicity.  This will mean that patients will not count towards
			//  meaningfull use demographic calculations.  If we have time in the future we should probably alert EHR users when a race is chosen but no ethnicity, or a ethnicity but no race.
			if(comboEthnicity.SelectedIndex==1) {
				listPatRaces.Add(PatRace.DeclinedToSpecifyEthnicity);
			}
			else if(comboEthnicity.SelectedIndex==2){
				listPatRaces.Add(PatRace.NotHispanic);
			}
			else if(comboEthnicity.SelectedIndex==3) {
				listPatRaces.Add(PatRace.Hispanic);
			}
			PatientRaces.Reconcile(PatCur.PatNum,listPatRaces);//Insert, Update, Delete if needed.
			PatCur.County=textCounty.Text;
			//site set when user picks from list.
			PatCur.GradeLevel=(PatientGrade)comboGradeLevel.SelectedIndex;
			PatCur.Urgency=(TreatmentUrgency)comboUrgency.SelectedIndex;
			//ResponsParty handled when buttons are pushed.
			if(Programs.IsEnabled(ProgramName.TrophyEnhanced)) {
				PatCur.TrophyFolder=textTrophyFolder.Text;
			}
			PatCur.Ward=textWard.Text;
			PatCur.PreferContactMethod=(ContactMethod)comboContact.SelectedIndex;
			PatCur.PreferConfirmMethod=(ContactMethod)comboConfirm.SelectedIndex;
			PatCur.PreferRecallMethod=(ContactMethod)comboRecall.SelectedIndex;
			PatCur.AdmitDate=PIn.Date(textAdmitDate.Text);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				PatCur.CanadianEligibilityCode=(byte)comboCanadianEligibilityCode.SelectedIndex;
			}
			if(PatCur.Guarantor==0){
				PatCur.Guarantor=PatCur.PatNum;
			}
			Patients.Update(PatCur,PatOld);
			if(PatCur.Birthdate!=PatOld.Birthdate || PatCur.Gender!=PatOld.Gender) {
				CDSinterventionCheckRequired=true;
			}
			if(CDSinterventionCheckRequired && CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).LabTestCDS) {
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(PatCur,PatCur);//both should be patCur.
				FormCDSI.ShowIfRequired(false);
			}
			if(checkArriveEarlySame.Checked){
				Patients.UpdateArriveEarlyForFam(PatCur);
			}
			if(checkSame.Checked){
				//might want to include a mechanism for comparing fields to be overwritten
				Patients.UpdateAddressForFam(PatCur);
			}
			if(checkBillProvSame.Checked) {
				Patients.UpdateBillingProviderForFam(PatCur);
			}
			if(checkNotesSame.Checked){
				Patients.UpdateNotesForFam(PatCur);
			}
			if(checkEmailPhoneSame.Checked) {
				Patients.UpdateEmailPhoneForFam(PatCur);
			}
			//If this patient is also a referral source,
			//keep address info synched:
			for(int i=0;i<Referrals.List.Length;i++){
				if(Referrals.List[i].PatNum==PatCur.PatNum){
					//Referrals.Cur=Referrals.List[i];
					Referrals.List[i].LName=PatCur.LName;
					Referrals.List[i].FName=PatCur.FName;
					Referrals.List[i].MName=PatCur.MiddleI;
					Referrals.List[i].Address=PatCur.Address;
					Referrals.List[i].Address2=PatCur.Address2;
					Referrals.List[i].City=PatCur.City;
					Referrals.List[i].ST=PatCur.State;
					Referrals.List[i].SSN=PatCur.SSN;
					Referrals.List[i].Zip=PatCur.Zip;
					Referrals.List[i].Telephone=TelephoneNumbers.FormatNumbersExactTen(PatCur.HmPhone);
					Referrals.List[i].EMail=PatCur.Email;
					Referrals.Update(Referrals.List[i]);
					Referrals.RefreshCache();
					break;
				}
			}
			//if patient is inactive, then disable any recalls
			if(PatCur.PatStatus==PatientStatus.Archived
				|| PatCur.PatStatus==PatientStatus.Deceased
				|| PatCur.PatStatus==PatientStatus.Inactive
				|| PatCur.PatStatus==PatientStatus.NonPatient
				|| PatCur.PatStatus==PatientStatus.Prospective)
			{
				List<Recall> recalls=Recalls.GetList(PatCur.PatNum);
				for(int i=0;i<recalls.Count;i++){
					recalls[i].IsDisabled=true;
					recalls[i].DateDue=DateTime.MinValue;
					Recalls.Update(recalls[i]);
				}
			}
			//if patient was re-activated, then re-enable any recalls
			else if(PatCur.PatStatus!=PatOld.PatStatus && PatCur.PatStatus==PatientStatus.Patient) {//if changed patstatus, and new status is Patient
				List<Recall> recalls=Recalls.GetList(PatCur.PatNum);
				for(int i=0;i<recalls.Count;i++) {
					if(recalls[i].IsDisabled) {
						recalls[i].IsDisabled=false;
						Recalls.Update(recalls[i]);
					}
				}
				Recalls.Synch(PatCur.PatNum);
			}
			//if pref for Patient Clone is enabled, check to see if this patient is part of a clone pair, either the clone or original
			//if part of a clone pair and the name or birthdate has been changed, synch the clone if PatCur is the original
			//warn the user if PatCur is the clone that the linkage will be broken and to fix the value changed on the clone patient to re-link
			//we do not want to synch from clone to original, so they have to fix manually
			if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)
				&& (PatCur.LName!=PatOld.LName || PatCur.FName!=PatOld.FName || PatCur.Birthdate!=PatOld.Birthdate)) 
			{
				Patient patClone;
				Patient patNonClone;
				List<Patient> listAmbiguousMatches;
				Patients.GetCloneAndNonClone(PatOld,out patClone,out patNonClone,out listAmbiguousMatches);
				if(patClone!=null) {
					if(PatOld.PatNum==patClone.PatNum) {//PatOld is the clone, message box warning the user that changing the name and/or birthdate will now break the clone linkage
						MessageBox.Show(Lan.g(this,"Changing the name of this patient clone will unlink it from the following patient")
							+":\r\n"+patNonClone.PatNum+" - "+Patients.GetNameFL(patNonClone.LName,patNonClone.FName,patNonClone.Preferred,patNonClone.MiddleI)+".\r\n"
							+Lan.g(this,"To re-link, the birthdates must be matching valid dates and the first and last names must be identical with the clone name all uppercase."));
					}
					else {//PatOld must be the non-clone, synch the name and birthdate changes to the clone
						Patient patCloneOld=patClone.Copy();
						string strChngTo=" "+Lan.g(this,"changed to")+" ";
						string changeMsg=Lan.g(this,"The following changes were synched to the clone of this patient,\r\n")
							+patClone.PatNum+" - "+Patients.GetNameFL(patClone.LName,patClone.FName,patClone.Preferred,patClone.MiddleI)+":";
						if(PatCur.LName!=PatOld.LName) {
							patClone.LName=PatCur.LName.ToUpper();
							changeMsg+="\r\n"+patCloneOld.LName+strChngTo+patClone.LName;
						}
						if(PatCur.FName!=PatOld.FName) {
							patClone.FName=PatCur.FName.ToUpper();
							changeMsg+="\r\n"+patCloneOld.FName+strChngTo+patClone.FName;
						}
						if(PatCur.Birthdate!=PatOld.Birthdate) {
							patClone.Birthdate=PatCur.Birthdate;
							changeMsg+="\r\n"+patCloneOld.Birthdate.ToShortDateString()+strChngTo+patClone.Birthdate.ToShortDateString();
						}
						Patients.Update(patClone,patCloneOld);
						MessageBox.Show(changeMsg);
					}
				}
			}
			//If there is an existing HL7 def enabled, send an ADT message if there is an outbound ADT message defined
			if(HL7Defs.IsExistingHL7Enabled()) {
				//new patients get the A04 ADT, updating existing patients we send an A08
				MessageHL7 messageHL7=null;
				if(IsNew) {
					messageHL7=MessageConstructor.GenerateADT(PatCur,Patients.GetPat(PatCur.Guarantor),EventTypeHL7.A04);
				}
				else {
					messageHL7=MessageConstructor.GenerateADT(PatCur,Patients.GetPat(PatCur.Guarantor),EventTypeHL7.A08);
				}
				//Will be null if there is no outbound ADT message defined, so do nothing
				if(messageHL7!=null) {
					HL7Msg hl7Msg=new HL7Msg();
					hl7Msg.AptNum=0;
					hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
					hl7Msg.MsgText=messageHL7.ToString();
					hl7Msg.PatNum=PatCur.PatNum;
					HL7Msgs.Insert(hl7Msg);
#if DEBUG
					MessageBox.Show(this,messageHL7.ToString());
#endif
				}
			}
			Plugins.HookAddCode(this,"FormPatientEdit.butOK_Click_end",PatCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPatientEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;
			}
			if(IsNew){
				//for(int i=0;i<RefList.Length;i++){
				//	RefAttaches.Delete(RefList[i]);
				//}
				Patients.Delete(PatCur);
			}
			//revert any changes to the guardian list for all family members
			Guardians.Sync(_listGuardiansForFamOld,FamCur.ListPats.Select(x => x.PatNum).ToList());
		}


		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

	}
}









