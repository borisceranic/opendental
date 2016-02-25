using System;

namespace OpenDentBusiness {
	public static class ODMathExtentions {
		///<summary>Used to check if a floating point number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.</summary>
		public static bool IsZero(this double val) {
			return Math.Abs(val)<=0.0000001f;
		}

		///<summary>Used to check if a floating point number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.</summary>
		// ReSharper disable once UnusedMember.Global
		public static bool IsZero(this float val) {
			return Math.Abs(val)<=0.0000001f;
		}

		// ReSharper disable once UnusedMember.Global
		public static bool IsEqual(this float val,float val2) {
			return IsZero(val-val2);
		}

		public static bool IsEqual(this double val,double val2) {
			return IsZero(val-val2);
		}

		public static string Right(this string s,int maxCharacters) {
			if(s==null || string.IsNullOrEmpty(s)) {
				return "";
			}
			if(s.Length>maxCharacters) {
				return s.Substring(s.Length-maxCharacters,maxCharacters);
			}
			return s;
		}

		//Example: 1/5+1/5-1/10-1/10-1/10-1/10 does not equal zero.
	}
}
