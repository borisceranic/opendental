using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans835Edit:Form {

		public string TranSetId835;
		public string MessageText835;
		private X835 _x835;
		private decimal _claimInsPaidSum;
		private decimal _provAdjAmtSum;

		public FormEtrans835Edit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans835Edit_Load(object sender,EventArgs e) {
			_x835=new X835(MessageText835,TranSetId835);
			FillAll();
		}

		private void FormEtrans835Edit_Resize(object sender,EventArgs e) {
			//This funciton is called before FormEtrans835Edit_Load() when using ShowDialog(). Therefore, x835 is null the first time FormEtrans835Edit_Resize() is called.
			if(_x835==null) {
				return;
			}
			gridProviderAdjustments.Width=butClose.Right-gridProviderAdjustments.Left;
			FillProviderAdjustmentDetails();//Because the grid columns change size depending on the form size.
			gridClaimDetails.Width=gridProviderAdjustments.Width;
			gridClaimDetails.Height=labelPaymentAmount.Top-5-gridClaimDetails.Top;
			FillClaimDetails();//Because the grid columns change size depending on the form size.
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information in this form.</summary>
		private void FillAll() {
			//*835 has 3 parts: Table 1 (header), Table 2 (claim level details, one CLP segment for each claim), and Table 3 (PLB: provider/check level details).
			FillHeader();//Table 1
			FillClaimDetails();//Table 2
			FillProviderAdjustmentDetails();//Table 3
			FillFooter();
			//The following concepts should each be addressed as development progresses.
			//*837 CLM01 -> 835 CLP01 (even for split claims)
			//*Advance payments (pg. 23): in PLB segment with adjustment reason code PI.  Can be yearly or monthly.  We need to find a way to pull provider level adjustments into a deposit.
			//*Bundled procs (pg. 27): have the original proc listed in SV06. Use Line Item Control Number to identify the original proc line.
			//*Predetermination (pg. 28): Identified by claim status code 25 in CLP02. Claim adjustment reason code is 101.
			//*Claim reversals (pg. 30): Identified by code 22 in CLP02. The original claim adjustment codes can be found in CAS01 to negate the original claim.
			//Use CLP07 to identify the original claim, or if different, get the original ref num from REF02 of 2040REF*F8.
			//*Interest and Prompt Payment Discounts (pg. 31): Located in AMT segments with qualifiers I (interest) and D8 (discount). Found at claim and provider/check level.
			//Not part of AR, but part of deposit. Handle this situation by using claimprocs with 2 new status, one for interest and one for discount? Would allow reports, deposits, and claim checks to work as is.
			//*Capitation and related payments or adjustments (pg. 34 & 52): Not many of our customers use capitation, so this will probably be our last concern.
			//*Claim splits (pg. 36): MIA or MOA segments will exist to indicate the claim was split.
			//*Service Line Splits (pg. 42): LQ segment with LQ01=HE and LQ02=N123 indicate the procedure was split.
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 1 (Header).</summary>
		private void FillHeader() {
			//Payer information
			textPayerName.Text=_x835.PayerName;
			textPayerID.Text=_x835.PayerId;
			textPayerAddress1.Text=_x835.PayerAddress;
			textPayerCity.Text=_x835.PayerCity;
			textPayerState.Text=_x835.PayerState;
			textPayerZip.Text=_x835.PayerZip;
			textPayerContactInfo.Text=_x835.PayerContactInfo;
			//Payee information
			textPayeeName.Text=_x835.PayeeName;
			labelPayeeIdType.Text=Lan.g(this,"Payee")+" "+_x835.PayeeIdType;
			textPayeeID.Text=_x835.PayeeId;
			//Payment information
			textTransHandlingDesc.Text=_x835.TransactionHandlingDescript;
			textPaymentMethod.Text=_x835.PayMethodDescript;
			if(_x835.IsCredit) {
				textPaymentAmount.Text=_x835.InsPaid.ToString("f2");
			}
			else {
				textPaymentAmount.Text="-"+_x835.InsPaid.ToString("f2");
			}
			textAcctNumEndingIn.Text=_x835.AccountNumReceiving;
			if(_x835.DateEffective.Year>1880) {
				textDateEffective.Text=_x835.DateEffective.ToShortDateString();
			}
			textCheckNumOrRefNum.Text=_x835.TransRefNum;
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 2 (Detail).</summary>
		private void FillClaimDetails() {
			const int colWidthName=250;
			const int colWidthDateService=80;
			const int colWidthClaimId=100;
			const int colWidthPayorControlNum=126;
			const int colWidthClaimAmt=80;
			const int colWidthPaidAmt=80;
			const int colWidthPatAmt=80;
			int colWidthVariable=gridClaimDetails.Width-colWidthName-colWidthDateService-colWidthClaimId-colWidthPayorControlNum-colWidthClaimAmt-colWidthPaidAmt-colWidthPatAmt;
			gridClaimDetails.BeginUpdate();
			gridClaimDetails.Columns.Clear();
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Patient"),colWidthName,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"DateService"),colWidthDateService,HorizontalAlignment.Center));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"ClaimIdentifier"),colWidthClaimId,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"PayorControlNum"),colWidthPayorControlNum,HorizontalAlignment.Center));//Payer Claim Control Number (CLP07)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Status"),colWidthVariable,HorizontalAlignment.Left));//Claim Status Code Description (CLP02)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"ClaimFee"),colWidthClaimAmt,HorizontalAlignment.Right));//Total Claim Charge Amount (CLP03)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"InsPaid"),colWidthPaidAmt,HorizontalAlignment.Right));//Claim Payment Amount (CLP04)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"PatPortion"),colWidthPatAmt,HorizontalAlignment.Right));//Patient Responsibility Amount (CLP05)
			gridClaimDetails.Rows.Clear();
			_claimInsPaidSum=0;
			List<Hx835_Claim> listClaimsPaid=_x835.ListClaimsPaid;
			for(int i=0;i<listClaimsPaid.Count;i++) {
				Hx835_Claim claimPaid=listClaimsPaid[i];
				ODGridRow row=new ODGridRow();
				row.Tag=claimPaid;
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientName));//Patient
				string strDateService=claimPaid.DateServiceStart.ToShortDateString();
				if(claimPaid.DateServiceEnd>claimPaid.DateServiceStart) {
					strDateService+=" to "+claimPaid.DateServiceEnd.ToShortDateString();
				}
				row.Cells.Add(new UI.ODGridCell(strDateService));//DateService
				row.Cells.Add(new UI.ODGridCell(claimPaid.ClaimTrackingNumber));//Claim Identfier
				row.Cells.Add(new UI.ODGridCell(claimPaid.PayerControlNumber));//PayorControlNum
				row.Cells.Add(new UI.ODGridCell(claimPaid.StatusCodeDescript));//Status
				row.Cells.Add(new UI.ODGridCell(claimPaid.ClaimFee.ToString("f2")));//ClaimFee
				row.Cells.Add(new UI.ODGridCell(claimPaid.InsPaid.ToString("f2")));//InsPaid
				_claimInsPaidSum+=claimPaid.InsPaid;
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientPortion.ToString("f2")));//PatPortion
				gridClaimDetails.Rows.Add(row);
			}
			gridClaimDetails.EndUpdate();
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 3 (Summary).</summary>
		private void FillProviderAdjustmentDetails() {
			if(_x835.ListProvAdjustments.Count==0) {
				gridProviderAdjustments.Title="Provider Adjustments (None Reported)";
			}
			else {
				gridProviderAdjustments.Title="Provider Adjustments";
			}
			const int colWidthNPI=88;
			const int colWidthFiscalPeriod=80;
			const int colWidthReasonCode=90;
			const int colWidthRefIdent=80;
			const int colWidthAmount=80;
			int colWidthVariable=gridProviderAdjustments.Width-colWidthNPI-colWidthFiscalPeriod-colWidthReasonCode-colWidthRefIdent-colWidthAmount;
			gridProviderAdjustments.BeginUpdate();
			gridProviderAdjustments.Columns.Clear();
			gridProviderAdjustments.Columns.Add(new ODGridColumn("NPI",colWidthNPI,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("FiscalPeriod",colWidthFiscalPeriod,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("ReasonCode",colWidthReasonCode,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("RefIdent",colWidthRefIdent,HorizontalAlignment.Center));			
			gridProviderAdjustments.Columns.Add(new ODGridColumn("AdjAmt",colWidthAmount,HorizontalAlignment.Right));
			gridProviderAdjustments.EndUpdate();
			gridProviderAdjustments.BeginUpdate();
			gridProviderAdjustments.Rows.Clear();
			_provAdjAmtSum=0;
			for(int i=0;i<_x835.ListProvAdjustments.Count;i++) {
				Hx835_ProvAdj provAdj=_x835.ListProvAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=provAdj;
				row.Cells.Add(new ODGridCell(provAdj.Npi));//NPI
				row.Cells.Add(new ODGridCell(provAdj.DateFiscalPeriod.ToShortDateString()));//FiscalPeriod
				row.Cells.Add(new ODGridCell(provAdj.ReasonCodeDescript));//Reason
				row.Cells.Add(new ODGridCell(provAdj.ReasonCode));//ReasonCode
				row.Cells.Add(new ODGridCell(provAdj.RefIdentification));//RefIdent
				row.Cells.Add(new ODGridCell(provAdj.AdjAmt.ToString("f2")));//AdjAmt
				_provAdjAmtSum+=provAdj.AdjAmt;
				gridProviderAdjustments.Rows.Add(row);
			}
			gridProviderAdjustments.EndUpdate();
		}

		private void FillFooter() {
			textClaimInsPaidSum.Text=_claimInsPaidSum.ToString("f2");
			textProjAdjAmtSum.Text=_provAdjAmtSum.ToString("f2");
			textPayAmountCalc.Text=(_claimInsPaidSum-_provAdjAmtSum).ToString("f2");
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(MessageText835);
			msgbox.ShowDialog();
		}

		private void gridProviderAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_ProvAdj provAdj=(Hx835_ProvAdj)gridProviderAdjustments.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(
				provAdj.Npi+"\r\n"
				+provAdj.DateFiscalPeriod.ToShortDateString()+"\r\n"
				+provAdj.ReasonCode+" "+provAdj.ReasonCodeDescript+"\r\n"
				+provAdj.RefIdentification+"\r\n"
				+provAdj.AdjAmt.ToString("f2"));
			msgbox.Show(this);
		}

		private void gridClaimDetails_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[e.Row].Tag;
			bool isPaymentEntered=true;
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when actually creating the check
				isPaymentEntered=false;
			}
			else if(!EnterPayment(claimPaid)) {
				isPaymentEntered=false;
			}			
			if(!isPaymentEntered) {
				FormEtrans835ClaimEdit formC=new FormEtrans835ClaimEdit(claimPaid);
				formC.ShowDialog(this);
			}
		}

		///<summary>Returns true if the payment information was loaded into payment entry window, otherwise returns false if the payment could not be entered.</summary>
		private bool EnterPayment(Hx835_Claim claimPaid) {
			//TODO: Do not enter payment if the claim is a split claim.
			//TODO: Do not enter by procedure if any procedures have been bundled/unbundled/split.
			long claimNum=claimPaid.GetOriginalClaimNum();
			if(claimNum==0) {//Original claim not found.
				return false;
			}
			Claim claim=Claims.GetClaim(claimNum);
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(claim.PatNum);
			List<InsSub> listInsSubs=InsSubs.RefreshForFam(fam);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(claim.PatNum);
			if(claim.ClaimStatus=="R") {
				//Do not fill payments if the claim is already received.
				FormEtrans835ClaimPay formP=new FormEtrans835ClaimPay(claimPaid,pat,fam,listInsPlans,listPatPlans,listInsSubs);
				formP.ShowDialog();
				return true;
			}
			if(claimPaid.ListProcs.Count==0) {//Procedure detail not provided with payment.  Enter by total.
				if(claim.ClaimType=="Cap") {
					return false;//Capitation claims should not be entered by total because the insurance payment would affect the patient balance.
				}
				ClaimProc ClaimProcCur=new ClaimProc();
				//ClaimProcs.Cur.ProcNum 
				ClaimProcCur.ClaimNum=claim.ClaimNum;
				ClaimProcCur.PatNum=claim.PatNum;
				ClaimProcCur.ProvNum=claim.ProvTreat;
				//ClaimProcs.Cur.FeeBilled
				//ClaimProcs.Cur.InsPayEst
				ClaimProcCur.DedApplied=(double)claimPaid.Deductible;
				ClaimProcCur.Status=ClaimProcStatus.Received;
				ClaimProcCur.InsPayAmt=(double)claimPaid.InsPaid;
				//remarks
				//ClaimProcs.Cur.ClaimPaymentNum
				ClaimProcCur.PlanNum=claim.PlanNum;
				ClaimProcCur.InsSubNum=claim.InsSubNum;
				ClaimProcCur.DateCP=DateTimeOD.Today;
				ClaimProcCur.ProcDate=claim.DateService;
				ClaimProcCur.DateEntry=DateTime.Now;//will get set anyway
				ClaimProcCur.ClinicNum=claim.ClinicNum;
				//Automatically set PayPlanNum if there is a payplan with matching PatNum, PlanNum, and InsSubNum that has not been paid in full.
				//By sending in ClaimNum, we ensure that we only get the payplan a claimproc from this claim was already attached to or payplans with no claimprocs attached.
				List<PayPlan> payPlanList=PayPlans.GetValidInsPayPlans(ClaimProcCur.PatNum,ClaimProcCur.PlanNum,ClaimProcCur.InsSubNum,ClaimProcCur.ClaimNum);
				ClaimProcCur.PayPlanNum=0;
				if(payPlanList.Count==1) {
					ClaimProcCur.PayPlanNum=payPlanList[0].PayPlanNum;
				}
				else if(payPlanList.Count>1) {
					//more than one valid PayPlan
					List<PayPlanCharge> chargeList=PayPlanCharges.Refresh(ClaimProcCur.PatNum);
					FormPayPlanSelect FormPPS=new FormPayPlanSelect(payPlanList,chargeList);
					FormPPS.ShowDialog();
					if(FormPPS.DialogResult==DialogResult.OK) {
						ClaimProcCur.PayPlanNum=payPlanList[FormPPS.IndexSelected].PayPlanNum;
					}
				}
				ClaimProcs.Insert(ClaimProcCur);
				FormEtrans835ClaimPay formP=new FormEtrans835ClaimPay(claimPaid,pat,fam,listInsPlans,listPatPlans,listInsSubs);
				if(formP.ShowDialog()!=DialogResult.OK) {
					ClaimProcs.Delete(ClaimProcCur);
				}
				else {//Payment was accepted by user.
					List<ClaimProc> listClaimProcsForClaim=ClaimProcs.RefreshForClaim(claim.ClaimNum);
					for(int i=0;i<listClaimProcsForClaim.Count;i++) {
						if(listClaimProcsForClaim[i].Status!=ClaimProcStatus.NotReceived) {
							continue;
						}
						//ClaimProcs.Cur=ClaimProcs.ForClaim[i];
						listClaimProcsForClaim[i].Status=ClaimProcStatus.Received;
						if(listClaimProcsForClaim[i].DedApplied>0) {
							listClaimProcsForClaim[i].InsPayEst+=listClaimProcsForClaim[i].DedApplied;
							listClaimProcsForClaim[i].DedApplied=0;//because ded will show as part of payment now.
						}
						listClaimProcsForClaim[i].DateEntry=DateTime.Now;//the date is was switched to rec'd
						ClaimProcs.Update(listClaimProcsForClaim[i]);
					}
				}
				claim.ClaimStatus="R";//Received.
				claim.DateReceived=DateTimeOD.Today;
				Claims.Update(claim);
			}
			else {//Procedure detail provided.  Enter by procedure.
				//TODO:
			}
			
			
			return true;
		}

		private void butPrint_Click(object sender,EventArgs e) {
			FormEtrans835Print form=new FormEtrans835Print(_x835);
			form.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}