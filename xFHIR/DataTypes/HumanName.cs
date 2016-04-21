

namespace xFHIR {
	///<summary>Name of a human - parts and usage.</summary>
	public class HumanName {
		///<summary>.</summary>
		public NameUse use;
		///<summary>Text representation of the full name.</summary>
		public string text;
		///<summary>Family name (often called 'Surname').</summary>
		public string family;
		///<summary>Given names (not always 'first'). Includes middle names.</summary>
		public string given;
		///<summary>Parts that come before the name.</summary>
		public string prefix;
		///<summary>Parts that come after the name.</summary>
		public string suffix;
		///<summary>Time period when name was/is in use.</summary>
		public Period period;
	}

	///<summary>.</summary>
	public enum NameUse {
		///<summary>Known as/conventional/the one you normally use.</summary>
		usual,
		///<summary>The formal name as registered in an official (government) registry, but which name might not be commonly used. May be called 
		///"legal name".</summary>
		official,
		///<summary>A temporary name. Name.period can provide more detailed information. This may also be used for temporary names assigned at birth or 
		///in emergency situations.</summary>
		temp,
		///<summary>A name that is used to address the person in an informal manner, but is not part of their formal or usual name.</summary>
		nickname,
		///<summary>Anonymous assigned name, alias, or pseudonym (used to protect a person's identity for privacy reasons).</summary>
		anonymous,
		///<summary>This name is no longer in use (or was never correct, but retained for records).</summary>
		old,
		///<summary>A name used prior to marriage. Marriage naming customs vary greatly around the world. This name use is for use by applications that 
		///collect and store "maiden" names. Though the concept of maiden name is often gender specific, the use of this term is not gender specific. 
		///The use of this term does not imply any particular history for a person's name, nor should the maiden name be determined 
		///algorithmically.</summary>
		maiden
	}
}