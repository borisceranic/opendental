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
			textInsPaid.Text=_claimInfo[2];
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
			for(int i=0;i<_listAdjustmentDetails.Count;i+=3) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i]));//Description
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i+1]));//Reason
				row.Cells.Add(new ODGridCell(_listAdjustmentDetails[i+2]));//Amount
				gridAdjustmentDetails.Rows.Add(row);
			}
			gridAdjustmentDetails.EndUpdate();
		}

		private void FillProcedureDetails() {
			List<string> listProcedureDetails=new List<string>();//TODO: Get data from x835.
			if(listProcedureDetails.Count==0) {
				gridProcedureDetails.Title="Procedure Details (None Reported)";
			}
			else {
				gridProcedureDetails.Title="Procedure Details";
			}
			//TODO:
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