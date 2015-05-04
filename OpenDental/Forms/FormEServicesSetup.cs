using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental {
	///<summary>Form manages all eServices setup.  Also includes monitoring for the Listener Service that is required for HQ hosted eServices.</summary>
	public partial class FormEServicesSetup:Form {
		private static MobileWeb.Mobile mb=new MobileWeb.Mobile();
		private static int _batchSize=100;
		///<summary>All statements of a patient are not uploaded. The limit is defined by the recent [statementLimitPerPatient] records</summary>
		private static int _statementLimitPerPatient=5;
		///<summary>This variable prevents the synching methods from being called when a previous synch is in progress.</summary>
		private static bool _isSynching;
		///<summary>This variable prevents multiple error message boxes from popping up if mobile synch server is not available.</summary>
		private static bool _isServerAvail=true;
		///<summary>True if a pref was saved and the other workstations need to have their cache refreshed when this form closes.</summary>
		private bool _changed;
		///<summary>If this variable is true then records are uploaded one at a time so that an error in uploading can be traced down to a single record</summary>
		private static bool _isTroubleshootMode=false;
		private static FormProgress FormP;
		///<summary>The background color used when the OpenDentalCustListener service is down.  Using Red was deemed too harsh.  This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_ALERT_BACKGROUND=Color.OrangeRed;
		///<summary>The text color used when the OpenDentalCustListener service is down.  This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_ALERT_TEXT=Color.Yellow;

		///<summary>Launches the eServices Setup window defaulted to the patient portal tab.</summary>
		public FormEServicesSetup():this(EService.PatientPortal){ 		
		}

		///<summary>Launches the eServices Setup window defaulted to the tab of the eService passed in.</summary>
		public FormEServicesSetup(EService setTab) {
			InitializeComponent();
			Lan.F(this);
			switch(setTab) {
				case EService.ListenerService:
					tabControl.SelectTab(tabListenerService);
					break;
				case EService.MobileOld:
					tabControl.SelectTab(tabMobileOld);
					break;
				case EService.MobileNew:
					tabControl.SelectTab(tabMobileNew);
					break;
				case EService.WebSched:
					tabControl.SelectTab(tabWebSched);
					break;
				case EService.PatientPortal:
				default:
					tabControl.SelectTab(tabPatientPortal);
					break;
			}
		}

		private void FormPatientPortalSetup_Load(object sender,EventArgs e) {
			textRedirectUrlPatientPortal.Text=PrefC.GetString(PrefName.PatientPortalURL);
			textBoxNotificationSubject.Text=PrefC.GetString(PrefName.PatientPortalNotifySubject);
			textBoxNotificationBody.Text=PrefC.GetString(PrefName.PatientPortalNotifyBody);
			textListenerPort.Text=PrefC.GetString(PrefName.CustListenerPort);
			#region mobile synch
			textMobileSyncServerURL.Text=PrefC.GetString(PrefName.MobileSyncServerURL);
			textSynchMinutes.Text=PrefC.GetInt(PrefName.MobileSyncIntervalMinutes).ToString();
			textDateBefore.Text=PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate).ToShortDateString();
			textMobileSynchWorkStation.Text=PrefC.GetString(PrefName.MobileSyncWorkstationName);
			textMobileUserName.Text=PrefC.GetString(PrefName.MobileUserName);
			textMobilePassword.Text="";//not stored locally, and not pulled from web server
			DateTime lastRun=PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun);
			if(lastRun.Year>1880) {
				textDateTimeLastRun.Text=lastRun.ToShortDateString()+" "+lastRun.ToShortTimeString();
			}
			//Web server is not contacted when loading this form.  That would be too slow.
			//CreateAppointments(5);
			#endregion
			#region Web Sched
			labelWebSchedEnable.Text="";
			if(PrefC.GetBool(PrefName.WebSchedService)) {
				butWebSchedEnable.Enabled=false;
				labelWebSchedEnable.Text=Lan.g(this,"Web Sched service is currently enabled.");
			}
			#endregion
			#region Listener Service
			FillTextListenerServiceStatus();
			FillGridListenerService();
			#endregion
			SetControlEnabledState();
		}

		private void SetControlEnabledState() {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				//Disable certain buttons but let them continue to view
				butSavePatientPortal.Enabled=false;
				butGetUrlPatientPortal.Enabled=false;
				groupBoxNotification.Enabled=false;
				textListenerPort.Enabled=false;
				butSaveListenerPort.Enabled=false;
				((Control)tabMobileOld).Enabled = false;
				return;
			}
			textListenerPort.Enabled=tabControl.SelectedTab!=tabMobileOld;
			butSaveListenerPort.Enabled=tabControl.SelectedTab!=tabMobileOld;
		}

		#region patient portal
		private void butGetUrlPatientPortal_Click(object sender,EventArgs e) {
			try {
				string url=CustomerUpdatesProxy.GetHostedURL("PatientPortal");
				textOpenDentalUrlPatientPortal.Text=url;
				if(textRedirectUrlPatientPortal.Text=="") {
					textRedirectUrlPatientPortal.Text=url;
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butSavePatientPortal_Click(object sender,EventArgs e) {
#if !DEBUG
			if(!textRedirectUrlPatientPortal.Text.ToUpper().StartsWith("HTTPS")) {
				MsgBox.Show(this,"Patient Facing URL must start with HTTPS.");
				return;
			}
#endif
			if(textBoxNotificationSubject.Text=="") {
				MsgBox.Show(this,"Notification Subject is empty");
				textBoxNotificationSubject.Focus();
				return;
			}
			if(textBoxNotificationBody.Text=="") {
				MsgBox.Show(this,"Notification Body is empty");
				textBoxNotificationBody.Focus();
				return;
			}
			if(!textBoxNotificationBody.Text.Contains("[URL]")) { //prompt user that they omitted the URL field but don't prevent them from continuing
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"[URL] not included in notification body. Continue without setting the [URL] field?")) {
					textBoxNotificationBody.Focus();
					return;
				}
			}
			if(Prefs.UpdateString(PrefName.PatientPortalURL,textRedirectUrlPatientPortal.Text)
				| Prefs.UpdateString(PrefName.PatientPortalNotifySubject,textBoxNotificationSubject.Text)
				| Prefs.UpdateString(PrefName.PatientPortalNotifyBody,textBoxNotificationBody.Text)) 
			{
				_changed=true;//Sends invalid signal upon closing the form.
			}
			MsgBox.Show(this,"Patient Portal Info Saved");
		}
		#endregion

		#region mobile web (new-style)
		private void butGetUrlMobileWeb_Click(object sender,EventArgs e) {
			try {
				string url=CustomerUpdatesProxy.GetHostedURL("MobileWeb");
				textOpenDentalUrlMobileWeb.Text=url;				
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		#region mobile synch (old-style)
		private void butCurrentWorkstation_Click(object sender,EventArgs e) {
			textMobileSynchWorkStation.Text=System.Environment.MachineName.ToUpper();
		}

		private void butSaveMobileSynch_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			if(!SavePrefs()) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done");
		}

		///<summary>Returns false if validation failed.  This also makes sure the web service exists, the customer is paid, and the registration key is correct.</summary>
		private bool SavePrefs() {
			//validation
			if(textSynchMinutes.errorProvider1.GetError(textSynchMinutes)!=""
				|| textDateBefore.errorProvider1.GetError(textDateBefore)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			//yes, workstation is allowed to be blank.  That's one way for user to turn off auto synch.
			//if(textMobileSynchWorkStation.Text=="") {
			//	MsgBox.Show(this,"WorkStation cannot be empty");
			//	return false;
			//}
			// the text field is read because the keyed in values have not been saved yet
			//if(textMobileSyncServerURL.Text.Contains("192.168.0.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
			if(textMobileSyncServerURL.Text.Contains("10.10.1.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
				IgnoreCertificateErrors();// done so that TestWebServiceExists() does not thow an error.
			}
			// if this is not done then an old non-functional url prevents any new url from being saved.
			Prefs.UpdateString(PrefName.MobileSyncServerURL,textMobileSyncServerURL.Text);
			if(!TestWebServiceExists()) {
				MsgBox.Show(this,"Web service not found.");
				return false;
			}
			if(mb.GetCustomerNum(PrefC.GetString(PrefName.RegistrationKey))==0) {
				MsgBox.Show(this,"Registration key is incorrect.");
				return false;
			}
			if(!VerifyPaidCustomer()) {
				return false;
			}
			//Minimum 10 char.  Must contain uppercase, lowercase, numbers, and symbols. Valid symbols are: !@#$%^&+= 
			//The set of symbols checked was far too small, not even including periods, commas, and parentheses.
			//So I rewrote it all.  New error messages say exactly what's wrong with it.
			if(textMobileUserName.Text!="") {//allowed to be blank
				if(textMobileUserName.Text.Length<10) {
					MsgBox.Show(this,"User Name must be at least 10 characters long.");
					return false;
				}
				if(!Regex.IsMatch(textMobileUserName.Text,"[A-Z]+")) {
					MsgBox.Show(this,"User Name must contain an uppercase letter.");
					return false;
				}
				if(!Regex.IsMatch(textMobileUserName.Text,"[a-z]+")) {
					MsgBox.Show(this,"User Name must contain an lowercase letter.");
					return false;
				}
				if(!Regex.IsMatch(textMobileUserName.Text,"[0-9]+")) {
					MsgBox.Show(this,"User Name must contain a number.");
					return false;
				}
				if(!Regex.IsMatch(textMobileUserName.Text,"[^0-9a-zA-Z]+")) {//absolutely anything except number, lower or upper.
					MsgBox.Show(this,"User Name must contain punctuation or symbols.");
					return false;
				}
			}
			if(textDateBefore.Text=="") {//default to one year if empty
				textDateBefore.Text=DateTime.Today.AddYears(-1).ToShortDateString();
				//not going to bother informing user.  They can see it.
			}
			//save to db------------------------------------------------------------------------------------
			if(Prefs.UpdateString(PrefName.MobileSyncServerURL,textMobileSyncServerURL.Text)
				| Prefs.UpdateInt(PrefName.MobileSyncIntervalMinutes,PIn.Int(textSynchMinutes.Text))//blank entry allowed
				| Prefs.UpdateString(PrefName.MobileExcludeApptsBeforeDate,POut.Date(PIn.Date(textDateBefore.Text),false))//blank 
				| Prefs.UpdateString(PrefName.MobileSyncWorkstationName,textMobileSynchWorkStation.Text)
				| Prefs.UpdateString(PrefName.MobileUserName,textMobileUserName.Text)) 
			{
				_changed=true;
			}
			//Username and password-----------------------------------------------------------------------------
			mb.SetMobileWebUserPassword(PrefC.GetString(PrefName.RegistrationKey),textMobileUserName.Text.Trim(),textMobilePassword.Text.Trim());
			return true;
		}

		///<summary>Uploads Preferences to the Patient Portal /Mobile Web.</summary>
		public static void UploadPreference(PrefName prefname) {
			if(PrefC.GetString(PrefName.RegistrationKey)=="") {
				return;//Prevents a bug when using the trial version with no registration key.  Practice edit, OK, was giving error.
			}
			try {
				if(TestWebServiceExists()) {
					Prefm prefm = Prefms.GetPrefm(prefname.ToString());
					mb.SetPreference(PrefC.GetString(PrefName.RegistrationKey),prefm);
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);//may not show if called from a thread but that does not matter - the failing of this method should not stop the  the code from proceeding.
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!SavePrefs()) {
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete all your data from our server?  This happens automatically before a full synch.")) {
				return;
			}
			mb.DeleteAllRecords(PrefC.GetString(PrefName.RegistrationKey));
			MsgBox.Show(this,"Done");
		}

		private void butFullSync_Click(object sender,EventArgs e) {
			if(!SavePrefs()) {
				return;
			}
			if(_isSynching) {
				MsgBox.Show(this,"A Synch is in progress at the moment. Please try again later.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will be time consuming. Continue anyway?")) {
				return;
			}
			//for full synch, delete all records then repopulate.
			mb.DeleteAllRecords(PrefC.GetString(PrefName.RegistrationKey));
			ShowProgressForm(DateTime.MinValue);
		}

		private void butSync_Click(object sender,EventArgs e) {
			if(!SavePrefs()) {
				return;
			}
			if(_isSynching) {
				MsgBox.Show(this,"A Synch is in progress at the moment. Please try again later.");
				return;
			}
			if(PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate).Year<1880) {
				MsgBox.Show(this,"Full synch has never been run before.");
				return;
			}
			DateTime changedSince=PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun);
			ShowProgressForm(changedSince);
		}

		private void ShowProgressForm(DateTime changedSince) {
			if(checkTroubleshooting.Checked) {
				_isTroubleshootMode=true;
			}
			else {
				_isTroubleshootMode=false;
			}
			DateTime timeSynchStarted=MiscData.GetNowDateTime();
			FormP=new FormProgress();
			FormP.MaxVal=100;//to keep the form from closing until the real MaxVal is set.
			FormP.NumberMultiplication=1;
			FormP.DisplayText="Preparing records for upload.";
			FormP.NumberFormat="F0";
			//start the thread that will perform the upload
			ThreadStart uploadDelegate= delegate { UploadWorker(changedSince,timeSynchStarted); };
			Thread workerThread=new Thread(uploadDelegate);
			workerThread.Start();
			//display the progress dialog to the user:
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				workerThread.Abort();
			}
			_changed=true;
			textDateTimeLastRun.Text=PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun).ToShortDateString()+" "+PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun).ToShortTimeString();
		}


		///<summary>This is the function that the worker thread uses to actually perform the upload.  Can also call this method in the ordinary way if the data to be transferred is small.  The timeSynchStarted must be passed in to ensure that no records are skipped due to small time differences.</summary>
		private static void UploadWorker(DateTime changedSince,DateTime timeSynchStarted) {
			int totalCount=100;
			try {//Dennis: try catch may not work: Does not work in threads, not sure about this. Note that all methods inside this try catch block are without exception handling. This is done on purpose so that when an exception does occur it does not update PrefName.MobileSyncDateTimeLastRun
				//The handling of PrefName.MobileSynchNewTables79 should never be removed in future versions
				DateTime changedProv=changedSince;
				DateTime changedDeleted=changedSince;
				DateTime changedPat=changedSince;
				DateTime changedStatement=changedSince;
				DateTime changedDocument=changedSince;
				DateTime changedRecall=changedSince;
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables79Done,false)) {
					changedProv=DateTime.MinValue;
					changedDeleted=DateTime.MinValue;
				}
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables112Done,false)) {
					changedPat=DateTime.MinValue;
					changedStatement=DateTime.MinValue;
					changedDocument=DateTime.MinValue;
				}
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables121Done,false)) {
					changedRecall=DateTime.MinValue;
					UploadPreference(PrefName.PracticeTitle); //done again because the previous upload did not include the prefnum
				}
				bool synchDelPat=true;
				if(PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun).Hour==timeSynchStarted.Hour) {
					synchDelPat=false;// synching delPatNumList is timeconsuming (15 seconds) for a dental office with around 5000 patients and it's mostly the same records that have to be deleted every time a synch happens. So it's done only once hourly.
				}
				//MobileWeb
				List<long> patNumList=Patientms.GetChangedSincePatNums(changedPat);
				List<long> aptNumList=Appointmentms.GetChangedSinceAptNums(changedSince,PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate));
				List<long> rxNumList=RxPatms.GetChangedSinceRxNums(changedSince);
				List<long> provNumList=Providerms.GetChangedSinceProvNums(changedProv);
				List<long> pharNumList=Pharmacyms.GetChangedSincePharmacyNums(changedSince);
				List<long> allergyDefNumList=AllergyDefms.GetChangedSinceAllergyDefNums(changedSince);
				List<long> allergyNumList=Allergyms.GetChangedSinceAllergyNums(changedSince);
				//exclusively Patient Portal
				/*
				List<long> eligibleForUploadPatNumList=Patientms.GetPatNumsEligibleForSynch();
				List<long> labPanelNumList=LabPanelms.GetChangedSinceLabPanelNums(changedSince,eligibleForUploadPatNumList);
				List<long> labResultNumList=LabResultms.GetChangedSinceLabResultNums(changedSince);
				List<long> medicationNumList=Medicationms.GetChangedSinceMedicationNums(changedSince);
				List<long> medicationPatNumList=MedicationPatms.GetChangedSinceMedicationPatNums(changedSince,eligibleForUploadPatNumList);
				List<long> diseaseDefNumList=DiseaseDefms.GetChangedSinceDiseaseDefNums(changedSince);
				List<long> diseaseNumList=Diseasems.GetChangedSinceDiseaseNums(changedSince,eligibleForUploadPatNumList);
				List<long> icd9NumList=ICD9ms.GetChangedSinceICD9Nums(changedSince);
				List<long> statementNumList=Statementms.GetChangedSinceStatementNums(changedStatement,eligibleForUploadPatNumList,statementLimitPerPatient);
				List<long> documentNumList=Documentms.GetChangedSinceDocumentNums(changedDocument,statementNumList);
				List<long> recallNumList=Recallms.GetChangedSinceRecallNums(changedRecall);*/
				List<long> delPatNumList=Patientms.GetPatNumsForDeletion();
				//List<DeletedObject> dO=DeletedObjects.GetDeletedSince(changedDeleted);dennis: delete this line later
				List<long> deletedObjectNumList=DeletedObjects.GetChangedSinceDeletedObjectNums(changedDeleted);//to delete appointments from mobile
				totalCount= patNumList.Count+aptNumList.Count+rxNumList.Count+provNumList.Count+pharNumList.Count
					//+labPanelNumList.Count+labResultNumList.Count+medicationNumList.Count+medicationPatNumList.Count
					//+allergyDefNumList.Count//+allergyNumList.Count+diseaseDefNumList.Count+diseaseNumList.Count+icd9NumList.Count
					//+statementNumList.Count+documentNumList.Count+recallNumList.Count
					+deletedObjectNumList.Count;
				if(synchDelPat) {
					totalCount+=delPatNumList.Count;
				}
				double currentVal=0;
				if(Application.OpenForms["FormProgress"]!=null) {// without this line the following error is thrown: "Invoke or BeginInvoke cannot be called on a control until the window handle has been created." or a null pointer exception is thrown when an automatic synch is done by the system.
					FormP.Invoke(new PassProgressDelegate(PassProgressToDialog),
						new object[] { currentVal,"?currentVal of ?maxVal records uploaded",totalCount,"" });
				}
				_isSynching=true;
				SynchGeneric(patNumList,SynchEntity.patient,totalCount,ref currentVal);
				SynchGeneric(aptNumList,SynchEntity.appointment,totalCount,ref currentVal);
				SynchGeneric(rxNumList,SynchEntity.prescription,totalCount,ref currentVal);
				SynchGeneric(provNumList,SynchEntity.provider,totalCount,ref currentVal);
				SynchGeneric(pharNumList,SynchEntity.pharmacy,totalCount,ref currentVal);
				//pat portal
				/*
				SynchGeneric(labPanelNumList,SynchEntity.labpanel,totalCount,ref currentVal);
				SynchGeneric(labResultNumList,SynchEntity.labresult,totalCount,ref currentVal);
				SynchGeneric(medicationNumList,SynchEntity.medication,totalCount,ref currentVal);
				SynchGeneric(medicationPatNumList,SynchEntity.medicationpat,totalCount,ref currentVal);
				SynchGeneric(allergyDefNumList,SynchEntity.allergydef,totalCount,ref currentVal);
				SynchGeneric(allergyNumList,SynchEntity.allergy,totalCount,ref currentVal);
				SynchGeneric(diseaseDefNumList,SynchEntity.diseasedef,totalCount,ref currentVal);
				SynchGeneric(diseaseNumList,SynchEntity.disease,totalCount,ref currentVal);
				SynchGeneric(icd9NumList,SynchEntity.icd9,totalCount,ref currentVal);
				SynchGeneric(statementNumList,SynchEntity.statement,totalCount,ref currentVal);
				SynchGeneric(documentNumList,SynchEntity.document,totalCount,ref currentVal);
				SynchGeneric(recallNumList,SynchEntity.recall,totalCount,ref currentVal);*/
				if(synchDelPat) {
					SynchGeneric(delPatNumList,SynchEntity.patientdel,totalCount,ref currentVal);
				}
				//DeleteObjects(dO,totalCount,ref currentVal);// this has to be done at this end because objects may have been created and deleted between synchs. If this function is place above then the such a deleted object will not be deleted from the server.
				SynchGeneric(deletedObjectNumList,SynchEntity.deletedobject,totalCount,ref currentVal);// this has to be done at this end because objects may have been created and deleted between synchs. If this function is place above then the such a deleted object will not be deleted from the server.
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables79Done,true)) {
					Prefs.UpdateBool(PrefName.MobileSynchNewTables79Done,true);
				}
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables112Done,true)) {
					Prefs.UpdateBool(PrefName.MobileSynchNewTables112Done,true);
				}
				if(!PrefC.GetBoolSilent(PrefName.MobileSynchNewTables121Done,true)) {
					Prefs.UpdateBool(PrefName.MobileSynchNewTables121Done,true);
				}
				Prefs.UpdateDateT(PrefName.MobileSyncDateTimeLastRun,timeSynchStarted);
				_isSynching=false;
			}
			catch(Exception e) {
				_isSynching=false;// this will ensure that the synch can start again. If this variable remains true due to an exception then a synch will never take place automatically.
				if(Application.OpenForms["FormProgress"]!=null) {// without this line the following error is thrown: "Invoke or BeginInvoke cannot be called on a control until the window handle has been created." or a null pointer exception is thrown when an automatic synch is done by the system.
					FormP.Invoke(new PassProgressDelegate(PassProgressToDialog),
						new object[] { 0,"?currentVal of ?maxVal records uploaded",totalCount,e.Message });
				}
			}
		}

		///<summary>a general function to reduce the amount of code for uploading</summary>
		private static void SynchGeneric(List<long> PKNumList,SynchEntity entity,double totalCount,ref double currentVal) {
			//Dennis: a try catch block here has been avoid on purpose.
			List<long> BlockPKNumList=null;
			int localBatchSize=_batchSize;
			if(_isTroubleshootMode) {
				localBatchSize=1;
			}
			string AtoZpath=ImageStore.GetPreferredAtoZpath();
			for(int start=0;start<PKNumList.Count;start+=localBatchSize) {
				if((start+localBatchSize)>PKNumList.Count) {
					localBatchSize=PKNumList.Count-start;
				}
				try {
					BlockPKNumList=PKNumList.GetRange(start,localBatchSize);
					switch(entity) {
						case SynchEntity.patient:
							List<Patientm> changedPatientmList=Patientms.GetMultPats(BlockPKNumList);
							mb.SynchPatients(PrefC.GetString(PrefName.RegistrationKey),changedPatientmList.ToArray());
							break;
						case SynchEntity.appointment:
							List<Appointmentm> changedAppointmentmList=Appointmentms.GetMultApts(BlockPKNumList);
							mb.SynchAppointments(PrefC.GetString(PrefName.RegistrationKey),changedAppointmentmList.ToArray());
							break;
						case SynchEntity.prescription:
							List<RxPatm> changedRxList=RxPatms.GetMultRxPats(BlockPKNumList);
							mb.SynchPrescriptions(PrefC.GetString(PrefName.RegistrationKey),changedRxList.ToArray());
							break;
						case SynchEntity.provider:
							List<Providerm> changedProvList=Providerms.GetMultProviderms(BlockPKNumList);
							mb.SynchProviders(PrefC.GetString(PrefName.RegistrationKey),changedProvList.ToArray());
							break;
						case SynchEntity.pharmacy:
							List<Pharmacym> changedPharmacyList=Pharmacyms.GetMultPharmacyms(BlockPKNumList);
							mb.SynchPharmacies(PrefC.GetString(PrefName.RegistrationKey),changedPharmacyList.ToArray());
							break;
						case SynchEntity.labpanel:
							List<LabPanelm> ChangedLabPanelList=LabPanelms.GetMultLabPanelms(BlockPKNumList);
							mb.SynchLabPanels(PrefC.GetString(PrefName.RegistrationKey),ChangedLabPanelList.ToArray());
							break;
						case SynchEntity.labresult:
							List<LabResultm> ChangedLabResultList=LabResultms.GetMultLabResultms(BlockPKNumList);
							mb.SynchLabResults(PrefC.GetString(PrefName.RegistrationKey),ChangedLabResultList.ToArray());
							break;
						case SynchEntity.medication:
							List<Medicationm> ChangedMedicationList=Medicationms.GetMultMedicationms(BlockPKNumList);
							mb.SynchMedications(PrefC.GetString(PrefName.RegistrationKey),ChangedMedicationList.ToArray());
							break;
						case SynchEntity.medicationpat:
							List<MedicationPatm> ChangedMedicationPatList=MedicationPatms.GetMultMedicationPatms(BlockPKNumList);
							mb.SynchMedicationPats(PrefC.GetString(PrefName.RegistrationKey),ChangedMedicationPatList.ToArray());
							break;
						case SynchEntity.allergy:
							List<Allergym> ChangedAllergyList=Allergyms.GetMultAllergyms(BlockPKNumList);
							mb.SynchAllergies(PrefC.GetString(PrefName.RegistrationKey),ChangedAllergyList.ToArray());
							break;
						case SynchEntity.allergydef:
							List<AllergyDefm> ChangedAllergyDefList=AllergyDefms.GetMultAllergyDefms(BlockPKNumList);
							mb.SynchAllergyDefs(PrefC.GetString(PrefName.RegistrationKey),ChangedAllergyDefList.ToArray());
							break;
						case SynchEntity.disease:
							List<Diseasem> ChangedDiseaseList=Diseasems.GetMultDiseasems(BlockPKNumList);
							mb.SynchDiseases(PrefC.GetString(PrefName.RegistrationKey),ChangedDiseaseList.ToArray());
							break;
						case SynchEntity.diseasedef:
							List<DiseaseDefm> ChangedDiseaseDefList=DiseaseDefms.GetMultDiseaseDefms(BlockPKNumList);
							mb.SynchDiseaseDefs(PrefC.GetString(PrefName.RegistrationKey),ChangedDiseaseDefList.ToArray());
							break;
						case SynchEntity.icd9:
							List<ICD9m> ChangedICD9List=ICD9ms.GetMultICD9ms(BlockPKNumList);
							mb.SynchICD9s(PrefC.GetString(PrefName.RegistrationKey),ChangedICD9List.ToArray());
							break;
						case SynchEntity.statement:
							List<Statementm> ChangedStatementList=Statementms.GetMultStatementms(BlockPKNumList);
							mb.SynchStatements(PrefC.GetString(PrefName.RegistrationKey),ChangedStatementList.ToArray());
							break;
						case SynchEntity.document:
							List<Documentm> ChangedDocumentList=Documentms.GetMultDocumentms(BlockPKNumList,AtoZpath);
							mb.SynchDocuments(PrefC.GetString(PrefName.RegistrationKey),ChangedDocumentList.ToArray());
							break;
						case SynchEntity.recall:
							List<Recallm> ChangedRecallList=Recallms.GetMultRecallms(BlockPKNumList);
							mb.SynchRecalls(PrefC.GetString(PrefName.RegistrationKey),ChangedRecallList.ToArray());
							break;
						case SynchEntity.deletedobject:
							List<DeletedObject> ChangedDeleteObjectList=DeletedObjects.GetMultDeletedObjects(BlockPKNumList);
							mb.DeleteObjects(PrefC.GetString(PrefName.RegistrationKey),ChangedDeleteObjectList.ToArray());
							break;
						case SynchEntity.patientdel:
							mb.DeletePatientsRecords(PrefC.GetString(PrefName.RegistrationKey),BlockPKNumList.ToArray());
							break;
					}
					//progressIndicator.CurrentVal+=LocalBatchSize;//not allowed
					currentVal+=localBatchSize;
					if(Application.OpenForms["FormProgress"]!=null) {// without this line the following error is thrown: "Invoke or BeginInvoke cannot be called on a control until the window handle has been created." or a null pointer exception is thrown when an automatic synch is done by the system.
						FormP.Invoke(new PassProgressDelegate(PassProgressToDialog),
							new object[] { currentVal,"?currentVal of ?maxVal records uploaded",totalCount,"" });
					}
				}
				catch(Exception e) {
					if(_isTroubleshootMode) {
						string errorMessage=entity+ " with Primary Key = "+BlockPKNumList[0].ToString()+" failed to synch. "+"\n"+e.Message;
						throw new Exception(errorMessage);
					}
					else {
						throw e;
					}
				}
			}//for loop ends here
		}

		///<summary>This method gets invoked from the worker thread.</summary>
		private static void PassProgressToDialog(double currentVal,string displayText,double maxVal,string errorMessage) {
			FormP.CurrentVal=currentVal;
			FormP.DisplayText=displayText;
			FormP.MaxVal=maxVal;
			FormP.ErrorMessage=errorMessage;
		}

		/*
		private static void DeleteObjects(List<DeletedObject> dO,double totalCount,ref double currentVal) {
			int LocalBatchSize=BatchSize;
			if(IsTroubleshootMode) {
				LocalBatchSize=1;
			}
			for(int start=0;start<dO.Count;start+=LocalBatchSize) {
				try {
				if((start+LocalBatchSize)>dO.Count) {
					mb.DeleteObjects(PrefC.GetString(PrefName.RegistrationKey),dO.ToArray()); //dennis check this - why is it not done in batches.
					LocalBatchSize=dO.Count-start;
				}
				currentVal+=BatchSize;
				if(Application.OpenForms["FormProgress"]!=null) {// without this line the following error is thrown: "Invoke or BeginInvoke cannot be called on a control until the window handle has been created." or a null pointer exception is thrown when an automatic synch is done by the system.
					FormP.Invoke(new PassProgressDelegate(PassProgressToDialog),
						new object[] {currentVal,"?currentVal of ?maxVal records uploaded",totalCount,"" });
				}
								}
				catch(Exception e) {
					if(IsTroubleshootMode) {
						//string errorMessage="DeleteObjects with Primary Key = "+BlockPKNumList.First() + " failed to synch. " +  "\n" + e.Message;
						//throw new Exception(errorMessage);
					}
					else {
						throw e;
					}
				}
			}//for loop ends here
			
		}
		*/
		/// <summary>An empty method to test if the webservice is up and running. This was made with the intention of testing the correctness of the webservice URL. If an incorrect webservice URL is used in a background thread the exception cannot be handled easily to a point where even a correct URL cannot be keyed in by the user. Because an exception in a background thread closes the Form which spawned it.</summary>
		private static bool TestWebServiceExists() {
			try {
				mb.Url=PrefC.GetString(PrefName.MobileSyncServerURL);
				if(mb.ServiceExists()) {
					return true;
				}
			}
			catch {
				return false;
			}
			return false;
		}

		private bool VerifyPaidCustomer() {
			//if(textMobileSyncServerURL.Text.Contains("192.168.0.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
			if(textMobileSyncServerURL.Text.Contains("10.10.1.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
				IgnoreCertificateErrors();
			}
			bool isPaidCustomer=mb.IsPaidCustomer(PrefC.GetString(PrefName.RegistrationKey));
			if(!isPaidCustomer) {
				textSynchMinutes.Text="0";
				Prefs.UpdateInt(PrefName.MobileSyncIntervalMinutes,0);
				_changed=true;
				MsgBox.Show(this,"This feature requires a separate monthly payment.  Please call customer support.");
				return false;
			}
			return true;
		}

		///<summary>Called from FormOpenDental and from FormEhrOnlineAccess.  doForce is set to false to follow regular synching interval.</summary>
		public static void SynchFromMain(bool doForce) {
			if(Application.OpenForms["FormPatientPortalSetup"]!=null) {//tested.  This prevents main synch whenever this form is open.
				return;
			}
			if(_isSynching) {
				return;
			}
			DateTime timeSynchStarted=MiscData.GetNowDateTime();
			if(!doForce) {//if doForce, we skip checking the interval
				if(timeSynchStarted < PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun).AddMinutes(PrefC.GetInt(PrefName.MobileSyncIntervalMinutes))) {
					return;
				}
			}
			//if(PrefC.GetString(PrefName.MobileSyncServerURL).Contains("192.168.0.196") || PrefC.GetString(PrefName.MobileSyncServerURL).Contains("localhost")) {
			if(PrefC.GetString(PrefName.MobileSyncServerURL).Contains("10.10.1.196") || PrefC.GetString(PrefName.MobileSyncServerURL).Contains("localhost")) {
				IgnoreCertificateErrors();
			}
			if(!TestWebServiceExists()) {
				if(!doForce) {//if being used from FormOpenDental as part of timer
					if(_isServerAvail) {//this will only happen the first time to prevent multiple windows.
						_isServerAvail=false;
						DialogResult res=MessageBox.Show("Mobile synch server not available.  Synch failed.  Turn off synch?","",MessageBoxButtons.YesNo);
						if(res==DialogResult.Yes) {
							Prefs.UpdateInt(PrefName.MobileSyncIntervalMinutes,0);
						}
					}
				}
				return;
			}
			else {
				_isServerAvail=true;
			}
			DateTime changedSince=PrefC.GetDateT(PrefName.MobileSyncDateTimeLastRun);
			//FormProgress FormP=new FormProgress();//but we won't display it.
			//FormP.NumberFormat="";
			//FormP.DisplayText="";
			//start the thread that will perform the upload
			ThreadStart uploadDelegate= delegate { UploadWorker(changedSince,timeSynchStarted); };
			Thread workerThread=new Thread(uploadDelegate);
			workerThread.Start();
		}

		#region Testing
		///<summary>This allows the code to continue by not throwing an exception even if there is a problem with the security certificate.</summary>
		private static void IgnoreCertificateErrors() {
			System.Net.ServicePointManager.ServerCertificateValidationCallback+=
			delegate(object sender,System.Security.Cryptography.X509Certificates.X509Certificate certificate,
									System.Security.Cryptography.X509Certificates.X509Chain chain,
									System.Net.Security.SslPolicyErrors sslPolicyErrors) {
				return true;
			};
		}

		/// <summary>For testing only</summary>
		private static void CreatePatients(int PatientCount) {
			for(int i=0;i<PatientCount;i++) {
				Patient newPat=new Patient();
				newPat.LName="Mathew"+i;
				newPat.FName="Dennis"+i;
				newPat.Address="Address Line 1.Address Line 1___"+i;
				newPat.Address2="Address Line 2. Address Line 2__"+i;
				newPat.AddrNote="Lives off in far off Siberia Lives off in far off Siberia"+i;
				newPat.AdmitDate=new DateTime(1985,3,3).AddDays(i);
				newPat.ApptModNote="Flies from Siberia on specially chartered flight piloted by goblins:)"+i;
				newPat.AskToArriveEarly=1555;
				newPat.BillingType=3;
				newPat.ChartNumber="111111"+i;
				newPat.City="NL";
				newPat.ClinicNum=i;
				newPat.CreditType="A";
				newPat.DateFirstVisit=new DateTime(1985,3,3).AddDays(i);
				newPat.Email="dennis.mathew________________seb@siberiacrawlmail.com";
				newPat.HmPhone="416-222-5678";
				newPat.WkPhone="416-222-5678";
				newPat.Zip="M3L 2L9";
				newPat.WirelessPhone="416-222-5678";
				newPat.Birthdate=new DateTime(1970,3,3).AddDays(i);
				Patients.Insert(newPat,false);
				//set Guarantor field the same as PatNum
				Patient patOld=newPat.Copy();
				newPat.Guarantor=newPat.PatNum;
				Patients.Update(newPat,patOld);
			}
		}

		/// <summary>For testing only</summary>
		private static void CreateAppointments(int AppointmentCount) {
			long[] patNumArray=Patients.GetAllPatNums(true);
			DateTime appdate= DateTime.Now;
			for(int i=0;i<patNumArray.Length;i++) {
				appdate=appdate.AddMinutes(20);
				for(int j=0;j<AppointmentCount;j++) {
					Appointment apt=new Appointment();
					appdate=appdate.AddMinutes(20);
					apt.PatNum=patNumArray[i];
					apt.DateTimeArrived=appdate;
					apt.DateTimeAskedToArrive=appdate;
					apt.DateTimeDismissed=appdate;
					apt.DateTimeSeated=appdate;
					apt.AptDateTime=appdate;
					apt.Note="some notenote noten otenotenot enotenot enote"+j;
					apt.IsNewPatient=true;
					apt.ProvNum=3;
					apt.AptStatus=ApptStatus.Scheduled;
					apt.AptDateTime=appdate;
					apt.Op=2;
					apt.Pattern="//XX//////";
					apt.ProcDescript="4-BWX";
					apt.ProcsColored="<span color=\"-16777216\">4-BWX</span>";
					Appointments.Insert(apt);
				}
			}
		}

		/// <summary>For testing only</summary>
		private static void CreatePrescriptions(int PrescriptionCount) {
			long[] patNumArray=Patients.GetAllPatNums(true);
			for(int i=0;i<patNumArray.Length;i++) {
				for(int j=0;j<PrescriptionCount;j++) {
					RxPat rxpat= new RxPat();
					rxpat.Drug="VicodinA VicodinB VicodinC"+j;
					rxpat.Disp="50.50";
					rxpat.IsControlled=true;
					rxpat.PatNum=patNumArray[i];
					rxpat.RxDate=new DateTime(2010,12,1,11,0,0);
					RxPats.Insert(rxpat);
				}
			}
		}

		private static void CreateStatements(int StatementCount) {
			long[] patNumArray=Patients.GetAllPatNums(true);
			for(int i=0;i<patNumArray.Length;i++) {
				for(int j=0;j<StatementCount;j++) {
					Statement st= new Statement();
					st.DateSent=new DateTime(2010,12,1,11,0,0).AddDays(1+j);
					st.DocNum=i+j;
					st.PatNum=patNumArray[i];
					Statements.Insert(st);
				}
			}
		}

		#endregion Testing

		#endregion

		#region web sched
		private void butWebSchedEnable_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			labelWebSchedEnable.Text="";
			Application.DoEvents();
			//The enable button is not enabled for offices that already have the service enabled.  Therefore go straight to making the web call to our service.
			Cursor.Current=Cursors.WaitCursor;
			#region Web Service Settings
