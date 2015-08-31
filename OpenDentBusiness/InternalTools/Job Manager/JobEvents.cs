using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobEvents{

		///<summary></summary>
		public static List<JobEvent> GetByJobNum(long jobNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobevent WHERE JobNum = "+POut.Long(jobNum);
			return Crud.JobEventCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<JobEvent> GetByOwner(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM jobevent WHERE Owner = "+POut.Long(userNum);
			return Crud.JobEventCrud.SelectMany(command);
		}

		///<summary>Gets one JobEvent from the db.</summary>
		public static JobEvent GetOne(long jobEventNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobEvent>(MethodBase.GetCurrentMethod(),jobEventNum);
			}
			return Crud.JobEventCrud.SelectOne(jobEventNum);
		}

		///<summary></summary>
		public static long Insert(JobEvent jobEvent){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobEvent.JobEventNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobEvent);
				return jobEvent.JobEventNum;
			}
			return Crud.JobEventCrud.Insert(jobEvent);
		}

		///<summary></summary>
		public static void Update(JobEvent jobEvent){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobEvent);
				return;
			}
			Crud.JobEventCrud.Update(jobEvent);
		}

		///<summary></summary>
		public static void Delete(long jobEventNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobEventNum);
				return;
			}
			Crud.JobEventCrud.Delete(jobEventNum);
		}



	}
}