//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RegistrationKeyCrud {
		///<summary>Gets one RegistrationKey object from the database using the primary key.  Returns null if not found.</summary>
		public static RegistrationKey SelectOne(long registrationKeyNum){
			string command="SELECT * FROM registrationkey "
				+"WHERE RegistrationKeyNum = "+POut.Long(registrationKeyNum);
			List<RegistrationKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RegistrationKey object from the database using a query.</summary>
		public static RegistrationKey SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RegistrationKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RegistrationKey objects from the database using a query.</summary>
		public static List<RegistrationKey> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RegistrationKey> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RegistrationKey> TableToList(DataTable table){
			List<RegistrationKey> retVal=new List<RegistrationKey>();
			RegistrationKey registrationKey;
			foreach(DataRow row in table.Rows) {
				registrationKey=new RegistrationKey();
				registrationKey.RegistrationKeyNum= PIn.Long  (row["RegistrationKeyNum"].ToString());
				registrationKey.PatNum            = PIn.Long  (row["PatNum"].ToString());
				registrationKey.RegKey            = PIn.String(row["RegKey"].ToString());
				registrationKey.Note              = PIn.String(row["Note"].ToString());
				registrationKey.DateStarted       = PIn.Date  (row["DateStarted"].ToString());
				registrationKey.DateDisabled      = PIn.Date  (row["DateDisabled"].ToString());
				registrationKey.DateEnded         = PIn.Date  (row["DateEnded"].ToString());
				registrationKey.IsForeign         = PIn.Bool  (row["IsForeign"].ToString());
				registrationKey.UsesServerVersion = PIn.Bool  (row["UsesServerVersion"].ToString());
				registrationKey.IsFreeVersion     = PIn.Bool  (row["IsFreeVersion"].ToString());
				registrationKey.IsOnlyForTesting  = PIn.Bool  (row["IsOnlyForTesting"].ToString());
				registrationKey.VotesAllotted     = PIn.Int   (row["VotesAllotted"].ToString());
				registrationKey.IsResellerCustomer= PIn.Bool  (row["IsResellerCustomer"].ToString());
				retVal.Add(registrationKey);
			}
			return retVal;
		}

		///<summary>Converts a list of RegistrationKey into a DataTable.</summary>
		public static DataTable ListToTable(List<RegistrationKey> listRegistrationKeys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="RegistrationKey";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("RegistrationKeyNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("RegKey");
			table.Columns.Add("Note");
			table.Columns.Add("DateStarted");
			table.Columns.Add("DateDisabled");
			table.Columns.Add("DateEnded");
			table.Columns.Add("IsForeign");
			table.Columns.Add("UsesServerVersion");
			table.Columns.Add("IsFreeVersion");
			table.Columns.Add("IsOnlyForTesting");
			table.Columns.Add("VotesAllotted");
			table.Columns.Add("IsResellerCustomer");
			foreach(RegistrationKey registrationKey in listRegistrationKeys) {
				table.Rows.Add(new object[] {
					POut.Long  (registrationKey.RegistrationKeyNum),
					POut.Long  (registrationKey.PatNum),
					POut.String(registrationKey.RegKey),
					POut.String(registrationKey.Note),
					POut.Date  (registrationKey.DateStarted),
					POut.Date  (registrationKey.DateDisabled),
					POut.Date  (registrationKey.DateEnded),
					POut.Bool  (registrationKey.IsForeign),
					POut.Bool  (registrationKey.UsesServerVersion),
					POut.Bool  (registrationKey.IsFreeVersion),
					POut.Bool  (registrationKey.IsOnlyForTesting),
					POut.Int   (registrationKey.VotesAllotted),
					POut.Bool  (registrationKey.IsResellerCustomer),
				});
			}
			return table;
		}

		///<summary>Inserts one RegistrationKey into the database.  Returns the new priKey.</summary>
		public static long Insert(RegistrationKey registrationKey){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				registrationKey.RegistrationKeyNum=DbHelper.GetNextOracleKey("registrationkey","RegistrationKeyNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(registrationKey,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							registrationKey.RegistrationKeyNum++;
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
				return Insert(registrationKey,false);
			}
		}

		///<summary>Inserts one RegistrationKey into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RegistrationKey registrationKey,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				registrationKey.RegistrationKeyNum=ReplicationServers.GetKey("registrationkey","RegistrationKeyNum");
			}
			string command="INSERT INTO registrationkey (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RegistrationKeyNum,";
			}
			command+="PatNum,RegKey,Note,DateStarted,DateDisabled,DateEnded,IsForeign,UsesServerVersion,IsFreeVersion,IsOnlyForTesting,VotesAllotted,IsResellerCustomer) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(registrationKey.RegistrationKeyNum)+",";
			}
			command+=
				     POut.Long  (registrationKey.PatNum)+","
				+"'"+POut.String(registrationKey.RegKey)+"',"
				+"'"+POut.String(registrationKey.Note)+"',"
				+    POut.Date  (registrationKey.DateStarted)+","
				+    POut.Date  (registrationKey.DateDisabled)+","
				+    POut.Date  (registrationKey.DateEnded)+","
				+    POut.Bool  (registrationKey.IsForeign)+","
				+    POut.Bool  (registrationKey.UsesServerVersion)+","
				+    POut.Bool  (registrationKey.IsFreeVersion)+","
				+    POut.Bool  (registrationKey.IsOnlyForTesting)+","
				+    POut.Int   (registrationKey.VotesAllotted)+","
				+    POut.Bool  (registrationKey.IsResellerCustomer)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				registrationKey.RegistrationKeyNum=Db.NonQ(command,true);
			}
			return registrationKey.RegistrationKeyNum;
		}

		///<summary>Inserts one RegistrationKey into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RegistrationKey registrationKey){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(registrationKey,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					registrationKey.RegistrationKeyNum=DbHelper.GetNextOracleKey("registrationkey","RegistrationKeyNum"); //Cacheless method
				}
				return InsertNoCache(registrationKey,true);
			}
		}

		///<summary>Inserts one RegistrationKey into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RegistrationKey registrationKey,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO registrationkey (";
			if(!useExistingPK && isRandomKeys) {
				registrationKey.RegistrationKeyNum=ReplicationServers.GetKeyNoCache("registrationkey","RegistrationKeyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RegistrationKeyNum,";
			}
			command+="PatNum,RegKey,Note,DateStarted,DateDisabled,DateEnded,IsForeign,UsesServerVersion,IsFreeVersion,IsOnlyForTesting,VotesAllotted,IsResellerCustomer) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(registrationKey.RegistrationKeyNum)+",";
			}
			command+=
				     POut.Long  (registrationKey.PatNum)+","
				+"'"+POut.String(registrationKey.RegKey)+"',"
				+"'"+POut.String(registrationKey.Note)+"',"
				+    POut.Date  (registrationKey.DateStarted)+","
				+    POut.Date  (registrationKey.DateDisabled)+","
				+    POut.Date  (registrationKey.DateEnded)+","
				+    POut.Bool  (registrationKey.IsForeign)+","
				+    POut.Bool  (registrationKey.UsesServerVersion)+","
				+    POut.Bool  (registrationKey.IsFreeVersion)+","
				+    POut.Bool  (registrationKey.IsOnlyForTesting)+","
				+    POut.Int   (registrationKey.VotesAllotted)+","
				+    POut.Bool  (registrationKey.IsResellerCustomer)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				registrationKey.RegistrationKeyNum=Db.NonQ(command,true);
			}
			return registrationKey.RegistrationKeyNum;
		}

		///<summary>Updates one RegistrationKey in the database.</summary>
		public static void Update(RegistrationKey registrationKey){
			string command="UPDATE registrationkey SET "
				+"PatNum            =  "+POut.Long  (registrationKey.PatNum)+", "
				+"RegKey            = '"+POut.String(registrationKey.RegKey)+"', "
				+"Note              = '"+POut.String(registrationKey.Note)+"', "
				+"DateStarted       =  "+POut.Date  (registrationKey.DateStarted)+", "
				+"DateDisabled      =  "+POut.Date  (registrationKey.DateDisabled)+", "
				+"DateEnded         =  "+POut.Date  (registrationKey.DateEnded)+", "
				+"IsForeign         =  "+POut.Bool  (registrationKey.IsForeign)+", "
				+"UsesServerVersion =  "+POut.Bool  (registrationKey.UsesServerVersion)+", "
				+"IsFreeVersion     =  "+POut.Bool  (registrationKey.IsFreeVersion)+", "
				+"IsOnlyForTesting  =  "+POut.Bool  (registrationKey.IsOnlyForTesting)+", "
				+"VotesAllotted     =  "+POut.Int   (registrationKey.VotesAllotted)+", "
				+"IsResellerCustomer=  "+POut.Bool  (registrationKey.IsResellerCustomer)+" "
				+"WHERE RegistrationKeyNum = "+POut.Long(registrationKey.RegistrationKeyNum);
			Db.NonQ(command);
		}

		///<summary>Updates one RegistrationKey in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RegistrationKey registrationKey,RegistrationKey oldRegistrationKey){
			string command="";
			if(registrationKey.PatNum != oldRegistrationKey.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(registrationKey.PatNum)+"";
			}
			if(registrationKey.RegKey != oldRegistrationKey.RegKey) {
				if(command!=""){ command+=",";}
				command+="RegKey = '"+POut.String(registrationKey.RegKey)+"'";
			}
			if(registrationKey.Note != oldRegistrationKey.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(registrationKey.Note)+"'";
			}
			if(registrationKey.DateStarted.Date != oldRegistrationKey.DateStarted.Date) {
				if(command!=""){ command+=",";}
				command+="DateStarted = "+POut.Date(registrationKey.DateStarted)+"";
			}
			if(registrationKey.DateDisabled.Date != oldRegistrationKey.DateDisabled.Date) {
				if(command!=""){ command+=",";}
				command+="DateDisabled = "+POut.Date(registrationKey.DateDisabled)+"";
			}
			if(registrationKey.DateEnded.Date != oldRegistrationKey.DateEnded.Date) {
				if(command!=""){ command+=",";}
				command+="DateEnded = "+POut.Date(registrationKey.DateEnded)+"";
			}
			if(registrationKey.IsForeign != oldRegistrationKey.IsForeign) {
				if(command!=""){ command+=",";}
				command+="IsForeign = "+POut.Bool(registrationKey.IsForeign)+"";
			}
			if(registrationKey.UsesServerVersion != oldRegistrationKey.UsesServerVersion) {
				if(command!=""){ command+=",";}
				command+="UsesServerVersion = "+POut.Bool(registrationKey.UsesServerVersion)+"";
			}
			if(registrationKey.IsFreeVersion != oldRegistrationKey.IsFreeVersion) {
				if(command!=""){ command+=",";}
				command+="IsFreeVersion = "+POut.Bool(registrationKey.IsFreeVersion)+"";
			}
			if(registrationKey.IsOnlyForTesting != oldRegistrationKey.IsOnlyForTesting) {
				if(command!=""){ command+=",";}
				command+="IsOnlyForTesting = "+POut.Bool(registrationKey.IsOnlyForTesting)+"";
			}
			if(registrationKey.VotesAllotted != oldRegistrationKey.VotesAllotted) {
				if(command!=""){ command+=",";}
				command+="VotesAllotted = "+POut.Int(registrationKey.VotesAllotted)+"";
			}
			if(registrationKey.IsResellerCustomer != oldRegistrationKey.IsResellerCustomer) {
				if(command!=""){ command+=",";}
				command+="IsResellerCustomer = "+POut.Bool(registrationKey.IsResellerCustomer)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE registrationkey SET "+command
				+" WHERE RegistrationKeyNum = "+POut.Long(registrationKey.RegistrationKeyNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one RegistrationKey from the database.</summary>
		public static void Delete(long registrationKeyNum){
			string command="DELETE FROM registrationkey "
				+"WHERE RegistrationKeyNum = "+POut.Long(registrationKeyNum);
			Db.NonQ(command);
		}

	}
}