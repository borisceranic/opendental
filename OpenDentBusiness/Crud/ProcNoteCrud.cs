//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcNoteCrud {
		///<summary>Gets one ProcNote object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcNote SelectOne(long procNoteNum){
			string command="SELECT * FROM procnote "
				+"WHERE ProcNoteNum = "+POut.Long(procNoteNum);
			List<ProcNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcNote object from the database using a query.</summary>
		public static ProcNote SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcNote objects from the database using a query.</summary>
		public static List<ProcNote> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcNote> TableToList(DataTable table){
			List<ProcNote> retVal=new List<ProcNote>();
			ProcNote procNote;
			foreach(DataRow row in table.Rows) {
				procNote=new ProcNote();
				procNote.ProcNoteNum  = PIn.Long  (row["ProcNoteNum"].ToString());
				procNote.PatNum       = PIn.Long  (row["PatNum"].ToString());
				procNote.ProcNum      = PIn.Long  (row["ProcNum"].ToString());
				procNote.EntryDateTime= PIn.DateT (row["EntryDateTime"].ToString());
				procNote.UserNum      = PIn.Long  (row["UserNum"].ToString());
				procNote.Note         = PIn.String(row["Note"].ToString());
				procNote.SigIsTopaz   = PIn.Bool  (row["SigIsTopaz"].ToString());
				procNote.Signature    = PIn.String(row["Signature"].ToString());
				retVal.Add(procNote);
			}
			return retVal;
		}

		///<summary>Converts a list of ProcNote into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcNote> listProcNotes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ProcNote";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ProcNoteNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ProcNum");
			table.Columns.Add("EntryDateTime");
			table.Columns.Add("UserNum");
			table.Columns.Add("Note");
			table.Columns.Add("SigIsTopaz");
			table.Columns.Add("Signature");
			foreach(ProcNote procNote in listProcNotes) {
				table.Rows.Add(new object[] {
					POut.Long  (procNote.ProcNoteNum),
					POut.Long  (procNote.PatNum),
					POut.Long  (procNote.ProcNum),
					POut.DateT (procNote.EntryDateTime,false),
					POut.Long  (procNote.UserNum),
					            procNote.Note,
					POut.Bool  (procNote.SigIsTopaz),
					            procNote.Signature,
				});
			}
			return table;
		}

		///<summary>Inserts one ProcNote into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcNote procNote){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				procNote.ProcNoteNum=DbHelper.GetNextOracleKey("procnote","ProcNoteNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(procNote,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							procNote.ProcNoteNum++;
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
				return Insert(procNote,false);
			}
		}

		///<summary>Inserts one ProcNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcNote procNote,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procNote.ProcNoteNum=ReplicationServers.GetKey("procnote","ProcNoteNum");
			}
			string command="INSERT INTO procnote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcNoteNum,";
			}
			command+="PatNum,ProcNum,EntryDateTime,UserNum,Note,SigIsTopaz,Signature) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procNote.ProcNoteNum)+",";
			}
			command+=
				     POut.Long  (procNote.PatNum)+","
				+    POut.Long  (procNote.ProcNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (procNote.UserNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Bool  (procNote.SigIsTopaz)+","
				+"'"+POut.String(procNote.Signature)+"')";
			if(procNote.Note==null) {
				procNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,procNote.Note);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				procNote.ProcNoteNum=Db.NonQ(command,true,paramNote);
			}
			return procNote.ProcNoteNum;
		}

		///<summary>Inserts one ProcNote into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcNote procNote){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(procNote,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					procNote.ProcNoteNum=DbHelper.GetNextOracleKey("procnote","ProcNoteNum"); //Cacheless method
				}
				return InsertNoCache(procNote,true);
			}
		}

		///<summary>Inserts one ProcNote into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcNote procNote,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO procnote (";
			if(!useExistingPK && isRandomKeys) {
				procNote.ProcNoteNum=ReplicationServers.GetKeyNoCache("procnote","ProcNoteNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcNoteNum,";
			}
			command+="PatNum,ProcNum,EntryDateTime,UserNum,Note,SigIsTopaz,Signature) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procNote.ProcNoteNum)+",";
			}
			command+=
				     POut.Long  (procNote.PatNum)+","
				+    POut.Long  (procNote.ProcNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (procNote.UserNum)+","
				+    DbHelper.ParamChar+"paramNote,"
				+    POut.Bool  (procNote.SigIsTopaz)+","
				+"'"+POut.String(procNote.Signature)+"')";
			if(procNote.Note==null) {
				procNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,procNote.Note);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				procNote.ProcNoteNum=Db.NonQ(command,true,paramNote);
			}
			return procNote.ProcNoteNum;
		}

		///<summary>Updates one ProcNote in the database.</summary>
		public static void Update(ProcNote procNote){
			string command="UPDATE procnote SET "
				+"PatNum       =  "+POut.Long  (procNote.PatNum)+", "
				+"ProcNum      =  "+POut.Long  (procNote.ProcNum)+", "
				//EntryDateTime not allowed to change
				+"UserNum      =  "+POut.Long  (procNote.UserNum)+", "
				+"Note         =  "+DbHelper.ParamChar+"paramNote, "
				+"SigIsTopaz   =  "+POut.Bool  (procNote.SigIsTopaz)+", "
				+"Signature    = '"+POut.String(procNote.Signature)+"' "
				+"WHERE ProcNoteNum = "+POut.Long(procNote.ProcNoteNum);
			if(procNote.Note==null) {
				procNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,procNote.Note);
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one ProcNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcNote procNote,ProcNote oldProcNote){
			string command="";
			if(procNote.PatNum != oldProcNote.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(procNote.PatNum)+"";
			}
			if(procNote.ProcNum != oldProcNote.ProcNum) {
				if(command!=""){ command+=",";}
				command+="ProcNum = "+POut.Long(procNote.ProcNum)+"";
			}
			//EntryDateTime not allowed to change
			if(procNote.UserNum != oldProcNote.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(procNote.UserNum)+"";
			}
			if(procNote.Note != oldProcNote.Note) {
				if(command!=""){ command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(procNote.SigIsTopaz != oldProcNote.SigIsTopaz) {
				if(command!=""){ command+=",";}
				command+="SigIsTopaz = "+POut.Bool(procNote.SigIsTopaz)+"";
			}
			if(procNote.Signature != oldProcNote.Signature) {
				if(command!=""){ command+=",";}
				command+="Signature = '"+POut.String(procNote.Signature)+"'";
			}
			if(command==""){
				return false;
			}
			if(procNote.Note==null) {
				procNote.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,procNote.Note);
			command="UPDATE procnote SET "+command
				+" WHERE ProcNoteNum = "+POut.Long(procNote.ProcNoteNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(ProcNote,ProcNote) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ProcNote procNote,ProcNote oldProcNote) {
			if(procNote.PatNum != oldProcNote.PatNum) {
				return true;
			}
			if(procNote.ProcNum != oldProcNote.ProcNum) {
				return true;
			}
			//EntryDateTime not allowed to change
			if(procNote.UserNum != oldProcNote.UserNum) {
				return true;
			}
			if(procNote.Note != oldProcNote.Note) {
				return true;
			}
			if(procNote.SigIsTopaz != oldProcNote.SigIsTopaz) {
				return true;
			}
			if(procNote.Signature != oldProcNote.Signature) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ProcNote from the database.</summary>
		public static void Delete(long procNoteNum){
			string command="DELETE FROM procnote "
				+"WHERE ProcNoteNum = "+POut.Long(procNoteNum);
			Db.NonQ(command);
		}

	}
}