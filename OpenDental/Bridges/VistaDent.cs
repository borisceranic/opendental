using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Bridges{
	/// <summary></summary>
	public class VistaDent {

		/// <summary></summary>
		public VistaDent() {
			
		}

		/// <summary></summary>
		public static void SendData(Program ProgramCur,Patient pat) {
			string path=Programs.GetProgramPath(ProgramCur);
			if(pat==null) {//Launch program without any patient.
				try {
					Process.Start(path);
				}
				catch {
					MessageBox.Show(path+" is not available.");
				}
				return;
			}
			//Documentation for command line arguements is very vague.
			string str="";
			str+="-first=\""+Tidy(pat.FName)+"\" ";
			str+="-last=\""+Tidy(pat.LName)+"\" ";
			if(ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Enter 0 to use PatientNum, or 1 to use ChartNum")=="0"){
				str+="-id=\""+pat.PatNum.ToString()+"\" ";
			}
			else{
				str+="-id=\""+Tidy(pat.ChartNumber)+"\" ";
			}
			str+="-DOB=\""+pat.Birthdate.ToString("yyyy-MM-dd")+"\" ";//Required.  Should update automatically based on patient id, in the case where first bridged with birthdate 01/01/0001, although we don't know for sure.
			if(pat.Gender==PatientGender.Female) {
				str+="-sex=\"f\"";//Probably what they use for female, based on their example for male, although we do not know for sure because the specification does not say.
			}
			else if(pat.Gender==PatientGender.Male) {
				str+="-sex=\"m\"";//This option is valid, because it is part of the example inside the specification.
			}
			else {
				str+="-sex=\"u\"";//Probably what they use for unknown (if unknown is even an option), based on their example for male, although we do not know for sure because the specification does not say.
			}
			try {
				Process.Start(path,str);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		///<summary>Removes semicolons and spaces.</summary>
		private static string Tidy(string input) {
			string retVal=input.Replace(";","");//get rid of any semicolons.
			retVal=input.Replace("\"","");//get rid of any quotation marks.
			retVal=retVal.Replace(" ","");
			return retVal;
		}

	}
}