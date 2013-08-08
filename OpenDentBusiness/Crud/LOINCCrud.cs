//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class LOINCCrud {
		///<summary>Gets one LOINC object from the database using the primary key.  Returns null if not found.</summary>
		public static LOINC SelectOne(long lOINCNum){
			string command="SELECT * FROM loinc "
				+"WHERE LOINCNum = "+POut.Long(lOINCNum);
			List<LOINC> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LOINC object from the database using a query.</summary>
		public static LOINC SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LOINC> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LOINC objects from the database using a query.</summary>
		public static List<LOINC> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LOINC> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<LOINC> TableToList(DataTable table){
			List<LOINC> retVal=new List<LOINC>();
			LOINC lOINC;
			for(int i=0;i<table.Rows.Count;i++) {
				lOINC=new LOINC();
				lOINC.LOINCNum               = PIn.Long  (table.Rows[i]["LOINCNum"].ToString());
				lOINC.LOINCCode              = PIn.String(table.Rows[i]["LOINCCode"].ToString());
				lOINC.Component              = PIn.String(table.Rows[i]["Component"].ToString());
				lOINC.PropertyObserved       = PIn.String(table.Rows[i]["PropertyObserved"].ToString());
				lOINC.TimeAspct              = PIn.String(table.Rows[i]["TimeAspct"].ToString());
				lOINC.SystemMeasured         = PIn.String(table.Rows[i]["SystemMeasured"].ToString());
				lOINC.ScaleType              = PIn.String(table.Rows[i]["ScaleType"].ToString());
				lOINC.MethodType             = PIn.String(table.Rows[i]["MethodType"].ToString());
				lOINC.StatusOfCode           = PIn.String(table.Rows[i]["StatusOfCode"].ToString());
				lOINC.NameShort              = PIn.String(table.Rows[i]["NameShort"].ToString());
				lOINC.ClassType              = PIn.Int   (table.Rows[i]["ClassType"].ToString());
				lOINC.UnitsRequired          = PIn.Bool  (table.Rows[i]["UnitsRequired"].ToString());
				lOINC.OrderObs               = PIn.String(table.Rows[i]["OrderObs"].ToString());
				lOINC.HL7FieldSubfieldID     = PIn.String(table.Rows[i]["HL7FieldSubfieldID"].ToString());
				lOINC.ExternalCopyrightNotice= PIn.String(table.Rows[i]["ExternalCopyrightNotice"].ToString());
				lOINC.NameLongCommon         = PIn.String(table.Rows[i]["NameLongCommon"].ToString());
				lOINC.UnitsUCUM              = PIn.String(table.Rows[i]["UnitsUCUM"].ToString());
				lOINC.RankCommonTests        = PIn.Int   (table.Rows[i]["RankCommonTests"].ToString());
				lOINC.RankCommonOrders       = PIn.Int   (table.Rows[i]["RankCommonOrders"].ToString());
				retVal.Add(lOINC);
			}
			return retVal;
		}

		///<summary>Inserts one LOINC into the database.  Returns the new priKey.</summary>
		public static long Insert(LOINC lOINC){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				lOINC.LOINCNum=DbHelper.GetNextOracleKey("loinc","LOINCNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(lOINC,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							lOINC.LOINCNum++;
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
				return Insert(lOINC,false);
			}
		}

		///<summary>Inserts one LOINC into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(LOINC lOINC,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				lOINC.LOINCNum=ReplicationServers.GetKey("loinc","LOINCNum");
			}
			string command="INSERT INTO loinc (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LOINCNum,";
			}
			command+="LOINCCode,Component,PropertyObserved,TimeAspct,SystemMeasured,ScaleType,MethodType,StatusOfCode,NameShort,ClassType,UnitsRequired,OrderObs,HL7FieldSubfieldID,ExternalCopyrightNotice,NameLongCommon,UnitsUCUM,RankCommonTests,RankCommonOrders) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(lOINC.LOINCNum)+",";
			}
			command+=
				 "'"+POut.String(lOINC.LOINCCode)+"',"
				+"'"+POut.String(lOINC.Component)+"',"
				+"'"+POut.String(lOINC.PropertyObserved)+"',"
				+"'"+POut.String(lOINC.TimeAspct)+"',"
				+"'"+POut.String(lOINC.SystemMeasured)+"',"
				+"'"+POut.String(lOINC.ScaleType)+"',"
				+"'"+POut.String(lOINC.MethodType)+"',"
				+"'"+POut.String(lOINC.StatusOfCode)+"',"
				+"'"+POut.String(lOINC.NameShort)+"',"
				+    POut.Int   (lOINC.ClassType)+","
				+    POut.Bool  (lOINC.UnitsRequired)+","
				+"'"+POut.String(lOINC.OrderObs)+"',"
				+"'"+POut.String(lOINC.HL7FieldSubfieldID)+"',"
				+DbHelper.ParamChar+"paramExternalCopyrightNotice,"
				+"'"+POut.String(lOINC.NameLongCommon)+"',"
				+"'"+POut.String(lOINC.UnitsUCUM)+"',"
				+    POut.Int   (lOINC.RankCommonTests)+","
				+    POut.Int   (lOINC.RankCommonOrders)+")";
			if(lOINC.ExternalCopyrightNotice==null) {
				lOINC.ExternalCopyrightNotice="";
			}
			OdSqlParameter paramExternalCopyrightNotice=new OdSqlParameter("paramExternalCopyrightNotice",OdDbType.Text,lOINC.ExternalCopyrightNotice);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramExternalCopyrightNotice);
			}
			else {
				lOINC.LOINCNum=Db.NonQ(command,true,paramExternalCopyrightNotice);
			}
			return lOINC.LOINCNum;
		}

		///<summary>Updates one LOINC in the database.</summary>
		public static void Update(LOINC lOINC){
			string command="UPDATE loinc SET "
				+"LOINCCode              = '"+POut.String(lOINC.LOINCCode)+"', "
				+"Component              = '"+POut.String(lOINC.Component)+"', "
				+"PropertyObserved       = '"+POut.String(lOINC.PropertyObserved)+"', "
				+"TimeAspct              = '"+POut.String(lOINC.TimeAspct)+"', "
				+"SystemMeasured         = '"+POut.String(lOINC.SystemMeasured)+"', "
				+"ScaleType              = '"+POut.String(lOINC.ScaleType)+"', "
				+"MethodType             = '"+POut.String(lOINC.MethodType)+"', "
				+"StatusOfCode           = '"+POut.String(lOINC.StatusOfCode)+"', "
				+"NameShort              = '"+POut.String(lOINC.NameShort)+"', "
				+"ClassType              =  "+POut.Int   (lOINC.ClassType)+", "
				+"UnitsRequired          =  "+POut.Bool  (lOINC.UnitsRequired)+", "
				+"OrderObs               = '"+POut.String(lOINC.OrderObs)+"', "
				+"HL7FieldSubfieldID     = '"+POut.String(lOINC.HL7FieldSubfieldID)+"', "
				+"ExternalCopyrightNotice=  "+DbHelper.ParamChar+"paramExternalCopyrightNotice, "
				+"NameLongCommon         = '"+POut.String(lOINC.NameLongCommon)+"', "
				+"UnitsUCUM              = '"+POut.String(lOINC.UnitsUCUM)+"', "
				+"RankCommonTests        =  "+POut.Int   (lOINC.RankCommonTests)+", "
				+"RankCommonOrders       =  "+POut.Int   (lOINC.RankCommonOrders)+" "
				+"WHERE LOINCNum = "+POut.Long(lOINC.LOINCNum);
			if(lOINC.ExternalCopyrightNotice==null) {
				lOINC.ExternalCopyrightNotice="";
			}
			OdSqlParameter paramExternalCopyrightNotice=new OdSqlParameter("paramExternalCopyrightNotice",OdDbType.Text,lOINC.ExternalCopyrightNotice);
			Db.NonQ(command,paramExternalCopyrightNotice);
		}

		///<summary>Updates one LOINC in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(LOINC lOINC,LOINC oldLOINC){
			string command="";
			if(lOINC.LOINCCode != oldLOINC.LOINCCode) {
				if(command!=""){ command+=",";}
				command+="LOINCCode = '"+POut.String(lOINC.LOINCCode)+"'";
			}
			if(lOINC.Component != oldLOINC.Component) {
				if(command!=""){ command+=",";}
				command+="Component = '"+POut.String(lOINC.Component)+"'";
			}
			if(lOINC.PropertyObserved != oldLOINC.PropertyObserved) {
				if(command!=""){ command+=",";}
				command+="PropertyObserved = '"+POut.String(lOINC.PropertyObserved)+"'";
			}
			if(lOINC.TimeAspct != oldLOINC.TimeAspct) {
				if(command!=""){ command+=",";}
				command+="TimeAspct = '"+POut.String(lOINC.TimeAspct)+"'";
			}
			if(lOINC.SystemMeasured != oldLOINC.SystemMeasured) {
				if(command!=""){ command+=",";}
				command+="SystemMeasured = '"+POut.String(lOINC.SystemMeasured)+"'";
			}
			if(lOINC.ScaleType != oldLOINC.ScaleType) {
				if(command!=""){ command+=",";}
				command+="ScaleType = '"+POut.String(lOINC.ScaleType)+"'";
			}
			if(lOINC.MethodType != oldLOINC.MethodType) {
				if(command!=""){ command+=",";}
				command+="MethodType = '"+POut.String(lOINC.MethodType)+"'";
			}
			if(lOINC.StatusOfCode != oldLOINC.StatusOfCode) {
				if(command!=""){ command+=",";}
				command+="StatusOfCode = '"+POut.String(lOINC.StatusOfCode)+"'";
			}
			if(lOINC.NameShort != oldLOINC.NameShort) {
				if(command!=""){ command+=",";}
				command+="NameShort = '"+POut.String(lOINC.NameShort)+"'";
			}
			if(lOINC.ClassType != oldLOINC.ClassType) {
				if(command!=""){ command+=",";}
				command+="ClassType = "+POut.Int(lOINC.ClassType)+"";
			}
			if(lOINC.UnitsRequired != oldLOINC.UnitsRequired) {
				if(command!=""){ command+=",";}
				command+="UnitsRequired = "+POut.Bool(lOINC.UnitsRequired)+"";
			}
			if(lOINC.OrderObs != oldLOINC.OrderObs) {
				if(command!=""){ command+=",";}
				command+="OrderObs = '"+POut.String(lOINC.OrderObs)+"'";
			}
			if(lOINC.HL7FieldSubfieldID != oldLOINC.HL7FieldSubfieldID) {
				if(command!=""){ command+=",";}
				command+="HL7FieldSubfieldID = '"+POut.String(lOINC.HL7FieldSubfieldID)+"'";
			}
			if(lOINC.ExternalCopyrightNotice != oldLOINC.ExternalCopyrightNotice) {
				if(command!=""){ command+=",";}
				command+="ExternalCopyrightNotice = "+DbHelper.ParamChar+"paramExternalCopyrightNotice";
			}
			if(lOINC.NameLongCommon != oldLOINC.NameLongCommon) {
				if(command!=""){ command+=",";}
				command+="NameLongCommon = '"+POut.String(lOINC.NameLongCommon)+"'";
			}
			if(lOINC.UnitsUCUM != oldLOINC.UnitsUCUM) {
				if(command!=""){ command+=",";}
				command+="UnitsUCUM = '"+POut.String(lOINC.UnitsUCUM)+"'";
			}
			if(lOINC.RankCommonTests != oldLOINC.RankCommonTests) {
				if(command!=""){ command+=",";}
				command+="RankCommonTests = "+POut.Int(lOINC.RankCommonTests)+"";
			}
			if(lOINC.RankCommonOrders != oldLOINC.RankCommonOrders) {
				if(command!=""){ command+=",";}
				command+="RankCommonOrders = "+POut.Int(lOINC.RankCommonOrders)+"";
			}
			if(command==""){
				return;
			}
			if(lOINC.ExternalCopyrightNotice==null) {
				lOINC.ExternalCopyrightNotice="";
			}
			OdSqlParameter paramExternalCopyrightNotice=new OdSqlParameter("paramExternalCopyrightNotice",OdDbType.Text,lOINC.ExternalCopyrightNotice);
			command="UPDATE loinc SET "+command
				+" WHERE LOINCNum = "+POut.Long(lOINC.LOINCNum);
			Db.NonQ(command,paramExternalCopyrightNotice);
		}

		///<summary>Deletes one LOINC from the database.</summary>
		public static void Delete(long lOINCNum){
			string command="DELETE FROM loinc "
				+"WHERE LOINCNum = "+POut.Long(lOINCNum);
			Db.NonQ(command);
		}

	}
}