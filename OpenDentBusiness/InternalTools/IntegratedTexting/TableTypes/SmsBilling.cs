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
		///<summary>Total number of clinics for this customer. ClinicsActive + ClinicsInactive = ClinicsTotal.</summary>
		public int ClinicsTotal;		
		///<summary>Subset of ClinicsTotal. Represents number of clinics which have no inactive date or an inactive date on or after 1st of the given calendar month. 
		///These clinics should each be included in the repeating charge for the given calendar month.
		/// ClinicsActive + ClinicsInactive = ClinicsTotal.</summary>
		public int ClinicsActive;
		///<summary>Subset of ClinicsTotal. Represents number of clinics which have an inactive date before 1st of the calendar month. ClinicsActive + ClinicsInactive = ClinicsTotal.</summary>
		public int ClinicsInactive;
		///<summary>Subset of ClinicsTotal. Represents number of clinics which actually accrued messaging charges for the given calendar month.</summary>
		public int ClinicsWithUsage;
		///<summary>Total number of phones for this customer. PhonesActive + PhonesInactive = PhonesTotal.</summary>
		public int PhonesTotal;
		///<summary>Subset of PhonesTotal. Represents number of Phones which have no inactive date or an inactive date on or after 1st of the given calendar month. 
		///PhonesActive + PhonesInactive = PhonesTotal.</summary>
		public int PhonesActive;
		///<summary>Subset of PhonesTotal. Represents number of Phones which have an inactive date before 1st of the calendar month. PhonesActive + PhonesInactive = PhonesTotal.</summary>
		public int PhonesInactive;
		///<summary>Subset of PhonesTotal. Represents number of Phones which actually accrued messaging charges for the given calendar month.</summary>
		public int PhonesWithUsage;

		public SmsBilling Clone() {
			return (SmsBilling)this.MemberwiseClone();
		}	
	}
}









