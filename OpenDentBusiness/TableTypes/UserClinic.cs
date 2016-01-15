using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness{
		///<summary>This linker table will enable users to be associated with multiple clinics.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class UserClinic:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long UserClinicNum;
		///<summary>FK to Userod.UserNum</summary>
		public long UserNum;
		///<summary>FK to Clinic.ClinicNum</summary>
		public long ClinicNum;

		public UserClinic() {

		}

		public UserClinic(long clinicNum,long userNum) {
			UserNum=userNum;
			ClinicNum=clinicNum;
		}

		///<summary></summary>
		public UserClinic Copy(){
			return (UserClinic)this.MemberwiseClone();
		}

	}


}
