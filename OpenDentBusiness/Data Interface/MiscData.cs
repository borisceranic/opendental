using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;

namespace OpenDentBusiness {

	///<summary>Miscellaneous database functions.</summary>
	public class MiscData {

		///<summary>Gets the current date/Time direcly from the server.  Mostly used to prevent uesr from altering the workstation date to bypass security.</summary>
		public static DateTime GetNowDateTime() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT NOW()";
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				command="SELECT CURRENT_TIMESTAMP FROM DUAL";
			}
			DataTable table=Db.GetTable(command);
			return PIn.PDateT(table.Rows[0][0].ToString());
		}

		///<summary>Generates a random primary key.  Tests to see if that key already exists before returning it for use.  Currently, the range of returned values is greater than 0, and less than or equal to 16777215, the limit for mysql medium int.  This will eventually change to a max of 18446744073709551615.  Then, the return value would have to be a ulong and the mysql type would have to be bigint.</summary>
		public static int GetKey(string tablename, string field) {
			//No need to check RemotingRole; no call to db.
			int numComputers=0;
			int myComputerNum=0;//One-based unique computer number index. Used to decide which key-partition to use for this computer.
			int myPartitionStart=0;
			int myPartitionEnd=0;
			//Calculate the primary key range for this computer if it has not already calculated.
			if(numComputers==0 || myComputerNum==0){
				try{
					numComputers=GetNumComputers();
					myComputerNum=GetComputerNumForName(Dns.GetHostName());					
				}catch{
					//This computer has not yet been added to the computer table. Generate any old random number as long as it is unique.
					//This is the first introduction of the computer into the cluster.
					numComputers=1;
					myComputerNum=1;
				}
				int keymaxval=16777215;
				int partitionSize=keymaxval/numComputers;//truncation here is good (to avoid partition overflow).
				myPartitionStart=(myComputerNum-1)*partitionSize+1;
				myPartitionEnd=myPartitionStart+partitionSize-1;
			}
			Random random=new Random();
			int rnd;
			do{
				rnd=random.Next(myPartitionStart,myPartitionEnd);
			}while(rnd==0||KeyInUse(tablename,field,rnd));
			return rnd;
		}

		public static int GetNumComputers(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT COUNT(*) FROM computer";
			DataTable table=Db.GetTable(command);
			return PIn.PInt(table.Rows[0][0].ToString());
		}

		public static int GetComputerNumForName(string computerName){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),computerName);
			}
			string command="SELECT COUNT(*) FROM computer "+
				"WHERE ComputerNum<=(SELECT ComputerNum FROM computer AS temp WHERE CompName "+
				"like '"+computerName+"')";
			DataTable table=Db.GetTable(command);
			return PIn.PInt(table.Rows[0][0].ToString());
		}

		private static bool KeyInUse(string tablename,string field,int keynum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),tablename,field,keynum);
			}
			string command="SELECT COUNT(*) FROM "+tablename+" WHERE "+field+"="+keynum.ToString();
			if(Db.GetCount(command)=="0"){
				return false;
			}
			return true;//already in use
		}

		///<summary>Used in MakeABackup to ensure a unique backup database name.</summary>
		private static bool Contains(string[] arrayToSearch,string valueToTest) {
			//No need to check RemotingRole; no call to db.
			string compare;
			for(int i=0;i<arrayToSearch.Length;i++) {
				compare=arrayToSearch[i];
				if(arrayToSearch[i]==valueToTest) {
					return true;
				}
			}
			return false;
		}

		///<summary>Backs up the database to the same directory as the original just in case the user did not have sense enough to do a backup first.</summary>
		public static int MakeABackup() {
			//This function should always make the backup on the server itself, and since no directories are
			//referred to (all handled with MySQL), this function will always be referred to the server from
			//client machines.
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			//only used in two places: upgrading version, and upgrading mysql version.
			//Both places check first to make sure user is using mysql.
			//we have to be careful to throw an exception if the backup is failing.
			DataConnection dcon=new DataConnection();
			string command="SELECT database()";
			DataTable table=dcon.GetTable(command);
			string oldDb=PIn.PString(table.Rows[0][0].ToString());
			string newDb=oldDb+"backup_"+DateTime.Today.ToString("MM_dd_yyyy");
			command="SHOW DATABASES";
			table=dcon.GetTable(command);
			string[] databases=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				databases[i]=table.Rows[i][0].ToString();
			}
			if(Contains(databases,newDb)) {//if the new database name already exists
				//find a unique one
				int uniqueID=1;
				string originalNewDb=newDb;
				do {
					newDb=originalNewDb+"_"+uniqueID.ToString();
					uniqueID++;
				}
				while(Contains(databases,newDb));
			}
			command="CREATE DATABASE "+newDb+" CHARACTER SET utf8";
			dcon.NonQ(command);
			command="SHOW TABLES";
			table=dcon.GetTable(command);
			string[] tableName=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				tableName[i]=table.Rows[i][0].ToString();
			}
			//switch to using the new database
			DataConnection newDcon=new DataConnection(newDb);
			for(int i=0;i<tableName.Length;i++) {
				command="SHOW CREATE TABLE "+oldDb+"."+tableName[i];
				table=newDcon.GetTable(command);
				command=table.Rows[0][1].ToString();
				newDcon.NonQ(command);//this has to be run using connection with new database
				command="INSERT INTO "+newDb+"."+tableName[i]
					+" SELECT * FROM "+oldDb+"."+tableName[i];
				newDcon.NonQ(command);
			}
			return 0;
		}

		public static string GetCurrentDatabase() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT database()";
			DataTable table=Db.GetTable(command);
			return PIn.PString(table.Rows[0][0].ToString());
		}

		public static string GetMySqlVersion() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT @@version";
			DataTable table=Db.GetTable(command);
			return PIn.PString(table.Rows[0][0].ToString());
		}

		///<summary>Returns a collection of unique AtoZ folders for the array of dbnames passed in.  It will not include the current AtoZ folder for this database, even if shared by another db.</summary>
		public static List<string> GetAtoZforDb(string[] dbNames) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("MiscData.GetAtoZforDb failed.  Updates not allowed from ClientWeb.");
			}
			List<string> retval=new List<string>();
			DataConnection dcon=null;
			string atozName;
			string atozThisDb=PrefC.GetString("DocPath");
			for(int i=0;i<dbNames.Length;i++) {
				try {
					dcon=new DataConnection(dbNames[i]);
					string command="SELECT ValueString FROM preference WHERE PrefName='DocPath'";
					atozName=dcon.GetScalar(command);
					if(retval.Contains(atozName)) {
						continue;
					}
					if(atozName==atozThisDb) {
						continue;
					}
					retval.Add(atozName);
				}
				catch {
					//don't add it to the list
				}
			}
			return retval;
		}

		public static void LockWorkstationsForDbs(string[] dbNames) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("MiscData.GetAtoZforDb failed.  Updates not allowed from ClientWeb.");
			}
			DataConnection dcon=null;
			for(int i=0;i<dbNames.Length;i++) {
				try {
					dcon=new DataConnection(dbNames[i]);
					string command="UPDATE preference SET ValueString ='"+POut.PString(Environment.MachineName)
						+"' WHERE PrefName='DocPath'";
					dcon.NonQ(command);
				}
				catch { }
			}
		}


	}

	
}































