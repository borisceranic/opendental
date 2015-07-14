using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.Bridges{

	///<summary>RESTful bridge to podium service.</summary>
	public class Podium {

		///<summary></summary>
		public Podium(){
			
		}

		public static void SendInvitation(string phoneNumber,string firstName,string lastName,string emailIn,bool isTest) {
			try {
				string apiUrl=""; //todo: get from program pref
				string apiToken=""; //todo: get from program pref
				string locationId=""; //todo: get from program pref
				using(WebClient client=new WebClient()) {
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
					}}",locationId,phoneNumber,firstName,lastName,emailIn,isTest);
					//Post with Authorization headers and a body comprised of a JSON serialized anonymous type.
					client.UploadString(apiUrl,"POST",bodyJson);
				}
			}
			catch (Exception ex) {
				MessageBox.Show("Error sending to Podium.\r\n"+ex.Message);
			}
			//Sample Request:

			//Accept: 'application/json'
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
}







