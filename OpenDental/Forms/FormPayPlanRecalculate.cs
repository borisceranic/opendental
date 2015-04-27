using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPayPlanRecalculate:Form {

		public bool isPrepay=true;
		public bool isRecalculateInterest=true;

		public FormPayPlanRecalculate() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPayPlanRecalculate_Load(object sender,EventArgs e) {
			radioPrepay.Checked=isPrepay;
			checkRecalculateInterest.Checked=isRecalculateInterest;
		}

		private void butOK_Click(object sender,EventArgs e) {
			isPrepay=radioPrepay.Checked;
			isRecalculateInterest=checkRecalculateInterest.Checked;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}