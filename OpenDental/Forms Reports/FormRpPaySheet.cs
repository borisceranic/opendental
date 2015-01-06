using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.ReportingComplex;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormRpPaySheet : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.MonthCalendar date2;
		private System.Windows.Forms.MonthCalendar date1;
		private System.Windows.Forms.Label labelTO;
		private System.ComponentModel.Container components = null;
		private ListBox listProv;
		private Label label1;
		private GroupBox groupBox1;
		private RadioButton radioPatient;
		private RadioButton radioCheck;
		private CheckBox checkPatientTypes;
		private ListBox listPatientTypes;
		private CheckBox checkInsuranceTypes;
		private CheckBox checkAllClin;
		private ListBox listClin;
		private Label labelClin;
		private Label label2;
		private ListBox listInsuranceTypes;
		private CheckBox checkAllProv;
		//private FormQuery FormQuery2;

		///<summary></summary>
		public FormRpPaySheet(){
			InitializeComponent();
 			Lan.F(this);
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpPaySheet));
			this.date2 = new System.Windows.Forms.MonthCalendar();
			this.date1 = new System.Windows.Forms.MonthCalendar();
			this.labelTO = new System.Windows.Forms.Label();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkAllProv = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.radioPatient = new System.Windows.Forms.RadioButton();
			this.radioCheck = new System.Windows.Forms.RadioButton();
			this.checkPatientTypes = new System.Windows.Forms.CheckBox();
			this.listPatientTypes = new System.Windows.Forms.ListBox();
			this.checkInsuranceTypes = new System.Windows.Forms.CheckBox();
			this.checkAllClin = new System.Windows.Forms.CheckBox();
			this.listClin = new System.Windows.Forms.ListBox();
			this.labelClin = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listInsuranceTypes = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// date2
			// 
			this.date2.Location = new System.Drawing.Point(285, 36);
			this.date2.Name = "date2";
			this.date2.TabIndex = 2;
			// 
			// date1
			// 
			this.date1.Location = new System.Drawing.Point(31, 36);
			this.date1.Name = "date1";
			this.date1.TabIndex = 1;
			// 
			// labelTO
			// 
			this.labelTO.Location = new System.Drawing.Point(212, 44);
			this.labelTO.Name = "labelTO";
			this.labelTO.Size = new System.Drawing.Size(72, 23);
			this.labelTO.TabIndex = 22;
			this.labelTO.Text = "TO";
			this.labelTO.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(524, 54);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(163, 199);
			this.listProv.TabIndex = 36;
			this.listProv.Click += new System.EventHandler(this.listProv_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(522, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 35;
			this.label1.Text = "Providers";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkAllProv
			// 
			this.checkAllProv.Checked = true;
			this.checkAllProv.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllProv.Location = new System.Drawing.Point(525, 34);
			this.checkAllProv.Name = "checkAllProv";
			this.checkAllProv.Size = new System.Drawing.Size(95, 16);
			this.checkAllProv.TabIndex = 43;
			this.checkAllProv.Text = "All";
			this.checkAllProv.Click += new System.EventHandler(this.checkAllProv_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.radioPatient);
			this.groupBox1.Controls.Add(this.radioCheck);
			this.groupBox1.Location = new System.Drawing.Point(31, 263);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(173, 93);
			this.groupBox1.TabIndex = 44;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group By";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(163, 29);
			this.label2.TabIndex = 55;
			this.label2.Text = "Either way, provider splits will still show separately.";
			// 
			// radioPatient
			// 
			this.radioPatient.Location = new System.Drawing.Point(8, 35);
			this.radioPatient.Name = "radioPatient";
			this.radioPatient.Size = new System.Drawing.Size(104, 18);
			this.radioPatient.TabIndex = 1;
			this.radioPatient.Text = "Patient";
			this.radioPatient.UseVisualStyleBackColor = true;
			// 
			// radioCheck
			// 
			this.radioCheck.Checked = true;
			this.radioCheck.Location = new System.Drawing.Point(8, 16);
			this.radioCheck.Name = "radioCheck";
			this.radioCheck.Size = new System.Drawing.Size(104, 18);
			this.radioCheck.TabIndex = 0;
			this.radioCheck.TabStop = true;
			this.radioCheck.Text = "Check";
			this.radioCheck.UseVisualStyleBackColor = true;
			// 
			// checkPatientTypes
			// 
			this.checkPatientTypes.Checked = true;
			this.checkPatientTypes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkPatientTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientTypes.Location = new System.Drawing.Point(382, 263);
			this.checkPatientTypes.Name = "checkPatientTypes";
			this.checkPatientTypes.Size = new System.Drawing.Size(166, 16);
			this.checkPatientTypes.TabIndex = 47;
			this.checkPatientTypes.Text = "All patient payment types";
			this.checkPatientTypes.Click += new System.EventHandler(this.checkAllTypes_Click);
			// 
			// listPatientTypes
			// 
			this.listPatientTypes.Location = new System.Drawing.Point(382, 285);
			this.listPatientTypes.Name = "listPatientTypes";
			this.listPatientTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listPatientTypes.Size = new System.Drawing.Size(163, 186);
			this.listPatientTypes.TabIndex = 46;
			// 
			// checkInsuranceTypes
			// 
			this.checkInsuranceTypes.Checked = true;
			this.checkInsuranceTypes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkInsuranceTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsuranceTypes.Location = new System.Drawing.Point(210, 263);
			this.checkInsuranceTypes.Name = "checkInsuranceTypes";
			this.checkInsuranceTypes.Size = new System.Drawing.Size(166, 16);
			this.checkInsuranceTypes.TabIndex = 48;
			this.checkInsuranceTypes.Text = "All insurance payment types";
			this.checkInsuranceTypes.Click += new System.EventHandler(this.checkIns_Click);
			// 
			// checkAllClin
			// 
			this.checkAllClin.Checked = true;
			this.checkAllClin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllClin.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClin.Location = new System.Drawing.Point(554, 305);
			this.checkAllClin.Name = "checkAllClin";
			this.checkAllClin.Size = new System.Drawing.Size(95, 16);
			this.checkAllClin.TabIndex = 54;
			this.checkAllClin.Text = "All";
			this.checkAllClin.Click += new System.EventHandler(this.checkAllClin_Click);
			// 
			// listClin
			// 
			this.listClin.Location = new System.Drawing.Point(554, 324);
			this.listClin.Name = "listClin";
			this.listClin.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClin.Size = new System.Drawing.Size(154, 147);
			this.listClin.TabIndex = 53;
			this.listClin.Click += new System.EventHandler(this.listClin_Click);
			// 
			// labelClin
			// 
			this.labelClin.Location = new System.Drawing.Point(551, 287);
			this.labelClin.Name = "labelClin";
			this.labelClin.Size = new System.Drawing.Size(104, 16);
			this.labelClin.TabIndex = 52;
			this.labelClin.Text = "Clinics";
			this.labelClin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
			this.butCancel.Location = new System.Drawing.Point(714, 445);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(714, 410);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listInsuranceTypes
			// 
			this.listInsuranceTypes.Location = new System.Drawing.Point(210, 285);
			this.listInsuranceTypes.Name = "listInsuranceTypes";
			this.listInsuranceTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listInsuranceTypes.Size = new System.Drawing.Size(163, 186);
			this.listInsuranceTypes.TabIndex = 55;
			// 
			// FormRpPaySheet
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(818, 495);
			this.Controls.Add(this.listInsuranceTypes);
			this.Controls.Add(this.checkAllClin);
			this.Controls.Add(this.listClin);
			this.Controls.Add(this.labelClin);
			this.Controls.Add(this.checkInsuranceTypes);
			this.Controls.Add(this.checkPatientTypes);
			this.Controls.Add(this.listPatientTypes);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkAllProv);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.date2);
			this.Controls.Add(this.date1);
			this.Controls.Add(this.labelTO);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpPaySheet";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Daily Payments Report";
			this.Load += new System.EventHandler(this.FormPaymentSheet_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPaymentSheet_Load(object sender, System.EventArgs e) {
			date1.SelectionStart=DateTime.Today;
			date2.SelectionStart=DateTime.Today;
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				listProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				listClin.Visible=false;
				labelClin.Visible=false;
				checkAllClin.Visible=false;
			}
			else {
				listClin.Items.Add(Lan.g(this,"Unassigned"));
				for(int i=0;i<Clinics.List.Length;i++) {
					listClin.Items.Add(Clinics.List[i].Description);
				}
			}
			for(int i=0;i<DefC.Short[(int)DefCat.PaymentTypes].Length;i++) {
				listPatientTypes.Items.Add(DefC.Short[(int)DefCat.PaymentTypes][i].ItemName);
			}
			listPatientTypes.Visible=false;
			for(int i=0;i<DefC.Short[(int)DefCat.InsurancePaymentType].Length;i++) {
				listInsuranceTypes.Items.Add(DefC.Short[(int)DefCat.InsurancePaymentType][i].ItemName);
			}
			listInsuranceTypes.Visible=false;
		}

		private void checkAllProv_Click(object sender,EventArgs e) {
			if(checkAllProv.Checked) {
				listProv.SelectedIndices.Clear();
			}
		}

		private void listProv_Click(object sender,EventArgs e) {
			if(listProv.SelectedIndices.Count>0) {
				checkAllProv.Checked=false;
			}
		}

		private void checkAllClin_Click(object sender,EventArgs e) {
			if(checkAllClin.Checked) {
				listClin.SelectedIndices.Clear();
			}
		}

		private void listClin_Click(object sender,EventArgs e) {
			if(listClin.SelectedIndices.Count>0) {
				checkAllClin.Checked=false;
			}
		}

		private void checkAllTypes_Click(object sender,EventArgs e) {
			if(checkPatientTypes.Checked){
				listPatientTypes.Visible=false;
			}
			else{
				listPatientTypes.Visible=true;
			}
		}

		private void checkIns_Click(object sender,EventArgs e) {
			if(checkInsuranceTypes.Checked) {
				listInsuranceTypes.Visible=false;
			}
			else {
				listInsuranceTypes.Visible=true;
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!checkAllProv.Checked && listProv.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(!checkAllClin.Checked && listClin.SelectedIndices.Count==0) {
					MsgBox.Show(this,"At least one clinic must be selected.");
					return;
				}
			}
			if(!checkPatientTypes.Checked && listPatientTypes.SelectedIndices.Count==0 && !checkInsuranceTypes.Checked && listInsuranceTypes.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one type must be selected.");
				return;
			}
			string whereProv="";
			if(!checkAllProv.Checked) {
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i==0) {
						whereProv+=" AND (";
					}
					else {
						whereProv+="OR ";
					}
					whereProv+="claimproc.ProvNum = "+POut.Long(ProviderC.ListShort[listProv.SelectedIndices[i]].ProvNum)+" ";
				}
				whereProv+=") ";
			}
			string whereClin="";
			if(!checkAllClin.Checked) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(i==0) {
						whereClin+=" AND (";
					}
					else {
						whereClin+="OR ";
					}
					if(listClin.SelectedIndices[i]==0) {
						whereClin+="claimproc.ClinicNum = 0 ";
					}
					else {
						whereClin+="claimproc.ClinicNum = "+POut.Long(Clinics.List[listClin.SelectedIndices[i]-1].ClinicNum)+" ";
					}
				}
				whereClin+=") ";
			}
			string queryIns=
				@"SELECT CONVERT("+DbHelper.DateFormatColumn("claimproc.DateCP","%c/%d/%Y")+",CHAR(25)) DateCP,carrier.CarrierName,MAX("
