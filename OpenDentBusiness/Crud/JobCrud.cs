//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class JobCrud {
		///<summary>Gets one Job object from the database using the primary key.  Returns null if not found.</summary>
		public static Job SelectOne(long jobNum){
			string command="SELECT * FROM job "
				+"WHERE JobNum = "+POut.Long(jobNum);
			List<Job> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Job object from the database using a query.</summary>
		public static Job SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Job> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Job objects from the database using a query.</summary>
		public static List<Job> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Job> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Job> TableToList(DataTable table){
			List<Job> retVal=new List<Job>();
			Job job;
			foreach(DataRow row in table.Rows) {
				job=new Job();
				job.JobNum                = PIn.Long  (row["JobNum"].ToString());
				job.UserNumConcept        = PIn.Long  (row["UserNumConcept"].ToString());
				job.UserNumExpert         = PIn.Long  (row["UserNumExpert"].ToString());
				job.UserNumEngineer       = PIn.Long  (row["UserNumEngineer"].ToString());
				job.UserNumApproverConcept= PIn.Long  (row["UserNumApproverConcept"].ToString());
				job.UserNumApproverJob    = PIn.Long  (row["UserNumApproverJob"].ToString());
				job.UserNumApproverChange = PIn.Long  (row["UserNumApproverChange"].ToString());
				job.UserNumDocumenter     = PIn.Long  (row["UserNumDocumenter"].ToString());
				job.UserNumCustContact    = PIn.Long  (row["UserNumCustContact"].ToString());
				job.UserNumCheckout       = PIn.Long  (row["UserNumCheckout"].ToString());
				job.UserNumInfo           = PIn.Long  (row["UserNumInfo"].ToString());
				job.ParentNum             = PIn.Long  (row["ParentNum"].ToString());
				job.DateTimeCustContact   = PIn.DateT (row["DateTimeCustContact"].ToString());
				string priority=row["Priority"].ToString();
				if(priority==""){
					job.Priority            =(JobPriority)0;
				}
				else try{
					job.Priority            =(JobPriority)Enum.Parse(typeof(JobPriority),priority);
				}
				catch{
					job.Priority            =(JobPriority)0;
				}
				string category=row["Category"].ToString();
				if(category==""){
					job.Category            =(JobCategory)0;
				}
				else try{
					job.Category            =(JobCategory)Enum.Parse(typeof(JobCategory),category);
				}
				catch{
					job.Category            =(JobCategory)0;
				}
				job.JobVersion            = PIn.String(row["JobVersion"].ToString());
				job.HoursEstimate         = PIn.Int   (row["HoursEstimate"].ToString());
				job.HoursActual           = PIn.Int   (row["HoursActual"].ToString());
				job.DateTimeEntry         = PIn.DateT (row["DateTimeEntry"].ToString());
				job.Description           = PIn.String(row["Description"].ToString());
				job.Documentation         = PIn.String(row["Documentation"].ToString());
				job.Title                 = PIn.String(row["Title"].ToString());
				string phaseCur=row["PhaseCur"].ToString();
				if(phaseCur==""){
					job.PhaseCur            =(JobPhase)0;
				}
				else try{
					job.PhaseCur            =(JobPhase)Enum.Parse(typeof(JobPhase),phaseCur);
				}
				catch{
					job.PhaseCur            =(JobPhase)0;
				}
				job.IsApprovalNeeded      = PIn.Bool  (row["IsApprovalNeeded"].ToString());
				job.AckDateTime           = PIn.DateT (row["AckDateTime"].ToString());
				retVal.Add(job);
			}
			return retVal;
		}

		///<summary>Converts a list of Job into a DataTable.</summary>
		public static DataTable ListToTable(List<Job> listJobs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Job";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("JobNum");
			table.Columns.Add("UserNumConcept");
			table.Columns.Add("UserNumExpert");
			table.Columns.Add("UserNumEngineer");
			table.Columns.Add("UserNumApproverConcept");
			table.Columns.Add("UserNumApproverJob");
			table.Columns.Add("UserNumApproverChange");
			table.Columns.Add("UserNumDocumenter");
			table.Columns.Add("UserNumCustContact");
			table.Columns.Add("UserNumCheckout");
			table.Columns.Add("UserNumInfo");
			table.Columns.Add("ParentNum");
			table.Columns.Add("DateTimeCustContact");
			table.Columns.Add("Priority");
			table.Columns.Add("Category");
			table.Columns.Add("JobVersion");
			table.Columns.Add("HoursEstimate");
			table.Columns.Add("HoursActual");
			table.Columns.Add("DateTimeEntry");
			table.Columns.Add("Description");
			table.Columns.Add("Documentation");
			table.Columns.Add("Title");
			table.Columns.Add("PhaseCur");
			table.Columns.Add("IsApprovalNeeded");
			table.Columns.Add("AckDateTime");
			foreach(Job job in listJobs) {
				table.Rows.Add(new object[] {
					POut.Long  (job.JobNum),
					POut.Long  (job.UserNumConcept),
					POut.Long  (job.UserNumExpert),
					POut.Long  (job.UserNumEngineer),
					POut.Long  (job.UserNumApproverConcept),
					POut.Long  (job.UserNumApproverJob),
					POut.Long  (job.UserNumApproverChange),
					POut.Long  (job.UserNumDocumenter),
					POut.Long  (job.UserNumCustContact),
					POut.Long  (job.UserNumCheckout),
					POut.Long  (job.UserNumInfo),
					POut.Long  (job.ParentNum),
					POut.DateT (job.DateTimeCustContact,false),
					POut.Int   ((int)job.Priority),
					POut.Int   ((int)job.Category),
					            job.JobVersion,
					POut.Int   (job.HoursEstimate),
					POut.Int   (job.HoursActual),
					POut.DateT (job.DateTimeEntry,false),
					            job.Description,
					            job.Documentation,
					            job.Title,
					POut.Int   ((int)job.PhaseCur),
					POut.Bool  (job.IsApprovalNeeded),
					POut.DateT (job.AckDateTime,false),
				});
			}
			return table;
		}

		///<summary>Inserts one Job into the database.  Returns the new priKey.</summary>
		public static long Insert(Job job){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				job.JobNum=DbHelper.GetNextOracleKey("job","JobNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(job,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							job.JobNum++;
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
				return Insert(job,false);
			}
		}

		///<summary>Inserts one Job into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Job job,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				job.JobNum=ReplicationServers.GetKey("job","JobNum");
			}
			string command="INSERT INTO job (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="JobNum,";
			}
			command+="UserNumConcept,UserNumExpert,UserNumEngineer,UserNumApproverConcept,UserNumApproverJob,UserNumApproverChange,UserNumDocumenter,UserNumCustContact,UserNumCheckout,UserNumInfo,ParentNum,DateTimeCustContact,Priority,Category,JobVersion,HoursEstimate,HoursActual,DateTimeEntry,Description,Documentation,Title,PhaseCur,IsApprovalNeeded,AckDateTime) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(job.JobNum)+",";
			}
			command+=
				     POut.Long  (job.UserNumConcept)+","
				+    POut.Long  (job.UserNumExpert)+","
				+    POut.Long  (job.UserNumEngineer)+","
				+    POut.Long  (job.UserNumApproverConcept)+","
				+    POut.Long  (job.UserNumApproverJob)+","
				+    POut.Long  (job.UserNumApproverChange)+","
				+    POut.Long  (job.UserNumDocumenter)+","
				+    POut.Long  (job.UserNumCustContact)+","
				+    POut.Long  (job.UserNumCheckout)+","
				+    POut.Long  (job.UserNumInfo)+","
				+    POut.Long  (job.ParentNum)+","
				+    POut.DateT (job.DateTimeCustContact)+","
				+"'"+POut.String(job.Priority.ToString())+"',"
				+"'"+POut.String(job.Category.ToString())+"',"
				+"'"+POut.String(job.JobVersion)+"',"
				+    POut.Int   (job.HoursEstimate)+","
				+    POut.Int   (job.HoursActual)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    DbHelper.ParamChar+"paramDocumentation,"
				+"'"+POut.String(job.Title)+"',"
				+"'"+POut.String(job.PhaseCur.ToString())+"',"
				+    POut.Bool  (job.IsApprovalNeeded)+","
				+    POut.DateT (job.AckDateTime)+")";
			if(job.Description==null) {
				job.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,job.Description);
			if(job.Documentation==null) {
				job.Documentation="";
			}
			OdSqlParameter paramDocumentation=new OdSqlParameter("paramDocumentation",OdDbType.Text,job.Documentation);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramDescription,paramDocumentation);
			}
			else {
				job.JobNum=Db.NonQ(command,true,paramDescription,paramDocumentation);
			}
			return job.JobNum;
		}

		///<summary>Inserts one Job into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Job job){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(job,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					job.JobNum=DbHelper.GetNextOracleKey("job","JobNum"); //Cacheless method
				}
				return InsertNoCache(job,true);
			}
		}

		///<summary>Inserts one Job into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Job job,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO job (";
			if(!useExistingPK && isRandomKeys) {
				job.JobNum=ReplicationServers.GetKeyNoCache("job","JobNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="JobNum,";
			}
			command+="UserNumConcept,UserNumExpert,UserNumEngineer,UserNumApproverConcept,UserNumApproverJob,UserNumApproverChange,UserNumDocumenter,UserNumCustContact,UserNumCheckout,UserNumInfo,ParentNum,DateTimeCustContact,Priority,Category,JobVersion,HoursEstimate,HoursActual,DateTimeEntry,Description,Documentation,Title,PhaseCur,IsApprovalNeeded,AckDateTime) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(job.JobNum)+",";
			}
			command+=
				     POut.Long  (job.UserNumConcept)+","
				+    POut.Long  (job.UserNumExpert)+","
				+    POut.Long  (job.UserNumEngineer)+","
				+    POut.Long  (job.UserNumApproverConcept)+","
				+    POut.Long  (job.UserNumApproverJob)+","
				+    POut.Long  (job.UserNumApproverChange)+","
				+    POut.Long  (job.UserNumDocumenter)+","
				+    POut.Long  (job.UserNumCustContact)+","
				+    POut.Long  (job.UserNumCheckout)+","
				+    POut.Long  (job.UserNumInfo)+","
				+    POut.Long  (job.ParentNum)+","
				+    POut.DateT (job.DateTimeCustContact)+","
				+"'"+POut.String(job.Priority.ToString())+"',"
				+"'"+POut.String(job.Category.ToString())+"',"
				+"'"+POut.String(job.JobVersion)+"',"
				+    POut.Int   (job.HoursEstimate)+","
				+    POut.Int   (job.HoursActual)+","
				+    DbHelper.Now()+","
				+    DbHelper.ParamChar+"paramDescription,"
				+    DbHelper.ParamChar+"paramDocumentation,"
				+"'"+POut.String(job.Title)+"',"
				+"'"+POut.String(job.PhaseCur.ToString())+"',"
				+    POut.Bool  (job.IsApprovalNeeded)+","
				+    POut.DateT (job.AckDateTime)+")";
			if(job.Description==null) {
				job.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,job.Description);
			if(job.Documentation==null) {
				job.Documentation="";
			}
			OdSqlParameter paramDocumentation=new OdSqlParameter("paramDocumentation",OdDbType.Text,job.Documentation);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription,paramDocumentation);
			}
			else {
				job.JobNum=Db.NonQ(command,true,paramDescription,paramDocumentation);
			}
			return job.JobNum;
		}

		///<summary>Updates one Job in the database.</summary>
		public static void Update(Job job){
			string command="UPDATE job SET "
				+"UserNumConcept        =  "+POut.Long  (job.UserNumConcept)+", "
				+"UserNumExpert         =  "+POut.Long  (job.UserNumExpert)+", "
				+"UserNumEngineer       =  "+POut.Long  (job.UserNumEngineer)+", "
				+"UserNumApproverConcept=  "+POut.Long  (job.UserNumApproverConcept)+", "
				+"UserNumApproverJob    =  "+POut.Long  (job.UserNumApproverJob)+", "
				+"UserNumApproverChange =  "+POut.Long  (job.UserNumApproverChange)+", "
				+"UserNumDocumenter     =  "+POut.Long  (job.UserNumDocumenter)+", "
				+"UserNumCustContact    =  "+POut.Long  (job.UserNumCustContact)+", "
				+"UserNumCheckout       =  "+POut.Long  (job.UserNumCheckout)+", "
				+"UserNumInfo           =  "+POut.Long  (job.UserNumInfo)+", "
				+"ParentNum             =  "+POut.Long  (job.ParentNum)+", "
				+"DateTimeCustContact   =  "+POut.DateT (job.DateTimeCustContact)+", "
				+"Priority              = '"+POut.String(job.Priority.ToString())+"', "
				+"Category              = '"+POut.String(job.Category.ToString())+"', "
				+"JobVersion            = '"+POut.String(job.JobVersion)+"', "
				+"HoursEstimate         =  "+POut.Int   (job.HoursEstimate)+", "
				+"HoursActual           =  "+POut.Int   (job.HoursActual)+", "
				//DateTimeEntry not allowed to change
				+"Description           =  "+DbHelper.ParamChar+"paramDescription, "
				+"Documentation         =  "+DbHelper.ParamChar+"paramDocumentation, "
				+"Title                 = '"+POut.String(job.Title)+"', "
				+"PhaseCur              = '"+POut.String(job.PhaseCur.ToString())+"', "
				+"IsApprovalNeeded      =  "+POut.Bool  (job.IsApprovalNeeded)+", "
				+"AckDateTime           =  "+POut.DateT (job.AckDateTime)+" "
				+"WHERE JobNum = "+POut.Long(job.JobNum);
			if(job.Description==null) {
				job.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,job.Description);
			if(job.Documentation==null) {
				job.Documentation="";
			}
			OdSqlParameter paramDocumentation=new OdSqlParameter("paramDocumentation",OdDbType.Text,job.Documentation);
			Db.NonQ(command,paramDescription,paramDocumentation);
		}

		///<summary>Updates one Job in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Job job,Job oldJob){
			string command="";
			if(job.UserNumConcept != oldJob.UserNumConcept) {
				if(command!=""){ command+=",";}
				command+="UserNumConcept = "+POut.Long(job.UserNumConcept)+"";
			}
			if(job.UserNumExpert != oldJob.UserNumExpert) {
				if(command!=""){ command+=",";}
				command+="UserNumExpert = "+POut.Long(job.UserNumExpert)+"";
			}
			if(job.UserNumEngineer != oldJob.UserNumEngineer) {
				if(command!=""){ command+=",";}
				command+="UserNumEngineer = "+POut.Long(job.UserNumEngineer)+"";
			}
			if(job.UserNumApproverConcept != oldJob.UserNumApproverConcept) {
				if(command!=""){ command+=",";}
				command+="UserNumApproverConcept = "+POut.Long(job.UserNumApproverConcept)+"";
			}
			if(job.UserNumApproverJob != oldJob.UserNumApproverJob) {
				if(command!=""){ command+=",";}
				command+="UserNumApproverJob = "+POut.Long(job.UserNumApproverJob)+"";
			}
			if(job.UserNumApproverChange != oldJob.UserNumApproverChange) {
				if(command!=""){ command+=",";}
				command+="UserNumApproverChange = "+POut.Long(job.UserNumApproverChange)+"";
			}
			if(job.UserNumDocumenter != oldJob.UserNumDocumenter) {
				if(command!=""){ command+=",";}
				command+="UserNumDocumenter = "+POut.Long(job.UserNumDocumenter)+"";
			}
			if(job.UserNumCustContact != oldJob.UserNumCustContact) {
				if(command!=""){ command+=",";}
				command+="UserNumCustContact = "+POut.Long(job.UserNumCustContact)+"";
			}
			if(job.UserNumCheckout != oldJob.UserNumCheckout) {
				if(command!=""){ command+=",";}
				command+="UserNumCheckout = "+POut.Long(job.UserNumCheckout)+"";
			}
			if(job.UserNumInfo != oldJob.UserNumInfo) {
				if(command!=""){ command+=",";}
				command+="UserNumInfo = "+POut.Long(job.UserNumInfo)+"";
			}
			if(job.ParentNum != oldJob.ParentNum) {
				if(command!=""){ command+=",";}
				command+="ParentNum = "+POut.Long(job.ParentNum)+"";
			}
			if(job.DateTimeCustContact != oldJob.DateTimeCustContact) {
				if(command!=""){ command+=",";}
				command+="DateTimeCustContact = "+POut.DateT(job.DateTimeCustContact)+"";
			}
			if(job.Priority != oldJob.Priority) {
				if(command!=""){ command+=",";}
				command+="Priority = '"+POut.String(job.Priority.ToString())+"'";
			}
			if(job.Category != oldJob.Category) {
				if(command!=""){ command+=",";}
				command+="Category = '"+POut.String(job.Category.ToString())+"'";
			}
			if(job.JobVersion != oldJob.JobVersion) {
				if(command!=""){ command+=",";}
				command+="JobVersion = '"+POut.String(job.JobVersion)+"'";
			}
			if(job.HoursEstimate != oldJob.HoursEstimate) {
				if(command!=""){ command+=",";}
				command+="HoursEstimate = "+POut.Int(job.HoursEstimate)+"";
			}
			if(job.HoursActual != oldJob.HoursActual) {
				if(command!=""){ command+=",";}
				command+="HoursActual = "+POut.Int(job.HoursActual)+"";
			}
			//DateTimeEntry not allowed to change
			if(job.Description != oldJob.Description) {
				if(command!=""){ command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(job.Documentation != oldJob.Documentation) {
				if(command!=""){ command+=",";}
				command+="Documentation = "+DbHelper.ParamChar+"paramDocumentation";
			}
			if(job.Title != oldJob.Title) {
				if(command!=""){ command+=",";}
				command+="Title = '"+POut.String(job.Title)+"'";
			}
			if(job.PhaseCur != oldJob.PhaseCur) {
				if(command!=""){ command+=",";}
				command+="PhaseCur = '"+POut.String(job.PhaseCur.ToString())+"'";
			}
			if(job.IsApprovalNeeded != oldJob.IsApprovalNeeded) {
				if(command!=""){ command+=",";}
				command+="IsApprovalNeeded = "+POut.Bool(job.IsApprovalNeeded)+"";
			}
			if(job.AckDateTime != oldJob.AckDateTime) {
				if(command!=""){ command+=",";}
				command+="AckDateTime = "+POut.DateT(job.AckDateTime)+"";
			}
			if(command==""){
				return false;
			}
			if(job.Description==null) {
				job.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,job.Description);
			if(job.Documentation==null) {
				job.Documentation="";
			}
			OdSqlParameter paramDocumentation=new OdSqlParameter("paramDocumentation",OdDbType.Text,job.Documentation);
			command="UPDATE job SET "+command
				+" WHERE JobNum = "+POut.Long(job.JobNum);
			Db.NonQ(command,paramDescription,paramDocumentation);
			return true;
		}

		///<summary>Returns true if Update(Job,Job) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Job job,Job oldJob) {
			if(job.UserNumConcept != oldJob.UserNumConcept) {
				return true;
			}
			if(job.UserNumExpert != oldJob.UserNumExpert) {
				return true;
			}
			if(job.UserNumEngineer != oldJob.UserNumEngineer) {
				return true;
			}
			if(job.UserNumApproverConcept != oldJob.UserNumApproverConcept) {
				return true;
			}
			if(job.UserNumApproverJob != oldJob.UserNumApproverJob) {
				return true;
			}
			if(job.UserNumApproverChange != oldJob.UserNumApproverChange) {
				return true;
			}
			if(job.UserNumDocumenter != oldJob.UserNumDocumenter) {
				return true;
			}
			if(job.UserNumCustContact != oldJob.UserNumCustContact) {
				return true;
			}
			if(job.UserNumCheckout != oldJob.UserNumCheckout) {
				return true;
			}
			if(job.UserNumInfo != oldJob.UserNumInfo) {
				return true;
			}
			if(job.ParentNum != oldJob.ParentNum) {
				return true;
			}
			if(job.DateTimeCustContact != oldJob.DateTimeCustContact) {
				return true;
			}
			if(job.Priority != oldJob.Priority) {
				return true;
			}
			if(job.Category != oldJob.Category) {
				return true;
			}
			if(job.JobVersion != oldJob.JobVersion) {
				return true;
			}
			if(job.HoursEstimate != oldJob.HoursEstimate) {
				return true;
			}
			if(job.HoursActual != oldJob.HoursActual) {
				return true;
			}
			//DateTimeEntry not allowed to change
			if(job.Description != oldJob.Description) {
				return true;
			}
			if(job.Documentation != oldJob.Documentation) {
				return true;
			}
			if(job.Title != oldJob.Title) {
				return true;
			}
			if(job.PhaseCur != oldJob.PhaseCur) {
				return true;
			}
			if(job.IsApprovalNeeded != oldJob.IsApprovalNeeded) {
				return true;
			}
			if(job.AckDateTime != oldJob.AckDateTime) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Job from the database.</summary>
		public static void Delete(long jobNum){
			string command="DELETE FROM job "
				+"WHERE JobNum = "+POut.Long(jobNum);
			Db.NonQ(command);
		}

	}
}