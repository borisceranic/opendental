using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Linq;

namespace OpenDental.Bridges {
	///<summary>RESTful bridge to podium service. Without using REST Sharp or JSON libraries this code might not work properly.</summary>
	public class Podium {

		public static DateTime DateTimeLastRan=DateTime.MinValue;

		///<summary></summary>
		public Podium() {

		}

		///<summary></summary>
		public static void ShowPage() {
			try {
				if(Programs.IsEnabled(ProgramName.Podium)) {
					Process.Start("http://www.opendental.com/manual/podiumdashboard.html");
				}
				else {
					Process.Start("http://www.opendental.com/manual/podiumod.html");
				}
			}
			catch {
				MsgBox.Show("Podium","Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet then try again.");
			}
		}

		///<summary>Tries each of the phone numbers provided in the list one at a time until it succeeds.</summary>
		public static bool SendInvitation(Patient pat,bool isNew,bool isTest=false) {
			List<string> listPhoneNumbers=new List<string>() { pat.WirelessPhone,pat.HmPhone };
			string firstName=pat.FName;
			string lastName=pat.LName;
			string emailIn=pat.Email;
			string isTestString="false";
			int statusCode=200;
			if(isTest) {
				isTestString="true";
			}
			for(int i=0;i<listPhoneNumbers.Count;i++) {
				string phoneNumber=new string(listPhoneNumbers[i].Where(x => char.IsDigit(x)).ToArray());
				if(phoneNumber=="") {
					continue;
				}
				string apiUrl="https://podium.co/api/v1/review_invitations";
				string apiToken=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.Podium),"Enter your API Token (required)");
				string locationId=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.Podium),"Enter your Location ID (required)");
				try {
					using(WebClientEx client=new WebClientEx()) {
						client.Headers[HttpRequestHeader.Accept]="application/json";
						client.Headers[HttpRequestHeader.ContentType]="application/json";
						client.Headers[HttpRequestHeader.Authorization]="Token token=\""+apiToken+"\"";
						client.Encoding=UnicodeEncoding.UTF8;
						string bodyJson=string.Format(@"
						{{
							""location_id"": ""{0}"",
							""phone_number"": ""{1}"",
							""customer"":  {{
								""first_name"": ""{2}"",
								""last_name"": ""{3}"",
								""email"": ""{4}""
							}},
							""test"": {5}
						}}",locationId,phoneNumber,firstName,lastName,emailIn,isTestString);
						//Post with Authorization headers and a body comprised of a JSON serialized anonymous type.
						client.UploadString(apiUrl,"POST",bodyJson);
						if(client.StatusCode==HttpStatusCode.OK) {
							MakeCommlog(pat,phoneNumber,statusCode);
							return true;
						}
						else {
							//eventlogging should also go here for non 200 status.
						}
					}
				}
				catch(Exception ex) {
					if(ex is WebException) {
						statusCode=(int)((HttpWebResponse)((WebException)ex).Response).StatusCode;
					}
					//Do nothing because a verbose commlog will be made below if all phone numbers fail.
				}
			}
			MakeCommlog(pat,"",statusCode);
			//explicitly failed or did not succeed.
			return false;
			//Sample Request:

			//Accept: 'application/json's
			//Content-Type: 'application/json'
			//Authorization: 'Token token="my_dummy_token"'
			//Body:
			//{
			//	"location_id": "54321",
			//	"phone_number": "1234567890",
			//	"customer": {
			//		"first_name": "Johnny",
			//		"last_name": "Appleseed",
			//		"email": "johnny.appleseed@gmail.com"
			//	},
			//	"test": true
			//}
			//NOTE:  There will never be a value after "customer": although it was initially interpreted that there would be a "new" flag there.
		}

		private static void MakeCommlog(Patient pat,string phoneNumber,int statusCode) {
			string commText=Lan.g("Podium","Podium review invitation request successfully sent")+". \r\n";
			if(statusCode!=200) {
				commText=Lan.g("Podium","Podium review invitation request failed to send.");
				if(statusCode==422) {//422 is Unprocessable Entity, which is sent in this case when a phone number has received an invite already.
					commText+="  "+Lan.g("Podium","The request failed because an identical request was previously sent.");
				}
				commText+="\r\n";
			}
			commText+=Lan.g("Podium","The information sent in the request was")+": \r\n"
					+"First name: \""+pat.FName+"\", Last name: \""+pat.LName+"\", Email: \""+pat.Email+"\"";
			if(phoneNumber!="") {//If successful.
				commText+=", Phone number: \""+phoneNumber+"\"";
			}
			else {
				string wirelessPhone=new string(pat.WirelessPhone.Where(x => char.IsDigit(x)).ToArray());
				string homePhone=new string(pat.HmPhone.Where(x => char.IsDigit(x)).ToArray());
				List<string> phonesTried=new List<string> { wirelessPhone,homePhone }.FindAll(x => x!="");
				string phoneNumbersTried=", No valid phone number found.";
				if(phonesTried.Count>0) {
					phoneNumbersTried=", "+Lan.g("Podium","Phone numbers tried")+": "+string.Join(", ",phonesTried);
				}
				commText+=phoneNumbersTried;
			}
			Commlog commlogCur=new Commlog();
			commlogCur.CommDateTime=DateTime.Now;
			commlogCur.DateTimeEnd=DateTime.Now;
			commlogCur.PatNum=pat.PatNum;
			commlogCur.UserNum=0;//run from server, no valid CurUser
			commlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			commlogCur.Note=commText;
			Commlogs.Insert(commlogCur);
		}

		private class WebClientEx:WebClient {
			//http://stackoverflow.com/questions/3574659/how-to-get-status-code-from-webclient
			private WebResponse _mResp = null;

			protected override WebResponse GetWebResponse(WebRequest req,IAsyncResult ar) {
				return _mResp = base.GetWebResponse(req,ar);
			}

			public HttpStatusCode StatusCode {
				get {
					HttpWebResponse httpWebResponse=_mResp as HttpWebResponse;
					return httpWebResponse!=null?httpWebResponse.StatusCode:HttpStatusCode.OK;
				}
			}
		}

	}
}







