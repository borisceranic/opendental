using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormTest:Form {
		public FormTest(Control control) {
			InitializeComponent();
			this.Controls.Add(control);
			control.SetBounds(0,0,this.ClientSize.Width,this.ClientSize.Height);
			control.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);//Allows resizing the control in this window.
		}
	}
}