+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+@") lfname,
provider.Abbr,
clinic.Description clinicDesc,
claimpayment.CheckNum,FORMAT(SUM(claimproc.InsPayAmt),2) amt,claimproc.ClaimNum,claimpayment.PayType 
FROM claimproc
LEFT JOIN insplan ON claimproc.PlanNum = insplan.PlanNum 
LEFT JOIN patient ON claimproc.PatNum = patient.PatNum
LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum
LEFT JOIN provider ON provider.ProvNum=claimproc.ProvNum
LEFT JOIN claimpayment ON claimproc.ClaimPaymentNum = claimpayment.ClaimPaymentNum
LEFT JOIN clinic ON clinic.ClinicNum=claimproc.ClinicNum
WHERE (claimproc.Status=1 OR claimproc.Status=4) "//received or supplemental
				+whereProv
				+whereClin
				+"AND claimpayment.CheckDate >= "+POut.Date(date1.SelectionStart)+" "
				+"AND claimpayment.CheckDate <= "+POut.Date(date2.SelectionStart)+" ";
			if(!checkInsuranceTypes.Checked && listInsuranceTypes.SelectedIndices.Count>0) {
				queryIns+="AND claimpayment.PayType IN (";
				for(int i=0;i<listInsuranceTypes.SelectedIndices.Count;i++) {
					if(i>0) {
						queryIns+=",";
					}
					queryIns+=POut.Long(DefC.Short[(int)DefCat.InsurancePaymentType][listInsuranceTypes.SelectedIndices[i]].DefNum);
				}
				queryIns+=") ";
			}
			queryIns+=@"GROUP BY CONVERT("+DbHelper.DateFormatColumn("claimproc.DateCP","%c/%d/%Y")+@",CHAR(25)),
