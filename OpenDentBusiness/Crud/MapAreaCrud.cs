//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MapAreaCrud {
		///<summary>Gets one MapArea object from the database using the primary key.  Returns null if not found.</summary>
		public static MapArea SelectOne(long mapAreaNum){
			string command="SELECT * FROM maparea "
				+"WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			List<MapArea> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MapArea object from the database using a query.</summary>
		public static MapArea SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MapArea> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MapArea objects from the database using a query.</summary>
		public static List<MapArea> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MapArea> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MapArea> TableToList(DataTable table){
			List<MapArea> retVal=new List<MapArea>();
			MapArea mapArea;
			foreach(DataRow row in table.Rows) {
				mapArea=new MapArea();
				mapArea.MapAreaNum = PIn.Long  (row["MapAreaNum"].ToString());
				mapArea.Extension  = PIn.Int   (row["Extension"].ToString());
				mapArea.XPos       = PIn.Double(row["XPos"].ToString());
				mapArea.YPos       = PIn.Double(row["YPos"].ToString());
				mapArea.Width      = PIn.Double(row["Width"].ToString());
				mapArea.Height     = PIn.Double(row["Height"].ToString());
				mapArea.Description= PIn.String(row["Description"].ToString());
				mapArea.ItemType   = (OpenDentBusiness.MapItemType)PIn.Int(row["ItemType"].ToString());
				retVal.Add(mapArea);
			}
			return retVal;
		}

		///<summary>Converts a list of MapArea into a DataTable.</summary>
		public static DataTable ListToTable(List<MapArea> listMapAreas,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="MapArea";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("MapAreaNum");
			table.Columns.Add("Extension");
			table.Columns.Add("XPos");
			table.Columns.Add("YPos");
			table.Columns.Add("Width");
			table.Columns.Add("Height");
			table.Columns.Add("Description");
			table.Columns.Add("ItemType");
			foreach(MapArea mapArea in listMapAreas) {
				table.Rows.Add(new object[] {
					POut.Long  (mapArea.MapAreaNum),
					POut.Int   (mapArea.Extension),
					POut.Double(mapArea.XPos),
					POut.Double(mapArea.YPos),
					POut.Double(mapArea.Width),
					POut.Double(mapArea.Height),
					POut.String(mapArea.Description),
					POut.Int   ((int)mapArea.ItemType),
				});
			}
			return table;
		}

		///<summary>Inserts one MapArea into the database.  Returns the new priKey.</summary>
		public static long Insert(MapArea mapArea){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				mapArea.MapAreaNum=DbHelper.GetNextOracleKey("maparea","MapAreaNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(mapArea,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							mapArea.MapAreaNum++;
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
				return Insert(mapArea,false);
			}
		}

		///<summary>Inserts one MapArea into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MapArea mapArea,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				mapArea.MapAreaNum=ReplicationServers.GetKey("maparea","MapAreaNum");
			}
			string command="INSERT INTO maparea (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MapAreaNum,";
			}
			command+="Extension,XPos,YPos,Width,Height,Description,ItemType) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mapArea.MapAreaNum)+",";
			}
			command+=
				     POut.Int   (mapArea.Extension)+","
				+"'"+POut.Double(mapArea.XPos)+"',"
				+"'"+POut.Double(mapArea.YPos)+"',"
				+"'"+POut.Double(mapArea.Width)+"',"
				+"'"+POut.Double(mapArea.Height)+"',"
				+"'"+POut.String(mapArea.Description)+"',"
				+    POut.Int   ((int)mapArea.ItemType)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mapArea.MapAreaNum=Db.NonQ(command,true);
			}
			return mapArea.MapAreaNum;
		}

		///<summary>Inserts one MapArea into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MapArea mapArea){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(mapArea,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					mapArea.MapAreaNum=DbHelper.GetNextOracleKey("maparea","MapAreaNum"); //Cacheless method
				}
				return InsertNoCache(mapArea,true);
			}
		}

		///<summary>Inserts one MapArea into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MapArea mapArea,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO maparea (";
			if(!useExistingPK && isRandomKeys) {
				mapArea.MapAreaNum=ReplicationServers.GetKeyNoCache("maparea","MapAreaNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MapAreaNum,";
			}
			command+="Extension,XPos,YPos,Width,Height,Description,ItemType) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(mapArea.MapAreaNum)+",";
			}
			command+=
				     POut.Int   (mapArea.Extension)+","
				+"'"+POut.Double(mapArea.XPos)+"',"
				+"'"+POut.Double(mapArea.YPos)+"',"
				+"'"+POut.Double(mapArea.Width)+"',"
				+"'"+POut.Double(mapArea.Height)+"',"
				+"'"+POut.String(mapArea.Description)+"',"
				+    POut.Int   ((int)mapArea.ItemType)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				mapArea.MapAreaNum=Db.NonQ(command,true);
			}
			return mapArea.MapAreaNum;
		}

		///<summary>Updates one MapArea in the database.</summary>
		public static void Update(MapArea mapArea){
			string command="UPDATE maparea SET "
				+"Extension  =  "+POut.Int   (mapArea.Extension)+", "
				+"XPos       = '"+POut.Double(mapArea.XPos)+"', "
				+"YPos       = '"+POut.Double(mapArea.YPos)+"', "
				+"Width      = '"+POut.Double(mapArea.Width)+"', "
				+"Height     = '"+POut.Double(mapArea.Height)+"', "
				+"Description= '"+POut.String(mapArea.Description)+"', "
				+"ItemType   =  "+POut.Int   ((int)mapArea.ItemType)+" "
				+"WHERE MapAreaNum = "+POut.Long(mapArea.MapAreaNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MapArea in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MapArea mapArea,MapArea oldMapArea){
			string command="";
			if(mapArea.Extension != oldMapArea.Extension) {
				if(command!=""){ command+=",";}
				command+="Extension = "+POut.Int(mapArea.Extension)+"";
			}
			if(mapArea.XPos != oldMapArea.XPos) {
				if(command!=""){ command+=",";}
				command+="XPos = '"+POut.Double(mapArea.XPos)+"'";
			}
			if(mapArea.YPos != oldMapArea.YPos) {
				if(command!=""){ command+=",";}
				command+="YPos = '"+POut.Double(mapArea.YPos)+"'";
			}
			if(mapArea.Width != oldMapArea.Width) {
				if(command!=""){ command+=",";}
				command+="Width = '"+POut.Double(mapArea.Width)+"'";
			}
			if(mapArea.Height != oldMapArea.Height) {
				if(command!=""){ command+=",";}
				command+="Height = '"+POut.Double(mapArea.Height)+"'";
			}
			if(mapArea.Description != oldMapArea.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(mapArea.Description)+"'";
			}
			if(mapArea.ItemType != oldMapArea.ItemType) {
				if(command!=""){ command+=",";}
				command+="ItemType = "+POut.Int   ((int)mapArea.ItemType)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE maparea SET "+command
				+" WHERE MapAreaNum = "+POut.Long(mapArea.MapAreaNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one MapArea from the database.</summary>
		public static void Delete(long mapAreaNum){
			string command="DELETE FROM maparea "
				+"WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			Db.NonQ(command);
		}

	}
}