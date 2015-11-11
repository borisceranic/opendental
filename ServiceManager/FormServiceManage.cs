using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenDentBusiness;


namespace ServiceManager {
	public partial class FormServiceManage:Form {
		///<summary>Indicates if a service was successfully installed while the service manager was showing.</summary>
		public bool HadServiceInstalled=false;
		private bool _isInstallOnly=false;

		private FileInfo _serviceFile {
			get {
				if(File.Exists(textPathToExe.Text)) {
					return new FileInfo(textPathToExe.Text);
				}
				return null;
			}
		}
		
		///<summary>Pass in empty string to create a new service. Pass in OpenDent string to manage an existing service.</summary>
		public FormServiceManage(string serviceName,bool isInstallOnly) {
			InitializeComponent();
			textName.Text=serviceName;
			textPathToExe.Text=Directory.GetCurrentDirectory();
			_isInstallOnly=isInstallOnly;
		}

		public static List<ServiceController> GetAllOpenDentServices() {
			List<ServiceController> serviceControllerList=new List<ServiceController>();
			ServiceController[] serviceControllersAll=ServiceController.GetServices();
			for(int i=0;i<serviceControllersAll.Length;i++) {
				if(serviceControllersAll[i].ServiceName.StartsWith("OpenDent")) {
					serviceControllerList.Add(serviceControllersAll[i]);
				}
			}
			return serviceControllerList;
		}

		private static ServiceController GetOpenDentServiceByName(string serviceName) {
			List<ServiceController> serviceControllersOpenDent=GetAllOpenDentServices();
			for(int i=0;i<serviceControllersOpenDent.Count;i++) {
				if(serviceControllersOpenDent[i].ServiceName==serviceName) {
					return serviceControllersOpenDent[i];
				}
			}
			return null;
		}

		private void FormServiceManager_Load(object sender,EventArgs e) {
			ServiceController service=GetOpenDentServiceByName(textName.Text);			
			if(service!=null) {//installed
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+service.ServiceName);
				textPathToExe.Text=hklm.GetValue("ImagePath").ToString().Replace("\"","");
				textStatus.Text="Installed";
				butInstall.Enabled=false;
				butUninstall.Enabled=true;
				butBrowse.Enabled=false;
				textPathToExe.ReadOnly=true;
				textName.ReadOnly=true;
				if(service.Status==ServiceControllerStatus.Running) {
					textStatus.Text+=", Running";
					butStart.Enabled=false;
					butStop.Enabled=true;
				}
				else {
					textStatus.Text+=", Stopped";
					butStart.Enabled=true;
					butStop.Enabled=false;
				}
			}
			else {
				textStatus.Text="Not installed";
				textName.ReadOnly=false;
				textPathToExe.ReadOnly=false;
				butInstall.Enabled=true;
				butUninstall.Enabled=false;
				butStart.Enabled=false;
				butStop.Enabled=false;
			}
			if(_isInstallOnly) {
				butUninstall.Enabled=false;
			}
		}

		private void butInstall_Click(object sender,EventArgs e) {
			if(_serviceFile==null) {
				MessageBox.Show("Select a valid service path");
				return;
			}
			if(textName.Text.Length<8 || textName.Text.Substring(0,8)!="OpenDent") {
				MessageBox.Show("Error.  Service name must begin with \"OpenDent\".");
				return;
			}
			List<ServiceController> allOpenDentServices=GetAllOpenDentServices();
			for(int i=0;i<allOpenDentServices.Count;i++) { //create list of all OpenDent service install paths to ensure only one service can be installed from each directory				
				if(textName.Text==allOpenDentServices[i].ServiceName) {
					MessageBox.Show("Error.  A service with this name is already installed.  Names must be unique.");
					return;
				}
				RegistryKey hklm=Registry.LocalMachine;
				hklm=hklm.OpenSubKey(@"System\CurrentControlSet\Services\"+allOpenDentServices[i].ServiceName);
				string installedServicePath=hklm.GetValue("ImagePath").ToString().Replace("\"","");
				if(installedServicePath==_serviceFile.FullName) {
					MessageBox.Show("Error.  Cannot install service.  This service is already installed from this directory.");
					return;
				}
			}
			if(_serviceFile.Name=="OpenDentalEConnector.exe") {
				FormWebConfigSettings FormWCS=new FormWebConfigSettings(_serviceFile);
				FormWCS.ShowDialog();
				if(FormWCS.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			if(ServiceHelper.Install(textName.Text,_serviceFile)) {
				HadServiceInstalled=true;
			}
			butRefresh_Click(this,e);
		}

		private void butUninstall_Click(object sender,EventArgs e) {
			if(_serviceFile==null) {
				MessageBox.Show("Selected service has an invalid path");
				return;
			}
			ServiceController service=ServiceHelper.GetServiceByExeFullPath(_serviceFile.FullName);
			if(service!=null) {
				if(!ServiceHelper.Uninstall(service)) {
					MessageBox.Show("The service was not successfully uninstalled.  This may be a permissions issue.");
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butStart_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			try {
				ServiceController service=new ServiceController(textName.Text);
				service.MachineName=Environment.MachineName;
				service.Start();
				service.WaitForStatus(ServiceControllerStatus.Running,new TimeSpan(0,0,7));
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
			Cursor=Cursors.Default;
			butRefresh_Click(this,e);
		}

		private void butStop_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			try {
				ServiceController service=GetOpenDentServiceByName(textName.Text);
				if(service==null) {
					return;
				}
				service.Stop();
				service.WaitForStatus(ServiceControllerStatus.Stopped,new TimeSpan(0,0,7));
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
			finally {
				Cursor=Cursors.Default;			
			}
			butRefresh_Click(this,e);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FormServiceManager_Load(this,e);
		}
				
		private void butBrowse_Click(object sender,EventArgs e) {
			OpenFileDialog fdlg=new OpenFileDialog();
			fdlg.Title="Select a Service";
			fdlg.InitialDirectory=Directory.GetCurrentDirectory();
			fdlg.Filter="Executable files(*.exe)|*.exe";
			fdlg.RestoreDirectory=true;
			if(fdlg.ShowDialog()!=DialogResult.OK) {
				return;				
			}
			textPathToExe.Text=fdlg.FileName;
		}
	}
}
