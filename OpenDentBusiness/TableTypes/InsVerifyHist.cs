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

		///<summary>TODO Should I "new" this method to get rid of the warning?? The warning wasn't removed from TaskHist.</summary>
		public InsVerifyHist Clone() {
			return (InsVerifyHist)this.MemberwiseClone();
		}
	}
}
