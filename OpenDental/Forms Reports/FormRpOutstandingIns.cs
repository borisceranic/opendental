using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using OpenDentBusiness;
using System.Collections;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormRpOutstandingIns:Form {
		private ODGrid gridMain;
		private CheckBox checkPreauth;
		private Label labelProv;
		private ValidNum textDaysOldMin;
		private Label labelDaysOldMin;
		private UI.Button butCancel;
		private ValidNum textDaysOldMax;
		private Label labelDaysOldMax;
    private DateTime dateMin;
    private DateTime dateMax;
    private List<long> provNumList;
    private bool isAllProv;
    private bool isPreauth;
		private DataTable Table;
		private UI.Button butPrint;
		private ComboBoxMulti comboBoxMultiProv;
		private bool headingPrinted;
		private int pagesPrinted;
		private Label label1;
		private Label label2;
		private TextBox textBox1;
		private UI.Button butExport;
		private int headingPrintH;
		private UI.Button butRefresh;
		private ComboBoxMulti comboBoxMultiClinics;
		private Label labelClinic;
		private decimal total;
		private CheckBox checkIgnoreCustom;
		private ComboBox comboLastClaimTrack;
		private Label label3;
		private List<Clinic> _listClinics;

		public FormRpOutstandingIns() {
			InitializeComponent();
			Lan.F(this);
		}

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpOutstandingIns));
			this.checkPreauth = new System.Windows.Forms.CheckBox();
			this.labelProv = new System.Windows.Forms.Label();
			this.labelDaysOldMin = new System.Windows.Forms.Label();
			this.labelDaysOldMax = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
			this.butPrint = new OpenDental.UI.Button();
			this.textDaysOldMax = new OpenDental.ValidNum();
			this.textDaysOldMin = new OpenDental.ValidNum();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
			this.labelClinic = new System.Windows.Forms.Label();
			this.checkIgnoreCustom = new System.Windows.Forms.CheckBox();
			this.comboLastClaimTrack = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// checkPreauth
			// 
			this.checkPreauth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreauth.Checked = true;
			this.checkPreauth.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkPreauth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreauth.Location = new System.Drawing.Point(281, 9);
			this.checkPreauth.Name = "checkPreauth";
			this.checkPreauth.Size = new System.Drawing.Size(149, 18);
			this.checkPreauth.TabIndex = 51;
			this.checkPreauth.Text = "Include Preauths";
			this.checkPreauth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreauth.CheckedChanged += new System.EventHandler(this.checkPreauth_CheckedChanged);
			// 
			// labelProv
			// 
			this.labelProv.Location = new System.Drawing.Point(431, 8);
			this.labelProv.Name = "labelProv";
			this.labelProv.Size = new System.Drawing.Size(87, 16);
			this.labelProv.TabIndex = 48;
			this.labelProv.Text = "Treat Provs";
			this.labelProv.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// labelDaysOldMin
			// 
			this.labelDaysOldMin.Location = new System.Drawing.Point(4, 7);
			this.labelDaysOldMin.Name = "labelDaysOldMin";
			this.labelDaysOldMin.Size = new System.Drawing.Size(93, 18);
			this.labelDaysOldMin.TabIndex = 46;
			this.labelDaysOldMin.Text = "Days Old (min)";
			this.labelDaysOldMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDaysOldMax
			// 
			this.labelDaysOldMax.Location = new System.Drawing.Point(164, 7);
			this.labelDaysOldMax.Name = "labelDaysOldMax";
			this.labelDaysOldMax.Size = new System.Drawing.Size(53, 18);
			this.labelDaysOldMax.TabIndex = 46;
			this.labelDaysOldMax.Text = "(max)";
			this.labelDaysOldMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(94, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 19);
			this.label1.TabIndex = 54;
			this.label1.Text = "(leave both blank to show all)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(730, 480);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 18);
			this.label2.TabIndex = 46;
			this.label2.Text = "Total";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(805, 479);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(61, 20);
			this.textBox1.TabIndex = 56;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(880, 5);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 58;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Image = global::OpenDental.Properties.Resources.butExport;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(97, 476);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(79, 24);
			this.butExport.TabIndex = 57;
			this.butExport.Text = "&Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// comboBoxMultiProv
			// 
			this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiProv.DroppedDown = false;
			this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
			this.comboBoxMultiProv.Location = new System.Drawing.Point(519, 7);
			this.comboBoxMultiProv.Name = "comboBoxMultiProv";
			this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
			this.comboBoxMultiProv.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiProv.TabIndex = 53;
			this.comboBoxMultiProv.Leave += new System.EventHandler(this.comboBoxMultiProv_Leave);
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
			this.butPrint.Location = new System.Drawing.Point(12, 476);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 24);
			this.butPrint.TabIndex = 52;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// textDaysOldMax
			// 
			this.textDaysOldMax.Location = new System.Drawing.Point(220, 7);
			this.textDaysOldMax.MaxVal = 255;
			this.textDaysOldMax.MinVal = 0;
			this.textDaysOldMax.Name = "textDaysOldMax";
			this.textDaysOldMax.Size = new System.Drawing.Size(60, 20);
			this.textDaysOldMax.TabIndex = 47;
			this.textDaysOldMax.TextChanged += new System.EventHandler(this.textDaysOldMax_TextChanged);
			// 
			// textDaysOldMin
			// 
			this.textDaysOldMin.Location = new System.Drawing.Point(101, 7);
			this.textDaysOldMin.MaxVal = 255;
			this.textDaysOldMin.MinVal = 0;
			this.textDaysOldMin.Name = "textDaysOldMin";
			this.textDaysOldMin.Size = new System.Drawing.Size(60, 20);
			this.textDaysOldMin.TabIndex = 47;
			this.textDaysOldMin.Text = "30";
			this.textDaysOldMin.TextChanged += new System.EventHandler(this.textDaysOldMin_TextChanged);
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
			this.butCancel.Location = new System.Drawing.Point(880, 476);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 45;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 56);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(950, 406);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = null;
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.DroppedDown = false;
			this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(519, 29);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 59;
			this.comboBoxMultiClinics.Visible = false;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(431, 31);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(87, 16);
			this.labelClinic.TabIndex = 60;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.labelClinic.Visible = false;
			// 
			// checkIgnoreCustom
			// 
			this.checkIgnoreCustom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIgnoreCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIgnoreCustom.Location = new System.Drawing.Point(281, 29);
			this.checkIgnoreCustom.Name = "checkIgnoreCustom";
			this.checkIgnoreCustom.Size = new System.Drawing.Size(149, 18);
			this.checkIgnoreCustom.TabIndex = 61;
			this.checkIgnoreCustom.Text = "Ignore Custom Tracking";
			this.checkIgnoreCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIgnoreCustom.CheckedChanged += new System.EventHandler(this.checkIgnoreCustom_CheckedChanged);
			// 
			// comboLastClaimTrack
			// 
			this.comboLastClaimTrack.FormattingEnabled = true;
			this.comboLastClaimTrack.Location = new System.Drawing.Point(688, 28);
			this.comboLastClaimTrack.Name = "comboLastClaimTrack";
			this.comboLastClaimTrack.Size = new System.Drawing.Size(178, 21);
			this.comboLastClaimTrack.TabIndex = 62;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(685, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(181, 16);
			this.label3.TabIndex = 63;
			this.label3.Text = "Last Claim Custom Tracking Status";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormRpOutstandingIns
			// 
			this.ClientSize = new System.Drawing.Size(974, 512);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboLastClaimTrack);
			this.Controls.Add(this.checkIgnoreCustom);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboBoxMultiClinics);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelDaysOldMin);
			this.Controls.Add(this.comboBoxMultiProv);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.checkPreauth);
			this.Controls.Add(this.labelProv);
			this.Controls.Add(this.textDaysOldMax);
			this.Controls.Add(this.textDaysOldMin);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelDaysOldMax);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpOutstandingIns";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Outstanding Insurance Claims";
			this.Load += new System.EventHandler(this.FormRpOutIns_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void FormRpOutIns_Load(object sender,EventArgs e) {
			FillProvs();
			if(PrefC.HasClinicsEnabled) {
				comboBoxMultiClinics.Visible=true;
				labelClinic.Visible=true;
				FillClinics();
			}
			FillCustomTrack();
			FillGrid();
		}

		private void FillProvs() {
			comboBoxMultiProv.Items.Add("All");
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				comboBoxMultiProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
			}
			comboBoxMultiProv.SetSelected(0,true);
			comboBoxMultiProv.RefreshText();
			isAllProv=true;
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
				if(FormOpenDental.ClinicNum==0) {
					comboBoxMultiClinics.SetSelected(curIndex,true);
				}
				if(_listClinics[i].ClinicNum==FormOpenDental.ClinicNum) {
					comboBoxMultiClinics.SelectedIndices.Clear();
					comboBoxMultiClinics.SetSelected(curIndex,true);
				}
			}
		}

		private void FillCustomTrack() {
			comboLastClaimTrack.Items.Add("All");
			Def[] arrayDefs=DefC.GetList(DefCat.ClaimCustomTracking);
			foreach(Def definition in arrayDefs) {
				comboLastClaimTrack.Items.Add(definition.ItemName);
			}
			comboLastClaimTrack.SelectedIndex=0;
		}

		private void FillGrid() {
			if(textDaysOldMin.Text.Trim()=="" || PIn.Double(textDaysOldMin.Text)==0) {
				dateMin=DateTime.MinValue;
			}
			else {
				dateMin = DateTimeOD.Today.AddDays(-1 * PIn.Int(textDaysOldMin.Text));
			}
			if(textDaysOldMax.Text.Trim()=="" || PIn.Double(textDaysOldMax.Text)==0) {
				dateMax=DateTime.MinValue;
			}
			else {
				dateMax = DateTimeOD.Today.AddDays(-1 * PIn.Int(textDaysOldMax.Text));
			}
			if(comboBoxMultiProv.SelectedIndices[0].ToString()=="0") {
				isAllProv=true;
			}
			else {
				isAllProv=false;
				provNumList=new List<long>();
				for(int i=0;i<comboBoxMultiProv.SelectedIndices.Count;i++) {
					provNumList.Add((long)ProviderC.ListShort[(int)comboBoxMultiProv.SelectedIndices[i]-1].ProvNum);
				}
			}
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
			isPreauth=checkPreauth.Checked;
			Table=RpOutstandingIns.GetOutInsClaims(isAllProv,provNumList,dateMin,dateMax,isPreauth,listClinicNums);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Carrier"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Phone"),103);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient Name"),140);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g(this,"Clinic"),100);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Date of Serv"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date Sent"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Track Status"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date Status"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),85,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			string type;
			total=0;
			for(int i=0;i<Table.Rows.Count;i++){
				DateTime dateLastLog=PIn.Date(Table.Rows[i]["DateLog"].ToString());
				int daysSuppressed=PIn.Int(Table.Rows[i]["DaysSuppressed"].ToString());
				if(!checkIgnoreCustom.Checked && dateLastLog.AddDays(daysSuppressed)>DateTime.Today) {
					continue;
				}
				long customTrackDefNum=PIn.Long(Table.Rows[i]["DefNum"].ToString());
				if(comboLastClaimTrack.SelectedIndex!=0 && customTrackDefNum!=DefC.GetList(DefCat.ClaimCustomTracking)[comboLastClaimTrack.SelectedIndex-1].DefNum) {
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(Table.Rows[i]["CarrierName"].ToString());
				row.Cells.Add(Table.Rows[i]["Phone"].ToString());
				type=Table.Rows[i]["ClaimType"].ToString();
				switch(type){
					case "P":
						type="Primary";
						break;
					case "S":
						type="Secondary";
						break;
					case "PreAuth":
						type="Preauth";
						break;
					case "Other":
						type="Other";
						break;
					case "Cap":
						type="Capitation";
						break;
					case "Med":
						type="Medical";//For possible future use.
						break;
					default:
						type="Error";//Not allowed to be blank.
						break;
				}
				row.Cells.Add(type);
				if(PrefC.GetBool(PrefName.ReportsShowPatNum)) {
					row.Cells.Add(Table.Rows[i]["PatNum"].ToString()+"-"+Table.Rows[i]["LName"].ToString()+", "+Table.Rows[i]["FName"].ToString()+" "+Table.Rows[i]["MiddleI"].ToString());
				}
				else {
					row.Cells.Add(Table.Rows[i]["LName"].ToString()+", "+Table.Rows[i]["FName"].ToString()+" "+Table.Rows[i]["MiddleI"].ToString());
				}
				if(PrefC.HasClinicsEnabled) {
					string clinicName="Unassigned";
					for(int j=0;j<_listClinics.Count;j++) {
						if(_listClinics[j].ClinicNum==PIn.Long(Table.Rows[i]["ClinicNum"].ToString())) {
							clinicName=_listClinics[j].Description;
							break;
						}
					}
					row.Cells.Add(clinicName);
				}
				DateTime dateService=PIn.Date(Table.Rows[i]["DateService"].ToString());
				if(dateService.Year<1880) {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(dateService.ToShortDateString());
				}
				row.Cells.Add(PIn.Date(Table.Rows[i]["DateSent"].ToString()).ToShortDateString());
				if(customTrackDefNum==0) {
					row.Cells.Add("-");
				}
				else {
					row.Cells.Add(DefC.GetList(DefCat.ClaimCustomTracking).First(x => x.DefNum==customTrackDefNum).ItemName);
				}
				DateTime dateLog=PIn.Date(Table.Rows[i]["DateLog"].ToString());
				if(dateLog.Year<1880) {
					row.Cells.Add("-");
				}
				else { 
					row.Cells.Add(dateLog.ToShortDateString());
				}
				row.Cells.Add(PIn.Double(Table.Rows[i]["ClaimFee"].ToString()).ToString("c"));
				gridMain.Rows.Add(row);
				total+=PIn.Decimal(Table.Rows[i]["ClaimFee"].ToString());
			}
			textBox1.Text=total.ToString("c");
			gridMain.EndUpdate();
		}

		private void listProv_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			Plugins.HookAddCode(this,"FormRpOutstandingIns.butRefresh_begin");
			FillGrid();
		}

		private void textDaysOldMin_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkPreauth_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkIgnoreCustom_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDaysOldMax_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboBoxMultiProv_Leave(object sender,EventArgs e) {
			for(int i=0;i<comboBoxMultiProv.SelectedIndices.Count;i++) {
				if(comboBoxMultiProv.SelectedIndices[i].ToString()=="0") {
					comboBoxMultiProv.SelectedIndices.Clear();
					comboBoxMultiProv.SetSelected(0,true);
					comboBoxMultiProv.RefreshText();
				}
			}
			if(comboBoxMultiProv.SelectedIndices.Count==0) {
				comboBoxMultiProv.SelectedIndices.Clear();
				comboBoxMultiProv.SetSelected(0,true);
				comboBoxMultiProv.RefreshText();
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			GotoModule.GotoAccount(PIn.Long(Table.Rows[e.Row]["PatNum"].ToString()));
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Claim claim=Claims.GetClaim(PIn.Long(Table.Rows[e.Row]["ClaimNum"].ToString()));
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormClaimEdit FormCE=new FormClaimEdit(claim,pat,fam);
			FormCE.IsNew=false;
			FormCE.ShowDialog();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			////Validating of parameters is done during RefreshGrid().
			//ReportSimpleGrid report=new ReportSimpleGrid();
			//report.Query = "SELECT carrier.CarrierName,patient.HmPhone,claim.ClaimType,patient.FName,patient.LName,patient.MiddleI,patient.PatNum,claim.DateService,claim.DateSent,claim.ClaimFee,claim.ClaimNum "
			//  +"FROM carrier,patient,claim,insplan "
			//  +"WHERE carrier.CarrierNum = insplan.CarrierNum "
			//  +"AND claim.PlanNum = insplan.PlanNum "
			//  +"AND claim.PatNum = patient.PatNum "
			//  +"AND claim.ClaimStatus='S' ";
			//if(dateMin!=DateTime.MinValue) {
			//  report.Query+="AND claim.DateSent <= "+POut.Date(dateMin)+" ";
			//}
			//if(dateMax!=DateTime.MinValue) {
			//  report.Query+="AND claim.DateSent >= "+POut.Date(dateMax)+" ";
			//}
			//if(!isAllProv) {
			//  if(provNumList.Count>0) {
			//    report.Query+="AND claim.ProvBill IN (";
			//    report.Query+=""+provNumList[0];
			//    for(int i=1;i<provNumList.Count;i++) {
			//      report.Query+=","+provNumList[i];
			//    }
			//    report.Query+=") ";
			//  }
			//}
			//if(!isPreauth) {
			//  report.Query+="AND claim.ClaimType!='Preauth' ";
			//}
			//report.Query+="ORDER BY carrier.Phone,insplan.PlanNum, carrier.Phone,insplan.PlanNum";
			//FormQuery FormQuery2=new FormQuery(report);
			//FormQuery2.IsReport=true;
			//DataTable tableTemp= report.GetTempTable();
			//report.TableQ=new DataTable(null);//new table no name
			//for(int i=0;i<6;i++) {//add columns
			//  report.TableQ.Columns.Add(new System.Data.DataColumn());//blank columns
			//}
			//report.InitializeColumns();
			//for(int i=0;i<tableTemp.Rows.Count;i++) {//loop through data rows
			//  DataRow row = report.TableQ.NewRow();//create new row called 'row' based on structure of TableQ
			//  //start filling 'row'. First column is carrier:
			//  row[0]=tableTemp.Rows[i][0];
			//  row[1]=tableTemp.Rows[i][7];
			//  if(PIn.String(tableTemp.Rows[i][2].ToString())=="P")
			//    row[2]="Primary";
			//  if(PIn.String(tableTemp.Rows[i][2].ToString())=="S")
			//    row[2]="Secondary";
			//  if(PIn.String(tableTemp.Rows[i][2].ToString())=="PreAuth")
			//    row[2]="PreAuth";
			//  if(PIn.String(tableTemp.Rows[i][2].ToString())=="Other")
			//    row[2]="Other";
			//  row[3]=tableTemp.Rows[i][4];
			//  row[4]=(PIn.Date(tableTemp.Rows[i][3].ToString())).ToString("d");
			//  row[5]=PIn.Double(tableTemp.Rows[i][6].ToString()).ToString("F");
			//  //TimeSpan d = DateTime.Today.Subtract((PIn.PDate(tableTemp.Rows[i][5].ToString())));
			//  //if(d.Days>5000)
			//  //	row[4]="";
			//  //else
			//  //	row[4]=d.Days.ToString();
			//  report.ColTotal[5]+=PIn.Double(tableTemp.Rows[i][6].ToString());
			//  report.TableQ.Rows.Add(row);
			//}
			//FormQuery2.ResetGrid();//this is a method in FormQuery;
			//report.Title="OUTSTANDING INSURANCE CLAIMS";
			//report.SubTitle.Add(PrefC.GetString(PrefName.PracticeTitle));
			//report.SubTitle.Add("Sent before "+dateMin.Date.ToShortDateString());
			//report.ColPos[0]=20;
			//report.ColPos[1]=210;
			//report.ColPos[2]=330;
			//report.ColPos[3]=430;
			//report.ColPos[4]=600;
			//report.ColPos[5]=690;
			//report.ColPos[6]=770;
			//report.ColCaption[0]=Lan.g(this,"Carrier");
			//report.ColCaption[1]=Lan.g(this,"Phone");
			//report.ColCaption[2]=Lan.g(this,"Type");
			//report.ColCaption[3]=Lan.g(this,"Patient Name");
			//report.ColCaption[4]=Lan.g(this,"Date of Service");
			//report.ColCaption[5]=Lan.g(this,"Amount");
			//report.ColAlign[5]=HorizontalAlignment.Right;
			//FormQuery2.ShowDialog();
			//DialogResult=DialogResult.OK;
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Landscape=false;
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
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Outstanding insurance report printed")) {
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
				text=Lan.g(this,"Outstanding Insurance Claims");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(isPreauth) {
					text="Including Preauthorization";
				}
				else {
					text="Not Including Preauthorization";
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				if(isAllProv) {
					text="For All Providers";
				}
				else {
					text="For Providers: ";
					for(int i=0;i<provNumList.Count;i++) {
						text+=Providers.GetFormalName(provNumList[i]);
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
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
				text="Total: $"+total.ToString("F");
				g.DrawString(text,subHeadingFont,Brushes.Black,center+gridMain.Width/2-g.MeasureString(text,subHeadingFont).Width-10,yPos);
			}
			g.Dispose();
		}

		private void butExport_Click(object sender,System.EventArgs e) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName="Outstanding Insurance Claims";
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw=new StreamWriter(saveFileDialog.FileName,false))
				//new FileStream(,FileMode.Create,FileAccess.Write,FileShare.Read)))
				{
					String line="";
					line+=Lan.g(this,"Carrier")+"\t"+Lan.g(this,"Phone")+"\t"+Lan.g(this,"Type")+"\t"+Lan.g(this,"Patient Name")+"\t"+Lan.g(this,"Date of Service")+"\t"+Lan.g(this,"Date Sent")+"\t"+Lan.g(this,"Amount");
					sw.WriteLine(line);
					for(int i=0;i<gridMain.Rows.Count;i++) {
						line="";
						for(int j=0;j<gridMain.Columns.Count;j++) {
							line+=gridMain.Rows[i].Cells[j].Text;
							if(j<gridMain.Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}
	}
}