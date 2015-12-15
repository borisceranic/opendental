//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SigElementCrud {
		///<summary>Gets one SigElement object from the database using the primary key.  Returns null if not found.</summary>
		public static SigElement SelectOne(long sigElementNum){
			string command="SELECT * FROM sigelement "
				+"WHERE SigElementNum = "+POut.Long(sigElementNum);
			List<SigElement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SigElement object from the database using a query.</summary>
		public static SigElement SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigElement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SigElement objects from the database using a query.</summary>
		public static List<SigElement> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigElement> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SigElement> TableToList(DataTable table){
			List<SigElement> retVal=new List<SigElement>();
			SigElement sigElement;
			foreach(DataRow row in table.Rows) {
				sigElement=new SigElement();
				sigElement.SigElementNum   = PIn.Long  (row["SigElementNum"].ToString());
				sigElement.SigElementDefNum= PIn.Long  (row["SigElementDefNum"].ToString());
				sigElement.SignalNum       = PIn.Long  (row["SignalNum"].ToString());
				retVal.Add(sigElement);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<SigElement> listSigElements) {
			DataTable table=new DataTable("SigElements");
			table.Columns.Add("SigElementNum");
			table.Columns.Add("SigElementDefNum");
			table.Columns.Add("SignalNum");
			foreach(SigElement sigElement in listSigElements) {
				table.Rows.Add(new object[] {
					POut.Long  (sigElement.SigElementNum),
					POut.Long  (sigElement.SigElementDefNum),
					POut.Long  (sigElement.SignalNum),
				});
			}
			return table;
		}

		///<summary>Inserts one SigElement into the database.  Returns the new priKey.</summary>
		public static long Insert(SigElement sigElement){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				sigElement.SigElementNum=DbHelper.GetNextOracleKey("sigelement","SigElementNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(sigElement,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							sigElement.SigElementNum++;
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
				return Insert(sigElement,false);
			}
		}

		///<summary>Inserts one SigElement into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SigElement sigElement,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				sigElement.SigElementNum=ReplicationServers.GetKey("sigelement","SigElementNum");
			}
			string command="INSERT INTO sigelement (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SigElementNum,";
			}
			command+="SigElementDefNum,SignalNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sigElement.SigElementNum)+",";
			}
			command+=
				     POut.Long  (sigElement.SigElementDefNum)+","
				+    POut.Long  (sigElement.SignalNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigElement.SigElementNum=Db.NonQ(command,true);
			}
			return sigElement.SigElementNum;
		}

		///<summary>Inserts one SigElement into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigElement sigElement){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(sigElement,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					sigElement.SigElementNum=DbHelper.GetNextOracleKey("sigelement","SigElementNum"); //Cacheless method
				}
				return InsertNoCache(sigElement,true);
			}
		}

		///<summary>Inserts one SigElement into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigElement sigElement,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO sigelement (";
			if(!useExistingPK && isRandomKeys) {
				sigElement.SigElementNum=ReplicationServers.GetKeyNoCache("sigelement","SigElementNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SigElementNum,";
			}
			command+="SigElementDefNum,SignalNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sigElement.SigElementNum)+",";
			}
			command+=
				     POut.Long  (sigElement.SigElementDefNum)+","
				+    POut.Long  (sigElement.SignalNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigElement.SigElementNum=Db.NonQ(command,true);
			}
			return sigElement.SigElementNum;
		}

		///<summary>Updates one SigElement in the database.</summary>
		public static void Update(SigElement sigElement){
			string command="UPDATE sigelement SET "
				+"SigElementDefNum=  "+POut.Long  (sigElement.SigElementDefNum)+", "
				+"SignalNum       =  "+POut.Long  (sigElement.SignalNum)+" "
				+"WHERE SigElementNum = "+POut.Long(sigElement.SigElementNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SigElement in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SigElement sigElement,SigElement oldSigElement){
			string command="";
			if(sigElement.SigElementDefNum != oldSigElement.SigElementDefNum) {
				if(command!=""){ command+=",";}
				command+="SigElementDefNum = "+POut.Long(sigElement.SigElementDefNum)+"";
			}
			if(sigElement.SignalNum != oldSigElement.SignalNum) {
				if(command!=""){ command+=",";}
				command+="SignalNum = "+POut.Long(sigElement.SignalNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE sigelement SET "+command
				+" WHERE SigElementNum = "+POut.Long(sigElement.SigElementNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one SigElement from the database.</summary>
		public static void Delete(long sigElementNum){
			string command="DELETE FROM sigelement "
				+"WHERE SigElementNum = "+POut.Long(sigElementNum);
			Db.NonQ(command);
		}

	}
}