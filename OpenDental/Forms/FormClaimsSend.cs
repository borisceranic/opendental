using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDental.Eclaims;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormClaimsSend:System.Windows.Forms.Form {
		private System.Windows.Forms.Label label6;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ContextMenu contextMenuStatus;
		private OpenDental.UI.ODToolBar ToolBarMain;
		///<summary>final list of eclaims(as Claim.ClaimNum) to send</summary>
		public static ArrayList eClaimList;
		private ODGrid gridMain;
		///<summary>The list of all claims regardless of any filters.  Filled on load.  Make sure to update this list with any updates (e.g. after validating claims)</summary>
		private ClaimSendQueueItem[] _arrayQueueAll;
		///<summary>A sub list of claims that show in the main grid.  This is a filtered list of _listQueueAll and will get refilled every time FillGrid is called.</summary>
		private ClaimSendQueueItem[] _arrayQueueFiltered;
		///<summary></summary>
		public long GotoPatNum;
		private ODGrid gridHistory;
		private MonthCalendar calendarTo;
		private OpenDental.UI.Button butDropTo;
		private OpenDental.UI.Button butDropFrom;
		private MonthCalendar calendarFrom;
		private ValidDate textDateTo;
		private Label label2;
		private ValidDate textDateFrom;
		private Label label1;
		///<summary></summary>
		public long GotoClaimNum;
		private ODToolBar ToolBarHistory;
		private DataTable tableHistory;
		private int pagesPrinted;
		private Panel panelSplitter;
		private Panel panelHistory;
		private Panel panel1;
		private PrintDocument pd2;
		bool MouseIsDownOnSplitter;
		int SplitterOriginalY;
		int OriginalMouseY;
		bool headingPrinted;
		int headingPrintH;
		private ComboBox comboClinic;
		private Label labelClinic;
		private ComboBox comboCustomTracking;
		private Label labelCustomTracking;
		private ComboBoxMulti comboHistoryType;
		private Label label4;
		private ContextMenu contextMenuEclaims;
		private List<EtransType> _listCurEtransTypes;
		private UI.Button butNextUnsent;
		//private ContextMenu contextMenuHist;
		private List<Clinic> _listClinics;
		private UI.Button butWeekPrevious;
		private UI.Button butWeekNext;
		///<summary>Represents the number of unsent claims per clinic. This is a 1:1 list with _listClinics.</summary>
		private List<int> _listNumberOfClaims;
		private delegate void ToolBarClick();

		///<summary></summary>
		public FormClaimsSend(){
			InitializeComponent();
			//tbQueue.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(tbQueue_CellDoubleClicked);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimsSend));
			this.label6 = new System.Windows.Forms.Label();
			this.contextMenuStatus = new System.Windows.Forms.ContextMenu();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panelSplitter = new System.Windows.Forms.Panel();
			this.panelHistory = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.contextMenuEclaims = new System.Windows.Forms.ContextMenu();
			this.comboCustomTracking = new System.Windows.Forms.ComboBox();
			this.labelCustomTracking = new System.Windows.Forms.Label();
			this.butNextUnsent = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butWeekPrevious = new OpenDental.UI.Button();
			this.butWeekNext = new OpenDental.UI.Button();
			this.comboHistoryType = new OpenDental.UI.ComboBoxMulti();
			this.gridHistory = new OpenDental.UI.ODGrid();
			this.ToolBarHistory = new OpenDental.UI.ODToolBar();
			this.butDropTo = new OpenDental.UI.Button();
			this.butDropFrom = new OpenDental.UI.Button();
			this.textDateFrom = new OpenDental.ValidDate();
			this.textDateTo = new OpenDental.ValidDate();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.panelHistory.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(107, -44);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 44);
			this.label6.TabIndex = 21;
			this.label6.Text = "Insurance Claims";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "");
			this.imageList1.Images.SetKeyName(1, "");
			this.imageList1.Images.SetKeyName(2, "");
			this.imageList1.Images.SetKeyName(3, "");
			this.imageList1.Images.SetKeyName(4, "");
			this.imageList1.Images.SetKeyName(5, "");
			this.imageList1.Images.SetKeyName(6, "");
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(232, 29);
			this.calendarTo.MaxSelectionCount = 1;
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 42;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(6, 29);
			this.calendarFrom.MaxSelectionCount = 1;
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 39;
			this.calendarFrom.Visible = false;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(268, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 18);
			this.label2.TabIndex = 36;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 18);
			this.label1.TabIndex = 34;
			this.label1.Text = "From";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// panelSplitter
			// 
			this.panelSplitter.Cursor = System.Windows.Forms.Cursors.SizeNS;
			this.panelSplitter.Location = new System.Drawing.Point(2, 398);
			this.panelSplitter.Name = "panelSplitter";
			this.panelSplitter.Size = new System.Drawing.Size(961, 6);
			this.panelSplitter.TabIndex = 50;
			this.panelSplitter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseDown);
			this.panelSplitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseMove);
			this.panelSplitter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseUp);
			// 
			// panelHistory
			// 
			this.panelHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.panelHistory.Controls.Add(this.butWeekPrevious);
			this.panelHistory.Controls.Add(this.butWeekNext);
			this.panelHistory.Controls.Add(this.label4);
			this.panelHistory.Controls.Add(this.comboHistoryType);
			this.panelHistory.Controls.Add(this.calendarFrom);
			this.panelHistory.Controls.Add(this.label1);
			this.panelHistory.Controls.Add(this.calendarTo);
			this.panelHistory.Controls.Add(this.gridHistory);
			this.panelHistory.Controls.Add(this.panel1);
			this.panelHistory.Controls.Add(this.butDropTo);
			this.panelHistory.Controls.Add(this.butDropFrom);
			this.panelHistory.Controls.Add(this.textDateFrom);
			this.panelHistory.Controls.Add(this.label2);
			this.panelHistory.Controls.Add(this.textDateTo);
			this.panelHistory.Location = new System.Drawing.Point(0, 403);
			this.panelHistory.Name = "panelHistory";
			this.panelHistory.Size = new System.Drawing.Size(972, 286);
			this.panelHistory.TabIndex = 51;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(419, 5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 18);
			this.label4.TabIndex = 47;
			this.label4.Text = "Type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Controls.Add(this.ToolBarHistory);
			this.panel1.Location = new System.Drawing.Point(587, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(376, 27);
			this.panel1.TabIndex = 44;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(74, 26);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(160, 21);
			this.comboClinic.TabIndex = 53;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(7, 29);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(65, 14);
			this.labelClinic.TabIndex = 52;
			this.labelClinic.Text = "Clinic Filter";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// comboCustomTracking
			// 
			this.comboCustomTracking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCustomTracking.Location = new System.Drawing.Point(514, 26);
			this.comboCustomTracking.MaxDropDownItems = 40;
			this.comboCustomTracking.Name = "comboCustomTracking";
			this.comboCustomTracking.Size = new System.Drawing.Size(160, 21);
			this.comboCustomTracking.TabIndex = 55;
			this.comboCustomTracking.SelectionChangeCommitted += new System.EventHandler(this.comboCustomTracking_SelectionChangeCommitted);
			// 
			// labelCustomTracking
			// 
			this.labelCustomTracking.Location = new System.Drawing.Point(370, 29);
			this.labelCustomTracking.Name = "labelCustomTracking";
			this.labelCustomTracking.Size = new System.Drawing.Size(142, 14);
			this.labelCustomTracking.TabIndex = 54;
			this.labelCustomTracking.Text = "Custom Tracking Filter";
			this.labelCustomTracking.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butNextUnsent
			// 
			this.butNextUnsent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNextUnsent.Autosize = true;
			this.butNextUnsent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNextUnsent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNextUnsent.CornerRadius = 4F;
			this.butNextUnsent.Location = new System.Drawing.Point(234, 25);
			this.butNextUnsent.Name = "butNextUnsent";
			this.butNextUnsent.Size = new System.Drawing.Size(74, 23);
			this.butNextUnsent.TabIndex = 56;
			this.butNextUnsent.Text = "Next Unsent";
			this.butNextUnsent.UseVisualStyleBackColor = true;
			this.butNextUnsent.Click += new System.EventHandler(this.butNextUnsent_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(4, 49);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(959, 350);
			this.gridMain.TabIndex = 32;
			this.gridMain.Title = "Claims Waiting to Send";
			this.gridMain.TranslationName = "TableQueue";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butWeekPrevious
			// 
			this.butWeekPrevious.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butWeekPrevious.Autosize = true;
			this.butWeekPrevious.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekPrevious.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekPrevious.CornerRadius = 4F;
			this.butWeekPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekPrevious.Image = ((System.Drawing.Image)(resources.GetObject("butWeekPrevious.Image")));
			this.butWeekPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekPrevious.Location = new System.Drawing.Point(198, 5);
			this.butWeekPrevious.Name = "butWeekPrevious";
			this.butWeekPrevious.Size = new System.Drawing.Size(33, 22);
			this.butWeekPrevious.TabIndex = 57;
			this.butWeekPrevious.Text = "W";
			this.butWeekPrevious.Click += new System.EventHandler(this.butWeekPrevious_Click);
			// 
			// butWeekNext
			// 
			this.butWeekNext.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butWeekNext.Autosize = false;
			this.butWeekNext.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekNext.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekNext.CornerRadius = 4F;
			this.butWeekNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekNext.Image = ((System.Drawing.Image)(resources.GetObject("butWeekNext.Image")));
			this.butWeekNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butWeekNext.Location = new System.Drawing.Point(234, 5);
			this.butWeekNext.Name = "butWeekNext";
			this.butWeekNext.Size = new System.Drawing.Size(33, 22);
			this.butWeekNext.TabIndex = 56;
			this.butWeekNext.Text = "W";
			this.butWeekNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekNext.Click += new System.EventHandler(this.butWeekNext_Click);
			// 
			// comboHistoryType
			// 
			this.comboHistoryType.BackColor = System.Drawing.SystemColors.Window;
			this.comboHistoryType.DroppedDown = false;
			this.comboHistoryType.Items = ((System.Collections.ArrayList)(resources.GetObject("comboHistoryType.Items")));
			this.comboHistoryType.Location = new System.Drawing.Point(465, 6);
			this.comboHistoryType.Name = "comboHistoryType";
			this.comboHistoryType.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboHistoryType.SelectedIndices")));
			this.comboHistoryType.Size = new System.Drawing.Size(100, 21);
			this.comboHistoryType.TabIndex = 45;
			// 
			// gridHistory
			// 
			this.gridHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.gridHistory.HasMultilineHeaders = false;
			this.gridHistory.HScrollVisible = false;
			this.gridHistory.Location = new System.Drawing.Point(4, 31);
			this.gridHistory.Name = "gridHistory";
			this.gridHistory.ScrollValue = 0;
			this.gridHistory.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridHistory.Size = new System.Drawing.Size(959, 252);
			this.gridHistory.TabIndex = 33;
			this.gridHistory.Title = "History";
			this.gridHistory.TranslationName = "TableClaimHistory";
			this.gridHistory.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridHistory_CellDoubleClick);
			// 
			// ToolBarHistory
			// 
			this.ToolBarHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ToolBarHistory.BackColor = System.Drawing.SystemColors.Control;
			this.ToolBarHistory.ImageList = this.imageList1;
			this.ToolBarHistory.Location = new System.Drawing.Point(1, 1);
			this.ToolBarHistory.Name = "ToolBarHistory";
			this.ToolBarHistory.Size = new System.Drawing.Size(375, 25);
			this.ToolBarHistory.TabIndex = 43;
			this.ToolBarHistory.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarHistory_ButtonClick);
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.Location = new System.Drawing.Point(397, 4);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(22, 23);
			this.butDropTo.TabIndex = 41;
			this.butDropTo.Text = "V";
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// butDropFrom
			// 
			this.butDropFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropFrom.Autosize = true;
			this.butDropFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropFrom.CornerRadius = 4F;
			this.butDropFrom.Location = new System.Drawing.Point(157, 4);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(22, 23);
			this.butDropFrom.TabIndex = 40;
			this.butDropFrom.Text = "V";
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(74, 6);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 35;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(314, 6);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 37;
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageList1;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(971, 25);
			this.ToolBarMain.TabIndex = 31;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// FormClaimsSend
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(971, 691);
			this.Controls.Add(this.butNextUnsent);
			this.Controls.Add(this.comboCustomTracking);
			this.Controls.Add(this.labelCustomTracking);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.panelHistory);
			this.Controls.Add(this.panelSplitter);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.label6);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimsSend";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Send Claims";
			this.Load += new System.EventHandler(this.FormClaimsSend_Load);
			this.Resize += new System.EventHandler(this.FormClaimsSend_Resize);
			this.panelHistory.ResumeLayout(false);
			this.panelHistory.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimsSend_Load(object sender, System.EventArgs e) {
			AdjustPanelSplit();
			_arrayQueueAll=Claims.GetQueueList(0,0,0);
			_listNumberOfClaims=new List<int>();
			contextMenuStatus.MenuItems.Add(Lan.g(this,"Go to Account"),new EventHandler(GotoAccount_Clicked));
			gridMain.ContextMenu=contextMenuStatus;
			Clearinghouse[] arrayClearinghouses=Clearinghouses.GetHqListt();
			for(int i=0;i<arrayClearinghouses.Length;i++){
				contextMenuEclaims.MenuItems.Add(arrayClearinghouses[i].Description,new EventHandler(menuItemClearinghouse_Click));
			}
			LayoutToolBars();
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				butNextUnsent.Visible=false;
			}
			else {
				_listClinics=Clinics.GetForUserod(Security.CurUser);
			}
			comboCustomTracking.Items.Add(Lan.g(this,"all"));
			comboCustomTracking.SelectedIndex=0;
			if(DefC.Long[(int)DefCat.ClaimCustomTracking].Length==0){
				labelCustomTracking.Visible=false;
				comboCustomTracking.Visible=false;
			}
			else{
				for(int i=0;i<DefC.Long[(int)DefCat.ClaimCustomTracking].Length;i++) {
					comboCustomTracking.Items.Add(DefC.Long[(int)DefCat.ClaimCustomTracking][i].ItemName);
				}
			}
			if(PrefC.RandomKeys && !PrefC.GetBool(PrefName.EasyNoClinics)){//using random keys and clinics
				//Does not pull in reports automatically, because they could easily get assigned to the wrong clearinghouse
			}
			else{
				FormClaimReports FormC=new FormClaimReports(); //the currently selected clinic is what the combobox defaults to.
				FormC.AutomaticMode=true;
				FormC.ShowDialog();
			}
			FillGrid();
			//Validate all claims if the preference is enabled.
			if(PrefC.GetBool(PrefName.ClaimsSendWindowValidatesOnLoad)) {
				//This can be very slow if there are lots of claims to validate.
				ValidateClaims(_arrayQueueAll.ToList());
			}
			textDateFrom.Text=DateTime.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
			_listCurEtransTypes=new List<EtransType>();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				_listCurEtransTypes.Add(EtransType.ClaimPrinted);
				_listCurEtransTypes.Add(EtransType.Claim_CA);
				_listCurEtransTypes.Add(EtransType.Claim_Ren);
				_listCurEtransTypes.Add(EtransType.Eligibility_CA);
				_listCurEtransTypes.Add(EtransType.ClaimReversal_CA);
				_listCurEtransTypes.Add(EtransType.Predeterm_CA);
				_listCurEtransTypes.Add(EtransType.RequestOutstand_CA);
				_listCurEtransTypes.Add(EtransType.RequestSumm_CA);
				_listCurEtransTypes.Add(EtransType.RequestPay_CA);
				_listCurEtransTypes.Add(EtransType.ClaimCOB_CA);
				_listCurEtransTypes.Add(EtransType.TextReport);
			}
			else {//United States
				_listCurEtransTypes.Add(EtransType.ClaimSent);
				_listCurEtransTypes.Add(EtransType.ClaimPrinted);
				_listCurEtransTypes.Add(EtransType.Claim_Ren);
				_listCurEtransTypes.Add(EtransType.StatusNotify_277);
				_listCurEtransTypes.Add(EtransType.TextReport);
				_listCurEtransTypes.Add(EtransType.ERA_835);
				_listCurEtransTypes.Add(EtransType.Ack_Interchange);
			}
			for(int i=0;i<_listCurEtransTypes.Count;i++) {
				comboHistoryType.Items.Add(_listCurEtransTypes[i].ToString());
				comboHistoryType.SetSelected(i,true);
			}
			FillHistory();
		}

		public void FillClinicsList(long claimCustomTracking) {
			int previousSelection=-1;
			if(comboClinic.SelectedIndex!=-1) {//Only -1 the first time this method is run.
				previousSelection=comboClinic.SelectedIndex;
			}
			comboClinic.Items.Clear();
			_listNumberOfClaims.Clear();
			if(!Security.CurUser.ClinicIsRestricted) {
				comboClinic.Items.Add(Lan.g(this,"Unassigned/Default"));
				comboClinic.SelectedIndex=0;
			}
			for(int i=0;i<_listClinics.Count;i++) {
				_listNumberOfClaims.Add(0);
				for(int j=0;j<_arrayQueueAll.Length;j++) {
					if(_arrayQueueAll[j].ClinicNum==_listClinics[i].ClinicNum) {
						if(claimCustomTracking==0 || _arrayQueueAll[j].CustomTracking==claimCustomTracking) {
							_listNumberOfClaims[i]=_listNumberOfClaims[i]+1;
						}
					}
				}
				int curIndex=comboClinic.Items.Add(_listClinics[i].Description+"  ("+_listNumberOfClaims[i]+")");
				if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
					comboClinic.SelectedIndex=curIndex;
				}
			}
			if(previousSelection!=-1) {
				comboClinic.SelectedIndex=previousSelection;
			}
		}

		///<summary></summary>
		public void LayoutToolBars(){
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Preview"),0,Lan.g(this,"Preview the Selected Claim"),"Preview"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Blank"),1,Lan.g(this,"Print a Blank Claim Form"),"Blank"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),2,Lan.g(this,"Print Selected Claims"),"Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Labels"),6,Lan.g(this,"Print Single Labels"),"Labels"));
			/*ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ODToolBarButton button=new ODToolBarButton(Lan.g(this,"Change Status"),-1,Lan.g(this,"Changes Status of Selected Claims"),"Status");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuStatus;
			ToolBarMain.Buttons.Add(button);*/
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ODToolBarButton button=new ODToolBarButton(Lan.g(this,"Send E-Claims"),4,Lan.g(this,"Send claims Electronically"),"Eclaims");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=contextMenuEclaims;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Validate Claims"),-1,Lan.g(this,"Refresh and Validate Selected Claims"),"Validate"));
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Outstanding"),-1,Lan.g(this,"Get Outstanding Transactions"),"Outstanding"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Payment Rec"),-1,Lan.g(this,"Get Payment Reconciliation Transactions"),"PayRec"));
				//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Summary Rec"),-1,Lan.g(this,"Get Summary Reconciliation Transactions"),"SummaryRec"));
			}
			else {
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Get Reports"),5,Lan.g(this,"Get Reports from Other Clearinghouses"),"Reports"));
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Refresh"),-1,Lan.g(this,"Refresh the Grid"),"Refresh"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"","Close"));
			/*ArrayList toolButItems=ToolButItems.GetForToolBar(ToolBarsAvail.ClaimsSend);
			for(int i=0;i<toolButItems.Count;i++){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				ToolBarMain.Buttons.Add(new ODToolBarButton(((ToolButItem)toolButItems[i]).ButtonText
					,-1,"",((ToolButItem)toolButItems[i]).ProgramNum));
			}*/
			ToolBarMain.Invalidate();
			ToolBarHistory.Buttons.Clear();
			ToolBarHistory.Buttons.Add(new ODToolBarButton(Lan.g(this,"Refresh"),-1,Lan.g(this,"Refresh this list."),"Refresh"));
			ToolBarHistory.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarHistory.Buttons.Add(new ODToolBarButton(Lan.g(this,"Undo"),-1,
				Lan.g(this,"Change the status of claims back to 'Waiting'."),"Undo"));
			ToolBarHistory.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarHistory.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print List"),2,
				Lan.g(this,"Print history list."),"PrintList"));
			/*#if DEBUG
			ToolBarHistory.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print Item"),2,
				Lan.g(this,"For debugging, this will simply display the first item in the list."),"PrintItem"));
			#else
			ToolBarHistory.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print Item"),2,
				Lan.g(this,"Print one item from the list."),"PrintItem"));
			#endif*/
			ToolBarHistory.Invalidate();
		}

		private void FormClaimsSend_Resize(object sender,EventArgs e) {
			AdjustPanelSplit();
		}

		private void GotoAccount_Clicked(object sender, System.EventArgs e){
			//accessed by right clicking
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one item first.");
				return;
			}
			ODEvent.Fire(new ODEventArgs("FormClaimSend_GoTo",_arrayQueueFiltered[gridMain.SelectedIndices[0]]));
			SendToBack();
		}

		private void menuItemClearinghouse_Click(object sender, System.EventArgs e){
			MenuItem menuitem=(MenuItem)sender;
			Clearinghouse[] arrayHqClearinghouses=Clearinghouses.GetHqListt();
			SendEclaimsToClearinghouse(arrayHqClearinghouses[menuitem.Index].ClearinghouseNum);
		}

		private void FillGrid() {
			FillGrid(false);
		}

		private void FillGrid(bool rememberSelection){
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				long claimCustomTracking=0;
				if(comboCustomTracking.SelectedIndex!=0) {
					claimCustomTracking=DefC.GetList(DefCat.ClaimCustomTracking)[comboCustomTracking.SelectedIndex-1].DefNum;
				}
				FillClinicsList(claimCustomTracking);
			}
			int oldScrollValue=0;
			List<long> listOldSelectedClaimNums=new List<long>();
			if(rememberSelection) {
				oldScrollValue=gridMain.ScrollValue;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					listOldSelectedClaimNums.Add(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum);
				}
			}
			ClaimSendQueueItem[] arrayClaimQueue=Claims.GetQueueList(0,0,0);//Get fresh new "all" list from db.
			for(int i=0;i<arrayClaimQueue.Length;i++) {
				//If any data in the new list needs to be refreshed because something changed, refresh it.
				//At this point, _arrayQueueAll is the old list of all claims.
				for(int j=0;j<_arrayQueueAll.Length;j++) {//Go through the old list of all claims.
					if(arrayClaimQueue[i].ClaimNum==_arrayQueueAll[j].ClaimNum) {//The same claim is in both the old and new "all" lists.
						arrayClaimQueue[i]=_arrayQueueAll[j];//Keep the same exact queue item as before so we can maintain the MissingData, etc.
					}
				}
				if(arrayClaimQueue[i].MissingData==null) {//Can only be null if the claim was not in the old "all" list.  For example, on load or when undo.
					arrayClaimQueue[i].MissingData="(validated when sending)";
				}
			}
			_arrayQueueAll=arrayClaimQueue;
			//Get filtered list from list all
			_arrayQueueFiltered=GetListQueueFiltered();//We update the class wide variable because it is used in double clicking and other events.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableQueue","Date Service"),80);//new column
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Patient Name"),120);//was 190
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Carrier Name"),220);//was 100
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Clinic"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","M/D"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Clearinghouse"),80);//5. This is position critical. See SendEclaimsToClearinghouse().
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Warnings"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableQueue","Missing Info"),300);//7. This is position critical.  If this changes, the code below must be updated as well.
			gridMain.Columns.Add(col);			 
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_arrayQueueFiltered.Length;i++){
				row=new ODGridRow();
				row.Cells.Add(_arrayQueueFiltered[i].DateService.ToShortDateString());
				row.Cells.Add(_arrayQueueFiltered[i].PatName);
				row.Cells.Add(_arrayQueueFiltered[i].Carrier);
				row.Cells.Add(Clinics.GetDesc(_arrayQueueFiltered[i].ClinicNum));
				switch(_arrayQueueFiltered[i].MedType){
					case EnumClaimMedType.Dental:
						row.Cells.Add("Dent");
						break;
					case EnumClaimMedType.Medical:
						row.Cells.Add("Med");
						break;
					case EnumClaimMedType.Institutional:
						row.Cells.Add("Inst");
						break;
				}
				if(_arrayQueueFiltered[i].NoSendElect){
					row.Cells.Add("Paper");
					row.Cells.Add("");
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ClearinghouseL.GetDescript(_arrayQueueFiltered[i].ClearinghouseNum));
					row.Cells.Add(_arrayQueueFiltered[i].Warnings);
					row.Cells.Add(_arrayQueueFiltered[i].MissingData);
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=oldScrollValue;
			for(int i=0;i<_arrayQueueFiltered.Length;i++) {
				for(int j=0;j<listOldSelectedClaimNums.Count;j++) {
					if(_arrayQueueFiltered[i].ClaimNum==listOldSelectedClaimNums[j]) {
						gridMain.SetSelected(i,true);
					}
				}
			}
		}

		///<summary>Returns a list of claim send queue items based on the filters.</summary>
		private ClaimSendQueueItem[] GetListQueueFiltered() {
			long clinicNum=0;
			long customTracking=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(comboClinic.SelectedIndex!=0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
				}
				else if(Security.CurUser.ClinicIsRestricted) {
					clinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				}
			}
			if(comboCustomTracking.SelectedIndex!=0) {
				customTracking=DefC.Long[(int)DefCat.ClaimCustomTracking][comboCustomTracking.SelectedIndex-1].DefNum;
			}
			List<ClaimSendQueueItem> listClaimSend=new List<ClaimSendQueueItem>();
			listClaimSend.AddRange(_arrayQueueAll);
			//Remove any non-matches
			//Creating a subset of listClaimSend with all entries c such that c.ClinicNum==clinicNum
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Filter by clinic only when clinics are enabled.
				listClaimSend=listClaimSend.FindAll(c => c.ClinicNum==clinicNum);
			}
			if(customTracking>0) {
				//Creating a subset of listClaimSend with all entries c such that c.CustomTracking==customTracking
				listClaimSend=listClaimSend.FindAll(c => c.CustomTracking==customTracking);
			}
			return listClaimSend.ToArray();
		}

		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e){
			int selected=e.Row;
			FormClaimPrint FormCP;
			FormCP=new FormClaimPrint();
			FormCP.PatNumCur=_arrayQueueFiltered[e.Row].PatNum;
			FormCP.ClaimNumCur=_arrayQueueFiltered[e.Row].ClaimNum;
			FormCP.PrintImmediately=false;
			FormCP.ShowDialog();				
			FillGrid();	
			gridMain.SetSelected(selected,true);
			FillHistory();
		}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			ToolBarClick toolClick;
			switch(e.Button.Tag.ToString()){
				case "Preview":
					toolBarButPreview_Click();
					break;
				case "Blank":
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
					//when it comes from a toolbar click.
					//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
					toolClick=toolBarButBlank_Click;
					this.BeginInvoke(toolClick);
					break;
				case "Print":
					toolClick=toolBarButPrint_Click;
					this.BeginInvoke(toolClick);
					break;
				case "Labels":
					toolClick=toolBarButLabels_Click;
					this.BeginInvoke(toolClick);
					break;
				case "Eclaims":
					SendEclaimsToClearinghouse(0);
					break;
				case "Validate":
					toolBarButValidate_Click();
					break;
				case "Reports":
					toolBarButReports_Click();
					break;
				case "Outstanding":
					toolBarButOutstanding_Click();
					break;
				case "PayRec":
					toolBarButPayRec_Click();
					break;
				case "SummaryRec":
					toolBarButSummaryRec_Click();
					break;
				case "Refresh":
					toolBarButRefresh_Click();
					break;
				case "Close":
					Close();
					break;
			}
		}

		private void toolBarButPreview_Click(){
			FormClaimPrint FormCP;
			FormCP=new FormClaimPrint();
			if(gridMain.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select a claim first."));
				return;
			}
			if(gridMain.SelectedIndices.Length > 1){
				MessageBox.Show(Lan.g(this,"Please select only one claim."));
				return;
			}
			FormCP.PatNumCur=_arrayQueueFiltered[gridMain.GetSelectedIndex()].PatNum;
			FormCP.ClaimNumCur=_arrayQueueFiltered[gridMain.GetSelectedIndex()].ClaimNum;
			FormCP.PrintImmediately=false;
			FormCP.ShowDialog();				
			FillGrid();
			FillHistory();
		}

		private void toolBarButBlank_Click(){
			PrintDocument pd=new PrintDocument();
			if(!PrinterL.SetPrinter(pd,PrintSituation.Claim,0,"Blank claim printed")){
				return;
			}
			FormClaimPrint FormCP=new FormClaimPrint();
			FormCP.PrintBlank=true;
			FormCP.PrintImmediate(pd.PrinterSettings);
		}

		private void toolBarButPrint_Click(){
			FormClaimPrint FormCP=new FormClaimPrint();
			if(gridMain.SelectedIndices.Length==0){
				for(int i=0;i<_arrayQueueFiltered.Length;i++){
					if((_arrayQueueFiltered[i].ClaimStatus=="W" || _arrayQueueFiltered[i].ClaimStatus=="P")
						&& _arrayQueueFiltered[i].NoSendElect)
					{
						gridMain.SetSelected(i,true);		
					}	
				}
				if(!MsgBox.Show(this,true,"No claims were selected.  Print all selected paper claims?")){
					return;
				}
			}
			PrintDocument pd=new PrintDocument();
			if(!PrinterL.SetPrinter(pd,PrintSituation.Claim,0,"Multiple claims printed")){
				return;
			}
			pd.PrinterSettings.Copies=1; //Used to be sent in the FormCP.PrintImmediate function call below.  Moved up here to keep same logic.
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				FormCP.PatNumCur=_arrayQueueFiltered[gridMain.SelectedIndices[i]].PatNum;
				FormCP.ClaimNumCur=_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum;
				FormCP.ClaimFormCur=null;//so that it will pull from the individual claim or plan.
				if(!FormCP.PrintImmediate(pd.PrinterSettings)) {
					return;
				}
				Etranss.SetClaimSentOrPrinted(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum,_arrayQueueFiltered[gridMain.SelectedIndices[i]].PatNum,0,EtransType.ClaimPrinted,0);
			}
			FillGrid();
			FillHistory();
		}

		private void toolBarButLabels_Click(){
			if(gridMain.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select a claim first."));
				return;
			}
			//PrintDocument pd=new PrintDocument();//only used to pass printerName
			//if(!PrinterL.SetPrinter(pd,PrintSituation.LabelSingle)){
			//	return;
			//}
			//Carrier carrier;
			Claim claim;
			InsPlan plan;
			List<long> carrierNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				claim=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum);
				plan=InsPlans.GetPlan(claim.PlanNum,new List <InsPlan> ());
				carrierNums.Add(plan.CarrierNum);
			}
			//carrier=Carriers.GetCarrier(plan.CarrierNum);
			//LabelSingle label=new LabelSingle();
			LabelSingle.PrintCarriers(carrierNums);//,pd.PrinterSettings.PrinterName)){
			//	return;
			//}
		}

		private void toolBarButValidate_Click() {
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show(Lan.g(this,"Please select one or more claims first."));
				return;
			}
			RefreshAndValidateSelections();
		}

		private void toolBarButRefresh_Click() {
			FillGrid(true);
		}

		///<summary>Fills grid with updated information, unless all of the selected claims are marked NoBillIns and none of them were deleted.</summary>
		private void RefreshAndValidateSelections() {
			List<ClaimSendQueueItem> listQueueItems=new List<ClaimSendQueueItem>();//List of claims needing to be validated.
			List<long> listQueueClaimNums=new List<long>();//List of claimNums to fetch new ClaimSendQuiteItems
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				//Create a list of claims so we only call one Claims.GetQueueList.
				listQueueClaimNums.Add(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum);
			}
			ClaimSendQueueItem[] arrayRefreshQueueItems=Claims.GetQueueList(listQueueClaimNums,0,0);
			int claimAlreadySentCount=0;
			for(int j=0;j<arrayRefreshQueueItems.Length;j++) {//Loop through all the refreshed ClaimSendQueueItems
				for(int k=0;k<_arrayQueueAll.Length;k++) {//Loop through all the ClaimSendQueueItems in the grid's main list
					if(arrayRefreshQueueItems[j].ClaimNum==_arrayQueueAll[k].ClaimNum) {//If you found the matching ClaimSendQueueItem
						if(_arrayQueueAll[k].ClaimStatus=="S" ||  _arrayQueueAll[k].ClaimStatus=="R") {
							claimAlreadySentCount++;
						}
						else {
							_arrayQueueAll[k]=arrayRefreshQueueItems[j];//Refresh the claim in the list
							listQueueItems.Add(_arrayQueueAll[k]);//Add to list to be validated again
						}
						break;
					}
				}
			}
			if(claimAlreadySentCount>0) {
				MsgBox.Show(this,"WARNING: Some of the selected claims have already been sent or received.  They will be removed from the grid.");
			}
			if(arrayRefreshQueueItems.Length!=gridMain.SelectedIndices.Length) {
				MsgBox.Show(this,"WARNING: One or more claims were deleted from outside this window.  They will be removed from the grid.");
			}
			if(listQueueItems.Count>0) {//At least one claim still exists
				ValidateClaims(listQueueItems);//Validate refeshed claims, also fills grid
			}
			else {
				FillGrid(true);//Refresh the grid so that the deleted claims disapear.
			}
		}

		///<Summary>Use clearinghouseNum of 0 to indicate automatic calculation of clearinghouses.</Summary>
		private void SendEclaimsToClearinghouse(long hqClearinghouseNum) {
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics is in use
				if(hqClearinghouseNum==0){
					MsgBox.Show(this,"When the Clinics option is enabled, you must use the dropdown list to select the clearinghouse to send to.");
					return;
				}
			}
			Clearinghouse clearDefault;
			if(hqClearinghouseNum==0){
				clearDefault=Clearinghouses.GetDefaultDental();
			}
			else{
				clearDefault=ClearinghouseL.GetClearinghouseHq(hqClearinghouseNum);
			}
			if(clearDefault!=null && clearDefault.ISA08=="113504607" && Process.GetProcessesByName("TesiaLink").Length==0){
				#if DEBUG
					if(!MsgBox.Show(this,true,"TesiaLink is not started.  Create file anyway?")){
						return;
					}
				#else
					MsgBox.Show(this,"Please start TesiaLink first.");
					return;
				#endif
			}
			if(gridMain.SelectedIndices.Length==0){//if none are selected
				for(int i=0;i<_arrayQueueFiltered.Length;i++) {//loop through all rows
					if(!_arrayQueueFiltered[i].NoSendElect) {
						if(hqClearinghouseNum==0) {//they did not use the dropdown list for specific clearinghouse
							//If clearinghouse is zero because they just pushed the button instead of using the dropdown list,
							//then don't check the clearinghouse of each claim.  Just select them if they are electronic.
							gridMain.SetSelected(i,true);
						}
						else {//if they used the dropdown list,
							//then first, try to only select items in the list that match the clearinghouse.
							if(_arrayQueueFiltered[i].ClearinghouseNum==hqClearinghouseNum) {
								gridMain.SetSelected(i,true);
							}
						}
					}
				}
				//If they used the dropdown list, and there still aren't any in the list that match the selected clearinghouse
				//then ask user if they want to send all of the electronic ones through this clearinghouse.
				if(hqClearinghouseNum!=0 && gridMain.SelectedIndices.Length==0) {
					if(comboClinic.SelectedIndex==0) {
						MsgBox.Show(this,"Please filter by clinic first.");
						return;
					}
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Send all e-claims through selected clearinghouse?")) {
						return;
					}
					for(int i=0;i<_arrayQueueFiltered.Length;i++) {
						if(!_arrayQueueFiltered[i].NoSendElect) {//no Missing Info
							gridMain.SetSelected(i,true);//this will include other clearinghouses
						}
					}
				}
				if(gridMain.SelectedIndices.Length==0){//No claims in filtered list
					MsgBox.Show(this,"No claims to send.");
					return;
				}
				if(hqClearinghouseNum!=0){//if they used the dropdown list to specify clearinghouse
					int[] selectedindices=(int[])gridMain.SelectedIndices.Clone();
					for(int i=0;i<selectedindices.Length;i++) {
						Clearinghouse clearRow=Clearinghouses.GetClearinghouse(_arrayQueueFiltered[selectedindices[i]].ClearinghouseNum);
						if(clearDefault.Eformat!=clearRow.Eformat) {
							MsgBox.Show(this,"The default clearinghouse format does not match the format of the selected clearinghouse.  You may need to change the clearinghouse format.  Or, you may need to add a Payor ID into a clearinghouse.");
							return;
						}
						if(!_arrayQueueFiltered[selectedindices[i]].NoSendElect) {//Only change the text to the clearing house name for electronic claims.
							gridMain.Rows[selectedindices[i]].Cells[5].Text=clearDefault.Description;
						}
					}
					FillGrid(true);
				}
				if(!MsgBox.Show(this,true,"Send all selected e-claims?")){
					FillGrid();//this changes back any clearinghouse descriptions that we changed manually.
					return;
				}
			}
			else {//some rows were manually selected by the user
				if(hqClearinghouseNum!=0) {//if they used the dropdown list to specify clearinghouse
					int[] selectedindices=(int[])gridMain.SelectedIndices.Clone();
					for(int i=0;i<selectedindices.Length;i++) {
						Clearinghouse clearRow=Clearinghouses.GetClearinghouse(_arrayQueueFiltered[selectedindices[i]].ClearinghouseNum);
						if(clearDefault.Eformat!=clearRow.Eformat) {
							MsgBox.Show(this,"The default clearinghouse format does not match the format of the selected clearinghouse.  You may need to change the clearinghouse format.  Or, you may need to add a Payor ID into a clearinghouse.");
							return;
						}
						if(!_arrayQueueFiltered[selectedindices[i]].NoSendElect) {//Only change the text to the clearing house name for electronic claims.
							gridMain.Rows[selectedindices[i]].Cells[5].Text=clearDefault.Description;//show the changed clearinghouse
						}
					}
				}
			}
			RefreshAndValidateSelections();
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics is in use
				long clinicNum0=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[0]].ClaimNum).ClinicNum;
				for(int i=1;i<gridMain.SelectedIndices.Length;i++){
					long clinicNum=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum).ClinicNum;
					if(clinicNum0!=clinicNum){
						MsgBox.Show(this,"All claims must be for the same clinic.  You can use the combobox at the top to filter.");
						return;
					}
				}
			}
			long clearinghouseNum0=_arrayQueueFiltered[gridMain.SelectedIndices[0]].ClearinghouseNum;
			EnumClaimMedType medType0=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[0]].ClaimNum).MedType;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {//we start with 0 so that we can check medtype match on the first claim
				long clearinghouseNumI=_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClearinghouseNum;
				if(clearinghouseNum0!=clearinghouseNumI) {
					MsgBox.Show(this,"All claims must be for the same clearinghouse.");
					return;
				}
				EnumClaimMedType medTypeI=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[i]].ClaimNum).MedType;
				if(medType0!=medTypeI) {
					MsgBox.Show(this,"All claims must have the same MedType.");
					return;
				}
				Clearinghouse clearh=Clearinghouses.GetClearinghouse(clearinghouseNumI);
				if(clearh.Eformat==ElectronicClaimFormat.x837D_4010 || clearh.Eformat==ElectronicClaimFormat.x837D_5010_dental) {
					if(medTypeI!=EnumClaimMedType.Dental) {
						MessageBox.Show("On claim "+i.ToString()+", the MedType does not match the clearinghouse e-format.");
						return;
					}
				}
				if(clearh.Eformat==ElectronicClaimFormat.x837_5010_med_inst) {
					if(medTypeI!=EnumClaimMedType.Medical && medTypeI!=EnumClaimMedType.Institutional) {
						MessageBox.Show("On claim "+i.ToString()+", the MedType does not match the clearinghouse e-format.");
						return;
					}
				}
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(!_arrayQueueFiltered[gridMain.SelectedIndices[i]].IsValid && !_arrayQueueFiltered[gridMain.SelectedIndices[i]].NoSendElect){
					MsgBox.Show(this,"Not allowed to send e-claims with missing information.");
					return;
				}
				if(_arrayQueueFiltered[gridMain.SelectedIndices[i]].NoSendElect) {
					MsgBox.Show(this,"Not allowed to send paper claims electronically.");
					return;
				}
			}
			List<ClaimSendQueueItem> queueItems=new List<ClaimSendQueueItem>();//a list of queue items to send
			ClaimSendQueueItem queueitem;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				queueitem=_arrayQueueFiltered[gridMain.SelectedIndices[i]].Copy();
				if(hqClearinghouseNum!=0) {
					queueitem.ClearinghouseNum=hqClearinghouseNum;
				}
				queueItems.Add(queueitem);
			}
			Claim claim0=Claims.GetClaim(_arrayQueueFiltered[gridMain.SelectedIndices[0]].ClaimNum);
			long claimClinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				claimClinicNum=claim0.ClinicNum;//All claims for the queueItems have same clinic, due to validation above.
			}
			Clearinghouse clearinghouseHq=ClearinghouseL.GetClearinghouseHq(queueItems[0].ClearinghouseNum);
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,claimClinicNum);
			EnumClaimMedType medType=claim0.MedType;
			//Already validated that all claims are for the same clearinghouse, clinic, and medType.
			//Validated that medtype matches clearinghouse e-format
			Cursor=Cursors.WaitCursor;
			Eclaims.Eclaims.SendBatch(clearinghouseClin,queueItems,medType);
			Cursor=Cursors.Default;
			//Loop through _listQueueAll and remove all items that were sent.
			List<ClaimSendQueueItem> listTempQueueItem=new List<ClaimSendQueueItem>(_arrayQueueAll);
			for(int i=0;i<queueItems.Count;i++) {
				if(queueItems[i].ClaimStatus=="S") {
					//Find the claim in the unfiltered list that was just sent and remove it.
					//(Find the index of listTempQueueItem c where c.ClaimNum is the same as the ClaimNum of the item just sent.)
					listTempQueueItem.RemoveAt(listTempQueueItem.FindIndex(c => c.ClaimNum==queueItems[i].ClaimNum));
					//one securitylog entry for each sent claim
					SecurityLogs.MakeLogEntry(Permissions.ClaimSend,queueItems[i].PatNum,Lan.g(this,"Claim sent from Claims Send Window."));
				}
			}
			_arrayQueueAll=listTempQueueItem.ToArray();
			//statuses changed to S in SendBatches
			FillGrid();
			FillHistory();
			//Now, the cool part.  Highlight all the claims that were just sent in the history grid
			for(int i=0;i<queueItems.Count;i++){
				for(int j=0;j<tableHistory.Rows.Count;j++){
					long claimNum=PIn.Long(tableHistory.Rows[j]["ClaimNum"].ToString());
					if(claimNum==queueItems[i].ClaimNum){
						gridHistory.SetSelected(j,true);
						break;
					}
				}
			}
		}

		///<summary>Validates all non-validated e-claims passed in.  Directly manipulates the corresponding ClaimSendQueueItem in _arrayQueueAll
		///If any information has changed, the grid will be refreshed and the selected items will remain selected.</summary>
		private void ValidateClaims(List<ClaimSendQueueItem> listClaimSendQueueItems) {
			//Only get a list of non-validated e-claims from the list passed in.
			List<ClaimSendQueueItem> listClaimsToValidate=listClaimSendQueueItems.FindAll(x => !x.IsValid && !x.NoSendElect);
			if(listClaimsToValidate.Count==0) {
				return;
			}
			Cursor.Current=Cursors.WaitCursor;
			//Loop through and validate all claims.
			Clearinghouse clearinghouseHq=ClearinghouseL.GetClearinghouseHq(listClaimsToValidate[0].ClearinghouseNum);
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
			for(int i=0;i<listClaimsToValidate.Count;i++) {
				Eclaims.Eclaims.GetMissingData(clearinghouseClin,listClaimsToValidate[i]);
				if(listClaimsToValidate[i].MissingData=="") {
					listClaimsToValidate[i].IsValid=true;
				}
			}
			//Push any changes made to the ClaimSendQueueItems passed in to _arrayQueueAll 
			for(int i=0;i<_arrayQueueAll.Length;i++) {
				ClaimSendQueueItem validatedClaimSendQueueItem=listClaimsToValidate.Find(x => x.ClaimNum==_arrayQueueAll[i].ClaimNum);
				if(validatedClaimSendQueueItem!=null) {
					_arrayQueueAll[i]=validatedClaimSendQueueItem.Copy();
				}
			}
			FillGrid(true);//Used here to display changes immediately
			Cursor.Current=Cursors.Default;
		}

		private void toolBarButReports_Click() {
			FormClaimReports FormC=new FormClaimReports();
			FormC.ShowDialog();
			FillHistory();//To show 277s after imported.
		}

		private void toolBarButOutstanding_Click() {
			FormCanadaOutstandingTransactions fcot=new FormCanadaOutstandingTransactions();
			fcot.ShowDialog();
		}

		private void toolBarButPayRec_Click() {
			FormCanadaPaymentReconciliation fcpr=new FormCanadaPaymentReconciliation();
			fcpr.ShowDialog();
		}

		private void toolBarButSummaryRec_Click() {
			FormCanadaSummaryReconciliation fcsr=new FormCanadaSummaryReconciliation();
			fcsr.ShowDialog();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butNextUnsent_Click(object sender,EventArgs e) {
			int clinicSelectedAdjust=0;
			if(!Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndex!=0) {
				clinicSelectedAdjust=1;
			}
			int newClinicSelected=-1;
			for(int i=0;i<_listNumberOfClaims.Count;i++) {
				//Ignore currently selected clinic
				if(i==comboClinic.SelectedIndex-clinicSelectedAdjust) {
					continue;
				}
				if(i>comboClinic.SelectedIndex-clinicSelectedAdjust && _listNumberOfClaims[i]>0) {
					comboClinic.SelectedIndex=i+clinicSelectedAdjust;
					FillGrid();
					return;
				}
				if(_listNumberOfClaims[i]>0 && newClinicSelected==-1) {
					newClinicSelected=i+clinicSelectedAdjust;
				}
			}
			if(newClinicSelected>=0) {
				comboClinic.SelectedIndex=newClinicSelected;
				FillGrid();
				return;
			}
		}

		private void comboCustomTracking_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillHistory(){
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				) {
				return;
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo;
			if(textDateTo.Text=="") {
				dateTo=DateTime.MaxValue;
			}
			else {
				dateTo=PIn.Date(textDateTo.Text);
			}
			List<EtransType> listSelectedEtransTypes=new List<EtransType>();
			for(int i=0;i<comboHistoryType.SelectedIndices.Count;i++) {//Some selected, add only those selected
				int selectedIdx=(int)comboHistoryType.SelectedIndices[i];
				listSelectedEtransTypes.Add(_listCurEtransTypes[selectedIdx]);
			}
			if(comboHistoryType.SelectedIndices.Count==0) {//None selected.  The user can unselect each option manually.
				listSelectedEtransTypes.AddRange(_listCurEtransTypes);
			}
			tableHistory=Etranss.RefreshHistory(dateFrom,dateTo,listSelectedEtransTypes);
			//listQueue=Claims.GetQueueList();
			gridHistory.BeginUpdate();
			gridHistory.Columns.Clear();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				ODGridColumn col;
				col=new ODGridColumn(Lan.g("TableClaimHistory","Patient Name"),130);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Carrier Name"),170);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Clearinghouse"),90);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Date"),70,HorizontalAlignment.Center);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Type"),100);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","AckCode"),100,HorizontalAlignment.Center);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Note"),100);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Office#"),100);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","CarrierCount"),0);
				gridHistory.Columns.Add(col);
				gridHistory.Rows.Clear();
				ODGridRow row;
				for(int i=0;i<tableHistory.Rows.Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(tableHistory.Rows[i]["patName"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["CarrierName"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["Clearinghouse"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["dateTimeTrans"].ToString());
					//((DateTime)tableHistory.Rows[i]["DateTimeTrans"]).ToShortDateString());
					//still need to trim the _CA
					row.Cells.Add(tableHistory.Rows[i]["etype"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["ack"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["Note"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["OfficeSequenceNumber"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["CarrierTransCounter"].ToString());
					gridHistory.Rows.Add(row);
				}
			}
			else {
				ODGridColumn col;
				col=new ODGridColumn(Lan.g("TableClaimHistory","Patient Name"),130);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Carrier Name"),170);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Clearinghouse"),90);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Date"),70,HorizontalAlignment.Center);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Type"),100);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","AckCode"),100,HorizontalAlignment.Center);
				gridHistory.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimHistory","Note"),0);
				gridHistory.Columns.Add(col);
				gridHistory.Rows.Clear();
				ODGridRow row;
				for(int i=0;i<tableHistory.Rows.Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(tableHistory.Rows[i]["patName"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["CarrierName"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["Clearinghouse"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["dateTimeTrans"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["etype"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["ack"].ToString());
					row.Cells.Add(tableHistory.Rows[i]["Note"].ToString());
					gridHistory.Rows.Add(row);
				}
			}
			gridHistory.EndUpdate();
			gridHistory.ScrollToEnd();
		}

		private void panelSplitter_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=true;
			SplitterOriginalY=panelSplitter.Top;
			OriginalMouseY=panelSplitter.Top+e.Y;
		}

		private void panelSplitter_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnSplitter)
				return;
			int splitterNewY=SplitterOriginalY+(panelSplitter.Top+e.Y)-OriginalMouseY;
			if(splitterNewY<130)//keeps it from going too high
				splitterNewY=130;
			if(splitterNewY>Height-130)//keeps it from going off the bottom edge
				splitterNewY=Height-130;
			panelSplitter.Top=splitterNewY;
			AdjustPanelSplit();
		}

		private void AdjustPanelSplit(){
			gridMain.Height=panelSplitter.Top-gridMain.Top;
			panelHistory.Top=panelSplitter.Bottom;
			panelHistory.Height=this.ClientSize.Height-panelHistory.Top-1;
			gridHistory.Height=panelHistory.Height-(ToolBarHistory.Location.Y+ToolBarHistory.Height+panelSplitter.Height);//Needs to be done because anchors were removed
			gridHistory.Location=new Point(gridHistory.Location.X,ToolBarHistory.Location.Y+ToolBarHistory.Height+5);
		}

		private void panelSplitter_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=false;
		}

		private void butDropFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void ToggleCalendars() {
			if(calendarFrom.Visible) {
				//hide the calendars
				calendarFrom.Visible=false;
				calendarTo.Visible=false;
			}
			else {
				//set the date on the calendars to match what's showing in the boxes
				if(textDateFrom.errorProvider1.GetError(textDateFrom)==""
					&& textDateTo.errorProvider1.GetError(textDateTo)=="") {//if no date errors
					if(textDateFrom.Text=="") {
						calendarFrom.SetDate(DateTime.Today);
					}
					else {
						calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
					}
					if(textDateTo.Text=="") {
						calendarTo.SetDate(DateTime.Today);
					}
					else {
						calendarTo.SetDate(PIn.Date(textDateTo.Text));
					}
				}
				//show the calendars
				calendarFrom.Visible=true;
				calendarTo.Visible=true;
			}
		}

		private void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			textDateFrom.Text=calendarFrom.SelectionStart.ToShortDateString();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			textDateTo.Text=calendarTo.SelectionStart.ToShortDateString();
		}

		private void ToolBarHistory_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()){
				case "Refresh":
					RefreshHistory_Click();
					break;
				case "Undo":
					Undo_Click();
					break;
				case "PrintList":
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus
					//when it comes from a toolbar click.
					ToolBarClick toolClick=PrintHistory_Click;
					this.BeginInvoke(toolClick);
					break;
				case "PrintItem":
					PrintItem_Click();
					break;
			}
		}

		private void RefreshHistory_Click() {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			calendarFrom.Visible=false;
			calendarTo.Visible=false;
			FillHistory();
		}

		private void Undo_Click(){
			if(gridHistory.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select at least one item first.");
				return;
			}
			if(gridHistory.SelectedIndices.Length>1){//if there are multiple items selected.
				//then they must all be Claim_Ren, ClaimSent, or ClaimPrinted
				EtransType etype;
				for(int i=0;i<gridHistory.SelectedIndices.Length;i++) {
					etype=(EtransType)PIn.Long(tableHistory.Rows[gridHistory.SelectedIndices[i]]["Etype"].ToString());
					if(etype!=EtransType.Claim_Ren && etype!=EtransType.ClaimSent && etype!=EtransType.ClaimPrinted){
						MsgBox.Show(this,"That type of transaction cannot be undone as a group.  Please undo one at a time.");
						return;
					}
				}
			}
			//loop through each selected item, and see if they are allowed to be "undone".
			//at this point, 
			for(int i=0;i<gridHistory.SelectedIndices.Length;i++) {
				if((EtransType)PIn.Long(tableHistory.Rows[gridHistory.SelectedIndices[i]]["Etype"].ToString())==EtransType.Claim_CA){
					//if a 
				}
				//else if(){
				
				//}
				
			}
			if(!MsgBox.Show(this,true,"Remove the selected claims from the history list, and change the claim status from 'Sent' back to 'Waiting to Send'?")){
				return;
			}
			for(int i=0;i<gridHistory.SelectedIndices.Length;i++){
				Etranss.Undo(PIn.Long(tableHistory.Rows[gridHistory.SelectedIndices[i]]["EtransNum"].ToString()));
			}
			FillGrid();
			FillHistory();
		}

		private void PrintHistory_Click() {
			pagesPrinted=0;
			//headingPrinted=false;
#if DEBUG
			PrintReport(true);
#else
			PrintReport(false);	
#endif
		}

		private void gridHistory_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Etrans et=Etranss.GetEtrans(PIn.Long(tableHistory.Rows[e.Row]["EtransNum"].ToString()));
			if(et.Etype==EtransType.StatusNotify_277) {
				FormEtrans277Edit Form277=new FormEtrans277Edit();
				Form277.EtransCur=et;
				Form277.ShowDialog();
				return;//No refresh needed because 277s are not editable, they are read only.
			}
			if(et.Etype==EtransType.ERA_835) {
				FormEtrans835Edit.ShowEra(et);
			}
			else {
				FormEtransEdit FormE=new FormEtransEdit();
				FormE.EtransCur=et;
				FormE.ShowDialog();
				if(FormE.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			int scroll=gridHistory.ScrollValue;
			FillHistory();
			for(int i=0;i<tableHistory.Rows.Count;i++){
				if(tableHistory.Rows[i]["EtransNum"].ToString()==et.EtransNum.ToString()){
					gridHistory.SetSelected(i,true);
				}
			}
			gridHistory.ScrollValue=scroll;
		}

		private void ShowRawMessage_Clicked(object sender,System.EventArgs e) {
			//accessed by right clicking on history
			
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintReport(bool justPreview) {
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.OriginAtMargins=true;
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			//isPrinting=true;
			//FillGrid();
			try {
				if(justPreview) {
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();
				}
				else {
					if(PrinterL.SetPrinter(pd2,PrintSituation.Default,0,"Claim history list printed")) {
						pd2.Print();
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
			//isPrinting=false;
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Claim History");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridHistory.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void PrintItem_Click(){
			//not currently accessible
			if(gridHistory.Rows.Count==0){
				MsgBox.Show(this,"There are no items to print.");
				return;
			}
			if(gridHistory.SelectedIndices.Length==0){
				#if DEBUG
				gridHistory.SetSelected(0,true);//saves you a click when testing
				#else
				MsgBox.Show(this,"Please select at least one item first.");
				return;
				#endif
			}
			//does not yet handle multiple selections
			Etrans etrans=Etranss.GetEtrans(PIn.Long(tableHistory.Rows[gridHistory.SelectedIndices[0]]["EtransNum"].ToString()));
			new FormCCDPrint(etrans,EtransMessageTexts.GetMessageText(etrans.EtransMessageTextNum),false);//Show the form and allow the user to print manually if desired.
			//MessageBox.Show(etrans.MessageText);
		}

		private void butWeekPrevious_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateFrom!=DateTime.MinValue) {
				dateTo=dateFrom.AddDays(-1);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else if(dateTo!=DateTime.MinValue) {//Invalid dateFrom but valid dateTo
				dateTo=dateTo.AddDays(-8);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.AddDays(-7).ToShortDateString();
				textDateTo.Text=DateTime.Today.ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}

		private void butWeekNext_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateTo!=DateTime.MinValue) {
				dateFrom=dateTo.AddDays(1);
				textDateFrom.Text=dateFrom.ToShortDateString();
				textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else if(dateFrom!=DateTime.MinValue) {//Invalid dateTo but valid dateFrom
				 dateFrom=dateFrom.AddDays(8);
				 textDateFrom.Text=dateFrom.ToShortDateString();
				 textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.ToShortDateString();
				textDateTo.Text=DateTime.Today.AddDays(7).ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}

		
		

	
		

		

		

		

		

					
				

	}
}







