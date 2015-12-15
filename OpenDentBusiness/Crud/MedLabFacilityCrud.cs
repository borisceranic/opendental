//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MedLabFacilityCrud {
		///<summary>Gets one MedLabFacility object from the database using the primary key.  Returns null if not found.</summary>
		public static MedLabFacility SelectOne(long medLabFacilityNum){
			string command="SELECT * FROM medlabfacility "
				+"WHERE MedLabFacilityNum = "+POut.Long(medLabFacilityNum);
			List<MedLabFacility> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MedLabFacility object from the database using a query.</summary>
		public static MedLabFacility SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabFacility> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MedLabFacility objects from the database using a query.</summary>
		public static List<MedLabFacility> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabFacility> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MedLabFacility> TableToList(DataTable table){
			List<MedLabFacility> retVal=new List<MedLabFacility>();
			MedLabFacility medLabFacility;
			foreach(DataRow row in table.Rows) {
				medLabFacility=new MedLabFacility();
				medLabFacility.MedLabFacilityNum= PIn.Long  (row["MedLabFacilityNum"].ToString());
				medLabFacility.FacilityName     = PIn.String(row["FacilityName"].ToString());
				medLabFacility.Address          = PIn.String(row["Address"].ToString());
				medLabFacility.City             = PIn.String(row["City"].ToString());
				medLabFacility.State            = PIn.String(row["State"].ToString());
				medLabFacility.Zip              = PIn.String(row["Zip"].ToString());
				medLabFacility.Phone            = PIn.String(row["Phone"].ToString());
				medLabFacility.DirectorTitle    = PIn.String(row["DirectorTitle"].ToString());
				medLabFacility.DirectorLName    = PIn.String(row["DirectorLName"].ToString());
				medLabFacility.DirectorFName    = PIn.String(row["DirectorFName"].ToString());
				retVal.Add(medLabFacility);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<MedLabFacility> listMedLabFacilitys) {
			DataTable table=new DataTable("MedLabFacilitys");
			table.Columns.Add("MedLabFacilityNum");
			table.Columns.Add("FacilityName");
			table.Columns.Add("Address");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("Phone");
			table.Columns.Add("DirectorTitle");
			table.Columns.Add("DirectorLName");
			table.Columns.Add("DirectorFName");
			foreach(MedLabFacility medLabFacility in listMedLabFacilitys) {
				table.Rows.Add(new object[] {
					POut.Long  (medLabFacility.MedLabFacilityNum),
					POut.String(medLabFacility.FacilityName),
					POut.String(medLabFacility.Address),
					POut.String(medLabFacility.City),
					POut.String(medLabFacility.State),
					POut.String(medLabFacility.Zip),
					POut.String(medLabFacility.Phone),
					POut.String(medLabFacility.DirectorTitle),
					POut.String(medLabFacility.DirectorLName),
					POut.String(medLabFacility.DirectorFName),
				});
			}
			return table;
		}

		///<summary>Inserts one MedLabFacility into the database.  Returns the new priKey.</summary>
		public static long Insert(MedLabFacility medLabFacility){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				medLabFacility.MedLabFacilityNum=DbHelper.GetNextOracleKey("medlabfacility","MedLabFacilityNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(medLabFacility,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							medLabFacility.MedLabFacilityNum++;
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
				return Insert(medLabFacility,false);
			}
		}

		///<summary>Inserts one MedLabFacility into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MedLabFacility medLabFacility,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				medLabFacility.MedLabFacilityNum=ReplicationServers.GetKey("medlabfacility","MedLabFacilityNum");
			}
			string command="INSERT INTO medlabfacility (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MedLabFacilityNum,";
			}
			command+="FacilityName,Address,City,State,Zip,Phone,DirectorTitle,DirectorLName,DirectorFName) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(medLabFacility.MedLabFacilityNum)+",";
			}
			command+=
				 "'"+POut.String(medLabFacility.FacilityName)+"',"
				+"'"+POut.String(medLabFacility.Address)+"',"
				+"'"+POut.String(medLabFacility.City)+"',"
				+"'"+POut.String(medLabFacility.State)+"',"
				+"'"+POut.String(medLabFacility.Zip)+"',"
				+"'"+POut.String(medLabFacility.Phone)+"',"
				+"'"+POut.String(medLabFacility.DirectorTitle)+"',"
				+"'"+POut.String(medLabFacility.DirectorLName)+"',"
				+"'"+POut.String(medLabFacility.DirectorFName)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				medLabFacility.MedLabFacilityNum=Db.NonQ(command,true);
			}
			return medLabFacility.MedLabFacilityNum;
		}

		///<summary>Inserts one MedLabFacility into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MedLabFacility medLabFacility){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(medLabFacility,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					medLabFacility.MedLabFacilityNum=DbHelper.GetNextOracleKey("medlabfacility","MedLabFacilityNum"); //Cacheless method
				}
				return InsertNoCache(medLabFacility,true);
			}
		}

		///<summary>Inserts one MedLabFacility into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MedLabFacility medLabFacility,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO medlabfacility (";
			if(!useExistingPK && isRandomKeys) {
				medLabFacility.MedLabFacilityNum=ReplicationServers.GetKeyNoCache("medlabfacility","MedLabFacilityNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MedLabFacilityNum,";
			}
			command+="FacilityName,Address,City,State,Zip,Phone,DirectorTitle,DirectorLName,DirectorFName) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(medLabFacility.MedLabFacilityNum)+",";
			}
			command+=
				 "'"+POut.String(medLabFacility.FacilityName)+"',"
				+"'"+POut.String(medLabFacility.Address)+"',"
				+"'"+POut.String(medLabFacility.City)+"',"
				+"'"+POut.String(medLabFacility.State)+"',"
				+"'"+POut.String(medLabFacility.Zip)+"',"
				+"'"+POut.String(medLabFacility.Phone)+"',"
				+"'"+POut.String(medLabFacility.DirectorTitle)+"',"
				+"'"+POut.String(medLabFacility.DirectorLName)+"',"
				+"'"+POut.String(medLabFacility.DirectorFName)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				medLabFacility.MedLabFacilityNum=Db.NonQ(command,true);
			}
			return medLabFacility.MedLabFacilityNum;
		}

		///<summary>Updates one MedLabFacility in the database.</summary>
		public static void Update(MedLabFacility medLabFacility){
			string command="UPDATE medlabfacility SET "
				+"FacilityName     = '"+POut.String(medLabFacility.FacilityName)+"', "
				+"Address          = '"+POut.String(medLabFacility.Address)+"', "
				+"City             = '"+POut.String(medLabFacility.City)+"', "
				+"State            = '"+POut.String(medLabFacility.State)+"', "
				+"Zip              = '"+POut.String(medLabFacility.Zip)+"', "
				+"Phone            = '"+POut.String(medLabFacility.Phone)+"', "
				+"DirectorTitle    = '"+POut.String(medLabFacility.DirectorTitle)+"', "
				+"DirectorLName    = '"+POut.String(medLabFacility.DirectorLName)+"', "
				+"DirectorFName    = '"+POut.String(medLabFacility.DirectorFName)+"' "
				+"WHERE MedLabFacilityNum = "+POut.Long(medLabFacility.MedLabFacilityNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MedLabFacility in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MedLabFacility medLabFacility,MedLabFacility oldMedLabFacility){
			string command="";
			if(medLabFacility.FacilityName != oldMedLabFacility.FacilityName) {
				if(command!=""){ command+=",";}
				command+="FacilityName = '"+POut.String(medLabFacility.FacilityName)+"'";
			}
			if(medLabFacility.Address != oldMedLabFacility.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(medLabFacility.Address)+"'";
			}
			if(medLabFacility.City != oldMedLabFacility.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(medLabFacility.City)+"'";
			}
			if(medLabFacility.State != oldMedLabFacility.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(medLabFacility.State)+"'";
			}
			if(medLabFacility.Zip != oldMedLabFacility.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(medLabFacility.Zip)+"'";
			}
			if(medLabFacility.Phone != oldMedLabFacility.Phone) {
				if(command!=""){ command+=",";}
				command+="Phone = '"+POut.String(medLabFacility.Phone)+"'";
			}
			if(medLabFacility.DirectorTitle != oldMedLabFacility.DirectorTitle) {
				if(command!=""){ command+=",";}
				command+="DirectorTitle = '"+POut.String(medLabFacility.DirectorTitle)+"'";
			}
			if(medLabFacility.DirectorLName != oldMedLabFacility.DirectorLName) {
				if(command!=""){ command+=",";}
				command+="DirectorLName = '"+POut.String(medLabFacility.DirectorLName)+"'";
			}
			if(medLabFacility.DirectorFName != oldMedLabFacility.DirectorFName) {
				if(command!=""){ command+=",";}
				command+="DirectorFName = '"+POut.String(medLabFacility.DirectorFName)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE medlabfacility SET "+command
				+" WHERE MedLabFacilityNum = "+POut.Long(medLabFacility.MedLabFacilityNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one MedLabFacility from the database.</summary>
		public static void Delete(long medLabFacilityNum){
			string command="DELETE FROM medlabfacility "
				+"WHERE MedLabFacilityNum = "+POut.Long(medLabFacilityNum);
			Db.NonQ(command);
		}

	}
}