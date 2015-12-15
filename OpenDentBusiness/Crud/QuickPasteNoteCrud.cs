//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class QuickPasteNoteCrud {
		///<summary>Gets one QuickPasteNote object from the database using the primary key.  Returns null if not found.</summary>
		public static QuickPasteNote SelectOne(long quickPasteNoteNum){
			string command="SELECT * FROM quickpastenote "
				+"WHERE QuickPasteNoteNum = "+POut.Long(quickPasteNoteNum);
			List<QuickPasteNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one QuickPasteNote object from the database using a query.</summary>
		public static QuickPasteNote SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<QuickPasteNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of QuickPasteNote objects from the database using a query.</summary>
		public static List<QuickPasteNote> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<QuickPasteNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<QuickPasteNote> TableToList(DataTable table){
			List<QuickPasteNote> retVal=new List<QuickPasteNote>();
			QuickPasteNote quickPasteNote;
			foreach(DataRow row in table.Rows) {
				quickPasteNote=new QuickPasteNote();
				quickPasteNote.QuickPasteNoteNum= PIn.Long  (row["QuickPasteNoteNum"].ToString());
				quickPasteNote.QuickPasteCatNum = PIn.Long  (row["QuickPasteCatNum"].ToString());
				quickPasteNote.ItemOrder        = PIn.Int   (row["ItemOrder"].ToString());
				quickPasteNote.Note             = PIn.String(row["Note"].ToString());
				quickPasteNote.Abbreviation     = PIn.String(row["Abbreviation"].ToString());
				retVal.Add(quickPasteNote);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<QuickPasteNote> listQuickPasteNotes) {
			DataTable table=new DataTable("QuickPasteNotes");
			table.Columns.Add("QuickPasteNoteNum");
			table.Columns.Add("QuickPasteCatNum");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("Note");
			table.Columns.Add("Abbreviation");
			foreach(QuickPasteNote quickPasteNote in listQuickPasteNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (quickPasteNote.QuickPasteNoteNum),
					POut.Long  (quickPasteNote.QuickPasteCatNum),
					POut.Int   (quickPasteNote.ItemOrder),
					POut.String(quickPasteNote.Note),
					POut.String(quickPasteNote.Abbreviation),
				});
			}
			return table;
		}

		///<summary>Inserts one QuickPasteNote into the database.  Returns the new priKey.</summary>
		public static long Insert(QuickPasteNote quickPasteNote){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				quickPasteNote.QuickPasteNoteNum=DbHelper.GetNextOracleKey("quickpastenote","QuickPasteNoteNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(quickPasteNote,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							quickPasteNote.QuickPasteNoteNum++;
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
				return Insert(quickPasteNote,false);
			}
		}

		///<summary>Inserts one QuickPasteNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(QuickPasteNote quickPasteNote,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				quickPasteNote.QuickPasteNoteNum=ReplicationServers.GetKey("quickpastenote","QuickPasteNoteNum");
			}
			string command="INSERT INTO quickpastenote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="QuickPasteNoteNum,";
			}
			command+="QuickPasteCatNum,ItemOrder,Note,Abbreviation) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(quickPasteNote.QuickPasteNoteNum)+",";
			}
			command+=
				     POut.Long  (quickPasteNote.QuickPasteCatNum)+","
				+    POut.Int   (quickPasteNote.ItemOrder)+","
				+"'"+POut.String(quickPasteNote.Note)+"',"
				+"'"+POut.String(quickPasteNote.Abbreviation)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				quickPasteNote.QuickPasteNoteNum=Db.NonQ(command,true);
			}
			return quickPasteNote.QuickPasteNoteNum;
		}

		///<summary>Inserts one QuickPasteNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(QuickPasteNote quickPasteNote){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(quickPasteNote,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					quickPasteNote.QuickPasteNoteNum=DbHelper.GetNextOracleKey("quickpastenote","QuickPasteNoteNum"); //Cacheless method
				}
				return InsertNoCache(quickPasteNote,true);
			}
		}

		///<summary>Inserts one QuickPasteNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(QuickPasteNote quickPasteNote,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO quickpastenote (";
			if(!useExistingPK && isRandomKeys) {
				quickPasteNote.QuickPasteNoteNum=ReplicationServers.GetKeyNoCache("quickpastenote","QuickPasteNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="QuickPasteNoteNum,";
			}
			command+="QuickPasteCatNum,ItemOrder,Note,Abbreviation) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(quickPasteNote.QuickPasteNoteNum)+",";
			}
			command+=
				     POut.Long  (quickPasteNote.QuickPasteCatNum)+","
				+    POut.Int   (quickPasteNote.ItemOrder)+","
				+"'"+POut.String(quickPasteNote.Note)+"',"
				+"'"+POut.String(quickPasteNote.Abbreviation)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				quickPasteNote.QuickPasteNoteNum=Db.NonQ(command,true);
			}
			return quickPasteNote.QuickPasteNoteNum;
		}

		///<summary>Updates one QuickPasteNote in the database.</summary>
		public static void Update(QuickPasteNote quickPasteNote){
			string command="UPDATE quickpastenote SET "
				+"QuickPasteCatNum =  "+POut.Long  (quickPasteNote.QuickPasteCatNum)+", "
				+"ItemOrder        =  "+POut.Int   (quickPasteNote.ItemOrder)+", "
				+"Note             = '"+POut.String(quickPasteNote.Note)+"', "
				+"Abbreviation     = '"+POut.String(quickPasteNote.Abbreviation)+"' "
				+"WHERE QuickPasteNoteNum = "+POut.Long(quickPasteNote.QuickPasteNoteNum);
			Db.NonQ(command);
		}

		///<summary>Updates one QuickPasteNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(QuickPasteNote quickPasteNote,QuickPasteNote oldQuickPasteNote){
			string command="";
			if(quickPasteNote.QuickPasteCatNum != oldQuickPasteNote.QuickPasteCatNum) {
				if(command!=""){ command+=",";}
				command+="QuickPasteCatNum = "+POut.Long(quickPasteNote.QuickPasteCatNum)+"";
			}
			if(quickPasteNote.ItemOrder != oldQuickPasteNote.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(quickPasteNote.ItemOrder)+"";
			}
			if(quickPasteNote.Note != oldQuickPasteNote.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(quickPasteNote.Note)+"'";
			}
			if(quickPasteNote.Abbreviation != oldQuickPasteNote.Abbreviation) {
				if(command!=""){ command+=",";}
				command+="Abbreviation = '"+POut.String(quickPasteNote.Abbreviation)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE quickpastenote SET "+command
				+" WHERE QuickPasteNoteNum = "+POut.Long(quickPasteNote.QuickPasteNoteNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one QuickPasteNote from the database.</summary>
		public static void Delete(long quickPasteNoteNum){
			string command="DELETE FROM quickpastenote "
				+"WHERE QuickPasteNoteNum = "+POut.Long(quickPasteNoteNum);
			Db.NonQ(command);
		}

	}
}