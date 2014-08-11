using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProcButtonQuicks{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all ProcButtonQuicks.</summary>
		private static List<ProcButtonQuick> listt;

		///<summary>A list of all ProcButtonQuicks.</summary>
		public static List<ProcButtonQuick> Listt{
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
			string command="SELECT * FROM procbuttonquick ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ProcButtonQuick";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.ProcButtonQuickCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static List<ProcButtonQuick> GetAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcButtonQuick>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM procbuttonquick";
			return Crud.ProcButtonQuickCrud.SelectMany(command);
		}

		///<summary>Sort by Y values first, then sort by X values.</summary>
		public static int sortYX(ProcButtonQuick p1,ProcButtonQuick p2) {
			//#error Move this to the S class once it is generated.
			if(p1.YPos!=p2.YPos) {
				return p1.YPos.CompareTo(p2.YPos);
			}
			return p1.ItemOrder.CompareTo(p2.ItemOrder);
		}

		///<summary></summary>
		public static long Insert(ProcButtonQuick procButtonQuick){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				procButtonQuick.ProcButtonQuickNum=Meth.GetLong(MethodBase.GetCurrentMethod(),procButtonQuick);
				return procButtonQuick.ProcButtonQuickNum;
			}
			return Crud.ProcButtonQuickCrud.Insert(procButtonQuick);
		}

		///<summary></summary>
		public static void Update(ProcButtonQuick procButtonQuick){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procButtonQuick);
				return;
			}
			Crud.ProcButtonQuickCrud.Update(procButtonQuick);
		}

		///<summary>Ensures that Quick Buttons category exists in DB, and validates all Quick buttons in the DB. 
		///Returns false if there is something wrong with ProcButtonQuick table. (Similar to DB maint.)</summary>
		public static bool ValidateAll() {


			return true;
		}

		///<summary></summary>
		public static void Delete(long procButtonQuickNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procButtonQuickNum);
				return;
			}
			string command= "DELETE FROM procbuttonquick WHERE ProcButtonQuickNum = "+POut.Long(procButtonQuickNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ProcButtonQuick> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcButtonQuick>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM procbuttonquick WHERE PatNum = "+POut.Long(patNum);
			return Crud.ProcButtonQuickCrud.SelectMany(command);
		}

		///<summary>Gets one ProcButtonQuick from the db.</summary>
		public static ProcButtonQuick GetOne(long procButtonQuickNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ProcButtonQuick>(MethodBase.GetCurrentMethod(),procButtonQuickNum);
			}
			return Crud.ProcButtonQuickCrud.SelectOne(procButtonQuickNum);
		}
		*/




	}
}