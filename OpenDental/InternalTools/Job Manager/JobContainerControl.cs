using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class JobContainerControl:DraggableControl {

		public Button butLeft;
		public Button butRight;
		public Button butMerge;

		public JobContainerControl() {
			InitializeComponent();
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Dispose();
		}

		private void JobContainerControl_MouseDown(object sender,MouseEventArgs e) {
			this.DoDragDrop(this,DragDropEffects.Move);
		}

	}
}
