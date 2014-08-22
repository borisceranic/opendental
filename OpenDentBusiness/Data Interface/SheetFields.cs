﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SheetFields{

		///<Summary>Gets one SheetField from the database.</Summary>
		public static SheetField CreateObject(long sheetFieldNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SheetField>(MethodBase.GetCurrentMethod(),sheetFieldNum);
			}
			return Crud.SheetFieldCrud.SelectOne(sheetFieldNum);
		}

		public static List<SheetField> GetListForSheet(long sheetNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetField>>(MethodBase.GetCurrentMethod(),sheetNum);
			}
			string command="SELECT * FROM sheetfield WHERE SheetNum="+POut.Long(sheetNum)
				+" ORDER BY SheetFieldNum";//the ordering is CRITICAL because the signature key is based on order.
			return Crud.SheetFieldCrud.SelectMany(command);
		}

		///<summary>When we need to use a sheet, we must run this method to pull all the associated fields and parameters from the database.  Then it will be ready for printing, copying, etc.</summary>
		public static void GetFieldsAndParameters(Sheet sheet){
			//No need to check RemotingRole; no call to db.
			sheet.SheetFields=GetListForSheet(sheet.SheetNum);
			//so parameters will also be in the field list, but they will just be ignored from here on out.
			//because we will have an explicit parameter list instead.
			sheet.Parameters=new List<SheetParameter>();
			SheetParameter param;
			//int paramVal;
			for(int i=0;i<sheet.SheetFields.Count;i++){
				if(sheet.SheetFields[i].FieldType==SheetFieldType.Parameter){
					param=new SheetParameter(true,sheet.SheetFields[i].FieldName,sheet.SheetFields[i].FieldValue);
					sheet.Parameters.Add(param);
				}
			}
		}

		///<summary>Used in SheetFiller to fill patient letter with exam sheet information.  Will return null if no exam sheet matching the description exists for the patient.  Usually just returns one field, but will return a list of fields if it's for a RadioButtonGroup.</summary>
		public static List<SheetField> GetFieldFromExamSheet(long patNum,string examDescript,string fieldName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetField>>(MethodBase.GetCurrentMethod(),patNum,examDescript,fieldName);
			}
			Sheet sheet=Sheets.GetMostRecentExamSheet(patNum,examDescript);
			if(sheet==null) {
				return null;
			}
			string command="SELECT * FROM sheetfield WHERE SheetNum="
				+POut.Long(sheet.SheetNum)+" "
				+"AND (RadioButtonGroup='"+POut.String(fieldName)+"' OR ReportableName='"+POut.String(fieldName)+"' OR FieldName='"+POut.String(fieldName)+"')";
			return Crud.SheetFieldCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(SheetField sheetField) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheetField.SheetFieldNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheetField);
				return sheetField.SheetFieldNum;
			}
			return Crud.SheetFieldCrud.Insert(sheetField);
		}

		///<summary></summary>
		public static void Update(SheetField sheetField) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetField);
				return;
			}
			Crud.SheetFieldCrud.Update(sheetField);
		}

		///<summary></summary>
		public static void DeleteObject(long sheetFieldNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetFieldNum);
				return;
			}
			Crud.SheetFieldCrud.Delete(sheetFieldNum);
		}

		///<summary>Deletes all existing drawing fields for a sheet from the database and then adds back the list supplied.</summary>
		public static void SetDrawings(List<SheetField> drawingList,long sheetNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),drawingList,sheetNum);
				return;
			}
			string command="DELETE FROM sheetfield WHERE SheetNum="+POut.Long(sheetNum)
				+" AND FieldType="+POut.Long((int)SheetFieldType.Drawing);
			Db.NonQ(command);
			foreach(SheetField field in drawingList){
				Insert(field);
			}
		}

		public static int SortDrawingOrder(SheetField f1,SheetField f2) {
			if(f1.Bounds.Top<f2.Bounds.Top
				&& f1.Bounds.Bottom>f2.Bounds.Bottom) {//f1 starts before and ends after f2 meaning it should be ordered before f2
					return -1;
			}
			else if(f2.Bounds.Top<f1.Bounds.Top
				&& f2.Bounds.Bottom>f1.Bounds.Bottom) {
					return 1;
			}
			if(f1.Bounds.Bottom!=f2.Bounds.Bottom) {
				return f1.Bounds.Bottom.CompareTo(f2.Bounds.Bottom);
			}
			return f1.XPos.CompareTo(f2.XPos);
		}

		///<summary>Sorts the sheet fields by SheetFieldNum.  This is used when creating a signature key and is absolutely critical that it not change.</summary>
		public static int SortPrimaryKey(SheetField f1,SheetField f2) {
			return f1.SheetFieldNum.CompareTo(f2.SheetFieldNum);
		}



	}
}