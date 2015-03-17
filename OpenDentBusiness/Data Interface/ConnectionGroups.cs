using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnectionGroups{
		#region CachePattern

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

		public static List<ConnectionGroup> GetListt() {
			List<ConnectionGroup> listConns=new List<ConnectionGroup>();
			for(int i=0;i<Listt.Count;i++) {
				listConns.Add(Listt[i].Copy());
			}
			return listConns;
		}

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM connectiongroup ORDER BY Description";
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

		///<summary></summary>
		public static List<ConnectionGroup> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnectionGroup>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM connectiongroup ORDER BY Description";
			return Crud.ConnectionGroupCrud.SelectMany(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
    public static void Sync(List<ConnectionGroup> listNew) {
      if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
        Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew);
        return;
      }
			ConnectionGroups.RefreshCache();
      List<ConnectionGroup> listDB=ConnectionGroups.Listt;
      Crud.ConnectionGroupCrud.Sync(listNew,listDB);
    }

		///<summary>Filters _listConns to only include connections that are associated to the selected connection group.</summary>
		public static List<CentralConnection> FilterConnsByGroup(List<CentralConnection> listConns,ConnectionGroup connGroup) {
			List<CentralConnection> retVal=new List<CentralConnection>();
			//Get all ConnGroupAttaches for selected group.
			List<ConnGroupAttach> listCentralConnGroupAttaches=ConnGroupAttaches.GetForGroup(connGroup.ConnectionGroupNum);
			for(int i=0;i<listConns.Count;i++) {//Go through connections and return a subset of only those in listConnAttaches (only those in the selected group).
				for(int j=0;j<listCentralConnGroupAttaches.Count;j++) {
					if(listConns[i].CentralConnectionNum==listCentralConnGroupAttaches[j].CentralConnectionNum) {//Connection entry found for selected group, display connection.
						retVal.Add(listConns[i]);
						break;
					}
				}
			}
			return retVal;
		}

		///<summary>Gets one ConnectionGroup from the db based on the ConnectionGroupNum.</summary>
		public static ConnectionGroup GetOne(long connectionGroupNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnectionGroup>(MethodBase.GetCurrentMethod(),connectionGroupNum);
			}
			return Crud.ConnectionGroupCrud.SelectOne(connectionGroupNum);
		}

		///<summary>Gets ConnectionGroups based on description.</summary>
		public static List<ConnectionGroup> GetByDescription(string description) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnectionGroup>>(MethodBase.GetCurrentMethod(),description);
			}
			string command="SELECT * FROM connectiongroup WHERE Description LIKE '%"+POut.String(description)+"%'";
			return Crud.ConnectionGroupCrud.SelectMany(command);
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
	}
}