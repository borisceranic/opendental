//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EvaluationCriterionDefCrud {
		///<summary>Gets one EvaluationCriterionDef object from the database using the primary key.  Returns null if not found.</summary>
		public static EvaluationCriterionDef SelectOne(long evaluationCriterionDefNum){
			string command="SELECT * FROM evaluationcriteriondef "
				+"WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDefNum);
			List<EvaluationCriterionDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EvaluationCriterionDef object from the database using a query.</summary>
		public static EvaluationCriterionDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EvaluationCriterionDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EvaluationCriterionDef objects from the database using a query.</summary>
		public static List<EvaluationCriterionDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EvaluationCriterionDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EvaluationCriterionDef> TableToList(DataTable table){
			List<EvaluationCriterionDef> retVal=new List<EvaluationCriterionDef>();
			EvaluationCriterionDef evaluationCriterionDef;
			foreach(DataRow row in table.Rows) {
				evaluationCriterionDef=new EvaluationCriterionDef();
				evaluationCriterionDef.EvaluationCriterionDefNum= PIn.Long  (row["EvaluationCriterionDefNum"].ToString());
				evaluationCriterionDef.EvaluationDefNum         = PIn.Long  (row["EvaluationDefNum"].ToString());
				evaluationCriterionDef.CriterionDescript        = PIn.String(row["CriterionDescript"].ToString());
				evaluationCriterionDef.IsCategoryName           = PIn.Bool  (row["IsCategoryName"].ToString());
				evaluationCriterionDef.GradingScaleNum          = PIn.Long  (row["GradingScaleNum"].ToString());
				evaluationCriterionDef.ItemOrder                = PIn.Int   (row["ItemOrder"].ToString());
				evaluationCriterionDef.MaxPointsPoss            = PIn.Float (row["MaxPointsPoss"].ToString());
				retVal.Add(evaluationCriterionDef);
			}
			return retVal;
		}

		///<summary>Inserts one EvaluationCriterionDef into the database.  Returns the new priKey.</summary>
		public static long Insert(EvaluationCriterionDef evaluationCriterionDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				evaluationCriterionDef.EvaluationCriterionDefNum=DbHelper.GetNextOracleKey("evaluationcriteriondef","EvaluationCriterionDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(evaluationCriterionDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							evaluationCriterionDef.EvaluationCriterionDefNum++;
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
				return Insert(evaluationCriterionDef,false);
			}
		}

		///<summary>Inserts one EvaluationCriterionDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EvaluationCriterionDef evaluationCriterionDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				evaluationCriterionDef.EvaluationCriterionDefNum=ReplicationServers.GetKey("evaluationcriteriondef","EvaluationCriterionDefNum");
			}
			string command="INSERT INTO evaluationcriteriondef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EvaluationCriterionDefNum,";
			}
			command+="EvaluationDefNum,CriterionDescript,IsCategoryName,GradingScaleNum,ItemOrder,MaxPointsPoss) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(evaluationCriterionDef.EvaluationCriterionDefNum)+",";
			}
			command+=
				     POut.Long  (evaluationCriterionDef.EvaluationDefNum)+","
				+"'"+POut.String(evaluationCriterionDef.CriterionDescript)+"',"
				+    POut.Bool  (evaluationCriterionDef.IsCategoryName)+","
				+    POut.Long  (evaluationCriterionDef.GradingScaleNum)+","
				+    POut.Int   (evaluationCriterionDef.ItemOrder)+","
				+    POut.Float (evaluationCriterionDef.MaxPointsPoss)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				evaluationCriterionDef.EvaluationCriterionDefNum=Db.NonQ(command,true);
			}
			return evaluationCriterionDef.EvaluationCriterionDefNum;
		}

		///<summary>Inserts one EvaluationCriterionDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationCriterionDef evaluationCriterionDef){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(evaluationCriterionDef,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					evaluationCriterionDef.EvaluationCriterionDefNum=DbHelper.GetNextOracleKey("evaluationcriteriondef","EvaluationCriterionDefNum"); //Cacheless method
				}
				return InsertNoCache(evaluationCriterionDef,true);
			}
		}

		///<summary>Inserts one EvaluationCriterionDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EvaluationCriterionDef evaluationCriterionDef,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO evaluationcriteriondef (";
			if(!useExistingPK && isRandomKeys) {
				evaluationCriterionDef.EvaluationCriterionDefNum=ReplicationServers.GetKeyNoCache("evaluationcriteriondef","EvaluationCriterionDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EvaluationCriterionDefNum,";
			}
			command+="EvaluationDefNum,CriterionDescript,IsCategoryName,GradingScaleNum,ItemOrder,MaxPointsPoss) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(evaluationCriterionDef.EvaluationCriterionDefNum)+",";
			}
			command+=
				     POut.Long  (evaluationCriterionDef.EvaluationDefNum)+","
				+"'"+POut.String(evaluationCriterionDef.CriterionDescript)+"',"
				+    POut.Bool  (evaluationCriterionDef.IsCategoryName)+","
				+    POut.Long  (evaluationCriterionDef.GradingScaleNum)+","
				+    POut.Int   (evaluationCriterionDef.ItemOrder)+","
				+    POut.Float (evaluationCriterionDef.MaxPointsPoss)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				evaluationCriterionDef.EvaluationCriterionDefNum=Db.NonQ(command,true);
			}
			return evaluationCriterionDef.EvaluationCriterionDefNum;
		}

		///<summary>Updates one EvaluationCriterionDef in the database.</summary>
		public static void Update(EvaluationCriterionDef evaluationCriterionDef){
			string command="UPDATE evaluationcriteriondef SET "
				+"EvaluationDefNum         =  "+POut.Long  (evaluationCriterionDef.EvaluationDefNum)+", "
				+"CriterionDescript        = '"+POut.String(evaluationCriterionDef.CriterionDescript)+"', "
				+"IsCategoryName           =  "+POut.Bool  (evaluationCriterionDef.IsCategoryName)+", "
				+"GradingScaleNum          =  "+POut.Long  (evaluationCriterionDef.GradingScaleNum)+", "
				+"ItemOrder                =  "+POut.Int   (evaluationCriterionDef.ItemOrder)+", "
				+"MaxPointsPoss            =  "+POut.Float (evaluationCriterionDef.MaxPointsPoss)+" "
				+"WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDef.EvaluationCriterionDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EvaluationCriterionDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EvaluationCriterionDef evaluationCriterionDef,EvaluationCriterionDef oldEvaluationCriterionDef){
			string command="";
			if(evaluationCriterionDef.EvaluationDefNum != oldEvaluationCriterionDef.EvaluationDefNum) {
				if(command!=""){ command+=",";}
				command+="EvaluationDefNum = "+POut.Long(evaluationCriterionDef.EvaluationDefNum)+"";
			}
			if(evaluationCriterionDef.CriterionDescript != oldEvaluationCriterionDef.CriterionDescript) {
				if(command!=""){ command+=",";}
				command+="CriterionDescript = '"+POut.String(evaluationCriterionDef.CriterionDescript)+"'";
			}
			if(evaluationCriterionDef.IsCategoryName != oldEvaluationCriterionDef.IsCategoryName) {
				if(command!=""){ command+=",";}
				command+="IsCategoryName = "+POut.Bool(evaluationCriterionDef.IsCategoryName)+"";
			}
			if(evaluationCriterionDef.GradingScaleNum != oldEvaluationCriterionDef.GradingScaleNum) {
				if(command!=""){ command+=",";}
				command+="GradingScaleNum = "+POut.Long(evaluationCriterionDef.GradingScaleNum)+"";
			}
			if(evaluationCriterionDef.ItemOrder != oldEvaluationCriterionDef.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(evaluationCriterionDef.ItemOrder)+"";
			}
			if(evaluationCriterionDef.MaxPointsPoss != oldEvaluationCriterionDef.MaxPointsPoss) {
				if(command!=""){ command+=",";}
				command+="MaxPointsPoss = "+POut.Float(evaluationCriterionDef.MaxPointsPoss)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE evaluationcriteriondef SET "+command
				+" WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDef.EvaluationCriterionDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one EvaluationCriterionDef from the database.</summary>
		public static void Delete(long evaluationCriterionDefNum){
			string command="DELETE FROM evaluationcriteriondef "
				+"WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDefNum);
			Db.NonQ(command);
		}

	}
}