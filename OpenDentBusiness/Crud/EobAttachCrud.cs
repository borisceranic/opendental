//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EobAttachCrud {
		///<summary>Gets one EobAttach object from the database using the primary key.  Returns null if not found.</summary>
		public static EobAttach SelectOne(long eobAttachNum){
			string command="SELECT * FROM eobattach "
				+"WHERE EobAttachNum = "+POut.Long(eobAttachNum);
			List<EobAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EobAttach object from the database using a query.</summary>
		public static EobAttach SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EobAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EobAttach objects from the database using a query.</summary>
		public static List<EobAttach> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EobAttach> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EobAttach> TableToList(DataTable table){
			List<EobAttach> retVal=new List<EobAttach>();
			EobAttach eobAttach;
			foreach(DataRow row in table.Rows) {
				eobAttach=new EobAttach();
				eobAttach.EobAttachNum   = PIn.Long  (row["EobAttachNum"].ToString());
				eobAttach.ClaimPaymentNum= PIn.Long  (row["ClaimPaymentNum"].ToString());
				eobAttach.DateTCreated   = PIn.DateT (row["DateTCreated"].ToString());
				eobAttach.FileName       = PIn.String(row["FileName"].ToString());
				eobAttach.RawBase64      = PIn.String(row["RawBase64"].ToString());
				retVal.Add(eobAttach);
			}
			return retVal;
		}

		///<summary>Converts a list of EobAttach into a DataTable.</summary>
		public static DataTable ListToTable(List<EobAttach> listEobAttachs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EobAttach";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EobAttachNum");
			table.Columns.Add("ClaimPaymentNum");
			table.Columns.Add("DateTCreated");
			table.Columns.Add("FileName");
			table.Columns.Add("RawBase64");
			foreach(EobAttach eobAttach in listEobAttachs) {
				table.Rows.Add(new object[] {
					POut.Long  (eobAttach.EobAttachNum),
					POut.Long  (eobAttach.ClaimPaymentNum),
					POut.DateT (eobAttach.DateTCreated,false),
					            eobAttach.FileName,
					            eobAttach.RawBase64,
				});
			}
			return table;
		}

		///<summary>Inserts one EobAttach into the database.  Returns the new priKey.</summary>
		public static long Insert(EobAttach eobAttach){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				eobAttach.EobAttachNum=DbHelper.GetNextOracleKey("eobattach","EobAttachNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(eobAttach,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							eobAttach.EobAttachNum++;
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
				return Insert(eobAttach,false);
			}
		}

		///<summary>Inserts one EobAttach into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EobAttach eobAttach,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				eobAttach.EobAttachNum=ReplicationServers.GetKey("eobattach","EobAttachNum");
			}
			string command="INSERT INTO eobattach (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EobAttachNum,";
			}
			command+="ClaimPaymentNum,DateTCreated,FileName,RawBase64) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(eobAttach.EobAttachNum)+",";
			}
			command+=
				     POut.Long  (eobAttach.ClaimPaymentNum)+","
				+    POut.DateT (eobAttach.DateTCreated)+","
				+"'"+POut.String(eobAttach.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64)";
			if(eobAttach.RawBase64==null) {
				eobAttach.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,eobAttach.RawBase64);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramRawBase64);
			}
			else {
				eobAttach.EobAttachNum=Db.NonQ(command,true,paramRawBase64);
			}
			return eobAttach.EobAttachNum;
		}

		///<summary>Inserts one EobAttach into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EobAttach eobAttach){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(eobAttach,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					eobAttach.EobAttachNum=DbHelper.GetNextOracleKey("eobattach","EobAttachNum"); //Cacheless method
				}
				return InsertNoCache(eobAttach,true);
			}
		}

		///<summary>Inserts one EobAttach into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EobAttach eobAttach,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO eobattach (";
			if(!useExistingPK && isRandomKeys) {
				eobAttach.EobAttachNum=ReplicationServers.GetKeyNoCache("eobattach","EobAttachNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EobAttachNum,";
			}
			command+="ClaimPaymentNum,DateTCreated,FileName,RawBase64) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(eobAttach.EobAttachNum)+",";
			}
			command+=
				     POut.Long  (eobAttach.ClaimPaymentNum)+","
				+    POut.DateT (eobAttach.DateTCreated)+","
				+"'"+POut.String(eobAttach.FileName)+"',"
				+    DbHelper.ParamChar+"paramRawBase64)";
			if(eobAttach.RawBase64==null) {
				eobAttach.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,eobAttach.RawBase64);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramRawBase64);
			}
			else {
				eobAttach.EobAttachNum=Db.NonQ(command,true,paramRawBase64);
			}
			return eobAttach.EobAttachNum;
		}

		///<summary>Updates one EobAttach in the database.</summary>
		public static void Update(EobAttach eobAttach){
			string command="UPDATE eobattach SET "
				+"ClaimPaymentNum=  "+POut.Long  (eobAttach.ClaimPaymentNum)+", "
				+"DateTCreated   =  "+POut.DateT (eobAttach.DateTCreated)+", "
				+"FileName       = '"+POut.String(eobAttach.FileName)+"', "
				+"RawBase64      =  "+DbHelper.ParamChar+"paramRawBase64 "
				+"WHERE EobAttachNum = "+POut.Long(eobAttach.EobAttachNum);
			if(eobAttach.RawBase64==null) {
				eobAttach.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,eobAttach.RawBase64);
			Db.NonQ(command,paramRawBase64);
		}

		///<summary>Updates one EobAttach in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EobAttach eobAttach,EobAttach oldEobAttach){
			string command="";
			if(eobAttach.ClaimPaymentNum != oldEobAttach.ClaimPaymentNum) {
				if(command!=""){ command+=",";}
				command+="ClaimPaymentNum = "+POut.Long(eobAttach.ClaimPaymentNum)+"";
			}
			if(eobAttach.DateTCreated != oldEobAttach.DateTCreated) {
				if(command!=""){ command+=",";}
				command+="DateTCreated = "+POut.DateT(eobAttach.DateTCreated)+"";
			}
			if(eobAttach.FileName != oldEobAttach.FileName) {
				if(command!=""){ command+=",";}
				command+="FileName = '"+POut.String(eobAttach.FileName)+"'";
			}
			if(eobAttach.RawBase64 != oldEobAttach.RawBase64) {
				if(command!=""){ command+=",";}
				command+="RawBase64 = "+DbHelper.ParamChar+"paramRawBase64";
			}
			if(command==""){
				return false;
			}
			if(eobAttach.RawBase64==null) {
				eobAttach.RawBase64="";
			}
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,eobAttach.RawBase64);
			command="UPDATE eobattach SET "+command
				+" WHERE EobAttachNum = "+POut.Long(eobAttach.EobAttachNum);
			Db.NonQ(command,paramRawBase64);
			return true;
		}

		///<summary>Returns true if Update(EobAttach,EobAttach) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EobAttach eobAttach,EobAttach oldEobAttach) {
			if(eobAttach.ClaimPaymentNum != oldEobAttach.ClaimPaymentNum) {
				return true;
			}
			if(eobAttach.DateTCreated != oldEobAttach.DateTCreated) {
				return true;
			}
			if(eobAttach.FileName != oldEobAttach.FileName) {
				return true;
			}
			if(eobAttach.RawBase64 != oldEobAttach.RawBase64) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EobAttach from the database.</summary>
		public static void Delete(long eobAttachNum){
			string command="DELETE FROM eobattach "
				+"WHERE EobAttachNum = "+POut.Long(eobAttachNum);
			Db.NonQ(command);
		}

	}
}