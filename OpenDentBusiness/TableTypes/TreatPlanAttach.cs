using System;

namespace OpenDentBusiness{

	///<summary>A treatment plan saved by a user.  Does not include the default tp, which is just a list of procedurelog entries with a status of tp.  A treatplan has many proctp's attached to it.</summary>
	[Serializable]
	[CrudTable(IsSynchable=true)]
	public class TreatPlanAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long TreatPlanAttachNum;
		///<summary>FK to treatplan.TreatPlanNum.</summary>
		public long TreatPlanNum;
		///<summary>FK to procedurelog.ProcNum.</summary>
		public long ProcNum;
		///<summary>FK to definition.DefNum, which contains the text of the priority. Identical to Procedure.Priority but used to allow different priorities
		/// for the same procedure depending on which TP it is a part of.</summary>
		public long Priority;

		///<summary></summary>
		public TreatPlanAttach Copy() {
			return (TreatPlanAttach)this.MemberwiseClone();
		}

	}

	

	


}