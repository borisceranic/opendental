//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SmsToMobileCrud {
		///<summary>Gets one SmsToMobile object from the database using the primary key.  Returns null if not found.</summary>
		public static SmsToMobile SelectOne(long smsToMobileNum){
			string command="SELECT * FROM smstomobile "
				+"WHERE SmsToMobileNum = "+POut.Long(smsToMobileNum);
			List<SmsToMobile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SmsToMobile object from the database using a query.</summary>
		public static SmsToMobile SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SmsToMobile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SmsToMobile objects from the database using a query.</summary>
		public static List<SmsToMobile> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SmsToMobile> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SmsToMobile> TableToList(DataTable table){
			List<SmsToMobile> retVal=new List<SmsToMobile>();
			SmsToMobile smsToMobile;
			for(int i=0;i<table.Rows.Count;i++) {
				smsToMobile=new SmsToMobile();
				smsToMobile.SmsToMobileNum    = PIn.Long  (table.Rows[i]["SmsToMobileNum"].ToString());
				smsToMobile.PatNum            = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				smsToMobile.GuidMessage       = PIn.String(table.Rows[i]["GuidMessage"].ToString());
				smsToMobile.GuidBatch         = PIn.String(table.Rows[i]["GuidBatch"].ToString());
				smsToMobile.SmsPhoneNumber    = PIn.String(table.Rows[i]["SmsPhoneNumber"].ToString());
				smsToMobile.MobilePhoneNumber = PIn.String(table.Rows[i]["MobilePhoneNumber"].ToString());
				smsToMobile.IsTimeSensitive   = PIn.Bool  (table.Rows[i]["IsTimeSensitive"].ToString());
				smsToMobile.MsgType           = (OpenDentBusiness.SMSMessageSource)PIn.Int(table.Rows[i]["MsgType"].ToString());
				smsToMobile.MsgText           = PIn.String(table.Rows[i]["MsgText"].ToString());
				smsToMobile.Status            = (OpenDentBusiness.SMSDeliveryStatus)PIn.Int(table.Rows[i]["Status"].ToString());
				smsToMobile.MsgParts          = PIn.Int   (table.Rows[i]["MsgParts"].ToString());
				smsToMobile.MsgCostUSD        = PIn.Double(table.Rows[i]["MsgCostUSD"].ToString());
				smsToMobile.ClinicNum         = PIn.Long  (table.Rows[i]["ClinicNum"].ToString());
				smsToMobile.CustErrorText     = PIn.String(table.Rows[i]["CustErrorText"].ToString());
				smsToMobile.DateTimeSent      = PIn.DateT (table.Rows[i]["DateTimeSent"].ToString());
				smsToMobile.DateTimeTerminated= PIn.DateT (table.Rows[i]["DateTimeTerminated"].ToString());
				retVal.Add(smsToMobile);
			}
			return retVal;
		}

		///<summary>Inserts one SmsToMobile into the database.  Returns the new priKey.</summary>
		public static long Insert(SmsToMobile smsToMobile){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				smsToMobile.SmsToMobileNum=DbHelper.GetNextOracleKey("smstomobile","SmsToMobileNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(smsToMobile,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							smsToMobile.SmsToMobileNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(smsToMobile,false);
			}
		}

		///<summary>Inserts one SmsToMobile into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SmsToMobile smsToMobile,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				smsToMobile.SmsToMobileNum=ReplicationServers.GetKey("smstomobile","SmsToMobileNum");
			}
			string command="INSERT INTO smstomobile (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SmsToMobileNum,";
			}
			command+="PatNum,GuidMessage,GuidBatch,SmsPhoneNumber,MobilePhoneNumber,IsTimeSensitive,MsgType,MsgText,Status,MsgParts,MsgCostUSD,ClinicNum,CustErrorText,DateTimeSent,DateTimeTerminated) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(smsToMobile.SmsToMobileNum)+",";
			}
			command+=
				     POut.Long  (smsToMobile.PatNum)+","
				+"'"+POut.String(smsToMobile.GuidMessage)+"',"
				+"'"+POut.String(smsToMobile.GuidBatch)+"',"
				+"'"+POut.String(smsToMobile.SmsPhoneNumber)+"',"
				+"'"+POut.String(smsToMobile.MobilePhoneNumber)+"',"
				+    POut.Bool  (smsToMobile.IsTimeSensitive)+","
				+    POut.Int   ((int)smsToMobile.MsgType)+","
				+    DbHelper.ParamChar+"paramMsgText,"
				+    POut.Int   ((int)smsToMobile.Status)+","
				+    POut.Int   (smsToMobile.MsgParts)+","
				+"'"+POut.Double(smsToMobile.MsgCostUSD)+"',"
				+    POut.Long  (smsToMobile.ClinicNum)+","
				+"'"+POut.String(smsToMobile.CustErrorText)+"',"
				+    POut.DateT (smsToMobile.DateTimeSent)+","
				+    POut.DateT (smsToMobile.DateTimeTerminated)+")";
			if(smsToMobile.MsgText==null) {
				smsToMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsToMobile.MsgText));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				smsToMobile.SmsToMobileNum=Db.NonQ(command,true,paramMsgText);
			}
			return smsToMobile.SmsToMobileNum;
		}

		///<summary>Updates one SmsToMobile in the database.</summary>
		public static void Update(SmsToMobile smsToMobile){
			string command="UPDATE smstomobile SET "
				+"PatNum            =  "+POut.Long  (smsToMobile.PatNum)+", "
				+"GuidMessage       = '"+POut.String(smsToMobile.GuidMessage)+"', "
				+"GuidBatch         = '"+POut.String(smsToMobile.GuidBatch)+"', "
				+"SmsPhoneNumber    = '"+POut.String(smsToMobile.SmsPhoneNumber)+"', "
				+"MobilePhoneNumber = '"+POut.String(smsToMobile.MobilePhoneNumber)+"', "
				+"IsTimeSensitive   =  "+POut.Bool  (smsToMobile.IsTimeSensitive)+", "
				+"MsgType           =  "+POut.Int   ((int)smsToMobile.MsgType)+", "
				+"MsgText           =  "+DbHelper.ParamChar+"paramMsgText, "
				+"Status            =  "+POut.Int   ((int)smsToMobile.Status)+", "
				+"MsgParts          =  "+POut.Int   (smsToMobile.MsgParts)+", "
				+"MsgCostUSD        = '"+POut.Double(smsToMobile.MsgCostUSD)+"', "
				+"ClinicNum         =  "+POut.Long  (smsToMobile.ClinicNum)+", "
				+"CustErrorText     = '"+POut.String(smsToMobile.CustErrorText)+"', "
				+"DateTimeSent      =  "+POut.DateT (smsToMobile.DateTimeSent)+", "
				+"DateTimeTerminated=  "+POut.DateT (smsToMobile.DateTimeTerminated)+" "
				+"WHERE SmsToMobileNum = "+POut.Long(smsToMobile.SmsToMobileNum);
			if(smsToMobile.MsgText==null) {
				smsToMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsToMobile.MsgText));
			Db.NonQ(command,paramMsgText);
		}

		///<summary>Updates one SmsToMobile in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SmsToMobile smsToMobile,SmsToMobile oldSmsToMobile){
			string command="";
			if(smsToMobile.PatNum != oldSmsToMobile.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(smsToMobile.PatNum)+"";
			}
			if(smsToMobile.GuidMessage != oldSmsToMobile.GuidMessage) {
				if(command!=""){ command+=",";}
				command+="GuidMessage = '"+POut.String(smsToMobile.GuidMessage)+"'";
			}
			if(smsToMobile.GuidBatch != oldSmsToMobile.GuidBatch) {
				if(command!=""){ command+=",";}
				command+="GuidBatch = '"+POut.String(smsToMobile.GuidBatch)+"'";
			}
			if(smsToMobile.SmsPhoneNumber != oldSmsToMobile.SmsPhoneNumber) {
				if(command!=""){ command+=",";}
				command+="SmsPhoneNumber = '"+POut.String(smsToMobile.SmsPhoneNumber)+"'";
			}
			if(smsToMobile.MobilePhoneNumber != oldSmsToMobile.MobilePhoneNumber) {
				if(command!=""){ command+=",";}
				command+="MobilePhoneNumber = '"+POut.String(smsToMobile.MobilePhoneNumber)+"'";
			}
			if(smsToMobile.IsTimeSensitive != oldSmsToMobile.IsTimeSensitive) {
				if(command!=""){ command+=",";}
				command+="IsTimeSensitive = "+POut.Bool(smsToMobile.IsTimeSensitive)+"";
			}
			if(smsToMobile.MsgType != oldSmsToMobile.MsgType) {
				if(command!=""){ command+=",";}
				command+="MsgType = "+POut.Int   ((int)smsToMobile.MsgType)+"";
			}
			if(smsToMobile.MsgText != oldSmsToMobile.MsgText) {
				if(command!=""){ command+=",";}
				command+="MsgText = "+DbHelper.ParamChar+"paramMsgText";
			}
			if(smsToMobile.Status != oldSmsToMobile.Status) {
				if(command!=""){ command+=",";}
				command+="Status = "+POut.Int   ((int)smsToMobile.Status)+"";
			}
			if(smsToMobile.MsgParts != oldSmsToMobile.MsgParts) {
				if(command!=""){ command+=",";}
				command+="MsgParts = "+POut.Int(smsToMobile.MsgParts)+"";
			}
			if(smsToMobile.MsgCostUSD != oldSmsToMobile.MsgCostUSD) {
				if(command!=""){ command+=",";}
				command+="MsgCostUSD = '"+POut.Double(smsToMobile.MsgCostUSD)+"'";
			}
			if(smsToMobile.ClinicNum != oldSmsToMobile.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(smsToMobile.ClinicNum)+"";
			}
			if(smsToMobile.CustErrorText != oldSmsToMobile.CustErrorText) {
				if(command!=""){ command+=",";}
				command+="CustErrorText = '"+POut.String(smsToMobile.CustErrorText)+"'";
			}
			if(smsToMobile.DateTimeSent != oldSmsToMobile.DateTimeSent) {
				if(command!=""){ command+=",";}
				command+="DateTimeSent = "+POut.DateT(smsToMobile.DateTimeSent)+"";
			}
			if(smsToMobile.DateTimeTerminated != oldSmsToMobile.DateTimeTerminated) {
				if(command!=""){ command+=",";}
				command+="DateTimeTerminated = "+POut.DateT(smsToMobile.DateTimeTerminated)+"";
			}
			if(command==""){
				return false;
			}
			if(smsToMobile.MsgText==null) {
				smsToMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsToMobile.MsgText));
			command="UPDATE smstomobile SET "+command
				+" WHERE SmsToMobileNum = "+POut.Long(smsToMobile.SmsToMobileNum);
			Db.NonQ(command,paramMsgText);
			return true;
		}

		///<summary>Deletes one SmsToMobile from the database.</summary>
		public static void Delete(long smsToMobileNum){
			string command="DELETE FROM smstomobile "
				+"WHERE SmsToMobileNum = "+POut.Long(smsToMobileNum);
			Db.NonQ(command);
		}

	}
}