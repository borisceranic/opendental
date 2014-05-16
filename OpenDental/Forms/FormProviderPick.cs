using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary>Pick a provider from the list.</summary>
	public class FormProviderPick:System.Windows.Forms.Form {
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		//private bool changed;
		//private User user;
		//private DataTable table;
		private OpenDental.UI.Button butOK;
		private Label labelUniqueID;
		private TextBox textUniqueID;
		///<summary>This can be set ahead of time to preselect a provider.  After closing with OK, this will have the selected provider number.</summary>
		public long SelectedProvNum;
		public bool IsStudentPicker=false;
		
		///<summary></summary>
		public FormProviderPick() {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProviderPick));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.labelUniqueID = new System.Windows.Forms.Label();
			this.textUniqueID = new System.Windows.Forms.TextBox();
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
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(411, 628);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(82, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Cancel";
			this.butClose.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(16, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(362, 642);
			this.gridMain.TabIndex = 13;
			this.gridMain.Title = "Providers";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(411, 596);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(82, 24);
			this.butOK.TabIndex = 14;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelUniqueID
			// 
			this.labelUniqueID.Location = new System.Drawing.Point(384, 12);
			this.labelUniqueID.Name = "labelUniqueID";
			this.labelUniqueID.Size = new System.Drawing.Size(90, 18);
			this.labelUniqueID.TabIndex = 27;
			this.labelUniqueID.Text = "Unique ID";
			this.labelUniqueID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textUniqueID
			// 
			this.textUniqueID.Location = new System.Drawing.Point(384, 33);
			this.textUniqueID.MaxLength = 15;
			this.textUniqueID.Name = "textUniqueID";
			this.textUniqueID.Size = new System.Drawing.Size(118, 20);
			this.textUniqueID.TabIndex = 26;
			this.textUniqueID.TextChanged += new System.EventHandler(this.textUniqueID_TextChanged);
			// 
			// FormProviderPick
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(514, 670);
			this.Controls.Add(this.labelUniqueID);
			this.Controls.Add(this.textUniqueID);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProviderPick";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Providers";
			this.Load += new System.EventHandler(this.FormProviderSelect_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormProviderSelect_Load(object sender, System.EventArgs e) {
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				labelUniqueID.Visible=false;
				textUniqueID.Visible=false;
			}
			FillGrid();
			if(SelectedProvNum!=0) {
				gridMain.SetSelected(Providers.GetIndex(SelectedProvNum),true);
			}
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				col=new ODGridColumn(Lan.g("TableProviders","Unique ID"),60);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProviders","Abbrev"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviders","Last Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviders","First Name"),90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				if(IsStudentPicker && ProviderC.ListShort[i].SchoolClassNum==0) {
					continue;
				}
				row=new ODGridRow();
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(ProviderC.ListShort[i].ProvNum.ToString());
				}
				row.Cells.Add(ProviderC.ListShort[i].Abbr);
				row.Cells.Add(ProviderC.ListShort[i].LName);
				row.Cells.Add(ProviderC.ListShort[i].FName);
				//wanted to do a background color here, but grid couldn't handle it.
				if(IsStudentPicker) {
					row.Tag=ProviderC.ListShort[i].ProvNum;
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectedProvNum=ProviderC.ListShort[e.Row].ProvNum;
			if(IsStudentPicker) {
				SelectedProvNum=PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			}
			DialogResult=DialogResult.OK;
		}

		private void textUniqueID_TextChanged(object sender,EventArgs e) {
			long provNum=0;
			if(long.TryParse(textUniqueID.Text,out provNum)) {
				gridMain.SetSelected(Providers.GetIndex(provNum),true);
				gridMain.ScrollToIndexBottom(Providers.GetIndex(provNum));
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a provider first.");
				return;
			}
			SelectedProvNum=ProviderC.ListShort[gridMain.GetSelectedIndex()].ProvNum;
			if(IsStudentPicker) {
				SelectedProvNum=PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		



	

	}
}
