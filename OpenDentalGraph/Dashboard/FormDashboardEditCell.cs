using System;
using System.Windows.Forms;

namespace OpenDentalGraph {
	public partial class FormDashboardEditCell:Form {
		private IODGraphPrinter _printer=null;
		public FormDashboardEditCell(Control dockControl,bool allowSave) {
			InitializeComponent();
			if(dockControl is IHasODGraphPrinter) {
				_printer=((IHasODGraphPrinter)dockControl).GetPrinter();
			}
			if(dockControl is IDashboardDockContainer) {
				this.Text="Edit Cell - "+((IDashboardDockContainer)dockControl).GetCellType().ToString();
			}
			splitContainer.Panel1.Controls.Add(dockControl);
			butOk.Enabled=allowSave;
			if(!allowSave) {
				this.Text=this.Text+ " (Changes Will Not Be Saved)";
			}
			panelPrint.Enabled=_printer!=null && _printer.CanPrint();
		}

		private void butSaveImage_Click(object sender,EventArgs e) {
			if(_printer!=null) {
				_printer.SaveImage();
			}
		}

		private void butPageSetup_Click(object sender,EventArgs e) {
			if(_printer!=null) {
				_printer.PrintPageSetup();
			}
		}

		private void butPrintPreview_Click(object sender,EventArgs e) {
			if(_printer!=null) {
				_printer.PrintPreview();
			}
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(_printer!=null) {
				_printer.Print();
			}
		}
	}
}
