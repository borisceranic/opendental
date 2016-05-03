using System;

namespace OpenDentBusiness{

	///<summary>A clinic is usually a separate physical office location.  If multiple clinics are sharing one database, then this is used.  Patients, Operatories, Claims, and many other types of objects can be assigned to a clinic.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class Clinic:TableBase {
		///<summary>Primary key.  Used in patient,payment,claimpayment,appointment,procedurelog, etc.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ClinicNum;
		///<summary>Clinics cache is sorted by Description if preference ClinicListIsAlphabetical is true.</summary>
		public string Description;
		///<summary>.</summary>
		public string Address;
		///<summary>Second line of address.</summary>
		public string Address2;
		///<summary>.</summary>
		public string City;
		///<summary>2 char in the US.</summary>
		public string State;
		///<summary>.</summary>
		public string Zip;
		///<summary>Overrides Address on claims if not blank.</summary>
		public string BillingAddress;
		///<summary>Second line of billing address.</summary>
		public string BillingAddress2;
		///<summary>Overrides City on claims if BillingAddress is not blank.</summary>
		public string BillingCity;
		///<summary>Overrides State on claims if BillingAddress is not blank.</summary>
		public string BillingState;
		///<summary>Overrides Zip on claims if BillingAddress is not blank.</summary>
		public string BillingZip;
		///<summary>Overrides practice PayTo address if not blank.</summary>
		public string PayToAddress;
		///<summary>Second line of PayTo address.</summary>
		public string PayToAddress2;
		///<summary>Overrides practice PayToCity if PayToAddress is not blank.</summary>
		public string PayToCity;
		///<summary>Overrides practice PayToState if PayToAddress is not blank.</summary>
		public string PayToState;
		///<summary>Overrides practice PayToZip if PayToAddress is not blank.</summary>
		public string PayToZip;
		///<summary>Does not include any punctuation.  Exactly 10 digits or blank in USA and Canada.</summary>
		public string Phone;
		///<summary>The account number for deposits.</summary>
		public string BankNumber;
		///<summary>Enum:PlaceOfService Usually 0 unless a mobile clinic for instance.</summary>
		public PlaceOfService DefaultPlaceService;
		///<summary>FK to provider.ProvNum.  0=Default practice provider, -1=Treating provider.</summary>
		public long InsBillingProv;
		///<summary>Does not include any punctuation.  Exactly 10 digits or empty in USA and Canada.</summary>
		public string Fax;
		///<summary>FK to EmailAddress.EmailAddressNum.</summary>
		public long EmailAddressNum;
		///<summary>FK to provider.ProvNum.  Used in place of the default practice provider when making new patients.</summary>
		public long DefaultProv;
		///<summary>DateSMSContract was signed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime SmsContractDate;
		///<summary>Always stored in USD, this is the desired limit for SMS out for a given month.</summary>
		public double SmsMonthlyLimit;
		///<summary>True if this clinic is a medical clinic.  Used to hide/change certain areas of Open Dental, like hiding the tooth chart and changing
		///'dentist' to 'provider'.</summary>
		public bool IsMedicalOnly;
		///<summary>True if this clinic's billing address should be used on outgoing claims.</summary>
		public bool UseBillAddrOnClaims;
		///<summary>FK to definition.DefNum when definition.DefCat is Regions.</summary>
		public long Region;
		///<summary>0 based.  Clinics cache is sorted by ItemOrder if the preference ClinicListIsAlphabetical is false.</summary>
		public int ItemOrder;
		///<summary>True if this clinic should be excluded from showing up in the Insurance Verification List.</summary>
		public bool IsInsVerifyExcluded;

		///<summary>Returns a copy of this Clinic.</summary>
		public Clinic Copy(){
			return (Clinic)this.MemberwiseClone();
		}

	}
	


}













