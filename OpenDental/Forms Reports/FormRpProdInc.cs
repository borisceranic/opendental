using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using OpenDentBusiness;
using OpenDental.ReportingComplex;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormRpProdInc : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listProv;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioDaily;
		private System.Windows.Forms.RadioButton radioMonthly;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textToday;
		private OpenDental.ValidDate textDateFrom;
		private OpenDental.ValidDate textDateTo;
		private System.Windows.Forms.RadioButton radioAnnual;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.UI.Button butThis;
		private OpenDental.UI.Button butLeft;
		private OpenDental.UI.Button butRight;
		private FormQuery FormQuery2;
		private DateTime dateFrom;
		private ListBox listClin;
		private Label labelClin;
		private DateTime dateTo;
		///<summary>Can be set externally when automating.</summary>
		public string DailyMonthlyAnnual;
		///<summary>If set externally, then this sets the date on startup.</summary>
		public DateTime DateStart;
		private GroupBox groupBox3;
		private RadioButton radioWriteoffPay;
		private RadioButton radioWriteoffProc;
		private Label label5;
		private CheckBox checkAllProv;
		private CheckBox checkAllClin;
		///<summary>If set externally, then this sets the date on startup.</summary>
		public DateTime DateEnd;
		private RadioButton radioProvider;
		private CheckBox checkClinicBreakdown;
    private CheckBox checkClinicInfo;
    private List<Clinic> _listClinics;

		///<summary></summary>
		public FormRpProdInc(){
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpProdInc));
      this.label1 = new System.Windows.Forms.Label();
      this.listProv = new System.Windows.Forms.ListBox();
      this.radioMonthly = new System.Windows.Forms.RadioButton();
      this.radioDaily = new System.Windows.Forms.RadioButton();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.radioProvider = new System.Windows.Forms.RadioButton();
      this.radioAnnual = new System.Windows.Forms.RadioButton();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.textToday = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.butRight = new OpenDental.UI.Button();
      this.butThis = new OpenDental.UI.Button();
      this.textDateFrom = new OpenDental.ValidDate();
      this.textDateTo = new OpenDental.ValidDate();
      this.butLeft = new OpenDental.UI.Button();
      this.listClin = new System.Windows.Forms.ListBox();
      this.labelClin = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this.radioWriteoffProc = new System.Windows.Forms.RadioButton();
      this.radioWriteoffPay = new System.Windows.Forms.RadioButton();
      this.checkAllProv = new System.Windows.Forms.CheckBox();
      this.checkAllClin = new System.Windows.Forms.CheckBox();
      this.checkClinicBreakdown = new System.Windows.Forms.CheckBox();
      this.butCancel = new OpenDental.UI.Button();
      this.butOK = new OpenDental.UI.Button();
      this.checkClinicInfo = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(35, 128);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(104, 16);
      this.label1.TabIndex = 29;
      this.label1.Text = "Providers";
      this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // listProv
      // 
      this.listProv.Location = new System.Drawing.Point(37, 165);
      this.listProv.Name = "listProv";
      this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listProv.Size = new System.Drawing.Size(154, 186);
      this.listProv.TabIndex = 30;
      this.listProv.Click += new System.EventHandler(this.listProv_Click);
      // 
      // radioMonthly
      // 
      this.radioMonthly.Checked = true;
      this.radioMonthly.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radioMonthly.Location = new System.Drawing.Point(14, 40);
      this.radioMonthly.Name = "radioMonthly";
      this.radioMonthly.Size = new System.Drawing.Size(104, 17);
      this.radioMonthly.TabIndex = 33;
      this.radioMonthly.TabStop = true;
      this.radioMonthly.Text = "Monthly";
      this.radioMonthly.Click += new System.EventHandler(this.radioMonthly_Click);
      // 
      // radioDaily
      // 
      this.radioDaily.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radioDaily.Location = new System.Drawing.Point(14, 21);
      this.radioDaily.Name = "radioDaily";
      this.radioDaily.Size = new System.Drawing.Size(104, 17);
      this.radioDaily.TabIndex = 34;
      this.radioDaily.Text = "Daily";
      this.radioDaily.Click += new System.EventHandler(this.radioDaily_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.radioProvider);
      this.groupBox1.Controls.Add(this.radioAnnual);
      this.groupBox1.Controls.Add(this.radioDaily);
      this.groupBox1.Controls.Add(this.radioMonthly);
      this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.groupBox1.Location = new System.Drawing.Point(37, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(123, 101);
      this.groupBox1.TabIndex = 35;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Report Type";
      // 
      // radioProvider
      // 
      this.radioProvider.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radioProvider.Location = new System.Drawing.Point(14, 78);
      this.radioProvider.Name = "radioProvider";
      this.radioProvider.Size = new System.Drawing.Size(104, 17);
      this.radioProvider.TabIndex = 36;
      this.radioProvider.Text = "Provider";
      this.radioProvider.Click += new System.EventHandler(this.radioProvider_Click);
      // 
      // radioAnnual
      // 
      this.radioAnnual.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radioAnnual.Location = new System.Drawing.Point(14, 59);
      this.radioAnnual.Name = "radioAnnual";
      this.radioAnnual.Size = new System.Drawing.Size(104, 17);
      this.radioAnnual.TabIndex = 35;
      this.radioAnnual.Text = "Annual";
      this.radioAnnual.Click += new System.EventHandler(this.radioAnnual_Click);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(9, 79);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(82, 18);
      this.label2.TabIndex = 37;
      this.label2.Text = "From";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(7, 105);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(82, 18);
      this.label3.TabIndex = 39;
      this.label3.Text = "To";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(356, 66);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(127, 20);
      this.label4.TabIndex = 41;
      this.label4.Text = "Today\'s Date";
      this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // textToday
      // 
      this.textToday.Location = new System.Drawing.Point(485, 64);
      this.textToday.Name = "textToday";
      this.textToday.ReadOnly = true;
      this.textToday.Size = new System.Drawing.Size(100, 20);
      this.textToday.TabIndex = 42;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.butRight);
      this.groupBox2.Controls.Add(this.butThis);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.textDateFrom);
      this.groupBox2.Controls.Add(this.textDateTo);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.butLeft);
      this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.groupBox2.Location = new System.Drawing.Point(390, 90);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(281, 144);
      this.groupBox2.TabIndex = 43;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Date Range";
      // 
      // butRight
      // 
      this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butRight.Autosize = true;
      this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butRight.CornerRadius = 4F;
      this.butRight.Image = global::OpenDental.Properties.Resources.Right;
      this.butRight.Location = new System.Drawing.Point(205, 30);
      this.butRight.Name = "butRight";
      this.butRight.Size = new System.Drawing.Size(45, 26);
      this.butRight.TabIndex = 46;
      this.butRight.Click += new System.EventHandler(this.butRight_Click);
      // 
      // butThis
      // 
      this.butThis.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butThis.Autosize = true;
      this.butThis.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butThis.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butThis.CornerRadius = 4F;
      this.butThis.Location = new System.Drawing.Point(95, 30);
      this.butThis.Name = "butThis";
      this.butThis.Size = new System.Drawing.Size(101, 26);
      this.butThis.TabIndex = 45;
      this.butThis.Text = "This";
      this.butThis.Click += new System.EventHandler(this.butThis_Click);
      // 
      // textDateFrom
      // 
      this.textDateFrom.Location = new System.Drawing.Point(95, 77);
      this.textDateFrom.Name = "textDateFrom";
      this.textDateFrom.Size = new System.Drawing.Size(100, 20);
      this.textDateFrom.TabIndex = 43;
      // 
      // textDateTo
      // 
      this.textDateTo.Location = new System.Drawing.Point(95, 104);
      this.textDateTo.Name = "textDateTo";
      this.textDateTo.Size = new System.Drawing.Size(100, 20);
      this.textDateTo.TabIndex = 44;
      // 
      // butLeft
      // 
      this.butLeft.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butLeft.Autosize = true;
      this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butLeft.CornerRadius = 4F;
      this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
      this.butLeft.Location = new System.Drawing.Point(41, 30);
      this.butLeft.Name = "butLeft";
      this.butLeft.Size = new System.Drawing.Size(45, 26);
      this.butLeft.TabIndex = 44;
      this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
      // 
      // listClin
      // 
      this.listClin.Location = new System.Drawing.Point(215, 165);
      this.listClin.Name = "listClin";
      this.listClin.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listClin.Size = new System.Drawing.Size(154, 186);
      this.listClin.TabIndex = 45;
      this.listClin.Click += new System.EventHandler(this.listClin_Click);
      // 
      // labelClin
      // 
      this.labelClin.Location = new System.Drawing.Point(212, 128);
      this.labelClin.Name = "labelClin";
      this.labelClin.Size = new System.Drawing.Size(104, 16);
      this.labelClin.TabIndex = 44;
      this.labelClin.Text = "Clinics";
      this.labelClin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.radioWriteoffProc);
      this.groupBox3.Controls.Add(this.radioWriteoffPay);
      this.groupBox3.Location = new System.Drawing.Point(390, 256);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(281, 95);
      this.groupBox3.TabIndex = 46;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Show Insurance Writeoffs";
      // 
      // label5
      // 
      this.label5.Location = new System.Drawing.Point(6, 71);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(269, 17);
      this.label5.TabIndex = 2;
      this.label5.Text = "(this is discussed in the PPO section of the manual)";
      // 
      // radioWriteoffProc
      // 
      this.radioWriteoffProc.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
      this.radioWriteoffProc.Location = new System.Drawing.Point(9, 41);
      this.radioWriteoffProc.Name = "radioWriteoffProc";
      this.radioWriteoffProc.Size = new System.Drawing.Size(244, 23);
      this.radioWriteoffProc.TabIndex = 1;
      this.radioWriteoffProc.Text = "Using procedure date.";
      this.radioWriteoffProc.TextAlign = System.Drawing.ContentAlignment.TopLeft;
      this.radioWriteoffProc.UseVisualStyleBackColor = true;
      // 
      // radioWriteoffPay
      // 
      this.radioWriteoffPay.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
      this.radioWriteoffPay.Checked = true;
      this.radioWriteoffPay.Location = new System.Drawing.Point(9, 20);
      this.radioWriteoffPay.Name = "radioWriteoffPay";
      this.radioWriteoffPay.Size = new System.Drawing.Size(244, 23);
      this.radioWriteoffPay.TabIndex = 0;
      this.radioWriteoffPay.TabStop = true;
      this.radioWriteoffPay.Text = "Using insurance payment date.";
      this.radioWriteoffPay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
      this.radioWriteoffPay.UseVisualStyleBackColor = true;
      // 
      // checkAllProv
      // 
      this.checkAllProv.Checked = true;
      this.checkAllProv.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkAllProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkAllProv.Location = new System.Drawing.Point(38, 146);
      this.checkAllProv.Name = "checkAllProv";
      this.checkAllProv.Size = new System.Drawing.Size(95, 16);
      this.checkAllProv.TabIndex = 47;
      this.checkAllProv.Text = "All";
      this.checkAllProv.Click += new System.EventHandler(this.checkAllProv_Click);
      // 
      // checkAllClin
      // 
      this.checkAllClin.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkAllClin.Location = new System.Drawing.Point(215, 146);
      this.checkAllClin.Name = "checkAllClin";
      this.checkAllClin.Size = new System.Drawing.Size(95, 16);
      this.checkAllClin.TabIndex = 48;
      this.checkAllClin.Text = "All";
      this.checkAllClin.Click += new System.EventHandler(this.checkAllClin_Click);
      // 
      // checkClinicBreakdown
      // 
      this.checkClinicBreakdown.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkClinicBreakdown.Location = new System.Drawing.Point(215, 371);
      this.checkClinicBreakdown.Name = "checkClinicBreakdown";
      this.checkClinicBreakdown.Size = new System.Drawing.Size(191, 16);
      this.checkClinicBreakdown.TabIndex = 49;
      this.checkClinicBreakdown.Text = "Show Clinic Breakdown";
      // 
      // butCancel
      // 
      this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.butCancel.Autosize = true;
      this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butCancel.CornerRadius = 4F;
      this.butCancel.Location = new System.Drawing.Point(710, 365);
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
      this.butOK.Location = new System.Drawing.Point(710, 330);
      this.butOK.Name = "butOK";
      this.butOK.Size = new System.Drawing.Size(75, 26);
      this.butOK.TabIndex = 3;
      this.butOK.Text = "&OK";
      this.butOK.Click += new System.EventHandler(this.butOK_Click);
      // 
      // checkClinicInfo
      // 
      this.checkClinicInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkClinicInfo.Location = new System.Drawing.Point(215, 353);
      this.checkClinicInfo.Name = "checkClinicInfo";
      this.checkClinicInfo.Size = new System.Drawing.Size(169, 16);
      this.checkClinicInfo.TabIndex = 51;
      this.checkClinicInfo.Text = "Show Clinic Info";
      this.checkClinicInfo.CheckedChanged += new System.EventHandler(this.checkClinicInfo_CheckedChanged);
      // 
      // FormRpProdInc
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(818, 417);
      this.Controls.Add(this.checkClinicInfo);
      this.Controls.Add(this.checkClinicBreakdown);
      this.Controls.Add(this.checkAllClin);
      this.Controls.Add(this.checkAllProv);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.listClin);
      this.Controls.Add(this.labelClin);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.textToday);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.listProv);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.butCancel);
      this.Controls.Add(this.butOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRpProdInc";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Production and Income Report";
      this.Load += new System.EventHandler(this.FormProduction_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

		}
		#endregion
		private void FormProduction_Load(object sender, System.EventArgs e) {
			textToday.Text=DateTime.Today.ToShortDateString();
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				listProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)){
				listClin.Visible=false;
				labelClin.Visible=false;
				checkAllClin.Visible=false;
        checkClinicInfo.Visible=false;
				checkClinicBreakdown.Visible=false;
			}
			else {
        checkClinicInfo.Checked=PrefC.GetBool(PrefName.ReportPandIhasClinicInfo);
				checkClinicBreakdown.Checked=PrefC.GetBool(PrefName.ReportPandIhasClinicBreakdown);
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					listClin.Items.Add(Lan.g(this,"Unassigned"));
					listClin.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClin.Items.Add(_listClinics[i].Description);
					if(Clinics.ClinicNum==0) {
						listClin.SetSelected(curIndex,true);
						checkAllClin.Checked=true;
					}
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						listClin.SelectedIndices.Clear();
						listClin.SetSelected(curIndex,true);
					}
				}
			}
			switch(DailyMonthlyAnnual){
				case "Daily":
					radioDaily.Checked=true;
					break;
				case "Monthly":
					radioMonthly.Checked=true;
					break;
				case "Annual":
					radioAnnual.Checked=true;
					break;
			}
			SetDates();
			if(PrefC.GetBool(PrefName.ReportsPPOwriteoffDefaultToProcDate)) {
				radioWriteoffProc.Checked=true;
			}
			if(DateStart.Year>1880){
				textDateFrom.Text=DateStart.ToShortDateString();
				textDateTo.Text=DateEnd.ToShortDateString();
				switch(DailyMonthlyAnnual) {
					case "Daily":
						RunDaily();
						break;
					case "Monthly":
						RunMonthly();
						break;
					case "Annual":
						RunAnnual();
						break;
				}
				Close();
			}
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

		private void radioDaily_Click(object sender, System.EventArgs e) {
			SetDates();
		}

		private void radioMonthly_Click(object sender, System.EventArgs e) {
			SetDates();
		}

		private void radioAnnual_Click(object sender, System.EventArgs e) {
			SetDates();
		}

		private void radioProvider_Click(object sender,EventArgs e) {
			SetDates();
		}

		private void SetDates(){
      if(radioDaily.Checked) {
				if(PrefC.HasClinicsEnabled) {
					checkClinicInfo.Visible=true;
					if(checkClinicInfo.Checked) {
						checkClinicBreakdown.Visible=true;
					}
					else {
						//Clinic info not checked so hide the clinic breakdown
						checkClinicBreakdown.Checked=false;
						checkClinicBreakdown.Visible=false;
					}
				}
        textDateFrom.Text=DateTime.Today.ToShortDateString();
        textDateTo.Text=DateTime.Today.ToShortDateString();
        butThis.Text=Lan.g(this,"Today");
      }
      else if(radioProvider.Checked) {
				if(PrefC.HasClinicsEnabled) {
					checkClinicInfo.Visible=false;
					checkClinicBreakdown.Visible=true;
				}
        textDateFrom.Text=DateTime.Today.ToShortDateString();
        textDateTo.Text=DateTime.Today.ToShortDateString();
        butThis.Text=Lan.g(this,"Today");
      }
			else if(radioMonthly.Checked) {
				if(PrefC.HasClinicsEnabled) {
					checkClinicInfo.Visible=false;
					checkClinicBreakdown.Visible=true;
				}
				textDateFrom.Text=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToShortDateString();
				textDateTo.Text=new DateTime(DateTime.Today.Year,DateTime.Today.Month
					,DateTime.DaysInMonth(DateTime.Today.Year,DateTime.Today.Month)).ToShortDateString();
				butThis.Text=Lan.g(this,"This Month");
			}
			else{//annual
				if(PrefC.HasClinicsEnabled) {
					checkClinicInfo.Visible=false;
					checkClinicBreakdown.Visible=true;
				}
				textDateFrom.Text=new DateTime(DateTime.Today.Year,1,1).ToShortDateString();
				textDateTo.Text=new DateTime(DateTime.Today.Year,12,31).ToShortDateString();
				butThis.Text=Lan.g(this,"This Year");
			}
		}

		private void butThis_Click(object sender, System.EventArgs e) {
			SetDates();
		}

		private void butLeft_Click(object sender, System.EventArgs e) {
			if(  textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			if(radioDaily.Checked || radioProvider.Checked) {
				textDateFrom.Text=dateFrom.AddDays(-1).ToShortDateString();
				textDateTo.Text=dateTo.AddDays(-1).ToShortDateString();
			}
			else if(radioMonthly.Checked){
				bool toLastDay=false;
				if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month)==dateTo.Day){
					toLastDay=true;
				}
				textDateFrom.Text=dateFrom.AddMonths(-1).ToShortDateString();
				textDateTo.Text=dateTo.AddMonths(-1).ToShortDateString();
				dateTo=PIn.Date(textDateTo.Text);
				if(toLastDay){
					textDateTo.Text=new DateTime(dateTo.Year,dateTo.Month,
						CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month))
						.ToShortDateString();
				}
			}
			else{//annual
				textDateFrom.Text=dateFrom.AddYears(-1).ToShortDateString();
				textDateTo.Text=dateTo.AddYears(-1).ToShortDateString();
			}
		}

		private void butRight_Click(object sender, System.EventArgs e) {
			if(  textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			if(radioDaily.Checked || radioProvider.Checked) {
				textDateFrom.Text=dateFrom.AddDays(1).ToShortDateString();
				textDateTo.Text=dateTo.AddDays(1).ToShortDateString();
			}
			else if(radioMonthly.Checked){
				bool toLastDay=false;
				if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month)==dateTo.Day){
					toLastDay=true;
				}
				textDateFrom.Text=dateFrom.AddMonths(1).ToShortDateString();
				textDateTo.Text=dateTo.AddMonths(1).ToShortDateString();
				dateTo=PIn.Date(textDateTo.Text);
				if(toLastDay){
					textDateTo.Text=new DateTime(dateTo.Year,dateTo.Month,
						CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month))
						.ToShortDateString();
				}
			}
			else{//annual
				textDateFrom.Text=dateFrom.AddYears(1).ToShortDateString();
				textDateTo.Text=dateTo.AddYears(1).ToShortDateString();
			}
		}
    
    private void checkClinicInfo_CheckedChanged(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				if(checkClinicInfo.Checked) {
					checkClinicBreakdown.Visible=true;
				}
				else {
					checkClinicBreakdown.Checked=false;
					checkClinicBreakdown.Visible=false;
				}
			}
    }

		private void RunDaily() {
			if(Plugins.HookMethod(this,"FormRpProdInc.RunDaily_Start",PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text))) {
				return;
			}
			if(checkAllProv.Checked) {
				for(int i=0;i<listProv.Items.Count;i++) {
					listProv.SetSelected(i,true);
				}
			}
			if(checkAllClin.Checked) {
				for(int i=0;i<listClin.Items.Count;i++) {
					listClin.SetSelected(i,true);
				}
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			List<Provider> listProvs=new List<Provider>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				listProvs.Add(ProviderC.ListShort[listProv.SelectedIndices[i]]);
			}
			List<Clinic> listClinics=new List<Clinic>();
			if(PrefC.HasClinicsEnabled) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(Security.CurUser.ClinicIsRestricted) {
						listClinics.Add(_listClinics[listClin.SelectedIndices[i]]);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							Clinic unassigned=new Clinic();
							unassigned.ClinicNum=0;
							unassigned.Description="Unassigned";
							listClinics.Add(unassigned);
						}
						else {
							listClinics.Add(_listClinics[listClin.SelectedIndices[i]-1]);//Minus 1 from the selected index
						}
					}
				}
			}
			DataSet dataSetDailyProd=RpProdInc.GetDailyData(dateFrom,dateTo,listProvs,listClinics,radioWriteoffPay.Checked
				,checkAllProv.Checked,checkAllClin.Checked,checkClinicBreakdown.Checked,checkClinicInfo.Checked);
			DataTable tableDailyProd=dataSetDailyProd.Tables["DailyProd"];//Includes multiple clinics that will get separated out later.
			DataSet dataSetDailyProdSplitByClinic=new DataSet();
			if(PrefC.HasClinicsEnabled && checkClinicInfo.Checked) {
				//Split up each clinic into its own table and add that to the data set split up by clinics.
				string lastClinic="";
				DataTable dtClinic=tableDailyProd.Clone();//Clones the structure, not the data.
				for(int i=0;i<tableDailyProd.Rows.Count;i++) {
					string currentClinic=tableDailyProd.Rows[i]["Clinic"].ToString();
					if(lastClinic=="") {
						lastClinic=currentClinic;
					}
					//Check if we have successfully added all rows for the current clinic and add the datatable to the dataset if there is information present.
					if(lastClinic!=currentClinic && dtClinic.Rows.Count>0) {
						DataTable dtClinicTemp=dtClinic.Copy();
						dtClinicTemp.TableName="Clinic"+i;//The name of the table does not matter but has to be unique in a DataSet.
						dataSetDailyProdSplitByClinic.Tables.Add(dtClinicTemp);
						dtClinic.Rows.Clear();//Clear out the data to start collecting the information for the next clinic.
						lastClinic=currentClinic;
					}
					dtClinic.Rows.Add(tableDailyProd.Rows[i].ItemArray);
					//If this is the last row, add dtClinic to the dataset.
					if(i==tableDailyProd.Rows.Count-1) {
						DataTable dtClinicTemp=dtClinic.Copy();
						//Added 1 to guarantee unique tablename.
						dtClinicTemp.TableName="Clinic"+(i+1);//The name of the table does not matter but has to be unique in a DataSet. 
						dataSetDailyProdSplitByClinic.Tables.Add(dtClinicTemp);
					}
				}
			}
			//The old daily prod and inc report (prior to report complex) had portait mode for non-clinic users and landscape for clinic users.
			ReportComplex report=new ReportComplex(true,PrefC.HasClinicsEnabled ? checkClinicInfo.Checked : false);
			report.ReportName="DailyP&I";
			report.AddTitle("Title",Lan.g(this,"Daily Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			string dateRangeStr=dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString();
			if(dateFrom.Date==dateTo.Date) {
				dateRangeStr=dateFrom.ToShortDateString();//Do not show a date range for the same day...
			}
			report.AddSubTitle("Date",dateRangeStr);
			if(checkAllProv.Checked) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				string str="";
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i>0) {
						str+=", ";
					}
					str+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
				}
				report.AddSubTitle("Providers",str);
			}
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClin.Checked) {
					report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
				}
				else {
					string clinNames="";
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							clinNames+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							clinNames+=_listClinics[listClin.SelectedIndices[i]].Description;
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								clinNames+=Lan.g(this,"Unassigned");
							}
							else {
								clinNames+=_listClinics[listClin.SelectedIndices[i]-1].Description;//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics",clinNames);
				}
			}
			//setup query
			QueryObject query=null;
			int dateWidth=75;
			int patientNameWidth=130;
			int descriptionWidth=220;
			int provWidth=65;
			int adjustWidth=75;
			int writeoffWidth=75;
      if(!PrefC.HasClinicsEnabled || !checkClinicInfo.Checked) {
        //Trim some fat off for non-clinic users because this report shows in portait mode.
        dateWidth=68;
        patientNameWidth=120;
        descriptionWidth=180;
        provWidth=55;
        adjustWidth=70;
        writeoffWidth=70;
      }
      Font font=new Font("Tahoma",8);
			query=report.AddQuery(tableDailyProd,Lan.g(this,"Date")+": "+DateTime.Today.ToShortDateString(),"ClinicSplit",SplitByKind.Value,1,true);
			query.AddColumn("Date",dateWidth,FieldValueType.String,font);
			query.AddColumn("Patient Name",patientNameWidth,FieldValueType.String,font);
			query.AddColumn("Description",descriptionWidth,FieldValueType.String,font);
			query.AddColumn("Prov",provWidth,FieldValueType.String,font);
			if(PrefC.HasClinicsEnabled && checkClinicInfo.Checked) {//Not no clinics
				query.AddColumn("Clinic",130,FieldValueType.String,font);
			}
			query.AddColumn("Production",75,FieldValueType.Number,font);
			query.AddColumn("Adjust",adjustWidth,FieldValueType.Number,font);
			query.AddColumn("Writeoff",writeoffWidth,FieldValueType.Number,font);
			query.AddColumn("Pt Income",75,FieldValueType.Number,font);
			query.AddColumn("Ins Income",75,FieldValueType.Number,font);
			//If more than one clinic selected, we want to add a table to the end of the report that totals all the clinics together.
			//When only one clinic is showing , the "Summary" at the end of every daily report will suffice. (total prod and total income lines).
			if(PrefC.HasClinicsEnabled && listClinics.Count > 1 && checkClinicInfo.Checked) {
				DataTable tableClinicTotals=GetClinicTotals(dataSetDailyProdSplitByClinic);
				query=report.AddQuery(tableClinicTotals,"Clinic Totals","",SplitByKind.None,2,true);
				query.AddColumn("Clinic",410,FieldValueType.String,font);
				query.AddColumn("Production",75,FieldValueType.Number,font);
				query.AddColumn("Adjust",75,FieldValueType.Number,font);
				query.AddColumn("Writeoff",75,FieldValueType.Number,font);
				query.AddColumn("Pt Income",75,FieldValueType.Number,font);
				query.AddColumn("Ins Income",75,FieldValueType.Number,font);
			}
			//Calculate the total production and total income and add them to the bottom of the report:
			double totalProduction=0;
			double totalIncome=0;
			for(int i=0;i<tableDailyProd.Rows.Count;i++) {
				//Total production is (Production + Adjustments - Writeoffs)
				totalProduction+=PIn.Double(tableDailyProd.Rows[i]["Production"].ToString());
				totalProduction+=PIn.Double(tableDailyProd.Rows[i]["Adjust"].ToString());
				totalProduction+=PIn.Double(tableDailyProd.Rows[i]["Writeoff"].ToString());
				//Total income is (Pt Income + Ins Income)
				totalIncome+=PIn.Double(tableDailyProd.Rows[i]["Pt Income"].ToString());
				totalIncome+=PIn.Double(tableDailyProd.Rows[i]["Ins Income"].ToString());
			}
			//Add the Total Production and Total Income to the bottom of the report if there were any rows present.
			if(tableDailyProd.Rows.Count > 0) {
				//Use a custom table and add it like it is a "query" to the report because using a group summary would be more complicated due
				//to the need to add and subtract from multiple columns at the same time.
				DataTable tableTotals=new DataTable("TotalProdAndInc");
				tableTotals.Columns.Add("Summary");
				tableTotals.Rows.Add(Lan.g(this,"Total Production (Production + Adjustments - Writeoffs):")+" "+totalProduction.ToString("c"));
				tableTotals.Rows.Add(Lan.g(this,"Total Income (Pt Income + Ins Income):")+" "+totalIncome.ToString("c"));
				//Add tableTotals to the report.
				//No column name and no header because we want to display this table to NOT look like a table.
				query=report.AddQuery(tableTotals,"","",SplitByKind.None,2,false);
				query.AddColumn("",785,FieldValueType.String,new Font("Tahoma",8,FontStyle.Bold));
			}
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {
				return;
			}
			// display report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;//Allow running multiple reports.
		}

		private DataTable GetClinicTotals(DataSet dataSetDailyProdSplitByClinic) {
			DataTable tableClinicTotals=new DataTable("ClinicTotals");
			tableClinicTotals.Columns.Add(new DataColumn("Clinic"));
			tableClinicTotals.Columns.Add(new DataColumn("Production"));
			tableClinicTotals.Columns.Add(new DataColumn("Adjust"));
			tableClinicTotals.Columns.Add(new DataColumn("Writeoff"));
			tableClinicTotals.Columns.Add(new DataColumn("Pt Income"));
			tableClinicTotals.Columns.Add(new DataColumn("Ins Income"));
			for(int i=0;i<dataSetDailyProdSplitByClinic.Tables.Count;i++) {
				string clinicDesc="";
				if(dataSetDailyProdSplitByClinic.Tables[i].Rows.Count > 0) {
					clinicDesc=dataSetDailyProdSplitByClinic.Tables[i].Rows[0]["Clinic"].ToString();//Take description of first row.
				}
				//Calculate the total production and total income for this clinic.
				double production=0;
				double adjust=0;
				double writeoff=0;
				double ptIncome=0;
				double insIncome=0;
				for(int j=0;j<dataSetDailyProdSplitByClinic.Tables[i].Rows.Count;j++) {
					production+=PIn.Double(dataSetDailyProdSplitByClinic.Tables[i].Rows[j]["Production"].ToString());
					adjust+=PIn.Double(dataSetDailyProdSplitByClinic.Tables[i].Rows[j]["Adjust"].ToString());
					writeoff+=PIn.Double(dataSetDailyProdSplitByClinic.Tables[i].Rows[j]["Writeoff"].ToString());
					ptIncome+=PIn.Double(dataSetDailyProdSplitByClinic.Tables[i].Rows[j]["Pt Income"].ToString());
					insIncome+=PIn.Double(dataSetDailyProdSplitByClinic.Tables[i].Rows[j]["Ins Income"].ToString());
				}
				tableClinicTotals.Rows.Add(clinicDesc,production,adjust,writeoff,ptIncome,insIncome);
			}
			return tableClinicTotals;
		}

		private void RunMonthly(){
			if(Plugins.HookMethod(this,"FormRpProdInc.RunMonthly_Start",PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text))) {
				return;
			}
			if(checkAllProv.Checked) {
				for(int i=0;i<listProv.Items.Count;i++) {
					listProv.SetSelected(i,true);
				}
			}
			if(checkAllClin.Checked) {
				for(int i=0;i<listClin.Items.Count;i++) {
					listClin.SetSelected(i,true);
				}
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			List<Provider> listProvs=new List<Provider>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				listProvs.Add(ProviderC.ListShort[listProv.SelectedIndices[i]]);
			}
			List<Clinic> listClinics=new List<Clinic>();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(Security.CurUser.ClinicIsRestricted) {
						listClinics.Add(_listClinics[listClin.SelectedIndices[i]]);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							Clinic unassigned=new Clinic();
							unassigned.ClinicNum=0;
							unassigned.Description="Unassigned";
							listClinics.Add(unassigned);//Will have ClinicNum of 0 for our "Unassigned" needs.
						}
						else {
							listClinics.Add(_listClinics[listClin.SelectedIndices[i]-1]);//Minus 1 from the selected index
						}
					}
				}
			}
			DataSet ds=RpProdInc.GetMonthlyData(dateFrom,dateTo,listProvs,listClinics,radioWriteoffPay.Checked,checkAllProv.Checked,checkAllClin.Checked);
			DataTable dt=ds.Tables["Total"];
			DataTable dtClinic=new DataTable();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				dtClinic=ds.Tables["Clinic"];
			}
			ReportComplex report=new ReportComplex(true,false);
			report.ReportName="MonthlyP&I";
			report.AddTitle("Title",Lan.g(this,"Monthly Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString());
			if(checkAllProv.Checked) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				string str="";
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i>0) {
						str+=", ";
					}
					str+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
				}
				report.AddSubTitle("Providers",str);
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(checkAllClin.Checked) {
					report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
				}
				else {
					string clinNames="";
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							clinNames+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							clinNames+=_listClinics[listClin.SelectedIndices[i]].Description;
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								clinNames+=Lan.g(this,"Unassigned");
							}
							else {
								clinNames+=_listClinics[listClin.SelectedIndices[i]-1].Description;//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics",clinNames);
				}
			}
			//setup query
			QueryObject query;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && checkClinicBreakdown.Checked) {
				query=report.AddQuery(dtClinic,"","Clinic",SplitByKind.Value,1,true);
			}
			else {
				query=report.AddQuery(dt,"","",SplitByKind.None,1,true);
			}
			// add columns to report
			Font font=new Font("Tahoma",8,FontStyle.Regular);
			query.AddColumn("Date",70,FieldValueType.String,font);
			query.AddColumn("Weekday",70,FieldValueType.String,font);
			query.AddColumn("Production",80,FieldValueType.Number,font);
			query.AddColumn("Sched",80,FieldValueType.Number,font);
			query.AddColumn("Adj",80,FieldValueType.Number,font);
			query.AddColumn("Writeoff",80,FieldValueType.Number,font);
			query.AddColumn("Tot Prod",80,FieldValueType.Number,font);
			query.AddColumn("Pt Income",80,FieldValueType.Number,font);
			query.AddColumn("Ins Income",80,FieldValueType.Number,font);
			query.AddColumn("Tot Income",80,FieldValueType.Number,font);
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && listClin.SelectedIndices.Count>1 && checkClinicBreakdown.Checked) {
				//If more than one clinic selected, we want to add a table to the end of the report that totals all the clinics together.
				query=report.AddQuery(dt,"Totals","",SplitByKind.None,2,true);
				query.AddColumn("Date",70,FieldValueType.String,font);
				query.AddColumn("Weekday",70,FieldValueType.String,font);
				query.AddColumn("Production",80,FieldValueType.Number,font);
				query.AddColumn("Sched",80,FieldValueType.Number,font);
				query.AddColumn("Adj",80,FieldValueType.Number,font);
				query.AddColumn("Writeoff",80,FieldValueType.Number,font);
				query.AddColumn("Tot Prod",80,FieldValueType.Number,font);
				query.AddColumn("Pt Income",80,FieldValueType.Number,font);
				query.AddColumn("Ins Income",80,FieldValueType.Number,font);
				query.AddColumn("Tot Income",80,FieldValueType.Number,font);
				query.AddGroupSummaryField("Total Production (Production + Adjustments - Writeoffs): ","Writeoff","Tot Prod",SummaryOperation.Sum,new List<int>() { 2 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),104,20);
				query.AddGroupSummaryField("Total Income (Pt Income + Ins Income): ","Writeoff","Total Income",SummaryOperation.Sum,new List<int>() { 2 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),0,25);
			}
			else {
				query.AddGroupSummaryField("Total Production (Production + Adjustments - Writeoffs): ","Writeoff","Tot Prod",SummaryOperation.Sum,new List<int>() { 1 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),104,20);
				query.AddGroupSummaryField("Total Income (Pt Income + Ins Income): ","Writeoff","Total Income",SummaryOperation.Sum,new List<int>() { 1 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),0,25);
			}
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {
				return;
			}
			// display report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;//Allow running multiple reports.
		}

		private void RunAnnual(){
			if(Plugins.HookMethod(this,"FormRpProdInc.RunAnnual_Start",PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text))) {
				return;
			}
			if(checkAllProv.Checked) {
				for(int i=0;i<listProv.Items.Count;i++) {
					listProv.SetSelected(i,true);
				}
			}
			if(checkAllClin.Checked) {
				for(int i=0;i<listClin.Items.Count;i++) {
					listClin.SetSelected(i,true);
				}
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			List<Provider> listProvs=new List<Provider>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				listProvs.Add(ProviderC.ListShort[listProv.SelectedIndices[i]]);
			}
			List<Clinic> listClinics=new List<Clinic>();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(Security.CurUser.ClinicIsRestricted) {
						listClinics.Add(_listClinics[listClin.SelectedIndices[i]]);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							Clinic unassigned=new Clinic();
							unassigned.ClinicNum=0;
							unassigned.Description="Unassigned";
							listClinics.Add(unassigned);//Will have ClinicNum of 0 for our "Unassigned" needs.
						}
						else {
							listClinics.Add(_listClinics[listClin.SelectedIndices[i]-1]);//Minus 1 from the selected index
						}
					}
				}
			}
			DataSet ds=RpProdInc.GetAnnualData(dateFrom,dateTo,listProvs,listClinics,radioWriteoffPay.Checked,checkAllProv.Checked,checkAllClin.Checked);
			DataTable dt=ds.Tables["Total"];
			DataTable dtClinic=new DataTable();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				dtClinic=ds.Tables["Clinic"];
			}
			ReportComplex report=new ReportComplex(true,false);
			report.ReportName="AnnualP&I";
			report.AddTitle("Title",Lan.g(this,"Annual Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString());
			if(checkAllProv.Checked) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				string str="";
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i>0) {
						str+=", ";
					}
					str+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
				}
				report.AddSubTitle("Providers",str);
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(checkAllClin.Checked) {
					report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
				}
				else {
					string clinNames="";
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							clinNames+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							clinNames+=_listClinics[listClin.SelectedIndices[i]].Description;
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								clinNames+=Lan.g(this,"Unassigned");
							}
							else {
								clinNames+=_listClinics[listClin.SelectedIndices[i]-1].Description;//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics",clinNames);
				}
			}
			//setup query
			QueryObject query;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && checkClinicBreakdown.Checked) {
				query=report.AddQuery(dtClinic,"","Clinic",SplitByKind.Value,1,true);
			}
			else {
				query=report.AddQuery(dt,"","",SplitByKind.None,1,true);
			}
			// add columns to report
			query.AddColumn("Month",75,FieldValueType.String);
			query.AddColumn("Production",90,FieldValueType.Number);
			query.AddColumn("Adjustments",90,FieldValueType.Number);
			query.AddColumn("Writeoff",90,FieldValueType.Number);
			query.AddColumn("Tot Prod",90,FieldValueType.Number);
			query.AddColumn("Pt Income",90,FieldValueType.Number);
			query.AddColumn("Ins Income",90,FieldValueType.Number);
			query.AddColumn("Total Income",90,FieldValueType.Number);
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && listClin.SelectedIndices.Count>1 && checkClinicBreakdown.Checked) {
				//If more than one clinic selected, we want to add a table to the end of the report that totals all the clinics together.
				query=report.AddQuery(dt,"Totals","",SplitByKind.None,2,true);
				query.AddColumn("Month",75,FieldValueType.String);
				query.AddColumn("Production",90,FieldValueType.Number);
				query.AddColumn("Adjustments",90,FieldValueType.Number);
				query.AddColumn("Writeoff",90,FieldValueType.Number);
				query.AddColumn("Tot Prod",90,FieldValueType.Number);
				query.AddColumn("Pt Income",90,FieldValueType.Number);
				query.AddColumn("Ins Income",90,FieldValueType.Number);
				query.AddColumn("Total Income",90,FieldValueType.Number);
				query.AddGroupSummaryField("Total Production (Production + Adjustments - Writeoffs): ","Writeoff","Tot Prod",SummaryOperation.Sum,new List<int>() { 2 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),106,20);
				query.AddGroupSummaryField("Total Income (Pt Income + Ins Income): ","Writeoff","Total Income",SummaryOperation.Sum,new List<int>() { 2 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),0,27);
			}
			else {
				query.AddGroupSummaryField("Total Production (Production + Adjustments - Writeoffs): ","Writeoff","Tot Prod",SummaryOperation.Sum,new List<int>() { 1 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),106,20);
				query.AddGroupSummaryField("Total Income (Pt Income + Ins Income): ","Writeoff","Total Income",SummaryOperation.Sum,new List<int>() { 1 },Color.Black,new Font("Tahoma",9,FontStyle.Bold),0,25);
			}
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {
				return;
			}
			// display report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;//Allow running multiple reports.
		}

		private void RunProvider() {
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			if(checkAllProv.Checked) {
				for(int i=0;i<listProv.Items.Count;i++) {
					listProv.SetSelected(i,true);
				}
			}
			if(checkAllClin.Checked) {
				for(int i=0;i<listClin.Items.Count;i++) {
					listClin.SetSelected(i,true);
				}
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			List<Provider> listProvs=new List<Provider>();
			for(int i=0;i<listProv.SelectedIndices.Count;i++) {
				listProvs.Add(ProviderC.ListShort[listProv.SelectedIndices[i]]);
			}
			List<Clinic> listClinics=new List<Clinic>();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(Security.CurUser.ClinicIsRestricted) {
						listClinics.Add(_listClinics[listClin.SelectedIndices[i]]);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							Clinic unassigned=new Clinic();
							unassigned.ClinicNum=0;
							unassigned.Description="Unassigned";
							listClinics.Add(unassigned);
						}
						else {
							listClinics.Add(_listClinics[listClin.SelectedIndices[i]-1]);//Minus 1 from the selected index
						}
					}
				}
			}
			DataSet ds=RpProdInc.GetProviderDataForClinics(dateFrom,dateTo,listProvs,listClinics,radioWriteoffPay.Checked,checkAllProv.Checked,checkAllClin.Checked);
			ReportComplex report=new ReportComplex(true,true);
			report.ReportName="Provider P&I";
			report.AddTitle("Title",Lan.g(this,"Provider Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString());
			if(checkAllProv.Checked) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				string str="";
				for(int i=0;i<listProv.SelectedIndices.Count;i++) {
					if(i>0) {
						str+=", ";
					}
					str+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
				}
				report.AddSubTitle("Providers",str);
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(checkAllClin.Checked) {
					report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
				}
				else {
					string clinNames="";
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							clinNames+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							clinNames+=_listClinics[listClin.SelectedIndices[i]].Description;
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								clinNames+=Lan.g(this,"Unassigned");
							}
							else {
								clinNames+=_listClinics[listClin.SelectedIndices[i]-1].Description;//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics",clinNames);
				}
			}
			//setup query
			QueryObject query;
			DataTable dtClinic=new DataTable();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				dtClinic=ds.Tables["Clinic"].Copy();
			}
			DataTable dt=ds.Tables["Total"].Copy();
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && checkClinicBreakdown.Checked) {
				query=report.AddQuery(dtClinic,"","Clinic",SplitByKind.Value,1,true);
			}
			else {
				query=report.AddQuery(dt,"","",SplitByKind.None,1,true);
			}
			// add columns to report
			query.AddColumn("Provider",75,FieldValueType.String);
			query.AddColumn("Production",120,FieldValueType.Number);
			query.AddColumn("Adjustments",120,FieldValueType.Number);
			query.AddColumn("Writeoff",120,FieldValueType.Number);
			query.AddColumn("Tot Prod",120,FieldValueType.Number);
			query.AddColumn("Pt Income",120,FieldValueType.Number);
			query.AddColumn("Ins Income",120,FieldValueType.Number);
			query.AddColumn("Total Income",120,FieldValueType.Number);
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && listClin.SelectedIndices.Count>1 && checkClinicBreakdown.Checked) {
				//If more than one clinic selected, we want to add a table to the end of the report that totals all the clinics together.
				query=report.AddQuery(dt,"Totals","",SplitByKind.None,2,true);
				query.AddColumn("Provider",75,FieldValueType.String);
				query.AddColumn("Production",120,FieldValueType.Number);
				query.AddColumn("Adjustments",120,FieldValueType.Number);
				query.AddColumn("Writeoff",120,FieldValueType.Number);
				query.AddColumn("Tot Prod",120,FieldValueType.Number);
				query.AddColumn("Pt Income",120,FieldValueType.Number);
				query.AddColumn("Ins Income",120,FieldValueType.Number);
				query.AddColumn("Total Income",120,FieldValueType.Number);
			}
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {//Does not actually submit queries because we use datatables in the central management tool.
				return;
			}
			// display the report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;//Allow running multiple reports.
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(  textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(!checkAllProv.Checked && listProv.SelectedIndices.Count==0){
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(!checkAllClin.Checked && listClin.SelectedIndices.Count==0) {
					MsgBox.Show(this,"At least one clinic must be selected.");
					return;
				}
			}
			dateFrom=PIn.Date(textDateFrom.Text);
			dateTo=PIn.Date(textDateTo.Text);
			if(dateTo<dateFrom) {
				MsgBox.Show(this,"To date cannot be before From date.");
				return;
			}
			if(radioDaily.Checked){
				RunDaily();
			}
			else if(radioMonthly.Checked){
				RunMonthly();
			}
			else if(radioAnnual.Checked) {
				RunAnnual();
			}
			else {//Provider
				RunProvider();
			}
			//DialogResult=DialogResult.OK;//Stay here so that a series of similar reports can be run
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	

	

		

		

		

		


		
	}
}








