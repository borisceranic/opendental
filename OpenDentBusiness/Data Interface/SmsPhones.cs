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

		///<summary>Gets sms phones when not using clinics.</summary>
		public static List<SmsPhone> GetForPractice() {
			//No remoting role check, No call to database.
			//Get for practice is just getting for clinic num 0
			return GetForClinics(new List<Clinic>() { new Clinic() });
		}

		///<summary>Returns usage for each of the phone numbers passed in. the list of int's are counts of messages 
		///for [SentAllTime],[SentLastMonth],[SentThisMonth],[CostThisMonth],[RcvdAllTime],[RcvdLastMonth],[RcvdThisMonth],[CostThisMonth].</summary>
		public static Dictionary<string,Dictionary<string,double>> GetSmsUsageLocal(List<SmsPhone> phones) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<string,Dictionary<string,double>>>(MethodBase.GetCurrentMethod(),phones);
			}
			//Dictionary<string,List<double>> retVal=new Dictionary<string,List<double>>();
			Dictionary<string,Dictionary<string,double>> retVal=new Dictionary<string,Dictionary<string,double>>();
			foreach(SmsPhone phone in phones) {
				retVal.Add(phone.PhoneNumber,new Dictionary<string,double>());
				//Initialize values to zero here. Important, otherwise we can get jagged arrays.
				retVal[phone.PhoneNumber].Add("SentAllTime",0);
				retVal[phone.PhoneNumber].Add("SentLastMonth",0);
				retVal[phone.PhoneNumber].Add("SentThisMonth",0);
				retVal[phone.PhoneNumber].Add("SentThisMonthCost",0);
				retVal[phone.PhoneNumber].Add("InboundAllTime",0);
				retVal[phone.PhoneNumber].Add("InboundLastMonth",0);
				retVal[phone.PhoneNumber].Add("InboundThisMonth",0);
				retVal[phone.PhoneNumber].Add("InboundThisMonthCost",0);
			}
			//Sent All Time
			string command="SELECT SmsPhoneNumber, COUNT(*) FROM smstomobile WHERE MsgChargeUSD>0 GROUP BY SmsPhoneNumber";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {//if there are messages in the table for that Phone number 
					retVal[phone]["SentAllTime"]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//Sent Last Month
			DateTime dateStartMonthCur=DateTime.Now.AddDays(-DateTime.Now.Day).Date;
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smstomobile "
				+"WHERE DateTimeSent >"+POut.Date(dateStartMonthCur.AddMonths(-1))+" "
				+"AND DateTimeSent<"+POut.Date(dateStartMonthCur)+" AND MsgChargeUSD>0 GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone]["SentLastMonth"]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//count and cost sent this month
			command="SELECT SmsPhoneNumber, COUNT(*), SUM(MsgChargeUSD) FROM smstomobile "
				+"WHERE DateTimeSent >"+POut.Date(dateStartMonthCur)+" AND MsgChargeUSD>0 GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone]["SentThisMonth"]=PIn.Int(table.Rows[i][1].ToString());//msg count
					retVal[phone]["SentThisMonthCost"]=PIn.Double(table.Rows[i][2].ToString());//msg costs
				}
			}
			//Inbound All Time
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {//if there are messages in the table for that Phone number 
					retVal[phone]["InboundAllTime"]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//Inbound Last Month
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile "
				+"WHERE DateTimeReceived >"+POut.Date(dateStartMonthCur.AddMonths(-1))+" "
				+"AND DateTimeReceived<"+POut.Date(dateStartMonthCur)+" GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone]["InboundLastMonth"]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//count and cost Inbound This Month
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile "
				+"WHERE DateTimeReceived >"+POut.Date(dateStartMonthCur)+" GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone]["InboundThisMonth"]=PIn.Int(table.Rows[i][1].ToString());//msg count
					retVal[phone]["InboundThisMonthCost"]=0;//PIn.Double(table.Rows[i][2].ToString());//msg costs
				}
			}
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
			WebServiceMainHQ.WebServiceMainHQ service=new WebServiceMainHQ.WebServiceMainHQ();
#if DEBUG
			service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";
#endif
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
			WebServiceMainHQ.WebServiceMainHQ service=new WebServiceMainHQ.WebServiceMainHQ();
#if DEBUG
			service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";
#endif
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