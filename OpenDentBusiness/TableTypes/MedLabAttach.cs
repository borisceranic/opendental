using System;

namespace OpenDentBusiness {
	///<summary>Links a MedLab to a place of service.  This prevents duplicating the laboratory information for every lab order and allows for a
	///single lab order to point to multiple places of service.  Every MedLab will have 1 to many laboratories attached.</summary>
	[Serializable]
	public class MedLabAttach:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long MedLabAttachNum;
		///<summary>FK to medlab.MedLabNum.</summary>
		public long MedLabNum;
		///<summary>FK to medlabfacility.MedLabFacilityNum.</summary>
		public long MedLabFacilityNum;

		///<summary></summary>
		public MedLabAttach Copy() {
			return (MedLabAttach)MemberwiseClone();
		}

	}

}