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

		///<summary>Creates two ApptComm items, one to send using dayInterval, and one to send using hourInterval.</summary>
		public static void InsertForAppt(Appointment appt) {
			if(appt.AptStatus!=ApptStatus.Scheduled && appt.AptStatus!=ApptStatus.ASAP) {
				return;//Do nothing unless it's scheduled or ASAP.
			}
			int dayInterval=PrefC.GetInt(PrefName.ApptReminderDayInterval);
			ApptComm apptComm;
			DateTime daySend=appt.AptDateTime.Subtract(new TimeSpan(dayInterval,0,0,0));
			//This prevents a UE while pre-inserting new appointments and prevents adding reminder if the interval can't be reached.
			if(dayInterval > 0 && appt.AptNum!=0 && daySend > DateTime.Now) {
				apptComm=new ApptComm();
				apptComm.ApptNum=appt.AptNum;
				apptComm.ApptCommType=IntervalType.Daily;
				apptComm.DateTimeSend=daySend;//Setting the ApptComm reminder to be sent dayInterval days before the appt.
				ApptComms.Insert(apptComm);
			}
			int hourInterval=PrefC.GetInt(PrefName.ApptReminderHourInterval);
			DateTime hourSend=appt.AptDateTime.Subtract(new TimeSpan(0,hourInterval,0,0));
			if(hourInterval > 0 && appt.AptNum!=0 && hourSend > DateTime.Now) {//This prevents a UE while pre-inserting new appointments.
				apptComm=new ApptComm();
				apptComm.ApptNum=appt.AptNum;
				apptComm.ApptCommType=IntervalType.Hourly;
				apptComm.DateTimeSend=hourSend;//Setting the ApptComm reminder to be sent hourInterval hours before the appt.
				ApptComms.Insert(apptComm);
			}
		}

		///<summary>Inserts appointment reminders for all future appointments.  
		///Used when automated appt reminder settings are changed so we can make sure all future appointments have appropriate reminders.</summary>
		public static void InsertForFutureAppts() {
			List<Appointment> listFutureAppts=Appointments.GetFutureSchedApts();
			foreach(Appointment appt in listFutureAppts) {
				DeleteForAppt(appt.AptNum);
				InsertForAppt(appt);
			}
		}

		///<summary>First deletes then re-inserts ApptComm items for an appointment that has been moved.</summary>
		public static void UpdateForAppt(Appointment appt) {
			DeleteForAppt(appt.AptNum);
			if(appt.AptDateTime > DateTime.Now) {//Prevents UE's when updating pre-inserted appointments with no scheduled time as well as appointments updated that were in the past.
				InsertForAppt(appt);
			}
		}

		///<summary>Send Appointment reminders for all ApptComm items.</summary>
		public static string SendReminders() {
			List<ApptComm> listApptComms=GetAll();
			string errorText="";
			foreach(ApptComm apptComm in listApptComms) {//Foreach loops are faster than For loops.
				if(apptComm.ApptCommType==IntervalType.Daily && (apptComm.DateTimeSend-DateTime.Now).TotalDays > 1) {//Skip if daily and not within one day of send date
					continue;//It's not currently enough days prior to the appointment to send a reminder.
				}
				if(apptComm.ApptCommType==IntervalType.Hourly && (apptComm.DateTimeSend-DateTime.Now).TotalHours > 1) {//Skip if hourly and not within one hour of send date
					continue;//It's not the correct number of hours prior to the appointment to send a reminder.
				}
				//Check for entries that should have already been sent.  Our send interval is set at 10 minutes, so 30 minutes leeway was deemed enough to 
				//catch edge cases (2 attempted sends on average).  If the DateTime the apptComm was supposed to be sent is older than 30 minutes ago, just delete it.
				if(apptComm.DateTimeSend < (DateTime.Now-new TimeSpan(0,30,0))) {
					Delete(apptComm.ApptCommNum);
					continue;
				}
				//It's within the correct day interval or within the correct hour interval, time to send a reminder.
				bool sendAll=PrefC.GetBool(PrefName.ApptReminderSendAll);
				string[] arraySendPriorities=PrefC.GetString(PrefName.ApptReminderSendOrder).Split(',');
				Appointment appt=Appointments.GetOneApt(apptComm.ApptNum);
				Family family=Patients.GetFamily(appt.PatNum);
				Patient pat=family.GetPatient(appt.PatNum);
				if(sendAll) {
					//Attempt to send both email and text reminder no matter what.
					//First we'll attempt to send via email.
					string emailError=SendEmail(pat,family,appt,apptComm.ApptCommType);
					if(emailError=="") {
						Commlog comm=new Commlog();
						comm.Mode_=CommItemMode.Email;
						comm.CommDateTime=DateTime.Now;
						comm.CommSource=CommItemSource.ApptReminder;
						comm.Note=Lans.g("ApptComms","Appointment reminder emailed for appointment on"+" "+appt.AptDateTime.ToShortDateString()+" at "+appt.AptDateTime.ToShortTimeString());
						comm.PatNum=pat.PatNum;
						comm.SentOrReceived=CommSentOrReceived.Sent;
						comm.UserNum=0;
						Commlogs.Insert(comm);
					}
					errorText+=emailError;
					//Second we'll attempt to send via text.
					string textError=SendText(pat,family,appt,apptComm.ApptCommType);		
					if(textError=="") {
						Commlog comm=new Commlog();
						comm.Mode_=CommItemMode.Text;
						comm.CommDateTime=DateTime.Now;
						comm.CommSource=CommItemSource.ApptReminder;
						comm.Note=Lans.g("ApptComms","Appointment reminder texted for appointment on"+" "+appt.AptDateTime.ToShortDateString()+" at "+appt.AptDateTime.ToShortTimeString());
						comm.PatNum=pat.PatNum;
						comm.SentOrReceived=CommSentOrReceived.Sent;
						comm.UserNum=0;
#warning delete this commlog insert, maybe, a commlog is also inserted from the SendText method.
						Commlogs.Insert(comm);
					}
					errorText+=textError;
					if(emailError=="" || textError=="") {//If either are successful, delete.
						Delete(apptComm.ApptCommNum);
					}
				}
				else {
					//Attempt to send reminders based on the priority of email/text/preferred.  Only attempt other methods if the previous priority failed. 
 					Commlog comm=new Commlog();
					for(int i=0;i<arraySendPriorities.Length;i++) {
						CommType priority=(CommType)PIn.Int(arraySendPriorities[i]);
						string error="";
						if(priority==CommType.Preferred) {
							if(pat.PreferContactMethod==ContactMethod.Email) {
								error=SendEmail(pat,family,appt,apptComm.ApptCommType);
								comm.Mode_=CommItemMode.Email;
							}
							else if(pat.PreferContactMethod==ContactMethod.TextMessage) {
								error=SendText(pat,family,appt,apptComm.ApptCommType);
								comm.Mode_=CommItemMode.Text;
							}
							else {
								//If they have a contact method other than email and textmessage we won't attempt sending on this step.
								//Simply continue on to the next priority.
								continue;
							}
						}
						if(priority==CommType.Email) {
							error=SendEmail(pat,family,appt,apptComm.ApptCommType);
							comm.Mode_=CommItemMode.Email;
						}
						if(priority==CommType.Text) {
							error=SendText(pat,family,appt,apptComm.ApptCommType);
							comm.Mode_=CommItemMode.Text;
						}
						if(error=="") {
							Delete(apptComm.ApptCommNum);
							comm.CommDateTime=DateTime.Now;
							comm.CommSource=CommItemSource.ApptReminder;
							comm.Note=Lans.g("ApptComms","Appointment reminder sent for appointment on"+appt.AptDateTime.ToShortDateString()+" at "+appt.AptDateTime.ToShortTimeString());
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

		///<summary>Generates text by replacing variable strings (such as [nameF]) with their corresponding parts.</summary>
		private static string GenerateText(string text,Patient pat,Appointment appt) {
			text=text.Replace("[title]",pat.Title);
			text=text.Replace("[nameF]",pat.GetNameFirst());//includes preferred.  Not sure I like this.
			text=text.Replace("[nameL]",pat.LName);
			text=text.Replace("[nameFLnoPref]",pat.GetNameFLnoPref());//Not sure how to handle the preferred.  Statements do it this way. ex. Statements line 320
			text=text.Replace("[nameFL]",pat.GetNameFL());
			text=text.Replace("[namePref]",pat.Preferred);
			text=text.Replace("[apptDate]",appt.AptDateTime.ToShortDateString());//Do we want to put logic in here to do "ask time arrive" instead of normal aptdatetime?
			text=text.Replace("[apptTime]",appt.AptDateTime.ToShortTimeString());
			if(text.Contains("[practiceName]")) {//Don't do extra work if we don't have to..
				text=text.Replace("[practiceName]",PrefC.GetString(PrefName.PracticeTitle));
			}
			if(text.Contains("[clinicName]")) {
				if(PrefC.HasClinicsEnabled) {
					text=text.Replace("[clinicName]",Clinics.GetDesc(pat.ClinicNum));
				}
				else {
					text=text.Replace("[clinicName]",PrefC.GetString(PrefName.PracticeTitle));//Clinics disabled but put clinicName.  Use practice info.
				}
			}
			if(text.Contains("[provName]")) {
				text=text.Replace("[provName]",Providers.GetFormalName(pat.PriProv));
			}
			return text;
		}

		///<summary>Helper function for SendReminders.  Sends an email with the requisite fields.  Skips sending if there is no practice/clinic email set up, if the patient/guarantor has a preferred contact method of None or DoNotCall, or neither the patient nor their guarantor has an email entered.</summary>
		private static string SendEmail(Patient pat,Family fam,Appointment appt,IntervalType intervalType) {
			EmailAddress emailAddress=EmailAddresses.GetByClinic(pat.ClinicNum);//Gets an address based on cascading priorities. Works for ClinicNum=0
			if(emailAddress.EmailUsername=="") {
				return Lans.g("ApptComms","No default email set up for practice/clinic")+".  ";
			}
			if(pat.PreferContactMethod==ContactMethod.DoNotCall) {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","has a preferred contact method of 'DoNotCall'")+".  ";
			}
			string patEmail=pat.Email;
			if(patEmail=="") {
				if(fam.ListPats[0].PreferContactMethod==ContactMethod.DoNotCall) {
					return pat.LName+", "+pat.FName+"'s "+Lans.g("ApptComms","guarantor has a preferred contact method of 'DoNotCall'")+".  ";
				}
				patEmail=fam.ListPats[0].Email;
			}
			if(patEmail=="") {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","has no email")+".  ";
			}
			EmailMessage emailMessage=new EmailMessage();
			emailMessage.PatNumSubj=pat.PatNum;
			emailMessage.ToAddress=patEmail;
			emailMessage.Subject="Appointment Reminder";
			emailMessage.CcAddress="";
			emailMessage.BccAddress="";
			//Determine if we should use the day or hour message.
			//Amount of time until the appointment.
			if(intervalType==IntervalType.Hourly) {
				emailMessage.BodyText=GenerateText(PrefC.GetString(PrefName.ApptReminderHourMessage),pat,appt);
			}
			else {
				emailMessage.BodyText=GenerateText(PrefC.GetString(PrefName.ApptReminderDayMessage),pat,appt);
			}
			emailMessage.FromAddress=emailAddress.SenderAddress;
			try {
				EmailMessages.SendEmailUnsecure(emailMessage,emailAddress);
			}
			catch(Exception ex) {
				return ex.Message+"  ";
			}
			return "";
		}

		///<summary>Helper function for SendReminders.  Sends a text message with the requisite fields.  Skips sending if text messaging isn't enabled, if the patient/guarantor has a preferred contact method of None or DoNotCall, or if neither the patient nor the guarantor has a valid wireless phone with text messaging enabled.</summary>
		private static string SendText(Patient pat,Family fam,Appointment appt,IntervalType intervalType) {
			if(!SmsPhones.IsIntegratedTextingEnabled()) {
				return Lans.g("ApptComms","Text messaging not enabled")+".  ";
			}
			if(pat.PreferContactMethod==ContactMethod.DoNotCall) {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","has a preferred contact method of 'DoNotCall'")+".  ";
			}
			string patPhone=pat.WirelessPhone;
			bool txtUnknownIsNo=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			//If texting is marked as no, the phone is blank, or unknown are treated as no, look for guarantor texting status.
			if(pat.TxtMsgOk==YN.No || patPhone=="" || (pat.TxtMsgOk==YN.Unknown && txtUnknownIsNo)) {
				if(fam.ListPats[0].PreferContactMethod==ContactMethod.DoNotCall) {
					return pat.LName+", "+pat.FName+"'s "+Lans.g("ApptComms","guarantor has a preferred contact method of 'DoNotCall'")+".  ";
				}
				patPhone=fam.ListPats[0].WirelessPhone;
				if(fam.ListPats[0].TxtMsgOk==YN.No || (fam.ListPats[0].TxtMsgOk==YN.Unknown && txtUnknownIsNo)) {
					patPhone="";
				}
			}
			if(patPhone=="") {
				return pat.LName+", "+pat.FName+" "+Lans.g("ApptComms","cannot be sent texts")+".  ";
			}
			string textMsg;
			if(intervalType==IntervalType.Hourly) {
				textMsg=GenerateText(PrefC.GetString(PrefName.ApptReminderHourMessage),pat,appt);
			}
			else {
				textMsg=GenerateText(PrefC.GetString(PrefName.ApptReminderDayMessage),pat,appt);
			}
			try {
				SmsToMobiles.SendSmsSingle(pat.PatNum,patPhone,textMsg,pat.ClinicNum);
			}
			catch(Exception ex) {
				return ex.Message+"  ";
			}
			return "";
		}

	}
}