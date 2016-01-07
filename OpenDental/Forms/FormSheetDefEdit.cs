using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
#warning remove linq.
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormSheetDefEdit:Form {
		public SheetDef SheetDefCur;
		public bool IsInternal;
		private bool MouseIsDown;
		private bool CtrlIsDown;
		private Point MouseOriginalPos;
		private Point MouseCurrentPos;
		private List<Point> OriginalControlPositions;
		///<summary>When you first mouse down, if you clicked on a valid control, this will be false.  For drag selection, this must be true.</summary>
		private bool ClickedOnBlankSpace;
		private bool AltIsDown;
		///<summary>This is our 'clipboard' for copy/paste of fields.</summary>
		private List<SheetFieldDef> ListSheetFieldDefsCopyPaste;
		private int PasteOffset=0;
		///<summary>After each 10 pastes to the upper left origin, this increments 10 to shift the next 10 down.</summary>
		private int PasteOffsetY=0;
		private bool IsTabMode;
		private List<SheetFieldDef> ListSheetFieldDefsTabOrder;
		public static Font tabOrderFont = new Font("Times New Roman",12f,FontStyle.Regular,GraphicsUnit.Pixel);
		private Bitmap BmBackground;
		private Graphics GraphicsBackground;
		///<summary>This stores the previous calculations so that we don't have to recal unless certain things have changed.  The key is the index of the sheetfield.  The data is an array of objects of different types as seen in the code.</summary>
		private Hashtable HashRtfStringCache=new Hashtable();
		///<summary>Arguments to draw fields such as pens, brushes, and fonts.</summary>
		private DrawFieldArgs _argsDF;
		///<summary>Only used here to draw the dashed margin lines.</summary>
		private System.Drawing.Printing.Margins _printMargin=new System.Drawing.Printing.Margins(0,0,40,60);
		private Image _toothChart;

		///<summary>Some controls (panels in this case) do not pass key events to the parent (the form in this case) even when the property KeyPreview is set.  Instead, the default key functionality occurs.  An example would be the arrow keys.  By default, arrow keys set focus to the "next" control.  Instead, want all key presses on this form and all of it's child controls to always call the FormSheetDefEdit_KeyDown method.</summary>
		protected override bool ProcessCmdKey(ref Message msg,Keys keyData) {
			FormSheetDefEdit_KeyDown(this,new KeyEventArgs(keyData));
			return true;//This indicates that all keys have been processed.
			//return base.ProcessCmdKey(ref msg,keyData);//We don't need this right now, because no textboxes, for example.
		}

		public FormSheetDefEdit(SheetDef sheetDef) {
			InitializeComponent();
			Lan.F(this);
			SheetDefCur=sheetDef;
			/*if(SheetDefCur.IsLandscape){
				Width=SheetDefCur.Height+185;
				Height=SheetDefCur.Width+60;
			}
			else{
				Width=SheetDefCur.Width+185;
				Height=SheetDefCur.Height+60;
			}*/
			if(sheetDef.IsLandscape){
				Width=sheetDef.Height+190;
				Height=sheetDef.Width+65;
			}
			else{
				Width=sheetDef.Width+190;
				Height=sheetDef.Height+65;
			}
			if(Width<600){
				Width=600;
			}
			if(Height<600){
				Height=600;
			}
			if(Width>SystemInformation.WorkingArea.Width){
				Width=SystemInformation.WorkingArea.Width;
			}
			if(Height>SystemInformation.WorkingArea.Height){
				Height=SystemInformation.WorkingArea.Height;
			}
		}

		private void FormSheetDefEdit_Load(object sender,EventArgs e) {
			if(IsInternal) {
				butDelete.Visible=false;
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
				groupAddNew.Visible=false;
				groupPage.Visible=false;
				groupAlignH.Visible=false;
				groupAlignV.Visible=false;
				linkLabelTips.Visible=false;
				butCopy.Visible=false;
				butPaste.Visible=false;
				butTabOrder.Visible=false;
			}
			else {
				labelInternal.Visible=false;
			}
			if(SheetDefCur.SheetType!=SheetTypeEnum.Statement 
				&& SheetDefCur.SheetType!=SheetTypeEnum.MedLabResults
				&& SheetDefCur.SheetType!=SheetTypeEnum.TreatmentPlan) 
			{
				butAddGrid.Visible=false;
			}
			if(Sheets.SheetTypeIsSinglePage(SheetDefCur.SheetType)) {
				groupPage.Visible=false;
			}
			if(SheetDefCur.SheetType==SheetTypeEnum.DepositSlip || SheetDefCur.SheetType==SheetTypeEnum.LabelCarrier) {
				butAddSpecial.Enabled=false; //grey out button
			}
			if(SheetDefCur.IsLandscape) {
				panelMain.Width=SheetDefCur.Height;
				panelMain.Height=SheetDefCur.Width;
			}
			else {
				panelMain.Width=SheetDefCur.Width;
				panelMain.Height=SheetDefCur.Height;
			}
			if(SheetDefCur.SheetType==SheetTypeEnum.TreatmentPlan) {
				butAddSpecial.Visible=true;
				_toothChart=GetToothChartHelper(0);
			}
			textDescription.Text=SheetDefCur.Description;
			panelMain.Height=SheetDefCur.HeightTotal;
			FillFieldList();
			RefreshDoubleBuffer();
			panelMain.Refresh();
			panelMain.Focus();
			//textDescription.Focus();
		}

		private void FillFieldList() {
			listFields.Items.Clear();
			string txt;
			SheetDefCur.SheetFieldDefs.Sort(CompareTabOrder);
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				switch(SheetDefCur.SheetFieldDefs[i].FieldType) {
					case SheetFieldType.StaticText:
						listFields.Items.Add(SheetDefCur.SheetFieldDefs[i].FieldValue);
						break;
					case SheetFieldType.Image:
						listFields.Items.Add(Lan.g(this,"Image:")+SheetDefCur.SheetFieldDefs[i].FieldName);
						break;
					case SheetFieldType.PatImage:
						listFields.Items.Add(Lan.g(this,"PatImg:")+DefC.GetName(DefCat.ImageCats,PIn.Long(SheetDefCur.SheetFieldDefs[i].FieldName)));
						break;
					case SheetFieldType.Line:
						listFields.Items.Add(Lan.g(this,"Line:")+SheetDefCur.SheetFieldDefs[i].XPos.ToString()+","+SheetDefCur.SheetFieldDefs[i].YPos.ToString()+","+"W:"+SheetDefCur.SheetFieldDefs[i].Width.ToString()+","+"H:"+SheetDefCur.SheetFieldDefs[i].Height.ToString());
						break;
					case SheetFieldType.Rectangle:
						listFields.Items.Add(Lan.g(this,"Rect:")+SheetDefCur.SheetFieldDefs[i].XPos.ToString()+","+SheetDefCur.SheetFieldDefs[i].YPos.ToString()+","+"W:"+SheetDefCur.SheetFieldDefs[i].Width.ToString()+","+"H:"+SheetDefCur.SheetFieldDefs[i].Height.ToString());
						break;
					case SheetFieldType.SigBox:
						listFields.Items.Add(Lan.g(this,"Signature Box"));
						break;
					case SheetFieldType.CheckBox:
						txt=SheetDefCur.SheetFieldDefs[i].TabOrder.ToString()+": ";
						if(SheetDefCur.SheetFieldDefs[i].FieldName.StartsWith("allergy:") || SheetDefCur.SheetFieldDefs[i].FieldName.StartsWith("problem:")) {
							txt+=SheetDefCur.SheetFieldDefs[i].FieldName.Remove(0,8);
						}
						else {
							txt+=SheetDefCur.SheetFieldDefs[i].FieldName;
						}
						if(SheetDefCur.SheetFieldDefs[i].RadioButtonValue!="") {
							txt+=" - "+SheetDefCur.SheetFieldDefs[i].RadioButtonValue;
						}
						listFields.Items.Add(txt);
						break;
					case SheetFieldType.InputField:
						listFields.Items.Add(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString()+": "+SheetDefCur.SheetFieldDefs[i].FieldName);
						break;
					case SheetFieldType.Grid:
						listFields.Items.Add("Grid:"+SheetDefCur.SheetFieldDefs[i].FieldName);
						break;
					default:
						listFields.Items.Add(SheetDefCur.SheetFieldDefs[i].FieldName);
						break;
				} //end switch
			}
		}

		///<summary>This is a comparator function used by List&lt;T&gt;.Sort() 
		///When compairing SheetFieldDef.TabOrder it returns a negative number if def1&lt;def2, 0 if def1==def2, and a positive number if def1&gt;def2.
		///Does not handle null values, but there should never be any instances of null being passed in. 
		///Must always return 0 when compairing item to itself.
		///This function should probably be moved to SheetFieldDefs.</summary>
		private static int CompareTabOrder(SheetFieldDef def1,SheetFieldDef def2) {
			if(def1.FieldType==def2.FieldType) {
				//do nothing
			}
			else if(def1.FieldType==SheetFieldType.Image) { //Always move images to the top of the list. This is because of the way the sheet is drawn.
				return -1;
			}
			else if(def2.FieldType==SheetFieldType.Image) { //Always move images to the top of the list. This is because of the way the sheet is drawn.
				return 1;
			}
			else if(def1.FieldType==SheetFieldType.PatImage) { //Move PatImage to the top of the list under images.
				return -1;
			}
			else if(def2.FieldType==SheetFieldType.PatImage) { //Move PatImage to the top of the list under images.
				return 1;
			}
			else if(def1.FieldType==SheetFieldType.Special) { //Move Special to the top of the list under PatImages.
				return -1;
			}
			else if(def2.FieldType==SheetFieldType.Special) { //Move Special to the top of the list under PatImages.
				return 1;
			}
			else if(def1.FieldType==SheetFieldType.OutputText) { //Move Output text to the top of the list under Special.
				return -1;
			}
			else if(def2.FieldType==SheetFieldType.OutputText) { //Move Output text to the top of the list under Special.
				return 1;
			}
			if(def1.TabOrder-def2.TabOrder==0) {
				int comp=(def1.FieldName+def1.RadioButtonValue).CompareTo(def2.FieldName+def2.RadioButtonValue); //RadioButtionValuecan be filled or ""
				if(comp!=0) {
					return comp;
				}
				comp=def1.YPos-def2.YPos; //arbitrarily order by YPos if both controls have the same tab orer and name. This will only happen if both fields are either identical or if they are both misc fields.
				if(comp!=0) {
					return comp;
				}
				return def1.XPos-def2.XPos; //If tabOrder, Name, and YPos are equal then compare based on X coordinate. 
			}
			return def1.TabOrder-def2.TabOrder;
		}

		private void panelMain_Paint(object sender,PaintEventArgs e) {
			Bitmap doubleBuffer=new Bitmap(panelMain.Width,panelMain.Height);
			Graphics g=Graphics.FromImage(doubleBuffer);
			g.DrawImage(BmBackground,0,0);
			DrawFields(g,false);
			e.Graphics.DrawImage(doubleBuffer,0,0);
			g.Dispose();
			doubleBuffer.Dispose();
			doubleBuffer=null;
		}

		///<summary>Whenever a user might have edited or moved a background image, this gets called.</summary>
		private void RefreshDoubleBuffer() {
			GraphicsBackground.FillRectangle(Brushes.White,0,0,BmBackground.Width,BmBackground.Height);
			DrawFields(GraphicsBackground,true);
		}

		///<summary>If drawImages is true then only image fields will be drawn. Otherwise, all fields but images will be drawn.</summary>
		private void DrawFields(Graphics g,bool onlyDrawImages) {
			//SheetDefCur.SheetFieldDefs.Sort(SheetFieldDefs.SortDrawingOrderLayers);// for field selection bug
			SetGraphicsHelper(g);
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				if(onlyDrawImages) {
					if(SheetDefCur.SheetFieldDefs[i].FieldType==SheetFieldType.Image) {
						DrawImagesHelper(g,i);
					}
					continue;
				} //end onlyDrawImages
				switch(SheetDefCur.SheetFieldDefs[i].FieldType) {
					case SheetFieldType.Parameter: //Skip
					case SheetFieldType.Image: //Handled above
						continue;
					case SheetFieldType.PatImage:
						DrawPatImageHelper(g,i);
						continue;
					case SheetFieldType.Line:
						DrawLineHelper(g,i);
						continue;
					case SheetFieldType.Rectangle:
						DrawRectangleHelper(g,i);
						continue;
					case SheetFieldType.CheckBox:
						DrawCheckBoxHelper(g,i);
						continue;
					case SheetFieldType.SigBox:
						DrawSigBoxHelper(g,i);
						continue;
					case SheetFieldType.Special:
						DrawSpecialHelper(g,i);
						continue;
					case SheetFieldType.Grid:
						DrawGridHelper(g,i);
						continue;
					case SheetFieldType.InputField:
					case SheetFieldType.StaticText:
					case SheetFieldType.OutputText:
					default:
						DrawStringHelper(g,i);
						DrawTabModeHelper(g,i);
						//throw new ApplicationException("Unsupported sheet field type : "+SheetDefCur.SheetFieldDefs[i].FieldType.ToString());
						continue;
				} //end switch
			}
			DrawSelectionRectangle(g);
			//Draw pagebreak
			Pen pDashPage=new Pen(Color.Green);
			pDashPage.DashPattern=new float[] {4.0F,3.0F,2.0F,3.0F};
			Pen pDashMargin=new Pen(Color.Green);
			pDashMargin.DashPattern=new float[] {1.0F,5.0F};
			int margins=(_printMargin.Top+_printMargin.Bottom);
			for(int i=1;i<SheetDefCur.PageCount;i++) {
				//g.DrawLine(pDashMargin,0,i*SheetDefCur.HeightPage-_printMargin.Bottom,SheetDefCur.WidthPage,i*SheetDefCur.HeightPage-_printMargin.Bottom);
				g.DrawLine(pDashPage,0,i*(SheetDefCur.HeightPage-margins)+_printMargin.Top,SheetDefCur.WidthPage,i*(SheetDefCur.HeightPage-margins)+_printMargin.Top);
				//g.DrawLine(pDashMargin,0,i*SheetDefCur.HeightPage+_printMargin.Top,SheetDefCur.WidthPage,i*SheetDefCur.HeightPage+_printMargin.Top);
			}
			//End Draw Page Break
		}

		#region DrawFields Helpers (In Alphabetical Order)
		private void SetGraphicsHelper(Graphics g) {
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.CompositingQuality=CompositingQuality.HighQuality; //This has to be here or the line thicknesses are wrong.
			if(_argsDF!=null) {
				_argsDF.Dispose();
			}
			_argsDF=new DrawFieldArgs(); //reset _argsDF
		}

		private void DrawCheckBoxHelper(Graphics g,int i) {
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRedThick;
			}
			else {
				_argsDF.pen=_argsDF.penBlueThick;
			}
			//g.DrawRectangle(pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,
			//	SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
			g.DrawLine(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].XPos+SheetDefCur.SheetFieldDefs[i].Width-1,SheetDefCur.SheetFieldDefs[i].YPos+SheetDefCur.SheetFieldDefs[i].Height-1);
			g.DrawLine(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos+SheetDefCur.SheetFieldDefs[i].Width-1,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos+SheetDefCur.SheetFieldDefs[i].Height-1);
			if(IsTabMode) {
				Rectangle tabRect=new Rectangle(SheetDefCur.SheetFieldDefs[i].XPos-1, //X
					SheetDefCur.SheetFieldDefs[i].YPos-1, //Y
					(int)g.MeasureString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont).Width+1, //Width
					12); //height
				if(ListSheetFieldDefsTabOrder.Contains(SheetDefCur.SheetFieldDefs[i])) { //blue border, white box, blue letters
					g.FillRectangle(Brushes.White,tabRect);
					g.DrawRectangle(Pens.Blue,tabRect);
					g.DrawString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont,Brushes.Blue,tabRect.X,tabRect.Y-1);
					//GraphicsHelper.DrawString(g,g,SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),SheetDefCur.GetFont(),Brushes.Blue,tabRect);
				}
				else { //Blue border, blue box, white letters
					g.FillRectangle(_argsDF.brushBlue,tabRect);
					g.DrawString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont,Brushes.White,tabRect.X,tabRect.Y-1);
					//GraphicsHelper.DrawString(g,g,SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),SheetDefCur.GetFont(),Brushes.White,tabRect);
				}
			}
		}

		private void DrawGridHelper(Graphics g,int i) {
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlack;
				_argsDF.brush=_argsDF.brushBlue;
			}
			List<DisplayField> columns=SheetUtil.GetGridColumnsAvailable(SheetDefCur.SheetFieldDefs[i].FieldName);
			//fGrid.Columns=SheetGridDefs.GetColumnsAvailable(fGrid.GridType);
			ODGrid odGrid=new ODGrid();
			odGrid.Width=0;
			//SheetDefCur.SheetFieldDefs[i].Width=0;
			for(int c=0;c<columns.Count;c++) {
				odGrid.Width+=columns[c].ColumnWidth;
				//SheetDefCur.SheetFieldDefs[i].Width+=Columns[c].ColumnWidth;
			}
			odGrid.HideScrollBars=true;

			#region  Fill Grid

			odGrid.BeginUpdate();
			odGrid.Columns.Clear();
			ODGridColumn col;
			for(int c=0;c<columns.Count;c++) {
				col=new ODGridColumn(columns[c].Description,columns[c].ColumnWidth);
				odGrid.Columns.Add(col);
			}
			ODGridRow row=new ODGridRow(); //Add dummy row
			for(int c=0;c<columns.Count;c++) {
				row.Cells.Add(" "); //add dummy row.
			}
			odGrid.Rows.Add(row);
			odGrid.EndUpdate(); //Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			#endregion

			int yPosGrid=SheetDefCur.SheetFieldDefs[i].YPos;
			if(SheetDefCur.SheetFieldDefs[i].FieldName=="StatementPayPlan") {
				SizeF sSize=g.MeasureString("Payment Plans",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
				g.FillRectangle(Brushes.White,SheetDefCur.SheetFieldDefs[i].XPos,yPosGrid,odGrid.Width,odGrid.TitleHeight);
				g.DrawString("Payment Plans",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),SheetDefCur.SheetFieldDefs[i].XPos+(SheetDefCur.SheetFieldDefs[i].Width-sSize.Width)/2,yPosGrid);
				yPosGrid+=odGrid.TitleHeight;
			}
			if(SheetDefCur.SheetFieldDefs[i].FieldName=="TreatPlanBenefitsFamily") {
				SizeF sSize=g.MeasureString("Family Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
				g.FillRectangle(Brushes.White,SheetDefCur.SheetFieldDefs[i].XPos,yPosGrid,odGrid.Width,odGrid.TitleHeight);
				g.DrawString("Family Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),Brushes.Black,SheetDefCur.SheetFieldDefs[i].XPos+(SheetDefCur.SheetFieldDefs[i].Width-sSize.Width)/2,yPosGrid);
				yPosGrid+=odGrid.TitleHeight;
			}
			if(SheetDefCur.SheetFieldDefs[i].FieldName=="TreatPlanBenefitsIndividual") {
				SizeF sSize=g.MeasureString("Individual Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold));
				g.FillRectangle(Brushes.White,SheetDefCur.SheetFieldDefs[i].XPos,yPosGrid,odGrid.Width,odGrid.TitleHeight);
				g.DrawString("Individual Insurance Benefits",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),Brushes.Black,SheetDefCur.SheetFieldDefs[i].XPos+(SheetDefCur.SheetFieldDefs[i].Width-sSize.Width)/2,yPosGrid);
				yPosGrid+=odGrid.TitleHeight;
			}
			odGrid.PrintHeader(g,SheetDefCur.SheetFieldDefs[i].XPos,yPosGrid);
			yPosGrid+=odGrid.HeaderHeight;
			odGrid.PrintRow(0,g,SheetDefCur.SheetFieldDefs[i].XPos,yPosGrid,false,true); //a single dummy row.
			yPosGrid+=odGrid.RowHeights[0]+2;

			#region drawFooter
			if(SheetDefCur.SheetFieldDefs[i].FieldName=="StatementPayPlan") {
				RectangleF rf=new RectangleF(SheetDefCur.Width-SheetDefCur.SheetFieldDefs[i].Width-60,yPosGrid,SheetDefCur.SheetFieldDefs[i].Width,odGrid.TitleHeight);
				g.FillRectangle(Brushes.White,rf);
				StringFormat sf=new StringFormat();
				sf.Alignment=StringAlignment.Far;
				g.DrawString("Payment Plan Amount Due: "+"0.00",new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
			}
			#endregion

			if(listFields.SelectedIndices.Contains(i)) {
				columns=SheetUtil.GetGridColumnsAvailable(SheetDefCur.SheetFieldDefs[i].FieldName);
				SheetDefCur.SheetFieldDefs[i].Width=0;
				for(int c=0;c<columns.Count;c++) {
					SheetDefCur.SheetFieldDefs[i].Width+=columns[c].ColumnWidth;
				}
				g.DrawRectangle(_argsDF.penRedThick,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
			}
			//g.DrawRectangle(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,
			//	SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
			//g.DrawString("Grid:"+SheetGridDefs.GetName(SheetDefCur.SheetFieldDefs[i].GridDef),Font,_argsDF.brush,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos);
		}

		private void DrawImagesHelper(Graphics g,int i) {
			string filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),SheetDefCur.SheetFieldDefs[i].FieldName);
			Image img=null;
			if(SheetDefCur.SheetFieldDefs[i].FieldName=="Patient Info.gif") {
				img=OpenDentBusiness.Properties.Resources.Patient_Info;
			}
			else if(File.Exists(filePathAndName)) {
				img=Image.FromFile(filePathAndName);
			}
			else {
#if DEBUG
				g.DrawRectangle(new Pen(Brushes.IndianRed),SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
				g.DrawString("Cannot find image: "+SheetDefCur.SheetFieldDefs[i].FieldName,Font,_argsDF.brush??Brushes.Black,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos);
#endif
				return;
			}
			g.DrawImage(img,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
#if DEBUG
			g.DrawRectangle(new Pen(Brushes.IndianRed),SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
#endif
			if(img!=null) {
				img.Dispose();
			}
		}

		private void DrawLineHelper(Graphics g,int i) {
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
			}
			else {
				_argsDF.penLine.Color=SheetDefCur.SheetFieldDefs[i].ItemColor;
				_argsDF.pen=_argsDF.penLine;
			}
			g.DrawLine(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].XPos+SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].YPos+SheetDefCur.SheetFieldDefs[i].Height);
		}

		private void DrawPatImageHelper(Graphics g,int i) {
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlack;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
			g.DrawString("PatImage: "+DefC.GetName(DefCat.ImageCats,PIn.Long(SheetDefCur.SheetFieldDefs[i].FieldName)),Font /*NOT _argsDF.font*/,_argsDF.brush,SheetDefCur.SheetFieldDefs[i].XPos+1,SheetDefCur.SheetFieldDefs[i].YPos+1);
		}

		private void DrawRectangleHelper(Graphics g,int i) {
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlack;
			}
			g.DrawRectangle(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
		}

		private void DrawSelectionRectangle(Graphics g) {
			if(ClickedOnBlankSpace) {
				g.DrawRectangle(_argsDF.penSelection,
					//The math functions are used below to account for users clicking and dragging up, down, left, or right.
					Math.Min(MouseOriginalPos.X,MouseCurrentPos.X), //X
					Math.Min(MouseOriginalPos.Y,MouseCurrentPos.Y), //Y
					Math.Abs(MouseCurrentPos.X-MouseOriginalPos.X), //Width
					Math.Abs(MouseCurrentPos.Y-MouseOriginalPos.Y)); //Height
			}
		}

		private void DrawSigBoxHelper(Graphics g,int i) {
			//font=new Font(Font,
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			g.DrawRectangle(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
			g.DrawString("(signature box)",Font,_argsDF.brush,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos);
		}

		private void DrawSpecialHelper(Graphics g,int i) {
			//TODO:
			if(listFields.SelectedIndices.Contains(i)) {
				_argsDF.pen=_argsDF.penRed;
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				_argsDF.pen=_argsDF.penBlue;
				_argsDF.brush=_argsDF.brushBlue;
			}
			switch(SheetDefCur.SheetFieldDefs[i].FieldName) {
				case "toothChart":
					Rectangle boundingBox=new Rectangle(SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
					float widthFactor=(float)boundingBox.Width/(float)_toothChart.Width;
					float heightFactor=(float)boundingBox.Height/(float)_toothChart.Height;
					int x,y,width,height;
					if(widthFactor<heightFactor) {
						//use width factor
						//img width will equal box width
						//offset height.
						x=SheetDefCur.SheetFieldDefs[i].XPos;
						y=SheetDefCur.SheetFieldDefs[i].YPos+(SheetDefCur.SheetFieldDefs[i].Height-(int)(_toothChart.Height*widthFactor))/2;
						height=(int)(_toothChart.Height*widthFactor);
						width=SheetDefCur.SheetFieldDefs[i].Width+1; //+1 to include the pixels
					}
					else {
						//use height factor
						//img height will equal box height
						//offset width
						x=SheetDefCur.SheetFieldDefs[i].XPos+(SheetDefCur.SheetFieldDefs[i].Width-(int)(_toothChart.Width*heightFactor))/2;
						y=SheetDefCur.SheetFieldDefs[i].YPos;
						height=SheetDefCur.SheetFieldDefs[i].Height+1;
						width=(int)(_toothChart.Width*heightFactor);
					}
					g.DrawImage(_toothChart,new Rectangle(x,y,width,height));
					g.DrawRectangle(Pens.LightGray,x,y,width,height); //outline tooth grid so user can see how much wasted space there is.
					break;
				case "toothChartLegend":
					using(Brush brushEx=new SolidBrush(DefC.Short[(int)DefCat.ChartGraphicColors][3].ItemColor)) 
					using(Brush brushEc=new SolidBrush(DefC.Short[(int)DefCat.ChartGraphicColors][2].ItemColor)) 
					using(Brush brushCo=new SolidBrush(DefC.Short[(int)DefCat.ChartGraphicColors][1].ItemColor)) 
					using(Brush brushRo=new SolidBrush(DefC.Short[(int)DefCat.ChartGraphicColors][4].ItemColor)) 
					using(Brush brushTp=new SolidBrush(DefC.Short[(int)DefCat.ChartGraphicColors][0].ItemColor)) 
					using(Font bodyFont=new Font("Arial",9f,FontStyle.Regular,GraphicsUnit.Point)) {
						float yPos=SheetDefCur.SheetFieldDefs[i].YPos;
						//calculate xPos as the point that will result in the legend being centered on the page.
						float xPos=0.5f*(SheetDefCur.Width-(g.MeasureString(Lan.g("ContrTreat","Existing"),bodyFont).Width+g.MeasureString(Lan.g("ContrTreat","Complete"),bodyFont).Width+g.MeasureString(Lan.g("ContrTreat","Referred Out"),bodyFont).Width+g.MeasureString(Lan.g("ContrTreat","Treatment Planned"),bodyFont).Width+123)); //inter-field spacing
						//Existing
						g.FillRectangle(brushEx,xPos,yPos,14,14);
						g.DrawString(Lan.g("ContrTreat","Existing"),bodyFont,Brushes.Black,xPos+16,yPos);
						xPos+=g.MeasureString(Lan.g("ContrTreat","Existing"),bodyFont).Width+23+16;
						//Complete/ExistingComplete
						g.FillRectangle(brushCo,xPos,yPos,7,14);
						g.FillRectangle(brushEc,xPos+7,yPos,7,14);
						g.DrawString(Lan.g("ContrTreat","Complete"),bodyFont,Brushes.Black,xPos+16,yPos);
						xPos+=g.MeasureString(Lan.g("ContrTreat","Complete"),bodyFont).Width+23+16;
						//ReferredOut
						g.FillRectangle(brushRo,xPos,yPos,14,14);
						g.DrawString(Lan.g("ContrTreat","Referred Out"),bodyFont,Brushes.Black,xPos+16,yPos);
						xPos+=g.MeasureString(Lan.g("ContrTreat","Referred Out"),bodyFont).Width+23+16;
						//TreatmentPlanned
						g.FillRectangle(brushTp,xPos,yPos,14,14);
						g.DrawString(Lan.g("ContrTreat","Treatment Planned"),bodyFont,Brushes.Black,xPos+16,yPos);
					}
					break;
				default:
					g.DrawString("(Special:Tooth Grid)",Font,_argsDF.brush,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos);
					break;
			} //end switch
			//draw rectangle on top of special so that user can see how big the field actually is.
			g.DrawRectangle(_argsDF.pen,SheetDefCur.SheetFieldDefs[i].XPos,SheetDefCur.SheetFieldDefs[i].YPos,SheetDefCur.SheetFieldDefs[i].Width,SheetDefCur.SheetFieldDefs[i].Height);
		}

		private Image GetToothChartHelper(long patNum) {
			//linesPrinted=0;
			double[] colTotal=new double[10];
			//headingPrinted=false;
			//graphicsPrinted=false;
			//mainPrinted=false;
			//benefitsPrinted=false;
			//notePrinted=false;
			//pagesPrinted=0;
			//prints the graphical tooth chart and legend
			//Panel panelHide=new Panel();
			//panelHide.Size=new Size(600,500);
			//panelHide.BackColor=this.BackColor;
			//panelHide.SendToBack();
			//this.Controls.Add(panelHide);
			SparksToothChart.ToothChartWrapper toothChart=new SparksToothChart.ToothChartWrapper();
			toothChart.ColorBackground=DefC.Long[(int)DefCat.ChartGraphicColors][14].ItemColor;
			toothChart.ColorText=DefC.Long[(int)DefCat.ChartGraphicColors][15].ItemColor;
			//toothChart.TaoRenderEnabled=true;
			//toothChart.TaoInitializeContexts();
			toothChart.Size=new Size(500,370);
			//toothChart.Location=new Point(-600,-500);//off the visible screen
			//toothChart.SendToBack();
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			toothChart.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
			toothChart.DeviceFormat=new SparksToothChart.ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			//Must be last setting set for preferences, because
			//this is the line where the device pixel format is
			//recreated.
			//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
			toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			//this.Controls.Add(toothChart);
			//toothChart.BringToFront();
			toothChart.ResetTeeth();
			List<ToothInitial> ToothInitialList=patNum==0?new List<ToothInitial>():ToothInitials.Refresh(patNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++) {
				switch(ToothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						toothChart.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			//ComputeProcListFiltered();
			//DrawProcsGraphics();
			toothChart.AutoFinish=true;
			Image chartBitmap=toothChart.GetBitmap();
			toothChart.Dispose();
			return chartBitmap;
		}

		private void DrawStringHelper(Graphics g,int i) {
			_argsDF.fontstyle=FontStyle.Regular;
			if(SheetDefCur.SheetFieldDefs[i].FontIsBold) {
				_argsDF.fontstyle=FontStyle.Bold;
			}
			_argsDF.font=new Font(SheetDefCur.SheetFieldDefs[i].FontName,SheetDefCur.SheetFieldDefs[i].FontSize,_argsDF.fontstyle,GraphicsUnit.Point);
			if(listFields.SelectedIndices.Contains(i)) {
				g.DrawRectangle(_argsDF.penRed,SheetDefCur.SheetFieldDefs[i].Bounds);
				_argsDF.brush=_argsDF.brushRed;
			}
			else {
				g.DrawRectangle(_argsDF.penBlue,SheetDefCur.SheetFieldDefs[i].Bounds);
				_argsDF.brush=_argsDF.brushBlue;
			}
			string str;
			if(SheetDefCur.SheetFieldDefs[i].FieldType==SheetFieldType.StaticText) {
				str=SheetDefCur.SheetFieldDefs[i].FieldValue;
				//g.DrawString(SheetDefCur.SheetFieldDefs[i].FieldValue,font,
				//	brush,SheetDefCur.SheetFieldDefs[i].Bounds);
				//Static text can have a custom color.
				//Check to see if this text box is selected.  If it is, do not change the color.
				if(!listFields.SelectedIndices.Contains(i)) {
					_argsDF.brushText.Color=SheetDefCur.SheetFieldDefs[i].ItemColor;
					_argsDF.brush=_argsDF.brushText;
				}
			}
			else {
				str=SheetDefCur.SheetFieldDefs[i].FieldName;
				//g.DrawString(SheetDefCur.SheetFieldDefs[i].FieldName,font,
				//	brush,SheetDefCur.SheetFieldDefs[i].Bounds);
			}
			//g.DrawString(str,font,brush,SheetDefCur.SheetFieldDefs[i].Bounds);//This was drawing differently than in RichTextBox, so problems with large text.
			DrawRTFstring(i,str,_argsDF.font,_argsDF.brush,g);
		}

		private void DrawTabModeHelper(Graphics g,int i) {
			if(!IsTabMode || SheetDefCur.SheetFieldDefs[i].FieldType!=SheetFieldType.InputField) {
				return;
			}
			Rectangle tabRect=new Rectangle(SheetDefCur.SheetFieldDefs[i].XPos-1, //X
				SheetDefCur.SheetFieldDefs[i].YPos-1, //Y
				(int)g.MeasureString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont).Width+1, //Width
				12); //height
			if(ListSheetFieldDefsTabOrder.Contains(SheetDefCur.SheetFieldDefs[i])) { //blue border, white box, blue letters
				g.FillRectangle(Brushes.White,tabRect);
				g.DrawRectangle(Pens.Blue,tabRect);
				g.DrawString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont,Brushes.Blue,tabRect.X,tabRect.Y-1);
				//GraphicsHelper.DrawString(g,g,SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),SheetDefCur.GetFont(),Brushes.Blue,tabRect);
			}
			else { //Blue border, blue box, white letters
				g.FillRectangle(_argsDF.brushBlue,tabRect);
				g.DrawString(SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),tabOrderFont,Brushes.White,tabRect.X,tabRect.Y-1);
				//GraphicsHelper.DrawString(g,g,SheetDefCur.SheetFieldDefs[i].TabOrder.ToString(),SheetDefCur.GetFont(),Brushes.White,tabRect);
			}
		}

		#endregion Draw Sheet Field Helpers

		///<summary>We need this special function to draw strings just like the RichTextBox control does, because sheet text is displayed using RichTextBoxes within FormSheetFillEdit.
		///Graphics.DrawString() uses a different font spacing than the RichTextBox control does.</summary>
		private void DrawRTFstring(int index,string str,Font font,Brush brush,Graphics g) {
			str=str.Replace("\r",""); //For some reason '\r' throws off character position calculations.  \n still handles the CRs.
			SheetFieldDef field=SheetDefCur.SheetFieldDefs[index];
			//Font spacing is different for g.DrawString() as compared to RichTextBox and TextBox controls.
			//We create a RichTextBox here in the same manner as in FormSheetFillEdit, but we only use it to determine where to draw text.
			//We do not add the RichTextBox control to this form, because its background will overwrite everything behind that we have already drawn.
			bool doCalc=true;
			object[] data=(object[])HashRtfStringCache[index.ToString()];
			if(data!=null) { //That field has been calculated
				//If any of the following factors change, then that could potentially change text positions.
				if(field.FontName.CompareTo(data[1])==0 //Has font name changed since last pass?
				   && field.FontSize.CompareTo(data[2])==0 //Has font size changed since last pass?
				   && field.FontIsBold.CompareTo(data[3])==0 //Has font boldness changed since last pass?
				   && field.Width.CompareTo(data[4])==0 //Has field width changed since last pass?
				   && field.Height.CompareTo(data[5])==0 //Has field height changed since last pass?
				   && str.CompareTo(data[6])==0 //Has field text changed since last pass?
				   && field.TextAlign.CompareTo(data[7])==0) //Has field text align changed since last pass?
				{
					doCalc=false; //Nothing has changed. Do not recalculate.
				}
			}
			if(doCalc) { //Data has not yet been cached for this text field, or the field has changed and needs to be recalculated.
				//All of these textbox fields are set using the same logic as in FormSheetFillEdit, so that text in this form matches exaclty.
				RichTextBox textbox=new RichTextBox();
				textbox.Visible=false;
				textbox.BorderStyle=BorderStyle.None;
				textbox.ScrollBars=RichTextBoxScrollBars.None;
				textbox.SelectionAlignment=field.TextAlign;
				textbox.Location=new Point(field.XPos,field.YPos);
				textbox.Width=field.Width;
				textbox.Height=field.Height;
				textbox.Font=font;
				textbox.ForeColor=((SolidBrush)brush).Color;
				if(field.Height<textbox.Font.Height+2) { //Same logic as FormSheetFillEdit.
					textbox.Multiline=false;
				}
				else {
					textbox.Multiline=true;
				}
				textbox.Text=str;
				Point[] positions=new Point[str.Length];
				for(int j=0;j<str.Length;j++) {
					positions[j]=textbox.GetPositionFromCharIndex(j); //This line is slow, so we try to minimize calling it by chaching positions each time there are changes.
				}
				textbox.Dispose();
				data=new object[] {positions,field.FontName,field.FontSize,field.FontIsBold,field.Width,field.Height,str,field.TextAlign};
				HashRtfStringCache[index.ToString()]=data;
			}
			Point[] charPositions=(Point[])data[0];
			for(int j=0;j<charPositions.Length;j++) { //This will draw text below the bottom line if the text is long. This is by design, so the user can see that the text is too big.
				g.DrawString(str.Substring(j,1),font,brush,field.Bounds.X+charPositions[j].X,field.Bounds.Y+charPositions[j].Y);
			}
		}

		private void butEdit_Click(object sender,EventArgs e) {
			FormSheetDef FormS=new FormSheetDef();
			FormS.SheetDefCur=SheetDefCur;
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			textDescription.Text=SheetDefCur.Description;
			//resize
			if(SheetDefCur.IsLandscape) {
				panelMain.Width=SheetDefCur.Height;
				panelMain.Height=SheetDefCur.Width;
			}
			else {
				panelMain.Width=SheetDefCur.Width;
				panelMain.Height=SheetDefCur.Height;
			}
			panelMain.Height=SheetDefCur.HeightTotal-(SheetDefCur.PageCount==1?0:SheetDefCur.PageCount*100-40);
			FillFieldList();
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butAddOutputText_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(SheetDefCur.SheetType,OutInCheck.Out).Count==0) {
				MsgBox.Show(this,"There are no output fields available for this type of sheet.");
				return;
			}
			Font font=new Font(SheetDefCur.FontName,SheetDefCur.FontSize);
			FormSheetFieldOutput FormS=new FormSheetFieldOutput();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewOutput("",SheetDefCur.FontSize,SheetDefCur.FontName,false,0,0,100,font.Height);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddStaticText_Click(object sender,EventArgs e) {
			Font font=new Font(SheetDefCur.FontName,SheetDefCur.FontSize);
			FormSheetFieldStatic FormS=new FormSheetFieldStatic();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewStaticText("",SheetDefCur.FontSize,SheetDefCur.FontName,false,0,0,100,font.Height);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddInputField_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(SheetDefCur.SheetType,OutInCheck.In).Count==0) {
				MsgBox.Show(this,"There are no input fields available for this type of sheet.");
				return;
			}
			Font font=new Font(SheetDefCur.FontName,SheetDefCur.FontSize);
			FormSheetFieldInput FormS=new FormSheetFieldInput();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewInput("",SheetDefCur.FontSize,SheetDefCur.FontName,false,0,0,100,font.Height);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddImage_Click(object sender,EventArgs e) {
			if(!PrefC.AtoZfolderUsed) {
				MsgBox.Show(this,"Not allowed because not using AtoZ folder");
				return;
			}
			//Font font=new Font(SheetDefCur.FontName,SheetDefCur.FontSize);
			FormSheetFieldImage FormS=new FormSheetFieldImage();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewImage("",0,0,100,100);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Insert(0,FormS.SheetFieldDefCur);
			FillFieldList();
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butAddLine_Click(object sender,EventArgs e) {
			FormSheetFieldLine FormS=new FormSheetFieldLine();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewLine(0,0,0,0);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddRect_Click(object sender,EventArgs e) {
			FormSheetFieldRect FormS=new FormSheetFieldRect();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewRect(0,0,0,0);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddCheckBox_Click(object sender,EventArgs e) {
			if(SheetFieldsAvailable.GetList(SheetDefCur.SheetType,OutInCheck.Check).Count==0) {
				MsgBox.Show(this,"There are no checkbox fields available for this type of sheet.");
				return;
			}
			FormSheetFieldCheckBox FormS=new FormSheetFieldCheckBox();
			FormS.IsNew=true;
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewCheckBox("",0,0,11,11);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddSigBox_Click(object sender,EventArgs e) {
			FormSheetFieldSigBox FormS=new FormSheetFieldSigBox();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewSigBox(0,0,364,81);
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddSpecial_Click(object sender,EventArgs e) {
			FormSheetFieldSpecial FormSFS=new FormSheetFieldSpecial();
			FormSFS.SheetDefCur=SheetDefCur;
			FormSFS.IsNew=true;
			FormSFS.ShowDialog();
			if(FormSFS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormSFS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddPatImage_Click(object sender,EventArgs e) {
			if(!PrefC.AtoZfolderUsed) {
				MsgBox.Show(this,"Not allowed because not using AtoZ folder");
				return;
			}
			//Font font=new Font(SheetDefCur.FontName,SheetDefCur.FontSize);
			FormSheetFieldPatImage FormS=new FormSheetFieldPatImage();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewImage("",0,0,100,100);
			FormS.SheetFieldDefCur.FieldType=SheetFieldType.PatImage;
			if(this.IsInternal) {
				FormS.IsReadOnly=true;
			}
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Insert(0,FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void butAddGrid_Click(object sender,EventArgs e) {
			FormSheetFieldGridType FormT=new FormSheetFieldGridType();
			FormT.SheetDefCur=SheetDefCur;
			FormT.ShowDialog();
			if(FormT.DialogResult!=DialogResult.OK) {
				return;
			}
			FormSheetFieldGrid FormS=new FormSheetFieldGrid();
			FormS.SheetDefCur=SheetDefCur;
			FormS.SheetFieldDefCur=SheetFieldDef.NewGrid(FormT.SelectedSheetGridType,0,0,100,100); //is resized from dialog window.
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDefCur.SheetFieldDefs.Add(FormS.SheetFieldDefCur);
			FillFieldList();
			panelMain.Refresh();
		}

		private void listFields_Click(object sender,EventArgs e) {
			//if(listFields.SelectedIndices.Count==0){
			//	return;
			//}
			panelMain.Refresh();
		}

		private void listFields_MouseDoubleClick(object sender,MouseEventArgs e) {
			int idx=listFields.IndexFromPoint(e.Location);
			if(idx==-1) {
				return;
			}
			listFields.SelectedIndices.Clear();
			listFields.SetSelected(idx,true);
			panelMain.Refresh();
			SheetFieldDef field=SheetDefCur.SheetFieldDefs[idx];
			SheetFieldDef fieldold=field.Copy();
			LaunchEditWindow(field);
			if(field.TabOrder!=fieldold.TabOrder) { //otherwise a different control will be selected.
				listFields.SelectedIndices.Clear();
			}
		}

		///<summary>Only for editing fields that already exist.</summary>
		private void LaunchEditWindow(SheetFieldDef field) {
			bool refreshBuffer=false;
			//not every field will have been saved to the database, so we can't depend on SheetFieldDefNum.
			int idx=SheetDefCur.SheetFieldDefs.IndexOf(field);
			switch(field.FieldType) {
				case SheetFieldType.InputField:
					FormSheetFieldInput FormS=new FormSheetFieldInput();
					FormS.SheetDefCur=SheetDefCur;
					FormS.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormS.IsReadOnly=true;
					}
					FormS.ShowDialog();
					if(FormS.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormS.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.OutputText:
					FormSheetFieldOutput FormSO=new FormSheetFieldOutput();
					FormSO.SheetDefCur=SheetDefCur;
					FormSO.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSO.IsReadOnly=true;
					}
					FormSO.ShowDialog();
					if(FormSO.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSO.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.StaticText:
					FormSheetFieldStatic FormSS=new FormSheetFieldStatic();
					FormSS.SheetDefCur=SheetDefCur;
					FormSS.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSS.IsReadOnly=true;
					}
					FormSS.ShowDialog();
					if(FormSS.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSS.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.Image:
					FormSheetFieldImage FormSI=new FormSheetFieldImage();
					FormSI.SheetDefCur=SheetDefCur;
					FormSI.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSI.IsReadOnly=true;
					}
					FormSI.ShowDialog();
					if(FormSI.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSI.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					refreshBuffer=true;
					break;
				case SheetFieldType.PatImage:
					FormSheetFieldPatImage FormSPI=new FormSheetFieldPatImage();
					FormSPI.SheetDefCur=SheetDefCur;
					FormSPI.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSPI.IsReadOnly=true;
					}
					FormSPI.ShowDialog();
					if(FormSPI.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSPI.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					refreshBuffer=true;
					break;
				case SheetFieldType.Line:
					FormSheetFieldLine FormSL=new FormSheetFieldLine();
					FormSL.SheetDefCur=SheetDefCur;
					FormSL.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSL.IsReadOnly=true;
					}
					FormSL.ShowDialog();
					if(FormSL.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSL.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.Rectangle:
					FormSheetFieldRect FormSR=new FormSheetFieldRect();
					FormSR.SheetDefCur=SheetDefCur;
					FormSR.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSR.IsReadOnly=true;
					}
					FormSR.ShowDialog();
					if(FormSR.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSR.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.CheckBox:
					FormSheetFieldCheckBox FormSB=new FormSheetFieldCheckBox();
					FormSB.SheetDefCur=SheetDefCur;
					FormSB.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSB.IsReadOnly=true;
					}
					FormSB.ShowDialog();
					if(FormSB.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSB.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.SigBox:
					FormSheetFieldSigBox FormSBx=new FormSheetFieldSigBox();
					FormSBx.SheetDefCur=SheetDefCur;
					FormSBx.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSBx.IsReadOnly=true;
					}
					FormSBx.ShowDialog();
					if(FormSBx.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSBx.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.Special:
					FormSheetFieldSpecial FormSFS=new FormSheetFieldSpecial();
					FormSFS.SheetDefCur=SheetDefCur;
					FormSFS.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSFS.IsReadOnly=true;
					}
					FormSFS.ShowDialog();
					if(FormSFS.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSFS.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
				case SheetFieldType.Grid:
					FormSheetFieldGrid FormSFG=new FormSheetFieldGrid();
					FormSFG.SheetDefCur=SheetDefCur;
					FormSFG.SheetFieldDefCur=field;
					if(this.IsInternal) {
						FormSFG.IsReadOnly=true;
					}
					FormSFG.ShowDialog();
					if(FormSFG.DialogResult!=DialogResult.OK) {
						return;
					}
					if(FormSFG.SheetFieldDefCur==null) {
						SheetDefCur.SheetFieldDefs.RemoveAt(idx);
					}
					break;
			}
			if(IsTabMode) {
				if(ListSheetFieldDefsTabOrder.Contains(field)) {
					ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
				}
				if(field.TabOrder>0 && field.TabOrder<=(ListSheetFieldDefsTabOrder.Count+1)) {
					ListSheetFieldDefsTabOrder.Insert(field.TabOrder-1,field);
				}
				RenumberTabOrderHelper();
				return;
			}
			//listFields.ClearSelected();
			FillFieldList();
			if(refreshBuffer) { //Only when image was edited.
				RefreshDoubleBuffer();
			}
			//for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
			//	if(SheetDefCur.SheetFieldDefs[i].FieldType==field.FieldType
			//		&& SheetDefCur.SheetFieldDefs[i].FieldType==field.FieldType
			//		&& SheetDefCur.SheetFieldDefs[i].FieldType==field.FieldType
			//}
			//idx=SheetDefCur.SheetFieldDefs.IndexOf(field);
			//if(idx>0) {//only true if field was not deleted.
			//	listFields.SetSelected(idx,true);
			//}
			if(listFields.Items.Count-1>=idx) {
				listFields.SelectedIndex=idx; //reselect the item.
			}
			panelMain.Refresh();
		}

		private void panelMain_MouseDown(object sender,MouseEventArgs e) {
			panel1.Select();
			if(AltIsDown) {
				PasteControlsFromMemory(e.Location);
				return;
			}
			MouseIsDown=true;
			ClickedOnBlankSpace=false;
			MouseOriginalPos=e.Location;
			MouseCurrentPos=e.Location;
			SheetFieldDef field=HitTest(e.X,e.Y);
			if(IsTabMode) {
				MouseIsDown=false;
				CtrlIsDown=false;
				AltIsDown=false;
				if(field==null
					//Some of the fields below are redundant and should never be returned from HitTest but are here to explicity exclude them.
				   || field.FieldType==SheetFieldType.Drawing || field.FieldType==SheetFieldType.Image || field.FieldType==SheetFieldType.Line || field.FieldType==SheetFieldType.OutputText || field.FieldType==SheetFieldType.Parameter || field.FieldType==SheetFieldType.PatImage || field.FieldType==SheetFieldType.Rectangle || field.FieldType==SheetFieldType.StaticText) {
					return;
				}
				if(ListSheetFieldDefsTabOrder.Contains(field)) {
					field.TabOrder=0;
					ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
				}
				else {
					ListSheetFieldDefsTabOrder.Add(field);
				}
				RenumberTabOrderHelper();
				return;
			}
			if(field==null) {
				ClickedOnBlankSpace=true;
				if(CtrlIsDown) {
					return; //so that you can add more to the previous selection
				}
				listFields.SelectedIndices.Clear(); //clear the existing selection
				panelMain.Refresh();
				return;
			}
			int idx=SheetDefCur.SheetFieldDefs.IndexOf(field);
			if(CtrlIsDown) {
				if(listFields.SelectedIndices.Contains(idx)) {
					listFields.SetSelected(idx,false);
				}
				else {
					listFields.SetSelected(idx,true);
				}
			}
			else { //Ctrl not down
				if(listFields.SelectedIndices.Contains(idx)) {
					//clicking on the group, probably to start a drag.
				}
				else {
					listFields.SelectedIndices.Clear();
					listFields.SetSelected(idx,true);
				}
			}
			OriginalControlPositions=new List<Point>();
			Point point;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				point=new Point(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos,SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos);
				OriginalControlPositions.Add(point);
			}
			panelMain.Refresh();
		}

		private void panelMain_MouseMove(object sender,MouseEventArgs e) {
			if(!MouseIsDown) {
				return;
			}
			if(IsInternal) {
				return;
			}
			if(IsTabMode) {
				return;
			}
			if(ClickedOnBlankSpace) {
				MouseCurrentPos=e.Location;
				panelMain.Refresh();
				return;
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos=OriginalControlPositions[i].X+e.X-MouseOriginalPos.X;
				SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos=OriginalControlPositions[i].Y+e.Y-MouseOriginalPos.Y;
			}
			panelMain.Refresh();
		}

		private void panelMain_MouseUp(object sender,MouseEventArgs e) {
			MouseIsDown=false;
			OriginalControlPositions=null;
			if(ClickedOnBlankSpace) { //if initial mouse down was not on a control.  ie, if we are dragging to select.
				Rectangle selectionBounds=new Rectangle(Math.Min(MouseOriginalPos.X,MouseCurrentPos.X), //X
					Math.Min(MouseOriginalPos.Y,MouseCurrentPos.Y), //Y
					Math.Abs(MouseCurrentPos.X-MouseOriginalPos.X), //Width
					Math.Abs(MouseCurrentPos.Y-MouseOriginalPos.Y)); //Height
				for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
					SheetFieldDef tempDef=SheetDefCur.SheetFieldDefs[i]; //to speed this process up instead of referencing the array every time.
					if(tempDef.FieldType==SheetFieldType.Line || tempDef.FieldType==SheetFieldType.Image) {
						continue; //lines and images are currently not selectable by drag and drop. will require lots of calculations, completely possible, but complex.
					}
					//If the selection is contained within the "hollow" portion of the rectangle, it shouldn't be selected.
					if(tempDef.FieldType==SheetFieldType.Rectangle) {
						Rectangle tempDefBounds=new Rectangle(tempDef.Bounds.X+4,tempDef.Bounds.Y+4,tempDef.Bounds.Width-8,tempDef.Bounds.Height-8);
						if(tempDefBounds.Contains(selectionBounds)) {
							continue;
						}
					}
					if(tempDef.BoundsF.IntersectsWith(selectionBounds)) {
						listFields.SetSelected(i,true); //Add to selected indicies
					}
				}
			}
			ClickedOnBlankSpace=false;
			panelMain.Refresh();
		}

		private void panelMain_MouseDoubleClick(object sender,MouseEventArgs e) {
			if(AltIsDown) {
				return;
			}
			SheetFieldDef field=HitTest(e.X,e.Y);
			if(field==null) {
				return;
			}
			SheetFieldDef fieldold=field.Copy();
			LaunchEditWindow(field);
			//if(field.TabOrder!=fieldold.TabOrder) {
			//  listFields.SelectedIndices.Clear();
			//}
			//if(isTabMode) {
			//  if(ListSheetFieldDefsTabOrder.Contains(field)){
			//    ListSheetFieldDefsTabOrder.RemoveAt(ListSheetFieldDefsTabOrder.IndexOf(field));
			//  }
			//  if(field.TabOrder>0 && field.TabOrder<ListSheetFieldDefsTabOrder.Count+1) {
			//    ListSheetFieldDefsTabOrder.Insert(field.TabOrder-1,field);
			//  }
			//  RenumberTabOrderHelper();
			//}
		}

		private void panelMain_Resize(object sender,EventArgs e) {
			if(BmBackground!=null && panelMain.Size==BmBackground.Size) {
				return;
			}
			if(GraphicsBackground!=null) {
				GraphicsBackground.Dispose();
			}
			if(BmBackground!=null) {
				BmBackground.Dispose();
			}
			BmBackground=new Bitmap(panelMain.Width,panelMain.Height);
			GraphicsBackground=Graphics.FromImage(BmBackground);
			panelMain.Refresh();
		}

		///<summary>Used To renumber TabOrder on controls</summary>
		private void RenumberTabOrderHelper() {
			for(int i=0;i<ListSheetFieldDefsTabOrder.Count;i++) {
				ListSheetFieldDefsTabOrder[i].TabOrder=i+1; //Start number tab order at 1
			}
			FillFieldList();
			panelMain.Refresh();
		}

		///<summary>Images will be ignored in the hit test since they frequently fill the entire background.  Lines will be ignored too, since a diagonal line could fill a large area.</summary>
		private SheetFieldDef HitTest(int x,int y) {
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				if(SheetDefCur.SheetFieldDefs[i].FieldType==SheetFieldType.Image) {
					continue;
				}
				if(SheetDefCur.SheetFieldDefs[i].FieldType==SheetFieldType.Line) {
					continue;
				}
				Rectangle fieldDefBounds=SheetDefCur.SheetFieldDefs[i].Bounds;
				if(fieldDefBounds.Contains(x,y)) {
					//Center of the rectangle will not be considered a hit.
					if(SheetDefCur.SheetFieldDefs[i].FieldType==SheetFieldType.Rectangle && new Rectangle(fieldDefBounds.X+4,fieldDefBounds.Y+4,fieldDefBounds.Width-8,fieldDefBounds.Height-8).Contains(x,y)) {
						continue;
					}
					return SheetDefCur.SheetFieldDefs[i];
				}
			}
			return null;
		}

		private void FormSheetDefEdit_KeyDown(object sender,KeyEventArgs e) {
			bool refreshBuffer=false;
			e.Handled=true;
			if(e.KeyCode==Keys.ControlKey && CtrlIsDown) {
				return;
			}
			if(IsInternal) {
				return;
			}
			if(e.Control) {
				CtrlIsDown=true;
			}
			if(CtrlIsDown && e.KeyCode==Keys.C) { //CTRL-C
				CopyControlsToMemory();
			}
			else if(CtrlIsDown && e.KeyCode==Keys.V) { //CTRL-V
				PasteControlsFromMemory(new Point(0,0));
			}
			else if(e.Alt) {
				Cursor=Cursors.Cross; //change cursor to rubber stamp cursor
				AltIsDown=true;
			}
			else if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) {
				if(listFields.SelectedIndices.Count==0) {
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete selected fields?")) {
					return;
				}
				for(int i=listFields.SelectedIndices.Count-1;i>=0;i--) { //iterate backwards through list
					SheetFieldDef fieldI=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]];
					if(fieldI.FieldType==SheetFieldType.Image) {
						refreshBuffer=true;
					}
					if(fieldI.FieldType==SheetFieldType.Grid && fieldI.FieldName=="TreatPlanMain"
						&& SheetDefCur.SheetFieldDefs.FindAll(x=>x.FieldType==SheetFieldType.Grid && x.FieldName=="TreatPlanMain").Count==1) 
					{
						MsgBox.Show(this,"Cannot delete the last main grid from treatment plan.");
						continue;//skip this one.
					}
					SheetDefCur.SheetFieldDefs.RemoveAt(listFields.SelectedIndices[i]);
				}
				FillFieldList();
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				if(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].FieldType==SheetFieldType.Image) {
					refreshBuffer=true;
				}
				switch(e.KeyCode) {
					case Keys.Up:
						if(e.Shift) {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos-=7;
						}
						else {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos--;
						}
						break;
					case Keys.Down:
						if(e.Shift) {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos+=7;
						}
						else {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos++;
						}
						break;
					case Keys.Left:
						if(e.Shift) {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos-=7;
						}
						else {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos--;
						}
						break;
					case Keys.Right:
						if(e.Shift) {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos+=7;
						}
						else {
							SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos++;
						}
						break;
					default:
						break;
				}
			}
			if(refreshBuffer) { //Only when an image was selected.
				RefreshDoubleBuffer();
			}
			panelMain.Refresh();
		}

		private void FormSheetDefEdit_KeyUp(object sender,KeyEventArgs e) {
			if((e.KeyCode&Keys.ControlKey)==Keys.ControlKey) {
				CtrlIsDown=false;
			}
			if(!e.Alt) {
				Cursor=Cursors.Default;
				AltIsDown=false;
			}
		}

		private void CopyControlsToMemory() {
			if(IsTabMode) {
				return;
			}
			if(listFields.SelectedIndices.Count==0) {
				return;
			}
			//List<SheetFieldDef> listDuplicates=new List<SheetFieldDef>();
			string strPrompt=Lan.g(this,"The following selected fields can cause conflicts if they are copied:\r\n");
			bool conflictingfield=false;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				SheetFieldDef fielddef=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]];
				switch(fielddef.FieldType) {
					case SheetFieldType.Drawing:
					case SheetFieldType.Image:
					case SheetFieldType.Line:
					case SheetFieldType.PatImage:
					//case SheetFieldType.Parameter://would not be seen on the sheet.
					case SheetFieldType.Rectangle:
					case SheetFieldType.SigBox:
					case SheetFieldType.StaticText:
						break; //it will always be ok to copy the types of fields above.
					case SheetFieldType.CheckBox:
						if(fielddef.FieldName!="misc") { //custom fields should be okay to copy
							strPrompt+=fielddef.FieldName+"."+fielddef.RadioButtonValue+"\r\n";
							conflictingfield=true;
						}
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
						if(fielddef.FieldName!="misc") { //custom fields should be okay to copy
							strPrompt+=fielddef.FieldName+"\r\n";
							conflictingfield=true;
						}
						break;
				}
			}
			strPrompt+=Lan.g(this,"Would you like to continue anyways?");
			if(conflictingfield && MessageBox.Show(strPrompt,Lan.g(this,"Warning"),MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				panel1.Select();
				CtrlIsDown=false;
				return;
			}
			ListSheetFieldDefsCopyPaste=new List<SheetFieldDef>(); //empty the remembered field list
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				ListSheetFieldDefsCopyPaste.Add(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].Copy()); //fill clipboard with copies of the controls. 
				//It would probably be safe to fill the clipboard with the originals. but it is safer to fill it with copies.
			}
			PasteOffset=0;
			PasteOffsetY=0; //reset PasteOffset for pasting a new set of fields.
		}

		private void PasteControlsFromMemory(Point origin) {
			if(IsTabMode) {
				return;
			}
			if(ListSheetFieldDefsCopyPaste==null || ListSheetFieldDefsCopyPaste.Count==0) {
				return;
			}
			if(origin.X==0 && origin.Y==0) { //allows for cascading pastes in the upper right hand corner.
				Rectangle r=panelMain.Bounds; //Gives relative position of panel (scroll position)
				int h=panel1.Height; //Current resized height/width of parent panel
				int w=panel1.Width;
				int maxH=0;
				int maxW=0;
				for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //calculate height/width of control to be pasted
					maxH=Math.Max(maxH,ListSheetFieldDefsCopyPaste[i].Height);
					maxW=Math.Max(maxW,ListSheetFieldDefsCopyPaste[i].Width);
				}
				origin=new Point((-1)*r.X+w/2-maxW/2-10,(-1)*r.Y+h/2-maxH/2-10); //Center: scroll position * (-1) + 1/2 size of window - 1/2 the size of the field - 10 for scroll bar
				origin.X+=PasteOffset;
				origin.Y+=PasteOffset+PasteOffsetY;
			}
			listFields.ClearSelected();
			int minX=int.MaxValue;
			int minY=int.MaxValue;
			for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //calculate offset
				minX=Math.Min(minX,ListSheetFieldDefsCopyPaste[i].XPos);
				minY=Math.Min(minY,ListSheetFieldDefsCopyPaste[i].YPos);
			}
			for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) { //create new controls
				Random rand=new Random();
				//this new key is only used for copy and paste function.
				//When this sheet is saved, all sheetfielddefs are deleted and reinserted, so the dummy PKs are harmless.
				//There's a VERY slight chance of PK duplication, but the only result would be selection of wrong field.
				int newDefNum=rand.Next(int.MaxValue);
				ListSheetFieldDefsCopyPaste[i].SheetFieldDefNum=newDefNum;
				SheetFieldDef fielddef=ListSheetFieldDefsCopyPaste[i].Copy();
				fielddef.XPos=fielddef.XPos-minX+origin.X;
				fielddef.YPos=fielddef.YPos-minY+origin.Y;
				SheetDefCur.SheetFieldDefs.Add(fielddef);
			}
			if(!AltIsDown) {
				PasteOffsetY+=((PasteOffset+10)/100)*10; //this will shift the pastes down 10 pixels every 10 pastes.
				PasteOffset=(PasteOffset+10)%100; //cascades and allows for 90 consecutive pastes without overlap
			}
			FillFieldList();
			//used to be in FillFieldList but was causing these fields to be selected every time FillFieldList was called.
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				if(ListSheetFieldDefsCopyPaste!=null) { //reselect pasted controls 
					for(int cp=0;cp<ListSheetFieldDefsCopyPaste.Count;cp++) {
						if(SheetDefCur.SheetFieldDefs[i].SheetFieldDefNum==ListSheetFieldDefsCopyPaste[cp].SheetFieldDefNum) {
							listFields.SetSelected(i,true); //safe to run multiple times.
						}
					}
				}
			}
			//for(int i=0;i<ListSheetFieldDefsCopyPaste.Count;i++) {//reselect newly added controls
			//  listFields.SetSelected((listFields.Items.Count-1)-i,true);//Add to selected indicies, which will be the newest clipboard.count controls on the bottom of the list.
			//}
			panelMain.Refresh();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsTabMode) {
				return;
			}
			if(SheetDefCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete entire sheet?")) {
				return;
			}
			try {
				SheetDefs.DeleteObject(SheetDefCur.SheetDefNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private bool VerifyDesign() {
			//Keep a temporary list of every medical input and check box so it saves time checking for duplicates.
			List<SheetFieldDef> medChkBoxList=new List<SheetFieldDef>();
			List<SheetFieldDef> inputMedList=new List<SheetFieldDef>();
			//Verify radio button groups.
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				SheetFieldDef field=SheetDefCur.SheetFieldDefs[i];
				if(field.FieldType==SheetFieldType.CheckBox && field.IsRequired && (field.RadioButtonGroup!="" //for misc radio groups
				                                                                    || field.RadioButtonValue!="")) //for built-in radio groups
				{
					//All radio buttons within a group must either all be marked required or all be marked not required. 
					//Not the most efficient check, but there won't usually be more than a few hundred items so the user will not ever notice. We can speed up later if needed.
					for(int j=0;j<SheetDefCur.SheetFieldDefs.Count;j++) {
						SheetFieldDef field2=SheetDefCur.SheetFieldDefs[j];
						if(field2.FieldType==SheetFieldType.CheckBox && !field2.IsRequired && field2.RadioButtonGroup.ToLower()==field.RadioButtonGroup.ToLower() //for misc groups
						   && field2.FieldName.ToLower()==field.FieldName.ToLower()) //for misc groups
						{
							MessageBox.Show(Lan.g(this,"Radio buttons in radio button group")+" '"+(field.RadioButtonGroup==""?field.FieldName:field.RadioButtonGroup)+"' "+Lan.g(this,"must all be marked required or all be marked not required."));
							return false;
						}
					}
				}
				if(field.FieldType==SheetFieldType.CheckBox && (field.FieldName.StartsWith("allergy:")) || field.FieldName.StartsWith("checkMed") || field.FieldName.StartsWith("problem:")) {
					for(int j=0;j<medChkBoxList.Count;j++) { //Check for duplicates.
						if(medChkBoxList[j].FieldName==field.FieldName && medChkBoxList[j].RadioButtonValue==field.RadioButtonValue) {
							MessageBox.Show(Lan.g(this,"Duplicate check box found")+": '"+field.FieldName+" "+field.RadioButtonValue+"'. "+Lan.g(this,"Only one of each type is allowed."));
							return false;
						}
					}
					//Not a duplicate so add it to the med chk box list.
					medChkBoxList.Add(field);
				}
				else if(field.FieldType==SheetFieldType.InputField && field.FieldName.StartsWith("inputMed")) {
					for(int j=0;j<inputMedList.Count;j++) {
						if(inputMedList[j].FieldName==field.FieldName) {
							MessageBox.Show(Lan.g(this,"Duplicate inputMed boxes found")+": '"+field.FieldName+"'. "+Lan.g(this,"Only one of each is allowed."));
							return false;
						}
					}
					inputMedList.Add(field);
				}
			}
			switch(SheetDefCur.SheetType) {
				case SheetTypeEnum.TreatmentPlan:
					if(SheetDefCur.SheetFieldDefs.FindAll(x => x.FieldType==SheetFieldType.SigBox).Count!=1) {
						MessageBox.Show(Lan.g(this,"Treatment plans must have exactly one signature box."));
						return false;
					}
					if(SheetDefCur.SheetFieldDefs.FindAll(x => x.FieldType==SheetFieldType.Grid && x.FieldName=="TreatPlanMain").Count<1) {
						MessageBox.Show(Lan.g(this,"Treatment plans must have one main grid."));
						return false;
					}
					break;
			}
			return true;
		}

		private void linkLabelTips_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			if(IsTabMode) {
				return;
			}
			string tips="";
			tips+="The following shortcuts and hotkeys are supported:\r\n";
			tips+="\r\n";
			tips+="CTRL + C : Copy selected field(s).\r\n";
			tips+="\r\n";
			tips+="CTRL + V : Paste.\r\n";
			tips+="\r\n";
			tips+="ALT + Click : 'Rubber stamp' paste to the cursor position.\r\n";
			tips+="\r\n";
			tips+="Click + Drag : Click on a blank space and then drag to group select.\r\n";
			tips+="\r\n";
			tips+="CTRL + Click + Drag : Add a group of fields to the selection.\r\n";
			tips+="\r\n";
			tips+="Delete or Backspace : Delete selected field(s).\r\n";
			MessageBox.Show(Lan.g(this,tips));
		}

		private void butAlignLeft_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			float minX=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[0]].BoundsF.Left;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				if(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Left<minX) { //current element is higher up than the current 'highest' element.
					minX=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Left;
				}
				for(int j=0;j<listFields.SelectedIndices.Count;j++) {
					if(i==j) { //Don't compare element to itself.
						continue;
					}
					if(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].Bounds.Y //compare the int bounds not the boundsF for practical use
					   ==SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[j]].Bounds.Y) //compare the int bounds not the boundsF for practical use
					{
						MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
						return;
					}
				}
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) { //Actually move the controls now
				SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].XPos=(int)minX;
			}
			panelMain.Refresh();
		}

		private void butAlignCenterH_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSelectedFields=new List<SheetFieldDef>();
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				listSelectedFields.Add(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]]);
			}
			List<int> yPositions=new List<int>();
			float maxX=int.MinValue;
			float minX=int.MaxValue;
			foreach(SheetFieldDef field in listSelectedFields) {
				if(yPositions.Contains(field.YPos)) {
					MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
					return;
				}
				yPositions.Add(field.YPos);
				if(maxX<field.Bounds.Right) {
					maxX=field.Bounds.Right;
				}
				if(minX>field.Bounds.Left) {
					minX=field.Bounds.Left;
				}
			}
			int avgX=(int)(minX+maxX)/2;
