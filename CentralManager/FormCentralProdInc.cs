using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental;
using OpenDental.ReportingComplex;
using System.Globalization;
using CodeBase;

namespace CentralManager {
	public partial class FormCentralProdInc:Form {
		private DateTime _dateFrom;
		private DateTime _dateTo;
		///<summary>Can be set externally when automating.</summary>
		public string DailyMonthlyAnnual;
		///<summary>If set externally, then this sets the date on startup.</summary>
		public DateTime DateStart;
		///<summary>If set externally, then this sets the date on startup.</summary>
		public DateTime DateEnd;
		/// <summary>Must be set externally.</summary>
		public List<CentralConnection> ConnList;
		public byte[] EncryptionKey;
		private Userod _userOld;
		private string _passwordTypedOld;


		public FormCentralProdInc() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormCentralProdInc_Load(object sender,System.EventArgs e) {
			_userOld=Security.CurUser;
			_passwordTypedOld=Security.PasswordTyped;
			textToday.Text=DateTime.Today.ToShortDateString();
			switch(DailyMonthlyAnnual) {
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
			//The CM tool runs against many databases thus does not care about default preferences.
			//If we enhance the CM tool to have default preferences, we will need to make sure that  the cache 
			//has been refreshed with the CM's cache instead of the potentially stale cache from an unknown source.
			//if(PrefC.GetBool(PrefName.ReportsPPOwriteoffDefaultToProcDate)) {
			//	radioWriteoffProc.Checked=true;
			//}
			if(DateStart.Year>1880) {
				textDateFrom.Text=DateStart.ToShortDateString();
				textDateTo.Text=DateEnd.ToShortDateString();
				switch(DailyMonthlyAnnual) {
					case "Daily":
						//RunDaily();
						break;
					case "Monthly":
						//RunMonthly();
						break;
					case "Annual":
						RunAnnual();
						break;
				}
				Close();
			}
		}

		private void radioDaily_Click(object sender,System.EventArgs e) {
			SetDates();
		}

		private void radioMonthly_Click(object sender,System.EventArgs e) {
			SetDates();
		}

		private void radioAnnual_Click(object sender,System.EventArgs e) {
			SetDates();
		}

		private void radioProvider_Click(object sender,EventArgs e) {
			SetDates();
		}

		private void SetDates() {
			if(radioDaily.Checked || radioProvider.Checked) {
				textDateFrom.Text=DateTime.Today.ToShortDateString();
				textDateTo.Text=DateTime.Today.ToShortDateString();
				butThis.Text=Lan.g(this,"Today");
			}
			else if(radioMonthly.Checked) {
				textDateFrom.Text=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToShortDateString();
				textDateTo.Text=new DateTime(DateTime.Today.Year,DateTime.Today.Month
					,DateTime.DaysInMonth(DateTime.Today.Year,DateTime.Today.Month)).ToShortDateString();
				butThis.Text=Lan.g(this,"This Month");
			}
			else {//annual
				textDateFrom.Text=new DateTime(DateTime.Today.Year,1,1).ToShortDateString();
				textDateTo.Text=new DateTime(DateTime.Today.Year,12,31).ToShortDateString();
				butThis.Text=Lan.g(this,"This Year");
			}
		}

		private void butThis_Click(object sender,System.EventArgs e) {
			SetDates();
		}

		private void butLeft_Click(object sender,System.EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			_dateFrom=PIn.Date(textDateFrom.Text);
			_dateTo=PIn.Date(textDateTo.Text);
			if(radioDaily.Checked || radioProvider.Checked) {
				textDateFrom.Text=_dateFrom.AddDays(-1).ToShortDateString();
				textDateTo.Text=_dateTo.AddDays(-1).ToShortDateString();
			}
			else if(radioMonthly.Checked) {
				bool toLastDay=false;
				if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(_dateTo.Year,_dateTo.Month)==_dateTo.Day) {
					toLastDay=true;
				}
				textDateFrom.Text=_dateFrom.AddMonths(-1).ToShortDateString();
				textDateTo.Text=_dateTo.AddMonths(-1).ToShortDateString();
				_dateTo=PIn.Date(textDateTo.Text);
				if(toLastDay) {
					textDateTo.Text=new DateTime(_dateTo.Year,_dateTo.Month,
						CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(_dateTo.Year,_dateTo.Month))
						.ToShortDateString();
				}
			}
			else {//annual
				textDateFrom.Text=_dateFrom.AddYears(-1).ToShortDateString();
				textDateTo.Text=_dateTo.AddYears(-1).ToShortDateString();
			}
		}

