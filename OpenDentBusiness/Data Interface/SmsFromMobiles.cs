using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsFromMobiles{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsMOs.</summary>
		private static List<SmsMO> listt;

		///<summary>A list of all SmsMOs.</summary>
		public static List<SmsMO> Listt{
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
			string command="SELECT * FROM smsmo ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsMO";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsMOCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsMO> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsMO>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsmo WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsMOCrud.SelectMany(command);
		}

		///<summary>Gets one SmsMO from the db.</summary>
		public static SmsMO GetOne(long smsMONum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsMO>(MethodBase.GetCurrentMethod(),smsMONum);
			}
			return Crud.SmsMOCrud.SelectOne(smsMONum);
		}

		///<summary></summary>
		public static long Insert(SmsMO smsMO){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsMO.SmsMONum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsMO);
				return smsMO.SmsMONum;
			}
			return Crud.SmsMOCrud.Insert(smsMO);
		}

		///<summary></summary>
		public static void Update(SmsMO smsMO){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsMO);
				return;
			}
			Crud.SmsMOCrud.Update(smsMO);
		}

		///<summary></summary>
		public static void Delete(long smsMONum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsMONum);
				return;
			}
			string command= "DELETE FROM smsmo WHERE SmsMONum = "+POut.Long(smsMONum);
			Db.NonQ(command);
		}
		*/



	}
}