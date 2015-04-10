using System;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable]
	public class SmsMT:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsMTNum;
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>GUID.</summary>
		public string GuidMessage;
		///<summary>GUID.</summary>
		public string GuidBatch;
		///<summary>This is the VLN that was used to sent this message.</summary>
		public string VlnNumber;
		///<summary>The phone number that this message was sent to. Must be kept in addition to the PatNum.</summary>
		public string PhonePat;
		///<summary>Set to true if this message should "jump the queue" and be sent asap.</summary>
		public bool IsTimeSensitive;
		public SMSMessageSource MsgType;
		///<summary>.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string MsgText;
		///<summary>Set by the Listener, tracks status of SMS.</summary>
		public SMSDeliveryStatus Status;
		///<summary>A single long message will be broken into several smaller 153 utf8 or 70 unicode character messages.</summary>
		public int MsgParts;
		///<summary>The amount charged to the customer. Total cost for this message.</summary>
		public double MsgCost;
		///<summary>FK to clinic.ClinicNum.  Only used when associating SMS accounts to clinics.</summary>
		public long ClinicNum;
		///<summary>Only used when SMSDeliveryStatus==Failed.</summary>
		public string FailCode;
		///<summary>Time message was sent and accepted at Open Dental.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSent;
		///<summary>Date time that the message was either successfully delivered or was failed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeTerminated;

		///<summary></summary>
		public SmsMT Copy() {
			return (SmsMT)this.MemberwiseClone();
		}
	}

	///<summary>This helps us determine how to handle messages.</summary>
	public enum SMSMessageSource{
		Generic,
		DirectSMS,
		Recall,
		Reminder
	}

	///<summary>None should never be used, the code should be re-written to not use it.</summary>
	public enum SMSDeliveryStatus {
		None,
		Pending,
		DeliveryConf,
		DeliveryUnconf,
		FailWithCharge,
		FailNoCharge
	}
}