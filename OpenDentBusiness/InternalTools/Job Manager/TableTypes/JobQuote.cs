using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobQuote:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobQuoteNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary></summary>
		public string Amount;
		///<summary></summary>
		public string Note;

		///<summary></summary>
		public JobQuote Copy() {
			return (JobQuote)this.MemberwiseClone();
		}
	}
}

				/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS jobquote";
					Db.NonQ(command);
					command=@"CREATE TABLE jobquote (
						JobQuoteNum bigint NOT NULL auto_increment PRIMARY KEY,
						PatNum bigint NOT NULL,
						Amount varchar(255) NOT NULL,
						Note varchar(255) NOT NULL,
						INDEX(PatNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				*/