using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormScreenGroupEdit:System.Windows.Forms.Form {
		private IContainer components;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label labelProv;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboProv;
		private System.Windows.Forms.ComboBox comboPlaceService;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ComboBox comboCounty;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label labelScreener;
		private System.Windows.Forms.TextBox textScreenDate;
		private System.Windows.Forms.TextBox textProvName;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.ComboBox comboGradeSchool;
		public ScreenGroup ScreenGroupCur;
		private UI.Button butCancel;
		private UI.Button butDelete;
		private UI.ODGrid gridMain;
		private OpenDentBusiness.Screen[] _arrayScreens;
		private ODGrid gridScreenPats;
		private List<ScreenPat> _listScreenPats;
		private UI.Button button1;
		private UI.Button butRemovePat;
		private UI.Button butStartScreens;
		private Label label4;
		private ContextMenu patContextMenu;

		///<summary></summary>
		public FormScreenGroupEdit()
		{
			//
			// Required for Windows Form Designer support
			//
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScreenGroupEdit));
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.labelProv = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textScreenDate = new System.Windows.Forms.TextBox();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.comboPlaceService = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboCounty = new System.Windows.Forms.ComboBox();
			this.comboGradeSchool = new System.Windows.Forms.ComboBox();
			this.textProvName = new System.Windows.Forms.TextBox();
			this.labelScreener = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridScreenPats = new OpenDental.UI.ODGrid();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butStartScreens = new OpenDental.UI.Button();
			this.butRemovePat = new OpenDental.UI.Button();
			this.button1 = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(10, 113);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(93, 16);
			this.label14.TabIndex = 12;
			this.label14.Text = "School";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(1, 92);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(101, 15);
			this.label13.TabIndex = 11;
			this.label13.Text = "County";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelProv
			// 
			this.labelProv.Location = new System.Drawing.Point(2, 71);
			this.labelProv.Name = "labelProv";
			this.labelProv.Size = new System.Drawing.Size(101, 16);
			this.labelProv.TabIndex = 50;
			this.labelProv.Text = "or Prov";
			this.labelProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 17);
			this.label1.TabIndex = 51;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textScreenDate
			// 
			this.textScreenDate.Location = new System.Drawing.Point(102, 9);
			this.textScreenDate.Name = "textScreenDate";
			this.textScreenDate.Size = new System.Drawing.Size(64, 20);
			this.textScreenDate.TabIndex = 0;
			this.textScreenDate.Validating += new System.ComponentModel.CancelEventHandler(this.textScreenDate_Validating);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(102, 29);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(173, 20);
			this.textDescription.TabIndex = 1;
			this.textDescription.TextChanged += new System.EventHandler(this.textProvName_TextChanged);
			// 
			// comboProv
			// 
			this.comboProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(102, 69);
			this.comboProv.MaxDropDownItems = 25;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(173, 21);
			this.comboProv.TabIndex = 2;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			this.comboProv.SelectionChangeCommitted += new System.EventHandler(this.comboProv_SelectionChangeCommitted);
			this.comboProv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboProv_KeyDown);
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.BackColor = System.Drawing.SystemColors.Window;
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(102, 132);
			this.comboPlaceService.MaxDropDownItems = 25;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(173, 21);
			this.comboPlaceService.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 133);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(98, 17);
			this.label2.TabIndex = 119;
			this.label2.Text = "Location";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(2, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 128;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboCounty
			// 
			this.comboCounty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCounty.Location = new System.Drawing.Point(102, 90);
			this.comboCounty.Name = "comboCounty";
			this.comboCounty.Size = new System.Drawing.Size(173, 21);
			this.comboCounty.TabIndex = 4;
			this.comboCounty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboCounty_KeyDown);
			// 
			// comboGradeSchool
			// 
			this.comboGradeSchool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGradeSchool.Location = new System.Drawing.Point(102, 111);
			this.comboGradeSchool.Name = "comboGradeSchool";
			this.comboGradeSchool.Size = new System.Drawing.Size(173, 21);
			this.comboGradeSchool.TabIndex = 140;
			this.comboGradeSchool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboGradeSchool_KeyDown);
			// 
			// textProvName
			// 
			this.textProvName.Location = new System.Drawing.Point(102, 49);
			this.textProvName.Name = "textProvName";
			this.textProvName.Size = new System.Drawing.Size(173, 20);
			this.textProvName.TabIndex = 141;
			this.textProvName.TextChanged += new System.EventHandler(this.textProvName_TextChanged);
			this.textProvName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textProvName_KeyUp);
			// 
			// labelScreener
			// 
			this.labelScreener.Location = new System.Drawing.Point(3, 51);
			this.labelScreener.Name = "labelScreener";
			this.labelScreener.Size = new System.Drawing.Size(99, 16);
			this.labelScreener.TabIndex = 142;
			this.labelScreener.Text = "Screener";
			this.labelScreener.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(472, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(390, 17);
			this.label4.TabIndex = 152;
			this.label4.Text = "Right click patient to set screening permission.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridScreenPats
			// 
			this.gridScreenPats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridScreenPats.HasMultilineHeaders = false;
			this.gridScreenPats.HScrollVisible = false;
			this.gridScreenPats.Location = new System.Drawing.Point(472, 21);
			this.gridScreenPats.Name = "gridScreenPats";
			this.gridScreenPats.ScrollValue = 0;
			this.gridScreenPats.Size = new System.Drawing.Size(390, 144);
			this.gridScreenPats.TabIndex = 148;
			this.gridScreenPats.Title = "Patients for Screening";
			this.gridScreenPats.TranslationName = null;
			this.gridScreenPats.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridScreenPats_CellDoubleClick);
			this.gridScreenPats.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridScreenPats_MouseClick);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(2, 165);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(860, 438);
			this.gridMain.TabIndex = 147;
			this.gridMain.Title = "Screenings";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butStartScreens
			// 
			this.butStartScreens.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStartScreens.Autosize = true;
			this.butStartScreens.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStartScreens.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStartScreens.CornerRadius = 4F;
			this.butStartScreens.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butStartScreens.Location = new System.Drawing.Point(375, 106);
			this.butStartScreens.Name = "butStartScreens";
			this.butStartScreens.Size = new System.Drawing.Size(92, 23);
			this.butStartScreens.TabIndex = 151;
			this.butStartScreens.Text = "Screen Patients";
			this.butStartScreens.UseVisualStyleBackColor = true;
			this.butStartScreens.Click += new System.EventHandler(this.butStartScreens_Click);
			// 
			// butRemovePat
			// 
			this.butRemovePat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemovePat.Autosize = true;
			this.butRemovePat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemovePat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemovePat.CornerRadius = 4F;
			this.butRemovePat.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butRemovePat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemovePat.Location = new System.Drawing.Point(392, 52);
			this.butRemovePat.Name = "butRemovePat";
			this.butRemovePat.Size = new System.Drawing.Size(75, 23);
			this.butRemovePat.TabIndex = 150;
			this.butRemovePat.Text = "Remove";
			this.butRemovePat.UseVisualStyleBackColor = true;
			this.butRemovePat.Click += new System.EventHandler(this.butRemovePat_Click);
			// 
			// button1
			// 
			this.button1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button1.Autosize = true;
			this.button1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button1.CornerRadius = 4F;
			this.button1.Image = global::OpenDental.Properties.Resources.Add;
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.Location = new System.Drawing.Point(392, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 149;
			this.button1.Text = "Add";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.butAddPat_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(375, 613);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(114, 24);
			this.butAdd.TabIndex = 146;
			this.butAdd.Text = "Add Anonymous";
			this.butAdd.Click += new System.EventHandler(this.butAddAnonymous_Click);
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
			this.butDelete.Location = new System.Drawing.Point(12, 613);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(70, 24);
			this.butDelete.TabIndex = 24;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(782, 613);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(70, 24);
			this.butCancel.TabIndex = 24;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(706, 613);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(70, 24);
			this.butOK.TabIndex = 24;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormScreenGroupEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(864, 641);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butStartScreens);
			this.Controls.Add(this.butRemovePat);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.gridScreenPats);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.textProvName);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.labelProv);
			this.Controls.Add(this.labelScreener);
			this.Controls.Add(this.comboGradeSchool);
			this.Controls.Add(this.comboCounty);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textScreenDate);
			this.Controls.Add(this.comboPlaceService);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormScreenGroupEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Screening Group";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormScreenGroupEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormScreenGroup_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormScreenGroup_Load(object sender, System.EventArgs e) {
			if(IsNew){
				ScreenGroups.Insert(ScreenGroupCur);
			}
			patContextMenu=new ContextMenu();
			_listScreenPats=ScreenPats.GetForScreenGroup(ScreenGroupCur.ScreenGroupNum);
			FillGrid();
			FillScreenPats();
			string[] patScreenPerms=Enum.GetNames(typeof(PatScreenPerm));
			for(int i=0;i<patScreenPerms.Length;i++) {
				patContextMenu.MenuItems.Add(new MenuItem(patScreenPerms[i],patContextMenuItem_Click));
			}
			if(_arrayScreens.Length>0){
				OpenDentBusiness.Screen ScreenCur=_arrayScreens[0];
				ScreenGroupCur.SGDate=ScreenCur.ScreenDate;
				ScreenGroupCur.ProvName=ScreenCur.ProvName;
				ScreenGroupCur.ProvNum=ScreenCur.ProvNum;
				ScreenGroupCur.County=ScreenCur.County;
				ScreenGroupCur.GradeSchool=ScreenCur.GradeSchool;
				ScreenGroupCur.PlaceService=ScreenCur.PlaceService;
			}
			textScreenDate.Text=ScreenGroupCur.SGDate.ToShortDateString();
			textDescription.Text=ScreenGroupCur.Description;
			textProvName.Text=ScreenGroupCur.ProvName;//has to be filled before provnum
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				comboProv.Items.Add(ProviderC.ListShort[i].Abbr);
				if(ScreenGroupCur.ProvNum==ProviderC.ListShort[i].ProvNum){
					comboProv.SelectedIndex=i;
				}
			}
			string[] CountiesListNames=Counties.GetListNames();
			comboCounty.Items.AddRange(CountiesListNames);
			if(ScreenGroupCur.County==null)
				ScreenGroupCur.County="";//prevents the next line from crashing
			comboCounty.SelectedIndex=comboCounty.Items.IndexOf(ScreenGroupCur.County);//"" etc OK
			for(int i=0;i<SiteC.List.Length;i++) {
				comboGradeSchool.Items.Add(SiteC.List[i].Description);
			}
			if(ScreenGroupCur.GradeSchool==null)
				ScreenGroupCur.GradeSchool="";//prevents the next line from crashing
			comboGradeSchool.SelectedIndex=comboGradeSchool.Items.IndexOf(ScreenGroupCur.GradeSchool);//"" etc OK
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ScreenGroupCur.PlaceService;
		}

		/*
		///<summary>This is never run.  It is only called when PrefName.PublicHealthScreeningUsePat is set to true.  The pref is set to 0 when added in convertdatabase and there is currently no UI to change it.  See note in Pref.cs pertaining to this.</summary>
		private void FillGridScreenPat() {
			ListScreenPats=ScreenPats.GetForScreenGroup(ScreenGroupCur.ScreenGroupNum);
			ListPats=Patients.GetPatsForScreenGroup(ScreenGroupCur.ScreenGroupNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"PatNum"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Name"),300);
			gridMain.Columns.Add(col);
			//todo: birthdate
			col=new ODGridColumn(Lan.g(this,"Age"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Race"),105);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Gender"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListPats.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(ListPats[i].PatNum.ToString());
				row.Cells.Add(ListPats[i].GetNameLF());
				row.Cells.Add(ListPats[i].Age.ToString());
				row.Cells.Add(PatientRaces.GetPatientRaceOldFromPatientRaces(ListPats[i].PatNum).ToString());
				row.Cells.Add(ListPats[i].Gender.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}*/

		private void FillGrid(){
			_arrayScreens=Screens.Refresh(ScreenGroupCur.ScreenGroupNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"#"),30);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Name"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Grade"),55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Age"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Race"),105);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Sex"),45);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Urgency"),70);
			gridMain.Columns.Add(col);
			if(!Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
				col=new ODGridColumn(Lan.g(this,"Caries"),45);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"ECC"),30);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"CarExp"),50);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"ExSeal"),45);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"NeedSeal"),60);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"NoTeeth"),55);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Comments"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_arrayScreens.Length;i++) {
				row=new ODGridRow();
				row.Cells.Add(_arrayScreens[i].ScreenGroupOrder.ToString());
				ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.ScreenPatNum==_arrayScreens[i].ScreenPatNum);
				row.Cells.Add((screenPat==null)?"Anonymous":Patients.GetPat(screenPat.PatNum).GetNameLF());
				row.Cells.Add(_arrayScreens[i].GradeLevel.ToString());
				row.Cells.Add(_arrayScreens[i].Age.ToString());
				row.Cells.Add(_arrayScreens[i].RaceOld.ToString());
				row.Cells.Add(_arrayScreens[i].Gender.ToString());
				row.Cells.Add(_arrayScreens[i].Urgency.ToString());
				if(!Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
					row.Cells.Add(getX(_arrayScreens[i].HasCaries));
					row.Cells.Add(getX(_arrayScreens[i].EarlyChildCaries));
					row.Cells.Add(getX(_arrayScreens[i].CariesExperience));
					row.Cells.Add(getX(_arrayScreens[i].ExistingSealants));
					row.Cells.Add(getX(_arrayScreens[i].NeedsSealants));
					row.Cells.Add(getX(_arrayScreens[i].MissingAllTeeth));
				}
				row.Cells.Add(_arrayScreens[i].Comments);
				gridMain.Rows.Add(row);
			}
			gridMain.Title=Lan.g(this,"Screenings")+" - "+_arrayScreens.Length;
			gridMain.EndUpdate();
		}

		private void FillScreenPats() {
			gridScreenPats.BeginUpdate();
			gridScreenPats.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Patient"),200);
			gridScreenPats.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Permission"),100);
			gridScreenPats.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Screened"),90);
			gridScreenPats.Columns.Add(col);
			gridScreenPats.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listScreenPats.Count;i++) {
				row=new ODGridRow();
				Patient pat=Patients.GetPat(_listScreenPats[i].PatNum);
				row.Cells.Add(pat.GetNameLF());
				row.Cells.Add(_listScreenPats[i].PatScreenPerm.ToString());
				OpenDentBusiness.Screen screen=_arrayScreens.FirstOrDefault(x => x.ScreenPatNum==_listScreenPats[i].ScreenPatNum);
				row.Cells.Add((screen==null)?"":"X");
				gridScreenPats.Rows.Add(row);
			}
			gridScreenPats.Title=Lan.g(this,"Patients for Screening")+" - "+_listScreenPats.Count;
			gridScreenPats.EndUpdate();
		}

		private void gridScreenPats_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			if(gridScreenPats.GetSelectedIndex()==-1) {
				return;
			}
			patContextMenu.Show(gridScreenPats,new Point(e.X,e.Y));
		}

		private void patContextMenuItem_Click(object sender,EventArgs e) {
			int idx=patContextMenu.MenuItems.IndexOf((MenuItem)sender);
			_listScreenPats[gridScreenPats.GetSelectedIndex()].PatScreenPerm=(PatScreenPerm)idx;
			FillScreenPats();
		}

		private string getX(YN ynValue){
			if(ynValue==YN.Yes)
				return "X";
			return "";
		}

		private void listMain_DoubleClick(object sender, System.EventArgs e) {
			/*if(PrefC.GetBool(PrefName.PublicHealthScreeningUsePat)) {
				FormScreenPatEdit FormSPE=new FormScreenPatEdit();
				FormSPE.ShowDialog();
				if(FormSPE.DialogResult!=DialogResult.OK) {
					return;
				}
				FillGridScreenPat();
			}
			else {*/
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenCur=_arrayScreens[gridMain.SelectedIndices[0]];
			FormSE.ScreenGroupCur=ScreenGroupCur;
			FormSE.ShowDialog();
			if(FormSE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void textScreenDate_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			try{
				DateTime.Parse(textScreenDate.Text);
			}
			catch{
				MessageBox.Show("Date invalid");
				e.Cancel=true;
			}
		}

		private void textProvName_TextChanged(object sender, System.EventArgs e) {
			/*if(textProvName.Text!=""){    //if a prov name was entered
				comboProv.SelectedIndex=-1;//then set the provnum to none.
			}*/
		}

		private void textProvName_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			comboProv.SelectedIndex=-1;//set the provnum to none.
		}

		private void comboProv_SelectedIndexChanged(object sender, System.EventArgs e) {
			if(comboProv.SelectedIndex!=-1){//if a prov was selected
				//set the provname accordingly
				textProvName.Text=ProviderC.ListShort[comboProv.SelectedIndex].LName+", "
					+ProviderC.ListShort[comboProv.SelectedIndex].FName;
			}
		}

		private void comboProv_SelectionChangeCommitted(object sender, System.EventArgs e) {
			
		}

		private void comboProv_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Back || e.KeyCode==Keys.Delete){
				comboProv.SelectedIndex=-1;
			}
		}

		private void comboCounty_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Back || e.KeyCode==Keys.Delete){
				comboCounty.SelectedIndex=-1;
			}
		}

		private void comboGradeSchool_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Back || e.KeyCode==Keys.Delete){
				comboGradeSchool.SelectedIndex=-1;
			}
		}

		private void butAddAnonymous_Click(object sender, System.EventArgs e) {
			/*if(PrefC.GetBool(PrefName.PublicHealthScreeningUsePat)) {
				FormScreenPatEdit FormSPE=new FormScreenPatEdit();
				while(true) {
					FormPatientSelect FormPS=new FormPatientSelect();
					FormPS.ShowDialog();
					if(FormPS.DialogResult!=DialogResult.OK) {
						return;
					}
					ScreenPat screenPat=new ScreenPat();
					screenPat.ScreenGroupNum=ScreenGroupCur.ScreenGroupNum;
					screenPat.SheetNum=PrefC.GetLong(PrefName.PublicHealthScreeningSheet);
					screenPat.PatNum=FormPS.SelectedPatNum;
					ScreenPats.Insert(screenPat);
					if(FormPS.DialogResult!=DialogResult.OK) {
						return;
					}
					FillGridScreenPat();
				}
			}
			else {*/
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=ScreenGroupCur;
			FormSE.IsNew=true;
			if(_arrayScreens.Length==0) {
				FormSE.ScreenCur=new OpenDentBusiness.Screen();
				FormSE.ScreenCur.ScreenGroupOrder=1;
			}
			else {
				FormSE.ScreenCur=_arrayScreens[_arrayScreens.Length-1];//'remembers' the last entry
				FormSE.ScreenCur.ScreenGroupOrder=FormSE.ScreenCur.ScreenGroupOrder+1;//increments for next
			}
			while(true) {
				//For Anonymous patients always default to unknowns.
				FormSE.ScreenCur.Gender=PatientGender.Unknown;
				FormSE.ScreenCur.RaceOld=PatientRaceOld.Unknown;
				FormSE.ShowDialog();
				if(FormSE.DialogResult!=DialogResult.OK) {
					return;
				}
				FormSE.ScreenCur.ScreenGroupOrder++;
				FillGrid();
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			/*if(PrefC.GetBool(PrefName.PublicHealthScreeningUsePat)){
				//never implemented.  Supposedly, a sheet would come up for creation/editing, based on the sheetdef.
				//But db fields couldn't even handle it yet.
				
				//FillGrid();
			}
			else{*/
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=ScreenGroupCur;
			FormSE.IsNew=false;
			FormSE.ScreenCur=_arrayScreens[e.Row];
			ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.ScreenPatNum==_arrayScreens[e.Row].ScreenPatNum);
			FormSE.ScreenPatCur=screenPat;//Null represents anonymous.
			FormSE.ShowDialog();
			FillGrid();
			FillScreenPats();
		}
		
		private void gridScreenPats_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Family fam=Patients.GetFamily(_listScreenPats[gridScreenPats.GetSelectedIndex()].PatNum);
			Patient pat=fam.GetPatient(_listScreenPats[gridScreenPats.GetSelectedIndex()].PatNum);
			FormPatientEdit FormPE=new FormPatientEdit(pat,fam);
			if(FormPE.ShowDialog()==DialogResult.OK) {//Information may have changed. Look for screens for this patient that may need changing.
				ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.PatNum==pat.PatNum);
				foreach(OpenDentBusiness.Screen screen in _arrayScreens){
					if(screen.ScreenPatNum==screenPat.ScreenPatNum) {//Found a screen belonging to the edited patient.
						//Don't unintelligently overwrite the screen's values.  They may have entered in patient information not pertaining to the screen such as address.
						screen.Birthdate=(pat.Birthdate==DateTime.MinValue)?screen.Birthdate:pat.Birthdate;//If birthdate isn't entered in pat select it will be mindate.
						screen.Age=(pat.Birthdate==DateTime.MinValue)?screen.Age:(byte)pat.Age;
						screen.Gender=pat.Gender;//Default value in pat edit is male. No way of knowing if it's intentional or not, just use it.
						screen.GradeLevel=(pat.GradeLevel==0)?screen.GradeLevel:pat.GradeLevel;//Default value is Unknown, use pat's grade if it's not unknown.
						Screens.Update(screen);
						FillGrid();
					}
				}
			}
		}

		private void butAddPat_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.ShowDialog();
			if(FormPS.DialogResult==DialogResult.OK){
				ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.PatNum==FormPS.SelectedPatNum);
				if(screenPat!=null) {
					MsgBox.Show(this,"Cannot add patient already in screen group.");
					for(int i=0;i<_listScreenPats.Count;i++) {
						if(_listScreenPats[i].ScreenPatNum==screenPat.ScreenPatNum) {
							gridScreenPats.SetSelected(i,true);
							break;
						}
					}
					return;
				}
				screenPat=new ScreenPat();
				screenPat.PatNum=FormPS.SelectedPatNum;
				screenPat.PatScreenPerm=PatScreenPerm.Unknown;
				screenPat.ScreenGroupNum=ScreenGroupCur.ScreenGroupNum;
				ScreenPats.Insert(screenPat);
				_listScreenPats.Add(screenPat);
				FillScreenPats();
			}
		}
		
		private void butRemovePat_Click(object sender,EventArgs e) {
			_listScreenPats.RemoveAt(gridScreenPats.GetSelectedIndex());
			FillScreenPats();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you want to delete this screening group? All screenings in this group will be deleted.")) {
				return;
			}
			ScreenGroups.Delete(ScreenGroupCur);//Also deletes screens.
			DialogResult=DialogResult.OK;
		}

		private void butStartScreens_Click(object sender,EventArgs e) {
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=ScreenGroupCur;
			FormSE.IsNew=true;
			int selectedIdx=gridScreenPats.GetSelectedIndex();
			int i=selectedIdx;
			if(i==-1){
				i=0;
			}
			while(true) {
				ScreenPat screenPat=_listScreenPats[i];
			//foreach(ScreenPat screenPat in _listScreenPats) {
				if(screenPat.PatScreenPerm!=PatScreenPerm.Allowed) {
					i=(i+1)%_listScreenPats.Count;//Causes the index to loop around when it gets to the end of the list so we can get to the beginning again.
					if(i==selectedIdx && selectedIdx!=-1) {
						break;
					}
					if(i==0 && selectedIdx==-1) {
						break;
					}
					continue;//Skip people who aren't allowed
				}
				if(_arrayScreens.FirstOrDefault(x => x.ScreenPatNum==screenPat.ScreenPatNum)!=null) {
					i=(i+1)%_listScreenPats.Count;//Causes the index to loop around when it gets to the end of the list so we can get to the beginning again.
					if(i==selectedIdx && selectedIdx!=-1) {
						break;
					}
					if(i==0 && selectedIdx==-1) {
						break;
					}
					continue;//If they already have a screen, don't make a new one.  We might think about opening up their old one for editing at this point.
				}
				FormSE.ScreenPatCur=screenPat;
				if(_arrayScreens.Length==0) {
					FormSE.ScreenCur=new OpenDentBusiness.Screen();
					FormSE.ScreenCur.ScreenGroupOrder=1;
				}
				else {
					FormSE.ScreenCur=_arrayScreens[_arrayScreens.Length-1];//'remembers' the last entry
					FormSE.ScreenCur.ScreenGroupOrder=FormSE.ScreenCur.ScreenGroupOrder+1;//increments for next
				}
				Patient pat=Patients.GetPat(screenPat.PatNum);//Get a patient so we can pre-fill some of the information (age/sex/birthdate/grade)
				FormSE.ScreenCur.Age=(pat.Birthdate==DateTime.MinValue)?FormSE.ScreenCur.Age:(byte)pat.Age;
				FormSE.ScreenCur.Birthdate=(pat.Birthdate==DateTime.MinValue)?FormSE.ScreenCur.Birthdate:pat.Birthdate;
				FormSE.ScreenCur.Gender=pat.Gender;//Default value in pat edit is male. No way of knowing if it's intentional or not, just use it.
				FormSE.ScreenCur.GradeLevel=(pat.GradeLevel==0)?FormSE.ScreenCur.GradeLevel:pat.GradeLevel;//Default value is Unknown, use pat's grade if it's not unknown.
				FormSE.ScreenCur.RaceOld=PatientRaceOld.Unknown;//Default to unknown. Patient Edit doesn't have the same type of race as screen edit.
				if(FormSE.ShowDialog()!=DialogResult.OK) {
					break;
				}
				FormSE.ScreenCur.ScreenGroupOrder++;
				FillGrid();
				i=(i+1)%_listScreenPats.Count;//Causes the index to loop around when it gets to the end of the list so we can get to the beginning again.
				if(i==selectedIdx && selectedIdx!=-1) {
					break;
				}
				if(i==0 && selectedIdx==-1) {
					break;
				}
			}
			FillScreenPats();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(_arrayScreens.Length==0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Since you have no screenings, the screener and location information cannot be saved.  Continue?")) {
					return;
				}
			}
			if(textDescription.Text=="") {
				MsgBox.Show(this,"Description field cannot be blank.");
				textDescription.Focus();
				return;
			}
			ScreenGroupCur.SGDate=PIn.Date(textScreenDate.Text);
			ScreenGroupCur.Description=textDescription.Text;
			ScreenGroupCur.ProvName=textProvName.Text;
			ScreenGroupCur.ProvNum=comboProv.SelectedIndex+1;//this works for -1 also.
			if(comboCounty.SelectedIndex==-1) {
				ScreenGroupCur.County="";
			}
			else {
				ScreenGroupCur.County=comboCounty.SelectedItem.ToString();
			}
			if(comboGradeSchool.SelectedIndex==-1) {
				ScreenGroupCur.GradeSchool="";
			}
			else {
				ScreenGroupCur.GradeSchool=comboGradeSchool.SelectedItem.ToString();
			}
			ScreenGroupCur.PlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			ScreenPats.Sync(_listScreenPats,ScreenGroupCur.ScreenGroupNum);
			ScreenGroups.Update(ScreenGroupCur);
			Screens.UpdateForGroup(ScreenGroupCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormScreenGroupEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(IsNew && DialogResult==DialogResult.Cancel) {
				ScreenGroups.Delete(ScreenGroupCur);//Also deletes screens.
			}
		}
		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		


		

		

		


	}
}





















