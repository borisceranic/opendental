using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAttaches{

		public static long Insert(EmailAttach attach) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				attach.EmailAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),attach);
				return attach.EmailAttachNum;
			}
			return Crud.EmailAttachCrud.Insert(attach);
		}

		public static List<EmailAttach> GetForEmail(long emailMessageNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailAttach>>(MethodBase.GetCurrentMethod(),emailMessageNum);
			}
			string command="SELECT * FROM emailattach WHERE EmailMessageNum="+POut.Long(emailMessageNum);
			return Crud.EmailAttachCrud.SelectMany(command);
		}

		public static List<EmailAttach> GetForEmails(List<long> listEmailMessageNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailAttach>>(MethodBase.GetCurrentMethod(),listEmailMessageNums);
			}
			List<EmailAttach> listAttaches=new List<EmailAttach>();
			if(listEmailMessageNums.Count < 1) {
				return listAttaches;
			}
			string command="SELECT * FROM emailattach WHERE EmailMessageNum IN("+string.Join(",",listEmailMessageNums)+")";
			return Crud.EmailAttachCrud.SelectMany(command);
		}

		///<summary>Gets one EmailAttach from the db. Used by Patient Portal.</summary>
		public static EmailAttach GetOne(long emailAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EmailAttach>(MethodBase.GetCurrentMethod(),emailAttachNum);
			}
			return Crud.EmailAttachCrud.SelectOne(emailAttachNum);
		}

		///<summary>Throws exceptions.  Creates a new file within the Out subfolder of the email attachment path (inside OpenDentImages) and returns an EmailAttach object referencing the new file.  The displayFileName will always contain valid file name characters, because it is either a hard coded value or is based on an existing valid file name.  The actual file name will end with the displayFileName, so that the actual files are easier to locate and have the same file extension as the displayedFileName.</summary>
		public static EmailAttach CreateAttach(string displayedFileName,byte[] arrayData) {
			return CreateAttach(displayedFileName,"",arrayData,true);
		}

		///<summary>Throws exceptions.  Creates a new file inside of the email attachment path (inside OpenDentImages) and returns an EmailAttach object referencing the new file.  If isOutbound is true, then the file will be saved to the "Out" subfolder, otherwise the file will be saved to the "In" subfolder.  The displayFileName will always contain valid file name characters, because it is either a hard coded value or is based on an existing valid file name.  If a file already exists matching the actualFileName, then an exception will occur.  Set actualFileName to empty string to generate a unique actual file name.  If the actual file name is generated, then actual file name will end with the displayFileName, so that the actual files are easier to locate and have the same file extension as the displayedFileName.</summary>
		public static EmailAttach CreateAttach(string displayedFileName,string actualFileName,byte[] arrayData,bool isOutbound) {
			//No need to check RemotingRole; no call to db.
			EmailAttach emailAttach=new EmailAttach();
			emailAttach.DisplayedFileName=displayedFileName;
			if(String.IsNullOrEmpty(emailAttach.DisplayedFileName)) {
				//This could only happen for malformed incoming emails, but should not happen.  Name uniqueness is virtually guaranteed below.
				//The actual file name will not have an extension, so the user will be asked to pick the program to open the attachment with when
				//the attachment is double-clicked.
				emailAttach.DisplayedFileName="attach";
			}
			string attachDir=GetAttachPath();
			string subDir="In";
			if(isOutbound) {
				subDir="Out";
			}
			if(!Directory.Exists(ODFileUtils.CombinePaths(attachDir,subDir))) {
				Directory.CreateDirectory(ODFileUtils.CombinePaths(attachDir,subDir));
			}
			if(String.IsNullOrEmpty(actualFileName)) {
				while(String.IsNullOrEmpty(emailAttach.ActualFileName) || File.Exists(ODFileUtils.CombinePaths(attachDir,emailAttach.ActualFileName))) {
					//Display name is tacked onto actual file name last as to ensure file extensions are the same.
					emailAttach.ActualFileName=ODFileUtils.CombinePaths(subDir,
						DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()
							+"_"+MiscUtils.CreateRandomAlphaNumericString(4)+"_"+emailAttach.DisplayedFileName);
				}
			}
			else {
				//The caller wants a specific actualFileName.  Use the given name as is.
				emailAttach.ActualFileName=ODFileUtils.CombinePaths(subDir,actualFileName);
			}
			string attachFilePath=ODFileUtils.CombinePaths(attachDir,emailAttach.ActualFileName);
			if(File.Exists(attachFilePath)) {
				throw new ApplicationException("Email attachment could not be saved because a file with the same name already exists.");
			}
			try {
				File.WriteAllBytes(attachFilePath,arrayData);
			}
			catch(Exception ex) {
				try {
					if(File.Exists(attachFilePath)) {
						File.Delete(attachFilePath);
					}
				}
				catch {
					//We tried our best to delete the file, and there is nothing else to try.
				}
				throw ex;//Show the initial error message, even if the Delete() failed.
			}
			return emailAttach;
		}

		public static string GetAttachPath() {
			//No need to check RemotingRole; no call to db.
			string attachPath;
			if(PrefC.AtoZfolderUsed) {
				attachPath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"EmailAttachments");
				if(!Directory.Exists(attachPath)) {
					Directory.CreateDirectory(attachPath);
				}
			}
			else {
				//For users who have the A to Z folders disabled, there is no defined image path, so we
				//have to use a temp path.  This means that the attachments might be available immediately afterward,
				//but probably not later.
				attachPath=ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental");//Have to use Path.GetTempPath() here instead of PrefL.GetTempPathFolder() because we can't access PrefL.
			}
			return attachPath;
		}

		///<summary>Returns all EmailAttaches assocaited to a specific EmailTemplateNum.</summary>
		public static List<EmailAttach> GetForTemplate(long emailTemplateNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailAttach>>(MethodBase.GetCurrentMethod(),emailTemplateNum);
			}
			string command="SELECT * FROM emailattach WHERE EmailTemplateNum="+POut.Long(emailTemplateNum);
			return Crud.EmailAttachCrud.SelectMany(command);
		}

		///<summary>Syncs a given list of EmailAttaches to a list of old EmailAttaches.</summary>
		public static void Sync(List<EmailAttach> emailAttachNew,List<EmailAttach> emailAttachOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAttachNew,emailAttachOld);
			}
			Crud.EmailAttachCrud.Sync(emailAttachNew,emailAttachOld);
		}

	}
}