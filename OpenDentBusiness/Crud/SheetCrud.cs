//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class SheetCrud {
		///<summary>Gets one Sheet object from the database using the primary key.  Returns null if not found.</summary>
		internal static Sheet SelectOne(long sheetNum){
			string command="SELECT * FROM sheet "
				+"WHERE SheetNum = "+POut.Long(sheetNum)+" LIMIT 1";
			List<Sheet> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Sheet object from the database using a query.</summary>
		internal static Sheet SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Sheet> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Sheet objects from the database using a query.</summary>
		internal static List<Sheet> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Sheet> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Sheet> TableToList(DataTable table){
			List<Sheet> retVal=new List<Sheet>();
			Sheet sheet;
			for(int i=0;i<table.Rows.Count;i++) {
				sheet=new Sheet();
				sheet.SheetNum      = PIn.Long  (table.Rows[i]["SheetNum"].ToString());
				sheet.SheetType     = (SheetTypeEnum)PIn.Int(table.Rows[i]["SheetType"].ToString());
				sheet.PatNum        = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				sheet.DateTimeSheet = PIn.DateT (table.Rows[i]["DateTimeSheet"].ToString());
				sheet.FontSize      = PIn.Float (table.Rows[i]["FontSize"].ToString());
				sheet.FontName      = PIn.String(table.Rows[i]["FontName"].ToString());
				sheet.Width         = PIn.Int   (table.Rows[i]["Width"].ToString());
				sheet.Height        = PIn.Int   (table.Rows[i]["Height"].ToString());
				sheet.IsLandscape   = PIn.Bool  (table.Rows[i]["IsLandscape"].ToString());
				sheet.InternalNote  = PIn.String(table.Rows[i]["InternalNote"].ToString());
				sheet.Description   = PIn.String(table.Rows[i]["Description"].ToString());
				sheet.ShowInTerminal= PIn.Byte  (table.Rows[i]["ShowInTerminal"].ToString());
				sheet.IsWebForm     = PIn.Bool  (table.Rows[i]["IsWebForm"].ToString());
				retVal.Add(sheet);
			}
			return retVal;
		}

		///<summary>Inserts one Sheet into the database.  Returns the new priKey.</summary>
		internal static long Insert(Sheet sheet){
			return Insert(sheet,false);
		}

		///<summary>Inserts one Sheet into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Sheet sheet,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				sheet.SheetNum=ReplicationServers.GetKey("sheet","SheetNum");
			}
			string command="INSERT INTO sheet (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SheetNum,";
			}
			command+="SheetType,PatNum,DateTimeSheet,FontSize,FontName,Width,Height,IsLandscape,InternalNote,Description,ShowInTerminal,IsWebForm) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sheet.SheetNum)+",";
			}
			command+=
				     POut.Int   ((int)sheet.SheetType)+","
				+    POut.Long  (sheet.PatNum)+","
				+    POut.DateT (sheet.DateTimeSheet)+","
				+    POut.Float (sheet.FontSize)+","
				+"'"+POut.String(sheet.FontName)+"',"
				+    POut.Int   (sheet.Width)+","
				+    POut.Int   (sheet.Height)+","
				+    POut.Bool  (sheet.IsLandscape)+","
				+"'"+POut.String(sheet.InternalNote)+"',"
				+"'"+POut.String(sheet.Description)+"',"
				+    POut.Byte  (sheet.ShowInTerminal)+","
				+    POut.Bool  (sheet.IsWebForm)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sheet.SheetNum=Db.NonQ(command,true);
			}
			return sheet.SheetNum;
		}

		///<summary>Updates one Sheet in the database.</summary>
		internal static void Update(Sheet sheet){
			string command="UPDATE sheet SET "
				+"SheetType     =  "+POut.Int   ((int)sheet.SheetType)+", "
				+"PatNum        =  "+POut.Long  (sheet.PatNum)+", "
				+"DateTimeSheet =  "+POut.DateT (sheet.DateTimeSheet)+", "
				+"FontSize      =  "+POut.Float (sheet.FontSize)+", "
				+"FontName      = '"+POut.String(sheet.FontName)+"', "
				+"Width         =  "+POut.Int   (sheet.Width)+", "
				+"Height        =  "+POut.Int   (sheet.Height)+", "
				+"IsLandscape   =  "+POut.Bool  (sheet.IsLandscape)+", "
				+"InternalNote  = '"+POut.String(sheet.InternalNote)+"', "
				+"Description   = '"+POut.String(sheet.Description)+"', "
				+"ShowInTerminal=  "+POut.Byte  (sheet.ShowInTerminal)+", "
				+"IsWebForm     =  "+POut.Bool  (sheet.IsWebForm)+" "
				+"WHERE SheetNum = "+POut.Long(sheet.SheetNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one Sheet in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Sheet sheet,Sheet oldSheet){
			string command="";
			if(sheet.SheetType != oldSheet.SheetType) {
				if(command!=""){ command+=",";}
				command+="SheetType = "+POut.Int   ((int)sheet.SheetType)+"";
			}
			if(sheet.PatNum != oldSheet.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(sheet.PatNum)+"";
			}
			if(sheet.DateTimeSheet != oldSheet.DateTimeSheet) {
				if(command!=""){ command+=",";}
				command+="DateTimeSheet = "+POut.DateT(sheet.DateTimeSheet)+"";
			}
			if(sheet.FontSize != oldSheet.FontSize) {
				if(command!=""){ command+=",";}
				command+="FontSize = "+POut.Float(sheet.FontSize)+"";
			}
			if(sheet.FontName != oldSheet.FontName) {
				if(command!=""){ command+=",";}
				command+="FontName = '"+POut.String(sheet.FontName)+"'";
			}
			if(sheet.Width != oldSheet.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(sheet.Width)+"";
			}
			if(sheet.Height != oldSheet.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(sheet.Height)+"";
			}
			if(sheet.IsLandscape != oldSheet.IsLandscape) {
				if(command!=""){ command+=",";}
				command+="IsLandscape = "+POut.Bool(sheet.IsLandscape)+"";
			}
			if(sheet.InternalNote != oldSheet.InternalNote) {
				if(command!=""){ command+=",";}
				command+="InternalNote = '"+POut.String(sheet.InternalNote)+"'";
			}
			if(sheet.Description != oldSheet.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(sheet.Description)+"'";
			}
			if(sheet.ShowInTerminal != oldSheet.ShowInTerminal) {
				if(command!=""){ command+=",";}
				command+="ShowInTerminal = "+POut.Byte(sheet.ShowInTerminal)+"";
			}
			if(sheet.IsWebForm != oldSheet.IsWebForm) {
				if(command!=""){ command+=",";}
				command+="IsWebForm = "+POut.Bool(sheet.IsWebForm)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE sheet SET "+command
				+" WHERE SheetNum = "+POut.Long(sheet.SheetNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one Sheet from the database.</summary>
		internal static void Delete(long sheetNum){
			string command="DELETE FROM sheet "
				+"WHERE SheetNum = "+POut.Long(sheetNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}