using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormScheduleDayEdit:ODForm {
		private OpenDental.UI.Button butAddTime;
		private OpenDental.UI.Button butCloseOffice;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		private DateTime _dateSched;
		private OpenDental.UI.ODGrid gridMain;
		private Label labelDate;
		private GroupBox groupBox3;
		private Label label1;
		private ListBox listProv;
		private OpenDental.UI.Button butOK;
		private Label label2;
		private GroupBox groupPractice;
		private OpenDental.UI.Button butNote;
		private OpenDental.UI.Button butHoliday;
		private OpenDental.UI.Button butProvNote;
		private ListBox listEmp;
		private ComboBox comboProv;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private GraphScheduleDay graphScheduleDay;
		///<summary>Working copy of schedule entries.</summary>
		private List<Schedule> _listScheds;
		///<summary>Stale copy of schedule entries.</summary>
		private List<Schedule> _listSchedsOld;
		private List<Provider> _listProvs;
		private List<Employee> _listEmps;
		private TabControl tabControl2;
		private TabPage tabPageProv;
		private TabPage tabPageEmp;
		///<summary>True if the checkbox for showing practice notes and holidays is checked.</summary>
		private bool _isPracticeNotes;
		private Label label3;
		private ComboBox comboClinic;
		private Label labelClinic;
		private List<Clinic> _listClinics;
		///<summary>Only used in schedule sorting. Greatly increases speed of large databases.</summary>
		private Dictionary<long,Employee> _dictEmpNumEmployee;
		///<summary>Only used in schedule sorting. Greatly increases speed of large databases.</summary>
		private Dictionary<long,Provider> _dictProvNumProvider;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;

		///<summary>True if the checkbox for showing clinic notes and holidays is checked.</summary>
		private bool _isClinicNotes;
		
		///<summary></summary>
		public FormScheduleDayEdit(DateTime dateSched) : this(dateSched,0,true,true) {
		}

		///<summary>When clinics are enabled, this will filter the employee list box by the clinic passed in.  Pass 0 to only show employees not assigned to a clinic.</summary>
		public FormScheduleDayEdit(DateTime dateSched,long clinicNum,bool isPracticeNotes,bool isClinicNotes) {
			InitializeComponent();
			_dateSched=dateSched;
			_selectedClinicNum=clinicNum;
			_isPracticeNotes=isPracticeNotes;
			_isClinicNotes=isClinicNotes;
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScheduleDayEdit));
			this.labelDate = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabPageProv = new System.Windows.Forms.TabPage();
			this.listProv = new System.Windows.Forms.ListBox();
			this.tabPageEmp = new System.Windows.Forms.TabPage();
			this.listEmp = new System.Windows.Forms.ListBox();
			this.butProvNote = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butAddTime = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.groupPractice = new System.Windows.Forms.GroupBox();
			this.butHoliday = new OpenDental.UI.Button();
			this.butNote = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.graphScheduleDay = new OpenDental.GraphScheduleDay();
			this.butOK = new OpenDental.UI.Button();
			this.butCloseOffice = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPageProv.SuspendLayout();
			this.tabPageEmp.SuspendLayout();
			this.groupPractice.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelDate
			// 
			this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDate.Location = new System.Drawing.Point(10, 12);
			this.labelDate.Name = "labelDate";
			this.labelDate.Size = new System.Drawing.Size(263, 23);
			this.labelDate.TabIndex = 9;
			this.labelDate.Text = "labelDate";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.tabControl2);
			this.groupBox3.Controls.Add(this.butProvNote);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.butAddTime);
			this.groupBox3.Location = new System.Drawing.Point(819, 36);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(179, 472);
			this.groupBox3.TabIndex = 12;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Add Time Block";
			// 
			// tabControl2
			// 
			this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl2.Controls.Add(this.tabPageProv);
			this.tabControl2.Controls.Add(this.tabPageEmp);
			this.tabControl2.Location = new System.Drawing.Point(5, 45);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(168, 391);
			this.tabControl2.TabIndex = 16;
			// 
			// tabPageProv
			// 
			this.tabPageProv.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageProv.Controls.Add(this.label3);
			this.tabPageProv.Controls.Add(this.listProv);
			this.tabPageProv.Controls.Add(this.comboProv);
			this.tabPageProv.Location = new System.Drawing.Point(4, 22);
			this.tabPageProv.Name = "tabPageProv";
			this.tabPageProv.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageProv.Size = new System.Drawing.Size(160, 365);
			this.tabPageProv.TabIndex = 0;
			this.tabPageProv.Text = "Providers (0)";
			// 
			// listProv
			// 
			this.listProv.FormattingEnabled = true;
			this.listProv.Location = new System.Drawing.Point(0, 0);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(160, 316);
			this.listProv.TabIndex = 6;
			this.listProv.SelectedIndexChanged += new System.EventHandler(this.listProv_SelectedIndexChanged);
			// 
			// tabPageEmp
			// 
			this.tabPageEmp.Controls.Add(this.listEmp);
			this.tabPageEmp.Location = new System.Drawing.Point(4, 22);
			this.tabPageEmp.Name = "tabPageEmp";
			this.tabPageEmp.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageEmp.Size = new System.Drawing.Size(160, 369);
			this.tabPageEmp.TabIndex = 1;
			this.tabPageEmp.Text = "Employees (0)";
			this.tabPageEmp.UseVisualStyleBackColor = true;
			// 
			// listEmp
			// 
			this.listEmp.FormattingEnabled = true;
			this.listEmp.Location = new System.Drawing.Point(0, 0);
			this.listEmp.Name = "listEmp";
			this.listEmp.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listEmp.Size = new System.Drawing.Size(160, 381);
			this.listEmp.TabIndex = 6;
			this.listEmp.SelectedIndexChanged += new System.EventHandler(this.listEmp_SelectedIndexChanged);
			// 
			// butProvNote
			// 
			this.butProvNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvNote.Autosize = true;
			this.butProvNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvNote.CornerRadius = 4F;
			this.butProvNote.Location = new System.Drawing.Point(93, 442);
			this.butProvNote.Name = "butProvNote";
			this.butProvNote.Size = new System.Drawing.Size(80, 24);
			this.butProvNote.TabIndex = 15;
			this.butProvNote.Text = "Note";
			this.butProvNote.Click += new System.EventHandler(this.butProvNote_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(2, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(170, 30);
			this.label1.TabIndex = 7;
			this.label1.Text = "Select One or More Providers or Employees";
			// 
			// butAddTime
			// 
			this.butAddTime.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddTime.Autosize = true;
			this.butAddTime.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTime.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTime.CornerRadius = 4F;
			this.butAddTime.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddTime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddTime.Location = new System.Drawing.Point(9, 442);
			this.butAddTime.Name = "butAddTime";
			this.butAddTime.Size = new System.Drawing.Size(80, 24);
			this.butAddTime.TabIndex = 4;
			this.butAddTime.Text = "&Add";
			this.butAddTime.Click += new System.EventHandler(this.butAddTime_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(12, 642);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(518, 44);
			this.label2.TabIndex = 14;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// groupPractice
			// 
			this.groupPractice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPractice.Controls.Add(this.butHoliday);
			this.groupPractice.Controls.Add(this.butNote);
			this.groupPractice.Location = new System.Drawing.Point(854, 551);
			this.groupPractice.Name = "groupPractice";
			this.groupPractice.Size = new System.Drawing.Size(110, 89);
			this.groupPractice.TabIndex = 15;
			this.groupPractice.TabStop = false;
			this.groupPractice.Text = "Practice";
			// 
			// butHoliday
			// 
			this.butHoliday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHoliday.Autosize = true;
			this.butHoliday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHoliday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHoliday.CornerRadius = 4F;
			this.butHoliday.Location = new System.Drawing.Point(14, 53);
			this.butHoliday.Name = "butHoliday";
			this.butHoliday.Size = new System.Drawing.Size(80, 24);
			this.butHoliday.TabIndex = 15;
			this.butHoliday.Text = "Holiday";
			this.butHoliday.Click += new System.EventHandler(this.butHoliday_Click);
			// 
			// butNote
			// 
			this.butNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNote.Autosize = true;
			this.butNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNote.CornerRadius = 4F;
			this.butNote.Location = new System.Drawing.Point(14, 20);
			this.butNote.Name = "butNote";
			this.butNote.Size = new System.Drawing.Size(80, 24);
			this.butNote.TabIndex = 14;
			this.butNote.Text = "Note";
			this.butNote.Click += new System.EventHandler(this.butNote_Click);
			// 
			// gridMain
			// 
			this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(3, 3);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(787, 575);
			this.gridMain.TabIndex = 8;
			this.gridMain.Title = "Edit Day";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// comboProv
			// 
			this.comboProv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(3, 342);
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(155, 21);
			this.comboProv.TabIndex = 16;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 32);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(801, 607);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.gridMain);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(793, 581);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "List";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.graphScheduleDay);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(793, 581);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Graph";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// graphScheduleDay
			// 
			this.graphScheduleDay.BarHeightPixels = 17;
			this.graphScheduleDay.BarSpacingPixels = 3;
			this.graphScheduleDay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphScheduleDay.EmployeeBarColor = System.Drawing.Color.LightSkyBlue;
			this.graphScheduleDay.EmployeeTextColor = System.Drawing.Color.Black;
			this.graphScheduleDay.EndHour = 19;
			this.graphScheduleDay.ExteriorPaddingPixels = 11;
			this.graphScheduleDay.GraphBackColor = System.Drawing.Color.White;
			this.graphScheduleDay.LineWidthPixels = 1;
			this.graphScheduleDay.Location = new System.Drawing.Point(3, 3);
			this.graphScheduleDay.Name = "graphScheduleDay";
			this.graphScheduleDay.PracticeBarColor = System.Drawing.Color.Salmon;
			this.graphScheduleDay.PracticeTextColor = System.Drawing.Color.Black;
			this.graphScheduleDay.ProviderBarColor = System.Drawing.Color.LightGreen;
			this.graphScheduleDay.ProviderTextColor = System.Drawing.Color.Black;
			this.graphScheduleDay.Size = new System.Drawing.Size(787, 575);
			this.graphScheduleDay.StartHour = 4;
			this.graphScheduleDay.TabIndex = 0;
			this.graphScheduleDay.TickHeightPixels = 5;
			this.graphScheduleDay.XAxisBackColor = System.Drawing.Color.White;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(819, 680);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 13;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCloseOffice
			// 
			this.butCloseOffice.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCloseOffice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCloseOffice.Autosize = true;
			this.butCloseOffice.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCloseOffice.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCloseOffice.CornerRadius = 4F;
			this.butCloseOffice.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butCloseOffice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCloseOffice.Location = new System.Drawing.Point(866, 521);
			this.butCloseOffice.Name = "butCloseOffice";
			this.butCloseOffice.Size = new System.Drawing.Size(80, 24);
			this.butCloseOffice.TabIndex = 5;
			this.butCloseOffice.Text = "Delete";
			this.butCloseOffice.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(906, 680);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(819, 9);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(179, 21);
			this.comboClinic.TabIndex = 37;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(774, 12);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(43, 13);
			this.labelClinic.TabIndex = 36;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 319);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(147, 21);
			this.label3.TabIndex = 17;
			this.label3.Text = "Default Prov for Unassigned*";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormScheduleDayEdit
			// 
			this.ClientSize = new System.Drawing.Size(1003, 720);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupPractice);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.labelDate);
			this.Controls.Add(this.butCloseOffice);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(966, 408);
			this.Name = "FormScheduleDayEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Day";
			this.Load += new System.EventHandler(this.FormScheduleDay_Load);
			this.groupBox3.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabPageProv.ResumeLayout(false);
			this.tabPageEmp.ResumeLayout(false);
			this.groupPractice.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormScheduleDay_Load(object sender,System.EventArgs e) {
			_listClinics=new List<Clinic>() { new Clinic() { Description=Lan.g(this,"Headquarters") } }; //Seed with "Headquarters"
			Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Description));
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetDesc(_selectedClinicNum); });
			if(!PrefC.HasClinicsEnabled) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			FillProvsAndEmps();
			//Fill Provider Override
			ProviderC.ListShort.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.SelectedIndex=_listProvs.FindIndex(x => x.ProvNum==PrefC.GetLong(PrefName.ScheduleProvUnassigned));
			labelDate.Text=_dateSched.ToString("dddd")+" "+_dateSched.ToShortDateString();
			_listScheds=Schedules.RefreshDayEditForPracticeProvsEmps(_dateSched,_listProvs.Select(x => x.ProvNum).ToList(),
				_listEmps.Select(x => x.EmployeeNum).ToList(),_isPracticeNotes,_isClinicNotes,_selectedClinicNum);//only does this on startup
			_listSchedsOld=_listScheds.Select(x => x.Copy()).ToList();
			FillGrid();
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			Text=Lan.g(this,"Edit Day")+" - "+_listClinics[comboClinic.SelectedIndex].Description;
			if(comboClinic.SelectedIndex<1) {
				groupPractice.Text=Lan.g(this,"Practice");
			}
			else {
				groupPractice.Text=Lan.g(this,"Clinic");
			}
			FillProvsAndEmps();
		}

		private void FillProvsAndEmps() {
			tabPageProv.Text=Lan.g(this,"Providers")+" (0)";
			tabPageEmp.Text=Lan.g(this,"Employees")+" (0)";
			if(PrefC.HasClinicsEnabled) {
				_listProvs=Providers.GetProvsForClinic(_selectedClinicNum);
				_listEmps=Employees.GetEmpsForClinic(_selectedClinicNum);
			}
			else {
				_listProvs=ProviderC.GetListShort();
				_listEmps=Employees.ListShort.ToList();
			}
			//Prov Listbox
			listProv.Items.Clear();
			listProv.ClearSelected();
			_listProvs.ForEach(x => listProv.Items.Add(x.Abbr));
			for(int i = 0;i<listProv.Items.Count;i++) {
				listProv.SetSelected(i,true);
			}
			//Emp Listbox
			listEmp.Items.Clear();
			listEmp.ClearSelected();
			_listEmps.ForEach(x => listEmp.Items.Add(x.FName));
			for(int i = 0;i<listEmp.Items.Count;i++) {
				listEmp.SetSelected(i,true);
			}
		}

		private void listProv_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageProv.Text=Lan.g(this,"Providers")+" ("+listProv.SelectedIndices.Count+")";
		}

		private void listEmp_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageEmp.Text=Lan.g(this,"Employees")+" ("+listEmp.SelectedIndices.Count+")";
		}

		private void FillGrid() {
			//do not refresh from db
			_dictEmpNumEmployee=_listScheds.Select(x => x.EmployeeNum).Distinct().Where(x => x>0).Select(x => Employees.GetEmp(x)).Where(x=>x!=null).ToDictionary(x => x.EmployeeNum,x => x);//speed up sort.
			_dictProvNumProvider=_listScheds.Select(x => x.ProvNum).Distinct().Where(x => x>0).Select(x => Providers.GetProv(x)).Where(x=>x!=null).ToDictionary(x => x.ProvNum,x => x);//speed up sort.
			_listScheds.Sort(CompareSchedule);
			graphScheduleDay.SetSchedules(_listScheds);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableSchedDay","Provider"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedDay","Employee"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedDay","Start Time"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedDay","Stop Time"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedDay","Ops"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedDay","Note"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			string note;
			string opdesc;
			//string opstr;
			//string[] oparray;
			foreach(Schedule schedCur in _listScheds) {
				row=new ODGridRow();
				//Prov
				if(schedCur.ProvNum==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(Providers.GetAbbr(schedCur.ProvNum));
				}
				//Employee
				if(schedCur.EmployeeNum==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(Employees.GetEmp(schedCur.EmployeeNum).FName);
				}
				//times
				if(schedCur.StartTime==TimeSpan.Zero && schedCur.StopTime==TimeSpan.Zero) {
					row.Cells.Add("");
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(schedCur.StartTime.ToShortTimeString());
					row.Cells.Add(schedCur.StopTime.ToShortTimeString());
				}
				//ops
				opdesc="";
				foreach(long opNumCur in schedCur.Ops) {
					Operatory opCur=Operatories.GetOperatory(opNumCur);
					if(opCur==null || opCur.IsHidden) {//Skip hidden operatories because it just confuses users.
						continue;
					}
					if(opdesc!="") {
						opdesc+=",";
					}
					opdesc+=opCur.Abbrev;
				}
				row.Cells.Add(opdesc);
				//note
				note="";
				if(schedCur.SchedType==ScheduleType.Practice) {//note or holiday
					if(schedCur.Status==SchedStatus.Holiday) {
						note+=Lan.g(this,"Holiday");
					}
					else {
						note+=Lan.g(this,"Note");
					}
					if(PrefC.HasClinicsEnabled && schedCur.SchedType==ScheduleType.Practice) {
						note+=(note==""?"":" ")+"("+(schedCur.ClinicNum>0?Clinics.GetDesc(schedCur.ClinicNum):"Headquarters")+")";
					}
				}
				note+=(note==""?"":": ")+schedCur.Note;
				row.Cells.Add(note);
				row.Tag=schedCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private int CompareSchedule(Schedule x,Schedule y){
			if(x==y){//also handles null==null
				return 0;
			}
			if(y==null){
				return 1;
			}
			if(x==null){
				return -1;
			}
			if(x.SchedType!=y.SchedType){
				return x.SchedType.CompareTo(y.SchedType);
			}
			if(x.ProvNum!=y.ProvNum){
				return _dictProvNumProvider[x.ProvNum].ItemOrder.CompareTo(_dictProvNumProvider[y.ProvNum].ItemOrder);
			}
			if(x.EmployeeNum!=y.EmployeeNum) {
				Employee empx= _dictEmpNumEmployee[x.EmployeeNum];//use dictionary to greatly speed up sort
				Employee empy= _dictEmpNumEmployee[y.EmployeeNum];//use dictionary to greatly speed up sort
				if(empx.FName!=empy.FName) {
					return empx.FName.CompareTo(empy.FName);
				}
				if(empx.LName!=empy.LName) {
					return empx.LName.CompareTo(empy.LName);
				}
				return x.EmployeeNum.CompareTo(y.EmployeeNum);
			}
			return x.StartTime.CompareTo(y.StartTime);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Schedule schedCur=(Schedule)gridMain.Rows[e.Row].Tag;//remember the clicked row
			FormScheduleEdit FormS=new FormScheduleEdit();
			FormS.ListScheds=_listScheds;
			FormS.SchedCur=schedCur;
			FormS.ClinicNum=_selectedClinicNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
			//this is safe and does nothing if the schedule is not found in the grid
			gridMain.SetSelected(gridMain.Rows.OfType<ODGridRow>().Select(x => (Schedule)x.Tag).ToList().IndexOf(schedCur),true);
		}

		//private void butAll_Click(object sender,EventArgs e) {
		//	for(int i=0;i<listProv.Items.Count;i++){
		//		listProv.SetSelected(i,true);
		//	}
		//}

		private void butAddTime_Click(object sender, System.EventArgs e) {
			if(listProv.SelectedIndices.Count==0 && listEmp.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one provider or one employee first.");
				return;
			}
			Schedule schedCur=new Schedule();
			schedCur.SchedDate=_dateSched;
			schedCur.Status=SchedStatus.Open;
			schedCur.StartTime=new TimeSpan(8,0,0);//8am
			schedCur.StopTime=new TimeSpan(17,0,0);//5pm
			//schedtype, provNum, and empnum will be set down below
			FormScheduleEdit FormS=new FormScheduleEdit();
			FormS.SchedCur=schedCur;
			FormS.ClinicNum=_selectedClinicNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			Schedule schedTemp;
			for(int i=0;i<listProv.SelectedIndices.Count;i++){
				schedTemp=new Schedule();
				schedTemp=schedCur.Copy();
				schedTemp.SchedType=ScheduleType.Provider;
				schedTemp.ProvNum=_listProvs[listProv.SelectedIndices[i]].ProvNum;
				_listScheds.Add(schedTemp);
			}
			for(int i=0;i<listEmp.SelectedIndices.Count;i++) {
				schedTemp=new Schedule();
				schedTemp=schedCur.Copy();
				schedTemp.SchedType=ScheduleType.Employee;
				schedTemp.EmployeeNum=_listEmps[listEmp.SelectedIndices[i]].EmployeeNum;
				_listScheds.Add(schedTemp);
			}
			FillGrid();
		}

		private void butProvNote_Click(object sender,EventArgs e) {
			if(listProv.SelectedIndices.Count==0 && listEmp.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one provider or one employee first.");
				return;
			}
			Schedule schedCur=new Schedule();
			schedCur.SchedDate=_dateSched;
			schedCur.Status=SchedStatus.Open;
			//schedtype, provNum, and empnum will be set down below
			FormScheduleEdit FormS=new FormScheduleEdit();
			FormS.SchedCur=schedCur;
			FormS.ClinicNum=_selectedClinicNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			Schedule schedTemp;
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				schedTemp=new Schedule();
				schedTemp=schedCur.Copy();
				schedTemp.SchedType=ScheduleType.Provider;
				schedTemp.ProvNum=_listProvs[listProv.SelectedIndices[i]].ProvNum;
				_listScheds.Add(schedTemp);
			}
			for(int i=0;i<listEmp.SelectedIndices.Count;i++) {
				schedTemp=new Schedule();
				schedTemp=schedCur.Copy();
				schedTemp.SchedType=ScheduleType.Employee;
				schedTemp.EmployeeNum=_listEmps[listEmp.SelectedIndices[i]].EmployeeNum;
				_listScheds.Add(schedTemp);
			}
			FillGrid();
		}

		private void butNote_Click(object sender,EventArgs e) {
			Schedule SchedCur=new Schedule();
			SchedCur.SchedDate=_dateSched;
			SchedCur.Status=SchedStatus.Open;
			SchedCur.SchedType=ScheduleType.Practice;
			FormScheduleEdit FormS=new FormScheduleEdit();
			FormS.SchedCur=SchedCur;
			FormS.ClinicNum=_selectedClinicNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			_listScheds.Add(SchedCur);
			FillGrid();
		}

		private void butHoliday_Click(object sender,System.EventArgs e) {
		  Schedule SchedCur=new Schedule();
      SchedCur.SchedDate=_dateSched;
      SchedCur.Status=SchedStatus.Holiday;
			SchedCur.SchedType=ScheduleType.Practice;
			SchedCur.ClinicNum=_selectedClinicNum;
		  FormScheduleEdit FormS=new FormScheduleEdit();
			FormS.ListScheds=_listScheds;
			FormS.SchedCur=SchedCur;
			FormS.ClinicNum=_selectedClinicNum;
      FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			_listScheds.Add(SchedCur);
      FillGrid();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				gridMain.SetSelected(true);
				if(!MsgBox.Show(this,true,"Are you sure you want to delete the entire schedule for this day?")) {
					gridMain.SetSelected(false);//So that they don't accidentally hit Delete again and it wipe out the entire day without warning.
					return;
				}
				_listScheds.Clear();
				FillGrid();
				return;
			}
			gridMain.SelectedIndices.OfType<int>().ToList().ForEach(x => _listScheds.Remove((Schedule)gridMain.Rows[x].Tag));
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			try {
				Schedules.SetForDay(_listScheds,_listSchedsOld);
			}
			catch(Exception ex) {
				MsgBox.Show(this,ex.Message);
				return;
			}
			if(comboProv.SelectedIndex!=-1
				&& Prefs.UpdateLong(PrefName.ScheduleProvUnassigned,ProviderC.ListShort[comboProv.SelectedIndex].ProvNum))//Must use provider cache here, not _listProvs.
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}







