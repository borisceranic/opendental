using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Linq;
using CodeBase;
using System.Reflection;

namespace OpenDentBusiness.WebBridges {
	///<summary>RESTful bridge to podium service. Without using REST Sharp or JSON libraries this code might not work properly.</summary>
	public class Podium {

		public static DateTime DateTimeLastRan=DateTime.MinValue;
		///<summary>Amount of time to wait inbetween trying to send Podium review invitations.</summary>
		public static int PodiumThreadIntervalMS=(int)TimeSpan.FromMinutes(5).TotalMilliseconds;

		///<summary></summary>
		public Podium() {

		}

		public static void ThreadPodiumSendInvitations(bool isEConnector) {
			long programNum=Programs.GetProgramNum(ProgramName.Podium);
			//Consider blocking re-entrance if this hasn't finished.
			//Only send invitations if the program link is enabled, the computer name is set to this computer, and eConnector is not set to send invitations 
			if(!Programs.IsEnabled(ProgramName.Podium) 
				|| !ODEnvironment.IdIsThisComputer(ProgramProperties.GetPropVal(programNum,PropertyDescs.ComputerNameOrIP)) 
				|| ProgramProperties.GetPropVal(programNum,PropertyDescs.UseEConnector)!=POut.Bool(isEConnector)) 
			{ 
				return;
			}
			//Keep a consistant "Now" timestamp throughout this method.
			DateTime nowDT=MiscData.GetNowDateTime();
			if(Podium.DateTimeLastRan==DateTime.MinValue) {//First time running the thread.
				Podium.DateTimeLastRan=nowDT.AddMilliseconds(-PodiumThreadIntervalMS);
			}
			ReviewInvitationTrigger newPatTrigger=PIn.Enum<ReviewInvitationTrigger>(ProgramProperties.GetPropVal(programNum,PropertyDescs.NewPatientTriggerType));
			ReviewInvitationTrigger existingPatTrigger=PIn.Enum<ReviewInvitationTrigger>(ProgramProperties.GetPropVal(programNum,PropertyDescs.ExistingPatientTriggerType));
			List<Appointment> listNewPatAppts=GetAppointmentsToSendReview(newPatTrigger,programNum,true);
			foreach(Appointment apptCur in listNewPatAppts) {
				Podium.SendData(Patients.GetPat(apptCur.PatNum),apptCur.ClinicNum);
			}
			List<Appointment> listExistingPatAppts=GetAppointmentsToSendReview(existingPatTrigger,programNum,false);
			foreach(Appointment apptCur in listExistingPatAppts) {
				Podium.SendData(Patients.GetPat(apptCur.PatNum),apptCur.ClinicNum);
			}
			Podium.DateTimeLastRan=nowDT;
		}

