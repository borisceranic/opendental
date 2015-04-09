using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness.HL7 {
	public class FieldHL7 {
		///<summary></summary>
		private string fullText;
		///<summary>Not often used. Some HL7 fields are allowed to "repeat" multiple times. For example, in immunization messaging export (VXU messages), PID-3 repeats twice, once for patient ID and once for SSN.</summary>
		public List<FieldHL7> ListRepeatFields=new List<FieldHL7>();
		public List<ComponentHL7> Components;
		///<summary>Delimiter order: component separator, repetition separator, escape character, subcomponent separator.
		///<para>Defaults: ^ component, ~ repetition, \ escape character, &amp; subcomponent.</para></summary>
		private char[] _delimiters;

		///<summary>Use this constructor when generating a message.</summary>
		internal FieldHL7(char[] delimiters){
			_delimiters=delimiters;
			fullText="";
			Components=new List<ComponentHL7>();
			ComponentHL7 component=new ComponentHL7("");
			Components.Add(component);
			//add more components later if needed.
		}

		///<summary>Use this constructor when we have a message to parse.  Uses the default delimiters.</summary>
		public FieldHL7(string fieldText) {
			_delimiters=new char[] { '^','~','\\','&' };//default delimiters in this order:  component separator, repetition separator, escape character, subcomponent separator
			FullText=fieldText;
		}

		///<summary>Use this constructor when we have a message to parse.  Uses the delimiters provided, retrieved from the enabled HL7 def if exists.</summary>
		public FieldHL7(string fieldText,char[] delimiters) {
			_delimiters=delimiters;
			FullText=fieldText;
		}

		public override string ToString() {
			return FullText;
		}

		///<summary>Setting the FullText resets all the child components to the values passed in here.</summary>
		public string FullText {
			get {
				StringBuilder sb=new StringBuilder();
				sb.Append(fullText);
				for(int i=0;i<ListRepeatFields.Count;i++) {
					sb.Append(_delimiters[1]);//Field repitition separator.  Always before each repeat field, even if fullText is blank.
					sb.Append(ListRepeatFields[i].FullText);
				}
				return sb.ToString();
			}
			set {
				string[] repeats=value.Split(new char[] { _delimiters[1] },StringSplitOptions.None);//repetitionSeparator defaults to ~
				FieldHL7 repeatField=null;
				for(int r=0;r<repeats.Length;r++) {
					string[] components=repeats[r].Split(new char[] { _delimiters[0] },StringSplitOptions.None);//leave empty entries in place, componentSeparator defaults to ^
					ComponentHL7 component;
					if(r==0) {
						fullText=repeats[r];
						Components=new List<ComponentHL7>();
					}
					else {
						repeatField=new FieldHL7(_delimiters);//initializes the list of components and adds one empty string component
						repeatField.fullText=repeats[r];
						repeatField.Components=new List<ComponentHL7>();
					}
					for(int i=0;i<components.Length;i++) {
						component=new ComponentHL7(components[i]);
						if(r==0) {
							Components.Add(component);
						}
						else {
							repeatField.Components.Add(component);
						}
					}
					if(r>0 && repeatField!=null) {
						ListRepeatFields.Add(repeatField);
					}
				}
			}
		}

		///<summary>If the index supplied is greater than the number of components, this will return an empty string.</summary>
		public string GetComponentVal(int indexPos) {
			if(indexPos > Components.Count-1) {
				return "";
			}
			return Components[indexPos].ComponentVal.Trim();
		}

		///<summary>This also resets the number of components.  And it sets fullText.</summary>
		public void SetVals(params string[] values){
			if(values.Length==1) {
				FullText=values[0];//this allows us to pass in all components for the field as one long string: comp1^comp2^comp3
				return;
			}
			fullText="";
			Components=new List<ComponentHL7>();
			ComponentHL7 component;
			for(int i=0;i<values.Length;i++) {
				component=new ComponentHL7(values[i]);
				Components.Add(component);
				fullText+=values[i];
				if(i<values.Length-1) {
					fullText+=_delimiters[0];
				}
			}
		}

		///<summary>Not often used. Some HL7 fields are allowed to "repeat" multiple times. For example, in immunization messaging export (VXU messages), PID-3 repeats twice, once for patient ID and once for SSN.</summary>
		public void RepeatVals(params string[] values) {
			FieldHL7 field=new FieldHL7(_delimiters);
			field.SetVals(values);
			ListRepeatFields.Add(field);
		}
		 
	}
}
