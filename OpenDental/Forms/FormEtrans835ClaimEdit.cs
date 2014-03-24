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

		private Hx835_Claim _claimEob;
		private Claim _claim;
		private decimal _claimAdjAmtSum;
		private decimal _procAdjAmtSum;

		public FormEtrans835ClaimEdit(Hx835_Claim claimEob) {
			InitializeComponent();
			Lan.F(this);
			_claimEob=claimEob;
		}

		private void FormEtrans835ClaimEdit_Load(object sender,EventArgs e) {
			long claimNum=Claims.GetClaimNumForIdentifier(_claimEob.ClaimTrackingNumber);
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
			if(_claimEob.Npi!="") {
				Text+=" - NPI: "+_claimEob.Npi;
			}
			Text+=" - Patient: "+_claimEob.PatientName;
			textSubscriberName.Text=_claimEob.SubscriberName;
			textPatientName.Text=_claimEob.PatientName;
			if(_claim==null) {
				textDateService.Text="";
			}
			else {
				textDateService.Text=_claim.DateService.ToShortDateString();
			}
			textClaimIdentifier.Text=_claimEob.ClaimTrackingNumber;
			textPayorControlNum.Text=_claimEob.PayorControlNumber;
			textStatus.Text=_claimEob.StatusCodeDescript;
			textClaimFee.Text=_claimEob.ClaimFee.ToString("f2");
			textClaimFee2.Text=_claimEob.ClaimFee.ToString("f2");
			textInsPaid.Text=_claimEob.InsPaid.ToString("f2");
			textInsPaidCalc.Text=(_claimEob.ClaimFee-_claimAdjAmtSum-_procAdjAmtSum).ToString("f2");
			textPatientPortion.Text=_claimEob.PatientPortion.ToString("f2");
			if(_claimEob.DatePayerReceived.Year>1880) {
				textDatePayerReceived.Text=_claimEob.DatePayerReceived.ToShortDateString();
			}
		}

		private void FillClaimAdjustments() {
			if(_claimEob.ListClaimAdjustments.Count==0) {
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
			for(int i=0;i<_claimEob.ListClaimAdjustments.Count;i++) {
				Hx835_Adj adj=_claimEob.ListClaimAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=adj;
				row.Cells.Add(new ODGridCell(adj.AdjustDescript));//Description
				row.Cells.Add(new ODGridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new ODGridCell(adj.AdjAmt.ToString("f2")));//AdjAmt
				_claimAdjAmtSum+=_claimEob.ListClaimAdjustments[i].AdjAmt;
				gridClaimAdjustments.Rows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
			textClaimAdjAmtSum.Text=_claimAdjAmtSum.ToString("f2");
		}

		private void FillProcedureBreakdown() {
			if(_claimEob.ListProcs.Count==0) {
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
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Contractual",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PayorReduct",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("OtherAdjust",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("InsPaid",colWidthInsPaid,HorizontalAlignment.Right));
			gridProcedureBreakdown.Rows.Clear();
			_procAdjAmtSum=0;
			for(int i=0;i<_claimEob.ListProcs.Count;i++) {
				Hx835_Proc proc=_claimEob.ListProcs[i];
				ODGridRow row=new ODGridRow();
				row.Tag=proc;
				row.Cells.Add(new ODGridCell(proc.ProcNum.ToString()));//ProcNum
				row.Cells.Add(new ODGridCell(proc.ProcCode));//ProcCode
				string procDescript="";
				if(ProcedureCodes.IsValidCode(proc.ProcCode)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.ProcCode);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//ProcDescript
				row.Cells.Add(new ODGridCell(proc.ProcFee.ToString("f2")));//ProcFee
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
			if(_claimEob.ListAdjudicationInfo.Count==0) {
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
			for(int i=0;i<_claimEob.ListAdjudicationInfo.Count;i++) {
				Hx835_Info info=_claimEob.ListAdjudicationInfo[i];
				ODGridRow row=new ODGridRow();
				row.Tag=info;
				row.Cells.Add(new UI.ODGridCell(info.FieldName));//Description
				row.Cells.Add(new UI.ODGridCell(info.FieldValue));//Value
				gridAdjudicationInfo.Rows.Add(row);
			}
			gridAdjudicationInfo.EndUpdate();
		}

		private void FillSupplementalInfo() {
			if(_claimEob.ListSupplementalInfo.Count==0) {
				gridSupplementalInfo.Title="Supplemental Info (None Reported)";
			}
			else {
				gridSupplementalInfo.Title="Supplemental Info";
			}
			gridSupplementalInfo.BeginUpdate();
			gridSupplementalInfo.Columns.Clear();
			const int colWidthAmt=80;
			int colWidthVariable=gridSupplementalInfo.Width-10-colWidthAmt;
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Description",colWidthVariable,HorizontalAlignment.Left));
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Amt",colWidthAmt,HorizontalAlignment.Right));
			gridSupplementalInfo.Rows.Clear();
			for(int i=0;i<_claimEob.ListSupplementalInfo.Count;i++) {
				Hx835_Info info=_claimEob.ListSupplementalInfo[i];
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
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(adj.AdjCode+" "+adj.AdjustDescript+"\r\r"+adj.ReasonDescript+"\r\n"+adj.AdjAmt.ToString("f2"));
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

		private void butEditClaim_Click(object sender,EventArgs e) {
			if(_claim==null) {
				MsgBox.Show(this,"The original claim could not be located for the given claim identifier.");
				return;
			}
			Patient pat=Patients.GetPat(_claim.PatNum);
			Family fam=Patients.GetFamily(_claim.PatNum);
			FormClaimEdit formCE=new FormClaimEdit(_claim,pat,fam);
			formCE.ShowDialog();//Non-modal might be nice here, but would require a change in logic within the OK/Cancel button clicks inside of FormClaimEdit.
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}