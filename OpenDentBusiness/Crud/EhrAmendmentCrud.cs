//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrAmendmentCrud {
		///<summary>Gets one EhrAmendment object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrAmendment SelectOne(long ehrAmendmentNum){
			string command="SELECT * FROM ehramendment "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendmentNum);
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrAmendment object from the database using a query.</summary>
		public static EhrAmendment SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrAmendment objects from the database using a query.</summary>
		public static List<EhrAmendment> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrAmendment> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrAmendment> TableToList(DataTable table){
			List<EhrAmendment> retVal=new List<EhrAmendment>();
			EhrAmendment ehrAmendment;
			for(int i=0;i<table.Rows.Count;i++) {
				ehrAmendment=new EhrAmendment();
				ehrAmendment.EhrAmendmentNum= PIn.Long  (table.Rows[i]["EhrAmendmentNum"].ToString());
				ehrAmendment.PatNum         = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				ehrAmendment.IsAccepted     = (OpenDentBusiness.YN)PIn.Int(table.Rows[i]["IsAccepted"].ToString());
				ehrAmendment.Description    = PIn.String(table.Rows[i]["Description"].ToString());
				ehrAmendment.Source         = (OpenDentBusiness.AmendmentSource)PIn.Int(table.Rows[i]["Source"].ToString());
				ehrAmendment.SourceName     = PIn.String(table.Rows[i]["SourceName"].ToString());
				ehrAmendment.FileName       = PIn.String(table.Rows[i]["FileName"].ToString());
				ehrAmendment.RawBase64      = PIn.String(table.Rows[i]["RawBase64"].ToString());
				ehrAmendment.DateTRequest   = PIn.DateT (table.Rows[i]["DateTRequest"].ToString());
				ehrAmendment.DateTAcceptDeny= PIn.DateT (table.Rows[i]["DateTAcceptDeny"].ToString());
				ehrAmendment.DateTAppend    = PIn.DateT (table.Rows[i]["DateTAppend"].ToString());
				retVal.Add(ehrAmendment);
			}
			return retVal;
		}

		///<summary>Inserts one EhrAmendment into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrAmendment ehrAmendment){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				ehrAmendment.EhrAmendmentNum=DbHelper.GetNextOracleKey("ehramendment","EhrAmendmentNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(ehrAmendment,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							ehrAmendment.EhrAmendmentNum++;
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
				return Insert(ehrAmendment,false);
			}
		}

		///<summary>Inserts one EhrAmendment into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrAmendment ehrAmendment,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrAmendment.EhrAmendmentNum=ReplicationServers.GetKey("ehramendment","EhrAmendmentNum");
			}
			string command="INSERT INTO ehramendment (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrAmendmentNum,";
			}
			command+="PatNum,IsAccepted,Description,Source,SourceName,FileName,RawBase64,DateTRequest,DateTAcceptDeny,DateTAppend) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrAmendment.EhrAmendmentNum)+",";
			}
			command+=
				     POut.Long  (ehrAmendment.PatNum)+","
				+    POut.Int   ((int)ehrAmendment.IsAccepted)+","
				+"'"+POut.String(ehrAmendment.Description)+"',"
				+    POut.Int   ((int)ehrAmendment.Source)+","
				+"'"+POut.String(ehrAmendment.SourceName)+"',"
				+"'"+POut.String(ehrAmendment.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64,"
				+    POut.DateT (ehrAmendment.DateTRequest)+","
				+    POut.DateT (ehrAmendment.DateTAcceptDeny)+","
				+    POut.DateT (ehrAmendment.DateTAppend)+")";
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,ehrAmendment.RawBase64);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramRawBase64);
			}
			else {
				ehrAmendment.EhrAmendmentNum=Db.NonQ(command,true,paramRawBase64);
			}
			return ehrAmendment.EhrAmendmentNum;
		}

		///<summary>Inserts one EhrAmendment into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAmendment ehrAmendment){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(ehrAmendment,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					ehrAmendment.EhrAmendmentNum=DbHelper.GetNextOracleKey("ehramendment","EhrAmendmentNum"); //Cacheless method
				}
				return InsertNoCache(ehrAmendment,true);
			}
		}

		///<summary>Inserts one EhrAmendment into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrAmendment ehrAmendment,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehramendment (";
			if(!useExistingPK && isRandomKeys) {
				ehrAmendment.EhrAmendmentNum=ReplicationServers.GetKeyNoCache("ehramendment","EhrAmendmentNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrAmendmentNum,";
			}
			command+="PatNum,IsAccepted,Description,Source,SourceName,FileName,RawBase64,DateTRequest,DateTAcceptDeny,DateTAppend) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrAmendment.EhrAmendmentNum)+",";
			}
			command+=
				     POut.Long  (ehrAmendment.PatNum)+","
				+    POut.Int   ((int)ehrAmendment.IsAccepted)+","
				+"'"+POut.String(ehrAmendment.Description)+"',"
				+    POut.Int   ((int)ehrAmendment.Source)+","
				+"'"+POut.String(ehrAmendment.SourceName)+"',"
				+"'"+POut.String(ehrAmendment.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64,"
				+    POut.DateT (ehrAmendment.DateTRequest)+","
				+    POut.DateT (ehrAmendment.DateTAcceptDeny)+","
				+    POut.DateT (ehrAmendment.DateTAppend)+")";
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,ehrAmendment.RawBase64);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramRawBase64);
			}
			else {
				ehrAmendment.EhrAmendmentNum=Db.NonQ(command,true,paramRawBase64);
			}
			return ehrAmendment.EhrAmendmentNum;
		}

		///<summary>Updates one EhrAmendment in the database.</summary>
		public static void Update(EhrAmendment ehrAmendment){
			string command="UPDATE ehramendment SET "
				+"PatNum         =  "+POut.Long  (ehrAmendment.PatNum)+", "
				+"IsAccepted     =  "+POut.Int   ((int)ehrAmendment.IsAccepted)+", "
				+"Description    = '"+POut.String(ehrAmendment.Description)+"', "
				+"Source         =  "+POut.Int   ((int)ehrAmendment.Source)+", "
				+"SourceName     = '"+POut.String(ehrAmendment.SourceName)+"', "
				+"FileName       = '"+POut.String(ehrAmendment.FileName)+"', "
				+"RawBase64      =  "+DbHelper.ParamChar+"paramRawBase64, "
				+"DateTRequest   =  "+POut.DateT (ehrAmendment.DateTRequest)+", "
				+"DateTAcceptDeny=  "+POut.DateT (ehrAmendment.DateTAcceptDeny)+", "
				+"DateTAppend    =  "+POut.DateT (ehrAmendment.DateTAppend)+" "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendment.EhrAmendmentNum);
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,ehrAmendment.RawBase64);
			Db.NonQ(command,paramRawBase64);
		}

		///<summary>Updates one EhrAmendment in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrAmendment ehrAmendment,EhrAmendment oldEhrAmendment){
			string command="";
			if(ehrAmendment.PatNum != oldEhrAmendment.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(ehrAmendment.PatNum)+"";
			}
			if(ehrAmendment.IsAccepted != oldEhrAmendment.IsAccepted) {
				if(command!=""){ command+=",";}
				command+="IsAccepted = "+POut.Int   ((int)ehrAmendment.IsAccepted)+"";
			}
			if(ehrAmendment.Description != oldEhrAmendment.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(ehrAmendment.Description)+"'";
			}
			if(ehrAmendment.Source != oldEhrAmendment.Source) {
				if(command!=""){ command+=",";}
				command+="Source = "+POut.Int   ((int)ehrAmendment.Source)+"";
			}
			if(ehrAmendment.SourceName != oldEhrAmendment.SourceName) {
				if(command!=""){ command+=",";}
				command+="SourceName = '"+POut.String(ehrAmendment.SourceName)+"'";
			}
			if(ehrAmendment.FileName != oldEhrAmendment.FileName) {
				if(command!=""){ command+=",";}
				command+="FileName = '"+POut.String(ehrAmendment.FileName)+"'";
			}
			if(ehrAmendment.RawBase64 != oldEhrAmendment.RawBase64) {
				if(command!=""){ command+=",";}
				command+="RawBase64 = "+DbHelper.ParamChar+"paramRawBase64";
			}
			if(ehrAmendment.DateTRequest != oldEhrAmendment.DateTRequest) {
				if(command!=""){ command+=",";}
				command+="DateTRequest = "+POut.DateT(ehrAmendment.DateTRequest)+"";
			}
			if(ehrAmendment.DateTAcceptDeny != oldEhrAmendment.DateTAcceptDeny) {
				if(command!=""){ command+=",";}
				command+="DateTAcceptDeny = "+POut.DateT(ehrAmendment.DateTAcceptDeny)+"";
			}
			if(ehrAmendment.DateTAppend != oldEhrAmendment.DateTAppend) {
				if(command!=""){ command+=",";}
				command+="DateTAppend = "+POut.DateT(ehrAmendment.DateTAppend)+"";
			}
			if(command==""){
				return false;
			}
			if(ehrAmendment.RawBase64==null) {
				ehrAmendment.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,ehrAmendment.RawBase64);
			command="UPDATE ehramendment SET "+command
				+" WHERE EhrAmendmentNum = "+POut.Long(ehrAmendment.EhrAmendmentNum);
			Db.NonQ(command,paramRawBase64);
			return true;
		}

		///<summary>Deletes one EhrAmendment from the database.</summary>
		public static void Delete(long ehrAmendmentNum){
			string command="DELETE FROM ehramendment "
				+"WHERE EhrAmendmentNum = "+POut.Long(ehrAmendmentNum);
			Db.NonQ(command);
		}

	}
}