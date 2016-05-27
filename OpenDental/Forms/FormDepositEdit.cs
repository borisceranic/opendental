using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System.Diagnostics;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormDepositEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ListBox listPayType;
		private System.Windows.Forms.Label label2;
		private Deposit DepositCur;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBankAccountInfo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textAmount;
		private System.Windows.Forms.GroupBox groupSelect;
		private OpenDental.UI.Button butPrint;
		private OpenDental.UI.ODGrid gridPat;
		private OpenDental.UI.ODGrid gridIns;
		///<summary></summary>
		public bool IsNew;
		private ClaimPayment[] ClaimPayList;
		private OpenDental.ValidDate textDateStart;
		private System.Windows.Forms.Label label5;
		private OpenDental.UI.Button butRefresh;
		private List<Payment> PatPayList;
		private ComboBox comboDepositAccount;
		private Label labelDepositAccount;
		private bool changed;
		private TextBox textDepositAccount;
		///<summary>Only used if linking to accounts</summary>
		private long[] DepositAccounts;
		private Label labelIncomeAccountQB;
		private ComboBox comboIncomeAccountQB;
		///<summary>Only used if linking to QB account.</summary>
		private List<string> DepositAccountsQB;
		private UI.Button butSendQB;
		private TextBox textMemo;
		private Label labelMemo;
		///<summary>Only used if linking to QB account.</summary>
		private List<string> IncomeAccountsQB;
		private ListBox listInsPayType;
		private Label label6;
		///<summary>True if the accounting software pref is set to QuickBooks.</summary>
		private bool IsQuickBooks;
		private TextBox textAmountSearch;
		private TextBox textCheckNumSearch;
		private Label label7;
		private Label label8;
		private TextBox textItemNum;
		private Label label9;
		private UI.Button butPDF;
		///<summary>Used to store DefNums in a 1:1 ratio for listInsPayType</summary>
		private List<long> _insPayDefNums;
		///<summary>Used to store DefNums in a 1:1 ratio for listPayType</summary>
		private List<long> _payTypeDefNums;
		///<summary>Keeps track of whether the payment has been saved to the database since the form was opened.</summary>
		private bool _hasBeenSavedToDB;

		///<summary></summary>
		public FormDepositEdit(Deposit depositCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			DepositCur=depositCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDepositEdit));
			this.groupSelect = new System.Windows.Forms.GroupBox();
			this.listInsPayType = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.listPayType = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBankAccountInfo = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textAmount = new System.Windows.Forms.TextBox();
			this.comboDepositAccount = new System.Windows.Forms.ComboBox();
			this.labelDepositAccount = new System.Windows.Forms.Label();
			this.textDepositAccount = new System.Windows.Forms.TextBox();
			this.labelIncomeAccountQB = new System.Windows.Forms.Label();
			this.comboIncomeAccountQB = new System.Windows.Forms.ComboBox();
			this.textMemo = new System.Windows.Forms.TextBox();
			this.labelMemo = new System.Windows.Forms.Label();
			this.textAmountSearch = new System.Windows.Forms.TextBox();
			this.textCheckNumSearch = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textItemNum = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butSendQB = new OpenDental.UI.Button();
			this.gridIns = new OpenDental.UI.ODGrid();
			this.butPrint = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butDelete = new OpenDental.UI.Button();
			this.gridPat = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			this.textDateStart = new OpenDental.ValidDate();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butPDF = new OpenDental.UI.Button();
			this.groupSelect.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupSelect
			// 
			this.groupSelect.Controls.Add(this.listInsPayType);
			this.groupSelect.Controls.Add(this.label6);
			this.groupSelect.Controls.Add(this.butRefresh);
			this.groupSelect.Controls.Add(this.textDateStart);
			this.groupSelect.Controls.Add(this.label5);
			this.groupSelect.Controls.Add(this.comboClinic);
			this.groupSelect.Controls.Add(this.labelClinic);
			this.groupSelect.Controls.Add(this.listPayType);
			this.groupSelect.Controls.Add(this.label2);
			this.groupSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSelect.Location = new System.Drawing.Point(602, 296);
			this.groupSelect.Name = "groupSelect";
			this.groupSelect.Size = new System.Drawing.Size(355, 324);
			this.groupSelect.TabIndex = 99;
			this.groupSelect.TabStop = false;
			this.groupSelect.Text = "Show";
			// 
			// listInsPayType
			// 
			this.listInsPayType.Location = new System.Drawing.Point(184, 111);
			this.listInsPayType.Name = "listInsPayType";
			this.listInsPayType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listInsPayType.Size = new System.Drawing.Size(165, 173);
			this.listInsPayType.TabIndex = 107;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(184, 94);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(165, 16);
			this.label6.TabIndex = 108;
			this.label6.Text = "Insurance Payment Types";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(14, 14);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(118, 16);
			this.label5.TabIndex = 104;
			this.label5.Text = "Start Date";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(14, 68);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(180, 21);
			this.comboClinic.TabIndex = 94;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(14, 51);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(102, 16);
			this.labelClinic.TabIndex = 93;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listPayType
			// 
			this.listPayType.Location = new System.Drawing.Point(14, 111);
			this.listPayType.Name = "listPayType";
			this.listPayType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listPayType.Size = new System.Drawing.Size(165, 173);
			this.listPayType.TabIndex = 96;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(165, 16);
			this.label2.TabIndex = 97;
			this.label2.Text = "Patient Payment Types";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(602, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 16);
			this.label1.TabIndex = 102;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(602, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(127, 16);
			this.label3.TabIndex = 104;
			this.label3.Text = "Bank Account Info";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBankAccountInfo
			// 
			this.textBankAccountInfo.Location = new System.Drawing.Point(602, 100);
			this.textBankAccountInfo.Multiline = true;
			this.textBankAccountInfo.Name = "textBankAccountInfo";
			this.textBankAccountInfo.Size = new System.Drawing.Size(289, 59);
			this.textBankAccountInfo.TabIndex = 105;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(602, 46);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(94, 16);
			this.label4.TabIndex = 106;
			this.label4.Text = "Amount";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(602, 63);
			this.textAmount.Name = "textAmount";
			this.textAmount.ReadOnly = true;
			this.textAmount.Size = new System.Drawing.Size(94, 20);
			this.textAmount.TabIndex = 107;
			// 
			// comboDepositAccount
			// 
			this.comboDepositAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDepositAccount.FormattingEnabled = true;
			this.comboDepositAccount.Location = new System.Drawing.Point(602, 230);
			this.comboDepositAccount.Name = "comboDepositAccount";
			this.comboDepositAccount.Size = new System.Drawing.Size(289, 21);
			this.comboDepositAccount.TabIndex = 110;
			// 
			// labelDepositAccount
			// 
			this.labelDepositAccount.Location = new System.Drawing.Point(602, 213);
			this.labelDepositAccount.Name = "labelDepositAccount";
			this.labelDepositAccount.Size = new System.Drawing.Size(289, 16);
			this.labelDepositAccount.TabIndex = 111;
			this.labelDepositAccount.Text = "Deposit into Account";
			this.labelDepositAccount.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDepositAccount
			// 
			this.textDepositAccount.Location = new System.Drawing.Point(602, 261);
			this.textDepositAccount.Name = "textDepositAccount";
			this.textDepositAccount.ReadOnly = true;
			this.textDepositAccount.Size = new System.Drawing.Size(289, 20);
			this.textDepositAccount.TabIndex = 112;
			// 
			// labelIncomeAccountQB
			// 
			this.labelIncomeAccountQB.Location = new System.Drawing.Point(602, 256);
			this.labelIncomeAccountQB.Name = "labelIncomeAccountQB";
			this.labelIncomeAccountQB.Size = new System.Drawing.Size(289, 11);
			this.labelIncomeAccountQB.TabIndex = 114;
			this.labelIncomeAccountQB.Text = "Income Account";
			this.labelIncomeAccountQB.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelIncomeAccountQB.Visible = false;
			// 
			// comboIncomeAccountQB
			// 
			this.comboIncomeAccountQB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboIncomeAccountQB.FormattingEnabled = true;
			this.comboIncomeAccountQB.Location = new System.Drawing.Point(602, 271);
			this.comboIncomeAccountQB.Name = "comboIncomeAccountQB";
			this.comboIncomeAccountQB.Size = new System.Drawing.Size(289, 21);
			this.comboIncomeAccountQB.TabIndex = 113;
			this.comboIncomeAccountQB.Visible = false;
			// 
			// textMemo
			// 
			this.textMemo.Location = new System.Drawing.Point(602, 177);
			this.textMemo.Multiline = true;
			this.textMemo.Name = "textMemo";
			this.textMemo.Size = new System.Drawing.Size(289, 35);
			this.textMemo.TabIndex = 117;
			// 
			// labelMemo
			// 
			this.labelMemo.Location = new System.Drawing.Point(602, 160);
			this.labelMemo.Name = "labelMemo";
			this.labelMemo.Size = new System.Drawing.Size(127, 16);
			this.labelMemo.TabIndex = 116;
			this.labelMemo.Text = "Memo";
			this.labelMemo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textAmountSearch
			// 
			this.textAmountSearch.Location = new System.Drawing.Point(473, 634);
			this.textAmountSearch.Name = "textAmountSearch";
			this.textAmountSearch.Size = new System.Drawing.Size(94, 20);
			this.textAmountSearch.TabIndex = 118;
			this.textAmountSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textAmountSearch_KeyUp);
			this.textAmountSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textAmountSearch_MouseUp);
			// 
			// textCheckNumSearch
			// 
			this.textCheckNumSearch.Location = new System.Drawing.Point(251, 634);
			this.textCheckNumSearch.Name = "textCheckNumSearch";
			this.textCheckNumSearch.Size = new System.Drawing.Size(94, 20);
			this.textCheckNumSearch.TabIndex = 119;
			this.textCheckNumSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCheckNumSearch_KeyUp);
			this.textCheckNumSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textCheckNumSearch_MouseUp);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(351, 634);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(121, 20);
			this.label7.TabIndex = 109;
			this.label7.Text = "Search Amount";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(102, 634);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(148, 20);
			this.label8.TabIndex = 120;
			this.label8.Text = "Search Check Number";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textItemNum
			// 
			this.textItemNum.Location = new System.Drawing.Point(710, 64);
			this.textItemNum.Name = "textItemNum";
			this.textItemNum.ReadOnly = true;
			this.textItemNum.Size = new System.Drawing.Size(54, 20);
			this.textItemNum.TabIndex = 121;
			this.textItemNum.Text = "0";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(710, 47);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 16);
			this.label9.TabIndex = 122;
			this.label9.Text = "Item Count";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSendQB
			// 
			this.butSendQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSendQB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSendQB.Autosize = true;
			this.butSendQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSendQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSendQB.CornerRadius = 4F;
			this.butSendQB.Location = new System.Drawing.Point(897, 268);
			this.butSendQB.Name = "butSendQB";
			this.butSendQB.Size = new System.Drawing.Size(75, 24);
			this.butSendQB.TabIndex = 115;
			this.butSendQB.Text = "&Send QB";
			this.butSendQB.Click += new System.EventHandler(this.butSendQB_Click);
			// 
			// gridIns
			// 
			this.gridIns.HScrollVisible = false;
			this.gridIns.Location = new System.Drawing.Point(8, 319);
			this.gridIns.Name = "gridIns";
			this.gridIns.ScrollValue = 0;
			this.gridIns.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridIns.Size = new System.Drawing.Size(584, 301);
			this.gridIns.TabIndex = 109;
			this.gridIns.Title = "Insurance Payments";
			this.gridIns.TranslationName = "TableDepositSlipIns";
			this.gridIns.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridIns_CellClick);
			this.gridIns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridIns_MouseUp);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(602, 632);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 24);
			this.butPrint.TabIndex = 108;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(602, 25);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(94, 20);
			this.textDate.TabIndex = 103;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(7, 631);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(85, 24);
			this.butDelete.TabIndex = 101;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// gridPat
			// 
			this.gridPat.HScrollVisible = false;
			this.gridPat.Location = new System.Drawing.Point(8, 12);
			this.gridPat.Name = "gridPat";
			this.gridPat.ScrollValue = 0;
			this.gridPat.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridPat.Size = new System.Drawing.Size(584, 299);
			this.gridPat.TabIndex = 100;
			this.gridPat.Title = "Patient Payments";
			this.gridPat.TranslationName = "TableDepositSlipPat";
			this.gridPat.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellClick);
			this.gridPat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridPat_MouseUp);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(144, 294);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 106;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(14, 31);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(94, 20);
			this.textDateStart.TabIndex = 105;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(800, 632);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(881, 632);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butPDF
			// 
			this.butPDF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPDF.Autosize = true;
			this.butPDF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPDF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPDF.CornerRadius = 4F;
			this.butPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPDF.Location = new System.Drawing.Point(683, 632);
			this.butPDF.Name = "butPDF";
			this.butPDF.Size = new System.Drawing.Size(75, 24);
			this.butPDF.TabIndex = 123;
			this.butPDF.TabStop = false;
			this.butPDF.Text = "Create PDF";
			this.butPDF.Click += new System.EventHandler(this.butPDF_Click);
			// 
			// FormDepositEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 667);
			this.Controls.Add(this.butPDF);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textItemNum);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textCheckNumSearch);
			this.Controls.Add(this.textAmountSearch);
			this.Controls.Add(this.textMemo);
			this.Controls.Add(this.labelMemo);
			this.Controls.Add(this.butSendQB);
			this.Controls.Add(this.textDepositAccount);
			this.Controls.Add(this.labelDepositAccount);
			this.Controls.Add(this.comboDepositAccount);
			this.Controls.Add(this.gridIns);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBankAccountInfo);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.gridPat);
			this.Controls.Add(this.groupSelect);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.labelIncomeAccountQB);
			this.Controls.Add(this.comboIncomeAccountQB);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDepositEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Deposit Slip";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormDepositEdit_Closing);
			this.Load += new System.EventHandler(this.FormDepositEdit_Load);
			this.groupSelect.ResumeLayout(false);
			this.groupSelect.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDepositEdit_Load(object sender,System.EventArgs e) {
			butSendQB.Visible=false;
			IsQuickBooks=PrefC.GetInt(PrefName.AccountingSoftware)==(int)AccountingSoftware.QuickBooks;
			if(IsNew) {
				if(!Security.IsAuthorized(Permissions.DepositSlips,DateTime.Today)) {
					//we will check the date again when saving
					DialogResult=DialogResult.Cancel;
					return;
				}
			}
			else {
				//We enforce security here based on date displayed, not date entered
				if(!Security.IsAuthorized(Permissions.DepositSlips,DepositCur.DateDeposit)) {
					butOK.Enabled=false;
					butDelete.Enabled=false;
				}
			}
			if(IsNew) {
				textDateStart.Text=PIn.Date(PrefC.GetString(PrefName.DateDepositsStarted)).ToShortDateString();
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					comboClinic.Visible=false;
					labelClinic.Visible=false;
				}
				comboClinic.Items.Clear();
				comboClinic.Items.Add(Lan.g(this,"all"));
				comboClinic.SelectedIndex=0;
				for(int i=0;i<Clinics.List.Length;i++) {
					comboClinic.Items.Add(Clinics.List[i].Description);
				}
				_payTypeDefNums=new List<long>();
				for(int i=0;i<DefC.Short[(int)DefCat.PaymentTypes].Length;i++) {
					if(DefC.Short[(int)DefCat.PaymentTypes][i].ItemValue!="") {
						continue;//skip defs not selected for deposit slip
					}
					listPayType.Items.Add(DefC.Short[(int)DefCat.PaymentTypes][i].ItemName);
					_payTypeDefNums.Add(DefC.Short[(int)DefCat.PaymentTypes][i].DefNum);
					listPayType.SetSelected(listPayType.Items.Count-1,true);
				}
				_insPayDefNums=new List<long>();
				for(int i=0;i<DefC.Short[(int)DefCat.InsurancePaymentType].Length;i++) {
					if(DefC.Short[(int)DefCat.InsurancePaymentType][i].ItemValue!="") {
						continue;//skip defs not selected for deposit slip
					}
					listInsPayType.Items.Add(DefC.Short[(int)DefCat.InsurancePaymentType][i].ItemName);
					_insPayDefNums.Add(DefC.Short[(int)DefCat.InsurancePaymentType][i].DefNum);
					listInsPayType.SetSelected(listInsPayType.Items.Count-1,true);
				}
				textDepositAccount.Visible=false;//this is never visible for new. It's a description if already attached.
				if(Accounts.DepositsLinked() && !IsQuickBooks) {
					DepositAccounts=Accounts.GetDepositAccounts();
					for(int i=0;i<DepositAccounts.Length;i++) {
						comboDepositAccount.Items.Add(Accounts.GetDescript(DepositAccounts[i]));
					}
					comboDepositAccount.SelectedIndex=0;
				}
				else {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
				}
			}
			else {//Not new.
				groupSelect.Visible=false;
				gridIns.SelectionMode=GridSelectionMode.None;
				gridPat.SelectionMode=GridSelectionMode.None;
				//we never again let user change the deposit linking again from here.
				//They need to detach it from within the transaction
				//Might be enhanced later to allow, but that's very complex.
				Transaction trans=Transactions.GetAttachedToDeposit(DepositCur.DepositNum);
				if(trans==null) {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
					textDepositAccount.Visible=false;
				}
				else {
					comboDepositAccount.Enabled=false;
					labelDepositAccount.Text=Lan.g(this,"Deposited into Account");
					List<JournalEntry> jeL=JournalEntries.GetForTrans(trans.TransactionNum);
					for(int i=0;i<jeL.Count;i++) {
						if(Accounts.GetAccount(jeL[i].AccountNum).AcctType==AccountType.Asset) {
							comboDepositAccount.Items.Add(Accounts.GetDescript(jeL[i].AccountNum));
							comboDepositAccount.SelectedIndex=0;
							textDepositAccount.Text=jeL[i].DateDisplayed.ToShortDateString()
								+" "+jeL[i].DebitAmt.ToString("c");
							break;
						}
					}
				}
			}
			if(IsQuickBooks) {//If in QuickBooks mode, always show deposit and income accounts so that users can send old deposits into QB.
				labelIncomeAccountQB.Visible=true;
				comboIncomeAccountQB.Visible=true;
				comboIncomeAccountQB.Items.Clear();
				textDepositAccount.Visible=false;
				labelDepositAccount.Visible=true;
				labelDepositAccount.Text=Lan.g(this,"Deposit into Account");
				comboDepositAccount.Visible=true;
				comboDepositAccount.Enabled=true;
				comboDepositAccount.Items.Clear();
				if(Accounts.DepositsLinked()) {
					if(!IsNew) {
						butSendQB.Visible=true;
					}
					DepositAccountsQB=Accounts.GetDepositAccountsQB();
					for(int i=0;i<DepositAccountsQB.Count;i++) {
						comboDepositAccount.Items.Add(DepositAccountsQB[i]);
					}
					comboDepositAccount.SelectedIndex=0;
					IncomeAccountsQB=Accounts.GetIncomeAccountsQB();
					for(int i=0;i<IncomeAccountsQB.Count;i++) {
						comboIncomeAccountQB.Items.Add(IncomeAccountsQB[i]);
					}
					comboIncomeAccountQB.SelectedIndex=0;
				}
			}
			textDate.Text=DepositCur.DateDeposit.ToShortDateString();
			textAmount.Text=DepositCur.Amount.ToString("F");
			textBankAccountInfo.Text=DepositCur.BankAccountInfo;
			textMemo.Text=DepositCur.Memo;
			FillGrids();
			if(IsNew) {
				gridPat.SetSelected(true);
				gridIns.SetSelected(true);
			}
			ComputeAmt();
		}

		///<summary></summary>
		private void FillGrids(){
			if(IsNew){
				DateTime dateStart=PIn.Date(textDateStart.Text);
				long clinicNum=0;
				if(comboClinic.SelectedIndex!=0){
					clinicNum=Clinics.List[comboClinic.SelectedIndex-1].ClinicNum;
				}
				List<long> payTypes=new List<long>();//[listPayType.SelectedIndices.Count];
				for(int i=0;i<listPayType.SelectedIndices.Count;i++) {
					payTypes.Add(_payTypeDefNums[listPayType.SelectedIndices[i]]);
				}
				List<long> insPayTypes=new List<long>();
				for(int i=0;i<listInsPayType.SelectedIndices.Count;i++) {
					insPayTypes.Add(_insPayDefNums[listInsPayType.SelectedIndices[i]]);
				}
				PatPayList=new List<Payment>();
				if(payTypes.Count!=0) {
					PatPayList=Payments.GetForDeposit(dateStart,clinicNum,payTypes);
				}
				ClaimPayList=new ClaimPayment[0];
				if(insPayTypes.Count!=0) {
					ClaimPayList=ClaimPayments.GetForDeposit(dateStart,clinicNum,insPayTypes);
				}
			}
			else{
				PatPayList=Payments.GetForDeposit(DepositCur.DepositNum);
				ClaimPayList=ClaimPayments.GetForDeposit(DepositCur.DepositNum);
			}
			//Fill Patient Payment Grid---------------------------------------
			List<long> patNums=new List<long>();
			for(int i=0;i<PatPayList.Count;i++){
				patNums.Add(PatPayList[i].PatNum);
			}
			Patient[] pats=Patients.GetMultPats(patNums);
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableDepositSlipPat","Date"),80);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Patient"),150);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Type"),70);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Check Number"),95);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Bank-Branch"),80);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Amount"),80);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			OpenDental.UI.ODGridRow row;
			for(int i=0;i<PatPayList.Count;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(PatPayList[i].PayDate.ToShortDateString());
				row.Cells.Add(Patients.GetOnePat(pats,PatPayList[i].PatNum).GetNameLF());
				row.Cells.Add(DefC.GetName(DefCat.PaymentTypes,PatPayList[i].PayType));
				row.Cells.Add(PatPayList[i].CheckNum);
				row.Cells.Add(PatPayList[i].BankBranch);
				row.Cells.Add(PatPayList[i].PayAmt.ToString("F"));
				gridPat.Rows.Add(row);
			}
			gridPat.EndUpdate();
			//Fill Insurance Payment Grid-------------------------------------
			gridIns.BeginUpdate();
			gridIns.Columns.Clear();
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Date"),80);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Carrier"),150);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Type"),70);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Check Number"),95);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Bank-Branch"),80);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Amount"),90);
			gridIns.Columns.Add(col);
			gridIns.Rows.Clear();
			for(int i=0;i<ClaimPayList.Length;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(ClaimPayList[i].CheckDate.ToShortDateString());
				row.Cells.Add(ClaimPayList[i].CarrierName);
				row.Cells.Add(DefC.GetName(DefCat.InsurancePaymentType,ClaimPayList[i].PayType));
				row.Cells.Add(ClaimPayList[i].CheckNum);
				row.Cells.Add(ClaimPayList[i].BankBranch);
				row.Cells.Add(ClaimPayList[i].CheckAmt.ToString("F"));
				gridIns.Rows.Add(row);
			}
			gridIns.EndUpdate();
		}

		///<summary>Usually run after any selected items changed. Recalculates amt based on selected items.  May get fired twice when click and mouse up, harmless.</summary>
		private void ComputeAmt(){
			textItemNum.Text=(gridIns.SelectedIndices.Length+gridPat.SelectedIndices.Length).ToString();
			if(!IsNew){
				return;
			}
			decimal amount=0;
			for(int i=0;i<gridPat.SelectedIndices.Length;i++){
				amount+=(decimal)PatPayList[gridPat.SelectedIndices[i]].PayAmt;
			}
			for(int i=0;i<gridIns.SelectedIndices.Length;i++){
				amount+=(decimal)ClaimPayList[gridIns.SelectedIndices[i]].CheckAmt;
			}
			textAmount.Text=amount.ToString("F");
			DepositCur.Amount=(double)amount;
		}

		private void gridPat_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			ComputeAmt();
		}

		private void gridPat_MouseUp(object sender,MouseEventArgs e) {
			ComputeAmt();
		}

		private void gridIns_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			ComputeAmt();
		}

		private void gridIns_MouseUp(object sender,MouseEventArgs e) {
			ComputeAmt();
		}

		///<summary>Remember that this can only happen if IsNew</summary>
		private void butRefresh_Click(object sender, System.EventArgs e) {
			if(textDateStart.errorProvider1.GetError(textDate)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(listInsPayType.SelectedIndices.Count==0 && listPayType.SelectedIndices.Count==0) {
				for(int i=0;i<listInsPayType.Items.Count;i++) {
					listInsPayType.SetSelected(i,true);
				}
				for(int j=0;j<listPayType.Items.Count;j++) {
					listPayType.SetSelected(j,true);
				}
			}
			FillGrids();
			gridPat.SetSelected(true);
			gridIns.SetSelected(true);
			ComputeAmt();
			if(comboClinic.SelectedIndex==0){
				textBankAccountInfo.Text=PrefC.GetString(PrefName.PracticeBankNumber);
			}
			else{
				textBankAccountInfo.Text=Clinics.List[comboClinic.SelectedIndex-1].BankNumber;
			}
			if(Prefs.UpdateString(PrefName.DateDepositsStarted,POut.Date(PIn.Date(textDateStart.Text),false))){
				changed=true;
			}
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			//If deposit is attached to a transaction which is more than 48 hours old, then not allowed to delete.
			//This is hard coded.  User would have to delete or detach from within transaction rather than here.
			Transaction trans=Transactions.GetAttachedToDeposit(DepositCur.DepositNum);
			if(trans != null){
				if(trans.DateTimeEntry < MiscData.GetNowDateTime().AddDays(-2) ){
					MsgBox.Show(this,"Not allowed to delete.  This deposit is already attached to an accounting transaction.  You will need to detach it from within the accounting section of the program.");
					return;
				}
				if(Transactions.IsReconciled(trans)) {
					MsgBox.Show(this,"Not allowed to delete.  This deposit is attached to an accounting transaction that has been reconciled.  You will need to detach it from within the accounting section of the program.");
					return;
				}
				try{
					Transactions.Delete(trans);
				}
				catch(ApplicationException ex){
					MessageBox.Show(ex.Message);
					return;
				}
			}
			if(!MsgBox.Show(this,true,"Delete?")){
				return;
			}
			Deposits.Delete(DepositCur);
			DialogResult=DialogResult.OK;
		}

		private void butSendQB_Click(object sender,EventArgs e) {
			DateTime date=PIn.Date(textDate.Text);//We use security on the date showing.
			if(!Security.IsAuthorized(Permissions.DepositSlips,date)) {
				return;
			}
			DepositCur.DateDeposit=date;
			CreateDepositQB(false);
		}

		///<summary>Returns true if a deposit was created OR if the user clicked continue anyway on pop up.</summary>
		private bool CreateDepositQB(bool allowContinue) {
			try {
				Cursor.Current=Cursors.WaitCursor;
				QuickBooks.CreateDeposit(DepositCur.DateDeposit
					,DepositAccountsQB[comboDepositAccount.SelectedIndex]
					,IncomeAccountsQB[comboIncomeAccountQB.SelectedIndex]
					,DepositCur.Amount
					,textMemo.Text);
				SecurityLogs.MakeLogEntry(Permissions.DepositSlips,0,Lan.g(this,"Deposit slip sent to QuickBooks.")+"\r\n"
					+Lan.g(this,"Deposit date")+": "+DepositCur.DateDeposit.ToShortDateString()+" "+Lan.g(this,"for")+" "+DepositCur.Amount.ToString("c"));
				Cursor.Current=Cursors.Default;
				MsgBox.Show(this,"Deposit successfully sent to QuickBooks.");
				butSendQB.Enabled=false;//Don't let user send same deposit more than once.  
			}
			catch(Exception ex) {
				Cursor.Current=Cursors.Default;
				if(allowContinue) {
					if(MessageBox.Show(ex.Message+"\r\n\r\n"
						+Lan.g(this,"A deposit has not been created in QuickBooks, continue anyway?")
						,Lan.g(this,"QuickBooks Deposit Create Failed")
						,MessageBoxButtons.YesNo)!=DialogResult.Yes)
					{
						return false;
					}
				}
				else {
					MessageBox.Show(ex.Message,Lan.g(this,"QuickBooks Deposit Create Failed"));
					return false;
				}
			}
			return true;
		}

		private void textCheckNumSearch_KeyUp(object sender,KeyEventArgs e) {
			Search();
		}

		private void textCheckNumSearch_MouseUp(object sender,MouseEventArgs e) {
			Search();
		}

		private void textAmountSearch_KeyUp(object sender,KeyEventArgs e) {
			Search();
		}

		private void textAmountSearch_MouseUp(object sender,MouseEventArgs e) {
			Search();
		}

		private void Search() {
			bool isScrollSet=false;
			for(int i=0;i<gridIns.Rows.Count;i++) {
				bool isBold=false;
				if(textAmountSearch.Text!="" && gridIns.Rows[i].Cells[5].Text.ToUpper().Contains(textAmountSearch.Text.ToUpper())) {
					isBold=true;
				}
				if(textCheckNumSearch.Text!="" && gridIns.Rows[i].Cells[3].Text.ToUpper().Contains(textCheckNumSearch.Text.ToUpper())) {
					isBold=true;
				}
				gridIns.Rows[i].Bold=isBold;
				if(isBold) {
					gridIns.Rows[i].ColorText=Color.Red;					
					if(!isScrollSet) {//scroll to the first match in the list.
						gridIns.ScrollToIndex(i);
						isScrollSet=true;
					}
				}
				else {//Standard row.
					gridIns.Rows[i].ColorText=Color.Black;
				}
			}//end i
			gridIns.Invalidate();
			bool isScrollSetPat=false;
			for(int i=0;i<gridPat.Rows.Count;i++) {
				bool isBold=false;
				if(textAmountSearch.Text!="" && gridPat.Rows[i].Cells[5].Text.ToUpper().Contains(textAmountSearch.Text.ToUpper())) {
					isBold=true;
				}
				if(textCheckNumSearch.Text!="" && gridPat.Rows[i].Cells[3].Text.ToUpper().Contains(textCheckNumSearch.Text.ToUpper())) {
					isBold=true;
				}
				gridPat.Rows[i].Bold=isBold;
				if(isBold) {
					gridPat.Rows[i].ColorText=Color.Red;
					if(!isScrollSetPat) {//scroll to the first match in the list.
						gridPat.ScrollToIndex(i);
						isScrollSetPat=true;
					}
				}
				else {//Standard row.
					gridPat.Rows[i].ColorText=Color.Black;
				}
			}//end i
			gridPat.Invalidate();
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(IsNew){
				if(!SaveToDB()) {
					return;
				}
			}
			else{//not new
				//Only allowed to change date and bank account info, NOT attached checks.
				//We enforce security here based on date displayed, not date entered.
				//If user is trying to change date without permission:
				DateTime date=PIn.Date(textDate.Text);
				if(Security.IsAuthorized(Permissions.DepositSlips,date,true)){
					if(!SaveToDB()) {
						return;
					}
				}
				//if security.NotAuthorized, then it simply skips the save process before printing
			}
			SheetDef sheetDef=null;
			List <SheetDef> depositSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.DepositSlip);
			if(depositSheetDefs.Count>0){
				sheetDef=depositSheetDefs[0];
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			else{
				sheetDef=SheetsInternal.DepositSlip();
			}
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,0);
			SheetParameter.SetParameter(sheet,"DepositNum",DepositCur.DepositNum);
			SheetFiller.FillFields(sheet);
			Graphics g=this.CreateGraphics();
			SheetUtil.CalculateHeights(sheet,g);
			SheetPrinting.Print(sheet);
			g.Dispose();
		}
		
		private void butPDF_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(IsNew){
				if(!SaveToDB()) {
					return;
				}
			}
			else{//not new
				//Only allowed to change date and bank account info, NOT attached checks.
				//We enforce security here based on date displayed, not date entered.
				//If user is trying to change date without permission:
				DateTime date=PIn.Date(textDate.Text);
				if(Security.IsAuthorized(Permissions.DepositSlips,date,true)){
					if(!SaveToDB()) {
						return;
					}
				}
				//if security.NotAuthorized, then it simply skips the save process before printing
			}
			SheetDef sheetDef=null;
			List <SheetDef> depositSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.DepositSlip);
			if(depositSheetDefs.Count>0){
				sheetDef=depositSheetDefs[0];
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			else{
				sheetDef=SheetsInternal.DepositSlip();
			}
			//The below mimics FormSheetFillEdit.butPDF_Click() and the above butPrint_Click().
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,0);//Does not insert.
			SheetParameter.SetParameter(sheet,"DepositNum",DepositCur.DepositNum);
			SheetFiller.FillFields(sheet);
			Graphics g=this.CreateGraphics();
			SheetUtil.CalculateHeights(sheet,g);
			string filePathAndName=PrefL.GetRandomTempFile(".pdf");
			SheetPrinting.CreatePdf(sheet,filePathAndName,null);
			g.Dispose();
			Process.Start(filePathAndName);
		}

		///<summary>Saves the selected rows to database.</summary>
		private bool SaveToDB(){
			if(textDate.errorProvider1.GetError(textDate)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			//Prevent backdating----------------------------------------------------------------------------------------
			DateTime date=PIn.Date(textDate.Text);
			//We enforce security here based on date displayed, not date entered
			if(!Security.IsAuthorized(Permissions.DepositSlips,date)) {
				return false;
			}
			DepositCur.DateDeposit=PIn.Date(textDate.Text);
			//amount already handled.
			DepositCur.BankAccountInfo=PIn.String(textBankAccountInfo.Text);
			DepositCur.Memo=PIn.String(textMemo.Text);
			if(IsNew){
				if(gridPat.SelectedIndices.Length+gridIns.SelectedIndices.Length>18 && IsQuickBooks) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"No more than 18 items will fit on a QuickBooks deposit slip. Continue anyway?")) {
						return false;
					}
				}
				else if(gridPat.SelectedIndices.Length+gridIns.SelectedIndices.Length>32) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"No more than 32 items will fit on a deposit slip. Continue anyway?")) {
						return false;
					}
				}
			}
			if(IsNew && !_hasBeenSavedToDB){	
				if(Accounts.DepositsLinked() && DepositCur.Amount>0
					&& IsQuickBooks && !CreateDepositQB(true)) //Create a deposit in QuickBooks
				{
					return false;
				}
				Deposits.Insert(DepositCur);
			}
			else{
				Deposits.Update(DepositCur);
			}
			if(IsNew){//never allowed to change or attach more checks after initial creation of deposit slip
				for(int i=0;i<gridPat.SelectedIndices.Length;i++){
					PatPayList[gridPat.SelectedIndices[i]].DepositNum=DepositCur.DepositNum;
					Payments.Update(PatPayList[gridPat.SelectedIndices[i]],false);
				}
				for(int i=0;i<gridIns.SelectedIndices.Length;i++){
					ClaimPayList[gridIns.SelectedIndices[i]].DepositNum=DepositCur.DepositNum;
					ClaimPayments.Update(ClaimPayList[gridIns.SelectedIndices[i]]);
				}
			}
			_hasBeenSavedToDB=true;//So that we don't insert the deposit slip again when clicking Print or PDF or OK
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!SaveToDB()){
				return;
			}
			if(IsNew) {
				if(Accounts.DepositsLinked() && DepositCur.Amount>0 && !IsQuickBooks) {
					//create a transaction here
					Transaction trans=new Transaction();
					trans.DepositNum=DepositCur.DepositNum;
					trans.UserNum=Security.CurUser.UserNum;
					Transactions.Insert(trans);
					//first the deposit entry
					JournalEntry je=new JournalEntry();
					je.AccountNum=DepositAccounts[comboDepositAccount.SelectedIndex];
					je.CheckNumber=Lan.g(this,"DEP");
					je.DateDisplayed=DepositCur.DateDeposit;//it would be nice to add security here.
					je.DebitAmt=DepositCur.Amount;
					je.Memo=Lan.g(this,"Deposit");
					je.Splits=Accounts.GetDescript(PrefC.GetLong(PrefName.AccountingIncomeAccount));
					je.TransactionNum=trans.TransactionNum;
					JournalEntries.Insert(je);
					//then, the income entry
					je=new JournalEntry();
					je.AccountNum=PrefC.GetLong(PrefName.AccountingIncomeAccount);
					//je.CheckNumber=;
					je.DateDisplayed=DepositCur.DateDeposit;//it would be nice to add security here.
					je.CreditAmt=DepositCur.Amount;
					je.Memo=Lan.g(this,"Deposit");
					je.Splits=Accounts.GetDescript(DepositAccounts[comboDepositAccount.SelectedIndex]);
					je.TransactionNum=trans.TransactionNum;
					JournalEntries.Insert(je);
				}
				SecurityLogs.MakeLogEntry(Permissions.DepositSlips,0,DepositCur.DateDeposit.ToShortDateString()+" New "+DepositCur.Amount.ToString("c"));
			}
			else {//Not new
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,0,DepositCur.DateDeposit.ToShortDateString()+" "+DepositCur.Amount.ToString("c"));
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormDepositEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(IsNew && DialogResult==DialogResult.Cancel) {
				//User might have printed this, causing an insert into the DB.
				Deposits.Delete(DepositCur);//This will handle unattaching payments from this deposit. A Transaction should not have been made yet.
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

	

		

		

		

		

		

		

		


	}
}





















