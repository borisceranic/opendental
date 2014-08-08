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
		///<summary>Must be set before the form is shown.</summary>
		public Etrans EtransCur;
		///<summary>Must be set before the form is shown.  The message text for EtransCur.</summary>
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
			const int colWidthRecd=32;
			const int colWidthName=250;
			const int colWidthDateService=80;
			const int colWidthClaimId=86;
			const int colWidthPayorControlNum=108;
			const int colWidthClaimAmt=80;
			const int colWidthPaidAmt=80;
			const int colWidthPatAmt=80;
			int colWidthVariable=gridClaimDetails.Width-colWidthRecd-colWidthName-colWidthDateService-colWidthClaimId-colWidthPayorControlNum-colWidthClaimAmt-colWidthPaidAmt-colWidthPatAmt;
			gridClaimDetails.BeginUpdate();
			gridClaimDetails.Columns.Clear();
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Recd"),colWidthRecd,HorizontalAlignment.Center));
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
				long claimNum=claimPaid.GetOriginalClaimNum();
				if(claimNum!=0 && Claims.GetClaim(claimNum).ClaimStatus=="R") {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientName.ToString()));//Patient
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
			bool isPaymentEntry=true;
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when actually creating the check
				isPaymentEntry=false;
			}
			//TODO: Do not enter payment if the claim is a split claim.
			long claimNum=claimPaid.GetOriginalClaimNum();
			Claim claim=null;
			if(claimNum==0) {//Original claim not found.
				isPaymentEntry=false;
			}
			else {
				claim=Claims.GetClaim(claimNum);
			}
			if(claim!=null && claim.ClaimStatus=="R") {
				//If the claim is already received, then we do not allow the user to enter payments.
				//Returning false will cause the EOB details window to show, then the user can click on the Edit Claim button to change the status of the claim if they would like to enter the payment again.
				Patient pat=Patients.GetPat(claim.PatNum);
				Family fam=Patients.GetFamily(claim.PatNum);
				FormClaimEdit formCE=new FormClaimEdit(claim,pat,fam);
				formCE.ShowDialog();
			}
			else {
				if(claimPaid.ListProcs.Count==0 && claim!=null && claim.ClaimType=="Cap") {//Procedure detail not provided with payment and the original claim is a capitation claim.
					//Capitation claims should not be entered by total because the insurance payment would affect the patient balance.
					isPaymentEntry=false;
				}
				if(isPaymentEntry) {
					EnterPayment(claimPaid,claim);
				}
				else {
					//Show EOB details (read only).
					FormEtrans835ClaimEdit formC=new FormEtrans835ClaimEdit(claimPaid);
					formC.ShowDialog(this);
					if(claim!=null) {
						claim=Claims.GetClaim(claim.ClaimNum);//The user might have edited the claim here.  We need to refresh from DB so we can check the status below.
					}
				}
			}
			if(claim!=null && claim.ClaimStatus=="R") {
				gridClaimDetails.Rows[e.Row].Cells[0].Text="X";//Indicate that payment is received.
			}
			else {
				gridClaimDetails.Rows[e.Row].Cells[0].Text="";//Indicate that payment is not received.
			}
		}

		///<summary>Enter either by total or by procedure, depending on whether or not procedure detail was provided in the 835 for this claim.</summary>
		private void EnterPayment(Hx835_Claim claimPaid,Claim claim) {
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(claim.PatNum);
			List<InsSub> listInsSubs=InsSubs.RefreshForFam(fam);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(claim.PatNum);
			List<ClaimProc> listClaimProcsForClaim=ClaimProcs.RefreshForClaim(claim.ClaimNum);
			ClaimProc cpByTotal=new ClaimProc();
			cpByTotal.DedApplied=(double)claimPaid.PatientPortion;
			cpByTotal.AllowedOverride=(double)claimPaid.AllowedAmt;
			cpByTotal.InsPayAmt=(double)claimPaid.InsPaid;
			cpByTotal.WriteOff=(double)claimPaid.Writeoff;
			cpByTotal.Remarks=claimPaid.GetRemarks();
			for(int i=0;i<listClaimProcsForClaim.Count;i++) {
				ClaimProc claimProc=listClaimProcsForClaim[i];
				if(claimProc.ClaimProcNum>0) {//Procedure, not an existing total payment.
					List<Hx835_Proc> listProcsForProcNum=claimPaid.GetPaymentsForClaimProc(claimProc);
					//If listProcsForProcNum.Count==0, then procedure payment details were not not found.
					//This can happen with procedures from older 837s, when we did not send out the procedure identifiers, in which case ProcNum would be 0.
					//Since we cannot place detail on the service line, we will leave the amounts for the procedure on the total payment line.
					//If listProcsForPorcNum.Count==1, then we know that the procedure was adjudicated as is or it might have been bundled, but we treat both situations the same way.
					//The 835 is required to include one line for each bundled procedure, which gives is a direct manner in which to associate each line to its original procedure.
					//If listProcForProcNum.Count > 1, then the procedure was either split or unbundled when it was adjudicated by the payer.
					//We will not bother to modify the procedure codes on the claim, because the user can see how the procedure was split or unbunbled by viewing the 835 details.
					//Instead, we will simply add up all of the partial payment lines for the procedure, and report the full payment amount on the original procedure.
					if(claimProc.Status==ClaimProcStatus.Received || claimProc.Status==ClaimProcStatus.Supplemental || claimProc.Status==ClaimProcStatus.CapComplete) {//Already received this procedure for some reason.
						//Do not modify the claimproc amounts, because they are historical now.
					}
					else {
						claimProc.DedApplied=0;
						claimProc.AllowedOverride=0;
						claimProc.InsPayAmt=0;
						claimProc.WriteOff=0;
						StringBuilder sb=new StringBuilder();
						for(int j=0;j<listProcsForProcNum.Count;j++) {
							Hx835_Proc procPaidPartial=listProcsForProcNum[j];
							claimProc.DedApplied+=(double)procPaidPartial.PatientPortion;
							claimProc.AllowedOverride+=(double)procPaidPartial.AllowedAmt;
							claimProc.InsPayAmt+=(double)procPaidPartial.InsPaid;
							claimProc.WriteOff+=(double)procPaidPartial.Writeoff;
							if(sb.Length>0) {
								sb.Append("\r\n");
							}
							sb.Append(procPaidPartial.GetRemarks());
						}
						claimProc.Remarks=sb.ToString();
					}
				}
				//Displace the procedure totals from the "by total" payment, since they have now been accounted for on the individual procedure line.  Totals will not be affected if no procedure details could be located.
				cpByTotal.DedApplied-=claimProc.DedApplied;
				cpByTotal.AllowedOverride-=claimProc.AllowedOverride;
				cpByTotal.InsPayAmt-=claimProc.InsPayAmt;
				cpByTotal.WriteOff-=claimProc.WriteOff;
			}
			bool isByTotal=true;
			if(claimPaid.ListProcs.Count>0) {//Procedure detail was provided in the 835.
				isByTotal=false;
			}
			//Do not create a total payment if the payment contains all zero amounts because it would not be useful.  Written to account for potential rounding errors in the amounts.
			if(Math.Round(cpByTotal.DedApplied,2)==0 && Math.Round(cpByTotal.AllowedOverride,2)==0 && Math.Round(cpByTotal.InsPayAmt,2)==0 && Math.Round(cpByTotal.WriteOff,2)==0) {
				isByTotal=false;
			}
			if(isByTotal) {
				cpByTotal.Status=ClaimProcStatus.NotReceived;//Will be marked received once the user accepts the payment amount by clicking OK in FormEtrans835ClaimPay.  Must be NotReceived, or will not be saved to database.
				//ClaimProcs.Cur.ProcNum 
				cpByTotal.ClaimNum=claim.ClaimNum;
				cpByTotal.PatNum=claim.PatNum;
				cpByTotal.ProvNum=claim.ProvTreat;
				//ClaimProcs.Cur.FeeBilled
				//ClaimProcs.Cur.InsPayEst
				//remarks
				//ClaimProcs.Cur.ClaimPaymentNum
				cpByTotal.PlanNum=claim.PlanNum;
				cpByTotal.InsSubNum=claim.InsSubNum;
				cpByTotal.DateCP=DateTimeOD.Today;
				cpByTotal.ProcDate=claim.DateService;
				cpByTotal.DateEntry=DateTime.Now;//will get set anyway
				cpByTotal.ClinicNum=claim.ClinicNum;
				//Automatically set PayPlanNum if there is a payplan with matching PatNum, PlanNum, and InsSubNum that has not been paid in full.
				//By sending in ClaimNum, we ensure that we only get the payplan a claimproc from this claim was already attached to or payplans with no claimprocs attached.
				List<PayPlan> listPayPlan=PayPlans.GetValidInsPayPlans(cpByTotal.PatNum,cpByTotal.PlanNum,cpByTotal.InsSubNum,cpByTotal.ClaimNum);
				cpByTotal.PayPlanNum=0;
				if(listPayPlan.Count==1) {
					cpByTotal.PayPlanNum=listPayPlan[0].PayPlanNum;
				}
				else if(listPayPlan.Count>1) {
					//more than one valid PayPlan
					List<PayPlanCharge> chargeList=PayPlanCharges.Refresh(cpByTotal.PatNum);
					FormPayPlanSelect FormPPS=new FormPayPlanSelect(listPayPlan,chargeList);
					FormPPS.ShowDialog();
					if(FormPPS.DialogResult==DialogResult.OK) {
						cpByTotal.PayPlanNum=listPayPlan[FormPPS.IndexSelected].PayPlanNum;
					}
				}
				listClaimProcsForClaim.Insert(0,cpByTotal);//Add to the beginning of the list, so that the ins paid amount will be highlighted when FormEtrans835ClaimPay loads.
			}
			FormEtrans835ClaimPay formP=new FormEtrans835ClaimPay(claimPaid,claim,pat,fam,listInsPlans,listPatPlans,listInsSubs);
			formP.ListClaimProcsForClaim=listClaimProcsForClaim;
			if(formP.ShowDialog()!=DialogResult.OK) {
				if(cpByTotal.ClaimProcNum!=0) {
					ClaimProcs.Delete(cpByTotal);
				}
			}
		}

		/////<summary>This function will enter payments on procs which are not yet received.
		/////Unmatched procedures and claim level adjustments will be rolled into a single "by total" payment when necessary.</summary>
		//private void EnterPaymentByProcedure(Hx835_Claim claimPaid,List <ClaimProc> listClaimProcsForClaim) {
			//List<ClaimProc> cpList=new List<ClaimProc>();
			//for(int i=0;i<gridProc.SelectedIndices.Length;i++) {
			//	//copy selected claimprocs to temporary array for editing.
			//	//no changes to the database will be made within that form.
			//	cpList.Add(ClaimProcsForClaim[gridProc.SelectedIndices[i]].Copy());
			//	if(ClaimCur.ClaimType=="PreAuth") {
			//		cpList[i].Status=ClaimProcStatus.Preauth;
			//	}
			//	else if(ClaimCur.ClaimType=="Cap") {
			//		;//do nothing.  The claimprocstatus will remain Capitation.
			//	}
			//	else {
			//		cpList[i].Status=ClaimProcStatus.Received;
			//		cpList[i].DateEntry=DateTime.Now;//date is was set rec'd
			//		cpList[i].InsPayAmt=cpList[i].InsPayEst;
			//		cpList[i].PayPlanNum=0;
			//		if(i==0) {
			//			//Automatically set PayPlanNum if there is a payplan with matching PatNum, PlanNum, and InsSubNum that has not been paid in full.
			//			//By sending in ClaimNum, we ensure that we only get the payplan a claimproc from this claim was already attached to or payplans with no claimprocs attached.
			//			List<PayPlan> payPlanList=PayPlans.GetValidInsPayPlans(cpList[i].PatNum,cpList[i].PlanNum,cpList[i].InsSubNum,cpList[i].ClaimNum);
			//			if(payPlanList.Count==1) {
			//				cpList[i].PayPlanNum=payPlanList[0].PayPlanNum;
			//			}
			//			else if(payPlanList.Count>1) {
			//				//more than one valid PayPlan
			//				List<PayPlanCharge> chargeList=PayPlanCharges.Refresh(cpList[i].PatNum);
			//				FormPayPlanSelect FormPPS=new FormPayPlanSelect(payPlanList,chargeList);
			//				FormPPS.ShowDialog();
			//				if(FormPPS.DialogResult==DialogResult.OK) {
			//					cpList[i].PayPlanNum=payPlanList[FormPPS.IndexSelected].PayPlanNum;
			//				}
			//			}
			//		}
			//		else {
			//			cpList[i].PayPlanNum=cpList[0].PayPlanNum;//set all procs to the same payplan, they can change it later if not correct for each claimproc that is different
			//		}
			//	}
			//	cpList[i].DateCP=DateTimeOD.Today;
			//}
			//if(ClaimCur.ClaimType=="PreAuth") {
			//	FormClaimPayPreAuth FormCPP=new FormClaimPayPreAuth(PatCur,FamCur,PlanList,PatPlanList,SubList);
			//	FormCPP.ClaimProcsToEdit=cpList;
			//	FormCPP.ShowDialog();
			//	if(FormCPP.DialogResult!=DialogResult.OK) {
			//		return;
			//	}
			//	//save changes now
			//	for(int i=0;i<FormCPP.ClaimProcsToEdit.Count;i++) {
			//		ClaimProcs.Update(FormCPP.ClaimProcsToEdit[i]);
			//		ClaimProcs.SetInsEstTotalOverride(FormCPP.ClaimProcsToEdit[i].ProcNum,FormCPP.ClaimProcsToEdit[i].PlanNum,
			//			FormCPP.ClaimProcsToEdit[i].InsPayEst,ClaimProcList);
			//	}
			//}
			//else {
			//	FormClaimPayTotal FormCPT=new FormClaimPayTotal(PatCur,FamCur,PlanList,PatPlanList,SubList);
			//	FormCPT.ClaimProcsToEdit=cpList.ToArray();
			//	FormCPT.ShowDialog();
			//	if(FormCPT.DialogResult!=DialogResult.OK) {
			//		return;
			//	}
			//	//save changes now
			//	for(int i=0;i<FormCPT.ClaimProcsToEdit.Length;i++) {
			//		ClaimProcs.Update(FormCPT.ClaimProcsToEdit[i]);
			//	}
			//}
			//comboClaimStatus.SelectedIndex=5;//Received
			//if(textDateRec.Text=="") {
			//	textDateRec.Text=DateTime.Today.ToShortDateString();
			//}
			//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			//FillGrids();
			//return listClaimProcsToEnter;
		//}

		private void butClaimDetails_Click(object sender,EventArgs e) {
			if(gridClaimDetails.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Choose a claim paid before viewing details.");
				return;
			}
			Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[gridClaimDetails.SelectedIndices[0]].Tag;
			FormEtrans835ClaimEdit formE=new FormEtrans835ClaimEdit(claimPaid);
			formE.ShowDialog();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			FormEtrans835Print form=new FormEtrans835Print(_x835);
			form.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			bool isReceived=true;
			for(int i=0;i<gridClaimDetails.Rows.Count;i++) {
				if(gridClaimDetails.Rows[i].Cells[0].Text=="") {
					isReceived=false;
					break;
				}
			}
			if(isReceived) {
				EtransCur.AckCode="Recd";
			}
			else {
				EtransCur.AckCode="";
			}
			Etranss.Update(EtransCur);
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}