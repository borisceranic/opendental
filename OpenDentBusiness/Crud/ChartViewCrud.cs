//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ChartViewCrud {
		///<summary>Gets one ChartView object from the database using the primary key.  Returns null if not found.</summary>
		public static ChartView SelectOne(long chartViewNum){
			string command="SELECT * FROM chartview "
				+"WHERE ChartViewNum = "+POut.Long(chartViewNum);
			List<ChartView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ChartView object from the database using a query.</summary>
		public static ChartView SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ChartView> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ChartView objects from the database using a query.</summary>
		public static List<ChartView> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ChartView> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ChartView> TableToList(DataTable table){
			List<ChartView> retVal=new List<ChartView>();
			ChartView chartView;
			foreach(DataRow row in table.Rows) {
				chartView=new ChartView();
				chartView.ChartViewNum     = PIn.Long  (row["ChartViewNum"].ToString());
				chartView.Description      = PIn.String(row["Description"].ToString());
				chartView.ItemOrder        = PIn.Int   (row["ItemOrder"].ToString());
				chartView.ProcStatuses     = (OpenDentBusiness.ChartViewProcStat)PIn.Int(row["ProcStatuses"].ToString());
				chartView.ObjectTypes      = (OpenDentBusiness.ChartViewObjs)PIn.Int(row["ObjectTypes"].ToString());
				chartView.ShowProcNotes    = PIn.Bool  (row["ShowProcNotes"].ToString());
				chartView.IsAudit          = PIn.Bool  (row["IsAudit"].ToString());
				chartView.SelectedTeethOnly= PIn.Bool  (row["SelectedTeethOnly"].ToString());
				chartView.OrionStatusFlags = (OpenDentBusiness.OrionStatus)PIn.Int(row["OrionStatusFlags"].ToString());
				chartView.DatesShowing     = (OpenDentBusiness.ChartViewDates)PIn.Int(row["DatesShowing"].ToString());
				chartView.IsTpCharting     = PIn.Bool  (row["IsTpCharting"].ToString());
				retVal.Add(chartView);
			}
			return retVal;
		}

		///<summary>Inserts one ChartView into the database.  Returns the new priKey.</summary>
		public static long Insert(ChartView chartView){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				chartView.ChartViewNum=DbHelper.GetNextOracleKey("chartview","ChartViewNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(chartView,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							chartView.ChartViewNum++;
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
				return Insert(chartView,false);
			}
		}

		///<summary>Inserts one ChartView into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ChartView chartView,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				chartView.ChartViewNum=ReplicationServers.GetKey("chartview","ChartViewNum");
			}
			string command="INSERT INTO chartview (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ChartViewNum,";
			}
			command+="Description,ItemOrder,ProcStatuses,ObjectTypes,ShowProcNotes,IsAudit,SelectedTeethOnly,OrionStatusFlags,DatesShowing,IsTpCharting) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(chartView.ChartViewNum)+",";
			}
			command+=
				 "'"+POut.String(chartView.Description)+"',"
				+    POut.Int   (chartView.ItemOrder)+","
				+    POut.Int   ((int)chartView.ProcStatuses)+","
				+    POut.Int   ((int)chartView.ObjectTypes)+","
				+    POut.Bool  (chartView.ShowProcNotes)+","
				+    POut.Bool  (chartView.IsAudit)+","
				+    POut.Bool  (chartView.SelectedTeethOnly)+","
				+    POut.Int   ((int)chartView.OrionStatusFlags)+","
				+    POut.Int   ((int)chartView.DatesShowing)+","
				+    POut.Bool  (chartView.IsTpCharting)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				chartView.ChartViewNum=Db.NonQ(command,true);
			}
			return chartView.ChartViewNum;
		}

		///<summary>Inserts one ChartView into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChartView chartView){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(chartView,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					chartView.ChartViewNum=DbHelper.GetNextOracleKey("chartview","ChartViewNum"); //Cacheless method
				}
				return InsertNoCache(chartView,true);
			}
		}

		///<summary>Inserts one ChartView into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChartView chartView,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO chartview (";
			if(!useExistingPK && isRandomKeys) {
				chartView.ChartViewNum=ReplicationServers.GetKeyNoCache("chartview","ChartViewNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ChartViewNum,";
			}
			command+="Description,ItemOrder,ProcStatuses,ObjectTypes,ShowProcNotes,IsAudit,SelectedTeethOnly,OrionStatusFlags,DatesShowing,IsTpCharting) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(chartView.ChartViewNum)+",";
			}
			command+=
				 "'"+POut.String(chartView.Description)+"',"
				+    POut.Int   (chartView.ItemOrder)+","
				+    POut.Int   ((int)chartView.ProcStatuses)+","
				+    POut.Int   ((int)chartView.ObjectTypes)+","
				+    POut.Bool  (chartView.ShowProcNotes)+","
				+    POut.Bool  (chartView.IsAudit)+","
				+    POut.Bool  (chartView.SelectedTeethOnly)+","
				+    POut.Int   ((int)chartView.OrionStatusFlags)+","
				+    POut.Int   ((int)chartView.DatesShowing)+","
				+    POut.Bool  (chartView.IsTpCharting)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				chartView.ChartViewNum=Db.NonQ(command,true);
			}
			return chartView.ChartViewNum;
		}

		///<summary>Updates one ChartView in the database.</summary>
		public static void Update(ChartView chartView){
			string command="UPDATE chartview SET "
				+"Description      = '"+POut.String(chartView.Description)+"', "
				+"ItemOrder        =  "+POut.Int   (chartView.ItemOrder)+", "
				+"ProcStatuses     =  "+POut.Int   ((int)chartView.ProcStatuses)+", "
				+"ObjectTypes      =  "+POut.Int   ((int)chartView.ObjectTypes)+", "
				+"ShowProcNotes    =  "+POut.Bool  (chartView.ShowProcNotes)+", "
				+"IsAudit          =  "+POut.Bool  (chartView.IsAudit)+", "
				+"SelectedTeethOnly=  "+POut.Bool  (chartView.SelectedTeethOnly)+", "
				+"OrionStatusFlags =  "+POut.Int   ((int)chartView.OrionStatusFlags)+", "
				+"DatesShowing     =  "+POut.Int   ((int)chartView.DatesShowing)+", "
				+"IsTpCharting     =  "+POut.Bool  (chartView.IsTpCharting)+" "
				+"WHERE ChartViewNum = "+POut.Long(chartView.ChartViewNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ChartView in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ChartView chartView,ChartView oldChartView){
			string command="";
			if(chartView.Description != oldChartView.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(chartView.Description)+"'";
			}
			if(chartView.ItemOrder != oldChartView.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(chartView.ItemOrder)+"";
			}
			if(chartView.ProcStatuses != oldChartView.ProcStatuses) {
				if(command!=""){ command+=",";}
				command+="ProcStatuses = "+POut.Int   ((int)chartView.ProcStatuses)+"";
			}
			if(chartView.ObjectTypes != oldChartView.ObjectTypes) {
				if(command!=""){ command+=",";}
				command+="ObjectTypes = "+POut.Int   ((int)chartView.ObjectTypes)+"";
			}
			if(chartView.ShowProcNotes != oldChartView.ShowProcNotes) {
				if(command!=""){ command+=",";}
				command+="ShowProcNotes = "+POut.Bool(chartView.ShowProcNotes)+"";
			}
			if(chartView.IsAudit != oldChartView.IsAudit) {
				if(command!=""){ command+=",";}
				command+="IsAudit = "+POut.Bool(chartView.IsAudit)+"";
			}
			if(chartView.SelectedTeethOnly != oldChartView.SelectedTeethOnly) {
				if(command!=""){ command+=",";}
				command+="SelectedTeethOnly = "+POut.Bool(chartView.SelectedTeethOnly)+"";
			}
			if(chartView.OrionStatusFlags != oldChartView.OrionStatusFlags) {
				if(command!=""){ command+=",";}
				command+="OrionStatusFlags = "+POut.Int   ((int)chartView.OrionStatusFlags)+"";
			}
			if(chartView.DatesShowing != oldChartView.DatesShowing) {
				if(command!=""){ command+=",";}
				command+="DatesShowing = "+POut.Int   ((int)chartView.DatesShowing)+"";
			}
			if(chartView.IsTpCharting != oldChartView.IsTpCharting) {
				if(command!=""){ command+=",";}
				command+="IsTpCharting = "+POut.Bool(chartView.IsTpCharting)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE chartview SET "+command
				+" WHERE ChartViewNum = "+POut.Long(chartView.ChartViewNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ChartView from the database.</summary>
		public static void Delete(long chartViewNum){
			string command="DELETE FROM chartview "
				+"WHERE ChartViewNum = "+POut.Long(chartViewNum);
			Db.NonQ(command);
		}

	}
}