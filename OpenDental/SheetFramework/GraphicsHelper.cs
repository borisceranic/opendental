using System.Drawing;
using PdfSharp.Drawing;

namespace OpenDental {
	public class GraphicsHelper {
		private static int topPad=2;
		private static int rightPad=5;//helps get measurements better.
		//private static float hScale=.983f;

		///<summary>This line spacing is specifically picked to match the RichTextBox.  Using this for drawing large text boxes on sheets may lead to extra 
		///white space at the bottom of large text fields.  Just space not characters. 
		///Used to scale text when drawing and measuring. Determines vertical text height and height of textboxes to be drawn. Only used for sheets. 
		///UI has a similar function in OdGrid.LinseSPacingForFont</summary>
		private static float LineSpacingForFont(string fontName) {
			if(fontName.ToLower()=="arial") {
				//Used to scale text when drawing and measuring. Determines vertical text height and height of textboxes to be drawn. Only used for sheets.
				return 1.055f;
			}
			else if(fontName.ToLower()=="courier new") {
				return 1.08f;
			}
			return 1.05f;
		}

		///<summary>Since Graphics doesn't have a line height property.  The second graphics object is used for measurement purposes.</summary>
		public static void DrawString(Graphics g,Graphics gfx,string str,Font font,Brush brush,Rectangle bounds,StringAlignment sa) {
			SizeF fit=new SizeF(bounds.Width-rightPad,font.Height);
			StringFormat format=StringFormat.GenericTypographic;
			float pixelsPerLine=LineSpacingForFont(font.Name) * (float)font.Height;
			float lineIdx=0;
			int chars;
			int lines;
			RectangleF layoutRectangle;
			float layoutH;
			for(int ix=0;ix<str.Length;ix+=chars) {
				if(bounds.Y+topPad+pixelsPerLine*lineIdx>bounds.Bottom) {
					break;
				}
				gfx.MeasureString(str.Substring(ix),font,fit,format,out chars,out lines);
				if(bounds.Y+topPad+pixelsPerLine*lineIdx+font.Height > bounds.Bottom) {
					layoutH=bounds.Bottom-(bounds.Y+topPad+pixelsPerLine*lineIdx);
				}
				else {
					layoutH=font.Height+2;
				}
				int adjX=0;
				int adjW=100;//any amount of extra padding here will not cause malfunction
				switch(sa) {
					case StringAlignment.Near:
						adjX=0;
						break;
					case StringAlignment.Far:
						adjX=-adjW;
						break;
					case StringAlignment.Center:
						adjX=-adjW/2;
						break;
				}
				layoutRectangle=new RectangleF(
					bounds.X+adjX,//must add same number of pixels 
					(float)(bounds.Y+topPad+pixelsPerLine*lineIdx),
					bounds.Width+adjW,
					layoutH);
				using(StringFormat sf=StringFormat.GenericDefault) {
					sf.Alignment=sa;
					//The overload for DrawString that takes a StringFormat will cause the tabs '\t' to be ignored.
					//In order for the tabs to not get ignored, we have to tell StringFormat how many pixels each tab should be.  
					//50.0f is the closest to our Fill Sheet Edit preview.
					sf.SetTabStops(0.0f,new float[1] { 50.0f });
					g.DrawString(str.Substring(ix,chars),font,brush,layoutRectangle,sf);
				}
				lineIdx+=1;
			}
		}

