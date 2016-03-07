using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsVerificationSetup:ODForm {
		private bool _hasChanged;

		public FormInsVerificationSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormInsVerificationSetup_Load(object sender,EventArgs e) {
			textInsBenefitEligibilityDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyBenefitEligibilityDays));
			textPatientEnrollmentDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyPatientEnrollmentDays));
			textScheduledAppointmentDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyAppointmentScheduledDays));
			checkInsVerifyUseCurrentUser.Checked=PrefC.GetBool(PrefName.InsVerifyDefaultToCurrentUser);
		}

		private void butOK_Click(object sender,EventArgs e) {
			int insBenefitEligibilityDays=0;//Placeholder
			if(!int.TryParse(textInsBenefitEligibilityDays.Text,out insBenefitEligibilityDays)) {
				MsgBox.Show(this,"The number entered for insurance benefit eligibility was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			int patientEnrollmentDays=0;//Placeholder
			if(!int.TryParse(textPatientEnrollmentDays.Text,out patientEnrollmentDays)) {
				MsgBox.Show(this,"The number entered for patient enrollment was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			int scheduledAppointmentDays=0;//Placeholder
			if(!int.TryParse(textScheduledAppointmentDays.Text,out scheduledAppointmentDays)) {
				MsgBox.Show(this,"The number entered for scheduled appointments was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(Prefs.UpdateInt(PrefName.InsVerifyBenefitEligibilityDays,insBenefitEligibilityDays)
				| Prefs.UpdateInt(PrefName.InsVerifyPatientEnrollmentDays,patientEnrollmentDays)
				| Prefs.UpdateInt(PrefName.InsVerifyAppointmentScheduledDays,scheduledAppointmentDays)
				| Prefs.UpdateBool(PrefName.InsVerifyDefaultToCurrentUser,checkInsVerifyUseCurrentUser.Checked)) 
			{
					_hasChanged=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormInsVerificationSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}