using System;

namespace OpenDentBusiness {
	///<summary>The EHRLabResult table is structured to tightly with the HL7 standard and should have names that more reflect how the user will
	///consume the data and for that reason for actual implementation we are using these medlab tables.
	///Medical lab result.  This table is currently only used for LabCorp, but may be utilized by other third party lab
	///services in the future.  These fields are required for the LabCorp result report, used to link the result to an order,
	///or for linking a parent and child result.  Contains data from the OBX, ZEF, and applicable NTE segments.</summary>
	[Serializable]
	public class MedLabResult:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long MedLabResultNum;
		///<summary>FK to medlab.medLabNum.  Each MedLab object can have one to many results pointing to it.</summary>
		public long MedLabNum;
		#region OBX Fields
		///<summary>OBX.3.1 - Observation Identifier.  Reflex results will have the ObsID of the parent in OBR.26 for linking.</summary>
		public string ObsID;
		///<summary>OBX.3.2 - Observation Text.  LabCorp report field "TESTS".  LabCorp test name.</summary>
		public string ObsText;
		///<summary>OBX.5.1 - Observation Value.  LabCorp report field "RESULT".  Can be null if coded entries, prelims, canceled, or >21 chars and being
		///returned as an attached NTE.  "TNP" will be reported for Test Not Performed.</summary>
		public string ObsValue;
		///<summary>OBX.6.1 - Identifier.  LabCorp report field "UNITS".  Units of measure, if too large it will be in the NTE segment.</summary>
		public string ObsUnits;
		///<summary>OBX.7 - Reference Ranges.  LabCorp report field "REFERENCE INTERVAL".  Only if applicable.</summary>
		public string ReferenceRange;
		///<summary>OBX.8 - Abnormal Flags.  LabCorp report field "FLAG".  Blank or null is normal.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public AbnormalFlag AbnormalFlag;
		///<summary>OBX.11 - Observation Result Status.</summary>
		public ResultStatus ResultStatus;
		///<summary>OBX.14 - Date/Time of Observation.  yyyyMMddHHmm format in the message, no seconds.
		///Date and time tech entered result into the Lab System.</summary>
		public DateTime DateTimeObs;
		///<summary>OBX.15 - Producer ID (Producer’s Reference).  LabCorp report field "LAB".  ID of LabCorp Facility responsible for performing the
		///testing.  The Lab Name is supplied in the ZPS segment.</summary>
		public string FacilityID;
		#endregion OBX Fields
		///<summary>FK to document.DocNum.  ZEF.2 - Embedded File.  Each result may have one or more ZEF segments for embedded files.
		///The base-64 text version of the PDF is sent in ZEF.2.  If the file size exceeds 50k, then multiple segments will be sent with 50k blocks
		///of the text.  When processing, we will concatenate all ZEF.2 fields, create the PDF document, store the file in the patient's image folder,
		///and create an entry in the document table.  Then update this field with the pointer to the document table entry.</summary>
		public long DocNum;
		///<summary>NTE.3 at the OBX level.  The NTE segment is repeatable and the Comment Text component is limited to 78 characters.  Multiple NTE
		///segments can be used for longer comments.  All NTE segments at the OBX level will be concatenated and stored in this one field.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;

		///<summary></summary>
		public MedLabResult Copy() {
			return (MedLabResult)MemberwiseClone();
		}

	}

	///<summary>MedLab Abnormal Flags.</summary>
	public enum AbnormalFlag {
		///<summary>0 - None.  Blank or null value indicates normal result, so no abnormal flag.</summary>
		None,
		///<summary>1 - Panic High.  Actual value is "&gt;" but symbol cannot be used as an enum value.</summary>
		_gt,
		///<summary>2 - Panic Low.  Actual value is "&lt;" but symbol cannot be used as an enum value.</summary>
		_lt,
		///<summary>3 - Abnormal.  Applies to non-numeric results.</summary>
		A,
		///<summary>4 - Critical Abnormal.  Applies to non-numeric results.</summary>
		AA,
		///<summary>5 - Above High Normal.</summary>
		H,
		///<summary>6 - Alert High.</summary>
		HH,
		///<summary>7 - Intermediate.  For Discrete Microbiology susceptibilities only.</summary>
		I,
		///<summary>8 - Below Low Normal.</summary>
		L,
		///<summary>9 - Alert Low.</summary>
		LL,
		///<summary>10 - Negative for Drug Interpretation Codes and Discrete Microbiology.</summary>
		NEG,
		///<summary>11 - Positive for Drug Interpretation Codes and Discrete Microbiology.</summary>
		POS,
		///<summary>12 - Resistant.  For Discrete Microbiology susceptibilities only.</summary>
		R,
		///<summary>13 - Susceptible.  For Discrete Microbiology susceptibilities only.</summary>
		S
	}

}