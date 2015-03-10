//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MedLabResultCrud {
		///<summary>Gets one MedLabResult object from the database using the primary key.  Returns null if not found.</summary>
		public static MedLabResult SelectOne(long medLabResultNum){
			string command="SELECT * FROM medlabresult "
				+"WHERE MedLabResultNum = "+POut.Long(medLabResultNum);
			List<MedLabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MedLabResult object from the database using a query.</summary>
		public static MedLabResult SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MedLabResult objects from the database using a query.</summary>
		public static List<MedLabResult> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MedLabResult> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MedLabResult> TableToList(DataTable table){
			List<MedLabResult> retVal=new List<MedLabResult>();
			MedLabResult medLabResult;
			for(int i=0;i<table.Rows.Count;i++) {
				medLabResult=new MedLabResult();
				medLabResult.MedLabResultNum= PIn.Long  (table.Rows[i]["MedLabResultNum"].ToString());
				medLabResult.MedLabNum      = PIn.Long  (table.Rows[i]["MedLabNum"].ToString());
				medLabResult.ObsID          = PIn.String(table.Rows[i]["ObsID"].ToString());
				medLabResult.ObsText        = PIn.String(table.Rows[i]["ObsText"].ToString());
				medLabResult.ObsLoinc       = PIn.String(table.Rows[i]["ObsLoinc"].ToString());
				medLabResult.ObsLoincText   = PIn.String(table.Rows[i]["ObsLoincText"].ToString());
				medLabResult.ObsValue       = PIn.String(table.Rows[i]["ObsValue"].ToString());
				medLabResult.ObsUnits       = PIn.String(table.Rows[i]["ObsUnits"].ToString());
				medLabResult.ReferenceRange = PIn.String(table.Rows[i]["ReferenceRange"].ToString());
				string abnormalFlag=table.Rows[i]["AbnormalFlag"].ToString();
				if(abnormalFlag==""){
					medLabResult.AbnormalFlag =(AbnormalFlag)0;
				}
				else try{
					medLabResult.AbnormalFlag =(AbnormalFlag)Enum.Parse(typeof(AbnormalFlag),abnormalFlag);
				}
				catch{
					medLabResult.AbnormalFlag =(AbnormalFlag)0;
				}
				string resultStatus=table.Rows[i]["ResultStatus"].ToString();
				if(resultStatus==""){
					medLabResult.ResultStatus =(ResultStatus)0;
				}
				else try{
					medLabResult.ResultStatus =(ResultStatus)Enum.Parse(typeof(ResultStatus),resultStatus);
				}
				catch{
					medLabResult.ResultStatus =(ResultStatus)0;
				}
				medLabResult.DateTimeObs    = PIn.DateT (table.Rows[i]["DateTimeObs"].ToString());
				medLabResult.FacilityID     = PIn.String(table.Rows[i]["FacilityID"].ToString());
				medLabResult.DocNum         = PIn.Long  (table.Rows[i]["DocNum"].ToString());
				medLabResult.Note           = PIn.String(table.Rows[i]["Note"].ToString());
				retVal.Add(medLabResult);
			}
			return retVal;
		}

		///<summary>Inserts one MedLabResult into the database.  Returns the new priKey.</summary>
		public static long Insert(MedLabResult medLabResult){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				medLabResult.MedLabResultNum=DbHelper.GetNextOracleKey("medlabresult","MedLabResultNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(medLabResult,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							medLabResult.MedLabResultNum++;
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
				return Insert(medLabResult,false);
			}
		}

		///<summary>Inserts one MedLabResult into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MedLabResult medLabResult,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				medLabResult.MedLabResultNum=ReplicationServers.GetKey("medlabresult","MedLabResultNum");
			}
			string command="INSERT INTO medlabresult (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MedLabResultNum,";
			}
			command+="MedLabNum,ObsID,ObsText,ObsLoinc,ObsLoincText,ObsValue,ObsUnits,ReferenceRange,AbnormalFlag,ResultStatus,DateTimeObs,FacilityID,DocNum,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(medLabResult.MedLabResultNum)+",";
			}
			command+=
				     POut.Long  (medLabResult.MedLabNum)+","
				+"'"+POut.String(medLabResult.ObsID)+"',"
				+"'"+POut.String(medLabResult.ObsText)+"',"
				+"'"+POut.String(medLabResult.ObsLoinc)+"',"
				+"'"+POut.String(medLabResult.ObsLoincText)+"',"
				+"'"+POut.String(medLabResult.ObsValue)+"',"
				+"'"+POut.String(medLabResult.ObsUnits)+"',"
				+"'"+POut.String(medLabResult.ReferenceRange)+"',"
				+"'"+POut.String(medLabResult.AbnormalFlag.ToString())+"',"
				+"'"+POut.String(medLabResult.ResultStatus.ToString())+"',"
				+    POut.DateT (medLabResult.DateTimeObs)+","
				+"'"+POut.String(medLabResult.FacilityID)+"',"
				+    POut.Long  (medLabResult.DocNum)+","
				+    DbHelper.ParamChar+"paramNote)";
			if(medLabResult.Note==null) {
				medLabResult.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,medLabResult.Note);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramNote);
			}
			else {
				medLabResult.MedLabResultNum=Db.NonQ(command,true,paramNote);
			}
			return medLabResult.MedLabResultNum;
		}

		///<summary>Updates one MedLabResult in the database.</summary>
		public static void Update(MedLabResult medLabResult){
			string command="UPDATE medlabresult SET "
				+"MedLabNum      =  "+POut.Long  (medLabResult.MedLabNum)+", "
				+"ObsID          = '"+POut.String(medLabResult.ObsID)+"', "
				+"ObsText        = '"+POut.String(medLabResult.ObsText)+"', "
				+"ObsLoinc       = '"+POut.String(medLabResult.ObsLoinc)+"', "
				+"ObsLoincText   = '"+POut.String(medLabResult.ObsLoincText)+"', "
				+"ObsValue       = '"+POut.String(medLabResult.ObsValue)+"', "
				+"ObsUnits       = '"+POut.String(medLabResult.ObsUnits)+"', "
				+"ReferenceRange = '"+POut.String(medLabResult.ReferenceRange)+"', "
				+"AbnormalFlag   = '"+POut.String(medLabResult.AbnormalFlag.ToString())+"', "
				+"ResultStatus   = '"+POut.String(medLabResult.ResultStatus.ToString())+"', "
				+"DateTimeObs    =  "+POut.DateT (medLabResult.DateTimeObs)+", "
				+"FacilityID     = '"+POut.String(medLabResult.FacilityID)+"', "
				+"DocNum         =  "+POut.Long  (medLabResult.DocNum)+", "
				+"Note           =  "+DbHelper.ParamChar+"paramNote "
				+"WHERE MedLabResultNum = "+POut.Long(medLabResult.MedLabResultNum);
			if(medLabResult.Note==null) {
				medLabResult.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,medLabResult.Note);
			Db.NonQ(command,paramNote);
		}

		///<summary>Updates one MedLabResult in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MedLabResult medLabResult,MedLabResult oldMedLabResult){
			string command="";
			if(medLabResult.MedLabNum != oldMedLabResult.MedLabNum) {
				if(command!=""){ command+=",";}
				command+="MedLabNum = "+POut.Long(medLabResult.MedLabNum)+"";
			}
			if(medLabResult.ObsID != oldMedLabResult.ObsID) {
				if(command!=""){ command+=",";}
				command+="ObsID = '"+POut.String(medLabResult.ObsID)+"'";
			}
			if(medLabResult.ObsText != oldMedLabResult.ObsText) {
				if(command!=""){ command+=",";}
				command+="ObsText = '"+POut.String(medLabResult.ObsText)+"'";
			}
			if(medLabResult.ObsLoinc != oldMedLabResult.ObsLoinc) {
				if(command!=""){ command+=",";}
				command+="ObsLoinc = '"+POut.String(medLabResult.ObsLoinc)+"'";
			}
			if(medLabResult.ObsLoincText != oldMedLabResult.ObsLoincText) {
				if(command!=""){ command+=",";}
				command+="ObsLoincText = '"+POut.String(medLabResult.ObsLoincText)+"'";
			}
			if(medLabResult.ObsValue != oldMedLabResult.ObsValue) {
				if(command!=""){ command+=",";}
				command+="ObsValue = '"+POut.String(medLabResult.ObsValue)+"'";
			}
			if(medLabResult.ObsUnits != oldMedLabResult.ObsUnits) {
				if(command!=""){ command+=",";}
				command+="ObsUnits = '"+POut.String(medLabResult.ObsUnits)+"'";
			}
			if(medLabResult.ReferenceRange != oldMedLabResult.ReferenceRange) {
				if(command!=""){ command+=",";}
				command+="ReferenceRange = '"+POut.String(medLabResult.ReferenceRange)+"'";
			}
			if(medLabResult.AbnormalFlag != oldMedLabResult.AbnormalFlag) {
				if(command!=""){ command+=",";}
				command+="AbnormalFlag = '"+POut.String(medLabResult.AbnormalFlag.ToString())+"'";
			}
			if(medLabResult.ResultStatus != oldMedLabResult.ResultStatus) {
				if(command!=""){ command+=",";}
				command+="ResultStatus = '"+POut.String(medLabResult.ResultStatus.ToString())+"'";
			}
			if(medLabResult.DateTimeObs != oldMedLabResult.DateTimeObs) {
				if(command!=""){ command+=",";}
				command+="DateTimeObs = "+POut.DateT(medLabResult.DateTimeObs)+"";
			}
			if(medLabResult.FacilityID != oldMedLabResult.FacilityID) {
				if(command!=""){ command+=",";}
				command+="FacilityID = '"+POut.String(medLabResult.FacilityID)+"'";
			}
			if(medLabResult.DocNum != oldMedLabResult.DocNum) {
				if(command!=""){ command+=",";}
				command+="DocNum = "+POut.Long(medLabResult.DocNum)+"";
			}
			if(medLabResult.Note != oldMedLabResult.Note) {
				if(command!=""){ command+=",";}
				command+="Note = "+DbHelper.ParamChar+"paramNote";
			}
			if(command==""){
				return false;
			}
			if(medLabResult.Note==null) {
				medLabResult.Note="";
			}
			OdSqlParameter paramNote=new OdSqlParameter("paramNote",OdDbType.Text,medLabResult.Note);
			command="UPDATE medlabresult SET "+command
				+" WHERE MedLabResultNum = "+POut.Long(medLabResult.MedLabResultNum);
			Db.NonQ(command,paramNote);
			return true;
		}

		///<summary>Deletes one MedLabResult from the database.</summary>
		public static void Delete(long medLabResultNum){
			string command="DELETE FROM medlabresult "
				+"WHERE MedLabResultNum = "+POut.Long(medLabResultNum);
			Db.NonQ(command);
		}

	}
}