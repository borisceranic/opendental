using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabFacilities{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabFacilities.</summary>
		private static List<MedLabFacility> listt;

		///<summary>A list of all MedLabFacilities.</summary>
		public static List<MedLabFacility> Listt{
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
			string command="SELECT * FROM medlabfacility ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabFacility";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabFacilityCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabFacility> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabFacility>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabfacility WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabFacilityCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabFacility from the db.</summary>
		public static MedLabFacility GetOne(long medLabFacilityNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabFacility>(MethodBase.GetCurrentMethod(),medLabFacilityNum);
			}
			return Crud.MedLabFacilityCrud.SelectOne(medLabFacilityNum);
		}

		///<summary></summary>
		public static long Insert(MedLabFacility medLabFacility){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				medLabFacility.MedLabFacilityNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabFacility);
				return medLabFacility.MedLabFacilityNum;
			}
			return Crud.MedLabFacilityCrud.Insert(medLabFacility);
		}

		///<summary></summary>
		public static void Update(MedLabFacility medLabFacility){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabFacility);
				return;
			}
			Crud.MedLabFacilityCrud.Update(medLabFacility);
		}

		///<summary></summary>
		public static void Delete(long medLabFacilityNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabFacilityNum);
				return;
			}
			string command= "DELETE FROM medlabfacility WHERE MedLabFacilityNum = "+POut.Long(medLabFacilityNum);
			Db.NonQ(command);
		}
		*/



	}
}