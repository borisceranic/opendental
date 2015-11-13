﻿using CodeBase;
using Ionic.Zip;
using Microsoft.Win32;
using OpenDentBusiness;
using OpenDentBusiness.WebServiceMainHQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using WebServiceSerializer;

namespace OpenDental {
	public class PrefL{

		///<summary>This ONLY runs when first opening the program.  It returns true if either no conversion is necessary, or if conversion was successful.  False for other situations like corrupt db, trying to convert to older version, etc.  Silent mode is mostly used from internal tools.  It is currently used in the Main Program if the silent command line argument is set.</summary>
		public static bool ConvertDB(bool silent,string toVersion) {
			ClassConvertDatabase ClassConvertDatabase2=new ClassConvertDatabase();
			string pref=PrefC.GetString(PrefName.DataBaseVersion);
				//(Pref)PrefC.HList["DataBaseVersion"];
			//Debug.WriteLine(pref.PrefName+","+pref.ValueString);
			if(ClassConvertDatabase2.Convert(pref,toVersion,silent)) {
				//((Pref)PrefC.HList["DataBaseVersion"]).ValueString)) {
				return true;
			}
			else {
				Application.Exit();
				return false;
			}
		}

		///<summary>This ONLY runs when first opening the program.  It returns true if either no conversion is necessary, or if conversion was successful.  False for other situations like corrupt db, trying to convert to older version, etc.</summary>
		public static bool ConvertDB() {
			return ConvertDB(false,Application.ProductVersion);
		}

		///<summary>Copies the installation directory files into the database as well as the AtoZ share.
		///Currently only called from FormUpdateSetup.</summary>
		public static bool CopyFromHereToUpdateFiles(Version versionCurrent) {
			//When we use the Recopy button we always want to save a copy into the AtoZ folder for backwards compatibility.
			return CopyFromHereToUpdateFiles(versionCurrent,false,true);
		}

