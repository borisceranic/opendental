using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAmountEdit:Form {
		public double Amount;

		public FormAmountEdit(string text) {
			InitializeComponent();
			labelText.Text=text;
			Lan.F(this);
		}

		private void FormAmountEdit_Load(object sender,EventArgs e) {
			textAmount.Text=POut.Double(Amount);
			textAmount.SelectionStart=0;
			textAmount.SelectionLength=textAmount.Text.Length;
		}

		private void butOK_Click(object sender,EventArgs e) {
			Amount=PIn.Double(textAmount.Text);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}