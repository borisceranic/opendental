using System;

namespace OpenDentBusiness {
	///<summary>A Mobile Originating SMS bound for the office. Will usually be a re-constructed message.</summary>
	[Serializable]
	public class SmsFromMobile:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsFromMobileNum;
		///<summary>FK to patient.PatNum. </summary>
		public long PatNum;
		///<summary>FK to clinic.ClinicNum. </summary>
		public long ClinicNum;
		///<summary>FK to commlog.CommlogNum. </summary>
		public long CommlogNum;
		///<summary>Contents of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string MsgText;
		///<summary>Date and time message was inserted into the DB.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeReceived;
		///<summary>This is the Phone Number of the office that the mobile device sent a message to.</summary>
		public string SmsPhoneNumber;
		///<summary>Message part sequence number. For single part messages this should always be 1. 
		///For messages that exist as multiple parts, due to staggered delivery of the parts, this will be a number between 1 and MsgTotal.</summary>
		public string MsgPart;
		///<summary>Total count of message parts for this single message identified by MsgRefID.
		///For single part messages this should always be 1.</summary>
		public string MsgTotal;
		///<summary>Each part of a multipart message will have the same MsgRefID.</summary>
		public string MsgRefID;

		///<summary></summary>
		public SmsFromMobile Copy() {
			return (SmsFromMobile)this.MemberwiseClone();
		}
	}
}