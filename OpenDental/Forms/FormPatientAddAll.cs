using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using System.Linq;

namespace OpenDental {
	public partial class FormPatientAddAll:Form {
		public string LName;
		public string FName;
		public DateTime Birthdate;
		public long SelectedPatNum;
		private string _lnameOld1;
		private string _fnameOld2="";
		private string _fnameOld3="";
		private string _fnameOld4="";
		private string _fnameOld5="";
		/// <summary>displayed from within code, not designer</summary>
		private System.Windows.Forms.ListBox listEmps1;
		private bool mouseIsInListEmps1;
		private string empOriginal1;
		/// <summary>displayed from within code, not designer</summary>
		private System.Windows.Forms.ListBox listEmps2;
		private bool mouseIsInListEmps2;
		private string empOriginal2;
		private List<Carrier> similarCars1;
		private string carOriginal1;
		private System.Windows.Forms.ListBox listCars1;
		private bool mouseIsInListCars1;
		private Carrier selectedCarrier1;
		private List<Carrier> similarCars2;
		private string carOriginal2;
		private System.Windows.Forms.ListBox listCars2;
		private bool mouseIsInListCars2;
		private Carrier selectedCarrier2;
		///<summary>If user picks a plan from list, but then changes one of the critical fields, this will be ignored.  Keep in mind that the plan here is just a copy.  It can't be updated, but must instead be inserted.</summary>
		private InsPlan selectedPlan1;
		private InsPlan selectedPlan2;
		private ToolTip _referredFromToolTip;
		private Referral _refCur;
		private System.Windows.Forms.ListBox listStates;//displayed from within code, not designer
		private string _stateOriginal;//used in the medicaidState dropdown logic
		private bool _mouseIsInListStates;

		public FormPatientAddAll() {
			InitializeComponent();
			Lan.F(this);
			_referredFromToolTip=new ToolTip();
			_referredFromToolTip.InitialDelay=500;
			_referredFromToolTip.ReshowDelay=100;
			listEmps1=new ListBox();
			listEmps1.Location=new Point(groupIns1.Left+textEmployer1.Left,
				groupIns1.Top+textEmployer1.Bottom);
			listEmps1.Size=new Size(254,100);
			listEmps1.Visible=false;
			listEmps1.Click += new System.EventHandler(listEmps1_Click);
			listEmps1.DoubleClick += new System.EventHandler(listEmps1_DoubleClick);
			listEmps1.MouseEnter += new System.EventHandler(listEmps1_MouseEnter);
			listEmps1.MouseLeave += new System.EventHandler(listEmps1_MouseLeave);
			Controls.Add(listEmps1);
			listEmps1.BringToFront();
			listEmps2=new ListBox();
			listEmps2.Location=new Point(groupIns2.Left+textEmployer2.Left,
				groupIns2.Top+textEmployer2.Bottom);
			listEmps2.Size=new Size(254,100);
			listEmps2.Visible=false;
			listEmps2.Click += new System.EventHandler(listEmps2_Click);
			listEmps2.DoubleClick += new System.EventHandler(listEmps2_DoubleClick);
			listEmps2.MouseEnter += new System.EventHandler(listEmps2_MouseEnter);
			listEmps2.MouseLeave += new System.EventHandler(listEmps2_MouseLeave);
			Controls.Add(listEmps2);
			listEmps2.BringToFront();
			listCars1=new ListBox();
			listCars1.Location=new Point(groupIns1.Left+textCarrier1.Left,
				groupIns1.Top+textCarrier1.Bottom);
			listCars1.Size=new Size(700,100);
			listCars1.HorizontalScrollbar=true;
			listCars1.Visible=false;
			listCars1.Click += new System.EventHandler(listCars1_Click);
			listCars1.DoubleClick += new System.EventHandler(listCars1_DoubleClick);
			listCars1.MouseEnter += new System.EventHandler(listCars1_MouseEnter);
			listCars1.MouseLeave += new System.EventHandler(listCars1_MouseLeave);
			Controls.Add(listCars1);
			listCars1.BringToFront();
			listCars2=new ListBox();
			listCars2.Location=new Point(groupIns2.Left+textCarrier2.Left,
				groupIns2.Top+textCarrier2.Bottom);
			listCars2.Size=new Size(700,100);
			listCars2.HorizontalScrollbar=true;
			listCars2.Visible=false;
			listCars2.Click += new System.EventHandler(listCars2_Click);
			listCars2.DoubleClick += new System.EventHandler(listCars2_DoubleClick);
			listCars2.MouseEnter += new System.EventHandler(listCars2_MouseEnter);
			listCars2.MouseLeave += new System.EventHandler(listCars2_MouseLeave);
			Controls.Add(listCars2);
			listCars2.BringToFront();
			listStates=new ListBox();
			listStates.Location=new Point(textState.Left+groupBox1.Left,textState.Bottom+groupBox1.Top);
			listStates.Size=new Size(textState.Width,100);
			listStates.Visible=false;
			listStates.Click += new System.EventHandler(listStates_Click);
			listStates.MouseEnter += new System.EventHandler(listStates_MouseEnter);
			listStates.MouseLeave += new System.EventHandler(listStates_MouseLeave);
			Controls.Add(listStates);
			listStates.BringToFront();
		}

		private void FormPatientAddAll_Load(object sender,EventArgs e) {
			textLName1.Text=LName;
			textFName1.Text=FName;
			if(Birthdate.Year<1880) {
				textBirthdate1.Text="";
			}
			else {
				textBirthdate1.Text=Birthdate.ToShortDateString();
			}
			textBirthdate1_Validated(this,null);
			listGender1.SelectedIndex=0;
			listGender2.SelectedIndex=0;
			listGender3.SelectedIndex=0;
			listGender4.SelectedIndex=0;
			listGender5.SelectedIndex=0;
			listPosition1.SelectedIndex=1;
			listPosition2.SelectedIndex=1;
			comboSecProv1.Items.Add(Lan.g(this,"none"));
			comboSecProv1.SelectedIndex=0;
			comboSecProv2.Items.Add(Lan.g(this,"none"));
			comboSecProv2.SelectedIndex=0;
			comboSecProv3.Items.Add(Lan.g(this,"none"));
			comboSecProv3.SelectedIndex=0;
			comboSecProv4.Items.Add(Lan.g(this,"none"));
			comboSecProv4.SelectedIndex=0;
			comboSecProv5.Items.Add(Lan.g(this,"none"));
			comboSecProv5.SelectedIndex=0;
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				comboPriProv1.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboSecProv1.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboPriProv2.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboSecProv2.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboPriProv3.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboSecProv3.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboPriProv4.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboSecProv4.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboPriProv5.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				comboSecProv5.Items.Add(ProviderC.ListShort[i].GetLongDesc());
			}
			int defaultindex=Providers.GetIndex(PrefC.GetLong(PrefName.PracticeDefaultProv));
			if(defaultindex==-1) {//default provider hidden
				defaultindex=0;
			}
			comboPriProv1.SelectedIndex=defaultindex;
			comboPriProv2.SelectedIndex=defaultindex;
			comboPriProv3.SelectedIndex=defaultindex;
			comboPriProv4.SelectedIndex=defaultindex;
			comboPriProv5.SelectedIndex=defaultindex;
			if(!Security.IsAuthorized(Permissions.RefAttachAdd,true)) {
				butClearReferralSource.Enabled=false;
				butReferredFrom.Enabled=false;
			}
			if(!PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				labelST.Text="ST";
				textCountry.Visible=false;
			}
			FillComboZip();
			ResetSubscriberLists();
		}

		private void FormPatientAddAll_Shown(object sender,EventArgs e) {
			
		}

		#region Names
		private void textLName1_TextChanged(object sender,EventArgs e) {
			if(textLName1.Text.Length==1){
				textLName1.Text=textLName1.Text.ToUpper();
				textLName1.SelectionStart=1;
			}
			SetLnames();
			_lnameOld1=textLName1.Text;
		}

		private void textLName2_TextChanged(object sender,EventArgs e) {
			if(textLName2.Text.Length==1){
				textLName2.Text=textLName2.Text.ToUpper();
				textLName2.SelectionStart=1;
			}
		}

		private void textLName3_TextChanged(object sender,EventArgs e) {
			if(textLName3.Text.Length==1){
				textLName3.Text=textLName3.Text.ToUpper();
				textLName3.SelectionStart=1;
			}
		}

		private void textLName4_TextChanged(object sender,EventArgs e) {
			if(textLName4.Text.Length==1){
				textLName4.Text=textLName4.Text.ToUpper();
				textLName4.SelectionStart=1;
			}
		}

		private void textLName5_TextChanged(object sender,EventArgs e) {
			if(textLName5.Text.Length==1){
				textLName5.Text=textLName5.Text.ToUpper();
				textLName5.SelectionStart=1;
			}
		}

