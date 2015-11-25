//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class RepeatChargeCrud {
		///<summary>Gets one RepeatCharge object from the database using the primary key.  Returns null if not found.</summary>
		public static RepeatCharge SelectOne(long repeatChargeNum){
			string command="SELECT * FROM repeatcharge "
				+"WHERE RepeatChargeNum = "+POut.Long(repeatChargeNum);
			List<RepeatCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RepeatCharge object from the database using a query.</summary>
		public static RepeatCharge SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RepeatCharge> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RepeatCharge objects from the database using a query.</summary>
		public static List<RepeatCharge> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RepeatCharge> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<RepeatCharge> TableToList(DataTable table){
			List<RepeatCharge> retVal=new List<RepeatCharge>();
			RepeatCharge repeatCharge;
			foreach(DataRow row in table.Rows) {
				repeatCharge=new RepeatCharge();
				repeatCharge.RepeatChargeNum= PIn.Long  (row["RepeatChargeNum"].ToString());
				repeatCharge.PatNum         = PIn.Long  (row["PatNum"].ToString());
				repeatCharge.ProcCode       = PIn.String(row["ProcCode"].ToString());
				repeatCharge.ChargeAmt      = PIn.Double(row["ChargeAmt"].ToString());
				repeatCharge.DateStart      = PIn.Date  (row["DateStart"].ToString());
				repeatCharge.DateStop       = PIn.Date  (row["DateStop"].ToString());
				repeatCharge.Note           = PIn.String(row["Note"].ToString());
				repeatCharge.CopyNoteToProc = PIn.Bool  (row["CopyNoteToProc"].ToString());
				repeatCharge.CreatesClaim   = PIn.Bool  (row["CreatesClaim"].ToString());
				repeatCharge.IsEnabled      = PIn.Bool  (row["IsEnabled"].ToString());
				retVal.Add(repeatCharge);
			}
			return retVal;
		}

		///<summary>Inserts one RepeatCharge into the database.  Returns the new priKey.</summary>
		public static long Insert(RepeatCharge repeatCharge){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				repeatCharge.RepeatChargeNum=DbHelper.GetNextOracleKey("repeatcharge","RepeatChargeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(repeatCharge,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							repeatCharge.RepeatChargeNum++;
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
				return Insert(repeatCharge,false);
			}
		}

		///<summary>Inserts one RepeatCharge into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(RepeatCharge repeatCharge,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				repeatCharge.RepeatChargeNum=ReplicationServers.GetKey("repeatcharge","RepeatChargeNum");
			}
			string command="INSERT INTO repeatcharge (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RepeatChargeNum,";
			}
			command+="PatNum,ProcCode,ChargeAmt,DateStart,DateStop,Note,CopyNoteToProc,CreatesClaim,IsEnabled) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(repeatCharge.RepeatChargeNum)+",";
			}
			command+=
				     POut.Long  (repeatCharge.PatNum)+","
				+"'"+POut.String(repeatCharge.ProcCode)+"',"
				+"'"+POut.Double(repeatCharge.ChargeAmt)+"',"
				+    POut.Date  (repeatCharge.DateStart)+","
				+    POut.Date  (repeatCharge.DateStop)+","
				+"'"+POut.String(repeatCharge.Note)+"',"
				+    POut.Bool  (repeatCharge.CopyNoteToProc)+","
				+    POut.Bool  (repeatCharge.CreatesClaim)+","
				+    POut.Bool  (repeatCharge.IsEnabled)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				repeatCharge.RepeatChargeNum=Db.NonQ(command,true);
			}
			return repeatCharge.RepeatChargeNum;
		}

		///<summary>Inserts one RepeatCharge into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RepeatCharge repeatCharge){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(repeatCharge,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					repeatCharge.RepeatChargeNum=DbHelper.GetNextOracleKey("repeatcharge","RepeatChargeNum"); //Cacheless method
				}
				return InsertNoCache(repeatCharge,true);
			}
		}

		///<summary>Inserts one RepeatCharge into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(RepeatCharge repeatCharge,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO repeatcharge (";
			if(!useExistingPK && isRandomKeys) {
				repeatCharge.RepeatChargeNum=ReplicationServers.GetKeyNoCache("repeatcharge","RepeatChargeNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="RepeatChargeNum,";
			}
			command+="PatNum,ProcCode,ChargeAmt,DateStart,DateStop,Note,CopyNoteToProc,CreatesClaim,IsEnabled) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(repeatCharge.RepeatChargeNum)+",";
			}
			command+=
				     POut.Long  (repeatCharge.PatNum)+","
				+"'"+POut.String(repeatCharge.ProcCode)+"',"
				+"'"+POut.Double(repeatCharge.ChargeAmt)+"',"
				+    POut.Date  (repeatCharge.DateStart)+","
				+    POut.Date  (repeatCharge.DateStop)+","
				+"'"+POut.String(repeatCharge.Note)+"',"
				+    POut.Bool  (repeatCharge.CopyNoteToProc)+","
				+    POut.Bool  (repeatCharge.CreatesClaim)+","
				+    POut.Bool  (repeatCharge.IsEnabled)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				repeatCharge.RepeatChargeNum=Db.NonQ(command,true);
			}
			return repeatCharge.RepeatChargeNum;
		}

		///<summary>Updates one RepeatCharge in the database.</summary>
		public static void Update(RepeatCharge repeatCharge){
			string command="UPDATE repeatcharge SET "
				+"PatNum         =  "+POut.Long  (repeatCharge.PatNum)+", "
				+"ProcCode       = '"+POut.String(repeatCharge.ProcCode)+"', "
				+"ChargeAmt      = '"+POut.Double(repeatCharge.ChargeAmt)+"', "
				+"DateStart      =  "+POut.Date  (repeatCharge.DateStart)+", "
				+"DateStop       =  "+POut.Date  (repeatCharge.DateStop)+", "
				+"Note           = '"+POut.String(repeatCharge.Note)+"', "
				+"CopyNoteToProc =  "+POut.Bool  (repeatCharge.CopyNoteToProc)+", "
				+"CreatesClaim   =  "+POut.Bool  (repeatCharge.CreatesClaim)+", "
				+"IsEnabled      =  "+POut.Bool  (repeatCharge.IsEnabled)+" "
				+"WHERE RepeatChargeNum = "+POut.Long(repeatCharge.RepeatChargeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one RepeatCharge in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(RepeatCharge repeatCharge,RepeatCharge oldRepeatCharge){
			string command="";
			if(repeatCharge.PatNum != oldRepeatCharge.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(repeatCharge.PatNum)+"";
			}
			if(repeatCharge.ProcCode != oldRepeatCharge.ProcCode) {
				if(command!=""){ command+=",";}
				command+="ProcCode = '"+POut.String(repeatCharge.ProcCode)+"'";
			}
			if(repeatCharge.ChargeAmt != oldRepeatCharge.ChargeAmt) {
				if(command!=""){ command+=",";}
				command+="ChargeAmt = '"+POut.Double(repeatCharge.ChargeAmt)+"'";
			}
			if(repeatCharge.DateStart.Date != oldRepeatCharge.DateStart.Date) {
				if(command!=""){ command+=",";}
				command+="DateStart = "+POut.Date(repeatCharge.DateStart)+"";
			}
			if(repeatCharge.DateStop.Date != oldRepeatCharge.DateStop.Date) {
				if(command!=""){ command+=",";}
				command+="DateStop = "+POut.Date(repeatCharge.DateStop)+"";
			}
			if(repeatCharge.Note != oldRepeatCharge.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(repeatCharge.Note)+"'";
			}
			if(repeatCharge.CopyNoteToProc != oldRepeatCharge.CopyNoteToProc) {
				if(command!=""){ command+=",";}
				command+="CopyNoteToProc = "+POut.Bool(repeatCharge.CopyNoteToProc)+"";
			}
			if(repeatCharge.CreatesClaim != oldRepeatCharge.CreatesClaim) {
				if(command!=""){ command+=",";}
				command+="CreatesClaim = "+POut.Bool(repeatCharge.CreatesClaim)+"";
			}
			if(repeatCharge.IsEnabled != oldRepeatCharge.IsEnabled) {
				if(command!=""){ command+=",";}
				command+="IsEnabled = "+POut.Bool(repeatCharge.IsEnabled)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE repeatcharge SET "+command
				+" WHERE RepeatChargeNum = "+POut.Long(repeatCharge.RepeatChargeNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one RepeatCharge from the database.</summary>
		public static void Delete(long repeatChargeNum){
			string command="DELETE FROM repeatcharge "
				+"WHERE RepeatChargeNum = "+POut.Long(repeatChargeNum);
			Db.NonQ(command);
		}

	}
}