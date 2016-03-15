using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormSchedule:ODForm {
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butRefresh;
		private ValidDate textDateTo;
		private Label label2;
		private ValidDate textDateFrom;
		private Label label1;
		private ListBox listProv;
		private CheckBox checkWeekend;
		private GroupBox groupCopy;
		private OpenDental.UI.Button butCopyDay;
		private TextBox textClipboard;
		private Label label3;
		private OpenDental.UI.Button butRepeat;
		private Label label4;
		private TextBox textRepeat;
		private OpenDental.UI.Button butPaste;
		private OpenDental.UI.Button butCopyWeek;
		private OpenDental.UI.Button butPrint;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private DateTime DateCopyStart;
		private OpenDental.UI.Button butDelete;
		private DateTime DateCopyEnd;
		private CheckBox checkPracticeNotes;
		private ListBox listEmp;
		private CheckBox checkReplace;
		private GroupBox groupPaste;
		///<summary>This tracks whether the provList or empList has been click on since the last refresh.  Forces user to refresh before deleting or pasting so that the list exactly matches the grid.</summary>
		private bool _provsChanged;
		private PrintDocument pd;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		private Label labelClinic;
		private ComboBox comboClinic;
		bool changed;
		private List<Clinic> _listClinics;
		private List<Provider> _listProvs;
		private TabControl tabControl1;
		private TabPage tabPageProv;
		private TabPage tabPageEmp;
		private CheckBox checkClinicNotes;
		private List<Employee> _listEmps;
		private DataTable _tableScheds;
		private bool _isResizing;

		///<summary></summary>
		public FormSchedule()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSchedule));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.listProv = new System.Windows.Forms.ListBox();
			this.checkWeekend = new System.Windows.Forms.CheckBox();
			this.groupCopy = new System.Windows.Forms.GroupBox();
			this.butCopyWeek = new OpenDental.UI.Button();
			this.butCopyDay = new OpenDental.UI.Button();
			this.textClipboard = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textRepeat = new System.Windows.Forms.TextBox();
			this.checkPracticeNotes = new System.Windows.Forms.CheckBox();
			this.listEmp = new System.Windows.Forms.ListBox();
			this.checkReplace = new System.Windows.Forms.CheckBox();
			this.groupPaste = new System.Windows.Forms.GroupBox();
			this.butRepeat = new OpenDental.UI.Button();
			this.butPaste = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageProv = new System.Windows.Forms.TabPage();
			this.tabPageEmp = new System.Windows.Forms.TabPage();
			this.checkClinicNotes = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butRefresh = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupCopy.SuspendLayout();
			this.groupPaste.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPageProv.SuspendLayout();
			this.tabPageEmp.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(100, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 15);
			this.label2.TabIndex = 9;
			this.label2.Text = "To Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 15);
			this.label1.TabIndex = 7;
			this.label1.Text = "From Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(0, 6);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(191, 277);
			this.listProv.TabIndex = 23;
			this.listProv.Click += new System.EventHandler(this.listProv_Click);
			this.listProv.SelectedIndexChanged += new System.EventHandler(this.listProv_SelectedIndexChanged);
			// 
			// checkWeekend
			// 
			this.checkWeekend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWeekend.Location = new System.Drawing.Point(30, 435);
			this.checkWeekend.Name = "checkWeekend";
			this.checkWeekend.Size = new System.Drawing.Size(143, 17);
			this.checkWeekend.TabIndex = 24;
			this.checkWeekend.Text = "Show Weekends";
			this.checkWeekend.UseVisualStyleBackColor = true;
			this.checkWeekend.Click += new System.EventHandler(this.checkWeekend_Click);
			// 
			// groupCopy
			// 
			this.groupCopy.Controls.Add(this.butCopyWeek);
			this.groupCopy.Controls.Add(this.butCopyDay);
			this.groupCopy.Controls.Add(this.textClipboard);
			this.groupCopy.Controls.Add(this.label3);
			this.groupCopy.Location = new System.Drawing.Point(23, 484);
			this.groupCopy.Name = "groupCopy";
			this.groupCopy.Size = new System.Drawing.Size(158, 111);
			this.groupCopy.TabIndex = 25;
			this.groupCopy.TabStop = false;
			this.groupCopy.Text = "Copy";
			// 
			// butCopyWeek
			// 
			this.butCopyWeek.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyWeek.Autosize = true;
			this.butCopyWeek.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyWeek.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyWeek.CornerRadius = 4F;
			this.butCopyWeek.Location = new System.Drawing.Point(6, 83);
			this.butCopyWeek.Name = "butCopyWeek";
			this.butCopyWeek.Size = new System.Drawing.Size(75, 24);
			this.butCopyWeek.TabIndex = 28;
			this.butCopyWeek.Text = "Copy Week";
			this.butCopyWeek.Click += new System.EventHandler(this.butCopyWeek_Click);
			// 
			// butCopyDay
			// 
			this.butCopyDay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyDay.Autosize = true;
			this.butCopyDay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyDay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyDay.CornerRadius = 4F;
			this.butCopyDay.Location = new System.Drawing.Point(6, 56);
			this.butCopyDay.Name = "butCopyDay";
			this.butCopyDay.Size = new System.Drawing.Size(75, 24);
			this.butCopyDay.TabIndex = 27;
			this.butCopyDay.Text = "Copy Day";
			this.butCopyDay.Click += new System.EventHandler(this.butCopyDay_Click);
			// 
			// textClipboard
			// 
			this.textClipboard.Location = new System.Drawing.Point(6, 33);
			this.textClipboard.Name = "textClipboard";
			this.textClipboard.ReadOnly = true;
			this.textClipboard.Size = new System.Drawing.Size(146, 20);
			this.textClipboard.TabIndex = 26;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 14);
			this.label3.TabIndex = 8;
			this.label3.Text = "Clipboard Contents";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(70, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 14);
			this.label4.TabIndex = 32;
			this.label4.Text = "#";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textRepeat
			// 
			this.textRepeat.Location = new System.Drawing.Point(110, 62);
			this.textRepeat.Name = "textRepeat";
			this.textRepeat.Size = new System.Drawing.Size(39, 20);
			this.textRepeat.TabIndex = 31;
			this.textRepeat.Text = "1";
			// 
			// checkPracticeNotes
			// 
			this.checkPracticeNotes.Checked = true;
			this.checkPracticeNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkPracticeNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPracticeNotes.Location = new System.Drawing.Point(12, 74);
			this.checkPracticeNotes.Name = "checkPracticeNotes";
			this.checkPracticeNotes.Size = new System.Drawing.Size(189, 17);
			this.checkPracticeNotes.TabIndex = 28;
			this.checkPracticeNotes.Text = "Show Practice Holidays and Notes";
			this.checkPracticeNotes.UseVisualStyleBackColor = true;
			this.checkPracticeNotes.Click += new System.EventHandler(this.checkPracticeNotes_Click);
			// 
			// listEmp
			// 
			this.listEmp.Location = new System.Drawing.Point(0, 45);
			this.listEmp.Name = "listEmp";
			this.listEmp.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listEmp.Size = new System.Drawing.Size(191, 238);
			this.listEmp.TabIndex = 30;
			this.listEmp.Click += new System.EventHandler(this.listEmp_Click);
			this.listEmp.SelectedIndexChanged += new System.EventHandler(this.listEmp_SelectedIndexChanged);
			// 
			// checkReplace
			// 
			this.checkReplace.Checked = true;
			this.checkReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkReplace.Location = new System.Drawing.Point(6, 14);
			this.checkReplace.Name = "checkReplace";
			this.checkReplace.Size = new System.Drawing.Size(146, 18);
			this.checkReplace.TabIndex = 31;
			this.checkReplace.Text = "Replace Existing";
			this.checkReplace.UseVisualStyleBackColor = true;
			// 
			// groupPaste
			// 
			this.groupPaste.Controls.Add(this.butRepeat);
			this.groupPaste.Controls.Add(this.label4);
			this.groupPaste.Controls.Add(this.checkReplace);
			this.groupPaste.Controls.Add(this.textRepeat);
			this.groupPaste.Controls.Add(this.butPaste);
			this.groupPaste.Location = new System.Drawing.Point(23, 596);
			this.groupPaste.Name = "groupPaste";
			this.groupPaste.Size = new System.Drawing.Size(158, 87);
			this.groupPaste.TabIndex = 32;
			this.groupPaste.TabStop = false;
			this.groupPaste.Text = "Paste";
			// 
			// butRepeat
			// 
			this.butRepeat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRepeat.Autosize = true;
			this.butRepeat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRepeat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRepeat.CornerRadius = 4F;
			this.butRepeat.Location = new System.Drawing.Point(6, 59);
			this.butRepeat.Name = "butRepeat";
			this.butRepeat.Size = new System.Drawing.Size(75, 24);
			this.butRepeat.TabIndex = 30;
			this.butRepeat.Text = "Repeat";
			this.butRepeat.Click += new System.EventHandler(this.butRepeat_Click);
			// 
			// butPaste
			// 
			this.butPaste.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPaste.Autosize = true;
			this.butPaste.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPaste.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPaste.CornerRadius = 4F;
			this.butPaste.Location = new System.Drawing.Point(6, 32);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(75, 24);
			this.butPaste.TabIndex = 29;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(9, 5);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(58, 13);
			this.labelClinic.TabIndex = 34;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(8, 20);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(170, 21);
			this.comboClinic.TabIndex = 35;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageProv);
			this.tabControl1.Controls.Add(this.tabPageEmp);
			this.tabControl1.Location = new System.Drawing.Point(1, 117);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(200, 312);
			this.tabControl1.TabIndex = 36;
			// 
			// tabPageProv
			// 
			this.tabPageProv.Controls.Add(this.listProv);
			this.tabPageProv.Location = new System.Drawing.Point(4, 22);
			this.tabPageProv.Name = "tabPageProv";
			this.tabPageProv.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageProv.Size = new System.Drawing.Size(192, 286);
			this.tabPageProv.TabIndex = 0;
			this.tabPageProv.Text = "Providers (0)";
			this.tabPageProv.UseVisualStyleBackColor = true;
			// 
			// tabPageEmp
			// 
			this.tabPageEmp.Controls.Add(this.listEmp);
			this.tabPageEmp.Controls.Add(this.comboClinic);
			this.tabPageEmp.Controls.Add(this.labelClinic);
			this.tabPageEmp.Location = new System.Drawing.Point(4, 22);
			this.tabPageEmp.Name = "tabPageEmp";
			this.tabPageEmp.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageEmp.Size = new System.Drawing.Size(192, 286);
			this.tabPageEmp.TabIndex = 1;
			this.tabPageEmp.Text = "Employees (0)";
			this.tabPageEmp.UseVisualStyleBackColor = true;
			// 
			// checkClinicNotes
			// 
			this.checkClinicNotes.Checked = true;
			this.checkClinicNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkClinicNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClinicNotes.Location = new System.Drawing.Point(12, 94);
			this.checkClinicNotes.Name = "checkClinicNotes";
			this.checkClinicNotes.Size = new System.Drawing.Size(189, 17);
			this.checkClinicNotes.TabIndex = 37;
			this.checkClinicNotes.Text = "Show Clinic Holidays and Notes";
			this.checkClinicNotes.UseVisualStyleBackColor = true;
			this.checkClinicNotes.Click += new System.EventHandler(this.checkClinicNotes_Click);
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
			this.butDelete.Location = new System.Drawing.Point(29, 454);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(103, 24);
			this.butDelete.TabIndex = 27;
			this.butDelete.Text = "Clear Week";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(551, 666);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(90, 24);
			this.butPrint.TabIndex = 26;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(102, 54);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(85, 20);
			this.textDateTo.TabIndex = 10;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(12, 54);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(85, 20);
			this.textDateFrom.TabIndex = 8;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(57, 8);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 11;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(207, 8);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(761, 652);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Schedule";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormSchedule
			// 
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.checkClinicNotes);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupCopy);
			this.Controls.Add(this.groupPaste);
			this.Controls.Add(this.checkPracticeNotes);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.checkWeekend);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(647, 727);
			this.Name = "FormSchedule";
			this.ShowInTaskbar = false;
			this.Text = "Schedule";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSchedule_FormClosing);
			this.Load += new System.EventHandler(this.FormSchedule_Load);
			this.ResizeBegin += new System.EventHandler(this.FormSchedule_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.FormSchedule_ResizeEnd);
			this.Resize += new System.EventHandler(this.FormSchedule_Resize);
			this.groupCopy.ResumeLayout(false);
			this.groupCopy.PerformLayout();
			this.groupPaste.ResumeLayout(false);
			this.groupPaste.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPageProv.ResumeLayout(false);
			this.tabPageEmp.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormSchedule_Load(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules,DateTime.MinValue,true)) {
				//gridMain.Enabled=false;
				butDelete.Enabled=false;
				groupCopy.Enabled=false;
				groupPaste.Enabled=false;
				if(PrefC.GetBool(PrefName.DistributorKey)) {//if this is OD HQ
					checkPracticeNotes.Checked=false;
					checkPracticeNotes.Enabled=false;
				}
			}
			DateTime dateFrom=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
			textDateFrom.Text=dateFrom.ToShortDateString();
			textDateTo.Text=dateFrom.AddMonths(12).AddDays(-1).ToShortDateString();
			if(PrefC.HasClinicsEnabled) {
				//Get available clinics and then filter the employee list box by those clinics.
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				comboClinic.Items.Clear();
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					comboClinic.SelectedIndex=0;
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=comboClinic.Items.Add(_listClinics[i].Description);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=curIndex;
					}
				}
			}
			else {//no clinics
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				checkClinicNotes.Visible=false;
				checkClinicNotes.Checked=false;
			}
			FillEmployeesAndProviders();
			FillGrid();
		}

		///<summary>Fills the employee box based on what clinic is selected.  Set selectAll to true to have all employees in the list box selected by default.</summary>
		private void FillEmployeesAndProviders() {
			tabPageEmp.Text=Lan.g(this,"Employees")+" (0)";
			//tabPageProv.Text=Lan.g(this,"Providers")+" (0)";
			if(PrefC.HasClinicsEnabled) {
				long clinicNum=0;
				if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, because Headquarters is the first option.
				}
				//clinicNum will be 0 for unrestricted users with HQ selected in which case this will get only emps/provs not assigned to a clinic
				_listEmps=Employees.GetEmpsForClinic(clinicNum);
				//_listProvs=Providers.GetProvsForClinic(clinicNum);//Do NOT filter providers by clinic yet.  This will be implemented in phase 2.
				_listProvs=ProviderC.GetListShort();
			}
			else {//Not using clinics
				_listEmps=Employees.GetListShort();
				_listProvs=ProviderC.GetListShort();
			}
			listEmp.Items.Clear();//clearing the items from the list does not trigger the selected index changed event, reset manually
			foreach(Employee empCur in _listEmps) {
				listEmp.Items.Add(empCur.FName);
				listEmp.SetSelected(listEmp.Items.Count-1,true);//select the item just added
			}
			//if the list has already been filled, no need to refill since we are not filtering by clinic, prevents an unnecessary flicker
			//listProv.Items.Clear();//add this line when we start to filter provs by clinic and remove the if list is empty check
			if(listProv.Items.Count==0) {
				foreach(Provider provCur in _listProvs) {
					listProv.Items.Add(provCur.Abbr);
					listProv.SetSelected(listProv.Items.Count-1,true);//select the item just added
				}
			}
		}

		private void FillGrid(bool isFromDb=true){
			DateCopyStart=DateTime.MinValue;
			DateCopyEnd=DateTime.MinValue;
			textClipboard.Text="";
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="")
			{
				//MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			//Clear out the columns and rows for dynamic resizing of the grid to calculate column widths
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			gridMain.EndUpdate();
			_provsChanged=false;
			List<long> provNums=new List<long>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++){
				provNums.Add(_listProvs[listProv.SelectedIndices[i]].ProvNum);
			}
			List<long> empNums=new List<long>();
			for(int i=0;i<listEmp.SelectedIndices.Count;i++){
				empNums.Add(_listEmps[listEmp.SelectedIndices[i]].EmployeeNum);
			}
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, because Headquarters is the first option.
				}
			}
			if(isFromDb || this._tableScheds==null) {
				_tableScheds=Schedules.GetPeriod(PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text),provNums,empNums,checkPracticeNotes.Checked,
					checkClinicNotes.Checked,clinicNum);
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			int colW;
			if(checkWeekend.Checked){
				colW=(gridMain.Width-20)/7;
			}
			else{
				colW=(gridMain.Width-20)/5;
			}
			ODGridColumn col;
			if(checkWeekend.Checked){
				col=new ODGridColumn(Lan.g("TableSchedule","Sunday"),colW);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableSchedule","Monday"),colW);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedule","Tuesday"),colW);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedule","Wednesday"),colW);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedule","Thursday"),colW);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSchedule","Friday"),colW);
			gridMain.Columns.Add(col);
			if(checkWeekend.Checked){
				col=new ODGridColumn(Lan.g("TableSchedule","Saturday"),colW);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableScheds.Rows.Count;i++){
				row=new ODGridRow();
				if(checkWeekend.Checked){
					row.Cells.Add(_tableScheds.Rows[i][0].ToString());
				}
				row.Cells.Add(_tableScheds.Rows[i][1].ToString());
				row.Cells.Add(_tableScheds.Rows[i][2].ToString());
				row.Cells.Add(_tableScheds.Rows[i][3].ToString());
				row.Cells.Add(_tableScheds.Rows[i][4].ToString());
				row.Cells.Add(_tableScheds.Rows[i][5].ToString());
				if(checkWeekend.Checked){
					row.Cells.Add(_tableScheds.Rows[i][6].ToString());
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			//Set today red
			if(!checkWeekend.Checked && (DateTime.Today.DayOfWeek==DayOfWeek.Sunday || DateTime.Today.DayOfWeek==DayOfWeek.Saturday)){
				return;
			}
			if(DateTime.Today>PIn.Date(textDateTo.Text) || DateTime.Today<PIn.Date(textDateFrom.Text)){
				return;
			}
			int colI=(int)DateTime.Today.DayOfWeek;
			if(!checkWeekend.Checked){
				colI--;
			}
			gridMain.Rows[Schedules.GetRowCal(PIn.Date(textDateFrom.Text),DateTime.Today)].Cells[colI].ColorText=Color.Red;
		}

		private void listProv_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageProv.Text=Lan.g(this,"Providers")+" ("+listProv.SelectedIndices.Count+")";
		}

		private void listEmp_SelectedIndexChanged(object sender,EventArgs e) {
			tabPageEmp.Text=Lan.g(this,"Employees")+" ("+listEmp.SelectedIndices.Count+")";
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			//tabPageProv.Text=Lan.g(this,"Providers")+" (0)";
			tabPageEmp.Text=Lan.g(this,"Employees")+" (0)";
			comboClinic.Text=Lan.g(this,"Show Practice Notes");
			if(!Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndex>0) {
				comboClinic.Text=Lan.g(this,"Show Practice and Clinic Notes");
			}
			FillEmployeesAndProviders();
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			FillGrid();
		}

		private void checkWeekend_Click(object sender,EventArgs e) {
			FillGrid(false);
		}

		private void checkPracticeNotes_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkClinicNotes_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules,DateTime.MinValue)) {
				return;
			}
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			int clickedCol=e.Col;
			if(!checkWeekend.Checked){
				clickedCol++;
			}
			//the "clickedCell" is in terms of the entire 7 col layout.
			Point clickedCell=new Point(clickedCol,e.Row);
			DateTime selectedDate=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),e.Row,clickedCol);
			if(selectedDate<PIn.Date(textDateFrom.Text) || selectedDate>PIn.Date(textDateTo.Text)){
				return;
			}
			//MessageBox.Show(selectedDate.ToShortDateString());
			long clinicNum=0; 
			if(Security.CurUser.ClinicIsRestricted) {
				clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			else if(comboClinic.SelectedIndex > 0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, because Headquarters is the first option.
			}
			FormScheduleDayEdit FormS=new FormScheduleDayEdit(selectedDate,clinicNum,checkPracticeNotes.Checked,checkClinicNotes.Checked);
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			FillGrid();
			if(checkWeekend.Checked){
				gridMain.SetSelected(clickedCell);
			}
			else{
				gridMain.SetSelected(new Point(clickedCell.X-1,clickedCell.Y));
			}
			changed=true;
		}

		private void listProv_Click(object sender,EventArgs e) {
			_provsChanged=true;
		}

		private void listEmp_Click(object sender,EventArgs e) {
			_provsChanged=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(_provsChanged) {
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			if(!MsgBox.Show(this,true,"Delete all displayed entries for the entire selected week?")){
				return;
			}
			int startI=1;
			if(checkWeekend.Checked) {
				startI=0;
			}
			DateTime dateSelectedStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,startI);
			DateTime dateSelectedEnd;
			if(checkWeekend.Checked) {
				dateSelectedEnd=dateSelectedStart.AddDays(6);
			}
			else {
				dateSelectedEnd=dateSelectedStart.AddDays(4);
			}
			List<long> provNums=new List<long>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				provNums.Add(_listProvs[listProv.SelectedIndices[i]].ProvNum);
			}
			List<long> empNums=new List<long>();
			for(int i=0;i<listEmp.SelectedIndices.Count;i++) {
				empNums.Add(_listEmps[listEmp.SelectedIndices[i]].EmployeeNum);
			}
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, because Headquarters is the first option.
				}
			}
			Schedules.Clear(dateSelectedStart,dateSelectedEnd,provNums,empNums,checkPracticeNotes.Checked,checkClinicNotes.Checked,clinicNum);
			FillGrid();
			changed=true;
		}

		private void butCopyDay_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") {
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			if(gridMain.SelectedCell.X==-1){
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			int selectedCol=gridMain.SelectedCell.X;
			if(!checkWeekend.Checked) {
				selectedCol++;
			}
			DateCopyStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,selectedCol);
			DateCopyEnd=DateCopyStart;
			textClipboard.Text=DateCopyStart.ToShortDateString();
		}

		private void butCopyWeek_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			int startI=1;
			if(checkWeekend.Checked){
				startI=0;
			}
			DateCopyStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,startI);
			if(checkWeekend.Checked){
				DateCopyEnd=DateCopyStart.AddDays(6);
			}
			else{
				DateCopyEnd=DateCopyStart.AddDays(4);
			}
			textClipboard.Text=DateCopyStart.ToShortDateString()+"-"+DateCopyEnd.ToShortDateString();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(DateCopyStart.Year<1880){
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			if(_provsChanged){
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			bool isWeek=DateCopyStart!=DateCopyEnd;
			if(isWeek){
				int startI=1;
				if(checkWeekend.Checked) {
					startI=0;
				}
				dateSelectedStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,startI);
				if(checkWeekend.Checked) {
					dateSelectedEnd=dateSelectedStart.AddDays(6);
				}
				else {
					dateSelectedEnd=dateSelectedStart.AddDays(4);
				}
			}
			else{
				int selectedCol=gridMain.SelectedCell.X;
				if(!checkWeekend.Checked) {
					selectedCol++;
				}
				dateSelectedStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,selectedCol);
				dateSelectedEnd=dateSelectedStart;
			}
			//it's not allowed to paste back over the same day or week.
			if(dateSelectedStart==DateCopyStart){
				MsgBox.Show(this,"Not allowed to paste back onto the same date as is on the clipboard.");
				return;
			}
			List<long> provNums=listProv.SelectedIndices.OfType<int>().Select(x => _listProvs[x].ProvNum).ToList();
			List<long> empNums=listEmp.SelectedIndices.OfType<int>().Select(x => _listEmps[x].EmployeeNum).ToList();
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, HQ is the first option.
				}
			}
			if(checkReplace.Checked){
				Schedules.Clear(dateSelectedStart,dateSelectedEnd,provNums,empNums,checkPracticeNotes.Checked,checkClinicNotes.Checked,clinicNum);
			}
			List<Schedule> SchedList=Schedules.RefreshPeriod(DateCopyStart,DateCopyEnd,provNums,empNums,checkPracticeNotes.Checked,
				checkClinicNotes.Checked,clinicNum);
			Schedule sched;
			int weekDelta=0;
			if(isWeek){
				TimeSpan span=dateSelectedStart-DateCopyStart;
				weekDelta=span.Days/7;//usually a positive # representing a future paste, but can be negative
			}
			for(int i=0;i<SchedList.Count;i++){
				sched=SchedList[i];
				if(isWeek){
					sched.SchedDate=sched.SchedDate.AddDays(weekDelta*7);
				}
				else{
					sched.SchedDate=dateSelectedStart;
				}
				Schedules.Insert(sched,false);
			}
			DateTime rememberDateStart=DateCopyStart;
			DateTime rememberDateEnd=DateCopyEnd;
			FillGrid();
			DateCopyStart=rememberDateStart;
			DateCopyEnd=rememberDateEnd;
			if(isWeek){
				textClipboard.Text=DateCopyStart.ToShortDateString()+"-"+DateCopyEnd.ToShortDateString();
			}
			else{
				textClipboard.Text=DateCopyStart.ToShortDateString();
			}
			changed=true;
		}

		private void butRepeat_Click(object sender,EventArgs e) {
			bool isWeek=false;
			if(DateCopyStart!=DateCopyEnd) {
				isWeek=true;
			}
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			int repeatCount;
			try{
				repeatCount=PIn.Int(textRepeat.Text);
			}
			catch{
				MsgBox.Show(this,"Please fix number box first.");
				return;
			}
			if(repeatCount>1250 && !isWeek) {
				MsgBox.Show(this,"Please enter a number of days less than 1250.");
				return;
			}
			if(repeatCount>250 && isWeek) {
				MsgBox.Show(this,"Please enter a number of weeks less than 250.");
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select a date first.");
				return;
			}
			if(DateCopyStart.Year<1880) {
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			if(_provsChanged) {
				MsgBox.Show(this,"Provider or Employee selection has been changed.  Please refresh first.");
				return;
			}
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			if(isWeek) {
				int startI=1;
				if(checkWeekend.Checked) {
					startI=0;
				}
				dateSelectedStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,startI);
				if(checkWeekend.Checked) {
					dateSelectedEnd=dateSelectedStart.AddDays(6);
				}
				else {
					dateSelectedEnd=dateSelectedStart.AddDays(4);
				}
			}
			else {
				int selectedCol=gridMain.SelectedCell.X;
				if(!checkWeekend.Checked) {
					selectedCol++;
				}
				dateSelectedStart=Schedules.GetDateCal(PIn.Date(textDateFrom.Text),gridMain.SelectedCell.Y,selectedCol);
				dateSelectedEnd=dateSelectedStart;
			}
			//it is allowed to paste back over the same day or week.
			//if(dateSelectedStart==DateCopyStart) {
			//	MsgBox.Show(this,"Not allowed to paste back onto the same date as is on the clipboard.");
			//	return;
			//}
			List<long> provNums=new List<long>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				provNums.Add(_listProvs[listProv.SelectedIndices[i]].ProvNum);
			}
			List<long> empNums=new List<long>();
			for(int i=0;i<listEmp.SelectedIndices.Count;i++) {
				empNums.Add(_listEmps[listEmp.SelectedIndices[i]].EmployeeNum);
			}
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;//Subtract 1, because Headquarters is the first option.
				}
			}
			List<Schedule> SchedList=Schedules.RefreshPeriod(DateCopyStart,DateCopyEnd,provNums,empNums,checkPracticeNotes.Checked,
				checkClinicNotes.Checked,clinicNum);
			Schedule sched;
			int weekDelta=0;
			TimeSpan span;
			if(isWeek) {
				span=dateSelectedStart-DateCopyStart;
				weekDelta=span.Days/7;//usually a positive # representing a future paste, but can be negative
			}
			int dayDelta=0;//this is needed when repeat pasting days in order to calculate skipping weekends.
			//dayDelta will start out zero and increment separately from r.
			for(int r=0;r<repeatCount;r++){//for example, user wants to repeat 3 times.
				if(checkReplace.Checked) {
					if(isWeek){
						Schedules.Clear(dateSelectedStart.AddDays(r*7),dateSelectedEnd.AddDays(r*7),provNums,empNums,checkPracticeNotes.Checked,
							checkClinicNotes.Checked,clinicNum);
					}
					else{
						Schedules.Clear(dateSelectedStart.AddDays(dayDelta),dateSelectedEnd.AddDays(dayDelta),provNums,empNums,checkPracticeNotes.Checked,
							checkClinicNotes.Checked,clinicNum);
					}
				}
				for(int i=0;i<SchedList.Count;i++) {//For example, if 3 weeks for one provider, then about 30 loops.
					sched=SchedList[i].Copy();
					if(isWeek) {
						sched.SchedDate=sched.SchedDate.AddDays((weekDelta+r)*7).AddHours(1).Date;
					}
					else {
						sched.SchedDate=dateSelectedStart.AddDays(dayDelta);
					}
					Schedules.Insert(sched,false);//if there is one provider in 4 ops, then this does 5 inserts per loop.
				}
				if(!checkWeekend.Checked && dateSelectedStart.AddDays(dayDelta).DayOfWeek==DayOfWeek.Friday){
					dayDelta+=3;
				}
				else{
					dayDelta++;
				}
			}
			DateTime rememberDateStart=DateCopyStart;
			DateTime rememberDateEnd=DateCopyEnd;
			FillGrid();
			DateCopyStart=rememberDateStart;
			DateCopyEnd=rememberDateEnd;
			if(isWeek) {
				textClipboard.Text=DateCopyStart.ToShortDateString()+"-"+DateCopyEnd.ToShortDateString();
			}
			else {
				textClipboard.Text=DateCopyStart.ToShortDateString();
			}
			changed=true;
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
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
				if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Staff schedule printed")) {
					pd.Print();
				}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
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
				text=Lan.g(this,"Schedule");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				//yPos+=(int)g.MeasureString(text,headingFont).Height;
				//text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				//g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=25;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		///<summary>Fires as resizing is happening.</summary>
		private void FormSchedule_Resize(object sender,EventArgs e) {
			if(_isResizing || WindowState==FormWindowState.Minimized) {
				return;
			}
			FillGrid(false);
		}

		///<summary>Fires when manual resizing begins.</summary>
		private void FormSchedule_ResizeBegin(object sender,EventArgs e) {
			_isResizing=true;
		}

		///<summary>Fires when resizing is complete, except when changing window state. I.e. this is not fired when the window is maximized or minimized.</summary>
		private void FormSchedule_ResizeEnd(object sender,EventArgs e) {
			FillGrid(false);
			_isResizing=false;
		}

		private void FormSchedule_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed){
				SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
			}
		}

	}
}





















