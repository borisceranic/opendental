//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class TaskUnreadCrud {
		///<summary>Gets one TaskUnread object from the database using the primary key.  Returns null if not found.</summary>
		public static TaskUnread SelectOne(long taskUnreadNum){
			string command="SELECT * FROM taskunread "
				+"WHERE TaskUnreadNum = "+POut.Long(taskUnreadNum);
			List<TaskUnread> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one TaskUnread object from the database using a query.</summary>
		public static TaskUnread SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskUnread> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of TaskUnread objects from the database using a query.</summary>
		public static List<TaskUnread> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskUnread> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<TaskUnread> TableToList(DataTable table){
			List<TaskUnread> retVal=new List<TaskUnread>();
			TaskUnread taskUnread;
			for(int i=0;i<table.Rows.Count;i++) {
				taskUnread=new TaskUnread();
				taskUnread.TaskUnreadNum= PIn.Long  (table.Rows[i]["TaskUnreadNum"].ToString());
				taskUnread.TaskNum      = PIn.Long  (table.Rows[i]["TaskNum"].ToString());
				taskUnread.UserNum      = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				retVal.Add(taskUnread);
			}
			return retVal;
		}

		///<summary>Inserts one TaskUnread into the database.  Returns the new priKey.</summary>
		public static long Insert(TaskUnread taskUnread){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				taskUnread.TaskUnreadNum=DbHelper.GetNextOracleKey("taskunread","TaskUnreadNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(taskUnread,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							taskUnread.TaskUnreadNum++;
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
				return Insert(taskUnread,false);
			}
		}

		///<summary>Inserts one TaskUnread into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(TaskUnread taskUnread,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				taskUnread.TaskUnreadNum=ReplicationServers.GetKey("taskunread","TaskUnreadNum");
			}
			string command="INSERT INTO taskunread (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="TaskUnreadNum,";
			}
			command+="TaskNum,UserNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(taskUnread.TaskUnreadNum)+",";
			}
			command+=
				     POut.Long  (taskUnread.TaskNum)+","
				+    POut.Long  (taskUnread.UserNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskUnread.TaskUnreadNum=Db.NonQ(command,true);
			}
			return taskUnread.TaskUnreadNum;
		}

		///<summary>Inserts one TaskUnread into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskUnread taskUnread){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(taskUnread,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					taskUnread.TaskUnreadNum=DbHelper.GetNextOracleKey("taskunread","TaskUnreadNum"); //Cacheless method
				}
				return InsertNoCache(taskUnread,true);
			}
		}

		///<summary>Inserts one TaskUnread into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskUnread taskUnread,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO taskunread (";
			if(!useExistingPK && isRandomKeys) {
				taskUnread.TaskUnreadNum=ReplicationServers.GetKeyNoCache("taskunread","TaskUnreadNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="TaskUnreadNum,";
			}
			command+="TaskNum,UserNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(taskUnread.TaskUnreadNum)+",";
			}
			command+=
				     POut.Long  (taskUnread.TaskNum)+","
				+    POut.Long  (taskUnread.UserNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskUnread.TaskUnreadNum=Db.NonQ(command,true);
			}
			return taskUnread.TaskUnreadNum;
		}

		///<summary>Updates one TaskUnread in the database.</summary>
		public static void Update(TaskUnread taskUnread){
			string command="UPDATE taskunread SET "
				+"TaskNum      =  "+POut.Long  (taskUnread.TaskNum)+", "
				+"UserNum      =  "+POut.Long  (taskUnread.UserNum)+" "
				+"WHERE TaskUnreadNum = "+POut.Long(taskUnread.TaskUnreadNum);
			Db.NonQ(command);
		}

		///<summary>Updates one TaskUnread in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(TaskUnread taskUnread,TaskUnread oldTaskUnread){
			string command="";
			if(taskUnread.TaskNum != oldTaskUnread.TaskNum) {
				if(command!=""){ command+=",";}
				command+="TaskNum = "+POut.Long(taskUnread.TaskNum)+"";
			}
			if(taskUnread.UserNum != oldTaskUnread.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(taskUnread.UserNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE taskunread SET "+command
				+" WHERE TaskUnreadNum = "+POut.Long(taskUnread.TaskUnreadNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one TaskUnread from the database.</summary>
		public static void Delete(long taskUnreadNum){
			string command="DELETE FROM taskunread "
				+"WHERE TaskUnreadNum = "+POut.Long(taskUnreadNum);
			Db.NonQ(command);
		}

	}
}