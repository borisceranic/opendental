//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LaboratoryCrud {
		///<summary>Gets one Laboratory object from the database using the primary key.  Returns null if not found.</summary>
		public static Laboratory SelectOne(long laboratoryNum){
			string command="SELECT * FROM laboratory "
				+"WHERE LaboratoryNum = "+POut.Long(laboratoryNum);
			List<Laboratory> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Laboratory object from the database using a query.</summary>
		public static Laboratory SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Laboratory> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Laboratory objects from the database using a query.</summary>
		public static List<Laboratory> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Laboratory> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Laboratory> TableToList(DataTable table){
			List<Laboratory> retVal=new List<Laboratory>();
			Laboratory laboratory;
			foreach(DataRow row in table.Rows) {
				laboratory=new Laboratory();
				laboratory.LaboratoryNum= PIn.Long  (row["LaboratoryNum"].ToString());
				laboratory.Description  = PIn.String(row["Description"].ToString());
				laboratory.Phone        = PIn.String(row["Phone"].ToString());
				laboratory.Notes        = PIn.String(row["Notes"].ToString());
				laboratory.Slip         = PIn.Long  (row["Slip"].ToString());
				laboratory.Address      = PIn.String(row["Address"].ToString());
				laboratory.City         = PIn.String(row["City"].ToString());
				laboratory.State        = PIn.String(row["State"].ToString());
				laboratory.Zip          = PIn.String(row["Zip"].ToString());
				laboratory.Email        = PIn.String(row["Email"].ToString());
				laboratory.WirelessPhone= PIn.String(row["WirelessPhone"].ToString());
				retVal.Add(laboratory);
			}
			return retVal;
		}

		///<summary>Converts a list of Laboratory into a DataTable.</summary>
		public static DataTable ListToTable(List<Laboratory> listLaboratorys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Laboratory";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("LaboratoryNum");
			table.Columns.Add("Description");
			table.Columns.Add("Phone");
			table.Columns.Add("Notes");
			table.Columns.Add("Slip");
			table.Columns.Add("Address");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("Email");
			table.Columns.Add("WirelessPhone");
			foreach(Laboratory laboratory in listLaboratorys) {
				table.Rows.Add(new object[] {
					POut.Long  (laboratory.LaboratoryNum),
					POut.String(laboratory.Description),
					POut.String(laboratory.Phone),
					POut.String(laboratory.Notes),
					POut.Long  (laboratory.Slip),
					POut.String(laboratory.Address),
					POut.String(laboratory.City),
					POut.String(laboratory.State),
					POut.String(laboratory.Zip),
					POut.String(laboratory.Email),
					POut.String(laboratory.WirelessPhone),
				});
			}
			return table;
		}

		///<summary>Inserts one Laboratory into the database.  Returns the new priKey.</summary>
		public static long Insert(Laboratory laboratory){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				laboratory.LaboratoryNum=DbHelper.GetNextOracleKey("laboratory","LaboratoryNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(laboratory,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							laboratory.LaboratoryNum++;
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
				return Insert(laboratory,false);
			}
		}

		///<summary>Inserts one Laboratory into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Laboratory laboratory,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				laboratory.LaboratoryNum=ReplicationServers.GetKey("laboratory","LaboratoryNum");
			}
			string command="INSERT INTO laboratory (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LaboratoryNum,";
			}
			command+="Description,Phone,Notes,Slip,Address,City,State,Zip,Email,WirelessPhone) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(laboratory.LaboratoryNum)+",";
			}
			command+=
				 "'"+POut.String(laboratory.Description)+"',"
				+"'"+POut.String(laboratory.Phone)+"',"
				+"'"+POut.String(laboratory.Notes)+"',"
				+    POut.Long  (laboratory.Slip)+","
				+"'"+POut.String(laboratory.Address)+"',"
				+"'"+POut.String(laboratory.City)+"',"
				+"'"+POut.String(laboratory.State)+"',"
				+"'"+POut.String(laboratory.Zip)+"',"
				+"'"+POut.String(laboratory.Email)+"',"
				+"'"+POut.String(laboratory.WirelessPhone)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				laboratory.LaboratoryNum=Db.NonQ(command,true);
			}
			return laboratory.LaboratoryNum;
		}

		///<summary>Inserts one Laboratory into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Laboratory laboratory){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(laboratory,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					laboratory.LaboratoryNum=DbHelper.GetNextOracleKey("laboratory","LaboratoryNum"); //Cacheless method
				}
				return InsertNoCache(laboratory,true);
			}
		}

		///<summary>Inserts one Laboratory into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Laboratory laboratory,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO laboratory (";
			if(!useExistingPK && isRandomKeys) {
				laboratory.LaboratoryNum=ReplicationServers.GetKeyNoCache("laboratory","LaboratoryNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="LaboratoryNum,";
			}
			command+="Description,Phone,Notes,Slip,Address,City,State,Zip,Email,WirelessPhone) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(laboratory.LaboratoryNum)+",";
			}
			command+=
				 "'"+POut.String(laboratory.Description)+"',"
				+"'"+POut.String(laboratory.Phone)+"',"
				+"'"+POut.String(laboratory.Notes)+"',"
				+    POut.Long  (laboratory.Slip)+","
				+"'"+POut.String(laboratory.Address)+"',"
				+"'"+POut.String(laboratory.City)+"',"
				+"'"+POut.String(laboratory.State)+"',"
				+"'"+POut.String(laboratory.Zip)+"',"
				+"'"+POut.String(laboratory.Email)+"',"
				+"'"+POut.String(laboratory.WirelessPhone)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				laboratory.LaboratoryNum=Db.NonQ(command,true);
			}
			return laboratory.LaboratoryNum;
		}

		///<summary>Updates one Laboratory in the database.</summary>
		public static void Update(Laboratory laboratory){
			string command="UPDATE laboratory SET "
				+"Description  = '"+POut.String(laboratory.Description)+"', "
				+"Phone        = '"+POut.String(laboratory.Phone)+"', "
				+"Notes        = '"+POut.String(laboratory.Notes)+"', "
				+"Slip         =  "+POut.Long  (laboratory.Slip)+", "
				+"Address      = '"+POut.String(laboratory.Address)+"', "
				+"City         = '"+POut.String(laboratory.City)+"', "
				+"State        = '"+POut.String(laboratory.State)+"', "
				+"Zip          = '"+POut.String(laboratory.Zip)+"', "
				+"Email        = '"+POut.String(laboratory.Email)+"', "
				+"WirelessPhone= '"+POut.String(laboratory.WirelessPhone)+"' "
				+"WHERE LaboratoryNum = "+POut.Long(laboratory.LaboratoryNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Laboratory in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Laboratory laboratory,Laboratory oldLaboratory){
			string command="";
			if(laboratory.Description != oldLaboratory.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(laboratory.Description)+"'";
			}
			if(laboratory.Phone != oldLaboratory.Phone) {
				if(command!=""){ command+=",";}
				command+="Phone = '"+POut.String(laboratory.Phone)+"'";
			}
			if(laboratory.Notes != oldLaboratory.Notes) {
				if(command!=""){ command+=",";}
				command+="Notes = '"+POut.String(laboratory.Notes)+"'";
			}
			if(laboratory.Slip != oldLaboratory.Slip) {
				if(command!=""){ command+=",";}
				command+="Slip = "+POut.Long(laboratory.Slip)+"";
			}
			if(laboratory.Address != oldLaboratory.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(laboratory.Address)+"'";
			}
			if(laboratory.City != oldLaboratory.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(laboratory.City)+"'";
			}
			if(laboratory.State != oldLaboratory.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(laboratory.State)+"'";
			}
			if(laboratory.Zip != oldLaboratory.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(laboratory.Zip)+"'";
			}
			if(laboratory.Email != oldLaboratory.Email) {
				if(command!=""){ command+=",";}
				command+="Email = '"+POut.String(laboratory.Email)+"'";
			}
			if(laboratory.WirelessPhone != oldLaboratory.WirelessPhone) {
				if(command!=""){ command+=",";}
				command+="WirelessPhone = '"+POut.String(laboratory.WirelessPhone)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE laboratory SET "+command
				+" WHERE LaboratoryNum = "+POut.Long(laboratory.LaboratoryNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Laboratory from the database.</summary>
		public static void Delete(long laboratoryNum){
			string command="DELETE FROM laboratory "
				+"WHERE LaboratoryNum = "+POut.Long(laboratoryNum);
			Db.NonQ(command);
		}

	}
}