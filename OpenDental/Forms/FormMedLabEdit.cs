using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Drawing;

namespace OpenDental {
	public partial class FormMedLabEdit:Form {
		public Patient PatCur;
		public List<MedLab> ListMedLabs;
		private List<MedLabResult> _listResults;

		public FormMedLabEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabEdit_Load(object sender,EventArgs e) {
			//Since all MedLabs in ListMedLabs have the same SpecimenID and SpecimenIDFiller, it is safe to assume all MedLab objects have
			//the same value for some of the fields and we will just pull from the first MedLab in the list.
			#region Patient Group Box
			textSpecimenNumber.Text=ListMedLabs[0].PatIDLab;
			textPatID.Text=PatCur.PatNum.ToString();
			textPatLName.Text=PatCur.LName;
			textPatFName.Text=PatCur.FName;
			textPatMiddleI.Text=PatCur.MiddleI;
			textPatSSN.Text="****-**-"+PatCur.SSN.PadLeft(4,' ').Substring(PatCur.SSN.PadLeft(4,' ').Length-4,4);//mask all but the last 4 digits. Ex: ****-**-1234
			textBirthdate.Text=PatCur.Birthdate.ToShortDateString();
			if(PatCur.Birthdate.Year < 1880) {
				textBirthdate.Text="";
			}
			textPatAge.Text=ListMedLabs[0].PatAge;
			textGender.Text=PatCur.Gender.ToString();
			textFasting.Text=ListMedLabs[0].PatFasting.ToString();
			if(ListMedLabs[0].PatFasting==YN.Unknown) {
				textFasting.Text="";
			}
			#endregion Patient Group Box
			#region Patient Address and Phone Group Box
			textAddress.Text=PatCur.Address;
			textAddress2.Text=PatCur.Address2;
			textCity.Text=PatCur.City;
			textState.Text=PatCur.State;
			textZip.Text=PatCur.Zip;
			textPatPhone.Text=PatCur.HmPhone;
			#endregion Patient Address and Phone Group Box
			textDateTCollect.Text=ListMedLabs[0].DateTimeCollected.ToString();
			textDateEntered.Text=ListMedLabs[0].DateTimeEntered.ToShortDateString();
			textDateTReport.Text=ListMedLabs[0].DateTimeReported.ToString();
			textTotVol.Text=ListMedLabs[0].TotalVolume;
			textClientAcc.Text=ListMedLabs[0].SpecimenID;
			textClientAltPatID.Text=PatCur.PatNum.ToString();
			//textTestsOrd.Text=ListMedLabs.ObsTestDescript;
			textControlNum.Text=ListMedLabs[0].SpecimenIDAlt;
			textAccountNum.Text=ListMedLabs[0].PatAccountNum;
			textAccountPh.Text=PrefC.GetString(PrefName.PracticePhone);
			#region Account Address Group Box
			//use practice billing address information if stored, otherwise practice address information
			if(PrefC.GetString(PrefName.PracticeBillingAddress)=="") {
				textAcctAddr.Text=PrefC.GetString(PrefName.PracticeAddress);
				textAcctAddr2.Text=PrefC.GetString(PrefName.PracticeAddress2);
				textAcctCity.Text=PrefC.GetString(PrefName.PracticeCity);
				textAcctState.Text=PrefC.GetString(PrefName.PracticeST);
				textAcctZip.Text=PrefC.GetString(PrefName.PracticeZip);
			}
			else {
				textAcctAddr.Text=PrefC.GetString(PrefName.PracticeBillingAddress);
				textAcctAddr2.Text=PrefC.GetString(PrefName.PracticeBillingAddress2);
				textAcctCity.Text=PrefC.GetString(PrefName.PracticeBillingCity);
				textAcctState.Text=PrefC.GetString(PrefName.PracticeBillingST);
				textAcctZip.Text=PrefC.GetString(PrefName.PracticeBillingZip);
			}
			#endregion Account Address Group Box
			textAddlInfo.Text=ListMedLabs[0].ClinicalInfo;
			#region Odering Physician Group Box
			textPhysicianName.Text=ListMedLabs[0].OrderingProvLName;
			if(ListMedLabs[0].OrderingProvLName!="") {
				textPhysicianName.Text+=", ";
			}
			textPhysicianName.Text+=ListMedLabs[0].OrderingProvFName;
			textPhysicianNPI.Text=ListMedLabs[0].OrderingProvNPI;
			textPhysicianID.Text=ListMedLabs[0].OrderingProvLocalID;
			#endregion Odering Physician Group Box
			//textGenComments.Text=ListMedLabs.NoteLab;
			FillGridResults();
			FillGridFacilities();
		}

