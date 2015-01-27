using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class UserodApptViews {

		///<summary>Gets the most recent UserodApptView from the db for the user and clinic.  clinicNum can be 0.  Returns null if no match found.</summary>
		public static UserodApptView GetOneForUserAndClinic(long userNum,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<UserodApptView>(MethodBase.GetCurrentMethod(),userNum,clinicNum);
			}
			string command="SELECT * FROM userodapptview "
				+"WHERE UserNum = "+POut.Long(userNum)+" "
				+"AND ClinicNum = "+POut.Long(clinicNum)+" ";//If clinicNum of 0 passed in, we MUST filter by 0 because that is a valid entry in the db.
			return Crud.UserodApptViewCrud.SelectOne(command);
		}

		public static void InsertOrUpdate(long userNum,long clinicNum,long apptViewNum) {
			//No need to check RemotingRole; no call to db.
			if(apptViewNum<1) {
				return;//do not record the 'none' view for the user
			}
			UserodApptView userodApptView=new UserodApptView();
			userodApptView.UserNum=userNum;
			userodApptView.ClinicNum=clinicNum;
			userodApptView.ApptViewNum=apptViewNum;
			//Check if there is already a row in the database for this user, clinic, and apptview.
			UserodApptView userodApptViewDb=GetOneForUserAndClinic(userodApptView.UserNum,userodApptView.ClinicNum);
			if(userodApptViewDb==null) {
				Insert(userodApptView);
			}
			else if(userodApptViewDb.ApptViewNum!=userodApptView.ApptViewNum) {
				userodApptViewDb.ApptViewNum=userodApptView.ApptViewNum;
				Update(userodApptViewDb);
			}
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

		///<summary>Gets all recent userodapptviews for the user passed in.  Multiple userodapptviews can be returned when using clinics.</summary>
		public static List<UserodApptView> GetForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<UserodApptView>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM userodapptview WHERE UserNum = "+POut.Long(userNum);
			return Crud.UserodApptViewCrud.SelectMany(command);
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