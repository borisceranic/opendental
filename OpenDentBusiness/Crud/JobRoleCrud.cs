//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobRoleCrud {
		///<summary>Gets one JobRole object from the database using the primary key.  Returns null if not found.</summary>
		public static JobRole SelectOne(long jobRoleNum){
			string command="SELECT * FROM jobrole "
				+"WHERE JobRoleNum = "+POut.Long(jobRoleNum);
			List<JobRole> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobRole object from the database using a query.</summary>
		public static JobRole SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobRole> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobRole objects from the database using a query.</summary>
		public static List<JobRole> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobRole> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobRole> TableToList(DataTable table){
			List<JobRole> retVal=new List<JobRole>();
			JobRole jobRole;
			for(int i=0;i<table.Rows.Count;i++) {
				jobRole=new JobRole();
				jobRole.JobRoleNum= PIn.Long  (table.Rows[i]["JobRoleNum"].ToString());
				jobRole.UserNum   = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				jobRole.RoleType  = (OpenDentBusiness.JobRoleType)PIn.Int(table.Rows[i]["RoleType"].ToString());
				retVal.Add(jobRole);
			}
			return retVal;
		}

		///<summary>Inserts one JobRole into the database.  Returns the new priKey.</summary>
		public static long Insert(JobRole jobRole){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				jobRole.JobRoleNum=DbHelper.GetNextOracleKey("jobrole","JobRoleNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(jobRole,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							jobRole.JobRoleNum++;
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
				return Insert(jobRole,false);
			}
		}

		///<summary>Inserts one JobRole into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobRole jobRole,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				jobRole.JobRoleNum=ReplicationServers.GetKey("jobrole","JobRoleNum");
			}
			string command="INSERT INTO jobrole (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobRoleNum,";
			}
			command+="UserNum,RoleType) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobRole.JobRoleNum)+",";
			}
			command+=
				     POut.Long  (jobRole.UserNum)+","
				+    POut.Int   ((int)jobRole.RoleType)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobRole.JobRoleNum=Db.NonQ(command,true);
			}
			return jobRole.JobRoleNum;
		}

		///<summary>Inserts one JobRole into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobRole jobRole){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(jobRole,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					jobRole.JobRoleNum=DbHelper.GetNextOracleKey("jobrole","JobRoleNum"); //Cacheless method
				}
				return InsertNoCache(jobRole,true);
			}
		}

		///<summary>Inserts one JobRole into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobRole jobRole,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO jobrole (";
			if(!useExistingPK && isRandomKeys) {
				jobRole.JobRoleNum=ReplicationServers.GetKeyNoCache("jobrole","JobRoleNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobRoleNum,";
			}
			command+="UserNum,RoleType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobRole.JobRoleNum)+",";
			}
			command+=
				     POut.Long  (jobRole.UserNum)+","
				+    POut.Int   ((int)jobRole.RoleType)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobRole.JobRoleNum=Db.NonQ(command,true);
			}
			return jobRole.JobRoleNum;
		}

		///<summary>Updates one JobRole in the database.</summary>
		public static void Update(JobRole jobRole){
			string command="UPDATE jobrole SET "
				+"UserNum   =  "+POut.Long  (jobRole.UserNum)+", "
				+"RoleType  =  "+POut.Int   ((int)jobRole.RoleType)+" "
				+"WHERE JobRoleNum = "+POut.Long(jobRole.JobRoleNum);
			Db.NonQ(command);
		}

		///<summary>Updates one JobRole in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobRole jobRole,JobRole oldJobRole){
			string command="";
			if(jobRole.UserNum != oldJobRole.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(jobRole.UserNum)+"";
			}
			if(jobRole.RoleType != oldJobRole.RoleType) {
				if(command!=""){ command+=",";}
				command+="RoleType = "+POut.Int   ((int)jobRole.RoleType)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE jobrole SET "+command
				+" WHERE JobRoleNum = "+POut.Long(jobRole.JobRoleNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one JobRole from the database.</summary>
		public static void Delete(long jobRoleNum){
			string command="DELETE FROM jobrole "
				+"WHERE JobRoleNum = "+POut.Long(jobRoleNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobRole> listNew,List<JobRole> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<JobRole> listIns    =new List<JobRole>();
			List<JobRole> listUpdNew =new List<JobRole>();
			List<JobRole> listUpdDB  =new List<JobRole>();
			List<JobRole> listDel    =new List<JobRole>();
			listNew.Sort((JobRole x,JobRole y) => { return x.JobRoleNum.CompareTo(y.JobRoleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((JobRole x,JobRole y) => { return x.JobRoleNum.CompareTo(y.JobRoleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			JobRole fieldNew;
			JobRole fieldDB;
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
				else if(fieldNew.JobRoleNum<fieldDB.JobRoleNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.JobRoleNum>fieldDB.JobRoleNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].JobRoleNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}