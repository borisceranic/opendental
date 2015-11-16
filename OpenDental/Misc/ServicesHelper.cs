using CodeBase;
using Microsoft.Win32;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows;
using System.Xml;

namespace OpenDental {
	///<summary>This is a helper class meant to be used by Open Dental to easily manage Windows services.
	///It is flagged as internal because the logic within this class requires valid paths relative to the OpenDental.exe (../Install dir)</summary>
	internal class ServicesHelper {
		///<summary>[CurrentDirectory]/InstallUtil/installutil.exe</summary>
		private static string _installUtilPath=Path.Combine(Directory.GetCurrentDirectory(),"InstallUtil","installutil.exe");

		///<summary>Returns true if the service was installed successfully.</summary>
		public static bool Install(string serviceName,FileInfo fileInfo) {
			try {
				Process process=new Process();
				process.StartInfo.FileName=_installUtilPath;
				//new strategy for having control over servicename
				//InstallUtil /ServiceName=OpenDentHL7_abc OpenDentHL7.exe
				process.StartInfo.Arguments="/ServiceName="+serviceName+" \""+fileInfo.FullName+"\"";
				process.Start();
				process.WaitForExit(10000);
				//Check to see if the service was successfully added.
				List<ServiceController> listServices=ServiceController.GetServices().ToList();
				if(!listServices.Exists(x => x.ServiceName==serviceName)) {
					return false;//The installutil.exe ran correctly (did not error out) but the service was not actually installed.
				}
				return true;
			}
			catch {
				//Do nothing.  The bool was already set accordingly, so whatever it is will be what we want to return.
			}
			return false;
		}

		///<summary>Returns true if the service was able to start successfully.</summary>
		public static bool Start(ServiceController service) {
			try {
				service.MachineName=Environment.MachineName;
				service.Start();
				service.WaitForStatus(ServiceControllerStatus.Running,new TimeSpan(0,0,7));
			}
			catch(Exception) {
				return false;
			}
			return true;
		}

		///<summary>Returns true if the service was able to uninstall successfully.</summary>
		public static bool Uninstall(ServiceController service) {
			try {
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+service.ServiceName);
				//Can be null but we don't care because if anything in the try-catch fails we wont set the preference for the new listener to true.
				string imagePath=hklm.GetValue("ImagePath").ToString().Replace("\"","");
				FileInfo serviceFile=new FileInfo(imagePath);
				Process process=new Process();
				process.StartInfo.WorkingDirectory=serviceFile.DirectoryName;
				process.StartInfo.FileName=_installUtilPath;
				process.StartInfo.Arguments="/u /ServiceName="+service.ServiceName+" "+serviceFile.FullName;
				process.Start();
				process.WaitForExit(10000);//Wait 10 seconds to give the user's computer opportunity to process the uninstall.
				//Check to see if the service was successfully removed.
				List<ServiceController> listServices=ServiceController.GetServices().ToList();
				if(listServices.Exists(x => x.ServiceName==service.ServiceName)) {//This might be a false positive if two services share the same name.
					return false;//The installutil.exe ran correctly (did not error out) but the service was not actually uninstalled.
				}
				return true;
			}
			catch {
				//Do nothing.  Something went wrong uninstalling the service.
			}
			return false;
		}