		private void butRight_Click(object sender,System.EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			_dateFrom=PIn.Date(textDateFrom.Text);
			_dateTo=PIn.Date(textDateTo.Text);
			if(radioDaily.Checked || radioProvider.Checked) {
				textDateFrom.Text=_dateFrom.AddDays(1).ToShortDateString();
				textDateTo.Text=_dateTo.AddDays(1).ToShortDateString();
			}
			else if(radioMonthly.Checked) {
				bool toLastDay=false;
				if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(_dateTo.Year,_dateTo.Month)==_dateTo.Day) {
					toLastDay=true;
				}
				textDateFrom.Text=_dateFrom.AddMonths(1).ToShortDateString();
				textDateTo.Text=_dateTo.AddMonths(1).ToShortDateString();
				_dateTo=PIn.Date(textDateTo.Text);
				if(toLastDay) {
					textDateTo.Text=new DateTime(_dateTo.Year,_dateTo.Month,
						CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(_dateTo.Year,_dateTo.Month))
						.ToShortDateString();
				}
			}
			else {//annual
				textDateFrom.Text=_dateFrom.AddYears(1).ToShortDateString();
				textDateTo.Text=_dateTo.AddYears(1).ToShortDateString();
			}
		}

		private void RunAnnual() {
			_dateFrom=PIn.Date(textDateFrom.Text);
			_dateTo=PIn.Date(textDateTo.Text);
			List<DataSet> listProdData=new List<DataSet>();
			List<string> listServerNames=new List<string>();
			List<long> listProvNums=new List<long>();
			List<long> listClinicNums=new List<long>();
			string computerName="";
			string database="";
			string user="";
			string mySqlPassword="";
			string strFailedConn="";
			string webServiceURI="";
			string odPassword="";
			for(int i=0;i<ConnList.Count;i++) {
				if(ConnList[i].DatabaseName!="") {
					//ServerName=localhost DatabaseName=opendental MySqlUser=root MySqlPassword=
					computerName=ConnList[i].ServerName;
					database=ConnList[i].DatabaseName;
					user=ConnList[i].MySqlUser;
					if(ConnList[i].MySqlPassword!="") {
						mySqlPassword=CentralConnections.Decrypt(ConnList[i].MySqlPassword,EncryptionKey);
					}
					RemotingClient.ServerURI="";
				}
				else if(ConnList[i].ServiceURI!="") {
					webServiceURI=ConnList[i].ServiceURI;
					RemotingClient.ServerURI=webServiceURI;
					try {
						odPassword=CentralConnections.Decrypt(ConnList[i].OdPassword,EncryptionKey);
						Security.CurUser=Security.LogInWeb(ConnList[i].OdUser,odPassword,"",Application.ProductVersion,ConnList[i].WebServiceIsEcw);
						Security.PasswordTyped=odPassword;
					}
					catch {
						//Not sure if anything needs to be here
					}
				}
				else {
					MessageBox.Show("Either a database or a Middle Tier URI must be specified in the connection.");
					return;
				}
				DataConnection.DBtype=DatabaseType.MySql;
				OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
				try {
					if(RemotingClient.ServerURI!="") {
						RemotingClient.RemotingRole=RemotingRole.ClientWeb;
					}
					else {
						dcon.SetDb(computerName,database,user,mySqlPassword,"","",DataConnection.DBtype);
						RemotingClient.RemotingRole=RemotingRole.ClientDirect;
					}
					Cache.RefreshCache(((int)InvalidType.AllLocal).ToString());
				}
				catch(Exception ex) {
					if(strFailedConn=="") {
						strFailedConn+="Some connections could not successfully be created for this report:\r\n";
					}
					if(RemotingClient.ServerURI!="") {
						strFailedConn+="WebService: "+webServiceURI+"\r\n";
					}
					else {
						strFailedConn+="Server: "+computerName+" DataBase: "+database+"\r\n";
					}
					continue;
				}
				listProvNums=new List<long>();
				for(int j=0;j<ProviderC.ListShort.Count;j++) {
					listProvNums.Add(ProviderC.ListShort[j].ProvNum);
				}
				listClinicNums=new List<long>();
				listClinicNums.Add(0);//Unassigned
				for(int j=0;j<Clinics.List.Length;j++) {
					listClinicNums.Add(Clinics.List[j].ClinicNum);
				}
				listProdData.Add(RpProdInc.GetAnnualData(_dateFrom,_dateTo,listProvNums,listClinicNums,radioWriteoffPay.Checked,true,true));
				if(ConnList[i].ServiceURI!="") {
					listServerNames.Add(ConnList[i].ServiceURI);
				}
				else {
					listServerNames.Add(ConnList[i].ServerName+", "+ConnList[i].DatabaseName);
				}
				//Cleaning up after WebService connections.
				Security.CurUser=null;
				Security.PasswordTyped=null;
			}
			ReportComplex report=new ReportComplex(true,true);
			report.ReportName="Appointments";
			report.AddTitle("Title",Lan.g(this,"Annual Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",_dateFrom.ToShortDateString()+" - "+_dateTo.ToShortDateString());
			report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
			//setup query
			QueryObject query;
			DataSet dsTotal=new DataSet();
			for(int i=0;i<listProdData.Count;i++) {
				DataTable dt=listProdData[i].Tables["Clinic"];
				DataTable dtTot=listProdData[i].Tables["Total"].Copy();
				dtTot.TableName=dtTot.TableName+"_"+i;
				dsTotal.Tables.Add(dtTot);
				query=report.AddQuery(dt,listServerNames[i],"Clinic",SplitByKind.Value,1,true);
				// add columns to report
				query.AddColumn("Month",75,FieldValueType.String);
				query.AddColumn("Production",120,FieldValueType.Number);
				query.AddColumn("Adjustments",120,FieldValueType.Number);
				query.AddColumn("Writeoff",120,FieldValueType.Number);
				query.AddColumn("Tot Prod",120,FieldValueType.Number);
				query.AddColumn("Pt Income",120,FieldValueType.Number);
				query.AddColumn("Ins Income",120,FieldValueType.Number);
				query.AddColumn("Total Income",120,FieldValueType.Number);
			}
			if(dsTotal.Tables.Count==0) {
				MsgBox.Show(this,"This report returned no values");
				return;
			}
			DataTable dtTotal;
			decimal production;
			decimal adjust;
			decimal inswriteoff;	//spk 5/19/05
			decimal totalproduction;
			decimal ptincome;
			decimal insincome;
			decimal totalincome;
			DateTime[] dates=new DateTime[_dateTo.Month-_dateFrom.Month+1];
			dtTotal=dsTotal.Tables[0].Clone();
			for(int i=0;i<dates.Length;i++) {//usually 12 months in loop
				dates[i]=_dateFrom.AddMonths(i);
				DataRow row=dtTotal.NewRow();
				row[0]=dates[i].ToString("MMM yy");//JAN 14
				production=0;
				adjust=0;
				inswriteoff=0;	//spk 5/19/05
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				for(int j=0;j<dsTotal.Tables.Count;j++) {
					for(int k=0;k<dsTotal.Tables[j].Rows.Count;k++) {
						if(dsTotal.Tables[j].Rows[k][0].ToString()==dates[i].ToString("MMM yyyy")) {
							production+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Production"].ToString());
							adjust+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Adjustments"].ToString());
							inswriteoff-=PIn.Decimal(dsTotal.Tables[j].Rows[k]["WriteOff"].ToString());
							ptincome+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Pt Income"].ToString());
							insincome+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Ins Income"].ToString());
						}
					}
				}
				totalproduction=production+adjust+inswriteoff;
				totalincome=ptincome+insincome;
				row[1]=production.ToString("n");
				row[2]=adjust.ToString("n");
				row[3]=inswriteoff.ToString("n");
				row[4]=totalproduction.ToString("n");
				row[5]=ptincome.ToString("n");
				row[6]=insincome.ToString("n");
				row[7]=totalincome.ToString("n");
				dtTotal.Rows.Add(row);
			}
			query=report.AddQuery(dtTotal,"Totals","",SplitByKind.None,2,true);
			query.AddColumn("Month",75,FieldValueType.String);
			query.AddColumn("Production",120,FieldValueType.Number);
			query.AddColumn("Adjustments",120,FieldValueType.Number);
			query.AddColumn("Writeoff",120,FieldValueType.Number);
			query.AddColumn("Tot Prod",120,FieldValueType.Number);
			query.AddColumn("Pt Income",120,FieldValueType.Number);
			query.AddColumn("Ins Income",120,FieldValueType.Number);
			query.AddColumn("Total Income",120,FieldValueType.Number);
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {//Does not actually submit queries because we use datatables in the central management tool.
				return;
			}
			if(strFailedConn!="") {
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(strFailedConn);
				msgBoxCP.ShowDialog();
			}
			// display the report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void RunProvider() {
			_dateFrom=PIn.Date(textDateFrom.Text);
			_dateTo=PIn.Date(textDateTo.Text);
			List<DataSet> listProdData=new List<DataSet>();
			List<string> listServerNames=new List<string>();
			List<long> listProvNums=new List<long>();
			List<long> listClinicNums=new List<long>();
			string computerName="";
			string database="";
			string user="";
			string mySqlPassword="";
			string strFailedConn="";
			string webServiceURI="";
			string odPassword="";
			for(int i=0;i<ConnList.Count;i++) {
				if(ConnList[i].DatabaseName!="") {
					//ServerName=localhost DatabaseName=opendental MySqlUser=root MySqlPassword=
					computerName=ConnList[i].ServerName;
					database=ConnList[i].DatabaseName;
					user=ConnList[i].MySqlUser;
					if(ConnList[i].MySqlPassword!="") {
						mySqlPassword=CentralConnections.Decrypt(ConnList[i].MySqlPassword,EncryptionKey);
					}
					RemotingClient.ServerURI="";
				}
				else if(ConnList[i].ServiceURI!="") {
					webServiceURI=ConnList[i].ServiceURI;
					RemotingClient.ServerURI=webServiceURI;
					try {
						odPassword=CentralConnections.Decrypt(ConnList[i].OdPassword,EncryptionKey);
						Security.CurUser=Security.LogInWeb(ConnList[i].OdUser,odPassword,"",Application.ProductVersion,ConnList[i].WebServiceIsEcw);
						Security.PasswordTyped=odPassword;
					}
					catch {
						//Not sure if anything needs to be here
					}
				}
				else {
					MessageBox.Show("Either a database or a Middle Tier URI must be specified in the connection.");
					return;
				}
				DataConnection.DBtype=DatabaseType.MySql;
				OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
				try {
					if(RemotingClient.ServerURI!="") {
						RemotingClient.RemotingRole=RemotingRole.ClientWeb;
					}
					else {
						dcon.SetDb(computerName,database,user,mySqlPassword,"","",DataConnection.DBtype);
						RemotingClient.RemotingRole=RemotingRole.ClientDirect;
					}
					Cache.RefreshCache(((int)InvalidType.AllLocal).ToString());
				}
				catch(Exception ex) {
					if(strFailedConn=="") {
						strFailedConn+="Some connections could not successfully be created for this report:\r\n";
					}
					if(RemotingClient.ServerURI!="") {
						strFailedConn+="WebService: "+webServiceURI+"\r\n";
					}
					else {
						strFailedConn+="Server: "+computerName+" DataBase: "+database+"\r\n";
					}
					continue;
				}
				listProvNums=new List<long>();
				for(int j=0;j<ProviderC.ListShort.Count;j++) {
					listProvNums.Add(ProviderC.ListShort[j].ProvNum);
				}
				listClinicNums=new List<long>();
				listClinicNums.Add(0);//Unassigned
				for(int j=0;j<Clinics.List.Length;j++) {
					listClinicNums.Add(Clinics.List[j].ClinicNum);
				}
				listProdData.Add(RpProdInc.GetProviderDataForClinics(_dateFrom,_dateTo,listProvNums,listClinicNums,radioWriteoffPay.Checked,true,true));
				if(ConnList[i].ServiceURI!="") {
					listServerNames.Add(ConnList[i].ServiceURI);
				}
				else {
					listServerNames.Add(ConnList[i].ServerName+", "+ConnList[i].DatabaseName);
				}
				//Cleaning up after WebService connections.
				Security.CurUser=null;
				Security.PasswordTyped=null;
			}
			ReportComplex report=new ReportComplex(true,true);
			report.ReportName="Provider P&I";
			report.AddTitle("Title",Lan.g(this,"Provider Production and Income"));
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",_dateFrom.ToShortDateString()+" - "+_dateTo.ToShortDateString());
			report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
			//setup query
			QueryObject query;
			DataSet dsTotal=new DataSet();
			for(int i=0;i<listProdData.Count;i++) {
				DataTable dt=listProdData[i].Tables["Clinic"];
				DataTable dtTot=listProdData[i].Tables["Total"].Copy();
				dtTot.TableName=dtTot.TableName+"_"+i;
				dsTotal.Tables.Add(dtTot);
				query=report.AddQuery(dt,listServerNames[i],"Clinic",SplitByKind.Value,1,true);
				// add columns to report
				query.AddColumn("Provider",75,FieldValueType.String);
				query.AddColumn("Production",120,FieldValueType.Number);
				query.AddColumn("Adjustments",120,FieldValueType.Number);
				query.AddColumn("Writeoff",120,FieldValueType.Number);
				query.AddColumn("Tot Prod",120,FieldValueType.Number);
				query.AddColumn("Pt Income",120,FieldValueType.Number);
				query.AddColumn("Ins Income",120,FieldValueType.Number);
				query.AddColumn("Total Income",120,FieldValueType.Number);
			}
			if(dsTotal.Tables.Count==0) {
				MsgBox.Show(this,"This report returned no values");
				return;
			}
			DataTable dtTotal;
			decimal production;
			decimal adjust;
			decimal inswriteoff;
			decimal totalproduction;
			decimal ptincome;
			decimal insincome;
			decimal totalincome;
			dtTotal=dsTotal.Tables[0].Clone();
			for(int i=0;i<listProvNums.Count;i++) {
				Provider provCur=Providers.GetProv(listProvNums[i]);
				DataRow row=dtTotal.NewRow();
				row[0]=provCur.Abbr;
				production=0;
				adjust=0;
				inswriteoff=0;
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				bool hasAnyAmount=false;
				for(int j=0;j<dsTotal.Tables.Count;j++) {
					for(int k=0;k<dsTotal.Tables[j].Rows.Count;k++) {
						if(dsTotal.Tables[j].Rows[k][0].ToString()==provCur.Abbr
							&& (PIn.Decimal(dsTotal.Tables[j].Rows[k]["Production"].ToString())!=0
							|| PIn.Decimal(dsTotal.Tables[j].Rows[k]["Adjustments"].ToString())!=0
							|| PIn.Decimal(dsTotal.Tables[j].Rows[k]["WriteOff"].ToString())!=0
							|| PIn.Decimal(dsTotal.Tables[j].Rows[k]["Pt Income"].ToString())!=0
							|| PIn.Decimal(dsTotal.Tables[j].Rows[k]["Ins Income"].ToString())!=0)) 
						{
							production+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Production"].ToString());
							adjust+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Adjustments"].ToString());
							inswriteoff+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["WriteOff"].ToString());//Writeoffs stored as negative number
							ptincome+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Pt Income"].ToString());
							insincome+=PIn.Decimal(dsTotal.Tables[j].Rows[k]["Ins Income"].ToString());
							hasAnyAmount=true;
						}
					}
				}
				totalproduction=production+adjust+inswriteoff;
				totalincome=ptincome+insincome;
				row[1]=production.ToString("n");
				row[2]=adjust.ToString("n");
				row[3]=inswriteoff.ToString("n");
				row[4]=totalproduction.ToString("n");
				row[5]=ptincome.ToString("n");
				row[6]=insincome.ToString("n");
				row[7]=totalincome.ToString("n");
				if(hasAnyAmount) {
					dtTotal.Rows.Add(row);
				}
			}
			query=report.AddQuery(dtTotal,"Totals","",SplitByKind.None,2,true);
			query.AddColumn("Provider",75,FieldValueType.String);
			query.AddColumn("Production",120,FieldValueType.Number);
			query.AddColumn("Adjustments",120,FieldValueType.Number);
			query.AddColumn("Writeoff",120,FieldValueType.Number);
			query.AddColumn("Tot Prod",120,FieldValueType.Number);
			query.AddColumn("Pt Income",120,FieldValueType.Number);
			query.AddColumn("Ins Income",120,FieldValueType.Number);
			query.AddColumn("Total Income",120,FieldValueType.Number);
			report.AddPageNum();
			// execute query
			if(!report.SubmitQueries()) {//Does not actually submit queries because we use datatables in the central management tool.
				return;
			}
			if(strFailedConn!="") {
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(strFailedConn);
				msgBoxCP.ShowDialog();
			}
			// display the report
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			_dateFrom=PIn.Date(textDateFrom.Text);
			_dateTo=PIn.Date(textDateTo.Text);
			if(_dateTo<_dateFrom) {
				MsgBox.Show(this,"To date cannot be before From date.");
				return;
			}
			if(radioDaily.Checked) {
				//RunDaily();
			}
			else if(radioMonthly.Checked) {
				//RunMonthly();
			}
			else if(radioAnnual.Checked) {
				if(_dateFrom.AddYears(1) <= _dateTo || _dateFrom.Year != _dateTo.Year) {
					MsgBox.Show(this,"Date range for annual report cannot be greater than one year and must be within the same year.");
					return;
				}
				Cursor=Cursors.WaitCursor;
				RunAnnual();
				Cursor=Cursors.Default;
			}
			else {//Provider
				Cursor=Cursors.WaitCursor;
				RunProvider();
				Cursor=Cursors.Default;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormCentralProdInc_FormClosing(object sender,FormClosingEventArgs e) {
			//This window can potentially change the username and password and we want to put them back to what they were before the window was launched.
			Security.CurUser=_userOld;
			Security.PasswordTyped=_passwordTypedOld;
		}

	}
}