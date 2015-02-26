using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace ServiceManager {
	public partial class FormWebConfigSettings:Form {
		private FileInfo _serviceFile;

		///<summary>Pass in the file information for the service file that is being installed.  We will use the file path to determine where to put the config file.</summary>
		public FormWebConfigSettings(FileInfo serviceFile) {
			InitializeComponent();
			_serviceFile=serviceFile;
		}

		private void FormWebConfigSettings_Load(object sender,EventArgs e) {
			string xmlPath=Path.Combine(Application.StartupPath,"FreeDentalConfig.xml");
			XmlDocument document=new XmlDocument();
			try {//Try FreeDentalConfig.xml first
				document.Load(xmlPath);
				XPathNavigator Navigator=document.CreateNavigator();
				XPathNavigator nav;
				nav=Navigator.SelectSingleNode("//DatabaseConnection");
				if(nav==null) {
					throw new Exception("DatabaseConnection element missing from FreeDentalConfig.xml, which is required.");
				}
				textServer.Text=nav.SelectSingleNode("ComputerName").Value;
				textDatabase.Text=nav.SelectSingleNode("Database").Value;
				textUser.Text=nav.SelectSingleNode("User").Value;
				textPassword.Text=nav.SelectSingleNode("Password").Value;
				textUserLow.Text="";
				textPasswordLow.Text="";
				comboLogLevel.Items.AddRange(Enum.GetNames(typeof(LogLevel)));//Isn't included in FreeDentalConfig, but is needed for the web service.
				comboLogLevel.SelectedItem=comboLogLevel.Items[0];
			}
			catch(Exception ex) {//FreeDentalConfig didn't load correctly
				textServer.Text="localhost";
				textDatabase.Text="opendental";
				textUser.Text="root";
				textPassword.Text="";
				textUserLow.Text="";
				textPasswordLow.Text="";
				comboLogLevel.Items.AddRange(Enum.GetNames(typeof(LogLevel)));
				comboLogLevel.SelectedItem=comboLogLevel.Items[0];
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			this.Close();
		}

		private void butOk_Click(object sender,EventArgs e) {
			XmlDocument document=new XmlDocument();
			if(textServer.Text=="") {
				MessageBox.Show("Cannot leave server field blank.");
				return;
			}
			if(textDatabase.Text=="") {
				MessageBox.Show("Cannot leave database field blank.");
				return;
			}
			if(textUser.Text=="") {
				MessageBox.Show("Cannot leave user field blank.");
				return;
			}
			//Creating Nodes
			XmlNode connSettings=document.CreateNode(XmlNodeType.Element,"ConnectionSettings","");
			XmlNode databaseConnection=document.CreateNode(XmlNodeType.Element,"DatabaseConnection","");
			XmlNode compName=document.CreateNode(XmlNodeType.Element,"ComputerName","");
			compName.InnerText=textServer.Text;
			XmlNode database=document.CreateNode(XmlNodeType.Element,"Database","");
			database.InnerText=textDatabase.Text;
			XmlNode user=document.CreateNode(XmlNodeType.Element,"User","");
			user.InnerText=textUser.Text;
			XmlNode password=document.CreateNode(XmlNodeType.Element,"Password","");
			password.InnerText=textPassword.Text;
			XmlNode userLow=document.CreateNode(XmlNodeType.Element,"UserLow","");
			userLow.InnerText=textUserLow.Text;
			XmlNode passwordLow=document.CreateNode(XmlNodeType.Element,"PasswordLow","");
			passwordLow.InnerText=textPasswordLow.Text;
			XmlNode dbType=document.CreateNode(XmlNodeType.Element,"DatabaseType","");
			dbType.InnerText="MySql";//Not going to support Oracle until someone complains.
			XmlNode logLevelOfApp=document.CreateNode(XmlNodeType.Element,"LogLevelOfApplication","");
			logLevelOfApp.InnerText=comboLogLevel.Items[comboLogLevel.SelectedIndex].ToString();
			//Assigning Structure
			databaseConnection.AppendChild(compName);
			databaseConnection.AppendChild(database);
			databaseConnection.AppendChild(user);
			databaseConnection.AppendChild(password);
			databaseConnection.AppendChild(userLow);
			databaseConnection.AppendChild(passwordLow);
			databaseConnection.AppendChild(dbType);
			connSettings.AppendChild(databaseConnection);
			connSettings.AppendChild(logLevelOfApp);
			document.AppendChild(connSettings);
			//Outputting completed XML document
			StringBuilder strb=new StringBuilder();
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars="   ";
			settings.NewLineChars="\r\n";
			settings.OmitXmlDeclaration=true;
			XmlWriter xmlWriter=XmlWriter.Create(strb,settings);
			document.WriteTo(xmlWriter);
			xmlWriter.Flush();
			try {
				File.WriteAllText(Path.Combine(_serviceFile.DirectoryName,"OpenDentalWebConfig.xml"),strb.ToString());
			}
			catch {
				MessageBox.Show("There was a problem writing a file to your system. Please go to (manual page) and follow instructions (numbers).");
				return;
			}
			xmlWriter.Close();
			strb.Clear();
			DialogResult=DialogResult.OK;
		}
	}
	
	///<summary>0=Error, 1=Information, 2=Verbose</summary>
	public enum LogLevel {
		///<summary>0 Logs only errors.</summary>
		Error=0,
		///<summary>1 Logs information plus errors.</summary>
		Information=1,
		///<summary>2 Most verbose form of logging (use sparingly for very specific troubleshooting). Logs all entries all the time.</summary>
		Verbose=2
	}
}
