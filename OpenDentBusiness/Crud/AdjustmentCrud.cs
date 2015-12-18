//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class AdjustmentCrud {
		///<summary>Gets one Adjustment object from the database using the primary key.  Returns null if not found.</summary>
		public static Adjustment SelectOne(long adjNum){
			string command="SELECT * FROM adjustment "
				+"WHERE AdjNum = "+POut.Long(adjNum);
			List<Adjustment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Adjustment object from the database using a query.</summary>
		public static Adjustment SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Adjustment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Adjustment objects from the database using a query.</summary>
		public static List<Adjustment> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Adjustment> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Adjustment> TableToList(DataTable table){
			List<Adjustment> retVal=new List<Adjustment>();
			Adjustment adjustment;
			foreach(DataRow row in table.Rows) {
				adjustment=new Adjustment();
				adjustment.AdjNum      = PIn.Long  (row["AdjNum"].ToString());
				adjustment.AdjDate     = PIn.Date  (row["AdjDate"].ToString());
				adjustment.AdjAmt      = PIn.Double(row["AdjAmt"].ToString());
				adjustment.PatNum      = PIn.Long  (row["PatNum"].ToString());
				adjustment.AdjType     = PIn.Long  (row["AdjType"].ToString());
				adjustment.ProvNum     = PIn.Long  (row["ProvNum"].ToString());
				adjustment.AdjNote     = PIn.String(row["AdjNote"].ToString());
				adjustment.ProcDate    = PIn.Date  (row["ProcDate"].ToString());
				adjustment.ProcNum     = PIn.Long  (row["ProcNum"].ToString());
				adjustment.DateEntry   = PIn.Date  (row["DateEntry"].ToString());
				adjustment.ClinicNum   = PIn.Long  (row["ClinicNum"].ToString());
				adjustment.StatementNum= PIn.Long  (row["StatementNum"].ToString());
				retVal.Add(adjustment);
			}
			return retVal;
		}

		///<summary>Converts a list of Adjustment into a DataTable.</summary>
		public static DataTable ListToTable(List<Adjustment> listAdjustments,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Adjustment";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AdjNum");
			table.Columns.Add("AdjDate");
			table.Columns.Add("AdjAmt");
			table.Columns.Add("PatNum");
			table.Columns.Add("AdjType");
			table.Columns.Add("ProvNum");
			table.Columns.Add("AdjNote");
			table.Columns.Add("ProcDate");
			table.Columns.Add("ProcNum");
			table.Columns.Add("DateEntry");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("StatementNum");
			foreach(Adjustment adjustment in listAdjustments) {
				table.Rows.Add(new object[] {
					POut.Long  (adjustment.AdjNum),
					POut.Date  (adjustment.AdjDate),
					POut.Double(adjustment.AdjAmt),
					POut.Long  (adjustment.PatNum),
					POut.Long  (adjustment.AdjType),
					POut.Long  (adjustment.ProvNum),
					POut.String(adjustment.AdjNote),
					POut.Date  (adjustment.ProcDate),
					POut.Long  (adjustment.ProcNum),
					POut.Date  (adjustment.DateEntry),
					POut.Long  (adjustment.ClinicNum),
					POut.Long  (adjustment.StatementNum),
				});
			}
			return table;
		}

		///<summary>Inserts one Adjustment into the database.  Returns the new priKey.</summary>
		public static long Insert(Adjustment adjustment){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				adjustment.AdjNum=DbHelper.GetNextOracleKey("adjustment","AdjNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(adjustment,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							adjustment.AdjNum++;
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
				return Insert(adjustment,false);
			}
		}

		///<summary>Inserts one Adjustment into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Adjustment adjustment,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				adjustment.AdjNum=ReplicationServers.GetKey("adjustment","AdjNum");
			}
			string command="INSERT INTO adjustment (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AdjNum,";
			}
			command+="AdjDate,AdjAmt,PatNum,AdjType,ProvNum,AdjNote,ProcDate,ProcNum,DateEntry,ClinicNum,StatementNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(adjustment.AdjNum)+",";
			}
			command+=
				     POut.Date  (adjustment.AdjDate)+","
				+"'"+POut.Double(adjustment.AdjAmt)+"',"
				+    POut.Long  (adjustment.PatNum)+","
				+    POut.Long  (adjustment.AdjType)+","
				+    POut.Long  (adjustment.ProvNum)+","
				+"'"+POut.String(adjustment.AdjNote)+"',"
				+    POut.Date  (adjustment.ProcDate)+","
				+    POut.Long  (adjustment.ProcNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (adjustment.ClinicNum)+","
				+    POut.Long  (adjustment.StatementNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				adjustment.AdjNum=Db.NonQ(command,true);
			}
			return adjustment.AdjNum;
		}

		///<summary>Inserts one Adjustment into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Adjustment adjustment){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(adjustment,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					adjustment.AdjNum=DbHelper.GetNextOracleKey("adjustment","AdjNum"); //Cacheless method
				}
				return InsertNoCache(adjustment,true);
			}
		}

		///<summary>Inserts one Adjustment into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Adjustment adjustment,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO adjustment (";
			if(!useExistingPK && isRandomKeys) {
				adjustment.AdjNum=ReplicationServers.GetKeyNoCache("adjustment","AdjNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AdjNum,";
			}
			command+="AdjDate,AdjAmt,PatNum,AdjType,ProvNum,AdjNote,ProcDate,ProcNum,DateEntry,ClinicNum,StatementNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(adjustment.AdjNum)+",";
			}
			command+=
				     POut.Date  (adjustment.AdjDate)+","
				+"'"+POut.Double(adjustment.AdjAmt)+"',"
				+    POut.Long  (adjustment.PatNum)+","
				+    POut.Long  (adjustment.AdjType)+","
				+    POut.Long  (adjustment.ProvNum)+","
				+"'"+POut.String(adjustment.AdjNote)+"',"
				+    POut.Date  (adjustment.ProcDate)+","
				+    POut.Long  (adjustment.ProcNum)+","
				+    DbHelper.Now()+","
				+    POut.Long  (adjustment.ClinicNum)+","
				+    POut.Long  (adjustment.StatementNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				adjustment.AdjNum=Db.NonQ(command,true);
			}
			return adjustment.AdjNum;
		}

		///<summary>Updates one Adjustment in the database.</summary>
		public static void Update(Adjustment adjustment){
			string command="UPDATE adjustment SET "
				+"AdjDate     =  "+POut.Date  (adjustment.AdjDate)+", "
				+"AdjAmt      = '"+POut.Double(adjustment.AdjAmt)+"', "
				+"PatNum      =  "+POut.Long  (adjustment.PatNum)+", "
				+"AdjType     =  "+POut.Long  (adjustment.AdjType)+", "
				+"ProvNum     =  "+POut.Long  (adjustment.ProvNum)+", "
				+"AdjNote     = '"+POut.String(adjustment.AdjNote)+"', "
				+"ProcDate    =  "+POut.Date  (adjustment.ProcDate)+", "
				+"ProcNum     =  "+POut.Long  (adjustment.ProcNum)+", "
				//DateEntry not allowed to change
				+"ClinicNum   =  "+POut.Long  (adjustment.ClinicNum)+", "
				+"StatementNum=  "+POut.Long  (adjustment.StatementNum)+" "
				+"WHERE AdjNum = "+POut.Long(adjustment.AdjNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Adjustment in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Adjustment adjustment,Adjustment oldAdjustment){
			string command="";
			if(adjustment.AdjDate.Date != oldAdjustment.AdjDate.Date) {
				if(command!=""){ command+=",";}
				command+="AdjDate = "+POut.Date(adjustment.AdjDate)+"";
			}
			if(adjustment.AdjAmt != oldAdjustment.AdjAmt) {
				if(command!=""){ command+=",";}
				command+="AdjAmt = '"+POut.Double(adjustment.AdjAmt)+"'";
			}
			if(adjustment.PatNum != oldAdjustment.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(adjustment.PatNum)+"";
			}
			if(adjustment.AdjType != oldAdjustment.AdjType) {
				if(command!=""){ command+=",";}
				command+="AdjType = "+POut.Long(adjustment.AdjType)+"";
			}
			if(adjustment.ProvNum != oldAdjustment.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(adjustment.ProvNum)+"";
			}
			if(adjustment.AdjNote != oldAdjustment.AdjNote) {
				if(command!=""){ command+=",";}
				command+="AdjNote = '"+POut.String(adjustment.AdjNote)+"'";
			}
			if(adjustment.ProcDate.Date != oldAdjustment.ProcDate.Date) {
				if(command!=""){ command+=",";}
				command+="ProcDate = "+POut.Date(adjustment.ProcDate)+"";
			}
			if(adjustment.ProcNum != oldAdjustment.ProcNum) {
				if(command!=""){ command+=",";}
				command+="ProcNum = "+POut.Long(adjustment.ProcNum)+"";
			}
			//DateEntry not allowed to change
			if(adjustment.ClinicNum != oldAdjustment.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(adjustment.ClinicNum)+"";
			}
			if(adjustment.StatementNum != oldAdjustment.StatementNum) {
				if(command!=""){ command+=",";}
				command+="StatementNum = "+POut.Long(adjustment.StatementNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE adjustment SET "+command
				+" WHERE AdjNum = "+POut.Long(adjustment.AdjNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Adjustment from the database.</summary>
		public static void Delete(long adjNum){
			string command="DELETE FROM adjustment "
				+"WHERE AdjNum = "+POut.Long(adjNum);
			Db.NonQ(command);
		}

	}
}