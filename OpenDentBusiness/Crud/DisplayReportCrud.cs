//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class DisplayReportCrud {
		///<summary>Gets one DisplayReport object from the database using the primary key.  Returns null if not found.</summary>
		public static DisplayReport SelectOne(long displayReportNum){
			string command="SELECT * FROM displayreport "
				+"WHERE DisplayReportNum = "+POut.Long(displayReportNum);
			List<DisplayReport> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one DisplayReport object from the database using a query.</summary>
		public static DisplayReport SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DisplayReport> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of DisplayReport objects from the database using a query.</summary>
		public static List<DisplayReport> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DisplayReport> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<DisplayReport> TableToList(DataTable table){
			List<DisplayReport> retVal=new List<DisplayReport>();
			DisplayReport displayReport;
			foreach(DataRow row in table.Rows) {
				displayReport=new DisplayReport();
				displayReport.DisplayReportNum= PIn.Long  (row["DisplayReportNum"].ToString());
				displayReport.InternalName    = PIn.String(row["InternalName"].ToString());
				displayReport.ItemOrder       = PIn.Int   (row["ItemOrder"].ToString());
				displayReport.Description     = PIn.String(row["Description"].ToString());
				displayReport.Category        = (OpenDentBusiness.DisplayReportCategory)PIn.Int(row["Category"].ToString());
				displayReport.IsHidden        = PIn.Bool  (row["IsHidden"].ToString());
				retVal.Add(displayReport);
			}
			return retVal;
		}

		///<summary>Converts a list of DisplayReport into a DataTable.</summary>
		public static DataTable ListToTable(List<DisplayReport> listDisplayReports,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="DisplayReport";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("DisplayReportNum");
			table.Columns.Add("InternalName");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("Description");
			table.Columns.Add("Category");
			table.Columns.Add("IsHidden");
			foreach(DisplayReport displayReport in listDisplayReports) {
				table.Rows.Add(new object[] {
					POut.Long  (displayReport.DisplayReportNum),
					            displayReport.InternalName,
					POut.Int   (displayReport.ItemOrder),
					            displayReport.Description,
					POut.Int   ((int)displayReport.Category),
					POut.Bool  (displayReport.IsHidden),
				});
			}
			return table;
		}

		///<summary>Inserts one DisplayReport into the database.  Returns the new priKey.</summary>
		public static long Insert(DisplayReport displayReport){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				displayReport.DisplayReportNum=DbHelper.GetNextOracleKey("displayreport","DisplayReportNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(displayReport,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							displayReport.DisplayReportNum++;
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
				return Insert(displayReport,false);
			}
		}

		///<summary>Inserts one DisplayReport into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(DisplayReport displayReport,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				displayReport.DisplayReportNum=ReplicationServers.GetKey("displayreport","DisplayReportNum");
			}
			string command="INSERT INTO displayreport (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="DisplayReportNum,";
			}
			command+="InternalName,ItemOrder,Description,Category,IsHidden) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(displayReport.DisplayReportNum)+",";
			}
			command+=
				 "'"+POut.String(displayReport.InternalName)+"',"
				+    POut.Int   (displayReport.ItemOrder)+","
				+"'"+POut.String(displayReport.Description)+"',"
				+    POut.Int   ((int)displayReport.Category)+","
				+    POut.Bool  (displayReport.IsHidden)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				displayReport.DisplayReportNum=Db.NonQ(command,true);
			}
			return displayReport.DisplayReportNum;
		}

		///<summary>Inserts one DisplayReport into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DisplayReport displayReport){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(displayReport,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					displayReport.DisplayReportNum=DbHelper.GetNextOracleKey("displayreport","DisplayReportNum"); //Cacheless method
				}
				return InsertNoCache(displayReport,true);
			}
		}

		///<summary>Inserts one DisplayReport into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DisplayReport displayReport,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO displayreport (";
			if(!useExistingPK && isRandomKeys) {
				displayReport.DisplayReportNum=ReplicationServers.GetKeyNoCache("displayreport","DisplayReportNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="DisplayReportNum,";
			}
			command+="InternalName,ItemOrder,Description,Category,IsHidden) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(displayReport.DisplayReportNum)+",";
			}
			command+=
				 "'"+POut.String(displayReport.InternalName)+"',"
				+    POut.Int   (displayReport.ItemOrder)+","
				+"'"+POut.String(displayReport.Description)+"',"
				+    POut.Int   ((int)displayReport.Category)+","
				+    POut.Bool  (displayReport.IsHidden)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				displayReport.DisplayReportNum=Db.NonQ(command,true);
			}
			return displayReport.DisplayReportNum;
		}

		///<summary>Updates one DisplayReport in the database.</summary>
		public static void Update(DisplayReport displayReport){
			string command="UPDATE displayreport SET "
				+"InternalName    = '"+POut.String(displayReport.InternalName)+"', "
				+"ItemOrder       =  "+POut.Int   (displayReport.ItemOrder)+", "
				+"Description     = '"+POut.String(displayReport.Description)+"', "
				+"Category        =  "+POut.Int   ((int)displayReport.Category)+", "
				+"IsHidden        =  "+POut.Bool  (displayReport.IsHidden)+" "
				+"WHERE DisplayReportNum = "+POut.Long(displayReport.DisplayReportNum);
			Db.NonQ(command);
		}

		///<summary>Updates one DisplayReport in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(DisplayReport displayReport,DisplayReport oldDisplayReport){
			string command="";
			if(displayReport.InternalName != oldDisplayReport.InternalName) {
				if(command!=""){ command+=",";}
				command+="InternalName = '"+POut.String(displayReport.InternalName)+"'";
			}
			if(displayReport.ItemOrder != oldDisplayReport.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(displayReport.ItemOrder)+"";
			}
			if(displayReport.Description != oldDisplayReport.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(displayReport.Description)+"'";
			}
			if(displayReport.Category != oldDisplayReport.Category) {
				if(command!=""){ command+=",";}
				command+="Category = "+POut.Int   ((int)displayReport.Category)+"";
			}
			if(displayReport.IsHidden != oldDisplayReport.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(displayReport.IsHidden)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE displayreport SET "+command
				+" WHERE DisplayReportNum = "+POut.Long(displayReport.DisplayReportNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(DisplayReport,DisplayReport) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(DisplayReport displayReport,DisplayReport oldDisplayReport) {
			if(displayReport.InternalName != oldDisplayReport.InternalName) {
				return true;
			}
			if(displayReport.ItemOrder != oldDisplayReport.ItemOrder) {
				return true;
			}
			if(displayReport.Description != oldDisplayReport.Description) {
				return true;
			}
			if(displayReport.Category != oldDisplayReport.Category) {
				return true;
			}
			if(displayReport.IsHidden != oldDisplayReport.IsHidden) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one DisplayReport from the database.</summary>
		public static void Delete(long displayReportNum){
			string command="DELETE FROM displayreport "
				+"WHERE DisplayReportNum = "+POut.Long(displayReportNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<DisplayReport> listNew,List<DisplayReport> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<DisplayReport> listIns    =new List<DisplayReport>();
			List<DisplayReport> listUpdNew =new List<DisplayReport>();
			List<DisplayReport> listUpdDB  =new List<DisplayReport>();
			List<DisplayReport> listDel    =new List<DisplayReport>();
			listNew.Sort((DisplayReport x,DisplayReport y) => { return x.DisplayReportNum.CompareTo(y.DisplayReportNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((DisplayReport x,DisplayReport y) => { return x.DisplayReportNum.CompareTo(y.DisplayReportNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			DisplayReport fieldNew;
			DisplayReport fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.DisplayReportNum<fieldDB.DisplayReportNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.DisplayReportNum>fieldDB.DisplayReportNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])){
					rowsUpdatedCount++;
				}
			}
			for(int i=0;i<listDel.Count;i++) {
				Delete(listDel[i].DisplayReportNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}