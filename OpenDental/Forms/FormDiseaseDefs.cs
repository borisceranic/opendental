using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// </summary>
	public class FormDiseaseDefs:System.Windows.Forms.Form {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		private Label label1;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private System.Windows.Forms.ToolTip toolTip1;
		private OpenDental.UI.Button butOK;
		///<summary>Set to true when user is using this to select a disease def. Currently used when adding Alerts to Rx.</summary>
		public bool IsSelectionMode;
		///<summary>Set to true when user is using FormMedical to allow multiple problems to be selected at once.</summary>
		public bool IsMultiSelect;
		///<summary>If IsSelectionMode, then after closing with OK, this will contain number.</summary>
		public long SelectedDiseaseDefNum;
		///<summary>If IsMultiSelect, then this will contain a list of numbers when closing with OK.</summary>
		public List<long> SelectedDiseaseDefNums;
		private ODGrid gridMain;
		private bool IsChanged;
		private CheckBox checkAlpha;
		///<summary>A complete list of disease defs including hidden.  Only used when not in selection mode (item orders can change).  It's main purpose is to keep track of the item order for the life of the window so that we do not have to make unnecessary update calls to the database every time the up and down buttons are clicked.</summary>
		private List<DiseaseDef> _listDiseaseDefs;

		///<summary></summary>
		public FormDiseaseDefs()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiseaseDefs));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.checkAlpha = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(335, 20);
			this.label1.TabIndex = 8;
			this.label1.Text = "This is a list of medical problems that patients might have. ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(18, 35);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(548, 628);
			this.gridMain.TabIndex = 16;
			this.gridMain.Title = null;
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
			this.butOK.Location = new System.Drawing.Point(584, 605);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(79, 26);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(584, 464);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 26);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(584, 432);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 26);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(584, 637);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(584, 265);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// checkIns
			// 
			this.checkAlpha.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAlpha.Location = new System.Drawing.Point(441,11);
			this.checkAlpha.Name = "checkIns";
			this.checkAlpha.Size = new System.Drawing.Size(222,18);
			this.checkAlpha.TabIndex = 18;
			this.checkAlpha.Text = "Keep problem list alphabetized";
			this.checkAlpha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAlpha.CheckedChanged += new System.EventHandler(this.checkAlpha_CheckedChanged);
			// 
			// FormDiseaseDefs
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(682, 675);
			this.Controls.Add(this.checkAlpha);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDiseaseDefs";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Problems";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDiseaseDefs_FormClosing);
			this.Load += new System.EventHandler(this.FormDiseaseDefs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDiseaseDefs_Load(object sender, System.EventArgs e) {
			if(DiseaseDefs.FixItemOrders()) {
				DataValid.SetInvalid(InvalidType.Diseases);
			}
			//checkProcLockingIsAllowed.Checked=PrefC.GetBool(PrefName.ProcLockingIsAllowed);
			if(IsSelectionMode){
				butClose.Text=Lan.g(this,"Cancel");
				butDown.Visible=false;
				butUp.Visible=false;
				checkAlpha.Visible=false;
				if(IsMultiSelect) {
					gridMain.SelectionMode=GridSelectionMode.MultiExtended;
				}
			}
			else{
				butOK.Visible=false;
			}
			checkAlpha.Checked=PrefC.GetBool(PrefName.ProblemListIsAlpabetical);
			if(PrefC.GetBool(PrefName.ProblemListIsAlpabetical)) {
				butUp.Visible=false;
				butDown.Visible=false;
			}
			_listDiseaseDefs=new List<DiseaseDef>();
			_listDiseaseDefs.AddRange(DiseaseDefs.ListLong);
			//RefreshList();
			FillGrid();
		}

		/////<summary>Simply fills and orders the list of disease defs.  Can be called at any time.</summary>
		//private void RefreshListOld() {
		//	//We need to keep track of the current item orders.
		//	Dictionary<long,int> _dictItemOrder=new Dictionary<long,int>();
		//	if(_listDiseaseDefs!=null) {
		//		for(int i=0;i<_listDiseaseDefs.Count;i++) {
		//			_dictItemOrder.Add(_listDiseaseDefs[i].DiseaseDefNum,i);
		//		}
		//	}
		//	//When the cache is refreshed, the item orders and what items are present in the list could have changed.  E.g. deleted, added, or have old ordering.
		//	DiseaseDefs.RefreshCache();
		//	if(_listDiseaseDefs==null) {//The first time loading the window this list will be null so we simply need to fill it and trust that it is already in the correct order.
		//		_listDiseaseDefs=new List<DiseaseDef>(DiseaseDefs.ListLong);
		//		return;
		//	}
		//	//At this point we don't know if the user changed the order of the list and then deleted or added a disease def.  
		//	//Therefore we have to use our dictionary of the "old" item orders and reorder the list from the cache.
		//	_listDiseaseDefs=new List<DiseaseDef>();
		//	for(int j=0;j<_dictItemOrder.Count;j++) {
		//		//Find the matching disease def and force it into the order that it was last in.
		//		for(int k=0;k<DiseaseDefs.ListLong.Length;k++) {
		//			if(_dictItemOrder.ContainsKey(DiseaseDefs.ListLong[k].DiseaseDefNum)
		//				&& _dictItemOrder[DiseaseDefs.ListLong[k].DiseaseDefNum]==j) 
		//			{
		//				_listDiseaseDefs.Add(DiseaseDefs.ListLong[k]);
		//				break;
		//			}
		//			//If no disease def match is found, then that means the user has deleted the disease def and it will not get added to _listDiseaseDefs.
		//		}
		//	}
		//	//Now we need to add any new disease defs (only one can be added at a time but should work for multiple if it is enhanced in the future) to the end of _listDiseasesDefs.
		//	for(int i=0;i<DiseaseDefs.ListLong.Length;i++) {
		//		if(!_dictItemOrder.ContainsKey(DiseaseDefs.ListLong[i].DiseaseDefNum)) {
		//			_listDiseaseDefs.Add(DiseaseDefs.ListLong[i]);
		//		}
		//	}
		//}

		private void FillGrid() {
			//listMain.SelectionMode=SelectionMode.MultiExtended;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"ICD-9"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"ICD-10"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"SNOMED CT"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),250);
			gridMain.Columns.Add(col);
			if(!IsSelectionMode) {
				col=new ODGridColumn(Lan.g(this,"Hidden"),50,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			if(IsSelectionMode) {
				for(int i=0;i<DiseaseDefs.List.Length;i++) {
					row=new ODGridRow();
					row.Cells.Add(DiseaseDefs.List[i].ICD9Code);
					row.Cells.Add(DiseaseDefs.List[i].Icd10Code);
					row.Cells.Add(DiseaseDefs.List[i].SnomedCode);
					row.Cells.Add(DiseaseDefs.List[i].DiseaseName);
					gridMain.Rows.Add(row);
				}
			}
			else {//Not selection mode - show hidden
				for(int i=0;i<_listDiseaseDefs.Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(_listDiseaseDefs[i].ICD9Code);
					row.Cells.Add(_listDiseaseDefs[i].Icd10Code);
					row.Cells.Add(_listDiseaseDefs[i].SnomedCode);
					row.Cells.Add(_listDiseaseDefs[i].DiseaseName);
					row.Cells.Add(_listDiseaseDefs[i].IsHidden?"X":"");
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!IsSelectionMode && !Security.IsAuthorized(Permissions.ProblemEdit)) {//trying to double click to edit, but no permission.
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				return;
			}
			if(IsSelectionMode) {
				if(IsMultiSelect) {
					SelectedDiseaseDefNums=new List<long>();
					SelectedDiseaseDefNums.Add(DiseaseDefs.List[gridMain.GetSelectedIndex()].DiseaseDefNum);
				}
				else {
					if(Snomeds.GetByCode(DiseaseDefs.List[gridMain.GetSelectedIndex()].SnomedCode)!=null) {
						SelectedDiseaseDefNum=DiseaseDefs.List[gridMain.GetSelectedIndex()].DiseaseDefNum;
					}
					else {
						MsgBox.Show(this,"You have selected a problem with an unofficial SNOMED CT code.  Please correct the problem definition by going to Lists | Problems and choosing an official code from the SNOMED CT list.");
						return;
					}
				}
				DialogResult=DialogResult.OK;
				return;
			}
			//everything below this point is _not_ selection mode.  User guaranteed to have permission for ProblemEdit.
			FormDiseaseDefEdit FormD=new FormDiseaseDefEdit(_listDiseaseDefs[gridMain.GetSelectedIndex()]);
			FormD.ShowDialog();
			//Security log entry made inside that form.
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			//RefreshList();
			IsChanged=true;
			FillGrid();
		}

		private void butAdd_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProblemEdit)) {
				return;
			}
			DiseaseDef def=new DiseaseDef();
			def.ItemOrder=_listDiseaseDefs.Count;
			FormDiseaseDefEdit FormD=new FormDiseaseDefEdit(def);//also sets ItemOrder correctly if using alphabetical during the insert diseaseDef call.
			FormD.IsNew=true;
			FormD.ShowDialog();
			//Security log entry made inside that form.
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			//Items are already in the right order in the DB, re-order in memory list to match
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				if(_listDiseaseDefs[i].ItemOrder>=def.ItemOrder) {
					_listDiseaseDefs[i].ItemOrder++;
				}
			}
			_listDiseaseDefs.Add(def);
			_listDiseaseDefs.Sort(DiseaseDefs.SortItemOrder);
			//RefreshList();
			IsChanged=true;
			FillGrid();
		}

		private void checkAlpha_CheckedChanged(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.ProblemListIsAlpabetical)==checkAlpha.Checked) {
				return;//when loading form.
			}
			if(!checkAlpha.Checked) {
				butUp.Visible=true;
				butDown.Visible=true;
				Prefs.UpdateBool(PrefName.ProblemListIsAlpabetical,checkAlpha.Checked);
				return;//turned off alphabetizing
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Problems will be reordered, this cannot be undone.")) {
				checkAlpha.Checked=false;
				return;
			}
			Prefs.UpdateBool(PrefName.ProblemListIsAlpabetical,checkAlpha.Checked);
			butUp.Visible=false;
			butDown.Visible=false;
			//_listDiseaseDefs.Sort(DiseaseDefs.SortAlphabetically);//Sort in memory list
			Cursor=Cursors.WaitCursor;
			DiseaseDefs.AlphabetizeDB();//Sort DB list
			Cursor=Cursors.Default;
			_listDiseaseDefs.Clear();
			_listDiseaseDefs.AddRange(DiseaseDefs.ListLong);
			IsChanged=true;
			//RefreshList();
			FillGrid();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			Array.Copy(gridMain.SelectedIndices,selected,gridMain.SelectedIndices.Length);
			if(selected[0]==0) {
				return;
			}
			for(int i=0;i<selected.Length;i++) {
				_listDiseaseDefs.Reverse(selected[i]-1,2);
			}
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				_listDiseaseDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]-1,true);
			}
			IsChanged=true;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			Array.Copy(gridMain.SelectedIndices,selected,gridMain.SelectedIndices.Length);
			if(selected[selected.Length-1]==_listDiseaseDefs.Count-1) {
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				_listDiseaseDefs.Reverse(selected[i],2);
			}
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				_listDiseaseDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
			IsChanged=true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//not even visible unless IsSelectionMode
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(IsMultiSelect) {
				SelectedDiseaseDefNums=new List<long>();
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					SelectedDiseaseDefNums.Add(DiseaseDefs.List[gridMain.SelectedIndices[i]].DiseaseDefNum);
				}
			}
			else if(IsSelectionMode) {
				if(Snomeds.GetByCode(DiseaseDefs.List[gridMain.GetSelectedIndex()].SnomedCode)!=null) {
					SelectedDiseaseDefNum=DiseaseDefs.List[gridMain.GetSelectedIndex()].DiseaseDefNum;
				}
				else {
					MsgBox.Show(this,"You have selected a problem containing an invalid SNOMED CT.");
					return;
				}
			}
			else {
				SelectedDiseaseDefNum=_listDiseaseDefs[gridMain.GetSelectedIndex()].DiseaseDefNum;
			}
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormDiseaseDefs_FormClosing(object sender,FormClosingEventArgs e) {
			if(IsChanged) {
				DiseaseDefs.Sync(_listDiseaseDefs);//Update if anything has changed, even in selection mode.
				DataValid.SetInvalid(InvalidType.Diseases);
			}
		}

		

		

		

		

		

		

		

		


	}
}



























