/*
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
		///<summary>Because we don't want users to have to enter in all 100 possible percentages into a grading scale, they can just set this bool instead.  For all other grading scale types, they will need to enter all possibilities.</summary>
		public bool IsPercentage;
		///<summary>For example, A-F or Pass/Fail.</summary>
		public string Description;

		///<summary></summary>
		public GradingScale Copy() {
			return (GradingScale)this.MemberwiseClone();
		}

	}

	
}




*/