using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace OpenDental.Bridges {
	///<summary></summary>
	public class DentalTek {

		///<summary></summary>
		public DentalTek() {
			
		}

		///<summary>Attempt to send a phone number to place a call using DentalTek.  Already surrounded in try-catch. 
		///Returns false if unsuccessful or a phone number wasn't passed in.</summary>
		public static bool PlaceCall(string phoneNumber,bool IsTest=false) {
			phoneNumber=new string(phoneNumber.Where(x => char.IsDigit(x)).ToArray());
			if(phoneNumber=="") {
				return false;
			}
			string testStr=IsTest?"&test=true":"";
			using(WebClient client = new WebClient()) {
				string response="";
				try {
					client.Headers[HttpRequestHeader.ContentType] = "application/json";
					client.Encoding=UnicodeEncoding.UTF8;
					string domainUser=System.Security.Principal.WindowsIdentity.GetCurrent().Name;
					string request="https://ivlrest.voiceelements.com/clicktocall?DomainUser="+domainUser+"&phonenumber="+phoneNumber+testStr;
					response=client.DownloadString(request);//GET
				}
				catch {
					//Can't think of anything useful to tell them about why the call attempt failed.
				}
				return response.Contains("Success");
			}
		}

		///<summary></summary>
		public static void SendData(Program ProgramCur,Patient pat) {
			string path=Programs.GetProgramPath(ProgramCur);
			if(pat==null) {
				MsgBox.Show("DentalTek","Please select a patient first.");
				return;
			}
			string phoneNumber="";
			Dictionary<ContactMethod,string> dictPrefContactMethod=new Dictionary<ContactMethod,string>();
			dictPrefContactMethod.Add(ContactMethod.HmPhone,pat.HmPhone);
			dictPrefContactMethod.Add(ContactMethod.WkPhone,pat.WkPhone);
			dictPrefContactMethod.Add(ContactMethod.WirelessPh,pat.WirelessPhone);
			if(dictPrefContactMethod.ContainsKey(pat.PreferContactMethod)) {
				phoneNumber=new string(dictPrefContactMethod[pat.PreferContactMethod].Where(x => char.IsDigit(x)).ToArray());
				PlaceCall(phoneNumber);
			}
			else {
				List<string> listPhoneNumbers=new List<string>();
				listPhoneNumbers.Add("HmPhone: "+pat.HmPhone);
				listPhoneNumbers.Add("WkPhone: "+pat.WkPhone);
				listPhoneNumbers.Add("WirelessPhone: "+pat.WirelessPhone);
				InputBox inputBox=new InputBox(Lan.g("DentalTek","Please select a phone number"),listPhoneNumbers);
				inputBox.comboSelection.SelectedIndex=0;//This could be set to always display the inputbox form to always allow users to choose the number.
				inputBox.ShowDialog();
				if(inputBox.DialogResult!=DialogResult.OK){
					return;
				}
				//Remove the titles that were added in addition to the phone numbers for UI purposes.
				phoneNumber=new string(listPhoneNumbers[inputBox.comboSelection.SelectedIndex].Where(x => char.IsDigit(x)).ToArray());
				if(!PlaceCall(phoneNumber)) {
					MsgBox.Show("DentalTek","Unable to place phone call.");
				}
			}

		}
	}
}