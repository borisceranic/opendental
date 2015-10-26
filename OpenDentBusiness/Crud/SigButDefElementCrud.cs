//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SigButDefElementCrud {
		///<summary>Gets one SigButDefElement object from the database using the primary key.  Returns null if not found.</summary>
		public static SigButDefElement SelectOne(long elementNum){
			string command="SELECT * FROM sigbutdefelement "
				+"WHERE ElementNum = "+POut.Long(elementNum);
			List<SigButDefElement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SigButDefElement object from the database using a query.</summary>
		public static SigButDefElement SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigButDefElement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SigButDefElement objects from the database using a query.</summary>
		public static List<SigButDefElement> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigButDefElement> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SigButDefElement> TableToList(DataTable table){
			List<SigButDefElement> retVal=new List<SigButDefElement>();
			SigButDefElement sigButDefElement;
			foreach(DataRow row in table.Rows) {
				sigButDefElement=new SigButDefElement();
				sigButDefElement.ElementNum      = PIn.Long  (row["ElementNum"].ToString());
				sigButDefElement.SigButDefNum    = PIn.Long  (row["SigButDefNum"].ToString());
				sigButDefElement.SigElementDefNum= PIn.Long  (row["SigElementDefNum"].ToString());
				retVal.Add(sigButDefElement);
			}
			return retVal;
		}

		///<summary>Inserts one SigButDefElement into the database.  Returns the new priKey.</summary>
		public static long Insert(SigButDefElement sigButDefElement){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				sigButDefElement.ElementNum=DbHelper.GetNextOracleKey("sigbutdefelement","ElementNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(sigButDefElement,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							sigButDefElement.ElementNum++;
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
				return Insert(sigButDefElement,false);
			}
		}

		///<summary>Inserts one SigButDefElement into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SigButDefElement sigButDefElement,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				sigButDefElement.ElementNum=ReplicationServers.GetKey("sigbutdefelement","ElementNum");
			}
			string command="INSERT INTO sigbutdefelement (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ElementNum,";
			}
			command+="SigButDefNum,SigElementDefNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sigButDefElement.ElementNum)+",";
			}
			command+=
				     POut.Long  (sigButDefElement.SigButDefNum)+","
				+    POut.Long  (sigButDefElement.SigElementDefNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigButDefElement.ElementNum=Db.NonQ(command,true);
			}
			return sigButDefElement.ElementNum;
		}

		///<summary>Inserts one SigButDefElement into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigButDefElement sigButDefElement){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(sigButDefElement,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					sigButDefElement.ElementNum=DbHelper.GetNextOracleKey("sigbutdefelement","ElementNum"); //Cacheless method
				}
				return InsertNoCache(sigButDefElement,true);
			}
		}

		///<summary>Inserts one SigButDefElement into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigButDefElement sigButDefElement,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO sigbutdefelement (";
			if(!useExistingPK && isRandomKeys) {
				sigButDefElement.ElementNum=ReplicationServers.GetKeyNoCache("sigbutdefelement","ElementNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ElementNum,";
			}
			command+="SigButDefNum,SigElementDefNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sigButDefElement.ElementNum)+",";
			}
			command+=
				     POut.Long  (sigButDefElement.SigButDefNum)+","
				+    POut.Long  (sigButDefElement.SigElementDefNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigButDefElement.ElementNum=Db.NonQ(command,true);
			}
			return sigButDefElement.ElementNum;
		}

		///<summary>Updates one SigButDefElement in the database.</summary>
		public static void Update(SigButDefElement sigButDefElement){
			string command="UPDATE sigbutdefelement SET "
				+"SigButDefNum    =  "+POut.Long  (sigButDefElement.SigButDefNum)+", "
				+"SigElementDefNum=  "+POut.Long  (sigButDefElement.SigElementDefNum)+" "
				+"WHERE ElementNum = "+POut.Long(sigButDefElement.ElementNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SigButDefElement in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SigButDefElement sigButDefElement,SigButDefElement oldSigButDefElement){
			string command="";
			if(sigButDefElement.SigButDefNum != oldSigButDefElement.SigButDefNum) {
				if(command!=""){ command+=",";}
				command+="SigButDefNum = "+POut.Long(sigButDefElement.SigButDefNum)+"";
			}
			if(sigButDefElement.SigElementDefNum != oldSigButDefElement.SigElementDefNum) {
				if(command!=""){ command+=",";}
				command+="SigElementDefNum = "+POut.Long(sigButDefElement.SigElementDefNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE sigbutdefelement SET "+command
				+" WHERE ElementNum = "+POut.Long(sigButDefElement.ElementNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one SigButDefElement from the database.</summary>
		public static void Delete(long elementNum){
			string command="DELETE FROM sigbutdefelement "
				+"WHERE ElementNum = "+POut.Long(elementNum);
			Db.NonQ(command);
		}

	}
}