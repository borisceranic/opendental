using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable]
	public class InsVerifyHist:InsVerify {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long InsVerifyHistNum;
		
		public InsVerifyHist() {
		}

		public InsVerifyHist(InsVerify insVerify) {
			this.InsVerifyNum=insVerify.InsVerifyNum;
			this.DateLastVerified=insVerify.DateLastVerified;
			this.UserNum=insVerify.UserNum;
			this.VerifyType=insVerify.VerifyType;
			this.FKey=insVerify.FKey;
			this.DefNum=insVerify.DefNum;
			this.Note=insVerify.Note;
			this.DateLastAssigned=insVerify.DateLastAssigned;
		}

		///<summary></summary>
		public InsVerifyHist Clone() {
			return (InsVerifyHist)this.MemberwiseClone();
		}
	}
}
