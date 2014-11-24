using System;
using System.Collections;
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

		///<summary></summary>
		public static void Update(UserGroup group){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),group);
				return;
			}
			Crud.UserGroupCrud.Update(group);
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













