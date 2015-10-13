/*=============================================================================================================
Open Dental is a dental practice management program.
Copyright (C) 2003-2013  Jordan Sparks, DMD.  http://www.opendental.com

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU Db Public License as published by the Free Software Foundation; either version 2 of the License,
or (at your option) any later version.

This program is distributed in the hope that it will be useful, but without any warranty. See the GNU Db Public License
for more details, available at http://www.opensource.org/licenses/gpl-license.php

Any changes to this program must follow the guidelines of the GPL license if a modified version is to be
redistributed.
===============================================================================================================*/


//#define ORA_DB
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using SparksToothChart;
//using OpenDental.SmartCards;
using OpenDental.UI;
using System.ServiceProcess;
using System.Linq;
using OpenDental.Bridges;
#if EHRTEST
using EHR;
#endif

//#if(ORA_DB)
//using OD_CRYPTO;
//#endif

namespace OpenDental{
	///<summary></summary>
	public class FormOpenDental:System.Windows.Forms.Form {
		private System.ComponentModel.IContainer components;
		//private bool[,] buttonDown=new bool[2,6];
		private System.Windows.Forms.Timer timerTimeIndic;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSettings;
		private System.Windows.Forms.MenuItem menuItemReports;
		private System.Windows.Forms.MenuItem menuItemPrinter;
		private System.Windows.Forms.MenuItem menuItemDataPath;
		private System.Windows.Forms.MenuItem menuItemConfig;
		private System.Windows.Forms.MenuItem menuItemAutoCodes;
		private System.Windows.Forms.MenuItem menuItemDefinitions;
		private System.Windows.Forms.MenuItem menuItemInsCats;
		private System.Windows.Forms.MenuItem menuItemLinks;
		private System.Windows.Forms.MenuItem menuItemRecall;
		private System.Windows.Forms.MenuItem menuItemEmployees;
		private System.Windows.Forms.MenuItem menuItemPractice;
		private System.Windows.Forms.MenuItem menuItemPrescriptions;
		private System.Windows.Forms.MenuItem menuItemProviders;
		private System.Windows.Forms.MenuItem menuItemProcCodes;
		private System.Windows.Forms.MenuItem menuItemPrintScreen;
		private System.Windows.Forms.MenuItem menuItemFinanceCharge;
		private System.Windows.Forms.MenuItem menuItemAging;
		private System.Windows.Forms.MenuItem menuItemSched;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemTranslation;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItemLists;
		private System.Windows.Forms.MenuItem menuItemTools;
		private System.Windows.Forms.MenuItem menuItemReferrals;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemDatabaseMaintenance;
		private System.Windows.Forms.MenuItem menuItemProcedureButtons;
		private System.Windows.Forms.MenuItem menuItemZipCodes;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuTelephone;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItemHelpIndex;
		private System.Windows.Forms.MenuItem menuItemClaimForms;
		private System.Windows.Forms.MenuItem menuItemContacts;
		private System.Windows.Forms.MenuItem menuItemMedications;
		private System.Windows.Forms.ImageList imageList32;
		private System.Windows.Forms.MenuItem menuItemApptViews;
		private System.Windows.Forms.MenuItem menuItemComputers;
		private System.Windows.Forms.MenuItem menuItemEmployers;
		private System.Windows.Forms.MenuItem menuItemEasy;
		private System.Windows.Forms.MenuItem menuItemCarriers;
		private System.Windows.Forms.MenuItem menuItemSchools;
		private System.Windows.Forms.MenuItem menuItemCounties;
		private System.Windows.Forms.MenuItem menuItemScreening;
		private System.Windows.Forms.MenuItem menuItemEmail;
		private System.Windows.Forms.MenuItem menuItemHelpContents;
		private System.Windows.Forms.MenuItem menuItemHelp;
		///<summary>The only reason this is public static is so that it can be seen from the terminal manager.  Otherwise, it's passed around properly.  I also used it in UserControlPhonePanel for simplicity</summary>
		public static long CurPatNum;
		private System.Windows.Forms.MenuItem menuItemClearinghouses;
		private System.Windows.Forms.MenuItem menuItemUpdate;
		private System.Windows.Forms.MenuItem menuItemHelpWindows;
		private System.Windows.Forms.MenuItem menuItemMisc;
		private System.Windows.Forms.MenuItem menuItemRemote;
		private System.Windows.Forms.MenuItem menuItemSchoolClass;
		private System.Windows.Forms.MenuItem menuItemSchoolCourses;
		private System.Windows.Forms.MenuItem menuItemSecurity;
		private System.Windows.Forms.MenuItem menuItemLogOff;
		private System.Windows.Forms.MenuItem menuItemInsPlans;
		private System.Windows.Forms.MenuItem menuItemClinics;
		private System.Windows.Forms.MenuItem menuItemOperatories;
		private System.Windows.Forms.Timer timerSignals;
		///<summary>When user logs out, this keeps track of where they were for when they log back in.</summary>
		private int LastModule;
		private System.Windows.Forms.MenuItem menuItemRepeatingCharges;
		private System.Windows.Forms.MenuItem menuItemImportXML;
		private MenuItem menuItemTimeCards;
		private MenuItem menuItemApptRules;
		private MenuItem menuItemAuditTrail;
		private MenuItem menuItemPatFieldDefs;
		private MenuItem menuItemTerminal;
		private MenuItem menuItemTerminalManager;
		private MenuItem menuItemQuestions;
		private MenuItem menuItemCustomReports;
		private MenuItem menuItemMessaging;
		private OpenDental.UI.LightSignalGrid lightSignalGrid1;
		private MenuItem menuItemMessagingButs;
		///<summary>This is not the actual date/time last refreshed.  It is really the server based date/time of the last item in the database retrieved on previous refreshes.  That way, the local workstation time is irrelevant.</summary>
		public static DateTime signalLastRefreshed;
		private FormSplash Splash;
		private Bitmap bitmapIcon;
		private MenuItem menuItemCreateAtoZFolders;
		private MenuItem menuItemLaboratories;
		///<summary>A list of button definitions for this computer.</summary>
		private SigButDef[] SigButDefList;
		/// <summary>Added these 3 fields for Oracle encrypted connection string</summary>
		private string connStr;
		private string key;
		private MenuItem menuItemGraphics;
		private MenuItem menuItemLabCases;
		private MenuItem menuItemRequirementsNeeded;
		private MenuItem menuItemReqStudents;
		private MenuItem menuItemAutoNotes;
		private MenuItem menuItemDisplayFields;
		private Panel panelSplitter;
		//private string dconnStr;
		private bool MouseIsDownOnSplitter;
		private Point SplitterOriginalLocation;
		private ContextMenu menuSplitter;
		private MenuItem menuItemDockBottom;
		private MenuItem menuItemDockRight;
		private ImageList imageListMain;
		private ContextMenu menuPatient;
		private ContextMenu menuLabel;
		private ContextMenu menuEmail;
		private ContextMenu menuLetter;
		private Point OriginalMousePos;
		private MenuItem menuItemCustomerManage;
		private System.Windows.Forms.Timer timerDisabledKey;
		///<summary>This list will only contain events for this computer where the users clicked to disable a popup for a specified period of time.  So it won't typically have many items in it.</summary>
		private List<PopupEvent> PopupEventList;
		private MenuItem menuItemPharmacies;
		private MenuItem menuItemSheets;
		private MenuItem menuItemRequestFeatures;
		private MenuItem menuItemModules;
		private MenuItem menuItemRecallTypes;
		private MenuItem menuItemFeeScheds;
		private MenuItem menuItemMobileSetup;
		private MenuItem menuItemLetters;
		//private UserControlPhonePanel phonePanel;
		///<summary>Command line args passed in when program starts.</summary>
		public string[] CommandLineArgs;
		//private Thread ThreadCommandLine;
		///<summary>Listens for commands coming from other instances of Open Dental on this computer.</summary>
		private TcpListener TcpListenerCommandLine;
		///<summary>True if there is already a different instance of OD running.  This prevents attempting to start the listener.</summary>
		public bool IsSecondInstance;
		private UserControlTasks userControlTasks1;
		private ContrAppt ContrAppt2;
		private ContrFamily ContrFamily2;
		private ContrFamilyEcw ContrFamily2Ecw;
		private ContrAccount ContrAccount2;
		private ContrTreat ContrTreat2;
		private ContrChart ContrChart2;
		private ContrImages ContrImages2;
		private ContrStaff ContrManage2;
		private OutlookBar myOutlookBar;
		private MenuItem menuItemShutdown;
		private System.Windows.Forms.Timer timerHeartBeat;
		private MenuItem menuItemInsFilingCodes;
		private MenuItem menuItemReplication;
		private MenuItem menuItemAutomation;
		private MenuItem menuItemMergePatients;
		private MenuItem menuItemDuplicateBlockouts;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private MenuItem menuItemPassword;
		private MenuItem menuItem3;
		private MenuItem menuApptFieldDefs;
		private MenuItem menuItemWebForms;
		private FormTerminalManager formTerminalManager;
		private FormPhoneTiles formPhoneTiles;
		private FormMapHQ formMapHQ;
		private System.Windows.Forms.Timer timerWebHostSynch;
		private MenuItem menuItemCCRecurring;
		private UserControlPhoneSmall phoneSmall;
		/////<summary>This will be null if EHR didn't load up.  EHRTEST conditional compilation constant is used because the EHR project is only part of the solution here at HQ.  We need to use late binding in a few places so that it will still compile for people who download our sourcecode.  But late binding prevents us from stepping through for debugging, so the EHRTEST lets us switch to early binding.</summary>
		//public static object ObjFormEhrMeasures;
		private MenuItem menuItemEHR;
		private System.Windows.Forms.Timer timerLogoff;
		//<summary>This will be null if EHR didn't load up.</summary>
		//public static Assembly AssemblyEHR;
		///<summary>The last local datetime that there was any mouse or keyboard activity.  Used for auto logoff comparison.</summary>
		private DateTime dateTimeLastActivity;
		private Form FormRecentlyOpenForLogoff;
		private Point locationMouseForLogoff;
		private MenuItem menuItemPayerIDs;
		private MenuItem menuItemTestLatency;
		private FormLogOn FormLogOn_;
		private System.Windows.Forms.Timer timerReplicationMonitor;
		///<summary>When auto log off is in use, we don't want to log off user if they are in the FormLogOn window.  Mostly a problem when using web service because CurUser is not null.</summary>
		private bool IsFormLogOnLastActive;
		private Panel panelPhoneSmall;
		private UI.Button butTriage;
		private UI.Button butBigPhones;
		private Label labelWaitTime;
		private Label labelTriage;
		private Label labelMsg;
		///<summary>This thread fills labelMsg</summary>
		private Thread ThreadVM;
		private ODThread _odThreadHqMetrics;
		private MenuItem menuItemWiki;
		private MenuItem menuItemProcLockTool;
		private MenuItem menuItemHL7;
		private MenuItem menuItemNewCropBilling;
		private MenuItem menuItemSpellCheck;
		private FormWiki FormMyWiki;
		private MenuItem menuItemResellers;
		private MenuItem menuItemXChargeReconcile;
		private FormCreditRecurringCharges FormCRC;
		private UI.Button butMapPhones;
		private ComboBox comboTriageCoordinator;
		private Label labelFieldType;
		///<summary>If the local computer is the computer where Podium invitations are sent, then this thread runs in the background and checks for 
		///appointments that started 10-40 minutes ago (depending on in the patient is a new patient) at 10 minute intervals.  No preferences.
		///In the future, some sort of identification should be made to tell if this thread is running on any computer.</summary>
		private ODThread _threadPodium;
		private int _podiumIntervalMS=(int)TimeSpan.FromMinutes(5).TotalMilliseconds;
		///<summary>If the local computer is the computer where incoming email is fetched, then this thread runs in the background and checks for new messages 
		///every x number of minutes (1 to 60) based on preference value.</summary>
		private Thread ThreadEmailInbox;
		private bool _isEmailThreadRunning=false;
		private AutoResetEvent _emailSleep=new AutoResetEvent(false);
				/// <summary>If OpenDental is running on the same machine as the mysql server, then a thread is runs in the background to update the local machine's time
		///using NTPv4 from the NIST time server set in the NistTimeServerUrl pref./// </summary>
		private Thread _threadTimeSynch;
		private bool _isTimeSynchThreadRunning=false;
		private MenuItem menuItemAppointments;
		private MenuItem menuItem8;
		private MenuItem menuItemPreferencesAppts;
		private MenuItem menuItemFamily;
		private MenuItem menuItem10;
		private MenuItem menuItemPreferencesFamily;
		private MenuItem menuItemChart;
		private MenuItem menuItem13;
		private MenuItem menuItemPreferencesChart;
		private MenuItem menuItemImages;
		private MenuItem menuItemManage;
		private MenuItem menuItem17;
		private MenuItem menuItemPreferencesManage;
		private MenuItem menuItem19;
		private MenuItem menuItem20;
		private MenuItem menuItemObsolete;
		private MenuItem menuItemAdvancedSetup;
		private MenuItem menuItemAccount;
		private MenuItem menuItemPreferencesTreatPlan;
		private MenuItem menuItemPreferencesImages;
		private MenuItem menuItem2;
		private MenuItem menuItemImagingPerComp;
		private MenuItem menuItemAllergies;
		private MenuItem menuItemProblems;
		private MenuItem menuItemDentalSchools;
		private MenuItem menuItemEvaluations;
		private AutoResetEvent _timeSynchSleep=new AutoResetEvent(false);
		///<summary>Used to stop threads from running during updates.  Currently only used to stop Mobile Synch.</summary>
		private bool _isStartingUp;
		private long _previousPatNum;
		private MenuItem menuItemDispensary;
		private MenuItem menuItemApptTypes;
		private DateTime _datePopupDelay;
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the Chart module have been changed.</summary>
		private Dictionary<string,object> dictChartPrefsCache=new Dictionary<string,object>();
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the non-modal task list have been changed.</summary>
		private Dictionary<string,object> dictTaskListPrefsCache=new Dictionary<string,object>();
		///<summary>Defaulted to 0 - Headquarters.  This is used by other modules to filter some areas of the program to the selected clinic. If a user is restricted to a specific clinic, this value will be set to that clinic and the user will not be able to select a different clinic.</summary>
		public static long ClinicNum=0;
		///<summary>This is used to determine how Open Dental closed.  If this is set to anything but 0 then some kind of error occurred and Open Dental was forced to close.  Currently only used when updating Open Dental silently.</summary>
		public static int ExitCode=0;
		///<summary>Will be set to true if the STOP SLAVE SQL was run on the replication server for which the local replication monitor is watching.
		///Replicaiton is NOT broken when this flag is true, because the user can re-enable replicaiton using the START SLAVE SQL without any ill effects.
		///This flag is used to display a warning to the user, but will not ever block the user from using OD.</summary>
		private bool _isReplicationSlaveStopped=false;
		private MenuItem menuClinics;
		private MenuItem menuItemEServices;
		private MenuItem menuItemPatientPortal;
		private MenuItem menuMobileWeb;
		private MenuItem menuItemIntegratedTexting;
		private MenuItem menuItemWebSched;
		private MenuItem menuItem14;
		private MenuItem menuItemListenerService;
		private MenuItem menuItemMobileSync;
		///<summary>HQ only. Keep track of last time triage task labels were filled. Too taxing on the server to perform every 1.6 seconds with the rest of the HQ thread metrics. Triage labels will be refreshed on ProcessSigsIntervalInSecs interval.</summary>
		DateTime _hqTriageMetricsLastRefreshed=DateTime.MinValue;
		///<summary>HQ only. Keep track of last time EServiceMetrics were filled. Server is only updating every 30 seconds so no need to go any faster than that.</summary>
		DateTime _hqEServiceMetricsLastRefreshed=DateTime.MinValue;
		///<summary>The current color of the eServices menu item in the main menu.</summary>
		private Color _colorEServicesBackground=SystemColors.Control;
		private ODThread _odThreadEServices;
		private ContextMenu menuText;
		private MenuItem menuItemTextMessagesReceived;
		private MenuItem menuItemTextMessagesSent;
		private MenuItem menuItemTextMessagesAll;
		private MenuItem menuItemRemoteSupport;
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
		///<summary>A specific reference to the "Text" button.  This special reference helps us preserve the notification text on the button after setup is modified.</summary>
		private ODToolBarButton _butText;
		private string _showForm="";
		private MenuItem menuItemPreferencesAccount;
		private MenuItem menuItem12;
		private MenuItem menuItemCCProcs;
		private FormSmsTextMessaging _formSmsTextMessaging;

