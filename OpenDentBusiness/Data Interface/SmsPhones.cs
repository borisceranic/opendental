using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsPhones{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsPhones.</summary>
		private static List<SmsPhone> listt;

		///<summary>A list of all SmsPhones.</summary>
		public static List<SmsPhone> Listt{
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
			string command="SELECT * FROM smsphone ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsPhone";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsPhoneCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsPhone> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsPhone>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsvln WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsVlnCrud.SelectMany(command);
		}

		///<summary>Gets one SmsPhone from the db.</summary>
		public static SmsPhone GetOne(long smsVlnNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsPhone>(MethodBase.GetCurrentMethod(),smsVlnNum);
			}
			return Crud.SmsVlnCrud.SelectOne(smsVlnNum);
		}

		///<summary></summary>
		public static void Delete(long smsVlnNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsVlnNum);
				return;
			}
			string command= "DELETE FROM smsvln WHERE SmsVlnNum = "+POut.Long(smsVlnNum);
			Db.NonQ(command);
		}
		*/

		///<summary>Gets one SmsPhone from the db. Returns null if not found.</summary>
		public static SmsPhone GetByPhone(string phoneNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SmsPhone>(MethodBase.GetCurrentMethod(),phoneNumber);
			}
			string command="SELECT * FROM smsphone WHERE PhoneNumber='"+POut.String(phoneNumber)+"'";
			return Crud.SmsPhoneCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(SmsPhone smsPhone) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				smsPhone.SmsPhoneNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsPhone);
				return smsPhone.SmsPhoneNum;
			}
			return Crud.SmsPhoneCrud.Insert(smsPhone);
		}

		///<summary></summary>
		public static void Update(SmsPhone smsPhone) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsPhone);
				return;
			}
			Crud.SmsPhoneCrud.Update(smsPhone);
		}

		///<summary>This will only be called by HQ via the listener in the event that this number has been cancelled.</summary>
		public static void UpdateToInactive(string phoneNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneNumber);
				return;
			}
			SmsPhone smsPhone=GetByPhone(phoneNumber);
			if(smsPhone==null) {
				return;
			}
			smsPhone.DateTimeInactive=DateTime.Now;
			Crud.SmsPhoneCrud.Update(smsPhone);
		}

		///<summary>Gets sms phones when not using clinics.</summary>
		public static List<SmsPhone> GetForPractice() {
			//No remoting role check, No call to database.
			//Get for practice is just getting for clinic num 0
			return GetForClinics(new List<long>() { 0 });//clinic num 0
		}

		public static List<SmsPhone> GetForClinics(List<long> listClinicNums) {
			if(listClinicNums.Count==0) {
				return new List<SmsPhone>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsPhone>>(MethodBase.GetCurrentMethod(),listClinicNums);
			}
			string command= "SELECT * FROM smsphone WHERE ClinicNum IN ("+String.Join(",",listClinicNums)+")";
			return Crud.SmsPhoneCrud.SelectMany(command);
		}

		public static DataTable GetSmsUsageLocal(List<long> listClinicNums, DateTime dateMonth) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,dateMonth);
			}
			#region Initialize retVal DataTable
			List<SmsPhone> listSmsPhones=GetForClinics(listClinicNums);
			DateTime dateStart=dateMonth.Date.AddDays(1-dateMonth.Day);//remove time portion and day of month portion. Remainder should be midnight of the first of the month
			DateTime dateEnd=dateStart.AddMonths(1);//This should be midnight of the first of the following month.
			//This query builds the data table that will be filled from several other queries, instead of writing one large complex query.
			//It is written this way so that the queries are simple to write and understand, and makes Oracle compatibility easier to maintain.
			string command=@"SELECT 
							  0 ClinicNum,
							  ' ' PhoneNumber,
							  ' ' CountryCode,
							  0 SentMonth,
							  0.0 SentCharge,
							  0 ReceivedMonth,
							  0.0 ReceivedCharge 
							FROM
							  DUAL";//this is a simple way to get a data table with the correct layout without having to query any real data.
			DataTable retVal=Db.GetTable(command).Clone();//use .Clone() to get schema only, with no rows.
			retVal.TableName="SmsUsageLocal";
			for(int i=0;i<listClinicNums.Count;i++) {
				DataRow row=retVal.NewRow();
				row["ClinicNum"]=listClinicNums[i];
				row["PhoneNumber"]="No Active Phones";
				SmsPhone firstActivePhone=listSmsPhones
					.Where(x => x.ClinicNum==listClinicNums[i])//phones for this clinic
					.Where(x => x.DateTimeInactive.Year<1880)//that are active
					.FirstOrDefault(x => x.DateTimeActive==listSmsPhones//and have the smallest active date (the oldest/first phones activated)
						.Where(y => y.ClinicNum==x.ClinicNum)
						.Where(y => y.DateTimeInactive.Year<1880)
						.Min(y => y.DateTimeActive));
				if(firstActivePhone!=null) {
					row["PhoneNumber"]=firstActivePhone.PhoneNumber;
					row["CountryCode"]=firstActivePhone.CountryCode;
				}
				row["SentMonth"]=0;
				row["SentCharge"]=0.0;
				row["ReceivedMonth"]=0;
				row["ReceivedCharge"]=0.0;
				retVal.Rows.Add(row);
			}
			#endregion
			#region Fill retVal DataTable
			//Sent Last Month
			command="SELECT ClinicNum, COUNT(*), ROUND(SUM(MsgChargeUSD),2) FROM smstomobile "
				+"WHERE DateTimeSent >="+POut.Date(dateStart)+" "
				+"AND DateTimeSent<"+POut.Date(dateEnd)+" "
				+"AND MsgChargeUSD>0 GROUP BY ClinicNum";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				for(int j=0;j<retVal.Rows.Count;j++) {
					if(retVal.Rows[j]["ClinicNum"].ToString()!=table.Rows[i]["ClinicNum"].ToString()) {
						continue;
					}
					retVal.Rows[j]["SentMonth"]=table.Rows[i][1];//.ToString();
					retVal.Rows[j]["SentCharge"]=table.Rows[i][2];//.ToString();
					break;
				}
			}
			//Received Month
			command="SELECT ClinicNum, COUNT(*) FROM smsfrommobile "
				+"WHERE DateTimeReceived >="+POut.Date(dateStart)+" "
				+"AND DateTimeReceived<"+POut.Date(dateEnd)+" "
				+"GROUP BY ClinicNum";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				for(int j=0;j<retVal.Rows.Count;j++) {
					if(retVal.Rows[j]["ClinicNum"].ToString()!=table.Rows[i]["ClinicNum"].ToString()) {
						continue;
					}
					retVal.Rows[j]["ReceivedMonth"]=table.Rows[i][1].ToString();
					retVal.Rows[j]["ReceivedCharge"]="0";
					break;
				}
			}
			#endregion
			return retVal;
		}

		///<summary>Surround with Try/Catch</summary>
		public static List<SmsPhone> SignContract(long clinicNum,double monthlyLimitUSD) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsPhone>>(MethodBase.GetCurrentMethod(),clinicNum,monthlyLimitUSD);
			}
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))){
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ClinicNum");
				writer.WriteString(clinicNum.ToString());
				writer.WriteEndElement(); //ClinicNum	
				writer.WriteStartElement("SmsMonthlyLimit");
				writer.WriteString(monthlyLimitUSD.ToString());
				writer.WriteEndElement(); //SmsMonthlyLimit	
				writer.WriteStartElement("CountryCode");
				writer.WriteString(CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2));//Example "en-US"="US"
				writer.WriteEndElement(); //SmsMonthlyLimit	
				writer.WriteEndElement(); //Payload	
			}
			WebServiceMainHQ.WebServiceMainHQ service=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			string result = "";
			try {
				result=service.SmsSignAgreement(WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.IntegratedTexting));
			}
			catch(Exception ex) {
				throw new Exception("Unable to sign agreement using web service.");
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//ListSmsPhone");
			if(node==null) {
				//should never happen
				throw new Exception("An error has occured while attempting to acknowledge agreement.");
			}
			List<SmsPhone> listPhones=null;
			using(XmlReader reader=XmlReader.Create(new System.IO.StringReader(node.InnerXml))) {
				System.Xml.Serialization.XmlSerializer xmlListSmsPhoneSerializer=new System.Xml.Serialization.XmlSerializer(typeof(List<SmsPhone>));
				listPhones=(List<SmsPhone>)xmlListSmsPhoneSerializer.Deserialize(reader);
			}
			if(listPhones==null || listPhones.Count==0) {
				//should never happen
				throw new Exception("An error has occured while attempting to sign contract.");
			}
			//Will always deletes old rows and inserts new rows because SmsPhoneNum is always 0 in new list.
			SmsPhones.UpdateOrInsertFromList(listPhones,clinicNum);
			return listPhones;
		}

		public static bool UnSignContract(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),clinicNum);
			}
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))){
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ClinicNum");
				writer.WriteString(clinicNum.ToString());
				writer.WriteEndElement(); //ClinicNum	
				writer.WriteEndElement(); //Payload	
			}
			WebServiceMainHQ.WebServiceMainHQ service=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			string result = "";
			try {
				result=service.SmsCancelService(WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.IntegratedTexting));
			}
			catch(Exception ex) {
				//nothing to do here. Throw up to UI layer.
				throw ex;
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
			return false;
		}

		///<summary>Find all phones in the db (by PhoneNumber) and sync with listPhonesSync. If a given PhoneNumber does not already exist then insert the SmsPhone.
		///Sets all ClinicNum(s) of phones in listPhonesSync to the given clinicNum.</summary>
		public static void UpdateOrInsertFromList(List<SmsPhone> listPhonesSync,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPhonesSync,clinicNum);
				return;
			}
			//Get all phones so we can filter as needed below.
			string command="SELECT * FROM smsphone";
			List<SmsPhone> listPhonesDb=Crud.SmsPhoneCrud.SelectMany(command);
			for(int i=0;i<listPhonesSync.Count;i++) {
				SmsPhone phoneOld=listPhonesDb.FirstOrDefault(x => x.PhoneNumber==listPhonesSync[i].PhoneNumber);
				//Upsert.
				if(phoneOld!=null) { //This phone already exists. Update it to look like the phone we are trying to insert.
					phoneOld.ClinicNum=clinicNum; //The clinic may have changed so set it to the new clinic.
					phoneOld.CountryCode=listPhonesSync[i].CountryCode;
					phoneOld.DateTimeActive=listPhonesSync[i].DateTimeActive;
					phoneOld.DateTimeInactive=listPhonesSync[i].DateTimeInactive;
					phoneOld.InactiveCode=listPhonesSync[i].InactiveCode;
					Update(phoneOld);
				}
				else { //This phone is new so insert it.
					Insert(listPhonesSync[i]);
				}			
			}
		}

		///<summary>Returns current clinic limit minus message usage for current calendar month.</summary>
		public static double GetClinicBalance(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),clinicNum);
			}
			double limit=0;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(PrefC.GetDate(PrefName.SmsContractDate).Year>1880) {
					limit=PrefC.GetDouble(PrefName.SmsMonthlyLimit);
				}
			}
			else { 
				if(clinicNum==0 && Clinics.List.Length>0) {//Sending text for "Unassigned" patient.  Use the first clinic in the list's information (for now).
					clinicNum=Clinics.List[0].ClinicNum;
				}
				Clinic clinicCur=Clinics.GetClinic(clinicNum);
				if(clinicCur!=null && clinicCur.SmsContractDate.Year>1880) {
					limit=clinicCur.SmsMonthlyLimit;
				}
			}
			DateTime dtStart=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
			DateTime dtEnd=dtStart.AddMonths(1);
			string command="SELECT SUM(MsgChargeUSD) FROM smstomobile WHERE ClinicNum="+POut.Long(clinicNum)+" "
				+"AND DateTimeSent>="+POut.Date(dtStart)+" AND DateTimeSent<"+POut.Date(dtEnd);
			limit-=PIn.Double(Db.GetScalar(command));
			return limit;
		}

		///<summary>Returns true if texting is enabled for any of the clinics, or if not using clinics, if it is enabled for the practice.</summary>
		public static bool IsIntegratedTextingEnabled() {
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				return PrefC.GetDateT(PrefName.SmsContractDate).Year>1880;
			}
			for(int i=0;i<Clinics.List.Length;i++) {
				if(Clinics.List[i].SmsContractDate.Year>1880) {
					return true;
				}
			}
			return false;
		}

		///<summary>Returns 0 if clinics not in use, or patient.ClinicNum if assigned to a clinic, or ClinicNum of first clinic.</summary>
		public static long GetClinicNumForTexting(long patNum) {
			if(PrefC.GetBool(PrefName.EasyNoClinics) || Clinics.List.Length==0) {
				return 0;//0 used for no clinics
			}
			Clinic clinic=Clinics.GetClinic(Patients.GetPat(patNum).ClinicNum);//if patnum invalid will throw unhandled exception.
			if(clinic!=null) {//if pat assigned to invalid clinic or clinic num 0
				return clinic.ClinicNum;
			}
			return Clinics.List[0].ClinicNum;
		}


	}
}