		private static List<Appointment> GetAppointmentsToSendReview(ReviewInvitationTrigger trigger,long programNum,bool isNewPatient) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),trigger,programNum,isNewPatient);
			}
			string minutesToWaitCompleted=ProgramProperties.GetPropVal(programNum,PropertyDescs.ApptSetCompletedMinutes);
			string minutesToWaitTimeArrived=ProgramProperties.GetPropVal(programNum,PropertyDescs.ApptTimeArrivedMinutes);
			string minutesToWaitTimeDismissed=ProgramProperties.GetPropVal(programNum,PropertyDescs.ApptTimeDismissedMinutes);
			string command="SELECT * "
				+"FROM appointment "
				+"LEFT JOIN securitylog ON securitylog.FKey=appointment.AptNum "
					+"AND securitylog.PermType="+POut.Int((int)Permissions.AppointmentEdit)+" AND securitylog.LogText LIKE '%Set Complete%' "
				+"LEFT JOIN commlog ON commlog.PatNum=appointment.PatNum "
					+"AND commlog.CommSource="+POut.Int((int)CommItemSource.ProgramLink)+" "
					+"AND DATE(commlog.DateTimeEnd)="+DbHelper.Curdate()+" "
					+"AND commlog.ProgramNum="+POut.Long(programNum)+" "
				+"WHERE ISNULL(commlog.PatNum) AND appointment.AptDateTime BETWEEN "+DbHelper.Curdate()+" AND "+DbHelper.Now()+" + INTERVAL 1 HOUR "//Hard code an hour to allow for appointments that have an early DateTimeArrived
				+"AND appointment.IsNewPatient="+POut.Bool(isNewPatient)+" ";
			if(trigger==ReviewInvitationTrigger.AppointmentCompleted) {
				command+="AND appointment.AptStatus="+POut.Int((int)ApptStatus.Complete)+" "
					+"AND NOT ISNULL(securitylog.PatNum) "
					+"AND securitylog.LogDateTime + INTERVAL "+minutesToWaitCompleted+" MINUTE <="+DbHelper.Now()+" ";
			}
			else if(trigger==ReviewInvitationTrigger.AppointmentTimeArrived) {
				command+="AND appointment.AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.ASAP)+","+POut.Int((int)ApptStatus.Complete)+") "
					+"AND ((appointment.AptStatus="+POut.Int((int)ApptStatus.Complete)+" AND NOT ISNULL(securitylog.PatNum) AND securitylog.LogDateTime + INTERVAL "+minutesToWaitCompleted+" MINUTE <="+DbHelper.Now()+") "
					+"OR (appointment.DateTimeArrived>"+DbHelper.Curdate()+" AND appointment.DateTimeArrived + INTERVAL "+minutesToWaitTimeArrived+" MINUTE<="+DbHelper.Now()+")) ";
			}
			else if(trigger==ReviewInvitationTrigger.AppointmentTimeDismissed) {
				command+="AND appointment.AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.ASAP)+","+POut.Int((int)ApptStatus.Complete)+") "
					+"AND ((appointment.AptStatus="+POut.Int((int)ApptStatus.Complete)+" AND NOT ISNULL(securitylog.PatNum) AND securitylog.LogDateTime + INTERVAL 90 MINUTE <="+DbHelper.Now()+") "
					+"OR (appointment.DateTimeDismissed>"+DbHelper.Curdate()+" AND appointment.DateTimeDismissed + INTERVAL "+minutesToWaitTimeDismissed+" MINUTE<="+DbHelper.Now()+")) ";
			}
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Throws exceptions.</summary>
		public static void ShowPage() {
			try {
				if(Programs.IsEnabled(ProgramName.Podium)) {
					Process.Start("http://www.opendental.com/manual/podiumdashboard.html");
				}
				else {
					Process.Start("http://www.opendental.com/manual/podiumod.html");
				}
			}
			catch {
				throw new Exception(Lans.g("Podium","Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet and then try again."));
			}
		}

		///<summary>Tries each of the phone numbers provided in the list one at a time until it succeeds.</summary>
		public static bool SendData(Patient pat,long clinicNum) {
			List<string> listPhoneNumbers=new List<string>() { pat.WirelessPhone,pat.HmPhone };
			string firstName=pat.FName;
			string lastName=pat.LName;
			string emailIn=pat.Email;
			string isTestString="false";
			int statusCode=200;
#if DEBUG
			isTestString="true";
#endif
			for(int i=0;i<listPhoneNumbers.Count;i++) {
				string phoneNumber=new string(listPhoneNumbers[i].Where(x => char.IsDigit(x)).ToArray());
				if(phoneNumber=="") {
					continue;
				}
				string apiUrl="https://podium.co/api/v1/review_invitations";
				string apiToken=ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.Podium),PropertyDescs.APIToken);//I might be able to use _programNum here if static is per class like I think it is
				string locationId=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetProgramNum(ProgramName.Podium),PropertyDescs.LocationID,clinicNum);
				try {
					using(WebClientEx client=new WebClientEx()) {
						client.Headers[HttpRequestHeader.Accept]="application/json";
						client.Headers[HttpRequestHeader.ContentType]="application/json";
						client.Headers[HttpRequestHeader.Authorization]="Token token=\""+apiToken+"\"";
						client.Encoding=UnicodeEncoding.UTF8;
						string bodyJson=string.Format(@"
						{{
							""location_id"": ""{0}"",
							""phone_number"": ""{1}"",
							""customer"":  {{
								""first_name"": ""{2}"",
								""last_name"": ""{3}"",
								""email"": ""{4}""
							}},
							""test"": {5}
						}}",locationId,phoneNumber,firstName,lastName,emailIn,isTestString);
						//Post with Authorization headers and a body comprised of a JSON serialized anonymous type.
						client.UploadString(apiUrl,"POST",bodyJson);
						if(client.StatusCode==HttpStatusCode.OK) {
							MakeCommlog(pat,phoneNumber,statusCode);
							return true;
						}
						else {
							//eventlogging should also go here for non 200 status.
						}
					}
				}
				catch(Exception ex) {
					if(ex is WebException) {
						statusCode=(int)((HttpWebResponse)((WebException)ex).Response).StatusCode;
					}
					//Do nothing because a verbose commlog will be made below if all phone numbers fail.
				}
			}
			MakeCommlog(pat,"",statusCode);
			//explicitly failed or did not succeed.
			return false;
			//Sample Request:

			//Accept: 'application/json's
			//Content-Type: 'application/json'
			//Authorization: 'Token token="my_dummy_token"'
			//Body:
			//{
			//	"location_id": "54321",
			//	"phone_number": "1234567890",
			//	"customer": {
			//		"first_name": "Johnny",
			//		"last_name": "Appleseed",
			//		"email": "johnny.appleseed@gmail.com"
			//	},
			//	"test": true
			//}
			//NOTE:  There will never be a value after "customer": although it was initially interpreted that there would be a "new" flag there.
		}

		private static void MakeCommlog(Patient pat,string phoneNumber,int statusCode) {
			string commText=Lans.g("Podium","Podium review invitation request successfully sent")+". \r\n";
			if(statusCode!=200) {
				commText=Lans.g("Podium","Podium review invitation request failed to send.");
				if(statusCode==422) {//422 is Unprocessable Entity, which is sent in this case when a phone number has received an invite already.
					commText+="  "+Lans.g("Podium","The request failed because an identical request was previously sent.");
				}
				commText+="\r\n";
			}
			commText+=Lans.g("Podium","The information sent in the request was")+": \r\n"
				+Lans.g("Podium","First name")+": \""+pat.FName+"\", "
				+Lans.g("Podium","Last name")+": \""+pat.LName+"\", "
				+Lans.g("Podium","Email")+": \""+pat.Email+"\"";
			if(phoneNumber!="") {//If successful.
				commText+=", "+Lans.g("Podium","Phone number")+": \""+phoneNumber+"\"";
			}
			else {
				string wirelessPhone=new string(pat.WirelessPhone.Where(x => char.IsDigit(x)).ToArray());
				string homePhone=new string(pat.HmPhone.Where(x => char.IsDigit(x)).ToArray());
				List<string> phonesTried=new List<string> { wirelessPhone,homePhone }.FindAll(x => x!="");
				string phoneNumbersTried=", "+Lans.g("Podium","No valid phone number found.");
				if(phonesTried.Count>0) {
					phoneNumbersTried=", "+Lans.g("Podium","Phone numbers tried")+": "+string.Join(", ",phonesTried);
				}
				commText+=phoneNumbersTried;
			}
			long programNum=Programs.GetProgramNum(ProgramName.Podium);
			Commlog commlogCur=new Commlog();
			commlogCur.CommDateTime=DateTime.Now;
			commlogCur.DateTimeEnd=DateTime.Now;
			commlogCur.PatNum=pat.PatNum;
			commlogCur.UserNum=0;//run from server, no valid CurUser
			commlogCur.CommSource=CommItemSource.ProgramLink;
			commlogCur.ProgramNum=programNum;
			commlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			commlogCur.Note=commText;
			Commlogs.Insert(commlogCur);
		}

		private class WebClientEx:WebClient {
			//http://stackoverflow.com/questions/3574659/how-to-get-status-code-from-webclient
			private WebResponse _mResp = null;

			protected override WebResponse GetWebResponse(WebRequest req,IAsyncResult ar) {
				return _mResp = base.GetWebResponse(req,ar);
			}

			public HttpStatusCode StatusCode {
				get {
					HttpWebResponse httpWebResponse=_mResp as HttpWebResponse;
					return httpWebResponse!=null?httpWebResponse.StatusCode:HttpStatusCode.OK;
				}
			}
		}

		public class PropertyDescs {
			public static string ComputerNameOrIP="Enter your computer name or IP (required)";
			public static string APIToken="Enter your API Token (required)";
			public static string LocationID="Enter your Location ID (required)";
			public static string UseEConnector="Enter 0 to use Open Dental for sending review invitations, or 1 to use eConnector";
			public static string ApptSetCompletedMinutes="Send after appointment completed (minutes)";
			public static string ApptTimeArrivedMinutes="Send after appointment time arrived (minutes)";
			public static string ApptTimeDismissedMinutes="Send after appointment time dismissed (minutes)";
			public static string ExistingPatientTriggerType="Existing patient trigger type";
			public static string NewPatientTriggerType="New patient trigger type";
			public static string DisableAdvertising="Disable Advertising";
		}
	}
		public enum ReviewInvitationTrigger {
			AppointmentCompleted,
			AppointmentTimeArrived,
			AppointmentTimeDismissed
		}
}







