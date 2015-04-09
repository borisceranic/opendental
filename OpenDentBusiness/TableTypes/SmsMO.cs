using System;

namespace OpenDentBusiness {
	///<summary>A Mobile Originating SMS bound for the office. Will usually be a re-constructed message.
	///In some rare cases these messages </summary>
	[Serializable]
	public class SmsMO:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsMONum;
		///<summary>FK to patient.PatNum. </summary>
		public long PatNum;
		///<summary>FK to clinic.ClinicNum. </summary>
		public long ClinicNum;
		///<summary>FK to commlog.CommlogNum. </summary>
		public long CommlogNum;
		///<summary>Contents of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string MsgText;
		///<summary>Time message was sent and accepted at Open Dental.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeEntry;
		///<summary>This is the VLN that this message was sent to.</summary>
		public string SmsVln;
		///<summary>For single messages this should always be 1. 
		///For messages that exist as multiple parts, due to staggered delivery of the parts, this will be a number between 1 and MsgTotal.</summary>
		public string MsgPart;
		///<summary>For single messages this should always be 1. For Multipart messages, this will be the total number of parts for the
		///message identified by MsgRefID.</summary>
		public string MsgTotal;
		///<summary>A unique identifier that allows multipart messages to be reconstructed as a single message.</summary>
		public string MsgRefID;

		///<summary></summary>
		public SmsMO Copy() {
			return (SmsMO)this.MemberwiseClone();
		}
	}
}