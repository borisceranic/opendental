using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
  ///<summary></summary>
  [Serializable]
	public class InsVerify:TableBase {
    ///<summary>Primary key.</summary>
    [CrudColumn(IsPriKey=true)]
		public long InsVerifyNum;
		///<summary>The date of the last successful verification.</summary>
		public DateTime DateLastVerified;
		///<summary>FK to userod.UserNum.  This is the assigned user for this verification.</summary>
		public long UserNum;
		///<summary>Enum:VerifyTypes The type of verification.</summary>
		public VerifyTypes VerifyType;
		///<summary>FK to any table defined in the VerifyType Enumeration.</summary>
		public long FKey;
		///<summary>FK to definition.DefNum.  Links to the category InsVerifyStatus</summary>
		public long DefNum;
		///<summary>Note for this insurance verification.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string Note;

    ///<summary></summary>
		public InsVerify Clone() {
			return (InsVerify)this.MemberwiseClone();
    }
	}

	///<summary></summary>
	public enum VerifyTypes {
		///<summary>0.  This means FKey should be 0.</summary>
		None,
		///<summary>1.  This means FKey will link to insplan.InsPlanNum</summary>
		InsuranceBenefit,
		///<summary>2.  This means FKey will link to patplan.PatPlanNum</summary>
		PatientEnrollment
	}
}