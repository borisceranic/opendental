//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class UpdateHistoryCrud {
		///<summary>Gets one UpdateHistory object from the database using the primary key.  Returns null if not found.</summary>
		public static UpdateHistory SelectOne(long updateHistoryNum){
			string command="SELECT * FROM updatehistory "
				+"WHERE UpdateHistoryNum = "+POut.Long(updateHistoryNum);
			List<UpdateHistory> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one UpdateHistory object from the database using a query.</summary>
		public static UpdateHistory SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UpdateHistory> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of UpdateHistory objects from the database using a query.</summary>
		public static List<UpdateHistory> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UpdateHistory> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<UpdateHistory> TableToList(DataTable table){
			List<UpdateHistory> retVal=new List<UpdateHistory>();
			UpdateHistory updateHistory;
			foreach(DataRow row in table.Rows) {
				updateHistory=new UpdateHistory();
				updateHistory.UpdateHistoryNum= PIn.Long  (row["UpdateHistoryNum"].ToString());
				updateHistory.DateTimeUpdated = PIn.DateT (row["DateTimeUpdated"].ToString());
				updateHistory.ProgramVersion  = PIn.String(row["ProgramVersion"].ToString());
				retVal.Add(updateHistory);
			}
			return retVal;
		}

		///<summary>Converts a list of UpdateHistory into a DataTable.</summary>
		public static DataTable ListToTable(List<UpdateHistory> listUpdateHistorys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="UpdateHistory";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("UpdateHistoryNum");
			table.Columns.Add("DateTimeUpdated");
			table.Columns.Add("ProgramVersion");
			foreach(UpdateHistory updateHistory in listUpdateHistorys) {
				table.Rows.Add(new object[] {
					POut.Long  (updateHistory.UpdateHistoryNum),
					POut.DateT (updateHistory.DateTimeUpdated,false),
					            updateHistory.ProgramVersion,
				});
			}
			return table;
		}

		///<summary>Inserts one UpdateHistory into the database.  Returns the new priKey.</summary>
		public static long Insert(UpdateHistory updateHistory){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				updateHistory.UpdateHistoryNum=DbHelper.GetNextOracleKey("updatehistory","UpdateHistoryNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(updateHistory,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							updateHistory.UpdateHistoryNum++;
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
				return Insert(updateHistory,false);
			}
		}

		///<summary>Inserts one UpdateHistory into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(UpdateHistory updateHistory,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				updateHistory.UpdateHistoryNum=ReplicationServers.GetKey("updatehistory","UpdateHistoryNum");
			}
			string command="INSERT INTO updatehistory (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UpdateHistoryNum,";
			}
			command+="DateTimeUpdated,ProgramVersion) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(updateHistory.UpdateHistoryNum)+",";
			}
			command+=
				     DbHelper.Now()+","
				+"'"+POut.String(updateHistory.ProgramVersion)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				updateHistory.UpdateHistoryNum=Db.NonQ(command,true);
			}
			return updateHistory.UpdateHistoryNum;
		}

		///<summary>Inserts one UpdateHistory into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UpdateHistory updateHistory){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(updateHistory,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					updateHistory.UpdateHistoryNum=DbHelper.GetNextOracleKey("updatehistory","UpdateHistoryNum"); //Cacheless method
				}
				return InsertNoCache(updateHistory,true);
			}
		}

		///<summary>Inserts one UpdateHistory into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(UpdateHistory updateHistory,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO updatehistory (";
			if(!useExistingPK && isRandomKeys) {
				updateHistory.UpdateHistoryNum=ReplicationServers.GetKeyNoCache("updatehistory","UpdateHistoryNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="UpdateHistoryNum,";
			}
			command+="DateTimeUpdated,ProgramVersion) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(updateHistory.UpdateHistoryNum)+",";
			}
			command+=
				     DbHelper.Now()+","
				+"'"+POut.String(updateHistory.ProgramVersion)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				updateHistory.UpdateHistoryNum=Db.NonQ(command,true);
			}
			return updateHistory.UpdateHistoryNum;
		}

		///<summary>Updates one UpdateHistory in the database.</summary>
		public static void Update(UpdateHistory updateHistory){
			string command="UPDATE updatehistory SET "
				//DateTimeUpdated not allowed to change
				+"ProgramVersion  = '"+POut.String(updateHistory.ProgramVersion)+"' "
				+"WHERE UpdateHistoryNum = "+POut.Long(updateHistory.UpdateHistoryNum);
			Db.NonQ(command);
		}

		///<summary>Updates one UpdateHistory in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(UpdateHistory updateHistory,UpdateHistory oldUpdateHistory){
			string command="";
			//DateTimeUpdated not allowed to change
			if(updateHistory.ProgramVersion != oldUpdateHistory.ProgramVersion) {
				if(command!=""){ command+=",";}
				command+="ProgramVersion = '"+POut.String(updateHistory.ProgramVersion)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE updatehistory SET "+command
				+" WHERE UpdateHistoryNum = "+POut.Long(updateHistory.UpdateHistoryNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(UpdateHistory,UpdateHistory) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(UpdateHistory updateHistory,UpdateHistory oldUpdateHistory) {
			//DateTimeUpdated not allowed to change
			if(updateHistory.ProgramVersion != oldUpdateHistory.ProgramVersion) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one UpdateHistory from the database.</summary>
		public static void Delete(long updateHistoryNum){
			string command="DELETE FROM updatehistory "
				+"WHERE UpdateHistoryNum = "+POut.Long(updateHistoryNum);
			Db.NonQ(command);
		}

	}
}