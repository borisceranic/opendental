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
	public class FormScreenGroupEdit:ODForm {
		private IContainer components;
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
		private ScreenGroup _screenGroup;
		private UI.Button butCancel;
		private UI.Button butDelete;
		private UI.ODGrid gridMain;
		private List<OpenDentBusiness.Screen> _listScreens;
		private ODGrid gridScreenPats;
		private List<ScreenPat> _listScreenPats;
		private UI.Button button1;
		private UI.Button butRemovePat;
		private UI.Button butStartScreens;
		private Label label4;
		private ContextMenu patContextMenu;

		///<summary></summary>
		public FormScreenGroupEdit(ScreenGroup screenGroup) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_screenGroup=screenGroup;
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
			this.textScreenDate.TabIndex = 6;
			this.textScreenDate.Validating += new System.ComponentModel.CancelEventHandler(this.textScreenDate_Validating);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(102, 29);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(173, 20);
			this.textDescription.TabIndex = 0;
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
			this.comboPlaceService.TabIndex = 5;
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
			this.comboCounty.TabIndex = 3;
			this.comboCounty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboCounty_KeyDown);
			// 
			// comboGradeSchool
			// 
			this.comboGradeSchool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGradeSchool.Location = new System.Drawing.Point(102, 111);
			this.comboGradeSchool.Name = "comboGradeSchool";
			this.comboGradeSchool.Size = new System.Drawing.Size(173, 21);
			this.comboGradeSchool.TabIndex = 4;
			this.comboGradeSchool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboGradeSchool_KeyDown);
			// 
			// textProvName
			// 
			this.textProvName.Location = new System.Drawing.Point(102, 49);
			this.textProvName.Name = "textProvName";
			this.textProvName.Size = new System.Drawing.Size(173, 20);
			this.textProvName.TabIndex = 1;
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
			this.butStartScreens.TabIndex = 9;
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
			this.butRemovePat.TabIndex = 8;
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
			this.button1.TabIndex = 7;
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
			this.butAdd.TabIndex = 10;
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
			this.butDelete.TabIndex = 13;
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
			this.butCancel.TabIndex = 12;
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
			this.butOK.TabIndex = 11;
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
			if(_screenGroup.IsNew) {
				ScreenGroups.Insert(_screenGroup);
			}
			_listScreenPats=ScreenPats.GetForScreenGroup(_screenGroup.ScreenGroupNum);
			FillGrid();
			FillScreenPats();
			patContextMenu=new ContextMenu();
			string[] patScreenPerms=Enum.GetNames(typeof(PatScreenPerm));
			for(int i=0;i<patScreenPerms.Length;i++) {
				patContextMenu.MenuItems.Add(new MenuItem(patScreenPerms[i],patContextMenuItem_Click));
			}
			textScreenDate.Text=_screenGroup.SGDate.ToShortDateString();
			textDescription.Text=_screenGroup.Description;
			textProvName.Text=_screenGroup.ProvName;//has to be filled before provnum
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				comboProv.Items.Add(ProviderC.ListShort[i].Abbr);
				if(_screenGroup.ProvNum==ProviderC.ListShort[i].ProvNum) {
					comboProv.SelectedIndex=i;
				}
			}
			string[] CountiesListNames=Counties.GetListNames();
			comboCounty.Items.AddRange(CountiesListNames);
			if(_screenGroup.County==null) {
				_screenGroup.County="";//prevents the next line from crashing
			}
			comboCounty.SelectedIndex=comboCounty.Items.IndexOf(_screenGroup.County);//"" etc OK
			for(int i=0;i<SiteC.List.Length;i++) {
				comboGradeSchool.Items.Add(SiteC.List[i].Description);
			}
			if(_screenGroup.GradeSchool==null) {
				_screenGroup.GradeSchool="";//prevents the next line from crashing
			}
			comboGradeSchool.SelectedIndex=comboGradeSchool.Items.IndexOf(_screenGroup.GradeSchool);//"" etc OK
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)_screenGroup.PlaceService;
		}

		private void FillGrid() {
			_listScreens=Screens.GetScreensForGroup(_screenGroup.ScreenGroupNum);
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
			if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				col=new ODGridColumn(Lan.g(this,"Caries"),45,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"ECC"),30,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"CarExp"),50,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"ExSeal"),45,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"NeedSeal"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"NoTeeth"),55,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Comments"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(OpenDentBusiness.Screen screen in _listScreens) {
				row=new ODGridRow();
				row.Cells.Add(screen.ScreenGroupOrder.ToString());
				ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.ScreenPatNum==screen.ScreenPatNum);
				row.Cells.Add((screenPat==null)?"Anonymous":Patients.GetLim(screenPat.PatNum).GetNameLF());
				row.Cells.Add(screen.GradeLevel.ToString());
				row.Cells.Add(screen.Age.ToString());
				row.Cells.Add(screen.RaceOld.ToString());
				row.Cells.Add(screen.Gender.ToString());
				row.Cells.Add(screen.Urgency.ToString());
				if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
					row.Cells.Add(screen.HasCaries==YN.Yes ? "X":"");
					row.Cells.Add(screen.EarlyChildCaries==YN.Yes ? "X":"");
					row.Cells.Add(screen.CariesExperience==YN.Yes ? "X":"");
					row.Cells.Add(screen.ExistingSealants==YN.Yes ? "X":"");
					row.Cells.Add(screen.NeedsSealants==YN.Yes ? "X":"");
					row.Cells.Add(screen.MissingAllTeeth==YN.Yes ? "X":"");
				}
				row.Cells.Add(screen.Comments);
				gridMain.Rows.Add(row);
			}
			gridMain.Title=Lan.g(this,"Screenings")+" - "+_listScreens.Count;
			gridMain.EndUpdate();
		}

		private void FillScreenPats() {
			gridScreenPats.BeginUpdate();
			gridScreenPats.Title=Lan.g(this,"Patients for Screening")+" - "+_listScreenPats.Count;
			gridScreenPats.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Patient"),200);
			gridScreenPats.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Permission"),100);
			gridScreenPats.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Screened"),90,HorizontalAlignment.Center);
			gridScreenPats.Columns.Add(col);
			gridScreenPats.Rows.Clear();
			ODGridRow row;
			foreach(ScreenPat screenPat in _listScreenPats) {
				row=new ODGridRow();
				Patient pat=Patients.GetLim(screenPat.PatNum);
				row.Cells.Add(pat.GetNameLF());
				row.Cells.Add(screenPat.PatScreenPerm.ToString());
				OpenDentBusiness.Screen screen=_listScreens.FirstOrDefault(x => x.ScreenPatNum==screenPat.ScreenPatNum);
				row.Cells.Add((screen==null)?"":"X");
				gridScreenPats.Rows.Add(row);
			}
			gridScreenPats.EndUpdate();
		}

		///<summary></summary>
		private void AddAnonymousScreens() {
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=_screenGroup;
			FormSE.IsNew=true;
			if(_listScreens.Count==0) {
				FormSE.ScreenCur=new OpenDentBusiness.Screen();
				FormSE.ScreenCur.ScreenGroupOrder=1;
			}
			else {
				FormSE.ScreenCur=_listScreens[_listScreens.Count-1];//'remembers' the last entry
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

		///<summary></summary>
		private void AddAnonymousScreensForSheets() {
			//Get the first custom Screening sheet or use the internal one
			SheetDef sheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.Screening);
			Sheet sheet=SheetUtil.CreateSheet(sheetDef);
			sheet.IsNew=true;
			SheetParameter.SetParameter(sheet,"ScreenGroupNum",_screenGroup.ScreenGroupNum);
			SheetFiller.FillFields(sheet);
			using(Graphics g=CreateGraphics()) {
				SheetUtil.CalculateHeights(sheet,g);
			}
			//Create a valid screen so that we can create a screening sheet with the corresponding ScreenNum.
			OpenDentBusiness.Screen screen=new OpenDentBusiness.Screen();
			screen.ScreenGroupNum=_screenGroup.ScreenGroupNum;
			screen.ScreenGroupOrder=1;
			if(_listScreens.Count!=0) {
				screen.ScreenGroupOrder=_listScreens.Last().ScreenGroupOrder+1;//increments for next
			}
			while(true) {
				//For Anonymous patients always default to unknowns.
				foreach(SheetField field in sheet.SheetFields) {
					switch(field.FieldName) {
						case "Race/Ethnicity":
							field.FieldValue="Unknown"+field.FieldValue;
							break;
						case "Gender":
							field.FieldValue="Unknown"+field.FieldValue;
							break;
					}
				}
				FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheet);
				FormSFE.ShowDialog();
				if(FormSFE.DialogResult!=DialogResult.OK) {
					return;
				}
				Screens.CreateScreenFromSheet(sheet,screen);
				screen.ScreenGroupOrder++;
				FillGrid();
			}
		}

		private void StartScreensForPats() {
			if(_listScreenPats.Count==0) {
				MsgBox.Show(this,"No patients for screening.");
				return;
			}
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=_screenGroup;
			FormSE.IsNew=true;
			int selectedIdx=gridScreenPats.GetSelectedIndex();
			int i=selectedIdx;
			if(i==-1) {
				i=0;
			}
			while(true) {
				ScreenPat screenPat=_listScreenPats[i];
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
				if(_listScreens.FirstOrDefault(x => x.ScreenPatNum==screenPat.ScreenPatNum)!=null) {
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
				if(_listScreens.Count==0) {
					FormSE.ScreenCur=new OpenDentBusiness.Screen();
					FormSE.ScreenCur.ScreenGroupOrder=1;
				}
				else {
					FormSE.ScreenCur=_listScreens[_listScreens.Count-1];//'remembers' the last entry
					FormSE.ScreenCur.ScreenGroupOrder=FormSE.ScreenCur.ScreenGroupOrder+1;//increments for next
				}
				Patient pat=Patients.GetPat(screenPat.PatNum);//Get a patient so we can pre-fill some of the information (age/sex/birthdate/grade)
				FormSE.ScreenCur.Age=(pat.Birthdate==DateTime.MinValue) ? FormSE.ScreenCur.Age : (byte)pat.Age;
				FormSE.ScreenCur.Birthdate=(pat.Birthdate==DateTime.MinValue) ? FormSE.ScreenCur.Birthdate : pat.Birthdate;
				FormSE.ScreenCur.Gender=pat.Gender;//Default value in pat edit is male. No way of knowing if it's intentional or not, just use it.
				FormSE.ScreenCur.GradeLevel=(pat.GradeLevel==0) ? FormSE.ScreenCur.GradeLevel : pat.GradeLevel;//Default value is Unknown, use pat's grade if it's not unknown.
				FormSE.ScreenCur.RaceOld=PatientRaceOld.Unknown;//Default to unknown. Patient Edit doesn't have the same type of race as screen edit.
				FormSE.ScreenCur.Urgency=pat.Urgency;
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

		private void StartScreensForPatsWithSheets() {
			//Get the first custom Screening sheet or use the internal one
			SheetDef sheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.Screening);
			int selectedIdx=gridScreenPats.GetSelectedIndex();
			int i=selectedIdx;
			if(i==-1) {
				i=0;
			}
			while(true) {
				ScreenPat screenPat=_listScreenPats[i];
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
				if(_listScreens.FirstOrDefault(x => x.ScreenPatNum==screenPat.ScreenPatNum)!=null) {
					i=(i+1)%_listScreenPats.Count;//Causes the index to loop around when it gets to the end of the list so we can get to the beginning again.
					if(i==selectedIdx && selectedIdx!=-1) {
						break;
					}
					if(i==0 && selectedIdx==-1) {
						break;
					}
					continue;//If they already have a screen, don't make a new one.  We might think about opening up their old one for editing at this point.
				}
				Sheet sheet=SheetUtil.CreateSheet(sheetDef);
				sheet.IsNew=true;
				sheet.PatNum=screenPat.PatNum;
				SheetParameter.SetParameter(sheet,"ScreenGroupNum",_screenGroup.ScreenGroupNum);//I think we may need this.
				SheetParameter.SetParameter(sheet,"PatNum",screenPat.PatNum);
				SheetFiller.FillFields(sheet);
				using(Graphics g=CreateGraphics()) {
					SheetUtil.CalculateHeights(sheet,g);
				}
				//Create a valid screen so that we can create a screening sheet with the corresponding ScreenNum.
				OpenDentBusiness.Screen screen=new OpenDentBusiness.Screen();
				screen.ScreenPatNum=screenPat.ScreenPatNum;
				screen.ScreenGroupNum=_screenGroup.ScreenGroupNum;
				screen.ScreenGroupOrder=1;
				if(_listScreens.Count!=0) {
					screen.ScreenGroupOrder=_listScreens.Last().ScreenGroupOrder+1;//increments for next
				}
				FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheet);
				FormSFE.ShowDialog();
				if(FormSFE.DialogResult!=DialogResult.OK) {
					break;
				}
				if(FormSFE.SheetCur!=null) {//It wasn't deleted, create a screen.
					Screens.CreateScreenFromSheet(sheet,screen);
					//Now try and process the screening chart for treatment planned sealants.
					foreach(SheetField field in sheet.SheetFields) {
						if(field.FieldName=="ChartSealantTreatment") {
							if(ProcedureCodes.GetCodeNum("D1351")==0) {
								MsgBox.Show(this,"The required sealant code is not present in the database.  The screening chart will not be processed.");
								break;
							}
							Screens.ProcessScreenChart(sheet,ScreenChartType.TP);
							break;
						}
					}
				}
				screen.ScreenGroupOrder++;
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

		private void ViewScreenForPat(OpenDentBusiness.Screen screenCur) {
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenGroupCur=_screenGroup;
			FormSE.IsNew=false;
			FormSE.ScreenCur=screenCur;
			ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.ScreenPatNum==screenCur.ScreenPatNum);
			FormSE.ScreenPatCur=screenPat;//Null represents anonymous.
			FormSE.ShowDialog();
		}

		private void ViewScreenForPatWithSheets(OpenDentBusiness.Screen screenCur) {
			Sheet sheet=Sheets.GetSheet(screenCur.SheetNum);
			if(sheet==null) {
				MsgBox.Show(this,"Sheet no longer exists.  It may have been deleted from the Chart Module.");
				return;
			}
			FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheet);
			if(FormSFE.ShowDialog()==DialogResult.OK) {
				//Now try and process the screening chart for completed sealants.
				if(FormSFE.SheetCur==null) {
					return;
				}
				foreach(SheetField field in sheet.SheetFields) {
					if(field.FieldName=="ChartSealantComplete") {
						if(ProcedureCodes.GetCodeNum("D1351")==0) {
							MsgBox.Show(this,"The required sealant code is not present in the database.  The screening chart will not be processed.");
							break;
						}
						Screens.ProcessScreenChart(sheet,ScreenChartType.C);
						break;
					}
				}
			}
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

		private void listMain_DoubleClick(object sender, System.EventArgs e) {
			FormScreenEdit FormSE=new FormScreenEdit();
			FormSE.ScreenCur=_listScreens[gridMain.SelectedIndices[0]];
			FormSE.ScreenGroupCur=_screenGroup;
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

		private void textProvName_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			comboProv.SelectedIndex=-1;//set the provnum to none.
		}

		private void comboProv_SelectedIndexChanged(object sender, System.EventArgs e) {
			if(comboProv.SelectedIndex!=-1) {//if a prov was selected
				//set the provname accordingly
				textProvName.Text=ProviderC.ListShort[comboProv.SelectedIndex].LName+", "
					+ProviderC.ListShort[comboProv.SelectedIndex].FName;
			}
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

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(PrefC.GetBool(PrefName.ScreeningsUseSheets)) {
				ViewScreenForPatWithSheets(_listScreens[e.Row]);
			}
			else {
				ViewScreenForPat(_listScreens[e.Row]);
			}
			FillGrid();
			FillScreenPats();
		}
		
		private void gridScreenPats_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Family fam=Patients.GetFamily(_listScreenPats[gridScreenPats.GetSelectedIndex()].PatNum);
			Patient pat=fam.GetPatient(_listScreenPats[gridScreenPats.GetSelectedIndex()].PatNum);
			FormPatientEdit FormPE=new FormPatientEdit(pat,fam);
			if(FormPE.ShowDialog()==DialogResult.OK) {//Information may have changed. Look for screens for this patient that may need changing.
				ScreenPat screenPat=_listScreenPats.FirstOrDefault(x => x.PatNum==pat.PatNum);
				foreach(OpenDentBusiness.Screen screen in _listScreens){
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

		private void butAddAnonymous_Click(object sender,System.EventArgs e) {
			if(PrefC.GetBool(PrefName.ScreeningsUseSheets)) {
				AddAnonymousScreensForSheets();
			}
			else {
				AddAnonymousScreens();
			}
		}

		private void butAddPat_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.ShowDialog();
			if(FormPS.DialogResult==DialogResult.OK) {
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
				screenPat.ScreenGroupNum=_screenGroup.ScreenGroupNum;
				ScreenPats.Insert(screenPat);
				_listScreenPats.Add(screenPat);
				FillScreenPats();
			}
		}
		
		private void butRemovePat_Click(object sender,EventArgs e) {
			if(gridScreenPats.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a patient to remove first.");
				return;
			}
			_listScreenPats.RemoveAt(gridScreenPats.GetSelectedIndex());
			FillScreenPats();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you want to delete this screening group? All screenings in this group will be deleted.")) {
				return;
			}
			ScreenGroups.Delete(_screenGroup);//Also deletes screens.
			DialogResult=DialogResult.OK;
		}

		private void butStartScreens_Click(object sender,EventArgs e) {
			if(_listScreenPats.Count==0) {
				MsgBox.Show(this,"No patients to screen.");
				return;
			}
			if(PrefC.GetBool(PrefName.ScreeningsUseSheets)) {
				StartScreensForPatsWithSheets();
			}
			else {
				StartScreensForPats();
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(textDescription.Text=="") {
				MsgBox.Show(this,"Description field cannot be blank.");
				textDescription.Focus();
				return;
			}
			_screenGroup.SGDate=PIn.Date(textScreenDate.Text);
			_screenGroup.Description=textDescription.Text;
			_screenGroup.ProvName=textProvName.Text;
			_screenGroup.ProvNum=comboProv.SelectedIndex+1;//this works for -1 also.
			if(comboCounty.SelectedIndex==-1) {
				_screenGroup.County="";
			}
			else {
				_screenGroup.County=comboCounty.SelectedItem.ToString();
			}
			if(comboGradeSchool.SelectedIndex==-1) {
				_screenGroup.GradeSchool="";
			}
			else {
				_screenGroup.GradeSchool=comboGradeSchool.SelectedItem.ToString();
			}
			_screenGroup.PlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			ScreenPats.Sync(_listScreenPats,_screenGroup.ScreenGroupNum);
			ScreenGroups.Update(_screenGroup);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormScreenGroupEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(_screenGroup.IsNew && DialogResult==DialogResult.Cancel) {
				ScreenGroups.Delete(_screenGroup);//Also deletes screens.
			}
		}
		

		

		

		

		

		

		

		

		

		

		

		

		

		

		

		


		

		

		


	}
}





















