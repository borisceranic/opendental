using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ApptComms {
		public const string ApptReminderMsgUS = @"Appointment Reminder: [nameF] is scheduled for [apptTime] on [apptDate] at [clinicName]. Call [clinicPhone] if issue. No Reply";

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

		///<summary>Retrieves all ApptComm entries that are scheduled to be sent between the specified times and the present.</summary>
		public static List<ApptComm> GetToSend(DateTime dateTimeStart,DateTime dateTimeEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptComm>>(MethodBase.GetCurrentMethod(),dateTimeStart,dateTimeEnd);
			}
			string command="SELECT * FROM apptcomm WHERE DateTimeSend BETWEEN "+POut.DateT(dateTimeStart)+" AND "+POut.DateT(dateTimeEnd);
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

		///<summary></summary>
		public static void DeleteForAppts(List<long> apptNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptNums);
				return;
			}
			if(apptNums==null || apptNums.Count<1) {
				return;
			}
			string command="DELETE FROM apptcomm WHERE ApptNum IN("+String.Join(",",apptNums)+")";
			Db.NonQ(command);
		}

		///<summary>Creates two ApptComm items, one to send using dayInterval, and one to send using hourInterval.</summary>
		public static void InsertForAppt(Appointment appt,double dayInterval,double hourInterval,DateTime automationBeginPref,DateTime automationEndPref) {
			if(appt.AptStatus!=ApptStatus.Scheduled && appt.AptStatus!=ApptStatus.ASAP) {
				return;//Do nothing unless it's scheduled or ASAP.
			}
			ApptComm apptComm;
			DateTime daySend=appt.AptDateTime.Subtract(TimeSpan.FromDays(dayInterval));
			//This prevents a UE while pre-inserting new appointments and prevents adding reminder if the interval can't be reached.
			if(dayInterval > 0 && appt.AptNum!=0 && daySend > DateTime.Now) {
				apptComm=new ApptComm();
				apptComm.ApptNum=appt.AptNum;
				apptComm.ApptCommType=IntervalType.Daily;
				DateTime automationBegin=new DateTime(daySend.Year,daySend.Month,daySend.Day
					,automationBeginPref.Hour,automationBeginPref.Minute,automationBeginPref.Second);
				if(daySend.TimeOfDay<automationBegin.TimeOfDay && automationBeginPref.TimeOfDay!=automationEndPref.TimeOfDay) {
					//The reminder is scheduled to be sent prior to automation beginning.  Let's schedule it to send the night before.
					DateTime automationEnd=new DateTime(daySend.Year,daySend.Month,daySend.Day
						,automationEndPref.Hour,automationEndPref.Minute,automationEndPref.Second);//Make sure to use the reminder's day/month/year, then subtract one day.
					DateTime dateSend=automationEnd.Subtract(TimeSpan.FromDays(1)).Subtract(TimeSpan.FromMinutes(30));//Schedule it for 30 minutes prior to automation end to make sure it attempts sending.
					apptComm.DateTimeSend=dateSend;
				}
				else { 
					apptComm.DateTimeSend=daySend;//Setting the ApptComm reminder to be sent dayInterval days before the appt.
				}
				ApptComms.Insert(apptComm);
			}
			DateTime hourSend=appt.AptDateTime.Subtract(TimeSpan.FromHours(hourInterval));
			if(hourInterval > 0 && appt.AptNum!=0 && hourSend > DateTime.Now) {//This prevents a UE while pre-inserting new appointments.
				apptComm=new ApptComm();
				apptComm.ApptNum=appt.AptNum;
				apptComm.ApptCommType=IntervalType.Hourly;
				DateTime automationBegin=new DateTime(hourSend.Year,hourSend.Month,hourSend.Day
					,automationBeginPref.Hour,automationBeginPref.Minute,automationBeginPref.Second);
				if(hourSend.TimeOfDay<automationBegin.TimeOfDay && automationBeginPref.TimeOfDay!=automationEndPref.TimeOfDay) {
					//The reminder is supposed to be sent prior to automation beginning.  Let's schedule it to send the night before.
					DateTime automationEnd=new DateTime(hourSend.Year,hourSend.Month,hourSend.Day
						,automationEndPref.Hour,automationEndPref.Minute,automationEndPref.Second);//Make sure to use the reminder's day/month/year, then subtract one day.
					DateTime sendDate=automationEnd.Subtract(TimeSpan.FromDays(1)).Subtract(TimeSpan.FromMinutes(30));//Schedule it for 30 minutes prior to automation end to make sure it attempts sending.
					apptComm.DateTimeSend=sendDate;
				}
				else { 
					apptComm.DateTimeSend=hourSend;//Setting the ApptComm reminder to be sent HourInterval hours before the appt.
				}
				ApptComms.Insert(apptComm);
			}
		}

		///<summary>Inserts appointment reminders for all future appointments.  
		///Used when automated appt reminder settings are changed so we can make sure all future appointments have appropriate reminders.</summary>
		public static void InsertForFutureAppts() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//Doing the remotingrole check here to save a lot of traffic over the wire in the Delete and Insert statements below.
			//DO NOT USE METHODS THAT USE THE CACHE AFTER THIS POINT
			List<Appointment> listFutureAppts=Appointments.GetFutureSchedApts();
			double dayInterval=PrefC.GetDouble(PrefName.ApptReminderDayInterval);
			double hourInterval=PrefC.GetDouble(PrefName.ApptReminderHourInterval);
			DateTime automationBeginPref=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeStart);
			DateTime automationEndPref=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeEnd);
			foreach(Appointment appt in listFutureAppts) {
				DeleteForAppt(appt.AptNum);
				//Get the DateTime automation end
				InsertForAppt(appt,dayInterval,hourInterval,automationBeginPref,automationEndPref);
			}
		}

		///<summary>First deletes then re-inserts ApptComm items for an appointment that has been moved.</summary>
		public static void UpdateForAppt(Appointment appt) {
			DeleteForAppt(appt.AptNum);
			if(appt.AptDateTime > DateTime.Now) {//Prevents UE's when updating pre-inserted appointments with no scheduled time as well as appointments updated that were in the past.
				double dayInterval=PrefC.GetDouble(PrefName.ApptReminderDayInterval);
				double hourInterval=PrefC.GetDouble(PrefName.ApptReminderHourInterval);
				DateTime automationBeginPref=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeStart);
				DateTime automationEndPref=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeEnd);
				InsertForAppt(appt,dayInterval,hourInterval,automationBeginPref,automationEndPref);
			}
		}

		///<summary>Send Appointment reminders for all ApptComm items.</summary>
		public static string SendReminders() {
			string errorText="";
			DateTime automationStart=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeStart);
			DateTime automationEnd=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeEnd);
			if(automationStart.TimeOfDay!=automationEnd.TimeOfDay && (DateTime.Now.TimeOfDay<automationStart.TimeOfDay || DateTime.Now.TimeOfDay>automationEnd.TimeOfDay)) {
				//Not currently within send window and automation start/end is enabled.
				return "";
			}
			DateTime dateTimeAutomationStart=new DateTime();
			DateTime dateTimeAutomationEnd=new DateTime();
			if(automationStart.TimeOfDay!=automationEnd.TimeOfDay) {
				//Get all entries for today, from automation start to automation end.
				dateTimeAutomationStart=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day
					,automationStart.Hour,automationStart.Minute,automationStart.Second);
				dateTimeAutomationEnd=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day
					,automationEnd.Hour,automationEnd.Minute,automationEnd.Second);
			}
			else {
				//Get all entries for today, from midnight to midnight.
				dateTimeAutomationStart=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
				dateTimeAutomationEnd=dateTimeAutomationStart.AddDays(1);
			}
			List<ApptComm> listApptComms=GetToSend(dateTimeAutomationStart,dateTimeAutomationEnd);
			foreach(ApptComm apptComm in listApptComms) {//Foreach loops are faster than For loops.  All reminders in the list are for appointments that haven't come yet.
				if(apptComm.ApptCommType==IntervalType.Daily && (apptComm.DateTimeSend-DateTime.Now).TotalDays > 0) {//Send if Now is <= 0 days prior to the send day.
					continue;//It's not currently enough days prior to the appointment to send a reminder.
				}
				if(apptComm.ApptCommType==IntervalType.Hourly && (apptComm.DateTimeSend-DateTime.Now).TotalHours > 0) {//Send if Now <= 0 hours prior to the send hour.
					continue;//It's not the correct number of hours prior to the appointment to send a reminder.
				}
				//Check for entries that should have already been sent.  Our send interval is set at 10 minutes, so 30 minutes leeway was deemed enough to 
				//catch edge cases (2 attempted sends on average).  If the DateTime the apptComm was supposed to be sent is older than 30 minutes ago, just delete it.
				if(apptComm.DateTimeSend < (DateTime.Now-new TimeSpan(0,30,0))) {//Only entries that were skipped due to the listener being down should be deleted here.
					Delete(apptComm.ApptCommNum);
					continue;
				}
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
						comm.Note=Lans.g("ApptComms","Appointment reminder emailed for appointment on")+" "+appt.AptDateTime.ToShortDateString()+" "
							+Lans.g("ApptComms","at")+" "+appt.AptDateTime.ToShortTimeString();
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
						comm.Note=Lans.g("ApptComms","Appointment reminder texted for appointment on")+" "+appt.AptDateTime.ToShortDateString()+" "
							+Lans.g("ApptComms","at")+" "+appt.AptDateTime.ToShortTimeString();
						comm.PatNum=pat.PatNum;
						comm.SentOrReceived=CommSentOrReceived.Sent;
						comm.UserNum=0;
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
							comm.Note=Lans.g("ApptComms","Appointment reminder sent for appointment on")+appt.AptDateTime.ToShortDateString()+" "+Lans.g("ApptComms","at")+" "+appt.AptDateTime.ToShortTimeString();
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
		private static string FillMessage(string text,Patient pat,Appointment appt) {
			Clinic clinic= Clinics.GetClinic(appt.ClinicNum);
			text=text.Replace("[nameF]",pat.GetNameFirst());//includes preferred.  Not sure I like this.
			text=text.Replace("[namePref]",pat.Preferred);
			text=text.Replace("[apptDate]",appt.AptDateTime.ToString("MMM d"));//Do we want to put logic in here to do "ask time arrive" instead of normal aptdatetime?
			text=text.Replace("[apptTime]",appt.AptDateTime.ToString("hh:mmtt"));
			if(text.Contains("[practiceName]")) {//Don't do extra work if we don't have to..
				text=text.Replace("[practiceName]",PrefC.GetString(PrefName.PracticeTitle));
			}
			if(text.Contains("[clinicName]")) {
				if(clinic!=null) {
					text=text.Replace("[clinicName]",clinic.Description);
				}
				else {
					text=text.Replace("[clinicName]",PrefC.GetString(PrefName.PracticeTitle));//Clinics disabled but put clinicName.  Use practice info.
				}
			}
			if(text.Contains("[clinicPhone]")) {
				string phone = "";
				if(clinic!=null) {
					phone=clinic.Phone;
				}
				else {
					phone=PrefC.GetString(PrefName.PracticePhone);
				}
				if(PrefC.GetLanguageAndRegion().Name.Right(2)=="US" && clinic.Phone.Length==10) {
					//Phone format "### ### ####" per Nathan's request
					phone = string.Format("{0} {1} {2}",phone.Substring(0,3),phone.Substring(3,3),phone.Substring(6));
				}
				text=text.Replace("[clinicPhone]",phone);
			}
			if(text.Contains("[provName]")) {
				text=text.Replace("[provName]",Providers.GetFormalName(appt.ProvNum));
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
			emailMessage.BodyText=FillMessage(PrefC.GetString(PrefName.ApptReminderEmailMessage),pat,appt);
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
			string message;
			if(SmsPhones.IsIntegratedTextingEnabled() && SmsPhones.IsTextingForCountry("US")) {
				message=FillMessage(ApptComms.ApptReminderMsgUS,pat,appt);
			}
			else {
				message=FillMessage(PrefC.GetString(PrefName.ApptReminderDayMessage),pat,appt);
			}
			try {
				SmsToMobiles.SendSmsSingle(pat.PatNum,patPhone,message,pat.ClinicNum,SmsMessageSource.Reminder);
			}
			catch(Exception ex) {
				return ex.Message+"  ";
			}
			return "";
		}

	}
}