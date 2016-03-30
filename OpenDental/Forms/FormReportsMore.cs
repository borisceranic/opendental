using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.WebBridges;
using OpenDental.Bridges;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormReportsMore:ODForm {
		private OpenDental.UI.Button butClose;
		private Label labelPublicHealth;
		private Label labelLists;
		private Label labelMonthly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ListBoxClickable listLists;
		private OpenDental.UI.ListBoxClickable listPublicHealth;
		private OpenDental.UI.Button butUserQuery;
		private OpenDental.UI.Button butPW;
		private OpenDental.UI.ListBoxClickable listProdInc;
		private Label labelProdInc;
		private OpenDental.UI.ListBoxClickable listDaily;
		private Label labelDaily;
		private Label label6;
		private OpenDental.UI.Button butLaserLabels;
		private OpenDental.UI.ListBoxClickable listArizonaPrimaryCare;
		private Label labelArizonaPrimaryCare;
		private OpenDental.UI.ListBoxClickable listMonthly;
		private MenuStrip menuMain;
		private UI.Button butUDS;
		private UI.Button butPatList;
		private UI.Button butPatExport;
		private GroupBox groupPatientReviews;
		private ToolStripMenuItem setupToolStripMenuItem;
		private UI.ODPictureBox picturePodium;
		private UI.ODPictureBox pictureDentalIntel;
		private GroupBox groupBusiness;
		///<summary>After this form closes, this value is checked to see if any non-modal dialog boxes are needed.</summary>
		public ReportModalSelection RpModalSelection;
		private List<DisplayReport> _listProdInc;
		private List<DisplayReport> _listMonthly;
		private List<DisplayReport> _listDaily;
		private List<DisplayReport> _listList;
		private List<DisplayReport> _listPublicHealth;
		private List<DisplayReport> _listArizonaPrimary;

		///<summary></summary>
		public FormReportsMore() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportsMore));
			this.groupBusiness = new System.Windows.Forms.GroupBox();
			this.groupPatientReviews = new System.Windows.Forms.GroupBox();
			this.labelArizonaPrimaryCare = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.labelDaily = new System.Windows.Forms.Label();
			this.labelProdInc = new System.Windows.Forms.Label();
			this.labelMonthly = new System.Windows.Forms.Label();
			this.labelLists = new System.Windows.Forms.Label();
			this.labelPublicHealth = new System.Windows.Forms.Label();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureDentalIntel = new OpenDental.UI.ODPictureBox();
			this.picturePodium = new OpenDental.UI.ODPictureBox();
			this.butPatExport = new OpenDental.UI.Button();
			this.butPatList = new OpenDental.UI.Button();
			this.listArizonaPrimaryCare = new OpenDental.UI.ListBoxClickable();
			this.butUDS = new OpenDental.UI.Button();
			this.butLaserLabels = new OpenDental.UI.Button();
			this.listDaily = new OpenDental.UI.ListBoxClickable();
			this.listProdInc = new OpenDental.UI.ListBoxClickable();
			this.butPW = new OpenDental.UI.Button();
			this.butUserQuery = new OpenDental.UI.Button();
			this.listPublicHealth = new OpenDental.UI.ListBoxClickable();
			this.listLists = new OpenDental.UI.ListBoxClickable();
			this.listMonthly = new OpenDental.UI.ListBoxClickable();
			this.butClose = new OpenDental.UI.Button();
			this.groupBusiness.SuspendLayout();
			this.groupPatientReviews.SuspendLayout();
			this.menuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBusiness
			// 
			this.groupBusiness.Controls.Add(this.pictureDentalIntel);
			this.groupBusiness.Location = new System.Drawing.Point(532, 87);
			this.groupBusiness.Name = "groupBusiness";
			this.groupBusiness.Size = new System.Drawing.Size(113, 53);
			this.groupBusiness.TabIndex = 28;
			this.groupBusiness.TabStop = false;
			this.groupBusiness.Text = "Business Analytics";
			// 
			// groupPatientReviews
			// 
			this.groupPatientReviews.Controls.Add(this.picturePodium);
			this.groupPatientReviews.Location = new System.Drawing.Point(532, 146);
			this.groupPatientReviews.Name = "groupPatientReviews";
			this.groupPatientReviews.Size = new System.Drawing.Size(113, 54);
			this.groupPatientReviews.TabIndex = 26;
			this.groupPatientReviews.TabStop = false;
			this.groupPatientReviews.Text = "Patient Reviews";
			// 
			// labelArizonaPrimaryCare
			// 
			this.labelArizonaPrimaryCare.AutoSize = true;
			this.labelArizonaPrimaryCare.Location = new System.Drawing.Point(291, 364);
			this.labelArizonaPrimaryCare.Name = "labelArizonaPrimaryCare";
			this.labelArizonaPrimaryCare.Size = new System.Drawing.Size(104, 13);
			this.labelArizonaPrimaryCare.TabIndex = 20;
			this.labelArizonaPrimaryCare.Text = "Arizona Primary Care";
			this.labelArizonaPrimaryCare.Visible = false;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(9, 506);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(479, 100);
			this.label6.TabIndex = 17;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// labelDaily
			// 
			this.labelDaily.Location = new System.Drawing.Point(9, 182);
			this.labelDaily.Name = "labelDaily";
			this.labelDaily.Size = new System.Drawing.Size(118, 18);
			this.labelDaily.TabIndex = 15;
			this.labelDaily.Text = "Daily";
			this.labelDaily.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelProdInc
			// 
			this.labelProdInc.Location = new System.Drawing.Point(9, 66);
			this.labelProdInc.Name = "labelProdInc";
			this.labelProdInc.Size = new System.Drawing.Size(207, 18);
			this.labelProdInc.TabIndex = 13;
			this.labelProdInc.Text = "Production and Income";
			this.labelProdInc.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelMonthly
			// 
			this.labelMonthly.Location = new System.Drawing.Point(9, 299);
			this.labelMonthly.Name = "labelMonthly";
			this.labelMonthly.Size = new System.Drawing.Size(118, 18);
			this.labelMonthly.TabIndex = 6;
			this.labelMonthly.Text = "Monthly";
			this.labelMonthly.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelLists
			// 
			this.labelLists.Location = new System.Drawing.Point(291, 66);
			this.labelLists.Name = "labelLists";
			this.labelLists.Size = new System.Drawing.Size(118, 18);
			this.labelLists.TabIndex = 4;
			this.labelLists.Text = "Lists";
			this.labelLists.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPublicHealth
			// 
			this.labelPublicHealth.Location = new System.Drawing.Point(291, 304);
			this.labelPublicHealth.Name = "labelPublicHealth";
			this.labelPublicHealth.Size = new System.Drawing.Size(118, 18);
			this.labelPublicHealth.TabIndex = 2;
			this.labelPublicHealth.Text = "Public Health";
			this.labelPublicHealth.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(680, 24);
			this.menuMain.TabIndex = 22;
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.setupToolStripMenuItem.Text = "Setup";
			this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
			// 
			// pictureDentalIntel
			// 
			this.pictureDentalIntel.HasBorder = false;
			this.pictureDentalIntel.Image = global::OpenDental.Properties.Resources.DI_Button_100x24;
			this.pictureDentalIntel.Location = new System.Drawing.Point(8, 19);
			this.pictureDentalIntel.Name = "pictureDentalIntel";
			this.pictureDentalIntel.Size = new System.Drawing.Size(95, 24);
			this.pictureDentalIntel.TabIndex = 0;
			this.pictureDentalIntel.TextNullImage = null;
			this.pictureDentalIntel.Click += new System.EventHandler(this.pictureDentalIntel_Click);
			// 
			// picturePodium
			// 
			this.picturePodium.HasBorder = false;
			this.picturePodium.Image = global::OpenDental.Properties.Resources.Podium_Button_100x24;
			this.picturePodium.Location = new System.Drawing.Point(8, 19);
			this.picturePodium.Name = "picturePodium";
			this.picturePodium.Size = new System.Drawing.Size(95, 24);
			this.picturePodium.TabIndex = 28;
			this.picturePodium.TextNullImage = null;
			this.picturePodium.Click += new System.EventHandler(this.picturePodium_Click);
			// 
			// butPatExport
			// 
			this.butPatExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatExport.Autosize = true;
			this.butPatExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatExport.CornerRadius = 4F;
			this.butPatExport.Location = new System.Drawing.Point(538, 277);
			this.butPatExport.Name = "butPatExport";
			this.butPatExport.Size = new System.Drawing.Size(101, 24);
			this.butPatExport.TabIndex = 24;
			this.butPatExport.Text = "EHR Pat Export";
			this.butPatExport.Visible = false;
			this.butPatExport.Click += new System.EventHandler(this.butPatExport_Click);
			// 
			// butPatList
			// 
			this.butPatList.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatList.Autosize = true;
			this.butPatList.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatList.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatList.CornerRadius = 4F;
			this.butPatList.Location = new System.Drawing.Point(538, 247);
			this.butPatList.Name = "butPatList";
			this.butPatList.Size = new System.Drawing.Size(101, 24);
			this.butPatList.TabIndex = 23;
			this.butPatList.Text = "EHR Patient List";
			this.butPatList.Click += new System.EventHandler(this.butPatList_Click);
			// 
			// listArizonaPrimaryCare
			// 
			this.listArizonaPrimaryCare.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listArizonaPrimaryCare.FormattingEnabled = true;
			this.listArizonaPrimaryCare.ItemHeight = 15;
			this.listArizonaPrimaryCare.Location = new System.Drawing.Point(294, 382);
			this.listArizonaPrimaryCare.Name = "listArizonaPrimaryCare";
			this.listArizonaPrimaryCare.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listArizonaPrimaryCare.Size = new System.Drawing.Size(204, 34);
			this.listArizonaPrimaryCare.TabIndex = 19;
			this.listArizonaPrimaryCare.Visible = false;
			this.listArizonaPrimaryCare.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listArizonaPrimaryCare_MouseDown);
			// 
			// butUDS
			// 
			this.butUDS.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUDS.Autosize = true;
			this.butUDS.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUDS.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUDS.CornerRadius = 4F;
			this.butUDS.Location = new System.Drawing.Point(406, 35);
			this.butUDS.Name = "butUDS";
			this.butUDS.Size = new System.Drawing.Size(92, 24);
			this.butUDS.TabIndex = 18;
			this.butUDS.Text = "UDS Reporting";
			this.butUDS.UseVisualStyleBackColor = true;
			this.butUDS.Visible = false;
			this.butUDS.Click += new System.EventHandler(this.butUDS_Click);
			// 
			// butLaserLabels
			// 
			this.butLaserLabels.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLaserLabels.Autosize = true;
			this.butLaserLabels.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLaserLabels.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLaserLabels.CornerRadius = 4F;
			this.butLaserLabels.Location = new System.Drawing.Point(294, 35);
			this.butLaserLabels.Name = "butLaserLabels";
			this.butLaserLabels.Size = new System.Drawing.Size(75, 24);
			this.butLaserLabels.TabIndex = 18;
			this.butLaserLabels.Text = "Laser Labels";
			this.butLaserLabels.UseVisualStyleBackColor = true;
			this.butLaserLabels.Click += new System.EventHandler(this.butLaserLabels_Click);
			// 
			// listDaily
			// 
			this.listDaily.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listDaily.FormattingEnabled = true;
			this.listDaily.ItemHeight = 15;
			this.listDaily.Location = new System.Drawing.Point(12, 203);
			this.listDaily.Name = "listDaily";
			this.listDaily.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listDaily.Size = new System.Drawing.Size(204, 94);
			this.listDaily.TabIndex = 16;
			this.listDaily.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDaily_MouseDown);
			// 
			// listProdInc
			// 
			this.listProdInc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listProdInc.FormattingEnabled = true;
			this.listProdInc.ItemHeight = 15;
			this.listProdInc.Location = new System.Drawing.Point(12, 87);
			this.listProdInc.Name = "listProdInc";
			this.listProdInc.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listProdInc.Size = new System.Drawing.Size(204, 94);
			this.listProdInc.TabIndex = 14;
			this.listProdInc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listProdInc_MouseDown);
			// 
			// butPW
			// 
			this.butPW.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPW.Autosize = true;
			this.butPW.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPW.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPW.CornerRadius = 4F;
			this.butPW.Location = new System.Drawing.Point(135, 35);
			this.butPW.Name = "butPW";
			this.butPW.Size = new System.Drawing.Size(84, 24);
			this.butPW.TabIndex = 12;
			this.butPW.Text = "PW Reports";
			this.butPW.Click += new System.EventHandler(this.butPW_Click);
			// 
			// butUserQuery
			// 
			this.butUserQuery.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUserQuery.Autosize = true;
			this.butUserQuery.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUserQuery.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUserQuery.CornerRadius = 4F;
			this.butUserQuery.Location = new System.Drawing.Point(12, 35);
			this.butUserQuery.Name = "butUserQuery";
			this.butUserQuery.Size = new System.Drawing.Size(84, 24);
			this.butUserQuery.TabIndex = 11;
			this.butUserQuery.Text = "User Query";
			this.butUserQuery.Click += new System.EventHandler(this.butUserQuery_Click);
			// 
			// listPublicHealth
			// 
			this.listPublicHealth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listPublicHealth.FormattingEnabled = true;
			this.listPublicHealth.ItemHeight = 15;
			this.listPublicHealth.Location = new System.Drawing.Point(294, 325);
			this.listPublicHealth.Name = "listPublicHealth";
			this.listPublicHealth.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listPublicHealth.Size = new System.Drawing.Size(204, 34);
			this.listPublicHealth.TabIndex = 10;
			this.listPublicHealth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPublicHealth_MouseDown);
			// 
			// listLists
			// 
			this.listLists.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listLists.FormattingEnabled = true;
			this.listLists.ItemHeight = 15;
			this.listLists.Location = new System.Drawing.Point(294, 87);
			this.listLists.Name = "listLists";
			this.listLists.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listLists.Size = new System.Drawing.Size(204, 214);
			this.listLists.TabIndex = 9;
			this.listLists.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listLists_MouseDown);
			// 
			// listMonthly
			// 
			this.listMonthly.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listMonthly.FormattingEnabled = true;
			this.listMonthly.ItemHeight = 15;
			this.listMonthly.Location = new System.Drawing.Point(12, 320);
			this.listMonthly.Name = "listMonthly";
			this.listMonthly.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listMonthly.Size = new System.Drawing.Size(204, 169);
			this.listMonthly.TabIndex = 8;
			this.listMonthly.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMonthly_MouseDown);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(583, 562);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormReportsMore
			// 
			this.ClientSize = new System.Drawing.Size(680, 612);
			this.Controls.Add(this.groupBusiness);
			this.Controls.Add(this.groupPatientReviews);
			this.Controls.Add(this.butPatExport);
			this.Controls.Add(this.butPatList);
			this.Controls.Add(this.labelArizonaPrimaryCare);
			this.Controls.Add(this.listArizonaPrimaryCare);
			this.Controls.Add(this.butUDS);
			this.Controls.Add(this.butLaserLabels);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.listDaily);
			this.Controls.Add(this.labelDaily);
			this.Controls.Add(this.listProdInc);
			this.Controls.Add(this.labelProdInc);
			this.Controls.Add(this.butPW);
			this.Controls.Add(this.butUserQuery);
			this.Controls.Add(this.listPublicHealth);
			this.Controls.Add(this.listLists);
			this.Controls.Add(this.listMonthly);
			this.Controls.Add(this.labelMonthly);
			this.Controls.Add(this.labelLists);
			this.Controls.Add(this.labelPublicHealth);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.menuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuMain;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormReportsMore";
			this.ShowInTaskbar = false;
			this.Text = "Reports";
			this.Load += new System.EventHandler(this.FormReportsMore_Load);
			this.groupBusiness.ResumeLayout(false);
			this.groupPatientReviews.ResumeLayout(false);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormReportsMore_Load(object sender,EventArgs e) {
			Plugins.HookAddCode(this,"FormReportsMore.FormReportsMore_Load_beginning");
			butPW.Visible=Programs.IsEnabled(ProgramName.PracticeWebReports);
			//hiding feature for 13.3
			//butPatList.Visible=PrefC.GetBool(PrefName.ShowFeatureEhr);
			butPatExport.Visible=PrefC.GetBool(PrefName.ShowFeatureEhr);
			FillLists();
			//Notify user if partial batch ins payments exist.
			if(ClaimPayments.HasPartialPayments()) {
				System.Windows.Forms.MessageBox.Show(Lan.g(this,"At least one insurance payment is not finalized")
					+".  "+Lan.g(this,"Reports will be inaccurate until all payments are finalized")
					+".\r\n"+Lan.g(this,"See query example in the online manual")+" #958, \"Claims with payments entered but no check, not finalized.\"");
			}
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.Podium)) {
				groupPatientReviews.Visible=false;
			}
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.DentalIntel)) {
				groupBusiness.Visible=false;
			}
		}

		///<summary>Takes all non-hidden display reports and displays them in their various listboxes.  
		///Hides listboxes that have no display reports.</summary>
		private void FillLists() {
			_listProdInc=DisplayReports.GetForCategory(DisplayReportCategory.ProdInc,false);
			_listMonthly=DisplayReports.GetForCategory(DisplayReportCategory.Monthly,false);
			_listDaily=DisplayReports.GetForCategory(DisplayReportCategory.Daily,false);
			_listList=DisplayReports.GetForCategory(DisplayReportCategory.Lists,false);
			_listPublicHealth=DisplayReports.GetForCategory(DisplayReportCategory.PublicHealth,false);
			_listArizonaPrimary=DisplayReports.GetForCategory(DisplayReportCategory.ArizonaPrimaryCare,false);
			//add the items to the list boxes and set the list box heights. (positions too?)
			listProdInc.Items.Clear();
			listDaily.Items.Clear();
			listMonthly.Items.Clear();
			listLists.Items.Clear();
			listPublicHealth.Items.Clear();
			listArizonaPrimaryCare.Items.Clear();
			listProdInc.Items.AddRange(_listProdInc.Select(x => x.Description).ToArray());
			if(_listProdInc.Count==0) {
				listProdInc.Visible=false;
				labelProdInc.Visible=false;
			}
			else {
				listProdInc.Visible=true;
				labelProdInc.Visible=true;
				listProdInc.Height=(_listProdInc.Count+1) * listProdInc.ItemHeight;
			}
			listDaily.Items.AddRange(_listDaily.Select(x => x.Description).ToArray());
			if(_listDaily.Count==0) {
				listDaily.Visible=false;
				labelDaily.Visible=false;
			}
			else {
				listDaily.Visible=true;
				labelDaily.Visible=true;
				listDaily.Height=(_listDaily.Count+1) * listDaily.ItemHeight;
			}
			listMonthly.Items.AddRange(_listMonthly.Select(x => x.Description).ToArray());
			if(_listMonthly.Count==0) {
				listMonthly.Visible=false;
				labelMonthly.Visible=false;
			}
			else {
				listMonthly.Visible=true;
				labelMonthly.Visible=true;
				listMonthly.Height=(_listMonthly.Count+1) * listMonthly.ItemHeight;
			}
			listLists.Items.AddRange(_listList.Select(x => x.Description).ToArray());
			if(_listList.Count==0) {
				listLists.Visible=false;
				labelLists.Visible=false;
			}
			else {
				listLists.Visible=true;
				labelLists.Visible=true;
				listLists.Height=(_listList.Count+1) * listLists.ItemHeight;
			}
			listPublicHealth.Items.AddRange(_listPublicHealth.Select(x => x.Description).ToArray());
			if(_listPublicHealth.Count==0) {
				listPublicHealth.Visible=false;
				labelPublicHealth.Visible=false;
			}
			else {
				listPublicHealth.Visible=true;
				labelPublicHealth.Visible=true;
				listPublicHealth.Height=(_listPublicHealth.Count+1) * listPublicHealth.ItemHeight;
			}
			//Arizona primary care list and label must only be visible when the Arizona primary
			//care option is checked in the miscellaneous options.
				listArizonaPrimaryCare.Items.AddRange(_listArizonaPrimary.Select(x => x.Description).ToArray());
				if(_listArizonaPrimary.Count==0 || !UsingArizonaPrimaryCare()) {
					listArizonaPrimaryCare.Visible=false;
					labelArizonaPrimaryCare.Visible=false;
				}
				else {
					listArizonaPrimaryCare.Visible=true;
					labelArizonaPrimaryCare.Visible=true;
					listArizonaPrimaryCare.Height=(_listArizonaPrimary.Count+1) * listArizonaPrimaryCare.ItemHeight;
				}
		}

		///<summary>Returns true if all of the required patient fields exist which are necessary to run the Arizona Primary Care reports.
		///Otherwise, false is returned.</summary>
		public static bool UsingArizonaPrimaryCare() {
			PatFieldDefs.RefreshCache();
			string[] patientFieldNames=new string[] {
				"SPID#",
				"Eligibility Status",
				"Household Gross Income",
				"Household % of Poverty",
			};
			int[] fieldCounts=new int[patientFieldNames.Length];
			foreach(PatFieldDef pfd in PatFieldDefs.ListShort) {
				for(int i=0;i<patientFieldNames.Length;i++) {
					if(pfd.FieldName.ToLower()==patientFieldNames[i].ToLower()) {
						fieldCounts[i]++;
						break;
					}
				}
			}
			for(int i=0;i<fieldCounts.Length;i++) {
				//Each field must be defined exactly once. This verifies that each requied field
				//both exists and is not ambiguous with another field of the same name.
				if(fieldCounts[i]!=1) {
					return false;
				}
			}
			return true;
		}

		private void butUserQuery_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Not allowed while using Oracle.");
				return;
			}
			FormQuery FormQuery2=new FormQuery(null);
			FormQuery2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,"");
		}

		private void butPW_Click(object sender,EventArgs e) {
			try {
				Process.Start("PWReports.exe");
			}
			catch {
				System.Windows.Forms.MessageBox.Show("PracticeWeb Reports module unavailable.");
			}
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Practice Web");
		}

		private void listProdInc_MouseDown(object sender,MouseEventArgs e) {
			int selected=listProdInc.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.ReportProdInc)) {
				return;
			}
			FormRpProdInc FormPI=new FormRpProdInc();
			string internalName=_listProdInc[selected].InternalName;
			switch(internalName) {
				case "ODToday"://Today
					FormPI.DailyMonthlyAnnual="Daily";
					FormPI.DateStart=DateTime.Today;
					FormPI.DateEnd=DateTime.Today;
					break;
				case "ODYesterday"://Yesterday
					FormPI.DailyMonthlyAnnual="Daily";
					if(DateTime.Today.DayOfWeek==DayOfWeek.Monday) {
						FormPI.DateStart=DateTime.Today.AddDays(-3);
						FormPI.DateEnd=DateTime.Today.AddDays(-3);
					}
					else {
						FormPI.DateStart=DateTime.Today.AddDays(-1);
						FormPI.DateEnd=DateTime.Today.AddDays(-1);
					}
					break;
				case "ODThisMonth"://This Month
					FormPI.DailyMonthlyAnnual="Monthly";
					FormPI.DateStart=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.AddMonths(1).Year,DateTime.Today.AddMonths(1).Month,1).AddDays(-1);
					break;
				case "ODLastMonth"://Last Month
					FormPI.DailyMonthlyAnnual="Monthly";
					FormPI.DateStart=new DateTime(DateTime.Today.AddMonths(-1).Year,DateTime.Today.AddMonths(-1).Month,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddDays(-1);
					break;
				case "ODThisYear"://This Year
					FormPI.DailyMonthlyAnnual="Annual";
					FormPI.DateStart=new DateTime(DateTime.Today.Year,1,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.Year,12,31);
					break;
				case "ODMoreOptions"://More Options
				default:
					//do nothing
					break;
			}
			FormPI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.ReportProdInc,0,"");
		}

		private void listDaily_MouseDown(object sender,MouseEventArgs e) {
			int selected=listDaily.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			string internalName=_listDaily[selected].InternalName;
			switch(internalName) {
				case "ODAdjustments"://Adjustments
					FormRpAdjSheet FormAdjSheet=new FormRpAdjSheet();
					FormAdjSheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Adjustments");
					break;
				case "ODPayments"://Payments
					FormRpPaySheet FormPaySheet=new FormRpPaySheet();
					FormPaySheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Payments");
					break;
				case "ODProcedures"://Procedures
					FormRpProcSheet FormProcSheet=new FormRpProcSheet();
					FormProcSheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedures");
					break;
				case "ODWriteoffs"://Writeoffs
					FormRpWriteoffSheet FormW=new FormRpWriteoffSheet();
					FormW.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Writeoffs");
					break;
				case "ODIncompleteProcNotes"://Incomplete Procedure Notes
					FormRpProcNote FormPN=new FormRpProcNote();
					FormPN.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedure Notes");
					break;
				case "ODRoutingSlips"://Routing Slips
					FormRpRouting FormR=new FormRpRouting();
					FormR.ShowDialog();
					break;
			}
		}

		private void listMonthly_MouseDown(object sender,MouseEventArgs e) {
			int selected=listMonthly.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			string internalName=_listMonthly[selected].InternalName;
			switch(internalName) {
				case "ODAgingAR"://Aging of Accounts Receivable Report
					if(!Security.IsAuthorized(Permissions.ReportProdInc)) {
						return;
					}
					FormRpAging FormA=new FormRpAging();
					FormA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Aging of A/R");
					break;
				case "ODClaimsNotSent"://Claims Not Sent
					FormRpClaimNotSent FormClaim=new FormRpClaimNotSent();
					FormClaim.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Claims Not Sent");
					break;
				case "ODCapitation"://Capitation Utilization
					FormRpCapitation FormC=new FormRpCapitation();
					FormC.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Capitation");
					break;
				case "ODFinanceCharge"://Finance Charge Report
					FormRpFinanceCharge FormRpFinance=new FormRpFinanceCharge();
					FormRpFinance.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Finance Charges");
					break;
				case "ODOutstandingInsClaims"://Outstanding Insurance Claims
					RpModalSelection=ReportModalSelection.OutstandingIns;
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Outstanding Insurance Claims");
					Close();
					break;
				case "ODProcsNotBilled"://Procedures Not Billed to Insurance
					FormRpProcNotBilledIns FormProc=new FormRpProcNotBilledIns();
					FormProc.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedures not billed to insurance.");
					break;
				case "ODPPOWriteoffs"://PPO Writeoffs
					FormRpPPOwriteoffs FormPPO=new FormRpPPOwriteoffs();
					FormPPO.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PPO Writeoffs.");
					break;
				case "ODPaymentPlans"://Payment Plans
					FormRpPayPlans FormPP=new FormRpPayPlans();
					FormPP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Payment Plans.");
					break;
				case "ODReceivablesBreakdown"://Receivable Breakdown
					if(!Security.IsAuthorized(Permissions.ReportProdInc)) {
						return;
					}
					FormRpReceivablesBreakdown FormRcv = new FormRpReceivablesBreakdown();
					FormRcv.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Receivable Breakdown.");
					break;
				case "ODUnearnedIncome"://Unearned Income
					FormRpUnearnedIncome FormU=new FormRpUnearnedIncome();
					FormU.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Unearned Income.");
					break;
				case "ODInsuranceOverpaid"://Insurance Overpaid
					FormRpInsOverpaid FormI=new FormRpInsOverpaid();
					FormI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Overpaid.");
					break;
			}
		}

		private void listLists_MouseDown(object sender,MouseEventArgs e) {
			int selected=listLists.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			string internalName=_listList[selected].InternalName;
			switch(internalName) {
				case "ODActivePatients"://Active Patients
					FormRpActivePatients FormAP=new FormRpActivePatients();
					FormAP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Active Patients");
					break;
				case "ODAppointments"://Appointments
					FormRpAppointments FormA=new FormRpAppointments();
					FormA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Appointments");
					break;
				case "ODBirthdays"://Birthdays
					FormRpBirthday FormB=new FormRpBirthday();
					FormB.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Birthdays");
					break;
				case "ODBrokenAppointments"://Broken Appointments
					FormRpBrokenAppointments FormBroken=new FormRpBrokenAppointments();
					FormBroken.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Broken Appointments");
					break;
				case "ODInsurancePlans"://Insurance Plans
					FormRpInsCo FormInsCo=new FormRpInsCo();
					FormInsCo.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Plans");
					break;
				case "ODNewPatients"://New Patients
					FormRpNewPatients FormNewPats=new FormRpNewPatients();
					FormNewPats.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"New Patients");
					break;
				case "ODPatientsRaw"://Patients - Raw
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						return;
					}
					FormRpPatients FormPatients=new FormRpPatients();
					FormPatients.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patients - Raw");
					break;
				case "ODPatientNotes"://Patient Notes
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						return;
					}
					FormSearchPatNotes FormPN=new FormSearchPatNotes();
					FormPN.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patient Notes");
					break;
				case "ODPrescriptions"://Prescriptions
					FormRpPrescriptions FormPrescript=new FormRpPrescriptions();
					FormPrescript.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Rx");
					break;
				case "ODProcedureCodes"://Procedure Codes
					FormRpProcCodes FormProcCodes=new FormRpProcCodes();
					FormProcCodes.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedure Codes");
					break;
				case "ODReferralsRaw"://Referrals - Raw
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						return;
					}
					FormRpReferrals FormReferral=new FormRpReferrals();
					FormReferral.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Referrals - Raw");
					break;
				case "ODReferralAnalysis"://Referral Analysis
					FormRpReferralAnalysis FormRA=new FormRpReferralAnalysis();
					FormRA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Referral Analysis");
					break;
				case "ODReferredProcTracking"://Referred Proc Tracking
					FormReferralProcTrack FormRP=new FormReferralProcTrack();
					FormRP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"ReferredProcTracking");
					Close();
					break;
				case "ODTreatmentFinder"://Treatment Finder
					RpModalSelection=ReportModalSelection.TreatmentFinder;
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Treatment Finder");
					Close();
					break;
				//case 12://Treatment Plan Manager
				//  FormTxPlanManager FormTM=new FormTxPlanManager();
				//  FormTM.ShowDialog();
				//  SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Treatment Plan Manager");
				//  break;
			}
		}

		private void listPublicHealth_MouseDown(object sender,MouseEventArgs e) {
			int selected=listPublicHealth.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			string internalName=_listPublicHealth[selected].InternalName;
			switch(internalName) {
				case "ODRawScreeningData"://Raw Screening Data
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						return;
					}
					FormRpPHRawScreen FormPH=new FormRpPHRawScreen();
					FormPH.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw Screening");
					break;
				case "ODRawPopulationData"://Raw Population Data
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						return;
					}
					FormRpPHRawPop FormPHR=new FormRpPHRawPop();
					FormPHR.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw population");
					break;

			}
		}

		private void listArizonaPrimaryCare_MouseDown(object sender,MouseEventArgs e) {
			int selected=this.listArizonaPrimaryCare.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			string internalName = _listArizonaPrimary[selected].InternalName;
			switch(internalName) {
				case "ODEligibilityFile"://Eligibility File
					FormRpArizonaPrimaryCareEligibility frapce=new FormRpArizonaPrimaryCareEligibility();
					frapce.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Arizona Primary Care Eligibility");
					break;
				case "ODEncounterFile"://Encounter File
					FormRpArizonaPrimaryCareEncounter frapcn=new FormRpArizonaPrimaryCareEncounter();
					frapcn.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Arizona Primary Care Encounter");
					break;
			}
		}

		private void butLaserLabels_Click(object sender,EventArgs e) {
			FormRpLaserLabels LaserLabels = new FormRpLaserLabels();
			LaserLabels.ShowDialog();
		}

		private void butUDS_Click(object sender,EventArgs e) {
			//Recommend checking for user query permission, unless bringing up the preview window first, then the query view button prevents user from accessing user queries.
			//if(!Security.IsAuthorized(Permissions.UserQuery)) {
			//  return;
			//}
			//not visible
			FormReportsUds FormRU=new FormReportsUds();
			FormRU.ShowDialog();
		}

		private void setupToolStripMenuItem_Click(object sender,EventArgs e) {
			FormReportSetup formRS = new FormReportSetup();
			if(formRS.ShowDialog()==DialogResult.OK) {
				FillLists();
			}
		}

		private void butPatList_Click(object sender,EventArgs e) {
			FormPatListEHR2014 FormPL=new FormPatListEHR2014();
			FormPL.ShowDialog();
		}

		private void butPatExport_Click(object sender,EventArgs e) {
			FormEhrPatientExport FormEhrPE=new FormEhrPatientExport();
			FormEhrPE.ShowDialog();
		}

		private void pictureDentalIntel_Click(object sender,EventArgs e) {
			DentalIntel.ShowPage();
		}

		private void picturePodium_Click(object sender,EventArgs e) {
			try {
				Podium.ShowPage();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		

		

		

	}

	///<summary>Used in FormReportsMore to indicate that a modal window should be shown.</summary>
	public enum ReportModalSelection {
		///<summary></summary>
		None,
		///<summary></summary>
		TreatmentFinder,
		///<summary></summary>
		OutstandingIns
	}
}