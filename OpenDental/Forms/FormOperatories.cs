using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormOperatories : System.Windows.Forms.Form{
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private ComboBox comboClinic;
		private Label labelClinic;
		private List<Operatory> _listOps;
		private UI.Button butPickClinic;
		private Clinic[] _arrayClinics;

		///<summary></summary>
		public FormOperatories()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatories));
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butPickClinic = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(251, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "(Also, see the appointment views section)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(21, 31);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(679, 432);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Operatories";
			this.gridMain.TranslationName = "TableOperatories";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(450, 8);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(226, 21);
			this.comboClinic.TabIndex = 119;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClinic.Location = new System.Drawing.Point(380, 12);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(67, 16);
			this.labelClinic.TabIndex = 120;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickClinic
			// 
			this.butPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic.Autosize = false;
			this.butPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic.CornerRadius = 2F;
			this.butPickClinic.Location = new System.Drawing.Point(677, 8);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic.TabIndex = 121;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(721, 235);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 26);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(721, 203);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 26);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
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
			this.butAdd.Location = new System.Drawing.Point(716, 31);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 26);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(721, 437);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormOperatories
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(818, 486);
			this.Controls.Add(this.butPickClinic);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormOperatories";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Operatories";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormOperatories_Closing);
			this.Load += new System.EventHandler(this.FormOperatories_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormOperatories_Load(object sender, System.EventArgs e) {
			if(!PrefC.HasClinicsEnabled) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				butPickClinic.Visible=false;
			}
			else {
				comboClinic.Items.Add(Lan.g(this,"All"));
				comboClinic.SelectedIndex=0;
				_arrayClinics=Clinics.GetList();
				for(int i=0;i<_arrayClinics.Length;i++) {
					comboClinic.Items.Add(_arrayClinics[i].Description);
					if(_arrayClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex!=0) {
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
			Cache.Refresh(InvalidType.Operatories);
			_listOps=OperatoryC.GetListt();//Already ordered by ItemOrder
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableOperatories","Op Name"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Abbrev"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsHidden"),64,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Clinic"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Provider"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Hygienist"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsHygiene"),72,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsWebSched"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			UI.ODGridRow row;
			for(int i=0;i<_listOps.Count;i++){
				if(PrefC.HasClinicsEnabled 
					&& comboClinic.SelectedIndex!=0 
					&& _listOps[i].ClinicNum!=_arrayClinics[comboClinic.SelectedIndex-1].ClinicNum) 
				{
					continue;
				}
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(_listOps[i].OpName);
				row.Cells.Add(_listOps[i].Abbrev);
				if(_listOps[i].IsHidden){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(Clinics.GetDesc(_listOps[i].ClinicNum));
				row.Cells.Add(Providers.GetAbbr(_listOps[i].ProvDentist));
				row.Cells.Add(Providers.GetAbbr(_listOps[i].ProvHygienist));
				if(_listOps[i].IsHygiene){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(_listOps[i].IsWebSched?"X":"");
        row.Tag=_listOps[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FormOperatoryEdit FormOE=new FormOperatoryEdit((Operatory)gridMain.Rows[e.Row].Tag);
			FormOE.ListOps=_listOps;
			FormOE.ShowDialog();
			FillGrid();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
			if(comboClinic.SelectedIndex!=0) {
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
			else {
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
		}

		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			Clinic[] arrayClinics=Clinics.GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				if(arrayClinics[i].ClinicNum!=FormC.SelectedClinicNum) {
					continue;
				}
				comboClinic.SelectedIndex=i+1;
			}
			FillGrid();
			if(comboClinic.SelectedIndex!=0) {
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
			else {
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			Operatory opCur=new Operatory();
			if(gridMain.SelectedIndices.Length>0){//a row is selected
				opCur.ItemOrder=gridMain.SelectedIndices[0];
			}
			else{
				opCur.ItemOrder=_listOps.Count;//goes at end of list
			}
			FormOperatoryEdit FormE=new FormOperatoryEdit(opCur);
			FormE.ListOps=_listOps;
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}

		private void butUp_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"You must first select a row.");
				return;
			}
			int selected=gridMain.SelectedIndices[0];
			if(selected==0){
				return;//already at the top
			}
			//move selected item up
			_listOps[selected].ItemOrder--;
			//move the one above it down
			_listOps[selected-1].ItemOrder++;
			//Swap positions
			_listOps.Reverse(selected-1,2);
			FillGrid();
			gridMain.SetSelected(selected-1,true);
		}

		private void butDown_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"You must first select a row.");
				return;
			}
			int selected=gridMain.SelectedIndices[0];
			if(selected==_listOps.Count-1){
				return;//already at the bottom
			}
			//move selected item down
			_listOps[selected].ItemOrder++;
			//move the one below it up
			_listOps[selected+1].ItemOrder--;
			//Swap positions
			_listOps.Reverse(selected,2);
			FillGrid();
			gridMain.SetSelected(selected+1,true);
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormOperatories_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			//Renumber the itemorders to match the grid.  In most cases this will not do anything, but will fix any duplicate itemorders.
			for(int i=0;i<_listOps.Count;i++) {
				_listOps[i].ItemOrder=i;
			}
			Operatories.Sync(_listOps);
			DataValid.SetInvalid(InvalidType.Operatories);//With sync we don't know if anything changed.
		}
	}
}





















