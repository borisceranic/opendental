//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class GradingScaleCrud {
		///<summary>Gets one GradingScale object from the database using the primary key.  Returns null if not found.</summary>
		public static GradingScale SelectOne(long gradingScaleNum){
			string command="SELECT * FROM gradingscale "
				+"WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			List<GradingScale> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one GradingScale object from the database using a query.</summary>
		public static GradingScale SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<GradingScale> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of GradingScale objects from the database using a query.</summary>
		public static List<GradingScale> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<GradingScale> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<GradingScale> TableToList(DataTable table){
			List<GradingScale> retVal=new List<GradingScale>();
			GradingScale gradingScale;
			for(int i=0;i<table.Rows.Count;i++) {
				gradingScale=new GradingScale();
				gradingScale.GradingScaleNum= PIn.Long  (table.Rows[i]["GradingScaleNum"].ToString());
				gradingScale.IsPercentage   = PIn.Bool  (table.Rows[i]["IsPercentage"].ToString());
				gradingScale.Description    = PIn.String(table.Rows[i]["Description"].ToString());
				retVal.Add(gradingScale);
			}
			return retVal;
		}

		///<summary>Inserts one GradingScale into the database.  Returns the new priKey.</summary>
		public static long Insert(GradingScale gradingScale){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				gradingScale.GradingScaleNum=DbHelper.GetNextOracleKey("gradingscale","GradingScaleNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(gradingScale,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							gradingScale.GradingScaleNum++;
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
				return Insert(gradingScale,false);
			}
		}

		///<summary>Inserts one GradingScale into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(GradingScale gradingScale,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				gradingScale.GradingScaleNum=ReplicationServers.GetKey("gradingscale","GradingScaleNum");
			}
			string command="INSERT INTO gradingscale (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="GradingScaleNum,";
			}
			command+="IsPercentage,Description) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(gradingScale.GradingScaleNum)+",";
			}
			command+=
				     POut.Bool  (gradingScale.IsPercentage)+","
				+"'"+POut.String(gradingScale.Description)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				gradingScale.GradingScaleNum=Db.NonQ(command,true);
			}
			return gradingScale.GradingScaleNum;
		}

		///<summary>Updates one GradingScale in the database.</summary>
		public static void Update(GradingScale gradingScale){
			string command="UPDATE gradingscale SET "
				+"IsPercentage   =  "+POut.Bool  (gradingScale.IsPercentage)+", "
				+"Description    = '"+POut.String(gradingScale.Description)+"' "
				+"WHERE GradingScaleNum = "+POut.Long(gradingScale.GradingScaleNum);
			Db.NonQ(command);
		}

		///<summary>Updates one GradingScale in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(GradingScale gradingScale,GradingScale oldGradingScale){
			string command="";
			if(gradingScale.IsPercentage != oldGradingScale.IsPercentage) {
				if(command!=""){ command+=",";}
				command+="IsPercentage = "+POut.Bool(gradingScale.IsPercentage)+"";
			}
			if(gradingScale.Description != oldGradingScale.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(gradingScale.Description)+"'";
			}
			if(command==""){
				return false;
			}
			command="UPDATE gradingscale SET "+command
				+" WHERE GradingScaleNum = "+POut.Long(gradingScale.GradingScaleNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one GradingScale from the database.</summary>
		public static void Delete(long gradingScaleNum){
			string command="DELETE FROM gradingscale "
				+"WHERE GradingScaleNum = "+POut.Long(gradingScaleNum);
			Db.NonQ(command);
		}

	}
}