using System;
using System.Collections;

namespace OpenDentBusiness {

	///<summary>A jobnote is a note that may be added to a job. Many notes may be attached to a job.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class JobNote:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long JobNoteNum;
		///<summary>FK to job.JobNum. The job this jobnote is attached to.</summary>
		public long JobNum;
		///<summary>FK to userod.UserNum. The user who created this jobnote.</summary>
		public long UserNum;
		///<summary>Date and time the note was created (editable).</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntryEditable)]
		public DateTime DateTimeNote;
		///<summary>Note. Text that the user wishes to show on the task.</summary>
		public string Note;

		///<summary></summary>
		public JobNote Copy() {
			return (JobNote)MemberwiseClone();
		}

	}

	/*
			command="DROP TABLE IF EXISTS jobnote";
			Db.NonQ(command);
			command=@"CREATE TABLE jobnote (
				JobNoteNum bigint NOT NULL auto_increment PRIMARY KEY,
				JobNum bigint NOT NULL,
				UserNum bigint NOT NULL,
				DateTimeNote datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				Note varchar(255) NOT NULL,
				INDEX(JobNum),
				INDEX(UserNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
		*/






}













