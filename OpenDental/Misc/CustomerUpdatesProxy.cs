using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using OpenDental.customerUpdates;
using OpenDentBusiness;

namespace OpenDental {
	public class CustomerUpdatesProxy {

		/// <summary>Get an instance of OpenDental.customerUpdates.Service1 (referred to as 'Customer Updates Web Service'. Also sets IWebProxy and ICredentials if specified for this customer. Service1 is ready to use on return.</summary>
		public static Service1 GetWebServiceInstance() {
			Service1 ws=new Service1();
			ws.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			//Uncomment this block if you want to test new web service funcionality on localhost. 
			//Use .\Development\Shared Projects Subversion\WebServiceCustomerUpdates solution to attach debugger to process 'ASP .NET Development Server - Port 3824'.
//#if DEBUG
//			ws.Url=@"http://localhost:3824/Service1.asmx";
//			ws.Timeout=(int)TimeSpan.FromMinutes(20).TotalMilliseconds;
//#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress) !="") {
				IWebProxy proxy = new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				ws.Proxy=proxy;
			}
			return ws;
		}

		/// <summary>Throws exception if anything fails. Exception should typically be caught and shown as MessageBox.
		/// Returns OD-Hosted URL for this customer's registration key.  Supported hostedService inputs: PatientPortal, MobileWeb.</summary>
		public static string GetHostedURL(eServiceCode eService) {
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("Input");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement(); //RegistrationKey
				writer.WriteStartElement("eService");
				writer.WriteString(eService.ToString());
				writer.WriteEndElement(); //ODHostedService					
				writer.WriteEndElement(); //Input					
			}
			Service1 ws=GetWebServiceInstance();
			string result=ws.RequestPatientPortalURL(strbuild.ToString());//may throw error
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//URL");
			if(node==null || string.IsNullOrEmpty(node.InnerText)) {
				throw new Exception("URL node not found");
			}
			return node.InnerText;			
		}
	}
}
