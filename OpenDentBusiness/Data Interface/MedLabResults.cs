using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabResults{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabResults.</summary>
		private static List<MedLabResult> listt;

		///<summary>A list of all MedLabResults.</summary>
		public static List<MedLabResult> Listt{
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
			string command="SELECT * FROM medlabresult ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabResult";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabResultCrud.TableToList(table);
		}
		#endregion
		*/



		///<summary>Returns a list of all MedLabResults from the db for a given MedLab.  Ordered by ResultStatus,DateTimeObs DESC.
		///Corrected (ResultStatus=0) will be first in the list then 1=Final, 2=Incomplete, 3=Preliminary, and 4=Cancelled.
		///Then ordered by DateTimeObs DESC, most recent of each status comes first in the list.
		///If there are no results for the lab (or medLabNum=0), this will return an empty list.</summary>
		public static List<MedLabResult> GetForLab(long medLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),medLabNum);
			}
			string command="SELECT * FROM medlabresult WHERE MedLabNum="+POut.Long(medLabNum)+" ORDER BY ResultStatus,DateTimeObs DESC";
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(MedLabResult medLabResult) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				medLabResult.MedLabResultNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabResult);
				return medLabResult.MedLabResultNum;
			}
			return Crud.MedLabResultCrud.Insert(medLabResult);
		}

		///<summary></summary>
		public static void Update(MedLabResult medLabResult) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabResult);
				return;
			}
			Crud.MedLabResultCrud.Update(medLabResult);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabResult> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabresult WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabResult from the db.</summary>
		public static MedLabResult GetOne(long medLabResultNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabResult>(MethodBase.GetCurrentMethod(),medLabResultNum);
			}
			return Crud.MedLabResultCrud.SelectOne(medLabResultNum);
		}

		///<summary></summary>
		public static void Delete(long medLabResultNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabResultNum);
				return;
			}
			string command= "DELETE FROM medlabresult WHERE MedLabResultNum = "+POut.Long(medLabResultNum);
			Db.NonQ(command);
		}
		*/



	}
}