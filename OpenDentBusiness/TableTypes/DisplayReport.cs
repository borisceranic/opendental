using System;
using System.Collections;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	///<summary>One item is needed for each field on a claimform.</summary>
	[Serializable()]
	[CrudTable(IsSynchable = true)]
	public class DisplayReport:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long DisplayReportNum;
		///<summary>.</summary>
		public string InternalName;
		///<summary>.</summary>
		public int ItemOrder;
		///<summary>.</summary>
		public string Description;
		///<summary>Enum: DisplayReportCategory.  0 - ProdInc; 1 - Daily, 2 - Monthly, 3 - Lists, 4 - PublicHealth, 5 - ArizonaPrimaryCare.</summary>
		public DisplayReportCategory Category;
		///<summary>.</summary>
		public bool IsHidden;

		///<summary>Returns a copy of the claimformitem.</summary>
    public DisplayReport Copy(){
			return (DisplayReport)this.MemberwiseClone(); 
		}


	}

		public enum DisplayReportCategory {
		///<summary>0</summary>
		ProdInc,
		///<summary>1</summary>
		Daily,
		///<summary>2</summary>
		Monthly,
		///<summary>3</summary>
		Lists,
		///<summary>4</summary>
		PublicHealth,
		///<summary>5</summary>
		ArizonaPrimaryCare
	}
	

	

}









