//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class InsFilingCodeSubtypeCrud {
		///<summary>Gets one InsFilingCodeSubtype object from the database using the primary key.  Returns null if not found.</summary>
		public static InsFilingCodeSubtype SelectOne(long insFilingCodeSubtypeNum){
			string command="SELECT * FROM insfilingcodesubtype "
				+"WHERE InsFilingCodeSubtypeNum = "+POut.Long(insFilingCodeSubtypeNum);
			List<InsFilingCodeSubtype> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one InsFilingCodeSubtype object from the database using a query.</summary>
		public static InsFilingCodeSubtype SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsFilingCodeSubtype> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of InsFilingCodeSubtype objects from the database using a query.</summary>
		public static List<InsFilingCodeSubtype> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsFilingCodeSubtype> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<InsFilingCodeSubtype> TableToList(DataTable table){
			List<InsFilingCodeSubtype> retVal=new List<InsFilingCodeSubtype>();
			InsFilingCodeSubtype insFilingCodeSubtype;
			foreach(DataRow row in table.Rows) {
				insFilingCodeSubtype=new InsFilingCodeSubtype();
				insFilingCodeSubtype.InsFilingCodeSubtypeNum= PIn.Long  (row["InsFilingCodeSubtypeNum"].ToString());
				insFilingCodeSubtype.InsFilingCodeNum       = PIn.Long  (row["InsFilingCodeNum"].ToString());
				insFilingCodeSubtype.Descript               = PIn.String(row["Descript"].ToString());
				retVal.Add(insFilingCodeSubtype);
			}
			return retVal;
		}

		///<summary>Converts a list of InsFilingCodeSubtype into a DataTable.</summary>
		public static DataTable ListToTable(List<InsFilingCodeSubtype> listInsFilingCodeSubtypes,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="InsFilingCodeSubtype";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("InsFilingCodeSubtypeNum");
			table.Columns.Add("InsFilingCodeNum");
			table.Columns.Add("Descript");
			foreach(InsFilingCodeSubtype insFilingCodeSubtype in listInsFilingCodeSubtypes) {
				table.Rows.Add(new object[] {
					POut.Long  (insFilingCodeSubtype.InsFilingCodeSubtypeNum),
					POut.Long  (insFilingCodeSubtype.InsFilingCodeNum),
					POut.String(insFilingCodeSubtype.Descript),
				});
			}
			return table;
		}

		///<summary>Inserts one InsFilingCodeSubtype into the database.  Returns the new priKey.</summary>
		public static long Insert(InsFilingCodeSubtype insFilingCodeSubtype){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				insFilingCodeSubtype.InsFilingCodeSubtypeNum=DbHelper.GetNextOracleKey("insfilingcodesubtype","InsFilingCodeSubtypeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(insFilingCodeSubtype,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							insFilingCodeSubtype.InsFilingCodeSubtypeNum++;
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
				return Insert(insFilingCodeSubtype,false);
			}
		}

		///<summary>Inserts one InsFilingCodeSubtype into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(InsFilingCodeSubtype insFilingCodeSubtype,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				insFilingCodeSubtype.InsFilingCodeSubtypeNum=ReplicationServers.GetKey("insfilingcodesubtype","InsFilingCodeSubtypeNum");
			}
			string command="INSERT INTO insfilingcodesubtype (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="InsFilingCodeSubtypeNum,";
			}
			command+="InsFilingCodeNum,Descript) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(insFilingCodeSubtype.InsFilingCodeSubtypeNum)+",";
			}
			command+=
				     POut.Long  (insFilingCodeSubtype.InsFilingCodeNum)+","
				+"'"+POut.String(insFilingCodeSubtype.Descript)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				insFilingCodeSubtype.InsFilingCodeSubtypeNum=Db.NonQ(command,true);
			}
			return insFilingCodeSubtype.InsFilingCodeSubtypeNum;
		}

		///<summary>Inserts one InsFilingCodeSubtype into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsFilingCodeSubtype insFilingCodeSubtype){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(insFilingCodeSubtype,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					insFilingCodeSubtype.InsFilingCodeSubtypeNum=DbHelper.GetNextOracleKey("insfilingcodesubtype","InsFilingCodeSubtypeNum"); //Cacheless method
				}
				return InsertNoCache(insFilingCodeSubtype,true);
			}
		}

		///<summary>Inserts one InsFilingCodeSubtype into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsFilingCodeSubtype insFilingCodeSubtype,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO insfilingcodesubtype (";
			if(!useExistingPK && isRandomKeys) {
				insFilingCodeSubtype.InsFilingCodeSubtypeNum=ReplicationServers.GetKeyNoCache("insfilingcodesubtype","InsFilingCodeSubtypeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="InsFilingCodeSubtypeNum,";
			}
			command+="InsFilingCodeNum,Descript) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(insFilingCodeSubtype.InsFilingCodeSubtypeNum)+",";
			}
			command+=
				     POut.Long  (insFilingCodeSubtype.InsFilingCodeNum)+","
				+"'"+POut.String(insFilingCodeSubtype.Descript)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				insFilingCodeSubtype.InsFilingCodeSubtypeNum=Db.NonQ(command,true);
			}
			return insFilingCodeSubtype.InsFilingCodeSubtypeNum;
		}

		///<summary>Updates one InsFilingCodeSubtype in the database.</summary>
		public static void Update(InsFilingCodeSubtype insFilingCodeSubtype){
			string command="UPDATE insfilingcodesubtype SET "
				+"InsFilingCodeNum       =  "+POut.Long  (insFilingCodeSubtype.InsFilingCodeNum)+", "
				+"Descript               = '"+POut.String(insFilingCodeSubtype.Descript)+"' "
				+"WHERE InsFilingCodeSubtypeNum = "+POut.Long(insFilingCodeSubtype.InsFilingCodeSubtypeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one InsFilingCodeSubtype in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(InsFilingCodeSubtype insFilingCodeSubtype,InsFilingCodeSubtype oldInsFilingCodeSubtype){
			string command="";
			if(insFilingCodeSubtype.InsFilingCodeNum != oldInsFilingCodeSubtype.InsFilingCodeNum) {
				if(command!=""){ command+=",";}
				command+="InsFilingCodeNum = "+POut.Long(insFilingCodeSubtype.InsFilingCodeNum)+"";
			}
			if(insFilingCodeSubtype.Descript != oldInsFilingCodeSubtype.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(insFilingCodeSubtype.Descript)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE insfilingcodesubtype SET "+command
				+" WHERE InsFilingCodeSubtypeNum = "+POut.Long(insFilingCodeSubtype.InsFilingCodeSubtypeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one InsFilingCodeSubtype from the database.</summary>
		public static void Delete(long insFilingCodeSubtypeNum){
			string command="DELETE FROM insfilingcodesubtype "
				+"WHERE InsFilingCodeSubtypeNum = "+POut.Long(insFilingCodeSubtypeNum);
			Db.NonQ(command);
		}

	}
}