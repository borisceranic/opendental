using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Jobs{

		///<summary></summary>
		public static List<Job> GetForExpert(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM job WHERE Expert = "+POut.Long(userNum);
			return Crud.JobCrud.SelectMany(command);
		}

		public static List<Job> GetForProject(long projectNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),projectNum);
			}
			string command="SELECT * FROM job WHERE ProjectNum = "+POut.Long(projectNum);
			return Crud.JobCrud.SelectMany(command);
		}

		///<summary>Gets one Job from the db.</summary>
		public static Job GetOne(long jobNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<Job>(MethodBase.GetCurrentMethod(),jobNum);
			}
			return Crud.JobCrud.SelectOne(jobNum);
		}

		///<summary></summary>
		public static long Insert(Job job){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				job.JobNum=Meth.GetLong(MethodBase.GetCurrentMethod(),job);
				return job.JobNum;
			}
			return Crud.JobCrud.Insert(job);
		}

		///<summary></summary>
		public static void Update(Job job){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job);
				return;
			}
			Crud.JobCrud.Update(job);
		}

		///<summary></summary>
		public static void Delete(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			Crud.JobCrud.Delete(jobNum);
		}



	}
}