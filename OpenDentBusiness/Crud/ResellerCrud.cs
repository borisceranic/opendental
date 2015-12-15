//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ResellerCrud {
		///<summary>Gets one Reseller object from the database using the primary key.  Returns null if not found.</summary>
		public static Reseller SelectOne(long resellerNum){
			string command="SELECT * FROM reseller "
				+"WHERE ResellerNum = "+POut.Long(resellerNum);
			List<Reseller> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Reseller object from the database using a query.</summary>
		public static Reseller SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Reseller> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Reseller objects from the database using a query.</summary>
		public static List<Reseller> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Reseller> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Reseller> TableToList(DataTable table){
			List<Reseller> retVal=new List<Reseller>();
			Reseller reseller;
			foreach(DataRow row in table.Rows) {
				reseller=new Reseller();
				reseller.ResellerNum     = PIn.Long  (row["ResellerNum"].ToString());
				reseller.PatNum          = PIn.Long  (row["PatNum"].ToString());
				reseller.UserName        = PIn.String(row["UserName"].ToString());
				reseller.ResellerPassword= PIn.String(row["ResellerPassword"].ToString());
				retVal.Add(reseller);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<Reseller> listResellers) {
			DataTable table=new DataTable("Resellers");
			table.Columns.Add("ResellerNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("UserName");
			table.Columns.Add("ResellerPassword");
			foreach(Reseller reseller in listResellers) {
				table.Rows.Add(new object[] {
					POut.Long  (reseller.ResellerNum),
					POut.Long  (reseller.PatNum),
					POut.String(reseller.UserName),
					POut.String(reseller.ResellerPassword),
				});
			}
			return table;
		}

		///<summary>Inserts one Reseller into the database.  Returns the new priKey.</summary>
		public static long Insert(Reseller reseller){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				reseller.ResellerNum=DbHelper.GetNextOracleKey("reseller","ResellerNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(reseller,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							reseller.ResellerNum++;
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
				return Insert(reseller,false);
			}
		}

		///<summary>Inserts one Reseller into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Reseller reseller,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				reseller.ResellerNum=ReplicationServers.GetKey("reseller","ResellerNum");
			}
			string command="INSERT INTO reseller (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ResellerNum,";
			}
			command+="PatNum,UserName,ResellerPassword) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(reseller.ResellerNum)+",";
			}
			command+=
				     POut.Long  (reseller.PatNum)+","
				+"'"+POut.String(reseller.UserName)+"',"
				+"'"+POut.String(reseller.ResellerPassword)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				reseller.ResellerNum=Db.NonQ(command,true);
			}
			return reseller.ResellerNum;
		}

		///<summary>Inserts one Reseller into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Reseller reseller){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(reseller,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					reseller.ResellerNum=DbHelper.GetNextOracleKey("reseller","ResellerNum"); //Cacheless method
				}
				return InsertNoCache(reseller,true);
			}
		}

		///<summary>Inserts one Reseller into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Reseller reseller,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO reseller (";
			if(!useExistingPK && isRandomKeys) {
				reseller.ResellerNum=ReplicationServers.GetKeyNoCache("reseller","ResellerNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ResellerNum,";
			}
			command+="PatNum,UserName,ResellerPassword) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(reseller.ResellerNum)+",";
			}
			command+=
				     POut.Long  (reseller.PatNum)+","
				+"'"+POut.String(reseller.UserName)+"',"
				+"'"+POut.String(reseller.ResellerPassword)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				reseller.ResellerNum=Db.NonQ(command,true);
			}
			return reseller.ResellerNum;
		}

		///<summary>Updates one Reseller in the database.</summary>
		public static void Update(Reseller reseller){
			string command="UPDATE reseller SET "
				+"PatNum          =  "+POut.Long  (reseller.PatNum)+", "
				+"UserName        = '"+POut.String(reseller.UserName)+"', "
				+"ResellerPassword= '"+POut.String(reseller.ResellerPassword)+"' "
				+"WHERE ResellerNum = "+POut.Long(reseller.ResellerNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Reseller in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Reseller reseller,Reseller oldReseller){
			string command="";
			if(reseller.PatNum != oldReseller.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(reseller.PatNum)+"";
			}
			if(reseller.UserName != oldReseller.UserName) {
				if(command!=""){ command+=",";}
				command+="UserName = '"+POut.String(reseller.UserName)+"'";
			}
			if(reseller.ResellerPassword != oldReseller.ResellerPassword) {
				if(command!=""){ command+=",";}
				command+="ResellerPassword = '"+POut.String(reseller.ResellerPassword)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE reseller SET "+command
				+" WHERE ResellerNum = "+POut.Long(reseller.ResellerNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Reseller from the database.</summary>
		public static void Delete(long resellerNum){
			string command="DELETE FROM reseller "
				+"WHERE ResellerNum = "+POut.Long(resellerNum);
			Db.NonQ(command);
		}

	}
}