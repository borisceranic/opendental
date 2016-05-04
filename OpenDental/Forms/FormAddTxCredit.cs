using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	///<summary>This form will not be available to those who are still using PayPlans Version 1.</summary>
	public partial class FormAddTxCredit:ODForm {
		private Family _famCur;
		private Patient _patCur;
		private PayPlan _payPlanCur;
		///<summary>Pass in the selected charge when the editing an existing charge.  Must be set if IsNew is false.</summary>
		public PayPlanCharge ChargeCur;
		public bool IsNew;
		public bool IsDeleted;
		private List<long> _listPatNums;
		private List<Adjustment> _listAdjustments;
		private List<Procedure> _listProcs;
		private List<PayPlanCharge> _listPayPlanCharges;
		private List<ClaimProc> _listInsPayAsTotal;
		private List<Payment> _listPayments;
		private List<PaySplit> _listPaySplits;
		private List<AccountEntry>_listAccountCharges;
		private decimal _accountCredits;
		///<summary>Payment plan credits for the current payment plan.  Must be passed-in.</summary>
		public List<PayPlanCharge> ListPayPlanCreditsCur;
		private Procedure _selectedProc;
		///<summary>For getting insurace estimates for TP procedures and completed procedures whose claim hasn't been received.</summary>
		private List<ClaimProc> _listClaimProcs;

		public FormAddTxCredit(PayPlan payPlanCur) {
			_payPlanCur=payPlanCur;
			_patCur=Patients.GetPat(payPlanCur.PatNum);
			_famCur=Patients.GetFamily(_payPlanCur.PatNum);
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAddTxPayment_Load(object sender,EventArgs e) {
			if(IsNew) {
				ChargeCur=new PayPlanCharge();
			}
			else {
				_selectedProc=Procedures.GetOneProc(ChargeCur.ProcNum,false);
				textAmt.Text=ChargeCur.Principal.ToString();
				if(_selectedProc.ProcStatus==ProcStat.TP) {
					textDate.Text="";
					textDate.ReadOnly=true;
				}
				else {
					textDate.Text=ChargeCur.ChargeDate.ToShortDateString();
				}
				textNote.Text=ChargeCur.Note;
			}
			_listPatNums=new List<long>();
			for(int i=0;i < _famCur.ListPats.Length;i++) {
				_listPatNums.Add(_famCur.ListPats[i].PatNum);
			}
			_listAdjustments=Adjustments.GetAdjustForPats(_listPatNums);
			_listProcs=Procedures.GetCompAndTpForPats(_listPatNums);
			List<PayPlan> listPayPlans=PayPlans.GetForPats(_listPatNums,_patCur.PatNum);//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			_listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0) {
				//get all current payplan charges for plans already on the patient, excluding the current one.
				_listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans,_patCur.PatNum)//Does not get charges for the future.
					.Where(x => !(x.PayPlanNum==_payPlanCur.PayPlanNum && x.ChargeType==PayPlanChargeType.Credit)).ToList(); //do not get credits for current payplan
			}
			_listPaySplits=PaySplits.GetForPats(_listPatNums);//Might contain payplan payments.
			_listPayments=Payments.GetNonSplitForPats(_listPatNums);
			_listInsPayAsTotal=ClaimProcs.GetByTotForPats(_listPatNums);//Claimprocs paid as total, might contain ins payplan payments.
			_listClaimProcs=ClaimProcs.GetForProcs(_listProcs.Select(x => x.ProcNum).ToList())
				.FindAll(x => (x.Status==ClaimProcStatus.Estimate || x.Status == ClaimProcStatus.NotReceived)); //only get claimprocs that haven't been received for those procedures.
			FillGrid();
		}

		private void FillGrid() {
			_listAccountCharges=GetAccountCharges();
			_accountCredits=GetAccountCredits();
			LinkChargesToCredits();
			//Fill right-hand grid with all the charges, filtered based on checkbox.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),65);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Stat"),30,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Procedure"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Fee"),60,HorizontalAlignment.Left);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt Rem"),60,HorizontalAlignment.Left);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.AmountEnd==0 || entryCharge.GetType()==typeof(Adjustment) || entryCharge.GetType()==typeof(PayPlanCharge)) {
					continue;
				}
				row=new ODGridRow();
				row.Tag=_listAccountCharges[i];
				Procedure proc=(Procedure)entryCharge.Tag;
				row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
				row.Cells.Add(_famCur.GetNameInFamFirst(entryCharge.PatNum));
				if(proc.ProcStatus==ProcStat.TP) {
					row.Cells.Add("TP");
				}
				else {
					row.Cells.Add("C");
				}
				row.Cells.Add(ProcedureCodes.GetStringProcCode(proc.CodeNum)+": "+Procedures.ConvertProcToString(proc.CodeNum,proc.Surf,proc.ToothNum,false));
				row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount Remaining
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Gets all charges for the current patient. Returns a list of AccountEntries.</summary>
		private List<AccountEntry> GetAccountCharges() {
			List<AccountEntry> listCharges=new List<AccountEntry>();
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
					if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit) {
						listCharges.Add(new AccountEntry(_listPayPlanCharges[i]));
					}
				}
			for(int i=0;i<_listAdjustments.Count;i++) {
				if(_listAdjustments[i].AdjAmt>0 && _listAdjustments[i].ProcNum==0) {
					listCharges.Add(new AccountEntry(_listAdjustments[i]));
				}
			}
			for(int i=0;i<_listProcs.Count;i++) {
				listCharges.Add(new AccountEntry(_listProcs[i]));
			}
			listCharges.Sort(AccountEntrySort);
			return listCharges;
		}

		///<summary>Gets all unattached credits for the current patient.  Returns the summed credits as a decimal.</summary>
		private decimal GetAccountCredits() {
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			decimal creditTotal=0;
			for(int i=0;i<_listAdjustments.Count;i++) {
				if(_listAdjustments[i].AdjAmt<0) {
					creditTotal-=(decimal)_listAdjustments[i].AdjAmt;
				}
			}
			for(int i=0;i<_listPaySplits.Count;i++) {
				creditTotal+=(decimal)_listPaySplits[i].SplitAmt;
			}
			for(int i=0;i<_listPayments.Count;i++) {
				creditTotal+=(decimal)_listPayments[i].PayAmt;
			}
			for(int i=0;i<_listInsPayAsTotal.Count;i++) {
				creditTotal+=(decimal)_listInsPayAsTotal[i].InsPayAmt;
			}
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
				if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Credit) {
					creditTotal+=(decimal)_listPayPlanCharges[i].Principal;
				}
			}
			for(int i = 0;i<ListPayPlanCreditsCur.Count;i++) {
				if(ListPayPlanCreditsCur[i].ChargeType==PayPlanChargeType.Credit) {
					creditTotal+=(decimal)ListPayPlanCreditsCur[i].Principal;
				}
			}
			for(int i = 0;i<_listClaimProcs.Count;i++) {
				if(_listClaimProcs[i].InsEstTotalOverride!=-1) {
					creditTotal+=(decimal)_listClaimProcs[i].InsEstTotalOverride;
				}
				else {
					creditTotal+=(decimal)_listClaimProcs[i].InsEstTotal;
				}
			}
			return creditTotal;
		}

		///<summary>Links charges to credits explicitly based on FKs first, then implicitly based on Date.</summary>
		private void LinkChargesToCredits() {
			#region Explicit
			for(int i = 0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				for(int j = 0;j<_listPaySplits.Count;j++) {
					PaySplit paySplit=_listPaySplits[j];
					decimal paySplitAmt=(decimal)paySplit.SplitAmt;
					if(charge.GetType()==typeof(Procedure) && paySplit.ProcNum==charge.PriKey) {
						charge.ListPaySplits.Add(paySplit);
						charge.AmountEnd-=paySplitAmt;
						_accountCredits-=paySplitAmt;
						charge.AmountStart-=paySplitAmt;
					}
					else if(charge.GetType()==typeof(PayPlanCharge) && ((PayPlanCharge)charge.Tag).PayPlanNum==paySplit.PayPlanNum && charge.AmountEnd>0 && paySplit.SplitAmt>0) {
						charge.AmountEnd-=paySplitAmt;
						_accountCredits-=paySplitAmt;
					}
				}
				for(int j = 0;j<_listAdjustments.Count;j++) {
					Adjustment adjustment=_listAdjustments[j];
					decimal adjustmentAmt=(decimal)adjustment.AdjAmt;
					if(charge.GetType()==typeof(Procedure) && adjustment.ProcNum==charge.PriKey) {
						charge.AmountEnd+=adjustmentAmt;
						if(adjustment.AdjAmt<0) {
							_accountCredits+=adjustmentAmt;
						}
						charge.AmountStart+=adjustmentAmt;
						//If the adjustment is attached to a procedure decrease the procedure's amountoriginal so we know what it was just prior to autosplitting.
					}
				}
				for(int j = 0;j < _listPayPlanCharges.Count;j++) {
					PayPlanCharge payPlanCharge=_listPayPlanCharges[j];
					if(charge.GetType()==typeof(Procedure) && payPlanCharge.ProcNum == charge.PriKey) //payPlanCharge.ProcNum will only be set for credits.
						{
						charge.AmountEnd-=(decimal)payPlanCharge.Principal;
						charge.AmountStart-=(decimal)payPlanCharge.Principal;
						_accountCredits-=(decimal)payPlanCharge.Principal;
					}
				}
				for(int j = 0;j < ListPayPlanCreditsCur.Count;j++) {
					PayPlanCharge payPlanCharge=ListPayPlanCreditsCur[j];
					if(charge.GetType()==typeof(Procedure) && payPlanCharge.ProcNum == charge.PriKey) {
						charge.AmountEnd-=(decimal)payPlanCharge.Principal;
						charge.AmountStart-=(decimal)payPlanCharge.Principal;
						_accountCredits-=(decimal)payPlanCharge.Principal;
					}
				}
				for(int j = 0;j < _listClaimProcs.Count;j++) {
					ClaimProc claimProcCur=_listClaimProcs[j];
					if(charge.GetType()==typeof(Procedure) && claimProcCur.ProcNum == charge.PriKey) {
						if(claimProcCur.InsEstTotalOverride!=-1) {
							charge.AmountEnd-=(decimal)claimProcCur.InsEstTotalOverride;
							charge.AmountStart-=(decimal)claimProcCur.InsEstTotalOverride;
							_accountCredits-=(decimal)claimProcCur.InsEstTotalOverride;
						}
						else {
							charge.AmountEnd-=(decimal)claimProcCur.InsEstTotal;
							charge.AmountStart-=(decimal)claimProcCur.InsEstTotal;
							_accountCredits-=(decimal)claimProcCur.InsEstTotal;
						}
					}
				}
			}
			#endregion Explicit
			//Apply negative charges as if they're credits.
			for(int i=0;i < _listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.AmountEnd<0) {
					_accountCredits-=entryCharge.AmountEnd;
					entryCharge.AmountEnd=0;
				}
			}
			#region Implicit
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count && _accountCredits>0;i++) {
				AccountEntry charge=_listAccountCharges[i];
				decimal amt=Math.Min(charge.AmountEnd,_accountCredits);
				charge.AmountEnd-=amt;
				_accountCredits-=amt;
				charge.AmountStart-=amt;//Decrease amount original for the charge so we know what it was just prior to when the autosplits were made.
			}
			#endregion Implicit
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			_selectedProc=(Procedure)((AccountEntry)gridMain.Rows[e.Row].Tag).Tag;
			textAmt.Text=POut.Decimal(((AccountEntry)gridMain.Rows[e.Row].Tag).AmountEnd);
			if(_selectedProc.ProcStatus==ProcStat.TP) {
				textDate.Text="";//Will get set to MaxValue on OK Click
				textDate.ReadOnly=true; //can't edit the date on TP'd procedures.
				textNote.Text=POut.String("(TP)"+ProcedureCodes.GetStringProcCode(_selectedProc.CodeNum)+": "
					+Procedures.ConvertProcToString(_selectedProc.CodeNum,_selectedProc.Surf,_selectedProc.ToothNum,false));
			}
			else {
				textDate.Text=((AccountEntry)gridMain.Rows[e.Row].Tag).Date.ToShortDateString();
				textDate.ReadOnly=false; 
				textNote.Text=POut.String(ProcedureCodes.GetStringProcCode(_selectedProc.CodeNum)+": "
					+Procedures.ConvertProcToString(_selectedProc.CodeNum,_selectedProc.Surf,_selectedProc.ToothNum,false));
			}
			Procedure proc=(Procedure)((AccountEntry)gridMain.Rows[e.Row].Tag).Tag;
			textNote.Text="Proc: "+POut.String(Procedures.GetDescription(proc));
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete this payment plan charge?  This cannot be undone.")) {
				return;
			}
			IsDeleted=true;
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			ChargeCur.ChargeDate=PIn.Date(_selectedProc.ProcStatus==ProcStat.TP ? DateTime.MaxValue.ToShortDateString() : textDate.Text);
			ChargeCur.ChargeType=PayPlanChargeType.Credit; //always a credit.
			ChargeCur.Interest=0; //credits never have interest
			ChargeCur.Note=PIn.String(textNote.Text);
			ChargeCur.Principal=PIn.Double(textAmt.Text);
			ChargeCur.ProcNum=_selectedProc.ProcNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		///<summary>Same as private class in PaySplitManager.</summary>
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