		///<summary></summary>
		public FormOpenDental(string[] cla){
			Logger.openlog.Log("Initializing Open Dental...",Logger.Severity.INFO);
			CommandLineArgs=cla;
			Splash=new FormSplash();
			if(CommandLineArgs.Length==0) {
				Splash.Show();
			}
			InitializeComponent();
			SystemEvents.SessionSwitch+=new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
			//toolbar
			ToolBarMain=new ODToolBar();
			ToolBarMain.Location=new Point(51,0);
			ToolBarMain.Size=new Size(931,25);
			ToolBarMain.Dock=DockStyle.Top;
			ToolBarMain.ImageList=imageListMain;
			ToolBarMain.ButtonClick+=new ODToolBarButtonClickEventHandler(ToolBarMain_ButtonClick);
			this.Controls.Add(ToolBarMain);
			//outlook bar
			myOutlookBar=new OutlookBar();
			myOutlookBar.Location=new Point(0,0);
			myOutlookBar.Size=new Size(51,626);
			myOutlookBar.ImageList=imageList32;
			myOutlookBar.Dock=DockStyle.Left;
			myOutlookBar.ButtonClicked+=new ButtonClickedEventHandler(myOutlookBar_ButtonClicked);
			this.Controls.Add(myOutlookBar);
			//contrAppt
			ContrAppt2=new ContrAppt();
			ContrAppt2.Visible=false;
			ContrAppt2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrAppt2);
			//contrFamily
			ContrFamily2=new ContrFamily();
			ContrFamily2.Visible=false;
			ContrFamily2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrFamily2);
			//contrFamilyEcw
			ContrFamily2Ecw=new ContrFamilyEcw();
			ContrFamily2Ecw.Visible=false;
			//ContrFamily2Ecw.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrFamily2Ecw);
			//contrAccount
			ContrAccount2=new ContrAccount();
			ContrAccount2.Visible=false;
			ContrAccount2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrAccount2);
			//contrTreat
			ContrTreat2=new ContrTreat();
			ContrTreat2.Visible=false;
			ContrTreat2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrTreat2);
			//contrChart
			ContrChart2=new ContrChart();
			ContrChart2.Visible=false;
			ContrChart2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrChart2);
			//contrImages
			ContrImages2=new ContrImages();
			ContrImages2.Visible=false;
			ContrImages2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrImages2);
			//contrManage
			ContrManage2=new ContrStaff();
			ContrManage2.Visible=false;
			ContrManage2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			this.Controls.Add(ContrManage2);
			//userControlTasks
			userControlTasks1=new UserControlTasks();
			userControlTasks1.Visible=false;
			userControlTasks1.GoToChanged+=new EventHandler(userControlTasks1_GoToChanged);
			GotoModule.ModuleSelected+=new ModuleEventHandler(GotoModule_ModuleSelected);
			this.Controls.Add(userControlTasks1);
			panelSplitter.ContextMenu=menuSplitter;
			menuItemDockBottom.Checked=true;
			phoneSmall=new UserControlPhoneSmall();
			phoneSmall.GoToChanged += new System.EventHandler(this.phoneSmall_GoToChanged);
			//phoneSmall.Visible=false;
			//this.Controls.Add(phoneSmall);
			panelPhoneSmall.Controls.Add(phoneSmall);
			panelPhoneSmall.Visible=false;
			//phonePanel=new UserControlPhonePanel();
			//phonePanel.Visible=false;
			//this.Controls.Add(phonePanel);
			//phonePanel.GoToChanged += new System.EventHandler(this.phonePanel_GoToChanged);
			Logger.openlog.Log("Open Dental initialization complete.",Logger.Severity.INFO);
			//Plugins.HookAddCode(this,"FormOpenDental.Constructor_end");//Can't do this because no plugins loaded.
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
				if(FormCRC!=null) {
					FormCRC.Dispose();
				}
				if(FormMyWiki!=null) {
					FormMyWiki.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpenDental));
			this.timerTimeIndic = new System.Windows.Forms.Timer(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemLogOff = new System.Windows.Forms.MenuItem();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemPassword = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemPrinter = new System.Windows.Forms.MenuItem();
			this.menuItemGraphics = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemConfig = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItemAppointments = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesAppts = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuApptFieldDefs = new System.Windows.Forms.MenuItem();
			this.menuItemApptRules = new System.Windows.Forms.MenuItem();
			this.menuItemApptTypes = new System.Windows.Forms.MenuItem();
			this.menuItemApptViews = new System.Windows.Forms.MenuItem();
			this.menuItemOperatories = new System.Windows.Forms.MenuItem();
			this.menuItemRecall = new System.Windows.Forms.MenuItem();
			this.menuItemRecallTypes = new System.Windows.Forms.MenuItem();
			this.menuItemFamily = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesFamily = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItemClaimForms = new System.Windows.Forms.MenuItem();
			this.menuItemClearinghouses = new System.Windows.Forms.MenuItem();
			this.menuItemInsCats = new System.Windows.Forms.MenuItem();
			this.menuItemInsFilingCodes = new System.Windows.Forms.MenuItem();
			this.menuItemPatFieldDefs = new System.Windows.Forms.MenuItem();
			this.menuItemPayerIDs = new System.Windows.Forms.MenuItem();
			this.menuItemAccount = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesAccount = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItemCCProcs = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesTreatPlan = new System.Windows.Forms.MenuItem();
			this.menuItemChart = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesChart = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItemEHR = new System.Windows.Forms.MenuItem();
			this.menuItemProcedureButtons = new System.Windows.Forms.MenuItem();
			this.menuItemImages = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesImages = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemImagingPerComp = new System.Windows.Forms.MenuItem();
			this.menuItemManage = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesManage = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItemEmail = new System.Windows.Forms.MenuItem();
			this.menuItemMessaging = new System.Windows.Forms.MenuItem();
			this.menuItemMessagingButs = new System.Windows.Forms.MenuItem();
			this.menuItemTimeCards = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItemAdvancedSetup = new System.Windows.Forms.MenuItem();
			this.menuItemComputers = new System.Windows.Forms.MenuItem();
			this.menuItemHL7 = new System.Windows.Forms.MenuItem();
			this.menuItemReplication = new System.Windows.Forms.MenuItem();
			this.menuItemEasy = new System.Windows.Forms.MenuItem();
			this.menuItemAutoCodes = new System.Windows.Forms.MenuItem();
			this.menuItemAutomation = new System.Windows.Forms.MenuItem();
			this.menuItemAutoNotes = new System.Windows.Forms.MenuItem();
			this.menuItemDataPath = new System.Windows.Forms.MenuItem();
			this.menuItemDefinitions = new System.Windows.Forms.MenuItem();
			this.menuItemDentalSchools = new System.Windows.Forms.MenuItem();
			this.menuItemDisplayFields = new System.Windows.Forms.MenuItem();
			this.menuItemFeeScheds = new System.Windows.Forms.MenuItem();
			this.menuItemLaboratories = new System.Windows.Forms.MenuItem();
			this.menuItemMisc = new System.Windows.Forms.MenuItem();
			this.menuItemModules = new System.Windows.Forms.MenuItem();
			this.menuItemPractice = new System.Windows.Forms.MenuItem();
			this.menuItemLinks = new System.Windows.Forms.MenuItem();
			this.menuItemRequirementsNeeded = new System.Windows.Forms.MenuItem();
			this.menuItemSched = new System.Windows.Forms.MenuItem();
			this.menuItemSecurity = new System.Windows.Forms.MenuItem();
			this.menuItemSheets = new System.Windows.Forms.MenuItem();
			this.menuItemSpellCheck = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItemObsolete = new System.Windows.Forms.MenuItem();
			this.menuItemLetters = new System.Windows.Forms.MenuItem();
			this.menuItemQuestions = new System.Windows.Forms.MenuItem();
			this.menuItemLists = new System.Windows.Forms.MenuItem();
			this.menuItemProcCodes = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemAllergies = new System.Windows.Forms.MenuItem();
			this.menuItemClinics = new System.Windows.Forms.MenuItem();
			this.menuItemContacts = new System.Windows.Forms.MenuItem();
			this.menuItemCounties = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolClass = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolCourses = new System.Windows.Forms.MenuItem();
			this.menuItemEmployees = new System.Windows.Forms.MenuItem();
			this.menuItemEmployers = new System.Windows.Forms.MenuItem();
			this.menuItemCarriers = new System.Windows.Forms.MenuItem();
			this.menuItemInsPlans = new System.Windows.Forms.MenuItem();
			this.menuItemLabCases = new System.Windows.Forms.MenuItem();
			this.menuItemMedications = new System.Windows.Forms.MenuItem();
			this.menuItemPharmacies = new System.Windows.Forms.MenuItem();
			this.menuItemProblems = new System.Windows.Forms.MenuItem();
			this.menuItemProviders = new System.Windows.Forms.MenuItem();
			this.menuItemPrescriptions = new System.Windows.Forms.MenuItem();
			this.menuItemReferrals = new System.Windows.Forms.MenuItem();
			this.menuItemSchools = new System.Windows.Forms.MenuItem();
			this.menuItemZipCodes = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemCustomReports = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemPrintScreen = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemDuplicateBlockouts = new System.Windows.Forms.MenuItem();
			this.menuItemCreateAtoZFolders = new System.Windows.Forms.MenuItem();
			this.menuItemImportXML = new System.Windows.Forms.MenuItem();
			this.menuItemMergePatients = new System.Windows.Forms.MenuItem();
			this.menuItemProcLockTool = new System.Windows.Forms.MenuItem();
			this.menuItemShutdown = new System.Windows.Forms.MenuItem();
			this.menuTelephone = new System.Windows.Forms.MenuItem();
			this.menuItemTestLatency = new System.Windows.Forms.MenuItem();
			this.menuItemXChargeReconcile = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItemAging = new System.Windows.Forms.MenuItem();
			this.menuItemAuditTrail = new System.Windows.Forms.MenuItem();
			this.menuItemFinanceCharge = new System.Windows.Forms.MenuItem();
			this.menuItemCCRecurring = new System.Windows.Forms.MenuItem();
			this.menuItemCustomerManage = new System.Windows.Forms.MenuItem();
			this.menuItemDatabaseMaintenance = new System.Windows.Forms.MenuItem();
			this.menuItemDispensary = new System.Windows.Forms.MenuItem();
			this.menuItemEvaluations = new System.Windows.Forms.MenuItem();
			this.menuItemTerminal = new System.Windows.Forms.MenuItem();
			this.menuItemTerminalManager = new System.Windows.Forms.MenuItem();
			this.menuItemTranslation = new System.Windows.Forms.MenuItem();
			this.menuItemMobileSetup = new System.Windows.Forms.MenuItem();
			this.menuItemNewCropBilling = new System.Windows.Forms.MenuItem();
			this.menuItemScreening = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatingCharges = new System.Windows.Forms.MenuItem();
			this.menuItemResellers = new System.Windows.Forms.MenuItem();
			this.menuItemReqStudents = new System.Windows.Forms.MenuItem();
			this.menuItemWebForms = new System.Windows.Forms.MenuItem();
			this.menuItemWiki = new System.Windows.Forms.MenuItem();
			this.menuClinics = new System.Windows.Forms.MenuItem();
			this.menuItemEServices = new System.Windows.Forms.MenuItem();
			this.menuItemMobileSync = new System.Windows.Forms.MenuItem();
			this.menuMobileWeb = new System.Windows.Forms.MenuItem();
			this.menuItemPatientPortal = new System.Windows.Forms.MenuItem();
			this.menuItemIntegratedTexting = new System.Windows.Forms.MenuItem();
			this.menuItemWebSched = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItemListenerService = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemRemote = new System.Windows.Forms.MenuItem();
			this.menuItemHelpWindows = new System.Windows.Forms.MenuItem();
			this.menuItemHelpContents = new System.Windows.Forms.MenuItem();
			this.menuItemHelpIndex = new System.Windows.Forms.MenuItem();
			this.menuItemRemoteSupport = new System.Windows.Forms.MenuItem();
			this.menuItemRequestFeatures = new System.Windows.Forms.MenuItem();
			this.menuItemUpdate = new System.Windows.Forms.MenuItem();
			this.imageList32 = new System.Windows.Forms.ImageList(this.components);
			this.timerSignals = new System.Windows.Forms.Timer(this.components);
			this.panelSplitter = new System.Windows.Forms.Panel();
			this.menuSplitter = new System.Windows.Forms.ContextMenu();
			this.menuItemDockBottom = new System.Windows.Forms.MenuItem();
			this.menuItemDockRight = new System.Windows.Forms.MenuItem();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.menuPatient = new System.Windows.Forms.ContextMenu();
			this.menuLabel = new System.Windows.Forms.ContextMenu();
			this.menuEmail = new System.Windows.Forms.ContextMenu();
			this.menuLetter = new System.Windows.Forms.ContextMenu();
			this.timerDisabledKey = new System.Windows.Forms.Timer(this.components);
			this.timerHeartBeat = new System.Windows.Forms.Timer(this.components);
			this.timerWebHostSynch = new System.Windows.Forms.Timer(this.components);
			this.timerLogoff = new System.Windows.Forms.Timer(this.components);
			this.timerReplicationMonitor = new System.Windows.Forms.Timer(this.components);
			this.panelPhoneSmall = new System.Windows.Forms.Panel();
			this.labelFieldType = new System.Windows.Forms.Label();
			this.comboTriageCoordinator = new System.Windows.Forms.ComboBox();
			this.labelMsg = new System.Windows.Forms.Label();
			this.butMapPhones = new OpenDental.UI.Button();
			this.butTriage = new OpenDental.UI.Button();
			this.butBigPhones = new OpenDental.UI.Button();
			this.labelWaitTime = new System.Windows.Forms.Label();
			this.labelTriage = new System.Windows.Forms.Label();
			this.menuText = new System.Windows.Forms.ContextMenu();
			this.menuItemTextMessagesAll = new System.Windows.Forms.MenuItem();
			this.menuItemTextMessagesReceived = new System.Windows.Forms.MenuItem();
			this.menuItemTextMessagesSent = new System.Windows.Forms.MenuItem();
			this.lightSignalGrid1 = new OpenDental.UI.LightSignalGrid();
			this.panelPhoneSmall.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerTimeIndic
			// 
			this.timerTimeIndic.Interval = 60000;
			this.timerTimeIndic.Tick += new System.EventHandler(this.timerTimeIndic_Tick);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogOff,
            this.menuItemFile,
            this.menuItemSettings,
            this.menuItemLists,
            this.menuItemReports,
            this.menuItemCustomReports,
            this.menuItemTools,
            this.menuClinics,
            this.menuItemEServices,
            this.menuItemHelp});
			// 
			// menuItemLogOff
			// 
			this.menuItemLogOff.Index = 0;
			this.menuItemLogOff.Text = "Log &Off";
			this.menuItemLogOff.Click += new System.EventHandler(this.menuItemLogOff_Click);
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 1;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPassword,
            this.menuItem3,
            this.menuItemPrinter,
            this.menuItemGraphics,
            this.menuItem6,
            this.menuItemConfig,
            this.menuItem7,
            this.menuItemExit});
			this.menuItemFile.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItemFile.Text = "&File";
			// 
			// menuItemPassword
			// 
			this.menuItemPassword.Index = 0;
			this.menuItemPassword.Text = "Change Password";
			this.menuItemPassword.Click += new System.EventHandler(this.menuItemPassword_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuItemPrinter
			// 
			this.menuItemPrinter.Index = 2;
			this.menuItemPrinter.Text = "&Printers";
			this.menuItemPrinter.Click += new System.EventHandler(this.menuItemPrinter_Click);
			// 
			// menuItemGraphics
			// 
			this.menuItemGraphics.Index = 3;
			this.menuItemGraphics.Text = "Graphics";
			this.menuItemGraphics.Click += new System.EventHandler(this.menuItemGraphics_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// menuItemConfig
			// 
			this.menuItemConfig.Index = 5;
			this.menuItemConfig.Text = "&Choose Database";
			this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 6;
			this.menuItem7.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 7;
			this.menuItemExit.ShowShortcut = false;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 2;
			this.menuItemSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAppointments,
            this.menuItemFamily,
            this.menuItemAccount,
            this.menuItemPreferencesTreatPlan,
            this.menuItemChart,
            this.menuItemImages,
            this.menuItemManage,
            this.menuItem19,
            this.menuItemAdvancedSetup,
            this.menuItemAutoCodes,
            this.menuItemAutomation,
            this.menuItemAutoNotes,
            this.menuItemDataPath,
            this.menuItemDefinitions,
            this.menuItemDentalSchools,
            this.menuItemDisplayFields,
            this.menuItemFeeScheds,
            this.menuItemLaboratories,
            this.menuItemMisc,
            this.menuItemModules,
            this.menuItemPractice,
            this.menuItemLinks,
            this.menuItemRequirementsNeeded,
            this.menuItemSched,
            this.menuItemSecurity,
            this.menuItemSheets,
            this.menuItemSpellCheck,
            this.menuItem20,
            this.menuItemObsolete});
			this.menuItemSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItemSettings.Text = "&Setup";
			// 
			// menuItemAppointments
			// 
			this.menuItemAppointments.Index = 0;
			this.menuItemAppointments.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesAppts,
            this.menuItem8,
            this.menuApptFieldDefs,
            this.menuItemApptRules,
            this.menuItemApptTypes,
            this.menuItemApptViews,
            this.menuItemOperatories,
            this.menuItemRecall,
            this.menuItemRecallTypes});
			this.menuItemAppointments.Text = "Appointments";
			// 
			// menuItemPreferencesAppts
			// 
			this.menuItemPreferencesAppts.Index = 0;
			this.menuItemPreferencesAppts.Text = "Appts Preferences";
			this.menuItemPreferencesAppts.Click += new System.EventHandler(this.menuItemPreferencesAppts_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// menuApptFieldDefs
			// 
			this.menuApptFieldDefs.Index = 2;
			this.menuApptFieldDefs.Text = "Appointment Field Defs";
			this.menuApptFieldDefs.Click += new System.EventHandler(this.menuItemApptFieldDefs_Click);
			// 
			// menuItemApptRules
			// 
			this.menuItemApptRules.Index = 3;
			this.menuItemApptRules.Text = "Appointment Rules";
			this.menuItemApptRules.Click += new System.EventHandler(this.menuItemApptRules_Click);
			// 
			// menuItemApptTypes
			// 
			this.menuItemApptTypes.Index = 4;
			this.menuItemApptTypes.Text = "Appointment Types";
			this.menuItemApptTypes.Click += new System.EventHandler(this.menuItemApptTypes_Click);
			// 
			// menuItemApptViews
			// 
			this.menuItemApptViews.Index = 5;
			this.menuItemApptViews.Text = "Appointment Views";
			this.menuItemApptViews.Click += new System.EventHandler(this.menuItemApptViews_Click);
			// 
			// menuItemOperatories
			// 
			this.menuItemOperatories.Index = 6;
			this.menuItemOperatories.Text = "Operatories";
			this.menuItemOperatories.Click += new System.EventHandler(this.menuItemOperatories_Click);
			// 
			// menuItemRecall
			// 
			this.menuItemRecall.Index = 7;
			this.menuItemRecall.Text = "Recall";
			this.menuItemRecall.Click += new System.EventHandler(this.menuItemRecall_Click);
			// 
			// menuItemRecallTypes
			// 
			this.menuItemRecallTypes.Index = 8;
			this.menuItemRecallTypes.Text = "RecallTypes";
			this.menuItemRecallTypes.Click += new System.EventHandler(this.menuItemRecallTypes_Click);
			// 
			// menuItemFamily
			// 
			this.menuItemFamily.Index = 1;
			this.menuItemFamily.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesFamily,
            this.menuItem10,
            this.menuItemClaimForms,
            this.menuItemClearinghouses,
            this.menuItemInsCats,
            this.menuItemInsFilingCodes,
            this.menuItemPatFieldDefs,
            this.menuItemPayerIDs});
			this.menuItemFamily.Text = "Family / Insurance";
			// 
			// menuItemPreferencesFamily
			// 
			this.menuItemPreferencesFamily.Index = 0;
			this.menuItemPreferencesFamily.Text = "Family Preferences";
			this.menuItemPreferencesFamily.Click += new System.EventHandler(this.menuItemPreferencesFamily_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.Text = "-";
			// 
			// menuItemClaimForms
			// 
			this.menuItemClaimForms.Index = 2;
			this.menuItemClaimForms.Text = "Claim Forms";
			this.menuItemClaimForms.Click += new System.EventHandler(this.menuItemClaimForms_Click);
			// 
			// menuItemClearinghouses
			// 
			this.menuItemClearinghouses.Index = 3;
			this.menuItemClearinghouses.Text = "Clearinghouses";
			this.menuItemClearinghouses.Click += new System.EventHandler(this.menuItemClearinghouses_Click);
			// 
			// menuItemInsCats
			// 
			this.menuItemInsCats.Index = 4;
			this.menuItemInsCats.Text = "Insurance Categories";
			this.menuItemInsCats.Click += new System.EventHandler(this.menuItemInsCats_Click);
			// 
			// menuItemInsFilingCodes
			// 
			this.menuItemInsFilingCodes.Index = 5;
			this.menuItemInsFilingCodes.Text = "Insurance Filing Codes";
			this.menuItemInsFilingCodes.Click += new System.EventHandler(this.menuItemInsFilingCodes_Click);
			// 
			// menuItemPatFieldDefs
			// 
			this.menuItemPatFieldDefs.Index = 6;
			this.menuItemPatFieldDefs.Text = "Patient Field Defs";
			this.menuItemPatFieldDefs.Click += new System.EventHandler(this.menuItemPatFieldDefs_Click);
			// 
			// menuItemPayerIDs
			// 
			this.menuItemPayerIDs.Index = 7;
			this.menuItemPayerIDs.Text = "Payer IDs";
			this.menuItemPayerIDs.Click += new System.EventHandler(this.menuItemPayerIDs_Click);
			// 
			// menuItemAccount
			// 
			this.menuItemAccount.Index = 2;
			this.menuItemAccount.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesAccount,
            this.menuItem12,
            this.menuItemCCProcs});
			this.menuItemAccount.Text = "Account";
			this.menuItemAccount.Click += new System.EventHandler(this.menuItemPreferencesAccount_Click);
			// 
			// menuItemPreferencesAccount
			// 
			this.menuItemPreferencesAccount.Index = 0;
			this.menuItemPreferencesAccount.Text = "Account Preferences";
			this.menuItemPreferencesAccount.Click += new System.EventHandler(this.menuItemPreferencesAccount_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 1;
			this.menuItem12.Text = "-";
			// 
			// menuItemCCProcs
			// 
			this.menuItemCCProcs.Index = 2;
			this.menuItemCCProcs.Text = "Default CC Procedures";
			this.menuItemCCProcs.Click += new System.EventHandler(this.menuItemDefaultCCProcs_Click);
			// 
			// menuItemPreferencesTreatPlan
			// 
			this.menuItemPreferencesTreatPlan.Index = 3;
			this.menuItemPreferencesTreatPlan.Text = "Treat\' Plan";
			this.menuItemPreferencesTreatPlan.Click += new System.EventHandler(this.menuItemPreferencesTreatPlan_Click);
			// 
			// menuItemChart
			// 
			this.menuItemChart.Index = 4;
			this.menuItemChart.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesChart,
            this.menuItem13,
            this.menuItemEHR,
            this.menuItemProcedureButtons});
			this.menuItemChart.Text = "Chart";
			// 
			// menuItemPreferencesChart
			// 
			this.menuItemPreferencesChart.Index = 0;
			this.menuItemPreferencesChart.Text = "Chart Preferences";
			this.menuItemPreferencesChart.Click += new System.EventHandler(this.menuItemPreferencesChart_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 1;
			this.menuItem13.Text = "-";
			// 
			// menuItemEHR
			// 
			this.menuItemEHR.Index = 2;
			this.menuItemEHR.Text = "EHR";
			this.menuItemEHR.Click += new System.EventHandler(this.menuItemEHR_Click);
			// 
			// menuItemProcedureButtons
			// 
			this.menuItemProcedureButtons.Index = 3;
			this.menuItemProcedureButtons.Text = "Procedure Buttons";
			this.menuItemProcedureButtons.Click += new System.EventHandler(this.menuItemProcedureButtons_Click);
			// 
			// menuItemImages
			// 
			this.menuItemImages.Index = 5;
			this.menuItemImages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesImages,
            this.menuItem2,
            this.menuItemImagingPerComp});
			this.menuItemImages.Text = "Images";
			// 
			// menuItemPreferencesImages
			// 
			this.menuItemPreferencesImages.Index = 0;
			this.menuItemPreferencesImages.Text = "Images Preferences";
			this.menuItemPreferencesImages.Click += new System.EventHandler(this.menuItemPreferencesImages_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// menuItemImagingPerComp
			// 
			this.menuItemImagingPerComp.Index = 2;
			this.menuItemImagingPerComp.Text = "Imaging Quality";
			this.menuItemImagingPerComp.Click += new System.EventHandler(this.menuItemImaging_Click);
			// 
			// menuItemManage
			// 
			this.menuItemManage.Index = 6;
			this.menuItemManage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesManage,
            this.menuItem17,
            this.menuItemEmail,
            this.menuItemMessaging,
            this.menuItemMessagingButs,
            this.menuItemTimeCards});
			this.menuItemManage.Text = "Manage";
			// 
			// menuItemPreferencesManage
			// 
			this.menuItemPreferencesManage.Index = 0;
			this.menuItemPreferencesManage.Text = "Manage Preferences";
			this.menuItemPreferencesManage.Click += new System.EventHandler(this.menuItemPreferencesManage_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "-";
			// 
			// menuItemEmail
			// 
			this.menuItemEmail.Index = 2;
			this.menuItemEmail.Text = "E-mail";
			this.menuItemEmail.Click += new System.EventHandler(this.menuItemEmail_Click);
			// 
			// menuItemMessaging
			// 
			this.menuItemMessaging.Index = 3;
			this.menuItemMessaging.Text = "Messaging";
			this.menuItemMessaging.Click += new System.EventHandler(this.menuItemMessaging_Click);
			// 
			// menuItemMessagingButs
			// 
			this.menuItemMessagingButs.Index = 4;
			this.menuItemMessagingButs.Text = "Messaging Buttons";
			this.menuItemMessagingButs.Click += new System.EventHandler(this.menuItemMessagingButs_Click);
			// 
			// menuItemTimeCards
			// 
			this.menuItemTimeCards.Index = 5;
			this.menuItemTimeCards.Text = "Time Cards";
			this.menuItemTimeCards.Click += new System.EventHandler(this.menuItemTimeCards_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 7;
			this.menuItem19.Text = "-";
			// 
			// menuItemAdvancedSetup
			// 
			this.menuItemAdvancedSetup.Index = 8;
			this.menuItemAdvancedSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemComputers,
            this.menuItemHL7,
            this.menuItemReplication,
            this.menuItemEasy});
			this.menuItemAdvancedSetup.Text = "Advanced Setup";
			// 
			// menuItemComputers
			// 
			this.menuItemComputers.Index = 0;
			this.menuItemComputers.Text = "Computers";
			this.menuItemComputers.Click += new System.EventHandler(this.menuItemComputers_Click);
			// 
			// menuItemHL7
			// 
			this.menuItemHL7.Index = 1;
			this.menuItemHL7.Text = "HL7";
			this.menuItemHL7.Click += new System.EventHandler(this.menuItemHL7_Click);
			// 
			// menuItemReplication
			// 
			this.menuItemReplication.Index = 2;
			this.menuItemReplication.Text = "Replication";
			this.menuItemReplication.Click += new System.EventHandler(this.menuItemReplication_Click);
			// 
			// menuItemEasy
			// 
			this.menuItemEasy.Index = 3;
			this.menuItemEasy.Text = "Show Features";
			this.menuItemEasy.Click += new System.EventHandler(this.menuItemEasy_Click);
			// 
			// menuItemAutoCodes
			// 
			this.menuItemAutoCodes.Index = 9;
			this.menuItemAutoCodes.Text = "Auto Codes";
			this.menuItemAutoCodes.Click += new System.EventHandler(this.menuItemAutoCodes_Click);
			// 
			// menuItemAutomation
			// 
			this.menuItemAutomation.Index = 10;
			this.menuItemAutomation.Text = "Automation";
			this.menuItemAutomation.Click += new System.EventHandler(this.menuItemAutomation_Click);
			// 
			// menuItemAutoNotes
			// 
			this.menuItemAutoNotes.Index = 11;
			this.menuItemAutoNotes.Text = "Auto Notes";
			this.menuItemAutoNotes.Click += new System.EventHandler(this.menuItemAutoNotes_Click);
			// 
			// menuItemDataPath
			// 
			this.menuItemDataPath.Index = 12;
			this.menuItemDataPath.Text = "Data Paths";
			this.menuItemDataPath.Click += new System.EventHandler(this.menuItemDataPath_Click);
			// 
			// menuItemDefinitions
			// 
			this.menuItemDefinitions.Index = 13;
			this.menuItemDefinitions.Text = "Definitions";
			this.menuItemDefinitions.Click += new System.EventHandler(this.menuItemDefinitions_Click);
			// 
			// menuItemDentalSchools
			// 
			this.menuItemDentalSchools.Index = 14;
			this.menuItemDentalSchools.Text = "Dental Schools";
			this.menuItemDentalSchools.Click += new System.EventHandler(this.menuItemDentalSchools_Click);
			// 
			// menuItemDisplayFields
			// 
			this.menuItemDisplayFields.Index = 15;
			this.menuItemDisplayFields.Text = "Display Fields";
			this.menuItemDisplayFields.Click += new System.EventHandler(this.menuItemDisplayFields_Click);
			// 
			// menuItemFeeScheds
			// 
			this.menuItemFeeScheds.Index = 16;
			this.menuItemFeeScheds.Text = "Fee Schedules";
			this.menuItemFeeScheds.Click += new System.EventHandler(this.menuItemFeeScheds_Click);
			// 
			// menuItemLaboratories
			// 
			this.menuItemLaboratories.Index = 17;
			this.menuItemLaboratories.Text = "Laboratories";
			this.menuItemLaboratories.Click += new System.EventHandler(this.menuItemLaboratories_Click);
			// 
			// menuItemMisc
			// 
			this.menuItemMisc.Index = 18;
			this.menuItemMisc.Text = "Miscellaneous";
			this.menuItemMisc.Click += new System.EventHandler(this.menuItemMisc_Click);
			// 
			// menuItemModules
			// 
			this.menuItemModules.Index = 19;
			this.menuItemModules.Text = "Module Preferences";
			this.menuItemModules.Click += new System.EventHandler(this.menuItemModules_Click);
			// 
			// menuItemPractice
			// 
			this.menuItemPractice.Index = 20;
			this.menuItemPractice.Text = "Practice";
			this.menuItemPractice.Click += new System.EventHandler(this.menuItemPractice_Click);
			// 
			// menuItemLinks
			// 
			this.menuItemLinks.Index = 21;
			this.menuItemLinks.Text = "Program Links";
			this.menuItemLinks.Click += new System.EventHandler(this.menuItemLinks_Click);
			// 
			// menuItemRequirementsNeeded
			// 
			this.menuItemRequirementsNeeded.Index = 22;
			this.menuItemRequirementsNeeded.Text = "Requirements Needed";
			this.menuItemRequirementsNeeded.Click += new System.EventHandler(this.menuItemRequirementsNeeded_Click);
			// 
			// menuItemSched
			// 
			this.menuItemSched.Index = 23;
			this.menuItemSched.Text = "Schedules";
			this.menuItemSched.Click += new System.EventHandler(this.menuItemSched_Click);
			// 
			// menuItemSecurity
			// 
			this.menuItemSecurity.Index = 24;
			this.menuItemSecurity.Text = "Security";
			this.menuItemSecurity.Click += new System.EventHandler(this.menuItemSecurity_Click);
			// 
			// menuItemSheets
			// 
			this.menuItemSheets.Index = 25;
			this.menuItemSheets.Text = "Sheets";
			this.menuItemSheets.Click += new System.EventHandler(this.menuItemSheets_Click);
			// 
			// menuItemSpellCheck
			// 
			this.menuItemSpellCheck.Index = 26;
			this.menuItemSpellCheck.Text = "Spell Check";
			this.menuItemSpellCheck.Click += new System.EventHandler(this.menuItemSpellCheck_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 27;
			this.menuItem20.Text = "-";
			// 
			// menuItemObsolete
			// 
			this.menuItemObsolete.Index = 28;
			this.menuItemObsolete.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLetters,
            this.menuItemQuestions});
			this.menuItemObsolete.Text = "Obsolete";
			// 
			// menuItemLetters
			// 
			this.menuItemLetters.Index = 0;
			this.menuItemLetters.Text = "Letters";
			this.menuItemLetters.Click += new System.EventHandler(this.menuItemLetters_Click);
			// 
			// menuItemQuestions
			// 
			this.menuItemQuestions.Index = 1;
			this.menuItemQuestions.Text = "Questionnaire";
			this.menuItemQuestions.Click += new System.EventHandler(this.menuItemQuestions_Click);
			// 
			// menuItemLists
			// 
			this.menuItemLists.Index = 3;
			this.menuItemLists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemProcCodes,
            this.menuItem5,
            this.menuItemAllergies,
            this.menuItemClinics,
            this.menuItemContacts,
            this.menuItemCounties,
            this.menuItemSchoolClass,
            this.menuItemSchoolCourses,
            this.menuItemEmployees,
            this.menuItemEmployers,
            this.menuItemCarriers,
            this.menuItemInsPlans,
            this.menuItemLabCases,
            this.menuItemMedications,
            this.menuItemPharmacies,
            this.menuItemProblems,
            this.menuItemProviders,
            this.menuItemPrescriptions,
            this.menuItemReferrals,
            this.menuItemSchools,
            this.menuItemZipCodes});
			this.menuItemLists.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.menuItemLists.Text = "&Lists";
			// 
			// menuItemProcCodes
			// 
			this.menuItemProcCodes.Index = 0;
			this.menuItemProcCodes.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.menuItemProcCodes.Text = "&Procedure Codes";
			this.menuItemProcCodes.Click += new System.EventHandler(this.menuItemProcCodes_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuItemAllergies
			// 
			this.menuItemAllergies.Index = 2;
			this.menuItemAllergies.Text = "Allergies";
			this.menuItemAllergies.Click += new System.EventHandler(this.menuItemAllergies_Click);
			// 
			// menuItemClinics
			// 
			this.menuItemClinics.Index = 3;
			this.menuItemClinics.Text = "Clinics";
			this.menuItemClinics.Click += new System.EventHandler(this.menuItemClinics_Click);
			// 
			// menuItemContacts
			// 
			this.menuItemContacts.Index = 4;
			this.menuItemContacts.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
			this.menuItemContacts.Text = "&Contacts";
			this.menuItemContacts.Click += new System.EventHandler(this.menuItemContacts_Click);
			// 
			// menuItemCounties
			// 
			this.menuItemCounties.Index = 5;
			this.menuItemCounties.Text = "Counties";
			this.menuItemCounties.Click += new System.EventHandler(this.menuItemCounties_Click);
			// 
			// menuItemSchoolClass
			// 
			this.menuItemSchoolClass.Index = 6;
			this.menuItemSchoolClass.Text = "Dental School Classes";
			this.menuItemSchoolClass.Click += new System.EventHandler(this.menuItemSchoolClass_Click);
			// 
			// menuItemSchoolCourses
			// 
			this.menuItemSchoolCourses.Index = 7;
			this.menuItemSchoolCourses.Text = "Dental School Courses";
			this.menuItemSchoolCourses.Click += new System.EventHandler(this.menuItemSchoolCourses_Click);
			// 
			// menuItemEmployees
			// 
			this.menuItemEmployees.Index = 8;
			this.menuItemEmployees.Text = "&Employees";
			this.menuItemEmployees.Click += new System.EventHandler(this.menuItemEmployees_Click);
			// 
			// menuItemEmployers
			// 
			this.menuItemEmployers.Index = 9;
			this.menuItemEmployers.Text = "Employers";
			this.menuItemEmployers.Click += new System.EventHandler(this.menuItemEmployers_Click);
			// 
			// menuItemCarriers
			// 
			this.menuItemCarriers.Index = 10;
			this.menuItemCarriers.Text = "Insurance Carriers";
			this.menuItemCarriers.Click += new System.EventHandler(this.menuItemCarriers_Click);
			// 
			// menuItemInsPlans
			// 
			this.menuItemInsPlans.Index = 11;
			this.menuItemInsPlans.Text = "&Insurance Plans";
			this.menuItemInsPlans.Click += new System.EventHandler(this.menuItemInsPlans_Click);
			// 
			// menuItemLabCases
			// 
			this.menuItemLabCases.Index = 12;
			this.menuItemLabCases.Text = "Lab Cases";
			this.menuItemLabCases.Click += new System.EventHandler(this.menuItemLabCases_Click);
			// 
			// menuItemMedications
			// 
			this.menuItemMedications.Index = 13;
			this.menuItemMedications.Text = "&Medications";
			this.menuItemMedications.Click += new System.EventHandler(this.menuItemMedications_Click);
			// 
			// menuItemPharmacies
			// 
			this.menuItemPharmacies.Index = 14;
			this.menuItemPharmacies.Text = "Pharmacies";
			this.menuItemPharmacies.Click += new System.EventHandler(this.menuItemPharmacies_Click);
			// 
			// menuItemProblems
			// 
			this.menuItemProblems.Index = 15;
			this.menuItemProblems.Text = "Problems";
			this.menuItemProblems.Click += new System.EventHandler(this.menuItemProblems_Click);
			// 
			// menuItemProviders
			// 
			this.menuItemProviders.Index = 16;
			this.menuItemProviders.Text = "Providers";
			this.menuItemProviders.Click += new System.EventHandler(this.menuItemProviders_Click);
			// 
			// menuItemPrescriptions
			// 
			this.menuItemPrescriptions.Index = 17;
			this.menuItemPrescriptions.Text = "Pre&scriptions";
			this.menuItemPrescriptions.Click += new System.EventHandler(this.menuItemPrescriptions_Click);
			// 
			// menuItemReferrals
			// 
			this.menuItemReferrals.Index = 18;
			this.menuItemReferrals.Text = "&Referrals";
			this.menuItemReferrals.Click += new System.EventHandler(this.menuItemReferrals_Click);
			// 
			// menuItemSchools
			// 
			this.menuItemSchools.Index = 19;
			this.menuItemSchools.Text = "Sites";
			this.menuItemSchools.Click += new System.EventHandler(this.menuItemSites_Click);
			// 
			// menuItemZipCodes
			// 
			this.menuItemZipCodes.Index = 20;
			this.menuItemZipCodes.Text = "&Zip Codes";
			this.menuItemZipCodes.Click += new System.EventHandler(this.menuItemZipCodes_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 4;
			this.menuItemReports.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.menuItemReports.Text = "&Reports";
			this.menuItemReports.Click += new System.EventHandler(this.menuItemReports_Click);
			// 
			// menuItemCustomReports
			// 
			this.menuItemCustomReports.Index = 5;
			this.menuItemCustomReports.Text = "Custom Reports";
			// 
			// menuItemTools
			// 
			this.menuItemTools.Index = 6;
			this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPrintScreen,
            this.menuItem1,
            this.menuItem9,
            this.menuItemAging,
            this.menuItemAuditTrail,
            this.menuItemFinanceCharge,
            this.menuItemCCRecurring,
            this.menuItemCustomerManage,
            this.menuItemDatabaseMaintenance,
            this.menuItemDispensary,
            this.menuItemEvaluations,
            this.menuItemTerminal,
            this.menuItemTerminalManager,
            this.menuItemTranslation,
            this.menuItemMobileSetup,
            this.menuItemNewCropBilling,
            this.menuItemScreening,
            this.menuItemRepeatingCharges,
            this.menuItemResellers,
            this.menuItemReqStudents,
            this.menuItemWebForms,
            this.menuItemWiki});
			this.menuItemTools.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.menuItemTools.Text = "&Tools";
			// 
			// menuItemPrintScreen
			// 
			this.menuItemPrintScreen.Index = 0;
			this.menuItemPrintScreen.Text = "&Print Screen Tool";
			this.menuItemPrintScreen.Click += new System.EventHandler(this.menuItemPrintScreen_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDuplicateBlockouts,
            this.menuItemCreateAtoZFolders,
            this.menuItemImportXML,
            this.menuItemMergePatients,
            this.menuItemProcLockTool,
            this.menuItemShutdown,
            this.menuTelephone,
            this.menuItemTestLatency,
            this.menuItemXChargeReconcile});
			this.menuItem1.Text = "Misc Tools";
			// 
			// menuItemDuplicateBlockouts
			// 
			this.menuItemDuplicateBlockouts.Index = 0;
			this.menuItemDuplicateBlockouts.Text = "Clear Duplicate Blockouts";
			this.menuItemDuplicateBlockouts.Click += new System.EventHandler(this.menuItemDuplicateBlockouts_Click);
			// 
			// menuItemCreateAtoZFolders
			// 
			this.menuItemCreateAtoZFolders.Index = 1;
			this.menuItemCreateAtoZFolders.Text = "Create A to Z Folders";
			this.menuItemCreateAtoZFolders.Click += new System.EventHandler(this.menuItemCreateAtoZFolders_Click);
			// 
			// menuItemImportXML
			// 
			this.menuItemImportXML.Index = 2;
			this.menuItemImportXML.Text = "Import Patient XML";
			this.menuItemImportXML.Click += new System.EventHandler(this.menuItemImportXML_Click);
			// 
			// menuItemMergePatients
			// 
			this.menuItemMergePatients.Index = 3;
			this.menuItemMergePatients.Text = "Merge Patients";
			this.menuItemMergePatients.Click += new System.EventHandler(this.menuItemMergePatients_Click);
			// 
			// menuItemProcLockTool
			// 
			this.menuItemProcLockTool.Index = 4;
			this.menuItemProcLockTool.Text = "Procedure Lock Tool";
			this.menuItemProcLockTool.Click += new System.EventHandler(this.menuItemProcLockTool_Click);
			// 
			// menuItemShutdown
			// 
			this.menuItemShutdown.Index = 5;
			this.menuItemShutdown.Text = "Shutdown All Workstations";
			this.menuItemShutdown.Click += new System.EventHandler(this.menuItemShutdown_Click);
			// 
			// menuTelephone
			// 
			this.menuTelephone.Index = 6;
			this.menuTelephone.Text = "Telephone Numbers";
			this.menuTelephone.Click += new System.EventHandler(this.menuTelephone_Click);
			// 
			// menuItemTestLatency
			// 
			this.menuItemTestLatency.Index = 7;
			this.menuItemTestLatency.Text = "Test Latency";
			this.menuItemTestLatency.Click += new System.EventHandler(this.menuItemTestLatency_Click);
			// 
			// menuItemXChargeReconcile
			// 
			this.menuItemXChargeReconcile.Index = 8;
			this.menuItemXChargeReconcile.Text = "X-Charge Reconcile";
			this.menuItemXChargeReconcile.Visible = false;
			this.menuItemXChargeReconcile.Click += new System.EventHandler(this.menuItemXChargeReconcile_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "-";
			// 
			// menuItemAging
			// 
			this.menuItemAging.Index = 3;
			this.menuItemAging.Text = "&Aging";
			this.menuItemAging.Click += new System.EventHandler(this.menuItemAging_Click);
			// 
			// menuItemAuditTrail
			// 
			this.menuItemAuditTrail.Index = 4;
			this.menuItemAuditTrail.Text = "Audit Trail";
			this.menuItemAuditTrail.Click += new System.EventHandler(this.menuItemAuditTrail_Click);
			// 
			// menuItemFinanceCharge
			// 
			this.menuItemFinanceCharge.Index = 5;
			this.menuItemFinanceCharge.Text = "Billing/&Finance Charges";
			this.menuItemFinanceCharge.Click += new System.EventHandler(this.menuItemFinanceCharge_Click);
			// 
			// menuItemCCRecurring
			// 
			this.menuItemCCRecurring.Index = 6;
			this.menuItemCCRecurring.Text = "CC Recurring Charges";
			this.menuItemCCRecurring.Click += new System.EventHandler(this.menuItemCCRecurring_Click);
			// 
			// menuItemCustomerManage
			// 
			this.menuItemCustomerManage.Index = 7;
			this.menuItemCustomerManage.Text = "Customer Management";
			this.menuItemCustomerManage.Click += new System.EventHandler(this.menuItemCustomerManage_Click);
			// 
			// menuItemDatabaseMaintenance
			// 
			this.menuItemDatabaseMaintenance.Index = 8;
			this.menuItemDatabaseMaintenance.Text = "Database Maintenance";
			this.menuItemDatabaseMaintenance.Click += new System.EventHandler(this.menuItemDatabaseMaintenance_Click);
			// 
			// menuItemDispensary
			// 
			this.menuItemDispensary.Index = 9;
			this.menuItemDispensary.Text = "Dispensary";
			this.menuItemDispensary.Visible = false;
			this.menuItemDispensary.Click += new System.EventHandler(this.menuItemDispensary_Click);
			// 
			// menuItemEvaluations
			// 
			this.menuItemEvaluations.Index = 10;
			this.menuItemEvaluations.Text = "Evaluations";
			this.menuItemEvaluations.Click += new System.EventHandler(this.menuItemEvaluations_Click);
			// 
			// menuItemTerminal
			// 
			this.menuItemTerminal.Index = 11;
			this.menuItemTerminal.Text = "Kiosk";
			this.menuItemTerminal.Click += new System.EventHandler(this.menuItemTerminal_Click);
			// 
			// menuItemTerminalManager
			// 
			this.menuItemTerminalManager.Index = 12;
			this.menuItemTerminalManager.Text = "Kiosk Manager";
			this.menuItemTerminalManager.Click += new System.EventHandler(this.menuItemTerminalManager_Click);
			// 
			// menuItemTranslation
			// 
			this.menuItemTranslation.Index = 13;
			this.menuItemTranslation.Text = "Language Translation";
			this.menuItemTranslation.Click += new System.EventHandler(this.menuItemTranslation_Click);
			// 
			// menuItemMobileSetup
			// 
			this.menuItemMobileSetup.Index = 14;
			this.menuItemMobileSetup.Text = "Mobile Synch";
			this.menuItemMobileSetup.Click += new System.EventHandler(this.menuItemMobileSetup_Click);
			// 
			// menuItemNewCropBilling
			// 
			this.menuItemNewCropBilling.Index = 15;
			this.menuItemNewCropBilling.Text = "NewCrop Billing";
			this.menuItemNewCropBilling.Click += new System.EventHandler(this.menuItemNewCropBilling_Click);
			// 
			// menuItemScreening
			// 
			this.menuItemScreening.Index = 16;
			this.menuItemScreening.Text = "Public Health Screening";
			this.menuItemScreening.Click += new System.EventHandler(this.menuItemScreening_Click);
			// 
			// menuItemRepeatingCharges
			// 
			this.menuItemRepeatingCharges.Index = 17;
			this.menuItemRepeatingCharges.Text = "Repeating Charges";
			this.menuItemRepeatingCharges.Click += new System.EventHandler(this.menuItemRepeatingCharges_Click);
			// 
			// menuItemResellers
			// 
			this.menuItemResellers.Index = 18;
			this.menuItemResellers.Text = "Resellers";
			this.menuItemResellers.Visible = false;
			this.menuItemResellers.Click += new System.EventHandler(this.menuItemResellers_Click);
			// 
			// menuItemReqStudents
			// 
			this.menuItemReqStudents.Index = 19;
			this.menuItemReqStudents.Text = "Student Requirements";
			this.menuItemReqStudents.Click += new System.EventHandler(this.menuItemReqStudents_Click);
			// 
			// menuItemWebForms
			// 
			this.menuItemWebForms.Index = 20;
			this.menuItemWebForms.Text = "WebForms";
			this.menuItemWebForms.Click += new System.EventHandler(this.menuItemWebForms_Click);
			// 
			// menuItemWiki
			// 
			this.menuItemWiki.Index = 21;
			this.menuItemWiki.Text = "Wiki";
			this.menuItemWiki.Click += new System.EventHandler(this.menuItemWiki_Click);
			// 
			// menuClinics
			// 
			this.menuClinics.Index = 7;
			this.menuClinics.Text = "&Clinics";
			// 
			// menuItemEServices
			// 
			this.menuItemEServices.Index = 8;
			this.menuItemEServices.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemMobileSync,
            this.menuMobileWeb,
            this.menuItemPatientPortal,
            this.menuItemIntegratedTexting,
            this.menuItemWebSched,
            this.menuItem14,
            this.menuItemListenerService});
			this.menuItemEServices.OwnerDraw = true;
			this.menuItemEServices.Text = "eServices";
			this.menuItemEServices.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.menuItemEServices_DrawItem);
			this.menuItemEServices.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.menuItemEServices_MeasureItem);
			// 
			// menuItemMobileSync
			// 
			this.menuItemMobileSync.Index = 0;
			this.menuItemMobileSync.Text = "Mobile Sync";
			this.menuItemMobileSync.Click += new System.EventHandler(this.menuItemMobileSync_Click);
			// 
			// menuMobileWeb
			// 
			this.menuMobileWeb.Index = 1;
			this.menuMobileWeb.Text = "Mobile Web";
			this.menuMobileWeb.Click += new System.EventHandler(this.menuMobileWeb_Click);
			// 
			// menuItemPatientPortal
			// 
			this.menuItemPatientPortal.Index = 2;
			this.menuItemPatientPortal.Text = "Patient Portal";
			this.menuItemPatientPortal.Click += new System.EventHandler(this.menuItemPatientPortal_Click);
			// 
			// menuItemIntegratedTexting
			// 
			this.menuItemIntegratedTexting.Index = 3;
			this.menuItemIntegratedTexting.Text = "Integrated Texting";
			this.menuItemIntegratedTexting.Click += new System.EventHandler(this.menuItemIntegratedTexting_Click);
			// 
			// menuItemWebSched
			// 
			this.menuItemWebSched.Index = 4;
			this.menuItemWebSched.Text = "Web Sched";
			this.menuItemWebSched.Click += new System.EventHandler(this.menuItemWebSched_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 5;
			this.menuItem14.Text = "-";
			// 
			// menuItemListenerService
			// 
			this.menuItemListenerService.Index = 6;
			this.menuItemListenerService.OwnerDraw = true;
			this.menuItemListenerService.Text = "Listener Service";
			this.menuItemListenerService.Click += new System.EventHandler(this.menuItemListenerService_Click);
			this.menuItemListenerService.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.menuItemEServices_DrawItem);
			this.menuItemListenerService.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.menuItemListenerService_MeasureItem);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 9;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRemote,
            this.menuItemHelpWindows,
            this.menuItemHelpContents,
            this.menuItemHelpIndex,
            this.menuItemRemoteSupport,
            this.menuItemRequestFeatures,
            this.menuItemUpdate});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemRemote
			// 
			this.menuItemRemote.Index = 0;
			this.menuItemRemote.Text = "Online Support";
			this.menuItemRemote.Click += new System.EventHandler(this.menuItemRemote_Click);
			// 
			// menuItemHelpWindows
			// 
			this.menuItemHelpWindows.Index = 1;
			this.menuItemHelpWindows.Text = "Local Help-Windows";
			this.menuItemHelpWindows.Click += new System.EventHandler(this.menuItemHelpWindows_Click);
			// 
			// menuItemHelpContents
			// 
			this.menuItemHelpContents.Index = 2;
			this.menuItemHelpContents.Text = "Online Help - Contents";
			this.menuItemHelpContents.Click += new System.EventHandler(this.menuItemHelpContents_Click);
			// 
			// menuItemHelpIndex
			// 
			this.menuItemHelpIndex.Index = 3;
			this.menuItemHelpIndex.Shortcut = System.Windows.Forms.Shortcut.ShiftF1;
			this.menuItemHelpIndex.Text = "Online Help - Index";
			this.menuItemHelpIndex.Click += new System.EventHandler(this.menuItemHelpIndex_Click);
			// 
			// menuItemRemoteSupport
			// 
			this.menuItemRemoteSupport.Index = 4;
			this.menuItemRemoteSupport.Text = "Remote Support with Code";
			this.menuItemRemoteSupport.Click += new System.EventHandler(this.menuItemRemoteSupport_Click);
			// 
			// menuItemRequestFeatures
			// 
			this.menuItemRequestFeatures.Index = 5;
			this.menuItemRequestFeatures.Text = "Request Features";
			this.menuItemRequestFeatures.Click += new System.EventHandler(this.menuItemRequestFeatures_Click);
			// 
			// menuItemUpdate
			// 
			this.menuItemUpdate.Index = 6;
			this.menuItemUpdate.Text = "&Update";
			this.menuItemUpdate.Click += new System.EventHandler(this.menuItemUpdate_Click);
			// 
			// imageList32
			// 
			this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
			this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList32.Images.SetKeyName(0, "Appt32.gif");
			this.imageList32.Images.SetKeyName(1, "Family32b.gif");
			this.imageList32.Images.SetKeyName(2, "Account32b.gif");
			this.imageList32.Images.SetKeyName(3, "TreatPlan3D.gif");
			this.imageList32.Images.SetKeyName(4, "chart32.gif");
			this.imageList32.Images.SetKeyName(5, "Images32.gif");
			this.imageList32.Images.SetKeyName(6, "Manage32.gif");
			// 
			// timerSignals
			// 
			this.timerSignals.Tick += new System.EventHandler(this.timerSignals_Tick);
			// 
			// panelSplitter
			// 
			this.panelSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSplitter.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.panelSplitter.Location = new System.Drawing.Point(71, 542);
			this.panelSplitter.Name = "panelSplitter";
			this.panelSplitter.Size = new System.Drawing.Size(769, 7);
			this.panelSplitter.TabIndex = 50;
			this.panelSplitter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseDown);
			this.panelSplitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseMove);
			this.panelSplitter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseUp);
			// 
			// menuSplitter
			// 
			this.menuSplitter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDockBottom,
            this.menuItemDockRight});
			// 
			// menuItemDockBottom
			// 
			this.menuItemDockBottom.Index = 0;
			this.menuItemDockBottom.Text = "Dock to Bottom";
			this.menuItemDockBottom.Click += new System.EventHandler(this.menuItemDockBottom_Click);
			// 
			// menuItemDockRight
			// 
			this.menuItemDockRight.Index = 1;
			this.menuItemDockRight.Text = "Dock to Right";
			this.menuItemDockRight.Click += new System.EventHandler(this.menuItemDockRight_Click);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Pat.gif");
			this.imageListMain.Images.SetKeyName(1, "commlog.gif");
			this.imageListMain.Images.SetKeyName(2, "email.gif");
			this.imageListMain.Images.SetKeyName(3, "tasksNicer.gif");
			this.imageListMain.Images.SetKeyName(4, "label.gif");
			this.imageListMain.Images.SetKeyName(5, "Text.gif");
			// 
			// menuPatient
			// 
			this.menuPatient.Popup += new System.EventHandler(this.menuPatient_Popup);
			// 
			// menuLabel
			// 
			this.menuLabel.Popup += new System.EventHandler(this.menuLabel_Popup);
			// 
			// menuEmail
			// 
			this.menuEmail.Popup += new System.EventHandler(this.menuEmail_Popup);
			// 
			// menuLetter
			// 
			this.menuLetter.Popup += new System.EventHandler(this.menuLetter_Popup);
			// 
			// timerDisabledKey
			// 
			this.timerDisabledKey.Enabled = true;
			this.timerDisabledKey.Interval = 600000;
			this.timerDisabledKey.Tick += new System.EventHandler(this.timerDisabledKey_Tick);
			// 
			// timerHeartBeat
			// 
			this.timerHeartBeat.Enabled = true;
			this.timerHeartBeat.Interval = 180000;
			this.timerHeartBeat.Tick += new System.EventHandler(this.timerHeartBeat_Tick);
			// 
			// timerWebHostSynch
			// 
			this.timerWebHostSynch.Enabled = true;
			this.timerWebHostSynch.Interval = 30000;
			this.timerWebHostSynch.Tick += new System.EventHandler(this.timerWebHostSynch_Tick);
			// 
			// timerLogoff
			// 
			this.timerLogoff.Interval = 15000;
			this.timerLogoff.Tick += new System.EventHandler(this.timerLogoff_Tick);
			// 
			// timerReplicationMonitor
			// 
			this.timerReplicationMonitor.Interval = 10000;
			this.timerReplicationMonitor.Tick += new System.EventHandler(this.timerReplicationMonitor_Tick);
			// 
			// panelPhoneSmall
			// 
			this.panelPhoneSmall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panelPhoneSmall.Controls.Add(this.labelFieldType);
			this.panelPhoneSmall.Controls.Add(this.comboTriageCoordinator);
			this.panelPhoneSmall.Controls.Add(this.labelMsg);
			this.panelPhoneSmall.Controls.Add(this.butMapPhones);
			this.panelPhoneSmall.Controls.Add(this.butTriage);
			this.panelPhoneSmall.Controls.Add(this.butBigPhones);
			this.panelPhoneSmall.Controls.Add(this.labelWaitTime);
			this.panelPhoneSmall.Controls.Add(this.labelTriage);
			this.panelPhoneSmall.Location = new System.Drawing.Point(71, 333);
			this.panelPhoneSmall.Name = "panelPhoneSmall";
			this.panelPhoneSmall.Size = new System.Drawing.Size(173, 216);
			this.panelPhoneSmall.TabIndex = 56;
			// 
			// labelFieldType
			// 
			this.labelFieldType.Location = new System.Drawing.Point(4, 25);
			this.labelFieldType.Name = "labelFieldType";
			this.labelFieldType.Size = new System.Drawing.Size(143, 15);
			this.labelFieldType.TabIndex = 88;
			this.labelFieldType.Text = "Triage Coordinator";
			this.labelFieldType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboTriageCoordinator
			// 
			this.comboTriageCoordinator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTriageCoordinator.FormattingEnabled = true;
			this.comboTriageCoordinator.Location = new System.Drawing.Point(0, 42);
			this.comboTriageCoordinator.MaxDropDownItems = 10;
			this.comboTriageCoordinator.Name = "comboTriageCoordinator";
			this.comboTriageCoordinator.Size = new System.Drawing.Size(173, 21);
			this.comboTriageCoordinator.TabIndex = 87;
			this.comboTriageCoordinator.SelectionChangeCommitted += new System.EventHandler(this.comboTriageCoordinator_SelectionChangeCommitted);
			// 
			// labelMsg
			// 
			this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMsg.ForeColor = System.Drawing.Color.Firebrick;
			this.labelMsg.Location = new System.Drawing.Point(-1, 2);
			this.labelMsg.Name = "labelMsg";
			this.labelMsg.Size = new System.Drawing.Size(35, 20);
			this.labelMsg.TabIndex = 53;
			this.labelMsg.Text = "V:00";
			this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butMapPhones
			// 
			this.butMapPhones.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMapPhones.Autosize = true;
			this.butMapPhones.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMapPhones.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMapPhones.CornerRadius = 4F;
			this.butMapPhones.Location = new System.Drawing.Point(154, 0);
			this.butMapPhones.Name = "butMapPhones";
			this.butMapPhones.Size = new System.Drawing.Size(18, 24);
			this.butMapPhones.TabIndex = 54;
			this.butMapPhones.Text = "P";
			this.butMapPhones.Click += new System.EventHandler(this.butMapPhones_Click);
			// 
			// butTriage
			// 
			this.butTriage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTriage.Autosize = true;
			this.butTriage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTriage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTriage.CornerRadius = 4F;
			this.butTriage.Location = new System.Drawing.Point(116, 0);
			this.butTriage.Name = "butTriage";
			this.butTriage.Size = new System.Drawing.Size(18, 24);
			this.butTriage.TabIndex = 52;
			this.butTriage.Text = "T";
			this.butTriage.Click += new System.EventHandler(this.butTriage_Click);
			// 
			// butBigPhones
			// 
			this.butBigPhones.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBigPhones.Autosize = true;
			this.butBigPhones.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBigPhones.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBigPhones.CornerRadius = 4F;
			this.butBigPhones.Location = new System.Drawing.Point(135, 0);
			this.butBigPhones.Name = "butBigPhones";
			this.butBigPhones.Size = new System.Drawing.Size(18, 24);
			this.butBigPhones.TabIndex = 52;
			this.butBigPhones.Text = "B";
			this.butBigPhones.Click += new System.EventHandler(this.butBigPhones_Click);
			// 
			// labelWaitTime
			// 
			this.labelWaitTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWaitTime.ForeColor = System.Drawing.Color.Black;
			this.labelWaitTime.Location = new System.Drawing.Point(89, 2);
			this.labelWaitTime.Name = "labelWaitTime";
			this.labelWaitTime.Size = new System.Drawing.Size(30, 20);
			this.labelWaitTime.TabIndex = 53;
			this.labelWaitTime.Text = "00m";
			this.labelWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTriage
			// 
			this.labelTriage.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriage.ForeColor = System.Drawing.Color.Black;
			this.labelTriage.Location = new System.Drawing.Point(41, 2);
			this.labelTriage.Name = "labelTriage";
			this.labelTriage.Size = new System.Drawing.Size(41, 20);
			this.labelTriage.TabIndex = 53;
			this.labelTriage.Text = "T:000";
			this.labelTriage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// menuText
			// 
			this.menuText.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTextMessagesAll,
            this.menuItemTextMessagesReceived,
            this.menuItemTextMessagesSent});
			// 
			// menuItemTextMessagesAll
			// 
			this.menuItemTextMessagesAll.Index = 0;
			this.menuItemTextMessagesAll.Text = "Text Messages All";
			this.menuItemTextMessagesAll.Click += new System.EventHandler(this.menuItemTextMessagesAll_Click);
			// 
			// menuItemTextMessagesReceived
			// 
			this.menuItemTextMessagesReceived.Index = 1;
			this.menuItemTextMessagesReceived.Text = "Text Messages Received";
			this.menuItemTextMessagesReceived.Click += new System.EventHandler(this.menuItemTextMessagesReceived_Click);
			// 
			// menuItemTextMessagesSent
			// 
			this.menuItemTextMessagesSent.Index = 2;
			this.menuItemTextMessagesSent.Text = "Text Messages Sent";
			this.menuItemTextMessagesSent.Click += new System.EventHandler(this.menuItemTextMessagesSent_Click);
			// 
			// lightSignalGrid1
			// 
			this.lightSignalGrid1.Location = new System.Drawing.Point(0, 463);
			this.lightSignalGrid1.Name = "lightSignalGrid1";
			this.lightSignalGrid1.Size = new System.Drawing.Size(50, 206);
			this.lightSignalGrid1.TabIndex = 20;
			this.lightSignalGrid1.Text = "lightSignalGrid1";
			this.lightSignalGrid1.ButtonClick += new OpenDental.UI.ODLightSignalGridClickEventHandler(this.lightSignalGrid1_ButtonClick);
			// 
			// FormOpenDental
			// 
			this.ClientSize = new System.Drawing.Size(982, 445);
			this.Controls.Add(this.panelPhoneSmall);
			this.Controls.Add(this.panelSplitter);
			this.Controls.Add(this.lightSignalGrid1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Menu = this.mainMenu;
			this.Name = "FormOpenDental";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Open Dental";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOpenDental_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOpenDental_FormClosed);
			this.Load += new System.EventHandler(this.FormOpenDental_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormOpenDental_KeyDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormOpenDental_MouseMove);
			this.Resize += new System.EventHandler(this.FormOpenDental_Resize);
			this.panelPhoneSmall.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private void FormOpenDental_Load(object sender,System.EventArgs e) {
			_isStartingUp=true;//Halts mobile synch during updates.
			Splash.Dispose();
			allNeutral();
			string odUser="";
			string odPassHash="";
			string webServiceUri="";
			YN webServiceIsEcw=YN.Unknown;
			bool isSilentUpdate=false;
			string odPassword="";
			string serverName="";
			string databaseName="";
			string mySqlUser="";
			string mySqlPassword="";
			if(CommandLineArgs.Length!=0) {
				for(int i=0;i<CommandLineArgs.Length;i++) {
					if(CommandLineArgs[i].StartsWith("UserName=") && CommandLineArgs[i].Length>9) {
						odUser=CommandLineArgs[i].Substring(9).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("PassHash=") && CommandLineArgs[i].Length>9) {
						odPassHash=CommandLineArgs[i].Substring(9).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("WebServiceUri=") && CommandLineArgs[i].Length>14) {
						webServiceUri=CommandLineArgs[i].Substring(14).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("WebServiceIsEcw=") && CommandLineArgs[i].Length>16) {
						if(CommandLineArgs[i].Substring(16).Trim('"')=="True") {
							webServiceIsEcw=YN.Yes;
						}
						else {
							webServiceIsEcw=YN.No;
						}
					}
					if(CommandLineArgs[i].StartsWith("OdPassword=") && CommandLineArgs[i].Length>11) {
						odPassword=CommandLineArgs[i].Substring(11).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("ServerName=") && CommandLineArgs[i].Length>11) {
						serverName=CommandLineArgs[i].Substring(11).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("DatabaseName=") && CommandLineArgs[i].Length>13) {
						databaseName=CommandLineArgs[i].Substring(13).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("MySqlUser=") && CommandLineArgs[i].Length>10) {
						mySqlUser=CommandLineArgs[i].Substring(10).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("MySqlPassword=") && CommandLineArgs[i].Length>14) {
						mySqlPassword=CommandLineArgs[i].Substring(14).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("IsSilentUpdate=") && CommandLineArgs[i].Length>15) {
						if(CommandLineArgs[i].Substring(15).Trim('"').ToLower()=="true") {
							isSilentUpdate=true;
						}
						else {
							isSilentUpdate=false;
						}
					}
				}
			}
			YN noShow=YN.Unknown;
			if(webServiceUri!="") {//a web service was specified
				if(odUser!="" && odPassword!="") {//and both a username and password were specified
					noShow=YN.Yes;
				}
			}
			else if(databaseName!="") {
				noShow=YN.Yes;
			}
			//Users that want to silently update MUST pass in the following command line args.
			if(isSilentUpdate && (odUser.Trim()==""
					|| (odPassword.Trim()=="" && odPassHash.Trim()=="")
					|| serverName.Trim()==""
					|| databaseName.Trim()==""
					|| mySqlUser.Trim()==""
					|| mySqlPassword.Trim()=="")) 
			{
				ExitCode=104;//Required command line arguments have not been set for silent updating
				Application.Exit();
				return;
			}
			Version versionOd=Assembly.GetAssembly(typeof(FormOpenDental)).GetName().Version;
			Version versionObBus=Assembly.GetAssembly(typeof(Db)).GetName().Version;
			if(versionOd!=versionObBus) {
				if(isSilentUpdate) {
					ExitCode=105;//File versions do not match
				}
				else {//Not a silent update.  Show a warning message.
					//No MsgBox or Lan.g() here, because we don't want to access the database if there is a version conflict.
					MessageBox.Show("Mismatched program file versions. Please run the Open Dental setup file again on this computer.");
				}
				Application.Exit();
				return;
			}
			FormChooseDatabase formChooseDb=new FormChooseDatabase();
			formChooseDb.OdUser=odUser;
			formChooseDb.OdPassHash=odPassHash;
			formChooseDb.WebServiceUri=webServiceUri;
			formChooseDb.WebServiceIsEcw=webServiceIsEcw;
			formChooseDb.OdPassword=odPassword;
			formChooseDb.ServerName=serverName;
			formChooseDb.DatabaseName=databaseName;
			formChooseDb.MySqlUser=mySqlUser;
			formChooseDb.MySqlPassword=mySqlPassword;
			formChooseDb.NoShow=noShow;//either unknown or yes
			formChooseDb.GetConfig();
			formChooseDb.GetCmdLine();
			while(true) {//Most users will loop through once.  If user tries to connect to a db with replication failure, they will loop through again.
				if(formChooseDb.NoShow==YN.Yes) {
					if(!formChooseDb.TryToConnect()) {
						if(isSilentUpdate) {
							ExitCode=106;//Connection to specified database has failed
							Application.Exit();
							return;
						}
						formChooseDb.ShowDialog();
						if(formChooseDb.DialogResult==DialogResult.Cancel) {
							Application.Exit();
							return;
						}
					}
				}
				else {
					formChooseDb.ShowDialog();
					if(formChooseDb.DialogResult==DialogResult.Cancel) {
						Application.Exit();
						return;
					}
				}
				Cursor=Cursors.WaitCursor;
				//theme
				try {
					ODToolBar.UseBlueTheme=PrefC.GetBool(PrefName.ColorTheme);
					ODGrid.UseBlueTheme=PrefC.GetBool(PrefName.ColorTheme);
				}
				catch {
					//try/catch in case you are trying to convert from an older version of OD and need to update the DB.
				}
				this.Invalidate();//apply them at next repaint.
				Plugins.LoadAllPlugins(this);//moved up from near RefreshLocalData(invalidTypes). New position might cause problems.
				Splash=new FormSplash();
				if(CommandLineArgs.Length==0) {
					Splash.Show();
				}
				if(!PrefsStartup(isSilentUpdate)) {//looks for the AtoZ folder here, but would like to eventually move that down to after login.  In Release, refreshes the Pref cache if conversion successful.
					Cursor=Cursors.Default;
					Splash.Dispose();
					if(ExitCode==0) {
						//PrefsStartup failed and ExitCode is still 0 which means an unexpected error must have occured.
						//Set the exit code to 999 which will represent an Unknown Error
						ExitCode=999;
					}
					Application.Exit();
					return;
				}
				if(isSilentUpdate) {
					//The db was successfully updated so there is nothing else that needs to be done after this point.
					Application.Exit();//Exits with ExitCode=0
					return;
				}
				if(ReplicationServers.Server_id!=0 && ReplicationServers.Server_id==PrefC.GetLong(PrefName.ReplicationFailureAtServer_id)) {
					MsgBox.Show(this,"This database is temporarily unavailable.  Please connect instead to your alternate database at the other location.");
					formChooseDb.NoShow=YN.No;//This ensures they will get a choose db window next time through the loop.
					ReplicationServers.Server_id=-1;
					continue;
				}
				break;
			}
			if(Programs.UsingEcwTightOrFullMode()) {
				Splash.Dispose();//We don't show splash screen when bridging to eCW.
			}
			RefreshLocalData(InvalidType.Prefs);//should only refresh preferences so that SignalLastClearedDate preference can be used in ClearOldSignals()
			Signalods.ClearOldSignals();
			//We no longer do this shotgun approach because it can slow the loading time.
			//RefreshLocalData(InvalidType.AllLocal);
			List<InvalidType> invalidTypes=new List<InvalidType>();
			//invalidTypes.Add(InvalidType.Prefs);//Preferences were refreshed above.  The only preference which might be stale is SignalLastClearedDate, but it is not used anywhere after calling ClearOldSignals() above.
			invalidTypes.Add(InvalidType.Defs);
			invalidTypes.Add(InvalidType.Providers);//obviously heavily used
			invalidTypes.Add(InvalidType.Programs);//already done above, but needs to be done explicitly to trigger the PostCleanup 
			invalidTypes.Add(InvalidType.ToolBut);//so program buttons will show in all the toolbars
			if(Programs.UsingEcwTightMode()) {
				lightSignalGrid1.Visible=false;
			}
			else{
				invalidTypes.Add(InvalidType.Signals);//so when mouse moves over light buttons, it won't crash
			}
			//Plugins.LoadAllPlugins(this);//moved up from right after optimizing tooth chart graphics.  New position might cause problems.
			//It was moved because RefreshLocalData()=>RefreshLocalDataPostCleanup()=>ContrChart2.InitializeLocalData()=>LayoutToolBar() has a hook.
			//Moved it up again on 10/3/13
			RefreshLocalData(invalidTypes.ToArray());
			FillSignalButtons(null);
			ContrManage2.InitializeOnStartup();//so that when a signal is received, it can handle it.
			//Lan.Refresh();//automatically skips if current culture is en-US
			//LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US
			DataValid.BecameInvalid += new OpenDental.ValidEventHandler(DataValid_BecameInvalid);
			signalLastRefreshed=MiscData.GetNowDateTime();
			if(PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)==0) {
				timerSignals.Enabled=false;
			}
			else {
				timerSignals.Interval=PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)*1000;
				timerSignals.Enabled=true;
			}
			timerTimeIndic.Enabled=true;
			myOutlookBar.Buttons[0].Caption=Lan.g(this,"Appts");
			myOutlookBar.Buttons[1].Caption=Lan.g(this,"Family");
			myOutlookBar.Buttons[2].Caption=Lan.g(this,"Account");
			myOutlookBar.Buttons[3].Caption=Lan.g(this,"Treat' Plan");
			//myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");//??done in RefreshLocalData
			myOutlookBar.Buttons[5].Caption=Lan.g(this,"Images");
			myOutlookBar.Buttons[6].Caption=Lan.g(this,"Manage");
			foreach(MenuItem menuItem in mainMenu.MenuItems){
				TranslateMenuItem(menuItem);
			}
			if(CultureInfo.CurrentCulture.Name=="en-US") {
				//for the business layer, this functionality is duplicated in the Lan class.  Need for SL.
				CultureInfo cInfo=(CultureInfo)CultureInfo.CurrentCulture.Clone();
				cInfo.DateTimeFormat.ShortDatePattern="MM/dd/yyyy";
				Application.CurrentCulture=cInfo;
				//
				//if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName=="en"){
				menuItemTranslation.Visible=false;
			}
			else {//not en-US
				CultureInfo cInfo=(CultureInfo)CultureInfo.CurrentCulture.Clone();
				string dateFormatCur=cInfo.DateTimeFormat.ShortDatePattern;
				//The carrot indicates the beginning of a word.  {2} means that there has to be exactly 2 y's.  [^a-z] means any character except a through z.  $ means end of word.
				if(Regex.IsMatch(dateFormatCur,"^y{2}[^a-z]")
					|| Regex.IsMatch(dateFormatCur,"[^a-z]y{2}$")) 
				{
					//We know there are only two y's in the format.  Force there to be four.
					cInfo.DateTimeFormat.ShortDatePattern=dateFormatCur.Replace("yy","yyyy");
					Application.CurrentCulture=cInfo;
				}
			}
			//For EHR users we want to load up the EHR code list from the obfuscated dll in a background thread because it takes roughly 11 seconds to load up.
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				try {
					Thread threadPreloadEhrCodeList=new Thread((ThreadStart)EhrCodes.UpdateList);//Background loading of the EHR.dll code list.
					threadPreloadEhrCodeList.Start();
				}
				catch { 
					//This should never happen.  It would most likely be due to a corrupt dll issue but I don't want to stop the start up sequence.
					//Users could theoretically use Open Dental for an entire day and never hit the code that utilizes the EhrCodes class.
					//Therefore, we do not want to cause any issues and the worst case scenario is the users has to put up with the 11 second delay (old behavior). 
				}
			}
			if(!File.Exists("Help.chm")){
				menuItemHelpWindows.Visible=false;
			}
			if(Environment.OSVersion.Platform==PlatformID.Unix){//Create A to Z unsupported on Unix for now.
				menuItemCreateAtoZFolders.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.ProcLockingIsAllowed)) {
				menuItemProcLockTool.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.ADAdescriptionsReset)) {
				ProcedureCodes.ResetADAdescriptions();
				Prefs.UpdateBool(PrefName.ADAdescriptionsReset,true);
			}
			#region Start OpenDent Services
			//If this is the designated update server, make sure that all "OpenDent..." services are up and running.
			//This is mainly to increase the odds that critical services required for e-Services are always running.
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" && ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName))) {
				List<ServiceController> listServices=ODEnvironment.GetAllOpenDentServices();
				for(int i=0;i<listServices.Count;i++) {
					//Only attempt to start services that are of status stopped or stop pending.
					//If we do not do this, an InvalidOperationException will throw that says "An instance of the service is already running"
					if(listServices[i].Status!=ServiceControllerStatus.Stopped && listServices[i].Status!=ServiceControllerStatus.StopPending) {
						continue;
					}
					try {
						listServices[i].Start();//We will not wait for the service to start so that loading OD does not take any longer that usual.
					}
					catch {
						//An InvalidOperationException can get thrown if the service could not be started.  E.g. current user is not running Open Dental as an administrator.
						//We do not want to halt the startup sequence here.  If we want to notify customers of a downed service, there needs to be an additional monitoring service installed.
					}
				}
			}
			#endregion
			Splash.Dispose();
			//Choose a default DirectX format when no DirectX format has been specified and running in DirectX tooth chart mode.
			//ComputerPref localComputerPrefs=ComputerPrefs.GetForLocalComputer();
			if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.DirectX && ComputerPrefs.LocalComputer.DirectXFormat=="") {
				//MessageBox.Show(Lan.g(this,"Optimizing tooth chart graphics. This may take a few minutes. You will be notified when the process is complete."));
				ComputerPrefs.LocalComputer.DirectXFormat=FormGraphics.GetPreferredDirectXFormat(this);
				if(ComputerPrefs.LocalComputer.DirectXFormat=="invalid") {
					//No valid local DirectX format could be found.
					//Simply revert to OpenGL.
					ComputerPrefs.LocalComputer.GraphicsSimple=DrawingMode.Simple2D;
				}
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
				ContrChart2.InitializeOnStartup();
				//MsgBox.Show(this,"Done optimizing tooth chart graphics.");
			}
			if(ODEnvironment.IsRunningOnDbServer(MiscData.GetODServer()) && PrefC.GetBool(PrefName.ShowFeatureEhr)) {//OpenDental has EHR enabled and is running on the same machine as the mysql server it is connected to.*/
				_threadTimeSynch=new Thread(new ThreadStart(ThreadTimeSynch_Synch));
				_threadTimeSynch.Start();
			}
			_isStartingUp=false;//Used to allow Mobile Synch to continue
			if(Security.CurUser==null) {//It could already be set if using web service because login from ChooseDatabase window.
				if(Programs.UsingEcwTightOrFullMode() && odUser!="") {//only leave it null if a user was passed in on the commandline.  If starting OD manually, it will jump into the else.
					//leave user as null
				}
				else {
					if(odUser!="" && odPassword!=""){//if a username and password were passed in
						//Userod user=Userods.GetUserByName(odUser,Programs.UsingEcwTight());
						Userod user=Userods.GetUserByName(odUser,Programs.UsingEcwTightOrFullMode());
						if(user!=null){
							if(Userods.CheckTypedPassword(odPassword,user.Password)){//password matches
								Security.CurUser=user.Copy();
								if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
									string pw=odPassword;
									//if(Programs.UsingEcwTight()) {//ecw requires hash, but non-ecw requires actual password
									if(Programs.UsingEcwTightOrFullMode()) {//ecw requires hash, but non-ecw requires actual password
										pw=Userods.EncryptPassword(pw,true);
									}
									Security.PasswordTyped=pw;
								}
								//TasksCheckOnStartup is one thing that doesn't get done because login window wasn't used
							}
						}
					}
					if(Security.CurUser==null) {//normal manual log in process
						Userod adminUser=Userods.GetAdminUser();
						if(adminUser.Password=="") {
							Security.CurUser=adminUser.Copy();
							SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged on.");
						}
						else {
							FormLogOn_=new FormLogOn();
							FormLogOn_.ShowDialog(this);
							if(FormLogOn_.DialogResult==DialogResult.Cancel) {
								Cursor=Cursors.Default;
								Application.Exit();
								return;
							}
						}
					}
				}
			}
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			myOutlookBar.SelectedIndex=Security.GetModule(0);//for eCW, this fails silently.
			//if(Programs.UsingEcwTight()) {
			if(Programs.UsingEcwTightOrFullMode()
				|| (HL7Defs.IsExistingHL7Enabled() && !HL7Defs.GetOneDeepEnabled().ShowAppts)) {
				myOutlookBar.SelectedIndex=4;//Chart module
				//ToolBarMain.Height=0;//this should force the modules further up on the screen
				//ToolBarMain.Visible=false;
				LayoutControls();
			}
			if(Programs.UsingOrion) {
				myOutlookBar.SelectedIndex=1;//Family module
			}
			myOutlookBar.Invalidate();
			LayoutToolBar();
			//If clinics are enabled, we will set the public ClinicNum variable
			//If the user is restricted to a clinic(s), and the computerpref clinic is not one of the user's restricted clinics, the user's clinic will be selected
			//If the user is not restricted, or if the user is restricted but has access to the computerpref clinic, the computerpref clinic will be selected
			//The ClinicNum will determine which view is loaded, either from the computerpref table or from the userodapptview table
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && Security.CurUser!=null) {//If block must be run before SetModuleSelected() so correct clinic filtration occurs.
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				bool isCompClinicAllowed=false;
				for(int i=0;i<listClinics.Count;i++) {
					if(listClinics[i].ClinicNum==ComputerPrefs.LocalComputer.ClinicNum) {
						isCompClinicAllowed=true;
						break;
					}
				}
				if(Security.CurUser.ClinicIsRestricted && !isCompClinicAllowed) {//user is restricted and does not have access to the computerpref clinic
					ClinicNum=Security.CurUser.ClinicNum;
				}
				else {//the user is either not restricted to a clinic(s) or the user is restricted but has access to the computerpref clinic
					ClinicNum=ComputerPrefs.LocalComputer.ClinicNum;
				}
				Clinics.ClinicNum=ClinicNum;
				RefreshMenuClinics();
			}
			Cursor=Cursors.Default;
			if(myOutlookBar.SelectedIndex==-1){
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
			Bridges.Trojan.StartupCheck();
			FormUAppoint.StartThreadIfEnabled();
			Bridges.ICat.StartFileWatcher();
			Bridges.TigerView.StartFileWatcher();
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				menuItemResellers.Visible=true;
				menuItemXChargeReconcile.Visible=true;
				#if !DEBUG
					if(Process.GetProcessesByName("WebCamOD").Length==0) {
						try {
							Process.Start("WebCamOD.exe");
						}
						catch { }//for example, if working from home.
					}
				#endif
				ThreadVM=new Thread(new ThreadStart(this.ThreadVM_SetLabelMsg));
				ThreadVM.Start();//It's done this way because the file activity tends to lock the UI on slow connections.
				//Only run this thread every 1.6 seconds.
				_odThreadHqMetrics=new ODThread(1600,ProcessHqMetrics);
				_odThreadHqMetrics.AddExceptionHandler(new ODThread.ExceptionDelegate(OnThreadHqMetricsException));
				_odThreadHqMetrics.Name="HQ Metrics Thread";
				_odThreadHqMetrics.Start();						
			}
			#if !TRIALONLY
				if(PrefC.GetDate(PrefName.BackupReminderLastDateRun).AddMonths(1)<DateTime.Today) {
					FormBackupReminder FormBR=new FormBackupReminder();
					FormBR.ShowDialog();
					if(FormBR.DialogResult==DialogResult.OK){
						Prefs.UpdateDateT(PrefName.BackupReminderLastDateRun,DateTimeOD.Today);
					}
					else{
						Application.Exit();
						return;
					}
				}
				//==tg  Commented out on 04/24/2014 after discussion deciding it was not necessary to synchronize time on start-up. Timeouts and other errors were causing complaints from customers.
				//if(PrefC.GetBool(PrefName.ShowFeatureEhr) && !_isTimeSynchThreadRunning) {
				//	FormEhrTimeSynch FormEhrTS = new FormEhrTimeSynch();
				//	FormEhrTS.IsAutoLaunch=true;
				//	if(!FormEhrTS.TimesInSynchFast()) {
				//		FormEhrTS.ShowDialog();
				//	}
				//}
			#endif
			FillPatientButton(null);
			//ThreadCommandLine=new Thread(new ThreadStart(Listen));
			//if(!IsSecondInstance) {//can't use a port that's already in use.
			//	//js We can't do this yet.  I tried it already, and it consumes nearly 100% CPU.  Not while testing, but later.
			//	//So until we really need to do it, it's easiest no just not start the thread for now.
			//	//ThreadCommandLine.Start();
			//}
			//if(CommandLineArgs.Length>0) {
			ProcessCommandLine(CommandLineArgs);
			//}
			try {
				Computers.UpdateHeartBeat(Environment.MachineName,true);
			}
			catch { }
			//string dllPathEHR=ODFileUtils.CombinePaths(Application.StartupPath,"EHR.dll");
			//if(PrefC.GetBoolSilent(PrefName.ShowFeatureEhr,false)) {
			//	#if EHRTEST
			//		FormEHR=new FormEHR();
			//		ContrChart2.InitializeLocalData();//because toolbar is now missing the EHR button.  Only a problem if a db conversion is done when opening the program.
			//	#else
			//		ObjFormEhrMeasures=null;
			//		AssemblyEHR=null;
			//		if(File.Exists(dllPathEHR)) {//EHR.dll is available, so load it up
			//			AssemblyEHR=Assembly.LoadFile(dllPathEHR);
			//			Type type=AssemblyEHR.GetType("EHR.FormEHR");//namespace.class
			//			ObjFormEhrMeasures=Activator.CreateInstance(type);
			//		}
			//	#endif
			//}
			Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),ClinicNum);
			dateTimeLastActivity=DateTime.Now;
			timerLogoff.Enabled=true;
			timerReplicationMonitor.Enabled=true;
			ThreadEmailInbox=new Thread(new ThreadStart(ThreadEmailInbox_Receive));
			ThreadEmailInbox.Start();
			_threadPodium=new ODThread(_podiumIntervalMS,ThreadPodiumSendInvitations);
			_threadPodium.AddExceptionHandler(PodiumMonitoringException);
			_threadPodium.Name="Podium Thread";
			_threadPodium.Start();
			StartEServiceMonitoring();
			Patient pat=Patients.GetPat(CurPatNum);
			if(pat!=null && (_showForm=="popup" || _showForm=="popups") && myOutlookBar.SelectedIndex!=-1) {
				FormPopupsForFam FormP=new FormPopupsForFam();
				FormP.PatCur=pat;
				FormP.ShowDialog();
			}
			bool isApptModuleSelected=false;
			if(myOutlookBar.SelectedIndex==0){
				isApptModuleSelected=true;
			}
			if(CurPatNum!=0 && _showForm=="apptsforpatient" && isApptModuleSelected) {
				ContrAppt2.DisplayOtherDlg(false);
			}
			if(!Prefs.IsODHQ()) {
				//Remove the menu items that are only needed for HQ like Default CC Procedures
				menuItemAccount.MenuItems.Clear();
			}
			ContrAppt2.SendSmsClickDelegate=OnTxtMsg_Click;//used in the appointment right click context menu.
			Plugins.HookAddCode(this,"FormOpenDental.Load_end");
		}

		private void comboTriageCoordinator_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboTriageCoordinator.SelectedIndex<0) {
				return;
			}
			Employee employee=Employees.GetEmp((string)comboTriageCoordinator.Items[comboTriageCoordinator.SelectedIndex],new List<Employee>(Employees.ListShort));
			if(employee==null) {
				return;
			}
			if(Prefs.UpdateLong(PrefName.HQTriageCoordinator,employee.EmployeeNum)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private bool PrefsStartup() {
			return PrefsStartup(false);
		}

		///<summary>Returns false if it can't complete a conversion, find datapath, or validate registration key.</summary>
		private bool PrefsStartup(bool isSilentUpdate){
			try {
				Cache.Refresh(InvalidType.Prefs);
			}
			catch(Exception ex) {
				if(isSilentUpdate) {
					ExitCode=100;//Database could not be accessed for cache refresh
					Application.Exit();
					return false;
				}
				MessageBox.Show(ex.Message);
				return false;//shuts program down.
			}
			if(!PrefL.CheckMySqlVersion(isSilentUpdate)){
				return false;
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				try {
					MiscData.SetSqlMode();
				}
				catch {
					if(isSilentUpdate) {
						ExitCode=111;//Global SQL mode could not be set
						Application.Exit();
						return false;
					}
					MessageBox.Show("Unable to set global sql mode.  User probably does not have enough permission.");
					return false;
				}
				string updateComputerName=PrefC.GetStringSilent(PrefName.UpdateInProgressOnComputerName);
				if(updateComputerName != "" && Environment.MachineName != updateComputerName) {
					if(isSilentUpdate) {
						ExitCode=120;//Computer trying to access DB during update
						Application.Exit();
						return false;
					}
					FormUpdateInProgress formUIP=new FormUpdateInProgress(updateComputerName);
					DialogResult result=formUIP.ShowDialog();
					if(result!=DialogResult.OK) {
						return false;//Either the user canceled out of the window or clicked the override button which 
					}
				}
			}
			//if RemotingRole.ClientWeb, version will have already been checked at login, so no danger here.
			//ClientWeb version can be older than this version, but that will be caught in a moment.
			if(isSilentUpdate) {
				if(!PrefL.ConvertDB(true,Application.ProductVersion)) {//refreshes Prefs if converted successfully.
					if(ExitCode==0) {//Unknown error occurred
						ExitCode=200;//Convert Database has failed during execution (Unknown Error)
					}
					Application.Exit();
					return false;
				}
			}
			else {
				if(!PrefL.ConvertDB()) {//refreshes Prefs if converted successfully.
					return false;
				}
			}
			if(!isSilentUpdate) {
				PrefL.MySqlVersion55Remind();
			}
			if(PrefC.AtoZfolderUsed) {
				string prefImagePath=ImageStore.GetPreferredAtoZpath();
				if(prefImagePath==null || !Directory.Exists(prefImagePath)) {//AtoZ folder not found
					if(isSilentUpdate) {
						ExitCode=300;//AtoZ folder not found (Warning)
						Application.Exit();
						return false;
					}
					Cache.Refresh(InvalidType.Security);
					FormPath FormP=new FormPath();
					FormP.IsStartingUp=true;
					FormP.ShowDialog();
					if(FormP.DialogResult!=DialogResult.OK) {
						MsgBox.Show(this,"Invalid A to Z path.  Closing program.");
						Application.Exit();
						return false;
					}
					Cache.Refresh(InvalidType.Prefs);//because listening thread not started yet.
				}
			}
			if(!PrefL.CheckProgramVersion(isSilentUpdate)){
				return false;
			}
			if(!FormRegistrationKey.ValidateKey(PrefC.GetString(PrefName.RegistrationKey))){
				if(isSilentUpdate) {
					ExitCode=311;//Registration Key could not be validated
					Application.Exit();
					return false;
				}
				FormRegistrationKey FormR=new FormRegistrationKey();
				FormR.ShowDialog();
				if(FormR.DialogResult!=DialogResult.OK){
					Application.Exit();
					return false;
				}
				Cache.Refresh(InvalidType.Prefs);
			}
			//This must be done at startup in case the user does not perform any action to save something to temp file.
			//This will cause slowdown, but only for the first week.
			if(DateTime.Today<PrefC.GetDate(PrefName.TempFolderDateFirstCleaned).AddDays(7)) {
				PrefL.GetTempFolderPath();//We don't care about the return value.  Just trying to trigger the one-time cleanup and create the temp/opendental directory.
			}
			Lans.RefreshCache();//automatically skips if current culture is en-US
			LanguageForeigns.Refresh(CultureInfo.CurrentCulture.Name,CultureInfo.CurrentCulture.TwoLetterISOLanguageName);//automatically skips if current culture is en-US
			//menuItemMergeDatabases.Visible=PrefC.GetBool(PrefName.RandomPrimaryKeys");
			return true;
		}
		
		///<summary>Refreshes certain rarely used data from database.  Must supply the types of data to refresh as flags.  Also performs a few other tasks that must be done when local data is changed.</summary>
		private void RefreshLocalData(params InvalidType[] itypes){
			List<int> itypeList=new List<int>();
			for(int i=0;i<itypes.Length;i++){
				itypeList.Add((int)itypes[i]);
			}
			string itypesStr="";
			bool isAll=false;
			for(int i=0;i<itypes.Length;i++) {
				if(i>0) {
					itypesStr+=",";
				}
				itypesStr+=((int)itypes[i]).ToString();
				if(itypes[i]==InvalidType.AllLocal){
					isAll=true;
				}
			}
			Cache.RefreshCache(itypesStr);
			RefreshLocalDataPostCleanup(itypeList,isAll,itypes);
		}
		
		///<summary>Performs a few tasks that must be done when local data is changed.</summary>
		private void RefreshLocalDataPostCleanup(List<int> itypeList,bool isAll,params InvalidType[] itypes) {//This is where the flickering and reset of windows happens
			#region IvalidType.Prefs
			if(itypeList.Contains((int)InvalidType.Prefs) || isAll) {
				if(PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
					menuItemSchools.Visible=false;
					menuItemCounties.Visible=false;
					menuItemScreening.Visible=false;
				}
				else {
					menuItemSchools.Visible=true;
					menuItemCounties.Visible=true;
					menuItemScreening.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					menuItemClinics.Visible=false;
					menuClinics.Visible=false;
				}
				else {
					menuItemClinics.Visible=true;
					menuClinics.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyHideClinical)) {
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Procs");
				}
				else {
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");
				}
				if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					menuItemSchoolClass.Visible=false;
					menuItemSchoolCourses.Visible=false;
					menuItemDentalSchools.Visible=false;
					menuItemRequirementsNeeded.Visible=false;
					menuItemReqStudents.Visible=false;
					menuItemEvaluations.Visible=false;
				}
				else {
					menuItemSchoolClass.Visible=true;
					menuItemSchoolCourses.Visible=true;
					menuItemRequirementsNeeded.Visible=true;
					menuItemReqStudents.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyHideRepeatCharges)) {
					menuItemRepeatingCharges.Visible=false;
				}
				else {
					menuItemRepeatingCharges.Visible=true;
				}
				if(PrefC.GetString(PrefName.DistributorKey)=="") {
					menuItemCustomerManage.Visible=false;
					menuItemNewCropBilling.Visible=false;
				}
				else {
					menuItemCustomerManage.Visible=true;
					menuItemNewCropBilling.Visible=true;
				}
				CheckCustomReports();
				if(NeedsRedraw("ChartModule")) {
					ContrChart2.InitializeLocalData();
				}
				if(NeedsRedraw("TaskLists")) {
					if(PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom)) {//Refreshing task list here may not be the best course of action.
						//separate if statement to prevent database call if not showing task list at bottom to begin with
						//ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
						if(ComputerPrefs.LocalComputer.TaskKeepListHidden) {
							userControlTasks1.Visible = false;
						}
						else if(this.WindowState!=FormWindowState.Minimized) {//task list show and window is not minimized.
							userControlTasks1.Visible = true;
							userControlTasks1.InitializeOnStartup();
							if(ComputerPrefs.LocalComputer.TaskDock == 0) {//bottom
								menuItemDockBottom.Checked = true;
								menuItemDockRight.Checked = false;
								panelSplitter.Cursor=Cursors.HSplit;
								panelSplitter.Height=7;
								int splitterNewY=540;
								if(ComputerPrefs.LocalComputer.TaskY!=0) {
									splitterNewY=ComputerPrefs.LocalComputer.TaskY;
									if(splitterNewY<300) {
										splitterNewY=300;//keeps it from going too high
									}
									if(splitterNewY>ClientSize.Height-50) {
										splitterNewY=ClientSize.Height-panelSplitter.Height-50;//keeps it from going off the bottom edge
									}
								}
								panelSplitter.Location=new Point(myOutlookBar.Width,splitterNewY);
							}
							else {//right
								menuItemDockRight.Checked = true;
								menuItemDockBottom.Checked = false;
								panelSplitter.Cursor=Cursors.VSplit;
								panelSplitter.Width=7;
								int splitterNewX=900;
								if(ComputerPrefs.LocalComputer.TaskX!=0) {
									splitterNewX=ComputerPrefs.LocalComputer.TaskX;
									if(splitterNewX<300) {
										splitterNewX=300;//keeps it from going too far to the left
									}
									if(splitterNewX>ClientSize.Width-50) {
										splitterNewX=ClientSize.Width-panelSplitter.Width-50;//keeps it from going off the right edge
									}
								}
								panelSplitter.Location=new Point(splitterNewX,ToolBarMain.Height);
							}
						}
					}
					else {
						userControlTasks1.Visible = false;
					}
				}
				LayoutControls();
			}
			#endregion
			#region InvalidType.Signals
			if(itypeList.Contains((int)InvalidType.Signals) || isAll) {
				FillSignalButtons(null);
			}
			#endregion
			#region InvalidType.Programs
			if(itypeList.Contains((int)InvalidType.Programs) || isAll) {
				if(Programs.GetCur(ProgramName.PT).Enabled) {
					Bridges.PaperlessTechnology.InitializeFileWatcher();
				}
			}
			#endregion
			#region InvalidType.Programs OR InvalidType.Prefs
			if(itypeList.Contains((int)InvalidType.Programs) || itypeList.Contains((int)InvalidType.Prefs) || isAll) {
				if(PrefC.GetBool(PrefName.EasyBasicModules)) {
					myOutlookBar.Buttons[3].Visible=false;
					myOutlookBar.Buttons[5].Visible=false;
					myOutlookBar.Buttons[6].Visible=false;
				}
				else {
					myOutlookBar.Buttons[3].Visible=true;
					myOutlookBar.Buttons[5].Visible=true;
					myOutlookBar.Buttons[6].Visible=true;
				}
				if(Programs.UsingEcwTightOrFullMode()) {//has nothing to do with HL7
					if(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"ShowImagesModule")=="1") {
						myOutlookBar.Buttons[5].Visible=true;
					}
					else {
						myOutlookBar.Buttons[5].Visible=false;
					}
				}
				if(Programs.UsingEcwTightMode()) {//has nothing to do with HL7
					myOutlookBar.Buttons[6].Visible=false;
				}
				if(Programs.UsingEcwTightOrFullMode()) {//old eCW interfaces
					if(Programs.UsingEcwTightMode()) {
						myOutlookBar.Buttons[0].Visible=false;//Appt
						myOutlookBar.Buttons[2].Visible=false;//Account
					}
					else if(Programs.UsingEcwFullMode()) {
						//We might create a special Appt module for eCW full users so they can access Recall.
						myOutlookBar.Buttons[0].Visible=false;//Appt
					}
				}
				else if(HL7Defs.IsExistingHL7Enabled()) {//There may be a def enabled as well as the old program link enabled. In this case, do not look at the def for whether or not to show the appt and account modules, instead go by the eCW interface enabled.
					HL7Def def=HL7Defs.GetOneDeepEnabled();
					myOutlookBar.Buttons[0].Visible=def.ShowAppts;//Appt
					myOutlookBar.Buttons[2].Visible=def.ShowAccount;//Account
				}
				else {//no def and not using eCW tight or full program link
					myOutlookBar.Buttons[0].Visible=true;//Appt
					myOutlookBar.Buttons[2].Visible=true;//Account
				}
				if(Programs.UsingOrion) {
					myOutlookBar.Buttons[0].Visible=false;//Appt module
					myOutlookBar.Buttons[2].Visible=false;//Account module
					myOutlookBar.Buttons[3].Visible=false;//TP module
				}
				myOutlookBar.Invalidate();
			}
			#endregion
			#region InvalidType.ToolBut
			if(itypeList.Contains((int)InvalidType.ToolBut) || isAll) {
				ContrAccount2.LayoutToolBar();
				ContrAppt2.LayoutToolBar();
				ContrChart2.LayoutToolBar();
				ContrImages2.LayoutToolBar();
				ContrFamily2.LayoutToolBar();
			}
			#endregion
			#region InvalidType.Views
			if(itypeList.Contains((int)InvalidType.Views) || isAll) {
				ContrAppt2.FillViews();
			}
			#endregion
			#region HQ Only
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				if(itypeList.Contains((int)InvalidType.Employees) || itypeList.Contains((int)InvalidType.Prefs) || isAll) { //refill the triage coordinator combo box
					comboTriageCoordinator.Items.Clear();
					Employee currentTriageEmployee=null;
					try {
						currentTriageEmployee=Employees.GetEmp(PrefC.GetLong(PrefName.HQTriageCoordinator));
					}
					catch{
						//swallow the case where PrefName.HQTriageCoordinator not found
					}
					int iSelItem=-1;
					for(int i=0;i<Employees.ListShort.Length;i++) {
						int iNewItem=comboTriageCoordinator.Items.Add(Employees.GetNameFL(Employees.ListShort[i]));
						if(currentTriageEmployee!=null && currentTriageEmployee.EmployeeNum==Employees.ListShort[i].EmployeeNum) {
							iSelItem=iNewItem;
						}
					}
					if(iSelItem>=0) {
						comboTriageCoordinator.SelectedIndex=iSelItem;
					}
				}
			}
			#endregion
			//TODO: If there are still issues with TP refreshing, include TP prefs in needsRedraw()
			ContrTreat2.InitializeLocalData();//easier to leave this here for now than to split it.
			dictChartPrefsCache.Clear();
			dictTaskListPrefsCache.Clear();
			//Chart Drawing Prefs
			dictChartPrefsCache.Add(PrefName.DistributorKey.ToString(),PrefC.GetBool(PrefName.DistributorKey));
			dictChartPrefsCache.Add(PrefName.UseInternationalToothNumbers.ToString(),PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			dictChartPrefsCache.Add("GraphicsUseHardware",ComputerPrefs.LocalComputer.GraphicsUseHardware);
			dictChartPrefsCache.Add("PreferredPixelFormatNum",ComputerPrefs.LocalComputer.PreferredPixelFormatNum);
			dictChartPrefsCache.Add("GraphicsSimple",ComputerPrefs.LocalComputer.GraphicsSimple);
			dictChartPrefsCache.Add(PrefName.ShowFeatureEhr.ToString(),PrefC.GetBool(PrefName.ShowFeatureEhr));
			dictChartPrefsCache.Add("DirectXFormat",ComputerPrefs.LocalComputer.DirectXFormat);
			//Task list drawing prefs
			dictTaskListPrefsCache.Add("TaskDock",ComputerPrefs.LocalComputer.TaskDock);
			dictTaskListPrefsCache.Add("TaskY",ComputerPrefs.LocalComputer.TaskY);
			dictTaskListPrefsCache.Add("TaskX",ComputerPrefs.LocalComputer.TaskX);
			dictTaskListPrefsCache.Add(PrefName.TaskListAlwaysShowsAtBottom.ToString(),PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom));
			dictTaskListPrefsCache.Add(PrefName.TasksCheckOnStartup.ToString(),PrefC.GetBool(PrefName.TasksCheckOnStartup));
			dictTaskListPrefsCache.Add(PrefName.TasksNewTrackedByUser.ToString(),PrefC.GetBool(PrefName.TasksNewTrackedByUser));
			dictTaskListPrefsCache.Add(PrefName.TasksShowOpenTickets.ToString(),PrefC.GetBool(PrefName.TasksShowOpenTickets));
			dictTaskListPrefsCache.Add("TaskKeepListHidden",ComputerPrefs.LocalComputer.TaskKeepListHidden);
		}

		///<summary>Compares preferences related to sections of the program that require redraws and returns true if a redraw is necessary, false otherwise.  If anything goes wrong with checking the status of any preference this method will return true.</summary>
		private bool NeedsRedraw(string section) {
			try {
				switch(section) {
					case "ChartModule":
						if(dictChartPrefsCache.Count==0
							|| PrefC.GetBool(PrefName.DistributorKey)!=(bool)dictChartPrefsCache["DistributorKey"]
							|| PrefC.GetInt(PrefName.UseInternationalToothNumbers)!=(int)dictChartPrefsCache["UseInternationalToothNumbers"]
							|| ComputerPrefs.LocalComputer.GraphicsUseHardware!=(bool)dictChartPrefsCache["GraphicsUseHardware"]
							|| ComputerPrefs.LocalComputer.PreferredPixelFormatNum!=(int)dictChartPrefsCache["PreferredPixelFormatNum"]
							|| ComputerPrefs.LocalComputer.GraphicsSimple!=(DrawingMode)dictChartPrefsCache["GraphicsSimple"]
							|| PrefC.GetBool(PrefName.ShowFeatureEhr)!=(bool)dictChartPrefsCache["ShowFeatureEhr"]
							|| ComputerPrefs.LocalComputer.DirectXFormat!=(string)dictChartPrefsCache["DirectXFormat"]) 
						{
							return true;
						}
						break;
					case "TaskLists":
						if(dictTaskListPrefsCache.Count==0
							|| ComputerPrefs.LocalComputer.TaskDock!=(int)dictTaskListPrefsCache["TaskDock"] //Checking for task list redrawing
							|| ComputerPrefs.LocalComputer.TaskY!=(int)dictTaskListPrefsCache["TaskY"]
							|| ComputerPrefs.LocalComputer.TaskX!=(int)dictTaskListPrefsCache["TaskX"]
							|| PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom)!=(bool)dictTaskListPrefsCache["TaskListAlwaysShowsAtBottom"]
							|| PrefC.GetBool(PrefName.TasksCheckOnStartup)!=(bool)dictTaskListPrefsCache["TasksCheckOnStartup"]
							|| PrefC.GetBool(PrefName.TasksNewTrackedByUser)!=(bool)dictTaskListPrefsCache["TasksNewTrackedByUser"]
							|| PrefC.GetBool(PrefName.TasksShowOpenTickets)!=(bool)dictTaskListPrefsCache["TasksShowOpenTickets"]
							|| ComputerPrefs.LocalComputer.TaskKeepListHidden!=(bool)dictTaskListPrefsCache["TaskKeepListHidden"]) 
						{
							return true;
						}
						break;
					//case "TreatmentPlan":
					//	//If needed implement this section
					//	break;
				}//end switch
				return false;
			}
			catch {
				return true;//Should never happen.  Would most likely be caused by invalid preferences within the database.
			}
		}

		///<summary>Sets up the custom reports list in the main menu when certain requirements are met, or disables the custom reports menu item when those same conditions are not met. This function is called during initialization, and on the event that the A to Z folder usage has changed.</summary>
		private void CheckCustomReports(){
			menuItemCustomReports.MenuItems.Clear();
			//Try to load custom reports, but only if using the A to Z folders.
			if(PrefC.AtoZfolderUsed) {
				string imagePath=ImageStore.GetPreferredAtoZpath();
				string reportFolderName=PrefC.GetString(PrefName.ReportFolderName);
				string reportDir=ODFileUtils.CombinePaths(imagePath,reportFolderName);
				try {
					if(Directory.Exists(reportDir)) {
						DirectoryInfo infoDir=new DirectoryInfo(reportDir);
						FileInfo[] filesRdl=infoDir.GetFiles("*.rdl");
						for(int i=0;i<filesRdl.Length;i++) {
							string itemName=Path.GetFileNameWithoutExtension(filesRdl[i].Name);
							menuItemCustomReports.MenuItems.Add(itemName,new System.EventHandler(this.menuItemRDLReport_Click));
						}
					}
				}
				catch {
					MsgBox.Show(this,"Failed to retreive custom reports.");
				}
			}
			if(menuItemCustomReports.MenuItems.Count==0) {
				menuItemCustomReports.Visible=false;
			}else{
				menuItemCustomReports.Visible=true;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			button=new ODToolBarButton(Lan.g(this,"Select Patient"),0,"","Patient");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=menuPatient;
			ToolBarMain.Buttons.Add(button);
			if(!Programs.UsingEcwTightMode()) {//eCW tight only gets Patient Select and Popups toolbar buttons
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Commlog"),1,Lan.g(this,"New Commlog Entry"),"Commlog"));
				button=new ODToolBarButton(Lan.g(this,"E-mail"),2,Lan.g(this,"Send E-mail"),"Email");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuEmail;
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"WebMail"),2,Lan.g(this,"Secure WebMail"),"WebMail");
				button.Enabled=true;//Always enabled.  If the patient does not have an email address, then the user will be blocked from the FormWebMailMessageEdit window.
				ToolBarMain.Buttons.Add(button);
				if(_butText==null) {//If laying out again (after modifying setup), we keep the button to preserve the current notification text.
					_butText=new ODToolBarButton(Lan.g(this,"Text"),5,Lan.g(this,"Send Text Message"),"Text");
					_butText.Style=ODToolBarButtonStyle.DropDownButton;
					_butText.DropDownMenu=menuText;
				}
				ToolBarMain.Buttons.Add(_butText);
				button=new ODToolBarButton(Lan.g(this,"Letter"),-1,Lan.g(this,"Quick Letter"),"Letter");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuLetter;
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"Forms"),-1,"","Form");
				//button.Style=ODToolBarButtonStyle.DropDownButton;
				//button.DropDownMenu=menuForm;
				ToolBarMain.Buttons.Add(button);
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"To Task List"),3,Lan.g(this,"Send to Task List"),"Tasklist"));
				button=new ODToolBarButton(Lan.g(this,"Label"),4,Lan.g(this,"Print Label"),"Label");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuLabel;
				ToolBarMain.Buttons.Add(button);
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Popups"),-1,Lan.g(this,"Edit popups for this patient"),"Popups"));
			ArrayList toolButItems=ToolButItems.GetForToolBar(ToolBarsAvail.AllModules);
			for(int i=0;i<toolButItems.Count;i++) {
			  //ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			  ToolBarMain.Buttons.Add(new ODToolBarButton(((ToolButItem)toolButItems[i]).ButtonText
			    ,-1,"",((ToolButItem)toolButItems[i]).ProgramNum));
			}
			Plugins.HookAddCode(this,"FormOpenDental.LayoutToolBar_end");
			ToolBarMain.Invalidate();
		}

		private void menuPatient_Popup(object sender,EventArgs e) {
			if(CurPatNum==0){
				return;
			}
			Family fam=Patients.GetFamily(CurPatNum);
			PatientL.AddFamilyToMenu(menuPatient,new EventHandler(menuPatient_Click),CurPatNum,fam);
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				//standard predefined button
				switch(e.Button.Tag.ToString()) {
					case "Patient":
						OnPatient_Click();
						break;
					case "Commlog":
						OnCommlog_Click();
						break;
					case "Email":
						OnEmail_Click();
						break;
					case "WebMail":
						OnWebMail_Click();
						break;
					case "Text":
						OnTxtMsg_Click(CurPatNum);
						break;
					case "Letter":
						OnLetter_Click();
						break;
					case "Form":
						OnForm_Click();
						break;
					case "Tasklist":
						OnTasklist_Click();
						break;
					case "Label":
						OnLabel_Click();
						break;
					case "Popups":
						OnPopups_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(long)) {
				ProgramL.Execute((long)e.Button.Tag,Patients.GetPat(CurPatNum));
			}
		}

		private void OnPatient_Click() {
			FormPatientSelect formPS=new FormPatientSelect();
			formPS.ShowDialog();
			if(formPS.DialogResult==DialogResult.OK) {
				CurPatNum=formPS.SelectedPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				if(ContrChart2.Visible) {//If currently in the chart module, then also refresh NewCrop prescription information on top of a regular refresh.
					ContrChart2.ModuleSelectedNewCrop(CurPatNum);
				}
				else {
					RefreshCurrentModule();
				}
				FillPatientButton(pat);
				Plugins.HookAddCode(this,"FormOpenDental.OnPatient_Click_end");   
			}
		}

		private void menuPatient_Click(object sender,System.EventArgs e) {
			Family fam=Patients.GetFamily(CurPatNum);
			CurPatNum=PatientL.ButtonSelect(menuPatient,sender,fam);
			//new family now
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
		}

		///<summary>Happens when any of the modules changes the current patient or when this main form changes the patient.  The calling module should refresh itself.  The current patNum is stored here in the parent form so that when switching modules, the parent form knows which patient to call up for that module.</summary>
		private void Contr_PatientSelected(object sender,PatientSelectedEventArgs e) {
			CurPatNum=e.Pat.PatNum;
			//RefreshCurrentModule();//We think this should be here because this function does the same thing as menuPatient_Click, but we're unsure of the extent of the change so left it out for now.
			FillPatientButton(e.Pat);
		}

		///<Summary>Serves four functions.  1. Sends the new patient to the dropdown menu for select patient.  2. Changes which toolbar buttons are enabled.  3. Sets main form text. 4. Displays any popup.</Summary>
		private void FillPatientButton(Patient pat){
			if(pat==null) {
				pat=new Patient();
			}
			bool patChanged=PatientL.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),pat.GetNameLF(),pat.PatNum);
			if(patChanged){
				if(AutomationL.Trigger(AutomationTrigger.OpenPatient,null,pat.PatNum)) {//if a trigger happened
					if(ContrAppt2.Visible) {
						ContrAppt2.MouseUpForced();
					}
				}
			}
			if(ToolBarMain.Buttons==null || ToolBarMain.Buttons.Count<2){//on startup.  js Not sure why it's checking count.
				return;
			}
			if(CurPatNum==0) {//Only on startup, I think.
				if(!Programs.UsingEcwTightMode()) {//eCW tight only gets Patient Select and Popups toolbar buttons
					ToolBarMain.Buttons["Email"].Enabled=false;
					ToolBarMain.Buttons["WebMail"].Enabled=false;
					ToolBarMain.Buttons["Commlog"].Enabled=false;
					ToolBarMain.Buttons["Letter"].Enabled=false;
					ToolBarMain.Buttons["Form"].Enabled=false;
					ToolBarMain.Buttons["Tasklist"].Enabled=false;
					ToolBarMain.Buttons["Label"].Enabled=false;
				}
				ToolBarMain.Buttons["Popups"].Enabled=false;
			}
			else {
				if(!Programs.UsingEcwTightMode()) {
					ToolBarMain.Buttons["Commlog"].Enabled=true;
					if(pat.Email!="") {
						ToolBarMain.Buttons["Email"].Enabled=true;
					}
					else {
						ToolBarMain.Buttons["Email"].Enabled=false;
					}
					if(Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled()) {
						ToolBarMain.Buttons["Text"].Enabled=true;
					}
					else {
						ToolBarMain.Buttons["Text"].Enabled=false;
					}
					ToolBarMain.Buttons["WebMail"].Enabled=true;
					ToolBarMain.Buttons["Letter"].Enabled=true;
					ToolBarMain.Buttons["Form"].Enabled=true;
					ToolBarMain.Buttons["Tasklist"].Enabled=true;
					ToolBarMain.Buttons["Label"].Enabled=true;
				}
				ToolBarMain.Buttons["Popups"].Enabled=true;
			}
			ToolBarMain.Invalidate();
			Text=PatientL.GetMainTitle(pat,ClinicNum);
			if(PopupEventList==null){
				PopupEventList=new List<PopupEvent>();
			}
			if(Plugins.HookMethod(this,"FormOpenDental.FillPatientButton_popups",pat,PopupEventList,patChanged)) {
				return;
			}
			if(!patChanged) {
				return;
			}
			if(ContrChart2.Visible) {
				TryNonPatientPopup();
			}
			//New patient selected.  Everything below here is for popups.
			//First, remove all expired popups from the event list.
			for(int i=PopupEventList.Count-1;i>=0;i--){//go backwards
				if(PopupEventList[i].DisableUntil<DateTime.Now){//expired
					PopupEventList.RemoveAt(i);
				}
			}
			//Now, loop through all popups for the patient.
			List<Popup> popList=Popups.GetForPatient(pat.PatNum);//get all possible 
			for(int i=0;i<popList.Count;i++) {
				//skip any popups that are disabled because they are on the event list
				bool popupIsDisabled=false;
				for(int e=0;e<PopupEventList.Count;e++){
					if(popList[i].PopupNum==PopupEventList[e].PopupNum){
						popupIsDisabled=true;
						break;
					}
				}
				if(popupIsDisabled){
					continue;
				}
				//This popup is not disabled, so show it.
				//A future improvement would be to assemble all the popups that are to be shown and then show them all in one large window.
				//But for now, they will show in sequence.
				if(ContrAppt2.Visible) {
					ContrAppt2.MouseUpForced();
				}
				FormPopupDisplay FormP=new FormPopupDisplay();
				FormP.PopupCur=popList[i];
				FormP.ShowDialog();
				if(FormP.MinutesDisabled>0){
					PopupEvent popevent=new PopupEvent();
					popevent.PopupNum=popList[i].PopupNum;
					popevent.DisableUntil=DateTime.Now+TimeSpan.FromMinutes(FormP.MinutesDisabled);
					PopupEventList.Add(popevent);
					PopupEventList.Sort();
				}
			}
		}

		private void OnEmail_Click() {
			//this button item will be disabled if pat does not have email address
			if(!Security.IsAuthorized(Permissions.EmailSend)){
				return;
			}
			EmailMessage message=new EmailMessage();
			message.PatNum=CurPatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			message.ToAddress=pat.Email;
			message.FromAddress=EmailAddresses.GetByClinic(pat.ClinicNum).SenderAddress;
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message);
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
			}
		}

		private void menuEmail_Popup(object sender,EventArgs e) {
			menuEmail.MenuItems.Clear();
			MenuItem menuItem;
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuEmail.MenuItems.Add(menuItem);
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++) {
				refer=Referrals.GetReferral(refAttaches[i].ReferralNum);
				if(refAttaches[i].IsFrom) {
					str=Lan.g(this,"From ");
				}
				else {
					str=Lan.g(this,"To ");
				}
				str+=Referrals.GetNameFL(refer.ReferralNum)+" <";
				if(refer.EMail==""){
					str+=Lan.g(this,"no email");
				}
				else{
					str+=refer.EMail;
				}
				str+=">";
				menuItem=new MenuItem(str,menuEmail_Click);
				menuItem.Tag=refer;
				menuEmail.MenuItems.Add(menuItem);
			}
		}

		private void OnWebMail_Click() {
			if(!Security.IsAuthorized(Permissions.WebmailSend)) {
				return;
			}
			FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(CurPatNum);
			FormWMME.ShowDialog();
		}

		private void menuEmail_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null){
				return;
			}
			LabelSingle label=new LabelSingle();
			if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				if(refer.EMail==""){
					return;
					//MsgBox.Show(this,"");
				}
				EmailMessage message=new EmailMessage();
				message.PatNum=CurPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				message.ToAddress=refer.EMail;//pat.Email;
				message.FromAddress=EmailAddresses.GetByClinic(pat.ClinicNum).SenderAddress;
				message.Subject=Lan.g(this,"RE: ")+pat.GetNameFL();
				FormEmailMessageEdit FormE=new FormEmailMessageEdit(message);
				FormE.IsNew=true;
				FormE.ShowDialog();
				if(FormE.DialogResult==DialogResult.OK) {
					RefreshCurrentModule();
				}
			}
		}

		private void OnCommlog_Click() {
			if(Plugins.HookMethod(this,"FormOpenDental.OnCommlog_Click",CurPatNum)) {
				return;
			}
			Commlog CommlogCur = new Commlog();
			CommlogCur.PatNum = CurPatNum;
			CommlogCur.CommDateTime = DateTime.Now;
			CommlogCur.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			if(PrefC.GetBool(PrefName.DistributorKey)) {//for OD HQ
				CommlogCur.Mode_=CommItemMode.None;
				CommlogCur.SentOrReceived=CommSentOrReceived.Neither;
			}
			else {
				CommlogCur.Mode_=CommItemMode.Phone;
				CommlogCur.SentOrReceived=CommSentOrReceived.Received;
			}
			CommlogCur.UserNum=Security.CurUser.UserNum;
			FormCommItem FormCI = new FormCommItem(CommlogCur);
			FormCI.IsNew = true;
			FormCI.ShowDialog();
			if(FormCI.DialogResult == DialogResult.OK) {
				RefreshCurrentModule();
			}
		}

		private void OnLetter_Click() {
			FormSheetPicker FormS=new FormSheetPicker();
			FormS.SheetType=SheetTypeEnum.PatientLetter;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			SheetDef sheetDef=FormS.SelectedSheetDefs[0];
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,CurPatNum);
			SheetParameter.SetParameter(sheet,"PatNum",CurPatNum);
			//SheetParameter.SetParameter(sheet,"ReferralNum",referral.ReferralNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet,this.CreateGraphics());
			FormSheetFillEdit FormSF=new FormSheetFillEdit(sheet);
			FormSF.ShowDialog();
			if(FormSF.DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
			}
			//Patient pat=Patients.GetPat(CurPatNum);
			//FormLetters FormL=new FormLetters(pat);
			//FormL.ShowDialog();
		}

		private void menuLetter_Popup(object sender,EventArgs e) {
			menuLetter.MenuItems.Clear();
			MenuItem menuItem;
			menuItem=new MenuItem(Lan.g(this,"Merge"),menuLetter_Click);
			menuItem.Tag="Merge";
			menuLetter.MenuItems.Add(menuItem);
			//menuItem=new MenuItem(Lan.g(this,"Stationery"),menuLetter_Click);
			//menuItem.Tag="Stationery";
			//menuLetter.MenuItems.Add(menuItem);
			menuLetter.MenuItems.Add("-");
			//Referrals---------------------------------------------------------------------------------------
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuLetter.MenuItems.Add(menuItem);
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++) {
				refer=Referrals.GetReferral(refAttaches[i].ReferralNum);
				if(refAttaches[i].IsFrom) {
					str=Lan.g(this,"From ");
				}
				else {
					str=Lan.g(this,"To ");
				}
				str+=Referrals.GetNameFL(refer.ReferralNum);
				menuItem=new MenuItem(str,menuLetter_Click);
				menuItem.Tag=refer;
				menuLetter.MenuItems.Add(menuItem);
			}
		}

		private void menuLetter_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null) {
				return;
			}
			Patient pat=Patients.GetPat(CurPatNum);
			if(((MenuItem)sender).Tag.GetType()==typeof(string)) {
				if(((MenuItem)sender).Tag.ToString()=="Merge") {
					FormLetterMerges FormL=new FormLetterMerges(pat);
					FormL.ShowDialog();
				}
				//if(((MenuItem)sender).Tag.ToString()=="Stationery") {
				//	FormCommunications.PrintStationery(pat);
				//}
			}
			if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				FormSheetPicker FormS=new FormSheetPicker();
				FormS.SheetType=SheetTypeEnum.ReferralLetter;
				FormS.ShowDialog();
				if(FormS.DialogResult!=DialogResult.OK){
					return;
				}
				SheetDef sheetDef=FormS.SelectedSheetDefs[0];
				Sheet sheet=SheetUtil.CreateSheet(sheetDef,CurPatNum);
				SheetParameter.SetParameter(sheet,"PatNum",CurPatNum);
				SheetParameter.SetParameter(sheet,"ReferralNum",refer.ReferralNum);
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet,this.CreateGraphics());
				FormSheetFillEdit FormSF=new FormSheetFillEdit(sheet);
				FormSF.ShowDialog();
				if(FormSF.DialogResult==DialogResult.OK) {
					RefreshCurrentModule();
				}
				//FormLetters FormL=new FormLetters(pat);
				//FormL.ReferralCur=refer;
				//FormL.ShowDialog();
			}
		}

		private void OnForm_Click() {
			FormPatientForms formP=new FormPatientForms();
			formP.PatNum=CurPatNum;
			formP.ShowDialog();
			//if(ContrAccount2.Visible || ContrChart2.Visible//The only two modules where a new form would show.
			//	|| ContrFamily2.Visible){//patient info
			//always refresh, especially to get the titlebar right after an import.
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
			//}
		}

		private void OnTasklist_Click(){
			FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient);//,CurPatNum);
			FormT.Location=new Point(50,50);
			FormT.ShowDialog();
			if(FormT.DialogResult!=DialogResult.OK) {
				return;
			}
			Task task=new Task();
			task.TaskListNum=-1;//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld=task.Copy();
			task.KeyNum=CurPatNum;
			task.ObjectType=TaskObjectType.Patient;
			task.TaskListNum=FormT.SelectedTaskListNum;
			task.UserNum=Security.CurUser.UserNum;
			FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
			FormTE.IsNew=true;
			FormTE.Show();
		}

		private delegate void ToolBarMainClick(long patNum);

		private void OnLabel_Click() {
			//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
			//when it comes from a toolbar click.
			//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
			ToolBarMainClick toolClick=LabelSingle.PrintPat;
			this.BeginInvoke(toolClick,CurPatNum);
		}

		private void menuLabel_Popup(object sender,EventArgs e) {
			menuLabel.MenuItems.Clear();
			MenuItem menuItem;
			List<SheetDef> LabelList=SheetDefs.GetCustomForType(SheetTypeEnum.LabelPatient);
			if(LabelList.Count==0){
				menuItem=new MenuItem(Lan.g(this,"LName, FName, Address"),menuLabel_Click);
				menuItem.Tag="PatientLFAddress";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Name, ChartNumber"),menuLabel_Click);
				menuItem.Tag="PatientLFChartNumber";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Name, PatNum"),menuLabel_Click);
				menuItem.Tag="PatientLFPatNum";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Radiograph"),menuLabel_Click);
				menuItem.Tag="PatRadiograph";
				menuLabel.MenuItems.Add(menuItem);
			}
			else{
				for(int i=0;i<LabelList.Count;i++) {
					menuItem=new MenuItem(LabelList[i].Description,menuLabel_Click);
					menuItem.Tag=LabelList[i];
					menuLabel.MenuItems.Add(menuItem);
				}
			}
			menuLabel.MenuItems.Add("-");
			//Carriers---------------------------------------------------------------------------------------
			Family fam=Patients.GetFamily(CurPatNum);
			List <PatPlan> PatPlanList=PatPlans.Refresh(CurPatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> PlanList=InsPlans.RefreshForSubList(subList);
			Carrier carrier;
			InsPlan plan;
			InsSub sub;
			for(int i=0;i<PatPlanList.Count;i++) {
				sub=InsSubs.GetSub(PatPlanList[i].InsSubNum,subList);
				plan=InsPlans.GetPlan(sub.PlanNum,PlanList);
				carrier=Carriers.GetCarrier(plan.CarrierNum);
				menuItem=new MenuItem(carrier.CarrierName,menuLabel_Click);
				menuItem.Tag=carrier;
				menuLabel.MenuItems.Add(menuItem);
			}
			menuLabel.MenuItems.Add("-");
			//Referrals---------------------------------------------------------------------------------------
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuLabel.MenuItems.Add(menuItem);
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++){
				refer=Referrals.GetReferral(refAttaches[i].ReferralNum);
				if(refAttaches[i].IsFrom){
					str=Lan.g(this,"From ");
				}
				else{
					str=Lan.g(this,"To ");
				}
				str+=Referrals.GetNameFL(refer.ReferralNum);
				menuItem=new MenuItem(str,menuLabel_Click);
				menuItem.Tag=refer;
				menuLabel.MenuItems.Add(menuItem);
			}
		}

		private void menuLabel_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null) {
				return;
			}
			//LabelSingle label=new LabelSingle();
			if(((MenuItem)sender).Tag.GetType()==typeof(string)){
				if(((MenuItem)sender).Tag.ToString()=="PatientLFAddress"){
					LabelSingle.PrintPatientLFAddress(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatientLFChartNumber") {
					LabelSingle.PrintPatientLFChartNumber(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatientLFPatNum") {
					LabelSingle.PrintPatientLFPatNum(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatRadiograph") {
					LabelSingle.PrintPatRadiograph(CurPatNum);
				}
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(SheetDef)){
				LabelSingle.PrintCustomPatient(CurPatNum,(SheetDef)((MenuItem)sender).Tag);
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(Carrier)){
				Carrier carrier=(Carrier)((MenuItem)sender).Tag;
				LabelSingle.PrintCarrier(carrier.CarrierNum);
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				LabelSingle.PrintReferral(refer.ReferralNum);
			}
		}

		private void OnPopups_Click() {
			FormPopupsForFam FormPFF=new FormPopupsForFam();
			FormPFF.PatCur=Patients.GetPat(CurPatNum);
			FormPFF.ShowDialog();
		}

		#region SMS Text Messaging

		///<summary>Called from the text message button and the right click context menu for an appointment.</summary>
		private void OnTxtMsg_Click(long patNum,string startingText="") {
			if(patNum==0) {
				FormTxtMsgEdit FormTxtME=new FormTxtMsgEdit();
				FormTxtME.Message=startingText;
				FormTxtME.PatNum=0;
				FormTxtME.ShowDialog();
				if(FormTxtME.DialogResult==DialogResult.OK) {
					RefreshCurrentModule();
				}
				return;
			}
			Patient pat=Patients.GetPat(patNum);
			bool updateTextYN=false;
			if(pat.TxtMsgOk==YN.No) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient is marked to not receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) {
					updateTextYN=true;
				}
				else {
					return;
				}
			}
			if(pat.TxtMsgOk==YN.Unknown && PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo)) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient might not want to receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) {
					updateTextYN=true;
				}
				else {
					return;
				}
			}
			if(updateTextYN) {
				Patient patOld=pat.Copy();
				pat.TxtMsgOk=YN.Yes;
				Patients.Update(pat,patOld);
			}
			FormTxtMsgEdit FormTME=new FormTxtMsgEdit();
			FormTME.Message=startingText;
			FormTME.PatNum=patNum;
			FormTME.WirelessPhone=pat.WirelessPhone;
			FormTME.TxtMsgOk=pat.TxtMsgOk;
			FormTME.ShowDialog();
			if(FormTME.DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
			}
		}

		private void menuItemTextMessagesReceived_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(false,true);
		}

		private void menuItemTextMessagesSent_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(true,false);
		}

		private void menuItemTextMessagesAll_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(true,true);
		}

		private void ShowFormTextMessagingModeless(bool isSent,bool isReceived) {
			if(_formSmsTextMessaging==null || _formSmsTextMessaging.IsDisposed) {
				_formSmsTextMessaging=new FormSmsTextMessaging();
				_formSmsTextMessaging.IsSent=isSent;
				_formSmsTextMessaging.IsReceived=isReceived;
				_formSmsTextMessaging.SmsNotifier=SetSmsNotificationText;
				_formSmsTextMessaging.UnreadMessageCount=0;
				_formSmsTextMessaging.CurPatNum=CurPatNum;
				if(!String.IsNullOrEmpty(_butText.NotificationText)) {
					_formSmsTextMessaging.UnreadMessageCount=PIn.Long(_butText.NotificationText);
				}
				_formSmsTextMessaging.PatientGoTo+=Contr_PatientSelected;
				_formSmsTextMessaging.FormClosed+=FormSmsTextMessaging_FormClosed;
			}
			_formSmsTextMessaging.Show();
			_formSmsTextMessaging.BringToFront();
		}

		void FormSmsTextMessaging_FormClosed(object sender,FormClosedEventArgs e) {
			_formSmsTextMessaging=null;
		}

		///<summary>Set isSignalNeeded to true if a refresh signal should be sent out to the other workstations.</summary>
		private void SetSmsNotificationText(string smsNotificationText,bool isSignalNeeded) {
			try {
				if(_butText==null) {
					return;//This button does not exist in eCW tight integration mode.
				}
				if(!_butText.Enabled) {
					return;//This button is disabled when neither of the Text Messaging bridges have been enabled.
				}
				if(this.InvokeRequired) {//For when called from the ThreadSmsTextMessageNotify() thread.
					this.Invoke((Action)delegate() { SetSmsNotificationText(smsNotificationText,isSignalNeeded); });
				}
				if(smsNotificationText=="0") {
					smsNotificationText="";
				}
				if(_butText.NotificationText==smsNotificationText) {
					return;//This prevents the toolbar from being invalidated unnecessarily.  Also prevents unnecessary signals.
				}
				_butText.NotificationText=smsNotificationText;
				if(menuItemTextMessagesReceived.Text.Contains("(")) {//Remove the old count from the menu item.
					menuItemTextMessagesReceived.Text=menuItemTextMessagesReceived.Text.Substring(0,menuItemTextMessagesReceived.Text.IndexOf("(")-1);
				}
				if(smsNotificationText!="") {
					menuItemTextMessagesReceived.Text+=" ("+smsNotificationText+")";
				}
				if(isSignalNeeded) {
					Signalods.InsertSmsNotification(PIn.Long(smsNotificationText));
				}
			}			
			finally { //Always redraw the toolbar item. We may call this method just for this purpose.
				ToolBarMain.Invalidate();//To cause the Text button to redraw.			
			}			
		}

		#endregion SMS Text Messaging

		private void RefreshMenuClinics() {
			menuClinics.MenuItems.Clear();
			MenuItem menuItem;
			if(!Security.CurUser.ClinicIsRestricted) {
				menuItem=new MenuItem(Lan.g(this,"Headquarters"),menuClinic_Click);
				menuItem.Tag=new Clinic();//Having a ClinicNum of 0 will make OD act like 'Headquarters'.  This allows the user to see unassigned appt views, all operatories, etc.
				if(ClinicNum==0) {
					menuItem.Checked=true;
				}
				menuClinics.MenuItems.Add(menuItem);
				menuClinics.MenuItems.Add("-");//Separator
			}
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
			for(int i=0;i<listClinics.Count;i++) {
				menuItem=new MenuItem(listClinics[i].Description,menuClinic_Click);
				menuItem.Tag=listClinics[i];
				if(ClinicNum==listClinics[i].ClinicNum) {
					menuItem.Checked=true;
				}
				menuClinics.MenuItems.Add(menuItem);
			}
			RefreshLocalData(InvalidType.Views);//fills apptviews, sets the view, and then calls ContrAppt.ModuleSelected
			if(!ContrAppt2.Visible) {
				RefreshCurrentModule();//calls ModuleSelected of the current module, don't do this if ContrAppt2 is visible since it was just done above
			}
		}

		///<summary>This is used to set the private class wide variable _clinicNum and refreshes the current module.</summary>
		private void menuClinic_Click(object sender,System.EventArgs e) {
			if(sender.GetType()!=typeof(MenuItem) && ((MenuItem)sender).Tag!=null) {
				return;
			}
			Clinic clinicCur=(Clinic)((MenuItem)sender).Tag;
			ClinicNum=clinicCur.ClinicNum;
			Clinics.ClinicNum=ClinicNum;
			Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),ClinicNum);
			RefreshMenuClinics();
		}
		
		private void FormOpenDental_Resize(object sender,EventArgs e) {
			LayoutControls();
			if(Plugins.PluginsAreLoaded) {
				Plugins.HookAddCode(this,"FormOpenDental.FormOpenDental_Resize_end");
			}
		}

		///<summary>This used to be called much more frequently when it was an actual layout event.</summary>
		private void LayoutControls(){
			//Debug.WriteLine("layout");
			if(this.WindowState==FormWindowState.Minimized) {
				return;
			}
			if(Width<200){
				Width=200;
			}
			Point position=new Point(myOutlookBar.Width,ToolBarMain.Height);
			int width=this.ClientSize.Width-position.X;
			int height=this.ClientSize.Height-position.Y;
			if(userControlTasks1.Visible) {
				if(menuItemDockBottom.Checked) {
					if(panelSplitter.Height>8) {//docking needs to be changed
						panelSplitter.Height=7;
						panelSplitter.Location=new Point(position.X,540);
					}
					panelSplitter.Location=new Point(position.X,panelSplitter.Location.Y);
					panelSplitter.Width=width;
					panelSplitter.Visible=true;
					if(PrefC.GetBool(PrefName.DockPhonePanelShow)){
						//phoneSmall.Visible=true;
						//phoneSmall.Location=new Point(position.X,panelSplitter.Bottom+butBigPhones.Height);
						phoneSmall.Location=new Point(0,comboTriageCoordinator.Bottom);
						userControlTasks1.Location=new Point(position.X+phoneSmall.Width,panelSplitter.Bottom);
						userControlTasks1.Width=width-phoneSmall.Width;
						//butBigPhones.Visible=true;
						//butBigPhones.Location=new Point(position.X+phoneSmall.Width-butBigPhones.Width,panelSplitter.Bottom);
						//butBigPhones.BringToFront();
						//labelMsg.Visible=true;
						//labelMsg.Location=new Point(position.X+phoneSmall.Width-butBigPhones.Width-labelMsg.Width,panelSplitter.Bottom);
						//labelMsg.BringToFront();
						panelPhoneSmall.Visible=true;
						panelPhoneSmall.Location=new Point(position.X,panelSplitter.Bottom);
						panelPhoneSmall.BringToFront();
					}
					else{
						//phoneSmall.Visible=false;
						//phonePanel.Visible=false;
						//butBigPhones.Visible=false;
						//labelMsg.Visible=false;
						panelPhoneSmall.Visible=false;
						userControlTasks1.Location=new Point(position.X,panelSplitter.Bottom);
						userControlTasks1.Width=width;
					}
					userControlTasks1.Height=this.ClientSize.Height-userControlTasks1.Top;
					height=ClientSize.Height-panelSplitter.Height-userControlTasks1.Height-ToolBarMain.Height;
				}
				else {//docked Right
					//phoneSmall.Visible=false;
					//phonePanel.Visible=false;
					//butBigPhones.Visible=false;
					//labelMsg.Visible=false;
					panelPhoneSmall.Visible=false;
					if(panelSplitter.Width>8) {//docking needs to be changed
						panelSplitter.Width=7;
						panelSplitter.Location=new Point(900,position.Y);
					}
					panelSplitter.Location=new Point(panelSplitter.Location.X,position.Y);
					panelSplitter.Height=height;
					panelSplitter.Visible=true;
					userControlTasks1.Location=new Point(panelSplitter.Right,position.Y);
					userControlTasks1.Height=height;
					userControlTasks1.Width=this.ClientSize.Width-userControlTasks1.Left;
					width=ClientSize.Width-panelSplitter.Width-userControlTasks1.Width-position.X;
				}
				panelSplitter.BringToFront();
				panelSplitter.Invalidate();
			}
			else {
				//phoneSmall.Visible=false;
				//phonePanel.Visible=false;
				//butBigPhones.Visible=false;
				//labelMsg.Visible=false;
				panelPhoneSmall.Visible=false;
				panelSplitter.Visible=false;
			}
			ContrAccount2.Location=position;
			ContrAccount2.Width=width;
			ContrAccount2.Height=height;
			ContrAppt2.Location=position;
			ContrAppt2.Width=width;
			ContrAppt2.Height=height;
			ContrChart2.Location=position;
			ContrChart2.Width=width;
			ContrChart2.Height=height;
			ContrImages2.Location=position;
			ContrImages2.Width=width;
			ContrImages2.Height=height;
			ContrFamily2.Location=position;
			ContrFamily2.Width=width;
			ContrFamily2.Height=height;
			ContrFamily2Ecw.Location=position;
			ContrFamily2Ecw.Width=width;
			ContrFamily2Ecw.Height=height;
			ContrManage2.Location=position;
			ContrManage2.Width=width;
			ContrManage2.Height=height;
			ContrTreat2.Location=position;
			ContrTreat2.Width=width;
			ContrTreat2.Height=height;
		}

		private void panelSplitter_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=true;
			SplitterOriginalLocation=panelSplitter.Location;
			OriginalMousePos=new Point(panelSplitter.Left+e.X,panelSplitter.Top+e.Y);
		}

		private void panelSplitter_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnSplitter){
				return;
			}
			if(menuItemDockBottom.Checked){
				int splitterNewY=SplitterOriginalLocation.Y+(panelSplitter.Top+e.Y)-OriginalMousePos.Y;
				if(splitterNewY<300){
					splitterNewY=300;//keeps it from going too high
				}
				if(splitterNewY>ClientSize.Height-50){
					splitterNewY=ClientSize.Height-panelSplitter.Height-50;//keeps it from going off the bottom edge
				}
				panelSplitter.Top=splitterNewY;
			}
			else{//docked right
				int splitterNewX=SplitterOriginalLocation.X+(panelSplitter.Left+e.X)-OriginalMousePos.X;
				if(splitterNewX<300) {
					splitterNewX=300;//keeps it from going too far to the left
				}
				if(splitterNewX>ClientSize.Width-50) {
					splitterNewX=ClientSize.Width-panelSplitter.Width-50;//keeps it from going off the right edge
				}
				panelSplitter.Left=splitterNewX;
			}
			LayoutControls();
		}

		private void panelSplitter_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=false;
			TaskDockSavePos();
		}

		private void menuItemDockBottom_Click(object sender,EventArgs e) {
			menuItemDockBottom.Checked=true;
			menuItemDockRight.Checked=false;
			panelSplitter.Cursor=Cursors.HSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		private void menuItemDockRight_Click(object sender,EventArgs e) {
			menuItemDockBottom.Checked=false;
			menuItemDockRight.Checked=true;
			//included now with layoutcontrols
			panelSplitter.Cursor=Cursors.VSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		///<summary>Every time user changes doc position, it will save automatically.</summary>
		private void TaskDockSavePos(){
			//ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
			if(menuItemDockBottom.Checked){
				ComputerPrefs.LocalComputer.TaskY = panelSplitter.Top;
				ComputerPrefs.LocalComputer.TaskDock = 0;
			}
			else{
				ComputerPrefs.LocalComputer.TaskX = panelSplitter.Left;
				ComputerPrefs.LocalComputer.TaskDock = 1;
			}
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
		}
		
		///<summary>This is called when any local data becomes outdated.  It's purpose is to tell the other computers to update certain local data.</summary>
		private void DataValid_BecameInvalid(OpenDental.ValidEventArgs e){
			if(e.OnlyLocal){//Currently used after doing a restore from FormBackup so that the local cache is forcefully updated.
				if(!PrefsStartup()){//??
					return;
				}
				RefreshLocalData(InvalidType.AllLocal);//does local computer only
				return;
			}
			if(!e.ITypes.Contains((int)InvalidType.Date) //local refresh for dates is handled within ContrAppt, not here
				&& !e.ITypes.Contains((int)InvalidType.Task)//Tasks are not "cached" data.
				&& !e.ITypes.Contains((int)InvalidType.TaskPopup))
			{
				InvalidType[] itypeArray=new InvalidType[e.ITypes.Count];
				for(int i=0;i<itypeArray.Length;i++){
					itypeArray[i]=(InvalidType)e.ITypes[i];
				}
				RefreshLocalData(itypeArray);//does local computer
			}
			if(e.ITypes.Contains((int)InvalidType.Task) || e.ITypes.Contains((int)InvalidType.TaskPopup)) {
				//One of the two task lists needs to be refreshed on this instance of OD
				if(userControlTasks1.Visible) {
					userControlTasks1.RefreshTasks();
				}
				//See if FormTasks is currently open.
				if(ContrManage2!=null && ContrManage2.FormT!=null && !ContrManage2.FormT.IsDisposed) {
					ContrManage2.FormT.RefreshUserControlTasks();
				}
			}
			string itypeString="";
			for(int i=0;i<e.ITypes.Count;i++){
				if(i>0){
					itypeString+=",";
				}
				itypeString+=e.ITypes[i].ToString();
			}
			Signalod sig=new Signalod();
			sig.ITypes=itypeString;
			if(e.ITypes.Contains((int)InvalidType.Date)){
				sig.DateViewing=e.DateViewing;
			}
			else{
				sig.DateViewing=DateTime.MinValue;
			}
			sig.SigType=SignalType.Invalid;
			if(e.ITypes.Contains((int)InvalidType.Task) || e.ITypes.Contains((int)InvalidType.TaskPopup)){
				sig.TaskNum=e.TaskNum;
			}
			Signalods.Insert(sig);
		}

		private void GotoModule_ModuleSelected(ModuleEventArgs e){
			if(e.DateSelected.Year>1880){
				AppointmentL.DateSelected=e.DateSelected;
			}
			if(e.SelectedAptNum>0){
				ContrApptSingle.SelectedAptNum=e.SelectedAptNum;
			}
			//patient can also be set separately ahead of time instead of doing it this way:
			if(e.PatNum !=0) {
				CurPatNum=e.PatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			UnselectActive();
			allNeutral();
			if(e.ClaimNum>0){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAccount2.Visible=true;
				this.ActiveControl=this.ContrAccount2;
				ContrAccount2.ModuleSelected(CurPatNum,e.ClaimNum);
			}
			else if(e.PinAppts.Count!=0){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAppt2.Visible=true;
				this.ActiveControl=this.ContrAppt2;
				ContrAppt2.ModuleSelected(CurPatNum,e.PinAppts);
			}
			else if(e.DocNum>0) {
				myOutlookBar.SelectedIndex=e.IModule;
				ContrImages2.Visible=true;
				this.ActiveControl=this.ContrImages2;
				ContrImages2.ModuleSelected(CurPatNum,e.DocNum);
			}
			else if(e.IModule!=-1){
				myOutlookBar.SelectedIndex=e.IModule;
				SetModuleSelected();
			}
			myOutlookBar.Invalidate();
		}

		///<summary>If this is initial call when opening program, then set sigListButs=null.  This will cause a call to be made to database to get current status of buttons.  Otherwise, it adds the signals passed in to the current state, then paints.</summary>
		private void FillSignalButtons(List<Signalod> sigListButs) {
			if(!lightSignalGrid1.Visible){//for faster eCW loading
				return;
			}
			if(sigListButs==null){
				SigButDefList=SigButDefs.GetByComputer(SystemInformation.ComputerName);
				lightSignalGrid1.SetButtons(SigButDefList);
				sigListButs=Signalods.RefreshCurrentButState();
			}
			SigElementDef element;
			SigButDef butDef;
			int row;
			Color color;
			for(int i=0;i<sigListButs.Count;i++){
				if(sigListButs[i].AckTime.Year>1880){//process ack
					for(int j=0;j<sigListButs[i].ElementList.Length;j++) {
						int rowAck=lightSignalGrid1.ProcessAck(sigListButs[i].ElementList[j].SignalNum);
						if(rowAck!=-1) {
							butDef=SigButDefs.GetByIndex(rowAck,SigButDefList);
							if(butDef!=null) {
								PaintOnIcon(butDef.SynchIcon,Color.White);
							}
						}
					}
				}
				else{//process normal message
					row=0;
					color=Color.White;
					for(int e=0;e<sigListButs[i].ElementList.Length;e++){
						element=SigElementDefs.GetElement(sigListButs[i].ElementList[e].SigElementDefNum);
						if(element.LightRow!=0){
							row=element.LightRow;
						}
						if(element.LightColor.ToArgb()!=Color.White.ToArgb()){
							color=element.LightColor;
						}
					}
					if(row!=0 && color!=Color.White) {
						lightSignalGrid1.SetButtonActive(row-1,color,sigListButs[i]);
						butDef=SigButDefs.GetByIndex(row-1,SigButDefList);
						if(butDef!=null){
							try{
								PaintOnIcon(butDef.SynchIcon,color);
							}
							catch{
								MessageBox.Show("Error painting on program icon.  Probably too many non-ack'd messages.");
							}
						}
					}
				}
			}
		}

		///<summary>Pass in the cellNum as 1-based.</summary>
		private void PaintOnIcon(int cellNum,Color color){
			Graphics g;
			if(bitmapIcon==null){
				bitmapIcon=new Bitmap(16,16);
				g=Graphics.FromImage(bitmapIcon);
				g.FillRectangle(new SolidBrush(Color.White),0,0,15,15);
				//horizontal
				g.DrawLine(Pens.Black,0,0,15,0);
				g.DrawLine(Pens.Black,0,5,15,5);
				g.DrawLine(Pens.Black,0,10,15,10);
				g.DrawLine(Pens.Black,0,15,15,15);
				//vertical
				g.DrawLine(Pens.Black,0,0,0,15);
				g.DrawLine(Pens.Black,5,0,5,15);
				g.DrawLine(Pens.Black,10,0,10,15);
				g.DrawLine(Pens.Black,15,0,15,15);
				g.Dispose();
			}
			if(cellNum==0){
				return;
			}
			g=Graphics.FromImage(bitmapIcon);
			int x=0;
			int y=0;
			switch(cellNum){
				case 1: x=1; y=1; break;
				case 2: x=6; y=1; break;
				case 3: x=11; y=1; break;
				case 4: x=1; y=6; break;
				case 5: x=6; y=6; break;
				case 6: x=11; y=6; break;
				case 7: x=1; y=11; break;
				case 8: x=6; y=11; break;
				case 9: x=11; y=11; break;
			}
			g.FillRectangle(new SolidBrush(color),x,y,4,4);
			IntPtr intPtr=bitmapIcon.GetHicon();
			Icon=Icon.FromHandle(intPtr);
			DestroyIcon(intPtr);
			g.Dispose();
		}

		[System.Runtime.InteropServices.DllImport("user32.dll",CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		private void lightSignalGrid1_ButtonClick(object sender,OpenDental.UI.ODLightSignalGridClickEventArgs e) {
			if(e.ActiveSignal!=null){//user trying to ack an existing light signal
				Signalods.AckButton(e.ButtonIndex+1,signalLastRefreshed);
				//then, manually ack the light on this computer.  The second ack in a few seconds will be ignored.
				lightSignalGrid1.SetButtonActive(e.ButtonIndex,Color.White,null);
				SigButDef butDef=SigButDefs.GetByIndex(e.ButtonIndex,SigButDefList);
				if(butDef!=null) {
					PaintOnIcon(butDef.SynchIcon,Color.White);
				}
				return;
			}
			if(e.ButtonDef==null || e.ButtonDef.ElementList.Length==0){//there is no signal to send
				return;
			}
			//user trying to send a signal
			Signalod sig=new Signalod();
			sig.SigType=SignalType.Button;
			//sig.ToUser=sigElementDefUser[listTo.SelectedIndex].SigText;
			//sig.FromUser=sigElementDefUser[listFrom.SelectedIndex].SigText;
			//need to do this all as a transaction?
			Signalods.Insert(sig);
			int row=0;
			Color color=Color.White;
			SigElementDef def;
			SigElement element;
			SigButDefElement[] butElements=SigButDefElements.GetForButton(e.ButtonDef.SigButDefNum);
			for(int i=0;i<butElements.Length;i++){
				element=new SigElement();
				element.SigElementDefNum=butElements[i].SigElementDefNum;
				element.SignalNum=sig.SignalNum;
				SigElements.Insert(element);
				if(SigElementDefs.GetElement(element.SigElementDefNum).SigElementType==SignalElementType.User){
					sig.ToUser=SigElementDefs.GetElement(element.SigElementDefNum).SigText;
					Signalods.Update(sig);
				}
				def=SigElementDefs.GetElement(element.SigElementDefNum);
				if(def.LightRow!=0) {
					row=def.LightRow;
				}
				if(def.LightColor.ToArgb()!=Color.White.ToArgb()) {
					color=def.LightColor;
				}
			}
			sig.ElementList=new SigElement[0];//we don't really care about these
			if(row!=0 && color!=Color.White) {
				lightSignalGrid1.SetButtonActive(row-1,color,sig);//this just makes it seem more responsive.
				//we can skip painting on the icon
			}
		}

		///<summary>Called every time timerSignals_Tick fires.  Usually about every 5-10 seconds.  Does not fire until one signal interval has passed (5-10 seconds after login).</summary>
		public void ProcessSignals(){
			if(Security.CurUser==null) {
				//User must be at the log in screen, so no need to process signals.  We will need to look for shutdown signals since the last refreshed time when the user attempts to log in.
				return;
			}
			try {
				List<Signalod> sigList=Signalods.RefreshTimed(signalLastRefreshed);//this also attaches all elements to their sigs				
				if(_butText!=null){ //Not visible if eCW, and not enabled unless Text Messaging is enabled.					
					//Check for the specific sms signal.
					Signalod signal=sigList.OrderByDescending(x => x.SigDateTime)
						.FirstOrDefault(x => x.SigType==SignalType.Invalid && x.ITypes==((int)InvalidType.SmsTextMsgReceivedUnreadCount).ToString());
					if(signal!=null){
						//This signal is not used when using Callfire so we should not have any conflicts here.
						//The toolbar button might need to change state if we have just recently changed the state of IsIntegratedTextingEnabled().
						_butText.Enabled=(Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled());
						SetSmsNotificationText(signal.SigText,false);//Do not resend the signal again.  Would cause infinate signal loop.
					}					
					if(_butText.Enabled && _butText.NotificationText==null) {//The Notification text has not been set since startup.  We need an accurate starting count.
						SetSmsNotificationText(SmsFromMobiles.GetSmsNotification(),true);//Queries the database.  Send signal since we queried the database.
					}
				}
				if(sigList.Count==0) {
					return;
				}
				//look for shutdown signal
				for(int i=0;i<sigList.Count;i++) {
					if(sigList[i].ITypes==((int)InvalidType.ShutDownNow).ToString()) {
						timerSignals.Enabled=false;//quit receiving signals.
						//close the webcam if present so that it can be updated too.
						if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
							Process[] processes=Process.GetProcessesByName("WebCamOD");
							for(int p=0;p<processes.Length;p++) {
								processes[p].Kill();
							}
						}
						//start the thread that will kill the application
						Thread killThread=new Thread(new ThreadStart(KillThread));
						killThread.Start();
						string msg="";
						if(Process.GetCurrentProcess().ProcessName=="OpenDental") {
							msg+="All copies of Open Dental ";
						}
						else {
							msg+=Process.GetCurrentProcess().ProcessName+" ";
						}
						msg+=Lan.g(this,"will shut down in 15 seconds.  Quickly click OK on any open windows with unsaved data.");
						MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(msg);
						msgbox.Size=new Size(300,300);
						msgbox.TopMost=true;
						msgbox.ShowDialog();
						return;
					}
				}
				if(sigList[sigList.Count-1].AckTime.Year>1880) {
					signalLastRefreshed=sigList[sigList.Count-1].AckTime;
				}
				else {
					signalLastRefreshed=sigList[sigList.Count-1].SigDateTime;
				}
				if(ContrAppt2.Visible && Signalods.ApptNeedsRefresh(sigList,AppointmentL.DateSelected.Date)) {
					ContrAppt2.RefreshPeriod();
				}
				bool areAnySignalsTasks=false;
				for(int i=0;i<sigList.Count;i++) {
					if(sigList[i].ITypes==((int)InvalidType.Task).ToString()
						|| sigList[i].ITypes==((int)InvalidType.TaskPopup).ToString()) {
						areAnySignalsTasks=true;
					}
				}
				List<Task> tasksPopup=Signalods.GetNewTaskPopupsThisUser(sigList,Security.CurUser.UserNum);
				if(tasksPopup.Count>0) {
					for(int i=0;i<tasksPopup.Count;i++) {
						//Even though this is triggered to popup, if this is my own task, then do not popup.
						List<TaskNote> notesForThisTask=TaskNotes.GetForTask(tasksPopup[i].TaskNum);
						if(notesForThisTask.Count==0) {//'sender' is the usernum on the task
							if(tasksPopup[i].UserNum==Security.CurUser.UserNum) {
								continue;
							}
						}
						else {//'sender' is the user on the last added note
							if(notesForThisTask[notesForThisTask.Count-1].UserNum==Security.CurUser.UserNum) {
								continue;
							}
						}
						if(tasksPopup[i].TaskListNum!=Security.CurUser.TaskListInBox//if not my inbox
							&& Security.CurUser.DefaultHidePopups)//and popups blocked
						{
							continue;//no sound or popup
							//in other words, popups will always show for my inbox even if popups blocked.
						}
						if(tasksPopup[i].TaskListNum==Security.CurUser.TaskListInBox//if my inbox
							&& Security.CurUser.InboxHidePopups)//and inbox popups blocked
						{
							continue;//no sound or popup
							//in other words, popups will not show for my inbox if InboxHidePopups is enabled.
						}
						System.Media.SoundPlayer soundplay=new SoundPlayer(Properties.Resources.notify);
						soundplay.Play();
						this.BringToFront();//don't know if this is doing anything.
						FormTaskEdit FormT=new FormTaskEdit(tasksPopup[i],tasksPopup[i].Copy());
						FormT.IsPopup=true;
						FormT.Closing+=new CancelEventHandler(TaskGoToEvent);
						FormT.Show();//non-modal
					}
				}
				if(areAnySignalsTasks || tasksPopup.Count>0) {
					if(userControlTasks1.Visible) {
						userControlTasks1.RefreshTasks();
					}
					//See if FormTasks is currently open.
					if(ContrManage2!=null && ContrManage2.FormT!=null && !ContrManage2.FormT.IsDisposed) {
						ContrManage2.FormT.RefreshUserControlTasks();
					}
				}
				List<int> itypes=Signalods.GetInvalidTypes(sigList);
				InvalidType[] itypeArray=new InvalidType[itypes.Count];
				for(int i=0;i<itypeArray.Length;i++) {
					itypeArray[i]=(InvalidType)itypes[i];
				}
				//InvalidTypes invalidTypes=Signalods.GetInvalidTypes(sigList);
				if(itypes.Count>0) {//invalidTypes!=0){
					RefreshLocalData(itypeArray);
				}
				List<Signalod> sigListButs=Signalods.GetButtonSigs(sigList);
				ContrManage2.LogMsgs(sigListButs);
				FillSignalButtons(sigListButs);
				//Need to add a test to this: do not play messages that are over 2 minutes old.
				Thread newThread=new Thread(new ParameterizedThreadStart(PlaySounds));
				newThread.Start(sigListButs);
				Plugins.HookAddCode(this,"FormOpenDental.ProcessSignals_end",sigList);
			}
			catch {
				signalLastRefreshed=MiscData.GetNowDateTime();
			}
		}

		///<summary>Starts the eService monitoring thread that will run once a minute.  Only runs if the user currently logged in has the eServices permission.</summary>
		private void StartEServiceMonitoring() {
			//If the user currently logged in has permission to view eService settings, turn on the listener monitor.
			if(Security.CurUser==null || !Security.IsAuthorized(Permissions.EServicesSetup,true)) {
				return;//Do not start the listener service monitor for users without permission.
			}
			if(_odThreadEServices==null) {
				//Process any Error signals that happened due to an update:
				EServiceSignals.ProcessErrorSignalsAroundTime(PrefC.GetDateT(PrefName.ProgramVersionLastUpdated));
				//Create a separate thread that will run every 60 seconds to monitor eService signals.
				_odThreadEServices=new ODThread(60000,ProcessEServiceSignals);
				//Add exception handling just in case MySQL is unreachable at any point in the lifetime of this session.
				_odThreadEServices.AddExceptionHandler(EServiceMonitoringException);
				_odThreadEServices.Name="eService Monitoring Thread";
				_odThreadEServices.GroupName="eServiceThreads";
			}
			_odThreadEServices.Start();
		}

		///<summary>Stops the eService monitoring thread and sets the eServices menu item colors to a disabled state because the Log On window will be shown next.</summary>
		private void StopEServiceMonitoring() {
			if(_odThreadEServices==null) {
				return;//Nothing to do, the service was already stopped.
			}
			//QuitSync the thread just because it has the power to live for up to a minute which is unnecessary.
			//There is no reason to wait more than 1 second for the thread to quit.
			_odThreadEServices.QuitSync(1000);
			_odThreadEServices=null;
			//Set the background color of the menu item back to gray just in case it was red for the last user that was logged in.
			_colorEServicesBackground=SystemColors.Control;
			InvalidateEServicesMenuItem();//No need to invoke this method, we should always be in the main thread when stopping the thread.
		}

		///<summary>The exception delegate for the eService monitoring thread.</summary>
		private void EServiceMonitoringException(Exception ex) {
			//Currently we don't want to do anything if the eService signal processing fails.  Simply try again in a minute.  
			//Most likely cause for exceptions will be database IO when computers are just sitting around not doing anything.
			//Implementing this delegate allows us to NOT litter ProcessEServiceSignals() with try catches.  
		}

		///<summary>The exception delegate for the Podium monitoring thread.</summary>
		private void PodiumMonitoringException(Exception ex) {
			//Currently we don't want to do anything if the Podium processing fails.  
		}

		///<summary>Worker method for _odThreadEServices.  Call StartEServiceMonitoring() to start monitoring eService signals instead of calling this method directly.</summary>
		private void ProcessEServiceSignals(ODThread odThread) {
			//--------------------------------------------------------------------------------------------------------------------------------------------
			//This method can get enhanced later to be more generic like ProcessSignals where it grabs a list of eService signals and loops through them.
			//For now, we only care about the status of the Listener Service and I want the first edition of this method to be as light as possible.
			//--------------------------------------------------------------------------------------------------------------------------------------------
			Color colorCur=_colorEServicesBackground;
			//The listener service will have a local heartbeat every 5 minutes so it's overkill to check every time timerSignals_Tick fires.
 			//Only check the Listener Service status once a minute.
			//The downside to doing this is that the menu item will stay red up to one minute when a user wants to stop monitoring the service.
			eServiceSignalSeverity listenerStatus=EServiceSignals.GetListenerServiceStatus();
			if(listenerStatus==eServiceSignalSeverity.None) {
				//This office has never had a valid listener service running and does not have more than 5 patients set up to use the listener service.
				//Quit the thread so that this computer does not waste its time sending queries to the server every minute.
				odThread.QuitAsync();
				return;
			}
			if(listenerStatus==eServiceSignalSeverity.Critical) {
				_colorEServicesBackground=COLOR_ESERVICE_CRITICAL_BACKGROUND;
			}
			else if(listenerStatus==eServiceSignalSeverity.Error) {
				_colorEServicesBackground=COLOR_ESERVICE_ERROR_BACKGROUND;
			}
			else {
				_colorEServicesBackground=SystemColors.Control;
			}
			if(colorCur!=_colorEServicesBackground) {
				//Since the status changed, redraw the eServices menu item.
				BeginInvoke(new InvalidateEServicesMenuItemDelegate(InvalidateEServicesMenuItem));
			}
		}

		protected delegate void InvalidateEServicesMenuItemDelegate();

		///<summary>MenuItem does not have an invalidate or refresh so we quickly disable and enable the menu item so that the OwnerDraw methods get called.</summary>
		private void InvalidateEServicesMenuItem() {
			menuItemEServices.Enabled=false;
			menuItemEServices.Enabled=true;
			menuItemListenerService.Enabled=false;
			menuItemListenerService.Enabled=true;
		}

		public void TaskGoToEvent(object sender, CancelEventArgs e){
			FormTaskEdit FormT=(FormTaskEdit)sender;
			TaskGoTo(FormT.GotoType,FormT.GotoKeyNum);
		}

		///<summary>Gives users 15 seconds to finish what they were doing before the program shuts down.</summary>
		private void KillThread() {
			Thread.Sleep(15000);//15 seconds
			//We have to call ProcessKillCommand with an Invoke, so that the Application.Exit()
			//will run from the main thread and bypass all FormClosing events except 
			//FormOpenDental.FormClosing. We need to bypass the closing events so that the closing
			//events do not have the opportunity to prevent the program from closing.
			Invoke(new ProcessKillCommandDelegate(ProcessKillCommand));
		}

		///<summary></summary>
		protected delegate void ProcessKillCommandDelegate();

		///<summary></summary>
		public void ProcessKillCommand() {
			//It is crucial that every form be forcefully closed so that they do not stay connected to a database that has been updated to a more recent version.
			CloseOpenForms(true);
			Application.Exit();//This will call FormOpenDental's closing event which will clean up all threads that are currently running.
		}

		private void PlaySounds(Object objSignalList){
			List<Signalod> signalList=(List<Signalod>)objSignalList;
			string strSound;
			byte[] rawData;
			MemoryStream stream=null;
			SoundPlayer simpleSound=null;
			try {
				//loop through each signal
				for(int s=0;s<signalList.Count;s++) {
					if(signalList[s].AckTime.Year>1880) {
						continue;//don't play any sounds for acks.
					}
					if(s>0) {
						Thread.Sleep(1000);//pause 1 second between signals.
					}
					//play all the sounds.
					for(int e=0;e<signalList[s].ElementList.Length;e++) {
						strSound=SigElementDefs.GetElement(signalList[s].ElementList[e].SigElementDefNum).Sound;
						if(strSound=="") {
							continue;
						}
						try {
							rawData=Convert.FromBase64String(strSound);
							stream=new MemoryStream(rawData);
							simpleSound = new SoundPlayer(stream);
							simpleSound.PlaySync();//sound will finish playing before thread continues
						}
						catch {
							//do nothing
						}
					}
				}
			}
			finally {
				if(stream!=null) {
					stream.Dispose();
				}
				if(simpleSound!=null) {
					simpleSound.Dispose();
				}
			}
		}

		private void myOutlookBar_ButtonClicked(object sender, OpenDental.ButtonClicked_EventArgs e){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					if(!Security.IsAuthorized(Permissions.AppointmentsModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 1:
					if(PrefC.GetBool(PrefName.EhrEmergencyNow)) {//if red emergency button is on
						if(Security.IsAuthorized(Permissions.EhrEmergencyAccess,true)) {
							break;//No need to check other permissions.
						}
					}
					//Whether or not they were authorized by the special situation above,
					//they can get into the Family module with the ordinary permissions.
					if(!Security.IsAuthorized(Permissions.FamilyModule)) {
						e.Cancel=true;
						return;
					}
					break;
				case 2:
					if(!Security.IsAuthorized(Permissions.AccountModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 3:
					if(!Security.IsAuthorized(Permissions.TPModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 4:
					if(!Security.IsAuthorized(Permissions.ChartModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 5:
					if(!Security.IsAuthorized(Permissions.ImagesModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 6:
					if(!Security.IsAuthorized(Permissions.ManageModule)){
						e.Cancel=true;
						return;
					}
					break;
			}
			UnselectActive();
			allNeutral();
			SetModuleSelected(true);
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing.</summary>
		private void SetModuleSelected() {
			SetModuleSelected(false);
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing. The menuBarClicked variable should be set to true when a module button is clicked, and should be false when called for refresh purposes.</summary>
		private void SetModuleSelected(bool menuBarClicked){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					ContrAppt2.InitializeOnStartup();
					ContrAppt2.Visible=true;
					this.ActiveControl=this.ContrAppt2;
					ContrAppt2.ModuleSelected(CurPatNum);
					break;
				case 1:
					if(HL7Defs.IsExistingHL7Enabled()) {
						HL7Def def=HL7Defs.GetOneDeepEnabled();
						if(def.ShowDemographics==HL7ShowDemographics.Hide) {
							ContrFamily2Ecw.Visible=true;
							this.ActiveControl=this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else {
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible=true;
							this.ActiveControl=this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					else {
						if(Programs.UsingEcwTightMode()) {
							ContrFamily2Ecw.Visible=true;
							this.ActiveControl=this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else {
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible=true;
							this.ActiveControl=this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					break;
				case 2:
					ContrAccount2.InitializeOnStartup();
					ContrAccount2.Visible=true;
					this.ActiveControl=this.ContrAccount2;
					ContrAccount2.ModuleSelected(CurPatNum);
					break;
				case 3:
					ContrTreat2.InitializeOnStartup();
					ContrTreat2.Visible=true;
					this.ActiveControl=this.ContrTreat2;
					ContrTreat2.ModuleSelected(CurPatNum);
					break;
				case 4:
					ContrChart2.InitializeOnStartup();
					ContrChart2.Visible=true;
					this.ActiveControl=this.ContrChart2;
					if(menuBarClicked) {
						ContrChart2.ModuleSelectedNewCrop(CurPatNum);//Special refresh that also queries the NewCrop web service for prescription information.
					}
					else {
						ContrChart2.ModuleSelected(CurPatNum,true);
					}
					TryNonPatientPopup();
					break;
				case 5:
					ContrImages2.InitializeOnStartup();
					ContrImages2.Visible=true;
					this.ActiveControl=this.ContrImages2;
					ContrImages2.ModuleSelected(CurPatNum);
					break;
				case 6:
					//ContrManage2.InitializeOnStartup();//This gets done earlier.
					ContrManage2.Visible=true;
					this.ActiveControl=this.ContrManage2;
					ContrManage2.ModuleSelected(CurPatNum);
					break;
			}
		}

		private void allNeutral(){
			ContrAppt2.Visible=false;
			ContrFamily2.Visible=false;
			ContrFamily2Ecw.Visible=false;
			ContrAccount2.Visible=false;
			ContrTreat2.Visible=false;
			ContrChart2.Visible=false;
			ContrImages2.Visible=false;
			ContrManage2.Visible=false;
		}

		private void UnselectActive(){
			if(ContrAppt2.Visible){
				ContrAppt2.ModuleUnselected();
			}
			if(ContrFamily2.Visible){
				ContrFamily2.ModuleUnselected();
			}
			if(ContrFamily2Ecw.Visible) {
				//ContrFamily2Ecw.ModuleUnselected();
			}
			if(ContrAccount2.Visible){
				ContrAccount2.ModuleUnselected();
			}
			if(ContrTreat2.Visible){
				ContrTreat2.ModuleUnselected();
			}
			if(ContrChart2.Visible){
				ContrChart2.ModuleUnselected();
			}
			if(ContrImages2.Visible){
				ContrImages2.ModuleUnselected();
			}
		}

		///<Summary>This also passes CurPatNum down to the currently selected module (except the Manage module).</Summary>
		private void RefreshCurrentModule(){
			if(ContrAppt2.Visible){
				ContrAppt2.ModuleSelected(CurPatNum);
			}
			if(ContrFamily2.Visible){
				ContrFamily2.ModuleSelected(CurPatNum);
			}
			if(ContrFamily2Ecw.Visible) {
				ContrFamily2Ecw.ModuleSelected(CurPatNum);
			}
			if(ContrAccount2.Visible){
				ContrAccount2.ModuleSelected(CurPatNum);
			}
			if(ContrTreat2.Visible){
				ContrTreat2.ModuleSelected(CurPatNum);
			}
			if(ContrChart2.Visible){
				ContrChart2.ModuleSelected(CurPatNum,true);
			}
			if(ContrImages2.Visible){
				ContrImages2.ModuleSelected(CurPatNum);
			}
			if(ContrManage2.Visible){
				ContrManage2.ModuleSelected(CurPatNum);
			}
		}

		/// <summary>sends function key presses to the appointment module and chart module</summary>
		private void FormOpenDental_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(ContrAppt2.Visible && e.KeyCode>=Keys.F1 && e.KeyCode<=Keys.F12){
				ContrAppt2.FunctionKeyPress(e.KeyCode);
			}
			if(ContrChart2.Visible && e.KeyCode>=Keys.F1 && e.KeyCode<=Keys.F12) {
				ContrChart2.FunctionKeyPressContrChart(e.KeyCode);
			}
			Keys keys=e.KeyCode;
			//Ctrl-Alt-R is supposed to show referral window, but it doesn't work on some computers.
			if((e.Modifiers&Keys.Alt)==Keys.Alt
				&& (e.Modifiers&Keys.Control)==Keys.Control
				&& (e.KeyCode&Keys.R)==Keys.R
				&& CurPatNum!=0)
			{
				FormReferralsPatient FormRE=new FormReferralsPatient();
				FormRE.PatNum=CurPatNum;
				FormRE.ShowDialog();
			}
			//so we're also going to use Ctrl-X to show the referral window.
			if((e.Modifiers&Keys.Control)==Keys.Control
				&& (e.KeyCode&Keys.X)==Keys.X
				&& CurPatNum!=0) 
			{
				FormReferralsPatient FormRE=new FormReferralsPatient();
				FormRE.PatNum=CurPatNum;
				FormRE.ShowDialog();
			}
		}

		private void timerTimeIndic_Tick(object sender, System.EventArgs e){
			//every minute:
			if(WindowState!=FormWindowState.Minimized
				&& ContrAppt2.Visible){
				ContrAppt2.TickRefresh();
      }
		}

		private void timerSignals_Tick(object sender, System.EventArgs e) {
			DateTime dtInactive=dateTimeLastActivity+TimeSpan.FromMinutes((double)PrefC.GetInt(PrefName.SignalInactiveMinutes));
			if((double)PrefC.GetInt(PrefName.SignalInactiveMinutes)!=0 && DateTime.Now>dtInactive) {
				return;
			}
			//typically every 4 seconds:
			ProcessSignals();
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				lightSignalGrid1.SetConfs(PhoneConfs.GetAll());
			}
		}

		private void timerDisabledKey_Tick(object sender,EventArgs e) {
			//every 10 minutes:
			if(PrefC.GetBoolSilent(PrefName.RegistrationKeyIsDisabled,false)) {
				MessageBox.Show("Registration key has been disabled.  You are using an unauthorized version of this program.","Warning",
					MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
		}

		private void timerHeartBeat_Tick(object sender,EventArgs e) {
			//every 3 minutes:
			try {
				Computers.UpdateHeartBeat(Environment.MachineName,false);
			}
			catch { }
		}

		private void ThreadPodiumSendInvitations(ODThread worker) {
			//Only send invitations if the program link is enabled and the computer name is set to this computer.
			if(!Programs.IsEnabled(ProgramName.Podium)
				|| !ODEnvironment.IdIsThisComputer(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.Podium),"Enter your computer name or IP (required)"))) 
			{
				return;
			}
			//Get the last 60 minutes of appointments in case there was a delay in the thread interval that would've prevented the appointment from being 
			//found at exactly 40 minutes for new patients.
			List<Appointment> listApptsCur=Appointments.GetAppointmentsStartingWithinPeriod(DateTime.Now.AddMinutes(-41).AddMilliseconds(-_podiumIntervalMS),DateTime.Now.AddMinutes(-10));
			for(int i=0;i<listApptsCur.Count;i++) {
				Appointment apptCur=listApptsCur[i];
				if(apptCur.AptStatus==ApptStatus.Broken) {
					continue;
				}
				Patient patCur=Patients.GetPat(apptCur.PatNum);
				int podiumMinutes=apptCur.IsNewPatient?40:10;
				if(apptCur.AptDateTime.AddMinutes(podiumMinutes)>=DateTime.Now 	//Appointment within this tick interval
					&& apptCur.AptDateTime.AddMinutes(podiumMinutes)<=DateTime.Now.AddMilliseconds(_podiumIntervalMS))	
				{
					Podium.SendInvitation(patCur,apptCur.IsNewPatient);
				}
			}
		}

		private void ThreadEmailInbox_Receive() {
			try {
				EmailMessages.CreateCertificateStoresIfNeeded();
			}
			catch {
				//Probably a permission issue creating the stores.  Nothing we can do except explain in the manual.
			}
			//The EmailInboxCheckInterval preference defines the number of minutes between automatic inbox receiving.
			//Do not perform the first email inbox receive within the first EmailInboxCheckInterval minutes of program startup (thread startup).
			_isEmailThreadRunning=true;
			while(_isEmailThreadRunning) {
				_emailSleep.WaitOne(TimeSpan.FromMinutes(PrefC.GetInt(PrefName.EmailInboxCheckInterval)));
				if(!_isEmailThreadRunning) {
					break;
				}
				EmailAddress Address=EmailAddresses.GetByClinic(0);//Default for clinic/practice.
				if(Address.Pop3ServerIncoming=="") {
					continue;//Email address not setup.
				}
				if(!ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {
					continue;//If the email inbox computer name is not setup, or if the name does not match this computer, then do not get email from this computer.
				}
				try {
					EmailMessages.ReceiveFromInbox(0,Address);
				}
				catch(Exception ex) {
					//Do not tell the user, because it would be annoying to see an error every 30 seconds (if the server was down for instance).
					//Maybe we could log to the system EventViewer Application log someday if users complain. Keep in mind that the user can always manually go to Manage | Email Inbox to see the error message.
				}
			}
		}

		private void ThreadTimeSynch_Synch() {
			_isTimeSynchThreadRunning=true;
			while(_isTimeSynchThreadRunning) {
				if(!_isTimeSynchThreadRunning) {
					break;
				}
				NTPv4 ntp=new NTPv4();
				double nistOffset=double.MaxValue;
				try {
					nistOffset=ntp.getTime(PrefC.GetString(PrefName.NistTimeServerUrl));
				}
				catch { } //Invalid NIST Server URL
				if(nistOffset!=double.MaxValue) {
					//Did not timeout, or have invalid NIST server URL
					try {
						WindowsTime.SetTime(DateTime.Now.AddMilliseconds(nistOffset)); //Sets local machine time
					}
					catch { } //Error setting local machine time
				}
				_timeSynchSleep.WaitOne(TimeSpan.FromMinutes(240));
			}
		}

		///<summary>Gets the encrypted connection string for the Oracle database from a config file.</summary>
		public bool GetOraConfig() {
			if(!File.Exists("ODOraConfig.xml")) {
				return false;
			}
			try {
				XmlDocument document=new XmlDocument();
				document.Load("ODOraConfig.xml");
				XPathNavigator Navigator=document.CreateNavigator();
				XPathNavigator nav;
				nav=Navigator.SelectSingleNode("//DatabaseConnection");
				if(nav!=null) {
					connStr=nav.SelectSingleNode("ConnectionString").Value;
					key=nav.SelectSingleNode("Key").Value;
				}
				DataConnection.DBtype=DatabaseType.Oracle;
				return true;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		///<summary>Decrypt the connection string and try to connect to the database directly. Only called if using a connection string and ChooseDatabase is not to be shown. Must call GetOraConfig first.</summary>
		public bool TryWithConnStr() {
			OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
			try {
				if(connStr!=null) {
#if ORA_DB
					OD_CRYPTO.Decryptor crypto=new OD_CRYPTO.Decryptor();
					dconnStr=crypto.Decrypt(connStr,key);
					crypto=null;
					dcon.SetDb(dconnStr,"",DatabaseType.Oracle);
#endif
				}
				//a direct connection does not utilize lower privileges.
				RemotingClient.RemotingRole=RemotingRole.ClientDirect;
				return true;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		private void userControlTasks1_GoToChanged(object sender,EventArgs e) {
			TaskGoTo(userControlTasks1.GotoType,userControlTasks1.GotoKeyNum);
		}

		private void TaskGoTo(TaskObjectType taskOT,long keyNum){
			if(taskOT==TaskObjectType.None) {
				return;
			}
			if(taskOT==TaskObjectType.Patient) {
				if(keyNum!=0) {
					CurPatNum=keyNum;
					Patient pat=Patients.GetPat(CurPatNum);
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			if(taskOT==TaskObjectType.Appointment) {
				if(keyNum!=0) {
					Appointment apt=Appointments.GetOneApt(keyNum);
					if(apt==null) {
						MsgBox.Show(this,"Appointment has been deleted, so it's not available.");
						return;
					}
					DateTime dateSelected=DateTime.MinValue;
					if(apt.AptStatus==ApptStatus.Planned || apt.AptStatus==ApptStatus.UnschedList) {
						//I did not add feature to put planned or unsched apt on pinboard.
						MsgBox.Show(this,"Cannot navigate to appointment.  Use the Other Appointments button.");
						//return;
					}
					else {
						dateSelected=apt.AptDateTime;
					}
					CurPatNum=apt.PatNum;//OnPatientSelected(apt.PatNum);
					GotoModule.GotoAppointment(dateSelected,apt.AptNum);
				}
			}
		}

		private void butBigPhones_Click(object sender,EventArgs e) {
			if(formPhoneTiles==null || formPhoneTiles.IsDisposed) {
				formPhoneTiles=new FormPhoneTiles();
				formPhoneTiles.GoToChanged += new System.EventHandler(this.phonePanel_GoToChanged);
				formPhoneTiles.Show();
				Rectangle rect=System.Windows.Forms.Screen.FromControl(this).Bounds;
				formPhoneTiles.Location=new Point((rect.Width-formPhoneTiles.Width)/2+rect.X,10);
				formPhoneTiles.BringToFront();
			}
			else {
				formPhoneTiles.Show();
				formPhoneTiles.BringToFront();
			}
		}

		private void butMapPhones_Click(object sender,EventArgs e) {
			if(formMapHQ==null || formMapHQ.IsDisposed) {
				formMapHQ=new FormMapHQ();
				formMapHQ.Show();
				formMapHQ.BringToFront();
			}
			else {
				formMapHQ.Show();
				formMapHQ.BringToFront();
			}
		}

		private void butTriage_Click(object sender,EventArgs e) {
			ContrManage2.JumpToTriageTaskWindow();
		}

		private void phonePanel_GoToChanged(object sender,EventArgs e) {
			if(formPhoneTiles.GotoPatNum!=0) {
				CurPatNum=formPhoneTiles.GotoPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
		}

		private void phoneSmall_GoToChanged(object sender,EventArgs e) {
			if(phoneSmall.GotoPatNum==0) {
				return;
			}
			CurPatNum=phoneSmall.GotoPatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
			Commlog commlog=Commlogs.GetIncompleteEntry(Security.CurUser.UserNum,CurPatNum);
			PhoneEmpDefault ped=PhoneEmpDefaults.GetByExtAndEmp(phoneSmall.Extension,Security.CurUser.EmployeeNum);
			if(ped!=null && ped.IsTriageOperator) {
				if(Plugins.HookMethod(this,"FormOpenDental.phoneSmall_GoToChanged_IsTriage",pat,phoneSmall.Extension)) {
					return;
				}
				Task task=new Task();
				task.TaskListNum=-1;//don't show it in any list yet.
				Tasks.Insert(task);
				Task taskOld=task.Copy();
				task.KeyNum=CurPatNum;
				task.ObjectType=TaskObjectType.Patient;
				task.TaskListNum=1697;//Hardcoded for internal Triage task list.
				task.UserNum=Security.CurUser.UserNum;
				task.Descript=Phones.GetPhoneForExtension(Phones.GetPhoneList(),phoneSmall.Extension).CustomerNumberRaw+" ";//Prefill description with customers number.
				FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
				FormTE.IsNew=true;
				FormTE.Show();
			}
			else {//Not a triage operator.
				if(Plugins.HookMethod(this,"FormOpenDental.phoneSmall_GoToChanged_NotTriage",pat)) {
					return;
				}
				if(commlog==null) {
					commlog = new Commlog();
					commlog.PatNum = CurPatNum;
					commlog.CommDateTime = DateTime.Now;
					commlog.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
					commlog.Mode_=CommItemMode.Phone;
					commlog.SentOrReceived=CommSentOrReceived.Received;
					commlog.UserNum=Security.CurUser.UserNum;
					FormCommItem FormCI=new FormCommItem(commlog);
					FormCI.IsNew = true;
					FormCI.ShowDialog();
					if(FormCI.DialogResult==DialogResult.OK) {
						RefreshCurrentModule();
					}
				}
				else {
					FormCommItem FormCI=new FormCommItem(commlog);
					FormCI.ShowDialog();
					if(FormCI.DialogResult==DialogResult.OK) {
						RefreshCurrentModule();
					}
				}
			}
		}

		private delegate void DelegateSetString(bool hasError,int voiceMailCount,TimeSpan ageOfOldestVoicemail);//typically at namespace level rather than class level

		///<summary>Always called using ThreadVM.</summary>
		private void ThreadVM_SetLabelMsg() {
#if DEBUG
      //Because path is not valid when Jordan is debugging from home.
#else
			while(true) {
				try {
					DirectoryInfo di=new DirectoryInfo(PhoneUI.PathPhoneMsg); //use this directory if you want to test with real files ===> @"\\10.10.1.197\Voicemail\default\998\Deleted"
					if(!di.Exists) {
						throw new Exception();//Causes the thread to sleep for 4 minutes then try again.
					}
					DateTime oldestVoicemail=DateTime.MaxValue;
					FileInfo[] fileInfos=di.GetFiles("*.txt");
					for(int i=0;i<fileInfos.Length;i++) {
						if(fileInfos[i].CreationTime<oldestVoicemail) {
							oldestVoicemail=fileInfos[i].CreationTime;
						}
					}
					TimeSpan ageOfOldestVoicemail=new TimeSpan(0);
					if(oldestVoicemail!=DateTime.MaxValue) {
						ageOfOldestVoicemail=DateTime.Now-oldestVoicemail;
					}
					this.Invoke(new DelegateSetString(SetVoicemailMetrics),new Object[] { false,fileInfos.Length,ageOfOldestVoicemail });					
				}
				catch(ThreadAbortException) {//OnClosing will abort the thread.
					return;//Exits the loop.
				}
				catch(Exception ex) {
					//Something went wrong with determining how many voicemails there are.  Sleep for 4 minutes than try again.
					this.Invoke(new DelegateSetString(SetVoicemailMetrics),new Object[] { true,0,new TimeSpan(0) });
					Thread.Sleep(240000);//4 minutes
					continue;
				}
				Thread.Sleep(3000);
			}
#endif
		}

		///<summary>Called from worker thread using delegate and Control.Invoke</summary>
		private void SetVoicemailMetrics(bool hasError,int voiceMailCount,TimeSpan ageOfOldestVoicemail) {
			if(hasError) {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,8.25f,FontStyle.Bold);
				labelMsg.Text="error";
				labelMsg.ForeColor=Color.Firebrick;
				return;
			}
			labelMsg.Text="V:"+voiceMailCount.ToString();
			if(voiceMailCount==0) {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,7.75f,FontStyle.Regular);
				labelMsg.ForeColor=Color.Black;
			}
			else {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,7.75f,FontStyle.Bold);
				labelMsg.ForeColor=Color.Firebrick;
			}
			if(formMapHQ!=null && !formMapHQ.IsDisposed) {				
				formMapHQ.SetVoicemailRed(voiceMailCount,ageOfOldestVoicemail);
			}
			if(formPhoneTiles!=null && !formPhoneTiles.IsDisposed) {
				formPhoneTiles.SetVoicemailCount(voiceMailCount);
			}
		}

		/*private void moduleStaffBilling_GoToChanged(object sender,GoToEventArgs e) {
			if(e.PatNum==0) {
				return;
			}
			CurPatNum=e.PatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(CurPatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
		}*/

		///<summary>This is recursive</summary>
		private void TranslateMenuItem(MenuItem menuItem){
			//first translate any child menuItems
			foreach(MenuItem mi in menuItem.MenuItems){
				TranslateMenuItem(mi);
			}
			//then this menuitem
			Lan.C("MainMenu",menuItem);
		}

		#region MenuEvents
		private void menuItemLogOff_Click(object sender, System.EventArgs e) {
			LogOffNow(false);
		}

		#region File

		//File
		private void menuItemPassword_Click(object sender,EventArgs e) {
			//no security blocking because everyone is allowed to change their own password.
			if(Security.CurUser.UserNumCEMT!=0) {
				MsgBox.Show(this,"Use the CEMT tool to change your password.");
				return;
			}
			FormUserPassword FormU=new FormUserPassword(false,Security.CurUser.UserName);
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.Cancel) {
				return;
			}
			Security.CurUser.Password=FormU.HashedResult;
			if(PrefC.GetBool(PrefName.PasswordsMustBeStrong)) {
				Security.CurUser.PasswordIsStrong=true;
			}
			try {
				Userods.UpdatePassword(Security.CurUser);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			DataValid.SetInvalid(InvalidType.Security);
		}

		private void menuItemPrinter_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPrinterSetup FormPS=new FormPrinterSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Printers");
		}

		private void menuItemGraphics_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FormGraphics fg=new FormGraphics();
			fg.ShowDialog();
			Cursor=Cursors.Default;
			if(fg.DialogResult==DialogResult.OK) {
				ContrChart2.InitializeLocalData();
				RefreshCurrentModule();
			}
		}

		private void menuItemConfig_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ChooseDatabase)){
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.ChooseDatabase,0,"");//make the entry before switching databases.
			FormChooseDatabase FormC=new FormChooseDatabase();//Choose Database is read only from this menu.
			FormC.IsAccessedFromMainMenu=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK){
				return;
			}
			CurPatNum=0;
			RefreshCurrentModule();//clumsy but necessary. Sets child PatNums to 0.
			if(!PrefsStartup()){
				return;
			}
			RefreshLocalData(InvalidType.AllLocal);
			//RefreshCurrentModule();
			menuItemLogOff_Click(this,e);//this is a quick shortcut.
		}

		private void menuItemExit_Click(object sender, System.EventArgs e) {
			//Thread2.Abort();
			//if(this.TcpListener2!=null){
			//	this.TcpListener2.Stop();  
			//}
			Application.Exit();
		}

		#endregion

		#region Setup

		//FormBackupJobsSelect FormBJS=new FormBackupJobsSelect();
		//FormBJS.ShowDialog();	

		//Setup
		private void menuItemApptFieldDefs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptFieldDefs FormA=new FormApptFieldDefs();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Field Defs");
		}

		private void menuItemApptRules_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptRules FormA=new FormApptRules();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Rules");
		}

		private void menuItemApptTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptTypes FormA=new FormApptTypes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Types");
		}

		private void menuItemApptViews_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormApptViews FormAV=new FormApptViews();
			FormAV.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Views");
		}

		private void menuItemAutoCodes_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAutoCode FormAC=new FormAutoCode();
			FormAC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Auto Codes");
		}

		private void menuItemAutomation_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAutomation FormA=new FormAutomation();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automation");
		}

		private void menuItemAutoNotes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAutoNotes FormA=new FormAutoNotes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Auto Notes");
		}

		private void menuItemClaimForms_Click(object sender, System.EventArgs e) {
			if(!PrefC.AtoZfolderUsed){
				MsgBox.Show(this,"Claim Forms feature is unavailable when data path A to Z folder is disabled.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClaimForms FormCF=new FormClaimForms();
			FormCF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Claim Forms");
		}

		private void menuItemClearinghouses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClearinghouses FormC=new FormClearinghouses();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clearinghouses");
		}

		private void menuItemComputers_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormComputers FormC=new FormComputers();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Computers");
		}

		private void menuItemDataPath_Click(object sender, System.EventArgs e) {
			//security is handled from within the form.
			FormPath FormP=new FormPath();
			FormP.ShowDialog();
			CheckCustomReports();
			this.RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Data Path");	
		}

		private void menuItemDefaultCCProcs_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefaultCCProcs FormD=new FormDefaultCCProcs();
			FormD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Default CC Procedures");
		}

		private void menuItemDefinitions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.AccountColors);//just the first cat.
			FormD.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
		}

		private void menuItemDentalSchools_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDentalSchoolSetup FormDSS=new FormDentalSchoolSetup();
			FormDSS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental Schools");
		}

		private void menuItemDisplayFields_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDisplayFieldCategories FormD=new FormDisplayFieldCategories();
			FormD.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Display Fields");
		}

		private void menuItemEmail_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmailAddresses FormEA=new FormEmailAddresses();
			FormEA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Email");
		}

		private void menuItemEHR_Click(object sender,EventArgs e) {
			//if(!Security.IsAuthorized(Permissions.Setup)) {
			//  return;
			//}
			FormEhrSetup FormE=new FormEhrSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"EHR");
		}

		private void menuItemFeeScheds_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormFeeScheds FormF=new FormFeeScheds();
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Fee Schedules");
		}

		private void menuItemHL7_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormHL7Defs FormH=new FormHL7Defs();
			FormH.CurPatNum=CurPatNum;
			FormH.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"HL7");
		}

		private void menuItemImaging_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormImagingSetup FormI=new FormImagingSetup();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Imaging");
		}

		private void menuItemInsCats_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInsCatsSetup FormE=new FormInsCatsSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Categories");
		}

		private void menuItemInsFilingCodes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormInsFilingCodes FormF=new FormInsFilingCodes();
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Filing Codes");
		}

		private void menuItemLaboratories_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormLaboratories FormL=new FormLaboratories();
			FormL.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Laboratories");
		}

		private void menuItemLetters_Click(object sender,EventArgs e) {
			FormLetters FormL=new FormLetters();
			FormL.ShowDialog();
		}

		private void menuItemMessaging_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingSetup FormM=new FormMessagingSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMessagingButs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingButSetup FormM=new FormMessagingButSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMisc_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormMisc FormM=new FormMisc();
			if(FormM.ShowDialog()!=DialogResult.OK) {
				return;
			}
			RecursiveInvalidate(this);
			//this.Invalidate();
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			//menuItemMergeDatabases.Visible=PrefC.GetBool(PrefName.RandomPrimaryKeys");
			//if(timerSignals.Interval==0){
			if(PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)==0){
				timerSignals.Enabled=false;
			}
			else{
				timerSignals.Interval=PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)*1000;
				timerSignals.Enabled=true;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Misc");
		}

		///<summary>Only used when blue theme is changed.</summary>
		private void RecursiveInvalidate(Control control){
			foreach(Control c in control.Controls) {
				RecursiveInvalidate(c);
			}
			control.Invalidate();
		}

		private void menuItemModules_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(0);//Default to Appts tab.
		}

		private void menuItemPreferencesAppts_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(0);
		}

		private void menuItemPreferencesFamily_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(1);
		}

		private void menuItemPreferencesAccount_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(2);
		}

		private void menuItemPreferencesTreatPlan_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(3);
		}

		private void menuItemPreferencesChart_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(4);
		}

		private void menuItemPreferencesImages_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(5);
		}

		private void menuItemPreferencesManage_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(6);
		}

		///<summary>Checks setup permission, launches the module setup window with the specified tab and then makes an audit entry.
		///This is simply a helper method because every preferences menu item will do the exact same code.</summary>
		private void LaunchModuleSetupWithTab(int selectedTab) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormModuleSetup FormM=new FormModuleSetup(selectedTab);
			if(FormM.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillPatientButton(Patients.GetPat(CurPatNum));
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Modules");
		}

		private void menuItemOperatories_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories");
		}

		private void menuItemPatFieldDefs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPatFieldDefs FormP=new FormPatFieldDefs();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Patient Field Defs");
		}

		private void menuItemPayerIDs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormElectIDs FormE=new FormElectIDs();
			FormE.IsSelectMode=false;
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Payer IDs");
		}

		private void menuItemPractice_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPractice FormPr=new FormPractice();
			FormPr.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Practice Info");
			RefreshCurrentModule();
		}

		private void menuItemProblems_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDiseaseDefs FormD=new FormDiseaseDefs();
			FormD.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Disease Defs");
		}

		private void menuItemProcedureButtons_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProcButtons FormPB=new FormProcButtons();
			FormPB.Owner=this;
			FormPB.ShowDialog();
			SetModuleSelected();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Procedure Buttons");	
		}

		private void menuItemLinks_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProgramLinks FormPL=new FormProgramLinks();
			FormPL.ShowDialog();
			ContrChart2.InitializeLocalData();//for eCW
			LayoutToolBar();
			if(CurPatNum>0) {
				Patient pat=Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Program Links");
		}

		/*
		private void menuItem_ProviderAllocatorSetup_Click(object sender,EventArgs e) {
			// Check Permissions
			if(!Security.IsAuthorized(Permissions.Setup)) {
				// Failed security prompts message box. Consider adding overload to not show message.
				//MessageBox.Show("Not Authorized to Run Setup for Provider Allocation Tool");
				return;
			}
			Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider fap = new OpenDental.Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider();
			fap.ShowDialog();
		}*/

		private void menuItemQuestions_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormQuestionDefs FormQ=new FormQuestionDefs();
			FormQ.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Questionnaire");
		}

		private void menuItemRecall_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRecallSetup FormRS=new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall");	
		}

		private void menuItemRecallTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRecallTypes FormRT=new FormRecallTypes();
			FormRT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Types");	
		}

		private void menuItemReplication_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormReplicationSetup FormRS=new FormReplicationSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Replication setup.");
		}

		private void menuItemRequirementsNeeded_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormReqNeededs FormR=new FormReqNeededs();
			FormR.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Requirements Needed");
		}

		private void menuItemSched_Click(object sender,EventArgs e) {
			//anyone should be able to view. Security must be inside schedule window.
			//if(!Security.IsAuthorized(Permissions.Schedules)) {
			//	return;
			//}
			FormSchedule FormS=new FormSchedule();
			FormS.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
		}

		/*private void menuItemBlockoutDefault_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Blockout);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Default");
		}*/

		private void menuItemSecurity_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormSecurity FormS=new FormSecurity();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"");
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {//clinics not enabled, refresh current module and return
				RefreshCurrentModule();
				return;
			}
			//clinics is enabled
			if(Security.CurUser.ClinicIsRestricted) {
				ClinicNum=Security.CurUser.ClinicNum;
			}
			Clinics.ClinicNum=ClinicNum;
			Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),ClinicNum);
			RefreshMenuClinics();//this calls ModuleSelected, so no need to call RefreshCurrentModule
		}

		private void menuItemSheets_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSheetDefs FormSD=new FormSheetDefs();
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sheets");
		}

		//This shows as "Show Features"
		private void menuItemEasy_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormShowFeatures FormE=new FormShowFeatures();
			FormE.ShowDialog();
			ContrAccount2.LayoutToolBar();//for repeating charges
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Show Features");
		}

		private void menuItemSpellCheck_Click(object sender,EventArgs e) {
			FormSpellCheck FormD=new FormSpellCheck();
			FormD.ShowDialog();
		}

		private void menuItemTimeCards_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTimeCardSetup FormTCS=new FormTimeCardSetup();
			FormTCS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Time Card Setup");
		}

		#endregion

		#region Lists

		//Lists
		private void menuItemProcCodes_Click(object sender, System.EventArgs e) {
			//security handled within form
			FormProcCodes FormP=new FormProcCodes();
			FormP.ShowDialog();	
		}

		private void menuItemAllergies_Click(object sender,EventArgs e) {
			new FormAllergySetup().ShowDialog();
		}

		private void menuItemClinics_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClinics FormC=new FormClinics();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clinics");
			//this menu item is only visible if the clinics show feature is enabled (!EasyNoClinics)
			if(Clinics.GetDesc(ClinicNum)=="") {//will be empty string if ClinicNum is not valid, in case they deleted the clinic
				ClinicNum=Security.CurUser.ClinicNum;
				Clinics.ClinicNum=ClinicNum;
				Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),ClinicNum);
			}
			RefreshMenuClinics();
			//reset the main title bar in case the user changes the clinic description for the selected clinic
			Patient pat=Patients.GetPat(CurPatNum);
			Text=PatientL.GetMainTitle(pat,ClinicNum);
			//reset the tip text in case the user changes the clinic description
		}
		
		private void menuItemContacts_Click(object sender, System.EventArgs e) {
			FormContacts FormC=new FormContacts();
			FormC.ShowDialog();
		}

		private void menuItemCounties_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormCounties FormC=new FormCounties();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Counties");
		}

		private void menuItemSchoolClass_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolClasses FormS=new FormSchoolClasses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Classes");
		}

		private void menuItemSchoolCourses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolCourses FormS=new FormSchoolCourses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Courses");
		}

		private void menuItemEmployees_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmployeeSelect FormEmp=new FormEmployeeSelect();
			FormEmp.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Employees");	
		}

		private void menuItemEmployers_Click(object sender, System.EventArgs e) {
			FormEmployers FormE=new FormEmployers();
			FormE.ShowDialog();
		}

		private void menuItemInstructors_Click(object sender, System.EventArgs e) {
			/*if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInstructors FormI=new FormInstructors();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Instructors");*/
		}

		private void menuItemCarriers_Click(object sender, System.EventArgs e) {
			FormCarriers FormC=new FormCarriers();
			FormC.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemInsPlans_Click(object sender, System.EventArgs e) {
			FormInsPlans FormIP = new FormInsPlans();
			FormIP.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemLabCases_Click(object sender,EventArgs e) {
			FormLabCases FormL=new FormLabCases();
			FormL.ShowDialog();
			if(FormL.GoToAptNum!=0) {
				Appointment apt=Appointments.GetOneApt(FormL.GoToAptNum);
				Patient pat=Patients.GetPat(apt.PatNum);
				PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(pat);
				//if(PatientSelected!=null){
				//	PatientSelected(this,eArgs);
				//}
				Contr_PatientSelected(this,eArgs);
				//OnPatientSelected(pat.PatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
				GotoModule.GotoAppointment(apt.AptDateTime,apt.AptNum);
			}
		}

		private void menuItemMedications_Click(object sender, System.EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.ShowDialog();
		}

		private void menuItemPharmacies_Click(object sender,EventArgs e) {
			FormPharmacies FormP=new FormPharmacies();
			FormP.ShowDialog();
		}

		private void menuItemProviders_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Providers,true) && !Security.IsAuthorized(Permissions.AdminDentalStudents,true)) {
				MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"
					+GroupPermissions.GetDesc(Permissions.Providers)+" "+Lans.g("Security","or")+" "+GroupPermissions.GetDesc(Permissions.AdminDentalStudents));
				return;
			}
			FormProviderSetup FormPS=new FormProviderSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Providers");		
		}

		private void menuItemPrescriptions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRxSetup FormRxSetup2=new FormRxSetup();
			FormRxSetup2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rx");		
		}

		private void menuItemReferrals_Click(object sender, System.EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.ShowDialog();		
		}

		private void menuItemSites_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSites FormS=new FormSites();
			FormS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sites");
		}

		private void menuItemZipCodes_Click(object sender, System.EventArgs e) {
			//if(!Security.IsAuthorized(Permissions.Setup)){
			//	return;
			//}
			FormZipCodes FormZ=new FormZipCodes();
			FormZ.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Setup,"Zip Codes");
		}

		#endregion

		#region Reports

		private void menuItemReports_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)) {
				return;
			}
			FormReportsMore FormR=new FormReportsMore();
			FormR.ShowDialog();
			if(FormR.RpModalSelection==ReportModalSelection.TreatmentFinder) {
				FormRpTreatmentFinder FormT=new FormRpTreatmentFinder();
				FormT.Show();
			}
			if(FormR.RpModalSelection==ReportModalSelection.OutstandingIns) {
				FormRpOutstandingIns FormOI=new FormRpOutstandingIns();
				FormOI.Show();
			}
		}

		#endregion

		#region CustomReports

		//Custom Reports
		private void menuItemRDLReport_Click(object sender,System.EventArgs e) {
			//This point in the code is only reached if the A to Z folders are enabled, thus
			//the image path should exist.
			FormReportCustom FormR=new FormReportCustom();
			FormR.SourceFilePath=
				ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),PrefC.GetString(PrefName.ReportFolderName),((MenuItem)sender).Text+".rdl");
			FormR.ShowDialog();
		}

		#endregion

		#region Tools

		//Tools
		private void menuItemPrintScreen_Click(object sender, System.EventArgs e) {
			FormPrntScrn FormPS=new FormPrntScrn();
			FormPS.ShowDialog();
		}

		//MiscTools
		private void menuItemDuplicateBlockouts_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormBlockoutDuplicatesFix form=new FormBlockoutDuplicatesFix();
			form.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clear duplicate blockouts.");
		}

		private void menuItemCreateAtoZFolders_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAtoZFoldersCreate FormA=new FormAtoZFoldersCreate();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK){
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Created AtoZ Folder");
			}
		}

		private void menuItemImportXML_Click(object sender,System.EventArgs e) {
			FormImportXML FormI=new FormImportXML();
			FormI.ShowDialog();
		}

		private void menuItemMergePatients_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PatientMerge)) {
				return;
			}
			FormPatientMerge fpm=new FormPatientMerge();
			fpm.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemProcLockTool_Click(object sender,EventArgs e) {
			FormProcLockTool FormT=new FormProcLockTool();
			FormT.ShowDialog();
			//security entries made inside the form
			//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Proc Lock Tool");
		}

		private void menuItemShutdown_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormShutdown FormS=new FormShutdown();
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			//turn off signal reception for 5 seconds so this workstation will not shut down.
			signalLastRefreshed=MiscData.GetNowDateTime().AddSeconds(5);
			Signalod sig=new Signalod();
			sig.ITypes=((int)InvalidType.ShutDownNow).ToString();
			sig.SigType=SignalType.Invalid;
			Signalods.Insert(sig);
			Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");
		}
		
		private void menuTelephone_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormTelephone FormT=new FormTelephone();
			FormT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Telephone");
		}

		private void menuItemTestLatency_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTestLatency formTL=new FormTestLatency();
			formTL.ShowDialog();
		}
		
		private void menuItemXChargeReconcile_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Accounting)) {
				return;
			}
			FormXChargeReconcile FormXCR=new FormXChargeReconcile();
			FormXCR.ShowDialog();
		}

		//End of MiscTools, resume Tools.
		private void menuItemAging_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAging FormAge=new FormAging();
			FormAge.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Aging Update");
		}

		private void menuItemAuditTrail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormAudit FormA=new FormAudit();
			FormA.CurPatNum=CurPatNum;
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Audit Trail");
		}

		private void menuItemFinanceCharge_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormFinanceCharges FormFC=new FormFinanceCharges();
			FormFC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Run Finance Charges");
		}

		private void menuItemCCRecurring_Click(object sender,EventArgs e) {
			if(FormCRC==null || FormCRC.IsDisposed) {
				FormCRC=new FormCreditRecurringCharges();
			}
			FormCRC.Show();
			if(FormCRC.WindowState==FormWindowState.Minimized) {
				FormCRC.WindowState=FormWindowState.Normal;
			}
			FormCRC.BringToFront();
		}

		private void menuItemCustomerManage_Click(object sender,EventArgs e) {
			FormCustomerManagement FormC=new FormCustomerManagement();
			FormC.ShowDialog();
			if(FormC.SelectedPatNum!=0) {
				CurPatNum=FormC.SelectedPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
		}

		private void menuItemDatabaseMaintenance_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDatabaseMaintenance FormDM=new FormDatabaseMaintenance();
			FormDM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Database Maintenance");
		}

		private void menuItemDispensary_Click(object sender,System.EventArgs e) {
			FormDispensary FormD=new FormDispensary();
			FormD.ShowDialog();
		}

		private void menuItemEvaluations_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdminDentalEvaluations,true) && (Security.CurUser.ProvNum==0 || Providers.GetProv(Security.CurUser.ProvNum).SchoolClassNum!=0)) {
				MsgBox.Show(this,"Only Instructors may view or edit evaluations.");
				return;
			}
			FormEvaluations FormE=new FormEvaluations();
			FormE.ShowDialog();
		}

		private void menuItemTerminal_Click(object sender,EventArgs e) {
			FormTerminal FormT=new FormTerminal();
			FormT.ShowDialog(); 
			Application.Exit();//always close after coming out of terminal mode as a safety precaution.*/
		}

		private void menuItemTerminalManager_Click(object sender,EventArgs e) {
			if(formTerminalManager==null || formTerminalManager.IsDisposed) {
				formTerminalManager=new FormTerminalManager();
			}
			formTerminalManager.Show();
			formTerminalManager.BringToFront();
		}

		private void menuItemTranslation_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTranslationCat FormTC=new FormTranslationCat();
			FormTC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Translations");
		}

		private void menuItemMobileSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			//MessageBox.Show("Not yet functional.");
			new FormEServicesSetup(FormEServicesSetup.EService.MobileOld).ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Mobile Sync");
		}

		private void menuItemNewCropBilling_Click(object sender,EventArgs e) {
			FormNewCropBilling FormN=new FormNewCropBilling();
			FormN.ShowDialog();
		}

		private void menuItemRepeatingCharges_Click(object sender, System.EventArgs e) {
			FormRepeatChargesUpdate FormR=new FormRepeatChargesUpdate();
			FormR.ShowDialog();
		}

		private void menuItemResellers_Click(object sender,EventArgs e) {
			FormResellers FormR=new FormResellers();
			FormR.ShowDialog();
		}

		private void menuItemScreening_Click(object sender,System.EventArgs e) {
			FormScreenGroups FormS=new FormScreenGroups();
			FormS.ShowDialog();
		}

		private void menuItemReqStudents_Click(object sender,EventArgs e) {
			Provider prov=Providers.GetProv(Security.CurUser.ProvNum);
			if(prov==null) {
				MsgBox.Show(this,"The current user is not attached to a provider. Attach the user to a provider to gain access to this feature.");
				return;
			}
			if(!prov.IsInstructor){//if a student is logged in
				//the student always has permission to view their own requirements
				FormReqStudentOne FormO=new FormReqStudentOne();
				FormO.ProvNum=prov.ProvNum;
				FormO.ShowDialog();
				return;
			}
			if(prov.IsInstructor) {
				FormReqStudentsMany FormM=new FormReqStudentsMany();
				FormM.ShowDialog();
			}
		}

		private void menuItemWebForms_Click(object sender,EventArgs e) {
			FormWebForms FormWF = new FormWebForms();
			FormWF.Show();
		}

		private void menuItemWiki_Click(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormOpenDental.menuItemWiki_Click")) {
				return;
			}
			if(FormMyWiki==null || FormMyWiki.IsDisposed) {
				FormMyWiki=new FormWiki();
			}
			FormMyWiki.Show();
			if(FormMyWiki.WindowState==FormWindowState.Minimized) {
				FormMyWiki.WindowState=FormWindowState.Normal;
			}
			FormMyWiki.BringToFront();
		}

		#endregion

		#region Clinics
		//menuClinics is a dynamic menu that is maintained within RefreshMenuClinics()
		#endregion

		#region eServices

		private void menuItemEServices_DrawItem(object sender,DrawItemEventArgs e) {
			//Get the text that is displaying from the menu item compenent.
			MenuItem menuItem=(MenuItem)sender;
			Color colorText=Color.White;
			if(_colorEServicesBackground!=COLOR_ESERVICE_CRITICAL_BACKGROUND) {
				//Always default the background and text colors to gray if the current eServices status is not critical.
				colorText=SystemColors.MenuText;
				//Only change the background to "control" if the status is not of Error.
				if(_colorEServicesBackground!=COLOR_ESERVICE_ERROR_BACKGROUND) {
					_colorEServicesBackground=SystemColors.Control;
				}
			}
			//Check if disabled or inactive (other app has focus).
			if(!menuItem.Enabled || e.State==(DrawItemState.NoAccelerator | DrawItemState.Inactive)) {
				colorText=SystemColors.ControlDark;
			}
			//Check if selected or hovering over.
			if(e.State==(DrawItemState.NoAccelerator | DrawItemState.Selected) 
				|| e.State==(DrawItemState.NoAccelerator | DrawItemState.HotLight)) 
			{
				if(_colorEServicesBackground==COLOR_ESERVICE_CRITICAL_BACKGROUND) {
					colorText=COLOR_ESERVICE_CRITICAL_TEXT;
				}
				else if(_colorEServicesBackground==COLOR_ESERVICE_ERROR_BACKGROUND) {
					colorText=COLOR_ESERVICE_ERROR_TEXT;
				}
				else {//Service not critical or has an error.
					//Color the background of the menu item the system's "menu hover" color when a user hovers or has clicked on the menu item.
					_colorEServicesBackground=SystemColors.Highlight;
					colorText=SystemColors.HighlightText;
				}
			}
			using(SolidBrush brushBackground=new SolidBrush(_colorEServicesBackground))
			using(SolidBrush brushFont=new SolidBrush(colorText)) {
				//Get the text that is displaying from the menu item compenent.
				string menuText=menuItem.Text;
				//Create a string format to center the text to mimic the other menu items.
				StringFormat stringFormat=new StringFormat();
				stringFormat.Alignment=StringAlignment.Center;
				e.Graphics.FillRectangle(brushBackground,e.Bounds);
				//We use the standard menu font so that the font on this one menu item will match the rest of the menu.
				e.Graphics.DrawString(menuText,SystemInformation.MenuFont,brushFont,e.Bounds,stringFormat);
			}
		}

		private void menuItemEServices_MeasureItem(object sender,MeasureItemEventArgs e) {
			//Measure the text showing.
			MenuItem menuItem=(MenuItem)sender;
			Size sizeString=TextRenderer.MeasureText(menuItem.Text,SystemInformation.MenuFont);
			e.ItemWidth=sizeString.Width;
			e.ItemHeight=sizeString.Height;
		}

		private void menuItemListenerService_MeasureItem(object sender,MeasureItemEventArgs e) {
			//Measure the text showing.
			MenuItem menuItem=(MenuItem)sender;
			Size sizeString=TextRenderer.MeasureText(menuItem.Text,SystemInformation.MenuFont);
			e.ItemWidth=sizeString.Width;
			e.ItemHeight=sizeString.Height+5;//Pad the bottom
		}

		private void menuItemMobileSync_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.MobileOld);
			FormESS.ShowDialog();
		}

		private void menuMobileWeb_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.MobileNew);
			FormESS.ShowDialog();
		}

		private void menuItemPatientPortal_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.PatientPortal);
			FormESS.ShowDialog();
		}

		private void menuItemIntegratedTexting_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.SmsService);
			FormESS.ShowDialog();
		}

		private void menuItemWebSched_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.WebSched);
			FormESS.ShowDialog();
		}

		private void menuItemListenerService_Click(object sender,EventArgs e) {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.ListenerService);
			FormESS.ShowDialog();
		}

		#endregion

		#region Help

		//Help
		private void menuItemRemote_Click(object sender,System.EventArgs e) {
			try {
				Process.Start("http://www.opendental.com/contact.html");
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Could not find")+" http://www.opendental.com/contact.html" + "\r\n"
					+Lan.g(this,"Please set up a default web browser."));
			}
			/*
			if(!MsgBox.Show(this,true,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?"))
			{
				return;
			}
			try{
				Process.Start("remoteclient.exe");//Network streaming remote client or any other similar client
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}*/
		}

		private void menuItemHelpWindows_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("Help.chm");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpContents_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("http://www.opendental.com/manual/toc.html");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpIndex_Click(object sender, System.EventArgs e) {
			try{
				Process.Start(@"http://www.opendental.com/manual/alphabetical.html");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemRemoteSupport_Click(object sender,EventArgs e) {
			//Check the installation directory for the GoToAssist corporate exe.
			string fileGTA=CodeBase.ODFileUtils.CombinePaths(Application.StartupPath,"GoToAssist_Corporate_Customer_ver11_3.exe");
			try {
				if(!File.Exists(fileGTA)) {
					throw new ApplicationException();//No message because a different message shows below.
				}
				//GTA exe is available, so load it up
				Process.Start(fileGTA);
			}
			catch {
				MsgBox.Show(this,"Could not find file.  Please use Online Support instead.");
			}
		}

		private void menuItemRequestFeatures_Click(object sender,EventArgs e) {
			FormFeatureRequest FormF=new FormFeatureRequest();
			FormF.Show();
		}

		private void menuItemUpdate_Click(object sender, System.EventArgs e) {
			//If A to Z folders are disabled, this menu option is unavailable, since
			//updates are handled more automatically.
			FormUpdate FormU = new FormUpdate();
			FormU.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Update Version");
		}

		#endregion

		#endregion

		//private void OnPatientCardInserted(object sender, PatientCardInsertedEventArgs e) {
		//  if (InvokeRequired) {
		//    Invoke(new PatientCardInsertedEventHandler(OnPatientCardInserted), new object[] { sender, e });
		//    return;
		//  }
		//  if (MessageBox.Show(this, string.Format(Lan.g(this, "A card belonging to {0} has been inserted. Do you wish to search for this patient now?"), e.Patient.GetNameFL()), "Open Dental", MessageBoxButtons.YesNo) != DialogResult.Yes)
		//  {
		//    return;
		//  }
		//  using (FormPatientSelect formPS = new FormPatientSelect()) {
		//    formPS.PreselectPatient(e.Patient);
		//    if(formPS.ShowDialog() == DialogResult.OK) {
		//      // OnPatientSelected(formPS.SelectedPatNum);
		//      // ModuleSelected(formPS.SelectedPatNum);
		//    }
		//  }
		//}

		///<summary>separate thread</summary>
		//public void Listen() {
		//	IPAddress ipAddress = Dns.GetHostAddresses("localhost")[0];
		//	TcpListenerCommandLine=new TcpListener(ipAddress,2123);
		//	TcpListenerCommandLine.Start();
		//	while(true) {
		//		if(!TcpListenerCommandLine.Pending()) {
		//			//Thread.Sleep(1000);//for 1 second
		//			continue;
		//		}
		//		TcpClient TcpClientRec = TcpListenerCommandLine.AcceptTcpClient();
		//		NetworkStream ns = TcpClientRec.GetStream();
		//		XmlSerializer serializer=new XmlSerializer(typeof(string[]));
		//		string[] args=(string[])serializer.Deserialize(ns);
		//		Invoke(new ProcessCommandLineDelegate(ProcessCommandLine),new object[] { args });
		//		ns.Close();
		//		TcpClientRec.Close();
		//	}
		//}

		

		/////<summary></summary>
		//protected delegate void ProcessCommandLineDelegate(string[] args);

		///<summary></summary>
		public void ProcessCommandLine(string[] args) {
			//if(!Programs.UsingEcwTight() && args.Length==0){
			if(!Programs.UsingEcwTightOrFullMode() && args.Length==0){//May have to modify to accept from other sw.
				SetModuleSelected();
				return;
			}
			/*string descript="";
			for(int i=0;i<args.Length;i++) {
				if(i>0) {
					descript+="\r\n";
				}
				descript+=args[i];
			}
			MessageBox.Show(descript);*/
			/*
			PatNum (the integer primary key)
			ChartNumber (alphanumeric)
			SSN (exactly nine digits. If required, we can gracefully handle dashes, but that is not yet implemented)
			UserName
			Password*/
			int patNum=0;
			string chartNumber="";
			string ssn="";
			string userName="";
			string passHash="";
			string aptNum="";
			string ecwConfigPath="";
			int userId=0;
			string jSessionId = "";
			string jSessionIdSSO = "";
			string lbSessionId="";
			Dictionary<string,int> dictModules=new Dictionary<string,int>();
			dictModules.Add("appt",0);
			dictModules.Add("family",1);
			dictModules.Add("account",2);
			dictModules.Add("txplan",3);
			dictModules.Add("treatplan",3);
			dictModules.Add("chart",4);
			dictModules.Add("images",5);
			dictModules.Add("manage",6);
			int startingModuleIdx=-1;
			for(int i=0;i<args.Length;i++) {
				if(args[i].StartsWith("PatNum=") && args[i].Length>7) {
					string patNumStr=args[i].Substring(7).Trim('"');
					try {
						patNum=Convert.ToInt32(patNumStr);
					}
					catch { }
				}
				if(args[i].StartsWith("ChartNumber=") && args[i].Length>12) {
					chartNumber=args[i].Substring(12).Trim('"');
				}
				if(args[i].StartsWith("SSN=") && args[i].Length>4) {
					ssn=args[i].Substring(4).Trim('"');
				}
				if(args[i].StartsWith("UserName=") && args[i].Length>9) {
					userName=args[i].Substring(9).Trim('"');
				}
				if(args[i].StartsWith("PassHash=") && args[i].Length>9) {
					passHash=args[i].Substring(9).Trim('"');
				}
				if(args[i].StartsWith("AptNum=") && args[i].Length>7) {
					aptNum=args[i].Substring(7).Trim('"');
				}
				if(args[i].StartsWith("EcwConfigPath=") && args[i].Length>14) {
					ecwConfigPath=args[i].Substring(14).Trim('"');
				}
				if(args[i].StartsWith("UserId=") && args[i].Length>7) {
					string userIdStr=args[i].Substring(7).Trim('"');
					try {
						userId=Convert.ToInt32(userIdStr);
					}
					catch { }
				}
				if(args[i].StartsWith("JSESSIONID=") && args[i].Length > 11) {
					jSessionId=args[i].Substring(11).Trim('"');
				}
				if(args[i].StartsWith("JSESSIONIDSSO=") && args[i].Length > 14) {
					jSessionIdSSO = args[i].Substring(14).Trim('"');
				}
				if(args[i].StartsWith("LBSESSIOINID=") && args[i].Length>12) {
					lbSessionId=args[i].Substring(12).Trim('"');
				}
				if(args[i].ToLower().StartsWith("module=") && args[i].Length>7) {
					string moduleName=args[i].Substring(7).Trim('"').ToLower();
					if(dictModules.ContainsKey(moduleName)) {
						startingModuleIdx=dictModules[moduleName];
					}
				}
				if(args[i].ToLower().StartsWith("show=") && args[i].Length>5) {
					_showForm=args[i].Substring(5).Trim('"').ToLower();//Currently only looks for "Popup" and "ApptsForPatient"
				}
			}
			if(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"IsLBSessionIdExcluded")=="1" //if check box in Program Links is checked
				&& lbSessionId=="" //if lbSessionId not previously set
				&& args.Length > 0 //there is at least one argument passed in
				&& !args[args.Length-1].StartsWith("LBSESSIONID="))//if there is an argument that is the last argument that is not called "LBSESSIONID", then use that argument, including the "name=" part
			{
				//An example of this is command line includes LBSESSIONID= icookie=ECWAPP3ECFH. The space makes icookie a separate parameter. We want to set lbSessionId="icookie=ECWAPP3ECFH". 
				//We are not guaranteed that the parameter is always going to be named icookie, in fact it will be different on each load balancer depending on the setup of the LB.  
				//Therefore, we cannot look for parameter name, but Aislinn from eCW guaranteed that it would be the last parameter every time during our (Cameron and Aislinn's) conversation on 3/5/2014.
				//jsalmon - This is very much a hack but the customer is very large and needs this change ASAP.  Nathan has suggested that we create a ticket with eCW to complain about this and make them fix it.
				lbSessionId=args[args.Length-1].Trim('"');
			}
			//eCW bridge values-------------------------------------------------------------
			Bridges.ECW.AptNum=PIn.Long(aptNum);
			Bridges.ECW.EcwConfigPath=ecwConfigPath;
			Bridges.ECW.UserId=userId;
			Bridges.ECW.JSessionId=jSessionId;
			Bridges.ECW.JSessionIdSSO=jSessionIdSSO;
			Bridges.ECW.LBSessionId=lbSessionId;
			//Username and password-----------------------------------------------------
			//users are allowed to use ecw tight integration without command line.  They can manually launch Open Dental.
			//if((Programs.UsingEcwTight() && Security.CurUser==null)//We always want to trigger login window for eCW tight, even if no username was passed in.
			if((Programs.UsingEcwTightOrFullMode() && Security.CurUser==null)//We always want to trigger login window for eCW tight, even if no username was passed in.
				|| (userName!=""//if a username was passed in, but not in tight eCW mode
				&& (Security.CurUser==null || Security.CurUser.UserName != userName))//and it's different from the current user
			) {
				//The purpose of this loop is to use the username and password that were passed in to determine which user to log in
				//log out------------------------------------
				LastModule=myOutlookBar.SelectedIndex;
				myOutlookBar.SelectedIndex=-1;
				myOutlookBar.Invalidate();
				UnselectActive();
				allNeutral();
				Userod user=Userods.GetUserByName(userName,true);
				if(user==null) {
					//if(Programs.UsingEcwTight() && userName!="") {
					if(Programs.UsingEcwTightOrFullMode() && userName!="") {
						user=new Userod();
						user.UserName=userName;
						user.UserGroupNum=PIn.Long(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"DefaultUserGroup"));
						if(passHash=="") {
							user.Password="";
						}
						else {
							user.Password=passHash;
						}
						Userods.Insert(user);//This can fail if duplicate username because of capitalization differences.
						DataValid.SetInvalid(InvalidType.Security);
					}
					else {//not using eCW in tight integration mode
						//So present logon screen
						FormLogOn_=new FormLogOn();
						FormLogOn_.ShowDialog(this);
						if(FormLogOn_.DialogResult==DialogResult.Cancel) {
							Application.Exit();
							return;
						}
						user=Security.CurUser.Copy();
					}
				}
				//Can't use Userods.CheckPassword, because we only have the hashed password.
				//if(passHash!=user.Password || !Programs.UsingEcwTight())//password not accepted or not using eCW
				if(passHash!=user.Password || !Programs.UsingEcwTightOrFullMode())//password not accepted or not using eCW
				{
					//So present logon screen
					FormLogOn_=new FormLogOn();
					FormLogOn_.ShowDialog(this);
					if(FormLogOn_.DialogResult==DialogResult.Cancel) {
						Application.Exit();
						return;
					}
				}
				else {//password accepted and using eCW tight.
					//this part usually happens in the logon window
					Security.CurUser = user.Copy();
					//let's skip tasks for now
					//if(PrefC.GetBool(PrefName.TasksCheckOnStartup")){
					//	int taskcount=Tasks.UserTasksCount(Security.CurUser.UserNum);
					//	if(taskcount>0){
					//		MessageBox.Show(Lan.g(this,"There are ")+taskcount+Lan.g(this," unfinished tasks on your tasklists."));
					//	}
					//}
				}
				myOutlookBar.SelectedIndex=Security.GetModule(LastModule);
				myOutlookBar.Invalidate();
				SetModuleSelected();
				Patient pat=Patients.GetPat(CurPatNum);//pat could be null
				Text=PatientL.GetMainTitle(pat,ClinicNum);//handles pat==null by not displaying pat name in title bar
				if(userControlTasks1.Visible) {
					userControlTasks1.InitializeOnStartup();
				}
				if(myOutlookBar.SelectedIndex==-1) {
					MsgBox.Show(this,"You do not have permission to use any modules.");
				}
			}
			if(startingModuleIdx!=-1 && startingModuleIdx==Security.GetModule(startingModuleIdx)) {
				UnselectActive();
				allNeutral();//Sets all controls to false.  Needed to set the new module as selected.
				myOutlookBar.SelectedIndex=startingModuleIdx;
				myOutlookBar.Invalidate();
			}
			SetModuleSelected();
			//patient id----------------------------------------------------------------
			if(patNum!=0) {
				Patient pat=Patients.GetPat(patNum);
				if(pat==null) {
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=patNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else if(chartNumber!="") {
				Patient pat=Patients.GetPatByChartNumber(chartNumber);
				if(pat==null) {
					//todo: decide action
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else if(ssn!="") {
				Patient pat=Patients.GetPatBySSN(ssn);
				if(pat==null) {
					//todo: decide action
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else{
					FillPatientButton(null);
			}
		}

		#region HQ only metrics
				
		///<summary>HQ only. ODThread wraps exception handling for us. Nothing to do here so we will just swallow any exceptions.</summary>
		private void OnThreadHqMetricsException(Exception e) {			
		}

		///<summary>HQ only. Runs every 1.6 seconds. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetrics(ODThread odThread) {
			ProcessHqMetricsPhones();
			ProcessHqMetricsEServices();
		}

		///<summary>HQ only. Called from ProcessHqMetrics(). Deals with HQ phone panel. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetricsPhones() {
#if DEBUG
			new DataConnection().SetDbT("localhost","customers","root","","","",DatabaseType.MySql,true);
#else
			new DataConnection().SetDbT("server","customers","root","","","",DatabaseType.MySql,true);
#endif
			if( //Fill the triage labels at the fastest interval if the HQ map is open. This is only typically for the project PC in the HQ call center.
				(formMapHQ!=null && !formMapHQ.IsDisposed) || 
				//For everyone else, Only fill triage labels at given interval. Too taxing on the server to perform every 1.6 seconds.
				DateTime.Now.Subtract(_hqTriageMetricsLastRefreshed).TotalSeconds>PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)) {
				DataTable phoneMetrics=Phones.GetTriageMetrics();
				this.Invoke(new FillTriageLabelsResultsArgs(OnFillTriageLabelsResults),phoneMetrics);
				//Reset the interval timer.
				_hqTriageMetricsLastRefreshed=DateTime.Now;
			}
			List<Phone> phoneList=Phones.GetPhoneList();
			List<PhoneEmpDefault> listPED=PhoneEmpDefaults.Refresh();
			//HQ Only - 'Office Down' TaskListNum = 2576.
			List<Task> listOfficesDown=Tasks.RefreshChildren(2576,false,DateTime.MinValue,Security.CurUser.UserNum,0);
			string ipaddressStr="";
			IPHostEntry iphostentry=Dns.GetHostEntry(Environment.MachineName);
			foreach(IPAddress ipaddress in iphostentry.AddressList) {
				if(ipaddress.ToString().Contains("10.10.2")) {
					ipaddressStr=ipaddress.ToString();
					break;
				}
			}
			//Get the extension linked to this machine or ip. Set in FormPhoneEmpDefaultEdit.
			int extension=PhoneEmpDefaults.GetPhoneExtension(ipaddressStr,Environment.MachineName,listPED);
			//Now get the Phone object for this extension. Phone table matches PhoneEmpDefault table more or less 1:1. 
			//Phone fields represent current state of the PhoneEmpDefault table and will be modified by the phone tracking server anytime a phone state changes for a given extension 
			//(EG... incoming call, outgoing call, hangup, etc).
			Phone phone=Phones.GetPhoneForExtension(phoneList,extension);
			bool isTriageOperator=PhoneEmpDefaults.IsTriageOperatorForExtension(extension,listPED);
			//send the results back to the UI layer for action.
			if(!this.IsDisposed) {
				Invoke(new ProcessHqMetricsResultsArgs(OnProcessHqMetricsResults),new object[] { listPED,phoneList,listOfficesDown,phone,isTriageOperator });
			}
		}

		///<summary>HQ only. Called from ProcessHqMetrics(). Deals with HQ EServices. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetricsEServices() {
			if(DateTime.Now.Subtract(_hqEServiceMetricsLastRefreshed).TotalSeconds<10) {
				return;
			}
			if(formMapHQ==null || formMapHQ.IsDisposed) { //Only run if the HQ map is open.
				return;
			}
			_hqEServiceMetricsLastRefreshed=DateTime.Now;
#if DEBUG
			new DataConnection().SetDbT("localhost","serviceshq","root","","","",DatabaseType.MySql,true);
#else
			new DataConnection().SetDbT("server184","serviceshq","root","","","",DatabaseType.MySql,true);			
#endif
			//Get important metrics from serviceshq db.
			EServiceMetrics metricsToday=EServiceMetrics.GetEServiceMetricsFromSignalHQ();
			formMapHQ.Invoke(new MethodInvoker(delegate { formMapHQ.SetEServiceMetrics(metricsToday); }));
		}

		/// <summary>HQ Only. OnProcessHqMetricsResults must be invoked from a worker thread. These are the arguments necessary.</summary>
		protected delegate void ProcessHqMetricsResultsArgs(List<PhoneEmpDefault> phoneEmpDefaultList,List<Phone> phoneList,List<Task> listOfficesDown,Phone phone,bool isTriageOperator);

		///<summary>HQ Only. Digest results of ProcessHqMetrics and update form controls accordingly.
		///phoneList is the list of all phone rows just pulled from the database.  phone is the one that we should display here, and it can be null.</summary>
		public void OnProcessHqMetricsResults(List<PhoneEmpDefault> phoneEmpDefaultList,List<Phone> phoneList,List<Task> listOfficesDown,Phone phone,bool isTriageOperator) {
			try {
				//Send the phoneList to the 2 places where it's needed.
				//1) Send to the small display in the main OD form (quick-glance).
				phoneSmall.SetPhoneList(phoneEmpDefaultList,phoneList);
				if(formPhoneTiles!=null && !formPhoneTiles.IsDisposed) { //2) Send to the big phones panel if it is open.
					formPhoneTiles.SetPhoneList(phoneEmpDefaultList,phoneList);
				}
				if(formMapHQ!=null && !formMapHQ.IsDisposed) { //3) Send to the map hq if it is open.
					formMapHQ.SetPhoneList(phoneEmpDefaultList,phoneList);
					formMapHQ.SetOfficesDownList(listOfficesDown);
				}
				//Now set the small display's current phone extension info.
				if(phone==null) {
					phoneSmall.Extension=0;
				}
				else {
					phoneSmall.Extension=phone.Extension;
				}
				phoneSmall.SetPhone(phone,PhoneEmpDefaults.GetEmpDefaultFromList(phone.EmployeeNum,phoneEmpDefaultList),isTriageOperator);
			}
			catch {
				//HQ users are complaining of unhandled exception when they close OD.
				//Suspect it could be caused here if the thread tries to access a control that has been disposed.
			}
		}

		/// <summary>HQ Only. OnFillTriageLabelsResults must be invoked from a worker thread. These are the arguments necessary.</summary>
		private delegate void FillTriageLabelsResultsArgs(DataTable phoneMetrics);

		/// <summary>HQ Only. Digest results of Phones.GetTriageMetrics() in ProcessHqMetrics(). Fills the triage labels and update form controls accordingly.</summary>
		private void OnFillTriageLabelsResults(DataTable phoneMetrics) {
			int countBlueTasks=PIn.Int(phoneMetrics.Rows[0]["CountBlueTasks"].ToString());
			int countWhiteTasks=PIn.Int(phoneMetrics.Rows[0]["CountWhiteTasks"].ToString());
			int countRedTasks=PIn.Int(phoneMetrics.Rows[0]["CountRedTasks"].ToString());
			DateTime timeOfOldestBlueTaskNote=PIn.Date(phoneMetrics.Rows[0]["TimeOfOldestBlueTaskNote"].ToString());
			DateTime timeOfOldestRedTaskNote=PIn.Date(phoneMetrics.Rows[0]["TimeOfOldestRedTaskNote"].ToString());
			TimeSpan triageBehind=new TimeSpan(0);
			if(timeOfOldestBlueTaskNote.Year>1880 && timeOfOldestRedTaskNote.Year>1880) {
				if(timeOfOldestBlueTaskNote<timeOfOldestRedTaskNote) {
					triageBehind=DateTime.Now-timeOfOldestBlueTaskNote;
				}
				else {//triageBehind based off of older RedTask
					triageBehind=DateTime.Now-timeOfOldestRedTaskNote;
				}
			}
			else if(timeOfOldestBlueTaskNote.Year>1880) {
				triageBehind=DateTime.Now-timeOfOldestBlueTaskNote;
			}
			else if(timeOfOldestRedTaskNote.Year>1880) {
				triageBehind=DateTime.Now-timeOfOldestRedTaskNote;
			}
			string countStr="0";
			if(countBlueTasks>0 || countRedTasks>0) {//Triage show red so users notice more.
				countStr=(countBlueTasks+countRedTasks).ToString();
				labelTriage.ForeColor=Color.Firebrick;
			}
			else {
				if(countWhiteTasks>0) {
					countStr="("+countWhiteTasks.ToString()+")";
				}
				labelTriage.ForeColor=Color.Black;
			}
			labelTriage.Text="T:"+countStr;
			labelWaitTime.Text=((int)triageBehind.TotalMinutes).ToString()+"m";
			if(formMapHQ!=null && !formMapHQ.IsDisposed) {
				formMapHQ.SetTriageNormal(countWhiteTasks,countBlueTasks,triageBehind,countRedTasks);
				TimeSpan urgentTriageBehind=new TimeSpan(0);
				if(timeOfOldestRedTaskNote.Year>1880) {
					urgentTriageBehind=DateTime.Now-timeOfOldestRedTaskNote;
				}
				formMapHQ.SetTriageUrgent(countRedTasks,urgentTriageBehind);
			}
		}
		
		#endregion

		private void TryNonPatientPopup() {
			Patient patCur=Patients.GetPat(CurPatNum);
			if(patCur!=null && _previousPatNum!=patCur.PatNum) {
				_datePopupDelay=DateTime.Now;
				_previousPatNum=patCur.PatNum;
			}
			if(PrefC.GetBool(PrefName.ChartNonPatientWarn) 
						&& patCur!=null 
						&& patCur.PatStatus.ToString()=="NonPatient"
						&& _datePopupDelay<=DateTime.Now) {
				MsgBox.Show(this,"A patient with the status NonPatient is currently selected.");
				_datePopupDelay=DateTime.Now.AddMinutes(5);
			}
		}

		
		/// <summary>This is set to 30 seconds</summary>
		private void timerWebHostSynch_Tick(object sender,EventArgs e) {
			if(_isStartingUp) {//If the program is starting up it may be updating and we do not want to synch yet.
				return;
			}
			try {
				string interval=PrefC.GetStringSilent(PrefName.MobileSyncIntervalMinutes);
				if(interval=="" || interval=="0") {//not a paid customer or chooses not to synch
					return;
				}
				if(System.Environment.MachineName.ToUpper()!=PrefC.GetStringSilent(PrefName.MobileSyncWorkstationName).ToUpper()) {
					//Since GetStringSilent returns "" before OD is connected to db, this gracefully loops out
					return;
				}
				if(PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate).Year<1880) {
					//full synch never run
					return;
				}
				FormEServicesSetup.SynchFromMain(false);
			}
			catch { }//If MySQL service has been lost will not automatically UE
		}

		private void timerReplicationMonitor_Tick(object sender,EventArgs e) {
			//this timer doesn't get turned on until after user successfully logs in.
			if(ReplicationServers.Listt.Count==0) {//Listt will be automatically refreshed if null.
				return;//user must not be using any replication
			}
			bool isSlaveMonitor=false;
			for(int i=0;i<ReplicationServers.Listt.Count;i++) {
				if(ReplicationServers.Listt[i].SlaveMonitor.ToString()!=Dns.GetHostName()) {
					isSlaveMonitor=true;
				}
			}
			if(!isSlaveMonitor) {
				return;
			}
			DataTable table=ReplicationServers.GetSlaveStatus();
			if(table.Rows.Count==0) {
				return;
			}
			if(table.Rows[0]["Replicate_Do_Db"].ToString().ToLower()!=DataConnection.GetDatabaseName().ToLower()) {//if the database we're connected to is not even involved in replication
				return;
			}
			string status=table.Rows[0]["Slave_SQL_Running"].ToString();
			if(status=="Yes") {
				_isReplicationSlaveStopped=false;
				return;
			}
			if(table.Rows[0]["Last_Errno"].ToString()=="0" && table.Rows[0]["Last_Error"].ToString()=="") {
				//The slave SQL is not running, but there was not an error.
				//This happens when the slave is manually stopped with an SQL statement, but stopping the slave does not hurt anything, so we should not prevent the user from using OD.
				if(!_isReplicationSlaveStopped) {
					_isReplicationSlaveStopped=true;
					MessageBox.Show(Lan.g(this,"Warning: Replication data receive is off at server ")+ReplicationServers.GetForLocalComputer().Descript+".\r\n"
						+Lan.g(this,"The server will not receive updates until the slave is started again.")+"\r\n"
						+Lan.g(this,"Contact your IT admin to run the SQL command SLAVE START.")
						);
				}
				return;
			}
			//Shut down all copies of OD and set ReplicationFailureAtServer_id to this server_id
			//No workstations will be able to connect to this single server while this flag is set.
			Prefs.UpdateLong(PrefName.ReplicationFailureAtServer_id,ReplicationServers.Server_id);
			//shut down all workstations on all servers
			FormOpenDental.signalLastRefreshed=MiscData.GetNowDateTime().AddSeconds(5);
			Signalod sig=new Signalod();
			sig.ITypes=((int)InvalidType.ShutDownNow).ToString();
			sig.SigType=SignalType.Invalid;
			Signalods.Insert(sig);
			Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
			timerReplicationMonitor.Enabled=false;
			MsgBox.Show(this,"This database is temporarily unavailable.  Please connect instead to your alternate database at the other location.");
			Application.Exit();
		}

		#region Logoff
		///<summary>This is set to 15 seconds.  This interval must be longer than the interval of the timer in FormLogoffWarning (10s), or it will go into a loop.</summary>
		private void timerLogoff_Tick(object sender,EventArgs e) {
			if(PrefC.GetInt(PrefName.SecurityLogOffAfterMinutes)==0) {
				return;
			}
			for(int f=Application.OpenForms.Count-1;f>=0;f--) {//This checks if any forms are open that make us not want to automatically log off. Currently only FormTerminal is checked for.
				Form openForm=Application.OpenForms[f];
				if(openForm.Name=="FormTerminal") {
					return;
				}
				//If anything is in progress we should halt the autologoff. After the window finishes, this will get hit after a maximum of 15 seconds and perform the auto-logoff.
				if(openForm.Name=="FormProgress") {
					return;
				}
			}
			//Warning.  When debugging this, the ActiveForm will be impossible to determine by setting breakpoints.
			//string activeFormText=Form.ActiveForm.Text;
			//If a breakpoint is set below here, ActiveForm will erroneously show as null.
			if(Form.ActiveForm==null) {//some other program has focus
				FormRecentlyOpenForLogoff=null;
				//Do not alter IsFormLogOnLastActive because it could still be active in background.
			}
			else if(Form.ActiveForm==this) {//main form active
				FormRecentlyOpenForLogoff=null;
				//User must have logged back in so IsFormLogOnLastActive should be false.
				IsFormLogOnLastActive=false;
			}
			else {//Some Open Dental dialog is active.
				if(Form.ActiveForm==FormRecentlyOpenForLogoff) {
					//The same form is active as last time, so don't add events again.
					//The active form will now be constantly resetting the dateTimeLastActivity.
				}
				else {//this is the first time this form has been encountered, so attach events and don't do anything else
					AttachMouseMoveToForm(Form.ActiveForm);
					FormRecentlyOpenForLogoff=Form.ActiveForm;
					dateTimeLastActivity=DateTime.Now;
					//Flag FormLogOn as the active form so that OD doesn't continue trying to log the user off when using the web service.
					if(Form.ActiveForm.GetType()==typeof(FormLogOn)) {
						IsFormLogOnLastActive=true;
					}
					else {
						IsFormLogOnLastActive=false;
					}
					return;
				}
			}
			DateTime dtDeadline=dateTimeLastActivity+TimeSpan.FromMinutes((double)PrefC.GetInt(PrefName.SecurityLogOffAfterMinutes));
			//Debug.WriteLine("Now:"+DateTime.Now.ToLongTimeString()+", Deadline:"+dtDeadline.ToLongTimeString());
			if(DateTime.Now<dtDeadline) {
				return;
			}
			if(Security.CurUser==null) {//nobody logged on
				return;
			}
			//The above check works unless using web service.  With web service, CurUser is not set to null when FormLogOn is shown.
			if(IsFormLogOnLastActive) {//Don't try to log off a user that is already logged off.
				return;
			}
			FormLogoffWarning formW=new FormLogoffWarning();
			formW.ShowDialog();
			//User could be working outside of OD and the Log On window will never become "active" so we set it here for a fail safe.
			IsFormLogOnLastActive=true;
			if(formW.DialogResult!=DialogResult.OK) {
				dateTimeLastActivity=DateTime.Now;
				return;//user hit cancel, so don't log off
			}
			try {
				LogOffNow(true);
			}
			catch { }
		}

		private void FormOpenDental_MouseMove(object sender,MouseEventArgs e) {
			//This has a few extra lines because my mouse during testing was firing a MouseMove every second,
			//even if no movement.  So this tests to see if there was actually movement.
			if(locationMouseForLogoff==e.Location) {
				//mouse hasn't moved
				return;
			}
			locationMouseForLogoff=e.Location;
			dateTimeLastActivity=DateTime.Now;
		}

		///<summary>Closes all open forms except FormOpenDental.  Set isForceClose to true if you want to ignore all potential closing events.  E.g. FormWikiEdit will ask users on closing if they are sure they want to discard unsaved work.  Returns false if there is an open form that requests attention, thus needs to stop the closing of the forms.</summary>
		private bool CloseOpenForms(bool isForceClose) {
			for(int f=Application.OpenForms.Count-1;f>=0;f--) {//Loop backwards so we don't get an array out of bounds error
				if(Application.OpenForms[f]==this) {// main form
					continue;
				}
				//If force closing, we HAVE to forcefully close everything related to Open Dental, regardless of plugins.  Otherwise, give plugins a chance to stop the log off event.
				if(!isForceClose) {
					//This hook was moved into this method so that the form closing loop could be shared.
					//It is correctly named and was not updated to say "FormOpenDental.CloseOpenForms" on purpose for backwards compatibility.
					if(Plugins.HookMethod(this,"FormOpenDental.LogOffNow_loopingforms",Application.OpenForms[f])) {
						continue;//if some criteria are met in the hook, don't close a certain form
					}
				}
				Form openForm=Application.OpenForms[f];//Copy so we have a reference to it after we close it.
				openForm.Hide();
				//Currently there is no way to tell if LogOffNow got called from a user initiating the log off, or if this is the auto log off feature.
				//Therefore, when the auto log off feature is enabled, we will forcefully close all forms because some forms might have pop ups within FormClosing prevent the form from closing.
				//That introduces the possibility of sensitive information staying visible in the background while the program waits for user input.
				if(isForceClose) {
					openForm.Dispose();//Strictly disposing of a form will not perform the closing events.
				}
				else {
					//Gracefully close each window.  If a window requesting attention causes the form to stay open.  Stop the log off event because the user chose to.
					openForm.Close();//Attempt to close the form
					if(openForm.IsDisposed==false) {//If the form was not closed.  
						//E.g. The wiki edit window will ask users if they want to lose their work or continue working.  This will get hit if they chose to continue working.
						openForm.Show();//Show that form again
						return false;//This form needs to stay open and stop all other forms from being closed.
					}
				}
			}
			return true;//All open forms have been closed at this point.
		}

		private void LogOffNow() {
			bool isForceClose=PrefC.GetLong(PrefName.SecurityLogOffAfterMinutes)>0;
			LogOffNow(isForceClose);
		}

		private void LogOffNow(bool isForced) {
			if(!CloseOpenForms(isForced)) {
				return;//A form is still open.  Do not continue to log the user off.
			}
			LastModule=myOutlookBar.SelectedIndex;
			myOutlookBar.SelectedIndex=-1;
			myOutlookBar.Invalidate();
			UnselectActive();
			allNeutral();
			if(userControlTasks1.Visible) {
				userControlTasks1.ClearLogOff();
			}
			if(isForced) {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has auto logged off.");
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged off.");
			}
			Userod oldUser=Security.CurUser;
			Security.CurUser=null;
			ClinicNum=0;
			Clinics.ClinicNum=ClinicNum;
			Text=PatientL.GetMainTitle(null,ClinicNum);
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Security.CurUser=oldUser;//so that the queries in FormLogOn() will work for the web service, since the web service requires a valid user to run queries.
			}
			StopEServiceMonitoring();
			FormLogOn_=new FormLogOn();
			FormLogOn_.ShowDialog(this);
			if(FormLogOn_.DialogResult==DialogResult.Cancel) {
				Application.Exit();
				return;
			}
			//If a different user logs on and they have clinics enabled, then clear the patient drop down history
			//since the current user may not have permission to access patients from the same clinic(s) as the old user
			if(oldUser.UserNum!=Security.CurUser.UserNum && !PrefC.GetBool(PrefName.EasyNoClinics)) {
				CurPatNum=0;
				PatientL.RemoveAllFromMenu(menuPatient);
			}
			myOutlookBar.SelectedIndex=Security.GetModule(LastModule);
			myOutlookBar.Invalidate();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(Security.CurUser.ClinicIsRestricted) {
					ClinicNum=Security.CurUser.ClinicNum;
				}
				else {
					ClinicNum=ComputerPrefs.LocalComputer.ClinicNum;
				}
				Clinics.ClinicNum=ClinicNum;
				RefreshMenuClinics();
			}
			SetModuleSelected();
			Patient pat=Patients.GetPat(CurPatNum);//pat could be null
			Text=PatientL.GetMainTitle(pat,ClinicNum);//handles pat==null by not displaying pat name in title bar
			FillPatientButton(pat);
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			StartEServiceMonitoring();
			//User logged back in so log on form is no longer the active window.
			IsFormLogOnLastActive=false;
			dateTimeLastActivity=DateTime.Now;
			if(myOutlookBar.SelectedIndex==-1) {
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
		}

		//The purpose of the 11 methods below is to have every child control and child form report MouseMove events to this parent form.
		//It must be dynamic and recursive.
		protected override void OnControlAdded(ControlEventArgs e) {
			AttachMouseMoveToControl(e.Control);
			base.OnControlAdded(e);
		}

		protected override void OnControlRemoved(ControlEventArgs e) {
			DetachMouseMoveFromControl(e.Control);
			base.OnControlRemoved(e);
		}

		private void AttachMouseMoveToForm(Form form) {
			form.MouseMove += new MouseEventHandler(ChildForm_MouseMove);
			AttachMouseMoveToChildren(form);
		}

		private void AttachMouseMoveToControl(Control control) {
			control.MouseMove += new MouseEventHandler(Child_MouseMove);
			control.ControlAdded += new ControlEventHandler(Child_ControlAdded);
			control.ControlRemoved += new ControlEventHandler(Child_ControlRemoved);
			AttachMouseMoveToChildren(control);
		}

		private void AttachMouseMoveToChildren(Control parent) {
			foreach(Control child in parent.Controls) {
				AttachMouseMoveToControl(child);
			}
		}

		private void DetachMouseMoveFromControl(Control control) {
			DetachMouseMoveFromChildren(control);
			control.MouseMove -= new MouseEventHandler(Child_MouseMove);
			control.ControlAdded -= new ControlEventHandler(Child_ControlAdded);
			control.ControlRemoved -= new ControlEventHandler(Child_ControlRemoved);
		}

		private void DetachMouseMoveFromChildren(Control parent) {
			foreach(Control child in parent.Controls) {
				DetachMouseMoveFromControl(child);
			}
		}

		private void Child_ControlAdded(object sender,ControlEventArgs e) {
			AttachMouseMoveToControl(e.Control);
		}

		private void Child_ControlRemoved(object sender,ControlEventArgs e) {
			DetachMouseMoveFromControl(e.Control);
		}

		private void Child_MouseMove(object sender,MouseEventArgs e) {
			Point loc=e.Location;
			Control child=(Control)sender;
			//Debug.WriteLine("Child_MouseMove:"+DateTime.Now.ToLongTimeString()+", sender:"+child.Name);
			do {
				loc.Offset(child.Left,child.Top);
				child = child.Parent;
			}
			while(child.Parent != null);//When it hits a form, either this one or any other active form.
			MouseEventArgs newArgs = new MouseEventArgs(e.Button,e.Clicks,loc.X,loc.Y,e.Delta);
			OnMouseMove(newArgs);
		}

		private void ChildForm_MouseMove(object sender,MouseEventArgs e) {
			Point loc=e.Location;
			Form childForm=(Form)sender;
			loc.Offset(childForm.Left,childForm.Top);
			MouseEventArgs newArgs = new MouseEventArgs(e.Button,e.Clicks,loc.X,loc.Y,e.Delta);
			OnMouseMove(newArgs);
		}
		#endregion Logoff

		private void SystemEvents_SessionSwitch(object sender,SessionSwitchEventArgs e) {
			if(e.Reason!=SessionSwitchReason.SessionLock) {
				return;
			}
			if(!PrefC.GetBool(PrefName.SecurityLogOffWithWindows)) {
				return;
			}
			if(Security.CurUser==null) {//not sure if this is a good test.
				return;
			}
			LogOffNow(true);
		}

		private void FormOpenDental_FormClosing(object sender,FormClosingEventArgs e) {
			//ExitCode will only be set if trying to silently update.  
			//If we start using ExitCode for anything other than silently updating, this can be moved towards the bottom of this closing.
			//If moved to the bottom, all of the clean up code that this closing event does needs to be considered in regards to updating silently from a CEMT computer.
			if(ExitCode!=0) {
				Environment.Exit(ExitCode);
			}
			try {
				Programs.ScrubExportedPatientData();//Required for EHR module d.7.
			}
			catch {
				//Can happen if cancel is clicked in Choose Database window.
			}
			try {
				Computers.ClearHeartBeat(Environment.MachineName);
			}
			catch { }
			FormUAppoint.AbortThread();
			//ICat.AbortThread
			////earlier, this wasn't working.  But I haven't tested it since moving it from Closing to FormClosing.
			//if(ThreadCommandLine!=null) {
			//	ThreadCommandLine.Abort();
			//}
			if(ThreadVM!=null) {
				ThreadVM.Abort();
				ThreadVM.Join();
				ThreadVM=null;
			}			
			if(_isEmailThreadRunning) {
				_isEmailThreadRunning=false;
				_emailSleep.Set();//If the thread is just sitting around waiting for the next email check interval, then this will cause the thread to exit immediately.
				//We used to show a message on exit because our email inbox used to delete messages after downloading. We only copy messages now, so we can kill the thread at any time.
				//FormAlertSimple formAS=new FormAlertSimple(Lan.g(this,"Shutting down.\r\nPlease wait while email finishes downloading.\r\nMay take up to 3 minutes to complete."));
				//formAS.Show();//Non-modal, so the program will not wait for user input before closing.
				ThreadEmailInbox.Abort();
				ThreadEmailInbox.Join();//We must wait for the thread to die, so that the .exe is freed up if the user is trying to update OD.
				ThreadEmailInbox=null;
			}
			if(_isTimeSynchThreadRunning) {
				_isTimeSynchThreadRunning=false;
				_timeSynchSleep.Set();
				_threadTimeSynch.Abort();
				_threadTimeSynch.Join();
				_threadTimeSynch=null;
			}
			ODThread.QuitSyncAllOdThreads();
			if(Security.CurUser!=null) {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged off.");
			}
			//if(PrefC.GetBool(PrefName.DistributorKey)) {//for OD HQ
			//  for(int f=Application.OpenForms.Count-1;f>=0;f--) {
			//    if(Application.OpenForms[f]==this) {// main form
			//      continue;
			//    }
			//    Application.OpenForms[f].Close();
			//  }
			//}
			string tempPath=PrefL.GetTempFolderPath();
			string[] arrayFileNames=Directory.GetFiles(tempPath,"*.*",SearchOption.AllDirectories);//All files in the current directory plus all files in all subdirectories.
			for(int i=0;i<arrayFileNames.Length;i++) {
				try {
					//All files related to updates need to stay.  They do not contain PHI information and will not harm anything if left around.
					if(arrayFileNames[i].Contains("UpdateFileCopier.exe")) {
						continue;//Skip any files related to updates.
					}
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if(arrayFileNames[i].Contains("UpdateFiles")) {
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if(arrayFileNames[i].Contains("updatefilecopier")) {
						continue;//Skip any files related to updates.
					}
					File.Delete(arrayFileNames[i]);
				}
				catch {
					//Do nothing because the file could have been in use or there were not sufficient permissions.
					//This file will most likely get deleted next time a temp file is created.
				}
			}
			List <string> listDirectories=new List<string>(Directory.GetDirectories(tempPath,"*",SearchOption.AllDirectories));//All subdirectories.
			listDirectories.Sort();//We need to sort so that we know for certain which directories are parent directories of other directories.
			for(int i=listDirectories.Count-1;i>=0;i--) {//Easier than recursion.  Since the list is ordered ascending, then going backwards means we delete subdirectories before their parent directories.
				try {
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if(listDirectories[i].Contains("UpdateFiles")) {
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if(listDirectories[i].Contains("updatefilecopier")) {
						continue;//Skip any files related to updates.
					}
					Directory.Delete(listDirectories[i]);
				}
				catch {
					//Do nothing because the folder could have been in use or there were not sufficient permissions.
					//This folder will most likely get deleted next time Open Dental closes.
				}
			}
		}

		private void FormOpenDental_FormClosed(object sender,FormClosedEventArgs e) {
			//Cleanup all resources related to the program which have their Dispose methods properly defined.
			//This helps ensure that the chart module and its tooth chart wrapper are properly disposed of in particular.
			//This step is necessary so that graphics memory does not fill up.
			Dispose();
			//"=====================================================
			//https://msdn.microsoft.com/en-us/library/system.environment.exit%28v=vs.110%29.aspx
			//Environment.Exit Method:
			//Terminates this process and gives the underlying operating system the specified exit code.
			//For the exitCode parameter, use a non-zero number to indicate an error. In your application, you can define your own error codes in an
			//enumeration, and return the appropriate error code based on the scenario. For example, return a value of 1 to indicate that the required file
			//is not present and a value of 2 to indicate that the file is in the wrong format. For a list of exit codes used by the Windows operating
			//system, see System Error Codes in the Windows documentation.
			//Calling the Exit method differs from using your programming language's return statement in the following ways:
			//*Exit always terminates an application. Using the return statement may terminate an application only if it is used in the application entry
			//	point, such as in the Main method.
			//*Exit terminates an application immediately, even if other threads are running. If the return statement is called in the application entry
			//	point, it causes an application to terminate only after all foreground threads have terminated.
			//*Exit requires the caller to have permission to call unmanaged code. The return statement does not.
			//*If Exit is called from a try or finally block, the code in any catch block does not execute. If the return statement is used, the code in the
			//catch block does execute.
			//====================================================="
			//Call Environment.Exit() to kill all threads which we forgot to close.  Also sends exit code 0 to the command line to indicate success.
			//If a thread needs to be gracefully quit, then it is up to the designing engineer to Join() to that thread before we get to this point.
			//We considered trying to get a list of active threads and logging debug information for those threads, but there is no way
			//to get the list of managed threads from the system.  It is our responsibility to keep track of our own managed threads.  There is a way
			//to get the list of unmanaged system threads for our application using Process.GetCurrentProcess().Threads, but that does not help us enough.
			//See http://stackoverflow.com/questions/466799/how-can-i-enumerate-all-managed-threads-in-c.  To keep track of a managed thread, use ODThread.
			//Environment.Exit requires permission for unmanaged code, which we have explicitly specified in the solution already.
			Environment.Exit(0);//Guaranteed to kill any threads which are still running.
		}


	


		

		

		


	



		


		

		

		

		

		

	

	

		

		

		


















	}

	public class PopupEvent:IComparable{
		public long PopupNum;
		///<summary>Disable this popup until this time.</summary>
		public DateTime DisableUntil;

		public int CompareTo(object obj) {
			PopupEvent pop=(PopupEvent)obj;
			return DisableUntil.CompareTo(pop.DisableUntil);
		}

		public override string ToString() {
			return PopupNum.ToString()+", "+DisableUntil.ToString();
		}
	}
}
