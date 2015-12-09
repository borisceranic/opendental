using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class InsVerifies {

		///<summary>Gets one InsVerify from the db.</summary>
		public static InsVerify GetOne(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsVerify>(MethodBase.GetCurrentMethod(),insVerifyNum);
			}
			return Crud.InsVerifyCrud.SelectOne(insVerifyNum);
		}

		///<summary></summary>
		public static long Insert(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				insVerify.InsVerifyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),insVerify);
				return insVerify.InsVerifyNum;
			}
			return Crud.InsVerifyCrud.Insert(insVerify);
		}

		///<summary></summary>
		public static void Update(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerify);
				return;
			}
			Crud.InsVerifyCrud.Update(insVerify);
		}

		///<summary></summary>
		public static void Delete(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerifyNum);
				return;
			}
			Crud.InsVerifyCrud.Delete(insVerifyNum);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all InsVerifies.</summary>
		private static List<InsVerify> listt;

		///<summary>A list of all InsVerifies.</summary>
		public static List<InsVerify> Listt{
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
			string command="SELECT * FROM insverify ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="InsVerify";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.InsVerifyCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<InsVerify> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerify>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM insverify WHERE PatNum = "+POut.Long(patNum);
			return Crud.InsVerifyCrud.SelectMany(command);
		}
		*/
	}
}