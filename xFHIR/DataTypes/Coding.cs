namespace xFHIR {
	///<summary>A reference to a code defined by a terminology system</summary>
	public class Coding {
		///<summary>Identity of the terminology system (uri).</summary>
		public string system;
		///<summary>Version of the system - if relevant.</summary>
		public string version;
		///<summary>Symbol in syntax defined by the system.</summary>
		public string code;
		///<summary>Representation defined by the system.</summary>
		public string display;
		///<summary>If this coding was chosen directly by the user.</summary>
		public bool userSelected;

	}
}