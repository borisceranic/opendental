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

		public Etrans EtransCur;
		private string _messageText;
		private X835 _x835;
		private decimal _claimPaymentTotal;
		private decimal _provAdjustmentTotal;

		public FormEtrans835Edit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans835Edit_Load(object sender,EventArgs e) {
			_messageText=EtransMessageTexts.GetMessageText(EtransCur.EtransMessageTextNum);
			_x835=new X835(_messageText);
			FillAll();
		}

		private void FormEtrans835Edit_Resize(object sender,EventArgs e) {
			//This funciton is called before FormEtrans835Edit_Load() when using ShowDialog(). Therefore, x835 is null the first time FormEtrans835Edit_Resize() is called.
			if(_x835==null) {
				return;
			}
			FillProviderAdjustmentDetails();//Because the grid columns change size depending on the form size.
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
			//*Reassociation (pg. 19): 835 TRN = Reassociation Key Segment. See TRN02.
			//*SVC02-(CAS03+CAS06+CAS09+CAS12+CAS15+CAS18)=SVC03
			//When the service payment information loop is not present, then: CLP03-(CAS03+CAS06+CAS09+CAS12+CAS15+CAS18)=CLP04
			//*Otherwise, CAS must also be considered from the service adjustment segment.
			//*Reassociation (pg. 20): Use the trace # in TRN02 and the company ID number in TRN03 to uniquely identify the claim payment/data.
			//*Institutional (pg. 23): CAS reason code 78 requires special handling.
			//*Advance payments (pg. 23): in PLB segment with adjustment reason code PI. Can be yearly or monthly.
			//*Bundled procs (pg. 27): have the original proc listed in SV06. Use Line Item Control Number to identify the original proc line.
			//*Line Item Control Number (pgs. 28 & 36): REF*6B or LX01 from 837 -> 2110REF in 835. We are not using REF*6B in 837, so we will get LX01 back in 835.
			//*Predetermination (pg. 28): Identified by claim status code 25 in CLP02. Claim adjustment reason code is 101.
			//*Claim reversals (pg. 30): Identified by code 22 in CLP02. The original claim adjustment codes can be found in CAS01 to negate the original claim.
			//Use CLP07 to identify the original claim, or if different, get the original ref num from REF02 of 2040REF*F8.
			//*Interest and Prompt Payment Discounts (pg. 31): Located in AMT segments with qualifiers I (interest) and D8 (discount). Found at claim and provider/check level.
			//Not part of AR, but part of deposit. Handle this situation by using claimprocs with 2 new status, one for interest and one for discount? Would allow reports, deposits, and claim checks to work as is.
			//*Capitation and related payments or adjustments (pg. 34 & 52): Not many of our customers use capitation, so this will probably be our last concern.
			//*Claim splits (pg. 36): MIA or MOA segments will exist to indicate the claim was split.
			//*Service Line Splits (pg. 42): LQ segment with LQ01=HE and LQ02=N123 indicate the procedure was split.
			//*PPOs (pg. 47): 2100CAS or 2110CAS will contain the value CO (Contractual Obligation) in CAS01. The PPO name is reported in REF02 of the Other Claim Related Information segment REF*CE.
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 1 (Header).</summary>
		private void FillHeader() {
			//Payer information
			textPayerName.Text=_x835.GetPayerName();
			textPayerID.Text=_x835.GetPayerID();
			textPayerAddress1.Text=_x835.GetPayerAddress1();
			textPayerCity.Text=_x835.GetPayerCityName();
			textPayerState.Text=_x835.GetPayerState();
			textPayerZip.Text=_x835.GetPayerZip();
			textPayerContactInfo.Text=_x835.GetPayerContactInfo();
			//Payee information
			textPayeeName.Text=_x835.GetPayeeName();
			labelPayeeIdType.Text=Lan.g(this,"Payee")+" "+_x835.GetPayeeIdType();
			textPayeeID.Text=_x835.GetPayeeId();
			//Payment information
			textTransHandlingDesc.Text=_x835.GetTransactionHandlingCodeDescription();
			textPaymentMethod.Text=_x835.GetPaymentMethodDescription();
			if(_x835.IsCredit()) {
				textPaymentAmount.Text=_x835.GetPaymentAmount();
			}
			else {
				textPaymentAmount.Text="-"+_x835.GetPaymentAmount();
			}
			textAcctNumEndingIn.Text=_x835.GetAccountNumReceivingShort();
			DateTime dateEffective=_x835.GetDateEffective();
			if(dateEffective.Year>1880) {
				textDateEffective.Text=dateEffective.ToShortDateString();
			}
			textCheckNumOrRefNum.Text=_x835.GetTransactionReferenceNumber();			
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 2 (Detail).</summary>
		private void FillClaimDetails() {
			List<string> claimTrackingNumbers=_x835.GetClaimTrackingNumbers();
			const int colWidthLname=150;
			const int colWidthFname=100;
			const int colWidthDateService=80;
			const int colWidthClaimId=100;
			const int colWidthPayorControlNum=126;
			const int colWidthClaimAmt=80;
			const int colWidthPaidAmt=80;
			const int colWidthPatAmt=80;
			int colWidthVariable=this.Width-2*gridClaimDetails.Left-36-colWidthLname-colWidthFname-colWidthDateService-colWidthClaimId-colWidthPayorControlNum-colWidthClaimAmt-colWidthPaidAmt-colWidthPatAmt;
			gridClaimDetails.BeginUpdate();
			gridClaimDetails.Columns.Clear();
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"LName"),colWidthLname,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"FName"),colWidthFname,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"DateService"),colWidthDateService,HorizontalAlignment.Center));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"ClaimIdentifier"),colWidthClaimId,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"PayorControlNum"),colWidthPayorControlNum,HorizontalAlignment.Center));//Payer Claim Control Number (CLP07)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Status"),colWidthVariable,HorizontalAlignment.Left));//Claim Status Code Description (CLP02)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"ClaimFee"),colWidthClaimAmt,HorizontalAlignment.Right));//Total Claim Charge Amount (CLP03)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"InsPaid"),colWidthPaidAmt,HorizontalAlignment.Right));//Claim Payment Amount (CLP04)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"PatPortion"),colWidthPatAmt,HorizontalAlignment.Right));//Patient Responsibility Amount (CLP05)
			gridClaimDetails.Rows.Clear();
			_claimPaymentTotal=0;
			for(int i=0;i<claimTrackingNumbers.Count;i++) {
				ODGridRow row=new ODGridRow();
				string[] claimInfo=_x835.GetClaimInfo(claimTrackingNumbers[i]);
				long claimNum=Claims.GetClaimNumForIdentifier(claimTrackingNumbers[i]);
				Claim claim=null;
				if(claimNum!=0) {
					claim=Claims.GetClaim(claimNum);
					Patient pat=Patients.GetPat(claim.PatNum);
					row.Cells.Add(new UI.ODGridCell(pat.LName));//LName
					row.Cells.Add(new UI.ODGridCell(pat.FName));//FName
					row.Cells.Add(new UI.ODGridCell(claim.DateService.ToShortDateString()));//DateService
				}
				else {
					row.Cells.Add(new UI.ODGridCell(""));//LName
					row.Cells.Add(new UI.ODGridCell(""));//FName
					row.Cells.Add(new UI.ODGridCell(""));//DateService
				}
				row.Cells.Add(new UI.ODGridCell(claimTrackingNumbers[i]));//Claim Identfier
				row.Cells.Add(new UI.ODGridCell(claimInfo[4]));//PayorControlNum
				row.Cells.Add(new UI.ODGridCell(claimInfo[0]));//Status
				row.Cells.Add(new UI.ODGridCell(claimInfo[1]));//ClaimFee
				row.Cells.Add(new UI.ODGridCell(claimInfo[2]));//InsPaid
				_claimPaymentTotal+=PIn.Decimal(claimInfo[2]);
				row.Cells.Add(new UI.ODGridCell(claimInfo[3]));//PatPortion
				row.Tag=claimTrackingNumbers[i];
				gridClaimDetails.Rows.Add(row);
			}
			gridClaimDetails.EndUpdate();
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 3 (Summary).</summary>
		private void FillProviderAdjustmentDetails() {
			const int colWidthNPI=88;
			const int colWidthFiscalPeriod=80;
			const int colWidthReasonCode=90;
			const int colWidthRefIdent=80;
			const int colWidthAmount=80;
			int colWidthVariable=this.Width-gridProviderAdjustments.Left-36-colWidthNPI-colWidthFiscalPeriod-colWidthReasonCode-colWidthRefIdent-colWidthAmount;
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
			List<string[]> providerAdjustments=_x835.GetProviderLevelAdjustments();
			_provAdjustmentTotal=0;
			for(int i=0;i<providerAdjustments.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(providerAdjustments[i][0]));//NPI
				row.Cells.Add(new ODGridCell(providerAdjustments[i][1]));//FiscalPeriod
				row.Cells.Add(new ODGridCell(providerAdjustments[i][2]));//Reason
				row.Cells.Add(new ODGridCell(providerAdjustments[i][3]));//ReasonCode
				row.Cells.Add(new ODGridCell(providerAdjustments[i][4]));//RefIdent
				row.Cells.Add(new ODGridCell(providerAdjustments[i][5]));//AdjAmt
				_provAdjustmentTotal+=PIn.Decimal(providerAdjustments[i][5]);
				gridProviderAdjustments.Rows.Add(row);
			}
			gridProviderAdjustments.EndUpdate();
		}

		private void FillFooter() {
			textClaimPaymentTotal.Text=_claimPaymentTotal.ToString("f2");
			textProvAdjustmentTotal.Text=_provAdjustmentTotal.ToString("f2");
			textPaymentAmountCalc.Text=(_claimPaymentTotal-_provAdjustmentTotal).ToString("f2");
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_messageText);
			msgbox.ShowDialog();
		}

		private void gridClaimDetails_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string claimTrackingNumber=(string)gridClaimDetails.Rows[e.Row].Tag;
			FormEtrans835ClaimEdit form=new FormEtrans835ClaimEdit(_x835,claimTrackingNumber);
			form.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
		
	}
}