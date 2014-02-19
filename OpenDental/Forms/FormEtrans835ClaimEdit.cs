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
			FillAdjudicationDetails();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillHeader();
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

		private void FillAdjudicationDetails() {
			gridAdjudicationDetails.BeginUpdate();
			gridAdjudicationDetails.Columns.Clear();
			gridAdjudicationDetails.Columns.Add(new UI.ODGridColumn("Description",gridAdjudicationDetails.Width/2,HorizontalAlignment.Left));
			gridAdjudicationDetails.Columns.Add(new UI.ODGridColumn("Value",gridAdjudicationDetails.Width/2,HorizontalAlignment.Left));
			gridAdjudicationDetails.Rows.Clear();
			List<string> listAdjudicationDetails=_x835.GetClaimAdjudicationInfo(_claimTrackingNumber);
			for(int i=0;i<listAdjudicationDetails.Count;i+=2) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new UI.ODGridCell(listAdjudicationDetails[i]));
				row.Cells.Add(new UI.ODGridCell(listAdjudicationDetails[i+1]));
				gridAdjudicationDetails.Rows.Add(row);
			}
			gridAdjudicationDetails.EndUpdate();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
		
	}
}