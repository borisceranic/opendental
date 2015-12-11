using System;
using System.Collections.Generic;
using System.Data;
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
				command+=" AND Status != " + (int)JobStatus.Done;
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
				command+=" AND Status NOT IN ("+(int)JobStatus.Deleted+","+(int)JobStatus.Done+","+(int)JobStatus.Rescinded+")";
			}
			return Crud.JobCrud.SelectMany(command);
		}

		///<summary>Sets a job's status and creates a JobEvent.  Does not set a new owner of the job.</summary>
		public static void SetStatus(Job job,JobStatus jobStatus,long jobOwner) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),job,jobStatus);
			}
			if(job.IsNew || job.Owner!=jobOwner || job.Status!=jobStatus) {
				JobEvent jobEventCur=new JobEvent();
				jobEventCur.Description=job.Description;
				jobEventCur.JobNum=job.JobNum;
				jobEventCur.Status=job.Status;
				jobEventCur.Owner=job.Owner;
				JobEvents.Insert(jobEventCur);
			}
			job.Status=jobStatus;
			job.Owner=jobOwner;
			Jobs.Update(job);
		}

		///<summary>Returns a data table for the Job Manager control.  This data table will be optionally grouped by the booleans passed in and
		///will be filtered to include entries based on the lists passed in.</summary>
		public static DataTable GetForJobManager(List<string> listExpertNums,List<string> listOwnerNums,List<string> listJobStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listExpertNums,listOwnerNums,listJobStatuses);
			}
			string command="SELECT * FROM job ";
			string whereClause="";
			if(listExpertNums.Count>0) {//There are specific experts
				whereClause+="Expert IN("+String.Join(",",listExpertNums)+") ";
			}
			if(listOwnerNums.Count>0) {//There are specific owners
				if(whereClause!="") {
					whereClause+="AND ";
				}
				whereClause+="Owner IN("+String.Join(",",listOwnerNums)+") ";
			}
			if(listJobStatuses.Count>0) {//There are specific statuses
				if(whereClause!="") {
					whereClause+="AND ";
				}
				whereClause+="Status IN("+String.Join(",",listJobStatuses)+") ";
			}
			if(whereClause!="") {
				whereClause="WHERE "+whereClause;
			}
			command+=whereClause;
			return Db.GetTable(command);
		}

		///<summary>Returns a data table for the Job Manager control.  This data table will be relevant to the currently logged in user.</summary>
		public static DataTable GetMyJobsTable(bool showCreated,bool showCompleted) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),showCreated,showCompleted);
			}
			string command="SELECT job.JobNum,job.Priority,job.DateTimeEntry,job.Expert,job.Owner,job.Status,job.Category"
				+",job.Title,job.HoursEstimate,mainevent.Owner EventOwner,jobproject.Title ProjectTitle "
				+"FROM job "
				+"LEFT JOIN jobproject ON job.ProjectNum=jobproject.JobProjectNum "
				+"LEFT JOIN jobevent mainevent ON job.JobNum=mainevent.JobNum "
					+"AND mainevent.DateTimeEntry=(SELECT MIN(DateTimeEntry) FROM jobevent subevent WHERE subevent.JobEventNum=mainevent.JobEventNum) "
				+"WHERE (job.Owner="+POut.Long(Security.CurUser.UserNum)+" "
					+"OR job.Expert="+POut.Long(Security.CurUser.UserNum);
			if(showCreated) {
				command+=" OR mainevent.Owner="+POut.Long(Security.CurUser.UserNum);
			}
			command+=") ";
			if(showCompleted) {
				command+="AND job.Status NOT IN("+POut.Int((int)JobStatus.Done)+","+POut.Int((int)JobStatus.Deleted)+") ";
			}
			command+="GROUP BY job.JobNum "
				+"ORDER BY job.Priority,job.DateTimeEntry";
			DataTable jobInfoTable=Db.GetTable(command);
			DataTable returnTable=new DataTable();
			returnTable.TableName="table";
			returnTable.Columns.Add("JobNum");
			returnTable.Columns.Add("priority");
			returnTable.Columns.Add("date");
			returnTable.Columns.Add("expert");
			returnTable.Columns.Add("owner");
			returnTable.Columns.Add("status");
			returnTable.Columns.Add("category");
			returnTable.Columns.Add("Title");
			returnTable.Columns.Add("Project");
			returnTable.Columns.Add("originator");
			returnTable.Columns.Add("EstimatedHours");
			//Sets the primary key for use in tagging. This must be the JobNum since the grid is tagged with the JobNum in the UI.
			returnTable.PrimaryKey=new DataColumn[] { returnTable.Columns["JobNum"] };
			DataRow r;
			foreach(DataRow dRow in jobInfoTable.Rows) {
				r=returnTable.NewRow();
				r["JobNum"]=dRow["JobNum"].ToString();
				r["priority"]=Enum.GetName(typeof(JobPriority),PIn.Int(dRow["Priority"].ToString()));
				r["date"]=PIn.DateT(dRow["DateTimeEntry"].ToString()).ToShortDateString();
				r["expert"]=Userods.GetName(PIn.Long(dRow["Expert"].ToString()));
				r["owner"]=Userods.GetName(PIn.Long(dRow["Owner"].ToString()));
				r["status"]=Enum.GetName(typeof(JobStatus),PIn.Int(dRow["Status"].ToString()));
				r["category"]=Enum.GetName(typeof(JobCategory),PIn.Int(dRow["Category"].ToString()));
				r["Title"]=dRow["Title"].ToString();
				r["Project"]=dRow["ProjectTitle"].ToString();
				r["originator"]=Userods.GetName(PIn.Long(dRow["EventOwner"].ToString()));
				r["EstimatedHours"]=dRow["HoursEstimate"].ToString();
				returnTable.Rows.Add(r);
			}
			return returnTable;
		}

		public static DataTable GetSummaryForOwner(long ownerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),ownerNum);
			}
			string command="SELECT SUM(HoursEstimate) AS 'numEstHours', COUNT(DISTINCT JobNum) AS 'numJobs' FROM job "
				+"WHERE Owner="+POut.Long(ownerNum)+" AND Status IN("
				+POut.Long((int)JobStatus.Assigned)
				+","+POut.Long((int)JobStatus.InProgress)
				+","+POut.Long((int)JobStatus.ReadyForReview)
				+","+POut.Long((int)JobStatus.ReadyToAssign)
				+","+POut.Long((int)JobStatus.OnHold)+")";
			return Db.GetTable(command);
		}

		public static DataTable GetSummaryForExpert(long expertNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),expertNum);
			}
			string command="SELECT SUM(HoursEstimate) AS 'numEstHours', COUNT(DISTINCT JobNum) AS 'numJobs' FROM job "
				+"WHERE Expert="+POut.Long(expertNum)+" AND Status IN("
				+POut.Long((int)JobStatus.Assigned)
				+","+POut.Long((int)JobStatus.InProgress)
				+","+POut.Long((int)JobStatus.ReadyForReview)
				+","+POut.Long((int)JobStatus.ReadyToAssign)
				+","+POut.Long((int)JobStatus.OnHold)+")";
			return Db.GetTable(command);
		}

	}
}