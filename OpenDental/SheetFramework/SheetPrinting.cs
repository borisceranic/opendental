﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OpenDental {
	public class SheetPrinting {
		///<summary>If there is only one sheet, then this will stay 0.</Summary>
		private static int sheetsPrinted;
		///<summary>Pages printed on current sheet. Only used for printing, not for generating PDFs.</summary>
		private static int pagesPrinted;
		///<summary>Used for determining page breaks. When moving to next page, use this Y value to determine the next field to print.</summary>
		private static int _yPosPrint;
		///<summary>Print margin of the default printer. only used in page break calulations, and only top and bottom are used.</summary>
		private static Margins _printMargin=new Margins(0,0,40,60);
		///<summary>If not a batch, then there will just be one sheet in the list.</summary>
		private static List<Sheet> SheetList;

		///<summary>Surround with try/catch.</summary>
		public static void PrintBatch(List<Sheet> sheetBatch){
			//currently no validation for parameters in a batch because of the way it was created.
			//could validate field names here later.
			SheetList=sheetBatch;
			sheetsPrinted=0;
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
				FormPrintPreview printPreview=new FormPrintPreview(sit,pd,SheetList.Count,0,"Batch of "+sheetBatch[0].Description+" printed");
				printPreview.ShowDialog();
			#else
				try {
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

		///<Summary>Surround with try/catch.</Summary>
		public static void Print(Sheet sheet){
			Print(sheet,1,false);
		}

		///<Summary>Surround with try/catch.</Summary>
		public static void Print(Sheet sheet,int copies){
			Print(sheet,copies,false);
		}

		///<Summary></Summary>
		public static void Print(Sheet sheet,int copies,bool isRxControlled){
			//parameter null check moved to SheetFiller.
			//could validate field names here later.
			SheetList=new List<Sheet>();
			for(int i=0;i<copies;i++){
				SheetList.Add(sheet.Copy());
			}
			sheetsPrinted=0;
			_yPosPrint=0;
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
				pd.DefaultPageSettings.Margins=new Margins(20,20,0,0);
				FormPrintPreview printPreview;
				printPreview=new FormPrintPreview(sit,pd,Sheets.CalculatePageCount(sheet,_printMargin),sheet.PatNum,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed");
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
		}

		private static void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Graphics g=e.Graphics;
			g.SmoothingMode=SmoothingMode.HighQuality;
			Sheet sheet=SheetList[sheetsPrinted];
			SheetUtil.CalculateHeights(sheet,g);//this is here because of easy access to g.
			Font font;
			FontStyle fontstyle;
			//first, draw images------------------------------------------------------------------------------------
			DrawImages(sheet,g);
			//then, drawings--------------------------------------------------------------------------------------------
			Pen pen=new Pen(Brushes.Black,2f);
			string[] pointStr;
			List<Point> points;
			Point point;
			string[] xy;
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) { 
					continue; //skip if on previous page
				} 
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) { 
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.Drawing) {
					continue;
				}
				pointStr=field.FieldValue.Split(';');
				points=new List<Point>();
				for(int p=0;p<pointStr.Length;p++) {
					xy=pointStr[p].Split(',');
					if(xy.Length==2) {
						point=new Point(PIn.Int(xy[0]),PIn.Int(xy[1]));
						points.Add(point);
					}
				}
				for(int i=1;i<points.Count;i++) {
					g.DrawLine(pen,points[i-1].X,points[i-1].Y-_yPosPrint,points[i].X,points[i].Y-_yPosPrint);
				}
			}
			//then, rectangles and lines----------------------------------------------------------------------------------
			Pen pen2=new Pen(Brushes.Black,1f);
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType==SheetFieldType.Rectangle) {
					g.DrawRectangle(pen2,field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
				}
				if(field.FieldType==SheetFieldType.Line) {
					g.DrawLine(pen2,field.XPos,field.YPos-_yPosPrint,
						field.XPos+field.Width,
						field.YPos+field.Height);
				}
			}
			//then, draw text-----------------------------------------------------------------------------------------------
			Bitmap doubleBuffer=new Bitmap(sheet.Width,sheet.Height);//IsLandscape??
			Graphics gfx=Graphics.FromImage(doubleBuffer);
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.InputField
				&& field.FieldType!=SheetFieldType.OutputText
				&& field.FieldType!=SheetFieldType.StaticText) {
					continue;
				}
				fontstyle=FontStyle.Regular;
				if(field.FontIsBold) {
					fontstyle=FontStyle.Bold;
				}
				font=new Font(field.FontName,field.FontSize,fontstyle);
				Plugins.HookAddCode(null,"SheetPrinting.pd_PrintPage_drawFieldLoop",field);
				Rectangle bounds=new Rectangle(field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
				GraphicsHelper.DrawString(g,gfx,field.FieldValue,font,Brushes.Black,bounds);
				//g.DrawString(field.FieldValue,font,Brushes.Black,field.BoundsF);
			}
			gfx.Dispose();
			//then, checkboxes----------------------------------------------------------------------------------
			Pen pen3=new Pen(Brushes.Black,1.6f);
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.CheckBox) {
					continue;
				}
				if(field.FieldValue=="X") {
					g.DrawLine(pen3,field.XPos,field.YPos-_yPosPrint,field.XPos+field.Width,field.YPos-_yPosPrint+field.Height);
					g.DrawLine(pen3,field.XPos+field.Width,field.YPos-_yPosPrint,field.XPos,field.YPos-_yPosPrint+field.Height);
				}
			}
			//then signature boxes----------------------------------------------------------------------
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.SigBox) {
					continue;
				}
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
				Bitmap sigBitmap=wrapper.GetSigImage();
				g.DrawImage(sigBitmap,field.XPos,field.YPos-_yPosPrint,field.Width-2,field.Height-2);
			}
			//Set the _yPosPrint for the next page
			foreach(SheetField field in sheet.SheetFields) {
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					//Either set new page to the top of the next control or the top of the next natural page break, whichever comes first.
					_yPosPrint=Math.Min(field.YPos-_printMargin.Top,_yPosPrint+sheet.HeightPage);
					break;
				}
			}
			pagesPrinted++;
			g.Dispose();
			if(pagesPrinted<Sheets.CalculatePageCount(sheet,_printMargin)) {
				e.HasMorePages=true;
			}
			else {//we are printing the last page of the current sheet.
				pagesPrinted=0;
				sheetsPrinted++;
				//heightsCalculated=false;
				if(sheetsPrinted<SheetList.Count){
					e.HasMorePages=true;
				}
				else{
					e.HasMorePages=false;
					sheetsPrinted=0;
				}	
			}
		}

		public static void CreatePdf(Sheet sheet,string fullFileName) {
			_yPosPrint=0;
			PdfDocument document=new PdfDocument();
			int pageCount=Sheets.CalculatePageCount(sheet,_printMargin);
			for(int i=0;i<pageCount;i++) {
				PdfPage page=document.AddPage();
				CreatePdfPage(sheet,page);
			}
			document.Save(fullFileName);
		}

		public static void CreatePdfPage(Sheet sheet,PdfPage page) {
			page.Width=p(sheet.Width);//XUnit.FromInch((double)sheet.Width/100);  //new XUnit((double)sheet.Width/100,XGraphicsUnit.Inch);
			page.Height=p(sheet.Height);//new XUnit((double)sheet.Height/100,XGraphicsUnit.Inch);
			if(sheet.IsLandscape){
				page.Orientation=PageOrientation.Landscape;
			}
			XGraphics g=XGraphics.FromPdfPage(page);
			g.SmoothingMode=XSmoothingMode.HighQuality;
			//g.PageUnit=XGraphicsUnit. //wish they had pixel
			//XTextFormatter tf = new XTextFormatter(g);//needed for text wrap
			//tf.Alignment=XParagraphAlignment.Left;
			//pd.DefaultPageSettings.Landscape=
			//already done?:SheetUtil.CalculateHeights(sheet,g);//this is here because of easy access to g.
			XFont xfont;
			XFontStyle xfontstyle;
			sheet.SheetFields.Sort(SheetFields.SortBottomBounds);
			//first, draw images--------------------------------------------------------------------------------------
			DrawImagesToPdf(sheet,g);
			//then, drawings--------------------------------------------------------------------------------------------
			XPen pen=new XPen(XColors.Black,p(2));
			string[] pointStr;
			List<Point> points;
			Point point;
			string[] xy;
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.Drawing){
					continue;
				}
				pointStr=field.FieldValue.Split(';');
				points=new List<Point>();
				for(int j=0;j<pointStr.Length;j++){
					xy=pointStr[j].Split(',');
					if(xy.Length==2){
						point=new Point(PIn.Int(xy[0]),PIn.Int(xy[1]));
						points.Add(point);
					}
				}
				for(int i=1;i<points.Count;i++){
					g.DrawLine(pen,p(points[i-1].X),p(points[i-1].Y-_yPosPrint),p(points[i].X),p(points[i].Y-_yPosPrint));
				}
			}
			//then, rectangles and lines----------------------------------------------------------------------------------
			XPen pen2=new XPen(XColors.Black,p(1));
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType==SheetFieldType.Rectangle){
					g.DrawRectangle(pen2,p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width),p(field.Height));
				}
				if(field.FieldType==SheetFieldType.Line){
					g.DrawLine(pen2,p(field.XPos),p(field.YPos-_yPosPrint),
						p(field.XPos+field.Width),
						p(field.YPos+field.Height));
				}
			}
			//then, draw text--------------------------------------------------------------------------------------------
			Bitmap doubleBuffer=new Bitmap(sheet.Width,sheet.Height);
			Graphics gfx=Graphics.FromImage(doubleBuffer);
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.InputField
					&& field.FieldType!=SheetFieldType.OutputText
					&& field.FieldType!=SheetFieldType.StaticText)
				{
					continue;
				}
				xfontstyle=XFontStyle.Regular;
				if(field.FontIsBold){
					xfontstyle=XFontStyle.Bold;
				}
				xfont=new XFont(field.FontName,field.FontSize,xfontstyle);
				//xfont=new XFont(field.FontName,field.FontSize,xfontstyle);
				//Rectangle rect=new Rectangle((int)p(field.XPos),(int)p(field.YPos),(int)p(field.Width),(int)p(field.Height));
				XRect xrect=new XRect(p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width),p(field.Height));
				//XStringFormat format=new XStringFormat();
				//tf.DrawString(field.FieldValue,font,XBrushes.Black,xrect,XStringFormats.TopLeft);
				GraphicsHelper.DrawStringX(g,gfx,1d/p(1),field.FieldValue,xfont,XBrushes.Black,xrect);
			}
			gfx.Dispose();
			//then, checkboxes----------------------------------------------------------------------------------
			XPen pen3=new XPen(XColors.Black,p(1.6f));
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.CheckBox){
					continue;
				}
				if(field.FieldValue=="X"){
					g.DrawLine(pen3,p(field.XPos),p(field.YPos-_yPosPrint),p(field.XPos+field.Width),p(field.YPos+field.Height));
					g.DrawLine(pen3,p(field.XPos+field.Width),p(field.YPos-_yPosPrint),p(field.XPos),p(field.YPos+field.Height));
				}
			}
			//then signature boxes----------------------------------------------------------------------
			foreach(SheetField field in sheet.SheetFields) {
				if(field.YPos<_yPosPrint) {
					continue; //skip if on previous page
				}
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom) {
					break; //Skip if on next page
				} 
				if(field.FieldType!=SheetFieldType.SigBox){
					continue;
				}
				SignatureBoxWrapper wrapper=new SignatureBoxWrapper();
				wrapper.Width=field.Width;
				wrapper.Height=field.Height;
				if(field.FieldValue.Length>0){//a signature is present
					bool sigIsTopaz=false;
					if(field.FieldValue[0]=='1'){
						sigIsTopaz=true;
					}
					string signature="";
					if(field.FieldValue.Length>1){
						signature=field.FieldValue.Substring(1);
					}
					string keyData=Sheets.GetSignatureKey(sheet);
					wrapper.FillSignature(sigIsTopaz,keyData,signature);
				}
				XImage sigBitmap=XImage.FromGdiPlusImage(wrapper.GetSigImage());
				g.DrawImage(sigBitmap,p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width-2),p(field.Height-2));
			}
			//Set the _yPosPrint for the next page
			foreach(SheetField field in sheet.SheetFields) {
				if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom
					&& field.YPos!=_yPosPrint+_printMargin.Top) {
					//Either set new page to the top of the next control or the top of the next natural page break, whichever comes first.
					_yPosPrint=Math.Min(field.YPos-_printMargin.Top,_yPosPrint+sheet.HeightPage);
					break;
				}
			}
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing and rendering the sheet fill edit window.</summary>
		public static void DrawImages(Sheet sheet,Graphics graphic,bool drawAll=false) {
			DrawImages(sheet,graphic,null,drawAll);
		}

		///<summary>Draws all images from the sheet onto the xgraphic passed in.  Used when exporting to pdfs.</summary>
		public static void DrawImagesToPdf(Sheet sheet,XGraphics xgraphic) {
			DrawImages(sheet,null,xgraphic);
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing, exporting to pdfs, or rendering the sheet fill edit window.  graphic should be null for pdfs and xgraphic should be null for printing and rendering the sheet fill edit window.</summary>
		private static void DrawImages(Sheet sheet,Graphics graphic,XGraphics xGraphic,bool drawAll=false) {
			Bitmap bmpOriginal=null;
			foreach(SheetField field in sheet.SheetFields) {
				if(drawAll) {//reset _yPosPrint because we are drawing all.
					_yPosPrint=0;
				}
				else {
					if(field.YPos<_yPosPrint) {
						continue; //skip if on previous page
					}
					if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom
						&& field.YPos!= _yPosPrint+_printMargin.Top) {
						break; //Skip if on next page
					} 
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
				#region Calculate the image ratio and location
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
					Bitmap bmpResampled=(Bitmap)bmpOriginal.Clone();
					if(bmpOriginal.HorizontalResolution!=96 || bmpOriginal.VerticalResolution!=96) {//to avoid slowdown for other pdfs
						//The scaling on the XGraphics.DrawImage() function causes unreadable output unless the image is in 96 DPI native format.
						//We use GDI here first to convert the image to the correct size and DPI, then pass the second image to XGraphics.DrawImage().
						bmpResampled.Dispose();
						bmpResampled=null;
						bmpResampled=new Bitmap((Bitmap)bmpOriginal.Clone());//resample the resolution.
					}
					xGraphic.DrawImage(bmpResampled,p(field.XPos+adjustX),p(field.YPos-_yPosPrint+adjustY),p(imgDrawWidth),p(imgDrawHeight));
					bmpResampled.Dispose();
					bmpResampled=null;
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

		/*//<summary>Converts pixels used by us to points used by PdfSharp.</summary>
		private double P(double pixels){
			return (double)pixels/100;
		}*/

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
