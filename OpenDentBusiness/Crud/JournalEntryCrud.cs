//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JournalEntryCrud {
		///<summary>Gets one JournalEntry object from the database using the primary key.  Returns null if not found.</summary>
		public static JournalEntry SelectOne(long journalEntryNum){
			string command="SELECT * FROM journalentry "
				+"WHERE JournalEntryNum = "+POut.Long(journalEntryNum);
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JournalEntry object from the database using a query.</summary>
		public static JournalEntry SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JournalEntry objects from the database using a query.</summary>
		public static List<JournalEntry> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JournalEntry> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JournalEntry> TableToList(DataTable table){
			List<JournalEntry> retVal=new List<JournalEntry>();
			JournalEntry journalEntry;
			foreach(DataRow row in table.Rows) {
				journalEntry=new JournalEntry();
				journalEntry.JournalEntryNum= PIn.Long  (row["JournalEntryNum"].ToString());
				journalEntry.TransactionNum = PIn.Long  (row["TransactionNum"].ToString());
				journalEntry.AccountNum     = PIn.Long  (row["AccountNum"].ToString());
				journalEntry.DateDisplayed  = PIn.Date  (row["DateDisplayed"].ToString());
				journalEntry.DebitAmt       = PIn.Double(row["DebitAmt"].ToString());
				journalEntry.CreditAmt      = PIn.Double(row["CreditAmt"].ToString());
				journalEntry.Memo           = PIn.String(row["Memo"].ToString());
				journalEntry.Splits         = PIn.String(row["Splits"].ToString());
				journalEntry.CheckNumber    = PIn.String(row["CheckNumber"].ToString());
				journalEntry.ReconcileNum   = PIn.Long  (row["ReconcileNum"].ToString());
				retVal.Add(journalEntry);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<JournalEntry> listJournalEntrys) {
			DataTable table=new DataTable("JournalEntrys");
			table.Columns.Add("JournalEntryNum");
			table.Columns.Add("TransactionNum");
			table.Columns.Add("AccountNum");
			table.Columns.Add("DateDisplayed");
			table.Columns.Add("DebitAmt");
			table.Columns.Add("CreditAmt");
			table.Columns.Add("Memo");
			table.Columns.Add("Splits");
			table.Columns.Add("CheckNumber");
			table.Columns.Add("ReconcileNum");
			foreach(JournalEntry journalEntry in listJournalEntrys) {
				table.Rows.Add(new object[] {
					POut.Long  (journalEntry.JournalEntryNum),
					POut.Long  (journalEntry.TransactionNum),
					POut.Long  (journalEntry.AccountNum),
					POut.Date  (journalEntry.DateDisplayed),
					POut.Double(journalEntry.DebitAmt),
					POut.Double(journalEntry.CreditAmt),
					POut.String(journalEntry.Memo),
					POut.String(journalEntry.Splits),
					POut.String(journalEntry.CheckNumber),
					POut.Long  (journalEntry.ReconcileNum),
				});
			}
			return table;
		}

		///<summary>Inserts one JournalEntry into the database.  Returns the new priKey.</summary>
		public static long Insert(JournalEntry journalEntry){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				journalEntry.JournalEntryNum=DbHelper.GetNextOracleKey("journalentry","JournalEntryNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(journalEntry,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							journalEntry.JournalEntryNum++;
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
				return Insert(journalEntry,false);
			}
		}

		///<summary>Inserts one JournalEntry into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JournalEntry journalEntry,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				journalEntry.JournalEntryNum=ReplicationServers.GetKey("journalentry","JournalEntryNum");
			}
			string command="INSERT INTO journalentry (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JournalEntryNum,";
			}
			command+="TransactionNum,AccountNum,DateDisplayed,DebitAmt,CreditAmt,Memo,Splits,CheckNumber,ReconcileNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(journalEntry.JournalEntryNum)+",";
			}
			command+=
				     POut.Long  (journalEntry.TransactionNum)+","
				+    POut.Long  (journalEntry.AccountNum)+","
				+    POut.Date  (journalEntry.DateDisplayed)+","
				+"'"+POut.Double(journalEntry.DebitAmt)+"',"
				+"'"+POut.Double(journalEntry.CreditAmt)+"',"
				+"'"+POut.String(journalEntry.Memo)+"',"
				+"'"+POut.String(journalEntry.Splits)+"',"
				+"'"+POut.String(journalEntry.CheckNumber)+"',"
				+    POut.Long  (journalEntry.ReconcileNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				journalEntry.JournalEntryNum=Db.NonQ(command,true);
			}
			return journalEntry.JournalEntryNum;
		}

		///<summary>Inserts one JournalEntry into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JournalEntry journalEntry){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(journalEntry,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					journalEntry.JournalEntryNum=DbHelper.GetNextOracleKey("journalentry","JournalEntryNum"); //Cacheless method
				}
				return InsertNoCache(journalEntry,true);
			}
		}

		///<summary>Inserts one JournalEntry into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JournalEntry journalEntry,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO journalentry (";
			if(!useExistingPK && isRandomKeys) {
				journalEntry.JournalEntryNum=ReplicationServers.GetKeyNoCache("journalentry","JournalEntryNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JournalEntryNum,";
			}
			command+="TransactionNum,AccountNum,DateDisplayed,DebitAmt,CreditAmt,Memo,Splits,CheckNumber,ReconcileNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(journalEntry.JournalEntryNum)+",";
			}
			command+=
				     POut.Long  (journalEntry.TransactionNum)+","
				+    POut.Long  (journalEntry.AccountNum)+","
				+    POut.Date  (journalEntry.DateDisplayed)+","
				+"'"+POut.Double(journalEntry.DebitAmt)+"',"
				+"'"+POut.Double(journalEntry.CreditAmt)+"',"
				+"'"+POut.String(journalEntry.Memo)+"',"
				+"'"+POut.String(journalEntry.Splits)+"',"
				+"'"+POut.String(journalEntry.CheckNumber)+"',"
				+    POut.Long  (journalEntry.ReconcileNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				journalEntry.JournalEntryNum=Db.NonQ(command,true);
			}
			return journalEntry.JournalEntryNum;
		}

		///<summary>Updates one JournalEntry in the database.</summary>
		public static void Update(JournalEntry journalEntry){
			string command="UPDATE journalentry SET "
				+"TransactionNum =  "+POut.Long  (journalEntry.TransactionNum)+", "
				+"AccountNum     =  "+POut.Long  (journalEntry.AccountNum)+", "
				+"DateDisplayed  =  "+POut.Date  (journalEntry.DateDisplayed)+", "
				+"DebitAmt       = '"+POut.Double(journalEntry.DebitAmt)+"', "
				+"CreditAmt      = '"+POut.Double(journalEntry.CreditAmt)+"', "
				+"Memo           = '"+POut.String(journalEntry.Memo)+"', "
				+"Splits         = '"+POut.String(journalEntry.Splits)+"', "
				+"CheckNumber    = '"+POut.String(journalEntry.CheckNumber)+"', "
				+"ReconcileNum   =  "+POut.Long  (journalEntry.ReconcileNum)+" "
				+"WHERE JournalEntryNum = "+POut.Long(journalEntry.JournalEntryNum);
			Db.NonQ(command);
		}

		///<summary>Updates one JournalEntry in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JournalEntry journalEntry,JournalEntry oldJournalEntry){
			string command="";
			if(journalEntry.TransactionNum != oldJournalEntry.TransactionNum) {
				if(command!=""){ command+=",";}
				command+="TransactionNum = "+POut.Long(journalEntry.TransactionNum)+"";
			}
			if(journalEntry.AccountNum != oldJournalEntry.AccountNum) {
				if(command!=""){ command+=",";}
				command+="AccountNum = "+POut.Long(journalEntry.AccountNum)+"";
			}
			if(journalEntry.DateDisplayed.Date != oldJournalEntry.DateDisplayed.Date) {
				if(command!=""){ command+=",";}
				command+="DateDisplayed = "+POut.Date(journalEntry.DateDisplayed)+"";
			}
			if(journalEntry.DebitAmt != oldJournalEntry.DebitAmt) {
				if(command!=""){ command+=",";}
				command+="DebitAmt = '"+POut.Double(journalEntry.DebitAmt)+"'";
			}
			if(journalEntry.CreditAmt != oldJournalEntry.CreditAmt) {
				if(command!=""){ command+=",";}
				command+="CreditAmt = '"+POut.Double(journalEntry.CreditAmt)+"'";
			}
			if(journalEntry.Memo != oldJournalEntry.Memo) {
				if(command!=""){ command+=",";}
				command+="Memo = '"+POut.String(journalEntry.Memo)+"'";
			}
			if(journalEntry.Splits != oldJournalEntry.Splits) {
				if(command!=""){ command+=",";}
				command+="Splits = '"+POut.String(journalEntry.Splits)+"'";
			}
			if(journalEntry.CheckNumber != oldJournalEntry.CheckNumber) {
				if(command!=""){ command+=",";}
				command+="CheckNumber = '"+POut.String(journalEntry.CheckNumber)+"'";
			}
			if(journalEntry.ReconcileNum != oldJournalEntry.ReconcileNum) {
				if(command!=""){ command+=",";}
				command+="ReconcileNum = "+POut.Long(journalEntry.ReconcileNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE journalentry SET "+command
				+" WHERE JournalEntryNum = "+POut.Long(journalEntry.JournalEntryNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one JournalEntry from the database.</summary>
		public static void Delete(long journalEntryNum){
			string command="DELETE FROM journalentry "
				+"WHERE JournalEntryNum = "+POut.Long(journalEntryNum);
			Db.NonQ(command);
		}

	}
}