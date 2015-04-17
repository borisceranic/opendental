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

		///<summary>Gets one EmailAttach from the db. Used by Patient Portal.</summary>
		public static EmailAttach GetOne(long emailAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EmailAttach>(MethodBase.GetCurrentMethod(),emailAttachNum);
			}
			return Crud.EmailAttachCrud.SelectOne(emailAttachNum);
		}

		///<summary>Creates the file name to be used for an attachment when given the display name of the file.  Uses time stamps and random strings to virtually guarantee file name uniqueness, although does not truly guarantee a unique file name.</summary>
		private static string CreateActualFileName(string displayFileName) {
			//Display name is tacked onto actual file name last as to ensure file extensions are the same.
			return DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()
				+"_"+MiscUtils.CreateRandomAlphaNumericString(4)+"_"+displayFileName;
		}

		///<summary>Throws exceptions.  Creates a new file inside of the email attachment path (inside OpenDentImages) and returns an EmailAttach object referencing the new file.  The displayFileName is what the user sees.  The actual file name of the saved file is partially based on the displayFileName, so that the actual files are easier to locate.</summary>
		public static EmailAttach CreateAttach(string displayFileName,byte[] arrayData) {
			//No need to check RemotingRole; no call to db.
			string displayFileNameAdjusted=displayFileName;
			if(String.IsNullOrEmpty(displayFileName)) {
				//This could only happen for malformed incoming emails, but should not happen.  Name uniqueness is virtually guaranteed below.
				//The actual file name will not have an extension, so the user will be asked to pick the program to open the attachment with when
				//the attachment is double-clicked.
				displayFileNameAdjusted="attach";
			}
			string attachDir=GetAttachPath();
			string actualFileName=CreateActualFileName(displayFileNameAdjusted);
			string attachFilePath=ODFileUtils.CombinePaths(attachDir,actualFileName);
			while(File.Exists(attachFilePath)) {
				actualFileName=CreateActualFileName(displayFileNameAdjusted);
				attachFilePath=ODFileUtils.CombinePaths(attachDir,actualFileName);
			}
			return CreateAttachQuick(displayFileName,actualFileName,arrayData);
		}

		///<summary>Throws exceptions.  Creates a new file inside of the email attachment path (inside OpenDentImages) and returns an EmailAttach object referencing the new file.  The displayFileName is what the user sees.  If a file already exists matching the actualFileName, then an exception will occur.</summary>
		private static EmailAttach CreateAttachQuick(string displayFileName,string actualFileName,byte[] arrayData) {
			//No need to check RemotingRole; no call to db.
			EmailAttach emailAttach=new EmailAttach();
			emailAttach.DisplayedFileName=displayFileName;
			emailAttach.ActualFileName=actualFileName;
			string attachFilePath=ODFileUtils.CombinePaths(GetAttachPath(),emailAttach.ActualFileName);
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

	}
}