using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class ScreenToothChart:UserControl {
		private string _toothValues;
		///<summary>Returns a list of tooth controls, listed from left to right, then right to left taking first top, then bottom as you are looking at the tooth chart.</summary>
		public List<UserControlScreenTooth> GetTeeth {
			get {
				return new List<UserControlScreenTooth>() {controlTooth2,controlTooth3,controlTooth4,controlTooth5,controlTooth12,controlTooth13,controlTooth14,
					controlTooth15,controlTooth18,controlTooth19,controlTooth20,controlTooth21,controlTooth28,controlTooth29,controlTooth30,controlTooth31 };
			}
		}

		public ScreenToothChart(string toothValues) {
			InitializeComponent();
			_toothValues=toothValues;
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
		}

		private void ScreenToothChart_Load(object sender,EventArgs e) {
			string[] teethValues=_toothValues.Split(';');
			controlTooth2.SetSelected(teethValues[0].Split(','));
			controlTooth3.SetSelected(teethValues[1].Split(','));
			controlTooth4.SetSelected(teethValues[2].Split(','));
			controlTooth5.SetSelected(teethValues[3].Split(','));
			controlTooth12.SetSelected(teethValues[4].Split(','));
			controlTooth13.SetSelected(teethValues[5].Split(','));
			controlTooth14.SetSelected(teethValues[6].Split(','));
			controlTooth15.SetSelected(teethValues[7].Split(','));
			controlTooth18.SetSelected(teethValues[8].Split(','));
			controlTooth19.SetSelected(teethValues[9].Split(','));
			controlTooth20.SetSelected(teethValues[10].Split(','));
			controlTooth21.SetSelected(teethValues[11].Split(','));
			controlTooth28.SetSelected(teethValues[12].Split(','));
			controlTooth29.SetSelected(teethValues[13].Split(','));
			controlTooth30.SetSelected(teethValues[14].Split(','));
			controlTooth31.SetSelected(teethValues[15].Split(','));
			Invalidate();
		}

	}
}
