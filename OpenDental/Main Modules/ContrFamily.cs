/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDental.Eclaims;

namespace OpenDental{

	///<summary></summary>
	public class ContrFamily : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ImageList imageListToolBar;
		private System.ComponentModel.IContainer components;
		private OpenDental.UI.ODToolBar ToolBarMain;
		///<summary>All recalls for this entire family.</summary>
		private List<Recall> RecallList;
		///<summary></summary>
		[Category("Data"),Description("Occurs when user changes current patient, usually by clicking on the Select Patient button.")]
		public event PatientSelectedEventHandler PatientSelected=null;
		private Patient PatCur;
		private Family FamCur;
		private OpenDental.UI.ODPictureBox picturePat;
		private List <InsPlan> PlanList;
		private OpenDental.UI.ODGrid gridIns;
		private List <PatPlan> PatPlanList;
		private ODGrid gridPat;
		private ContextMenu menuInsurance;
		private MenuItem menuPlansForFam;
		private List <Benefit> BenefitList;
		private ODGrid gridFamily;
		private ODGrid gridRecall;
		private PatField[] PatFieldList;
		private bool InitializedOnStartup;
		private ODGrid gridSuperFam;
		private List<InsSub> SubList;
		private List<Patient> SuperFamilyGuarantors;
		private List<Patient> SuperFamilyMembers;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private SortStrategy _superFamSortStrat;

