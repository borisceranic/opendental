//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MedLabSpecimenCrud {
		///<summary>Gets one MedLabSpecimen object from the database using the primary key.  Returns null if not found.</summary>
		public static MedLabSpecimen SelectOne(long medLabSpecimenNum){
			string command="SELECT * FROM medlabspecimen "
				+"WHERE MedLabSpecimenNum = "+POut.Long(medLabSpecimenNum);
			List<MedLabSpecimen> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MedLabSpecimen object from the database using a query.</summary>
		public static MedLabSpecimen SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabSpecimen> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MedLabSpecimen objects from the database using a query.</summary>
		public static List<MedLabSpecimen> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabSpecimen> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MedLabSpecimen> TableToList(DataTable table){
			List<MedLabSpecimen> retVal=new List<MedLabSpecimen>();
			MedLabSpecimen medLabSpecimen;
			foreach(DataRow row in table.Rows) {
				medLabSpecimen=new MedLabSpecimen();
				medLabSpecimen.MedLabSpecimenNum= PIn.Long  (row["MedLabSpecimenNum"].ToString());
				medLabSpecimen.MedLabNum        = PIn.Long  (row["MedLabNum"].ToString());
				medLabSpecimen.SpecimenID       = PIn.String(row["SpecimenID"].ToString());
				medLabSpecimen.SpecimenDescript = PIn.String(row["SpecimenDescript"].ToString());
				medLabSpecimen.DateTimeCollected= PIn.DateT (row["DateTimeCollected"].ToString());
				retVal.Add(medLabSpecimen);
			}
			return retVal;
		}

		///<summary>Inserts one MedLabSpecimen into the database.  Returns the new priKey.</summary>
		public static long Insert(MedLabSpecimen medLabSpecimen){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				medLabSpecimen.MedLabSpecimenNum=DbHelper.GetNextOracleKey("medlabspecimen","MedLabSpecimenNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(medLabSpecimen,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							medLabSpecimen.MedLabSpecimenNum++;
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
				return Insert(medLabSpecimen,false);
			}
		}

		///<summary>Inserts one MedLabSpecimen into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MedLabSpecimen medLabSpecimen,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				medLabSpecimen.MedLabSpecimenNum=ReplicationServers.GetKey("medlabspecimen","MedLabSpecimenNum");
			}
			string command="INSERT INTO medlabspecimen (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MedLabSpecimenNum,";
			}
			command+="MedLabNum,SpecimenID,SpecimenDescript,DateTimeCollected) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(medLabSpecimen.MedLabSpecimenNum)+",";
			}
			command+=
				     POut.Long  (medLabSpecimen.MedLabNum)+","
				+"'"+POut.String(medLabSpecimen.SpecimenID)+"',"
				+"'"+POut.String(medLabSpecimen.SpecimenDescript)+"',"
				+    POut.DateT (medLabSpecimen.DateTimeCollected)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				medLabSpecimen.MedLabSpecimenNum=Db.NonQ(command,true);
			}
			return medLabSpecimen.MedLabSpecimenNum;
		}

		///<summary>Inserts one MedLabSpecimen into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MedLabSpecimen medLabSpecimen){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(medLabSpecimen,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					medLabSpecimen.MedLabSpecimenNum=DbHelper.GetNextOracleKey("medlabspecimen","MedLabSpecimenNum"); //Cacheless method
				}
				return InsertNoCache(medLabSpecimen,true);
			}
		}

		///<summary>Inserts one MedLabSpecimen into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MedLabSpecimen medLabSpecimen,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO medlabspecimen (";
			if(!useExistingPK && isRandomKeys) {
				medLabSpecimen.MedLabSpecimenNum=ReplicationServers.GetKeyNoCache("medlabspecimen","MedLabSpecimenNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MedLabSpecimenNum,";
			}
			command+="MedLabNum,SpecimenID,SpecimenDescript,DateTimeCollected) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(medLabSpecimen.MedLabSpecimenNum)+",";
			}
			command+=
				     POut.Long  (medLabSpecimen.MedLabNum)+","
				+"'"+POut.String(medLabSpecimen.SpecimenID)+"',"
				+"'"+POut.String(medLabSpecimen.SpecimenDescript)+"',"
				+    POut.DateT (medLabSpecimen.DateTimeCollected)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				medLabSpecimen.MedLabSpecimenNum=Db.NonQ(command,true);
			}
			return medLabSpecimen.MedLabSpecimenNum;
		}

		///<summary>Updates one MedLabSpecimen in the database.</summary>
		public static void Update(MedLabSpecimen medLabSpecimen){
			string command="UPDATE medlabspecimen SET "
				+"MedLabNum        =  "+POut.Long  (medLabSpecimen.MedLabNum)+", "
				+"SpecimenID       = '"+POut.String(medLabSpecimen.SpecimenID)+"', "
				+"SpecimenDescript = '"+POut.String(medLabSpecimen.SpecimenDescript)+"', "
				+"DateTimeCollected=  "+POut.DateT (medLabSpecimen.DateTimeCollected)+" "
				+"WHERE MedLabSpecimenNum = "+POut.Long(medLabSpecimen.MedLabSpecimenNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MedLabSpecimen in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MedLabSpecimen medLabSpecimen,MedLabSpecimen oldMedLabSpecimen){
			string command="";
			if(medLabSpecimen.MedLabNum != oldMedLabSpecimen.MedLabNum) {
				if(command!=""){ command+=",";}
				command+="MedLabNum = "+POut.Long(medLabSpecimen.MedLabNum)+"";
			}
			if(medLabSpecimen.SpecimenID != oldMedLabSpecimen.SpecimenID) {
				if(command!=""){ command+=",";}
				command+="SpecimenID = '"+POut.String(medLabSpecimen.SpecimenID)+"'";
			}
			if(medLabSpecimen.SpecimenDescript != oldMedLabSpecimen.SpecimenDescript) {
				if(command!=""){ command+=",";}
				command+="SpecimenDescript = '"+POut.String(medLabSpecimen.SpecimenDescript)+"'";
			}
			if(medLabSpecimen.DateTimeCollected != oldMedLabSpecimen.DateTimeCollected) {
				if(command!=""){ command+=",";}
				command+="DateTimeCollected = "+POut.DateT(medLabSpecimen.DateTimeCollected)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE medlabspecimen SET "+command
				+" WHERE MedLabSpecimenNum = "+POut.Long(medLabSpecimen.MedLabSpecimenNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(MedLabSpecimen,MedLabSpecimen) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(MedLabSpecimen medLabSpecimen,MedLabSpecimen oldMedLabSpecimen) {
			if(medLabSpecimen.MedLabNum != oldMedLabSpecimen.MedLabNum) {
				return true;
			}
			if(medLabSpecimen.SpecimenID != oldMedLabSpecimen.SpecimenID) {
				return true;
			}
			if(medLabSpecimen.SpecimenDescript != oldMedLabSpecimen.SpecimenDescript) {
				return true;
			}
			if(medLabSpecimen.DateTimeCollected != oldMedLabSpecimen.DateTimeCollected) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one MedLabSpecimen from the database.</summary>
		public static void Delete(long medLabSpecimenNum){
			string command="DELETE FROM medlabspecimen "
				+"WHERE MedLabSpecimenNum = "+POut.Long(medLabSpecimenNum);
			Db.NonQ(command);
		}

	}
}