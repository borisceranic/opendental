using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Data;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormRpProcNotBilledIns : ODForm {
		private System.Windows.Forms.MonthCalendar calendarTo;
		private System.Windows.Forms.MonthCalendar calendarFrom;
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butPrint;
		private IContainer components;
		private CheckBox checkMedical;
		private Label label1;
		private UI.Button butDropFrom;
		private ValidDate textDateFrom;
		private UI.Button butDropTo;
		private Label label2;
		private ValidDate textDateTo;
		private Label labelClinic;
		private UI.ComboBoxMulti comboBoxMultiClinics;
		private UI.ODGrid gridMain;
		private List<Clinic> _listClinics;
		private ReportComplex _myReport;
		private decimal _procTotalAmt;
		private DateTime _myReportDateFrom;
		private UI.Button butNewClaims;
		private DateTime _myReportDateTo;
		private const int _colWidthPatName=200;
		private const int _colWidthProcDate=110;
		private const int _colWidthAmount=90;
		private const int _colWidthClinic=75;
		private CheckBox checkAutoGroupProcs;
		private DateTime _dateFromPrev=DateTime.MinValue;
		private UI.Button butRefresh;
		private DateTime _dateToPrev=DateTime.MinValue;

		///<summary></summary>
		public FormRpProcNotBilledIns(){
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpProcNotBilledIns));
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.butClose = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.checkMedical = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butDropFrom = new OpenDental.UI.Button();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butDropTo = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textDateTo = new OpenDental.ValidDate();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butNewClaims = new OpenDental.UI.Button();
			this.checkAutoGroupProcs = new System.Windows.Forms.CheckBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(251, 50);
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 2;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(25, 50);
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 1;
			this.calendarFrom.Visible = false;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(872, 662);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 4;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(25, 662);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkMedical
			// 
			this.checkMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.Location = new System.Drawing.Point(654, 8);
			this.checkMedical.Name = "checkMedical";
			this.checkMedical.Size = new System.Drawing.Size(227, 21);
			this.checkMedical.TabIndex = 11;
			this.checkMedical.Text = "Include Medical Procedures";
			this.checkMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.UseVisualStyleBackColor = true;
			this.checkMedical.Visible = false;
			this.checkMedical.Click += new System.EventHandler(this.checkMedical_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(47, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 61;
			this.label1.Text = "From";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDropFrom
			// 
			this.butDropFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropFrom.Autosize = true;
			this.butDropFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropFrom.CornerRadius = 4F;
			this.butDropFrom.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butDropFrom.Location = new System.Drawing.Point(177, 28);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(22, 18);
			this.butDropFrom.TabIndex = 63;
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(98, 27);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(102, 20);
			this.textDateFrom.TabIndex = 62;
			this.textDateFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textDateFrom_KeyUp);
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butDropTo.Location = new System.Drawing.Point(404, 27);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(22, 18);
			this.butDropTo.TabIndex = 66;
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(279, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 16);
			this.label2.TabIndex = 64;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(325, 26);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(102, 20);
			this.textDateTo.TabIndex = 65;
			this.textDateTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textDateTo_KeyUp);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(437, 28);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(55, 16);
			this.labelClinic.TabIndex = 68;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.DroppedDown = false;
			this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(493, 26);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 67;
			this.comboBoxMultiClinics.Visible = false;
			this.comboBoxMultiClinics.Leave += new System.EventHandler(this.comboBoxMultiClinics_SelectionChangeCommitted);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(25, 50);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(922, 609);
			this.gridMain.TabIndex = 69;
			this.gridMain.Title = "Procedures Not Billed";
			this.gridMain.TranslationName = null;
			this.gridMain.Resize += new System.EventHandler(this.gridMain_Resize);
			// 
			// butNewClaims
			// 
			this.butNewClaims.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewClaims.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butNewClaims.Autosize = true;
			this.butNewClaims.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNewClaims.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNewClaims.CornerRadius = 4F;
			this.butNewClaims.Location = new System.Drawing.Point(437, 662);
			this.butNewClaims.Name = "butNewClaims";
			this.butNewClaims.Size = new System.Drawing.Size(100, 26);
			this.butNewClaims.TabIndex = 71;
			this.butNewClaims.Text = "New Claims";
			this.butNewClaims.UseVisualStyleBackColor = true;
			this.butNewClaims.Click += new System.EventHandler(this.butNewClaims_Click);
			// 
			// checkAutoGroupProcs
			// 
			this.checkAutoGroupProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.Location = new System.Drawing.Point(654, 28);
			this.checkAutoGroupProcs.Name = "checkAutoGroupProcs";
			this.checkAutoGroupProcs.Size = new System.Drawing.Size(227, 21);
			this.checkAutoGroupProcs.TabIndex = 72;
			this.checkAutoGroupProcs.Text = "Automatically Group Procedures";
			this.checkAutoGroupProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.UseVisualStyleBackColor = true;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(896, 21);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(50, 26);
			this.butRefresh.TabIndex = 73;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// FormRpProcNotBilledIns
			// 
			this.AcceptButton = this.butPrint;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.checkAutoGroupProcs);
			this.Controls.Add(this.butNewClaims);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboBoxMultiClinics);
			this.Controls.Add(this.butDropTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDropFrom);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.checkMedical);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.calendarTo);
			this.Controls.Add(this.calendarFrom);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormRpProcNotBilledIns";
			this.ShowInTaskbar = false;
			this.Text = "Procedures Not Billed to Insurance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRpProcNotBilledIns_FormClosing);
			this.Load += new System.EventHandler(this.FormProcNotAttach_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void gridMain_Resize(object sender,EventArgs e) {
			CreateOrResizeGridColumns();
		}

		private void FormProcNotAttach_Load(object sender, System.EventArgs e) {
			calendarFrom.SelectionStart=DateTime.Today;
			calendarTo.SelectionStart=DateTime.Today;
			textDateFrom.Text=DateTime.Today.ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
			if(PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				checkMedical.Visible=true;
			}
			if(PrefC.HasClinicsEnabled) {
				comboBoxMultiClinics.Visible=true;
				labelClinic.Visible=true;
				FillClinics();
			}
			if(PrefC.GetBool(PrefName.ClaimProcsNotBilledToInsAutoGroup)) {
				checkAutoGroupProcs.Checked=true;
			}
			FillGrid();
		}

		private void CreateOrResizeGridColumns() {
			gridMain.BeginUpdate();
			int colWidthVariable=gridMain.Width-_colWidthPatName-_colWidthProcDate-_colWidthAmount-10;//10 for scrollbar
			if(PrefC.HasClinicsEnabled) {
				colWidthVariable-=_colWidthClinic;
			}
			ODGridColumn col=null;
			if(gridMain.Columns.Count==0) {
				col=new ODGridColumn(Lan.g(this,"Patient Name"),_colWidthPatName);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Date"),_colWidthProcDate);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Descipion"),colWidthVariable);
				gridMain.Columns.Add(col);
				if(PrefC.HasClinicsEnabled) {
					col=new ODGridColumn(Lan.g(this,"Clinic"),_colWidthClinic);
					gridMain.Columns.Add(col);
				}
				col=new ODGridColumn(Lan.g(this,"Amount"),_colWidthAmount,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			else {
				gridMain.Columns[2].ColWidth=colWidthVariable;//Procedure Description
			}
			gridMain.EndUpdate();
		}
		
		private void FillGrid() {
			RefreshReport();			
			CreateOrResizeGridColumns();
			gridMain.BeginUpdate();
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_myReport.ReportObjects.Count;i++) {
				if(_myReport.ReportObjects[i].ObjectType!=ReportObjectType.QueryObject) {
					continue;
				}
				QueryObject queryObj=(QueryObject)_myReport.ReportObjects[i];
				for(int j=0;j<queryObj.ReportTable.Rows.Count;j++) {
					row=new ODGridRow();
					row.Cells.Add(queryObj.ReportTable.Rows[j][0].ToString());//Procedure Name
					row.Cells.Add(PIn.Date(queryObj.ReportTable.Rows[j][1].ToString()).ToShortDateString());//Procedure Date
					row.Cells.Add(queryObj.ReportTable.Rows[j][2].ToString());//Procedure Description
					if(PrefC.HasClinicsEnabled) {
						long clinicNum=PIn.Long(queryObj.ReportTable.Rows[j][5].ToString());
						if(clinicNum==0) {
							row.Cells.Add("Unassigned");
						}
						else {
							row.Cells.Add(Clinics.GetDesc(clinicNum));
						}
					}
					row.Cells.Add(PIn.Double(queryObj.ReportTable.Rows[j][3].ToString()).ToString("c"));//Amount
					_procTotalAmt+=PIn.Decimal(queryObj.ReportTable.Rows[j][3].ToString());
					row.Tag=PIn.Long(queryObj.ReportTable.Rows[j][4].ToString());//Tag set to ProcNum.  Used in butNewClaims_Click().
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
		}

		private void FillClinics() {
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboBoxMultiClinics.Items.Add(Lan.g(this,"All"));
			if(!Security.CurUser.ClinicIsRestricted) {
				comboBoxMultiClinics.Items.Add(Lan.g(this,"Unassigned"));
				comboBoxMultiClinics.SetSelected(1,true);
			}
			for(int i=0;i<_listClinics.Count;i++) {
				int curIndex=comboBoxMultiClinics.Items.Add(_listClinics[i].Description);
				if(Clinics.ClinicNum==0) {
					comboBoxMultiClinics.SetSelected(curIndex,true);
				}
				if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
					comboBoxMultiClinics.SelectedIndices.Clear();
					comboBoxMultiClinics.SetSelected(curIndex,true);
				}
			}
		}

		//Only called in FillGrid().
		private void RefreshReport() {
			ValidateFields();
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {//All option selected
					for(int j=0;j<_listClinics.Count;j++) {
						listClinicNums.Add(_listClinics[j].ClinicNum);//Add all clinics this person has access to.
					}
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);//Unassigned
					}
				}
				else {//All option not selected
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);//Minus 1 to skip over the All.
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {//Not restricted and user selected Unassigned.
							listClinicNums.Add(0);//Unassigned
						}
						else {//Not restricted and Unassigned option is not selected.
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);//Minus 2 to skip over All and Unassigned.
						}
					}
				}
			}
			DataTable tableNotBilled=RpProcNotBilledIns.GetProcsNotBilled(listClinicNums,checkMedical.Checked,_myReportDateFrom,_myReportDateTo);
			string subtitleClinics="";
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {//All option selected
					subtitleClinics=Lan.g(this,"All Clinics");
				}
				else {
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							subtitleClinics+=_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].Description;//Minus 1 for All.
						}
						else {//Not restricted
							if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {//Unassigned option selected.
								subtitleClinics+=Lan.g(this,"Unassigned");
							}
							else {
								subtitleClinics+=_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].Description;//Minus 2 for All and Unassigned.
							}
						}
					}
				}
			}
			_myReport=new ReportComplex(true,false);
			_myReport.ReportName=Lan.g(this,"Procedures Not Billed to Insurance");
			_myReport.AddTitle("Title",Lan.g(this,"Procedures Not Billed to Insurance"));
			_myReport.AddSubTitle("Practice Name",PrefC.GetString(PrefName.PracticeTitle));
			if(_myReportDateFrom==_myReportDateTo) {
				_myReport.AddSubTitle("Report Dates",_myReportDateFrom.ToShortDateString());
			}
			else {
				_myReport.AddSubTitle("Report Dates",_myReportDateFrom.ToShortDateString()+" - "+_myReportDateTo.ToShortDateString());
			}
			if(PrefC.HasClinicsEnabled) {
				_myReport.AddSubTitle("Clinics",subtitleClinics);
			}
			QueryObject query=_myReport.AddQuery(tableNotBilled,DateTimeOD.Today.ToShortDateString());
			query.AddColumn("Patient Name",_colWidthPatName,FieldValueType.String);
			query.AddColumn("Procedure Date",_colWidthProcDate,FieldValueType.Date);
			query.GetColumnDetail("Procedure Date").StringFormat="d";
			query.AddColumn("Procedure Description",300,FieldValueType.String);
			query.AddColumn("Amount",_colWidthAmount,FieldValueType.Number);
			_myReport.AddPageNum();
			if(!_myReport.SubmitQueries(false)) {//If we are loading and there are no results for _myReport do not show msgbox found in SubmitQueryies(...).
				return;
			}
		}

		private void butDropFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			textDateFrom.Text=calendarFrom.SelectionStart.ToShortDateString();
			FillGrid();
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			textDateTo.Text=calendarTo.SelectionStart.ToShortDateString();
			FillGrid();
		}

		private void ToggleCalendars() {
			if(calendarFrom.Visible) {//Both calendars are currently visible.
				//Hide the calendars and FillGrid() so that the new date range values will be reflected.
				calendarFrom.Visible=false;
				calendarTo.Visible=false;
				FillGrid();
			}
			else {//Both calendars are currently invisible.
				//show the calendars
				calendarFrom.Visible=true;
				calendarTo.Visible=true;
				//set the date on the calendars to match what's showing in the boxes
				if(textDateFrom.errorProvider1.GetError(textDateFrom)=="" && textDateTo.errorProvider1.GetError(textDateTo)=="") {//if no date errors
					if(textDateFrom.Text=="") {
						calendarFrom.SetDate(DateTime.Today);
					}
					else {
						calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
					}
					if(textDateTo.Text=="") {
						calendarTo.SetDate(DateTime.Today);
					}
					else {
						calendarTo.SetDate(PIn.Date(textDateTo.Text));
					}
				}
			}
		}

		private DateTime GetDateFrom() {
			try {
				return DateTime.Parse(textDateFrom.Text);
			}
			catch{
			}
			return DateTime.MinValue;
		}

		private DateTime GetDateTo() {
			try {
				return DateTime.Parse(textDateTo.Text);
			}
			catch{
			}
			return DateTime.MinValue;
		}

		//Only called in RefreshReport().
		private void ValidateFields() {
			_myReportDateFrom=GetDateFrom();
			_myReportDateTo=GetDateTo();
			if(_myReportDateFrom>_myReportDateTo) {
				_myReportDateFrom=DateTime.MinValue;
				_myReportDateTo=DateTime.MaxValue;
			}
			if(PrefC.HasClinicsEnabled) {
				bool isAllClinics=comboBoxMultiClinics.ListSelectedIndices.Contains(0);
				if(!isAllClinics && comboBoxMultiClinics.SelectedIndices.Count==0) {
					comboBoxMultiClinics.SetSelected(0,true);//All clinics.
				}
			}
		}

		private void textDateFrom_KeyUp(object sender,KeyEventArgs e) {
			DateTime dateFrom=GetDateFrom();
			if(dateFrom!=_dateFromPrev) {
				FillGrid();
				_dateFromPrev=dateFrom;
			}
		}

		private void textDateTo_KeyUp(object sender,KeyEventArgs e) {
			DateTime dateTo=GetDateTo();
			if(dateTo!=_dateToPrev) {
				FillGrid();
				_dateToPrev=dateTo;
			}
		}

		private void comboBoxMultiClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkMedical_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butPrint_Click(object sender,EventArgs e) {
			FormReportComplex FormR=new FormReportComplex(_myReport);
			FormR.ShowDialog();
		}
		
		private void butNewClaims_Click(object sender,EventArgs e) {
			if(!ContrAccount.CheckClearinghouseDefaults()) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {//Equivalent to selecting all procedures from gridMain.
				gridMain.SetSelected(true);
			}
			//Generate List and Table----------------------------------------------------------------------------------------------------------------------
			//List of all procedures being shown.
			//Pulls procedures based off of the PatNum, if the row was selected in gridMain and if it has been attached to a claim.
			List<ProcNotBilled> listNotBilledProcs=new List<ProcNotBilled>();
			List<long> listPatNums=new List<long>();
			long patNumOld=0;
			List<Claim> listPatClaims=new List<Claim>();
			List<ClaimProc> listPatClaimProcs=new List<ClaimProc>();
			List<ClaimProc> listCurClaimProcs=new List<ClaimProc>();
			//Table rows need to be 1:1 with gridMain rows due to logic in ContrAccount.toolBarButIns_Click(...).
			DataTable table=new DataTable();
			//Required columns as mentioned by ContrAccount.toolBarButIns_Click().
			table.Columns.Add("ProcNum");
			table.Columns.Add("chargesDouble");
			for(int i=0;i<gridMain.Rows.Count;i++) {//Loop through gridMain to construct listNotBilledProcs.
				long procNumCur=PIn.Long(gridMain.Rows[i].Tag.ToString());//Tag is set to procNum in fillGrid().
				Procedure procCur=Procedures.GetOneProc(procNumCur,false);
				long patNumCur=procCur.PatNum;
				if(patNumOld!=patNumCur) {//Procedures in gridMain are ordered by patient, so when the patient changes, we know previous patient is complete.
					listPatClaims=Claims.Refresh(patNumCur);
					listPatClaimProcs=ClaimProcs.Refresh(patNumCur);
					patNumOld=procCur.PatNum;
				}
				listCurClaimProcs=ClaimProcs.GetForProc(listPatClaimProcs,procNumCur);
				bool hasPriClaim=false;
				bool hasSecClaim=false;
				for(int j=0;j<listCurClaimProcs.Count;j++) {
					ClaimProc claimProcCur=listCurClaimProcs[j];
					if(claimProcCur.ClaimNum > 0 && claimProcCur.Status!=ClaimProcStatus.Preauth && claimProcCur.Status!=ClaimProcStatus.Estimate) {
						Claim claimCur=Claims.GetFromList(listPatClaims,claimProcCur.ClaimNum);
						switch(claimCur.ClaimType) {
							case "P":
								hasPriClaim=true;
								break;
							case "S":
								hasSecClaim=true;
								break;
						}
					}
				}
				bool isSelected=gridMain.SelectedIndices.Contains(i);
				listNotBilledProcs.Add(new ProcNotBilled(patNumCur,procNumCur,i,isSelected,hasPriClaim,hasSecClaim,procCur.ClinicNum,procCur.PlaceService));
				DataRow row=table.NewRow();
				row["ProcNum"]=procNumCur;
				#region Calculate chargesDouble
				//Logic copied from AccountModules.GetAccount(...) line 857.
				double qty=(procCur.UnitQty+procCur.BaseUnits);
				if(qty==0){
					qty=1;
				}
				double writeOffCapSum=listPatClaimProcs.Where(x => x.Status==ClaimProcStatus.CapComplete).Select(y => y.WriteOff).ToList().Sum();
				row["chargesDouble"]=(procCur.ProcFee*qty)-writeOffCapSum;
				#endregion Calculate chargesDouble
				table.Rows.Add(row);
				if(listPatNums.Contains(patNumCur)) {
					continue;
				}
				listPatNums.Add(patNumCur);
			}
			//Create Claims--------------------------------------------------------------------------------------------------------------------------------
			Patient patCur=null;
			List<PatPlan> listPatPlans=null;
			List<InsSub> listInsSubs=null;
			List<InsPlan> listInsPlans=null;
			List<Procedure> listPatientProcs=null;
			ProcNotBilled procNotBilled=new ProcNotBilled();//When automatically grouping,  this is used as the procedure to group by.
			patNumOld=0;
			string claimErrors="";
			int claimCreatedCount=0;
			int patIndex=0;
			//The procedures show in the grid ordered by patient.  Also listPatNums contains unique patnums which are in the same order as the grid.
			while(patIndex < listPatNums.Count) {
				List<ProcNotBilled> listProcs=listNotBilledProcs.Where(x => x.PatNum==listPatNums[patIndex] && x.IsRowSelected && !x.IsAttached).ToList();
				if(listProcs.Count==0) {
					patNumOld=listPatNums[patIndex];
					patIndex++;//No procedures were selected for this patient.
					continue;
				}
				else {
					//Maintain the same patient, in order to create one or more additional claims for the remaining procedures.
					//Currently will only happen for specific instances; 
					//--Canadian customers who are attempting to create a claim with over 7 procedures.
					//--When checkAutoGroupProcs is checked and when there are multiple procedure groupings by GroupKey status, ClinicNum, and placeService.
				}
				if(patNumOld!=listPatNums[patIndex]) {//The patient could repeat if we had to group the procedures for the patinet into multiple claims.
					patCur=Patients.GetPat(listPatNums[patIndex]);
					listPatPlans=PatPlans.Refresh(patCur.PatNum);
					listInsSubs=InsSubs.RefreshForFam(Patients.GetFamily(patCur.PatNum));
					listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
					listPatientProcs=Procedures.Refresh(patCur.PatNum);
				}
				if(checkAutoGroupProcs.Checked) {//Automatically Group Procedures.
					procNotBilled=listProcs[0];
					//Update listProcs to reflect those that match the procNotBilled values.
					listProcs=listProcs.FindAll(x => x.HasPriClaim==procNotBilled.HasPriClaim && x.HasSecClaim==procNotBilled.HasSecClaim);
					if(PrefC.HasClinicsEnabled) {//Group by clinic only if clinics enabled.
						listProcs=listProcs.FindAll(x => x.ClinicNum==procNotBilled.ClinicNum);
					}
					else if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {//Group by Place of Service only if Public Health feature is enabled.
						listProcs=listProcs.FindAll(x => x.PlaceService==procNotBilled.PlaceService);
					}
				}
				GetUniqueDiagnosticCodes(listProcs,listPatientProcs,listPatPlans,listInsSubs,listInsPlans);
				if(listProcs.Count>7 && CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					listProcs=listProcs.Take(7).ToList();//Returns first 7 items of the list.
				}
				gridMain.SetSelected(false);//Need to deslect all rows each time so that ContrAccount.toolBarButIns_Click(...) only uses pertinent rows.
				for(int j=0;j<listProcs.Count;j++) {
					gridMain.SetSelected(listProcs[j].RowIndex,true);//Select the pertinent rows so that they will be attached to the claim below.
				}
				ContrAccount.toolBarButIns_Click(false,patCur,Patients.GetFamily(patCur.PatNum),gridMain,table,
					procNotBilled.HasPriClaim,procNotBilled.HasSecClaim);
				string errorTitle=patCur.PatNum+" "+patCur.GetNameLFnoPref()+" - ";
				if(patNumOld==patCur.PatNum && !string.IsNullOrEmpty(ContrAccount.ClaimErrorsCur)){ 
					claimErrors+="\t\t"+ContrAccount.ClaimErrorsCur+"\r\n";
				}
				else if(!string.IsNullOrEmpty(ContrAccount.ClaimErrorsCur)){
					claimErrors+=errorTitle+ContrAccount.ClaimErrorsCur+"\r\n";
				}
				claimCreatedCount+=ContrAccount.ClaimCreatedCount;
				listProcs.ForEach(x => x.IsAttached=true);//This way we can not attach procedures to multiple claims thanks to the logic above.
				patNumOld=patCur.PatNum;
			}
			FillGrid();
			if(!string.IsNullOrEmpty(claimErrors)) {
				MsgBoxCopyPaste form=new MsgBoxCopyPaste(claimErrors);
				form.ShowDialog();
			}
			MessageBox.Show(Lan.g(this,"Number of claims created")+": "+claimCreatedCount);
		}
		
		///<summary>Mimics ContrAccount.CreateClaim(...).  Removes items from listProcs until unique diagnosis code count is low enough.</summary>
		private void GetUniqueDiagnosticCodes(List<ProcNotBilled> listProcs,List<Procedure> listPatProcs,List <PatPlan> listPatPlans,
			List<InsSub> listInsSubs,List<InsPlan> listInsPlans)
		{
			List<Procedure> listProcedures=new List<Procedure>();
			for(int i=0;i<listProcs.Count;i++) {
				listProcedures.Add(Procedures.GetProcFromList(listPatProcs,listProcs[i].ProcNum));
			}
			//If they have medical insurance and no dental, make the claim type Medical.  This is to avoid the scenario of multiple med ins and no dental.
			bool isMedical=false;
			if(PatPlans.GetOrdinal(PriSecMed.Medical,listPatPlans,listInsPlans,listInsSubs)>0
				&& PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlans,listInsPlans,listInsSubs)==0
				&& PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlans,listInsPlans,listInsSubs)==0)
			{
				isMedical=true;
			}
			while(!isMedical && Procedures.GetUniqueDiagnosticCodes(listProcedures,false).Count > 4) {//dental
				int index=listProcedures.Count-1;
				listProcedures.RemoveAt(index);
				listProcs.RemoveAt(index);
			}
			while(isMedical && Procedures.GetUniqueDiagnosticCodes(listProcedures,true).Count > 12) {//medical
				int index=listProcedures.Count-1;
				listProcedures.RemoveAt(index);
				listProcs.RemoveAt(index);
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormRpProcNotBilledIns_FormClosing(object sender,FormClosingEventArgs e) {
			Prefs.UpdateBool(PrefName.ClaimProcsNotBilledToInsAutoGroup,checkAutoGroupProcs.Checked);
		}

	}//end class FormRpProcNotBilledIns

	///<summary>Used so that we can easily select pertinent procedures for a specific patient when creating claims.</summary>
	internal class ProcNotBilled {
		public long PatNum;
		public long ProcNum;
		public int RowIndex;
		public bool IsRowSelected;
		///<summary>Flag used to make sure we do not attach procedures to multiple claims.
		///Very important for Canadian customers when we need to make multiple claims.</summary>
		public bool IsAttached;
		public bool HasPriClaim;
		public bool HasSecClaim;
		public long ClinicNum;
		public PlaceOfService PlaceService;

		public ProcNotBilled() {
			HasPriClaim=false;
			HasSecClaim=false;
		}

		public ProcNotBilled(long patNum,long procNum,int rowIndex,bool isRowSelected,
			bool hasPriClaim,bool hasSecClaim,long clinicNum,PlaceOfService placeService)
		{
			PatNum=patNum;
			ProcNum=procNum;
			RowIndex=rowIndex;
			IsRowSelected=isRowSelected;
			IsAttached=false;
			HasPriClaim=hasPriClaim;
			HasSecClaim=hasSecClaim;
			ClinicNum=clinicNum;
			PlaceService=placeService;
		}
	}

}