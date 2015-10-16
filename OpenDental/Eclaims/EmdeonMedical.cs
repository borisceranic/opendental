using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Ionic.Zip;

namespace OpenDental.Eclaims
{
	public class EmdeonMedical{

		private static string emdeonITSUrlTest="https://cert.its.emdeon.com/ITS/ITSWS.asmx";//test url
		private static string emdeonITSUrl="https://its.emdeon.com/ITS/ITSWS.asmx";//production url
		public static string ErrorMessage="";

		///<summary></summary>
		public EmdeonMedical()
		{			
		}

		///<summary>Returns true if the communications were successful, and false if they failed. If they failed, a rollback will happen automatically by deleting the previously created X12 file. The batchnum is supplied for the possible rollback.  Also used for mail retrieval.</summary>
		public static bool Launch(Clearinghouse clearinghouseClin,int batchNum,EnumClaimMedType medType){ //called from Eclaims.cs. Clinic-level clearinghouse passed in.
			string batchFile="";
			try {
				if(!Directory.Exists(clearinghouseClin.ExportPath)) {
					throw new Exception("Clearinghouse export path is invalid.");
				}
				//We make sure to only send the X12 batch file for the current batch, so that if there is a failure, then we know
				//for sure that we need to reverse the batch. This will also help us avoid any exterraneous/old batch files in the
				//same directory which might be left if there is a permission issue when trying to delete the batch files after processing.
				batchFile=Path.Combine(clearinghouseClin.ExportPath,"claims"+batchNum+".txt");
				//byte[] fileBytes=File.ReadAllBytes(batchFile);//unused
				MemoryStream zipMemoryStream=new MemoryStream();
				ZipFile tempZip=new ZipFile();
				tempZip.AddFile(batchFile,"");
				tempZip.Save(zipMemoryStream);
				tempZip.Dispose();
				zipMemoryStream.Position=0;
				byte[] zipFileBytes=zipMemoryStream.GetBuffer();
				string zipFileBytesBase64=Convert.ToBase64String(zipFileBytes);
				zipMemoryStream.Dispose();
				if(clearinghouseClin.ISA15=="P") {//production interface
					string messageType="MCD";//medical
					if(medType==EnumClaimMedType.Institutional) {
						messageType="HCD";
					}
					else if(medType==EnumClaimMedType.Dental) {
						//messageType="DCD";//not used/tested yet, but planned for future.
					}
					EmdeonITS.ITSWS itsws=new EmdeonITS.ITSWS();
					itsws.Url=emdeonITSUrl;
					EmdeonITS.ITSReturn response=itsws.PutFileExt(clearinghouseClin.LoginID,clearinghouseClin.Password,messageType,Path.GetFileName(batchFile),zipFileBytesBase64);
					if(response.ErrorCode!=0) { //Batch submission successful.
						throw new Exception("Emdeon rejected all claims in the current batch file "+batchFile+". Error number from Emdeon: "+response.ErrorCode+". Error message from Emdeon: "+response.Response);
					}
				}
				else {//test interface
					string messageType="MCT";//medical
					if(medType==EnumClaimMedType.Institutional) {
						messageType="HCT";
					}
					else if(medType==EnumClaimMedType.Dental) {
						//messageType="DCT";//not used/tested yet, but planned for future.
					}
					EmdeonITSTest.ITSWS itswsTest=new EmdeonITSTest.ITSWS();
					itswsTest.Url=emdeonITSUrlTest;
					EmdeonITSTest.ITSReturn responseTest=itswsTest.PutFileExt(clearinghouseClin.LoginID,clearinghouseClin.Password,messageType,Path.GetFileName(batchFile),zipFileBytesBase64);
					if(responseTest.ErrorCode!=0) { //Batch submission successful.
						throw new Exception("Emdeon rejected all claims in the current batch file "+batchFile+". Error number from Emdeon: "+responseTest.ErrorCode+". Error message from Emdeon: "+responseTest.Response);
					}
				}
			}
			catch(Exception e) {
				ErrorMessage=e.Message;
				x837Controller.Rollback(clearinghouseClin,batchNum);
				return false;
			}
			finally {
				try {
					if(batchFile!="") {
						File.Delete(batchFile);
					}
				}
				catch {
					ErrorMessage="Failed to remove batch file"+batchFile+". Probably due to a permission issue.  Check folder permissions and manually delete.";
				}
			}
			return true;
		}

