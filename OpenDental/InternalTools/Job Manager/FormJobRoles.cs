using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormJobRoles:Form {
		private long _userNum;
		private List<JobRole> _jobRoles;

		///<summary>Pass in the jobNum for existing jobs.</summary>
		public FormJobRoles(long userNum) {
			_userNum=userNum;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobRoles_Load(object sender,EventArgs e) {
			_jobRoles=JobRoles.GetForUser(_userNum);
			Array jobRoleValues=Enum.GetValues(typeof(JobRoleType));
			for(int i=0;i<jobRoleValues.Length;i++) {
				listAvailable.Items.Add(Enum.GetName(typeof(JobRoleType),i));
				if(_jobRoles.Count(x => (int)x.RoleType==i)>0) {
					listAvailable.SelectedIndices.Add(i);
				}
			}
		}


		private void butEngineer_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Engineer);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
		}

		private void butPreExpert_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Writeup);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Review);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Engineer);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
		}

		private void butExpert_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Writeup);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Assignment);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Review);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Engineer);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Quote);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Override);
		}

		private void butTechWriter_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Documentation);
		}

		private void butJobManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Assignment);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Approval);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Quote);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Override);
		}

		private void butFeatureManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.FeatureManager);
		}

		private void butQueryManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.QueryManager);
		}

		private void butCustomerManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.NotifyCustomer);
		}

		private void butQuoteManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobRoleType.Concept);
			listAvailable.SelectedIndices.Add((int)JobRoleType.Quote);
		}

		private void butOK_Click(object sender,EventArgs e) {
			_jobRoles.Clear();
			JobRole jobRole;
			for(int i=0;i<listAvailable.SelectedIndices.Count;i++) {
				jobRole=new JobRole();
				jobRole.UserNum=_userNum;
				jobRole.RoleType=(JobRoleType)listAvailable.SelectedIndices[i];
				_jobRoles.Add(jobRole);
			}
			JobRoles.Sync(_jobRoles,_userNum);
			DataValid.SetInvalid(InvalidType.JobRoles);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}