using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Text; 
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{

	///<summary></summary>
	public partial class ClassConvertDatabase{
		private System.Version FromVersion;
		private System.Version ToVersion;

		///<summary>Return false to indicate exit app.  Only called when program first starts up at the beginning of FormOpenDental.PrefsStartup.</summary>
		public bool Convert(string fromVersion,string toVersion,bool isSilent) {
			FromVersion=new Version(fromVersion);
			ToVersion=new Version(toVersion);//Application.ProductVersion);
			if(FromVersion>=new Version("3.4.0") && PrefC.GetBool(PrefName.CorruptedDatabase)){
				if(isSilent) {
					FormOpenDental.ExitCode=201;//Database was corrupted due to an update failure
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"Your database is corrupted because an update failed.  Please contact us.  This database is unusable and you will need to restore from a backup.");
				return false;//shuts program down.
			}
			if(FromVersion==ToVersion) {
				return true;//no conversion necessary
			}
			if(FromVersion.CompareTo(ToVersion)>0){//"Cannot convert database to an older version."
				//no longer necessary to catch it here.  It will be handled soon enough in CheckProgramVersion
				return true;
			}
			if(FromVersion < new Version("2.8.0")){
				if(isSilent) {
					FormOpenDental.ExitCode=130;//Database must be upgraded to 2.8 to continue
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"This database is too old to easily convert in one step. Please upgrade to 2.1 if necessary, then to 2.8.  Then you will be able to upgrade to this version. We apologize for the inconvenience.");
				return false;
			}
			if(FromVersion < new Version("6.6.2")) {
				if(isSilent) {
					FormOpenDental.ExitCode=131;//Database must be upgraded to 11.1 to continue
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"This database is too old to easily convert in one step. Please upgrade to 11.1 first.  Then you will be able to upgrade to this version. We apologize for the inconvenience.");
				return false;
			}
			if(FromVersion < new Version("3.0.1")) {
				if(!isSilent) {
					MsgBox.Show(this,"This is an old database.  The conversion must be done using MySQL 4.1 (not MySQL 5.0) or it will fail.");
				}
			}
			if(FromVersion.ToString()=="2.9.0.0" || FromVersion.ToString()=="3.0.0.0" || FromVersion.ToString()=="4.7.0.0"){
				if(isSilent) {
					FormOpenDental.ExitCode=190;//Cannot convert this database version which was only for development purposes
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"Cannot convert this database version which was only for development purposes.");
				return false;
			}
			if(FromVersion > new Version("4.7.0") && FromVersion.Build==0){
				if(isSilent) {
					FormOpenDental.ExitCode=190;//Cannot convert this database version which was only for development purposes
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"Cannot convert this database version which was only for development purposes.");
				return false;
			}
			if(FromVersion >= ConvertDatabases.LatestVersion){
				return true;//no conversion necessary
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				if(isSilent) {
					FormOpenDental.ExitCode=140;//Web client cannot convert database
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"Web client cannot convert database.  Must be using a direct connection.");
				return false;
			}
			if(ReplicationServers.ServerIsBlocked()) {
				if(isSilent) {
					FormOpenDental.ExitCode=150;//Replication server is blocked from performing updates
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"This replication server is blocked from performing updates.");
				return false;
			}
#if TRIALONLY
			//Trial users should never be able to update a database.
			if(PrefC.GetString(PrefName.RegistrationKey)!="") {//Allow databases with no reg key to update.  Needed by our conversion department.
				if(isSilent) {
					FormOpenDental.ExitCode=191;//Trial versions cannot connect to live databases
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"Trial versions cannot connect to live databases.  Please run the Setup.exe in the AtoZ folder to reinstall your original version.");
				return false;
			}
#endif
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" //using web service
				&& !ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()))//and not on web server 
			{
#if !DEBUG
				if(isSilent) {
					FormOpenDental.ExitCode=141;//Updates are only allowed from a designated web server
					Application.Exit();
					return false;
				}
#endif
				MessageBox.Show(Lan.g(this,"Updates are only allowed from the web server: ")+PrefC.GetString(PrefName.WebServiceServerName));
				return false;//if you are in debug mode and you really need to update the DB, you can manually clear the WebServiceServerName preference.
			}
			//If MyISAM and InnoDb mix, then try to fix
			if(DataConnection.DBtype==DatabaseType.MySql) {//not for Oracle
				string namesInnodb=InnoDb.GetInnodbTableNames();//Or possibly some other format.
				int numMyisam=DatabaseMaintenance.GetMyisamTableCount();
				if(namesInnodb!="" && numMyisam>0) {
					if(!isSilent) {
						MessageBox.Show(Lan.g(this,"A mixture of database tables in InnoDB and MyISAM format were found.  A database backup will now be made, and then the following InnoDB tables will be converted to MyISAM format: ")+namesInnodb);
					}
					if(!Shared.MakeABackup(isSilent)) {
						Cursor.Current=Cursors.Default;
						if(isSilent) {
							FormOpenDental.ExitCode=101;//Database Backup failed
							Application.Exit();
						}
						return false;
					}
					if(!DatabaseMaintenance.ConvertTablesToMyisam()) {
						if(isSilent) {
							FormOpenDental.ExitCode=102;//Failed to convert InnoDB tables to MyISAM format
							Application.Exit();
							return false;
						}
						MessageBox.Show(Lan.g(this,"Failed to convert InnoDB tables to MyISAM format. Please contact support."));
						return false;
					}
					if(!isSilent) {
						MessageBox.Show(Lan.g(this,"All tables converted to MyISAM format successfully."));
					}
					namesInnodb="";
				}
				if(namesInnodb=="" && numMyisam>0) {//if all tables are myisam
					//but default storage engine is innodb, then kick them out.
					if(DatabaseMaintenance.GetStorageEngineDefaultName().ToUpper()!="MYISAM") { //Probably InnoDB but could be another format.
						if(isSilent) {
							FormOpenDental.ExitCode=103;//Default database .ini setting is innoDB
							Application.Exit();
							return false;
						}
						MessageBox.Show(Lan.g(this,"The database tables are in MyISAM format, but the default database engine format is InnoDB. You must change the default storage engine within the my.ini (or my.cnf) file on the database server and restart MySQL in order to fix this problem. Exiting."));
						return false;
					}
				}
			}
#if DEBUG
			if(!isSilent && MessageBox.Show("You are in Debug mode.  Your database can now be converted"+"\r"
				+"from version"+" "+FromVersion.ToString()+"\r"
				+"to version"+" "+ToVersion.ToString()+"\r"
				+"You can click Cancel to skip conversion and attempt to run the newer code against the older database."
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return true;//If user clicks cancel, then do nothing
			}
#else
			if(!isSilent && MessageBox.Show(Lan.g(this,"Your database will now be converted")+"\r"
				+Lan.g(this,"from version")+" "+FromVersion.ToString()+"\r"
				+Lan.g(this,"to version")+" "+ToVersion.ToString()+"\r"
				+Lan.g(this,"The conversion works best if you are on the server.  Depending on the speed of your computer, it can be as fast as a few seconds, or it can take as long as 10 minutes.")
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return false;//If user clicks cancel, then close the program
			}
#endif
			Cursor.Current=Cursors.WaitCursor;
			ODThread odThread=new ODThread(ShowUpgradeProgress);//Prepare a thread that will show the progress of the upgrade.
#if !DEBUG
			if(!isSilent) {
				if(DataConnection.DBtype!=DatabaseType.MySql
				&& !MsgBox.Show(this,true,"If you have not made a backup, please Cancel and backup before continuing.  Continue?")) {
					return false;
				}
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				if(!Shared.MakeABackup(isSilent)) {
					Cursor.Current=Cursors.Default;
					if(isSilent) {
						FormOpenDental.ExitCode=101;//Database Backup failed
						Application.Exit();
					}
					return false;
				}
			}
			//We've been getting an increasing number of phone calls with databases that have duplicate preferences which is impossible
			//unless a user has gotten this far and another computer in the office is in the middle of an update as well.
			//The issue is most likely due to the blocking messageboxes above which wait indefinitely for user input right before upgrading the database.
			//This means that the cache for this computer could be stale and we need to manually refresh our cache to double check 
			//that the database isn't flagged as corrupt, an update isn't in progress, or that the database version hasn't changed (someone successfully updated already).
			Prefs.RefreshCache();
			//Now check the preferences that should stop this computer from executing an update.
			if(PrefC.GetBool(PrefName.CorruptedDatabase) 
				|| (PrefC.GetString(PrefName.UpdateInProgressOnComputerName)!="" && PrefC.GetString(PrefName.UpdateInProgressOnComputerName)!=Environment.MachineName))
			{
				//At this point, the pref "corrupted database" being true means that a computer is in the middle of running the upgrade script.
				//There will be another corrupted database check on start up which will take care of the scenario where this is truly a corrupted database.
				//Also, we need to make sure that the update in progress preference is set to this computer because we JUST set it to that value before entering this method.
				//If it has changed, we absolutely know without a doubt that another computer is trying to update at the same time.
				if(isSilent) {
					FormOpenDental.ExitCode=142;//Update is already in progress from another computer
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"An update is already in progress from another computer.");
				return false;
			}
			//Double check that the database version has not changed.  This check is here just in case another computer has successfully updated the database already.
			Version versionDatabase=new Version(PrefC.GetString(PrefName.DataBaseVersion));
			if(FromVersion!=versionDatabase) {
				if(isSilent) {
					FormOpenDental.ExitCode=143;//Database has already been updated from another computer
					Application.Exit();
					return false;
				}
				MsgBox.Show(this,"The database has already been updated from another computer.");
				return false;
			}
			try {
#endif
				if(FromVersion < new Version("7.5.17")) {//Insurance Plan schema conversion
					if(isSilent) {
						FormOpenDental.ExitCode=139;//Update must be done manually to fix Insurance Plan Schema
						Application.Exit();
						return false;
					}
					Cursor.Current=Cursors.Default;
					YN InsPlanConverstion_7_5_17_AutoMergeYN=YN.Unknown;
					if(FromVersion < new Version("7.5.1")) {
						FormInsPlanConvert_7_5_17 form=new FormInsPlanConvert_7_5_17();
						if(PrefC.GetBoolSilent(PrefName.InsurancePlansShared,true)) {
							form.InsPlanConverstion_7_5_17_AutoMergeYN=YN.Yes;
						}
						else {
							form.InsPlanConverstion_7_5_17_AutoMergeYN=YN.No;
						}
						form.ShowDialog();
						if(form.DialogResult==DialogResult.Cancel) {
							MessageBox.Show("Your database has not been altered.");
							return false;
						}
						InsPlanConverstion_7_5_17_AutoMergeYN=form.InsPlanConverstion_7_5_17_AutoMergeYN;
					}
					ConvertDatabases.Set_7_5_17_AutoMerge(InsPlanConverstion_7_5_17_AutoMergeYN);//does nothing if this pref is already present for some reason.
					Cursor.Current=Cursors.WaitCursor;
				}
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,true);
				}
				ConvertDatabases.FromVersion=FromVersion;
#if !DEBUG
				//Typically the UpdateInProgressOnComputerName preference will have already been set within FormUpdate.
				//However, the user could have cancelled out of FormUpdate after successfully downloading the Setup.exe
				//OR the Setup.exe could have been manually sent to our customer (during troubleshooting with HQ).
				//For those scenarios, the preference will be empty at this point and we need to let other computers know that an update going to start.
				//Updating the string (again) here will guarantee that all computers know an update is in fact in progress from this machine.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,Environment.MachineName);
#endif
				//Show a progress window that will indecate to the user that there is an active update in progress.  Currently okay to show during isSilent.
				odThread.Name="ConvertDatabasesProgressThread";
				odThread.Start();
				ConvertDatabases.To2_8_2();//begins going through the chain of conversion steps
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","DEFCON 1"));//Send the phrase that closes the window.
				odThread.QuitSync(500);//Give the thread half of a second to gracefully close (should be instantaneous).
				Cursor.Current=Cursors.Default;
				if(FromVersion>=new Version("3.4.0")) {
					//CacheL.Refresh(InvalidType.Prefs);//or it won't know it has to update in the next line.
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false,true);//more forceful refresh in order to properly change flag
				}
				if(!isSilent) {
					MsgBox.Show(this,"Database update successful");
				}
				Cache.Refresh(InvalidType.Prefs);
				return true;
#if !DEBUG
			}
			catch(System.IO.FileNotFoundException e) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","DEFCON 1"));//Send the phrase that closes the window.
				odThread.QuitSync(500);//Give the thread half of a second to gracefully close (should be instantaneous).
				if(isSilent) {
					FormOpenDental.ExitCode=160;//File not found exception
					if(FromVersion>=new Version("3.4.0")) {
						Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
					}
					Application.Exit();
					return false;
				}
				MessageBox.Show(e.FileName+" "+Lan.g(this,"could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
				}
				//Prefs.Refresh();
				return false;
			}
			catch(System.IO.DirectoryNotFoundException) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","DEFCON 1"));//Send the phrase that closes the window.
				odThread.QuitSync(500);//Give the thread half of a second to gracefully close (should be instantaneous).
				if(isSilent) {
					FormOpenDental.ExitCode=160;//ConversionFiles folder could not be found
					if(FromVersion>=new Version("3.4.0")) {
						Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
					}
					Application.Exit();
					return false;
				}
				MessageBox.Show(Lan.g(this,"ConversionFiles folder could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
				}
				//Prefs.Refresh();
				return false;
			}
			catch(Exception ex) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","DEFCON 1"));//Send the phrase that closes the window.
				odThread.QuitSync(500);//Give the thread half of a second to gracefully close (should be instantaneous).
				if(isSilent) {
					FormOpenDental.ExitCode=201;//Database was corrupted due to an update failure
					Application.Exit();
					return false;
				}
				//	MessageBox.Show();
				MessageBox.Show(ex.Message+"\r\n\r\n"
					+Lan.g(this,"Conversion unsuccessful. Your database is now corrupted and you cannot use it.  Please contact us."));
				//Then, application will exit, and database will remain tagged as corrupted.
				return false;
			}
#endif
		}

		private static void ShowUpgradeProgress(ODThread odThread) {
			FormProgressStatus FormPS=new FormProgressStatus("ConvertDatabases");
			FormPS.TopMost=true;//Make this window show on top of ALL other windows.
			FormPS.ShowDialog();
		}

	}

}