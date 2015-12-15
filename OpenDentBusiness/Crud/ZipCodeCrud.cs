//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ZipCodeCrud {
		///<summary>Gets one ZipCode object from the database using the primary key.  Returns null if not found.</summary>
		public static ZipCode SelectOne(long zipCodeNum){
			string command="SELECT * FROM zipcode "
				+"WHERE ZipCodeNum = "+POut.Long(zipCodeNum);
			List<ZipCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ZipCode object from the database using a query.</summary>
		public static ZipCode SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ZipCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ZipCode objects from the database using a query.</summary>
		public static List<ZipCode> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ZipCode> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ZipCode> TableToList(DataTable table){
			List<ZipCode> retVal=new List<ZipCode>();
			ZipCode zipCode;
			foreach(DataRow row in table.Rows) {
				zipCode=new ZipCode();
				zipCode.ZipCodeNum   = PIn.Long  (row["ZipCodeNum"].ToString());
				zipCode.ZipCodeDigits= PIn.String(row["ZipCodeDigits"].ToString());
				zipCode.City         = PIn.String(row["City"].ToString());
				zipCode.State        = PIn.String(row["State"].ToString());
				zipCode.IsFrequent   = PIn.Bool  (row["IsFrequent"].ToString());
				retVal.Add(zipCode);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ZipCode> listZipCodes) {
			DataTable table=new DataTable("ZipCodes");
			table.Columns.Add("ZipCodeNum");
			table.Columns.Add("ZipCodeDigits");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("IsFrequent");
			foreach(ZipCode zipCode in listZipCodes) {
				table.Rows.Add(new object[] {
					POut.Long  (zipCode.ZipCodeNum),
					POut.String(zipCode.ZipCodeDigits),
					POut.String(zipCode.City),
					POut.String(zipCode.State),
					POut.Bool  (zipCode.IsFrequent),
				});
			}
			return table;
		}

		///<summary>Inserts one ZipCode into the database.  Returns the new priKey.</summary>
		public static long Insert(ZipCode zipCode){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				zipCode.ZipCodeNum=DbHelper.GetNextOracleKey("zipcode","ZipCodeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(zipCode,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							zipCode.ZipCodeNum++;
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
				return Insert(zipCode,false);
			}
		}

		///<summary>Inserts one ZipCode into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ZipCode zipCode,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				zipCode.ZipCodeNum=ReplicationServers.GetKey("zipcode","ZipCodeNum");
			}
			string command="INSERT INTO zipcode (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ZipCodeNum,";
			}
			command+="ZipCodeDigits,City,State,IsFrequent) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(zipCode.ZipCodeNum)+",";
			}
			command+=
				 "'"+POut.String(zipCode.ZipCodeDigits)+"',"
				+"'"+POut.String(zipCode.City)+"',"
				+"'"+POut.String(zipCode.State)+"',"
				+    POut.Bool  (zipCode.IsFrequent)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				zipCode.ZipCodeNum=Db.NonQ(command,true);
			}
			return zipCode.ZipCodeNum;
		}

		///<summary>Inserts one ZipCode into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ZipCode zipCode){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(zipCode,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					zipCode.ZipCodeNum=DbHelper.GetNextOracleKey("zipcode","ZipCodeNum"); //Cacheless method
				}
				return InsertNoCache(zipCode,true);
			}
		}

		///<summary>Inserts one ZipCode into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ZipCode zipCode,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO zipcode (";
			if(!useExistingPK && isRandomKeys) {
				zipCode.ZipCodeNum=ReplicationServers.GetKeyNoCache("zipcode","ZipCodeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ZipCodeNum,";
			}
			command+="ZipCodeDigits,City,State,IsFrequent) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(zipCode.ZipCodeNum)+",";
			}
			command+=
				 "'"+POut.String(zipCode.ZipCodeDigits)+"',"
				+"'"+POut.String(zipCode.City)+"',"
				+"'"+POut.String(zipCode.State)+"',"
				+    POut.Bool  (zipCode.IsFrequent)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				zipCode.ZipCodeNum=Db.NonQ(command,true);
			}
			return zipCode.ZipCodeNum;
		}

		///<summary>Updates one ZipCode in the database.</summary>
		public static void Update(ZipCode zipCode){
			string command="UPDATE zipcode SET "
				+"ZipCodeDigits= '"+POut.String(zipCode.ZipCodeDigits)+"', "
				+"City         = '"+POut.String(zipCode.City)+"', "
				+"State        = '"+POut.String(zipCode.State)+"', "
				+"IsFrequent   =  "+POut.Bool  (zipCode.IsFrequent)+" "
				+"WHERE ZipCodeNum = "+POut.Long(zipCode.ZipCodeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ZipCode in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ZipCode zipCode,ZipCode oldZipCode){
			string command="";
			if(zipCode.ZipCodeDigits != oldZipCode.ZipCodeDigits) {
				if(command!=""){ command+=",";}
				command+="ZipCodeDigits = '"+POut.String(zipCode.ZipCodeDigits)+"'";
			}
			if(zipCode.City != oldZipCode.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(zipCode.City)+"'";
			}
			if(zipCode.State != oldZipCode.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(zipCode.State)+"'";
			}
			if(zipCode.IsFrequent != oldZipCode.IsFrequent) {
				if(command!=""){ command+=",";}
				command+="IsFrequent = "+POut.Bool(zipCode.IsFrequent)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE zipcode SET "+command
				+" WHERE ZipCodeNum = "+POut.Long(zipCode.ZipCodeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ZipCode from the database.</summary>
		public static void Delete(long zipCodeNum){
			string command="DELETE FROM zipcode "
				+"WHERE ZipCodeNum = "+POut.Long(zipCodeNum);
			Db.NonQ(command);
		}

	}
}