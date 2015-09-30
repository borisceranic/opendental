using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Sheets{
		
		///<Summary>Gets one Sheet from the database.</Summary>
		public static Sheet CreateObject(long sheetNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Sheet>(MethodBase.GetCurrentMethod(),sheetNum);
			}
			return Crud.SheetCrud.SelectOne(sheetNum);
		}

		///<summary>Gets a single sheet from the database.  Then, gets all the fields and parameters for it.  So it returns a fully functional sheet.</summary>
		public static Sheet GetSheet(long sheetNum) {
			//No need to check RemotingRole; no call to db.
			Sheet sheet=CreateObject(sheetNum);
			SheetFields.GetFieldsAndParameters(sheet);
			return sheet;
		}

		///<Summary>This is normally done in FormSheetFillEdit, but if we bypass that window for some reason, we can also save a new sheet here.  Does not save any drawings.  Does not save signatures.  Does not save any parameters (PatNum parameters never get saved anyway).</Summary>
		public static void SaveNewSheet(Sheet sheet) {
			//No need to check RemotingRole; no call to db.
			if(!sheet.IsNew) {
				throw new Exception("Only new sheets allowed");
			}
			Insert(sheet);
			foreach(SheetField fld in sheet.SheetFields) {
				fld.SheetNum=sheet.SheetNum;
				SheetFields.Insert(fld);
			}
		}

		///<Summary>Saves a list of sheets to the Database. Only saves new sheets, ignores sheets that are not new.</Summary>
		public static void SaveNewSheetList(List<Sheet> listSheets) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listSheets);
				return;
			}
			for(int i=0;i<listSheets.Count;i++) {
				if(!listSheets[i].IsNew) {
					continue;
				}
				Crud.SheetCrud.Insert(listSheets[i]);
				foreach(SheetField fld in listSheets[i].SheetFields) {
					fld.SheetNum=listSheets[i].SheetNum;
					Crud.SheetFieldCrud.Insert(fld);
				}
			}
		}

		///<summary>Used in FormRefAttachEdit to show all referral slips for the patient/referral combo.  Usually 0 or 1 results.</summary>
		public static List<Sheet> GetReferralSlips(long patNum,long referralNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),patNum,referralNum);
			}
			string command="SELECT * FROM sheet WHERE PatNum="+POut.Long(patNum)
				+" AND EXISTS(SELECT * FROM sheetfield "
				+"WHERE sheet.SheetNum=sheetfield.SheetNum "
				+"AND sheetfield.FieldType="+POut.Long((int)SheetFieldType.Parameter)
				+" AND sheetfield.FieldName='ReferralNum' "
				+"AND sheetfield.FieldValue='"+POut.Long(referralNum)+"')"
				+" ORDER BY DateTimeSheet";
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Used in FormLabCaseEdit to view an existing lab slip.  Will return null if none exist.</summary>
		public static Sheet GetLabSlip(long patNum,long labCaseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Sheet>(MethodBase.GetCurrentMethod(),patNum,labCaseNum);
			}
			string command="SELECT sheet.* FROM sheet,sheetfield "
				+"WHERE sheet.SheetNum=sheetfield.SheetNum"
				+" AND sheet.PatNum="+POut.Long(patNum)
				+" AND sheet.SheetType="+POut.Long((int)SheetTypeEnum.LabSlip)
				+" AND sheetfield.FieldType="+POut.Long((int)SheetFieldType.Parameter)
				+" AND sheetfield.FieldName='LabCaseNum' "
				+"AND sheetfield.FieldValue='"+POut.Long(labCaseNum)+"'";
			return Crud.SheetCrud.SelectOne(command);
		}

		///<summary>Used in FormRxEdit to view an existing rx.  Will return null if none exist.</summary>
		public static Sheet GetRx(long patNum,long rxNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Sheet>(MethodBase.GetCurrentMethod(),patNum,rxNum);
			}
			string command="SELECT sheet.* FROM sheet,sheetfield "
				+"WHERE sheet.PatNum="+POut.Long(patNum)
				+" AND sheet.SheetType="+POut.Long((int)SheetTypeEnum.Rx)
				+" AND sheetfield.FieldType="+POut.Long((int)SheetFieldType.Parameter)
				+" AND sheetfield.FieldName='RxNum' "
				+"AND sheetfield.FieldValue='"+POut.Long(rxNum)+"'";
			return Crud.SheetCrud.SelectOne(command);
		}

		///<summary>Gets all sheets for a patient that have the terminal flag set.</summary>
		public static List<Sheet> GetForTerminal(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM sheet WHERE PatNum="+POut.Long(patNum)
				+" AND ShowInTerminal > 0 ORDER BY ShowInTerminal";
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Get all sheets for a patient for today.</summary>
		public static List<Sheet> GetForPatientForToday(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string datesql="CURDATE()";
			if(DataConnection.DBtype==DatabaseType.Oracle){
				datesql="(SELECT CURRENT_DATE FROM dual)";
			}
			string command="SELECT * FROM sheet WHERE PatNum="+POut.Long(patNum)
				+" AND "+DbHelper.DtimeToDate("DateTimeSheet")+" = "+datesql;
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Get all sheets for a patient.</summary>
		public static List<Sheet> GetForPatient(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM sheet WHERE PatNum="+POut.Long(patNum);
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Get all sheets that reference a given document. Primarily used to prevent deleting an in use document.</summary>
		/// <returns>List of sheets that have fields that reference the given DocNum. Returns empty list if document is not referenced.</returns>
		public static List<Sheet> GetForDocument(long docNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),docNum);
			}
			string command="";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command="SELECT sheet.* FROM sheetfield "
					+"LEFT JOIN sheet ON sheet.SheetNum = sheetfield.SheetNum "
					+"WHERE FieldType = 10 "//PatImage
					+"AND FieldValue = "+POut.Long(docNum)+" "//FieldName == DocCategory, which we do not care about here.
					+"GROUP BY sheet.SheetNum";
			}
			else {//Oracle
				//This query has so much unique Oracle problems that it made more sense to just rewrite it.
				command="SELECT sheet.SheetNum,sheet.SheetType,sheet.PatNum,sheet.DateTimeSheet,sheet.FontSize,sheet.FontName,sheet.Width"
					+",sheet.Height,sheet.IsLandscape,DBMS_LOB.SUBSTR(sheet.InternalNote,1000,1),sheet.Description,sheet.ShowInTerminal,sheet.IsWebForm FROM sheet "
					+"LEFT JOIN sheetfield ON sheet.SheetNum = sheetfield.SheetNum "
					+"WHERE FieldType = 10 "//PatImage
					+"AND TO_CHAR(FieldValue) = '"+POut.Long(docNum)+"' "//FieldName == DocCategory, which we do not care about here.
					+"GROUP BY sheet.SheetNum,sheet.SheetType,sheet.PatNum,sheet.DateTimeSheet,sheet.FontSize,sheet.FontName,sheet.Width"
					+",sheet.Height,sheet.IsLandscape,DBMS_LOB.SUBSTR(sheet.InternalNote,1000,1),sheet.Description,sheet.ShowInTerminal,sheet.IsWebForm";
			}
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Gets the most recent Exam Sheet based on description to fill a patient letter.</summary>
		public static Sheet GetMostRecentExamSheet(long patNum,string examDescript) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Sheet>(MethodBase.GetCurrentMethod(),patNum,examDescript);
			}
			string command="SELECT * FROM sheet WHERE DateTimeSheet="
				+"(SELECT MAX(DateTimeSheet) FROM sheet WHERE PatNum="+POut.Long(patNum)+" "
				+"AND Description='"+POut.String(examDescript)+"') "
				+"AND PatNum="+POut.Long(patNum)+" "
				+"AND Description='"+POut.String(examDescript)+"' "
				+"LIMIT 1";
			return Crud.SheetCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(Sheet sheet) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheet.SheetNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheet);
				return sheet.SheetNum;
			}
			return Crud.SheetCrud.Insert(sheet);
		}

		///<summary></summary>
		public static void Update(Sheet sheet) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheet);
				return;
			}
			Crud.SheetCrud.Update(sheet);
		}

		///<summary></summary>
		public static void DeleteObject(long sheetNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetNum);
				return;
			}
			string command="DELETE FROM sheetfield WHERE SheetNum="+POut.Long(sheetNum);
			Db.NonQ(command);
			Crud.SheetCrud.Delete(sheetNum);
		}

		///<summary>Converts parameters into sheetfield objects, and then saves those objects in the database.  The parameters will never again enjoy full parameter status, but will just be read-only fields from here on out.  It ignores PatNum parameters, since those are already part of the sheet itself.</summary>
		public static void SaveParameters(Sheet sheet){
			//No need to check RemotingRole; no call to db
			SheetField field;
			for(int i=0;i<sheet.Parameters.Count;i++){
				if(sheet.Parameters[i].ParamName=="PatNum"){
					continue;
				}
				field=new SheetField();
				field.IsNew=true;
				field.SheetNum=sheet.SheetNum;
				field.FieldType=SheetFieldType.Parameter;
				field.FieldName=sheet.Parameters[i].ParamName;
				field.FieldValue=sheet.Parameters[i].ParamValue.ToString();//the object will be an int. Stored as a string.
				field.FontSize=0;
				field.FontName="";
				field.FontIsBold=false;
				field.XPos=0;
				field.YPos=0;
				field.Width=0;
				field.Height=0;
				field.GrowthBehavior=GrowthBehaviorEnum.None;
				field.RadioButtonValue="";
				SheetFields.Insert(field);
			}
		}

		///<summary>Loops through all the fields in the sheet and appends together all the FieldValues.  It obviously excludes all SigBox fieldtypes.  It does include Drawing fieldtypes, so any change at all to any drawing will invalidate the signature.  It does include Image fieldtypes, although that's just a filename and does not really have any meaningful data about the image itself.  The order is absolutely critical.</summary>
		public static string GetSignatureKey(Sheet sheet) {
			//No need to check RemotingRole; no call to db
			StringBuilder strBuild=new StringBuilder();
			//The order of sheet fields is absolutely critical when it comes to the signature key.
			//Therefore, we will make a local copy of the sheet fields and sort them how we want them here just in case their order has changed for any other reason.
			List<SheetField> sheetFieldsCopy=new List<SheetField>();
			for(int i=0;i<sheet.SheetFields.Count;i++) {
				sheetFieldsCopy.Add(sheet.SheetFields[i]);
			}
			sheetFieldsCopy.Sort(SheetFields.SortPrimaryKey);
			for(int i=0;i<sheetFieldsCopy.Count;i++) {
				if(sheetFieldsCopy[i].FieldValue=="") {
					continue;
				}
				if(sheetFieldsCopy[i].FieldType==SheetFieldType.SigBox) {
					continue;
				}
				strBuild.Append(sheetFieldsCopy[i].FieldValue);
			}
			return strBuild.ToString();
		}

		public static DataTable GetPatientFormsTable(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum);
			}
			//DataConnection dcon=new DataConnection();
			DataTable table=new DataTable("");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("date");
			table.Columns.Add("dateOnly",typeof(DateTime));//to help with sorting
			table.Columns.Add("dateTime",typeof(DateTime));
			table.Columns.Add("description");
			table.Columns.Add("DocNum");
			table.Columns.Add("imageCat");
			table.Columns.Add("SheetNum");
			table.Columns.Add("showInTerminal");
			table.Columns.Add("time");
			table.Columns.Add("timeOnly",typeof(TimeSpan));//to help with sorting
			//but we won't actually fill this table with rows until the very end.  It's more useful to use a List<> for now.
			List<DataRow> rows=new List<DataRow>();
			//sheet---------------------------------------------------------------------------------------
			string command="SELECT DateTimeSheet,SheetNum,Description,ShowInTerminal "
				+"FROM sheet WHERE PatNum ="+POut.Long(patNum)+" "
				+"AND (SheetType="+POut.Long((int)SheetTypeEnum.PatientForm)+" OR SheetType="+POut.Long((int)SheetTypeEnum.MedicalHistory);
			if(PrefC.GetBool(PrefName.PatientFormsShowConsent)) {
				command+=" OR SheetType="+POut.Long((int)SheetTypeEnum.Consent);//Show consent forms if pref is true.
			}
			command+=")";
				//+"ORDER BY ShowInTerminal";//DATE(DateTimeSheet),ShowInTerminal,TIME(DateTimeSheet)";
			DataTable rawSheet=Db.GetTable(command);
			DateTime dateT;
			for(int i=0;i<rawSheet.Rows.Count;i++) {
				row=table.NewRow();
				dateT=PIn.DateT(rawSheet.Rows[i]["DateTimeSheet"].ToString());
				row["date"]=dateT.ToShortDateString();
				row["dateOnly"]=dateT.Date;
				row["dateTime"]=dateT;
				row["description"]=rawSheet.Rows[i]["Description"].ToString();
				row["DocNum"]="0";
				row["imageCat"]="";
				row["SheetNum"]=rawSheet.Rows[i]["SheetNum"].ToString();
				if(rawSheet.Rows[i]["ShowInTerminal"].ToString()=="0") {
					row["showInTerminal"]="";
				}
				else {
					row["showInTerminal"]=rawSheet.Rows[i]["ShowInTerminal"].ToString();
				}
				if(dateT.TimeOfDay!=TimeSpan.Zero) {
					row["time"]=dateT.ToString("h:mm")+dateT.ToString("%t").ToLower();
				}
				row["timeOnly"]=dateT.TimeOfDay;
				rows.Add(row);
			}
			//document---------------------------------------------------------------------------------------
			command="SELECT DateCreated,DocCategory,DocNum,Description "
				+"FROM document,definition "
				+"WHERE document.DocCategory=definition.DefNum"
				+" AND PatNum ="+POut.Long(patNum)
				+" AND definition.ItemValue LIKE '%F%'";
				//+" ORDER BY DateCreated";
			DataTable rawDoc=Db.GetTable(command);
			long docCat;
			for(int i=0;i<rawDoc.Rows.Count;i++) {
				row=table.NewRow();
				dateT=PIn.DateT(rawDoc.Rows[i]["DateCreated"].ToString());
				row["date"]=dateT.ToShortDateString();
				row["dateOnly"]=dateT.Date;
				row["dateTime"]=dateT;
				row["description"]=rawDoc.Rows[i]["Description"].ToString();
				row["DocNum"]=rawDoc.Rows[i]["DocNum"].ToString();
				docCat=PIn.Long(rawDoc.Rows[i]["DocCategory"].ToString());
				row["imageCat"]=DefC.GetName(DefCat.ImageCats,docCat);
				row["SheetNum"]="0";
				row["showInTerminal"]="";
				if(dateT.TimeOfDay!=TimeSpan.Zero) {
					row["time"]=dateT.ToString("h:mm")+dateT.ToString("%t").ToLower();
				}
				row["timeOnly"]=dateT.TimeOfDay;
				rows.Add(row);
			}
			//Sorting
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			DataView view = table.DefaultView;
			view.Sort = "dateOnly,showInTerminal,timeOnly";
			table = view.ToTable();
			return table;
		}

		///<summary>Returns all sheets for the given patient in the given date range which have a description matching the examDescript in a case insensitive manner. If examDescript is blank, then sheets with any description are returned.</summary>
		public static List<Sheet> GetExamSheetsTable(long patNum,DateTime startDate,DateTime endDate,string examDescript) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sheet>>(MethodBase.GetCurrentMethod(),patNum,startDate,endDate,examDescript);
			}
			string command="SELECT * "
				+"FROM sheet WHERE PatNum="+POut.Long(patNum)+" "
				+"AND SheetType="+POut.Int((int)SheetTypeEnum.ExamSheet)+" ";
			if(examDescript!=""){
				command+="AND Description LIKE '%"+POut.String(examDescript)+"%' ";//case insensitive text matches
			}
			command+="AND "+DbHelper.DtimeToDate("DateTimeSheet")+">="+POut.Date(startDate)+" AND "+DbHelper.DtimeToDate("DateTimeSheet")+"<="+POut.Date(endDate)+" "
				+"ORDER BY DateTimeSheet";
			return Crud.SheetCrud.SelectMany(command);
		}

		///<summary>Used to get sheets filled via the web.</summary>
		public static DataTable GetWebFormSheetsTable(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			DataTable table=new DataTable("");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("date");
			table.Columns.Add("dateOnly",typeof(DateTime));//to help with sorting
			table.Columns.Add("dateTime",typeof(DateTime));
			table.Columns.Add("description");
			table.Columns.Add("time");
			table.Columns.Add("timeOnly",typeof(TimeSpan));//to help with sorting
			table.Columns.Add("PatNum");
			table.Columns.Add("SheetNum");
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT DateTimeSheet,Description,PatNum,SheetNum "
				+"FROM sheet WHERE " 
				+"DateTimeSheet >= "+POut.Date(dateFrom)+" AND DateTimeSheet <= "+POut.Date(dateTo.AddDays(1))+ " "
				+"AND IsWebForm = "+POut.Bool(true)+ " "
				+"AND (SheetType="+POut.Long((int)SheetTypeEnum.PatientForm)+" OR SheetType="+POut.Long((int)SheetTypeEnum.MedicalHistory)+") ";
			DataTable rawSheet=Db.GetTable(command);
			DateTime dateT;
			for(int i=0;i<rawSheet.Rows.Count;i++) {
				row=table.NewRow();
				dateT=PIn.DateT(rawSheet.Rows[i]["DateTimeSheet"].ToString());
				row["date"]=dateT.ToShortDateString();
				row["dateOnly"]=dateT.Date;
				row["dateTime"]=dateT;
				row["description"]=rawSheet.Rows[i]["Description"].ToString();
				row["PatNum"]=rawSheet.Rows[i]["PatNum"].ToString();
				row["SheetNum"]=rawSheet.Rows[i]["SheetNum"].ToString();
				if(dateT.TimeOfDay!=TimeSpan.Zero) {
					row["time"]=dateT.ToString("h:mm")+dateT.ToString("%t").ToLower();
				}
				row["timeOnly"]=dateT.TimeOfDay;
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			DataView view = table.DefaultView;
			view.Sort = "dateOnly,timeOnly";
			table = view.ToTable();
			return table;
		}

		public static bool ContainsStaticField(Sheet sheet,string fieldName) {
			//No need to check RemotingRole; no call to db
			foreach(SheetField field in sheet.SheetFields) {
				if(field.FieldType!=SheetFieldType.StaticText) {
					continue;
				}
				if(field.FieldValue.Contains("["+fieldName+"]")) {
					return true;
				}
			}
			return false;
		}

		///<summary></summary>
		public static byte GetBiggestShowInTerminal(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<byte>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT MAX(ShowInTerminal) FROM sheet WHERE PatNum="+POut.Long(patNum);
			return PIn.Byte(Db.GetScalar(command));
		}

		///<summary></summary>
		public static void ClearFromTerminal(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="UPDATE sheet SET ShowInTerminal=0 WHERE PatNum="+POut.Long(patNum);
			Db.NonQ(command);
		}

		///<summary>This gives the number of pages required to print all fields. This must be calculated ahead of time when creating multi page pdfs.</summary>
		public static int CalculatePageCount(Sheet sheet,System.Drawing.Printing.Margins m) {
			//HeightLastField takes the bottom bounds (sum of lengths of Y. Ex. Y=0 to Y=1099, bounds=1100).
			//HeightPage is the value of Width/Length depending on Landscape/Portrait.
			if(sheet.HeightLastField<=sheet.HeightPage && sheet.SheetType!=SheetTypeEnum.MedLabResults) {//MedLabResults always implements footer, needs true multi-page count
				return 1;//if all of the fields are less than one page, even if some of the fields fall within the margin of the first page.
			}
			if(SheetTypeIsSinglePage(sheet.SheetType)) {
				return 1;//labels and RX forms are always single pages
			}
			SetPageMargin(sheet,m);
			int printableHeightPerPage=(sheet.HeightPage-(m.Top+m.Bottom));
			if(printableHeightPerPage<1) {
				return 1;//otherwise we get negative, infinite, or thousands of pages.
			}
			int maxY=0;
			for(int i=0;i<sheet.SheetFields.Count;i++) {
				maxY=Math.Max(maxY,sheet.SheetFields[i].Bounds.Bottom);
			}
			int pageCount=1;
			maxY-=m.Top;//adjust for ignoring the top margin of the first page.
			pageCount=Convert.ToInt32(Math.Ceiling((double)maxY/printableHeightPerPage));
			pageCount=Math.Max(pageCount,1);//minimum of at least one page.
			return pageCount;
		}

		public static void SetPageMargin(Sheet sheet,System.Drawing.Printing.Margins m) {
			m.Left=0;
			m.Right=0;
			if(SheetTypeIsSinglePage(sheet.SheetType)) {
				m.Top=0;
				m.Bottom=0;
				//m=new System.Drawing.Printing.Margins(0,0,0,0); //does not work, creates new reference.
			}
			else {
				m.Top=40;
				if(sheet.SheetType==SheetTypeEnum.MedLabResults) {
					m.Top=120;
				}
				m.Bottom=60;
			}
			return;
		}

		public static bool SheetTypeIsSinglePage(SheetTypeEnum sheetType) {
			switch(sheetType) {
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelReferral:
				//case SheetTypeEnum.ReferralSlip:
				case SheetTypeEnum.LabelAppointment:
				case SheetTypeEnum.Rx:
				//case SheetTypeEnum.Consent:
				//case SheetTypeEnum.PatientLetter:
				//case SheetTypeEnum.ReferralLetter:
				//case SheetTypeEnum.PatientForm:
				//case SheetTypeEnum.RoutingSlip:
				//case SheetTypeEnum.MedicalHistory:
				//case SheetTypeEnum.LabSlip:
				//case SheetTypeEnum.ExamSheet:
				case SheetTypeEnum.DepositSlip:
				//case SheetTypeEnum.Statement:
					return true;
			}
			return false;
		}

		
		

		

	}
}