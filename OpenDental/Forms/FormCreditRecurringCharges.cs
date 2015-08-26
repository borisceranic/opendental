using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormCreditRecurringCharges:Form {
		private DataTable table;
		private PrintDocument pd;
		private int pagesPrinted;
		private int headingPrintH;
		private bool headingPrinted;
		private bool insertPayment;
		private Program _program;
		private DateTime nowDateTime;
		private string xPath;
		private int _success;
		private int _failed;

		///<summary>Only works for XCharge and PayConnect so far.</summary>
		public FormCreditRecurringCharges() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRecurringCharges_Load(object sender,EventArgs e) {
			if(Programs.IsEnabled(ProgramName.PayConnect)) {
				_program=Programs.GetCur(ProgramName.PayConnect);
			}
			else if(Programs.IsEnabled(ProgramName.Xcharge)) {
				_program=Programs.GetCur(ProgramName.Xcharge);
				xPath=Programs.GetProgramPath(_program);
				if(!File.Exists(xPath)) {//program path is invalid
					//if user has setup permission and they want to edit the program path, show the X-Charge setup window
					if(Security.IsAuthorized(Permissions.Setup)
						&& MsgBox.Show(this,MsgBoxButtons.YesNo,"The X-Charge path is not valid.  Would you like to edit the path?"))
					{
						FormXchargeSetup FormX=new FormXchargeSetup();
						FormX.ShowDialog();
						if(FormX.DialogResult==DialogResult.OK) {
							//The user could have correctly enabled the X-Charge bridge, we need to update our local _programCur and _xPath variable2
							_program=Programs.GetCur(ProgramName.Xcharge);
							xPath=Programs.GetProgramPath(_program);
						}
					}
					//if the program path still does not exist, whether or not they attempted to edit the program link, tell them to edit and close the form
					if(!File.Exists(xPath)) {
						MsgBox.Show(this,"The X-Charge program path is not valid.  Edit the program link in order to use the CC Recurring Charges feature.");
						Close();
						return;
					}
				}
			}
			if(_program==null) {
				MsgBox.Show(this,"The PayConnect or X-Charge program link must be enabled in order to use the CC Recurring Charges feature.");
				Close();
				return;
			}
			//X-Charge or PayConnect is enabled and if X-Charge is enabled the path to the X-Charge executable is valid
			nowDateTime=MiscData.GetNowDateTime();
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			FillGrid();
			gridMain.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void FillGrid() {
			table=CreditCards.GetRecurringChargeList();
			table.Columns.Add("RepeatChargeAmt");
			Dictionary<long,double> dictFamBals=new Dictionary<long,double>();//Keeps track of the family balance for each patient
			//Calculate the repeat charge amount and the amount to be charged for each credit card
			for(int i=table.Rows.Count-1;i>-1;i--) {//loop through backwards since we may remove rows if the charge amount is <=0 or patCur==null
				Double famBalTotal=PIn.Double(table.Rows[i]["FamBalTotal"].ToString());
				Double rptChargeAmt;
				long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
				string procedures=table.Rows[i]["Procedures"].ToString();
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					table.Rows.RemoveAt(i);
					continue;
				}
				if(Prefs.IsODHQ()) {//HQ calculates repeating charges based on the presence of procedures on the patient's account that are linked to the CC
					if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
						rptChargeAmt=CreditCards.TotalRecurringCharges(patNum,procedures,patCur.BillingCycleDay);
					}
					else {
						rptChargeAmt=CreditCards.TotalRecurringCharges(patNum,procedures,PIn.Date(table.Rows[i]["DateStart"].ToString()).Day);
					}
					if(table.Rows[i]["PayOrder"].ToString()=="2") {//Repeat charge is from a payment plan
						famBalTotal+=rptChargeAmt;
						rptChargeAmt+=PIn.Double(table.Rows[i]["ChargeAmt"].ToString());
					}
				}
				else {//non-HQ calculates repeating charges by the ChargeAmt on the credit card
					rptChargeAmt=PIn.Double(table.Rows[i]["ChargeAmt"].ToString());
				}
				if(!dictFamBals.ContainsKey(patCur.Guarantor)) {
					dictFamBals.Add(patCur.Guarantor,famBalTotal);
				}
				Double chargeAmt=Math.Min(dictFamBals[patCur.Guarantor],rptChargeAmt);//Charges the lesser of the family balance and the repeat charge amount
				if(chargeAmt<=0) {
					table.Rows.RemoveAt(i);
					continue;
				}
				table.Rows[i]["ChargeAmt"]=chargeAmt;
				table.Rows[i]["RepeatChargeAmt"]=rptChargeAmt;
				dictFamBals[patCur.Guarantor]-=chargeAmt;//Decrease so the sum of repeating charges on all cards is not greater than the family balance
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRecurring","PatNum"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Name"),270);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Total Bal"),100,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","RepeatingAmt"),100,HorizontalAlignment.Right);//RptChrgAmt
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","ChargeAmt"),100,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			OpenDental.UI.ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new OpenDental.UI.ODGridRow();
				Double famBalTotal=PIn.Double(table.Rows[i]["FamBalTotal"].ToString());
				Double chargeAmt=PIn.Double(table.Rows[i]["ChargeAmt"].ToString());
				Double rptChargeAmt=PIn.Double(table.Rows[i]["RepeatChargeAmt"].ToString());
				row.Cells.Add(table.Rows[i]["PatNum"].ToString());
				row.Cells.Add(table.Rows[i]["PatName"].ToString());
				row.Cells.Add(famBalTotal.ToString("c"));
				row.Cells.Add(rptChargeAmt.ToString("c"));
				row.Cells.Add(chargeAmt.ToString("c"));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			labelTotal.Text=Lan.g(this,"Total=")+gridMain.Rows.Count.ToString();
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		///<summary>Returns a valid DateTime for the payment's PayDate.  Contains logic if payment should be for the previous or the current month.</summary>
		private DateTime GetPayDate(DateTime latestPayment,DateTime dateStart) {
			//Most common, current day >= dateStart so we use current month and year with the dateStart day.  Will always be a legal DateTime.
			if(nowDateTime.Day>=dateStart.Day) {
				return new DateTime(nowDateTime.Year,nowDateTime.Month,dateStart.Day);
			}
			//If not enough days in current month to match the dateStart see if on the last day in the month.
			//Example: dateStart=08/31/2009 and month is February 28th so we need the PayDate to be today not for last day on the last month, which would happen below.
			int daysInMonth=DateTime.DaysInMonth(nowDateTime.Year,nowDateTime.Month);
			if(daysInMonth<=dateStart.Day && daysInMonth==nowDateTime.Day) {
				return nowDateTime;//Today is last day of the month so return today as the PayDate.
			}
			//PayDate needs to be for the previous month so we need to determine if using the dateStart day would be a legal DateTime.
			DateTime nowMinusOneMonth=nowDateTime.AddMonths(-1);
			daysInMonth=DateTime.DaysInMonth(nowMinusOneMonth.Year,nowMinusOneMonth.Month);
			if(daysInMonth<=dateStart.Day) {
				return new DateTime(nowMinusOneMonth.Year,nowMinusOneMonth.Month,daysInMonth);//Returns the last day of the previous month.
			}
			return new DateTime(nowMinusOneMonth.Year,nowMinusOneMonth.Month,dateStart.Day);//Previous month contains a legal date using dateStart's day.
		}

		///<summary>Tests the selected indicies with newly calculated pay dates.  If there's a date violation, a warning shows and false is returned.</summary>
		private bool PaymentsWithinLockDate() {
			//Check if user has the payment create permission in the first place to save time.
			if(!Security.IsAuthorized(Permissions.PaymentCreate,nowDateTime.Date)) {
				return false;
			}
			List<string> warnings=new List<string>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				//Calculate what the new pay date will be.
				DateTime newPayDate=GetPayDate(PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["LatestPayment"].ToString()),
			      PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["DateStart"].ToString()));
				//Test if the user can create a payment with the new pay date.
				if(!Security.IsAuthorized(Permissions.PaymentCreate,newPayDate,true)) {
					if(warnings.Count==0) {
						warnings.Add("Lock date limitation is preventing the recurring charges from running:");
					}
					warnings.Add(newPayDate.ToShortDateString()+" - "
						+table.Rows[i]["PatNum"].ToString()+": "
						+table.Rows[i]["PatName"].ToString()+" - "
						+PIn.Double(table.Rows[i]["FamBalTotal"].ToString()).ToString("c")+" - "
						+PIn.Double(table.Rows[i]["ChargeAmt"].ToString()).ToString("c"));
				}
			}
			if(warnings.Count>0) {
				string msg="";
				for(int i=0;i<warnings.Count;i++) {
					if(i>0) {
						msg+="\r\n";
					}
					msg+=warnings[i];
				}
				//Show the warning message.  This allows the user the ability to unhighlight rows or go change the date limitation.
				MessageBox.Show(msg);
				return false;
			}
			return true;
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}
		
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Must select at least one recurring charge.");
				return;
			}
			long patNum=PIn.Long(table.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
		}

		private void butPrintList_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;
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
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"CreditCard recurring charges list printed")) {
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
				text=Lan.g(this,"Recurring Charges");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
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
			}
			g.Dispose();
		}

		private void butAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void SendXCharge() {
			StringBuilder strBuilderResultFile=new StringBuilder();
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			string user=ProgramProperties.GetPropVal(_program.ProgramNum,"Username");
			string password=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_program.ProgramNum,"Password"));
			string resultfile=ODFileUtils.CombinePaths(Path.GetDirectoryName(xPath),"XResult.txt");
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				if(table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString()!="" &&
					CreditCards.IsDuplicateXChargeToken(table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString()))
				{
					MessageBox.Show(Lan.g(this,"A duplicate token was found, the card cannot be charged for customer: ")+table.Rows[i]["PatName"].ToString());
					continue;
				}
				insertPayment=false;
				ProcessStartInfo info=new ProcessStartInfo(xPath);
				long patNum=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					continue;
				}
				try {
					File.Delete(resultfile);//delete the old result file.
				}
				catch {
					//Probably did not have permissions to delete the file.  Don't do anything, because a message will show telling them that the cards left in the grid failed.
					//They will then go try and run the cards in the Account module and will then get a detailed message telling them what is wrong.
					continue;
				}
				info.Arguments="";
				double amt=PIn.Double(table.Rows[gridMain.SelectedIndices[i]]["ChargeAmt"].ToString());
				DateTime exp=PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["CCExpiration"].ToString());
				string address=PIn.String(table.Rows[gridMain.SelectedIndices[i]]["Address"].ToString());
				string addressPat=PIn.String(table.Rows[gridMain.SelectedIndices[i]]["AddressPat"].ToString());
				string zip=PIn.String(table.Rows[gridMain.SelectedIndices[i]]["Zip"].ToString());
				string zipPat=PIn.String(table.Rows[gridMain.SelectedIndices[i]]["ZipPat"].ToString());
				info.Arguments+="/AMOUNT:"+amt.ToString("F2")+" /LOCKAMOUNT ";
				info.Arguments+="/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE ";
				if(table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString()!="") {
					info.Arguments+="/XCACCOUNTID:"+table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString()+" ";
					info.Arguments+="/RECURRING ";
				}
				else {
					info.Arguments+="/ACCOUNT:"+table.Rows[gridMain.SelectedIndices[i]]["CCNumberMasked"].ToString()+" ";
				}
				if(exp.Year>1880) {
					info.Arguments+="/EXP:"+exp.ToString("MMyy")+" ";
				}
				if(address!="") {
					info.Arguments+="\"/ADDRESS:"+address+"\" ";
				}
				else if(addressPat!="") {
					info.Arguments+="\"/ADDRESS:"+addressPat+"\" ";
				}
				//If ODHQ, do not add the zip code if the customer has an active foreign registration key
				bool hasForeignKey=false;
				if(Prefs.IsODHQ()) {
					hasForeignKey=RegistrationKeys.GetForPatient(patNum)
						.Where(x => x.IsForeign)
						.Where(x => x.DateStarted<=DateTimeOD.Today)
						.Where(x => x.DateEnded.Year<1880 || x.DateEnded>=DateTimeOD.Today)
						.Where(x => x.DateDisabled.Year<1880 || x.DateDisabled>=DateTimeOD.Today)
						.Count()>0;
				}
				if(zip!="" && !hasForeignKey) {
					info.Arguments+="\"/ZIP:"+zip+"\" ";
				}
				else if(zipPat!="" && !hasForeignKey) {
					info.Arguments+="\"/ZIP:"+zipPat+"\" ";
				}
				info.Arguments+="/RECEIPT:Pat"+patNum+" ";//aka invoice#
				info.Arguments+="\"/CLERK:"+Security.CurUser.UserName+" R\" /LOCKCLERK ";
				info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
				info.Arguments+="/USERID:"+user+" ";
				info.Arguments+="/PASSWORD:"+password+" ";
				info.Arguments+="/HIDEMAINWINDOW ";
				info.Arguments+="/AUTOPROCESS ";
				info.Arguments+="/SMALLWINDOW ";
				info.Arguments+="/AUTOCLOSE ";
				info.Arguments+="/NORESULTDIALOG ";
				Cursor=Cursors.WaitCursor;
				Process process=new Process();
				process.StartInfo=info;
				process.EnableRaisingEvents=true;
				process.Start();
				while(!process.HasExited) {
					Application.DoEvents();
				}
				Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
				Cursor=Cursors.Default;
				string line="";
				StringBuilder strBuilderResultText=new StringBuilder();
				strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+table.Rows[i]["PatName"].ToString());
				try {
					using(TextReader reader=new StreamReader(resultfile)) {
						line=reader.ReadLine();
						while(line!=null) {
							strBuilderResultText.AppendLine(line);
							if(line.StartsWith("RESULT=")) {
								if(line=="RESULT=SUCCESS") {
									_success++;
									labelCharged.Text=Lan.g(this,"Charged=")+_success;
									insertPayment=true;
								}
								else {
									_failed++;
									labelFailed.Text=Lan.g(this,"Failed=")+_failed;
								}
							}
							line=reader.ReadLine();
						}
						strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
						strBuilderResultFile.AppendLine();
					}
				}
				catch {
					continue;//Cards will still be in the list if something went wrong.
				}
				if(insertPayment) {
					CreatePayment(patCur,i,strBuilderResultText.ToString());
				}
			}
			try {
				File.WriteAllText(ODFileUtils.CombinePaths(Path.GetDirectoryName(xPath),"RecurringChargeResult.txt"),strBuilderResultFile.ToString());
			}
			catch { } //Do nothing cause this is just for internal use.
		}

		private void SendPayConnect() {
			StringBuilder strBuilderResultFile=new StringBuilder();
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			#region Card Charge Loop
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				string tokenOrCCMasked=table.Rows[gridMain.SelectedIndices[i]]["PayConnectToken"].ToString();
				if(tokenOrCCMasked!="" && CreditCards.IsDuplicatePayConnectToken(tokenOrCCMasked)) {
					MessageBox.Show(Lan.g(this,"A duplicate token was found, the card cannot be charged for customer: ")+table.Rows[i]["PatName"].ToString());
					continue;
				}
				long patNum=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					continue;
				}
				DateTime exp=PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["PayConnectTokenExp"].ToString());
				if(tokenOrCCMasked=="") {
					tokenOrCCMasked=table.Rows[gridMain.SelectedIndices[i]]["CCNumberMasked"].ToString();
					exp=PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["CCExpiration"].ToString());
				}
				decimal amt=PIn.Decimal(table.Rows[gridMain.SelectedIndices[i]]["ChargeAmt"].ToString());
				string zip=PIn.String(table.Rows[gridMain.SelectedIndices[i]]["Zip"].ToString());
				//request a PayConnect token, if a token was already saved PayConnect will return the same token,
				//otherwise replace CCNumberMasked with the returned token if the sale successful
				PayConnectService.creditCardRequest payConnectRequest=PayConnect.BuildSaleRequest(amt,tokenOrCCMasked,exp.Year,exp.Month,
					patCur.GetNameFLnoPref(),"",zip,null,PayConnectService.transType.SALE,"",true);
				PayConnectService.transResponse payConnectResponse=PayConnect.ProcessCreditCard(payConnectRequest);
				StringBuilder strBuilderResultText=new StringBuilder();//this payment's result text, used in payment note and then appended to file string builder
				strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+patCur.GetNameFLnoPref());
				if(payConnectResponse==null) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					strBuilderResultText.AppendLine(Lan.g(this,"Transaction Failed, unkown error"));
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
					continue;
				}
				else if(payConnectResponse.Status.code!=0) {//error in transaction
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					strBuilderResultText.AppendLine(Lan.g(this,"Transaction Type")+": "+PayConnectService.transType.SALE.ToString());
					strBuilderResultText.AppendLine(Lan.g(this,"Status")+": "+payConnectResponse.Status.description);
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
					continue;
				}
				//approved sale, update CC, add result to file string builder, and create payment
				_success++;
				labelCharged.Text=Lan.g(this,"Charged=")+_success;
				//Update the credit card token values if sale approved
				CreditCard ccCur=CreditCards.GetOne(PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["CreditCardNum"].ToString()));
				PayConnectService.expiration payConnectExp=payConnectResponse.PaymentToken.Expiration;
				//if stored CC token or token expiration are different than those returned by PayConnect, update the stored CC
				if(ccCur.PayConnectToken!=payConnectResponse.PaymentToken.TokenId
					|| ccCur.PayConnectTokenExp.Year!=payConnectExp.year
					|| ccCur.PayConnectTokenExp.Month!=payConnectExp.month)
				{
					ccCur.PayConnectToken=payConnectResponse.PaymentToken.TokenId;
					ccCur.PayConnectTokenExp=new DateTime(payConnectExp.year,payConnectExp.month,DateTime.DaysInMonth(payConnectExp.year,payConnectExp.month));
					ccCur.CCNumberMasked=ccCur.PayConnectToken.Substring(ccCur.PayConnectToken.Length-4).PadLeft(ccCur.PayConnectToken.Length,'X');
					ccCur.CCExpiration=ccCur.PayConnectTokenExp;
					CreditCards.Update(ccCur);
				}
				//add to strbuilder that will be written to txt file
				strBuilderResultText.AppendLine("RESULT="+payConnectResponse.Status.description);
				strBuilderResultText.AppendLine("TRANS TYPE="+PayConnectService.transType.SALE.ToString());
				strBuilderResultText.AppendLine("AUTH CODE="+payConnectResponse.AuthCode);
				strBuilderResultText.AppendLine("ENTRY=MANUAL");
				strBuilderResultText.AppendLine("CLERK="+Security.CurUser.UserName);
				strBuilderResultText.AppendLine("TRANSACTION NUMBER="+payConnectResponse.RefNumber);
				strBuilderResultText.AppendLine("ACCOUNT="+ccCur.CCNumberMasked);//XXXXXXXXXXXX1234, all but last four numbers of the token replaced with X's
				strBuilderResultText.AppendLine("EXPIRATION="+payConnectResponse.PaymentToken.Expiration.month.ToString().PadLeft(2,'0')
					+(payConnectResponse.PaymentToken.Expiration.year%100));
				strBuilderResultText.AppendLine("CARD TYPE="+PayConnect.GetCardType(tokenOrCCMasked).ToString());
				strBuilderResultText.AppendLine("AMOUNT="+payConnectRequest.Amount.ToString("F2"));
				CreatePayment(patCur,i,strBuilderResultText.ToString());
				strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
			}
			#endregion Card Charge Loop
			if(PrefC.GetBool(PrefName.AtoZfolderUsed)) {
				try {
					string payConnectResultDir=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"PayConnect");
					if(!Directory.Exists(payConnectResultDir)) {
						Directory.CreateDirectory(payConnectResultDir);
					}
					File.WriteAllText(ODFileUtils.CombinePaths(payConnectResultDir,"RecurringChargeResult.txt"),strBuilderResultFile.ToString());
				}
				catch { } //Do nothing cause this is just for internal use.
			}
		}

		///<summary>Inserts a payment and paysplit, called after processing a payment through either X-Charge or PayConnect. indexCur is the current index
		///of the gridMain row this payment is for.</summary>
		private void CreatePayment(Patient patCur,int indexCur,string note) {
			Payment paymentCur=new Payment();
			paymentCur.DateEntry=nowDateTime.Date;
			paymentCur.PayDate=GetPayDate(PIn.Date(table.Rows[gridMain.SelectedIndices[indexCur]]["LatestPayment"].ToString()),
				PIn.Date(table.Rows[gridMain.SelectedIndices[indexCur]]["DateStart"].ToString()));
			paymentCur.PatNum=patCur.PatNum;
			paymentCur.ClinicNum=PIn.Long(table.Rows[gridMain.SelectedIndices[indexCur]]["ClinicNum"].ToString());
			paymentCur.PayType=PIn.Int(ProgramProperties.GetPropVal(_program.ProgramNum,"PaymentType"));
			paymentCur.PayAmt=PIn.Double(table.Rows[gridMain.SelectedIndices[indexCur]]["ChargeAmt"].ToString());
			paymentCur.PayNote=note;
			paymentCur.IsRecurringCC=true;
			Payments.Insert(paymentCur);
			long provNum=PIn.Long(table.Rows[gridMain.SelectedIndices[indexCur]]["ProvNum"].ToString());//for payment plans only
			if(provNum==0) {//Regular payments need to apply to the provider that the family owes the most money to.
				DataTable dt=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
				double highestAmt=0;
				for(int j=0;j<dt.Rows.Count;j++) {
					double afterIns=PIn.Double(dt.Rows[j]["AfterIns"].ToString());
					if(highestAmt>=afterIns) {
						continue;
					}
					highestAmt=afterIns;
					provNum=PIn.Long(dt.Rows[j]["ProvNum"].ToString());
				}
			}
			PaySplit split=new PaySplit();
			split.PatNum=paymentCur.PatNum;
			split.ClinicNum=paymentCur.ClinicNum;
			split.PayNum=paymentCur.PayNum;
			split.ProcDate=paymentCur.PayDate;
			split.DatePay=paymentCur.PayDate;
			split.ProvNum=provNum;
			split.SplitAmt=paymentCur.PayAmt;
			split.PayPlanNum=PIn.Long(table.Rows[gridMain.SelectedIndices[indexCur]]["PayPlanNum"].ToString());
			PaySplits.Insert(split);
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				Ledgers.ComputeAging(patCur.Guarantor,PrefC.GetDate(PrefName.DateLastAging),false);
			}
			else {
				Ledgers.ComputeAging(patCur.Guarantor,DateTimeOD.Today,false);
				if(PrefC.GetDate(PrefName.DateLastAging)!=DateTime.Today) {
					Prefs.UpdateString(PrefName.DateLastAging,POut.Date(DateTime.Today,false));
					//Since this is always called from UI, the above line works fine to keep the prefs cache current.
				}
			}
		}

		///<summary>Will process payments for all authorized charges for each CC stored and marked for recurring charges.  X-Charge or PayConnect must be
		///enabled.  Program validation done on load and if properties are not valid the form will close and exit.</summary>
		private void butSend_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Must select at least one recurring charge.");
				return;
			}
			if(!PaymentsWithinLockDate()) {
				return;
			}
			_success=0;
			_failed=0;
			if(_program.ProgName==ProgramName.Xcharge.ToString()) {
				SendXCharge();
			}
			else if(_program.ProgName==ProgramName.PayConnect.ToString()) {
				SendPayConnect();
			}
			FillGrid();
			labelCharged.Text=Lan.g(this,"Charged=")+_success;
			labelFailed.Text=Lan.g(this,"Failed=")+_failed;
			MsgBox.Show(this,"Done charging cards.\r\nIf there are any patients remaining in list, print the list and handle each one manually.");
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

	}
}