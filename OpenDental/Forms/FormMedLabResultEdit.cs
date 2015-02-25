using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMedLabResultEdit:Form {
		public MedLab MedLabCur;

		public FormMedLabResultEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabResultEdit_Load(object sender,EventArgs e) {
			//FillGrid();
		}

		private void butPatSelect_Click(object sender,EventArgs e) {

		}

		private void butShowHL7_Click(object sender,EventArgs e) {

		}

		private void butDelete_Click(object sender,EventArgs e) {

		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}