#if DEBUG
			OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
#else
			OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
			updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress) !="") {
				IWebProxy proxy = new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				updateService.Proxy=proxy;
			}
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
			}
			#endregion
			string result="";
			try {
				result=updateService.ValidateWebSched(strbuild.ToString());
			}
			catch {
				//Our service might be down or the client might not be connected to the internet or the transmission got cut off somehow.
			}
			Cursor.Current=Cursors.Default;
			string error="";
			int errorCode=0;
			if(Recalls.IsWebSchedResponseValid(result,out error,out errorCode)) {
				//Everything went good, the office is actively on support and has an active WebSched repeating charge.
				butWebSchedEnable.Enabled=false;
				labelWebSchedEnable.Text=Lan.g(this,"The Web Sched service has been enabled.");
				//This if statement will only save database calls in the off chance that this window was originally loaded with the pref turned off and got turned on by another computer while open.
				if(Prefs.UpdateBool(PrefName.WebSchedService,true)) {
					_changed=true;
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"The Web Sched service was enabled.");
				}
				return;
			}
			#region Error Handling
			//At this point we know something went wrong.  So we need to give the user a hint as to why they can't enable the Web Sched service.
			if(errorCode==110) {//Customer not registered for WebSched monthly service
				//We want to launch our Web Sched page if the user is not signed up:
				try {
					Process.Start(Recalls.GetWebSchedPromoURL());
				}
				catch(Exception) {
					//The promotional web site can't be shown, most likely due to the computer not having a default browser.  Simply don't do anything.
				}
				//Just in case no browser was opened for them, make the message next to the button say something now so that they can visually see that something should have happened.
				labelWebSchedEnable.Text=error;
				return;
			}
			else if(errorCode==120) {
				labelWebSchedEnable.Text=error;
				return;
			}
			//For every other error message returned, we'll simply show a generic error in the label and display the detailed error in a pop up.
			labelWebSchedEnable.Text=Lan.g(this,"There was a problem enabling the Web Sched.  Please give us a call or try again.");
			MessageBox.Show(error);
			#endregion
		}

		private void butSignUp_Click(object sender,EventArgs e) {
			try {
				Process.Start(Recalls.GetWebSchedPromoURL());
			}
			catch(Exception) {
				//The promotional web site can't be shown, most likely due to the computer not having a default browser.
				MessageBox.Show(Lan.g(this,"Sign up page could not load.  Please visit the following web site")+":\r\n"+Recalls.GetWebSchedPromoURL());
			}
		}

		private void butWebSchedSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRecallSetup FormRS=new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall");
		}

		private void butRecallTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRecallTypes FormRT=new FormRecallTypes();
			FormRT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Types");
		}

		private void butOperatories_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories");
		}
		#endregion

		#region Listener Service

		///<summary>Updates the text box that is displaying the current status of the Listener Service.  Returns the status just in case other logic is needed outside of updating the status box.</summary>
		private eServiceSignalSeverity FillTextListenerServiceStatus() {
			eServiceSignalSeverity eServiceStatus=EServiceSignals.GetServiceStatus(eServiceCode.ListenerService);
			if(eServiceStatus==eServiceSignalSeverity.Critical) {
				textListenerServiceStatus.BackColor=COLOR_ESERVICE_ALERT_BACKGROUND;
				textListenerServiceStatus.ForeColor=COLOR_ESERVICE_ALERT_TEXT;
				butStartListenerService.Enabled=true;
			}
			else {
				textListenerServiceStatus.BackColor=SystemColors.Control;
				textListenerServiceStatus.ForeColor=SystemColors.WindowText;
				butStartListenerService.Enabled=false;
			}
			textListenerServiceStatus.Text=eServiceStatus.ToString();
			return eServiceStatus;
		}

		private void FillGridListenerService() {
			//Display some historical information for the last 30 days in this grid about the lifespan of the listener heartbeats.
			List<EServiceSignal> listESignals=EServiceSignals.GetServiceHistory(eServiceCode.ListenerService,DateTime.Today.AddDays(-30),DateTime.Today);
			gridListenerServiceStatusHistory.BeginUpdate();
			gridListenerServiceStatusHistory.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"DateTime"),120);
			gridListenerServiceStatusHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Status"),90);
			gridListenerServiceStatusHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Details"),0);
			gridListenerServiceStatusHistory.Columns.Add(col);
			gridListenerServiceStatusHistory.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listESignals.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listESignals[i].SigDateTime.ToString());
				row.Cells.Add(listESignals[i].Severity.ToString());
				row.Cells.Add(listESignals[i].Description.ToString());
				gridListenerServiceStatusHistory.Rows.Add(row);
			}
			gridListenerServiceStatusHistory.EndUpdate();
		}

		private void butSaveListenerPort_Click(object sender,EventArgs e) {
			if(textListenerPort.errorProvider1.GetError(textListenerPort)!="") {
				MessageBox.Show(Lan.g(this,"Listener Port must be a number between 0-65535."));
				return;
			}
			if(Prefs.UpdateString(PrefName.CustListenerPort,textListenerPort.Text)) {
				_changed=true;//Sends invalid signal upon closing the form.
			}
			MsgBox.Show(this,"Listener Port Saved");
		}

		private void butStartListenerService_Click(object sender,EventArgs e) {
			//No setup permission check here so that anyone can hopefully get the service back up and running.
			//Check to see if the service started up on its own while we were in this window.
			if(FillTextListenerServiceStatus()==eServiceSignalSeverity.Working) {
				//Use a slightly different message than below so that we can easily tell which part of this method customers reached.
				MsgBox.Show(this,"Listener Service already started.  Please call us for support if eServices are still not working.");
				return;
			}
			//Check to see if the listener service is installed on this computer.
			List<ServiceController> listServices=ODEnvironment.GetAllOpenDentServices();
			ServiceController listenerService=null;
			//Look for the service that uses "OpenDentalCustListener.exe"
			for(int i=0;i<listServices.Count;i++) {
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+listServices[i].ServiceName);
				string test=hklm.GetValue("ImagePath").ToString();
				string test1=test.Replace("\"","");
				string[] arrayExePath=hklm.GetValue("ImagePath").ToString().Replace("\"","").Split('\\');
				//This will not work if in the future we allow command line args for the listener service that include paths.
				if(arrayExePath[arrayExePath.Length-1]=="OpenDentalCustListener.exe") {
					listenerService=listServices[i];
					break;
				}
			}
			if(listenerService==null) {
				MsgBox.Show(this,"Listener Service was not found on this computer.  The service can only be started from the computer that is hosting the Listener Service.");
				return;
			}
			//The listener service is installed on this computer.  Try to start it if it is in a stopped or stop pending status.
			//If we do not do this, an InvalidOperationException will throw that says "An instance of the service is already running"
			if(listenerService.Status==ServiceControllerStatus.Stopped || listenerService.Status==ServiceControllerStatus.StopPending) {
				try {
					Cursor=Cursors.WaitCursor;
					listenerService.Start();
					listenerService.WaitForStatus(ServiceControllerStatus.Running,new TimeSpan(0,0,7));
					Cursor=Cursors.Default;
				}
				catch {
					Cursor=Cursors.Default;
					//An InvalidOperationException can get thrown if the service could not be started.  E.g. current user is not running Open Dental as an administrator.
					MsgBox.Show(this,"There was a problem starting the Listener Service.  Please go manually start your Listener Service or call us for help.");
					return;
				}
				MsgBox.Show(this,"Listener Service Started.");
			}
			else {
				MsgBox.Show(this,"Listener Service is already running.  Please call us for support if eServices are still not working.");
			}
			FillTextListenerServiceStatus();
			FillGridListenerService();
		}

		private void butListenerServiceHistoryRefresh_Click(object sender,EventArgs e) {
			FillTextListenerServiceStatus();
			FillGridListenerService();
		}

		private void butListenerAlertsOff_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			//Insert a row into the eservicesignal table to indicate to all computers to stop monitoring.
			EServiceSignal signalDisable=new EServiceSignal();
			signalDisable.Description="Stop Monitoring clicked from setup window.";
			signalDisable.IsProcessed=true;
			signalDisable.ReasonCategory=0;
			signalDisable.ReasonCode=0;
			signalDisable.ServiceCode=(int)eServiceCode.ListenerService;
			signalDisable.Severity=eServiceSignalSeverity.NotEnabled;
			signalDisable.Tag="";
			signalDisable.SigDateTime=DateTime.Now;
			EServiceSignals.Insert(signalDisable);
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Listener Service monitoring manually stopped via eServices Setup window.");
			MsgBox.Show(this,"Monitoring shutdown signal sent.  This will take up to one minute.");
			FillGridListenerService();
			FillTextListenerServiceStatus();
		}

		#endregion

		private void tabControl_SelectedIndexChanged(object sender,EventArgs e) {
			SetControlEnabledState();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormPatientPortalSetup_FormClosed(object sender,FormClosedEventArgs e) {
			if(_changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private enum SynchEntity {
			patient,
			appointment,
			prescription,
			provider,
			pharmacy,
			labpanel,
			labresult,
			medication,
			medicationpat,
			allergy,
			allergydef,
			disease,
			diseasedef,
			icd9,
			statement,
			document,
			recall,
			deletedobject,
			patientdel
		}

		///<summary>Typically used in ctor determine which tab should be activated be default.</summary>
		public enum EService {
			PatientPortal,
			MobileOld,
			MobileNew,
			WebSched,
			ListenerService,
		}
	}
}