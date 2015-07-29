using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	public class SheetUtil {
		private static List<MedLabResult> _listResults;
		///<summary>Supply a template sheet as well as a list of primary keys.  This method creates a new collection of sheets which each have a parameter of int.  It also fills the sheets with data from the database, so no need to run that separately.</summary>
		public static List<Sheet> CreateBatch(SheetDef sheetDef,List<long> priKeys) {
			//we'll assume for now that a batch sheet has only one parameter, so no need to check for values.
			//foreach(SheetParameter param in sheet.Parameters){
			//	if(param.IsRequired && param.ParamValue==null){
			//		throw new ApplicationException(Lan.g("Sheet","Parameter not specified for sheet: ")+param.ParamName);
			//	}
			//}
			List<Sheet> retVal=new List<Sheet>();
			//List<int> paramVals=(List<int>)sheet.Parameters[0].ParamValue;
			Sheet newSheet;
			SheetParameter paramNew;
			for(int i=0;i<priKeys.Count;i++){
				newSheet=CreateSheet(sheetDef);
				newSheet.Parameters=new List<SheetParameter>();
				paramNew=new SheetParameter(sheetDef.Parameters[0].IsRequired,sheetDef.Parameters[0].ParamName);
				paramNew.ParamValue=priKeys[i];
				newSheet.Parameters.Add(paramNew);
				SheetFiller.FillFields(newSheet);
				retVal.Add(newSheet);
			}
			return retVal;
		}

		///<summary>Just before printing or displaying the final sheet output, the heights and y positions of various fields are adjusted according to their growth behavior.  This also now gets run every time a user changes the value of a textbox while filling out a sheet.</summary>
		public static void CalculateHeights(Sheet sheet,Graphics g,Statement stmt=null,bool isPrinting=false,int topMargin=40,int bottomMargin=60,MedLab medLab=null){
			//Sheet sheetCopy=sheet.Clone();
			int calcH;
			Font font;
			FontStyle fontstyle;
			foreach(SheetField field in sheet.SheetFields) {
				if(field.FieldType==SheetFieldType.Image || field.FieldType==SheetFieldType.PatImage) {
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
					if(field.FieldName=="Patient Info.gif" || File.Exists(filePathAndName)) {
						continue;
					}
					else {//img doesn't exist or we do not have access to it.
						field.Height=0;//Set height to zero so that it will not cause extra pages to print.
					}
				}
				if(field.GrowthBehavior==GrowthBehaviorEnum.None){//Images don't have growth behavior, so images are excluded below this point.
					continue;
				}
				fontstyle=FontStyle.Regular;
				if(field.FontIsBold){
					fontstyle=FontStyle.Bold;
				}
				font=new Font(field.FontName,field.FontSize,fontstyle);
				//calcH=(int)g.MeasureString(field.FieldValue,font).Height;//this was too short
				if(field.FieldType!=SheetFieldType.Grid) {
					calcH=GraphicsHelper.MeasureStringH(g,field.FieldValue,font,field.Width);
				}
				else {//handle grid height calculation seperately.
					calcH=CalculateGridHeightHelper(field,sheet,g,stmt,topMargin,bottomMargin,medLab);
				}
				if(calcH<=field.Height //calc height is smaller
					&& field.FieldName!="StatementPayPlan") 
				{
					continue;
				}
				int amountOfGrowth=calcH-field.Height;
				field.Height=calcH;
				if(field.GrowthBehavior==GrowthBehaviorEnum.DownLocal){
					MoveAllDownWhichIntersect(sheet,field,amountOfGrowth);
				}
				else if(field.GrowthBehavior==GrowthBehaviorEnum.DownGlobal){
					//All sheet grids should have DownGlobal growth.
					MoveAllDownBelowThis(sheet,field,amountOfGrowth);
				}
			}
			if(isPrinting && !Sheets.SheetTypeIsSinglePage(sheet.SheetType)) {
				//now break all text fields in between lines, not in the middle of actual text
				sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
				int originalSheetFieldCount=sheet.SheetFields.Count;
				for(int i=0;i<originalSheetFieldCount;i++) {
					SheetField fieldCur=sheet.SheetFields[i];
					if(fieldCur.FieldType==SheetFieldType.StaticText
						|| fieldCur.FieldType==SheetFieldType.OutputText
						|| fieldCur.FieldType==SheetFieldType.InputField)
					{
						//recursive function to split text boxes for page breaks in between lines of text, not in the middle of text
						CalculateHeightsPageBreak(fieldCur,sheet,g);
					}
				}
				//sort the fields again since we may have broken up some of the text fields into multiple fields and added them to sheetfields.
				sheet.SheetFields.Sort(SheetFields.SortDrawingOrderLayers);
			}
			//return sheetCopy;
		}

		///<summary>Recursive.</summary>
		private static void CalculateHeightsPageBreak(SheetField field,Sheet sheet,Graphics g) {
			double lineSpacingForPdf=1.01d;
			FontStyle fontstyle=FontStyle.Regular;
			if(field.FontIsBold) {
				fontstyle=FontStyle.Bold;
			}
			Font font=new Font(field.FontName,field.FontSize,fontstyle);
			//adjust the height of the text box to accomodate PDFs if the field has a growth behavior other than None
			double calcH=lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,field.FieldValue,font,field.Width);
			if(field.GrowthBehavior!=GrowthBehaviorEnum.None && field.Height<Convert.ToInt32(Math.Ceiling(calcH))) {
				int amtGrowth=Convert.ToInt32(Math.Ceiling(calcH)-field.Height);
				field.Height+=amtGrowth;
				if(field.GrowthBehavior==GrowthBehaviorEnum.DownLocal) {
					MoveAllDownWhichIntersect(sheet,field,amtGrowth);
				}
				else if(field.GrowthBehavior==GrowthBehaviorEnum.DownGlobal) {
					MoveAllDownBelowThis(sheet,field,amtGrowth);
				}
			}
			int topMargin=40;
			if(sheet.SheetType==SheetTypeEnum.MedLabResults) {
				topMargin=120;
			}
			int pageCount;
			int bottomCurPage=SheetPrinting.bottomCurPage(field.YPos,sheet,out pageCount);
			//recursion base case, the field now fits on the current page, break out of recursion
			if(field.YPos+field.Height<=bottomCurPage) {
				return;
			}
			//field extends beyond the bottom of the current page, so we will split the text box in between lines, not through the middle of text
			string measureText="Any";
			double calcHLine=lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,measureText,font,field.Width);//calcHLine is height of single line of text
			//if the height of one line is greater than the printable height of the page, don't try to split between lines
			if(Convert.ToInt32(Math.Ceiling(calcHLine))>(sheet.HeightPage-60-topMargin)) {
				return;
			}
			if(Convert.ToInt32(Math.Ceiling(field.YPos+calcHLine))>bottomCurPage) {//if no lines of text will fit on current page, move the entire text box to the next page
				int moveAmount=bottomCurPage+1-field.YPos;
				field.Height+=moveAmount;
				MoveAllDownWhichIntersect(sheet,field,moveAmount);
				field.Height-=moveAmount;
				field.YPos+=moveAmount;
				//recursive call
				CalculateHeightsPageBreak(field,sheet,g);
				return;
			}
			calcH=0;
			int fieldH=0;
			measureText="";
			//while YPos + calc height of the string <= the bottom of the current page, add a new line and the text Any to the string
			while(Convert.ToInt32(Math.Ceiling(field.YPos+calcH))<=bottomCurPage) {
				fieldH=Convert.ToInt32(Math.Ceiling(calcH));
				if(measureText!="") {
					measureText+="\r\n";
				}
				measureText+="Any";//add new line and another word to measure the height of an additional line of text
				calcH=lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,measureText,font,field.Width);
			}
			//get ready to copy text from the current field to a copy of the field that will be moved down.
			SheetField fieldNew=new SheetField();
			fieldNew=field.Copy();
			field.Height=fieldH;
			fieldNew.Height-=fieldH;//reduce the size of the new text box by the height of the text removed
			fieldNew.YPos+=fieldH;//move the new field down the amount of the removed text to maintain the distance between all fields below
			//this is so all new line characters will be a single character, we will replace \n's with \r\n's after this for loop
			fieldNew.FieldValue=fieldNew.FieldValue.Replace("\r\n","\n");
			int exponentN=Convert.ToInt32(Math.Ceiling(Math.Log(fieldNew.FieldValue.Length,2)))-1;
			int indexCur=Convert.ToInt32(Math.Pow((double)2,(double)exponentN));
			int fieldHeightCur=0;
			while(exponentN>0) {
				exponentN--;
				if(indexCur>=fieldNew.FieldValue.Length
					|| Convert.ToInt32(Math.Ceiling(lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,fieldNew.FieldValue.Substring(0,indexCur+1),
								font,fieldNew.Width)))>field.Height) {
					indexCur-=Convert.ToInt32(Math.Pow((double)2,(double)exponentN));
				}
				else {
					indexCur+=Convert.ToInt32(Math.Pow((double)2,(double)exponentN));
				}
			}
			if(indexCur>=fieldNew.FieldValue.Length) {//just in case, set indexCur to the last character if it is larger than the size of the fieldValue
				indexCur=fieldNew.FieldValue.Length-1;
			}
			fieldHeightCur=Convert.ToInt32(Math.Ceiling(lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,fieldNew.FieldValue.Substring(0,indexCur+1),
				font,fieldNew.Width)));
			while(fieldHeightCur>field.Height) {
				indexCur--;
				fieldHeightCur=Convert.ToInt32(Math.Ceiling(lineSpacingForPdf*GraphicsHelper.MeasureStringH(g,fieldNew.FieldValue.Substring(0,indexCur+1),
					font,fieldNew.Width)));
			}
			//add the new line character to the previous line so the next page doesn't start with a blank line
			if(fieldNew.FieldValue.Length>indexCur+1
				&& (fieldNew.FieldValue[indexCur+1]=='\r'
				|| fieldNew.FieldValue[indexCur+1]=='\n')) {
				indexCur++;
			}
			field.FieldValue=fieldNew.FieldValue.Substring(0,indexCur+1);
			if(field.FieldValue[indexCur]=='\r' || field.FieldValue[indexCur]=='\n') {
				field.FieldValue=field.FieldValue.Substring(0,indexCur);
			}
			field.FieldValue=field.FieldValue.Replace("\n","\r\n");
			if(fieldNew.FieldValue.Length>indexCur+1) {
				fieldNew.FieldValue=fieldNew.FieldValue.Substring(indexCur+1);
				fieldNew.FieldValue=fieldNew.FieldValue.Replace("\n","\r\n");
			}
			else {
				//no text left for the field that would have been on the next page, done, break out of recursion
				return;
			}
			int moveAmountNew=bottomCurPage+1-fieldNew.YPos;
			fieldNew.Height+=moveAmountNew;
			MoveAllDownWhichIntersect(sheet,fieldNew,moveAmountNew);
			fieldNew.Height-=moveAmountNew;
			fieldNew.YPos+=moveAmountNew;
			sheet.SheetFields.Add(fieldNew);
			//recursive call
			CalculateHeightsPageBreak(fieldNew,sheet,g);
		}

		///<summary>Calculates height of grid taking into account page breaks, word wrapping, cell width, font size, and actual data to be used to fill this grid.</summary>
		private static int CalculateGridHeightHelper(SheetField field,Sheet sheet,Graphics g,Statement stmt,int topMargin,int bottomMargin,MedLab medLab) {
			UI.ODGrid odGrid=new UI.ODGrid();
			odGrid.FontForSheets=new Font(field.FontName,field.FontSize,field.FontIsBold?FontStyle.Bold:FontStyle.Regular);
			odGrid.Width=field.Width;
			odGrid.HideScrollBars=true;
			odGrid.YPosField=field.YPos;
			odGrid.TopMargin=topMargin;
			odGrid.BottomMargin=bottomMargin;
			odGrid.PageHeight=sheet.HeightPage;
			odGrid.Title=field.FieldName;
			if(stmt!=null) {
				odGrid.Title+=(stmt.Intermingled?".Intermingled":".NotIntermingled");//Important for calculating heights.
			}
			DataTable Table=SheetUtil.GetDataTableForGridType(field.FieldName,stmt,medLab);
			List<DisplayField> Columns=SheetUtil.GetGridColumnsAvailable(field.FieldName);
			#region  Fill Grid
			odGrid.BeginUpdate();
			odGrid.Columns.Clear();
			ODGridColumn col;
			for(int i=0;i<Columns.Count;i++) {
				col=new ODGridColumn(Columns[i].InternalName,Columns[i].ColumnWidth);
				odGrid.Columns.Add(col);
			}
			ODGridRow row;
			for(int i=0;i<Table.Rows.Count;i++) {
				row=new ODGridRow();
				for(int c=0;c<Columns.Count;c++) {//Selectively fill columns from the dataTable into the odGrid.
					row.Cells.Add(Table.Rows[i][Columns[c].InternalName].ToString());
				}
				if(Table.Columns.Contains("PatNum")) {//Used for statments to determine account splitting.
					row.Tag=Table.Rows[i]["PatNum"].ToString();
				}
				odGrid.Rows.Add(row);
			}
			odGrid.EndUpdate(true);//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			#endregion
			return odGrid.PrintHeight;
		}

		public static void MoveAllDownBelowThis(Sheet sheet,SheetField field,int amountOfGrowth){
			foreach(SheetField field2 in sheet.SheetFields) {
				if(field2.YPos>field.YPos) {//for all fields that are below this one
					field2.YPos+=amountOfGrowth;//bump down by amount that this one grew
				}
			}
		}

		///<Summary>Supply the field that we are testing.  All other fields which intersect with it will be moved down.  Each time one (or maybe some) is moved down, this method is called recursively.  The end result should be no intersections among fields near the original field that grew.</Summary>
		public static void MoveAllDownWhichIntersect(Sheet sheet,SheetField field,int amountOfGrowth) {
			//Phase 1 is to move everything that intersects with the field down. Phase 2 is to call this method on everything that was moved.
			//Phase 1: Move 
			List<SheetField> affectedFields=new List<SheetField>();
			foreach(SheetField field2 in sheet.SheetFields) {
				if(field2==field){
					continue;
				}
				if(field2.YPos<field.YPos){//only fields which are below this one
					continue;
				}
				if(field2.FieldType==SheetFieldType.Drawing){
					continue;
					//drawings do not get moved down.
				}
				if(field.Bounds.IntersectsWith(field2.Bounds)) {
					field2.YPos+=amountOfGrowth;
					affectedFields.Add(field2);
				}
			}
			//Phase 2: Recursion
			foreach(SheetField field2 in affectedFields) {
			  //reuse the same amountOfGrowth again.
			  MoveAllDownWhichIntersect(sheet,field2,amountOfGrowth);
			}
		}

		///<summary>Creates a Sheet object from a sheetDef, complete with fields and parameters.  Sets date to today. If patNum=0, do not save to DB, such as for labels.</summary>
		public static Sheet CreateSheet(SheetDef sheetDef,long patNum=0,bool hidePaymentOptions=false) {
			Sheet sheet=new Sheet();
			sheet.IsNew=true;
			sheet.DateTimeSheet=DateTime.Now;
			sheet.FontName=sheetDef.FontName;
			sheet.FontSize=sheetDef.FontSize;
			sheet.Height=sheetDef.Height;
			sheet.SheetType=sheetDef.SheetType;
			sheet.Width=sheetDef.Width;
			sheet.PatNum=patNum;
			sheet.Description=sheetDef.Description;
			sheet.IsLandscape=sheetDef.IsLandscape;
			sheet.IsMultiPage=sheetDef.IsMultiPage;
			sheet.SheetFields=CreateFieldList(sheetDef.SheetFieldDefs,hidePaymentOptions);//Blank fields with no values. Values filled later from SheetFiller.FillFields()
			sheet.Parameters=sheetDef.Parameters;
			return sheet;
		}

		///<summary>Returns either a user defined statements sheet, the internal sheet if StatementsUseSheets is true. Returns null if StatementsUseSheets is false.</summary>
		public static SheetDef GetStatementSheetDef() {
			if(!PrefC.GetBool(PrefName.StatementsUseSheets)) {
				return null;
			}
			List<SheetDef> listDefs=SheetDefs.GetCustomForType(SheetTypeEnum.Statement);
			if(listDefs.Count>0) {
				return SheetDefs.GetSheetDef(listDefs[0].SheetDefNum);//Return first custom statement. Should be ordred by Description ascending.
			}
			return SheetsInternal.GetSheetDef(SheetInternalType.Statement);
		}

		///<summary>Returns either a user defined MedLabResults sheet or the internal sheet.</summary>
		public static SheetDef GetMedLabResultsSheetDef() {
			List<SheetDef> listDefs=SheetDefs.GetCustomForType(SheetTypeEnum.MedLabResults);
			if(listDefs.Count>0) {
				return SheetDefs.GetSheetDef(listDefs[0].SheetDefNum);//Return first custom statement. Should be ordred by Description ascending.
			}
			return SheetsInternal.GetSheetDef(SheetInternalType.MedLabResults);
		}

		/*
		///<summary>After pulling a list of SheetFieldData objects from the database, we use this to convert it to a list of SheetFields as we create the Sheet.</summary>
		public static List<SheetField> CreateSheetFields(List<SheetFieldData> sheetFieldDataList){
			List<SheetField> retVal=new List<SheetField>();
			SheetField field;
			FontStyle style;
			for(int i=0;i<sheetFieldDataList.Count;i++){
				style=FontStyle.Regular;
				if(sheetFieldDataList[i].FontIsBold){
					style=FontStyle.Bold;
				}
				field=new SheetField(sheetFieldDataList[i].FieldType,sheetFieldDataList[i].FieldName,sheetFieldDataList[i].FieldValue,
					sheetFieldDataList[i].XPos,sheetFieldDataList[i].YPos,sheetFieldDataList[i].Width,sheetFieldDataList[i].Height,
					new Font(sheetFieldDataList[i].FontName,sheetFieldDataList[i].FontSize,style),sheetFieldDataList[i].GrowthBehavior);
				retVal.Add(field);
			}
			return retVal;
		}*/

		///<summary>Creates the initial fields from the sheetDef.FieldDefs.</summary>
		private static List<SheetField> CreateFieldList(List<SheetFieldDef> sheetFieldDefList,bool hidePaymentOptions=false){
			List<SheetField> retVal=new List<SheetField>();
			SheetField field;
			for(int i=0;i<sheetFieldDefList.Count;i++){
				if(hidePaymentOptions && fieldIsPaymentOptionHelper(sheetFieldDefList[i])){
					continue;
				}
				field=new SheetField();
				field.IsNew=true;
				field.FieldName=sheetFieldDefList[i].FieldName;
				field.FieldType=sheetFieldDefList[i].FieldType;
				field.FieldValue=sheetFieldDefList[i].FieldValue;
				field.FontIsBold=sheetFieldDefList[i].FontIsBold;
				field.FontName=sheetFieldDefList[i].FontName;
				field.FontSize=sheetFieldDefList[i].FontSize;
				field.GrowthBehavior=sheetFieldDefList[i].GrowthBehavior;
				field.Height=sheetFieldDefList[i].Height;
				field.RadioButtonValue=sheetFieldDefList[i].RadioButtonValue;
				//field.SheetNum=sheetFieldList[i];//set later
				field.Width=sheetFieldDefList[i].Width;
				field.XPos=sheetFieldDefList[i].XPos;
				field.YPos=sheetFieldDefList[i].YPos;
				field.RadioButtonGroup=sheetFieldDefList[i].RadioButtonGroup;
				field.IsRequired=sheetFieldDefList[i].IsRequired;
				field.TabOrder=sheetFieldDefList[i].TabOrder;
				field.ReportableName=sheetFieldDefList[i].ReportableName;
				field.TextAlign=sheetFieldDefList[i].TextAlign;
				field.ItemColor=sheetFieldDefList[i].ItemColor;
				retVal.Add(field);
			}
			return retVal;
		}

		private static bool fieldIsPaymentOptionHelper(SheetFieldDef sheetFieldDef) {
			if(sheetFieldDef.IsPaymentOption) {
				return true;
			}
			switch(sheetFieldDef.FieldName) {
				case "StatementEnclosed":
				case "StatementAging":
					return true;
			}
			return false;
		}

		///<summary>Typically returns something similar to \\SERVER\OpenDentImages\SheetImages</summary>
		public static string GetImagePath(){
			string imagePath;
			if(!PrefC.AtoZfolderUsed) {
				throw new ApplicationException("Must be using AtoZ folders.");
			}
			imagePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"SheetImages");
			if(!Directory.Exists(imagePath)) {
				Directory.CreateDirectory(imagePath);
			}
			return imagePath;
		}

		///<summary>Typically returns something similar to \\SERVER\OpenDentImages\SheetImages</summary>
		public static string GetPatImagePath() {
			string imagePath;
			if(!PrefC.AtoZfolderUsed) {
				throw new ApplicationException("Must be using AtoZ folders.");
			}
			imagePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"SheetPatImages");
			if(!Directory.Exists(imagePath)) {
				Directory.CreateDirectory(imagePath);
			}
			return imagePath;
		}

		///<summary>Returns the current list of all columns available for the grid in the data table.</summary>
		public static List<DisplayField> GetGridColumnsAvailable(string gridType) {
			int i=0;
			List<DisplayField> retVal=new List<DisplayField>();
			switch(gridType) {
				case "StatementMain":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="date",Description="Date",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="patient",Description="Patient",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="ProcCode",Description="Code",ColumnWidth=45,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="tth",Description="Tooth",ColumnWidth=45,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="description",Description="Description",ColumnWidth=275,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="charges",Description="Charges",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="credits",Description="Credits",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="balance",Description="Balance",ColumnWidth=60,ItemOrder=++i });
					break;
				case "StatementEnclosed":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="AmountDue",Description="Amount Due",ColumnWidth=107,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="DateDue",Description="Date Due",ColumnWidth=107,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="AmountEnclosed",Description="Amount Enclosed",ColumnWidth=107,ItemOrder=++i });
					break;
				case "StatementAging":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age00to30",Description="0-30",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age31to60",Description="31-60",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age61to90",Description="61-90",ColumnWidth=100,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="Age90plus",Description="over 90",ColumnWidth=100,ItemOrder=++i });
					break;
				case "StatementPayPlan":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="date",Description="Date",ColumnWidth=80,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="description",Description="Description",ColumnWidth=250,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="charges",Description="Charges",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="credits",Description="Credits",ColumnWidth=60,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="balance",Description="Balance",ColumnWidth=60,ItemOrder=++i });
					break;
				case "MedLabResults":
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsIDValue",Description="Test / Result",ColumnWidth=500,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsAbnormalFlag",Description="Flag",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsUnits",Description="Units",ColumnWidth=70,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsRefRange",Description="Ref Interval",ColumnWidth=97,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="facilityID",Description="Lab",ColumnWidth=28,ItemOrder=++i });
					break;
			}
			return retVal;
		}

		///<summary></summary>
		public static List<string> GetGridsAvailable(SheetTypeEnum sheetType) {
			List<string> retVal=new List<string>();
			switch(sheetType) {
				case SheetTypeEnum.Statement:
					retVal.Add("StatementAging");
					retVal.Add("StatementEnclosed");
					retVal.Add("StatementMain");
					retVal.Add("StatementPayPlan");
					break;
				case SheetTypeEnum.MedLabResults:
					retVal.Add("MedLabResults");
					break;
			}
			return retVal;
		}

		public static DataTable GetDataTableForGridType(string gridType,Statement stmt=null,MedLab medLab=null) {
			DataTable retVal=new DataTable();
			switch(gridType) {
				case "StatementMain":
					retVal=getTable_StatementMain(stmt);
					break;
				case "StatementAging":
					retVal=getTable_StatementAging(stmt);
					break;
				case "StatementPayPlan":
					retVal=getTable_StatementPayPlan(stmt);
					break;
				case "StatementEnclosed":
					retVal=getTable_StatementEnclosed(stmt);
					break;
				case "MedLabResults":
					retVal=getTable_MedLabResults(medLab);
					break;
				default:
					break;
			}
			return retVal;
		}

		///<summary>Gets account tables by calling AccountModules.GetAccount and then appends dataRows together into a single table. </summary>
		private static DataTable getTable_StatementMain(Statement stmt) {
			DataTable retVal=null;
			DataSet ds=AccountModules.GetAccount(stmt.PatNum,stmt.DateRangeFrom,stmt.DateRangeTo,stmt.Intermingled,stmt.SinglePatient,stmt.StatementNum,
				PrefC.GetBool(PrefName.StatementShowProcBreakdown),PrefC.GetBool(PrefName.StatementShowNotes),stmt.IsInvoice,PrefC.GetBool(PrefName.StatementShowAdjNotes),true,true);
			foreach(DataTable t in ds.Tables) {
				if(!t.TableName.StartsWith("account")) {
					continue;
				}
				if(retVal==null) {//first pass
					retVal=t.Clone();
				}
				foreach(DataRow r in t.Rows) {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && stmt.IsReceipt) {//Canadian. en-CA or fr-CA
						if(r["StatementNum"].ToString()!="0") {//Hide statement rows for Canadian receipts.
							continue;
						}
						if(r["ClaimNum"].ToString()!="0") {//Hide claim rows and claim payment rows for Canadian receipts.
							continue;
						}
						if(PIn.Long(r["ProcNum"].ToString())!=0){
							r["description"]="";//Description: blank in Canada normally because this information is used on taxes and is considered a security concern.
						}
						r["ProcCode"]="";//Code: blank in Canada normally because this information is used on taxes and is considered a security concern.
						r["tth"]="";//Tooth: blank in Canada normally because this information is used on taxes and is considered a security concern.
					}
					if(CultureInfo.CurrentCulture.Name=="en-US"	&& stmt.IsReceipt && r["PayNum"].ToString()=="0") {//Hide everything except patient payments
						continue;
						//js Some additional features would be nice for receipts, such as hiding the bal column, the aging, and the amount due sections.
					}
					if(CultureInfo.CurrentCulture.Name=="en-AU" && r["prov"].ToString().Trim()!="") {//English (Australia)
						r["description"]=r["prov"].ToString()+" - "+r["description"].ToString();
					}
					retVal.ImportRow(r);
				}
				if(t.Rows.Count==0) {
					Patient p=Patients.GetPat(PIn.Long(t.TableName.Replace("account","")));
					if(p==null) {
						p=Patients.GetPat(stmt.PatNum);
					}
					retVal.Rows.Add(
						"",//"AdjNum"          
						"",//"balance"         
						0,//"balanceDouble"   
						"",//"charges"         
						0,//"chargesDouble"   
						"",//"ClaimNum"        
						"",//"ClaimPaymentNum" 
						"",//"clinic"          
						"",//"colorText"       
						"",//"credits"         
						0,//"creditsDouble"   
						DateTime.Today.ToShortDateString(),//"date"            
						DateTime.Today,//"DateTime"        
						Lans.g("Statements","No Account Activity"),//"description"     
						p.FName,//"patient"         
						p.PatNum,//"PatNum"          
						0,//"PayNum"          
						0,//"PayPlanNum"      
						0,//"PayPlanChargeNum"
						"",//"ProcCode"        
						0,//"ProcNum"         
						0,//"ProcNumLab"      
						0,//"procsOnObj"      
						0,//"prov"            
						0,//"StatementNum"    
						"",//"ToothNum"        
						"",//"ToothRange"      
						""//"tth"       
						);
				}
			}
			return retVal;
		}

		private static DataTable getTable_StatementAging(Statement stmt) {
			DataTable retVal=new DataTable();
			retVal.Columns.Add(new DataColumn("Age00to30"));
			retVal.Columns.Add(new DataColumn("Age31to60"));
			retVal.Columns.Add(new DataColumn("Age61to90"));
			retVal.Columns.Add(new DataColumn("Age90plus"));
			Patient guar=Patients.GetPat(Patients.GetPat(stmt.PatNum).Guarantor);
			DataRow row=retVal.NewRow();
			row[0]=guar.Bal_0_30.ToString("F");
			row[1]=guar.Bal_31_60.ToString("F");
			row[2]=guar.Bal_61_90.ToString("F");
			row[3]=guar.BalOver90.ToString("F");
			retVal.Rows.Add(row);
			return retVal;
		}

		private static DataTable getTable_StatementPayPlan(Statement stmt) {
			DataTable retVal=new DataTable();
			DataSet ds=AccountModules.GetAccount(stmt.PatNum,stmt.DateRangeFrom,stmt.DateRangeTo,stmt.Intermingled,stmt.SinglePatient,stmt.StatementNum,PrefC.GetBool(PrefName.StatementShowProcBreakdown),PrefC.GetBool(PrefName.StatementShowNotes),stmt.IsInvoice,PrefC.GetBool(PrefName.StatementShowAdjNotes),true,true);
			foreach(DataTable t in ds.Tables) {
				if(!t.TableName.StartsWith("payplan")) {
					continue;
				}
				retVal=t.Clone();
				foreach(DataRow r in t.Rows) {
					retVal.ImportRow(r);
				}
			}
			return retVal;
		}

		private static DataTable getTable_StatementEnclosed(Statement stmt) {
			DataSet dataSet=AccountModules.GetStatementDataSet(stmt);
			DataTable tableMisc=dataSet.Tables["misc"];
			string text="";
			DataTable table=new DataTable();
			table.Columns.Add(new DataColumn("AmountDue"));
			table.Columns.Add(new DataColumn("DateDue"));
			table.Columns.Add(new DataColumn("AmountEnclosed"));
			DataRow row=table.NewRow();
			Patient patGuar=Patients.GetPat(Patients.GetPat(stmt.PatNum).Guarantor);
			double balTotal=patGuar.BalTotal;
			if(!PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {//this is typical
				balTotal-=patGuar.InsEst;
			}
			for(int m=0;m<tableMisc.Rows.Count;m++) {
				if(tableMisc.Rows[m]["descript"].ToString()=="payPlanDue") {
					balTotal+=PIn.Double(tableMisc.Rows[m]["value"].ToString());
					//payPlanDue;//PatGuar.PayPlanDue;
				}
			}
			InstallmentPlan installPlan=InstallmentPlans.GetOneForFam(patGuar.PatNum);
			if(installPlan!=null) {
				//show lesser of normal total balance or the monthly payment amount.
				if(installPlan.MonthlyPayment < balTotal) {
					text=installPlan.MonthlyPayment.ToString("F");
				}
				else {
					text=balTotal.ToString("F");
				}
			}
			else {//no installmentplan
				text=balTotal.ToString("F");
			}
			row[0]=text;
			if(PrefC.GetLong(PrefName.StatementsCalcDueDate)==-1) {
				text=Lans.g("Statements","Upon Receipt");
			}
			else {
				text=DateTime.Today.AddDays(PrefC.GetLong(PrefName.StatementsCalcDueDate)).ToShortDateString();
			}
			row[1]=text;
			row[2]="";
			table.Rows.Add(row);
			return table;
		}

		private static DataTable getTable_MedLabResults(MedLab medLab) {
			DataTable retval=new DataTable();
			retval.Columns.Add(new DataColumn("obsIDValue"));
			retval.Columns.Add(new DataColumn("obsAbnormalFlag"));
			retval.Columns.Add(new DataColumn("obsUnits"));
			retval.Columns.Add(new DataColumn("obsRefRange"));
			retval.Columns.Add(new DataColumn("facilityID"));
			List<MedLab> listMedLabs=MedLabs.GetForPatAndSpecimen(medLab.PatNum,medLab.SpecimenID,medLab.SpecimenIDFiller);//should always be at least one MedLab
			MedLabs.GetListFacNums(listMedLabs,out _listResults);//refreshes and sorts the classwide _listResults variable
			string obsDescriptPrev="";
			for(int i=0;i<_listResults.Count;i++) {
				//LabCorp requested that these non-performance results not be displayed on the report
				if((_listResults[i].ResultStatus==ResultStatus.F || _listResults[i].ResultStatus==ResultStatus.X)
					&& _listResults[i].ObsValue==""
					&& _listResults[i].Note=="") {
					continue;
				}
				string obsDescript="";
				MedLab medLabCur=MedLabs.GetOne(_listResults[i].MedLabNum);
				if(i==0 || _listResults[i].MedLabNum!=_listResults[i-1].MedLabNum) {
					if(medLabCur.ActionCode!=ResultAction.G) {
						if(obsDescriptPrev==medLabCur.ObsTestDescript) {
							obsDescript=".";
						}
						else {
							obsDescript=medLabCur.ObsTestDescript;
							obsDescriptPrev=obsDescript;
						}
					}
				}
				DataRow row=retval.NewRow();
				string spaces="  ";
				string spaces2="    ";
				string obsVal="";
				int padR=38;
				string newLine="";
				if(obsDescript!="") {
					if(obsDescript==_listResults[i].ObsText) {
						spaces="";
						spaces2="  ";
						padR=40;
					}
					else {
						obsVal+=obsDescript+"\r\n";
						newLine+="\r\n";
					}
				}
				if(_listResults[i].ObsValue=="Test Not Performed") {
					obsVal+=spaces+_listResults[i].ObsText;
				}
				else if(_listResults[i].ObsText=="."
					|| _listResults[i].ObsValue.Contains(":")
					|| _listResults[i].ObsValue.Length>20
					|| medLabCur.ActionCode==ResultAction.G) {
					obsVal+=spaces+_listResults[i].ObsText+"\r\n"+spaces2+_listResults[i].ObsValue.Replace("\r\n","\r\n"+spaces2);
					newLine+="\r\n";
				}
				else {
					obsVal+=spaces+_listResults[i].ObsText.PadRight(padR,' ')+_listResults[i].ObsValue;
				}
				if(_listResults[i].Note!="") {
					obsVal+="\r\n"+spaces2+_listResults[i].Note.Replace("\r\n","\r\n"+spaces2);
				}
				row["obsIDValue"]=obsVal;
				row["obsAbnormalFlag"]=newLine+MedLabResults.GetAbnormalFlagDescript(_listResults[i].AbnormalFlag);
				row["obsUnits"]=newLine+_listResults[i].ObsUnits;
				row["obsRefRange"]=newLine+_listResults[i].ReferenceRange;
				row["facilityID"]=newLine+_listResults[i].FacilityID;
				retval.Rows.Add(row);
			}
			return retval;
		}
	}
}
