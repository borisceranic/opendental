//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatFieldDefCrud {
		///<summary>Gets one PatFieldDef object from the database using the primary key.  Returns null if not found.</summary>
		public static PatFieldDef SelectOne(long patFieldDefNum){
			string command="SELECT * FROM patfielddef "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDefNum);
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatFieldDef object from the database using a query.</summary>
		public static PatFieldDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatFieldDef objects from the database using a query.</summary>
		public static List<PatFieldDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatFieldDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatFieldDef> TableToList(DataTable table){
			List<PatFieldDef> retVal=new List<PatFieldDef>();
			PatFieldDef patFieldDef;
			foreach(DataRow row in table.Rows) {
				patFieldDef=new PatFieldDef();
				patFieldDef.PatFieldDefNum= PIn.Long  (row["PatFieldDefNum"].ToString());
				patFieldDef.FieldName     = PIn.String(row["FieldName"].ToString());
				patFieldDef.FieldType     = (OpenDentBusiness.PatFieldType)PIn.Int(row["FieldType"].ToString());
				patFieldDef.PickList      = PIn.String(row["PickList"].ToString());
				patFieldDef.ItemOrder     = PIn.Int   (row["ItemOrder"].ToString());
				patFieldDef.IsHidden      = PIn.Bool  (row["IsHidden"].ToString());
				retVal.Add(patFieldDef);
			}
			return retVal;
		}

		///<summary>Inserts one PatFieldDef into the database.  Returns the new priKey.</summary>
		public static long Insert(PatFieldDef patFieldDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				patFieldDef.PatFieldDefNum=DbHelper.GetNextOracleKey("patfielddef","PatFieldDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(patFieldDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							patFieldDef.PatFieldDefNum++;
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
				return Insert(patFieldDef,false);
			}
		}

		///<summary>Inserts one PatFieldDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatFieldDef patFieldDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				patFieldDef.PatFieldDefNum=ReplicationServers.GetKey("patfielddef","PatFieldDefNum");
			}
			string command="INSERT INTO patfielddef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatFieldDefNum,";
			}
			command+="FieldName,FieldType,PickList,ItemOrder,IsHidden) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patFieldDef.PatFieldDefNum)+",";
			}
			command+=
				 "'"+POut.String(patFieldDef.FieldName)+"',"
				+    POut.Int   ((int)patFieldDef.FieldType)+","
				+"'"+POut.String(patFieldDef.PickList)+"',"
				+    POut.Int   (patFieldDef.ItemOrder)+","
				+    POut.Bool  (patFieldDef.IsHidden)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patFieldDef.PatFieldDefNum=Db.NonQ(command,true);
			}
			return patFieldDef.PatFieldDefNum;
		}

		///<summary>Inserts one PatFieldDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatFieldDef patFieldDef){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(patFieldDef,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					patFieldDef.PatFieldDefNum=DbHelper.GetNextOracleKey("patfielddef","PatFieldDefNum"); //Cacheless method
				}
				return InsertNoCache(patFieldDef,true);
			}
		}

		///<summary>Inserts one PatFieldDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatFieldDef patFieldDef,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO patfielddef (";
			if(!useExistingPK && isRandomKeys) {
				patFieldDef.PatFieldDefNum=ReplicationServers.GetKeyNoCache("patfielddef","PatFieldDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatFieldDefNum,";
			}
			command+="FieldName,FieldType,PickList,ItemOrder,IsHidden) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patFieldDef.PatFieldDefNum)+",";
			}
			command+=
				 "'"+POut.String(patFieldDef.FieldName)+"',"
				+    POut.Int   ((int)patFieldDef.FieldType)+","
				+"'"+POut.String(patFieldDef.PickList)+"',"
				+    POut.Int   (patFieldDef.ItemOrder)+","
				+    POut.Bool  (patFieldDef.IsHidden)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				patFieldDef.PatFieldDefNum=Db.NonQ(command,true);
			}
			return patFieldDef.PatFieldDefNum;
		}

		///<summary>Updates one PatFieldDef in the database.</summary>
		public static void Update(PatFieldDef patFieldDef){
			string command="UPDATE patfielddef SET "
				+"FieldName     = '"+POut.String(patFieldDef.FieldName)+"', "
				+"FieldType     =  "+POut.Int   ((int)patFieldDef.FieldType)+", "
				+"PickList      = '"+POut.String(patFieldDef.PickList)+"', "
				+"ItemOrder     =  "+POut.Int   (patFieldDef.ItemOrder)+", "
				+"IsHidden      =  "+POut.Bool  (patFieldDef.IsHidden)+" "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDef.PatFieldDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PatFieldDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatFieldDef patFieldDef,PatFieldDef oldPatFieldDef){
			string command="";
			if(patFieldDef.FieldName != oldPatFieldDef.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(patFieldDef.FieldName)+"'";
			}
			if(patFieldDef.FieldType != oldPatFieldDef.FieldType) {
				if(command!=""){ command+=",";}
				command+="FieldType = "+POut.Int   ((int)patFieldDef.FieldType)+"";
			}
			if(patFieldDef.PickList != oldPatFieldDef.PickList) {
				if(command!=""){ command+=",";}
				command+="PickList = '"+POut.String(patFieldDef.PickList)+"'";
			}
			if(patFieldDef.ItemOrder != oldPatFieldDef.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(patFieldDef.ItemOrder)+"";
			}
			if(patFieldDef.IsHidden != oldPatFieldDef.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(patFieldDef.IsHidden)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE patfielddef SET "+command
				+" WHERE PatFieldDefNum = "+POut.Long(patFieldDef.PatFieldDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one PatFieldDef from the database.</summary>
		public static void Delete(long patFieldDefNum){
			string command="DELETE FROM patfielddef "
				+"WHERE PatFieldDefNum = "+POut.Long(patFieldDefNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<PatFieldDef> listNew,List<PatFieldDef> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<PatFieldDef> listIns    =new List<PatFieldDef>();
			List<PatFieldDef> listUpdNew =new List<PatFieldDef>();
			List<PatFieldDef> listUpdDB  =new List<PatFieldDef>();
			List<PatFieldDef> listDel    =new List<PatFieldDef>();
			listNew.Sort((PatFieldDef x,PatFieldDef y) => { return x.PatFieldDefNum.CompareTo(y.PatFieldDefNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((PatFieldDef x,PatFieldDef y) => { return x.PatFieldDefNum.CompareTo(y.PatFieldDefNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			PatFieldDef fieldNew;
			PatFieldDef fieldDB;
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
				else if(fieldNew.PatFieldDefNum<fieldDB.PatFieldDefNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.PatFieldDefNum>fieldDB.PatFieldDefNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].PatFieldDefNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}