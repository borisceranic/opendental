﻿using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace OpenDental {
	public class SheetPrinting {
		///<summary>If there is only one sheet, then this will stay 0.</Summary>
		private static int _sheetsPrinted;
		///<summary>Pages printed on current sheet.</summary>
		private static int _pagesPrinted;
		///<summary>Used for determining page breaks. When moving to next page, use this Y value to determine the next field to print.</summary>
		private static int _yPosPrint;
		///<summary>Print margin of the default printer. only used in page break calulations, and only top and bottom are used.</summary>
		private static Margins _printMargin=new Margins(0,0,40,60);
		///<summary>If not a batch, then there will just be one sheet in the list.</summary>
		private static List<Sheet> _sheetList;
		///<summary>Used to force old single page behavior. Used for labels.</summary>
		//private static bool _forceSinglePage;
		private static bool _printCalibration=false;//debug only
		private static bool _isPrinting=false;
		private static Statement _stmt;

		///<summary>The treatment finder needs to be able to clear out the pages printed variable before it prints a batch.</summary>
		public static int PagesPrinted {
			get {
				return _pagesPrinted;
			}
			set {
				_pagesPrinted=value;
			}
		}

		///<summary>The treatment finder needs this so that it can use the same Margins in its page calculations.</summary>
		public static Margins PrintMargin {
			get {
				return _printMargin;
			}
		}


		public static void PrintStatement(object parameters) {
			List<object> listParams=(List<object>)parameters;
			SheetDef sheetDef=(SheetDef)listParams[0];
			Statement stmt=(Statement)listParams[1];
			string filePath=(string)listParams[2];
			try {
				ProcessStartInfo info=new ProcessStartInfo();
				info.Verb="print";
				info.FileName=filePath;
				info.CreateNoWindow=true;
				info.WindowStyle=ProcessWindowStyle.Hidden;
				Process p=new Process();
				p.StartInfo=info;
				p.Start();
				p.WaitForInputIdle();
				System.Threading.Thread.Sleep(3000);
				if(p.CloseMainWindow()==false) {
					p.Kill();
				}
			}
			catch(Exception ex) {
				//Must rest sheet, as PDF printing modifies fields.
				Sheet sheet=SheetUtil.CreateSheet(sheetDef,stmt.PatNum,stmt.HidePayment);
				SheetFiller.FillFields(sheet,stmt);
				SheetUtil.CalculateHeights(sheet,Graphics.FromImage(new Bitmap(sheet.HeightPage,sheet.WidthPage)),stmt);
				SheetPrinting.Print(sheet,1,false,stmt);//use GDI+ printing, which is slightly different than the pdf.
			}
		}

		///<summary>Surround with try/catch.</summary>
		public static void PrintBatch(List<Sheet> sheetBatch,Statement stmt=null){
			//currently no validation for parameters in a batch because of the way it was created.
			//could validate field names here later.
			_sheetList=sheetBatch;
			_sheetsPrinted=0;
			_pagesPrinted=0;
			_yPosPrint=0;
			PrintDocument pd=new PrintDocument();
			pd.OriginAtMargins=true;
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			if(sheetBatch[0].Width>0 && sheetBatch[0].Height>0){
				pd.DefaultPageSettings.PaperSize=new PaperSize("Default",sheetBatch[0].Width,sheetBatch[0].Height);
			}
			PrintSituation sit=PrintSituation.Default;
			pd.DefaultPageSettings.Landscape=sheetBatch[0].IsLandscape;
			switch(sheetBatch[0].SheetType){
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelReferral:
					sit=PrintSituation.LabelSingle;
					break;
				case SheetTypeEnum.ReferralSlip:
					sit=PrintSituation.Default;
					break;
			}
			//later: add a check here for print preview.
			#if DEBUG
				pd.DefaultPageSettings.Margins=new Margins(20,20,0,0);
				int pageCount=0;
				foreach(Sheet s in _sheetList) {
					//SetForceSinglePage(s);
					SheetUtil.CalculateHeights(s,Graphics.FromImage(new Bitmap(s.WidthPage,s.HeightPage)),stmt,_isPrinting,_printMargin.Top,_printMargin.Bottom);
					pageCount+=Sheets.CalculatePageCount(s,_printMargin);//(_forceSinglePage?1:Sheets.CalculatePageCount(s,_printMargin));
				}
				FormPrintPreview printPreview=new FormPrintPreview(sit,pd,pageCount,0,"Batch of "+sheetBatch[0].Description+" printed");
				printPreview.ShowDialog();
			#else
				try {
					foreach(Sheet s in _sheetList) {
						s.SheetFields.Sort(OpenDentBusiness.SheetFields.SortDrawingOrderLayers);
					}
					if(!PrinterL.SetPrinter(pd,sit,0,"Batch of "+sheetBatch[0].Description+" printed")) {
						return;
					}
					pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
					pd.Print();
				}
				catch(Exception ex){
					throw ex;
					//MessageBox.Show(Lan.g("Sheet","Printer not available"));
				}
			#endif
		}

		public static void PrintRx(Sheet sheet,bool isControlled){
			Print(sheet,1,isControlled);
		}

		public static void SetZero() {
			_sheetsPrinted=0;
			_yPosPrint=0;
		}

		///<Summary></Summary>
		public static void Print(Sheet sheet,int copies=1,bool isRxControlled=false,Statement stmt=null){
			//parameter null check moved to SheetFiller.
			//could validate field names here later.
			_stmt=stmt;
			_isPrinting=true;
			_sheetList=new List<Sheet>();
			for(int i=0;i<copies;i++){
				_sheetList.Add(sheet.Copy());
			}
			_sheetsPrinted=0;
			_yPosPrint=0;// _printMargin.Top;
			PrintDocument pd=new PrintDocument();
			pd.OriginAtMargins=true;
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			if(pd.DefaultPageSettings.PrintableArea.Width==0) {
				//prevents bug in some printers that do not specify paper size
				pd.DefaultPageSettings.PaperSize=new PaperSize("paper",850,1100);
			}
			if(sheet.SheetType==SheetTypeEnum.LabelPatient
				|| sheet.SheetType==SheetTypeEnum.LabelCarrier
				|| sheet.SheetType==SheetTypeEnum.LabelAppointment
				|| sheet.SheetType==SheetTypeEnum.LabelReferral) 
			{//I think this causes problems for non-label sheet types.
				if(sheet.Width>0 && sheet.Height>0) {
					pd.DefaultPageSettings.PaperSize=new PaperSize("Default",sheet.Width,sheet.Height);
				}
			}
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd.OriginAtMargins=true;
			//SetForceSinglePage(sheet);
			PrintSituation sit=PrintSituation.Default;
			pd.DefaultPageSettings.Landscape=sheet.IsLandscape;
			switch(sheet.SheetType){
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelReferral:
				case SheetTypeEnum.LabelAppointment:
					sit=PrintSituation.LabelSingle;
					break;
				case SheetTypeEnum.ReferralSlip:
					sit=PrintSituation.Default;
					break;
				case SheetTypeEnum.Rx:
					if(isRxControlled){
						sit=PrintSituation.RxControlled;
					}
					else{
						sit=PrintSituation.Rx;
					}
					break;
			}
			//later: add a check here for print preview.
			#if DEBUG
				//pd.DefaultPageSettings.Margins=new Margins(0,0,40,60);
				//pd.OriginAtMargins=true;
				//pd.DefaultPageSettings.PaperSize=new PaperSize("Default",sheet.Width,sheet.Height-(100));
				FormPrintPreview printPreview;
				int pageCount=0;
				foreach(Sheet s in _sheetList) {
					//SetForceSinglePage(s);
					pageCount+=Sheets.CalculatePageCount(s,_printMargin);// (_forceSinglePage?1:Sheets.CalculatePageCount(s,_printMargin));
				}
				printPreview=new FormPrintPreview(sit,pd,pageCount,sheet.PatNum,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed");
				printPreview.ShowDialog();
			#else
				try {
					if(sheet.PatNum!=null){
						if(!PrinterL.SetPrinter(pd,sit,sheet.PatNum,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed")) {
							return;
						}
					}
					else{
						if(!PrinterL.SetPrinter(pd,sit,0,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed")) {
							return;
						}
					}
					pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
					pd.Print();
				}
				catch(Exception ex){
					throw ex;
					//MessageBox.Show(Lan.g("Sheet","Printer not available"));
				}
			#endif
			_isPrinting=false;
			GC.Collect();//We are done with printing so we can forcefully clean up all the objects and controls that were used in printing.
		}

		///<summary>This gets called for every page to be printed when sending to a printer.  Will stop printing when e.HasMorePages==false.  See also CreatePdfPage.</summary>
		private static void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Graphics g=e.Graphics;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;//Necessary for very large images that need to be scaled down.
			Sheet sheet=_sheetList[_sheetsPrinted];
			Sheets.SetPageMargin(sheet,_printMargin);
			SheetUtil.CalculateHeights(sheet,g,_stmt,_isPrinting,_printMargin.Top,_printMargin.Bottom);//this is here because of easy access to g.
			sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
			//Begin drawing.
			foreach(SheetField field in sheet.SheetFields) {
				if(!fieldOnCurPageHelper(field,sheet,_printMargin,_yPosPrint)) { 
					continue; 
				}
				switch(field.FieldType) {
					case SheetFieldType.Image:
					case SheetFieldType.PatImage:
						try {
							drawFieldImage(field,g,null);
						}
						catch(OutOfMemoryException ex) {
							//Cancel the print job because there is a static image on this sheet which is to big for the printer to handle.
							MessageBox.Show(ex.Message);//Custom message that is already translated.
							e.Cancel=true;
							return;
						}
						break;
					case SheetFieldType.Drawing:
						drawFieldDrawing(field,g,null);
						break;
					case SheetFieldType.Rectangle:
						drawFieldRectangle(field,g,null);
						break;
					case SheetFieldType.Line:
						drawFieldLine(field,g,null);
						break;
					case SheetFieldType.Grid:
						drawFieldGrid(field,sheet,g,null,_stmt);
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
					case SheetFieldType.StaticText:
						drawFieldText(field,sheet,g,null);
						break;
					case SheetFieldType.CheckBox:
						drawFieldCheckBox(field,g,null);
						break;
					case SheetFieldType.SigBox:
						drawFieldSigBox(field,sheet,g,null);
						break;
					default:
						//Parameter or possibly new field type.
						break;
				}
			}//end foreach SheetField
			drawHeader(sheet,g,null);
			drawFooter(sheet,g,null);
			#if DEBUG
				if(_printCalibration) {
					drawCalibration(sheet,g,e,null,null);
				}
			#endif
			g.Dispose();
			g=null;
			#region Set variables for next page to be printed
			_yPosPrint+=sheet.HeightPage-(_printMargin.Bottom+_printMargin.Top);//move _yPosPrint down equal to the amount of printable area per page.
			_pagesPrinted++;
			if(_pagesPrinted<Sheets.CalculatePageCount(sheet,_printMargin)) {
				e.HasMorePages=true;
			}
			else {//we are printing the last page of the current sheet.
				_yPosPrint=0;// _printMargin.Top;
				_pagesPrinted=0;
				_sheetsPrinted++;
				if(_sheetsPrinted<_sheetList.Count){
					e.HasMorePages=true;
				}
				else{
					e.HasMorePages=false;
					_sheetsPrinted=0;
				}
			}
			#endregion
		}

		private static bool fieldOnCurPageHelper(SheetField field,Sheet sheet,Margins _printMargin,int _yPosPrint) {
			//Even though _printMargins and _yPosPrint are available in this context they are passed in so for future compatibility with webforms.
			if(field.YPos>(_yPosPrint+sheet.HeightPage)){
				return false;//field is entirely on one of the next pages.
			}
			if(field.Bounds.Bottom<_yPosPrint && _pagesPrinted>0) {
				return false;//field is entirely on one of the previous pages. Unless we are on the first page, then it is in the top margin.
			}
			return true;//field is all or partially on current page.
		}

		#region Drawing Helpers. One for almost every field type. =====================================================================================
		///<summary>Public accessor to the draw image function</summary>
		public static void drawImageHelper(SheetField field,Graphics g) {
			drawFieldImage(field,g,null);
		}

		///<summary>Draws the image to the graphics object passed in.  Can throw an OutOfMemoryException when printing that will have a message that should be displayed and the print job should be cancelled.</summary>
		public static void drawFieldImage(SheetField field,Graphics g, XGraphics gx) {
			Bitmap bmpOriginal=null;
			string filePathAndName="";
			#region Get the path for the image
			switch(field.FieldType) {
				case SheetFieldType.Image:
					filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),field.FieldName);
					break;
				case SheetFieldType.PatImage:
					if(field.FieldValue=="") {
						//There is no document object to use for display, but there may be a baked in image and that situation is dealt with below.
						filePathAndName="";
						break;
					}
					Document patDoc=Documents.GetByNum(PIn.Long(field.FieldValue));
					List<string> paths=Documents.GetPaths(new List<long> { patDoc.DocNum },ImageStore.GetPreferredAtoZpath());
					if(paths.Count < 1) {//No path was found so we cannot draw the image.
						return;
					}
					filePathAndName=paths[0];
					break;
				default:
					//not an image field
					return;
			}
			#endregion
			#region Load the image into bmpOriginal
			if(field.FieldName=="Patient Info.gif") {
				bmpOriginal=OpenDentBusiness.Properties.Resources.Patient_Info;
			}
			else if(File.Exists(filePathAndName)) {
				bmpOriginal=new Bitmap(filePathAndName);
			}
			else {
				return;
			}
			#endregion
			#region Calculate the image ratio and location, set values for imgDrawWidth and imgDrawHeight
			//inscribe image in field while maintaining aspect ratio.
			float imgRatio=(float)bmpOriginal.Width/(float)bmpOriginal.Height;
			float fieldRatio=(float)field.Width/(float)field.Height;
			float imgDrawHeight=field.Height;//drawn size of image
			float imgDrawWidth=field.Width;//drawn size of image
			int adjustY=0;//added to YPos
			int adjustX=0;//added to XPos
			//For patient images, we need to make sure the images will fit and can maintain aspect ratio.
			if(field.FieldType==SheetFieldType.PatImage && imgRatio>fieldRatio) {//image is too wide
				//X pos and width of field remain unchanged
				//Y pos and height must change
				imgDrawHeight=(float)bmpOriginal.Height*((float)field.Width/(float)bmpOriginal.Width);//img.Height*(width based scale) This also handles images that are too small.
				adjustY=(int)((field.Height-imgDrawHeight)/2f);//adjustY= half of the unused vertical field space
			}
			else if(field.FieldType==SheetFieldType.PatImage && imgRatio<fieldRatio) {//image is too tall
				//X pos and width must change
				//Y pos and height remain unchanged
				imgDrawWidth=(float)bmpOriginal.Width*((float)field.Height/(float)bmpOriginal.Height);//img.Height*(width based scale) This also handles images that are too small.
				adjustX=(int)((field.Width-imgDrawWidth)/2f);//adjustY= half of the unused horizontal field space
			}
			else {//image ratio == field ratio
				//do nothing
			}
			#endregion
			//We used to scale down bmpOriginal here to avoid memory exceptions.
			//Doing so was causing significant quality loss when printing or creating pdfs with very large images.
			if(gx==null) {
				try {
					//Always use the original BMP so that very large images can be scaled by the graphics class thus keeping a high quality image by using interpolation.
					g.DrawImage(bmpOriginal,
						new Rectangle(field.XPos+adjustX,field.YPos+adjustY-_yPosPrint,(int)imgDrawWidth,(int)imgDrawHeight),
						new Rectangle(0,0,bmpOriginal.Width,bmpOriginal.Height),
						GraphicsUnit.Pixel);
				}
				catch(OutOfMemoryException) {
					throw new OutOfMemoryException(Lan.g("Sheets","A static image on this sheet is too high in quality and cannot be printed.")+"\r\n"
						+Lan.g("Sheets","Try printing to a different printer or lower the quality of the static image")+":\r\n"
						+filePathAndName);
				}
			}
			else {
				MemoryStream ms=null;
				//For some reason PdfSharp's XImage cannot handle TIFF images.
				if(filePathAndName.ToLower().EndsWith(".tif") || filePathAndName.ToLower().EndsWith(".tiff")) {
					//Trick PdfSharp when we get a TIFF image into thinking it is a different image type.
					//Saving to BMP format will sometimes increase the file size dramatically.  E.g. an 11MB JPG turned into a 240MB BMP.
					//Instead of using BMP, we will use JPG which should have little to no quality loss and should be more compressed than BMP.
					ms=new MemoryStream();
					bmpOriginal.Save(ms,ImageFormat.Jpeg);
					bmpOriginal.Dispose();
					bmpOriginal=new Bitmap(ms);
				}
				XImage xI=XImage.FromGdiPlusImage(bmpOriginal);
				gx.DrawImage(xI,p(field.XPos+adjustX),p(field.YPos-_yPosPrint+adjustY),p(imgDrawWidth),p(imgDrawHeight));
				xI.Dispose();
				xI=null;
				if(ms!=null) {
					ms.Dispose();
					ms=null;
				}
			}
			if(bmpOriginal!=null) {
				bmpOriginal.Dispose();
				bmpOriginal=null;
			}
		}

		public static void drawFieldDrawing(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				Pen pen=new Pen(Brushes.Black,2f);
				List<Point> points=new List<Point>();
				string[] pairs=field.FieldValue.Split(new string[] { ";" },StringSplitOptions.RemoveEmptyEntries);
				foreach(string p in pairs) {
					points.Add(new Point(PIn.Int(p.Split(',')[0]),PIn.Int(p.Split(',')[1])));
				}
				for(int i=1;i<points.Count;i++) {
					g.DrawLine(pen,points[i-1].X,points[i-1].Y-_yPosPrint,points[i].X,points[i].Y-_yPosPrint);
				}
				pen.Dispose();
				pen=null;
			}
			else {
				XPen pen=new XPen(XColors.Black,p(2));
				List<Point> points=new List<Point>();
				string[] pairs=field.FieldValue.Split(new string[] { ";" },StringSplitOptions.RemoveEmptyEntries);
				foreach(string p2 in pairs) {
					points.Add(new Point(PIn.Int(p2.Split(',')[0]),PIn.Int(p2.Split(',')[1])));
				}
				for(int i=1;i<points.Count;i++) {
					gx.DrawLine(pen,p(points[i-1].X),p(points[i-1].Y-_yPosPrint),p(points[i].X),p(points[i].Y-_yPosPrint));
				}
				pen=null;
			}
		}

		public static void drawFieldRectangle(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				g.DrawRectangle(Pens.Black,field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
			}
			else {
				gx.DrawRectangle(XPens.Black,p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width),p(field.Height));
			}
		}

		public static void drawFieldLine(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				g.DrawLine((field.ItemColor==Color.FromArgb(0)?Pens.Black:new Pen(field.ItemColor,1)),
					field.XPos,field.YPos-_yPosPrint,
					field.XPos+field.Width,
					field.YPos-_yPosPrint+field.Height);
			}
			else {
				gx.DrawLine((field.ItemColor==Color.FromArgb(0)?XPens.Black:new XPen(field.ItemColor,1)),
					p(field.XPos),p(field.YPos-_yPosPrint),
					p(field.XPos+field.Width),
					p(field.YPos-_yPosPrint+field.Height));
			}
		}

		public static void drawFieldGrid(SheetField field,Sheet sheet,Graphics g,XGraphics gx, Statement stmt) {
			UI.ODGrid odGrid=new UI.ODGrid();//Only used for measurements, also contains printing/drawing logic.
			int _yAdjCurRow=0;//used to adjust for Titles, Headers, Rows, and footers (all considered part of the same row).
			odGrid.Width=0;
			List<DisplayField> Columns=SheetUtil.GetGridColumnsAvailable(field.FieldName);
			foreach(DisplayField Col in Columns) {
				odGrid.Width+=Col.ColumnWidth;
			}
			odGrid.Height=field.Height;
			odGrid.HideScrollBars=true;
			odGrid.YPosField=field.YPos;
			odGrid.Title=field.FieldName+(stmt.Intermingled?".Intermingled":".NotIntermingled");//Important for calculating heights.
			odGrid.TopMargin=40;
			odGrid.BottomMargin=60;
			odGrid.PageHeight=sheet.HeightPage;
			DataTable Table=SheetUtil.GetDataTableForGridType(field.FieldName,stmt);
			#region  Fill Grid, Set Text Alignment
			odGrid.BeginUpdate();
			odGrid.Columns.Clear();
			ODGridColumn col;
			for(int i=0;i<Columns.Count;i++) {
				col=new ODGridColumn(Columns[i].Description,Columns[i].ColumnWidth);
				switch(field.FieldName+"."+Columns[i].InternalName) {//Unusual switch statement to differentiate similar column names in different grids.
					case "StatementMain.charges":
					case "StatementMain.credits":
					case "StatementMain.balance":
					case "StatementPayPlan.charges":
					case "StatementPayPlan.credits":
					case "StatementPayPlan.balance":
						col.TextAlign=HorizontalAlignment.Right;
						break;
					case "StatementAging.Age00to30":
					case "StatementAging.Age31to60":
					case "StatementAging.Age61to90":
					case "StatementAging.Age90plus":
					case "StatementEnclosed.AmountDue":
					case "StatementEnclosed.DateDue":
						col.TextAlign=HorizontalAlignment.Center;
						break;
					default:
						col.TextAlign=HorizontalAlignment.Left;
						break;
				}
				odGrid.Columns.Add(col);
			}
			ODGridRow row;
			for(int i=0;i<Table.Rows.Count;i++) {
				row=new ODGridRow();
				for(int c=0;c<Columns.Count;c++) {//Selectively fill columns from the dataTable into the odGrid.
					row.Cells.Add(Table.Rows[i][Columns[c].InternalName].ToString());
				}
				odGrid.Rows.Add(row);
			}
			odGrid.EndUpdate(true);//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			#endregion
			for(int i=0;i<odGrid.RowHeights.Length;i++) {
				if(_isPrinting
					&& (odGrid.PrintRows[i].YPos-_printMargin.Top<_yPosPrint //rows at the end of previous page
						|| odGrid.PrintRows[i].YPos-sheet.HeightPage+_printMargin.Bottom>_yPosPrint)) 
				{
					continue;//continue because we do not want to draw rows from other pages.
				}
				_yAdjCurRow=0;
				//if(odGrid.PrintRows[i].YPos<_yPosPrint
				//	|| odGrid.PrintRows[i].YPos-_yPosPrint>sheet.HeightPage) {
				//	continue;//skip rows on previous page and rows on next page.
				//}
				#region Draw Title
				if(odGrid.PrintRows[i].IsTitleRow) {
					switch(field.FieldName) {//Draw titles differently for different grids.
						case "StatementMain":
							Patient pat=Patients.GetPat(PIn.Long(Table.Rows[i]["PatNum"].ToString()));
							string patName="";
							if(pat!=null) {//should always be true
								patName=pat.GetNameFLnoPref();
							}
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos-10,odGrid.PrintRows[i].YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString(patName,new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos-10,odGrid.PrintRows[i].YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,p(field.XPos-10),p(odGrid.PrintRows[i].YPos-_yPosPrint-1),p(odGrid.Width),p(odGrid.TitleHeight));
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,Graphics.FromImage(new Bitmap(100,100)),(double)((1d)/p(1)),patName,new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new XRect(p(field.XPos-10),p(odGrid.PrintRows[i].YPos-_yPosPrint-1),p(300),p(100)),XStringAlignment.Near);
									//gx.DrawString(patName,new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos-10,yPosGrid);
								}
							}
							break;
						case "StatementPayPlan":
							SizeF sSize=new SizeF();
							using(Graphics f= Graphics.FromImage(new Bitmap(100,100))) {//using graphics f because g is null when gx is not.
								sSize=f.MeasureString("Payment Plans",new Font("Arial",10,FontStyle.Bold));
							}
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString("Payment Plans",new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,odGrid.PrintRows[i].YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight);
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,Graphics.FromImage(new Bitmap(100,100)),(double)((1d)/p(1)),"Payment Plans",new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new XRect(p(field.XPos+field.Width/2),p(odGrid.PrintRows[i].YPos-_yPosPrint-1),p(300),p(100)),XStringAlignment.Center);
									//gx.DrawString("Payment Plans",new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,yPosGrid);
								}
							}
							break;
						default:
							if(gx==null) {
								odGrid.PrintTitle(g,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint);
							}
							else {
								odGrid.PrintTitleX(gx,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint);
							}
							break;
					}
					_yAdjCurRow+=odGrid.TitleHeight;
				}
				#endregion
				#region Draw Header
				if(odGrid.PrintRows[i].IsHeaderRow) {
					if(gx==null) {
						odGrid.PrintHeader(g,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow);
					}
					else {
						odGrid.PrintHeaderX(gx,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow);
					}
					_yAdjCurRow+=odGrid.HeaderHeight;
				}
				#endregion
				#region Draw Row
				if(gx==null) {
					odGrid.PrintRow(i,g,odGrid.Font,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow,odGrid.PrintRows[i].IsBottomRow,true);
				}
				else {
					odGrid.PrintRowX(i,gx,odGrid.Font,field.XPos,odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow,odGrid.PrintRows[i].IsBottomRow,true);
				}
				_yAdjCurRow+=odGrid.RowHeights[i];
				#endregion
				#region Draw Footer (rare)
				if(odGrid.PrintRows[i].IsFooterRow) {
					_yAdjCurRow+=2;
					switch(field.FieldName) {
						case "StatementPayPlan":
							DataTable tableMisc=AccountModules.GetStatementDataSet(stmt).Tables["misc"];
							if(tableMisc==null) {
								tableMisc=new DataTable();
							}
							Double payPlanDue=0;
							for(int m=0;m<tableMisc.Rows.Count;m++) {
								if(tableMisc.Rows[m]["descript"].ToString()=="payPlanDue") {
									payPlanDue=PIn.Double(tableMisc.Rows[m]["value"].ToString());
								}
							}
							if(gx==null) {
								RectangleF rf=new RectangleF(sheet.Width-60-field.Width,odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight);
								g.FillRectangle(Brushes.White,rf);
								StringFormat sf=new StringFormat();
								sf.Alignment=StringAlignment.Far;
								g.DrawString("Payment Plan Amount Due: "+payPlanDue.ToString("c"),new Font("Arial",9,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
							}
							else {
								gx.DrawRectangle(Brushes.White,p(sheet.Width-field.Width-60),p(odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow),p(field.Width),p(odGrid.TitleHeight));
								using(Font _font=new Font("Arial",9,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,Graphics.FromImage(new Bitmap(100,100)),(double)((1d)/p(1)),"Payment Plan Amount Due: "+payPlanDue.ToString("c"),new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new XRect(p(sheet.Width-60),p(odGrid.PrintRows[i].YPos-_yPosPrint+_yAdjCurRow),p(field.Width),p(odGrid.TitleHeight)),XStringAlignment.Far);
								}
							}
							break;
					}
				}
				#endregion
			}
		}

		///<summary>Calculates the bottom of the current page assuming a 40px and 60px top and bottom margin respectively.</summary>
		public static int bottomCurPage(int yPos,Sheet sheet, out int pageCount) {
			int retVal=sheet.HeightPage-60;//First page bottom is not changed by top margin. Example: 1100px page height, 60px bottom, 1040px is first page bottom
			pageCount=0;
			while(retVal<yPos){
				pageCount++;
				retVal+=(sheet.HeightPage-100);//each page bottom after the first, 1040px is first page break+1100px page height-top margin-bottom margin=2140px
			}
			return retVal;
		}

		public static void drawFieldText(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			Bitmap doubleBuffer=new Bitmap(sheet.Width,sheet.Height);//IsLandscape??
			Graphics gfx=Graphics.FromImage(doubleBuffer);
			Plugins.HookAddCode(null,"SheetPrinting.pd_PrintPage_drawFieldLoop",field);
			if(gx==null){
				FontStyle fontstyle=(field.FontIsBold?FontStyle.Bold:FontStyle.Regular);
				Font font=new Font(field.FontName,field.FontSize,fontstyle);
				Rectangle bounds=new Rectangle(field.XPos,field.YPos-_yPosPrint,field.Width,Math.Min(field.Height,_yPosPrint+sheet.HeightPage-_printMargin.Bottom-field.YPos));
				StringAlignment sa= StringAlignment.Near;
				switch(field.TextAlign) {
					case System.Windows.Forms.HorizontalAlignment.Left:
						sa=StringAlignment.Near;
						break;
					case System.Windows.Forms.HorizontalAlignment.Center:
						sa=StringAlignment.Center;
						break;
					case System.Windows.Forms.HorizontalAlignment.Right:
						sa=StringAlignment.Far;
						break;
				}
				GraphicsHelper.DrawString(g,gfx,field.FieldValue,font,(field.ItemColor==Color.FromArgb(0)?Brushes.Black:new SolidBrush(field.ItemColor)),bounds,sa);
				font.Dispose();
				font=null;
			}
			else{
				XFontStyle xfontstyle=(field.FontIsBold?XFontStyle.Bold:XFontStyle.Regular);
				XFont xfont=new XFont(field.FontName,field.FontSize,xfontstyle);
				XStringAlignment xsa= XStringAlignment.Near;
				switch(field.TextAlign) {
					case System.Windows.Forms.HorizontalAlignment.Left:
						xsa=XStringAlignment.Near;
						break;
					case System.Windows.Forms.HorizontalAlignment.Center:
						xsa=XStringAlignment.Center;
						field.XPos+=field.Width/2;
						break;
					case System.Windows.Forms.HorizontalAlignment.Right:
						xsa=XStringAlignment.Far;
						field.XPos+=field.Width;
						break;
				}
				XRect xrect=new XRect(p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width),p(field.Height));
				GraphicsHelper.DrawStringX(gx,gfx,1d/p(1),field.FieldValue,xfont,(field.ItemColor==Color.FromArgb(0)?XBrushes.Black:new XSolidBrush(field.ItemColor)),xrect,xsa);
				//xfont.Dispose();
				xfont=null;
			}
			doubleBuffer.Dispose();
			doubleBuffer=null;
			gfx.Dispose();
			gfx=null;
		}

		public static void drawFieldCheckBox(SheetField field,Graphics g,XGraphics gx) {
			if(field.FieldValue!="X") {
				return;
			}
			if(gx==null) {
				Pen pen3=new Pen(Brushes.Black,1.6f);
				g.DrawLine(pen3,field.XPos,field.YPos-_yPosPrint,field.XPos+field.Width,field.YPos-_yPosPrint+field.Height);
				g.DrawLine(pen3,field.XPos+field.Width,field.YPos-_yPosPrint,field.XPos,field.YPos-_yPosPrint+field.Height);
				pen3.Dispose();
				pen3=null;
			}
			else {
				XPen pen3=new XPen(XColors.Black,p(1.6f));
				gx.DrawLine(pen3,p(field.XPos),p(field.YPos-_yPosPrint),p(field.XPos+field.Width),p(field.YPos-_yPosPrint+field.Height));
				gx.DrawLine(pen3,p(field.XPos+field.Width),p(field.YPos-_yPosPrint),p(field.XPos),p(field.YPos-_yPosPrint+field.Height));
				pen3=null;
			}
		}

		public static void drawFieldSigBox(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			SignatureBoxWrapper wrapper=new SignatureBoxWrapper();
			wrapper.Width=field.Width;
			wrapper.Height=field.Height;
			if(field.FieldValue.Length>0) {//a signature is present
				bool sigIsTopaz=false;
				if(field.FieldValue[0]=='1') {
					sigIsTopaz=true;
				}
				string signature="";
				if(field.FieldValue.Length>1) {
					signature=field.FieldValue.Substring(1);
				}
				string keyData=Sheets.GetSignatureKey(sheet);
				wrapper.FillSignature(sigIsTopaz,keyData,signature);
			}
			if(g!=null) {
				Bitmap sigBitmap=wrapper.GetSigImage();
				g.DrawImage(sigBitmap,field.XPos,field.YPos-_yPosPrint,field.Width-2,field.Height-2);
				sigBitmap.Dispose();
				sigBitmap=null;
			}
			else {
				XImage sigBitmap=XImage.FromGdiPlusImage(wrapper.GetSigImage());
				gx.DrawImage(sigBitmap,p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width-2),p(field.Height-2));
				sigBitmap.Dispose();
				sigBitmap=null;
			}
		}

		private static void drawHeader(Sheet sheet,Graphics g,XGraphics gx) {
			if(_pagesPrinted==0) {
				return;//Never draw header on first page
			}
			//white-out the header.
			if(gx==null) {
				g.FillRectangle(Brushes.White,0,0,sheet.WidthPage,_printMargin.Top);
			}
			else {
				gx.DrawRectangle(XPens.White,Brushes.White,p(0),p(0),p(sheet.WidthPage),p(_printMargin.Top));
			}
		}

		private static void drawFooter(Sheet sheet,Graphics g,XGraphics gx) {
			if(Sheets.CalculatePageCount(sheet,_printMargin)==1) {
				return;//Never draw footers on single page sheets.
			}
			//whiteout footer.
			if(gx==null) {
				g.FillRectangle(Brushes.White,0,sheet.HeightPage-_printMargin.Bottom,sheet.WidthPage,sheet.HeightPage);
			}
			else {
				gx.DrawRectangle(XPens.White,Brushes.White,p(0),p(sheet.HeightPage-_printMargin.Bottom),p(sheet.WidthPage),p(sheet.HeightPage));
			}
		}

		private static void drawCalibration(Sheet sheet,Graphics g,PrintPageEventArgs e,XGraphics gx, PdfPage page) {
			Font font=new Font("Calibri",10f,FontStyle.Regular);
			XFont xfont=new XFont("Calibri",p(10f),XFontStyle.Regular);
			int sLineSize=15;
			int mLineSize=45;
			int lLineSize=90;
			for(int pass=0;pass<3;pass++) {
				int xO=0;//xOrigin
				int yO=0;//yOrigin
				switch(pass) {
					case 0: xO=yO=0; break;
					case 1: xO=sheet.WidthPage/2; yO=sheet.HeightPage/2; break;
					case 2: xO=sheet.WidthPage; yO=sheet.HeightPage; break;
				}
				for(int i=-100;i<2000;i++) {
					if(i%100==0 && pass==0) {
						//label Axis
						if(g!=null) {
							if(i==0) {
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(4,4));//label 0
							}//don't draw the zero twice
							else {
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(xO+75,i+2));//label Y-axis
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(i+2,yO+75));//label X-axis
							}
						}
						else {
							if(i==0) {

							}//don't draw the zero twice
							else {
								gx.DrawString(i.ToString(),xfont,XBrushes.Black,p(xO+75),p(i+2));//label Y-axis
								gx.DrawString(i.ToString(),xfont,XBrushes.Black,p(i+2),p(yO+75));//label X-axis
							}
						}
					}
					if(i%100==0) {
						//draw large lines and label txt
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-lLineSize+xO,i),new Point(+lLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-lLineSize+yO),new Point(i,+lLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-lLineSize+xO),p(i),p(+lLineSize+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-lLineSize+yO),p(i),p(+lLineSize+yO));//Allong X-axis
						}
					}
					else if(i%50==0) {
						//draw 50px lines
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-mLineSize+xO,i),new Point(+mLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-mLineSize+yO),new Point(i,+mLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-mLineSize+xO),p(i),p(+mLineSize+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-mLineSize+yO),p(i),p(+mLineSize+yO));//Allong X-axis
						}
					}
					else if(i%10==0) {
						//draw small lines
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-sLineSize+xO,i),new Point(+sLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-sLineSize+yO),new Point(i,+sLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,new Point(-sLineSize+xO,i),new Point(+sLineSize+xO,i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-sLineSize+yO),p(i),p(+sLineSize+yO));//Allong X-axis
						}
					}
					else if(i%2==0) {
						//draw dots
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-1+xO,i),new Point(+1+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-1+yO),new Point(i,+1+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-1+xO),p(i),p(+1+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-1+yO),p(i),p(+1+yO));//Allong X-axis
						}
					}
				}//end i -100=>2000
			}//end pass
			//infoBlock
			PrinterSettings settings = new PrinterSettings();
			if(g!=null) {
				g.FillRectangle(Brushes.White,110,110,480,100);
				g.DrawRectangle(Pens.Black,110,110,480,100);
				g.DrawString("Sheet Height = "+sheet.HeightPage.ToString(),font,Brushes.Black,112,112);
				g.DrawString("Sheet Width = "+sheet.WidthPage.ToString(),font,Brushes.Black,112,124);//12px per line
				g.DrawString("DefaultPrinter = "+settings.PrinterName,font,Brushes.Black,112,136);
				g.DrawString("HardMarginX = "+e.PageSettings.HardMarginX,font,Brushes.Black,112,148);
				g.DrawString("HardMarginY = "+e.PageSettings.HardMarginY,font,Brushes.Black,112,160);
			}
			else {
				gx.DrawRectangle(XPens.Black,Brushes.White,p(110),p(110),p(480),p(100));
				gx.DrawRectangle(XPens.Black,p(110),p(110),p(480),p(100));
				gx.DrawString("Sheet Height = "+sheet.HeightPage.ToString(),xfont,XBrushes.Black,p(112),p(112));
				gx.DrawString("Sheet Width = "+sheet.WidthPage.ToString(),xfont,XBrushes.Black,p(112),p(124));//12px per line
				gx.DrawString("DefaultPrinter = "+settings.PrinterName,xfont,XBrushes.Black,p(112),p(136));
				gx.DrawString("HardMarginX = "+settings.DefaultPageSettings.HardMarginX,xfont,XBrushes.Black,p(112),p(148));
				gx.DrawString("HardMarginY = "+settings.DefaultPageSettings.HardMarginY,xfont,XBrushes.Black,p(112),p(160));
				gx.DrawString("PDF TrimMargins ^v<> = "+page.TrimMargins.Top+","+page.TrimMargins.Bottom+","+page.TrimMargins.Left+","+page.TrimMargins.Right,xfont,XBrushes.Black,p(112),p(172));
			}
			font.Dispose();
			font=null;
			xfont=null;
		}

		#endregion

		public static void CreatePdf(Sheet sheet,string fullFileName,Statement stmt) {
			_stmt=stmt;
			_isPrinting=true;
			_yPosPrint=0;
			PdfDocument document=new PdfDocument();
			//SetForceSinglePage(sheet);
			int pageCount=Sheets.CalculatePageCount(sheet,_printMargin);
			for(int i=0;i<pageCount;i++) {
				_pagesPrinted=i;
				PdfPage page=document.AddPage();
				CreatePdfPage(sheet,page);
			}
			document.Save(fullFileName);
			_isPrinting=false;
			GC.Collect();//We are done creating the pdf so we can forcefully clean up all the objects and controls that were used.
		}

		///<summary>Called for every page that is generated for a PDF docuemnt. Pages and yPos must be tracked outside of this function. See also pd_PrintPage.</summary>
		public static void CreatePdfPage(Sheet sheet,PdfPage page) {
			sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);//should always be sorted.
			page.Width=p(sheet.Width);//XUnit.FromInch((double)sheet.Width/100);  //new XUnit((double)sheet.Width/100,XGraphicsUnit.Inch);
			page.Height=p(sheet.Height);//new XUnit((double)sheet.Height/100,XGraphicsUnit.Inch);
			if(sheet.IsLandscape){
				page.Orientation=PageOrientation.Landscape;
			}
			Sheets.SetPageMargin(sheet,_printMargin);
			XGraphics gx=XGraphics.FromPdfPage(page);
			gx.SmoothingMode=XSmoothingMode.HighQuality;
			foreach(SheetField field in sheet.SheetFields) {
				if(!fieldOnCurPageHelper(field,sheet,_printMargin,_yPosPrint)) { 
					continue; 
				}
				switch(field.FieldType) {
					case SheetFieldType.Image:
					case SheetFieldType.PatImage:
						drawFieldImage(field,null,gx);
						break;
					case SheetFieldType.Drawing:
						drawFieldDrawing(field,null,gx);
						break;
					case SheetFieldType.Rectangle:
						drawFieldRectangle(field,null,gx);
						break;
					case SheetFieldType.Line:
						drawFieldLine(field,null,gx);
						break;
					case SheetFieldType.Grid:
						drawFieldGrid(field,sheet,null,gx,_stmt);
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
					case SheetFieldType.StaticText:
						drawFieldText(field,sheet,null,gx);
						break;
					case SheetFieldType.CheckBox:
						drawFieldCheckBox(field,null,gx);
						break;
					case SheetFieldType.SigBox:
						drawFieldSigBox(field,sheet,null,gx);
						break;
					default:
						//Parameter or possibly new field type.
						break;
				}
			}//end foreach SheetField
			drawHeader(sheet,null,gx);
			drawFooter(sheet,null,gx);
			gx.Dispose();
			gx=null;
			#region Set variables for next page to be printed
			_yPosPrint+=sheet.HeightPage-(_printMargin.Bottom+_printMargin.Top);//move _yPosPrint down equal to the amount of printable area per page.
			_pagesPrinted++;
			if(_pagesPrinted<Sheets.CalculatePageCount(sheet,_printMargin)) {
				//e.HasMorePages=true;
			}
			else {//we are printing the last page of the current sheet.
				_yPosPrint=_printMargin.Top;
				_pagesPrinted=0;
				_sheetsPrinted++;
				//if(_sheetsPrinted<_sheetList.Count) {
				//	//e.HasMorePages=true;
				//}
				//else {
				//	//e.HasMorePages=false;
				//	_sheetsPrinted=0;
				//}
			}


			//foreach(SheetField field in sheet.SheetFields) {
			//	if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom
			//		&& field.YPos!=_yPosPrint+_printMargin.Top) {
			//		//Either set new page to the top of the next control or the top of the next natural page break, whichever comes first.
			//		_yPosPrint=Math.Min(field.YPos-_printMargin.Top,_yPosPrint+sheet.HeightPage);
			//		break;
			//	}
			//}
			#endregion
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing and rendering the sheet fill edit window.</summary>
		public static void DrawImages(Sheet sheet,Graphics graphic,bool drawAll=false) {
			DrawImages(sheet,graphic,null,drawAll);
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing, exporting to pdfs, or rendering the sheet fill edit window.  graphic should be null for pdfs and xgraphic should be null for printing and rendering the sheet fill edit window.</summary>
		private static void DrawImages(Sheet sheet,Graphics graphic,XGraphics xGraphic,bool drawAll=false) {
			Bitmap bmpOriginal=null;
			if(drawAll){// || _forceSinglePage) {//reset _yPosPrint because we are drawing all.
				_yPosPrint=0;
			}
			foreach(SheetField field in sheet.SheetFields) {
				if(!drawAll ){//&& !_forceSinglePage) {
					if(field.YPos<_yPosPrint) {
						continue; //skip if on previous page
					}
					if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom
						&& field.YPos!= _yPosPrint+_printMargin.Top) {
						break; //Skip if on next page
					}
				}
				if(field.Height==0 || field.Width==0) {
					continue;//might be possible with really old sheets.
				}
				#region Get the path for the image
				string filePathAndName="";
				switch(field.FieldType) {
					case SheetFieldType.Image:
						filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),field.FieldName);
						break;
					case SheetFieldType.PatImage:
						if(field.FieldValue=="") {
							//There is no document object to use for display, but there may be a baked in image and that situation is dealt with below.
							filePathAndName="";
							break;
						}
						Document patDoc=Documents.GetByNum(PIn.Long(field.FieldValue));
						List<string> paths=Documents.GetPaths(new List<long> { patDoc.DocNum },ImageStore.GetPreferredAtoZpath());
						if(paths.Count < 1) {//No path was found so we cannot draw the image.
							continue;
						}
						filePathAndName=paths[0];
						break;
					default:
						//not an image field
						continue;
				}
				#endregion
				#region Load the image into bmpOriginal
				if(field.FieldName=="Patient Info.gif") {
					bmpOriginal=OpenDentBusiness.Properties.Resources.Patient_Info;
				}
				else if(File.Exists(filePathAndName)) {
					bmpOriginal=new Bitmap(filePathAndName);
				}
				else {
					continue;
				}
				#endregion
				#region Calculate the image ratio and location, set values for imgDrawWidth and imgDrawHeight
				//inscribe image in field while maintaining aspect ratio.
				float imgRatio=(float)bmpOriginal.Width/(float)bmpOriginal.Height;
				float fieldRatio=(float)field.Width/(float)field.Height;
				float imgDrawHeight=field.Height;//drawn size of image
				float imgDrawWidth=field.Width;//drawn size of image
				int adjustY=0;//added to YPos
				int adjustX=0;//added to XPos
				//For patient images, we need to make sure the images will fit and can maintain aspect ratio.
				if(field.FieldType==SheetFieldType.PatImage && imgRatio>fieldRatio) {//image is too wide
					//X pos and width of field remain unchanged
					//Y pos and height must change
					imgDrawHeight=(float)bmpOriginal.Height*((float)field.Width/(float)bmpOriginal.Width);//img.Height*(width based scale) This also handles images that are too small.
					adjustY=(int)((field.Height-imgDrawHeight)/2f);//adjustY= half of the unused vertical field space
				}
				else if(field.FieldType==SheetFieldType.PatImage && imgRatio<fieldRatio) {//image is too tall
					//X pos and width must change
					//Y pos and height remain unchanged
					imgDrawWidth=(float)bmpOriginal.Width*((float)field.Height/(float)bmpOriginal.Height);//img.Height*(width based scale) This also handles images that are too small.
					adjustX=(int)((field.Width-imgDrawWidth)/2f);//adjustY= half of the unused horizontal field space
				}
				else {//image ratio == field ratio
					//do nothing
				}
				#endregion
				#region Draw the image
				if(xGraphic!=null) {//Drawing an image to a pdf.
					XImage xI=XImage.FromGdiPlusImage((Bitmap)bmpOriginal.Clone());
					xGraphic.DrawImage(xI,p(field.XPos+adjustX),p(field.YPos-_yPosPrint+adjustY),p(imgDrawWidth),p(imgDrawHeight));
					if(xI!=null) {//should always happen
						xI.Dispose();
						xI=null;
					}
				}
				else if(graphic!=null) {//Drawing an image to a printer or the sheet fill edit window.
					graphic.DrawImage(bmpOriginal,field.XPos+adjustX,field.YPos+adjustY-_yPosPrint,imgDrawWidth,imgDrawHeight);
				}
				#endregion
			}
			if(bmpOriginal!=null) {
				bmpOriginal.Dispose();
				bmpOriginal=null;
			}
		}
		
		///<summary>Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(int pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
				//XUnit.FromInch((double)pixels/100);
		}

		///<summary>Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(float pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
		}
		
	}
}
