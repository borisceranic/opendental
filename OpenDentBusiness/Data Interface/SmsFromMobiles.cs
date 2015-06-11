using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsFromMobiles{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsFromMObiles.</summary>
		private static List<SmsFromMobile> listt;

		///<summary>A list of all SmsFromMObiles.</summary>
		public static List<SmsFromMobile> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM smsfrommobile ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsFromMobile";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsFromMobileCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsFromMobile> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsfrommobile WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Gets one SmsFromMobile from the db.</summary>
		public static SmsFromMobile GetOne(long smsFromMobileNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsFromMobile>(MethodBase.GetCurrentMethod(),smsFromMobileNum);
			}
			return Crud.SmsFromMobileCrud.SelectOne(smsFromMobileNum);
		}
		


		///<summary></summary>
		public static void Update(SmsFromMobile smsFromMobile){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobile);
				return;
			}
			Crud.SmsFromMobileCrud.Update(smsFromMobile);
		}

		///<summary></summary>
		public static void Delete(long smsFromMobileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobileNum);
				return;
			}
			string command= "DELETE FROM smsfrommobile WHERE SmsFromMobileNum = "+POut.Long(smsFromMobileNum);
			Db.NonQ(command);
		}
		*/

		///<summary>Returns the number of messages which have not yet been read.  If there are no unread messages, then empty string is returned.  If more than 99 messages are unread, then "99" is returned.  The count limit is 99, because only 2 digits can fit in the SMS notification text.</summary>
		public static string GetSmsNotification() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT COUNT(*) FROM smsfrommobile WHERE SmsStatus="+POut.Int((int)SmsFromStatus.ReceivedUnread);
			int smsUnreadCount=PIn.Int(Db.GetCount(command));
			if(smsUnreadCount==0) {
				return "";
			}
			if(smsUnreadCount>99) {
				return "99";
			}
			return smsUnreadCount.ToString();
		}

		///<summary>Call ProcessInboundSms instead.</summary>
		private static long Insert(SmsFromMobile smsFromMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsFromMobile.SmsFromMobileNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsFromMobile);
				return smsFromMobile.SmsFromMobileNum;
			}
			return Crud.SmsFromMobileCrud.Insert(smsFromMobile);
		}

		///<summary>Gets all SMS incoming messages for the specified filters.  If dateStart is 01/01/0001 then no start date will be used.  If dateEnd is 01/01/0001 then no end date will be used.  If listClinicNums is empty then will return messages for all clinics.  If arrayStatuses is empty then messages will all statuses will be returned.</summary>
		public static List<SmsFromMobile> GetMessages(DateTime dateStart,DateTime dateEnd,List <long> listClinicNums,params SmsFromStatus[] arrayStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,arrayStatuses);
			}
			List <string> listCommandFilters=new List<string>();
			if(dateStart>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+">="+POut.Date(dateStart));
			}
			if(dateEnd>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+"<="+POut.Date(dateEnd));
			}
			if(listClinicNums.Count>0) {
				string[] arrayClinicNumStrs=new string[listClinicNums.Count];
				for(int i=0;i<listClinicNums.Count;i++) {
					arrayClinicNumStrs[i]=POut.Long(listClinicNums[i]);
				}
				listCommandFilters.Add("ClinicNum IN ("+String.Join(",",arrayClinicNumStrs)+")");
			}
			if(arrayStatuses.Length>0) {
				string statuses="";
				for(int i=0;i<arrayStatuses.Length;i++) {
					if(i>0) {
						statuses+=",";
					}
					statuses+=(int)arrayStatuses[i];
				}
				listCommandFilters.Add("SmsStatus IN ("+statuses+")");
			}
			string command="SELECT * FROM smsfrommobile";
			if(listCommandFilters.Count>0) {
				command+=" WHERE "+String.Join(" AND ",listCommandFilters);
			}
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}
		
		///<summary>Attempts to find exact match for patient. If found, creates commlog, associates Patnum, and inserts into DB.
		///Otherwise, it simply inserts SmsFromMobiles into the DB. ClinicNum should have already been set before calling this function.</summary>
		public static void ProcessInboundSms(List<SmsFromMobile> listMessages) {
			if(listMessages==null || listMessages.Count==0) {
				return;
			}
			for(int i=0;i<listMessages.Count;i++) {
				SmsFromMobile sms=listMessages[i];
				sms.DateTimeReceived=DateTime.Now;
				List<long> listPatNums=FindPatNums(sms.MobilePhoneNumber);
				//We could not find definitive match, either 0 matches found, or more than one match found
				if(listPatNums.Count!=1) {
					Insert(sms);
					continue;
				}
				//We found exactly one match
				//associate patnum, create commlog, insert message
				sms.PatNum=listPatNums[0];
				Commlog comm=new Commlog() {
					 CommDateTime=sms.DateTimeReceived,
					 DateTimeEnd=sms.DateTimeReceived.AddSeconds(1),
					 Mode_= CommItemMode.Text,
					 Note=sms.MsgText,
					 PatNum=sms.PatNum,
					 //CommType=??,
					 SentOrReceived= CommSentOrReceived.Received
				};
				sms.CommlogNum=Commlogs.Insert(comm);
				Insert(sms);
			}
		}

		public static string GetSmsFromStatusDescript(SmsFromStatus smsFromStatus) {
			//No need to check RemotingRole; no call to db.
			if(smsFromStatus==SmsFromStatus.ReceivedUnread) {
				return "Unread";
			}
			else if(smsFromStatus==SmsFromStatus.ReceivedRead) {
				return "Read";
			}
			else if(smsFromStatus==SmsFromStatus.ReceivedJunk) {
				return "Junk";
			}
			return "";
		}

		///<summary>Updates only the changed fields of the SMS text message (if any).</summary>
		public static bool Update(SmsFromMobile smsFromMobile,SmsFromMobile oldSmsFromMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),smsFromMobile,oldSmsFromMobile);
			}
			return Crud.SmsFromMobileCrud.Update(smsFromMobile,oldSmsFromMobile);
		}

		///<summary>Used to link SmsFromMobiles to the patients that they came from.</summary>
		public static List<long> FindPatNums(string PhonePat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),PhonePat);
			}
			List<long> listPatNums=new List<long>();
			try {
				string command="SELECT PatNum FROM phonenumber WHERE PhoneNumberVal='"+POut.String(PhonePat)+"'";
				listPatNums = Db.GetListLong(command);
				command="SELECT PatNum FROM patient WHERE HmPhone='"+POut.String(PhonePat)+"' OR WkPhone='"+POut.String(PhonePat)+"' OR WirelessPhone='"+POut.String(PhonePat)+"'";
				listPatNums.AddRange(Db.GetListLong(command));
			}
			catch {	}
			return listPatNums.Distinct().ToList();
		}

	}
}