		///<summary>The pdfSharp version of drawstring.  g is used for measurement.  scaleToPix scales xObjects to pixels.</summary>
		public static void DrawStringX(XGraphics xg,Graphics g,double scaleToPix,string str,XFont xfont,XBrush xbrush,XRect xbounds, XStringAlignment sa) {
			//There are two coordinate systems here: pixels (used by us) and points (used by PdfSharp).
			//MeasureString and ALL related measurement functions must use pixels.
			//DrawString is the ONLY function that uses points.
			//pixels:
			Rectangle bounds=new Rectangle((int)(scaleToPix*xbounds.Left),
				(int)(scaleToPix*xbounds.Top),
				(int)(scaleToPix*xbounds.Width),
				(int)(scaleToPix*xbounds.Height));
			FontStyle fontstyle=FontStyle.Regular;
			if(xfont.Style==XFontStyle.Bold) {
				fontstyle=FontStyle.Bold;
			}
			//pixels: (except Size is em-size)
			Font font=new Font(xfont.Name,(float)xfont.Size,fontstyle);
			//pixels:
			SizeF fit=new SizeF((float)(bounds.Width-rightPad),(float)(font.Height));
			StringFormat format=StringFormat.GenericTypographic;
			//pixels:
			float pixelsPerLine=LineSpacingForFont(font.Name) * (float)font.Height;
			float lineIdx=0;
			int chars;
			int lines;
			//points:
			RectangleF layoutRectangle;
			for(int ix=0;ix<str.Length;ix+=chars) {
				if(bounds.Y+topPad+pixelsPerLine*lineIdx>bounds.Bottom) {
					break;
				}
				//pixels:
				g.MeasureString(str.Substring(ix),font,fit,format,out chars,out lines);
				//PdfSharp isn't smart enough to cut off the lower half of a line.
				//if(bounds.Y+topPad+pixelsPerLine*lineIdx+font.Height > bounds.Bottom) {
				//	layoutH=bounds.Bottom-(bounds.Y+topPad+pixelsPerLine*lineIdx);
				//}
				//else {
				//	layoutH=font.Height+2;
				//}
				//use points here:
				float adjustTextDown=10f;//this value was arrived at by trial and error.
				layoutRectangle=new RectangleF(
					(float)xbounds.X,
					//(float)(xbounds.Y+(float)topPad/scaleToPix+(pixelsPerLine/scaleToPix)*lineIdx),
					(float)(xbounds.Y+adjustTextDown+(pixelsPerLine/scaleToPix)*lineIdx),
					(float)xbounds.Width+50,//any amount of extra padding here will not cause malfunction
					0);//layoutH);
				XStringFormat sf=XStringFormats.Default;
				sf.Alignment=sa;
				//sf.LineAlignment= XLineAlignment.Near;
				//xg.DrawString(str.Substring(ix,chars),xfont,xbrush,layoutRectangle,sf);
				//There is currently a problem with printing the tab character '\t' when using XStringFormat.
				//C#'s StringFormat has a method called SetTabStops() which can be used to get the tabs to be drawn (see regular printing above).
				//We're doing nothing for now because the current complaint is only for printing, not PDF creation.  
				//A workaround is to not use tabs and to instead use separate static text fields that are spaced out as desired.
				xg.DrawString(str.Substring(ix,chars),xfont,xbrush,(double)layoutRectangle.Left,(double)layoutRectangle.Top,sf);
				lineIdx+=1;
			}
		}

		public static int MeasureStringH(Graphics g,string text,Font font,int width) {
			return (int)MeasureString(g,text,font,width).Height;
		}

		///<summary>This also differs from the regular MeasureString in that it will correctly measure trailing carriage returns as requiring another line.</summary>
		public static SizeF MeasureString(Graphics g,string text,Font font,int width) {
			StringFormat format=StringFormat.GenericTypographic;
			float pixelsPerLine=LineSpacingForFont(font.Name) * (float)font.Height;
			int chars;
			int lines;
			SizeF fit=new SizeF(width-rightPad,float.MaxValue);//arbitrarily large height
			g.MeasureString(text,font,fit,format,out chars,out lines);
			float h=topPad + ((float)lines)*pixelsPerLine;
			if(text.EndsWith("\n")) {
				h+=font.Height;//add another line to handle the trailing Carriage return.
			}
			return new SizeF((float)width,h);
		}


	}
}
