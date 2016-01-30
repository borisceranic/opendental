//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class DrugUnitCrud {
		///<summary>Gets one DrugUnit object from the database using the primary key.  Returns null if not found.</summary>
		public static DrugUnit SelectOne(long drugUnitNum){
			string command="SELECT * FROM drugunit "
				+"WHERE DrugUnitNum = "+POut.Long(drugUnitNum);
			List<DrugUnit> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one DrugUnit object from the database using a query.</summary>
		public static DrugUnit SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DrugUnit> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of DrugUnit objects from the database using a query.</summary>
		public static List<DrugUnit> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<DrugUnit> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<DrugUnit> TableToList(DataTable table){
			List<DrugUnit> retVal=new List<DrugUnit>();
			DrugUnit drugUnit;
			foreach(DataRow row in table.Rows) {
				drugUnit=new DrugUnit();
				drugUnit.DrugUnitNum   = PIn.Long  (row["DrugUnitNum"].ToString());
				drugUnit.UnitIdentifier= PIn.String(row["UnitIdentifier"].ToString());
				drugUnit.UnitText      = PIn.String(row["UnitText"].ToString());
				retVal.Add(drugUnit);
			}
			return retVal;
		}

		///<summary>Inserts one DrugUnit into the database.  Returns the new priKey.</summary>
		public static long Insert(DrugUnit drugUnit){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				drugUnit.DrugUnitNum=DbHelper.GetNextOracleKey("drugunit","DrugUnitNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(drugUnit,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							drugUnit.DrugUnitNum++;
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
				return Insert(drugUnit,false);
			}
		}

		///<summary>Inserts one DrugUnit into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(DrugUnit drugUnit,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				drugUnit.DrugUnitNum=ReplicationServers.GetKey("drugunit","DrugUnitNum");
			}
			string command="INSERT INTO drugunit (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="DrugUnitNum,";
			}
			command+="UnitIdentifier,UnitText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(drugUnit.DrugUnitNum)+",";
			}
			command+=
				 "'"+POut.String(drugUnit.UnitIdentifier)+"',"
				+"'"+POut.String(drugUnit.UnitText)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				drugUnit.DrugUnitNum=Db.NonQ(command,true);
			}
			return drugUnit.DrugUnitNum;
		}

		///<summary>Inserts one DrugUnit into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DrugUnit drugUnit){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(drugUnit,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					drugUnit.DrugUnitNum=DbHelper.GetNextOracleKey("drugunit","DrugUnitNum"); //Cacheless method
				}
				return InsertNoCache(drugUnit,true);
			}
		}

		///<summary>Inserts one DrugUnit into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(DrugUnit drugUnit,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO drugunit (";
			if(!useExistingPK && isRandomKeys) {
				drugUnit.DrugUnitNum=ReplicationServers.GetKeyNoCache("drugunit","DrugUnitNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="DrugUnitNum,";
			}
			command+="UnitIdentifier,UnitText) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(drugUnit.DrugUnitNum)+",";
			}
			command+=
				 "'"+POut.String(drugUnit.UnitIdentifier)+"',"
				+"'"+POut.String(drugUnit.UnitText)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				drugUnit.DrugUnitNum=Db.NonQ(command,true);
			}
			return drugUnit.DrugUnitNum;
		}

		///<summary>Updates one DrugUnit in the database.</summary>
		public static void Update(DrugUnit drugUnit){
			string command="UPDATE drugunit SET "
				+"UnitIdentifier= '"+POut.String(drugUnit.UnitIdentifier)+"', "
				+"UnitText      = '"+POut.String(drugUnit.UnitText)+"' "
				+"WHERE DrugUnitNum = "+POut.Long(drugUnit.DrugUnitNum);
			Db.NonQ(command);
		}

		///<summary>Updates one DrugUnit in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(DrugUnit drugUnit,DrugUnit oldDrugUnit){
			string command="";
			if(drugUnit.UnitIdentifier != oldDrugUnit.UnitIdentifier) {
				if(command!=""){ command+=",";}
				command+="UnitIdentifier = '"+POut.String(drugUnit.UnitIdentifier)+"'";
			}
			if(drugUnit.UnitText != oldDrugUnit.UnitText) {
				if(command!=""){ command+=",";}
				command+="UnitText = '"+POut.String(drugUnit.UnitText)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE drugunit SET "+command
				+" WHERE DrugUnitNum = "+POut.Long(drugUnit.DrugUnitNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(DrugUnit,DrugUnit) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(DrugUnit drugUnit,DrugUnit oldDrugUnit) {
			if(drugUnit.UnitIdentifier != oldDrugUnit.UnitIdentifier) {
				return true;
			}
			if(drugUnit.UnitText != oldDrugUnit.UnitText) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one DrugUnit from the database.</summary>
		public static void Delete(long drugUnitNum){
			string command="DELETE FROM drugunit "
				+"WHERE DrugUnitNum = "+POut.Long(drugUnitNum);
			Db.NonQ(command);
		}

	}
}