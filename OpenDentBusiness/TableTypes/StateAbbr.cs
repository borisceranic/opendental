using System;

namespace OpenDentBusiness {
	///<summary>State abbreviations are always copied to patient records rather than linked.  
	///Items in this list can be freely altered or deleted without harming patient data.</summary>
	[Serializable]
	public class StateAbbr:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long StateAbbrNum;
		///<summary>Full state name</summary>
		public string Description;
		///<summary>Short state abbreviation (usually 2 digit)</summary>
		public string Abbr;

		///<summary></summary>
		public StateAbbr Clone() {
			return (StateAbbr)this.MemberwiseClone();
		}
	}
}