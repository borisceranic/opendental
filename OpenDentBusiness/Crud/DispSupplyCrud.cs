//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class DispSupplyCrud {
		///<summary>Gets one DispSupply object from the database using the primary key.  Returns null if not found.</summary>
		public static DispSupply SelectOne(long dispSupplyNum){
			string command="SELECT * FROM dispsupply "
				+"WHERE DispSupplyNum = "+POut.Long(dispSupplyNum);
			List<DispSupply> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one DispSupply object from the database using a query.</summary>
		public static DispSupply SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DispSupply> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of DispSupply objects from the database using a query.</summary>
		public static List<DispSupply> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DispSupply> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<DispSupply> TableToList(DataTable table){
			List<DispSupply> retVal=new List<DispSupply>();
			DispSupply dispSupply;
			foreach(DataRow row in table.Rows) {
				dispSupply=new DispSupply();
				dispSupply.DispSupplyNum= PIn.Long  (row["DispSupplyNum"].ToString());
				dispSupply.SupplyNum    = PIn.Long  (row["SupplyNum"].ToString());
				dispSupply.ProvNum      = PIn.Long  (row["ProvNum"].ToString());
				dispSupply.DateDispensed= PIn.Date  (row["DateDispensed"].ToString());
				dispSupply.DispQuantity = PIn.Float (row["DispQuantity"].ToString());
				dispSupply.Note         = PIn.String(row["Note"].ToString());
				retVal.Add(dispSupply);
			}
			return retVal;
		}

		///<summary>Converts a list of DispSupply into a DataTable.</summary>
		public static DataTable ListToTable(List<DispSupply> listDispSupplys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="DispSupply";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("DispSupplyNum");
			table.Columns.Add("SupplyNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("DateDispensed");
			table.Columns.Add("DispQuantity");
			table.Columns.Add("Note");
			foreach(DispSupply dispSupply in listDispSupplys) {
				table.Rows.Add(new object[] {
					POut.Long  (dispSupply.DispSupplyNum),
					POut.Long  (dispSupply.SupplyNum),
					POut.Long  (dispSupply.ProvNum),
					POut.DateT (dispSupply.DateDispensed,false),
					POut.Float (dispSupply.DispQuantity),
					            dispSupply.Note,
				});
			}
			return table;
		}

		///<summary>Inserts one DispSupply into the database.  Returns the new priKey.</summary>
		public static long Insert(DispSupply dispSupply){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				dispSupply.DispSupplyNum=DbHelper.GetNextOracleKey("dispsupply","DispSupplyNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(dispSupply,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							dispSupply.DispSupplyNum++;
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
				return Insert(dispSupply,false);
			}
		}

		///<summary>Inserts one DispSupply into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(DispSupply dispSupply,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				dispSupply.DispSupplyNum=ReplicationServers.GetKey("dispsupply","DispSupplyNum");
			}
			string command="INSERT INTO dispsupply (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="DispSupplyNum,";
			}
			command+="SupplyNum,ProvNum,DateDispensed,DispQuantity,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(dispSupply.DispSupplyNum)+",";
			}
			command+=
				     POut.Long  (dispSupply.SupplyNum)+","
				+    POut.Long  (dispSupply.ProvNum)+","
				+    POut.Date  (dispSupply.DateDispensed)+","
				+    POut.Float (dispSupply.DispQuantity)+","
				+    DbHelper.ParamChar+"paramNote)";
			if(dispSupply.Note==null) {
				dispSupply.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringNote(dispSupply.Note));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				dispSupply.DispSupplyNum=Db.NonQ(command,true,paramNote);
			}
			return dispSupply.DispSupplyNum;
		}

		///<summary>Inserts one DispSupply into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DispSupply dispSupply){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(dispSupply,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					dispSupply.DispSupplyNum=DbHelper.GetNextOracleKey("dispsupply","DispSupplyNum"); //Cacheless method
				}
				return InsertNoCache(dispSupply,true);
			}
		}

		///<summary>Inserts one DispSupply into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DispSupply dispSupply,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO dispsupply (";
			if(!useExistingPK && isRandomKeys) {
				dispSupply.DispSupplyNum=ReplicationServers.GetKeyNoCache("dispsupply","DispSupplyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="DispSupplyNum,";
			}
			command+="SupplyNum,ProvNum,DateDispensed,DispQuantity,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(dispSupply.DispSupplyNum)+",";
			}
			command+=
				     POut.Long  (dispSupply.SupplyNum)+","
				+    POut.Long  (dispSupply.ProvNum)+","
				+    POut.Date  (dispSupply.DateDispensed)+","
				+    POut.Float (dispSupply.DispQuantity)+","
				+    DbHelper.ParamChar+"paramNote)";
			if(dispSupply.Note==null) {
				dispSupply.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringNote(dispSupply.Note));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				dispSupply.DispSupplyNum=Db.NonQ(command,true,paramNote);
			}
			return dispSupply.DispSupplyNum;
		}

		///<summary>Updates one DispSupply in the database.</summary>
		public static void Update(DispSupply dispSupply){
			string command="UPDATE dispsupply SET "
				+"SupplyNum    =  "+POut.Long  (dispSupply.SupplyNum)+", "
				+"ProvNum      =  "+POut.Long  (dispSupply.ProvNum)+", "
				+"DateDispensed=  "+POut.Date  (dispSupply.DateDispensed)+", "
				+"DispQuantity =  "+POut.Float (dispSupply.DispQuantity)+", "
				+"Note         =  "+DbHelper.ParamChar+"paramNote "
				+"WHERE DispSupplyNum = "+POut.Long(dispSupply.DispSupplyNum);
			if(dispSupply.Note==null) {
				dispSupply.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringNote(dispSupply.Note));
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one DispSupply in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(DispSupply dispSupply,DispSupply oldDispSupply){
			string command="";
			if(dispSupply.SupplyNum != oldDispSupply.SupplyNum) {
				if(command!=""){ command+=",";}
				command+="SupplyNum = "+POut.Long(dispSupply.SupplyNum)+"";
			}
			if(dispSupply.ProvNum != oldDispSupply.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(dispSupply.ProvNum)+"";
			}
			if(dispSupply.DateDispensed.Date != oldDispSupply.DateDispensed.Date) {
				if(command!=""){ command+=",";}
				command+="DateDispensed = "+POut.Date(dispSupply.DateDispensed)+"";
			}
			if(dispSupply.DispQuantity != oldDispSupply.DispQuantity) {
				if(command!=""){ command+=",";}
				command+="DispQuantity = "+POut.Float(dispSupply.DispQuantity)+"";
			}
			if(dispSupply.Note != oldDispSupply.Note) {
				if(command!=""){ command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(command==""){
				return false;
			}
			if(dispSupply.Note==null) {
				dispSupply.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,POut.StringNote(dispSupply.Note));
			command="UPDATE dispsupply SET "+command
				+" WHERE DispSupplyNum = "+POut.Long(dispSupply.DispSupplyNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Returns true if Update(DispSupply,DispSupply) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(DispSupply dispSupply,DispSupply oldDispSupply) {
			if(dispSupply.SupplyNum != oldDispSupply.SupplyNum) {
				return true;
			}
			if(dispSupply.ProvNum != oldDispSupply.ProvNum) {
				return true;
			}
			if(dispSupply.DateDispensed.Date != oldDispSupply.DateDispensed.Date) {
				return true;
			}
			if(dispSupply.DispQuantity != oldDispSupply.DispQuantity) {
				return true;
			}
			if(dispSupply.Note != oldDispSupply.Note) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one DispSupply from the database.</summary>
		public static void Delete(long dispSupplyNum){
			string command="DELETE FROM dispsupply "
				+"WHERE DispSupplyNum = "+POut.Long(dispSupplyNum);
			Db.NonQ(command);
		}

	}
}