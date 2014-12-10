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

		///<summary>Inerts, updates, or deletes fields in DB to match the list of supplied sheet fields.</summary>
		public static void InsUpDel(List<SheetFieldDef> listNew,long sheetFieldDefNum) {
			List<SheetFieldDef> listDB=GetForSheetDef(sheetFieldDefNum);
			List<SheetFieldDef> listIns=new List<SheetFieldDef>();
			List<SheetFieldDef> listUpd=new List<SheetFieldDef>();
			List<SheetFieldDef> listDel=new List<SheetFieldDef>();
			listNew.Sort(SortInsUpDel);
			listDB.Sort(SortInsUpDel);
			int idxNew=0;
			int idxDB=0;
			SheetFieldDef fieldNew;//=new SheetFieldDef();
			SheetFieldDef fieldDB;//=new SheetFieldDef();
			if(idxNew<listNew.Count) {
				fieldNew=listNew[idxNew];
			}
			else {
				fieldNew=null;
			}
			if(idxDB<listDB.Count) {
				fieldDB=listDB[idxDB];
			}
			else {
				fieldDB=null;
			}
			while(fieldNew!=null || fieldDB!=null) {
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				else {
					fieldNew=null;
				}
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				else {
					fieldDB=null;
				}
				switch(GetAction(fieldNew,fieldDB)) {
					case InsUpDelNone.Insert:
						listIns.Add(fieldNew);
						idxNew++;
						break;
					case InsUpDelNone.Update:
						listUpd.Add(fieldNew);
						idxNew++;
						idxDB++;
						break;
					case InsUpDelNone.Delete:
						listDel.Add(fieldDB);
						idxDB++;
						break;
					case InsUpDelNone.None:
						idxNew++;
						idxDB++;
						break;
				}
			}
			//Insert
			for(int i=0;i<listIns.Count;i++) {
				SheetFieldDefs.Insert(listIns[i]);
			}
			//Update
			for(int i=0;i<listUpd.Count;i++) {
				SheetFieldDefs.Update(listUpd[i]);
			}
			//Delete
			for(int i=0;i<listDel.Count;i++) {
				SheetFieldDefs.Delete(listDel[i].SheetFieldDefNum);
			}
		}

		///<summary>Accepts nulls. Insert always applies to NewItem, Delete always applies to DBItem. Update and None are self explanatory.</summary>
		public static InsUpDelNone GetAction(SheetFieldDef NewItem,SheetFieldDef DBItem) {
			if(NewItem==null && DBItem==null) {
				return InsUpDelNone.None;
			}
			if(NewItem!=null && DBItem==null) {
				return InsUpDelNone.Insert;
			}
			if(NewItem==null && DBItem!=null) {
				return InsUpDelNone.Delete;
			}
			if(SortInsUpDel(NewItem,DBItem)<0){
				return InsUpDelNone.Insert;
			}
			else if(SortInsUpDel(NewItem,DBItem)>0){
				return InsUpDelNone.Delete;
			}
			else if(NewItem.FieldType       != DBItem.FieldType           
				|| NewItem.FieldName         != DBItem.FieldName           
				|| NewItem.FieldValue        != DBItem.FieldValue          
				|| NewItem.FontSize          != DBItem.FontSize            
				|| NewItem.FontName          != DBItem.FontName            
				|| NewItem.FontIsBold        != DBItem.FontIsBold          
				|| NewItem.XPos              != DBItem.XPos                
				|| NewItem.YPos              != DBItem.YPos                
				|| NewItem.Width             != DBItem.Width               
				|| NewItem.Height            != DBItem.Height              
				|| NewItem.GrowthBehavior    != DBItem.GrowthBehavior      
				|| NewItem.RadioButtonValue  != DBItem.RadioButtonValue    
				|| NewItem.RadioButtonGroup  != DBItem.RadioButtonGroup    
				|| NewItem.IsRequired        != DBItem.IsRequired          
				|| NewItem.TabOrder          != DBItem.TabOrder            
				|| NewItem.ReportableName    != DBItem.ReportableName      
				|| NewItem.FKey              != DBItem.FKey                
				|| NewItem.TextAlign         != DBItem.TextAlign           
				|| NewItem.IsPaymentOption   != DBItem.IsPaymentOption     
				|| NewItem.ItemColor         != DBItem.ItemColor
				|| NewItem.FieldType        == SheetFieldType.Grid) 
			{
				return InsUpDelNone.Update;
			}
			return InsUpDelNone.None;
		}

		public static int SortInsUpDel(SheetFieldDef NewItem,SheetFieldDef DBItem) {
			if(NewItem.SheetFieldDefNum != DBItem.SheetFieldDefNum && NewItem.SheetFieldDefNum!=0)	return NewItem.SheetFieldDefNum.CompareTo(DBItem.SheetFieldDefNum);
			if(NewItem.YPos             != DBItem.YPos) return NewItem.YPos.CompareTo(DBItem.YPos);
			if(NewItem.XPos             != DBItem.XPos) return NewItem.XPos.CompareTo(DBItem.XPos);
			if(NewItem.Width            != DBItem.Width) return NewItem.Width.CompareTo(DBItem.Width);
			if(NewItem.Height           != DBItem.Height) return NewItem.Height.CompareTo(DBItem.Height); 
			return 0;
		}



	}

	public enum InsUpDelNone {
		Insert,
		Update,
		Delete,
		None
	}
}