//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class GuardianCrud {
		///<summary>Gets one Guardian object from the database using the primary key.  Returns null if not found.</summary>
		public static Guardian SelectOne(long guardianNum){
			string command="SELECT * FROM guardian "
				+"WHERE GuardianNum = "+POut.Long(guardianNum);
			List<Guardian> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Guardian object from the database using a query.</summary>
		public static Guardian SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Guardian> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Guardian objects from the database using a query.</summary>
		public static List<Guardian> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Guardian> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Guardian> TableToList(DataTable table){
			List<Guardian> retVal=new List<Guardian>();
			Guardian guardian;
			foreach(DataRow row in table.Rows) {
				guardian=new Guardian();
				guardian.GuardianNum   = PIn.Long  (row["GuardianNum"].ToString());
				guardian.PatNumChild   = PIn.Long  (row["PatNumChild"].ToString());
				guardian.PatNumGuardian= PIn.Long  (row["PatNumGuardian"].ToString());
				guardian.Relationship  = (OpenDentBusiness.GuardianRelationship)PIn.Int(row["Relationship"].ToString());
				guardian.IsGuardian    = PIn.Bool  (row["IsGuardian"].ToString());
				retVal.Add(guardian);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<Guardian> listGuardians) {
			DataTable table=new DataTable("Guardians");
			table.Columns.Add("GuardianNum");
			table.Columns.Add("PatNumChild");
			table.Columns.Add("PatNumGuardian");
			table.Columns.Add("Relationship");
			table.Columns.Add("IsGuardian");
			foreach(Guardian guardian in listGuardians) {
				table.Rows.Add(new object[] {
					POut.Long  (guardian.GuardianNum),
					POut.Long  (guardian.PatNumChild),
					POut.Long  (guardian.PatNumGuardian),
					POut.Int   ((int)guardian.Relationship),
					POut.Bool  (guardian.IsGuardian),
				});
			}
			return table;
		}

		///<summary>Inserts one Guardian into the database.  Returns the new priKey.</summary>
		public static long Insert(Guardian guardian){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				guardian.GuardianNum=DbHelper.GetNextOracleKey("guardian","GuardianNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(guardian,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							guardian.GuardianNum++;
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
				return Insert(guardian,false);
			}
		}

		///<summary>Inserts one Guardian into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Guardian guardian,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				guardian.GuardianNum=ReplicationServers.GetKey("guardian","GuardianNum");
			}
			string command="INSERT INTO guardian (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="GuardianNum,";
			}
			command+="PatNumChild,PatNumGuardian,Relationship,IsGuardian) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(guardian.GuardianNum)+",";
			}
			command+=
				     POut.Long  (guardian.PatNumChild)+","
				+    POut.Long  (guardian.PatNumGuardian)+","
				+    POut.Int   ((int)guardian.Relationship)+","
				+    POut.Bool  (guardian.IsGuardian)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				guardian.GuardianNum=Db.NonQ(command,true);
			}
			return guardian.GuardianNum;
		}

		///<summary>Inserts one Guardian into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Guardian guardian){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(guardian,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					guardian.GuardianNum=DbHelper.GetNextOracleKey("guardian","GuardianNum"); //Cacheless method
				}
				return InsertNoCache(guardian,true);
			}
		}

		///<summary>Inserts one Guardian into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Guardian guardian,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO guardian (";
			if(!useExistingPK && isRandomKeys) {
				guardian.GuardianNum=ReplicationServers.GetKeyNoCache("guardian","GuardianNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="GuardianNum,";
			}
			command+="PatNumChild,PatNumGuardian,Relationship,IsGuardian) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(guardian.GuardianNum)+",";
			}
			command+=
				     POut.Long  (guardian.PatNumChild)+","
				+    POut.Long  (guardian.PatNumGuardian)+","
				+    POut.Int   ((int)guardian.Relationship)+","
				+    POut.Bool  (guardian.IsGuardian)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				guardian.GuardianNum=Db.NonQ(command,true);
			}
			return guardian.GuardianNum;
		}

		///<summary>Updates one Guardian in the database.</summary>
		public static void Update(Guardian guardian){
			string command="UPDATE guardian SET "
				+"PatNumChild   =  "+POut.Long  (guardian.PatNumChild)+", "
				+"PatNumGuardian=  "+POut.Long  (guardian.PatNumGuardian)+", "
				+"Relationship  =  "+POut.Int   ((int)guardian.Relationship)+", "
				+"IsGuardian    =  "+POut.Bool  (guardian.IsGuardian)+" "
				+"WHERE GuardianNum = "+POut.Long(guardian.GuardianNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Guardian in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Guardian guardian,Guardian oldGuardian){
			string command="";
			if(guardian.PatNumChild != oldGuardian.PatNumChild) {
				if(command!=""){ command+=",";}
				command+="PatNumChild = "+POut.Long(guardian.PatNumChild)+"";
			}
			if(guardian.PatNumGuardian != oldGuardian.PatNumGuardian) {
				if(command!=""){ command+=",";}
				command+="PatNumGuardian = "+POut.Long(guardian.PatNumGuardian)+"";
			}
			if(guardian.Relationship != oldGuardian.Relationship) {
				if(command!=""){ command+=",";}
				command+="Relationship = "+POut.Int   ((int)guardian.Relationship)+"";
			}
			if(guardian.IsGuardian != oldGuardian.IsGuardian) {
				if(command!=""){ command+=",";}
				command+="IsGuardian = "+POut.Bool(guardian.IsGuardian)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE guardian SET "+command
				+" WHERE GuardianNum = "+POut.Long(guardian.GuardianNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Guardian from the database.</summary>
		public static void Delete(long guardianNum){
			string command="DELETE FROM guardian "
				+"WHERE GuardianNum = "+POut.Long(guardianNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<Guardian> listNew,List<Guardian> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Guardian> listIns    =new List<Guardian>();
			List<Guardian> listUpdNew =new List<Guardian>();
			List<Guardian> listUpdDB  =new List<Guardian>();
			List<Guardian> listDel    =new List<Guardian>();
			listNew.Sort((Guardian x,Guardian y) => { return x.GuardianNum.CompareTo(y.GuardianNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((Guardian x,Guardian y) => { return x.GuardianNum.CompareTo(y.GuardianNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			Guardian fieldNew;
			Guardian fieldDB;
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
				else if(fieldNew.GuardianNum<fieldDB.GuardianNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.GuardianNum>fieldDB.GuardianNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].GuardianNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}