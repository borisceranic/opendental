using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class StmtAdjAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long StmtAdjAttachNum;
		///<summary>FK to statement.StatementNum.</summary>
		public long StatementNum;
		///<summary>FK to adjustment.AdjNum.</summary>
		public long AdjNum;

		public StmtAdjAttach Copy() {
			return (StmtAdjAttach)this.MemberwiseClone();
		}
	}

}
