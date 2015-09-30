using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
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
		private Program prog;
		private DateTime nowDateTime;
		private string xPath;

		///<summary>Only works for XCharge so far.</summary>
		public FormCreditRecurringCharges() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRecurringCharges_Load(object sender,EventArgs e) {
			if(!Prefs.IsODHQ()) {
				checkHideBold.Checked=true;
				checkHideBold.Visible=false;
			}
			nowDateTime=MiscData.GetNowDateTime();
			prog=Programs.GetCur(ProgramName.Xcharge);
			xPath=Programs.GetProgramPath(prog);
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			FillGrid();
			gridMain.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void FillGrid() {
			//Currently only working for X-Charge. If more added then move this check out of FillGrid.
			#region XCharge Check
			if(prog==null){
				MsgBox.Show(this,"X-Charge entry is missing from the database.");//should never happen
				return;
			}
			if(!prog.Enabled) {
				if(Security.IsAuthorized(Permissions.Setup)) {
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
					if(FormX.DialogResult==DialogResult.OK) {
						//The user could have correctly enabled the X-Charge bridge, we need to update our local prog variable.
						prog=Programs.GetCur(ProgramName.Xcharge);
					}
				}
				return;
			}
			if(!File.Exists(xPath)) {
				MsgBox.Show(this,"Path is not valid.");
				if(Security.IsAuthorized(Permissions.Setup)){
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
					if(FormX.DialogResult==DialogResult.OK) {
						//The user could have correctly enabled the X-Charge bridge, we need to update our local prog variable.
						prog=Programs.GetCur(ProgramName.Xcharge);
					}
				}
				return;
			}
			#endregion
			table=CreditCards.GetRecurringChargeList();
			table.Columns.Add("RepeatChargeAmt");
			Dictionary<long,double> dictFamBals=new Dictionary<long,double>();//Keeps track of the family balance for each patient
			//Calculate the repeat charge amount and the amount to be charged for each credit card
			for(int i=table.Rows.Count-1;i>-1;i--) {//loop through backwards since we may remove rows if the charge amount is <=0 or patCur==null
				Double famBalTotal=PIn.Double(table.Rows[i]["FamBalTotal"].ToString());
				Double rptChargeAmt;
				//will be 0 if this is not a payplan row, if negative don't subtract from the FamBalTotal
				Double payPlanDue=Math.Max(PIn.Double(table.Rows[i]["PayPlanDue"].ToString()),0);
				long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
				string procedures=table.Rows[i]["Procedures"].ToString();
				if(Prefs.IsODHQ()) {//HQ calculates repeating charges based on the presence of procedures on the patient's account that are linked to the CC
					if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
						rptChargeAmt=CreditCards.TotalRecurringCharges(patNum,procedures,PIn.Int(table.Rows[i]["BillingCycleDay"].ToString()));
					}
					else {
						rptChargeAmt=CreditCards.TotalRecurringCharges(patNum,procedures,PIn.Date(table.Rows[i]["DateStart"].ToString()).Day);
					}
					rptChargeAmt+=payPlanDue;//payPlanDue will be 0 if this is not a payplan row.  If negative amount due on payplan, payPlanDue is set to 0 above
				}
				else {//non-HQ calculates repeating charges by the ChargeAmt on the credit card which is the sum of repeat charge and payplan payment amount
					rptChargeAmt=PIn.Double(table.Rows[i]["ChargeAmt"].ToString());
				}
				//the Total Bal column should display the famBalTotal plus payPlanDue on the attached payplan if there is one with a positive amount due
				//if the payplan has a negative amount due, it is set to 0 above and does not subtract from famBalTotal
				//if the account balance is negative, the Total Bal column should still display the entire amount due on the payplan (if >0)
				//if the account balance is negative and there is no payplan, the Total Bal column will be the negative account balance
				if(payPlanDue>0) {//if there is a payplan attached to this repeatcharge and a positive amount due
					famBalTotal=Math.Max(famBalTotal,0)+payPlanDue;//if famBalTotal<0 then famBalTotal=0+payPlanDue, else famBalTotal=famBalTotal+payPlanDue
				}
				long guarNum=PIn.Long(table.Rows[i]["Guarantor"].ToString());
				//if guarantor is already in the dict and this is a payplan charge row, add the payPlanDue to fambal so the patient is charged
				if(dictFamBals.ContainsKey(guarNum) && payPlanDue>0) {
					dictFamBals[guarNum]=Math.Max(dictFamBals[guarNum],0)+payPlanDue;//this way the payplan charge will be charged even if the fam bal is < 0
				}
				if(!dictFamBals.ContainsKey(guarNum)) {
					dictFamBals.Add(guarNum,famBalTotal);
				}
				//Charge the lesser of famBalTotal (includes payPlanDue) and the repeat charge amount (includes payPlanDue)
				Double chargeAmt=Math.Min(dictFamBals[guarNum],rptChargeAmt);
				if(chargeAmt<=0) {
					table.Rows.RemoveAt(i);
					continue;
				}
				table.Rows[i]["ChargeAmt"]=chargeAmt;
				table.Rows[i]["RepeatChargeAmt"]=rptChargeAmt;
				dictFamBals[guarNum]-=chargeAmt;//Decrease so the sum of repeating charges on all cards is not greater than the family balance
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRecurring","PatNum"),55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Name"),255);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Family Bal"),90,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","PayPlan Due"),90,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Total Due"),90,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Repeating Amt"),100,HorizontalAlignment.Right);//RptChrgAmt
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecurring","Charge Amt"),100,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			OpenDental.UI.ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new OpenDental.UI.ODGridRow();
				Double famBalTotal=PIn.Double(table.Rows[i]["FamBalTotal"].ToString());//pat bal+payplan due, but if pat bal<0 and payplan due>0 then just payplan due
				Double payPlanDue=PIn.Double(table.Rows[i]["PayPlanDue"].ToString());
				Double chargeAmt=PIn.Double(table.Rows[i]["ChargeAmt"].ToString());
				Double rptChargeAmt=PIn.Double(table.Rows[i]["RepeatChargeAmt"].ToString());//includes repeat charge (from procs if ODHQ) and attached payplan
				row.Cells.Add(table.Rows[i]["PatNum"].ToString());
				row.Cells.Add(table.Rows[i]["PatName"].ToString());
				row.Cells.Add(famBalTotal.ToString("c"));
				if(payPlanDue!=0) {
					row.Cells.Add(payPlanDue.ToString("c"));
					//negative family balance does not subtract from payplan amount due and negative payplan amount due does not subtract from family balance due
					row.Cells.Add((Math.Max(famBalTotal,0)+Math.Max(payPlanDue,0)).ToString("c"));
				}
				else {
					row.Cells.Add("");
					row.Cells.Add(famBalTotal.ToString("c"));
				}
				row.Cells.Add(rptChargeAmt.ToString("c"));
				row.Cells.Add(chargeAmt.ToString("c"));
				if(!checkHideBold.Checked) {
					double diff=(Math.Max(famBalTotal,0)+Math.Max(payPlanDue,0))-rptChargeAmt;
					if(diff.IsZero()) {
						//don't bold anything
					}
					else if(diff>0) {
						row.Cells[5].Bold=YN.Yes;//"Repeating Amt"
						row.Cells[6].Bold=YN.Yes;//"Charge Amt"
					}
					else if(diff<0) {
						row.Cells[4].Bold=YN.Yes;//"Total Due"
						row.Cells[6].Bold=YN.Yes;//"Charge Amt"
					}
				}
				row.Tag=PIn.Long(table.Rows[i]["CreditCardNum"].ToString());
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
			Cursor=Cursors.WaitCursor;
			List<long> listSelectedCCNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listSelectedCCNums.Add((long)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
			}
			FillGrid();
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(listSelectedCCNums.Contains((long)gridMain.Rows[i].Tag)) {
					gridMain.SetSelected(i,true);
				}
			}
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
			Cursor=Cursors.Default;
		}

		private void checkHideBold_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			List<long> listSelectedCCNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listSelectedCCNums.Add((long)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
			}
			FillGrid();
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(listSelectedCCNums.Contains((long)gridMain.Rows[i].Tag)) {
					gridMain.SetSelected(i,true);
				}
			}
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
			Cursor=Cursors.Default;
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

		private void butSend_Click(object sender,EventArgs e) {
			//Assuming the use of XCharge.  If adding another vendor (PayConnect for example)
			//make sure to move XCharge validation in FillGrid() to here.
			if(prog==null) {//Gets filled in FillGrid()
				return;
			}
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Must select at least one recurring charge.");
				return;
			}
			if(!PaymentsWithinLockDate()) {
				return;
			}
			string recurringResultFile="Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString()+"\r\n\r\n";
			int failed=0;
			int success=0;
			string user=ProgramProperties.GetPropVal(prog.ProgramNum,"Username");
			string password=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.ProgramNum,"Password"));
			#region Card Charge Loop
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				#region X-Charge
				if(table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString()!="" &&
					CreditCards.IsDuplicateXChargeToken(table.Rows[gridMain.SelectedIndices[i]]["XChargeToken"].ToString())) 
				{
					MessageBox.Show(Lan.g(this,"A duplicate token was found, the card cannot be charged for customer: ")+table.Rows[i]["PatName"].ToString());
					continue;
				}
				insertPayment=false;
				ProcessStartInfo info=new ProcessStartInfo(xPath);
				long patNum=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				string resultfile=Path.Combine(Path.GetDirectoryName(xPath),"XResult.txt");
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
				string resultText="";
				recurringResultFile+="PatNum: "+patNum+" Name: "+table.Rows[i]["PatName"].ToString()+"\r\n";
				try {
					using(TextReader reader=new StreamReader(resultfile)) {
						line=reader.ReadLine();
						while(line!=null) {
							if(resultText!="") {
								resultText+="\r\n";
							}
							resultText+=line;
							if(line.StartsWith("RESULT=")) {
								if(line=="RESULT=SUCCESS") {
									success++;
									labelCharged.Text=Lan.g(this,"Charged=")+success;
									insertPayment=true;
								}
								else {
									failed++;
									labelFailed.Text=Lan.g(this,"Failed=")+failed;
								}
							}
							line=reader.ReadLine();
						}
						recurringResultFile+=resultText+"\r\n\r\n";
					}
				}
				catch {
					continue;//Cards will still be in the list if something went wrong.
				}
				#endregion
				if(insertPayment) {
					Patient patCur=Patients.GetPat(patNum);
					Payment paymentCur=new Payment();
					paymentCur.DateEntry=nowDateTime.Date;
					DateTime dateStart=PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["DateStart"].ToString());
					if(Prefs.IsODHQ() && PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
						dateStart=new DateTime(dateStart.Year,dateStart.Month,PIn.Int(table.Rows[gridMain.SelectedIndices[i]]["BillingCycleDay"].ToString()));
					}
					paymentCur.PayDate=GetPayDate(PIn.Date(table.Rows[gridMain.SelectedIndices[i]]["LatestPayment"].ToString()),dateStart);
					paymentCur.PatNum=patCur.PatNum;
					paymentCur.ClinicNum=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["ClinicNum"].ToString());
					paymentCur.PayType=PIn.Int(ProgramProperties.GetPropVal(prog.ProgramNum,"PaymentType"));
					paymentCur.PayAmt=amt;
					double payPlanDue=PIn.Double(table.Rows[gridMain.SelectedIndices[i]]["PayPlanDue"].ToString());
					paymentCur.PayNote=resultText;
					paymentCur.IsRecurringCC=true;
					Payments.Insert(paymentCur);
					long provNumPayPlan=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["ProvNum"].ToString());//for payment plans only
					//Regular payments need to apply to the provider that the family owes the most money to.
					//Also get provNum for provider owed the most if the card is for a payplan and for other repeating charges and they will be charged for both
					//the payplan and regular repeating charges
					long provNumRegPmts=0;
					if(provNumPayPlan==0 || paymentCur.PayAmt-payPlanDue>0) {//provNum==0 for cards not attached to a payplan.
						DataTable dt=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
						double highestAmt=0;
						for(int j=0;j<dt.Rows.Count;j++) {
							double afterIns=PIn.Double(dt.Rows[j]["AfterIns"].ToString());
							if(highestAmt>=afterIns) {
								continue;
							}
							highestAmt=afterIns;
							provNumRegPmts=PIn.Long(dt.Rows[j]["ProvNum"].ToString());
						}
					}
					PaySplit split=new PaySplit();
					split.PatNum=paymentCur.PatNum;
					split.ClinicNum=paymentCur.ClinicNum;
					split.PayNum=paymentCur.PayNum;
					split.ProcDate=paymentCur.PayDate;
					split.DatePay=paymentCur.PayDate;
					split.PayPlanNum=PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PayPlanNum"].ToString());
					if(split.PayPlanNum==0) {//this row is not for a payplan
						split.SplitAmt=paymentCur.PayAmt;
						paymentCur.PayAmt-=split.SplitAmt;
						split.ProvNum=provNumRegPmts;
						split.ClinicNum=patCur.ClinicNum;
					}
					else {//row includes a payplan amount due, could also include a regular repeating pay amount as part of the total charge amount
						split.SplitAmt=payPlanDue;
						paymentCur.PayAmt-=split.SplitAmt;//subtract the payplan pay amount from the total payment amount and create another split not attached to payplan
						split.ProvNum=provNumPayPlan;
					}
					PaySplits.Insert(split);
					//if the above split was for a payment plan and there is still some PayAmt left, insert another split not attached to the payplan
					if(paymentCur.PayAmt>0) {
						split=new PaySplit();
						split.PatNum=paymentCur.PatNum;
						split.ClinicNum=patCur.ClinicNum;
						split.PayNum=paymentCur.PayNum;
						split.ProcDate=paymentCur.PayDate;
						split.DatePay=paymentCur.PayDate;
						split.ProvNum=provNumRegPmts;
						split.SplitAmt=paymentCur.PayAmt;
						split.PayPlanNum=0;
						PaySplits.Insert(split);
					}
					if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
						Ledgers.ComputeAging(patCur.Guarantor,PrefC.GetDate(PrefName.DateLastAging),false);
					}
					else {
						Ledgers.ComputeAging(patCur.Guarantor,DateTimeOD.Today,false);
						if(PrefC.GetDate(PrefName.DateLastAging) != DateTime.Today) {
							Prefs.UpdateString(PrefName.DateLastAging,POut.Date(DateTime.Today,false));
							//Since this is always called from UI, the above line works fine to keep the prefs cache current.
						}
					}
				}
			}
			#endregion
			try {
				File.WriteAllText(Path.Combine(Path.GetDirectoryName(xPath),"RecurringChargeResult.txt"),recurringResultFile);
			}
			catch { } //Do nothing cause this is just for internal use.
			FillGrid();
			labelCharged.Text=Lan.g(this,"Charged=")+success;
			labelFailed.Text=Lan.g(this,"Failed=")+failed;
			MsgBox.Show(this,"Done charging cards.\r\nIf there are any patients remaining in list, print the list and handle each one manually.");
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

	}
}