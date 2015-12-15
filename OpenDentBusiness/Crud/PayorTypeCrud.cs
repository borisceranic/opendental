//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PayorTypeCrud {
		///<summary>Gets one PayorType object from the database using the primary key.  Returns null if not found.</summary>
		public static PayorType SelectOne(long payorTypeNum){
			string command="SELECT * FROM payortype "
				+"WHERE PayorTypeNum = "+POut.Long(payorTypeNum);
			List<PayorType> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PayorType object from the database using a query.</summary>
		public static PayorType SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayorType> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PayorType objects from the database using a query.</summary>
		public static List<PayorType> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayorType> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PayorType> TableToList(DataTable table){
			List<PayorType> retVal=new List<PayorType>();
			PayorType payorType;
			foreach(DataRow row in table.Rows) {
				payorType=new PayorType();
				payorType.PayorTypeNum= PIn.Long  (row["PayorTypeNum"].ToString());
				payorType.PatNum      = PIn.Long  (row["PatNum"].ToString());
				payorType.DateStart   = PIn.Date  (row["DateStart"].ToString());
				payorType.SopCode     = PIn.String(row["SopCode"].ToString());
				payorType.Note        = PIn.String(row["Note"].ToString());
				retVal.Add(payorType);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<PayorType> listPayorTypes) {
			DataTable table=new DataTable("PayorTypes");
			table.Columns.Add("PayorTypeNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("DateStart");
			table.Columns.Add("SopCode");
			table.Columns.Add("Note");
			foreach(PayorType payorType in listPayorTypes) {
				table.Rows.Add(new object[] {
					POut.Long  (payorType.PayorTypeNum),
					POut.Long  (payorType.PatNum),
					POut.Date  (payorType.DateStart),
					POut.String(payorType.SopCode),
					POut.String(payorType.Note),
				});
			}
			return table;
		}

		///<summary>Inserts one PayorType into the database.  Returns the new priKey.</summary>
		public static long Insert(PayorType payorType){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				payorType.PayorTypeNum=DbHelper.GetNextOracleKey("payortype","PayorTypeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(payorType,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							payorType.PayorTypeNum++;
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
				return Insert(payorType,false);
			}
		}

		///<summary>Inserts one PayorType into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PayorType payorType,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				payorType.PayorTypeNum=ReplicationServers.GetKey("payortype","PayorTypeNum");
			}
			string command="INSERT INTO payortype (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PayorTypeNum,";
			}
			command+="PatNum,DateStart,SopCode,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(payorType.PayorTypeNum)+",";
			}
			command+=
				     POut.Long  (payorType.PatNum)+","
				+    POut.Date  (payorType.DateStart)+","
				+"'"+POut.String(payorType.SopCode)+"',"
				+"'"+POut.String(payorType.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				payorType.PayorTypeNum=Db.NonQ(command,true);
			}
			return payorType.PayorTypeNum;
		}

		///<summary>Inserts one PayorType into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayorType payorType){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(payorType,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					payorType.PayorTypeNum=DbHelper.GetNextOracleKey("payortype","PayorTypeNum"); //Cacheless method
				}
				return InsertNoCache(payorType,true);
			}
		}

		///<summary>Inserts one PayorType into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayorType payorType,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO payortype (";
			if(!useExistingPK && isRandomKeys) {
				payorType.PayorTypeNum=ReplicationServers.GetKeyNoCache("payortype","PayorTypeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PayorTypeNum,";
			}
			command+="PatNum,DateStart,SopCode,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(payorType.PayorTypeNum)+",";
			}
			command+=
				     POut.Long  (payorType.PatNum)+","
				+    POut.Date  (payorType.DateStart)+","
				+"'"+POut.String(payorType.SopCode)+"',"
				+"'"+POut.String(payorType.Note)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				payorType.PayorTypeNum=Db.NonQ(command,true);
			}
			return payorType.PayorTypeNum;
		}

		///<summary>Updates one PayorType in the database.</summary>
		public static void Update(PayorType payorType){
			string command="UPDATE payortype SET "
				+"PatNum      =  "+POut.Long  (payorType.PatNum)+", "
				+"DateStart   =  "+POut.Date  (payorType.DateStart)+", "
				+"SopCode     = '"+POut.String(payorType.SopCode)+"', "
				+"Note        = '"+POut.String(payorType.Note)+"' "
				+"WHERE PayorTypeNum = "+POut.Long(payorType.PayorTypeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PayorType in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PayorType payorType,PayorType oldPayorType){
			string command="";
			if(payorType.PatNum != oldPayorType.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(payorType.PatNum)+"";
			}
			if(payorType.DateStart.Date != oldPayorType.DateStart.Date) {
				if(command!=""){ command+=",";}
				command+="DateStart = "+POut.Date(payorType.DateStart)+"";
			}
			if(payorType.SopCode != oldPayorType.SopCode) {
				if(command!=""){ command+=",";}
				command+="SopCode = '"+POut.String(payorType.SopCode)+"'";
			}
			if(payorType.Note != oldPayorType.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(payorType.Note)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE payortype SET "+command
				+" WHERE PayorTypeNum = "+POut.Long(payorType.PayorTypeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one PayorType from the database.</summary>
		public static void Delete(long payorTypeNum){
			string command="DELETE FROM payortype "
				+"WHERE PayorTypeNum = "+POut.Long(payorTypeNum);
			Db.NonQ(command);
		}

	}
}