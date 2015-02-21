using System;

namespace OpenDentBusiness {
	///<summary>LabCorp lab specimen.  Contains data from the SPM segment.  Each LabCorpLab object can have 0 to many specimen segments.
	///Each segment will be its own row in this table.</summary>
	[Serializable]
	public class LabCorpSpecimen:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long LabCorpSpecimenNum;
		///<summary>FK to labcorplab.LabCorpLabNum.  Each LabCorpLab object can have 0 to many specimens pointing to it.</summary>
		public long LabCorpLabNum;
		///<summary>SPM.2 - Specimen ID.  Unique identifier for the specimen as referenced by the Placer application, the Filler application, or both.
		///The value sent in this field should be the identification value sent on the specimen container.</summary>
		public string SpecimenID;
		///<summary>SPM.14 - Specimen Description.  Additional information about the specimen.</summary>
		public string SpecimenDescript;
		///<summary>SPM.17 - Specimen Collection Date/Time.  yyyyMMddHHmm format in the message, no seconds.  The date and time when the specimen was
		///acquired from the source.  This is a DR - Date/Time Range data type, so it may have more than one component if a specimen was collected over
		///a period of time.  The first component is the start date/time so we will make sure to only store SPM.17.1 in this field.</summary>
		public DateTime DateTimeCollected;

		///<summary></summary>
		public LabCorpSpecimen Copy() {
			return (LabCorpSpecimen)MemberwiseClone();
		}

	}

}