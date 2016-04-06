using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormMisc : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private IContainer components;
		private System.Windows.Forms.TextBox textMainWindowTitle;
		private System.Windows.Forms.Label label3;
		private OpenDental.ValidNumber textSigInterval;
		private OpenDental.UI.Button butLanguages;
		private Label label4;
		private ToolTip toolTip1;
		private ComboBox comboShowID;
		private CheckBox checkTaskListAlwaysShow;
		private CheckBox checkTasksCheckOnStartup;
		private CheckBox checkBoxTaskKeepListHidden;
		//private ComputerPref computerPref;
		private ValidNumber validNumX;
		private Label labelX;
		private GroupBox groupBox2;
		private GroupBox groupBoxTaskDefaults;
		private ValidNumber validNumY;
		private Label labelY;
		private RadioButton radioRight;
		private RadioButton radioBottom;
		private Label label15;
		private Label label17;
		private GroupBox groupBox6;
		private CheckBox checkTitleBarShowSite;
		private TextBox textWebServiceServerName;
		private Label label2;
		private CheckBox checkTasksNewTrackedByUser;
		private CheckBox checkShowOpenTickets;
		private CheckBox checkColorTheme;
		private ValidNumber textInactiveSignal;
		private Label label5;
		private CheckBox checkPrefFName;
		private CheckBox checkRefresh;
		private CheckBox checkImeCompositionCompatibility;
		private TextBox textLanguageAndRegion;
		private Label label6;
		private UI.Button butPickLanguageAndRegion;
    private CheckBox checkTimeCardUseLocal;
		private ComboBox comboTrackClinic;
		private Label labelTrackClinic;
		private Label label1;
		private UI.Button butDecimal;
		private TextBox textNumDecimals;
		private Label label7;
		private List<string> _trackLastClinicBy;
		//private List<Def> posAdjTypes;

		///<summary></summary>
		public FormMisc(){
			InitializeComponent();
			Lan.F(this);
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

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMisc));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkBoxTaskKeepListHidden = new System.Windows.Forms.CheckBox();
			this.checkTaskListAlwaysShow = new System.Windows.Forms.CheckBox();
			this.textNumDecimals = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboTrackClinic = new System.Windows.Forms.ComboBox();
			this.labelTrackClinic = new System.Windows.Forms.Label();
			this.checkTimeCardUseLocal = new System.Windows.Forms.CheckBox();
			this.textLanguageAndRegion = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.checkImeCompositionCompatibility = new System.Windows.Forms.CheckBox();
			this.checkRefresh = new System.Windows.Forms.CheckBox();
			this.checkPrefFName = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.checkColorTheme = new System.Windows.Forms.CheckBox();
			this.textWebServiceServerName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkTitleBarShowSite = new System.Windows.Forms.CheckBox();
			this.textMainWindowTitle = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.comboShowID = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkShowOpenTickets = new System.Windows.Forms.CheckBox();
			this.checkTasksNewTrackedByUser = new System.Windows.Forms.CheckBox();
			this.groupBoxTaskDefaults = new System.Windows.Forms.GroupBox();
			this.radioRight = new System.Windows.Forms.RadioButton();
			this.radioBottom = new System.Windows.Forms.RadioButton();
			this.validNumY = new OpenDental.ValidNumber();
			this.labelY = new System.Windows.Forms.Label();
			this.validNumX = new OpenDental.ValidNumber();
			this.labelX = new System.Windows.Forms.Label();
			this.checkTasksCheckOnStartup = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butDecimal = new OpenDental.UI.Button();
			this.butPickLanguageAndRegion = new OpenDental.UI.Button();
			this.textInactiveSignal = new OpenDental.ValidNumber();
			this.butLanguages = new OpenDental.UI.Button();
			this.textSigInterval = new OpenDental.ValidNumber();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox6.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBoxTaskDefaults.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			this.toolTip1.AutoPopDelay = 600000;
			this.toolTip1.InitialDelay = 0;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 0;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.toolTip1.ToolTipTitle = "Help";
			// 
			// checkBoxTaskKeepListHidden
			// 
			this.checkBoxTaskKeepListHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxTaskKeepListHidden.Location = new System.Drawing.Point(63, 19);
			this.checkBoxTaskKeepListHidden.Name = "checkBoxTaskKeepListHidden";
			this.checkBoxTaskKeepListHidden.Size = new System.Drawing.Size(218, 20);
			this.checkBoxTaskKeepListHidden.TabIndex = 185;
			this.checkBoxTaskKeepListHidden.Text = "Don\'t show on this computer";
			this.checkBoxTaskKeepListHidden.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.toolTip1.SetToolTip(this.checkBoxTaskKeepListHidden, resources.GetString("checkBoxTaskKeepListHidden.ToolTip"));
			this.checkBoxTaskKeepListHidden.UseVisualStyleBackColor = true;
			this.checkBoxTaskKeepListHidden.CheckedChanged += new System.EventHandler(this.checkBoxTaskKeepListHidden_CheckedChanged);
			// 
			// checkTaskListAlwaysShow
			// 
			this.checkTaskListAlwaysShow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTaskListAlwaysShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTaskListAlwaysShow.Location = new System.Drawing.Point(158, 34);
			this.checkTaskListAlwaysShow.Name = "checkTaskListAlwaysShow";
			this.checkTaskListAlwaysShow.Size = new System.Drawing.Size(190, 14);
			this.checkTaskListAlwaysShow.TabIndex = 74;
			this.checkTaskListAlwaysShow.Text = "Global - Always show Task List";
			this.checkTaskListAlwaysShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.checkTaskListAlwaysShow, resources.GetString("checkTaskListAlwaysShow.ToolTip"));
			this.checkTaskListAlwaysShow.CheckedChanged += new System.EventHandler(this.checkTaskListAlwaysShow_CheckedChanged);
			// 
			// textNumDecimals
			// 
			this.textNumDecimals.Location = new System.Drawing.Point(402, 323);
			this.textNumDecimals.Name = "textNumDecimals";
			this.textNumDecimals.ReadOnly = true;
			this.textNumDecimals.Size = new System.Drawing.Size(47, 20);
			this.textNumDecimals.TabIndex = 217;
			this.textNumDecimals.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(122, 325);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(279, 17);
			this.label7.TabIndex = 218;
			this.label7.Text = "Currency number of digits after decimal";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTrackClinic
			// 
			this.comboTrackClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrackClinic.FormattingEnabled = true;
			this.comboTrackClinic.Location = new System.Drawing.Point(319, 582);
			this.comboTrackClinic.Name = "comboTrackClinic";
			this.comboTrackClinic.Size = new System.Drawing.Size(130, 21);
			this.comboTrackClinic.TabIndex = 215;
			// 
			// labelTrackClinic
			// 
			this.labelTrackClinic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelTrackClinic.Location = new System.Drawing.Point(122, 585);
			this.labelTrackClinic.Name = "labelTrackClinic";
			this.labelTrackClinic.Size = new System.Drawing.Size(195, 15);
			this.labelTrackClinic.TabIndex = 216;
			this.labelTrackClinic.Text = "Track Last Clinic By";
			this.labelTrackClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkTimeCardUseLocal
			// 
			this.checkTimeCardUseLocal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardUseLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeCardUseLocal.Location = new System.Drawing.Point(81, 137);
			this.checkTimeCardUseLocal.Name = "checkTimeCardUseLocal";
			this.checkTimeCardUseLocal.Size = new System.Drawing.Size(368, 17);
			this.checkTimeCardUseLocal.TabIndex = 214;
			this.checkTimeCardUseLocal.Text = "Time Cards use local time";
			this.checkTimeCardUseLocal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLanguageAndRegion
			// 
			this.textLanguageAndRegion.Location = new System.Drawing.Point(284, 297);
			this.textLanguageAndRegion.Name = "textLanguageAndRegion";
			this.textLanguageAndRegion.ReadOnly = true;
			this.textLanguageAndRegion.Size = new System.Drawing.Size(165, 20);
			this.textLanguageAndRegion.TabIndex = 204;
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(4, 299);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(279, 17);
			this.label6.TabIndex = 205;
			this.label6.Text = "Language and region used by program";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkImeCompositionCompatibility
			// 
			this.checkImeCompositionCompatibility.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkImeCompositionCompatibility.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkImeCompositionCompatibility.Location = new System.Drawing.Point(15, 345);
			this.checkImeCompositionCompatibility.Name = "checkImeCompositionCompatibility";
			this.checkImeCompositionCompatibility.Size = new System.Drawing.Size(434, 19);
			this.checkImeCompositionCompatibility.TabIndex = 203;
			this.checkImeCompositionCompatibility.Text = "Text boxes use foreign language Input Method Editor (IME) composition";
			this.checkImeCompositionCompatibility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRefresh
			// 
			this.checkRefresh.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRefresh.Location = new System.Drawing.Point(14, 158);
			this.checkRefresh.Name = "checkRefresh";
			this.checkRefresh.Size = new System.Drawing.Size(435, 18);
			this.checkRefresh.TabIndex = 202;
			this.checkRefresh.Text = "New Computers default to refresh while typing in Select Patient window";
			this.checkRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRefresh.UseVisualStyleBackColor = true;
			// 
			// checkPrefFName
			// 
			this.checkPrefFName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPrefFName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPrefFName.Location = new System.Drawing.Point(15, 175);
			this.checkPrefFName.Name = "checkPrefFName";
			this.checkPrefFName.Size = new System.Drawing.Size(434, 19);
			this.checkPrefFName.TabIndex = 79;
			this.checkPrefFName.Text = "Search for preferred name in first name field in Select Patient window";
			this.checkPrefFName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(41, 231);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(333, 35);
			this.label5.TabIndex = 200;
			this.label5.Text = "Disable signal interval after this many minutes of user inactivity.\r\nLeave blank " +
    "to disable.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkColorTheme
			// 
			this.checkColorTheme.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkColorTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkColorTheme.Location = new System.Drawing.Point(87, 118);
			this.checkColorTheme.Name = "checkColorTheme";
			this.checkColorTheme.Size = new System.Drawing.Size(362, 17);
			this.checkColorTheme.TabIndex = 199;
			this.checkColorTheme.Text = "Blue theme";
			this.checkColorTheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWebServiceServerName
			// 
			this.textWebServiceServerName.Location = new System.Drawing.Point(284, 368);
			this.textWebServiceServerName.Name = "textWebServiceServerName";
			this.textWebServiceServerName.Size = new System.Drawing.Size(165, 20);
			this.textWebServiceServerName.TabIndex = 197;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(4, 371);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(279, 17);
			this.label2.TabIndex = 198;
			this.label2.Text = "Update Server Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(453, 17);
			this.label1.TabIndex = 196;
			this.label1.Text = "See Setup | Module Preferences for setup options that were previously in this win" +
    "dow.";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkTitleBarShowSite);
			this.groupBox6.Controls.Add(this.textMainWindowTitle);
			this.groupBox6.Controls.Add(this.label15);
			this.groupBox6.Controls.Add(this.comboShowID);
			this.groupBox6.Controls.Add(this.label17);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(12, 31);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(453, 83);
			this.groupBox6.TabIndex = 195;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Main Window Title";
			// 
			// checkTitleBarShowSite
			// 
			this.checkTitleBarShowSite.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTitleBarShowSite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTitleBarShowSite.Location = new System.Drawing.Point(75, 60);
			this.checkTitleBarShowSite.Name = "checkTitleBarShowSite";
			this.checkTitleBarShowSite.Size = new System.Drawing.Size(362, 17);
			this.checkTitleBarShowSite.TabIndex = 74;
			this.checkTitleBarShowSite.Text = "Show Site (public health must also be turned on)";
			this.checkTitleBarShowSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMainWindowTitle
			// 
			this.textMainWindowTitle.Location = new System.Drawing.Point(170, 14);
			this.textMainWindowTitle.Name = "textMainWindowTitle";
			this.textMainWindowTitle.Size = new System.Drawing.Size(267, 20);
			this.textMainWindowTitle.TabIndex = 38;
			// 
			// label15
			// 
			this.label15.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label15.Location = new System.Drawing.Point(86, 17);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(83, 17);
			this.label15.TabIndex = 39;
			this.label15.Text = "Title Text";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboShowID
			// 
			this.comboShowID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShowID.FormattingEnabled = true;
			this.comboShowID.Location = new System.Drawing.Point(307, 36);
			this.comboShowID.Name = "comboShowID";
			this.comboShowID.Size = new System.Drawing.Size(130, 21);
			this.comboShowID.TabIndex = 72;
			// 
			// label17
			// 
			this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label17.Location = new System.Drawing.Point(110, 39);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(195, 15);
			this.label17.TabIndex = 73;
			this.label17.Text = "Show ID in title bar";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkShowOpenTickets);
			this.groupBox2.Controls.Add(this.checkTasksNewTrackedByUser);
			this.groupBox2.Controls.Add(this.groupBoxTaskDefaults);
			this.groupBox2.Controls.Add(this.checkTaskListAlwaysShow);
			this.groupBox2.Controls.Add(this.checkTasksCheckOnStartup);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(101, 392);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(364, 184);
			this.groupBox2.TabIndex = 188;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Task List";
			// 
			// checkShowOpenTickets
			// 
			this.checkShowOpenTickets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowOpenTickets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowOpenTickets.Location = new System.Drawing.Point(6, 72);
			this.checkShowOpenTickets.Name = "checkShowOpenTickets";
			this.checkShowOpenTickets.Size = new System.Drawing.Size(342, 14);
			this.checkShowOpenTickets.TabIndex = 78;
			this.checkShowOpenTickets.Text = "Show open tickets for user";
			this.checkShowOpenTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTasksNewTrackedByUser
			// 
			this.checkTasksNewTrackedByUser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTasksNewTrackedByUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTasksNewTrackedByUser.Location = new System.Drawing.Point(48, 53);
			this.checkTasksNewTrackedByUser.Name = "checkTasksNewTrackedByUser";
			this.checkTasksNewTrackedByUser.Size = new System.Drawing.Size(300, 14);
			this.checkTasksNewTrackedByUser.TabIndex = 77;
			this.checkTasksNewTrackedByUser.Text = "New/Viewed status tracked by individual user";
			this.checkTasksNewTrackedByUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxTaskDefaults
			// 
			this.groupBoxTaskDefaults.Controls.Add(this.radioRight);
			this.groupBoxTaskDefaults.Controls.Add(this.radioBottom);
			this.groupBoxTaskDefaults.Controls.Add(this.validNumY);
			this.groupBoxTaskDefaults.Controls.Add(this.labelY);
			this.groupBoxTaskDefaults.Controls.Add(this.validNumX);
			this.groupBoxTaskDefaults.Controls.Add(this.labelX);
			this.groupBoxTaskDefaults.Controls.Add(this.checkBoxTaskKeepListHidden);
			this.groupBoxTaskDefaults.Enabled = false;
			this.groupBoxTaskDefaults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBoxTaskDefaults.Location = new System.Drawing.Point(67, 94);
			this.groupBoxTaskDefaults.Name = "groupBoxTaskDefaults";
			this.groupBoxTaskDefaults.Size = new System.Drawing.Size(290, 83);
			this.groupBoxTaskDefaults.TabIndex = 76;
			this.groupBoxTaskDefaults.TabStop = false;
			this.groupBoxTaskDefaults.Text = "Local Computer Default Settings";
			// 
			// radioRight
			// 
			this.radioRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioRight.Location = new System.Drawing.Point(84, 38);
			this.radioRight.Name = "radioRight";
			this.radioRight.Size = new System.Drawing.Size(99, 17);
			this.radioRight.TabIndex = 191;
			this.radioRight.TabStop = true;
			this.radioRight.Text = "Dock Right";
			this.radioRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioRight.UseVisualStyleBackColor = true;
			// 
			// radioBottom
			// 
			this.radioBottom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioBottom.Location = new System.Drawing.Point(185, 38);
			this.radioBottom.Name = "radioBottom";
			this.radioBottom.Size = new System.Drawing.Size(96, 17);
			this.radioBottom.TabIndex = 190;
			this.radioBottom.TabStop = true;
			this.radioBottom.Text = "Dock Bottom";
			this.radioBottom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioBottom.UseVisualStyleBackColor = true;
			// 
			// validNumY
			// 
			this.validNumY.Location = new System.Drawing.Point(234, 59);
			this.validNumY.MaxLength = 4;
			this.validNumY.MaxVal = 1200;
			this.validNumY.MinVal = 300;
			this.validNumY.Name = "validNumY";
			this.validNumY.Size = new System.Drawing.Size(47, 20);
			this.validNumY.TabIndex = 188;
			this.validNumY.Text = "542";
			this.validNumY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelY
			// 
			this.labelY.Location = new System.Drawing.Point(166, 59);
			this.labelY.Name = "labelY";
			this.labelY.Size = new System.Drawing.Size(62, 18);
			this.labelY.TabIndex = 189;
			this.labelY.Text = "Y Default";
			this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// validNumX
			// 
			this.validNumX.Location = new System.Drawing.Point(114, 59);
			this.validNumX.MaxLength = 4;
			this.validNumX.MaxVal = 2000;
			this.validNumX.MinVal = 300;
			this.validNumX.Name = "validNumX";
			this.validNumX.Size = new System.Drawing.Size(47, 20);
			this.validNumX.TabIndex = 186;
			this.validNumX.Text = "542";
			this.validNumX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelX
			// 
			this.labelX.Location = new System.Drawing.Point(46, 59);
			this.labelX.Name = "labelX";
			this.labelX.Size = new System.Drawing.Size(62, 18);
			this.labelX.TabIndex = 187;
			this.labelX.Text = "X Default";
			this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTasksCheckOnStartup
			// 
			this.checkTasksCheckOnStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTasksCheckOnStartup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTasksCheckOnStartup.Location = new System.Drawing.Point(118, 12);
			this.checkTasksCheckOnStartup.Name = "checkTasksCheckOnStartup";
			this.checkTasksCheckOnStartup.Size = new System.Drawing.Size(230, 19);
			this.checkTasksCheckOnStartup.TabIndex = 75;
			this.checkTasksCheckOnStartup.Text = "Check for new user tasks on startup";
			this.checkTasksCheckOnStartup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(59, 272);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(298, 17);
			this.label4.TabIndex = 64;
			this.label4.Text = "Languages used by patients.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(41, 196);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(333, 35);
			this.label3.TabIndex = 56;
			this.label3.Text = "Process signal interval in seconds.  Usually every 6 to 20 seconds.\r\nLeave blank " +
    "to disable autorefresh";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDecimal
			// 
			this.butDecimal.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDecimal.Autosize = false;
			this.butDecimal.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDecimal.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDecimal.CornerRadius = 2F;
			this.butDecimal.Location = new System.Drawing.Point(455, 322);
			this.butDecimal.Name = "butDecimal";
			this.butDecimal.Size = new System.Drawing.Size(23, 21);
			this.butDecimal.TabIndex = 219;
			this.butDecimal.Text = "...";
			this.butDecimal.Click += new System.EventHandler(this.butDecimal_Click);
			// 
			// butPickLanguageAndRegion
			// 
			this.butPickLanguageAndRegion.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickLanguageAndRegion.Autosize = false;
			this.butPickLanguageAndRegion.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickLanguageAndRegion.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickLanguageAndRegion.CornerRadius = 2F;
			this.butPickLanguageAndRegion.Location = new System.Drawing.Point(455, 296);
			this.butPickLanguageAndRegion.Name = "butPickLanguageAndRegion";
			this.butPickLanguageAndRegion.Size = new System.Drawing.Size(23, 21);
			this.butPickLanguageAndRegion.TabIndex = 206;
			this.butPickLanguageAndRegion.Text = "...";
			this.butPickLanguageAndRegion.Click += new System.EventHandler(this.butPickLanguageAndRegion_Click);
			// 
			// textInactiveSignal
			// 
			this.textInactiveSignal.Location = new System.Drawing.Point(375, 236);
			this.textInactiveSignal.MaxVal = 1000000;
			this.textInactiveSignal.MinVal = 1;
			this.textInactiveSignal.Name = "textInactiveSignal";
			this.textInactiveSignal.Size = new System.Drawing.Size(74, 20);
			this.textInactiveSignal.TabIndex = 201;
			this.textInactiveSignal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butLanguages
			// 
			this.butLanguages.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLanguages.Autosize = true;
			this.butLanguages.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLanguages.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLanguages.CornerRadius = 4F;
			this.butLanguages.Location = new System.Drawing.Point(360, 267);
			this.butLanguages.Name = "butLanguages";
			this.butLanguages.Size = new System.Drawing.Size(88, 24);
			this.butLanguages.TabIndex = 63;
			this.butLanguages.Text = "Edit Languages";
			this.butLanguages.Click += new System.EventHandler(this.butLanguages_Click);
			// 
			// textSigInterval
			// 
			this.textSigInterval.Location = new System.Drawing.Point(375, 201);
			this.textSigInterval.MaxVal = 1000000;
			this.textSigInterval.MinVal = 1;
			this.textSigInterval.Name = "textSigInterval";
			this.textSigInterval.Size = new System.Drawing.Size(74, 20);
			this.textSigInterval.TabIndex = 57;
			this.textSigInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(490, 579);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(490, 541);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormMisc
			// 
			this.ClientSize = new System.Drawing.Size(582, 616);
			this.Controls.Add(this.butDecimal);
			this.Controls.Add(this.textNumDecimals);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboTrackClinic);
			this.Controls.Add(this.labelTrackClinic);
			this.Controls.Add(this.checkTimeCardUseLocal);
			this.Controls.Add(this.butPickLanguageAndRegion);
			this.Controls.Add(this.textLanguageAndRegion);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.checkImeCompositionCompatibility);
			this.Controls.Add(this.checkRefresh);
			this.Controls.Add(this.checkPrefFName);
			this.Controls.Add(this.textInactiveSignal);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkColorTheme);
			this.Controls.Add(this.textWebServiceServerName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butLanguages);
			this.Controls.Add(this.textSigInterval);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMisc";
			this.ShowInTaskbar = false;
			this.Text = "Miscellaneous Setup";
			this.Load += new System.EventHandler(this.FormMisc_Load);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBoxTaskDefaults.ResumeLayout(false);
			this.groupBoxTaskDefaults.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormMisc_Load(object sender, System.EventArgs e) {
			if(PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs)==0){
				textSigInterval.Text="";
			}
			else{
				textSigInterval.Text=PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs).ToString();
			}
			if(PrefC.GetLong(PrefName.SignalInactiveMinutes)==0) {
				textInactiveSignal.Text="";
			}
			else {
				textInactiveSignal.Text=PrefC.GetLong(PrefName.SignalInactiveMinutes).ToString();
			}
			checkColorTheme.Checked=PrefC.GetBool(PrefName.ColorTheme);
      checkTimeCardUseLocal.Checked=PrefC.GetBool(PrefName.LocalTimeOverridesServerTime);
			checkRefresh.Checked=!PrefC.GetBool(PrefName.PatientSelectUsesSearchButton);
			checkPrefFName.Checked=PrefC.GetBool(PrefName.PatientSelectUseFNameForPreferred);
			textMainWindowTitle.Text=PrefC.GetString(PrefName.MainWindowTitle);
			comboShowID.Items.Add(Lan.g(this,"None"));
			comboShowID.Items.Add(Lan.g(this,"PatNum"));
			comboShowID.Items.Add(Lan.g(this,"ChartNumber"));
			comboShowID.Items.Add(Lan.g(this,"Birthdate"));
			comboShowID.SelectedIndex=PrefC.GetInt(PrefName.ShowIDinTitleBar);
			checkTasksCheckOnStartup.Checked=PrefC.GetBool(PrefName.TasksCheckOnStartup);
			checkTaskListAlwaysShow.Checked=PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom);
			checkImeCompositionCompatibility.Checked=PrefC.GetBool(PrefName.ImeCompositionCompatibility);
			if(checkTaskListAlwaysShow.Checked) {
				groupBoxTaskDefaults.Enabled=true;
			}
			else {
				groupBoxTaskDefaults.Enabled=false;
			}
			checkTasksNewTrackedByUser.Checked=PrefC.GetBool(PrefName.TasksNewTrackedByUser);
			checkShowOpenTickets.Checked=PrefC.GetBool(PrefName.TasksShowOpenTickets);
			//computerPref=ComputerPrefs.GetForLocalComputer();
			checkBoxTaskKeepListHidden.Checked=ComputerPrefs.LocalComputer.TaskKeepListHidden;
			if(ComputerPrefs.LocalComputer.TaskDock==0) {
				radioBottom.Checked=true;
			}
			else {
				radioRight.Checked=true;
			}
			validNumX.Text=ComputerPrefs.LocalComputer.TaskX.ToString();
			validNumY.Text=ComputerPrefs.LocalComputer.TaskY.ToString();
			checkTitleBarShowSite.Checked=PrefC.GetBool(PrefName.TitleBarShowSite);
			textWebServiceServerName.Text=PrefC.GetString(PrefName.WebServiceServerName);
			if(PrefC.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text=Lan.g(this,"None");
			}
			textNumDecimals.Text=CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
			_trackLastClinicBy=new List<string> { "None","Workstation","User" };//must be in english because these values are stored in DB.
			_trackLastClinicBy.ForEach(x => comboTrackClinic.Items.Add(Lan.g(this,x)));//translation is for display only.
			comboTrackClinic.SelectedIndex=_trackLastClinicBy.FindIndex(x => x==PrefC.GetString(PrefName.ClinicTrackLast));
			if(comboTrackClinic.SelectedIndex==-1) {
				comboTrackClinic.SelectedIndex=0;
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelTrackClinic.Visible=false;
				comboTrackClinic.Visible=false;
			}
		}

		private void butLanguages_Click(object sender,EventArgs e) {
			FormLanguagesUsed FormL=new FormLanguagesUsed();
			FormL.ShowDialog();
			if(FormL.DialogResult==DialogResult.OK){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butPickLanguageAndRegion_Click(object sender,EventArgs e) {
			FormLanguageAndRegion FormLAR=new FormLanguageAndRegion();//FormLanguageAndRegion saves pref to DB.
			FormLAR.ShowDialog();
			if(PrefC.GetString(PrefName.LanguageAndRegion)!="") {
				textLanguageAndRegion.Text=PrefC.GetLanguageAndRegion().DisplayName;
			}
			else {
				textLanguageAndRegion.Text=Lan.g(this,"None");
			}
		}

		private void butDecimal_Click(object sender,EventArgs e) {
			FormDecimalSettings FormDS=new FormDecimalSettings();
			FormDS.ShowDialog();
		}

		private void checkTaskListAlwaysShow_CheckedChanged(object sender,EventArgs e) {
			if(checkTaskListAlwaysShow.Checked) {
				groupBoxTaskDefaults.Enabled=true;
			}
			else {
				groupBoxTaskDefaults.Enabled=false;
			}
		}

		private void checkBoxTaskKeepListHidden_CheckedChanged(object sender,EventArgs e) {
			if(checkBoxTaskKeepListHidden.Checked) {
				radioBottom.Enabled=false;
				radioRight.Enabled=false;
				labelX.Enabled=false;
				labelY.Enabled=false;
				validNumX.Enabled=false;
				validNumY.Enabled=false;
			}
			else {
				radioBottom.Enabled=true;
				radioRight.Enabled=true;
				labelX.Enabled=true;
				labelY.Enabled=true;
				validNumX.Enabled=true;
				validNumY.Enabled=true;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if( validNumX.errorProvider1.GetError(validNumX)!=""
				| validNumY.errorProvider1.GetError(validNumY)!="")
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(PIn.Long(textSigInterval.Text)>=(5+(PIn.Long(textInactiveSignal.Text)*60)) && PIn.Long(textInactiveSignal.Text)!=0) {//Signal Refresh time is less than or equal to 5 seconds plus the number of seconds in textSigInterval
				string question=Lans.g(this,"The inactive signal time is less than or equal to the signal refresh time.")+"\r\n"
					+Lans.g(this,"This could inadvertently cause signals to not correctly refresh.  Continue?");
				if(MessageBox.Show(question,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
					return;
				}
			}
			if(comboTrackClinic.SelectedIndex<0) {
				comboTrackClinic.SelectedIndex=0;
			}
			bool changed=false;
			if( Prefs.UpdateString(PrefName.MainWindowTitle,textMainWindowTitle.Text)
				| Prefs.UpdateLong(PrefName.ShowIDinTitleBar,comboShowID.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TaskListAlwaysShowsAtBottom, checkTaskListAlwaysShow.Checked)
				| Prefs.UpdateBool(PrefName.TasksCheckOnStartup, checkTasksCheckOnStartup.Checked)
				| Prefs.UpdateBool(PrefName.TasksNewTrackedByUser, checkTasksNewTrackedByUser.Checked)
				| Prefs.UpdateBool(PrefName.TasksShowOpenTickets,checkShowOpenTickets.Checked)
				| Prefs.UpdateBool(PrefName.TitleBarShowSite, checkTitleBarShowSite.Checked)
				| Prefs.UpdateString(PrefName.WebServiceServerName,textWebServiceServerName.Text)
        | Prefs.UpdateBool(PrefName.LocalTimeOverridesServerTime,checkTimeCardUseLocal.Checked)
				| Prefs.UpdateBool(PrefName.PatientSelectUseFNameForPreferred,checkPrefFName.Checked)
				| Prefs.UpdateBool(PrefName.PatientSelectUsesSearchButton,!checkRefresh.Checked)
				| Prefs.UpdateBool(PrefName.ImeCompositionCompatibility,checkImeCompositionCompatibility.Checked)
				| Prefs.UpdateString(PrefName.ClinicTrackLast,_trackLastClinicBy[comboTrackClinic.SelectedIndex])
				)
			{
				changed=true;
			}
			if(Prefs.UpdateBool(PrefName.ColorTheme,checkColorTheme.Checked)) {
				changed=true;
				OpenDental.UI.ODToolBar.UseBlueTheme=checkColorTheme.Checked;
				OpenDental.UI.ODGrid.UseBlueTheme=checkColorTheme.Checked;
			}
			//task list------------------------------------------------------------------------------------------
			if(ComputerPrefs.LocalComputer.TaskKeepListHidden!=checkBoxTaskKeepListHidden.Checked) {
				ComputerPrefs.LocalComputer.TaskKeepListHidden=checkBoxTaskKeepListHidden.Checked;
				changed=true;//needed to trigger screen refresh
			}
			if(radioBottom.Checked && ComputerPrefs.LocalComputer.TaskDock!=0) {
				ComputerPrefs.LocalComputer.TaskDock=0;
				changed=true;
			}
			else if(!radioBottom.Checked && ComputerPrefs.LocalComputer.TaskDock!=1) {
				ComputerPrefs.LocalComputer.TaskDock=1;
				changed=true;
			}
			if(ComputerPrefs.LocalComputer.TaskX!=PIn.Int(validNumX.Text)) {
				ComputerPrefs.LocalComputer.TaskX=PIn.Int(validNumX.Text);
				changed=true;
			}
			if(ComputerPrefs.LocalComputer.TaskY!=PIn.Int(validNumY.Text)) {
				ComputerPrefs.LocalComputer.TaskY=PIn.Int(validNumY.Text);
				changed=true;
			}
			//end of tasklist section-----------------------------------------------------------------------------
			if(textSigInterval.Text==""){
				if(Prefs.UpdateLong(PrefName.ProcessSigsIntervalInSecs,0)){
					changed=true;
				}
			}
			else{
				if(Prefs.UpdateLong(PrefName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))){
					changed=true;
				}
			}
			if(textInactiveSignal.Text=="") {
				if(Prefs.UpdateLong(PrefName.SignalInactiveMinutes,0)) {
					changed=true;
				}
			}
			else {
				if(Prefs.UpdateLong(PrefName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))) {
					changed=true;
				}
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs, InvalidType.Computers);
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}





