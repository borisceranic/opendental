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

		private Hx835_Claim _claimPaid;
		private Claim _claim;
		private decimal _claimAdjAmtSum;
		private decimal _procAdjAmtSum;

		public FormEtrans835ClaimEdit(Hx835_Claim claimPaid) {
			InitializeComponent();
			Lan.F(this);
			_claimPaid=claimPaid;
		}

		private void FormEtrans835ClaimEdit_Load(object sender,EventArgs e) {
			long claimNum=_claimPaid.ClaimNum;
			_claim=null;
			if(claimNum!=0) {
				_claim=Claims.GetClaim(claimNum);
			}
			FillAll();
		}

		private void FormEtrans835ClaimEdit_Resize(object sender,EventArgs e) {
			FillClaimAdjustments();//Because the grid columns change size depending on the form size.
			FillProcedureBreakdown();//Because the grid columns change size depending on the form size.
			FillAdjudicationInfo();//Because the grid columns change size depending on the form size.
			FillSupplementalInfo();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillClaimAdjustments();
			FillProcedureBreakdown();
			FillAdjudicationInfo();
			FillSupplementalInfo();
			FillHeader();//Must be last, so internal summations are complete before filling totals in textboxes.
		}

		private void FillHeader() {
			Text="Claim Explanation of Benefits (EOB)";
			if(_claimPaid.Npi!="") {
				Text+=" - NPI: "+_claimPaid.Npi;
			}
			Text+=" - Patient: "+_claimPaid.PatientName;
			textSubscriberName.Text=_claimPaid.SubscriberName.ToString();
			textPatientName.Text=_claimPaid.PatientName.ToString();
			textDateService.Text=_claimPaid.DateServiceStart.ToShortDateString();
			if(_claimPaid.DateServiceEnd>_claimPaid.DateServiceStart) {
				textDateService.Text+=" to "+_claimPaid.DateServiceEnd.ToShortDateString();
				textDateService.Width=160;//Increase width to accout for extra text.
			}
			textClaimIdentifier.Text=_claimPaid.ClaimTrackingNumber;
			textPayorControlNum.Text=_claimPaid.PayerControlNumber;
			textStatus.Text=_claimPaid.StatusCodeDescript;
			textClaimFee.Text=_claimPaid.ClaimFee.ToString("f2");
			textClaimFee2.Text=_claimPaid.ClaimFee.ToString("f2");
			textInsPaid.Text=_claimPaid.InsPaid.ToString("f2");
			textInsPaidCalc.Text=(_claimPaid.ClaimFee-_claimAdjAmtSum-_procAdjAmtSum).ToString("f2");
			textPatientPortion.Text=_claimPaid.PatientPortion.ToString("f2");
			if(_claimPaid.DatePayerReceived.Year>1880) {
				textDatePayerReceived.Text=_claimPaid.DatePayerReceived.ToShortDateString();
			}
		}

		private void FillClaimAdjustments() {
			if(_claimPaid.ListClaimAdjustments.Count==0) {
				gridClaimAdjustments.Title="EOB Claim Adjustments (None Reported)";
			}
			else {
				gridClaimAdjustments.Title="EOB Claim Adjustments";
			}
			gridClaimAdjustments.BeginUpdate();
			gridClaimAdjustments.Columns.Clear();
			const int colWidthReason=507;
			const int colWidthAdjAmt=62;
			int colWidthVariable=gridClaimAdjustments.Width-10-colWidthReason-colWidthAdjAmt;
			//The size and order of the columns here mimics the EOB Claim Adjustments grid in FormEtrans835ClaimPay as close as possible.
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Reason",colWidthReason,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Remarks",colWidthVariable,HorizontalAlignment.Left));
			gridClaimAdjustments.Rows.Clear();
			_claimAdjAmtSum=0;
			for(int i=0;i<_claimPaid.ListClaimAdjustments.Count;i++) {
				Hx835_Adj adj=_claimPaid.ListClaimAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=adj;
				row.Cells.Add(new ODGridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new ODGridCell(adj.AdjAmt.ToString("f2")));//AdjAmt
				row.Cells.Add(new ODGridCell(adj.AdjustRemarks));//Remarks
				_claimAdjAmtSum+=_claimPaid.ListClaimAdjustments[i].AdjAmt;
				gridClaimAdjustments.Rows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
			textClaimAdjAmtSum.Text=_claimAdjAmtSum.ToString("f2");
		}

		private void FillProcedureBreakdown() {
			if(_claimPaid.ListProcs.Count==0) {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown (None Reported)";
			}
			else {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown";
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
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Fee Billed",colWidthProcFee,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PatPortion",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Contractual",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PayorReduct",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("OtherAdjust",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("InsPaid",colWidthInsPaid,HorizontalAlignment.Right));
			gridProcedureBreakdown.Rows.Clear();
			_procAdjAmtSum=0;
			for(int i=0;i<_claimPaid.ListProcs.Count;i++) {
				Hx835_Proc proc=_claimPaid.ListProcs[i];
				ODGridRow row=new ODGridRow();
				row.Tag=proc;
				if(proc.ProcNum==0) {
					row.Cells.Add(new ODGridCell(""));//ProcNum
				}
				else {
					row.Cells.Add(new ODGridCell(proc.ProcNum.ToString()));//ProcNum
				}
				row.Cells.Add(new ODGridCell(proc.ProcCodeAdjudicated));//ProcCode
				string procDescript="";
				if(ProcedureCodes.IsValidCode(proc.ProcCodeAdjudicated)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.ProcCodeAdjudicated);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//ProcDescript
				row.Cells.Add(new ODGridCell(proc.ProcFee.ToString("f2")));//Fee Billed
				decimal adjAmtForProc=0;
				decimal patPortionForProc=0;
				decimal contractualForProc=0;
				decimal payorInitReductForProc=0;
				decimal otherAdjustForProc=0;
				for(int j=0;j<proc.ListProcAdjustments.Count;j++) {
					Hx835_Adj adj=proc.ListProcAdjustments[j];
					if(adj.AdjCode=="PR") {//Patient Responsibility
						patPortionForProc+=adj.AdjAmt;
					}
					else if(adj.AdjCode=="CO") {//Contractual Obligations
						contractualForProc+=adj.AdjAmt;
					}
					else if(adj.AdjCode=="PI") {//Payor Initiated Reductions
						payorInitReductForProc+=adj.AdjAmt;
					}
					else {//Other Adjustments
						otherAdjustForProc+=adj.AdjAmt;
					}
					adjAmtForProc+=adj.AdjAmt;
					_procAdjAmtSum+=adj.AdjAmt;
				}
				row.Cells.Add(new ODGridCell(patPortionForProc.ToString("f2")));//PatPortion
				row.Cells.Add(new ODGridCell(contractualForProc.ToString("f2")));//Contractual
				row.Cells.Add(new ODGridCell(payorInitReductForProc.ToString("f2")));//PayorReduct
				row.Cells.Add(new ODGridCell(otherAdjustForProc.ToString("f2")));//OtherAdjust
				row.Cells.Add(new ODGridCell(proc.InsPaid.ToString("f2")));//InsPaid
				gridProcedureBreakdown.Rows.Add(row);
			}
			gridProcedureBreakdown.EndUpdate();
			textProcAdjAmtSum.Text=_procAdjAmtSum.ToString("f2");
		}

		private void FillAdjudicationInfo() {
			if(_claimPaid.ListAdjudicationInfo.Count==0) {
				gridAdjudicationInfo.Title="EOB Claim Adjudication Info (None Reported)";
			}
			else {
				gridAdjudicationInfo.Title="EOB Claim Adjudication Info";
			}
			gridAdjudicationInfo.BeginUpdate();
			gridAdjudicationInfo.Columns.Clear();
			gridAdjudicationInfo.Columns.Add(new UI.ODGridColumn("Description",gridAdjudicationInfo.Width/2,HorizontalAlignment.Left));
			gridAdjudicationInfo.Columns.Add(new UI.ODGridColumn("Value",gridAdjudicationInfo.Width/2,HorizontalAlignment.Left));
			gridAdjudicationInfo.Rows.Clear();
			for(int i=0;i<_claimPaid.ListAdjudicationInfo.Count;i++) {
				Hx835_Info info=_claimPaid.ListAdjudicationInfo[i];
				ODGridRow row=new ODGridRow();
				row.Tag=info;
				row.Cells.Add(new UI.ODGridCell(info.FieldName));//Description
				row.Cells.Add(new UI.ODGridCell(info.FieldValue));//Value
				gridAdjudicationInfo.Rows.Add(row);
			}
			gridAdjudicationInfo.EndUpdate();
		}

		private void FillSupplementalInfo() {
			if(_claimPaid.ListSupplementalInfo.Count==0) {
				gridSupplementalInfo.Title="EOB Supplemental Info (None Reported)";
			}
			else {
				gridSupplementalInfo.Title="EOB Supplemental Info";
			}
			gridSupplementalInfo.BeginUpdate();
			gridSupplementalInfo.Columns.Clear();
			const int colWidthAmt=80;
			int colWidthVariable=gridSupplementalInfo.Width-10-colWidthAmt;
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Description",colWidthVariable,HorizontalAlignment.Left));
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Amt",colWidthAmt,HorizontalAlignment.Right));
			gridSupplementalInfo.Rows.Clear();
			for(int i=0;i<_claimPaid.ListSupplementalInfo.Count;i++) {
				Hx835_Info info=_claimPaid.ListSupplementalInfo[i];
				ODGridRow row=new ODGridRow();
				row.Tag=info;
				row.Cells.Add(info.FieldName);//Description
				row.Cells.Add(info.FieldValue);//Amount
				gridSupplementalInfo.Rows.Add(row);
			}
			gridSupplementalInfo.EndUpdate();
		}

		private void gridClaimAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Adj adj=(Hx835_Adj)gridClaimAdjustments.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(adj.AdjCode+" "+adj.AdjustRemarks+"\r\r"+adj.ReasonDescript+"\r\n"+adj.AdjAmt.ToString("f2"));
			msgbox.Show(this);
		}

		private void gridProcedureBreakdown_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Proc proc=(Hx835_Proc)gridProcedureBreakdown.Rows[e.Row].Tag;
			FormEtrans835ProcEdit form=new FormEtrans835ProcEdit(proc);
			form.Show();
		}

		private void gridAdjudicationInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Info info=(Hx835_Info)gridAdjudicationInfo.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(info.FieldName+"\r\n"+info.FieldValue);
			msgbox.Show(this);
		}

		private void gridSupplementalInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Info info=(Hx835_Info)gridSupplementalInfo.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(info.FieldName+"\r\n"+info.FieldValue);
			msgbox.Show(this);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}