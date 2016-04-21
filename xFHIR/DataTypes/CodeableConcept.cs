using System.Collections.Generic;

namespace xFHIR {
	///<summary>Concept - reference to a terminology or just text.</summary>
	public class CodeableConcept {
		///<summary>Code defined by a terminology system.</summary>
		public List<Coding> code;
		///<summary>Plain text representation of the concept.</summary>
		public string text;

		public CodeableConcept() {
			code=new List<Coding>();
			text="";
		}
	}
}