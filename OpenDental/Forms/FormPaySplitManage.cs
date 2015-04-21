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
		public List<PaySplit> ListSplitsCur;
		///<summary>Amount currently available for paying off charges.</summary>
		private double _payAvailableCur;
		///<summary>List of current account charges for the family.</summary>
		private List<AccountEntry> _listAccountCharges;
		///<summary>The amount entered for the current payment.  May be changed in this window.</summary>
		public double PaymentAmt;
		public Family FamCur;
		public Patient PatCur;
		public Payment PaymentCur;
		public DateTime PayDate;
		public bool IsNew;

		public FormPaySplitManage() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPaySplitManage_Load(object sender,EventArgs e) {
			//Our goal is to make sure amounts remain consistant as the user performs manual modifications.
			//On OK click submit all paysplits.
			//We also need to handle when they have existing paysplits on an old payment or existing paysplits on a new payment.
			_listAccountCharges=new List<AccountEntry>();
			_payAvailableCur=PaymentAmt;
			textPayAmt.Text=PaymentAmt.ToString("f");
			List<long> listPatNums=new List<long>();
			for(int i=0;i<FamCur.ListPats.Length;i++) {
				listPatNums.Add(FamCur.ListPats[i].PatNum);
			}
			if(IsNew) {
				ListSplitsCur=AutoSplitForPayment(listPatNums,PaymentCur.PayNum,ref _payAvailableCur,PayDate);//New payment, generated autosplits overwrites any manual/pre-existing splits.
				//For reference later: Assigning to ListSplitsCur instead of _payAvailableCur ensures that for old payments, splits aren't over-written, and no duplication takes place if it's a new payment with pre-existing splits since they're wiped out.
				textSplitAmt.Text=(PaymentAmt-_payAvailableCur).ToString("f");//Amount Paid Increases as PayAmt decreases.
			}
			else {//Not new, or new with at least one pre-existing paysplit.  
						//Don't overwrite, but if it's new with pre-existing then the auto-split logic won't detect it since it hasn't been saved in DB yet.
				textSplitAmt.Text=POut.Double(PaymentCur.PayAmt);
				double diff=_payAvailableCur-PaymentCur.PayAmt;//Old payment. They may have added funds to the payment for some reason, which will be reflected in PayAmt but not in PaymentCur.
				if(diff>0) {//If they increased the amount of the old payment, they want to be able to use it. 
					_payAvailableCur=diff;
				}
				else {//If they decreased the amount of the old payment, don't let them assign any new charges to this payment (but they can certainly take some off).
					_payAvailableCur=0;
				}
				//We want to fill the charge table like we usually do.
				//I originally thought we didn't want to make any new autosplits, but I think now that if they opened an old payment and increased the value of the payment they should get autosplits made.
				//If they didn't change the payment amount _payAvailableCur will be 0 so no new autosplits will be made anyways.
				//Problem:  If it's a new payment with existing paysplits the paysplits won't be counted in when autosplits are determined.
				AutoSplitForPayment(listPatNums,PaymentCur.PayNum,ref _payAvailableCur,PayDate);//With the splits already made on old payment their charges will be marked correctly.
				//For reference later: Assigning to ListSplitsCur instead of _payAvailableCur ensures that for old payments or new payments with existing splits they aren't over-written, and no duplication takes place.
			}
			FillChargeGrid();
		}

		///<summary>Fills charge grid, and then split grid.</summary>
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
			for(int i=0;i<_listAccountCharges.Count;i++) {
				if(_listAccountCharges[i].AmountCur==0 && !checkShowPaid.Checked) {//Filter out those that have been paid if checkShowPaid is unchecked.
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(_listAccountCharges[i].Date.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(_listAccountCharges[i].ProvNum));//Provider
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					row.Cells.Add(Clinics.GetDesc(_listAccountCharges[i].ClinicNum));
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(Patients.GetPat(_listAccountCharges[i].PatNum).GetNameFL());
				row.Cells.Add(_listAccountCharges[i].Type);//Type
				if(_listAccountCharges[i].Type=="Proc") {
					//Get the proc and add its description if the row is a proc.
					Procedure proc=Procedures.GetOneProc(_listAccountCharges[i].PriKey,false);
					string procDesc=Procedures.GetDescription(proc);
					row.Cells[row.Cells.Count-1].Text+=": "+procDesc;
					row.Tag=proc;//Used later when making splits for this charge
				}
				else if(_listAccountCharges[i].Type=="PayPlanCharge"){
					PayPlanCharge charge=PayPlanCharges.GetOne(_listAccountCharges[i].PriKey);
					row.Tag=charge;//Used later when making splits for this charge
				}
				else{
					Adjustment adjust=Adjustments.GetOne(_listAccountCharges[i].PriKey);
					row.Tag=adjust;//Used later when making splits for this charge
				}
				row.Cells.Add(_listAccountCharges[i].AmountCur.ToString("f"));//Amount
				if(_listAccountCharges[i].AmountCur==0) {
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
			for(int i=0;i<ListSplitsCur.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(ListSplitsCur[i].DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(ListSplitsCur[i].ProvNum));//Prov
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					if(ListSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetClinic(ListSplitsCur[i].ClinicNum).Description);//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				row.Cells.Add(Patients.GetPat(ListSplitsCur[i].PatNum).GetNameFL());//Patient
				if(ListSplitsCur[i].ProcNum==0 && ListSplitsCur[i].PayPlanNum!=0) {
					row.Cells.Add("PayPlanCharge");//Type
				}
				else if(ListSplitsCur[i].ProcNum!=0) {
					Procedure proc=Procedures.GetOneProc(ListSplitsCur[i].ProcNum,false);
					string procDesc=Procedures.GetDescription(proc);
					row.Cells.Add("Proc: "+procDesc);//Type
				}
				else {//Unattached split for a positive adjustment
					row.Cells.Add("Adjustment");//Type
				}
				row.Cells.Add(ListSplitsCur[i].SplitAmt.ToString("f"));//Amount
				row.Tag=ListSplitsCur[i];
				gridSplits.Rows.Add(row);
			}
			gridSplits.EndUpdate();
		}

		///<summary>Creates and paysplits associated to the patient passed in for the payment passed in until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  payAmt is passed as a ref so that we can do this same logic for an entire 
		///family while keeping track of the remaining pay amount.</summary>
		private List<PaySplit> AutoSplitForPayment(List<long> listPatNums,long payNum,ref double payAmt,DateTime date) {
			//Get the lists of items we'll be using to calculate with.
			//double payAmt=_payAvailableCur;
			List<Procedure> listProcs=Procedures.GetCompleteForPats(listPatNums);
			//listPayments should be empty, there isn't currently a way to make payments without at least one split.
			//During research however we found there were sometimes payments with no splits, so erred on the side of caution.
			List<Payment> listPayments=Payments.GetNonSplitForPats(listPatNums);
			List<Adjustment> listNegAdjustments=Adjustments.GetAdjustForPats(listPatNums,false);
			List<Adjustment> listPosAdjustments=Adjustments.GetAdjustForPats(listPatNums,true);
			List<PaySplit> listPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			List<ClaimProc> listInsPayAsTotal=ClaimProcs.GetByTotForPats(listPatNums);//Claimprocs paid as total, might contain ins payplan payments.
			List<PayPlan> listPayPlans=PayPlans.GetForPats(listPatNums,PatCur.PatNum);//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			double payPlanAmt=0;
			for(int i=0;i<listPayPlans.Count;i++) {
				if(listPatNums.Contains(listPayPlans[i].PatNum)) {//The payplan is for someone in the current family.  Excludes payment plan guarantors outside of the family.
					payPlanAmt+=listPayPlans[i].CompletedAmt;
				}
			}
			List<PayPlanCharge> listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0){
				listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans,PatCur.PatNum);//Does not get charges for the future.
			}
			for(int i=0;i<listProcs.Count;i++) {//Getting the actual amount patient is responsible for.
				//Calculated using writeoffs, inspayest, inspayamt.  Done the same way ContrAcct does it.				
				listProcs[i].ProcFee=ClaimProcs.GetPatPortion(listProcs[i]);
			}
			//Now we have all negative and positive adjustments.
			//In the next two regions, apply credits to their procedures if they have been attached.
			#region Splits
			for(int i=0;i<listPaySplits.Count;i++) {
				for(int j=0;j<listProcs.Count;j++) {
					if(listPaySplits[i].ProcNum!=listProcs[j].ProcNum) {
						continue;
					}
					//This is the proc the split is associated with.
					if(listProcs[j].ProcFee<listPaySplits[i].SplitAmt) {
						listPaySplits[i].SplitAmt-=listProcs[j].ProcFee;//They overpaid on the split
						listProcs[j].ProcFee=0;
					}
					else {//ProcFee>=SplitAmt
						listProcs[j].ProcFee-=listPaySplits[i].SplitAmt;
						listPaySplits[i].SplitAmt=0;
						break;
					}
				}
			}
			#endregion Splits
			#region Negative Adjustments
			//Attribute negative adjustments (Proc Discounts will be accounted for here, they're displayed in the Proc Discount field but there are also adjustments made)
			for(int i=0;i<listNegAdjustments.Count;i++) {
				for(int j=0;j<listProcs.Count;j++) {
					if(listNegAdjustments[i].ProcNum!=listProcs[j].ProcNum) {
						continue;
					}
					double creditAmt=-listNegAdjustments[i].AdjAmt;//AdjAmt is negative.
					//This is the proc the adjustment is associated with.
					if(listProcs[j].ProcFee<creditAmt) {
						listNegAdjustments[i].AdjAmt+=listProcs[j].ProcFee;//Reduce the credit by the fee.
						listProcs[j].ProcFee=0;
					}
					else {//ProcFee>=creditAmt
						listProcs[j].ProcFee-=creditAmt;
						listNegAdjustments[i].AdjAmt=0;
						break;
					}
				}
			}
			#endregion Negative Adjustments
			#region Paysplits
			//Attribute splits to their payplans.
			for(int i=0;i<listPaySplits.Count;i++) {
				for(int j=0;j<listPayPlanCharges.Count;j++) {
					if(listPaySplits[i].PayPlanNum!=listPayPlanCharges[j].PayPlanNum || listPayPlanCharges[j].Principal<=0) {
						continue;
					}
					if((listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest)<=listPaySplits[i].SplitAmt) {//Used part of split as it potentially covers more than one payplancharge
						listPaySplits[i].SplitAmt-=(listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest);
						listPayPlanCharges[j].Principal=0;
						listPayPlanCharges[j].Interest=0;
					}
					else {//Principal+Interest>SplitAmt.  Use all of split.  Eliminate the charge's interest first, then the principal. 
						if(listPayPlanCharges[j].Interest<listPaySplits[i].SplitAmt) {//Use part of split on interest.
							listPaySplits[i].SplitAmt-=listPayPlanCharges[j].Interest;
							listPayPlanCharges[j].Interest=0;
						}
						else {//Interest>=SplitAmt.  Use all of split on interest.
							listPayPlanCharges[j].Interest-=listPaySplits[i].SplitAmt;
							listPaySplits[i].SplitAmt=0;
							break;//If split is 0, we don't need to continue.
						}
						//(Principal+Interest)>SplitAmt => Principal>SplitAmt-Interest.  Since now SplitAmt=SplitAmt-Interest, really we have Principal>SplitAmt.
						//Use all of split on principal (Only happens if the split exactly pays off the charge).
						listPayPlanCharges[j].Principal-=listPaySplits[i].SplitAmt;
						listPaySplits[i].SplitAmt=0;
						break;
					}
				}
			}
			#endregion Paysplits
			#region As Total Ins Payments
			//Attribute "as total" insurance payments to their potential ins payplans.
			for(int i=0;i<listInsPayAsTotal.Count;i++) {
				for(int j=0;j<listPayPlanCharges.Count;j++) {
					if(listInsPayAsTotal[i].PayPlanNum!=listPayPlanCharges[j].PayPlanNum || listPayPlanCharges[j].Principal<=0) {
						continue;
					}
					if((listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest)<=listInsPayAsTotal[i].InsPayAmt) {//Use part of split as it potentially covers more than one payplancharge
						listInsPayAsTotal[i].InsPayAmt-=(listPayPlanCharges[j].Principal+listPayPlanCharges[j].Interest);
						listPayPlanCharges[j].Principal=0;
						listPayPlanCharges[j].Interest=0;
					}
					else {//Principal+Interest>InsPayAmt.  Use all of split.  Eliminate the charge's interest first, then the principal. 
						if(listPayPlanCharges[j].Interest<listInsPayAsTotal[i].InsPayAmt) {//Use part of split on interest
							listInsPayAsTotal[i].InsPayAmt-=listPayPlanCharges[j].Interest;
							listPayPlanCharges[j].Interest=0;
						}
						else {//Interest>=InsPayAmt.  Use all of split on interest.
							listPayPlanCharges[j].Interest-=listInsPayAsTotal[i].InsPayAmt;
							listInsPayAsTotal[i].InsPayAmt=0;
							break;//If split is 0, we don't need to continue.
						}
						//(Principal+Interest)>InsPayAmt => Principal>InsPayAmt-Interest.  Since now InsPayAmt=InsPayAmt-Interest, really we have Principal>InsPayAmt.
						//Apply the rest that's left over on the principal.
						listPayPlanCharges[j].Principal-=listInsPayAsTotal[i].InsPayAmt;
						listInsPayAsTotal[i].InsPayAmt=0;
						break;
					}
				}
			}
			#endregion As Total Ins Payments
			//Over the next 3 regions, we will do the following:
			//Now we have all splits and neg adjustments attributed to their procedures/payplan (If they were attributed).  We may have a whole host of 
			//procs with 0 balance (or possibly negative) as well as payplan charges that have yet to be paid off (excluding those in the future).  Pay
			//everything off FIFO by date, across object boundaries by making a list containing AccountEntry objects.  Then, go through the list and apply
			//payments.  What remains is everything which is unpaid, so make splits for our current payment based on those.  Order by date.
			#region List of Charges
			_listAccountCharges=new List<AccountEntry>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
				AccountEntry accountEntry=new AccountEntry();
				accountEntry.Date=listPayPlanCharges[i].ChargeDate;
				accountEntry.PriKey=listPayPlanCharges[i].PayPlanChargeNum;
				accountEntry.Type="PayPlanCharge";
				accountEntry.AmountCur=listPayPlanCharges[i].Principal+listPayPlanCharges[i].Interest;
				accountEntry.AmountOriginal=listPayPlanCharges[i].Principal+listPayPlanCharges[i].Interest;
				accountEntry.ProvNum=listPayPlanCharges[i].ProvNum;
				accountEntry.ClinicNum=listPayPlanCharges[i].ClinicNum;
				accountEntry.PatNum=listPayPlanCharges[i].PatNum;
				_listAccountCharges.Add(accountEntry);
			}
			for(int i=0;i<listPosAdjustments.Count;i++) {
				AccountEntry accountEntry=new AccountEntry();
				accountEntry.Date=listPosAdjustments[i].AdjDate;
				accountEntry.PriKey=listPosAdjustments[i].AdjNum;
				accountEntry.Type="Adjustment";
				accountEntry.AmountCur=listPosAdjustments[i].AdjAmt;
				accountEntry.AmountOriginal=listPosAdjustments[i].AdjAmt;
				accountEntry.ProvNum=listPosAdjustments[i].ProvNum;
				accountEntry.ClinicNum=listPosAdjustments[i].ClinicNum;
				accountEntry.PatNum=listPosAdjustments[i].PatNum;
				_listAccountCharges.Add(accountEntry);
			}
			for(int i=0;i<listProcs.Count;i++) {
				AccountEntry accountEntry=new AccountEntry();
				accountEntry.Date=listProcs[i].ProcDate;
				accountEntry.PriKey=listProcs[i].ProcNum;
				accountEntry.Type="Proc";
				accountEntry.AmountCur=listProcs[i].ProcFee;
				accountEntry.AmountOriginal=listProcs[i].ProcFee;
				accountEntry.ProvNum=listProcs[i].ProvNum;
				accountEntry.ClinicNum=listProcs[i].ClinicNum;
				accountEntry.PatNum=listProcs[i].PatNum;
				_listAccountCharges.Add(accountEntry);
			}
			_listAccountCharges.Sort(RowComparer);
			#endregion List of Charges
			#region List of Credits
			//Getting a date-sorted table of all credits that haven't been attributed to anything.
			List<AccountEntry> listAccountCredits=new List<AccountEntry>();
			for(int i=0;i<listNegAdjustments.Count;i++) {
				if(listNegAdjustments[i].AdjAmt<0) {//There's some remaining
					AccountEntry accountEntry=new AccountEntry();
					accountEntry.Date=listNegAdjustments[i].AdjDate;
					accountEntry.PriKey=listNegAdjustments[i].AdjNum;
					accountEntry.Type="NegAdjust";
					accountEntry.AmountCur=0-listNegAdjustments[i].AdjAmt;//Turning it positive so we can treat all credits the same.
					accountEntry.AmountOriginal=0-listNegAdjustments[i].AdjAmt;
					accountEntry.ProvNum=listNegAdjustments[i].ProvNum;
					accountEntry.ClinicNum=listNegAdjustments[i].ClinicNum;
					accountEntry.PatNum=listNegAdjustments[i].PatNum;
					listAccountCredits.Add(accountEntry);
				}
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				if(listPaySplits[i].SplitAmt>0) {
					AccountEntry accountEntry=new AccountEntry();
					accountEntry.Date=listPaySplits[i].DatePay;
					accountEntry.PriKey=listPaySplits[i].SplitNum;
					accountEntry.Type="PaySplit";
					accountEntry.AmountCur=listPaySplits[i].SplitAmt;
					accountEntry.AmountOriginal=listPaySplits[i].SplitAmt;
					accountEntry.ProvNum=listPaySplits[i].ProvNum;
					accountEntry.ClinicNum=listPaySplits[i].ClinicNum;
					accountEntry.PatNum=listPaySplits[i].PatNum;
					listAccountCredits.Add(accountEntry);
				}
			}
			for(int i=0;i<listPayments.Count;i++) {
				if(listPayments[i].PayAmt>0) {
					AccountEntry accountEntry=new AccountEntry();
					accountEntry.Date=listPayments[i].PayDate;
					accountEntry.PriKey=listPayments[i].PayNum;
					accountEntry.Type="Payment";
					accountEntry.AmountCur=listPayments[i].PayAmt;
					accountEntry.AmountOriginal=listPayments[i].PayAmt;
					accountEntry.ProvNum=0;//Payments don't have a ProvNum
					accountEntry.ClinicNum=listPayments[i].ClinicNum;
					accountEntry.PatNum=listPayments[i].PatNum;
					listAccountCredits.Add(accountEntry);
				}
			}
			for(int i=0;i<listInsPayAsTotal.Count;i++) {
				if(listInsPayAsTotal[i].InsPayAmt>0) {
					AccountEntry accountEntry=new AccountEntry();
					accountEntry.Date=listInsPayAsTotal[i].DateCP;
					accountEntry.PriKey=listInsPayAsTotal[i].ClaimProcNum;
					accountEntry.Type="ClaimProc";
					accountEntry.AmountCur=listInsPayAsTotal[i].InsPayAmt;
					accountEntry.AmountOriginal=listInsPayAsTotal[i].InsPayAmt;
					accountEntry.ProvNum=listInsPayAsTotal[i].ProvNum;
					accountEntry.ClinicNum=listInsPayAsTotal[i].ClinicNum;
					accountEntry.PatNum=listInsPayAsTotal[i].PatNum;
					listAccountCredits.Add(accountEntry);
				}
			}
			listAccountCredits.Sort(RowComparer);
			#endregion List of Credits
			#region Implicit Linking
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				if(_listAccountCharges[i].AmountCur<=0) {
					continue;
				}
				double owedAmt=_listAccountCharges[i].AmountCur;
				if(payPlanAmt>0 && owedAmt<payPlanAmt && _listAccountCharges[i].Type!="PayPlanCharge") {//Use the payplan to pay the full charge, as long as it's not a payplancharge (Is this what we want? Most of me says yes, especially when we take into account what Providers get credit.)
					payPlanAmt-=owedAmt;//Still more left in the payplan amount, keep using it.
					_listAccountCharges[i].AmountCur=0;
					continue;
				}
				else if(payPlanAmt>0 && _listAccountCharges[i].Type!="PayPlanCharge") {//Using up the last of the payPlanAmt, time to use the credits
					owedAmt-=payPlanAmt;
					payPlanAmt=0;
				}
				for(int j=0;j<listAccountCredits.Count;j++) {
					if(listAccountCredits[j].AmountCur<=0) {//May have been set to 0 for a previous owed item.
						continue;
					}
					double creditAmt=listAccountCredits[j].AmountCur;
					if(owedAmt<creditAmt) {//This credit item can pay the owed item in full. Use a partial amount.
						creditAmt-=owedAmt;
						listAccountCredits[j].AmountCur=creditAmt;//Change the credit item to reflect the "payment".
						_listAccountCharges[i].AmountCur=0;
						break;
					}
					else {//This credit item cannot pay the owed item in full. Use all of credit, continue on to the next credit item.
						owedAmt-=creditAmt;
						creditAmt=0;
						listAccountCredits[j].AmountCur=0;
					}
				}
				_listAccountCharges[i].AmountCur=owedAmt;
			}
			#endregion Implicit Linking
			#region Auto-split Current Payment
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			List<PaySplit> listAutoSplits=new List<PaySplit>();
			for(int i=0;i<_listAccountCharges.Count;i++) {
				if(payAmt<=0 || _listAccountCharges[i].AmountCur<=0) {
					continue;
				}
				double amtOwed=_listAccountCharges[i].AmountCur;
				double amtPaidForProc=0;
				if(amtOwed<payAmt) {//Use partial payment
					payAmt-=amtOwed;
					amtPaidForProc=amtOwed;
					_listAccountCharges[i].AmountCur=0;
				}
				else {//Use full payment
					amtPaidForProc=payAmt;
					amtOwed-=payAmt;
					payAmt=0;
					_listAccountCharges[i].AmountCur=amtOwed;
				}
				PaySplit split=new PaySplit();
				split.SplitAmt=amtPaidForProc;
				split.DatePay=date;
				if(_listAccountCharges[i].Type=="Proc") {
					Procedure proc=Procedures.GetOneProc(_listAccountCharges[i].PriKey,false);
					split.PatNum=proc.PatNum;
					split.ProcDate=proc.ProcDate;
					split.ProcNum=proc.ProcNum;
					split.ProvNum=proc.ProvNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=proc.ClinicNum;
					}
				}
				else if(_listAccountCharges[i].Type=="PayPlanCharge") {
					PayPlanCharge charge=PayPlanCharges.GetOne(_listAccountCharges[i].PriKey);
					split.PatNum=charge.PatNum;
					split.ProcDate=charge.ChargeDate;
					split.ProvNum=charge.ProvNum;
					split.PayPlanNum=charge.PayPlanNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=charge.ClinicNum;
					}
				}
				else {
					Adjustment adjustment=Adjustments.GetOne(_listAccountCharges[i].PriKey);
					split.PatNum=adjustment.PatNum;
					split.ProcDate=adjustment.AdjDate;
					split.ProvNum=adjustment.ProvNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=adjustment.ClinicNum;
					}
				}
				split.PayNum=payNum;
				listAutoSplits.Add(split);
			}
			#endregion Auto-Spit Current Payment
			return listAutoSplits;
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.</summary>
		private void CreateSplit(ODGridRow row,ref double payAmt,bool strict) {
			double chargeAmt=PIn.Double(row.Cells[5].Text);
			double payment=payAmt;
			if((chargeAmt<=0 || payment<=0) && strict) {//They selected a paid row or the amount they want to pay with is empty, skip it.
				return;
			}
			if(payment>_payAvailableCur) {//They tried to enter in a partial payment for more than was remaining on the payment. Reduce it down.
				payment=_payAvailableCur;
			}
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			if(row.Tag.GetType()==typeof(Procedure)) {
				Procedure proc=(Procedure)row.Tag;
				for(int j=0;j<_listAccountCharges.Count;j++) {
					if(_listAccountCharges[j].PriKey==proc.ProcNum && _listAccountCharges[j].Type=="Proc") {
						double amtOwed=_listAccountCharges[j].AmountCur;
						if(amtOwed-payment<0) {//Partial payment
							payment-=amtOwed;
							_listAccountCharges[j].AmountCur=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_listAccountCharges[j].AmountCur=amtOwed-payment;
							split.SplitAmt=payment;
							payment=0;
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
				for(int j=0;j<_listAccountCharges.Count;j++) {
					if(_listAccountCharges[j].PriKey==charge.PayPlanChargeNum && _listAccountCharges[j].Type=="PayPlanCharge") {
						double amtOwed=_listAccountCharges[j].AmountCur;
						if(amtOwed-payment<0) {//Partial payment
							payment-=amtOwed;
							_listAccountCharges[j].AmountCur=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_listAccountCharges[j].AmountCur=amtOwed-payment;
							split.SplitAmt=payment;
							payment=0;
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
				for(int j=0;j<_listAccountCharges.Count;j++) {
					if(_listAccountCharges[j].PriKey==adjust.AdjNum && _listAccountCharges[j].Type=="Adjustment") {
						double amtOwed=_listAccountCharges[j].AmountCur;
						if(amtOwed-payment<0) {//Partial payment
							payment-=amtOwed;
							_listAccountCharges[j].AmountCur=0;//Reflect payment in underlying datastructure
							split.SplitAmt=amtOwed;
						}
						else {//Full payment
							_listAccountCharges[j].AmountCur=amtOwed-payment;
							split.SplitAmt=payment;
							payment=0;
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
			ListSplitsCur.Add(split);
			if(strict) {
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+split.SplitAmt).ToString("f");
				_payAvailableCur-=split.SplitAmt;
				payAmt=payment;
			}
			else {
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+payment).ToString("f");
				_payAvailableCur-=payment;
			}
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.</summary>
		private void DeleteSelected() {
			if(gridSplits.SelectedIndices.Length==0) {
				return;
			}
			for(int i=gridSplits.SelectedIndices.Length-1;i>=0;i--) {
				PaySplit split=ListSplitsCur[gridSplits.SelectedIndices[i]];
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
					for(int j=0;j<_listAccountCharges.Count;j++) {
						if(_listAccountCharges[j].Type=="PayPlanCharge"
							&& listPayPlanChargeNums.Contains(_listAccountCharges[j].PriKey)//If we find the PriKey in the list of chargenums then we have the right payplan
							&& _listAccountCharges[j].Date==split.ProcDate
							&& split.ProvNum==_listAccountCharges[j].ProvNum) 						
						{//Only way to tell if it's the right charge is looking for matching payplan and date.
							double chargeAmtNew=_listAccountCharges[j].AmountCur+split.SplitAmt;
							if(chargeAmtNew>_listAccountCharges[j].AmountOriginal) {//Trying to refund an overpayment, just increase charge's amount to the max.
								_listAccountCharges[j].AmountCur=_listAccountCharges[j].AmountOriginal;
							}
							else {
								_listAccountCharges[j].AmountCur+=split.SplitAmt;//Give the money back to the charge so it will display.
							}
							break;
						}
					}
				}
				else if(splitType=="Adjustment") {//Adjustment
					//Find adjustment that has the AdjDate and PatNum for this split.ProcDate
					//Until adjustments get attached to paysplits we have no way of knowing what adjustment on a particular day to credit.  We just guess.
					for(int j=0;j<_listAccountCharges.Count;j++) {
						if(_listAccountCharges[j].Type=="Adjustment" 
							&& split.ProcNum==0 && split.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& split.ProcDate==_listAccountCharges[j].Date
							&& split.ProvNum==_listAccountCharges[j].ProvNum) 
						{
							double chargeAmtNew=_listAccountCharges[j].AmountCur+split.SplitAmt;
							if(chargeAmtNew>_listAccountCharges[j].AmountOriginal) {//Trying to refund an overpayment, just increase charge's amount to the max.
								_listAccountCharges[j].AmountCur=_listAccountCharges[j].AmountOriginal;
							}
							else {
								_listAccountCharges[j].AmountCur+=split.SplitAmt;//Give the money back to the charge so it will display.
							}
							break;
						}
					}
				}
				else {//Procedure
					//We can look for the type Procedure with prikey of split.ProcNum to re-fill this one.
					for(int j=0;j<_listAccountCharges.Count;j++) {
						if(_listAccountCharges[j].Type=="Proc" && _listAccountCharges[j].PriKey==split.ProcNum) {
							double chargeAmtNew=_listAccountCharges[j].AmountCur+split.SplitAmt;
							if(chargeAmtNew>_listAccountCharges[j].AmountOriginal) {//Trying to refund an overpayment, just increase charge's amount to the max.
								_listAccountCharges[j].AmountCur=_listAccountCharges[j].AmountOriginal;
							}
							else {
								_listAccountCharges[j].AmountCur+=split.SplitAmt;//Give the money back to the charge so it will display.
							}
							break;
						}
					}
				}
				ListSplitsCur.RemoveAt(gridSplits.SelectedIndices[i]);
			}
		}
		
		///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
		private void gridSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplitOld=ListSplitsCur[e.Row];
			PaySplit paySplitNew=paySplitOld.Copy();
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur);
			FormPSE.PaySplitCur=paySplitNew;
			FormPSE.IsNew=false;
			//FormPSE.Remain=PIn.Double(textAmtAvailable.Text);//Not used in PaySplitEdit, the button is never even visible.
			if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplitNew contains all the info we want.  
				double splitDiff=paySplitNew.SplitAmt-paySplitOld.SplitAmt;
				if(_payAvailableCur-splitDiff<0) {//New amount is greater, and it's more than the payment had left.
					MsgBox.Show(this,"Entered amount for split is greater than amount remaining for this payment.");
					return;
				}
				DeleteSelected();
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					FillChargeGrid();
					return;
				}
				//Find the charge row for this new split.
				List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(paySplitNew.PayPlanNum);
				List<long> listPayPlanChargeNums=new List<long>();
				for(int j=0;j<listCharges.Count;j++) {
					listPayPlanChargeNums.Add(listCharges[j].PayPlanChargeNum);
				}
				for(int i=0;i<_listAccountCharges.Count;i++){
					long priKey=_listAccountCharges[i].PriKey;
					string chargeType=_listAccountCharges[i].Type;
					if(paySplitNew.ProcNum!=0 && priKey==paySplitNew.ProcNum) {//New Split is for this proc
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(_listAccountCharges[i].Type=="Adjustment" //New split is for this adjust
							&& paySplitNew.ProcNum==0 && paySplitNew.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& paySplitNew.ProcDate==_listAccountCharges[i].Date
							&& paySplitNew.ProvNum==_listAccountCharges[i].ProvNum)
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(listPayPlanChargeNums.Contains(_listAccountCharges[i].PriKey)//New split is for this PayPlanCharge
							&& _listAccountCharges[i].Type=="PayPlanCharge")
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					//If none of these, it's unattached to the best of our knowledge.
				}
				_payAvailableCur-=paySplitNew.SplitAmt;
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+paySplitNew.SplitAmt).ToString("f");
				ListSplitsCur.Add(paySplitNew);
			}
			FillChargeGrid();
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
				CreateSplit(gridCharges.Rows[gridCharges.SelectedIndices[i]],ref amt,true);
			}
			FillChargeGrid();
		}

		///<summary>Creates paysplits after allowing the user to enter in a custom amount to pay for each selected charge.</summary>
		private void butCreatePartial_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				if(_payAvailableCur>0) {//Only make splits if we have some available.
					FormAmountEdit FormAE=new FormAmountEdit(gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text);
					FormAE.Amount=PIn.Double(gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[5].Text);
					FormAE.ShowDialog();
					if(FormAE.DialogResult==DialogResult.OK) {
						double amount=FormAE.Amount;
						if(amount>0) {
							CreateSplit(gridCharges.Rows[gridCharges.SelectedIndices[i]],ref amount,true);
						}
					}
				}
			}
			FillChargeGrid();
		}

		///<summary>Creates a manual paysplit.</summary>
		private void butCreateManual_Click(object sender,EventArgs e) {
			PaySplit paySplitNew=new PaySplit();
			paySplitNew.SplitAmt=_payAvailableCur;
			paySplitNew.DateEntry=DateTime.Today;
			paySplitNew.DatePay=DateTime.Today;
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur);
			FormPSE.PaySplitCur=paySplitNew;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				if(_payAvailableCur-paySplitNew.SplitAmt<0) {//New amount is greater, and it's more than the payment had left.
					MsgBox.Show(this,"Entered amount for split is greater than amount remaining for this payment.");
					return;
				}
				//Find the charge row for this new split.
				List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(paySplitNew.PayPlanNum);
				List<long> listPayPlanChargeNums=new List<long>();
				for(int j=0;j<listCharges.Count;j++) {
					listPayPlanChargeNums.Add(listCharges[j].PayPlanChargeNum);
				}
				for(int i=0;i<_listAccountCharges.Count;i++){
					long priKey=_listAccountCharges[i].PriKey;
					string chargeType=_listAccountCharges[i].Type;
					if(paySplitNew.ProcNum!=0 && priKey==paySplitNew.ProcNum) {//New Split is for this proc
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(_listAccountCharges[i].Type=="Adjustment" //New split is for this adjust
							&& paySplitNew.ProcNum==0 && paySplitNew.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& paySplitNew.ProcDate==_listAccountCharges[i].Date
							&& paySplitNew.ProvNum==_listAccountCharges[i].ProvNum)
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(listPayPlanChargeNums.Contains(_listAccountCharges[i].PriKey)//New split is for this PayPlanCharge
							&& _listAccountCharges[i].Type=="PayPlanCharge")
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed-paySplitNew.SplitAmt<0) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					//If none of these, it's unattached to the best of our knowledge.
				}
				_payAvailableCur-=paySplitNew.SplitAmt;
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+paySplitNew.SplitAmt).ToString("f");
				ListSplitsCur.Add(paySplitNew);
				FillChargeGrid();
			}
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
			PaymentCur.PayAmt=PIn.Double(textSplitAmt.Text);
			if(ListSplitsCur.Count>0) {
				PaymentCur.IsSplit=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//No paysplits were inserted. Just close the window.
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int RowComparer(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private class AccountEntry {
			public DateTime Date;
			public long PriKey;
			public string Type;
			public double AmountCur;
			public double AmountOriginal;
			public long ProvNum;
			public long ClinicNum;
			public long PatNum;
		}

	}
}