claimproc.ClaimPaymentNum,provider.ProvNum,
claimproc.ClinicNum,carrier.CarrierName,provider.Abbr,
clinic.Description,claimpayment.CheckNum";
			if(radioPatient.Checked) {
				queryIns+=",patient.PatNum";
			}
			queryIns+=" ORDER BY claimpayment.PayType,claimproc.DateCP,lfname";
			if(!checkInsuranceTypes.Checked && listInsuranceTypes.SelectedIndices.Count==0) {
				queryIns=DbHelper.LimitOrderBy(queryIns,0);
			}
			//patient payments-----------------------------------------------------------------------------------------
			whereProv="";
			if(!checkAllProv.Checked) {
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i==0) {
						whereProv+=" AND (";
					}
					else {
						whereProv+="OR ";
					}
					whereProv+="paysplit.ProvNum = "+POut.Long(ProviderC.ListShort[listProv.SelectedIndices[i]].ProvNum)+" ";
				}
				whereProv+=") ";
			}
			whereClin="";
			if(!checkAllClin.Checked) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(i==0) {
						whereClin+=" AND (";
					}
					else {
						whereClin+="OR ";
					}
					if(listClin.SelectedIndices[i]==0) {
						whereClin+="payment.ClinicNum = 0 ";
					}
					else {
						whereClin+="payment.ClinicNum = "+POut.Long(Clinics.List[listClin.SelectedIndices[i]-1].ClinicNum)+" ";
					}
				}
				whereClin+=") ";
			}
			string queryPat=
				@"SELECT CONVERT("+DbHelper.DateFormatColumn("payment.PayDate","%c/%d/%Y")+",CHAR(25)) AS DatePay,MAX("
