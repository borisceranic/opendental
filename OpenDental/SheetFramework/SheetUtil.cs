using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using OpenDentBusiness;
using CodeBase;
using System.Data;
using OpenDental.UI;

namespace OpenDental{
	public class SheetUtil {
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
		public static void CalculateHeights(Sheet sheet,Graphics g){
			//Sheet sheetCopy=sheet.Clone();
			int calcH;
			Font font;
			FontStyle fontstyle;
			foreach(SheetField field in sheet.SheetFields) {
				if(sheet.SheetType==SheetTypeEnum.Statement && field.IsPaymentOption && !sheet.GArgs.ShowPaymentOptions) {
					continue;//skip payment option fields on statments if neccesary
				}
				if(field.FieldType==SheetFieldType.Image 
					||field.FieldType==SheetFieldType.PatImage) {
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
					if(field.FieldName=="Patient Info.gif"
					||File.Exists(filePathAndName)) {
						continue;
					}
					else {//img doesn't exist or we do not have access to it.
						field.Height=0;//Set height to zero so that it will not cause extra pages to print.
					}
					#endregion
				}
				if(field.GrowthBehavior==GrowthBehaviorEnum.None){
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
					calcH=CalculateGridHeightHelper(field,sheet,g);
				}
				if(field.FieldType==SheetFieldType.Grid) {
					SheetGridDef grid=SheetGridDefs.GetOne(field.FKey);
					if(calcH<=field.Height 
						&& grid.GridType!=SheetGridType.StatementPayPlan) //0 height payment plan should shrink/not be drawn. All fields below should be moved up.
					{
						continue;
					}
				}
				else if(calcH<=field.Height ){
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
			//return sheetCopy;
		}

		///<summary>Calculates height of grid taking into account page breaks, word wrapping, cell width, font size, and actual data to be used to fill this grid.</summary>
		private static int CalculateGridHeightHelper(SheetField field,Sheet sheet,Graphics g) {
			UI.ODGrid odGrid=new UI.ODGrid();//Only used for measurements. That way if OD Grid is ever drawn Differently, it should affect this behavior as well.
			odGrid.Width=field.Width;
			odGrid.HideScrollBars=true;
			int _pageCount=0;
			int yPosGrid=field.YPos;
			//Add Title and Header header height at begining of the grid
			if(yPosGrid+odGrid.TitleHeight+odGrid.HeaderHeight>sheet.HeightPage-60) {
				//grid is placed near bottom of page, increase yPosGrid to the value of the start of the next page.
				yPosGrid+=(sheet.HeightPage-60)-(yPosGrid%sheet.HeightPage);
				//Example: grid at bottom of second page, each page is 1100px, bottom margin is 60px.
				//2130+=(1100-60)-(2130%1100)
				//2130+=(1040)-(1030)
				//2130+=10
				//The content of the third page starts at 2140, which is exactly what the math worked out to.
			}
			SheetGridDef fGrid=SheetGridDefs.GetOne(field.FKey);
//#warning fix this: should not just set to all defulat columns.
//			fGrid.Columns=SheetGridDefs.GetColumnsAvailable(fGrid.GridType);//SheetGridColDefs
			odGrid.Title=fGrid.Title;
			//SheetGridDefs.GridArgs gArgs=new SheetGridDefs.GridArgs { patnum=sheet.PatNum,StartDate=DateTime.Parse("2014-11-01"),StopDate=DateTime.Parse("2014-11-30") };
			DataTable Table=SheetGridDefs.GetDataTableForGridType(fGrid.GridType,sheet.GArgs);
			#region  Fill Grid
			odGrid.BeginUpdate();
			odGrid.Columns.Clear();
			ODGridColumn col;
			for(int i=0;i<fGrid.Columns.Count;i++){
				col=new ODGridColumn(fGrid.Columns[i].DisplayName,fGrid.Columns[i].Width);
				odGrid.Columns.Add(col);
			}
			ODGridRow row;
			for(int i=0;i<Table.Rows.Count;i++){
				row=new ODGridRow();
				for(int c=0;c<fGrid.Columns.Count;c++){//Selectively fill columns from the dataTable into the odGrid.
					row.Cells.Add(Table.Rows[i][fGrid.Columns[c].ColName].ToString());
				}
				odGrid.Rows.Add(row);
			}
			odGrid.EndUpdate();//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			#endregion
			bool drawTitle=SheetGridDefs.gridHasDefaultTitle(fGrid.GridType);
			bool drawHeader=true;
			bool drawFooter=false;
			if(fGrid.GridType==SheetGridType.StatementMain && !sheet.GArgs.Intermingled) {
				drawTitle=true;
			}
			int pageBreak=SheetPrinting.bottomCurPage(yPosGrid,sheet,out _pageCount);
			//odGrid.DrawTitleAndHeaders(g,field.XPos,yPosGrid);
			//yPosGrid+=odGrid.TitleHeight+odGrid.HeaderHeight;
			//Add each row height, add blank space for the end of each page and headers on the next page.
			for(int i=0;i<odGrid.RowHeights.Length;i++) {
				#region Split patient accounts on Statment grids.
				if(fGrid.GridType==SheetGridType.StatementMain
					&& !sheet.GArgs.Intermingled
					&& i>0 
					&& Table.Rows[i]["patient"].ToString()!=Table.Rows[i-1]["patient"].ToString()) 
				{//
					yPosGrid+=20;//space out grids.
					drawTitle=true;
					drawHeader=true;
				}
				#endregion
				#region Page break logic
				if(fGrid.GridType==SheetGridType.StatementPayPlan && i==odGrid.RowHeights.Length-1) {
					drawFooter=true;
				}
				if(yPosGrid+odGrid.RowHeights[i]+(drawTitle?odGrid.TitleHeight:0)+(drawHeader?odGrid.HeaderHeight:0)+(drawFooter?odGrid.TitleHeight:0)>pageBreak) {
					yPosGrid=pageBreak+1;
					pageBreak=SheetPrinting.bottomCurPage(yPosGrid,sheet,out _pageCount);
					drawHeader=true;
				}
				#endregion
				if(drawTitle) {
					yPosGrid+=odGrid.TitleHeight;
					drawTitle=false;
				}
				if(drawHeader) {
					yPosGrid+=odGrid.HeaderHeight;
					drawHeader=false;
				}
				yPosGrid+=odGrid.RowHeights[i];
				if(drawFooter) {
					yPosGrid+=odGrid.TitleHeight+2;
				}
			}
			return yPosGrid-field.YPos;
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

		///<summary>Creates a Sheet object from a sheetDef, complete with fields and parameters.  This overload is only to be used when the sheet will not be saved to the database, such as for labels</summary>
		public static Sheet CreateSheet(SheetDef sheetDef) {
			return CreateSheet(sheetDef,0);
		}

		///<summary>Creates a Sheet object from a sheetDef, complete with fields and parameters.  Sets date to today.</summary>
		public static Sheet CreateSheet(SheetDef sheetDef,long patNum) {
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
			sheet.SheetFields=CreateFieldList(sheetDef.SheetFieldDefs);//Blank fields with no values. Values filled later from SheetFiller.FillFields()
			sheet.Parameters=sheetDef.Parameters;
			return sheet;
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
		private static List<SheetField> CreateFieldList(List<SheetFieldDef> sheetFieldDefList){
			List<SheetField> retVal=new List<SheetField>();
			SheetField field;
			for(int i=0;i<sheetFieldDefList.Count;i++){
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
				field.FKey=sheetFieldDefList[i].FKey;
				field.TextAlign=sheetFieldDefList[i].TextAlign;
				field.IsPaymentOption=sheetFieldDefList[i].IsPaymentOption;
				field.ItemColor=sheetFieldDefList[i].ItemColor;
				retVal.Add(field);
			}
			return retVal;
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
		


	}
}
