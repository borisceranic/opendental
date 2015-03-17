using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnGroupAttaches{
		#region CachePattern
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
			string command="SELECT * FROM conngroupattach";
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

		///<summary></summary>
		public static List<ConnGroupAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM conngroupattach";
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		 ///<summary>Inserts, updates, or deletes database rows to match supplied list.  Must always pass in ConnectionGroupNum.</summary>
     public static void Sync(List<ConnGroupAttach> listNew,long connectionGroupNum) {
				if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
							Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,connectionGroupNum);//never pass DB list through the web service
							return;
				}
				List<ConnGroupAttach> listDB=ConnGroupAttaches.GetForGroup(connectionGroupNum);
				Crud.ConnGroupAttachCrud.Sync(listNew,listDB);
     }


		///<summary>Gets one ConnGroupAttach from the db.</summary>
		public static ConnGroupAttach GetOne(long connGroupAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnGroupAttach>(MethodBase.GetCurrentMethod(),connGroupAttachNum);
			}
			return Crud.ConnGroupAttachCrud.SelectOne(connGroupAttachNum);
		}

		///<summary>Gets all ConnGroupAttaches for a given ConnectionGroupNum.</summary>
		public static List<ConnGroupAttach> GetForGroup(long connectionGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),connectionGroupNum);
			}
			string command="SELECT * FROM conngroupattach WHERE ConnectionGroupNum="+POut.Long(connectionGroupNum);
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets all ConnGroupAttaches for a given CentralConnectionNum.</summary>
		public static List<ConnGroupAttach> GetForConnection(long connectionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),connectionNum);
			}
			string command="SELECT * FROM conngroupattach WHERE CentralConnectionNum="+POut.Long(connectionNum);
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets count of ConnGroupAttaches for a ConnectionGroup.</summary>
		public static int GetCountByGroup(long groupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<int>(MethodBase.GetCurrentMethod(),groupNum);
			}
			string command="SELECT COUNT(*) FROM conngroupattach WHERE ConnectionGroupNum="+POut.Long(groupNum);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Gets count of ConnGroupAttaches for a CentralConnection.</summary>
		public static int GetCountByConnection(long connectionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<int>(MethodBase.GetCurrentMethod(),connectionNum);
			}
			string command="SELECT COUNT(*) FROM conngroupattach WHERE CentralConnectionNum="+POut.Long(connectionNum);
			return PIn.Int(Db.GetCount(command));
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

	}
}