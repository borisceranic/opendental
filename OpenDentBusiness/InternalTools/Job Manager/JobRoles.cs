using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobRoles{

		///<summary>JobRoles cannot be hidden so there is only one list.</summary>
		private static List<JobRole> _list;
		private static object _lockObj=new object();

		public static List<JobRole> List {
			//No need to check RemotingRole; no call to db.
			get {
				return GetList();
			}
			set {
				lock(_lockObj) {
					_list=value;
				}
			}
		}

		public static List<JobRole> GetList() {
			//No need to check RemotingRole; no call to db.
			bool isListNull=false;
			lock(_lockObj) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			List<JobRole> jobRoles;
			lock(_lockObj) {
				jobRoles=new List<JobRole>();
				for(int i=0;i<_list.Count;i++) {
					jobRoles.Add(_list[i].Copy());
				}
			}
			return jobRoles;
		}

		///<summary>Refresh all jobroles.  Not actually part of official cache.</summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM jobrole";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="JobRole";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List=Crud.JobRoleCrud.TableToList(table);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(JobRoleType jobRole) {
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(jobRole,false);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(JobRoleType jobRole,bool suppressMessage) {
			//No need to check RemotingRole; no call to db.
			if(Security.CurUser==null) {
				return false;
			}
			if(GetList().Count(x => x.UserNum==Security.CurUser.UserNum && x.RoleType==jobRole) > 0) {
				return true;
			}
			if(!suppressMessage) {
				MessageBox.Show(Lans.g("Security","A user with the SecurityAdmin permission must grant you access for job role")+"\r\n"+jobRole.ToString());
			}
			return false;
		}

		///<summary></summary>
		public static List<JobRole> GetForUser(long userNum){
			return GetList().Where(x => x.UserNum==userNum).ToList(); 
		}

		///<summary>Gets one JobRole from the db.</summary>
		public static JobRole GetOne(long jobRoleNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobRole>(MethodBase.GetCurrentMethod(),jobRoleNum);
			}
			return Crud.JobRoleCrud.SelectOne(jobRoleNum);
		}

		///<summary></summary>
		public static long Insert(JobRole jobRole){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobRole.JobRoleNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobRole);
				return jobRole.JobRoleNum;
			}
			return Crud.JobRoleCrud.Insert(jobRole);
		}

		///<summary></summary>
		public static void Update(JobRole jobRole){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobRole);
				return;
			}
			Crud.JobRoleCrud.Update(jobRole);
		}

		///<summary>Inserts, updates, or deletes the passed in list against rows for the passed in user.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobRole> listNew,long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,userNum);
			}
			List<JobRole> listDB=GetForUser(userNum);
			return Crud.JobRoleCrud.Sync(listNew,listDB);
		}

		///<summary></summary>
		public static void Delete(long jobRoleNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobRoleNum);
				return;
			}
			Crud.JobRoleCrud.Delete(jobRoleNum);
		}



	}
}