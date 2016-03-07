using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormMedical : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butAdd;
		private IContainer components;
		private OpenDental.UI.Button butAddDisease;// Required designer variable.
		private Patient PatCur;
		private OpenDental.UI.ODGrid gridMeds;
		private OpenDental.UI.ODGrid gridDiseases;
		private List<Disease> DiseaseList;
		private CheckBox checkDiscontinued;
		private ODGrid gridAllergies;
		private UI.Button butAddAllergy;
		private PatientNote PatientNoteCur;
		private CheckBox checkShowInactiveAllergies;
		private List<Allergy> allergyList;
		private UI.Button butPrint;
		private List<MedicationPat> medList;
		private int pagesPrinted;
		private PrintDocument pd;
		private bool headingPrinted;
		private CheckBox checkShowInactiveProblems;
		private ImageList imageListInfoButton;
		private ODGrid gridFamilyHealth;
		private UI.Button butAddFamilyHistory;
		private List<FamilyHealth> ListFamHealth;
		private int headingPrintH;
		private long _EhrMeasureEventNum;
		private TabControl tabControlFormMedical;
		private TabPage tabProblems;
		private TabPage tabMedications;
		private TabPage tabAllergies;
		private TabPage tabFamHealthHist;
		private TabPage tabMedical;
		private Label label4;
		private Label label2;
		private Label label3;
		private Label label1;
		private ODtextBox textMedical;
		private UI.Button butPrintMedical;
		private ODtextBox textService;
		private ODtextBox textMedUrgNote;
		private CheckBox checkPremed;
		private GroupBox groupMedsDocumented;
		private RadioButton radioMedsDocumentedNo;
		private RadioButton radioMedsDocumentedYes;
		private Label label6;
		private ODtextBox textMedicalComp;
		private TabPage tabVitalSigns;
		private UI.Button butAddVitalSign;
		private ODGrid gridVitalSigns;
		private UI.Button butGrowthChart;
		private long _EhrNotPerfNum;
		private List<Vitalsign> _listVitalSigns;


		///<summary></summary>
		public FormMedical(PatientNote patientNoteCur,Patient patCur){
			InitializeComponent();// Required for Windows Form Designer support
			PatCur=patCur;
			PatientNoteCur=patientNoteCur;
			Lan.F(this);
		}

		///<summary></summary>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedical));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butAddDisease = new OpenDental.UI.Button();
			this.gridMeds = new OpenDental.UI.ODGrid();
			this.gridDiseases = new OpenDental.UI.ODGrid();
			this.checkDiscontinued = new System.Windows.Forms.CheckBox();
			this.gridAllergies = new OpenDental.UI.ODGrid();
			this.butAddAllergy = new OpenDental.UI.Button();
			this.checkShowInactiveAllergies = new System.Windows.Forms.CheckBox();
			this.butPrint = new OpenDental.UI.Button();
			this.checkShowInactiveProblems = new System.Windows.Forms.CheckBox();
			this.imageListInfoButton = new System.Windows.Forms.ImageList(this.components);
			this.gridFamilyHealth = new OpenDental.UI.ODGrid();
			this.butAddFamilyHistory = new OpenDental.UI.Button();
			this.tabControlFormMedical = new System.Windows.Forms.TabControl();
			this.tabMedical = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textMedical = new OpenDental.ODtextBox();
			this.butPrintMedical = new OpenDental.UI.Button();
			this.textService = new OpenDental.ODtextBox();
			this.textMedUrgNote = new OpenDental.ODtextBox();
			this.checkPremed = new System.Windows.Forms.CheckBox();
			this.groupMedsDocumented = new System.Windows.Forms.GroupBox();
			this.radioMedsDocumentedNo = new System.Windows.Forms.RadioButton();
			this.radioMedsDocumentedYes = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.textMedicalComp = new OpenDental.ODtextBox();
			this.tabProblems = new System.Windows.Forms.TabPage();
			this.tabMedications = new System.Windows.Forms.TabPage();
			this.tabAllergies = new System.Windows.Forms.TabPage();
			this.tabFamHealthHist = new System.Windows.Forms.TabPage();
			this.tabVitalSigns = new System.Windows.Forms.TabPage();
			this.butGrowthChart = new OpenDental.UI.Button();
			this.butAddVitalSign = new OpenDental.UI.Button();
			this.gridVitalSigns = new OpenDental.UI.ODGrid();
			this.tabControlFormMedical.SuspendLayout();
			this.tabMedical.SuspendLayout();
			this.groupMedsDocumented.SuspendLayout();
			this.tabProblems.SuspendLayout();
			this.tabMedications.SuspendLayout();
			this.tabAllergies.SuspendLayout();
			this.tabFamHealthHist.SuspendLayout();
			this.tabVitalSigns.SuspendLayout();
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
			this.butOK.Location = new System.Drawing.Point(610, 417);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
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
			this.butCancel.Location = new System.Drawing.Point(703, 417);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(6, 6);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(123, 23);
			this.butAdd.TabIndex = 51;
			this.butAdd.Text = "&Add Medication";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butAddDisease
			// 
			this.butAddDisease.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddDisease.Autosize = true;
			this.butAddDisease.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddDisease.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddDisease.CornerRadius = 4F;
			this.butAddDisease.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddDisease.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddDisease.Location = new System.Drawing.Point(6, 6);
			this.butAddDisease.Name = "butAddDisease";
			this.butAddDisease.Size = new System.Drawing.Size(98, 23);
			this.butAddDisease.TabIndex = 58;
			this.butAddDisease.Text = "Add Problem";
			this.butAddDisease.Click += new System.EventHandler(this.butAddProblem_Click);
			// 
			// gridMeds
			// 
			this.gridMeds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMeds.HasAddButton = false;
			this.gridMeds.HasMultilineHeaders = false;
			this.gridMeds.HScrollVisible = false;
			this.gridMeds.Location = new System.Drawing.Point(6, 35);
			this.gridMeds.Name = "gridMeds";
			this.gridMeds.ScrollValue = 0;
			this.gridMeds.Size = new System.Drawing.Size(757, 342);
			this.gridMeds.TabIndex = 59;
			this.gridMeds.Title = "Medications";
			this.gridMeds.TranslationName = "TableMedications";
			this.gridMeds.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMeds_CellDoubleClick);
			this.gridMeds.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMeds_CellClick);
			// 
			// gridDiseases
			// 
			this.gridDiseases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridDiseases.HasAddButton = false;
			this.gridDiseases.HasMultilineHeaders = false;
			this.gridDiseases.HScrollVisible = false;
			this.gridDiseases.Location = new System.Drawing.Point(6, 35);
			this.gridDiseases.Name = "gridDiseases";
			this.gridDiseases.ScrollValue = 0;
			this.gridDiseases.Size = new System.Drawing.Size(759, 342);
			this.gridDiseases.TabIndex = 60;
			this.gridDiseases.Title = "Problems";
			this.gridDiseases.TranslationName = "TableDiseases";
			this.gridDiseases.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDiseases_CellDoubleClick);
			this.gridDiseases.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDiseases_CellClick);
			// 
			// checkDiscontinued
			// 
			this.checkDiscontinued.Location = new System.Drawing.Point(155, 11);
			this.checkDiscontinued.Name = "checkDiscontinued";
			this.checkDiscontinued.Size = new System.Drawing.Size(201, 18);
			this.checkDiscontinued.TabIndex = 61;
			this.checkDiscontinued.Tag = "";
			this.checkDiscontinued.Text = "Show Discontinued Medications";
			this.checkDiscontinued.UseVisualStyleBackColor = true;
			this.checkDiscontinued.KeyUp += new System.Windows.Forms.KeyEventHandler(this.checkDiscontinued_KeyUp);
			this.checkDiscontinued.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkShowDiscontinuedMeds_MouseUp);
			// 
			// gridAllergies
			// 
			this.gridAllergies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAllergies.HasAddButton = false;
			this.gridAllergies.HasMultilineHeaders = false;
			this.gridAllergies.HScrollVisible = false;
			this.gridAllergies.Location = new System.Drawing.Point(6, 35);
			this.gridAllergies.Name = "gridAllergies";
			this.gridAllergies.ScrollValue = 0;
			this.gridAllergies.Size = new System.Drawing.Size(757, 342);
			this.gridAllergies.TabIndex = 63;
			this.gridAllergies.Title = "Allergies";
			this.gridAllergies.TranslationName = "TableDiseases";
			this.gridAllergies.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllergies_CellDoubleClick);
			this.gridAllergies.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllergies_CellClick);
			// 
			// butAddAllergy
			// 
			this.butAddAllergy.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddAllergy.Autosize = true;
			this.butAddAllergy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAllergy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAllergy.CornerRadius = 4F;
			this.butAddAllergy.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAllergy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAllergy.Location = new System.Drawing.Point(6, 6);
			this.butAddAllergy.Name = "butAddAllergy";
			this.butAddAllergy.Size = new System.Drawing.Size(98, 23);
			this.butAddAllergy.TabIndex = 64;
			this.butAddAllergy.Text = "Add Allergy";
			this.butAddAllergy.Click += new System.EventHandler(this.butAddAllergy_Click);
			// 
			// checkShowInactiveAllergies
			// 
			this.checkShowInactiveAllergies.Location = new System.Drawing.Point(155, 11);
			this.checkShowInactiveAllergies.Name = "checkShowInactiveAllergies";
			this.checkShowInactiveAllergies.Size = new System.Drawing.Size(201, 18);
			this.checkShowInactiveAllergies.TabIndex = 65;
			this.checkShowInactiveAllergies.Tag = "";
			this.checkShowInactiveAllergies.Text = "Show Inactive Allergies";
			this.checkShowInactiveAllergies.UseVisualStyleBackColor = true;
			this.checkShowInactiveAllergies.CheckedChanged += new System.EventHandler(this.checkShowInactiveAllergies_CheckedChanged);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(475, 6);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(116, 24);
			this.butPrint.TabIndex = 66;
			this.butPrint.Text = "Print Medications";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkShowInactiveProblems
			// 
			this.checkShowInactiveProblems.Location = new System.Drawing.Point(155, 11);
			this.checkShowInactiveProblems.Name = "checkShowInactiveProblems";
			this.checkShowInactiveProblems.Size = new System.Drawing.Size(201, 18);
			this.checkShowInactiveProblems.TabIndex = 65;
			this.checkShowInactiveProblems.Tag = "";
			this.checkShowInactiveProblems.Text = "Show Inactive Problems";
			this.checkShowInactiveProblems.UseVisualStyleBackColor = true;
			this.checkShowInactiveProblems.CheckedChanged += new System.EventHandler(this.checkShowInactiveProblems_CheckedChanged);
			// 
			// imageListInfoButton
			// 
			this.imageListInfoButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListInfoButton.ImageStream")));
			this.imageListInfoButton.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListInfoButton.Images.SetKeyName(0, "iButton_16px.png");
			// 
			// gridFamilyHealth
			// 
			this.gridFamilyHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridFamilyHealth.HasAddButton = false;
			this.gridFamilyHealth.HasMultilineHeaders = false;
			this.gridFamilyHealth.HScrollVisible = false;
			this.gridFamilyHealth.Location = new System.Drawing.Point(6, 35);
			this.gridFamilyHealth.Name = "gridFamilyHealth";
			this.gridFamilyHealth.ScrollValue = 0;
			this.gridFamilyHealth.Size = new System.Drawing.Size(757, 342);
			this.gridFamilyHealth.TabIndex = 69;
			this.gridFamilyHealth.Title = "Family Health History";
			this.gridFamilyHealth.TranslationName = "TableDiseases";
			this.gridFamilyHealth.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamilyHealth_CellDoubleClick);
			// 
			// butAddFamilyHistory
			// 
			this.butAddFamilyHistory.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddFamilyHistory.Autosize = true;
			this.butAddFamilyHistory.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddFamilyHistory.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddFamilyHistory.CornerRadius = 4F;
			this.butAddFamilyHistory.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddFamilyHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddFamilyHistory.Location = new System.Drawing.Point(6, 6);
			this.butAddFamilyHistory.Name = "butAddFamilyHistory";
			this.butAddFamilyHistory.Size = new System.Drawing.Size(137, 23);
			this.butAddFamilyHistory.TabIndex = 70;
			this.butAddFamilyHistory.Text = "Add Family History";
			this.butAddFamilyHistory.Click += new System.EventHandler(this.butAddFamilyHistory_Click);
			// 
			// tabControlFormMedical
			// 
			this.tabControlFormMedical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlFormMedical.Controls.Add(this.tabMedical);
			this.tabControlFormMedical.Controls.Add(this.tabProblems);
			this.tabControlFormMedical.Controls.Add(this.tabMedications);
			this.tabControlFormMedical.Controls.Add(this.tabAllergies);
			this.tabControlFormMedical.Controls.Add(this.tabFamHealthHist);
			this.tabControlFormMedical.Controls.Add(this.tabVitalSigns);
			this.tabControlFormMedical.Location = new System.Drawing.Point(4, 3);
			this.tabControlFormMedical.Name = "tabControlFormMedical";
			this.tabControlFormMedical.SelectedIndex = 0;
			this.tabControlFormMedical.Size = new System.Drawing.Size(777, 409);
			this.tabControlFormMedical.TabIndex = 73;
			// 
			// tabMedical
			// 
			this.tabMedical.Controls.Add(this.label4);
			this.tabMedical.Controls.Add(this.label2);
			this.tabMedical.Controls.Add(this.label3);
			this.tabMedical.Controls.Add(this.label1);
			this.tabMedical.Controls.Add(this.textMedical);
			this.tabMedical.Controls.Add(this.butPrintMedical);
			this.tabMedical.Controls.Add(this.textService);
			this.tabMedical.Controls.Add(this.textMedUrgNote);
			this.tabMedical.Controls.Add(this.checkPremed);
			this.tabMedical.Controls.Add(this.groupMedsDocumented);
			this.tabMedical.Controls.Add(this.label6);
			this.tabMedical.Controls.Add(this.textMedicalComp);
			this.tabMedical.Location = new System.Drawing.Point(4, 22);
			this.tabMedical.Name = "tabMedical";
			this.tabMedical.Padding = new System.Windows.Forms.Padding(3);
			this.tabMedical.Size = new System.Drawing.Size(769, 383);
			this.tabMedical.TabIndex = 4;
			this.tabMedical.Text = "Medical Info";
			this.tabMedical.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 18);
			this.label4.TabIndex = 85;
			this.label4.Text = "Medical Summary";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(131, 18);
			this.label2.TabIndex = 86;
			this.label2.Text = "Med Urgent";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(230, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 18);
			this.label3.TabIndex = 87;
			this.label3.Text = "Service Notes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(121, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(390, 18);
			this.label1.TabIndex = 93;
			this.label1.Text = "To print medications, use button in Medications tab.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMedical
			// 
			this.textMedical.AcceptsTab = true;
			this.textMedical.BackColor = System.Drawing.SystemColors.Window;
			this.textMedical.DetectUrls = false;
			this.textMedical.Location = new System.Drawing.Point(6, 139);
			this.textMedical.Name = "textMedical";
			this.textMedical.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalSummary;
			this.textMedical.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedical.Size = new System.Drawing.Size(220, 69);
			this.textMedical.TabIndex = 2;
			this.textMedical.Text = "";
			// 
			// butPrintMedical
			// 
			this.butPrintMedical.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintMedical.Autosize = true;
			this.butPrintMedical.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintMedical.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintMedical.CornerRadius = 4F;
			this.butPrintMedical.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrintMedical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintMedical.Location = new System.Drawing.Point(6, 6);
			this.butPrintMedical.Name = "butPrintMedical";
			this.butPrintMedical.Size = new System.Drawing.Size(112, 24);
			this.butPrintMedical.TabIndex = 92;
			this.butPrintMedical.Text = "Print Medical";
			this.butPrintMedical.Click += new System.EventHandler(this.butPrintMedical_Click);
			// 
			// textService
			// 
			this.textService.AcceptsTab = true;
			this.textService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textService.BackColor = System.Drawing.SystemColors.Window;
			this.textService.DetectUrls = false;
			this.textService.Location = new System.Drawing.Point(233, 53);
			this.textService.Name = "textService";
			this.textService.QuickPasteType = OpenDentBusiness.QuickPasteType.ServiceNotes;
			this.textService.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textService.Size = new System.Drawing.Size(527, 155);
			this.textService.TabIndex = 3;
			this.textService.Text = "";
			// 
			// textMedUrgNote
			// 
			this.textMedUrgNote.AcceptsTab = true;
			this.textMedUrgNote.BackColor = System.Drawing.SystemColors.Window;
			this.textMedUrgNote.DetectUrls = false;
			this.textMedUrgNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textMedUrgNote.ForeColor = System.Drawing.Color.Red;
			this.textMedUrgNote.Location = new System.Drawing.Point(7, 53);
			this.textMedUrgNote.Name = "textMedUrgNote";
			this.textMedUrgNote.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalUrgent;
			this.textMedUrgNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedUrgNote.Size = new System.Drawing.Size(220, 64);
			this.textMedUrgNote.TabIndex = 1;
			this.textMedUrgNote.Text = "";
			// 
			// checkPremed
			// 
			this.checkPremed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPremed.Location = new System.Drawing.Point(387, 29);
			this.checkPremed.Name = "checkPremed";
			this.checkPremed.Size = new System.Drawing.Size(195, 18);
			this.checkPremed.TabIndex = 5;
			this.checkPremed.Text = "Premedicate (PAC or other)";
			this.checkPremed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPremed.UseVisualStyleBackColor = true;
			// 
			// groupMedsDocumented
			// 
			this.groupMedsDocumented.Controls.Add(this.radioMedsDocumentedNo);
			this.groupMedsDocumented.Controls.Add(this.radioMedsDocumentedYes);
			this.groupMedsDocumented.Location = new System.Drawing.Point(601, 14);
			this.groupMedsDocumented.Name = "groupMedsDocumented";
			this.groupMedsDocumented.Size = new System.Drawing.Size(159, 33);
			this.groupMedsDocumented.TabIndex = 6;
			this.groupMedsDocumented.TabStop = false;
			this.groupMedsDocumented.Text = "Current Meds Documented";
			// 
			// radioMedsDocumentedNo
			// 
			this.radioMedsDocumentedNo.Location = new System.Drawing.Point(93, 13);
			this.radioMedsDocumentedNo.Name = "radioMedsDocumentedNo";
			this.radioMedsDocumentedNo.Size = new System.Drawing.Size(60, 18);
			this.radioMedsDocumentedNo.TabIndex = 1;
			this.radioMedsDocumentedNo.Text = "No";
			this.radioMedsDocumentedNo.UseVisualStyleBackColor = true;
			// 
			// radioMedsDocumentedYes
			// 
			this.radioMedsDocumentedYes.Checked = true;
			this.radioMedsDocumentedYes.Location = new System.Drawing.Point(23, 13);
			this.radioMedsDocumentedYes.Name = "radioMedsDocumentedYes";
			this.radioMedsDocumentedYes.Size = new System.Drawing.Size(66, 18);
			this.radioMedsDocumentedYes.TabIndex = 0;
			this.radioMedsDocumentedYes.TabStop = true;
			this.radioMedsDocumentedYes.Text = "Yes";
			this.radioMedsDocumentedYes.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 211);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(607, 18);
			this.label6.TabIndex = 82;
			this.label6.Text = "Medical History - Complete and Detailed (does not show in chart)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textMedicalComp
			// 
			this.textMedicalComp.AcceptsTab = true;
			this.textMedicalComp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textMedicalComp.BackColor = System.Drawing.SystemColors.Window;
			this.textMedicalComp.DetectUrls = false;
			this.textMedicalComp.Location = new System.Drawing.Point(9, 230);
			this.textMedicalComp.Name = "textMedicalComp";
			this.textMedicalComp.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalHistory;
			this.textMedicalComp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedicalComp.Size = new System.Drawing.Size(751, 145);
			this.textMedicalComp.TabIndex = 4;
			this.textMedicalComp.Text = "";
			// 
			// tabProblems
			// 
			this.tabProblems.Controls.Add(this.gridDiseases);
			this.tabProblems.Controls.Add(this.butAddDisease);
			this.tabProblems.Controls.Add(this.checkShowInactiveProblems);
			this.tabProblems.Location = new System.Drawing.Point(4, 22);
			this.tabProblems.Name = "tabProblems";
			this.tabProblems.Padding = new System.Windows.Forms.Padding(3);
			this.tabProblems.Size = new System.Drawing.Size(769, 383);
			this.tabProblems.TabIndex = 0;
			this.tabProblems.Text = "Problems";
			this.tabProblems.UseVisualStyleBackColor = true;
			// 
			// tabMedications
			// 
			this.tabMedications.Controls.Add(this.butAdd);
			this.tabMedications.Controls.Add(this.gridMeds);
			this.tabMedications.Controls.Add(this.checkDiscontinued);
			this.tabMedications.Controls.Add(this.butPrint);
			this.tabMedications.Location = new System.Drawing.Point(4, 22);
			this.tabMedications.Name = "tabMedications";
			this.tabMedications.Padding = new System.Windows.Forms.Padding(3);
			this.tabMedications.Size = new System.Drawing.Size(769, 383);
			this.tabMedications.TabIndex = 1;
			this.tabMedications.Text = "Medications";
			this.tabMedications.UseVisualStyleBackColor = true;
			// 
			// tabAllergies
			// 
			this.tabAllergies.Controls.Add(this.gridAllergies);
			this.tabAllergies.Controls.Add(this.checkShowInactiveAllergies);
			this.tabAllergies.Controls.Add(this.butAddAllergy);
			this.tabAllergies.Location = new System.Drawing.Point(4, 22);
			this.tabAllergies.Name = "tabAllergies";
			this.tabAllergies.Padding = new System.Windows.Forms.Padding(3);
			this.tabAllergies.Size = new System.Drawing.Size(769, 383);
			this.tabAllergies.TabIndex = 2;
			this.tabAllergies.Text = "Allergies";
			this.tabAllergies.UseVisualStyleBackColor = true;
			// 
			// tabFamHealthHist
			// 
			this.tabFamHealthHist.Controls.Add(this.gridFamilyHealth);
			this.tabFamHealthHist.Controls.Add(this.butAddFamilyHistory);
			this.tabFamHealthHist.Location = new System.Drawing.Point(4, 22);
			this.tabFamHealthHist.Name = "tabFamHealthHist";
			this.tabFamHealthHist.Padding = new System.Windows.Forms.Padding(3);
			this.tabFamHealthHist.Size = new System.Drawing.Size(769, 383);
			this.tabFamHealthHist.TabIndex = 3;
			this.tabFamHealthHist.Text = "Family Health History";
			this.tabFamHealthHist.UseVisualStyleBackColor = true;
			// 
			// tabVitalSigns
			// 
			this.tabVitalSigns.Controls.Add(this.butGrowthChart);
			this.tabVitalSigns.Controls.Add(this.butAddVitalSign);
			this.tabVitalSigns.Controls.Add(this.gridVitalSigns);
			this.tabVitalSigns.Location = new System.Drawing.Point(4, 22);
			this.tabVitalSigns.Name = "tabVitalSigns";
			this.tabVitalSigns.Padding = new System.Windows.Forms.Padding(3);
			this.tabVitalSigns.Size = new System.Drawing.Size(769, 383);
			this.tabVitalSigns.TabIndex = 5;
			this.tabVitalSigns.Text = "Vital Signs";
			this.tabVitalSigns.UseVisualStyleBackColor = true;
			// 
			// butGrowthChart
			// 
			this.butGrowthChart.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butGrowthChart.Autosize = true;
			this.butGrowthChart.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGrowthChart.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGrowthChart.CornerRadius = 4F;
			this.butGrowthChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGrowthChart.Location = new System.Drawing.Point(122, 6);
			this.butGrowthChart.Name = "butGrowthChart";
			this.butGrowthChart.Size = new System.Drawing.Size(92, 23);
			this.butGrowthChart.TabIndex = 72;
			this.butGrowthChart.Text = "Growth Chart";
			this.butGrowthChart.Click += new System.EventHandler(this.butGrowthChart_Click);
			// 
			// butAddVitalSign
			// 
			this.butAddVitalSign.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddVitalSign.Autosize = true;
			this.butAddVitalSign.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddVitalSign.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddVitalSign.CornerRadius = 4F;
			this.butAddVitalSign.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddVitalSign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddVitalSign.Location = new System.Drawing.Point(6, 6);
			this.butAddVitalSign.Name = "butAddVitalSign";
			this.butAddVitalSign.Size = new System.Drawing.Size(110, 23);
			this.butAddVitalSign.TabIndex = 71;
			this.butAddVitalSign.Text = "Add Vital Sign";
			this.butAddVitalSign.Click += new System.EventHandler(this.butAddVitalSign_Click);
			// 
			// gridVitalSigns
			// 
			this.gridVitalSigns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridVitalSigns.HasAddButton = false;
			this.gridVitalSigns.HasMultilineHeaders = false;
			this.gridVitalSigns.HScrollVisible = false;
			this.gridVitalSigns.Location = new System.Drawing.Point(6, 35);
			this.gridVitalSigns.Name = "gridVitalSigns";
			this.gridVitalSigns.ScrollValue = 0;
			this.gridVitalSigns.Size = new System.Drawing.Size(757, 342);
			this.gridVitalSigns.TabIndex = 4;
			this.gridVitalSigns.Title = "Vital Signs";
			this.gridVitalSigns.TranslationName = null;
			this.gridVitalSigns.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridVitalSigns_CellDoubleClick);
			// 
			// FormMedical
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(788, 450);
			this.Controls.Add(this.tabControlFormMedical);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 250);
			this.Name = "FormMedical";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medical";
			this.Load += new System.EventHandler(this.FormMedical_Load);
			this.tabControlFormMedical.ResumeLayout(false);
			this.tabMedical.ResumeLayout(false);
			this.groupMedsDocumented.ResumeLayout(false);
			this.tabProblems.ResumeLayout(false);
			this.tabMedications.ResumeLayout(false);
			this.tabAllergies.ResumeLayout(false);
			this.tabFamHealthHist.ResumeLayout(false);
			this.tabVitalSigns.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMedical_Load(object sender, System.EventArgs e){
			SecurityLogs.MakeLogEntry(Permissions.MedicalInfoViewed,PatCur.PatNum,"Patient medical information viewed");
			checkPremed.Checked=PatCur.Premed;
			textMedUrgNote.Text=PatCur.MedUrgNote;
			textMedical.Text=PatientNoteCur.Medical;
			textMedicalComp.Text=PatientNoteCur.MedicalComp;
			textService.Text=PatientNoteCur.Service;
			FillMeds();
			FillProblems();
			FillAllergies();
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				FillFamilyHealth();
				FillVitalSigns();
			}
			else {
				//remove EHR only tabs if ShowFeatureEHR is not enabled.
				tabControlFormMedical.TabPages.RemoveByKey("tabVitalSigns");
				tabControlFormMedical.TabPages.RemoveByKey("tabFamHealthHist");
			}
			List<EhrMeasureEvent> listDocumentedMedEvents=EhrMeasureEvents.RefreshByType(PatCur.PatNum,EhrMeasureEventType.CurrentMedsDocumented);
			_EhrMeasureEventNum=0;
			for(int i=0;i<listDocumentedMedEvents.Count;i++) {
				if(listDocumentedMedEvents[i].DateTEvent.Date==DateTime.Today) {
					radioMedsDocumentedYes.Checked=true;
					_EhrMeasureEventNum=listDocumentedMedEvents[i].EhrMeasureEventNum;
					break;
				}
			}
			_EhrNotPerfNum=0;
			List<EhrNotPerformed> listNotPerfs=EhrNotPerformeds.Refresh(PatCur.PatNum);
			for(int i=0;i<listNotPerfs.Count;i++) {
				if(listNotPerfs[i].CodeValue!="428191000124101") {//this is the only allowed code for Current Meds Documented procedure
					continue;
				}
				if(listNotPerfs[i].DateEntry.Date==DateTime.Today) {
					radioMedsDocumentedNo.Checked=!radioMedsDocumentedYes.Checked;//only check the No radio button if the Yes radio button is not already set
					_EhrNotPerfNum=listNotPerfs[i].EhrNotPerformedNum;
					break;
				}
			}
		}

		private void FillMeds() {
			Medications.Refresh();
			medList=MedicationPats.Refresh(PatCur.PatNum,checkDiscontinued.Checked);
			gridMeds.BeginUpdate();
			gridMeds.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton){//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridMeds.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableMedications","Medication"),180);
			gridMeds.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Notes"),240);
			gridMeds.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Notes for Patient"),240);
			gridMeds.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Status"),40,HorizontalAlignment.Center);
			gridMeds.Columns.Add(col);
			gridMeds.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<medList.Count;i++) {
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				if(medList[i].MedicationNum==0) {
					row.Cells.Add(medList[i].MedDescript);
					row.Cells.Add("");//generic notes
				}
				else {
					Medication generic=Medications.GetGeneric(medList[i].MedicationNum);
					string medName=Medications.GetMedication(medList[i].MedicationNum).MedName;
					if(generic.MedicationNum!=medList[i].MedicationNum) {//not generic
						medName+=" ("+generic.MedName+")";
					}
					row.Cells.Add(medName);
					row.Cells.Add(Medications.GetGeneric(medList[i].MedicationNum).Notes);
				}
				row.Cells.Add(medList[i].PatNote);
				if(MedicationPats.IsMedActive(medList[i])) {
					row.Cells.Add("Active");
				}
				else {
					row.Cells.Add("Inactive");
				}
				gridMeds.Rows.Add(row);
			}
			gridMeds.EndUpdate();
		}

		private void gridMeds_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			FormInfobutton FormIB = new FormInfobutton();
			FormIB.PatCur=PatCur;
			//FormInfoButton allows MedicationCur to be null, so this will still work for medication orders returned from NewCrop (because MedicationNum will be 0).
			FormIB.ListObjects.Add(medList[e.Row]);//TODO: verify that this is what we need to get.
			//FormIB.ListObjects.Add(Medications.GetMedicationFromDb(medList[e.Row].MedicationNum));//TODO: verify that this is what we need to get.
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridMeds_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedPat FormMP=new FormMedPat();
			FormMP.MedicationPatCur=medList[e.Row];
			FormMP.ShowDialog();
			if(FormMP.DialogResult==DialogResult.OK 
				&& FormMP.MedicationPatCur!=null //Can get be null if the user removed the medication from the patient.
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).MedicationCDS) 
			{
				object triggerObject=null;
				if(FormMP.MedicationPatCur.MedicationNum > 0) {//0 indicats the med is from NewCrop.
					triggerObject=Medications.GetMedication(FormMP.MedicationPatCur.MedicationNum);
				}
				else if(FormMP.MedicationPatCur.RxCui > 0) {//Meds from NewCrop might have a valid RxNorm.
					triggerObject=RxNorms.GetByRxCUI(FormMP.MedicationPatCur.RxCui.ToString());
				}
				if(triggerObject!=null) {
					FormCDSIntervention FormCDSI=new FormCDSIntervention();
					FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(triggerObject,PatCur);
					FormCDSI.ShowIfRequired(false);
				}
			}
			FillMeds();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			//select medication from list.  Additional meds can be added to the list from within that dlg
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult!=DialogResult.OK){
				return;
			} 
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).MedicationCDS) {
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(Medications.GetMedication(FormM.SelectedMedicationNum),PatCur);
				FormCDSI.ShowIfRequired();
				if(FormCDSI.DialogResult==DialogResult.Abort) {
					return;//do not add medication
				}
			}
			MedicationPat MedicationPatCur=new MedicationPat();
			MedicationPatCur.PatNum=PatCur.PatNum;
			MedicationPatCur.MedicationNum=FormM.SelectedMedicationNum;
			MedicationPatCur.RxCui=Medications.GetMedication(FormM.SelectedMedicationNum).RxCui;
			MedicationPatCur.ProvNum=PatCur.PriProv;
			FormMedPat FormMP=new FormMedPat();
			FormMP.MedicationPatCur=MedicationPatCur;
			FormMP.IsNew=true;
			FormMP.ShowDialog();
			if(FormMP.DialogResult!=DialogResult.OK){
				return;
			}
			FillMeds();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			//pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
