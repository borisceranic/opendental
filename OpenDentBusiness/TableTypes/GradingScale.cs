using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>Used in Evaluations.  Describes a scale to be used in grading.  Freeform scales are not allowed.  Percentage scales are handled a little differently than the other scales.</summary>
	[Serializable]
	public class GradingScale:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long GradingScaleNum;
		///<summary>Enum:ScaleType Used to determine method of assigning grades.  PickList will be the only type that has GradingScaleItems.</summary>
		public ScaleType ScaleType;
		///<summary>For example, A-F or Pass/Fail.</summary>
		public string Description;
		///<summary>For ScaleType=Points, this is just a default.  For percentages, this gets set to 100 and cannot be changed.  For PickList, 4 is a typical example.</summary>
		public float MaxPointsPoss;

		///<summary></summary>
		public GradingScale Copy() {
			return (GradingScale)this.MemberwiseClone();
		}

	}

	
}