		///<summary>Checks to see any OpenDentalCustListener services are currently installed.
		///If present, each CustListener service will be uninstalled.
		///After successfully removing all CustListener services, one eConnector service will be installed.
		///Returns true if the CustListener service was successfully upgraded to the eConnector service.</summary>
		///<param name="isSilent">Set to false to throw meaningful exceptions to display to the user, otherwise fails silently.</param>
		///<param name="isListening">Will get set to true if the customer was previously using the CustListener service.</param>
		///<returns>True if only one CustListener services present and was successfully uninstalled along with the eConnector service getting installed.
		///False if more than one CustListener service is present or the eConnector service could not install.</returns>
		public static bool UpgradeOrInstallEConnector(bool isSilent,out bool isListening) {
			isListening=false;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				if(!isSilent) {
					MsgBox.Show("ServicesHelper","Not allowed to install services when using the middle tier.");
				}
				return false;
			}
			try {
				//Check to see if CustListener service is installed and needs to be uninstalled.
				List<ServiceController> listCustListenerServices=GetServicesByExe("OpenDentalCustListener.exe");
				if(listCustListenerServices.Count>0) {
					isListening=true;
				}
				if(listCustListenerServices.Count==1) {//Only uninstall the listener service if there is exactly one found.  This is just a nicety.
					ServiceController custListenerService=listCustListenerServices[0];
					if(custListenerService.Status==ServiceControllerStatus.Running) {
						custListenerService.Stop();
					}
					if(!Uninstall(custListenerService)) {
						//Do nothing.  We want to try to install the eConnector service anyway.
					}
				}
				List<ServiceController> listEConnectorServices=GetServicesByExe("OpenDentalEConnector.exe");
				if(listEConnectorServices.Count>0) {
					return true;//An eConnector service is already installed.
				}
				string eConnectorExePath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalEConnector","OpenDentalEConnector.exe");
				FileInfo eConnectorExeFI=new FileInfo(eConnectorExePath);
				if(!Install("OpenDentalEConnector",eConnectorExeFI)) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServicesHelper","Unable to install the service."));
					}
					return false;
				}
				//Create a new OpenDentalWebConfig.xml file for the eConnector if one is not already present.
				if(!CreateConfigForEConnector(isListening)) {
					throw new ApplicationException(Lans.g("ServicesHelper","The config file could not be created."));
				}
				//Now that the service has finally installed we need to try and start it.
				listEConnectorServices=GetServicesByExe("OpenDentalEConnector.exe");
				if(listEConnectorServices.Count < 1) {
					throw new ApplicationException(Lans.g("ServicesHelper","Service could not be found in order to automatically start."));
				}
				if(!Start(listEConnectorServices[0])) {
					throw new ApplicationException(Lans.g("ServicesHelper","Unable to start the service."));
				}
				return true;
			}
			catch(Exception ex) {
				if(!isSilent) {
					MessageBox.Show(Lans.g("ServicesHelper","Failed upgrading to the eConnector service:")+"\r\n"+ex.Message);
				}
				return false;
			}
		}

		///<summary>Creates a default OpenDentalWebConfig.xml file for the eConnector if one is not already present.
		///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
		///Users should not be installing the eConnector via the middle tier.</summary>
		private static bool CreateConfigForEConnector(bool isListening) {
			string eConnectorConfigPath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalEConnector","OpenDentalWebConfig.xml");
			string custListenerConfigPath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalCustListener","OpenDentalWebConfig.xml");
			//Check to see if there is already a config file present.
			if(File.Exists(eConnectorConfigPath)) {
				return true;//Nothing to do.
			}
			//At this point we know that the eConnector does not have a config file present.
			//Check to see if the user is currently using the CustListener service.
			if(isListening) {
				//Try and grab a copy of the CustListener service config file first.
				if(File.Exists(custListenerConfigPath)) {
					try {
						File.Copy(custListenerConfigPath,"",false);
						//If we got to this point the copy was successful and now the eConnector has a valid config file.
						return true;
					}
					catch(Exception) {
						//The copy didn't work for some reason.  Simply try to create a new file in the eConnector directory.
					}
				}
			}
			XmlDocument document=new XmlDocument();
			//Creating Nodes
			XmlNode nodeConnSettings=document.CreateNode(XmlNodeType.Element,"ConnectionSettings","");
			XmlNode nodeDbeConn=document.CreateNode(XmlNodeType.Element,"DatabaseConnection","");
			XmlNode nodeCompName=document.CreateNode(XmlNodeType.Element,"ComputerName","");
			nodeCompName.InnerText=DataConnection.GetServerName();
			XmlNode nodeDatabase=document.CreateNode(XmlNodeType.Element,"Database","");
			nodeDatabase.InnerText=DataConnection.GetDatabaseName();
			XmlNode nodeUser=document.CreateNode(XmlNodeType.Element,"User","");
			nodeUser.InnerText=DataConnection.GetMysqlUser();
			XmlNode nodePassword=document.CreateNode(XmlNodeType.Element,"Password","");
			nodePassword.InnerText=DataConnection.GetMysqlPass();
			XmlNode nodeUserLow=document.CreateNode(XmlNodeType.Element,"UserLow","");
			nodeUserLow.InnerText=DataConnection.GetMysqlUserLow();
			XmlNode nodePasswordLow=document.CreateNode(XmlNodeType.Element,"PasswordLow","");
			nodePasswordLow.InnerText=DataConnection.GetMysqlPassLow();
			XmlNode nodeDbType=document.CreateNode(XmlNodeType.Element,"DatabaseType","");
			nodeDbType.InnerText="MySql";//Not going to support Oracle until someone complains.
			XmlNode nodeLogLevelOfApp=document.CreateNode(XmlNodeType.Element,"LogLevelOfApplication","");
			nodeLogLevelOfApp.InnerText="Error";
			//Assigning Structure
			nodeDbeConn.AppendChild(nodeCompName);
			nodeDbeConn.AppendChild(nodeDatabase);
			nodeDbeConn.AppendChild(nodeUser);
			nodeDbeConn.AppendChild(nodePassword);
			nodeDbeConn.AppendChild(nodeUserLow);
			nodeDbeConn.AppendChild(nodePasswordLow);
			nodeDbeConn.AppendChild(nodeDbType);
			nodeConnSettings.AppendChild(nodeDbeConn);
			nodeConnSettings.AppendChild(nodeLogLevelOfApp);
			document.AppendChild(nodeConnSettings);
			//Outputting completed XML document
			StringBuilder strb=new StringBuilder();
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars="   ";
			settings.NewLineChars="\r\n";
			settings.OmitXmlDeclaration=true;
			try {
				using(XmlWriter xmlWriter=XmlWriter.Create(strb,settings)) {
					document.WriteTo(xmlWriter);
					xmlWriter.Flush();
					File.WriteAllText(eConnectorConfigPath,strb.ToString());
				}
			}
			catch {
				return false;
			}
			return true;
		}

		///<summary>Returns all services that their "Path to executeable" contains the passed in executable name.</summary>
		///<param name="exeName">E.g. OpenDentalCustListener.exe</param>
		public static List<ServiceController> GetServicesByExe(string exeName) {
			RegistryKey hklm;
			List<ServiceController> retVal=new List<ServiceController>();
			List<ServiceController> listServices=ServiceController.GetServices().ToList();
			foreach(ServiceController serviceCur in listServices) {
				hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(Path.Combine(@"System\CurrentControlSet\Services\",serviceCur.ServiceName));
				if(hklm.GetValue("ImagePath")==null) {
					continue;
				}
				string installedServicePath=hklm.GetValue("ImagePath").ToString().Replace("\"","");
				if(installedServicePath.Contains(exeName)) {
					retVal.Add(serviceCur);
				}
			}
			return retVal;
		}

		///<summary>Returns one service that has "Path to executeable" set to the full path passed in.  Returns null if not found.</summary>
		///<param name="exeFullPath">E.g. C:\Program Files(x86)\Open Dental\OpenDentalCustListener\OpenDentalCustListener.exe</param>
		public static ServiceController GetServiceByExeFullPath(string exeFullPath) {
			return GetServicesByExe(exeFullPath).FirstOrDefault();
		}

	}
}
