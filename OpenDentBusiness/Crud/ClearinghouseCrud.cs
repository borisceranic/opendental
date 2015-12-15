//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ClearinghouseCrud {
		///<summary>Gets one Clearinghouse object from the database using the primary key.  Returns null if not found.</summary>
		public static Clearinghouse SelectOne(long clearinghouseNum){
			string command="SELECT * FROM clearinghouse "
				+"WHERE ClearinghouseNum = "+POut.Long(clearinghouseNum);
			List<Clearinghouse> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Clearinghouse object from the database using a query.</summary>
		public static Clearinghouse SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Clearinghouse> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Clearinghouse objects from the database using a query.</summary>
		public static List<Clearinghouse> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Clearinghouse> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Clearinghouse> TableToList(DataTable table){
			List<Clearinghouse> retVal=new List<Clearinghouse>();
			Clearinghouse clearinghouse;
			foreach(DataRow row in table.Rows) {
				clearinghouse=new Clearinghouse();
				clearinghouse.ClearinghouseNum  = PIn.Long  (row["ClearinghouseNum"].ToString());
				clearinghouse.Description       = PIn.String(row["Description"].ToString());
				clearinghouse.ExportPath        = PIn.String(row["ExportPath"].ToString());
				clearinghouse.Payors            = PIn.String(row["Payors"].ToString());
				clearinghouse.Eformat           = (OpenDentBusiness.ElectronicClaimFormat)PIn.Int(row["Eformat"].ToString());
				clearinghouse.ISA05             = PIn.String(row["ISA05"].ToString());
				clearinghouse.SenderTIN         = PIn.String(row["SenderTIN"].ToString());
				clearinghouse.ISA07             = PIn.String(row["ISA07"].ToString());
				clearinghouse.ISA08             = PIn.String(row["ISA08"].ToString());
				clearinghouse.ISA15             = PIn.String(row["ISA15"].ToString());
				clearinghouse.Password          = PIn.String(row["Password"].ToString());
				clearinghouse.ResponsePath      = PIn.String(row["ResponsePath"].ToString());
				clearinghouse.CommBridge        = (OpenDentBusiness.EclaimsCommBridge)PIn.Int(row["CommBridge"].ToString());
				clearinghouse.ClientProgram     = PIn.String(row["ClientProgram"].ToString());
				clearinghouse.LastBatchNumber   = PIn.Int   (row["LastBatchNumber"].ToString());
				clearinghouse.ModemPort         = PIn.Byte  (row["ModemPort"].ToString());
				clearinghouse.LoginID           = PIn.String(row["LoginID"].ToString());
				clearinghouse.SenderName        = PIn.String(row["SenderName"].ToString());
				clearinghouse.SenderTelephone   = PIn.String(row["SenderTelephone"].ToString());
				clearinghouse.GS03              = PIn.String(row["GS03"].ToString());
				clearinghouse.ISA02             = PIn.String(row["ISA02"].ToString());
				clearinghouse.ISA04             = PIn.String(row["ISA04"].ToString());
				clearinghouse.ISA16             = PIn.String(row["ISA16"].ToString());
				clearinghouse.SeparatorData     = PIn.String(row["SeparatorData"].ToString());
				clearinghouse.SeparatorSegment  = PIn.String(row["SeparatorSegment"].ToString());
				clearinghouse.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				clearinghouse.HqClearinghouseNum= PIn.Long  (row["HqClearinghouseNum"].ToString());
				retVal.Add(clearinghouse);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<Clearinghouse> listClearinghouses) {
			DataTable table=new DataTable("Clearinghouses");
			table.Columns.Add("ClearinghouseNum");
			table.Columns.Add("Description");
			table.Columns.Add("ExportPath");
			table.Columns.Add("Payors");
			table.Columns.Add("Eformat");
			table.Columns.Add("ISA05");
			table.Columns.Add("SenderTIN");
			table.Columns.Add("ISA07");
			table.Columns.Add("ISA08");
			table.Columns.Add("ISA15");
			table.Columns.Add("Password");
			table.Columns.Add("ResponsePath");
			table.Columns.Add("CommBridge");
			table.Columns.Add("ClientProgram");
			table.Columns.Add("LastBatchNumber");
			table.Columns.Add("ModemPort");
			table.Columns.Add("LoginID");
			table.Columns.Add("SenderName");
			table.Columns.Add("SenderTelephone");
			table.Columns.Add("GS03");
			table.Columns.Add("ISA02");
			table.Columns.Add("ISA04");
			table.Columns.Add("ISA16");
			table.Columns.Add("SeparatorData");
			table.Columns.Add("SeparatorSegment");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("HqClearinghouseNum");
			foreach(Clearinghouse clearinghouse in listClearinghouses) {
				table.Rows.Add(new object[] {
					POut.Long  (clearinghouse.ClearinghouseNum),
					POut.String(clearinghouse.Description),
					POut.String(clearinghouse.ExportPath),
					POut.String(clearinghouse.Payors),
					POut.Int   ((int)clearinghouse.Eformat),
					POut.String(clearinghouse.ISA05),
					POut.String(clearinghouse.SenderTIN),
					POut.String(clearinghouse.ISA07),
					POut.String(clearinghouse.ISA08),
					POut.String(clearinghouse.ISA15),
					POut.String(clearinghouse.Password),
					POut.String(clearinghouse.ResponsePath),
					POut.Int   ((int)clearinghouse.CommBridge),
					POut.String(clearinghouse.ClientProgram),
					POut.Int   (clearinghouse.LastBatchNumber),
					POut.Byte  (clearinghouse.ModemPort),
					POut.String(clearinghouse.LoginID),
					POut.String(clearinghouse.SenderName),
					POut.String(clearinghouse.SenderTelephone),
					POut.String(clearinghouse.GS03),
					POut.String(clearinghouse.ISA02),
					POut.String(clearinghouse.ISA04),
					POut.String(clearinghouse.ISA16),
					POut.String(clearinghouse.SeparatorData),
					POut.String(clearinghouse.SeparatorSegment),
					POut.Long  (clearinghouse.ClinicNum),
					POut.Long  (clearinghouse.HqClearinghouseNum),
				});
			}
			return table;
		}

		///<summary>Inserts one Clearinghouse into the database.  Returns the new priKey.</summary>
		public static long Insert(Clearinghouse clearinghouse){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				clearinghouse.ClearinghouseNum=DbHelper.GetNextOracleKey("clearinghouse","ClearinghouseNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(clearinghouse,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							clearinghouse.ClearinghouseNum++;
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
				return Insert(clearinghouse,false);
			}
		}

		///<summary>Inserts one Clearinghouse into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Clearinghouse clearinghouse,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				clearinghouse.ClearinghouseNum=ReplicationServers.GetKey("clearinghouse","ClearinghouseNum");
			}
			string command="INSERT INTO clearinghouse (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClearinghouseNum,";
			}
			command+="Description,ExportPath,Payors,Eformat,ISA05,SenderTIN,ISA07,ISA08,ISA15,Password,ResponsePath,CommBridge,ClientProgram,LastBatchNumber,ModemPort,LoginID,SenderName,SenderTelephone,GS03,ISA02,ISA04,ISA16,SeparatorData,SeparatorSegment,ClinicNum,HqClearinghouseNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(clearinghouse.ClearinghouseNum)+",";
			}
			command+=
				 "'"+POut.String(clearinghouse.Description)+"',"
				+"'"+POut.String(clearinghouse.ExportPath)+"',"
				+"'"+POut.String(clearinghouse.Payors)+"',"
				+    POut.Int   ((int)clearinghouse.Eformat)+","
				+"'"+POut.String(clearinghouse.ISA05)+"',"
				+"'"+POut.String(clearinghouse.SenderTIN)+"',"
				+"'"+POut.String(clearinghouse.ISA07)+"',"
				+"'"+POut.String(clearinghouse.ISA08)+"',"
				+"'"+POut.String(clearinghouse.ISA15)+"',"
				+"'"+POut.String(clearinghouse.Password)+"',"
				+"'"+POut.String(clearinghouse.ResponsePath)+"',"
				+    POut.Int   ((int)clearinghouse.CommBridge)+","
				+"'"+POut.String(clearinghouse.ClientProgram)+"',"
				+    POut.Int   (clearinghouse.LastBatchNumber)+","
				+    POut.Byte  (clearinghouse.ModemPort)+","
				+"'"+POut.String(clearinghouse.LoginID)+"',"
				+"'"+POut.String(clearinghouse.SenderName)+"',"
				+"'"+POut.String(clearinghouse.SenderTelephone)+"',"
				+"'"+POut.String(clearinghouse.GS03)+"',"
				+"'"+POut.String(clearinghouse.ISA02)+"',"
				+"'"+POut.String(clearinghouse.ISA04)+"',"
				+"'"+POut.String(clearinghouse.ISA16)+"',"
				+"'"+POut.String(clearinghouse.SeparatorData)+"',"
				+"'"+POut.String(clearinghouse.SeparatorSegment)+"',"
				+    POut.Long  (clearinghouse.ClinicNum)+","
				+    POut.Long  (clearinghouse.HqClearinghouseNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				clearinghouse.ClearinghouseNum=Db.NonQ(command,true);
			}
			return clearinghouse.ClearinghouseNum;
		}

		///<summary>Inserts one Clearinghouse into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Clearinghouse clearinghouse){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(clearinghouse,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					clearinghouse.ClearinghouseNum=DbHelper.GetNextOracleKey("clearinghouse","ClearinghouseNum"); //Cacheless method
				}
				return InsertNoCache(clearinghouse,true);
			}
		}

		///<summary>Inserts one Clearinghouse into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Clearinghouse clearinghouse,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO clearinghouse (";
			if(!useExistingPK && isRandomKeys) {
				clearinghouse.ClearinghouseNum=ReplicationServers.GetKeyNoCache("clearinghouse","ClearinghouseNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ClearinghouseNum,";
			}
			command+="Description,ExportPath,Payors,Eformat,ISA05,SenderTIN,ISA07,ISA08,ISA15,Password,ResponsePath,CommBridge,ClientProgram,LastBatchNumber,ModemPort,LoginID,SenderName,SenderTelephone,GS03,ISA02,ISA04,ISA16,SeparatorData,SeparatorSegment,ClinicNum,HqClearinghouseNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(clearinghouse.ClearinghouseNum)+",";
			}
			command+=
				 "'"+POut.String(clearinghouse.Description)+"',"
				+"'"+POut.String(clearinghouse.ExportPath)+"',"
				+"'"+POut.String(clearinghouse.Payors)+"',"
				+    POut.Int   ((int)clearinghouse.Eformat)+","
				+"'"+POut.String(clearinghouse.ISA05)+"',"
				+"'"+POut.String(clearinghouse.SenderTIN)+"',"
				+"'"+POut.String(clearinghouse.ISA07)+"',"
				+"'"+POut.String(clearinghouse.ISA08)+"',"
				+"'"+POut.String(clearinghouse.ISA15)+"',"
				+"'"+POut.String(clearinghouse.Password)+"',"
				+"'"+POut.String(clearinghouse.ResponsePath)+"',"
				+    POut.Int   ((int)clearinghouse.CommBridge)+","
				+"'"+POut.String(clearinghouse.ClientProgram)+"',"
				+    POut.Int   (clearinghouse.LastBatchNumber)+","
				+    POut.Byte  (clearinghouse.ModemPort)+","
				+"'"+POut.String(clearinghouse.LoginID)+"',"
				+"'"+POut.String(clearinghouse.SenderName)+"',"
				+"'"+POut.String(clearinghouse.SenderTelephone)+"',"
				+"'"+POut.String(clearinghouse.GS03)+"',"
				+"'"+POut.String(clearinghouse.ISA02)+"',"
				+"'"+POut.String(clearinghouse.ISA04)+"',"
				+"'"+POut.String(clearinghouse.ISA16)+"',"
				+"'"+POut.String(clearinghouse.SeparatorData)+"',"
				+"'"+POut.String(clearinghouse.SeparatorSegment)+"',"
				+    POut.Long  (clearinghouse.ClinicNum)+","
				+    POut.Long  (clearinghouse.HqClearinghouseNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				clearinghouse.ClearinghouseNum=Db.NonQ(command,true);
			}
			return clearinghouse.ClearinghouseNum;
		}

		///<summary>Updates one Clearinghouse in the database.</summary>
		public static void Update(Clearinghouse clearinghouse){
			string command="UPDATE clearinghouse SET "
				+"Description       = '"+POut.String(clearinghouse.Description)+"', "
				+"ExportPath        = '"+POut.String(clearinghouse.ExportPath)+"', "
				+"Payors            = '"+POut.String(clearinghouse.Payors)+"', "
				+"Eformat           =  "+POut.Int   ((int)clearinghouse.Eformat)+", "
				+"ISA05             = '"+POut.String(clearinghouse.ISA05)+"', "
				+"SenderTIN         = '"+POut.String(clearinghouse.SenderTIN)+"', "
				+"ISA07             = '"+POut.String(clearinghouse.ISA07)+"', "
				+"ISA08             = '"+POut.String(clearinghouse.ISA08)+"', "
				+"ISA15             = '"+POut.String(clearinghouse.ISA15)+"', "
				+"Password          = '"+POut.String(clearinghouse.Password)+"', "
				+"ResponsePath      = '"+POut.String(clearinghouse.ResponsePath)+"', "
				+"CommBridge        =  "+POut.Int   ((int)clearinghouse.CommBridge)+", "
				+"ClientProgram     = '"+POut.String(clearinghouse.ClientProgram)+"', "
				//LastBatchNumber excluded from update
				+"ModemPort         =  "+POut.Byte  (clearinghouse.ModemPort)+", "
				+"LoginID           = '"+POut.String(clearinghouse.LoginID)+"', "
				+"SenderName        = '"+POut.String(clearinghouse.SenderName)+"', "
				+"SenderTelephone   = '"+POut.String(clearinghouse.SenderTelephone)+"', "
				+"GS03              = '"+POut.String(clearinghouse.GS03)+"', "
				+"ISA02             = '"+POut.String(clearinghouse.ISA02)+"', "
				+"ISA04             = '"+POut.String(clearinghouse.ISA04)+"', "
				+"ISA16             = '"+POut.String(clearinghouse.ISA16)+"', "
				+"SeparatorData     = '"+POut.String(clearinghouse.SeparatorData)+"', "
				+"SeparatorSegment  = '"+POut.String(clearinghouse.SeparatorSegment)+"', "
				+"ClinicNum         =  "+POut.Long  (clearinghouse.ClinicNum)+", "
				+"HqClearinghouseNum=  "+POut.Long  (clearinghouse.HqClearinghouseNum)+" "
				+"WHERE ClearinghouseNum = "+POut.Long(clearinghouse.ClearinghouseNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Clearinghouse in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Clearinghouse clearinghouse,Clearinghouse oldClearinghouse){
			string command="";
			if(clearinghouse.Description != oldClearinghouse.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(clearinghouse.Description)+"'";
			}
			if(clearinghouse.ExportPath != oldClearinghouse.ExportPath) {
				if(command!=""){ command+=",";}
				command+="ExportPath = '"+POut.String(clearinghouse.ExportPath)+"'";
			}
			if(clearinghouse.Payors != oldClearinghouse.Payors) {
				if(command!=""){ command+=",";}
				command+="Payors = '"+POut.String(clearinghouse.Payors)+"'";
			}
			if(clearinghouse.Eformat != oldClearinghouse.Eformat) {
				if(command!=""){ command+=",";}
				command+="Eformat = "+POut.Int   ((int)clearinghouse.Eformat)+"";
			}
			if(clearinghouse.ISA05 != oldClearinghouse.ISA05) {
				if(command!=""){ command+=",";}
				command+="ISA05 = '"+POut.String(clearinghouse.ISA05)+"'";
			}
			if(clearinghouse.SenderTIN != oldClearinghouse.SenderTIN) {
				if(command!=""){ command+=",";}
				command+="SenderTIN = '"+POut.String(clearinghouse.SenderTIN)+"'";
			}
			if(clearinghouse.ISA07 != oldClearinghouse.ISA07) {
				if(command!=""){ command+=",";}
				command+="ISA07 = '"+POut.String(clearinghouse.ISA07)+"'";
			}
			if(clearinghouse.ISA08 != oldClearinghouse.ISA08) {
				if(command!=""){ command+=",";}
				command+="ISA08 = '"+POut.String(clearinghouse.ISA08)+"'";
			}
			if(clearinghouse.ISA15 != oldClearinghouse.ISA15) {
				if(command!=""){ command+=",";}
				command+="ISA15 = '"+POut.String(clearinghouse.ISA15)+"'";
			}
			if(clearinghouse.Password != oldClearinghouse.Password) {
				if(command!=""){ command+=",";}
				command+="Password = '"+POut.String(clearinghouse.Password)+"'";
			}
			if(clearinghouse.ResponsePath != oldClearinghouse.ResponsePath) {
				if(command!=""){ command+=",";}
				command+="ResponsePath = '"+POut.String(clearinghouse.ResponsePath)+"'";
			}
			if(clearinghouse.CommBridge != oldClearinghouse.CommBridge) {
				if(command!=""){ command+=",";}
				command+="CommBridge = "+POut.Int   ((int)clearinghouse.CommBridge)+"";
			}
			if(clearinghouse.ClientProgram != oldClearinghouse.ClientProgram) {
				if(command!=""){ command+=",";}
				command+="ClientProgram = '"+POut.String(clearinghouse.ClientProgram)+"'";
			}
			//LastBatchNumber excluded from update
			if(clearinghouse.ModemPort != oldClearinghouse.ModemPort) {
				if(command!=""){ command+=",";}
				command+="ModemPort = "+POut.Byte(clearinghouse.ModemPort)+"";
			}
			if(clearinghouse.LoginID != oldClearinghouse.LoginID) {
				if(command!=""){ command+=",";}
				command+="LoginID = '"+POut.String(clearinghouse.LoginID)+"'";
			}
			if(clearinghouse.SenderName != oldClearinghouse.SenderName) {
				if(command!=""){ command+=",";}
				command+="SenderName = '"+POut.String(clearinghouse.SenderName)+"'";
			}
			if(clearinghouse.SenderTelephone != oldClearinghouse.SenderTelephone) {
				if(command!=""){ command+=",";}
				command+="SenderTelephone = '"+POut.String(clearinghouse.SenderTelephone)+"'";
			}
			if(clearinghouse.GS03 != oldClearinghouse.GS03) {
				if(command!=""){ command+=",";}
				command+="GS03 = '"+POut.String(clearinghouse.GS03)+"'";
			}
			if(clearinghouse.ISA02 != oldClearinghouse.ISA02) {
				if(command!=""){ command+=",";}
				command+="ISA02 = '"+POut.String(clearinghouse.ISA02)+"'";
			}
			if(clearinghouse.ISA04 != oldClearinghouse.ISA04) {
				if(command!=""){ command+=",";}
				command+="ISA04 = '"+POut.String(clearinghouse.ISA04)+"'";
			}
			if(clearinghouse.ISA16 != oldClearinghouse.ISA16) {
				if(command!=""){ command+=",";}
				command+="ISA16 = '"+POut.String(clearinghouse.ISA16)+"'";
			}
			if(clearinghouse.SeparatorData != oldClearinghouse.SeparatorData) {
				if(command!=""){ command+=",";}
				command+="SeparatorData = '"+POut.String(clearinghouse.SeparatorData)+"'";
			}
			if(clearinghouse.SeparatorSegment != oldClearinghouse.SeparatorSegment) {
				if(command!=""){ command+=",";}
				command+="SeparatorSegment = '"+POut.String(clearinghouse.SeparatorSegment)+"'";
			}
			if(clearinghouse.ClinicNum != oldClearinghouse.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(clearinghouse.ClinicNum)+"";
			}
			if(clearinghouse.HqClearinghouseNum != oldClearinghouse.HqClearinghouseNum) {
				if(command!=""){ command+=",";}
				command+="HqClearinghouseNum = "+POut.Long(clearinghouse.HqClearinghouseNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE clearinghouse SET "+command
				+" WHERE ClearinghouseNum = "+POut.Long(clearinghouse.ClearinghouseNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Clearinghouse from the database.</summary>
		public static void Delete(long clearinghouseNum){
			string command="DELETE FROM clearinghouse "
				+"WHERE ClearinghouseNum = "+POut.Long(clearinghouseNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<Clearinghouse> listNew,List<Clearinghouse> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Clearinghouse> listIns    =new List<Clearinghouse>();
			List<Clearinghouse> listUpdNew =new List<Clearinghouse>();
			List<Clearinghouse> listUpdDB  =new List<Clearinghouse>();
			List<Clearinghouse> listDel    =new List<Clearinghouse>();
			listNew.Sort((Clearinghouse x,Clearinghouse y) => { return x.ClearinghouseNum.CompareTo(y.ClearinghouseNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((Clearinghouse x,Clearinghouse y) => { return x.ClearinghouseNum.CompareTo(y.ClearinghouseNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			Clearinghouse fieldNew;
			Clearinghouse fieldDB;
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
				else if(fieldNew.ClearinghouseNum<fieldDB.ClearinghouseNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ClearinghouseNum>fieldDB.ClearinghouseNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].ClearinghouseNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}