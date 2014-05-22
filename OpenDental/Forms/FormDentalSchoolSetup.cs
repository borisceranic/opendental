using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormDentalSchoolSetup:Form {

		public FormDentalSchoolSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDentalSchoolSetup_Load(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				butInstructorPicker.Enabled=false;
				butStudentPicker.Enabled=false;
			}
			UserGroup studentGroup=UserGroups.GetGroup(PrefC.GetLong(PrefName.SecurityGroupForStudents));
			UserGroup instructorGroup=UserGroups.GetGroup(PrefC.GetLong(PrefName.SecurityGroupForInstructors));
			if(studentGroup!=null) {
				textStudents.Text=UserGroups.GetGroup(PrefC.GetLong(PrefName.SecurityGroupForStudents)).Description;
			}
			if(instructorGroup!=null) {
				textInstructors.Text=UserGroups.GetGroup(PrefC.GetLong(PrefName.SecurityGroupForInstructors)).Description;
			}
		}

		private void butStudentPicker_Click(object sender,EventArgs e) {
			FormUserGroupPicker FormUGP=new FormUserGroupPicker();
			FormUGP.ShowDialog();
			if(FormUGP.DialogResult==DialogResult.OK) {
				if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will change the security group for all students. Continue?")) {
					try {
						Userods.UpdateUserGroupsForDentalSchools(FormUGP.UserGroup,false);
						Prefs.UpdateLong(PrefName.SecurityGroupForStudents,FormUGP.UserGroup.UserGroupNum);
					}
					catch {
						MsgBox.Show(this,"Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.  Give the SecurityAdmin permission to at least one user that is in another group or is not flagged as a student or instructor.");
						return;
					}
					textStudents.Text=FormUGP.UserGroup.Description;
				}
			}
		}

		private void butInstructorPicker_Click(object sender,EventArgs e) {
			FormUserGroupPicker FormUGP=new FormUserGroupPicker();
			FormUGP.ShowDialog();
			if(FormUGP.DialogResult==DialogResult.OK) {
				if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will change the security group for all instructors. Continue?")) {
					try {
						Userods.UpdateUserGroupsForDentalSchools(FormUGP.UserGroup,true);
						Prefs.UpdateLong(PrefName.SecurityGroupForInstructors,FormUGP.UserGroup.UserGroupNum);
					}
					catch {
						MsgBox.Show(this,"Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.  Give the SecurityAdmin permission to at least one user that is in another group or is not flagged as a student or instructor.");
						return;
					}
					textInstructors.Text=FormUGP.UserGroup.Description;
				}
			}
		}

		private void butGradingScales_Click(object sender,EventArgs e) {
			FormGradingScales FormGS=new FormGradingScales();
			FormGS.ShowDialog();
		}

		private void butEvaluation_Click(object sender,EventArgs e) {
			FormEvaluationDefs FormED=new FormEvaluationDefs();
			FormED.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}