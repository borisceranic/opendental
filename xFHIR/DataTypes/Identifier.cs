using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xFHIR {
	///<summary>An identifier intended for computation.</summary>
	public class Identifier {
		///<summary>usual | official | temp | secondary (If known).</summary>
		public IdentifierUse use;
		///<summary>Description of identifier.</summary>
		public CodeableConcept type;
		///<summary>The namespace for the identifier (uri).</summary>
		public string system;
		///<summary>The value that is unique</summary>
		public string value;
		///<summary>Time period when id is/was valid for use</summary>
		public Period period;
		///<summary>Organization that issued id (may be just text)</summary>
		public Reference assigner;
	}

	public enum IdentifierUse {
		///<summary>The identifier recommended for display and use in real-world interactions.</summary>
		usual,
		///<summary>The identifier considered to be most trusted for the identification of this item.</summary>
		official,
		///<summary>A temporary identifier.</summary>
		temp,
		///<summary>An identifier that was assigned in secondary use - it serves to identify the object in a relative context, but cannot be consistently 
		///assigned to the same object again in a different context.</summary>
		secondary
	}
}