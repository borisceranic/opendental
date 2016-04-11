﻿using System;

namespace OpenDentBusiness {
	///<summary>A Mobile Originating SMS bound for the office. Will usually be a re-constructed message.</summary>
	[Serializable]
	public class SmsFromMobile:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsFromMobileNum;
		///<summary>FK to patient.PatNum. Not sent from HQ.</summary>
		public long PatNum;
		///<summary>FK to clinic.ClinicNum. </summary>
		public long ClinicNum;
		///<summary>FK to commlog.CommlogNum. Not sent from HQ.</summary>
		public long CommlogNum;
		///<summary>Contents of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClobNote)]
		public string MsgText;
		///<summary>Date and time message was inserted into the DB. Not sent from HQ.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeReceived;
		///<summary>This is the Phone Number of the office that the mobile device sent a message to.</summary>
		public string SmsPhoneNumber;
		///<summary>This is the PhoneNumber that this message was sent from.</summary>
		public string MobilePhoneNumber;
		///<summary>Message part sequence number. For single part messages this should always be 1. 
		///For messages that exist as multiple parts, due to staggered delivery of the parts, this will be a number between 1 and MsgTotal.</summary>
		public int MsgPart;
		///<summary>Total count of message parts for this single message identified by MsgRefID.
		///For single part messages this should always be 1.</summary>
		public int MsgTotal;
		///<summary>Each part of a multipart message will have the same MsgRefID.</summary>
		public string MsgRefID;
		///<summary>Enum:SmsFromStatus .</summary>
		public SmsFromStatus SmsStatus;
		///<summary>Words surrounded by spaces, flags should be all lower case. This allows simple querrying. Example: " junk  recall " allows you to 
		///write "WHERE Flags like "% junk %" without having to worry about commas. Also, adding and removing tags is easier. Example: Flags=Flags.Replace(" junk ","");</summary>
		public string Flags;
		///<summary>Messages are not deleted, they can only be hidden.</summary>
		public bool IsHidden;
		public int MatchCount;
		/////<summary>Indicates if this message was found to match a pending matchable transaction. EG appointment confirmation.</summary>
		//public SmsMatchCodeStatus MatchStatus;
		/////<summary>Base36 guid generated at HQ which will be used to match incoming responses against outgoing transaction requests.</summary>
		//public string MatchGuid;

		///<summary></summary>
		public SmsFromMobile Copy() {
			return (SmsFromMobile)this.MemberwiseClone();
		}

		///<summary>Convenient way to access the Flags and check or set Read status.</summary>
		public bool IsRead {
			get {
				return Flags.Contains(" read ");
			}
			set {
				Flags=Flags.Replace(" read ","");
				if(value) {
					Flags=Flags+" read ";
				}
			}
		}

	}

	///<summary>Status of an incoming message.</summary>
	public enum SmsFromStatus {
		///<summary>0</summary>
		ReceivedUnread,
		///<summary>1</summary>
		ReceivedRead,
	}
	
	public enum SmsMatchCodeStatus {
		///<summary>User reponse did not match positive or negative action code (NO MATCH). This is not an automatically digestable response..</summary>
		NotFound,
		///<summary>User reponse matched positive action code (YES).</summary>
		Positive,
		///<summary>User reponse matched negative action code (NO).</summary>
		Negative
	}
}