using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobEvents{

		///<summary></summary>
		public static List<JobEvent> GetForJob(long jobNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobevent WHERE JobNum = "+POut.Long(jobNum)
				+" ORDER BY DateTimeEntry";
			return Crud.JobEventCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<JobEvent> GetByOwner(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM jobevent WHERE Owner = "+POut.Long(userNum)
				+" ORDER BY DateTimeEntry";
			return Crud.JobEventCrud.SelectMany(command);
		}

		///<summary>Returns blank if no previous owner</summary>
		public static string GetPrevOwnerName(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),jobNum);
			}
			if(jobNum==0) {
				return "";
			}
			long ownerCur=Jobs.GetOne(jobNum).OwnerNum;
			string command="SELECT * FROM jobevent WHERE JobNum = "+POut.Long(jobNum)
				+" AND OwnerNum!="+POut.Long(ownerCur)+" ORDER BY DateTimeEntry DESC";
			List<JobEvent> listJobEvents=Crud.JobEventCrud.SelectMany(command);
			return Userods.GetName(listJobEvents.Select(x=>x.OwnerNum).FirstOrDefault());
		}

		///<summary>Gets one JobEvent from the db.</summary>
		public static JobEvent GetOne(long jobEventNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobEvent>(MethodBase.GetCurrentMethod(),jobEventNum);
			}
			return Crud.JobEventCrud.SelectOne(jobEventNum);
		}

		///<summary>Gets JobEvents for a specified JobNum.</summary>
		public static List<JobEvent> GetJobEvents(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobevent WHERE JobNum="+POut.Long(jobNum)
				+" ORDER BY DateTimeEntry";
			return Crud.JobEventCrud.SelectMany(command);
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

		public static void DeleteForJob(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			string command="DELETE FROM jobevent WHERE JobNum="+POut.Long(jobNum);
			Db.NonQ(command);
		}

		///<summary>Gets the most recent JobEvent of a job.</summary>
		public static JobEvent GetMostRecent(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<JobEvent>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobevent WHERE JobNum="+jobNum
				+" AND DateTimeEntry=(SELECT MAX(DateTimeEntry) FROM jobevent WHERE JobNum="+jobNum+")";
			return Crud.JobEventCrud.SelectOne(command);
		}

		public static List<JobEvent> GetJobEventsForJobs(List<long> jobNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobEvent>>(MethodBase.GetCurrentMethod(),jobNums);
			}
			if(jobNums==null || jobNums.Count==0) {
				return new List<JobEvent>();
			}
			string command="SELECT * FROM jobevent WHERE JobNum IN ("+string.Join(",",jobNums)+")";
			return Crud.JobEventCrud.SelectMany(command);
		}
	}
}