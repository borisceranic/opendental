using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserodApptViews{

		///<summary>Gets all recent userodapptviews for the user passed in.  Multiple userodapptviews can be returned when using clinics.</summary>
		public static List<UserodApptView> GetForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<UserodApptView>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM userodapptview WHERE UserNum = "+POut.Long(userNum);
			return Crud.UserodApptViewCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(UserodApptView userodApptView) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				userodApptView.UserodApptViewNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userodApptView);
				return userodApptView.UserodApptViewNum;
			}
			return Crud.UserodApptViewCrud.Insert(userodApptView);
		}

		///<summary></summary>
		public static void Update(UserodApptView userodApptView) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userodApptView);
				return;
			}
			Crud.UserodApptViewCrud.Update(userodApptView);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all UserodApptViews.</summary>
		private static List<UserodApptView> listt;

		///<summary>A list of all UserodApptViews.</summary>
		public static List<UserodApptView> Listt{
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
			string command="SELECT * FROM userodapptview ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="UserodApptView";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.UserodApptViewCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one UserodApptView from the db.</summary>
		public static UserodApptView GetOne(long userodApptViewNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<UserodApptView>(MethodBase.GetCurrentMethod(),userodApptViewNum);
			}
			return Crud.UserodApptViewCrud.SelectOne(userodApptViewNum);
		}

		///<summary></summary>
		public static void Delete(long userodApptViewNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userodApptViewNum);
				return;
			}
			string command= "DELETE FROM userodapptview WHERE UserodApptViewNum = "+POut.Long(userodApptViewNum);
			Db.NonQ(command);
		}
		*/



	}
}