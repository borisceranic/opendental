//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ScheduleOpCrud {
		///<summary>Gets one ScheduleOp object from the database using the primary key.  Returns null if not found.</summary>
		public static ScheduleOp SelectOne(long scheduleOpNum){
			string command="SELECT * FROM scheduleop "
				+"WHERE ScheduleOpNum = "+POut.Long(scheduleOpNum);
			List<ScheduleOp> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ScheduleOp object from the database using a query.</summary>
		public static ScheduleOp SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ScheduleOp> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ScheduleOp objects from the database using a query.</summary>
		public static List<ScheduleOp> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ScheduleOp> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ScheduleOp> TableToList(DataTable table){
			List<ScheduleOp> retVal=new List<ScheduleOp>();
			ScheduleOp scheduleOp;
			foreach(DataRow row in table.Rows) {
				scheduleOp=new ScheduleOp();
				scheduleOp.ScheduleOpNum= PIn.Long  (row["ScheduleOpNum"].ToString());
				scheduleOp.ScheduleNum  = PIn.Long  (row["ScheduleNum"].ToString());
				scheduleOp.OperatoryNum = PIn.Long  (row["OperatoryNum"].ToString());
				retVal.Add(scheduleOp);
			}
			return retVal;
		}

		///<summary>Inserts one ScheduleOp into the database.  Returns the new priKey.</summary>
		public static long Insert(ScheduleOp scheduleOp){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				scheduleOp.ScheduleOpNum=DbHelper.GetNextOracleKey("scheduleop","ScheduleOpNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(scheduleOp,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							scheduleOp.ScheduleOpNum++;
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
				return Insert(scheduleOp,false);
			}
		}

		///<summary>Inserts one ScheduleOp into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ScheduleOp scheduleOp,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				scheduleOp.ScheduleOpNum=ReplicationServers.GetKey("scheduleop","ScheduleOpNum");
			}
			string command="INSERT INTO scheduleop (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ScheduleOpNum,";
			}
			command+="ScheduleNum,OperatoryNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(scheduleOp.ScheduleOpNum)+",";
			}
			command+=
				     POut.Long  (scheduleOp.ScheduleNum)+","
				+    POut.Long  (scheduleOp.OperatoryNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				scheduleOp.ScheduleOpNum=Db.NonQ(command,true);
			}
			return scheduleOp.ScheduleOpNum;
		}

		///<summary>Inserts one ScheduleOp into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ScheduleOp scheduleOp){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(scheduleOp,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					scheduleOp.ScheduleOpNum=DbHelper.GetNextOracleKey("scheduleop","ScheduleOpNum"); //Cacheless method
				}
				return InsertNoCache(scheduleOp,true);
			}
		}

		///<summary>Inserts one ScheduleOp into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ScheduleOp scheduleOp,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO scheduleop (";
			if(!useExistingPK && isRandomKeys) {
				scheduleOp.ScheduleOpNum=ReplicationServers.GetKeyNoCache("scheduleop","ScheduleOpNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ScheduleOpNum,";
			}
			command+="ScheduleNum,OperatoryNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(scheduleOp.ScheduleOpNum)+",";
			}
			command+=
				     POut.Long  (scheduleOp.ScheduleNum)+","
				+    POut.Long  (scheduleOp.OperatoryNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				scheduleOp.ScheduleOpNum=Db.NonQ(command,true);
			}
			return scheduleOp.ScheduleOpNum;
		}

		///<summary>Updates one ScheduleOp in the database.</summary>
		public static void Update(ScheduleOp scheduleOp){
			string command="UPDATE scheduleop SET "
				+"ScheduleNum  =  "+POut.Long  (scheduleOp.ScheduleNum)+", "
				+"OperatoryNum =  "+POut.Long  (scheduleOp.OperatoryNum)+" "
				+"WHERE ScheduleOpNum = "+POut.Long(scheduleOp.ScheduleOpNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ScheduleOp in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ScheduleOp scheduleOp,ScheduleOp oldScheduleOp){
			string command="";
			if(scheduleOp.ScheduleNum != oldScheduleOp.ScheduleNum) {
				if(command!=""){ command+=",";}
				command+="ScheduleNum = "+POut.Long(scheduleOp.ScheduleNum)+"";
			}
			if(scheduleOp.OperatoryNum != oldScheduleOp.OperatoryNum) {
				if(command!=""){ command+=",";}
				command+="OperatoryNum = "+POut.Long(scheduleOp.OperatoryNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE scheduleop SET "+command
				+" WHERE ScheduleOpNum = "+POut.Long(scheduleOp.ScheduleOpNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ScheduleOp from the database.</summary>
		public static void Delete(long scheduleOpNum){
			string command="DELETE FROM scheduleop "
				+"WHERE ScheduleOpNum = "+POut.Long(scheduleOpNum);
			Db.NonQ(command);
		}

	}
}