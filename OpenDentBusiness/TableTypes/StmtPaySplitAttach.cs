using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class StmtPaySplitAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long StmtPaySplitAttachNum;
		///<summary>FK to statement.StatementNum.</summary>
		public long StatementNum;
		///<summary>FK to paysplit.PaySplitNum.</summary>
		public long PaySplitNum;

		public StmtPaySplitAttach Copy() {
			return (StmtPaySplitAttach)this.MemberwiseClone();
		}
	}

}
