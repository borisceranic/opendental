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
		private List<string> _listClaimAdjustments;
		private List<string> _listProcInfo;
		private List<string> _listAdjudicationInfo;
		private decimal _claimAdjAmtSum;
		private decimal _procAdjAmtSum;

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
			FillClaimAdjustments();//Because the grid columns change size depending on the form size.
			FillProcedureBreakdown();//Because the grid columns change size depending on the form size.
			FillAdjudicationInfo();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillClaimAdjustments();
			FillProcedureBreakdown();
			FillAdjudicationInfo();
			FillHeader();
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
			textInsPaidCalc.Text=(PIn.Decimal(_claimInfo[1])-_claimAdjAmtSum-_procAdjAmtSum).ToString("f2");
			textPatientPortion.Text=_claimInfo[3];
		}

		private void FillClaimAdjustments() {
			_listClaimAdjustments=_x835.GetClaimAdjustmentInfo(_claimTrackingNumber);
			if(_listClaimAdjustments.Count==0) {
				gridClaimAdjustments.Title="Claim Adjustments (None Reported)";
			}
			else {
				gridClaimAdjustments.Title="Claim Adjustments";
			}
			gridClaimAdjustments.BeginUpdate();
			gridClaimAdjustments.Columns.Clear();
			const int colWidthDescription=200;
			const int colWidthAdjAmt=80;
			int colWidthVariable=gridClaimAdjustments.Width-10-colWidthDescription-colWidthAdjAmt;
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Description",colWidthDescription,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridClaimAdjustments.Rows.Clear();
			_claimAdjAmtSum=0;
			for(int i=0;i<_listClaimAdjustments.Count;i+=4) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(_listClaimAdjustments[i]));//Description
				row.Cells.Add(new ODGridCell(_listClaimAdjustments[i+1]));//Reason
				row.Cells.Add(new ODGridCell(_listClaimAdjustments[i+2]));//AdjAmt
				decimal adjAmt=PIn.Decimal(_listClaimAdjustments[i+2]);
				_claimAdjAmtSum+=adjAmt;
				gridClaimAdjustments.Rows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
			textClaimAdjAmtSum.Text=_claimAdjAmtSum.ToString("f2");
		}

		private void FillProcedureBreakdown() {
			_listProcInfo=_x835.GetProcInfo(_claimTrackingNumber);
			if(_listProcInfo.Count==0) {
				gridProcedureBreakdown.Title="Procedure Breakdown (None Reported)";
			}
			else {
				gridProcedureBreakdown.Title="Procedure Breakdown";
			}
			gridProcedureBreakdown.BeginUpdate();
			const int colWidthProcNum=80;
			const int colWidthProcCode=80;
			const int colWidthProcFee=80;
			const int colWidthAdjAmt=80;
			const int colWidthInsPaid=80;
			int colWidthVariable=gridProcedureBreakdown.Width-10-colWidthProcNum-colWidthProcCode-colWidthProcFee-4*colWidthAdjAmt-colWidthInsPaid;
			gridProcedureBreakdown.Columns.Clear();
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcNum",colWidthProcNum,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcCode",colWidthProcCode,HorizontalAlignment.Center));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcDescript",colWidthVariable,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcFee",colWidthProcFee,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PatPortion",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ContractOblig",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PayorReduct",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("OtherAdjust",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("InsPaid",colWidthInsPaid,HorizontalAlignment.Right));
			gridProcedureBreakdown.Rows.Clear();
			_procAdjAmtSum=0;
			for(int i=0;i<_listProcInfo.Count;i+=5) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(_listProcInfo[i+4]));//ProcNum
				string strProcCode=_listProcInfo[i+1];
				row.Cells.Add(new ODGridCell(strProcCode));//ProcCode
				string procDescript="";
				if(ProcedureCodes.IsValidCode(strProcCode)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(strProcCode);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//ProcDescript
				row.Cells.Add(new ODGridCell(_listProcInfo[i+2]));//ProcFee
				int segSvcIndex=PIn.Int(_listProcInfo[i]);
				List<string> listProcAdjustments=_x835.GetProcAdjustmentInfo(segSvcIndex);
				decimal adjAmtForProc=0;
				decimal patPortionForProc=0;
				decimal contractObligForProc=0;
				decimal payorInitReductForProc=0;
				decimal otherAdjustForProc=0;
				for(int j=0;j<listProcAdjustments.Count;j+=4) {
					decimal adjAmt=PIn.Decimal(listProcAdjustments[j+2]);
					if(listProcAdjustments[j+3]=="PR") {//Patient Responsibility
						patPortionForProc+=adjAmt;
					}
					else if(listProcAdjustments[j+3]=="CO") {//Contractual Obligations
						contractObligForProc+=adjAmt;
					}
					else if(listProcAdjustments[j+3]=="PI") {//Payor Initiated Reductions
						payorInitReductForProc+=adjAmt;
					}
					else {//Other Adjustments
						otherAdjustForProc+=adjAmt;
					}
					adjAmtForProc+=adjAmt;
					_procAdjAmtSum+=adjAmt;
				}
				row.Cells.Add(new ODGridCell(patPortionForProc.ToString("f2")));//PatPortion
				row.Cells.Add(new ODGridCell(contractObligForProc.ToString("f2")));//ContractOblig
				row.Cells.Add(new ODGridCell(payorInitReductForProc.ToString("f2")));//PayorReduct
				row.Cells.Add(new ODGridCell(otherAdjustForProc.ToString("f2")));//OtherAdjust
				row.Cells.Add(new ODGridCell(_listProcInfo[i+3]));//InsPaid
				row.Tag=i;
				gridProcedureBreakdown.Rows.Add(row);
			}
			gridProcedureBreakdown.EndUpdate();
			textProcAdjAmtSum.Text=_procAdjAmtSum.ToString("f2");
		}

		private void FillAdjudicationInfo() {
			_listAdjudicationInfo=_x835.GetClaimAdjudicationInfo(_claimTrackingNumber);
			if(_listAdjudicationInfo.Count==0) {
				gridAdjudicationInfo.Title="Claim Adjudication Info (None Reported)";
			}
			else {
				gridAdjudicationInfo.Title="Claim Adjudication Info";
			}
			gridAdjudicationInfo.BeginUpdate();
			gridAdjudicationInfo.Columns.Clear();
			gridAdjudicationInfo.Columns.Add(new UI.ODGridColumn("Description",gridAdjudicationInfo.Width/2,HorizontalAlignment.Left));
			gridAdjudicationInfo.Columns.Add(new UI.ODGridColumn("Value",gridAdjudicationInfo.Width/2,HorizontalAlignment.Left));
			gridAdjudicationInfo.Rows.Clear();
			for(int i=0;i<_listAdjudicationInfo.Count;i+=2) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new UI.ODGridCell(_listAdjudicationInfo[i]));
				row.Cells.Add(new UI.ODGridCell(_listAdjudicationInfo[i+1]));
				gridAdjudicationInfo.Rows.Add(row);
			}
			gridAdjudicationInfo.EndUpdate();
		}

		private void gridProcedureBreakdown_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int procInfoIndex=(int)gridProcedureBreakdown.Rows[e.Row].Tag;
			int strSegSvcIndex=PIn.Int(_listProcInfo[procInfoIndex]);
			string strProcNum=_listProcInfo[procInfoIndex+4];
			string strProcCode=_listProcInfo[procInfoIndex+1];
			string strProcFee=_listProcInfo[procInfoIndex+2];
			string strInsPaid=_listProcInfo[procInfoIndex+3];
			FormEtrans835ProcEdit form=new FormEtrans835ProcEdit(_x835,strSegSvcIndex,strProcNum,strProcCode,strProcFee,strInsPaid);
			form.Show();
		}

		private void gridAdjudicationInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_listAdjudicationInfo[e.Row*2+1]);
			msgbox.Show(this);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}