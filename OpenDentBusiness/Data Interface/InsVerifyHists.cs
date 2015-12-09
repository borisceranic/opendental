using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class InsVerifyHists{

		///<summary>Gets one InsVerifyHist from the db.</summary>
		public static InsVerifyHist GetOne(long insVerifyHistNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsVerifyHist>(MethodBase.GetCurrentMethod(),insVerifyHistNum);
			}
			return Crud.InsVerifyHistCrud.SelectOne(insVerifyHistNum);
		}

		///<summary></summary>
		public static long Insert(InsVerifyHist insVerifyHist) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				insVerifyHist.InsVerifyHistNum=Meth.GetLong(MethodBase.GetCurrentMethod(),insVerifyHist);
				return insVerifyHist.InsVerifyHistNum;
			}
			return Crud.InsVerifyHistCrud.Insert(insVerifyHist);
		}

		///<summary></summary>
		public static void Update(InsVerifyHist insVerifyHist) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerifyHist);
				return;
			}
			Crud.InsVerifyHistCrud.Update(insVerifyHist);
		}

		///<summary></summary>
		public static void Delete(long insVerifyHistNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerifyHistNum);
				return;
			}
			Crud.InsVerifyHistCrud.Delete(insVerifyHistNum);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all InsVerifyHists.</summary>
		private static List<InsVerifyHist> listt;

		///<summary>A list of all InsVerifyHists.</summary>
		public static List<InsVerifyHist> Listt{
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
			string command="SELECT * FROM insverifyhist ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="InsVerifyHist";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.InsVerifyHistCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<InsVerifyHist> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerifyHist>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM insverifyhist WHERE PatNum = "+POut.Long(patNum);
			return Crud.InsVerifyHistCrud.SelectMany(command);
		}
		
		*/



	}
}