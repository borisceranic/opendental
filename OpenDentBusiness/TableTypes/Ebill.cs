using System;

namespace OpenDentBusiness {
	///<summary>Keeps track of account details of e-statements per clinic.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class Ebill:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EbillNum;
		///<summary>FK to clinic.ClinicNum</summary>
		public long ClinicNum;
		///<summary>The account number for the e-statement client.</summary>
		public string ClientAcctNumber;
		///<summary>The user name for this particular account.</summary>
		public string ElectUserName;
		///<summary>The password for this particular account.</summary>
		public string ElectPassword;

		public Ebill Copy(){
			return (Ebill)this.MemberwiseClone();
		}
	}
	
}