		private void FillGridResults() {
			gridResults.BeginUpdate();
			gridResults.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Test",275);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Result",220);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Flag",70);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Units",135);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Reference Interval",175);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Lab",55);
			gridResults.Columns.Add(col);
			gridResults.Rows.Clear();
			ODGridRow row;
			RefreshResults();
			for(int i=0;i<_listResults.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listResults[i].ObsText);
				row.Cells.Add(_listResults[i].ObsValue);
				row.Cells.Add(MedLabResults.GetAbnormalFlagDescript(_listResults[i].AbnormalFlag));
				row.Cells.Add(_listResults[i].ObsUnits);
				row.Cells.Add(_listResults[i].ReferenceRange);
				row.Cells.Add(_listResults[i].FacilityID);
				gridResults.Rows.Add(row);
			}
			gridResults.EndUpdate();
		}

		private void FillGridFacilities() {
			if(_listResults==null) {
				RefreshResults();
			}
			gridFacilities.BeginUpdate();
			gridFacilities.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("ID",40);//Facility ID from the MedLabResult
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Name",200);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Address",165);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("City",90);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("State",35);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Zip",70);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Phone",130);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Director",200);//FName LName, Title
			gridFacilities.Columns.Add(col);
			gridFacilities.Rows.Clear();
			ODGridRow row;
			Dictionary<long,string> dictFacNumFacId=new Dictionary<long,string>();//links a FacilityNum to a facility ID
			for(int i=0;i<_listResults.Count;i++) {
				List<MedLabFacAttach> listFacAttaches=MedLabFacAttaches.GetAllForLabOrResult(0,_listResults[i].MedLabResultNum);
				for(int j=0;j<listFacAttaches.Count;j++) {
					MedLabFacility facilityCur=MedLabFacilities.GetOne(listFacAttaches[j].MedLabFacilityNum);
					if(dictFacNumFacId.ContainsKey(facilityCur.MedLabFacilityNum)) {
						if(dictFacNumFacId[facilityCur.MedLabFacilityNum]!=_listResults[i].FacilityID) {

						}
						continue;
					}
					else {
						dictFacNumFacId.Add(facilityCur.MedLabFacilityNum,_listResults[i].FacilityID);
					}
					row=new ODGridRow();
					row.Cells.Add(dictFacNumFacId[facilityCur.MedLabFacilityNum]);
					row.Cells.Add(facilityCur.FacilityName);
					row.Cells.Add(facilityCur.Address);
					row.Cells.Add(facilityCur.City);
					row.Cells.Add(facilityCur.State);
					row.Cells.Add(facilityCur.Zip);
					row.Cells.Add(facilityCur.Phone);
					string directorName=facilityCur.DirectorFName;
					if(facilityCur.DirectorFName!="" && facilityCur.DirectorLName!="") {
						directorName+=" ";
					}
					directorName+=facilityCur.DirectorLName;
					if(directorName!="" && facilityCur.DirectorTitle!="") {
						directorName+=", "+facilityCur.DirectorTitle;
					}
					row.Cells.Add(directorName);//could be blank
					gridFacilities.Rows.Add(row);
				}
			}
			gridFacilities.EndUpdate();
		}

		///<summary>Fills _listResults with the most recent and/or most final results for ListMedLabs.</summary>
		private void RefreshResults() {
			_listResults=MedLabResults.GetAllForLabs(ListMedLabs);
			for(int i=_listResults.Count-1;i>-1;i--) {//loop through backward and only keep the most final/most recent result
				if(i==0) {
					break;
				}
				if(_listResults[i].ObsID==_listResults[i-1].ObsID && _listResults[i].ObsIDSub==_listResults[i-1].ObsIDSub) {
					_listResults.RemoveAt(i);
				}
			}
			_listResults.Sort(SortResultsByPriKey);
		}

		///<summary>Sort by MedLabResult.MedLabResultNum.</summary>
		private static int SortResultsByPriKey(MedLabResult medLabResultX,MedLabResult medLabResultY) {
			return medLabResultX.MedLabResultNum.CompareTo(medLabResultY.MedLabResultNum);
		}

		private void butPatSelect_Click(object sender,EventArgs e) {

		}

		private void butProvSelect_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormPP.SelectedProvNum!=ListMedLabs[0].ProvNum) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Update all lab tests and results for this specimen with the selected ordering provider?")) {
					return;
				}
			}
			Provider prov=Providers.GetProv(FormPP.SelectedProvNum);
			for(int i=0;i<ListMedLabs.Count;i++) {
				ListMedLabs[i].OrderingProvLName=prov.LName;
				ListMedLabs[i].OrderingProvFName=prov.FName;
				ListMedLabs[i].OrderingProvNPI=prov.NationalProvID;
				ListMedLabs[i].OrderingProvLocalID=prov.ProvNum.ToString();
				ListMedLabs[i].ProvNum=prov.ProvNum;
				MedLabs.Update(ListMedLabs[i]);
			}
			string provName=prov.LName;
			if(provName!="" && prov.FName!="") {
				provName+=", ";
			}
			provName+=prov.FName;
			textPhysicianName.Text=provName;
			textPhysicianNPI.Text=prov.NationalProvID;
			textPhysicianID.Text=prov.ProvNum.ToString();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			SheetDef sheetDef=SheetUtil.GetMedLabResultsSheetDef();
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,PatCur.PatNum);
			SheetFiller.FillFields(sheet,null,ListMedLabs[0]);
			SheetUtil.CalculateHeights(sheet,Graphics.FromImage(new Bitmap(sheet.HeightPage,sheet.WidthPage)),null,true,120,60,ListMedLabs[0]);
			//print directly to PDF here, and save it.
			FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheet);
			FormSFE.MedLabCur=ListMedLabs[0];
			FormSFE.IsStatement=true;
			FormSFE.ShowDialog();
		}

		private void butShowHL7_Click(object sender,EventArgs e) {

		}

		private void butDelete_Click(object sender,EventArgs e) {

		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}