		public static bool Retrieve(Clearinghouse clearinghouseClin) {//called from FormClaimReports. clinic-level clearinghouse passed in.
			try {
				if(!Directory.Exists(clearinghouseClin.ResponsePath)) {
					throw new Exception("Clearinghouse response path is invalid.");
				}
				bool reportsDownloaded=false;
				if(clearinghouseClin.ISA15=="P") {//production interface
					string[] messageTypes=new string[] { 
						"MCD", //Medical
						"HCD", //Institutional
						//"DCD"  //Dental. Planned for future.
					};
					for(int i=0;i<messageTypes.Length;i++) {
						EmdeonITS.ITSWS itsws=new EmdeonITS.ITSWS();
						itsws.Url=emdeonITSUrl;
						//Download the most up to date reports, but do not delete them from the server yet.
						EmdeonITS.ITSReturn response=itsws.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,messageTypes[i]+"G");
						if(response.ErrorCode==0) { //Report retrieval successful.
							string reportFileDataBase64=response.Response;
							byte[] reportFileDataBytes=Convert.FromBase64String(reportFileDataBase64);
							string reportFilePath=CodeBase.ODFileUtils.CreateRandomFile(clearinghouseClin.ResponsePath,".zip");
							File.WriteAllBytes(reportFilePath,reportFileDataBytes);
							reportsDownloaded=true;
							//Now that the file has been saved, remove the report file from the Emdeon production server.
							//If deleting the report fails, we don't care because that will simply mean that we download it again next time.
							//Thus we don't need to check the status after this next call.
							itsws.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,messageTypes[i]+"D");
						}
						else if(response.ErrorCode!=209) { //Report retrieval failure, excluding the error that can be returned when the mailbox is empty.
							throw new Exception("Failed to get reports. Error number from Emdeon: "+response.ErrorCode+". Error message from Emdeon: "+response.Response);
						}
					}
				}
				else { //test interface
					string[] messageTypes=new string[] { 
						"MCT", //Medical
						"HCT", //Institutional
						//"DCT"  //Dental. Planned for future.
					};
					for(int i=0;i<messageTypes.Length;i++) {
						EmdeonITSTest.ITSWS itswsTest=new EmdeonITSTest.ITSWS();
						itswsTest.Url=emdeonITSUrlTest;
						//Download the most up to date reports, but do not delete them from the server yet.
						EmdeonITSTest.ITSReturn responseTest=itswsTest.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,messageTypes[i]+"G");
						if(responseTest.ErrorCode==0) { //Report retrieval successful.
							string reportFileDataBase64=responseTest.Response;
							byte[] reportFileDataBytes=Convert.FromBase64String(reportFileDataBase64);
							string reportFilePath=CodeBase.ODFileUtils.CreateRandomFile(clearinghouseClin.ResponsePath,".zip");
							File.WriteAllBytes(reportFilePath,reportFileDataBytes);
							reportsDownloaded=true;
							//Now that the file has been saved, remove the report file from the Emdeon test server.
							//If deleting the report fails, we don't care because that will simply mean that we download it again next time.
							//Thus we don't need to check the status after this next call.
							itswsTest.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,messageTypes[i]+"D");
						}
						else if(responseTest.ErrorCode!=209) { //Report retrieval failure, excluding the error that can be returned when the mailbox is empty.
							throw new Exception("Failed to get reports. Error number from Emdeon: "+responseTest.ErrorCode+". Error message from Emdeon: "+responseTest.Response);
						}
					}
				}
				if(!reportsDownloaded) {
					ErrorMessage="Report mailbox is empty.";
				}
			}
			catch(Exception ex) {
				ErrorMessage=ex.Message; 
				return false;
			}
			return true;
		}

	}
}