using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Text.RegularExpressions;

namespace OpenDental {
	public partial class FormEtrans835ClaimSelect:Form {

		///<summary>The selected patient.  Used to get list of claims to fill grid.</summary>
		private long _patNum;
		///<summary>The claim the user selected from the grid.</summary>
		private Claim _claimSelected=null;
		private Hx835_Claim _x835Claim;

		///<summary>The claim the user selected from the grid.</summary>
		public Claim ClaimSelected {
        get { return _claimSelected; }
    }

		///<summary>PatNum used to get claims to fill grid.  x835Claim used to fill default text for date and claim fee filters and disallow OK click if 
		///claim details do not match.</summary>
		public FormEtrans835ClaimSelect(long patNum,Hx835_Claim x835Claim) {
			InitializeComponent();
			Lan.F(this);
			_x835Claim=x835Claim;
			_patNum=patNum;
			textPatient.Text=Patients.GetLim(_patNum).GetNameLF();
			textClaimFee.Text=x835Claim.ClaimFee.ToString();
			textDateFrom.Text=x835Claim.DateServiceStart.ToShortDateString();
			textDateTo.Text=x835Claim.DateServiceEnd.ToShortDateString();
		}

		private void FormEtrans835ClaimSelect_Load(object sender,EventArgs e) {
			FillGridClaims();
			HighlightRows();
		}

