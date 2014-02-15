﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness{
	///<summary>.</summary>
	[Serializable()]
	public class HL7Def:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long HL7DefNum;
		///<summary></summary>
		public string Description;
		///<summary>Enum:ModeTxHL7 File, TcpIp.</summary>
		public ModeTxHL7 ModeTx;
		///<summary>Only used for File mode</summary>
		public string IncomingFolder;
		///<summary>Only used for File mode</summary>
		public string OutgoingFolder;
		///<summary>Only used for tcpip mode. Example: 1461</summary>
		public string IncomingPort;
		///<summary>Only used for tcpip mode. Example: 192.168.0.23:1462</summary>
		public string OutgoingIpPort;
		///<summary>Only relevant for outgoing. Incoming field separators are defined in MSH. Default |.</summary>
		public string FieldSeparator;
		///<summary>Only relevant for outgoing. Incoming field separators are defined in MSH. Default ^.</summary>
		public string ComponentSeparator;
		///<summary>Only relevant for outgoing. Incoming field separators are defined in MSH. Default &.</summary>
		public string SubcomponentSeparator;
		///<summary>Only relevant for outgoing. Incoming field separators are defined in MSH. Default ~.</summary>
		public string RepetitionSeparator;
		///<summary>Only relevant for outgoing. Incoming field separators are defined in MSH. Default \.</summary>
		public string EscapeCharacter;
		///<summary>If this is set, then there will be no child tables. Internal types are fully defined within the C# code rather than in the database.</summary>
		public bool IsInternal;
		///<summary>This will always have a value because we always start with a copy of some internal type.</summary>
		public string InternalType;
		///<summary>Example: 12.2.14. This will be empty if IsInternal. This records the version at which they made their copy. We might have made significant improvements since their copy.</summary>
		public string InternalTypeVersion;
		///<summary>.</summary>
		public bool IsEnabled;
		///<summary></summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		///<summary>The machine name of the computer where the OpenDentHL7 service for this def is running.</summary>
		public string HL7Server;
		///<summary>The name of the HL7 service for this def.  Must begin with OpenDent...</summary>
		public string HL7ServiceName;
		///<summary>Enum:HL7ShowDemographics Hide,Show,Change,ChangeAndAdd</summary>
		public HL7ShowDemographics ShowDemographics;
		///<summary>Show Appointments module.</summary>
		public bool ShowAppts;
		///<summary>Show Account module</summary>
		public bool ShowAccount;
		///<summary>Send the quadrant in the tooth number component instead of the surface component of the FT1.26 field of the outgoing DFT messages.  Only for eCW.</summary>
		public bool IsQuadAsToothNum;


		///<Summary>List of messages associated with this hierarchical definition.  Use items in this list to get to items lower in the hierarchy.</Summary>
		[CrudColumn(IsNotDbColumn=true)]
		[XmlIgnore]
		public List<HL7DefMessage> hl7DefMessages;

		///<summary></summary>
		public HL7Def Clone() {
			return (HL7Def)this.MemberwiseClone();
		}

		public void AddMessage(HL7DefMessage msg,MessageTypeHL7 messageType,EventTypeHL7 eventType,InOutHL7 inOrOut,int itemOrder,string note) {
			if(hl7DefMessages==null) {
				hl7DefMessages=new List<HL7DefMessage>();
			}
			msg.MessageType=messageType;
			msg.EventType=eventType;
			msg.InOrOut=inOrOut;
			msg.ItemOrder=itemOrder;
			msg.Note=note;
			this.hl7DefMessages.Add(msg);
		}

		public void AddMessage(HL7DefMessage msg,MessageTypeHL7 messageType,EventTypeHL7 eventType,InOutHL7 inOrOut,int itemOrder) {
			AddMessage(msg,messageType,eventType,inOrOut,itemOrder,"");
		}

	}

	public enum ModeTxHL7 {
		///<summary>0</summary>
		File,
		///<summary>1</summary>
		TcpIp
	}

	public enum HL7ShowDemographics {
		///<summary>Cannot see or change.</summary>
		Hide,
		///<summary>Can see, but not change.</summary>
		Show,
		///<summary>Can change, but not add patients.  Might get overwritten by next incoming message.</summary>
		Change,
		///<summary>Can change and add patients.  Might get overwritten by next incoming message.</summary>
		ChangeAndAdd
	}

	
}
