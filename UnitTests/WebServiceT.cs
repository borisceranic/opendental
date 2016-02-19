using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using OpenDental;
using OpenDentBusiness;

namespace UnitTests {
	public class WebServiceT {
		/// <summary></summary>
		public static List<string> RunAll() {
			List<string> retVal=new List<string>();
			List<string> strErrors;
			Stopwatch s=new Stopwatch();
			#region GetString
			s.Start();
			string strResult=WebServiceTests.GetString("Input");
			s.Stop();
			if(strResult=="Processed: Input") {
				retVal.Add("GetString: Passed.");
			}
			else {
				retVal.Add("GetString: Failed.  The string should be 'Processed: Input' but returned '"+(strResult??"null")+"'.");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetString
			#region GetStringLong
			int intStrLen=1000000;
			string strAlphNumLong=CoreTypesT.CreateRandomAlphaNumericString(intStrLen);
			s.Restart();
			strResult=WebServiceTests.GetString(strAlphNumLong);
			s.Stop();
			if(strResult=="Processed: "+strAlphNumLong) {
				retVal.Add("GetStringLong: Passed.");
			}
			else {
				retVal.Add("GetStringLong: Failed.");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with string of length "+intStrLen;
			#endregion GetStringLong
			#region GetStringDirty
			s.Restart();
			strResult=WebServiceTests.GetString(WebServiceTests.DirtyString);
			s.Stop();
			if(strResult=="Processed: "+WebServiceTests.DirtyString) {
				retVal.Add("GetStringDirty: Passed.");
			}
			else {
				retVal.Add("GetStringDirty: Failed.");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with string of length "+WebServiceTests.DirtyString.Length;
			#endregion GetStringDirty
			#region GetStringMaxSize
			//out of memory exception when trying to create a string this large???
			//string str=new string('A',int.MaxValue);
			//try {
			//	s.Restart();
			//	strResult=WebServiceTests.GetString(str);
			//	s.Stop();
			//	retVal.Add("GetStringMaxSize: Passed.");
			//}
			//catch(Exception ex) {
			//	s.Stop();
			//	retVal.Add("GetStringMaxSize: Failed.  Exception message "+ex.Message+"  "+ex.StackTrace+".");
			//}
			//retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with string of length "+int.MaxValue;
			#endregion GetStringMaxSize
			#region GetStringNull
			s.Restart();
			strResult=WebServiceTests.GetStringNull();
			s.Stop();
			if(strResult==null) {
				retVal.Add("GetStringNull: Passed.");
			}
			else {
				retVal.Add("GetStringNull: Failed.  The string should be null but returned '"+strResult+"'.");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetStringNull
			#region GetStringCarriageReturn
			s.Restart();
			strResult=WebServiceTests.GetStringCarriageReturn(WebServiceTests.NewLineString);
			s.Stop();
			if(strResult=="Processed: "+WebServiceTests.NewLineString) {
				retVal.Add("GetStringCarriageReturn: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetStringCarriageReturn: Failed.  The string should be '{0}' but returned {1}.",
					"Processed: "+WebServiceTests.NewLineString,(strResult==null?"null":("'"+strResult+"'"))));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetStringCarriageReturn
			#region GetInt
			s.Restart();
			int intResult=WebServiceTests.GetInt(1);
			s.Stop();
			if(intResult==2) {
				retVal.Add("GetInt: Passed.");
			}
			else {
				retVal.Add("GetInt: Failed.  Should be 2 but returned "+intResult+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetInt
			#region GetLong
			s.Restart();
			long longResult=WebServiceTests.GetLong(1);
			s.Stop();
			if(longResult==2) {
				retVal.Add("GetLong: Passed.");
			}
			else {
				retVal.Add("GetLong: Failed.  Should be 2 but returned "+longResult+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetLong
			#region GetVoid
			try {
				s.Restart();
				WebServiceTests.GetVoid();
				s.Stop();
				retVal.Add("GetVoid: Passed.");
			}
			catch(Exception ex) {
				s.Stop();
				retVal.Add("GetVoid: Failed.  Exception message "+ex.Message+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetVoid
			#region GetBool
			s.Restart();
			bool boolResult=WebServiceTests.GetBool();
			s.Stop();
			if(boolResult==true) {
				retVal.Add("GetBool: Passed.");
			}
			else {
				retVal.Add("GetBool: Failed.  Should be true but returned "+boolResult+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetBool
			#region GetObjectPat
			s.Restart();
			Patient pat=WebServiceTests.GetObjectPat();
			s.Stop();
			strErrors=new List<string>();
			if(pat==null) {
				strErrors.Add("The patient returned is null.");
			}
			else {
				if(pat.FName!=null) {
					strErrors.Add("The patient.FName should be null but returned '"+pat.FName+"'.");
				}
				if(pat.LName!="Smith") {
					strErrors.Add("The patient.LName should be 'Smith' but returned "+(pat.LName==null?"null":("'"+pat.LName+"'"))+"'.");
				}
				if(pat.AddrNote!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The patient.AddrNote should be '{0}' but returned {1}.",WebServiceTests.DirtyString,
						pat.AddrNote==null?"null":("'"+pat.AddrNote+"'")));
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetObjectPat: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetObjectPat: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with AddrNote containing dirty string of length "+WebServiceTests.DirtyString.Length;
			#endregion GetObjectPat
			#region GetTable
			s.Restart();
			DataTable table=WebServiceTests.GetTable();
			s.Stop();
			if(table!=null && table.Rows!=null && table.Rows.Count>0 && table.Rows[0]["Col1"]!=null && table.Rows[0]["Col1"].ToString()=="cell00") {
				retVal.Add("GetTable: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetTable: Failed.  The DataTable cell should be '{0}' but returned {1}.","cell00",
					table==null?"a null table":table.Rows==null?"a null DataRowCollection":table.Rows.Count<1?"an insufficient number of rows":
					table.Rows[0]["Col1"]==null?"a null cell":("'"+table.Rows[0]["Col1"]+"'")));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetTable
			#region GetTableCarriageReturn
			s.Restart();
			table=WebServiceTests.GetTableCarriageReturn();
			s.Stop();
			if(table!=null && table.Rows!=null && table.Rows.Count>0 && table.Columns.Count>0 && table.Rows[0]!=null && table.Rows[0][0]!=null
				&& table.Rows[0][0].ToString()==WebServiceTests.NewLineString)
			{
				retVal.Add("GetTableCarriageReturn: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetTableCarriageReturn: Failed.  The table cell should be '{0}' but returned {1}.",WebServiceTests.NewLineString,
					table==null?"a null table":table.Rows==null?"a null DataRowCollection":table.Rows.Count<1?"an insufficient number of rows":
					table.Columns.Count<1?"an insufficient number of columns":table.Rows[0]==null?"a null DataRow":table.Rows[0][0]==null?"a null cell":
					("'"+table.Rows[0][0]+"'")));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetTableCarriageReturn
			#region GetTable2by3
			s.Restart();
			table=WebServiceTests.GetTable2by3();
			s.Stop();
			strErrors=new List<string>();
			for(int i = 0;i<table.Rows.Count;i++) {
				for(int j = 0;j<table.Columns.Count;j++) {
					if(table.Rows[i][j].ToString()!="cell"+i+j) {
						strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.","cell"+i+j,table.Rows[i][j]));
					}
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetTable2by3: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetTable2by3: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetTable2by3
			#region GetTableSpecialChars
			s.Restart();
			table=WebServiceTests.GetTableSpecialChars();
			s.Stop();
			char[] chars={'|','<','>','&','\'','"','\\','/'};
			strErrors=new List<string>();
			for(int i=0;i<table.Rows.Count;i++) {
				for(int j=0;j<table.Columns.Count-1;j++) {//last column is for DirtyString
					if(table.Rows[i][j].ToString()!="cell"+i+j+chars[i*2+j]) {
						strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.","cell"+i+j+chars[i*2+j],table.Rows[i][j]));
					}
				}
			}
			if(table.Rows[0]["DirtyString"].ToString()!=WebServiceTests.DirtyString) {
				strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.",WebServiceTests.DirtyString,table.Rows[0]["DirtyString"]));
			}
			if(strErrors.Count==0) {
				retVal.Add("GetTableSpecialChars: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetTableSpecialChars: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with cell containing dirty string of length "+WebServiceTests.DirtyString.Length;
			#endregion GetTableSpecialChars
			#region GetTableDataTypes
			s.Restart();
			table=WebServiceTests.GetTableDataTypes();
			s.Stop();
			strErrors=new List<string>();
			if(table==null || table.Rows==null || table.Columns==null || table.Rows.Count<1 || table.Rows[0]==null) {
				strErrors.Add(table==null?"The DataTable is null.":table.Rows==null?"The DataRowCollection is null.":
					table.Columns==null?"The DataColumnCollection is null.":table.Rows.Count<1?"The DataRowCollection is empty.":"The DataRow is null.");
			}
			else {
				if(table.Columns.Count<1 || table.Rows[0][0]==null || table.Rows[0][0].GetType()!=typeof(string)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(string),
						table.Columns.Count<1?"an insufficient column count":table.Rows[0][0]==null?"a null cell":table.Rows[0][0].GetType().ToString()));
				}
				if(table.Columns.Count<2 || table.Rows[0][1]==null || table.Rows[0][1].GetType()!=typeof(decimal)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(decimal),
						table.Columns.Count<2?"an insufficient column count":table.Rows[0][1]==null?"a null cell":table.Rows[0][1].GetType().ToString()));
				}
				if(table.Columns.Count<3 || table.Rows[0][2]==null || table.Rows[0][2].GetType()!=typeof(DateTime)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(DateTime),
						table.Columns.Count<3?"an insufficient column count":table.Rows[0][2]==null?"a null cell":table.Rows[0][2].GetType().ToString()));
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetTableDataTypes: Passed.");
			}
			else {
				retVal.Add("GetTableDataTypes: Failed.  "+string.Join("  ",strErrors));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetTableDataTypes
			#region GetDataSet
			s.Restart();
			DataSet ds=WebServiceTests.GetDataSet();
			s.Stop();
			strErrors=new List<string>();
			if(ds==null || ds.Tables==null || ds.Tables.Count<1 || ds.Tables[0]==null || ds.Tables[0].TableName!="table0") {
				strErrors.Add(string.Format("The DataTable's name in the DataSet should be {0} but returned {1}.","table0",
					ds==null?"a null DataSet":ds.Tables==null?"a null DataTableCollection":ds.Tables.Count<1?"an empty DataTableCollection":
					ds.Tables[0]==null?"a null DataTable":ds.Tables[0].TableName??"a null TableName"));
			}
			if(ds==null || ds.Tables==null || ds.Tables.Count<1 || ds.Tables[0]==null || ds.Tables[0].Rows.Count<1
				|| ds.Tables[0].Rows[0]["DirtyString"].ToString()!=WebServiceTests.DirtyString)
			{
				strErrors.Add(string.Format(@"The cell value in the DataSet should be {0} but returned {1}.",WebServiceTests.DirtyString,
					ds==null?"a null DataSet":ds.Tables==null?"a null DataTableCollection":ds.Tables.Count<1?"an empty DataTableCollection":
					ds.Tables[0]==null?"a null DataTable":ds.Tables[0].Rows.Count<1?"an empty DataRowCollection":
					ds.Tables[0].Rows[0]["DirtyString"]??"a null cell"));
			}
			if(strErrors.Count==0) {
				retVal.Add("GetDataSet: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetDataSet: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with cell containing dirty string of length "+WebServiceTests.DirtyString.Length;
			#endregion GetDataSet
			#region GetListInt
			s.Restart();
			List<int> listInt=WebServiceTests.GetListInt();
			s.Stop();
			if(listInt!=null && listInt.Count>0 && listInt[0]==2) {
				retVal.Add("GetListInt: Passed.");
			}
			else {
				retVal.Add("GetListInt: Failed.  The list of ints should contain 2 but returned "+
					(listInt==null?"a null list":listInt.Count<1?"an empty list":listInt[0].ToString())+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetListInt
			#region GetArrayPatient
			s.Restart();
			Patient[] arrayPat=WebServiceTests.GetArrayPatient();
			s.Stop();
			strErrors=new List<string>();
			if(arrayPat==null || arrayPat.Length<2) {
				strErrors.Add(arrayPat==null?"The patient array is null.":"The patient array contains an insufficient number of patients.");
			}
			else {
				if(arrayPat[0]==null || arrayPat[0].LName!="Jones") {
					strErrors.Add(string.Format("The patient in the array should have the LName {0} but returned {1}.","Jones",
						arrayPat[0]==null?"a null patient":arrayPat[0].LName??"a null LName"));
				}
				if(arrayPat[0]==null || arrayPat[0].AddrNote!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The patient in the array should have the AddrNote {0} but returned {1}.",WebServiceTests.DirtyString,
						arrayPat[0]==null?"a null patient":arrayPat[0].AddrNote??"a null AddrNote"));
				}
				if(arrayPat[1]!=null) {
					strErrors.Add("The patient array should contain a null patient but returned a non-null patient.");
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetArrayPatient: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetArrayPatient: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with AddrNote containing dirty string of length "+WebServiceTests.DirtyString.Length;
			#endregion GetArrayPatient
			#region SendNullParam
			s.Restart();
			strResult=WebServiceTests.SendNullParam(null);
			s.Stop();
			if(strResult==null) {
				retVal.Add("SendNullParam: Passed.");
			}
			else {
				retVal.Add("SendNullParam: Failed.  The parameter sent and returned should be null but returned "+strResult+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendNullParam
			#region GetObjectNull
			s.Restart();
			Patient pat2=WebServiceTests.GetObjectNull();
			s.Stop();
			if(pat2==null) {
				retVal.Add("GetObjectNull: Passed.");
			}
			else {
				retVal.Add("GetObjectNull: Failed.  The patient returned should be null but returned a patient.");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetObjectNull
			#region SendColorParam
			s.Restart();
			Color colorResult=WebServiceTests.SendColorParam(Color.Green);
			s.Stop();
			if(colorResult!=null && colorResult.ToArgb()==Color.Green.ToArgb()) {
				retVal.Add("SendColorParam: Passed.");
			}
			else {
				retVal.Add(string.Format("SendColorParam: Failed.  The color parameter sent and returned should be {0} but returned {1}.",
					Color.Green.ToArgb().ToString(),colorResult==null?"null":colorResult.ToArgb().ToString()));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendColorParam
			#region SendProviderColor
			Provider prov=new Provider();
			prov.ProvColor=Color.Fuchsia;
			s.Restart();
			colorResult=WebServiceTests.SendProviderColor(prov);
			s.Stop();
			if(colorResult!=null && colorResult.ToArgb()==Color.Fuchsia.ToArgb()) {
				retVal.Add("SendProviderColor: Passed.");
			}
			else {
				retVal.Add(string.Format("SendProviderColor: Failed.  The ProvColor set on the provider parameter sent should be {0} but returned {1}.",
					Color.Fuchsia.ToArgb().ToString(),colorResult==null?"null":colorResult.ToArgb().ToString()));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendProviderColor
			#region SendSheetParameter
			SheetParameter sheetParam=new SheetParameter(false,"ParamName");
			s.Restart();
			strResult=WebServiceTests.SendSheetParameter(sheetParam);
			s.Stop();
			if(strResult=="ParamName") {
				retVal.Add("SendSheetParameter: Passed.");
			}
			else {
				retVal.Add("SendSheetParameter: Failed.  The sheet parameter name should be ParamName but returned "+(strResult??"null")+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendSheetParameter
			#region SendSheetWithFields
			Sheet sheet=new Sheet();
			sheet.SheetFields=new List<SheetField>();
			sheet.Parameters=new List<SheetParameter>();
			SheetField field=new SheetField();
			field.FieldName="FieldName";
			sheet.SheetFields.Add(field);
			s.Restart();
			strResult=WebServiceTests.SendSheetWithFields(sheet);
			s.Stop();
			if(strResult=="FieldName") {
				retVal.Add("SendSheetWithFields: Passed.");
			}
			else {
				retVal.Add("SendSheetWithFields: Failed.  The SheetField.FieldName should be FieldName but returned "+(strResult??"null")+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendSheetWithFields
			#region SendSheetDefWithFieldDefs
			SheetDef sheetdef=new SheetDef();
			sheetdef.SheetFieldDefs=new List<SheetFieldDef>();
			sheetdef.Parameters=new List<SheetParameter>();
			SheetFieldDef fielddef=new SheetFieldDef();
			fielddef.FieldName="FieldName";
			sheetdef.SheetFieldDefs.Add(fielddef);
			s.Restart();
			strResult=WebServiceTests.SendSheetDefWithFieldDefs(sheetdef);
			s.Stop();
			if(strResult=="FieldName") {
				retVal.Add("SendSheetDefWithFieldDefs: Passed.");
			}
			else {
				retVal.Add("SendSheetDefWithFieldDefs: Failed.  The SheetFieldDef.FieldName should be FieldName but returned "+(strResult??"null")+".");
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion SendSheetDefWithFieldDefs
			#region GetTimeSpan
			s.Restart();
			TimeSpan tspan=WebServiceTests.GetTimeSpan();
			s.Stop();
			if(tspan==new TimeSpan(1,0,0)) {
				retVal.Add("GetTimeSpan: Passed.");
			}
			else {
				retVal.Add(string.Format("GetTimeSpan: Failed.  The timespan returned should be {0} but returned {1}.",new TimeSpan(1,0,0).ToStringHmmss(),
					tspan==null?"null":tspan.ToStringHmmss()));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetTimeSpan
			#region GetStringContainingCR
			s.Restart();
			strResult=WebServiceTests.GetStringContainingCR();
			s.Stop();
			if(strResult==WebServiceTests.NewLineString) {
				retVal.Add("GetStringContainingCR: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetStringContainingCR: Failed.  The string should be {0} but returned {1}.",WebServiceTests.NewLineString,
					strResult??"null"));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetStringContainingCR
			#region GetListTasksContainingCR
			s.Restart();
			Task t=WebServiceTests.GetListTasksContainingCR()[0];
			s.Stop();
			if(t!=null && t.Descript==WebServiceTests.NewLineString) {
				retVal.Add("GetListTasksContainingCR: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetListTasksContainingCR: Failed.  The task description should be {0} but returned {1}.",
					WebServiceTests.NewLineString,t==null?"a null task":t.Descript??"a null description"));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion GetListTasksContainingCR
			#region GetListTasksSpecialChars
			//Tests special chars, new lines, Date, DateTime, and enum values in a list of objects as the parameter and the return value
			List<Task> listTasks=new List<Task> {new Task {
				Descript=WebServiceTests.DirtyString,
				ParentDesc=WebServiceTests.NewLineString,
				DateTask=WebServiceTests.DateTodayTest,
				DateTimeEntry=WebServiceTests.DateTEntryTest,
				TaskStatus=TaskStatusEnum.Done } };
			s.Restart();
			List<Task> listTasksReturned=WebServiceTests.GetListTasksSpecialChars(listTasks);
			s.Stop();
			strErrors=new List<string>();
			if(listTasksReturned==null || listTasksReturned.Count<1) {
				strErrors.Add(listTasksReturned==null?"The list of tasks is null.":"The list of tasks contains an insufficient number of tasks.");
			}
			int idx=0;
			foreach(Task task in listTasksReturned) {
				if(task==null) {
					strErrors.Add("The tasklist contains a null task.");
					idx++;
					continue;
				}
				if(idx==0 && task.Descript!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The task.Descript should be {0} but returned {1}.",WebServiceTests.DirtyString,task.Descript??"null"));
				}
				if(task.ParentDesc!=WebServiceTests.NewLineString) {
					strErrors.Add(string.Format(@"The task.ParentDesc should be {0} but returned {1}.",WebServiceTests.NewLineString,task.ParentDesc??"null"));
				}
				if(task.DateTask==null || task.DateTask.Date!=WebServiceTests.DateTodayTest.Date) {
					strErrors.Add(string.Format("The task.DateTask should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
						task.DateTask==null?"null":task.DateTask.ToShortDateString()));
				}
				if(task.DateTimeEntry!=WebServiceTests.DateTEntryTest) {
					strErrors.Add(string.Format("The task.DateTimeEntry should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
						task.DateTimeEntry==null?"null":task.DateTimeEntry.ToString()));
				}
				if(task.TaskStatus!=TaskStatusEnum.Done) {
					strErrors.Add(string.Format("The task.TaskStatus should be {0} but returned {1}.",TaskStatusEnum.Done,task.TaskStatus));
				}
				idx++;
			}
			if(strErrors.Count==0) {
				retVal.Add("GetListTasksSpecialChars: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetListTasksSpecialChars: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with task parameter and return value with descript containing dirty string of length "
				+WebServiceTests.DirtyString.Length;
			#endregion GetListTasksSpecialChars
			#region GetFamily
			s.Restart();
			Family result=WebServiceTests.GetFamily();
			s.Stop();
			strErrors=new List<string>();
			if(result==null || result.ListPats==null || result.ListPats.Length<2) {
				strErrors.Add("The Family"+result==null?" object is null.":result.ListPats==null?".ListPats is null.":
					".ListPats contains an insufficient number of patients.");
			}
			else {
				for(int i=0;i<result.ListPats.Length;i++) {
					Patient p=result.ListPats[i];
					if(p.FName!=(i==0?"John":"Jennifer")) {
						strErrors.Add(string.Format("The patient.FName should be {0} but returned {1}.",i==0?"John":"Jennifer",p.FName??"null"));
					}
					if(p.LName!=null) {
						strErrors.Add("The patient.LName should be null but returned "+p.LName+".");
					}
					if(i==0 && p.AddrNote!=WebServiceTests.DirtyString) {
						strErrors.Add(string.Format(@"The patient.AddrNote should be {0} but returned {1}.",WebServiceTests.DirtyString,pat.AddrNote??"null"));
					}
					if(p.ApptModNote!=WebServiceTests.NewLineString) {
						strErrors.Add(string.Format(@"The patient.ApptModNote should be {0} but returned {1}.",WebServiceTests.NewLineString,p.ApptModNote??"null"));
					}
					if(p.Email!="service@opendental.com") {
						strErrors.Add("The patient.Email should be service@opendental.com but returned "+(p.Email??"null")+".");
					}
					if(p.PatStatus!=PatientStatus.NonPatient) {
						strErrors.Add("The patient.PatStatus should be "+PatientStatus.NonPatient+" but returned "+p.PatStatus+".");
					}
					if(p.AdmitDate==null || p.AdmitDate.Date!=WebServiceTests.DateTodayTest.Date) {
						strErrors.Add(string.Format("The patient.AdmitDate should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
							p.AdmitDate==null?"null":p.AdmitDate.ToShortDateString()));
					}
					if(p.DateTStamp!=WebServiceTests.DateTEntryTest) {
						strErrors.Add(string.Format("The patient.DateTStamp should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
							p.DateTStamp==null?"null":p.DateTStamp.ToString()));
					}
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetFamily: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetFamily: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with AddrNote field containing dirty string of length "
				+WebServiceTests.DirtyString.Length;
			#endregion GetFamily
			#region GetListMedLabsSpecialChars
			s.Restart();
			List<MedLab> listMLabs=WebServiceTests.GetListMedLabsSpecialChars();
			s.Stop();
			strErrors=new List<string>();
			if(listMLabs==null || listMLabs.Count<1) {
				strErrors.Add("The list of MedLabs is "+listMLabs==null?"null.":"empty.");
			}
			else {
				MedLab mlab=listMLabs[0];
				if(mlab.MedLabNum!=1) {
					strErrors.Add("The MedLabNum should be 1 but returned "+mlab.MedLabNum+".");
				}
				if(mlab.NoteLab!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The MedLab.NoteLab should be {0} but returned {1}.",WebServiceTests.DirtyString,mlab.NoteLab??"null"));
				}
				if(mlab.NotePat!=WebServiceTests.NewLineString) {
					strErrors.Add(string.Format(@"The MedLab.NotePat should be {0} but returned {1}.",WebServiceTests.NewLineString,mlab.NotePat??"null"));
				}
				if(mlab.ResultStatus!=ResultStatus.P) {
					strErrors.Add("The MedLab.ResultStatus should be "+ResultStatus.P+" but returned "+mlab.ResultStatus+".");
				}
				if(mlab.DateTimeEntered==null || mlab.DateTimeEntered.Date!=WebServiceTests.DateTodayTest.Date) {
					strErrors.Add(string.Format("The MedLab.DateTimeEntered should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
						mlab.DateTimeEntered==null?"null":mlab.DateTimeEntered.ToShortDateString()));
				}
				if(mlab.DateTimeReported!=WebServiceTests.DateTEntryTest) {
					strErrors.Add(string.Format("The MedLab.DateTimeReported should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
						mlab.DateTimeReported==null?"null":mlab.DateTimeReported.ToString()));
				}
				if(mlab.ListMedLabResults==null || mlab.ListMedLabResults.Count<1) {
					strErrors.Add("The list of MedLabResults for the MedLab is "+mlab.ListMedLabResults==null?"null.":"empty.");
				}
				else {
					MedLabResult mlr=mlab.ListMedLabResults[0];
					if(mlr.MedLabResultNum!=2) {
						strErrors.Add("The MedLabResultNum should be 2 but returned "+mlr.MedLabResultNum+".");
					}
					if(mlr.MedLabNum!=1) {
						strErrors.Add("The MedLabResult.MedLabNum should be 1 but returned "+mlr.MedLabNum+".");
					}
					if(mlr.Note!=WebServiceTests.DirtyString) {
						strErrors.Add(string.Format(@"The MedLabResult.Note should be {0} but returned {1}.",WebServiceTests.DirtyString,mlr.Note??"null"));
					}
					if(mlr.ObsText!=WebServiceTests.NewLineString) {
						strErrors.Add(string.Format(@"The MedLabResult.ObsText should be {0} but returned {1}.",WebServiceTests.NewLineString,mlr.ObsText??"null"));
					}
					if(mlr.ObsSubType!=DataSubtype.PDF) {
						strErrors.Add("The MedLabResult.ObsSubType should be "+DataSubtype.PDF+" but returned "+mlr.ObsSubType+".");
					}
					if(mlr.DateTimeObs!=WebServiceTests.DateTEntryTest) {
						strErrors.Add(string.Format("The MedLabResult.DateTimeObs should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
							mlr.DateTimeObs==null?"null":mlr.DateTimeObs.ToString()));
					}
				}
			}
			if(strErrors.Count==0) {
				retVal.Add("GetListMedLabsSpecialChars: Passed.");
			}
			else {
				retVal.Add(string.Format(@"GetListMedLabsSpecialChars: Failed.  {0}",string.Join("  ",strErrors)));
			}
			retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds+" with lab note and result note both containing dirty strings each of length "
				+WebServiceTests.DirtyString.Length;
			#endregion GetListMedLabsSpecialChars
			#region EhrTriggerMatch
			//Vitalsign vs=new Vitalsign();
			//s.Restart();
			//List<CDSIntervention> listCds=WebServiceTests.TriggerMatch(vs);
			//s.Stop();
			//strErrors=new List<string>();
			//if(listCds==null || listCds.Count<1 || listCds[0]==null) {
			//	strErrors.Add("List<CDSIntervention> "+listCds==null?"is null.":listCds.Count<1?"is empty.":"contains a null CDSIntervention object.");
			//}
			//else {
			//	CDSIntervention cdsCur=listCds[0];
			//	if(cdsCur.InterventionMessage!=WebServiceTests.DirtyString) {
			//		strErrors.Add(string.Format(@"The CDSIntervention.InterventionMessage should be {0} but returned {1}.",WebServiceTests.DirtyString,
			//			cdsCur.InterventionMessage??"null"));
			//	}
			//	if(cdsCur.TriggerObjects==null || cdsCur.TriggerObjects.Count==0) {
			//		strErrors.Add("TriggerObjects "+cdsCur.TriggerObjects==null?"is null.":"is empty.");
			//	}
			//	else {
			//		foreach(object obj in cdsCur.TriggerObjects) {
			//			string retStr="";
			//			string labResultStr="";
			//			switch(obj.GetType().ToString()) {
			//				case "OpenDentBusiness.DiseaseDef":
			//					retStr=((DiseaseDef)obj).DiseaseName;
			//					break;
			//				case "OpenDentBusiness.ICD9":
			//					retStr=((ICD9)obj).Description;
			//					break;
			//				case "OpenDentBusiness.Icd10":
			//					retStr=((Icd10)obj).Description;
			//					break;
			//				case "OpenDentBusiness.Snomed":
			//					retStr=((Snomed)obj).Description;
			//					break;
			//				case "OpenDentBusiness.Medication":
			//					retStr=((Medication)obj).Notes;
			//					break;
			//				case "OpenDentBusiness.RxNorm":
			//					retStr=((RxNorm)obj).Description;
			//					break;
			//				case "OpenDentBusiness.Cvx":
			//					retStr=((Cvx)obj).Description;
			//					break;
			//				case "OpenDentBusiness.AllergyDef":
			//					retStr=((AllergyDef)obj).Description;
			//					break;
			//				case "OpenDentBusiness.EhrLabResult":
			//					EhrLabResult resultCur=(EhrLabResult)obj;
			//					retStr=resultCur.UnitsText;
			//					if(resultCur.ListEhrLabResultNotes!=null && resultCur.ListEhrLabResultNotes.Count>0) {
			//						labResultStr=resultCur.ListEhrLabResultNotes[0].Comments;
			//					}
			//					else {
			//						strErrors.Add("The EhrLabResult list of notes is "+resultCur.ListEhrLabResultNotes==null?"null.":"empty.");
			//					}
			//					break;
			//				case "OpenDentBusiness.Patient":
			//					retStr=((Patient)obj).AddrNote;
			//					break;
			//				case "OpenDentBusiness.Vitalsign":
			//					retStr=((Vitalsign)obj).WeightCode;
			//					break;
			//				case "OpenDentBusiness.MedicationPat":
			//					retStr=((MedicationPat)obj).MedDescript;
			//					break;
			//				default:
			//					break;
			//			}
			//			if(retStr!=WebServiceTests.DirtyString) {
			//				int idxFirstDiff=0;
			//				if(retStr.Length>0) {
			//					for(int i = 0;i<WebServiceTests.DirtyString.Length&&i<retStr.Length;i++) {
			//						if(WebServiceTests.DirtyString[i]!=retStr[i]) {
			//							idxFirstDiff=i;
			//							break;
			//						}
			//					}
			//				}
			//				strErrors.Add(string.Format(@"The {0} object string should be {1} but returned {2}.",obj.GetType().FullName,
			//					WebServiceTests.DirtyString.Substring(idxFirstDiff,Math.Min(25,WebServiceTests.DirtyString.Length-idxFirstDiff-1)),
			//					retStr==null?"null":retStr.Length>(idxFirstDiff+1)?retStr.Substring(idxFirstDiff,Math.Min(25,retStr.Length-idxFirstDiff-1)):""));
			//			}
			//		}
			//	}
			//}
			//if(strErrors.Count==0) {
			//	retVal.Add("EhrTriggerMatch: Passed.");
			//}
			//else {
			//	retVal.Add(string.Format(@"EhrTriggerMatch: Failed.  {0}",string.Join("  ",strErrors)));
			//}
			//retVal[retVal.Count-1]+="  Elapsed time: "+s.Elapsed.TotalSeconds;
			#endregion EhrTriggerMatch
			#region ObjectParamType
			Vitalsign vs=new Vitalsign { IsIneligible=true };
			s.Restart();
			vs=WebServiceTests.GetVitalsignFromObjectParam(vs);
			s.Stop();
			strErrors=new List<string>();
			if(vs.GetType()!=typeof(Vitalsign)) {
				strErrors.Add(string.Format("The object returned is not a {0} it is a {1}.",typeof(Vitalsign),vs.GetType()));
			}
			if(vs.IsIneligible) {
				strErrors.Add(string.Format("The vitalsign object IsIneligible flag should be {0} but returned {1}.","true",vs.IsIneligible.ToString()));
			}
			if(vs.Documentation!=WebServiceTests.DirtyString) {
				strErrors.Add("The vitalsign object returned did not have the correct dirty string.");
			}
			retVal.Add(string.Format(@"ObjectParamType: {0}.{1}",strErrors.Count==0?"Passed":"Failed",strErrors.Count==0?"":("  "+string.Join("  ",strErrors)))
				+"  Elapsed time: "+s.Elapsed.TotalSeconds+" with return parameter containing dirty string of length "+WebServiceTests.DirtyString.Length);
			#endregion ObjectParamType
			#region ObjectReturnType
			//vs=new Vitalsign { IsIneligible=true };
			//s.Restart();
			//object obj=WebServiceTests.GetObjectFromVitalsignParam(vs);
			//s.Stop();
			//strErrors=new List<string>();
			//if(obj.GetType()!=typeof(Vitalsign)) {
			//	strErrors.Add(string.Format("The object returned is not a {0} it is a {1}.",typeof(Vitalsign),obj.GetType()));
			//}
			//vs=(Vitalsign)obj;
			//if(vs.IsIneligible) {
			//	strErrors.Add(string.Format("The vitalsign object IsIneligible flag should be {0} but returned {1}.","true",vs.IsIneligible.ToString()));
			//}
			//if(vs.Documentation!=WebServiceTests.DirtyString) {
			//	strErrors.Add("The vitalsign object returned did not have the correct dirty string.");
			//}
			//retVal.Add(string.Format(@"ObjectReturnType: {0}.{1}",strErrors.Count==0?"Passed":"Failed",strErrors.Count==0?"":("  "+string.Join("  ",strErrors)))
			//	+"  Elapsed time: "+s.Elapsed.TotalSeconds+" with return parameter containing dirty string of length "+WebServiceTests.DirtyString.Length);
			#endregion ObjectReturnType
			#region QualityMeasures.GetAll2014
			//call QualityMeasures.GetAll2014(dateStart,dateEnd,ProvNum) which returns a List<QualityMeasure>
			//a QualityMeasure has a ton of complex data types as fields including dictionaries and lists of objects that have other lists of objects for fields
			//major test.  have to come up with a database with good data to return for testing
			#endregion QualityMeasures.GetAll2014
			#region GetProcCodeWithDirtyProperty
			Def d;
			if(DefC.GetList(DefCat.ProcCodeCats).Length==0) {
				d=new Def() { Category=DefCat.ProcCodeCats,ItemName=WebServiceTests.DirtyString };
				d.DefNum=Defs.Insert(d);
			}
			else {
				d=DefC.GetList(DefCat.ProcCodeCats)[0];
				d.ItemName=WebServiceTests.DirtyString;
				Defs.Update(d);
			}
			Defs.RefreshCache();
			DataValid.SetInvalid(InvalidType.Defs);
			d=DefC.GetDef(DefCat.ProcCodeCats,d.DefNum);
			ProcedureCode pc=new ProcedureCode { IsNew=true,ProcCat=d.DefNum };
			ProcedureCode pc2=new ProcedureCode { IsNew=true };
			s.Restart();
			List<ProcedureCode> listPcs=new List<ProcedureCode>();
			strErrors=new List<string>();
			try {
				listPcs=WebServiceTests.GetProcCodeWithDirtyProperty(pc,pc2);
				s.Stop();
			}
			catch(Exception ex) {
				s.Stop();
				strErrors.Add("Cannot serialize a property with a getter that does not retrieve the same value the setter is manipulating.");
				strErrors.Add(ex.Message);
				strErrors.Add(ex.StackTrace);
			}
			if(listPcs.Count>0 && (listPcs[0].IsNew || listPcs[1].IsNew)) {
				strErrors.Add(string.Format("One or more of the returned ProcedureCode objects IsNew flag should be {0} but returned {1}.","false","true"));
			}
			if(listPcs.Count>0 && (listPcs[0].ProcCat!=d.DefNum||listPcs[1].ProcCat!=d.DefNum)) {
				strErrors.Add("One or more of the ProcedureCode objects returned did not have the correct ProcCat.");
			}
			if(listPcs.Count>0 && (listPcs[0].ProcCatDescript!=d.ItemName || listPcs[1].ProcCatDescript!=d.ItemName)) {
				strErrors.Add("One or more of the ProcedureCode objects returned did not have the correct dirty string.");
			}
			retVal.Add(string.Format(@"GetProcCodeWithDirtyProperty: {0}.{1}",strErrors.Count==0?"Passed":"Failed",strErrors.Count==0?"":("  "+string.Join("  ",strErrors)))
				+"  Elapsed time: "+s.Elapsed.TotalSeconds+" with property sent and received containing dirty string of length "+WebServiceTests.DirtyString.Length);
			#endregion GetProcCodeWithDirtyProperty
			#region SimulatedProcUpdate
			pc=new ProcedureCode { Descript="periodic oral evaluation - established patient & stuff",ProcCatDescript="Exams & Xrays",DefaultNote=WebServiceTests.DirtyString };
			pc2=pc.Copy();
			s.Restart();
			WebServiceTests.SimulatedProcUpdate(pc);
			s.Stop();
			strErrors=new List<string>();
			if(pc.Descript!=pc2.Descript || pc.ProcCatDescript!=pc2.ProcCatDescript) {
				strErrors.Add(string.Format(@"The Descript before is ""{0}"" and after is ""{1}"".  The ProcCatDescript before is ""{2}"" and after is ""{3}"".",pc2.Descript,pc.Descript,pc2.ProcCatDescript,pc.ProcCatDescript));
			}
			if(pc.DefaultNote!=pc2.DefaultNote) {
				strErrors.Add("The dirty string was altered from the simulated update call.");
			}
			retVal.Add(string.Format(@"SimulatedProcUpdate: {0}.{1}",strErrors.Count==0?"Passed":"Failed",strErrors.Count==0?"":("  "+string.Join("  ",strErrors)))
				+"  Elapsed time: "+s.Elapsed.TotalSeconds+" with DefaultNote sent and received containing dirty string of length "+WebServiceTests.DirtyString.Length);
			#endregion SimulatedProcUpdate
			return retVal;
		}
	}
}
