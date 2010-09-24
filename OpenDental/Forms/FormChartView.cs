using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormChartView : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.ODGrid gridMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDefault;
		private Label label2;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private ListBox listAvailable;
		private Label label3;
		private OpenDental.UI.Button butRight;
		private OpenDental.UI.Button butLeft;
		private bool changed;
		private OpenDental.UI.Button butOK;
		private List<DisplayField> ListShowing;
		private GroupBox groupBox7;
		private CheckBox checkSheets;
		private CheckBox checkTasks;
		private CheckBox checkEmail;
		private CheckBox checkCommFamily;
		private CheckBox checkAppt;
		private CheckBox checkLabCase;
		private CheckBox checkRx;
		private CheckBox checkComm;
		private GroupBox groupBox6;
		private CheckBox checkShowCn;
		private CheckBox checkShowE;
		private CheckBox checkShowR;
		private CheckBox checkShowC;
		private CheckBox checkShowTP;
		private CheckBox checkShowTeeth;
		private CheckBox checkNotes;
		private CheckBox checkAudit;
		private OpenDental.UI.Button butShowAll;
		private OpenDental.UI.Button butShowNone;
		private TextBox textBoxViewDesc;
		private GroupBox groupBoxProperties;
		private OpenDental.UI.Button butDelete;
		private Label labelDescription;
		private ListBox listProcStatusCodes;
		public ChartView ChartViewCur;

		///<summary></summary>
		public FormChartView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChartView));
			this.label2 = new System.Windows.Forms.Label();
			this.listAvailable = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.checkSheets = new System.Windows.Forms.CheckBox();
			this.checkTasks = new System.Windows.Forms.CheckBox();
			this.checkEmail = new System.Windows.Forms.CheckBox();
			this.checkCommFamily = new System.Windows.Forms.CheckBox();
			this.checkAppt = new System.Windows.Forms.CheckBox();
			this.checkLabCase = new System.Windows.Forms.CheckBox();
			this.checkRx = new System.Windows.Forms.CheckBox();
			this.checkComm = new System.Windows.Forms.CheckBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.checkShowCn = new System.Windows.Forms.CheckBox();
			this.checkShowE = new System.Windows.Forms.CheckBox();
			this.checkShowR = new System.Windows.Forms.CheckBox();
			this.checkShowC = new System.Windows.Forms.CheckBox();
			this.checkShowTP = new System.Windows.Forms.CheckBox();
			this.checkShowTeeth = new System.Windows.Forms.CheckBox();
			this.checkNotes = new System.Windows.Forms.CheckBox();
			this.checkAudit = new System.Windows.Forms.CheckBox();
			this.textBoxViewDesc = new System.Windows.Forms.TextBox();
			this.groupBoxProperties = new System.Windows.Forms.GroupBox();
			this.butShowNone = new OpenDental.UI.Button();
			this.butShowAll = new OpenDental.UI.Button();
			this.labelDescription = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butDefault = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butCancel = new OpenDental.UI.Button();
			this.listProcStatusCodes = new System.Windows.Forms.ListBox();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBoxProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(126,241);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(213,16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Sets entire list to the default.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listAvailable
			// 
			this.listAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listAvailable.FormattingEnabled = true;
			this.listAvailable.IntegralHeight = false;
			this.listAvailable.Location = new System.Drawing.Point(388,277);
			this.listAvailable.Name = "listAvailable";
			this.listAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listAvailable.Size = new System.Drawing.Size(158,361);
			this.listAvailable.TabIndex = 15;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(385,257);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(158,17);
			this.label3.TabIndex = 16;
			this.label3.Text = "Available Fields";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
			this.groupBox7.Location = new System.Drawing.Point(145,13);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(125,148);
			this.groupBox7.TabIndex = 64;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Object Types";
			// 
			// checkSheets
			// 
			this.checkSheets.Checked = true;
			this.checkSheets.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSheets.Location = new System.Drawing.Point(10,130);
			this.checkSheets.Name = "checkSheets";
			this.checkSheets.Size = new System.Drawing.Size(102,13);
			this.checkSheets.TabIndex = 219;
			this.checkSheets.Text = "Sheets";
			this.checkSheets.Click += new System.EventHandler(this.checkSheets_Click);
			// 
			// checkTasks
			// 
			this.checkTasks.Checked = true;
			this.checkTasks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkTasks.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTasks.Location = new System.Drawing.Point(10,66);
			this.checkTasks.Name = "checkTasks";
			this.checkTasks.Size = new System.Drawing.Size(102,13);
			this.checkTasks.TabIndex = 218;
			this.checkTasks.Text = "Tasks";
			this.checkTasks.Click += new System.EventHandler(this.checkTasks_Click);
			// 
			// checkEmail
			// 
			this.checkEmail.Checked = true;
			this.checkEmail.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEmail.Location = new System.Drawing.Point(10,82);
			this.checkEmail.Name = "checkEmail";
			this.checkEmail.Size = new System.Drawing.Size(102,13);
			this.checkEmail.TabIndex = 217;
			this.checkEmail.Text = "Email";
			this.checkEmail.Click += new System.EventHandler(this.checkEmail_Click);
			// 
			// checkCommFamily
			// 
			this.checkCommFamily.Checked = true;
			this.checkCommFamily.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkCommFamily.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCommFamily.Location = new System.Drawing.Point(26,49);
			this.checkCommFamily.Name = "checkCommFamily";
			this.checkCommFamily.Size = new System.Drawing.Size(88,13);
			this.checkCommFamily.TabIndex = 20;
			this.checkCommFamily.Text = "Family";
			this.checkCommFamily.Click += new System.EventHandler(this.checkCommFamily_Click);
			// 
			// checkAppt
			// 
			this.checkAppt.Checked = true;
			this.checkAppt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAppt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppt.Location = new System.Drawing.Point(10,17);
			this.checkAppt.Name = "checkAppt";
			this.checkAppt.Size = new System.Drawing.Size(102,13);
			this.checkAppt.TabIndex = 20;
			this.checkAppt.Text = "Appointments";
			this.checkAppt.Click += new System.EventHandler(this.checkAppt_Click);
			// 
			// checkLabCase
			// 
			this.checkLabCase.Checked = true;
			this.checkLabCase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkLabCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkLabCase.Location = new System.Drawing.Point(10,98);
			this.checkLabCase.Name = "checkLabCase";
			this.checkLabCase.Size = new System.Drawing.Size(102,13);
			this.checkLabCase.TabIndex = 17;
			this.checkLabCase.Text = "Lab Cases";
			this.checkLabCase.Click += new System.EventHandler(this.checkLabCase_Click);
			// 
			// checkRx
			// 
			this.checkRx.Checked = true;
			this.checkRx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRx.Location = new System.Drawing.Point(10,114);
			this.checkRx.Name = "checkRx";
			this.checkRx.Size = new System.Drawing.Size(102,13);
			this.checkRx.TabIndex = 8;
			this.checkRx.Text = "Rx";
			this.checkRx.Click += new System.EventHandler(this.checkRx_Click);
			// 
			// checkComm
			// 
			this.checkComm.Checked = true;
			this.checkComm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkComm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkComm.Location = new System.Drawing.Point(10,33);
			this.checkComm.Name = "checkComm";
			this.checkComm.Size = new System.Drawing.Size(102,13);
			this.checkComm.TabIndex = 16;
			this.checkComm.Text = "Comm Log";
			this.checkComm.Click += new System.EventHandler(this.checkComm_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.checkShowCn);
			this.groupBox6.Controls.Add(this.checkShowE);
			this.groupBox6.Controls.Add(this.checkShowR);
			this.groupBox6.Controls.Add(this.checkShowC);
			this.groupBox6.Controls.Add(this.checkShowTP);
			this.groupBox6.Location = new System.Drawing.Point(6,13);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(121,99);
			this.groupBox6.TabIndex = 63;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Procedures";
			// 
			// checkShowCn
			// 
			this.checkShowCn.Checked = true;
			this.checkShowCn.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowCn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCn.Location = new System.Drawing.Point(9,81);
			this.checkShowCn.Name = "checkShowCn";
			this.checkShowCn.Size = new System.Drawing.Size(101,13);
			this.checkShowCn.TabIndex = 15;
			this.checkShowCn.Text = "Conditions";
			this.checkShowCn.Click += new System.EventHandler(this.checkShowCn_Click);
			// 
			// checkShowE
			// 
			this.checkShowE.Checked = true;
			this.checkShowE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowE.Location = new System.Drawing.Point(9,49);
			this.checkShowE.Name = "checkShowE";
			this.checkShowE.Size = new System.Drawing.Size(101,13);
			this.checkShowE.TabIndex = 10;
			this.checkShowE.Text = "Existing";
			this.checkShowE.Click += new System.EventHandler(this.checkShowE_Click);
			// 
			// checkShowR
			// 
			this.checkShowR.Checked = true;
			this.checkShowR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowR.Location = new System.Drawing.Point(9,65);
			this.checkShowR.Name = "checkShowR";
			this.checkShowR.Size = new System.Drawing.Size(101,13);
			this.checkShowR.TabIndex = 14;
			this.checkShowR.Text = "Referred";
			this.checkShowR.Click += new System.EventHandler(this.checkShowR_Click);
			// 
			// checkShowC
			// 
			this.checkShowC.Checked = true;
			this.checkShowC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowC.Location = new System.Drawing.Point(9,33);
			this.checkShowC.Name = "checkShowC";
			this.checkShowC.Size = new System.Drawing.Size(101,13);
			this.checkShowC.TabIndex = 9;
			this.checkShowC.Text = "Completed";
			this.checkShowC.Click += new System.EventHandler(this.checkShowC_Click);
			// 
			// checkShowTP
			// 
			this.checkShowTP.Checked = true;
			this.checkShowTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTP.Location = new System.Drawing.Point(9,17);
			this.checkShowTP.Name = "checkShowTP";
			this.checkShowTP.Size = new System.Drawing.Size(101,13);
			this.checkShowTP.TabIndex = 8;
			this.checkShowTP.Text = "Treat Plan";
			this.checkShowTP.Click += new System.EventHandler(this.checkShowTP_Click);
			// 
			// checkShowTeeth
			// 
			this.checkShowTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTeeth.Location = new System.Drawing.Point(155,163);
			this.checkShowTeeth.Name = "checkShowTeeth";
			this.checkShowTeeth.Size = new System.Drawing.Size(99,13);
			this.checkShowTeeth.TabIndex = 61;
			this.checkShowTeeth.Text = "Selected Teeth";
			this.checkShowTeeth.Click += new System.EventHandler(this.checkShowTeeth_Click);
			// 
			// checkNotes
			// 
			this.checkNotes.AllowDrop = true;
			this.checkNotes.Checked = true;
			this.checkNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotes.Location = new System.Drawing.Point(15,114);
			this.checkNotes.Name = "checkNotes";
			this.checkNotes.Size = new System.Drawing.Size(102,13);
			this.checkNotes.TabIndex = 58;
			this.checkNotes.Text = "Proc Notes";
			this.checkNotes.Click += new System.EventHandler(this.checkNotes_Click);
			// 
			// checkAudit
			// 
			this.checkAudit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAudit.Location = new System.Drawing.Point(155,179);
			this.checkAudit.Name = "checkAudit";
			this.checkAudit.Size = new System.Drawing.Size(73,13);
			this.checkAudit.TabIndex = 62;
			this.checkAudit.Text = "Audit";
			this.checkAudit.Click += new System.EventHandler(this.checkAudit_Click);
			// 
			// textBoxViewDesc
			// 
			this.textBoxViewDesc.Location = new System.Drawing.Point(134,11);
			this.textBoxViewDesc.Name = "textBoxViewDesc";
			this.textBoxViewDesc.Size = new System.Drawing.Size(367,20);
			this.textBoxViewDesc.TabIndex = 1;
			// 
			// groupBoxProperties
			// 
			this.groupBoxProperties.Controls.Add(this.groupBox6);
			this.groupBoxProperties.Controls.Add(this.butShowNone);
			this.groupBoxProperties.Controls.Add(this.butShowAll);
			this.groupBoxProperties.Controls.Add(this.checkAudit);
			this.groupBoxProperties.Controls.Add(this.groupBox7);
			this.groupBoxProperties.Controls.Add(this.checkNotes);
			this.groupBoxProperties.Controls.Add(this.checkShowTeeth);
			this.groupBoxProperties.Location = new System.Drawing.Point(27,31);
			this.groupBoxProperties.Name = "groupBoxProperties";
			this.groupBoxProperties.Size = new System.Drawing.Size(277,197);
			this.groupBoxProperties.TabIndex = 67;
			this.groupBoxProperties.TabStop = false;
			// 
			// butShowNone
			// 
			this.butShowNone.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShowNone.Autosize = true;
			this.butShowNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowNone.CornerRadius = 4F;
			this.butShowNone.Location = new System.Drawing.Point(69,138);
			this.butShowNone.Name = "butShowNone";
			this.butShowNone.Size = new System.Drawing.Size(58,23);
			this.butShowNone.TabIndex = 60;
			this.butShowNone.Text = "None";
			this.butShowNone.Click += new System.EventHandler(this.butShowNone_Click);
			// 
			// butShowAll
			// 
			this.butShowAll.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShowAll.Autosize = true;
			this.butShowAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowAll.CornerRadius = 4F;
			this.butShowAll.Location = new System.Drawing.Point(10,138);
			this.butShowAll.Name = "butShowAll";
			this.butShowAll.Size = new System.Drawing.Size(53,23);
			this.butShowAll.TabIndex = 59;
			this.butShowAll.Text = "All";
			this.butShowAll.Click += new System.EventHandler(this.butShowAll_Click);
			// 
			// labelDescription
			// 
			this.labelDescription.Location = new System.Drawing.Point(20,12);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(108,17);
			this.labelDescription.TabIndex = 69;
			this.labelDescription.Text = "Description";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(584,522);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75,24);
			this.butDelete.TabIndex = 68;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(584,614);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 56;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(335,397);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35,24);
			this.butRight.TabIndex = 55;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1,0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(335,357);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35,24);
			this.butLeft.TabIndex = 54;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(124,648);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82,24);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0,1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(27,648);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82,24);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDefault
			// 
			this.butDefault.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDefault.Autosize = true;
			this.butDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefault.CornerRadius = 4F;
			this.butDefault.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDefault.Location = new System.Drawing.Point(27,235);
			this.butDefault.Name = "butDefault";
			this.butDefault.Size = new System.Drawing.Size(91,24);
			this.butDefault.TabIndex = 4;
			this.butDefault.Text = "Set to Default";
			this.butDefault.Click += new System.EventHandler(this.butDefault_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(27,264);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292,374);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = "Fields Showing";
			this.gridMain.TranslationName = "FormDisplayFields";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(584,648);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 57;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// listProcStatusCodes
			// 
			this.listProcStatusCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listProcStatusCodes.FormattingEnabled = true;
			this.listProcStatusCodes.IntegralHeight = false;
			this.listProcStatusCodes.Location = new System.Drawing.Point(388,61);
			this.listProcStatusCodes.Name = "listProcStatusCodes";
			this.listProcStatusCodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProcStatusCodes.Size = new System.Drawing.Size(158,162);
			this.listProcStatusCodes.TabIndex = 70;
			this.listProcStatusCodes.Visible = false;
			// 
			// FormChartView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(683,696);
			this.Controls.Add(this.listProcStatusCodes);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.groupBoxProperties);
			this.Controls.Add(this.textBoxViewDesc);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listAvailable);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDefault);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormChartView";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chart View Edit";
			this.Load += new System.EventHandler(this.FormChartView_Load);
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBoxProperties.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormChartView_Load(object sender,EventArgs e) {
			if(!ChartViewCur.IsNew) {
				textBoxViewDesc.Text=ChartViewCur.Description;
			}
			else {
				ChartViewCur.ItemOrder+=1;
				textBoxViewDesc.Text=ChartViewCur.ItemOrder.ToString();
			}
			if(Programs.UsingOrion) {
				listProcStatusCodes.Visible=true;
				listProcStatusCodes.Items.AddRange(Enum.GetNames(typeof(OrionStatus)));
			}
			checkAppt.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.Appointments)==ChartViewObjs.Appointments;
			checkComm.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.CommLog)==ChartViewObjs.CommLog;
			checkCommFamily.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.CommLogFamily)==ChartViewObjs.CommLogFamily;
			checkTasks.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.Tasks)==ChartViewObjs.Tasks;
			checkEmail.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.Email)==ChartViewObjs.Email;
			checkLabCase.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.LabCases)==ChartViewObjs.LabCases;
			checkRx.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.Rx)==ChartViewObjs.Rx;
			checkSheets.Checked=(ChartViewCur.ObjectTypes & ChartViewObjs.Sheets)==ChartViewObjs.Sheets;
			checkShowTP.Checked=(ChartViewCur.ProcStatuses & ChartViewProcStat.TP)==ChartViewProcStat.TP;
			checkShowC.Checked=(ChartViewCur.ProcStatuses & ChartViewProcStat.C)==ChartViewProcStat.C;
			checkShowE.Checked=(ChartViewCur.ProcStatuses & ChartViewProcStat.EC)==ChartViewProcStat.EC;
			checkShowR.Checked=(ChartViewCur.ProcStatuses & ChartViewProcStat.R)==ChartViewProcStat.R;
			checkShowCn.Checked=(ChartViewCur.ProcStatuses & ChartViewProcStat.Cn)==ChartViewProcStat.Cn;
			checkShowTeeth.Checked=ChartViewCur.SelectedTeethOnly;
			checkNotes.Checked=ChartViewCur.ShowProcNotes;
			checkAudit.Checked=ChartViewCur.IsAudit;
			DisplayFields.RefreshCache();
			ListShowing=DisplayFields.GetForChartView(ChartViewCur.ChartViewNum);//This will be zero for a new ChartView
			FillGrids();
		}

		private void FillGrids() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormChartView","FieldName"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormChartView","New Descript"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormChartView","Width"),60);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListShowing.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(ListShowing[i].InternalName);
				row.Cells.Add(ListShowing[i].Description);
				row.Cells.Add(ListShowing[i].ColumnWidth.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			List<DisplayField> availList=DisplayFields.GetAllAvailableList(DisplayFieldCategory.None);
			for(int i=0;i<ListShowing.Count;i++) {
				for(int j=0;j<availList.Count;j++) {
					if(ListShowing[i].InternalName==availList[j].InternalName) {
						availList.RemoveAt(j);
						break;
					}
				}
			}
			listAvailable.Items.Clear();
			for(int i=0;i<availList.Count;i++) {
				listAvailable.Items.Add(availList[i]);
			}
		}

		#region Show
		private void checkShowTP_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkShowC_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkShowE_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkShowR_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkShowCn_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkNotes_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkAppt_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkComm_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkCommFamily_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkLabCase_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkRx_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkTasks_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkEmail_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkSheets_Click(object sender,System.EventArgs e) {
			changed=true;
		}

		private void checkShowTeeth_Click(object sender,System.EventArgs e) {
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
			changed=true;
		}

		private void checkAudit_Click(object sender,EventArgs e) {
			changed=true;
		}

		private void butShowAll_Click(object sender,EventArgs e) {
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
			changed=true;
		}

		private void butShowNone_Click(object sender,EventArgs e) {
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
			changed=true;
		}
		#endregion Show

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDisplayFieldEdit formD=new FormDisplayFieldEdit();
			formD.FieldCur=ListShowing[e.Row];
			formD.ShowDialog();
			FillGrids();
			changed=true;
		}

		private void butDefault_Click(object sender,EventArgs e) {
			ListShowing=DisplayFields.GetDefaultList(DisplayFieldCategory.None);
			FillGrids();
			changed=true;
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(listAvailable.SelectedItems.Count==0) {
				MsgBox.Show(this,"Please select an item in the list on the right first.");
				return;
			}
			DisplayField field;
			for(int i=0;i<listAvailable.SelectedItems.Count;i++) {
				field=(DisplayField)listAvailable.SelectedItems[i];
				ListShowing.Add(field);
			}
			FillGrids();
			changed=true;
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid on the left first.");
				return;
			}
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {//go backwards
				ListShowing.RemoveAt(gridMain.SelectedIndices[i]);
			}
			FillGrids();
			changed=true;
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[0]==0) {
				return;
			}
			for(int i=0;i<selected.Length;i++) {
				ListShowing.Reverse(selected[i]-1,2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]-1,true);
			}
			changed=true;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[selected.Length-1]==ListShowing.Count-1) {
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				ListShowing.Reverse(selected[i],2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
			changed=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(ChartViewCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete this chart view?")) {
				return;
			}
			try {
				ChartViews.Delete(ChartViewCur.ChartViewNum);
				DisplayFields.DeleteForChartView(ChartViewCur.ChartViewNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textBoxViewDesc.Text.Trim()=="") {
				MsgBox.Show(this,"View description cannot be blank.");
				return;
			}
			if(ChartViewCur.Description!=textBoxViewDesc.Text) {
				changed=true;
			}
			if(!changed) {
				DialogResult=DialogResult.OK;
				return;
			}
			ChartViewCur.Description=textBoxViewDesc.Text;
			ChartViewCur.ObjectTypes=ChartViewObjs.None;
			if(checkAppt.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.Appointments;
			}
			if(checkCommFamily.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.CommLogFamily;
			}
			if(checkTasks.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.Tasks;
			}
			if(checkEmail.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.Email;
			}
			if(checkLabCase.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.LabCases;
			}
			if(checkRx.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.Rx;
			}
			if(checkComm.Checked) {
			ChartViewCur.ObjectTypes|=ChartViewObjs.CommLog;
			}
			if(checkSheets.Checked) {
				ChartViewCur.ObjectTypes|=ChartViewObjs.Sheets;
			}
			ChartViewCur.ProcStatuses=ChartViewProcStat.None;
			if(checkShowTP.Checked) {
				ChartViewCur.ProcStatuses|=ChartViewProcStat.TP;
			}
			if(checkShowC.Checked) {
				ChartViewCur.ProcStatuses|=ChartViewProcStat.C;
			}
			if(checkShowE.Checked) {
				ChartViewCur.ProcStatuses|=ChartViewProcStat.EC;
			}
			if(checkShowR.Checked) {
				ChartViewCur.ProcStatuses|=ChartViewProcStat.R;
			}
			if(checkShowCn.Checked) {
				ChartViewCur.ProcStatuses|=ChartViewProcStat.Cn;
			}	
			ChartViewCur.SelectedTeethOnly=checkShowTeeth.Checked;
			ChartViewCur.ShowProcNotes=checkNotes.Checked;
			ChartViewCur.IsAudit=checkAudit.Checked;
			if(!ChartViewCur.IsNew) {
				ChartViews.Update(ChartViewCur);
			}
			else {
				ChartViewCur.ItemOrder=-1;
				ChartViews.Insert(ChartViewCur);
			}
			DisplayFields.SaveListForChartView(ListShowing,ChartViewCur.ChartViewNum);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		


		

		

		

		

		

		


	}
}





















