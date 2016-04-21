using System;

namespace xFHIR {
	///<summary>Time range defined by start and end date/time.	If present, start SHALL have a lower value than end.</summary>
	public class Period {
		///<summary>Starting time with inclusive boundary.</summary>
		public DateTime start;
		///<summary>End time with inclusive boundary, if not ongoing.</summary>
		public DateTime end;
	}
}