using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnGroupAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all ConnGroupAttaches.</summary>
		private static List<ConnGroupAttach> listt;

		///<summary>A list of all ConnGroupAttaches.</summary>
		public static List<ConnGroupAttach> Listt{
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
			string command="SELECT * FROM conngroupattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ConnGroupAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.ConnGroupAttachCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ConnGroupAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM conngroupattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets one ConnGroupAttach from the db.</summary>
		public static ConnGroupAttach GetOne(long connGroupAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnGroupAttach>(MethodBase.GetCurrentMethod(),connGroupAttachNum);
			}
			return Crud.ConnGroupAttachCrud.SelectOne(connGroupAttachNum);
		}

		///<summary></summary>
		public static long Insert(ConnGroupAttach connGroupAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				connGroupAttach.ConnGroupAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),connGroupAttach);
				return connGroupAttach.ConnGroupAttachNum;
			}
			return Crud.ConnGroupAttachCrud.Insert(connGroupAttach);
		}

		///<summary></summary>
		public static void Update(ConnGroupAttach connGroupAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connGroupAttach);
				return;
			}
			Crud.ConnGroupAttachCrud.Update(connGroupAttach);
		}

		///<summary></summary>
		public static void Delete(long connGroupAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connGroupAttachNum);
				return;
			}
			string command= "DELETE FROM conngroupattach WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttachNum);
			Db.NonQ(command);
		}
		*/



	}
}