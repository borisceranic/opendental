using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class PhoneConf:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long PhoneConfNum;
		///<summary>0 to 19.</summary>
		public int ButtonIndex;
		///<summary></summary>
		public int Occupants;
		///<summary>Acts like a FKey to Asterisk phone extentions. Manually manipulated to change behavior.</summary>
		public int Extension;

		public PhoneConf Copy() {
			return (PhoneConf)this.MemberwiseClone();
		}
	}
}



