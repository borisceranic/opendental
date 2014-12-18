using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SheetFieldDefs{
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string c="SELECT * FROM sheetfielddef ORDER BY SheetDefNum";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),c);
			table.TableName="sheetfielddef";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			SheetFieldDefC.Listt=Crud.SheetFieldDefCrud.TableToList(table);
		}

		///<summary>Gets all internal SheetFieldDefs from the database for a specific sheet, used in FormSheetFieldExam.</summary>
		public static List<SheetFieldDef> GetForExamSheet(long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetFieldDef>>(MethodBase.GetCurrentMethod(),sheetDefNum);
			}
			string command="SELECT * FROM sheetfielddef WHERE SheetDefNum="+POut.Long(sheetDefNum)+" "
				+"AND ((FieldName!='misc' AND FieldName!='') OR (ReportableName!='')) "
				+"GROUP BY FieldName,ReportableName";
			return Crud.SheetFieldDefCrud.SelectMany(command);
		}

		///<summary>Gets all SheetFieldDefs from the database for a specific sheet, used in FormSheetFieldExam.</summary>
		public static List<SheetFieldDef> GetForSheetDef(long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetFieldDef>>(MethodBase.GetCurrentMethod(),sheetDefNum);
			}
			string command="SELECT * FROM sheetfielddef WHERE SheetDefNum="+POut.Long(sheetDefNum);
			return Crud.SheetFieldDefCrud.SelectMany(command);
		}

		///<Summary>Gets one SheetFieldDef from the database.</Summary>
		public static SheetFieldDef CreateObject(long sheetFieldDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SheetFieldDef>(MethodBase.GetCurrentMethod(),sheetFieldDefNum);
			}
			return Crud.SheetFieldDefCrud.SelectOne(sheetFieldDefNum);
		}

		///<summary></summary>
		public static long Insert(SheetFieldDef sheetFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheetFieldDef.SheetFieldDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheetFieldDef);
				return sheetFieldDef.SheetFieldDefNum;
			}
			return Crud.SheetFieldDefCrud.Insert(sheetFieldDef);
		}

		///<summary></summary>
		public static void Update(SheetFieldDef sheetFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetFieldDef);
				return;
			}
			Crud.SheetFieldDefCrud.Update(sheetFieldDef);
		}

		///<summary></summary>
		public static void Delete(long sheetFieldDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetFieldDefNum);
				return;
			}
			Crud.SheetFieldDefCrud.Delete(sheetFieldDefNum);
		}

		public static void Synch(List<SheetFieldDef> listNew,long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,sheetDefNum);//never pass DB list through the web service
				return;
			}
			Synch(listNew,sheetDefNum,null);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list. Should always pass in sheetDefNum, listDB should never be null.</summary>
		public static void Synch(List<SheetFieldDef> listNew,long sheetDefNum, List<SheetFieldDef> listDB=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,sheetDefNum);//never pass DB list through the web service
				return;
			}
			if(listDB==null) {
				//fill list with Num
			}
			/*Crud.*/Synch(listNew,listDB);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static void Synch(List<SheetFieldDef> listNew,List<SheetFieldDef> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<SheetFieldDef> listIns=    new List<SheetFieldDef>();
			List<SheetFieldDef> listUpdNew =new List<SheetFieldDef>();
			List<SheetFieldDef> listUpdDB  =new List<SheetFieldDef>();
			List<SheetFieldDef> listDel    =new List<SheetFieldDef>();
			listNew.Sort((SheetFieldDef x,SheetFieldDef y) => { return x.SheetFieldDefNum.CompareTo(y.SheetFieldDefNum); });//Anonymous function, just sorts by compairing PK.
			listDB.Sort((SheetFieldDef x,SheetFieldDef y) => { return x.SheetFieldDefNum.CompareTo(y.SheetFieldDefNum); });//Anonymous function, just sorts by compairing PK.
			int idxNew=0;
			int idxDB=0;
			SheetFieldDef fieldNew;
			SheetFieldDef fieldDB;
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.SheetFieldDefNum<fieldDB.SheetFieldDefNum) {//newPK less than dbPK
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.SheetFieldDefNum>fieldDB.SheetFieldDefNum) {//dbPK less than newPK
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Crud.SheetFieldDefCrud.Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				Crud.SheetFieldDefCrud.Update(listUpdNew[i],listUpdDB[i]);
			}
			for(int i=0;i<listDel.Count;i++) {
				Crud.SheetFieldDefCrud.Delete(listDel[i].SheetFieldDefNum);
			}
		}

		///<summary>Sorts fields in the order that they shoudl be drawn on top of eachother. First Images, then Drawings, Lines, Rectangles, Text, Check Boxes, and SigBoxes. In that order.</summary>
		public static int SortDrawingOrderLayers(SheetFieldDef f1,SheetFieldDef f2) {
			if(f1.FieldType!=f2.FieldType) {
				return SheetFields.FieldTypeSortOrder(f1.FieldType).CompareTo(SheetFields.FieldTypeSortOrder(f2.FieldType));
			}
			return f1.YPos.CompareTo(f2.YPos);
			//return f1.SheetFieldNum.CompareTo(f2.SheetFieldNum);
		}
		
	}
}