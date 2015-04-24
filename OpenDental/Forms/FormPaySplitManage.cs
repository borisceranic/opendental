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
		///<summary>List of current account credits for the family.</summary>
		private List<AccountEntry> _listAccountCredits;
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
			_listAccountCharges=new List<AccountEntry>();
			_payAvailableCur=PaymentAmt;
			textPayAmt.Text=PaymentAmt.ToString("f");
			List<long> listPatNums=new List<long>();
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
			//Over the next 5 regions, we will do the following:
			//Create a list of account charges
			//Create a list of account credits
			//Explicitly link any of the credits to their corresponding charges if a link can be made. (ie. PaySplit.ProcNum to a Procedure.ProcNum)
			//Implicitly link any of the remaining credits to the non-zero charges FIFO by date.
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
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
			_listAccountCharges.Sort(AccountEntrySort);
			#endregion Construct List of Charges
			#region Construct List of Credits
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			_listAccountCredits=new List<AccountEntry>();
			for(int i=0;i<listNegAdjustments.Count;i++) {
				_listAccountCredits.Add(new AccountEntry(listNegAdjustments[i]));
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				_listAccountCredits.Add(new AccountEntry(listPaySplits[i]));
			}
			for(int i=0;i<listPayments.Count;i++) {				
				_listAccountCredits.Add(new AccountEntry(listPayments[i]));
			}
			for(int i=0;i<listInsPayAsTotal.Count;i++) {			
				_listAccountCredits.Add(new AccountEntry(listInsPayAsTotal[i]));
			}
			for(int i=0;i<listPayPlans.Count;i++) {
				_listAccountCredits.Add(new AccountEntry(listPayPlans[i]));
			}
			_listAccountCredits.Sort(AccountEntrySort);
			#endregion Construct List of Credits
			#region Explicitly Link Credits
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				for(int j=0;j<_listAccountCredits.Count;j++) {
					AccountEntry credit=_listAccountCredits[j];
					if(credit.GetType()==typeof(PaySplit)) {//Credit is a paysplit
						PaySplit paySplit=(PaySplit)credit.Tag;
						if(charge.GetType()==typeof(Procedure) && paySplit.ProcNum==charge.PriKey) {//Charge is a Procedure, and paysplit is attached to it.
							ApplyCredit(charge,credit);							
							break;
						}
						else if(charge.GetType()==typeof(PayPlanCharge)) {//Charge is a payplancharge, paysplit may or may not be attached to its payplan.
							PayPlanCharge payPlanCharge=(PayPlanCharge)charge.Tag;
							if(payPlanCharge.PayPlanNum==paySplit.PayPlanNum && charge.AmountCur>0) {
								//PaySplit was made for the same PayPlan and there's some left over in this charge.  Make an attribution.
								ApplyCredit(charge,credit);
								break;
							}
						}
					}
					else if(credit.GetType()==typeof(Adjustment)) {//Credit is an adjustment
						Adjustment adjustment=(Adjustment)credit.Tag;
						if(adjustment.ProcNum==charge.PriKey) {//Adjustment is attached to this charge
							ApplyCredit(charge,credit);
							break;
						}
					}
				}
			}
			#endregion Explicitly Link Credits
			#region Cleanup Negative Transactions
			//For now we do not support negative charges nor negative credits for implicit linking and auto-splits.
			for(int i=_listAccountCredits.Count-1;i>=0;i--) {
				if(_listAccountCredits[i].AmountCur<0) {
					_listAccountCredits.RemoveAt(i);
				}
			}
			for(int i=_listAccountCharges.Count-1;i>=0;i--) {
				if(_listAccountCharges[i].AmountCur<0) {
					_listAccountCharges.RemoveAt(i);
				}
			}
			#endregion Cleanup Negative Transactions
			#region Implicitly Link Credits
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				for(int j=0;j<_listAccountCredits.Count;j++) {
					AccountEntry credit=_listAccountCredits[j];
					if(charge.AmountCur<=0 || credit.AmountCur<=0) {//The charge.AmountCur can change as we look through the credits.
						continue;//The credit has already been used.  May have been set to 0 for a previous owed item.
					}
					if(credit.GetType()==typeof(PayPlan) && charge.GetType()==typeof(PayPlanCharge)) {
						//These are skipped because payplancharges are paid by splits and adjustments only.
						//The payplan creates an immediate credit for the patient account, but not the payment plan (the payment plan is credited over time).
						continue;
					}
					ApplyCredit(charge,credit);
				}
			}
			#endregion Implicitly Link Credits
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
				AccountEntry credit=new AccountEntry(split);
				credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
				_listAccountCredits.Add(credit);
				listAutoSplits.Add(split);
			}
			#endregion Auto-Spit Current Payment
			return listAutoSplits;
		}

		private void ApplyCredit(AccountEntry charge,AccountEntry credit) {
			if(charge.AmountCur<credit.AmountCur) {
				credit.AmountCur-=charge.AmountCur;
				charge.AmountCur=0;
			}
			else {
				charge.AmountCur-=credit.AmountCur;
				credit.AmountCur=0;
			}
			credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.</summary>
		private double CreateSplit(ODGridRow row,double payAmt,bool strict) {
			double chargeAmt=PIn.Double(row.Cells[5].Text);
			if((chargeAmt<=0 || payAmt<=0) && strict) {//They selected a paid row or the amount they want to pay with is empty, skip it.
				return payAmt;
			}
			if(payAmt>_payAvailableCur) {//They tried to enter in a partial payment for more than was remaining on the payment. Reduce it down.
				payAmt=_payAvailableCur;
			}
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			AccountEntry charge=null;
			if(row.Tag.GetType()==typeof(Procedure)) {//Row selected is a Procedure.
				Procedure proc=(Procedure)row.Tag;
				for(int j=0;j<_listAccountCharges.Count;j++) {//Go through charges and find the charge entry for this Procedure.
					if(_listAccountCharges[j].PriKey!=proc.ProcNum || _listAccountCharges[j].GetType()!=typeof(Procedure)) {
						continue;
					}
					else {
						charge=_listAccountCharges[j];
						split.ProcNum=charge.PriKey;
						break;
					}					
				}
			}
			else if(row.Tag.GetType()==typeof(PayPlanCharge)) {//Row selected is a PayPlanCharge.
				PayPlanCharge payPlanCharge=(PayPlanCharge)row.Tag;
				for(int j=0;j<_listAccountCharges.Count;j++) {//Go through charges and find the charge entry for this PayPlanCharge.
					if(_listAccountCharges[j].PriKey!=payPlanCharge.PayPlanChargeNum || _listAccountCharges[j].GetType()!=typeof(PayPlanCharge)) {
						continue;
					}
					else {
						charge=_listAccountCharges[j];
						split.PayPlanNum=payPlanCharge.PayPlanNum;
						break;
					}					
				}
			}
			else if(row.Tag.GetType()==typeof(Adjustment)) {//Row selected is an Adjustment.
				Adjustment adjust=(Adjustment)row.Tag;
				for(int j=0;j<_listAccountCharges.Count;j++) {//Go through charges to find the charge entry for this Adjustment.
					if(_listAccountCharges[j].PriKey!=adjust.AdjNum || _listAccountCharges[j].GetType()!=typeof(Adjustment)) {
						continue;
					}
					else {
						charge=_listAccountCharges[j];
						break;
					}
				}
			}
			double amtOwed=charge.AmountCur;
			if(amtOwed<payAmt) {//Partial payment
				payAmt-=amtOwed;
				charge.AmountCur=0;//Reflect payment in underlying datastructure
				split.SplitAmt=amtOwed;
			}
			else {//Full payment
				charge.AmountCur=amtOwed-payAmt;
				split.SplitAmt=payAmt;
				payAmt=0;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
				split.ClinicNum=charge.ClinicNum;
			}
			split.ProvNum=charge.ProvNum;
			split.PatNum=charge.PatNum;
			split.ProcDate=charge.Date;
			split.PayNum=PaymentCur.PayNum;
			AccountEntry credit=new AccountEntry(split);
			credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
			_listAccountCredits.Add(credit);
			ListSplitsCur.Add(split);
			if(strict) {
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+split.SplitAmt).ToString("f");
				_payAvailableCur-=split.SplitAmt;
			}
			else {
				textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+payAmt).ToString("f");
				_payAvailableCur-=payAmt;
			}
			return payAmt;
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
				List<long> listChargeAccountEntryNums=new List<long>();
				for(int j=0;j<_listAccountCredits.Count;j++) {
					AccountEntry credit=_listAccountCredits[j];
					if(credit.GetType()==typeof(PaySplit) && credit.PriKey==split.SplitNum) {//Found the split being deleted, find what it's attached to.
						listChargeAccountEntryNums=credit.ListChargeAccountEntryNums;//Typically will only be one entry unless it was implicitly linked to several charges.
						_listAccountCredits.RemoveAt(j);
						break;
					}
				}
				for(int j=0;j<_listAccountCharges.Count;j++) {
					AccountEntry charge=_listAccountCharges[j];
					if(!listChargeAccountEntryNums.Contains(charge.AccountEntryNum))	{
						continue;
					}
					double chargeAmtNew=charge.AmountCur+split.SplitAmt;
					if(chargeAmtNew>charge.AmountOriginal) {//Trying to delete an overpayment, just increase charge's amount to the max.
						charge.AmountCur=charge.AmountOriginal;
					}
					else {
						charge.AmountCur+=split.SplitAmt;//Give the money back to the charge so it will display.
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
			if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplitNew contains all the info we want.  
				double splitDiff=paySplitNew.SplitAmt-paySplitOld.SplitAmt;
				if(_payAvailableCur<splitDiff) {//New amount is greater, and it's more than the payment had left.
					MsgBox.Show(this,"Entered amount for split is greater than amount remaining for this payment.");
					return;
				}
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
				DeleteSelected();
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					FillChargeGrid();
					return;
				}
				UpdateForManualSplit(paySplitNew);
			}
			FillChargeGrid();
		}

		///<summary>Creates a manual paysplit.</summary>
		private void butCreateManual_Click(object sender,EventArgs e) {
			PaySplit paySplitNew=new PaySplit();
			paySplitNew.SplitAmt=_payAvailableCur;
			paySplitNew.DateEntry=DateTime.Today;
			paySplitNew.DatePay=DateTime.Today;
			paySplitNew.PayNum=PaymentCur.PayNum;
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur);
			FormPSE.PaySplitCur=paySplitNew;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				if(_payAvailableCur-paySplitNew.SplitAmt<0) {//New amount is greater, and it's more than the payment had left.
					MsgBox.Show(this,"Entered amount for split is greater than amount remaining for this payment.");
					return;
				}
				UpdateForManualSplit(paySplitNew);
			}
			FillChargeGrid();
		}

		///<summary>Updates the underlying data structures when a manual split is created or edited.</summary>
		private void UpdateForManualSplit(PaySplit paySplit) {
			//Find the charge row for this new split.
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(paySplit.PayPlanNum);
			List<long> listPayPlanChargeNums=new List<long>();
			for(int j=0;j<listCharges.Count;j++) {
				listPayPlanChargeNums.Add(listCharges[j].PayPlanChargeNum);
			}
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				if(paySplit.ProcNum!=0 && charge.PriKey==paySplit.ProcNum) {//New Split is for this proc
					double amtOwed=charge.AmountCur;
					if(amtOwed<paySplit.SplitAmt) {//Partial payment
						charge.AmountCur=0;//Reflect payment in underlying datastructure
					}
					else {//Full payment
						charge.AmountCur=amtOwed-paySplit.SplitAmt;
					}
					AccountEntry credit=new AccountEntry(paySplit);
					credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
					_listAccountCredits.Add(credit);
 					break;
				}
				else if(charge.GetType()==typeof(Adjustment) //New split is for this adjust
						&& paySplit.ProcNum==0 && paySplit.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
						&& paySplit.ProcDate==charge.Date
						&& paySplit.ProvNum==charge.ProvNum)
				{
					double amtOwed=charge.AmountCur;
					if(amtOwed<paySplit.SplitAmt) {//Partial payment
						charge.AmountCur=0;//Reflect payment in underlying datastructure
					}
					else {//Full payment
						charge.AmountCur=amtOwed-paySplit.SplitAmt;
					}
					AccountEntry credit=new AccountEntry(paySplit);
					credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
					_listAccountCredits.Add(credit); 						
					break;
				}
				else if(listPayPlanChargeNums.Contains(charge.PriKey)//New split is for this PayPlanCharge
						&& charge.GetType()==typeof(PayPlanCharge))
				{
					double amtOwed=charge.AmountCur;
					if(amtOwed<paySplit.SplitAmt) {//Partial payment
						charge.AmountCur=0;//Reflect payment in underlying datastructure
					}
					else {//Full payment
						charge.AmountCur=amtOwed-paySplit.SplitAmt;
					}
					AccountEntry credit=new AccountEntry(paySplit);
					credit.ListChargeAccountEntryNums.Add(charge.AccountEntryNum);
					_listAccountCredits.Add(credit);
 					break;
				}
				//If none of these, it's unattached to the best of our knowledge.
			}
			_payAvailableCur-=paySplit.SplitAmt;
			textSplitAmt.Text=(PIn.Double(textSplitAmt.Text)+paySplit.SplitAmt).ToString("f");
			ListSplitsCur.Add(paySplit);
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
			ODGridRow row;
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				row=gridCharges.Rows[gridCharges.SelectedIndices[i]];
				amt=CreateSplit(row,amt,true);
			}
			FillChargeGrid();
		}

		///<summary>Creates paysplits after allowing the user to enter in a custom amount to pay for each selected charge.</summary>
		private void butCreatePartial_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				if(_payAvailableCur<=0) {//Only make splits if we have some available.
					continue;
				}
				string chargeAmt=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text;
				FormAmountEdit FormAE=new FormAmountEdit(chargeAmt);
				string remainingAmt=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[5].Text;
				FormAE.Amount=PIn.Double(remainingAmt);
				FormAE.ShowDialog();
				if(FormAE.DialogResult==DialogResult.OK) {
					double amount=FormAE.Amount;
					if(amount>0) {
						ODGridRow row=gridCharges.Rows[gridCharges.SelectedIndices[i]];
						amount=CreateSplit(row,amount,true);
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
		private int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private class AccountEntry {

			private static long AccountEntryAutoIncrementValue=1;

			///<summary>No matter which constructor is used, the AccountEntryNum will be unique and automatically assigned.</summary>
			public long AccountEntryNum=(AccountEntryAutoIncrementValue++);
			public object Tag;
			public DateTime Date;
			public long PriKey;
			public double AmountOriginal;
			public double AmountCur;
			public long ProvNum;
			public long ClinicNum;
			public long PatNum;
			public List<long> ListChargeAccountEntryNums;//List of PaySplitNum so we can have multiple splits for the same charge.

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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
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
				ListChargeAccountEntryNums=new List<long>();
			}

		}

	}
}