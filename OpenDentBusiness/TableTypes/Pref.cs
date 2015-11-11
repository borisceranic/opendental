using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Stores small bits of data for a wide variety of purposes.  Any data that's too small to warrant its own table will usually end up here.</summary>
	[Serializable]
	[CrudTable(TableName="preference")]
	public class Pref:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long PrefNum;
		///<summary>The text 'key' in the key/value pairing.</summary>
		public string PrefName;
		///<summary>The stored value.</summary>
		public string ValueString;
		///<summary>Documentation on usage and values of each pref.  Mostly deprecated now in favor of using XML comments in the code.</summary>
		public string Comments;

		///<summary>Returns a copy of the pref.</summary>
		public Pref Copy() {
			return (Pref)this.MemberwiseClone();
		}
	}

	///<summary>Because this enum is stored in the database as strings rather than as numbers, we can do the order alphabetically.  This enum must exactly match the prefs in the database.  Deprecated preferences will start with "Deprecated" in the summary section.</summary>
	public enum PrefName {
		AccountingCashIncomeAccount,
		AccountingDepositAccounts,
		AccountingIncomeAccount,
		AccountingLockDate,
		///<summary>Enum:AccountingSoftware 0=None, 1=Open Dental, 2=QuickBooks</summary>
		AccountingSoftware,
		AccountShowPaymentNums,
		///<summary>Show questionnaire button in account module toolbar.  Set in FormShowFeatures.</summary>
		AccountShowQuestionnaire,
		///<summary>Show TrojanCollect button in account module toolbar.  Set in FormShowFeatures.</summary>
		AccountShowTrojanExpressCollect,
		ADAComplianceDateTime,
		ADAdescriptionsReset,
		///<summary>Enum:ADPCompanyCode Used to generate the export file from FormTimeCardManage. Set in FormTimeCardSetup.</summary>
		ADPCompanyCode,
		AgingCalculatedMonthlyInsteadOfDaily,
		///<summary>FK to allergydef.AllergyDefNum</summary>
		AllergiesIndicateNone,
		AllowedFeeSchedsAutomate,
		AllowSettingProcsComplete,
		AppointmentBubblesDisabled,
		AppointmentBubblesNoteLength,
		///<summary>Enum:SearchBehaviorCriteria 0=ProviderTime, 1=ProviderTimeOperatory</summary>
		AppointmentSearchBehavior,
		AppointmentTimeArrivedTrigger,
		AppointmentTimeDismissedTrigger,
		///<summary>The number of minutes that the appointment schedule is broken up into.  E.g. "10" represents 10 minute increments.</summary>
		AppointmentTimeIncrement,
		///<summary>Set to true if appointment times are locked by default.</summary>
		AppointmentTimeIsLocked,
		AppointmentTimeSeatedTrigger,
		ApptBubbleDelay,
		ApptExclamationShowForUnsentIns,
		/// <summary>Boolean defaults to 0, when true appt module will default to week view</summary>
		ApptModuleDefaultToWeek,
		///<summary>Keeps the waiting room indicator times current.  Initially 1.</summary>
		ApptModuleRefreshesEveryMinute,
		///<summary>Integer</summary>
		ApptPrintColumnsPerPage,
		///<summary>Float</summary>
		ApptPrintFontSize,
		///<summary>Stored as DateTime.  Currently the date portion is not used but might be used in future versions.</summary>
		ApptPrintTimeStart,
		///<summary>Stored as DateTime.  Currently the date portion is not used but might be used in future versions.</summary>
		ApptPrintTimeStop,
		ApptReminderDayInterval,
		ApptReminderDayMessage,
		ApptReminderHourInterval,
		ApptReminderHourMessage,
		///<summary>Boolean defaults to 0.</summary>
		ApptReminderSendAll,
		///<summary>Comma-delimited string.</summary>
		ApptReminderSendOrder,
		///<summary>Used by OD HQ.  Not added to db convert script.  Used to store the IP address of the asterisk phone server for the phone comms and voice mails.</summary>
		AsteriskServerIp,
		///<summary>Deprecated, but must remain here to avoid breaking updates.</summary>
		AtoZfolderNotRequired,
		///<summary>Normally 1.  If this is set to 0, then that means images are being stored in the database.  This used to be called AtoZfolderNotRequired, but that name was confusing.</summary>
		AtoZfolderUsed,
		///<summary>Used to determine the runtime of the threads that do automatic communication in the listener.  Stored as a DateTime.</summary>
		AutomaticCommunicationTimeStart,
		///<summary>Used to determine the runtime of the threads that do automatic communication in the listener.  Stored as a DateTime.</summary>
		AutomaticCommunicationTimeEnd,
		///<summary>Boolean.  Defaults to same value as ShowFeatureEhr.  Used to determine whether automatic summary of care webmails are sent.</summary>
		AutomaticSummaryOfCareWebmail,
		AutoResetTPEntryStatus,
		BackupExcludeImageFolder,
		BackupFromPath,
		BackupReminderLastDateRun,
		BackupRestoreAtoZToPath,
		BackupRestoreFromPath,
		BackupRestoreToPath,
		BackupToPath,
		BalancesDontSubtractIns,
		BankAddress,
		BankRouting,
		BillingAgeOfAccount,
		BillingChargeAdjustmentType,
		BillingChargeAmount,
		BillingChargeLastRun,
		///<summary>Value is a string, either Billing or Finance.</summary>
		BillingChargeOrFinanceIsDefault,
		BillingDefaultsInvoiceNote,
		BillingDefaultsIntermingle,
		BillingDefaultsLastDays,
		BillingDefaultsNote,
		///<summary>Value is an integer, identifying the max number of electronic statements that can be sent per batch.  Default of 0, which indicates no limit.</summary>
		BillingElectBatchMax,
		///<summary>Deprecated.  Use ebill.ClientAcctNumber instead.</summary>
		BillingElectClientAcctNumber,
		BillingElectCreditCardChoices,
		///<summary>Deprecated.  Use ebill.ElectPassword instead.</summary>
		BillingElectPassword,
		///<summary>No UI, can only be manually enabled by a programmer.  Only used for debugging electronic statements, because it will bloat the OpenDentImages folder.  Originally created to help with the "missing brackets bug" for EHG billing.</summary>
		BillingElectSaveHistory,
		///<summary>Deprecated.  Use ebill.ElectUserName instead.</summary>
		BillingElectUserName,
		BillingElectVendorId,
		BillingElectVendorPMSCode,
		BillingEmailBodyText,
		BillingEmailSubject,
		BillingExcludeBadAddresses,
		BillingExcludeIfUnsentProcs,
		BillingExcludeInactive,
		BillingExcludeInsPending,
		BillingExcludeLessThan,
		BillingExcludeNegative,
		BillingIgnoreInPerson,
		BillingIncludeChanged,
		///<summary>Used with repeat charges to apply repeat charges to patient accounts on billing cycle date.</summary>
		BillingUseBillingCycleDay,
		BillingSelectBillingTypes,
		/// <summary>0=no,1=EHG,2=POS(xml file),3=ClaimX(xml file)</summary>
		BillingUseElectronic,
		BirthdayPostcardMsg,
		///<summary>FK to definition.DefNum.  The adjustment type that will be used on the adjustment that is automatically created when an appointment is broken.</summary>
		BrokenAppointmentAdjustmentType,
		///<summary>Boolean.  0 by default.  When true, makes a commlog, otherwise makes an adjustment.</summary>
		BrokenApptCommLogNotAdjustment,
		///<summary>Boolean.  0 by default.  When true, makes a commlog in addition to an ADA D9986 procedure.</summary>
		BrokenApptCommLogWithProcedure,
		///<summary>Boolean.  0 by default.  When true, makes an adjustment in addition to an ADA D9986 procedure.</summary>
		BrokenApptAdjustmentWithProcedure,
		///<summary>For Ontario Dental Association fee schedules.</summary>
		CanadaODAMemberNumber,
		///<summary>For Ontario Dental Association fee schedules.</summary>
		CanadaODAMemberPass,
		///<summary>Boolean.  0 by default.  If enabled, only CEMT can edit certain security settings.  Currently only used for global lock date.</summary>
		CentralManagerSecurityLock,
		///<summary>This is the hash of the password that is needed to open the Central Manager tool.</summary>
		CentralManagerPassHash,
		///<summary>Deprecated.</summary>
		ChartQuickAddHideAmalgam,
		///<summary>Deprecated. If set to true (1), then after adding a proc, a row will be added to datatable instead of rebuilding entire datatable by making queries to the database.
		///This preference was never fully implemented and should not be used.  We may revisit some day.</summary>
		ChartAddProcNoRefreshGrid,
		///<summary>Preference to warn users when they have a nonpatient selected.</summary>
		ChartNonPatientWarn,
		ClaimAttachExportPath,
		ClaimFormTreatDentSaysSigOnFile,
		ClaimMedTypeIsInstWhenInsPlanIsMedical,
		///<summary>Blank by default.  Computer name to receive reports from automatically.</summary>
		ClaimReportComputerName,
		///<summary>Report receive interval.  5 by default.</summary>
		ClaimReportReceiveInterval,
		///<summary>Boolean.  0 by default.  If enabled, the Send Claims window will automatically validate e-claims upon loading the window.
		///Validating all claims on load was old behavior that was significantly slowing down the loading of the send claims window.
		///Several offices complained that we took away the validation until they attempt sending the claim.</summary>
		ClaimsSendWindowValidatesOnLoad,
		///<summary>Boolean.  0 by default.  If enabled, snapshots of claimprocs are created when claims are created.  There is currently no UI for this preference.</summary>
		ClaimSnapshotEnabled,
		ClaimsValidateACN,
		ClearinghouseDefaultDent,
		ClearinghouseDefaultMed,
		ColorTheme,
		ConfirmEmailMessage,
		ConfirmEmailSubject,
		ConfirmPostcardMessage,
		///<summary>FK to definition.DefNum.  Initially 0.</summary>
		ConfirmStatusEmailed,
		///<summary>FK to definition.DefNum.</summary>
		ConfirmStatusTextMessaged,
		///<summary>The message that goes out to patients when doing a batch confirmation.</summary>
		ConfirmTextMessage,
		///<summary>Selected connection group within the CEMT.</summary>
		ConnGroupCEMT,
		CoPay_FeeSchedule_BlankLikeZero,
		///<summary>Boolean.  Typically set to true when an update is in progress and will be set to false when finished.  Otherwise true means that the database is in a corrupt state.</summary>
		CorruptedDatabase,
		///<summary>This is the default encounter code used for automatically generating encounters when specific actions are performed in Open Dental.  The code is displayed/set in FormEhrSettings.  We will set it and give the user a list of 9 suggested codes to use such that the encounters generated will cause the pateint to be considered part of the initial patient population in the 9 clinical quality measures tracked by OD.  CQMDefaultEncounterCodeSystem will identify the code system this code is from and the code value will be a FK to that code system.</summary>
		CQMDefaultEncounterCodeValue,
		CQMDefaultEncounterCodeSystem,
		CropDelta,
		///<summary>Used by OD HQ.  Not added to db convert script.  Allowable timeout for Negotiator to establish a connection with Listener. Different than SocketTimeoutMS and TransmissionTimeoutMS.  Specifies the allowable timeout for Patient Portal Negotiator to establish a connection with Listener.  Negotiator will only wait this long to get an acknowledgement that the Listener is available for a transmission before timing out.  Initially 10000</summary>
		CustListenerConnectionRequestTimeoutMS,		
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalCustListener when service initialized.  Specifies the time (in minutes) between each time that the listener service will upload it's current heartbeat to HQ.  Initially 360</summary>
		CustListenerHeartbeatFrequencyMinutes,
		///<summary>Used by OpenDentalCustListener.  String specifies which port the OpenDentalWebService should look for on the customer's server in order to create a socket connection.  Initially 25255</summary>
		CustListenerPort,
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalCustListener when service initialized.  Specifies the read/write socket timeout.  Initially 3000</summary>
		CustListenerSocketTimeoutMS,
		///<summary>Used by OD HQ.  Not added to db convert script.  Specifies the entire wait time alloted for a transmission initiated by the patient portal.  Negotiator will only wait this long for a valid response back from Listener before timing out.  Initially 30000</summary>		
		CustListenerTransmissionTimeoutMS,
		CustomizedForPracticeWeb,
		DatabaseConvertedForMySql41,
		DataBaseVersion,
		DateDepositsStarted,
		DateLastAging,
		DefaultCCProcs,
		DefaultClaimForm,
		DefaultProcedurePlaceService,
		///<summary>Boolean.  Set to 1 to indicate that this database holds customers instead of patients.  Used by OD HQ.  Used for showing extra phone numbers, showing some extra buttons for tools that only we use, behavior of checkboxes in repeating charge window, etc.  But phone panel visibility is based on DockPhonePanelShow.</summary>
		DistributorKey,
		DockPhonePanelShow,
		///<summary>The AtoZ folder path.</summary>
		DocPath,
		///<summary>The ICD Diagnosis Code version primarily used by the practice.  Value of '9' for ICD-9, and '10' for ICD-10.</summary>
		DxIcdVersion,
		EasyBasicModules,
		/// <summary>Depricated.</summary>
		EasyHideAdvancedIns,
		EasyHideCapitation,
		EasyHideClinical,
		EasyHideDentalSchools,
		EasyHideHospitals,
		EasyHideInsurance,
		EasyHideMedicaid,
		EasyHidePrinters,
		EasyHidePublicHealth,
		EasyHideRepeatCharges,
		EasyNoClinics,
		EclaimsSeparateTreatProv,
		EHREmailFromAddress,
		EHREmailPassword,
		EHREmailPOPserver,
		EHREmailPort,
		EhrRxAlertHighSeverity,
		///<summary>This pref is hidden, so no practical way for user to turn this on.  Only used for ehr testing.</summary>
		EHREmailToAddress,
		///<summary>Date when user upgraded to 13.1.14 and started using NewCrop Guids on Rxs.</summary>
		ElectronicRxDateStartedUsing131,
		/// <summary>FK to EmailAddress.EmailAddressNum.  It is not required that a default be set.</summary>
		EmailDefaultAddressNum,
		///<summary>The name of the only computer allowed to get new email messages from an email inbox (including Direct messages).</summary>
		EmailInboxComputerName,
		///<summary>Time interval in minutes describing how often to automatically check the email inbox for new messages. Default is 5 minutes.</summary>
		EmailInboxCheckInterval,
		///<summary>FK to EmailAddress.EmailAddressNum.  Used for webmail notifications (Patient Portal).</summary>
		EmailNotifyAddressNum,
		/// <summary>Deprecated. Use emailaddress.EmailPassword instead.</summary>
		EmailPassword,
		/// <summary>Deprecated. Use emailaddress.ServerPort instead.</summary>
		EmailPort,
		/// <summary>Deprecated. Use emailaddress.SenderAddress instead.</summary>
		EmailSenderAddress,
		/// <summary>Deprecated. Use emailaddress.SMTPserver instead.</summary>
		EmailSMTPserver,
		/// <summary>Deprecated. Use emailaddress.EmailUsername instead.</summary>
		EmailUsername,
		/// <summary>Deprecated. Use emailaddress.UseSSL instead.</summary>
		EmailUseSSL,
		/// <summary>Boolean. 0 means false and means it is not an EHR Emergency, and emergency access to the family module is not granted.</summary>
		EhrEmergencyNow,
		///<summary>There is no UI for this.  It's only used by OD HQ.</summary>
		EhrProvKeyGeneratorPath,
		EnableAnesthMod,
		///<summary>Warns the user if the Medicaid ID is not the proper number of digits for that state.</summary>
		EnforceMedicaidIDLength,
		ExportPath,
		///<summary>Allows guarantor access to all family health information in the patient portal.  Default is 1.</summary>
		FamPhiAccess,
		FinanceChargeAdjustmentType,
		FinanceChargeAPR,
		FinanceChargeAtLeast,
		FinanceChargeLastRun,
		FinanceChargeOnlyIfOver,
		FuchsListSelectionColor,
		FuchsOptionsOn,
		GenericEClaimsForm,
		///<summary>Has no UI.  Used to validate help support.  See the OpenDentalHelp project for more information on HelpKey.</summary>
		HelpKey,
		HL7FolderOut,
		HL7FolderIn,
		///<summary>Used by HQ. Projected onto wall displayed on top of FormMapHQ</summary>
		HQTriageCoordinator,
		///<summary>procedurelog.DiagnosticCode will be set to this for new procedures and complete procedures if this field was blank when set complete.
		///This can be an ICD-9 or an ICD-10.  In future versions, could be another an ICD-11, ICD-12, etc.</summary>
		ICD9DefaultForNewProcs,
		ImagesModuleTreeIsCollapsed,
		ImageWindowingMax,
		ImageWindowingMin,
		///<summary>Boolean.  False by default.  When enabled a fix is enabled within ODTextBox (RichTextBox) for foreign users that use 
		///a different language input methodology that requires the composition of symbols in order to display their language correctly.
		///E.g. the Korean symbol '역' (dur) will not display correctly inside ODTextBoxes without this set to true.</summary>
		ImeCompositionCompatibility,
		///<summary>0=Default practice provider, -1=Treating Provider. Otherwise, FK to provider.ProvNum.</summary>
		InsBillingProv,
		InsDefaultCobRule,
		InsDefaultPPOpercent,
		InsDefaultShowUCRonClaims,
		InsDefaultAssignBen,
		///<summary>0=unknown, user did not make a selection.  1=Yes, 2=No.</summary>
		InsPlanConverstion_7_5_17_AutoMergeYN,
		///<summary>0 by default.  If false, secondary PPO writeoffs will always be zero (normal).  At least one customer wants to see secondary writeoffs.</summary>
		InsPPOsecWriteoffs,
		InsurancePlansShared,
		///<summary>Writeoff description displayed in the Account Module and on statements.  If blank, the default is "Writeoff".
		///We are using "Writeoff" since "PPO Discount" was only used for a brief time in 15.3 while it was Beta and no customer requested it</summary>
		InsWriteoffDescript,
		IntermingleFamilyDefault,
		LabelPatientDefaultSheetDefNum,
		///<summary>Used to determine how many windows are displayed throughout the program, translation, charting, and other features. Version 15.4.1</summary>
		LanguageAndRegion,
		///<summary>Initially set to Declined to Specify.  Indicates which language from the LanguagesUsedByPatients preference is the language that indicates the patient declined to specify.  Text must exactly match a language in the list of available languages.  Can be blank if the user deletes the language from the list of available languages.</summary>
		LanguagesIndicateNone,
		///<summary>Comma-delimited list of two-letter language names and custom language names.  The custom language names are the full string name and are not necessarily supported by Microsoft.</summary>
		LanguagesUsedByPatients,
		LetterMergePath,
		MainWindowTitle,
		///<summary>0=Meaningful Use Stage 1, 1=Meaningful Use Stage 2.  Global, affects all providers.  Changes the MU grid that is seen for individual patients and for summary reports.</summary>
		MeaningfulUseTwo,
		///<summary>Number of days after medication order start date until stop date.  Used when automatically inserting a medication order when creating
		///a new Rx.  Default value is 7 days.  If set to 0 days, the automatic stop date will not be entered.</summary>
		MedDefaultStopDays,
		///<summary>New procs will use the fee amount tied to the medical code instead of the ADA code.</summary>
		MedicalFeeUsedForNewProcs,
		///<summary>FK to medication.MedicationNum</summary>
		MedicationsIndicateNone,
		///<summary>If MedLabReconcileDone=="0", a one time reconciliation of the MedLab HL7 messages is needed. The reconcile will reprocess the original
		///HL7 messages for any MedLabs with PatNum=0 in order to create the embedded PDF files from the base64 text in the ZEF segments. The old method
		///of waiting to extract these files until the message is manually attached to a patient was very slow using the middle tier. The new method is to
		///create the PDF files and save them in the image folder in a subdirectory called "MedLabEmbeddedFiles" if a pat is not located from the details
		///in the PID segment of the message. Attaching the MedLabs to a patient is now just a matter of moving the files to the patient's image folder.
		///All files will now be extracted and stored, either in a pat's folder or in the "MedLabEmbeddedFiles" folder, by the HL7 service.</summary>
		MedLabReconcileDone,
		MobileSyncDateTimeLastRun,
		///<summary>Used one time after the conversion to 7.9 for initial synch of the provider table.</summary>
		MobileSynchNewTables79Done,
		///<summary>Used one time after the conversion to 11.2 for re-synch of the patient records because a)2 columns BalTotal and InsEst have been added to the patientm table. b) the table documentm has been added</summary>
		MobileSynchNewTables112Done,
		///<summary>Used one time after the conversion to 12.1 for the recallm table being added and for upload of the practice Title.</summary>
		MobileSynchNewTables121Done,
		MobileSyncIntervalMinutes,
		MobileSyncServerURL,
		MobileSyncWorkstationName,
		MobileExcludeApptsBeforeDate,
		MobileUserName,
		//MobileSyncLastFileNumber,
		//MobileSyncPath,
		///<summary>The major and minor version of the current MySQL connection.  Gets updated on startup when a new version is detected.</summary>
		MySqlVersion,
		///<summary>There is no UI for user to change this.  Format, if OD customer, is PatNum-(RandomString)(CheckSum).  Example: 1234-W6c43.  Format for resellers is up to them.</summary>
		NewCropAccountId,
		///<summary>The date this customer last checked with HQ to determine which provider have access to eRx.</summary>
		NewCropDateLastAccessCheck,
		///<summary>True for customers who were using NewCrop before version 15.4.  True if NewCropAccountId was not blank when upgraded.</summary>
		NewCropIsLegacy,
		///<summary>Controls which NewCrop database to use.  If false, then the customer uses the First Data Bank (FDB) database, otherwise the 
		///customer uses the LexiData database.  Connecting to LexiData saves NewCrop some money on the new accounts.  Additionally, the RxNorms which
		///come back from the prescription refresh in the Chart are more complete for the LexiData database than for the FDB database.</summary>
		NewCropIsLexiData,
		///<summary>There is no UI for user to change this. For distributors, this is part of the credentials.  OD credentials are not stored here, but are hard-coded.</summary>
		NewCropName,
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		NewCropPartnerName,
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		NewCropPassword,
		///<summary>URL of the time server to use for EHR time synchronization.  Only used for EHR.  Example nist-time-server.eoni.com</summary>
		NistTimeServerUrl,
		OpenDentalVendor,
		OracleInsertId,
		PasswordsMustBeStrong,
		PatientFormsShowConsent,
		///<summary>Free-form 'Body' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		PatientPortalNotifyBody,
		///<summary>Free-form 'Subject' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		PatientPortalNotifySubject,
		PatientPortalURL,
		PatientSelectUseFNameForPreferred,
		///<summary>Boolean. This is the default for new computers, otherwise it uses the computerpref PatSelectSearchMode.</summary>
		PatientSelectUsesSearchButton,
		///<summary>Boolean. True by default.  If false, the Payment window follows old "Split to family" behavior.</summary>
		PaymentsPromptForAutoSplit,
		PayPlansBillInAdvanceDays,
		PerioColorCAL,
		PerioColorFurcations,
		PerioColorFurcationsRed,
		PerioColorGM,
		PerioColorMGJ,
		PerioColorProbing,
		PerioColorProbingRed,
		PerioRedCAL,
		PerioRedFurc,
		PerioRedGing,
		PerioRedMGJ,
		PerioRedMob,
		PerioRedProb,
		PlannedApptTreatedAsRegularAppt,
		PracticeAddress,
		PracticeAddress2,
		PracticeBankNumber,
		PracticeBillingAddress,
		PracticeBillingAddress2,
		PracticeBillingCity,
		PracticeBillingST,
		PracticeBillingZip,
		PracticeCity,
		PracticeDefaultBillType,
		PracticeDefaultProv,
		///<summary>In USA and Canada, enforced to be exactly 10 digits or blank.</summary>
		PracticeFax,
		///<summary>This preference is used to hide/change certain OD features, like hiding the tooth chart and changing 'dentist' to 'provider'.</summary>
		PracticeIsMedicalOnly,
		PracticePayToAddress,
		PracticePayToAddress2,
		PracticePayToCity,
		PracticePayToST,
		PracticePayToZip,
		///<summary>In USA and Canada, enforced to be exactly 10 digits or blank.</summary>
		PracticePhone,
		PracticeST,
		PracticeTitle,
		PracticeZip,
		///<summary>This is the default pregnancy code used for diagnosing pregnancy from FormVitalSignEdit2014 and is displayed/set in FormEhrSettings.  When the check box for BMI and BP not taken due to pregnancy Dx is selected, this code value will be inserted into the diseasedef table in the column identified by the PregnancyDefaultCodeSystem (i.e. diseasedef.SnomedCode, diseasedef.ICD9Code).  It will then be a FK in the diseasedef table to the associated code system table.</summary>
		PregnancyDefaultCodeValue,
		PregnancyDefaultCodeSystem,
		///<summary>In Patient Edit and Add Family windows, the Primary Provider defaults to 'Select Provider' instead of the practice provider.</summary>
		PriProvDefaultToSelectProv,
		///<summary>FK to diseasedef.DiseaseDefNum</summary>
		ProblemsIndicateNone,
		ProblemListIsAlpabetical,
		///<summary>In FormProcCodes, this is the default for the ShowHidden checkbox.</summary>
		ProcCodeListShowHidden,
		ProcLockingIsAllowed,
		///<summary>Frequency at which signals are processed. Also used by HQ to determine triage label refresh frequency.</summary>		
		ProcessSigsIntervalInSecs,
		ProcGroupNoteDoesAggregate,
		///<summary>Stores the DateTime of when the ProgramVersion preference last changed.</summary>
		ProgramVersionLastUpdated,
		ProgramVersion,
		ProviderIncomeTransferShows,
		///<summary>Was never used.  Was supposed to indicate FK to sheet.Sheet_DEF_Num, so not even named correctly. Must be an exam sheet. Only makes sense if PublicHealthScreeningUsePat is true.</summary>
		PublicHealthScreeningSheet,
		///<summary>Was never used.  Always 0.  Boolean. Work for attaching to patients stopped 11/30/2012, there is currently no access to change the value of this preference.    When in this mode, screenings will be attached to actual PatNums rather than just freeform text names.</summary>
		PublicHealthScreeningUsePat,
		QuickBooksCompanyFile,
		QuickBooksDepositAccounts,
		QuickBooksIncomeAccount,
		///<summary>Date when user upgraded to or past 15.4.1 and started using ADA procedures to count CPOE radiology orders for EHR.</summary>
		RadiologyDateStartedUsing154,
		RandomPrimaryKeys,
		RecallAdjustDown,
		RecallAdjustRight,
		///<summary>Defaults to 12 for new customers.  The number in this field is considered adult.  Only used when automatically adding procedures to a new recall appointment.</summary>
		RecallAgeAdult,
		RecallCardsShowReturnAdd,
		///<summary>-1 indicates min for all dates</summary>
		RecallDaysFuture,
		///<summary>-1 indicates min for all dates</summary>
		RecallDaysPast,
		RecallEmailFamMsg,
		RecallEmailFamMsg2,
		RecallEmailFamMsg3,
		RecallEmailMessage,
		RecallEmailMessage2,
		RecallEmailMessage3,
		RecallEmailSubject,
		RecallEmailSubject2,
		RecallEmailSubject3,
		RecallExcludeIfAnyFutureAppt,
		RecallGroupByFamily,
		RecallMaxNumberReminders,
		RecallPostcardFamMsg,
		RecallPostcardFamMsg2,
		RecallPostcardFamMsg3,
		RecallPostcardMessage,
		RecallPostcardMessage2,
		RecallPostcardMessage3,
		RecallPostcardsPerSheet,
		RecallShowIfDaysFirstReminder,
		RecallShowIfDaysSecondReminder,
		RecallStatusEmailed,
		RecallStatusMailed,
		///<summary>Used if younger than 12 on the recall date.</summary>
		RecallTypeSpecialChildProphy,
		RecallTypeSpecialPerio,
		RecallTypeSpecialProphy,
		///<summary>Comma-delimited list. FK to recalltype.RecallTypeNum.</summary>
		RecallTypesShowingInList,
		///<summary>If false, then it will only use email in the recall list if email is the preferred recall method.</summary>
		RecallUseEmailIfHasEmailAddress,
		RegistrationKey,
		RegistrationKeyIsDisabled,
		RegistrationNumberClaim,
		RenaissanceLastBatchNumber,
		///<summary>If replication has failed, this indicates the server_id.  No computer will be able to connect to this single server until this flag is cleared.</summary>
		ReplicationFailureAtServer_id,
		///<summary>The PK of the replication server that is flagged as the "report server".  If using replication, "create table" or "drop table" commands can only be executed within the user query window when connected to this server.</summary>
		ReplicationUserQueryServer,
		ReportFolderName,
		ReportPandIhasClinicBreakdown,
		ReportPandIschedProdSubtractsWO,
		ReportsPPOwriteoffDefaultToProcDate,
		ReportsShowPatNum,
		RequiredFieldColor,
		RxSendNewToQueue,
		SalesTaxPercentage,
		ScannerCompression,
		ScannerResolution,
		ScannerSuppressDialog,
		ScheduleProvUnassigned,
		///<summary>UserGroupNum for Instructors.  Set only for dental schools in dental school setup.</summary>
		SecurityGroupForInstructors,
		///<summary>UserGroupNum for Students.  Set only for dental schools in dental school setup.</summary>
		SecurityGroupForStudents,
		SecurityLockDate,
		///<summary>Set to 0 to always grant permission. 1 means only today.</summary>
		SecurityLockDays,
		SecurityLockIncludesAdmin,
		///<summary>Set to 0 to disable auto logoff.</summary>
		SecurityLogOffAfterMinutes,
		SecurityLogOffWithWindows,
		ShowAccountFamilyCommEntries,
		ShowFeatureEhr,
		///<summary>Set to 1 by default.  Shows a button in Edit Patient Information that lets users launch Google Maps.</summary>
		ShowFeatureGoogleMaps,
		ShowFeatureMedicalInsurance,
		///<summary>Set to 1 to enable the Synch Clone button in the Family module which allows users to create and synch clones of patients.</summary>
		ShowFeaturePatientClone,
		ShowFeatureSuperfamilies,
		///<summary>0=None, 1=PatNum, 2=ChartNumber, 3=Birthdate</summary>
		ShowIDinTitleBar,
		ShowProgressNotesInsteadofCommLog,
		ShowUrgFinNoteInProgressNotes,
		///<summary>Used to stop signals after a period of inactivity.  A value of 0 disables this feature.  Default value of 0 to maintain backward compatibility</summary>
		SignalInactiveMinutes,
		///<summary>Only used on startup.  The date in which stale signalods were removed.</summary>
		SignalLastClearedDate,
		///<summary>Blank if not signed. Date signed. For practice level contract, if using clinics see Clinic.SmsContractDate. Record of signing also kept at HQ.</summary>
		SmsContractDate,
		///<summary>(Deprecated) Blank if not signed. Name signed. For practice level contract, if using clinics see Clinic.SmsContractName. Record of signing also kept at HQ.</summary>
		SmsContractName,
		///<summary>Always stored in US dollars. This is the desired limit for SMS outbound messages per month.</summary>
		SmsMonthlyLimit,
		/// <summary>Name of this Software.  Defaults to 'Open Dental Software'.</summary>
		SoftwareName,
		SolidBlockouts,
		SpellCheckIsEnabled,
		StatementAccountsUseChartNumber,
		StatementsCalcDueDate,
		StatementShowCreditCard,
		///<summary>Show payment notes.</summary>
		StatementShowNotes,
		StatementShowAdjNotes,
		StatementShowProcBreakdown,
		StatementShowReturnAddress,
		///<summary>Deprecated.  Not used anywhere.</summary>
		StatementSummaryShowInsInfo,
		StatementsUseSheets,
		StoreCCnumbers,
		StoreCCtokens,
		SubscriberAllowChangeAlways,
		TaskAncestorsAllSetInVersion55,
		TaskListAlwaysShowsAtBottom,
		TasksCheckOnStartup,
		TasksNewTrackedByUser,
		TasksShowOpenTickets,
		///<summary>Keeps track of date of one-time cleanup of temp files.  Prevents continued annoying cleanups after the first month.</summary>
		TempFolderDateFirstCleaned,
		TerminalClosePassword,
		TextMsgOkStatusTreatAsNo,
		TimeCardADPExportIncludesName,
		///<summary>0=Sun,1=Mon...6=Sat</summary>
		TimeCardOvertimeFirstDayOfWeek,
		TimecardSecurityEnabled,
		///<summary>Boolean.  0 by default.  When enabled, FormTimeCard and FormTimeCardMange display H:mm:ss instead of HH:mm</summary>
		TimeCardShowSeconds,
		TimeCardsMakesAdjustmentsForOverBreaks,
		///<summary>bool</summary>
		TimeCardsUseDecimalInsteadOfColon,
		TimecardUsersDontEditOwnCard,
		TitleBarShowSite,
		///<summary>Deprecated.  Not used anywhere.</summary>
		ToothChartMoveMenuToRight,
		TreatmentPlanNote,
		TreatPlanDiscountAdjustmentType,
		///<summary>Set to 0 to clear out previous discounts.</summary>
		TreatPlanDiscountPercent,
		TreatPlanItemized,
		TreatPlanPriorityForDeclined,
		///<summary>When a TP is signed a PDF will be generated and saved. If disabled, TPs will be redrawn with current data (pre 15.4 behavior).</summary>
		TreatPlanSaveSignedToPdf,
		TreatPlanShowCompleted,
		TreatPlanShowGraphics,
		TreatPlanShowIns,
		TreatPlanUseSheets,
		TrojanExpressCollectBillingType,
		TrojanExpressCollectPassword,
		TrojanExpressCollectPath,
		TrojanExpressCollectPreviousFileNumber,
		UpdateCode,
		UpdateInProgressOnComputerName,
		///<summary>Described in the Update Setup window and in the manual.  Can contain multiple db names separated by commas.  Should not include current db name.</summary>
		UpdateMultipleDatabases,
		UpdateServerAddress,
		UpdateShowMsiButtons,
		UpdateWebProxyAddress,
		UpdateWebProxyPassword,
		UpdateWebProxyUserName,
		UpdateWebsitePath,
		UpdateWindowShowsClassicView,
		UseBillingAddressOnClaims,
		///<summary>Enum:ToothNumberingNomenclature 0=Universal(American), 1=FDI, 2=Haderup, 3=Palmer</summary>
		UseInternationalToothNumbers,
		///<summary>Boolean.  0 by default.  When enabled, users must enter their user name manually at the log on window.</summary>
		UserNameManualEntry,
		///<summary>Boolean. 0 by default. When enabled, chart module procedures that are complete will use the provider's color as row's background color</summary>
		UseProviderColorsInChart,
		WaitingRoomAlertColor,
		///<summary>0 to disable.  When enabled, sets rows to alert color based on wait time.</summary>
		WaitingRoomAlertTime,
		///<summary>Boolean.  0 by default.  When enabled, the waiting room will filter itself by the selected appointment view.  0, normal filtering, will show all patients waiting for the entire practice (or entire clinic when using clinics).</summary>
		WaitingRoomFilterByView,
		///<summary>Only used for sheet synch.  See Mobile... for URL for mobile synch.</summary>
		WebHostSynchServerURL,
		///<summary>Stored as an int value from the WebSchedAutomaticSend enum.</summary>
		WebSchedAutomaticSendSetting,
		WebSchedMessage,
		WebSchedMessage2,
		WebSchedMessage3,
		WebServiceHQServerURL,
		///<summary>Enum: WebSchedProviderRules 0=FirstAvailable, 1=PrimaryProvider, 2=SecondaryProvider, 3=LastSeenHygienist</summary>
		WebSchedProviderRule,
		///<summary>Boolean. 0 by default. True when Web Sched service is enabled.  Loosely keeps track of service status, calling our web service to verify active service is still required.  This preference is mainly used to quickly (without web call) make the UI of Open Dental different and less annoying (advertising wise) depeding on if the service is enabled or not.</summary>
		WebSchedService,
		WebSchedSubject,
		WebSchedSubject2,
		WebSchedSubject3,
		WebServiceServerName,
		WordProcessorPath,
		XRayExposureLevel
	}

	///<summary>Used by pref "AppointmentSearchBehavior". </summary>
	public enum SearchBehaviorCriteria {
		ProviderTime,
		ProviderTimeOperatory
	}

	///<summary>Used by pref "AccountingSoftware".  0=OpenDental, 1=QuickBooks</summary>
	public enum AccountingSoftware {
		OpenDental,
		QuickBooks
	}

	///<summary>Used by pref "WebSchedProviderRule". Determines how Web Sched will decide on what provider time slots to show patients.</summary>
	public enum WebSchedProviderRules {
		///<summary>0 - Dynamically picks the first available provider based on the time slot picked by the patient.</summary>
		FirstAvailable,
		///<summary>1 - Only shows time slots that are available via the patient's primary provider.</summary>
		PrimaryProvider,
		///<summary>2 - Only shows time slots that are available via the patient's secondary provider.</summary>
		SecondaryProvider,
		///<summary>3 - Only shows time slots that are available via the patient's last seen hygienist.</summary>
		LastSeenHygienist
	}
	



}
