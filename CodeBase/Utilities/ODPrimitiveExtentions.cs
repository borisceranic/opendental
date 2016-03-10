using System;
using System.ComponentModel;
using System.Reflection;

namespace OpenDentBusiness {
	public static class ODPrimitiveExtentions {
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

		public static string Left(this string s,int maxCharacters,bool hasElipsis=false) {
			if(s==null || string.IsNullOrEmpty(s) || maxCharacters<1) {
				return "";
			}
			if(s.Length>maxCharacters) {
				if(hasElipsis && maxCharacters>4) {
					return s.Substring(0,maxCharacters-3)+"...";
				}
				return s.Substring(0,maxCharacters);
			}
			return s;
		}

		public static string Right(this string s,int maxCharacters) {
			if(s==null || string.IsNullOrEmpty(s) || maxCharacters<1) {
				return "";
			}
			if(s.Length>maxCharacters) {
				return s.Substring(s.Length-maxCharacters,maxCharacters);
			}
			return s;
		}

		///<summary>Returns the Description attribute if avaiable. If not, returns enum.ToString().</summary>
		public static string GetDescription(this Enum value) {
			Type type = value.GetType();
			string name = Enum.GetName(type,value);
			if(name == null) {
				return value.ToString();
			}
			FieldInfo field = type.GetField(name);
			if(field == null) {
				return value.ToString();
			}
			DescriptionAttribute attr = Attribute.GetCustomAttribute(field,typeof(DescriptionAttribute)) as DescriptionAttribute;
			if(attr == null) {
				return value.ToString();
			}
			return attr.Description;
		}

		//Example: 1/5+1/5-1/10-1/10-1/10-1/10 does not equal zero.
	}
}
