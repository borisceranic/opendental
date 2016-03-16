using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph {
	public partial class BaseGraphOptionsCtrl:UserControl {
		public event EventHandler InputsChanged;
		public BaseGraphOptionsCtrl() {
			InitializeComponent();			
		}
		
		protected void OnBaseInputsChanged(object sender,EventArgs e) {
			if(InputsChanged!=null) {
				InputsChanged(this,new EventArgs());
			}
		}

		public virtual int GetPanelHeight() {
			return 63;
		}

		public virtual bool HasGroupOptions {
			get {
				return true;
			}
		}
	}
}
