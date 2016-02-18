using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenDentBusiness;
using System.Drawing.Printing;
using System.Drawing;

namespace OpenDentalGraph {
	public partial class FormPrintSettings:Form {
		private Chart _chartCur;
		private bool _isLoading=true;

		public FormPrintSettings(Chart chart) {
			_chartCur=chart;
			InitializeComponent();
		}

		private void FormPrintSettings_Load(object sender,EventArgs e) {
			_chartCur.Printing.PrintDocument = new PrintDocument();
			_chartCur.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(ChartGenericFormat_PrintPage);
			_chartCur.Printing.PrintDocument.OriginAtMargins=true;
			//Default to more or less full screen landscape.
			textWidth.Text="900";
			textHeight.Text="600";
			textMarginHeight.Text="0";
			textMarginWidth.Text="0";
			textXPos.Text="100";
			textYPos.Text="100";
			_isLoading=false;
			MakePage();
		}

		private void MakePage() {
			_chartCur.Printing.PrintDocument.DefaultPageSettings.Landscape = checkLandscape.Checked;
			_chartCur.Printing.PrintDocument.DefaultPageSettings.Margins=new System.Drawing.Printing.Margins(PIn.Int(textMarginWidth.Text),PIn.Int(textMarginWidth.Text),PIn.Int(textMarginHeight.Text),PIn.Int(textMarginHeight.Text));
			printPreviewControl.Document=_chartCur.Printing.PrintDocument;
		}

		private void ChartGenericFormat_PrintPage(object sender,PrintPageEventArgs ev) {
			Rectangle chartPosition=new Rectangle(PIn.Int(textXPos.Text),PIn.Int(textYPos.Text), PIn.Int(textWidth.Text), PIn.Int(textHeight.Text));
			_chartCur.Printing.PrintPaint(ev.Graphics,chartPosition);
		}

		private void butExport_Click(object sender,EventArgs e) {
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
			_chartCur.Size=new Size(PIn.Int(textWidth.Text),PIn.Int(textHeight.Text));
			_chartCur.SaveImage(sd.FileName,format);
			_chartCur.Size=sizeOld;
			_chartCur.Dock=dockOld;
			MessageBox.Show(Lans.g(this,"Chart saved."));
		}

		private void butPrint_Click(object sender,EventArgs e) {
			_chartCur.Printing.Print(true);
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void refresh_Event(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			timer1.Stop();
			timer1.Start();
		}
		
		private void checkLandscape_CheckedChanged(object sender,EventArgs e) {
			MakePage();
		}

		private void timer1_Tick(object sender,EventArgs e) {
			timer1.Stop();
			MakePage();
		}
	}
}
