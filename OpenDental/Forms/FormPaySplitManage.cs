using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPaySplitManage:Form {
		///<summary>List of current paysplits for this payment.</summary>
		private List<PaySplit> _listSplitsCur;
		///<summary>Table of outstanding charges on the family account</summary>
		private static DataTable _tableCharges;
		///<summary>Amount currently available for paying off charges.</summary>
		private double _payAvailableCur;
		///<summary>The amount entered for the current payment. May be changed in this window.</summary>
		public double PaymentAmt;
		public long ClinicNum;
		public Patient[] ArrayPatsForFam;
		public Payment PaymentCur;
		public DateTime PayDate;
		public bool IsNew;

		public FormPaySplitManage() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPaySplitManage_Load(object sender,EventArgs e) {
			//Make sure amounts remain consistant as things get deleted or created.
			//On OK click submit all paysplits.
			_tableCharges=new DataTable();
			_payAvailableCur=PaymentAmt;
			textPayAmt.Text=PaymentAmt.ToString("f");
			if(IsNew) {
				_listSplitsCur=AutoSplitForPayment(ArrayPatsForFam,PaymentCur.PayNum,ref _payAvailableCur,PayDate,ClinicNum);
				textSplitAmt.Text=(PaymentAmt-_payAvailableCur).ToString("f");//Amount Paid Increases as PayAmt decreases.
			}
			else {
				textSplitAmt.Text=POut.Double(PaymentCur.PayAmt);
				double diff=_payAvailableCur-PaymentCur.PayAmt;//Old payment. They may have added funds to the payment for some reason, which will be reflected in PayAmt but not in PaymentCur.
				if(diff>0) {//If they increased the amount of the old payment, they want to be able to use it. 
					_payAvailableCur=diff;
				}
				else {//If they decreased the amount of the old payment, don't let them assign any new charges to this payment (but they can certainly take some off).
					_payAvailableCur=0;
				}
				double amt=0;
				//We want to fill the charge table like we usually do, but not make any new splits since this is an old payment.
				AutoSplitForPayment(ArrayPatsForFam,PaymentCur.PayNum,ref amt,PayDate,ClinicNum);//With the splits already made, their charges will be marked paid.
				_listSplitsCur=PaySplits.GetForPayment(PaymentCur.PayNum);
			}
			FillChargeGrid();
		}

		///<summary>Fills charge grid.</summary>
		private void FillChargeGrid() {
			//Fill right-hand grid with all the charges, filtered based on checkbox.
			textAmtAvailable.Text=_payAvailableCur.ToString("f");
			gridCharges.BeginUpdate();
			gridCharges.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),65);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),40);
			gridCharges.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
				col=new ODGridColumn(Lan.g(this,"Clinic"),40);
				gridCharges.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Patient"),75);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),95);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),50,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Paid"),10);
			gridCharges.Columns.Add(col);
			gridCharges.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableCharges.Rows.Count;i++) {
				if(PIn.Double(_tableCharges.Rows[i]["Amount"].ToString())==0 && !checkShowPaid.Checked) {//Filter out those that have been paid if checkShowPaid is unchecked.
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(_tableCharges.Rows[i]["Date"].ToString());//Date
				row.Cells.Add(_tableCharges.Rows[i]["Prov"].ToString());//Provider
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					row.Cells.Add(_tableCharges.Rows[i]["Clinic"].ToString());
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(_tableCharges.Rows[i]["Patient"].ToString());
				row.Cells.Add(_tableCharges.Rows[i]["Type"].ToString());//Type
				if(_tableCharges.Rows[i]["Type"].ToString()=="Proc") {
					//Get the proc and add its description if the row is a proc.
					Procedure proc=Procedures.GetOneProc(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()),false);
					string procDesc=Procedures.GetDescription(proc);
					row.Cells[row.Cells.Count-1].Text+=": "+procDesc;
					row.Tag=proc;//Used later when making splits for this charge
				}
				else if(_tableCharges.Rows[i]["Type"].ToString()=="PayPlanCharge"){
					PayPlanCharge charge=PayPlanCharges.GetOne(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()));
					row.Tag=charge;//Used later when making splits for this charge
				}
				else{
					Adjustment adjust=Adjustments.GetOne(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()));
					row.Tag=adjust;//Used later when making splits for this charge
				}
				row.Cells.Add(PIn.Double(_tableCharges.Rows[i]["Amount"].ToString()).ToString("f"));//Amount
				if(PIn.Double(_tableCharges.Rows[i]["Amount"].ToString())==0) {
					row.Cells.Add("X");//Paid
				}
				gridCharges.Rows.Add(row);
			}
			gridCharges.EndUpdate();
			FillSplitGrid();
		}

		///<summary>Fills the paysplit grid.</summary>
		private void FillSplitGrid() {
			//Fill left grid with paysplits created, highlight procs on right.
			gridSplits.BeginUpdate();
			gridSplits.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),40);
			gridSplits.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
				col=new ODGridColumn(Lan.g(this,"Clinic"),40);
				gridSplits.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Patient"),80);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),50,HorizontalAlignment.Right);
			gridSplits.Columns.Add(col);
			gridSplits.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listSplitsCur.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listSplitsCur[i].DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(_listSplitsCur[i].ProvNum));//Prov
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					if(_listSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetClinic(_listSplitsCur[i].ClinicNum).Description);//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				row.Cells.Add(Patients.GetPat(_listSplitsCur[i].PatNum).GetNameFL());//Patient
				if(_listSplitsCur[i].ProcNum==0 && _listSplitsCur[i].PayPlanNum!=0) {
					row.Cells.Add("PayPlanCharge");//Type
				}
				else if(_listSplitsCur[i].ProcNum!=0) {
					Procedure proc=Procedures.GetOneProc(_listSplitsCur[i].ProcNum,false);
					string procDesc=Procedures.GetDescription(proc);
					row.Cells.Add("Proc: "+procDesc);//Type
				}
				else {//Unattached split for a positive adjustment
					row.Cells.Add("Adjustment");//Type
				}
				row.Cells.Add(_listSplitsCur[i].SplitAmt.ToString("f"));//Amount
				row.Tag=_listSplitsCur[i];
				gridSplits.Rows.Add(row);
			}
			gridSplits.EndUpdate();
		}

		///<summary>Creates and paysplits associated to the patient passed in for the payment passed in until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  payAmt is passed as a ref so that we can do this same logic for an entire 
		///family while keeping track of the remaining pay amount.</summary>
		public static List<PaySplit> AutoSplitForPayment(Patient[] arrayPats,long payNum,ref double payAmt,DateTime date,long clinic) {
			//Get the lists of items we'll be using to calculate with.
			List<Procedure> listComplProcs=Procedures.GetCompleteForPats(arrayPats);//Ordered by ProcDdate
			List<Payment> listPayments=Payments.GetNonSplitForPats(arrayPats);//Ordered by PayDate
			List<Adjustment> listNegAdjustments=Adjustments.GetAdjustForPats(arrayPats,false);//Ordered by DateEntry
			List<Adjustment> listPosAdjustments=Adjustments.GetAdjustForPats(arrayPats,true);//Ordered by DateEntry
			List<PaySplit> listPaySplits=PaySplits.GetForPats(arrayPats);//Ordered by DateEntry, might contain payplan payments.
			List<PayPlan> listPayPlans=PayPlans.GetForPats(arrayPats);//Used to figure out how much we need to pay off procs with
			List<PayPlanCharge> listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0){
				listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans);//Ordered by ChargeDate, doesn't get charges for the future.
			}
			for(int i=0;i<listComplProcs.Count;i++) {//Getting the actual amount patient is responsible for.
				//Calculated using writeoffs, inspayest, inspayamt.  Done the same way ContrAcct does it.
				listComplProcs[i].ProcFee=Procedures.GetPatPortion(listComplProcs[i]);
			}
			//Now we have all negative and positive adjustments.
			//Attribute splits and neg adjustments to their procedures if they have been attached to some.
			//Attribute splits
			for(int i=0;i<listPaySplits.Count;i++) {
				for(int j=0;j<listComplProcs.Count;j++) {
					if(listPaySplits[i].ProcNum==listComplProcs[j].ProcNum) {//This is the proc the split is associated with
						if(listComplProcs[j].ProcFee-listPaySplits[i].SplitAmt<0) {//The split amount brings it below zero which isn't common
							listComplProcs[j].ProcFee=0;
							listPaySplits[i].SplitAmt=0;
						}
						else {
							listComplProcs[j].ProcFee-=listPaySplits[i].SplitAmt;
							listPaySplits[i].SplitAmt=0;
							break;
						}
					}
				}
			}
			//Attribute negative adjustments (Proc Discounts will be accounted for here, they're displayed in the Proc Discount field but there are also adjustments made)
			for(int i=0;i<listNegAdjustments.Count;i++) {
				for(int j=0;j<listComplProcs.Count;j++) {
					if(listNegAdjustments[i].ProcNum==listComplProcs[j].ProcNum) {//This is the proc the adjustment is associated with
						if(listComplProcs[j].ProcFee+listNegAdjustments[i].AdjAmt<0) {//Discounts are stored as negatives, so we want to add the negative to the fee
							listComplProcs[j].ProcFee=0;
							listNegAdjustments[i].AdjAmt=0;
						}
						else {
							listComplProcs[j].ProcFee+=listNegAdjustments[i].AdjAmt;
							listNegAdjustments[i].AdjAmt=0;
							break;
						}
					}
				}
			}
			//Attribute splits to their payplans.
			for(int i=0;i<listPaySplits.Count;i++) {
				for(int j=0;j<listPayPlanCharges.Count;j++) {
					if(listPaySplits[i].PayPlanNum==listPayPlanCharges[j].PayPlanNum && listPayPlanCharges[j].Principal>0) {
						if((listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest)-listPaySplits[i].SplitAmt<0) {//Use part of split as it covers more than one payplancharge
							listPaySplits[i].SplitAmt-=(listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest);
							listPayPlanCharges[j].Principal=0;
							listPayPlanCharges[j].Interest=0;
						}
						else {//Use all of split. Eliminate the charge's interest first, then the principal. 
							if(listPayPlanCharges[j].Interest-listPaySplits[i].SplitAmt<0) {//Use part of split on interest
								listPaySplits[i].SplitAmt-=listPayPlanCharges[j].Interest;
								listPayPlanCharges[j].Interest=0;
							}
							else {//Use all of split on interest
								listPayPlanCharges[j].Interest-=listPaySplits[i].SplitAmt;
								listPaySplits[i].SplitAmt=0;
								break;//If split is 0, we don't need to continue.
							}
							if(listPayPlanCharges[j].Principal-listPaySplits[i].SplitAmt<0) {//Use part of split on principal (Most common scenario)
								listPaySplits[i].SplitAmt-=listPayPlanCharges[j].Principal;
								listPayPlanCharges[j].Principal=0;
							}
							else {//Use all of split on principal (Only happens if the split exactly pays off the charge)
								listPayPlanCharges[j].Principal-=listPaySplits[i].SplitAmt;
								listPaySplits[i].SplitAmt=0;
							}
							break;
						}
					}
				}
			}
			//Now we have all splits and neg adjustments attributed to their procedures/payplan (If they were attributed).  We may have a whole host of procs with 0 balance (or possibly negative)
			//as well as payplan charges that have yet to be paid off (excluding those in the future).
			//Pay everything off FIFO by date, across object boundaries by making a datatable containing columns date, type, pri key, and owed amount.  Then go through the dataset and pay stuff off.
			//Then what we have left is everything that has nothing left to pay it off, so make splits for our current payment based on those ordered by date.
			_tableCharges.Columns.Add("Date");
			_tableCharges.Columns.Add("PriKey");
			_tableCharges.Columns.Add("Type");
			_tableCharges.Columns.Add("Amount");
			_tableCharges.Columns.Add("Prov");
			_tableCharges.Columns.Add("Clinic");
			_tableCharges.Columns.Add("Patient");
			_tableCharges.Columns.Add("OrigAmt");
			DataRow row;
			List<DataRow> listDataRows=new List<DataRow>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
				row=_tableCharges.NewRow();
				row["Date"]=listPayPlanCharges[i].ChargeDate.ToShortDateString();
				row["PriKey"]=listPayPlanCharges[i].PayPlanChargeNum;
				row["Type"]="PayPlanCharge";
				row["Amount"]=listPayPlanCharges[i].Principal+listPayPlanCharges[i].Interest;
				row["Prov"]=Providers.GetAbbr(listPayPlanCharges[i].ProvNum);
				row["Clinic"]=Clinics.GetDesc(listPayPlanCharges[i].ClinicNum);
				row["Patient"]=Patients.GetPat(listPayPlanCharges[i].PatNum).GetNameFL();
				row["OrigAmt"]=listPayPlanCharges[i].Principal+listPayPlanCharges[i].Interest;
				listDataRows.Add(row);
			}
			for(int i=0;i<listPosAdjustments.Count;i++) {
				row=_tableCharges.NewRow();
				row["Date"]=listPosAdjustments[i].AdjDate.ToShortDateString();
				row["PriKey"]=listPosAdjustments[i].AdjNum;
				row["Type"]="Adjustment";
				row["Amount"]=listPosAdjustments[i].AdjAmt;
				row["Prov"]=Providers.GetAbbr(listPosAdjustments[i].ProvNum);
				row["Clinic"]=Clinics.GetDesc(listPosAdjustments[i].ClinicNum);
				row["Patient"]=Patients.GetPat(listPosAdjustments[i].PatNum).GetNameFL();
				row["OrigAmt"]=listPosAdjustments[i].AdjAmt;
				listDataRows.Add(row);
			}
			for(int i=0;i<listComplProcs.Count;i++) {
				row=_tableCharges.NewRow();
				row["Date"]=listComplProcs[i].ProcDate.ToShortDateString();
				row["PriKey"]=listComplProcs[i].ProcNum;
				row["Type"]="Proc";
				row["Amount"]=listComplProcs[i].ProcFee;
				row["Prov"]=Providers.GetAbbr(listComplProcs[i].ProvNum);
				row["Clinic"]=Clinics.GetDesc(listComplProcs[i].ClinicNum);
				row["Patient"]=Patients.GetPat(listComplProcs[i].PatNum).GetNameFL();
				row["OrigAmt"]=listComplProcs[i].ProcFee;
				listDataRows.Add(row);
			}
			listDataRows.Sort(RowComparer);
			for(int i=0;i<listDataRows.Count;i++) {
				_tableCharges.Rows.Add(listDataRows[i]);
			}
			//Getting a date-sorted table of all credits that haven't been attributed to anything.
			DataTable tableCredits=new DataTable();
			tableCredits.Columns.Add("Date");
			tableCredits.Columns.Add("PriKey");
			tableCredits.Columns.Add("Type");
			tableCredits.Columns.Add("Amount");
			DataRow rowCredit;
			List<DataRow> listCreditDataRows=new List<DataRow>();
			for(int i=0;i<listNegAdjustments.Count;i++) {
				if(listNegAdjustments[i].AdjAmt<0) {//There's some remaining
					rowCredit=tableCredits.NewRow();
					rowCredit["Date"]=listNegAdjustments[i].AdjDate.ToShortDateString();
					rowCredit["PriKey"]=listNegAdjustments[i].AdjNum;
					rowCredit["Type"]="NegAdjust";
					rowCredit["Amount"]=0-listNegAdjustments[i].AdjAmt;//Turning it positive so we can treat all credits the same.
					listCreditDataRows.Add(rowCredit);
				}
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				if(listPaySplits[i].SplitAmt>0) {
					rowCredit=tableCredits.NewRow();
					rowCredit["Date"]=listPaySplits[i].DatePay.ToShortDateString();
					rowCredit["PriKey"]=listPaySplits[i].SplitNum;
					rowCredit["Type"]="PaySplit";
					rowCredit["Amount"]=listPaySplits[i].SplitAmt;
					listCreditDataRows.Add(rowCredit);
				}
			}
			for(int i=0;i<listPayments.Count;i++) {
				if(listPayments[i].PayAmt>0) {
					rowCredit=tableCredits.NewRow();
					rowCredit["Date"]=listPayments[i].PayDate.ToShortDateString();
					rowCredit["PriKey"]=listPayments[i].PayNum;
					rowCredit["Type"]="Payment";
					rowCredit["Amount"]=listPayments[i].PayAmt;
					listCreditDataRows.Add(rowCredit);
				}
			}
			listCreditDataRows.Sort(RowComparer);
			for(int i=0;i<listCreditDataRows.Count;i++) {
				tableCredits.Rows.Add(listCreditDataRows[i]);
			}
			//Now we have a date-sorted dataset of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_tableCharges.Rows.Count;i++) {
				double owedAmt=PIn.Double(_tableCharges.Rows[i]["Amount"].ToString());
				if(owedAmt>0) {
					for(int j=0;j<tableCredits.Rows.Count;j++) {
						double creditAmt=PIn.Double(tableCredits.Rows[j]["Amount"].ToString());
						if(creditAmt>0) {//May have been set to 0 for a previous owed item.
							if(owedAmt-creditAmt<0) {//This credit item can pay the owed item in full. Use a partial amount.
								creditAmt-=owedAmt;
								tableCredits.Rows[j]["Amount"]=creditAmt;//Change the credit item to reflect the "payment".
								_tableCharges.Rows[i]["Amount"]=0;
								break;
							}
							else {//This credit item cannot pay the owed item in full. Use all of credit, continue on to the next credit item.
								owedAmt-=creditAmt;
								creditAmt=0;
								tableCredits.Rows[j]["Amount"]=0;
								_tableCharges.Rows[i]["Amount"]=owedAmt;
							}
						}
					}
				}
			}
			//At this point we have a table of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			List<PaySplit> listAutoSplits=new List<PaySplit>();
			for(int i=0;i<_tableCharges.Rows.Count;i++) {
				double amtPaidForProc=0;
				double amtOwed=PIn.Double(_tableCharges.Rows[i]["Amount"].ToString());
				if(amtOwed>0 && payAmt>0) {//Still more to pay off on this charge and we have money left in the payment entered.
					if(amtOwed-payAmt<0) {//Use partial payment
						payAmt-=amtOwed;
						amtPaidForProc=amtOwed;
						_tableCharges.Rows[i]["Amount"]="0";
					}
					else {//Use full payment
						amtPaidForProc=payAmt;
						amtOwed-=payAmt;
						payAmt=0;
						_tableCharges.Rows[i]["Amount"]=amtOwed;
					}
				}
				if(amtPaidForProc>0) {//Only greater than zero if some of the proc was actually paid by this payment
					PaySplit split=new PaySplit();
					split.SplitAmt=amtPaidForProc;
					split.DatePay=date;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
						split.ClinicNum=clinic;
					}
					if(_tableCharges.Rows[i]["Type"].ToString()=="Proc") {
						Procedure proc=Procedures.GetOneProc(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()),false);
						split.PatNum=proc.PatNum;
						split.ProcDate=proc.ProcDate;
						split.ProcNum=proc.ProcNum;
						split.ProvNum=proc.ProvNum;
					}
					else if(_tableCharges.Rows[i]["Type"].ToString()=="PayPlanCharge") {
						PayPlanCharge charge=PayPlanCharges.GetOne(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()));
						split.PatNum=charge.PatNum;
						split.ProcDate=charge.ChargeDate;
						split.ProvNum=charge.ProvNum;
						split.PayPlanNum=charge.PayPlanNum;
					}
					else {
						Adjustment adjustment=Adjustments.GetOne(PIn.Long(_tableCharges.Rows[i]["PriKey"].ToString()));
						split.PatNum=adjustment.PatNum;
						split.ProcDate=adjustment.AdjDate;
						split.ProvNum=adjustment.ProvNum;
					}
					split.PayNum=payNum;
					listAutoSplits.Add(split);
				}
			}
			return listAutoSplits;
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.</summary>
		private void CreateSplit(ODGridRow row,ref double payAmt) {
			double chargeAmt=PIn.Double(row.Cells[2].Text);
			if(chargeAmt<=0 || payAmt<=0) {//They selected a paid row or the amount they want to pay with is empty, skip it.
				return;
			}
			if(payAmt>_payAvailableCur) {//They tried to enter in a partial payment for more than was remaining on the payment. Reduce it down.
				payAmt=_payAvailableCur;
			}
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			if(row.Tag.GetType()==typeof(Procedure)) {
				Procedure proc=(Procedure)row.Tag;
				for(int j=0;j<_tableCharges.Rows.Count;j++) {
					if(PIn.Long(_tableCharges.Rows[j]["PriKey"].ToString())==proc.ProcNum && _tableCharges.Rows[j]["Type"].ToString()=="Proc") {
						double amtOwed=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString());
						if(amtOwed-payAmt<0) {//Partial payment
							payAmt-=amtOwed;
							_tableCharges.Rows[j]["Amount"]=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_tableCharges.Rows[j]["Amount"]=amtOwed-payAmt;
							split.SplitAmt=payAmt;
							payAmt=0;
						}
						break;
					}
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					split.ClinicNum=proc.ClinicNum;
				}
				split.PatNum=proc.PatNum;
				split.ProcDate=proc.ProcDate;
				split.ProcNum=proc.ProcNum;
				split.ProvNum=proc.ProvNum;
			}
			else if(row.Tag.GetType()==typeof(PayPlanCharge)) {
				PayPlanCharge charge=(PayPlanCharge)row.Tag;
				for(int j=0;j<_tableCharges.Rows.Count;j++) {
					if(PIn.Long(_tableCharges.Rows[j]["PriKey"].ToString())==charge.PayPlanChargeNum && _tableCharges.Rows[j]["Type"].ToString()=="PayPlanCharge") {
						double amtOwed=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString());
						if(amtOwed-payAmt<0) {//Partial payment
							payAmt-=amtOwed;
							_tableCharges.Rows[j]["Amount"]=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_tableCharges.Rows[j]["Amount"]=amtOwed-payAmt;
							split.SplitAmt=payAmt;
							payAmt=0;
						}
						break;
					}
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					split.ClinicNum=charge.ClinicNum;
				}
				split.PatNum=charge.PatNum;
				split.ProcDate=charge.ChargeDate;
				split.ProvNum=charge.ProvNum;
				split.PayPlanNum=charge.PayPlanNum;
			}
			else if(row.Tag.GetType()==typeof(Adjustment)) {
				Adjustment adjust=(Adjustment)row.Tag;
				for(int j=0;j<_tableCharges.Rows.Count;j++) {
					if(PIn.Long(_tableCharges.Rows[j]["PriKey"].ToString())==adjust.AdjNum && _tableCharges.Rows[j]["Type"].ToString()=="Adjustment") {
						double amtOwed=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString());
						if(amtOwed-payAmt<0) {//Partial payment
							payAmt-=amtOwed;
							_tableCharges.Rows[j]["Amount"]=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_tableCharges.Rows[j]["Amount"]=amtOwed-payAmt;
							split.SplitAmt=payAmt;
							payAmt=0;
						}
						break;
					}
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					split.ClinicNum=adjust.ClinicNum;
				}
				split.PatNum=adjust.PatNum;
				split.ProcDate=adjust.AdjDate;
				split.ProvNum=adjust.ProvNum;
			}
			split.PayNum=PaymentCur.PayNum;
			_listSplitsCur.Add(split);
			textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+split.SplitAmt).ToString("f");
			_payAvailableCur-=split.SplitAmt;
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.</summary>
		private void DeleteSelected() {
			if(gridSplits.SelectedIndices.Length==0) {
				return;
			}
			for(int i=gridSplits.SelectedIndices.Length-1;i>=0;i--) {
				PaySplit split=_listSplitsCur[gridSplits.SelectedIndices[i]];
				_payAvailableCur+=split.SplitAmt;
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)-split.SplitAmt).ToString("f");
				string splitType="";
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					splitType=gridSplits.Rows[gridSplits.SelectedIndices[i]].Cells[4].Text;
				}
				else {
					splitType=gridSplits.Rows[gridSplits.SelectedIndices[i]].Cells[3].Text;
				}
				if(splitType=="PayPlanCharge") {//PayPlanCharge
					//Get charges for the split's payplan.
					List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(split.PayPlanNum);
					List<long> listPayPlanChargeNums=new List<long>();
					for(int j=0;j<listCharges.Count;j++) {
						listPayPlanChargeNums.Add(listCharges[j].PayPlanChargeNum);
					}
					//Then look for the matching row in _tableCharges.  The best we can do is look at the charge primary key (it has to be long to the payplan, split isn't more specific than that),
					//then the charge date.  There may be overlap in charges that were done on the same day (for example, downpayments) but until charges get attached to splits we have no way to know.
					for(int j=0;j<_tableCharges.Rows.Count;j++) {
						if(_tableCharges.Rows[j]["Type"].ToString()=="PayPlanCharge"
							&& listPayPlanChargeNums.Contains(PIn.Long(_tableCharges.Rows[j]["PriKey"].ToString()))//If we find the PriKey in the list of chargenums then we have the right payplan
							&& PIn.DateT(_tableCharges.Rows[j]["Date"].ToString())==split.ProcDate
							&& Providers.GetAbbr(split.ProvNum)==_tableCharges.Rows[j]["Prov"].ToString()) 						
						{//Only way to tell if it's the right charge is looking for matching payplan and date.
							_tableCharges.Rows[j]["Amount"]=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString())+split.SplitAmt;//Give the money back to the charge so it will display.
							break;
						}
					}
				}
				else if(splitType=="Adjustment") {//Adjustment
					//Find adjustment that has the AdjDate and PatNum for this split.ProcDate
					//Until adjustments get attached to paysplits we have no way of knowing what adjustment on a particular day to credit.  We just guess.
					for(int j=0;j<_tableCharges.Rows.Count;j++) {
						if(_tableCharges.Rows[j]["Type"].ToString()=="Adjustment" 
							&& split.ProcNum==0 && split.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& split.ProcDate==PIn.DateT(_tableCharges.Rows[j]["Date"].ToString())
							&& Providers.GetAbbr(split.ProvNum)==_tableCharges.Rows[j]["Prov"].ToString()) 
						{
							_tableCharges.Rows[j]["Amount"]=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString())+split.SplitAmt;//Give the money back to the charge so it will display
							break;
						}
					}
				}
				else {//Procedure
					//We can look for the type Procedure with prikey of split.ProcNum to re-fill this one.
					for(int j=0;j<_tableCharges.Rows.Count;j++) {
						if(_tableCharges.Rows[j]["Type"].ToString()=="Proc" && PIn.Long(_tableCharges.Rows[j]["PriKey"].ToString())==split.ProcNum) {
							_tableCharges.Rows[j]["Amount"]=PIn.Double(_tableCharges.Rows[j]["Amount"].ToString())+split.SplitAmt;//Give the money back to the charge so it will display.
						}
					}
				}
				_listSplitsCur.RemoveAt(gridSplits.SelectedIndices[i]);
			}
		}

		private void checkShowPaid_CheckedChanged(object sender,EventArgs e) {
			FillChargeGrid();
		}

		///<summary>Creates paysplits for selected charges if there is enough payment left.</summary>
		private void butCreateSplit_Click(object sender,EventArgs e) {
			//PayAmt needs to have an amount, only make splits for rows that have an outstanding balance.
			if(_payAvailableCur==0) {
				return;
			}
			double amt=_payAvailableCur;
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				CreateSplit(gridCharges.Rows[gridCharges.SelectedIndices[i]],ref amt);
			}
			FillChargeGrid();
		}

		///<summary>Creates paysplits after allowing the user to enter in a custom amount to pay for each selected charge.</summary>
		private void butCreatePartial_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				if(_payAvailableCur>0) {//Only make splits if we have some available.
					FormAmountEdit FormAE=new FormAmountEdit(gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[1].Text);
					FormAE.Amount=PIn.Double(gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[2].Text);
					FormAE.ShowDialog();
					if(FormAE.DialogResult==DialogResult.OK) {
						double amount=FormAE.Amount;
						if(amount>0) {
							CreateSplit(gridCharges.Rows[gridCharges.SelectedIndices[i]],ref amount);
						}
					}
				}
			}
			FillChargeGrid();
		}

		///<summary>Deletes all paysplits.</summary>
		private void butClearAll_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
			FillChargeGrid();
		}

		///<summary>Deletes selected paysplits.</summary>
		private void butDeleteSplit_Click(object sender,EventArgs e) {
			DeleteSelected();
			FillChargeGrid();
		}

		///<summary>Allows editing of the payment amount.</summary>
		private void butEdit_Click(object sender,EventArgs e) {
			FormAmountEdit FormAE=new FormAmountEdit("Payment Amount");
			FormAE.Amount=PaymentAmt;
			FormAE.ShowDialog();
			if(FormAE.DialogResult==DialogResult.OK) {
				double newAmount=FormAE.Amount;
				double diff=PaymentAmt-newAmount;
				if(_payAvailableCur-diff<0) {//They decreased the payment amount
					PaymentAmt-=_payAvailableCur;//Decrease the payment amount down so the amount remaining is 0
					textPayAmt.Text=PaymentAmt.ToString("f");
					_payAvailableCur=0;
				}
				else {//They increased the payment amount or decreased it enough so the AmtRemaining isn't negative
					PaymentAmt=newAmount;
					textPayAmt.Text=PaymentAmt.ToString("f");
					_payAvailableCur-=diff;
				}
			}
			textAmtAvailable.Text=_payAvailableCur.ToString("f");
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!IsNew) {//If old payment and they click OK, delete and re-insert the splits as they may have totally changed stuff around.
				PaySplits.DeleteForPayment(PaymentCur.PayNum);
			}
			for(int i=0;i<_listSplitsCur.Count;i++) {
				PaySplits.Insert(_listSplitsCur[i]);
			}
			PaymentCur.PayAmt=PIn.Double(textSplitAmt.Text);
			if(_listSplitsCur.Count>0) {
				PaymentCur.IsSplit=true;
			}
			try {
				Payments.Update(PaymentCur,true);
			}
			catch {
				//Invalid date entered from Payment window
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//No paysplits were inserted. Just close the window.
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		public static int RowComparer(DataRow x,DataRow y) {
			DateTime xDateTime=PIn.DateT(x["Date"].ToString());
			DateTime yDateTime=PIn.DateT(y["Date"].ToString());
			return xDateTime.CompareTo(yDateTime);
		}

	}
}