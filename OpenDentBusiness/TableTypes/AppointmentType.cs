using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness{
	
	///<summary>Appointment type is used to override appointment color.  Might control other properties on appointments in the future.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class AppointmentType:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long AppointmentTypeNum;
		///<summary></summary>
		public string AppointmentTypeName;
		///<summary></summary>
		public Color AppointmentTypeColor;
		///<summary>0 based</summary>
		public int ItemOrder;
		///<summary></summary>
		public bool IsHidden;

		///<summary>Returns a copy of the appointment.</summary>
		public AppointmentType Clone() {
			return (AppointmentType)this.MemberwiseClone();
		}

		
	}
	
	


}









