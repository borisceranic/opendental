using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Bridges {
	///<summary>Bridge to Apteryx's XVWeb</summary>
	public class XVWeb {

		///<summary></summary>
		public XVWeb() {

		}

		///<summary></summary>
		public static void SendData(Program ProgramCur,Patient pat) {
			string path=ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Enter desired URL address for XVWeb");
			if(pat==null) {
				//Launch program without any patient.
				try {
					Process.Start(path);//should start XVWeb without bringing up a pt.
				}
				catch {
					MessageBox.Show(Lan.g("XVWeb","Could not find")+path+"\r\n"
						+Lan.g("XVWeb","Please set up a default web browser")+".");
				}
				return;
			}
			string urlcomm="?patientid=";
			if(ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Enter 0 to use PatientNum, or 1 to use ChartNum")=="0") {
				urlcomm+=pat.PatNum.ToString();
			}
			else {
				urlcomm+=Tidy(pat.ChartNumber);
			}
			//Nearly always tidy the names in one way or another
			urlcomm+="&lastname="+Tidy(pat.LName);
			urlcomm+="&firstname="+Tidy(pat.FName);
			//This patterns shows a way to handle gender unknown when gender is optional.
			if(pat.Gender==PatientGender.Female) {
				urlcomm+="&gender=Female";
			}
			else if(pat.Gender==PatientGender.Male) {
				urlcomm+="&gender=Male";
			}
			else if(pat.Gender==PatientGender.Unknown){
				urlcomm+="&gender=Other";
			}
			if(pat.Birthdate.Year>1880) {
				urlcomm+="&birthdate="+pat.Birthdate.ToString("MM/dd/yyyy");
			}
			try {
				Process.Start(path+urlcomm);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		///<summary>Removes ampersands and number symbols.</summary>
		private static string Tidy(string input) {
			string retVal=input.Replace("&","");//get rid of any ampersands.
			retVal=retVal.Replace("#","");//get rid of any number signs.
			return retVal;
		}

	}
}







