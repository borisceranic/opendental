using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenDentBusiness;
using System.Drawing.Printing;
using System.Drawing;

namespace OpenDentalGraph {
	public partial class FormPrintSettings:Form {
		private Chart _chartCur;
		public FormPrintSettings(Chart chart) {
			_chartCur=chart;
			InitializeComponent();
		}

		private void FormPrintSettings_Load(object sender,EventArgs e) {
			//default to the chart's current dimensions
			textWidth.Text=_chartCur.Width.ToString();
			textHeight.Text=_chartCur.Height.ToString();
			//default to 1".
			textMarginHeight.Text="100";
			textMarginWidth.Text="100";
			//default to top, left corner.
			textXPos.Text="0";
			textYPos.Text="0";
		}

		private void MakePage() {
			_chartCur.Printing.PrintDocument = new PrintDocument();
			_chartCur.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(ChartGenericFormat_PrintPage);
			_chartCur.Printing.PrintDocument.DefaultPageSettings.Landscape = checkLandscape.Checked;
			_chartCur.Printing.PrintDocument.OriginAtMargins=true;
			_chartCur.Printing.PrintDocument.DefaultPageSettings.Margins=new System.Drawing.Printing.Margins(PIn.Int(textMarginWidth.Text),PIn.Int(textMarginWidth.Text),PIn.Int(textMarginHeight.Text),PIn.Int(textMarginHeight.Text));

		}

		private void ChartGenericFormat_PrintPage(object sender,PrintPageEventArgs ev) {
			Rectangle chartPosition=new Rectangle(PIn.Int(textXPos.Text),PIn.Int(textYPos.Text), PIn.Int(textWidth.Text), PIn.Int(textHeight.Text));
			_chartCur.Printing.PrintPaint(ev.Graphics,chartPosition);
		}

		private void butExport_Click(object sender,EventArgs e) {
			//MakePage(); //doesn't currently use the size, position, and margins entered.
			SaveFileDialog sd=new SaveFileDialog() {
				Filter="Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif",
				FilterIndex=2,
				RestoreDirectory=true,
			};
			if(sd.ShowDialog()!=DialogResult.OK) {
				return;
			}
			ChartImageFormat format = ChartImageFormat.Bmp;
			if(sd.FileName.EndsWith("bmp")) {
				format=ChartImageFormat.Bmp;
			}
			else if(sd.FileName.EndsWith("jpg")) {
				format=ChartImageFormat.Jpeg;
			}
			else if(sd.FileName.EndsWith("emf")) {
				format=ChartImageFormat.Emf;
			}
			else if(sd.FileName.EndsWith("gif")) {
				format=ChartImageFormat.Gif;
			}
			else if(sd.FileName.EndsWith("png")) {
				format=ChartImageFormat.Png;
			}
			else if(sd.FileName.EndsWith("tif")) {
				format=ChartImageFormat.Tiff;
			}
			Size sizeOld=_chartCur.Size;
			DockStyle dockOld=_chartCur.Dock;
			_chartCur.Dock=DockStyle.None;
			_chartCur.Size= new Size(PIn.Int(textWidth.Text),PIn.Int(textHeight.Text));
			_chartCur.SaveImage(sd.FileName,format);
			_chartCur.Size=sizeOld;
			_chartCur.Dock=dockOld;
			MessageBox.Show(Lans.g(this,"Chart saved."));
		}

		private void butPrintPreview_Click(object sender,EventArgs e) {
			MakePage();
			_chartCur.Printing.PrintPreview();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			MakePage();
			_chartCur.Printing.Print(true);
		}

	}
}
