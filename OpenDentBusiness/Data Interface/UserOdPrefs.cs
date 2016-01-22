using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserOdPrefs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all UserOdPrefs.</summary>
		private static List<UserOdPref> listt;

		///<summary>A list of all UserOdPrefs.</summary>
		public static List<UserOdPref> Listt{
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
			string command="SELECT * FROM userodpref ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="UserOdPref";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.UserOdPrefCrud.TableToList(table);
		}
		#endregion

		///<summary></summary>
		public static List<UserOdPref> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<UserOdPref>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM userodpref WHERE PatNum = "+POut.Long(patNum);
			return Crud.UserOdPrefCrud.SelectMany(command);
		}

		///<summary>Gets one UserOdPref from the db.</summary>
		public static UserOdPref GetOne(long userOdPrefNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserOdPref>(MethodBase.GetCurrentMethod(),userOdPrefNum);
			}
			return Crud.UserOdPrefCrud.SelectOne(userOdPrefNum);
		}

		///<summary></summary>
		public static long Insert(UserOdPref userOdPref){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				userOdPref.UserOdPrefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userOdPref);
				return userOdPref.UserOdPrefNum;
			}
			return Crud.UserOdPrefCrud.Insert(userOdPref);
		}

		///<summary></summary>
		public static void Update(UserOdPref userOdPref){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userOdPref);
				return;
			}
			Crud.UserOdPrefCrud.Update(userOdPref);
		}

		public static void Update(long fkey,UserOdFkeyType fkeyType,string valueString) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),fkey,fkeyType,valueString);
				return;
			}
			string command="UPDATE userodpref SET ValueString='"+POut.String(valueString)+"' "+
				"WHERE Fkey="+POut.Long(fkey)+" AND FkeyType="+POut.Int((int)fkeyType);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(long userOdPrefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userOdPrefNum);
				return;
			}
			Crud.UserOdPrefCrud.Delete(userOdPrefNum);
		}

	}
}