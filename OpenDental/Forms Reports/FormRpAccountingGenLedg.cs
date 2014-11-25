using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.ReportingComplex;

namespace OpenDental{
///<summary></summary>
	public class FormRpAccountingGenLedg:System.Windows.Forms.Form {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.MonthCalendar date2;
		private System.Windows.Forms.MonthCalendar date1;
		private System.Windows.Forms.Label labelTO;
		private UI.Button butTest;
		private System.ComponentModel.Container components = null;
		//private FormQuery FormQuery2;

		///<summary></summary>
		public FormRpAccountingGenLedg() {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpAccountingGenLedg));
			this.date2 = new System.Windows.Forms.MonthCalendar();
			this.date1 = new System.Windows.Forms.MonthCalendar();
			this.labelTO = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butTest = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// date2
			// 
			this.date2.Location = new System.Drawing.Point(285, 36);
			this.date2.MaxSelectionCount = 1;
			this.date2.Name = "date2";
			this.date2.TabIndex = 2;
			// 
			// date1
			// 
			this.date1.Location = new System.Drawing.Point(31, 36);
			this.date1.MaxSelectionCount = 1;
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
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(534, 291);
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
			this.butOK.Location = new System.Drawing.Point(534, 256);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butTest
			// 
			this.butTest.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butTest.Autosize = true;
			this.butTest.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTest.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTest.CornerRadius = 4F;
			this.butTest.Location = new System.Drawing.Point(534, 222);
			this.butTest.Name = "butTest";
			this.butTest.Size = new System.Drawing.Size(75, 26);
			this.butTest.TabIndex = 23;
			this.butTest.Text = "Test Report";
			this.butTest.Click += new System.EventHandler(this.butTest_Click);
			// 
			// FormRpAccountingGenLedg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(649, 330);
			this.Controls.Add(this.butTest);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.date2);
			this.Controls.Add(this.date1);
			this.Controls.Add(this.labelTO);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpAccountingGenLedg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "General Ledger Report";
			this.Load += new System.EventHandler(this.FormRpAccountingGenLedg_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpAccountingGenLedg_Load(object sender, System.EventArgs e) {
			if(!PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				butTest.Visible=false;
			}
			if(DateTime.Today.Month>6){//default to this year
				date1.SelectionStart=new DateTime(DateTime.Today.Year,1,1);
				date2.SelectionStart=new DateTime(DateTime.Today.Year,12,31);
			}
			else{//default to last year
				date1.SelectionStart=new DateTime(DateTime.Today.Year-1,1,1);
				date2.SelectionStart=new DateTime(DateTime.Today.Year-1,12,31);
			}
		}

		private void butTest_Click(object sender,EventArgs e) {
			string queryString="DROP TABLE IF EXISTS tempstartingbals; "
				+"CREATE TABLE tempstartingbals "
				+"SELECT Description,AcctType,ROUND(SUM(DebitAmt-CreditAmt),2) SumTotal,account.AccountNum "
        +"FROM account, journalentry "
        +"WHERE account.AccountNum=journalentry.AccountNum "
        +"AND DateDisplayed &lt; '2007-01-01' "
        +"AND (AcctType=0 OR AcctType=1 OR AcctType=2)/*assetes,liablities,equity*/ "
        +"GROUP BY account.AccountNum; "
        +"SELECT DATE('2006-12-31') DateDisplayed,SumTotal  DebitAmt,0 CreditAmt,'' Memo,'' Splits,'' CheckNumber,Description,AcctType,AccountNum "
        +"FROM tempstartingbals "
        +"UNION ALL "
        +"SELECT DateDisplayed, Memo, DebitAmt, CreditAmt, Splits, CheckNumber, account.Description, AcctType, account.AccountNum  "
        +"FROM account "
        +"LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum "
        +"AND DateDisplayed &gt;= '2007-01-01' "
        +"AND DateDisplayed &lt;= '2007-12-31' "
        +"WHERE account.AcctType= 0 OR account.AcctType=1 OR account.AcctType=2 "
        +"UNION ALL "
        +"SELECT DateDisplayed, Memo, DebitAmt, CreditAmt, Splits, CheckNumber, account.Description, AcctType, account.AccountNum "
        +"FROM account "
        +"LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum "
        +"AND DateDisplayed &gt;= '2007-01-01' "
        +"AND DateDisplayed &lt;= '2007-12-31' "
        +"WHERE account.AcctType= 3 OR account.AcctType=4 "
        +"ORDER BY AcctType, Description, DateDisplayed; "
        +"DROP TABLE IF EXISTS tempstartingbals;";
			//create the report
			ReportComplex report=new ReportComplex("General Ledger","",true,true,false);
			report.ReportName="General Ledger";
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",date1.SelectionStart.ToShortDateString()+" - "+date2.SelectionStart.ToShortDateString());
			//setup query
			QueryObject query;
			query=report.AddQuery(queryString,"","Description",SplitByKind.Value,1,true);
			// add columns to report
			query.AddColumn("Date",75,FieldValueType.Date);
			query.GetColumnDetail("Date").SuppressIfDuplicate = true;
			query.GetColumnDetail("Date").FormatString="d";
			query.AddColumn("Patient",175,FieldValueType.String);
			query.AddColumn("Age",45,FieldValueType.Age);
			query.AddColumn("Time",65,FieldValueType.Date);
			query.GetColumnDetail("Time").FormatString="t";
			query.GetColumnDetail("Time").TextAlign = ContentAlignment.MiddleRight;
			query.GetColumnHeader("Time").TextAlign = ContentAlignment.MiddleRight;
			query.AddColumn("Length",60,FieldValueType.Integer);
			query.GetColumnHeader("Length").Location=new Point(
				query.GetColumnHeader("Length").Location.X,
				query.GetColumnHeader("Length").Location.Y);
			query.GetColumnHeader("Length").TextAlign = ContentAlignment.MiddleCenter;
			query.GetColumnDetail("Length").TextAlign = ContentAlignment.MiddleCenter;
			query.GetColumnDetail("Length").Location=new Point(
				query.GetColumnDetail("Length").Location.X,
				query.GetColumnDetail("Length").Location.Y);
			query.AddColumn("Description",170,FieldValueType.String);
			query.AddColumn("Home Ph.",120,FieldValueType.String);
			query.AddColumn("Work Ph.",120,FieldValueType.String);
			query.AddColumn("Cell Ph.",120,FieldValueType.String);
			query.ReportObjects.Add(new ReportObject("Buffer","Group Footer",new Point(0,0),new Size(1,50),"",new Font("Tahoma",9),ContentAlignment.MiddleCenter));
			report.AddPageNum();
			report.AddGridLines();
			// execute query
			if(!report.SubmitQueries()) {
				return;
			}
			// display report
			FormReportComplex FormR=new FormReportComplex(report);
			//FormR.MyReport=report;
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			FormReportForRdl FormR=new FormReportForRdl();
			string s=Properties.Resources.ReportAccountingGenLedger;
			s=s.Replace("1/1/2007 - 12/31/2007",date1.SelectionStart.ToShortDateString()+" - "+date2.SelectionStart.ToShortDateString());
			s=s.Replace("2007-01-01",POut.Date(date1.SelectionStart,false));
			s=s.Replace("2007-12-31",POut.Date(date2.SelectionStart,false));
			s=s.Replace("2006-12-31",POut.Date(date1.SelectionStart.AddDays(-1),false));
			FormR.SourceRdlString=s;
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;		
		}

		

		

		

	}
}
