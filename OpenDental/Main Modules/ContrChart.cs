/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using SHDocVw;
using SparksToothChart;
#if EHRTEST
using EHR;
#endif

namespace OpenDental {
	///<summary></summary>
	public class ContrChart : System.Windows.Forms.UserControl	{
		private OpenDental.UI.Button butAddProc;
		private OpenDental.UI.Button butM;
		private OpenDental.UI.Button butOI;
		private OpenDental.UI.Button butD;
		private OpenDental.UI.Button butBF;
		private OpenDental.UI.Button butL;
		private OpenDental.UI.Button butV;
		private System.Windows.Forms.TextBox textSurf;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioEntryTP;
		private System.Windows.Forms.RadioButton radioEntryEO;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.RadioButton radioEntryEC;
		private ProcStat newStatus;
		private OpenDental.UI.Button button1;
		private System.Windows.Forms.RadioButton radioEntryC;
		//private bool dataValid=false;
		private System.Windows.Forms.ListBox listDx;
		private int[] hiLightedRows=new int[1];
		//private ContrApptSingle ApptPlanned;
		private System.Windows.Forms.CheckBox checkDone;
		private System.Windows.Forms.RadioButton radioEntryR;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox checkNotes;
		private System.Windows.Forms.CheckBox checkShowR;
		private OpenDental.UI.Button butShowNone;
		private OpenDental.UI.Button butShowAll;
		private System.Windows.Forms.CheckBox checkShowE;
		private System.Windows.Forms.CheckBox checkShowC;
		private System.Windows.Forms.CheckBox checkShowTP;
		private System.Windows.Forms.CheckBox checkRx;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.TextBox textProcCode;
		private System.Windows.Forms.Label label14;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butNew;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.Label labelDx;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.ContextMenu menuProgRight;
		private System.Windows.Forms.MenuItem menuItemPrintProg;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabControl tabControlImages;
		private System.Windows.Forms.Panel panelImages;
		///<summary>public for plugins</summary>
		public bool TreatmentNoteChanged;
		///<summary>Keeps track of which tab is selected. It's the index of the selected tab.</summary>
		private int selectedImageTab=0;
		private bool MouseIsDownOnImageSplitter;
		private int ImageSplitterOriginalY;
		private int OriginalImageMousePos;
		private System.Windows.Forms.ImageList imageListThumbnails;
		private System.Windows.Forms.ListView listViewImages;
		///<summary>The indices of the image categories which are visible in Chart.</summary>
		private ArrayList visImageCats;
		///<summary>The indices within Documents.List[i] of docs which are visible in Chart.</summary>
		private ArrayList visImages;
		///<summary>Full path to the patient folder, including \ on the end</summary>
		private string patFolder;
		private OpenDental.ODtextBox textTreatmentNotes;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkToday;
		private FormImageViewer formImageViewer;
		private Family FamCur;
		private Patient PatCur;
		private List <InsPlan> PlanList;
		private List<InsSub> SubList;
		///<summary></summary>
		[Category("Data"),Description("Occurs when user changes current patient, usually by clicking on the Select Patient button.")]
		public event PatientSelectedEventHandler PatientSelected=null;
		///<summary>For one patient. Allows highlighting rows.</summary>
		private Appointment[] ApptList;
		private System.Drawing.Printing.PrintDocument pd2;
		private int pagesPrinted;
		private System.Windows.Forms.CheckBox checkShowTeeth;//used in printing progress notes
		private bool headingPrinted;
		private int headingPrintH;
		private Document[] DocumentList;
		private List <PatPlan> PatPlanList;
		private MenuItem menuItemSetComplete;
		private MenuItem menuItemEditSelected;
		private MenuItem menuItemGroupSelected;
		private OpenDental.UI.Button butPin;
		private ListBox listButtonCats;
		private ListView listViewButtons;
		private List <Benefit> BenefitList;
		private ImageList imageListProcButtons;
		private ColumnHeader columnHeader1;
		private TabControl tabProc;
		private TabPage tabEnterTx;
		private TabPage tabMissing;
		private ProcButton[] ProcButtonList;
		private TabPage tabPrimary;
		private TabPage tabMovements;
		private GroupBox groupBox1;
		private OpenDental.UI.Button butUnhide;
		private Label label5;
		private ListBox listHidden;
		private OpenDental.UI.Button butEdentulous;
		private Label label7;
		private OpenDental.UI.Button butNotMissing;
		private OpenDental.UI.Button butMissing;
		private OpenDental.UI.Button butHidden;
		private GroupBox groupBox3;
		private Label label8;
		private GroupBox groupBox4;
		private ValidDouble textTipB;
		private Label label11;
		private ValidDouble textTipM;
		private Label label12;
		private ValidDouble textRotate;
		private Label label15;
		private ValidDouble textShiftB;
		private Label label10;
		private ValidDouble textShiftO;
		private Label label9;
		private ValidDouble textShiftM;
		private OpenDental.UI.Button butRotatePlus;
		private OpenDental.UI.Button butMixed;
		private OpenDental.UI.Button butAllPrimary;
		private OpenDental.UI.Button butAllPerm;
		private GroupBox groupBox5;
		private OpenDental.UI.Button butPerm;
		private OpenDental.UI.Button butPrimary;
		private int SelectedProcTab;
		private OpenDental.UI.Button butTipBplus;
		private OpenDental.UI.Button butTipMplus;
		private OpenDental.UI.Button butShiftBplus;
		private OpenDental.UI.Button butShiftOplus;
		private OpenDental.UI.Button butShiftMplus;
		private Label label16;
		private OpenDental.UI.Button butApplyMovements;
		private OpenDental.UI.Button butTipBminus;
		private OpenDental.UI.Button butTipMminus;
		private OpenDental.UI.Button butRotateMinus;
		private OpenDental.UI.Button butShiftBminus;
		private OpenDental.UI.Button butShiftOminus;
		private OpenDental.UI.Button butShiftMminus;
		private ODGrid gridProg;
		private ODGrid gridPtInfo;
		private CheckBox checkComm;
		private List<ToothInitial> ToothInitialList;
		private MenuItem menuItemPrintDay;
		///<summary>a list of the hidden teeth as strings. Includes "1"-"32", and "A"-"Z"</summary>
		private ArrayList HiddenTeeth;
		private CheckBox checkAudit;
		//<summary>This date will usually have minVal except while the hospital print function is running.</summary>
		//private DateTime hospitalDate;
		private PatientNote PatientNoteCur;
		private DataSet DataSetMain;
		private MenuItem menuItemLabFee;
		private MenuItem menuItemLabFeeDetach;
		private MenuItem menuItemDelete;
		private ToothChartWrapper toothChart;
		//private int lastPatNum;
		private TabPage tabPlanned;
		private TabPage tabShow;
		private GroupBox groupBox7;
		private GroupBox groupBox6;
		private CheckBox checkAppt;
		private CheckBox checkLabCase;
		private OpenDental.UI.Button butAddKey;
		//private MenuItem menuItemDeleteSelected;
		private CheckBox checkCommFamily;
		private OpenDental.UI.Button butForeignKey;
		private CheckBox checkTasks;
		private CheckBox checkEmail;
		private long PrevPtNum;
		private CheckBox checkSheets;
		private TabPage tabDraw;
		private RadioButton radioPointer;
		private RadioButton radioEraser;
		private RadioButton radioPen;
		private Panel panelDrawColor;
		private GroupBox groupBox8;
		private Panel panelTPlight;
		private Panel panelTPdark;
		private Label label18;
		private OpenDental.UI.Button butColorOther;
		private Panel panelRdark;
		private Label label21;
		private Panel panelRlight;
		private Panel panelEOdark;
		private Label label20;
		private Panel panelEOlight;
		private Panel panelECdark;
		private Label label19;
		private Panel panelEClight;
		private Panel panelCdark;
		private Label label17;
		private Panel panelClight;
		private RadioButton radioColorChanger;
		private Panel panelBlack;
		private Label label22;
		private ODGrid gridPlanned;
		private ContextMenu menuConsent;
		private RadioButton radioEntryCn;
		private CheckBox checkShowCn;
		private int Chartscrollval;
		private OpenDental.UI.Button butECWdown;
		private OpenDental.UI.Button butECWup;
		private System.Windows.Forms.WebBrowser webBrowserEcw;
		private Panel panelEcw;
		private Label labelECWerror;
		private ContextMenu menuToothChart;
		private MenuItem menuItemChartBig;
		private MenuItem menuItemChartSave;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butDown;
		private PatField[] PatFieldList;
		private OpenDental.UI.Button butChartViewDown;
		private OpenDental.UI.Button butChartViewUp;
		private OpenDental.UI.Button butChartViewAdd;
		private Label labelCustView;
		private ODGrid gridChartViews;
		private bool InitializedOnStartup;
		//<summary>Can be null if user has not set up any views.  Defaults to first in list when starting up.</summary>
		private ChartView ChartViewCurDisplay;
		private TabPage tabPatInfo;
		private ListBox listProcStatusCodes;
		private bool chartCustViewChanged;
		private ComboBox comboPrognosis;
		private Label labelPrognosis;
//		private TextBox textShowDateRange;
//		private UI.Button butShowDateRange;
		private long orionProvNum;
		private DateTime ShowDateStart;
		private UI.Button butShowDateRange;
		private TextBox textShowDateRange;
		private TabPage tabCustomer;
		private ODGrid gridCustomerViews;
		private Label labelTimes;
		private Label labelMonth1;
		private Label labelMonth2;
		private Label labelMonth3;
		private Label labelMonthAvg;
		private TextBox textMonthAvg;
		private TextBox textMonth3;
		private TextBox textMonth2;
		private TextBox textMonth1;
		private ListBox listCommonProcs;
		private Label labelCommonProc;
		private DateTime ShowDateEnd;
		private Label label2;
		private Label labelMonth0;
		private TextBox textMonth0;
		private UI.Button butPhoneNums;
		private Label label3;
		private ODButtonPanel panelQuickButtons;
		private Label label1;
		private UI.Button butClearAllMovements;
		private CheckBox checkShowCompleted;
		private UI.Button butErxAccess;
		private ContextMenu menuErx;
		private MenuItem menuItemErxRefresh;
		private CheckBox checkTreatPlans;
		private Panel panelTP;
		private ODGrid gridTreatPlans;
		private ODGrid gridTpProcs;
		private Label label4;
		private ListBox listPriorities;
		private UI.Button butNewTP;
		private bool IsDistributorKey;
		[DllImport("wininet.dll",CharSet = CharSet.Auto,SetLastError = true)]
		static extern bool InternetSetCookie(string lpszUrlName,string lbszCookieName,string lpszCookieData);
		private List<ProcButtonQuick> listProcButtonQuicks;
		///<summary>List containing only rows showing in gridPlanned, can be the same as _tablePlannedAll</summary>
		private List<DataRow> _listPlannedAppt;
		private DataTable _tablePlannedAll;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		///<summary>Used to cache the selected AptNums of the items in the main grid, to reselect them after a refresh.</summary>
		private List<long> _listSelectedAptNums=new List<long>();
		///<summary>Gets set when a patient is selected and eRx prescriptions are refreshed.
		///Cannot use PatCur or _patNumLast, because they are cleared when the module is unselected.</summary>
		private long _patNumLastErx=0;
		///<summary>List of all TPs for the current patient.  Does not include Saved status.</summary>
		private List<TreatPlan> _listTreatPlans;
		///<summary>Dictionary linking a TreatPlanNum key to the list of TpRows for that TP.</summary>
		private Dictionary<long,List<TpRow>> _dictTpNumListTpRows;
		///<summary>This is a filtered list containing only TP procedures.  It's also already sorted by priority and tooth number.</summary>
		private Procedure[] _arrayTpProcs;
		///<summary>Filtered list of procedures, used to fill tooth chart with a subset of procs for the patient.</summary>
		private List<DataRow> _procListFiltered;
		///<summary>List of all procedures (except deleted status) for the current patient.</summary>
		private List<Procedure> _listPatProcs;
		///<summary>Used for calculating insurance information. It might be able to remove this without much refactoring.</summary>
		private List<ClaimProcHist> _listClaimProcHists;
		///<summary>A subset of DataSetMain.  The procedures that need to be drawn in the graphical tooth chart.</summary>
		private List<DataRow> _procList;
		///<summary>A copy of ProcList used to revert list of DataRows back to normal ChartModule after switching to IsTpCharting view.</summary>
		private List<DataRow> _procListOrig;
	
		///<summary></summary>
		public ContrChart(){
			Logger.openlog.Log("Initializing chart module...",Logger.Severity.INFO);
			InitializeComponent();
			tabControlImages.DrawItem += new DrawItemEventHandler(OnDrawItem);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//panelQuickButtons.Enabled=false;
				butBF.Text=Lan.g(this,"B/V");//vestibular instead of facial
				butV.Text=Lan.g(this,"5");
			}
			else{
				menuItemLabFee.Visible=false;
				menuItemLabFeeDetach.Visible=false;
			}
			ODEvent.Fired+=ErxBrowserClosed;
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrChart));
			SparksToothChart.ToothChartData toothChartData1 = new SparksToothChart.ToothChartData();
			this.textSurf = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioEntryCn = new System.Windows.Forms.RadioButton();
			this.radioEntryR = new System.Windows.Forms.RadioButton();
			this.radioEntryC = new System.Windows.Forms.RadioButton();
			this.radioEntryEO = new System.Windows.Forms.RadioButton();
			this.radioEntryEC = new System.Windows.Forms.RadioButton();
			this.radioEntryTP = new System.Windows.Forms.RadioButton();
			this.listDx = new System.Windows.Forms.ListBox();
			this.labelDx = new System.Windows.Forms.Label();
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.listViewButtons = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imageListProcButtons = new System.Windows.Forms.ImageList(this.components);
			this.listButtonCats = new System.Windows.Forms.ListBox();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.checkToday = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textProcCode = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.checkAudit = new System.Windows.Forms.CheckBox();
			this.checkComm = new System.Windows.Forms.CheckBox();
			this.checkShowTeeth = new System.Windows.Forms.CheckBox();
			this.checkNotes = new System.Windows.Forms.CheckBox();
			this.checkRx = new System.Windows.Forms.CheckBox();
			this.checkShowR = new System.Windows.Forms.CheckBox();
			this.checkShowE = new System.Windows.Forms.CheckBox();
			this.checkShowC = new System.Windows.Forms.CheckBox();
			this.checkShowTP = new System.Windows.Forms.CheckBox();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuProgRight = new System.Windows.Forms.ContextMenu();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItemSetComplete = new System.Windows.Forms.MenuItem();
			this.menuItemEditSelected = new System.Windows.Forms.MenuItem();
			this.menuItemGroupSelected = new System.Windows.Forms.MenuItem();
			this.menuItemPrintProg = new System.Windows.Forms.MenuItem();
			this.menuItemPrintDay = new System.Windows.Forms.MenuItem();
			this.menuItemLabFeeDetach = new System.Windows.Forms.MenuItem();
			this.menuItemLabFee = new System.Windows.Forms.MenuItem();
			this.tabControlImages = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.panelImages = new System.Windows.Forms.Panel();
			this.listViewImages = new System.Windows.Forms.ListView();
			this.imageListThumbnails = new System.Windows.Forms.ImageList(this.components);
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.tabProc = new System.Windows.Forms.TabControl();
			this.tabEnterTx = new System.Windows.Forms.TabPage();
			this.checkTreatPlans = new System.Windows.Forms.CheckBox();
			this.panelQuickButtons = new OpenDental.UI.ODButtonPanel();
			this.comboPrognosis = new System.Windows.Forms.ComboBox();
			this.labelPrognosis = new System.Windows.Forms.Label();
			this.butD = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butBF = new OpenDental.UI.Button();
			this.butL = new OpenDental.UI.Button();
			this.butM = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butAddProc = new OpenDental.UI.Button();
			this.butV = new OpenDental.UI.Button();
			this.butOI = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tabMissing = new System.Windows.Forms.TabPage();
			this.butUnhide = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.listHidden = new System.Windows.Forms.ListBox();
			this.butEdentulous = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butNotMissing = new OpenDental.UI.Button();
			this.butMissing = new OpenDental.UI.Button();
			this.butHidden = new OpenDental.UI.Button();
			this.tabMovements = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.butClearAllMovements = new OpenDental.UI.Button();
			this.label16 = new System.Windows.Forms.Label();
			this.butApplyMovements = new OpenDental.UI.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butTipBplus = new OpenDental.UI.Button();
			this.butTipBminus = new OpenDental.UI.Button();
			this.butTipMplus = new OpenDental.UI.Button();
			this.butTipMminus = new OpenDental.UI.Button();
			this.butRotatePlus = new OpenDental.UI.Button();
			this.butRotateMinus = new OpenDental.UI.Button();
			this.textTipB = new OpenDental.ValidDouble();
			this.label11 = new System.Windows.Forms.Label();
			this.textTipM = new OpenDental.ValidDouble();
			this.label12 = new System.Windows.Forms.Label();
			this.textRotate = new OpenDental.ValidDouble();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butShiftBplus = new OpenDental.UI.Button();
			this.butShiftBminus = new OpenDental.UI.Button();
			this.butShiftOplus = new OpenDental.UI.Button();
			this.butShiftOminus = new OpenDental.UI.Button();
			this.butShiftMplus = new OpenDental.UI.Button();
			this.butShiftMminus = new OpenDental.UI.Button();
			this.textShiftB = new OpenDental.ValidDouble();
			this.label10 = new System.Windows.Forms.Label();
			this.textShiftO = new OpenDental.ValidDouble();
			this.label9 = new System.Windows.Forms.Label();
			this.textShiftM = new OpenDental.ValidDouble();
			this.label8 = new System.Windows.Forms.Label();
			this.tabPrimary = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butPerm = new OpenDental.UI.Button();
			this.butPrimary = new OpenDental.UI.Button();
			this.butMixed = new OpenDental.UI.Button();
			this.butAllPrimary = new OpenDental.UI.Button();
			this.butAllPerm = new OpenDental.UI.Button();
			this.tabPlanned = new System.Windows.Forms.TabPage();
			this.checkShowCompleted = new System.Windows.Forms.CheckBox();
			this.butPin = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butNew = new OpenDental.UI.Button();
			this.gridPlanned = new OpenDental.UI.ODGrid();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.tabShow = new System.Windows.Forms.TabPage();
			this.butShowDateRange = new OpenDental.UI.Button();
			this.textShowDateRange = new System.Windows.Forms.TextBox();
			this.listProcStatusCodes = new System.Windows.Forms.ListBox();
			this.labelCustView = new System.Windows.Forms.Label();
			this.butChartViewDown = new OpenDental.UI.Button();
			this.butChartViewUp = new OpenDental.UI.Button();
			this.butChartViewAdd = new OpenDental.UI.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkSheets = new System.Windows.Forms.CheckBox();
			this.checkTasks = new System.Windows.Forms.CheckBox();
			this.checkEmail = new System.Windows.Forms.CheckBox();
			this.checkCommFamily = new System.Windows.Forms.CheckBox();
			this.checkAppt = new System.Windows.Forms.CheckBox();
			this.checkLabCase = new System.Windows.Forms.CheckBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkShowCn = new System.Windows.Forms.CheckBox();
			this.butShowAll = new OpenDental.UI.Button();
			this.butShowNone = new OpenDental.UI.Button();
			this.gridChartViews = new OpenDental.UI.ODGrid();
			this.tabDraw = new System.Windows.Forms.TabPage();
			this.radioColorChanger = new System.Windows.Forms.RadioButton();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.panelBlack = new System.Windows.Forms.Panel();
			this.label22 = new System.Windows.Forms.Label();
			this.butColorOther = new OpenDental.UI.Button();
			this.panelRdark = new System.Windows.Forms.Panel();
			this.label21 = new System.Windows.Forms.Label();
			this.panelRlight = new System.Windows.Forms.Panel();
			this.panelEOdark = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.panelEOlight = new System.Windows.Forms.Panel();
			this.panelECdark = new System.Windows.Forms.Panel();
			this.label19 = new System.Windows.Forms.Label();
			this.panelEClight = new System.Windows.Forms.Panel();
			this.panelCdark = new System.Windows.Forms.Panel();
			this.label17 = new System.Windows.Forms.Label();
			this.panelClight = new System.Windows.Forms.Panel();
			this.panelTPdark = new System.Windows.Forms.Panel();
			this.label18 = new System.Windows.Forms.Label();
			this.panelTPlight = new System.Windows.Forms.Panel();
			this.panelDrawColor = new System.Windows.Forms.Panel();
			this.radioEraser = new System.Windows.Forms.RadioButton();
			this.radioPen = new System.Windows.Forms.RadioButton();
			this.radioPointer = new System.Windows.Forms.RadioButton();
			this.tabPatInfo = new System.Windows.Forms.TabPage();
			this.tabCustomer = new System.Windows.Forms.TabPage();
			this.labelMonth0 = new System.Windows.Forms.Label();
			this.textMonth0 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.labelCommonProc = new System.Windows.Forms.Label();
			this.labelTimes = new System.Windows.Forms.Label();
			this.labelMonth1 = new System.Windows.Forms.Label();
			this.labelMonth2 = new System.Windows.Forms.Label();
			this.labelMonth3 = new System.Windows.Forms.Label();
			this.labelMonthAvg = new System.Windows.Forms.Label();
			this.textMonthAvg = new System.Windows.Forms.TextBox();
			this.textMonth3 = new System.Windows.Forms.TextBox();
			this.textMonth2 = new System.Windows.Forms.TextBox();
			this.textMonth1 = new System.Windows.Forms.TextBox();
			this.listCommonProcs = new System.Windows.Forms.ListBox();
			this.gridCustomerViews = new OpenDental.UI.ODGrid();
			this.menuConsent = new System.Windows.Forms.ContextMenu();
			this.panelEcw = new System.Windows.Forms.Panel();
			this.labelECWerror = new System.Windows.Forms.Label();
			this.webBrowserEcw = new System.Windows.Forms.WebBrowser();
			this.butECWdown = new OpenDental.UI.Button();
			this.butECWup = new OpenDental.UI.Button();
			this.menuToothChart = new System.Windows.Forms.ContextMenu();
			this.menuItemChartBig = new System.Windows.Forms.MenuItem();
			this.menuItemChartSave = new System.Windows.Forms.MenuItem();
			this.toothChart = new SparksToothChart.ToothChartWrapper();
			this.gridProg = new OpenDental.UI.ODGrid();
			this.gridPtInfo = new OpenDental.UI.ODGrid();
			this.menuErx = new System.Windows.Forms.ContextMenu();
			this.menuItemErxRefresh = new System.Windows.Forms.MenuItem();
			this.panelTP = new System.Windows.Forms.Panel();
			this.butNewTP = new OpenDental.UI.Button();
			this.gridTreatPlans = new OpenDental.UI.ODGrid();
			this.gridTpProcs = new OpenDental.UI.ODGrid();
			this.label4 = new System.Windows.Forms.Label();
			this.listPriorities = new System.Windows.Forms.ListBox();
			this.butErxAccess = new OpenDental.UI.Button();
			this.butPhoneNums = new OpenDental.UI.Button();
			this.butForeignKey = new OpenDental.UI.Button();
			this.butAddKey = new OpenDental.UI.Button();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.button1 = new OpenDental.UI.Button();
			this.textTreatmentNotes = new OpenDental.ODtextBox();
			this.groupBox2.SuspendLayout();
			this.tabControlImages.SuspendLayout();
			this.panelImages.SuspendLayout();
			this.tabProc.SuspendLayout();
			this.tabEnterTx.SuspendLayout();
			this.tabMissing.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabMovements.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabPrimary.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabPlanned.SuspendLayout();
			this.tabShow.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabDraw.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.tabCustomer.SuspendLayout();
			this.panelEcw.SuspendLayout();
			this.panelTP.SuspendLayout();
			this.SuspendLayout();
			// 
			// textSurf
			// 
			this.textSurf.BackColor = System.Drawing.SystemColors.Window;
			this.textSurf.Location = new System.Drawing.Point(8, 2);
			this.textSurf.Name = "textSurf";
			this.textSurf.ReadOnly = true;
			this.textSurf.Size = new System.Drawing.Size(72, 20);
			this.textSurf.TabIndex = 25;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.Window;
			this.groupBox2.Controls.Add(this.radioEntryCn);
			this.groupBox2.Controls.Add(this.radioEntryR);
			this.groupBox2.Controls.Add(this.radioEntryC);
			this.groupBox2.Controls.Add(this.radioEntryEO);
			this.groupBox2.Controls.Add(this.radioEntryEC);
			this.groupBox2.Controls.Add(this.radioEntryTP);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(1, 85);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(88, 110);
			this.groupBox2.TabIndex = 35;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Entry Status";
			// 
			// radioEntryCn
			// 
			this.radioEntryCn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryCn.Location = new System.Drawing.Point(2, 91);
			this.radioEntryCn.Name = "radioEntryCn";
			this.radioEntryCn.Size = new System.Drawing.Size(75, 16);
			this.radioEntryCn.TabIndex = 5;
			this.radioEntryCn.Text = "Condition";
			this.radioEntryCn.CheckedChanged += new System.EventHandler(this.radioEntryCn_CheckedChanged);
			// 
			// radioEntryR
			// 
			this.radioEntryR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryR.Location = new System.Drawing.Point(2, 76);
			this.radioEntryR.Name = "radioEntryR";
			this.radioEntryR.Size = new System.Drawing.Size(75, 16);
			this.radioEntryR.TabIndex = 4;
			this.radioEntryR.Text = "Referred";
			this.radioEntryR.CheckedChanged += new System.EventHandler(this.radioEntryR_CheckedChanged);
			// 
			// radioEntryC
			// 
			this.radioEntryC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryC.Location = new System.Drawing.Point(2, 31);
			this.radioEntryC.Name = "radioEntryC";
			this.radioEntryC.Size = new System.Drawing.Size(74, 16);
			this.radioEntryC.TabIndex = 3;
			this.radioEntryC.Text = "Complete";
			this.radioEntryC.CheckedChanged += new System.EventHandler(this.radioEntryC_CheckedChanged);
			// 
			// radioEntryEO
			// 
			this.radioEntryEO.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryEO.Location = new System.Drawing.Point(2, 61);
			this.radioEntryEO.Name = "radioEntryEO";
			this.radioEntryEO.Size = new System.Drawing.Size(72, 16);
			this.radioEntryEO.TabIndex = 2;
			this.radioEntryEO.Text = "ExistOther";
			this.radioEntryEO.CheckedChanged += new System.EventHandler(this.radioEntryEO_CheckedChanged);
			// 
			// radioEntryEC
			// 
			this.radioEntryEC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryEC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioEntryEC.Location = new System.Drawing.Point(2, 46);
			this.radioEntryEC.Name = "radioEntryEC";
			this.radioEntryEC.Size = new System.Drawing.Size(84, 16);
			this.radioEntryEC.TabIndex = 1;
			this.radioEntryEC.Text = "ExistCurProv";
			this.radioEntryEC.CheckedChanged += new System.EventHandler(this.radioEntryEC_CheckedChanged);
			// 
			// radioEntryTP
			// 
			this.radioEntryTP.Checked = true;
			this.radioEntryTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryTP.Location = new System.Drawing.Point(2, 16);
			this.radioEntryTP.Name = "radioEntryTP";
			this.radioEntryTP.Size = new System.Drawing.Size(77, 16);
			this.radioEntryTP.TabIndex = 0;
			this.radioEntryTP.TabStop = true;
			this.radioEntryTP.Text = "TreatPlan";
			this.radioEntryTP.CheckedChanged += new System.EventHandler(this.radioEntryTP_CheckedChanged);
			// 
			// listDx
			// 
			this.listDx.Location = new System.Drawing.Point(91, 16);
			this.listDx.Name = "listDx";
			this.listDx.Size = new System.Drawing.Size(94, 134);
			this.listDx.TabIndex = 46;
			this.listDx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDx_MouseDown);
			// 
			// labelDx
			// 
			this.labelDx.Location = new System.Drawing.Point(89, -2);
			this.labelDx.Name = "labelDx";
			this.labelDx.Size = new System.Drawing.Size(100, 18);
			this.labelDx.TabIndex = 47;
			this.labelDx.Text = "Diagnosis";
			this.labelDx.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkDone
			// 
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.Location = new System.Drawing.Point(324, 5);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(67, 16);
			this.checkDone.TabIndex = 0;
			this.checkDone.Text = "Done";
			this.checkDone.Click += new System.EventHandler(this.checkDone_Click);
			// 
			// listViewButtons
			// 
			this.listViewButtons.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewButtons.AutoArrange = false;
			this.listViewButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.listViewButtons.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewButtons.Location = new System.Drawing.Point(311, 40);
			this.listViewButtons.MultiSelect = false;
			this.listViewButtons.Name = "listViewButtons";
			this.listViewButtons.Size = new System.Drawing.Size(199, 192);
			this.listViewButtons.SmallImageList = this.imageListProcButtons;
			this.listViewButtons.TabIndex = 188;
			this.listViewButtons.UseCompatibleStateImageBehavior = false;
			this.listViewButtons.View = System.Windows.Forms.View.Details;
			this.listViewButtons.Click += new System.EventHandler(this.listViewButtons_Click);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 155;
			// 
			// imageListProcButtons
			// 
			this.imageListProcButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProcButtons.ImageStream")));
			this.imageListProcButtons.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListProcButtons.Images.SetKeyName(0, "deposit.gif");
			// 
			// listButtonCats
			// 
			this.listButtonCats.IntegralHeight = false;
			this.listButtonCats.Location = new System.Drawing.Point(187, 40);
			this.listButtonCats.MultiColumn = true;
			this.listButtonCats.Name = "listButtonCats";
			this.listButtonCats.Size = new System.Drawing.Size(122, 192);
			this.listButtonCats.TabIndex = 59;
			this.listButtonCats.Click += new System.EventHandler(this.listButtonCats_Click);
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.Location = new System.Drawing.Point(91, 211);
			this.comboPriority.MaxDropDownItems = 40;
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(96, 21);
			this.comboPriority.TabIndex = 54;
			// 
			// checkToday
			// 
			this.checkToday.Checked = true;
			this.checkToday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkToday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkToday.Location = new System.Drawing.Point(1, 194);
			this.checkToday.Name = "checkToday";
			this.checkToday.Size = new System.Drawing.Size(80, 18);
			this.checkToday.TabIndex = 58;
			this.checkToday.Text = "Today";
			this.checkToday.CheckedChanged += new System.EventHandler(this.checkToday_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(89, 192);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79, 17);
			this.label6.TabIndex = 57;
			this.label6.Text = "Priority";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textProcCode
			// 
			this.textProcCode.Location = new System.Drawing.Point(330, 3);
			this.textProcCode.Name = "textProcCode";
			this.textProcCode.Size = new System.Drawing.Size(108, 20);
			this.textProcCode.TabIndex = 50;
			this.textProcCode.Text = "Type Proc Code";
			this.textProcCode.TextChanged += new System.EventHandler(this.textProcCode_TextChanged);
			this.textProcCode.Enter += new System.EventHandler(this.textProcCode_Enter);
			this.textProcCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textProcCode_KeyDown);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(282, 5);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(45, 17);
			this.label14.TabIndex = 51;
			this.label14.Text = "Or";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(308, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(128, 18);
			this.label13.TabIndex = 49;
			this.label13.Text = "Or Single Click:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkAudit
			// 
			this.checkAudit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAudit.Location = new System.Drawing.Point(154, 170);
			this.checkAudit.Name = "checkAudit";
			this.checkAudit.Size = new System.Drawing.Size(73, 13);
			this.checkAudit.TabIndex = 17;
			this.checkAudit.Text = "Audit";
			this.checkAudit.Click += new System.EventHandler(this.checkAudit_Click);
			// 
			// checkComm
			// 
			this.checkComm.Checked = true;
			this.checkComm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkComm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkComm.Location = new System.Drawing.Point(10, 33);
			this.checkComm.Name = "checkComm";
			this.checkComm.Size = new System.Drawing.Size(102, 13);
			this.checkComm.TabIndex = 16;
			this.checkComm.Text = "Comm Log";
			this.checkComm.Click += new System.EventHandler(this.checkComm_Click);
			// 
			// checkShowTeeth
			// 
			this.checkShowTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTeeth.Location = new System.Drawing.Point(154, 154);
			this.checkShowTeeth.Name = "checkShowTeeth";
			this.checkShowTeeth.Size = new System.Drawing.Size(104, 13);
			this.checkShowTeeth.TabIndex = 15;
			this.checkShowTeeth.Text = "Selected Teeth";
			this.checkShowTeeth.Click += new System.EventHandler(this.checkShowTeeth_Click);
			// 
			// checkNotes
			// 
			this.checkNotes.AllowDrop = true;
			this.checkNotes.Checked = true;
			this.checkNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotes.Location = new System.Drawing.Point(15, 105);
			this.checkNotes.Name = "checkNotes";
			this.checkNotes.Size = new System.Drawing.Size(102, 13);
			this.checkNotes.TabIndex = 11;
			this.checkNotes.Text = "Proc Notes";
			this.checkNotes.Click += new System.EventHandler(this.checkNotes_Click);
			// 
			// checkRx
			// 
			this.checkRx.Checked = true;
			this.checkRx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRx.Location = new System.Drawing.Point(10, 114);
			this.checkRx.Name = "checkRx";
			this.checkRx.Size = new System.Drawing.Size(102, 13);
			this.checkRx.TabIndex = 8;
			this.checkRx.Text = "Rx";
			this.checkRx.Click += new System.EventHandler(this.checkRx_Click);
			// 
			// checkShowR
			// 
			this.checkShowR.Checked = true;
			this.checkShowR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowR.Location = new System.Drawing.Point(9, 65);
			this.checkShowR.Name = "checkShowR";
			this.checkShowR.Size = new System.Drawing.Size(101, 13);
			this.checkShowR.TabIndex = 14;
			this.checkShowR.Text = "Referred";
			this.checkShowR.Click += new System.EventHandler(this.checkShowR_Click);
			// 
			// checkShowE
			// 
			this.checkShowE.Checked = true;
			this.checkShowE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowE.Location = new System.Drawing.Point(9, 49);
			this.checkShowE.Name = "checkShowE";
			this.checkShowE.Size = new System.Drawing.Size(101, 13);
			this.checkShowE.TabIndex = 10;
			this.checkShowE.Text = "Existing";
			this.checkShowE.Click += new System.EventHandler(this.checkShowE_Click);
			// 
			// checkShowC
			// 
			this.checkShowC.Checked = true;
			this.checkShowC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowC.Location = new System.Drawing.Point(9, 33);
			this.checkShowC.Name = "checkShowC";
			this.checkShowC.Size = new System.Drawing.Size(101, 13);
			this.checkShowC.TabIndex = 9;
			this.checkShowC.Text = "Completed";
			this.checkShowC.Click += new System.EventHandler(this.checkShowC_Click);
			// 
			// checkShowTP
			// 
			this.checkShowTP.Checked = true;
			this.checkShowTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTP.Location = new System.Drawing.Point(9, 17);
			this.checkShowTP.Name = "checkShowTP";
			this.checkShowTP.Size = new System.Drawing.Size(101, 13);
			this.checkShowTP.TabIndex = 8;
			this.checkShowTP.Text = "Treat Plan";
			this.checkShowTP.Click += new System.EventHandler(this.checkShowTP_Click);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Pat.gif");
			this.imageListMain.Images.SetKeyName(1, "Rx.gif");
			this.imageListMain.Images.SetKeyName(2, "Probe.gif");
			this.imageListMain.Images.SetKeyName(3, "Anesth.gif");
			this.imageListMain.Images.SetKeyName(4, "commlog.gif");
			// 
			// menuProgRight
			// 
			this.menuProgRight.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDelete,
            this.menuItemSetComplete,
            this.menuItemEditSelected,
            this.menuItemGroupSelected,
            this.menuItemPrintProg,
            this.menuItemPrintDay,
            this.menuItemLabFeeDetach,
            this.menuItemLabFee});
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 0;
			this.menuItemDelete.Text = "Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// menuItemSetComplete
			// 
			this.menuItemSetComplete.Index = 1;
			this.menuItemSetComplete.Text = "Set Complete";
			this.menuItemSetComplete.Click += new System.EventHandler(this.menuItemSetComplete_Click);
			// 
			// menuItemEditSelected
			// 
			this.menuItemEditSelected.Index = 2;
			this.menuItemEditSelected.Text = "Edit All";
			this.menuItemEditSelected.Click += new System.EventHandler(this.menuItemEditSelected_Click);
			// 
			// menuItemGroupSelected
			// 
			this.menuItemGroupSelected.Index = 3;
			this.menuItemGroupSelected.Text = "Group Note";
			this.menuItemGroupSelected.Click += new System.EventHandler(this.menuItemGroupSelected_Click);
			// 
			// menuItemPrintProg
			// 
			this.menuItemPrintProg.Index = 4;
			this.menuItemPrintProg.Text = "Print Progress Notes ...";
			this.menuItemPrintProg.Click += new System.EventHandler(this.menuItemPrintProg_Click);
			// 
			// menuItemPrintDay
			// 
			this.menuItemPrintDay.Index = 5;
			this.menuItemPrintDay.Text = "Print Day for Hospital";
			this.menuItemPrintDay.Click += new System.EventHandler(this.menuItemPrintDay_Click);
			// 
			// menuItemLabFeeDetach
			// 
			this.menuItemLabFeeDetach.Index = 6;
			this.menuItemLabFeeDetach.Text = "Detach Lab Fee";
			this.menuItemLabFeeDetach.Click += new System.EventHandler(this.menuItemLabFeeDetach_Click);
			// 
			// menuItemLabFee
			// 
			this.menuItemLabFee.Index = 7;
			this.menuItemLabFee.Text = "Attach Lab Fee";
			this.menuItemLabFee.Click += new System.EventHandler(this.menuItemLabFee_Click);
			// 
			// tabControlImages
			// 
			this.tabControlImages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControlImages.Controls.Add(this.tabPage1);
			this.tabControlImages.Controls.Add(this.tabPage2);
			this.tabControlImages.Controls.Add(this.tabPage4);
			this.tabControlImages.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabControlImages.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControlImages.ItemSize = new System.Drawing.Size(42, 22);
			this.tabControlImages.Location = new System.Drawing.Point(0, 681);
			this.tabControlImages.Name = "tabControlImages";
			this.tabControlImages.SelectedIndex = 0;
			this.tabControlImages.Size = new System.Drawing.Size(939, 27);
			this.tabControlImages.TabIndex = 185;
			this.tabControlImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlImages_MouseDown);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(931, 0);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "BW\'s";
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(931, 0);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Pano";
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4, 4);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(931, 0);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "tabPage4";
			// 
			// panelImages
			// 
			this.panelImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelImages.Controls.Add(this.listViewImages);
			this.panelImages.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelImages.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panelImages.Location = new System.Drawing.Point(0, 592);
			this.panelImages.Name = "panelImages";
			this.panelImages.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.panelImages.Size = new System.Drawing.Size(939, 89);
			this.panelImages.TabIndex = 186;
			this.panelImages.Visible = false;
			this.panelImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseDown);
			this.panelImages.MouseLeave += new System.EventHandler(this.panelImages_MouseLeave);
			this.panelImages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseMove);
			this.panelImages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseUp);
			// 
			// listViewImages
			// 
			this.listViewImages.Activation = System.Windows.Forms.ItemActivation.TwoClick;
			this.listViewImages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewImages.HideSelection = false;
			this.listViewImages.LabelWrap = false;
			this.listViewImages.LargeImageList = this.imageListThumbnails;
			this.listViewImages.Location = new System.Drawing.Point(0, 4);
			this.listViewImages.MultiSelect = false;
			this.listViewImages.Name = "listViewImages";
			this.listViewImages.Size = new System.Drawing.Size(937, 83);
			this.listViewImages.TabIndex = 0;
			this.listViewImages.UseCompatibleStateImageBehavior = false;
			this.listViewImages.DoubleClick += new System.EventHandler(this.listViewImages_DoubleClick);
			// 
			// imageListThumbnails
			// 
			this.imageListThumbnails.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListThumbnails.ImageSize = new System.Drawing.Size(100, 100);
			this.imageListThumbnails.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// pd2
			// 
			this.pd2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd2_PrintPage);
			// 
			// tabProc
			// 
			this.tabProc.Controls.Add(this.tabEnterTx);
			this.tabProc.Controls.Add(this.tabMissing);
			this.tabProc.Controls.Add(this.tabMovements);
			this.tabProc.Controls.Add(this.tabPrimary);
			this.tabProc.Controls.Add(this.tabPlanned);
			this.tabProc.Controls.Add(this.tabShow);
			this.tabProc.Controls.Add(this.tabDraw);
			this.tabProc.Controls.Add(this.tabPatInfo);
			this.tabProc.Controls.Add(this.tabCustomer);
			this.tabProc.Location = new System.Drawing.Point(415, 28);
			this.tabProc.Name = "tabProc";
			this.tabProc.SelectedIndex = 0;
			this.tabProc.Size = new System.Drawing.Size(524, 259);
			this.tabProc.TabIndex = 190;
			this.tabProc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabProc_MouseDown);
			// 
			// tabEnterTx
			// 
			this.tabEnterTx.Controls.Add(this.checkTreatPlans);
			this.tabEnterTx.Controls.Add(this.panelQuickButtons);
			this.tabEnterTx.Controls.Add(this.comboPrognosis);
			this.tabEnterTx.Controls.Add(this.labelPrognosis);
			this.tabEnterTx.Controls.Add(this.listDx);
			this.tabEnterTx.Controls.Add(this.listViewButtons);
			this.tabEnterTx.Controls.Add(this.groupBox2);
			this.tabEnterTx.Controls.Add(this.listButtonCats);
			this.tabEnterTx.Controls.Add(this.butD);
			this.tabEnterTx.Controls.Add(this.comboPriority);
			this.tabEnterTx.Controls.Add(this.textSurf);
			this.tabEnterTx.Controls.Add(this.textDate);
			this.tabEnterTx.Controls.Add(this.butBF);
			this.tabEnterTx.Controls.Add(this.checkToday);
			this.tabEnterTx.Controls.Add(this.butL);
			this.tabEnterTx.Controls.Add(this.label6);
			this.tabEnterTx.Controls.Add(this.butM);
			this.tabEnterTx.Controls.Add(this.butOK);
			this.tabEnterTx.Controls.Add(this.butAddProc);
			this.tabEnterTx.Controls.Add(this.butV);
			this.tabEnterTx.Controls.Add(this.textProcCode);
			this.tabEnterTx.Controls.Add(this.butOI);
			this.tabEnterTx.Controls.Add(this.label14);
			this.tabEnterTx.Controls.Add(this.labelDx);
			this.tabEnterTx.Controls.Add(this.label13);
			this.tabEnterTx.Controls.Add(this.label3);
			this.tabEnterTx.Location = new System.Drawing.Point(4, 22);
			this.tabEnterTx.Name = "tabEnterTx";
			this.tabEnterTx.Padding = new System.Windows.Forms.Padding(3);
			this.tabEnterTx.Size = new System.Drawing.Size(516, 233);
			this.tabEnterTx.TabIndex = 0;
			this.tabEnterTx.Text = "Enter Treatment";
			this.tabEnterTx.UseVisualStyleBackColor = true;
			// 
			// checkTreatPlans
			// 
			this.checkTreatPlans.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTreatPlans.Location = new System.Drawing.Point(192,213);
			this.checkTreatPlans.Name = "checkTreatPlans";
			this.checkTreatPlans.Size = new System.Drawing.Size(112,17);
			this.checkTreatPlans.TabIndex = 203;
			this.checkTreatPlans.Text = "Treatment Plans";
			this.checkTreatPlans.CheckedChanged += new System.EventHandler(this.checkTreatPlans_CheckedChanged);
			// 
			// comboPrognosis
			// 
			this.comboPrognosis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPrognosis.Location = new System.Drawing.Point(91, 174);
			this.comboPrognosis.MaxDropDownItems = 40;
			this.comboPrognosis.Name = "comboPrognosis";
			this.comboPrognosis.Size = new System.Drawing.Size(96, 21);
			this.comboPrognosis.TabIndex = 199;
			// 
			// labelPrognosis
			// 
			this.labelPrognosis.Location = new System.Drawing.Point(89, 155);
			this.labelPrognosis.Name = "labelPrognosis";
			this.labelPrognosis.Size = new System.Drawing.Size(79, 17);
			this.labelPrognosis.TabIndex = 200;
			this.labelPrognosis.Text = "Prognosis";
			this.labelPrognosis.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(184, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 18);
			this.label3.TabIndex = 201;
			this.label3.Text = "Procedure Buttons:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabMissing
			// 
			this.tabMissing.Controls.Add(this.butUnhide);
			this.tabMissing.Controls.Add(this.label5);
			this.tabMissing.Controls.Add(this.listHidden);
			this.tabMissing.Controls.Add(this.butEdentulous);
			this.tabMissing.Controls.Add(this.groupBox1);
			this.tabMissing.Location = new System.Drawing.Point(4, 22);
			this.tabMissing.Name = "tabMissing";
			this.tabMissing.Padding = new System.Windows.Forms.Padding(3);
			this.tabMissing.Size = new System.Drawing.Size(516, 233);
			this.tabMissing.TabIndex = 1;
			this.tabMissing.Text = "Missing Teeth";
			this.tabMissing.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(304, 12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(147, 17);
			this.label5.TabIndex = 19;
			this.label5.Text = "Hidden Teeth";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listHidden
			// 
			this.listHidden.FormattingEnabled = true;
			this.listHidden.Location = new System.Drawing.Point(307, 33);
			this.listHidden.Name = "listHidden";
			this.listHidden.Size = new System.Drawing.Size(94, 69);
			this.listHidden.TabIndex = 18;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.butNotMissing);
			this.groupBox1.Controls.Add(this.butMissing);
			this.groupBox1.Controls.Add(this.butHidden);
			this.groupBox1.Location = new System.Drawing.Point(20, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(267, 90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Set Selected Teeth";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(115, 46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(146, 17);
			this.label7.TabIndex = 20;
			this.label7.Text = "(including numbers)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabMovements
			// 
			this.tabMovements.Controls.Add(this.label1);
			this.tabMovements.Controls.Add(this.butClearAllMovements);
			this.tabMovements.Controls.Add(this.label16);
			this.tabMovements.Controls.Add(this.butApplyMovements);
			this.tabMovements.Controls.Add(this.groupBox4);
			this.tabMovements.Controls.Add(this.groupBox3);
			this.tabMovements.Location = new System.Drawing.Point(4, 22);
			this.tabMovements.Name = "tabMovements";
			this.tabMovements.Size = new System.Drawing.Size(516, 233);
			this.tabMovements.TabIndex = 3;
			this.tabMovements.Text = "Movements";
			this.tabMovements.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(47, 198);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(180, 18);
			this.label1.TabIndex = 33;
			this.label1.Text = "Clear all tooth movements.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(233, 198);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(229, 18);
			this.label16.TabIndex = 29;
			this.label16.Text = "(if you typed in changes)";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butTipBplus);
			this.groupBox4.Controls.Add(this.butTipBminus);
			this.groupBox4.Controls.Add(this.butTipMplus);
			this.groupBox4.Controls.Add(this.butTipMminus);
			this.groupBox4.Controls.Add(this.butRotatePlus);
			this.groupBox4.Controls.Add(this.butRotateMinus);
			this.groupBox4.Controls.Add(this.textTipB);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.textTipM);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.textRotate);
			this.groupBox4.Controls.Add(this.label15);
			this.groupBox4.Location = new System.Drawing.Point(255, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(207, 109);
			this.groupBox4.TabIndex = 15;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Rotate/Tip degrees";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(3, 77);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(68, 18);
			this.label11.TabIndex = 28;
			this.label11.Text = "Labial Tip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3, 49);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(68, 18);
			this.label12.TabIndex = 24;
			this.label12.Text = "Mesial Tip";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(3, 20);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(68, 18);
			this.label15.TabIndex = 20;
			this.label15.Text = "Rotate";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.butShiftBplus);
			this.groupBox3.Controls.Add(this.butShiftBminus);
			this.groupBox3.Controls.Add(this.butShiftOplus);
			this.groupBox3.Controls.Add(this.butShiftOminus);
			this.groupBox3.Controls.Add(this.butShiftMplus);
			this.groupBox3.Controls.Add(this.butShiftMminus);
			this.groupBox3.Controls.Add(this.textShiftB);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.textShiftO);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.textShiftM);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Location = new System.Drawing.Point(20, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(207, 109);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Shift millimeters";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(3, 77);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(68, 18);
			this.label10.TabIndex = 28;
			this.label10.Text = "Labial";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(3, 49);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(68, 18);
			this.label9.TabIndex = 24;
			this.label9.Text = "Occlusal";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(3, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(68, 18);
			this.label8.TabIndex = 20;
			this.label8.Text = "Mesial";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPrimary
			// 
			this.tabPrimary.Controls.Add(this.groupBox5);
			this.tabPrimary.Controls.Add(this.butMixed);
			this.tabPrimary.Controls.Add(this.butAllPrimary);
			this.tabPrimary.Controls.Add(this.butAllPerm);
			this.tabPrimary.Location = new System.Drawing.Point(4, 22);
			this.tabPrimary.Name = "tabPrimary";
			this.tabPrimary.Size = new System.Drawing.Size(516, 233);
			this.tabPrimary.TabIndex = 2;
			this.tabPrimary.Text = "Primary";
			this.tabPrimary.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.butPerm);
			this.groupBox5.Controls.Add(this.butPrimary);
			this.groupBox5.Location = new System.Drawing.Point(20, 12);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(153, 90);
			this.groupBox5.TabIndex = 21;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Set Selected Teeth";
			// 
			// tabPlanned
			// 
			this.tabPlanned.BackColor = System.Drawing.Color.White;
			this.tabPlanned.Controls.Add(this.checkShowCompleted);
			this.tabPlanned.Controls.Add(this.butPin);
			this.tabPlanned.Controls.Add(this.butClear);
			this.tabPlanned.Controls.Add(this.butNew);
			this.tabPlanned.Controls.Add(this.checkDone);
			this.tabPlanned.Controls.Add(this.gridPlanned);
			this.tabPlanned.Controls.Add(this.butDown);
			this.tabPlanned.Controls.Add(this.butUp);
			this.tabPlanned.Location = new System.Drawing.Point(4, 22);
			this.tabPlanned.Name = "tabPlanned";
			this.tabPlanned.Size = new System.Drawing.Size(516, 233);
			this.tabPlanned.TabIndex = 4;
			this.tabPlanned.Text = "Planned Appts";
			this.tabPlanned.UseVisualStyleBackColor = true;
			// 
			// checkShowCompleted
			// 
			this.checkShowCompleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCompleted.Location = new System.Drawing.Point(381, 5);
			this.checkShowCompleted.Name = "checkShowCompleted";
			this.checkShowCompleted.Size = new System.Drawing.Size(132, 16);
			this.checkShowCompleted.TabIndex = 196;
			this.checkShowCompleted.Text = "Show Completed";
			this.checkShowCompleted.CheckedChanged += new System.EventHandler(this.checkShowCompleted_CheckedChanged);
			// 
			// gridPlanned
			// 
			this.gridPlanned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPlanned.HasMultilineHeaders = false;
			this.gridPlanned.HScrollVisible = false;
			this.gridPlanned.Location = new System.Drawing.Point(0, 25);
			this.gridPlanned.Name = "gridPlanned";
			this.gridPlanned.ScrollValue = 0;
			this.gridPlanned.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridPlanned.Size = new System.Drawing.Size(516, 208);
			this.gridPlanned.TabIndex = 193;
			this.gridPlanned.Title = "Planned Appointments";
			this.gridPlanned.TranslationName = "TablePlannedAppts";
			this.gridPlanned.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPlanned_CellDoubleClick);
			// 
			// tabShow
			// 
			this.tabShow.BackColor = System.Drawing.Color.White;
			this.tabShow.Controls.Add(this.butShowDateRange);
			this.tabShow.Controls.Add(this.textShowDateRange);
			this.tabShow.Controls.Add(this.listProcStatusCodes);
			this.tabShow.Controls.Add(this.labelCustView);
			this.tabShow.Controls.Add(this.butChartViewDown);
			this.tabShow.Controls.Add(this.butChartViewUp);
			this.tabShow.Controls.Add(this.butChartViewAdd);
			this.tabShow.Controls.Add(this.groupBox7);
			this.tabShow.Controls.Add(this.groupBox6);
			this.tabShow.Controls.Add(this.checkShowTeeth);
			this.tabShow.Controls.Add(this.checkNotes);
			this.tabShow.Controls.Add(this.checkAudit);
			this.tabShow.Controls.Add(this.butShowAll);
			this.tabShow.Controls.Add(this.butShowNone);
			this.tabShow.Controls.Add(this.gridChartViews);
			this.tabShow.Location = new System.Drawing.Point(4, 22);
			this.tabShow.Name = "tabShow";
			this.tabShow.Size = new System.Drawing.Size(516, 233);
			this.tabShow.TabIndex = 5;
			this.tabShow.Text = "Show";
			this.tabShow.UseVisualStyleBackColor = true;
			// 
			// textShowDateRange
			// 
			this.textShowDateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textShowDateRange.Location = new System.Drawing.Point(144, 188);
			this.textShowDateRange.Name = "textShowDateRange";
			this.textShowDateRange.ReadOnly = true;
			this.textShowDateRange.Size = new System.Drawing.Size(125, 19);
			this.textShowDateRange.TabIndex = 46;
			// 
			// listProcStatusCodes
			// 
			this.listProcStatusCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listProcStatusCodes.ColumnWidth = 60;
			this.listProcStatusCodes.FormattingEnabled = true;
			this.listProcStatusCodes.IntegralHeight = false;
			this.listProcStatusCodes.Location = new System.Drawing.Point(6, 156);
			this.listProcStatusCodes.MultiColumn = true;
			this.listProcStatusCodes.Name = "listProcStatusCodes";
			this.listProcStatusCodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProcStatusCodes.Size = new System.Drawing.Size(134, 74);
			this.listProcStatusCodes.TabIndex = 45;
			this.listProcStatusCodes.Visible = false;
			this.listProcStatusCodes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listProcStatusCodes_MouseUp);
			// 
			// labelCustView
			// 
			this.labelCustView.AutoSize = true;
			this.labelCustView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCustView.ForeColor = System.Drawing.Color.Red;
			this.labelCustView.Location = new System.Drawing.Point(160, 211);
			this.labelCustView.Name = "labelCustView";
			this.labelCustView.Size = new System.Drawing.Size(96, 16);
			this.labelCustView.TabIndex = 43;
			this.labelCustView.Text = "Custom View";
			this.labelCustView.Visible = false;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.checkSheets);
			this.groupBox7.Controls.Add(this.checkTasks);
			this.groupBox7.Controls.Add(this.checkEmail);
			this.groupBox7.Controls.Add(this.checkCommFamily);
			this.groupBox7.Controls.Add(this.checkAppt);
			this.groupBox7.Controls.Add(this.checkLabCase);
			this.groupBox7.Controls.Add(this.checkRx);
			this.groupBox7.Controls.Add(this.checkComm);
			this.groupBox7.Location = new System.Drawing.Point(144, 4);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(125, 148);
			this.groupBox7.TabIndex = 19;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Object Types";
			// 
			// checkSheets
			// 
			this.checkSheets.Checked = true;
			this.checkSheets.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSheets.Location = new System.Drawing.Point(10, 130);
			this.checkSheets.Name = "checkSheets";
			this.checkSheets.Size = new System.Drawing.Size(102, 13);
			this.checkSheets.TabIndex = 219;
			this.checkSheets.Text = "Sheets";
			this.checkSheets.Click += new System.EventHandler(this.checkSheets_Click);
			// 
			// checkTasks
			// 
			this.checkTasks.Checked = true;
			this.checkTasks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkTasks.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTasks.Location = new System.Drawing.Point(10, 66);
			this.checkTasks.Name = "checkTasks";
			this.checkTasks.Size = new System.Drawing.Size(102, 13);
			this.checkTasks.TabIndex = 218;
			this.checkTasks.Text = "Tasks";
			this.checkTasks.Click += new System.EventHandler(this.checkTasks_Click);
			// 
			// checkEmail
			// 
			this.checkEmail.Checked = true;
			this.checkEmail.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEmail.Location = new System.Drawing.Point(10, 82);
			this.checkEmail.Name = "checkEmail";
			this.checkEmail.Size = new System.Drawing.Size(102, 13);
			this.checkEmail.TabIndex = 217;
			this.checkEmail.Text = "Email";
			this.checkEmail.Click += new System.EventHandler(this.checkEmail_Click);
			// 
			// checkCommFamily
			// 
			this.checkCommFamily.Checked = true;
			this.checkCommFamily.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkCommFamily.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCommFamily.Location = new System.Drawing.Point(26, 49);
			this.checkCommFamily.Name = "checkCommFamily";
			this.checkCommFamily.Size = new System.Drawing.Size(88, 13);
			this.checkCommFamily.TabIndex = 20;
			this.checkCommFamily.Text = "Family";
			this.checkCommFamily.Click += new System.EventHandler(this.checkCommFamily_Click);
			// 
			// checkAppt
			// 
			this.checkAppt.Checked = true;
			this.checkAppt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAppt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppt.Location = new System.Drawing.Point(10, 17);
			this.checkAppt.Name = "checkAppt";
			this.checkAppt.Size = new System.Drawing.Size(102, 13);
			this.checkAppt.TabIndex = 20;
			this.checkAppt.Text = "Appointments";
			this.checkAppt.Click += new System.EventHandler(this.checkAppt_Click);
			// 
			// checkLabCase
			// 
			this.checkLabCase.Checked = true;
			this.checkLabCase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkLabCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkLabCase.Location = new System.Drawing.Point(10, 98);
			this.checkLabCase.Name = "checkLabCase";
			this.checkLabCase.Size = new System.Drawing.Size(102, 13);
			this.checkLabCase.TabIndex = 17;
			this.checkLabCase.Text = "Lab Cases";
			this.checkLabCase.Click += new System.EventHandler(this.checkLabCase_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkShowCn);
			this.groupBox6.Controls.Add(this.checkShowE);
			this.groupBox6.Controls.Add(this.checkShowR);
			this.groupBox6.Controls.Add(this.checkShowC);
			this.groupBox6.Controls.Add(this.checkShowTP);
			this.groupBox6.Location = new System.Drawing.Point(6, 4);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(121, 99);
			this.groupBox6.TabIndex = 18;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Procedures";
			// 
			// checkShowCn
			// 
			this.checkShowCn.Checked = true;
			this.checkShowCn.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowCn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCn.Location = new System.Drawing.Point(9, 81);
			this.checkShowCn.Name = "checkShowCn";
			this.checkShowCn.Size = new System.Drawing.Size(101, 13);
			this.checkShowCn.TabIndex = 15;
			this.checkShowCn.Text = "Conditions";
			this.checkShowCn.Click += new System.EventHandler(this.checkShowCn_Click);
			// 
			// gridChartViews
			// 
			this.gridChartViews.HasMultilineHeaders = false;
			this.gridChartViews.HScrollVisible = false;
			this.gridChartViews.Location = new System.Drawing.Point(303, 8);
			this.gridChartViews.Name = "gridChartViews";
			this.gridChartViews.ScrollValue = 0;
			this.gridChartViews.Size = new System.Drawing.Size(191, 173);
			this.gridChartViews.TabIndex = 44;
			this.gridChartViews.Title = "Chart Views";
			this.gridChartViews.TranslationName = "GridChartViews";
			this.gridChartViews.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridChartViews_DoubleClick);
			this.gridChartViews.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridChartViews_CellClick);
			// 
			// tabDraw
			// 
			this.tabDraw.Controls.Add(this.radioColorChanger);
			this.tabDraw.Controls.Add(this.groupBox8);
			this.tabDraw.Controls.Add(this.panelDrawColor);
			this.tabDraw.Controls.Add(this.radioEraser);
			this.tabDraw.Controls.Add(this.radioPen);
			this.tabDraw.Controls.Add(this.radioPointer);
			this.tabDraw.Location = new System.Drawing.Point(4, 22);
			this.tabDraw.Name = "tabDraw";
			this.tabDraw.Size = new System.Drawing.Size(516, 233);
			this.tabDraw.TabIndex = 6;
			this.tabDraw.Text = "Draw";
			this.tabDraw.UseVisualStyleBackColor = true;
			// 
			// radioColorChanger
			// 
			this.radioColorChanger.Location = new System.Drawing.Point(14, 70);
			this.radioColorChanger.Name = "radioColorChanger";
			this.radioColorChanger.Size = new System.Drawing.Size(122, 17);
			this.radioColorChanger.TabIndex = 5;
			this.radioColorChanger.TabStop = true;
			this.radioColorChanger.Text = "Color Changer";
			this.radioColorChanger.UseVisualStyleBackColor = true;
			this.radioColorChanger.Click += new System.EventHandler(this.radioColorChanger_Click);
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.panelBlack);
			this.groupBox8.Controls.Add(this.label22);
			this.groupBox8.Controls.Add(this.butColorOther);
			this.groupBox8.Controls.Add(this.panelRdark);
			this.groupBox8.Controls.Add(this.label21);
			this.groupBox8.Controls.Add(this.panelRlight);
			this.groupBox8.Controls.Add(this.panelEOdark);
			this.groupBox8.Controls.Add(this.label20);
			this.groupBox8.Controls.Add(this.panelEOlight);
			this.groupBox8.Controls.Add(this.panelECdark);
			this.groupBox8.Controls.Add(this.label19);
			this.groupBox8.Controls.Add(this.panelEClight);
			this.groupBox8.Controls.Add(this.panelCdark);
			this.groupBox8.Controls.Add(this.label17);
			this.groupBox8.Controls.Add(this.panelClight);
			this.groupBox8.Controls.Add(this.panelTPdark);
			this.groupBox8.Controls.Add(this.label18);
			this.groupBox8.Controls.Add(this.panelTPlight);
			this.groupBox8.Location = new System.Drawing.Point(160, 11);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(157, 214);
			this.groupBox8.TabIndex = 4;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Set Color";
			// 
			// panelBlack
			// 
			this.panelBlack.BackColor = System.Drawing.Color.Black;
			this.panelBlack.Location = new System.Drawing.Point(95, 147);
			this.panelBlack.Name = "panelBlack";
			this.panelBlack.Size = new System.Drawing.Size(22, 22);
			this.panelBlack.TabIndex = 194;
			this.panelBlack.Click += new System.EventHandler(this.panelBlack_Click);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(11, 150);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(82, 17);
			this.label22.TabIndex = 193;
			this.label22.Text = "Black";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelRdark
			// 
			this.panelRdark.BackColor = System.Drawing.Color.Black;
			this.panelRdark.Location = new System.Drawing.Point(95, 121);
			this.panelRdark.Name = "panelRdark";
			this.panelRdark.Size = new System.Drawing.Size(22, 22);
			this.panelRdark.TabIndex = 18;
			this.panelRdark.Click += new System.EventHandler(this.panelRdark_Click);
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(11, 124);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(82, 17);
			this.label21.TabIndex = 17;
			this.label21.Text = "Referred";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelRlight
			// 
			this.panelRlight.BackColor = System.Drawing.Color.Black;
			this.panelRlight.Location = new System.Drawing.Point(123, 121);
			this.panelRlight.Name = "panelRlight";
			this.panelRlight.Size = new System.Drawing.Size(22, 22);
			this.panelRlight.TabIndex = 16;
			this.panelRlight.Click += new System.EventHandler(this.panelRlight_Click);
			// 
			// panelEOdark
			// 
			this.panelEOdark.BackColor = System.Drawing.Color.Black;
			this.panelEOdark.Location = new System.Drawing.Point(95, 95);
			this.panelEOdark.Name = "panelEOdark";
			this.panelEOdark.Size = new System.Drawing.Size(22, 22);
			this.panelEOdark.TabIndex = 15;
			this.panelEOdark.Click += new System.EventHandler(this.panelEOdark_Click);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(11, 98);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(82, 17);
			this.label20.TabIndex = 14;
			this.label20.Text = "ExistOther";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelEOlight
			// 
			this.panelEOlight.BackColor = System.Drawing.Color.Black;
			this.panelEOlight.Location = new System.Drawing.Point(123, 95);
			this.panelEOlight.Name = "panelEOlight";
			this.panelEOlight.Size = new System.Drawing.Size(22, 22);
			this.panelEOlight.TabIndex = 13;
			this.panelEOlight.Click += new System.EventHandler(this.panelEOlight_Click);
			// 
			// panelECdark
			// 
			this.panelECdark.BackColor = System.Drawing.Color.Black;
			this.panelECdark.Location = new System.Drawing.Point(95, 69);
			this.panelECdark.Name = "panelECdark";
			this.panelECdark.Size = new System.Drawing.Size(22, 22);
			this.panelECdark.TabIndex = 12;
			this.panelECdark.Click += new System.EventHandler(this.panelECdark_Click);
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(11, 72);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(82, 17);
			this.label19.TabIndex = 11;
			this.label19.Text = "ExistCurProv";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelEClight
			// 
			this.panelEClight.BackColor = System.Drawing.Color.Black;
			this.panelEClight.Location = new System.Drawing.Point(123, 69);
			this.panelEClight.Name = "panelEClight";
			this.panelEClight.Size = new System.Drawing.Size(22, 22);
			this.panelEClight.TabIndex = 10;
			this.panelEClight.Click += new System.EventHandler(this.panelEClight_Click);
			// 
			// panelCdark
			// 
			this.panelCdark.BackColor = System.Drawing.Color.Black;
			this.panelCdark.Location = new System.Drawing.Point(95, 43);
			this.panelCdark.Name = "panelCdark";
			this.panelCdark.Size = new System.Drawing.Size(22, 22);
			this.panelCdark.TabIndex = 9;
			this.panelCdark.Click += new System.EventHandler(this.panelCdark_Click);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(11, 46);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(82, 17);
			this.label17.TabIndex = 8;
			this.label17.Text = "Complete";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelClight
			// 
			this.panelClight.BackColor = System.Drawing.Color.Black;
			this.panelClight.Location = new System.Drawing.Point(123, 43);
			this.panelClight.Name = "panelClight";
			this.panelClight.Size = new System.Drawing.Size(22, 22);
			this.panelClight.TabIndex = 7;
			this.panelClight.Click += new System.EventHandler(this.panelClight_Click);
			// 
			// panelTPdark
			// 
			this.panelTPdark.BackColor = System.Drawing.Color.Black;
			this.panelTPdark.Location = new System.Drawing.Point(95, 17);
			this.panelTPdark.Name = "panelTPdark";
			this.panelTPdark.Size = new System.Drawing.Size(22, 22);
			this.panelTPdark.TabIndex = 6;
			this.panelTPdark.Click += new System.EventHandler(this.panelTPdark_Click);
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(11, 20);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(82, 17);
			this.label18.TabIndex = 5;
			this.label18.Text = "TreatPlan";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelTPlight
			// 
			this.panelTPlight.BackColor = System.Drawing.Color.Black;
			this.panelTPlight.Location = new System.Drawing.Point(123, 17);
			this.panelTPlight.Name = "panelTPlight";
			this.panelTPlight.Size = new System.Drawing.Size(22, 22);
			this.panelTPlight.TabIndex = 4;
			this.panelTPlight.Click += new System.EventHandler(this.panelTPlight_Click);
			// 
			// panelDrawColor
			// 
			this.panelDrawColor.BackColor = System.Drawing.Color.Black;
			this.panelDrawColor.Location = new System.Drawing.Point(13, 101);
			this.panelDrawColor.Name = "panelDrawColor";
			this.panelDrawColor.Size = new System.Drawing.Size(22, 22);
			this.panelDrawColor.TabIndex = 3;
			this.panelDrawColor.DoubleClick += new System.EventHandler(this.panelDrawColor_DoubleClick);
			// 
			// radioEraser
			// 
			this.radioEraser.Location = new System.Drawing.Point(14, 51);
			this.radioEraser.Name = "radioEraser";
			this.radioEraser.Size = new System.Drawing.Size(122, 17);
			this.radioEraser.TabIndex = 2;
			this.radioEraser.TabStop = true;
			this.radioEraser.Text = "Eraser";
			this.radioEraser.UseVisualStyleBackColor = true;
			this.radioEraser.Click += new System.EventHandler(this.radioEraser_Click);
			// 
			// radioPen
			// 
			this.radioPen.Location = new System.Drawing.Point(14, 32);
			this.radioPen.Name = "radioPen";
			this.radioPen.Size = new System.Drawing.Size(122, 17);
			this.radioPen.TabIndex = 1;
			this.radioPen.TabStop = true;
			this.radioPen.Text = "Pen";
			this.radioPen.UseVisualStyleBackColor = true;
			this.radioPen.Click += new System.EventHandler(this.radioPen_Click);
			// 
			// radioPointer
			// 
			this.radioPointer.Checked = true;
			this.radioPointer.Location = new System.Drawing.Point(14, 13);
			this.radioPointer.Name = "radioPointer";
			this.radioPointer.Size = new System.Drawing.Size(122, 17);
			this.radioPointer.TabIndex = 0;
			this.radioPointer.TabStop = true;
			this.radioPointer.Text = "Pointer";
			this.radioPointer.UseVisualStyleBackColor = true;
			this.radioPointer.Click += new System.EventHandler(this.radioPointer_Click);
			// 
			// tabPatInfo
			// 
			this.tabPatInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPatInfo.Name = "tabPatInfo";
			this.tabPatInfo.Size = new System.Drawing.Size(516, 233);
			this.tabPatInfo.TabIndex = 7;
			this.tabPatInfo.Text = "Pat Info";
			this.tabPatInfo.UseVisualStyleBackColor = true;
			// 
			// tabCustomer
			// 
			this.tabCustomer.Controls.Add(this.labelMonth0);
			this.tabCustomer.Controls.Add(this.textMonth0);
			this.tabCustomer.Controls.Add(this.label2);
			this.tabCustomer.Controls.Add(this.labelCommonProc);
			this.tabCustomer.Controls.Add(this.labelTimes);
			this.tabCustomer.Controls.Add(this.labelMonth1);
			this.tabCustomer.Controls.Add(this.labelMonth2);
			this.tabCustomer.Controls.Add(this.labelMonth3);
			this.tabCustomer.Controls.Add(this.labelMonthAvg);
			this.tabCustomer.Controls.Add(this.textMonthAvg);
			this.tabCustomer.Controls.Add(this.textMonth3);
			this.tabCustomer.Controls.Add(this.textMonth2);
			this.tabCustomer.Controls.Add(this.textMonth1);
			this.tabCustomer.Controls.Add(this.listCommonProcs);
			this.tabCustomer.Controls.Add(this.gridCustomerViews);
			this.tabCustomer.Location = new System.Drawing.Point(4, 22);
			this.tabCustomer.Name = "tabCustomer";
			this.tabCustomer.Padding = new System.Windows.Forms.Padding(3);
			this.tabCustomer.Size = new System.Drawing.Size(516, 233);
			this.tabCustomer.TabIndex = 8;
			this.tabCustomer.Text = "Customer";
			this.tabCustomer.UseVisualStyleBackColor = true;
			// 
			// labelMonth0
			// 
			this.labelMonth0.Location = new System.Drawing.Point(340, 109);
			this.labelMonth0.Name = "labelMonth0";
			this.labelMonth0.Size = new System.Drawing.Size(72, 20);
			this.labelMonth0.TabIndex = 62;
			this.labelMonth0.Text = "month 0";
			this.labelMonth0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMonth0
			// 
			this.textMonth0.Location = new System.Drawing.Point(413, 110);
			this.textMonth0.Name = "textMonth0";
			this.textMonth0.ReadOnly = true;
			this.textMonth0.Size = new System.Drawing.Size(50, 20);
			this.textMonth0.TabIndex = 61;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(340, 163);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(170, 41);
			this.label2.TabIndex = 60;
			this.label2.Text = "(Avg is based on entire family call history excluding first two months)";
			// 
			// labelCommonProc
			// 
			this.labelCommonProc.Location = new System.Drawing.Point(201, 11);
			this.labelCommonProc.Name = "labelCommonProc";
			this.labelCommonProc.Size = new System.Drawing.Size(123, 16);
			this.labelCommonProc.TabIndex = 59;
			this.labelCommonProc.Text = "Quick add:";
			this.labelCommonProc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTimes
			// 
			this.labelTimes.Location = new System.Drawing.Point(347, 11);
			this.labelTimes.Name = "labelTimes";
			this.labelTimes.Size = new System.Drawing.Size(120, 16);
			this.labelTimes.TabIndex = 58;
			this.labelTimes.Text = "Total time for family:";
			this.labelTimes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMonth1
			// 
			this.labelMonth1.Location = new System.Drawing.Point(340, 83);
			this.labelMonth1.Name = "labelMonth1";
			this.labelMonth1.Size = new System.Drawing.Size(72, 20);
			this.labelMonth1.TabIndex = 57;
			this.labelMonth1.Text = "month 1";
			this.labelMonth1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMonth2
			// 
			this.labelMonth2.Location = new System.Drawing.Point(340, 57);
			this.labelMonth2.Name = "labelMonth2";
			this.labelMonth2.Size = new System.Drawing.Size(72, 20);
			this.labelMonth2.TabIndex = 56;
			this.labelMonth2.Text = "month 2";
			this.labelMonth2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMonth3
			// 
			this.labelMonth3.Location = new System.Drawing.Point(340, 31);
			this.labelMonth3.Name = "labelMonth3";
			this.labelMonth3.Size = new System.Drawing.Size(72, 20);
			this.labelMonth3.TabIndex = 55;
			this.labelMonth3.Text = "month 3";
			this.labelMonth3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMonthAvg
			// 
			this.labelMonthAvg.Location = new System.Drawing.Point(340, 135);
			this.labelMonthAvg.Name = "labelMonthAvg";
			this.labelMonthAvg.Size = new System.Drawing.Size(72, 20);
			this.labelMonthAvg.TabIndex = 54;
			this.labelMonthAvg.Text = "Avg";
			this.labelMonthAvg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMonthAvg
			// 
			this.textMonthAvg.Location = new System.Drawing.Point(413, 136);
			this.textMonthAvg.Name = "textMonthAvg";
			this.textMonthAvg.ReadOnly = true;
			this.textMonthAvg.Size = new System.Drawing.Size(50, 20);
			this.textMonthAvg.TabIndex = 50;
			// 
			// textMonth3
			// 
			this.textMonth3.Location = new System.Drawing.Point(413, 32);
			this.textMonth3.Name = "textMonth3";
			this.textMonth3.ReadOnly = true;
			this.textMonth3.Size = new System.Drawing.Size(50, 20);
			this.textMonth3.TabIndex = 49;
			// 
			// textMonth2
			// 
			this.textMonth2.Location = new System.Drawing.Point(413, 58);
			this.textMonth2.Name = "textMonth2";
			this.textMonth2.ReadOnly = true;
			this.textMonth2.Size = new System.Drawing.Size(50, 20);
			this.textMonth2.TabIndex = 48;
			// 
			// textMonth1
			// 
			this.textMonth1.Location = new System.Drawing.Point(413, 84);
			this.textMonth1.Name = "textMonth1";
			this.textMonth1.ReadOnly = true;
			this.textMonth1.Size = new System.Drawing.Size(50, 20);
			this.textMonth1.TabIndex = 47;
			// 
			// listCommonProcs
			// 
			this.listCommonProcs.FormattingEnabled = true;
			this.listCommonProcs.Items.AddRange(new object[] {
            "Monthly Maintenance",
            "Monthly Mobile",
            "Monthly E-Mail Support",
            "Monthly EHR",
            "Data Conversion",
            "Trial Conversion",
            "Demo",
            "Online Training",
            "Additional Online Training",
            "eCW Online Training",
            "eCW Installation Verify",
            "Programming",
            "Query Programming"});
			this.listCommonProcs.Location = new System.Drawing.Point(203, 30);
			this.listCommonProcs.Name = "listCommonProcs";
			this.listCommonProcs.Size = new System.Drawing.Size(131, 173);
			this.listCommonProcs.TabIndex = 46;
			this.listCommonProcs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listCommonProcs_MouseDown);
			// 
			// gridCustomerViews
			// 
			this.gridCustomerViews.HasMultilineHeaders = false;
			this.gridCustomerViews.HScrollVisible = false;
			this.gridCustomerViews.Location = new System.Drawing.Point(6, 30);
			this.gridCustomerViews.Name = "gridCustomerViews";
			this.gridCustomerViews.ScrollValue = 0;
			this.gridCustomerViews.Size = new System.Drawing.Size(191, 173);
			this.gridCustomerViews.TabIndex = 45;
			this.gridCustomerViews.Title = "Chart Views";
			this.gridCustomerViews.TranslationName = "GridChartViews";
			this.gridCustomerViews.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerViews_CellDoubleClick);
			this.gridCustomerViews.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerViews_CellClick);
			// 
			// menuConsent
			// 
			this.menuConsent.Popup += new System.EventHandler(this.menuConsent_Popup);
			// 
			// panelEcw
			// 
			this.panelEcw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelEcw.Controls.Add(this.labelECWerror);
			this.panelEcw.Controls.Add(this.webBrowserEcw);
			this.panelEcw.Controls.Add(this.butECWdown);
			this.panelEcw.Controls.Add(this.butECWup);
			this.panelEcw.Location = new System.Drawing.Point(444, 540);
			this.panelEcw.Name = "panelEcw";
			this.panelEcw.Size = new System.Drawing.Size(373, 52);
			this.panelEcw.TabIndex = 197;
			// 
			// labelECWerror
			// 
			this.labelECWerror.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelECWerror.Location = new System.Drawing.Point(25, 22);
			this.labelECWerror.Name = "labelECWerror";
			this.labelECWerror.Size = new System.Drawing.Size(314, 14);
			this.labelECWerror.TabIndex = 199;
			this.labelECWerror.Text = "Error:";
			this.labelECWerror.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// menuToothChart
			// 
			this.menuToothChart.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemChartBig,
            this.menuItemChartSave});
			this.menuToothChart.Popup += new System.EventHandler(this.menuToothChart_Popup);
			// 
			// menuItemChartBig
			// 
			this.menuItemChartBig.Index = 0;
			this.menuItemChartBig.Text = "Show Big";
			this.menuItemChartBig.Click += new System.EventHandler(this.menuItemChartBig_Click);
			// 
			// menuItemChartSave
			// 
			this.menuItemChartSave.Index = 1;
			this.menuItemChartSave.Text = "Save to Images";
			this.menuItemChartSave.Click += new System.EventHandler(this.menuItemChartSave_Click);
			// 
			// toothChart
			// 
			this.toothChart.AutoFinish = false;
			this.toothChart.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
			this.toothChart.Cursor = System.Windows.Forms.Cursors.Default;
			this.toothChart.CursorTool = SparksToothChart.CursorTool.Pointer;
			this.toothChart.DeviceFormat = null;
			this.toothChart.DrawMode = OpenDentBusiness.DrawingMode.Simple2D;
			this.toothChart.Location = new System.Drawing.Point(0, 26);
			this.toothChart.Name = "toothChart";
			this.toothChart.PerioMode = false;
			this.toothChart.PreferredPixelFormatNumber = 0;
			this.toothChart.Size = new System.Drawing.Size(410, 307);
			this.toothChart.TabIndex = 194;
			toothChartData1.SizeControl = new System.Drawing.Size(410, 307);
			this.toothChart.TcData = toothChartData1;
			this.toothChart.UseHardware = false;
			this.toothChart.SegmentDrawn += new SparksToothChart.ToothChartDrawEventHandler(this.toothChart_SegmentDrawn);
			// 
			// gridProg
			// 
			this.gridProg.AllowSortingByColumn = true;
			this.gridProg.HasMultilineHeaders = false;
			this.gridProg.HScrollVisible = true;
			this.gridProg.Location = new System.Drawing.Point(415, 291);
			this.gridProg.Name = "gridProg";
			this.gridProg.ScrollValue = 0;
			this.gridProg.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProg.Size = new System.Drawing.Size(523, 65);
			this.gridProg.TabIndex = 192;
			this.gridProg.Title = "Progress Notes";
			this.gridProg.TranslationName = "TableProg";
			this.gridProg.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProg_CellDoubleClick);
			this.gridProg.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProg_CellClick);
			this.gridProg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridProg_KeyDown);
			this.gridProg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridProg_MouseUp);
			// 
			// gridPtInfo
			// 
			this.gridPtInfo.HasMultilineHeaders = false;
			this.gridPtInfo.HScrollVisible = false;
			this.gridPtInfo.Location = new System.Drawing.Point(0, 405);
			this.gridPtInfo.Name = "gridPtInfo";
			this.gridPtInfo.ScrollValue = 0;
			this.gridPtInfo.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridPtInfo.Size = new System.Drawing.Size(411, 325);
			this.gridPtInfo.TabIndex = 193;
			this.gridPtInfo.Title = "Patient Info";
			this.gridPtInfo.TranslationName = "TableChartPtInfo";
			this.gridPtInfo.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPtInfo_CellDoubleClick);
			// 
			// menuErx
			// 
			this.menuErx.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemErxRefresh});
			// 
			// menuItemErxRefresh
			// 
			this.menuItemErxRefresh.Index = 0;
			this.menuItemErxRefresh.Text = "Refresh";
			this.menuItemErxRefresh.Click += new System.EventHandler(this.menuItemErxRefresh_Click);
			// 
			// panelTP
			// 
			this.panelTP.Controls.Add(this.butNewTP);
			this.panelTP.Controls.Add(this.gridTreatPlans);
			this.panelTP.Controls.Add(this.gridTpProcs);
			this.panelTP.Controls.Add(this.label4);
			this.panelTP.Controls.Add(this.listPriorities);
			this.panelTP.Location = new System.Drawing.Point(415, 357);
			this.panelTP.MaximumSize = new System.Drawing.Size(523, 999);
			this.panelTP.MinimumSize = new System.Drawing.Size(523, 164);
			this.panelTP.Name = "panelTP";
			this.panelTP.Size = new System.Drawing.Size(523, 179);
			this.panelTP.TabIndex = 210;
			// 
			// butNewTP
			// 
			this.butNewTP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewTP.Autosize = true;
			this.butNewTP.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNewTP.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNewTP.CornerRadius = 4F;
			this.butNewTP.Image = global::OpenDental.Properties.Resources.Add;
			this.butNewTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNewTP.Location = new System.Drawing.Point(436, 0);
			this.butNewTP.Name = "butNewTP";
			this.butNewTP.Size = new System.Drawing.Size(77, 23);
			this.butNewTP.TabIndex = 216;
			this.butNewTP.Text = "New TP";
			this.butNewTP.Click += new System.EventHandler(this.butNewTP_Click);
			// 
			// gridTreatPlans
			// 
			this.gridTreatPlans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTreatPlans.HasMultilineHeaders = false;
			this.gridTreatPlans.HScrollVisible = false;
			this.gridTreatPlans.Location = new System.Drawing.Point(0, 0);
			this.gridTreatPlans.MaximumSize = new System.Drawing.Size(430, 200);
			this.gridTreatPlans.Name = "gridTreatPlans";
			this.gridTreatPlans.ScrollValue = 0;
			this.gridTreatPlans.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridTreatPlans.Size = new System.Drawing.Size(430, 115);
			this.gridTreatPlans.TabIndex = 214;
			this.gridTreatPlans.Title = "Treatment Plans";
			this.gridTreatPlans.TranslationName = "TableTPList";
			this.gridTreatPlans.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTreatPlans_CellDoubleClick);
			this.gridTreatPlans.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTreatPlans_CellClick);
			// 
			// gridTpProcs
			// 
			this.gridTpProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTpProcs.HasMultilineHeaders = false;
			this.gridTpProcs.HScrollVisible = true;
			this.gridTpProcs.Location = new System.Drawing.Point(0, 117);
			this.gridTpProcs.Name = "gridTpProcs";
			this.gridTpProcs.ScrollValue = 0;
			this.gridTpProcs.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridTpProcs.Size = new System.Drawing.Size(446, 60);
			this.gridTpProcs.TabIndex = 213;
			this.gridTpProcs.Title = "Procedures";
			this.gridTpProcs.TranslationName = "TableTP";
			this.gridTpProcs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTpProcs_CellDoubleClick);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(451, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 15);
			this.label4.TabIndex = 211;
			this.label4.Text = "Set Priority";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listPriorities
			// 
			this.listPriorities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listPriorities.Location = new System.Drawing.Point(452, 133);
			this.listPriorities.MaximumSize = new System.Drawing.Size(70, 250);
			this.listPriorities.MinimumSize = new System.Drawing.Size(70, 17);
			this.listPriorities.Name = "listPriorities";
			this.listPriorities.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listPriorities.Size = new System.Drawing.Size(70, 43);
			this.listPriorities.TabIndex = 212;
			this.listPriorities.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPriorities_MouseDown);
			// 
			// butErxAccess
			// 
			this.butErxAccess.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butErxAccess.Autosize = true;
			this.butErxAccess.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butErxAccess.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butErxAccess.CornerRadius = 4F;
			this.butErxAccess.Enabled = false;
			this.butErxAccess.Location = new System.Drawing.Point(88, 425);
			this.butErxAccess.Name = "butErxAccess";
			this.butErxAccess.Size = new System.Drawing.Size(75, 14);
			this.butErxAccess.TabIndex = 199;
			this.butErxAccess.Text = "Erx Access";
			this.butErxAccess.UseVisualStyleBackColor = true;
			this.butErxAccess.Click += new System.EventHandler(this.butErxAccess_Click);
			// 
			// butPhoneNums
			// 
			this.butPhoneNums.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPhoneNums.Autosize = true;
			this.butPhoneNums.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPhoneNums.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPhoneNums.CornerRadius = 4F;
			this.butPhoneNums.Enabled = false;
			this.butPhoneNums.Location = new System.Drawing.Point(169, 425);
			this.butPhoneNums.Name = "butPhoneNums";
			this.butPhoneNums.Size = new System.Drawing.Size(75, 14);
			this.butPhoneNums.TabIndex = 198;
			this.butPhoneNums.Text = "Phone Nums";
			this.butPhoneNums.UseVisualStyleBackColor = true;
			this.butPhoneNums.Click += new System.EventHandler(this.butPhoneNums_Click);
			// 
			// webBrowserEcw
			// 
			this.webBrowserEcw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.webBrowserEcw.Location = new System.Drawing.Point(1, 11);
			this.webBrowserEcw.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowserEcw.Name = "webBrowserEcw";
			this.webBrowserEcw.Size = new System.Drawing.Size(370, 52);
			this.webBrowserEcw.TabIndex = 198;
			this.webBrowserEcw.Url = new System.Uri("", System.UriKind.Relative);
			// 
			// butECWdown
			// 
			this.butECWdown.AdjustImageLocation = new System.Drawing.Point(0, -1);
			this.butECWdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butECWdown.Autosize = true;
			this.butECWdown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butECWdown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butECWdown.CornerRadius = 2F;
			this.butECWdown.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butECWdown.Location = new System.Drawing.Point(321, 1);
			this.butECWdown.Name = "butECWdown";
			this.butECWdown.Size = new System.Drawing.Size(24, 9);
			this.butECWdown.TabIndex = 197;
			this.butECWdown.UseVisualStyleBackColor = true;
			this.butECWdown.Click += new System.EventHandler(this.butECWdown_Click);
			// 
			// butECWup
			// 
			this.butECWup.AdjustImageLocation = new System.Drawing.Point(0, -1);
			this.butECWup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butECWup.Autosize = true;
			this.butECWup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butECWup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butECWup.CornerRadius = 2F;
			this.butECWup.Image = global::OpenDental.Properties.Resources.arrowUpTriangle;
			this.butECWup.Location = new System.Drawing.Point(346, 1);
			this.butECWup.Name = "butECWup";
			this.butECWup.Size = new System.Drawing.Size(24, 9);
			this.butECWup.TabIndex = 196;
			this.butECWup.UseVisualStyleBackColor = true;
			this.butECWup.Click += new System.EventHandler(this.butECWup_Click);
			// 
			// butForeignKey
			// 
			this.butForeignKey.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butForeignKey.Autosize = true;
			this.butForeignKey.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butForeignKey.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butForeignKey.CornerRadius = 4F;
			this.butForeignKey.Enabled = false;
			this.butForeignKey.Location = new System.Drawing.Point(250, 425);
			this.butForeignKey.Name = "butForeignKey";
			this.butForeignKey.Size = new System.Drawing.Size(75, 14);
			this.butForeignKey.TabIndex = 196;
			this.butForeignKey.Text = "Foreign Key";
			this.butForeignKey.UseVisualStyleBackColor = true;
			this.butForeignKey.Click += new System.EventHandler(this.butForeignKey_Click);
			// 
			// butAddKey
			// 
			this.butAddKey.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddKey.Autosize = true;
			this.butAddKey.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddKey.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddKey.CornerRadius = 4F;
			this.butAddKey.Enabled = false;
			this.butAddKey.Location = new System.Drawing.Point(331, 425);
			this.butAddKey.Name = "butAddKey";
			this.butAddKey.Size = new System.Drawing.Size(78, 14);
			this.butAddKey.TabIndex = 195;
			this.butAddKey.Text = "USA Key";
			this.butAddKey.UseVisualStyleBackColor = true;
			this.butAddKey.Click += new System.EventHandler(this.butAddKey_Click);	// 
			// panelQuickButtons
			// 
			this.panelQuickButtons.Location = new System.Drawing.Point(315, 45);
			this.panelQuickButtons.Name = "panelQuickButtons";
			this.panelQuickButtons.Size = new System.Drawing.Size(195, 182);
			this.panelQuickButtons.TabIndex = 202;
			this.panelQuickButtons.UseBlueTheme = false;
			this.panelQuickButtons.ItemClickBut += new OpenDental.UI.ODButtonPanelEventHandler(this.panelQuickButtons_ItemClickBut);
			// 
			// butD
			// 
			this.butD.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butD.Autosize = true;
			this.butD.BackColor = System.Drawing.SystemColors.Control;
			this.butD.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butD.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butD.CornerRadius = 4F;
			this.butD.Location = new System.Drawing.Point(61, 43);
			this.butD.Name = "butD";
			this.butD.Size = new System.Drawing.Size(24, 20);
			this.butD.TabIndex = 20;
			this.butD.Text = "D";
			this.butD.UseVisualStyleBackColor = false;
			this.butD.Click += new System.EventHandler(this.butD_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(0, 211);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(89, 20);
			this.textDate.TabIndex = 55;
			// 
			// butBF
			// 
			this.butBF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBF.Autosize = true;
			this.butBF.BackColor = System.Drawing.SystemColors.Control;
			this.butBF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBF.CornerRadius = 4F;
			this.butBF.Location = new System.Drawing.Point(22, 23);
			this.butBF.Name = "butBF";
			this.butBF.Size = new System.Drawing.Size(28, 20);
			this.butBF.TabIndex = 21;
			this.butBF.Text = "B/F";
			this.butBF.UseVisualStyleBackColor = false;
			this.butBF.Click += new System.EventHandler(this.butBF_Click);
			// 
			// butL
			// 
			this.butL.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butL.Autosize = true;
			this.butL.BackColor = System.Drawing.SystemColors.Control;
			this.butL.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butL.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butL.CornerRadius = 4F;
			this.butL.Location = new System.Drawing.Point(32, 63);
			this.butL.Name = "butL";
			this.butL.Size = new System.Drawing.Size(24, 20);
			this.butL.TabIndex = 22;
			this.butL.Text = "L";
			this.butL.UseVisualStyleBackColor = false;
			this.butL.Click += new System.EventHandler(this.butL_Click);
			// 
			// butM
			// 
			this.butM.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butM.Autosize = true;
			this.butM.BackColor = System.Drawing.SystemColors.Control;
			this.butM.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butM.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butM.CornerRadius = 4F;
			this.butM.Location = new System.Drawing.Point(3, 43);
			this.butM.Name = "butM";
			this.butM.Size = new System.Drawing.Size(24, 20);
			this.butM.TabIndex = 18;
			this.butM.Text = "M";
			this.butM.UseVisualStyleBackColor = false;
			this.butM.Click += new System.EventHandler(this.butM_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(442, 1);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(44, 23);
			this.butOK.TabIndex = 52;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAddProc
			// 
			this.butAddProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProc.Autosize = true;
			this.butAddProc.BackColor = System.Drawing.SystemColors.Control;
			this.butAddProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddProc.CornerRadius = 4F;
			this.butAddProc.Location = new System.Drawing.Point(191, 1);
			this.butAddProc.Name = "butAddProc";
			this.butAddProc.Size = new System.Drawing.Size(89, 23);
			this.butAddProc.TabIndex = 17;
			this.butAddProc.Text = "Procedure List";
			this.butAddProc.UseVisualStyleBackColor = false;
			this.butAddProc.Click += new System.EventHandler(this.butAddProc_Click);
			// 
			// butV
			// 
			this.butV.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butV.Autosize = true;
			this.butV.BackColor = System.Drawing.SystemColors.Control;
			this.butV.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butV.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butV.CornerRadius = 4F;
			this.butV.Location = new System.Drawing.Point(50, 23);
			this.butV.Name = "butV";
			this.butV.Size = new System.Drawing.Size(17, 20);
			this.butV.TabIndex = 24;
			this.butV.Text = "V";
			this.butV.UseVisualStyleBackColor = false;
			this.butV.Click += new System.EventHandler(this.butV_Click);
			// 
			// butOI
			// 
			this.butOI.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOI.Autosize = true;
			this.butOI.BackColor = System.Drawing.SystemColors.Control;
			this.butOI.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOI.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOI.CornerRadius = 4F;
			this.butOI.Location = new System.Drawing.Point(27, 43);
			this.butOI.Name = "butOI";
			this.butOI.Size = new System.Drawing.Size(34, 20);
			this.butOI.TabIndex = 19;
			this.butOI.Text = "O/I";
			this.butOI.UseVisualStyleBackColor = false;
			this.butOI.Click += new System.EventHandler(this.butOI_Click);
			// 
			// butUnhide
			// 
			this.butUnhide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnhide.Autosize = true;
			this.butUnhide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnhide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnhide.CornerRadius = 4F;
			this.butUnhide.Location = new System.Drawing.Point(307, 113);
			this.butUnhide.Name = "butUnhide";
			this.butUnhide.Size = new System.Drawing.Size(71, 23);
			this.butUnhide.TabIndex = 20;
			this.butUnhide.Text = "Unhide";
			this.butUnhide.Click += new System.EventHandler(this.butUnhide_Click);
			// 
			// butEdentulous
			// 
			this.butEdentulous.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdentulous.Autosize = true;
			this.butEdentulous.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdentulous.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdentulous.CornerRadius = 4F;
			this.butEdentulous.Location = new System.Drawing.Point(31, 113);
			this.butEdentulous.Name = "butEdentulous";
			this.butEdentulous.Size = new System.Drawing.Size(82, 23);
			this.butEdentulous.TabIndex = 16;
			this.butEdentulous.Text = "Edentulous";
			this.butEdentulous.Click += new System.EventHandler(this.butEdentulous_Click);
			// 
			// butNotMissing
			// 
			this.butNotMissing.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNotMissing.Autosize = true;
			this.butNotMissing.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNotMissing.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNotMissing.CornerRadius = 4F;
			this.butNotMissing.Location = new System.Drawing.Point(11, 53);
			this.butNotMissing.Name = "butNotMissing";
			this.butNotMissing.Size = new System.Drawing.Size(82, 23);
			this.butNotMissing.TabIndex = 15;
			this.butNotMissing.Text = "Not Missing";
			this.butNotMissing.Click += new System.EventHandler(this.butNotMissing_Click);
			// 
			// butMissing
			// 
			this.butMissing.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMissing.Autosize = true;
			this.butMissing.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMissing.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMissing.CornerRadius = 4F;
			this.butMissing.Location = new System.Drawing.Point(11, 21);
			this.butMissing.Name = "butMissing";
			this.butMissing.Size = new System.Drawing.Size(82, 23);
			this.butMissing.TabIndex = 14;
			this.butMissing.Text = "Missing";
			this.butMissing.Click += new System.EventHandler(this.butMissing_Click);
			// 
			// butHidden
			// 
			this.butHidden.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHidden.Autosize = true;
			this.butHidden.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHidden.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHidden.CornerRadius = 4F;
			this.butHidden.Location = new System.Drawing.Point(172, 21);
			this.butHidden.Name = "butHidden";
			this.butHidden.Size = new System.Drawing.Size(82, 23);
			this.butHidden.TabIndex = 17;
			this.butHidden.Text = "Hidden";
			this.butHidden.Click += new System.EventHandler(this.butHidden_Click);
			// 
			// butClearAllMovements
			// 
			this.butClearAllMovements.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearAllMovements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClearAllMovements.Autosize = true;
			this.butClearAllMovements.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearAllMovements.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearAllMovements.CornerRadius = 4F;
			this.butClearAllMovements.Location = new System.Drawing.Point(159, 169);
			this.butClearAllMovements.Name = "butClearAllMovements";
			this.butClearAllMovements.Size = new System.Drawing.Size(68, 23);
			this.butClearAllMovements.TabIndex = 32;
			this.butClearAllMovements.Text = "Clear All";
			this.butClearAllMovements.Click += new System.EventHandler(this.butClearAllMovements_Click);
			// 
			// butApplyMovements
			// 
			this.butApplyMovements.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butApplyMovements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butApplyMovements.Autosize = true;
			this.butApplyMovements.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butApplyMovements.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butApplyMovements.CornerRadius = 4F;
			this.butApplyMovements.Location = new System.Drawing.Point(394, 169);
			this.butApplyMovements.Name = "butApplyMovements";
			this.butApplyMovements.Size = new System.Drawing.Size(68, 23);
			this.butApplyMovements.TabIndex = 16;
			this.butApplyMovements.Text = "Apply";
			this.butApplyMovements.Click += new System.EventHandler(this.butApplyMovements_Click);
			// 
			// butTipBplus
			// 
			this.butTipBplus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTipBplus.Autosize = true;
			this.butTipBplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipBplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipBplus.CornerRadius = 4F;
			this.butTipBplus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butTipBplus.Location = new System.Drawing.Point(159, 76);
			this.butTipBplus.Name = "butTipBplus";
			this.butTipBplus.Size = new System.Drawing.Size(31, 23);
			this.butTipBplus.TabIndex = 34;
			this.butTipBplus.Text = "+";
			this.butTipBplus.Click += new System.EventHandler(this.butTipBplus_Click);
			// 
			// butTipBminus
			// 
			this.butTipBminus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTipBminus.Autosize = true;
			this.butTipBminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipBminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipBminus.CornerRadius = 4F;
			this.butTipBminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butTipBminus.Location = new System.Drawing.Point(122, 76);
			this.butTipBminus.Name = "butTipBminus";
			this.butTipBminus.Size = new System.Drawing.Size(31, 23);
			this.butTipBminus.TabIndex = 35;
			this.butTipBminus.Text = "-";
			this.butTipBminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butTipBminus.Click += new System.EventHandler(this.butTipBminus_Click);
			// 
			// butTipMplus
			// 
			this.butTipMplus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTipMplus.Autosize = true;
			this.butTipMplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipMplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipMplus.CornerRadius = 4F;
			this.butTipMplus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butTipMplus.Location = new System.Drawing.Point(159, 47);
			this.butTipMplus.Name = "butTipMplus";
			this.butTipMplus.Size = new System.Drawing.Size(31, 23);
			this.butTipMplus.TabIndex = 32;
			this.butTipMplus.Text = "+";
			this.butTipMplus.Click += new System.EventHandler(this.butTipMplus_Click);
			// 
			// butTipMminus
			// 
			this.butTipMminus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTipMminus.Autosize = true;
			this.butTipMminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipMminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipMminus.CornerRadius = 4F;
			this.butTipMminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butTipMminus.Location = new System.Drawing.Point(122, 47);
			this.butTipMminus.Name = "butTipMminus";
			this.butTipMminus.Size = new System.Drawing.Size(31, 23);
			this.butTipMminus.TabIndex = 33;
			this.butTipMminus.Text = "-";
			this.butTipMminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butTipMminus.Click += new System.EventHandler(this.butTipMminus_Click);
			// 
			// butRotatePlus
			// 
			this.butRotatePlus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRotatePlus.Autosize = true;
			this.butRotatePlus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRotatePlus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRotatePlus.CornerRadius = 4F;
			this.butRotatePlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butRotatePlus.Location = new System.Drawing.Point(159, 18);
			this.butRotatePlus.Name = "butRotatePlus";
			this.butRotatePlus.Size = new System.Drawing.Size(31, 23);
			this.butRotatePlus.TabIndex = 30;
			this.butRotatePlus.Text = "+";
			this.butRotatePlus.Click += new System.EventHandler(this.butRotatePlus_Click);
			// 
			// butRotateMinus
			// 
			this.butRotateMinus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRotateMinus.Autosize = true;
			this.butRotateMinus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRotateMinus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRotateMinus.CornerRadius = 4F;
			this.butRotateMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butRotateMinus.Location = new System.Drawing.Point(122, 18);
			this.butRotateMinus.Name = "butRotateMinus";
			this.butRotateMinus.Size = new System.Drawing.Size(31, 23);
			this.butRotateMinus.TabIndex = 31;
			this.butRotateMinus.Text = "-";
			this.butRotateMinus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butRotateMinus.Click += new System.EventHandler(this.butRotateMinus_Click);
			// 
			// textTipB
			// 
			this.textTipB.Location = new System.Drawing.Point(72, 77);
			this.textTipB.MaxVal = 100000000D;
			this.textTipB.MinVal = -100000000D;
			this.textTipB.Name = "textTipB";
			this.textTipB.Size = new System.Drawing.Size(38, 20);
			this.textTipB.TabIndex = 29;
			// 
			// textTipM
			// 
			this.textTipM.Location = new System.Drawing.Point(72, 49);
			this.textTipM.MaxVal = 100000000D;
			this.textTipM.MinVal = -100000000D;
			this.textTipM.Name = "textTipM";
			this.textTipM.Size = new System.Drawing.Size(38, 20);
			this.textTipM.TabIndex = 25;
			// 
			// textRotate
			// 
			this.textRotate.Location = new System.Drawing.Point(72, 20);
			this.textRotate.MaxVal = 100000000D;
			this.textRotate.MinVal = -100000000D;
			this.textRotate.Name = "textRotate";
			this.textRotate.Size = new System.Drawing.Size(38, 20);
			this.textRotate.TabIndex = 21;
			// 
			// butShiftBplus
			// 
			this.butShiftBplus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftBplus.Autosize = true;
			this.butShiftBplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftBplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftBplus.CornerRadius = 4F;
			this.butShiftBplus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftBplus.Location = new System.Drawing.Point(158, 76);
			this.butShiftBplus.Name = "butShiftBplus";
			this.butShiftBplus.Size = new System.Drawing.Size(31, 23);
			this.butShiftBplus.TabIndex = 40;
			this.butShiftBplus.Text = "+";
			this.butShiftBplus.Click += new System.EventHandler(this.butShiftBplus_Click);
			// 
			// butShiftBminus
			// 
			this.butShiftBminus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftBminus.Autosize = true;
			this.butShiftBminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftBminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftBminus.CornerRadius = 4F;
			this.butShiftBminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftBminus.Location = new System.Drawing.Point(121, 76);
			this.butShiftBminus.Name = "butShiftBminus";
			this.butShiftBminus.Size = new System.Drawing.Size(31, 23);
			this.butShiftBminus.TabIndex = 41;
			this.butShiftBminus.Text = "-";
			this.butShiftBminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftBminus.Click += new System.EventHandler(this.butShiftBminus_Click);
			// 
			// butShiftOplus
			// 
			this.butShiftOplus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftOplus.Autosize = true;
			this.butShiftOplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftOplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftOplus.CornerRadius = 4F;
			this.butShiftOplus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftOplus.Location = new System.Drawing.Point(158, 47);
			this.butShiftOplus.Name = "butShiftOplus";
			this.butShiftOplus.Size = new System.Drawing.Size(31, 23);
			this.butShiftOplus.TabIndex = 38;
			this.butShiftOplus.Text = "+";
			this.butShiftOplus.Click += new System.EventHandler(this.butShiftOplus_Click);
			// 
			// butShiftOminus
			// 
			this.butShiftOminus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftOminus.Autosize = true;
			this.butShiftOminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftOminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftOminus.CornerRadius = 4F;
			this.butShiftOminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftOminus.Location = new System.Drawing.Point(121, 47);
			this.butShiftOminus.Name = "butShiftOminus";
			this.butShiftOminus.Size = new System.Drawing.Size(31, 23);
			this.butShiftOminus.TabIndex = 39;
			this.butShiftOminus.Text = "-";
			this.butShiftOminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftOminus.Click += new System.EventHandler(this.butShiftOminus_Click);
			// 
			// butShiftMplus
			// 
			this.butShiftMplus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftMplus.Autosize = true;
			this.butShiftMplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftMplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftMplus.CornerRadius = 4F;
			this.butShiftMplus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftMplus.Location = new System.Drawing.Point(158, 18);
			this.butShiftMplus.Name = "butShiftMplus";
			this.butShiftMplus.Size = new System.Drawing.Size(31, 23);
			this.butShiftMplus.TabIndex = 36;
			this.butShiftMplus.Text = "+";
			this.butShiftMplus.Click += new System.EventHandler(this.butShiftMplus_Click);
			// 
			// butShiftMminus
			// 
			this.butShiftMminus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShiftMminus.Autosize = true;
			this.butShiftMminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftMminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftMminus.CornerRadius = 4F;
			this.butShiftMminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butShiftMminus.Location = new System.Drawing.Point(121, 18);
			this.butShiftMminus.Name = "butShiftMminus";
			this.butShiftMminus.Size = new System.Drawing.Size(31, 23);
			this.butShiftMminus.TabIndex = 37;
			this.butShiftMminus.Text = "-";
			this.butShiftMminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftMminus.Click += new System.EventHandler(this.butShiftMminus_Click);
			// 
			// textShiftB
			// 
			this.textShiftB.Location = new System.Drawing.Point(72, 77);
			this.textShiftB.MaxVal = 100000000D;
			this.textShiftB.MinVal = -100000000D;
			this.textShiftB.Name = "textShiftB";
			this.textShiftB.Size = new System.Drawing.Size(38, 20);
			this.textShiftB.TabIndex = 29;
			// 
			// textShiftO
			// 
			this.textShiftO.Location = new System.Drawing.Point(72, 49);
			this.textShiftO.MaxVal = 100000000D;
			this.textShiftO.MinVal = -100000000D;
			this.textShiftO.Name = "textShiftO";
			this.textShiftO.Size = new System.Drawing.Size(38, 20);
			this.textShiftO.TabIndex = 25;
			// 
			// textShiftM
			// 
			this.textShiftM.Location = new System.Drawing.Point(72, 20);
			this.textShiftM.MaxVal = 100000000D;
			this.textShiftM.MinVal = -100000000D;
			this.textShiftM.Name = "textShiftM";
			this.textShiftM.Size = new System.Drawing.Size(38, 20);
			this.textShiftM.TabIndex = 21;
			// 
			// butPerm
			// 
			this.butPerm.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPerm.Autosize = true;
			this.butPerm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPerm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPerm.CornerRadius = 4F;
			this.butPerm.Location = new System.Drawing.Point(11, 53);
			this.butPerm.Name = "butPerm";
			this.butPerm.Size = new System.Drawing.Size(82, 23);
			this.butPerm.TabIndex = 15;
			this.butPerm.Text = "Permanent";
			this.butPerm.Click += new System.EventHandler(this.butPerm_Click);
			// 
			// butPrimary
			// 
			this.butPrimary.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrimary.Autosize = true;
			this.butPrimary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrimary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrimary.CornerRadius = 4F;
			this.butPrimary.Location = new System.Drawing.Point(11, 21);
			this.butPrimary.Name = "butPrimary";
			this.butPrimary.Size = new System.Drawing.Size(82, 23);
			this.butPrimary.TabIndex = 14;
			this.butPrimary.Text = "Primary";
			this.butPrimary.Click += new System.EventHandler(this.butPrimary_Click);
			// 
			// butMixed
			// 
			this.butMixed.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMixed.Autosize = true;
			this.butMixed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMixed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMixed.CornerRadius = 4F;
			this.butMixed.Location = new System.Drawing.Point(334, 33);
			this.butMixed.Name = "butMixed";
			this.butMixed.Size = new System.Drawing.Size(107, 23);
			this.butMixed.TabIndex = 20;
			this.butMixed.Text = "Set Mixed Dentition";
			this.butMixed.Click += new System.EventHandler(this.butMixed_Click);
			// 
			// butAllPrimary
			// 
			this.butAllPrimary.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAllPrimary.Autosize = true;
			this.butAllPrimary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllPrimary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllPrimary.CornerRadius = 4F;
			this.butAllPrimary.Location = new System.Drawing.Point(201, 33);
			this.butAllPrimary.Name = "butAllPrimary";
			this.butAllPrimary.Size = new System.Drawing.Size(107, 23);
			this.butAllPrimary.TabIndex = 19;
			this.butAllPrimary.Text = "Set All Primary";
			this.butAllPrimary.Click += new System.EventHandler(this.butAllPrimary_Click);
			// 
			// butAllPerm
			// 
			this.butAllPerm.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAllPerm.Autosize = true;
			this.butAllPerm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllPerm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllPerm.CornerRadius = 4F;
			this.butAllPerm.Location = new System.Drawing.Point(201, 65);
			this.butAllPerm.Name = "butAllPerm";
			this.butAllPerm.Size = new System.Drawing.Size(107, 23);
			this.butAllPerm.TabIndex = 18;
			this.butAllPerm.Text = "Set All Permanent";
			this.butAllPerm.Click += new System.EventHandler(this.butAllPerm_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(283, 1);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(33, 23);
			this.butDown.TabIndex = 195;
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(244, 1);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(33, 23);
			this.butUp.TabIndex = 194;
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butPin
			// 
			this.butPin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPin.Autosize = true;
			this.butPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPin.CornerRadius = 4F;
			this.butPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPin.Location = new System.Drawing.Point(163, 1);
			this.butPin.Name = "butPin";
			this.butPin.Size = new System.Drawing.Size(75, 23);
			this.butPin.TabIndex = 6;
			this.butPin.Text = "Pin Board";
			this.butPin.Click += new System.EventHandler(this.butPin_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClear.Location = new System.Drawing.Point(82, 1);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(75, 23);
			this.butClear.TabIndex = 5;
			this.butClear.Text = "Delete";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Image = global::OpenDental.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(1, 1);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(75, 23);
			this.butNew.TabIndex = 4;
			this.butNew.Text = "Add";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// butShowDateRange
			// 
			this.butShowDateRange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowDateRange.Autosize = true;
			this.butShowDateRange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowDateRange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowDateRange.CornerRadius = 4F;
			this.butShowDateRange.Location = new System.Drawing.Point(273, 186);
			this.butShowDateRange.Name = "butShowDateRange";
			this.butShowDateRange.Size = new System.Drawing.Size(24, 22);
			this.butShowDateRange.TabIndex = 47;
			this.butShowDateRange.Text = "...";
			this.butShowDateRange.UseVisualStyleBackColor = true;
			this.butShowDateRange.Click += new System.EventHandler(this.butShowDateRange_Click);
			// 
			// butChartViewDown
			// 
			this.butChartViewDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChartViewDown.Autosize = true;
			this.butChartViewDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChartViewDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChartViewDown.CornerRadius = 4F;
			this.butChartViewDown.Image = global::OpenDental.Properties.Resources.down;
			this.butChartViewDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butChartViewDown.Location = new System.Drawing.Point(426, 195);
			this.butChartViewDown.Name = "butChartViewDown";
			this.butChartViewDown.Size = new System.Drawing.Size(68, 24);
			this.butChartViewDown.TabIndex = 41;
			this.butChartViewDown.Text = "&Down";
			this.butChartViewDown.Click += new System.EventHandler(this.butChartViewDown_Click);
			// 
			// butChartViewUp
			// 
			this.butChartViewUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChartViewUp.Autosize = true;
			this.butChartViewUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChartViewUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChartViewUp.CornerRadius = 4F;
			this.butChartViewUp.Image = global::OpenDental.Properties.Resources.up;
			this.butChartViewUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butChartViewUp.Location = new System.Drawing.Point(367, 195);
			this.butChartViewUp.Name = "butChartViewUp";
			this.butChartViewUp.Size = new System.Drawing.Size(54, 24);
			this.butChartViewUp.TabIndex = 42;
			this.butChartViewUp.Text = "&Up";
			this.butChartViewUp.Click += new System.EventHandler(this.butChartViewUp_Click);
			// 
			// butChartViewAdd
			// 
			this.butChartViewAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChartViewAdd.Autosize = true;
			this.butChartViewAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChartViewAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChartViewAdd.CornerRadius = 4F;
			this.butChartViewAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butChartViewAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butChartViewAdd.Location = new System.Drawing.Point(303, 195);
			this.butChartViewAdd.Name = "butChartViewAdd";
			this.butChartViewAdd.Size = new System.Drawing.Size(59, 24);
			this.butChartViewAdd.TabIndex = 40;
			this.butChartViewAdd.Text = "&Add";
			this.butChartViewAdd.Click += new System.EventHandler(this.butChartViewAdd_Click);
			// 
			// butShowAll
			// 
			this.butShowAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowAll.Autosize = true;
			this.butShowAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowAll.CornerRadius = 4F;
			this.butShowAll.Location = new System.Drawing.Point(6, 129);
			this.butShowAll.Name = "butShowAll";
			this.butShowAll.Size = new System.Drawing.Size(53, 23);
			this.butShowAll.TabIndex = 12;
			this.butShowAll.Text = "All";
			this.butShowAll.Click += new System.EventHandler(this.butShowAll_Click);
			// 
			// butShowNone
			// 
			this.butShowNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowNone.Autosize = true;
			this.butShowNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowNone.CornerRadius = 4F;
			this.butShowNone.Location = new System.Drawing.Point(69, 129);
			this.butShowNone.Name = "butShowNone";
			this.butShowNone.Size = new System.Drawing.Size(58, 23);
			this.butShowNone.TabIndex = 13;
			this.butShowNone.Text = "None";
			this.butShowNone.Click += new System.EventHandler(this.butShowNone_Click);
			// 
			// butColorOther
			// 
			this.butColorOther.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butColorOther.Autosize = true;
			this.butColorOther.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butColorOther.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butColorOther.CornerRadius = 4F;
			this.butColorOther.Location = new System.Drawing.Point(95, 179);
			this.butColorOther.Name = "butColorOther";
			this.butColorOther.Size = new System.Drawing.Size(50, 24);
			this.butColorOther.TabIndex = 192;
			this.butColorOther.Text = "Other";
			this.butColorOther.Click += new System.EventHandler(this.butColorOther_Click);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(939, 25);
			this.ToolBarMain.TabIndex = 177;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// button1
			// 
			this.button1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button1.Autosize = true;
			this.button1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button1.CornerRadius = 4F;
			this.button1.Location = new System.Drawing.Point(127, 692);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 36;
			this.button1.Text = "invisible";
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textTreatmentNotes
			// 
			this.textTreatmentNotes.AcceptsTab = true;
			this.textTreatmentNotes.DetectUrls = false;
			this.textTreatmentNotes.Location = new System.Drawing.Point(0, 334);
			this.textTreatmentNotes.Name = "textTreatmentNotes";
			this.textTreatmentNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.ChartTreatment;
			this.textTreatmentNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textTreatmentNotes.Size = new System.Drawing.Size(411, 71);
			this.textTreatmentNotes.TabIndex = 187;
			this.textTreatmentNotes.Text = "";
			this.textTreatmentNotes.TextChanged += new System.EventHandler(this.textTreatmentNotes_TextChanged);
			this.textTreatmentNotes.Leave += new System.EventHandler(this.textTreatmentNotes_Leave);
			// 
			// ContrChart
			// 
			this.Controls.Add(this.panelTP);
			this.Controls.Add(this.gridProg);
			this.Controls.Add(this.butErxAccess);
			this.Controls.Add(this.butPhoneNums);
			this.Controls.Add(this.panelEcw);
			this.Controls.Add(this.butForeignKey);
			this.Controls.Add(this.butAddKey);
			this.Controls.Add(this.toothChart);
			this.Controls.Add(this.tabProc);
			this.Controls.Add(this.panelImages);
			this.Controls.Add(this.tabControlImages);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textTreatmentNotes);
			this.Controls.Add(this.gridPtInfo);
			this.Name = "ContrChart";
			this.Size = new System.Drawing.Size(939, 708);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrChart_Layout);
			this.Resize += new System.EventHandler(this.ContrChart_Resize);
			this.groupBox2.ResumeLayout(false);
			this.tabControlImages.ResumeLayout(false);
			this.panelImages.ResumeLayout(false);
			this.tabProc.ResumeLayout(false);
			this.tabEnterTx.ResumeLayout(false);
			this.tabEnterTx.PerformLayout();
			this.tabMissing.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabMovements.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPrimary.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.tabPlanned.ResumeLayout(false);
			this.tabShow.ResumeLayout(false);
			this.tabShow.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.tabDraw.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.tabCustomer.ResumeLayout(false);
			this.tabCustomer.PerformLayout();
			this.panelEcw.ResumeLayout(false);
			this.panelTP.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ContrChart_Layout(object sender, System.Windows.Forms.LayoutEventArgs e){
			gridProg.Height=ClientSize.Height-tabControlImages.Height-gridProg.Location.Y+1;
			gridPtInfo.Height=tabControlImages.Top-gridPtInfo.Top;
			if(panelImages.Visible) {
				gridProg.Height-=(panelImages.Height+2);
				gridPtInfo.Height-=(panelImages.Height+2);
			}
			gridProg.Invalidate();
			gridPtInfo.Invalidate();
		}

		///<summary>Made public to be able to resize the controls without having to display them.</summary>
		public void ContrChart_Resize(object sender,EventArgs e) {
			ChartLayoutHelper.Resize(gridProg,panelImages,panelEcw,tabControlImages,ClientSize,gridPtInfo,toothChart,textTreatmentNotes);
			ChartLayoutHelper.SetTpChartingHelper((ChartViewCurDisplay!=null && ChartViewCurDisplay.IsTpCharting),PatCur,gridProg,listButtonCats,
				checkTreatPlans,panelTP,gridTreatPlans,gridTpProcs,butNewTP,listPriorities);
		}

		///<summary></summary>
		public void InitializeOnStartup(){
			if(InitializedOnStartup) {
				return;
			}
			InitializedOnStartup=true;
			newStatus=ProcStat.TP;
			ChartLayoutHelper.InitializeOnStartup(this,tabProc,gridProg,panelEcw,tabControlImages,ClientSize,gridPtInfo,toothChart,textTreatmentNotes,butECWup,butECWdown,tabPatInfo);
			ChartLayoutHelper.SetTpChartingHelper((ChartViewCurDisplay!=null && ChartViewCurDisplay.IsTpCharting),PatCur,gridProg,listButtonCats,
				checkTreatPlans,panelTP,gridTreatPlans,gridTpProcs,butNewTP,listPriorities);
			//can't use Lan.F
			Lan.C(this,new Control[]{
				checkDone,
				butNew,
				butClear,
				checkShowTP,
				checkShowC,
				checkShowE,
				checkShowR,
				checkRx,
				checkNotes,
				checkTreatPlans,
				labelDx,
				butM,
				butOI,
				butD,
				butL,
				butBF,
				butV,
				groupBox2,
				radioEntryTP,
				radioEntryC,
				radioEntryEC,
				radioEntryEO,
				radioEntryR,
				checkToday,
				labelDx,
				label6,
				butAddProc,
				label14,
				//textProcCode is handled in ClearButtons()
				butOK,
				label13,
				tabEnterTx,
				tabMissing,
				tabMovements,
				tabPrimary,
				tabPlanned,
				tabShow,
				tabDraw},
				true);
			LayoutToolBar();
			//ComputerPref localComputerPrefs=ComputerPrefs.GetForLocalComputer();
			this.toothChart.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			this.toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;//triggers ResetControls.
			Plugins.HookAddCode(this,"ContrChart.InitializeOnStartup_end",PatCur);
		}

		///<summary>Called every time prefs are changed from any workstation.</summary>
		public void InitializeLocalData(){
			IsDistributorKey=PrefC.GetBool(PrefName.DistributorKey);
			if(!IsDistributorKey) {
				butAddKey.Visible=false;
				butForeignKey.Visible=false;
				butPhoneNums.Visible=false;
				butErxAccess.Visible=false;
				tabProc.TabPages.Remove(tabCustomer);
			}
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
			toothChart.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			//Must be last preference set here, because this causes the 
																													//pixel format to be recreated.
			toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;
			//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			if(PatCur!=null){
				FillToothChart(true);
			}
			//if(PrefC.GetBoolSilent(PrefName.ChartQuickAddHideAmalgam,true)){ //Preference is Deprecated.
			//	panelQuickPasteAmalgam.Visible=false;
			//}
			//else{
			//	panelQuickPasteAmalgam.Visible=true;
			//}
			if(ToolButItems.List!=null){
				LayoutToolBar();
				if(PatCur==null) {
					if(HasHideRxButtonsEcw()) {
						//Don't show the Rx and eRx buttons.
					}
					else {
						if(UsingEcwTightOrFull()) {
							if(!Environment.Is64BitOperatingSystem) {
								ToolBarMain.Buttons["Rx"].Enabled=false;
							}
							//eRx already disabled because it is never enabled for eCW Tight or Full
						}
						else {
							ToolBarMain.Buttons["Rx"].Enabled=false;
							ToolBarMain.Buttons["eRx"].Enabled=false;
						}
					}
					ToolBarMain.Buttons["LabCase"].Enabled=false;
					if(ToolBarMain.Buttons["Perio"]!=null) {
						ToolBarMain.Buttons["Perio"].Enabled = false;
					}
					if(ToolBarMain.Buttons["Ortho"]!=null) {
						ToolBarMain.Buttons["Ortho"].Enabled = false;
					}
					ToolBarMain.Buttons["Consent"].Enabled = false;
					if(ToolBarMain.Buttons["ToothChart"]!=null) {
						ToolBarMain.Buttons["ToothChart"].Enabled = false;
					}
					ToolBarMain.Buttons["ExamSheet"].Enabled=false;
					if(UsingEcwTight()) {
						ToolBarMain.Buttons["Commlog"].Enabled=false;
						webBrowserEcw.Url=null;
					}
					if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
						ToolBarMain.Buttons["EHR"].Enabled=false;
					}
					if(ToolBarMain.Buttons["HL7"]!=null) {
						ToolBarMain.Buttons["HL7"].Enabled=false;
					}
				}
			}
		}

		///<summary>This reduces the number of places where Programs.UsingEcwTight() is called.  This helps with organization.  All calls from ContrChart must pass through here.  They also must have been checked to not involve the Orion bridge or layout logic.</summary>
		private bool UsingEcwTight() {
			return Programs.UsingEcwTightMode();
		}

		///<summary>This reduces the number of places where Programs.UsingEcwTightOrFull() is called.  This helps with organization.  All calls from ContrChart must pass through here.  They also must have been checked to not involve the Orion bridge or layout logic.</summary>
		private bool UsingEcwTightOrFull() {
			return Programs.UsingEcwTightOrFullMode();
		}

		///<summary>Returns true if eCW is enabled and they turned on the Hide Chart Rx Buttons setting within the program link.</summary>
		private bool HasHideRxButtonsEcw() {
			if(Programs.IsEnabled(ProgramName.eClinicalWorks) 
				&& ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"HideChartRxButtons")=="1") 
			{
				return true;
			}
			return false;
		}

		///<summary>Causes the toolbars to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			if(HasHideRxButtonsEcw()) {
				//Don't show the Rx and eRx buttons.
			}
			else {
				if(UsingEcwTightOrFull()) {
					if(!Environment.Is64BitOperatingSystem) {
						ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"New Rx"),1,"","Rx"));
					}
					//don't add eRx
				}
				else {
					ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"New Rx"),1,"","Rx"));
					button=new ODToolBarButton(Lan.g(this,"eRx"),1,"","eRx");
					button.Style=ODToolBarButtonStyle.DropDownButton;
					button.DropDownMenu=menuErx;
					ToolBarMain.Buttons.Add(button);
				}
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"LabCase"),-1,"","LabCase"));
			if(!Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Perio Chart"),2,"","Perio"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Ortho Chart"),-1,"","Ortho"));
			}			
			button=new ODToolBarButton(Lan.g(this,"Consent"),-1,"","Consent");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=menuConsent;
			ToolBarMain.Buttons.Add(button);
			//if(PrefC.GetBool(PrefName.ToothChartMoveMenuToRight)) {
			//	ToolBarMain.Buttons.Add(new ODToolBarButton("                   .",-1,"",""));
			//}
			if(!Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
				button=new ODToolBarButton(Lan.g(this,"Tooth Chart"),-1,"","ToothChart");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuToothChart;
				ToolBarMain.Buttons.Add(button);
			}
			button=new ODToolBarButton(Lan.g(this,"Exam Sheet"),-1,"","ExamSheet");
			button.Style=ODToolBarButtonStyle.PushButton;
			ToolBarMain.Buttons.Add(button);
			if(UsingEcwTight()) {//button will show in this toolbar instead of the usual one.
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Commlog"),4,Lan.g(this,"New Commlog Entry"),"Commlog"));
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("EHR",-1,"","EHR"));
			}
			if(HL7Defs.IsExistingHL7Enabled() && !UsingEcwTightOrFull()) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(HL7Defs.GetOneDeepEnabled().Description,-1,"","HL7"));
			}
			HL7Def hl7DefCur=HL7Defs.GetOneDeepEnabled(true);
			if(hl7DefCur!=null) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(hl7DefCur.Description,-1,"","MedLab"));
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.ChartModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrChart.LayoutToolBar_end",PatCur);
		}

		///<summary>This function does not follow our usual pattern. This function is just like ModuleSelected() but also pulls down prescription data for
		///the current patient from the NewCrop web service. It was created to limit the number of times that NewCrop prescriptions are refreshed. Each 
		///time that NewCrop data is refreshed, the user must wait at least a few seconds. This function allows the chart to fully load, then a wait 
		///cursor displays while the web service call is being performed. If new data is pulled in from NewCrop, then the module is refreshed again to 
		///show the new prescriptions. Only called from FormOpenDental when the Chart module button is clicked or when a new patient is selected from 
		///within the Chart.</summary>
		public void ModuleSelectedNewCrop(long patNum) {
			ModuleSelected(patNum,true);
			if(_patNumLastErx!=patNum) {
				this.Cursor=Cursors.WaitCursor;
				Application.DoEvents();
				if(NewCropRefreshPrescriptions()) {//This is where _patNumLastErx is set to the current patient.
					ModuleSelected(patNum);
				}
				this.Cursor=Cursors.Default;
			}
		}

		/// <summary></summary>
		public void ModuleSelected(long patNum) {
			ModuleSelected(patNum,true,false);
		}

		/// <summary>Only use this overload when isClinicRefresh is set to true.  This is only used when calling ModuleSelected from FromOpenDental.
		///When isClinicRefresh is true the tab control tabProc is redrawn and only needs to be done when the clinic is changed or the module is selected
		///for the first time.</summary>
		public void ModuleSelected(long patNum,bool isClinicRefresh) {
			ModuleSelected(patNum,true,isClinicRefresh);
		}

		///<summary>Only use this overload when isFullRefresh is set to false.  This is ONLY in a few places and only for eCW at this point.  Speeds things up by refreshing less data.</summary>
		public void ModuleSelected(long patNum,bool isFullRefresh,bool isClinicRefresh) {
			EasyHideClinicalData();
			RefreshModuleData(patNum,isFullRefresh);
			RefreshModuleScreen(isClinicRefresh);
			Plugins.HookAddCode(this,"ContrChart.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			//toothChart.Dispose();?
			if(FamCur==null)
				return;
			if(TreatmentNoteChanged){
				PatientNoteCur.Treatment=textTreatmentNotes.Text;
				PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
				TreatmentNoteChanged=false;
			}
			FamCur=null;
			PatCur=null;
			PlanList=null;
			SubList=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrChart.ModuleUnselected_end");
		}
		
		///<summary>isFullRefresh is ONLY for eCW at this point.</summary>
		private void RefreshModuleData(long patNum,bool isFullRefresh) {
			if(patNum==0){
				PatCur=null;
				FamCur=null;
				return;
			}
			if(!isFullRefresh) {
				return;
			}
			FamCur=Patients.GetFamily(patNum);
			PatCur=FamCur.GetPatient(patNum);
			SubList=InsSubs.RefreshForFam(FamCur);
			PlanList=InsPlans.RefreshForSubList(SubList);
			PatPlanList=PatPlans.Refresh(patNum);
			BenefitList=Benefits.Refresh(PatPlanList,SubList);
			_listClaimProcHists=ClaimProcs.GetHistList(patNum,BenefitList,PatPlanList,PlanList,DateTimeOD.Today,SubList);
//todo: track down where this is altered.  Optimize for eCW:
			PatientNoteCur=PatientNotes.Refresh(patNum,PatCur.Guarantor);
			if(PrefC.AtoZfolderUsed) {
				patFolder=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());//GetImageFolder();
			}
			DocumentList=Documents.GetAllWithPat(patNum);
//todo: might change for planned appt:
			ApptList=Appointments.GetForPat(patNum);
//todo: refresh as needed elsewhere:
			ToothInitialList=ToothInitials.Refresh(patNum);
//todo: optimize for Full mode:
			PatFieldList=PatFields.Refresh(patNum);
			if(_patNumLast!=patNum) {
				SecurityLogs.MakeLogEntry(Permissions.ChartModule,patNum,"");
				_patNumLast=patNum;
			}
		}		

		private void RefreshModuleScreen(bool isClinicRefresh=false){
			//ParentForm.Text=Patients.GetMainTitle(PatCur);
			LayoutToolBar();
			if(PatCur==null){
				//groupShow.Enabled=false;
				gridPtInfo.Enabled=false;
				//tabPlanned.Enabled=false;
				toothChart.Enabled=false;
				gridProg.Enabled=false;
				if(HasHideRxButtonsEcw()) {
					//Don't show the Rx and eRx buttons.
				}
				else {
					//if(UsingEcwTight()) {
					if(UsingEcwTightOrFull()) {
						if(!Environment.Is64BitOperatingSystem) {
							ToolBarMain.Buttons["Rx"].Enabled=false;
						}
						//eRx already disabled because it is never enabled for eCW Tight or Full
					}
					else {
						ToolBarMain.Buttons["Rx"].Enabled=false;
						ToolBarMain.Buttons["eRx"].Enabled=false;
					}
				}
				ToolBarMain.Buttons["LabCase"].Enabled=false;
				if(ToolBarMain.Buttons["Perio"]!=null) {
					ToolBarMain.Buttons["Perio"].Enabled = false;
				}
				if(ToolBarMain.Buttons["Ortho"]!=null) {
					ToolBarMain.Buttons["Ortho"].Enabled = false;
				}
				ToolBarMain.Buttons["Consent"].Enabled = false;
				if(ToolBarMain.Buttons["ToothChart"]!=null) {
					ToolBarMain.Buttons["ToothChart"].Enabled = false;
				}
				ToolBarMain.Buttons["ExamSheet"].Enabled=false;
				if(UsingEcwTight()) {
					ToolBarMain.Buttons["Commlog"].Enabled=false;
					webBrowserEcw.Url=null;
				}
				//if(FormOpenDental.ObjSomeEhrSuperClass!=null) {//didn't work
				if(ToolBarMain.Buttons["EHR"]!=null){
					ToolBarMain.Buttons["EHR"].Enabled=false;
				}
				if(ToolBarMain.Buttons["HL7"]!=null) {
					ToolBarMain.Buttons["HL7"].Enabled=false;
				}
				tabProc.Enabled = false;
				butAddKey.Enabled=false;
				butForeignKey.Enabled=false;
				butPhoneNums.Enabled=false;
				butErxAccess.Enabled=false;
			}
			else {
				//groupShow.Enabled=true;
				gridPtInfo.Enabled=true;
				//groupPlanned.Enabled=true;
				toothChart.Enabled=true;
				gridProg.Enabled=true;
				if(HasHideRxButtonsEcw()) {
					//Don't show the Rx and eRx buttons.
				}
				else {
					//if(UsingEcwTight()) {
					if(UsingEcwTightOrFull()) {
						if(!Environment.Is64BitOperatingSystem) {
							ToolBarMain.Buttons["Rx"].Enabled=true;
						}
						//don't enable eRx
					}
					else {
						ToolBarMain.Buttons["Rx"].Enabled=true;
						ToolBarMain.Buttons["eRx"].Enabled=true;
					}
				}
				ToolBarMain.Buttons["LabCase"].Enabled=true;
				if(ToolBarMain.Buttons["Perio"]!=null) {
					ToolBarMain.Buttons["Perio"].Enabled = true;
				}
				if(ToolBarMain.Buttons["Ortho"]!=null) {
					ToolBarMain.Buttons["Ortho"].Enabled = true;
				}
				ToolBarMain.Buttons["Consent"].Enabled = true;
				if(ToolBarMain.Buttons["ToothChart"]!=null) {
					ToolBarMain.Buttons["ToothChart"].Enabled =true;
				}
				ToolBarMain.Buttons["ExamSheet"].Enabled=true;
				if(UsingEcwTightOrFull()) {
					if(UsingEcwTight()) {
						ToolBarMain.Buttons["Commlog"].Enabled=true;
					}
					//the following sequence also gets repeated after exiting the Rx window to refresh.
					String strAppServer="";
					try {
						if(Bridges.ECW.UserId==0 || String.IsNullOrEmpty(Bridges.ECW.EcwConfigPath)) {
							webBrowserEcw.Url=null;
							labelECWerror.Text="This panel does not display unless\r\nOpen Dental is launched from inside eCW";
							labelECWerror.Visible=true;
						}
						else {
							//this property will not exist if using Oracle, eCW will never use Oracle
							string uristring=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"MedicalPanelUrl");
							string path="";
							if(uristring=="") {
								XmlTextReader reader=new XmlTextReader(Bridges.ECW.EcwConfigPath);
								while(reader.Read()) {
									if(reader.Name.ToString()=="server") {
										strAppServer=reader.ReadString().Trim();
									}
								}
								path="http://"+strAppServer+"/mobiledoc/jsp/dashboard/Overview.jsp"
									+"?ptId="+PatCur.PatNum.ToString()+"&panelName=overview&pnencid="
									+Bridges.ECW.AptNum.ToString()+"&context=progressnotes&TrUserId="+Bridges.ECW.UserId.ToString();
								//set cookie
								if(!String.IsNullOrEmpty(Bridges.ECW.JSessionId)) {
									InternetSetCookie("http://"+strAppServer,null,"JSESSIONID = "+Bridges.ECW.JSessionId);
								}
								if(!String.IsNullOrEmpty(Bridges.ECW.JSessionIdSSO)) {
									InternetSetCookie("http://"+strAppServer,null,"JSESSIONIDSSO = "+Bridges.ECW.JSessionIdSSO);
								}
								if(!String.IsNullOrEmpty(Bridges.ECW.LBSessionId)) {
									if(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"IsLBSessionIdExcluded")=="0") {
										InternetSetCookie("http://"+strAppServer,null,"LBSESSIONID = "+Bridges.ECW.LBSessionId);
									}
									else {
										InternetSetCookie("http://"+strAppServer,null,Bridges.ECW.LBSessionId);
									}
								}
							}
							else {
								path=uristring
									+"?ptId="+PatCur.PatNum.ToString()+"&panelName=overview&pnencid="
									+Bridges.ECW.AptNum.ToString()+"&context=progressnotes&TrUserId="+Bridges.ECW.UserId.ToString();
								//parse out with regex: uristring
								Match match=Regex.Match(uristring,@"\b([^:]+://[^/]+)/");//http://servername
								if(!match.Success || match.Groups.Count<2) {//if no match, or match but no group 1 to grab
									throw new Exception("Invalid URL saved in the Medical Panel URL field of the eClinicalWorks program link.");
								}
								string cookieUrl=match.Groups[1].Value;
								//set cookie
								if(!String.IsNullOrEmpty(Bridges.ECW.JSessionId)) {
									InternetSetCookie(cookieUrl,null,"JSESSIONID = "+Bridges.ECW.JSessionId);
								}
								if(!String.IsNullOrEmpty(Bridges.ECW.JSessionIdSSO)) {
									InternetSetCookie(cookieUrl,null,"JSESSIONIDSSO = "+Bridges.ECW.JSessionIdSSO);
								}
								if(!String.IsNullOrEmpty(Bridges.ECW.LBSessionId)) {
									if(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"IsLBSessionIdExcluded")=="0") {
										InternetSetCookie(cookieUrl,null,"LBSESSIONID = "+Bridges.ECW.LBSessionId);
									}
									else {
										InternetSetCookie(cookieUrl,null,Bridges.ECW.LBSessionId);
									}
								}
							}
							//navigate
							webBrowserEcw.Navigate(path); 
							labelECWerror.Visible=false;
						}
					}
					catch (Exception ex){
						webBrowserEcw.Url=null;
						labelECWerror.Text="Error: "+ex.Message;
						labelECWerror.Visible=true;
					}
				}
				if(PrefC.GetBool(PrefName.ShowFeatureEhr)) { //didn't work either
				//if(ToolBarMain.Buttons["EHR"]!=null) {
					ToolBarMain.Buttons["EHR"].Enabled=true;
				}
				if(ToolBarMain.Buttons["HL7"]!=null) {
					ToolBarMain.Buttons["HL7"].Enabled=true;
				}
				tabProc.Enabled=true;
				butAddKey.Enabled=true;
				butForeignKey.Enabled=true;
				butPhoneNums.Enabled=true;
				butErxAccess.Enabled=true;
				if(PrevPtNum != PatCur.PatNum) {//reset to TP status on every new patient selected
					if(PrefC.GetBool(PrefName.AutoResetTPEntryStatus)) {
						radioEntryTP.Select();
					}
					PrevPtNum = PatCur.PatNum;
				}
			}
			if(Programs.UsingOrion) {
				radioEntryC.Visible=false;
				radioEntryEC.Visible=false;
				radioEntryR.Visible=false;
				radioEntryCn.Visible=false;
				radioEntryEO.Location=new Point(radioEntryEO.Location.X,31);
				groupBox2.Height=54;
				menuItemSetComplete.Visible=false;
			}
			if(!UsingEcwTightOrFull() && isClinicRefresh) {
				ChartLayoutHelper.SetToothChartVisibleHelper(toothChart,textTreatmentNotes);
				if(Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
					tabProc.TabPages.Remove(tabMissing);
					tabProc.TabPages.Remove(tabMovements);
					tabProc.TabPages.Remove(tabPrimary);
					if(tabProc.SelectedTab==tabMissing || tabProc.SelectedTab==tabMovements || tabProc.SelectedTab==tabPrimary) {
						tabProc.SelectedTab=tabEnterTx;
					}
					SelectedProcTab=tabProc.SelectedIndex;
					textSurf.Visible=false;
					butBF.Visible=false;
					butOI.Visible=false;
					butV.Visible=false;
					butM.Visible=false;
					butD.Visible=false;
					butL.Visible=false;
					checkShowTeeth.Visible=false;
				}
				else {
					TabPage selectedTab=tabProc.SelectedTab;
					tabProc.TabPages.Remove(tabMissing);
					tabProc.TabPages.Remove(tabMovements);
					tabProc.TabPages.Remove(tabPrimary);
					tabProc.TabPages.Remove(tabPlanned);
					tabProc.TabPages.Remove(tabShow);
					tabProc.TabPages.Remove(tabDraw);
					tabProc.TabPages.Add(tabMissing);
					tabProc.TabPages.Add(tabMovements);
					tabProc.TabPages.Add(tabPrimary);
					tabProc.TabPages.Add(tabPlanned);
					tabProc.TabPages.Add(tabShow);
					tabProc.TabPages.Add(tabDraw);
					tabProc.SelectedTab=selectedTab;
					SelectedProcTab=tabProc.SelectedIndex;
					textSurf.Visible=true;
					butBF.Visible=true;
					butOI.Visible=true;
					butV.Visible=true;
					butM.Visible=true;
					butD.Visible=true;
					butL.Visible=true;
					checkShowTeeth.Visible=true;
				}
			}
			ToolBarMain.Invalidate();
			ClearButtons();
			FillChartViewsGrid();
			if(IsDistributorKey) {
				FillCustomerTab();
			}
			FillProgNotes();
			FillPlanned();
			FillPtInfo();
			FillDxProcImage();
			FillImages();
		}

		private void EasyHideClinicalData(){
			if(PrefC.GetBool(PrefName.EasyHideClinical)){
				gridPtInfo.Visible=false;
				checkShowE.Visible=false;
				checkShowR.Visible=false;
				checkRx.Visible=false;
				checkComm.Visible=false;
				checkNotes.Visible=false;
				butShowNone.Visible=false;
				butShowAll.Visible=false;
				//panelEnterTx.Visible=false;//next line changes it, though
				radioEntryEC.Visible=false;
				radioEntryEO.Visible=false;
				radioEntryR.Visible=false;
				labelDx.Visible=false;
				listDx.Visible=false;
				labelPrognosis.Visible=false;
				comboPrognosis.Visible=false;
			}
			else{
				gridPtInfo.Visible=true;
				checkShowE.Visible=true;
				checkShowR.Visible=true;
				checkRx.Visible=true;
				checkComm.Visible=true;
				checkNotes.Visible=true;
				butShowNone.Visible=true;
				butShowAll.Visible=true;
				radioEntryEC.Visible=true;
				radioEntryEO.Visible=true;
				radioEntryR.Visible=true;
				labelDx.Visible=true;
				listDx.Visible=true;
				labelPrognosis.Visible=true;
				comboPrognosis.Visible=true;
			}
		}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					case "Rx":
						Tool_Rx_Click();
						break;
					case "eRx":
						Tool_eRx_Click();
						break;
					case "LabCase":
						Tool_LabCase_Click();
						break;
					case "Perio":
						Tool_Perio_Click();
						break;
					case "Ortho":
						Tool_Ortho_Click();
						break;
					case "Anesthesia":
						Tool_Anesthesia_Click();
						break;
					case "Consent":
						Tool_Consent_Click();
						break;
					case "Commlog"://only for eCW
						Tool_Commlog_Click();
						break;
					case "ToothChart":
						Tool_ToothChart_Click();
						break;
					case "ExamSheet":
						Tool_ExamSheet_Click();
						break;
					case "EHR":
						Tool_EHR_Click(false);
						break;
					case "HL7":
						Tool_HL7_Click();
						break;
					case "MedLab":
						Tool_MedLab_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,PatCur);
			}
		}

		///<summary></summary>
		private void OnPatientSelected(Patient pat) {
			PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(pat);
			if(PatientSelected!=null){
				PatientSelected(this,eArgs);
			}
		}

		private void Tool_Rx_Click(){
			if(!Security.IsAuthorized(Permissions.RxCreate)) {
				return;
			}
			if(UsingEcwTightOrFull() && Bridges.ECW.UserId!=0) {
				VBbridges.Ecw.LoadRxForm((int)Bridges.ECW.UserId,Bridges.ECW.EcwConfigPath,(int)Bridges.ECW.AptNum);
				//refresh the right panel:
				try {
					string strAppServer=VBbridges.Ecw.GetAppServer((int)Bridges.ECW.UserId,Bridges.ECW.EcwConfigPath);
					webBrowserEcw.Url=new Uri("http://"+strAppServer+"/mobiledoc/jsp/dashboard/Overview.jsp?ptId="
							+PatCur.PatNum.ToString()+"&panelName=overview&pnencid="
							+Bridges.ECW.AptNum.ToString()+"&context=progressnotes&TrUserId="+Bridges.ECW.UserId.ToString());
					labelECWerror.Visible=false;
				}
				catch(Exception ex) {
					webBrowserEcw.Url=null;
					labelECWerror.Text="Error: "+ex.Message;
					labelECWerror.Visible=true;
				}
			}
			else {
				FormRxSelect FormRS=new FormRxSelect(PatCur);
				FormRS.ShowDialog();
				if(FormRS.DialogResult!=DialogResult.OK) return;
				ModuleSelected(PatCur.PatNum);
				SecurityLogs.MakeLogEntry(Permissions.RxCreate,PatCur.PatNum,"Created prescription.");
			}
		}

		///<summary>Returns false if account ID is blank or not in format of 1 or more digits, followed by 3 random alpha-numberic characters, followed by a 2 digit checksum. Only returns true when the NewCrop Account ID is one that was created by OD.</summary>
 		private bool NewCropIsAccountIdValid() {
			bool validKey=false;
			string newCropAccountId=PrefC.GetString(PrefName.NewCropAccountId);
			if(Regex.IsMatch(newCropAccountId,"[0-9]+\\-[0-9A-Za-z]{3}[0-9]{2}")) { //Must contain at least 1 digit for patnum, 1 dash, 3 random alpha-numeric characters, then 2 digits for checksum.
				//Verify key checksum to make certain that this key was generated by OD and not a reseller.
				long patNum=PIn.Long(newCropAccountId.Substring(0,newCropAccountId.IndexOf('-')));
				long checkSum=patNum;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+1])*3;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+2])*5;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+3])*7;
				if((checkSum%100).ToString().PadLeft(2,'0')==newCropAccountId.Substring(newCropAccountId.Length-2)) {
					validKey=true;
				}
			}
			return validKey;
		}

		///<summary>Returns true if new information was pulled back from NewCrop.</summary>
		private bool NewCropRefreshPrescriptions() {
			Program programNewCrop=Programs.GetCur(ProgramName.eRx);
			if(ToolBarMain.Buttons["eRx"]!=null) {//Hidden for eCW
				ToolBarMain.Buttons["eRx"].IsRed=false; //Set the eRx button back to default color.
				ToolBarMain.Invalidate();
			}
			if(!programNewCrop.Enabled) {
				return false;
			}
			if(PatCur==null) {
				return false;
			}
			string newCropAccountId=PrefC.GetString(PrefName.NewCropAccountId);
			if(newCropAccountId=="") {//We check for NewCropAccountID validity below, but we also need to be sure to exit this check for resellers if blank.
				return false;
			}
			if(!NewCropIsAccountIdValid()) {
				//The NewCropAccountID will be invalid for resellers, because the checksum will be wrong.
				//Therefore, resellers should be allowed to continue if both the NewCropName and NewCropPassword are specified. NewCrop does not allow blank passwords.
				if(PrefC.GetString(PrefName.NewCropName)=="" || PrefC.GetString(PrefName.NewCropPassword)=="") {
					return false;
				}
			}
			NewCrop.Update1 wsNewCrop=new NewCrop.Update1();//New Crop web services interface.
			NewCrop.Credentials credentials=new NewCrop.Credentials();
			NewCrop.AccountRequest accountRequest=new NewCrop.AccountRequest();
			NewCrop.PatientRequest patientRequest=new NewCrop.PatientRequest();
			NewCrop.PrescriptionHistoryRequest prescriptionHistoryRequest=new NewCrop.PrescriptionHistoryRequest();
			NewCrop.PatientInformationRequester patientInfoRequester=new NewCrop.PatientInformationRequester();
			NewCrop.Result response=new NewCrop.Result();
#if DEBUG
			wsNewCrop.Url="https://preproduction.newcropaccounts.com/v7/WebServices/Update1.asmx";
#endif
			credentials.PartnerName=ErxXml.NewCropPartnerName;
			credentials.Name=ErxXml.NewCropAccountName;
			credentials.Password=ErxXml.NewCropAccountPasssword;
			accountRequest.AccountId=newCropAccountId;
			accountRequest.SiteId="1";//Accounts are always created with SiteId=1.
			patientRequest.PatientId=POut.Long(PatCur.PatNum);
			_patNumLastErx=PatCur.PatNum;
			prescriptionHistoryRequest.StartHistory=new DateTime(2012,11,2);//Only used for archived prescriptions. This is the date of first release for NewCrop integration.
			prescriptionHistoryRequest.EndHistory=DateTime.Now;//Only used for archived prescriptions.
			//Prescription Archive Status Values:
			//N = Not archived (i.e. Current Medication) 
			//Y = Archived (i.e. Previous Mediation)
			//% = Both Not Archived and Archived
			//Note: This field will contain values other than Y,N in future releases.
			prescriptionHistoryRequest.PrescriptionArchiveStatus="N";
			//Prescription Status Values:
			//C = Completed Prescription
			//P = Pending Medication
			//% = Both C and P.
			prescriptionHistoryRequest.PrescriptionStatus="C";
			//Prescription Sub Status Values:
			//% = All meds (Returns all meds regardless of the sub status)
			//A = NS (Returns only meds that have a 'NS' - Needs staff sub status)
			//U = DR (Returns only meds that have a 'DR' - Needs doctor review sub status)
			//P = Renewal Request that has been selected for processing on the NewCrop screens - it has not yet been denied, denied and re-written or accepted
			//S = Standard Rx (Returns only meds that have an 'InProc' - InProcess sub status)
			//D = DrugSet source - indicates the prescription was created by selecting the medication from the DrugSet selection box on the ComposeRx page
			//O = Outside Prescription - indicates the prescription was created on the MedEntry page, not prescribed.
			prescriptionHistoryRequest.PrescriptionSubStatus="S";
			patientInfoRequester.UserType="Staff";//Allowed values: Doctor,Staff
			if(Security.CurUser.ProvNum!=0) {//If the current OD user is associated to a doctor, then the request is from a doctor, otherwise from a staff member.
			  patientInfoRequester.UserType="Doctor";
			}
			patientInfoRequester.UserId=POut.Long(Security.CurUser.UserNum);
			//Send the request to NewCrop. Always returns all current medications, and returns medications between the StartHistory and EndHistory dates if requesting archived medications.
			//The patientIdType parameter was added for another vendor and is not often used. We do not use this field. We must pass empty string.
			//The includeSchema parameter is useful for first-time debugging, but in release mode, we should pass N for no.
			wsNewCrop.Timeout=3000;//3 second. The default is 100 seconds, but we cannot wait that long, because prescriptions are checked each time the Chart is refreshed. 1 second is too little, 2 seconds works most of the time. 3 seconds is safe.
			try {
				//throw new Exception("Test communication error in debug mode.");
				response=wsNewCrop.GetPatientFullMedicationHistory6(credentials,accountRequest,patientRequest,prescriptionHistoryRequest,patientInfoRequester,"","N");
			}
			catch { //An exception is thrown when the timeout is reached, or when the NewCrop servers are not accessible (because the servers are down, or because local internet is down).
				//We used to show a popup here each time the refresh failed, but users found it annoying when the NewCrop severs were down, because the popup would show each time they visited the Chart and impeded user workflow.
				//We tried silently logging a warning message into the Application log within system Event Viewer, but we found out that a decent number of users do not have permission to write to the Application log, which causes UEs sometimes.
				//We tried showing a popup exactly 1 time for each instance of OD launched, to avoid the permission issue, but users were still complaining about it popping up and they didn't know what to do to fix it.
				//We now change the background color of the eRx button red, and show an error message when user click the eRx button to alert them that interactions may be out of date.
				if(ToolBarMain.Buttons["eRx"]!=null) {//Hidden for eCW
					ToolBarMain.Buttons["eRx"].IsRed=true; //Marks the eRx button to be drawn with a red color.
					ToolBarMain.Invalidate();
				}
				return false;
			}
			//response.Message = Error message if error.
			//response.RowCount = Number of prescription records returned.
			//response.Status = Status of request. "OK" = success.
			//response.Timing = Not sure what this is for. Tells us how quickly the server responded to the request?
			//response.XmlResponse = The XML data returned, encoded in base 64.
			if(response.Status!=NewCrop.StatusType.OK) {//Other statuses include Fail (ex if credentials are invalid), NotFound (ex if patientId invalid or accoundId invalid), Unknown (no known examples yet)
				//For now we simply abort gracefully.
				return false;
			}
			byte[] xmlResponseBytes=Convert.FromBase64String(response.XmlResponse);
			string xmlResponse=Encoding.UTF8.GetString(xmlResponseBytes);
#if DEBUG//For capturing the xmlReponse with the newlines properly showing.
			string tempFile=PrefL.GetRandomTempFile(".txt");
			File.WriteAllText(tempFile,xmlResponse);
#endif
			XmlDocument xml=new XmlDocument();
			try {
				xml.LoadXml(xmlResponse);
			}
			catch { //In case NewCrop returns invalid XML.
				return false;//abort gracefully
			}
			DateTime rxStartDateT=PrefC.GetDateT(PrefName.ElectronicRxDateStartedUsing131);
			XmlNode nodeNewDataSet=xml.FirstChild;
			foreach(XmlNode nodeTable in nodeNewDataSet.ChildNodes) {
				RxPat rxOld=null;
				MedicationPat medOrderOld=null;
				RxPat rx=new RxPat();
				//rx.IsControlled not important.  Only used in sending, but this Rx was already sent.
				rx.Disp="";
				rx.DosageCode="";
				rx.Drug="";
				rx.Notes="";
				rx.Refills="";
				rx.SendStatus=RxSendStatus.SentElect;
				rx.Sig="";
				string additionalSig="";
				bool isProv=true;
				long rxCui=0;
				string strDrugName="";
				string strGenericName="";
				string strProvNumOrNpi="";//We used to send ProvNum in LicensedPrescriber.ID to NewCrop, but now we send NPI. We will receive ProvNum for older prescriptions.
				foreach(XmlNode nodeRxFieldParent in nodeTable.ChildNodes) {
					XmlNode nodeRxField=nodeRxFieldParent.FirstChild;
					if(nodeRxField==null) {
						continue;
					}
					switch(nodeRxFieldParent.Name.ToLower()) {
						case "dispense"://ex 5.555
							rx.Disp=nodeRxField.Value;
							break;
						case "druginfo"://ex lisinopril 5 mg Tab
							rx.Drug=nodeRxField.Value;
							break;
						case "drugname"://ex lisinopril
							strDrugName=nodeRxField.Value;
							break;
						case "externalpatientid"://patnum passed back from the compose request that initiated this prescription
							rx.PatNum=PIn.Long(nodeRxField.Value);
							break;
						case "externalphysicianid"://NPI passed back from the compose request that initiated this prescription.  For older prescriptions, this will be ProvNum.
							strProvNumOrNpi=nodeRxField.Value;
							break;
						case "externaluserid"://The person who ordered the prescription. Is a ProvNum when provider, or an EmployeeNum when an employee. If EmployeeNum, then is prepended with "emp" because of how we sent it to NewCrop in the first place.
							if(nodeRxField.Value.StartsWith("emp")) {
								isProv=false;
							}
							break;
						case "genericname":
							strGenericName=nodeRxField.Value;
							break;
						case "patientfriendlysig"://The concat of all the codified fields.
							rx.Sig=nodeRxField.Value;
							break;
						case "pharmacyncpdp"://ex 9998888
							//We will use this information in the future to find a pharmacy already entered into OD, or to create one dynamically if it does not exist.
							//rx.PharmacyNum;//Get the pharmacy where pharmacy.PharmID = node.Value
							break;
						case "prescriptiondate":
							rx.RxDate=PIn.DateT(nodeRxField.Value);
							break;
						case "prescriptionguid"://32 characters with 4 hyphens. ex ba4d4a84-af0a-4cbf-9437-36feda97d1b6
							rx.NewCropGuid=nodeRxField.Value;
							rxOld=RxPats.GetRxNewCrop(nodeRxField.Value);
							medOrderOld=MedicationPats.GetMedicationOrderByNewCropGuid(nodeRxField.Value);
							break;
						case "prescriptionnotes"://from the Additional Sig box at the bottom
							additionalSig=nodeRxField.Value;
							break;
						case "refills"://ex 1
							rx.Refills=nodeRxField.Value;
							break;
						case "rxcui"://ex 311354
							rxCui=PIn.Long(nodeRxField.Value);//The RxCui is not returned with all prescriptions, so it can be zero (not set).
							break;						
					}
				}//end inner foreach
				if(rx.RxDate<rxStartDateT) {//Ignore prescriptions created before version 13.1.14, because those prescriptions were entered manually by the user.
					continue;
				}
				if(additionalSig!="") {
					if(rx.Sig!="") {//If patient friend SIG is present.
						rx.Sig+=" ";
					}
					rx.Sig+=additionalSig;
				}
				//Determine the provider. This is a mess, because we used to send ProvNum in the outgoing XML LicensedPrescriber.ID,
				//but now we send NPI to avoid multiple billing charges for two provider records with the same NPI
				//(the same doctor entered multiple times, for example, one provider for each clinic).
				ErxLog erxLog=ErxLogs.GetLatestForPat(rx.PatNum,rx.RxDate);//Locate the original request corresponding to this prescription.
				if((erxLog==null || erxLog.ProvNum==0)) {//Not found or the provnum is unknown.
					//The erxLog.ProvNum will be 0 for prescriptions fetched from NewCrop before version 13.3. Could also happen if
					//prescriptions were created when NewCrop was brand new (right before ErxLog was created),
					//or if someone lost a database and they are downloading all the prescriptions from scratch again.
					if(rxOld==null) {//The prescription is being dowloaded for the first time, or is being downloaded again after it was deleted manually by the user.
						for(int j=0;j<ProviderC.ListShort.Count;j++) {//Try to locate a visible provider matching the NPI on the prescription.
							if(strProvNumOrNpi.Length==10 && ProviderC.ListShort[j].NationalProvID==strProvNumOrNpi) {
								rx.ProvNum=ProviderC.ListShort[j].ProvNum;
								break;
							}
						}
						if(rx.ProvNum==0) {//No visible provider found matching the NPI on the prescription.
							for(int j=0;j<ProviderC.ListLong.Count;j++) {//Try finding a hidden provider matching the NPI on the prescription, or a matching provnum.
								if(strProvNumOrNpi.Length==10 && ProviderC.ListLong[j].NationalProvID==strProvNumOrNpi) {
									rx.ProvNum=ProviderC.ListLong[j].ProvNum;
									break;
								}
								if(ProviderC.ListLong[j].ProvNum.ToString()==strProvNumOrNpi) {
									rx.ProvNum=ProviderC.ListLong[j].ProvNum;
									break;
								}
							}
						}
						//If rx.ProvNum is still zero, then that means the provider NPI/ProvNum has been modified or somehow deleted (for example, database was lost) for the provider record originally used.
						if(rx.ProvNum==0) {//Catch all
							if(PatCur.PriProv!=0) {
								rx.ProvNum=PatCur.PriProv;
							}
							else {
								rx.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
							}
						}
					}
					else {//The prescription has already been downloaded in the past.
						rx.ProvNum=rxOld.ProvNum;//Preserve the provnum if already in the database, because it may have already been corrected by the user after the previous download.
					}
				}
				else {
					rx.ProvNum=erxLog.ProvNum;
				}
				if(rxOld==null) {
					rx.IsNew=true;//Might not be necessary, but does not hurt.
					SecurityLogs.MakeLogEntry(Permissions.RxCreate,rx.PatNum,"eRx automatically created: "+rx.Drug);
					RxPats.Insert(rx);
				}
				else {//The prescription was already in our database. Update it.
					rx.RxNum=rxOld.RxNum;
					RxPats.Update(rx);
				}
				//If rxCui==0, then NewCrop did not provide an RxCui.  Attempt to locate an RxCui using the other provided drug information.  An RxCui is not required for our program.  Meds missing an RxCui are not exported in CCD messages.
				if(rxCui==0 && strDrugName!="") {
					List<RxNorm> listRxNorms=RxNorms.GetListByCodeOrDesc(strDrugName,true,true);//Exact case insensitive match ignoring numbers.
					if(listRxNorms.Count>0) {
						rxCui=PIn.Long(listRxNorms[0].RxCui);
					}					
				}
				//If rxCui==0, then NewCrop did not provide an RxCui and we could not locate an RxCui by DrugName.  Try searching by GenericName.
				if(rxCui==0 && strGenericName!="") {
					List<RxNorm> listRxNorms=RxNorms.GetListByCodeOrDesc(strGenericName,true,true);//Exact case insensitive match ignoring numbers.
					if(listRxNorms.Count>0) {
						rxCui=PIn.Long(listRxNorms[0].RxCui);
					}
				}
				//If rxCui==0, then NewCrop did not provide an RxCui and we could not locate an RxCui by DrugName or GenericName.
				if(rxCui==0) {
					//We may need to enhance in future to support more advanced RxNorm searches.
					//For example: DrugName=Cafatine, DrugInfo=Cafatine 1 mg-100 mg Tab, GenericName=ergotamine-caffeine.
					//This drug could not be found by DrugName nor GenericName, but could be found when the GenericName was split by non-alpha characters, then the words in the generic name were swapped.
					//Namely, "caffeine ergotamine" is in the RxNorm table.
				}
				MedicationPats.InsertOrUpdateMedOrderForRx(rx,rxCui,isProv);//MedicationNum of 0, because we do not want to bloat the medication list in OD. In this special situation, we instead set the MedDescript, RxCui and NewCropGuid columns.
			}//end foreach
			return true;
		}

		private void Tool_eRx_Click() {
			if(!Security.IsAuthorized(Permissions.RxCreate)) {
				return;
			}
			Program programNewCrop=Programs.GetCur(ProgramName.eRx);
			string newCropAccountId=PrefC.GetString(PrefName.NewCropAccountId);
			if(newCropAccountId==""){//NewCrop has not been enabled yet.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Continuing will enable basic Electronic Rx (eRx).  Fees are associated with this secure e-prescribing system.  See our online manual for details.  To enable comprehensive eRx (with drug interaction checks, formulary checks, etc.), contact support.  At this time, eRx only works for the United States and its territories, including Puerto Rico.  Continue?")) {
					return;
				}
				//prepare the xml document to send--------------------------------------------------------------------------------------
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = ("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("CustomerIdRequest");
					writer.WriteStartElement("RegistrationKey");
					writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
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
				string patNum="";
				try {
					string result=updateService.RequestCustomerID(strbuild.ToString());//may throw error
					XmlDocument doc=new XmlDocument();
					doc.LoadXml(result);
					XmlNode node=doc.SelectSingleNode("//CustomerIdResponse");
					if(node!=null) {
						patNum=node.InnerText;
					}
					if(patNum=="") {
						throw new ApplicationException("Failed to validate registration key.");
					}
					newCropAccountId=patNum;
					newCropAccountId+="-"+CodeBase.MiscUtils.CreateRandomAlphaNumericString(3);
					long checkSum=PIn.Long(patNum);
					checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+1])*3;
					checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+2])*5;
					checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+3])*7;
					newCropAccountId+=(checkSum%100).ToString().PadLeft(2,'0');
					Prefs.UpdateString(PrefName.NewCropAccountId,newCropAccountId);
					programNewCrop.Enabled=true;
					Programs.Update(programNewCrop);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			else { //newCropAccountId!=""
				if(!programNewCrop.Enabled) {
					MessageBox.Show(Lan.g(this,"eRx is currently disabled.")+"\r\n"+Lan.g(this,"To enable, see our online manual for instructions."));
					return;
				}
				if(!NewCropIsAccountIdValid()) {
					string newCropName=PrefC.GetString(PrefName.NewCropName);
					string newCropPassword=PrefC.GetString(PrefName.NewCropPassword);
					if(newCropName=="" || newCropPassword=="") { //NewCrop does not allow blank passwords.
						MsgBox.Show(this,"NewCropName preference and NewCropPassword preference must not be blank when using a NewCrop AccountID provided by a reseller.");
						return;
					}
				}
			}
			//Validation------------------------------------------------------------------------------------------------------------------------------------------------------
			if(Security.CurUser.EmployeeNum==0 && Security.CurUser.ProvNum==0) {
				MsgBox.Show(this,"This user must be associated with either a provider or an employee.  The security admin must make this change before this user can submit prescriptions.");
				return;
			}
			if(PatCur==null) {
				MsgBox.Show(this,"No patient selected.");
				return;
			}
			string practicePhone=PrefC.GetString(PrefName.PracticePhone);
			if(!Regex.IsMatch(practicePhone,"^[0-9]{10}$")) {//"^[0-9]{10}(x[0-9]+)?$")) {
				MsgBox.Show(this,"Practice phone must be exactly 10 digits.");
				return;
			}
			if(practicePhone.StartsWith("555")) {
				MsgBox.Show(this,"Practice phone cannot start with 555.");
				return;
			}
			if(Regex.IsMatch(practicePhone,"^[0-9]{3}555[0-9]{4}$")) {
				MsgBox.Show(this,"Practice phone cannot contain 555 in the middle 3 digits.");
				return;
			}
			string practiceFax=PrefC.GetString(PrefName.PracticeFax);
			if(!Regex.IsMatch(practiceFax,"^[0-9]{10}(x[0-9]+)?$")) {
				MsgBox.Show(this,"Practice fax must be exactly 10 digits.");
				return;
			}
			if(practiceFax.StartsWith("555")) {
				MsgBox.Show(this,"Practice fax cannot start with 555.");
				return;
			}
			if(Regex.IsMatch(practiceFax,"^[0-9]{3}555[0-9]{4}$")) {
				MsgBox.Show(this,"Practice fax cannot contain 555 in the middle 3 digits.");
				return;
			}
			if(PrefC.GetString(PrefName.PracticeAddress)=="") {
				MsgBox.Show(this,"Practice address blank.");
				return;
			}
			if(Regex.IsMatch(PrefC.GetString(PrefName.PracticeAddress),".*P\\.?O\\.? .*",RegexOptions.IgnoreCase)) {
				MsgBox.Show(this,"Practice address cannot be a PO BOX.");
				return;
			}
			if(PrefC.GetString(PrefName.PracticeCity)=="") {
				MsgBox.Show(this,"Practice city blank.");
				return;
			}
			List<string> stateCodes=new List<string>(new string[] {
				//50 States.
				"AK","AL","AR","AZ","CA","CO","CT","DE","FL","GA",
				"HI","IA","ID","IL","IN","KS","KY","LA","MA","MD",
				"ME","MI","MN","MO","MS","MT","NC","ND","NE","NH",
				"NJ","NM","NV","NY","OH","OK","OR","PA","RI","SC",
				"SD","TN","TX","UT","VA","VT","WA","WI","WV","WY",
				//US Districts
				"DC",
				//US territories. Reference http://www.itl.nist.gov/fipspubs/fip5-2.htm
				"AS","FM","GU","MH","MP","PW","PR","UM","VI",
			});
			if(stateCodes.IndexOf(PrefC.GetString(PrefName.PracticeST))<0) {
				MsgBox.Show(this,"Practice state abbreviation invalid.");
				return;
			}
			string practiceZip=Regex.Replace(PrefC.GetString(PrefName.PracticeZip),"[^0-9]*","");//Zip with all non-numeric characters removed.
			if(practiceZip.Length!=9) {
				MsgBox.Show(this,"Practice zip must be 9 digits.");
				return;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && PatCur.ClinicNum!=0) { //Using clinics and the patient is assigned to a clinic.
				Clinic clinic=Clinics.GetClinic(PatCur.ClinicNum);
				if(!Regex.IsMatch(clinic.Phone,"^[0-9]{10}?$")) {
					MessageBox.Show(Lan.g(this,"Clinic phone must be exactly 10 digits")+": "+clinic.Description);
					return;
				}
				if(clinic.Phone.StartsWith("555")) {
					MessageBox.Show(Lan.g(this,"Clinic phone cannot start with 555")+": "+clinic.Description);
					return;
				}
				if(Regex.IsMatch(clinic.Phone,"^[0-9]{3}555[0-9]{4}$")) {
					MessageBox.Show(Lan.g(this,"Clinic phone cannot contain 555 in the middle 3 digits")+": "+clinic.Description);
					return;
				}
				if(!Regex.IsMatch(clinic.Fax,"^[0-9]{10}?$")) {
					MessageBox.Show(Lan.g(this,"Clinic fax must be exactly 10 digits")+": "+clinic.Description);
					return;
				}
				if(clinic.Fax.StartsWith("555")) {
					MessageBox.Show(Lan.g(this,"Clinic fax cannot start with 555")+": "+clinic.Description);
					return;
				}
				if(Regex.IsMatch(clinic.Fax,"^[0-9]{3}555[0-9]{4}$")) {
					MessageBox.Show(Lan.g(this,"Clinic fax cannot contain 555 in the middle 3 digits")+": "+clinic.Description);
					return;
				}
				if(clinic.Address=="") {
					MessageBox.Show(Lan.g(this,"Clinic address blank")+": "+clinic.Description);
					return;
				}
				if(Regex.IsMatch(clinic.Address,".*P\\.?O\\.? .*",RegexOptions.IgnoreCase)) {
					MessageBox.Show(Lan.g(this,"Clinic address cannot be a PO BOX")+": "+clinic.Description);
					return;
				}
				if(clinic.City=="") {
					MessageBox.Show(Lan.g(this,"Clinic city blank")+": "+clinic.Description);
					return;
				}
				if(stateCodes.IndexOf(clinic.State)<0) {
					MessageBox.Show(Lan.g(this,"Clinic state abbreviation invalid")+": "+clinic.Description);
					return;
				}
				string clinicZip=Regex.Replace(clinic.Zip,"[^0-9]*","");//Zip with all non-numeric characters removed.
				if(clinicZip.Length!=9) {
					MessageBox.Show(Lan.g(this,"Clinic zip must be 9 digits")+": "+clinic.Description);
					return;
				}
			}
			#region Provider Validation
			Provider prov=null;
			if(Security.CurUser.ProvNum!=0) {
				prov=Providers.GetProv(Security.CurUser.ProvNum);
			}
			else {
				prov=Providers.GetProv(PatCur.PriProv);
			}
			if(prov.IsNotPerson) {
				MessageBox.Show(Lan.g(this,"Provider must be a person")+": "+prov.Abbr);
				return;
			}
			string fname=prov.FName.Trim();
			if(fname=="") {
				MessageBox.Show(Lan.g(this,"Provider first name missing")+": "+prov.Abbr);
				return;
			}
			if(Regex.Replace(fname,"[^A-Za-z\\-]*","")!=fname) {
				MessageBox.Show(Lan.g(this,"Provider first name can only contain letters and dashes.")+": "+prov.Abbr);
				return;
			}
			string lname=prov.LName.Trim();
			if(lname=="") {
				MessageBox.Show(Lan.g(this,"Provider last name missing")+": "+prov.Abbr);
				return;
			}
			if(Regex.Replace(lname,"[^A-Za-z\\-]*","")!=lname) { //Will catch situations such as "Dale Jr. III" and "Ross DMD".
				MessageBox.Show(Lan.g(this,"Provider last name can only contain letters and dashes.  Use the suffix box for I, II, III, Jr, or Sr")+": "+prov.Abbr);
				return;
			}
			//prov.Suffix is not validated here. In ErxXml.cs, the suffix is converted to the appropriate suffix enumeration value, or defaults to DDS if the suffix does not make sense.
			if(prov.DEANum.ToLower()!="none" && !Regex.IsMatch(prov.DEANum,"^[A-Za-z]{2}[0-9]{7}$")) {
				MessageBox.Show(Lan.g(this,"Provider DEA Number must be 2 letters followed by 7 digits.  If no DEA Number, enter NONE.")+": "+prov.Abbr);
				return;
			}
			string npi=Regex.Replace(prov.NationalProvID,"[^0-9]*","");//NPI with all non-numeric characters removed.
			if(npi.Length!=10) {
				MessageBox.Show(Lan.g(this,"Provider NPI must be exactly 10 digits")+": "+prov.Abbr);
				return;
			}
			if(prov.StateLicense=="") {
				MessageBox.Show(Lan.g(this,"Provider state license missing")+": "+prov.Abbr);
				return;
			}
			if(stateCodes.IndexOf(prov.StateWhereLicensed)<0) {
				MessageBox.Show(Lan.g(this,"Provider state where licensed invalid")+": "+prov.Abbr);
				return;
			}
			UpdateErxAccess(npi);
			ProviderErx provErx=ProviderErxs.GetOneForNpi(npi);
			if(!PrefC.GetBool(PrefName.NewCropIsLegacy) && !provErx.IsIdentifyProofed) {
				if(PrefC.GetString(PrefName.NewCropPartnerName)!="" || PrefC.GetString(PrefName.NewCropPassword)!="") {//Customer of a distributor
					MessageBox.Show(Lan.g(this,"Provider")+" "+prov.Abbr+" "
						+Lan.g(this,"must complete Identify Proofing (IDP) before using eRx.  Call support for details."));
				}
				else {//Customer of OD proper or customer of a reseller
					if(MessageBox.Show(Lan.g(this,"Provider")+" "+prov.Abbr+" "
						+Lan.g(this,"must complete Identify Proofing (IDP) before using eRx.  There is a one time setup fee for Identity Proofing.  "
						+"Step-by-Step details on IDP process can be found in the Online Software Manual (search for 'Identity Proofing') or call support.  "
						+"Click OK below to begin Identity Proofing process now, or click Cancel to go back to the Chart."),
						"",MessageBoxButtons.OKCancel)==DialogResult.OK)
					{
						Process.Start(
							"https://universalid.verizon.com/uid/index.php/issuanceservice/registration/individual/41511098-65c2-4cfb-8ee5-6e021671a669/ZFR");
					}
				}
				return;
			}
			if(!provErx.IsEnabled) {
				MessageBox.Show(Lan.g(this,"Contact support to enable eRx for provider")+" "+prov.Abbr);
				return;
			}
			#endregion Provider Validation
			Employee emp=null;
			if(Security.CurUser.EmployeeNum!=0) {
				emp=Employees.GetEmp(Security.CurUser.EmployeeNum);
				if(emp.LName=="") {//Checked in UI, but check here just in case this database was converted from another software.
					MessageBox.Show(Lan.g(this,"Employee last name missing for user")+": "+Security.CurUser.UserName);
					return;
				}
				if(emp.FName=="") {//Not validated in UI.
					MessageBox.Show(Lan.g(this,"Employee first name missing for user")+": "+Security.CurUser.UserName);
					return;
				}
			}
			if(PatCur.Birthdate.Year<1880) {
				MsgBox.Show(this,"Patient birthdate missing.");
				return;
			}
			if(PatCur.State!="" && stateCodes.IndexOf(PatCur.State)<0) {
				MsgBox.Show(this,"Patient state abbreviation invalid");
				return;
			}
			if(PatCur.Zip!="" && !Regex.IsMatch(PatCur.Zip,@"^[0-9]{5}\-?([0-9]{4})?$")) {//Blank, or #####, or #####-####, or #########
				MsgBox.Show(this,"Patient zip invalid");
				return;
			}
			InternetExplorer IEControl;
			try {
				IEControl=new InternetExplorer();
			}
			catch {
				MsgBox.Show(this,"Internet Explorer not installed.");
				return;
			}
			string clickThroughXml=ErxXml.BuildClickThroughXml(prov,emp,PatCur);
//			string xmlBase64=System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(clickThroughXml)));
//			xmlBase64=xmlBase64.Replace("+","%2B");//A common base 64 character which needs to be escaped within URLs.
//			xmlBase64=xmlBase64.Replace("/","%2F");//A common base 64 character which needs to be escaped within URLs.
//			xmlBase64=xmlBase64.Replace("=","%3D");//Base 64 strings usually end in '=', which could mean a new parameter definition within the URL so we escape.
//			String postdata="RxInput=base64:"+xmlBase64;
//			byte[] PostDataBytes=System.Text.Encoding.UTF8.GetBytes(postdata);
//			string additionalHeaders="Content-Type: application/x-www-form-urlencoded\r\n";
//			IWebBrowserApp IE=(IWebBrowserApp)IEControl;
//			IE.Visible=true;
//#if DEBUG
//			string newCropUrl="http://preproduction.newcropaccounts.com/interfaceV7/rxentry.aspx";
//#else //Debug
//			string newCropUrl="https://secure.newcropaccounts.com/interfacev7/rxentry.aspx";
//#endif
//			IE.Navigate(newCropUrl,null,null,PostDataBytes,additionalHeaders);
			FormErx formErx=new FormErx();
			formErx.PatCur=PatCur;
			formErx.ClickThroughXml=clickThroughXml;
			formErx.Show();//Non-modal so user can browse OD while writing prescription.  When form is closed, ErxBrowserClosed() is called below.			
			ErxLog erxLog=new ErxLog();
			erxLog.PatNum=PatCur.PatNum;
			erxLog.MsgText=clickThroughXml;
			erxLog.ProvNum=prov.ProvNum;
			ErxLogs.Insert(erxLog);
		}

		///<summary>If we are still making too many web calls in the future (by calling NewCropRefreshPrescriptions()), when a patient is selected before
		///eRx clicked and refreshing prescriptions, we could avoid if they don't have an appt today, but because we don't know clinically what to do when
		///patients are there, we are not going to do this now.</summary>
		private void ErxBrowserClosed(ODEventArgs e) {
			if(e.Name!="ErxBrowserClosed") {
				return;
			}
			Patient pat=(Patient)e.Tag;
			if(pat==null) {
				return;//A subwindow of FormErx was opened due to a link being clicked into a new window instance.
			}
			if(PatCur==null || PatCur.PatNum!=pat.PatNum) {
				return;//FormErx was closed for another patient.
			}
			//Refresh prescriptions from NewCrop, since the user probably just added at least one.
			this.Cursor=Cursors.WaitCursor;
			Application.DoEvents();
			if(NewCropRefreshPrescriptions()) {
				ModuleSelected(PatCur.PatNum);
			}
			this.Cursor=Cursors.Default;
		}

		///<summary>Manuall refresh prescriptions from eRx.</summary>
		private void menuItemErxRefresh_Click(object sender,EventArgs e) {
			this.Cursor=Cursors.WaitCursor;
			Application.DoEvents();
			if(NewCropRefreshPrescriptions()) {
				ModuleSelected(PatCur.PatNum);
			}
			this.Cursor=Cursors.Default;
		}

		///<summary>Returns an error message upon error.  Otherwise returns empty string.</summary>
		private void UpdateErxAccess(string npi) {
			ProviderErx provErxCur=ProviderErxs.GetOneForNpi(npi);
			if(provErxCur==null) {
				//The provider is not yet part of the providererx table.  This extra refresh will only happen one time for each new provider.
				//First refresh cache to verify the provider was not added within the last signal interval.  Prevents duplicates for long signal intervals.
				ProviderErxs.RefreshCache();
				provErxCur=ProviderErxs.GetOneForNpi(npi);
			}
			if(provErxCur==null) {
				provErxCur=new ProviderErx();
				provErxCur.PatNum=0;
				provErxCur.NationalProviderID=npi;
				provErxCur.IsEnabled=PrefC.GetBool(PrefName.NewCropIsLegacy);
				provErxCur.IsIdentifyProofed=false;
				provErxCur.IsSentToHq=false;
				ProviderErxs.Insert(provErxCur);
				DataValid.SetInvalid(InvalidType.ProviderErxs);
			}
			bool isDistributorCustomer=false;
			if(PrefC.GetString(PrefName.NewCropPartnerName)!="" || PrefC.GetString(PrefName.NewCropPassword)!="") {
				isDistributorCustomer=true;
			}
			bool isOdUpdateAddress=false;
			if(PrefC.GetString(PrefName.UpdateServerAddress).ToLower().Contains("opendentalsoft.com") || 
				PrefC.GetString(PrefName.UpdateServerAddress).ToLower().Contains("open-dent.com")) {
				isOdUpdateAddress=true;
			}
			if(isDistributorCustomer && isOdUpdateAddress) {
				//The distributor forgot to change the "Server Address for Updates" inside of the Update Setup window for this customer.
				//Do not contact the OD web service.
			}
			else if(!provErxCur.IsEnabled || !provErxCur.IsSentToHq || PrefC.GetDate(PrefName.NewCropDateLastAccessCheck).Month < DateTimeOD.Today.Month) {
				//An OD customer, or a Distributor customer if the distributor has a custom web service for updates.
				//For distributors who implement this feature, you will be able to use FormErxAccess at your office to control individual provider access.
				//We compare the last access date by month above, because eRx charges are based on monthly usage.  Avoid extra charges for disabled providers.
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars=("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("ErxAccessRequest");
						writer.WriteStartElement("RegistrationKey");
						writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
						writer.WriteEndElement();//End reg key
					List <ProviderErx> listUnsentProviders=ProviderErxs.GetAllUnsent();
					for(int i=0;i<listUnsentProviders.Count;i++) {
						writer.WriteStartElement("Prov");
							writer.WriteAttributeString("NPI",listUnsentProviders[i].NationalProviderID);
							writer.WriteAttributeString("IsEna",listUnsentProviders[i].IsEnabled?"1":"0");
						writer.WriteEndElement();//End Prov
					}
					writer.WriteEndElement();//End ErxAccessRequest
				}
#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
					updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
				if(PrefC.GetString(PrefName.UpdateWebProxyAddress) != "") {
					IWebProxy proxy=new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
					ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
					proxy.Credentials=cred;
					updateService.Proxy=proxy;
				}
				try {
					string result=updateService.GetErxAccess(strbuild.ToString());//may throw error
					XmlDocument doc=new XmlDocument();
					doc.LoadXml(result);
					XmlNodeList listNodes=doc.SelectNodes("//Prov");
					bool isCacheRefresNeeded=false;
					for(int i=0;i<listNodes.Count;i++) {//Loop through providers.
						XmlNode nodeProv=listNodes[i];
						string provNpi="";
						bool isProvEnabled=false;
						bool isProvIdp=false;
						for(int j=0;j<nodeProv.Attributes.Count;j++) {//Loop through the attributes for the current provider.
							XmlAttribute attribute=nodeProv.Attributes[j];
							if(attribute.Name=="NPI") {
								provNpi=Regex.Replace(attribute.Value,"[^0-9]*","");//NPI with all non-numeric characters removed.
								if(provNpi.Length!=10) {
									provNpi="";
									break;//Invalid NPI
								}
							}
							else if(attribute.Name=="IsEna" && attribute.Value=="1") {
								isProvEnabled=true;
							}
							else if(attribute.Name=="IsIdp" && attribute.Value=="1") {
								isProvIdp=true;
							}
						}
						if(provNpi=="") {
							continue;
						}
						ProviderErx oldProvErx=ProviderErxs.GetOneForNpi(provNpi);
						if(oldProvErx==null) {
							continue;
						}
						ProviderErx provErx=oldProvErx.Clone();
						provErx.IsEnabled=isProvEnabled;
						provErx.IsIdentifyProofed=isProvIdp;
						provErx.IsSentToHq=true;
						if(ProviderErxs.Update(provErx,oldProvErx)) {
							isCacheRefresNeeded=true;
						}						
					}
					if(isCacheRefresNeeded) {
						DataValid.SetInvalid(InvalidType.ProviderErxs);
					}
					if(Prefs.UpdateDateT(PrefName.NewCropDateLastAccessCheck,DateTimeOD.Today)) {
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				catch(Exception ex) {
					//Failed to contact server and/or update provider IsEnabled statuses.  We will simply use what we already know in the local database.
				}
			}
		}

		private void Tool_LabCase_Click() {
			LabCase lab=new LabCase();
			lab.PatNum=PatCur.PatNum;
			lab.ProvNum=Patients.GetProvNum(PatCur);
			lab.DateTimeCreated=MiscData.GetNowDateTime();
			LabCases.Insert(lab);//it will be deleted inside the form if user clicks cancel.
			//We need the primary key in order to attach lab slip.
			FormLabCaseEdit FormL=new FormLabCaseEdit();
			FormL.CaseCur=lab;
			FormL.IsNew=true;
			FormL.ShowDialog();
			//needs to always refresh due to complex ok/cancel
			ModuleSelected(PatCur.PatNum);
		}

		private void Tool_Perio_Click(){
			FormPerio FormP=new FormPerio(PatCur);
			FormP.ShowDialog();
		}

		private void Tool_Ortho_Click() {
			FormOrthoChart FormOC=new FormOrthoChart(PatCur);
			FormOC.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void Tool_Anesthesia_Click() {
			/*
			AnestheticData AnestheticDataCur;
			AnestheticDataCur = new AnestheticData();
			FormAnestheticRecord FormAR = new FormAnestheticRecord(PatCur, AnestheticDataCur);
			FormAR.ShowDialog();

			PatCur = Patients.GetPat(Convert.ToInt32(PatCur.PatNum));
			OnPatientSelected(Convert.ToInt32(PatCur.PatNum), Convert.ToString(PatCur), true, Convert.ToString(PatCur));
			FillPtInfo();
			return;*/
		}

		private void Tool_Consent_Click() {
			List<SheetDef> listSheets=SheetDefs.GetCustomForType(SheetTypeEnum.Consent);
			if(listSheets.Count>0){
				MsgBox.Show(this,"Please use dropdown list.");
				return;
			}
			SheetDef sheetDef=SheetsInternal.GetSheetDef(SheetInternalType.Consent);
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,PatCur.PatNum);
			SheetParameter.SetParameter(sheet,"PatNum",PatCur.PatNum);
			SheetFiller.FillFields(sheet);
			Graphics g=this.CreateGraphics();
			SheetUtil.CalculateHeights(sheet,g);
			g.Dispose();
			FormSheetFillEdit FormS=new FormSheetFillEdit(sheet);
			FormS.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void Tool_ToothChart_Click() {
			if(Programs.UsingOrion) {
				menuItemChartSave_Click(this,new EventArgs());
				return;
			}
			MsgBox.Show(this,"Please use dropdown list.");
			return;
		}

		private void Tool_ExamSheet_Click(){
			FormExamSheets fes=new FormExamSheets();
			fes.PatNum=PatCur.PatNum;
			fes.ShowDialog();
			RefreshModuleScreen();
		}

		///<summary>Only used for eCW tight.  Everyone else has the commlog button up in the main toolbar.</summary>
		private void Tool_Commlog_Click() {
			Commlog CommlogCur = new Commlog();
			CommlogCur.PatNum = PatCur.PatNum;
			CommlogCur.CommDateTime = DateTime.Now;
			CommlogCur.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			CommlogCur.Mode_=CommItemMode.Phone;
			CommlogCur.SentOrReceived=CommSentOrReceived.Received;
			CommlogCur.UserNum=Security.CurUser.UserNum;
			FormCommItem FormCI = new FormCommItem(CommlogCur);
			FormCI.IsNew = true;
			FormCI.ShowDialog();
			if(FormCI.DialogResult == DialogResult.OK) {
				ModuleSelected(PatCur.PatNum);
			}
		}

		private void Tool_EHR_Click(bool onLoadShowOrders) {
			//Quarterly key check was removed from here so that any customer can use EHR tools
			//But we require a EHR subscription for them to obtain their MU reports.
			FormEHR FormE = new FormEHR();
			FormE.PatNum=PatCur.PatNum;
			FormE.PatNotCur=PatientNoteCur;
			FormE.PatFamCur=FamCur;
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormE.ResultOnClosing==EhrFormResult.PatientSelect) {
				OnPatientSelected(Patients.GetPat(FormE.PatNum));
				ModuleSelected(FormE.PatNum);
			}
		}

		//private void Tool_EHR_Click_old(bool onLoadShowOrders) {
		//	#if EHRTEST
		//		//so we can step through for debugging.
		//	/*
		//		EhrQuarterlyKey keyThisQ=EhrQuarterlyKeys.GetKeyThisQuarter();
		//		if(keyThisQ==null) {
		//			MessageBox.Show("No quarterly key entered for this quarter.");
		//			return;
		//		}
		//		if(!((FormEHR)FormOpenDental.FormEHR).QuarterlyKeyIsValid((DateTime.Today.Year-2000).ToString(),EhrQuarterlyKeys.MonthToQuarter(DateTime.Today.Month).ToString(),
		//			PrefC.GetString(PrefName.PracticeTitle),keyThisQ.KeyValue)) {
		//			MessageBox.Show("Invalid quarterly key.");
		//			return;
		//		}
		//	*/
		//		((FormEHR)FormOpenDental.FormEHR).PatNum=PatCur.PatNum;
		//		((FormEHR)FormOpenDental.FormEHR).OnShowLaunchOrders=onLoadShowOrders;
		//		((FormEHR)FormOpenDental.FormEHR).ShowDialog();
		//		if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.None) {
		//			//return;
		//		}
		//		if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.RxEdit) {
		//			FormRxEdit FormRXE=new FormRxEdit(PatCur,RxPats.GetRx(((FormEHR)FormOpenDental.FormEHR).LaunchRxNum));
		//			FormRXE.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);//recursive.  The only way out of the loop is EhrFormResult.None.
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.RxSelect) {
		//			FormRxSelect FormRS=new FormRxSelect(PatCur);
		//			FormRS.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Medical) {
		//			FormMedical formM=new FormMedical(PatientNoteCur,PatCur);
		//			formM.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.PatientEdit) {
		//			FormPatientEdit formP=new FormPatientEdit(PatCur,FamCur);
		//			formP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Online) {
		//			FormEhrOnlineAccess formO=new FormEhrOnlineAccess();
		//			formO.PatCur=PatCur;
		//			formO.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedReconcile) {
		//			FormMedicationReconcile FormMR=new FormMedicationReconcile();
		//			FormMR.PatCur=PatCur;
		//			FormMR.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Referrals) {
		//			FormReferralsPatient formRP=new FormReferralsPatient();
		//			formRP.PatNum=PatCur.PatNum;
		//			formRP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedicationPatEdit) {
		//			FormMedPat formMP=new FormMedPat();
		//			formMP.MedicationPatCur=MedicationPats.GetOne(((FormEHR)FormOpenDental.FormEHR).LaunchMedicationPatNum);
		//			formMP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(true);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedicationPatNew) {
		//			//This cannot happen unless a provider is logged in with a valid ehr key
		//			FormMedications FormM=new FormMedications();
		//			FormM.IsSelectionMode=true;
		//			FormM.ShowDialog();
		//			if(FormM.DialogResult==DialogResult.OK) {
		//				Medication med=Medications.GetMedicationFromDb(FormM.SelectedMedicationNum);
		//				if(med.RxCui==0 //if the med has no Cui, it won't trigger an alert
		//					|| RxAlertL.DisplayAlerts(PatCur.PatNum,med.RxCui,0))//user sees alert and wants to continue
		//				{
		//					MedicationPat medicationPat=new MedicationPat();
		//					medicationPat.PatNum=PatCur.PatNum;
		//					medicationPat.MedicationNum=FormM.SelectedMedicationNum;
		//					medicationPat.ProvNum=Security.CurUser.ProvNum;
		//					medicationPat.DateStart=DateTime.Today;
		//					FormMedPat FormMP=new FormMedPat();
		//					FormMP.MedicationPatCur=medicationPat;
		//					FormMP.IsNew=true;
		//					FormMP.IsNewMedOrder=true;
		//					FormMP.ShowDialog();
		//					if(FormMP.DialogResult==DialogResult.OK) {
		//						ModuleSelected(PatCur.PatNum);
		//					}
		//				}
		//			}
		//			Tool_EHR_Click(true);
		//		}
		//	//#else
		//	//TODO:
		//		//Type type=FormOpenDental.AssemblyEHR.GetType("OpenDental.ObjSomeEhrSuperClass");//namespace.class
		//		object[] args;
		//		EhrQuarterlyKey keyThisQ=EhrQuarterlyKeys.GetKeyThisQuarter();
		//		if(keyThisQ==null) {
		//			MessageBox.Show("No quarterly key entered for this quarter.");
		//			return;
		//		}
		//		args=new object[] { (DateTime.Today.Year-2000).ToString(),EhrQuarterlyKeys.MonthToQuarter(DateTime.Today.Month).ToString(),
		//			PrefC.GetString(PrefName.PracticeTitle),keyThisQ.KeyValue };
		//		FormEHR Ehr = new FormEHR();
		//		Ehr.PatNum=PatCur.PatNum;
		//		Ehr.PatNotCur=PatientNoteCur;
		//		Ehr.PatFamCur=FamCur;
		//		Ehr.ShowDialog();
		//		if(!(bool)type.InvokeMember("QuarterlyKeyIsValid",System.Reflection.BindingFlags.InvokeMethod,null,FormOpenDental.ObjSomeEhrSuperClass,args)) {
		//			MessageBox.Show("Invalid quarterly key.");
		//			return;
		//		}
		//		//args=new object[] {PatCur.PatNum};
		//		//type.InvokeMember("PatNum",System.Reflection.BindingFlags.SetField,null,FormOpenDental.ObjSomeEhrSuperClass,args);
		//		//type.InvokeMember("ShowDialog",System.Reflection.BindingFlags.InvokeMethod,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.None) {
		//		//	return;
		//		//}
		//		//if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.RxEdit) {
		//		//	long launchRxNum=(long)type.InvokeMember("LaunchRxNum",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//	FormRxEdit FormRXE=new FormRxEdit(PatCur,RxPats.GetRx(launchRxNum));
		//		//	FormRXE.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.RxSelect) {
		//		//	FormRxSelect FormRS=new FormRxSelect(PatCur);
		//		//	FormRS.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Medical) {
		//		//	FormMedical formM=new FormMedical(PatientNoteCur,PatCur);
		//		//	formM.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.PatientEdit) {
		//		//	FormPatientEdit formP=new FormPatientEdit(PatCur,FamCur);
		//		//	formP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Online) {
		//		//	FormPatientPortal formPP=new FormPatientPortal();
		//		//	formPP.PatCur=PatCur;
		//		//	formPP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedReconcile) {
		//		//	FormMedicationReconcile FormMR=new FormMedicationReconcile();
		//		//	FormMR.PatCur=PatCur;
		//		//	FormMR.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Referrals) {
		//		//	FormReferralsPatient formRP=new FormReferralsPatient();
		//		//	formRP.PatNum=PatCur.PatNum;
		//		//	formRP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedicationPatEdit) {
		//		//	long medicationPatNum=(long)type.InvokeMember("LaunchMedicationPatNum",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//	FormMedPat formMP=new FormMedPat();
		//		//	formMP.MedicationPatCur=MedicationPats.GetOne(medicationPatNum);
		//		//	formMP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(true);
		//		//}
		//		/*No longer allowed to create medication orders from the MedicalOrder (CPOE) window.
		//		else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedicationPatNew) {
		//			//This cannot happen unless a provider is logged in with a valid ehr key
		//			FormMedications FormM=new FormMedications();
		//			FormM.IsSelectionMode=true;
		//			FormM.ShowDialog();
		//			if(FormM.DialogResult==DialogResult.OK) {
		//				Medication med=Medications.GetMedicationFromDb(FormM.SelectedMedicationNum);
		//				if(med.RxCui==0 //if the med has no Cui, it won't trigger an alert
		//					|| RxAlertL.DisplayAlerts(PatCur.PatNum,med.RxCui,0))//user sees alert and wants to continue
		//				{
		//					MedicationPat medicationPat=new MedicationPat();
		//					medicationPat.PatNum=PatCur.PatNum;
		//					medicationPat.MedicationNum=FormM.SelectedMedicationNum;
		//					medicationPat.ProvNum=Security.CurUser.ProvNum;
		//					FormMedPat FormMP=new FormMedPat();
		//					FormMP.MedicationPatCur=medicationPat;
		//					FormMP.IsNew=true;
		//					FormMP.ShowDialog();
		//					if(FormMP.DialogResult==DialogResult.OK) {
		//						ModuleSelected(PatCur.PatNum);
		//					}
		//				}
		//			}
		//			Tool_EHR_Click(true);
		//		}*/
		//	#endif
		//}

		private void Tool_HL7_Click() {
			DataRow row;
			if(gridProg.SelectedIndices.Length==0) {
				//autoselect procedures
				for(int i=0;i<gridProg.Rows.Count;i++) {//loop through every line showing in progress notes
					row=(DataRow)gridProg.Rows[i].Tag;
					if(row["ProcNum"].ToString()=="0") {
						continue;//ignore non-procedures
					}
					//May want to ignore procs with zero fee?
					//if((decimal)row["chargesDouble"]==0) {
					//  continue;//ignore zero fee procedures, but user can explicitly select them
					//}
					if(PIn.Date(row["ProcDate"].ToString())==DateTime.Today && PIn.Int(row["ProcStatus"].ToString())==(int)ProcStat.C) {
						gridProg.SetSelected(i,true);
					}
				}
				if(gridProg.SelectedIndices.Length==0) {//if still none selected
					MsgBox.Show(this,"Please select procedures first.");
					return;
				}
			}
			List<Procedure> procs=new List<Procedure>();
			bool allAreProcedures=true;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()=="0") {
					allAreProcedures=false;
				}
				else {
					procs.Add(Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false));
				}
			}
			if(!allAreProcedures) {
				MsgBox.Show(this,"You can only select procedures.");
				return;
			}
			long aptNum=0;
			for(int i=0;i<procs.Count;i++) {
				if(procs[i].AptNum==0) {
					continue;
				}
				aptNum=procs[i].AptNum;
				break;
			}
//todo: compare with: Bridges.ECW.AptNum, no need to generate PDF segment, pdfs only with eCW and this button not available with eCW integration
			MessageHL7 messageHL7=MessageConstructor.GenerateDFT(procs,EventTypeHL7.P03,PatCur,FamCur.ListPats[0],aptNum,"treatment","PDF Segment");
			if(messageHL7==null) {
				MsgBox.Show(this,"There is no DFT message type defined for the enabled HL7 definition.");
				return;
			}
			HL7Msg hl7Msg=new HL7Msg();
			hl7Msg.AptNum=aptNum;
			hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
			hl7Msg.MsgText=messageHL7.ToString();
			hl7Msg.PatNum=PatCur.PatNum;
			HL7Msgs.Insert(hl7Msg);
#if DEBUG
			MsgBox.Show(this,messageHL7.ToString());
#endif
		}

		private void Tool_MedLab_Click() {
			FormMedLabs FormML=new FormMedLabs();
			FormML.PatCur=PatCur;
			FormML.Show();
		}

		private void menuConsent_Popup(object sender,EventArgs e) {
			menuConsent.MenuItems.Clear();
			List<SheetDef> listSheets=SheetDefs.GetCustomForType(SheetTypeEnum.Consent);
			MenuItem menuItem;
			for(int i=0;i<listSheets.Count;i++){
				menuItem=new MenuItem(listSheets[i].Description);
				menuItem.Tag=listSheets[i];
				menuItem.Click+=new EventHandler(menuConsent_Click);
				menuConsent.MenuItems.Add(menuItem);
			}
		}

		private void menuConsent_Click(object sender,EventArgs e) {
			SheetDef sheetDef=(SheetDef)(((MenuItem)sender).Tag);
			SheetDefs.GetFieldsAndParameters(sheetDef);
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,PatCur.PatNum);
			SheetParameter.SetParameter(sheet,"PatNum",PatCur.PatNum);
			SheetFiller.FillFields(sheet);
			Graphics g=this.CreateGraphics();
			SheetUtil.CalculateHeights(sheet,g);
			g.Dispose();
			FormSheetFillEdit FormS=new FormSheetFillEdit(sheet);
			FormS.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void menuToothChart_Popup(object sender,EventArgs e) {
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			//only enable the big button if 3D graphics
			/*if(computerPref.GraphicsSimple) {
				menuItemChartBig.Enabled=false;
			}
			else {
				menuItemChartBig.Enabled=true;
			}*/
		}

		private void menuItemChartBig_Click(object sender,EventArgs e) {
			FormToothChartingBig FormT=new FormToothChartingBig(checkShowTeeth.Checked,ToothInitialList,_procList);
			FormT.Show();
		}

		private void menuItemChartSave_Click(object sender,EventArgs e) {
			long defNum=DefC.GetImageCat(ImageCategorySpecial.T);
			if(defNum==0){//no category set for Tooth Charts.
				MessageBox.Show(Lan.g(this,"No Def set for Tooth Charts."));
				return;
			}
			Point origin=this.PointToScreen(toothChart.Location);
			Bitmap chartBitmap=new Bitmap(toothChart.Width,toothChart.Height,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Graphics g=Graphics.FromImage(chartBitmap);
			g.CopyFromScreen(origin,new Point(0,0),toothChart.Size,CopyPixelOperation.SourceCopy);
			g.Dispose();
			try {
				ImageStore.Import(chartBitmap,defNum,ImageType.Photo,PatCur);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Unable to save file: ") + ex.Message);
				chartBitmap.Dispose();
				chartBitmap=null;
				return;
			}
			MsgBox.Show(this,"Saved.");
			chartBitmap.Dispose();
			chartBitmap=null;
		}

		public void FillPtInfo() {
			if(Plugins.HookMethod(this,"ContrChart.FillPtInfo",PatCur)) {
				return;
			}
			ChartLayoutHelper.GridPtInfoSetSize(gridPtInfo,tabControlImages);
			textTreatmentNotes.Text="";
			if(PatCur==null) {
				gridPtInfo.BeginUpdate();
				gridPtInfo.Rows.Clear();
				gridPtInfo.Columns.Clear();
				gridPtInfo.EndUpdate();
				return;
			}
			else {
				textTreatmentNotes.Text=PatientNoteCur.Treatment;
				textTreatmentNotes.Enabled=true;
				textTreatmentNotes.Select(textTreatmentNotes.Text.Length+2,1);
				textTreatmentNotes.ScrollToCaret();
				TreatmentNoteChanged=false;
			}
			gridPtInfo.BeginUpdate();
			gridPtInfo.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);//Lan.g("TableChartPtInfo",""),);
			gridPtInfo.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridPtInfo.Columns.Add(col);
			gridPtInfo.Rows.Clear();
			ODGridCell cell;
			ODGridRow row;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.ChartPatientInformation);
			for(int f=0;f<fields.Count;f++) {
				row=new ODGridRow();
				//within a case statement, the row may be re-instantiated if needed, effectively removing the first cell added here:
				if(fields[f].Description=="") {
					row.Cells.Add(fields[f].InternalName);
				}
				else {
					row.Cells.Add(fields[f].Description);
				}
				int ordinal=0;
				switch(fields[f].InternalName) {
					case "Age":
						row.Cells.Add(PatientLogic.DateToAgeString(PatCur.Birthdate,PatCur.DateTimeDeceased));
						break;
					case "ABC0":
						row.Cells.Add(PatCur.CreditType);
						break;
					case "Billing Type":
						row.Cells.Add(DefC.GetName(DefCat.BillingTypes,PatCur.BillingType));
						break;
					case "Referred From":
						List<RefAttach> RefAttachList=RefAttaches.Refresh(PatCur.PatNum);
						string referral="";
						for(int i=0;i<RefAttachList.Count;i++) {
							if(RefAttachList[i].IsFrom) {
								referral=Referrals.GetNameLF(RefAttachList[i].ReferralNum);
								break;
							}
						}
						if(referral=="") {
							referral="??";
						}
						row.Cells.Add(referral);
						row.Tag="Referral";
						break;
					case "Date First Visit":
						if(PatCur.DateFirstVisit.Year<1880) {
							row.Cells.Add("??");
						}
						else if(PatCur.DateFirstVisit==DateTime.Today) {
							row.Cells.Add(Lan.g("TableChartPtInfo","NEW PAT"));
						}
						else {
							row.Cells.Add(PatCur.DateFirstVisit.ToShortDateString());
						}
						row.Tag=null;
						break;
					case "Prov. (Pri, Sec)":
						string provText="";
						if(PatCur.PriProv!=0) {
							provText+=Providers.GetAbbr(PatCur.PriProv)+", ";							
						}
						else {
							provText+=Lan.g("TableChartPtInfo","None")+", ";
						}
						if(PatCur.SecProv != 0) {
							provText+=Providers.GetAbbr(PatCur.SecProv);
						}
						else {
							provText+=Lan.g("TableChartPtInfo","None");
						}
						row.Cells.Add(provText);
						row.Tag = null;
						break;
					case "Pri Ins":
						string name;
						ordinal=PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,PlanList,SubList);
						if(ordinal>0) {
							InsSub sub=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,ordinal),SubList);
							name=InsPlans.GetCarrierName(sub.PlanNum,PlanList);
							if(PatPlanList[0].IsPending) {
								name+=Lan.g("TableChartPtInfo"," (pending)");
							}
							row.Cells.Add(name);
						}
						else {
							row.Cells.Add("");
						}
						row.Tag=null;
						break;
					case "Sec Ins":
						ordinal=PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,PlanList,SubList);
						if(ordinal>0) {
							InsSub sub=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,ordinal),SubList);
							name=InsPlans.GetCarrierName(sub.PlanNum,PlanList);
							if(PatPlanList[1].IsPending) {
								name+=Lan.g("TableChartPtInfo"," (pending)");
							}
							row.Cells.Add(name);
						}
						else {
							row.Cells.Add("");
						}
						row.Tag=null;
						break;
					case "Payor Types":
						row.Tag="Payor Types";
						row.Cells.Add(PayorTypes.GetCurrentDescription(PatCur.PatNum));
						break;
					case "Registration Keys":
						//Not even available to most users.
						RegistrationKey[] keys=RegistrationKeys.GetForPatient(PatCur.PatNum);
						for(int i=0;i<keys.Length;i++) {
							//For non-guarantors with reseller keys, we do not want to show other family member reseller keys (there will be a lot of them).
							if(PatCur.PatNum!=PatCur.Guarantor
								&& keys[i].IsResellerCustomer 
								&& keys[i].PatNum!=PatCur.PatNum) 
							{
								//The current patient selected is not the guarantor and this is a reseller key for another family member.  Do not show it in this patient's chart module.
								continue;
							}
							row=new ODGridRow();
							row.Cells.Add(Lan.g("TableChartPtInfo","Registration Key"));
							string str=keys[i].RegKey.Substring(0,4)+"-"+keys[i].RegKey.Substring(4,4)+"-"
								+keys[i].RegKey.Substring(8,4)+"-"+keys[i].RegKey.Substring(12,4);
							str+="  |  PatNum: "+keys[i].PatNum.ToString();//Always show the PatNum
							if(keys[i].IsForeign) {
								str+="\r\nForeign";
							}
							else {
								str+="\r\nUSA";
							}
							str+="\r\nStarted: "+keys[i].DateStarted.ToShortDateString();
							if(keys[i].DateDisabled.Year>1880) {
								str+="\r\nDisabled: "+keys[i].DateDisabled.ToShortDateString();
							}
							if(keys[i].DateEnded.Year>1880) {
								str+="\r\nEnded: "+keys[i].DateEnded.ToShortDateString();
							}
							if(keys[i].Note!="") {
								str+=keys[i].Note;
							}
							row.Cells.Add(str);
							row.Tag=keys[i].Copy();
							gridPtInfo.Rows.Add(row);
						}
						break;
					case "Ehr Provider Keys":
						//Not even available to most users.
						List<EhrProvKey> listProvKeys=EhrProvKeys.RefreshForFam(PatCur.Guarantor);
						string desc="";
						for(int i=0;i<listProvKeys.Count;i++) {
							if(i>0) {
								desc+="\r\n";
							}
							desc+=listProvKeys[i].LName+", "+listProvKeys[i].FName+", "
								+listProvKeys[i].YearValue+", "+listProvKeys[i].ProvKey;
						}
						row.Cells.Add(desc);
						row.ColorBackG=Color.PowderBlue;
						row.Tag="EhrProvKeys";
						break;
					case "Premedicate":
						if(PatCur.Premed) {
							row=new ODGridRow();
							row.Cells.Add("");
							cell=new ODGridCell();
							if(fields[f].Description=="") {
								cell.Text=fields[f].InternalName;
							}
							else {
								cell.Text=fields[f].Description;
							}
							cell.ColorText=Color.Red;
							cell.Bold=YN.Yes;
							row.Cells.Add(cell);
							row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
							row.Tag="med";
							gridPtInfo.Rows.Add(row);
						}
						break;
					case "Problems":
						List<Disease> DiseaseList=Diseases.Refresh(PatCur.PatNum,true);
						row=new ODGridRow();
						cell=new ODGridCell();
						if(fields[f].Description=="") {
							cell.Text=fields[f].InternalName;
						}
						else {
							cell.Text=fields[f].Description;
						}
						cell.Bold=YN.Yes;
						row.Cells.Add(cell);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						if(DiseaseList.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add(Lan.g("TableChartPtInfo","none"));
						}
						//Add a new row for each med.
						for(int i=0;i<DiseaseList.Count;i++) {
							row=new ODGridRow(); 
							if(DiseaseList[i].DiseaseDefNum!=0) {
								cell=new ODGridCell(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
								cell.ColorText=Color.Red;
								cell.Bold=YN.Yes;
								row.Cells.Add(cell);
								row.Cells.Add(DiseaseList[i].PatNote);
							}
							else {
								row.Cells.Add("");
								cell=new ODGridCell(DiseaseDefs.GetItem(DiseaseList[i].DiseaseDefNum).DiseaseName);
								cell.ColorText=Color.Red;
								cell.Bold=YN.Yes;
								row.Cells.Add(cell);
								//row.Cells.Add(DiseaseList[i].PatNote);//no place to show a pat note
							}
							row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
							row.Tag="med";
							if(i!=DiseaseList.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					case "Med Urgent":
						cell=new ODGridCell();
						cell.Text=PatCur.MedUrgNote;
						cell.ColorText=Color.Red;
						cell.Bold=YN.Yes;
						row.Cells.Add(cell);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						break;
					case "Medical Summary":
						row.Cells.Add(PatientNoteCur.Medical);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						break;
					case "Service Notes":
						row.Cells.Add(PatientNoteCur.Service);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						break;
					case "Medications":
						Medications.Refresh();
						List<MedicationPat> medList=MedicationPats.Refresh(PatCur.PatNum,false);
						row=new ODGridRow();
						cell=new ODGridCell();
						if(fields[f].Description=="") {
							cell.Text=fields[f].InternalName;
						}
						else {
							cell.Text=fields[f].Description;
						}
						cell.Bold=YN.Yes;
						row.Cells.Add(cell);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						if(medList.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add(Lan.g("TableChartPtInfo","none"));
						}
						string text;
						Medication med;
						for(int i=0;i<medList.Count;i++) {
							row=new ODGridRow();
							if(medList[i].MedicationNum==0) {//NewCrop medication order.
								row.Cells.Add(medList[i].MedDescript);
							}
							else {
								med=Medications.GetMedication(medList[i].MedicationNum);
								text=med.MedName;
								if(med.MedicationNum != med.GenericNum) {
									text+="("+Medications.GetMedication(med.GenericNum).MedName+")";
								}
								row.Cells.Add(text);
							}
							text=medList[i].PatNote;
							string noteMedGeneric="";
							if(medList[i].MedicationNum!=0) {
								noteMedGeneric=Medications.GetGeneric(medList[i].MedicationNum).Notes;
							}
							if(noteMedGeneric!="") {
								text+="("+noteMedGeneric+")";
							}
							row.Cells.Add(text);
							row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
							row.Tag="med";
							if(i!=medList.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					case "Allergies":
						List<Allergy> allergyList=Allergies.GetAll(PatCur.PatNum,false);
						row=new ODGridRow();
						cell=new ODGridCell();
						if(fields[f].Description=="") {
							cell.Text=fields[f].InternalName;
						}
						else {
							cell.Text=fields[f].Description;
						}
						cell.Bold=YN.Yes;
						row.Cells.Add(cell);
						row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
						row.Tag="med";
						if(allergyList.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add(Lan.g("TableChartPtInfo","none"));
						}
						for(int i=0;allergyList.Count>i;i++) {
							row=new ODGridRow();
							cell=new ODGridCell(AllergyDefs.GetOne(allergyList[i].AllergyDefNum).Description);
							cell.Bold=YN.Yes;
							cell.ColorText=Color.Red;
							row.Cells.Add(cell);
							row.Cells.Add(allergyList[i].Reaction);
							row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][3].ItemColor;
							row.Tag="med";
							if(i!=allergyList.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					case "PatFields":
						PatField field;
						for(int i=0;i<PatFieldDefs.ListShort.Count;i++) {
							row=new ODGridRow();
							row.Cells.Add(PatFieldDefs.ListShort[i].FieldName);
							field=PatFields.GetByName(PatFieldDefs.ListShort[i].FieldName,PatFieldList);
							if(field==null) {
								row.Cells.Add("");
							}
							else {
								if(PatFieldDefs.ListShort[i].FieldType==PatFieldType.Checkbox) {
									row.Cells.Add("X");
								}
								else {
									row.Cells.Add(field.FieldValue);
								}
							}
							row.Tag="PatField"+i.ToString();
							gridPtInfo.Rows.Add(row);
						}
						break;
					case "Birthdate":
						if(PatCur.Birthdate.Year<1880) {
							row.Cells.Add("");
						}
						else {
							row.Cells.Add(PatCur.Birthdate.ToShortDateString());
						}
						break;
					case "City":
						row.Cells.Add(PatCur.City);
						break;
					case "AskToArriveEarly":
						if(PatCur.AskToArriveEarly==0) {
							row.Cells.Add("");
						}
						else {
							row.Cells.Add(PatCur.AskToArriveEarly.ToString()+" minute(s)");
						}
						break;
					case "Super Head":
						if(PatCur.SuperFamily!=0) {
							Patient tempSuper = Patients.GetPat(PatCur.SuperFamily);
							row.Cells.Add(tempSuper.GetNameLF()+" ("+tempSuper.PatNum+")");
						}
						else {
							continue;//do not allow this row to be added if there is no data to in the row.
						}
						break;
					case "Patient Portal":
						row.Tag="Patient Portal";
						if(PatCur.OnlinePassword=="") {
							row.Cells.Add(Lan.g(this,"No access"));
						}
						else {
							row.Cells.Add(Lan.g(this,"Online"));
						}
						break;
					case "References":
						List<CustRefEntry> custREList=CustRefEntries.GetEntryListForCustomer(PatCur.PatNum);
						if(custREList.Count==0) {
							row.Cells.Add(Lan.g("TablePatient","None"));
							row.Tag="References";
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
						}
						else {
							row.Cells.Add(Lan.g("TablePatient",""));
							row.Tag="References";
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
							gridPtInfo.Rows.Add(row);
						}
						for(int i=0;i<custREList.Count;i++) {
							row=new ODGridRow();
							row.Cells.Add(custREList[i].DateEntry.ToShortDateString());
							row.Cells.Add(CustReferences.GetCustNameFL(custREList[i].PatNumRef));
							row.Tag=custREList[i];
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
							if(i<custREList.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					case "Broken Appts":
						row.Tag="Broken Appts";
						int count=0;
						DataTable table=DataSetMain.Tables["ProgNotes"];
						if(ProcedureCodes.IsValidCode("D9986")) {
							for(int i=0;i<table.Rows.Count;i++) {
								Procedure proc=Procedures.GetOneProc(PIn.Long(table.Rows[i]["ProcNum"].ToString()),false);
								ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
								if(procCode.ProcCode=="D9986") {
									count++;
								}
							}
						}
						else {
							count=Adjustments.GetAdjustForPatByType(PatCur.PatNum,PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType)).Count;
						}
						row.Cells.Add(count.ToString());
						break;
				}
				if(fields[f].InternalName=="PatFields"
					|| fields[f].InternalName=="Premedicate"
					|| fields[f].InternalName=="Registration Keys") {
					//For fields that might have zero rows, we can't add the row here.  Adding rows is instead done in the case clause.
					//But some fields that are based on lists will always have one row, even if there are no items in the list.
					//Do not add those kinds here.
				}
				else {
					gridPtInfo.Rows.Add(row);
				}
			}
			gridPtInfo.EndUpdate();
		}

		private void textTreatmentNotes_TextChanged(object sender, System.EventArgs e) {
			TreatmentNoteChanged=true;
		}

		private void textTreatmentNotes_Leave(object sender, System.EventArgs e) {
			//need to skip this if selecting another module. Handled in ModuleUnselected due to click event
			if(FamCur==null)
				return;
			if(TreatmentNoteChanged){
				PatientNoteCur.Treatment=textTreatmentNotes.Text;
				PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
				TreatmentNoteChanged=false;
			}
		}

		///<summary>The supplied procedure row must include these columns: isLocked,ProcDate,ProcStatus,ProcCode,Surf,ToothNum, and ToothRange, all in raw database format.</summary>
		private bool ShouldDisplayProc(DataRow row){
			//if printing for hospital
			/*
			if(hospitalDate.Year > 1880) {
				if(hospitalDate.Date != PIn.DateT(row["ProcDate"].ToString()).Date) {
					return false;
				}
				if(row["ProcStatus"].ToString() != ((int)ProcStat.C).ToString()) {
					return false;
				}
			}*/
			if(checkShowTeeth.Checked) {//Only show selected teeth
				bool showProc = false;
				//ArrayList selectedTeeth = new ArrayList();//integers 1-32
				//for(int i = 0;i < toothChart.SelectedTeeth.Count;i++) {
				//	selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
				//}
				switch(ProcedureCodes.GetProcCode(row["ProcCode"].ToString()).TreatArea) {
					case TreatmentArea.Arch:
						for(int s=0;s<toothChart.SelectedTeeth.Count;s++) {
							if(row["Surf"].ToString() == "U" && Tooth.IsMaxillary(toothChart.SelectedTeeth[s]) ) {
								showProc = true;
							}
							else if(row["Surf"].ToString() == "L" && !Tooth.IsMaxillary(toothChart.SelectedTeeth[s])) {
								showProc = true;
							}
						}
						break;
					case TreatmentArea.Mouth:
					case TreatmentArea.None:
					case TreatmentArea.Sextant://nobody will miss it
						showProc = false;
						break;
					case TreatmentArea.Quad:
						for(int s = 0;s < toothChart.SelectedTeeth.Count;s++) {
							if(row["Surf"].ToString()=="UR" && Tooth.ToInt(toothChart.SelectedTeeth[s]) >= 1 && Tooth.ToInt(toothChart.SelectedTeeth[s]) <= 8) {
								showProc = true;
							}
							else if(row["Surf"].ToString()=="UL" && Tooth.ToInt(toothChart.SelectedTeeth[s]) >= 9 && Tooth.ToInt(toothChart.SelectedTeeth[s]) <= 16) {
								showProc = true;
							}
							else if(row["Surf"].ToString()=="LL" && Tooth.ToInt(toothChart.SelectedTeeth[s]) >= 17 && Tooth.ToInt(toothChart.SelectedTeeth[s]) <= 24) {
								showProc = true;
							}
							else if(row["Surf"].ToString()=="LR" && Tooth.ToInt(toothChart.SelectedTeeth[s]) >= 25 && Tooth.ToInt(toothChart.SelectedTeeth[s]) <= 32) {
								showProc = true;
							}
						}
						break;
					case TreatmentArea.Surf:
					case TreatmentArea.Tooth:
						for(int s=0;s<toothChart.SelectedTeeth.Count;s++) {
							if(row["ToothNum"].ToString()==toothChart.SelectedTeeth[s]) {
								showProc = true;
							}
						}
						break;
					case TreatmentArea.ToothRange:
						string[] range = row["ToothRange"].ToString().Split(',');
						for(int s = 0;s <toothChart.SelectedTeeth.Count;s++) {
							for(int r = 0;r < range.Length;r++) {
								if(range[r] == toothChart.SelectedTeeth[s]) {
									showProc = true;
								}
							}
						}
						break;
				}
				if(!showProc) {
					return false;
				}
			}
			bool isLocked=(row["isLocked"].ToString()=="X");
			if(!ProcStatDesired((ProcStat)PIn.Long(row["ProcStatus"].ToString()),isLocked)) {
				return false;
			}
			if(Programs.UsingOrion) {
				if(!OrionProcStatDesired((row["orionStatus2"].ToString()))) {
					return false;
				}
			}
			// Put check for showing hygine in here
			// Put check for showing films in here
			return true;
		}

		/// <summary> Checks ProcStat passed to see if one of the check boxes on the form contains a check for the ps passed. For example if ps is TP and the checkShowTP.Checked is true it will return true.</summary>
		private bool ProcStatDesired(ProcStat ps,bool isLocked) {
			switch(ps) {
				case ProcStat.TP:
					if(checkShowTP.Checked) {
						return true;
					}
					break;
				case ProcStat.C:
					if(checkShowC.Checked) {
						return true;
					}
					break;
				case ProcStat.EC:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.EO:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.R:
					if(checkShowR.Checked) {
						return true;
					}
					break;
				case ProcStat.D:
					if(checkAudit.Checked || (checkShowC.Checked && isLocked)) {
						return true;
					}
					break;
				case ProcStat.Cn:
					if(checkShowCn.Checked) {
						return true;
					}
					break;
				case ProcStat.TPi:
					if(checkTreatPlans.Checked) {
						return true;
					}
					break;
			}
			//TODO: if proc Date is within show date range; return true;
			return false;
		}

		private bool OrionProcStatDesired(string status2) {
			//We ought to include procs with no status2 in case one slips through the cracks and for testing.
			if(status2==OrionStatus.None.ToString() || listProcStatusCodes.SelectedItems.Count==0) {
				return true;
			}
			//Convert the graphical status "os" into a single string status "status2".
			//Not needed because we never translate orion fields to other languages.
			/*
			string status2="";
			if(os==Lans.g("enumStatus2",OrionStatus.TP.ToString())) {
				status2=OrionStatus.TP.ToString();
			}
			 * etc*/
			for(int i=0;i<listProcStatusCodes.SelectedItems.Count;i++) {
				if(listProcStatusCodes.SelectedItems[i].ToString()==status2) {
					return true;
				}
			}
			return false;
		}

		private void FillProgNotes(){
			FillProgNotes(false);
		}

		private void FillProgNotes(bool retainSelection) {
			if(PatCur!=null && PatCur.PatNum>0) {
				TreatPlans.AuditPlans(PatCur.PatNum);//consider moving this, to reduce calls to db
			}
			Plugins.HookAddCode(this,"ContrChart.FillProgNotes_begin");
			//ArrayList selectedTeeth=new ArrayList();//integers 1-32
			//for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
			//	selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
			//}
			//List<string> selectedTeeth=new List<string>(toothChart.SelectedTeeth);
			if(Programs.UsingOrion) {
				listProcStatusCodes.Visible=true;
				if(listProcStatusCodes.Items.Count==0) {
					string[] statusNames=Enum.GetNames(typeof(OrionStatus));
					for(int i=1;i<statusNames.Length;i++) {
						listProcStatusCodes.Items.Add(statusNames[i]);
					}
				}
			}
			gridProg.BeginUpdate();
			gridProg.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> fields;
			//DisplayFields.RefreshCache();
			if(gridChartViews.Rows.Count==0) {//No chart views, Use default values.
				fields=DisplayFields.GetDefaultList(DisplayFieldCategory.None);
				gridProg.Title="Progress Notes";
				if(!chartCustViewChanged) {
					checkSheets.Checked=true;
					checkTasks.Checked=true;
					checkEmail.Checked=true;
					checkCommFamily.Checked=true;
					checkAppt.Checked=true;
					checkLabCase.Checked=true;
					checkRx.Checked=true;
					checkComm.Checked=true;
					checkShowTP.Checked=true;
					checkShowC.Checked=true;
					checkShowE.Checked=true;
					checkShowR.Checked=true;
					checkShowCn.Checked=true;
					checkNotes.Checked=true;
					checkNotes.Checked=true;
					checkShowTeeth.Checked=false;
					checkAudit.Checked=false;
					textShowDateRange.Text="All Dates";
				}
			}
			else {
				if(ChartViewCurDisplay==null) {
					ChartViewCurDisplay=ChartViews.Listt[0];
				}
				fields=DisplayFields.GetForChartView(ChartViewCurDisplay.ChartViewNum);
				gridProg.Title=ChartViewCurDisplay.Description;
				if(!chartCustViewChanged) {
					checkSheets.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.Sheets)==ChartViewObjs.Sheets;
					checkTasks.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.Tasks)==ChartViewObjs.Tasks;
					checkEmail.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.Email)==ChartViewObjs.Email;
					checkCommFamily.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.CommLogFamily)==ChartViewObjs.CommLogFamily;
					checkAppt.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.Appointments)==ChartViewObjs.Appointments;
					checkLabCase.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.LabCases)==ChartViewObjs.LabCases;
					checkRx.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.Rx)==ChartViewObjs.Rx;
					checkComm.Checked=(ChartViewCurDisplay.ObjectTypes & ChartViewObjs.CommLog)==ChartViewObjs.CommLog;
					checkShowTP.Checked=(ChartViewCurDisplay.ProcStatuses & ChartViewProcStat.TP)==ChartViewProcStat.TP;
					checkShowC.Checked=(ChartViewCurDisplay.ProcStatuses & ChartViewProcStat.C)==ChartViewProcStat.C;
					checkShowE.Checked=(ChartViewCurDisplay.ProcStatuses & ChartViewProcStat.EC)==ChartViewProcStat.EC;
					checkShowR.Checked=(ChartViewCurDisplay.ProcStatuses & ChartViewProcStat.R)==ChartViewProcStat.R;
					checkShowCn.Checked=(ChartViewCurDisplay.ProcStatuses & ChartViewProcStat.Cn)==ChartViewProcStat.Cn;
					checkShowTeeth.Checked=ChartViewCurDisplay.SelectedTeethOnly;
					checkNotes.Checked=ChartViewCurDisplay.ShowProcNotes;
					checkAudit.Checked=ChartViewCurDisplay.IsAudit;
					SetDateRange();
					FillDateRange();
					gridChartViews.SetSelected(ChartViewCurDisplay.ItemOrder,true);
					if(IsDistributorKey) {
						gridCustomerViews.SetSelected(ChartViewCurDisplay.ItemOrder,true);
					}
					if(Programs.UsingOrion) {
						listProcStatusCodes.ClearSelected();
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.TP)==OrionStatus.TP) {
							listProcStatusCodes.SetSelected(0,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.C)==OrionStatus.C) {
							listProcStatusCodes.SetSelected(1,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.E)==OrionStatus.E) {
							listProcStatusCodes.SetSelected(2,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.R)==OrionStatus.R) {
							listProcStatusCodes.SetSelected(3,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.RO)==OrionStatus.RO) {
							listProcStatusCodes.SetSelected(4,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.CS)==OrionStatus.CS) {
							listProcStatusCodes.SetSelected(5,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.CR)==OrionStatus.CR) {
							listProcStatusCodes.SetSelected(6,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.CA_Tx)==OrionStatus.CA_Tx) {
							listProcStatusCodes.SetSelected(7,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.CA_EPRD)==OrionStatus.CA_EPRD) {
							listProcStatusCodes.SetSelected(8,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.CA_PD)==OrionStatus.CA_PD) {
							listProcStatusCodes.SetSelected(9,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.S)==OrionStatus.S) {
							listProcStatusCodes.SetSelected(10,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.ST)==OrionStatus.ST) {
							listProcStatusCodes.SetSelected(11,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.W)==OrionStatus.W) {
							listProcStatusCodes.SetSelected(12,true);
						}
						if((ChartViewCurDisplay.OrionStatusFlags & OrionStatus.A)==OrionStatus.A) {
							listProcStatusCodes.SetSelected(13,true);
						}
					}
				}
				else {
					gridChartViews.SetSelected(false);
					if(IsDistributorKey) {
						gridCustomerViews.SetSelected(false);
					}
				}
			}
			bool showSelectedTeeth=checkShowTeeth.Checked;
			if(Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
				checkShowTeeth.Checked=false;
			}
			DataSetMain=null;
			if(PatCur!=null) {
				if(UsingEcwTight()) {//ecw customers
					ChartModuleComponentsToLoad componentsToLoad = new ChartModuleComponentsToLoad(
					checkAppt.Checked,				        //showAppointments
					checkComm.Checked,								//showCommLog.  The button is in a different toolbar.
					checkShowC.Checked,               //showCompleted
					checkShowCn.Checked,              //showConditions
					false, //checkEmail.Checked,      //showEmail
					checkShowE.Checked,               //showExisting
					false, //checkCommFamily.Checked,	//showFamilyCommLog
					false,														//showFormPat
					checkLabCase.Checked,			        //showLabCases
					checkNotes.Checked,				        //showProcNotes
					checkShowR.Checked,				        //showReferred
					checkRx.Checked,					        //showRX
					checkSheets.Checked,			        //showSheets, consent
					false, //checkTasks.Checked,			//showTasks (for now)
					checkShowTP.Checked);			        //showTreatPlan
					DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked,componentsToLoad);//showConditions
				}
				else {//all other customers and ecw full users
					//DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked);
					DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked,new ChartModuleComponentsToLoad(
						checkAppt.Checked,				//showAppointments
						checkComm.Checked,				//showCommLog
						checkShowC.Checked,				//showCompleted
						checkShowCn.Checked,			//showConditions
						checkEmail.Checked,				//showEmail
						checkShowE.Checked,				//showExisting
						checkCommFamily.Checked,	//showFamilyCommLog
						true,											//showFormPat
						checkLabCase.Checked,			//showLabCases
						checkNotes.Checked,				//showProcNotes
						checkShowR.Checked,				//showReferred
						checkRx.Checked,					//showRX
						checkSheets.Checked,			//showSheets, consent
						checkTasks.Checked,				//showTasks
						checkShowTP.Checked)			//showTreatPlan
					);
				}
			}
			for(int i=0;i<fields.Count;i++) {
				if(fields[i].Description=="") {
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Th") {
					col.SortingStrategy=GridSortingStrategy.ToothNumberParse;
				}
				if(fields[i].InternalName=="Date") {
					col.SortingStrategy=GridSortingStrategy.DateParse;
				}
				if(fields[i].InternalName=="Amount") {
					col.SortingStrategy=GridSortingStrategy.AmountParse;
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="ADA Code"
					|| fields[i].InternalName=="User"
					|| fields[i].InternalName=="Signed"
					|| fields[i].InternalName=="Locked")
				{
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridProg.Columns.Add(col);
			}
			if(gridProg.Columns.Count<3) {//0 wouldn't be possible.
				gridProg.NoteSpanStart=0;
				gridProg.NoteSpanStop=gridProg.Columns.Count-1;
			}
			else {
				gridProg.NoteSpanStart=2;
				if(gridProg.Columns.Count>7) {
					gridProg.NoteSpanStop=7;
				}
				else {
					gridProg.NoteSpanStop=gridProg.Columns.Count-1;
				}
			}
			gridProg.Rows.Clear();
			ODGridRow row;
			//Type type;
			if(DataSetMain==null) {
				//ChartLayoutHelper.SetGridProgWidth(gridProg,ClientSize,panelEcw,textTreatmentNotes,toothChart);
				gridProg.EndUpdate();
				FillToothChart(false);//?
				ChartLayoutHelper.SetTpChartingHelper((ChartViewCurDisplay!=null && ChartViewCurDisplay.IsTpCharting),PatCur,gridProg,listButtonCats,
					checkTreatPlans,panelTP,gridTreatPlans,gridTpProcs,butNewTP,listPriorities);
				FillTreatPlans();
				FillTpProcs();
				return;
			}
			DataTable table=DataSetMain.Tables["ProgNotes"];
			List<ProcGroupItem> procGroupItems=ProcGroupItems.Refresh(PatCur.PatNum);
			_procList=new List<DataRow>();
			List<long> procNumList=new List<long>();//a list of all procNums of procs that will be visible
			bool showGroupNote;
			if(checkShowTeeth.Checked) {
				//we will want to see groupnotes that are attached to any procs that should be visible.
				foreach(DataRow rowCur in table.Rows) {
					string procNumStr=rowCur["ProcNum"].ToString();
					if(procNumStr=="0") {//if this is not a procedure
						continue;
					}
					if(rowCur["ProcCode"].ToString()==ProcedureCodes.GroupProcCode) {
						continue;//skip procgroups
					}
					if(ShouldDisplayProc(rowCur)) {
						procNumList.Add(PIn.Long(procNumStr));//remember that procnum
					}
				}
			}
			foreach(DataRow rowCur in table.Rows) {
				long procNumCur=PIn.Long(rowCur["ProcNum"].ToString());//increase code efficiency
				long patNumCur=PIn.Long(rowCur["PatNum"].ToString());//increase code efficiency
				if(procNumCur!=0) {//if this is a procedure 
					//if it's a group note and we are viewing by tooth number
					if(rowCur["ProcCode"].ToString()==ProcedureCodes.GroupProcCode && checkShowTeeth.Checked) {
						//consult the list of previously obtained procedures and ProcGroupItems to see if this procgroup should be visible.
						showGroupNote=false;
						for(int j=0;j<procGroupItems.Count;j++) {//loop through all procGroupItems for the patient. 
							if(procGroupItems[j].GroupNum==procNumCur) {//if this item is associated with this group note
								for(int k=0;k<procNumList.Count;k++) {//check all of the visible procs
									if(procNumList[k]==procGroupItems[j].ProcNum) {//if this group note is associated with a visible proc
										showGroupNote=true;
									}
								}
							}
						}
						if(!showGroupNote) {
							continue;//don't show it in the grid
						}
					}
					else {//procedure or group note, not viewing by tooth number
						if(ShouldDisplayProc(rowCur)) {
							_procList.Add(rowCur);//show it in the graphical tooth chart
							//show it in the grid below
						}
						else {
							continue;//don't show it in the grid
						}
					}
				}
				else if(rowCur["CommlogNum"].ToString()!="0") {//if this is a commlog
					if(!checkComm.Checked) {
						continue;
					}
					if(patNumCur!=PatCur.PatNum) {//if this is a different family member
						if(!checkCommFamily.Checked) {
							continue;
						}
					}
				}
				else if(rowCur["RxNum"].ToString()!="0") {//if this is an Rx
					if(!checkRx.Checked) {
						continue;
					}
				}
				else if(rowCur["LabCaseNum"].ToString()!="0") {//if this is a LabCase
					if(!checkLabCase.Checked) {
						continue;
					}
				}
				else if(rowCur["TaskNum"].ToString()!="0") {//if this is a TaskItem
					if(!checkTasks.Checked) {
						continue;
					}
					if(patNumCur!=PatCur.PatNum) {//if this is a different family member
						if(!checkCommFamily.Checked) { //uses same check box as commlog
							continue;
						}
					}
				}
				else if(rowCur["EmailMessageNum"].ToString()!="0") {//if this is an Email
					if(!checkEmail.Checked) {
						continue;
					}
				}
				else if(rowCur["AptNum"].ToString()!="0") {//if this is an Appointment
					if(!checkAppt.Checked) {
						continue;
					}
				}
				else if(rowCur["SheetNum"].ToString()!="0") {//if this is a sheet
					if(!checkSheets.Checked) {
						continue;
					}
				}
				if(ShowDateStart.Year>1880 && PIn.Date(rowCur["ProcDate"].ToString()).Date<ShowDateStart.Date) {
					continue;
				}
				if(ShowDateEnd.Year>1880 && PIn.Date(rowCur["ProcDate"].ToString()).Date>ShowDateEnd.Date) {
					continue;
				}
				row=new ODGridRow();
				row.ColorLborder=Color.Black;
				//remember that columns that start with lowercase are already altered for display rather than being raw data.
				for(int f=0;f<fields.Count;f++) {
					switch(fields[f].InternalName) {
						case "Date":
							row.Cells.Add(rowCur["procDate"].ToString());
							break;
						case "Time":
							row.Cells.Add(rowCur["procTime"].ToString());
							break;
						case "Th":
							row.Cells.Add(rowCur["toothNum"].ToString());
							break;
						case "Surf":
							row.Cells.Add(rowCur["surf"].ToString());
							break;
						case "Dx":
							row.Cells.Add(rowCur["dx"].ToString());
							break;
						case "Description":
							row.Cells.Add(rowCur["description"].ToString());
							break;
						case "Stat":
							row.Cells.Add(rowCur["procStatus"].ToString());
							break;
						case "Prov":
							row.Cells.Add(rowCur["prov"].ToString());
							break;
						case "Amount":
							row.Cells.Add(rowCur["procFee"].ToString());
							break;
						case "ADA Code":
							row.Cells.Add(rowCur["ProcCode"].ToString());
							break;
						case "User":
							row.Cells.Add(rowCur["user"].ToString());
							break;
						case "Signed":
							row.Cells.Add(rowCur["signature"].ToString());
							break;
						case "Priority":
							row.Cells.Add(rowCur["priority"].ToString());
							break;
						case "Date Entry":
							row.Cells.Add(rowCur["dateEntryC"].ToString());
							break;
						case "Prognosis":
							row.Cells.Add(rowCur["prognosis"].ToString());
							break;
						case "Date TP":
							row.Cells.Add(rowCur["dateTP"].ToString());
							break;
						case "End Time":
							row.Cells.Add(rowCur["procTimeEnd"].ToString());
							break;
						case "Quadrant":
							row.Cells.Add(rowCur["quadrant"].ToString());
							break;
						case "Schedule By":
							row.Cells.Add(rowCur["orionDateScheduleBy"].ToString());
							break;
						case "Stop Clock":
							row.Cells.Add(rowCur["orionDateStopClock"].ToString());
							break;
						case "DPC":
							row.Cells.Add(rowCur["orionDPC"].ToString());
							break;
						case "Effective Comm":
							row.Cells.Add(rowCur["orionIsEffectiveComm"].ToString());
							break;
						case "On Call":
							row.Cells.Add(rowCur["orionIsOnCall"].ToString());
							break;
						case "Stat 2":
							row.Cells.Add(rowCur["orionStatus2"].ToString());
							break;
						case "DPCpost":
							row.Cells.Add(rowCur["orionDPCpost"].ToString());
							break;
						case "Length":
							row.Cells.Add(rowCur["length"].ToString());
							break;
						case "Abbr":
							row.Cells.Add(rowCur["AbbrDesc"].ToString());
							break;
						case "Locked":
							row.Cells.Add(rowCur["isLocked"].ToString());
							break;
						default:
							row.Cells.Add("");
							break;
					}
				}
				if(checkNotes.Checked) {
					row.Note=rowCur["note"].ToString();
				}
				row.ColorText=Color.FromArgb(PIn.Int(rowCur["colorText"].ToString()));
				long provNum=PIn.Long(rowCur["ProvNum"].ToString());
				if(PrefC.GetBool(PrefName.UseProviderColorsInChart)
						&& procNumCur>0
						&& provNum>0
						&& new[] { ProcStat.C,ProcStat.EC }.Contains((ProcStat)PIn.Int(rowCur["ProcStatus"].ToString())))
				{
					row.ColorBackG=Providers.GetColor(provNum);
				}
				else {
					row.ColorBackG=Color.FromArgb(PIn.Int(rowCur["colorBackG"].ToString()));
				}
				row.Tag=rowCur;
				gridProg.Rows.Add(row);
			}
			ChartLayoutHelper.SetGridProgWidth(gridProg,ClientSize,panelEcw,textTreatmentNotes,toothChart);
			gridProg.EndUpdate();
			if(Chartscrollval==0) {
				gridProg.ScrollToEnd();
			}
			else {
				gridProg.ScrollValue=Chartscrollval;
				Chartscrollval=0;
			}
			ChartLayoutHelper.SetTpChartingHelper((ChartViewCurDisplay!=null && ChartViewCurDisplay.IsTpCharting),PatCur,gridProg,listButtonCats,
				checkTreatPlans,panelTP,gridTreatPlans,gridTpProcs,butNewTP,listPriorities);
			List<long> listTreatPlanNums=new List<long>();
			if(gridTreatPlans.SelectedIndices.Length>0) {
				listTreatPlanNums=gridTreatPlans.SelectedIndices
					.Where(x => _listTreatPlans[x].PatNum==PatCur.PatNum)//must check PatNum because _listTreatPlans might be from previous patient
					.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			}
			FillTreatPlans();
			for(int i=0;i<gridTreatPlans.Rows.Count && listTreatPlanNums.Count>0;i++) {
				gridTreatPlans.SetSelected(i,listTreatPlanNums.Contains(_listTreatPlans[i].TreatPlanNum));
			}
			if(gridTreatPlans.GetSelectedIndex()>-1) {
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			FillTpProcs();
			//create a copy of the original _procList for filling the tooth chart with all procs. Deep copy of filtered list required.
			_procListOrig=new List<DataRow>();
			DataTable tableCur=DataSetMain.Tables["ProgNotes"].Copy();
			foreach(DataRow rowCur in tableCur.Rows) {
				if(_procList.Select(x => x["ProcNum"].ToString()).Contains(rowCur["ProcNum"].ToString())) {
					_procListOrig.Add(rowCur);
				}
			}
			FillToothChart(retainSelection);
			checkShowTeeth.Checked=showSelectedTeeth;
		}

		private void FillTreatPlans() {
			gridTreatPlans.BeginUpdate();
			gridTreatPlans.Columns.Clear();
			gridTreatPlans.Columns.Add(new ODGridColumn(Lan.g("ChartTPList","Status"),50));
			gridTreatPlans.Columns.Add(new ODGridColumn(Lan.g("ChartTPList","Heading"),315));
			gridTreatPlans.Columns.Add(new ODGridColumn(Lan.g("ChartTPList","Procs"),50,HorizontalAlignment.Center));
			gridTreatPlans.Rows.Clear();
			if(PatCur==null || !checkTreatPlans.Checked) {
				gridTreatPlans.EndUpdate();
				return;
			}
			_listPatProcs=Procedures.Refresh(PatCur.PatNum);
			_arrayTpProcs=Procedures.GetListTP(_listPatProcs);//sorted by priority, then toothnum
			_listTreatPlans=TreatPlans.GetAllCurrentForPat(PatCur.PatNum);
			ODGridRow row;
			string str;
			List<TreatPlanAttach> listTpAttaches=TreatPlanAttaches.GetAllForPatNum(PatCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listTreatPlans[i].TPStatus.ToString());
				row.Cells.Add(_listTreatPlans[i].Heading);
				//if(_listTreatPlans[i].ResponsParty!=0) {
				//	//This should never be used for Active or Inactive treatment plans. Saved TPs only.
				//	str+="\r\n"+Lan.g(this,"Responsible Party: ")+Patients.GetLim(_listTreatPlans[i].ResponsParty).GetNameLF();
				//}
				row.Cells.Add(listTpAttaches.FindAll(x=>x.TreatPlanNum==_listTreatPlans[i].TreatPlanNum).Count.ToString());
				row.Tag=listTpAttaches.FindAll(x => x.TreatPlanNum==_listTreatPlans[i].TreatPlanNum);
				gridTreatPlans.Rows.Add(row);
			}
			gridTreatPlans.EndUpdate();
			gridTreatPlans.SetSelected(0,true);
		}

		///<summary>Calls FillTpProcData and FillTpProcDisplay as well as showing checkTreatPlans and filling the priority list.</summary>
		private void FillTpProcs() {
			if(!checkTreatPlans.Checked) {
				return;
			}
			FillTpProcData();
			FillTpProcDisplay();
		}
		
		/// <summary>Fills _dictTpNumListTpRows with TreatPlanNums linked to the list of TpRows for the TP, used to fill gridTpProcs.</summary>
		private void FillTpProcData() {
			_dictTpNumListTpRows=new Dictionary<long,List<TpRow>>();
			List<TpRow> listTpRows;
			for(int i=0;i<gridTreatPlans.SelectedIndices.Length;i++) {
				listTpRows=new List<TpRow>();
				long treatPlanNumCur=_listTreatPlans[gridTreatPlans.SelectedIndices[i]].TreatPlanNum;
				List<TreatPlanAttach> listTreatPlanAttaches=(List<TreatPlanAttach>)gridTreatPlans.Rows[gridTreatPlans.SelectedIndices[i]].Tag;
				List<Procedure> listProcsForTP=Procedures.GetManyProc(listTreatPlanAttaches.Select(x => x.ProcNum).ToList(),false)
					.OrderBy(x => DefC.GetOrder(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==x.ProcNum).Priority)<0)
					.ThenBy(x => DefC.GetOrder(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==x.ProcNum).Priority))
					.ThenBy(x => Tooth.ToInt(x.ToothNum))
					.ThenBy(x => x.ProcDate).ToList();
				List<ProcTP> listProcTPsCur=new List<ProcTP>();
				TpRow row;
				for(int j=0;j<listProcsForTP.Count;j++) {
					row=new TpRow();
					//Fill TpRow object with information.
					row.Priority=DefC.GetName(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcsForTP[j].ProcNum).Priority);
					row.Tth=Tooth.ToInternat(listProcsForTP[j].ToothNum);
					if(ProcedureCodes.GetProcCode(listProcsForTP[j].CodeNum).TreatArea==TreatmentArea.Surf) {
						row.Surf=Tooth.SurfTidyFromDbToDisplay(listProcsForTP[j].Surf,listProcsForTP[j].ToothNum);
					}
					else if(ProcedureCodes.GetProcCode(listProcsForTP[j].CodeNum).TreatArea==TreatmentArea.Sextant) {
						row.Surf=Tooth.GetSextant(listProcsForTP[j].Surf,(ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
					}
					else {
						row.Surf=listProcsForTP[j].Surf; //I think this will properly allow UR, L, etc.
					}
					row.Code=ProcedureCodes.GetProcCode(listProcsForTP[j].CodeNum).ProcCode;//returns new ProcedureCode if not found
					string descript=ProcedureCodes.GetLaymanTerm(listProcsForTP[j].CodeNum);
					if(listProcsForTP[j].ToothRange!="") {
						descript+=" #"+Tooth.FormatRangeForDisplay(listProcsForTP[j].ToothRange);
					}
					row.Description=descript;
					row.ColorText=DefC.GetColor(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcsForTP[j].ProcNum).Priority);
					if(row.ColorText==System.Drawing.Color.White) {
						row.ColorText=System.Drawing.Color.Black;
					}
					Procedure proc=listProcsForTP[j];
					ProcTP procTP=new ProcTP();//dummy ProcTP for local list listProcTPsCur, used as the tag on this TP grid row
					procTP.PatNum=PatCur.PatNum;
					procTP.TreatPlanNum=treatPlanNumCur;
					procTP.ProcNumOrig=proc.ProcNum;
					procTP.ItemOrder=i;
					procTP.Priority=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==proc.ProcNum).Priority;
					procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
					if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
						procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
					}
					else {
						procTP.Surf=proc.Surf;//for UR, L, etc.
					}
					procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
					procTP.Descript=row.Description;
					procTP.Prognosis=row.Prognosis;
					procTP.Dx=row.Dx;
					listProcTPsCur.Add(procTP);
					row.Tag=procTP;
					listTpRows.Add(row);
				}
				//if there is another treatment plan after this one, add a row with just the TP Header and a bold lower line
				if(i<gridTreatPlans.SelectedIndices.Length-1 && listTpRows.Count>0) {
					listTpRows[listTpRows.Count-1].ColorLborder=Color.FromArgb(102,102,122);
				}
				_listTreatPlans[gridTreatPlans.SelectedIndices[i]].ListProcTPs=listProcTPsCur;
				_dictTpNumListTpRows[_listTreatPlans[gridTreatPlans.SelectedIndices[i]].TreatPlanNum]=listTpRows;
			}
		}

		///<summary>Fills gridTpProcs with data in _dictTpNumListTpRows.  Could be filled with procs from more than one TP.</summary>
		private void FillTpProcDisplay() {
			gridTpProcs.BeginUpdate();
			gridTpProcs.Columns.Clear();
			gridTpProcs.Columns.Add(new ODGridColumn(Lan.g("GridChartTPProcs","Priority"),50));
			gridTpProcs.Columns.Add(new ODGridColumn(Lan.g("GridChartTPProcs","Tth"),35));
			gridTpProcs.Columns.Add(new ODGridColumn(Lan.g("GridChartTPProcs","Surf"),40));
			gridTpProcs.Columns.Add(new ODGridColumn(Lan.g("GridChartTPProcs","Code"),50));
			gridTpProcs.Columns.Add(new ODGridColumn(Lan.g("GridChartTPProcs","Description"),256));
			gridTpProcs.Rows.Clear();
			if(PatCur==null || _dictTpNumListTpRows==null || gridTreatPlans.Rows.Count==0) {
				gridTpProcs.EndUpdate();
				return;
			}
			ODGridRow row;
			foreach(KeyValuePair<long,List<TpRow>> kvPair in _dictTpNumListTpRows) {
				row=new ODGridRow();
				if(_dictTpNumListTpRows.Count>1) {
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add(_listTreatPlans.FindAll(x => x.TreatPlanNum==kvPair.Key).DefaultIfEmpty(new TreatPlan() { Heading="" }).FirstOrDefault().Heading);
					row.Bold=true;
					row.ColorLborder=Color.FromArgb(102,102,122);//from odGrid painting logic
					row.ColorBackG=Color.FromArgb(224,223,227);//from odGrid painting logic
					gridTpProcs.Rows.Add(row);
					row=new ODGridRow();
				}
				foreach(TpRow tpRow in kvPair.Value) {
					ProcTP procTp=new ProcTP();
					if(tpRow.Tag!=null) {
						procTp=(ProcTP)tpRow.Tag;
					}
					row.Cells.Add(tpRow.Priority??"");
					row.Cells.Add(tpRow.Tth??"");
					row.Cells.Add(tpRow.Surf??"");
					row.Cells.Add(tpRow.Code??"");
					row.Cells.Add(tpRow.Description??"");
					row.ColorText=tpRow.ColorText;
					row.ColorLborder=tpRow.ColorLborder;
					row.Tag=tpRow.Tag;//Tag is a ProcTP
					row.Bold=tpRow.Bold;
					gridTpProcs.Rows.Add(row);
					row=new ODGridRow();
				}
			}
			gridTpProcs.EndUpdate();
		}

		private void FillChartViewsGrid() {
			if(PatCur==null) {
				butChartViewAdd.Enabled=false;
				butChartViewDown.Enabled=false;
				butChartViewUp.Enabled=false;
				gridChartViews.Enabled=false;
				return;
			}
			else {
				butChartViewAdd.Enabled=true;
				butChartViewDown.Enabled=true;
				butChartViewUp.Enabled=true;
				gridChartViews.Enabled=true;
			}
			ChartViews.RefreshCache();
			gridChartViews.BeginUpdate();
			gridChartViews.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("GridChartViews","F#"),25);
			gridChartViews.Columns.Add(col);
			col=new ODGridColumn(Lan.g("GridChartViews","View"),0);
			gridChartViews.Columns.Add(col);
			gridChartViews.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ChartViews.Listt.Count;i++) {
				row=new ODGridRow();
				//assign hot keys F1-F12
				if(i<11) {
					row.Cells.Add("F"+(i+1));
				}
				row.Cells.Add(ChartViews.Listt[i].Description);
				gridChartViews.Rows.Add(row);
			}
			gridChartViews.EndUpdate();
		}

		///<summary>FillChartViewsGrid should be called first to synch the grids thus having the chart view cache already refreshed.</summary>
		private void FillCustomerViewsGrid() {
			//ChartViews.RefreshCache();
			gridCustomerViews.BeginUpdate();
			gridCustomerViews.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("GridCustomerViews","F#"),25);
			gridCustomerViews.Columns.Add(col);
			col=new ODGridColumn(Lan.g("GridCustomerViews","View"),0);
			gridCustomerViews.Columns.Add(col);
			gridCustomerViews.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ChartViews.Listt.Count;i++) {
				row=new ODGridRow();
				//assign hot keys F1-F12
				if(i<11) {
					row.Cells.Add("F"+(i+1));
				}
				row.Cells.Add(ChartViews.Listt[i].Description);
				gridCustomerViews.Rows.Add(row);
			}
			gridCustomerViews.EndUpdate();
		}

		private void FillCustomerTab() {
			FillCustomerViewsGrid();
			if(PatCur==null) {
				gridCustomerViews.Enabled=false;
				listCommonProcs.Enabled=false;
				labelMonth1.Text="";
				labelMonth2.Text="";
				labelMonth3.Text="";
			}
			else {
				//Monthly call time breakdown.
				DateTime startDate=new DateTime(1,1,1);
				Procedure firstProc=Procedures.GetFirstCompletedProcForFamily(PatCur.Guarantor);
				if(firstProc!=null) {
					startDate=firstProc.ProcDate;
				}
				DateTime month0=DateTime.Now;
				DateTime month1=DateTime.Now.AddMonths(-1);
				DateTime month2=DateTime.Now.AddMonths(-2);
				DateTime month3=DateTime.Now.AddMonths(-3);
				//Set the month labels.
				labelMonth0.Text=CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month0.Month);
				labelMonth1.Text=CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month1.Month);
				labelMonth2.Text=CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month2.Month);
				labelMonth3.Text=CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month3.Month);
				List<Commlog>	commlogsList=Commlogs.GetTimedCommlogsForPat(PatCur.Guarantor);
				TimeSpan month0Span=new TimeSpan();
				TimeSpan month1Span=new TimeSpan();
				TimeSpan month2Span=new TimeSpan();
				TimeSpan month3Span=new TimeSpan();
				TimeSpan totalSpan=new TimeSpan();
				int avgCount=0;
				bool addToAvg=true;
				//Add up the length of time each call took within the corresponding month.
				for(int i=0;i<commlogsList.Count;i++) {
					DateTime tempDateTime=commlogsList[i].CommDateTime;
					DateTime tempTimeEnd=commlogsList[i].DateTimeEnd;
					TimeSpan tempSpan=tempTimeEnd-tempDateTime;
					if(tempDateTime.Year==month0.Year && tempDateTime.Month==month0.Month) {
						month0Span=month0Span.Add(tempSpan);
						addToAvg=false;//Avg should not include this months numbers.
					}
					else if(tempDateTime.Year==month1.Year && tempDateTime.Month==month1.Month) {
						month1Span=month1Span.Add(tempSpan);
					}
					else if(tempDateTime.Year==month2.Year && tempDateTime.Month==month2.Month) {
						month2Span=month2Span.Add(tempSpan);
					}
					else if(tempDateTime.Year==month3.Year && tempDateTime.Month==month3.Month) {
						month3Span=month3Span.Add(tempSpan);
					}
					//Take current commlog and see if its greater than or equal to two months after first completed proc date.
					if(new DateTime(tempDateTime.Year,tempDateTime.Month,1)>=new DateTime(startDate.AddMonths(2).Year,startDate.AddMonths(2).Month,1)
						&& addToAvg) {
						totalSpan=totalSpan.Add(tempSpan);
						avgCount++;
					}
					addToAvg=true;
				}
				if(month0Span.Hours>=3) {
					textMonth0.BackColor=Color.Red;
					textMonth0.ForeColor=Color.White;
					textMonth0.Font=new Font(textMonth1.Font,FontStyle.Bold);
				}
				else {
					textMonth0.ForeColor=Color.Black;
					textMonth0.BackColor=SystemColors.Control;
					textMonth0.Font=new Font(textMonth1.Font,FontStyle.Regular);
				}
				if(month1Span.Hours>=3) {
					textMonth1.BackColor=Color.Red;
					textMonth1.ForeColor=Color.White;
					textMonth1.Font=new Font(textMonth1.Font,FontStyle.Bold);
				}
				else {
					textMonth1.ForeColor=Color.Black;
					textMonth1.BackColor=SystemColors.Control;
					textMonth1.Font=new Font(textMonth1.Font,FontStyle.Regular);
				}
				if(month2Span.Hours>=3) {
					textMonth2.BackColor=Color.Red;
					textMonth2.ForeColor=Color.White;
					textMonth2.Font=new Font(textMonth2.Font,FontStyle.Bold);
				}
				else {
					textMonth2.ForeColor=Color.Black;
					textMonth2.BackColor=SystemColors.Control;
					textMonth2.Font=new Font(textMonth2.Font,FontStyle.Regular);
				}
				if(month3Span.Hours>=3) {
					textMonth3.BackColor=Color.Red;
					textMonth3.ForeColor=Color.White;
					textMonth3.Font=new Font(textMonth3.Font,FontStyle.Bold);
				}
				else {
					textMonth3.ForeColor=Color.Black;
					textMonth3.BackColor=SystemColors.Control;
					textMonth3.Font=new Font(textMonth3.Font,FontStyle.Regular);
				}
				//Set the text of the boxes.
				textMonth0.Text=month0Span.ToStringHmm();
				textMonth1.Text=month1Span.ToStringHmm();
				textMonth2.Text=month2Span.ToStringHmm();
				textMonth3.Text=month3Span.ToStringHmm();
				if(avgCount>0) {
					int test=(int)totalSpan.TotalMinutes/avgCount;
					textMonthAvg.Text=new TimeSpan(0,(int)totalSpan.TotalMinutes/avgCount,0).ToStringHmm();
				}
				else {
					textMonthAvg.Text="";
				}
			}
		}

		private void SetChartView(ChartView chartView) {
			ChartViewCurDisplay=chartView;
			labelCustView.Visible=false;
			chartCustViewChanged=false;
			FillProgNotes();
		}

		///<summary>This is, of course, called when module refreshed.  But it's also called when user sets missing teeth or tooth movements.  In that case, the Progress notes are not refreshed, so it's a little faster.  This also fills in the movement amounts.</summary>
		private void FillToothChart(bool retainSelection){
			if(toothChart.Visible==false) {//if the tooth chart is not visible (for medical only feature), no need to fill with patient data
				return;
			}
			Cursor=Cursors.WaitCursor;
			toothChart.SuspendLayout();
			toothChart.ColorBackground=DefC.Long[(int)DefCat.ChartGraphicColors][10].ItemColor;
			toothChart.ColorText=DefC.Long[(int)DefCat.ChartGraphicColors][11].ItemColor;
			toothChart.ColorTextHighlight=DefC.Long[(int)DefCat.ChartGraphicColors][12].ItemColor;
			toothChart.ColorBackHighlight=DefC.Long[(int)DefCat.ChartGraphicColors][13].ItemColor;
			//remember which teeth were selected
			//ArrayList selectedTeeth=new ArrayList();//integers 1-32
			//for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
			//	selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
			//}
			List<string> selectedTeeth=new List<string>(toothChart.SelectedTeeth);
			toothChart.ResetTeeth();
			if(PatCur==null) {
				toothChart.ResumeLayout();
				FillMovementsAndHidden();
				Cursor=Cursors.Default;
				return;
			}
			//primary teeth need to be set before resetting selected teeth, because some of them might be primary.
			//primary teeth also need to be set before initial list so that we can set a primary tooth missing.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			if(checkShowTeeth.Checked || retainSelection) {
				for(int i=0;i<selectedTeeth.Count;i++) {
					toothChart.SetSelected(selectedTeeth[i],true);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++){
				switch(ToothInitialList[i].InitialType){
					case ToothInitialType.Missing:
						toothChart.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(ToothInitialList[i].ToothNum);
						break;
					//case ToothInitialType.Primary:
					//	break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			DrawProcGraphics();
			toothChart.ResumeLayout();
			FillMovementsAndHidden();
			Cursor=Cursors.Default;
		}

		private void DrawProcGraphics(){
			DataTable tableCur=DataSetMain.Tables["ProgNotes"].Copy();
			if(checkTreatPlans.Checked) {
				//filter list of DataRows to only include completed work and work for the selected treatment plans
				List<long> listProcNumsAll=gridTreatPlans.SelectedIndices.SelectMany(x => _listTreatPlans[x].ListProcTPs).Select(x => x.ProcNumOrig).ToList();
				_procList=new List<DataRow>();
				foreach(DataRow rowCur in tableCur.Rows) {
					//If proc status is anything except TP and TPi
					if(new[] { ProcStat.C,ProcStat.Cn,ProcStat.EC,ProcStat.EO,ProcStat.R }.Contains((ProcStat)PIn.Long(rowCur["ProcStatus"].ToString()))
						|| listProcNumsAll.Contains(PIn.Long(rowCur["ProcNum"].ToString())))
					{
						_procList.Add(rowCur);
					}
				}
			}
			else {
				//put list back to the original list of DataRows
				_procList=new List<DataRow>();
				foreach(DataRow rowCur in tableCur.Rows) {
					if(_procListOrig.Select(x => x["ProcNum"].ToString()).Contains(rowCur["ProcNum"].ToString())) {
						_procList.Add(rowCur);
					}
				}
			}
			//this requires: ProcStatus, ProcCode, ToothNum, HideGraphics, Surf, and ToothRange.  All need to be raw database values.
			string[] teeth;
			Color cLight=Color.White;
			Color cDark=Color.White;
			for(int i=0;i<_procList.Count;i++) {
				if(_procList[i]["HideGraphics"].ToString()=="1") {
					continue;
				}
				if(ProcedureCodes.GetProcCode(_procList[i]["ProcCode"].ToString()).PaintType==ToothPaintingType.Extraction && (
					PIn.Long(_procList[i]["ProcStatus"].ToString())==(int)ProcStat.C
					|| PIn.Long(_procList[i]["ProcStatus"].ToString())==(int)ProcStat.EC
					|| PIn.Long(_procList[i]["ProcStatus"].ToString())==(int)ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(_procList[i]["ProcCode"].ToString()).GraphicColor==Color.FromArgb(0)){
					switch((ProcStat)PIn.Long(_procList[i]["ProcStatus"].ToString())) {
						case ProcStat.C:
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][1].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][6].ItemColor;
							break;
						case ProcStat.TP:
						case ProcStat.TPi:// TPi color should be the same as TP color.
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][0].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][5].ItemColor;
							break;
						case ProcStat.EC:
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][2].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][7].ItemColor;
							break;
						case ProcStat.EO:
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][3].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][8].ItemColor;
							break;
						case ProcStat.R:
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][4].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][9].ItemColor;
							break;
						case ProcStat.Cn:
							cDark=DefC.Short[(int)DefCat.ChartGraphicColors][16].ItemColor;
							cLight=DefC.Short[(int)DefCat.ChartGraphicColors][17].ItemColor;
							break;
					}
				}
				else{
					cDark=ProcedureCodes.GetProcCode(_procList[i]["ProcCode"].ToString()).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(_procList[i]["ProcCode"].ToString()).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(_procList[i]["ProcCode"].ToString()).PaintType){
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,_procList[i]["ToothNum"].ToString())){
							toothChart.SetPontic(_procList[i]["ToothNum"].ToString(),cDark);
						}
						else{
							toothChart.SetCrown(_procList[i]["ToothNum"].ToString(),cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,_procList[i]["ToothNum"].ToString())) {
							toothChart.SetPontic(_procList[i]["ToothNum"].ToString(),cLight);
						}
						else {
							toothChart.SetCrown(_procList[i]["ToothNum"].ToString(),cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChart.SetCrown(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChart.SetCrown(_procList[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(_procList[i]["Surf"].ToString()=="U"){
							teeth=new string[14];
							for(int t=0;t<14;t++){
								teeth[t]=(t+2).ToString();
							}
						}
						else if(_procList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else{
							teeth=_procList[i]["ToothRange"].ToString().Split(new char[] {','});
						}
						for(int t=0;t<teeth.Length;t++){
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cDark);
							}
							else {
								toothChart.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(_procList[i]["Surf"].ToString()=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(_procList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=_procList[i]["ToothRange"].ToString().Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cLight);
							}
							else {
								toothChart.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChart.SetBigX(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChart.SetSurfaceColors(_procList[i]["ToothNum"].ToString(),_procList[i]["Surf"].ToString(),cDark);
						break;
				  case ToothPaintingType.FillingLight:
						toothChart.SetSurfaceColors(_procList[i]["ToothNum"].ToString(),_procList[i]["Surf"].ToString(),cLight);
						break;
					case ToothPaintingType.Implant:
						toothChart.SetImplant(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChart.SetBU(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.RCT:
						toothChart.SetRCT(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChart.SetSealant(_procList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Veneer:
						toothChart.SetVeneer(_procList[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.Watch:
						toothChart.SetWatch(_procList[i]["ToothNum"].ToString(),cDark);
						break;
				}
			}
		}

		private void checkToday_CheckedChanged(object sender, System.EventArgs e) {
			if(checkToday.Checked){
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			else{
				//
			}
		}

		///<summary>Gets run with each ModuleSelected.  Fills Dx, Prognosis, Priorities, ProcButtons, Date, and Image categories</summary>
		private void FillDxProcImage(){
			//if(textDate.errorProvider1.GetError(textDate)==""){
			if(checkToday.Checked){//textDate.Text=="" || 
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			//}
			listDx.Items.Clear();
			for(int i=0;i<DefC.Short[(int)DefCat.Diagnosis].Length;i++){//move to instantClasses?
				this.listDx.Items.Add(DefC.Short[(int)DefCat.Diagnosis][i].ItemName);
			}
			int selectedPrognosis=comboPrognosis.SelectedIndex;//retain prognosis selection
			comboPrognosis.Items.Clear();
			comboPrognosis.Items.Add(Lan.g(this,"no prognosis"));
			for(int i=0;i<DefC.Short[(int)DefCat.Prognosis].Length;i++) {
				comboPrognosis.Items.Add(DefC.Short[(int)DefCat.Prognosis][i].ItemName);
			}
			int selectedPriority=comboPriority.SelectedIndex;//retain current selection
			comboPriority.Items.Clear();
			comboPriority.Items.Add(Lan.g(this,"no priority"));
			for(int i=0;i<DefC.Short[(int)DefCat.TxPriorities].Length;i++){
				this.comboPriority.Items.Add(DefC.Short[(int)DefCat.TxPriorities][i].ItemName);
			}
			if(selectedPrognosis>0 && selectedPrognosis<comboPrognosis.Items.Count) {
				comboPrognosis.SelectedIndex=selectedPrognosis;
			}
			else {
				comboPrognosis.SelectedIndex=0;
			}
			if(selectedPriority>0 && selectedPriority<comboPriority.Items.Count)
				//set the selected to what it was before.
				comboPriority.SelectedIndex=selectedPriority;
			else
				comboPriority.SelectedIndex=0;
				//or just set to no priority
			int selectedButtonCat=listButtonCats.SelectedIndex;
			listButtonCats.Items.Clear();
			listButtonCats.Items.Add(Lan.g(this,"Quick Buttons"));
			for(int i=0;i<DefC.Short[(int)DefCat.ProcButtonCats].Length;i++){
				listButtonCats.Items.Add(DefC.Short[(int)DefCat.ProcButtonCats][i].ItemName);
			}
			if(selectedButtonCat < listButtonCats.Items.Count){
				listButtonCats.SelectedIndex=selectedButtonCat;
			}
			if(listButtonCats.SelectedIndex==-1	&& listButtonCats.Items.Count>0){
				listButtonCats.SelectedIndex=0;
			}
			FillProcButtons();
			int selectedImageTab=tabControlImages.SelectedIndex;//retains current selection
			tabControlImages.TabPages.Clear();
			TabPage page;
			page=new TabPage();
			page.Text=Lan.g(this,"All");
			tabControlImages.TabPages.Add(page);
			visImageCats=new ArrayList();
			for(int i=0;i<DefC.Short[(int)DefCat.ImageCats].Length;i++){
				if(DefC.Short[(int)DefCat.ImageCats][i].ItemValue=="X" || DefC.Short[(int)DefCat.ImageCats][i].ItemValue=="XP" || DefC.Short[(int)DefCat.ImageCats][i].ItemValue=="XR") {
					//if tagged to show in Chart
					visImageCats.Add(i);
					page=new TabPage();
					page.Text=DefC.Short[(int)DefCat.ImageCats][i].ItemName;
					tabControlImages.TabPages.Add(page);
				}
			}
			if(selectedImageTab<tabControlImages.TabCount){
				tabControlImages.SelectedIndex=selectedImageTab;
			}
			panelTPdark.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][0].ItemColor;
			panelCdark.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][1].ItemColor;
			panelECdark.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][2].ItemColor;
			panelEOdark.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][3].ItemColor;
			panelRdark.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][4].ItemColor;
			panelTPlight.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][5].ItemColor;
			panelClight.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][6].ItemColor;
			panelEClight.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][7].ItemColor;
			panelEOlight.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][8].ItemColor;
			panelRlight.BackColor=DefC.Long[(int)DefCat.ChartGraphicColors][9].ItemColor;
    }

		private void FillProcButtons(){
			listViewButtons.Items.Clear();
			imageListProcButtons.Images.Clear();
			panelQuickButtons.Visible = false;
			if(listButtonCats.SelectedIndex==-1){
				ProcButtonList=new ProcButton[0];
				return;
			}
			if(listButtonCats.SelectedIndex==0){
				panelQuickButtons.Visible = true;
				panelQuickButtons.Location=listViewButtons.Location;
				panelQuickButtons.Size=listViewButtons.Size;
				fillPanelQuickButtons();
				panelQuickButtons.Visible = true;
				panelQuickButtons.Location=listViewButtons.Location;
				panelQuickButtons.Size=listViewButtons.Size;
				ProcButtonList=new ProcButton[0];
				return;
			}
			ProcButtons.RefreshCache();
			ProcButtonList=ProcButtons.GetForCat(DefC.Short[(int)DefCat.ProcButtonCats][listButtonCats.SelectedIndex-1].DefNum);
			ListViewItem item;
			for(int i=0;i<ProcButtonList.Length;i++){
				if(ProcButtonList[i].ButtonImage!="") {
					//image keys are simply the ProcButtonNum
					imageListProcButtons.Images.Add(ProcButtonList[i].ProcButtonNum.ToString(),PIn.Bitmap(ProcButtonList[i].ButtonImage));
				}
				item=new ListViewItem(new string[] {ProcButtonList[i].Description},ProcButtonList[i].ProcButtonNum.ToString());
				listViewButtons.Items.Add(item);
			}
    }

		private void fillPanelQuickButtons(){
			panelQuickButtons.BeginUpdate();
			panelQuickButtons.Items.Clear();
			listProcButtonQuicks=ProcButtonQuicks.GetAll();
			listProcButtonQuicks.Sort(ProcButtonQuicks.sortYX);
			ODPanelItem pItem;
			for(int i=0;i<listProcButtonQuicks.Count;i++) {
				pItem=new ODPanelItem();
				pItem.Text=listProcButtonQuicks[i].Description;
				pItem.YPos=listProcButtonQuicks[i].YPos;
				pItem.ItemOrder=i;
				pItem.ItemType=(listProcButtonQuicks[i].IsLabel?ODPanelItemType.Label:ODPanelItemType.Button);
				pItem.Tags.Add(listProcButtonQuicks[i]);
				panelQuickButtons.Items.Add(pItem);
			}
			panelQuickButtons.EndUpdate();
		}

		///<summary>Gets run on ModuleSelected and each time a different images tab is selected. It first creates any missing thumbnails, then displays them. So it will be faster after the first time.</summary>
		private void FillImages(){
			visImages=new ArrayList();
			listViewImages.Items.Clear();
			imageListThumbnails.Images.Clear();
			if(!PrefC.AtoZfolderUsed) {
				//Don't show any images if there is no document path.
				return;
			}
			if(PatCur==null){
				return;
			}
			if(patFolder==""){
				return;
			}
			if(!panelImages.Visible){
				return;
			}
			for(int i=0;i<DocumentList.Length;i++){
				if(!visImageCats.Contains(DefC.GetOrder(DefCat.ImageCats,DocumentList[i].DocCategory))){
					continue;//if category not visible, continue
				}
				if(tabControlImages.SelectedIndex>0){//any category except 'all'
					if(DocumentList[i].DocCategory!=DefC.Short[(int)DefCat.ImageCats]
						[(int)visImageCats[tabControlImages.SelectedIndex-1]].DefNum)
					{
						continue;//if not in category, continue
					}
				}
				//Documents.Cur=DocumentList[i];
				imageListThumbnails.Images.Add(Documents.GetThumbnail(DocumentList[i],patFolder,
					imageListThumbnails.ImageSize.Width));
				visImages.Add(i);
				ListViewItem item=new ListViewItem(DocumentList[i].DateCreated.ToShortDateString()+": "
					+DocumentList[i].Description,imageListThumbnails.Images.Count-1);
				//item.ToolTipText=patFolder+DocumentList[i].FileName;//not supported by Mono
        listViewImages.Items.Add(item);
			}//for
		}

		///<summary>Inserts TreatPlanAttaches that allows the procedure to be charted to one or more treatment plans at the same time.</summary>
		private void AttachProcToTPs(Procedure proc) {
			if(proc.ProcStatus!=ProcStat.TP && proc.ProcStatus!=ProcStat.TPi) {
				return;
			}
			List<long> listTpNums=new List<long>();
			if(gridTreatPlans.GetSelectedIndex()>=0) {
				listTpNums=gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			}
			_listTreatPlans=TreatPlans.GetAllCurrentForPat(PatCur.PatNum);
			//If there is no active TP, make sure to add an active treatment plan.
			if(_listTreatPlans.All(x => x.TPStatus!=TreatPlanStatus.Active)) {
				TreatPlan activePlan=new TreatPlan() {
					Heading=Lans.g("TreatPlans","Active Treatment Plan"),
					Note=PrefC.GetString(PrefName.TreatmentPlanNote),
					TPStatus=TreatPlanStatus.Active,
					PatNum=PatCur.PatNum
				};
				activePlan.TreatPlanNum=TreatPlans.Insert(activePlan);
				_listTreatPlans=new List<TreatPlan>() { activePlan };
				listTpNums.Add(activePlan.TreatPlanNum);
			}
			else if(listTpNums.Count==0) {//NOT treat plan charting so no TP selected, active plan exists so get TPNum from active plan
				listTpNums.Add(_listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Active).TreatPlanNum);
			}
			long priorityNum=0;
			if(comboPriority.SelectedIndex>0) {
				priorityNum=DefC.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			}
			listTpNums.ForEach(x => TreatPlanAttaches.Insert(new TreatPlanAttach() { TreatPlanNum=x,ProcNum=proc.ProcNum,Priority=priorityNum }));
			//if all treatplans selected are not the active treatplan, then chart proc as status TPi
			//we know there is an active plan for the patient at this point
			if(_listTreatPlans.FindAll(x => listTpNums.Contains(x.TreatPlanNum)).All(x => x.TPStatus!=TreatPlanStatus.Active)) {
				Procedure procOld=proc.Copy();
				proc.ProcStatus=ProcStat.TPi;//change proc status to TPi if all selected plans are Inactive status
				Procedures.Update(proc,procOld);
			}
		}

		#region EnterTx
		private void ClearButtons() {
			//unfortunately, these colors no longer show since the XP button style was introduced.
			butM.BackColor=Color.FromName("Control"); ;
			butOI.BackColor=Color.FromName("Control");
			butD.BackColor=Color.FromName("Control");
			butL.BackColor=Color.FromName("Control");
			butBF.BackColor=Color.FromName("Control");
			butV.BackColor=Color.FromName("Control");
			textSurf.Text="";
			listDx.SelectedIndex=-1;
			//listProcButtons.SelectedIndex=-1;
			listViewButtons.SelectedIndices.Clear();
			textProcCode.Text=Lan.g(this,"Type Proc Code");
		}

		private void UpdateSurf (){
			textSurf.Text="";
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butM.BackColor==Color.White){
				textSurf.AppendText("M");
			}
			if(butOI.BackColor==Color.White){
				if(ToothGraphic.IsAnterior(toothChart.SelectedTeeth[0])){
					textSurf.AppendText("I");
				}
				else{	
					textSurf.AppendText("O");
				}
			}
			if(butD.BackColor==Color.White){
				textSurf.AppendText("D");
			}
			if(butV.BackColor==Color.White){
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					textSurf.AppendText("5");
				}
				else {
					textSurf.AppendText("V");
				}
			}
			if(butBF.BackColor==Color.White){
				if(ToothGraphic.IsAnterior(toothChart.SelectedTeeth[0])) {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						textSurf.AppendText("V");//vestibular
					}
					else {
						textSurf.AppendText("F");
					}
				}
				else {
					textSurf.AppendText("B");
				}
			}
			if(butL.BackColor==Color.White){
				textSurf.AppendText("L");
			}
		}

		private void butBF_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butBF.BackColor==Color.White){
				butBF.BackColor=SystemColors.Control;
			}
			else{
				butBF.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butV_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butV.BackColor==Color.White){
				butV.BackColor=SystemColors.Control;
			}
			else{
				butV.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butM_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butM.BackColor==Color.White){
				butM.BackColor=SystemColors.Control;
			}
			else{
				butM.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOI_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butOI.BackColor==Color.White){
				butOI.BackColor=SystemColors.Control;
			}
			else{
				butOI.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butD_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butD.BackColor==Color.White){
				butD.BackColor=SystemColors.Control;
			}
			else{
				butD.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butL_Click(object sender, System.EventArgs e){
			if(toothChart.SelectedTeeth.Count==0){
				return;
			}
			if(butL.BackColor==Color.White){
				butL.BackColor=SystemColors.Control;
			}
			else{
				butL.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void gridProg_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Chartscrollval=gridProg.ScrollValue;
			DataRow row=(DataRow)gridProg.Rows[e.Row].Tag;
			if(row["ProcNum"].ToString()!="0"){
				if(checkAudit.Checked){
					MsgBox.Show(this,"Not allowed to edit procedures when in audit mode.");
					return;
				}
				Procedure proc=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				if(ProcedureCodes.GetStringProcCode(proc.CodeNum)==ProcedureCodes.GroupProcCode){
					FormProcGroup FormP=new FormProcGroup();		
					List<ProcGroupItem> groupItemList=ProcGroupItems.GetForGroup(proc.ProcNum);
					List<Procedure> procList=new List<Procedure>();
					for(int i=0;i<groupItemList.Count;i++){
						procList.Add(Procedures.GetOneProc(groupItemList[i].ProcNum,false));
					}
					FormP.GroupCur=proc;
					FormP.GroupItemList=groupItemList;
					FormP.ProcList=procList;
					FormP.ShowDialog();
					if(FormP.DialogResult==DialogResult.OK){
						ModuleSelected(PatCur.PatNum);
						FillProgNotes();
					}
					return;
				}
				else{
					FormProcEdit FormP=new FormProcEdit(proc,PatCur,FamCur);
					Plugins.HookAddCode(this, "ContrChart.gridProg_CellDoubleClick_proc", proc, FormP);
					if(!FormP.IsDisposed) { //Form might be disposed by the above hook.
						FormP.ShowDialog();
					} 
					Plugins.HookAddCode(this, "ContrChart.gridProg_CellDoubleClick_proc2", proc, FormP);
					if(FormP.DialogResult!=DialogResult.OK) {
						return;
					}
				}
			}
			else if(row["CommlogNum"].ToString()!="0"){
				Commlog comm=Commlogs.GetOne(PIn.Long(row["CommlogNum"].ToString()));
				FormCommItem FormC=new FormCommItem(comm);
				FormC.ShowDialog();
				if(FormC.DialogResult!=DialogResult.OK){
					return;
				}
			}
			else if(row["RxNum"].ToString()!="0") {
				RxPat rx=RxPats.GetRx(PIn.Long(row["RxNum"].ToString()));
				FormRxEdit FormRxE=new FormRxEdit(PatCur,rx);
				FormRxE.ShowDialog();
				if(FormRxE.DialogResult!=DialogResult.OK){
					return;
				}
			}
			else if(row["LabCaseNum"].ToString()!="0") {
				LabCase lab=LabCases.GetOne(PIn.Long(row["LabCaseNum"].ToString()));
				FormLabCaseEdit FormL=new FormLabCaseEdit();
				FormL.CaseCur=lab;
				FormL.ShowDialog();
				//needs to always refresh due to complex ok/cancel
			}
			else if(row["TaskNum"].ToString()!="0") {
				Task task=Tasks.GetOne(PIn.Long(row["TaskNum"].ToString()));
				if(task==null) {
					MsgBox.Show(this,"This task has been deleted by another user.");
				}
				else {
					FormTaskEdit FormT=new FormTaskEdit(task,task.Copy());
					FormT.Closing+=new CancelEventHandler(TaskGoToEvent);
					FormT.Show();//non-modal
				}
			}
			else if(row["AptNum"].ToString()!="0") {
				//Appointment apt=Appointments.GetOneApt(
				FormApptEdit FormA=new FormApptEdit(PIn.Long(row["AptNum"].ToString()));
				//PinIsVisible=false
				FormA.IsInChartModule=true;
				FormA.ShowDialog();
				if(FormA.CloseOD) {
					((Form)this.Parent).Close();
					return;
				}
				if(FormA.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			else if(row["EmailMessageNum"].ToString()!="0") {
				EmailMessage msg=EmailMessages.GetOne(PIn.Long(row["EmailMessageNum"].ToString()));
				if(msg.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| msg.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
					|| msg.SentOrReceived==EmailSentOrReceived.WebMailSent
					|| msg.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
				{
					//web mail uses special secure messaging portal
					FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(PatCur.PatNum,msg.EmailMessageNum);
					if(FormWMME.ShowDialog()==DialogResult.Cancel) {//This will cause an unneccesary refresh in the cazse of a validation error with the webmail
						return;
					}
				}
				else {
					FormEmailMessageEdit FormE=new FormEmailMessageEdit(msg);
					FormE.ShowDialog();
					if(FormE.DialogResult!=DialogResult.OK) {
						return;
					}
				}
			}
			else if(row["SheetNum"].ToString()!="0") {
				Sheet sheet=Sheets.GetSheet(PIn.Long(row["SheetNum"].ToString()));
				FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheet);
				FormSFE.ShowDialog();
				if(FormSFE.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			else if(row["FormPatNum"].ToString()!="0"){
				FormPat form=FormPats.GetOne(PIn.Long(row["FormPatNum"].ToString()));
				FormFormPatEdit FormP=new FormFormPatEdit();
				FormP.FormPatCur=form;
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.OK)
				{
					ModuleSelected(PatCur.PatNum);//Why is this called here and down 3 lines? Do we need the Allocator, or should we return here?
				}
			}
			ModuleSelected(PatCur.PatNum);
			Reporting.Allocators.MyAllocator1_ProviderPayment.AllocateWithToolCheck(this.PatCur.Guarantor);
		}

		public void TaskGoToEvent(object sender,CancelEventArgs e) {
			if(PatCur==null) {
				return;
			}
			FormTaskEdit FormT=(FormTaskEdit)sender;
			TaskObjectType GotoType=FormT.GotoType;
			long keyNum=FormT.GotoKeyNum;
			if(GotoType==TaskObjectType.None) {
				ModuleSelected(PatCur.PatNum);
				return;
			}
			if(GotoType == TaskObjectType.Patient) {
				if(keyNum != 0) {
					Patient pat = Patients.GetPat(keyNum);
					OnPatientSelected(pat);
					ModuleSelected(pat.PatNum);
					return;
				}
			}
			if(GotoType == TaskObjectType.Appointment) {
				//There's nothing to do here, since we're not in the appt module.
				return;
			}
		}

		///<summary>Sets many fields for a new procedure, then displays it for editing before inserting it into the db.  No need to worry about ProcOld because it's an insert, not an update.</summary>
		private void AddProcedure(Procedure ProcCur){
			//procnum
			ProcCur.PatNum=PatCur.PatNum;
			//aptnum
			//proccode
			//ProcCur.CodeNum=ProcedureCodes.GetProcCode(ProcCur.OldCode).CodeNum;//already set
			if(newStatus==ProcStat.EO){
				ProcCur.ProcDate=DateTime.MinValue;
			}
			else if(textDate.errorProvider1.GetError(textDate)!=""){
				ProcCur.ProcDate=DateTimeOD.Today;
			}
			else{
				ProcCur.ProcDate=PIn.Date(textDate.Text);
			}
			ProcCur.DateTP=ProcCur.ProcDate;
			long provPri=PatCur.PriProv;
			long provSec=PatCur.SecProv;
			if(ProcedureCodes.GetProcCode(ProcCur.CodeNum).IsHygiene && provSec != 0) {
				ProcCur.ProvNum=provSec;
			}
			else {
				ProcCur.ProvNum=provPri;
			}
			if(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProvNumDefault!=0) {//override provnum if there is a default for this proc
				ProcCur.ProvNum=ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProvNumDefault;
			}
			ProcCur.ClinicNum=PatCur.ClinicNum;
			if(newStatus==ProcStat.R || newStatus==ProcStat.EO || newStatus==ProcStat.EC) {
				ProcCur.ProcFee=0;
			}
			else {
				//int totUnits = ProcCur.BaseUnits + ProcCur.UnitQty;
				InsPlan insPlanPrimary=null;
				InsSub insSubPrimary=null;
				if(PatPlanList.Count>0) {
					insSubPrimary=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
					insPlanPrimary=InsPlans.GetPlan(insSubPrimary.PlanNum,PlanList);
				}
				//Check if it is also a medical procedure.
				double procFee;
				bool hasMedCode = false;
				ProcCur.MedicalCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum).MedicalCode;
				if(ProcCur.MedicalCode != null && ProcCur.MedicalCode != "") {
					hasMedCode = true;
				}
				//Get fee schedule for medical or dental.
				long feeSch;
				if(hasMedCode) {
					feeSch=Fees.GetMedFeeSched(PatCur,PlanList,PatPlanList,SubList);
				}
				else {
					feeSch=Fees.GetFeeSched(PatCur,PlanList,PatPlanList,SubList);
				}
				//Get the fee amount for medical or dental.
				if(PrefC.GetBool(PrefName.MedicalFeeUsedForNewProcs) && hasMedCode) {
					procFee=Fees.GetAmount0(ProcedureCodes.GetProcCode(ProcCur.MedicalCode).CodeNum,feeSch,ProcCur.ClinicNum,ProcCur.ProvNum);
				}
				else {
					procFee=Fees.GetAmount0(ProcCur.CodeNum,feeSch,ProcCur.ClinicNum,ProcCur.ProvNum);
				}
				if(insPlanPrimary!=null && insPlanPrimary.PlanType=="p" && !(hasMedCode && insPlanPrimary.IsMedical)) {//PPO
					double provFee=Fees.GetAmount0(ProcCur.CodeNum,Providers.GetProv(Patients.GetProvNum(PatCur)).FeeSched,ProcCur.ClinicNum,ProcCur.ProvNum);
					if(provFee>procFee) {
						ProcCur.ProcFee=provFee;
					}
					else {
						ProcCur.ProcFee=procFee;
					}
				}
				else {
					ProcCur.ProcFee=procFee;
				}
			}
			//ProcCur.OverridePri=-1;
			//ProcCur.OverrideSec=-1;
			//surf
			//ToothNum
			//Procedures.Cur.ToothRange
			//ProcCur.NoBillIns=ProcedureCodes.GetProcCode(ProcCur.ProcCode).NoBillIns;
			if(comboPriority.SelectedIndex==0) {
				ProcCur.Priority=0;
			}
			else {
				ProcCur.Priority=DefC.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			}
			ProcCur.ProcStatus=newStatus;
			for(int i=0;i<ApptList.Length;i++) {
				if(ApptList[i].AptDateTime.Date==DateTime.Today && ApptList[i].AptStatus!=ApptStatus.Planned) {
					provPri=ApptList[i].ProvNum;
					provSec=ApptList[i].ProvHyg;
					break;
				}
			}
			if(newStatus==ProcStat.C) {
				ProcCur.Note=ProcCodeNotes.GetNote(ProcCur.ProvNum,ProcCur.CodeNum);
			}
			else {
				ProcCur.Note="";
			}
			if(listDx.SelectedIndex!=-1) {
				ProcCur.Dx=DefC.Short[(int)DefCat.Diagnosis][listDx.SelectedIndex].DefNum;
			}
			if(comboPrognosis.SelectedIndex==0) {
				ProcCur.Prognosis=0;
			}
			else {
				ProcCur.Prognosis=DefC.Short[(int)DefCat.Prognosis][comboPrognosis.SelectedIndex-1].DefNum;
			}
			//nextaptnum
			ProcCur.DateEntryC=DateTime.Now;
			ProcCur.BaseUnits=ProcedureCodes.GetProcCode(ProcCur.CodeNum).BaseUnits;
			ProcCur.SiteNum=PatCur.SiteNum;
			ProcCur.RevCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum).RevenueCodeDefault;
			ProcCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			if(Userods.IsUserCpoe(Security.CurUser)) {
				//This procedure is considered CPOE because the provider is the one that has added it.
				ProcCur.IsCpoe=true;
			}
			ProcCur.ProcNum=Procedures.Insert(ProcCur);
			if(newStatus==ProcStat.TP) {
				AttachProcToTPs(ProcCur);
			}
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.CodeNum).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(PatCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,new List<ClaimProc>(),true,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
			FormProcEdit FormPE=new FormProcEdit(ProcCur,PatCur.Copy(),FamCur);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.Cancel){
				//any created claimprocs are automatically deleted from within procEdit window.
				try{
					Procedures.Delete(ProcCur.ProcNum);//also deletes the claimprocs
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
				}
			}
			else if(UsingEcwTight()) {
				//do not synch recall.  Too slow
			}
			else if(Programs.UsingOrion){
				//No need to synch with Orion mode.
			}
			else if(newStatus==ProcStat.C 
				|| newStatus==ProcStat.EC 
				|| newStatus==ProcStat.EO) 
			{//only run Recalls for completed, existing current, or existing other
				//Auto-insert default encounter for provider.
				if(newStatus==ProcStat.C) {
					Encounters.InsertDefaultEncounter(ProcCur.PatNum,ProcCur.ProvNum,ProcCur.ProcDate);
				}
				Recalls.Synch(PatCur.PatNum);
			}
		}
			
		///<summary>No user dialog is shown.  This only works for some kinds of procedures.  Set the codeNum first.</summary>
		private void AddQuick(Procedure ProcCur){
			Plugins.HookAddCode(this,"ContrChart.AddQuick_begin",ProcCur);
			//procnum
			ProcCur.PatNum=PatCur.PatNum;
			//aptnum
			//ProcCur.CodeNum=ProcedureCodes.GetProcCode(ProcCur.OldCode).CodeNum;//already set
			if(newStatus==ProcStat.EO){
				ProcCur.ProcDate=DateTime.MinValue;
			}
			else if(textDate.errorProvider1.GetError(textDate)!=""){
				ProcCur.ProcDate=DateTimeOD.Today;
			}
			else{
				ProcCur.ProcDate=PIn.Date(textDate.Text);
			}
			ProcCur.DateTP=ProcCur.ProcDate;
			long provPri=PatCur.PriProv;
			long provSec=PatCur.SecProv;
			for(int i=0;i<ApptList.Length;i++) {
				if(ApptList[i].AptDateTime.Date==DateTime.Today && ApptList[i].AptStatus!=ApptStatus.Planned) {
					provPri=ApptList[i].ProvNum;
					provSec=ApptList[i].ProvHyg;
					break;
				}
			}
			if(ProcedureCodes.GetProcCode(ProcCur.CodeNum).IsHygiene && provSec != 0) {
				ProcCur.ProvNum=provSec;
			}
			else {
				ProcCur.ProvNum=provPri;
			}
			if(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProvNumDefault!=0) {//override provnum if there is a default for this proc
				ProcCur.ProvNum=ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProvNumDefault;
			}
			if(newStatus==ProcStat.C) {
				ProcCur.Note=ProcCodeNotes.GetNote(ProcCur.ProvNum,ProcCur.CodeNum);
			}
			else {
				ProcCur.Note="";
			}
			ProcCur.ClinicNum=PatCur.ClinicNum;
			if(newStatus==ProcStat.R || newStatus==ProcStat.EO || newStatus==ProcStat.EC) {
				ProcCur.ProcFee=0;
			}
			else {
				InsPlan insPlanPrimary=null;
				InsSub insSubPrimary=null;
				if(PatPlanList.Count>0) {
					insSubPrimary=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
					insPlanPrimary=InsPlans.GetPlan(insSubPrimary.PlanNum,PlanList);
				}
				//Check if it is also a medical procedure.
				double procFee;
				bool hasMedCode = false;
				ProcCur.MedicalCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum).MedicalCode;
				if(ProcCur.MedicalCode != null && ProcCur.MedicalCode != "") {
					hasMedCode = true;
				}
				//Get fee schedule for medical or dental.
				long feeSch;
				if(hasMedCode) {
					feeSch=Fees.GetMedFeeSched(PatCur,PlanList,PatPlanList,SubList);
				}
				else {
					feeSch=Fees.GetFeeSched(PatCur,PlanList,PatPlanList,SubList);
				}
				//Get the fee amount for medical or dental.
				if(PrefC.GetBool(PrefName.MedicalFeeUsedForNewProcs) && hasMedCode) {
					procFee=Fees.GetAmount0(ProcedureCodes.GetProcCode(ProcCur.MedicalCode).CodeNum,feeSch,ProcCur.ClinicNum,ProcCur.ProvNum);
				}
				else {
					procFee=Fees.GetAmount0(ProcCur.CodeNum,feeSch,ProcCur.ClinicNum,ProcCur.ProvNum);
				}
				if(insPlanPrimary!=null && insPlanPrimary.PlanType=="p" && !(hasMedCode && insPlanPrimary.IsMedical)) {//PPO
					double provFee=Fees.GetAmount0(ProcCur.CodeNum,Providers.GetProv(Patients.GetProvNum(PatCur)).FeeSched,ProcCur.ClinicNum,ProcCur.ProvNum);
					if(provFee>procFee) {
						ProcCur.ProcFee=provFee;
					}
					else {
						ProcCur.ProcFee=procFee;
					}
				}
				else {
					ProcCur.ProcFee=procFee;
				}
			}
			//surf
			//toothnum
			//ToothRange
			if(comboPriority.SelectedIndex==0) {
				ProcCur.Priority=0;
			}
			else {
				ProcCur.Priority=DefC.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			}
			ProcCur.ProcStatus=newStatus;
			if(listDx.SelectedIndex!=-1) {
				ProcCur.Dx=DefC.Short[(int)DefCat.Diagnosis][listDx.SelectedIndex].DefNum;
			}
			if(comboPrognosis.SelectedIndex==0) {
				ProcCur.Prognosis=0;
			}
			else {
				ProcCur.Prognosis=DefC.Short[(int)DefCat.Prognosis][comboPrognosis.SelectedIndex-1].DefNum;
			}
			ProcCur.BaseUnits=ProcedureCodes.GetProcCode(ProcCur.CodeNum).BaseUnits;
			ProcCur.SiteNum=PatCur.SiteNum;
			ProcCur.RevCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum).RevenueCodeDefault;
			//nextaptnum
			ProcCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			if(Userods.IsUserCpoe(Security.CurUser)) {
				//This procedure is considered CPOE because the provider is the one that has added it.
				ProcCur.IsCpoe=true;
			}
			ProcCur.ProcNum=Procedures.Insert(ProcCur);
			if(newStatus==ProcStat.TP) {
				AttachProcToTPs(ProcCur);
			}
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.CodeNum).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(PatCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			if(UsingEcwTight()) {
				//do not synch recall.  Too slow
			}
			else if(Programs.UsingOrion){
				//No need to synch with Orion mode.
			}
			else if(newStatus==ProcStat.C 
				|| newStatus==ProcStat.EC 
				|| newStatus==ProcStat.EO)
			{//only run Recalls for completed, existing current, or existing other
				//Auto-insert default encounter
				if(newStatus==ProcStat.C) {
					Encounters.InsertDefaultEncounter(ProcCur.PatNum,ProcCur.ProvNum,ProcCur.ProcDate);
				}
				Recalls.Synch(PatCur.PatNum);
			}
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,new List<ClaimProc>(),true,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
			if(Programs.UsingOrion){//Orion requires a DPC to be set. Force the proc edit window open so they can set it.
				FormProcEdit FormP=new FormProcEdit(ProcCur,PatCur.Copy(),FamCur);
				FormP.IsNew=true;
				FormP.OrionProvNum=Providers.GetOrionProvNum(ProcCur.ProvNum);
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel){
					//any created claimprocs are automatically deleted from within procEdit window.
					try{
						Procedures.Delete(ProcCur.ProcNum);//also deletes the claimprocs
					}
					catch(Exception ex){
						MessageBox.Show(ex.Message);
						return;
					}
				}
				else{
					//Do not synch. Recalls based on ScheduleByDate reports in Orion mode.
					//Recalls.Synch(PatCur.PatNum);
				}
			}
			logComplCreate(ProcCur);
		}

		private void butAddProc_Click(object sender,System.EventArgs e) {
			orionProvNum=0;
			if(newStatus==ProcStat.C) {
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
					return;
				}
			}
			bool isValid;
			TreatmentArea tArea;
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			List<string> procCodes=new List<string>();
			Procedures.SetDateFirstVisit(DateTimeOD.Today,1,PatCur);
			Procedure ProcCur;
			for(int n=0;n==0 || n<toothChart.SelectedTeeth.Count;n++) {
				isValid=true;
				ProcCur=new Procedure();//going to be an insert, so no need to set Procedures.CurOld
				//Procedure
				ProcCur.CodeNum = FormP.SelectedCodeNum;
				//Procedures.Cur.ProcCode=ProcButtonItems.CodeList[i];
				tArea=ProcedureCodes.GetProcCode(ProcCur.CodeNum).TreatArea;
				if((tArea==TreatmentArea.Arch
					|| tArea==TreatmentArea.Mouth
					|| tArea==TreatmentArea.Quad
					|| tArea==TreatmentArea.Sextant
					|| tArea==TreatmentArea.ToothRange)
					&& n>0) {//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(tArea==TreatmentArea.Quad) {
					//This is optimized for single proc like a space maintainer.  User can select a tooth to set quadrant.
					if(toothChart.SelectedTeeth.Count>0) {
						ProcCur.Surf=Tooth.GetQuadrant(toothChart.SelectedTeeth[0]);
					}
					AddProcedure(ProcCur);
				}
				else if(tArea==TreatmentArea.Surf) {
					if(toothChart.SelectedTeeth.Count==0) {
						isValid=false;
					}
					else {
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						//Procedures.Cur=ProcCur;
					}
					if(textSurf.Text=="") {
						isValid=false;
					}
					else {
						ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,ProcCur.ToothNum);
					}
					if(isValid) {
						AddQuick(ProcCur);
					}
					else {
						AddProcedure(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.Tooth) {
					if(toothChart.SelectedTeeth.Count==0) {
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						//Procedures.Cur=ProcCur;
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.ToothRange) {
					if(toothChart.SelectedTeeth.Count==0) {
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothRange="";
						for(int b=0;b<toothChart.SelectedTeeth.Count;b++) {
							if(b!=0) ProcCur.ToothRange+=",";
							ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
						}
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);//it's nice to see the procedure to verify the range
					}
				}
				else if(tArea==TreatmentArea.Arch) {
					if(toothChart.SelectedTeeth.Count==0) {
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
						continue;
					}
					if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])) {
						ProcCur.Surf="U";
					}
					else {
						ProcCur.Surf="L";
					}
					//Procedures.Cur=ProcCur;
					AddQuick(ProcCur);
				}
				else if(tArea==TreatmentArea.Sextant) {
					//Procedures.Cur=ProcCur;
					AddProcedure(ProcCur);
				}
				else {//mouth
					//Procedures.Cur=ProcCur;
					AddQuick(ProcCur);
				}
				procCodes.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
			}//for n
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
			ClearButtons();
			FillProgNotes();
			if(newStatus==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodes,PatCur.PatNum);
			}
		}
		
		private void listDx_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			//newDx=Defs.Defns[(int)DefCat.Diagnosis][listDx.IndexFromPoint(e.X,e.Y)].DefNum;
		}

		private void gridProg_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) {
				DeleteRows();
			}
		}

		private void DeleteRows(){
			if(gridProg.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Selected Item(s)?"),"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK){
				return;
			}
			int skippedSecurity=0;
			int skippedC=0;
			int skippedComlog=0;
			int skippedLabCases=0;
			int skippedSheets=0;
			int skippedApts=0;
			int skippedEmails=0;
			int skippedTasks=0;
			DataRow row;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()!="0"){
					if(PIn.Long(row["ProcStatus"].ToString())==(int)ProcStat.C
						|| PIn.Bool(row["IsLocked"].ToString()))//takes care of locked group notes and invalidated (deleted and locked) procs
					{
						skippedC++;
					}
					else{
						try{
							DateTime dateEntryC=DateTime.MinValue;
							if(row["dateEntryC"].ToString()!="") {
								dateEntryC=DateTime.Parse(row["dateEntryC"].ToString());
							}
							if(!Security.IsAuthorized(Permissions.ProcDelete,dateEntryC,true)) {
								skippedSecurity++;
								continue;
							}
							Procedures.Delete(PIn.Long(row["ProcNum"].ToString()));//also deletes the claimprocs
							SecurityLogs.MakeLogEntry(Permissions.ProcDelete,PatCur.PatNum,row["ProcCode"].ToString()+", "+PIn.Double(row["procFee"].ToString()).ToString("c"));
						}
						catch(Exception ex){
							MessageBox.Show(ex.Message);
						}
					}
				}
				else if(row["RxNum"].ToString()!="0"){
					RxPat rxPat=RxPats.GetRx(PIn.Long(row["RxNum"].ToString()));
					SecurityLogs.MakeLogEntry(Permissions.RxEdit,PatCur.PatNum,"FROM("+rxPat.RxDate.ToShortDateString()+","+rxPat.Drug+","+rxPat.ProvNum+","+rxPat.Disp+","+rxPat.Refills+")"+"\r\nTO('deleted')",rxPat.RxNum);
					RxPats.Delete(PIn.Long(row["RxNum"].ToString()));
				}
				else if(row["CommlogNum"].ToString()!="0"){
					skippedComlog++;
				}
				else if(row["LabCaseNum"].ToString()!="0") {
					skippedLabCases++;
				}
				else if(row["SheetNum"].ToString()!="0") {
					skippedSheets++;
				}
				else if(row["AptNum"].ToString()!="0") {
					skippedApts++;
				}
				else if(row["EmailMessageNum"].ToString()!="0") {
					skippedEmails++;
				}
				else if(row["TaskNum"].ToString()!="0") {
					skippedTasks++;
				}
			}
			Recalls.Synch(PatCur.PatNum);
			if(skippedC>0){
				MessageBox.Show(Lan.g(this,"Not allowed to delete completed procedures from here.")+"\r"
					+skippedC.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedComlog>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete commlog entries from here.")+"\r"
					+skippedComlog.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedSecurity>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete procedures due to security.")+"\r"
					+skippedSecurity.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedLabCases>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete lab case entries from here.")+"\r"
					+skippedLabCases.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedSheets>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete sheets from here.")+"\r"
					+skippedSheets.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedApts>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete appointments from here.")+"\r"
					+skippedApts.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedEmails>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete emails from here.")+"\r"
					+skippedEmails.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedTasks>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete tasks from here.")+"\r"
					+skippedTasks.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void radioEntryEO_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.EO;
		}

		private void radioEntryEC_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.EC;
		}

		private void radioEntryTP_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.TP;
		}

		private void radioEntryC_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.C;
		}

		private void radioEntryR_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.R;
		}

		private void radioEntryCn_CheckedChanged(object sender,EventArgs e) {
			newStatus=ProcStat.Cn;
		}

		private void listButtonCats_Click(object sender,EventArgs e) {
			FillProcButtons();
		}

		private void listViewButtons_Click(object sender,EventArgs e) {
			if(newStatus==ProcStat.C) {
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					return;
				}
			}
			if(listViewButtons.SelectedIndices.Count==0) {
				return;
			}
			ProcButton ProcButtonCur=ProcButtonList[listViewButtons.SelectedIndices[0]];
			ProcButtonClicked(ProcButtonCur.ProcButtonNum);//,"");
		}

		/////<summary>If quickbutton, then pass the code in and set procButtonNum to 0.</summary>
		//private void ProcButtonClicked(long procButtonNum,string quickcode) {
		//	ProcButtonClicked(procButtonNum,quickcode,null);
		//}

		///<summary>If quickbutton, then pass the PBQ in and set procButtonNum to 0.</summary>
		private void ProcButtonClicked(long procButtonNum,ProcButtonQuick pbq=null) {
			orionProvNum=0;
			if(newStatus==ProcStat.C){
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))){
					return;
				}
			}
			#if TRIALONLY
				if(procButtonNum==0){
					MsgBox.Show(this,"Quick buttons do not work in the trial version because dummy codes are being used instead of real codes.  Just to the left, change to a different category to see other procedure buttons available which do work.");
					return;
				}
			#endif 
			bool isValid;
			TreatmentArea tArea;
			int quadCount=0;//automates quadrant codes.
			long[] codeList;
			long[] autoCodeList;
			if(procButtonNum==0){
				codeList=new long[1];
				codeList[0]=ProcedureCodes.GetCodeNum(pbq.CodeValue);
				if(codeList[0]==0) {
					MessageBox.Show(this,Lan.g(this,"Procedure code does not exist in database")+" : "+pbq.CodeValue);
					return;
				}
				autoCodeList=new long[0];
			}
			else{
				codeList=ProcButtonItems.GetCodeNumListForButton(procButtonNum);
				autoCodeList=ProcButtonItems.GetAutoListForButton(procButtonNum);
				//if(codeList.
			}
			//It is very important that we stop users here before entering any procedures or doing any automation.
			for(int i=0;i<codeList.Length;i++) {
				if(IsToothRangeWithPrimary(codeList[i])) {
					//FormProcEdit does not currently allow you to select primary teeth for a tooth range procedure code.  We don't allow users to create these procedures because they would never be able to edit.
					MsgBox.Show(this,"The tooth range treatment area doesn't allow primary teeth.  Please unselect any primary teeth.");
					return;
				}
			}
			//Do not return past this point---------------------------------------------------------------------------------
			Procedures.SetDateFirstVisit(DateTimeOD.Today,1,PatCur);
			List<string> procCodes=new List<string>();
			Procedure ProcCur=null;
			//"Bug fix" for Dr. Lazar-------------
			bool isPeriapicalSix=false;
			if(codeList.Length==6) {//quick check before checking all codes. So that the program isn't slowed down too much.
				string tempVal="";
				foreach(long code in codeList) {
					tempVal+=ProcedureCodes.GetProcCode(code).AbbrDesc;
				}
				if(tempVal=="PAPA+PA+PA+PA+PA+") {
					isPeriapicalSix = true;
					toothChart.SelectedTeeth.Clear();//set tooth numbers later
				}
			}
			for(int i=0;i<codeList.Length;i++){
				//needs to loop at least once, regardless of whether any teeth are selected.	
				for(int n=0;n==0 || n<toothChart.SelectedTeeth.Count;n++) {//Consider changing to a Do{}While() loop.
					isValid=true;
					ProcCur=new Procedure();//insert, so no need to set CurOld
					ProcCur.CodeNum=ProcedureCodes.GetProcCode(codeList[i]).CodeNum;
					tArea=ProcedureCodes.GetProcCode(ProcCur.CodeNum).TreatArea;
					//"Bug fix" for Dr. Lazar-------------
					if(isPeriapicalSix) {
						//PA code is already set to treatment area mouth by default.
						ProcCur.ToothNum=",8,14,19,24,30".Split(',')[i];//first code has tooth num "";
						if(i==0) {
							tArea=TreatmentArea.Mouth;
						}
						else {
							tArea=TreatmentArea.Tooth;
						}
					}
					if((tArea==TreatmentArea.Arch
						|| tArea==TreatmentArea.Mouth
						|| tArea==TreatmentArea.Quad
						|| tArea==TreatmentArea.Sextant
						|| tArea==TreatmentArea.ToothRange)
						&& n>0){//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==TreatmentArea.Quad){
						switch(quadCount%4){
							case 0: ProcCur.Surf="UR"; break;
							case 1: ProcCur.Surf="UL"; break;
							case 2: ProcCur.Surf="LL"; break;
							case 3: ProcCur.Surf="LR"; break;
						}
						quadCount++;
						if(pbq!=null && !string.IsNullOrWhiteSpace(pbq.Surf)) {//from quick buttons only.
							ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(pbq.Surf,ProcCur.ToothNum);
							//ProcCur.Surf=pbq.Surf;
						}
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Surf){
						if(toothChart.SelectedTeeth.Count==0){
							isValid=false;
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						}
						if(textSurf.Text=="" && pbq==null){
							isValid=false;// Pre-ODButtonPanel behavior
						}
						else if(pbq!=null && pbq.Surf==""){
							isValid=false; // ODButtonPanel behavior
						}
						else {
							ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,ProcCur.ToothNum);//it's ok if toothnum is not valid.
							if(pbq!=null && !string.IsNullOrWhiteSpace(pbq.Surf)) {//from quick buttons only.
								ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(pbq.Surf,ProcCur.ToothNum);
								//ProcCur.Surf=pbq.Surf;
							}
						}
						if(isValid){
							AddQuick(ProcCur);
						}
						else{
							AddProcedure(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Tooth){
						if(isPeriapicalSix) {
							AddQuick(ProcCur);
						}
						else if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.ToothRange){
						if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothRange="";
							for(int b=0;b<toothChart.SelectedTeeth.Count;b++) {
								if(b!=0) ProcCur.ToothRange+=",";
								ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
							}
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Arch){
						if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
							continue;
						}
						if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])){
							ProcCur.Surf="U";
						}
						else{
							ProcCur.Surf="L";
						}
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Sextant){
						AddProcedure(ProcCur);
					}
					else{//mouth
						AddQuick(ProcCur);
					}
					procCodes.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
				}//n selected teeth
			}//end Part 1 checking for ProcCodes, now will check for AutoCodes
			string toothNum;
			bool isAdditional;
			//long orionProvNum=0;
			for(int i=0;i<autoCodeList.Length;i++){
				for(int n=0;n==0 || n<toothChart.SelectedTeeth.Count;n++) {
					isValid=true;
					if(toothChart.SelectedTeeth.Count!=0)
						toothNum=toothChart.SelectedTeeth[n];
					else
						toothNum="";
					isAdditional= n!=0;
					ProcCur=new Procedure();//this will be an insert, so no need to set CurOld
					//Clean to db
					string surf=Tooth.SurfTidyForClaims(textSurf.Text,toothNum);
					ProcCur.CodeNum=AutoCodeItems.GetCodeNum(autoCodeList[i],toothNum,surf,isAdditional,PatCur.PatNum,PatCur.Age);
					tArea=ProcedureCodes.GetProcCode(ProcCur.CodeNum).TreatArea;
					if((tArea==TreatmentArea.Arch
						|| tArea==TreatmentArea.Mouth
						|| tArea==TreatmentArea.Quad
						|| tArea==TreatmentArea.Sextant
						|| tArea==TreatmentArea.ToothRange)
						&& n>0){//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==TreatmentArea.Quad){
						switch(quadCount%4){
							case 0: ProcCur.Surf="UR"; break;
							case 1: ProcCur.Surf="UL"; break;
							case 2: ProcCur.Surf="LL"; break;
							case 3: ProcCur.Surf="LR"; break;
						}
						quadCount++;
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Surf){
						if(toothChart.SelectedTeeth.Count==0){
							isValid=false;
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						}
						if(textSurf.Text==""){
							isValid=false;
						}
						else{
							ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,ProcCur.ToothNum);//it's ok if toothnum is invalid
						}
						
						if(isValid){
							AddQuick(ProcCur);
						}
						else{
							AddProcedure(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Tooth){
						if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.ToothRange){
						if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothRange="";
							for(int b=0;b<toothChart.SelectedTeeth.Count;b++) {
								if(b!=0) ProcCur.ToothRange+=",";
								ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
							}
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Arch){
						if(toothChart.SelectedTeeth.Count==0) {
							AddProcedure(ProcCur);
							continue;
						}
						if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])){
							ProcCur.Surf="U";
						}
						else{
							ProcCur.Surf="L";
						}
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Sextant){
						AddProcedure(ProcCur);
					}
					else{//mouth
						AddQuick(ProcCur);
					}
					procCodes.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
				}//n selected teeth
				//orionProvNum=ProcCur.ProvNum;
			}//for i
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);			
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
			ClearButtons();
			FillProgNotes();
			if(newStatus==ProcStat.C){
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodes,PatCur.PatNum);
			}
		}

		private void textProcCode_TextChanged(object sender,EventArgs e) {
			if(textProcCode.Text=="d") {
				textProcCode.Text="D";
				textProcCode.SelectionStart=1;
			}
		}

		private void textProcCode_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				EnterTypedCode();
			}
		}

		private void textProcCode_Enter(object sender,EventArgs e) {
			if(textProcCode.Text==Lan.g(this,"Type Proc Code")) {
				textProcCode.Text="";
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			EnterTypedCode();
		}

		private void EnterTypedCode() {
			//orionProcNum=0;
			if(newStatus==ProcStat.C) {
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
					return;
				}
			}
			if(CultureInfo.CurrentCulture.Name=="en-US" && Regex.IsMatch(textProcCode.Text,@"^\d{4}$")){//if exactly 4 digits
				if(!ProcedureCodeC.HList.ContainsKey(textProcCode.Text)) {//4 digit code is not found
					textProcCode.Text="D"+textProcCode.Text;
				}
				else { //or if it's a 4 digit code that's hidden, also add the D
					ProcedureCode procCode=ProcedureCodes.GetProcCode(textProcCode.Text);
					if(DefC.GetHidden(DefCat.ProcCodeCats,procCode.ProcCat)) {
						textProcCode.Text="D"+textProcCode.Text;
					}
				}
			}
			if(!ProcedureCodeC.HList.ContainsKey(textProcCode.Text)) {
				MessageBox.Show(Lan.g(this,"Invalid code."));
				//textProcCode.Text="";
				textProcCode.SelectionStart=textProcCode.Text.Length;
				return;
			}
			if(DefC.GetHidden(DefCat.ProcCodeCats,ProcedureCodes.GetProcCode(textProcCode.Text).ProcCat)) {//if the category is hidden
				MessageBox.Show(Lan.g(this,"Code is in a hidden category and cannot be added from here."));
				textProcCode.SelectionStart=textProcCode.Text.Length;
				return;
			}
			if(IsToothRangeWithPrimary(ProcedureCodes.GetCodeNum(textProcCode.Text))) {
				//FormProcEdit does not currently allow you to select primary teeth for a tooth range procedure code.  We don't allow users to create these procedures because they would never be able to edit.
				MsgBox.Show(this,"The tooth range treatment area doesn't allow primary teeth.  Please unselect any primary teeth.");
				return;
			}
			//Do not return past this point---------------------------------------------------------------------------------
			List<string> procCodes=new List<string>();
			Procedures.SetDateFirstVisit(DateTimeOD.Today,1,PatCur);
			TreatmentArea tArea;
			Procedure ProcCur;
			for(int n=0;n==0 || n<toothChart.SelectedTeeth.Count;n++) {//always loops at least once.
				ProcCur=new Procedure();//this will be an insert, so no need to set CurOld
				ProcCur.CodeNum=ProcedureCodes.GetCodeNum(textProcCode.Text);
				bool isValid=true;
				tArea=ProcedureCodes.GetProcCode(ProcCur.CodeNum).TreatArea;
				if((tArea==TreatmentArea.Arch
					|| tArea==TreatmentArea.Mouth
					|| tArea==TreatmentArea.Quad
					|| tArea==TreatmentArea.Sextant
					|| tArea==TreatmentArea.ToothRange)
					&& n>0) {//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(tArea==TreatmentArea.Quad) {
					//This is optimized for single proc like a space maintainer.  User can select a tooth to set quadrant.
					if(toothChart.SelectedTeeth.Count>0) {
						ProcCur.Surf=Tooth.GetQuadrant(toothChart.SelectedTeeth[0]);
						AddQuick(ProcCur);
					}
					else {
						AddProcedure(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.Surf) {
					if(toothChart.SelectedTeeth.Count==0) {
						isValid=false;
					}
					else {
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
					}
					if(textSurf.Text=="") {
						isValid=false;
					}
					else {
						ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,ProcCur.ToothNum);//it's ok if toothnum is invalid
					}
					if(isValid) {
						AddQuick(ProcCur);
					}
					else {
						AddProcedure(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.Tooth) {
					if(toothChart.SelectedTeeth.Count==0) {
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.ToothRange) {
					if(toothChart.SelectedTeeth.Count==0) {
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothRange="";
						for(int b=0;b<toothChart.SelectedTeeth.Count;b++) {
							if(b!=0) ProcCur.ToothRange+=",";
							ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
						}
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.Arch) {
					if(toothChart.SelectedTeeth.Count==0) {
						AddProcedure(ProcCur);
						continue;
					}
					if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])) {
						ProcCur.Surf="U";
					}
					else {
						ProcCur.Surf="L";
					}
					AddQuick(ProcCur);
				}
				else if(tArea==TreatmentArea.Sextant) {
					AddProcedure(ProcCur);
				}
				else {//mouth
					AddQuick(ProcCur);
				}
				procCodes.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
			}//n selected teeth
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);			
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
			ClearButtons();
			FillProgNotes();
			textProcCode.Text="";
			textProcCode.Select();
			if(newStatus==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodes,PatCur.PatNum);
			}
		}

		///<summary>Returns true if the code's treatment area is "ToothRange" and if any selected teeth in the chart module are primary.</summary>
		private bool IsToothRangeWithPrimary(long codeNum) {
			if(ProcedureCodes.GetProcCode(codeNum).TreatArea==TreatmentArea.ToothRange) {
				for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
					//Don't need to check the tooth nomenclature because the selected teeth are always the same letters/numbers.
					if(Regex.IsMatch(toothChart.SelectedTeeth[i],"[A-Z]")) {
						return true;
					}
				}
			}
			return false;
		}

		private void checkTreatPlans_CheckedChanged(object sender,EventArgs e) {
			gridTreatPlans.SetSelected(false);
			if(_listTreatPlans!=null && _listTreatPlans.Count>0) {
				gridTreatPlans.SetSelected(0,true);
			}
			FillProgNotes();
		}
		#endregion EnterTx

		#region MissingTeeth
		private void butMissing_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Missing);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butNotMissing_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.ClearValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Missing);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butEdentulous_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Missing);
			for(int i=1;i<=32;i++){
				ToothInitials.SetValueQuick(PatCur.PatNum,i.ToString(),ToothInitialType.Missing,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butHidden_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Hidden);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butUnhide_Click(object sender,EventArgs e) {
			if(listHidden.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an item from the list first.");
				return;
			}
			ToothInitials.ClearValue(PatCur.PatNum,(string)HiddenTeeth[listHidden.SelectedIndex],ToothInitialType.Hidden);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}
		#endregion MissingTeeth

		#region Movements
		private void FillMovementsAndHidden(){
			if(tabProc.Height<50){//skip if the tab control is short(not visible){
				return;
			}
			if(tabProc.SelectedIndex==2)//movements tab
			{
				if(toothChart.SelectedTeeth.Count==0) {
					textShiftM.Text="";
					textShiftO.Text="";
					textShiftB.Text="";
					textRotate.Text="";
					textTipM.Text="";
					textTipB.Text="";
					return;
				}
				textShiftM.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftM).ToString();  
				textShiftO.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftO).ToString();
				textShiftB.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftB).ToString();
				textRotate.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.Rotate).ToString();
				textTipM.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.TipM).ToString();
				textTipB.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.TipB).ToString();
				//At this point, all 6 blanks have either a number or 0.
				//As we go through this loop, none of the values will change.
				//The only thing that will happen is that some of them will become blank.
				string move;
				for(int i=1;i<toothChart.SelectedTeeth.Count;i++) {
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM).ToString();
					if(textShiftM.Text != move){
						textShiftM.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO).ToString();
					if(textShiftO.Text != move) {
						textShiftO.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB).ToString();
					if(textShiftB.Text != move) {
						textShiftB.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.Rotate).ToString();
					if(textRotate.Text != move) {
						textRotate.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.TipM).ToString();
					if(textTipM.Text != move) {
						textTipM.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.TipB).ToString();
					if(textTipB.Text != move) {
						textTipB.Text="";
					}
				}
			}//if movements tab
			if(tabProc.SelectedIndex==1){//missing teeth
				listHidden.Items.Clear();
				HiddenTeeth=ToothInitials.GetHiddenTeeth(ToothInitialList);
				for(int i=0;i<HiddenTeeth.Count;i++){
					listHidden.Items.Add(Tooth.ToInternat((string)HiddenTeeth[i]));
				}
			}
		}

		private void butShiftMminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftMplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butRotateMinus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Rotate,-20);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butRotatePlus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Rotate,20);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipM,-10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipM,10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipB,-10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipB,10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butApplyMovements_Click(object sender,EventArgs e) {
			if(textShiftM.errorProvider1.GetError(textShiftM)!=""
				|| textShiftO.errorProvider1.GetError(textShiftO)!=""
				|| textShiftB.errorProvider1.GetError(textShiftB)!=""
				|| textRotate.errorProvider1.GetError(textRotate)!=""
				|| textTipM.errorProvider1.GetError(textTipM)!=""
				|| textTipB.errorProvider1.GetError(textTipB)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				if(textShiftM.Text!=""){
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftM,PIn.Float(textShiftM.Text));
				}
				if(textShiftO.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftO,PIn.Float(textShiftO.Text));
				}
				if(textShiftB.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftB,PIn.Float(textShiftB.Text));
				}
				if(textRotate.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.Rotate,PIn.Float(textRotate.Text));
				}
				if(textTipM.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.TipM,PIn.Float(textTipM.Text));
				}
				if(textTipB.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.TipB,PIn.Float(textTipB.Text));
				}
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butClearAllMovements_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This will clear all movements made on all teeth for this patient.  Continue?")) {
				return;
			}
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Rotate);
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.ShiftB);
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.ShiftM);
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.ShiftO);
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.TipB);
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.TipM);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}
		#endregion Movements

		#region Primary
		private void butPrimary_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Primary);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butPerm_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Count==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Count;i++) {
				if(Tooth.IsPrimary(toothChart.SelectedTeeth[i])){
					ToothInitials.ClearValue(PatCur.PatNum,Tooth.PriToPerm(toothChart.SelectedTeeth[i])
						,ToothInitialType.Primary);
				}
				else{
					ToothInitials.ClearValue(PatCur.PatNum,toothChart.SelectedTeeth[i]
						,ToothInitialType.Primary);
				}
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butAllPrimary_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			for(int i=1;i<=32;i++){
				ToothInitials.SetValueQuick(PatCur.PatNum,i.ToString(),ToothInitialType.Primary,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butAllPerm_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butMixed_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			string[] priTeeth=new string[] 
				{"1","2","4","5","6","11","12","13","15","16","17","18","20","21","22","27","28","29","31","32"};
			for(int i=0;i<priTeeth.Length;i++) {
				ToothInitials.SetValueQuick(PatCur.PatNum,priTeeth[i],ToothInitialType.Primary,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}
		#endregion Primary

		#region Planned
		private void FillPlanned(){
			if(PatCur==null){
				checkDone.Checked=false;
				butNew.Enabled=false;
				butPin.Enabled=false;
				butClear.Enabled=false;
				butUp.Enabled=false;
				butDown.Enabled=false;
				gridPlanned.Enabled=false;
				return;
			}
			else{
				butNew.Enabled=true;
				butPin.Enabled=true;
				butClear.Enabled=true;
				butUp.Enabled=true;
				butDown.Enabled=true;
				gridPlanned.Enabled=true;
			}
			if(PatCur.PlannedIsDone) {
				checkDone.Checked=true;
			}
			else {
				checkDone.Checked=false;
			}
			//Fill grid
			gridPlanned.BeginUpdate();
			gridPlanned.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TablePlannedAppts","#"),25,HorizontalAlignment.Center);
			gridPlanned.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePlannedAppts","Min"),35);
			gridPlanned.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePlannedAppts","Procedures"),175);
			gridPlanned.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePlannedAppts","Note"),175);
			gridPlanned.Columns.Add(col);
			if(Programs.UsingOrion) {
				col=new ODGridColumn(Lan.g("TablePlannedAppts","SchedBy"),80);
			}
			else {
				col=new ODGridColumn(Lan.g("TablePlannedAppts","DateSched"),80);
			}
			gridPlanned.Columns.Add(col);
			gridPlanned.Rows.Clear();
			ODGridRow row;
			_tablePlannedAll=DataSetMain.Tables["Planned"];
			//This gets done in the business layer:
			/*
			bool iochanged=false;
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["ItemOrder"].ToString()!=i.ToString()) {
					PlannedAppt planned=PlannedAppts.CreateObject(PIn.PLong(table.Rows[i]["PlannedApptNum"].ToString()));
					planned.ItemOrder=i;
					PlannedAppts.InsertOrUpdate(planned);
					iochanged=true;
				}
			}
			if(iochanged) {
				DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked);
				table=DataSetMain.Tables["Planned"];
			}*/
			_listPlannedAppt=new List<DataRow>();
			for(int i=0;i<_tablePlannedAll.Rows.Count;i++){
				if(_tablePlannedAll.Rows[i]["AptStatus"].ToString()=="2" && !checkShowCompleted.Checked) {
					continue;
				}
				_listPlannedAppt.Add(_tablePlannedAll.Rows[i]);//List containing only rows we are showing, can be the same as _tablePlannedAll
				row=new ODGridRow();
				row.Cells.Add((gridPlanned.Rows.Count+1).ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["minutes"].ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["ProcDescript"].ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["Note"].ToString());
				if(Programs.UsingOrion) {
					string text;
					List<Procedure> procsList=Procedures.Refresh(PatCur.PatNum);
					DateTime newDateSched=new DateTime();
					for(int p=0;p<procsList.Count;p++) {
						if(procsList[p].PlannedAptNum==PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString())) {
							OrionProc op=OrionProcs.GetOneByProcNum(procsList[p].ProcNum);
							if(op!=null && op.DateScheduleBy.Year>1880) {
								if(newDateSched.Year<1880) {
									newDateSched=op.DateScheduleBy;
								}
								else {
									if(op.DateScheduleBy<newDateSched) {
										newDateSched=op.DateScheduleBy;
									}
								}
							}
						}
					}
					if(newDateSched.Year>1880) {
						text=newDateSched.ToShortDateString();
					}
					else {
						text="None";
					}
					row.Cells.Add(text);
				}
				else {//Not Orion
					ApptStatus aptStatus=(ApptStatus)(PIn.Long(_tablePlannedAll.Rows[i]["AptStatus"].ToString()));
					if(aptStatus==ApptStatus.UnschedList) {
						row.Cells.Add(Lan.g(this,"Unsched"));
					}
					else if(aptStatus==ApptStatus.Broken) {
						row.Cells.Add(Lan.g(this,"Broken"));
					}
					else {//scheduled, complete and ASAP
						row.Cells.Add(_tablePlannedAll.Rows[i]["dateSched"].ToString());
					}
				}
				row.ColorText=Color.FromArgb(PIn.Int(_tablePlannedAll.Rows[i]["colorText"].ToString()));
				row.ColorBackG=Color.FromArgb(PIn.Int(_tablePlannedAll.Rows[i]["colorBackG"].ToString()));
				row.Tag=PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString());
				gridPlanned.Rows.Add(row);
			}
			gridPlanned.EndUpdate();
			for(int i=0;i<_listPlannedAppt.Count;i++) {
				if(_listSelectedAptNums.Contains(PIn.Long(_listPlannedAppt[i]["AptNum"].ToString()))) {
					gridPlanned.SetSelected(i,true);
				}
			}
		}

		private void butNew_Click(object sender, System.EventArgs e) {
			/*if(ApptPlanned.Visible){
				if(MessageBox.Show(Lan.g(this,"Replace existing planned appointment?")
					,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
					return;
				//Procedures.UnattachProcsInPlannedAppt(ApptPlanned.Info.MyApt.AptNum);
				AppointmentL.Delete(PIn.PInt(ApptPlanned.DataRoww["AptNum"].ToString()));
			}*/
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			Appointment AptCur=new Appointment();
			AptCur.PatNum=PatCur.PatNum;
			AptCur.ProvNum=PatCur.PriProv;
			AptCur.ClinicNum=PatCur.ClinicNum;
			AptCur.AptStatus=ApptStatus.Planned;
			AptCur.AptDateTime=DateTimeOD.Today;
			AptCur.Pattern="/X/";
			AptCur.TimeLocked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			Appointments.Insert(AptCur);
			PlannedAppt plannedAppt=new PlannedAppt();
			plannedAppt.AptNum=AptCur.AptNum;
			plannedAppt.PatNum=PatCur.PatNum;
			plannedAppt.ItemOrder=DataSetMain.Tables["Planned"].Rows.Count+1;
			PlannedAppts.Insert(plannedAppt);
			FormApptEdit FormApptEdit2=new FormApptEdit(AptCur.AptNum);
			FormApptEdit2.IsNew=true;
			FormApptEdit2.ShowDialog();
			if(FormApptEdit2.DialogResult!=DialogResult.OK){
				//delete new appt, delete plannedappt, and unattach procs already handled in dialog
				FillPlanned();
				return;
			}
			List<Procedure> myProcList=Procedures.Refresh(PatCur.PatNum);
			bool allProcsHyg=true;
			for(int i=0;i<myProcList.Count;i++){
				if(myProcList[i].PlannedAptNum!=AptCur.AptNum)
					continue;//only concerned with procs on this plannedAppt
				if(!ProcedureCodes.GetProcCode(myProcList[i].CodeNum).IsHygiene){
					allProcsHyg=false;
					break;
				}
			}
			if(allProcsHyg && PatCur.SecProv!=0){
				Appointment aptOld=AptCur.Clone();
				AptCur.ProvNum=PatCur.SecProv;
				Appointments.Update(AptCur,aptOld);
			}
			Patient patOld=PatCur.Copy();
			//PatCur.NextAptNum=AptCur.AptNum;
			PatCur.PlannedIsDone=false;
			Patients.Update(PatCur,patOld);
			ModuleSelected(PatCur.PatNum);//if procs were added in appt, then this will display them
		}

		///<summary>This is the listener for the Delete button.</summary>
		private void butClear_Click(object sender, System.EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select an item first");
				return;
			}
			if(!MsgBox.Show(this,true,"Delete planned appointment(s)?")){
				return;
			}
			for(int i=0;i<gridPlanned.SelectedIndices.Length;i++){			
				Appointments.Delete(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptNum"].ToString()));
			}
			ModuleSelected(PatCur.PatNum);
		}

		///<summary></summary>
		private void butPin_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select an item first");
				return;
			}
			List<long> aptNums=new List<long>();
			for(int i=0;i<gridPlanned.SelectedIndices.Length;i++){
				ApptStatus aptStatus=(ApptStatus)(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptStatus"].ToString()));
				if(aptStatus==ApptStatus.Complete) {
					//Warn the user they are moving a completed appointment.
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to move an already completed appointment.  Continue?")) {
						return;
					}
					aptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString()));
				}
				else if(aptStatus==ApptStatus.Scheduled || aptStatus==ApptStatus.ASAP) {
					//Warn the user they are moving an already scheduled appointment.
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to move an appointment already on the schedule.  Continue?")) {
						return;
					}
					aptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString()));
				}
				else if(aptStatus==ApptStatus.UnschedList || aptStatus==ApptStatus.Broken) {
					//Dont need to warn user, just put onto the pinboard.
					aptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString()));
				}
				else { //No appointment
					aptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptNum"].ToString()));
				}
			}
			GotoModule.PinToAppt(aptNums,0);
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(gridPlanned.SelectedIndices.Length>1) {
				MsgBox.Show(this,"Please only select one item first.");
				return;
			}
			DataTable table=DataSetMain.Tables["Planned"];
			int idx=gridPlanned.SelectedIndices[0];
			if(idx==0) {
				return;
			}
			DataRow selectedApptRow=_listPlannedAppt[idx];//Get selected data row
			//Get data row above the selected, since we are moving up we are going to need it to adjust its item order
			DataRow aboveSelectedApptRow=_listPlannedAppt[idx-1];//idx guaranteed to be >0
			moveItemOrderHelper(selectedApptRow,PIn.Int(aboveSelectedApptRow["ItemOrder"].ToString()));//Sets the selected rows item order = the above rows and adjust everything inbetween
			saveChangesToDBHelper();//Loops through list, gets PlannedAppt, sets the new ItemOrder and then updates if needed
			DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked);
			_listSelectedAptNums.Clear();
			FillPlanned();
			gridPlanned.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(gridPlanned.SelectedIndices.Length>1) {
				MsgBox.Show(this,"Please only select one item first.");
				return;
			}
			DataTable table=DataSetMain.Tables["Planned"];
			int idx=gridPlanned.SelectedIndices[0];
			if(idx==_listPlannedAppt.Count-1) {
				return;
			}
			DataRow selectedApptRow=_listPlannedAppt[idx];//Get selected data row
			DataRow belowSelectedApptRow=_listPlannedAppt[idx+1];//Get data row below the selected, since we are moving down we are going to need it to adjust its item order
			moveItemOrderHelper(selectedApptRow,PIn.Int(belowSelectedApptRow["ItemOrder"].ToString()));//Sets the selected rows item order = the above rows and adjust everything inbetween
			saveChangesToDBHelper();//Loops through list, gets PlannedAppt, sets the new ItemOrder and then updates if needed
			DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked);
			_listSelectedAptNums.Clear();
			FillPlanned();
			gridPlanned.SetSelected(idx+1,true);
		}

		private void gridPlanned_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			long aptnum=(long)gridPlanned.Rows[e.Row].Tag;
			FormApptEdit FormAE=new FormApptEdit(aptnum);
			FormAE.ShowDialog();
			if(Programs.UsingOrion) {
				if(FormAE.DialogResult==DialogResult.OK) {
					ModuleSelected(PatCur.PatNum);//if procs were added in appt, then this will display them*/
				}
			}
			else {
				ModuleSelected(PatCur.PatNum);//if procs were added in appt, then this will display them*/
			}
			gridPlanned.SetSelected(false);
			for(int i=0;i<gridPlanned.Rows.Count;i++) {
				if((long)gridPlanned.Rows[i].Tag==aptnum) {
					gridPlanned.SetSelected(i,true);
				}
			}
		}

		private void checkDone_Click(object sender, System.EventArgs e) {
			Patient oldPat=PatCur.Copy();
			if(checkDone.Checked){
				if(_tablePlannedAll.Rows.Count>0) {
					if(!MsgBox.Show(this,true,"ALL planned appointment(s) will be deleted. Continue?")){
						checkDone.Checked=false;
						return; 
					}
					for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
						Appointments.Delete(PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString()));
					}
				}
				PatCur.PlannedIsDone=true;
				Patients.Update(PatCur,oldPat);
			}
			else{
				PatCur.PlannedIsDone=false;
				Patients.Update(PatCur,oldPat);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void checkShowCompleted_CheckedChanged(object sender,EventArgs e) {
			_listSelectedAptNums.Clear();
			foreach(int index in gridPlanned.SelectedIndices) {
				_listSelectedAptNums.Add(PIn.Long(_listPlannedAppt[index]["AptNum"].ToString()));
			}
			FillPlanned();
		}

		///<summary>Sets item orders appropriately. Does not reorder list, and does not repaint/refill grid.</summary>
		private void moveItemOrderHelper(DataRow plannedAppt,int newItemOrder) {
			int plannedApptItemOrder=PIn.Int(plannedAppt["ItemOrder"].ToString());
			if(plannedApptItemOrder>newItemOrder) {//moving item up, itterate down through list
				for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
					int itemOrderCur=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					if(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()==plannedAppt["PlannedApptNum"].ToString()) {
						_tablePlannedAll.Rows[i]["ItemOrder"]=newItemOrder;//set item order of this PlannedAppt.
						continue;
					}
					if(itemOrderCur>=newItemOrder && itemOrderCur<plannedApptItemOrder) {//all items between newItemOrder and oldItemOrder
						_tablePlannedAll.Rows[i]["ItemOrder"]=itemOrderCur+1;
					}
				}
			}
			else {//moving item down, itterate up through list
				for(int i=_tablePlannedAll.Rows.Count-1;i>=0;i--) {
					int itemOrderCur=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					if(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()==plannedAppt["PlannedApptNum"].ToString()) {
						_tablePlannedAll.Rows[i]["ItemOrder"]=newItemOrder;//set item order of this PlannedAppt.
						continue;
					}
					if(itemOrderCur<=newItemOrder && itemOrderCur>plannedApptItemOrder) {//all items between newItemOrder and oldItemOrder
						_tablePlannedAll.Rows[i]["ItemOrder"]=itemOrderCur-1;
					}
				}
			}
			//tablePlannedAll has correct itemOrder values, which we need to copy to _listPlannedAppt without changing the actual order of _listPlannedAppt.
			for(int i=0;i<_listPlannedAppt.Count;i++) {
				for(int j=0;j<_tablePlannedAll.Rows.Count;j++) {
					if(_listPlannedAppt[i]["PlannedApptNum"].ToString()!=_tablePlannedAll.Rows[j]["PlannedApptNum"].ToString()) {
						continue;
					}
					_listPlannedAppt[i]=_tablePlannedAll.Rows[j];//update order.
				}
			}
		}

		///<summary>Updates database based on the values in _tablePlannedAll.Rows.</summary>
		private void saveChangesToDBHelper() {
			//Get all PlannedAppts from db to check for changes
			List<PlannedAppt> listPlannedAllDB=PlannedAppts.Refresh(PatCur.PatNum);
			//Itterate through current PlannedAppts list in memory and compare to db list
			for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
				//find db version of PlannedAppt to update
				PlannedAppt oldPlannedAppt=null;
				PlannedAppt plannedAppt=null;
				for(int j=0;j<listPlannedAllDB.Count;j++) {
					if(PIn.Long(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString())!=listPlannedAllDB[j].PlannedApptNum) {
						continue;//not the correct PlannedAppt
					}
					//found the correct PlannedAppt
					oldPlannedAppt=PlannedAppts.GetOne(PIn.Long(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()));
					plannedAppt=oldPlannedAppt.Copy();
					plannedAppt.ItemOrder=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					break;//found match
				}
				if(plannedAppt==null) {//should never happen, this would mean a planned appt in our local list doesn't exist in the db
					continue;
				}
				PlannedAppts.Update(plannedAppt,oldPlannedAppt);
			}
		}
		#endregion

		#region Show
		private void button1_Click(object sender, System.EventArgs e) {
			//sometimes used for testing purposes
		}

		private void checkShowTP_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowC_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowE_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowR_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowCn_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkNotes_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkAppt_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkComm_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkCommFamily_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkLabCase_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkRx_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkTasks_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkEmail_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkSheets_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowTeeth_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			if(checkShowTeeth.Checked) {
				checkShowTP.Checked=true;
				checkShowC.Checked=true;
				checkShowE.Checked=true;
				checkShowR.Checked=true;
				checkShowCn.Checked=true;
				checkNotes.Checked=true;
				checkAppt.Checked=false;
				checkComm.Checked=false;
				checkCommFamily.Checked=false;
				checkLabCase.Checked=false;
				checkRx.Checked=false;
				checkEmail.Checked=false;
				checkTasks.Checked=false;
				checkSheets.Checked=false;
			}
			else {
				checkShowTP.Checked=true;
				checkShowC.Checked=true;
				checkShowE.Checked=true;
				checkShowR.Checked=true;
				checkShowCn.Checked=true;
				checkNotes.Checked=true;
				checkAppt.Checked=true;
				checkComm.Checked=true;
				checkCommFamily.Checked=true;
				checkLabCase.Checked=true;
				checkRx.Checked=true;
				checkEmail.Checked=true;
				checkTasks.Checked=true;
				checkSheets.Checked=true;
			}
			FillProgNotes(true);
		}

		private void checkAudit_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void butShowAll_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			checkShowTP.Checked=true;
			checkShowC.Checked=true;
			checkShowE.Checked=true;
			checkShowR.Checked=true;
			checkShowCn.Checked=true;
			checkNotes.Checked=true;
			checkAppt.Checked=true;
			checkComm.Checked=true;
			checkCommFamily.Checked=true;
			checkLabCase.Checked=true;
			checkRx.Checked=true;
			checkShowTeeth.Checked=false;
			checkTasks.Checked=true;
			checkEmail.Checked=true;
			checkSheets.Checked=true;
			FillProgNotes();
		}

		private void butShowNone_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			checkShowTP.Checked=false;
			checkShowC.Checked=false;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkShowCn.Checked=false;
			checkNotes.Checked=false;
			checkAppt.Checked=false;
			checkComm.Checked=false;
			checkCommFamily.Checked=false;
			checkLabCase.Checked=false;
			checkRx.Checked=false;
			checkShowTeeth.Checked=false;
			checkTasks.Checked=false;
			checkEmail.Checked=false;
			checkSheets.Checked=false;
			FillProgNotes();
		}
		#endregion Show

		#region Draw
		private void radioPointer_Click(object sender,EventArgs e) {
			toothChart.CursorTool=CursorTool.Pointer;
		}

		private void radioPen_Click(object sender,EventArgs e) {
			toothChart.CursorTool=CursorTool.Pen;
		}

		private void radioEraser_Click(object sender,EventArgs e) {
			toothChart.CursorTool=CursorTool.Eraser;
		}

		private void radioColorChanger_Click(object sender,EventArgs e) {
			toothChart.CursorTool=CursorTool.ColorChanger;
		}

		private void panelDrawColor_DoubleClick(object sender,EventArgs e) {
			//do nothing
		}

		private void toothChart_SegmentDrawn(object sender,ToothChartDrawEventArgs e) {
			if(radioPen.Checked){
				ToothInitial ti=new ToothInitial();
				ti.DrawingSegment=e.DrawingSegement;
				ti.InitialType=ToothInitialType.Drawing;
				ti.PatNum=PatCur.PatNum;
				ti.ColorDraw=panelDrawColor.BackColor;
				ToothInitials.Insert(ti);
				ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
				FillToothChart(true);
			}
			else if(radioEraser.Checked){
				//for(int i=0;i<ToothInitialList.Count;i++){
				for(int i=ToothInitialList.Count-1;i>=0;i--) {//go backwards
					if(ToothInitialList[i].InitialType!=ToothInitialType.Drawing) {
						continue;
					}
					if(ToothInitialList[i].DrawingSegment!=e.DrawingSegement) {
						continue;
					}
					ToothInitials.Delete(ToothInitialList[i]);
					ToothInitialList.RemoveAt(i);
					//no need to refresh since that's handled by the toothchart.
				}
			}
			else if(radioColorChanger.Checked){
				for(int i=0;i<ToothInitialList.Count;i++){
					if(ToothInitialList[i].InitialType!=ToothInitialType.Drawing){
						continue;
					}
					if(ToothInitialList[i].DrawingSegment!=e.DrawingSegement){
						continue;
					}
					ToothInitialList[i].ColorDraw=panelDrawColor.BackColor;
					ToothInitials.Update(ToothInitialList[i]);
					FillToothChart(true);
				}
			}
		}

		private void panelTPdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelTPdark.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelTPlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelTPlight.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelCdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelCdark.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelClight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelClight.BackColor;
		}

		private void panelECdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelECdark.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEClight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEClight.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEOdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEOdark.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEOlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEOlight.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelRdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelRdark.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelRlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelRlight.BackColor;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelBlack_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=Color.Black;
			toothChart.ColorDrawing=Color.Black;
		}

		private void butColorOther_Click(object sender,EventArgs e) {
			ColorDialog cd=new ColorDialog();
			cd.Color=butColorOther.BackColor;
			if(cd.ShowDialog()!=DialogResult.OK){
				return;
			}
			panelDrawColor.BackColor=cd.Color;
			toothChart.ColorDrawing=panelDrawColor.BackColor;
		}
		#endregion Draw

		private void listProcStatusCodes_MouseUp(object sender,MouseEventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void gridPtInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(Plugins.HookMethod(this,"ContrChart.gridPtInfo_CellDoubleClick",PatCur,FamCur,e,PatientNoteCur)) {
				return;
			}
			if(TerminalActives.PatIsInUse(PatCur.PatNum)) {
				MsgBox.Show(this,"Patient is currently entering info at a reception terminal.  Please try again later.");
				return;
			}
			if(gridPtInfo.Rows[e.Row].Tag!=null) {
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="med") {
					FormMedical FormM=new FormMedical(PatientNoteCur,PatCur);
					FormM.ShowDialog();
					ModuleSelected(PatCur.PatNum);
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.GetType()==typeof(RegistrationKey)) {
					FormRegistrationKeyEdit FormR=new FormRegistrationKeyEdit();
					FormR.RegKey=(RegistrationKey)gridPtInfo.Rows[e.Row].Tag;
					FormR.ShowDialog();
					FillPtInfo();
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="EhrProvKeys") {
					FormEhrProvKeysCustomer FormPK=new FormEhrProvKeysCustomer();
					FormPK.Guarantor=PatCur.Guarantor;
					FormPK.ShowDialog();
					ModuleSelected(PatCur.PatNum);
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Referral") {
					//RefAttach refattach=(RefAttach)gridPat.Rows[e.Row].Tag;
					FormReferralsPatient FormRE=new FormReferralsPatient();
					FormRE.PatNum=PatCur.PatNum;
					FormRE.ShowDialog();
					ModuleSelected(PatCur.PatNum);
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="References") {
					FormReference FormR=new FormReference();
					FormR.ShowDialog();
					if(FormR.GotoPatNum!=0) {
						Patient pat=Patients.GetPat(FormR.GotoPatNum);
						OnPatientSelected(pat);
						GotoModule.GotoFamily(FormR.GotoPatNum);
						return;
					}
					if(FormR.DialogResult!=DialogResult.OK) {
						return;
					}
					for(int i=0;i<FormR.SelectedCustRefs.Count;i++) {
						CustRefEntry custEntry=new CustRefEntry();
						custEntry.DateEntry=DateTime.Now;
						custEntry.PatNumCust=PatCur.PatNum;
						custEntry.PatNumRef=FormR.SelectedCustRefs[i].PatNum;
						CustRefEntries.Insert(custEntry);
					}
					FillPtInfo();
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Patient Portal") {
					FormPatientPortal FormPP=new FormPatientPortal();
					FormPP.PatCur=PatCur;
					FormPP.ShowDialog();
					if(FormPP.DialogResult==DialogResult.OK) {
						FillPtInfo();
					}
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Payor Types") {
					FormPayorTypes FormPT=new FormPayorTypes();
					FormPT.PatCur=PatCur;
					FormPT.ShowDialog();
					if(FormPT.DialogResult==DialogResult.OK) {
						FillPtInfo();
					}
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.GetType()==typeof(CustRefEntry)) {
					FormReferenceEntryEdit FormRE=new FormReferenceEntryEdit((CustRefEntry)gridPtInfo.Rows[e.Row].Tag);
					FormRE.ShowDialog();
					FillPtInfo();
					return;
				}
				else {//patfield
					string tag=gridPtInfo.Rows[e.Row].Tag.ToString();
					tag=tag.Substring(8);//strips off all but the number: PatField1
					int index=PIn.Int(tag);
					PatField field=PatFields.GetByName(PatFieldDefs.ListShort[index].FieldName,PatFieldList);
					if(field==null) {
						field=new PatField();
						field.PatNum=PatCur.PatNum;
						field.FieldName=PatFieldDefs.ListShort[index].FieldName;
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Text) {
							FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
							FormPF.IsNew=true;
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.PickList) {
							FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(field);
							FormPF.IsNew=true;
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Date) {
							FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(field);
							FormPF.IsNew=true;
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Checkbox) {
							FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(field);
							FormPF.IsNew=true;
							FormPF.ShowDialog();
						}
					}
					else {
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Text) {
							FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.PickList) {
							FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(field);
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Date) {
							FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(field);
							FormPF.ShowDialog();
						}
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Checkbox) {
							FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(field);
							FormPF.ShowDialog();
						}
					}
				}
			}
			else {
				string email=PatCur.Email;
				long siteNum=PatCur.SiteNum;
				FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
				FormP.IsNew=false;
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.OK) {
					OnPatientSelected(PatCur);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void toothChart_Click(object sender,EventArgs e) {
			textSurf.Text="";
			//if(toothChart.SelectedTeeth.Length==1) {
				//butO.BackColor=SystemColors.Control;
				//butB.BackColor=SystemColors.Control;
				//butF.BackColor=SystemColors.Control;
				//if(Tooth.IsAnterior(toothChart.SelectedTeeth[0])) {
					//butB.Text="";
					//butO.Text="";
					//butB.Enabled=false;
					//butO.Enabled=false;
					//butF.Text="F";
					//butI.Text="I";
					//butF.Enabled=true;
					//butI.Enabled=true;
				//}
				//else {
					//butB.Text="B";
					//butO.Text="O";
					//butB.Enabled=true;
					//butO.Enabled=true;
					//butF.Text="";
					//butI.Text="";
					//butF.Enabled=false;
					//butI.Enabled=false;
				//}
			//}
			if(checkShowTeeth.Checked) {
				FillProgNotes();
			}
			FillMovementsAndHidden();
		}

		private void gridProg_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				if(PrefC.GetBool(PrefName.EasyHideHospitals)){
					menuItemPrintDay.Visible=false;
				}
				else{
					menuItemPrintDay.Visible=true;
				}
				Plugins.HookAddCode(this,"ContrChart.gridProg_MouseUp_end",menuProgRight,gridProg,PatCur);
				menuProgRight.Show(gridProg,new Point(e.X,e.Y));
			}
		}

		private void menuItemPrintProg_Click(object sender, System.EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			#if DEBUG
				PrintReport(true);
			#else
				PrintReport(false);	
			#endif
		}

		private void menuItemPrintDay_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one item first.");
				return;
			}
			DataRow row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			//hospitalDate=PIn.DateT(row["ProcDate"].ToString());
			//Store the state of all checkboxes in temporary variables
			bool showRx=this.checkRx.Checked;
			bool showComm=this.checkComm.Checked;
			bool showApt=this.checkAppt.Checked;
			bool showEmail=this.checkEmail.Checked;
			bool showTask=this.checkTasks.Checked;
			bool showLab=this.checkLabCase.Checked;
			bool showSheets=this.checkSheets.Checked;
			bool showTeeth=this.checkShowTeeth.Checked;
			bool showAudit=this.checkAudit.Checked;
			DateTime showDateStart=ShowDateStart;
			DateTime showDateEnd=ShowDateEnd;
			bool showTP=this.checkShowTP.Checked;
			bool showComplete=this.checkShowC.Checked;
			bool showExist=this.checkShowE.Checked;
			bool showRefer=this.checkShowR.Checked;
			bool showCond=this.checkShowCn.Checked;
			bool showProcNote=this.checkNotes.Checked;
			bool customView=this.chartCustViewChanged;
			//Set the checkboxes to desired values for print out
			checkRx.Checked=false;
			checkComm.Checked=false;
			checkAppt.Checked=false;
			checkEmail.Checked=false;
			checkTasks.Checked=false;
			checkLabCase.Checked=false;
			checkSheets.Checked=false;
			checkShowTeeth.Checked=false;
			checkAudit.Checked=false;
			ShowDateStart=PIn.DateT(row["ProcDate"].ToString());
			ShowDateEnd=PIn.DateT(row["ProcDate"].ToString());
			checkShowTP.Checked=false;
			checkShowC.Checked=true;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkShowCn.Checked=false;
			checkNotes.Checked=true;
			chartCustViewChanged=true;//custom view will not reset the check boxes so we force it true.
			//Fill progress notes with only desired rows to be printed, then print.
			FillProgNotes();
			if(gridProg.Rows.Count==0) {
				MsgBox.Show(this,"No completed procedures or notes to print");
			}
			else{
				try {
					pagesPrinted=0;
					headingPrinted=false;
					#if DEBUG
						PrintDay(true);
					#else
						PrintDay(false);
					#endif
				}
				catch {

				}
			}
			//Set Date values and checkboxes back to original values, then refill progress notes.
			//hospitalDate=DateTime.MinValue;
			checkRx.Checked=showRx;
			checkComm.Checked=showComm;
			checkAppt.Checked=showApt;
			checkEmail.Checked=showEmail;
			checkTasks.Checked=showTask;
			checkLabCase.Checked=showLab;
			checkSheets.Checked=showSheets;
			checkShowTeeth.Checked=showTeeth;
			checkAudit.Checked=showAudit;
			ShowDateStart=showDateStart;
			ShowDateEnd=showDateEnd;
			checkShowTP.Checked=showTP;
			checkShowC.Checked=showComplete;
			checkShowE.Checked=showExist;
			checkShowR.Checked=showRefer;
			checkShowCn.Checked=showCond;
			checkNotes.Checked=showProcNote;
			chartCustViewChanged=customView;
			FillProgNotes();
		}

		private void menuItemDelete_Click(object sender,EventArgs e) {
			DeleteRows();
		}

		private void menuItemSetComplete_Click(object sender,EventArgs e) {
			//moved down so we can have the date first
			//if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
			//	return;
			//}
			if(gridProg.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(checkAudit.Checked) {
				MsgBox.Show(this,"Not allowed in audit mode.");
				return;
			}
			DataRow row;
			Appointment apt;
			//One appointment-------------------------------------------------------------------------------------------------------------
			if(gridProg.SelectedIndices.Length==1
				&& ((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["AptNum"].ToString()!="0")
			{
				if(!Security.IsAuthorized(Permissions.AppointmentEdit)){
					return;
				}
				apt=Appointments.GetOneApt(PIn.Long(((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["AptNum"].ToString()));
				if(apt.AptStatus == ApptStatus.Complete) {
					MsgBox.Show(this,"Already complete.");
					return;
				}
				if(apt.AptStatus == ApptStatus.PtNote
					|| apt.AptStatus == ApptStatus.PtNoteCompleted
					|| apt.AptStatus == ApptStatus.Planned
					|| apt.AptStatus==ApptStatus.UnschedList)
				{
					MsgBox.Show(this,"Not allowed for that status.");
					return;
				}
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime)) {
					return;
				}
				if(apt.AptDateTime.Date>DateTime.Today) {
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Appointment is in the future.  Set complete anyway?")){
						return;
					}
				}
				else if(!MsgBox.Show(this,true,"Set appointment complete?")){
					return;
				}
				InsSub sub1=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,PlanList,SubList)),SubList);
				InsSub sub2=InsSubs.GetSub(PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,PlanList,SubList)),SubList);
				Appointments.SetAptStatusComplete(apt.AptNum,sub1.PlanNum,sub2.PlanNum);
				ProcedureL.SetCompleteInAppt(apt,PlanList,PatPlanList,PatCur.SiteNum,PatCur.Age,SubList);//loops through each proc, also makes completed security logs
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit, apt.PatNum,
					apt.ProcDescript+", "+apt.AptDateTime.ToString()+", Set Complete",
					apt.AptNum);
				//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					//S14 - Appt Modification event
					MessageHL7 messageHL7=MessageConstructor.GenerateSIU(PatCur,FamCur.GetPatient(PatCur.Guarantor),EventTypeHL7.S14,apt);
					//Will be null if there is no outbound SIU message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=apt.AptNum;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=PatCur.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
				Recalls.Synch(PatCur.PatNum);
				Recalls.SynchScheduledApptFull(PatCur.PatNum);
				ModuleSelected(PatCur.PatNum);
				return;
			}
			//One task
			if(gridProg.SelectedIndices.Length==1
				&& ((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["TaskNum"].ToString()!="0")
			{
				if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"The selected task will be marked Done and will affect all users.")) {
					long taskNum=PIn.Long(((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["TaskNum"].ToString());
					Task taskCur=Tasks.GetOne(taskNum);
					Task taskOld=taskCur.Copy();
					taskCur.TaskStatus=TaskStatusEnum.Done;//global even if new status is tracked by user
					if(taskOld.TaskStatus!=TaskStatusEnum.Done) {
						taskCur.DateTimeFinished=DateTime.Now;
					}
					TaskUnreads.DeleteForTask(taskCur.TaskNum);//clear out taskunreads. We have too many tasks to read the done ones.
					Tasks.Update(taskCur,taskOld);
					TaskHist taskHist=new TaskHist(taskOld);
					taskHist.UserNumHist=Security.CurUser.UserNum;
					TaskHists.Insert(taskHist);
					ModuleSelected(PatCur.PatNum);
					return;
				}
			}
			//Multiple procedures------------------------------------------------------------------------------------------------------
			if(!PrefC.GetBool(PrefName.AllowSettingProcsComplete)){
				MsgBox.Show(this,"Only single appointments and tasks may be set complete.  If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
				return;
			}
			//check to make sure we don't have non-procedures
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()=="0" || row["ProcCode"].ToString()=="~GRP~") {
					MsgBox.Show(this,"Only procedures, single appointments, or tasks may be set complete.");
					return;
				}
			}
			Procedure procCur;
			Procedure procOld;
			ProcedureCode procCode;
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			//this loop is just for security:
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				procCur=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				if(procCur.ProcStatus==ProcStat.C){
					continue;//because it will be skipped below anyway
				}
				if(procCur.AptNum!=0) {//if attached to an appointment
					apt=Appointments.GetOneApt(procCur.AptNum);
					if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date) {
						MessageBox.Show(Lan.g(this,"Not allowed because a procedure is attached to a future appointment with a date of ")+apt.AptDateTime.ToShortDateString());
						return;
					}
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime)) {
						return;
					}
				}
				else{
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
						return;
					}
				}
			}
			List<string> procCodeList=new List<string>();//for automation
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				apt=null;
				procCur=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				if(procCur.ProcStatus==ProcStat.C) {
					continue;//don't allow setting a procedure complete again.  Important for security reasons.
				}
				procOld=procCur.Copy();
				procCode=ProcedureCodes.GetProcCode(procCur.CodeNum);
				procCodeList.Add(ProcedureCodes.GetStringProcCode(procCur.CodeNum));
				if(procOld.ProcStatus!=ProcStat.C) {
					//if procedure was already complete, then don't add more notes.
					procCur.Note+=ProcCodeNotes.GetNote(procCur.ProvNum,procCur.CodeNum);//note wasn't complete, so add notes
				}
				procCur.DateEntryC=DateTime.Now;
				if(procCur.AptNum!=0) {//if attached to an appointment
					apt=Appointments.GetOneApt(procCur.AptNum);
					procCur.ProcDate=apt.AptDateTime;
					procCur.ClinicNum=apt.ClinicNum;
					procCur.PlaceService=Clinics.GetPlaceService(apt.ClinicNum);
				}
				else {
					procCur.ProcDate=PIn.Date(textDate.Text);
					procCur.PlaceService=(PlaceOfService)PrefC.GetLong(PrefName.DefaultProcedurePlaceService);
				}
				if(procCur.ProcDate.Year<1880) {
					procCur.ProcDate=MiscData.GetNowDateTime();
				}
				procCur.SiteNum=PatCur.SiteNum;
				Procedures.SetDateFirstVisit(procCur.ProcDate,2,PatCur);
				if(procCode.PaintType==ToothPaintingType.Extraction) {//if an extraction, then mark previous procs hidden
					//Procedures.SetHideGraphical(procCur);//might not matter anymore
					ToothInitials.SetValue(PatCur.PatNum,procCur.ToothNum,ToothInitialType.Missing);
				}
				procCur.ProcStatus=ProcStat.C;
				if(procCur.DiagnosticCode=="") {
					procCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canada
					Procedures.SetCanadianLabFeesCompleteForProc(procCur);
				}
				Plugins.HookAddCode(this,"ContrChart.menuItemSetComplete_Click_procLoop",procCur,procOld);
				Procedures.Update(procCur,procOld);
				ProcedureL.LogProcComplCreate(PatCur.PatNum,procCur,procCur.ToothNum);
				//Tried to move it to the business layer, but too complex for now.
				//Procedures.SetComplete(
				//	((Procedure)gridProg.Rows[gridProg.SelectedIndices[i]].Tag).ProcNum,PIn.PDate(textDate.Text));
				Procedures.ComputeEstimates(procCur,procCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
				//Auto insert encounter. Have to run this every time because entries might not all be on the same date or provider.
				if(procOld.ProcStatus!=ProcStat.C && procCur.ProcStatus==ProcStat.C) {
					Encounters.InsertDefaultEncounter(procCur.PatNum,procCur.ProvNum,procCur.ProcDate);
				}
			}
			AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodeList,PatCur.PatNum);
			Recalls.Synch(PatCur.PatNum);
			//if(skipped>0){
			//	MessageBox.Show(Lan.g(this,".")+"\r\n"
			//		+skipped.ToString()+" "+Lan.g(this,"procedures(s) skipped."));
			//}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemEditSelected_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select procedures first.");
				return;
			}
			DataRow row;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()=="0") {
					MsgBox.Show(this,"Only procedures may be edited.");
					return;
				}
			}
			List<Procedure> proclist=new List<Procedure>();
			Procedure proc;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				proc=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false);
				proclist.Add(proc);
			}
			FormProcEditAll FormP=new FormProcEditAll();
			FormP.ProcList=proclist;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK){
				ModuleSelected(PatCur.PatNum);
			}
		}

		private void menuItemGroupSelected_Click(object sender,EventArgs e){
			DataRow row;
			if(gridProg.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select procedures to attach a group note to.");
				return;
			}
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()=="0") { //This is not a procedure.
					MsgBox.Show(this,"You may only attach a group note to procedures.");
					return;
				}
			}
			List<Procedure> proclist=new List<Procedure>();
			Procedure proc;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){//Create proclist from selected items.
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				proc=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				proclist.Add(proc);
			}
			//Validate the list of procedures------------------------------------------------------------------------------------
			DateTime procDate=proclist[0].ProcDate;
			long clinicNum=proclist[0].ClinicNum;
			long provNum=proclist[0].ProvNum;
			for(int i=0;i<proclist.Count;i++){//starts at 0 to check procStatus
				if(ProcedureCodes.GetStringProcCode(proclist[i].CodeNum)==ProcedureCodes.GroupProcCode){
					MsgBox.Show(this,"You cannot attach a group note to another group note.");
					return;
				}
				if(proclist[i].IsLocked) {
					MsgBox.Show(this,"Procedures cannot be locked.");
					return;
				}
				if(proclist[i].ProcDate!=procDate){
					MsgBox.Show(this,"Procedures must have the same date to attach a group note.");
					return;
				}
				if(proclist[i].ProcStatus!=ProcStat.C){
					MsgBox.Show(this,"Procedures must be complete to attach a group note.");
					return;
				}
				if(proclist[i].ClinicNum!=clinicNum){
					MsgBox.Show(this,"Procedures must have the same clinic to attach a group note.");
					return;
				}
				if(proclist[i].ProvNum!=provNum){
					MsgBox.Show(this,"Procedures must have the same provider to attach a group note.");
					return;
				}
			}
			//Procedures are valid. Create new Procedure "group" and ProcGroupItems-------------------------------------------------------
			Procedure group=new Procedure();
			group.PatNum=PatCur.PatNum;
			group.ProcStatus=ProcStat.EC;
			group.DateEntryC=DateTime.Now;
			group.ProcDate=procDate;
			group.ProvNum=provNum;
			group.ClinicNum=clinicNum;
			group.CodeNum=ProcedureCodes.GetCodeNum(ProcedureCodes.GroupProcCode);
			if(PrefC.GetBool(PrefName.ProcGroupNoteDoesAggregate)) {
				string aggNote="";
				for(int i=0;i<proclist.Count;i++) {
					if(i>0 && proclist[i-1].Note!="") {
						aggNote+="\r\n";
					}
					aggNote+=proclist[i].Note;
				}
				group.Note=aggNote;
			}
			else {
				group.Note=ProcCodeNotes.GetNote(group.ProvNum,group.CodeNum);
			}
			group.IsNew=true;
			Procedures.Insert(group);
			List<ProcGroupItem> groupItemList=new List<ProcGroupItem>();
			ProcGroupItem groupItem;
			for(int i=0;i<proclist.Count;i++){
				groupItem=new ProcGroupItem();
				groupItem.ProcNum=proclist[i].ProcNum;
				groupItem.GroupNum=group.ProcNum;
				ProcGroupItems.Insert(groupItem);
				groupItemList.Add(groupItem);
			}
			if(Programs.UsingOrion) {
				OrionProc op=new OrionProc();
				op.ProcNum=group.ProcNum;
				op.Status2=OrionStatus.C;
				OrionProcs.Insert(op);
			}
			FormProcGroup FormP=new FormProcGroup();
			FormP.GroupCur=group;
			FormP.GroupItemList=groupItemList;
			FormP.ProcList=proclist;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			if(PrefC.GetBool(PrefName.ProcGroupNoteDoesAggregate)) {
				//remove the notes from all the attached procs
				for(int i=0;i<proclist.Count;i++) {
					Procedure oldProc=proclist[i].Copy();
					Procedure changedProc=proclist[i].Copy();
					changedProc.Note="";
					Procedures.Update(changedProc,oldProc);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemLabFee_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length<2 || gridProg.SelectedIndices.Length>3) {
				MsgBox.Show(this,"Please select two or three procedures, one regular and the other one or two lab.");
				return;
			}
			//One check that is not made is whether a lab proc is already attached to a different proc.
			DataRow row1=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			DataRow row2=(DataRow)gridProg.Rows[gridProg.SelectedIndices[1]].Tag;
			DataRow row3=null;
			if(gridProg.SelectedIndices.Length==3) {
				row3=(DataRow)gridProg.Rows[gridProg.SelectedIndices[2]].Tag;
			}
			if(row1["ProcNum"].ToString()=="0" || row2["ProcNum"].ToString()=="0" || (row3!=null && row3["ProcNum"].ToString()=="0")) {
				MsgBox.Show(this,"All selected items must be procedures.");
				return;
			}
			List<long> procNumsReg=new List<long>();
			List<long> procNumsLab=new List<long>();
			if(ProcedureCodes.GetProcCode(row1["ProcCode"].ToString()).IsCanadianLab) {
				procNumsLab.Add(PIn.Long(row1["ProcNum"].ToString()));
			}
			else {
				procNumsReg.Add(PIn.Long(row1["ProcNum"].ToString()));
			}
			if(ProcedureCodes.GetProcCode(row2["ProcCode"].ToString()).IsCanadianLab) {
				procNumsLab.Add(PIn.Long(row2["ProcNum"].ToString()));
			}
			else {
				procNumsReg.Add(PIn.Long(row2["ProcNum"].ToString()));
			}
			if(row3!=null) {
				if(ProcedureCodes.GetProcCode(row3["ProcCode"].ToString()).IsCanadianLab) {
					procNumsLab.Add(PIn.Long(row3["ProcNum"].ToString()));
				}
				else {
					procNumsReg.Add(PIn.Long(row3["ProcNum"].ToString()));
				}
			}
			if(procNumsReg.Count==0) {
				MsgBox.Show(this,"One of the selected procedures must be a regular non-lab procedure as defined in Procedure Codes.");
				return;
			}
			if(procNumsReg.Count>1) {
				MsgBox.Show(this,"Only one of the selected procedures may be a regular non-lab procedure as defined in Procedure Codes.");
				return;
			}
			//We only alter the lab procedure(s), not the regular procedure.
			Procedure procLab;
			Procedure procOld;
			for(int i=0;i<procNumsLab.Count;i++) {
				procLab=Procedures.GetOneProc(procNumsLab[i],false);
				procOld=procLab.Copy();
				procLab.ProcNumLab=procNumsReg[0];
				Procedures.Update(procLab,procOld);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemLabFeeDetach_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one lab procedure first.");
				return;
			}
			DataRow row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			if(row["ProcNum"].ToString()=="0") {
				MsgBox.Show(this,"Please select a lab procedure first.");
				return;
			}
			if(row["ProcNumLab"].ToString()=="0") {
				MsgBox.Show(this,"The selected procedure is not attached as a lab procedure.");
				return;
			}
			Procedure procLab=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false);
			Procedure procOld=procLab.Copy();
			procLab.ProcNumLab=0;
			Procedures.Update(procLab,procOld);
			ModuleSelected(PatCur.PatNum);
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintReport(bool justPreview){
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			//pd2.DefaultPageSettings.Margins=new Margins(50,50,40,25);
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try{
				if(justPreview){
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();				
			  }
				else{
					if(PrinterL.SetPrinter(pd2,PrintSituation.Default,PatCur.PatNum,"Progress notes printed")){
						pd2.Print();
					}
				}
			}
			catch{
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(50,40,800,1000);//1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			#region printHeading
			text="Chart Progress Notes";//heading
			g.DrawString(text,headingFont,Brushes.Black,400-g.MeasureString(text,headingFont).Width/2,yPos);
			text=DateTime.Today.ToShortDateString();//date
			g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
			yPos+=(int)g.MeasureString(text,headingFont).Height;
			text=PatCur.GetNameFL();//name
			if(g.MeasureString(text,subHeadingFont).Width>700) {
				//extremely long name
				text=PatCur.GetNameFirst()[0]+". "+PatCur.LName;//example: J. Sparks
			}
			string[] headerText={ text };
			Plugins.HookAddCode(this,"ContrChart.pd2_PrintPage_middle",PatCur,e,g,headerText);
			text=headerText[0];
			g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
			text="Page "+(pagesPrinted+1);
			g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
			yPos+=30;
			headingPrinted=true;
			headingPrintH=yPos;
			#endregion
			yPos=gridProg.PrintPage(g,pagesPrinted,bounds,headingPrintH,true);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintDay(bool justPreview) {
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPageDay);
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.OriginAtMargins=true;
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try {
				if(justPreview) {
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();
				}
				else {
					if(PrinterL.SetPrinter(pd2,PrintSituation.Default,PatCur.PatNum,"Day report for hospital printed")) {
						pd2.Print();
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd2_PrintPageDay(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(50,40,800,1000);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text="Chart Progress Notes";
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				//practice
				text=PrefC.GetString(PrefName.PracticeTitle);
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					DataRow row;
					long procNum;
					long clinicNum;
					for(int i=0;i<gridProg.Rows.Count;i++) {
						row=(DataRow)gridProg.Rows[i].Tag;
						procNum=PIn.Long(row["ProcNum"].ToString());
						if(procNum==0) {
							continue;
						}
						clinicNum=Procedures.GetClinicNum(procNum);
						if(clinicNum!=0) {//The first clinicNum that's encountered
							text=Clinics.GetDesc(clinicNum);
							break;
						}
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				//name
				text=PatCur.GetNameFL();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Birthdate: "+PatCur.Birthdate.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Printed: "+DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Ward: "+PatCur.Ward;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				//Patient images are not shown when the A to Z folders are disabled.
				if(PrefC.AtoZfolderUsed){
					Bitmap picturePat;
					bool patientPictExists=Documents.GetPatPict(PatCur.PatNum,ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath()),out picturePat);
					if(picturePat!=null){//Successfully loaded a patient picture?
						Bitmap thumbnail=ImageHelper.GetThumbnail(picturePat,80);
						g.DrawImage(thumbnail,center-40,yPos);
					}
					if(patientPictExists){
						yPos+=80;
					}
					yPos+=30;
					headingPrinted=true;
					headingPrintH=yPos;
				}
			}
			#endregion
			yPos=gridProg.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1){
				e.HasMorePages=true;
			}
			else {
				g.DrawString("Signature_________________________________________________________",
								subHeadingFont,Brushes.Black,160,yPos+20);
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		///<summary>Draws one button for the tabControlImages.</summary>
		private void OnDrawItem(object sender, DrawItemEventArgs e){
      Graphics g=e.Graphics;
      Pen penBlue=new Pen(Color.FromArgb(97,136,173));
			Pen penRed=new Pen(Color.FromArgb(140,51,46));
			Pen penOrange=new Pen(Color.FromArgb(250,176,3),2);
			Pen penDkOrange=new Pen(Color.FromArgb(227,119,4));
			SolidBrush brBlack=new SolidBrush(Color.Black);
			int selected=tabControlImages.TabPages.IndexOf(tabControlImages.SelectedTab);
			Rectangle bounds=e.Bounds;
			Rectangle rect=new Rectangle(bounds.X+2,bounds.Y+1,bounds.Width-5,bounds.Height-4);
			if(e.Index==selected){
				g.FillRectangle(new SolidBrush(Color.White),rect);
				//g.DrawRectangle(penBlue,rect);
				g.DrawLine(penOrange,rect.X,rect.Bottom-1,rect.Right,rect.Bottom-1);
				g.DrawLine(penDkOrange,rect.X+1,rect.Bottom,rect.Right-2,rect.Bottom);
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X+3,bounds.Y+6);
			}
			else{
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X,bounds.Y);
			}
    }

		private void panelImages_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(e.Y>3){
				return;
			}
			MouseIsDownOnImageSplitter=true;
			ImageSplitterOriginalY=panelImages.Top;
			OriginalImageMousePos=panelImages.Top+e.Y;
		}

		private void panelImages_MouseLeave(object sender, System.EventArgs e) {
			//not needed.
		}

		private void panelImages_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnImageSplitter){
				if(e.Y<=3){
					panelImages.Cursor=Cursors.HSplit;
				}
				else{
					panelImages.Cursor=Cursors.Default;
				}
				return;
			}
			//panelNewTop
			int panelNewH=panelImages.Bottom
				-(ImageSplitterOriginalY+(panelImages.Top+e.Y)-OriginalImageMousePos);//-top
			if(panelNewH<10)//cTeeth.Bottom)
				panelNewH=10;//cTeeth.Bottom;//keeps it from going too low
			if(panelNewH>panelImages.Bottom-toothChart.Bottom)
				panelNewH=panelImages.Bottom-toothChart.Bottom;//keeps it from going too high
			panelImages.Height=panelNewH;
			if(UsingEcwTightOrFull()) {//this might belong in ChartLayoutHelper
				if(panelImages.Visible) {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1
						-(panelImages.Height+2);
				}
				else {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1;
				}
			}
		}

		private void panelImages_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnImageSplitter){
				return;
			}
			MouseIsDownOnImageSplitter=false;
		}

		private void tabProc_MouseDown(object sender,MouseEventArgs e) {
			ChartLayoutHelper.tabProc_MouseDown(SelectedProcTab,gridProg,tabProc,ClientSize,e);
			ChartLayoutHelper.SetTpChartingHelper((ChartViewCurDisplay!=null && ChartViewCurDisplay.IsTpCharting),PatCur,gridProg,listButtonCats,
				checkTreatPlans,panelTP,gridTreatPlans,gridTpProcs,butNewTP,listPriorities);
			SelectedProcTab=tabProc.SelectedIndex;
			FillMovementsAndHidden();
		}

		private void tabControlImages_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(selectedImageTab==-1){
				selectedImageTab=tabControlImages.SelectedIndex;
				return;
			}
			Rectangle rect=tabControlImages.GetTabRect(selectedImageTab);
			if(rect.Contains(e.X,e.Y)){//clicked on the already selected tab
				if(panelImages.Visible){
					panelImages.Visible=false;
				}
				else{
					panelImages.Visible=true;
				}
			}
			else{//clicked on a new tab
				if(!panelImages.Visible){
					panelImages.Visible=true;
				}
			}
			selectedImageTab=tabControlImages.SelectedIndex;
			FillImages();//it will not actually fill the images unless panelImages is visible
			if(UsingEcwTightOrFull()) {
				if(panelImages.Visible) {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1-(panelImages.Height+2);
				}
				else {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1;
				}
			}
		}

		///<summary>Updates priority of all selected procedures to the selected priority.</summary>
		private void listPriorities_MouseDown(object sender,MouseEventArgs e) {
			int clickedRow=listPriorities.IndexFromPoint(e.X,e.Y);
			if(clickedRow==-1) {
				return;//nothing clicked, do nothing.
			}
			List<long> selectedTpNums=new List<long>();
			selectedTpNums.AddRange(gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum));
			//Priority of Procedures is dependent on which TP it is attached to. Track selected procedures by TPNum and ProcNum
			List<Tuple<long,long>> selectedTpNumProcNums=new List<Tuple<long,long>>();
			selectedTpNumProcNums.AddRange(gridTpProcs.SelectedIndices.Where(x => gridTpProcs.Rows[x].Tag!=null).Select(x => (ProcTP)gridTpProcs.Rows[x].Tag)
				.Select(x => new Tuple<long,long>(x.TreatPlanNum,x.ProcNumOrig)));
			List<TreatPlanAttach> listAllTpAttaches=gridTreatPlans.SelectedIndices.ToList().SelectMany(x => (List<TreatPlanAttach>)gridTreatPlans.Rows[x].Tag).ToList();
			foreach(int selectedIdx in gridTpProcs.SelectedIndices) {
				if(gridTpProcs.Rows[selectedIdx].Tag==null) {
					continue;//must be a header row.
				}
				ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[selectedIdx].Tag;
				TreatPlanAttach tpa=listAllTpAttaches.FirstOrDefault(x => x.ProcNum==procTpCur.ProcNumOrig && x.TreatPlanNum==procTpCur.TreatPlanNum);
				if(tpa==null) {
					continue;//should never happen.
				}
				tpa.Priority=0;
				if(clickedRow>0) {
					tpa.Priority=DefC.Short[(int)DefCat.TxPriorities][clickedRow-1].DefNum;
				}
			}
			listAllTpAttaches.Select(x => x.TreatPlanNum).Distinct().ToList()
				.ForEach(x => TreatPlanAttaches.Sync(listAllTpAttaches.FindAll(y => y.TreatPlanNum==x),x));//sync each TP seperately
			TreatPlans.AuditPlans(PatCur.PatNum);//consider adding logic here to update active plan priorities instead of calling the entire AuditPlans function
			selectedTpNums.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x)),true));
			FillTpProcs();
			//Reselect TPs and Procs.
			for(int i=0;i<gridTpProcs.Rows.Count;i++) {
				if(gridTpProcs.Rows[i].Tag==null) {
					continue;
				}
				ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[i].Tag;
				gridTpProcs.SetSelected(i,selectedTpNumProcNums.Contains(new Tuple<long,long>(procTpCur.TreatPlanNum,procTpCur.ProcNumOrig)));
			}
		}

		private void listViewImages_DoubleClick(object sender, System.EventArgs e) {
			if(listViewImages.SelectedIndices.Count==0){
				return;//clicked on white space.
			}
			Document DocCur=DocumentList[(int)visImages[listViewImages.SelectedIndices[0]]];
			if(!ImageHelper.HasImageExtension(DocCur.FileName)){
				try{
					Process.Start(ODFileUtils.CombinePaths(patFolder,DocCur.FileName));
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
				}
				return;
			}
			if(formImageViewer==null || !formImageViewer.Visible){
				formImageViewer=new FormImageViewer();
				formImageViewer.Show();
			}
			if(formImageViewer.WindowState==FormWindowState.Minimized){
				formImageViewer.WindowState=FormWindowState.Normal;
			}
			formImageViewer.BringToFront();
			formImageViewer.SetImage(DocCur,PatCur.GetNameLF()+" - "
				+DocCur.DateCreated.ToShortDateString()+": "+DocCur.Description);
		}

		private void gridChartViews_CellClick(object sender,ODGridClickEventArgs e) {
			ChartViewsCellClicked(e);
		}

		private void gridChartViews_DoubleClick(object sender,ODGridClickEventArgs e) {
			ChartViewsDoubleClicked(e);
		}

		private void gridCustomerViews_CellClick(object sender,ODGridClickEventArgs e) {
			ChartViewsCellClicked(e);
		}

		private void gridCustomerViews_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			ChartViewsDoubleClicked(e);
		}

		private void gridTreatPlans_CellClick(object sender,ODGridClickEventArgs e) {
			gridTpProcs.SetSelected(false);
			FillTpProcs();
			FillToothChart(false);
		}

		private void gridTreatPlans_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			TreatPlan tpSelected=_listTreatPlans[e.Row];
			FormTreatPlanCurEdit FormTPC=new FormTreatPlanCurEdit();
			FormTPC.TreatPlanCur=tpSelected;
			FormTPC.ShowDialog();
			if(FormTPC.DialogResult!=DialogResult.OK) {
				return;
			}
			FillTreatPlans();
			_listTreatPlans.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x.TreatPlanNum)),
				FormTPC.TreatPlanCur.TreatPlanNum==x.TreatPlanNum));
			if(gridTreatPlans.GetSelectedIndex()>-1) {
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void gridTpProcs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridTpProcs.Rows[e.Row].Tag==null) {
				return;//clicked on header row
			}
			ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[e.Row].Tag;
			Procedure procCur=Procedures.GetOneProc(procTpCur.ProcNumOrig,true);
			List<ClaimProc> claimProcList=ClaimProcs.RefreshForTP(PatCur.PatNum);
			//generate a new loop list containing only the procs before this one in it
			List<ClaimProcHist> listClaimProcHistsLoop=new List<ClaimProcHist>();
			for(int i=0;i<_arrayTpProcs.Length;i++) {
				if(_arrayTpProcs[i].ProcNum==procCur.ProcNum) {
					break;
				}
				listClaimProcHistsLoop.AddRange(ClaimProcs.GetHistForProc(claimProcList,_arrayTpProcs[i].ProcNum,_arrayTpProcs[i].CodeNum));
			}
			FormProcEdit FormPE=new FormProcEdit(procCur,PatCur,FamCur);
			FormPE.LoopList=listClaimProcHistsLoop;
			FormPE.HistList=_listClaimProcHists;
			FormPE.ShowDialog();
			List<long> listSelectedTpNums=gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			RefreshModuleData(PatCur.PatNum,true);
			FillProgNotes();
			gridTreatPlans.SetSelected(false);
			listSelectedTpNums.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x)),true));
			FillTpProcs();
			for(int i=0;i<gridTpProcs.Rows.Count;i++) {
				if(gridTpProcs.Rows[i].Tag==null) {
					continue;
				}
				ProcTP procTp=(ProcTP)gridTpProcs.Rows[i].Tag;
				gridTpProcs.SetSelected(i,(procTp.ProcNumOrig==procTpCur.ProcNumOrig && procTp.TreatPlanNum==procTpCur.TreatPlanNum));
			}
		}

		private void ChartViewsCellClicked(ODGridClickEventArgs e) {
			SetChartView(ChartViews.Listt[e.Row]);
			gridChartViews.SetSelected(e.Row,true);
			if(IsDistributorKey) {
				gridCustomerViews.SetSelected(e.Row,true);
			}
		}

		private void ChartViewsDoubleClicked(ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			int count=gridChartViews.Rows.Count;
			FormChartView FormC=new FormChartView();
			FormC.ChartViewCur=ChartViews.Listt[e.Row];
			FormC.ShowDialog();
			FillChartViewsGrid();
			if(IsDistributorKey) {
				FillCustomerViewsGrid();
			}
			if(gridChartViews.Rows.Count==0) {
				FillProgNotes();
				return;//deleted last view, so display default
			}
			if(gridChartViews.Rows.Count==count) {
				gridChartViews.SetSelected(FormC.ChartViewCur.ItemOrder,true);
				SetChartView(ChartViews.Listt[FormC.ChartViewCur.ItemOrder]);
			}
			else if(gridChartViews.Rows.Count>0) {
				for(int i=0;i<ChartViews.Listt.Count;i++) {
					ChartViews.Listt[i].ItemOrder=i;
					ChartViews.Update(ChartViews.Listt[i]);
				}
				if(FormC.ChartViewCur.ItemOrder!=0) {
					gridChartViews.SetSelected(FormC.ChartViewCur.ItemOrder-1,true);
					SetChartView(ChartViews.Listt[FormC.ChartViewCur.ItemOrder-1]);
				}
				else {
					gridChartViews.SetSelected(0,true);
					if(IsDistributorKey) {

					}
					SetChartView(ChartViews.Listt[0]);
				}
			}
			if(IsDistributorKey) {
				gridCustomerViews.SetSelected(gridChartViews.GetSelectedIndex(),true);
			}
		}

		private void butChartViewAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			int count=gridChartViews.Rows.Count;
			int selectedIndex=gridChartViews.GetSelectedIndex();
			FormChartView FormChartAdd=new FormChartView();
			FormChartAdd.ChartViewCur=new ChartView();
			FormChartAdd.ChartViewCur.IsNew=true;
			FormChartAdd.ChartViewCur.ItemOrder=count;
			if(checkAppt.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=1;
			}
			if(checkComm.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=2;
			}
			if(checkCommFamily.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=4;
			}
			if(checkTasks.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=8;
			}
			if(checkEmail.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=16;
			}
			if(checkLabCase.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=32;
			}
			if(checkRx.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=64;
			}
			if(checkSheets.Checked) {
				FormChartAdd.ChartViewCur.ObjectTypes+=128;
			}
			if(checkShowTP.Checked) {
				FormChartAdd.ChartViewCur.ProcStatuses+=1;
			}
			if(checkShowC.Checked) {
				FormChartAdd.ChartViewCur.ProcStatuses+=2;
			}
			if(checkShowE.Checked) {
				FormChartAdd.ChartViewCur.ProcStatuses+=4;
			}
			if(checkShowR.Checked) {
				FormChartAdd.ChartViewCur.ProcStatuses+=16;
			}
			if(checkShowCn.Checked) {
				FormChartAdd.ChartViewCur.ProcStatuses+=64;
			}
			FormChartAdd.ChartViewCur.SelectedTeethOnly=checkShowTeeth.Checked;
			FormChartAdd.ChartViewCur.ShowProcNotes=checkNotes.Checked;
			FormChartAdd.ChartViewCur.IsAudit=checkAudit.Checked;
			for(int i=0;i<listProcStatusCodes.SelectedItems.Count;i++) {
				if(listProcStatusCodes.SelectedItems[i].ToString()=="TP") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.TP;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="C") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.C;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="E") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.E;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="R") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.R;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="RO") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.RO;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="CS") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.CS;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="CR") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.CR;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="CA_Tx") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.CA_Tx;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="CA_EPRD") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.CA_EPRD;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="CA_PD") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.CA_PD;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="S") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.S;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="ST") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.ST;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="W") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.W;
				}
				if(listProcStatusCodes.SelectedItems[i].ToString()=="A") {
					FormChartAdd.ChartViewCur.OrionStatusFlags|=OrionStatus.A;
				}
			}
			FormChartAdd.ShowDialog();
			FillChartViewsGrid();
			if(IsDistributorKey) {
				FillCustomerViewsGrid();
			}
			int count2=gridChartViews.Rows.Count;
			if(count2==0) { 
				return; 
			}
			if(count2==count) {
				if(selectedIndex!=-1) {
					gridChartViews.SetSelected(selectedIndex,true);
					if(IsDistributorKey) {
						gridCustomerViews.SetSelected(selectedIndex,true);
					}
					SetChartView(ChartViews.Listt[selectedIndex]);
				}
			}
			else {
				FormChartAdd.ChartViewCur.ItemOrder=count;
				ChartViews.Update(FormChartAdd.ChartViewCur);
				FillChartViewsGrid();
				if(IsDistributorKey) {
					FillCustomerViewsGrid();
				}
				SetChartView(ChartViews.Listt[count]);
				gridChartViews.SetSelected(count,true);
				if(IsDistributorKey) {
					gridCustomerViews.SetSelected(selectedIndex,true);
				}
			}
		}

		private void butChartViewUp_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			if(gridChartViews.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a view first.");
				return;
			}
			int oldIdx;
			int newIdx;
			ChartView oldItem;
			ChartView newItem;
			if(gridChartViews.GetSelectedIndex()!=-1) {
				oldIdx=gridChartViews.GetSelectedIndex();
				if(oldIdx==0) {
					return;//can't move up any more
				}
				newIdx=oldIdx-1; 
				for(int i=0;i<ChartViews.Listt.Count;i++) {
					if(ChartViews.Listt[i].ItemOrder==oldIdx) {
						oldItem=ChartViews.Listt[i];
						newItem=ChartViews.Listt[newIdx];
						oldItem.ItemOrder=newItem.ItemOrder;
						newItem.ItemOrder+=1;
						ChartViews.Update(oldItem);
						ChartViews.Update(newItem);
					}
				}
				FillChartViewsGrid();
				gridChartViews.SetSelected(newIdx,true);
				if(IsDistributorKey) {
					FillCustomerViewsGrid();
					gridCustomerViews.SetSelected(newIdx,true);
				}
				SetChartView(ChartViews.Listt[newIdx]);
			}
		}

		private void butChartViewDown_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			if(gridChartViews.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a view first.");
				return;
			}
			int oldIdx;
			int newIdx;
			ChartView oldItem;
			ChartView newItem;
			if(gridChartViews.GetSelectedIndex()!=-1) {
				oldIdx=gridChartViews.GetSelectedIndex();
				if(oldIdx==ChartViews.Listt.Count-1) {
					return;//can't move down any more
				}
				newIdx=oldIdx+1;
				for(int i=0;i<ChartViews.Listt.Count;i++) {
					if(ChartViews.Listt[i].ItemOrder==newIdx) {
						newItem=ChartViews.Listt[i];
						oldItem=ChartViews.Listt[oldIdx];
						newItem.ItemOrder=oldItem.ItemOrder;
						oldItem.ItemOrder+=1;
						ChartViews.Update(oldItem);
						ChartViews.Update(newItem);
					}
				}
				FillChartViewsGrid();
				gridChartViews.SetSelected(newIdx,true);
				if(IsDistributorKey) {
					FillCustomerViewsGrid();
					gridCustomerViews.SetSelected(newIdx,true);
				}
				SetChartView(ChartViews.Listt[newIdx]);
			}
		}

		public void FunctionKeyPressContrChart(Keys keys) {
			switch(keys) {
				case Keys.F1: 
					if(gridChartViews.Rows.Count>0) {
						gridChartViews.SetSelected(0,true);
						SetChartView(ChartViews.Listt[0]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>0) {
						gridCustomerViews.SetSelected(0,true);
					}
					break;
				case Keys.F2:
					if(gridChartViews.Rows.Count>1) {
						gridChartViews.SetSelected(1,true);
						SetChartView(ChartViews.Listt[1]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>1) {
						gridCustomerViews.SetSelected(1,true);
					}
					break;
				case Keys.F3:
					if(gridChartViews.Rows.Count>2) {
						gridChartViews.SetSelected(2,true);
						SetChartView(ChartViews.Listt[2]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>2) {
						gridCustomerViews.SetSelected(2,true);
					}
					break;
				case Keys.F4:
					if(gridChartViews.Rows.Count>3) {
						gridChartViews.SetSelected(3,true);
						SetChartView(ChartViews.Listt[3]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>3) {
						gridCustomerViews.SetSelected(3,true);
					}
					break;
				case Keys.F5:
					if(gridChartViews.Rows.Count>4) {
						gridChartViews.SetSelected(4,true);
						SetChartView(ChartViews.Listt[4]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>4) {
						gridCustomerViews.SetSelected(4,true);
					}
					break;
				case Keys.F6:
					if(gridChartViews.Rows.Count>5) {
						gridChartViews.SetSelected(5,true);
						SetChartView(ChartViews.Listt[5]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>5) {
						gridCustomerViews.SetSelected(5,true);
					}
					break;
				case Keys.F7:
					if(gridChartViews.Rows.Count>6) {
						gridChartViews.SetSelected(6,true);
						SetChartView(ChartViews.Listt[6]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>6) {
						gridCustomerViews.SetSelected(6,true);
					}
					break;
				case Keys.F8:
					if(gridChartViews.Rows.Count>7) {
						gridChartViews.SetSelected(7,true);
						SetChartView(ChartViews.Listt[7]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>7) {
						gridCustomerViews.SetSelected(7,true);
					}
					break;
				case Keys.F9:
					if(gridChartViews.Rows.Count>8) {
						gridChartViews.SetSelected(8,true);
						SetChartView(ChartViews.Listt[8]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>8) {
						gridCustomerViews.SetSelected(8,true);
					}
					break;
				case Keys.F10:
					if(gridChartViews.Rows.Count>9) {
						gridChartViews.SetSelected(9,true);
						SetChartView(ChartViews.Listt[9]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>9) {
						gridCustomerViews.SetSelected(9,true);
					}
					break;
				case Keys.F11:
					if(gridChartViews.Rows.Count>10) {
						gridChartViews.SetSelected(10,true);
						SetChartView(ChartViews.Listt[10]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>10) {
						gridCustomerViews.SetSelected(10,true);
					}
					break;
				case Keys.F12:
					if(gridChartViews.Rows.Count>11) {
						gridChartViews.SetSelected(11,true);
						SetChartView(ChartViews.Listt[11]);
					}
					if(IsDistributorKey && gridCustomerViews.Rows.Count>11) {
						gridCustomerViews.SetSelected(11,true);
					}
					break;
			}
		}

		private void listCommonProcs_MouseDown(object sender,MouseEventArgs e) {
			if(listCommonProcs.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a procedure.");
				return;
			}
			string procCode="";
			double procFee=0;
			//Hard coded internal procedures.
			switch(listCommonProcs.SelectedIndex) {
				case 0://Monthly Maintenance
					procCode="001";
					procFee=149;
					break;
				case 1://Monthly Mobile
					procCode="027";
					procFee=10;
					break;
				case 2://Monthly E-Mail Support
					procCode="008";
					procFee=89;
					break;
				case 3://Monthly EHR
					procCode="029";
					break;
				case 4://Data Conversion
					procCode="005";
					procFee=700;
					break;
				case 5://Trial Conversion
					procCode="N5641";
					break;
				case 6://Demo
					procCode="018";
					break;
				case 7://Online Training
					procCode="N1254";
					break;
				case 8://Additional Online Training
					procCode="N8989";
					procFee=50;
					break;
				case 9://eCW Online Training
					procCode="eCW1";
					break;
				case 10://eCW Installation Verification
					procCode="eCW2";
					break;
				case 11://Programming
					procCode="007";
					break;
				case 12://Query Programming
					procCode="023";
					procFee=90;
					break;
			}
			//Simply add the procedure to the customers account.
			Procedure proc=new Procedure();
			proc.CodeNum=ProcedureCodes.GetCodeNum(procCode);
			proc.DateEntryC=DateTimeOD.Today;
			proc.PatNum=PatCur.PatNum;
			proc.ProcDate=DateTime.Now;
			proc.DateTP=DateTime.Now;
			proc.ProcFee=procFee;
			proc.ProcStatus=ProcStat.TP;
			proc.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			proc.MedicalCode=ProcedureCodes.GetProcCode(proc.CodeNum).MedicalCode;
			proc.BaseUnits=ProcedureCodes.GetProcCode(proc.CodeNum).BaseUnits;
			Procedures.Insert(proc);//no recall synch needed because dental offices don't use this feature
			listCommonProcs.SelectedIndex=-1;
			FillProgNotes();
		}

		private void butNewTP_Click(object sender,EventArgs e) {
			FormTreatPlanCurEdit FormTPCE=new FormTreatPlanCurEdit();
			FormTPCE.TreatPlanCur=new TreatPlan() {
				Heading="Inactive Treatment Plan",
				Note=PrefC.GetString(PrefName.TreatmentPlanNote),
				PatNum=PatCur.PatNum,
				TPStatus=TreatPlanStatus.Inactive,
			};
			FormTPCE.ShowDialog();
			if(FormTPCE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillTreatPlans();
			_listTreatPlans.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x.TreatPlanNum)),
				FormTPCE.TreatPlanCur.TreatPlanNum==x.TreatPlanNum));
			if(gridTreatPlans.GetSelectedIndex()>-1) {
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			ModuleSelected(PatCur.PatNum);//refreshes TPs
		}

		private void butBig_Click(object sender,EventArgs e) {
			
		}

		private void butECWup_Click(object sender,EventArgs e) {
			panelEcw.Location=toothChart.Location;
			if(panelImages.Visible) {
				panelEcw.Height=tabControlImages.Top-panelEcw.Top+1-(panelImages.Height+2);
			}
			else {
				panelEcw.Height=tabControlImages.Top-panelEcw.Top+1;
			}
			butECWdown.Visible=true;
			butECWup.Visible=false;
		}

		private void butECWdown_Click(object sender,EventArgs e) {
			panelEcw.Location=new Point(524+2,textTreatmentNotes.Bottom+1);
			if(panelImages.Visible) {
				panelEcw.Height=tabControlImages.Top-panelEcw.Top+1-(panelImages.Height+2);
			}
			else {
				panelEcw.Height=tabControlImages.Top-panelEcw.Top+1;
			}
			butECWdown.Visible=false;
			butECWup.Visible=true;
		}

		//#region Quick Buttons (Deprecated)
		//private void panelQuickButtons_Paint(object sender,PaintEventArgs e) {

		//}
		
		//private void buttonCDO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DO";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void buttonCMOD_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MOD";
		//	ProcButtonClicked(0,"D2393");
		//}

		//private void buttonCO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "O";
		//	ProcButtonClicked(0,"D2391");
		//}

		//private void buttonCMO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MO";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butCOL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "OL";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butCOB_Click(object sender,EventArgs e) {
		//	textSurf.Text = "OB";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butDL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DL";
		//	ProcButtonClicked(0,"D2331");
		//}

		//private void butML_Click(object sender,EventArgs e) {
		//	textSurf.Text = "ML";
		//	ProcButtonClicked(0,"D2331");
		//}

		//private void buttonCSeal_Click(object sender,EventArgs e) {
		//	textSurf.Text = "";
		//	ProcButtonClicked(0,"D1351");
		//}

		//private void buttonADO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DO";
		//	ProcButtonClicked(0,"D2150");
		//}

		//private void buttonAMOD_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MOD";
		//	ProcButtonClicked(0,"D2160");
		//}

		//private void buttonAO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "O";
		//	ProcButtonClicked(0,"D2140");
		//}

		//private void buttonAMO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MO";
		//	ProcButtonClicked(0,"D2150");
		//}

		//private void butCMDL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MDL";
		//	ProcButtonClicked(0,"D2332");
		//}

		//private void buttonAOL_Click(object sender, EventArgs e){
		//	textSurf.Text = "OL";
		//	ProcButtonClicked(0, "D2150");
		//}

		//private void buttonAOB_Click(object sender, EventArgs e){
		//	textSurf.Text = "OB";
		//	ProcButtonClicked(0, "D2150");
		//}

		//private void buttonAMODL_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODL";
		//	ProcButtonClicked(0, "D2161");
		//}

		//private void buttonAMODB_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODB";
		//	ProcButtonClicked(0, "D2161");
		//}

		//private void buttonCMODL_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODL";
		//	ProcButtonClicked(0, "D2394");
		//}

		//private void buttonCMODB_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODB";
		//	ProcButtonClicked(0, "D2394");
		//}
		//#endregion Quick Buttons

		private void butAddKey_Click(object sender,EventArgs e) {
			RegistrationKey key=new RegistrationKey();
			key.PatNum=PatCur.PatNum;
			//Notes are not commonly necessary, because most customers have only one office (thus only 1 key is necessary).
			//A tech can edit the note later after it is added if necessary.
			key.Note="";
			key.DateStarted=DateTime.Today;
			key.IsForeign=false;
			key.VotesAllotted=100;
			RegistrationKeys.Insert(key);
			FillPtInfo();//Refresh registration key list in patient info grid.
		}

		private void butForeignKey_Click(object sender,EventArgs e) {
			RegistrationKey key=new RegistrationKey();
			key.PatNum=PatCur.PatNum;
			key.Note="";
			key.DateStarted=DateTime.Today;
			key.IsForeign=true;
			key.VotesAllotted=100;
			RegistrationKeys.Insert(key);
			FillPtInfo();
		}

		private void butPhoneNums_Click(object sender,EventArgs e) {
			if(FormOpenDental.CurPatNum==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			FormPhoneNumbersManage FormM=new FormPhoneNumbersManage();
			FormM.PatNum=FormOpenDental.CurPatNum;
			FormM.ShowDialog();			
		}

		private void butErxAccess_Click(object sender,EventArgs e) {
			FormErxAccess FormEA=new FormErxAccess(PatCur);
			FormEA.ShowDialog();
		}

		private void butLoadDirectX_Click(object sender,EventArgs e) {
			//toothChart.LoadDirectX();
		}

		private void gridProg_CellClick(object sender,ODGridClickEventArgs e) {
			DataTable progNotes=DataSetMain.Tables["ProgNotes"];
			//DataRow rowClicked=progNotes.Rows[e.Row];
			DataRow rowClicked=(DataRow)gridProg.Rows[e.Row].Tag;
			long procNum=PIn.Long(rowClicked["ProcNum"].ToString());
			if(procNum==0){//if not a procedure
				return;
			}
			long codeNum=PIn.Long(rowClicked["CodeNum"].ToString());
			if(ProcedureCodes.GetStringProcCode(codeNum)!=ProcedureCodes.GroupProcCode){//if not a group note
				return;
			}
			List<ProcGroupItem> groupItemList=ProcGroupItems.GetForGroup(procNum);
			//for(int i=0;i<progNotes.Rows.Count;i++){
			for(int i=0;i<gridProg.Rows.Count;i++){
				DataRow row=(DataRow)gridProg.Rows[i].Tag;
				if(row["ProcNum"].ToString()=="0"){
					continue;
				}
				long procnum=PIn.Long(row["ProcNum"].ToString());
				for(int j=0;j<groupItemList.Count;j++){
					if(procnum==groupItemList[j].ProcNum){
						gridProg.SetSelected(i,true);
					}
				}
			}
		}

		///<summary>This does not currently handle custom views.</summary>
		private void SetDateRange() {
			switch(ChartViewCurDisplay.DatesShowing) {
				case ChartViewDates.All:
					ShowDateStart=DateTime.MinValue;
					ShowDateEnd=DateTime.MinValue;//interpreted as empty.  We want to show all future dates.
					break;
				case ChartViewDates.Today:
					ShowDateStart=DateTime.Today;
					ShowDateEnd=DateTime.Today;
					break;
				case ChartViewDates.Yesterday:
					ShowDateStart=DateTime.Today.AddDays(-1);
					ShowDateEnd=DateTime.Today.AddDays(-1);
					break;
				case ChartViewDates.ThisYear:
					ShowDateStart=new DateTime(DateTime.Today.Year,1,1);
					ShowDateEnd=new DateTime(DateTime.Today.Year,12,31);
					break;
				case ChartViewDates.LastYear:
					ShowDateStart=new DateTime(DateTime.Today.Year-1,1,1);
					ShowDateEnd=new DateTime(DateTime.Today.Year-1,12,31);
					break;
			}
		}

		/// <summary>This method is used to set the Date Range filter start and stop dates based on either a custom date range or DatesShowing property of chart view.</summary>
		private void FillDateRange() {
			textShowDateRange.Text="";
			if(ShowDateStart.Year > 1880) {
				textShowDateRange.Text+=ShowDateStart.ToShortDateString();
			}
			if(ShowDateEnd.Year > 1880 && ShowDateStart != ShowDateEnd) {
				if(textShowDateRange.Text!="") {
					textShowDateRange.Text+="-";
				}
				textShowDateRange.Text+=ShowDateEnd.ToShortDateString();
			}
			if(textShowDateRange.Text=="") {
				textShowDateRange.Text=Lan.g(this,"All Dates");
			}
		}

		private void butShowDateRange_Click(object sender,EventArgs e) {
			FormChartViewDateFilter FormC=new FormChartViewDateFilter();
			FormC.DateStart=ShowDateStart;
			FormC.DateEnd=ShowDateEnd;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			ShowDateStart=FormC.DateStart;
			ShowDateEnd=FormC.DateEnd;
			if(gridChartViews.Rows.Count>0) {//enable custom view label
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true; 
			FillDateRange();
			FillProgNotes();
		}

		///<summary>Handles single clicks that occur on button items. Not double clicks, not labels, and not empty space.</summary>
		private void panelQuickButtons_ItemClickBut(object sender,ODButtonPanelEventArgs e) {
			ProcButtonQuick pbq=null;
			for(int i=0;i<e.Item.Tags.Count;i++){
				if(e.Item.Tags[i].GetType()!=typeof(ProcButtonQuick)){
					continue;
				}
				pbq=(ProcButtonQuick)e.Item.Tags[i];//should always happen
			}
			if(pbq==null){
				return;//should never happen.
			}
			ProcButtonClicked(0,pbq);
		}

		///<summary>Creates log entries for completed procedures</summary>
		private void logComplCreate(Procedure procCur) {
			if(newStatus!=ProcStat.C) {
				return;
			}
			string teeth=String.Join(", ",toothChart.SelectedTeeth);
			ProcedureL.LogProcComplCreate(PatCur.PatNum,procCur,teeth);
		}

		#region VisiQuick integration code written by Thomas Jensen tje@thomsystems.com 
		/*
		private void XrayLinkBtn_Click(object sender, System.EventArgs e)	// TJE
		{
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)
				return;
			VQLink.VQStart(false,"",0,0);
		}

		private void SetPanelCol(Panel p, char c)	// TJE
		{
			if (c != '0')
				p.BackColor=SystemColors.ActiveCaption;
			else
				p.BackColor=SystemColors.ActiveBorder;
		}

		private void VQUpdatePatient()	// TJE
		{
			String	s;
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)	
				s="";
			else
				s=VQLink.SearchTStatus(Patients.Cur.PatNum);
			if (s.Length>=32) 
			{
				SetPanelCol(tooth11,s[0]);
				SetPanelCol(tooth12,s[1]);
				SetPanelCol(tooth13,s[2]);
				SetPanelCol(tooth14,s[3]);
				SetPanelCol(tooth15,s[4]);
				SetPanelCol(tooth16,s[5]);
				SetPanelCol(tooth17,s[6]);
				SetPanelCol(tooth18,s[7]);
				SetPanelCol(tooth21,s[8]);
				SetPanelCol(tooth22,s[9]);
				SetPanelCol(tooth23,s[10]);
				SetPanelCol(tooth24,s[11]);
				SetPanelCol(tooth25,s[12]);
				SetPanelCol(tooth26,s[13]);
				SetPanelCol(tooth27,s[14]);
				SetPanelCol(tooth28,s[15]);
				SetPanelCol(tooth31,s[16]);
				SetPanelCol(tooth32,s[17]);
				SetPanelCol(tooth33,s[18]);
				SetPanelCol(tooth34,s[19]);
				SetPanelCol(tooth35,s[20]);
				SetPanelCol(tooth36,s[21]);
				SetPanelCol(tooth37,s[22]);
				SetPanelCol(tooth38,s[23]);
				SetPanelCol(tooth41,s[24]);
				SetPanelCol(tooth42,s[25]);
				SetPanelCol(tooth43,s[26]);
				SetPanelCol(tooth44,s[27]);
				SetPanelCol(tooth45,s[28]);
				SetPanelCol(tooth46,s[29]);
				SetPanelCol(tooth47,s[30]);
				SetPanelCol(tooth48,s[31]);
			}
			if (s.Length>=32+6) 
			{
				SetPanelCol(toothpanos,s[32]);
				SetPanelCol(toothcephs,s[33]);
				if (s[34]!='0' | s[35]!='0' | s[36]!='0' | s[37]!='0') 
				{
					SetPanelCol(toothbw,'1');
					SetPanelCol(toothbwfloat,'1');
				}
				else
				{
					SetPanelCol(toothbw,'0');
					SetPanelCol(toothbwfloat,'0');
				}
			}
			if (s.Length>=32+6+9) 
			{
				if (s[39]!='0' | s[40]!='0' | s[41]!='0' | s[43]!='0') 
					SetPanelCol(toothcolors,'1');
				else
					SetPanelCol(toothcolors,'0');
				SetPanelCol(toothxrays,s[42]);
				SetPanelCol(toothpanos,s[44]);
				SetPanelCol(toothcephs,s[45]);
				SetPanelCol(toothdocs,s[46]);
			}
			if (s.Length>=32+6+9+1) 
			{
				SetPanelCol(toothfiles,s[47]);
			}
		}

		private void tooth18_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos(((Panel)sender).Name.Substring(5,2),VisiQuick.spf_tinymode+VisiQuick.spf_single,0);	
		}

		private void toothbwfloat_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_tinymode+VisiQuick.spf_2horizontal,VisiQuick.spi_bitewings);
		}

		private void toothbw_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.npi_xrayview,VisiQuick.spi_bitewings);
		}

		private void toothxrays_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_xrayview);
		}

		private void toothcolors_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_colorview);
		}

		private void toothpanos_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_panview);
		}

		private void toothcephs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_cephview);
		}

		private void toothdocs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_docview);
		}

		private void toothfiles_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_fileview);
		}
		*/
		#endregion
	}//end class


	


}