		///<summary>Gets all claims for the patient selected.  Fills gridClaims and tags each row with its corrisponding claim object.</summary>
		private void FillGridClaims() {
			int sortByColIdx=gridClaims.SortedByColumnIdx;  //Keep previous sorting
			bool isSortAsc=gridClaims.SortedIsAscending;
			if(sortByColIdx==-1) {
				sortByColIdx=0;
				isSortAsc=false;
			}
			gridClaims.BeginUpdate();
			gridClaims.Rows.Clear();
			gridClaims.Columns.Clear();
			gridClaims.Columns.Add(new UI.ODGridColumn("Date Service",100,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.DateParse });
			gridClaims.Columns.Add(new UI.ODGridColumn("Carrier",240,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridClaims.Columns.Add(new UI.ODGridColumn("Status",120,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });			
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				gridClaims.Columns.Add(new UI.ODGridColumn("Clinic",190,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			}
			gridClaims.Columns.Add(new UI.ODGridColumn("ClaimFee",70,HorizontalAlignment.Right) { SortingStrategy=UI.GridSortingStrategy.AmountParse });
			List<Claim> listClaims=Claims.Refresh(_patNum);
			for(int i=0;i<listClaims.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=listClaims[i];
				row.Cells.Add(listClaims[i].DateService.ToShortDateString());//DOS
				row.Cells.Add(Carriers.GetName(InsPlans.RefreshOne(listClaims[i].PlanNum).CarrierNum));//Carrier
				row.Cells.Add(Claims.GetClaimStatusString(listClaims[i].ClaimStatus));//Status
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
					Clinic clinic=Clinics.GetClinic(listClaims[i].ClinicNum);
					if(clinic==null) {
						row.Cells.Add("");//Clinic
					}
					else {
						row.Cells.Add(clinic.Description);//Clinic
					}
				}
				row.Cells.Add(listClaims[i].ClaimFee.ToString("f"));//Claimfee
				gridClaims.Rows.Add(row);
			}
			gridClaims.EndUpdate();
			gridClaims.SortForced(sortByColIdx,isSortAsc);
		}

		///<summary>Sets the foreground text to red if any row has a DOS between textDOSFrom and textDOSTo and matches textClaimFee </summary>
		private void HighlightRows() {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			double fee=PIn.Double(textClaimFee.Text);
			int rowsHighlightCount=0;
			int lastHighlightIndex=0;
			gridClaims.BeginUpdate();
			for(int i=0;i<gridClaims.Rows.Count;i++) {
				gridClaims.Rows[i].ColorText=Color.Black;  //reset row highlighting
				gridClaims.Rows[i].Bold=false;  //reset row highlighting
				Claim claim=(Claim)gridClaims.Rows[i].Tag;
				YN isFeeMatch=YN.No;  //If fee matches then yes, if fee doesnt match then no, if no fee entered then unknown
				YN isDateMatch=YN.No; //If both dates match then yes, if both dates dont match then no, if no dates entered then unknown
				//Check fee
				if(textClaimFee.Text==""){  //No fee entered
					isFeeMatch=YN.Unknown;
				}
				else {
					if(claim.ClaimFee.ToString("f").Contains(textClaimFee.Text)){
						isFeeMatch=YN.Yes;
					}
				}
				//Check date
				if(dateFrom==DateTime.MinValue && dateTo==DateTime.MinValue) {  //No dates entered
					isDateMatch=YN.Unknown;
				}
				else {  //At least one date entered
					if((dateFrom.CompareTo(claim.DateService)<=0 || dateFrom==DateTime.MinValue) 
						&& (dateTo.CompareTo(claim.DateService)>=0 || dateTo==DateTime.MinValue)) {
							isDateMatch=YN.Yes;
					}
				}
				if((isFeeMatch==YN.Yes || isDateMatch==YN.Yes) && (isFeeMatch!=YN.No && isDateMatch!=YN.No)) { //If either match and neither don't match
					//Highlight row
					gridClaims.Rows[i].ColorText=Color.Red;
					gridClaims.Rows[i].Bold=true;
					rowsHighlightCount++;
					lastHighlightIndex=i;
				}
			}
			gridClaims.EndUpdate();
			if(rowsHighlightCount==1) {
				gridClaims.SetSelected(lastHighlightIndex,true);
			}
		}

		private void butPatFind_Click(object sender,EventArgs e) {
			FormPatientSelect formP=new FormPatientSelect();
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			_patNum=formP.SelectedPatNum;
			textPatient.Text=Patients.GetLim(_patNum).GetNameLF();
			FillGridClaims();
		}

		private void gridClaims_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FormClaimEdit formCE=new FormClaimEdit((Claim)gridClaims.Rows[e.Row].Tag,Patients.GetPat(_patNum),Patients.GetFamily(_patNum));
			formCE.ShowDialog();
			Claim claim=Claims.GetClaim(((Claim)gridClaims.Rows[e.Row].Tag).ClaimNum);//This is the easiest way to determine if the claim was deleted.
			if(claim==null) {//Was deleted.
				gridClaims.BeginUpdate();
				gridClaims.Rows.RemoveAt(e.Row);//This will also deselect the row.
				gridClaims.EndUpdate();
				return;
			}
			if(formCE.DialogResult==DialogResult.OK) {
				//Update row
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=claim;
				row.Cells.Add(claim.DateService.ToShortDateString());//DOS
				row.Cells.Add(Carriers.GetName(InsPlans.RefreshOne(claim.PlanNum).CarrierNum));//Carrier
				row.Cells.Add(Claims.GetClaimStatusString(claim.ClaimStatus));//Status
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
					Clinic clinic=Clinics.GetClinic(claim.ClinicNum);
					if(clinic==null) {
						row.Cells.Add("");//Clinic
					}
					else {
						row.Cells.Add(clinic.Description);//Clinic
					}
				}
				row.Cells.Add(claim.ClaimFee.ToString("f"));//Claimfee
				gridClaims.BeginUpdate();
				gridClaims.Rows[e.Row]=row;
				gridClaims.EndUpdate();
				gridClaims.SetSelected(e.Row,true);//Reselect Row
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			HighlightRows();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridClaims.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"You must select a claim.");
				return;
			}
			Claim claimSelected=(Claim)gridClaims.Rows[gridClaims.GetSelectedIndex()].Tag;
			double claimFee835=(double)_x835Claim.ClaimFee;
			if(!claimSelected.ClaimFee.IsEqual(claimFee835)) {
				MessageBox.Show(Lan.g(this,"Claim fee on claim does not match ERA.")+"  "+Lan.g(this,"Expected")+" "+claimFee835.ToString("f"));
				return;
			}
			if((claimSelected.DateService.Date.CompareTo(_x835Claim.DateServiceStart.Date) < 0)
				|| (claimSelected.DateService.Date.CompareTo(_x835Claim.DateServiceEnd.Date) > 0))
			{
				MessageBox.Show(Lan.g(this,"Date of service on claim does not match service date range on ERA.")+"\r\n"+Lan.g(this,"Expected")+" "
					+_x835Claim.DateServiceStart.ToShortDateString()+" - "+_x835Claim.DateServiceEnd.ToShortDateString());
				return;
			}
			_claimSelected=claimSelected;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}