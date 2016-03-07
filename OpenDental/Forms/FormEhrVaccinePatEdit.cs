﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrVaccinePatEdit:ODForm {
		public VaccinePat VaccinePatCur;
		public bool IsNew;
		private long _provNumSelectedOrdering;
		private long _provNumSelectedAdministering;
		private List<VaccineObs> _listVaccineObservations;
		private List<VaccineObs> _listVaccineObservationGroups;

		public FormEhrVaccinePatEdit() {
			InitializeComponent();
		}

		private void FormVaccinePatEdit_Load(object sender,EventArgs e) {
			Patient pat=Patients.GetLim(VaccinePatCur.PatNum);
			if(pat.Age!=0 && pat.Age<3) {
				labelDocument.Text="Document reason not given below.  Reason can include a contraindication due to a specific allergy, adverse effect, intollerance, or specific disease.";//less leeway with reasons for kids.
			}
			comboVaccine.Items.Clear();
			for(int i=0;i<VaccineDefs.Listt.Count;i++) {
				comboVaccine.Items.Add(VaccineDefs.Listt[i].CVXCode + " - " + VaccineDefs.Listt[i].VaccineName);
				if(VaccineDefs.Listt[i].VaccineDefNum==VaccinePatCur.VaccineDefNum) {
					comboVaccine.SelectedIndex=i;
				}
			}
			if(!IsNew && VaccinePatCur.VaccineDefNum!=0) {
				VaccineDef vaccineDef=VaccineDefs.GetOne(VaccinePatCur.VaccineDefNum);//Need vaccine to get manufacturer.
				DrugManufacturer manufacturer=DrugManufacturers.GetOne(vaccineDef.DrugManufacturerNum);
				textManufacturer.Text=manufacturer.ManufacturerCode + " - " + manufacturer.ManufacturerName;
			}
			if(VaccinePatCur.DateTimeStart.Year>1880) {
				textDateTimeStart.Text=VaccinePatCur.DateTimeStart.ToString();
			}
			if(VaccinePatCur.DateTimeEnd.Year>1880) {
				textDateTimeStop.Text=VaccinePatCur.DateTimeEnd.ToString();
			}
			if(VaccinePatCur.AdministeredAmt!=0){
				textAmount.Text=VaccinePatCur.AdministeredAmt.ToString();
			}
			comboUnits.Items.Clear();
			comboUnits.Items.Add("none");
			comboUnits.SelectedIndex=0;
			for(int i=0;i<DrugUnits.Listt.Count;i++) {
				comboUnits.Items.Add(DrugUnits.Listt[i].UnitIdentifier);
				if(DrugUnits.Listt[i].DrugUnitNum==VaccinePatCur.DrugUnitNum) {
					comboUnits.SelectedIndex=i+1;
				}
			}
			textLotNum.Text=VaccinePatCur.LotNumber;
			if(VaccinePatCur.DateExpire.Year>1880) {
				textDateExpiration.Text=VaccinePatCur.DateExpire.ToShortDateString();
			}
			listRefusalReason.Items.Clear();
			string[] arrayVaccineRefusalReasons=Enum.GetNames(typeof(VaccineRefusalReason));
			for(int i=0;i<arrayVaccineRefusalReasons.Length;i++) {
				listRefusalReason.Items.Add(arrayVaccineRefusalReasons[i]);
				VaccineRefusalReason refusalReason=(VaccineRefusalReason)i;
				if(refusalReason==VaccinePatCur.RefusalReason) {
					listRefusalReason.SelectedIndex=i;
				}
			}
			listCompletionStatus.Items.Clear();
			string[] arrayCompletionStatuses=Enum.GetNames(typeof(VaccineCompletionStatus));
			for(int i=0;i<arrayCompletionStatuses.Length;i++) {
				listCompletionStatus.Items.Add(arrayCompletionStatuses[i]);
				VaccineCompletionStatus completionStatus=(VaccineCompletionStatus)i;
				if(completionStatus==VaccinePatCur.CompletionStatus) {
					listCompletionStatus.SelectedIndex=i;
				}
			}
			textNote.Text=VaccinePatCur.Note;
			if(IsNew) {
				if(pat.ClinicNum==0) {
					VaccinePatCur.FilledCity=PrefC.GetString(PrefName.PracticeCity);
					VaccinePatCur.FilledST=PrefC.GetString(PrefName.PracticeST);
				}
				else {
					Clinic clinic=Clinics.GetClinic(pat.ClinicNum);
					VaccinePatCur.FilledCity=clinic.City;
					VaccinePatCur.FilledST=clinic.State;
				}
			}
			textFilledCity.Text=VaccinePatCur.FilledCity;
			textFilledSt.Text=VaccinePatCur.FilledST;
			if(IsNew) {
				VaccinePatCur.UserNum=Security.CurUser.UserNum;
			}
			Userod user=Userods.GetUser(VaccinePatCur.UserNum);
			if(user!=null) {//Will be null for vaccines entered in older versions, before the UserNum column was created.
				textUser.Text=user.UserName;
			}
			_provNumSelectedOrdering=VaccinePatCur.ProvNumOrdering;
			comboProvNumOrdering.Items.Clear();
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				comboProvNumOrdering.Items.Add(ProviderC.ListShort[i].GetLongDesc());//Only visible provs added to combobox.
				if(ProviderC.ListShort[i].ProvNum==VaccinePatCur.ProvNumOrdering) {
					comboProvNumOrdering.SelectedIndex=i;//Sets combo text too.
				}
			}
			if(comboProvNumOrdering.SelectedIndex==-1) {//The provider exists but is hidden
				comboProvNumOrdering.Text=Providers.GetLongDesc(_provNumSelectedOrdering);//Appends "(hidden)" to the end of the long description.
			}
			_provNumSelectedAdministering=VaccinePatCur.ProvNumAdminister;
			comboProvNumAdministering.Items.Clear();
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				comboProvNumAdministering.Items.Add(ProviderC.ListShort[i].GetLongDesc());//Only visible provs added to combobox.
				if(ProviderC.ListShort[i].ProvNum==VaccinePatCur.ProvNumAdminister) {
					comboProvNumAdministering.SelectedIndex=i;//Sets combo text too.
				}
			}
			if(comboProvNumAdministering.SelectedIndex==-1) {//The provider exists but is hidden
				comboProvNumAdministering.Text=Providers.GetLongDesc(_provNumSelectedAdministering);//Appends "(hidden)" to the end of the long description.
			}
			comboAdministrationRoute.Items.Clear();
			string[] arrayVaccineAdministrationRoutes=Enum.GetNames(typeof(VaccineAdministrationRoute));
			for(int i=0;i<arrayVaccineAdministrationRoutes.Length;i++) {
				comboAdministrationRoute.Items.Add(arrayVaccineAdministrationRoutes[i]);
				VaccineAdministrationRoute administrationRoute=(VaccineAdministrationRoute)i;
				if(administrationRoute==VaccinePatCur.AdministrationRoute) {
					comboAdministrationRoute.SelectedIndex=i;
				}
			}
			comboAdministrationSite.Items.Clear();
			string[] arrayVaccineAdministrationSites=Enum.GetNames(typeof(VaccineAdministrationSite));
			for(int i=0;i<arrayVaccineAdministrationSites.Length;i++) {
				comboAdministrationSite.Items.Add(arrayVaccineAdministrationSites[i]);
				VaccineAdministrationSite administrationSite=(VaccineAdministrationSite)i;
				if(administrationSite==VaccinePatCur.AdministrationSite) {
					comboAdministrationSite.SelectedIndex=i;
				}
			}
			listAdministrationNote.Items.Clear();
			string[] arrayAdministrationNotes=Enum.GetNames(typeof(VaccineAdministrationNote));
			for(int i=0;i<arrayAdministrationNotes.Length;i++) {
				listAdministrationNote.Items.Add(arrayAdministrationNotes[i]);
				VaccineAdministrationNote administrationNote=(VaccineAdministrationNote)i;
				if(administrationNote==VaccinePatCur.AdministrationNoteCode) {
					listAdministrationNote.SelectedIndex=i;
				}
			}
			listAction.Items.Clear();
			string[] arrayVaccineActions=Enum.GetNames(typeof(VaccineAction));
			for(int i=0;i<arrayVaccineActions.Length;i++) {
				listAction.Items.Add(arrayVaccineActions[i]);
				VaccineAction action=(VaccineAction)i;
				if(action==VaccinePatCur.ActionCode) {
					listAction.SelectedIndex=i;
				}
			}
			_listVaccineObservations=VaccineObses.GetForVaccine(VaccinePatCur.VaccinePatNum);
			FillObservations();
		}

		private void FillObservations() {
			gridObservations.BeginUpdate();
			gridObservations.Columns.Clear();
			gridObservations.Columns.Add(new UI.ODGridColumn("Question",150));
			gridObservations.Columns.Add(new UI.ODGridColumn("Value",0));
			gridObservations.EndUpdate();
			gridObservations.BeginUpdate();
			gridObservations.Rows.Clear();
			for(int i=0;i<_listVaccineObservations.Count;i++) {
				VaccineObs vaccineObs=_listVaccineObservations[i];
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=vaccineObs;
				row.Cells.Add(new UI.ODGridCell(vaccineObs.IdentifyingCode.ToString()));
				row.Cells.Add(new UI.ODGridCell(vaccineObs.ValReported));
				gridObservations.Rows.Add(row);
			}
			if(_listVaccineObservationGroups==null) {
				_listVaccineObservationGroups=new List<VaccineObs>();
				for(int i=0;i<_listVaccineObservations.Count;i++) {
					VaccineObs vaccineObs=_listVaccineObservations[i];
					if(vaccineObs.VaccineObsNumGroup==0 || vaccineObs.VaccineObsNumGroup==vaccineObs.VaccineObsNum) {
						_listVaccineObservationGroups.Add(vaccineObs);
					}
					else {
						for(int j=0;j<_listVaccineObservations.Count;j++) {
							if(j!=i && _listVaccineObservations[j].VaccineObsNum==_listVaccineObservations[i].VaccineObsNumGroup) {
								_listVaccineObservationGroups.Add(_listVaccineObservations[j]);
								break;
							}
						}
					}
				}
			}
			gridObservations.EndUpdate();
		}

		private void comboVaccine_SelectedIndexChanged(object sender,EventArgs e) {
			DrugManufacturer manufacturer=DrugManufacturers.GetOne(VaccineDefs.Listt[comboVaccine.SelectedIndex].DrugManufacturerNum);
			textManufacturer.Text=manufacturer.ManufacturerCode + " - " + manufacturer.ManufacturerName;
		}

		private void comboProvNumOrdering_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelectedOrdering=ProviderC.ListShort[comboProvNumOrdering.SelectedIndex].ProvNum;
		}

		private void butPickProvOrdering_Click(object sender,EventArgs e) {
			FormProviderPick formP=new FormProviderPick();
			if(comboProvNumOrdering.SelectedIndex > -1) {//Initial formP selection if selected prov is not hidden.
				formP.SelectedProvNum=_provNumSelectedOrdering;
			}
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProvNumOrdering.SelectedIndex=Providers.GetIndex(formP.SelectedProvNum);
			_provNumSelectedOrdering=formP.SelectedProvNum;
		}

		private void butNoneProvOrdering_Click(object sender,EventArgs e) {
			_provNumSelectedOrdering=0;
			comboProvNumOrdering.SelectedIndex=-1;
		}

		private void comboProvNumAdministering_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelectedAdministering=ProviderC.ListShort[comboProvNumAdministering.SelectedIndex].ProvNum;
		}

		private void butPickProvAdministering_Click(object sender,EventArgs e) {
			FormProviderPick formP=new FormProviderPick();
			if(comboProvNumAdministering.SelectedIndex > -1) {//Initial formP selection if selected prov is not hidden.
				formP.SelectedProvNum=_provNumSelectedAdministering;
			}
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProvNumAdministering.SelectedIndex=Providers.GetIndex(formP.SelectedProvNum);
			_provNumSelectedAdministering=formP.SelectedProvNum;
		}

		private void butNoneProvAdministering_Click(object sender,EventArgs e) {
			_provNumSelectedAdministering=0;
			comboProvNumAdministering.SelectedIndex=-1;
		}

		private void gridObservations_CellClick(object sender,UI.ODGridClickEventArgs e) {
			if(gridObservations.SelectedIndices.Length>1) {
				return;//Do not select group if the user has selected more than one item (otherwise it would deselect some of the rows the user clicked, which would make using the group button impossible).
			}
			//Select all observations which are in the same group.
			VaccineObs vaccineObsGroup=_listVaccineObservationGroups[e.Row];
			gridObservations.SetSelected(false);//Deselect all.
			for(int i=0;i<_listVaccineObservationGroups.Count;i++) {
				if(_listVaccineObservationGroups[i]==vaccineObsGroup) {
					gridObservations.SetSelected(i,true);
				}
			}
		}

		private void gridObservations_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			VaccineObs vaccineObs=(VaccineObs)gridObservations.Rows[e.Row].Tag;
			FormVaccineObsEdit form=new FormVaccineObsEdit(vaccineObs);
			form.ShowDialog();
			if(vaccineObs.VaccinePatNum==0) {//Was deleted
				//If the observation identifying the group is deleted, then we need to reassign a new group.
				List<int> listRegroupIndicies=new List<int>();
				for(int i=0;i<_listVaccineObservations.Count;i++) {
					if(i!=e.Row && _listVaccineObservationGroups[i]==_listVaccineObservationGroups[e.Row]) {
						listRegroupIndicies.Add(i);
					}
				}
				if(listRegroupIndicies.Count>0) {
					VaccineObs vaccineObsGroup=_listVaccineObservations[listRegroupIndicies[0]];
					for(int i=0;i<listRegroupIndicies.Count;i++) {
						_listVaccineObservationGroups[listRegroupIndicies[i]]=vaccineObsGroup;
					}
				}
				//Delete the observation and corresponding group reference.
				_listVaccineObservations.RemoveAt(e.Row);
				_listVaccineObservationGroups.RemoveAt(e.Row);
			}
			FillObservations();
		}

		private void butAddObservation_Click(object sender,EventArgs e) {
			VaccineObs vaccineObs=new VaccineObs();
			vaccineObs.IsNew=true;
			vaccineObs.VaccinePatNum=-1;//Temporary dummy value (cannot be zero). Helps track new observations which have not been deleted.
			FormVaccineObsEdit form=new FormVaccineObsEdit(vaccineObs);
			if(form.ShowDialog()==DialogResult.OK) {
				_listVaccineObservations.Add(vaccineObs);
				_listVaccineObservationGroups.Add(vaccineObs);//In its own group with a single item initially.
				FillObservations();
			}
		}

		private void butGroupObservations_Click(object sender,EventArgs e) {
			if(gridObservations.SelectedIndices.Length<2) {
				MsgBox.Show(this,"Two or more observations must be selected.");
				return;
			}
			VaccineObs vaccineObsGroup=(VaccineObs)gridObservations.Rows[gridObservations.SelectedIndices[0]].Tag;
			for(int i=0;i<gridObservations.SelectedIndices.Length;i++) {
				_listVaccineObservationGroups[gridObservations.SelectedIndices[i]]=vaccineObsGroup;
			}
		}

		private void butUngroupObservations_Click(object sender,EventArgs e) {
			if(gridObservations.SelectedIndices.Length<1) {
				MsgBox.Show(this,"At least one observation must be selected.");
				return;
			}
			for(int i=0;i<gridObservations.SelectedIndices.Length;i++) {
				int index=gridObservations.SelectedIndices[i];
				_listVaccineObservationGroups[index]=_listVaccineObservations[index];//The vaccine is in its own group with a single item.
			}
			gridObservations.SetSelected(false);//Deselect all.
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MessageBox.Show("Delete?","Delete?",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
				return;
			}
			VaccinePats.Delete(VaccinePatCur.VaccinePatNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDateExpiration.errorProvider1.GetError(textDateExpiration)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			VaccineCompletionStatus vaccineCompletionStatus=(VaccineCompletionStatus)listCompletionStatus.SelectedIndex;
			if(comboVaccine.SelectedIndex==-1 && vaccineCompletionStatus!=VaccineCompletionStatus.NotAdministered) {
				//When the vaccine is not administered, the CVX code is automatically assumed to be 998 and there is no manufacturer.  Therefore, no vaccine def is needed.
				MessageBox.Show(this,"Please select a vaccine.");
				return;
			}
			if(vaccineCompletionStatus==VaccineCompletionStatus.NotAdministered) {
				if(textNote.Text=="") {
					MessageBox.Show(this,"Please enter documentation in the note.");
					return;
				}
				VaccinePatCur.VaccineDefNum=0;//Written for clarity
			}
			else {
				VaccinePatCur.VaccineDefNum=VaccineDefs.Listt[comboVaccine.SelectedIndex].VaccineDefNum;
			}
			try {
				VaccinePatCur.DateTimeStart=PIn.DateT(textDateTimeStart.Text);
				VaccinePatCur.DateTimeEnd=PIn.DateT(textDateTimeStop.Text);
			}
			catch {
				MessageBox.Show(this,"Please enter start and end times in format DD/MM/YYYY HH:mm AM/PM");
			}
			if(textAmount.Text==""){
				VaccinePatCur.AdministeredAmt=0;
			}
			else{
				try {
					VaccinePatCur.AdministeredAmt=PIn.Float(textAmount.Text);
				}
				catch {
					MessageBox.Show(this,"Please enter a valid amount.");
				}
			}
			if(comboUnits.SelectedIndex==0) {//'none'
				VaccinePatCur.DrugUnitNum=0;
			}
			else{
				VaccinePatCur.DrugUnitNum=DrugUnits.Listt[comboUnits.SelectedIndex-1].DrugUnitNum;
			}
			VaccinePatCur.LotNumber=textLotNum.Text;
			VaccinePatCur.DateExpire=PIn.Date(textDateExpiration.Text);
			VaccinePatCur.RefusalReason=(VaccineRefusalReason)listRefusalReason.SelectedIndex;
			VaccinePatCur.CompletionStatus=(VaccineCompletionStatus)listCompletionStatus.SelectedIndex;
			VaccinePatCur.Note=textNote.Text;
			VaccinePatCur.FilledCity=textFilledCity.Text;
			VaccinePatCur.FilledST=textFilledSt.Text;
			//VaccinePatCur.UserNum;//Was set when loading and cannot be edited by the user.
			VaccinePatCur.ProvNumOrdering=_provNumSelectedOrdering;
			VaccinePatCur.ProvNumAdminister=_provNumSelectedAdministering;
			VaccinePatCur.AdministrationRoute=(VaccineAdministrationRoute)comboAdministrationRoute.SelectedIndex;
			VaccinePatCur.AdministrationSite=(VaccineAdministrationSite)comboAdministrationSite.SelectedIndex;
			VaccinePatCur.AdministrationNoteCode=(VaccineAdministrationNote)listAdministrationNote.SelectedIndex;
			VaccinePatCur.ActionCode=(VaccineAction)listAction.SelectedIndex;
			if(IsNew) {
				VaccinePats.Insert(VaccinePatCur);
			}
			else {
				VaccinePats.Update(VaccinePatCur);
			}
			//We must delete then update/insert the observations after we insert the vaccinepat record, in case the vaccinepat is new.
			VaccineObses.DeleteForVaccinePat(VaccinePatCur.VaccinePatNum);
			for(int i=0;i<_listVaccineObservations.Count;i++) {
				VaccineObs vaccineObs=_listVaccineObservations[i];
				vaccineObs.VaccinePatNum=VaccinePatCur.VaccinePatNum;
				VaccineObses.Insert(vaccineObs);
			}
			//Update the vaccine observation group ids, now that the vaccine observation records have been inserted.
			for(int i=0;i<_listVaccineObservations.Count;i++) {
				VaccineObs vaccineObs=_listVaccineObservations[i];
				vaccineObs.VaccineObsNumGroup=_listVaccineObservationGroups[i].VaccineObsNum;
				VaccineObses.Update(vaccineObs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		

	

	


	}
}