		private void textFName1_TextChanged(object sender,EventArgs e) {
			if(textFName1.Text.Length==1){
				textFName1.Text=textFName1.Text.ToUpper();
				textFName1.SelectionStart=1;
			}
			SetLnames();
		}

		private void textFName2_TextChanged(object sender,EventArgs e) {
			if(textFName2.Text.Length==1){
				textFName2.Text=textFName2.Text.ToUpper();
				textFName2.SelectionStart=1;
			}
			SetLnames();
			_fnameOld2=textFName2.Text;
		}

		private void textFName3_TextChanged(object sender,EventArgs e) {
			if(textFName3.Text.Length==1){
				textFName3.Text=textFName3.Text.ToUpper();
				textFName3.SelectionStart=1;
			}
			SetLnames();
			_fnameOld3=textFName3.Text;
		}

		private void textFName4_TextChanged(object sender,EventArgs e) {
			if(textFName4.Text.Length==1){
				textFName4.Text=textFName4.Text.ToUpper();
				textFName4.SelectionStart=1;
			}
			SetLnames();
			_fnameOld4=textFName4.Text;
		}

		private void textFName5_TextChanged(object sender,EventArgs e) {
			if(textFName5.Text.Length==1){
				textFName5.Text=textFName5.Text.ToUpper();
				textFName5.SelectionStart=1;
			}
			SetLnames();
			_fnameOld5=textFName5.Text;
		}

		private void SetLnames() {
			SetLname(textLName2,textFName2,_fnameOld2);
			SetLname(textLName3,textFName3,_fnameOld3);
			SetLname(textLName4,textFName4,_fnameOld4);
			SetLname(textLName5,textFName5,_fnameOld5);
			ResetSubscriberLists();
		}

		private void SetLname(TextBox textLname,TextBox textFname,string fnameOld) {
			if(textLname.Text=="" && textFname.Text=="") {
				textLname.Text="";
			}
			else if(textLname.Text=="" && textFname.Text!="") {
				textLname.Text=textLName1.Text;
			}
			else if(textLname.Text==_lnameOld1 && textFname.Text=="") {
				if(fnameOld!="" && textFname.Text=="") {
					textLname.Text="";
				}
				else {
					textLname.Text=textLName1.Text;
				}
			}
			else if(textLname.Text==_lnameOld1 && textFname.Text!="") {
				textLname.Text=textLName1.Text;
			}
		}
		#endregion Names

		#region BirthdateAndAge
		private void textBirthdate1_Validated(object sender,EventArgs e) {
			if(textBirthdate1.errorProvider1.GetError(textBirthdate1)!=""){
				textAge1.Text="";
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate1.Text);
			if(birthdate>DateTime.Today){
				birthdate=birthdate.AddYears(-100);
			}
			textAge1.Text=PatientLogic.DateToAgeString(birthdate);
		}

		private void textBirthdate2_Validated(object sender,EventArgs e) {
			if(textBirthdate2.errorProvider1.GetError(textBirthdate2)!=""){
				textAge2.Text="";
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate2.Text);
			if(birthdate>DateTime.Today){
				birthdate=birthdate.AddYears(-100);
			}
			textAge2.Text=PatientLogic.DateToAgeString(birthdate);
		}

		private void textBirthdate3_Validated(object sender,EventArgs e) {
			if(textBirthdate3.errorProvider1.GetError(textBirthdate3)!=""){
				textAge3.Text="";
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate3.Text);
			if(birthdate>DateTime.Today){
				birthdate=birthdate.AddYears(-100);
			}
			textAge3.Text=PatientLogic.DateToAgeString(birthdate);
		}

		private void textBirthdate4_Validated(object sender,EventArgs e) {
			if(textBirthdate4.errorProvider1.GetError(textBirthdate4)!=""){
				textAge4.Text="";
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate4.Text);
			if(birthdate>DateTime.Today){
				birthdate=birthdate.AddYears(-100);
			}
			textAge4.Text=PatientLogic.DateToAgeString(birthdate);
		}

		private void textBirthdate5_Validated(object sender,EventArgs e) {
			if(textBirthdate5.errorProvider1.GetError(textBirthdate5)!=""){
				textAge5.Text="";
				return;
			}
			DateTime birthdate=PIn.Date(textBirthdate5.Text);
			if(birthdate>DateTime.Today){
				birthdate=birthdate.AddYears(-100);
			}
			textAge5.Text=PatientLogic.DateToAgeString(birthdate);
		}
		#endregion BirthdateAndAge

		#region InsCheckProvAutomation
		private void checkInsOne1_Click(object sender,EventArgs e) {
			if(textFName2.Text!="" && checkInsOne1.Checked){
				checkInsOne2.Checked=true;
			}
			else{
				checkInsOne2.Checked=false;
			}
			if(textFName3.Text!="" && checkInsOne1.Checked){
				checkInsOne3.Checked=true;
			}
			else{
				checkInsOne3.Checked=false;
			}
			if(textFName4.Text!="" && checkInsOne1.Checked){
				checkInsOne4.Checked=true;
			}
			else{
				checkInsOne4.Checked=false;
			}
			if(textFName5.Text!="" && checkInsOne1.Checked){
				checkInsOne5.Checked=true;
			}
			else{
				checkInsOne5.Checked=false;
			}
		}

		private void checkInsTwo1_Click(object sender,EventArgs e) {
			if(textFName2.Text!="" && checkInsTwo1.Checked){
				checkInsTwo2.Checked=true;
			}
			else{
				checkInsTwo2.Checked=false;
			}
			if(textFName3.Text!="" && checkInsTwo1.Checked){
				checkInsTwo3.Checked=true;
			}
			else{
				checkInsTwo3.Checked=false;
			}
			if(textFName4.Text!="" && checkInsTwo1.Checked){
				checkInsTwo4.Checked=true;
			}
			else{
				checkInsTwo4.Checked=false;
			}
			if(textFName5.Text!="" && checkInsTwo1.Checked){
				checkInsTwo5.Checked=true;
			}
			else{
				checkInsTwo5.Checked=false;
			}
		}

		private void comboPriProv1_SelectionChangeCommitted(object sender,EventArgs e) {
			comboPriProv2.SelectedIndex=comboPriProv1.SelectedIndex;
			comboPriProv3.SelectedIndex=comboPriProv1.SelectedIndex;
			comboPriProv4.SelectedIndex=comboPriProv1.SelectedIndex;
			comboPriProv5.SelectedIndex=comboPriProv1.SelectedIndex;
		}

		private void comboSecProv1_SelectionChangeCommitted(object sender,EventArgs e) {
			comboSecProv2.SelectedIndex=comboSecProv1.SelectedIndex;
			comboSecProv3.SelectedIndex=comboSecProv1.SelectedIndex;
			comboSecProv4.SelectedIndex=comboSecProv1.SelectedIndex;
			comboSecProv5.SelectedIndex=comboSecProv1.SelectedIndex;
		}
		#endregion InsCheckProvAutomation

		#region AddressPhone
		private void textHmPhone_TextChanged(object sender, System.EventArgs e) {
		 	int cursor=textHmPhone.SelectionStart;
			int length=textHmPhone.Text.Length;
			textHmPhone.Text=TelephoneNumbers.AutoFormat(textHmPhone.Text);
			if(textHmPhone.Text.Length>length){
				cursor++;
			}
			textHmPhone.SelectionStart=cursor;		
		}

		private void textAddress_TextChanged(object sender, System.EventArgs e) {
			if(textAddress.Text.Length==1){
				textAddress.Text=textAddress.Text.ToUpper();
				textAddress.SelectionStart=1;
			}
		}

		private void textAddress2_TextChanged(object sender, System.EventArgs e) {
			if(textAddress2.Text.Length==1){
				textAddress2.Text=textAddress2.Text.ToUpper();
				textAddress2.SelectionStart=1;
			}
		}

		private void textCity_TextChanged(object sender, System.EventArgs e) {
			if(textCity.Text.Length==1){
				textCity.Text=textCity.Text.ToUpper();
				textCity.SelectionStart=1;
			}
		}

