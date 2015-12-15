//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobLinkCrud {
		///<summary>Gets one JobLink object from the database using the primary key.  Returns null if not found.</summary>
		public static JobLink SelectOne(long jobLinkNum){
			string command="SELECT * FROM joblink "
				+"WHERE JobLinkNum = "+POut.Long(jobLinkNum);
			List<JobLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one JobLink object from the database using a query.</summary>
		public static JobLink SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobLink> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of JobLink objects from the database using a query.</summary>
		public static List<JobLink> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<JobLink> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<JobLink> TableToList(DataTable table){
			List<JobLink> retVal=new List<JobLink>();
			JobLink jobLink;
			foreach(DataRow row in table.Rows) {
				jobLink=new JobLink();
				jobLink.JobLinkNum= PIn.Long  (row["JobLinkNum"].ToString());
				jobLink.JobNum    = PIn.Long  (row["JobNum"].ToString());
				jobLink.FKey      = PIn.Long  (row["FKey"].ToString());
				jobLink.LinkType  = (OpenDentBusiness.JobLinkType)PIn.Int(row["LinkType"].ToString());
				retVal.Add(jobLink);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<JobLink> listJobLinks) {
			DataTable table=new DataTable("JobLinks");
			table.Columns.Add("JobLinkNum");
			table.Columns.Add("JobNum");
			table.Columns.Add("FKey");
			table.Columns.Add("LinkType");
			foreach(JobLink jobLink in listJobLinks) {
				table.Rows.Add(new object[] {
					POut.Long  (jobLink.JobLinkNum),
					POut.Long  (jobLink.JobNum),
					POut.Long  (jobLink.FKey),
					POut.Int   ((int)jobLink.LinkType),
				});
			}
			return table;
		}

		///<summary>Inserts one JobLink into the database.  Returns the new priKey.</summary>
		public static long Insert(JobLink jobLink){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				jobLink.JobLinkNum=DbHelper.GetNextOracleKey("joblink","JobLinkNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(jobLink,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							jobLink.JobLinkNum++;
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
				return Insert(jobLink,false);
			}
		}

		///<summary>Inserts one JobLink into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(JobLink jobLink,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				jobLink.JobLinkNum=ReplicationServers.GetKey("joblink","JobLinkNum");
			}
			string command="INSERT INTO joblink (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobLinkNum,";
			}
			command+="JobNum,FKey,LinkType) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(jobLink.JobLinkNum)+",";
			}
			command+=
				     POut.Long  (jobLink.JobNum)+","
				+    POut.Long  (jobLink.FKey)+","
				+    POut.Int   ((int)jobLink.LinkType)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobLink.JobLinkNum=Db.NonQ(command,true);
			}
			return jobLink.JobLinkNum;
		}

		///<summary>Inserts one JobLink into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobLink jobLink){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(jobLink,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					jobLink.JobLinkNum=DbHelper.GetNextOracleKey("joblink","JobLinkNum"); //Cacheless method
				}
				return InsertNoCache(jobLink,true);
			}
		}

		///<summary>Inserts one JobLink into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(JobLink jobLink,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO joblink (";
			if(!useExistingPK && isRandomKeys) {
				jobLink.JobLinkNum=ReplicationServers.GetKeyNoCache("joblink","JobLinkNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobLinkNum,";
			}
			command+="JobNum,FKey,LinkType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(jobLink.JobLinkNum)+",";
			}
			command+=
				     POut.Long  (jobLink.JobNum)+","
				+    POut.Long  (jobLink.FKey)+","
				+    POut.Int   ((int)jobLink.LinkType)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				jobLink.JobLinkNum=Db.NonQ(command,true);
			}
			return jobLink.JobLinkNum;
		}

		///<summary>Updates one JobLink in the database.</summary>
		public static void Update(JobLink jobLink){
			string command="UPDATE joblink SET "
				+"JobNum    =  "+POut.Long  (jobLink.JobNum)+", "
				+"FKey      =  "+POut.Long  (jobLink.FKey)+", "
				+"LinkType  =  "+POut.Int   ((int)jobLink.LinkType)+" "
				+"WHERE JobLinkNum = "+POut.Long(jobLink.JobLinkNum);
			Db.NonQ(command);
		}

		///<summary>Updates one JobLink in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(JobLink jobLink,JobLink oldJobLink){
			string command="";
			if(jobLink.JobNum != oldJobLink.JobNum) {
				if(command!=""){ command+=",";}
				command+="JobNum = "+POut.Long(jobLink.JobNum)+"";
			}
			if(jobLink.FKey != oldJobLink.FKey) {
				if(command!=""){ command+=",";}
				command+="FKey = "+POut.Long(jobLink.FKey)+"";
			}
			if(jobLink.LinkType != oldJobLink.LinkType) {
				if(command!=""){ command+=",";}
				command+="LinkType = "+POut.Int   ((int)jobLink.LinkType)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE joblink SET "+command
				+" WHERE JobLinkNum = "+POut.Long(jobLink.JobLinkNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one JobLink from the database.</summary>
		public static void Delete(long jobLinkNum){
			string command="DELETE FROM joblink "
				+"WHERE JobLinkNum = "+POut.Long(jobLinkNum);
			Db.NonQ(command);
		}

	}
}