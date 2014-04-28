using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class GradingScales{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all GradingScales.</summary>
		private static List<GradingScale> listt;

		///<summary>A list of all GradingScales.</summary>
		public static List<GradingScale> Listt{
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
			string command="SELECT * FROM gradingscale ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="GradingScale";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.GradingScaleCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<GradingScale> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<GradingScale>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM gradingscale WHERE PatNum = "+POut.Long(patNum);
			return Crud.GradingScaleCrud.SelectMany(command);
		}

		///<summary>Gets one GradingScale from the db.</summary>
		public static GradingScale GetOne(long gradingScaleNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<GradingScale>(MethodBase.GetCurrentMethod(),gradingScaleNum);
			}
			return Crud.GradingScaleCrud.SelectOne(gradingScaleNum);
		}

		///<summary></summary>
		public static long Insert(GradingScale gradingScale){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				gradingScale.GradingScaleNum=Meth.GetLong(MethodBase.GetCurrentMethod(),gradingScale);
				return gradingScale.GradingScaleNum;
			}
			return Crud.GradingScaleCrud.Insert(gradingScale);
		}

		///<summary></summary>
		public static void Update(GradingScale gradingScale){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gradingScale);
				return;
			}
			Crud.GradingScaleCrud.Update(gradingScale);
		}

		///<summary></summary>
		public static void Delete(long gradingScaleNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gradingScaleNum);
				return;
			}
			string command= "DELETE FROM gradingscale WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			Db.NonQ(command);
		}
		*/



	}
}