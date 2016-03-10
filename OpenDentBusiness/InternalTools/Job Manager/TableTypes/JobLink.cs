using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true,IsSynchable=true)]
	//[CrudTable(IsSynchable=true)]
	public class JobLink:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobLinkNum;
		///<summary>FK to job.JobNum.</summary>
		public long JobNum;
		///<summary>FK to table primary key based on LinkType.</summary>
		public long FKey;
		///<summary>Type of table this links to and what role the objects on that table are.</summary>
		public JobLinkType LinkType;
		///<summary>Contains other information such as a unc path to a file.</summary>
		public string Tag;

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
		QueryRequest,
		///<summary>4 -</summary>
		Watcher,
		///<summary>5 -</summary>
		File
	}

}


// ALTER TABLE joblink ADD Tag varchar(255) NOT NULL