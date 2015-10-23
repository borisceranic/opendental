//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class SheetDefCrud {
		///<summary>Gets one SheetDef object from the database using the primary key.  Returns null if not found.</summary>
		public static SheetDef SelectOne(long sheetDefNum){
			string command="SELECT * FROM sheetdef "
				+"WHERE SheetDefNum = "+POut.Long(sheetDefNum);
			List<SheetDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SheetDef object from the database using a query.</summary>
		public static SheetDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SheetDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SheetDef objects from the database using a query.</summary>
		public static List<SheetDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SheetDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SheetDef> TableToList(DataTable table){
			List<SheetDef> retVal=new List<SheetDef>();
			SheetDef sheetDef;
			for(int i=0;i<table.Rows.Count;i++) {
				sheetDef=new SheetDef();
				sheetDef.SheetDefNum= PIn.Long  (table.Rows[i]["SheetDefNum"].ToString());
				sheetDef.Description= PIn.String(table.Rows[i]["Description"].ToString());
				sheetDef.SheetType  = (OpenDentBusiness.SheetTypeEnum)PIn.Int(table.Rows[i]["SheetType"].ToString());
				sheetDef.FontSize   = PIn.Float (table.Rows[i]["FontSize"].ToString());
				sheetDef.FontName   = PIn.String(table.Rows[i]["FontName"].ToString());
				sheetDef.Width      = PIn.Int   (table.Rows[i]["Width"].ToString());
				sheetDef.Height     = PIn.Int   (table.Rows[i]["Height"].ToString());
				sheetDef.IsLandscape= PIn.Bool  (table.Rows[i]["IsLandscape"].ToString());
				sheetDef.PageCount  = PIn.Int   (table.Rows[i]["PageCount"].ToString());
				sheetDef.IsMultiPage= PIn.Bool  (table.Rows[i]["IsMultiPage"].ToString());
				retVal.Add(sheetDef);
			}
			return retVal;
		}

		///<summary>Inserts one SheetDef into the database.  Returns the new priKey.</summary>
		public static long Insert(SheetDef sheetDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				sheetDef.SheetDefNum=DbHelper.GetNextOracleKey("sheetdef","SheetDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(sheetDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							sheetDef.SheetDefNum++;
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
				return Insert(sheetDef,false);
			}
		}

		///<summary>Inserts one SheetDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SheetDef sheetDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				sheetDef.SheetDefNum=ReplicationServers.GetKey("sheetdef","SheetDefNum");
			}
			string command="INSERT INTO sheetdef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SheetDefNum,";
			}
			command+="Description,SheetType,FontSize,FontName,Width,Height,IsLandscape,PageCount,IsMultiPage) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sheetDef.SheetDefNum)+",";
			}
			command+=
				 "'"+POut.String(sheetDef.Description)+"',"
				+    POut.Int   ((int)sheetDef.SheetType)+","
				+    POut.Float (sheetDef.FontSize)+","
				+"'"+POut.String(sheetDef.FontName)+"',"
				+    POut.Int   (sheetDef.Width)+","
				+    POut.Int   (sheetDef.Height)+","
				+    POut.Bool  (sheetDef.IsLandscape)+","
				+    POut.Int   (sheetDef.PageCount)+","
				+    POut.Bool  (sheetDef.IsMultiPage)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sheetDef.SheetDefNum=Db.NonQ(command,true);
			}
			return sheetDef.SheetDefNum;
		}

		///<summary>Inserts one SheetDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SheetDef sheetDef){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(sheetDef,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					sheetDef.SheetDefNum=DbHelper.GetNextOracleKey("sheetdef","SheetDefNum"); //Cacheless method
				}
				return InsertNoCache(sheetDef,true);
			}
		}

		///<summary>Inserts one SheetDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SheetDef sheetDef,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO sheetdef (";
			if(!useExistingPK && isRandomKeys) {
				sheetDef.SheetDefNum=ReplicationServers.GetKeyNoCache("sheetdef","SheetDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SheetDefNum,";
			}
			command+="Description,SheetType,FontSize,FontName,Width,Height,IsLandscape,PageCount,IsMultiPage) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sheetDef.SheetDefNum)+",";
			}
			command+=
				 "'"+POut.String(sheetDef.Description)+"',"
				+    POut.Int   ((int)sheetDef.SheetType)+","
				+    POut.Float (sheetDef.FontSize)+","
				+"'"+POut.String(sheetDef.FontName)+"',"
				+    POut.Int   (sheetDef.Width)+","
				+    POut.Int   (sheetDef.Height)+","
				+    POut.Bool  (sheetDef.IsLandscape)+","
				+    POut.Int   (sheetDef.PageCount)+","
				+    POut.Bool  (sheetDef.IsMultiPage)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				sheetDef.SheetDefNum=Db.NonQ(command,true);
			}
			return sheetDef.SheetDefNum;
		}

		///<summary>Updates one SheetDef in the database.</summary>
		public static void Update(SheetDef sheetDef){
			string command="UPDATE sheetdef SET "
				+"Description= '"+POut.String(sheetDef.Description)+"', "
				+"SheetType  =  "+POut.Int   ((int)sheetDef.SheetType)+", "
				+"FontSize   =  "+POut.Float (sheetDef.FontSize)+", "
				+"FontName   = '"+POut.String(sheetDef.FontName)+"', "
				+"Width      =  "+POut.Int   (sheetDef.Width)+", "
				+"Height     =  "+POut.Int   (sheetDef.Height)+", "
				+"IsLandscape=  "+POut.Bool  (sheetDef.IsLandscape)+", "
				+"PageCount  =  "+POut.Int   (sheetDef.PageCount)+", "
				+"IsMultiPage=  "+POut.Bool  (sheetDef.IsMultiPage)+" "
				+"WHERE SheetDefNum = "+POut.Long(sheetDef.SheetDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SheetDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SheetDef sheetDef,SheetDef oldSheetDef){
			string command="";
			if(sheetDef.Description != oldSheetDef.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(sheetDef.Description)+"'";
			}
			if(sheetDef.SheetType != oldSheetDef.SheetType) {
				if(command!=""){ command+=",";}
				command+="SheetType = "+POut.Int   ((int)sheetDef.SheetType)+"";
			}
			if(sheetDef.FontSize != oldSheetDef.FontSize) {
				if(command!=""){ command+=",";}
				command+="FontSize = "+POut.Float(sheetDef.FontSize)+"";
			}
			if(sheetDef.FontName != oldSheetDef.FontName) {
				if(command!=""){ command+=",";}
				command+="FontName = '"+POut.String(sheetDef.FontName)+"'";
			}
			if(sheetDef.Width != oldSheetDef.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(sheetDef.Width)+"";
			}
			if(sheetDef.Height != oldSheetDef.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(sheetDef.Height)+"";
			}
			if(sheetDef.IsLandscape != oldSheetDef.IsLandscape) {
				if(command!=""){ command+=",";}
				command+="IsLandscape = "+POut.Bool(sheetDef.IsLandscape)+"";
			}
			if(sheetDef.PageCount != oldSheetDef.PageCount) {
				if(command!=""){ command+=",";}
				command+="PageCount = "+POut.Int(sheetDef.PageCount)+"";
			}
			if(sheetDef.IsMultiPage != oldSheetDef.IsMultiPage) {
				if(command!=""){ command+=",";}
				command+="IsMultiPage = "+POut.Bool(sheetDef.IsMultiPage)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE sheetdef SET "+command
				+" WHERE SheetDefNum = "+POut.Long(sheetDef.SheetDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one SheetDef from the database.</summary>
		public static void Delete(long sheetDefNum){
			string command="DELETE FROM sheetdef "
				+"WHERE SheetDefNum = "+POut.Long(sheetDefNum);
			Db.NonQ(command);
		}

	}
}