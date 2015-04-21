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
		public int _splitIdx;

		public FormPaySplitManage() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPaySplitManage_Load(object sender,EventArgs e) {
			_listAccountCharges=new List<AccountEntry>();
			_payAvailableCur=PaymentAmt;
			textPayAmt.Text=PaymentAmt.ToString("f");
			List<long> listPatNums=new List<long>();
			_splitIdx=0;
			for(int i=0;i<FamCur.ListPats.Length;i++) {
				listPatNums.Add(FamCur.ListPats[i].PatNum);
			}
			if(IsNew) {
				ListSplitsCur=AutoSplitForPayment(listPatNums,PaymentCur.PayNum,PayDate);//New payment, generated autosplits overwrites any manual/pre-existing splits.
				textSplitAmt.Text=(PaymentAmt-_payAvailableCur).ToString("f");//Amount Paid Increases as PayAmt decreases.
			}
			else {//Existing.
				textSplitAmt.Text=POut.Double(PaymentCur.PayAmt);
				if(_payAvailableCur>PaymentCur.PayAmt) {//If they increased the amount of the old payment, they want to be able to use it. 
					_payAvailableCur=_payAvailableCur-PaymentCur.PayAmt;
				}
				else {//If they decreased (or did not change) the amount of the old payment.
					_payAvailableCur=0;//Don't let them assign any new charges to this payment (but they can certainly take some off).
				}
				//We want to fill the charge table like we usually do.
				//With the splits already made on the existing payment they will be attributed correctly in AutoSplitForPayment.
				//AutoSplitForPayment will return new auto-splits if _payAvailableCur allows for some to be made.  Add these new splits to ListSplitsCur for display.
				ListSplitsCur.AddRange(AutoSplitForPayment(listPatNums,PaymentCur.PayNum,PayDate));
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
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
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
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					row.Cells.Add(Clinics.GetDesc(_listAccountCharges[i].ClinicNum));
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(Patients.GetPat(_listAccountCharges[i].PatNum).GetNameFL());
				row.Cells.Add(_listAccountCharges[i].GetType().Name);//Type
				if(_listAccountCharges[i].GetType()==typeof(Procedure)) {
					//Get the proc and add its description if the row is a proc.
					Procedure proc=(Procedure)_listAccountCharges[i].Tag;
					row.Cells[row.Cells.Count-1].Text+=": "+Procedures.GetDescription(proc);
				}
				row.Tag=_listAccountCharges[i].Tag;
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
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
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
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					if(ListSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetClinic(ListSplitsCur[i].ClinicNum).Description);//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				row.Cells.Add(Patients.GetPat(ListSplitsCur[i].PatNum).GetNameFL());//Patient
				if(ListSplitsCur[i].ProcNum!=0) {//Procedure
					Procedure proc=Procedures.GetOneProc(ListSplitsCur[i].ProcNum,false);
					string procDesc=Procedures.GetDescription(proc);
					row.Cells.Add("Proc: "+procDesc);//Type
				}
				else if(ListSplitsCur[i].PayPlanNum!=0) {
					row.Cells.Add("PayPlanCharge");//Type
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
		private List<PaySplit> AutoSplitForPayment(List<long> listPatNums,long payNum,DateTime date) {
			//Get the lists of items we'll be using to calculate with.
			List<Procedure> listProcs=Procedures.GetCompleteForPats(listPatNums);
			//listPayments should be empty, there isn't currently a way to make payments without at least one split.
			//During research however we found there were sometimes payments with no splits, so erred on the side of caution.
			List<Payment> listPayments=Payments.GetNonSplitForPats(listPatNums);
			List<Adjustment> listNegAdjustments=Adjustments.GetAdjustForPats(listPatNums,false);
			List<Adjustment> listPosAdjustments=Adjustments.GetAdjustForPats(listPatNums,true);
			List<PaySplit> listPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			List<ClaimProc> listInsPayAsTotal=ClaimProcs.GetByTotForPats(listPatNums);//Claimprocs paid as total, might contain ins payplan payments.
			List<PayPlan> listPayPlans=PayPlans.GetForPats(listPatNums,PatCur.PatNum);//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			List<PayPlanCharge> listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0){
				listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans,PatCur.PatNum);//Does not get charges for the future.
			}
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			for(int i=0;i<listPatNums.Count;i++) {
				listClaimProcs.AddRange(ClaimProcs.Refresh(listPatNums[i]));//There is no ClaimProcs.Refresh() for a family.
			}
			//Calculated using writeoffs, inspayest, inspayamt.  Done the same way ContrAcct does it.				
			for(int i=0;i<listProcs.Count;i++) {
				listProcs[i].ProcFee=ClaimProcs.GetPatPortion(listProcs[i],listClaimProcs);
			}
			//Now we have all negative and positive adjustments.
			#region Explicitly Linked Credits: Paysplits for Procedures
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
			#endregion Explicitly Linked Credits: Paysplits for Procedures
			#region Explicitly Linked Credits: Negative Adjustments for Procedures
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
			#endregion Explicitly Linked Credits: Negative Adjustments for Procedures
			#region Explicitly Linked Credits: Paysplits for Payplans
			//Attribute splits to their payplans.
			for(int i=0;i<listPaySplits.Count;i++) {
				PaySplit paySplit=listPaySplits[i];
				for(int j=0;j<listPayPlanCharges.Count;j++) {
					PayPlanCharge payPlanCharge=listPayPlanCharges[j];
					if(paySplit.PayPlanNum!=payPlanCharge.PayPlanNum || payPlanCharge.Principal<=0) {
						continue;//Not the correct payplan or the charge has been paid in full already.
					}
					if((payPlanCharge.Principal+payPlanCharge.Interest)<=paySplit.SplitAmt) {
						//Used part of split as it potentially covers more than one payplancharge.
						paySplit.SplitAmt-=(payPlanCharge.Principal+payPlanCharge.Interest);
						payPlanCharge.Principal=0;
						payPlanCharge.Interest=0;
					}
					else {//Principal+Interest>SplitAmt.  Use all of split.  Eliminate the charge's interest first, then the principal. 
						if(payPlanCharge.Interest<paySplit.SplitAmt) {//Use part of split on interest.
							paySplit.SplitAmt-=payPlanCharge.Interest;
							payPlanCharge.Interest=0;
						}
						else {//Interest>=SplitAmt.  Use all of split on interest.
							payPlanCharge.Interest-=paySplit.SplitAmt;
							paySplit.SplitAmt=0;
							break;//If split is 0, we don't need to continue.
						}
						//(Principal+Interest)>SplitAmt => Principal>SplitAmt-Interest.  Since now SplitAmt=SplitAmt-Interest, really we have Principal>SplitAmt.
						//Use all of split on principal (Only happens if the split exactly pays off the charge).
						payPlanCharge.Principal-=paySplit.SplitAmt;
						paySplit.SplitAmt=0;
						break;
					}
				}
			}
			#endregion Explicitly Linked Credits: Paysplits for Payplans
			#region Explicitly Linked Credits: Insurance Payments Received for Ins Payplans
			//Attribute "as total" insurance payments to their potential ins payplans.
			for(int i=0;i<listInsPayAsTotal.Count;i++) {
				ClaimProc claimProcTot=listInsPayAsTotal[i];
				for(int j=0;j<listPayPlanCharges.Count;j++) {
					PayPlanCharge payPlanCharge=listPayPlanCharges[j];
					if(claimProcTot.PayPlanNum!=payPlanCharge.PayPlanNum || payPlanCharge.Principal<=0) {
						continue;//Not the correct payplan, or the charge has been paid in full already.
					}
					if((payPlanCharge.Principal+payPlanCharge.Interest)<=claimProcTot.InsPayAmt) {
						//Use part of ins pay as it potentially covers more than one payplancharge.
						claimProcTot.InsPayAmt-=(payPlanCharge.Principal+payPlanCharge.Interest);
						payPlanCharge.Principal=0;
						payPlanCharge.Interest=0;
					}
					else {//Principal+Interest>InsPayAmt.  Use all of ins pay.  Eliminate the charge's interest first, then the principal. 
						if(payPlanCharge.Interest<claimProcTot.InsPayAmt) {//Use part of ins pay on interest.
							claimProcTot.InsPayAmt-=payPlanCharge.Interest;
							payPlanCharge.Interest=0;
						}
						else {//Interest>=InsPayAmt.  Use all of ins pay on interest.
							payPlanCharge.Interest-=claimProcTot.InsPayAmt;
							claimProcTot.InsPayAmt=0;
							break;//If split is 0, we don't need to continue.
						}
						//(Principal+Interest)>InsPayAmt => Principal>InsPayAmt-Interest.  Since now InsPayAmt=InsPayAmt-Interest, really we have Principal>InsPayAmt.
						//Apply the rest that's left over on the principal.
						payPlanCharge.Principal-=claimProcTot.InsPayAmt;
						claimProcTot.InsPayAmt=0;
						break;
					}
				}
			}
			#endregion Explicitly Linked Credits: Insurance Payments Received for Ins Payplans
			//Over the next 4 regions, we will do the following:
			//Now we have all credits attributed to their procedures/payplancharges (if they were attributed).  We may have a whole host of 
			//procs with remaining fees, as well as payplan charges that have yet to be paid off (excluding those in the future).  Pay
			//everything off FIFO by date, across object boundaries by making a list containing AccountEntry objects.  Then, go through the list and apply
			//payments.  What remains is everything which is unpaid, so make splits for our current payment based on those.  Order by date.
			#region Construct List of Charges
			_listAccountCharges=new List<AccountEntry>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
				_listAccountCharges.Add(new AccountEntry(listPayPlanCharges[i]));
			}
			for(int i=0;i<listPosAdjustments.Count;i++) {
				_listAccountCharges.Add(new AccountEntry(listPosAdjustments[i]));
			}
			for(int i=0;i<listProcs.Count;i++) {
				_listAccountCharges.Add(new AccountEntry(listProcs[i]));
			}
			_listAccountCharges.Sort(AccountEntryComparer);
			#endregion Construct List of Charges
			#region Construct List of Credits
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			List<AccountEntry> listAccountCredits=new List<AccountEntry>();
			for(int i=0;i<listNegAdjustments.Count;i++) {
				if(listNegAdjustments[i].AdjAmt>=0) {//There's none remaining
					continue;
				}
				listAccountCredits.Add(new AccountEntry(listNegAdjustments[i]));
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				if(listPaySplits[i].SplitAmt<=0) {
					continue;
				}
				listAccountCredits.Add(new AccountEntry(listPaySplits[i]));
			}
			for(int i=0;i<listPayments.Count;i++) {
				if(listPayments[i].PayAmt<=0) {
					continue;
				}				
				listAccountCredits.Add(new AccountEntry(listPayments[i]));
			}
			for(int i=0;i<listInsPayAsTotal.Count;i++) {
				if(listInsPayAsTotal[i].InsPayAmt<=0) {
					continue;
				}				
				listAccountCredits.Add(new AccountEntry(listInsPayAsTotal[i]));
			}
			for(int i=0;i<listPayPlans.Count;i++) {
				listAccountCredits.Add(new AccountEntry(listPayPlans[i]));
			}
			listAccountCredits.Sort(AccountEntryComparer);
			#endregion Construct List of Credits
			#region Implicit Linking
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountCur<=0) {
					continue;//Charge is already paid.  Payment is already linked to this charge, either implicit or explicit.
				}
				for(int j=0;j<listAccountCredits.Count;j++) {
					AccountEntry credit=listAccountCredits[j];
					if(credit.AmountCur<=0) {
						continue;//The credit has already been used.  May have been set to 0 for a previous owed item.
					}
					if(credit.GetType()==typeof(PayPlan) && charge.GetType()==typeof(PayPlanCharge)) {
						//These are skipped because payplancharges are paid by splits and adjustments only.
						//The payplan creates an immediate credit for the patient account, but not the payment plan (the payment plan is credited over time).
						continue;
					}
					if(charge.AmountCur<credit.AmountCur) {//This credit item can pay the owed item in full.  Use a partial amount.
						credit.AmountCur-=charge.AmountCur;
						charge.AmountCur=0;
						break;
					}
					else {//This credit item cannot pay the owed item in full. Use all of credit, continue on to the next credit item.
						charge.AmountCur-=credit.AmountCur;
						credit.AmountCur=0;
						listAccountCredits[j].AmountCur=0;
					}
				}
			}
			#endregion Implicit Linking
			#region Auto-split Current Payment
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			List<PaySplit> listAutoSplits=new List<PaySplit>();
			for(int i=0;i<_listAccountCharges.Count && _payAvailableCur>0;i++) {
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountCur<=0) {
					continue;//Skip charges which are already paid.
				}
				PaySplit split=new PaySplit();
				if(charge.AmountCur<_payAvailableCur) {//Use partial payment
					split.SplitAmt=charge.AmountCur;
					_payAvailableCur-=charge.AmountCur;
					charge.AmountCur=0;
				}
				else {//Use full payment
					split.SplitAmt=_payAvailableCur;
					charge.AmountCur-=_payAvailableCur;
					_payAvailableCur=0;
				}				
				split.DatePay=date;
				if(charge.GetType()==typeof(Procedure)) {
					Procedure proc=(Procedure)charge.Tag;
					split.PatNum=proc.PatNum;
					split.ProcDate=proc.ProcDate;
					split.ProcNum=proc.ProcNum;
					split.ProvNum=proc.ProvNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=proc.ClinicNum;
					}
				}
				else if(charge.GetType()==typeof(PayPlanCharge)) {
					PayPlanCharge payPlanCharge=(PayPlanCharge)charge.Tag;
					split.PatNum=payPlanCharge.PatNum;
					split.ProcDate=payPlanCharge.ChargeDate;
					split.ProvNum=payPlanCharge.ProvNum;
					split.PayPlanNum=payPlanCharge.PayPlanNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=charge.ClinicNum;
					}
				}
				else {//Adjustment
					Adjustment adjustment=(Adjustment)charge.Tag;
					split.PatNum=adjustment.PatNum;
					split.ProcDate=adjustment.AdjDate;
					split.ProvNum=adjustment.ProvNum;
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
						split.ClinicNum=adjustment.ClinicNum;
					}
				}
				split.PayNum=payNum;
				split.SplitNum=_splitIdx;
				_listAccountCharges[i].ListPaySplitNums.Add(split.SplitNum);
				_splitIdx++;
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
			split.SplitNum=_splitIdx;
			_splitIdx++;
			split.DatePay=DateTime.Today;
			if(row.Tag.GetType()==typeof(Procedure)) {
				Procedure proc=(Procedure)row.Tag;
				for(int j=0;j<_listAccountCharges.Count;j++) {
					if(_listAccountCharges[j].PriKey!=proc.ProcNum || _listAccountCharges[j].GetType()!=typeof(Procedure)) {
						continue;
					}
					double amtOwed=_listAccountCharges[j].AmountCur;
					if(amtOwed<payment) {//Partial payment
						payment-=amtOwed;
						_listAccountCharges[j].AmountCur=0;//Reflect payment in underlying datastructure
						split.SplitAmt=amtOwed;
					}
					else {//Full payment
						_listAccountCharges[j].AmountCur=amtOwed-payment;
						split.SplitAmt=payment;
						payment=0;
					}
					_listAccountCharges[j].ListPaySplitNums.Add(split.SplitNum);
					break;					
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
					if(_listAccountCharges[j].PriKey!=charge.PayPlanChargeNum || _listAccountCharges[j].GetType()!=typeof(PayPlanCharge)) {
						continue;
					}
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
					_listAccountCharges[j].ListPaySplitNums.Add(split.SplitNum);
					break;					
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
					if(_listAccountCharges[j].PriKey!=adjust.AdjNum || _listAccountCharges[j].GetType()!=typeof(Adjustment)) {
						continue;
					}
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
					_listAccountCharges[j].ListPaySplitNums.Add(split.SplitNum);
					break;
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
				for(int j=0;j<_listAccountCharges.Count;j++) {
					if(!_listAccountCharges[j].ListPaySplitNums.Contains(split.SplitNum))	{
						continue;
					}
					double chargeAmtNew=_listAccountCharges[j].AmountCur+split.SplitAmt;
					if(chargeAmtNew>_listAccountCharges[j].AmountOriginal) {//Trying to refund an overpayment, just increase charge's amount to the max.
						_listAccountCharges[j].AmountCur=_listAccountCharges[j].AmountOriginal;
					}
					else {
						_listAccountCharges[j].AmountCur+=split.SplitAmt;//Give the money back to the charge so it will display.
					}
					_listAccountCharges[j].ListPaySplitNums.Remove(split.SplitNum);
					break;
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
			if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplitNew contains all the info we want.  
				double splitDiff=paySplitNew.SplitAmt-paySplitOld.SplitAmt;
				if(_payAvailableCur<splitDiff) {//New amount is greater, and it's more than the payment had left.
					MsgBox.Show(this,"Entered amount for split is greater than amount remaining for this payment.");
					return;
				}
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  Paysplit may be re-associated with a different charge.
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
				for(int i=0;i<_listAccountCharges.Count;i++) {
					long priKey=_listAccountCharges[i].PriKey;
					if(paySplitNew.ProcNum!=0 && priKey==paySplitNew.ProcNum) {//New Split is for this proc
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
						_listAccountCharges[i].ListPaySplitNums.Add(paySplitNew.SplitNum);
 						break;
					}
					else if(_listAccountCharges[i].GetType()==typeof(Adjustment) //New split is for this adjust
							&& paySplitNew.ProcNum==0 && paySplitNew.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& paySplitNew.ProcDate==_listAccountCharges[i].Date
							&& paySplitNew.ProvNum==_listAccountCharges[i].ProvNum)
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
						_listAccountCharges[i].ListPaySplitNums.Add(paySplitNew.SplitNum);
 						break;
					}
					else if(listPayPlanChargeNums.Contains(_listAccountCharges[i].PriKey)//New split is for this PayPlanCharge
							&& _listAccountCharges[i].GetType()==typeof(PayPlanCharge))
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
						_listAccountCharges[i].ListPaySplitNums.Add(paySplitNew.SplitNum);
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
				if(_payAvailableCur<=0) {//Only make splits if we have some available.
					continue;
				}
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
					if(paySplitNew.ProcNum!=0 && priKey==paySplitNew.ProcNum) {//New Split is for this proc
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(_listAccountCharges[i].GetType()==typeof(Adjustment) //New split is for this adjust
							&& paySplitNew.ProcNum==0 && paySplitNew.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
							&& paySplitNew.ProcDate==_listAccountCharges[i].Date
							&& paySplitNew.ProvNum==_listAccountCharges[i].ProvNum)
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
							_listAccountCharges[i].AmountCur=0;//Reflect payment in underlying datastructure
						}
						else {//Full payment
							_listAccountCharges[i].AmountCur=amtOwed-paySplitNew.SplitAmt;
						}
 						break;
					}
					else if(listPayPlanChargeNums.Contains(_listAccountCharges[i].PriKey)//New split is for this PayPlanCharge
							&& _listAccountCharges[i].GetType()==typeof(PayPlanCharge))
					{
						double amtOwed=_listAccountCharges[i].AmountCur;
						if(amtOwed<paySplitNew.SplitAmt) {//Partial payment
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
				if(_payAvailableCur<diff) {//They decreased the payment amount
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
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//No paysplits were inserted.  Just close the window.
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int AccountEntryComparer(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private class AccountEntry {
			public object Tag;
			public DateTime Date;
			public long PriKey;
			public double AmountOriginal;
			public double AmountCur;
			public long ProvNum;
			public long ClinicNum;
			public long PatNum;
			public List<long> ListPaySplitNums;//List of PaySplitNum so we can have multiple splits for the same charge.

			public new Type GetType() {
				return Tag.GetType();
			}

			public AccountEntry(PayPlanCharge payPlanCharge) {
				Tag=payPlanCharge;
				Date=payPlanCharge.ChargeDate;
				PriKey=payPlanCharge.PayPlanChargeNum;
				AmountOriginal=payPlanCharge.Principal+payPlanCharge.Interest;
				AmountCur=AmountOriginal;
				ProvNum=payPlanCharge.ProvNum;
				ClinicNum=payPlanCharge.ClinicNum;
				PatNum=payPlanCharge.PatNum;
				ListPaySplitNums=new List<long>();
			}

			///<summary>Turns negative adjustments positive.</summary>
			public AccountEntry(Adjustment adjustment) {
				Tag=adjustment;
				Date=adjustment.AdjDate;
				PriKey=adjustment.AdjNum;
				AmountOriginal=Math.Abs(adjustment.AdjAmt);
				AmountCur=AmountOriginal;
				ProvNum=adjustment.ProvNum;
				ClinicNum=adjustment.ClinicNum;
				PatNum=adjustment.PatNum;
				ListPaySplitNums=new List<long>();
			}

			public AccountEntry(Procedure proc) {
				Tag=proc;
				Date=proc.ProcDate;
				PriKey=proc.ProcNum;
				AmountOriginal=proc.ProcFee;
				AmountCur=AmountOriginal;
				ProvNum=proc.ProvNum;
				ClinicNum=proc.ClinicNum;
				PatNum=proc.PatNum;
				ListPaySplitNums=new List<long>();
			}

			public AccountEntry(PaySplit paySplit) {
				Tag=paySplit;
				Date=paySplit.DatePay;
				PriKey=paySplit.SplitNum;
				AmountOriginal=paySplit.SplitAmt;
				AmountCur=AmountOriginal;
				ProvNum=paySplit.ProvNum;
				ClinicNum=paySplit.ClinicNum;
				PatNum=paySplit.PatNum;
				ListPaySplitNums=new List<long>();
			}

			public AccountEntry(Payment payment) {
				Tag=payment;
				Date=payment.PayDate;
				PriKey=payment.PayNum;
				AmountOriginal=payment.PayAmt;
				AmountCur=AmountOriginal;
				ProvNum=0;//Payments don't have a ProvNum
				ClinicNum=payment.ClinicNum;
				PatNum=payment.PatNum;
				ListPaySplitNums=new List<long>();
			}

			public AccountEntry(ClaimProc claimProc) {
				Tag=claimProc;
				Date=claimProc.DateCP;
				PriKey=claimProc.ClaimProcNum;
				AmountOriginal=claimProc.InsPayAmt;
				AmountCur=AmountOriginal;
				ProvNum=claimProc.ProvNum;
				ClinicNum=claimProc.ClinicNum;
				PatNum=claimProc.PatNum;
				ListPaySplitNums=new List<long>();
			}

			public AccountEntry(PayPlan payPlan) {
				Tag=payPlan;
				Date=payPlan.PayPlanDate;
				PriKey=payPlan.PayPlanNum;
				AmountOriginal=payPlan.CompletedAmt;
				AmountCur=AmountOriginal;
				ProvNum=0;
				ClinicNum=0;
				PatNum=payPlan.PatNum;
				ListPaySplitNums=new List<long>();
			}

		}

	}
}