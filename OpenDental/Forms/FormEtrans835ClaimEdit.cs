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
	public partial class FormEtrans835ClaimEdit:Form {

		private X835 _x835;
		private string _claimTrackingNumber;
		private string[] _claimInfo;
		private Claim _claim;
		private Patient _pat;
		private List<string> _listAdjustmentDetails;
		private List<string> _listAdjudicationDetails;

		public FormEtrans835ClaimEdit(X835 x835,string claimTrackingNumber) {
			InitializeComponent();
			Lan.F(this);
			_x835=x835;
			_claimTrackingNumber=claimTrackingNumber;
		}

		private void FormEtrans835ClaimEdit_Load(object sender,EventArgs e) {
			_claimInfo=_x835.GetClaimInfo(_claimTrackingNumber);
			long claimNum=Claims.GetClaimNumForIdentifier(_claimTrackingNumber);
			_claim=null;
			_pat=null;
			if(claimNum!=0) {
				_claim=Claims.GetClaim(claimNum);
				_pat=Patients.GetPat(_claim.PatNum);
			}
			FillAll();
		}

		private void FormEtrans835ClaimEdit_Resize(object sender,EventArgs e) {
			FillAdjustmentDetail();//Because the grid columns change size depending on the form size.
			FillProcedureDetails();//Because the grid columns change size depending on the form size.
			FillAdjudicationDetails();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillHeader();
			FillAdjustmentDetail();
			FillProcedureDetails();
			FillAdjudicationDetails();
		}

		private void FillHeader() {
			if(_pat==null) {
				textPatientName.Text="";
			}
			else {
				textPatientName.Text=_pat.GetNameFLFormal();
			}
			if(_claim==null) {
				textDateService.Text="";
			}
			else {
				textDateService.Text=_claim.DateService.ToShortDateString();
			}
			textClaimIdentifier.Text=_claimTrackingNumber;
			textPayorControlNum.Text=_claimInfo[4];
			textStatus.Text=_claimInfo[0];
			textClaimFee.Text=_claimInfo[1];
			textClaimFee2.Text=_claimInfo[1];
			textInsPaid.Text=_claimInfo[2];
			textInsPaid2.Text=_claimInfo[2];
			textPatientPortion.Text=_claimInfo[3];
		}

		private void FillAdjustmentDetail() {
			_listAdjustmentDetails=_x835.GetClaimAdjustmentInfo(_claimTrackingNumber);
			if(_listAdjustmentDetails.Count==0) {
				gridAdjustmentDetails.Title="Adjustment Details (None Reported)";
			}
			else {
				gridAdjustmentDetails.Title="Adjustment Details";
			}
			gridAdjustmentDetails.BeginUpdate();
			gridAdjustmentDetails.Columns.Clear();
			const int colWidthDescription=200;
			const int colWidthAmount=80;
			int colWidthVariable=gridAdjustmentDetails.Width-10-colWidthDescription-colWidthAmount;
			gridAdjustmentDetails.Columns.Add(new UI.ODGridColumn("Description",colWidthDescription,HorizontalAlignment.Left));
			gridAdjustmentDetails.Columns.Add(new UI.ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridAdjustmentDetails.Columns.Add(new UI.ODGridColumn("Amount",colWidthAmount,HorizontalAlignment.Right));
			gridAdjustmentDetails.Rows.Clear();
			decimal totalAdjustments=0;
			for(int i=0;i<_listAdjustmentDetails.Count;i+=3) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i]));//Description
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i+1]));//Reason
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i+2]));//Amount
				decimal adjAmount=PIn.Decimal(_listAdjustmentDetails[i+2]);
				totalAdjustments+=adjAmount;
				gridAdjustmentDetails.Rows.Add(row);
			}
			gridAdjustmentDetails.EndUpdate();
			textTotalAdjustments.Text=totalAdjustments.ToString("f2");
		}

		private void FillProcedureDetails() {
			List<string> listProcInfo=_x835.GetProcInfo(_claimTrackingNumber);
			if(listProcInfo.Count==0) {
				gridProcedureDetails.Title="Procedure Details (None Reported)";
			}
			else {
				gridProcedureDetails.Title="Procedure Details";
			}
			gridProcedureDetails.BeginUpdate();
			const int colWidthProcNum=80;
			const int colWidthProcCode=80;
			const int colWidthProcFee=80;
			const int colWidthAdjAmt=80;
			const int colWidthInsPaid=80;
			int colWidthVariable=gridProcedureDetails.Width-10-colWidthProcNum-colWidthProcCode-colWidthProcFee-colWidthAdjAmt-colWidthInsPaid;
			gridProcedureDetails.Columns.Clear();
			gridProcedureDetails.Columns.Add(new ODGridColumn("ProcNum",colWidthProcNum,HorizontalAlignment.Left));
			gridProcedureDetails.Columns.Add(new ODGridColumn("ProcCode",colWidthProcCode,HorizontalAlignment.Center));
			gridProcedureDetails.Columns.Add(new ODGridColumn("ProcDescript",colWidthVariable,HorizontalAlignment.Left));
			gridProcedureDetails.Columns.Add(new ODGridColumn("ProcFee",colWidthProcFee,HorizontalAlignment.Right));
			gridProcedureDetails.Columns.Add(new ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureDetails.Columns.Add(new ODGridColumn("InsPaid",colWidthInsPaid,HorizontalAlignment.Right));
			gridProcedureDetails.Rows.Clear();
			decimal totalProcAdjustments=0;
			for(int i=0;i<listProcInfo.Count;i+=5) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(listProcInfo[i+4]));//ProcNum
				string strProcCode=listProcInfo[i+1];
				row.Cells.Add(new ODGridCell(strProcCode));//ProcCode
				string procDescript="";
				if(ProcedureCodes.IsValidCode(strProcCode)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(strProcCode);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//ProcDescript
				row.Cells.Add(new ODGridCell(listProcInfo[i+2]));//ProcFee
				int segSvcIndex=PIn.Int(listProcInfo[i]);
				List<string> listProcAdjustments=_x835.GetProcAdjustmentInfo(segSvcIndex);
				decimal adjAmtForProc=0;
				for(int j=0;j<listProcAdjustments.Count;j+=3) {
					decimal adjAmt=PIn.Decimal(listProcAdjustments[j+2]);
					adjAmtForProc+=adjAmt;
					totalProcAdjustments+=adjAmt;
				}
				row.Cells.Add(new ODGridCell(adjAmtForProc.ToString("f2")));//AdjAmt
				row.Cells.Add(new ODGridCell(listProcInfo[i+3]));//InsPaid
				gridProcedureDetails.Rows.Add(row);
			}
			gridProcedureDetails.EndUpdate();
			textProcAdjustments.Text=totalProcAdjustments.ToString("f2");
		}

		private void FillAdjudicationDetails() {
			_listAdjudicationDetails=_x835.GetClaimAdjudicationInfo(_claimTrackingNumber);
			if(_listAdjudicationDetails.Count==0) {
				gridAdjudicationDetails.Title="Adjudication Details (None Reported)";
			}
			else {
				gridAdjudicationDetails.Title="Adjudication Details";
			}
			gridAdjudicationDetails.BeginUpdate();
			gridAdjudicationDetails.Columns.Clear();
			gridAdjudicationDetails.Columns.Add(new UI.ODGridColumn("Description",gridAdjudicationDetails.Width/2,HorizontalAlignment.Left));
			gridAdjudicationDetails.Columns.Add(new UI.ODGridColumn("Value",gridAdjudicationDetails.Width/2,HorizontalAlignment.Left));
			gridAdjudicationDetails.Rows.Clear();
			for(int i=0;i<_listAdjudicationDetails.Count;i+=2) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new UI.ODGridCell(_listAdjudicationDetails[i]));
				row.Cells.Add(new UI.ODGridCell(_listAdjudicationDetails[i+1]));
				gridAdjudicationDetails.Rows.Add(row);
			}
			gridAdjudicationDetails.EndUpdate();
		}

		private void gridAdjudicationDetails_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_listAdjudicationDetails[e.Row*2+1]);
			msgbox.Show(this);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}