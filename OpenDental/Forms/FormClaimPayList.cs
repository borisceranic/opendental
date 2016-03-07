using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormClaimPayList:ODForm {
		List<ClaimPayment> ListClaimPay;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoClaimNum;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoPatNum;
		//<summary>Set to true if the batch list was accessed originally by going through a claim.  This disables the GotoAccount feature.</summary>
		//public bool IsFromClaim;

		public FormClaimPayList() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormClaimPayList_Load(object sender,EventArgs e) {
			textDateFrom.Text=DateTime.Now.AddMonths(-1).ToShortDateString();
			textDateTo.Text=DateTime.Now.ToShortDateString();
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				comboClinic.Items.Add("All");
				comboClinic.SelectedIndex=0;
				for(int i=0;i<Clinics.List.Length;i++) {
					comboClinic.Items.Add(Clinics.List[i].Description);
				}
			}
			FillGrid();
		}

		private void FillGrid(){
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			long clinicNum=0;
			if(comboClinic.SelectedIndex>0) {
				clinicNum=Clinics.List[comboClinic.SelectedIndex-1].ClinicNum;
			}
			ListClaimPay=ClaimPayments.GetForDateRange(dateFrom,dateTo,clinicNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),75,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Partial"),40,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Carrier"),200);
			gridMain.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				col=new ODGridColumn(Lan.g(this,"Clinic"),100);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Note"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Scanned"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);			
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListClaimPay.Count;i++){
				row=new ODGridRow();
				if(ListClaimPay[i].CheckDate.Year<1800) {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ListClaimPay[i].CheckDate.ToShortDateString());
				}
				row.Cells.Add(DefC.GetName(DefCat.InsurancePaymentType,ListClaimPay[i].PayType));
				row.Cells.Add(ListClaimPay[i].CheckAmt.ToString("c"));
				row.Cells.Add(ListClaimPay[i].IsPartial?"X":"");
				row.Cells.Add(ListClaimPay[i].CarrierName);
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					row.Cells.Add(Clinics.GetDesc(ListClaimPay[i].ClinicNum));
				}
				row.Cells.Add(ListClaimPay[i].Note);
				row.Cells.Add(EobAttaches.Exists(ListClaimPay[i].ClaimPaymentNum)?"X":"");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}
		
		private void butAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when saving the check to prevent backdating
				return;
			}
			ClaimPayment claimPayment=new ClaimPayment();
			claimPayment.CheckDate=DateTime.Now;
			claimPayment.IsPartial=true;
			FormClaimPayEdit FormCPE=new FormClaimPayEdit(claimPayment);
			FormCPE.IsNew=true;
			FormCPE.ShowDialog();
			if(FormCPE.DialogResult!=DialogResult.OK) {
				return;
			}
			FormClaimPayBatch FormCPB=new FormClaimPayBatch(claimPayment);
			//FormCPB.IsFromClaim=IsFromClaim;
			FormCPB.ShowDialog();
			if(IsDisposed) {//Auto-Logoff was causing an unhandled exception below.  Can't use dialogue result check here because we want to referesh the grid below even if user clicked cancel.
				return; //Don't refresh the grid, as the form is already disposed.
			}
			if(FormCPB.GotoClaimNum!=0) {
				GotoClaimNum=FormCPB.GotoClaimNum;
				GotoPatNum=FormCPB.GotoPatNum;
				Close();
			}
			else {
				FillGrid();
			}
		}               

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {
				return;
			}
			FormClaimPayBatch FormCPB=new FormClaimPayBatch(ListClaimPay[gridMain.GetSelectedIndex()]);
			//FormCPB.IsFromClaim=IsFromClaim;
			FormCPB.ShowDialog();
			if(FormCPB.GotoClaimNum!=0){
				GotoClaimNum=FormCPB.GotoClaimNum;
				GotoPatNum=FormCPB.GotoPatNum;
				Close();
			}
			else{
				FillGrid();
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}


	}
}