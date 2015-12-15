//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ReqNeededCrud {
		///<summary>Gets one ReqNeeded object from the database using the primary key.  Returns null if not found.</summary>
		public static ReqNeeded SelectOne(long reqNeededNum){
			string command="SELECT * FROM reqneeded "
				+"WHERE ReqNeededNum = "+POut.Long(reqNeededNum);
			List<ReqNeeded> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ReqNeeded object from the database using a query.</summary>
		public static ReqNeeded SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ReqNeeded> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ReqNeeded objects from the database using a query.</summary>
		public static List<ReqNeeded> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ReqNeeded> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ReqNeeded> TableToList(DataTable table){
			List<ReqNeeded> retVal=new List<ReqNeeded>();
			ReqNeeded reqNeeded;
			foreach(DataRow row in table.Rows) {
				reqNeeded=new ReqNeeded();
				reqNeeded.ReqNeededNum   = PIn.Long  (row["ReqNeededNum"].ToString());
				reqNeeded.Descript       = PIn.String(row["Descript"].ToString());
				reqNeeded.SchoolCourseNum= PIn.Long  (row["SchoolCourseNum"].ToString());
				reqNeeded.SchoolClassNum = PIn.Long  (row["SchoolClassNum"].ToString());
				retVal.Add(reqNeeded);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ReqNeeded> listReqNeededs) {
			DataTable table=new DataTable("ReqNeededs");
			table.Columns.Add("ReqNeededNum");
			table.Columns.Add("Descript");
			table.Columns.Add("SchoolCourseNum");
			table.Columns.Add("SchoolClassNum");
			foreach(ReqNeeded reqNeeded in listReqNeededs) {
				table.Rows.Add(new object[] {
					POut.Long  (reqNeeded.ReqNeededNum),
					POut.String(reqNeeded.Descript),
					POut.Long  (reqNeeded.SchoolCourseNum),
					POut.Long  (reqNeeded.SchoolClassNum),
				});
			}
			return table;
		}

		///<summary>Inserts one ReqNeeded into the database.  Returns the new priKey.</summary>
		public static long Insert(ReqNeeded reqNeeded){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				reqNeeded.ReqNeededNum=DbHelper.GetNextOracleKey("reqneeded","ReqNeededNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(reqNeeded,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							reqNeeded.ReqNeededNum++;
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
				return Insert(reqNeeded,false);
			}
		}

		///<summary>Inserts one ReqNeeded into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ReqNeeded reqNeeded,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				reqNeeded.ReqNeededNum=ReplicationServers.GetKey("reqneeded","ReqNeededNum");
			}
			string command="INSERT INTO reqneeded (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ReqNeededNum,";
			}
			command+="Descript,SchoolCourseNum,SchoolClassNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(reqNeeded.ReqNeededNum)+",";
			}
			command+=
				 "'"+POut.String(reqNeeded.Descript)+"',"
				+    POut.Long  (reqNeeded.SchoolCourseNum)+","
				+    POut.Long  (reqNeeded.SchoolClassNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				reqNeeded.ReqNeededNum=Db.NonQ(command,true);
			}
			return reqNeeded.ReqNeededNum;
		}

		///<summary>Inserts one ReqNeeded into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReqNeeded reqNeeded){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(reqNeeded,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					reqNeeded.ReqNeededNum=DbHelper.GetNextOracleKey("reqneeded","ReqNeededNum"); //Cacheless method
				}
				return InsertNoCache(reqNeeded,true);
			}
		}

		///<summary>Inserts one ReqNeeded into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReqNeeded reqNeeded,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO reqneeded (";
			if(!useExistingPK && isRandomKeys) {
				reqNeeded.ReqNeededNum=ReplicationServers.GetKeyNoCache("reqneeded","ReqNeededNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ReqNeededNum,";
			}
			command+="Descript,SchoolCourseNum,SchoolClassNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(reqNeeded.ReqNeededNum)+",";
			}
			command+=
				 "'"+POut.String(reqNeeded.Descript)+"',"
				+    POut.Long  (reqNeeded.SchoolCourseNum)+","
				+    POut.Long  (reqNeeded.SchoolClassNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				reqNeeded.ReqNeededNum=Db.NonQ(command,true);
			}
			return reqNeeded.ReqNeededNum;
		}

		///<summary>Updates one ReqNeeded in the database.</summary>
		public static void Update(ReqNeeded reqNeeded){
			string command="UPDATE reqneeded SET "
				+"Descript       = '"+POut.String(reqNeeded.Descript)+"', "
				+"SchoolCourseNum=  "+POut.Long  (reqNeeded.SchoolCourseNum)+", "
				+"SchoolClassNum =  "+POut.Long  (reqNeeded.SchoolClassNum)+" "
				+"WHERE ReqNeededNum = "+POut.Long(reqNeeded.ReqNeededNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ReqNeeded in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ReqNeeded reqNeeded,ReqNeeded oldReqNeeded){
			string command="";
			if(reqNeeded.Descript != oldReqNeeded.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(reqNeeded.Descript)+"'";
			}
			if(reqNeeded.SchoolCourseNum != oldReqNeeded.SchoolCourseNum) {
				if(command!=""){ command+=",";}
				command+="SchoolCourseNum = "+POut.Long(reqNeeded.SchoolCourseNum)+"";
			}
			if(reqNeeded.SchoolClassNum != oldReqNeeded.SchoolClassNum) {
				if(command!=""){ command+=",";}
				command+="SchoolClassNum = "+POut.Long(reqNeeded.SchoolClassNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE reqneeded SET "+command
				+" WHERE ReqNeededNum = "+POut.Long(reqNeeded.ReqNeededNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ReqNeeded from the database.</summary>
		public static void Delete(long reqNeededNum){
			string command="DELETE FROM reqneeded "
				+"WHERE ReqNeededNum = "+POut.Long(reqNeededNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ReqNeeded> listNew,List<ReqNeeded> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ReqNeeded> listIns    =new List<ReqNeeded>();
			List<ReqNeeded> listUpdNew =new List<ReqNeeded>();
			List<ReqNeeded> listUpdDB  =new List<ReqNeeded>();
			List<ReqNeeded> listDel    =new List<ReqNeeded>();
			listNew.Sort((ReqNeeded x,ReqNeeded y) => { return x.ReqNeededNum.CompareTo(y.ReqNeededNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((ReqNeeded x,ReqNeeded y) => { return x.ReqNeededNum.CompareTo(y.ReqNeededNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ReqNeeded fieldNew;
			ReqNeeded fieldDB;
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
				else if(fieldNew.ReqNeededNum<fieldDB.ReqNeededNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ReqNeededNum>fieldDB.ReqNeededNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ReqNeededNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}