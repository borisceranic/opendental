using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class ODForm:Form {

		public ODForm() {
			InitializeComponent();
			Lan.F(this);
		}

		private void ODForm_KeyUp(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.F1) {
				try {
					OpenDentalHelp.ODHelp.ShowHelp(((Form)sender).Name);
				}
				catch(Exception ex) {
					MessageBox.Show(this,ex.Message);
				}
			}
		}
	}
}