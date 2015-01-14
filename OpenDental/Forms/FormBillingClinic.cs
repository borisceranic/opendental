using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormBillingClinic:Form {
		///<summary>0 - all clinics.</summary>
		public long ClinicNum;
		private List<Clinic> _listClinics;

		public FormBillingClinic() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormBillingClinic_Load(object sender,EventArgs e) {
			listClinic.Items.Add("All");
			listClinic.SelectedIndex=0;
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			for(int i=0;i<_listClinics.Count;i++) {
				listClinic.Items.Add(_listClinics[i].Description);
				if(_listClinics[i].ClinicNum==ClinicNum) {
					listClinic.SelectedIndex=i+1;
				}
			}
		}

		private void listClinic_DoubleClick(object sender,EventArgs e) {
			if(listClinic.SelectedIndex<0) {
				return;
			}
			ClinicNum=0;
			if(listClinic.SelectedIndex>0) {
				ClinicNum=_listClinics[listClinic.SelectedIndex-1].ClinicNum;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			ClinicNum=0;
			if(listClinic.SelectedIndex>0) {
				ClinicNum=_listClinics[listClinic.SelectedIndex-1].ClinicNum;
			}
			DialogResult=DialogResult.OK;
		}
	}
}