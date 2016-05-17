using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPaySplitManage:ODForm {
		///<summary>List of current paysplits for this payment.</summary>
		public List<PaySplit> ListSplitsCur;
		///<summary>List of current account charges for the family.  Gets filled from AutoSplitForPayment</summary>
		private List<AccountEntry> _listAccountCharges;
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.  May be changed in this window.</summary>
		public decimal PaymentAmt;
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.
		///If this value is zero, it will be set to the summation of the split amounts when OK is clicked.</summary>
		public decimal AmtTotal;
		public Family FamCur;
		public Patient PatCur;
		///<summary>Payment from the Payment window, amount gets modified only in the case that the original paymentAmt is zero.  It gets increased to
		///whatever the paysplit total is when the window is closed.</summary>
		public Payment PaymentCur;
		public DateTime PayDate;
		public bool IsNew;
		private List<long> listPatNums;

		public FormPaySplitManage() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPaySplitManage_Load(object sender,EventArgs e) {
			Init(false);
		}

		///<summary>Performs all of the Load functionality.  Public so it can be called from unit tests.</summary>
		public void Init(bool isTest) {
			if(!Security.IsAuthorized(Permissions.PaymentEdit,PaymentCur.PayDate,true)) {
				butDelete.Enabled=false;
				butClear.Enabled=false;
			}
			_listAccountCharges=new List<AccountEntry>();
			textPayAmt.Text=PaymentAmt.ToString("f");
			AmtTotal=PaymentAmt;
			listPatNums=new List<long>();
			for(int i=0;i<FamCur.ListPats.Length;i++) {
				listPatNums.Add(FamCur.ListPats[i].PatNum);
			}
			//This logic will ensure that regardless of if it's a new, or old payment any created paysplits that haven't been saved, 
			//such as if splits were made in this window then the window was closed and then reopened, will persist.
			decimal splitTotal=0;
			for(int i=0;i<ListSplitsCur.Count;i++) {
				splitTotal+=(decimal)ListSplitsCur[i].SplitAmt;
			}
			textSplitTotal.Text=POut.Decimal(splitTotal);
			PaymentAmt=Math.Round(PaymentAmt-splitTotal,3);
			//We want to fill the charge table.
			//AutoSplitForPayment will return new auto-splits if _payAvailableCur allows for some to be made.  Add these new splits to ListSplitsCur for display.
			ListSplitsCur.AddRange(AutoSplitForPayment(PaymentCur.PayNum,PayDate,isTest));
			FillGridSplits();
			//Select all charges on the right side that the paysplits are associated with.  Helps the user see what charges are attached.
			gridSplits.SetSelected(true);
			HighlightChargesForSplits();
		}

		///<summary>Fills the paysplit grid.</summary>
		private void FillGridSplits() {
			//Fill left grid with paysplits created
			gridSplits.BeginUpdate();
			gridSplits.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),65,HorizontalAlignment.Center);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),40);
			gridSplits.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
				col=new ODGridColumn(Lan.g(this,"Clinic"),40);
				gridSplits.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Patient"),100);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),55,HorizontalAlignment.Right);
			gridSplits.Columns.Add(col);
			gridSplits.Rows.Clear();
			ODGridRow row;
			decimal splitTotal=0;
			for(int i=0;i<ListSplitsCur.Count;i++) {
				splitTotal+=(decimal)ListSplitsCur[i].SplitAmt;
				row=new ODGridRow();
				row.Tag=ListSplitsCur[i];
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
				string type="";
				if(ListSplitsCur[i].PayPlanNum!=0) {
					type+="PayPlanCharge";//Type
					if(ListSplitsCur[i].ProcNum==0 && ListSplitsCur[i].ProvNum!=0) {
						type+=" (interest)";
					}
				}
				if(ListSplitsCur[i].ProcNum!=0) {//Procedure
					Procedure proc=Procedures.GetOneProc(ListSplitsCur[i].ProcNum,false);
					string procDesc=Procedures.GetDescription(proc);
					if(type!="") {
						type+="\r\n";
					}
					type+="Proc: "+procDesc;//Type
				}
				if(ListSplitsCur[i].ProvNum==0) {//Unattached split
					if(type!="") {
						type+="\r\n";
					}
					type+="Unallocated";//Type
				}
				row.Cells.Add(type);
				if(row.Cells[row.Cells.Count-1].Text=="Unallocated") {
					row.Cells[row.Cells.Count-1].ColorText=Color.Red;
				}
				row.Cells.Add(ListSplitsCur[i].SplitAmt.ToString("f"));//Amount
				gridSplits.Rows.Add(row);
			}
			textSplitTotal.Text=POut.Decimal(splitTotal);
			gridSplits.EndUpdate();
			FillGridCharges();
		}

		///<summary>Fills charge grid, and then split grid.</summary>
		private void FillGridCharges() {
			//Fill right-hand grid with all the charges, filtered based on checkbox.
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
			col=new ODGridColumn(Lan.g(this,"Patient"),100);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt Orig"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt Start"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt End"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			gridCharges.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(!checkShowPaid.Checked) {//Filter out those that are paid in full and from other payments if checkbox unchecked.
					bool isFound=false;
					if(entryCharge.AmountEnd!=0) {
						isFound=true;
					}
					for(int j=0;j<gridSplits.Rows.Count;j++) {
						PaySplit entryCredit=(PaySplit)gridSplits.Rows[j].Tag;
						if(entryCharge.ListPaySplits.Contains(entryCredit)) 
							//Charge is paid for by a split in this payment, display it.
						{
							if(entryCharge.GetType()==typeof(Procedure) && entryCredit.PayPlanNum!=0) {
								//Don't show the charge if it's a proc being paid by a payplan split.
								//From the user's perspective they're paying the "debits" not the procs.
							}
							else { 
								isFound=true;
								break;
							}
						}
					}
					if(!isFound) {//Hiding charges that aren't associated with the current payment or have been paid in full.
						continue;
					}
				}
				row=new ODGridRow();
				row.Tag=_listAccountCharges[i];
				row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					row.Cells.Add(Clinics.GetDesc(entryCharge.ClinicNum));
				}
				row.Cells.Add(Patients.GetPat(entryCharge.PatNum).GetNameFL());
				row.Cells.Add(entryCharge.GetType().Name);//Type
				if(entryCharge.GetType()==typeof(Procedure)) {
					//Get the proc and add its description if the row is a proc.
					Procedure proc=(Procedure)entryCharge.Tag;
					row.Cells[row.Cells.Count-1].Text="Proc: "+Procedures.GetDescription(proc);
				}
				row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				row.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
				row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
				gridCharges.Rows.Add(row);
			}
			gridCharges.EndUpdate();
		}

		///<summary>Creates paysplits associated to the patient passed in for the current payment until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  PaymentAmt will attempt to move towards 0 as paysplits are created.</summary>
		private List<PaySplit> AutoSplitForPayment(long payNum,DateTime date,bool isTest) {
			int payPlanVersionCur=PrefC.GetInt(PrefName.PayPlansVersion);
			//Get the lists of items we'll be using to calculate with.
			List<Procedure> listProcs=Procedures.GetCompleteForPats(listPatNums);
			//listPayments should be empty, there isn't currently a way to make payments without at least one split.
			//During research however we found there were sometimes payments with no splits, so erred on the side of caution.
			List<Payment> listPayments=Payments.GetNonSplitForPats(listPatNums);
			List<Adjustment> listAdjustments=Adjustments.GetAdjustForPats(listPatNums);
			List<PaySplit> listPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			//Fix the memory locations of the existing pay splits for this payment within the list of pay splits for the entire family.
			//This is necessary for associating the correct tag values to grid rows.
			for(int i=listPaySplits.Count-1;i>=0;i--) {
				bool isFound=false;
				for(int j=0;j<ListSplitsCur.Count;j++) {
					if(listPaySplits[i].SplitNum==ListSplitsCur[j].SplitNum) {
						listPaySplits[i]=ListSplitsCur[j];
						isFound=true;
					}
				}
				if(!isFound && listPaySplits[i].PayNum==PaymentCur.PayNum) {
					//If we have a split that's not found in the passed-in list of splits for the payment
					//and the split we got from the DB is on this payment, remove it because the user must have deleted the split from the payment window.
					//The payment window won't update the DB with the change until it's closed.
					listPaySplits.RemoveAt(i);
				}
			}
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
			//Over the next 5 regions, we will do the following:
			//Create a list of account charges
			//Create a list of account credits
			//Explicitly link any of the credits to their corresponding charges if a link can be made. (ie. PaySplit.ProcNum to a Procedure.ProcNum)
			//Implicitly link any of the remaining credits to the non-zero charges FIFO by date.
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			#region Construct List of Charges
			_listAccountCharges=new List<AccountEntry>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
				if(listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit
					&& listPayPlans.Find(x => x.PayPlanNum==listPayPlanCharges[i].PayPlanNum).InsSubNum==0)
				{ //for v1, debits are the only ChargeType.  We only want debits from normal payplans, not ins payplans.
					_listAccountCharges.Add(new AccountEntry(listPayPlanCharges[i]));
				}
			}
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjAmt>0 && listAdjustments[i].ProcNum==0) {
					_listAccountCharges.Add(new AccountEntry(listAdjustments[i]));
				}
			}
			for(int i=0;i<listProcs.Count;i++) {
				_listAccountCharges.Add(new AccountEntry(listProcs[i]));
			}
			_listAccountCharges.Sort(AccountEntrySort);
			#endregion Construct List of Charges
			#region Construct List of Credits
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			decimal creditTotal=0;
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjAmt<0) {
					creditTotal-=(decimal)listAdjustments[i].AdjAmt;
				}
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				creditTotal+=(decimal)listPaySplits[i].SplitAmt;
			}
			for(int i=0;i<ListSplitsCur.Count;i++) {
				if(ListSplitsCur[i].SplitNum==0) {
					//If they created new splits on an old payment we need to add those to the credits list since they won't be over-written unlike a new payment.
					creditTotal+=(decimal)ListSplitsCur[i].SplitAmt;//Adding splits that haven't been entered into DB yet (re-opened split manager)
				}
			}
			for(int i=0;i<listPayments.Count;i++) {
				creditTotal+=(decimal)listPayments[i].PayAmt;
			}
			for(int i=0;i<listInsPayAsTotal.Count;i++) {			
				creditTotal+=(decimal)listInsPayAsTotal[i].InsPayAmt;
			}
			if(payPlanVersionCur==1) { //for version 1, just use the completed amount of the payment plan.
				for(int i=0;i<listPayPlans.Count;i++) {
					creditTotal+=(decimal)listPayPlans[i].CompletedAmt;
				}
			}
			else if(payPlanVersionCur==2) { //for version 2, use the sum of the credits that are in the past.
				for(int i=0;i<listPayPlanCharges.Count;i++) {
					if(listPayPlanCharges[i].ChargeType!=PayPlanChargeType.Credit) {
						continue;
					}
					if(listPayPlanCharges[i].ProcNum!=0) {
						continue;
					}
					//Credits not attached to a procedure need to be used to counteract general charges instead of explicitly linked.
					creditTotal+=(decimal)listPayPlanCharges[i].Principal;
				}
			}
			#endregion Construct List of Credits
			#region Explicitly Link Credits
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				ExplicitlyJoinSplits(listPaySplits,charge,creditTotal);
				if(payPlanVersionCur==2 && charge.GetType()==typeof(Procedure)) {
					for(int j=0;j<listPayPlanCharges.Count;j++) {
						if(listPayPlanCharges[j].ChargeType!=PayPlanChargeType.Credit) {
							continue;
						}
						if(listPayPlanCharges[j].ProcNum==0) {
							continue;
						}
						if(listPayPlanCharges[j].Principal==0) {
							continue;
						}
						//Pay off the procs the charge is attached to so the procs don't show up as "charges" in the grid
						AccountEntry procEntry=_listAccountCharges
							.Find(x => x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcNum==listPayPlanCharges[j].ProcNum);
						procEntry.AmountEnd-=(decimal)listPayPlanCharges[j].Principal;
						listPayPlanCharges[j].Principal=0;
					}
				}
				List<PaySplit> listSplitsTemp=new List<PaySplit>();
				foreach(PaySplit paySplit in ListSplitsCur) {
					if(listPaySplits.Exists(x => x.SplitNum==paySplit.SplitNum)) {
						continue;//skip paysplits which have already been explicitly linked above.
					}
					listSplitsTemp.Add(paySplit.Copy());
				}
				ExplicitlyJoinSplits(listSplitsTemp,charge,creditTotal);
				for(int j=0;j<listAdjustments.Count;j++) {
					Adjustment adjustment=listAdjustments[j];
					decimal adjustmentAmt=(decimal)adjustment.AdjAmt;
					if(charge.GetType()==typeof(Procedure) && adjustment.ProcNum==charge.PriKey) {
						charge.AmountEnd+=adjustmentAmt;
						if(adjustment.AdjAmt<0) {
							creditTotal+=adjustmentAmt;
						}
						charge.AmountStart+=adjustmentAmt;
						//If the adjustment is attached to a procedure decrease the procedure's amountoriginal so we know what it was just prior to autosplitting.
					}
				}
			}
			#endregion Explicitly Link Credits
			//Apply negative charges as if they're credits.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.AmountEnd<0) {
					creditTotal-=entryCharge.AmountEnd;
					entryCharge.AmountEnd=0;
				}				
			}
			#region Implicitly Link Credits
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count && creditTotal>0;i++) {
				AccountEntry charge=_listAccountCharges[i];
				decimal amt=Math.Min(charge.AmountEnd,creditTotal);
				charge.AmountEnd-=amt;
				creditTotal-=amt;
				charge.AmountStart-=amt;//Decrease amount original for the charge so we know what it was just prior to when the autosplits were made.
			}
			#endregion Implicitly Link Credits
			#region Auto-Split Current Payment
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			List<PaySplit> listAutoSplits=new List<PaySplit>();
			PaySplit split;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				if(PaymentAmt==0) {
					break;
				}
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountEnd==0) {
					continue;//Skip charges which are already paid.
				}
				if(PaymentAmt<0 && charge.AmountEnd>0) {//If they're different signs, don't make any guesses.  
					//Remaining credits will always be all of one sign.
					if(!isTest) {
						MsgBox.Show(this,"Payment cannot be automatically allocated because there are no outstanding negative balances.");
					}
					return listAutoSplits;//Will be empty
				}
				split=new PaySplit();
				if(charge.GetType()==typeof(PayPlanCharge) && payPlanVersionCur==2) {//Payments need to be allocated differently with version 2 payplancharges
					CreatePayPlanAutoSplits(((PayPlanCharge)charge.Tag).PayPlanNum,date,payNum,listAutoSplits,listPayPlanCharges);
					continue;
				}
				else {//v1 payplan
					if(Math.Abs(charge.AmountEnd)<Math.Abs(PaymentAmt)) {//charge has "less" than the payment, use partial payment.
						split.SplitAmt=(double)charge.AmountEnd;
						PaymentAmt=Math.Round(PaymentAmt-charge.AmountEnd,3);
						charge.AmountEnd=0;
					}
					else {//Use full payment
						split.SplitAmt=(double)PaymentAmt;
						charge.AmountEnd-=PaymentAmt;
						PaymentAmt=0;
					}
				}
				split.DatePay=date;
				split.PatNum=charge.PatNum;
				split.ProcDate=charge.Date;
				split.ProvNum=charge.ProvNum;
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=charge.ClinicNum;
				}
				if(charge.GetType()==typeof(Procedure)) {
					split.ProcNum=charge.PriKey;
				}
				else if(charge.GetType()==typeof(PayPlanCharge)) {
					split.PayPlanNum=((PayPlanCharge)charge.Tag).PayPlanNum;
				}
				split.PayNum=payNum;
				charge.ListPaySplits.Add(split);
				listAutoSplits.Add(split);
			}
			if(listAutoSplits.Count==0 && ListSplitsCur.Count==0 && PaymentAmt != 0) {//Ensure there is at least one auto split if they entered a payAmt.
				split=new PaySplit();
				split.SplitAmt=(double)PaymentAmt;
				PaymentAmt=0;
				split.DatePay=date;
				split.PatNum=PaymentCur.PatNum;
				split.ProcDate=PaymentCur.PayDate;
				split.ProvNum=0;
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=payNum;
				listAutoSplits.Add(split);
			}
			if(PaymentAmt != 0) {//Create an unallocated split if there is any remaining payment amount.
				split=new PaySplit();
				split.SplitAmt=(double)PaymentAmt;
				PaymentAmt=0;
				split.DatePay=date;
				split.PatNum=PaymentCur.PatNum;
				split.ProcDate=PaymentCur.PayDate;
				split.ProvNum=0;
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=payNum;
				listAutoSplits.Add(split);
			}
			#endregion Auto-Split Current Payment
			return listAutoSplits;
		}

		private void ExplicitlyJoinSplits(List<PaySplit> listSplits,AccountEntry charge,decimal creditTotal) {
			for(int i=0;i<listSplits.Count;i++) {
				PaySplit paySplit=listSplits[i];//The splits in listSplits aren't saved anywhere.  Therefore we can change the SplitAmt at will.
				decimal paySplitAmt=(decimal)paySplit.SplitAmt;
				if(charge.GetType()==typeof(Procedure) && paySplit.ProcNum==charge.PriKey) {
					charge.ListPaySplits.Add(paySplit);
					charge.AmountEnd-=paySplitAmt;
					creditTotal-=paySplitAmt;
					if(paySplit.PayNum!=PaymentCur.PayNum) {//This will make it so the AmountOriginal will reflect only what this payment paid.
						charge.AmountStart-=paySplitAmt;
					}
				}
				else if(charge.GetType()==typeof(PayPlanCharge) && ((PayPlanCharge)charge.Tag).PayPlanNum==paySplit.PayPlanNum && charge.AmountEnd>0) {
					//It's a split for the payplan.  Pay off the charge in full or in partial.
					if(paySplitAmt <= charge.AmountEnd) {//partial payment of the charge
						charge.AmountEnd-=paySplitAmt;
						creditTotal-=paySplitAmt;
						paySplit.SplitAmt=0;
					}
					else {//full payment, paySplitAmt > charge.AmountEnd
						creditTotal-=charge.AmountEnd;
						//AmountEnd is decreased with each payment on this credit.  
						//We know that paySplitAmt is greater than AmountEnd.
						paySplit.SplitAmt-=(double)charge.AmountEnd;
						charge.AmountEnd=0;
						break;
					}
				}
			}
		}

		///<summary>Only for V2 PayPlans.  Creates a series of payment splits for a payplan, including interest splits and splits to procedures associated to the payplan.</summary>
		private void CreatePayPlanAutoSplits(long payPlanNum,DateTime date,long payNum,List<PaySplit> listAutoSplits,List<PayPlanCharge> listPayPlanCharges) {
			//NOTE: This method is called in a loop, one per account charge, in the automation logic.
			//Charge is a debit.  Make a split for the interest.  Use the rest for splits among procs attached to the payplan that aren't already paid off.
			//If there is any remaining amount, make a split to the payplan prov
			foreach(AccountEntry entry in _listAccountCharges) {//Pay off payment plan interest.
				if(PaymentAmt<=0) {
					break;
				}
				if(entry.AmountEnd<=0) {
					continue;
				}
				if(entry.GetType()!=typeof(PayPlanCharge)) {
					continue;
				}
				PayPlanCharge payPlanChargeNew=(PayPlanCharge)entry.Tag;
				if(payPlanChargeNew.PayPlanNum!=payPlanNum) {
					continue;
				}
				//A second debit for the same payplan.  Let's do more splits for the amount of interest on other debits.
				PaySplit paySplitNew=new PaySplit();
				paySplitNew.DatePay=date;
				paySplitNew.PatNum=entry.PatNum;
				paySplitNew.ProcDate=entry.Date;
				paySplitNew.ProvNum=payPlanChargeNew.ProvNum;
				if(PrefC.HasClinicsEnabled) {//Clinics
					paySplitNew.ClinicNum=entry.ClinicNum;
				}
				paySplitNew.PayPlanNum=payPlanChargeNew.PayPlanNum;
				paySplitNew.PayNum=payNum;
				paySplitNew.SplitAmt=Math.Min(payPlanChargeNew.Interest,(double)PaymentAmt);
				PaymentAmt=Math.Round(PaymentAmt-(decimal)paySplitNew.SplitAmt,3);
				entry.AmountEnd-=(decimal)paySplitNew.SplitAmt;
				entry.ListPaySplits.Add(paySplitNew);
				listAutoSplits.Add(paySplitNew);
			}
			//Pay off payment plan procedures.
			List<PayPlanCharge> listPayPlanCredits=listPayPlanCharges
				.FindAll(x => x.ChargeType==PayPlanChargeType.Credit && x.PayPlanNum==payPlanNum);
			List<AccountEntry> listPayPlanProcs=new List<AccountEntry>();
			foreach(PayPlanCharge payPlanCredit in listPayPlanCredits) {//For each credit
				foreach(AccountEntry entry in _listAccountCharges) {//Go through account entries
					if(entry.GetType()!=typeof(Procedure)) {//If it's a proc
						continue;
					}
					Procedure proc=(Procedure)entry.Tag;
					if(proc.ProcNum==payPlanCredit.ProcNum) {//See if the proc has the credit's procnum
						listPayPlanProcs.Add(entry);//if so, add it so we then have a list of payplan's procs, and we can also show how much is left due for it
					}
				}
			}
			//Attach splits to proc's procnum, prov, and payplan's payplannum
			foreach(AccountEntry entry in listPayPlanProcs) {
				if(PaymentAmt<=0) {
					break;
				}
				Procedure proc=(Procedure)entry.Tag;
				if(proc.ProcFee<=0) {
					continue;
				}
				//NOTE: Procs will always have charge.AmountEnd of zero at this point, since the payplan Credits have already counteracted them.  
				//We keep track of how much is left on the proc via the proc's ProcFee
				//Create a new paysplit per proc to counteract the balance in the Account Module ledger.
				PaySplit paySplit=new PaySplit();
				paySplit.DatePay=date;
				paySplit.PatNum=proc.PatNum;
				paySplit.ProcDate=entry.Date;
				paySplit.ProvNum=proc.ProvNum;
				paySplit.ClinicNum=proc.ClinicNum;
				paySplit.PayPlanNum=payPlanNum;
				paySplit.PayNum=payNum;				
				paySplit.SplitAmt=Math.Min((double)PaymentAmt,proc.ProcFee);
				PaymentAmt=Math.Round(PaymentAmt-(decimal)paySplit.SplitAmt,3);
				proc.ProcFee-=paySplit.SplitAmt;
				paySplit.ProcNum=proc.ProcNum;
				//Applying the payment to counteract the payplancharge debits in the charge grid of this window.
				//Spread the SplitAmt around to the debits so one doesn't take all the "money" from the proc and go negative.
				decimal payAmt=(decimal)paySplit.SplitAmt;
				foreach(AccountEntry acctEntry in _listAccountCharges) {
					if(payAmt<=0) {
						break;
					}
					if(acctEntry.GetType()!=typeof(PayPlanCharge) || ((PayPlanCharge)acctEntry.Tag).PayPlanNum!=payPlanNum) {
						continue;
					}
					//spread out the amount to the debits here.
					if(acctEntry.AmountEnd < payAmt) {//Use part of payAmt (not all)
						payAmt-=acctEntry.AmountEnd;
						acctEntry.AmountEnd=0;
					}
					else {//Use all of payAmt
						acctEntry.AmountEnd-=payAmt;
						payAmt=0;
					}
				}
				entry.ListPaySplits.Add(paySplit);
				listAutoSplits.Add(paySplit);
			}
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.
		///If payAmt==0, attempt to pay charge in full.</summary>
		private void CreateSplit(AccountEntry charge,decimal payAmt) {
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			if(charge.GetType()==typeof(Procedure)) {//Row selected is a Procedure.
				Procedure proc=(Procedure)charge.Tag;
				split.ProcNum=charge.PriKey;
			}
			else if(charge.GetType()==typeof(PayPlanCharge)) {//Row selected is a PayPlanCharge.  If on PP version 2 it will be a debit type.
				if(PrefC.GetInt(PrefName.PayPlansVersion)==2) {
					//CreatePayPlanSplits takes care of updating PaymentAmt
					CreatePayPlanAutoSplits(((PayPlanCharge)charge.Tag).PayPlanNum,DateTime.Today,PaymentCur.PayNum,ListSplitsCur,
						PayPlanCharges.GetCreditsForPayPlan(((PayPlanCharge)charge.Tag).PayPlanNum));
					return;
				}
				else {
					PayPlanCharge payPlanCharge=(PayPlanCharge)charge.Tag;
					split.PayPlanNum=payPlanCharge.PayPlanNum;
				}
			}
			else if(charge.Tag.GetType()==typeof(Adjustment)) {//Row selected is an Adjustment.
				//Do nothing, nothing to link.
			}
			else {//PaySplits and overpayment refunds.
				//Do nothing, nothing to link.
			}
			decimal chargeAmt=charge.AmountEnd;
			if(Math.Abs(chargeAmt)<Math.Abs(payAmt) || PIn.Decimal(textPayAmt.Text)==0) {//Full payment of charge
				split.SplitAmt=(double)chargeAmt;
				charge.AmountEnd=0;//Reflect payment in underlying datastructure
			}
			else {//Partial payment of charge
				charge.AmountEnd-=payAmt;
				split.SplitAmt=(double)payAmt;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
				split.ClinicNum=charge.ClinicNum;
			}
			PaymentAmt=PaymentAmt-(decimal)split.SplitAmt;
			split.ProvNum=charge.ProvNum;
			split.PatNum=charge.PatNum;
			split.ProcDate=charge.Date;
			split.PayNum=PaymentCur.PayNum;
			charge.ListPaySplits.Add(split);
			ListSplitsCur.Add(split);
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.</summary>
		private void DeleteSelected() {
			for(int i=gridSplits.SelectedIndices.Length-1;i>=0;i--) {
				int idx=gridSplits.SelectedIndices[i];
				PaySplit paySplit=(PaySplit)gridSplits.Rows[idx].Tag;
				if(paySplit.PayPlanNum!=0 && PrefC.GetInt(PrefName.PayPlansVersion)==2) {
					foreach(AccountEntry charge in _listAccountCharges) {
						if(charge.GetType()!=typeof(PayPlanCharge)) {//If the charge is not a payplancharge, we don't care
							continue;
						}
						if(((PayPlanCharge)charge.Tag).PayPlanNum!=paySplit.PayPlanNum) {//If the payplancharge is not for the payplan, we don't care
							continue;
						}
						if(paySplit.SplitAmt<=0) {
							break;
						}
						//It is now a charge for the payplan
						//When a split is deleted, put the money back on the payplan charge
						decimal chargeAmtNew=charge.AmountEnd+(decimal)paySplit.SplitAmt;//Take the current value of the charge and add the split amt to it
						if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//The split has more in it than the debit can take, use only a part of it
							//Find out how much of the split goes into the debit
							decimal debitDifference=charge.AmountStart-charge.AmountEnd;
							paySplit.SplitAmt-=(double)debitDifference;
							charge.AmountEnd=charge.AmountStart;
							PaymentAmt+=debitDifference;
							if(paySplit.ProcNum!=0) { 
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=(double)debitDifference;//Put money back on the procfee so when we add splits later it calculates correctly
							}
						}
						else {
							charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.  Uses full paysplit amount.
							PaymentAmt+=(decimal)paySplit.SplitAmt;
							if(paySplit.ProcNum!=0) {
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=paySplit.SplitAmt;//Put money back on the procfee so when we add splits later it calculates correctly
							}
							paySplit.SplitAmt=0;
						}
						charge.ListPaySplits.Remove(paySplit);
					}
				}
				else { 
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.ListPaySplits.Contains(paySplit)) {
							continue;
						}
						decimal chargeAmtNew=charge.AmountEnd+(decimal)paySplit.SplitAmt;
						if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//Trying to delete an overpayment, just increase charge's amount to the max.
							charge.AmountEnd=charge.AmountStart;
						}
						else {
							charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
						}
						charge.ListPaySplits.Remove(paySplit);
					}
				}
				ListSplitsCur.Remove(paySplit);
				PaymentAmt=PaymentAmt+(decimal)paySplit.SplitAmt;
			}
			FillGridSplits();
		}
		
		///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
		private void gridSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.PaymentEdit,PaymentCur.PayDate,true)) {
				return;
			}
			PaySplit paySplitOld=(PaySplit)gridSplits.Rows[e.Row].Tag;
			PaySplit paySplit=paySplitOld.Copy();
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur);
			FormPSE.PaySplitCur=paySplit;
			if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplit contains all the info we want.  
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
				DeleteSelected();
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					FillGridSplits();
					return;
				}
				UpdateForManualSplit(paySplit);
			}
		}

		///<summary>When a paysplit is selected this method highlights all charges associated with it.</summary>
		private void gridSplits_CellClick(object sender,ODGridClickEventArgs e) {
			HighlightChargesForSplits();
		}

		private void HighlightChargesForSplits() {
			gridCharges.SetSelected(false);
			for(int i=0;i<gridSplits.SelectedIndices.Length;i++) {
				PaySplit paySplit=(PaySplit)gridSplits.Rows[gridSplits.SelectedIndices[i]].Tag;
				for(int j=0;j<gridCharges.Rows.Count;j++) {
					AccountEntry accountEntryCharge=(AccountEntry)gridCharges.Rows[j].Tag;
					if(accountEntryCharge.ListPaySplits.Contains(paySplit)) {
						gridCharges.SetSelected(j,true);
					}
					if(accountEntryCharge.GetType()==typeof(PayPlanCharge)) {
						if(paySplit.PayPlanNum==((PayPlanCharge)accountEntryCharge.Tag).PayPlanNum) {
							gridCharges.SetSelected(j,true);
						}
					}
				}
			}
		}

		///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
		private void gridCharges_CellClick(object sender,ODGridClickEventArgs e) {
			gridSplits.SetSelected(false);
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				AccountEntry accountEntryCharge=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
				for(int j=0;j<gridSplits.Rows.Count;j++) {
					PaySplit paySplit=(PaySplit)gridSplits.Rows[j].Tag;
					if(accountEntryCharge.ListPaySplits.Contains(paySplit)) {
						gridSplits.SetSelected(j,true);
					}
					if(accountEntryCharge.GetType()==typeof(PayPlanCharge)) {
						if(paySplit.PayPlanNum==((PayPlanCharge)accountEntryCharge.Tag).PayPlanNum) {
							gridSplits.SetSelected(j,true);
						}
					}
				}
			}
		}

		///<summary>Creates a manual paysplit.</summary>
		private void butAdd_Click(object sender,EventArgs e) {
			PaySplit paySplit=new PaySplit();
			paySplit.SplitAmt=0;
			paySplit.DateEntry=DateTime.Today;
			paySplit.DatePay=DateTime.Today;
			paySplit.PayNum=PaymentCur.PayNum;
			paySplit.ProvNum=Patients.GetProvNum(PatCur);
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur);
			FormPSE.PaySplitCur=paySplit;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				UpdateForManualSplit(paySplit);
			}
		}

		///<summary>Updates the underlying data structures when a manual split is created or edited.</summary>
		private void UpdateForManualSplit(PaySplit paySplit) {
			//Find the charge row for this new split.
			ListSplitsCur.Add(paySplit);
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(paySplit.PayPlanNum);
			List<long> listPayPlanChargeNums=new List<long>();
			for(int j=0;j<listCharges.Count;j++) {
				listPayPlanChargeNums.Add(listCharges[j].PayPlanChargeNum);
			}
			//Locate a charge to apply the credit to, if a reasonable match exists.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountEnd==0) {
					continue;
				}
				bool isMatchFound=false;
				if(charge.GetType()==typeof(Procedure) && charge.PriKey==paySplit.ProcNum) {//New Split is for this proc
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(Adjustment) //New split is for this adjust
						&& paySplit.ProcNum==0 && paySplit.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
						&& paySplit.ProcDate==charge.Date
						&& paySplit.ProvNum==charge.ProvNum)
				{
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PayPlanCharge) && listPayPlanChargeNums.Contains(charge.PriKey)) {//Payplancharge of same payplan as paysplit
					isMatchFound=true;
				}				
				if(isMatchFound) {
					decimal amtOwed=charge.AmountEnd;
					if(Math.Abs(amtOwed)<Math.Abs((decimal)paySplit.SplitAmt)) {//Partial payment
						charge.AmountEnd=0;//Reflect payment in underlying datastructure
					}
					else {//Full payment
						charge.AmountEnd=amtOwed-(decimal)paySplit.SplitAmt;
					}
					charge.ListPaySplits.Add(paySplit);
				}
				//If none of these, it's unattached to the best of our knowledge.				
			}			
			FillGridSplits();//Fills charge grid too.
		}

		private void checkShowPaid_CheckedChanged(object sender,EventArgs e) {
			FillGridSplits();
		}

		///<summary>Creates paysplits for selected charges if there is enough payment left.</summary>
		private void butAddSplit_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				AccountEntry charge=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
				CreateSplit(charge,PaymentAmt);
			}
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>Creates paysplits after allowing the user to enter in a custom amount to pay for each selected charge.</summary>
		private void butAddPartial_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				string chargeDescript="";
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					chargeDescript=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text;
				}
				else {
					chargeDescript=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[3].Text;
				}
				FormAmountEdit FormAE=new FormAmountEdit(chargeDescript);
				string remainingChargeAmt=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[5].Text;
				FormAE.Amount=PIn.Decimal(remainingChargeAmt);
				FormAE.ShowDialog();
				if(FormAE.DialogResult==DialogResult.OK) {
					decimal amount=FormAE.Amount;
					if(amount!=0) {
						AccountEntry charge=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
						CreateSplit(charge,amount);
					}
				}
			}
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>Deletes all paysplits.</summary>
		private void butClearAll_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
		}

		///<summary>Deletes selected paysplits.</summary>
		private void butDeleteSplit_Click(object sender,EventArgs e) {
			DeleteSelected();
		}

		private void butOK_Click(object sender,EventArgs e) {
			decimal payAmt=PIn.Decimal(textPayAmt.Text);
			decimal splitTotal=PIn.Decimal(textSplitTotal.Text);
			//Create an unallocated split if there is any remaining in the payment amount.
			//Only create the unallocated payment if the sum of the splits is less than the whole payment amount.
			if(Math.Abs(payAmt)>Math.Abs(splitTotal) && payAmt!=0) {
				PaySplit split=new PaySplit();
				split.SplitAmt=(double)(payAmt-splitTotal);
				PaymentAmt=0;
				split.DatePay=PaymentCur.DateEntry;
				split.PatNum=PaymentCur.PatNum;
				split.ProcDate=PaymentCur.PayDate;
				split.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=PaymentCur.PayNum;
				ListSplitsCur.Add(split);
				MsgBox.Show(this,"Payment split total does not equal payment amount.  An unallocated payment split has been added, please check for correctness.");
				FillGridSplits();
				return;
			}
			else if(payAmt==0) {//If they have a payment amount of 0 set the payment's PayAmt to what the split total is.
				AmtTotal=splitTotal;
			}
			else if(payAmt==splitTotal) {
				//Do nothing.
			}
			else {
				MsgBox.Show(this,"Payment amount cannot be less than the total split value.");
				return;
			}
			for(int i=ListSplitsCur.Count-1;i>=0;i--) {
				if(ListSplitsCur[i].SplitAmt==0) {
					ListSplitsCur.RemoveAt(i);//We don't want any zero splits.  They were there just for display purposes.
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//No paysplits were inserted.  Just close the window.
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private class AccountEntry {

			private static long AccountEntryAutoIncrementValue=1;

			///<summary>No matter which constructor is used, the AccountEntryNum will be unique and automatically assigned.</summary>
			public long AccountEntryNum=(AccountEntryAutoIncrementValue++);
			//Read only data.  Do not modify, or else the historic information will be changed.
			public object Tag;
			public DateTime Date;
			public long PriKey;
			public long ProvNum;
			public long ClinicNum;
			public long PatNum;
			public decimal AmountOriginal;
			//Variables below will be changed as needed.
			public decimal AmountStart;
			public decimal AmountEnd;
			public List<PaySplit> ListPaySplits=new List<PaySplit>();//List of paysplits for this charge.

			public new Type GetType() {
				return Tag.GetType();
			}

			public AccountEntry(PayPlanCharge payPlanCharge) {
				Tag=payPlanCharge;
				Date=payPlanCharge.ChargeDate;
				PriKey=payPlanCharge.PayPlanChargeNum;
				AmountOriginal=(decimal)payPlanCharge.Principal+(decimal)payPlanCharge.Interest;
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=payPlanCharge.ProvNum;
				ClinicNum=payPlanCharge.ClinicNum;
				PatNum=payPlanCharge.PatNum;
			}

			///<summary>Turns negative adjustments positive.</summary>
			public AccountEntry(Adjustment adjustment) {
				Tag=adjustment;
				Date=adjustment.AdjDate;
				PriKey=adjustment.AdjNum;
				AmountOriginal=(decimal)adjustment.AdjAmt;
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=adjustment.ProvNum;
				ClinicNum=adjustment.ClinicNum;
				PatNum=adjustment.PatNum;
			}

			public AccountEntry(Procedure proc) {
				Tag=proc;
				Date=proc.ProcDate;
				PriKey=proc.ProcNum;
				AmountOriginal=(decimal)proc.ProcFee;
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=proc.ProvNum;
				ClinicNum=proc.ClinicNum;
				PatNum=proc.PatNum;
			}

		}

	}
}