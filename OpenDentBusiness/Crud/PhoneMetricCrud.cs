//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PhoneMetricCrud {
		///<summary>Gets one PhoneMetric object from the database using the primary key.  Returns null if not found.</summary>
		public static PhoneMetric SelectOne(long phoneMetricNum){
			string command="SELECT * FROM phonemetric "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetricNum);
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PhoneMetric object from the database using a query.</summary>
		public static PhoneMetric SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PhoneMetric objects from the database using a query.</summary>
		public static List<PhoneMetric> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<PhoneMetric> TableToList(DataTable table){
			List<PhoneMetric> retVal=new List<PhoneMetric>();
			PhoneMetric phoneMetric;
			foreach(DataRow row in table.Rows) {
				phoneMetric=new PhoneMetric();
				phoneMetric.PhoneMetricNum= PIn.Long  (row["PhoneMetricNum"].ToString());
				phoneMetric.DateTimeEntry = PIn.DateT (row["DateTimeEntry"].ToString());
				phoneMetric.VoiceMails    = PIn.Int   (row["VoiceMails"].ToString());
				phoneMetric.Triages       = PIn.Int   (row["Triages"].ToString());
				phoneMetric.MinutesBehind = PIn.Int   (row["MinutesBehind"].ToString());
				retVal.Add(phoneMetric);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<PhoneMetric> listPhoneMetrics) {
			DataTable table=new DataTable("PhoneMetrics");
			table.Columns.Add("PhoneMetricNum");
			table.Columns.Add("DateTimeEntry");
			table.Columns.Add("VoiceMails");
			table.Columns.Add("Triages");
			table.Columns.Add("MinutesBehind");
			foreach(PhoneMetric phoneMetric in listPhoneMetrics) {
				table.Rows.Add(new object[] {
					POut.Long  (phoneMetric.PhoneMetricNum),
					POut.DateT (phoneMetric.DateTimeEntry),
					POut.Int   (phoneMetric.VoiceMails),
					POut.Int   (phoneMetric.Triages),
					POut.Int   (phoneMetric.MinutesBehind),
				});
			}
			return table;
		}

		///<summary>Inserts one PhoneMetric into the database.  Returns the new priKey.</summary>
		public static long Insert(PhoneMetric phoneMetric){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				phoneMetric.PhoneMetricNum=DbHelper.GetNextOracleKey("phonemetric","PhoneMetricNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(phoneMetric,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							phoneMetric.PhoneMetricNum++;
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
				return Insert(phoneMetric,false);
			}
		}

		///<summary>Inserts one PhoneMetric into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(PhoneMetric phoneMetric,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				phoneMetric.PhoneMetricNum=ReplicationServers.GetKey("phonemetric","PhoneMetricNum");
			}
			string command="INSERT INTO phonemetric (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PhoneMetricNum,";
			}
			command+="DateTimeEntry,VoiceMails,Triages,MinutesBehind) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(phoneMetric.PhoneMetricNum)+",";
			}
			command+=
				     POut.DateT (phoneMetric.DateTimeEntry)+","
				+    POut.Int   (phoneMetric.VoiceMails)+","
				+    POut.Int   (phoneMetric.Triages)+","
				+    POut.Int   (phoneMetric.MinutesBehind)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneMetric.PhoneMetricNum=Db.NonQ(command,true);
			}
			return phoneMetric.PhoneMetricNum;
		}

		///<summary>Inserts one PhoneMetric into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneMetric phoneMetric){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(phoneMetric,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					phoneMetric.PhoneMetricNum=DbHelper.GetNextOracleKey("phonemetric","PhoneMetricNum"); //Cacheless method
				}
				return InsertNoCache(phoneMetric,true);
			}
		}

		///<summary>Inserts one PhoneMetric into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(PhoneMetric phoneMetric,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO phonemetric (";
			if(!useExistingPK && isRandomKeys) {
				phoneMetric.PhoneMetricNum=ReplicationServers.GetKeyNoCache("phonemetric","PhoneMetricNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="PhoneMetricNum,";
			}
			command+="DateTimeEntry,VoiceMails,Triages,MinutesBehind) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(phoneMetric.PhoneMetricNum)+",";
			}
			command+=
				     POut.DateT (phoneMetric.DateTimeEntry)+","
				+    POut.Int   (phoneMetric.VoiceMails)+","
				+    POut.Int   (phoneMetric.Triages)+","
				+    POut.Int   (phoneMetric.MinutesBehind)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneMetric.PhoneMetricNum=Db.NonQ(command,true);
			}
			return phoneMetric.PhoneMetricNum;
		}

		///<summary>Updates one PhoneMetric in the database.</summary>
		public static void Update(PhoneMetric phoneMetric){
			string command="UPDATE phonemetric SET "
				+"DateTimeEntry =  "+POut.DateT (phoneMetric.DateTimeEntry)+", "
				+"VoiceMails    =  "+POut.Int   (phoneMetric.VoiceMails)+", "
				+"Triages       =  "+POut.Int   (phoneMetric.Triages)+", "
				+"MinutesBehind =  "+POut.Int   (phoneMetric.MinutesBehind)+" "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetric.PhoneMetricNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PhoneMetric in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(PhoneMetric phoneMetric,PhoneMetric oldPhoneMetric){
			string command="";
			if(phoneMetric.DateTimeEntry != oldPhoneMetric.DateTimeEntry) {
				if(command!=""){ command+=",";}
				command+="DateTimeEntry = "+POut.DateT(phoneMetric.DateTimeEntry)+"";
			}
			if(phoneMetric.VoiceMails != oldPhoneMetric.VoiceMails) {
				if(command!=""){ command+=",";}
				command+="VoiceMails = "+POut.Int(phoneMetric.VoiceMails)+"";
			}
			if(phoneMetric.Triages != oldPhoneMetric.Triages) {
				if(command!=""){ command+=",";}
				command+="Triages = "+POut.Int(phoneMetric.Triages)+"";
			}
			if(phoneMetric.MinutesBehind != oldPhoneMetric.MinutesBehind) {
				if(command!=""){ command+=",";}
				command+="MinutesBehind = "+POut.Int(phoneMetric.MinutesBehind)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE phonemetric SET "+command
				+" WHERE PhoneMetricNum = "+POut.Long(phoneMetric.PhoneMetricNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one PhoneMetric from the database.</summary>
		public static void Delete(long phoneMetricNum){
			string command="DELETE FROM phonemetric "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetricNum);
			Db.NonQ(command);
		}

	}
}