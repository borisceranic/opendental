using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace OpenDentServer {
	[RunInstallerAttribute(true)]
	public class MyProjectInstaller:Installer {
		private ServiceInstaller serviceInstaller1;
		private ServiceProcessInstaller processInstaller;

		public MyProjectInstaller() {
			processInstaller = new ServiceProcessInstaller();
			serviceInstaller1 = new ServiceInstaller();
			processInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller1.StartType = ServiceStartMode.Automatic;
      //new strategy for setting the service name:
			if(Context.Parameters.ContainsKey("ServiceName")) {//if InstallUtil.exe was used with /ServiceName=xyz parameter 
				serviceInstaller1.ServiceName=Context.Parameters["ServiceName"];
			}
			else {
				serviceInstaller1.ServiceName="OpenDentHL7";
			}
			Installers.Add(serviceInstaller1);
			Installers.Add(processInstaller);
		}
	}
}
