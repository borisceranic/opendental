using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EServiceSignals{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all EServiceSignals.</summary>
		private static List<EServiceSignal> listt;

		///<summary>A list of all EServiceSignals.</summary>
		public static List<EServiceSignal> Listt{
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
			string command="SELECT * FROM eservicesignal ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EServiceSignal";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EServiceSignalCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EServiceSignal> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EServiceSignal>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM eservicesignal WHERE PatNum = "+POut.Long(patNum);
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Gets one EServiceSignal from the db.</summary>
		public static EServiceSignal GetOne(long eServiceSignalNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EServiceSignal>(MethodBase.GetCurrentMethod(),eServiceSignalNum);
			}
			return Crud.EServiceSignalCrud.SelectOne(eServiceSignalNum);
		}

		///<summary></summary>
		public static long Insert(EServiceSignal eServiceSignal){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				eServiceSignal.EServiceSignalNum=Meth.GetLong(MethodBase.GetCurrentMethod(),eServiceSignal);
				return eServiceSignal.EServiceSignalNum;
			}
			return Crud.EServiceSignalCrud.Insert(eServiceSignal);
		}

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignal);
				return;
			}
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary></summary>
		public static void Delete(long eServiceSignalNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignalNum);
				return;
			}
			string command= "DELETE FROM eservicesignal WHERE EServiceSignalNum = "+POut.Long(eServiceSignalNum);
			Db.NonQ(command);
		}
		*/



	}
}