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

		public static List<Job> GetForProject(long projectNum,bool showFinished) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),projectNum);
			}
			string command="SELECT * FROM job WHERE ProjectNum = "+POut.Long(projectNum);
			if(!showFinished) {
				command+=" AND JobStatus != " + (int)JobStatus.Done;
			}
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

		///<summary>You must surround with a try-catch when calling this method.  Deletes one job from the database.  
		///Also deletes all JobLinks associated with the job.  Jobs that have reviews on them may not be deleted and will throw an exception.</summary>
		public static void Delete(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			List<JobLink> listJobLinks=JobLinks.GetByJobNum(jobNum);
			for(int i=0;i<listJobLinks.Count;i++) { //look for reviews. Throw an exception if one is found.
				if(listJobLinks[i].LinkType==JobLinkType.Review) {
					throw new Exception(Lans.g("Jobs","Not allowed to delete a job that has attached reviews.  Set the status to deleted instead.")); //The exception is caught in FormJobEdit.
				}
			}
			//if there are any reviews, any code below this will not be executed.
			string command="DELETE FROM JobLink	WHERE JobNum="+jobNum;
			Db.NonQ(command);//Delete all jobLinks with matching jobNum from the linker table.
			command="DELETE FROM JobEvent WHERE JobNum="+jobNum;
			Db.NonQ(command);//Delete all jobEvents with matching jobNum.
			Crud.JobCrud.Delete(jobNum); //Finally, delete the job itself.
		}

		///<summary>Returns a table for use in UserControlJobs, filtered by the passed in params. 
		///String params can be "", JobNum can be 0, and other long params can be -1 if you do not want to filter by those params.</summary>
		public static DataTable GetJobDataTable(long jobNum,string expert,string owner,string version,
			string project,string title,long status,long priority,long type,bool showHidden) {
				if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
					return Meth.GetTable(MethodBase.GetCurrentMethod(),jobNum,expert,owner,version,project,title,status,priority,type,showHidden);
				}
				string command="SELECT JobNum, JobVersion, JobPriority, owner.UserName AS Owner, expert.UserName AS Expert, JobType, " 
						+"JobStatus, ProjectNum, Description, Title "
					+"FROM job "
					+"LEFT JOIN userod owner ON owner.UserNum = job.Owner "
					+"LEFT JOIN userod expert ON expert.UserNum = job.Expert "
					+"WHERE TRUE ";
			if(expert!="") {
				command+=" AND expert.UserName LIKE '%"+expert+"%'";
			} 
			if(owner!="") {
				command+=" AND owner.UserName LIKE '%"+owner+"%'";
			}
			if(version!=""){
				command+=" AND JobVersion LIKE '%"+version+"%'";
			}
			if(title!="") {
				command+=" AND Title LIKE '%"+title+"%'";
			}
			if(jobNum!=0) {
				command+=" AND JobNum="+jobNum;
			}
			if(status!=-1) {
				command+= " AND JobStatus="+status;			
			}
			if(priority!=-1) {
				command+=" AND JobPriority="+priority;
			}
			if(type!=-1) {
				command+=" AND JobType="+type;
			}
			if(!showHidden) {
				command+=" AND JobStatus NOT IN ("+(int)JobStatus.Deleted+","+(int)JobStatus.Done+","+(int)JobStatus.Rescinded+")";
			}
			DataTable table=Db.GetTable(command); //JobNum, JobVersion, JobPriority, Owner, Expert, JobType, JobStatus, ProjectNum, Description 
			return table;
		}

		///<summary>Sets a job as NeedsApproval and creates a JobEvent.  Set the new owner of the job prior to calling this method.</summary>
		public static void SendForApproval(Job job) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job);
			}
			job.JobStatus=JobStatus.NeedsApproval;
			JobEvent jobEventRecent=JobEvents.GetMostRecent(job.JobNum);
			if(job.IsNew || jobEventRecent==null || jobEventRecent.Owner!=job.Owner || jobEventRecent.JobStatus!=job.JobStatus) {
				JobEvent jobEventCur=new JobEvent();
				jobEventCur.Description=job.Description;
				jobEventCur.JobNum=job.JobNum;
				jobEventCur.JobStatus=job.JobStatus;
				jobEventCur.Owner=job.Owner;
				JobEvents.Insert(jobEventCur);
			}
			Jobs.Update(job);
			Signalods.SetInvalid(InvalidType.Job);
		}

		///<summary>Sets a job's status and creates a JobEvent.  Does not set a new owner of the job.</summary>
		public static void SetStatus(Job job,JobStatus jobStatus) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job,jobStatus);
			}
			job.JobStatus=jobStatus;
			JobEvent jobEventRecent=JobEvents.GetMostRecent(job.JobNum);
			if(job.IsNew || jobEventRecent==null || jobEventRecent.Owner!=job.Owner || jobEventRecent.JobStatus!=job.JobStatus) {
				JobEvent jobEventCur=new JobEvent();
				jobEventCur.Description=job.Description;
				jobEventCur.JobNum=job.JobNum;
				jobEventCur.JobStatus=job.JobStatus;
				jobEventCur.Owner=job.Owner;
				JobEvents.Insert(jobEventCur);
			}
			Jobs.Update(job);
			Signalods.SetInvalid(InvalidType.Job);
		}

	}
}