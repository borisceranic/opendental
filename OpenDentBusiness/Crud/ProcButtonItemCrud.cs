//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcButtonItemCrud {
		///<summary>Gets one ProcButtonItem object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcButtonItem SelectOne(long procButtonItemNum){
			string command="SELECT * FROM procbuttonitem "
				+"WHERE ProcButtonItemNum = "+POut.Long(procButtonItemNum);
			List<ProcButtonItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcButtonItem object from the database using a query.</summary>
		public static ProcButtonItem SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcButtonItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcButtonItem objects from the database using a query.</summary>
		public static List<ProcButtonItem> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcButtonItem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcButtonItem> TableToList(DataTable table){
			List<ProcButtonItem> retVal=new List<ProcButtonItem>();
			ProcButtonItem procButtonItem;
			for(int i=0;i<table.Rows.Count;i++) {
				procButtonItem=new ProcButtonItem();
				procButtonItem.ProcButtonItemNum= PIn.Long  (table.Rows[i]["ProcButtonItemNum"].ToString());
				procButtonItem.ProcButtonNum    = PIn.Long  (table.Rows[i]["ProcButtonNum"].ToString());
				procButtonItem.OldCode          = PIn.String(table.Rows[i]["OldCode"].ToString());
				procButtonItem.AutoCodeNum      = PIn.Long  (table.Rows[i]["AutoCodeNum"].ToString());
				procButtonItem.CodeNum          = PIn.Long  (table.Rows[i]["CodeNum"].ToString());
				retVal.Add(procButtonItem);
			}
			return retVal;
		}

		///<summary>Inserts one ProcButtonItem into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcButtonItem procButtonItem){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				procButtonItem.ProcButtonItemNum=DbHelper.GetNextOracleKey("procbuttonitem","ProcButtonItemNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(procButtonItem,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							procButtonItem.ProcButtonItemNum++;
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
				return Insert(procButtonItem,false);
			}
		}

		///<summary>Inserts one ProcButtonItem into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcButtonItem procButtonItem,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procButtonItem.ProcButtonItemNum=ReplicationServers.GetKey("procbuttonitem","ProcButtonItemNum");
			}
			string command="INSERT INTO procbuttonitem (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcButtonItemNum,";
			}
			command+="ProcButtonNum,OldCode,AutoCodeNum,CodeNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procButtonItem.ProcButtonItemNum)+",";
			}
			command+=
				     POut.Long  (procButtonItem.ProcButtonNum)+","
				+"'"+POut.String(procButtonItem.OldCode)+"',"
				+    POut.Long  (procButtonItem.AutoCodeNum)+","
				+    POut.Long  (procButtonItem.CodeNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procButtonItem.ProcButtonItemNum=Db.NonQ(command,true);
			}
			return procButtonItem.ProcButtonItemNum;
		}

		///<summary>Inserts one ProcButtonItem into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcButtonItem procButtonItem){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(procButtonItem,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					procButtonItem.ProcButtonItemNum=DbHelper.GetNextOracleKey("procbuttonitem","ProcButtonItemNum"); //Cacheless method
				}
				return InsertNoCache(procButtonItem,true);
			}
		}

		///<summary>Inserts one ProcButtonItem into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcButtonItem procButtonItem,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO procbuttonitem (";
			if(!useExistingPK && isRandomKeys) {
				procButtonItem.ProcButtonItemNum=ReplicationServers.GetKeyNoCache("procbuttonitem","ProcButtonItemNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcButtonItemNum,";
			}
			command+="ProcButtonNum,OldCode,AutoCodeNum,CodeNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procButtonItem.ProcButtonItemNum)+",";
			}
			command+=
				     POut.Long  (procButtonItem.ProcButtonNum)+","
				+"'"+POut.String(procButtonItem.OldCode)+"',"
				+    POut.Long  (procButtonItem.AutoCodeNum)+","
				+    POut.Long  (procButtonItem.CodeNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				procButtonItem.ProcButtonItemNum=Db.NonQ(command,true);
			}
			return procButtonItem.ProcButtonItemNum;
		}

		///<summary>Updates one ProcButtonItem in the database.</summary>
		public static void Update(ProcButtonItem procButtonItem){
			string command="UPDATE procbuttonitem SET "
				+"ProcButtonNum    =  "+POut.Long  (procButtonItem.ProcButtonNum)+", "
				+"OldCode          = '"+POut.String(procButtonItem.OldCode)+"', "
				+"AutoCodeNum      =  "+POut.Long  (procButtonItem.AutoCodeNum)+", "
				+"CodeNum          =  "+POut.Long  (procButtonItem.CodeNum)+" "
				+"WHERE ProcButtonItemNum = "+POut.Long(procButtonItem.ProcButtonItemNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcButtonItem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcButtonItem procButtonItem,ProcButtonItem oldProcButtonItem){
			string command="";
			if(procButtonItem.ProcButtonNum != oldProcButtonItem.ProcButtonNum) {
				if(command!=""){ command+=",";}
				command+="ProcButtonNum = "+POut.Long(procButtonItem.ProcButtonNum)+"";
			}
			if(procButtonItem.OldCode != oldProcButtonItem.OldCode) {
				if(command!=""){ command+=",";}
				command+="OldCode = '"+POut.String(procButtonItem.OldCode)+"'";
			}
			if(procButtonItem.AutoCodeNum != oldProcButtonItem.AutoCodeNum) {
				if(command!=""){ command+=",";}
				command+="AutoCodeNum = "+POut.Long(procButtonItem.AutoCodeNum)+"";
			}
			if(procButtonItem.CodeNum != oldProcButtonItem.CodeNum) {
				if(command!=""){ command+=",";}
				command+="CodeNum = "+POut.Long(procButtonItem.CodeNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE procbuttonitem SET "+command
				+" WHERE ProcButtonItemNum = "+POut.Long(procButtonItem.ProcButtonItemNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ProcButtonItem from the database.</summary>
		public static void Delete(long procButtonItemNum){
			string command="DELETE FROM procbuttonitem "
				+"WHERE ProcButtonItemNum = "+POut.Long(procButtonItemNum);
			Db.NonQ(command);
		}

	}
}