using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SheetGridColDefs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SheetGridColDefs.</summary>
		private static List<SheetGridColDef> listt;

		///<summary>A list of all SheetGridColDefs.</summary>
		public static List<SheetGridColDef> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM sheetgridcoldef ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SheetGridColDef";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SheetGridColDefCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SheetGridColDef> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetGridColDef>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM sheetgridcoldef WHERE PatNum = "+POut.Long(patNum);
			return Crud.SheetGridColDefCrud.SelectMany(command);
		}

		///<summary>Gets one SheetGridColDef from the db.</summary>
		public static SheetGridColDef GetOne(long sheetGridColDefNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SheetGridColDef>(MethodBase.GetCurrentMethod(),sheetGridColDefNum);
			}
			return Crud.SheetGridColDefCrud.SelectOne(sheetGridColDefNum);
		}

		///<summary></summary>
		public static void Delete(long sheetGridColDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetGridColDefNum);
				return;
			}
			string command= "DELETE FROM sheetgridcoldef WHERE SheetGridColDefNum = "+POut.Long(sheetGridColDefNum);
			Db.NonQ(command);
		}
		*/

		///<summary></summary>
		public static long Insert(SheetGridColDef sheetGridColDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheetGridColDef.SheetGridColDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheetGridColDef);
				return sheetGridColDef.SheetGridColDefNum;
			}
			return Crud.SheetGridColDefCrud.Insert(sheetGridColDef);
		}

		///<summary></summary>
		public static void Update(SheetGridColDef sheetGridColDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetGridColDef);
				return;
			}
			Crud.SheetGridColDefCrud.Update(sheetGridColDef);
		}

		///<summary></summary>
		public static void Delete(long sheetGridColDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetGridColDefNum);
				return;
			}
			Crud.SheetGridColDefCrud.Delete(sheetGridColDefNum);
		}

		///<summary></summary>
		public static List<SheetGridColDef> GetForGridDef(long gridDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetGridColDef>>(MethodBase.GetCurrentMethod(),gridDefNum);
			}
			string command="SELECT * FROM sheetgridcoldef WHERE SheetGridDefNum = "+POut.Long(gridDefNum)+" ORDER BY ItemOrder";
			return Crud.SheetGridColDefCrud.SelectMany(command);
		}

		public static void DeleteForGridDef(long gridDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gridDefNum);
				return;
			}
			string command= "DELETE FROM sheetgridcoldef WHERE SheetGridDefNum = "+POut.Long(gridDefNum);
			Db.NonQ(command);
		}

		///<summary>Sorts by Item order, PK, then hash.</summary>
		public static int SortItemOrder(SheetGridColDef x,SheetGridColDef y) {
			if(x.ItemOrder!=y.ItemOrder) {
				return x.ItemOrder.CompareTo(y.ItemOrder);
			}
			if(x.SheetGridColDefNum!=y.SheetGridColDefNum) {
				return x.SheetGridColDefNum.CompareTo(y.SheetGridColDefNum);
			}
			return x.GetHashCode().CompareTo(y.GetHashCode());
		}

	}
}