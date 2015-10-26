//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class InstallmentPlanCrud {
		///<summary>Gets one InstallmentPlan object from the database using the primary key.  Returns null if not found.</summary>
		public static InstallmentPlan SelectOne(long installmentPlanNum){
			string command="SELECT * FROM installmentplan "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlanNum);
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one InstallmentPlan object from the database using a query.</summary>
		public static InstallmentPlan SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of InstallmentPlan objects from the database using a query.</summary>
		public static List<InstallmentPlan> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InstallmentPlan> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<InstallmentPlan> TableToList(DataTable table){
			List<InstallmentPlan> retVal=new List<InstallmentPlan>();
			InstallmentPlan installmentPlan;
			foreach(DataRow row in table.Rows) {
				installmentPlan=new InstallmentPlan();
				installmentPlan.InstallmentPlanNum= PIn.Long  (row["InstallmentPlanNum"].ToString());
				installmentPlan.PatNum            = PIn.Long  (row["PatNum"].ToString());
				installmentPlan.DateAgreement     = PIn.Date  (row["DateAgreement"].ToString());
				installmentPlan.DateFirstPayment  = PIn.Date  (row["DateFirstPayment"].ToString());
				installmentPlan.MonthlyPayment    = PIn.Double(row["MonthlyPayment"].ToString());
				installmentPlan.APR               = PIn.Float (row["APR"].ToString());
				installmentPlan.Note              = PIn.String(row["Note"].ToString());
				retVal.Add(installmentPlan);
			}
			return retVal;
		}

		///<summary>Inserts one InstallmentPlan into the database.  Returns the new priKey.</summary>
		public static long Insert(InstallmentPlan installmentPlan){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				installmentPlan.InstallmentPlanNum=DbHelper.GetNextOracleKey("installmentplan","InstallmentPlanNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(installmentPlan,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							installmentPlan.InstallmentPlanNum++;
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
				return Insert(installmentPlan,false);
			}
		}

		///<summary>Inserts one InstallmentPlan into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(InstallmentPlan installmentPlan,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				installmentPlan.InstallmentPlanNum=ReplicationServers.GetKey("installmentplan","InstallmentPlanNum");
			}
			string command="INSERT INTO installmentplan (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="InstallmentPlanNum,";
			}
			command+="PatNum,DateAgreement,DateFirstPayment,MonthlyPayment,APR,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(installmentPlan.InstallmentPlanNum)+",";
			}
			command+=
				     POut.Long  (installmentPlan.PatNum)+","
				+    POut.Date  (installmentPlan.DateAgreement)+","
				+    POut.Date  (installmentPlan.DateFirstPayment)+","
				+"'"+POut.Double(installmentPlan.MonthlyPayment)+"',"
				+    POut.Float (installmentPlan.APR)+","
				+"'"+POut.String(installmentPlan.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				installmentPlan.InstallmentPlanNum=Db.NonQ(command,true);
			}
			return installmentPlan.InstallmentPlanNum;
		}

		///<summary>Inserts one InstallmentPlan into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InstallmentPlan installmentPlan){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(installmentPlan,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					installmentPlan.InstallmentPlanNum=DbHelper.GetNextOracleKey("installmentplan","InstallmentPlanNum"); //Cacheless method
				}
				return InsertNoCache(installmentPlan,true);
			}
		}

		///<summary>Inserts one InstallmentPlan into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InstallmentPlan installmentPlan,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO installmentplan (";
			if(!useExistingPK && isRandomKeys) {
				installmentPlan.InstallmentPlanNum=ReplicationServers.GetKeyNoCache("installmentplan","InstallmentPlanNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="InstallmentPlanNum,";
			}
			command+="PatNum,DateAgreement,DateFirstPayment,MonthlyPayment,APR,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(installmentPlan.InstallmentPlanNum)+",";
			}
			command+=
				     POut.Long  (installmentPlan.PatNum)+","
				+    POut.Date  (installmentPlan.DateAgreement)+","
				+    POut.Date  (installmentPlan.DateFirstPayment)+","
				+"'"+POut.Double(installmentPlan.MonthlyPayment)+"',"
				+    POut.Float (installmentPlan.APR)+","
				+"'"+POut.String(installmentPlan.Note)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				installmentPlan.InstallmentPlanNum=Db.NonQ(command,true);
			}
			return installmentPlan.InstallmentPlanNum;
		}

		///<summary>Updates one InstallmentPlan in the database.</summary>
		public static void Update(InstallmentPlan installmentPlan){
			string command="UPDATE installmentplan SET "
				+"PatNum            =  "+POut.Long  (installmentPlan.PatNum)+", "
				+"DateAgreement     =  "+POut.Date  (installmentPlan.DateAgreement)+", "
				+"DateFirstPayment  =  "+POut.Date  (installmentPlan.DateFirstPayment)+", "
				+"MonthlyPayment    = '"+POut.Double(installmentPlan.MonthlyPayment)+"', "
				+"APR               =  "+POut.Float (installmentPlan.APR)+", "
				+"Note              = '"+POut.String(installmentPlan.Note)+"' "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlan.InstallmentPlanNum);
			Db.NonQ(command);
		}

		///<summary>Updates one InstallmentPlan in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(InstallmentPlan installmentPlan,InstallmentPlan oldInstallmentPlan){
			string command="";
			if(installmentPlan.PatNum != oldInstallmentPlan.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(installmentPlan.PatNum)+"";
			}
			if(installmentPlan.DateAgreement != oldInstallmentPlan.DateAgreement) {
				if(command!=""){ command+=",";}
				command+="DateAgreement = "+POut.Date(installmentPlan.DateAgreement)+"";
			}
			if(installmentPlan.DateFirstPayment != oldInstallmentPlan.DateFirstPayment) {
				if(command!=""){ command+=",";}
				command+="DateFirstPayment = "+POut.Date(installmentPlan.DateFirstPayment)+"";
			}
			if(installmentPlan.MonthlyPayment != oldInstallmentPlan.MonthlyPayment) {
				if(command!=""){ command+=",";}
				command+="MonthlyPayment = '"+POut.Double(installmentPlan.MonthlyPayment)+"'";
			}
			if(installmentPlan.APR != oldInstallmentPlan.APR) {
				if(command!=""){ command+=",";}
				command+="APR = "+POut.Float(installmentPlan.APR)+"";
			}
			if(installmentPlan.Note != oldInstallmentPlan.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(installmentPlan.Note)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE installmentplan SET "+command
				+" WHERE InstallmentPlanNum = "+POut.Long(installmentPlan.InstallmentPlanNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one InstallmentPlan from the database.</summary>
		public static void Delete(long installmentPlanNum){
			string command="DELETE FROM installmentplan "
				+"WHERE InstallmentPlanNum = "+POut.Long(installmentPlanNum);
			Db.NonQ(command);
		}

	}
}