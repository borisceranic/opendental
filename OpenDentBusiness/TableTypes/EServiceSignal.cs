using System;

namespace OpenDentBusiness {
	///<summary>Communication item from OD Cloud to workstation.</summary>
	[Serializable]
	public class EServiceSignal:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EServiceSignalNum;
		///<summary>The reason for the eServiceSignal. This code is used to determine what actions to take and how to process this message.</summary>
		public int ReasonCode;
		///<summary>Adverse reaction description.</summary>
		public eServiceStatus SeverityStatus;
		///<summary>Human readable description of what this signal means, or a message for the user.</summary>
		public string Description;
		///<summary>Time signal was sent.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime TimeStamp;
		///<summary>Used to store serialized data that can be used for processing this signal. 
		///For instance, if a SMSNumberAssigned message (ReasonCode 20001) then this would contain serialized data 
		///about the new VLN that is to be assigned and inserted into the local DB.</summary>
		public string Tag;
		///<summary>After a message has been processed or acknowledged this is set true.</summary>
		public bool IsProcessed;

		///<summary></summary>
		public EServiceSignal Copy() {
			return (EServiceSignal)this.MemberwiseClone();
		}
	}


	/////<summary>Communication item bound for workstation.</summary>
	//[Serializable]
	//public class EServiceSignalHQ:TableBase {
	//	///<summary>Primary key.</summary>
	//	[CrudColumn(IsPriKey=true)]
	//	public long EServiceSignalHQNum;
	//	public long CustPatNum;
	//	///<summary>The reason for the eServiceSignal. This code is used to determine what actions to take and how to process this message.</summary>
	//	public int ReasonCode;
	//	///<summary>Adverse reaction description.</summary>
	//	public eServiceStatus Severity;
	//	///<summary>Human readable description of what this signal means, or a message for the user.</summary>
	//	public string Description;
	//	///<summary>The historical date that the patient had the adverse reaction to this agent.</summary>
	//	public DateTime TimeStamp;
	//	///<summary>Used to store serialized data that can be used for processing this signal. For instance, if a SMSNumberAssigned message (ReasonCode 20001) 
	//	///then this would contain serialized data about the new VLN that is to be assigned and inserted into the local DB.</summary>
	//	public string Tag;
	//	///<summary>After a message has been processed or acknowledged this is set true.</summary>
	//	public bool IsProcessed;

	//	///<summary></summary>
	//	public EServiceSignal Copy() {
	//		return (EServiceSignal)this.MemberwiseClone();
	//	}
	//}

	///<summary>Used to determine that status of the entire service. 
	///For example, if there is a Listener eServiceSignal (ReasonCode 10000 to 19999) and a Status of Error, 
	///then the entire listener service is considered to be in the error state.</summary>
	public enum eServiceStatus {
		NotEnabled,
		Working,
		Warning,
		Error,
		Critical
	}
}