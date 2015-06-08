using System;

namespace OpenDentBusiness {
	///<summary>Messages are only inserted into this table after they are accepted by ODHQ.</summary>
	[Serializable]
	public class SmsToMobile:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsToMobileNum;
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>GUID. Uniquely identifies this message and is used for tracking message status.</summary>
		public string GuidMessage;
		///<summary>GUID. When sending batch messages, all messages will have the same batch GUID that should be the GUID of the first message within the batch.</summary>
		public string GuidBatch;
		///<summary>This is the sending phone number in international format. Each office may have several different numbers that they use.</summary>
		public string SmsPhoneNumber;
		///<summary>The phone number that this message was sent to. Must be kept in addition to the PatNum.</summary>
		public string MobilePhoneNumber;
		///<summary>Set to true if this message should "jump the queue" and be sent asap.</summary>
		public bool IsTimeSensitive;
		///<summary>Enum:SmsMessageSource  This is used to identify where in the program this message originated from.</summary>
		public SmsMessageSource MsgType;
		///<summary>The contents of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string MsgText;
		///<summary>Enum:SmsDeliveryStatus  Set by the Listener, tracks status of SMS.</summary>
		public SmsDeliveryStatus Status;
		///<summary>The count of parts that this message will be broken into when sent.
		///A single long message will be broken into several smaller 153 utf8 or 70 unicode character messages.</summary>
		public int MsgParts;
		///<summary>The amount charged to the customer. Total cost for this message always stored in US Dollars.</summary>
		public double MsgCostUSD;
		///<summary>FK to clinic.ClinicNum.  0 when not using clinics.</summary>
		public long ClinicNum;
		///<summary>Only used when SmsDeliveryStatus==Failed.</summary>
		public string CustErrorText;
		///<summary>Time message was accepted at ODHQ.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSent;
		///<summary>Date time that the message was either successfully delivered or failed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeTerminated;

		public SmsToMobile Copy() {
			return (SmsToMobile)this.MemberwiseClone();
		}
	}

	///<summary>This helps us determine how to handle messages.</summary>
	public enum SmsMessageSource{
		///<summary>0. Should not be used.</summary>
		Undefined,
		///<summary>1. This should be used for one-off messages that might be sent as direct communication with patient.</summary>
		DirectSMS,
		///<summary>2. Used when sending single or batch recall SMS.</summary>
		Recall,
		///<summary>3. Used when sending single or batch reminder SMS.</summary>
		Reminder
	}

	///<summary>None should never be used, the code should be re-written to not use it.</summary>
	public enum SmsDeliveryStatus {
		///<summary>0. Should not be used.</summary>
		None,
		///<summary>1. After a message has been accepted at ODHQ. Before any feedback.</summary>
		Pending,
		///<summary>2. Delivered to customer, carrier replied with confirmation.</summary>
		DeliveryConf,
		///<summary>3. Delivered to customer, no confirmation of failure or delivery sent back from carrier.</summary>
		DeliveryUnconf,
		///<summary>4. Attempted delivery, failure message return after arriving at handset.</summary>
		FailWithCharge,
		///<summary>5. Attempted delivery, immediate failure confirmation recieved from carrier.</summary>
		FailNoCharge
	}
}