//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EhrMeasureEventCrud {
		///<summary>Gets one EhrMeasureEvent object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrMeasureEvent SelectOne(long ehrMeasureEventNum){
			string command="SELECT * FROM ehrmeasureevent "
				+"WHERE EhrMeasureEventNum = "+POut.Long(ehrMeasureEventNum);
			List<EhrMeasureEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrMeasureEvent object from the database using a query.</summary>
		public static EhrMeasureEvent SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrMeasureEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrMeasureEvent objects from the database using a query.</summary>
		public static List<EhrMeasureEvent> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrMeasureEvent> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrMeasureEvent> TableToList(DataTable table){
			List<EhrMeasureEvent> retVal=new List<EhrMeasureEvent>();
			EhrMeasureEvent ehrMeasureEvent;
			foreach(DataRow row in table.Rows) {
				ehrMeasureEvent=new EhrMeasureEvent();
				ehrMeasureEvent.EhrMeasureEventNum    = PIn.Long  (row["EhrMeasureEventNum"].ToString());
				ehrMeasureEvent.DateTEvent            = PIn.DateT (row["DateTEvent"].ToString());
				ehrMeasureEvent.EventType             = (OpenDentBusiness.EhrMeasureEventType)PIn.Int(row["EventType"].ToString());
				ehrMeasureEvent.PatNum                = PIn.Long  (row["PatNum"].ToString());
				ehrMeasureEvent.MoreInfo              = PIn.String(row["MoreInfo"].ToString());
				ehrMeasureEvent.CodeValueEvent        = PIn.String(row["CodeValueEvent"].ToString());
				ehrMeasureEvent.CodeSystemEvent       = PIn.String(row["CodeSystemEvent"].ToString());
				ehrMeasureEvent.CodeValueResult       = PIn.String(row["CodeValueResult"].ToString());
				ehrMeasureEvent.CodeSystemResult      = PIn.String(row["CodeSystemResult"].ToString());
				ehrMeasureEvent.FKey                  = PIn.Long  (row["FKey"].ToString());
				ehrMeasureEvent.DateStartTobacco      = PIn.Date  (row["DateStartTobacco"].ToString());
				ehrMeasureEvent.TobaccoCessationDesire= PIn.Byte  (row["TobaccoCessationDesire"].ToString());
				retVal.Add(ehrMeasureEvent);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrMeasureEvent into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrMeasureEvent> listEhrMeasureEvents,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrMeasureEvent";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrMeasureEventNum");
			table.Columns.Add("DateTEvent");
			table.Columns.Add("EventType");
			table.Columns.Add("PatNum");
			table.Columns.Add("MoreInfo");
			table.Columns.Add("CodeValueEvent");
			table.Columns.Add("CodeSystemEvent");
			table.Columns.Add("CodeValueResult");
			table.Columns.Add("CodeSystemResult");
			table.Columns.Add("FKey");
			table.Columns.Add("DateStartTobacco");
			table.Columns.Add("TobaccoCessationDesire");
			foreach(EhrMeasureEvent ehrMeasureEvent in listEhrMeasureEvents) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrMeasureEvent.EhrMeasureEventNum),
					POut.DateT (ehrMeasureEvent.DateTEvent,false),
					POut.Int   ((int)ehrMeasureEvent.EventType),
					POut.Long  (ehrMeasureEvent.PatNum),
					            ehrMeasureEvent.MoreInfo,
					            ehrMeasureEvent.CodeValueEvent,
					            ehrMeasureEvent.CodeSystemEvent,
					            ehrMeasureEvent.CodeValueResult,
					            ehrMeasureEvent.CodeSystemResult,
					POut.Long  (ehrMeasureEvent.FKey),
					POut.DateT (ehrMeasureEvent.DateStartTobacco,false),
					POut.Byte  (ehrMeasureEvent.TobaccoCessationDesire),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrMeasureEvent into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrMeasureEvent ehrMeasureEvent){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				ehrMeasureEvent.EhrMeasureEventNum=DbHelper.GetNextOracleKey("ehrmeasureevent","EhrMeasureEventNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(ehrMeasureEvent,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							ehrMeasureEvent.EhrMeasureEventNum++;
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
				return Insert(ehrMeasureEvent,false);
			}
		}

		///<summary>Inserts one EhrMeasureEvent into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrMeasureEvent ehrMeasureEvent,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrMeasureEvent.EhrMeasureEventNum=ReplicationServers.GetKey("ehrmeasureevent","EhrMeasureEventNum");
			}
			string command="INSERT INTO ehrmeasureevent (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrMeasureEventNum,";
			}
			command+="DateTEvent,EventType,PatNum,MoreInfo,CodeValueEvent,CodeSystemEvent,CodeValueResult,CodeSystemResult,FKey,DateStartTobacco,TobaccoCessationDesire) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrMeasureEvent.EhrMeasureEventNum)+",";
			}
			command+=
				     POut.DateT (ehrMeasureEvent.DateTEvent)+","
				+    POut.Int   ((int)ehrMeasureEvent.EventType)+","
				+    POut.Long  (ehrMeasureEvent.PatNum)+","
				+"'"+POut.String(ehrMeasureEvent.MoreInfo)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeValueEvent)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeSystemEvent)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeValueResult)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeSystemResult)+"',"
				+    POut.Long  (ehrMeasureEvent.FKey)+","
				+    POut.Date  (ehrMeasureEvent.DateStartTobacco)+","
				+    POut.Byte  (ehrMeasureEvent.TobaccoCessationDesire)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrMeasureEvent.EhrMeasureEventNum=Db.NonQ(command,true);
			}
			return ehrMeasureEvent.EhrMeasureEventNum;
		}

		///<summary>Inserts one EhrMeasureEvent into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrMeasureEvent ehrMeasureEvent){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(ehrMeasureEvent,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					ehrMeasureEvent.EhrMeasureEventNum=DbHelper.GetNextOracleKey("ehrmeasureevent","EhrMeasureEventNum"); //Cacheless method
				}
				return InsertNoCache(ehrMeasureEvent,true);
			}
		}

		///<summary>Inserts one EhrMeasureEvent into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrMeasureEvent ehrMeasureEvent,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrmeasureevent (";
			if(!useExistingPK && isRandomKeys) {
				ehrMeasureEvent.EhrMeasureEventNum=ReplicationServers.GetKeyNoCache("ehrmeasureevent","EhrMeasureEventNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrMeasureEventNum,";
			}
			command+="DateTEvent,EventType,PatNum,MoreInfo,CodeValueEvent,CodeSystemEvent,CodeValueResult,CodeSystemResult,FKey,DateStartTobacco,TobaccoCessationDesire) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrMeasureEvent.EhrMeasureEventNum)+",";
			}
			command+=
				     POut.DateT (ehrMeasureEvent.DateTEvent)+","
				+    POut.Int   ((int)ehrMeasureEvent.EventType)+","
				+    POut.Long  (ehrMeasureEvent.PatNum)+","
				+"'"+POut.String(ehrMeasureEvent.MoreInfo)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeValueEvent)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeSystemEvent)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeValueResult)+"',"
				+"'"+POut.String(ehrMeasureEvent.CodeSystemResult)+"',"
				+    POut.Long  (ehrMeasureEvent.FKey)+","
				+    POut.Date  (ehrMeasureEvent.DateStartTobacco)+","
				+    POut.Byte  (ehrMeasureEvent.TobaccoCessationDesire)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrMeasureEvent.EhrMeasureEventNum=Db.NonQ(command,true);
			}
			return ehrMeasureEvent.EhrMeasureEventNum;
		}

		///<summary>Updates one EhrMeasureEvent in the database.</summary>
		public static void Update(EhrMeasureEvent ehrMeasureEvent){
			string command="UPDATE ehrmeasureevent SET "
				+"DateTEvent            =  "+POut.DateT (ehrMeasureEvent.DateTEvent)+", "
				+"EventType             =  "+POut.Int   ((int)ehrMeasureEvent.EventType)+", "
				+"PatNum                =  "+POut.Long  (ehrMeasureEvent.PatNum)+", "
				+"MoreInfo              = '"+POut.String(ehrMeasureEvent.MoreInfo)+"', "
				+"CodeValueEvent        = '"+POut.String(ehrMeasureEvent.CodeValueEvent)+"', "
				+"CodeSystemEvent       = '"+POut.String(ehrMeasureEvent.CodeSystemEvent)+"', "
				+"CodeValueResult       = '"+POut.String(ehrMeasureEvent.CodeValueResult)+"', "
				+"CodeSystemResult      = '"+POut.String(ehrMeasureEvent.CodeSystemResult)+"', "
				+"FKey                  =  "+POut.Long  (ehrMeasureEvent.FKey)+", "
				+"DateStartTobacco      =  "+POut.Date  (ehrMeasureEvent.DateStartTobacco)+", "
				+"TobaccoCessationDesire=  "+POut.Byte  (ehrMeasureEvent.TobaccoCessationDesire)+" "
				+"WHERE EhrMeasureEventNum = "+POut.Long(ehrMeasureEvent.EhrMeasureEventNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrMeasureEvent in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrMeasureEvent ehrMeasureEvent,EhrMeasureEvent oldEhrMeasureEvent){
			string command="";
			if(ehrMeasureEvent.DateTEvent != oldEhrMeasureEvent.DateTEvent) {
				if(command!=""){ command+=",";}
				command+="DateTEvent = "+POut.DateT(ehrMeasureEvent.DateTEvent)+"";
			}
			if(ehrMeasureEvent.EventType != oldEhrMeasureEvent.EventType) {
				if(command!=""){ command+=",";}
				command+="EventType = "+POut.Int   ((int)ehrMeasureEvent.EventType)+"";
			}
			if(ehrMeasureEvent.PatNum != oldEhrMeasureEvent.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(ehrMeasureEvent.PatNum)+"";
			}
			if(ehrMeasureEvent.MoreInfo != oldEhrMeasureEvent.MoreInfo) {
				if(command!=""){ command+=",";}
				command+="MoreInfo = '"+POut.String(ehrMeasureEvent.MoreInfo)+"'";
			}
			if(ehrMeasureEvent.CodeValueEvent != oldEhrMeasureEvent.CodeValueEvent) {
				if(command!=""){ command+=",";}
				command+="CodeValueEvent = '"+POut.String(ehrMeasureEvent.CodeValueEvent)+"'";
			}
			if(ehrMeasureEvent.CodeSystemEvent != oldEhrMeasureEvent.CodeSystemEvent) {
				if(command!=""){ command+=",";}
				command+="CodeSystemEvent = '"+POut.String(ehrMeasureEvent.CodeSystemEvent)+"'";
			}
			if(ehrMeasureEvent.CodeValueResult != oldEhrMeasureEvent.CodeValueResult) {
				if(command!=""){ command+=",";}
				command+="CodeValueResult = '"+POut.String(ehrMeasureEvent.CodeValueResult)+"'";
			}
			if(ehrMeasureEvent.CodeSystemResult != oldEhrMeasureEvent.CodeSystemResult) {
				if(command!=""){ command+=",";}
				command+="CodeSystemResult = '"+POut.String(ehrMeasureEvent.CodeSystemResult)+"'";
			}
			if(ehrMeasureEvent.FKey != oldEhrMeasureEvent.FKey) {
				if(command!=""){ command+=",";}
				command+="FKey = "+POut.Long(ehrMeasureEvent.FKey)+"";
			}
			if(ehrMeasureEvent.DateStartTobacco.Date != oldEhrMeasureEvent.DateStartTobacco.Date) {
				if(command!=""){ command+=",";}
				command+="DateStartTobacco = "+POut.Date(ehrMeasureEvent.DateStartTobacco)+"";
			}
			if(ehrMeasureEvent.TobaccoCessationDesire != oldEhrMeasureEvent.TobaccoCessationDesire) {
				if(command!=""){ command+=",";}
				command+="TobaccoCessationDesire = "+POut.Byte(ehrMeasureEvent.TobaccoCessationDesire)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE ehrmeasureevent SET "+command
				+" WHERE EhrMeasureEventNum = "+POut.Long(ehrMeasureEvent.EhrMeasureEventNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EhrMeasureEvent,EhrMeasureEvent) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrMeasureEvent ehrMeasureEvent,EhrMeasureEvent oldEhrMeasureEvent) {
			if(ehrMeasureEvent.DateTEvent != oldEhrMeasureEvent.DateTEvent) {
				return true;
			}
			if(ehrMeasureEvent.EventType != oldEhrMeasureEvent.EventType) {
				return true;
			}
			if(ehrMeasureEvent.PatNum != oldEhrMeasureEvent.PatNum) {
				return true;
			}
			if(ehrMeasureEvent.MoreInfo != oldEhrMeasureEvent.MoreInfo) {
				return true;
			}
			if(ehrMeasureEvent.CodeValueEvent != oldEhrMeasureEvent.CodeValueEvent) {
				return true;
			}
			if(ehrMeasureEvent.CodeSystemEvent != oldEhrMeasureEvent.CodeSystemEvent) {
				return true;
			}
			if(ehrMeasureEvent.CodeValueResult != oldEhrMeasureEvent.CodeValueResult) {
				return true;
			}
			if(ehrMeasureEvent.CodeSystemResult != oldEhrMeasureEvent.CodeSystemResult) {
				return true;
			}
			if(ehrMeasureEvent.FKey != oldEhrMeasureEvent.FKey) {
				return true;
			}
			if(ehrMeasureEvent.DateStartTobacco.Date != oldEhrMeasureEvent.DateStartTobacco.Date) {
				return true;
			}
			if(ehrMeasureEvent.TobaccoCessationDesire != oldEhrMeasureEvent.TobaccoCessationDesire) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrMeasureEvent from the database.</summary>
		public static void Delete(long ehrMeasureEventNum){
			string command="DELETE FROM ehrmeasureevent "
				+"WHERE EhrMeasureEventNum = "+POut.Long(ehrMeasureEventNum);
			Db.NonQ(command);
		}

	}
}