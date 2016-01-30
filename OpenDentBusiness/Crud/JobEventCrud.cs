//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobEventCrud {
		///<summary>Gets one JobEvent object from the database using the primary key.  Returns null if not found.</summary>
		public static JobEvent SelectOne(long jobEventNum){
			string command="SELECT * FROM jobevent "
				+"WHERE JobEventNum = "+POut.Long(jobEventNum);
			List<JobEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobEvent object from the database using a query.</summary>
		public static JobEvent SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobEvent objects from the database using a query.</summary>
		public static List<JobEvent> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobEvent> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobEvent> TableToList(DataTable table){
			List<JobEvent> retVal=new List<JobEvent>();
			JobEvent jobEvent;
			foreach(DataRow row in table.Rows) {
				jobEvent=new JobEvent();
				jobEvent.JobEventNum  = PIn.Long  (row["JobEventNum"].ToString());
				jobEvent.JobNum       = PIn.Long  (row["JobNum"].ToString());
				jobEvent.OwnerNum     = PIn.Long  (row["OwnerNum"].ToString());
				jobEvent.DateTimeEntry= PIn.DateT (row["DateTimeEntry"].ToString());
				jobEvent.Description  = PIn.String(row["Description"].ToString());
				string jobStatus=row["JobStatus"].ToString();
				if(jobStatus==""){
					jobEvent.JobStatus  =(JobStat)0;
				}
				else try{
					jobEvent.JobStatus  =(JobStat)Enum.Parse(typeof(JobStat),jobStatus);
				}
				catch{
					jobEvent.JobStatus  =(JobStat)0;
				}
				retVal.Add(jobEvent);
			}
			return retVal;
		}

		///<summary>Inserts one JobEvent into the database.  Returns the new priKey.</summary>
		public static long Insert(JobEvent jobEvent){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				jobEvent.JobEventNum=DbHelper.GetNextOracleKey("jobevent","JobEventNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(jobEvent,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							jobEvent.JobEventNum++;
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
				return Insert(jobEvent,false);
			}
		}

		///<summary>Inserts one JobEvent into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobEvent jobEvent,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				jobEvent.JobEventNum=ReplicationServers.GetKey("jobevent","JobEventNum");
			}
			string command="INSERT INTO jobevent (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobEventNum,";
			}
			command+="JobNum,OwnerNum,DateTimeEntry,Description,JobStatus) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobEvent.JobEventNum)+",";
			}
			command+=
				     POut.Long  (jobEvent.JobNum)+","
				+    POut.Long  (jobEvent.OwnerNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramDescription,"
				+"'"+POut.String(jobEvent.JobStatus.ToString())+"')";
			if(jobEvent.Description==null) {
				jobEvent.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,jobEvent.Description);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramDescription);
			}
			else {
				jobEvent.JobEventNum=Db.NonQ(command,true,paramDescription);
			}
			return jobEvent.JobEventNum;
		}

		///<summary>Inserts one JobEvent into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobEvent jobEvent){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(jobEvent,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					jobEvent.JobEventNum=DbHelper.GetNextOracleKey("jobevent","JobEventNum"); //Cacheless method
				}
				return InsertNoCache(jobEvent,true);
			}
		}

		///<summary>Inserts one JobEvent into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobEvent jobEvent,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO jobevent (";
			if(!useExistingPK && isRandomKeys) {
				jobEvent.JobEventNum=ReplicationServers.GetKeyNoCache("jobevent","JobEventNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobEventNum,";
			}
			command+="JobNum,OwnerNum,DateTimeEntry,Description,JobStatus) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobEvent.JobEventNum)+",";
			}
			command+=
				     POut.Long  (jobEvent.JobNum)+","
				+    POut.Long  (jobEvent.OwnerNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramDescription,"
				+"'"+POut.String(jobEvent.JobStatus.ToString())+"')";
			if(jobEvent.Description==null) {
				jobEvent.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,jobEvent.Description);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription);
			}
			else {
				jobEvent.JobEventNum=Db.NonQ(command,true,paramDescription);
			}
			return jobEvent.JobEventNum;
		}

		///<summary>Updates one JobEvent in the database.</summary>
		public static void Update(JobEvent jobEvent){
			string command="UPDATE jobevent SET "
				+"JobNum       =  "+POut.Long  (jobEvent.JobNum)+", "
				+"OwnerNum     =  "+POut.Long  (jobEvent.OwnerNum)+", "
				//DateTimeEntry not allowed to change
				+"Description  =  "+DbHelper.ParamChar+"paramDescription, "
				+"JobStatus    = '"+POut.String(jobEvent.JobStatus.ToString())+"' "
				+"WHERE JobEventNum = "+POut.Long(jobEvent.JobEventNum);
			if(jobEvent.Description==null) {
				jobEvent.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,jobEvent.Description);
			Db.NonQ(command,paramDescription);
		}

		///<summary>Updates one JobEvent in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobEvent jobEvent,JobEvent oldJobEvent){
			string command="";
			if(jobEvent.JobNum != oldJobEvent.JobNum) {
				if(command!=""){ command+=",";}
				command+="JobNum = "+POut.Long(jobEvent.JobNum)+"";
			}
			if(jobEvent.OwnerNum != oldJobEvent.OwnerNum) {
				if(command!=""){ command+=",";}
				command+="OwnerNum = "+POut.Long(jobEvent.OwnerNum)+"";
			}
			//DateTimeEntry not allowed to change
			if(jobEvent.Description != oldJobEvent.Description) {
				if(command!=""){ command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(jobEvent.JobStatus != oldJobEvent.JobStatus) {
				if(command!=""){ command+=",";}
				command+="JobStatus = '"+POut.String(jobEvent.JobStatus.ToString())+"'";
			}
			if(command==""){
				return false;
			}
			if(jobEvent.Description==null) {
				jobEvent.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,jobEvent.Description);
			command="UPDATE jobevent SET "+command
				+" WHERE JobEventNum = "+POut.Long(jobEvent.JobEventNum);
			Db.NonQ(command,paramDescription);
			return true;
		}

		///<summary>Returns true if Update(JobEvent,JobEvent) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(JobEvent jobEvent,JobEvent oldJobEvent) {
			if(jobEvent.JobNum != oldJobEvent.JobNum) {
				return true;
			}
			if(jobEvent.OwnerNum != oldJobEvent.OwnerNum) {
				return true;
			}
			//DateTimeEntry not allowed to change
			if(jobEvent.Description != oldJobEvent.Description) {
				return true;
			}
			if(jobEvent.JobStatus != oldJobEvent.JobStatus) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one JobEvent from the database.</summary>
		public static void Delete(long jobEventNum){
			string command="DELETE FROM jobevent "
				+"WHERE JobEventNum = "+POut.Long(jobEventNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobEvent> listNew,List<JobEvent> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<JobEvent> listIns    =new List<JobEvent>();
			List<JobEvent> listUpdNew =new List<JobEvent>();
			List<JobEvent> listUpdDB  =new List<JobEvent>();
			List<JobEvent> listDel    =new List<JobEvent>();
			listNew.Sort((JobEvent x,JobEvent y) => { return x.JobEventNum.CompareTo(y.JobEventNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((JobEvent x,JobEvent y) => { return x.JobEventNum.CompareTo(y.JobEventNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			JobEvent fieldNew;
			JobEvent fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.JobEventNum<fieldDB.JobEventNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.JobEventNum>fieldDB.JobEventNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])){
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].JobEventNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}