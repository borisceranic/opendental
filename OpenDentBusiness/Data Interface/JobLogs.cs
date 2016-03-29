using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobLogs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.


		///<summary></summary>
		public static void Update(JobLog jobLog){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobLog);
				return;
			}
			Crud.JobLogCrud.Update(jobLog);
		}

		///<summary></summary>
		public static void Delete(long jobLogNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobLogNum);
				return;
			}
			Crud.JobLogCrud.Delete(jobLogNum);
		}

		

		
		*/

		/// <summary>Inserts log entry to DB and returns the resulting JobLog.</summary>
		public static JobLog MakeLogEntry(Job jobNew,Job jobOld) {
			if(jobNew==null) {
				return null;//should never happen
			}
			JobLog jobLog = new JobLog() {
				JobNum=jobNew.JobNum,
				UserNumChanged=Security.CurUser.UserNum,
				UserNumExpert=jobNew.UserNumExpert,
				UserNumEngineer=jobNew.UserNumEngineer,
				Description=""
			};
			List<string> logDescriptions = new List<string>();
			if(jobOld.IsApprovalNeeded && !jobNew.IsApprovalNeeded) {
				if(jobOld.PhaseCur==JobPhase.Concept  && (jobNew.PhaseCur==JobPhase.Definiton || jobNew.PhaseCur==JobPhase.Development)) {
					logDescriptions.Add("Concept approved.");
					jobLog.MainRTF=jobNew.Description;
				}
				if((jobOld.PhaseCur==JobPhase.Concept || jobOld.PhaseCur==JobPhase.Definiton) && jobNew.PhaseCur==JobPhase.Development) {
					logDescriptions.Add("Job approved.");
					jobLog.MainRTF=jobNew.Description;
				}
				if(jobOld.PhaseCur==JobPhase.Development && jobNew.PhaseCur==JobPhase.Development) {
					logDescriptions.Add("Changes approved.");
					jobLog.MainRTF=jobNew.Description;
				}
			}
			else if(jobNew.PhaseCur==JobPhase.Documentation && jobOld.PhaseCur!=JobPhase.Documentation) {
				logDescriptions.Add("Job implemented.");
				jobLog.MainRTF+=jobNew.Description;
			}
			if(jobOld.PhaseCur>jobNew.PhaseCur && jobOld.PhaseCur!=JobPhase.Cancelled) {
				logDescriptions.Add("Job Unapproved.");//may be a chance for a false positive when using override permission.
			}
			if(jobNew.UserNumExpert!=jobOld.UserNumExpert) {
				logDescriptions.Add("Expert changed.");
			}
			if(jobNew.UserNumEngineer!=jobOld.UserNumEngineer) {
				logDescriptions.Add("Engineer changed.");
			}
			jobLog.Description=string.Join("\r\n",logDescriptions);
			if(string.IsNullOrWhiteSpace(jobLog.Description)) {
				return null;//no job log created or inserted.
			}
			JobLogs.Insert(jobLog);
			return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
		}

		public static void DeleteForJob(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			string command = "DELETE FROM joblog WHERE JobNum="+POut.Long(jobNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static List<JobLog> GetJobLogsForJobs(List<long> listJobNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobLog>>(MethodBase.GetCurrentMethod(),listJobNums);
			}
			if(listJobNums==null || listJobNums.Count==0) {
				return new List<JobLog>();
			}
			string command = "SELECT * FROM joblog WHERE JobNum IN ("+string.Join(",",listJobNums)+")";
			return Crud.JobLogCrud.SelectMany(command);
		}

		///<summary>Gets one JobLog from the db.</summary>
		public static JobLog GetOne(long jobLogNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<JobLog>(MethodBase.GetCurrentMethod(),jobLogNum);
			}
			return Crud.JobLogCrud.SelectOne(jobLogNum);
		}

		///<summary></summary>
		public static long Insert(JobLog jobLog) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				jobLog.JobLogNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobLog);
				return jobLog.JobLogNum;
			}
			return Crud.JobLogCrud.Insert(jobLog);
		}

	}
}