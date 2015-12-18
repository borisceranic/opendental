//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CountyCrud {
		///<summary>Gets one County object from the database using the primary key.  Returns null if not found.</summary>
		public static County SelectOne(long countyNum){
			string command="SELECT * FROM county "
				+"WHERE CountyNum = "+POut.Long(countyNum);
			List<County> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one County object from the database using a query.</summary>
		public static County SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<County> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of County objects from the database using a query.</summary>
		public static List<County> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<County> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<County> TableToList(DataTable table){
			List<County> retVal=new List<County>();
			County county;
			foreach(DataRow row in table.Rows) {
				county=new County();
				county.CountyNum = PIn.Long  (row["CountyNum"].ToString());
				county.CountyName= PIn.String(row["CountyName"].ToString());
				county.CountyCode= PIn.String(row["CountyCode"].ToString());
				retVal.Add(county);
			}
			return retVal;
		}

		///<summary>Converts a list of County into a DataTable.</summary>
		public static DataTable ListToTable(List<County> listCountys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="County";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CountyNum");
			table.Columns.Add("CountyName");
			table.Columns.Add("CountyCode");
			foreach(County county in listCountys) {
				table.Rows.Add(new object[] {
					POut.Long  (county.CountyNum),
					POut.String(county.CountyName),
					POut.String(county.CountyCode),
				});
			}
			return table;
		}

		///<summary>Inserts one County into the database.  Returns the new priKey.</summary>
		public static long Insert(County county){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				county.CountyNum=DbHelper.GetNextOracleKey("county","CountyNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(county,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							county.CountyNum++;
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
				return Insert(county,false);
			}
		}

		///<summary>Inserts one County into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(County county,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				county.CountyNum=ReplicationServers.GetKey("county","CountyNum");
			}
			string command="INSERT INTO county (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CountyNum,";
			}
			command+="CountyName,CountyCode) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(county.CountyNum)+",";
			}
			command+=
				 "'"+POut.String(county.CountyName)+"',"
				+"'"+POut.String(county.CountyCode)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				county.CountyNum=Db.NonQ(command,true);
			}
			return county.CountyNum;
		}

		///<summary>Inserts one County into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(County county){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(county,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					county.CountyNum=DbHelper.GetNextOracleKey("county","CountyNum"); //Cacheless method
				}
				return InsertNoCache(county,true);
			}
		}

		///<summary>Inserts one County into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(County county,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO county (";
			if(!useExistingPK && isRandomKeys) {
				county.CountyNum=ReplicationServers.GetKeyNoCache("county","CountyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CountyNum,";
			}
			command+="CountyName,CountyCode) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(county.CountyNum)+",";
			}
			command+=
				 "'"+POut.String(county.CountyName)+"',"
				+"'"+POut.String(county.CountyCode)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				county.CountyNum=Db.NonQ(command,true);
			}
			return county.CountyNum;
		}

		///<summary>Updates one County in the database.</summary>
		public static void Update(County county){
			string command="UPDATE county SET "
				+"CountyName= '"+POut.String(county.CountyName)+"', "
				+"CountyCode= '"+POut.String(county.CountyCode)+"' "
				+"WHERE CountyNum = "+POut.Long(county.CountyNum);
			Db.NonQ(command);
		}

		///<summary>Updates one County in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(County county,County oldCounty){
			string command="";
			if(county.CountyName != oldCounty.CountyName) {
				if(command!=""){ command+=",";}
				command+="CountyName = '"+POut.String(county.CountyName)+"'";
			}
			if(county.CountyCode != oldCounty.CountyCode) {
				if(command!=""){ command+=",";}
				command+="CountyCode = '"+POut.String(county.CountyCode)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE county SET "+command
				+" WHERE CountyNum = "+POut.Long(county.CountyNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one County from the database.</summary>
		public static void Delete(long countyNum){
			string command="DELETE FROM county "
				+"WHERE CountyNum = "+POut.Long(countyNum);
			Db.NonQ(command);
		}

	}
}