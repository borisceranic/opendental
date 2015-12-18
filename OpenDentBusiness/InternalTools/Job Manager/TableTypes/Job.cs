using System;
using System.Collections.Generic;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class Job:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobNum;
		///<summary>FK to userod.UserNum.</summary>
		public long Expert;
		///<summary>FK to project.ProjectNum.</summary>
		public long ProjectNum;
		///<summary>The priority of the job.</summmary>
		public JobPriority Priority;
		///<summary>The type of the job.</summary>
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
		///<summary>The description of the job.</summary>
		public string Description;
		///<summary>The short title of the job.</summary>
		public string Title;
		///<summary>The current status of the job.  Historical statuses for this job can be found in the jobevent table.</summary>
		public JobStatus Status;
		///<summary>FK to userod.UserNum.  The current owner of the job.  Historical owner data stored in JobEvent.Owner.</summary>
		public long Owner;

		///<summary></summary>
		public Job Copy() {
			return (Job)this.MemberwiseClone();
		}
	}


	public enum JobStatus {
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
		///<summary>14 -</summary>
		Deleted,
		///<summary>15 -</summary>
		NeedsClarification,
		///<summary>16 -</summary>
		NeedsJobApproval,
		///<summary>17 -</summary>
		OnHoldEngineer,
		///<summary>18 -</summary>
		ReadyToAssign
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
		Bridge
	}

}




/*
					command="DROP TABLE IF EXISTS job";
					Db.NonQ(command);
					command=@"CREATE TABLE job (
						JobNum bigint NOT NULL auto_increment PRIMARY KEY,
						Expert bigint NOT NULL,
						ProjectNum bigint NOT NULL,
						Priority tinyint NOT NULL,
						Category tinyint NOT NULL,
						JobVersion varchar(255) NOT NULL,
						HoursEstimate int NOT NULL,
						HoursActual int NOT NULL,
						DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						Description varchar(255) NOT NULL,
						Title varchar(255) NOT NULL,
						Status tinyint NOT NULL,
						Owner bigint NOT NULL,
						INDEX(Expert),
						INDEX(ProjectNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				*/

