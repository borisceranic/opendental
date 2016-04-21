namespace xFHIR {
	public class ContactPoint {

		public ContactPointSystem system;

		public string value;

		public ContactPointUse use;

		public uint rank;

		public Period period;

	}

	public enum ContactPointSystem {
		///<summary>The value is a telephone number used for voice calls. Use of full international numbers starting with + is recommended to enable
		///automatic dialing support but not required.</summary>
		phone,
		///<summary>The value is a fax machine. Use of full international numbers starting with + is recommended to enable automatic dialing support 
		///but not required.</summary>
		fax,
		///<summary>The value is an email address.</summary>
		email,
		///<summary>The value is a pager number. These may be local pager numbers that are only usable on a particular pager system.</summary>
		pager,
		///<summary>A contact that is not a phone, fax, or email address. The format of the value SHOULD be a URL. This is intended for various personal contacts including blogs, Twitter, Facebook, etc. Do not use for email addresses. If this is not a URL, then it will require human interpretation.</summary>
		other
	}

	public enum ContactPointUse {

	}
}