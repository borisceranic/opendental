using CodeBase;
using Ionic.Zip;
using OpenDentBusiness;
using OpenDentBusiness.WebServiceMainHQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
			if(!isSilent) {
				ODThread odThreadRecopy=new ODThread(ShowRecopyProgress);
				odThreadRecopy.Name="RecopyProgressThread";
				odThreadRecopy.Start(true);
			}
			#region Delete Old UpdateFiles Folders
			string folderTempUpdateFiles=ODFileUtils.CombinePaths(GetTempFolderPath(),"UpdateFiles");
			string folderAtoZUpdateFiles="";
			if(PrefC.AtoZfolderUsed && hasAtoZ) {
				folderAtoZUpdateFiles=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"UpdateFiles");
			}
			ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Removing old update files...")));
			//Try to delete the UpdateFiles folder from both the AtoZ share and the local TEMP dir.
			if(!DeleteFolder(folderAtoZUpdateFiles) | !DeleteFolder(folderTempUpdateFiles)) {//Logical OR to prevent short curcuit.
				ODEvent.Fire(new ODEventArgs("RecopyProgress","DEFCON 1"));
				if(isSilent) {
					FormOpenDental.ExitCode=301;//UpdateFiles folder cannot be deleted (Warning)
					Application.Exit();
					return false;
				}
				MsgBox.Show("Prefs","Unable to delete old UpdateFiles folder.  Go manually delete the UpdateFiles folder then retry.");
				return false;
			}
			#endregion
			#region Copy Current Installation Directory To UpdateFiles Folders
			ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Backing up new update files...")));
			//Copy the installation directory files to the UpdateFiles share and a TEMP dir that we just created which we will zip up and insert into the db.
			//When PrefC.AtoZfolderUsed is true and we're upgrading from a version prior to 15.3.10, this copy that we are about to make allows backwards 
			//compatibility for versions of OD that do not look at the database for their UpdateFiles.
			if(!CopyInstallFilesToPath(folderAtoZUpdateFiles,versionCurrent) | !CopyInstallFilesToPath(folderTempUpdateFiles,versionCurrent)) {
				ODEvent.Fire(new ODEventArgs("RecopyProgress","DEFCON 1"));
				if(isSilent) {
					FormOpenDental.ExitCode=302;//Installation files could not be copied.
					Application.Exit();
					return false;
				}
				MsgBox.Show("Prefs","Failed to copy the current installations files on this computer.\r\n"
					+"This could be due to a lack of permissions, or file(s) in the installation directory are still in use.");
				return false;
			}
			#endregion
			#region Get Current MySQL max_allowed_packet Setting
			//Starting in v15.3, we always insert the UpdateFiles into the database.
			int maxAllowedPacket=0;
			int defaultMaxAllowedPacketSize=41943040;//40MB
			if(DataConnection.DBtype==DatabaseType.MySql) {
				ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Getting MySQL max allowed packet setting...")));
				maxAllowedPacket=MiscData.GetMaxAllowedPacket();
				//If trying to get the max_allowed_packet value for MySQL failed, assume they can handle 40MB of data.
				//Our installations of MySQL defaults the global property 'max_allowed_packet' to 40MB.
				//Nathan suggested forcing the global and local max_allowed_packet to 40MB if it was set to anything less.
				if(maxAllowedPacket < defaultMaxAllowedPacketSize) {
					try {
						maxAllowedPacket=MiscData.SetMaxAllowedPacket(defaultMaxAllowedPacketSize);
					}
					catch(Exception ex) {
						//Do nothing.  Either maxAllowedPacket is set to something small (e.g. 10MB) and we failed to update it to 40MB (should be fine)
						//             OR we failed to get and set the global variable due to MySQL permissions and a UE was thrown.
						//             Regardless, if maxAllowedPacket is 0 (the only thing that we can't have happen) it will get updated to 40MB later down.
						EventLog.WriteEntry("OpenDental","Error updating max_allowed_packet from "+maxAllowedPacket
							+" to "+defaultMaxAllowedPacketSize+":\r\n"+ex.Message,EventLogEntryType.Error);
					}
				}
			}
			//Only change maxAllowedPacket if we couldn't successfully get the current value from the database or using Oracle.
			//This will let the program attempt to insert the UpdateFiles into the db with the assumption that they are using our default setting (40MB).
			//Worst case scenario, the user will hit the max_packet_allowed error below which will simply notify them to update their my.ini manually.
			if(maxAllowedPacket==0) {
				maxAllowedPacket=defaultMaxAllowedPacketSize;
			}
			#endregion
			using(ZipFile zipFile=new ZipFile())
			using(MemoryStream memStream=new MemoryStream()) {
				try {
					#region Compress New Update Files
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Compressing new update files...")));
					try {
						zipFile.AddDirectory(folderTempUpdateFiles);
						zipFile.Save(memStream);
					}
					catch(Exception ex) {
						EventLog.WriteEntry("OpenDental","Error compressing UpdateFiles:\r\n"+ex.Message,EventLogEntryType.Error);
						throw ex;
					}
					StringBuilder strBuilder=new StringBuilder();
					byte[] zipFileBytes=new byte[memStream.Length];
					int readBytes=0;
					memStream.Position=0;//Start at the beginning of the stream.
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Converting new update files...")));
					try {
						while((readBytes=memStream.Read(zipFileBytes,0,zipFileBytes.Length))>0) {
							strBuilder.Append(Convert.ToBase64String(zipFileBytes,0,readBytes));
						}
					}
					catch(Exception ex) {
						EventLog.WriteEntry("OpenDental","Error converting UpdateFiles to Base64:\r\n"+ex.Message,EventLogEntryType.Error);
						throw ex;
					}
					#endregion
					#region Break Up New Update Files
					//Now we need to break up the Base64 string into payloads that are small enough to send to MySQL.
					//Each character in Base64 represents 6 bits.  Therefore, 4 chars are used to represent 3 bytes
					//However, MySQL doesn't know that we are sending over a Base64 string so lets assume 1 char = 1 byte.
					//Also, we want to 'buffer' a few KB for MySQL because the query itself and the parameter information will take up some bytes (unknown).
					int charsPerPayload=maxAllowedPacket-8192;//Arbitrarily subtracted 8KB from max allowed bytes for MySQL "header" information.
					List<string> listRawBase64s=new List<string>();
					string strBase64=strBuilder.ToString();
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Creating new update file payloads...")));
					try {
						for(int i=0;i<strBase64.Length;i+=charsPerPayload) {
							listRawBase64s.Add(strBase64.Substring(i,Math.Min(charsPerPayload,strBase64.Length-i)));
						}
					}
					catch(Exception ex) {
						EventLog.WriteEntry("OpenDental","Error creating UpdateFile payloads:\r\n"+ex.Message,EventLogEntryType.Error);
						throw ex;
					}
					#endregion
					#region Insert New Update Files Into Db
					//Now we have a list of Base64 strings each of which are guaranteed to successfully send to MySQL under the max_packet_allowed limitation.
					//Clear and prep the current UpdateFiles row in the documentmisc table for the updated binaries.
					long docNum=DocumentMiscs.SetUpdateFilesZip();
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Inserting new update files into database...")));
					try {
						foreach(string rawBase64 in listRawBase64s) {
							DocumentMiscs.AppendRawBase64ForUpdateFiles(rawBase64);
						}
					}
					catch(Exception ex) {
						EventLog.WriteEntry("OpenDental","Error inserting UpdateFiles into database:"
							+"\r\n"+ex.Message
							+"\r\n  docNum: "+docNum
							+"\r\n  maxAllowedPacket: "+maxAllowedPacket
							+"\r\n  charsPerPayload: "+charsPerPayload
							+"\r\n  strBase64.Length: "+strBase64.Length
							+"\r\n  listRawBase64s.Count: "+listRawBase64s.Count,EventLogEntryType.Error);
						throw ex;
					}
					#endregion
				}
				catch(Exception) {
					ODEvent.Fire(new ODEventArgs("RecopyProgress","DEFCON 1"));
					if(isSilent) {
						FormOpenDental.ExitCode=303;//Failed inserting update files into the database.
						Application.Exit();
						return false;
					}
					string errorStr=Lan.g("Prefs","Failed inserting update files into the database."
						+"\r\nPlease call us or have your IT admin increase the max_allowed_packet to 40MB in the my.ini file.");
					try {
						string innoDBTableNames=InnoDb.GetInnodbTableNames();
						if(innoDBTableNames!="") {
							//Starting in MySQL 5.6 you can specify innodb_log_file_size in the my.ini and it typicaly needs to be set higher than 48 MB (default).
							//There is danger in manipulating this value so we should not do it for the customer but have their IT do it.
							//Since the innodb_log_size variable only exists in MySQL 5.6 and greater, if the user adds this setting to their my.ini file for an
							//older version, then MySQL will fail to start.  
							//An alternative solution would be to convert their tables over to MyISAM instead of letting them continue with InnoDB (if possible).
							//The following message to the user is vague on purpose to avoid listing version numbers.
							errorStr+="\r\n"+Lan.g("Prefs","InnoDB tables have been detected, you may need to increase the innodb_log_file_size variable.");
						}
					}
					catch(Exception) {
						//Do not add the additional InnoDB warning because it will often times just confuse our typical users (odds are they are using MyISAM).
					}
					MessageBox.Show(errorStr);
					return false;
				}
				ODEvent.Fire(new ODEventArgs("RecopyProgress","DEFCON 1"));
				return true;//Inserting a compressed version of the files in the current installation directory into the database was successful.
			}
		}

		///<summary>Creates the directory passed in and copies the files in the current installation directory to the specified folder path.
		///Creates a Manifest.txt file with the version information passed in.
		///Returns false if anything goes wrong.  Returns true if copy was successful or if the folder path passed in is null or blank.</summary>
		private static bool CopyInstallFilesToPath(string folderPath,Version versionCurrent) {
			if(string.IsNullOrEmpty(folderPath)) {
				return true;//Some customers will not be using the AtoZ folder which will pass in blank.
			}
			try {
				Directory.CreateDirectory(folderPath);
				DirectoryInfo dirInfo=new DirectoryInfo(Application.StartupPath);
				FileInfo[] appfiles=dirInfo.GetFiles();
				for(int i = 0;i<appfiles.Length;i++) {
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
					File.Copy(appfiles[i].FullName,ODFileUtils.CombinePaths(folderPath,appfiles[i].Name));
				}
				//Create a simple manifest file so that we know what version the files are for.
				File.WriteAllText(ODFileUtils.CombinePaths(folderPath,"Manifest.txt"),versionCurrent.ToString(3));
			}
			catch(Exception) {
				return false;
			}
			return true;
		}

		///<summary>Recursively deletes all folders and files in the provided folder path.
		///Waits up to 10 seconds for the delete command to finish.  Returns false if anything goes wrong.</summary>
		private static bool DeleteFolder(string folderPath) {
			if(string.IsNullOrEmpty(folderPath)) {
				return true;//Nothing to delete.
			}
			if(!Directory.Exists(folderPath)) {
				return true;//Already deleted
			}
			//The directory we want to delete is present so try and recursively delete it and all its content.
			try {
				Directory.Delete(folderPath,true);
			}
			catch {
				return false;//Something went wrong, typically a permission issue or files are still in use.
			}
			//The delete seems to have been successful, wait up to 10 seconds so that CreateDirectory won't malfunction.
			DateTime dateTimeWait=DateTime.Now.AddSeconds(10);
			while(Directory.Exists(folderPath) && DateTime.Now < dateTimeWait) {
				Application.DoEvents();
			}
			if(Directory.Exists(folderPath)) {//Dir is still present after 10 seconds of waiting for the delete to complete.
				return false;
			}
			return true;//Dir deleted successfully.
		}

		private static void ShowRecopyProgress(ODThread odThread) {
			FormProgressStatus FormPS=new FormProgressStatus("RecopyProgress");
			FormPS.TopMost=true;//Make this window show on top of ALL other windows.
			FormPS.ShowDialog();
		}

		///<summary>Called from FormBackups after a restore.</summary>
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
				UpdateHistory updateHistory=new UpdateHistory(currentVersion.ToString());
				UpdateHistories.Insert(updateHistory);
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");//now, other workstations will be allowed to update.
				Cache.Refresh(InvalidType.Prefs);
				bool needsEConnectorUpgrade=false;
				//Check to see if the eConnector has ever been installed.  Warn them about potential complications due to converting to eConnector.
				//This only needs to happen once due to transitioning users over to the new eConnector service.
				if(!PrefC.GetBool(PrefName.EConnectorEnabled)) {
					//This upgrade might require converting the CustListener service over to the eConnector.
					if(PrefC.GetString(PrefName.WebServiceServerName)=="") {
						//There isn't an "Update Server Name" set so we don't know if this is the correct computer that should be running the eConnector.
						//Check to see if it currently has the listener installed on it and if it does, upgrade it to the eConnector.
						//Otherwise, warn them that their eServices might not work.
						int countCustListeners=0;
						try {
							countCustListeners=ServicesHelper.GetServicesByExe("OpenDentalCustListener.exe").Count;
						}
						catch(Exception) {
							//Do nothing and assume no CustListeners are installed.
						}
						if(countCustListeners > 0) {
							needsEConnectorUpgrade=true;//This computer is not set as the upgrade server but is upgrading and DOES have a listener present.
						}
						else {//No listener services found on the computer doing the upgrade.
							//Warn the user that their eServices will go down if there are any entries in the eservicesignal table within the last month.
							List<EServiceSignal> listSignals=EServiceSignals.GetServiceHistory(eServiceCode.ListenerService,DateTime.Now.AddMonths(-1),DateTime.Now);
							if(listSignals.Count > 0 && !isSilent) {
								MsgBox.Show("PrefL","eServices will not work until the eConnector service is installed on the computer that is running the Listener Service.  Please contact us for help installing the eConnector service or see our online manual for more information.");
							}
						}
					}
					//The "Update Server Name" preference is set to something so check to see if this is the Update Server computer.
					if(ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()) || needsEConnectorUpgrade) {
						//This is the computer that the eConnector should be installed on.  Try to upgrade or install it.
						bool isListening;
						//if isSilent=false, a messagebox will be displayed if anything goes wrong.
						if(ServicesHelper.UpgradeOrInstallEConnector(isSilent,out isListening)) {
							Prefs.UpdateBool(PrefName.EConnectorEnabled,true);//No need to send an invalidate signal to other workstations.  They were kicked out.
							try {
								WebServiceMainHQ webServiceMain=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
								ListenerServiceType listenerType=WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
									webServiceMain.SetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey)),isListening)
								);
								string logText=Lan.g("PrefL","eConnector status automatically set to")+" "+listenerType.ToString()+" "
									+Lan.g("PrefL","via the upgrade process.");
								SecurityLogs.MakeLogEntry(Permissions.EServicesSetup,0,logText);
							}
							catch(Exception) {
								if(!isSilent) {
									//Notify the user that HQ was not updated regarding the status of the eConnector (important).
									MsgBox.Show("PrefL","Could not update the eConnector communication status.  Please contact us to enable eServices.");
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
					try {
						using(ZipFile unzipped=ZipFile.Read(rawBytes)) {
							unzipped.ExtractAll(folderUpdate);
						}
					}
					catch(Exception) {
						//fail silently
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

		///<summary></summary>
		private static bool CheckProgramVersionClassic() {
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=new Version(Application.ProductVersion);
			string database=MiscData.GetCurrentDatabase();
			if(storedVersion<currentVersion) {
				Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				UpdateHistory updateHistory=new UpdateHistory(currentVersion.ToString());
				UpdateHistories.Insert(updateHistory);
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
