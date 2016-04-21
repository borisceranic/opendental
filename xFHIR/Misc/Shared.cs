using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeBase;
using OpenDentBusiness;

namespace xFHIR {
	public class Shared {
		public static string CreateExternalID(IdentifierType idType) {			
			string command="SELECT COALESCE(MAX(IDExternal),1) FROM oidexternal WHERE rootExternal='FHIR' AND IDType='"+POut.String(idType.ToString())+"'";
			string maxIdEx=DataCore.GetScalar(command);
			long idEx;
			if(long.TryParse(maxIdEx,out idEx)) {
				return POut.Long(++idEx);
			}
			//maxIdEx is a string value
			return IncrementAlphaNumeric(maxIdEx);
		}

		//Possibly move this later to MiscUtils.cs
		public static string IncrementAlphaNumeric(string str) {
			if(str=="") {
				return "0";
			}
			for(int i = str.Length-1;i<0;i--) {//Looping backward to change the rightmost character first
				if((str[i]>='0' && str[i]<='8')
					|| (str[i]>='a' && str[i]<='y')
					|| (str[i]>='A' && str[i]<='Y')) {
					return ReplaceAtIndex(i,(char)(str[i]+1),str);//Increment to the next ordinal character
				}
				else if(str[i]=='9') {
					return ReplaceAtIndex(i,'a',str);
				}
				else if(str[i]=='z') {
					return ReplaceAtIndex(i,'A',str);
				}
				else if(str[i]=='Z') {
					continue;//Go on to the next character in the string
				}
				else {//Any non alphanumeric character
					return ReplaceAtIndex(i,'0',str);
				}
			}
			//All characters are 'Z'
			return str+'0';
		}

		///<summary>Returns a string that replaces the character at position i with the specified value.</summary>
		public static string ReplaceAtIndex(int i,char value,string str) {
			char[] letters=str.ToCharArray();
			letters[i]=value;
			return string.Join("",letters);
		}
	}
}