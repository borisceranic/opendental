using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Linq;
using System.Globalization;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsToMobiles{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsToMobile.</summary>
		private static List<SmsToMobile> listt;

		///<summary>A list of all SmsToMobile.</summary>
		public static List<SmsToMobile> Listt{
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
			string command="SELECT * FROM smstomobile ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsToMobile";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsToMobileCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsToMobile> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsToMobile>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smstomobile WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsToMobileCrud.SelectMany(command);
		}

		///<summary>Gets one SmsToMobile from the db.</summary>
		public static SmsToMobile GetOne(long smsToMobileNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsToMobile>(MethodBase.GetCurrentMethod(),smsToMobileNum);
			}
			return Crud.SmsToMobileCrud.SelectOne(smsToMobileNum);
		}

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile);
		}

		///<summary></summary>
		public static void Delete(long smsToMobileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobileNum);
				return;
			}
			string command= "DELETE FROM smstomobile WHERE smsToMobileNum = "+POut.Long(smsToMobileNum);
			Db.NonQ(command);
		}
		*/

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile);
		}

		///<summary>Gets one SmsToMobile from the db.</summary>
		public static SmsToMobile GetMessageByGuid(string guid) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SmsToMobile>(MethodBase.GetCurrentMethod(),guid);
			}
			string command="SELECT * FROM smstomobile WHERE GuidMessage='"+guid+"'";
			return Crud.SmsToMobileCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(SmsToMobile smsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				smsToMobile.SmsToMobileNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsToMobile);
				return smsToMobile.SmsToMobileNum;
			}
			return Crud.SmsToMobileCrud.Insert(smsToMobile);
		}

		///<summary>Gets all SMS incoming messages for the specified filters.  If dateStart is 01/01/0001 then no start date will be used.  If dateEnd is 01/01/0001 then no end date will be used.  If listClinicNums is empty then will return messages for all clinics.  If patNum is non-zero, then only the messages for the specified patient will be returned, otherwise messages for all patients will be returned.</summary>
		public static List<SmsToMobile> GetMessages(DateTime dateStart,DateTime dateEnd,List<long> listClinicNums,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsToMobile>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,patNum);
			}
			List<string> listCommandFilters=new List<string>();
			if(dateStart>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeSent")+">="+POut.Date(dateStart));
			}
			if(dateEnd>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeSent")+"<="+POut.Date(dateEnd));
			}
			if(listClinicNums.Count>0) {
				string[] arrayClinicNumStrs=new string[listClinicNums.Count];
				for(int i=0;i<listClinicNums.Count;i++) {
					arrayClinicNumStrs[i]=POut.Long(listClinicNums[i]);
				}
				listCommandFilters.Add("ClinicNum IN ("+String.Join(",",arrayClinicNumStrs)+")");
			}
			if(patNum!=0) {
				listCommandFilters.Add("PatNum="+POut.Long(patNum));
			}
			string command="SELECT * FROM smstomobile";
			if(listCommandFilters.Count>0) {
				command+=" WHERE "+String.Join(" AND ",listCommandFilters);
			}
			return Crud.SmsToMobileCrud.SelectMany(command);
		}

		///<summary>Convert a phone number to internation format and remove all punctuation. Validates input number format. Throws exceptions.</summary>
		public static string ConvertPhoneToInternational(string phoneRaw,string countryCode) {
			if(string.IsNullOrWhiteSpace(phoneRaw)) {
				throw new Exception("Input phone number must be set");
			}
			//Remove non-numeric.
			string ret=new string(phoneRaw.Where(x => char.IsDigit(x)).ToArray());			
			switch(countryCode.ToUpper()) {
				case "US":
				case "CA":
					if(!ret.StartsWith("1")) { //Add a "1" if US.
						ret="1"+ret;
					}
					if(ret.Length!=11) {
						throw new Exception("Input phone number cannot be properly formatted for country code: "+countryCode);
					}
					break;
			}			
			return ret;
		}

		///<summary>Surround with Try/Catch.  Sent as time sensitive message.</summary>
		public static bool SendSmsSingle(long patNum,string wirelessPhone,string message,long clinicNum,SmsMessageSource smsMessageSource,bool makeCommLog=true) {
			double balance=SmsPhones.GetClinicBalance(clinicNum);
			if(balance-0.04<0) {
				throw new Exception("To send this message first increase spending limit for integrated texting from eServices Setup.");
			}				
			SmsToMobile smsToMobile=new SmsToMobile();
			smsToMobile.ClinicNum=clinicNum;
			smsToMobile.GuidMessage=Guid.NewGuid().ToString();
			smsToMobile.GuidBatch=smsToMobile.GuidMessage;
			smsToMobile.IsTimeSensitive=true;
			smsToMobile.MobilePhoneNumber=ConvertPhoneToInternational(wirelessPhone,CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2));//Example "en-US"="US"
			smsToMobile.PatNum=patNum;
			smsToMobile.MsgText=message;
			smsToMobile.MsgType=smsMessageSource;
			SmsToMobiles.SendSms(new List<SmsToMobile>() { smsToMobile });//Will throw if failed.
			smsToMobile.SmsStatus=SmsDeliveryStatus.Pending;
			smsToMobile.DateTimeSent=DateTime.Now;
			SmsToMobiles.Insert(smsToMobile);
			if(patNum !=0 && makeCommLog) {  //Patient specified and calling code won't make commlog, make it here.
				Commlogs.Insert(new Commlog() {
					CommDateTime=smsToMobile.DateTimeSent,
					Mode_=CommItemMode.Text,
					Note="Text message sent: "+smsToMobile.MsgText,
					PatNum=smsToMobile.PatNum,
					CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC),
					SentOrReceived=CommSentOrReceived.Sent
				});
			}
			return true;
		}

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile,SmsToMobile oldSmsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile,oldSmsToMobile);
		}

		///<summary>Surround with try/catch. Returns true if all messages succeded, throws exception if it failed. 
		///All Integrated Texting should use this method, CallFire texting does not use this method.</summary>
		public static bool SendSms(List<SmsToMobile> listMessages) {
			if(listMessages==null || listMessages.Count==0) {
				throw new Exception("No messages to send.");
			}
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))){
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ListSmsToMobile");
				System.Xml.Serialization.XmlSerializer xmlListSmsToMobileSerializer=new System.Xml.Serialization.XmlSerializer(typeof(List<SmsToMobile>));
				xmlListSmsToMobileSerializer.Serialize(writer,listMessages);
				writer.WriteEndElement(); //ListSmsToMobile	
				writer.WriteEndElement(); //Payload	
			}
			WebServiceMainHQ.WebServiceMainHQ service=WebServiceMainHQProxy.GetWebServiceMainHQInstance();			
			string result = "";
			try {
				result=service.SmsSend(WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.IntegratedTexting));
			}
			catch(Exception ex) {
				throw new Exception("Unable to send using web service.");
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//Success");
			if(node!=null) {
				return true;
			}
			//Should never happen, we didn't get an explicit fail or success
			throw new Exception("Unknown error has occured.");
		}

	}
}