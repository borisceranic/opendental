//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatFieldCrud {
		///<summary>Gets one PatField object from the database using the primary key.  Returns null if not found.</summary>
		public static PatField SelectOne(long patFieldNum){
			string command="SELECT * FROM patfield "
				+"WHERE PatFieldNum = "+POut.Long(patFieldNum);
			List<PatField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatField object from the database using a query.</summary>
		public static PatField SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatField objects from the database using a query.</summary>
		public static List<PatField> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PatField> TableToList(DataTable table){
			List<PatField> retVal=new List<PatField>();
			PatField patField;
			foreach(DataRow row in table.Rows) {
				patField=new PatField();
				patField.PatFieldNum= PIn.Long  (row["PatFieldNum"].ToString());
				patField.PatNum     = PIn.Long  (row["PatNum"].ToString());
				patField.FieldName  = PIn.String(row["FieldName"].ToString());
				patField.FieldValue = PIn.String(row["FieldValue"].ToString());
				retVal.Add(patField);
			}
			return retVal;
		}

		///<summary>Converts a list of PatField into a DataTable.</summary>
		public static DataTable ListToTable(List<PatField> listPatFields,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="PatField";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("PatFieldNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldValue");
			foreach(PatField patField in listPatFields) {
				table.Rows.Add(new object[] {
					POut.Long  (patField.PatFieldNum),
					POut.Long  (patField.PatNum),
					POut.String(patField.FieldName),
					POut.String(patField.FieldValue),
				});
			}
			return table;
		}

		///<summary>Inserts one PatField into the database.  Returns the new priKey.</summary>
		public static long Insert(PatField patField){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				patField.PatFieldNum=DbHelper.GetNextOracleKey("patfield","PatFieldNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(patField,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							patField.PatFieldNum++;
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
				return Insert(patField,false);
			}
		}

		///<summary>Inserts one PatField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PatField patField,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				patField.PatFieldNum=ReplicationServers.GetKey("patfield","PatFieldNum");
			}
			string command="INSERT INTO patfield (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatFieldNum,";
			}
			command+="PatNum,FieldName,FieldValue) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patField.PatFieldNum)+",";
			}
			command+=
				     POut.Long  (patField.PatNum)+","
				+"'"+POut.String(patField.FieldName)+"',"
				+"'"+POut.String(patField.FieldValue)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patField.PatFieldNum=Db.NonQ(command,true);
			}
			return patField.PatFieldNum;
		}

		///<summary>Inserts one PatField into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatField patField){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(patField,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					patField.PatFieldNum=DbHelper.GetNextOracleKey("patfield","PatFieldNum"); //Cacheless method
				}
				return InsertNoCache(patField,true);
			}
		}

		///<summary>Inserts one PatField into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PatField patField,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO patfield (";
			if(!useExistingPK && isRandomKeys) {
				patField.PatFieldNum=ReplicationServers.GetKeyNoCache("patfield","PatFieldNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PatFieldNum,";
			}
			command+="PatNum,FieldName,FieldValue) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(patField.PatFieldNum)+",";
			}
			command+=
				     POut.Long  (patField.PatNum)+","
				+"'"+POut.String(patField.FieldName)+"',"
				+"'"+POut.String(patField.FieldValue)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				patField.PatFieldNum=Db.NonQ(command,true);
			}
			return patField.PatFieldNum;
		}

		///<summary>Updates one PatField in the database.</summary>
		public static void Update(PatField patField){
			string command="UPDATE patfield SET "
				+"PatNum     =  "+POut.Long  (patField.PatNum)+", "
				+"FieldName  = '"+POut.String(patField.FieldName)+"', "
				+"FieldValue = '"+POut.String(patField.FieldValue)+"' "
				+"WHERE PatFieldNum = "+POut.Long(patField.PatFieldNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PatField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PatField patField,PatField oldPatField){
			string command="";
			if(patField.PatNum != oldPatField.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(patField.PatNum)+"";
			}
			if(patField.FieldName != oldPatField.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(patField.FieldName)+"'";
			}
			if(patField.FieldValue != oldPatField.FieldValue) {
				if(command!=""){ command+=",";}
				command+="FieldValue = '"+POut.String(patField.FieldValue)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE patfield SET "+command
				+" WHERE PatFieldNum = "+POut.Long(patField.PatFieldNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one PatField from the database.</summary>
		public static void Delete(long patFieldNum){
			string command="DELETE FROM patfield "
				+"WHERE PatFieldNum = "+POut.Long(patFieldNum);
			Db.NonQ(command);
		}

	}
}