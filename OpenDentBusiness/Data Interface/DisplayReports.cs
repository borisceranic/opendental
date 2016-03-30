using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DisplayReports{
		///<summary>Get all display reports for the passed-in category.  Pass in true to retrieve hidden display reports.</summary>
		public static List<DisplayReport> GetForCategory(DisplayReportCategory category, bool showHidden) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DisplayReport>>(MethodBase.GetCurrentMethod(),category,showHidden);
			}
			string command="SELECT * FROM displayreport WHERE Category="+POut.Int((int)category)+" ";
			if(!showHidden) {
				command+="AND IsHidden = 0 ";
			}
			command+="ORDER BY ItemOrder";
			return Crud.DisplayReportCrud.SelectMany(command);
		}

		///<summary>Pass in true to also retrieve hidden display reports.</summary>
		public static List<DisplayReport> GetAll(bool showHidden) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DisplayReport>>(MethodBase.GetCurrentMethod(),showHidden);
			}
			string command="SELECT * FROM displayreport ";
			if(!showHidden) {
				command+="WHERE IsHidden = 0 ";
			}
			command+="ORDER BY ItemOrder";
			return Crud.DisplayReportCrud.SelectMany(command);
		}

		///Must pass in a list of all current display reports, even hidden ones.
		public static void Sync(List<DisplayReport> listDisplayReport) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listDisplayReport);
			}
			Crud.DisplayReportCrud.Sync(listDisplayReport,GetAll(true));
		}


		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all DisplayReports.</summary>
		private static List<DisplayReport> listt;

		///<summary>A list of all DisplayReports.</summary>
		public static List<DisplayReport> Listt{
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
			string command="SELECT * FROM displayreport ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="DisplayReport";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.DisplayReportCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DisplayReport> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DisplayReport>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM displayreport WHERE PatNum = "+POut.Long(patNum);
			return Crud.DisplayReportCrud.SelectMany(command);
		}

		///<summary>Gets one DisplayReport from the db.</summary>
		public static DisplayReport GetOne(long displayReportNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<DisplayReport>(MethodBase.GetCurrentMethod(),displayReportNum);
			}
			return Crud.DisplayReportCrud.SelectOne(displayReportNum);
		}

		///<summary></summary>
		public static long Insert(DisplayReport displayReport){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				displayReport.DisplayReportNum=Meth.GetLong(MethodBase.GetCurrentMethod(),displayReport);
				return displayReport.DisplayReportNum;
			}
			return Crud.DisplayReportCrud.Insert(displayReport);
		}

		///<summary></summary>
		public static void Update(DisplayReport displayReport){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),displayReport);
				return;
			}
			Crud.DisplayReportCrud.Update(displayReport);
		}

		///<summary></summary>
		public static void Delete(long displayReportNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),displayReportNum);
				return;
			}
			Crud.DisplayReportCrud.Delete(displayReportNum);
		}

		

		
		*/



	}
}