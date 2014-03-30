/*
using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>The specific grades allowed on a scale.  Contains both the GradeShowing and the optional equialent number.  There are no FKs to these items.  The values are all copied from here into student records as they are used, so these can be changed without damaging any student records.  A grading scale that is a percentage type will not have any items in it.</summary>
	[Serializable]
	public class GradingScaleItem:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long GradingScaleItemNum;
		///<summary>FK to gradingscale.GradingScaleNum</summary>
		public long GradingScaleNum;
		///<summary>For example A, B, C, D, F, or 1-10, etc.  Required.</summary>
		public string GradeShowing;
		///<summary>For example A=4, A-=3.8, pass=1, etc.  Required.</summary>
		public float GradeNumber;
		///<summary>Optional additional info about what this particular grade means.  Just used as guidance and does not get copied to the individual student record.</summary>
		public string Description;

		///<summary></summary>
		public GradingScaleItem Copy() {
			return (GradingScaleItem)this.MemberwiseClone();
		}

	}

	
}




*/