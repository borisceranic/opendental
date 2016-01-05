using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class Jobs {

		///<summary></summary>
		public static List<Job> GetForExpert(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM job WHERE Expert = "+POut.Long(userNum);
			return Crud.JobCrud.SelectMany(command);
		}

		public static List<Job> GetForProject(long projectNum,bool showFinished) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),projectNum,showFinished);
			}
			string command="SELECT * FROM job WHERE ProjectNum = "+POut.Long(projectNum);
			if(!showFinished) {
				command+=" AND Status != " + (int)JobStat.Complete;
			}
			return Crud.JobCrud.SelectMany(command);
		}

		///<summary>Gets one Job from the db.</summary>
		public static Job GetOne(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Job>(MethodBase.GetCurrentMethod(),jobNum);
			}
			return Crud.JobCrud.SelectOne(jobNum);
		}

		///<summary></summary>
		public static long Insert(Job job) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				job.JobNum=Meth.GetLong(MethodBase.GetCurrentMethod(),job);
				return job.JobNum;
			}
			return Crud.JobCrud.Insert(job);
		}

		///<summary></summary>
		public static void Update(Job job) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job);
				return;
			}
			Crud.JobCrud.Update(job);
		}

		///<summary>You must surround with a try-catch when calling this method.  Deletes one job from the database.  
		///Also deletes all JobLinks, Job Events, and Job Notes associated with the job.  Jobs that have reviews or quotes on them may not be deleted and will throw an exception.</summary>
		public static void Delete(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			if(JobReviews.GetForJob(jobNum).Count>0 || JobQuotes.GetForJob(jobNum).Count>0) {
				throw new Exception(Lans.g("Jobs","Not allowed to delete a job that has attached reviews or quotes.  Set the status to deleted instead."));//The exception is caught in FormJobEdit.
			}
			//JobReviews.DeleteForJob(jobNum);//do not delete, blocked above
			//JobQuotes.DeleteForJob(jobNum);//do not delete, blocked above
			JobLinks.DeleteForJob(jobNum);
			JobEvents.DeleteForJob(jobNum);
			JobNotes.DeleteForJob(jobNum);
			Crud.JobCrud.Delete(jobNum); //Finally, delete the job itself.
		}

		///<summary>Returns a list for use in UserControlJobs, filtered by the passed in params. 
		///String params can be "", JobNum can be 0, and other long params can be -1 if you do not want to filter by those params.</summary>
		public static List<Job> GetJobList(long jobNum,string expert,string owner,string version,
			string project,string title,long status,long priority,long category,bool showHidden) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod(),jobNum,expert,owner,version,project,title,status,priority,category,showHidden);
			}
			string command="SELECT job.*"
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
			if(version!="") {
				command+=" AND JobVersion LIKE '%"+version+"%'";
			}
			if(title!="") {
				command+=" AND Title LIKE '%"+title+"%'";
			}
			if(jobNum!=0) {
				command+=" AND JobNum="+jobNum;
			}
			if(status>-1) {
				command+= " AND Status="+status;
			}
			if(priority>-1) {
				command+=" AND Priority="+priority;
			}
			if(category>-1) {
				command+=" AND Category="+category;
			}
			if(!showHidden) {
				command+=" AND Status NOT IN ("+(int)JobStat.Deleted+","+(int)JobStat.Complete+","+(int)JobStat.Rescinded+")";
			}
			return Crud.JobCrud.SelectMany(command);
		}

		///<summary>Sets a job's status and creates a JobEvent.  Does not set a new owner of the job.</summary>
		public static void SetStatus(Job job,JobStat jobStatus,long jobOwnerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job,jobStatus);
			}
			if(job.IsNew || job.OwnerNum!=jobOwnerNum || job.JobStatus!=jobStatus) {
				JobEvent jobEventCur=new JobEvent();
				jobEventCur.Description=job.Description;
				jobEventCur.JobNum=job.JobNum;
				jobEventCur.JobStatus=job.JobStatus;
				jobEventCur.OwnerNum=job.OwnerNum;
				JobEvents.Insert(jobEventCur);
			}
			job.JobStatus=jobStatus;
			job.OwnerNum=jobOwnerNum;
			Jobs.Update(job);
		}

		///<summary>Returns a data table for the Job Manager control.  This data table will be optionally grouped by the booleans passed in and
		///will be filtered to include entries based on the lists passed in.</summary>
		public static DataTable GetForJobManager(List<string> listExpertNums,List<string> listOwnerNums,List<string> listJobStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listExpertNums,listOwnerNums,listJobStatuses);
			}
			string command="SELECT * FROM job ";
			List<string> listWhereClauses=new List<string>();
			if(listExpertNums.Count>0) {//There are specific experts
				listWhereClauses.Add("Expert IN("+String.Join(",",listExpertNums)+")");
			}
			if(listOwnerNums.Count>0) {//There are specific owners
				listWhereClauses.Add("Owner IN("+String.Join(",",listOwnerNums)+")");
			}
			if(listJobStatuses.Count>0) {//There are specific statuses
				listWhereClauses.Add("Status IN("+String.Join(",",listJobStatuses)+")");
			}
			if(listWhereClauses.Count>0) {
				command+="WHERE "+string.Join(" AND ",listWhereClauses);
			}
			return Db.GetTable(command);
		}

		public static DataTable GetSummaryForOwner(long ownerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),ownerNum);
			}
			string command="SELECT SUM(HoursEstimate) AS 'numEstHours', COUNT(DISTINCT JobNum) AS 'numJobs' FROM job "
				+"WHERE Owner="+POut.Long(ownerNum)+" AND Status IN("
				+POut.Long((int)JobStat.Assigned)
				+","+POut.Long((int)JobStat.CurrentlyWorkingOn)
				+","+POut.Long((int)JobStat.ReadyForReview)
				+","+POut.Long((int)JobStat.ReadyToAssign)
				+","+POut.Long((int)JobStat.OnHoldExpert)+")";
			return Db.GetTable(command);
		}

		public static DataTable GetSummaryForExpert(long expertNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),expertNum);
			}
			string command="SELECT SUM(HoursEstimate) AS 'numEstHours', COUNT(DISTINCT JobNum) AS 'numJobs' FROM job "
				+"WHERE Expert="+POut.Long(expertNum)+" AND Status IN("
				+POut.Long((int)JobStat.Assigned)
				+","+POut.Long((int)JobStat.CurrentlyWorkingOn)
				+","+POut.Long((int)JobStat.ReadyForReview)
				+","+POut.Long((int)JobStat.ReadyToAssign)
				+","+POut.Long((int)JobStat.OnHoldExpert)+")";
			return Db.GetTable(command);
		}

		public static List<Job> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Job>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM job";
			return Crud.JobCrud.SelectMany(command);
		}

		public static bool ValidateJobNum(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT COUNT(*) FROM job WHERE JobNum="+POut.Long(jobNum);
			return Db.GetScalar(command)!="0";
		}

		///<summary>Efficiently queries DB to fill all in memory lists for all jobs passed in.</summary>
		public static void FillInMemoryLists(List<Job> listJobsAll) {
			//No need for remoting call here.
			List<long> jobNums=listJobsAll.Select(x=>x.JobNum).ToList();
			Dictionary<long,List<JobLink>> listJobLinksAll=JobLinks.GetJobLinksForJobs(jobNums).GroupBy(x=>x.JobNum).ToDictionary(x=>x.Key,x=>x.ToList());
			Dictionary<long,List<JobNote>> listJobNotesAll=JobNotes.GetJobNotesForJobs(jobNums).GroupBy(x => x.JobNum).ToDictionary(x => x.Key,x => x.ToList());
			Dictionary<long,List<JobReview>> listJobReviewsAll=JobReviews.GetJobReviewsForJobs(jobNums).GroupBy(x => x.JobNum).ToDictionary(x => x.Key,x => x.ToList());
			Dictionary<long,List<JobQuote>> listJobQuotesAll=JobQuotes.GetJobQuotesForJobs(jobNums).GroupBy(x => x.JobNum).ToDictionary(x => x.Key,x => x.ToList());
			Dictionary<long,List<JobEvent>> listJobEventsAll=JobEvents.GetJobEventsForJobs(jobNums).GroupBy(x => x.JobNum).ToDictionary(x => x.Key,x => x.ToList());
			for(int i=0;i<listJobsAll.Count;i++) {
				Job job=listJobsAll[i];
				if(!listJobLinksAll.TryGetValue(job.JobNum,out job.ListJobLinks)) {
					job.ListJobLinks=new List<JobLink>();//empty list if not found
				}
				if(!listJobNotesAll.TryGetValue(job.JobNum,out job.ListJobNotes)) {
					job.ListJobNotes=new List<JobNote>();//empty list if not found
				}
				if(!listJobReviewsAll.TryGetValue(job.JobNum,out job.ListJobReviews)) {
					job.ListJobReviews=new List<JobReview>();//empty list if not found
				}
				if(!listJobQuotesAll.TryGetValue(job.JobNum,out job.ListJobQuotes)) {
					job.ListJobQuotes=new List<JobQuote>();//empty list if not found
				}
				if(!listJobEventsAll.TryGetValue(job.JobNum,out job.ListJobEvents)) {
					job.ListJobEvents=new List<JobEvent>();//empty list if not found
				}
			}
		}

		///<summary>Must be called after job is filled using Jobs.FillInMemoryLists(). Returns list of user nums associated with this job.
		/// Currently that is Expert, Owner, and Watchers.</summary>
		public static List<long> GetUserNums(Job job) {
			List<long> retVal=new List<long> {
				job.ExpertNum,
				job.OwnerNum
			};
			job.ListJobLinks.FindAll(x=>x.LinkType==JobLinkType.Watcher).ForEach(x=>retVal.Add(x.FKey));
			job.ListJobReviews.ForEach(x=>retVal.Add(x.ReviewerNum));
			return retVal;
		}

		///<summary>Attempts to find infinite loop when changing job parent. Can be optimized to reduce trips to DB since we have all jobs in memory in the job manager.</summary>
		public static bool CheckForLoop(long jobNum,long jobNumParent) {
			List<long> lineage=new List<long>(){jobNum};
			long parentNumNext=jobNumParent;
			while(parentNumNext!=0){
				if(lineage.Contains(parentNumNext)) {
					return true;//loop found
				}
				Job jobNext=Jobs.GetOne(parentNumNext);
				lineage.Add(parentNumNext);
				parentNumNext=jobNext.ParentNum;
			} 
			return false;//no loop detected
		}
	}
}