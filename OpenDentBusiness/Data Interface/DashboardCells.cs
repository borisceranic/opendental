using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DashboardCells{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all DashboardCells.</summary>
		private static List<DashboardCell> listt;

		///<summary>A list of all DashboardCells.</summary>
		public static List<DashboardCell> Listt{
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
			string command="SELECT * FROM dashboardcell ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="DashboardCell";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.DashboardCellCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DashboardCell> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DashboardCell>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM dashboardcell WHERE PatNum = "+POut.Long(patNum);
			return Crud.DashboardCellCrud.SelectMany(command);
		}

		///<summary>Gets one DashboardCell from the db.</summary>
		public static DashboardCell GetOne(long dashboardCellNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<DashboardCell>(MethodBase.GetCurrentMethod(),dashboardCellNum);
			}
			return Crud.DashboardCellCrud.SelectOne(dashboardCellNum);
		}

		///<summary></summary>
		public static long Insert(DashboardCell dashboardCell){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				dashboardCell.DashboardCellNum=Meth.GetLong(MethodBase.GetCurrentMethod(),dashboardCell);
				return dashboardCell.DashboardCellNum;
			}
			return Crud.DashboardCellCrud.Insert(dashboardCell);
		}

		///<summary></summary>
		public static void Update(DashboardCell dashboardCell){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dashboardCell);
				return;
			}
			Crud.DashboardCellCrud.Update(dashboardCell);
		}

		///<summary></summary>
		public static void Delete(long dashboardCellNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dashboardCellNum);
				return;
			}
			Crud.DashboardCellCrud.Delete(dashboardCellNum);
		}

		

		
		*/



	}
}