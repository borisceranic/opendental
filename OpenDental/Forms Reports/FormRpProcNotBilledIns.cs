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
		private int _pagesPrinted;
		private bool _headingPrinted;
		private int _headingPrintH;
		private decimal _procTotalAmt;
		private DateTime _myReportDateFrom;
		private UI.Button butNewClaims;
		private DateTime _myReportDateTo;
		private List<long> _listPatNum;
		private bool _isOnLoad;
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
			_isOnLoad=true;
			_listPatNum=new List<long>();
			calendarFrom.SelectionStart=DateTime.Today;
			calendarTo.SelectionStart=DateTime.Today;
			textDateFrom.Text=DateTime.Today.ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
			if(PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				checkMedical.Visible=true;
			}
			if(PrefC.HasClinicsEnabled) {
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
			else {
				comboBoxMultiClinics.Visible=false;
				labelClinic.Visible=false;
			}
			FillGrid();
			_isOnLoad=false;
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
					row.Tag=PIn.Long(queryObj.ReportTable.Rows[j][4].ToString());//Used in butNewClaims_Click().
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
		}

		//Only called in FillGrid().
		private void RefreshReport() {
			ValidateFields();
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {
					for(int j=0;j<_listClinics.Count;j++) {
						listClinicNums.Add(_listClinics[j].ClinicNum);//Add all clinics this person has access to.
					}
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);
					}
				}
				else {
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {
							listClinicNums.Add(0);
						}
						else {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);
						}
					}
				}
			}
			DataTable tableNotBilled=RpProcNotBilledIns.GetProcsNotBilled(listClinicNums,checkMedical.Checked,_myReportDateFrom,_myReportDateTo);
			string subtitleClinics="";
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {
					for(int j=0;j<_listClinics.Count;j++) {
						listClinicNums.Add(_listClinics[j].ClinicNum);//Add all clinics this person has access to.
					}
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);
					}
				}
				else {
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {
							listClinicNums.Add(0);
						}
						else {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);
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
			if(!_myReport.SubmitQueries(!_isOnLoad)) {//If we are loading and there are no results for _myReport do not show msgbox found in SubmitQueryies(...).
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
			if(_myReportDateFrom<_myReportDateTo) {
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

		private bool CheckClearinghouseDefaults() {
			if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==0) {
				MsgBox.Show(this,"No default dental clearinghouse defined.");
				return false;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)&&PrefC.GetLong(PrefName.ClearinghouseDefaultMed)==0) {
				MsgBox.Show(this,"No default medical clearinghouse defined.");
				return false;
			}
			return true;
		}

		///<summary>The only validation that's been done is just to make sure that only procedures are selected.  
		///All validation on the procedures selected is done here.  Creates and saves claim initially, attaching all selected procedures.  
		///But it does not refresh any data. Does not do a final update of the new claim.  Does not enter fee amounts.
		///claimType=P,S,Med,or Other
		///Returns a 'new' claim object (ClaimNum=0) to indicate that the user does not want to create the claim or there are validation issues.</summary>
		private Claim CreateClaim(Patient PatCur,string claimType,List<PatPlan> PatPlanList,List<InsPlan> planList,List<ClaimProc> ClaimProcList,List<Procedure> procsForPat,List<InsSub> subList,List<PatNumWithProcNum>listProcsForClaim) {
			long claimFormNum=0;
			InsPlan PlanCur=new InsPlan();
			InsSub SubCur=new InsSub();
			Relat relatOther=Relat.Self;
			switch(claimType) {
				case "P":
					SubCur=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,planList,subList)),subList);
					PlanCur=InsPlans.GetPlan(SubCur.PlanNum,planList);
					break;
				case "S":
					SubCur=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,planList,subList)),subList);
					PlanCur=InsPlans.GetPlan(SubCur.PlanNum,planList);
					break;
				case "Med":
					//It's already been verified that a med plan exists
					SubCur=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Medical,PatPlanList,planList,subList)),subList);
					PlanCur=InsPlans.GetPlan(SubCur.PlanNum,planList);
					break;
				case "Other":
					FormClaimCreate FormCC=new FormClaimCreate(PatCur.PatNum);
					FormCC.ShowDialog();
					if(FormCC.DialogResult!=DialogResult.OK) {
						return new Claim();
					}
					PlanCur=FormCC.SelectedPlan;
					SubCur=FormCC.SelectedSub;
					relatOther=FormCC.PatRelat;
					break;
			}
			Procedure proc;
			List<Procedure> listProcs=new List<Procedure>();
			for(int i=0;i<listProcsForClaim.Count;i++) {
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				string des=ProcedureCodes.GetProcCode(proc.CodeNum).Descript;
				listProcs.Add(proc);
			}
			if((claimType=="P"||claimType=="S")&&Procedures.GetUniqueDiagnosticCodes(listProcs,false).Count>4) {
				MsgBox.Show(this,"Claim has more than 4 unique diagnosis codes.  Create multiple claims instead.");
				return new Claim();
			}
			if(Procedures.GetUniqueDiagnosticCodes(listProcs,true).Count>12) {
				MsgBox.Show(this,"Claim has more than 12 unique diagnosis codes.  Create multiple claims instead.");
				return new Claim();
			}
			for(int i=0;i<listProcsForClaim.Count;i++) {
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				if(Procedures.NoBillIns(proc,ClaimProcList,PlanCur.PlanNum)) {
					MsgBox.Show(this,"Not allowed to send procedures to insurance that are marked 'Do not bill to ins'.");
					return new Claim();
				}
			}
			for(int i=0;i<listProcsForClaim.Count;i++) {
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				string des=ProcedureCodes.GetProcCode(proc.CodeNum).Descript;
				if(Procedures.IsAlreadyAttachedToClaim(proc,ClaimProcList,SubCur.InsSubNum)) {
					MsgBox.Show(this,"Not allowed to send a procedure to the same insurance company twice.");
					return new Claim();
				}
			}
			proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[0].ProcNum);
			long clinicNum=proc.ClinicNum;
			PlaceOfService placeService=proc.PlaceService;
			for(int i=1;i<listProcsForClaim.Count;i++) {//skips 0
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				if(clinicNum!=proc.ClinicNum) {
					MsgBox.Show(this,"All procedures do not have the same clinic.");
					return new Claim();
				}
				if(proc.PlaceService!=placeService) {
					MsgBox.Show(this,"All procedures do not have the same place of service.");
					return new Claim();
				}
			}
			ClaimProc[] claimProcs=new ClaimProc[listProcsForClaim.Count];//1:1 with selectedIndices
			long procNum;
			for(int i=0;i<listProcsForClaim.Count;i++) {//loop through selected procs
																														//and try to find an estimate that can be used
				procNum=PIn.Long(listProcsForClaim[i].ProcNum.ToString());
				claimProcs[i]=Procedures.GetClaimProcEstimate(procNum,ClaimProcList,PlanCur,SubCur.InsSubNum);
			}
			for(int i=0;i<claimProcs.Length;i++) {//loop through each claimProc
																							//and create any missing estimates. This handles claims to 3rd and 4th ins co's.
				if(claimProcs[i]==null) {
					claimProcs[i]=new ClaimProc();
					proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
					ClaimProcs.CreateEst(claimProcs[i],proc,PlanCur,SubCur);
				}
			}
			Claim ClaimCur=new Claim();
			Claims.Insert(ClaimCur);
			for(int i=0;i<claimProcs.Length;i++) {
				if(claimProcs[i].Status==ClaimProcStatus.CapComplete) {
					claimProcs[i].ClaimNum=ClaimCur.ClaimNum;
					claimProcs[i]=claimProcs[i].Copy();
					claimProcs[i].WriteOff=0;
					claimProcs[i].CopayAmt=-1;
					claimProcs[i].CopayOverride=-1;
					//status will get changed down below
					ClaimProcs.Insert(claimProcs[i]);//this makes a duplicate in db with different claimProcNum
				}
			}
			ClaimCur.PatNum=PatCur.PatNum;
			ClaimCur.DateService=claimProcs[claimProcs.Length-1].ProcDate;
			ClaimCur.ClinicNum=clinicNum;
			ClaimCur.PlaceService=proc.PlaceService;
			//datesent
			ClaimCur.ClaimStatus="U";
			//datereceived
			InsSub sub;
			ClaimCur.PlanNum=PlanCur.PlanNum;
			ClaimCur.InsSubNum=SubCur.InsSubNum;
			switch(claimType) {
				case "P":
					ClaimCur.PatRelat=PatPlans.GetRelat(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,planList,subList));
					ClaimCur.ClaimType="P";
					ClaimCur.InsSubNum2=PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,planList,subList));
					sub=InsSubs.GetSub(ClaimCur.InsSubNum2,subList);
					if(sub.PlanNum>0&&InsPlans.RefreshOne(sub.PlanNum).IsMedical) {
						ClaimCur.PlanNum2=0;//no sec ins
						ClaimCur.PatRelat2=Relat.Self;
					}
					else {
						ClaimCur.PlanNum2=sub.PlanNum;//might be 0 if no sec ins
						ClaimCur.PatRelat2=PatPlans.GetRelat(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,planList,subList));
					}
					break;
				case "S":
					ClaimCur.PatRelat=PatPlans.GetRelat(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,planList,subList));
					ClaimCur.ClaimType="S";
					ClaimCur.InsSubNum2=PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,planList,subList));
					sub=InsSubs.GetSub(ClaimCur.InsSubNum2,subList);
					ClaimCur.PlanNum2=sub.PlanNum;
					ClaimCur.PatRelat2=PatPlans.GetRelat(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,planList,subList));
					break;
				case "Med":
					ClaimCur.PatRelat=PatPlans.GetFromList(PatPlanList,SubCur.InsSubNum).Relationship;
					ClaimCur.ClaimType="Other";
					if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)) {
						ClaimCur.MedType=EnumClaimMedType.Institutional;
					}
					else {
						ClaimCur.MedType=EnumClaimMedType.Medical;
					}
					break;
				case "Other":
					ClaimCur.PatRelat=relatOther;
					ClaimCur.ClaimType="Other";
					//plannum2 is not automatically filled in.
					ClaimCur.ClaimForm=claimFormNum;
					if(PlanCur.IsMedical) {
						if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)) {
							ClaimCur.MedType=EnumClaimMedType.Institutional;
						}
						else {
							ClaimCur.MedType=EnumClaimMedType.Medical;
						}
					}
					break;
			}
			if(PlanCur.PlanType=="c") {//if capitation
				ClaimCur.ClaimType="Cap";
			}
			ClaimCur.ProvTreat=Procedures.GetProcFromList(procsForPat,PIn.Long(listProcsForClaim[0].ProcNum.ToString())).ProvNum;
			for(int i=0;i<listProcsForClaim.Count;i++) {
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				if(!Providers.GetIsSec(proc.ProvNum)) {//if not a hygienist
					ClaimCur.ProvTreat=proc.ProvNum;
				}
			}
			if(Providers.GetIsSec(ClaimCur.ProvTreat)) {
				ClaimCur.ProvTreat=PatCur.PriProv;
				//OK if 0, because auto select first in list when open claim
			}
			ClaimCur.IsProsthesis="N";
			ClaimCur.ProvBill=Providers.GetBillingProvNum(ClaimCur.ProvTreat,ClaimCur.ClinicNum);
			Provider prov=Providers.GetProv(ClaimCur.ProvTreat);
			if(prov.ProvNumBillingOverride!=0) {
				ClaimCur.ProvBill=prov.ProvNumBillingOverride;
			}
			ClaimCur.EmployRelated=YN.No;
			ClaimCur.ClaimForm=PlanCur.ClaimFormNum;
			for(int i=0;i<claimProcs.Length;i++) {
				proc=Procedures.GetProcFromList(procsForPat,listProcsForClaim[i].ProcNum);
				claimProcs[i].ClaimNum=ClaimCur.ClaimNum;
				if(PlanCur.PlanType=="c") {
					claimProcs[i].Status=ClaimProcStatus.CapClaim;
				}
				else
					claimProcs[i].Status=ClaimProcStatus.NotReceived;
				if(PlanCur.UseAltCode&&(ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1!="")) {
					claimProcs[i].CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1;
				}
				else if(PlanCur.IsMedical&&proc.MedicalCode!="") {
					claimProcs[i].CodeSent=proc.MedicalCode;
				}
				else {
					claimProcs[i].CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode;
					if(claimProcs[i].CodeSent.Length>5&&claimProcs[i].CodeSent.Substring(0,1)=="D") {
						claimProcs[i].CodeSent=claimProcs[i].CodeSent.Substring(0,5);
					}
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						if(claimProcs[i].CodeSent.Length>5) { //In Canadian electronic claims, codes can contain letters or numbers and cannot be longer than 5 characters.
							claimProcs[i].CodeSent=claimProcs[i].CodeSent.Substring(0,5);
						}
					}
				}
			}//for claimProc
			List <ClaimProc> listClaimProcs=new List<ClaimProc>(claimProcs);
			for(int i=0;i<listClaimProcs.Count;i++) {
				listClaimProcs[i].LineNumber=(byte)(i+1);
				ClaimProcs.Update(listClaimProcs[i]);
			}
			//Insert claim snapshots for historical reporting purposes.
			CreateClaimSnapshot(claimType,listClaimProcs,proc.ProcFee);//,procsForPat
			return ClaimCur;
			//return null;
		}

		//<summary>The procsForPat variable is all of the current procedures for the current patient. The tableAccount variable is the table from the DataSetMain object containing the information for the account grid.</summary>
		private void InsCanadaValidateProcs(List<Procedure> procsForPat,List<PatNumWithProcNum> listProcsForClaim) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Not Canadian (en-CA or fr-CA).
				return;
			}
			if(listProcsForClaim.Count<8) {//Canadian customer but no need to split procedures to multiple claims.
				return;
			}
			//Limit each claim to 7 procedures. Canadian claim requirement.
			listProcsForClaim.Take(7).ToList().ForEach(p => p.IsVisited=true);
			listProcsForClaim.RemoveAll(p => p.IsVisited == false);
		}

		private void CreateClaimSnapshot(string claimType,List<ClaimProc> listClaimProcs,double procFee) {//,List<Procedure> listPatProcs
			if(!PrefC.GetBool(PrefName.ClaimSnapshotEnabled) || (claimType!="P" && claimType!="S")) {
				return;
			}
			//Loop through all the claimprocs and create a claimsnapshot entry for each.
			for(int i=0;i<listClaimProcs.Count;i++) {
				if(listClaimProcs[i].Status==ClaimProcStatus.CapClaim
					|| listClaimProcs[i].Status==ClaimProcStatus.CapComplete
					|| listClaimProcs[i].Status==ClaimProcStatus.CapEstimate
					|| listClaimProcs[i].Status==ClaimProcStatus.Preauth) 
				{
					continue;
				}
				ClaimSnapshot snapshot=new ClaimSnapshot();
				snapshot.ProcNum=listClaimProcs[i].ProcNum;
				snapshot.Writeoff=listClaimProcs[i].WriteOffEst;
				snapshot.InsPayEst=listClaimProcs[i].InsEstTotal;
				snapshot.Fee=procFee;
				snapshot.ClaimType=claimType;				
				ClaimSnapshots.Insert(snapshot);
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

		//Mimics FormRpOustandingIns.butPrint_Click()
		private void butPrint_Click(object sender,EventArgs e) {
			FormReportComplex FormR=new FormReportComplex(_myReport);
			FormR.ShowDialog();
		}

		//Logic mimics ContrAccount.toolBarButIns_Click()
		private void butNewClaims_Click(object sender,EventArgs e) {//Mimics ContrAccount.toolBarButIns_Click()
			if(!CheckClearinghouseDefaults()) {
				return;
			}
			List<PatNumWithProcNum> listProcWithIndice=new List<PatNumWithProcNum>();
			List<PatNumWithProcNum> listProcsForClaim=null;//Stores pertinent procedures to be put on each claim as they are built.
			_listPatNum.Clear();
			long procNumCur;
			long patNumCur;
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(gridMain.SelectedIndices.Length!=0 && !gridMain.SelectedIndices.Contains(i)) {
					continue;//We have selected indices from gridMain and this row is not one of them.
				}
				procNumCur=PIn.Long(gridMain.Rows[i].Tag.ToString());//Tag is set to procNum in fillGrid().
				patNumCur=Procedures.GetOneProc(procNumCur,false).PatNum;//??optimize?
				listProcWithIndice.Add(new PatNumWithProcNum(patNumCur,procNumCur));
				if(_listPatNum.Contains(patNumCur)) {
					continue;
				}
				_listPatNum.Add(patNumCur);
			}
			Patient PatCur;
			Family FamCur;
			List <PatPlan> PatPlanList=null;
			List<InsSub> SubList=null;
			List<InsPlan> InsPlanList=null;
			List<ClaimProc> ClaimProcList=null;
			List <Benefit> BenefitList=null;
			List<Procedure> procsForPat=null;
			bool doMultiClaimForPat=false;//Flag only used for Canadian customers who have more then 7 procedures attempting to be attached to a claim.
			long patNumOld=0;
			for(int i=0;i<_listPatNum.Count;i++) {
				if(doMultiClaimForPat) {
					//Equivalent to maintaining the same patNum.
					//Will only happen for Canadian customers where a patient has more then 7 procedures attempting to be attached to a claim.
					//When we need multiple claims the logic below will pick the remaining non visited prcedures for the same patient using the IsVisited flag.
					//Currently this will only happen for Canadian customers when a patient has more than 7 procedures attempting to attach to a claim.
					i--;
					doMultiClaimForPat=false;
				}
				PatCur=Patients.GetPat(_listPatNum[i]);
				//This way we do not do unnecessary queries with the database when we revisit a patient.
				//Currently should only happen for Canadian customers who need to create multiple claims for a single patient.
				if(patNumOld!=PatCur.PatNum) {
					FamCur=Patients.GetFamily(_listPatNum[i]);
					listProcsForClaim=listProcWithIndice.Where(x => x.PatNum==PatCur.PatNum&&!x.IsVisited).ToList();
					doMultiClaimForPat=(listProcsForClaim.Count>7&&CultureInfo.CurrentCulture.Name.EndsWith("CA"));
					PatPlanList=PatPlans.Refresh(PatCur.PatNum);
					SubList=InsSubs.RefreshForFam(FamCur);
					InsPlanList=InsPlans.RefreshForSubList(SubList);
					ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
					BenefitList=Benefits.Refresh(PatPlanList,SubList);
					procsForPat=Procedures.Refresh(PatCur.PatNum);
				}
				if(PatPlanList.Count==0) {
					MsgBox.Show(this,"Patient does not have insurance.");
					return;
				}
				InsCanadaValidateProcs(procsForPat,listProcsForClaim);//Unselects lab fees and limitis Canadian customers to at most 7 procs per claim.
				string claimType="P";
				//If they have medical insurance and no dental, make the claim type Medical.  This is to avoid the scenario of multiple med ins and no dental.
				if(PatPlans.GetOrdinal(PriSecMed.Medical,PatPlanList,InsPlanList,SubList)>0
					&&PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,InsPlanList,SubList)==0
					&&PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,InsPlanList,SubList)==0) {
					claimType="Med";
				}
				Claim ClaimCur=CreateClaim(PatCur,claimType,PatPlanList,InsPlanList,ClaimProcList,procsForPat,SubList,listProcsForClaim);
				ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
				if(ClaimCur.ClaimNum==0) {
					return;
				}
				ClaimCur.ClaimStatus="W";
				ClaimCur.DateSent=DateTimeOD.Today;
				ClaimL.CalculateAndUpdate(procsForPat,InsPlanList,ClaimCur,PatPlanList,BenefitList,PatCur.Age,SubList);
				if(PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,InsPlanList,SubList)>0 //if there exists a secondary plan
					&&!CultureInfo.CurrentCulture.Name.EndsWith("CA"))//And not Canada (don't create secondary claim for Canada)
				{
					InsSub sub=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,InsPlanList,SubList)),SubList);
					InsPlan plan=InsPlans.GetPlan(sub.PlanNum,InsPlanList);
					ClaimCur=CreateClaim(PatCur,"S",PatPlanList,InsPlanList,ClaimProcList,procsForPat,SubList,listProcsForClaim);
					if(ClaimCur.ClaimNum==0) {
						return;
					}
					ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
					ClaimCur.ClaimStatus="H";
					ClaimL.CalculateAndUpdate(procsForPat,InsPlanList,ClaimCur,PatPlanList,BenefitList,PatCur.Age,SubList);
				}
				patNumOld=PatCur.PatNum;
			}
			gridMain.SetSelected(false);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

	//Used so that when we can easily select pertinent procedures for a specific patient when creating claims. 
	class PatNumWithProcNum {
		public long PatNum;
		public long ProcNum;
		//Flag used to make sure we do not attach procedures to multiple claims.
		//Very important for Canadian customers when we need to make multiple claims.
		public bool IsVisited;

		public PatNumWithProcNum(long patNum,long procNum) {
			this.PatNum=patNum;
			this.ProcNum=procNum;
			this.IsVisited=false;
		}
	}
}