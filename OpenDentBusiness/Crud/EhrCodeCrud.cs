//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrCodeCrud {
		///<summary>Gets one EhrCode object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrCode SelectOne(long ehrCodeNum){
			string command="SELECT * FROM ehrcode "
				+"WHERE EhrCodeNum = "+POut.Long(ehrCodeNum);
			List<EhrCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrCode object from the database using a query.</summary>
		public static EhrCode SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrCode objects from the database using a query.</summary>
		public static List<EhrCode> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrCode> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrCode> TableToList(DataTable table){
			List<EhrCode> retVal=new List<EhrCode>();
			EhrCode ehrCode;
			foreach(DataRow row in table.Rows) {
				ehrCode=new EhrCode();
				ehrCode.EhrCodeNum   = PIn.Long  (row["EhrCodeNum"].ToString());
				ehrCode.MeasureIds   = PIn.String(row["MeasureIds"].ToString());
				ehrCode.ValueSetName = PIn.String(row["ValueSetName"].ToString());
				ehrCode.ValueSetOID  = PIn.String(row["ValueSetOID"].ToString());
				ehrCode.QDMCategory  = PIn.String(row["QDMCategory"].ToString());
				ehrCode.CodeValue    = PIn.String(row["CodeValue"].ToString());
				ehrCode.Description  = PIn.String(row["Description"].ToString());
				ehrCode.CodeSystem   = PIn.String(row["CodeSystem"].ToString());
				ehrCode.CodeSystemOID= PIn.String(row["CodeSystemOID"].ToString());
				ehrCode.IsInDb       = PIn.Bool  (row["IsInDb"].ToString());
				retVal.Add(ehrCode);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrCode> listEhrCodes) {
			DataTable table=new DataTable("EhrCodes");
			table.Columns.Add("EhrCodeNum");
			table.Columns.Add("MeasureIds");
			table.Columns.Add("ValueSetName");
			table.Columns.Add("ValueSetOID");
			table.Columns.Add("QDMCategory");
			table.Columns.Add("CodeValue");
			table.Columns.Add("Description");
			table.Columns.Add("CodeSystem");
			table.Columns.Add("CodeSystemOID");
			table.Columns.Add("IsInDb");
			foreach(EhrCode ehrCode in listEhrCodes) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrCode.EhrCodeNum),
					POut.String(ehrCode.MeasureIds),
					POut.String(ehrCode.ValueSetName),
					POut.String(ehrCode.ValueSetOID),
					POut.String(ehrCode.QDMCategory),
					POut.String(ehrCode.CodeValue),
					POut.String(ehrCode.Description),
					POut.String(ehrCode.CodeSystem),
					POut.String(ehrCode.CodeSystemOID),
					POut.Bool  (ehrCode.IsInDb),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrCode into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrCode ehrCode){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				ehrCode.EhrCodeNum=DbHelper.GetNextOracleKey("ehrcode","EhrCodeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(ehrCode,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							ehrCode.EhrCodeNum++;
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
				return Insert(ehrCode,false);
			}
		}

		///<summary>Inserts one EhrCode into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrCode ehrCode,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrCode.EhrCodeNum=ReplicationServers.GetKey("ehrcode","EhrCodeNum");
			}
			string command="INSERT INTO ehrcode (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrCodeNum,";
			}
			command+="MeasureIds,ValueSetName,ValueSetOID,QDMCategory,CodeValue,Description,CodeSystem,CodeSystemOID,IsInDb) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrCode.EhrCodeNum)+",";
			}
			command+=
				 "'"+POut.String(ehrCode.MeasureIds)+"',"
				+"'"+POut.String(ehrCode.ValueSetName)+"',"
				+"'"+POut.String(ehrCode.ValueSetOID)+"',"
				+"'"+POut.String(ehrCode.QDMCategory)+"',"
				+"'"+POut.String(ehrCode.CodeValue)+"',"
				+"'"+POut.String(ehrCode.Description)+"',"
				+"'"+POut.String(ehrCode.CodeSystem)+"',"
				+"'"+POut.String(ehrCode.CodeSystemOID)+"',"
				+    POut.Bool  (ehrCode.IsInDb)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrCode.EhrCodeNum=Db.NonQ(command,true);
			}
			return ehrCode.EhrCodeNum;
		}

		///<summary>Inserts one EhrCode into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrCode ehrCode){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(ehrCode,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					ehrCode.EhrCodeNum=DbHelper.GetNextOracleKey("ehrcode","EhrCodeNum"); //Cacheless method
				}
				return InsertNoCache(ehrCode,true);
			}
		}

		///<summary>Inserts one EhrCode into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrCode ehrCode,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrcode (";
			if(!useExistingPK && isRandomKeys) {
				ehrCode.EhrCodeNum=ReplicationServers.GetKeyNoCache("ehrcode","EhrCodeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrCodeNum,";
			}
			command+="MeasureIds,ValueSetName,ValueSetOID,QDMCategory,CodeValue,Description,CodeSystem,CodeSystemOID,IsInDb) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrCode.EhrCodeNum)+",";
			}
			command+=
				 "'"+POut.String(ehrCode.MeasureIds)+"',"
				+"'"+POut.String(ehrCode.ValueSetName)+"',"
				+"'"+POut.String(ehrCode.ValueSetOID)+"',"
				+"'"+POut.String(ehrCode.QDMCategory)+"',"
				+"'"+POut.String(ehrCode.CodeValue)+"',"
				+"'"+POut.String(ehrCode.Description)+"',"
				+"'"+POut.String(ehrCode.CodeSystem)+"',"
				+"'"+POut.String(ehrCode.CodeSystemOID)+"',"
				+    POut.Bool  (ehrCode.IsInDb)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrCode.EhrCodeNum=Db.NonQ(command,true);
			}
			return ehrCode.EhrCodeNum;
		}

		///<summary>Updates one EhrCode in the database.</summary>
		public static void Update(EhrCode ehrCode){
			string command="UPDATE ehrcode SET "
				+"MeasureIds   = '"+POut.String(ehrCode.MeasureIds)+"', "
				+"ValueSetName = '"+POut.String(ehrCode.ValueSetName)+"', "
				+"ValueSetOID  = '"+POut.String(ehrCode.ValueSetOID)+"', "
				+"QDMCategory  = '"+POut.String(ehrCode.QDMCategory)+"', "
				+"CodeValue    = '"+POut.String(ehrCode.CodeValue)+"', "
				+"Description  = '"+POut.String(ehrCode.Description)+"', "
				+"CodeSystem   = '"+POut.String(ehrCode.CodeSystem)+"', "
				+"CodeSystemOID= '"+POut.String(ehrCode.CodeSystemOID)+"', "
				+"IsInDb       =  "+POut.Bool  (ehrCode.IsInDb)+" "
				+"WHERE EhrCodeNum = "+POut.Long(ehrCode.EhrCodeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrCode in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrCode ehrCode,EhrCode oldEhrCode){
			string command="";
			if(ehrCode.MeasureIds != oldEhrCode.MeasureIds) {
				if(command!=""){ command+=",";}
				command+="MeasureIds = '"+POut.String(ehrCode.MeasureIds)+"'";
			}
			if(ehrCode.ValueSetName != oldEhrCode.ValueSetName) {
				if(command!=""){ command+=",";}
				command+="ValueSetName = '"+POut.String(ehrCode.ValueSetName)+"'";
			}
			if(ehrCode.ValueSetOID != oldEhrCode.ValueSetOID) {
				if(command!=""){ command+=",";}
				command+="ValueSetOID = '"+POut.String(ehrCode.ValueSetOID)+"'";
			}
			if(ehrCode.QDMCategory != oldEhrCode.QDMCategory) {
				if(command!=""){ command+=",";}
				command+="QDMCategory = '"+POut.String(ehrCode.QDMCategory)+"'";
			}
			if(ehrCode.CodeValue != oldEhrCode.CodeValue) {
				if(command!=""){ command+=",";}
				command+="CodeValue = '"+POut.String(ehrCode.CodeValue)+"'";
			}
			if(ehrCode.Description != oldEhrCode.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(ehrCode.Description)+"'";
			}
			if(ehrCode.CodeSystem != oldEhrCode.CodeSystem) {
				if(command!=""){ command+=",";}
				command+="CodeSystem = '"+POut.String(ehrCode.CodeSystem)+"'";
			}
			if(ehrCode.CodeSystemOID != oldEhrCode.CodeSystemOID) {
				if(command!=""){ command+=",";}
				command+="CodeSystemOID = '"+POut.String(ehrCode.CodeSystemOID)+"'";
			}
			if(ehrCode.IsInDb != oldEhrCode.IsInDb) {
				if(command!=""){ command+=",";}
				command+="IsInDb = "+POut.Bool(ehrCode.IsInDb)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE ehrcode SET "+command
				+" WHERE EhrCodeNum = "+POut.Long(ehrCode.EhrCodeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one EhrCode from the database.</summary>
		public static void Delete(long ehrCodeNum){
			string command="DELETE FROM ehrcode "
				+"WHERE EhrCodeNum = "+POut.Long(ehrCodeNum);
			Db.NonQ(command);
		}

	}
}