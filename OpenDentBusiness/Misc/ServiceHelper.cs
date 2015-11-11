using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Threading;

namespace OpenDentBusiness {
	public class ServiceHelper {

		public static bool Install(string serviceName,FileInfo fileInfo) {
			try {
				Process process=new Process();
				process.StartInfo.WorkingDirectory=fileInfo.DirectoryName;
				process.StartInfo.FileName=Path.Combine(Directory.GetCurrentDirectory(),"installutil.exe");
				//new strategy for having control over servicename
				//InstallUtil /ServiceName=OpenDentHL7_abc OpenDentHL7.exe
				process.StartInfo.Arguments="/ServiceName="+serviceName+" "+fileInfo.Name;
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

		public static bool Uninstall(ServiceController service) {
			try {
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+service.ServiceName);
				//Can be null but we don't care because if anything in the try-catch fails we wont set the preference for the new listener to true.
				string imagePath=hklm.GetValue("ImagePath").ToString().Replace("\"","");
				FileInfo serviceFile=new FileInfo(imagePath);
				Process process=new Process();
				process.StartInfo.WorkingDirectory=serviceFile.DirectoryName;
				process.StartInfo.FileName=Path.Combine(Directory.GetCurrentDirectory(),"installutil.exe");
				process.StartInfo.Arguments="/u /ServiceName="+service.ServiceName+" "+serviceFile.Name;
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
				string eConnectorExePath=Path.Combine(Directory.GetCurrentDirectory(),"OpenDentalEConnector","OpenDentalEConnector.exe");
				FileInfo eConnectorExeFI=new FileInfo(eConnectorExePath);
				if(!Install("OpenDentalEConnector",eConnectorExeFI)) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServiceHelper","Unable to install the eConnector service."));
					}
					return false;
				}
				return true;
			}
			catch(Exception ex) {
				if(!isSilent) {
					MessageBox.Show(Lans.g("ServiceHelper","Failed upgrading to the eConnector service:")+"\r\n"+ex.Message);
				}
				return false;
			}
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
