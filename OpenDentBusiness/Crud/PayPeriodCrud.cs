//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PayPeriodCrud {
		///<summary>Gets one PayPeriod object from the database using the primary key.  Returns null if not found.</summary>
		public static PayPeriod SelectOne(long payPeriodNum){
			string command="SELECT * FROM payperiod "
				+"WHERE PayPeriodNum = "+POut.Long(payPeriodNum);
			List<PayPeriod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PayPeriod object from the database using a query.</summary>
		public static PayPeriod SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPeriod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PayPeriod objects from the database using a query.</summary>
		public static List<PayPeriod> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPeriod> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PayPeriod> TableToList(DataTable table){
			List<PayPeriod> retVal=new List<PayPeriod>();
			PayPeriod payPeriod;
			foreach(DataRow row in table.Rows) {
				payPeriod=new PayPeriod();
				payPeriod.PayPeriodNum= PIn.Long  (row["PayPeriodNum"].ToString());
				payPeriod.DateStart   = PIn.Date  (row["DateStart"].ToString());
				payPeriod.DateStop    = PIn.Date  (row["DateStop"].ToString());
				payPeriod.DatePaycheck= PIn.Date  (row["DatePaycheck"].ToString());
				retVal.Add(payPeriod);
			}
			return retVal;
		}

		///<summary>Inserts one PayPeriod into the database.  Returns the new priKey.</summary>
		public static long Insert(PayPeriod payPeriod){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				payPeriod.PayPeriodNum=DbHelper.GetNextOracleKey("payperiod","PayPeriodNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(payPeriod,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							payPeriod.PayPeriodNum++;
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
				return Insert(payPeriod,false);
			}
		}

		///<summary>Inserts one PayPeriod into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PayPeriod payPeriod,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				payPeriod.PayPeriodNum=ReplicationServers.GetKey("payperiod","PayPeriodNum");
			}
			string command="INSERT INTO payperiod (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PayPeriodNum,";
			}
			command+="DateStart,DateStop,DatePaycheck) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(payPeriod.PayPeriodNum)+",";
			}
			command+=
				     POut.Date  (payPeriod.DateStart)+","
				+    POut.Date  (payPeriod.DateStop)+","
				+    POut.Date  (payPeriod.DatePaycheck)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				payPeriod.PayPeriodNum=Db.NonQ(command,true);
			}
			return payPeriod.PayPeriodNum;
		}

		///<summary>Inserts one PayPeriod into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayPeriod payPeriod){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(payPeriod,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					payPeriod.PayPeriodNum=DbHelper.GetNextOracleKey("payperiod","PayPeriodNum"); //Cacheless method
				}
				return InsertNoCache(payPeriod,true);
			}
		}

		///<summary>Inserts one PayPeriod into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayPeriod payPeriod,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO payperiod (";
			if(!useExistingPK && isRandomKeys) {
				payPeriod.PayPeriodNum=ReplicationServers.GetKeyNoCache("payperiod","PayPeriodNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PayPeriodNum,";
			}
			command+="DateStart,DateStop,DatePaycheck) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(payPeriod.PayPeriodNum)+",";
			}
			command+=
				     POut.Date  (payPeriod.DateStart)+","
				+    POut.Date  (payPeriod.DateStop)+","
				+    POut.Date  (payPeriod.DatePaycheck)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				payPeriod.PayPeriodNum=Db.NonQ(command,true);
			}
			return payPeriod.PayPeriodNum;
		}

		///<summary>Updates one PayPeriod in the database.</summary>
		public static void Update(PayPeriod payPeriod){
			string command="UPDATE payperiod SET "
				+"DateStart   =  "+POut.Date  (payPeriod.DateStart)+", "
				+"DateStop    =  "+POut.Date  (payPeriod.DateStop)+", "
				+"DatePaycheck=  "+POut.Date  (payPeriod.DatePaycheck)+" "
				+"WHERE PayPeriodNum = "+POut.Long(payPeriod.PayPeriodNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PayPeriod in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PayPeriod payPeriod,PayPeriod oldPayPeriod){
			string command="";
			if(payPeriod.DateStart != oldPayPeriod.DateStart) {
				if(command!=""){ command+=",";}
				command+="DateStart = "+POut.Date(payPeriod.DateStart)+"";
			}
			if(payPeriod.DateStop != oldPayPeriod.DateStop) {
				if(command!=""){ command+=",";}
				command+="DateStop = "+POut.Date(payPeriod.DateStop)+"";
			}
			if(payPeriod.DatePaycheck != oldPayPeriod.DatePaycheck) {
				if(command!=""){ command+=",";}
				command+="DatePaycheck = "+POut.Date(payPeriod.DatePaycheck)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE payperiod SET "+command
				+" WHERE PayPeriodNum = "+POut.Long(payPeriod.PayPeriodNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(PayPeriod,PayPeriod) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(PayPeriod payPeriod,PayPeriod oldPayPeriod) {
			if(payPeriod.DateStart != oldPayPeriod.DateStart) {
				return true;
			}
			if(payPeriod.DateStop != oldPayPeriod.DateStop) {
				return true;
			}
			if(payPeriod.DatePaycheck != oldPayPeriod.DatePaycheck) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one PayPeriod from the database.</summary>
		public static void Delete(long payPeriodNum){
			string command="DELETE FROM payperiod "
				+"WHERE PayPeriodNum = "+POut.Long(payPeriodNum);
			Db.NonQ(command);
		}

	}
}