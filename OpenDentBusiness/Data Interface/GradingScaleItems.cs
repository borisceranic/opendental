using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class GradingScaleItems{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all GradingScaleItems.</summary>
		private static List<GradingScaleItem> listt;

		///<summary>A list of all GradingScaleItems.</summary>
		public static List<GradingScaleItem> Listt{
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
			string command="SELECT * FROM gradingscaleitem ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="GradingScaleItem";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.GradingScaleItemCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static List<GradingScaleItem> Refresh(long gradingScaleNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<GradingScaleItem>>(MethodBase.GetCurrentMethod(),gradingScaleNum);
			}
			string command="SELECT * FROM gradingscaleitem WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			return Crud.GradingScaleItemCrud.SelectMany(command);
		}

		///<summary>Gets one GradingScaleItem from the db.</summary>
		public static GradingScaleItem GetOne(long gradingScaleItemNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<GradingScaleItem>(MethodBase.GetCurrentMethod(),gradingScaleItemNum);
			}
			return Crud.GradingScaleItemCrud.SelectOne(gradingScaleItemNum);
		}

		///<summary></summary>
		public static long Insert(GradingScaleItem gradingScaleItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				gradingScaleItem.GradingScaleItemNum=Meth.GetLong(MethodBase.GetCurrentMethod(),gradingScaleItem);
				return gradingScaleItem.GradingScaleItemNum;
			}
			return Crud.GradingScaleItemCrud.Insert(gradingScaleItem);
		}

		///<summary></summary>
		public static void Update(GradingScaleItem gradingScaleItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gradingScaleItem);
				return;
			}
			Crud.GradingScaleItemCrud.Update(gradingScaleItem);
		}

		///<summary></summary>
		public static void DeleteAllFromGradingScale(long gradingScaleNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gradingScaleNum);
				return;
			}
			string command= "DELETE FROM gradingscaleitem WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(long gradingScaleItemNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gradingScaleItemNum);
				return;
			}
			string command= "DELETE FROM gradingscaleitem WHERE GradingScaleItemNum = "+POut.Long(gradingScaleItemNum);
			Db.NonQ(command);
		}



	}
}