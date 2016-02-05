using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph {
	public partial class BrokenApptGraphOptionsCtrl:BaseGraphOptionsCtrl {
		public enum Grouping { proc, adjustment, apptStatus };
		public Grouping CurGrouping {
			get {
				if(radioButton2.Checked) {
					return Grouping.adjustment;
				}
				else if(radioButton3.Checked) {
					return Grouping.apptStatus;
				}
				return Grouping.proc;				
			}
			set {
				switch(value) {
					case Grouping.proc:
						radioButton1.Checked=true;
						break;
					case Grouping.adjustment:
						radioButton2.Checked=true;
						break;
					case Grouping.apptStatus:
						radioButton3.Checked=true;
						break;
				}
			}
		}

		public BrokenApptGraphOptionsCtrl() {
			InitializeComponent();
		}


		private void radioChanged(object sender,EventArgs e) {
			if((sender is RadioButton) && !((RadioButton)sender).Checked) {
				return;
			}
			OnBaseInputsChanged(sender,e);
		}
	}
}
