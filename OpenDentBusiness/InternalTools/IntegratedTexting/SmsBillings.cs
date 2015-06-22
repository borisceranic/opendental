using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsBillings{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all SmsBillings.</summary>
		private static List<SmsBilling> listt;

		///<summary>A list of all SmsBillings.</summary>
		public static List<SmsBilling> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM smsbilling ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SmsBilling";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.SmsBillingCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsBilling> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsBilling>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsbilling WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsBillingCrud.SelectMany(command);
		}

		///<summary>Gets one SmsBilling from the db.</summary>
		public static SmsBilling GetOne(long smsBillingNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsBilling>(MethodBase.GetCurrentMethod(),smsBillingNum);
			}
			return Crud.SmsBillingCrud.SelectOne(smsBillingNum);
		}

		///<summary></summary>
		public static long Insert(SmsBilling smsBilling){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsBilling.SmsBillingNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsBilling);
				return smsBilling.SmsBillingNum;
			}
			return Crud.SmsBillingCrud.Insert(smsBilling);
		}

		///<summary></summary>
		public static void Update(SmsBilling smsBilling){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsBilling);
				return;
			}
			Crud.SmsBillingCrud.Update(smsBilling);
		}

		///<summary></summary>
		public static void Delete(long smsBillingNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsBillingNum);
				return;
			}
			Crud.SmsBillingCrud.Delete(smsBillingNum);
		}
		*/


		/*
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS smsbilling";
					Db.NonQ(command);
					command=@"CREATE TABLE smsbilling (
						SmsBillingNum bigint NOT NULL auto_increment PRIMARY KEY,
						RegistrationKeyNum bigint NOT NULL,
						CustPatNum bigint NOT NULL,
						DateUsage date NOT NULL DEFAULT '0001-01-01',
						MsgChargeTotalUSD float NOT NULL,
						ClinicsTotal int NOT NULL,
						ClinicsActive int NOT NULL,
						ClinicsInactive int NOT NULL,
						ClinicsWithUsage int NOT NULL,
						PhonesTotal int NOT NULL,
						PhonesActive int NOT NULL,
						PhonesInactive int NOT NULL,
						PhonesWithUsage int NOT NULL,
						INDEX(RegistrationKeyNum),
						INDEX(CustPatNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE smsbilling'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE smsbilling (
						SmsBillingNum number(20) NOT NULL,
						RegistrationKeyNum number(20) NOT NULL,
						CustPatNum number(20) NOT NULL,
						DateUsage date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						MsgChargeTotalUSD number(38,8) NOT NULL,
						ClinicsTotal number(11) NOT NULL,
						ClinicsActive number(11) NOT NULL,
						ClinicsInactive number(11) NOT NULL,
						ClinicsWithUsage number(11) NOT NULL,
						PhonesTotal number(11) NOT NULL,
						PhonesActive number(11) NOT NULL,
						PhonesInactive number(11) NOT NULL,
						PhonesWithUsage number(11) NOT NULL,
						CONSTRAINT smsbilling_SmsBillingNum PRIMARY KEY (SmsBillingNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX smsbilling_RegistrationKeyNum ON smsbilling (RegistrationKeyNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX smsbilling_CustPatNum ON smsbilling (CustPatNum)";
					Db.NonQ(command);
				}
		*/

	}
}