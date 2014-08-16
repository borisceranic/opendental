using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenDentBusiness.HL7 {
	///<summary>Parses a single incoming HL7 field.</summary>
	public class FieldParser {
		//HL7 has very specific data types.  Each data type that we use will have a corresponding parser method here.
		//Data types are listed in 2.15.

		///<summary>yyyyMMddHHmmss.  Can have more precision than seconds and won't break.  If less than 8 digits, returns MinVal.</summary>
		public static DateTime DateTimeParse(string str) {
			int year=0;
			int month=0;
			int day=0;
			int hour=0;
			int minute=0;
			if(str.Length<8) {
				return DateTime.MinValue;
			}
			try {
				year=PIn.Int(str.Substring(0,4));
				month=PIn.Int(str.Substring(4,2));
				day=PIn.Int(str.Substring(6,2));
			}
			catch(Exception ex) {//PIn.Int could fail if not able to parse into an Int32
				return DateTime.MinValue;
			}
			if(str.Length>=10) {
				try {
					hour=PIn.Int(str.Substring(8,2));
				}
				catch(Exception ex) {
					//do nothing, hour will remain 0
				}
			}
			if(str.Length>=12) {
				try {
					minute=PIn.Int(str.Substring(10,2));
				}
				catch(Exception ex) {
					//do nothing, minute will remain 0
				}
			}
			//skip seconds and any trailing numbers
			DateTime retVal=new DateTime(year,month,day,hour,minute,0);
			return retVal;
		}

		///<summary>M,F,U</summary>
		public static PatientGender GenderParse(string str) {
			if(str.ToLower()=="m" || str.ToLower()=="male") {
				return PatientGender.Male;
			}
			else if(str.ToLower()=="f" || str.ToLower()=="female") {
				return PatientGender.Female;
			}
			else {
				return PatientGender.Unknown;
			}
		}

		public static PatientPosition MaritalStatusParse(string str) {
			switch(str.ToLower()) {
				case "single":
				case "s"://Single
				case "unknown":
				case "u"://Unknown
				case "partner":
				case "p"://Domestic partner
				case "g"://Living together
				case "r"://Registered domestic partner
				case "b"://Unmarried
				case "o"://Other
				case "t"://Unreported
					return PatientPosition.Single;
				case "married":
				case "m"://Married
				case "legally separated":
				case "e"://Legally Separated
				case "a"://Separated
				case "c"://Common law
				case "i"://Interlocutory (divorce is not yet final)
					return PatientPosition.Married;
				case "divorced":
				case "d"://Divorced
				case "n"://Annulled
					return PatientPosition.Divorced;
				case "widowed":
				case "w"://Widowed
					return PatientPosition.Widowed;
				default:
					return PatientPosition.Single;
			}
		}

		/// <summary>If it's exactly 10 digits, it will be formatted like this: (###)###-####.  Otherwise, no change.</summary>
		public static string PhoneParse(string str) {
			if(str.Length != 10) {
				return str;//no change
			}
			return "("+str.Substring(0,3)+")"+str.Substring(3,3)+"-"+str.Substring(6);
		}

		public static string ProcessPattern(DateTime startTime,DateTime stopTime) {
			int minutes=(int)((stopTime-startTime).TotalMinutes);
			if(minutes<=0) {
				return "//";//we don't want it to be zero minutes
			}
			int increments5=minutes/5;
			StringBuilder pattern=new StringBuilder();
			for(int i=0;i<increments5;i++) {
				pattern.Append("X");//make it all provider time, I guess.
			}
			return pattern.ToString();
		}
		
		///<summary>Used by eCW.  This will locate a provider by EcwID and update the FName, LName, and MI if necessary.  If no provider is found by EcwID, than a new provider is inserted and the FName, LName, and MI are set.  Supply in format UPIN^LastName^FirstName^MI (PV1 or AIP) or UPIN^LastName, FirstName MI (AIG).  If UPIN(abbr) does not exist, provider gets created.  If name has changed, provider gets updated.  ProvNum is returned.  If blank, then returns 0.  If field is NULL, returns 0. For PV1, the provider.LName field will hold "LastName, FirstName MI". They can manually change later.</summary>
		public static long ProvProcessEcw(FieldHL7 field) {
			if(field==null) {
				return 0;
			}
			string eID=field.GetComponentVal(0);
			eID=eID.Trim();
			if(eID=="") {
				return 0;
			}
			Provider prov=Providers.GetProvByEcwID(eID);
			bool isNewProv=false;
			bool provChanged=false;
			if(prov==null) {
				isNewProv=true;
				prov=new Provider();
				prov.Abbr=eID;//They can manually change this later.
				prov.EcwID=eID;
				prov.FeeSched=FeeSchedC.ListShort[0].FeeSchedNum;
			}
			if(field.Components.Count==4) {//PV1 segment in format UPIN^LastName^FirstName^MI
				if(prov.LName!=field.GetComponentVal(1)) {
					provChanged=true;
					prov.LName=field.GetComponentVal(1);
				}
				if(prov.FName!=field.GetComponentVal(2)) {
					provChanged=true;
					prov.FName=field.GetComponentVal(2);
				}
				if(prov.MI!=field.GetComponentVal(3)) {
					provChanged=true;
					prov.MI=field.GetComponentVal(3);
				}
			}
			else if(field.Components.Count==2) {//AIG segment in format UPIN^LastName, FirstName MI
				string[] components=field.GetComponentVal(1).Split(' ');
				if(components.Length>0) {
					components[0]=components[0].TrimEnd(',');
					if(prov.LName!=components[0]) {
						provChanged=true;
						prov.LName=components[0];
					}
				}
				if(components.Length>1 && prov.FName!=components[1]) {
					provChanged=true;
					prov.FName=components[1];
				}
				if(components.Length>2 && prov.MI!=components[2]) {
					provChanged=true;
					prov.MI=components[2];
				}
			}
			if(isNewProv) {
				Providers.Insert(prov);
				Providers.RefreshCache();
			}
			else if(provChanged) {
				Providers.Update(prov);
				Providers.RefreshCache();
			}
			return prov.ProvNum;
		}

		///<summary>This field could be a CWE data type or a XCN data type, depending on if it came from an AIG segment, an AIP segment, or a PV1 segment.  The AIG segment would have this as a CWE data type in the format ProvNum^LName, FName^^Abbr.  For the AIP and PV1 segments, the data type is XCN and the format would be ProvNum^LName^FName^^^Abbr.  This will return 0 if the field or segName are null or if no provider can be found.  A new provider will not be inserted with the information provided if not found by ProvNum or name and abbr.</summary>
		public static long ProvParse(FieldHL7 field,SegmentNameHL7 segName) {
			long provNum=0;
			if(field==null) {
				return 0;
			}
			try {
				provNum=PIn.Long(field.GetComponentVal(0));//if component is empty string, provNum will be 0
			}
			catch(Exception ex) {
				//PIn.Long failed to convert the component to a long, provNum will remain 0 and we will attempt to get by name and abbr below
			}			
			if(Providers.GetProv(provNum)!=null) {//if provNum=0 or invalid, GetProv will return null and we will attempt to find the provider by name and abbr
				return provNum;
			}
			provNum=0;//just in case we had a valid long in the ProvNum component but it was not a valid provNum
			//Couldn't find the provider with the ProvNum provided, we will attempt to find by FName, LName, and Abbr
			string provLName="";
			string provFName="";
			string provAbbr="";
			if(segName==SegmentNameHL7.AIG) {//AIG is the data type CWE with format ProvNum^LName, FName^^Abbr
				//GetComponentVal will return an empty string if the index is greater than the number of the components for this field minus 1
				string[] components=field.GetComponentVal(1).Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
				if(components.Length>0) {
					provLName=components[0].TrimEnd(',');
				}
				if(components.Length>1) {
					provFName=components[1];
				}
				provAbbr=field.GetComponentVal(3);
			}
			else if(segName==SegmentNameHL7.AIP || segName==SegmentNameHL7.PV1) {//AIP and PV1 are the data type XCN with the format ProvNum^LName^FName^^^Abbr
				provLName=field.GetComponentVal(1);
				provFName=field.GetComponentVal(2);
				provAbbr=field.GetComponentVal(5);
			}
			if(provAbbr=="") {
				return 0;//there has to be a LName, FName, and Abbr if we are trying to match without a ProvNum.  LName and FName empty string check happens in GetProvsByFLName
			}
			List<Provider> listProvs=Providers.GetProvsByFLName(provLName,provFName);
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].Abbr.ToLower()==provAbbr.ToLower()) {
					//There should be only one provider with this Abbr, although we only warn them about the duplication and allow them to have more than one with the same Abbr.
					//With the LName, FName, and Abbr we can be more certain we retrieve the correct provider.
					provNum=listProvs[i].ProvNum;
					break;
				}
			}
			return provNum;
		}

		///<summary>Returns the race for the CDCREC code supplied using the new patient race enum.  Default is to return PatRace.Other.</summary>
		public static PatRace RaceParse(string strCode) {
			switch(strCode) {
				case "2054-5":
					return PatRace.AfricanAmerican;
				case "1002-5":
					return PatRace.AmericanIndian;
				case "2028-9":
					return PatRace.Asian;
				case "2076-8":
					return PatRace.HawaiiOrPacIsland;
				case "2135-2":
					return PatRace.Hispanic;
				case "2131-1":
					return PatRace.Other;
				case "2106-3":
					return PatRace.White;
				case "2186-5":
					return PatRace.NotHispanic;
				default:
					return PatRace.Other;
			}
		}

		///<summary>Returns the depricated PatientRaceOld enum.  It gets converted to new patient race entries where it's called.  This is the old way of receiving the race, just a string that matches exactly.</summary>
		public static PatientRaceOld RaceParseOld(string str) {
			switch(str) {
				case "American Indian Or Alaska Native":
					return PatientRaceOld.AmericanIndian;
				case "Asian":
					return PatientRaceOld.Asian;
				case "Native Hawaiian or Other Pacific":
					return PatientRaceOld.HawaiiOrPacIsland;
				case "Black or African American":
					return PatientRaceOld.AfricanAmerican;
				case "White":
					return PatientRaceOld.White;
				case "Hispanic":
					return PatientRaceOld.HispanicLatino;
				case "Other Race":
					return PatientRaceOld.Other;
				default:
					return PatientRaceOld.Other;
			}
		}

		public static double SecondsToMinutes(string secs) {
			double retVal;
			try {
				retVal=double.Parse(secs);
			}
			catch {//couldn't parse the value to a double so just return 0
				return 0;
			}
			return retVal/60;
		}

		/// <summary>Will return 0 if string cannot be parsed to a number.  Will return 0 if the fee schedule passed in does not exactly match the description of a regular fee schedule.</summary>
		public static long FeeScheduleParse(string str) {
			if(str=="") {
				return 0;
			}
			FeeSched feeSched=FeeScheds.GetByExactName(str,FeeScheduleType.Normal);
			if(feeSched==null) {
				return 0;
			}
			return feeSched.FeeSchedNum;
		}

		///<summary>A string supplied with new line escape commands (\.br\) will be converted to a string with \r\n in it.  The escapeChar supplied will have been retrieved from the escape characters defined in the message, usually "\".  Example: string supplied - line 1\.br\line2\.br\line3; string returned - line 1\r\nline2\r\nline3.</summary>
		public static string StringNewLineParse(string str,char escapeChar) {
			string strToReplace=escapeChar+".br"+escapeChar;
			if(escapeChar=='\\') {
				//double \'s are required if \ is the escapeChar for the string to replace
				strToReplace=escapeChar+strToReplace+escapeChar;
			}
			return str.Replace(strToReplace,"\r\n");
		}
	}
}
