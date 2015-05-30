using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	///<summary></summary>
	public class FormRpAging : System.Windows.Forms.Form{
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private FormQuery FormQuery2;
		private System.Windows.Forms.Label label1;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.ListBox listBillType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radio30;
		private System.Windows.Forms.RadioButton radio90;
		private System.Windows.Forms.RadioButton radio60;
		private System.Windows.Forms.CheckBox checkIncludeNeg;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkOnlyNeg;
		private System.Windows.Forms.CheckBox checkExcludeInactive;
		private ListBox listProv;
		private Label label3;
		private CheckBox checkProvAll;
		private CheckBox checkBillTypesAll;
		private CheckBox checkBadAddress;
		private CheckBox checkAllClin;
		private ListBox listClin;
		private Label labelClin;
		private System.Windows.Forms.RadioButton radioAny;
		private List<Clinic> _listClinics;

		///<summary></summary>
		public FormRpAging(){
			InitializeComponent();
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpAging));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radio30 = new System.Windows.Forms.RadioButton();
			this.radio90 = new System.Windows.Forms.RadioButton();
			this.radio60 = new System.Windows.Forms.RadioButton();
			this.radioAny = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.listBillType = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkIncludeNeg = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkOnlyNeg = new System.Windows.Forms.CheckBox();
			this.checkExcludeInactive = new System.Windows.Forms.CheckBox();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkProvAll = new System.Windows.Forms.CheckBox();
			this.checkBillTypesAll = new System.Windows.Forms.CheckBox();
			this.checkBadAddress = new System.Windows.Forms.CheckBox();
			this.checkAllClin = new System.Windows.Forms.CheckBox();
			this.listClin = new System.Windows.Forms.ListBox();
			this.labelClin = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(744, 396);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(744, 362);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radio30);
			this.groupBox1.Controls.Add(this.radio90);
			this.groupBox1.Controls.Add(this.radio60);
			this.groupBox1.Controls.Add(this.radioAny);
			this.groupBox1.Location = new System.Drawing.Point(57, 109);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(175, 120);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Age of Account";
			// 
			// radio30
			// 
			this.radio30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio30.Location = new System.Drawing.Point(12, 44);
			this.radio30.Name = "radio30";
			this.radio30.Size = new System.Drawing.Size(152, 16);
			this.radio30.TabIndex = 1;
			this.radio30.Text = "Over 30 Days";
			// 
			// radio90
			// 
			this.radio90.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio90.Location = new System.Drawing.Point(12, 90);
			this.radio90.Name = "radio90";
			this.radio90.Size = new System.Drawing.Size(152, 18);
			this.radio90.TabIndex = 3;
			this.radio90.Text = "Over 90 Days";
			// 
			// radio60
			// 
			this.radio60.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio60.Location = new System.Drawing.Point(12, 66);
			this.radio60.Name = "radio60";
			this.radio60.Size = new System.Drawing.Size(152, 18);
			this.radio60.TabIndex = 2;
			this.radio60.Text = "Over 60 Days";
			// 
			// radioAny
			// 
			this.radioAny.Checked = true;
			this.radioAny.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAny.Location = new System.Drawing.Point(12, 20);
			this.radioAny.Name = "radioAny";
			this.radioAny.Size = new System.Drawing.Size(152, 18);
			this.radioAny.TabIndex = 0;
			this.radioAny.TabStop = true;
			this.radioAny.Text = "Any Balance";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 14);
			this.label1.TabIndex = 11;
			this.label1.Text = "As Of Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(126, 48);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(106, 20);
			this.textDate.TabIndex = 0;
			// 
			// listBillType
			// 
			this.listBillType.Location = new System.Drawing.Point(335, 69);
			this.listBillType.Name = "listBillType";
			this.listBillType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillType.Size = new System.Drawing.Size(158, 186);
			this.listBillType.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(332, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Billing Types";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkIncludeNeg
			// 
			this.checkIncludeNeg.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeNeg.Location = new System.Drawing.Point(9, 28);
			this.checkIncludeNeg.Name = "checkIncludeNeg";
			this.checkIncludeNeg.Size = new System.Drawing.Size(194, 20);
			this.checkIncludeNeg.TabIndex = 17;
			this.checkIncludeNeg.Text = "Include negative balances";
			this.checkIncludeNeg.Click += new System.EventHandler(this.checkIncludeNeg_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkOnlyNeg);
			this.groupBox2.Controls.Add(this.checkIncludeNeg);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(57, 256);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(225, 84);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Negative Balances";
			// 
			// checkOnlyNeg
			// 
			this.checkOnlyNeg.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkOnlyNeg.Location = new System.Drawing.Point(9, 55);
			this.checkOnlyNeg.Name = "checkOnlyNeg";
			this.checkOnlyNeg.Size = new System.Drawing.Size(210, 19);
			this.checkOnlyNeg.TabIndex = 18;
			this.checkOnlyNeg.Text = "Only show negative balances";
			this.checkOnlyNeg.Click += new System.EventHandler(this.checkOnlyNeg_Click);
			// 
			// checkExcludeInactive
			// 
			this.checkExcludeInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInactive.Location = new System.Drawing.Point(66, 363);
			this.checkExcludeInactive.Name = "checkExcludeInactive";
			this.checkExcludeInactive.Size = new System.Drawing.Size(248, 18);
			this.checkExcludeInactive.TabIndex = 19;
			this.checkExcludeInactive.Text = "Exclude inactive patients";
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(499, 69);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(163, 186);
			this.listProv.TabIndex = 39;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(496, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 38;
			this.label3.Text = "Providers";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkProvAll
			// 
			this.checkProvAll.Checked = true;
			this.checkProvAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkProvAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProvAll.Location = new System.Drawing.Point(499, 46);
			this.checkProvAll.Name = "checkProvAll";
			this.checkProvAll.Size = new System.Drawing.Size(145, 18);
			this.checkProvAll.TabIndex = 40;
			this.checkProvAll.Text = "All";
			this.checkProvAll.Click += new System.EventHandler(this.checkProvAll_Click);
			// 
			// checkBillTypesAll
			// 
			this.checkBillTypesAll.Checked = true;
			this.checkBillTypesAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBillTypesAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBillTypesAll.Location = new System.Drawing.Point(335, 46);
			this.checkBillTypesAll.Name = "checkBillTypesAll";
			this.checkBillTypesAll.Size = new System.Drawing.Size(145, 18);
			this.checkBillTypesAll.TabIndex = 41;
			this.checkBillTypesAll.Text = "All";
			this.checkBillTypesAll.Click += new System.EventHandler(this.checkBillTypesAll_Click);
			// 
			// checkBadAddress
			// 
			this.checkBadAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBadAddress.Location = new System.Drawing.Point(66, 390);
			this.checkBadAddress.Name = "checkBadAddress";
			this.checkBadAddress.Size = new System.Drawing.Size(248, 18);
			this.checkBadAddress.TabIndex = 43;
			this.checkBadAddress.Text = "Exclude bad addresses (no zipcode)";
			// 
			// checkAllClin
			// 
			this.checkAllClin.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClin.Location = new System.Drawing.Point(668, 46);
			this.checkAllClin.Name = "checkAllClin";
			this.checkAllClin.Size = new System.Drawing.Size(95, 16);
			this.checkAllClin.TabIndex = 51;
			this.checkAllClin.Text = "All";
			this.checkAllClin.Click += new System.EventHandler(this.checkAllClin_Click);
			// 
			// listClin
			// 
			this.listClin.Location = new System.Drawing.Point(668, 69);
			this.listClin.Name = "listClin";
			this.listClin.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClin.Size = new System.Drawing.Size(163, 186);
			this.listClin.TabIndex = 50;
			this.listClin.Click += new System.EventHandler(this.listClin_Click);
			// 
			// labelClin
			// 
			this.labelClin.Location = new System.Drawing.Point(665, 25);
			this.labelClin.Name = "labelClin";
			this.labelClin.Size = new System.Drawing.Size(104, 16);
			this.labelClin.TabIndex = 49;
			this.labelClin.Text = "Clinics";
			this.labelClin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormRpAging
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(849, 450);
			this.Controls.Add(this.checkAllClin);
			this.Controls.Add(this.listClin);
			this.Controls.Add(this.labelClin);
			this.Controls.Add(this.checkBadAddress);
			this.Controls.Add(this.checkBillTypesAll);
			this.Controls.Add(this.checkProvAll);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkExcludeInactive);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listBillType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpAging";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Aging of Accounts Receivable Report";
			this.Load += new System.EventHandler(this.FormAging_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAging_Load(object sender, System.EventArgs e) {
			DateTime lastAgingDate=PrefC.GetDate(PrefName.DateLastAging);
			if(lastAgingDate.Year<1880) {
				textDate.Text="";
			}
			else if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)){
				textDate.Text=lastAgingDate.ToShortDateString();
			}
			else{
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			for(int i=0;i<DefC.Short[(int)DefCat.BillingTypes].Length;i++){
				listBillType.Items.Add(DefC.Short[(int)DefCat.BillingTypes][i].ItemName);
			}
			if(listBillType.Items.Count>0){
				listBillType.SelectedIndex=0;
			}
			listBillType.Visible=false;
			checkBillTypesAll.Checked=true;
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				listProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
			}
			if(listProv.Items.Count>0) {
				listProv.SelectedIndex=0;
			}
			checkProvAll.Checked=true;
			listProv.Visible=false;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				listClin.Visible=false;
				labelClin.Visible=false;
				checkAllClin.Visible=false;
			}
			else {
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					listClin.Items.Add(Lan.g(this,"Unassigned"));
					listClin.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClin.Items.Add(_listClinics[i].Description);
					if(FormOpenDental.ClinicNum==0) {
						listClin.SetSelected(curIndex,true);
						checkAllClin.Checked=true;
					}
					if(_listClinics[i].ClinicNum==FormOpenDental.ClinicNum) {
						listClin.SelectedIndices.Clear();
						listClin.SetSelected(curIndex,true);
					}
				}
			}
		}

		private void checkBillTypesAll_Click(object sender,EventArgs e) {
			if(checkBillTypesAll.Checked){
				listBillType.Visible=false;
			}
			else{
				listBillType.Visible=true;
			}
		}

		private void checkProvAll_Click(object sender,EventArgs e) {
			if(checkProvAll.Checked) {
				listProv.Visible=false;
			}
			else {
				listProv.Visible=true;
			}
		}

		private void checkIncludeNeg_Click(object sender, System.EventArgs e) {
			if(checkIncludeNeg.Checked){
				checkOnlyNeg.Checked=false;
			}
		}

		private void checkOnlyNeg_Click(object sender, System.EventArgs e) {
			if(checkOnlyNeg.Checked){
				checkIncludeNeg.Checked=false;
			}
		}

		private void checkAllClin_Click(object sender,EventArgs e) {
			if(checkAllClin.Checked) {
				for(int i=0;i<listClin.Items.Count;i++) {
					listClin.SetSelected(i,true);
				}
			}
			else {
				listClin.SelectedIndices.Clear();
			}
		}

		private void listClin_Click(object sender,EventArgs e) {
			if(listClin.SelectedIndices.Count>0) {
				checkAllClin.Checked=false;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!checkBillTypesAll.Checked && listBillType.SelectedIndices.Count==0){
				MsgBox.Show(this,"At least one billing type must be selected.");
				return;
			}
			if(!checkProvAll.Checked && listProv.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(textDate.errorProvider1.GetError(textDate) != "") {
				MsgBox.Show(this,"Invalid date.");
				return;
			}
			DateTime asOfDate=PIn.Date(textDate.Text);
			//The aging report always show historical numbers based on the date entered.
			Ledgers.ComputeAging(0,asOfDate,true);
			ReportSimpleGrid report=new ReportSimpleGrid();
			string cmd="SELECT ";
			if(PrefC.GetBool(PrefName.ReportsShowPatNum)){
				cmd+=DbHelper.Concat("Cast(PatNum AS CHAR)","'-'","LName","', '","FName","' '","MiddleI");
			}
			else{
				cmd+=DbHelper.Concat("LName","', '","FName","' '","MiddleI");
			}
			cmd+=",Bal_0_30,Bal_31_60,Bal_61_90,BalOver90"
				+",BalTotal "
				+",InsEst"
				+",BalTotal-InsEst AS ";//\"$pat\" ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				cmd+="$pat ";
			}
			else { //Oracle needs quotes.
				cmd+="\"$pat\" ";
			}
			cmd+="FROM patient "
				+"WHERE ";
			if(checkExcludeInactive.Checked) {
				cmd+="(patstatus != 2) AND ";
			}
			if(checkBadAddress.Checked) {
				cmd+="(zip !='') AND ";
			}
			if(checkOnlyNeg.Checked){
				cmd+="BalTotal < '-.005' ";
			}
			else{
				if(radioAny.Checked){
					cmd+=
						"(Bal_0_30 > '.005' OR Bal_31_60 > '.005' OR Bal_61_90 > '.005' OR BalOver90 > '.005'";
				}
				else if(radio30.Checked){
					cmd+=
						"(Bal_31_60 > '.005' OR Bal_61_90 > '.005' OR BalOver90 > '.005'";
				}
				else if(radio60.Checked){
					cmd+=
						"(Bal_61_90 > '.005' OR BalOver90 > '.005'";
				}
				else if(radio90.Checked){
					cmd+=
						"(BalOver90 > '.005'";
				}
				if(checkIncludeNeg.Checked){
					cmd+=" OR BalTotal < '-.005'";
				}
				cmd+=") ";
			}
			if(!checkBillTypesAll.Checked){
				for(int i=0;i<listBillType.SelectedIndices.Count;i++) {
					if(i==0) {
						cmd+=" AND (billingtype = ";
					}
					else {
						cmd+=" OR billingtype = ";
					}
					cmd+=POut.Long(DefC.Short[(int)DefCat.BillingTypes][listBillType.SelectedIndices[i]].DefNum);
				}
				cmd+=") ";
			}
			if(!checkProvAll.Checked) {
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i==0) {
						cmd+=" AND (PriProv = ";
					}
					else {
						cmd+=" OR PriProv = ";
					}
					cmd+=POut.Long(ProviderC.ListShort[listProv.SelectedIndices[i]].ProvNum);
				}
				cmd+=") ";
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				cmd+=" AND ClinicNum IN(";
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(i>0) {
						cmd+=",";
					}
					if(Security.CurUser.ClinicIsRestricted) {
						cmd+=POut.Long(_listClinics[listClin.SelectedIndices[i]].ClinicNum);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							cmd+="0";
						}
						else {
							cmd+=POut.Long(_listClinics[listClin.SelectedIndices[i]-1].ClinicNum);//Minus 1 from the selected index
						}
					}
				}
				cmd+=") ";
			}
			cmd+="ORDER BY LName,FName";
			report.Query=cmd;
			FormQuery2=new FormQuery(report);
			FormQuery2.IsReport=true;
			FormQuery2.SubmitReportQuery();
			//Recompute aging in a non-historical way, so that the numbers are returned to the way they
			//are normally used in other parts of the program.
			Ledgers.RunAging();
			//if(Prefs.UpdateString(PrefName.DateLastAging",POut.PDate(asOfDate,false))) {
			//	DataValid.SetInvalid(InvalidType.Prefs);
			//}
			report.Title="AGING OF ACCOUNTS RECEIVABLE REPORT";
			report.SubTitle.Add(PrefC.GetString(PrefName.PracticeTitle));
			report.SubTitle.Add("As of "+textDate.Text);
			if(radioAny.Checked){
				report.SubTitle.Add("Any Balance");
			}
			if(radio30.Checked){
				report.SubTitle.Add("Over 30 Days");
			}
			if(radio60.Checked){
				report.SubTitle.Add("Over 60 Days");
			}
			if(radio90.Checked){
				report.SubTitle.Add("Over 90 Days");
			}
			if(checkBillTypesAll.Checked){
				report.SubTitle.Add("All Billing Types");
			}
			else{
				string subt=DefC.Short[(int)DefCat.BillingTypes][listBillType.SelectedIndices[0]].ItemName;
				for(int i=1;i<listBillType.SelectedIndices.Count;i++){
					subt+=", "+DefC.Short[(int)DefCat.BillingTypes][listBillType.SelectedIndices[i]].ItemName;
				}
				report.SubTitle.Add(subt);
			}
			//report.InitializeColumns(8);
			report.SetColumn(this,0,"GUARANTOR",160);
			report.SetColumn(this,1,"0-30 DAYS",80,HorizontalAlignment.Right);
			report.SetColumn(this,2,"30-60 DAYS",80,HorizontalAlignment.Right);
			report.SetColumn(this,3,"60-90 DAYS",80,HorizontalAlignment.Right);
			report.SetColumn(this,4,"> 90 DAYS",80,HorizontalAlignment.Right);
			report.SetColumn(this,5,"TOTAL",85,HorizontalAlignment.Right);
			report.SetColumn(this,6,"-INS EST",85,HorizontalAlignment.Right);
			report.SetColumn(this,7,"=PATIENT",85,HorizontalAlignment.Right);
			FormQuery2.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
		
		}

		

		

		

		

	}

	/*///<summary></summary>
	public struct Aging{
		///<summary></summary>
		public double Charges;
		///<summary></summary>
		public double Payments;
		///<summary></summary>
		public double Aged;
	}*/



}
