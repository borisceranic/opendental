//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PayPlanChargeCrud {
		///<summary>Gets one PayPlanCharge object from the database using the primary key.  Returns null if not found.</summary>
		public static PayPlanCharge SelectOne(long payPlanChargeNum){
			string command="SELECT * FROM payplancharge "
				+"WHERE PayPlanChargeNum = "+POut.Long(payPlanChargeNum);
			List<PayPlanCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PayPlanCharge object from the database using a query.</summary>
		public static PayPlanCharge SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPlanCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PayPlanCharge objects from the database using a query.</summary>
		public static List<PayPlanCharge> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPlanCharge> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PayPlanCharge> TableToList(DataTable table){
			List<PayPlanCharge> retVal=new List<PayPlanCharge>();
			PayPlanCharge payPlanCharge;
			foreach(DataRow row in table.Rows) {
				payPlanCharge=new PayPlanCharge();
				payPlanCharge.PayPlanChargeNum= PIn.Long  (row["PayPlanChargeNum"].ToString());
				payPlanCharge.PayPlanNum      = PIn.Long  (row["PayPlanNum"].ToString());
				payPlanCharge.Guarantor       = PIn.Long  (row["Guarantor"].ToString());
				payPlanCharge.PatNum          = PIn.Long  (row["PatNum"].ToString());
				payPlanCharge.ChargeDate      = PIn.Date  (row["ChargeDate"].ToString());
				payPlanCharge.Principal       = PIn.Double(row["Principal"].ToString());
				payPlanCharge.Interest        = PIn.Double(row["Interest"].ToString());
				payPlanCharge.Note            = PIn.String(row["Note"].ToString());
				payPlanCharge.ProvNum         = PIn.Long  (row["ProvNum"].ToString());
				payPlanCharge.ClinicNum       = PIn.Long  (row["ClinicNum"].ToString());
				retVal.Add(payPlanCharge);
			}
			return retVal;
		}

		///<summary>Inserts one PayPlanCharge into the database.  Returns the new priKey.</summary>
		public static long Insert(PayPlanCharge payPlanCharge){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				payPlanCharge.PayPlanChargeNum=DbHelper.GetNextOracleKey("payplancharge","PayPlanChargeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(payPlanCharge,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							payPlanCharge.PayPlanChargeNum++;
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
				return Insert(payPlanCharge,false);
			}
		}

		///<summary>Inserts one PayPlanCharge into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PayPlanCharge payPlanCharge,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				payPlanCharge.PayPlanChargeNum=ReplicationServers.GetKey("payplancharge","PayPlanChargeNum");
			}
			string command="INSERT INTO payplancharge (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PayPlanChargeNum,";
			}
			command+="PayPlanNum,Guarantor,PatNum,ChargeDate,Principal,Interest,Note,ProvNum,ClinicNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(payPlanCharge.PayPlanChargeNum)+",";
			}
			command+=
				     POut.Long  (payPlanCharge.PayPlanNum)+","
				+    POut.Long  (payPlanCharge.Guarantor)+","
				+    POut.Long  (payPlanCharge.PatNum)+","
				+    POut.Date  (payPlanCharge.ChargeDate)+","
				+"'"+POut.Double(payPlanCharge.Principal)+"',"
				+"'"+POut.Double(payPlanCharge.Interest)+"',"
				+"'"+POut.String(payPlanCharge.Note)+"',"
				+    POut.Long  (payPlanCharge.ProvNum)+","
				+    POut.Long  (payPlanCharge.ClinicNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				payPlanCharge.PayPlanChargeNum=Db.NonQ(command,true);
			}
			return payPlanCharge.PayPlanChargeNum;
		}

		///<summary>Inserts one PayPlanCharge into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayPlanCharge payPlanCharge){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(payPlanCharge,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					payPlanCharge.PayPlanChargeNum=DbHelper.GetNextOracleKey("payplancharge","PayPlanChargeNum"); //Cacheless method
				}
				return InsertNoCache(payPlanCharge,true);
			}
		}

		///<summary>Inserts one PayPlanCharge into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PayPlanCharge payPlanCharge,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO payplancharge (";
			if(!useExistingPK && isRandomKeys) {
				payPlanCharge.PayPlanChargeNum=ReplicationServers.GetKeyNoCache("payplancharge","PayPlanChargeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PayPlanChargeNum,";
			}
			command+="PayPlanNum,Guarantor,PatNum,ChargeDate,Principal,Interest,Note,ProvNum,ClinicNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(payPlanCharge.PayPlanChargeNum)+",";
			}
			command+=
				     POut.Long  (payPlanCharge.PayPlanNum)+","
				+    POut.Long  (payPlanCharge.Guarantor)+","
				+    POut.Long  (payPlanCharge.PatNum)+","
				+    POut.Date  (payPlanCharge.ChargeDate)+","
				+"'"+POut.Double(payPlanCharge.Principal)+"',"
				+"'"+POut.Double(payPlanCharge.Interest)+"',"
				+"'"+POut.String(payPlanCharge.Note)+"',"
				+    POut.Long  (payPlanCharge.ProvNum)+","
				+    POut.Long  (payPlanCharge.ClinicNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				payPlanCharge.PayPlanChargeNum=Db.NonQ(command,true);
			}
			return payPlanCharge.PayPlanChargeNum;
		}

		///<summary>Updates one PayPlanCharge in the database.</summary>
		public static void Update(PayPlanCharge payPlanCharge){
			string command="UPDATE payplancharge SET "
				+"PayPlanNum      =  "+POut.Long  (payPlanCharge.PayPlanNum)+", "
				+"Guarantor       =  "+POut.Long  (payPlanCharge.Guarantor)+", "
				+"PatNum          =  "+POut.Long  (payPlanCharge.PatNum)+", "
				+"ChargeDate      =  "+POut.Date  (payPlanCharge.ChargeDate)+", "
				+"Principal       = '"+POut.Double(payPlanCharge.Principal)+"', "
				+"Interest        = '"+POut.Double(payPlanCharge.Interest)+"', "
				+"Note            = '"+POut.String(payPlanCharge.Note)+"', "
				+"ProvNum         =  "+POut.Long  (payPlanCharge.ProvNum)+", "
				+"ClinicNum       =  "+POut.Long  (payPlanCharge.ClinicNum)+" "
				+"WHERE PayPlanChargeNum = "+POut.Long(payPlanCharge.PayPlanChargeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PayPlanCharge in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PayPlanCharge payPlanCharge,PayPlanCharge oldPayPlanCharge){
			string command="";
			if(payPlanCharge.PayPlanNum != oldPayPlanCharge.PayPlanNum) {
				if(command!=""){ command+=",";}
				command+="PayPlanNum = "+POut.Long(payPlanCharge.PayPlanNum)+"";
			}
			if(payPlanCharge.Guarantor != oldPayPlanCharge.Guarantor) {
				if(command!=""){ command+=",";}
				command+="Guarantor = "+POut.Long(payPlanCharge.Guarantor)+"";
			}
			if(payPlanCharge.PatNum != oldPayPlanCharge.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(payPlanCharge.PatNum)+"";
			}
			if(payPlanCharge.ChargeDate != oldPayPlanCharge.ChargeDate) {
				if(command!=""){ command+=",";}
				command+="ChargeDate = "+POut.Date(payPlanCharge.ChargeDate)+"";
			}
			if(payPlanCharge.Principal != oldPayPlanCharge.Principal) {
				if(command!=""){ command+=",";}
				command+="Principal = '"+POut.Double(payPlanCharge.Principal)+"'";
			}
			if(payPlanCharge.Interest != oldPayPlanCharge.Interest) {
				if(command!=""){ command+=",";}
				command+="Interest = '"+POut.Double(payPlanCharge.Interest)+"'";
			}
			if(payPlanCharge.Note != oldPayPlanCharge.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(payPlanCharge.Note)+"'";
			}
			if(payPlanCharge.ProvNum != oldPayPlanCharge.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(payPlanCharge.ProvNum)+"";
			}
			if(payPlanCharge.ClinicNum != oldPayPlanCharge.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(payPlanCharge.ClinicNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE payplancharge SET "+command
				+" WHERE PayPlanChargeNum = "+POut.Long(payPlanCharge.PayPlanChargeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one PayPlanCharge from the database.</summary>
		public static void Delete(long payPlanChargeNum){
			string command="DELETE FROM payplancharge "
				+"WHERE PayPlanChargeNum = "+POut.Long(payPlanChargeNum);
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<PayPlanCharge> listNew,List<PayPlanCharge> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<PayPlanCharge> listIns    =new List<PayPlanCharge>();
			List<PayPlanCharge> listUpdNew =new List<PayPlanCharge>();
			List<PayPlanCharge> listUpdDB  =new List<PayPlanCharge>();
			List<PayPlanCharge> listDel    =new List<PayPlanCharge>();
			listNew.Sort((PayPlanCharge x,PayPlanCharge y) => { return x.PayPlanChargeNum.CompareTo(y.PayPlanChargeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((PayPlanCharge x,PayPlanCharge y) => { return x.PayPlanChargeNum.CompareTo(y.PayPlanChargeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			PayPlanCharge fieldNew;
			PayPlanCharge fieldDB;
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
				else if(fieldNew.PayPlanChargeNum<fieldDB.PayPlanChargeNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.PayPlanChargeNum>fieldDB.PayPlanChargeNum) {//dbPK less than newPK, dbItem is 'next'
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
				Delete(listDel[i].PayPlanChargeNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}