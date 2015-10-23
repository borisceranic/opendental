//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ConnGroupAttachCrud {
		///<summary>Gets one ConnGroupAttach object from the database using the primary key.  Returns null if not found.</summary>
		public static ConnGroupAttach SelectOne(long connGroupAttachNum){
			string command="SELECT * FROM conngroupattach "
				+"WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttachNum);
			List<ConnGroupAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ConnGroupAttach object from the database using a query.</summary>
		public static ConnGroupAttach SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ConnGroupAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ConnGroupAttach objects from the database using a query.</summary>
		public static List<ConnGroupAttach> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ConnGroupAttach> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ConnGroupAttach> TableToList(DataTable table){
			List<ConnGroupAttach> retVal=new List<ConnGroupAttach>();
			ConnGroupAttach connGroupAttach;
			for(int i=0;i<table.Rows.Count;i++) {
				connGroupAttach=new ConnGroupAttach();
				connGroupAttach.ConnGroupAttachNum  = PIn.Long  (table.Rows[i]["ConnGroupAttachNum"].ToString());
				connGroupAttach.ConnectionGroupNum  = PIn.Long  (table.Rows[i]["ConnectionGroupNum"].ToString());
				connGroupAttach.CentralConnectionNum= PIn.Long  (table.Rows[i]["CentralConnectionNum"].ToString());
				retVal.Add(connGroupAttach);
			}
			return retVal;
		}

		///<summary>Inserts one ConnGroupAttach into the database.  Returns the new priKey.</summary>
		public static long Insert(ConnGroupAttach connGroupAttach){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				connGroupAttach.ConnGroupAttachNum=DbHelper.GetNextOracleKey("conngroupattach","ConnGroupAttachNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(connGroupAttach,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							connGroupAttach.ConnGroupAttachNum++;
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
				return Insert(connGroupAttach,false);
			}
		}

		///<summary>Inserts one ConnGroupAttach into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ConnGroupAttach connGroupAttach,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				connGroupAttach.ConnGroupAttachNum=ReplicationServers.GetKey("conngroupattach","ConnGroupAttachNum");
			}
			string command="INSERT INTO conngroupattach (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ConnGroupAttachNum,";
			}
			command+="ConnectionGroupNum,CentralConnectionNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(connGroupAttach.ConnGroupAttachNum)+",";
			}
			command+=
				     POut.Long  (connGroupAttach.ConnectionGroupNum)+","
				+    POut.Long  (connGroupAttach.CentralConnectionNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				connGroupAttach.ConnGroupAttachNum=Db.NonQ(command,true);
			}
			return connGroupAttach.ConnGroupAttachNum;
		}

		///<summary>Inserts one ConnGroupAttach into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ConnGroupAttach connGroupAttach){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(connGroupAttach,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					connGroupAttach.ConnGroupAttachNum=DbHelper.GetNextOracleKey("conngroupattach","ConnGroupAttachNum"); //Cacheless method
				}
				return InsertNoCache(connGroupAttach,true);
			}
		}

		///<summary>Inserts one ConnGroupAttach into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ConnGroupAttach connGroupAttach,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO conngroupattach (";
			if(!useExistingPK && isRandomKeys) {
				connGroupAttach.ConnGroupAttachNum=ReplicationServers.GetKeyNoCache("conngroupattach","ConnGroupAttachNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ConnGroupAttachNum,";
			}
			command+="ConnectionGroupNum,CentralConnectionNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(connGroupAttach.ConnGroupAttachNum)+",";
			}
			command+=
				     POut.Long  (connGroupAttach.ConnectionGroupNum)+","
				+    POut.Long  (connGroupAttach.CentralConnectionNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				connGroupAttach.ConnGroupAttachNum=Db.NonQ(command,true);
			}
			return connGroupAttach.ConnGroupAttachNum;
		}

		///<summary>Updates one ConnGroupAttach in the database.</summary>
		public static void Update(ConnGroupAttach connGroupAttach){
			string command="UPDATE conngroupattach SET "
				+"ConnectionGroupNum  =  "+POut.Long  (connGroupAttach.ConnectionGroupNum)+", "
				+"CentralConnectionNum=  "+POut.Long  (connGroupAttach.CentralConnectionNum)+" "
				+"WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttach.ConnGroupAttachNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ConnGroupAttach in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ConnGroupAttach connGroupAttach,ConnGroupAttach oldConnGroupAttach){
			string command="";
			if(connGroupAttach.ConnectionGroupNum != oldConnGroupAttach.ConnectionGroupNum) {
				if(command!=""){ command+=",";}
				command+="ConnectionGroupNum = "+POut.Long(connGroupAttach.ConnectionGroupNum)+"";
			}
			if(connGroupAttach.CentralConnectionNum != oldConnGroupAttach.CentralConnectionNum) {
				if(command!=""){ command+=",";}
				command+="CentralConnectionNum = "+POut.Long(connGroupAttach.CentralConnectionNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE conngroupattach SET "+command
				+" WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttach.ConnGroupAttachNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ConnGroupAttach from the database.</summary>
		public static void Delete(long connGroupAttachNum){
			string command="DELETE FROM conngroupattach "
				+"WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttachNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ConnGroupAttach> listNew,List<ConnGroupAttach> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ConnGroupAttach> listIns    =new List<ConnGroupAttach>();
			List<ConnGroupAttach> listUpdNew =new List<ConnGroupAttach>();
			List<ConnGroupAttach> listUpdDB  =new List<ConnGroupAttach>();
			List<ConnGroupAttach> listDel    =new List<ConnGroupAttach>();
			listNew.Sort((ConnGroupAttach x,ConnGroupAttach y) => { return x.ConnGroupAttachNum.CompareTo(y.ConnGroupAttachNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ConnGroupAttach x,ConnGroupAttach y) => { return x.ConnGroupAttachNum.CompareTo(y.ConnGroupAttachNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ConnGroupAttach fieldNew;
			ConnGroupAttach fieldDB;
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
				else if(fieldNew.ConnGroupAttachNum<fieldDB.ConnGroupAttachNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ConnGroupAttachNum>fieldDB.ConnGroupAttachNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ConnGroupAttachNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}