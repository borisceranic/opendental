//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RecallTriggerCrud {
		///<summary>Gets one RecallTrigger object from the database using the primary key.  Returns null if not found.</summary>
		public static RecallTrigger SelectOne(long recallTriggerNum){
			string command="SELECT * FROM recalltrigger "
				+"WHERE RecallTriggerNum = "+POut.Long(recallTriggerNum);
			List<RecallTrigger> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RecallTrigger object from the database using a query.</summary>
		public static RecallTrigger SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RecallTrigger> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RecallTrigger objects from the database using a query.</summary>
		public static List<RecallTrigger> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RecallTrigger> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RecallTrigger> TableToList(DataTable table){
			List<RecallTrigger> retVal=new List<RecallTrigger>();
			RecallTrigger recallTrigger;
			foreach(DataRow row in table.Rows) {
				recallTrigger=new RecallTrigger();
				recallTrigger.RecallTriggerNum= PIn.Long  (row["RecallTriggerNum"].ToString());
				recallTrigger.RecallTypeNum   = PIn.Long  (row["RecallTypeNum"].ToString());
				recallTrigger.CodeNum         = PIn.Long  (row["CodeNum"].ToString());
				retVal.Add(recallTrigger);
			}
			return retVal;
		}

		///<summary>Converts a list of RecallTrigger into a DataTable.</summary>
		public static DataTable ListToTable(List<RecallTrigger> listRecallTriggers,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RecallTrigger";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RecallTriggerNum");
			table.Columns.Add("RecallTypeNum");
			table.Columns.Add("CodeNum");
			foreach(RecallTrigger recallTrigger in listRecallTriggers) {
				table.Rows.Add(new object[] {
					POut.Long  (recallTrigger.RecallTriggerNum),
					POut.Long  (recallTrigger.RecallTypeNum),
					POut.Long  (recallTrigger.CodeNum),
				});
			}
			return table;
		}

		///<summary>Inserts one RecallTrigger into the database.  Returns the new priKey.</summary>
		public static long Insert(RecallTrigger recallTrigger){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				recallTrigger.RecallTriggerNum=DbHelper.GetNextOracleKey("recalltrigger","RecallTriggerNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(recallTrigger,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							recallTrigger.RecallTriggerNum++;
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
				return Insert(recallTrigger,false);
			}
		}

		///<summary>Inserts one RecallTrigger into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RecallTrigger recallTrigger,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				recallTrigger.RecallTriggerNum=ReplicationServers.GetKey("recalltrigger","RecallTriggerNum");
			}
			string command="INSERT INTO recalltrigger (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RecallTriggerNum,";
			}
			command+="RecallTypeNum,CodeNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(recallTrigger.RecallTriggerNum)+",";
			}
			command+=
				     POut.Long  (recallTrigger.RecallTypeNum)+","
				+    POut.Long  (recallTrigger.CodeNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				recallTrigger.RecallTriggerNum=Db.NonQ(command,true);
			}
			return recallTrigger.RecallTriggerNum;
		}

		///<summary>Inserts one RecallTrigger into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RecallTrigger recallTrigger){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(recallTrigger,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					recallTrigger.RecallTriggerNum=DbHelper.GetNextOracleKey("recalltrigger","RecallTriggerNum"); //Cacheless method
				}
				return InsertNoCache(recallTrigger,true);
			}
		}

		///<summary>Inserts one RecallTrigger into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RecallTrigger recallTrigger,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO recalltrigger (";
			if(!useExistingPK && isRandomKeys) {
				recallTrigger.RecallTriggerNum=ReplicationServers.GetKeyNoCache("recalltrigger","RecallTriggerNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RecallTriggerNum,";
			}
			command+="RecallTypeNum,CodeNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(recallTrigger.RecallTriggerNum)+",";
			}
			command+=
				     POut.Long  (recallTrigger.RecallTypeNum)+","
				+    POut.Long  (recallTrigger.CodeNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				recallTrigger.RecallTriggerNum=Db.NonQ(command,true);
			}
			return recallTrigger.RecallTriggerNum;
		}

		///<summary>Updates one RecallTrigger in the database.</summary>
		public static void Update(RecallTrigger recallTrigger){
			string command="UPDATE recalltrigger SET "
				+"RecallTypeNum   =  "+POut.Long  (recallTrigger.RecallTypeNum)+", "
				+"CodeNum         =  "+POut.Long  (recallTrigger.CodeNum)+" "
				+"WHERE RecallTriggerNum = "+POut.Long(recallTrigger.RecallTriggerNum);
			Db.NonQ(command);
		}

		///<summary>Updates one RecallTrigger in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RecallTrigger recallTrigger,RecallTrigger oldRecallTrigger){
			string command="";
			if(recallTrigger.RecallTypeNum != oldRecallTrigger.RecallTypeNum) {
				if(command!=""){ command+=",";}
				command+="RecallTypeNum = "+POut.Long(recallTrigger.RecallTypeNum)+"";
			}
			if(recallTrigger.CodeNum != oldRecallTrigger.CodeNum) {
				if(command!=""){ command+=",";}
				command+="CodeNum = "+POut.Long(recallTrigger.CodeNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE recalltrigger SET "+command
				+" WHERE RecallTriggerNum = "+POut.Long(recallTrigger.RecallTriggerNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one RecallTrigger from the database.</summary>
		public static void Delete(long recallTriggerNum){
			string command="DELETE FROM recalltrigger "
				+"WHERE RecallTriggerNum = "+POut.Long(recallTriggerNum);
			Db.NonQ(command);
		}

	}
}