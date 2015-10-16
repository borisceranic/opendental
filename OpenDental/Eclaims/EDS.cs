using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Eclaims {
	public class EDS {

		///<summary></summary>
		public EDS() {

		}

		///<summary>Sends an X12 270 request and returns X12 271 response or an error message.</summary>
		public static string Benefits270(Clearinghouse clearinghouseClin,string x12message) {//called from x270Controller. Clinic-level clearinghouse passed in.
			string retVal="";
			try {
				HttpWebRequest webReq;
				WebResponse webResponseXml;
				//Production URL.  For testing, set username to 'test' and password to 'test'.
				//When the username and password are both set to 'test', the X12 270 request will be ignored and just the transmission will be verified.
				webReq=(HttpWebRequest)WebRequest.Create("https://web2.edsedi.com/eds/Transmit_Request");
				webReq.KeepAlive=false;
				webReq.Method="POST";
				webReq.ContentType="text/xml";
				string postDataXml="<?xml version=\"1.0\" encoding=\"us-ascii\"?>"
					+"<content>"
						+"<header>"
							+"<userId>"+clearinghouseClin.LoginID+"</userId>"
							+"<pass>"+clearinghouseClin.Password+"</pass>"
							+"<process>transmitEligibility</process>"
							+"<version>1</version>"
						+"</header>"
						+"<body>"
							+"<type>EDI</type>"//Can only be EDI
							+"<data>"+x12message+"</data>"
							+"<returnType>EDI</returnType>"//Can be EDI, HTML, or EDI.HTML, but should mimic the above type
						+"</body>"
					+"</content>";
				ASCIIEncoding encoding=new ASCIIEncoding();
				byte[] arrayXmlBytes=encoding.GetBytes(postDataXml);
				Stream streamOut=webReq.GetRequestStream();
				streamOut.Write(arrayXmlBytes,0,arrayXmlBytes.Length);
				streamOut.Close();
				webResponseXml=webReq.GetResponse();
				//Process the response
				StreamReader readStream=new StreamReader(webResponseXml.GetResponseStream(),Encoding.ASCII);
				string responseXml=readStream.ReadToEnd();
				readStream.Close();
				XmlDocument xmlDoc=new XmlDocument();
				xmlDoc.LoadXml(responseXml);
				XmlNode nodeErrorCode=xmlDoc.SelectSingleNode(@"content/body/ERROR_CODE");
				if(nodeErrorCode!=null && nodeErrorCode.InnerText.ToString()!="0") {
					throw new Exception("Error Code: "+nodeErrorCode+" - "+xmlDoc.SelectSingleNode(@"content/body/ERROR_MSG").InnerText.ToString());
				}
				retVal=xmlDoc.SelectSingleNode(@"content/body/ediData").InnerText.ToString();
			}
			catch(Exception e) {
				retVal=e.Message;
			}
			return retVal;
		}

	}

}
