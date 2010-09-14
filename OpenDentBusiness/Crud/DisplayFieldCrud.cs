//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class DisplayFieldCrud {
		/*///<summary>Gets one DisplayField object from the database using the primary key.  Returns null if not found.</summary>
		internal static DisplayField SelectOne(long displayFieldNum){
			string command="SELECT * FROM displayfield "
				+"WHERE DisplayFieldNum = "+POut.Long(displayFieldNum)+" LIMIT 1";
			List<DisplayField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one DisplayField object from the database using a query.</summary>
		internal static DisplayField SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DisplayField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of DisplayField objects from the database using a query.</summary>
		internal static List<DisplayField> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DisplayField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<DisplayField> TableToList(DataTable table){
			List<DisplayField> retVal=new List<DisplayField>();
			DisplayField displayField;
			for(int i=0;i<table.Rows.Count;i++) {
				displayField=new DisplayField();
				displayField.DisplayFieldNum= PIn.Long  (table.Rows[i]["DisplayFieldNum"].ToString());
				displayField.InternalName   = PIn.String(table.Rows[i]["InternalName"].ToString());
				displayField.ItemOrder      = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				displayField.Description    = PIn.String(table.Rows[i]["Description"].ToString());
				displayField.ColumnWidth    = PIn.Int   (table.Rows[i]["ColumnWidth"].ToString());
				displayField.Category       = (DisplayFieldCategory)PIn.Int(table.Rows[i]["Category"].ToString());
				displayField.ChartViewNum   = PIn.Long  (table.Rows[i]["ChartViewNum"].ToString());
				retVal.Add(displayField);
			}
			return retVal;
		}*/

		///<summary>Inserts one DisplayField into the database.  Returns the new priKey.</summary>
		internal static long Insert(DisplayField displayField){
			return Insert(displayField,false);
		}

		///<summary>Inserts one DisplayField into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(DisplayField displayField,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				displayField.DisplayFieldNum=ReplicationServers.GetKey("displayfield","DisplayFieldNum");
			}
			string command="INSERT INTO displayfield (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="DisplayFieldNum,";
			}
			command+="InternalName,ItemOrder,Description,ColumnWidth,Category,ChartViewNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(displayField.DisplayFieldNum)+",";
			}
			command+=
				 "'"+POut.String(displayField.InternalName)+"',"
				+    POut.Int   (displayField.ItemOrder)+","
				+"'"+POut.String(displayField.Description)+"',"
				+    POut.Int   (displayField.ColumnWidth)+","
				+    POut.Int   ((int)displayField.Category)+","
				+    POut.Long  (displayField.ChartViewNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				displayField.DisplayFieldNum=Db.NonQ(command,true);
			}
			return displayField.DisplayFieldNum;
		}

		/*///<summary>Updates one DisplayField in the database.</summary>
		internal static void Update(DisplayField displayField){
			string command="UPDATE displayfield SET "
				+"InternalName   = '"+POut.String(displayField.InternalName)+"', "
				+"ItemOrder      =  "+POut.Int   (displayField.ItemOrder)+", "
				+"Description    = '"+POut.String(displayField.Description)+"', "
				+"ColumnWidth    =  "+POut.Int   (displayField.ColumnWidth)+", "
				+"Category       =  "+POut.Int   ((int)displayField.Category)+", "
				+"ChartViewNum   =  "+POut.Long  (displayField.ChartViewNum)+" "
				+"WHERE DisplayFieldNum = "+POut.Long(displayField.DisplayFieldNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one DisplayField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(DisplayField displayField,DisplayField oldDisplayField){
			string command="";
			if(displayField.InternalName != oldDisplayField.InternalName) {
				if(command!=""){ command+=",";}
				command+="InternalName = '"+POut.String(displayField.InternalName)+"'";
			}
			if(displayField.ItemOrder != oldDisplayField.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(displayField.ItemOrder)+"";
			}
			if(displayField.Description != oldDisplayField.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(displayField.Description)+"'";
			}
			if(displayField.ColumnWidth != oldDisplayField.ColumnWidth) {
				if(command!=""){ command+=",";}
				command+="ColumnWidth = "+POut.Int(displayField.ColumnWidth)+"";
			}
			if(displayField.Category != oldDisplayField.Category) {
				if(command!=""){ command+=",";}
				command+="Category = "+POut.Int   ((int)displayField.Category)+"";
			}
			if(displayField.ChartViewNum != oldDisplayField.ChartViewNum) {
				if(command!=""){ command+=",";}
				command+="ChartViewNum = "+POut.Long(displayField.ChartViewNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE displayfield SET "+command
				+" WHERE DisplayFieldNum = "+POut.Long(displayField.DisplayFieldNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one DisplayField from the database.</summary>
		internal static void Delete(long displayFieldNum){
			string command="DELETE FROM displayfield "
				+"WHERE DisplayFieldNum = "+POut.Long(displayFieldNum)+" LIMIT 1";
			Db.NonQ(command);
		}*/

	}
}