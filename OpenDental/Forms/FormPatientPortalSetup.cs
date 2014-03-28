using System;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Net;
using System.Xml;

namespace OpenDental {
	public partial class FormPatientPortalSetup:Form {
		public FormPatientPortalSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPatientPortalSetup_Load(object sender,EventArgs e) {
			textPatientPortalURL.Text=PrefC.GetString(PrefName.PatientPortalURL);
			textBoxNotificationSubject.Text=PrefC.GetString(PrefName.PatientPortalNotifySubject);
			textBoxNotificationBody.Text=PrefC.GetString(PrefName.PatientPortalNotifyBody);
			textListenerPort.Text=PrefC.GetString(PrefName.CustListenerPort);
			if(!Security.IsAuthorized(Permissions.Setup)) {
				butOK.Enabled=false;
				butGetURL.Enabled=false;
				groupBoxNotification.Enabled=false;
			}
		}

		private void butGetURL_Click(object sender,EventArgs e) {
			try {
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = ("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("RegistrationKey");
					writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
					writer.WriteEndElement();
				}
				OpenDental.customerUpdates.Service1 portalService=new OpenDental.customerUpdates.Service1();			
				portalService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
				if(PrefC.GetString(PrefName.UpdateWebProxyAddress) !="") {
					IWebProxy proxy = new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
					ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
					proxy.Credentials=cred;
					portalService.Proxy=proxy;
				}
				string result=portalService.RequestPatientPortalURL(strbuild.ToString());//may throw error
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(result);
				XmlNode node=doc.SelectSingleNode("//Error");
				if(node!=null) {
					MessageBox.Show(node.InnerText);
					return;
				}
				node=doc.SelectSingleNode("//URL");
				if(node==null || string.IsNullOrEmpty(node.InnerText)) {
					MsgBox.Show(this,"URL node not found");
					return;
				}
				textOpenDentalURl.Text=node.InnerText;
				if(textPatientPortalURL.Text=="") {
					textPatientPortalURL.Text=node.InnerText;
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		
		private void butOK_Click(object sender,EventArgs e) {
#if !DEBUG
			if(!textPatientPortalURL.Text.ToUpper().StartsWith("HTTPS")) {
				MsgBox.Show(this,"Patient Portal URL must start with HTTPS.");
				return;
			}
#endif
			if(textListenerPort.errorProvider1.GetError(textListenerPort)!="") {
				MessageBox.Show(Lan.g(this,"Listener Port must be a number between 0-65535."));
				return;
			}
			if(textBoxNotificationSubject.Text=="") {
				MsgBox.Show(this,"Notification Subject is empty");
				textBoxNotificationSubject.Focus();
				return;
			}
			if(textBoxNotificationBody.Text=="") {
				MsgBox.Show(this,"Notification Body is empty");
				textBoxNotificationBody.Focus();
				return;
			}
			if(!textBoxNotificationBody.Text.Contains("[URL]")) { //prompt user that they omitted the URL field but don't prevent them from continuing
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"[URL] not included in notification body. Continue without setting the [URL] field?")) {
					textBoxNotificationBody.Focus();
					return;
				}
			}
			if(Prefs.UpdateString(PrefName.PatientPortalURL,textPatientPortalURL.Text)
				| Prefs.UpdateString(PrefName.PatientPortalNotifySubject,textBoxNotificationSubject.Text)
				| Prefs.UpdateString(PrefName.PatientPortalNotifyBody,textBoxNotificationBody.Text)
				| Prefs.UpdateString(PrefName.CustListenerPort,textListenerPort.Text)) 
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}