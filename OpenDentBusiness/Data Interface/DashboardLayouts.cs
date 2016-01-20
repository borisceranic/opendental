using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DashboardLayouts{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all DashboardLayouts.</summary>
		private static List<DashboardLayout> listt;

		///<summary>A list of all DashboardLayouts.</summary>
		public static List<DashboardLayout> Listt{
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
			string command="SELECT * FROM dashboardlayout ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="DashboardLayout";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.DashboardLayoutCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DashboardLayout> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DashboardLayout>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM dashboardlayout WHERE PatNum = "+POut.Long(patNum);
			return Crud.DashboardLayoutCrud.SelectMany(command);
		}

		///<summary>Gets one DashboardLayout from the db.</summary>
		public static DashboardLayout GetOne(long dashboardLayoutNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<DashboardLayout>(MethodBase.GetCurrentMethod(),dashboardLayoutNum);
			}
			return Crud.DashboardLayoutCrud.SelectOne(dashboardLayoutNum);
		}

		///<summary></summary>
		public static long Insert(DashboardLayout dashboardLayout){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				dashboardLayout.DashboardLayoutNum=Meth.GetLong(MethodBase.GetCurrentMethod(),dashboardLayout);
				return dashboardLayout.DashboardLayoutNum;
			}
			return Crud.DashboardLayoutCrud.Insert(dashboardLayout);
		}

		///<summary></summary>
		public static void Update(DashboardLayout dashboardLayout){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dashboardLayout);
				return;
			}
			Crud.DashboardLayoutCrud.Update(dashboardLayout);
		}

		///<summary></summary>
		public static void Delete(long dashboardLayoutNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dashboardLayoutNum);
				return;
			}
			Crud.DashboardLayoutCrud.Delete(dashboardLayoutNum);
		}

		

		
		*/



	}
}