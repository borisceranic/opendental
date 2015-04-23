using System;


namespace OpenDentBusiness {
	///<summary>Communication item from OD Cloud to workstation.</summary>
	[Serializable]
	public class EServiceSignal:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EServiceSignalNum;
		///<summary>Service which this signal applies to. Defined by eServiceCodes.</summary>
		public int ServiceCode;
		///<summary>Category defined by ReasonCodeCategories. Can be zero if no grouping is necessary per a given service. Stored as an int for forward compatibility.</summary>
		public int ReasonCategory;
		///<summary>The reason for the eServiceSignal. This code is used to determine what actions to take and how to process this message. 
		///It is a function of ReasonCategory. It will most likely be defined by an enum that lives on HQ-only closed source.</summary>
		public int ReasonCode;
		///<summary>Adverse reaction description.</summary>
		public eServiceStatus Severity;
		///<summary>Human readable description of what this signal means, or a message for the user.</summary>
		public string Description;
		///<summary>Time signal was sent.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime TimeStamp;
		///<summary>Used to store serialized data that can be used for processing this signal.</summary>
		public string Tag;
		///<summary>After a message has been processed or acknowledged this is set true.</summary>
		public bool IsProcessed;

		///<summary></summary>
		public EServiceSignal Copy() {
			return (EServiceSignal)this.MemberwiseClone();
		}
	}
	
	///<summary>Used to determine that status of the entire service. 
	///For example, if there is a Listener eServiceSignal (ReasonCode 10000 to 19999) and a Status of Error, 
	///then the entire listener service is considered to be in the error state.</summary>
	public enum eServiceStatus {
		NotEnabled,
		Info,
		Working,
		Warning,
		Error,
		Critical
	}

	///<summary>Used by EServiceSignal.ServiceCode. Each service will have an entry here. Stored as an int for forward compatibility.</summary>
	public enum eServiceCode {
		Undefined=0,
		ListenerService=1,
		SMSService=2
	}
}