+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+@") AS lfname,provider.Abbr,
clinic.Description clinicDesc,
payment.CheckNum,
FORMAT(SUM(paysplit.SplitAmt),2) amt, payment.PayNum,ItemName,payment.PayType 
FROM payment
LEFT JOIN paysplit ON payment.PayNum=paysplit.PayNum
LEFT JOIN patient ON payment.PatNum=patient.PatNum
LEFT JOIN provider ON paysplit.ProvNum=provider.ProvNum
LEFT JOIN definition ON payment.PayType=definition.DefNum 
LEFT JOIN clinic ON payment.ClinicNum=clinic.ClinicNum
WHERE 1 "
				+whereProv
				+whereClin
				+"AND paysplit.DatePay >= "+POut.Date(date1.SelectionStart)+" "
				+"AND paysplit.DatePay <= "+POut.Date(date2.SelectionStart)+" ";
			if(!checkPatientTypes.Checked && listPatientTypes.SelectedIndices.Count>0) {
				queryPat+="AND (";
				for(int i=0;i<listPatientTypes.SelectedIndices.Count;i++) {
					if(i>0) {
						queryPat+="OR ";
					}
					queryPat+="payment.PayType = "+POut.Long(DefC.Short[(int)DefCat.PaymentTypes][listPatientTypes.SelectedIndices[i]].DefNum)+" ";
				}
				queryPat+=") ";
			}
			queryPat+=@"GROUP BY "
				+"payment.PayNum,payment.PayDate,provider.ProvNum,payment.ClinicNum"
				+",provider.Abbr,clinic.Description,payment.CheckNum,definition.ItemName";
			if(radioPatient.Checked) {
				queryPat+=",patient.PatNum";
			}
			queryPat+=" ORDER BY payment.PayType,paysplit.DatePay,lfname";
			if(!checkPatientTypes.Checked && listPatientTypes.SelectedIndices.Count==0) {
				queryPat=DbHelper.LimitOrderBy(queryPat,0);
			}
			DataTable tableIns=Reports.GetTable(queryIns);
			DataTable tablePat=Reports.GetTable(queryPat);
			string subtitleProvs="";
			string subtitleClinics="";
			if(checkAllProv.Checked) {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			else {
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i>0) {
						subtitleProvs+=", ";
					}
					subtitleProvs+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
				}
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(checkAllClin.Checked) {
					subtitleClinics=Lan.g(this,"All Clinics");
				}
				else {
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(listClin.SelectedIndices[i]==0) {
							subtitleClinics+=Lan.g(this,"Unassigned");
						}
						else {
							subtitleClinics+=Clinics.List[listClin.SelectedIndices[i]-1].Description;
						}
					}
				}
			}
			Font font=new Font("Tahoma",9);
			Font fontBold=new Font("Tahoma",9,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			ReportComplex report=new ReportComplex(true,false);
			report.ReportName=Lan.g(this,"Daily Payments");
			report.AddTitle("Title",Lan.g(this,"Daily Payments"),fontTitle);
			report.AddSubTitle("Providers",subtitleProvs,fontSubTitle);
			report.AddSubTitle("Clinics",subtitleClinics,fontSubTitle);
			Dictionary<long,string> dictInsDefNames=new Dictionary<long,string>();
			Dictionary<long,string> dictPatDefNames=new Dictionary<long,string>();
			List<Def> insDefs=new List<Def>(DefC.GetList(DefCat.InsurancePaymentType));
			List<Def> patDefs=new List<Def>(DefC.GetList(DefCat.PaymentTypes));
			for(int i=0;i<insDefs.Count;i++) {
				dictInsDefNames.Add(insDefs[i].DefNum,insDefs[i].ItemName);
			}
			for(int i=0;i<patDefs.Count;i++) {
				dictPatDefNames.Add(patDefs[i].DefNum,patDefs[i].ItemName);
			}
			int[] summaryGroups1= { 1 };
			int[] summaryGroups2= { 2 };
			int[] summaryGroups3= { 1,2 };
			QueryObject query=report.AddQuery(tableIns,"Insurance Payments","PayType",SplitByKind.Definition,1,true,dictInsDefNames,fontSubTitle);
			query.AddColumn("Date",90,FieldValueType.Date,font);
			//query.GetColumnDetail("Date").SuppressIfDuplicate = true;
			query.GetColumnDetail("Date").StringFormat="d";
			query.AddColumn("Carrier",150,FieldValueType.String,font);
			query.AddColumn("Patient Name",150,FieldValueType.String,font);
			query.AddColumn("Provider",90,FieldValueType.String,font);
			query.AddColumn("Clinic",120,FieldValueType.String,font);
			query.AddColumn("Check#",75,FieldValueType.String,font);
			query.AddColumn("Amount",90,FieldValueType.Number,font);
			query.AddGroupSummaryField("Total Insurance Payments:",Color.Black,"Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroups1),fontBold,0,10);
			query=report.AddQuery(tablePat,"Patient Payments","PayType",SplitByKind.Definition,2,true,dictPatDefNames,fontSubTitle);
			query.AddColumn("Date",90,FieldValueType.Date,font);
			//query.GetColumnDetail("Date").SuppressIfDuplicate = true;
			query.GetColumnDetail("Date").StringFormat="d";
			query.AddColumn("Patient Name",270,FieldValueType.String,font);
			query.AddColumn("Provider",90,FieldValueType.String,font);
			query.AddColumn("Clinic",120,FieldValueType.String,font);
			query.AddColumn("Check#",75,FieldValueType.String,font);
			query.AddColumn("Amount",120,FieldValueType.Number,font);
			query.AddGroupSummaryField("Total Patient Payments:",Color.Black,"Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroups2),fontBold,0,10);
			query.AddGroupSummaryField("Total All Payments:",Color.Black,"Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroups3),fontBold,0,10);
			report.AddPageNum(font);
			report.AddGridLines();
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;		
		}

		

		

		

		

		

	}
}
