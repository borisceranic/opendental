//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class OIDInternalCrud {
		///<summary>Gets one OIDInternal object from the database using the primary key.  Returns null if not found.</summary>
		public static OIDInternal SelectOne(long oIDInternalNum){
			string command="SELECT * FROM oidinternal "
				+"WHERE OIDInternalNum = "+POut.Long(oIDInternalNum);
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one OIDInternal object from the database using a query.</summary>
		public static OIDInternal SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of OIDInternal objects from the database using a query.</summary>
		public static List<OIDInternal> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<OIDInternal> TableToList(DataTable table){
			List<OIDInternal> retVal=new List<OIDInternal>();
			OIDInternal oIDInternal;
			for(int i=0;i<table.Rows.Count;i++) {
				oIDInternal=new OIDInternal();
				oIDInternal.OIDInternalNum= PIn.Long  (table.Rows[i]["OIDInternalNum"].ToString());
				string iDType=table.Rows[i]["IDType"].ToString();
				if(iDType==""){
					oIDInternal.IDType      =(IdentifierType)0;
				}
				else try{
					oIDInternal.IDType      =(IdentifierType)Enum.Parse(typeof(IdentifierType),iDType);
				}
				catch{
					oIDInternal.IDType      =(IdentifierType)0;
				}
				oIDInternal.IDRoot        = PIn.String(table.Rows[i]["IDRoot"].ToString());
				retVal.Add(oIDInternal);
			}
			return retVal;
		}

		///<summary>Inserts one OIDInternal into the database.  Returns the new priKey.</summary>
		public static long Insert(OIDInternal oIDInternal){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				oIDInternal.OIDInternalNum=DbHelper.GetNextOracleKey("oidinternal","OIDInternalNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(oIDInternal,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							oIDInternal.OIDInternalNum++;
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
				return Insert(oIDInternal,false);
			}
		}

		///<summary>Inserts one OIDInternal into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(OIDInternal oIDInternal,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				oIDInternal.OIDInternalNum=ReplicationServers.GetKey("oidinternal","OIDInternalNum");
			}
			string command="INSERT INTO oidinternal (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="OIDInternalNum,";
			}
			command+="IDType,IDRoot) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(oIDInternal.OIDInternalNum)+",";
			}
			command+=
				 "'"+POut.String(oIDInternal.IDType.ToString())+"',"
				+"'"+POut.String(oIDInternal.IDRoot)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				oIDInternal.OIDInternalNum=Db.NonQ(command,true);
			}
			return oIDInternal.OIDInternalNum;
		}

		///<summary>Inserts one OIDInternal into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OIDInternal oIDInternal){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(oIDInternal,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					oIDInternal.OIDInternalNum=DbHelper.GetNextOracleKey("oidinternal","OIDInternalNum"); //Cacheless method
				}
				return InsertNoCache(oIDInternal,true);
			}
		}

		///<summary>Inserts one OIDInternal into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OIDInternal oIDInternal,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO oidinternal (";
			if(!useExistingPK && isRandomKeys) {
				oIDInternal.OIDInternalNum=ReplicationServers.GetKeyNoCache("oidinternal","OIDInternalNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="OIDInternalNum,";
			}
			command+="IDType,IDRoot) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(oIDInternal.OIDInternalNum)+",";
			}
			command+=
				 "'"+POut.String(oIDInternal.IDType.ToString())+"',"
				+"'"+POut.String(oIDInternal.IDRoot)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				oIDInternal.OIDInternalNum=Db.NonQ(command,true);
			}
			return oIDInternal.OIDInternalNum;
		}

		///<summary>Updates one OIDInternal in the database.</summary>
		public static void Update(OIDInternal oIDInternal){
			string command="UPDATE oidinternal SET "
				+"IDType        = '"+POut.String(oIDInternal.IDType.ToString())+"', "
				+"IDRoot        = '"+POut.String(oIDInternal.IDRoot)+"' "
				+"WHERE OIDInternalNum = "+POut.Long(oIDInternal.OIDInternalNum);
			Db.NonQ(command);
		}

		///<summary>Updates one OIDInternal in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(OIDInternal oIDInternal,OIDInternal oldOIDInternal){
			string command="";
			if(oIDInternal.IDType != oldOIDInternal.IDType) {
				if(command!=""){ command+=",";}
				command+="IDType = '"+POut.String(oIDInternal.IDType.ToString())+"'";
			}
			if(oIDInternal.IDRoot != oldOIDInternal.IDRoot) {
				if(command!=""){ command+=",";}
				command+="IDRoot = '"+POut.String(oIDInternal.IDRoot)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE oidinternal SET "+command
				+" WHERE OIDInternalNum = "+POut.Long(oIDInternal.OIDInternalNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one OIDInternal from the database.</summary>
		public static void Delete(long oIDInternalNum){
			string command="DELETE FROM oidinternal "
				+"WHERE OIDInternalNum = "+POut.Long(oIDInternalNum);
			Db.NonQ(command);
		}

	}
}