using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Net;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClearinghouses : System.Windows.Forms.Form{
		private System.Windows.Forms.TextBox textBox1;
		private OpenDental.TableClearinghouses gridMain;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butAdd;
		private GroupBox groupBox1;
		private UI.Button butDefaultMedical;
		private UI.Button butDefaultDental;
		private TextBox textReportCheckInterval;
		private TextBox textReportComputerName;
		private UI.Button butThisComputer;
		private Label labelReportheckUnits;
		private Label labelReportCheckInterval;
		private Label labelReportComputerName;
		private bool listHasChanged;
		private List<Clearinghouse> _listClearinghouses;

		///<summary></summary>
		public FormClearinghouses()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.C(this, new System.Windows.Forms.Control[]
			{
				textBox1
			});
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClearinghouses));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.TableClearinghouses();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butAdd = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butDefaultMedical = new OpenDental.UI.Button();
			this.butDefaultDental = new OpenDental.UI.Button();
			this.textReportCheckInterval = new System.Windows.Forms.TextBox();
			this.textReportComputerName = new System.Windows.Forms.TextBox();
			this.butThisComputer = new OpenDental.UI.Button();
			this.labelReportheckUnits = new System.Windows.Forms.Label();
			this.labelReportCheckInterval = new System.Windows.Forms.Label();
			this.labelReportComputerName = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(807, 465);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.BackColor = System.Drawing.SystemColors.Window;
			this.gridMain.Location = new System.Drawing.Point(6, 61);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 1;
			this.gridMain.SelectedIndices = new int[0];
			this.gridMain.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.gridMain.Size = new System.Drawing.Size(879, 318);
			this.gridMain.TabIndex = 2;
			this.gridMain.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.gridMain_CellDoubleClicked);
			// 
			// textBox1
			// 
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(10, 8);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(597, 50);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(805, 385);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 24);
			this.butAdd.TabIndex = 8;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butDefaultMedical);
			this.groupBox1.Controls.Add(this.butDefaultDental);
			this.groupBox1.Location = new System.Drawing.Point(6, 387);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(97, 86);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Set Default";
			// 
			// butDefaultMedical
			// 
			this.butDefaultMedical.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefaultMedical.Autosize = true;
			this.butDefaultMedical.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefaultMedical.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefaultMedical.CornerRadius = 4F;
			this.butDefaultMedical.Location = new System.Drawing.Point(15, 49);
			this.butDefaultMedical.Name = "butDefaultMedical";
			this.butDefaultMedical.Size = new System.Drawing.Size(75, 24);
			this.butDefaultMedical.TabIndex = 2;
			this.butDefaultMedical.Text = "Medical";
			this.butDefaultMedical.Click += new System.EventHandler(this.butDefaultMedical_Click);
			// 
			// butDefaultDental
			// 
			this.butDefaultDental.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefaultDental.Autosize = true;
			this.butDefaultDental.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefaultDental.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefaultDental.CornerRadius = 4F;
			this.butDefaultDental.Location = new System.Drawing.Point(15, 19);
			this.butDefaultDental.Name = "butDefaultDental";
			this.butDefaultDental.Size = new System.Drawing.Size(75, 24);
			this.butDefaultDental.TabIndex = 1;
			this.butDefaultDental.Text = "Dental";
			this.butDefaultDental.Click += new System.EventHandler(this.butDefaultDental_Click);
			// 
			// textReportCheckInterval
			// 
			this.textReportCheckInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textReportCheckInterval.Location = new System.Drawing.Point(404, 439);
			this.textReportCheckInterval.MaxLength = 2147483647;
			this.textReportCheckInterval.Multiline = true;
			this.textReportCheckInterval.Name = "textReportCheckInterval";
			this.textReportCheckInterval.Size = new System.Drawing.Size(30, 20);
			this.textReportCheckInterval.TabIndex = 14;
			// 
			// textReportComputerName
			// 
			this.textReportComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textReportComputerName.Location = new System.Drawing.Point(404, 413);
			this.textReportComputerName.MaxLength = 2147483647;
			this.textReportComputerName.Multiline = true;
			this.textReportComputerName.Name = "textReportComputerName";
			this.textReportComputerName.Size = new System.Drawing.Size(240, 20);
			this.textReportComputerName.TabIndex = 11;
			// 
			// butThisComputer
			// 
			this.butThisComputer.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butThisComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butThisComputer.Autosize = true;
			this.butThisComputer.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butThisComputer.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butThisComputer.CornerRadius = 4F;
			this.butThisComputer.Location = new System.Drawing.Point(646, 411);
			this.butThisComputer.Name = "butThisComputer";
			this.butThisComputer.Size = new System.Drawing.Size(87, 24);
			this.butThisComputer.TabIndex = 16;
			this.butThisComputer.Text = "This Computer";
			this.butThisComputer.Click += new System.EventHandler(this.butThisComputer_Click);
			// 
			// labelReportheckUnits
			// 
			this.labelReportheckUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelReportheckUnits.Location = new System.Drawing.Point(440, 439);
			this.labelReportheckUnits.Name = "labelReportheckUnits";
			this.labelReportheckUnits.Size = new System.Drawing.Size(198, 20);
			this.labelReportheckUnits.TabIndex = 15;
			this.labelReportheckUnits.Text = "minutes (1 to 60)";
			this.labelReportheckUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelReportCheckInterval
			// 
			this.labelReportCheckInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelReportCheckInterval.Location = new System.Drawing.Point(107, 439);
			this.labelReportCheckInterval.Name = "labelReportCheckInterval";
			this.labelReportCheckInterval.Size = new System.Drawing.Size(295, 20);
			this.labelReportCheckInterval.TabIndex = 13;
			this.labelReportCheckInterval.Text = "Report Receive Interval";
			this.labelReportCheckInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelReportComputerName
			// 
			this.labelReportComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelReportComputerName.Location = new System.Drawing.Point(107, 413);
			this.labelReportComputerName.Name = "labelReportComputerName";
			this.labelReportComputerName.Size = new System.Drawing.Size(295, 20);
			this.labelReportComputerName.TabIndex = 12;
			this.labelReportComputerName.Text = "Computer To Receive Reports Automatically";
			this.labelReportComputerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormClearinghouses
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(891, 503);
			this.Controls.Add(this.textReportCheckInterval);
			this.Controls.Add(this.textReportComputerName);
			this.Controls.Add(this.butThisComputer);
			this.Controls.Add(this.labelReportheckUnits);
			this.Controls.Add(this.labelReportCheckInterval);
			this.Controls.Add(this.labelReportComputerName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClearinghouses";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "E-Claims";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClearinghouses_Closing);
			this.Load += new System.EventHandler(this.FormClearinghouses_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClearinghouses_Load(object sender, System.EventArgs e) {
			textReportComputerName.Text=PrefC.GetString(PrefName.ClaimReportComputerName);
			textReportCheckInterval.Text=POut.Int(PrefC.GetInt(PrefName.ClaimReportReceiveInterval));
			_listClearinghouses=Clearinghouses.GetListShort();
			FillGrid();
		}

		private void FillGrid(){
			gridMain.ResetRows(_listClearinghouses.Count);
			gridMain.SetGridColor(Color.Gray);
			gridMain.SetBackGColor(Color.White);
			for(int i=0;i<_listClearinghouses.Count;i++) {
				gridMain.Cell[0,i]=_listClearinghouses[i].Description;
				gridMain.Cell[1,i]=_listClearinghouses[i].ExportPath;
				gridMain.Cell[2,i]=_listClearinghouses[i].Eformat.ToString();
				string s="";
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==_listClearinghouses[i].ClearinghouseNum) {
					s+="Dent";
				}
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultMed)==_listClearinghouses[i].ClearinghouseNum) {
					if(s!=""){
						s+=",";
					}
					s+="Med";
				}
				gridMain.Cell[3,i]=s;
				gridMain.Cell[4,i]=_listClearinghouses[i].Payors;
			}
			gridMain.LayoutTables();
		}

		private void gridMain_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			FormClearinghouseEdit FormCE=new FormClearinghouseEdit();
			FormCE.ClearinghouseCur=_listClearinghouses[e.Row];
			FormCE.ShowDialog();
			if(FormCE.DialogResult!=DialogResult.OK){
				return;
			}
			if(FormCE.ClearinghouseCur==null) {//Clearinghouse was deleted.
				_listClearinghouses.RemoveAt(e.Row);
			}
			else {
				_listClearinghouses[e.Row]=FormCE.ClearinghouseCur.Copy();
			}
			listHasChanged=true;
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormClearinghouseEdit FormCE=new FormClearinghouseEdit();
			FormCE.ClearinghouseCur=new Clearinghouse();
			FormCE.IsNew=true;
			FormCE.ShowDialog();
			if(FormCE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormCE.ClearinghouseCur!=null) {
				_listClearinghouses.Add(FormCE.ClearinghouseCur.Copy());
			}
			listHasChanged=true;
			FillGrid();
		}

		private void butDefaultDental_Click(object sender,EventArgs e) {
			if(gridMain.SelectedRow==-1){
				MsgBox.Show(this,"Please select a row first.");
				return;
			}
			Clearinghouse ch=_listClearinghouses[gridMain.SelectedRow];
			if(ch.Eformat==ElectronicClaimFormat.x837_5010_med_inst){//med/inst clearinghouse
				MsgBox.Show(this,"The selected clearinghouse must first be set to a dental e-claim format.");
				return;
			}
			if(Prefs.UpdateLong(PrefName.ClearinghouseDefaultDent,ch.ClearinghouseNum)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butDefaultMedical_Click(object sender,EventArgs e) {
			if(gridMain.SelectedRow==-1){
				MsgBox.Show(this,"Please select a row first.");
				return;
			}
			Clearinghouse clearhouse=_listClearinghouses[gridMain.SelectedRow];
			if(clearhouse.Eformat!=ElectronicClaimFormat.x837_5010_med_inst){//anything except the med/inst format
				MsgBox.Show(this,"The selected clearinghouse must first be set to the med/inst e-claim format.");
				return;
			}
			if(Prefs.UpdateLong(PrefName.ClearinghouseDefaultMed,clearhouse.ClearinghouseNum)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butThisComputer_Click(object sender,EventArgs e) {
			textReportComputerName.Text=Dns.GetHostName();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			if(textReportComputerName.Text.Trim().ToLower()=="localhost" || textReportComputerName.Text.Trim()=="127.0.0.1") {
				MsgBox.Show(this,"Computer name to fetch new reports from cannot be localhost or 127.0.0.1 or any other loopback address.");
				return;
			}
			int reportCheckIntervalMinuteCount=0;
			try {
				reportCheckIntervalMinuteCount=PIn.Int(textReportCheckInterval.Text);
				if(reportCheckIntervalMinuteCount<1 || reportCheckIntervalMinuteCount>60) {
					throw new ApplicationException("Invalid value.");//User never sees this message.
				}
			}
			catch {
				MsgBox.Show(this,"Report check interval must be between 1 and 60 inclusive.");
				return;
			}
			if(PIn.Int(textReportCheckInterval.Text)!=PrefC.GetInt(PrefName.ClaimReportReceiveInterval)
				|| textReportComputerName.Text!=PrefC.GetString(PrefName.ClaimReportComputerName))
			{
				Prefs.UpdateString(PrefName.ClaimReportComputerName,textReportComputerName.Text);
				Prefs.UpdateInt(PrefName.ClaimReportReceiveInterval,reportCheckIntervalMinuteCount);
				MsgBox.Show(this,"You will need to restart the program for changes to take effect.");
			}
			Close();
		}

		private void FormClearinghouses_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==0){
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"A default clearinghouse should be set. Continue anyway?")){
					e.Cancel=true;
					return;
				}
			}
			//validate that the default dental clearinghouse is not type mismatched.
			Clearinghouse chDent=Clearinghouses.GetClearinghouse(PrefC.GetLong(PrefName.ClearinghouseDefaultDent));
			if(chDent!=null) {
				if(chDent.Eformat==ElectronicClaimFormat.x837_5010_med_inst) {//mismatch
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The default dental clearinghouse should be set to a dental e-claim format.  Continue anyway?")) {
						e.Cancel=true;
						return;
					}
				}
			}
			//validate medical clearinghouse
			Clearinghouse chMed=Clearinghouses.GetClearinghouse(PrefC.GetLong(PrefName.ClearinghouseDefaultMed));
			if(chMed!=null) {
				if(chMed.Eformat!=ElectronicClaimFormat.x837_5010_med_inst) {//mismatch
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The default medical clearinghouse should be set to a med/inst e-claim format.  Continue anyway?")) {
						e.Cancel=true;
						return;
					}
				}
			}
			if(listHasChanged){
				//update all computers including this one:
				DataValid.SetInvalid(InvalidType.ClearHouses);
			}
		}

		

		

		

	}
}





















