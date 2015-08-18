using System;

namespace OpenDentBusiness{

	///<summary>Used in public health.  The database table only has 3 columns.  There are 5 additional columns in C# that are not in the databae.  These extra columns are used in the UI to organize input, and are transferred to the screen table as needed.</summary>
	[Serializable]
	public class ScreenGroup:TableBase {
		///<summary>Primary key</summary>
		[CrudColumn(IsPriKey=true)]
		public long ScreenGroupNum;
		///<summary>Up to the user.</summary>
		public string Description;
		///<summary>Date used to help order the groups.</summary>
		public DateTime SGDate;
		///<summary>Not a database column. Used if ProvNum=0.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string ProvName;
		///<summary>Not a database column. Foreign key to provider.ProvNum. Can be 0 if not a standard provider.  In that case, a ProvName should be entered.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public long ProvNum;
		///<summary>Not a database column. See the PlaceOfService enum.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public PlaceOfService PlaceService;
		///<summary>Not a database column. Foreign key to county.CountyName, although it will not crash if key absent.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string County;
		///<summary>Not a database column. Foreign key to school.SchoolName, although it will not crash if key absent.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string GradeSchool;
	}

	

	

}













