using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class OrthoChartTabLinks{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all OrthoChartTabLinks.</summary>
		private static List<OrthoChartTabLink> _list;

		///<summary>A list of all OrthoChartTabLinks.</summary>
		public static List<OrthoChartTabLink> List{
			get {
				if(_list==null) {
					RefreshCache();
				}
				return _list;
			}
			set {
				_list=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM orthocharttablink ORDER BY ItemOrder";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="OrthoChartTabLink";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			_list=Crud.OrthoChartTabLinkCrud.TableToList(table);
		}
		#endregion

		///<summary>Inserts, updates, or deletes the passed in list against the stale list listOld.  Returns true if db changes were made.</summary>
		public static bool Sync(List<OrthoChartTabLink> listNew,List<OrthoChartTabLink> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			return Crud.OrthoChartTabLinkCrud.Sync(listNew,listOld);
		}
		
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<OrthoChartTabLink> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoChartTabLink>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM orthocharttablink WHERE PatNum = "+POut.Long(patNum);
			return Crud.OrthoChartTabLinkCrud.SelectMany(command);
		}

		///<summary>Gets one OrthoChartTabLink from the db.</summary>
		public static OrthoChartTabLink GetOne(long orthoChartTabLinkNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<OrthoChartTabLink>(MethodBase.GetCurrentMethod(),orthoChartTabLinkNum);
			}
			return Crud.OrthoChartTabLinkCrud.SelectOne(orthoChartTabLinkNum);
		}

		///<summary></summary>
		public static long Insert(OrthoChartTabLink orthoChartTabLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				orthoChartTabLink.OrthoChartTabLinkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),orthoChartTabLink);
				return orthoChartTabLink.OrthoChartTabLinkNum;
			}
			return Crud.OrthoChartTabLinkCrud.Insert(orthoChartTabLink);
		}

		///<summary></summary>
		public static void Update(OrthoChartTabLink orthoChartTabLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoChartTabLink);
				return;
			}
			Crud.OrthoChartTabLinkCrud.Update(orthoChartTabLink);
		}

		///<summary></summary>
		public static void Delete(long orthoChartTabLinkNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoChartTabLinkNum);
				return;
			}
			Crud.OrthoChartTabLinkCrud.Delete(orthoChartTabLinkNum);
		}

		

		
		*/



	}
}