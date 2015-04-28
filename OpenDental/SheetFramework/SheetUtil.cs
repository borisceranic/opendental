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
using System.Globalization;

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
			//return sheetCopy;
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
#warning Cameron12345 Remove this comment block if releasing MedLabs
			//List<SheetDef> listDefs=SheetDefs.GetCustomForType(SheetTypeEnum.MedLabResults);
			//if(listDefs.Count>0) {
			//	return SheetDefs.GetSheetDef(listDefs[0].SheetDefNum);//Return first custom statement. Should be ordred by Description ascending.
			//}
			//return SheetsInternal.GetSheetDef(SheetInternalType.MedLabResults);
			return null;
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
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsIDValue",Description="Test / Result",ColumnWidth=506,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsAbnormalFlag",Description="Flag",ColumnWidth=78,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsUnits",Description="Units",ColumnWidth=56,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="obsRefRange",Description="Ref Interval",ColumnWidth=75,ItemOrder=++i });
					retVal.Add(new DisplayField { Category=DisplayFieldCategory.None,InternalName="facilityID",Description="Lab",ColumnWidth=35,ItemOrder=++i });
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
			Dictionary<long,string> dictLabNumLabId=SheetUtil.GetDictFacNumFacId(listMedLabs);
			for(int i=0;i<_listResults.Count;i++) {
				//LabCorp requested that these non-performance results not be displayed on the report
				if(_listResults[i].ResultStatus==ResultStatus.F
					&& _listResults[i].ObsValue==""
					&& _listResults[i].Note=="")
				{
					continue;
				}
				DataRow row=retval.NewRow();
				string spaces="    ";
				string obsVal=_listResults[i].ObsText+"\r\n"+spaces+_listResults[i].ObsValue.Replace("\r\n","\r\n"+spaces);
				if(_listResults[i].Note!="") {
					obsVal+="\r\n"+spaces;
				}
				obsVal+=_listResults[i].Note.Replace("\r\n","\r\n"+spaces);
				row["obsIDValue"]=obsVal;
				row["obsAbnormalFlag"]=MedLabResults.GetAbnormalFlagDescript(_listResults[i].AbnormalFlag);
				row["obsUnits"]=_listResults[i].ObsUnits;
				row["obsRefRange"]=_listResults[i].ReferenceRange;
				row["facilityID"]=_listResults[i].FacilityID;
				retval.Rows.Add(row);
			}
			return retval;
		}

		///<summary>Returns a dictionary linking the MedLabFacilityNum on each result to a facility ID that is unique for the report.
		///Each message has a facility or facilities with footnote IDs, e.g. 01, 02, etc.  The results each link to the facility that performed the test.
		///But if there are multiple messages for a test order, e.g. when there is a final result for a subset of the original test results,
		///the additional message may have a facility with footnote ID of 01 that is different than the original message facility with ID 01.
		///So each ID could link to multiple facilities.  We will have to append _1, _2, etc to differentiate them on the report.</summary>
		public static Dictionary<long,string> GetDictFacNumFacId(List<MedLab> listMedLabs) {
			_listResults=MedLabResults.GetAllForLabs(listMedLabs);//use the classwide variable so we can use the list to create the data table
			for(int i=_listResults.Count-1;i>-1;i--) {//loop through backward and only keep the most final/most recent result
				if(i==0) {
					break;
				}
				if(_listResults[i].ObsID==_listResults[i-1].ObsID && _listResults[i].ObsIDSub==_listResults[i-1].ObsIDSub) {
					_listResults.RemoveAt(i);
				}
			}
			_listResults.Sort(SortResultsByPriKey);
			//_listResults will now only contain the most recent or most final/corrected results, sorted by the order inserted in the db
			Dictionary<long,string> dictMedLabNumLabID=new Dictionary<long,string>();
			for(int i=0;i<_listResults.Count;i++) {
				List<MedLabFacAttach> listFacAttaches=MedLabFacAttaches.GetAllForLabOrResult(0,_listResults[i].MedLabResultNum);
				if(listFacAttaches.Count==0) {
					continue;
				}
				//each result may have been processed at more than one facility, but we will only show the most recent on the report
				if(dictMedLabNumLabID.ContainsKey(listFacAttaches[0].MedLabFacilityNum)) {
					if(dictMedLabNumLabID[listFacAttaches[0].MedLabFacilityNum].Contains("_")
						&& !dictMedLabNumLabID.ContainsValue(_listResults[i].FacilityID))
					{
						dictMedLabNumLabID[listFacAttaches[0].MedLabFacilityNum]=_listResults[i].FacilityID;
					}
					else {
						_listResults[i].FacilityID=dictMedLabNumLabID[listFacAttaches[0].MedLabFacilityNum];
					}
					continue;
				}
				//if the facility ID is already linked to a different facilitynum, add the facilitynum by not the ID
				//the facility may be referenced by a different ID in another result, and we will use that result ID for all results that reference this facility
				if(dictMedLabNumLabID.ContainsValue(_listResults[i].FacilityID)) {
					//we need to find a unique ID for this facility
					int appendNum=0;
					string val=_listResults[i].FacilityID;
					while(dictMedLabNumLabID.ContainsValue(val)) {
						appendNum++;
						val=_listResults[i].FacilityID+"_"+appendNum;
					}
					dictMedLabNumLabID.Add(listFacAttaches[0].MedLabFacilityNum,val);
					_listResults[i].FacilityID=val;
					continue;
				}
				//the dictionary doesn't contain the facilitynum or ID, so add them
				dictMedLabNumLabID.Add(listFacAttaches[0].MedLabFacilityNum,_listResults[i].FacilityID);
			}
			//update any IDs on the results that have an "_" in them, in case we found a valid ID in a subsequent result
			for(int i=0;i<_listResults.Count;i++) {
				if(!_listResults[i].FacilityID.Contains("_")) {
					continue;
				}
				List<MedLabFacAttach> listFacAttaches=MedLabFacAttaches.GetAllForLabOrResult(0,_listResults[i].MedLabResultNum);
				if(listFacAttaches.Count==0) {
					continue;
				}
				_listResults[i].FacilityID=dictMedLabNumLabID[listFacAttaches[0].MedLabFacilityNum];
			}
			return dictMedLabNumLabID;
		}

		///<summary>Sort by MedLabResult.MedLabResultNum.</summary>
		private static int SortResultsByPriKey(MedLabResult medLabResultX,MedLabResult medLabResultY) {
			return medLabResultX.MedLabResultNum.CompareTo(medLabResultY.MedLabResultNum);
		}
	}
}
