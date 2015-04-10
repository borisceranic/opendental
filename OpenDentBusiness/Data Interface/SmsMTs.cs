using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsMTs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsMTs.</summary>
		private static List<SmsMT> listt;

		///<summary>A list of all SmsMTs.</summary>
		public static List<SmsMT> Listt{
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
			string command="SELECT * FROM smsmt ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsMT";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsMTCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsMT> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsMT>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsmt WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsMTCrud.SelectMany(command);
		}

		///<summary>Gets one SmsMT from the db.</summary>
		public static SmsMT GetOne(long smsMTNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsMT>(MethodBase.GetCurrentMethod(),smsMTNum);
			}
			return Crud.SmsMTCrud.SelectOne(smsMTNum);
		}

		///<summary></summary>
		public static long Insert(SmsMT smsMT){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsMT.SmsMTNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsMT);
				return smsMT.SmsMTNum;
			}
			return Crud.SmsMTCrud.Insert(smsMT);
		}

		///<summary></summary>
		public static void Update(SmsMT smsMT){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsMT);
				return;
			}
			Crud.SmsMTCrud.Update(smsMT);
		}

		///<summary></summary>
		public static void Delete(long smsMTNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsMTNum);
				return;
			}
			string command= "DELETE FROM smsmt WHERE SmsMTNum = "+POut.Long(smsMTNum);
			Db.NonQ(command);
		}
		*/



	}
}