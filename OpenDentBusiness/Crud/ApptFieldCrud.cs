//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ApptFieldCrud {
		///<summary>Gets one ApptField object from the database using the primary key.  Returns null if not found.</summary>
		public static ApptField SelectOne(long apptFieldNum){
			string command="SELECT * FROM apptfield "
				+"WHERE ApptFieldNum = "+POut.Long(apptFieldNum);
			List<ApptField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ApptField object from the database using a query.</summary>
		public static ApptField SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ApptField objects from the database using a query.</summary>
		public static List<ApptField> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ApptField> TableToList(DataTable table){
			List<ApptField> retVal=new List<ApptField>();
			ApptField apptField;
			foreach(DataRow row in table.Rows) {
				apptField=new ApptField();
				apptField.ApptFieldNum= PIn.Long  (row["ApptFieldNum"].ToString());
				apptField.AptNum      = PIn.Long  (row["AptNum"].ToString());
				apptField.FieldName   = PIn.String(row["FieldName"].ToString());
				apptField.FieldValue  = PIn.String(row["FieldValue"].ToString());
				retVal.Add(apptField);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ApptField> listApptFields) {
			DataTable table=new DataTable("ApptFields");
			table.Columns.Add("ApptFieldNum");
			table.Columns.Add("AptNum");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldValue");
			foreach(ApptField apptField in listApptFields) {
				table.Rows.Add(new object[] {
					POut.Long  (apptField.ApptFieldNum),
					POut.Long  (apptField.AptNum),
					POut.String(apptField.FieldName),
					POut.String(apptField.FieldValue),
				});
			}
			return table;
		}

		///<summary>Inserts one ApptField into the database.  Returns the new priKey.</summary>
		public static long Insert(ApptField apptField){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				apptField.ApptFieldNum=DbHelper.GetNextOracleKey("apptfield","ApptFieldNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(apptField,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							apptField.ApptFieldNum++;
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
				return Insert(apptField,false);
			}
		}

		///<summary>Inserts one ApptField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ApptField apptField,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				apptField.ApptFieldNum=ReplicationServers.GetKey("apptfield","ApptFieldNum");
			}
			string command="INSERT INTO apptfield (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ApptFieldNum,";
			}
			command+="AptNum,FieldName,FieldValue) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(apptField.ApptFieldNum)+",";
			}
			command+=
				     POut.Long  (apptField.AptNum)+","
				+"'"+POut.String(apptField.FieldName)+"',"
				+"'"+POut.String(apptField.FieldValue)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				apptField.ApptFieldNum=Db.NonQ(command,true);
			}
			return apptField.ApptFieldNum;
		}

		///<summary>Inserts one ApptField into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptField apptField){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(apptField,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					apptField.ApptFieldNum=DbHelper.GetNextOracleKey("apptfield","ApptFieldNum"); //Cacheless method
				}
				return InsertNoCache(apptField,true);
			}
		}

		///<summary>Inserts one ApptField into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptField apptField,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO apptfield (";
			if(!useExistingPK && isRandomKeys) {
				apptField.ApptFieldNum=ReplicationServers.GetKeyNoCache("apptfield","ApptFieldNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ApptFieldNum,";
			}
			command+="AptNum,FieldName,FieldValue) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(apptField.ApptFieldNum)+",";
			}
			command+=
				     POut.Long  (apptField.AptNum)+","
				+"'"+POut.String(apptField.FieldName)+"',"
				+"'"+POut.String(apptField.FieldValue)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				apptField.ApptFieldNum=Db.NonQ(command,true);
			}
			return apptField.ApptFieldNum;
		}

		///<summary>Updates one ApptField in the database.</summary>
		public static void Update(ApptField apptField){
			string command="UPDATE apptfield SET "
				+"AptNum      =  "+POut.Long  (apptField.AptNum)+", "
				+"FieldName   = '"+POut.String(apptField.FieldName)+"', "
				+"FieldValue  = '"+POut.String(apptField.FieldValue)+"' "
				+"WHERE ApptFieldNum = "+POut.Long(apptField.ApptFieldNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ApptField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ApptField apptField,ApptField oldApptField){
			string command="";
			if(apptField.AptNum != oldApptField.AptNum) {
				if(command!=""){ command+=",";}
				command+="AptNum = "+POut.Long(apptField.AptNum)+"";
			}
			if(apptField.FieldName != oldApptField.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(apptField.FieldName)+"'";
			}
			if(apptField.FieldValue != oldApptField.FieldValue) {
				if(command!=""){ command+=",";}
				command+="FieldValue = '"+POut.String(apptField.FieldValue)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE apptfield SET "+command
				+" WHERE ApptFieldNum = "+POut.Long(apptField.ApptFieldNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ApptField from the database.</summary>
		public static void Delete(long apptFieldNum){
			string command="DELETE FROM apptfield "
				+"WHERE ApptFieldNum = "+POut.Long(apptFieldNum);
			Db.NonQ(command);
		}

	}
}