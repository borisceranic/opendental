using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable]
	public class SmsVln:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsVlnNum;
		///<summary>FK to clinic.ClinicNum. </summary>
		public long ClinicNum;
		///<summary>String representation of the phone number. Ex: 15035551234 This field should not contain any formatting characters.</summary>
		public string VlnNumber;
		///<summary>Date and time this VLN became active.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateActive;
		///<summary>Date and time this VLN became inactive.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateInactive;
		///<summary>Used to indicate why this number was made inactive.</summary>
		public string InactiveCode;

		///<summary></summary>
		public SmsVln Copy() {
			return (SmsVln)this.MemberwiseClone();
		}
	}
}