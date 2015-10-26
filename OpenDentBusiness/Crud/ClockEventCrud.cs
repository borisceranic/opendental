//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ClockEventCrud {
		///<summary>Gets one ClockEvent object from the database using the primary key.  Returns null if not found.</summary>
		public static ClockEvent SelectOne(long clockEventNum){
			string command="SELECT * FROM clockevent "
				+"WHERE ClockEventNum = "+POut.Long(clockEventNum);
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ClockEvent object from the database using a query.</summary>
		public static ClockEvent SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ClockEvent objects from the database using a query.</summary>
		public static List<ClockEvent> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ClockEvent> TableToList(DataTable table){
			List<ClockEvent> retVal=new List<ClockEvent>();
			ClockEvent clockEvent;
			foreach(DataRow row in table.Rows) {
				clockEvent=new ClockEvent();
				clockEvent.ClockEventNum     = PIn.Long  (row["ClockEventNum"].ToString());
				clockEvent.EmployeeNum       = PIn.Long  (row["EmployeeNum"].ToString());
				clockEvent.TimeEntered1      = PIn.DateT (row["TimeEntered1"].ToString());
				clockEvent.TimeDisplayed1    = PIn.DateT (row["TimeDisplayed1"].ToString());
				clockEvent.ClockStatus       = (OpenDentBusiness.TimeClockStatus)PIn.Int(row["ClockStatus"].ToString());
				clockEvent.Note              = PIn.String(row["Note"].ToString());
				clockEvent.TimeEntered2      = PIn.DateT (row["TimeEntered2"].ToString());
				clockEvent.TimeDisplayed2    = PIn.DateT (row["TimeDisplayed2"].ToString());
				clockEvent.OTimeHours        = PIn.TSpan (row["OTimeHours"].ToString());
				clockEvent.OTimeAuto         = PIn.TSpan (row["OTimeAuto"].ToString());
				clockEvent.Adjust            = PIn.TSpan (row["Adjust"].ToString());
				clockEvent.AdjustAuto        = PIn.TSpan (row["AdjustAuto"].ToString());
				clockEvent.AdjustIsOverridden= PIn.Bool  (row["AdjustIsOverridden"].ToString());
				clockEvent.Rate2Hours        = PIn.TSpan (row["Rate2Hours"].ToString());
				clockEvent.Rate2Auto         = PIn.TSpan (row["Rate2Auto"].ToString());
				retVal.Add(clockEvent);
			}
			return retVal;
		}

		///<summary>Inserts one ClockEvent into the database.  Returns the new priKey.</summary>
		public static long Insert(ClockEvent clockEvent){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				clockEvent.ClockEventNum=DbHelper.GetNextOracleKey("clockevent","ClockEventNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(clockEvent,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							clockEvent.ClockEventNum++;
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
				return Insert(clockEvent,false);
			}
		}

		///<summary>Inserts one ClockEvent into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ClockEvent clockEvent,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				clockEvent.ClockEventNum=ReplicationServers.GetKey("clockevent","ClockEventNum");
			}
			string command="INSERT INTO clockevent (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClockEventNum,";
			}
			command+="EmployeeNum,TimeEntered1,TimeDisplayed1,ClockStatus,Note,TimeEntered2,TimeDisplayed2,OTimeHours,OTimeAuto,Adjust,AdjustAuto,AdjustIsOverridden,Rate2Hours,Rate2Auto) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(clockEvent.ClockEventNum)+",";
			}
			command+=
				     POut.Long  (clockEvent.EmployeeNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.Now()+","
				+    POut.Int   ((int)clockEvent.ClockStatus)+","
				+"'"+POut.String(clockEvent.Note)+"',"
				+    POut.DateT (clockEvent.TimeEntered2)+","
				+    POut.DateT (clockEvent.TimeDisplayed2)+","
				+"'"+POut.TSpan (clockEvent.OTimeHours)+"',"
				+"'"+POut.TSpan (clockEvent.OTimeAuto)+"',"
				+"'"+POut.TSpan (clockEvent.Adjust)+"',"
				+"'"+POut.TSpan (clockEvent.AdjustAuto)+"',"
				+    POut.Bool  (clockEvent.AdjustIsOverridden)+","
				+"'"+POut.TSpan (clockEvent.Rate2Hours)+"',"
				+"'"+POut.TSpan (clockEvent.Rate2Auto)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				clockEvent.ClockEventNum=Db.NonQ(command,true);
			}
			return clockEvent.ClockEventNum;
		}

		///<summary>Inserts one ClockEvent into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClockEvent clockEvent){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(clockEvent,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					clockEvent.ClockEventNum=DbHelper.GetNextOracleKey("clockevent","ClockEventNum"); //Cacheless method
				}
				return InsertNoCache(clockEvent,true);
			}
		}

		///<summary>Inserts one ClockEvent into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ClockEvent clockEvent,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO clockevent (";
			if(!useExistingPK && isRandomKeys) {
				clockEvent.ClockEventNum=ReplicationServers.GetKeyNoCache("clockevent","ClockEventNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ClockEventNum,";
			}
			command+="EmployeeNum,TimeEntered1,TimeDisplayed1,ClockStatus,Note,TimeEntered2,TimeDisplayed2,OTimeHours,OTimeAuto,Adjust,AdjustAuto,AdjustIsOverridden,Rate2Hours,Rate2Auto) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(clockEvent.ClockEventNum)+",";
			}
			command+=
				     POut.Long  (clockEvent.EmployeeNum)+","
				+    DbHelper.Now()+","
				+    DbHelper.Now()+","
				+    POut.Int   ((int)clockEvent.ClockStatus)+","
				+"'"+POut.String(clockEvent.Note)+"',"
				+    POut.DateT (clockEvent.TimeEntered2)+","
				+    POut.DateT (clockEvent.TimeDisplayed2)+","
				+"'"+POut.TSpan (clockEvent.OTimeHours)+"',"
				+"'"+POut.TSpan (clockEvent.OTimeAuto)+"',"
				+"'"+POut.TSpan (clockEvent.Adjust)+"',"
				+"'"+POut.TSpan (clockEvent.AdjustAuto)+"',"
				+    POut.Bool  (clockEvent.AdjustIsOverridden)+","
				+"'"+POut.TSpan (clockEvent.Rate2Hours)+"',"
				+"'"+POut.TSpan (clockEvent.Rate2Auto)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				clockEvent.ClockEventNum=Db.NonQ(command,true);
			}
			return clockEvent.ClockEventNum;
		}

		///<summary>Updates one ClockEvent in the database.</summary>
		public static void Update(ClockEvent clockEvent){
			string command="UPDATE clockevent SET "
				+"EmployeeNum       =  "+POut.Long  (clockEvent.EmployeeNum)+", "
				//TimeEntered1 not allowed to change
				+"TimeDisplayed1    =  "+POut.DateT (clockEvent.TimeDisplayed1)+", "
				+"ClockStatus       =  "+POut.Int   ((int)clockEvent.ClockStatus)+", "
				+"Note              = '"+POut.String(clockEvent.Note)+"', "
				+"TimeEntered2      =  "+POut.DateT (clockEvent.TimeEntered2)+", "
				+"TimeDisplayed2    =  "+POut.DateT (clockEvent.TimeDisplayed2)+", "
				+"OTimeHours        = '"+POut.TSpan (clockEvent.OTimeHours)+"', "
				+"OTimeAuto         = '"+POut.TSpan (clockEvent.OTimeAuto)+"', "
				+"Adjust            = '"+POut.TSpan (clockEvent.Adjust)+"', "
				+"AdjustAuto        = '"+POut.TSpan (clockEvent.AdjustAuto)+"', "
				+"AdjustIsOverridden=  "+POut.Bool  (clockEvent.AdjustIsOverridden)+", "
				+"Rate2Hours        = '"+POut.TSpan (clockEvent.Rate2Hours)+"', "
				+"Rate2Auto         = '"+POut.TSpan (clockEvent.Rate2Auto)+"' "
				+"WHERE ClockEventNum = "+POut.Long(clockEvent.ClockEventNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ClockEvent in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ClockEvent clockEvent,ClockEvent oldClockEvent){
			string command="";
			if(clockEvent.EmployeeNum != oldClockEvent.EmployeeNum) {
				if(command!=""){ command+=",";}
				command+="EmployeeNum = "+POut.Long(clockEvent.EmployeeNum)+"";
			}
			//TimeEntered1 not allowed to change
			if(clockEvent.TimeDisplayed1 != oldClockEvent.TimeDisplayed1) {
				if(command!=""){ command+=",";}
				command+="TimeDisplayed1 = "+POut.DateT(clockEvent.TimeDisplayed1)+"";
			}
			if(clockEvent.ClockStatus != oldClockEvent.ClockStatus) {
				if(command!=""){ command+=",";}
				command+="ClockStatus = "+POut.Int   ((int)clockEvent.ClockStatus)+"";
			}
			if(clockEvent.Note != oldClockEvent.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(clockEvent.Note)+"'";
			}
			if(clockEvent.TimeEntered2 != oldClockEvent.TimeEntered2) {
				if(command!=""){ command+=",";}
				command+="TimeEntered2 = "+POut.DateT(clockEvent.TimeEntered2)+"";
			}
			if(clockEvent.TimeDisplayed2 != oldClockEvent.TimeDisplayed2) {
				if(command!=""){ command+=",";}
				command+="TimeDisplayed2 = "+POut.DateT(clockEvent.TimeDisplayed2)+"";
			}
			if(clockEvent.OTimeHours != oldClockEvent.OTimeHours) {
				if(command!=""){ command+=",";}
				command+="OTimeHours = '"+POut.TSpan (clockEvent.OTimeHours)+"'";
			}
			if(clockEvent.OTimeAuto != oldClockEvent.OTimeAuto) {
				if(command!=""){ command+=",";}
				command+="OTimeAuto = '"+POut.TSpan (clockEvent.OTimeAuto)+"'";
			}
			if(clockEvent.Adjust != oldClockEvent.Adjust) {
				if(command!=""){ command+=",";}
				command+="Adjust = '"+POut.TSpan (clockEvent.Adjust)+"'";
			}
			if(clockEvent.AdjustAuto != oldClockEvent.AdjustAuto) {
				if(command!=""){ command+=",";}
				command+="AdjustAuto = '"+POut.TSpan (clockEvent.AdjustAuto)+"'";
			}
			if(clockEvent.AdjustIsOverridden != oldClockEvent.AdjustIsOverridden) {
				if(command!=""){ command+=",";}
				command+="AdjustIsOverridden = "+POut.Bool(clockEvent.AdjustIsOverridden)+"";
			}
			if(clockEvent.Rate2Hours != oldClockEvent.Rate2Hours) {
				if(command!=""){ command+=",";}
				command+="Rate2Hours = '"+POut.TSpan (clockEvent.Rate2Hours)+"'";
			}
			if(clockEvent.Rate2Auto != oldClockEvent.Rate2Auto) {
				if(command!=""){ command+=",";}
				command+="Rate2Auto = '"+POut.TSpan (clockEvent.Rate2Auto)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE clockevent SET "+command
				+" WHERE ClockEventNum = "+POut.Long(clockEvent.ClockEventNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ClockEvent from the database.</summary>
		public static void Delete(long clockEventNum){
			string command="DELETE FROM clockevent "
				+"WHERE ClockEventNum = "+POut.Long(clockEventNum);
			Db.NonQ(command);
		}

	}
}