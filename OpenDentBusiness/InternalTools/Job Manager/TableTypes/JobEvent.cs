﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  
	///All schema changes are done directly on our live database as needed.
	///This table is used to provide a history of a job based on when the status has changed.  
	///It is also used to display the current status of the job and the description. 
	///These will be created when the Owner or the Status change.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobEvent:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobEventNum;
		///<summary>FK to job.JobNum.  Links this event to the source job.</summary>
		public long JobNum;
		///<summary>FK to customers' userod.UserNum.  Shows in the JobEdit window.</summary>
		public long Owner;
		///<summary>Date/Time the event was created.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>Copy of the job description at the time of the event creation.</summary>
		public string Description;
		///<summary>The status of the referenced job.</summary>
		public JobStatus JobReviewStatus;

		///<summary></summary>
		public JobEvent Copy() {
			return (JobEvent)this.MemberwiseClone();
		}
	}

	public enum JobStatus {
		///<summary>0 -</summary>
		Concept,
		///<summary>1 -</summary>
		NeedsExpertDefinition,
		///<summary>2 -</summary>
		UnderConstruction,
		///<summary>3 -</summary>
		NeedsApproval,
		///<summary>4 -</summary>
		ReadyToAssign,
		///<summary>5 -</summary>
		Assigned,
		///<summary>6 -</summary>
		InProgress,
		///<summary>7 -</summary>
		OnHold,
		///<summary>8 -</summary>
		Rescinded,
		///<summary>9 -</summary>
		ReadyForReview,
		///<summary>10 -</summary>
		Done,
		///<summary>11 -</summary>
		ReadyToBeDocumented,
		///<summary>12 -</summary>
		Documented,
		///<summary>13 -</summary>
		QuestionForManager,
		///<summary>14 -</summary>
		Deleted,
		///<summary>15 -</summary>
		QuestionForEngineers
	}

}



/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS jobevent";
					Db.NonQ(command);
					command=@"CREATE TABLE jobevent (
						JobEventNum bigint NOT NULL auto_increment PRIMARY KEY,
						JobNum bigint NOT NULL,
						Owner bigint NOT NULL,
						DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						Description varchar(255) NOT NULL,
						JobReviewStatus tinyint NOT NULL,
						INDEX(JobNum),
						INDEX(Owner)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE jobevent'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE jobevent (
						JobEventNum number(20) NOT NULL,
						JobNum number(20) NOT NULL,
						Owner number(20) NOT NULL,
						DateTimeEntry date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						Description varchar2(255),
						JobReviewStatus number(3) NOT NULL,
						CONSTRAINT jobevent_JobEventNum PRIMARY KEY (JobEventNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX jobevent_JobNum ON jobevent (JobNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX jobevent_Owner ON jobevent (Owner)";
					Db.NonQ(command);
				}
				*/