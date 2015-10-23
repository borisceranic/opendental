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
			_eBillDefault=new Ebill();
			_eBillDefault.ClientAcctNumber=PrefC.GetString(PrefName.BillingElectClientAcctNumber);
			_eBillDefault.ElectUserName=PrefC.GetString(PrefName.BillingElectUserName);
			_eBillDefault.ElectPassword=PrefC.GetString(PrefName.BillingElectPassword);
			_eBillDefault.ClinicNum=0;
			_eBillCur=_eBillDefault;
			textClientAcctNumber.Text=PrefC.GetString(PrefName.BillingElectClientAcctNumber);
			textUserName.Text=PrefC.GetString(PrefName.BillingElectUserName);
			textPassword.Text=PrefC.GetString(PrefName.BillingElectPassword);
			//email
			textBillingEmailSubject.Text=PrefC.GetString(PrefName.BillingEmailSubject);
			textBillingEmailBody.Text=PrefC.GetString(PrefName.BillingEmailBodyText);
			textInvoiceNote.Text=PrefC.GetString(PrefName.BillingDefaultsInvoiceNote);
			_listEbills=Ebills.GetList();
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Visible=true;
				labelClinic.Visible=true;
				comboClinic.Items.Clear();
				_listClinics=new List<Clinic>();
				if(!Security.CurUser.ClinicIsRestricted || FormOpenDental.ClinicNum==0) {
					Clinic clinicUnassigned=new Clinic();
					clinicUnassigned.ClinicNum=0;
					clinicUnassigned.Description="Unassigned/Default";
					_listClinics.Add(clinicUnassigned);
				}
				_listClinics.AddRange(Clinics.GetForUserod(Security.CurUser));
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Description);
					if(_listClinics[i].ClinicNum==FormOpenDental.ClinicNum) {
						comboClinic.SelectedIndex=i;
						Ebill eBill=null;
						if(_listClinics[i].ClinicNum==0) {
							eBill=_eBillDefault;
						}
						else {
							eBill=_listEbills.FirstOrDefault(x => x.ClinicNum==_listClinics[i].ClinicNum);//Can be null.
						}
						LoadEbill(eBill);
					}
				}
			}
		}

		///<summary>eBill can be null, creates Ebill if needed.</summary>
		private void LoadEbill(Ebill eBill) {
			if(eBill==null) {//Matching Ebill entry not found.  Make a new entry with default values.
				eBill=new Ebill();
				eBill.ClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
				eBill.ClientAcctNumber="";
				eBill.ElectUserName="";
				eBill.ElectPassword="";
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
				| Prefs.UpdateString(PrefName.BillingElectClientAcctNumber,_eBillDefault.ClientAcctNumber)
				| Prefs.UpdateString(PrefName.BillingElectUserName,_eBillDefault.ElectUserName)
				| Prefs.UpdateString(PrefName.BillingElectPassword,_eBillDefault.ElectPassword)
				| Prefs.UpdateString(PrefName.BillingDefaultsInvoiceNote,textInvoiceNote.Text))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(PrefC.HasClinicsEnabled && Ebills.Sync(_listEbills)) {
				DataValid.SetInvalid(InvalidType.Ebills);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	
	}
}