//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SmsFromMobileCrud {
		///<summary>Gets one SmsFromMobile object from the database using the primary key.  Returns null if not found.</summary>
		public static SmsFromMobile SelectOne(long smsFromMobileNum){
			string command="SELECT * FROM smsfrommobile "
				+"WHERE SmsFromMobileNum = "+POut.Long(smsFromMobileNum);
			List<SmsFromMobile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SmsFromMobile object from the database using a query.</summary>
		public static SmsFromMobile SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SmsFromMobile> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SmsFromMobile objects from the database using a query.</summary>
		public static List<SmsFromMobile> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SmsFromMobile> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SmsFromMobile> TableToList(DataTable table){
			List<SmsFromMobile> retVal=new List<SmsFromMobile>();
			SmsFromMobile smsFromMobile;
			foreach(DataRow row in table.Rows) {
				smsFromMobile=new SmsFromMobile();
				smsFromMobile.SmsFromMobileNum = PIn.Long  (row["SmsFromMobileNum"].ToString());
				smsFromMobile.PatNum           = PIn.Long  (row["PatNum"].ToString());
				smsFromMobile.ClinicNum        = PIn.Long  (row["ClinicNum"].ToString());
				smsFromMobile.CommlogNum       = PIn.Long  (row["CommlogNum"].ToString());
				smsFromMobile.MsgText          = PIn.String(row["MsgText"].ToString());
				smsFromMobile.DateTimeReceived = PIn.DateT (row["DateTimeReceived"].ToString());
				smsFromMobile.SmsPhoneNumber   = PIn.String(row["SmsPhoneNumber"].ToString());
				smsFromMobile.MobilePhoneNumber= PIn.String(row["MobilePhoneNumber"].ToString());
				smsFromMobile.MsgPart          = PIn.Int   (row["MsgPart"].ToString());
				smsFromMobile.MsgTotal         = PIn.Int   (row["MsgTotal"].ToString());
				smsFromMobile.MsgRefID         = PIn.String(row["MsgRefID"].ToString());
				smsFromMobile.SmsStatus        = (OpenDentBusiness.SmsFromStatus)PIn.Int(row["SmsStatus"].ToString());
				smsFromMobile.Flags            = PIn.String(row["Flags"].ToString());
				smsFromMobile.IsHidden         = PIn.Bool  (row["IsHidden"].ToString());
				smsFromMobile.MatchCount       = PIn.Int   (row["MatchCount"].ToString());
				retVal.Add(smsFromMobile);
			}
			return retVal;
		}

		///<summary>Converts a list of SmsFromMobile into a DataTable.</summary>
		public static DataTable ListToTable(List<SmsFromMobile> listSmsFromMobiles,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SmsFromMobile";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SmsFromMobileNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("CommlogNum");
			table.Columns.Add("MsgText");
			table.Columns.Add("DateTimeReceived");
			table.Columns.Add("SmsPhoneNumber");
			table.Columns.Add("MobilePhoneNumber");
			table.Columns.Add("MsgPart");
			table.Columns.Add("MsgTotal");
			table.Columns.Add("MsgRefID");
			table.Columns.Add("SmsStatus");
			table.Columns.Add("Flags");
			table.Columns.Add("IsHidden");
			table.Columns.Add("MatchCount");
			foreach(SmsFromMobile smsFromMobile in listSmsFromMobiles) {
				table.Rows.Add(new object[] {
					POut.Long  (smsFromMobile.SmsFromMobileNum),
					POut.Long  (smsFromMobile.PatNum),
					POut.Long  (smsFromMobile.ClinicNum),
					POut.Long  (smsFromMobile.CommlogNum),
					POut.String(smsFromMobile.MsgText),
					POut.DateT (smsFromMobile.DateTimeReceived),
					POut.String(smsFromMobile.SmsPhoneNumber),
					POut.String(smsFromMobile.MobilePhoneNumber),
					POut.Int   (smsFromMobile.MsgPart),
					POut.Int   (smsFromMobile.MsgTotal),
					POut.String(smsFromMobile.MsgRefID),
					POut.Int   ((int)smsFromMobile.SmsStatus),
					POut.String(smsFromMobile.Flags),
					POut.Bool  (smsFromMobile.IsHidden),
					POut.Int   (smsFromMobile.MatchCount),
				});
			}
			return table;
		}

		///<summary>Inserts one SmsFromMobile into the database.  Returns the new priKey.</summary>
		public static long Insert(SmsFromMobile smsFromMobile){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				smsFromMobile.SmsFromMobileNum=DbHelper.GetNextOracleKey("smsfrommobile","SmsFromMobileNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(smsFromMobile,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							smsFromMobile.SmsFromMobileNum++;
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
				return Insert(smsFromMobile,false);
			}
		}

		///<summary>Inserts one SmsFromMobile into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SmsFromMobile smsFromMobile,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				smsFromMobile.SmsFromMobileNum=ReplicationServers.GetKey("smsfrommobile","SmsFromMobileNum");
			}
			string command="INSERT INTO smsfrommobile (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SmsFromMobileNum,";
			}
			command+="PatNum,ClinicNum,CommlogNum,MsgText,DateTimeReceived,SmsPhoneNumber,MobilePhoneNumber,MsgPart,MsgTotal,MsgRefID,SmsStatus,Flags,IsHidden,MatchCount) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(smsFromMobile.SmsFromMobileNum)+",";
			}
			command+=
				     POut.Long  (smsFromMobile.PatNum)+","
				+    POut.Long  (smsFromMobile.ClinicNum)+","
				+    POut.Long  (smsFromMobile.CommlogNum)+","
				+    DbHelper.ParamChar+"paramMsgText,"
				+    POut.DateT (smsFromMobile.DateTimeReceived)+","
				+"'"+POut.String(smsFromMobile.SmsPhoneNumber)+"',"
				+"'"+POut.String(smsFromMobile.MobilePhoneNumber)+"',"
				+    POut.Int   (smsFromMobile.MsgPart)+","
				+    POut.Int   (smsFromMobile.MsgTotal)+","
				+"'"+POut.String(smsFromMobile.MsgRefID)+"',"
				+    POut.Int   ((int)smsFromMobile.SmsStatus)+","
				+"'"+POut.String(smsFromMobile.Flags)+"',"
				+    POut.Bool  (smsFromMobile.IsHidden)+","
				+    POut.Int   (smsFromMobile.MatchCount)+")";
			if(smsFromMobile.MsgText==null) {
				smsFromMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsFromMobile.MsgText));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				smsFromMobile.SmsFromMobileNum=Db.NonQ(command,true,paramMsgText);
			}
			return smsFromMobile.SmsFromMobileNum;
		}

		///<summary>Inserts one SmsFromMobile into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsFromMobile smsFromMobile){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(smsFromMobile,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					smsFromMobile.SmsFromMobileNum=DbHelper.GetNextOracleKey("smsfrommobile","SmsFromMobileNum"); //Cacheless method
				}
				return InsertNoCache(smsFromMobile,true);
			}
		}

		///<summary>Inserts one SmsFromMobile into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SmsFromMobile smsFromMobile,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO smsfrommobile (";
			if(!useExistingPK && isRandomKeys) {
				smsFromMobile.SmsFromMobileNum=ReplicationServers.GetKeyNoCache("smsfrommobile","SmsFromMobileNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SmsFromMobileNum,";
			}
			command+="PatNum,ClinicNum,CommlogNum,MsgText,DateTimeReceived,SmsPhoneNumber,MobilePhoneNumber,MsgPart,MsgTotal,MsgRefID,SmsStatus,Flags,IsHidden,MatchCount) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(smsFromMobile.SmsFromMobileNum)+",";
			}
			command+=
				     POut.Long  (smsFromMobile.PatNum)+","
				+    POut.Long  (smsFromMobile.ClinicNum)+","
				+    POut.Long  (smsFromMobile.CommlogNum)+","
				+    DbHelper.ParamChar+"paramMsgText,"
				+    POut.DateT (smsFromMobile.DateTimeReceived)+","
				+"'"+POut.String(smsFromMobile.SmsPhoneNumber)+"',"
				+"'"+POut.String(smsFromMobile.MobilePhoneNumber)+"',"
				+    POut.Int   (smsFromMobile.MsgPart)+","
				+    POut.Int   (smsFromMobile.MsgTotal)+","
				+"'"+POut.String(smsFromMobile.MsgRefID)+"',"
				+    POut.Int   ((int)smsFromMobile.SmsStatus)+","
				+"'"+POut.String(smsFromMobile.Flags)+"',"
				+    POut.Bool  (smsFromMobile.IsHidden)+","
				+    POut.Int   (smsFromMobile.MatchCount)+")";
			if(smsFromMobile.MsgText==null) {
				smsFromMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsFromMobile.MsgText));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				smsFromMobile.SmsFromMobileNum=Db.NonQ(command,true,paramMsgText);
			}
			return smsFromMobile.SmsFromMobileNum;
		}

		///<summary>Updates one SmsFromMobile in the database.</summary>
		public static void Update(SmsFromMobile smsFromMobile){
			string command="UPDATE smsfrommobile SET "
				+"PatNum           =  "+POut.Long  (smsFromMobile.PatNum)+", "
				+"ClinicNum        =  "+POut.Long  (smsFromMobile.ClinicNum)+", "
				+"CommlogNum       =  "+POut.Long  (smsFromMobile.CommlogNum)+", "
				+"MsgText          =  "+DbHelper.ParamChar+"paramMsgText, "
				+"DateTimeReceived =  "+POut.DateT (smsFromMobile.DateTimeReceived)+", "
				+"SmsPhoneNumber   = '"+POut.String(smsFromMobile.SmsPhoneNumber)+"', "
				+"MobilePhoneNumber= '"+POut.String(smsFromMobile.MobilePhoneNumber)+"', "
				+"MsgPart          =  "+POut.Int   (smsFromMobile.MsgPart)+", "
				+"MsgTotal         =  "+POut.Int   (smsFromMobile.MsgTotal)+", "
				+"MsgRefID         = '"+POut.String(smsFromMobile.MsgRefID)+"', "
				+"SmsStatus        =  "+POut.Int   ((int)smsFromMobile.SmsStatus)+", "
				+"Flags            = '"+POut.String(smsFromMobile.Flags)+"', "
				+"IsHidden         =  "+POut.Bool  (smsFromMobile.IsHidden)+", "
				+"MatchCount       =  "+POut.Int   (smsFromMobile.MatchCount)+" "
				+"WHERE SmsFromMobileNum = "+POut.Long(smsFromMobile.SmsFromMobileNum);
			if(smsFromMobile.MsgText==null) {
				smsFromMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsFromMobile.MsgText));
			Db.NonQ(command,paramMsgText);
		}

		///<summary>Updates one SmsFromMobile in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SmsFromMobile smsFromMobile,SmsFromMobile oldSmsFromMobile){
			string command="";
			if(smsFromMobile.PatNum != oldSmsFromMobile.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(smsFromMobile.PatNum)+"";
			}
			if(smsFromMobile.ClinicNum != oldSmsFromMobile.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(smsFromMobile.ClinicNum)+"";
			}
			if(smsFromMobile.CommlogNum != oldSmsFromMobile.CommlogNum) {
				if(command!=""){ command+=",";}
				command+="CommlogNum = "+POut.Long(smsFromMobile.CommlogNum)+"";
			}
			if(smsFromMobile.MsgText != oldSmsFromMobile.MsgText) {
				if(command!=""){ command+=",";}
				command+="MsgText = "+DbHelper.ParamChar+"paramMsgText";
			}
			if(smsFromMobile.DateTimeReceived != oldSmsFromMobile.DateTimeReceived) {
				if(command!=""){ command+=",";}
				command+="DateTimeReceived = "+POut.DateT(smsFromMobile.DateTimeReceived)+"";
			}
			if(smsFromMobile.SmsPhoneNumber != oldSmsFromMobile.SmsPhoneNumber) {
				if(command!=""){ command+=",";}
				command+="SmsPhoneNumber = '"+POut.String(smsFromMobile.SmsPhoneNumber)+"'";
			}
			if(smsFromMobile.MobilePhoneNumber != oldSmsFromMobile.MobilePhoneNumber) {
				if(command!=""){ command+=",";}
				command+="MobilePhoneNumber = '"+POut.String(smsFromMobile.MobilePhoneNumber)+"'";
			}
			if(smsFromMobile.MsgPart != oldSmsFromMobile.MsgPart) {
				if(command!=""){ command+=",";}
				command+="MsgPart = "+POut.Int(smsFromMobile.MsgPart)+"";
			}
			if(smsFromMobile.MsgTotal != oldSmsFromMobile.MsgTotal) {
				if(command!=""){ command+=",";}
				command+="MsgTotal = "+POut.Int(smsFromMobile.MsgTotal)+"";
			}
			if(smsFromMobile.MsgRefID != oldSmsFromMobile.MsgRefID) {
				if(command!=""){ command+=",";}
				command+="MsgRefID = '"+POut.String(smsFromMobile.MsgRefID)+"'";
			}
			if(smsFromMobile.SmsStatus != oldSmsFromMobile.SmsStatus) {
				if(command!=""){ command+=",";}
				command+="SmsStatus = "+POut.Int   ((int)smsFromMobile.SmsStatus)+"";
			}
			if(smsFromMobile.Flags != oldSmsFromMobile.Flags) {
				if(command!=""){ command+=",";}
				command+="Flags = '"+POut.String(smsFromMobile.Flags)+"'";
			}
			if(smsFromMobile.IsHidden != oldSmsFromMobile.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(smsFromMobile.IsHidden)+"";
			}
			if(smsFromMobile.MatchCount != oldSmsFromMobile.MatchCount) {
				if(command!=""){ command+=",";}
				command+="MatchCount = "+POut.Int(smsFromMobile.MatchCount)+"";
			}
			if(command==""){
				return false;
			}
			if(smsFromMobile.MsgText==null) {
				smsFromMobile.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,POut.StringNote(smsFromMobile.MsgText));
			command="UPDATE smsfrommobile SET "+command
				+" WHERE SmsFromMobileNum = "+POut.Long(smsFromMobile.SmsFromMobileNum);
			Db.NonQ(command,paramMsgText);
			return true;
		}

		///<summary>Deletes one SmsFromMobile from the database.</summary>
		public static void Delete(long smsFromMobileNum){
			string command="DELETE FROM smsfrommobile "
				+"WHERE SmsFromMobileNum = "+POut.Long(smsFromMobileNum);
			Db.NonQ(command);
		}

	}
}