		private void textState_TextChanged(object sender, System.EventArgs e) {
			if(CultureInfo.CurrentCulture.Name=="en-US" //if USA or Canada, capitalize first 2 letters
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textState.Text.Length==1 || textState.Text.Length==2){
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=2;
				}
			}
			else{
				if(textState.Text.Length==1){
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=1;
				}
			}
		}

		private void textZip_TextChanged(object sender, System.EventArgs e) {
			comboZip.SelectedIndex=-1;
		}

		private void comboZip_SelectionChangeCommitted(object sender, System.EventArgs e) {
			//this happens when a zipcode is selected from the combobox of frequent zips.
			//The combo box is tucked under textZip because Microsoft makes stupid controls.
			textCity.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).City;
			textState.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).State;
			textZip.Text=((ZipCode)ZipCodes.ALFrequent[comboZip.SelectedIndex]).ZipCodeDigits;
		}

		private void textZip_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			//fired as soon as control loses focus.
			//it's here to validate if zip is typed in to text box instead of picked from list.
			//if(textZip.Text=="" && (textCity.Text!="" || textState.Text!="")){
			//	if(MessageBox.Show(Lan.g(this,"Delete the City and State?"),"",MessageBoxButtons.OKCancel)
			//		==DialogResult.OK){
			//		textCity.Text="";
			//		textState.Text="";
			//	}	
			//	return;
			//}
			if(textZip.Text.Length<5){
				return;
			}
			if(comboZip.SelectedIndex!=-1){
				return;
			}
			//the autofill only works if both city and state are left blank
			if(textCity.Text!="" || textState.Text!=""){
				return;
			}
			ZipCodes.GetALMatches(textZip.Text);
			if(ZipCodes.ALMatches.Count==0){
				//No match found. Must enter info for new zipcode
				ZipCode ZipCodeCur=new ZipCode();
				ZipCodeCur.ZipCodeDigits=textZip.Text;
				FormZipCodeEdit FormZE=new FormZipCodeEdit();
				FormZE.ZipCodeCur=ZipCodeCur;
				FormZE.IsNew=true;
				FormZE.ShowDialog();
				if(FormZE.DialogResult!=DialogResult.OK){
					return;
				}
				DataValid.SetInvalid(InvalidType.ZipCodes);//FormZipCodeEdit does not contain internal refresh
				FillComboZip();
				textCity.Text=ZipCodeCur.City;
				textState.Text=ZipCodeCur.State;
				textZip.Text=ZipCodeCur.ZipCodeDigits;
			}
			else if(ZipCodes.ALMatches.Count==1){
				//only one match found.  Use it.
				textCity.Text=((ZipCode)ZipCodes.ALMatches[0]).City;
				textState.Text=((ZipCode)ZipCodes.ALMatches[0]).State;
			}
			else{
				//multiple matches found.  Pick one
				FormZipSelect FormZS=new FormZipSelect();
				FormZS.ShowDialog();
				FillComboZip();
				if(FormZS.DialogResult!=DialogResult.OK){
					return;
				}
				DataValid.SetInvalid(InvalidType.ZipCodes);
				textCity.Text=FormZS.ZipSelected.City;
				textState.Text=FormZS.ZipSelected.State;
				textZip.Text=FormZS.ZipSelected.ZipCodeDigits;
			}
		}

		private void FillComboZip(){
			comboZip.Items.Clear();
			for(int i=0;i<ZipCodes.ALFrequent.Count;i++){
				comboZip.Items.Add(((ZipCode)ZipCodes.ALFrequent[i]).ZipCodeDigits
					+"("+((ZipCode)ZipCodes.ALFrequent[i]).City+")");
			}
		}

		private void textState_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listStates.Visible=false;
				return;
			}
			if(textState.Text=="") {
				listStates.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listStates.Items.Count==0) {
					return;
				}
				if(listStates.SelectedIndex==-1) {
					listStates.SelectedIndex=0;
					textState.Text=listStates.SelectedItem.ToString();
				}
				else if(listStates.SelectedIndex==listStates.Items.Count-1) {
					listStates.SelectedIndex=-1;
					textState.Text=_stateOriginal;
				}
				else {
					listStates.SelectedIndex++;
					textState.Text=listStates.SelectedItem.ToString();
				}
				textState.SelectionStart=textState.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listStates.Items.Count==0) {
					return;
				}
				if(listStates.SelectedIndex==-1) {
					listStates.SelectedIndex=listStates.Items.Count-1;
					textState.Text=listStates.SelectedItem.ToString();
				}
				else if(listStates.SelectedIndex==0) {
					listStates.SelectedIndex=-1;
					textState.Text=_stateOriginal;
				}
				else {
					listStates.SelectedIndex--;
					textState.Text=listStates.SelectedItem.ToString();
				}
				textState.SelectionStart=textState.Text.Length;
				return;
			}
			if(textState.Text.Length==1) {
				textState.Text=textState.Text.ToUpper();
				textState.SelectionStart=1;
			}
			_stateOriginal=textState.Text;//the original text is preserved when using up and down arrows
			listStates.Items.Clear();
			List<StateAbbr> similarAbbrs=StateAbbrs.GetSimilarAbbrs(textState.Text);
			for(int i=0;i<similarAbbrs.Count;i++) {
				listStates.Items.Add(similarAbbrs[i].Abbr);
			}
			int h=13*similarAbbrs.Count+5;
			if(h > ClientSize.Height-listStates.Top) {
				h=ClientSize.Height-listStates.Top;
			}
			listStates.Size=new Size(textState.Width,h);
			listStates.Visible=true;
		}

		private void textState_Leave(object sender,System.EventArgs e) {
			if(_mouseIsInListStates) {
				return;
			}
			listStates.Visible=false;
		}

		private void listStates_Click(object sender,System.EventArgs e) {
			textState.Text=listStates.SelectedItem.ToString();
			textState.Focus();
			textState.SelectionStart=textState.Text.Length;
			listStates.Visible=false;
		}

		private void listStates_MouseEnter(object sender,System.EventArgs e) {
			_mouseIsInListStates=true;
		}

		private void listStates_MouseLeave(object sender,System.EventArgs e) {
			_mouseIsInListStates=false;
		}
		#endregion AddressPhone

		#region Referral
		private void butReferredFrom_Click(object sender,EventArgs e) {
			Referral refCur=new Referral();
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Is the referral source an existing patient?")) {//patient referral
				FormPatientSelect FormPS=new FormPatientSelect();
				FormPS.SelectionModeOnly=true;
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK) {
					return;
				}
				refCur.PatNum=FormPS.SelectedPatNum;
				bool referralIsNew=true;
				if(Referrals.List.Any(x => x.PatNum==FormPS.SelectedPatNum)) {
					refCur=Referrals.List.FirstOrDefault(x => x.PatNum==FormPS.SelectedPatNum).Copy();
					referralIsNew=false;
				}
				FormReferralEdit FormRefEdit=new FormReferralEdit(refCur);//the ReferralNum must be added here
				FormRefEdit.IsNew=referralIsNew;
				FormRefEdit.ShowDialog();
				if(FormRefEdit.DialogResult!=DialogResult.OK) {
					return;
				}
				refCur=FormRefEdit.RefCur;//not needed, but it makes it clear that we are editing the ref in FormRefEdit
			}
			else {//not a patient referral, must be a doctor or marketing/other so show the referral select window with doctor and other check boxes checked
				FormReferralSelect FormRS=new FormReferralSelect();
				FormRS.IsSelectionMode=true;
				FormRS.IsShowPat=false;
				FormRS.ShowDialog();
				if(FormRS.DialogResult!=DialogResult.OK) {
					if(_refCur!=null && _refCur.ReferralNum>0) {
						_refCur=Referrals.GetReferral(_refCur.ReferralNum);
						FillReferredFrom();//the user may have edited a referral and then cancelled attaching to the patient, refill the text box to reflect any changes
					}
					return;
				}
				refCur=FormRS.SelectedReferral;
			}
			_refCur=refCur;
			FillReferredFrom();
		}

		private void butClearReferralSource_Click(object sender,EventArgs e) {
			_refCur=null;
			textReferredFrom.Clear();
		}

		///<summary>Fills the Referred From text box with the name and referral type from the private classwide variable _refCur.</summary>
		private void FillReferredFrom() {
			string firstRefNameTypeAbbr="";
			string firstRefType="";
			string firstRefFullName="";
			if(_refCur==null) {
				return;
			}
			firstRefFullName=Referrals.GetNameLF(_refCur.ReferralNum);
			if(_refCur.PatNum>0) {
				firstRefType=" (patient)";
			}
			else if(_refCur.IsDoctor) {
				firstRefType=" (doctor)";
			}
			firstRefNameTypeAbbr=firstRefFullName;
			for(int i=1;i<firstRefFullName.Length+1;i++) {//i is used as the length to substring, not an index, so i<firstRefName.Length+1 is safe
				if(TextRenderer.MeasureText(firstRefFullName.Substring(0,i)+firstRefType,textReferredFrom.Font).Width<textReferredFrom.Width) {
					continue;
				}
				firstRefNameTypeAbbr=firstRefFullName.Substring(0,i-1);
				break;
			}
			firstRefNameTypeAbbr+=firstRefType;//firstRefType could be blank, but it will show regardless of the length of firstRefName
			//Example: Schmidt, John Jacob Jingleheimer, DDS (doctor) (+5 more) 
			//might be shortened to : Schmidt, John Jaco (doctor) (+5 more) 
			textReferredFrom.Text=firstRefNameTypeAbbr;//text box might be something like: Schmidt, John Jaco (doctor) (+5 more)
			_referredFromToolTip.SetToolTip(textReferredFrom,firstRefFullName+firstRefType);//tooltip will be: Schmidt, John Jacob Jingleheimer, DDS (doctor)
		}
		#endregion Referral

		#region Subscriber
		///<summary>Resets the text for each of the six options in the dropdown.  Does this without changing the selected index.</summary>
		private void ResetSubscriberLists(){
			int selectedIndex1=comboSubscriber1.SelectedIndex;
			int selectedIndex2=comboSubscriber2.SelectedIndex;
			comboSubscriber1.Items.Clear();
			comboSubscriber2.Items.Clear();
			comboSubscriber1.Items.Add(Lan.g(this,"none"));
			comboSubscriber2.Items.Add(Lan.g(this,"none"));
			string str;
			for(int i=0;i<5;i++){
				str=(i+1).ToString()+" - ";
				switch(i){
					case 0:
						str+=textLName1.Text+", "+textFName1.Text;
						break;
					case 1:
						str+=textLName2.Text+", "+textFName2.Text;
						break;
					case 2:
						str+=textLName3.Text+", "+textFName3.Text;
						break;
					case 3:
						str+=textLName4.Text+", "+textFName4.Text;
						break;
					case 4:
						str+=textLName5.Text+", "+textFName5.Text;
						break;
				}
				comboSubscriber1.Items.Add(str);
				comboSubscriber2.Items.Add(str);
			}
			if(selectedIndex1==-1){
				comboSubscriber1.SelectedIndex=0;
			}
			else{
				comboSubscriber1.SelectedIndex=selectedIndex1;
			}
			if(selectedIndex2==-1){
				comboSubscriber2.SelectedIndex=0;
			}
			else{
				comboSubscriber2.SelectedIndex=selectedIndex2;
			}
		}
		#endregion Subscriber

		#region Employer
		private void textEmployer1_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listEmps1.Visible=false;
				textCarrier1.Focus();
				return;
			}
			if(textEmployer1.Text=="") {
				listEmps1.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listEmps1.Items.Count==0) {
					return;
				}
				if(listEmps1.SelectedIndex==-1) {
					listEmps1.SelectedIndex=0;
					textEmployer1.Text=listEmps1.SelectedItem.ToString();
				}
				else if(listEmps1.SelectedIndex==listEmps1.Items.Count-1) {
					listEmps1.SelectedIndex=-1;
					textEmployer1.Text=empOriginal1;
				}
				else {
					listEmps1.SelectedIndex++;
					textEmployer1.Text=listEmps1.SelectedItem.ToString();
				}
				textEmployer1.SelectionStart=textEmployer1.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listEmps1.Items.Count==0) {
					return;
				}
				if(listEmps1.SelectedIndex==-1) {
					listEmps1.SelectedIndex=listEmps1.Items.Count-1;
					textEmployer1.Text=listEmps1.SelectedItem.ToString();
				}
				else if(listEmps1.SelectedIndex==0) {
					listEmps1.SelectedIndex=-1;
					textEmployer1.Text=empOriginal1;
				}
				else {
					listEmps1.SelectedIndex--;
					textEmployer1.Text=listEmps1.SelectedItem.ToString();
				}
				textEmployer1.SelectionStart=textEmployer1.Text.Length;
				return;
			}
			if(textEmployer1.Text.Length==1) {
				textEmployer1.Text=textEmployer1.Text.ToUpper();
				textEmployer1.SelectionStart=1;
			}
			empOriginal1=textEmployer1.Text;//the original text is preserved when using up and down arrows
			listEmps1.Items.Clear();
			List<Employer> similarEmps=Employers.GetSimilarNames(textEmployer1.Text);
			for(int i=0;i<similarEmps.Count;i++) {
				listEmps1.Items.Add(similarEmps[i].EmpName);
			}
			int h=13*similarEmps.Count+5;
			if(h > ClientSize.Height-listEmps1.Top){
				h=ClientSize.Height-listEmps1.Top;
			}
			listEmps1.Size=new Size(231,h);
			listEmps1.Visible=true;
		}

		private void textEmployer1_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListEmps1) {
				return;
			}
			listEmps1.Visible=false;
		}

		private void listEmps1_Click(object sender,System.EventArgs e) {
			textEmployer1.Text=listEmps1.SelectedItem.ToString();
			textEmployer1.Focus();
			textEmployer1.SelectionStart=textEmployer1.Text.Length;
			listEmps1.Visible=false;
		}

		private void listEmps1_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
			textEmployer1.Text=listEmps1.SelectedItem.ToString();
			textEmployer1.Focus();
			textEmployer1.SelectionStart=textEmployer1.Text.Length;
			listEmps1.Visible=false;
		}

		private void listEmps1_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListEmps1=true;
		}

		private void listEmps1_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListEmps1=false;
		}

		private void textEmployer2_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listEmps2.Visible=false;
				textCarrier2.Focus();
				return;
			}
			if(textEmployer2.Text=="") {
				listEmps2.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listEmps2.Items.Count==0) {
					return;
				}
				if(listEmps2.SelectedIndex==-1) {
					listEmps2.SelectedIndex=0;
					textEmployer2.Text=listEmps2.SelectedItem.ToString();
				}
				else if(listEmps2.SelectedIndex==listEmps2.Items.Count-1) {
					listEmps2.SelectedIndex=-1;
					textEmployer2.Text=empOriginal2;
				}
				else {
					listEmps2.SelectedIndex++;
					textEmployer2.Text=listEmps2.SelectedItem.ToString();
				}
				textEmployer2.SelectionStart=textEmployer2.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listEmps2.Items.Count==0) {
					return;
				}
				if(listEmps2.SelectedIndex==-1) {
					listEmps2.SelectedIndex=listEmps2.Items.Count-1;
					textEmployer2.Text=listEmps2.SelectedItem.ToString();
				}
				else if(listEmps2.SelectedIndex==0) {
					listEmps2.SelectedIndex=-1;
					textEmployer2.Text=empOriginal2;
				}
				else {
					listEmps2.SelectedIndex--;
					textEmployer2.Text=listEmps2.SelectedItem.ToString();
				}
				textEmployer2.SelectionStart=textEmployer2.Text.Length;
				return;
			}
			if(textEmployer2.Text.Length==1) {
				textEmployer2.Text=textEmployer2.Text.ToUpper();
				textEmployer2.SelectionStart=1;
			}
			empOriginal2=textEmployer2.Text;//the original text is preserved when using up and down arrows
			listEmps2.Items.Clear();
			List<Employer> similarEmps2=Employers.GetSimilarNames(textEmployer2.Text);
			for(int i=0;i<similarEmps2.Count;i++) {
				listEmps2.Items.Add(similarEmps2[i].EmpName);
			}
			int h=13*similarEmps2.Count+5;
			if(h > ClientSize.Height-listEmps2.Top){
				h=ClientSize.Height-listEmps2.Top;
			}
			listEmps2.Size=new Size(231,h);
			listEmps2.Visible=true;
		}

		private void textEmployer2_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListEmps2) {
				return;
			}
			listEmps2.Visible=false;
		}

		private void listEmps2_Click(object sender,System.EventArgs e) {
			textEmployer2.Text=listEmps2.SelectedItem.ToString();
			textEmployer2.Focus();
			textEmployer2.SelectionStart=textEmployer2.Text.Length;
			listEmps2.Visible=false;
		}

		private void listEmps2_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
			textEmployer2.Text=listEmps2.SelectedItem.ToString();
			textEmployer2.Focus();
			textEmployer2.SelectionStart=textEmployer2.Text.Length;
			listEmps2.Visible=false;
		}

		private void listEmps2_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListEmps2=true;
		}

		private void listEmps2_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListEmps2=false;
		}
		#endregion Employer

		#region Carrier
		///<summary>Fills the carrier fields on the form based on the specified carrierNum.</summary>
		private void FillCarrier1(long carrierNum) {
			selectedCarrier1=Carriers.GetCarrier(carrierNum);
			textCarrier1.Text=selectedCarrier1.CarrierName;
			textPhone1.Text=selectedCarrier1.Phone;
		}

		private void textCarrier1_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				if(listCars1.SelectedIndex==-1) {
					textPhone1.Focus();
				}
				else {
					FillCarrier1(similarCars1[listCars1.SelectedIndex].CarrierNum);
					textCarrier1.Focus();
					textCarrier1.SelectionStart=textCarrier1.Text.Length;
				}
				listCars1.Visible=false;
				return;
			}
			if(textCarrier1.Text=="") {
				listCars1.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listCars1.Items.Count==0) {
					return;
				}
				if(listCars1.SelectedIndex==-1) {
					listCars1.SelectedIndex=0;
					textCarrier1.Text=similarCars1[listCars1.SelectedIndex].CarrierName;
				}
				else if(listCars1.SelectedIndex==listCars1.Items.Count-1) {
					listCars1.SelectedIndex=-1;
					textCarrier1.Text=carOriginal1;
				}
				else {
					listCars1.SelectedIndex++;
					textCarrier1.Text=similarCars1[listCars1.SelectedIndex].CarrierName;
				}
				textCarrier1.SelectionStart=textCarrier1.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listCars1.Items.Count==0) {
					return;
				}
				if(listCars1.SelectedIndex==-1) {
					listCars1.SelectedIndex=listCars1.Items.Count-1;
					textCarrier1.Text=similarCars1[listCars1.SelectedIndex].CarrierName;
				}
				else if(listCars1.SelectedIndex==0) {
					listCars1.SelectedIndex=-1;
					textCarrier1.Text=carOriginal1;
				}
				else {
					listCars1.SelectedIndex--;
					textCarrier1.Text=similarCars1[listCars1.SelectedIndex].CarrierName;
				}
				textCarrier1.SelectionStart=textCarrier1.Text.Length;
				return;
			}
			if(textCarrier1.Text.Length==1) {
				textCarrier1.Text=textCarrier1.Text.ToUpper();
				textCarrier1.SelectionStart=1;
			}
			carOriginal1=textCarrier1.Text;//the original text is preserved when using up and down arrows
			listCars1.Items.Clear();
			similarCars1=Carriers.GetSimilarNames(textCarrier1.Text);
			for(int i=0;i<similarCars1.Count;i++) {
				listCars1.Items.Add(similarCars1[i].CarrierName+", "
					+similarCars1[i].Phone+", "
					+similarCars1[i].Address+", "
					+similarCars1[i].Address2+", "
					+similarCars1[i].City+", "
					+similarCars1[i].State+", "
					+similarCars1[i].Zip);
			}
			int h=13*similarCars1.Count+5;
			if(h > ClientSize.Height-listCars1.Top){
				h=ClientSize.Height-listCars1.Top;
			}
			listCars1.Size=new Size(listCars1.Width,h);
			listCars1.Visible=true;
		}

		private void textCarrier1_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListCars1) {
				return;
			}
			//or if user clicked on a different text box.
			if(listCars1.SelectedIndex!=-1) {
				FillCarrier1(similarCars1[listCars1.SelectedIndex].CarrierNum);
			}
			listCars1.Visible=false;
		}

		private void listCars1_Click(object sender,System.EventArgs e) {
			FillCarrier1(similarCars1[listCars1.SelectedIndex].CarrierNum);
			textCarrier1.Focus();
			textCarrier1.SelectionStart=textCarrier1.Text.Length;
			listCars1.Visible=false;
		}

		private void listCars1_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
		}

		private void listCars1_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListCars1=true;
		}

		private void listCars1_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListCars1=false;
		}

		///<summary>Fills the carrier fields on the form based on the specified carrierNum.</summary>
		private void FillCarrier2(long carrierNum) {
			selectedCarrier2=Carriers.GetCarrier(carrierNum);
			textCarrier2.Text=selectedCarrier2.CarrierName;
			textPhone2.Text=selectedCarrier2.Phone;
		}

		private void textCarrier2_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				if(listCars2.SelectedIndex==-1) {
					textPhone2.Focus();
				}
				else {
					FillCarrier2(similarCars2[listCars2.SelectedIndex].CarrierNum);
					textCarrier2.Focus();
					textCarrier2.SelectionStart=textCarrier2.Text.Length;
				}
				listCars2.Visible=false;
				return;
			}
			if(textCarrier2.Text=="") {
				listCars2.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listCars2.Items.Count==0) {
					return;
				}
				if(listCars2.SelectedIndex==-1) {
					listCars2.SelectedIndex=0;
					textCarrier2.Text=similarCars2[listCars2.SelectedIndex].CarrierName;
				}
				else if(listCars2.SelectedIndex==listCars2.Items.Count-1) {
					listCars2.SelectedIndex=-1;
					textCarrier2.Text=carOriginal2;
				}
				else {
					listCars2.SelectedIndex++;
					textCarrier2.Text=similarCars2[listCars2.SelectedIndex].CarrierName;
				}
				textCarrier2.SelectionStart=textCarrier2.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listCars2.Items.Count==0) {
					return;
				}
				if(listCars2.SelectedIndex==-1) {
					listCars2.SelectedIndex=listCars2.Items.Count-1;
					textCarrier2.Text=similarCars2[listCars2.SelectedIndex].CarrierName;
				}
				else if(listCars2.SelectedIndex==0) {
					listCars2.SelectedIndex=-1;
					textCarrier2.Text=carOriginal2;
				}
				else {
					listCars2.SelectedIndex--;
					textCarrier2.Text=similarCars2[listCars2.SelectedIndex].CarrierName;
				}
				textCarrier2.SelectionStart=textCarrier2.Text.Length;
				return;
			}
			if(textCarrier2.Text.Length==1) {
				textCarrier2.Text=textCarrier2.Text.ToUpper();
				textCarrier2.SelectionStart=1;
			}
			carOriginal2=textCarrier2.Text;//the original text is preserved when using up and down arrows
			listCars2.Items.Clear();
			similarCars2=Carriers.GetSimilarNames(textCarrier2.Text);
			for(int i=0;i<similarCars2.Count;i++) {
				listCars2.Items.Add(similarCars2[i].CarrierName+", "
					+similarCars2[i].Phone+", "
					+similarCars2[i].Address+", "
					+similarCars2[i].Address2+", "
					+similarCars2[i].City+", "
					+similarCars2[i].State+", "
					+similarCars2[i].Zip);
			}
			int h=13*similarCars2.Count+5;
			if(h > ClientSize.Height-listCars2.Top){
				h=ClientSize.Height-listCars2.Top;
			}
			listCars2.Size=new Size(listCars2.Width,h);
			listCars2.Visible=true;
		}

		private void textCarrier2_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListCars2) {
				return;
			}
			//or if user clicked on a different text box.
			if(listCars2.SelectedIndex!=-1) {
				FillCarrier2(similarCars2[listCars2.SelectedIndex].CarrierNum);
			}
			listCars2.Visible=false;
		}

		private void listCars2_Click(object sender,System.EventArgs e) {
			FillCarrier2(similarCars2[listCars2.SelectedIndex].CarrierNum);
			textCarrier2.Focus();
			textCarrier2.SelectionStart=textCarrier2.Text.Length;
			listCars2.Visible=false;
		}

		private void listCars2_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
		}

		private void listCars2_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListCars2=true;
		}

		private void listCars2_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListCars2=false;
		}
		#endregion Carrier

		#region InsPlanPick
		private void butPick1_Click(object sender,EventArgs e) {
			FormInsPlans FormIP=new FormInsPlans();
			FormIP.empText=textEmployer1.Text;
			FormIP.carrierText=textCarrier1.Text;
			FormIP.IsSelectMode=true;
			FormIP.ShowDialog();
			if(FormIP.DialogResult==DialogResult.Cancel) {
				return;
			}
			selectedPlan1=FormIP.SelectedPlan.Copy();
			//Non-synched fields:
			//selectedPlan1.SubscriberID=textSubscriberID.Text;//later
			//selectedPlan1.DateEffective=DateTime.MinValue;
			//selectedPlan1.DateTerm=DateTime.MinValue;
			//PlanCur.ReleaseInfo=checkRelease.Checked;
			//PlanCur.AssignBen=checkAssign.Checked;
			//PlanCur.SubscNote=textSubscNote.Text;
			//Benefits will be created when click OK.
			textEmployer1.Text=Employers.GetName(selectedPlan1.EmployerNum);
			FillCarrier1(selectedPlan1.CarrierNum);
			textGroupName1.Text=selectedPlan1.GroupName;
			textGroupNum1.Text=selectedPlan1.GroupNum;
		}

		private void butPick2_Click(object sender,EventArgs e) {
			FormInsPlans FormIP=new FormInsPlans();
			FormIP.empText=textEmployer2.Text;
			FormIP.carrierText=textCarrier2.Text;
			FormIP.IsSelectMode=true;
			FormIP.ShowDialog();
			if(FormIP.DialogResult==DialogResult.Cancel) {
				return;
			}
			selectedPlan2=FormIP.SelectedPlan.Copy();
			//Non-synched fields:
			//selectedPlan2.SubscriberID=textSubscriberID.Text;//later
			//selectedPlan2.DateEffective=DateTime.MinValue;
			//selectedPlan2.DateTerm=DateTime.MinValue;
			//PlanCur.ReleaseInfo=checkRelease.Checked;
			//PlanCur.AssignBen=checkAssign.Checked;
			//PlanCur.SubscNote=textSubscNote.Text;
			//Benefits will be created when click OK.
			textEmployer2.Text=Employers.GetName(selectedPlan2.EmployerNum);
			FillCarrier2(selectedPlan2.CarrierNum);
			textGroupName2.Text=selectedPlan2.GroupName;
			textGroupNum2.Text=selectedPlan2.GroupNum;
		}
		#endregion InsPlanPick

		private void butOK_Click(object sender,EventArgs e) {
			#region Validation
			if(  textBirthdate1.errorProvider1.GetError(textBirthdate1)!=""
				|| textBirthdate2.errorProvider1.GetError(textBirthdate2)!=""
				|| textBirthdate3.errorProvider1.GetError(textBirthdate3)!=""
				|| textBirthdate4.errorProvider1.GetError(textBirthdate4)!=""
				|| textBirthdate5.errorProvider1.GetError(textBirthdate5)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			//no validation on birthdate reasonableness.
			if(textLName1.Text=="" || textFName1.Text==""){
				MsgBox.Show(this,"Guarantor name must be entered.");
				return;
			}
			#region Validate Insurance Subscribers
			if((comboSubscriber1.SelectedIndex==2 || comboSubscriber2.SelectedIndex==2) && (textFName2.Text=="" || textLName2.Text=="")) {
				MsgBox.Show(this,"Subscriber must have name entered.");
				return;
			}
			if((comboSubscriber1.SelectedIndex==3 || comboSubscriber2.SelectedIndex==3) && (textFName3.Text=="" || textLName3.Text=="")) {
				MsgBox.Show(this,"Subscriber must have name entered.");
				return;
			}
			if((comboSubscriber1.SelectedIndex==4 || comboSubscriber2.SelectedIndex==4) && (textFName4.Text=="" || textLName4.Text=="")) {
				MsgBox.Show(this,"Subscriber must have name entered.");
				return;
			}
			if((comboSubscriber1.SelectedIndex==5 || comboSubscriber2.SelectedIndex==5) && (textFName5.Text=="" || textLName5.Text=="")) {
				MsgBox.Show(this,"Subscriber must have name entered.");
				return;
			}
			#endregion Validate Insurance Subscribers
			#region Validate Insurance Plans
			bool insComplete1=false;
			bool insComplete2=false;
			if(comboSubscriber1.SelectedIndex>0
				&& textSubscriberID1.Text!=""
				&& textCarrier1.Text!="")
			{
				insComplete1=true;
			}
			if(comboSubscriber2.SelectedIndex>0
				&& textSubscriberID2.Text!=""
				&& textCarrier2.Text!="")
			{
				insComplete2=true;
			}
			//test for insurance having only some of the critical fields filled in
			if(comboSubscriber1.SelectedIndex>0
				|| textSubscriberID1.Text!=""
				|| textCarrier1.Text!="")
			{
				if(!insComplete1) {
					MsgBox.Show(this,"Subscriber, Subscriber ID, and Carrier are all required fields if adding insurance.");
					return;
				}
			}
			if(comboSubscriber2.SelectedIndex>0
				|| textSubscriberID2.Text!=""
				|| textCarrier2.Text!="")
			{
				if(!insComplete2) {
					MsgBox.Show(this,"Subscriber, Subscriber ID, and Carrier are all required fields if adding insurance.");
					return;
				}
			}
			if(checkInsOne1.Checked
				|| checkInsOne2.Checked
				|| checkInsOne3.Checked
				|| checkInsOne4.Checked
				|| checkInsOne5.Checked)
			{
				if(!insComplete1) {
					MsgBox.Show(this,"Subscriber, Subscriber ID, and Carrier are all required fields if adding insurance.");
					return;
				}
			}
			if(checkInsTwo1.Checked
				|| checkInsTwo2.Checked
				|| checkInsTwo3.Checked
				|| checkInsTwo4.Checked
				|| checkInsTwo5.Checked)
			{
				if(!insComplete2) {
					MsgBox.Show(this,"Subscriber, Subscriber ID, and Carrier are all required fields if adding insurance.");
					return;
				}
			}
			#endregion Validate Insurance Plans
			#region Validate Insurance Subscriptions
			if(insComplete1) {
				if(!checkInsOne1.Checked
					&& !checkInsOne2.Checked
					&& !checkInsOne3.Checked
					&& !checkInsOne4.Checked
					&& !checkInsOne5.Checked)
				{
					MsgBox.Show(this,"Insurance information has been filled in, but has not been assigned to any patients.");
					return;
				}
				if(checkInsOne1.Checked && (textLName1.Text=="" || textFName1.Text=="")//Insurance1 assigned to invalid patient1
					|| checkInsOne2.Checked && (textLName2.Text=="" || textFName2.Text=="")//Insurance1 assigned to invalid patient2
					|| checkInsOne3.Checked && (textLName3.Text=="" || textFName3.Text=="")//Insurance1 assigned to invalid patient3
					|| checkInsOne4.Checked && (textLName4.Text=="" || textFName4.Text=="")//Insurance1 assigned to invalid patient4
					|| checkInsOne5.Checked && (textLName5.Text=="" || textFName5.Text=="")) //Insurance1 assigned to invalid patient5
				{
					MsgBox.Show(this,"Insurance information 1 has been filled in, but has been assigned to a patient with no name.");
					return;
				}
			}
			if(insComplete2) {
				if(!checkInsTwo1.Checked
					&& !checkInsTwo2.Checked
					&& !checkInsTwo3.Checked
					&& !checkInsTwo4.Checked
					&& !checkInsTwo5.Checked) {
					MsgBox.Show(this,"Insurance information 2 has been filled in, but has not been assigned to any patients.");
					return;
				}
				if(checkInsTwo1.Checked && (textLName1.Text=="" || textFName1.Text=="")//Insurance2 assigned to invalid patient1
					|| checkInsTwo2.Checked && (textLName2.Text=="" || textFName2.Text=="")//Insurance2 assigned to invalid patient2
					|| checkInsTwo3.Checked && (textLName3.Text=="" || textFName3.Text=="")//Insurance2 assigned to invalid patient3
					|| checkInsTwo4.Checked && (textLName4.Text=="" || textFName4.Text=="")//Insurance2 assigned to invalid patient4
					|| checkInsTwo5.Checked && (textLName5.Text=="" || textFName5.Text=="")) //Insurance2 assigned to invalid patient5
				{
					MsgBox.Show(this,"Insurance information 2 has been filled in, but has been assigned to a patient with no name.");
					return;
				}
			}
			#endregion Validate Insurance Subscriptions
			#endregion Validation
			#region Create Family
			Patient[] arrayPatsInFam=new Patient[5];
			Patient pat=new Patient();
			pat.BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType);
			pat.PatStatus=PatientStatus.Patient;
			pat.HmPhone=textHmPhone.Text;
			pat.Address=textAddress.Text;
			pat.Address2=textAddress2.Text;
			pat.City=textCity.Text;
			pat.State=textState.Text;
			pat.Country=textCountry.Text;
			pat.Zip=textZip.Text;
			pat.AddrNote=textAddrNotes.Text;
			pat.ClinicNum=FormOpenDental.ClinicNum;
			RefAttach refAttachCur=new RefAttach();
			if(_refCur!=null) {
				refAttachCur.ReferralNum=_refCur.ReferralNum;
				refAttachCur.IsFrom=true;
				refAttachCur.RefDate=DateTimeOD.Today;
				if(_refCur.IsDoctor) {//whether using ehr or not
					refAttachCur.IsTransitionOfCare=true;
				}
				refAttachCur.ItemOrder=1;
			}
			for(int i=0;i<arrayPatsInFam.Length;i++) {
				//this is just in case, since we are using the same Patient object for every family member inserted
				//probably not necessary since inserting will assign a new PatNum
				pat.PatNum=0;
				switch(i) {
					case 0://guarantor
						pat.LName=textLName1.Text;
						pat.FName=textFName1.Text;
						pat.Gender=(PatientGender)listGender1.SelectedIndex;
						pat.Position=(PatientPosition)listPosition1.SelectedIndex;
						pat.Birthdate=PIn.Date(textBirthdate1.Text);
						pat.PriProv=ProviderC.ListShort[comboPriProv1.SelectedIndex].ProvNum;
						if(comboSecProv1.SelectedIndex>0) {
							pat.SecProv=ProviderC.ListShort[comboSecProv1.SelectedIndex-1].ProvNum;//comboSecProv# contains 'none' so selected index -1
						}
						break;
					case 1://patient 2
						if(textFName2.Text=="" || textLName2.Text=="") {
							continue;
						}
						pat.PatNum=0;//may not be necessary, insert pat again with new values, insert will assign new PatNum
						pat.LName=textLName2.Text;
						pat.FName=textFName2.Text;
						pat.Gender=(PatientGender)listGender2.SelectedIndex;
						pat.Position=(PatientPosition)listPosition2.SelectedIndex;
						pat.Birthdate=PIn.Date(textBirthdate2.Text);
						pat.PriProv=ProviderC.ListShort[comboPriProv2.SelectedIndex].ProvNum;
						if(comboSecProv2.SelectedIndex>0) {
							pat.SecProv=ProviderC.ListShort[comboSecProv2.SelectedIndex-1].ProvNum;//comboSecProv# contains 'none' so selected index -1
						}
						break;
					case 2://patient 3
						if(textFName3.Text=="" || textLName3.Text=="") {
							continue;
						}
						pat.PatNum=0;//may not be necessary, insert pat again with new values, insert will assign new PatNum
						pat.LName=textLName3.Text;
						pat.FName=textFName3.Text;
						pat.Gender=(PatientGender)listGender3.SelectedIndex;
						pat.Position=PatientPosition.Child;
						pat.Birthdate=PIn.Date(textBirthdate3.Text);
						pat.PriProv=ProviderC.ListShort[comboPriProv3.SelectedIndex].ProvNum;
						if(comboSecProv3.SelectedIndex>0) {
							pat.SecProv=ProviderC.ListShort[comboSecProv3.SelectedIndex-1].ProvNum;//comboSecProv# contains 'none' so selected index -1
						}
						break;
					case 3://patient 4
						if(textFName4.Text=="" || textLName4.Text=="") {
							continue;
						}
						pat.PatNum=0;//may not be necessary, insert pat again with new values, insert will assign new PatNum
						pat.LName=textLName4.Text;
						pat.FName=textFName4.Text;
						pat.Gender=(PatientGender)listGender4.SelectedIndex;
						pat.Position=PatientPosition.Child;
						pat.Birthdate=PIn.Date(textBirthdate4.Text);
						pat.PriProv=ProviderC.ListShort[comboPriProv4.SelectedIndex].ProvNum;
						if(comboSecProv4.SelectedIndex>0) {
							pat.SecProv=ProviderC.ListShort[comboSecProv4.SelectedIndex-1].ProvNum;//comboSecProv# contains 'none' so selected index -1
						}
						break;
					case 4://patient 5
						if(textFName5.Text=="" || textLName5.Text=="") {
							continue;
						}
						pat.PatNum=0;//may not be necessary, insert pat again with new values, insert will assign new PatNum
						pat.LName=textLName5.Text;
						pat.FName=textFName5.Text;
						pat.Gender=(PatientGender)listGender5.SelectedIndex;
						pat.Position=PatientPosition.Child;
						pat.Birthdate=PIn.Date(textBirthdate5.Text);
						pat.PriProv=ProviderC.ListShort[comboPriProv5.SelectedIndex].ProvNum;
						if(comboSecProv5.SelectedIndex>0) {
							pat.SecProv=ProviderC.ListShort[comboSecProv5.SelectedIndex-1].ProvNum;//comboSecProv# contains 'none' so selected index -1
						}
						break;
				}
				Patients.Insert(pat,false);
				//if this is the first family member it is the guarantor, so set pat.Guarantor=pat.PatNum and update
				//if this is not the first family member, the guarantor has been inserted and pat.Guarantor will already be set before inserting
				if(i==0) {
					Patient patOld=pat.Copy();
					pat.Guarantor=pat.PatNum;
					Patients.Update(pat,patOld);
				}
				arrayPatsInFam[i]=pat.Copy();//add patient to local patient array of family members, arrayPatsInFam[0] will be the guarantor
				if(_refCur!=null) {
					refAttachCur.PatNum=pat.PatNum;
					RefAttaches.Insert(refAttachCur);
					SecurityLogs.MakeLogEntry(Permissions.RefAttachAdd,pat.PatNum,"Referred From "+Referrals.GetNameFL(refAttachCur.ReferralNum));
				}
				CustReference custRef=new CustReference();
				custRef.PatNum=pat.PatNum;
				CustReferences.Insert(custRef);
			}
			#endregion Create Family
			#region Insurance
			InsSub sub1=null;
			InsSub sub2=null;
			#region Validate Plans
			//validate the ins fields.  If they don't match perfectly, then set the selected plan to null
			if(selectedPlan1!=null) {
				if(Employers.GetName(selectedPlan1.EmployerNum)!=textEmployer1.Text
					|| Carriers.GetName(selectedPlan1.CarrierNum)!=textCarrier1.Text
					|| selectedPlan1.GroupName!=textGroupName1.Text
					|| selectedPlan1.GroupNum!=textGroupNum1.Text)
				{
					selectedPlan1=null;
				}
			}
			if(selectedPlan2!=null) {
				if(Employers.GetName(selectedPlan2.EmployerNum)!=textEmployer2.Text
					|| Carriers.GetName(selectedPlan2.CarrierNum)!=textCarrier2.Text
					|| selectedPlan2.GroupName!=textGroupName2.Text
					|| selectedPlan2.GroupNum!=textGroupNum2.Text)
				{
					selectedPlan2=null;
				}
			}
			//validate the carrier fields.  If they don't match perfectly, then set the selected plan to null
			if(selectedCarrier1!=null) {
				if(selectedCarrier1.CarrierName!=textCarrier1.Text || selectedCarrier1.Phone!=textPhone1.Text) {
					selectedCarrier1=null;
				}
			}
			if(selectedCarrier2!=null) {
				if(selectedCarrier2.CarrierName!=textCarrier2.Text || selectedCarrier2.Phone!=textPhone2.Text) {
					selectedCarrier2=null;
				}
			}
			#endregion Validate Plans
			#region Insert InsPlans, Benefits, and InsSubs
			#region Primary Ins
			if(insComplete1) {
				if(selectedCarrier1==null) {
					//get a carrier, possibly creating a new one if needed.
					selectedCarrier1=Carriers.GetByNameAndPhone(textCarrier1.Text,textPhone1.Text);
				}
				if(selectedPlan1==null) {
					//don't try to get a copy of an existing plan. Instead, start from scratch.
					selectedPlan1=new InsPlan();
					selectedPlan1.EmployerNum=Employers.GetEmployerNum(textEmployer1.Text);
					selectedPlan1.CarrierNum=selectedCarrier1.CarrierNum;
					selectedPlan1.GroupName=textGroupName1.Text;
					selectedPlan1.GroupNum=textGroupNum1.Text;
					selectedPlan1.PlanType="";
					InsPlans.Insert(selectedPlan1);
					Benefit ben=new Benefit();
					ben.PlanNum=selectedPlan1.PlanNum;//same for all benefits inserted
					ben.BenefitType=InsBenefitType.CoInsurance;//same for all benefits inserted from CovCatC.ListShort
					ben.MonetaryAmt=-1;//same for all benefits inserted from CovCatC.ListShort
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;//same for all benefits inserted
					ben.CodeNum=0;//same for all benefits inserted
					ben.CoverageLevel=BenefitCoverageLevel.None;//same for all benefits inserted from CovCatC.ListShort
					for(int i=0;i<CovCatC.ListShort.Count;i++) {
						if(CovCatC.ListShort[i].DefaultPercent==-1) {
							continue;
						}
						ben.CovCatNum=CovCatC.ListShort[i].CovCatNum;
						ben.Percent=CovCatC.ListShort[i].DefaultPercent;
						Benefits.Insert(ben);
					}
					ben.BenefitType=InsBenefitType.Deductible;//same for Diagnostic and RoutinePreventive
					ben.Percent=-1;//same for Diagnostic and RoutinePreventive
					ben.MonetaryAmt=0;//same for Diagnostic and RoutinePreventive
					ben.CoverageLevel=BenefitCoverageLevel.Individual;//same for Diagnostic and RoutinePreventive
					//Zero deductible diagnostic
					if(CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)!=null) {
						ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
						Benefits.Insert(ben);
					}
					//Zero deductible preventive
					if(CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null) {
						ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
						Benefits.Insert(ben);
					}
				}
				sub1=new InsSub();
				sub1.PlanNum=selectedPlan1.PlanNum;
				sub1.AssignBen=PrefC.GetBool(PrefName.InsDefaultAssignBen);
				sub1.ReleaseInfo=true;
				sub1.DateEffective=DateTime.MinValue;
				sub1.DateTerm=DateTime.MinValue;
				if(comboSubscriber1.SelectedIndex>0) {//comboSubscriber has been validated to contain the same number of indexes as the family list
					sub1.Subscriber=arrayPatsInFam[comboSubscriber1.SelectedIndex-1].PatNum;
				}
				sub1.SubscriberID=textSubscriberID1.Text;
				InsSubs.Insert(sub1);
			}
			#endregion Primary Ins
			#region Secondary Ins
			if(insComplete2) {
				if(selectedCarrier2==null) {
					selectedCarrier2=Carriers.GetByNameAndPhone(textCarrier2.Text,textPhone2.Text);
				}
				if(selectedPlan2==null) {
					//don't try to get a copy of an existing plan. Instead, start from scratch.
					selectedPlan2=new InsPlan();
					selectedPlan2.EmployerNum=Employers.GetEmployerNum(textEmployer2.Text);
					selectedPlan2.CarrierNum=selectedCarrier2.CarrierNum;
					selectedPlan2.GroupName=textGroupName2.Text;
					selectedPlan2.GroupNum=textGroupNum2.Text;
					selectedPlan2.PlanType="";
					InsPlans.Insert(selectedPlan2);
					Benefit ben=new Benefit();
					ben.PlanNum=selectedPlan2.PlanNum;//same for all benefits inserted
					ben.BenefitType=InsBenefitType.CoInsurance;//same for all benefits inserted from CovCatC.ListShort
					ben.MonetaryAmt=-1;//same for all benefits inserted from CovCatC.ListShort
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;//same for all benefits inserted
					ben.CodeNum=0;//same for all benefits inserted
					ben.CoverageLevel=BenefitCoverageLevel.None;//same for all benefits inserted from CovCatC.ListShort
					for(int i=0;i<CovCatC.ListShort.Count;i++) {
						if(CovCatC.ListShort[i].DefaultPercent==-1) {
							continue;
						}
						ben.CovCatNum=CovCatC.ListShort[i].CovCatNum;
						ben.Percent=CovCatC.ListShort[i].DefaultPercent;
						Benefits.Insert(ben);
					}
					ben.BenefitType=InsBenefitType.Deductible;//same for Diagnostic and RoutinePreventive
					ben.Percent=-1;//same for Diagnostic and RoutinePreventive
					ben.MonetaryAmt=0;//same for Diagnostic and RoutinePreventive
					ben.CoverageLevel=BenefitCoverageLevel.Individual;//same for Diagnostic and RoutinePreventive
					//Zero deductible diagnostic
					if(CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)!=null) {
						ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
						Benefits.Insert(ben);
					}
					//Zero deductible preventive
					if(CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null) {
						ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
						Benefits.Insert(ben);
					}
				}
				sub2=new InsSub();
				sub2.PlanNum=selectedPlan2.PlanNum;
				sub2.AssignBen=PrefC.GetBool(PrefName.InsDefaultAssignBen);
				sub2.ReleaseInfo=true;
				sub2.DateEffective=DateTime.MinValue;
				sub2.DateTerm=DateTime.MinValue;
				if(comboSubscriber2.SelectedIndex>0) {//comboSubscriber has been validated to contain the same number of indexes as the family list
					sub2.Subscriber=arrayPatsInFam[comboSubscriber2.SelectedIndex-1].PatNum;
				}
				sub2.SubscriberID=textSubscriberID2.Text;
				InsSubs.Insert(sub2);
			}
			#endregion Secondary Ins
			#endregion Insert InsPlans, Benefits, and InsSubs
			#region Create PatPlans
			PatPlan patplan1=new PatPlan();
			if(insComplete1) {
				patplan1.InsSubNum=sub1.InsSubNum;
			}
			PatPlan patplan2=new PatPlan();
			if(insComplete2) {
				patplan2.InsSubNum=sub2.InsSubNum;
			}
			for(int i=0;i<arrayPatsInFam.Length;i++) {//loop for each possible family member. Position 0 is the guarantor and is required but all others could be null
				if(!insComplete1 && !insComplete2) {
					break;
				}
				if(arrayPatsInFam[i]==null) {
					continue;
				}
				patplan1.PatNum=arrayPatsInFam[i].PatNum;
				patplan1.Ordinal=1;
				patplan1.Relationship=Relat.Child;//default realtionship to child, will be set to Self or Spouse for the two adults in the family
				patplan2.PatNum=arrayPatsInFam[i].PatNum;
				patplan2.Ordinal=2;
				patplan2.Relationship=Relat.Child;//default realtionship to child, will be set to Self or Spouse for the two adults in the family
				switch(i) {
					case 0://guarantor, 1st adult
						if(insComplete1 && checkInsOne1.Checked) {
							//the only situation where ordinal would be 2 is if ins2 is checked and ins1 does not have this pat as the subscriber and ins2 does
							if(checkInsTwo1.Checked && comboSubscriber1.SelectedIndex!=1 && comboSubscriber2.SelectedIndex==1) {//both combo boxes contain 'none'
								patplan1.Ordinal=2;
							}
							patplan1.Relationship=Relat.Self;//the subscriber would never be a child, so default to Self
							if(comboSubscriber1.SelectedIndex==2) {
								patplan1.Relationship=Relat.Spouse;
							}
							PatPlans.Insert(patplan1);
						}
						if(insComplete2 && checkInsTwo1.Checked) {
							//the only situations where ordinal would be 1 is if ins1 is not checked or if ins2 has this patient as subscriber and ins1 does not.
							if(!checkInsOne1.Checked || (comboSubscriber2.SelectedIndex==1 && comboSubscriber1.SelectedIndex!=1))	{
								patplan2.Ordinal=1;
							}
							patplan2.Relationship=Relat.Self;//the subscriber would never be a child, so default to Self
							if(comboSubscriber2.SelectedIndex==2) {
								patplan2.Relationship=Relat.Spouse;
							}
							PatPlans.Insert(patplan2);
						}
						continue;
					case 1://patient 1, 2nd adult
						if(insComplete1 && checkInsOne2.Checked) {
							//the only situation where ordinal would be 2 is if ins2 is checked and ins1 does not have this pat as the subscriber and ins2 does
							if(checkInsTwo2.Checked && comboSubscriber1.SelectedIndex!=2 && comboSubscriber2.SelectedIndex==2) {
								patplan1.Ordinal=2;
							}
							patplan1.Relationship=Relat.Self;//the subscriber would never be a child, so default to Self
							if(comboSubscriber1.SelectedIndex==1) {
								patplan1.Relationship=Relat.Spouse;
							}
							PatPlans.Insert(patplan1);
						}
						if(insComplete2 && checkInsTwo2.Checked) {
							//the only situations where ordinal would be 1 is if ins1 is not checked or if ins2 has this patient as subscriber and ins1 does not.
							if(!checkInsOne2.Checked || (comboSubscriber2.SelectedIndex==2 && comboSubscriber1.SelectedIndex!=2))	{
								patplan2.Ordinal=1;
							}
							patplan2.Relationship=Relat.Self;//the subscriber would never be a child, so default to Self
							if(comboSubscriber2.SelectedIndex==1) {
								patplan2.Relationship=Relat.Spouse;
							}
							PatPlans.Insert(patplan2);
						}
						continue;
					case 2://patient 2, 1st child
						if(insComplete1 && checkInsOne3.Checked) {
							PatPlans.Insert(patplan1);
						}
						if(insComplete2 && checkInsTwo3.Checked) {
							//the only situation where ordinal would be 1 is if ins1 is not checked.
							if(!checkInsOne3.Checked) {
								patplan2.Ordinal=1;
							}
							PatPlans.Insert(patplan2);
						}
						continue;
					case 3://patient 3, 2nd child
						if(insComplete1 && checkInsOne4.Checked) {
							PatPlans.Insert(patplan1);
						}
						if(insComplete2 && checkInsTwo4.Checked) {
							//the only situation where ordinal would be 1 is if ins1 is not checked.
							if(!checkInsOne4.Checked) {
								patplan2.Ordinal=1;
							}
							PatPlans.Insert(patplan2);
						}
						continue;
					case 4://patient 4, 3rd child
						if(insComplete1 && checkInsOne5.Checked) {
							PatPlans.Insert(patplan1);
						}
						if(insComplete2 && checkInsTwo5.Checked) {
							//the only situation where ordinal would be 1 is if ins1 is not checked.
							if(!checkInsOne5.Checked) {
								patplan2.Ordinal=1;
							}
							PatPlans.Insert(patplan2);
						}
						continue;
				}
			}
			#endregion Create PatPlans
			#endregion Insurance
			SelectedPatNum=arrayPatsInFam[0].PatNum;
			#region Send HL7 if Applicable
			//If there is an existing HL7 def enabled, send an ADT message for each patient inserted if there is an outbound ADT message defined
			if(HL7Defs.IsExistingHL7Enabled()) {
				for(int i=0;i<5;i++) {
					if(arrayPatsInFam[i]==null) {
						continue;
					}
					//new patients get the A04 ADT, updating existing patients we send an A08
					MessageHL7 messageHL7=MessageConstructor.GenerateADT(arrayPatsInFam[i],arrayPatsInFam[0],EventTypeHL7.A04);//arrayPatsInFam[0] is guar, never null
					//Will be null if there is no outbound ADT message defined, so do nothing
					if(messageHL7==null) {
						continue;
					}
					HL7Msg hl7Msg=new HL7Msg();
					hl7Msg.AptNum=0;
					hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
					hl7Msg.MsgText=messageHL7.ToString();
					hl7Msg.PatNum=arrayPatsInFam[i].PatNum;
					HL7Msgs.Insert(hl7Msg);
#if DEBUG
					MessageBox.Show(this,messageHL7.ToString());
#endif
				}
			}
			#endregion Send HL7 if Applicable
			MessageBox.Show("Done");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		

		

		





		

		

		

		

		




	

		
	}
}