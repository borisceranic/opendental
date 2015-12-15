//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ToothGridDefCrud {
		///<summary>Gets one ToothGridDef object from the database using the primary key.  Returns null if not found.</summary>
		public static ToothGridDef SelectOne(long toothGridDefNum){
			string command="SELECT * FROM toothgriddef "
				+"WHERE ToothGridDefNum = "+POut.Long(toothGridDefNum);
			List<ToothGridDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ToothGridDef object from the database using a query.</summary>
		public static ToothGridDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ToothGridDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ToothGridDef objects from the database using a query.</summary>
		public static List<ToothGridDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ToothGridDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ToothGridDef> TableToList(DataTable table){
			List<ToothGridDef> retVal=new List<ToothGridDef>();
			ToothGridDef toothGridDef;
			foreach(DataRow row in table.Rows) {
				toothGridDef=new ToothGridDef();
				toothGridDef.ToothGridDefNum = PIn.Long  (row["ToothGridDefNum"].ToString());
				toothGridDef.SheetFieldDefNum= PIn.Long  (row["SheetFieldDefNum"].ToString());
				toothGridDef.NameInternal    = PIn.String(row["NameInternal"].ToString());
				toothGridDef.NameShowing     = PIn.String(row["NameShowing"].ToString());
				toothGridDef.CellType        = (OpenDentBusiness.ToothGridCellType)PIn.Int(row["CellType"].ToString());
				toothGridDef.ItemOrder       = PIn.Int   (row["ItemOrder"].ToString());
				toothGridDef.ColumnWidth     = PIn.Int   (row["ColumnWidth"].ToString());
				toothGridDef.CodeNum         = PIn.Long  (row["CodeNum"].ToString());
				toothGridDef.ProcStatus      = (OpenDentBusiness.ProcStat)PIn.Int(row["ProcStatus"].ToString());
				retVal.Add(toothGridDef);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ToothGridDef> listToothGridDefs) {
			DataTable table=new DataTable("ToothGridDefs");
			table.Columns.Add("ToothGridDefNum");
			table.Columns.Add("SheetFieldDefNum");
			table.Columns.Add("NameInternal");
			table.Columns.Add("NameShowing");
			table.Columns.Add("CellType");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("ColumnWidth");
			table.Columns.Add("CodeNum");
			table.Columns.Add("ProcStatus");
			foreach(ToothGridDef toothGridDef in listToothGridDefs) {
				table.Rows.Add(new object[] {
					POut.Long  (toothGridDef.ToothGridDefNum),
					POut.Long  (toothGridDef.SheetFieldDefNum),
					POut.String(toothGridDef.NameInternal),
					POut.String(toothGridDef.NameShowing),
					POut.Int   ((int)toothGridDef.CellType),
					POut.Int   (toothGridDef.ItemOrder),
					POut.Int   (toothGridDef.ColumnWidth),
					POut.Long  (toothGridDef.CodeNum),
					POut.Int   ((int)toothGridDef.ProcStatus),
				});
			}
			return table;
		}

		///<summary>Inserts one ToothGridDef into the database.  Returns the new priKey.</summary>
		public static long Insert(ToothGridDef toothGridDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				toothGridDef.ToothGridDefNum=DbHelper.GetNextOracleKey("toothgriddef","ToothGridDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(toothGridDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							toothGridDef.ToothGridDefNum++;
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
				return Insert(toothGridDef,false);
			}
		}

		///<summary>Inserts one ToothGridDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ToothGridDef toothGridDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				toothGridDef.ToothGridDefNum=ReplicationServers.GetKey("toothgriddef","ToothGridDefNum");
			}
			string command="INSERT INTO toothgriddef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ToothGridDefNum,";
			}
			command+="SheetFieldDefNum,NameInternal,NameShowing,CellType,ItemOrder,ColumnWidth,CodeNum,ProcStatus) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(toothGridDef.ToothGridDefNum)+",";
			}
			command+=
				     POut.Long  (toothGridDef.SheetFieldDefNum)+","
				+"'"+POut.String(toothGridDef.NameInternal)+"',"
				+"'"+POut.String(toothGridDef.NameShowing)+"',"
				+    POut.Int   ((int)toothGridDef.CellType)+","
				+    POut.Int   (toothGridDef.ItemOrder)+","
				+    POut.Int   (toothGridDef.ColumnWidth)+","
				+    POut.Long  (toothGridDef.CodeNum)+","
				+    POut.Int   ((int)toothGridDef.ProcStatus)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				toothGridDef.ToothGridDefNum=Db.NonQ(command,true);
			}
			return toothGridDef.ToothGridDefNum;
		}

		///<summary>Inserts one ToothGridDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ToothGridDef toothGridDef){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(toothGridDef,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					toothGridDef.ToothGridDefNum=DbHelper.GetNextOracleKey("toothgriddef","ToothGridDefNum"); //Cacheless method
				}
				return InsertNoCache(toothGridDef,true);
			}
		}

		///<summary>Inserts one ToothGridDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ToothGridDef toothGridDef,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO toothgriddef (";
			if(!useExistingPK && isRandomKeys) {
				toothGridDef.ToothGridDefNum=ReplicationServers.GetKeyNoCache("toothgriddef","ToothGridDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ToothGridDefNum,";
			}
			command+="SheetFieldDefNum,NameInternal,NameShowing,CellType,ItemOrder,ColumnWidth,CodeNum,ProcStatus) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(toothGridDef.ToothGridDefNum)+",";
			}
			command+=
				     POut.Long  (toothGridDef.SheetFieldDefNum)+","
				+"'"+POut.String(toothGridDef.NameInternal)+"',"
				+"'"+POut.String(toothGridDef.NameShowing)+"',"
				+    POut.Int   ((int)toothGridDef.CellType)+","
				+    POut.Int   (toothGridDef.ItemOrder)+","
				+    POut.Int   (toothGridDef.ColumnWidth)+","
				+    POut.Long  (toothGridDef.CodeNum)+","
				+    POut.Int   ((int)toothGridDef.ProcStatus)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				toothGridDef.ToothGridDefNum=Db.NonQ(command,true);
			}
			return toothGridDef.ToothGridDefNum;
		}

		///<summary>Updates one ToothGridDef in the database.</summary>
		public static void Update(ToothGridDef toothGridDef){
			string command="UPDATE toothgriddef SET "
				+"SheetFieldDefNum=  "+POut.Long  (toothGridDef.SheetFieldDefNum)+", "
				+"NameInternal    = '"+POut.String(toothGridDef.NameInternal)+"', "
				+"NameShowing     = '"+POut.String(toothGridDef.NameShowing)+"', "
				+"CellType        =  "+POut.Int   ((int)toothGridDef.CellType)+", "
				+"ItemOrder       =  "+POut.Int   (toothGridDef.ItemOrder)+", "
				+"ColumnWidth     =  "+POut.Int   (toothGridDef.ColumnWidth)+", "
				+"CodeNum         =  "+POut.Long  (toothGridDef.CodeNum)+", "
				+"ProcStatus      =  "+POut.Int   ((int)toothGridDef.ProcStatus)+" "
				+"WHERE ToothGridDefNum = "+POut.Long(toothGridDef.ToothGridDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ToothGridDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ToothGridDef toothGridDef,ToothGridDef oldToothGridDef){
			string command="";
			if(toothGridDef.SheetFieldDefNum != oldToothGridDef.SheetFieldDefNum) {
				if(command!=""){ command+=",";}
				command+="SheetFieldDefNum = "+POut.Long(toothGridDef.SheetFieldDefNum)+"";
			}
			if(toothGridDef.NameInternal != oldToothGridDef.NameInternal) {
				if(command!=""){ command+=",";}
				command+="NameInternal = '"+POut.String(toothGridDef.NameInternal)+"'";
			}
			if(toothGridDef.NameShowing != oldToothGridDef.NameShowing) {
				if(command!=""){ command+=",";}
				command+="NameShowing = '"+POut.String(toothGridDef.NameShowing)+"'";
			}
			if(toothGridDef.CellType != oldToothGridDef.CellType) {
				if(command!=""){ command+=",";}
				command+="CellType = "+POut.Int   ((int)toothGridDef.CellType)+"";
			}
			if(toothGridDef.ItemOrder != oldToothGridDef.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(toothGridDef.ItemOrder)+"";
			}
			if(toothGridDef.ColumnWidth != oldToothGridDef.ColumnWidth) {
				if(command!=""){ command+=",";}
				command+="ColumnWidth = "+POut.Int(toothGridDef.ColumnWidth)+"";
			}
			if(toothGridDef.CodeNum != oldToothGridDef.CodeNum) {
				if(command!=""){ command+=",";}
				command+="CodeNum = "+POut.Long(toothGridDef.CodeNum)+"";
			}
			if(toothGridDef.ProcStatus != oldToothGridDef.ProcStatus) {
				if(command!=""){ command+=",";}
				command+="ProcStatus = "+POut.Int   ((int)toothGridDef.ProcStatus)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE toothgriddef SET "+command
				+" WHERE ToothGridDefNum = "+POut.Long(toothGridDef.ToothGridDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ToothGridDef from the database.</summary>
		public static void Delete(long toothGridDefNum){
			string command="DELETE FROM toothgriddef "
				+"WHERE ToothGridDefNum = "+POut.Long(toothGridDefNum);
			Db.NonQ(command);
		}

	}
}