		///<summary>Copies the installation directory files into the database.  Set hasAtoZ to true to copy to the AtoZ share as well.</summary>
		public static bool CopyFromHereToUpdateFiles(Version versionCurrent,bool isSilent,bool hasAtoZ) {
			string folderUpdate="";
			if(PrefC.AtoZfolderUsed && hasAtoZ) {
				folderUpdate=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"UpdateFiles");
			}
			else {
				folderUpdate=ODFileUtils.CombinePaths(GetTempFolderPath(),"UpdateFiles");
			}
			if(Directory.Exists(folderUpdate)) {
				try {
					Directory.Delete(folderUpdate,true);
				}
				catch {
					if(isSilent) {
						FormOpenDental.ExitCode=301;//UpdateFiles folder cannot be deleted (Warning)
						Application.Exit();
						return false;
					}
					MessageBox.Show(Lan.g("Prefs","Please delete this folder and then re-open the program: ")+folderUpdate);
					return false;
				}
				//wait a bit so that CreateDirectory won't malfunction.
				DateTime now=DateTime.Now;
				while(Directory.Exists(folderUpdate) && DateTime.Now < now.AddSeconds(10)) {//up to 10 seconds
					Application.DoEvents();
				}
				if(Directory.Exists(folderUpdate)) {
					if(isSilent) {
						FormOpenDental.ExitCode=301;//UpdateFiles folder cannot be deleted (Warning)
						Application.Exit();
						return false;
					}
					MessageBox.Show(Lan.g("Prefs","Please delete this folder and then re-open the program: ")+folderUpdate);
					return false;
				}
			}
			//Copy the installation directory files to the UpdateFiles share or a temp dir that we just created which we will zip up and insert into the db.
			//When PrefC.AtoZfolderUsed is true and we're upgrading from a version prior to 15.3.10, this copy that we are about to make allows backwards 
			//compatibility for versions of OD that do not look at the database for their UpdateFiles.
			try {
				Directory.CreateDirectory(folderUpdate);
				DirectoryInfo dirInfo=new DirectoryInfo(Application.StartupPath);
				FileInfo[] appfiles=dirInfo.GetFiles();
				for(int i=0;i<appfiles.Length;i++) {
					if(appfiles[i].Name=="FreeDentalConfig.xml") {
						continue;//skip this one.
					}
					if(appfiles[i].Name=="OpenDentalServerConfig.xml") {
						continue;//skip also
					}
					if(appfiles[i].Name.StartsWith("openlog")) {
						continue;//these can be big and are irrelevant
					}
					if(appfiles[i].Name.Contains("__")) {//double underscore
						continue;//So that plugin dlls can purposely skip the file copy.
					}
					//include UpdateFileCopier
					File.Copy(appfiles[i].FullName,ODFileUtils.CombinePaths(folderUpdate,appfiles[i].Name));
				}
				//Create a simple manifest file so that we know what version the files are for.
				File.WriteAllText(ODFileUtils.CombinePaths(folderUpdate,"Manifest.txt"),versionCurrent.ToString(3));
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g("Prefs","Failed copying the update files to the following directory")+":\r\n"
					+folderUpdate+"\r\n"+"\r\n"
					+Lan.g("Prefs","This could be due to a lack of permissions to create the above folder or the files in the installation directory are still in use."));
				return false;
			}
			//Starting in v15.3, we always insert the UpdateFiles into the database.
			ZipFile zipFile=new ZipFile();
			MemoryStream memStream=new MemoryStream();
			try {
				zipFile.AddDirectory(folderUpdate);
				zipFile.Save(memStream);
				bool isFirst=true;
				long docNum=0;
				//Our installations of MySQL defaults the global property 'max_allowed_packet' to 40MB.
				//The UpdateFiles folder will only get larger as time goes on.  Therefore, we want to break up the UpdateFiles folder into 15MB chunks.
				//If the chunk size is to be changed, it must be changed to a size that is divisible by 3.
				//Because we are converting the byte array into a Base64String, it needs to be in 3-byte chunks to perform the conversion without padding.
				//Any incomplete 3-byte chunks will get padded with '=' to complete the 3-byte chunk.
				//If this ever happens in the middle of inserting, the zip will be corrupted and we will not be able to extract the data later.
				//Converting the file to Base64String bloats the size by approximately 30% so we need to make sure that the chunk size is well below 
				//the max_allowed_packet size.
				byte[] zipFileBytes=new byte[15728640]; //15MB
				int readBytes=0;
				memStream.Position=0;//Start at the beginning of the stream.
				while((readBytes=memStream.Read(zipFileBytes,0,zipFileBytes.Length))>0) {
					string zipFileBytesBase64=Convert.ToBase64String(zipFileBytes,0,readBytes);
					if(isFirst) {
						docNum=DocumentMiscs.SetUpdateFilesZip(zipFileBytesBase64);
						isFirst=false;
					}
					else {
						DocumentMiscs.AppendRawBase64ForDoc(zipFileBytesBase64,docNum); //Updates document by appending more of it into the DB (30MB increments)
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g("Prefs","Failed inserting update files into the database."
					+"\r\nPlease call us or have your IT admin increase the max_allowed_packet to 40MB in the my.ini file."));
				return false;
			}
			finally {
				zipFile.Dispose();
				memStream.Dispose();
			}
			return true;
		}

