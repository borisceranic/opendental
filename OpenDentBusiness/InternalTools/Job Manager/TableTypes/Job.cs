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
		///<summary>FK to customers' userod.UserNum.</summary>
		public long Expert;
		///<summary>FK to project.ProjectNum.</summary>
		public long ProjectNum;
		///<summary>The priority of the job.</summmary>
		public JobPriority JobPriority;
		///<summary>The type of the job.</summary>
		public JobType JobType;
		///<summary>The version the job is for.</summary>
		public string JobVersion;
		///<summary>The estimated hours a job will take.</summary>
		public int HoursEstimate;
		///<summary>The actual hours a job took.</summary>
		public int HoursActual;
		///<summary>The date/time that the job was created.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>The description of the job.</summary>
		public string Description;

		///<summary></summary>
		public Job Copy() {
			return (Job)this.MemberwiseClone();
		}
	}



	public enum JobPriority {
		///<summary>0 -</summary>
		High,
		///<summary>1 -</summary>
		Medium,
		///<summary>2 -</summary>
		Low
	}

	public enum JobType {
		///<summary>0 -</summary>
		Bridge,
		///<summary>1 -</summary>
		Feature,
		///<summary>2 -</summary>
		Enhancement,
		///<summary>3 -</summary>
		Bug,
		///<summary>4 -</summary>
		Query
	}

}




/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS job";
					Db.NonQ(command);
					command=@"CREATE TABLE job (
						JobNum bigint NOT NULL auto_increment PRIMARY KEY,
						Expert bigint NOT NULL,
						ProjectNum bigint NOT NULL,
						JobPriority tinyint NOT NULL,
						JobType tinyint NOT NULL,
						JobVersion varchar(255) NOT NULL,
						HoursEstimate int NOT NULL,
						HoursActual int NOT NULL,
						DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						Description varchar(255) NOT NULL,
						INDEX(Expert),
						INDEX(ProjectNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE job'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE job (
						JobNum number(20) NOT NULL,
						Expert number(20) NOT NULL,
						ProjectNum number(20) NOT NULL,
						JobPriority number(3) NOT NULL,
						JobType number(3) NOT NULL,
						JobVersion varchar2(255),
						HoursEstimate number(11) NOT NULL,
						HoursActual number(11) NOT NULL,
						DateTimeEntry date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						Description varchar2(255),
						CONSTRAINT job_JobNum PRIMARY KEY (JobNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX job_Expert ON job (Expert)";
					Db.NonQ(command);
					command=@"CREATE INDEX job_ProjectNum ON job (ProjectNum)";
					Db.NonQ(command);
				}
				*/