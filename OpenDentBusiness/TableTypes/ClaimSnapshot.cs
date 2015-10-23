using System;

namespace OpenDentBusiness {
	///<summary>Stores the original insurance writeoff, fee, and expected insurance payment information on claims.</summary>
	[Serializable()]
	public class ClaimSnapshot:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ClaimSnapshotNum;
		///<summary>FK to procedurelog.ProcNum</summary>
		public long ProcNum;
		///<summary>"S" or "P" only.</summary>
		public string ClaimType;
		///<summary></summary>
		public double Writeoff;
		///<summary>Expected amount the insurance will pay on the procedure.</summary>
		public double InsPayEst;
		///<summary>Procedure's ProcFee</summary>
		public double Fee;
		///<summary>The date/time that the snapshot was created.  Not user editable.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTEntry;

		public ClaimSnapshot Copy(){
			return (ClaimSnapshot)this.MemberwiseClone();
		}
	}
	
}
