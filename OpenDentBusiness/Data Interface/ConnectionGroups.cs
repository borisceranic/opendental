using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnectionGroups{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all ConnectionGroups.</summary>
		private static List<ConnectionGroup> listt;

		///<summary>A list of all ConnectionGroups.</summary>
		public static List<ConnectionGroup> Listt{
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
			string command="SELECT * FROM connectiongroup ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ConnectionGroup";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.ConnectionGroupCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ConnectionGroup> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnectionGroup>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM connectiongroup WHERE PatNum = "+POut.Long(patNum);
			return Crud.ConnectionGroupCrud.SelectMany(command);
		}

		///<summary>Gets one ConnectionGroup from the db.</summary>
		public static ConnectionGroup GetOne(long connectionGroupNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnectionGroup>(MethodBase.GetCurrentMethod(),connectionGroupNum);
			}
			return Crud.ConnectionGroupCrud.SelectOne(connectionGroupNum);
		}

		///<summary></summary>
		public static long Insert(ConnectionGroup connectionGroup){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				connectionGroup.ConnectionGroupNum=Meth.GetLong(MethodBase.GetCurrentMethod(),connectionGroup);
				return connectionGroup.ConnectionGroupNum;
			}
			return Crud.ConnectionGroupCrud.Insert(connectionGroup);
		}

		///<summary></summary>
		public static void Update(ConnectionGroup connectionGroup){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connectionGroup);
				return;
			}
			Crud.ConnectionGroupCrud.Update(connectionGroup);
		}

		///<summary></summary>
		public static void Delete(long connectionGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connectionGroupNum);
				return;
			}
			string command= "DELETE FROM connectiongroup WHERE ConnectionGroupNum = "+POut.Long(connectionGroupNum);
			Db.NonQ(command);
		}
		*/



	}
}