using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace OpenDentBusiness.Crud{
	///<summary>Does not support Oracle. Uses customer connection strings. NOT AUTO GENERATED.</summary>
	public class BugCrud {
		///<summary>Gets one Bug object from the database using the primary key.  Returns null if not found.</summary>
		public static Bug SelectOne(long bugId){
			string command="SELECT * FROM bug "
				+"WHERE BugId = "+POut.Long(bugId);
			List<Bug> list=TableToList(BugDb.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Bug object from the database using a query.</summary>
		public static Bug SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Bug> list=TableToList(BugDb.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Bug objects from the database using a query.</summary>
		public static List<Bug> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Bug> list=TableToList(BugDb.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Bug> TableToList(DataTable table){
			List<Bug> retVal=new List<Bug>();
			Bug bug;
			foreach(DataRow row in table.Rows) {
				bug=new Bug();
				bug.BugId        = PIn.Long  (row["BugId"].ToString());
				bug.CreationDate = PIn.DateT (row["CreationDate"].ToString());
				bug.Status_      = (BugStatus)Enum.Parse(typeof(OpenDentBusiness.BugStatus),row["Status_"].ToString());
				bug.Type_        = (BugType)Enum.Parse(typeof(OpenDentBusiness.BugType),row["Type_"].ToString());
				bug.PriorityLevel= PIn.Int   (row["PriorityLevel"].ToString());
				bug.VersionsFound= PIn.String(row["VersionsFound"].ToString());
				bug.VersionsFixed= PIn.String(row["VersionsFixed"].ToString());
				bug.Description  = PIn.String(row["Description"].ToString());
				bug.LongDesc     = PIn.String(row["LongDesc"].ToString());
				bug.PrivateDesc  = PIn.String(row["PrivateDesc"].ToString());
				bug.Discussion   = PIn.String(row["Discussion"].ToString());
				bug.Submitter    = PIn.Long  (row["Submitter"].ToString());
				retVal.Add(bug);
			}
			return retVal;
		}

		///<summary>Inserts one Bug into the database.  Returns the new priKey.</summary>
		public static long Insert(Bug bug){
			//if(DataConnection.DBtype==DatabaseType.Oracle) {
			//	bug.BugId=DbHelper.GetNextOracleKey("bug","BugId");
			//	int loopcount=0;
			//	while(loopcount<100){
			//		try {
			//			return Insert(bug,true);
			//		}
			//		catch(Oracle.DataAccess.Client.OracleException ex){
			//			if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
			//				bug.BugId++;
			//				loopcount++;
			//			}
			//			else{
			//				throw ex;
			//			}
			//		}
			//	}
			//	throw new ApplicationException("Insert failed.  Could not generate primary key.");
			//}
			//else {
			return Insert(bug,false);
			//}
		}

		///<summary>Inserts one Bug into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Bug bug,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				bug.BugId=BugDb.GetKey();
			}
			string command="INSERT INTO bug (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="BugId,";
			}
			command+="CreationDate,Status_,Type_,PriorityLevel,VersionsFound,VersionsFixed,Description,LongDesc,PrivateDesc,Discussion,Submitter) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(bug.BugId)+",";
			}
			command+=
				     POut.DateT (bug.CreationDate)+","
				+    POut.Int   ((int)bug.Status_)+","
				+    POut.Int   ((int)bug.Type_)+","
				+    POut.Int   (bug.PriorityLevel)+","
				+"'"+POut.String(bug.VersionsFound)+"',"
				+"'"+POut.String(bug.VersionsFixed)+"',"
				+"'"+POut.String(bug.Description)+"',"
				+"'"+POut.String(bug.LongDesc)+"',"
				+"'"+POut.String(bug.PrivateDesc)+"',"
				+"'"+POut.String(bug.Discussion)+"',"
				+    POut.Long  (bug.Submitter)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				BugDb.NonQ(command);
			}
			else {
				bug.BugId=BugDb.NonQ(command,true);
			}
			return bug.BugId;
		}

		///<summary>Inserts one Bug into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Bug bug){
			//if(DataConnection.DBtype==DatabaseType.MySql) {
			return InsertNoCache(bug,false);
			//}
			//else {
			//	if(DataConnection.DBtype==DatabaseType.Oracle) {
			//		bug.BugId=DbHelper.GetNextOracleKey("bug","BugId"); //Cacheless method
			//	}
			//	return InsertNoCache(bug,true);
			//}
		}

		///<summary>Inserts one Bug into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Bug bug,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO bug (";
			if(!useExistingPK && isRandomKeys) {
				bug.BugId=BugDb.GetKeyNoCache();
			}
			if(isRandomKeys || useExistingPK) {
				command+="BugId,";
			}
			command+="CreationDate,Status_,Type_,PriorityLevel,VersionsFound,VersionsFixed,Description,LongDesc,PrivateDesc,Discussion,Submitter) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(bug.BugId)+",";
			}
			command+=
				     POut.DateT (bug.CreationDate)+","
				+    POut.Int   ((int)bug.Status_)+","
				+    POut.Int   ((int)bug.Type_)+","
				+    POut.Int   (bug.PriorityLevel)+","
				+"'"+POut.String(bug.VersionsFound)+"',"
				+"'"+POut.String(bug.VersionsFixed)+"',"
				+"'"+POut.String(bug.Description)+"',"
				+"'"+POut.String(bug.LongDesc)+"',"
				+"'"+POut.String(bug.PrivateDesc)+"',"
				+"'"+POut.String(bug.Discussion)+"',"
				+    POut.Long  (bug.Submitter)+")";
			if(useExistingPK || isRandomKeys) {
				BugDb.NonQ(command);
			}
			else {
				bug.BugId=BugDb.NonQ(command,true);
			}
			return bug.BugId;
		}

		///<summary>Updates one Bug in the database.</summary>
		public static void Update(Bug bug){
			string command="UPDATE bug SET "
				+"CreationDate =  "+POut.DateT (bug.CreationDate)+", "
				+"Status_      =  "+POut.Int   ((int)bug.Status_)+", "
				+"Type_        =  "+POut.Int   ((int)bug.Type_)+", "
				+"PriorityLevel=  "+POut.Int   (bug.PriorityLevel)+", "
				+"VersionsFound= '"+POut.String(bug.VersionsFound)+"', "
				+"VersionsFixed= '"+POut.String(bug.VersionsFixed)+"', "
				+"Description  = '"+POut.String(bug.Description)+"', "
				+"LongDesc     = '"+POut.String(bug.LongDesc)+"', "
				+"PrivateDesc  = '"+POut.String(bug.PrivateDesc)+"', "
				+"Discussion   = '"+POut.String(bug.Discussion)+"', "
				+"Submitter    =  "+POut.Long  (bug.Submitter)+" "
				+"WHERE BugId = "+POut.Long(bug.BugId);
			BugDb.NonQ(command);
		}

		///<summary>Updates one Bug in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Bug bug,Bug oldBug){
			string command="";
			if(bug.CreationDate != oldBug.CreationDate) {
				if(command!=""){ command+=",";}
				command+="CreationDate = "+POut.DateT(bug.CreationDate)+"";
			}
			if(bug.Status_ != oldBug.Status_) {
				if(command!=""){ command+=",";}
				command+="Status_ = "+POut.Int   ((int)bug.Status_)+"";
			}
			if(bug.Type_ != oldBug.Type_) {
				if(command!=""){ command+=",";}
				command+="Type_ = "+POut.Int   ((int)bug.Type_)+"";
			}
			if(bug.PriorityLevel != oldBug.PriorityLevel) {
				if(command!=""){ command+=",";}
				command+="PriorityLevel = "+POut.Int(bug.PriorityLevel)+"";
			}
			if(bug.VersionsFound != oldBug.VersionsFound) {
				if(command!=""){ command+=",";}
				command+="VersionsFound = '"+POut.String(bug.VersionsFound)+"'";
			}
			if(bug.VersionsFixed != oldBug.VersionsFixed) {
				if(command!=""){ command+=",";}
				command+="VersionsFixed = '"+POut.String(bug.VersionsFixed)+"'";
			}
			if(bug.Description != oldBug.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(bug.Description)+"'";
			}
			if(bug.LongDesc != oldBug.LongDesc) {
				if(command!=""){ command+=",";}
				command+="LongDesc = '"+POut.String(bug.LongDesc)+"'";
			}
			if(bug.PrivateDesc != oldBug.PrivateDesc) {
				if(command!=""){ command+=",";}
				command+="PrivateDesc = '"+POut.String(bug.PrivateDesc)+"'";
			}
			if(bug.Discussion != oldBug.Discussion) {
				if(command!=""){ command+=",";}
				command+="Discussion = '"+POut.String(bug.Discussion)+"'";
			}
			if(bug.Submitter != oldBug.Submitter) {
				if(command!=""){ command+=",";}
				command+="Submitter = "+POut.Long(bug.Submitter)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE bug SET "+command
				+" WHERE BugId = "+POut.Long(bug.BugId);
			BugDb.NonQ(command);
			return true;
		}

		///<summary>Deletes one Bug from the database.</summary>
		public static void Delete(long bugId){
			string command="DELETE FROM bug "
				+"WHERE BugId = "+POut.Long(bugId);
			BugDb.NonQ(command);
		}

		///<summary>Handles DB connections for the Bugs database.</summary>
		public class BugDb {
#if DEBUG
			private static string _connectionString="server=localhost;uid=root;pwd=;database=bugs;";
#else
			private static string _connectionString="server=server;uid=root;pwd=;database=bugs;";
#endif
			private static Random _random=new Random();

			public static long NonQ(string command,bool returnPk=false) {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					throw new Exception("BugsDb only works with MySQL DBs");
				}
				long retVal=0;
				using(MySql.Data.MySqlClient.MySqlConnection conn=new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
				using(MySql.Data.MySqlClient.MySqlCommand cmd=new MySql.Data.MySqlClient.MySqlCommand()) {
					cmd.Connection=conn;
					cmd.CommandText=command;
					cmd.CommandType=CommandType.Text;
					conn.Open();
					try {
						retVal=cmd.ExecuteNonQuery();
					}
					catch(MySqlException ex) {
						if(ex.Number==1153) {
							throw new ApplicationException("Please add the following to your my.ini file: max_allowed_packet=40000000");
						}
						throw ex;
					}
					if(returnPk) {
						cmd.CommandText="SELECT LAST_INSERT_ID()";
						MySqlDataReader dr=cmd.ExecuteReader();
						if(dr.Read()) {
							retVal=Convert.ToInt64(dr[0].ToString());//InsertID
						}
					}
					conn.Close();
				}//endusing
				return retVal;//either insert id or row count.
			}

			public static DataTable GetTable(string command) {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					throw new Exception("BugsDb only works with MySQL DBs");
				}
				DataTable table=new DataTable();
				using(MySql.Data.MySqlClient.MySqlConnection conn=new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
				using(MySql.Data.MySqlClient.MySqlCommand cmd=new MySql.Data.MySqlClient.MySqlCommand()) {
					cmd.Connection=conn;
					cmd.CommandText=command;
					cmd.CommandType=CommandType.Text;
					conn.Open();
					using(MySqlDataAdapter da=new MySqlDataAdapter(cmd)) {
						da.Fill(table);
					}
					conn.Close();
				}//endusing
				return table;
			}

			public static long GetKey() {
				long rangeStart=10000;
				long rangeEnd=long.MaxValue;
				long server_id=ReplicationServers.GetServer_id();
				if(server_id!=0) {
					ReplicationServer thisServer=ReplicationServers.GetServer(server_id);
					if(thisServer!=null && thisServer.RangeEnd-thisServer.RangeStart >= 999999) {
						rangeStart=thisServer.RangeStart;
						rangeEnd=thisServer.RangeEnd;
					}
				}
				long span=rangeEnd-rangeStart;
				long rndLong=(long)(_random.NextDouble()*span)+rangeStart;
				while(rndLong==0 
				|| rndLong<rangeStart 
				|| rndLong>rangeStart 
				|| KeyInUse(rndLong)) {
					rndLong=(long)(_random.NextDouble()*span)+rangeStart;
				}
				return rndLong;
			}

			public static long GetKeyNoCache() {
				long rangeStart=10000;
				long rangeEnd=long.MaxValue;
				long server_id=ReplicationServers.GetServer_id();
				if(server_id!=0) {
					ReplicationServer thisServer=ReplicationServers.GetServer(server_id);
					if(thisServer!=null && thisServer.RangeEnd-thisServer.RangeStart >= 999999) {
						rangeStart=thisServer.RangeStart;
						rangeEnd=thisServer.RangeEnd;
					}
				}
				long span=rangeEnd-rangeStart;
				long rndLong=(long)(_random.NextDouble()*span)+rangeStart;
				while(rndLong==0 
				|| rndLong<rangeStart 
				|| rndLong>rangeStart 
				|| KeyInUse(rndLong)) {
					rndLong=(long)(_random.NextDouble()*span)+rangeStart;
				}
				return rndLong;
			}

			private static bool KeyInUse(long rndLong) {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					throw new Exception("BugsDb only works with MySQL DBs");
				}
				DataTable table=GetTable("SELECT COUNT(*) FROM bug WHERE bugid="+POut.Long(rndLong));
				if(table.Rows[0][0].ToString()=="0") {
					return false;
				}
				return true;
			}

		}


	}
}