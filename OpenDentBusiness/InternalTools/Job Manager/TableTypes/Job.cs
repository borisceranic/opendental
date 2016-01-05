using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.
	/// Base object for use in the job tracking system.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class Job:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobNum;
		///<summary>FK to userod.UserNum.</summary>
		public long ExpertNum;
		///<summary>FK to userod.UserNum.  The current owner of the job.  Historical owner data stored in JobEvent.Owner.</summary>
		public long OwnerNum;
		///<summary>FK to job.JobNum.</summary>
		public long ParentNum;
		///<summary>The priority of the job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public JobPriority Priority;
		///<summary>The type of the job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public JobCategory Category;
		///<summary>The version the job is for.</summary>
		public string JobVersion;
		///<summary>The estimated hours a job will take.</summary>
		public int HoursEstimate;
		///<summary>The actual hours a job took.</summary>
		public int HoursActual;
		///<summary>The date/time that the job was created.  Not user editable.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>The description of the job. RTF content of the main body of the Job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]//Text
		public string Description;
		///<summary>The short title of the job.</summary>
		public string Title;
		///<summary>The current status of the job.  Historical statuses for this job can be found in the jobevent table.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public JobStat JobStatus;

		//The following varables should be filled by the class that uses them. Not filled in S class. 
		//Just a convenient way to package a job for passing around in the job manager.
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobLink> ListJobLinks=new List<JobLink>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobNote> ListJobNotes=new List<JobNote>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobReview> ListJobReviews=new List<JobReview>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobQuote> ListJobQuotes=new List<JobQuote>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobEvent> ListJobEvents=new List<JobEvent>();

		public Job() {
			JobVersion="";
			Description="";
			Title="";
		}

		///<summary></summary>
		public Job Copy() {
			Job job=(Job)this.MemberwiseClone();
			job.ListJobLinks=this.ListJobLinks.Select(x => x.Copy()).ToList();
			job.ListJobNotes=this.ListJobNotes.Select(x => x.Copy()).ToList();
			job.ListJobReviews=this.ListJobReviews.Select(x => x.Copy()).ToList();
			job.ListJobQuotes=this.ListJobQuotes.Select(x => x.Copy()).ToList();
			job.ListJobEvents=this.ListJobEvents.Select(x => x.Copy()).ToList();
			return job;
		}

		///<summary>Used primarily to display a Job in the tree view.</summary>
		public override string ToString() {
			return Category.ToString().Substring(0,1)+JobNum+" - "+Title;
		}
	}


	public enum JobStat {
		///<summary>0 -</summary>
		Concept,
		///<summary>1 -</summary>
		ConceptApproved,
		///<summary>2 -</summary>
		CurrentlyWriting,
		///<summary>3 -</summary>
		NeedsConceptApproval,
		///<summary>4 -</summary>
		JobApproved,
		///<summary>5 -</summary>
		Assigned,
		///<summary>6 -</summary>
		CurrentlyWorkingOn,
		///<summary>7 -</summary>
		OnHoldExpert,
		///<summary>8 -</summary>
		Rescinded,
		///<summary>9 -</summary>
		ReadyForReview,
		///<summary>10 -</summary>
		Complete,
		///<summary>11 -</summary>
		ReadyToBeDocumented,
		///<summary>12 -</summary>
		NotifyCustomer,
		///<summary>13 -</summary>
		Deleted,
		///<summary>14 -</summary>
		NeedsDocumentationClarification,
		///<summary>15 -</summary>
		NeedsJobApproval,
		///<summary>16 -</summary>
		OnHoldEngineer,
		///<summary>17 -</summary>
		ReadyToAssign,
		///<summary>18 -</summary>
		NeedsJobClarification
	}

	public enum JobPriority {
		///<summary>0 -</summary>
		High,
		///<summary>1 -</summary>
		Medium,
		///<summary>2 -</summary>
		Low
	}

	public enum JobCategory {
		///<summary>0 -</summary>
		Feature,
		///<summary>1 -</summary>
		Bug,
		///<summary>2 -</summary>
		Enhancement,
		///<summary>3 -</summary>
		Query,
		///<summary>4 -</summary>
		ProgramBridge
	}

}


