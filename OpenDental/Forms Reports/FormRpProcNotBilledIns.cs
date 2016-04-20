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
			this.SuspendLayout();
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(251, 33);
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 2;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(25, 33);
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
			this.butClose.Location = new System.Drawing.Point(872, 666);
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
			this.butPrint.Location = new System.Drawing.Point(25, 666);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkMedical
			// 
			this.checkMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.Location = new System.Drawing.Point(730, 9);
			this.checkMedical.Name = "checkMedical";
			this.checkMedical.Size = new System.Drawing.Size(217, 21);
			this.checkMedical.TabIndex = 11;
			this.checkMedical.Text = "Include Medical Procedures";
			this.checkMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.UseVisualStyleBackColor = true;
			this.checkMedical.Visible = false;
			this.checkMedical.Click += new System.EventHandler(this.checkMedical_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(47, 11);
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
			this.butDropFrom.Location = new System.Drawing.Point(177, 11);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(22, 18);
			this.butDropFrom.TabIndex = 63;
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(98, 10);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(102, 20);
			this.textDateFrom.TabIndex = 62;
			this.textDateFrom.Validated += new System.EventHandler(this.textDateFrom_Validated);
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butDropTo.Location = new System.Drawing.Point(414, 10);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(22, 18);
			this.butDropTo.TabIndex = 66;
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(289, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 16);
			this.label2.TabIndex = 64;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(335, 9);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(102, 20);
			this.textDateTo.TabIndex = 65;
			this.textDateTo.Validated += new System.EventHandler(this.textDateTo_Validated);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(481, 10);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(87, 16);
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
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(569, 8);
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
			this.gridMain.Location = new System.Drawing.Point(25, 33);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(922, 630);
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
			this.butNewClaims.Location = new System.Drawing.Point(450, 666);
			this.butNewClaims.Name = "butNewClaims";
			this.butNewClaims.Size = new System.Drawing.Size(75, 26);
			this.butNewClaims.TabIndex = 71;
			this.butNewClaims.Text = "New Claims";
			this.butNewClaims.UseVisualStyleBackColor = true;
			this.butNewClaims.Click += new System.EventHandler(this.butNewClaims_Click);
			// 
			// FormRpProcNotBilledIns
			// 
			this.AcceptButton = this.butPrint;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(974, 696);
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
			FillGrid();
		}

		private void CreateOrResizeGridColumns() {
			gridMain.BeginUpdate();
			int colWidthVariable=gridMain.Width-_colWidthPatName-_colWidthProcDate-_colWidthAmount-10;//10 for scrollbar
			ODGridColumn col=null;
			if(gridMain.Columns.Count==0) {
				col=new ODGridColumn(Lan.g(this,"Patient Name"),_colWidthPatName);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Date"),_colWidthProcDate);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Descipion"),colWidthVariable);
				gridMain.Columns.Add(col);
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
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			textDateTo.Text=calendarTo.SelectionStart.ToShortDateString();
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

		//Only called in RefreshReport().
		private void ValidateFields() {
			try {
				_myReportDateFrom=DateTime.Parse(textDateFrom.Text);
			}
			catch{
				_myReportDateFrom=DateTime.MinValue;
			}
			try {
				_myReportDateTo=DateTime.Parse(textDateTo.Text);
			}
			catch{
				_myReportDateTo=DateTime.MaxValue;
			}
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

		private void comboBoxMultiClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void textDateTo_Validated(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDateFrom_Validated(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkMedical_Click(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butPrint_Click(object sender,EventArgs e) {
			FormReportComplex FormR=new FormReportComplex(_myReport);
			FormR.ShowDialog();
		}
		
		private void butNewClaims_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {//Equivalent to selecting all procedures from gridMain.
				gridMain.SetSelected(true);
			}
			//Generate List and Table----------------------------------------------------------------------------------------------------------------------
			//List of all procedures being shown.
			//Pulls procedures based off of the PatNum, if the row was selected in gridMain and if it has been attached to a claim.
			List<PatNumWithProcNum> listProcWithIndices=new List<PatNumWithProcNum>();
			List<long> listPatNums=new List<long>();
			//Table needs to be 1:1 with gridMain due to logic in ContrAccount.toolBarButIns_Click(...).
			DataTable table=new DataTable();
			//Required columns as mentioned by ContrAccount.toolBarButIns_Click().
			table.Columns.Add("ProcNum");
			table.Columns.Add("chargesDouble");
			for(int i=0;i<gridMain.Rows.Count;i++) {
				long procNumCur=PIn.Long(gridMain.Rows[i].Tag.ToString());//Tag is set to procNum in fillGrid().
				Procedure procCur=Procedures.GetOneProc(procNumCur,false);
				long patNumCur=procCur.PatNum;
				bool isSelected=gridMain.SelectedIndices.Contains(i);
				listProcWithIndices.Add(new PatNumWithProcNum(patNumCur,procNumCur,i,isSelected));
				DataRow row=table.NewRow();
				row["ProcNum"]=procNumCur;
				#region Calculate chargesDouble
				//Logic copied from AccountModules.GetAccount(...) line 857.
				double qty=(procCur.UnitQty+procCur.BaseUnits);
				if(qty==0){
					qty=1;
				}
				List<ClaimProc> listClaimProcs=ClaimProcs.RefreshForProc(procNumCur);
				double writeOffCapSum=listClaimProcs.Where(x => x.Status==ClaimProcStatus.CapComplete).Select(y => y.WriteOff).ToList().Sum();
				row["chargesDouble"]=(procCur.ProcFee*qty)-writeOffCapSum;
				#endregion Calculate chargesDouble
				table.Rows.Add(row);
				if(listPatNums.Contains(patNumCur)) {
					continue;
				}
				listPatNums.Add(patNumCur);
			}
			//Create Claims--------------------------------------------------------------------------------------------------------------------------------
			string claimErrors="";
			int claimCreatedCount=0;
			long patNumOld=0;
			//The procedures show in the grid ordered by patient.  Also listPatNums contains unique patnums which are in the same order as the grid.
			for(int i=0;i<listPatNums.Count;i++) {
				Patient patCur=Patients.GetPat(listPatNums[i]);
				gridMain.SetSelected(false);//Need to deslect all rows each time so that ContrAccount.toolBarButIns_Click(...) only uses pertinent rows.
				List<PatNumWithProcNum> listProcs=listProcWithIndices.Where(x => x.PatNum == listPatNums[i] && x.IsRowSelected && !x.IsAttached).ToList();
				if(listProcs.Count>7 && CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					listProcs=listProcs.Take(7).ToList();//Returns first 7 items of the list.
					i--;//To maintain the same patient, in order to create one or more additional claims for the remaining procedures.
				}
				for(int j=0;j<listProcs.Count;j++) {
					//Select the pertinent rows so that they will be flagged to be attached to the claim below.
					gridMain.SetSelected(listProcs[j].RowIndex,true);
				}
				ContrAccount.toolBarButIns_Click(false,patCur,Patients.GetFamily(patCur.PatNum),gridMain,table);
				string errorTitle=patCur.PatNum+" "+patCur.GetNameLFnoPref()+" - ";
				if(patNumOld==patCur.PatNum) {//Should only happen for Canadian customers who are attempting to create a claim with more then 7 procedures.
					string spaces=new string(new char[errorTitle.Length]).Replace('\0', ' ');
					claimErrors+=spaces+ContrAccount.ClaimErrorsCur+"\r\n";
				}
				else {
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

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	
	}//end class FormRpProcNotBilledIns

	///<summary>Used so that we can easily select pertinent procedures for a specific patient when creating claims.</summary>
	internal class PatNumWithProcNum {
		public long PatNum;
		public long ProcNum;
		public int RowIndex;
		public bool IsRowSelected;
		///<summary>Flag used to make sure we do not attach procedures to multiple claims.
		///Very important for Canadian customers when we need to make multiple claims.</summary>
		public bool IsAttached;

		public PatNumWithProcNum(long patNum,long procNum,int rowIndex,bool isRowSelected) {
			PatNum=patNum;
			ProcNum=procNum;
			RowIndex=rowIndex;
			IsRowSelected=isRowSelected;
			IsAttached=false;
		}
	}
}