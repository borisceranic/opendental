using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabFacAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabFacAttaches.</summary>
		private static List<MedLabFacAttach> listt;

		///<summary>A list of all MedLabFacAttaches.</summary>
		public static List<MedLabFacAttach> Listt{
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
			string command="SELECT * FROM medlabfacattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabFacAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabFacAttachCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static long Insert(MedLabFacAttach medLabFacAttach) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				medLabFacAttach.MedLabFacAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabFacAttach);
				return medLabFacAttach.MedLabFacAttachNum;
			}
			return Crud.MedLabFacAttachCrud.Insert(medLabFacAttach);
		}

		///<summary>Gets all MedLabFacAttach objects from the db for a MedLab or a MedLabResult.  Only one parameter is required,
		///EITHER a MedLabNum OR a MedLabResultNum.  The other parameter should be 0.  If both parameters are >0, then list
		///returned will be all MedLabFacAttaches with EITHER the MedLabNum OR the MedLabResultNum provided.</summary>
		public static List<MedLabFacAttach> GetAllForLabOrResult(long medLabNum,long medLabResultNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabFacAttach>>(MethodBase.GetCurrentMethod(),medLabNum,medLabResultNum);
			}
			string command="SELECT * FROM medlabfacattach WHERE ";
			if(medLabNum!=0) {
				command+="MedLabNum="+POut.Long(medLabNum);
			}
			if(medLabResultNum!=0) {
				if(medLabNum!=0) {
					command+=" OR ";
				}
				command+="MedLabResultNum="+POut.Long(medLabResultNum)+" ";
			}
			command+="ORDER BY MedLabFacAttachNum DESC";
			return Crud.MedLabFacAttachCrud.SelectMany(command);
		}

		///<summary>Delete all MedLabFacAttach objects for the list of MedLabNums and/or list of MedLabResultNums.  Supply either list or both lists and
		///the MedLabFacAttach entries for either list will be deleted.  This could leave MedLabFacility entries not attached
		///to any lab or result, but we won't worry about cleaning those up since the MedLabFacility table will likely always remain very small.</summary>
		public static void DeleteAllForLabsOrResults(List<long> listLabNums,List<long> listResultNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listLabNums,listResultNums);
				return;
			}
			string command="DELETE FROM medlabfacattach "
				+"WHERE MedLabNum IN("+String.Join(",",listLabNums)+") "
				+"OR MedLabResultNum IN("+String.Join(",",listResultNums)+")";
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabFacAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabFacAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabfacattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabFacAttachCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabFacAttach from the db.</summary>
		public static MedLabFacAttach GetOne(long medLabFacAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabFacAttach>(MethodBase.GetCurrentMethod(),medLabFacAttachNum);
			}
			return Crud.MedLabFacAttachCrud.SelectOne(medLabFacAttachNum);
		}

		///<summary></summary>
		public static void Update(MedLabFacAttach medLabFacAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabFacAttach);
				return;
			}
			Crud.MedLabFacAttachCrud.Update(medLabFacAttach);
		}

		///<summary></summary>
		public static void Delete(long medLabFacAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabFacAttachNum);
				return;
			}
			string command= "DELETE FROM medlabfacattach WHERE MedLabFacAttachNum = "+POut.Long(medLabFacAttachNum);
			Db.NonQ(command);
		}
		*/



	}
}