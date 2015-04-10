using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsVlns{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsVlns.</summary>
		private static List<SmsVln> listt;

		///<summary>A list of all SmsVlns.</summary>
		public static List<SmsVln> Listt{
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
			string command="SELECT * FROM smsvln ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsVln";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsVlnCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsVln> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsVln>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsvln WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsVlnCrud.SelectMany(command);
		}

		///<summary>Gets one SmsVln from the db.</summary>
		public static SmsVln GetOne(long smsVlnNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsVln>(MethodBase.GetCurrentMethod(),smsVlnNum);
			}
			return Crud.SmsVlnCrud.SelectOne(smsVlnNum);
		}

		///<summary></summary>
		public static long Insert(SmsVln smsVln){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsVln.SmsVlnNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsVln);
				return smsVln.SmsVlnNum;
			}
			return Crud.SmsVlnCrud.Insert(smsVln);
		}

		///<summary></summary>
		public static void Update(SmsVln smsVln){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsVln);
				return;
			}
			Crud.SmsVlnCrud.Update(smsVln);
		}

		///<summary></summary>
		public static void Delete(long smsVlnNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsVlnNum);
				return;
			}
			string command= "DELETE FROM smsvln WHERE SmsVlnNum = "+POut.Long(smsVlnNum);
			Db.NonQ(command);
		}
		*/



	}
}