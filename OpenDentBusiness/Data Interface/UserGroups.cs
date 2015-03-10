using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserGroups {
		private static UserGroup[] list;

		///<summary>A list of all user groups, ordered by description.</summary>
		public static UserGroup[] List {
			//No need to check RemotingRole; no call to db.
			get {
				if(list==null) {
					RefreshCache();
				}
				return list;
			}
			set {
				list=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * from usergroup ORDER BY Description";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="UserGroup";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			list=Crud.UserGroupCrud.TableToList(table).ToArray();
		}

		///<summary>A list of all user groups, ordered by description.  Does not include CEMT user groups.</summary>
		public static List<UserGroup> GetList() {
			return GetList(false);
		}

		///<summary>A list of all user groups, ordered by description.  Set includeCEMT to true if you want CEMT user groups included.</summary>
		public static List<UserGroup> GetList(bool includeCEMT) {
			//No need to check RemotingRole; no call to db.
			List<UserGroup> retVal=new List<UserGroup>();
			for(int i=0;i<List.Length;i++) {
				if(!includeCEMT && UserGroups.List[i].UserGroupNumCEMT!=0) {
					continue;
				}
				retVal.Add(List[i].Copy());
			}
			return retVal;
		}

		///<summary></summary>
		public static void Update(UserGroup group){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),group);
				return;
			}
			Crud.UserGroupCrud.Update(group);
		}

		///<summary>Only called from the CEMT in order to update a remote database with changes.  This method will update rows based on the UserGroupNumCEMT instead of the typical UserGroupNum column.</summary>
		public static void UpdateCEMT(UserGroup userGroupCEMT) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userGroupCEMT);
				return;
			}
			string command="UPDATE usergroup SET "
				+"Description = '"+POut.String(userGroupCEMT.Description)+"' "
				+"WHERE UserGroupNumCEMT = "+POut.Long(userGroupCEMT.UserGroupNum);
			Db.NonQ(command);
		}

		public static List<UserGroup> GetCEMTGroups() {
			//No need to check RemotingRole; no call to db.
			List<UserGroup> retVal=new List<UserGroup>();
			for(int i=0;i<List.Length;i++){
				if(List[i].UserGroupNumCEMT!=0){
					retVal.Add(List[i].Copy());
				}
			}
			return retVal;
		}

		///<summary></summary>
		public static long Insert(UserGroup group) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				group.UserGroupNum=Meth.GetLong(MethodBase.GetCurrentMethod(),group);
				return group.UserGroupNum;
			}
			return Crud.UserGroupCrud.Insert(group);
		}

		///<summary>Checks for dependencies first</summary>
		public static void Delete(UserGroup group){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),group);
				return;
			}
			string command="SELECT COUNT(*) FROM userod WHERE UserGroupNum='"
				+POut.Long(group.UserGroupNum)+"'";
			DataTable table=Db.GetTable(command);
			if(table.Rows[0][0].ToString()!="0"){
				throw new Exception(Lans.g("UserGroups","Must move users to another group first."));
			}
			if(PrefC.GetLong(PrefName.SecurityGroupForStudents)==group.UserGroupNum) {
				throw new Exception(Lans.g("UserGroups","Group is the default group for students and cannot be deleted.  Change the default student group before deleting."));
			}
			if(PrefC.GetLong(PrefName.SecurityGroupForInstructors)==group.UserGroupNum) {
				throw new Exception(Lans.g("UserGroups","Group is the default group for instructors and cannot be deleted.  Change the default instructors group before deleting."));
			}
			command= "DELETE FROM usergroup WHERE UserGroupNum='"
				+POut.Long(group.UserGroupNum)+"'";
			Db.NonQ(command);
			command="DELETE FROM grouppermission WHERE UserGroupNum='"
				+POut.Long(group.UserGroupNum)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static UserGroup GetGroup(long userGroupNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].UserGroupNum==userGroupNum){
					return List[i].Copy();
				}
			}
			return null;
		}

		

	}
 
	

	
}













