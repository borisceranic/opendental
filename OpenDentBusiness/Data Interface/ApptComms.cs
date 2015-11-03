using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ApptComms{
		
		///<summary></summary>
		public static List<ApptComm> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptComm>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM apptcomm WHERE PatNum = "+POut.Long(patNum);
			return Crud.ApptCommCrud.SelectMany(command);
		}

		///<summary>Gets one ApptComm from the db.</summary>
		public static ApptComm GetOne(long apptCommNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ApptComm>(MethodBase.GetCurrentMethod(),apptCommNum);
			}
			return Crud.ApptCommCrud.SelectOne(apptCommNum);
		}

		///<summary>Gets all ApptComm entries from the db.</summary>
		public static List<ApptComm> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptComm>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM apptcomm";
			return Crud.ApptCommCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ApptComm apptComm){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				apptComm.ApptCommNum=Meth.GetLong(MethodBase.GetCurrentMethod(),apptComm);
				return apptComm.ApptCommNum;
			}
			return Crud.ApptCommCrud.Insert(apptComm);
		}

		///<summary></summary>
		public static void Delete(long apptCommNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptCommNum);
				return;
			}
			Crud.ApptCommCrud.Delete(apptCommNum);
		}

		///<summary></summary>
		public static void DeleteForAppt(long apptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptNum);
				return;
			}
			string command="DELETE FROM apptcomm WHERE ApptNum="+POut.Long(apptNum);
			Db.NonQ(command);
		}

		///<summary>Send Appointment reminders for all ApptComm items.</summary>
		public static string SendReminders() {
			List<ApptComm> listApptComms=GetAll();
			int dayInterval=PrefC.GetInt(PrefName.ApptReminderDayInterval);
			int hourInterval=PrefC.GetInt(PrefName.ApptReminderHourInterval);
			string errorText="";
			for(int i=0;i<listApptComms.Count;i++) {
				if(listApptComms[i].ApptCommType==IntervalType.Daily && (DateTime.Now-listApptComms[i].DateTimeSend).Days<0) {
					continue;//It's not currently enough days prior to the appointment to send a reminder.
				}
				if(listApptComms[i].ApptCommType==IntervalType.Hourly && (DateTime.Now-listApptComms[i].DateTimeSend).Hours<0) {
					continue;//It's the correct number of days but current hour isn't in the time interval.
				}
				if(listApptComms[i].DateTimeSend<(DateTime.Now-TimeSpan.FromDays(1))) {//Entries in the past.  Delete?  This shouldn't happen.  If we don't delete they'll be sent.  Maybe one day in the past is safe to delete?
					Delete(listApptComms[i].ApptCommNum);
					continue;
				}
				//It's within the correct day interval and within the correct hour interval, time to send a reminder.
				bool sendAll=PrefC.GetBool(PrefName.ApptReminderSendAll);
				string[] arraySendPriorities=PrefC.GetString(PrefName.ApptReminderSendOrder).Split(',');
				Appointment appt=Appointments.GetOneApt(listApptComms[i].ApptNum);
				Family family=Patients.GetFamily(appt.PatNum);
				Patient pat=family.GetPatient(appt.PatNum);
				if(sendAll) {
					//Attempt to send both email and text reminder no matter what.
					//First we'll attempt to send via email.
					string emailError=SendEmail(pat,family,appt,dayInterval,hourInterval);
					if(emailError=="") {
						Commlog comm=new Commlog();
						comm.Mode_=CommItemMode.Email;
						comm.CommDateTime=DateTime.Now;
						comm.CommSource=CommItemSource.User;
						comm.Note=Lans.g("ApptComms","Appointment reminder emailed.");
						comm.PatNum=pat.PatNum;
						comm.SentOrReceived=CommSentOrReceived.Sent;
						comm.UserNum=0;
						Commlogs.Insert(comm);
					}
					errorText+=emailError;
					//Second we'll attempt to send via text.
					string textError=SendText(pat,family,appt,dayInterval,hourInterval);		
					if(textError=="") {
						Commlog comm=new Commlog();
						comm.Mode_=CommItemMode.Text;
						comm.CommDateTime=DateTime.Now;
						comm.CommSource=CommItemSource.User;
						comm.Note=Lans.g("ApptComms","Appointment reminder texted.");
						comm.PatNum=pat.PatNum;
						comm.SentOrReceived=CommSentOrReceived.Sent;
						comm.UserNum=0;
						Commlogs.Insert(comm);
					}
					errorText+=textError;
					if(errorText=="") {//If both are successful, delete.  I think perhaps we can delete if even only one is successful, but that's an executive decision.
						Delete(listApptComms[i].ApptCommNum);
					}
				}
				else {
					//Attempt to send reminders based on the priority of email/text/preferred.  Only attempt other methods if the previous priority failed. 
 					Commlog comm=new Commlog();
					for(int j=0;j<arraySendPriorities.Length;i++) {
						CommType priority=(CommType)PIn.Int(arraySendPriorities[i]);
						string error="";
						if(priority==CommType.Preferred) {
							if(pat.PreferContactMethod==ContactMethod.Email) {
								error=SendEmail(pat,family,appt,dayInterval,hourInterval);
								comm.Mode_=CommItemMode.Email;
							}
							else if(pat.PreferContactMethod==ContactMethod.TextMessage) {
								error=SendText(pat,family,appt,dayInterval,hourInterval);
								comm.Mode_=CommItemMode.Text;
							}
						}
						if(priority==CommType.Email) {
							error=SendEmail(pat,family,appt,dayInterval,hourInterval);
							comm.Mode_=CommItemMode.Email;
						}
						if(priority==CommType.Text) {
							error=SendText(pat,family,appt,dayInterval,hourInterval);
							comm.Mode_=CommItemMode.Text;
						}
						if(error=="") {
							Delete(listApptComms[i].ApptCommNum);
							comm.CommDateTime=DateTime.Now;
							comm.CommSource=CommItemSource.User;
							comm.Note=Lans.g("ApptComms","Appointment reminder sent.");
							comm.PatNum=pat.PatNum;
							comm.SentOrReceived=CommSentOrReceived.Sent;
							comm.UserNum=0;
							Commlogs.Insert(comm);
							break;//It was sent successfully, don't continue attempting to send.
						}
						else {
							errorText+=error;
						}
					}
				}
			}
			return errorText;
		}

		///<summary>Helper function for SendReminders.</summary>
		private static string SendEmail(Patient pat,Family fam,Appointment appt,int dayInterval,int hourInterval) {
			string patEmail=pat.Email;
			if(patEmail=="") {
				patEmail=fam.ListPats[0].Email;
			}
			if(patEmail=="") {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","has no email.");
			}
			string errorText="";
			EmailMessage emailMessage=new EmailMessage();
			emailMessage.PatNumSubj=pat.PatNum;
			emailMessage.ToAddress=patEmail;
			emailMessage.Subject="Appointment Reminder";
			//Determine if we should use the day or hour message.
			//Amount of time until the appointment.
			if((appt.AptDateTime-DateTime.Now).Days < dayInterval && (appt.AptDateTime=DateTime.Now).Hour <= hourInterval) { //Same day and correct hour
				//now - send date = amount of time past when it should be sent.  If it's the same day and it should be sent now or previously
				emailMessage.BodyText=PrefC.GetString(PrefName.ApptReminderHourMessage);
			}
			else {
				emailMessage.BodyText=PrefC.GetString(PrefName.ApptReminderDayMessage);
			}
			//"This is a reminder that you have an appointment on "+appt.AptDateTime.ToShortDateString()+" at "+appt.AptDateTime.ToShortTimeString();
			//Probably need more, this is my best guess right now.
			EmailAddress emailAddress;
			if(PrefC.HasClinicsEnabled) {
				emailAddress=EmailAddresses.GetByClinic(pat.ClinicNum);
			}
			else {
				emailAddress=EmailAddresses.GetByClinic(0);
			}
			try {
				EmailMessages.SendEmailUnsecure(emailMessage,emailAddress);
			}
			catch(Exception ex) {
				errorText+=ex.Message+"\r\n";
			}
			return errorText;
		}

		///<summary>Helper function for SendReminders.</summary>
		private static string SendText(Patient pat,Family fam,Appointment appt,int dayInterval,int hourInterval) {
			string errorText="";
			string patPhone=pat.WirelessPhone;
			if(pat.TxtMsgOk==YN.No || patPhone=="") {//If it's marked as No perhaps we should just stop here and not get the guarantor's info.
				patPhone=fam.ListPats[0].WirelessPhone;
				if(fam.ListPats[0].TxtMsgOk==YN.No) {
					patPhone="";
				}
			}
			if(patPhone=="") {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","cannot be sent texts.");
			}
			string textMsg;
			if((appt.AptDateTime-DateTime.Now).Days < dayInterval && (appt.AptDateTime=DateTime.Now).Hour <= hourInterval) { //Same day and correct hour
				textMsg=PrefC.GetString(PrefName.ApptReminderHourMessage);
			}
			else {
				textMsg=PrefC.GetString(PrefName.ApptReminderDayMessage);
			}
			try {
				SmsToMobiles.SendSmsSingle(pat.PatNum,patPhone,textMsg,Clinics.ClinicNum);
			}
			catch(Exception ex) {
				errorText+=ex.Message+"\r\n";
			}
			return errorText;
		}

	}
}