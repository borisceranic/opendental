using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserClinics{
		#region CachePattern
		
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all UserClinics.</summary>
		private static List<UserClinic> _listt;
		private static object _lockObj=new object();

		///<summary>A list of all UserClinics.</summary>
		public static List<UserClinic> Listt{
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>A list of all UserCliniics.</summary>
		public static List<UserClinic> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			List<UserClinic> listUserClinics=new List<UserClinic>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listUserClinics.Add(_listt[i].Copy());
				}
			}
			return listUserClinics;
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM userclinic";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="UserClinic";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			_listt=Crud.UserClinicCrud.TableToList(table);
		}
		#endregion

		///<summary>Gets all User to Clinic associations for the user.  Can return an empty list if there are none.</summary>
		public static List<UserClinic> GetForUser(long userNum){
			List<UserClinic> listUserClinics=GetListt();
			List<UserClinic> retVal=new List<UserClinic>();
			foreach(UserClinic userClinic in listUserClinics) {
				if(userClinic.UserNum==userNum) {
					retVal.Add(userClinic);
				}
			}
			return retVal;
		}

		///<summary>Gets all User to Clinic associations for a clinic.  Can return an empty list if there are none.</summary>
		public static List<UserClinic> GetForClinic(long clinicNum) {
			List<UserClinic> listUserClinics=GetListt();
			List<UserClinic> retVal=new List<UserClinic>();
			foreach(UserClinic userClinic in listUserClinics) {
				if(userClinic.ClinicNum==clinicNum) {
					retVal.Add(userClinic);
				}
			}
			return retVal;
		}

		///<summary>Gets one UserClinic from cache.  Can return null if none are found.</summary>
		public static UserClinic GetOne(long userClinicNum){
			List<UserClinic> listUserClinics=GetListt();
			foreach(UserClinic userClinic in listUserClinics) {
				if(userClinic.UserClinicNum==userClinicNum) {
					return userClinic;
				}
			}
			return null;
		}

		///<summary></summary>
		public static long Insert(UserClinic userClinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				userClinic.UserClinicNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userClinic);
				return userClinic.UserClinicNum;
			}
			return Crud.UserClinicCrud.Insert(userClinic);
		}

		///<summary></summary>
		public static void Update(UserClinic userClinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userClinic);
				return;
			}
			Crud.UserClinicCrud.Update(userClinic);
		}

		///<summary></summary>
		public static void Delete(long userClinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userClinicNum);
				return;
			}
			Crud.UserClinicCrud.Delete(userClinicNum);
		}

		public static bool Sync(List<UserClinic> listNew,long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,userNum);
			}
			List<UserClinic> listOld=UserClinics.GetForUser(userNum);
			return Crud.UserClinicCrud.Sync(listNew,listOld);
		}

		///<summary>Deletes all User to Clinic associations for a specific user.</summary>
		public static void DeleteForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userNum);
				return;
			}
			string command="DELETE FROM userclinic WHERE UserNum="+POut.Long(userNum);
			Db.NonQ(command);
		}


	}
}