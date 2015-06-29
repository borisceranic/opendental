using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

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
		public static void Update(SmsPhone smsVln){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsVln);
				return;
			}
			Crud.SmsVlnCrud.Update(smsVln);
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

		///<summary></summary>
		public static long Insert(SmsPhone smsPhone) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				smsPhone.SmsPhoneNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsPhone);
				return smsPhone.SmsPhoneNum;
			}
			return Crud.SmsPhoneCrud.Insert(smsPhone);
		}

		///<summary>Gets sms phones when not using clinics.</summary>
		public static List<SmsPhone> GetForPractice() {
			//No remoting role check, No call to database.
			//Get for practice is just getting for clinic num 0
			return GetForClinics(new List<Clinic>() { new Clinic() });//clinic num 0
		}

		public static List<SmsPhone> GetForClinics(List<Clinic> listClinics) {
			if(listClinics.Count==0){
				return new List<SmsPhone>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsPhone>>(MethodBase.GetCurrentMethod(),listClinics);
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.Count;i++) {
				listClinicNums.Add(listClinics[i].ClinicNum);
			}
			string command= "SELECT * FROM smsphone WHERE ClinicNum IN ("+String.Join(",",listClinicNums)+")";
			return Crud.SmsPhoneCrud.SelectMany(command);
		}

		///<summary>Returns data table containing usage for supplied phones for the given month.  
		///Pass in all phones for a given clinic, or a single phone for individual usage.  Returns null if no valid phones supplied.</summary>
		public static DataTable GetSmsUsageLocal(List<SmsPhone> phones, DateTime DateMonth) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DataTable>(MethodBase.GetCurrentMethod(),phones,DateMonth);
			}
			#region Initialize retVal DataTable
			//Dictionary contains Clinic Num and the first SmsPhone (the main phone) for each account. Should have been passed in.
			Dictionary<long,SmsPhone> ClinicNumInitialPhoneDate=new Dictionary<long,SmsPhone>();
			for(int i=0;i<phones.Count;i++) {
				if(!ClinicNumInitialPhoneDate.ContainsKey(phones[i].ClinicNum)){
					ClinicNumInitialPhoneDate.Add(phones[i].ClinicNum,phones[i]);
				}
				else if(ClinicNumInitialPhoneDate[phones[i].ClinicNum].DateTimeActive>phones[i].DateTimeActive){
					ClinicNumInitialPhoneDate[phones[i].ClinicNum]=phones[i];
				}
			}
			List<long> listPhoneNums=new List<long>();
			foreach(SmsPhone phone in ClinicNumInitialPhoneDate.Values) {
				listPhoneNums.Add(phone.SmsPhoneNum);
			}
			if(listPhoneNums.Count==0) {
				return null;//no valid phone nums supplied.
			}
			DateTime dateStart=DateMonth.Date.AddDays(1-DateMonth.Day);//remove time portion and day of month portion. Remainder should be midnight of the first of the month
			DateTime dateEnd=dateStart.AddMonths(1);//This should be midnight of the first of the following month.
			//This query builds the data table that will be filled from several other queries, instead of writing one large complex query.
			//It is written this way so that the queries are simple to write and understand, and makes Oracle compatibility easier to maintain.
			string command=@"SELECT 
							  ClinicNum,
							  PhoneNumber,
							  CountryCode,
							  0 SentMonth,
							  0.0 SentCharge,
							  0 ReceivedMonth,
							  0.0 ReceivedCharge 
							FROM
							  SmsPhone 
							WHERE SmsPhoneNum IN ("+String.Join(",",listPhoneNums)+")";
			DataTable retVal=Db.GetTable(command);
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
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)){
				writer.WriteStartElement("Request");
				writer.WriteStartElement("Credentials");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticeTitle");
				writer.WriteString(PrefC.GetString(PrefName.PracticeTitle));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticePhone");
				writer.WriteString(PrefC.GetString(PrefName.PracticePhone));
				writer.WriteEndElement();
				writer.WriteStartElement("ProgramVersion");
				writer.WriteString(PrefC.GetString(PrefName.ProgramVersion));
				writer.WriteEndElement();
				writer.WriteStartElement("ServiceCode");
				writer.WriteString(eServiceCode.IntegratedTexting.ToString());
				writer.WriteEndElement();
				writer.WriteEndElement();//End Credentials
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
				writer.WriteEndElement(); //Request
			}
			WebServiceMainHQ.WebServiceMainHQ service=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			string result = "";
			try {
				result=service.SmsSignAgreement(strbuild.ToString());
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
			SmsPhones.InsertNewFromList(listPhones,clinicNum);
			return listPhones;
		}

		public static bool UnSignContract(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),clinicNum);
			}
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("Request");
				writer.WriteStartElement("Credentials");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticeTitle");
				writer.WriteString(PrefC.GetString(PrefName.PracticeTitle));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticePhone");
				writer.WriteString(PrefC.GetString(PrefName.PracticePhone));
				writer.WriteEndElement();
				writer.WriteStartElement("ProgramVersion");
				writer.WriteString(PrefC.GetString(PrefName.ProgramVersion));
				writer.WriteEndElement();
				writer.WriteStartElement("ServiceCode");
				writer.WriteString(eServiceCode.IntegratedTexting.ToString());
				writer.WriteEndElement();
				writer.WriteEndElement();//End Credentials
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ClinicNum");
				writer.WriteString(clinicNum.ToString());
				writer.WriteEndElement(); //ClinicNum	
				writer.WriteEndElement(); //Payload	
				writer.WriteEndElement(); //Request
			}
			WebServiceMainHQ.WebServiceMainHQ service=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			string result = "";
			try {
				result=service.SmsCancelService(strbuild.ToString());
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

		public static void InsertNewFromList(List<SmsPhone> listPhones,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPhones,clinicNum);
				return;
			}
			string command="SELECT * FROM smsphone WHERE clinicNum="+POut.Long(clinicNum);
			List<SmsPhone> listPhonesBD=Crud.SmsPhoneCrud.SelectMany(command);
			for(int i=0;i<listPhones.Count;i++) {
				bool isNew=true;
				for(int j=0;j<listPhonesBD.Count;j++) {
					if(listPhones[i].PhoneNumber==listPhonesBD[j].PhoneNumber) {
						////Do not reactivate the phone number if it is set incative, it can only be set inactive from HQ.
						//if(listPhones[i].DateTimeActive.Year<1880) {
						//	listPhones[i].DateTimeActive=DateTime.Now;
						//}
						//if(listPhones[i].DateTimeInactive.Year>1880) {
						//	listPhones[i].DateTimeInactive=DateTime.MinValue;
						//}
						isNew=false; 
						break;
					}
				}
				if(isNew) {
					Insert(listPhones[i]);
				}
			}
		}

		///<summary>Returns current clinic limit minus message usage for current calendar month.</summary>
		public static double GetClinicBalance(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),clinicNum);
			}
			double limit=0;
			if(PrefC.GetBool(PrefName.EasyNoClinics) && PrefC.GetDate(PrefName.SmsContractDate).Year>1880){
				limit=PrefC.GetDouble(PrefName.SmsMonthlyLimit);
			}
			else if(Clinics.GetClinic(clinicNum).SmsContractDate.Year>1880){
				limit=Clinics.GetClinic(clinicNum).SmsMonthlyLimit;	
			}
			string command="SELECT SUM(MsgChargeUSD) FROM smstomobile WHERE ClinicNum="+POut.Long(clinicNum);
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