#warning remove lamda statement below.
			listSelectedFields.ForEach(field => field.XPos=avgX-field.Width/2);
			panelMain.Refresh();
		}

		private void butAlignRight_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			List<SheetFieldDef> listSelectedFields=new List<SheetFieldDef>();
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				listSelectedFields.Add(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]]);
			}
			if(listSelectedFields.Exists(f1 => listSelectedFields.FindAll(f2 => f1.YPos==f2.YPos).Count>1)) {
				MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
				return;
			}
			int maxX=listSelectedFields.Max(d => d.Bounds.Right);
			listSelectedFields.ForEach(field => field.XPos=maxX-field.Width);
			panelMain.Refresh();
		}

		/// <summary>When clicked it will set all selected elements' Y coordinates to the smallest Y coordinate in the group, unless two controls have the same X coordinate.</summary>
		private void butAlignTop_Click(object sender,EventArgs e) {
			if(listFields.SelectedIndices.Count<2) {
				return;
			}
			float minY=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[0]].BoundsF.Top;
			for(int i=0;i<listFields.SelectedIndices.Count;i++) {
				if(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Top<minY) { //current element is higher up than the current 'highest' element.
					minY=SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].BoundsF.Top;
				}
				for(int j=0;j<listFields.SelectedIndices.Count;j++) {
					if(i==j) { //Don't compare element to itself.
						continue;
					}
					if(SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].Bounds.X //compair the int bounds not the boundsF for practical use
					   ==SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[j]].Bounds.X) //compair the int bounds not the boundsF for practical use
					{
						MsgBox.Show(this,"Cannot align controls. Two or more selected controls will overlap.");
						return;
					}
				}
			}
			for(int i=0;i<listFields.SelectedIndices.Count;i++) { //Actually move the controls now
				SheetDefCur.SheetFieldDefs[listFields.SelectedIndices[i]].YPos=(int)minY;
			}
			panelMain.Refresh();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			PasteControlsFromMemory(new Point(0,0));
		}

		private void butCopy_Click(object sender,EventArgs e) {
			CopyControlsToMemory();
		}

		private void butTabOrder_Click(object sender,EventArgs e) {
			IsTabMode=!IsTabMode;
			if(IsTabMode) {
				butOK.Enabled=false;
				butCancel.Enabled=false;
				butDelete.Enabled=false;
				groupAddNew.Enabled=false;
				butCopy.Enabled=false;
				butPaste.Enabled=false;
				groupAlignH.Enabled=false;
				groupAlignV.Enabled=false;
				//butAlignLeft.Enabled=false;
				//butAlignTop.Enabled=false;
				butEdit.Enabled=false;
				ListSheetFieldDefsTabOrder=new List<SheetFieldDef>(); //clear or create the list of tab orders.
			}
			else {
				butOK.Enabled=true;
				butCancel.Enabled=true;
				butDelete.Enabled=true;
				groupAddNew.Enabled=true;
				butCopy.Enabled=true;
				butPaste.Enabled=true;
				groupAlignH.Enabled=true;
				groupAlignV.Enabled=true;
				//butAlignLeft.Enabled=true;
				//butAlignTop.Enabled=true;
				butEdit.Enabled=true;
			}
			panelMain.Refresh();
		}

		private void butPageAdd_Click(object sender,EventArgs e) {
			if(SheetDefCur.PageCount>9) {
				//Maximum PageCount 10, this is an arbitrary number. If this number gets too big there may be issues with the graphics trying to draw the sheet.
				return;
			}
			SheetDefCur.PageCount++;
			//SheetDefCur.IsMultiPage=true;
			panelMain.Height=SheetDefCur.HeightTotal-(SheetDefCur.PageCount==1?0:SheetDefCur.PageCount*100-40);
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butPageRemove_Click(object sender,EventArgs e) {
			if(SheetDefCur.PageCount<2) {
				//SheetDefCur.IsMultiPage=false;
				//Minimum PageCount 1
				return;
			}
			SheetDefCur.PageCount--;
			if(SheetDefCur.PageCount==1) {
				//SheetDefCur.IsMultiPage=false;
			}
			if(SheetDefCur.SheetFieldDefs.FindAll(i => i.YPos>SheetDefCur.HeightTotal).Count>0) { //Find all fields that have a YPos greater than the bottom of the page.
				MsgBox.Show(this,"Cannot remove pages that contain sheet fields.");
				SheetDefCur.PageCount++;
				return;
			}
			panelMain.Height=SheetDefCur.HeightTotal-(SheetDefCur.PageCount==1?0:SheetDefCur.PageCount*100-40);
			RefreshDoubleBuffer();
			panelMain.Refresh();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!VerifyDesign()) {
				return;
			}
			SheetDefs.InsertOrUpdate(SheetDefCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

	public class DrawFieldArgs:IDisposable {
		public Pen penBlue;
		public Pen penRed;
		public Pen penBlueThick;
		public Pen penRedThick;
		public Pen penBlack;
		public Pen penSelection;
		///<summary>Line color can be customized.  Make sure to explicitly set the color of this pen before using it because it might contain a color of a previous line.</summary>
		public Pen penLine;
		public Pen pen;
		public Brush brush;
		public SolidBrush brushBlue;
		public SolidBrush brushRed;
		///<summary>Static text color can be customized.  Make sure to explicitly set the color of this brush before using it because it might contain a color of previous static text.</summary>
		public SolidBrush brushText;
		public Font font;
		public FontStyle fontstyle;

		public DrawFieldArgs() {
			penBlue=new Pen(Color.Blue);
			penRed=new Pen(Color.Red);
			penBlueThick=new Pen(Color.Blue,1.6f);
			penRedThick=new Pen(Color.Red,1.6f);
			penBlack=new Pen(Color.Black);
			penSelection=new Pen(Color.Black);
			penLine=new Pen(Color.Black);
			brushBlue=new SolidBrush(Color.Blue);
			brushRed=new SolidBrush(Color.Red);
			brushText=new SolidBrush(Color.Black);
			pen=penBlack;
			brush=brushText;
		}

		public void Dispose() {
			penBlue.Dispose();
			penRed.Dispose();
			penBlueThick.Dispose();
			penRedThick.Dispose();
			penBlack.Dispose();
			penSelection.Dispose();
			penLine.Dispose();
			brushBlue.Dispose();
			brushRed.Dispose();
			brushText.Dispose();
		}
	}
}