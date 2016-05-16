using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class StmtProcAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long StmtProcAttachNum;
		///<summary>FK to statement.StatementNum.</summary>
		public long StatementNum;
		///<summary>FK to procedurelog.ProcNum.</summary>
		public long ProcNum;

		public StmtProcAttach Copy() {
			return (StmtProcAttach)this.MemberwiseClone();
		}
	}

}
