//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ToothGridCellCrud {
		///<summary>Gets one ToothGridCell object from the database using the primary key.  Returns null if not found.</summary>
		public static ToothGridCell SelectOne(long toothGridCellNum){
			string command="SELECT * FROM toothgridcell "
				+"WHERE ToothGridCellNum = "+POut.Long(toothGridCellNum);
			List<ToothGridCell> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ToothGridCell object from the database using a query.</summary>
		public static ToothGridCell SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ToothGridCell> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ToothGridCell objects from the database using a query.</summary>
		public static List<ToothGridCell> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ToothGridCell> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ToothGridCell> TableToList(DataTable table){
			List<ToothGridCell> retVal=new List<ToothGridCell>();
			ToothGridCell toothGridCell;
			foreach(DataRow row in table.Rows) {
				toothGridCell=new ToothGridCell();
				toothGridCell.ToothGridCellNum= PIn.Long  (row["ToothGridCellNum"].ToString());
				toothGridCell.SheetFieldNum   = PIn.Long  (row["SheetFieldNum"].ToString());
				toothGridCell.ToothGridColNum = PIn.Long  (row["ToothGridColNum"].ToString());
				toothGridCell.ValueEntered    = PIn.String(row["ValueEntered"].ToString());
				toothGridCell.ToothNum        = PIn.String(row["ToothNum"].ToString());
				retVal.Add(toothGridCell);
			}
			return retVal;
		}

		///<summary>Inserts one ToothGridCell into the database.  Returns the new priKey.</summary>
		public static long Insert(ToothGridCell toothGridCell){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				toothGridCell.ToothGridCellNum=DbHelper.GetNextOracleKey("toothgridcell","ToothGridCellNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(toothGridCell,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							toothGridCell.ToothGridCellNum++;
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
				return Insert(toothGridCell,false);
			}
		}

		///<summary>Inserts one ToothGridCell into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ToothGridCell toothGridCell,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				toothGridCell.ToothGridCellNum=ReplicationServers.GetKey("toothgridcell","ToothGridCellNum");
			}
			string command="INSERT INTO toothgridcell (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ToothGridCellNum,";
			}
			command+="SheetFieldNum,ToothGridColNum,ValueEntered,ToothNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(toothGridCell.ToothGridCellNum)+",";
			}
			command+=
				     POut.Long  (toothGridCell.SheetFieldNum)+","
				+    POut.Long  (toothGridCell.ToothGridColNum)+","
				+"'"+POut.String(toothGridCell.ValueEntered)+"',"
				+"'"+POut.String(toothGridCell.ToothNum)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				toothGridCell.ToothGridCellNum=Db.NonQ(command,true);
			}
			return toothGridCell.ToothGridCellNum;
		}

		///<summary>Inserts one ToothGridCell into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ToothGridCell toothGridCell){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(toothGridCell,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					toothGridCell.ToothGridCellNum=DbHelper.GetNextOracleKey("toothgridcell","ToothGridCellNum"); //Cacheless method
				}
				return InsertNoCache(toothGridCell,true);
			}
		}

		///<summary>Inserts one ToothGridCell into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ToothGridCell toothGridCell,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO toothgridcell (";
			if(!useExistingPK && isRandomKeys) {
				toothGridCell.ToothGridCellNum=ReplicationServers.GetKeyNoCache("toothgridcell","ToothGridCellNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ToothGridCellNum,";
			}
			command+="SheetFieldNum,ToothGridColNum,ValueEntered,ToothNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(toothGridCell.ToothGridCellNum)+",";
			}
			command+=
				     POut.Long  (toothGridCell.SheetFieldNum)+","
				+    POut.Long  (toothGridCell.ToothGridColNum)+","
				+"'"+POut.String(toothGridCell.ValueEntered)+"',"
				+"'"+POut.String(toothGridCell.ToothNum)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				toothGridCell.ToothGridCellNum=Db.NonQ(command,true);
			}
			return toothGridCell.ToothGridCellNum;
		}

		///<summary>Updates one ToothGridCell in the database.</summary>
		public static void Update(ToothGridCell toothGridCell){
			string command="UPDATE toothgridcell SET "
				+"SheetFieldNum   =  "+POut.Long  (toothGridCell.SheetFieldNum)+", "
				+"ToothGridColNum =  "+POut.Long  (toothGridCell.ToothGridColNum)+", "
				+"ValueEntered    = '"+POut.String(toothGridCell.ValueEntered)+"', "
				+"ToothNum        = '"+POut.String(toothGridCell.ToothNum)+"' "
				+"WHERE ToothGridCellNum = "+POut.Long(toothGridCell.ToothGridCellNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ToothGridCell in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ToothGridCell toothGridCell,ToothGridCell oldToothGridCell){
			string command="";
			if(toothGridCell.SheetFieldNum != oldToothGridCell.SheetFieldNum) {
				if(command!=""){ command+=",";}
				command+="SheetFieldNum = "+POut.Long(toothGridCell.SheetFieldNum)+"";
			}
			if(toothGridCell.ToothGridColNum != oldToothGridCell.ToothGridColNum) {
				if(command!=""){ command+=",";}
				command+="ToothGridColNum = "+POut.Long(toothGridCell.ToothGridColNum)+"";
			}
			if(toothGridCell.ValueEntered != oldToothGridCell.ValueEntered) {
				if(command!=""){ command+=",";}
				command+="ValueEntered = '"+POut.String(toothGridCell.ValueEntered)+"'";
			}
			if(toothGridCell.ToothNum != oldToothGridCell.ToothNum) {
				if(command!=""){ command+=",";}
				command+="ToothNum = '"+POut.String(toothGridCell.ToothNum)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE toothgridcell SET "+command
				+" WHERE ToothGridCellNum = "+POut.Long(toothGridCell.ToothGridCellNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ToothGridCell,ToothGridCell) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ToothGridCell toothGridCell,ToothGridCell oldToothGridCell) {
			if(toothGridCell.SheetFieldNum != oldToothGridCell.SheetFieldNum) {
				return true;
			}
			if(toothGridCell.ToothGridColNum != oldToothGridCell.ToothGridColNum) {
				return true;
			}
			if(toothGridCell.ValueEntered != oldToothGridCell.ValueEntered) {
				return true;
			}
			if(toothGridCell.ToothNum != oldToothGridCell.ToothNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ToothGridCell from the database.</summary>
		public static void Delete(long toothGridCellNum){
			string command="DELETE FROM toothgridcell "
				+"WHERE ToothGridCellNum = "+POut.Long(toothGridCellNum);
			Db.NonQ(command);
		}

	}
}