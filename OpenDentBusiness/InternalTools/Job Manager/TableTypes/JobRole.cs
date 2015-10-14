using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true,IsSynchable=true)]
	public class JobRole:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobRoleNum;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNum;
		///<summary>Enum:JobRoleType The role type that this user has.</summary>
		public JobRoleType RoleType;

		///<summary></summary>
		public JobRole Copy() {
			return (JobRole)this.MemberwiseClone();
		}
	}

	///<summary></summary>
	public enum JobRoleType {
		///<summary>0 -</summary>
		Writeup,
		///<summary>1 -</summary>
		Assignment,
		///<summary>2 -</summary>
		Approval,
		///<summary>3 -</summary>
		Documentation,
		///<summary>4 -</summary>
		Review,
		///<summary>5 -</summary>
		Engineer,
		///<summary>6 -</summary>
		Concept,
		///<summary>7 -</summary>
		QueryManager,
		///<summary>8 -</summary>
		FeatureManager,
		///<summary>9 -</summary>
		NotifyCustomer,
		///<summary>10 -</summary>
		Quote,
		///<summary>11 -</summary>
		Override

	}

}

/*				command="DROP TABLE IF EXISTS jobrole";
					Db.NonQ(command);
					command=@"CREATE TABLE jobrole (
						JobRoleNum bigint NOT NULL auto_increment PRIMARY KEY,
						UserNum bigint NOT NULL,
						RoleType tinyint NOT NULL,
						INDEX(UserNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);				*/
