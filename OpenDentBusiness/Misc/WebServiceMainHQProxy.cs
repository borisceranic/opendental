using System;
using System.Text;
using System.Xml;

namespace OpenDentBusiness {
	public static class WebServiceMainHQProxy {

		///<summary>Get an instance of the WebServicesHQ web service which includes the URL (pulled from PrefC). 
		///Optionally, you can provide the URL. This option should only be used by web apps which don't want to cause a call to PrefC.</summary>
		public static WebServiceMainHQ.WebServiceMainHQ GetWebServiceMainHQInstance(string webServiceHqUrl="") {
			WebServiceMainHQ.WebServiceMainHQ service=new WebServiceMainHQ.WebServiceMainHQ();
			if(string.IsNullOrEmpty(webServiceHqUrl)) { //Default to the production URL.				
				service.Url=PrefC.GetString(PrefName.WebServiceHQServerURL);
			}
			else { //URL was provided so use that.
				service.Url=webServiceHqUrl;
			}
#if DEBUG
			//Change arguments for debug only.
			//service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//localhost
			//service.Url="http://10.10.2.18:55018/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//Sam's Computer
			//service.Url="http://server184:49999/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//Sam's Computer
			service.Timeout=(int)TimeSpan.FromMinutes(60).TotalMilliseconds;
#endif
			return service;
		}


		///<summary>Any calls to WebServiceMainHQ must go through this method. The payload created here will be digested and extracted to OpenDentalWebServiceHQ.PayloadArgs.</summary>
		/// <param name="payloadContentxAsXml">Use CreateXmlWriterSettings(true) to create your payload xml. Outer-most xml element MUST be labeled 'Payload'.</param>
		/// <param name="serviceCode">Used on case by case basis to validate that customer is registered for the given service.</param>
		public static string CreateWebServiceHQPayload(string payloadContentxAsXml,eServiceCode serviceCode) {
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,CreateXmlWriterSettings(false))) {
				writer.WriteStartElement("Request");
				writer.WriteStartElement("Credentials");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticeTitle");
				writer.WriteString(PrefC.GetString(PrefName.PracticeTitle));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticePhone");
				writer.WriteString(PrefC.GetString(PrefName.PracticePhone));
				writer.WriteEndElement();
				writer.WriteStartElement("ProgramVersion");
				writer.WriteString(PrefC.GetString(PrefName.ProgramVersion));
				writer.WriteEndElement();
				writer.WriteStartElement("ServiceCode");
				writer.WriteString(serviceCode.ToString());
				writer.WriteEndElement();
				writer.WriteEndElement(); //Credentials
				writer.WriteRaw(payloadContentxAsXml);
				writer.WriteEndElement(); //Request
			}
			return strbuild.ToString();
		}

		public static XmlWriterSettings CreateXmlWriterSettings(bool omitXmlDeclaration) {
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			settings.OmitXmlDeclaration=omitXmlDeclaration;
			return settings;
		}
	}


}