		///<summary></summary>
		public ContrFamily(){
			Logger.openlog.Log("Initializing family module...",Logger.Severity.INFO);
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
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

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrFamily));
			this.imageListToolBar = new System.Windows.Forms.ImageList(this.components);
			this.menuInsurance = new System.Windows.Forms.ContextMenu();
			this.menuPlansForFam = new System.Windows.Forms.MenuItem();
			this.picturePat = new OpenDental.UI.ODPictureBox();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.gridSuperFam = new OpenDental.UI.ODGrid();
			this.gridRecall = new OpenDental.UI.ODGrid();
			this.gridFamily = new OpenDental.UI.ODGrid();
			this.gridPat = new OpenDental.UI.ODGrid();
			this.gridIns = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// imageListToolBar
			// 
			this.imageListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolBar.ImageStream")));
			this.imageListToolBar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListToolBar.Images.SetKeyName(0, "");
			this.imageListToolBar.Images.SetKeyName(1, "");
			this.imageListToolBar.Images.SetKeyName(2, "");
			this.imageListToolBar.Images.SetKeyName(3, "");
			this.imageListToolBar.Images.SetKeyName(4, "");
			this.imageListToolBar.Images.SetKeyName(5, "");
			this.imageListToolBar.Images.SetKeyName(6, "Umbrella.gif");
			// 
			// menuInsurance
			// 
			this.menuInsurance.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPlansForFam});
			// 
			// menuPlansForFam
			// 
			this.menuPlansForFam.Index = 0;
			this.menuPlansForFam.Text = "Plans for Family";
			this.menuPlansForFam.Click += new System.EventHandler(this.menuPlansForFam_Click);
			// 
			// picturePat
			// 
			this.picturePat.Location = new System.Drawing.Point(1, 27);
			this.picturePat.Name = "picturePat";
			this.picturePat.Size = new System.Drawing.Size(100, 100);
			this.picturePat.TabIndex = 28;
			this.picturePat.Text = "picturePat";
			this.picturePat.TextNullImage = "Patient Picture Unavailable";
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListToolBar;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(939, 25);
			this.ToolBarMain.TabIndex = 19;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// gridSuperFam
			// 
			this.gridSuperFam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridSuperFam.HasAddButton = false;
			this.gridSuperFam.HasMultilineHeaders = false;
			this.gridSuperFam.HScrollVisible = false;
			this.gridSuperFam.Location = new System.Drawing.Point(254, 129);
			this.gridSuperFam.Name = "gridSuperFam";
			this.gridSuperFam.ScrollValue = 0;
			this.gridSuperFam.Size = new System.Drawing.Size(329, 579);
			this.gridSuperFam.TabIndex = 33;
			this.gridSuperFam.Title = "Super Family";
			this.gridSuperFam.TranslationName = "TableSuper";
			this.gridSuperFam.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSuperFam_CellDoubleClick);
			this.gridSuperFam.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSuperFam_CellClick);
			// 
			// gridRecall
			// 
			this.gridRecall.HasAddButton = false;
			this.gridRecall.HasMultilineHeaders = false;
			this.gridRecall.HScrollVisible = true;
			this.gridRecall.Location = new System.Drawing.Point(585, 27);
			this.gridRecall.Name = "gridRecall";
			this.gridRecall.ScrollValue = 0;
			this.gridRecall.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridRecall.Size = new System.Drawing.Size(525, 100);
			this.gridRecall.TabIndex = 32;
			this.gridRecall.Title = "Recall";
			this.gridRecall.TranslationName = "TableRecall";
			this.gridRecall.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRecall_CellDoubleClick);
			this.gridRecall.DoubleClick += new System.EventHandler(this.gridRecall_DoubleClick);
			// 
			// gridFamily
			// 
			this.gridFamily.HasAddButton = false;
			this.gridFamily.HasMultilineHeaders = false;
			this.gridFamily.HScrollVisible = false;
			this.gridFamily.Location = new System.Drawing.Point(103, 27);
			this.gridFamily.Name = "gridFamily";
			this.gridFamily.ScrollValue = 0;
			this.gridFamily.SelectedRowColor = System.Drawing.Color.DarkSalmon;
			this.gridFamily.Size = new System.Drawing.Size(480, 100);
			this.gridFamily.TabIndex = 31;
			this.gridFamily.Title = "Family Members";
			this.gridFamily.TranslationName = "TableFamily";
			this.gridFamily.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamily_CellDoubleClick);
			this.gridFamily.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamily_CellClick);
			// 
			// gridPat
			// 
			this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridPat.HasAddButton = false;
			this.gridPat.HasMultilineHeaders = false;
			this.gridPat.HScrollVisible = false;
			this.gridPat.Location = new System.Drawing.Point(0, 129);
			this.gridPat.Name = "gridPat";
			this.gridPat.ScrollValue = 0;
			this.gridPat.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridPat.Size = new System.Drawing.Size(252, 579);
			this.gridPat.TabIndex = 30;
			this.gridPat.Title = "Patient Information";
			this.gridPat.TranslationName = "TablePatient";
			this.gridPat.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellDoubleClick);
			this.gridPat.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellClick);
			// 
			// gridIns
			// 
			this.gridIns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridIns.HasAddButton = false;
			this.gridIns.HasMultilineHeaders = false;
			this.gridIns.HScrollVisible = true;
			this.gridIns.Location = new System.Drawing.Point(254, 129);
			this.gridIns.Name = "gridIns";
			this.gridIns.ScrollValue = 0;
			this.gridIns.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridIns.Size = new System.Drawing.Size(685, 579);
			this.gridIns.TabIndex = 29;
			this.gridIns.Title = "Insurance Plans";
			this.gridIns.TranslationName = "TableCoverage";
			this.gridIns.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridIns_CellDoubleClick);
			// 
			// ContrFamily
			// 
			this.Controls.Add(this.gridSuperFam);
			this.Controls.Add(this.gridRecall);
			this.Controls.Add(this.gridFamily);
			this.Controls.Add(this.gridPat);
			this.Controls.Add(this.gridIns);
			this.Controls.Add(this.picturePat);
			this.Controls.Add(this.ToolBarMain);
			this.Name = "ContrFamily";
			this.Size = new System.Drawing.Size(939, 708);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrFamily_Layout);
			this.Resize += new System.EventHandler(this.ContrFamily_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary></summary>
		public void ModuleSelected(long patNum) {
			RefreshModuleData(patNum);
			RefreshModuleScreen();
			Plugins.HookAddCode(this,"ContrFamily.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			FamCur=null;
			PlanList=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrFamily.ModuleUnselected_end");
		}

		private void RefreshModuleData(long patNum) {
			if(patNum==0){
				PatCur=null;
				FamCur=null;
				PatPlanList=new List <PatPlan> (); 
				return;
			}
			FamCur=Patients.GetFamily(patNum);
			PatCur=FamCur.GetPatient(patNum);
			SubList=InsSubs.RefreshForFam(FamCur);
			PlanList=InsPlans.RefreshForSubList(SubList);
			PatPlanList=PatPlans.Refresh(patNum);
			BenefitList=Benefits.Refresh(PatPlanList,SubList);
			RecallList=Recalls.GetList(MiscUtils.ArrayToList<Patient>(FamCur.ListPats));
			PatFieldList=PatFields.Refresh(patNum);
			SuperFamilyMembers=Patients.GetBySuperFamily(PatCur.SuperFamily);
			SuperFamilyGuarantors=Patients.GetSuperFamilyGuarantors(PatCur.SuperFamily);
			if(_patNumLast!=patNum) {
				SecurityLogs.MakeLogEntry(Permissions.FamilyModule,patNum,"");
				_patNumLast=patNum;//Stops module from making too many logs
			}
			//Takes the preference string and converts it to an enum object
			_superFamSortStrat=(SortStrategy)PrefC.GetInt(PrefName.SuperFamSortStrategy);
		}

		private void RefreshModuleScreen(){
			//ParentForm.Text=Patients.GetMainTitle(PatCur);
			if(PatCur!=null){//if there is a patient
				//ToolBarMain.Buttons["Recall"].Enabled=true;
				ToolBarMain.Buttons["Add"].Enabled=true;
				ToolBarMain.Buttons["Delete"].Enabled=true;
				ToolBarMain.Buttons["Guarantor"].Enabled=true;
				ToolBarMain.Buttons["Move"].Enabled=true;
				if(ToolBarMain.Buttons["Ins"]!=null && !PrefC.GetBool(PrefName.EasyHideInsurance)) {
					ToolBarMain.Buttons["Ins"].Enabled=true;
				}
				if(ToolBarMain.Buttons["AddSuper"]==null){//because the toolbar only refreshes on restart. //PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)){
					gridSuperFam.Visible=false;
				}
				else{
					ToolBarMain.Buttons["AddSuper"].Enabled=true;
					ToolBarMain.Buttons["RemoveSuper"].Enabled=true;
					ToolBarMain.Buttons["DisbandSuper"].Enabled=true;
					if(PatCur.SuperFamily!=0) {//show Super Family Grid
						gridSuperFam.Visible=true;
						gridIns.Location=new Point(gridSuperFam.Right+2,gridIns.Top);// new Point(585,129);//gridIns (X,Y) normally=(254,129) reduced=(585,129)
						gridIns.Width=this.Width-gridIns.Left;//585;;
					}
					else {//Hide super family grid
						gridSuperFam.Visible=false;
						gridIns.Location=gridSuperFam.Location;//254,129);//gridIns (X,Y) normally=(254,129) reduced=(585,129)
						gridIns.Width=this.Width-gridIns.Left;//254;
					}
				}
				if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
					ToolBarMain.Buttons["SynchClone"].Enabled=true;
				}
				ToolBarMain.Invalidate();
			}
			else{
				//Hide super family grid, safe to run even if grid is already hidden.
				gridSuperFam.Visible=false;
				gridIns.Location=gridSuperFam.Location;//254,129);//gridIns (X,Y) normally=(254,129) reduced=(585,129)
				gridIns.Width=this.Width-gridIns.Left;//254;
				//ToolBarMain.Buttons["Recall"].Enabled=false;
				ToolBarMain.Buttons["Add"].Enabled=false;
				ToolBarMain.Buttons["Delete"].Enabled=false;
				ToolBarMain.Buttons["Guarantor"].Enabled=false;
				ToolBarMain.Buttons["Move"].Enabled=false;
				if(ToolBarMain.Buttons["AddSuper"]!=null) {//because the toolbar only refreshes on restart.
					ToolBarMain.Buttons["AddSuper"].Enabled=false;
					ToolBarMain.Buttons["RemoveSuper"].Enabled=false;
					ToolBarMain.Buttons["DisbandSuper"].Enabled=false;
				}
				if(ToolBarMain.Buttons["Ins"]!=null && !PrefC.GetBool(PrefName.EasyHideInsurance)) {
					ToolBarMain.Buttons["Ins"].Enabled=false;
				}
				if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
					ToolBarMain.Buttons["SynchClone"].Enabled=false;
				}
				ToolBarMain.Invalidate();
				//Patients.Cur=new Patient();
			}
			if(PrefC.GetBool(PrefName.EasyHideInsurance)){
				gridIns.Visible=false;
			}
			else{
				gridIns.Visible=true;
			}
			//Cannot add new patients from OD select patient interface.  Patient must be added from HL7 message.
			if(HL7Defs.IsExistingHL7Enabled()) {
				HL7Def def=HL7Defs.GetOneDeepEnabled();
				if(def.ShowDemographics!=HL7ShowDemographics.ChangeAndAdd) {
					ToolBarMain.Buttons["Add"].Enabled=false;
					ToolBarMain.Buttons["Delete"].Enabled=false;
					if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
						ToolBarMain.Buttons["SynchClone"].Enabled=false;
					}
				}
			}
			else {
				if(Programs.UsingEcwFullMode()) {
					ToolBarMain.Buttons["Add"].Enabled=false;
					ToolBarMain.Buttons["Delete"].Enabled=false;
					if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
						ToolBarMain.Buttons["SynchClone"].Enabled=false;
					}
				}
			}
			FillPatientPicture();
			FillPatientData();
			FillFamilyData();
			FillGridRecall();
			FillInsData();
			FillGridSuperFam();
			Plugins.HookAddCode(this,"ContrFamily.RefreshModuleScreen_end");
		} 

		private void FillPatientPicture(){
			picturePat.Image=null;
			picturePat.TextNullImage=Lan.g(this,"Patient Picture Unavailable");
			if(PatCur==null || 
				!PrefC.AtoZfolderUsed){//Do not use patient image when A to Z folders are disabled.
				return;
			}
			try{
				Bitmap patPict;
				Documents.GetPatPict(	PatCur.PatNum,ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath()),out patPict);
				picturePat.Image=patPict;
			}
			catch{
			}
		}

		///<summary></summary>
		public void InitializeOnStartup(){
			if(InitializedOnStartup) {
				return;
			}
			InitializedOnStartup=true;
			//tbFamily.InstantClasses();
			//cannot use Lan.F(this);
			Lan.C(this,new Control[]
				{
					//butPatEdit,
					//butEditPriCov,
					//butEditPriPlan,
					//butEditSecCov,
					//butEditSecPlan,
					gridFamily,
					gridRecall,
					gridPat,
					gridSuperFam,
					gridIns,
				});
			LayoutToolBar();
			//gridPat.Height=this.ClientRectangle.Bottom-gridPat.Top-2;
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Recall"),1,"","Recall"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Family Members:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add"),2,"Add Family Member","Add"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Delete"),3,Lan.g(this,"Delete Family Member"),"Delete"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Set Guarantor"),4,Lan.g(this,"Set as Guarantor"),"Guarantor"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Move"),5,Lan.g(this,"Move to Another Family"),"Move"));
			if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Synch Clone"),-1,Lan.g(this,"Synch information to the clone patient or create a clone of the currently selected patient if one does not exist"),"SynchClone"));
			}
			if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Super Family:"),-1,"","");
				button.Style=ODToolBarButtonStyle.Label;
				ToolBarMain.Buttons.Add(button);
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add"),-1,"Add selected patient to a super family","AddSuper"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Remove"),-1,Lan.g(this,"Remove selected patient, and their family, from super family"),"RemoveSuper"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Disband"),-1,Lan.g(this,"Disband the current super family by removing all members of the super family."),"DisbandSuper"));
			}
			if(!PrefC.GetBool(PrefName.EasyHideInsurance)){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Add Insurance"),6,"","Ins");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuInsurance;
				ToolBarMain.Buttons.Add(button);
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.FamilyModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrFamily.LayoutToolBar_end",PatCur);
		}

		private void ContrFamily_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			
		}

		private void ContrFamily_Resize(object sender,EventArgs e) {
			/*if(Height>gridPat.Top){
				gridPat.Height=Height-gridPat.Top-2;
				gridIns.Height=Height-gridIns.Top-2;
			}
			if(Width>gridIns.Left){
				gridIns.Width=Width-gridIns.Left-2;
			}*/
		}

		//private void butOutlook_Click(object sender, System.EventArgs e) {
			/*Process[] procsOutlook = Process.GetProcessesByName("outlook");
			if(procsOutlook.Length==0){
				try{
					Process.Start("Outlook");
				}
				catch{}
			}*/
		//}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					//case "Recall":
					//	ToolButRecall_Click();
					//	break;
					case "Add":
						ToolButAdd_Click();
						break;
					case "Delete":
						ToolButDelete_Click();
						break;
					case "Guarantor":
						ToolButGuarantor_Click();
						break;
					case "Move":
						ToolButMove_Click();
						break;
					case "Ins":
						ToolButIns_Click();
						break;
					case "AddSuper":
						ToolButAddSuper_Click();
						break;
					case "RemoveSuper":
						ToolButRemoveSuper_Click();
						break;
					case "DisbandSuper":
						ToolButDisbandSuper_Click();
						break;
					case "SynchClone":
						ToolButSynchClone_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,PatCur);
			}
		}

		///<summary>Public so it can be used from plugin.</summary>
		public void OnPatientSelected(Patient pat){
			PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(pat);
			if(PatientSelected!=null){
				PatientSelected(this,eArgs);
			}
		}

		#region gridPatient

		private void gridPat_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(Plugins.HookMethod(this,"ContrFamily.gridPat_CellDoubleClick",PatCur)) {
				return;
			}
			if(TerminalActives.PatIsInUse(PatCur.PatNum)){
				MsgBox.Show(this,"Patient is currently entering info at a reception terminal.  Please try again later.");
				return;
			}
			if(gridPat.Rows[e.Row].Tag!=null){
				if(gridPat.Rows[e.Row].Tag.ToString()=="Referral"){
					//RefAttach refattach=(RefAttach)gridPat.Rows[e.Row].Tag;
					FormReferralsPatient FormRE=new FormReferralsPatient();
					FormRE.PatNum=PatCur.PatNum;
					FormRE.ShowDialog();
				}
				else if(gridPat.Rows[e.Row].Tag.ToString()=="References") {
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
				}
				else if(gridPat.Rows[e.Row].Tag.GetType()==typeof(CustRefEntry)) {
					FormReferenceEntryEdit FormRE=new FormReferenceEntryEdit((CustRefEntry)gridPat.Rows[e.Row].Tag);
					FormRE.ShowDialog();
				}
				else if(gridPat.Rows[e.Row].Tag.ToString().Equals("Payor Types")) {
					FormPayorTypes FormPT = new FormPayorTypes();
					FormPT.PatCur=PatCur;
					FormPT.ShowDialog();
				}
				else {//patfield
					string tag=gridPat.Rows[e.Row].Tag.ToString();
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
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Currency) {
							FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(field);
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
						if(PatFieldDefs.ListShort[index].FieldType==PatFieldType.Currency) {
							FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(field);
							FormPF.ShowDialog();
						}
					}
				}
			}
			else{
				string email=PatCur.Email;
				long siteNum=PatCur.SiteNum;
				//
				FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
				FormP.IsNew=false;
				FormP.ShowDialog();
				//there are many things which may have changed that need to trigger refresh:
				//FName, LName, MiddleI, Preferred, SiteNum, or ChartNumber should refresh title bar.
				//Email change should change email but enabled.
				//Instead of checking for each of those:
				/*
				if(email!=PatCur.Email){//PatCur.EmailChanged){//do it this way later
					OnPatientSelected(PatCur.PatNum,PatCur.GetNameLF(),PatCur.Email!="",PatCur.ChartNumber);
				}
				if(siteNum!=PatCur.SiteNum){
					OnPatientSelected(PatCur.PatNum,PatCur.GetNameLF(),PatCur.Email!="",PatCur.ChartNumber);
				}*/
				if(FormP.DialogResult==DialogResult.OK) {
					OnPatientSelected(PatCur);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void gridPat_CellClick(object sender,ODGridClickEventArgs e) {
			ODGridCell gridCellCur=gridPat.Rows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined. 
			//If we support color and underline in the future, this might be changed to a regex of the cell text.
			if(gridCellCur.ColorText==System.Drawing.Color.Blue && gridCellCur.Underline==YN.Yes && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		private void FillPatientData(){
			if(PatCur==null){
				gridPat.BeginUpdate();
				gridPat.Rows.Clear();
				gridPat.Columns.Clear();
				gridPat.EndUpdate();
				return;
			}
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);
			gridPat.Columns.Add(col);
			col=new ODGridColumn("",150);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			ODGridRow row;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation);
			for(int f=0;f<fields.Count;f++) {
				row=new ODGridRow();
				if(fields[f].Description==""){
					if(fields[f].InternalName=="SS#"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("SIN");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("");
						}
						else{
							row.Cells.Add("SS#");
						}
					}
					else if(fields[f].InternalName=="State"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("Province");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("");
						}
						else{
							row.Cells.Add("State");
						}
					}
					else if(fields[f].InternalName=="Zip"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("Postal Code");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("Postcode");
						}
						else{
							row.Cells.Add(Lan.g("TablePatient","Zip"));
						}
					}
					else if(fields[f].InternalName=="PatFields"){
						//don't add a cell
					}
					else{
						row.Cells.Add(fields[f].InternalName);
					}
				}
				else{
					if(fields[f].InternalName=="PatFields") {
						//don't add a cell
					}
					else {
						row.Cells.Add(fields[f].Description);
					}
				}
				switch(fields[f].InternalName){
					case "Last":
						row.Cells.Add(PatCur.LName);
						break;
					case "First":
						row.Cells.Add(PatCur.FName);
						break;
					case "Middle":
						row.Cells.Add(PatCur.MiddleI);
						break;
					case "Preferred":
						row.Cells.Add(PatCur.Preferred);
						break;
					case "Title":
						row.Cells.Add(PatCur.Title);
						break;
					case "Salutation":
						row.Cells.Add(PatCur.Salutation);
						break;
					case "Status":
						row.Cells.Add(PatCur.PatStatus.ToString());
						if(PatCur.PatStatus==PatientStatus.Deceased) {
							row.ColorText=Color.Red;
						}
						break;
					case "Gender":
						row.Cells.Add(PatCur.Gender.ToString());
						break;
					case "Position":
						row.Cells.Add(PatCur.Position.ToString());
						break;
					case "Birthdate":
						if(PatCur.Birthdate.Year < 1880){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add(PatCur.Birthdate.ToString("d"));
						}
						break;
					case "Age":
						row.Cells.Add(PatientLogic.DateToAgeString(PatCur.Birthdate,PatCur.DateTimeDeceased));
						break;
					case "SS#":
						if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="US" 
							&& PatCur.SSN !=null && PatCur.SSN.Length==9)
						{
							row.Cells.Add(PatCur.SSN.Substring(0,3)+"-"+PatCur.SSN.Substring(3,2)+"-"+PatCur.SSN.Substring(5,4));
						}
						else {
							row.Cells.Add(PatCur.SSN);
						}
						break;
					case "Address":
						row.Cells.Add(PatCur.Address);
						row.Bold=true;
						break;
					case "Address2":
						row.Cells.Add(PatCur.Address2);
						break;
					case "City":
						row.Cells.Add(PatCur.City);
						break;
					case "State":
						row.Cells.Add(PatCur.State);
						break;
					case "Country":
						row.Cells.Add(PatCur.Country);
						break;
					case "Zip":
						row.Cells.Add(PatCur.Zip);
						break;
					case "Hm Phone":
						row.Cells.Add(PatCur.HmPhone);
						if(PatCur.PreferContactMethod==ContactMethod.HmPhone || PatCur.PreferContactMethod==ContactMethod.None){
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					case "Wk Phone":
						row.Cells.Add(PatCur.WkPhone);
						if(PatCur.PreferContactMethod==ContactMethod.WkPhone) {
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					case "Wireless Ph":
						row.Cells.Add(PatCur.WirelessPhone);
						if(PatCur.PreferContactMethod==ContactMethod.WirelessPh) {
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					case "E-mail":
						row.Cells.Add(PatCur.Email);
						if(PatCur.PreferContactMethod==ContactMethod.Email) {
							row.Bold=true;
						}
						break;
					case "Contact Method":
						row.Cells.Add(PatCur.PreferContactMethod.ToString());
						if(PatCur.PreferContactMethod==ContactMethod.DoNotCall || PatCur.PreferContactMethod==ContactMethod.SeeNotes){
							row.Bold=true;
						}
						break;
					case "ABC0":
						row.Cells.Add(PatCur.CreditType);
						break;
					case "Chart Num":
						row.Cells.Add(PatCur.ChartNumber);
						break;
					case "Billing Type":
						row.Cells.Add(DefC.GetName(DefCat.BillingTypes,PatCur.BillingType));
						break;
					case "Ward":
						row.Cells.Add(PatCur.Ward);
						break;
					case "AdmitDate":
						row.Cells.Add(PatCur.AdmitDate.ToShortDateString());
						break;
					case "Primary Provider":
						if(PatCur.PriProv!=0) {
							row.Cells.Add(Providers.GetLongDesc(Patients.GetProvNum(PatCur)));
						}
						else {
							row.Cells.Add(Lan.g("TablePatient","None"));
						}
						break;
					case "Sec. Provider":
						if(PatCur.SecProv != 0){
							row.Cells.Add(Providers.GetLongDesc(PatCur.SecProv));
						}
						else{
							row.Cells.Add(Lan.g("TablePatient","None"));
						}
						break;
					case "Payor Types":
						row.Tag="Payor Types";
						row.Cells.Add(PayorTypes.GetCurrentDescription(PatCur.PatNum));
						break;
					case "Language":
						if(PatCur.Language=="" || PatCur.Language==null){
							row.Cells.Add("");
						}
						else{
							try {
								row.Cells.Add(CodeBase.MiscUtils.GetCultureFromThreeLetter(PatCur.Language).DisplayName);
								//row.Cells.Add(CultureInfo.GetCultureInfo(PatCur.Language).DisplayName);
							}
							catch {
								row.Cells.Add(PatCur.Language);
							}
						}
						break;
					case "Clinic":
						row.Cells.Add(Clinics.GetDesc(PatCur.ClinicNum));
						break;
					case "ResponsParty":
						if(PatCur.ResponsParty==0){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add(Patients.GetLim(PatCur.ResponsParty).GetNameLF());
						}
						row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
						break;
					case "Referrals":
						List<RefAttach> RefList=RefAttaches.Refresh(PatCur.PatNum);
						if(RefList.Count==0){
							row.Cells.Add(Lan.g("TablePatient","None"));
							row.Tag="Referral";
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
						}
						//else{
						//	row.Cells.Add("");
						//	row.Tag="Referral";
						//	row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
						//}
						for(int i=0;i<RefList.Count;i++) {
							row=new ODGridRow();
							if(RefList[i].IsFrom){
								row.Cells.Add(Lan.g("TablePatient","Referred From"));
							}
							else{
								row.Cells.Add(Lan.g("TablePatient","Referred To"));
							}
							try{
								string refInfo=Referrals.GetNameLF(RefList[i].ReferralNum);
								string phoneInfo=Referrals.GetPhone(RefList[i].ReferralNum);
								if(phoneInfo!="" || RefList[i].Note!=""){
									refInfo+="\r\n"+phoneInfo+" "+RefList[i].Note;
								}
								row.Cells.Add(refInfo);
							}
							catch{
								row.Cells.Add("");//if referral is null because using random keys and had bug.
							}
							row.Tag="Referral";
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
							if(i<RefList.Count-1){
								gridPat.Rows.Add(row);
							}
						}
						break;
					case "Addr/Ph Note":
						row.Cells.Add(PatCur.AddrNote);
						if(PatCur.AddrNote!=""){
							row.ColorText=Color.Red;
							row.Bold=true;
						}
						break;
					case "Guardians":
						List<Guardian> guardianList=Guardians.Refresh(PatCur.PatNum);
						string str="";
						for(int g=0;g<guardianList.Count;g++) {
							if(!guardianList[g].IsGuardian) {
								continue;
							}
							if(g>0) {
								str+=",";
							}
							str+=FamCur.GetNameInFamFirst(guardianList[g].PatNumGuardian)+Guardians.GetGuardianRelationshipStr(guardianList[g].Relationship);
						}
						row.Cells.Add(str);
						break;
					case "PatFields":
						PatField field;
						for(int i=0;i<PatFieldDefs.ListShort.Count;i++) {
							if(i>0){
								row=new ODGridRow();
							}
							row.Cells.Add(PatFieldDefs.ListShort[i].FieldName);
							field=PatFields.GetByName(PatFieldDefs.ListShort[i].FieldName,PatFieldList);
							if(field==null){
								row.Cells.Add("");
							}
							else{
								if(PatFieldDefs.ListShort[i].FieldType==PatFieldType.Checkbox) {
									row.Cells.Add("X");
								}
								else if(PatFieldDefs.ListShort[i].FieldType==PatFieldType.Currency) {
									row.Cells.Add(PIn.Double(field.FieldValue).ToString("c"));
								}
								else {
									row.Cells.Add(field.FieldValue);
								}
							}
							row.Tag="PatField"+i.ToString();
							gridPat.Rows.Add(row);
						}
						break;
					case "Arrive Early":
						if(PatCur.AskToArriveEarly==0){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add(PatCur.AskToArriveEarly.ToString());
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
							gridPat.Rows.Add(row);
						}
						for(int i=0;i<custREList.Count;i++) {
							row=new ODGridRow();
							if(custREList[i].PatNumRef==PatCur.PatNum) {
								row.Cells.Add(custREList[i].DateEntry.ToShortDateString());
								row.Cells.Add("For: "+CustReferences.GetCustNameFL(custREList[i].PatNumCust));
							}
							else {
								row.Cells.Add("");
								row.Cells.Add(CustReferences.GetCustNameFL(custREList[i].PatNumRef));
							}
							row.Tag=custREList[i];
							row.ColorBackG=DefC.Short[(int)DefCat.MiscColors][8].ItemColor;
							if(i<custREList.Count-1) {
								gridPat.Rows.Add(row);
							}
						}
						break;
					case "Super Head":
						string fieldVal="";
						if(PatCur.SuperFamily!=0) {
							Patient supHead=Patients.GetPat(PatCur.SuperFamily);
							fieldVal=supHead.GetNameLF()+" ("+supHead.PatNum+")";
						}
						row.Cells.Add(fieldVal);
						break;
				}
				if(fields[f].InternalName=="PatFields"){
					//don't add the row here
				}
				else{
					gridPat.Rows.Add(row);
				}
			}
			gridPat.EndUpdate();
		}

		#endregion gridPatient 

		#region gridFamily

		private void FillFamilyData(){
			gridFamily.BeginUpdate();
			gridFamily.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePatient","Name"),140);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Position"),65);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Gender"),55);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Status"),65);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Age"),45);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Recall Due"),80);
			gridFamily.Columns.Add(col);
			gridFamily.Rows.Clear();
			if(PatCur==null){
				gridFamily.EndUpdate();
				return;
			}
			ODGridRow row;
			DateTime recallDate;
			ODGridCell cell;
			for(int i=0;i<FamCur.ListPats.Length;i++){
				row=new ODGridRow();
				row.Cells.Add(FamCur.GetNameInFamLFI(i));
				row.Cells.Add(Lan.g("enumPatientPosition",FamCur.ListPats[i].Position.ToString()));
				row.Cells.Add(Lan.g("enumPatientGender",FamCur.ListPats[i].Gender.ToString()));
				row.Cells.Add(Lan.g("enumPatientStatus",FamCur.ListPats[i].PatStatus.ToString()));
				row.Cells.Add(Patients.AgeToString(FamCur.ListPats[i].Age));
				recallDate=DateTime.MinValue;
				for(int j=0;j<RecallList.Count;j++){
					if(RecallList[j].PatNum==FamCur.ListPats[i].PatNum
						&& (RecallList[j].RecallTypeNum==PrefC.GetLong(PrefName.RecallTypeSpecialProphy)
						|| RecallList[j].RecallTypeNum==PrefC.GetLong(PrefName.RecallTypeSpecialPerio)))
					{
						recallDate=RecallList[j].DateDue;
					}
				}
				cell=new ODGridCell();
				if(recallDate.Year>1880){
					cell.Text=recallDate.ToShortDateString();
					if(recallDate<DateTime.Today){
						cell.Bold=YN.Yes;
						cell.ColorText=Color.Firebrick;
					}
				}
				row.Cells.Add(cell);
				if(i==0){//guarantor
					row.Bold=true;
				}
				gridFamily.Rows.Add(row);
			}
			gridFamily.EndUpdate();
			gridFamily.SetSelected(FamCur.GetIndex(PatCur.PatNum),true);
		}

		private void gridFamily_CellClick(object sender,ODGridClickEventArgs e) {
			//if (tbFamily.SelectedRow != -1){
			//	tbFamily.ColorRow(tbFamily.SelectedRow,Color.White);
			//}
			//tbFamily.SelectedRow=e.Row;
			//tbFamily.ColorRow(e.Row,Color.DarkSalmon);
			OnPatientSelected(FamCur.ListPats[e.Row]);
			ModuleSelected(FamCur.ListPats[e.Row].PatNum);
		}

		private void gridFamily_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
			FormP.IsNew=false;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				OnPatientSelected(PatCur);
			}
			ModuleSelected(PatCur.PatNum);
		}

		//private void butAddPt_Click(object sender, System.EventArgs e) {
		private void ToolButAdd_Click() {
			//At HQ, we cannot allow users to add patients to reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot add patients to a reseller family.");
				return;
			}
			Patient tempPat=new Patient();
			tempPat.LName      =PatCur.LName;
			tempPat.PatStatus  =PatientStatus.Patient;
			tempPat.Address    =PatCur.Address;
			tempPat.Address2   =PatCur.Address2;
			tempPat.City       =PatCur.City;
			tempPat.State      =PatCur.State;
			tempPat.Zip        =PatCur.Zip;
			tempPat.HmPhone    =PatCur.HmPhone;
			tempPat.Guarantor  =PatCur.Guarantor;
			tempPat.CreditType =PatCur.CreditType;
			if(!PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				tempPat.PriProv  =PatCur.PriProv;
			}
			tempPat.SecProv    =PatCur.SecProv;
			tempPat.FeeSched   =PatCur.FeeSched;
			tempPat.BillingType=PatCur.BillingType;
			tempPat.AddrNote   =PatCur.AddrNote;
			tempPat.ClinicNum  =PatCur.ClinicNum;//this is probably better in case they don't have user.ClinicNums set.
			//tempPat.ClinicNum  =Security.CurUser.ClinicNum;
			if(Patients.GetPat(tempPat.Guarantor).SuperFamily!=0) {
				tempPat.SuperFamily=PatCur.SuperFamily;
			}
			Patients.Insert(tempPat,false);
			SecurityLogs.MakeLogEntry(Permissions.PatientCreate,tempPat.PatNum,"Created from Family Module Add button.");
			CustReference custRef=new CustReference();
			custRef.PatNum=tempPat.PatNum;
			CustReferences.Insert(custRef);
			//add the tempPat to the FamCur list, but ModuleSelected below will refill the FamCur list in case the user cancels and tempPat is deleted
			//This would be a faster way to add to the array, but since it is not a pattern that is used anywhere we will use the alternate method of
			//creating a list, adding the patient, and converting back to an array
			//Array.Resize(ref FamCur.ListPats,FamCur.ListPats.Length+1);
			//FamCur.ListPats[FamCur.ListPats.Length-1]=tempPat;
			//Adding the temp patient to the FamCur.ListPats without calling GetFamily which makes a call to the db
			List<Patient> listPatsTemp=FamCur.ListPats.ToList();
			listPatsTemp.Add(tempPat);
			FamCur.ListPats=listPatsTemp.ToArray();
			FormPatientEdit FormPE=new FormPatientEdit(tempPat,FamCur);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK){
				OnPatientSelected(tempPat);
				ModuleSelected(tempPat.PatNum);
			}
			else{
				ModuleSelected(PatCur.PatNum);
			}
		}

		//private void butDeletePt_Click(object sender, System.EventArgs e) {
		private void ToolButDelete_Click(){
			//this doesn't actually delete the patient, just changes their status
			//and they will never show again in the patient selection list.
			//check for plans, appointments, procedures, etc.
			List<Procedure> procList=Procedures.Refresh(PatCur.PatNum);
			List<Appointment> apptList=Appointments.GetListForPat(PatCur.PatNum);
			List<Claim> claimList=Claims.Refresh(PatCur.PatNum);
			Adjustment[] AdjustmentList=Adjustments.Refresh(PatCur.PatNum);
			PaySplit[] PaySplitList=PaySplits.Refresh(PatCur.PatNum);//
			List<ClaimProc> claimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			List<Commlog> commlogList=Commlogs.Refresh(PatCur.PatNum);
			int payPlanCount=PayPlans.GetDependencyCount(PatCur.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(FamCur);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<MedicationPat> medList=MedicationPats.Refresh(PatCur.PatNum,false);
			PatPlanList=PatPlans.Refresh(PatCur.PatNum);
			//CovPats.Refresh(planList,PatPlanList);
			List<RefAttach> RefAttachList=RefAttaches.Refresh(PatCur.PatNum);
			List<Sheet> sheetList=Sheets.GetForPatient(PatCur.PatNum);
			RepeatCharge[] repeatChargeList=RepeatCharges.Refresh(PatCur.PatNum);
			List<CreditCard> listCreditCards=CreditCards.Refresh(PatCur.PatNum);
			RegistrationKey[] arrayRegistrationKeys=RegistrationKeys.GetForPatient(PatCur.PatNum);
			bool hasProcs=procList.Count>0;
			bool hasAppt=apptList.Count>0;
			bool hasClaims=claimList.Count>0;
			bool hasAdj=AdjustmentList.Length>0;
			bool hasPay=PaySplitList.Length>0;
			bool hasClaimProcs=claimProcList.Count>0;
			bool hasComm=commlogList.Count>0;
			bool hasPayPlans=payPlanCount>0;
			bool hasInsPlans=false;
			bool hasMeds=medList.Count>0;
			bool isSuperFamilyHead=PatCur.PatNum==PatCur.SuperFamily;
			for(int i=0;i<subList.Count;i++) {
				if(subList[i].Subscriber==PatCur.PatNum) {
					hasInsPlans=true;
				}
			}
			bool hasRef=RefAttachList.Count>0;
			bool hasSheets=sheetList.Count>0;
			bool hasRepeat=repeatChargeList.Length>0;
			bool hasCC=listCreditCards.Count>0;
			bool hasRegKey=arrayRegistrationKeys.Length>0;
			if(hasProcs || hasAppt || hasClaims || hasAdj || hasPay || hasClaimProcs || hasComm || hasPayPlans
				|| hasInsPlans || hasRef || hasMeds || isSuperFamilyHead || hasSheets || hasRepeat || hasCC || hasRegKey) 
			{
				string message=Lan.g(this,"You cannot delete this patient without first deleting the following data:")+"\r";
				if(hasProcs) {
					message+=Lan.g(this,"Procedures")+"\r";
				}
				if(hasAppt) {
					message+=Lan.g(this,"Appointments")+"\r";
				}
				if(hasClaims) {
					message+=Lan.g(this,"Claims")+"\r";
				}
				if(hasAdj) {
					message+=Lan.g(this,"Adjustments")+"\r";
				}
				if(hasPay) {
					message+=Lan.g(this,"Payments")+"\r";
				}
				if(hasClaimProcs) {
					message+=Lan.g(this,"Procedures attached to claims")+"\r";
				}
				if(hasComm) {
					message+=Lan.g(this,"Commlog entries")+"\r";
				}
				if(hasPayPlans) {
					message+=Lan.g(this,"Payment plans")+"\r";
				}
				if(hasInsPlans) {
					message+=Lan.g(this,"Insurance plans")+"\r";
				}
				if(hasRef) {
					message+=Lan.g(this,"References")+"\r";
				}
				if(hasMeds) {
					message+=Lan.g(this,"Medications")+"\r";
				}
				if(isSuperFamilyHead) {
					message+=Lan.g(this,"Attached Super Family")+"\r";
				}
				if(hasSheets) {
					message+=Lan.g(this,"Sheets")+"\r";
				}
				if(hasRepeat) {
					message+=Lan.g(this,"Repeating Charges")+"\r";
				}
				if(hasCC) {
					message+=Lan.g(this,"Credit Cards")+"\r";
				}
				if(hasRegKey) {
					message+=Lan.g(this,"Registration Keys")+"\r";
				}
				MessageBox.Show(message);
				return;
			}
			Patient PatOld=PatCur.Copy();
			if(PatCur.PatNum==PatCur.Guarantor){//if selecting guarantor
				if(FamCur.ListPats.Length==1){
					if(!MsgBox.Show(this,true,"Delete Patient?")) {
						return;
					}
					PatCur.PatStatus=PatientStatus.Deleted;
					PatCur.ChartNumber="";
					PatCur.ClinicNum=0;
					Popups.MoveForDeletePat(PatCur);
					PatCur.SuperFamily=0;
					Patients.Update(PatCur,PatOld);
					for(int i=0;i<RecallList.Count;i++){
						if(RecallList[i].PatNum==PatCur.PatNum){
							RecallList[i].IsDisabled=true;
							RecallList[i].DateDue=DateTime.MinValue;
							Recalls.Update(RecallList[i]);
						}
					}
					OnPatientSelected(new Patient());
					ModuleSelected(0);
					//does not delete notes or plans, etc.
				}
				else{
					MessageBox.Show(Lan.g(this,"You cannot delete the guarantor if there are other family members. You would have to make a different family member the guarantor first."));
				}
			}
			else{//not selecting guarantor
				if(!MsgBox.Show(this,true,"Delete Patient?")) {
					return;
				}
				PatCur.PatStatus=PatientStatus.Deleted;
				PatCur.ChartNumber="";
				PatCur.ClinicNum=0;
				Popups.MoveForDeletePat(PatCur);
				PatCur.Guarantor=PatCur.PatNum;
				PatCur.SuperFamily=0;
				Patients.Update(PatCur,PatOld);
				for(int i=0;i<RecallList.Count;i++){
					if(RecallList[i].PatNum==PatCur.PatNum){
						RecallList[i].IsDisabled=true;
						RecallList[i].DateDue=DateTime.MinValue;
						Recalls.Update(RecallList[i]);
					}
				}
				ModuleSelected(PatOld.Guarantor);//Sets PatCur to PatOld guarantor.
				OnPatientSelected(PatCur);//PatCur is now the Guarantor.
			}
			PatientL.RemoveFromMenu(PatOld.GetNameLF(),PatOld.PatNum);//Always remove deleted patients from the dropdown menu.
		}

		//private void butSetGuar_Click(object sender,System.EventArgs e){
		private void ToolButGuarantor_Click() {
			if(PatCur.PatNum==PatCur.Guarantor) {
				MessageBox.Show(Lan.g(this,"Patient is already the guarantor.  Please select a different family member."));
				return;
			}
			//At HQ, we cannot allow users to change the guarantor of reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot change the guarantor of a reseller family.");
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Make the selected patient the guarantor?")
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			if(PatCur.SuperFamily==PatCur.Guarantor) {//guarantor is also the head of a super family
				Patients.MoveSuperFamily(PatCur.SuperFamily,PatCur.PatNum);
			}
			Patients.ChangeGuarantorToCur(FamCur,PatCur);
			ModuleSelected(PatCur.PatNum);
		}

		//private void butMovePat_Click(object sender, System.EventArgs e) {
		private void ToolButMove_Click() {
			//At HQ, we cannot allow users to move patients of reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot move patients of a reseller family.");
				return;
			}
			Patient PatOld=PatCur.Copy();
			//Patient PatCur;
			if(PatCur.PatNum==PatCur.Guarantor){//if guarantor selected
				if(FamCur.ListPats.Length==1){//and no other family members
					//no need to check insurance.  It will follow.
					if(!MsgBox.Show(this,true,"Moving the guarantor will cause two families to be combined.  The financial notes for both families will be combined and may need to be edited.  The address notes will also be combined and may need to be edited. Do you wish to continue?")) {
						return;
					}
					if(!MsgBox.Show(this,true,"Select the family to move this patient to from the list that will come up next.")) {
						return;
					}
					FormPatientSelect FormPS=new FormPatientSelect();
					FormPS.SelectionModeOnly=true;
					FormPS.ShowDialog();
					if(FormPS.DialogResult!=DialogResult.OK){
						return;
					}
					Patient patInNewFam=Patients.GetPat(FormPS.SelectedPatNum);
					if(PatCur.SuperFamily!=patInNewFam.SuperFamily){//If they are moving into or out of a superfamily
						if(PatCur.SuperFamily!=0) {//If they are currently in a SuperFamily and moving out.  Otherwise, no superfamily popups to worry about.
							Popups.CopyForMovingSuperFamily(PatCur,patInNewFam.SuperFamily);
						}
					}
					PatCur.Guarantor=patInNewFam.Guarantor;
					PatCur.SuperFamily=patInNewFam.SuperFamily;
					Patients.Update(PatCur,PatOld);
					FamCur=Patients.GetFamily(PatCur.PatNum);
					Patients.CombineGuarantors(FamCur,PatCur);
				}
				else{//there are other family members
					MessageBox.Show(Lan.g(this,"You cannot move the guarantor.  If you wish to move the guarantor, you must make another family member the guarantor first."));
				}
			}
			else{//guarantor not selected
				if(!MsgBox.Show(this,true,"Preparing to move family member.  Financial notes and address notes will not be transferred.  Popups will be copied.  Proceed to next step?")){
					return;
				}
				switch(MessageBox.Show(Lan.g(this,"Create new family instead of moving to an existing family?"),"",MessageBoxButtons.YesNoCancel)){
					case DialogResult.Cancel:
						return;
					case DialogResult.Yes://new family (split)
						Popups.CopyForMovingFamilyMember(PatCur);//Copy Family Level Popups to new family. 
						//Don't need to copy SuperFamily Popups. Stays in same super family.
						PatCur.Guarantor=PatCur.PatNum;
						//keep current superfamily
						Patients.Update(PatCur,PatOld);
						break;
					case DialogResult.No://move to an existing family
						if(!MsgBox.Show(this,true,"Select the family to move this patient to from the list that will come up next.")){
							return;
						}
						FormPatientSelect FormPS=new FormPatientSelect();
						FormPS.SelectionModeOnly=true;
						FormPS.ShowDialog();
						if(FormPS.DialogResult!=DialogResult.OK){
							return;
						}						
						Patient patInNewFam=Patients.GetPat(FormPS.SelectedPatNum);
						if(patInNewFam.Guarantor==PatCur.Guarantor) {
							return;// Patient is already a part of the family.
						}
						Popups.CopyForMovingFamilyMember(PatCur);//Copy Family Level Popups to new Family. 
						if(PatCur.SuperFamily!=patInNewFam.SuperFamily){//If they are moving into or out of a superfamily
							if(PatCur.SuperFamily!=0) {//If they are currently in a SuperFamily.  Otherwise, no superfamily popups to worry about.
								Popups.CopyForMovingSuperFamily(PatCur,patInNewFam.SuperFamily);
							}
						}
						PatCur.Guarantor=patInNewFam.Guarantor;
						PatCur.SuperFamily=patInNewFam.SuperFamily;//assign to the new superfamily
						Patients.Update(PatCur,PatOld);
						break;
				}
			}//end guarantor not selected
			ModuleSelected(PatCur.PatNum);
		}

		///<summary>Synch specific data from the non-clone to the clone of the pair the selected patient belongs to.  The non-clone will have mixed case first and last names, while the clone will have all-caps.  The clone will be moved to the non-clone family.  The insurance plans currently attached to the non-clone will be updated to the clone.  The address, phone, and a few other demographics will be copied to the clone.  If no changes need to be made, the user will get a message that the patients are already in synch.  If any change happens to the clone, the user will see exactly what changes OD makes.  If the currently selected patient does not have a clone, a messsage box will ask the user if they want to create an all-caps clone of the patient.</summary>
		private void ToolButSynchClone_Click() {
			Patient patClone;
			Patient patNonClone;
			List<Patient> listAmbiguousMatches;
			Patient patCloneOld;
			Patients.GetCloneAndNonClone(PatCur,out patClone,out patNonClone,out listAmbiguousMatches);
			string strDataUpdated="";
			bool isNewClone=false;
			if(patClone==null) {
				if(listAmbiguousMatches.Count==0) {//no clone exists, ask if one should be created
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"The currently selected patient does not have a clone, would you like to create one?")) {
						return;
					}
					if(PatCur.LName==PatCur.LName.ToUpper() && PatCur.FName==PatCur.FName.ToUpper()) {
						MsgBox.Show(this,"In order to create a clone, the selected patient cannot have an all upper case first and last name.");
						return;
					}
					if(PatCur.Birthdate.Year<1880) {
						MsgBox.Show(this,"In order to create a clone, the selected patient must have a valid birthdate.");
						return;
					}
					//create clone
					patNonClone=PatCur.Copy();
					patClone=new Patient();
					patClone.LName=patNonClone.LName.ToUpper();
					patClone.FName=patNonClone.FName.ToUpper();
					patClone.MiddleI=patNonClone.MiddleI.ToUpper();
					patClone.Birthdate=patNonClone.Birthdate;
					//We intentionally don't synch the patient's provider since the clone feature is so the clone can be assigned to a different provider for tracking production.
					patClone.PriProv=PrefC.GetLong(PrefName.PracticeDefaultProv);
					long patClonePatNum=Patients.Insert(patClone,false);
					SecurityLogs.MakeLogEntry(Permissions.PatientCreate,patClonePatNum,"Created from Family Module SynchClone button.");
					patClone=Patients.GetPat(patClonePatNum);//this is so the fields not set will be refreshed to their non-null default value, i.e. '' instead of null
					strDataUpdated+=Lan.g(this,"The following patient was created")+": "
						+patClone.PatNum+" - "+Patients.GetNameFL(patClone.LName,patClone.FName,patClone.Preferred,patClone.MiddleI)+".\r\n";
					isNewClone=true;
				}
				else {
					strDataUpdated=Lan.g(this,"The synch cannot take place due to an issue matching the name and/or birthdate with one and only one other patient.")+"\r\n"
						+Lan.g(this,"The following patients are causing the ambiguity and must be corrected manually.")+"\r\n";
					for(int i=0;i<listAmbiguousMatches.Count;i++) {
						if(i>4) {//only show the first 5 matches, there may be a lot of matches.  One customer has a patient who cares for many other patients and is entered in the system 70 times.
							break;
						}
						strDataUpdated+=listAmbiguousMatches[i].PatNum+" - "
							+Patients.GetNameFL(listAmbiguousMatches[i].LName,listAmbiguousMatches[i].FName,listAmbiguousMatches[i].Preferred,listAmbiguousMatches[i].MiddleI)
							+", Birthdate "+listAmbiguousMatches[i].Birthdate.ToShortDateString()+"\r\n";
					}
					MessageBox.Show(strDataUpdated);
					return;
				}
			}
			//either clone was found and patCloneOld has been set to patClone or no clone existed and a new patient was inserted to be the new clone patient
			#region Synch Clone Data - Patient Demographics
			patCloneOld=patClone.Copy();
			string strPatCloneNumAndName=patClone.PatNum+" - "+Patients.GetNameFL(patClone.LName,patClone.FName,patClone.Preferred,patClone.MiddleI);
			List<string[]> listFieldsUpdated=new List<string[]>();//this is a list of string arrays, where the arrays hold the three values field name, old value, and new value for the fields updated 
			List<string> listFieldsCleared=new List<string>();
			if(patClone.Title!=patNonClone.Title) {
				if(patClone.Title!="" && patNonClone.Title=="") {
					listFieldsCleared.Add("Title");
				}
				listFieldsUpdated.Add(new string[3] { "Title",patClone.Title,patNonClone.Title });
				patClone.Title=patNonClone.Title;
			}
			if(patClone.Preferred!=patNonClone.Preferred.ToUpper()) {
				if(patClone.Preferred!="" && patNonClone.Preferred=="") {
					listFieldsCleared.Add("Preferred Name");
				}
				listFieldsUpdated.Add(new string[3] { "Preferred Name",patClone.Preferred,patNonClone.Preferred.ToUpper() });
				patClone.Preferred=patNonClone.Preferred.ToUpper();
			}
			if(patClone.MiddleI!=patNonClone.MiddleI.ToUpper()) {
				if(patClone.MiddleI!=""	&& patNonClone.MiddleI=="") {
					listFieldsCleared.Add("Middle Initial");
				}
				listFieldsUpdated.Add(new string[3] { "Middle Initial",patClone.MiddleI,patNonClone.MiddleI.ToUpper() });
				patClone.MiddleI=patNonClone.MiddleI.ToUpper();
			}
			if(patClone.Guarantor!=patNonClone.Guarantor) {
				Patient patCloneGuar=Patients.GetPat(patClone.Guarantor);
				Patient patNonCloneGuar=Patients.GetPat(patNonClone.Guarantor);
				string strPatCloneGuarName="";
				string strPatNonCloneGuarName="";
				if(patCloneGuar!=null) {
					strPatCloneGuarName=Patients.GetNameFL(patCloneGuar.LName,patCloneGuar.FName,patCloneGuar.Preferred,patCloneGuar.MiddleI);
				}
				if(patNonCloneGuar!=null) {
					strPatNonCloneGuarName=Patients.GetNameFL(patNonCloneGuar.LName,patNonCloneGuar.FName,patNonCloneGuar.Preferred,patNonCloneGuar.MiddleI);
				}
				listFieldsUpdated.Add(new string[3] { "Guarantor",patClone.Guarantor.ToString()+" - "+strPatCloneGuarName,patNonClone.Guarantor.ToString()+" - "+strPatNonCloneGuarName });
				patClone.Guarantor=patNonClone.Guarantor;
			}
			if(patClone.ResponsParty!=patNonClone.ResponsParty) {
				Patient patCloneRespPart=Patients.GetPat(patClone.ResponsParty);
				Patient patNonCloneRespPart=Patients.GetPat(patNonClone.ResponsParty);
				string strPatCloneRespPartName="";
				string strPatNonCloneRespPartName="";
				if(patCloneRespPart!=null) {
					strPatCloneRespPartName=Patients.GetNameFL(patCloneRespPart.LName,patCloneRespPart.FName,patCloneRespPart.Preferred,patCloneRespPart.MiddleI);
				}
				if(patNonCloneRespPart!=null) {
					strPatNonCloneRespPartName=Patients.GetNameFL(patNonCloneRespPart.LName,patNonCloneRespPart.FName,patNonCloneRespPart.Preferred,patNonCloneRespPart.MiddleI);
				}
				listFieldsUpdated.Add(new string[3] { "Responsible Party",patClone.ResponsParty.ToString()+" - "+strPatCloneRespPartName,patNonClone.ResponsParty.ToString()+" - "+strPatNonCloneRespPartName });
				patClone.ResponsParty=patNonClone.ResponsParty;
			}
			if(patClone.SuperFamily!=patNonClone.SuperFamily) {
				Patient patCloneSupFam=Patients.GetPat(patClone.SuperFamily);
				Patient patNonCloneSupFam=Patients.GetPat(patNonClone.SuperFamily);
				string strPatCloneSupFamName="";
				string strPatNonCloneSupFamName="";
				if(patCloneSupFam!=null) {
					strPatCloneSupFamName=Patients.GetNameFL(patCloneSupFam.LName,patCloneSupFam.FName,patCloneSupFam.Preferred,patCloneSupFam.MiddleI);
				}
				if(patNonCloneSupFam!=null) {
					strPatNonCloneSupFamName=Patients.GetNameFL(patNonCloneSupFam.LName,patNonCloneSupFam.FName,patNonCloneSupFam.Preferred,patNonCloneSupFam.MiddleI);
				}
				listFieldsUpdated.Add(new string[3] { "Super Family",patClone.SuperFamily.ToString()+" - "+strPatCloneSupFamName,patNonClone.SuperFamily.ToString()+" - "+strPatNonCloneSupFamName });
				patClone.SuperFamily=patNonClone.SuperFamily;
			}
			if(patClone.PatStatus!=patNonClone.PatStatus) {
				listFieldsUpdated.Add(new string[3] { "Patient Status",patClone.PatStatus.ToString(),patNonClone.PatStatus.ToString() });
				patClone.PatStatus=patNonClone.PatStatus;
			}
			if(patClone.Gender!=patNonClone.Gender) {
				listFieldsUpdated.Add(new string[3] { "Gender",patClone.Gender.ToString(),patNonClone.Gender.ToString() });
				patClone.Gender=patNonClone.Gender;
			}
			if(patClone.Language!=patNonClone.Language) {
				string strPatCloneLang="";
				string strPatNonCloneLang="";
				try {
					strPatCloneLang=CodeBase.MiscUtils.GetCultureFromThreeLetter(patClone.Language).DisplayName;
				}
				catch {
					strPatCloneLang=patClone.Language;
				}
				try {
					strPatNonCloneLang=CodeBase.MiscUtils.GetCultureFromThreeLetter(patNonClone.Language).DisplayName;
				}
				catch {
					strPatNonCloneLang=patNonClone.Language;
				}
				listFieldsUpdated.Add(new string[3] { "Language",strPatCloneLang,strPatNonCloneLang });
				patClone.Language=patNonClone.Language;
			}
			if(patClone.SSN!=patNonClone.SSN) {
				if(patClone.SSN!=""	&& patNonClone.SSN=="") {
					listFieldsCleared.Add("SSN");
				}
				listFieldsUpdated.Add(new string[3] { "SSN",patClone.SSN,patNonClone.SSN });
				patClone.SSN=patNonClone.SSN;
			}
			if(patClone.Position!=patNonClone.Position) {
				listFieldsUpdated.Add(new string[3] { "Position",patClone.Position.ToString(),patNonClone.Position.ToString() });
				patClone.Position=patNonClone.Position;
			}
			if(patClone.Address!=patNonClone.Address) {
				if(patClone.Address!=""	&& patNonClone.Address=="") {
					listFieldsCleared.Add("Address");
				}
				listFieldsUpdated.Add(new string[3] { "Address",patClone.Address,patNonClone.Address });
				patClone.Address=patNonClone.Address;
			}
			if(patClone.Address2!=patNonClone.Address2) {
				if(patClone.Address2!="" && patNonClone.Address2=="") {
					listFieldsCleared.Add("Address2");
				}
				listFieldsUpdated.Add(new string[3] { "Address2",patClone.Address2,patNonClone.Address2 });
				patClone.Address2=patNonClone.Address2;
			}
			if(patClone.City!=patNonClone.City) {
				if(patClone.City!="" && patNonClone.City=="") {
					listFieldsCleared.Add("City");
				}
				listFieldsUpdated.Add(new string[3] { "City",patClone.City,patNonClone.City });
				patClone.City=patNonClone.City;
			}
			if(patClone.State!=patNonClone.State) {
				if(patClone.State!=""	&& patNonClone.State=="") {
					listFieldsCleared.Add("State");
				}
				listFieldsUpdated.Add(new string[3] { "State",patClone.State,patNonClone.State });
				patClone.State=patNonClone.State;
			}
			if(patClone.Zip!=patNonClone.Zip) {
				if(patClone.Zip!=""	&& patNonClone.Zip=="") {
					listFieldsCleared.Add("Zip");
				}
				listFieldsUpdated.Add(new string[3] { "Zip",patClone.Zip,patNonClone.Zip });
				patClone.Zip=patNonClone.Zip;
			}
			if(patClone.County!=patNonClone.County) {
				if(patClone.County!=""	&& patNonClone.County=="") {
					listFieldsCleared.Add("County");
				}
				listFieldsUpdated.Add(new string[3] { "County",patClone.County,patNonClone.County });
				patClone.County=patNonClone.County;
			}
			if(patClone.AddrNote!=patNonClone.AddrNote) {
				if(patClone.AddrNote!=""	&& patNonClone.AddrNote=="") {
					listFieldsCleared.Add("Address Note");
				}
				listFieldsUpdated.Add(new string[3] { "Address Note",patClone.AddrNote,patNonClone.AddrNote });
				patClone.AddrNote=patNonClone.AddrNote;
			}
			if(patClone.HmPhone!=patNonClone.HmPhone) {
				if(patClone.HmPhone!=""	&& patNonClone.HmPhone=="") {
					listFieldsCleared.Add("Home Phone");
				}
				listFieldsUpdated.Add(new string[3] { "Home Phone",patClone.HmPhone,patNonClone.HmPhone });
				patClone.HmPhone=patNonClone.HmPhone;
			}
			if(patClone.WirelessPhone!=patNonClone.WirelessPhone) {
				if(patClone.WirelessPhone!=""	&& patNonClone.WirelessPhone=="") {
					listFieldsCleared.Add("Wireless Phone");
				}
				listFieldsUpdated.Add(new string[3] { "Wireless Phone",patClone.WirelessPhone,patNonClone.WirelessPhone });
				patClone.WirelessPhone=patNonClone.WirelessPhone;
			}
			if(patClone.WkPhone!=patNonClone.WkPhone) {
				if(patClone.WkPhone!=""	&& patNonClone.WkPhone=="") {
					listFieldsCleared.Add("Work Phone");
				}
				listFieldsUpdated.Add(new string[3] { "Work Phone",patClone.WkPhone,patNonClone.WkPhone });
				patClone.WkPhone=patNonClone.WkPhone;
			}
			if(patClone.Email!=patNonClone.Email) {
				if(patClone.Email!=""	&& patNonClone.Email=="") {
					listFieldsCleared.Add("Email");
				}
				listFieldsUpdated.Add(new string[3] { "Email",patClone.Email,patNonClone.Email });
				patClone.Email=patNonClone.Email;
			}
			if(patClone.TxtMsgOk!=patNonClone.TxtMsgOk) {
				listFieldsUpdated.Add(new string[3] { "TxtMsgOk",patClone.TxtMsgOk.ToString(),patNonClone.TxtMsgOk.ToString() });
				patClone.TxtMsgOk=patNonClone.TxtMsgOk;
			}
			if(patClone.BillingType!=patNonClone.BillingType) {
				string cloneBillType="";
				string nonCloneBillType="";
				for(int i=0;i<DefC.Long[(int)DefCat.BillingTypes].Length;i++){
					if(patClone.BillingType==DefC.Long[(int)DefCat.BillingTypes][i].DefNum) {
						cloneBillType=DefC.Long[(int)DefCat.BillingTypes][i].ItemName;
					}
					if(patNonClone.BillingType==DefC.Long[(int)DefCat.BillingTypes][i].DefNum) {
						nonCloneBillType=DefC.Long[(int)DefCat.BillingTypes][i].ItemName;
					}
				}
				listFieldsUpdated.Add(new string[3] { "Billing Type",cloneBillType,nonCloneBillType });
				patClone.BillingType=patNonClone.BillingType;
			}
			if(patClone.FeeSched!=patNonClone.FeeSched) {
				string cloneFeeSched="";
				string nonCloneFeeSched="";
				for(int i=0;i<FeeSchedC.ListLong.Count;i++) {
					if(patClone.FeeSched==FeeSchedC.ListLong[i].FeeSchedNum) {
						cloneFeeSched=FeeSchedC.ListLong[i].Description;
					}
					if(patNonClone.FeeSched==FeeSchedC.ListLong[i].FeeSchedNum) {
						nonCloneFeeSched=FeeSchedC.ListLong[i].Description;
					}
				}
				listFieldsUpdated.Add(new string[3] { "Fee Schedule",cloneFeeSched,nonCloneFeeSched });
				patClone.FeeSched=patNonClone.FeeSched;
			}
			if(patClone.CreditType!=patNonClone.CreditType) {
				if(patClone.CreditType!=""	&& patNonClone.CreditType=="") {
					listFieldsCleared.Add("Credit Type");
				}
				listFieldsUpdated.Add(new string[3] { "Credit Type",patClone.CreditType,patNonClone.CreditType });
				patClone.CreditType=patNonClone.CreditType;
			}
			if(patClone.MedicaidID!=patNonClone.MedicaidID) {
				if(patClone.MedicaidID!=""	&& patNonClone.MedicaidID=="") {
					listFieldsCleared.Add("Medicaid ID");
				}
				listFieldsUpdated.Add(new string[3] { "Medicaid ID",patClone.MedicaidID,patNonClone.MedicaidID });
				patClone.MedicaidID=patNonClone.MedicaidID;
			}
			if(patClone.MedUrgNote!=patNonClone.MedUrgNote) {
				if(patClone.MedUrgNote!=""	&& patNonClone.MedUrgNote=="") {
					listFieldsCleared.Add("Medical Urgent Note");
				}
				listFieldsUpdated.Add(new string[3] { "Medical Urgent Note",patClone.MedUrgNote,patNonClone.MedUrgNote });
				patClone.MedUrgNote=patNonClone.MedUrgNote;
			}
			if(!isNewClone && listFieldsCleared.Count>0) {//fields for the clone have data that is about to be overwritten by empty strings, ask if the user is sure
				string strMsg=Lan.g(this,"The following fields have data entered for the clone and will be overwritten with blanks from the original patient")+":\r\n";
				strMsg+=string.Join("\r\n",listFieldsCleared);
				strMsg+="\r\n"+Lan.g(this,"Are you sure you want to proceed?");
				if(MessageBox.Show(strMsg,"",MessageBoxButtons.YesNo)==DialogResult.No) {
					return;
				}
			}
			Patients.Update(patClone,patCloneOld);
			if(!isNewClone && listFieldsUpdated.Count>0) {
				strDataUpdated+=Lan.g(this,"The following patient demographic changes were made to the clone patient")+" "+strPatCloneNumAndName+":\r\n";
			}
			string strChngFrom=" "+Lan.g(this,"changed from")+" ";
			string strChngTo=" "+Lan.g(this,"to")+" ";
			string strBlank=Lan.g(this,"blank");
			for(int i=0;i<listFieldsUpdated.Count;i++) {
				if(isNewClone) {
					break;
				}
				strDataUpdated+=listFieldsUpdated[i][0]+strChngFrom;
				if(listFieldsUpdated[i][1]=="") {//blank filled with data
					strDataUpdated+=strBlank;
				}
				else {
					strDataUpdated+=listFieldsUpdated[i][1];
				}
				strDataUpdated+=strChngTo;
				if(listFieldsUpdated[i][2]=="") {
					strDataUpdated+=strBlank+"\r\n";
				}
				else {
					strDataUpdated+=listFieldsUpdated[i][2]+"\r\n";
				}
			}
			#endregion Synch Clone Data - Patient Demographics
			#region Synch Clone Data - PatPlans
			bool patPlansChanged=false;
			List<PatPlan> listPatPlansNonClone=PatPlans.Refresh(patNonClone.PatNum);//ordered by ordinal
			List<PatPlan> listPatPlansClone=PatPlans.Refresh(patClone.PatNum);//ordered by ordinal
			//the clone patient has more insurance plans entered than the original patient, ask the user if they are sure they want to make this change.
			//There could be a plan entered on the clone instead of the original by accident and they might not want to lose that data.
			if(listPatPlansClone.Count>listPatPlansNonClone.Count) {
				string strMsg=Lan.g(this,"The clone patient has")+" "+listPatPlansClone.Count.ToString()+" "
					+Lan.g(this,"insurance plan(s) attached while the original patient has")+" "+listPatPlansNonClone.Count.ToString()+" "
					+Lan.g(this,"plan(s).  The clone patient's additional plan(s) will be dropped.")+"\r\n"
					+Lan.g(this,"Are you sure you want to proceed?");
				if(MessageBox.Show(strMsg,"",MessageBoxButtons.YesNo)==DialogResult.No) {
					return;
				}
			}
			List<Claim> claimList=Claims.Refresh(patClone.PatNum);//used to determine if the patplan we are going to drop is attached to a claim with today's date
			for(int i=claimList.Count-1;i>-1;i--) {//remove any claims that do not have a date of today, we are only concerned with claims with today's date
				if(claimList[i].DateService==DateTime.Today) {
					continue;
				}
				claimList.RemoveAt(i);
			}
			//if the clone has more patplans than the non-clone, drop the additional patplans
			//we will compute all estimates for the clone after all of the patplan adding/dropping/rearranging
			for(int i=listPatPlansClone.Count-1;i>listPatPlansNonClone.Count-1;i--) {
				InsSub insSubCloneCur=InsSubs.GetOne(listPatPlansClone[i].InsSubNum);
				//we will drop the clone's patplan because the clone has more patplans than the non-clone
				//before we can drop the plan, we have to make sure there is not a claim with today's date
				bool isAttachedToClaim=false;
				for(int j=0;j<claimList.Count;j++) {//claimList will only contain claims with DateService=Today
					if(claimList[j].PlanNum!=insSubCloneCur.PlanNum) {//different insplan
						continue;
					}
					strDataUpdated+=Lan.g(this,"The clone's currently attached insurance does not match.  Due to a claim with today's date we cannot synch the plans, the issue must be corrected manually on the following plan")+": "+InsPlans.GetDescript(insSubCloneCur.PlanNum,FamCur,PlanList,listPatPlansClone[i].InsSubNum,SubList)+".\r\n";
					isAttachedToClaim=true;
					break;
				}
				if(isAttachedToClaim) {//we will continue trying to drop non-clone additional plans, but only if no claim for today exists
					continue;
				}
				strDataUpdated+=Lan.g(this,"The following insurance plan was dropped from the clone patient due to it not existing with the same ordinal on the original patient")+": "
					+InsPlans.GetDescript(insSubCloneCur.PlanNum,FamCur,PlanList,listPatPlansClone[i].InsSubNum,SubList)+".\r\n";
				patPlansChanged=true;
				PatPlans.DeleteNonContiguous(listPatPlansClone[i].PatPlanNum);
				listPatPlansClone.RemoveAt(i);
			}
			for(int i=0;i<listPatPlansNonClone.Count;i++) {
				InsSub insSubNonCloneCur=InsSubs.GetOne(listPatPlansNonClone[i].InsSubNum);
				string insPlanNonCloneDescriptCur=InsPlans.GetDescript(insSubNonCloneCur.PlanNum,FamCur,PlanList,listPatPlansNonClone[i].InsSubNum,SubList);
				if(listPatPlansClone.Count<i+1) {//if there is not a PatPlan at this ordinal position for the clone, add a new one that is an exact copy, with correct PatNum of course
					PatPlan patPlanNew=listPatPlansNonClone[i].Copy();
					patPlanNew.PatNum=patClone.PatNum;
					PatPlans.Insert(patPlanNew);
					strDataUpdated+=Lan.g(this,"The following insurance was added to the clone patient")+": "+insPlanNonCloneDescriptCur+".\r\n";
					patPlansChanged=true;
					continue;
				}
				InsSub insSubCloneCur=InsSubs.GetOne(listPatPlansClone[i].InsSubNum);
				string insPlanCloneDescriptCur=InsPlans.GetDescript(insSubCloneCur.PlanNum,FamCur,PlanList,listPatPlansClone[i].InsSubNum,SubList);
				if(listPatPlansNonClone[i].InsSubNum!=listPatPlansClone[i].InsSubNum) {//both pats have a patplan at this ordinal, but the clone's is pointing to a different inssub
					//we will drop the clone's patplan and add the non-clone's patplan
					//before we can drop the plan, we have to make sure there is not a claim with today's date
					bool isAttachedToClaim=false;
					for(int j=0;j<claimList.Count;j++) {//claimList will only contain claims with DateService=Today
						if(claimList[j].PlanNum!=insSubCloneCur.PlanNum) {//different insplan
							continue;
						}
						strDataUpdated+=Lan.g(this,"The clone's currently attached insurance does not match.  Due to a claim with today's date we cannot synch the plans, the issue must be corrected manually on the following plan")+": "+insPlanCloneDescriptCur+".\r\n";
						isAttachedToClaim=true;
						break;
					}
					if(isAttachedToClaim) {//if we cannot change this plan to match the non-clone's plan at the same ordinal, we will synch the rest of the plans and let the user know to fix manually
						continue;
					}
					strDataUpdated+=Lan.g(this,"The following plan was updated to match the original patient's plan")+": "+insPlanCloneDescriptCur+".\r\n";
					patPlansChanged=true;
					PatPlans.DeleteNonContiguous(listPatPlansClone[i].PatPlanNum);//we use the NonContiguous version because we are going to insert into this same ordinal, compute estimates will happen at the end of all the changes
					PatPlan patPlanCopy=listPatPlansNonClone[i].Copy();
					patPlanCopy.PatNum=patClone.PatNum;
					PatPlans.Insert(patPlanCopy);
				}
				else {
					//both clone and non-clone have the same patplan.InsSubNum at this position in their list, just make sure all data in the patplans match
					if(listPatPlansNonClone[i].Ordinal!=listPatPlansClone[i].Ordinal) {
						strDataUpdated+=Lan.g(this,"The ordinal of the clone patient's insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lan.g(this,"was updated to")+" "+listPatPlansNonClone[i].Ordinal.ToString()+".\r\n";
						patPlansChanged=true;
						listPatPlansClone[i].Ordinal=listPatPlansNonClone[i].Ordinal;
					}
					if(listPatPlansNonClone[i].IsPending!=listPatPlansClone[i].IsPending) {
						strDataUpdated+=Lan.g(this,"The pending status of the clone patient's insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lan.g(this,"was updated to")+" "+listPatPlansNonClone[i].IsPending.ToString()+".\r\n";
						patPlansChanged=true;
						listPatPlansClone[i].IsPending=listPatPlansNonClone[i].IsPending;
					}
					if(listPatPlansNonClone[i].Relationship!=listPatPlansClone[i].Relationship) {
						strDataUpdated+=Lan.g(this,"The relationship to the subscriber of the clone patient's insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lan.g(this,"was updated to")+" "+listPatPlansNonClone[i].Relationship.ToString()+".\r\n";
						patPlansChanged=true;
						listPatPlansClone[i].Relationship=listPatPlansNonClone[i].Relationship;
					}
					if(listPatPlansNonClone[i].PatID!=listPatPlansClone[i].PatID) {
						strDataUpdated+=Lan.g(this,"The patient ID of the clone patient's insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lan.g(this,"was updated to")+" "+listPatPlansNonClone[i].PatID+".\r\n";
						patPlansChanged=true;
						listPatPlansClone[i].PatID=listPatPlansNonClone[i].PatID;
					}
					PatPlans.Update(listPatPlansClone[i]);
				}
			}
			if(patPlansChanged) {
				//compute all estimates for clone after making changes to the patplans
				List<ClaimProc> claimProcs=ClaimProcs.Refresh(patClone.PatNum);
				List<Procedure> procs=Procedures.Refresh(patClone.PatNum);
				listPatPlansClone=PatPlans.Refresh(patClone.PatNum);
				SubList=InsSubs.RefreshForFam(FamCur);
				PlanList=InsPlans.RefreshForSubList(SubList);
				BenefitList=Benefits.Refresh(listPatPlansClone,SubList);
				Procedures.ComputeEstimatesForAll(patClone.PatNum,claimProcs,procs,PlanList,listPatPlansClone,BenefitList,patClone.Age,SubList);
				Patients.SetHasIns(patClone.PatNum);
			}
			#endregion Synch Clone Data - PatPlans
			RefreshModuleData(PatCur.PatNum);
			RefreshModuleScreen();
			if(strDataUpdated=="") {
				strDataUpdated=Lan.g(this,"No changes were made, data already in synch.");
			}
			MessageBox.Show(this,strDataUpdated);
		}
		#endregion

		#region gridRecall
		private void FillGridRecall(){
			gridRecall.BeginUpdate();
			//standard width is 354.  Nice to grow it to 525 if space allows.
			int maxWidth=Width-gridRecall.Left;
			if(maxWidth>525){
				maxWidth=525;
			}
			if(maxWidth>354) {
				gridRecall.Width=maxWidth;
			}
			else {
				gridRecall.Width=354;
			}
			gridRecall.Columns.Clear();
			List<DisplayField> listRecallFields=DisplayFields.GetForCategory(DisplayFieldCategory.FamilyRecallGrid);
			ODGridColumn col;
			for(int i=0;i<listRecallFields.Count;i++) {
				if(listRecallFields[i].Description=="") {
					col=new ODGridColumn(listRecallFields[i].InternalName,listRecallFields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(listRecallFields[i].Description,listRecallFields[i].ColumnWidth);
				}
				gridRecall.Columns.Add(col);
			}
			gridRecall.Rows.Clear();
			if(PatCur==null){
				gridRecall.EndUpdate();
				return;
			}
			//we just want the recall for the current patient
			List<Recall> recallListPat=new List<Recall>();
			for(int i=0;i<RecallList.Count;i++){
				if(RecallList[i].PatNum==PatCur.PatNum){
					recallListPat.Add(RecallList[i]);
				}
			}
			ODGridRow row;
			ODGridCell cell;
			for(int i=0;i<recallListPat.Count;i++){
				row=new ODGridRow();
				for(int j=0;j<listRecallFields.Count;j++) {
					switch (listRecallFields[j].InternalName) {
						case "Type":
							string cellStr=RecallTypes.GetDescription(recallListPat[i].RecallTypeNum);
							row.Cells.Add(cellStr);
							break;
						case "Due Date":
							if(recallListPat[i].DateDue.Year<1880) {
								row.Cells.Add("");
							}
							else {
								cell=new ODGridCell(recallListPat[i].DateDue.ToShortDateString());
								if(recallListPat[i].DateDue<DateTime.Today) {
									cell.Bold=YN.Yes;
									cell.ColorText=Color.Firebrick;
								}
								row.Cells.Add(cell);
							}
							break;
						case "Sched Date":
							if(recallListPat[i].DateScheduled.Year<1880) {
								row.Cells.Add("");
							}
							else {
								row.Cells.Add(recallListPat[i].DateScheduled.ToShortDateString());
							}
							break;
						case "Notes":
							cellStr="";
							if(recallListPat[i].IsDisabled) {
								cellStr+=Lan.g(this,"Disabled");
								if(recallListPat[i].DatePrevious.Year>1800) {
									cellStr+=Lan.g(this,". Previous: ")+recallListPat[i].DatePrevious.ToShortDateString();
									if(recallListPat[i].RecallInterval!=new Interval(0,0,0,0)) {
										DateTime duedate=recallListPat[i].DatePrevious+recallListPat[i].RecallInterval;
										cellStr+=Lan.g(this,". (Due): ")+duedate.ToShortDateString();
									}
								}
							}
							if(recallListPat[i].DisableUntilDate.Year>1880) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=Lan.g(this,"Disabled until ")+recallListPat[i].DisableUntilDate.ToShortDateString();
							}
							if(recallListPat[i].DisableUntilBalance>0) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=Lan.g(this,"Disabled until balance ")+recallListPat[i].DisableUntilBalance.ToString("c");
							}
							if(recallListPat[i].RecallStatus!=0) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=DefC.GetName(DefCat.RecallUnschedStatus,recallListPat[i].RecallStatus);
							}
							if(recallListPat[i].Note!="") {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=recallListPat[i].Note;
							}
							row.Cells.Add(cellStr);
							break;
						case "Previous Date":
							if(recallListPat[i].DatePrevious.Year>1880) {
								row.Cells.Add(recallListPat[i].DatePrevious.ToShortDateString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Interval":
							row.Cells.Add(recallListPat[i].RecallInterval.ToString());
							break;
					}
				}
				gridRecall.Rows.Add(row);
			}
			gridRecall.EndUpdate();
		}

		private void gridRecall_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//use doubleclick instead
		}

		private void gridRecall_DoubleClick(object sender,EventArgs e) {
			if(PatCur==null){
				return;
			}
			FormRecallsPat FormR=new FormRecallsPat();
			FormR.PatNum=PatCur.PatNum;
			FormR.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}
		#endregion gridRecall

		#region gridSuperFam
		private void FillGridSuperFam() {
			gridSuperFam.BeginUpdate();
			gridSuperFam.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("gridSuperFam","Name"),280);
			gridSuperFam.Columns.Add(col);
			col=new ODGridColumn(Lan.g("gridSuperFam","Stmt"),0);
			gridSuperFam.Columns.Add(col);
			gridSuperFam.Rows.Clear();
			if(PatCur==null) {
				return;
			}
			ODGridRow row;
			SuperFamilyGuarantors.Sort(sortPatientListBySuperFamily);
			string superfam="";
			for(int i=0;i<SuperFamilyGuarantors.Count;i++) {
				row=new ODGridRow();
				superfam=SuperFamilyGuarantors[i].GetNameFL();
				for(int j=0;j<SuperFamilyMembers.Count;j++) {
					if(SuperFamilyMembers[j].Guarantor==SuperFamilyGuarantors[i].Guarantor && SuperFamilyMembers[j].PatNum!=SuperFamilyGuarantors[i].PatNum) {
						superfam+=", "+SuperFamilyMembers[j].GetNameFL();
					}
				}
				row.Cells.Add(superfam);
				row.Tag=SuperFamilyGuarantors[i].PatNum;
				if(i==0) {
					row.Cells[0].Bold=YN.Yes;
					row.Cells[0].ColorText=Color.OrangeRed;
				}
				if(SuperFamilyGuarantors[i].HasSuperBilling) {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				gridSuperFam.Rows.Add(row);
			}
			gridSuperFam.EndUpdate();
			for(int i=0;i<gridSuperFam.Rows.Count;i++) {
				if((long)gridSuperFam.Rows[i].Tag==PatCur.Guarantor) {
					gridSuperFam.SetSelected(i,true);
					break;
				}
			}
		}

		private int sortPatientListBySuperFamily(Patient pat1,Patient pat2) {
			if(pat1.PatNum==pat1.SuperFamily) {//Superheads always go to the top no matter what.
						return -1;
			}
			if(pat2.PatNum==pat2.SuperFamily) {
						return 1;
			}
			switch(_superFamSortStrat) {
				case SortStrategy.NameAsc:
					return pat1.GetNameFL().CompareTo(pat2.GetNameFL());
				case SortStrategy.NameDesc:
					return pat2.GetNameFL().CompareTo(pat1.GetNameFL());
				case SortStrategy.PatNumAsc:
					return pat1.PatNum.CompareTo(pat2.PatNum);
				case SortStrategy.PatNumDesc:
					return pat2.PatNum.CompareTo(pat1.PatNum);
				default:
					return pat1.PatNum.CompareTo(pat2.PatNum);//Default behavior
			}
		}

		private void gridSuperFam_CellClick(object sender,ODGridClickEventArgs e) {
			OnPatientSelected(SuperFamilyGuarantors[e.Row]);
			ModuleSelected(SuperFamilyGuarantors[e.Row].PatNum);
		}

		private void gridSuperFam_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//OnPatientSelected(SuperFamilyGuarantors[e.Row].PatNum,SuperFamilyGuarantors[e.Row].GetNameLF(),SuperFamilyGuarantors[e.Row].Email!="",
			//  SuperFamilyGuarantors[e.Row].ChartNumber);
			//ModuleSelected(SuperFamilyGuarantors[e.Row].PatNum);
		}

		private void ToolButAddSuper_Click() {
			if(PatCur.SuperFamily==0) {
				Patients.AssignToSuperfamily(PatCur.Guarantor,PatCur.Guarantor);
			}
			else {//we must want to add some other family to this superfamily
				FormPatientSelect formPS = new FormPatientSelect();
				formPS.SelectionModeOnly=true;
				formPS.ShowDialog();
				if(formPS.DialogResult!=DialogResult.OK) {
					return;
				}
				Patient patSelected=Patients.GetPat(formPS.SelectedPatNum);
				if(patSelected.SuperFamily==PatCur.SuperFamily) {
					MsgBox.Show(this,"That patient is already part of this superfamily.");
					return;
				}
				Patients.AssignToSuperfamily(patSelected.Guarantor,PatCur.SuperFamily);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButRemoveSuper_Click() {
			if(PatCur.SuperFamily==PatCur.Guarantor) {
				MsgBox.Show(this,"You cannot delete the head of a super family.");
				return;
			}
			if(PatCur.SuperFamily==0) {
				return;
			}
			for(int i=0;i<FamCur.ListPats.Length;i++) {//remove whole family
				Patient tempPat=FamCur.ListPats[i].Copy();
				Popups.CopyForMovingSuperFamily(tempPat,0);
				tempPat.SuperFamily=0;
				Patients.Update(tempPat,FamCur.ListPats[i]);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButDisbandSuper_Click() {
			if(PatCur.SuperFamily==0) {
				return;
			}
			Patient superHead = Patients.GetPat(PatCur.SuperFamily);
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Would you like to disband and remove all members in the super family of "+superHead.GetNameFL()+"?")) {
				return;
			}
			Popups.RemoveForDisbandingSuperFamily(PatCur);
			Patients.DisbandSuperFamily(superHead.PatNum);
			ModuleSelected(PatCur.PatNum);
		}

		#endregion gridSuperFam

		#region gridIns
		private void menuPlansForFam_Click(object sender,EventArgs e) {
			FormPlansForFamily FormP=new FormPlansForFamily();
			FormP.FamCur=FamCur;
			FormP.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButIns_Click(){
			DialogResult result=MessageBox.Show(Lan.g(this,"Is this patient the subscriber?"),"",MessageBoxButtons.YesNoCancel);
			if(result==DialogResult.Cancel){
				return;
			}
			//Pick a subscriber------------------------------------------------------------------------------------------------
			Patient subscriber;
			if(result==DialogResult.Yes){//current patient is subscriber
				subscriber=PatCur.Copy();
			}
			else{//patient is not subscriber
				//show list of patients in this family
				FormSubscriberSelect FormS=new FormSubscriberSelect(FamCur);
				FormS.ShowDialog();
				if(FormS.DialogResult==DialogResult.Cancel){
					return;
				}
				subscriber=Patients.GetPat(FormS.SelectedPatNum);
			}
			//Subscriber has been chosen. Now, pick a plan-------------------------------------------------------------------
			InsPlan plan=null;
			InsSub sub=null;
			bool planIsNew=false;
			List<InsSub> subList=InsSubs.GetListForSubscriber(subscriber.PatNum);
			if(subList.Count==0){
				planIsNew=true;
			}
			else{
				FormInsSelectSubscr FormISS=new FormInsSelectSubscr(subscriber.PatNum,PatCur.PatNum);
				FormISS.ShowDialog();
				if(FormISS.DialogResult==DialogResult.Cancel) {
					return;
				}
				if(FormISS.SelectedInsSubNum==0){//'New' option selected.
					planIsNew=true;
				}
				else{
					sub=InsSubs.GetSub(FormISS.SelectedInsSubNum,subList);
					plan=InsPlans.GetPlan(sub.PlanNum,new List<InsPlan>());
				}
			}
			//New plan was selected instead of an existing plan.  Create the plan--------------------------------------------
			if(planIsNew){
				plan=new InsPlan();
				plan.EmployerNum=subscriber.EmployerNum;
				plan.PlanType="";
				InsPlans.Insert(plan);
				sub=new InsSub();
				sub.PlanNum=plan.PlanNum;
				sub.Subscriber=subscriber.PatNum;
				if(subscriber.MedicaidID==""){
					sub.SubscriberID=subscriber.SSN;
				}
				else{
					sub.SubscriberID=subscriber.MedicaidID;
				}
				sub.ReleaseInfo=true;
				sub.AssignBen=true;
				InsSubs.Insert(sub);
				Benefit ben;
				for(int i=0;i<CovCatC.ListShort.Count;i++){
					if(CovCatC.ListShort[i].DefaultPercent==-1){
						continue;
					}
					ben=new Benefit();
					ben.BenefitType=InsBenefitType.CoInsurance;
					ben.CovCatNum=CovCatC.ListShort[i].CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.Percent=CovCatC.ListShort[i].DefaultPercent;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.CodeNum=0;
					Benefits.Insert(ben);
				}
				//Zero deductible diagnostic
				if(CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)!=null) {
					ben=new Benefit();
					ben.CodeNum=0;
					ben.BenefitType=InsBenefitType.Deductible;
					ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.MonetaryAmt=0;
					ben.Percent=-1;
					ben.CoverageLevel=BenefitCoverageLevel.Individual;
					Benefits.Insert(ben);
				}
				//Zero deductible preventive
				if(CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null) {
					ben=new Benefit();
					ben.CodeNum=0;
					ben.BenefitType=InsBenefitType.Deductible;
					ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.MonetaryAmt=0;
					ben.Percent=-1;
					ben.CoverageLevel=BenefitCoverageLevel.Individual;
					Benefits.Insert(ben);
				}
			}
			//Then attach plan------------------------------------------------------------------------------------------------
			PatPlan patplan=new PatPlan();
			patplan.Ordinal=(byte)(PatPlanList.Count+1);//so the ordinal of the first entry will be 1, NOT 0.
			patplan.PatNum=PatCur.PatNum;
			patplan.InsSubNum=sub.InsSubNum;
			patplan.Relationship=Relat.Self;
			PatPlans.Insert(patplan);
			//Then, display insPlanEdit to user-------------------------------------------------------------------------------
			FormInsPlan FormI=new FormInsPlan(plan,patplan,sub);
			FormI.IsNewPlan=planIsNew;
			FormI.IsNewPatPlan=true;
			FormI.ShowDialog();//this updates estimates also.
			//if cancel, then patplan is deleted from within that dialog.
			//if cancel, and planIsNew, then plan and benefits are also deleted.
			ModuleSelected(PatCur.PatNum);
		}

		private void FillInsData(){
			if(PatPlanList.Count==0){
				gridIns.BeginUpdate();
				gridIns.Columns.Clear();
				gridIns.Rows.Clear();
				gridIns.EndUpdate();
				return;
			}
			List<InsSub> subArray=new List<InsSub>();//prevents repeated calls to db.
			List<InsPlan> planArray=new List<InsPlan>();
			InsSub sub;
			for(int i=0;i<PatPlanList.Count;i++){
				sub=InsSubs.GetSub(PatPlanList[i].InsSubNum,SubList);
				subArray.Add(sub);
				planArray.Add(InsPlans.GetPlan(sub.PlanNum,PlanList));
			}
			gridIns.BeginUpdate();
			gridIns.Columns.Clear();
			gridIns.Rows.Clear();
			OpenDental.UI.ODGridColumn col;
			col=new ODGridColumn("",150);
			gridIns.Columns.Add(col);
			int dentalOrdinal=1;
			for(int i=0;i<PatPlanList.Count;i++) {
				if(planArray[i].IsMedical) {
					col=new ODGridColumn(Lan.g("TableCoverage","Medical"),170);
					gridIns.Columns.Add(col);
				}
				else { //dental
					if(dentalOrdinal==1) {
						col=new ODGridColumn(Lan.g("TableCoverage","Primary"),170);
						gridIns.Columns.Add(col);
					}
					else if(dentalOrdinal==2) {
						col=new ODGridColumn(Lan.g("TableCoverage","Secondary"),170);
						gridIns.Columns.Add(col);
					}
					else {
						col=new ODGridColumn(Lan.g("TableCoverage","Other"),170);
						gridIns.Columns.Add(col);
					}
					dentalOrdinal++;
				}
			}
			OpenDental.UI.ODGridRow row=new ODGridRow();
			//subscriber
			row.Cells.Add(Lan.g("TableCoverage","Subscriber"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(FamCur.GetNameInFamFL(subArray[i].Subscriber));
			}
			row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][0].ItemColor;
			gridIns.Rows.Add(row);
			//subscriber ID
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Subscriber ID"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(subArray[i].SubscriberID);
			}
			row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][0].ItemColor;
			gridIns.Rows.Add(row);
			//relationship
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Rel'ship to Sub"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(Lan.g("enumRelat",PatPlanList[i].Relationship.ToString()));
			}
			row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][0].ItemColor;
			gridIns.Rows.Add(row);
			//patient ID
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Patient ID"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(PatPlanList[i].PatID);
			}
			row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][0].ItemColor;
			gridIns.Rows.Add(row);
			//pending
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Pending"));
			for(int i=0;i<PatPlanList.Count;i++){
				if(PatPlanList[i].IsPending){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
			}
			row.ColorBackG=DefC.Long[(int)DefCat.MiscColors][0].ItemColor;
			row.ColorLborder=Color.Black;
			gridIns.Rows.Add(row);
			//employer
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Employer"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(Employers.GetName(planArray[i].EmployerNum));
			}
			gridIns.Rows.Add(row);
			//carrier
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Carrier"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(InsPlans.GetCarrierName(planArray[i].PlanNum,planArray));
			}
			gridIns.Rows.Add(row);
			//group name
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Group Name"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(planArray[i].GroupName);
			}
			gridIns.Rows.Add(row);
			//group number
			row=new ODGridRow();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				row.Cells.Add(Lan.g("TableCoverage","Plan Number"));
			}
			else {
				row.Cells.Add(Lan.g("TableCoverage","Group Number"));
			}
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(planArray[i].GroupNum);
			}
			gridIns.Rows.Add(row);
			//plan type
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Type"));
			for(int i=0;i<planArray.Count;i++) {
				switch(planArray[i].PlanType){
					default://malfunction
						row.Cells.Add("");
						break;
					case "":
						row.Cells.Add(Lan.g(this,"Category Percentage"));
						break;
					case "p":
						row.Cells.Add(Lan.g(this,"PPO Percentage"));
						break;
					case "f":
						row.Cells.Add(Lan.g(this,"Medicaid or Flat Co-pay"));
						break;
					case "c":
						row.Cells.Add(Lan.g(this,"Capitation"));
						break;
				}
			}
			gridIns.Rows.Add(row);
			//fee schedule
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Fee Schedule"));
			for(int i=0;i<planArray.Count;i++) {
				row.Cells.Add(FeeScheds.GetDescription(planArray[i].FeeSched));
			}
			row.ColorLborder=Color.Black;
			gridIns.Rows.Add(row);
			//Calendar vs service year------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Benefit Period"));
			for(int i=0;i<planArray.Count;i++) {
				if(planArray[i].MonthRenew==0) {
					row.Cells.Add(Lan.g("TableCoverage","Calendar Year"));
				}
				else {
					DateTime dateservice=new DateTime(2000,planArray[i].MonthRenew,1);
					row.Cells.Add(Lan.g("TableCoverage","Service year begins:")+" "+dateservice.ToString("MMMM"));
				}
			}
			gridIns.Rows.Add(row);
			//Benefits-----------------------------------------------------------------------------------------------------
			List <Benefit> bensForPat=Benefits.Refresh(PatPlanList,SubList);
			Benefit[,] benMatrix=Benefits.GetDisplayMatrix(bensForPat,PatPlanList,SubList);
			string desc;
			string val;
			ProcedureCode proccode=null;
			for(int y=0;y<benMatrix.GetLength(1);y++){//rows
				row=new ODGridRow();
				desc="";
				//some of the columns might be null, but at least one will not be.  Find it.
				for(int x=0;x<benMatrix.GetLength(0);x++){//columns
					if(benMatrix[x,y]==null){
						continue;
					}
					//create a description for the benefit
					if(benMatrix[x,y].PatPlanNum!=0) {
						desc+=Lan.g(this,"(pat)")+" ";
					}
					if(benMatrix[x,y].CoverageLevel==BenefitCoverageLevel.Family) {
						desc+=Lan.g(this,"Fam")+" ";
					}
					if(benMatrix[x,y].CodeNum!=0) {
						proccode=ProcedureCodes.GetProcCode(benMatrix[x,y].CodeNum);
					}
					if(benMatrix[x,y].BenefitType==InsBenefitType.CoInsurance && benMatrix[x,y].Percent != -1) {
						if(benMatrix[x,y].CodeNum==0) {
							desc+=CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" % ";
						}
						else {
							desc+=proccode.ProcCode+"-"+proccode.AbbrDesc+" % ";
						}
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Deductible) {
						desc+=Lan.g(this,"Deductible")+" "+CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& (benMatrix[x,y].TimePeriod==BenefitTimePeriod.ServiceYear
						|| benMatrix[x,y].TimePeriod==BenefitTimePeriod.CalendarYear))
					{//annual max
						desc+=Lan.g(this,"Annual Max")+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& CovCats.GetForEbenCat(EbenefitCategory.Orthodontics)!=null
						&& benMatrix[x,y].CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& benMatrix[x,y].TimePeriod==BenefitTimePeriod.Lifetime)
					{
						desc+=Lan.g(this,"Ortho Max")+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null
						&& benMatrix[x,y].CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
						&& benMatrix[x,y].Quantity !=0)
					{
						desc+="Exam frequency ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].CodeNum==ProcedureCodes.GetCodeNum("D0274")//4BW
						&& benMatrix[x,y].Quantity !=0)
					{
						desc+="BW frequency ";
					}
					//Canada BW
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].CodeNum==ProcedureCodes.GetCodeNum("02144")//4BW in Quebec and also the rest of Canada.
						&& benMatrix[x,y].Quantity !=0
						&& CultureInfo.CurrentCulture.Name.EndsWith("CA"))
					{
						desc+="BW frequency ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].CodeNum==ProcedureCodes.GetCodeNum("D0330")//Pano
						&& benMatrix[x,y].Quantity !=0)
					{
						desc+="Pano/FMX frequency ";
					}
					//Canada Pano
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& ((Canadian.IsQuebec() && benMatrix[x,y].CodeNum==ProcedureCodes.GetCodeNum("02600"))//Different code for Quebec,
							|| (!Canadian.IsQuebec() && benMatrix[x,y].CodeNum==ProcedureCodes.GetCodeNum("02601")))//than for the rest of Canada.
						&& benMatrix[x,y].Quantity !=0
						&& CultureInfo.CurrentCulture.Name.EndsWith("CA"))
					{
						desc+="Pano/FMX frequency ";
					}
					else if(benMatrix[x,y].CodeNum==0){//e.g. flo
						desc+=ProcedureCodes.GetProcCode(benMatrix[x,y].CodeNum).AbbrDesc+" ";
					}
					else{
						desc+=Lan.g("enumInsBenefitType",benMatrix[x,y].BenefitType.ToString())+" ";
					}
					row.Cells.Add(desc);
					break;
				}
				//remember that matrix does not include the description column
				for(int x=0;x<benMatrix.GetLength(0);x++){//columns
					val="";
					//this matrix cell might be null
					if(benMatrix[x,y]==null){
						row.Cells.Add("");
						continue;
					}
					if(benMatrix[x,y].Percent != -1) {
						val+=benMatrix[x,y].Percent.ToString()+"% ";
					}
					if(benMatrix[x,y].MonetaryAmt != -1) {
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}
					/*
					if(benMatrix[x,y].BenefitType==InsBenefitType.CoInsurance) {
						val+=benMatrix[x,y].Percent.ToString()+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Deductible
						&& benMatrix[x,y].MonetaryAmt==0)
					{//deductible 0
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& (benMatrix[x,y].TimePeriod==BenefitTimePeriod.ServiceYear
						|| benMatrix[x,y].TimePeriod==BenefitTimePeriod.CalendarYear)
						&& benMatrix[x,y].MonetaryAmt==0)
					{//annual max 0
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}*/
					if(benMatrix[x,y].BenefitType==InsBenefitType.Exclusions
						|| benMatrix[x,y].BenefitType==InsBenefitType.Limitations) 
					{
						if(benMatrix[x,y].CodeNum != 0) {
							proccode=ProcedureCodes.GetProcCode(benMatrix[x,y].CodeNum);
							val+=proccode.ProcCode+"-"+proccode.AbbrDesc+" ";
						}
						else if(benMatrix[x,y].CovCatNum != 0){
							val+=CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" ";
						}
					}
					if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.NumberOfServices){//eg 2 times per CalendarYear
						val+=benMatrix[x,y].Quantity.ToString()+" "+Lan.g(this,"times per")+" "
							+Lan.g("enumBenefitQuantity",benMatrix[x,y].TimePeriod.ToString())+" ";
					}
					else if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.Months) {//eg Every 2 months
						val+=Lan.g(this,"Every ")+benMatrix[x,y].Quantity.ToString()+" month";
						if(benMatrix[x,y].Quantity>1){
							val+="s";
						}
					}
					else if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.Years) {//eg Every 2 years
						val+="Every "+benMatrix[x,y].Quantity.ToString()+" year";
						if(benMatrix[x,y].Quantity>1) {
							val+="s";
						}
					}
					else{
						if(benMatrix[x,y].QuantityQualifier!=BenefitQuantity.None){//e.g. flo
							val+=Lan.g("enumBenefitQuantity",benMatrix[x,y].QuantityQualifier.ToString())+" ";
						}
						if(benMatrix[x,y].Quantity!=0){
							val+=benMatrix[x,y].Quantity.ToString()+" ";
						}
					}
					//if(benMatrix[x,y].MonetaryAmt!=0){
					//	val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					//}
					//if(val==""){
					//	val="val";
					//}
					row.Cells.Add(val);
				}
				gridIns.Rows.Add(row);
			}
			//Plan note
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Ins Plan Note"));
			OpenDental.UI.ODGridCell cell;
			for(int i=0;i<PatPlanList.Count;i++){
				cell=new ODGridCell();
				cell.Text=planArray[i].PlanNote;
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
			}
			gridIns.Rows.Add(row);
			//Subscriber Note
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Subscriber Note"));
			for(int i=0;i<PatPlanList.Count;i++) {
				cell=new ODGridCell();
				cell.Text=subArray[i].SubscNote;
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
			}
			gridIns.Rows.Add(row);
			gridIns.EndUpdate();
		}

		private void gridIns_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(e.Col==0){
				return;
			}
			Cursor=Cursors.WaitCursor;
			PatPlan patPlan=PatPlanList[e.Col-1];
			InsSub insSub=InsSubs.GetSub(patPlan.InsSubNum,SubList);
			PlanList=InsPlans.RefreshForSubList(SubList);//this is only here in case, if in FormModuleSetup, the InsDefaultCobRule is changed and cob changed for all plans.
			InsPlan insPlan=InsPlans.GetPlan(insSub.PlanNum,PlanList);
			FormInsPlan FormIP=new FormInsPlan(insPlan,patPlan,insSub);
			FormIP.ShowDialog();
			Cursor=Cursors.Default;
			ModuleSelected(PatCur.PatNum);
		}

		#endregion gridIns





























	}
}
