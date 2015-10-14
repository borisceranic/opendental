using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobProject:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobProjectNum;
		///<summary>FK to project.ProjectNum.  Links this project to the source project.</summary>
		public long RootProjectNum;
		///<summary>FK to project.ProjectNum.  Links this project to its parent project.</summary>
		public long ParentProjectNum;
		///<summary>The title of the project.</summary>
		public string Title;
		///<summary>A description of the project.</summary>
		public string Description;
		///<summary>The status of this project.</summary>
		public JobProjectStatus ProjectStatus;

		///<summary></summary>
		public JobProject Copy() {
			return (JobProject)this.MemberwiseClone();
		}
	}

	public enum JobProjectStatus {
		///<summary>0 -</summary>
		Design,
		///<summary>1 -</summary>
		InProgress,
		///<summary>2 -</summary>
		OnHold,
		///<summary>3 -</summary>
		Done
	}

}


/*
				command="DROP TABLE IF EXISTS jobproject";
				Db.NonQ(command);
				command=@"CREATE TABLE jobproject (
					JobProjectNum bigint NOT NULL auto_increment PRIMARY KEY,
					RootProjectNum bigint NOT NULL,
					ParentProjectNum bigint NOT NULL,
					Title varchar(255) NOT NULL,
					Description varchar(255) NOT NULL,
					ProjectStatus tinyint NOT NULL,
					INDEX(RootProjectNum),
					INDEX(ParentProjectNum)
					) DEFAULT CHARSET=utf8";
				Db.NonQ(command);
			*/