		public static bool Install(string serviceName,FileInfo fileInfo) {
			try {
				Process process=new Process();
				process.StartInfo.WorkingDirectory=fileInfo.DirectoryName;
				process.StartInfo.FileName=Path.Combine(Directory.GetCurrentDirectory(),"installutil.exe");
				//new strategy for having control over servicename
				//InstallUtil /ServiceName=OpenDentHL7_abc OpenDentHL7.exe
				process.StartInfo.Arguments="/ServiceName="+serviceName+" "+fileInfo.FullName;
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

			///<summary>Called in two places.  Once from FormOpenDental.PrefsStartup, and also from FormBackups after a restore.</summary>
		public static bool CheckProgramVersion() {
			return CheckProgramVersion(false);
		}

		///<summary>Called in two places.  Once from FormOpenDental.PrefsStartup, and also from FormBackups after a restore.</summary>
		public static bool CheckProgramVersion(bool isSilent) {
#if DEBUG
			return true;//Development mode never needs to check versions or copy files to other directories.  Simply return true at this point.
#endif
			if(PrefC.GetBool(PrefName.UpdateWindowShowsClassicView)) {
				if(isSilent) {
					FormOpenDental.ExitCode=399;//Classic View is not supported with Silent Update
					Application.Exit();
					return false;
				}
				return CheckProgramVersionClassic();
			}
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=new Version(Application.ProductVersion);
			string database="";
			//string command="";
			if(DataConnection.DBtype==DatabaseType.MySql){
				database=MiscData.GetCurrentDatabase();
			}
			//Give option to downgrade to server if client version > server version and both the WebServiceServerName isn't blank and the current computer ID is not the same as the WebServiceServerName
			if(storedVersion<currentVersion 
				&& PrefC.GetString(PrefName.WebServiceServerName)!="" 
				&& !ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()))
			{
				if(isSilent) {
					FormOpenDental.ExitCode=310;//Client version is higher than Server Version and update is not allowed from Client.
					Application.Exit();
					return false;
				}
				//Offer to downgrade
				string message=Lan.g("Prefs","Your version is more recent than the server version.");
				message+="\r\n"+Lan.g("Prefs","Updates are only allowed from the web server")+": "+PrefC.GetString(PrefName.WebServiceServerName);
				message+="\r\n"+Lan.g("Prefs","Do you want to downgrade to the server version?");
				if(MessageBox.Show(message,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					Application.Exit();
					return false;//If user clicks cancel, then exit program
				}
			}
			//Push update to server if client version > server version and either the WebServiceServerName is blank or the current computer ID is the same as the WebServiceServerName
			//At this point we know 100% it's going to be an upgrade
			else if(storedVersion<currentVersion 
				&& (PrefC.GetString(PrefName.WebServiceServerName)=="" || ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()))) {
#if TRIALONLY
				if(PrefC.GetString(PrefName.RegistrationKey)!="") {//Allow databases with no reg key to continue.  Needed by our conversion department.
					//Trial users should never be able to update a database, not even the ProgramVersion preference.
					MsgBox.Show("PrefL","Trial versions cannot connect to live databases.  Please run the Setup.exe in the AtoZ folder to reinstall your original version.");
					Application.Exit();
					return false;//Should not get to this line.  Just in case.
				}
#endif
				//This has been commented out because it was deemed unnecessary: 10/10/14 per Jason and Derek
				//There are two different situations where this might happen.
				//if(PrefC.GetString(PrefName.UpdateInProgressOnComputerName)==""){//1. Just performed an update from this workstation on another database.
				//	//This is very common for admins when viewing slighly older databases.
				//	//There should be no annoying behavior here.  So do nothing.
				//	#if !DEBUG
				//		//Excluding this in debug allows us to view slightly older databases without accidentally altering them.
				//		Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				//		Cache.Refresh(InvalidType.Prefs);
				//	#endif
				//	return true;
				//}
				//and 2a. Just performed an update from this workstation on this database.  
				//or 2b. Just performed an update from this workstation for multiple databases.
				//In both 2a and 2b, we already downloaded Setup file to correct location for this db, so skip 1 above.
				//This computer just performed an update, but none of the other computers has updated yet.
				//So attempt to stash all files that are in the Application directory.
				//At this point we know that we are going to perform an update.
				bool hasAtoZ=false;
				//Check to see if the version we are coming from is prior to v15.3.
				//If we are coming from an older version, we need to put a copy of the Update Files into the AtoZ share for backwards compatibility.
				if(storedVersion<new Version("15.3.10")) {
					//In 15.3.10 we started to explicitly use the database for storing the Update Files folder.
					//Any clients updating from a previous version still need the Update Files in the AtoZ because they look there instead of the db.
					hasAtoZ=true;
				}
				if(!CopyFromHereToUpdateFiles(currentVersion,isSilent,hasAtoZ)) {
					Application.Exit();
					return false;
				}
				Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");//now, other workstations will be allowed to update.
				Prefs.UpdateDateT(PrefName.ProgramVersionLastUpdated,DateTime.Now);
				Cache.Refresh(InvalidType.Prefs);
				//If this is the Update Server computer, we need to check if they have upgraded the CustListener service to the eConnector.
				if(PrefC.GetString(PrefName.WebServiceServerName)!="" 
					&& ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower())
					&& !PrefC.GetBool(PrefName.EConnectorEnabled))
				{
					//Customer has not upgraded to the eConnector service.
					bool isListening;
					//if isSilent=false, a messagebox will be displayed if anything goes wrong.
					if(UpgradeOrInstallEConnector(isSilent,out isListening)) {
						Prefs.UpdateBool(PrefName.EConnectorEnabled,true);
						try {
							WebServiceMainHQ webServiceMain=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
							webServiceMain.SetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey)),isListening);
						}
						catch(Exception ex) {
							if(!isSilent) {
								//We probably don't want to notify them that a connection to HQ to update their listener type has failed.  Or do we?
							}
						}
					}
					else {//Upgrading to eConnector failed.
						//Purposefully do not fail the upgrade if automatically upgrading to the eConnector failed.
						//The user will call us up when their eServices are no longer working and we will be able to assist them in installing the new service.
						//NEVER update the EConnectorEnabled preference to false.  There is no such thing.  It is used as a one time flag.
					}
				}
			}
			if(storedVersion>currentVersion) {
				if(isSilent) {//This should never happen after a silent update.
					FormOpenDental.ExitCode=312;//Stored version is higher that client version after an update was successful.
					Application.Exit();
					return false;
				}
				//performs both upgrades and downgrades by recopying update files from DB to temp folder, then from temp folder to local program path.
				//This is the update sequence for both a direct workstation, and for a ClientWeb workstation.
				string folderUpdate=ODFileUtils.CombinePaths(GetTempFolderPath(),"UpdateFiles");
				if(Directory.Exists(folderUpdate)) {
					Directory.Delete(folderUpdate,true);
				}
				DocumentMisc docmisc=DocumentMiscs.GetUpdateFilesZip();
				if(docmisc!=null) {
					byte[] rawBytes=Convert.FromBase64String(docmisc.RawBase64);
					using(ZipFile unzipped=ZipFile.Read(rawBytes)) {
						unzipped.ExtractAll(folderUpdate);
					}
				} 
				//look at the manifest to see if it's the version we need
				string manifestVersion="";
				try {
					manifestVersion=File.ReadAllText(ODFileUtils.CombinePaths(folderUpdate,"Manifest.txt"));
				}
				catch {
					//fail silently
				}
				if(manifestVersion!=storedVersion.ToString(3)) {//manifest version is wrong
					//No point trying the Setup.exe because that's probably wrong too.
					//Just go straight to downloading and running the Setup.exe.
					string manpath=ODFileUtils.CombinePaths(folderUpdate,"Manifest.txt");
					if(MessageBox.Show(Lan.g("Prefs","The expected version information was not found in this file: ")+manpath+".  "
						+Lan.g("Prefs","There is probably a permission issue on that folder which should be fixed. ")
						+"\r\n\r\n"+Lan.g("Prefs","The suggested solution is to return to the computer where the update was just run.  Go to Help | Update | Setup, and click the Recopy button.")
						+"\r\n\r\n"+Lan.g("Prefs","If, instead, you click OK in this window, then a fresh Setup file will be downloaded and run."),						
						"",MessageBoxButtons.OKCancel)!=DialogResult.OK)//they don't want to download again.
					{
						Application.Exit();
						return false;
					}
					DownloadAndRunSetup(storedVersion,currentVersion);
					Application.Exit();
					return false;
				}
				//manifest version matches
				if(MessageBox.Show(Lan.g("Prefs","Files will now be copied.")+"\r\n"
					+Lan.g("Prefs","Workstation version will be updated from ")+currentVersion.ToString(3)
					+Lan.g("Prefs"," to ")+storedVersion.ToString(3),
					"",MessageBoxButtons.OKCancel)
					!=DialogResult.OK)//they don't want to update for some reason.
				{
					Application.Exit();
					return false;
				}
				string tempDir=GetTempFolderPath();
				//copy UpdateFileCopier.exe to the temp directory
				File.Copy(ODFileUtils.CombinePaths(folderUpdate,"UpdateFileCopier.exe"),//source
					ODFileUtils.CombinePaths(tempDir,"UpdateFileCopier.exe"),//dest
					true);//overwrite
				//wait a moment to make sure the file was copied
				Thread.Sleep(500);
				//launch UpdateFileCopier to copy all files to here.
				int processId=Process.GetCurrentProcess().Id;
				string appDir=Application.StartupPath;
				string startFileName=ODFileUtils.CombinePaths(tempDir,"UpdateFileCopier.exe");
				string arguments="\""+folderUpdate+"\""//pass the source directory to the file copier.
					+" "+processId.ToString()//and the processId of Open Dental.
					+" \""+appDir+"\"";//and the directory where OD is running
				Process.Start(startFileName,arguments);					
				Application.Exit();//always exits, whether launch of setup worked or not
				return false;
			}
			return true;
		}

		///<summary>If AtoZ.manifest was wrong, or if user is not using AtoZ, then just download again.  Will use dir selected by user.  If an appropriate download is not available, it will fail and inform user.</summary>
		private static void DownloadAndRunSetup(Version storedVersion,Version currentVersion) {
			string patchName="Setup.exe";
			string updateUri=PrefC.GetString(PrefName.UpdateWebsitePath);
			string updateCode=PrefC.GetString(PrefName.UpdateCode);
			string updateInfoMajor="";
			string updateInfoMinor="";
			if(!FormUpdate.ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)){
				return;
			}
			if(MessageBox.Show(
				Lan.g("Prefs","Setup file will now be downloaded.")+"\r\n"
				+Lan.g("Prefs","Workstation version will be updated from ")+currentVersion.ToString(3)
				+Lan.g("Prefs"," to ")+storedVersion.ToString(3),
				"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK)//they don't want to update for some reason.
			{
				return;
			}
			FolderBrowserDialog dlg=new FolderBrowserDialog();
			dlg.SelectedPath=ImageStore.GetPreferredAtoZpath();
			dlg.Description=Lan.g("Prefs","Setup.exe will be downloaded to the folder you select below");
			if(dlg.ShowDialog()!=DialogResult.OK) {
				return;//app will exit
			}
			string tempFile=ODFileUtils.CombinePaths(dlg.SelectedPath,patchName);
			//ODFileUtils.CombinePaths(GetTempFolderPath(),patchName);
			FormUpdate.DownloadInstallPatchFromURI(updateUri+updateCode+"/"+patchName,//Source URI
				tempFile,true,false,null);//Local destination file.
			File.Delete(tempFile);//Cleanup install file.
		}

				///<summary>This ONLY runs when first opening the program.  Gets run early in the sequence. Returns false if the program should exit.</summary>
		public static bool CheckMySqlVersion() {
			return CheckMySqlVersion(false);
		}

		///<summary>This ONLY runs when first opening the program.  Gets run early in the sequence. Returns false if the program should exit.</summary>
		public static bool CheckMySqlVersion(bool isSilent) {
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return true;
			}
			bool hasBackup=false;
			string thisVersion=MiscData.GetMySqlVersion();
			Version versionMySQL=new Version(thisVersion);
			if(versionMySQL < new Version(5,0)) {
				if(isSilent) {
					FormOpenDental.ExitCode=110;//MySQL version lower than 5.0
					Application.Exit();
					return false;
				}
				//We will force users to upgrade to 5.0, but not yet to 5.5
				MessageBox.Show(Lan.g("Prefs","Your version of MySQL won't work with this program")+": "+thisVersion
					+".  "+Lan.g("Prefs","You should upgrade to MySQL 5.0 using the installer on our website."));
				Application.Exit();
				return false;
			}
			if(!PrefC.ContainsKey("MySqlVersion")) {//db has not yet been updated to store this pref
				//We're going to skip this.  We will recommend that people first upgrade OD, then MySQL, so this won't be an issue.
			}
			else {//Using a version that stores the MySQL version as a preference.
				//There was an old bug where the MySQLVersion preference could be stored as 5,5 instead of 5.5 due to converting the version into a float.
				//Replace any commas with periods before checking if the preference is going to change.
				//This is simply an attempt to avoid making unnecessary backups for users with a corrupt version (e.g. 5,5).
				if(PrefC.GetString(PrefName.MySqlVersion).Contains(",")) {
					Prefs.UpdateString(PrefName.MySqlVersion,PrefC.GetString(PrefName.MySqlVersion).Replace(",","."));
				}
				//Now check to see if the MySQL version has been updated.  If it has, make an automatic backup, repair, and optimize all tables.
				if(Prefs.UpdateString(PrefName.MySqlVersion,(thisVersion))) {
					if(!isSilent) {
						if(!MsgBox.Show("Prefs",MsgBoxButtons.OKCancel,"Tables will now be backed up, optimized, and repaired.  This will take a minute or two.  Continue?")) {
							Application.Exit();
							return false;
						}
					}
					if(!Shared.BackupRepairAndOptimize(isSilent)) {
						if(isSilent) {
							FormOpenDental.ExitCode=101;//Database Backup failed
						}
						Application.Exit();
						return false;
					}
					hasBackup=true;
				}
			}
			if(PrefC.ContainsKey("DatabaseConvertedForMySql41")) {
				return true;//already converted
			}
			if(!isSilent) {
				if(!MsgBox.Show("Prefs",true,"Your database will now be converted for use with MySQL 4.1.")) {
					Application.Exit();
					return false;
				}
			}
			//ClassConvertDatabase CCD=new ClassConvertDatabase();
			if(!hasBackup) {//A backup could have been made if the tables were optimized and repaired above.
				if(!Shared.MakeABackup(isSilent)) {
					if(isSilent) {
						FormOpenDental.ExitCode=101;//Database Backup failed
					}
					Application.Exit();
					return false;//but this should never happen
				}
			}
			if(!isSilent) {
				MsgBox.Show("Prefs","Backup performed");
			}
			Prefs.ConvertToMySqlVersion41();
			if(!isSilent) {
				MsgBox.Show("Prefs","Converted");
			}
			//Refresh();
			return true;
		}

		///<summary>This runs when first opening the program.  If MySql is not at 5.5 or higher, it reminds the user, but does not force them to upgrade.</summary>
		public static void MySqlVersion55Remind(){
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return;
			}
			string thisVersion=MiscData.GetMySqlVersion();
			Version versionMySQL=new Version(thisVersion);
			if(versionMySQL < new Version(5,5) && !Programs.IsEnabled(ProgramName.eClinicalWorks)) {//Do not show msg if MySQL version is 5.5 or greater or eCW is enabled
				MsgBox.Show("Prefs","You should upgrade to MySQL 5.5 using the installer posted on our website.  It's not urgent, but until you upgrade, you are likely to get a few errors each day which will require restarting the MySQL service.");
			}
		}

		///<summary>Essentially no changes have been made to this since version 6.5.</summary>
		private static bool CheckProgramVersionClassic() {
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=new Version(Application.ProductVersion);
			string database=MiscData.GetCurrentDatabase();
			if(storedVersion<currentVersion) {
				Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				Prefs.UpdateDateT(PrefName.ProgramVersionLastUpdated,PIn.DateT(DateTime.Now.ToShortDateString()));
				Cache.Refresh(InvalidType.Prefs);
			}
			if(storedVersion>currentVersion) {
				if(PrefC.AtoZfolderUsed) {
					string setupBinPath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Setup.exe");
					if(File.Exists(setupBinPath)) {
						if(MessageBox.Show("You are attempting to run version "+currentVersion.ToString(3)+",\r\n"
							+"But the database "+database+"\r\n"
							+"is already using version "+storedVersion.ToString(3)+".\r\n"
							+"A newer version must have already been installed on at least one computer.\r\n"  
							+"The setup program stored in your A to Z folder will now be launched.\r\n"
							+"Or, if you hit Cancel, then you will have the option to download again."
							,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							if(MessageBox.Show("Download again?","",MessageBoxButtons.OKCancel)
								==DialogResult.OK) {
								FormUpdate FormU=new FormUpdate();
								FormU.ShowDialog();
							}
							Application.Exit();
							return false;
						}
						try {
							Process.Start(setupBinPath);
						}
						catch {
							MessageBox.Show("Could not launch Setup.exe");
						}
					}
					else if(MessageBox.Show("A newer version has been installed on at least one computer,"+
							"but Setup.exe could not be found in any of the following paths: "+
							ImageStore.GetPreferredAtoZpath()+".  Download again?","",MessageBoxButtons.OKCancel)==DialogResult.OK) {
						FormUpdate FormU=new FormUpdate();
						FormU.ShowDialog();
					}
				}
				else {//Not using image path.
					//perform program update automatically.
					string patchName="Setup.exe";
					string updateUri=PrefC.GetString(PrefName.UpdateWebsitePath);
					string updateCode=PrefC.GetString(PrefName.UpdateCode);
					string updateInfoMajor="";
					string updateInfoMinor="";
					if(FormUpdate.ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)) {
						if(MessageBox.Show(updateInfoMajor+Lan.g("Prefs","Perform program update now?"),"",
							MessageBoxButtons.YesNo)==DialogResult.Yes) {
							string tempFile=ODFileUtils.CombinePaths(GetTempFolderPath(),patchName);//Resort to a more common temp file name.
							FormUpdate.DownloadInstallPatchFromURI(updateUri+updateCode+"/"+patchName,//Source URI
								tempFile,true,true,null);//Local destination file.
							File.Delete(tempFile);//Cleanup install file.
						}
					}
				}
				Application.Exit();//always exits, whether launch of setup worked or not
				return false;
			}
			return true;
		}

		///<summary>Returns the path to the temporary opendental directory, temp/opendental.  Also performs one-time cleanup, if necessary.  In FormOpenDental_FormClosing, the contents of temp/opendental get cleaned up.</summary>
		public static string GetTempFolderPath() {
			//Will clean up entire temp folder for a month after the enhancement of temp file cleanups as long as the temp\opendental folder doesn't already exist.
			string tempPathOD=ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental");
			if(Directory.Exists(tempPathOD)) {
				//Cleanup has already run for the old temp folder.  Do nothing.
				return tempPathOD;
			}
			Directory.CreateDirectory(tempPathOD);
			if(DateTime.Today>PrefC.GetDate(PrefName.TempFolderDateFirstCleaned).AddMonths(1)) {
				return tempPathOD;
			}
			//This might be used if this is the first time running this version on the computer that did the db update.
			//This might also be used if this is a computer that was turned off for a few weeks around the time of update conversion.
			//We need some sort of time limit just in case it's annoying and keeps happening.
			//So this will have a small risk of missing a computer, but the benefit of limiting outweighs the risk.
			//Empty entire temp folder.  Blank folders will be left behind because they do not matter.
			string[] arrayFileNames=Directory.GetFiles(Path.GetTempPath());
			for(int i=0;i<arrayFileNames.Length;i++) {
				try {
					if(arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.'))==".exe" || arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.'))==".cs") {
						//Do nothing.  We don't care about .exe or .cs files and don't want to interrupt other programs' files.
					}
					else {
						File.Delete(arrayFileNames[i]);
					}
				}
				catch {
					//Do nothing because the file could have been in use or there were not sufficient permissions.
					//This file will most likely get deleted next time a temp file is created.
				}
			}
			return tempPathOD;
		}

		///<summary>Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.</summary>
		public static string GetRandomTempFile(string ext) {
			return ODFileUtils.CreateRandomFile(GetTempFolderPath(),ext);
		}

	}
}
