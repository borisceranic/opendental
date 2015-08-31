using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobLinks{		

		///<summary></summary>
		public static List<JobLink> GetByJobNum(long jobNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobLink>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM joblink WHERE JobNum = "+POut.Long(jobNum);
			return Crud.JobLinkCrud.SelectMany(command);
		}

		///<summary>Gets one JobLink from the db.</summary>
		public static JobLink GetOne(long jobLinknum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobLink>(MethodBase.GetCurrentMethod(),jobLinknum);
			}
			return Crud.JobLinkCrud.SelectOne(jobLinknum);
		}

		///<summary></summary>
		public static long Insert(JobLink jobLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobLink.JobLinknum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobLink);
				return jobLink.JobLinknum;
			}
			return Crud.JobLinkCrud.Insert(jobLink);
		}

		///<summary></summary>
		public static void Update(JobLink jobLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobLink);
				return;
			}
			Crud.JobLinkCrud.Update(jobLink);
		}

		///<summary></summary>
		public static void Delete(long jobLinknum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobLinknum);
				return;
			}
			Crud.JobLinkCrud.Delete(jobLinknum);
		}



	}
}