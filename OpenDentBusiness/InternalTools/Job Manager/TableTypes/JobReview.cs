using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobReview:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobReviewNum;
		///<summary>FK to customers' userod.UserNum.  Links this project to the source project.</summary>
		public long Reviewer;
		///<summary>Date/Time the review was created.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		///<summary>The text in this review.</summary>
		public string Description;
		///<summary>The status of this review.</summary>
		public JobReviewStatus JobReviewStatus;

		///<summary></summary>
		public JobReview Copy() {
			return (JobReview)this.MemberwiseClone();
		}
	}

	public enum JobReviewStatus {
		///<summary>0 -</summary>
		Sent,
		///<summary>1 -</summary>
		Seen,
		///<summary>2 -</summary>
		UnderReview,
		///<summary>3 -</summary>
		NeedsAdditionalWork,
		///<summary>4 -</summary>
		Done
	}

}




	/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS jobreview";
					Db.NonQ(command);
					command=@"CREATE TABLE jobreview (
						JobReviewNum bigint NOT NULL auto_increment PRIMARY KEY,
						Reviewer bigint NOT NULL,
						DateTStamp timestamp,
						Description varchar(255) NOT NULL,
						JobReviewStatus tinyint NOT NULL,
						INDEX(Reviewer)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE jobreview'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE jobreview (
						JobReviewNum number(20) NOT NULL,
						Reviewer number(20) NOT NULL,
						DateTStamp timestamp,
						Description varchar2(255),
						JobReviewStatus number(3) NOT NULL,
						CONSTRAINT jobreview_JobReviewNum PRIMARY KEY (JobReviewNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX jobreview_Reviewer ON jobreview (Reviewer)";
					Db.NonQ(command);
				}
				*/