using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobLink:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobLinknum;
		///<summary>FK to job.JobNum.</summary>
		public long JobNum;
		///<summary>FK to table primary key based on LinkType.</summary>
		public long FKey;
		///<summary>Type of table this links to.</summary>
		public JobLinkType LinkType;

		///<summary></summary>
		public JobLink Copy() {
			return (JobLink)this.MemberwiseClone();
		}
	}

	public enum JobLinkType {
		///<summary>0 -</summary>
		Task,
		///<summary>1 -</summary>
		Request,
		///<summary>2 -</summary>
		Bug,
		///<summary>3 -</summary>
		Review,
		///<summary>4 -</summary>
		QueryRequest
	}

}
	
/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS joblink";
					Db.NonQ(command);
					command=@"CREATE TABLE joblink (
						JobLinknum bigint NOT NULL auto_increment PRIMARY KEY,
						JobNum bigint NOT NULL,
						FKey bigint NOT NULL,
						LinkType tinyint NOT NULL,
						INDEX(JobNum),
						INDEX(FKey)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE joblink'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE joblink (
						JobLinknum number(20) NOT NULL,
						JobNum number(20) NOT NULL,
						FKey number(20) NOT NULL,
						LinkType number(3) NOT NULL,
						CONSTRAINT joblink_JobLinknum PRIMARY KEY (JobLinknum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX joblink_JobNum ON joblink (JobNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX joblink_FKey ON joblink (FKey)";
					Db.NonQ(command);
				}
				*/
