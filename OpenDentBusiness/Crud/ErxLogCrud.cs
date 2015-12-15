//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ErxLogCrud {
		///<summary>Gets one ErxLog object from the database using the primary key.  Returns null if not found.</summary>
		public static ErxLog SelectOne(long erxLogNum){
			string command="SELECT * FROM erxlog "
				+"WHERE ErxLogNum = "+POut.Long(erxLogNum);
			List<ErxLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ErxLog object from the database using a query.</summary>
		public static ErxLog SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ErxLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ErxLog objects from the database using a query.</summary>
		public static List<ErxLog> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ErxLog> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ErxLog> TableToList(DataTable table){
			List<ErxLog> retVal=new List<ErxLog>();
			ErxLog erxLog;
			foreach(DataRow row in table.Rows) {
				erxLog=new ErxLog();
				erxLog.ErxLogNum = PIn.Long  (row["ErxLogNum"].ToString());
				erxLog.PatNum    = PIn.Long  (row["PatNum"].ToString());
				erxLog.MsgText   = PIn.String(row["MsgText"].ToString());
				erxLog.DateTStamp= PIn.DateT (row["DateTStamp"].ToString());
				erxLog.ProvNum   = PIn.Long  (row["ProvNum"].ToString());
				retVal.Add(erxLog);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ErxLog> listErxLogs) {
			DataTable table=new DataTable("ErxLogs");
			table.Columns.Add("ErxLogNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("MsgText");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("ProvNum");
			foreach(ErxLog erxLog in listErxLogs) {
				table.Rows.Add(new object[] {
					POut.Long  (erxLog.ErxLogNum),
					POut.Long  (erxLog.PatNum),
					POut.String(erxLog.MsgText),
					POut.DateT (erxLog.DateTStamp),
					POut.Long  (erxLog.ProvNum),
				});
			}
			return table;
		}

		///<summary>Inserts one ErxLog into the database.  Returns the new priKey.</summary>
		public static long Insert(ErxLog erxLog){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				erxLog.ErxLogNum=DbHelper.GetNextOracleKey("erxlog","ErxLogNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(erxLog,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							erxLog.ErxLogNum++;
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
				return Insert(erxLog,false);
			}
		}

		///<summary>Inserts one ErxLog into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ErxLog erxLog,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				erxLog.ErxLogNum=ReplicationServers.GetKey("erxlog","ErxLogNum");
			}
			string command="INSERT INTO erxlog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ErxLogNum,";
			}
			command+="PatNum,MsgText,ProvNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(erxLog.ErxLogNum)+",";
			}
			command+=
				     POut.Long  (erxLog.PatNum)+","
				+    DbHelper.ParamChar+"paramMsgText,"
				//DateTStamp can only be set by MySQL
				+    POut.Long  (erxLog.ProvNum)+")";
			if(erxLog.MsgText==null) {
				erxLog.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,erxLog.MsgText);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				erxLog.ErxLogNum=Db.NonQ(command,true,paramMsgText);
			}
			return erxLog.ErxLogNum;
		}

		///<summary>Inserts one ErxLog into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ErxLog erxLog){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(erxLog,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					erxLog.ErxLogNum=DbHelper.GetNextOracleKey("erxlog","ErxLogNum"); //Cacheless method
				}
				return InsertNoCache(erxLog,true);
			}
		}

		///<summary>Inserts one ErxLog into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ErxLog erxLog,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO erxlog (";
			if(!useExistingPK && isRandomKeys) {
				erxLog.ErxLogNum=ReplicationServers.GetKeyNoCache("erxlog","ErxLogNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ErxLogNum,";
			}
			command+="PatNum,MsgText,ProvNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(erxLog.ErxLogNum)+",";
			}
			command+=
				     POut.Long  (erxLog.PatNum)+","
				+    DbHelper.ParamChar+"paramMsgText,"
				//DateTStamp can only be set by MySQL
				+    POut.Long  (erxLog.ProvNum)+")";
			if(erxLog.MsgText==null) {
				erxLog.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,erxLog.MsgText);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				erxLog.ErxLogNum=Db.NonQ(command,true,paramMsgText);
			}
			return erxLog.ErxLogNum;
		}

		///<summary>Updates one ErxLog in the database.</summary>
		public static void Update(ErxLog erxLog){
			string command="UPDATE erxlog SET "
				+"PatNum    =  "+POut.Long  (erxLog.PatNum)+", "
				+"MsgText   =  "+DbHelper.ParamChar+"paramMsgText, "
				//DateTStamp can only be set by MySQL
				+"ProvNum   =  "+POut.Long  (erxLog.ProvNum)+" "
				+"WHERE ErxLogNum = "+POut.Long(erxLog.ErxLogNum);
			if(erxLog.MsgText==null) {
				erxLog.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,erxLog.MsgText);
			Db.NonQ(command,paramMsgText);
		}

		///<summary>Updates one ErxLog in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ErxLog erxLog,ErxLog oldErxLog){
			string command="";
			if(erxLog.PatNum != oldErxLog.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(erxLog.PatNum)+"";
			}
			if(erxLog.MsgText != oldErxLog.MsgText) {
				if(command!=""){ command+=",";}
				command+="MsgText = "+DbHelper.ParamChar+"paramMsgText";
			}
			//DateTStamp can only be set by MySQL
			if(erxLog.ProvNum != oldErxLog.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(erxLog.ProvNum)+"";
			}
			if(command==""){
				return false;
			}
			if(erxLog.MsgText==null) {
				erxLog.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,erxLog.MsgText);
			command="UPDATE erxlog SET "+command
				+" WHERE ErxLogNum = "+POut.Long(erxLog.ErxLogNum);
			Db.NonQ(command,paramMsgText);
			return true;
		}

		///<summary>Deletes one ErxLog from the database.</summary>
		public static void Delete(long erxLogNum){
			string command="DELETE FROM erxlog "
				+"WHERE ErxLogNum = "+POut.Long(erxLogNum);
			Db.NonQ(command);
		}

	}
}