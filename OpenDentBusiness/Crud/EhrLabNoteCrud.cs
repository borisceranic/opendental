//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using EhrLaboratories;

namespace OpenDentBusiness.Crud{
	public class EhrLabNoteCrud {
		///<summary>Gets one EhrLabNote object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrLabNote SelectOne(long ehrLabNoteNum){
			string command="SELECT * FROM ehrlabnote "
				+"WHERE EhrLabNoteNum = "+POut.Long(ehrLabNoteNum);
			List<EhrLabNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrLabNote object from the database using a query.</summary>
		public static EhrLabNote SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrLabNote objects from the database using a query.</summary>
		public static List<EhrLabNote> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrLabNote> TableToList(DataTable table){
			List<EhrLabNote> retVal=new List<EhrLabNote>();
			EhrLabNote ehrLabNote;
			for(int i=0;i<table.Rows.Count;i++) {
				ehrLabNote=new EhrLabNote();
				ehrLabNote.EhrLabNoteNum  = PIn.Long  (table.Rows[i]["EhrLabNoteNum"].ToString());
				ehrLabNote.EhrLabNum      = PIn.Long  (table.Rows[i]["EhrLabNum"].ToString());
				ehrLabNote.EhrLabResultNum= PIn.Long  (table.Rows[i]["EhrLabResultNum"].ToString());
				ehrLabNote.Comments       = PIn.String(table.Rows[i]["Comments"].ToString());
				retVal.Add(ehrLabNote);
			}
			return retVal;
		}

		///<summary>Inserts one EhrLabNote into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrLabNote ehrLabNote){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				ehrLabNote.EhrLabNoteNum=DbHelper.GetNextOracleKey("ehrlabnote","EhrLabNoteNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(ehrLabNote,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							ehrLabNote.EhrLabNoteNum++;
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
				return Insert(ehrLabNote,false);
			}
		}

		///<summary>Inserts one EhrLabNote into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrLabNote ehrLabNote,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrLabNote.EhrLabNoteNum=ReplicationServers.GetKey("ehrlabnote","EhrLabNoteNum");
			}
			string command="INSERT INTO ehrlabnote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrLabNoteNum,";
			}
			command+="EhrLabNum,EhrLabResultNum,Comments) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrLabNote.EhrLabNoteNum)+",";
			}
			command+=
				     POut.Long  (ehrLabNote.EhrLabNum)+","
				+    POut.Long  (ehrLabNote.EhrLabResultNum)+","
				+    DbHelper.ParamChar+"paramComments)";
			if(ehrLabNote.Comments==null) {
				ehrLabNote.Comments="";
			}
			OdSqlParameter paramComments=new OdSqlParameter("paramComments",OdDbType.Text,ehrLabNote.Comments);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramComments);
			}
			else {
				ehrLabNote.EhrLabNoteNum=Db.NonQ(command,true,paramComments);
			}
			return ehrLabNote.EhrLabNoteNum;
		}

		///<summary>Updates one EhrLabNote in the database.</summary>
		public static void Update(EhrLabNote ehrLabNote){
			string command="UPDATE ehrlabnote SET "
				+"EhrLabNum      =  "+POut.Long  (ehrLabNote.EhrLabNum)+", "
				+"EhrLabResultNum=  "+POut.Long  (ehrLabNote.EhrLabResultNum)+", "
				+"Comments       =  "+DbHelper.ParamChar+"paramComments "
				+"WHERE EhrLabNoteNum = "+POut.Long(ehrLabNote.EhrLabNoteNum);
			if(ehrLabNote.Comments==null) {
				ehrLabNote.Comments="";
			}
			OdSqlParameter paramComments=new OdSqlParameter("paramComments",OdDbType.Text,ehrLabNote.Comments);
			Db.NonQ(command,paramComments);
		}

		///<summary>Updates one EhrLabNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrLabNote ehrLabNote,EhrLabNote oldEhrLabNote){
			string command="";
			if(ehrLabNote.EhrLabNum != oldEhrLabNote.EhrLabNum) {
				if(command!=""){ command+=",";}
				command+="EhrLabNum = "+POut.Long(ehrLabNote.EhrLabNum)+"";
			}
			if(ehrLabNote.EhrLabResultNum != oldEhrLabNote.EhrLabResultNum) {
				if(command!=""){ command+=",";}
				command+="EhrLabResultNum = "+POut.Long(ehrLabNote.EhrLabResultNum)+"";
			}
			if(ehrLabNote.Comments != oldEhrLabNote.Comments) {
				if(command!=""){ command+=",";}
				command+="Comments = "+DbHelper.ParamChar+"paramComments";
			}
			if(command==""){
				return false;
			}
			if(ehrLabNote.Comments==null) {
				ehrLabNote.Comments="";
			}
			OdSqlParameter paramComments=new OdSqlParameter("paramComments",OdDbType.Text,ehrLabNote.Comments);
			command="UPDATE ehrlabnote SET "+command
				+" WHERE EhrLabNoteNum = "+POut.Long(ehrLabNote.EhrLabNoteNum);
			Db.NonQ(command,paramComments);
			return true;
		}

		///<summary>Deletes one EhrLabNote from the database.</summary>
		public static void Delete(long ehrLabNoteNum){
			string command="DELETE FROM ehrlabnote "
				+"WHERE EhrLabNoteNum = "+POut.Long(ehrLabNoteNum);
			Db.NonQ(command);
		}

	}
}