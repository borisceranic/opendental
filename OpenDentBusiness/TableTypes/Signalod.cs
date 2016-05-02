using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>An actual signal that gets sent out as part of the messaging functionality.</summary>
	[Serializable]
	public class Signalod:TableBase,IComparable{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SignalNum;
		///<summary>Text version of 'user' this message was sent from, which can actually be any description of a group or individual.</summary>
		public string FromUser;
		///<summary>Enum:InvalidType List of InvalidType long values separated by commas.  Can be empty.  When Date or Tasks are used, they are used all alone with no other flags present.</summary>
		public string ITypes;
		///<summary>If IType=Date, then this is the affected date in the Appointments module.</summary>
		public DateTime DateViewing;
		///<summary>Enum:SignalType  Button, or Invalid.</summary>
		public SignalType SigType;
		///<summary>This is used if the type is button and the user types in some text, or for type Invalid when ITypes equals InvalidType.SmsTextMsgReceivedUnreadCount.  This is the typed portion and does not include any of the text that was on the buttons, or is the count of unread SMS messages.  Button types of signals are displayed in their own separate list in addition to any light and sound that they may cause, whereas the SMS unread message count is shown in the dropdown portion of the Text button in the main toolbar.</summary>
		public string SigText;
		///<summary>The exact server time when this signal was entered into db.  This does not need to be set by sender since it's handled automatically.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime SigDateTime;
		///<summary>Text version of 'user' this message was sent to, which can actually be any description of a group or individual.</summary>
		public string ToUser;
		///<summary>If this signal has been acknowledged, then this will contain the server date and time.  This is how lights get turned off also.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime AckTime;
		///<summary>FK to task.TaskNum.  If IType=Tasks, then this is the taskNum that was added.</summary>
		public long TaskNum;
		///<summary>Usually identifies the object that was edited to cause the signal to be created.</summary>
		public long FKey;
		///<summary>Describes the type of object referenced by the FKey.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public KeyType FKeyType;
		///<summary>Not a database field.  The sounds and lights attached to the signal.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public SigElement[] ElementList;

		///<summary>IComparable.CompareTo implementation.  This is used to order signals.  This is needed because ordering signals is too complex to do with a query.</summary>
		public int CompareTo(object obj) {
			if(!(obj is Signalod)) {
				throw new ArgumentException("object is not a Signalod");
			}
			Signalod sig=(Signalod)obj;
			DateTime date1;
			DateTime date2;
			if(AckTime.Year<1880){//if not acknowledged
				date1=SigDateTime;
			}
			else{
				date1=AckTime;
			}
			if(sig.AckTime.Year<1880) {//if not acknowledged
				date2=sig.SigDateTime;
			}
			else {
				date2=sig.AckTime;
			}
			return date1.CompareTo(date2);
		}
		
		///<summary></summary>
		public Signalod Copy() {
			Signalod s=(Signalod)this.MemberwiseClone();
			s.ElementList=new SigElement[ElementList.Length];
			ElementList.CopyTo(s.ElementList,0);
			return s;
		}

	
	}

	///<summary>Do not combine with SignalType, they must be seperate. Stored as string, safe to reorder enum values.</summary>
	public enum KeyType {
		Undefined = 0,
		Job,
		Operatory,
		Provider


	}

	

	


}



















