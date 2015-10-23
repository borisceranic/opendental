//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ConnectionGroupCrud {
		///<summary>Gets one ConnectionGroup object from the database using the primary key.  Returns null if not found.</summary>
		public static ConnectionGroup SelectOne(long connectionGroupNum){
			string command="SELECT * FROM connectiongroup "
				+"WHERE ConnectionGroupNum = "+POut.Long(connectionGroupNum);
			List<ConnectionGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ConnectionGroup object from the database using a query.</summary>
		public static ConnectionGroup SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ConnectionGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ConnectionGroup objects from the database using a query.</summary>
		public static List<ConnectionGroup> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ConnectionGroup> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ConnectionGroup> TableToList(DataTable table){
			List<ConnectionGroup> retVal=new List<ConnectionGroup>();
			ConnectionGroup connectionGroup;
			for(int i=0;i<table.Rows.Count;i++) {
				connectionGroup=new ConnectionGroup();
				connectionGroup.ConnectionGroupNum= PIn.Long  (table.Rows[i]["ConnectionGroupNum"].ToString());
				connectionGroup.Description       = PIn.String(table.Rows[i]["Description"].ToString());
				retVal.Add(connectionGroup);
			}
			return retVal;
		}

		///<summary>Inserts one ConnectionGroup into the database.  Returns the new priKey.</summary>
		public static long Insert(ConnectionGroup connectionGroup){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				connectionGroup.ConnectionGroupNum=DbHelper.GetNextOracleKey("connectiongroup","ConnectionGroupNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(connectionGroup,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							connectionGroup.ConnectionGroupNum++;
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
				return Insert(connectionGroup,false);
			}
		}

		///<summary>Inserts one ConnectionGroup into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ConnectionGroup connectionGroup,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				connectionGroup.ConnectionGroupNum=ReplicationServers.GetKey("connectiongroup","ConnectionGroupNum");
			}
			string command="INSERT INTO connectiongroup (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ConnectionGroupNum,";
			}
			command+="Description) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(connectionGroup.ConnectionGroupNum)+",";
			}
			command+=
				 "'"+POut.String(connectionGroup.Description)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				connectionGroup.ConnectionGroupNum=Db.NonQ(command,true);
			}
			return connectionGroup.ConnectionGroupNum;
		}

		///<summary>Inserts one ConnectionGroup into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ConnectionGroup connectionGroup){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(connectionGroup,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					connectionGroup.ConnectionGroupNum=DbHelper.GetNextOracleKey("connectiongroup","ConnectionGroupNum"); //Cacheless method
				}
				return InsertNoCache(connectionGroup,true);
			}
		}

		///<summary>Inserts one ConnectionGroup into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ConnectionGroup connectionGroup,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO connectiongroup (";
			if(!useExistingPK && isRandomKeys) {
				connectionGroup.ConnectionGroupNum=ReplicationServers.GetKeyNoCache("connectiongroup","ConnectionGroupNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ConnectionGroupNum,";
			}
			command+="Description) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(connectionGroup.ConnectionGroupNum)+",";
			}
			command+=
				 "'"+POut.String(connectionGroup.Description)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				connectionGroup.ConnectionGroupNum=Db.NonQ(command,true);
			}
			return connectionGroup.ConnectionGroupNum;
		}

		///<summary>Updates one ConnectionGroup in the database.</summary>
		public static void Update(ConnectionGroup connectionGroup){
			string command="UPDATE connectiongroup SET "
				+"Description       = '"+POut.String(connectionGroup.Description)+"' "
				+"WHERE ConnectionGroupNum = "+POut.Long(connectionGroup.ConnectionGroupNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ConnectionGroup in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ConnectionGroup connectionGroup,ConnectionGroup oldConnectionGroup){
			string command="";
			if(connectionGroup.Description != oldConnectionGroup.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(connectionGroup.Description)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE connectiongroup SET "+command
				+" WHERE ConnectionGroupNum = "+POut.Long(connectionGroup.ConnectionGroupNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ConnectionGroup from the database.</summary>
		public static void Delete(long connectionGroupNum){
			string command="DELETE FROM connectiongroup "
				+"WHERE ConnectionGroupNum = "+POut.Long(connectionGroupNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ConnectionGroup> listNew,List<ConnectionGroup> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ConnectionGroup> listIns    =new List<ConnectionGroup>();
			List<ConnectionGroup> listUpdNew =new List<ConnectionGroup>();
			List<ConnectionGroup> listUpdDB  =new List<ConnectionGroup>();
			List<ConnectionGroup> listDel    =new List<ConnectionGroup>();
			listNew.Sort((ConnectionGroup x,ConnectionGroup y) => { return x.ConnectionGroupNum.CompareTo(y.ConnectionGroupNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ConnectionGroup x,ConnectionGroup y) => { return x.ConnectionGroupNum.CompareTo(y.ConnectionGroupNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ConnectionGroup fieldNew;
			ConnectionGroup fieldDB;
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
				else if(fieldNew.ConnectionGroupNum<fieldDB.ConnectionGroupNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ConnectionGroupNum>fieldDB.ConnectionGroupNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ConnectionGroupNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}