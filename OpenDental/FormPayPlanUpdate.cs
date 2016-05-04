using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.IO;
using OpenDental.ReportingComplex;
using System.Drawing.Printing;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Pdf;
using OpenDental.UI;
using System.Linq;
using System.Diagnostics;

namespace OpenDental {
	public partial class FormPayPlanUpdate:ODForm {

		public FormPayPlanUpdate() {
			InitializeComponent();
			Lan.F(this);
		}

		///<summary>Constructs the pdf and formats it for the Payment Plan Conversion Changelog pdf report.</summary>
		private MigraDoc.DocumentObjectModel.Document CreateDocument(PrintDocument pd, DataTable table) {
			MigraDoc.DocumentObjectModel.Document doc= new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch((double)pd.DefaultPageSettings.PaperSize.Width/100);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch((double)pd.DefaultPageSettings.PaperSize.Height/100);
			doc.DefaultPageSetup.TopMargin=Unit.FromInch((double)pd.DefaultPageSettings.Margins.Top/100);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch((double)pd.DefaultPageSettings.Margins.Left/100);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch((double)pd.DefaultPageSettings.Margins.Right/100);
			doc.DefaultPageSetup.BottomMargin=Unit.FromInch((double)pd.DefaultPageSettings.Margins.Bottom/100);
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			string text;
			MigraDoc.DocumentObjectModel.Font font;
			//HEADING-----------------------------------------------------------------------------------------------------------
			#region Heading
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Center;
			par.Format=parformat;
			font=MigraDocHelper.CreateFont(14,true);
			text=Lan.g(this,"Payment Plan Update Changelog");
			par.AddFormattedText(text,font);
			text=DateTime.Today.ToShortDateString();
			font=MigraDocHelper.CreateFont(10);
			par.AddLineBreak();
			#endregion Heading
			ODGridColumn gcol;
			ODGridRow grow;
			ODGrid gridPP = new ODGrid();
			this.Controls.Add(gridPP);
			gridPP.BeginUpdate();
			gridPP.Columns.Clear();
			gcol=new ODGridColumn(Lan.g(this,"PatNum"),50);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"Patient"),150);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"Guarantor"),150);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"Birthdate"),80);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"Old Balance"),80);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"New Balance"),80);
			gridPP.Columns.Add(gcol);
			gcol=new ODGridColumn(Lan.g(this,"Amount Changed"),80);
			gridPP.Columns.Add(gcol);
			gridPP.Width=gridPP.WidthAllColumns+20;
			gridPP.EndUpdate();
			//We currently show payment plan breakdowns on all statements, receipts, and invoices.
			if(table.Rows.Count>0) {
				MigraDocHelper.InsertSpacer(section,5);
				gridPP.BeginUpdate();
				gridPP.Rows.Clear();
				for(int p = 0;p<table.Rows.Count;p++) {
					grow=new ODGridRow();
					grow.Cells.Add(table.Rows[p]["PatNum"].ToString());
					grow.Cells.Add(table.Rows[p]["Patient"].ToString());
					grow.Cells.Add(table.Rows[p]["Guarantor"].ToString());
					grow.Cells.Add((PIn.Date(table.Rows[p]["Birthdate"].ToString())).ToShortDateString());
					grow.Cells.Add(PIn.Double(table.Rows[p]["PatBalOld"].ToString()).ToString("c"));
					grow.Cells.Add(PIn.Double(table.Rows[p]["PatBalNew"].ToString()).ToString("c"));
					grow.Cells.Add(PIn.Double(table.Rows[p]["PatBalChanged"].ToString()).ToString("c"));
					gridPP.Rows.Add(grow);
				}
				gridPP.EndUpdate();
				MigraDocHelper.DrawGrid(section,gridPP);
				MigraDocHelper.InsertSpacer(section,5);
				par=section.AddParagraph();
				par.Format.Font=MigraDocHelper.CreateFont(10,true);
				par.Format.Alignment=ParagraphAlignment.Right;
				par.Format.RightIndent=Unit.FromInch(0.5);
				double oldBalance=0;
				double newBalance=0;
				double changeBalance=0;
				int rows=0;
				for(int m = 0;m<table.Rows.Count;m++) {
					oldBalance+=PIn.Double(table.Rows[m]["PatBalOld"].ToString());
					newBalance+=PIn.Double(table.Rows[m]["PatBalNew"].ToString());
					changeBalance+=PIn.Double(table.Rows[m]["PatBalChanged"].ToString());
					rows++;
				}
				par.AddText(Lan.g(this,"Old Balance Total:")+"    ");
				par.AddText(oldBalance.ToString("c"));
				par.AddLineBreak();
				par.AddText(Lan.g(this,"New Balance Total:")+"    ");
				par.AddText(newBalance.ToString("c"));
				par.AddLineBreak();
				par.AddText("———————————————");
				par.AddLineBreak();
				par.AddText(Lan.g(this,"Total Change in Balance:")+"    ");
				par.AddText((Math.Abs(changeBalance)).ToString("c"));
				par.AddLineBreak();
				par.AddLineBreak();
				par.AddText(Lan.g(this,"Total Patients Affected:")+"    ");
				par.AddText(rows.ToString());
			}
			return doc;
		}

		///<summary>Saves the a pdf version of the Payment Plan Conversion Changelog. 
		///Attempts to save it to the default export path first, otherwise saves it in the AtoZ folder in the Reports folder.  
		///If it fails (most likely a permissions issue), it will prompt the user to save it to another location.  
		///If they click cancel, it will bring up the report in their default pdf viewer so they can print or save the report if need be.
		///This method is recursive (it will call itself until it succeeds), but if the User clicks cancel when the SaveDialog comes up, they will fall out of this method.</summary>
		private void SavePDF(DataTable tableBalances, bool ManualSave=false) {
			PrintDocument pd=new PrintDocument();
			pd.DefaultPageSettings.Margins=new Margins(40,40,40,60);
			if(pd.DefaultPageSettings.PrintableArea.Height<1000
				|| pd.DefaultPageSettings.PrintableArea.Width<750) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			MigraDoc.DocumentObjectModel.Document doc=CreateDocument(pd,tableBalances);
			MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
			pdfRenderer.Document=doc;
			pdfRenderer.RenderDocument();
			//construct save path
			string savePath;
			if(ManualSave) {
				SaveFileDialog saveFileDialog2=new SaveFileDialog();
				saveFileDialog2.AddExtension=true;
				saveFileDialog2.FileName="Payment Plan Update Log.pdf";
				saveFileDialog2.Filter="*PDFs(*.pdf)|.pdf|All files(*.*)|*.*";
				saveFileDialog2.FilterIndex=0;
				if(saveFileDialog2.ShowDialog()!=DialogResult.OK) {
					savePath=PrefL.GetTempFolderPath()+"Payplan Update Changelog.pdf";
					pdfRenderer.PdfDocument.Save(savePath);
					Process.Start(savePath);
					MsgBox.Show(this,"This document reflects the changes that were made when your Payment Plans were updated.  " 
						+"It is important that you print or save this document for future reference.");
					return;
				}
				savePath=saveFileDialog2.FileName;
			}
			else {
				if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
					savePath=PrefC.GetString(PrefName.ExportPath)+"Payplan Update Changelog.pdf";
				}
				else {
					savePath=CodeBase.ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Reports\\Payplan Update Changelog.pdf");
				}
			}
			try {
				pdfRenderer.PdfDocument.Save(savePath);
				string message=Lan.g(this,"The Payment Plan Changelog Report has been stored in")+" "+savePath;
				MessageBox.Show(message);
			}
			catch (Exception e){
				string error=Lan.g(this,"There was an error saving the Payment Plan Changelog Report:")
					+"\r\n"+e.Message
					+"\r\n"+Lan.g(this,"Please choose a different location to save this report.");
				MessageBox.Show(error);
				SavePDF(tableBalances,true);
			}
		}

		private void butUpdate_Click(object sender,EventArgs e) {
			List<Patient> listBalOld;
			List<Patient> listBalNew;
			DataTable tableBalChanged;
			Cursor=Cursors.WaitCursor;
			//Run Aging and get data for patients' Old Balances
			//update preference.
			//Run Aging again and get data for patients' new Balances
			//store all data in datatable and use that to calculate report.
			Ledgers.RunAging();
			listBalOld=Patients.GetAllPatients();
			PayPlans.PayPlanUpdate(2);
			Ledgers.RunAging();
			listBalNew=Patients.GetAllPatients();
			tableBalChanged=new DataTable();
			tableBalChanged.Columns.Add("PatNum");
			tableBalChanged.Columns.Add("Patient");
			tableBalChanged.Columns.Add("Guarantor");
			tableBalChanged.Columns.Add("Birthdate");
			tableBalChanged.Columns.Add("PatBalOld");
			tableBalChanged.Columns.Add("PatBalNew");
			tableBalChanged.Columns.Add("PatBalChanged");
			foreach(Patient oldPatCur in listBalOld) {
				Patient newPatCur=listBalNew.Where(x => x.PatNum == oldPatCur.PatNum).First();
				if(oldPatCur.BalTotal == newPatCur.BalTotal) {
					continue;
				}
				Patient patGuar=Patients.GetPat(oldPatCur.Guarantor);
				DataRow row=tableBalChanged.NewRow();
				row["PatNum"]=oldPatCur.PatNum;
				row["Patient"]=oldPatCur.LName+", "+oldPatCur.FName;
				row["Guarantor"]=patGuar.LName+", "+patGuar.FName;
				row["Birthdate"]=oldPatCur.Birthdate;
				row["PatBalOld"]=oldPatCur.BalTotal;
				row["PatBalNew"]=newPatCur.BalTotal;
				row["PatBalChanged"]=Math.Abs(oldPatCur.BalTotal - newPatCur.BalTotal);
				tableBalChanged.Rows.Add(row);
			}
			SavePDF(tableBalChanged);
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
		}

		private void butLater_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}