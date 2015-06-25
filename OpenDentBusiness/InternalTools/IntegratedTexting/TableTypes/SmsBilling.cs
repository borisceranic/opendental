using System;
using System.Collections;

namespace OpenDentBusiness{
	///<summary>Only used internally by OpenDental, Inc.  Not used by anyone else. Aggregates customer charges for integrated texting.</summary>
	[Serializable()]
	[CrudTable(IsMissingInGeneral=true)]//Remove this line to perform one-time CRUD updated to this class. Place back before committing changes.
	public class SmsBilling:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsBillingNum;
		///<summary>Should be unique in this table.</summary>
		public long RegistrationKeyNum;
		///<summary>May duplicate as one OD customer can have multiple RegistrationKeyNum(s).</summary>
		public long CustPatNum;
		///<summary>Indicates which calendar month this usage metric is for. Should be 1st of month at midnight.</summary>
		public DateTime DateUsage;
		///<summary>The aggregate cost of all message parts charged by Open Dental to the customer. Will need to be summed as the sum of all parts of multi-part messages.
		///Is a function of MsgCost and is calculated from SMSResponse as a result of SendSMS. Maps to OpenDentBusiness.SmsToMobile.MsgCostUSD.</summary>
		public float MsgChargeTotalUSD;
		///<summary>The aggregate cost of all clinic access charges. Calculated using ClinicsActive * RepeatingCharge.ChargeAmt WHERE ProcCode='-38'.</summary>
		public float AccessChargeTotalUSD;
		///<summary>Total number of clinics for this customer.</summary>
		public int ClinicsTotal;		
		///<summary>Subset of ClinicsTotal. Represents number of clinics which have no inactive date or an inactive date on or after 1st of the given calendar month. 
		///These clinics should each be included in the repeating charge for the given calendar month.</summary>
		public int ClinicsActive;
		///<summary>Subset of ClinicsTotal. Represents number of clinics which actually accrued messaging charges for the given calendar month.</summary>
		public int ClinicsWithUsage;
		///<summary>Total number of phones for this customer, active or archived.</summary>
		public int PhonesTotal;
		///<summary>Subset of PhonesTotal. Represents number of Phones which have no inactive date or an inactive date on or after 1st of the given calendar month.</summary>
		public int PhonesActive;
		///<summary>Subset of PhonesTotal. Represents number of Phones which actually accrued messaging charges for the given calendar month.</summary>
		public int PhonesWithUsage;
		///<summary>Sum of messages in MTerminate and MTSend which were succesfully transmitted to patients for this customer for the given calendar month.</summary>
		public int MessagesSentOk;
		///<summary>Sum of messages in MOTerminate which were succesfully transmitted to the customer for the given calendar month. MOUnsent messages will not be included in this count as they have not technically been sent on to the customer yet.</summary>
		public int MessagesRcvOk;
		///<summary>Sum of messages in MTerminate which expired before being transmitted to patients for this customer for the given calendar month.</summary>
		public int MessagesSentFail;
		///<summary>Sum of messages in MOerminate which expired before being transmitted to the customer for the given calendar month. MOUnsent messages will not be included in this count as they have not technically been sent on to the customer yet.</summary>
		public int MessagesRcvFail;

		public SmsBilling Clone() {
			return (SmsBilling)this.MemberwiseClone();
		}	
	}
}

//CREATE TABLE smsbilling (
//	SmsBillingNum BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
//	RegistrationKeyNum BIGINT NOT NULL,
//	CustPatNum BIGINT NOT NULL,
//	DateUsage DATE NOT NULL DEFAULT '0001-01-01',
//	MsgChargeTotalUSD FLOAT NOT NULL,
//	ClinicsTotal INT NOT NULL,
//	ClinicsActive INT NOT NULL,
//	ClinicsWithUsage INT NOT NULL,
//	PhonesTotal INT NOT NULL,
//	PhonesActive INT NOT NULL,
//	PhonesWithUsage INT NOT NULL,
//	INDEX(RegistrationKeyNum),
//	INDEX(CustPatNum)
//	) DEFAULT CHARSET=utf8;







