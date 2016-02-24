using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDentBusiness.WebBridges;

namespace OpenDental {
	public partial class FormPodiumSetup:Form {

		private Program _progCur;
		private ProgramProperty _apiToken;
		private ProgramProperty _compName;
		///<summary>Dictionary used to store changes for each clinic to be updated or inserted when saving to DB.</summary>
		private Dictionary<long,ProgramProperty> _dictLocationIDs=new Dictionary<long, ProgramProperty>();
		private ProgramProperty _useEConnector;
		private ProgramProperty _disableAdvertising;
		private ProgramProperty _apptSetCompleteMins;
		private ProgramProperty _apptTimeArrivedMins;
		private ProgramProperty _apptTimeDismissedMins;
		private ProgramProperty _newPatTriggerType;
		private ProgramProperty _existingPatTriggerType;
		private ReviewInvitationTrigger _existingPatTriggerEnum;
		private ReviewInvitationTrigger _newPatTriggerEnum;
		private bool _hasProgramPropertyChanged;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		private List<ProgramProperty> _listProgramProperties;
		private long _clinicNumCur;

		public FormPodiumSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPodiumSetup_Load(object sender,EventArgs e) {
			if(!PrefC.HasClinicsEnabled) {//clinics are not enabled, use ClinicNum 0 to indicate 'Headquarters' or practice level program properties
				checkEnabled.Text=Lan.g(this,"Enabled");
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
				_clinicNumCur=0;
			}
			else {//Using clinics
				_listUserClinicNums=new List<long>();
				comboClinic.Items.Clear();
				if(Security.CurUser.ClinicIsRestricted) {
					if(checkEnabled.Checked) {
						checkEnabled.Enabled=false;
						_clinicNumCur=0;
					}
				}
				else {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					//this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
					_listUserClinicNums.Add(0);
					comboClinic.SelectedIndex=0;
					_clinicNumCur=0;
				}
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Description);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(Clinics.ClinicNum==listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
						}
						_clinicNumCur=_listUserClinicNums[comboClinic.SelectedIndex];
					}
				}
			}
			_progCur=Programs.GetCur(ProgramName.Podium);
			if(_progCur==null) {
				MsgBox.Show(this,"The Podium bridge is missing from the database.");//should never happen
				return;
			}
			try {
				_listProgramProperties=ProgramProperties.GetListForProgramAndClinicWithDefault(_progCur.ProgramNum,_listUserClinicNums[comboClinic.SelectedIndex]);
				_useEConnector=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.UseEConnector);
				_disableAdvertising=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.DisableAdvertising);
				_apptSetCompleteMins=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.ApptSetCompletedMinutes);
				_apptTimeArrivedMins=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.ApptTimeArrivedMinutes);
				_apptTimeDismissedMins=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.ApptTimeDismissedMinutes);
				_compName=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.ComputerNameOrIP);
				_apiToken=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.APIToken);
				//_locationID=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.LocationID);
				List<ProgramProperty> listLocationIDs=ProgramProperties.GetListForProgram(_progCur.ProgramNum).FindAll(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.LocationID);
				_dictLocationIDs.Clear();
				foreach(ProgramProperty ppCur in listLocationIDs) {//If clinics is off, this will only grab the program property with a 0 clinic num (_listUserClinicNums will only have 0).
					if(_dictLocationIDs.ContainsKey(ppCur.ClinicNum) || !_listUserClinicNums.Contains(ppCur.ClinicNum)) {
						continue;
					}
					_dictLocationIDs.Add(ppCur.ClinicNum,ppCur);
				}
				_newPatTriggerType=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.NewPatientTriggerType);
				_existingPatTriggerType=_listProgramProperties.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.ExistingPatientTriggerType);
			}
			catch(Exception) {
				MsgBox.Show(this,"You are missing a program property for Podium.  Please contact support to resolve this issue.");
				Close();
			}
			FillForm();
			SetAdvertising();
		}

		///<summary>Handles both visibility and checking of checkHideButtons.</summary>
		private void SetAdvertising() {
			checkHideButtons.Visible=true;
			ProgramProperty prop=ProgramProperties.GetForProgram(_progCur.ProgramNum).FirstOrDefault(x => x.PropertyDesc=="Disable Advertising");
			if(checkEnabled.Checked || prop==null) {
				checkHideButtons.Visible=false;
			}
			if(prop!=null) {
				checkHideButtons.Checked=(prop.PropertyValue=="1");
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			SaveClinicCurProgramPropertiesToDict();
			_clinicNumCur=_listUserClinicNums[comboClinic.SelectedIndex];
			//This will either display the HQ value, or the clinic specific value.
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
				textLocationID.Text=_dictLocationIDs[_clinicNumCur].PropertyValue;
			}
			else {
				textLocationID.Text=_dictLocationIDs[0].PropertyValue;//Default to showing the HQ value when filling info for a clinic with no program property.
			}
		}

		private Dictionary<long,ProgramProperty> GetDictionaryDeepCopy(Dictionary<long,ProgramProperty> dict) {
			Dictionary<long,ProgramProperty> retVal=new Dictionary<long, ProgramProperty>();
			foreach(KeyValuePair<long,ProgramProperty> item in dict) {
				retVal.Add(item.Key,item.Value);
			}
			return retVal;
		}
		
		///<summary>Updates the in memory dictionary with any changes made to the locationID for each clinic.</summary>
		private void SaveClinicCurProgramPropertiesToDict() {
			if(_clinicNumCur==0) {
				if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {//Should always happen
					string defaultLocationID=_dictLocationIDs[_clinicNumCur].PropertyValue;
					Dictionary<long,ProgramProperty> dictLocationIDsCur=GetDictionaryDeepCopy(_dictLocationIDs);
					foreach(KeyValuePair<long,ProgramProperty> item in dictLocationIDsCur) {
						ProgramProperty ppCur=item.Value;
						if(ppCur.PropertyValue==defaultLocationID) {
							ppCur.PropertyValue=textLocationID.Text;
							_dictLocationIDs[item.Key]=ppCur;
						}
					}
				}
				return;
			}
			ProgramProperty ppLocationID=new ProgramProperty();
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
				ppLocationID=_dictLocationIDs[_clinicNumCur];//Override the database's property with what is in memory.
			}
			else {//Get default programproperty from db.
				ppLocationID=ProgramProperties.GetListForProgramAndClinicWithDefault(_progCur.ProgramNum,_clinicNumCur)
					.FirstOrDefault(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.LocationID);
			}
			if(ppLocationID.ClinicNum==0) {//No program property for current clinic, since _clinicNumCur!=0
				ProgramProperty ppLocationIDNew=ppLocationID.Copy();
				ppLocationIDNew.ProgramPropertyNum=0;
				ppLocationIDNew.ClinicNum=_clinicNumCur;
				ppLocationIDNew.PropertyValue=textLocationID.Text;
				if(!_dictLocationIDs.ContainsKey(_clinicNumCur)) {//Should always happen
					_dictLocationIDs.Add(_clinicNumCur,ppLocationIDNew);
				}
				return;
			}
			//At this point we know that the clinicnum isn't 0 and the database has a property for that clinicnum.
			if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {//Should always happen
				ppLocationID.PropertyValue=textLocationID.Text;
				_dictLocationIDs[_clinicNumCur]=ppLocationID;
			}
			else {
				_dictLocationIDs.Add(_clinicNumCur,ppLocationID);//Should never happen.
			}
		}

		private void FillForm() {
			try {
				checkUseEConnector.Checked=PIn.Bool(_useEConnector.PropertyValue);
				textProgName.Text=_progCur.ProgName;
				textProgDesc.Text=_progCur.ProgDesc;
				checkEnabled.Checked=_progCur.Enabled;
				textPath.Text=_progCur.Path;
				checkHideButtons.Checked=PIn.Bool(_disableAdvertising.PropertyValue);
				textApptSetComplete.Text=_apptSetCompleteMins.PropertyValue;
				textApptTimeArrived.Text=_apptTimeArrivedMins.PropertyValue;
				textApptTimeDismissed.Text=_apptTimeDismissedMins.PropertyValue;
				textCompNameOrIP.Text=_compName.PropertyValue;
				textAPIToken.Text=_apiToken.PropertyValue;
				if(_dictLocationIDs.ContainsKey(_clinicNumCur)) {
					textLocationID.Text=_dictLocationIDs[_clinicNumCur].PropertyValue;
				}
				else {
					textLocationID.Text=_dictLocationIDs[0].PropertyValue;//Default to showing the HQ value when filling info for a clinic with no program property.
				}
				ReviewInvitationTrigger existingPatTriggerType=PIn.Enum<ReviewInvitationTrigger>(_existingPatTriggerType.PropertyValue);
				ReviewInvitationTrigger newPatTriggerType=PIn.Enum<ReviewInvitationTrigger>(_newPatTriggerType.PropertyValue);
				switch(existingPatTriggerType) {
					case ReviewInvitationTrigger.AppointmentCompleted:
						radioSetCompleteExistingPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeArrived:
						radioTimeArrivedExistingPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeDismissed:
						radioTimeDismissedExistingPat.Checked=true;
						break;
				}
				switch(newPatTriggerType) {
					case ReviewInvitationTrigger.AppointmentCompleted:
						radioSetCompleteNewPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeArrived:
						radioTimeArrivedNewPat.Checked=true;
						break;
					case ReviewInvitationTrigger.AppointmentTimeDismissed:
						radioTimeDismissedNewPat.Checked=true;
						break;
				}
				_existingPatTriggerEnum=existingPatTriggerType;
				_newPatTriggerEnum=newPatTriggerType;
			}
			catch(Exception) {
				MsgBox.Show(this,"You are missing a program property from the database.  Please call support to resolve this issue.");
				Close();
			}
		}

		private void RadioButton_CheckChanged(object sender,EventArgs e) {
			RadioButton buttonCur=(RadioButton)sender;
			if(buttonCur.Checked) {
				switch(buttonCur.Name) {
					case "radioSetCompleteExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentCompleted;
						break;
					case "radioTimeArrivedExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeArrived;
						break;
					case "radioTimeDismissedExistingPat":
						_existingPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeDismissed;
						break;
					case "radioSetCompleteNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentCompleted;
						break;
					case "radioTimeArrivedNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeArrived;
						break;
					case "radioTimeDismissedNewPat":
						_newPatTriggerEnum=ReviewInvitationTrigger.AppointmentTimeDismissed;
						break;
				}
			}
		}

		private void SaveProgram() {
			SaveClinicCurProgramPropertiesToDict();
			_progCur.ProgName=textProgName.Text;
			_progCur.ProgDesc=textProgDesc.Text;
			_progCur.Enabled=checkEnabled.Checked;
			_progCur.Path=textPath.Text;
			UpdateProgramProperty(_useEConnector,POut.Bool(checkUseEConnector.Checked));
			UpdateProgramProperty(_disableAdvertising,POut.Bool(checkHideButtons.Checked));
			UpdateProgramProperty(_apptSetCompleteMins,textApptSetComplete.Text);
			UpdateProgramProperty(_apptTimeArrivedMins,textApptTimeArrived.Text);
			UpdateProgramProperty(_apptTimeDismissedMins,textApptTimeDismissed.Text);
			UpdateProgramProperty(_compName,textCompNameOrIP.Text);
			UpdateProgramProperty(_apiToken,textAPIToken.Text);
			UpdateProgramProperty(_newPatTriggerType,POut.Int((int)_newPatTriggerEnum));
			UpdateProgramProperty(_existingPatTriggerType,POut.Int((int)_existingPatTriggerEnum));
			UpsertProgramPropertiesForClinics();
			Programs.Update(_progCur);
		}

		private void UpdateProgramProperty(ProgramProperty ppFromDb,string newpropertyValue) {
			if(ppFromDb.PropertyValue==newpropertyValue) {
				return;
			}
			ppFromDb.PropertyValue=newpropertyValue;
			ProgramProperties.Update(ppFromDb);
			_hasProgramPropertyChanged=true;
		}

		private void UpsertProgramPropertiesForClinics() {
			List<ProgramProperty> listLocationIDsFromDb=ProgramProperties.GetListForProgram(_progCur.ProgramNum).FindAll(x => x.PropertyDesc==Podium.ProgramPropertyDescriptions.LocationID);
			List<ProgramProperty> listLocationIDsCur=_dictLocationIDs.Values.ToList();
			foreach(ProgramProperty ppCur in listLocationIDsCur) {
				if(listLocationIDsFromDb.Exists(x => x.ProgramPropertyNum == ppCur.ProgramPropertyNum)) {
					UpdateProgramProperty(listLocationIDsFromDb[listLocationIDsFromDb.FindIndex(x => x.ProgramPropertyNum == ppCur.ProgramPropertyNum)],ppCur.PropertyValue);//ppCur.PropertyValue will match textLocationID.Text
				}
				else {
					ProgramProperties.Insert(ppCur);//Program property for that clinicnum didn't exist, so insert it into the db.
					_hasProgramPropertyChanged=true;
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			SaveProgram();
			if(_hasProgramPropertyChanged) {
				DataValid.SetInvalid(InvalidType.Programs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}