using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>One credit card along with any recurring charge information.</summary>
	[Serializable]
	public class CreditCard:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long CreditCardNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>.</summary>
		public string Address;
		///<summary>Postal code.</summary>
		public string Zip;
		///<summary>Token for X-Charge. Alphanumeric, upper and lower case, about 15 char long.  Passed into Xcharge instead of the actual card number.</summary>
		public string XChargeToken;
		///<summary>Credit Card Number.  Will be stored masked: XXXXXXXXXXXX1234.</summary>
		public string CCNumberMasked;
		///<summary>Only month and year are used, the day will usually be 1.</summary>
		public DateTime CCExpiration;
		///<summary>The order that multiple cards will show.  Zero-based.  First one will be default.</summary>
		public int ItemOrder;
		///<summary>Amount set for recurring charges.</summary>
		public Double ChargeAmt;
		///<summary>Start date for recurring charges.</summary>
		public DateTime DateStart;
		///<summary>Stop date for recurring charges.</summary>
		public DateTime DateStop;
		///<summary>Any notes about the credit card or account goes here.</summary>
		public string Note;
		///<summary>FK to payplan.PayPlanNum.</summary>
		public long PayPlanNum;
		///<summary>Token for PayConnect.  PayConnect returns a token and token expiration, when requested by the merchant's system, to be used instead
		///of actual credit card number in subsequent transactions.</summary>
		public string PayConnectToken;
		///<summary>Expiration for the PayConnect token.  Used with the PayConnect token instead of the actual credit card number and expiration.</summary>
		public DateTime PayConnectTokenExp;
		///<summary>What procedures will go on this card as a recurring charge.  Comma delimited list of ProcCodes.</summary>
		public string Procedures;
		///<summary>Indicates which application made this credit card and token. PatientPortal will only use PatientPortal sourced rows. Likewise 
		///OD Proper will only use ODProper rows.</summary>
		public CreditCardSource CCSource;


		///<summary></summary>
		public CreditCard Clone() {
			return (CreditCard)this.MemberwiseClone();
		}
	}

	public enum CreditCardSource {
		/// <summary>0</summary>
		ODProper,
		/// <summary>1</summary>
		PatientPortal
	}
}
