using System;

namespace OpenDentBusiness {
	///<summary>Links a LabCorpLab to a place of service.  This prevents duplicating the laboratory information for every lab order and allows for a
	///single lab order to point to multiple places of service.  Every LabCorpLab will have 1 to many laboratories attached.</summary>
	[Serializable]
	public class LabCorpLabAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long LabCorpLabAttachNum;
		///<summary>FK to labcorplab.LabCorpLabNum.</summary>
		public long LabCorpLabNum;
		///<summary>FK to labcorpfacility.LabCorpFacilityNum.</summary>
		public long LabCorpFacilityNum;

		///<summary></summary>
		public LabCorpLabAttach Copy() {
			return (LabCorpLabAttach)MemberwiseClone();
		}

	}

}