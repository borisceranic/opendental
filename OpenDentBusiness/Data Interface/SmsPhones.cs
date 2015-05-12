using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

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
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsPhone>>(MethodBase.GetCurrentMethod());
			}
			string command= "SELECT * FROM smsvln WHERE ClinicNum=0";
			return Crud.SmsPhoneCrud.SelectMany(command);
		}
		
		///<summary>Returns usage for each of the phone numbers passed in. the list of int's are counts of messages 
		///for [SentAllTime],[SentLastMonth],[SentThisMonth],[CostThisMonth],[RcvdAllTime],[RcvdLastMonth],[RcvdThisMonth],[CostThisMonth].</summary>
		public static Dictionary<string,List<double>> GetSMSUsage(List<SmsPhone> phones) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<string,List<double>>>(MethodBase.GetCurrentMethod(),phones);
			}
			Dictionary<string,List<double>> retVal=new Dictionary<string,List<double>>();
			foreach(SmsPhone phone in phones) {
				retVal.Add(phone.PhoneNumber,new List<double> { 0,0,0,0,0,0,0,0 });//Initialize values to zero here. Important, otherwise we can get jagged arrays.
			}
			//Sent All Time
			string command="SELECT SmsPhoneNumber, COUNT(*) FROM smstomobile WHERE MsgCostUSD>0 GROUP BY SmsPhoneNumber";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {//if there are messages in the table for that Phone number 
					retVal[phone][0]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//Sent Last Month
			DateTime dateStartMonthCur=DateTime.Now.AddDays(-DateTime.Now.Day).Date;
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smstomobile "
				+"WHERE DateTimeEntry >"+POut.Date(dateStartMonthCur.AddMonths(-1))+" "
				+"AND DateTimeEntry<"+POut.Date(dateStartMonthCur)+" AND MsgCostUSD>0 GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone][1]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//count and cost sent this month
			command="SELECT SmsPhoneNumber, COUNT(*), SUM(MsgCostUSD) FROM smstomobile "
				+"WHERE DateTimeEntry >"+POut.Date(dateStartMonthCur)+" AND MsgCostUSD>0 GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone][2]=PIn.Int(table.Rows[i][1].ToString());//msg count
					retVal[phone][3]=PIn.Double(table.Rows[i][2].ToString());//msg costs
				}
			}
			//Inbound All Time
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {//if there are messages in the table for that Phone number 
					retVal[phone][4]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//Inbound Last Month
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile "
				+"WHERE DateTimeEntry >"+POut.Date(dateStartMonthCur.AddMonths(-1))+" "
				+"AND DateTimeEntry<"+POut.Date(dateStartMonthCur)+" GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone][5]=PIn.Int(table.Rows[i][1].ToString());
				}
			}
			//count and cost Inbound This Month
			command="SELECT SmsPhoneNumber, COUNT(*) FROM smsfrommobile "
				+"WHERE DateTimeEntry >"+POut.Date(dateStartMonthCur)+" GROUP BY SmsPhoneNumber";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string phone=table.Rows[i][0].ToString();
				if(retVal.ContainsKey(phone)) {
					retVal[phone][6]=PIn.Int(table.Rows[i][1].ToString());//msg count
					retVal[phone][7]=0;//PIn.Double(table.Rows[i][2].ToString());//msg costs
				}
			}
			return retVal;
		}

	}
}