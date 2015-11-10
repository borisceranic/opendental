using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class ApptComm:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ApptCommNum;
		///<summary>FK to appointment.AptNum.</summary>
		public long ApptNum;
		///<summary>Enum: IntervalType.</summary>
		public IntervalType ApptCommType;
		///<summary>DateTime this AptComm should be sent.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSend;
	}

	///<summary></summary>
	public enum CommType {
		///<summary></summary>
		Preferred,
		///<summary></summary>
		Text,
		///<summary></summary>
		Email
	}

	///<summary></summary>
	public enum IntervalType {
		///<summary></summary>
		Daily,
		///<summary></summary>
		Hourly
	}

}
