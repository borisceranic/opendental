using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class StateAbbrs{
		#region CachePattern
		///<summary>A list of all StateAbbrs.</summary>
		private static List<StateAbbr> listt;

		///<summary>A list of all StateAbbrs.</summary>
		public static List<StateAbbr> Listt{
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
			string command="SELECT * FROM stateabbr ORDER BY Abbr";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="StateAbbr";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.StateAbbrCrud.TableToList(table);
		}
		#endregion

		///<summary></summary>
		public static long Insert(StateAbbr stateAbbr) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				stateAbbr.StateAbbrNum=Meth.GetLong(MethodBase.GetCurrentMethod(),stateAbbr);
				return stateAbbr.StateAbbrNum;
			}
			return Crud.StateAbbrCrud.Insert(stateAbbr);
		}

		///<summary></summary>
		public static void Update(StateAbbr stateAbbr) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stateAbbr);
				return;
			}
			Crud.StateAbbrCrud.Update(stateAbbr);
		}

		///<summary></summary>
		public static void Delete(long stateAbbrNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stateAbbrNum);
				return;
			}
			Crud.StateAbbrCrud.Delete(stateAbbrNum);
		}

		///<summary>Returns an list of StatesAbbrs with abbreviations similar to the supplied string.
		///Used in dropdown list from state field for faster entry.</summary>
		public static List<StateAbbr> GetSimilarAbbrs(string abbr) {
			//No need to check RemotingRole; no call to db.
			List<StateAbbr> retVal=new List<StateAbbr>();
			List<StateAbbr> stateAbbrs=StateAbbrs.Listt;
			for(int i=0;i<stateAbbrs.Count;i++) {
				if(stateAbbrs[i].Abbr.StartsWith(abbr,StringComparison.CurrentCultureIgnoreCase)) {
					retVal.Add(stateAbbrs[i]);
				}
			}
			return retVal;
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<StateAbbr> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<StateAbbr>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM stateabbr WHERE PatNum = "+POut.Long(patNum);
			return Crud.StateAbbrCrud.SelectMany(command);
		}

		///<summary>Gets one StateAbbr from the db.</summary>
		public static StateAbbr GetOne(long stateAbbrNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<StateAbbr>(MethodBase.GetCurrentMethod(),stateAbbrNum);
			}
			return Crud.StateAbbrCrud.SelectOne(stateAbbrNum);
		}
		*/

	}
}