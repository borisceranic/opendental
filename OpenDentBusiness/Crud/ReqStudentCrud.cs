//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ReqStudentCrud {
		///<summary>Gets one ReqStudent object from the database using the primary key.  Returns null if not found.</summary>
		public static ReqStudent SelectOne(long reqStudentNum){
			string command="SELECT * FROM reqstudent "
				+"WHERE ReqStudentNum = "+POut.Long(reqStudentNum);
			List<ReqStudent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ReqStudent object from the database using a query.</summary>
		public static ReqStudent SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ReqStudent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ReqStudent objects from the database using a query.</summary>
		public static List<ReqStudent> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ReqStudent> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ReqStudent> TableToList(DataTable table){
			List<ReqStudent> retVal=new List<ReqStudent>();
			ReqStudent reqStudent;
			for(int i=0;i<table.Rows.Count;i++) {
				reqStudent=new ReqStudent();
				reqStudent.ReqStudentNum  = PIn.Long  (table.Rows[i]["ReqStudentNum"].ToString());
				reqStudent.ReqNeededNum   = PIn.Long  (table.Rows[i]["ReqNeededNum"].ToString());
				reqStudent.Descript       = PIn.String(table.Rows[i]["Descript"].ToString());
				reqStudent.SchoolCourseNum= PIn.Long  (table.Rows[i]["SchoolCourseNum"].ToString());
				reqStudent.ProvNum        = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				reqStudent.AptNum         = PIn.Long  (table.Rows[i]["AptNum"].ToString());
				reqStudent.PatNum         = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				reqStudent.InstructorNum  = PIn.Long  (table.Rows[i]["InstructorNum"].ToString());
				reqStudent.DateCompleted  = PIn.Date  (table.Rows[i]["DateCompleted"].ToString());
				retVal.Add(reqStudent);
			}
			return retVal;
		}

		///<summary>Inserts one ReqStudent into the database.  Returns the new priKey.</summary>
		public static long Insert(ReqStudent reqStudent){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				reqStudent.ReqStudentNum=DbHelper.GetNextOracleKey("reqstudent","ReqStudentNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(reqStudent,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							reqStudent.ReqStudentNum++;
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
				return Insert(reqStudent,false);
			}
		}

		///<summary>Inserts one ReqStudent into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ReqStudent reqStudent,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				reqStudent.ReqStudentNum=ReplicationServers.GetKey("reqstudent","ReqStudentNum");
			}
			string command="INSERT INTO reqstudent (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ReqStudentNum,";
			}
			command+="ReqNeededNum,Descript,SchoolCourseNum,ProvNum,AptNum,PatNum,InstructorNum,DateCompleted) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(reqStudent.ReqStudentNum)+",";
			}
			command+=
				     POut.Long  (reqStudent.ReqNeededNum)+","
				+"'"+POut.String(reqStudent.Descript)+"',"
				+    POut.Long  (reqStudent.SchoolCourseNum)+","
				+    POut.Long  (reqStudent.ProvNum)+","
				+    POut.Long  (reqStudent.AptNum)+","
				+    POut.Long  (reqStudent.PatNum)+","
				+    POut.Long  (reqStudent.InstructorNum)+","
				+    POut.Date  (reqStudent.DateCompleted)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				reqStudent.ReqStudentNum=Db.NonQ(command,true);
			}
			return reqStudent.ReqStudentNum;
		}

		///<summary>Inserts one ReqStudent into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReqStudent reqStudent){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(reqStudent,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					reqStudent.ReqStudentNum=DbHelper.GetNextOracleKey("reqstudent","ReqStudentNum"); //Cacheless method
				}
				return InsertNoCache(reqStudent,true);
			}
		}

		///<summary>Inserts one ReqStudent into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ReqStudent reqStudent,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO reqstudent (";
			if(!useExistingPK && isRandomKeys) {
				reqStudent.ReqStudentNum=ReplicationServers.GetKeyNoCache("reqstudent","ReqStudentNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ReqStudentNum,";
			}
			command+="ReqNeededNum,Descript,SchoolCourseNum,ProvNum,AptNum,PatNum,InstructorNum,DateCompleted) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(reqStudent.ReqStudentNum)+",";
			}
			command+=
				     POut.Long  (reqStudent.ReqNeededNum)+","
				+"'"+POut.String(reqStudent.Descript)+"',"
				+    POut.Long  (reqStudent.SchoolCourseNum)+","
				+    POut.Long  (reqStudent.ProvNum)+","
				+    POut.Long  (reqStudent.AptNum)+","
				+    POut.Long  (reqStudent.PatNum)+","
				+    POut.Long  (reqStudent.InstructorNum)+","
				+    POut.Date  (reqStudent.DateCompleted)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				reqStudent.ReqStudentNum=Db.NonQ(command,true);
			}
			return reqStudent.ReqStudentNum;
		}

		///<summary>Updates one ReqStudent in the database.</summary>
		public static void Update(ReqStudent reqStudent){
			string command="UPDATE reqstudent SET "
				+"ReqNeededNum   =  "+POut.Long  (reqStudent.ReqNeededNum)+", "
				+"Descript       = '"+POut.String(reqStudent.Descript)+"', "
				+"SchoolCourseNum=  "+POut.Long  (reqStudent.SchoolCourseNum)+", "
				+"ProvNum        =  "+POut.Long  (reqStudent.ProvNum)+", "
				+"AptNum         =  "+POut.Long  (reqStudent.AptNum)+", "
				+"PatNum         =  "+POut.Long  (reqStudent.PatNum)+", "
				+"InstructorNum  =  "+POut.Long  (reqStudent.InstructorNum)+", "
				+"DateCompleted  =  "+POut.Date  (reqStudent.DateCompleted)+" "
				+"WHERE ReqStudentNum = "+POut.Long(reqStudent.ReqStudentNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ReqStudent in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ReqStudent reqStudent,ReqStudent oldReqStudent){
			string command="";
			if(reqStudent.ReqNeededNum != oldReqStudent.ReqNeededNum) {
				if(command!=""){ command+=",";}
				command+="ReqNeededNum = "+POut.Long(reqStudent.ReqNeededNum)+"";
			}
			if(reqStudent.Descript != oldReqStudent.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(reqStudent.Descript)+"'";
			}
			if(reqStudent.SchoolCourseNum != oldReqStudent.SchoolCourseNum) {
				if(command!=""){ command+=",";}
				command+="SchoolCourseNum = "+POut.Long(reqStudent.SchoolCourseNum)+"";
			}
			if(reqStudent.ProvNum != oldReqStudent.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(reqStudent.ProvNum)+"";
			}
			if(reqStudent.AptNum != oldReqStudent.AptNum) {
				if(command!=""){ command+=",";}
				command+="AptNum = "+POut.Long(reqStudent.AptNum)+"";
			}
			if(reqStudent.PatNum != oldReqStudent.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(reqStudent.PatNum)+"";
			}
			if(reqStudent.InstructorNum != oldReqStudent.InstructorNum) {
				if(command!=""){ command+=",";}
				command+="InstructorNum = "+POut.Long(reqStudent.InstructorNum)+"";
			}
			if(reqStudent.DateCompleted != oldReqStudent.DateCompleted) {
				if(command!=""){ command+=",";}
				command+="DateCompleted = "+POut.Date(reqStudent.DateCompleted)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE reqstudent SET "+command
				+" WHERE ReqStudentNum = "+POut.Long(reqStudent.ReqStudentNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ReqStudent from the database.</summary>
		public static void Delete(long reqStudentNum){
			string command="DELETE FROM reqstudent "
				+"WHERE ReqStudentNum = "+POut.Long(reqStudentNum);
			Db.NonQ(command);
		}

	}
}