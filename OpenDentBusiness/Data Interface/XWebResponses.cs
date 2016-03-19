using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class XWebResponses{
		///<summary>Gets one XWebResponse from the db.</summary>
		public static XWebResponse GetOne(long xWebResponseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<XWebResponse>(MethodBase.GetCurrentMethod(),xWebResponseNum);
			}
			return Crud.XWebResponseCrud.SelectOne(xWebResponseNum);
		}

		///<summary>Gets all XWebResponses where TransactionStatus==XWebTransactionStatus.HpfPending from the db.</summary>
		public static List<XWebResponse> GetPendingHPFs(DateTime notNewerThan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<XWebResponse>>(MethodBase.GetCurrentMethod());
			}
			return Crud.XWebResponseCrud.SelectMany("SELECT * FROM xwebresponse "
				+"WHERE "
				+"TransactionStatus = "+POut.Int((int)XWebTransactionStatus.HpfPending)
				+"AND DateTUpdate = <"+POut.DateT(notNewerThan));
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all XWebResponses.</summary>
		private static List<XWebResponse> listt;

		///<summary>A list of all XWebResponses.</summary>
		public static List<XWebResponse> Listt{
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
			string command="SELECT * FROM xwebresponse ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="XWebResponse";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.XWebResponseCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<XWebResponse> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<XWebResponse>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM xwebresponse WHERE PatNum = "+POut.Long(patNum);
			return Crud.XWebResponseCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(XWebResponse xWebResponse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				xWebResponse.XWebResponseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),xWebResponse);
				return xWebResponse.XWebResponseNum;
			}
			return Crud.XWebResponseCrud.Insert(xWebResponse);
		}

		///<summary></summary>
		public static void Update(XWebResponse xWebResponse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xWebResponse);
				return;
			}
			Crud.XWebResponseCrud.Update(xWebResponse);
		}

		///<summary></summary>
		public static void Delete(long xWebResponseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xWebResponseNum);
				return;
			}
			Crud.XWebResponseCrud.Delete(xWebResponseNum);
		}

		

		
		*/



	}
}