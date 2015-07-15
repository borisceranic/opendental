using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.Bridges {
	///<summary>RESTful bridge to podium service. Without using REST Sharp or JSON libraries this code might not work properly.</summary>
	public class Podium {

		///<summary></summary>
		public Podium(){
			
		}

		///<summary>Tries each of the phone numbers provided in the list one at a time until it succeeds.</summary>
		public static bool SendInvitation(List<string> listPhoneNumbers,string firstName,string lastName,string emailIn,bool isTest) {
			try {
				for(int i=0;i<listPhoneNumbers.Count;i++) {
					string apiUrl=""; //todo: Hard coded per api documentation
					string apiToken=""; //todo: get from program pref
					string locationId=""; //todo: get from program pref
					using(WebClientEx client=new WebClientEx()) {
						client.Headers[HttpRequestHeader.Accept]="application/json";
						client.Headers[HttpRequestHeader.ContentType]="application/json";
						client.Headers[HttpRequestHeader.Authorization]="Token token=\""+apiToken+"\"";
						client.Encoding=UnicodeEncoding.UTF8;
						string bodyJson=string.Format(@"
						{{
							""location_id"": ""{0}"",
							""phone_number"": ""{1}"",
							""customer"": {{
								""first_name"": ""{2}"",
								""last_name"": ""{3}"",
								""emailIn"": ""{4}""
							}},
							""test"": {5}
						}}",locationId,listPhoneNumbers[i],firstName,lastName,emailIn,isTest);
						//Post with Authorization headers and a body comprised of a JSON serialized anonymous type.
						client.UploadString(apiUrl,"POST",bodyJson);
						if(client.StatusCode==HttpStatusCode.OK) {
							return true;
						}
					}
				}
			}
			catch (Exception ex) {
				MessageBox.Show(Lan.g("Podium","Error sending to Podium.")+"\r\n"+ex.Message);
			}
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
			//		"emailIn": "johnny.appleseed@gmail.com"
			//	},
			//	"test": true
			//}
		}

	}

	class WebClientEx:WebClient {
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