#if DEBUG
        FormRpPrintPreview pView = new FormRpPrintPreview();
        pView.printPreviewControl2.Document=pd;
        pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,PatCur.PatNum,"Medications printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Medications List For ")+PatCur.FName+" "+PatCur.LName;
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=Lan.g(this,"Created ")+DateTime.Now.ToString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMeds.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		/// <summary>This report is a brute force, one page medical history report. It is not designed to handle more than one page. It does not print service notes or medications.</summary>
		private void butPrintMedical_Click(object sender,EventArgs e) {
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPageMedical);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try {
#if DEBUG
        FormRpPrintPreview pView = new FormRpPrintPreview();
        pView.printPreviewControl2.Document=pd;
        pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,PatCur.PatNum,"Medical information printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPageMedical(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			Font bodyFont=new Font(FontFamily.GenericSansSerif,10);
			StringFormat format=new StringFormat();
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			int textHeight;
			RectangleF textRect;
			text=Lan.g(this,"Medical History For ")+PatCur.FName+" "+PatCur.LName;
			g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,headingFont).Height;
			text=Lan.g(this,"Birthdate: ")+PatCur.Birthdate.ToShortDateString();
			g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=Lan.g(this,"Created ")+DateTime.Now.ToString();
			g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			yPos+=25;
			if(gridDiseases.Rows.Count>0) {
				text=Lan.g(this,"Problems");
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				yPos+=2;
				yPos=gridDiseases.PrintPage(g,0,bounds,yPos);
				yPos+=25;
			}
			if(gridAllergies.Rows.Count>0) {
				text=Lan.g(this,"Allergies");
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				yPos+=2;
				yPos=gridAllergies.PrintPage(g,0,bounds,yPos);
				yPos+=25;
			}
			text=Lan.g(this,"Premedicate (PAC or other): ")+(checkPremed.Checked?"Y":"N");
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,subHeadingFont,Brushes.Black,textRect);
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical Urgent Note");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedUrgNote.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);//maybe red?
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical Summary");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedical.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical History - Complete and Detailed");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedicalComp.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);
			yPos+=textHeight;
			g.Dispose();
		}

		private void FillFamilyHealth() {
			ListFamHealth=FamilyHealths.GetFamilyHealthForPat(PatCur.PatNum);
			gridFamilyHealth.BeginUpdate();
			gridFamilyHealth.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableFamilyHealth","Relationship"),150,HorizontalAlignment.Center);
			gridFamilyHealth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFamilyHealth","Name"),150);
			gridFamilyHealth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFamilyHealth","Problem"),180);
			gridFamilyHealth.Columns.Add(col);
			gridFamilyHealth.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListFamHealth.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Lan.g("enumFamilyRelationship",ListFamHealth[i].Relationship.ToString()));
				row.Cells.Add(ListFamHealth[i].PersonName);
				row.Cells.Add(DiseaseDefs.GetName(ListFamHealth[i].DiseaseDefNum));
				gridFamilyHealth.Rows.Add(row);
			}
			gridFamilyHealth.EndUpdate();
		}

		private void FillProblems(){
			DiseaseList=Diseases.Refresh(checkShowInactiveProblems.Checked,PatCur.PatNum);
			gridDiseases.BeginUpdate();
			gridDiseases.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridDiseases.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableDiseases","Name"),200);//total is about 325
			gridDiseases.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDiseases","Patient Note"),450);
			gridDiseases.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDisease","Status"),40,HorizontalAlignment.Center);
			gridDiseases.Columns.Add(col);
			gridDiseases.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<DiseaseList.Count;i++){
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				if(DiseaseList[i].DiseaseDefNum!=0) {
					row.Cells.Add(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
				}
				else {
					row.Cells.Add(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
				}
				row.Cells.Add(DiseaseList[i].PatNote);
				row.Cells.Add(DiseaseList[i].ProbStatus.ToString());
				gridDiseases.Rows.Add(row);
			}
			gridDiseases.EndUpdate();
		}

		private void FillAllergies() {
			allergyList=Allergies.GetAll(PatCur.PatNum,checkShowInactiveAllergies.Checked);
			gridAllergies.BeginUpdate();
			gridAllergies.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridAllergies.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableAllergies","Allergy"),150);
			gridAllergies.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAllergies","Reaction"),500);
			gridAllergies.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAllergies","Status"),40,HorizontalAlignment.Center);
			gridAllergies.Columns.Add(col);
			gridAllergies.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<allergyList.Count;i++){
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				AllergyDef allergyDef=AllergyDefs.GetOne(allergyList[i].AllergyDefNum);
				row.Cells.Add(allergyDef.Description);
				if(allergyList[i].DateAdverseReaction<DateTime.Parse("1-1-1800")) {
					row.Cells.Add(allergyList[i].Reaction);
				}
				else {
					row.Cells.Add(allergyList[i].Reaction+" "+allergyList[i].DateAdverseReaction.ToShortDateString());
				}
				if(allergyList[i].StatusIsActive) {
					row.Cells.Add("Active");
				}
				else {
					row.Cells.Add("Inactive");
				}
				gridAllergies.Rows.Add(row);
			}
			gridAllergies.EndUpdate();
		}

		private void FillVitalSigns() {
			gridVitalSigns.BeginUpdate();
			gridVitalSigns.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Date",80);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Pulse",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Height",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Weight",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("BP",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("BMI",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Documentation for Followup or Ineligible",150);
			gridVitalSigns.Columns.Add(col);
			_listVitalSigns=Vitalsigns.Refresh(PatCur.PatNum);
			gridVitalSigns.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listVitalSigns.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listVitalSigns[i].DateTaken.ToShortDateString());
				row.Cells.Add(_listVitalSigns[i].Pulse.ToString()+" bpm");
				row.Cells.Add(_listVitalSigns[i].Height.ToString()+" in.");
				row.Cells.Add(_listVitalSigns[i].Weight.ToString()+" lbs.");
				row.Cells.Add(_listVitalSigns[i].BpSystolic.ToString()+"/"+_listVitalSigns[i].BpDiastolic.ToString());
				//BMI = (lbs*703)/(in^2)
				float bmi=Vitalsigns.CalcBMI(_listVitalSigns[i].Weight,_listVitalSigns[i].Height);
				if(bmi!=0) {
					row.Cells.Add(bmi.ToString("n1"));
				}
				else {//leave cell blank because there is not a valid bmi
					row.Cells.Add("");
				}
				row.Cells.Add(_listVitalSigns[i].Documentation);
				gridVitalSigns.Rows.Add(row);
			}
			gridVitalSigns.EndUpdate();
		}

		private void butAddProblem_Click(object sender,EventArgs e) {
			FormDiseaseDefs formDD=new FormDiseaseDefs();
			formDD.IsSelectionMode=true;
			formDD.IsMultiSelect=true;
			formDD.ShowDialog();
			if(formDD.DialogResult!=DialogResult.OK) {
				return;
			}
			for(int i=0;i<formDD.SelectedDiseaseDefNums.Count;i++) {
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).ProblemCDS){
					FormCDSIntervention FormCDSI=new FormCDSIntervention();
					FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(DiseaseDefs.GetItem(formDD.SelectedDiseaseDefNums[i]),PatCur);
					FormCDSI.ShowIfRequired();
					if(FormCDSI.DialogResult==DialogResult.Abort) {
						continue;//cancel 
					}
				}
				SecurityLogs.MakeLogEntry(Permissions.PatProblemListEdit,PatCur.PatNum,DiseaseDefs.GetName(formDD.SelectedDiseaseDefNums[i])+" added"); //Audit log made outside form because the form is just a list of problems and is called from many places.
				Disease disease=new Disease();
				disease.PatNum=PatCur.PatNum;
				disease.DiseaseDefNum=formDD.SelectedDiseaseDefNums[i];
				Diseases.Insert(disease);
			}
			FillProblems();
		}

		/*private void butIcd9_Click(object sender,EventArgs e) {
			FormIcd9s formI=new FormIcd9s();
			formI.IsSelectionMode=true;
			formI.ShowDialog();
			if(formI.DialogResult!=DialogResult.OK) {
				return;
			}
			Disease disease=new Disease();
			disease.PatNum=PatCur.PatNum;
			disease.ICD9Num=formI.SelectedIcd9Num;
			Diseases.Insert(disease);
			FillProblems();
		}*/

		private void gridDiseases_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			FormInfobutton FormIB=new FormInfobutton();
			FormIB.PatCur=PatCur;
			FormIB.ListObjects.Add(DiseaseDefs.GetItem(DiseaseList[e.Row].DiseaseDefNum));//TODO: verify that this is what we need to get.
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridDiseases_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDiseaseEdit FormD=new FormDiseaseEdit(DiseaseList[e.Row]);
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.OK 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ProblemCDS) 
			{
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(DiseaseDefs.GetItem(DiseaseList[e.Row].DiseaseDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillProblems();
		}

		private void checkShowInactiveProblems_CheckedChanged(object sender,EventArgs e) {
			FillProblems();
		}

		private void gridFamilyHealth_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormFamilyHealthEdit FormFHE=new FormFamilyHealthEdit();
			FormFHE.FamilyHealthCur=ListFamHealth[e.Row];
			FormFHE.ShowDialog();
			FillFamilyHealth();
		}

		private void butAddFamilyHistory_Click(object sender,EventArgs e) {
			FormFamilyHealthEdit FormFHE=new FormFamilyHealthEdit();
			FamilyHealth famH=new FamilyHealth();
			famH.PatNum=PatCur.PatNum;
			famH.IsNew=true;
			FormFHE.FamilyHealthCur=famH;
			FormFHE.ShowDialog();
			if(FormFHE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillFamilyHealth();
		}

		private void gridAllergies_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			FormInfobutton FormIB=new FormInfobutton();
			FormIB.PatCur=PatCur;
			//TODO: get right object and pass it in.
			//FormIB. = Medications.GetMedicationFromDb(medList[e.Row].MedicationNum);//TODO: verify that this is what we need to get.
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridAllergies_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAllergyEdit FAE=new FormAllergyEdit();
			FAE.AllergyCur=allergyList[gridAllergies.GetSelectedIndex()];
			FAE.ShowDialog();
			if(FAE.DialogResult==DialogResult.OK 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).AllergyCDS) 
			{
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(AllergyDefs.GetOne(FAE.AllergyCur.AllergyDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillAllergies();
		}
		
		private void checkShowInactiveAllergies_CheckedChanged(object sender,EventArgs e) {
			FillAllergies();
		}

		/*
		private void butQuestions_Click(object sender,EventArgs e) {
			FormQuestionnaire FormQ=new FormQuestionnaire(PatCur.PatNum);
			FormQ.ShowDialog();
			if(Questions.PatHasQuest(PatCur.PatNum)) {
				butQuestions.Text=Lan.g(this,"Edit Questionnaire");
			}
			else {
				butQuestions.Text=Lan.g(this,"New Questionnaire");
			}
		}*/

		private void checkShowDiscontinuedMeds_MouseUp(object sender,MouseEventArgs e) {
			FillMeds();
		}

		private void checkDiscontinued_KeyUp(object sender,KeyEventArgs e) {
			FillMeds();
		}

		private void butMedicationReconcile_Click(object sender,EventArgs e) {
			FormMedicationReconcile FormMR=new FormMedicationReconcile();
			FormMR.PatCur=PatCur;
			FormMR.ShowDialog();
			FillMeds();
		}

		private void butAddAllergy_Click(object sender,EventArgs e) {
			FormAllergyEdit formA=new FormAllergyEdit();
			formA.AllergyCur=new Allergy();
			formA.AllergyCur.StatusIsActive=true;
			formA.AllergyCur.PatNum=PatCur.PatNum;
			formA.AllergyCur.IsNew=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).AllergyCDS) {
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(AllergyDefs.GetOne(formA.AllergyCur.AllergyDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillAllergies();
		}

		private void butAddVitalSign_Click(object sender,EventArgs e) {
			FormVitalsignEdit2014 FormVSE=new FormVitalsignEdit2014();
			FormVSE.VitalsignCur=new Vitalsign();
			FormVSE.VitalsignCur.PatNum=PatCur.PatNum;
			FormVSE.VitalsignCur.DateTaken=DateTime.Today;
			FormVSE.VitalsignCur.IsNew=true;
			FormVSE.ShowDialog();
			FillVitalSigns();
		}

		private void gridVitalSigns_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormVitalsignEdit2014 FormVSE=new FormVitalsignEdit2014();
			FormVSE.VitalsignCur=_listVitalSigns[e.Row];
			FormVSE.ShowDialog();
			FillVitalSigns();
		}

		private void butGrowthChart_Click(object sender,EventArgs e) {
			FormEhrGrowthCharts FormGC=new FormEhrGrowthCharts();
			FormGC.PatNum=PatCur.PatNum;
			FormGC.ShowDialog();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			Patient PatOld=PatCur.Copy();
			PatCur.Premed=checkPremed.Checked;
			PatCur.MedUrgNote=textMedUrgNote.Text;
			Patients.Update(PatCur,PatOld);
			PatientNoteCur.Medical=textMedical.Text;
			PatientNoteCur.Service=textService.Text;
			PatientNoteCur.MedicalComp=textMedicalComp.Text;
			PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
			//Insert an ehrmeasureevent for CurrentMedsDocumented if user selected Yes and there isn't one with today's date
			if(radioMedsDocumentedYes.Checked && _EhrMeasureEventNum==0) {
				EhrMeasureEvent ehrMeasureEventCur=new EhrMeasureEvent();
				ehrMeasureEventCur.PatNum=PatCur.PatNum;
				ehrMeasureEventCur.DateTEvent=DateTime.Now;
				ehrMeasureEventCur.EventType=EhrMeasureEventType.CurrentMedsDocumented;
				ehrMeasureEventCur.CodeValueEvent="428191000124101";//SNOMEDCT code for document current meds procedure
				ehrMeasureEventCur.CodeSystemEvent="SNOMEDCT";
				EhrMeasureEvents.Insert(ehrMeasureEventCur);
			}
			//No is selected, if no EhrNotPerformed item for current meds documented, launch not performed edit window to allow user to select valid reason.
			if(radioMedsDocumentedNo.Checked) {
				if(_EhrNotPerfNum==0) {
					FormEhrNotPerformedEdit FormNP=new FormEhrNotPerformedEdit();
					FormNP.EhrNotPerfCur=new EhrNotPerformed();
					FormNP.EhrNotPerfCur.IsNew=true;
					FormNP.EhrNotPerfCur.PatNum=PatCur.PatNum;
					FormNP.EhrNotPerfCur.ProvNum=PatCur.PriProv;
					FormNP.SelectedItemIndex=(int)EhrNotPerformedItem.DocumentCurrentMeds;
					FormNP.EhrNotPerfCur.DateEntry=DateTime.Today;
					FormNP.IsDateReadOnly=true;
					FormNP.ShowDialog();
					if(FormNP.DialogResult==DialogResult.OK) {//if they just inserted a not performed item, set the private class-wide variable for the next if statement
						_EhrNotPerfNum=FormNP.EhrNotPerfCur.EhrNotPerformedNum;
					}
				}
				if(_EhrNotPerfNum>0 && _EhrMeasureEventNum>0) {//if not performed item is entered with today's date, delete existing performed item
					EhrMeasureEvents.Delete(_EhrMeasureEventNum);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		

	

		

		

		

	

		

	}
}
