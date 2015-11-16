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
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;

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
		///<summary>The background color used when the OpenDentalCustListener service is down.  Using Red was deemed too harsh.
		///This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_CRITICAL_BACKGROUND=Color.OrangeRed;
		///<summary>The text color used when the OpenDentalCustListener service is down.
		///This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_CRITICAL_TEXT=Color.Yellow;
		///<summary>The background color used when the OpenDentalCustListener service has an error that has not be processed.
		///This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_ERROR_BACKGROUND=Color.LightGoldenrodYellow;
		///<summary>The text color used when the OpenDentalCustListener service has an error that has not be processed.
		///This variable should be treated as a constant which is why it is in all caps.  The type 'System.Drawing.Color' cannot be declared const.</summary>
		private Color COLOR_ESERVICE_ERROR_TEXT=Color.OrangeRed;
		private Clinic _clinicCur;
		///<summary>A list of clinics that the user currently logged into has access to.</summary>
		private List<Clinic> _listClinics;
		///<summary>A list of all clinics.  This list could include clinics that the user should not have access to so be careful using it.</summary>
		private List<Clinic> _listAllClinics;
		private List<SmsPhone> _listPhones;
		private List<RecallType> _listRecallTypes;
		///<summary>A list of all operatories that have IsWebSched set to true.</summary>
		private List<Operatory> _listWebSchedOps;
		///<summary>A deep copy of ProviderC.GetListShort().  Use the cache instead of this list if you need an up to date list of providers.</summary>
		private List<Provider> _listProviders;
		///<summary>Provider number used to filter the Time Slots grid.  0 is treated as 'All'</summary>
		private long _webSchedProvNum=0;
		///<summary>Clinic number used to filter the Time Slots grid.  0 is treated as 'Unassigned'</summary>
		private long _webSchedClinicNum=0;
		private ListenerServiceType _listenerType=ListenerServiceType.NoListener;
		private WebServiceMainHQ _webServiceMain {
			get {
				return WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			}
		}
		
		///<summary>Launches the eServices Setup window defaulted to the tab of the eService passed in.</summary>
		public FormEServicesSetup(EService setTab=EService.PatientPortal) {
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
				case EService.SmsService:
					tabControl.SelectTab(tabSmsServices);
					break;
				case EService.PatientPortal:
				default:
					tabControl.SelectTab(tabPatientPortal);
					break;
			}
		}

		private void FormEServicesSetup_Load(object sender,EventArgs e) {
			textRedirectUrlPatientPortal.Text=PrefC.GetString(PrefName.PatientPortalURL);
			textBoxNotificationSubject.Text=PrefC.GetString(PrefName.PatientPortalNotifySubject);
			textBoxNotificationBody.Text=PrefC.GetString(PrefName.PatientPortalNotifyBody);
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
			textWebSchedDateStart.Text=DateTime.Today.ToShortDateString();
			comboWebSchedClinic.Items.Clear();
			comboWebSchedClinic.Items.Add(Lan.g(this,"Unassigned"));
			_listAllClinics=Clinics.GetList().ToList();
			for(int i=0;i<_listAllClinics.Count;i++) {
				comboWebSchedClinic.Items.Add(_listAllClinics[i].Description);
			}
			comboWebSchedClinic.SelectedIndex=0;
			_listProviders=ProviderC.GetListShort();
			comboWebSchedProviders.Items.Clear();
			comboWebSchedProviders.Items.Add(Lan.g(this,"All"));
			for(int i=0;i<_listProviders.Count;i++) {
				comboWebSchedProviders.Items.Add(_listProviders[i].GetLongDesc());
			}
			comboWebSchedProviders.SelectedIndex=0;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelWebSchedClinic.Visible=false;
				comboWebSchedClinic.Visible=false;
				butWebSchedPickClinic.Visible=false;
			}
			FillGridWebSchedRecallTypes();
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlots();
			listBoxWebSchedProviderPref.SelectedIndex=PrefC.GetInt(PrefName.WebSchedProviderRule);
			#endregion
			#region Listener Service
			textListenerPort.Text=PrefC.GetString(PrefName.CustListenerPort);
			try {
				_listenerType=WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
					_webServiceMain.GetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey)))
				);
			}
			catch(Exception ex) {
				checkAllowEConnectorComm.Enabled=false;
			}
			SetEConnectorCommunicationStatus();
			//Check to see if the eConnector service is already installed.  If it is, disable the install button.
			//Users who want to install multiple on one computer can use the Service Manager instead.
			try {
				if(ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe").Count > 0) {
					butInstallEConnector.Enabled=false;
				}
			}
			catch(Exception) {
				//Do nothing.  The Install button will simply be visible.
			}
			FillTextListenerServiceStatus();
			FillGridListenerService();
			#endregion
			#region Sms Service
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			if(_clinicCur==null && _listClinics.Count>0) {
				_clinicCur=_listClinics[0];//default to first clinic in list, if no clinics were passed into this form using the constructor.
			}
			FillComboClinicSms();
			textCountryCode.Text=CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2);
			FillGridClinics();
			FillGridSmsUsage();
			SetSmsServiceAgreement();
			#endregion
			SetControlEnabledState();
		}

		private void FillComboClinicSms() {
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinicSms.Items.Add(PrefC.GetString(PrefName.PracticeTitle));
				comboClinicSms.SelectedIndex=0;
				comboClinicSms.Enabled=false;
			}
			else {
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinicSms.Items.Add(_listClinics[i].Description);
				}
				if(comboClinicSms.Items.Count>0) {
					comboClinicSms.SelectedIndex=0;//select first clinic in list
				}
			}
		}

		private void SetControlEnabledState() {
			if(!Security.IsAuthorized(Permissions.EServicesSetup)) {
				//Disable certain buttons but let them continue to view
				butSavePatientPortal.Enabled=false;
				butGetUrlPatientPortal.Enabled=false;
				groupBoxNotification.Enabled=false;
				textListenerPort.Enabled=false;
				butListenerServiceAck.Enabled=false;
				butSaveListenerPort.Enabled=false;
				butWebSchedEnable.Enabled=false;
				listBoxWebSchedProviderPref.Enabled=false;
				butRecallSchedSetup.Enabled=false;
				((Control)tabMobileOld).Enabled=false;
				return;
			}
			textListenerPort.Enabled=tabControl.SelectedTab!=tabMobileOld;
			butSaveListenerPort.Enabled=tabControl.SelectedTab!=tabMobileOld;
		}

		#region patient portal
		private void butGetUrlPatientPortal_Click(object sender,EventArgs e) {
			try {
				string url=CustomerUpdatesProxy.GetHostedURL(eServiceCode.PatientPortal);
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
				string url=CustomerUpdatesProxy.GetHostedURL(eServiceCode.MobileWeb);
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
		///<summary>Also refreshed the combo box of available recall types.</summary>
		private void FillGridWebSchedRecallTypes() {
			//Keep track of the previously selected recall type.
			long selectedRecallTypeNum=0;
			if(comboWebSchedRecallTypes.SelectedIndex!=-1) {
				selectedRecallTypeNum=_listRecallTypes[comboWebSchedRecallTypes.SelectedIndex].RecallTypeNum;
			}
			//Fill the combo boxes for the time slots preview.
			comboWebSchedRecallTypes.Items.Clear();
			_listRecallTypes=RecallTypeC.GetListt();
			for(int i=0;i<_listRecallTypes.Count;i++) {
				comboWebSchedRecallTypes.Items.Add(_listRecallTypes[i].Description);
				if(_listRecallTypes[i].RecallTypeNum==selectedRecallTypeNum) {
					comboWebSchedRecallTypes.SelectedIndex=i;
				}
			}
			if(selectedRecallTypeNum==0 && comboWebSchedRecallTypes.Items.Count > 0) {
				comboWebSchedRecallTypes.SelectedIndex=0;//Arbitrarily select the first recall type.
			}
			gridWebSchedRecallTypes.BeginUpdate();
			gridWebSchedRecallTypes.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRecallTypes","Description"),130);
			gridWebSchedRecallTypes.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallTypes","Time Pattern"),100);
			gridWebSchedRecallTypes.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallTypes","Time Length"),0);
			col.TextAlign=HorizontalAlignment.Center;
			gridWebSchedRecallTypes.Columns.Add(col);
			gridWebSchedRecallTypes.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listRecallTypes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listRecallTypes[i].Description);
				row.Cells.Add(_listRecallTypes[i].TimePattern);
				int timeLength=RecallTypes.ConvertTimePattern(_listRecallTypes[i].TimePattern).Length * 5;
				if(timeLength==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(timeLength.ToString()+" "+Lan.g("TableRecallTypes","mins"));
				}
				gridWebSchedRecallTypes.Rows.Add(row);
			}
			gridWebSchedRecallTypes.EndUpdate();
		}

		private void FillGridWebSchedOperatories() {
			_listWebSchedOps=Operatories.GetOpsForWebSched();
			gridWebSchedOperatories.BeginUpdate();
			gridWebSchedOperatories.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableOperatories","Op Name"),170);
			gridWebSchedOperatories.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Abbrev"),70);
			gridWebSchedOperatories.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Clinic"),80);
			gridWebSchedOperatories.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Provider"),90);
			gridWebSchedOperatories.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Hygienist"),90);
			gridWebSchedOperatories.Columns.Add(col);
			gridWebSchedOperatories.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listWebSchedOps.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listWebSchedOps[i].OpName);
				row.Cells.Add(_listWebSchedOps[i].Abbrev);
				row.Cells.Add(Clinics.GetDesc(_listWebSchedOps[i].ClinicNum));
				row.Cells.Add(Providers.GetAbbr(_listWebSchedOps[i].ProvDentist));
				row.Cells.Add(Providers.GetAbbr(_listWebSchedOps[i].ProvHygienist));
				gridWebSchedOperatories.Rows.Add(row);
			}
			gridWebSchedOperatories.EndUpdate();
		}

		private void FillGridWebSchedTimeSlots() {
			//Validate time slot settings.
			if(textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart)!="") {
				//Don't bother warning the user.  It will just be annoying.  The red indecator should be sufficient.
				return;
			}
			if(comboWebSchedRecallTypes.SelectedIndex < 0
				|| comboWebSchedClinic.SelectedIndex < 0
				|| comboWebSchedProviders.SelectedIndex < 0) 
			{
				return;
			}
			DateTime dateStart=PIn.Date(textWebSchedDateStart.Text);
			RecallType recallType=_listRecallTypes[comboWebSchedRecallTypes.SelectedIndex];
			Clinic clinic=_listAllClinics.Find(x => x.ClinicNum==_webSchedClinicNum);//null clinic is treated as unassigned.
			List<Provider> listProviders=new List<Provider>(_listProviders);//Use all providers by default.
			Provider provider=_listProviders.Find(x => x.ProvNum==_webSchedProvNum);
			if(provider!=null) {
				//Only use the provider that the user picked from the provider picker.
				listProviders=new List<Provider>() { provider };
			}
			Cursor=Cursors.WaitCursor;
			DataTable tableTimeSlots=new DataTable();
			try {
				//Get the next 30 days of open time schedules with the current settings
				tableTimeSlots=Recalls.GetAvailableWebSchedTimeSlots(recallType,listProviders,clinic,dateStart,dateStart.AddDays(30));
			}
			catch(Exception ex) {
				//The user might not have Web Sched ops set up correctly.  Don't warn them here because it is just annoying.  They'll figure it out.
			}
			Cursor=Cursors.Default;
			gridWebSchedTimeSlots.BeginUpdate();
			gridWebSchedTimeSlots.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",0);
			col.TextAlign=HorizontalAlignment.Center;
			gridWebSchedTimeSlots.Columns.Add(col);
			gridWebSchedTimeSlots.Rows.Clear();
			ODGridRow row;
			DateTime dateTimeSlotLast=DateTime.MinValue;
			for(int i=0;i<tableTimeSlots.Rows.Count;i++) {
				DateTime dateTimeSlot=PIn.Date(tableTimeSlots.Rows[i]["SchedDate"].ToString());
				//Make a new row for every unique day.
				if(dateTimeSlotLast.Date!=dateTimeSlot.Date) {
					dateTimeSlotLast=dateTimeSlot;
					row=new ODGridRow();
					row.ColorBackG=Color.LightBlue;
					row.Cells.Add(dateTimeSlot.ToShortDateString());
					gridWebSchedTimeSlots.Rows.Add(row);
				}
				row=new ODGridRow();
				DateTime timeStart=PIn.DateT(tableTimeSlots.Rows[i]["TimeStart"].ToString());
				DateTime timeStop=PIn.DateT(tableTimeSlots.Rows[i]["TimeStop"].ToString());
				row.Cells.Add(timeStart.ToShortTimeString()+" - "+timeStop.ToShortTimeString());
				gridWebSchedTimeSlots.Rows.Add(row);
			}
			gridWebSchedTimeSlots.EndUpdate();
		}

		private void gridWebSchedRecallTypes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRecallTypes FormRT=new FormRecallTypes();
			FormRT.ShowDialog();
			FillGridWebSchedRecallTypes();
			FillGridWebSchedTimeSlots();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Types accessed via EServices Setup window.");
		}

		private void gridWebSchedOperatories_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlots();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories accessed via EServices Setup window.");
		}

		private void listBoxWebSchedProviderPref_SelectedIndexChanged(object sender,EventArgs e) {
			if(Prefs.UpdateInt(PrefName.WebSchedProviderRule,listBoxWebSchedProviderPref.SelectedIndex)) {
				_changed=true;
			}
		}

		private void comboWebSchedRecallTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridWebSchedTimeSlots();
		}

		private void comboWebSchedProviders_SelectionChangeCommitted(object sender,EventArgs e) {
			_webSchedProvNum=0;
			if(comboWebSchedProviders.SelectedIndex > 0) {//Greater than 0 due to "All"
				_webSchedProvNum=_listProviders[comboWebSchedProviders.SelectedIndex-1].ProvNum;//-1 for 'All'
			}
			FillGridWebSchedTimeSlots();
		}

		private void comboWebSchedClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			_webSchedClinicNum=0;
			if(comboWebSchedClinic.SelectedIndex > 0) {//Greater than 0 due to "Unassigned"
				_webSchedClinicNum=_listAllClinics[comboWebSchedClinic.SelectedIndex-1].ClinicNum;//-1 for 'Unassigned'
			}
			FillGridWebSchedTimeSlots();
		}

		private void textWebSchedDateStart_TextChanged(object sender,EventArgs e) {
			//Only refresh the grid if the user has typed in a valid date.
			if(textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart)=="") {
				FillGridWebSchedTimeSlots();
			}
		}

		private void butWebSchedEnable_Click(object sender,EventArgs e) {
			labelWebSchedEnable.Text="";
			Application.DoEvents();
			//The enable button is not enabled for offices that already have the service enabled.  Therefore go straight to making the web call to our service.
			Cursor.Current=Cursors.WaitCursor;
			string error="";
			try {
				Recalls.ValidateWebSched();
			}
			catch(Exception ex) {
				//Prep a generic error response just in case something unexpected went wrong.
				error=Lan.g(this,"There was a problem enabling the Web Sched.  Please give us a call or try again.");
				if(ex.GetType()==typeof(ODException)) {
					error=ex.Message;//Show special errors for ODExceptions that were already translated.
					//At this point we know something went wrong.  So we need to give the user a hint as to why they can't enable
					if(((ODException)ex).ErrorCode==110) {//Customer not registered for Web Sched monthly service
						//We want to launch our Web Sched page if the user is not signed up:
						try {
							Process.Start(Recalls.GetWebSchedPromoURL());
						}
						catch(Exception) {
							//The promotional web site can't be shown, most likely due to the computer not having a default browser.  Simply do nothing.
						}
					}
				}
			}
			Cursor.Current=Cursors.Default;
			if(error!="") {
				//Just in case no browser was opened for them, make the message next to the button say something now so that they can visually see that something should have happened.
				labelWebSchedEnable.Text=error;
				MessageBox.Show(error);
				return;
			}
			//Everything went good, the office is actively on support and has an active WebSched repeating charge.
			butWebSchedEnable.Enabled=false;
			labelWebSchedEnable.Text=Lan.g(this,"The Web Sched service has been enabled.");
			//This if statement will only save database calls in the off chance that this window was originally loaded with the pref turned off and got turned on by another computer while open.
			if(Prefs.UpdateBool(PrefName.WebSchedService,true)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.EServicesSetup,0,"The Web Sched service was enabled.");
			}
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
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Setup accessed via EServices Setup window.");
		}

		private void butWebSchedToday_Click(object sender,EventArgs e) {
			textWebSchedDateStart.Text=DateTime.Today.ToShortDateString();
			//Don't need to call FillTimeSlots because textChanged event already calls it.
		}

		private void butWebSchedPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			if(comboWebSchedProviders.SelectedIndex>0) {
				FormPP.SelectedProvNum=_webSchedProvNum;
			}
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboWebSchedProviders.SelectedIndex=_listProviders.FindIndex(x => x.ProvNum==FormPP.SelectedProvNum)+1;//+1 for 'All'
			_webSchedProvNum=FormPP.SelectedProvNum;
			FillGridWebSchedTimeSlots();
		}

		private void butWebSchedPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			comboWebSchedClinic.SelectedIndex=_listAllClinics.FindIndex(x => x.ClinicNum==FormC.SelectedClinicNum)+1;//+1 for 'Unassigned'
			_webSchedClinicNum=FormC.SelectedClinicNum;
			FillGridWebSchedTimeSlots();
		}
		#endregion

		#region Listener Service

		///<summary>Updates the text box that is displaying the current status of the Listener Service.  Returns the status just in case other logic is needed outside of updating the status box.</summary>
		private eServiceSignalSeverity FillTextListenerServiceStatus() {
			eServiceSignalSeverity eServiceStatus=EServiceSignals.GetListenerServiceStatus();
			if(eServiceStatus==eServiceSignalSeverity.Critical) {
				textListenerServiceStatus.BackColor=COLOR_ESERVICE_CRITICAL_BACKGROUND;
				textListenerServiceStatus.ForeColor=COLOR_ESERVICE_CRITICAL_TEXT;
				butStartListenerService.Enabled=true;
			}
			else if(eServiceStatus==eServiceSignalSeverity.Error) {
				textListenerServiceStatus.BackColor=COLOR_ESERVICE_ERROR_BACKGROUND;
				textListenerServiceStatus.ForeColor=COLOR_ESERVICE_ERROR_TEXT;
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

		private void SetEConnectorCommunicationStatus() {
			textEConnectorListeningType.Text=_listenerType.ToString();
			switch(_listenerType) {
				case ListenerServiceType.ListenerService:
					checkAllowEConnectorComm.Checked=true;
					break;
				case ListenerServiceType.ListenerServiceProxy:
					checkAllowEConnectorComm.Checked=true;
					textListenerPort.Enabled=false;
					break;
				case ListenerServiceType.NoListener:
					checkAllowEConnectorComm.Checked=false;
					break;
				case ListenerServiceType.DisabledByHQ:
				default:
					checkAllowEConnectorComm.Enabled=false;
					break;
			}
		}

		private void butInstallEConnector_Click(object sender,EventArgs e) {
			//Check to see if the update server preference is set.
			//If set, make sure that this is set to the computer currently logged on.
			DialogResult result;
			string updateServerName=PrefC.GetString(PrefName.WebServiceServerName);
			if(!string.IsNullOrEmpty(updateServerName) && !ODEnvironment.IdIsThisComputer(updateServerName.ToLower())) {
				result=MessageBox.Show(Lan.g(this,"The eConnector service should be installed on the Update Server")+": "+updateServerName+"\r\n"
					+Lan.g(this,"Are you trying to install the eConnector on a different computer by accident?"),"",MessageBoxButtons.YesNoCancel);
				//Only saying No to this message box pop up will allow the user to continue (meaning they fully understand what they are getting into).
				if(result!=DialogResult.No) {
					return;
				}
			}
			//Only ask the user if they want to set the Update Server Name preference if it is not already set.
			if(string.IsNullOrEmpty(updateServerName)) {
				result=MessageBox.Show(Lan.g(this,"The computer that has the eConnector service installed should be set as the Update Server.")+"\r\n"
					+Lan.g(this,"Would you like to make this computer the Update Server?"),"",MessageBoxButtons.YesNoCancel);
				if(result==DialogResult.Cancel) {
					return;
				}
				else if(result==DialogResult.Yes) {
					Prefs.UpdateString(PrefName.WebServiceServerName,Dns.GetHostName());
					_changed=true;
				}
			}
			//At this point the user wants to install the eConnector service (or upgrade the old cust listener to the eConnector).
			bool isListening;
			if(!ServicesHelper.UpgradeOrInstallEConnector(false,out isListening)) {
				//Warning messages would have already been shown to the user, simply return.
				return;
			}
			//The eConnector service was successfully installed and is running.
			//If this is the first time installing the eConnector, ask them if they are willing to accept inbound comms from Open Dental.
			if(!PrefC.GetBool(PrefName.EConnectorEnabled)) {
				bool startListenerCommunications=false;
				string messageStartListener=Lan.g(this,"The eConnector you installed is currently not accepting inbound communication.")+"  "
					+Lan.g(this,"eServices will not work as expected untill inbound communication is allowed.")+"\r\n\r\n"
					+Lan.g(this,"Do you want to start accepting inbound communication?");
				if(MessageBox.Show(messageStartListener,"",MessageBoxButtons.YesNo)==DialogResult.Yes) {
					startListenerCommunications=true;
				}
				Prefs.UpdateBool(PrefName.EConnectorEnabled,true);
				_changed=true;
				try {
					_listenerType=WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
						OpenDentBusiness.WebServiceMainHQProxy.GetWebServiceMainHQInstance().SetEConnectorType(
							WebSerializer.SerializePrimitive<string>(OpenDentBusiness.PrefC.GetString(OpenDentBusiness.PrefName.RegistrationKey)),
							startListenerCommunications
						)
					);
				}
				catch(Exception) {
					MsgBox.Show(this,"Could not update the eConnector communication status.  Please contact us to enable eServices.");
				}
			}
			SetEConnectorCommunicationStatus();
			MsgBox.Show(this,"eConnector successfully installed");
			butInstallEConnector.Enabled=false;
			FillTextListenerServiceStatus();
			FillGridListenerService();
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
				//Color the row if it is an error that has not been processed.
				if(listESignals[i].Severity==eServiceSignalSeverity.Error && !listESignals[i].IsProcessed) {
					row.ColorBackG=COLOR_ESERVICE_ERROR_BACKGROUND;
				}
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
			ListenerServiceType listenerTypeOld=_listenerType;
			try {
				_listenerType=WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
					_webServiceMain.SetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey))
						,checkAllowEConnectorComm.Checked)
				);
				SetEConnectorCommunicationStatus();
			}
			catch(Exception) {
				MsgBox.Show(this,"Could not update the eConnector communication status.  Please contact us to enable eServices.");
				return;
			}
			MsgBox.Show(this,"eConnector settings saved.");
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
			List<ServiceController> listOdServices=ODEnvironment.GetAllOpenDentServices();
			List<ServiceController> listListenerServices=new List<ServiceController>();
			//Look for the service that uses "OpenDentalCustListener.exe"
			for(int i=0;i<listOdServices.Count;i++) {
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+listOdServices[i].ServiceName);
				string test=hklm.GetValue("ImagePath").ToString();
				string test1=test.Replace("\"","");
				string[] arrayExePath=hklm.GetValue("ImagePath").ToString().Replace("\"","").Split('\\');
				//This will not work if in the future we allow command line args for the listener service that include paths.
				if(arrayExePath[arrayExePath.Length-1].StartsWith("OpenDentalEConnector.exe")) {
					listListenerServices.Add(listOdServices[i]);
				}
			}
			if(listListenerServices.Count==0) {
				MsgBox.Show(this,"Listener Services were not found on this computer.  The service can only be started from the computer that is hosting Listener Services.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			List<ServiceController> listListenerServicesErrors=new List<ServiceController>();
			for(int i=0;i<listListenerServices.Count;i++) {
				//The listener service is installed on this computer.  Try to start it if it is in a stopped or stop pending status.
				//If we do not do this, an InvalidOperationException will throw that says "An instance of the service is already running"
				if(listListenerServices[i].Status==ServiceControllerStatus.Stopped || listListenerServices[i].Status==ServiceControllerStatus.StopPending) {
					try {
						listListenerServices[i].Start();
						listListenerServices[i].WaitForStatus(ServiceControllerStatus.Running,new TimeSpan(0,0,7));
					}
					catch {
						//An InvalidOperationException can get thrown if the service could not be started.  E.g. current user is not running Open Dental as an administrator.
						listListenerServicesErrors.Add(listListenerServices[i]);
					}
				}
			}
			Cursor=Cursors.Default;
			if(listListenerServicesErrors.Count!=0) {
				string error=Lan.g(this,"There was a problem starting Listener Services.  Please go manually start the following Listener Services")+":";
				for(int i=0;i<listListenerServicesErrors.Count;i++) {
					error+="\r\n"+listListenerServicesErrors[i].DisplayName;
				}
				MessageBox.Show(error);
			}
			else {
				MsgBox.Show(this,"Listener Services Started.");
			}
			FillTextListenerServiceStatus();
			FillGridListenerService();
		}

		private void butListenerServiceHistoryRefresh_Click(object sender,EventArgs e) {
			FillTextListenerServiceStatus();
			FillGridListenerService();
		}

		private void butListenerServiceAck_Click(object sender,EventArgs e) {
			EServiceSignals.ProcessSignalsForSeverity(eServiceSignalSeverity.Error);
			FillTextListenerServiceStatus();
			FillGridListenerService();
			MsgBox.Show(this,"Errors successfully acknowledged.");
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

		#region Sms Services

		///<summary>Called on form load and when typing into Monthly limit box.</summary>
		private void SetSmsServiceAgreement(bool isTyping=false) {
			double smsMonthlyLimit;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Text=Lans.g(this,"Practice Title");
				smsMonthlyLimit=PrefC.GetDouble(PrefName.SmsMonthlyLimit);
			}
			else if(_listClinics.Count==0) {//clinics enabled, but no clinics are present;
				labelClinic.Text=Lans.g(this,"Clinic");
				textSmsLimit.Enabled=false;
				checkSmsAgree.Enabled=false;
				butSmsSubmit.Enabled=false;
				butSmsCancel.Enabled=false;
				butSmsUnsubscribe.Enabled=false;
				comboClinicSms.Enabled=false;
				smsMonthlyLimit=0;
			}
			else {
				labelClinic.Text=Lans.g(this,"Clinic");
				smsMonthlyLimit=_listClinics[comboClinicSms.SelectedIndex].SmsMonthlyLimit;
			}
			//fill text
			if(String.IsNullOrEmpty(textSmsLimit.Text) && !isTyping) {//blank text box, fill with stored value
				textSmsLimit.Text=smsMonthlyLimit.ToString("c",new CultureInfo("en-US"));
			}
			//parse text, which will usually be displayed in USD, unless the user is typing
			double smsMonthlyLimitText=PIn.Double(textSmsLimit.Text.Trim('$'));
			//If they have a non-zero contract amount they should always be able to click unsubscribe.
			if(smsMonthlyLimit>0) {
				butSmsUnsubscribe.Enabled=true;
				butSmsSubmit.Text=Lans.g(this,"Update");
			}
			else {
				butSmsUnsubscribe.Enabled=false;
				butSmsSubmit.Text=Lans.g(this,"Submit");
			}
			//If they have entered something that does not match what is stored in DB enable cancel button.
			if(smsMonthlyLimitText==smsMonthlyLimit) {
				butSmsCancel.Enabled=false;
			}
			else {
				butSmsCancel.Enabled=true;
			}
			//They have typed in blank or 0
			if(smsMonthlyLimitText==0) {
				checkSmsAgree.Enabled=false;
				checkSmsAgree.Checked=false;
			}
			//Either they typed in the same amount or nothing has been edited
			else if(smsMonthlyLimitText==smsMonthlyLimit) {
				checkSmsAgree.Enabled=false;
				checkSmsAgree.Checked=true;
			}
			//They have typed something in that is not zero and does not match their currently set limit.
			else {
				checkSmsAgree.Enabled=true;
				checkSmsAgree.Checked=false;
			}
			butSmsSubmit.Enabled=checkSmsAgree.Checked&checkSmsAgree.Enabled;
		}

		private void checkSmsAgree_CheckedChanged(object sender,EventArgs e) {
			double monthlyLimit=0;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				monthlyLimit=PrefC.GetDouble(PrefName.SmsMonthlyLimit);
			}
			else {
				monthlyLimit=_clinicCur.SmsMonthlyLimit;
			}
			if(checkSmsAgree.Checked //they have checked agree
				&& PIn.Double(textSmsLimit.Text.Trim('$'))>0 //there is a valid dollar amount
				&& PIn.Double(textSmsLimit.Text.Trim('$'))!=monthlyLimit)//and the dollar amount isn't the already signed contract amount. 
			{
				butSmsSubmit.Enabled=true;
			}
			else {
				butSmsSubmit.Enabled=false;
			}
		}

		private void comboClinicSms_SelectedIndexChanged(object sender,EventArgs e) {
			if(_listClinics==null || _listClinics.Count==0) {
				_clinicCur=null;
				return;
			}
			_clinicCur=_listClinics[comboClinicSms.SelectedIndex];
			textSmsLimit.Text="";
			SetSmsServiceAgreement();
		}

		private void FillGridClinics() {
			Clinics.RefreshCache();
			_listClinics=Clinics.GetForUserod(Security.CurUser);//refresh potentially changed data.
			gridClinics.BeginUpdate();
			gridClinics.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Location"),150);
			gridClinics.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Subscribed"),80);
			gridClinics.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Limit"),80);
			gridClinics.Columns.Add(col);
			gridClinics.Rows.Clear();
			ODGridRow row;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				row=new ODGridRow();
				row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle));
				row.Cells.Add(PrefC.GetDate(PrefName.SmsContractDate).Year>1800?"Yes":"No");
				row.Cells.Add((PrefC.GetDouble(PrefName.SmsMonthlyLimit)).ToString("c",new CultureInfo("en-US")));//Charge this month (Must always be in USD)
				gridClinics.Rows.Add(row);
			}
			else {
				for(int i=0;i<_listClinics.Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(_listClinics[i].Description);
					row.Cells.Add(_listClinics[i].SmsContractDate.Year>1800?"Yes":"No");
					row.Cells.Add(_listClinics[i].SmsMonthlyLimit.ToString("c",new CultureInfo("en-US")));//Charge this month (Must always be in USD)
					gridClinics.Rows.Add(row);
				}
			}
			gridClinics.EndUpdate();
		}

		private void FillGridSmsUsage() {
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				_listPhones=SmsPhones.GetForPractice();
			}
			else {
				_listPhones=SmsPhones.GetForClinics(_listClinics.Select(x=>x.ClinicNum).ToList()); //new List<Clinic> { _clinicCur });
			}
			gridSmsSummary.BeginUpdate();
			gridSmsSummary.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Location"),120,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Primary\r\nPhone Number"),105,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Country\r\nCode"),60,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			//col=new ODGridColumn(Lan.g(this,"Sent\r\nAll Time"),70,HorizontalAlignment.Right);
			//gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Sent\r\nFor Month"),70,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Sent\r\nCharges"),70,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			//col=new ODGridColumn(Lan.g(this,"Recieved\r\nAll Time"),70,HorizontalAlignment.Right);
			//gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Recieved\r\nFor Month"),70,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Recieved\r\nCharges"),70,HorizontalAlignment.Right);
			gridSmsSummary.Columns.Add(col);
			gridSmsSummary.Rows.Clear();
			DataTable usage=SmsPhones.GetSmsUsageLocal(PrefC.GetBool(PrefName.EasyNoClinics)?new List<long>{0}:_listClinics.Select(x => x.ClinicNum).ToList(),dateTimePickerSms.Value);
			if(usage==null || usage.Rows.Count==0) {
				gridSmsSummary.EndUpdate();
				return;
			}
			//Only add 1 row if not using clinics. Otherwise add 1 row per clinic.
			for(int i=0;(!PrefC.GetBool(PrefName.EasyNoClinics) && i<_listClinics.Count) || (PrefC.GetBool(PrefName.EasyNoClinics) && i==0);i++) {// Or i==0 allows us to use the same code for practice and clinics
				ODGridRow row=new ODGridRow();
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {					
					row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle));
				}
				else {
					row.Cells.Add(_listClinics[i].Description);
				}
				bool hasRow=false;
				foreach(DataRow dataRow in usage.Rows) {
					if(PrefC.GetBool(PrefName.EasyNoClinics)) {
						//do nothing, we want to run through this code for practice level usage
					}
					else if(PIn.Long(dataRow["ClinicNum"].ToString())!=_listClinics[i].ClinicNum) {
						continue;
					}
					hasRow=true;
					row.Cells.Add(dataRow["PhoneNumber"].ToString());
					row.Cells.Add(dataRow["CountryCode"].ToString());
					//row.Cells.Add(dataRow["SentAllTime"].ToString());
					row.Cells.Add(dataRow["SentMonth"].ToString());
					row.Cells.Add(PIn.Double(dataRow["SentCharge"].ToString()).ToString("c",new CultureInfo("en-US")));
					//row.Cells.Add(dataRow["ReceivedAllTime"].ToString());
					row.Cells.Add(dataRow["ReceivedMonth"].ToString());
					row.Cells.Add(PIn.Double(dataRow["ReceivedCharge"].ToString()).ToString("c",new CultureInfo("en-US")));
				}
				if(!hasRow) {
					row.Cells.Add("");//phone number
					row.Cells.Add("");//country code
					//row.Cells.Add("0");//Sent All Time
					row.Cells.Add("0");//Sent Month
					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Sent Charge
					//row.Cells.Add("0");//Rcvd All Time
					row.Cells.Add("0");//Rcvd Month
					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Rcvd Charge
				}
				gridSmsSummary.Rows.Add(row);
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && _listClinics.Count>1) {//Total row if there is more than one clinic (Will not display for practice because practice will have no clinics.
				ODGridRow row=new ODGridRow();
				//long totalSent=0;
				long totalSentMonth=0;
				double totalSentCharge=0f;
				//long totalReceived=0;
				long totalReceivedMonth=0;
				double totalReceivedCharge=0f;
				foreach(DataRow dataRow in usage.Rows) {
					//totalSent+=PIn.Long(dataRow["SentAllTime"].ToString());
					totalSentMonth+=PIn.Long(dataRow["SentMonth"].ToString());
					totalSentCharge+=PIn.Double(dataRow["SentCharge"].ToString());
					//totalReceived+=PIn.Long(dataRow["ReceivedAllTime"].ToString());
					totalReceivedMonth+=PIn.Long(dataRow["ReceivedMonth"].ToString());
					totalReceivedCharge+=PIn.Double(dataRow["ReceivedCharge"].ToString());
				}
				row.Cells.Add("");
				row.Cells.Add("");
				row.Cells.Add(Lans.g(this,"Total"));
				//row.Cells.Add(totalSent.ToString());
				row.Cells.Add(totalSentMonth.ToString());
				row.Cells.Add(totalSentCharge.ToString("c",new CultureInfo("en-US")));
				//row.Cells.Add(totalReceived.ToString());
				row.Cells.Add(totalReceivedMonth.ToString());
				row.Cells.Add(totalReceivedCharge.ToString("c",new CultureInfo("en-US")));
				row.ColorBackG=Color.LightYellow;
				gridSmsSummary.Rows.Add(row);
			}
			gridSmsSummary.EndUpdate();
		}

		/////<summary>Not used. Delete after porting required code form it.</summary>
		//private void FillGridSmsUsageTwo() {
		//	if(PrefC.GetBool(PrefName.EasyNoClinics)) {
		//		_listPhones=SmsPhones.GetForPractice();
		//	}
		//	else {
		//		_listPhones=SmsPhones.GetForClinics(_listClinics);
		//	}
		//	gridSmsSummary.BeginUpdate();
		//	gridSmsSummary.Columns.Clear();
		//	ODGridColumn col=new ODGridColumn(Lan.g(this,"Location"),130);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Subscribed"),50);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Limit"),75,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Virtual Phone #"),130,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Sent All Time"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Sent Last Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Sent This Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Charge This Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Rcvd All Time"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Rcvd Last Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Rcvd This Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	col=new ODGridColumn(Lan.g(this,"Charge This Mo"),60,HorizontalAlignment.Right);
		//	gridSmsSummary.Columns.Add(col);
		//	//ODGridColumn col=new ODGridColumn(Lan.g(this,"Virtual Phone #"),130,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"All Time"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"Sent"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"This Month"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"This Month"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"Received"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"Last Month"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"Received This Month"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	//col=new ODGridColumn(Lan.g(this,"Charge This Month"),60,HorizontalAlignment.Right);
		//	//gridSmsSummary.Columns.Add(col);
		//	gridSmsSummary.Rows.Clear();
		//	ODGridRow row;
		//	Dictionary<string,Dictionary<string,double>> usage=SmsPhones.GetSmsUsageLocal(_listPhones,dateTimePickerSms.Value);
		//	if(PrefC.GetBool(PrefName.EasyNoClinics)) {//Practice level information
		//		row=new ODGridRow();
		//		row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle));
		//		row.Cells.Add(PrefC.GetDate(PrefName.SmsContractDate).Year>1800?"Yes":"No");
		//		row.Cells.Add((PrefC.GetDouble(PrefName.SmsMonthlyLimit)).ToString("c",new CultureInfo("en-US")));//Charge this month (Must always be in USD)
		//		bool firstRow=true;
		//		if(_listPhones.Count==0) {
		//			_listPhones.Add(new SmsPhone() { PhoneNumber="" });//dummy row
		//		}
		//		foreach(SmsPhone phone in _listPhones) {
		//			if(phone.ClinicNum!=0) {
		//				continue;
		//			}
		//			if(!firstRow) {
		//				row=new ODGridRow();
		//				row.Cells.Add("");
		//				row.Cells.Add("");
		//				row.Cells.Add("");
		//			}
		//			row.Cells.Add(phone.PhoneNumber);
		//			if(usage.ContainsKey(phone.PhoneNumber)) {
		//				row.Cells.Add(usage[phone.PhoneNumber]["SentAllTime"].ToString());//Sent All Time
		//				row.Cells.Add(usage[phone.PhoneNumber]["SentLastMonth"].ToString());//Sent Last Month
		//				row.Cells.Add(usage[phone.PhoneNumber]["SentThisMonth"].ToString());//Sent This Month
		//				row.Cells.Add(usage[phone.PhoneNumber]["SentThisMonthCost"].ToString("c",new CultureInfo("en-US")));//Charge this month
		//				row.Cells.Add(usage[phone.PhoneNumber]["InboundAllTime"].ToString());//Rcvd All Time
		//				row.Cells.Add(usage[phone.PhoneNumber]["InboundLastMonth"].ToString());//Rcvd Last Month
		//				row.Cells.Add(usage[phone.PhoneNumber]["InboundThisMonth"].ToString());//Rcvd This Month
		//				row.Cells.Add(usage[phone.PhoneNumber]["InboundThisMonthCost"].ToString("c",new CultureInfo("en-US")));//Charge This Month
		//			}
		//			else {
		//				row.Cells.Add("0");//Sent All Time
		//				row.Cells.Add("0");//Sent Last Month
		//				row.Cells.Add("0");//Sent This Month
		//				row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//				row.Cells.Add("0");//Rcvd All Time
		//				row.Cells.Add("0");//Rcvd Last Month
		//				row.Cells.Add("0");//Rcvd This Month
		//				row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//			}
		//			firstRow=false;
		//			gridSmsSummary.Rows.Add(row);
		//		}
		//		row=new ODGridRow();
		//		row.Cells.Add("");
		//		row.Cells.Add("");
		//		row.Cells.Add("");
		//		row.Cells.Add("TOTALS");
		//		row.Cells.Add("0");//Sent All Time
		//		row.Cells.Add("0");//Sent Last Month
		//		row.Cells.Add("0");//Sent This Month
		//		row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//		row.Cells.Add("0");//Rcvd All Time
		//		row.Cells.Add("0");//Rcvd Last Month
		//		row.Cells.Add("0");//Rcvd This Month
		//		row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//		gridSmsSummary.Rows.Add(row);
		//	}
		//	else {//Using Clinics
		//		for(int i=0;i<_listClinics.Count;i++) {
		//			row=new ODGridRow();
		//			row.Cells.Add(_listClinics[i].Description);
		//			row.Cells.Add(_listClinics[i].SmsContractDate.Year>1800?"Yes":"No");
		//			row.Cells.Add(_listClinics[i].SmsMonthlyLimit.ToString("c",new CultureInfo("en-US")));//Charge this month (Must always be in USD)
		//			bool firstRow=true;
		//			if(_listPhones.Count==0 || !_listPhones.Exists(p => p.ClinicNum==_listClinics[i].ClinicNum)) {
		//				_listPhones.Add(new SmsPhone() { PhoneNumber="",ClinicNum=_listClinics[i].ClinicNum });//dummy row
		//			}
		//			foreach(SmsPhone phone in _listPhones) {
		//				if(phone.ClinicNum!=_listClinics[i].ClinicNum) {
		//					continue;
		//				}
		//				if(!firstRow) {
		//					row=new ODGridRow();
		//					row.Cells.Add("");
		//					row.Cells.Add("");
		//					row.Cells.Add("");
		//				}
		//				row.Cells.Add(phone.PhoneNumber);
		//				if(usage.ContainsKey(phone.PhoneNumber)) {
		//					row.Cells.Add(usage[phone.PhoneNumber]["SentAllTime"].ToString());//Sent All Time
		//					row.Cells.Add(usage[phone.PhoneNumber]["SentLastMonth"].ToString());//Sent Last Month
		//					row.Cells.Add(usage[phone.PhoneNumber]["SentThisMonth"].ToString());//Sent This Month
		//					row.Cells.Add(usage[phone.PhoneNumber]["SentThisMonthCost"].ToString("c",new CultureInfo("en-US")));//Charge this month
		//					row.Cells.Add(usage[phone.PhoneNumber]["InboundAllTime"].ToString());//Rcvd All Time
		//					row.Cells.Add(usage[phone.PhoneNumber]["InboundLastMonth"].ToString());//Rcvd Last Month
		//					row.Cells.Add(usage[phone.PhoneNumber]["InboundThisMonth"].ToString());//Rcvd This Month
		//					row.Cells.Add(usage[phone.PhoneNumber]["InboundThisMonthCost"].ToString("c",new CultureInfo("en-US")));//Charge This Month
		//				}
		//				else {
		//					row.Cells.Add("0");//Sent All Time
		//					row.Cells.Add("0");//Sent Last Month
		//					row.Cells.Add("0");//Sent This Month
		//					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//					row.Cells.Add("0");//Rcvd All Time
		//					row.Cells.Add("0");//Rcvd Last Month
		//					row.Cells.Add("0");//Rcvd This Month
		//					row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//				}
		//				firstRow=false;
		//				gridSmsSummary.Rows.Add(row);
		//			}
		//			row=new ODGridRow();
		//			row.ColorBackG=Color.LightGray;
		//			row.Cells.Add("");
		//			row.Cells.Add("");
		//			row.Cells.Add("");
		//			row.Cells.Add("Totals Per Clinic");
		//			row.Cells.Add("0");//Sent All Time
		//			row.Cells.Add("0");//Sent Last Month
		//			row.Cells.Add("0");//Sent This Month
		//			row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//			row.Cells.Add("0");//Rcvd All Time
		//			row.Cells.Add("0");//Rcvd Last Month
		//			row.Cells.Add("0");//Rcvd This Month
		//			row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//			gridSmsSummary.Rows.Add(row);
		//		}
		//		row=new ODGridRow();
		//		row.ColorBackG=Color.Gray;
		//		row.Cells.Add("");
		//		row.Cells.Add("");
		//		row.Cells.Add("");
		//		row.Cells.Add("Totals For All");
		//		row.Cells.Add("0");//Sent All Time
		//		row.Cells.Add("0");//Sent Last Month
		//		row.Cells.Add("0");//Sent This Month
		//		row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//		row.Cells.Add("0");//Rcvd All Time
		//		row.Cells.Add("0");//Rcvd Last Month
		//		row.Cells.Add("0");//Rcvd This Month
		//		row.Cells.Add((0f).ToString("c",new CultureInfo("en-US")));//Charge this month
		//		gridSmsSummary.Rows.Add(row);
		//	}
		//	gridSmsSummary.EndUpdate();
		//}


		private void textSmsLimit_TextChanged(object sender,EventArgs e) {
			SetSmsServiceAgreement(true);
		}

		private void textSmsLimit_Leave(object sender,EventArgs e) {
			//Attempt to clean up the input.
			textSmsLimit.Text=PIn.Double(textSmsLimit.Text.Trim('$')).ToString("c",new CultureInfo("en-US"));
			SetSmsServiceAgreement();
		}

		private void butSmsSubmit_Click(object sender,EventArgs e) {
			if(!checkSmsAgree.Checked) {
				MsgBox.Show(this,"You must agree to the service agreement.");
			}
			double amount=PIn.Double(textSmsLimit.Text.Trim('$'));
			if(amount<=0) {
				MsgBox.Show(this,"Please enter valid amount.");
				return;
			}
			if(Programs.IsEnabled(ProgramName.CallFire)) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Call Fire is currently enabled for texting.\r\nPress OK to switch to Integrated Texting, press Cancel to retain Call Fire")) {
					return;
				}
				Program callfire=Programs.GetCur(ProgramName.CallFire);
				if(callfire!=null) {
					callfire.Enabled=false;
					Programs.Update(callfire);
				}
			}
			List<SmsPhone> listClinicPhones=new List<SmsPhone>();
			try {
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					listClinicPhones=SmsPhones.SignContract(0,amount);
				}
				else {
					listClinicPhones=SmsPhones.SignContract(_clinicCur.ClinicNum,amount);
				}
			}
			catch (Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			if(listClinicPhones==null || listClinicPhones.Count==0) {
				MsgBox.Show(this,"Unable to initialize account.");
				return;
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				Prefs.UpdateDateT(PrefName.SmsContractDate,DateTime.Now);
				Prefs.UpdateDouble(PrefName.SmsMonthlyLimit,amount);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			else {
				_clinicCur.SmsMonthlyLimit=amount;
				_clinicCur.SmsContractDate=DateTime.Now;
				Clinics.Update(_clinicCur);
				DataValid.SetInvalid(InvalidType.Providers);//includes clinics.
			}
			FillGridClinics();
			FillGridSmsUsage();
			textSmsLimit.Text="";//set blank so that when we call SetSmsServiceAgreement it will populate with new value.
			SetSmsServiceAgreement();
			DataValid.SetInvalid(InvalidType.SmsTextMsgReceivedUnreadCount);
		}

		private void butSmsCancel_Click(object sender,EventArgs e) {
			textSmsLimit.Text="";//clear user input
			SetSmsServiceAgreement();//sets to previous value if applicable.
		}

		private void gridClinics_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			comboClinicSms.SelectedIndex=e.Row;
			FillGridSmsUsage();
			SetSmsServiceAgreement();
		}

		private void butSmsUnsubscribe_Click(object sender,EventArgs e) {
			try {
				long ClinicNum=0;
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					ClinicNum=_clinicCur.ClinicNum;
				}
				if(!SmsPhones.UnSignContract(ClinicNum)) {
					MsgBox.Show(this,"Unable to unsign contract.");
					return;
				}
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					Prefs.UpdateDateT(PrefName.SmsContractDate,DateTime.MinValue);
					textSmsLimit.Text="";
					Prefs.UpdateDouble(PrefName.SmsMonthlyLimit,0);
					DataValid.SetInvalid(InvalidType.Prefs);
					Prefs.RefreshCache();
				}
				else {
					_listClinics[comboClinicSms.SelectedIndex].SmsMonthlyLimit=0;
					textSmsLimit.Text="";
					_listClinics[comboClinicSms.SelectedIndex].SmsContractDate=DateTime.MinValue;
					Clinics.Update(_listClinics[comboClinicSms.SelectedIndex]);
					DataValid.SetInvalid(InvalidType.Providers);
					Clinics.RefreshCache();
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FillGridClinics();
			FillGridSmsUsage();
			SetSmsServiceAgreement();
			DataValid.SetInvalid(InvalidType.SmsTextMsgReceivedUnreadCount);
		}

		private void butBackMonth_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=dateTimePickerSms.Value.AddMonths(-1);
		}

		private void butFwdMonth_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=dateTimePickerSms.Value.AddMonths(1);//triggers refresh
		}

		private void butToday_Click(object sender,EventArgs e) {
			dateTimePickerSms.Value=DateTime.Now.Date;//triggers refresh
		}

		private void dateTimePickerSms_ValueChanged(object sender,EventArgs e) {
			FillGridSmsUsage();
		}


		#endregion

		private void tabControl_SelectedIndexChanged(object sender,EventArgs e) {
			//jsalmon - The following method call was causing the "not authorized for ..." message to continuously pop up and was very annoying.
			//SetControlEnabledState();
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
			SmsService
		}
	}
}