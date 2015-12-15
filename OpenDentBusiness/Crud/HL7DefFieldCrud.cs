//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class HL7DefFieldCrud {
		///<summary>Gets one HL7DefField object from the database using the primary key.  Returns null if not found.</summary>
		public static HL7DefField SelectOne(long hL7DefFieldNum){
			string command="SELECT * FROM hl7deffield "
				+"WHERE HL7DefFieldNum = "+POut.Long(hL7DefFieldNum);
			List<HL7DefField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one HL7DefField object from the database using a query.</summary>
		public static HL7DefField SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7DefField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of HL7DefField objects from the database using a query.</summary>
		public static List<HL7DefField> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7DefField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<HL7DefField> TableToList(DataTable table){
			List<HL7DefField> retVal=new List<HL7DefField>();
			HL7DefField hL7DefField;
			foreach(DataRow row in table.Rows) {
				hL7DefField=new HL7DefField();
				hL7DefField.HL7DefFieldNum  = PIn.Long  (row["HL7DefFieldNum"].ToString());
				hL7DefField.HL7DefSegmentNum= PIn.Long  (row["HL7DefSegmentNum"].ToString());
				hL7DefField.OrdinalPos      = PIn.Int   (row["OrdinalPos"].ToString());
				hL7DefField.TableId         = PIn.String(row["TableId"].ToString());
				string dataType=row["DataType"].ToString();
				if(dataType==""){
					hL7DefField.DataType      =(DataTypeHL7)0;
				}
				else try{
					hL7DefField.DataType      =(DataTypeHL7)Enum.Parse(typeof(DataTypeHL7),dataType);
				}
				catch{
					hL7DefField.DataType      =(DataTypeHL7)0;
				}
				hL7DefField.FieldName       = PIn.String(row["FieldName"].ToString());
				hL7DefField.FixedText       = PIn.String(row["FixedText"].ToString());
				retVal.Add(hL7DefField);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<HL7DefField> listHL7DefFields) {
			DataTable table=new DataTable("HL7DefFields");
			table.Columns.Add("HL7DefFieldNum");
			table.Columns.Add("HL7DefSegmentNum");
			table.Columns.Add("OrdinalPos");
			table.Columns.Add("TableId");
			table.Columns.Add("DataType");
			table.Columns.Add("FieldName");
			table.Columns.Add("FixedText");
			foreach(HL7DefField hL7DefField in listHL7DefFields) {
				table.Rows.Add(new object[] {
					POut.Long  (hL7DefField.HL7DefFieldNum),
					POut.Long  (hL7DefField.HL7DefSegmentNum),
					POut.Int   (hL7DefField.OrdinalPos),
					POut.String(hL7DefField.TableId),
					POut.Int   ((int)hL7DefField.DataType),
					POut.String(hL7DefField.FieldName),
					POut.String(hL7DefField.FixedText),
				});
			}
			return table;
		}

		///<summary>Inserts one HL7DefField into the database.  Returns the new priKey.</summary>
		public static long Insert(HL7DefField hL7DefField){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				hL7DefField.HL7DefFieldNum=DbHelper.GetNextOracleKey("hl7deffield","HL7DefFieldNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(hL7DefField,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							hL7DefField.HL7DefFieldNum++;
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
				return Insert(hL7DefField,false);
			}
		}

		///<summary>Inserts one HL7DefField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(HL7DefField hL7DefField,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				hL7DefField.HL7DefFieldNum=ReplicationServers.GetKey("hl7deffield","HL7DefFieldNum");
			}
			string command="INSERT INTO hl7deffield (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="HL7DefFieldNum,";
			}
			command+="HL7DefSegmentNum,OrdinalPos,TableId,DataType,FieldName,FixedText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(hL7DefField.HL7DefFieldNum)+",";
			}
			command+=
				     POut.Long  (hL7DefField.HL7DefSegmentNum)+","
				+    POut.Int   (hL7DefField.OrdinalPos)+","
				+"'"+POut.String(hL7DefField.TableId)+"',"
				+"'"+POut.String(hL7DefField.DataType.ToString())+"',"
				+"'"+POut.String(hL7DefField.FieldName)+"',"
				+"'"+POut.String(hL7DefField.FixedText)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				hL7DefField.HL7DefFieldNum=Db.NonQ(command,true);
			}
			return hL7DefField.HL7DefFieldNum;
		}

		///<summary>Inserts one HL7DefField into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(HL7DefField hL7DefField){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(hL7DefField,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					hL7DefField.HL7DefFieldNum=DbHelper.GetNextOracleKey("hl7deffield","HL7DefFieldNum"); //Cacheless method
				}
				return InsertNoCache(hL7DefField,true);
			}
		}

		///<summary>Inserts one HL7DefField into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(HL7DefField hL7DefField,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO hl7deffield (";
			if(!useExistingPK && isRandomKeys) {
				hL7DefField.HL7DefFieldNum=ReplicationServers.GetKeyNoCache("hl7deffield","HL7DefFieldNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="HL7DefFieldNum,";
			}
			command+="HL7DefSegmentNum,OrdinalPos,TableId,DataType,FieldName,FixedText) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(hL7DefField.HL7DefFieldNum)+",";
			}
			command+=
				     POut.Long  (hL7DefField.HL7DefSegmentNum)+","
				+    POut.Int   (hL7DefField.OrdinalPos)+","
				+"'"+POut.String(hL7DefField.TableId)+"',"
				+"'"+POut.String(hL7DefField.DataType.ToString())+"',"
				+"'"+POut.String(hL7DefField.FieldName)+"',"
				+"'"+POut.String(hL7DefField.FixedText)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				hL7DefField.HL7DefFieldNum=Db.NonQ(command,true);
			}
			return hL7DefField.HL7DefFieldNum;
		}

		///<summary>Updates one HL7DefField in the database.</summary>
		public static void Update(HL7DefField hL7DefField){
			string command="UPDATE hl7deffield SET "
				+"HL7DefSegmentNum=  "+POut.Long  (hL7DefField.HL7DefSegmentNum)+", "
				+"OrdinalPos      =  "+POut.Int   (hL7DefField.OrdinalPos)+", "
				+"TableId         = '"+POut.String(hL7DefField.TableId)+"', "
				+"DataType        = '"+POut.String(hL7DefField.DataType.ToString())+"', "
				+"FieldName       = '"+POut.String(hL7DefField.FieldName)+"', "
				+"FixedText       = '"+POut.String(hL7DefField.FixedText)+"' "
				+"WHERE HL7DefFieldNum = "+POut.Long(hL7DefField.HL7DefFieldNum);
			Db.NonQ(command);
		}

		///<summary>Updates one HL7DefField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(HL7DefField hL7DefField,HL7DefField oldHL7DefField){
			string command="";
			if(hL7DefField.HL7DefSegmentNum != oldHL7DefField.HL7DefSegmentNum) {
				if(command!=""){ command+=",";}
				command+="HL7DefSegmentNum = "+POut.Long(hL7DefField.HL7DefSegmentNum)+"";
			}
			if(hL7DefField.OrdinalPos != oldHL7DefField.OrdinalPos) {
				if(command!=""){ command+=",";}
				command+="OrdinalPos = "+POut.Int(hL7DefField.OrdinalPos)+"";
			}
			if(hL7DefField.TableId != oldHL7DefField.TableId) {
				if(command!=""){ command+=",";}
				command+="TableId = '"+POut.String(hL7DefField.TableId)+"'";
			}
			if(hL7DefField.DataType != oldHL7DefField.DataType) {
				if(command!=""){ command+=",";}
				command+="DataType = '"+POut.String(hL7DefField.DataType.ToString())+"'";
			}
			if(hL7DefField.FieldName != oldHL7DefField.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(hL7DefField.FieldName)+"'";
			}
			if(hL7DefField.FixedText != oldHL7DefField.FixedText) {
				if(command!=""){ command+=",";}
				command+="FixedText = '"+POut.String(hL7DefField.FixedText)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE hl7deffield SET "+command
				+" WHERE HL7DefFieldNum = "+POut.Long(hL7DefField.HL7DefFieldNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one HL7DefField from the database.</summary>
		public static void Delete(long hL7DefFieldNum){
			string command="DELETE FROM hl7deffield "
				+"WHERE HL7DefFieldNum = "+POut.Long(hL7DefFieldNum);
			Db.NonQ(command);
		}

	}
}