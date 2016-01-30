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
			foreach(DataRow row in table.Rows) {
				smsToMobile=new SmsToMobile();
				smsToMobile.SmsToMobileNum    = PIn.Long  (row["SmsToMobileNum"].ToString());
				smsToMobile.PatNum            = PIn.Long  (row["PatNum"].ToString());
				smsToMobile.GuidMessage       = PIn.String(row["GuidMessage"].ToString());
				smsToMobile.GuidBatch         = PIn.String(row["GuidBatch"].ToString());
				smsToMobile.SmsPhoneNumber    = PIn.String(row["SmsPhoneNumber"].ToString());
				smsToMobile.MobilePhoneNumber = PIn.String(row["MobilePhoneNumber"].ToString());
				smsToMobile.IsTimeSensitive   = PIn.Bool  (row["IsTimeSensitive"].ToString());
				smsToMobile.MsgType           = (OpenDentBusiness.SmsMessageSource)PIn.Int(row["MsgType"].ToString());
				smsToMobile.MsgText           = PIn.String(row["MsgText"].ToString());
				smsToMobile.SmsStatus         = (OpenDentBusiness.SmsDeliveryStatus)PIn.Int(row["SmsStatus"].ToString());
				smsToMobile.MsgParts          = PIn.Int   (row["MsgParts"].ToString());
				smsToMobile.MsgChargeUSD      = PIn.Float (row["MsgChargeUSD"].ToString());
				smsToMobile.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				smsToMobile.CustErrorText     = PIn.String(row["CustErrorText"].ToString());
				smsToMobile.DateTimeSent      = PIn.DateT (row["DateTimeSent"].ToString());
				smsToMobile.DateTimeTerminated= PIn.DateT (row["DateTimeTerminated"].ToString());
				smsToMobile.IsHidden          = PIn.Bool  (row["IsHidden"].ToString());
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
			command+="PatNum,GuidMessage,GuidBatch,SmsPhoneNumber,MobilePhoneNumber,IsTimeSensitive,MsgType,MsgText,SmsStatus,MsgParts,MsgChargeUSD,ClinicNum,CustErrorText,DateTimeSent,DateTimeTerminated,IsHidden) VALUES(";
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
				+    POut.Int   ((int)smsToMobile.SmsStatus)+","
				+    POut.Int   (smsToMobile.MsgParts)+","
				+    POut.Float (smsToMobile.MsgChargeUSD)+","
				+    POut.Long  (smsToMobile.ClinicNum)+","
				+"'"+POut.String(smsToMobile.CustErrorText)+"',"
				+    POut.DateT (smsToMobile.DateTimeSent)+","
				+    POut.DateT (smsToMobile.DateTimeTerminated)+","
				+    POut.Bool  (smsToMobile.IsHidden)+")";
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

		///<summary>Inserts one SmsToMobile into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsToMobile smsToMobile){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(smsToMobile,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					smsToMobile.SmsToMobileNum=DbHelper.GetNextOracleKey("smstomobile","SmsToMobileNum"); //Cacheless method
				}
				return InsertNoCache(smsToMobile,true);
			}
		}

		///<summary>Inserts one SmsToMobile into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsToMobile smsToMobile,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO smstomobile (";
			if(!useExistingPK && isRandomKeys) {
				smsToMobile.SmsToMobileNum=ReplicationServers.GetKeyNoCache("smstomobile","SmsToMobileNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SmsToMobileNum,";
			}
			command+="PatNum,GuidMessage,GuidBatch,SmsPhoneNumber,MobilePhoneNumber,IsTimeSensitive,MsgType,MsgText,SmsStatus,MsgParts,MsgChargeUSD,ClinicNum,CustErrorText,DateTimeSent,DateTimeTerminated,IsHidden) VALUES(";
			if(isRandomKeys || useExistingPK) {
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
				+    POut.Int   ((int)smsToMobile.SmsStatus)+","
				+    POut.Int   (smsToMobile.MsgParts)+","
				+    POut.Float (smsToMobile.MsgChargeUSD)+","
				+    POut.Long  (smsToMobile.ClinicNum)+","
				+"'"+POut.String(smsToMobile.CustErrorText)+"',"
				+    POut.DateT (smsToMobile.DateTimeSent)+","
				+    POut.DateT (smsToMobile.DateTimeTerminated)+","
				+    POut.Bool  (smsToMobile.IsHidden)+")";
			if(smsToMobile.MsgText==null) {
				smsToMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsToMobile.MsgText));
			if(useExistingPK || isRandomKeys) {
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
				+"SmsStatus         =  "+POut.Int   ((int)smsToMobile.SmsStatus)+", "
				+"MsgParts          =  "+POut.Int   (smsToMobile.MsgParts)+", "
				+"MsgChargeUSD      =  "+POut.Float (smsToMobile.MsgChargeUSD)+", "
				+"ClinicNum         =  "+POut.Long  (smsToMobile.ClinicNum)+", "
				+"CustErrorText     = '"+POut.String(smsToMobile.CustErrorText)+"', "
				+"DateTimeSent      =  "+POut.DateT (smsToMobile.DateTimeSent)+", "
				+"DateTimeTerminated=  "+POut.DateT (smsToMobile.DateTimeTerminated)+", "
				+"IsHidden          =  "+POut.Bool  (smsToMobile.IsHidden)+" "
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
			if(smsToMobile.SmsStatus != oldSmsToMobile.SmsStatus) {
				if(command!=""){ command+=",";}
				command+="SmsStatus = "+POut.Int   ((int)smsToMobile.SmsStatus)+"";
			}
			if(smsToMobile.MsgParts != oldSmsToMobile.MsgParts) {
				if(command!=""){ command+=",";}
				command+="MsgParts = "+POut.Int(smsToMobile.MsgParts)+"";
			}
			if(smsToMobile.MsgChargeUSD != oldSmsToMobile.MsgChargeUSD) {
				if(command!=""){ command+=",";}
				command+="MsgChargeUSD = "+POut.Float(smsToMobile.MsgChargeUSD)+"";
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
			if(smsToMobile.IsHidden != oldSmsToMobile.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(smsToMobile.IsHidden)+"";
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

		///<summary>Returns true if Update(SmsToMobile,SmsToMobile) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SmsToMobile smsToMobile,SmsToMobile oldSmsToMobile) {
			if(smsToMobile.PatNum != oldSmsToMobile.PatNum) {
				return true;
			}
			if(smsToMobile.GuidMessage != oldSmsToMobile.GuidMessage) {
				return true;
			}
			if(smsToMobile.GuidBatch != oldSmsToMobile.GuidBatch) {
				return true;
			}
			if(smsToMobile.SmsPhoneNumber != oldSmsToMobile.SmsPhoneNumber) {
				return true;
			}
			if(smsToMobile.MobilePhoneNumber != oldSmsToMobile.MobilePhoneNumber) {
				return true;
			}
			if(smsToMobile.IsTimeSensitive != oldSmsToMobile.IsTimeSensitive) {
				return true;
			}
			if(smsToMobile.MsgType != oldSmsToMobile.MsgType) {
				return true;
			}
			if(smsToMobile.MsgText != oldSmsToMobile.MsgText) {
				return true;
			}
			if(smsToMobile.SmsStatus != oldSmsToMobile.SmsStatus) {
				return true;
			}
			if(smsToMobile.MsgParts != oldSmsToMobile.MsgParts) {
				return true;
			}
			if(smsToMobile.MsgChargeUSD != oldSmsToMobile.MsgChargeUSD) {
				return true;
			}
			if(smsToMobile.ClinicNum != oldSmsToMobile.ClinicNum) {
				return true;
			}
			if(smsToMobile.CustErrorText != oldSmsToMobile.CustErrorText) {
				return true;
			}
			if(smsToMobile.DateTimeSent != oldSmsToMobile.DateTimeSent) {
				return true;
			}
			if(smsToMobile.DateTimeTerminated != oldSmsToMobile.DateTimeTerminated) {
				return true;
			}
			if(smsToMobile.IsHidden != oldSmsToMobile.IsHidden) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SmsToMobile from the database.</summary>
		public static void Delete(long smsToMobileNum){
			string command="DELETE FROM smstomobile "
				+"WHERE SmsToMobileNum = "+POut.Long(smsToMobileNum);
			Db.NonQ(command);
		}

	}
}