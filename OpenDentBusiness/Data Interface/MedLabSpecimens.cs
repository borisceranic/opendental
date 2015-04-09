using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabSpecimens{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabSpecimens.</summary>
		private static List<MedLabSpecimen> listt;

		///<summary>A list of all MedLabSpecimens.</summary>
		public static List<MedLabSpecimen> Listt{
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
			string command="SELECT * FROM medlabspecimen ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabSpecimen";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabSpecimenCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static long Insert(MedLabSpecimen medLabSpecimen) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				medLabSpecimen.MedLabSpecimenNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabSpecimen);
				return medLabSpecimen.MedLabSpecimenNum;
			}
			return Crud.MedLabSpecimenCrud.Insert(medLabSpecimen);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabSpecimen> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabSpecimen>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabspecimen WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabSpecimenCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabSpecimen from the db.</summary>
		public static MedLabSpecimen GetOne(long medLabSpecimenNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabSpecimen>(MethodBase.GetCurrentMethod(),medLabSpecimenNum);
			}
			return Crud.MedLabSpecimenCrud.SelectOne(medLabSpecimenNum);
		}

		///<summary></summary>
		public static void Update(MedLabSpecimen medLabSpecimen){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabSpecimen);
				return;
			}
			Crud.MedLabSpecimenCrud.Update(medLabSpecimen);
		}

		///<summary></summary>
		public static void Delete(long medLabSpecimenNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabSpecimenNum);
				return;
			}
			string command= "DELETE FROM medlabspecimen WHERE MedLabSpecimenNum = "+POut.Long(medLabSpecimenNum);
			Db.NonQ(command);
		}
		*/



	}
}