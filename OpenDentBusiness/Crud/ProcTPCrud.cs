//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcTPCrud {
		///<summary>Gets one ProcTP object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcTP SelectOne(long procTPNum){
			string command="SELECT * FROM proctp "
				+"WHERE ProcTPNum = "+POut.Long(procTPNum);
			List<ProcTP> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcTP object from the database using a query.</summary>
		public static ProcTP SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcTP> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcTP objects from the database using a query.</summary>
		public static List<ProcTP> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcTP> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcTP> TableToList(DataTable table){
			List<ProcTP> retVal=new List<ProcTP>();
			ProcTP procTP;
			foreach(DataRow row in table.Rows) {
				procTP=new ProcTP();
				procTP.ProcTPNum   = PIn.Long  (row["ProcTPNum"].ToString());
				procTP.TreatPlanNum= PIn.Long  (row["TreatPlanNum"].ToString());
				procTP.PatNum      = PIn.Long  (row["PatNum"].ToString());
				procTP.ProcNumOrig = PIn.Long  (row["ProcNumOrig"].ToString());
				procTP.ItemOrder   = PIn.Int   (row["ItemOrder"].ToString());
				procTP.Priority    = PIn.Long  (row["Priority"].ToString());
				procTP.ToothNumTP  = PIn.String(row["ToothNumTP"].ToString());
				procTP.Surf        = PIn.String(row["Surf"].ToString());
				procTP.ProcCode    = PIn.String(row["ProcCode"].ToString());
				procTP.Descript    = PIn.String(row["Descript"].ToString());
				procTP.FeeAmt      = PIn.Double(row["FeeAmt"].ToString());
				procTP.PriInsAmt   = PIn.Double(row["PriInsAmt"].ToString());
				procTP.SecInsAmt   = PIn.Double(row["SecInsAmt"].ToString());
				procTP.PatAmt      = PIn.Double(row["PatAmt"].ToString());
				procTP.Discount    = PIn.Double(row["Discount"].ToString());
				procTP.Prognosis   = PIn.String(row["Prognosis"].ToString());
				procTP.Dx          = PIn.String(row["Dx"].ToString());
				retVal.Add(procTP);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<ProcTP> listProcTPs) {
			DataTable table=new DataTable("ProcTPs");
			table.Columns.Add("ProcTPNum");
			table.Columns.Add("TreatPlanNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ProcNumOrig");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("Priority");
			table.Columns.Add("ToothNumTP");
			table.Columns.Add("Surf");
			table.Columns.Add("ProcCode");
			table.Columns.Add("Descript");
			table.Columns.Add("FeeAmt");
			table.Columns.Add("PriInsAmt");
			table.Columns.Add("SecInsAmt");
			table.Columns.Add("PatAmt");
			table.Columns.Add("Discount");
			table.Columns.Add("Prognosis");
			table.Columns.Add("Dx");
			foreach(ProcTP procTP in listProcTPs) {
				table.Rows.Add(new object[] {
					POut.Long  (procTP.ProcTPNum),
					POut.Long  (procTP.TreatPlanNum),
					POut.Long  (procTP.PatNum),
					POut.Long  (procTP.ProcNumOrig),
					POut.Int   (procTP.ItemOrder),
					POut.Long  (procTP.Priority),
					POut.String(procTP.ToothNumTP),
					POut.String(procTP.Surf),
					POut.String(procTP.ProcCode),
					POut.String(procTP.Descript),
					POut.Double(procTP.FeeAmt),
					POut.Double(procTP.PriInsAmt),
					POut.Double(procTP.SecInsAmt),
					POut.Double(procTP.PatAmt),
					POut.Double(procTP.Discount),
					POut.String(procTP.Prognosis),
					POut.String(procTP.Dx),
				});
			}
			return table;
		}

		///<summary>Inserts one ProcTP into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcTP procTP){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				procTP.ProcTPNum=DbHelper.GetNextOracleKey("proctp","ProcTPNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(procTP,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							procTP.ProcTPNum++;
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
				return Insert(procTP,false);
			}
		}

		///<summary>Inserts one ProcTP into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcTP procTP,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procTP.ProcTPNum=ReplicationServers.GetKey("proctp","ProcTPNum");
			}
			string command="INSERT INTO proctp (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcTPNum,";
			}
			command+="TreatPlanNum,PatNum,ProcNumOrig,ItemOrder,Priority,ToothNumTP,Surf,ProcCode,Descript,FeeAmt,PriInsAmt,SecInsAmt,PatAmt,Discount,Prognosis,Dx) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procTP.ProcTPNum)+",";
			}
			command+=
				     POut.Long  (procTP.TreatPlanNum)+","
				+    POut.Long  (procTP.PatNum)+","
				+    POut.Long  (procTP.ProcNumOrig)+","
				+    POut.Int   (procTP.ItemOrder)+","
				+    POut.Long  (procTP.Priority)+","
				+"'"+POut.String(procTP.ToothNumTP)+"',"
				+"'"+POut.String(procTP.Surf)+"',"
				+"'"+POut.String(procTP.ProcCode)+"',"
				+"'"+POut.String(procTP.Descript)+"',"
				+"'"+POut.Double(procTP.FeeAmt)+"',"
				+"'"+POut.Double(procTP.PriInsAmt)+"',"
				+"'"+POut.Double(procTP.SecInsAmt)+"',"
				+"'"+POut.Double(procTP.PatAmt)+"',"
				+"'"+POut.Double(procTP.Discount)+"',"
				+"'"+POut.String(procTP.Prognosis)+"',"
				+"'"+POut.String(procTP.Dx)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procTP.ProcTPNum=Db.NonQ(command,true);
			}
			return procTP.ProcTPNum;
		}

		///<summary>Inserts one ProcTP into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcTP procTP){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(procTP,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					procTP.ProcTPNum=DbHelper.GetNextOracleKey("proctp","ProcTPNum"); //Cacheless method
				}
				return InsertNoCache(procTP,true);
			}
		}

		///<summary>Inserts one ProcTP into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ProcTP procTP,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO proctp (";
			if(!useExistingPK && isRandomKeys) {
				procTP.ProcTPNum=ReplicationServers.GetKeyNoCache("proctp","ProcTPNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcTPNum,";
			}
			command+="TreatPlanNum,PatNum,ProcNumOrig,ItemOrder,Priority,ToothNumTP,Surf,ProcCode,Descript,FeeAmt,PriInsAmt,SecInsAmt,PatAmt,Discount,Prognosis,Dx) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procTP.ProcTPNum)+",";
			}
			command+=
				     POut.Long  (procTP.TreatPlanNum)+","
				+    POut.Long  (procTP.PatNum)+","
				+    POut.Long  (procTP.ProcNumOrig)+","
				+    POut.Int   (procTP.ItemOrder)+","
				+    POut.Long  (procTP.Priority)+","
				+"'"+POut.String(procTP.ToothNumTP)+"',"
				+"'"+POut.String(procTP.Surf)+"',"
				+"'"+POut.String(procTP.ProcCode)+"',"
				+"'"+POut.String(procTP.Descript)+"',"
				+"'"+POut.Double(procTP.FeeAmt)+"',"
				+"'"+POut.Double(procTP.PriInsAmt)+"',"
				+"'"+POut.Double(procTP.SecInsAmt)+"',"
				+"'"+POut.Double(procTP.PatAmt)+"',"
				+"'"+POut.Double(procTP.Discount)+"',"
				+"'"+POut.String(procTP.Prognosis)+"',"
				+"'"+POut.String(procTP.Dx)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				procTP.ProcTPNum=Db.NonQ(command,true);
			}
			return procTP.ProcTPNum;
		}

		///<summary>Updates one ProcTP in the database.</summary>
		public static void Update(ProcTP procTP){
			string command="UPDATE proctp SET "
				+"TreatPlanNum=  "+POut.Long  (procTP.TreatPlanNum)+", "
				+"PatNum      =  "+POut.Long  (procTP.PatNum)+", "
				+"ProcNumOrig =  "+POut.Long  (procTP.ProcNumOrig)+", "
				+"ItemOrder   =  "+POut.Int   (procTP.ItemOrder)+", "
				+"Priority    =  "+POut.Long  (procTP.Priority)+", "
				+"ToothNumTP  = '"+POut.String(procTP.ToothNumTP)+"', "
				+"Surf        = '"+POut.String(procTP.Surf)+"', "
				+"ProcCode    = '"+POut.String(procTP.ProcCode)+"', "
				+"Descript    = '"+POut.String(procTP.Descript)+"', "
				+"FeeAmt      = '"+POut.Double(procTP.FeeAmt)+"', "
				+"PriInsAmt   = '"+POut.Double(procTP.PriInsAmt)+"', "
				+"SecInsAmt   = '"+POut.Double(procTP.SecInsAmt)+"', "
				+"PatAmt      = '"+POut.Double(procTP.PatAmt)+"', "
				+"Discount    = '"+POut.Double(procTP.Discount)+"', "
				+"Prognosis   = '"+POut.String(procTP.Prognosis)+"', "
				+"Dx          = '"+POut.String(procTP.Dx)+"' "
				+"WHERE ProcTPNum = "+POut.Long(procTP.ProcTPNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcTP in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ProcTP procTP,ProcTP oldProcTP){
			string command="";
			if(procTP.TreatPlanNum != oldProcTP.TreatPlanNum) {
				if(command!=""){ command+=",";}
				command+="TreatPlanNum = "+POut.Long(procTP.TreatPlanNum)+"";
			}
			if(procTP.PatNum != oldProcTP.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(procTP.PatNum)+"";
			}
			if(procTP.ProcNumOrig != oldProcTP.ProcNumOrig) {
				if(command!=""){ command+=",";}
				command+="ProcNumOrig = "+POut.Long(procTP.ProcNumOrig)+"";
			}
			if(procTP.ItemOrder != oldProcTP.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(procTP.ItemOrder)+"";
			}
			if(procTP.Priority != oldProcTP.Priority) {
				if(command!=""){ command+=",";}
				command+="Priority = "+POut.Long(procTP.Priority)+"";
			}
			if(procTP.ToothNumTP != oldProcTP.ToothNumTP) {
				if(command!=""){ command+=",";}
				command+="ToothNumTP = '"+POut.String(procTP.ToothNumTP)+"'";
			}
			if(procTP.Surf != oldProcTP.Surf) {
				if(command!=""){ command+=",";}
				command+="Surf = '"+POut.String(procTP.Surf)+"'";
			}
			if(procTP.ProcCode != oldProcTP.ProcCode) {
				if(command!=""){ command+=",";}
				command+="ProcCode = '"+POut.String(procTP.ProcCode)+"'";
			}
			if(procTP.Descript != oldProcTP.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(procTP.Descript)+"'";
			}
			if(procTP.FeeAmt != oldProcTP.FeeAmt) {
				if(command!=""){ command+=",";}
				command+="FeeAmt = '"+POut.Double(procTP.FeeAmt)+"'";
			}
			if(procTP.PriInsAmt != oldProcTP.PriInsAmt) {
				if(command!=""){ command+=",";}
				command+="PriInsAmt = '"+POut.Double(procTP.PriInsAmt)+"'";
			}
			if(procTP.SecInsAmt != oldProcTP.SecInsAmt) {
				if(command!=""){ command+=",";}
				command+="SecInsAmt = '"+POut.Double(procTP.SecInsAmt)+"'";
			}
			if(procTP.PatAmt != oldProcTP.PatAmt) {
				if(command!=""){ command+=",";}
				command+="PatAmt = '"+POut.Double(procTP.PatAmt)+"'";
			}
			if(procTP.Discount != oldProcTP.Discount) {
				if(command!=""){ command+=",";}
				command+="Discount = '"+POut.Double(procTP.Discount)+"'";
			}
			if(procTP.Prognosis != oldProcTP.Prognosis) {
				if(command!=""){ command+=",";}
				command+="Prognosis = '"+POut.String(procTP.Prognosis)+"'";
			}
			if(procTP.Dx != oldProcTP.Dx) {
				if(command!=""){ command+=",";}
				command+="Dx = '"+POut.String(procTP.Dx)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE proctp SET "+command
				+" WHERE ProcTPNum = "+POut.Long(procTP.ProcTPNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one ProcTP from the database.</summary>
		public static void Delete(long procTPNum){
			string command="DELETE FROM proctp "
				+"WHERE ProcTPNum = "+POut.Long(procTPNum);
			Db.NonQ(command);
		}

	}
}