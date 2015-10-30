using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormBillingDefaults:Form {
		private List<Clinic> _listClinics;
		private List<Ebill> _listEbills;
		///<summary>The eBill corresponding to the currently selected clinic if clinics are enabled.</summary>
		private Ebill _eBillCur;
		///<summary>The eBill corresponding to the default credentials.</summary>
		private Ebill _eBillDefault;

		public FormBillingDefaults() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormBillingDefaults_Load(object sender,EventArgs e) {
			textDays.Text=PrefC.GetLong(PrefName.BillingDefaultsLastDays).ToString();
			checkIntermingled.Checked=PrefC.GetBool(PrefName.BillingDefaultsIntermingle);
			textNote.Text=PrefC.GetString(PrefName.BillingDefaultsNote);
			listElectBilling.SelectedIndex=0;
			if(PrefC.GetString(PrefName.BillingUseElectronic)=="1") {
				listElectBilling.SelectedIndex=1;
			}
			if(PrefC.GetString(PrefName.BillingUseElectronic)=="2") {
				listElectBilling.SelectedIndex=2;
			}
			if(PrefC.GetString(PrefName.BillingUseElectronic)=="3") {
				listElectBilling.SelectedIndex=3;
			}
			textVendorId.Text=PrefC.GetString(PrefName.BillingElectVendorId);
			textVendorPMScode.Text=PrefC.GetString(PrefName.BillingElectVendorPMSCode);
			string cc=PrefC.GetString(PrefName.BillingElectCreditCardChoices);
			if(cc.Contains("MC")) {
				checkMC.Checked=true;
			}
			if(cc.Contains("V")) {
				checkV.Checked=true;
			}
			if(cc.Contains("D")) {
				checkD.Checked=true;
			}
			if(cc.Contains("A")) {
				checkAmEx.Checked=true;
			}
			textBillingEmailSubject.Text=PrefC.GetString(PrefName.BillingEmailSubject);
			textBillingEmailBody.Text=PrefC.GetString(PrefName.BillingEmailBodyText);
			textInvoiceNote.Text=PrefC.GetString(PrefName.BillingDefaultsInvoiceNote);
			_listEbills=Ebills.GetList();
			//Find the default Ebill
			for(int i=0;i<_listEbills.Count;i++) {
				if(_listEbills[i].ClinicNum==0) {
					_eBillDefault=_listEbills[i];
				}
			}
			_eBillCur=_eBillDefault;
			//Set the textboxes to default values.
			textClientAcctNumber.Text=_eBillDefault.ClientAcctNumber;
			textUserName.Text=_eBillDefault.ElectUserName;
			textPassword.Text=_eBillDefault.ElectPassword;
			string[] arrayEbillAddressEnums=Enum.GetNames(typeof(EbillAddress));
			for(int i=0;i<arrayEbillAddressEnums.Length;i++) {
				comboPracticeAddr.Items.Add(arrayEbillAddressEnums[i]);
				comboRemitAddr.Items.Add(arrayEbillAddressEnums[i]);
				//If clinics are off don't add the Clinic specific EbillAddress enums
				if(!PrefC.HasClinicsEnabled && i==2) {
					break;
				}
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Visible=true;
				labelClinic.Visible=true;
				//Bold clinic specific fields.
				groupBoxBilling.Text=Lan.g(this,"Electronic Billing - Bolded fields are clinic specific");
				labelAcctNum.Font=new Font(labelAcctNum.Font,FontStyle.Bold);
				labelUserName.Font=new Font(labelUserName.Font,FontStyle.Bold);
				labelPassword.Font=new Font(labelPassword.Font,FontStyle.Bold);
				labelClinic.Font=new Font(labelClinic.Font,FontStyle.Bold);
				labelPracticeAddr.Font=new Font(labelPracticeAddr.Font,FontStyle.Bold);
				labelRemitAddr.Font=new Font(labelRemitAddr.Font,FontStyle.Bold);
				comboClinic.Items.Clear();
				_listClinics=new List<Clinic>();
				//Add "Unassigned/Default" option if the user isn't restricted or the selected clinic is 0.
				if(!Security.CurUser.ClinicIsRestricted || FormOpenDental.ClinicNum==0) {
					Clinic clinicUnassigned=new Clinic();
					clinicUnassigned.ClinicNum=0;
					clinicUnassigned.Description="Unassigned/Default";
					_listClinics.Add(clinicUnassigned);
				}
				_listClinics.AddRange(Clinics.GetForUserod(Security.CurUser));
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Description);
					//If the clinic we're adding is the one currently selected in OD, attempt to find its Ebill entry.
					if(_listClinics[i].ClinicNum==FormOpenDental.ClinicNum) {
						comboClinic.SelectedIndex=i;
						Ebill eBill=null;
						if(FormOpenDental.ClinicNum==0) {//Use the default Ebill if OD has Headquarters selected or if clinics are disabled.
							eBill=_eBillDefault;
						}
						else {
							eBill=_listEbills.FirstOrDefault(x => x.ClinicNum==_listClinics[i].ClinicNum);//Can be null.
						}
						//_eBillCur will be the default Ebill, the clinic's Ebill, or null if there are no existing ebills for OD's selected clinic.
						_eBillCur=eBill;
					}
				}
			}
			//Load _eBillCur's fields into the UI.
			LoadEbill(_eBillCur);
		}

		///<summary>eBill can be null, creates Ebill if needed.</summary>
		private void LoadEbill(Ebill eBill) {
			if(eBill==null) {//Matching Ebill entry not found.  Make a new entry with default values.
				eBill=new Ebill();
				eBill.ClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				eBill.ClientAcctNumber="";
				eBill.ElectUserName="";
				eBill.ElectPassword="";
				eBill.PracticeAddress=EbillAddress.PracticePhysical;
				eBill.RemitAddress=EbillAddress.PracticeBilling;
				_listEbills.Add(eBill);
			}
			textClientAcctNumber.Text=_eBillDefault.ClientAcctNumber;
			if(eBill.ClientAcctNumber!="") {//If the Ebill field is blank use default value.
				textClientAcctNumber.Text=eBill.ClientAcctNumber;
			}
			textUserName.Text=_eBillDefault.ElectUserName;
			if(eBill.ElectUserName!="") {//If the Ebill field is blank use default value.
				textUserName.Text=eBill.ElectUserName;
			}
			textPassword.Text=_eBillDefault.ElectPassword;
			if(eBill.ElectPassword!="") {//If the Ebill field is blank use default value.
				textPassword.Text=eBill.ElectPassword;
			}
			//If clinics are disabled and the eBill had a clinic specific enum, set it to default value.  May happen if clinics were previously enabled.
			if(PrefC.HasClinicsEnabled) {
				comboPracticeAddr.SelectedIndex=(int)eBill.PracticeAddress;
				comboRemitAddr.SelectedIndex=(int)eBill.RemitAddress;
			}
			else {//No clinics
				if(eBill.PracticeAddress==EbillAddress.ClinicPhysical) {
					comboPracticeAddr.SelectedIndex=0;//PracticePhysical
				}
				else if(eBill.PracticeAddress==EbillAddress.ClinicBilling) {
					comboPracticeAddr.SelectedIndex=1;//PracticeBilling
				}
				else if(eBill.PracticeAddress==EbillAddress.ClinicPayTo) {
					comboPracticeAddr.SelectedIndex=2;//PracticePayTo
				}
				else {
					comboPracticeAddr.SelectedIndex=(int)eBill.PracticeAddress;
				}
				if(eBill.RemitAddress==EbillAddress.ClinicPhysical) {
					comboRemitAddr.SelectedIndex=0;//PracticePhysical
				}
				else if(eBill.RemitAddress==EbillAddress.ClinicBilling) {
					comboRemitAddr.SelectedIndex=1;//PracticeBilling
				}
				else if(eBill.RemitAddress==EbillAddress.ClinicPayTo) {
					comboRemitAddr.SelectedIndex=2;//PracticePayTo
				}
				else {
					comboRemitAddr.SelectedIndex=(int)eBill.RemitAddress;
				}
			}
			_eBillCur=eBill;
		}

		///<summary>Saves the current Ebill information from the UI into the cache.</summary>
		private void SaveEbill(Ebill eBill) {
			if(eBill.ClinicNum==0) {//If the ebill being edited is for the defaults use what's in the text
				eBill.ClientAcctNumber=textClientAcctNumber.Text;
				eBill.ElectUserName=textUserName.Text;
				eBill.ElectPassword=textPassword.Text;
			}			
			else {//If the ebill isn't the default
				if(textClientAcctNumber.Text!="" && textClientAcctNumber.Text!=_eBillDefault.ClientAcctNumber) {
					eBill.ClientAcctNumber=textClientAcctNumber.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ClientAcctNumber="";
				}
				if(textUserName.Text!="" && textUserName.Text!=_eBillDefault.ElectUserName) {
					eBill.ElectUserName=textUserName.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ElectUserName="";
				}
				if(textPassword.Text!="" && textPassword.Text!=_eBillDefault.ElectPassword) {
					eBill.ElectPassword=textPassword.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ElectPassword="";
				}
			}
			eBill.PracticeAddress=(EbillAddress)comboPracticeAddr.SelectedIndex;
			eBill.RemitAddress=(EbillAddress)comboRemitAddr.SelectedIndex;			
		}

		private void listElectBilling_SelectedIndexChanged(object sender,EventArgs e) {
			//If Dental X Change is selected, enable its textboxes and combo.
			if(listElectBilling.SelectedIndex==1) {
				comboRemitAddr.Enabled=true;
				textUserName.ReadOnly=false;
				textPassword.ReadOnly=false;
				textClientAcctNumber.ReadOnly=false;
				textVendorId.ReadOnly=false;
				textVendorPMScode.ReadOnly=false;
			}
			else {
				//If Dental X Change is not selected, disable changing information for fields the selected format won't use.
				comboRemitAddr.Enabled=false;
				textUserName.ReadOnly=true;
				textPassword.ReadOnly=true;
				textClientAcctNumber.ReadOnly=true;
				textVendorId.ReadOnly=true;
				textVendorPMScode.ReadOnly=true;
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			SaveEbill(_eBillCur);
			Ebill eBill=null;
			if((!Security.CurUser.ClinicIsRestricted || FormOpenDental.ClinicNum==0) && comboClinic.SelectedIndex==0) {//Unassigned/Default
				eBill=_eBillDefault;
			}
			else {//Otherwise locate the Ebill from the cache.
				for(int i=0;i<_listEbills.Count;i++) {
					if(_listEbills[i].ClinicNum==_listClinics[comboClinic.SelectedIndex].ClinicNum) {//Check for existing Ebill entry
						eBill=_listEbills[i];
						break;
					}
				}
			}
			LoadEbill(eBill);//Could be null if user switches to a clinic which has not Ebill entry yet.
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDays.errorProvider1.GetError(textDays)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			string cc="";
			if(checkMC.Checked) {
				cc="MC";
			}
			if(checkV.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="V";
			}
			if(checkD.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="D";
			}
			if(checkAmEx.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="A";
			}
			string billingUseElectronic=listElectBilling.SelectedIndex.ToString();
			SaveEbill(_eBillCur);
			if(Prefs.UpdateLong(PrefName.BillingDefaultsLastDays,PIn.Long(textDays.Text))
				| Prefs.UpdateBool(PrefName.BillingDefaultsIntermingle,checkIntermingled.Checked)
				| Prefs.UpdateString(PrefName.BillingDefaultsNote,textNote.Text)
				| Prefs.UpdateString(PrefName.BillingUseElectronic,billingUseElectronic)
				| Prefs.UpdateString(PrefName.BillingEmailSubject,textBillingEmailSubject.Text)
				| Prefs.UpdateString(PrefName.BillingEmailBodyText,textBillingEmailBody.Text)
				| Prefs.UpdateString(PrefName.BillingElectVendorId,textVendorId.Text)
				| Prefs.UpdateString(PrefName.BillingElectVendorPMSCode,textVendorPMScode.Text)
				| Prefs.UpdateString(PrefName.BillingElectCreditCardChoices,cc)
				| Prefs.UpdateString(PrefName.BillingDefaultsInvoiceNote,textInvoiceNote.Text))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(Ebills.Sync(_listEbills)) {//Includes the default Ebill
				DataValid.SetInvalid(InvalidType.Ebills);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	
	}
}