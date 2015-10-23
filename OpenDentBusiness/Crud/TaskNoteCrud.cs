//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class TaskNoteCrud {
		///<summary>Gets one TaskNote object from the database using the primary key.  Returns null if not found.</summary>
		public static TaskNote SelectOne(long taskNoteNum){
			string command="SELECT * FROM tasknote "
				+"WHERE TaskNoteNum = "+POut.Long(taskNoteNum);
			List<TaskNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one TaskNote object from the database using a query.</summary>
		public static TaskNote SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of TaskNote objects from the database using a query.</summary>
		public static List<TaskNote> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<TaskNote> TableToList(DataTable table){
			List<TaskNote> retVal=new List<TaskNote>();
			TaskNote taskNote;
			for(int i=0;i<table.Rows.Count;i++) {
				taskNote=new TaskNote();
				taskNote.TaskNoteNum = PIn.Long  (table.Rows[i]["TaskNoteNum"].ToString());
				taskNote.TaskNum     = PIn.Long  (table.Rows[i]["TaskNum"].ToString());
				taskNote.UserNum     = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				taskNote.DateTimeNote= PIn.DateT (table.Rows[i]["DateTimeNote"].ToString());
				taskNote.Note        = PIn.String(table.Rows[i]["Note"].ToString());
				retVal.Add(taskNote);
			}
			return retVal;
		}

		///<summary>Inserts one TaskNote into the database.  Returns the new priKey.</summary>
		public static long Insert(TaskNote taskNote){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				taskNote.TaskNoteNum=DbHelper.GetNextOracleKey("tasknote","TaskNoteNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(taskNote,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							taskNote.TaskNoteNum++;
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
				return Insert(taskNote,false);
			}
		}

		///<summary>Inserts one TaskNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(TaskNote taskNote,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				taskNote.TaskNoteNum=ReplicationServers.GetKey("tasknote","TaskNoteNum");
			}
			string command="INSERT INTO tasknote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="TaskNoteNum,";
			}
			command+="TaskNum,UserNum,DateTimeNote,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(taskNote.TaskNoteNum)+",";
			}
			command+=
				     POut.Long  (taskNote.TaskNum)+","
				+    POut.Long  (taskNote.UserNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.String(taskNote.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskNote.TaskNoteNum=Db.NonQ(command,true);
			}
			return taskNote.TaskNoteNum;
		}

		///<summary>Inserts one TaskNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskNote taskNote){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(taskNote,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					taskNote.TaskNoteNum=DbHelper.GetNextOracleKey("tasknote","TaskNoteNum"); //Cacheless method
				}
				return InsertNoCache(taskNote,true);
			}
		}

		///<summary>Inserts one TaskNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(TaskNote taskNote,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO tasknote (";
			if(!useExistingPK && isRandomKeys) {
				taskNote.TaskNoteNum=ReplicationServers.GetKeyNoCache("tasknote","TaskNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="TaskNoteNum,";
			}
			command+="TaskNum,UserNum,DateTimeNote,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(taskNote.TaskNoteNum)+",";
			}
			command+=
				     POut.Long  (taskNote.TaskNum)+","
				+    POut.Long  (taskNote.UserNum)+","
				+    DbHelper.Now()+","
				+"'"+POut.String(taskNote.Note)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskNote.TaskNoteNum=Db.NonQ(command,true);
			}
			return taskNote.TaskNoteNum;
		}

		///<summary>Updates one TaskNote in the database.</summary>
		public static void Update(TaskNote taskNote){
			string command="UPDATE tasknote SET "
				+"TaskNum     =  "+POut.Long  (taskNote.TaskNum)+", "
				+"UserNum     =  "+POut.Long  (taskNote.UserNum)+", "
				+"DateTimeNote=  "+POut.DateT (taskNote.DateTimeNote)+", "
				+"Note        = '"+POut.String(taskNote.Note)+"' "
				+"WHERE TaskNoteNum = "+POut.Long(taskNote.TaskNoteNum);
			Db.NonQ(command);
		}

		///<summary>Updates one TaskNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(TaskNote taskNote,TaskNote oldTaskNote){
			string command="";
			if(taskNote.TaskNum != oldTaskNote.TaskNum) {
				if(command!=""){ command+=",";}
				command+="TaskNum = "+POut.Long(taskNote.TaskNum)+"";
			}
			if(taskNote.UserNum != oldTaskNote.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(taskNote.UserNum)+"";
			}
			if(taskNote.DateTimeNote != oldTaskNote.DateTimeNote) {
				if(command!=""){ command+=",";}
				command+="DateTimeNote = "+POut.DateT(taskNote.DateTimeNote)+"";
			}
			if(taskNote.Note != oldTaskNote.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(taskNote.Note)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE tasknote SET "+command
				+" WHERE TaskNoteNum = "+POut.Long(taskNote.TaskNoteNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one TaskNote from the database.</summary>
		public static void Delete(long taskNoteNum){
			string command="DELETE FROM tasknote "
				+"WHERE TaskNoteNum = "+POut.Long(taskNoteNum);
			Db.NonQ(command);
		}

	}
}