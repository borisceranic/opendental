using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class OrthoChartTabs{
		#region CachePattern

		///<summary>A list of all OrthoChartTabs.</summary>
		private static List<OrthoChartTab> _list;
		///<summary>A list of all OrthoChartTabs which are not hidden.</summary>
		private static List<OrthoChartTab> _listt;

		///<summary>A list of all OrthoChartTabs.</summary>
		public static List<OrthoChartTab> List {
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

		///<summary>A list of all OrthoChartTabs which are not hidden.</summary>
		public static List<OrthoChartTab> Listt {
			get {
				if(_listt==null) {
					RefreshCache();
				}
				return _listt;
			}
			set {
				_listt=value;
			}
		}

		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM orthocharttab ORDER BY ItemOrder";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="OrthoChartTab";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			_list=Crud.OrthoChartTabCrud.TableToList(table);
			_listt=_list.FindAll(x => !x.IsHidden);
		}
		#endregion

		///<summary>Inserts, updates, or deletes the passed in list against the stale list listOld.  Returns true if db changes were made.</summary>
		public static bool Sync(List<OrthoChartTab> listNew,List<OrthoChartTab> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			return Crud.OrthoChartTabCrud.Sync(listNew,listOld);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<OrthoChartTab> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<OrthoChartTab>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM orthocharttab WHERE PatNum = "+POut.Long(patNum);
			return Crud.OrthoChartTabCrud.SelectMany(command);
		}

		///<summary>Gets one OrthoChartTab from the db.</summary>
		public static OrthoChartTab GetOne(long orthoChartTabNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<OrthoChartTab>(MethodBase.GetCurrentMethod(),orthoChartTabNum);
			}
			return Crud.OrthoChartTabCrud.SelectOne(orthoChartTabNum);
		}

		///<summary></summary>
		public static long Insert(OrthoChartTab orthoChartTab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				orthoChartTab.OrthoChartTabNum=Meth.GetLong(MethodBase.GetCurrentMethod(),orthoChartTab);
				return orthoChartTab.OrthoChartTabNum;
			}
			return Crud.OrthoChartTabCrud.Insert(orthoChartTab);
		}

		///<summary></summary>
		public static void Update(OrthoChartTab orthoChartTab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoChartTab);
				return;
			}
			Crud.OrthoChartTabCrud.Update(orthoChartTab);
		}

		///<summary></summary>
		public static void Delete(long orthoChartTabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoChartTabNum);
				return;
			}
			Crud.OrthoChartTabCrud.Delete(orthoChartTabNum);
		}

		

		
		*/



	}
}