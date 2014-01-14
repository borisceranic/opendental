//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using EhrLaboratories;

namespace OpenDentBusiness.Crud{
	public class EhrLabCrud {
		///<summary>Gets one EhrLab object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrLab SelectOne(long ehrLabNum){
			string command="SELECT * FROM ehrlab "
				+"WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			List<EhrLab> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrLab object from the database using a query.</summary>
		public static EhrLab SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLab> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrLab objects from the database using a query.</summary>
		public static List<EhrLab> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLab> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrLab> TableToList(DataTable table){
			List<EhrLab> retVal=new List<EhrLab>();
			EhrLab ehrLab;
			for(int i=0;i<table.Rows.Count;i++) {
				ehrLab=new EhrLab();
				ehrLab.EhrLabNum                                    = PIn.Long  (table.Rows[i]["EhrLabNum"].ToString());
				ehrLab.PatNum                                       = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				string orderControlCode=table.Rows[i]["OrderControlCode"].ToString();
				if(orderControlCode==""){
					ehrLab.OrderControlCode                           =(HL70119)0;
				}
				else try{
					ehrLab.OrderControlCode                           =(HL70119)Enum.Parse(typeof(HL70119),orderControlCode);
				}
				catch{
					ehrLab.OrderControlCode                           =(HL70119)0;
				}
				ehrLab.PlacerOrderNum                               = PIn.String(table.Rows[i]["PlacerOrderNum"].ToString());
				ehrLab.PlacerOrderNamespace                         = PIn.String(table.Rows[i]["PlacerOrderNamespace"].ToString());
				ehrLab.PlacerOrderUniversalID                       = PIn.String(table.Rows[i]["PlacerOrderUniversalID"].ToString());
				ehrLab.PlacerOrderUniversalIDType                   = PIn.String(table.Rows[i]["PlacerOrderUniversalIDType"].ToString());
				ehrLab.FillerOrderNum                               = PIn.String(table.Rows[i]["FillerOrderNum"].ToString());
				ehrLab.FillerOrderNamespace                         = PIn.String(table.Rows[i]["FillerOrderNamespace"].ToString());
				ehrLab.FillerOrderUniversalID                       = PIn.String(table.Rows[i]["FillerOrderUniversalID"].ToString());
				ehrLab.FillerOrderUniversalIDType                   = PIn.String(table.Rows[i]["FillerOrderUniversalIDType"].ToString());
				ehrLab.PlacerGroupNum                               = PIn.String(table.Rows[i]["PlacerGroupNum"].ToString());
				ehrLab.PlacerGroupNamespace                         = PIn.String(table.Rows[i]["PlacerGroupNamespace"].ToString());
				ehrLab.PlacerGroupUniversalID                       = PIn.String(table.Rows[i]["PlacerGroupUniversalID"].ToString());
				ehrLab.PlacerGroupUniversalIDType                   = PIn.String(table.Rows[i]["PlacerGroupUniversalIDType"].ToString());
				ehrLab.OrderingProviderID                           = PIn.String(table.Rows[i]["OrderingProviderID"].ToString());
				ehrLab.OrderingProviderLName                        = PIn.String(table.Rows[i]["OrderingProviderLName"].ToString());
				ehrLab.OrderingProviderFName                        = PIn.String(table.Rows[i]["OrderingProviderFName"].ToString());
				ehrLab.OrderingProviderMiddleNames                  = PIn.String(table.Rows[i]["OrderingProviderMiddleNames"].ToString());
				ehrLab.OrderingProviderSuffix                       = PIn.String(table.Rows[i]["OrderingProviderSuffix"].ToString());
				ehrLab.OrderingProviderPrefix                       = PIn.String(table.Rows[i]["OrderingProviderPrefix"].ToString());
				ehrLab.OrderingProviderAssigningAuthorityNamespaceID= PIn.String(table.Rows[i]["OrderingProviderAssigningAuthorityNamespaceID"].ToString());
				ehrLab.OrderingProviderAssigningAuthorityUniversalID= PIn.String(table.Rows[i]["OrderingProviderAssigningAuthorityUniversalID"].ToString());
				ehrLab.OrderingProviderAssigningAuthorityIDType     = PIn.String(table.Rows[i]["OrderingProviderAssigningAuthorityIDType"].ToString());
				string orderingProviderNameTypeCode=table.Rows[i]["OrderingProviderNameTypeCode"].ToString();
				if(orderingProviderNameTypeCode==""){
					ehrLab.OrderingProviderNameTypeCode               =(HL70200)0;
				}
				else try{
					ehrLab.OrderingProviderNameTypeCode               =(HL70200)Enum.Parse(typeof(HL70200),orderingProviderNameTypeCode);
				}
				catch{
					ehrLab.OrderingProviderNameTypeCode               =(HL70200)0;
				}
				string orderingProviderIdentifierTypeCode=table.Rows[i]["OrderingProviderIdentifierTypeCode"].ToString();
				if(orderingProviderIdentifierTypeCode==""){
					ehrLab.OrderingProviderIdentifierTypeCode         =(HL70203)0;
				}
				else try{
					ehrLab.OrderingProviderIdentifierTypeCode         =(HL70203)Enum.Parse(typeof(HL70203),orderingProviderIdentifierTypeCode);
				}
				catch{
					ehrLab.OrderingProviderIdentifierTypeCode         =(HL70203)0;
				}
				ehrLab.SetIdOBR                                     = PIn.Long  (table.Rows[i]["SetIdOBR"].ToString());
				ehrLab.UsiID                                        = PIn.String(table.Rows[i]["UsiID"].ToString());
				ehrLab.UsiText                                      = PIn.String(table.Rows[i]["UsiText"].ToString());
				ehrLab.UsiCodeSystemName                            = PIn.String(table.Rows[i]["UsiCodeSystemName"].ToString());
				ehrLab.UsiIDAlt                                     = PIn.String(table.Rows[i]["UsiIDAlt"].ToString());
				ehrLab.UsiTextAlt                                   = PIn.String(table.Rows[i]["UsiTextAlt"].ToString());
				ehrLab.UsiCodeSystemNameAlt                         = PIn.String(table.Rows[i]["UsiCodeSystemNameAlt"].ToString());
				ehrLab.UsiTextOriginal                              = PIn.String(table.Rows[i]["UsiTextOriginal"].ToString());
				ehrLab.ObservationDateTimeStart                     = PIn.String(table.Rows[i]["ObservationDateTimeStart"].ToString());
				ehrLab.ObservationDateTimeEnd                       = PIn.String(table.Rows[i]["ObservationDateTimeEnd"].ToString());
				string specimenActionCode=table.Rows[i]["SpecimenActionCode"].ToString();
				if(specimenActionCode==""){
					ehrLab.SpecimenActionCode                         =(HL70065)0;
				}
				else try{
					ehrLab.SpecimenActionCode                         =(HL70065)Enum.Parse(typeof(HL70065),specimenActionCode);
				}
				catch{
					ehrLab.SpecimenActionCode                         =(HL70065)0;
				}
				ehrLab.ResultDateTime                               = PIn.String(table.Rows[i]["ResultDateTime"].ToString());
				string resultStatus=table.Rows[i]["ResultStatus"].ToString();
				if(resultStatus==""){
					ehrLab.ResultStatus                               =(HL70123)0;
				}
				else try{
					ehrLab.ResultStatus                               =(HL70123)Enum.Parse(typeof(HL70123),resultStatus);
				}
				catch{
					ehrLab.ResultStatus                               =(HL70123)0;
				}
				ehrLab.ParentObservationID                          = PIn.String(table.Rows[i]["ParentObservationID"].ToString());
				ehrLab.ParentObservationText                        = PIn.String(table.Rows[i]["ParentObservationText"].ToString());
				ehrLab.ParentObservationCodeSystemName              = PIn.String(table.Rows[i]["ParentObservationCodeSystemName"].ToString());
				ehrLab.ParentObservationIDAlt                       = PIn.String(table.Rows[i]["ParentObservationIDAlt"].ToString());
				ehrLab.ParentObservationTextAlt                     = PIn.String(table.Rows[i]["ParentObservationTextAlt"].ToString());
				ehrLab.ParentObservationCodeSystemNameAlt           = PIn.String(table.Rows[i]["ParentObservationCodeSystemNameAlt"].ToString());
				ehrLab.ParentObservationTextOriginal                = PIn.String(table.Rows[i]["ParentObservationTextOriginal"].ToString());
				ehrLab.ParentObservationSubID                       = PIn.String(table.Rows[i]["ParentObservationSubID"].ToString());
				ehrLab.ParentPlacerOrderNum                         = PIn.String(table.Rows[i]["ParentPlacerOrderNum"].ToString());
				ehrLab.ParentPlacerOrderNamespace                   = PIn.String(table.Rows[i]["ParentPlacerOrderNamespace"].ToString());
				ehrLab.ParentPlacerOrderUniversalID                 = PIn.String(table.Rows[i]["ParentPlacerOrderUniversalID"].ToString());
				ehrLab.ParentPlacerOrderUniversalIDType             = PIn.String(table.Rows[i]["ParentPlacerOrderUniversalIDType"].ToString());
				ehrLab.ParentFillerOrderNum                         = PIn.String(table.Rows[i]["ParentFillerOrderNum"].ToString());
				ehrLab.ParentFillerOrderNamespace                   = PIn.String(table.Rows[i]["ParentFillerOrderNamespace"].ToString());
				ehrLab.ParentFillerOrderUniversalID                 = PIn.String(table.Rows[i]["ParentFillerOrderUniversalID"].ToString());
				ehrLab.ParentFillerOrderUniversalIDType             = PIn.String(table.Rows[i]["ParentFillerOrderUniversalIDType"].ToString());
				ehrLab.ListEhrLabResultsHandlingF                   = PIn.Bool  (table.Rows[i]["ListEhrLabResultsHandlingF"].ToString());
				ehrLab.ListEhrLabResultsHandlingN                   = PIn.Bool  (table.Rows[i]["ListEhrLabResultsHandlingN"].ToString());
				ehrLab.TQ1SetId                                     = PIn.Long  (table.Rows[i]["TQ1SetId"].ToString());
				ehrLab.TQ1DateTimeStart                             = PIn.String(table.Rows[i]["TQ1DateTimeStart"].ToString());
				ehrLab.TQ1DateTimeEnd                               = PIn.String(table.Rows[i]["TQ1DateTimeEnd"].ToString());
				retVal.Add(ehrLab);
			}
			return retVal;
		}

		///<summary>Inserts one EhrLab into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrLab ehrLab){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				ehrLab.EhrLabNum=DbHelper.GetNextOracleKey("ehrlab","EhrLabNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(ehrLab,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							ehrLab.EhrLabNum++;
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
				return Insert(ehrLab,false);
			}
		}

		///<summary>Inserts one EhrLab into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrLab ehrLab,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrLab.EhrLabNum=ReplicationServers.GetKey("ehrlab","EhrLabNum");
			}
			string command="INSERT INTO ehrlab (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrLabNum,";
			}
			command+="PatNum,OrderControlCode,PlacerOrderNum,PlacerOrderNamespace,PlacerOrderUniversalID,PlacerOrderUniversalIDType,FillerOrderNum,FillerOrderNamespace,FillerOrderUniversalID,FillerOrderUniversalIDType,PlacerGroupNum,PlacerGroupNamespace,PlacerGroupUniversalID,PlacerGroupUniversalIDType,OrderingProviderID,OrderingProviderLName,OrderingProviderFName,OrderingProviderMiddleNames,OrderingProviderSuffix,OrderingProviderPrefix,OrderingProviderAssigningAuthorityNamespaceID,OrderingProviderAssigningAuthorityUniversalID,OrderingProviderAssigningAuthorityIDType,OrderingProviderNameTypeCode,OrderingProviderIdentifierTypeCode,SetIdOBR,UsiID,UsiText,UsiCodeSystemName,UsiIDAlt,UsiTextAlt,UsiCodeSystemNameAlt,UsiTextOriginal,ObservationDateTimeStart,ObservationDateTimeEnd,SpecimenActionCode,ResultDateTime,ResultStatus,ParentObservationID,ParentObservationText,ParentObservationCodeSystemName,ParentObservationIDAlt,ParentObservationTextAlt,ParentObservationCodeSystemNameAlt,ParentObservationTextOriginal,ParentObservationSubID,ParentPlacerOrderNum,ParentPlacerOrderNamespace,ParentPlacerOrderUniversalID,ParentPlacerOrderUniversalIDType,ParentFillerOrderNum,ParentFillerOrderNamespace,ParentFillerOrderUniversalID,ParentFillerOrderUniversalIDType,ListEhrLabResultsHandlingF,ListEhrLabResultsHandlingN,TQ1SetId,TQ1DateTimeStart,TQ1DateTimeEnd) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrLab.EhrLabNum)+",";
			}
			command+=
				     POut.Long  (ehrLab.PatNum)+","
				+"'"+POut.String(ehrLab.OrderControlCode.ToString())+"',"
				+"'"+POut.String(ehrLab.PlacerOrderNum)+"',"
				+"'"+POut.String(ehrLab.PlacerOrderNamespace)+"',"
				+"'"+POut.String(ehrLab.PlacerOrderUniversalID)+"',"
				+"'"+POut.String(ehrLab.PlacerOrderUniversalIDType)+"',"
				+"'"+POut.String(ehrLab.FillerOrderNum)+"',"
				+"'"+POut.String(ehrLab.FillerOrderNamespace)+"',"
				+"'"+POut.String(ehrLab.FillerOrderUniversalID)+"',"
				+"'"+POut.String(ehrLab.FillerOrderUniversalIDType)+"',"
				+"'"+POut.String(ehrLab.PlacerGroupNum)+"',"
				+"'"+POut.String(ehrLab.PlacerGroupNamespace)+"',"
				+"'"+POut.String(ehrLab.PlacerGroupUniversalID)+"',"
				+"'"+POut.String(ehrLab.PlacerGroupUniversalIDType)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderID)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderLName)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderFName)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderMiddleNames)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderSuffix)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderPrefix)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderAssigningAuthorityNamespaceID)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderAssigningAuthorityUniversalID)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderAssigningAuthorityIDType)+"',"
				+"'"+POut.String(ehrLab.OrderingProviderNameTypeCode.ToString())+"',"
				+"'"+POut.String(ehrLab.OrderingProviderIdentifierTypeCode.ToString())+"',"
				+    POut.Long  (ehrLab.SetIdOBR)+","
				+"'"+POut.String(ehrLab.UsiID)+"',"
				+"'"+POut.String(ehrLab.UsiText)+"',"
				+"'"+POut.String(ehrLab.UsiCodeSystemName)+"',"
				+"'"+POut.String(ehrLab.UsiIDAlt)+"',"
				+"'"+POut.String(ehrLab.UsiTextAlt)+"',"
				+"'"+POut.String(ehrLab.UsiCodeSystemNameAlt)+"',"
				+"'"+POut.String(ehrLab.UsiTextOriginal)+"',"
				+"'"+POut.String(ehrLab.ObservationDateTimeStart)+"',"
				+"'"+POut.String(ehrLab.ObservationDateTimeEnd)+"',"
				+"'"+POut.String(ehrLab.SpecimenActionCode.ToString())+"',"
				+"'"+POut.String(ehrLab.ResultDateTime)+"',"
				+"'"+POut.String(ehrLab.ResultStatus.ToString())+"',"
				+"'"+POut.String(ehrLab.ParentObservationID)+"',"
				+"'"+POut.String(ehrLab.ParentObservationText)+"',"
				+"'"+POut.String(ehrLab.ParentObservationCodeSystemName)+"',"
				+"'"+POut.String(ehrLab.ParentObservationIDAlt)+"',"
				+"'"+POut.String(ehrLab.ParentObservationTextAlt)+"',"
				+"'"+POut.String(ehrLab.ParentObservationCodeSystemNameAlt)+"',"
				+"'"+POut.String(ehrLab.ParentObservationTextOriginal)+"',"
				+"'"+POut.String(ehrLab.ParentObservationSubID)+"',"
				+"'"+POut.String(ehrLab.ParentPlacerOrderNum)+"',"
				+"'"+POut.String(ehrLab.ParentPlacerOrderNamespace)+"',"
				+"'"+POut.String(ehrLab.ParentPlacerOrderUniversalID)+"',"
				+"'"+POut.String(ehrLab.ParentPlacerOrderUniversalIDType)+"',"
				+"'"+POut.String(ehrLab.ParentFillerOrderNum)+"',"
				+"'"+POut.String(ehrLab.ParentFillerOrderNamespace)+"',"
				+"'"+POut.String(ehrLab.ParentFillerOrderUniversalID)+"',"
				+"'"+POut.String(ehrLab.ParentFillerOrderUniversalIDType)+"',"
				+    POut.Bool  (ehrLab.ListEhrLabResultsHandlingF)+","
				+    POut.Bool  (ehrLab.ListEhrLabResultsHandlingN)+","
				+    POut.Long  (ehrLab.TQ1SetId)+","
				+"'"+POut.String(ehrLab.TQ1DateTimeStart)+"',"
				+"'"+POut.String(ehrLab.TQ1DateTimeEnd)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrLab.EhrLabNum=Db.NonQ(command,true);
			}
			return ehrLab.EhrLabNum;
		}

		///<summary>Updates one EhrLab in the database.</summary>
		public static void Update(EhrLab ehrLab){
			string command="UPDATE ehrlab SET "
				+"PatNum                                       =  "+POut.Long  (ehrLab.PatNum)+", "
				+"OrderControlCode                             = '"+POut.String(ehrLab.OrderControlCode.ToString())+"', "
				+"PlacerOrderNum                               = '"+POut.String(ehrLab.PlacerOrderNum)+"', "
				+"PlacerOrderNamespace                         = '"+POut.String(ehrLab.PlacerOrderNamespace)+"', "
				+"PlacerOrderUniversalID                       = '"+POut.String(ehrLab.PlacerOrderUniversalID)+"', "
				+"PlacerOrderUniversalIDType                   = '"+POut.String(ehrLab.PlacerOrderUniversalIDType)+"', "
				+"FillerOrderNum                               = '"+POut.String(ehrLab.FillerOrderNum)+"', "
				+"FillerOrderNamespace                         = '"+POut.String(ehrLab.FillerOrderNamespace)+"', "
				+"FillerOrderUniversalID                       = '"+POut.String(ehrLab.FillerOrderUniversalID)+"', "
				+"FillerOrderUniversalIDType                   = '"+POut.String(ehrLab.FillerOrderUniversalIDType)+"', "
				+"PlacerGroupNum                               = '"+POut.String(ehrLab.PlacerGroupNum)+"', "
				+"PlacerGroupNamespace                         = '"+POut.String(ehrLab.PlacerGroupNamespace)+"', "
				+"PlacerGroupUniversalID                       = '"+POut.String(ehrLab.PlacerGroupUniversalID)+"', "
				+"PlacerGroupUniversalIDType                   = '"+POut.String(ehrLab.PlacerGroupUniversalIDType)+"', "
				+"OrderingProviderID                           = '"+POut.String(ehrLab.OrderingProviderID)+"', "
				+"OrderingProviderLName                        = '"+POut.String(ehrLab.OrderingProviderLName)+"', "
				+"OrderingProviderFName                        = '"+POut.String(ehrLab.OrderingProviderFName)+"', "
				+"OrderingProviderMiddleNames                  = '"+POut.String(ehrLab.OrderingProviderMiddleNames)+"', "
				+"OrderingProviderSuffix                       = '"+POut.String(ehrLab.OrderingProviderSuffix)+"', "
				+"OrderingProviderPrefix                       = '"+POut.String(ehrLab.OrderingProviderPrefix)+"', "
				+"OrderingProviderAssigningAuthorityNamespaceID= '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityNamespaceID)+"', "
				+"OrderingProviderAssigningAuthorityUniversalID= '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityUniversalID)+"', "
				+"OrderingProviderAssigningAuthorityIDType     = '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityIDType)+"', "
				+"OrderingProviderNameTypeCode                 = '"+POut.String(ehrLab.OrderingProviderNameTypeCode.ToString())+"', "
				+"OrderingProviderIdentifierTypeCode           = '"+POut.String(ehrLab.OrderingProviderIdentifierTypeCode.ToString())+"', "
				+"SetIdOBR                                     =  "+POut.Long  (ehrLab.SetIdOBR)+", "
				+"UsiID                                        = '"+POut.String(ehrLab.UsiID)+"', "
				+"UsiText                                      = '"+POut.String(ehrLab.UsiText)+"', "
				+"UsiCodeSystemName                            = '"+POut.String(ehrLab.UsiCodeSystemName)+"', "
				+"UsiIDAlt                                     = '"+POut.String(ehrLab.UsiIDAlt)+"', "
				+"UsiTextAlt                                   = '"+POut.String(ehrLab.UsiTextAlt)+"', "
				+"UsiCodeSystemNameAlt                         = '"+POut.String(ehrLab.UsiCodeSystemNameAlt)+"', "
				+"UsiTextOriginal                              = '"+POut.String(ehrLab.UsiTextOriginal)+"', "
				+"ObservationDateTimeStart                     = '"+POut.String(ehrLab.ObservationDateTimeStart)+"', "
				+"ObservationDateTimeEnd                       = '"+POut.String(ehrLab.ObservationDateTimeEnd)+"', "
				+"SpecimenActionCode                           = '"+POut.String(ehrLab.SpecimenActionCode.ToString())+"', "
				+"ResultDateTime                               = '"+POut.String(ehrLab.ResultDateTime)+"', "
				+"ResultStatus                                 = '"+POut.String(ehrLab.ResultStatus.ToString())+"', "
				+"ParentObservationID                          = '"+POut.String(ehrLab.ParentObservationID)+"', "
				+"ParentObservationText                        = '"+POut.String(ehrLab.ParentObservationText)+"', "
				+"ParentObservationCodeSystemName              = '"+POut.String(ehrLab.ParentObservationCodeSystemName)+"', "
				+"ParentObservationIDAlt                       = '"+POut.String(ehrLab.ParentObservationIDAlt)+"', "
				+"ParentObservationTextAlt                     = '"+POut.String(ehrLab.ParentObservationTextAlt)+"', "
				+"ParentObservationCodeSystemNameAlt           = '"+POut.String(ehrLab.ParentObservationCodeSystemNameAlt)+"', "
				+"ParentObservationTextOriginal                = '"+POut.String(ehrLab.ParentObservationTextOriginal)+"', "
				+"ParentObservationSubID                       = '"+POut.String(ehrLab.ParentObservationSubID)+"', "
				+"ParentPlacerOrderNum                         = '"+POut.String(ehrLab.ParentPlacerOrderNum)+"', "
				+"ParentPlacerOrderNamespace                   = '"+POut.String(ehrLab.ParentPlacerOrderNamespace)+"', "
				+"ParentPlacerOrderUniversalID                 = '"+POut.String(ehrLab.ParentPlacerOrderUniversalID)+"', "
				+"ParentPlacerOrderUniversalIDType             = '"+POut.String(ehrLab.ParentPlacerOrderUniversalIDType)+"', "
				+"ParentFillerOrderNum                         = '"+POut.String(ehrLab.ParentFillerOrderNum)+"', "
				+"ParentFillerOrderNamespace                   = '"+POut.String(ehrLab.ParentFillerOrderNamespace)+"', "
				+"ParentFillerOrderUniversalID                 = '"+POut.String(ehrLab.ParentFillerOrderUniversalID)+"', "
				+"ParentFillerOrderUniversalIDType             = '"+POut.String(ehrLab.ParentFillerOrderUniversalIDType)+"', "
				+"ListEhrLabResultsHandlingF                   =  "+POut.Bool  (ehrLab.ListEhrLabResultsHandlingF)+", "
				+"ListEhrLabResultsHandlingN                   =  "+POut.Bool  (ehrLab.ListEhrLabResultsHandlingN)+", "
				+"TQ1SetId                                     =  "+POut.Long  (ehrLab.TQ1SetId)+", "
				+"TQ1DateTimeStart                             = '"+POut.String(ehrLab.TQ1DateTimeStart)+"', "
				+"TQ1DateTimeEnd                               = '"+POut.String(ehrLab.TQ1DateTimeEnd)+"' "
				+"WHERE EhrLabNum = "+POut.Long(ehrLab.EhrLabNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrLab in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(EhrLab ehrLab,EhrLab oldEhrLab){
			string command="";
			if(ehrLab.PatNum != oldEhrLab.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(ehrLab.PatNum)+"";
			}
			if(ehrLab.OrderControlCode != oldEhrLab.OrderControlCode) {
				if(command!=""){ command+=",";}
				command+="OrderControlCode = '"+POut.String(ehrLab.OrderControlCode.ToString())+"'";
			}
			if(ehrLab.PlacerOrderNum != oldEhrLab.PlacerOrderNum) {
				if(command!=""){ command+=",";}
				command+="PlacerOrderNum = '"+POut.String(ehrLab.PlacerOrderNum)+"'";
			}
			if(ehrLab.PlacerOrderNamespace != oldEhrLab.PlacerOrderNamespace) {
				if(command!=""){ command+=",";}
				command+="PlacerOrderNamespace = '"+POut.String(ehrLab.PlacerOrderNamespace)+"'";
			}
			if(ehrLab.PlacerOrderUniversalID != oldEhrLab.PlacerOrderUniversalID) {
				if(command!=""){ command+=",";}
				command+="PlacerOrderUniversalID = '"+POut.String(ehrLab.PlacerOrderUniversalID)+"'";
			}
			if(ehrLab.PlacerOrderUniversalIDType != oldEhrLab.PlacerOrderUniversalIDType) {
				if(command!=""){ command+=",";}
				command+="PlacerOrderUniversalIDType = '"+POut.String(ehrLab.PlacerOrderUniversalIDType)+"'";
			}
			if(ehrLab.FillerOrderNum != oldEhrLab.FillerOrderNum) {
				if(command!=""){ command+=",";}
				command+="FillerOrderNum = '"+POut.String(ehrLab.FillerOrderNum)+"'";
			}
			if(ehrLab.FillerOrderNamespace != oldEhrLab.FillerOrderNamespace) {
				if(command!=""){ command+=",";}
				command+="FillerOrderNamespace = '"+POut.String(ehrLab.FillerOrderNamespace)+"'";
			}
			if(ehrLab.FillerOrderUniversalID != oldEhrLab.FillerOrderUniversalID) {
				if(command!=""){ command+=",";}
				command+="FillerOrderUniversalID = '"+POut.String(ehrLab.FillerOrderUniversalID)+"'";
			}
			if(ehrLab.FillerOrderUniversalIDType != oldEhrLab.FillerOrderUniversalIDType) {
				if(command!=""){ command+=",";}
				command+="FillerOrderUniversalIDType = '"+POut.String(ehrLab.FillerOrderUniversalIDType)+"'";
			}
			if(ehrLab.PlacerGroupNum != oldEhrLab.PlacerGroupNum) {
				if(command!=""){ command+=",";}
				command+="PlacerGroupNum = '"+POut.String(ehrLab.PlacerGroupNum)+"'";
			}
			if(ehrLab.PlacerGroupNamespace != oldEhrLab.PlacerGroupNamespace) {
				if(command!=""){ command+=",";}
				command+="PlacerGroupNamespace = '"+POut.String(ehrLab.PlacerGroupNamespace)+"'";
			}
			if(ehrLab.PlacerGroupUniversalID != oldEhrLab.PlacerGroupUniversalID) {
				if(command!=""){ command+=",";}
				command+="PlacerGroupUniversalID = '"+POut.String(ehrLab.PlacerGroupUniversalID)+"'";
			}
			if(ehrLab.PlacerGroupUniversalIDType != oldEhrLab.PlacerGroupUniversalIDType) {
				if(command!=""){ command+=",";}
				command+="PlacerGroupUniversalIDType = '"+POut.String(ehrLab.PlacerGroupUniversalIDType)+"'";
			}
			if(ehrLab.OrderingProviderID != oldEhrLab.OrderingProviderID) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderID = '"+POut.String(ehrLab.OrderingProviderID)+"'";
			}
			if(ehrLab.OrderingProviderLName != oldEhrLab.OrderingProviderLName) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderLName = '"+POut.String(ehrLab.OrderingProviderLName)+"'";
			}
			if(ehrLab.OrderingProviderFName != oldEhrLab.OrderingProviderFName) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderFName = '"+POut.String(ehrLab.OrderingProviderFName)+"'";
			}
			if(ehrLab.OrderingProviderMiddleNames != oldEhrLab.OrderingProviderMiddleNames) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderMiddleNames = '"+POut.String(ehrLab.OrderingProviderMiddleNames)+"'";
			}
			if(ehrLab.OrderingProviderSuffix != oldEhrLab.OrderingProviderSuffix) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderSuffix = '"+POut.String(ehrLab.OrderingProviderSuffix)+"'";
			}
			if(ehrLab.OrderingProviderPrefix != oldEhrLab.OrderingProviderPrefix) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderPrefix = '"+POut.String(ehrLab.OrderingProviderPrefix)+"'";
			}
			if(ehrLab.OrderingProviderAssigningAuthorityNamespaceID != oldEhrLab.OrderingProviderAssigningAuthorityNamespaceID) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderAssigningAuthorityNamespaceID = '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityNamespaceID)+"'";
			}
			if(ehrLab.OrderingProviderAssigningAuthorityUniversalID != oldEhrLab.OrderingProviderAssigningAuthorityUniversalID) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderAssigningAuthorityUniversalID = '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityUniversalID)+"'";
			}
			if(ehrLab.OrderingProviderAssigningAuthorityIDType != oldEhrLab.OrderingProviderAssigningAuthorityIDType) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderAssigningAuthorityIDType = '"+POut.String(ehrLab.OrderingProviderAssigningAuthorityIDType)+"'";
			}
			if(ehrLab.OrderingProviderNameTypeCode != oldEhrLab.OrderingProviderNameTypeCode) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderNameTypeCode = '"+POut.String(ehrLab.OrderingProviderNameTypeCode.ToString())+"'";
			}
			if(ehrLab.OrderingProviderIdentifierTypeCode != oldEhrLab.OrderingProviderIdentifierTypeCode) {
				if(command!=""){ command+=",";}
				command+="OrderingProviderIdentifierTypeCode = '"+POut.String(ehrLab.OrderingProviderIdentifierTypeCode.ToString())+"'";
			}
			if(ehrLab.SetIdOBR != oldEhrLab.SetIdOBR) {
				if(command!=""){ command+=",";}
				command+="SetIdOBR = "+POut.Long(ehrLab.SetIdOBR)+"";
			}
			if(ehrLab.UsiID != oldEhrLab.UsiID) {
				if(command!=""){ command+=",";}
				command+="UsiID = '"+POut.String(ehrLab.UsiID)+"'";
			}
			if(ehrLab.UsiText != oldEhrLab.UsiText) {
				if(command!=""){ command+=",";}
				command+="UsiText = '"+POut.String(ehrLab.UsiText)+"'";
			}
			if(ehrLab.UsiCodeSystemName != oldEhrLab.UsiCodeSystemName) {
				if(command!=""){ command+=",";}
				command+="UsiCodeSystemName = '"+POut.String(ehrLab.UsiCodeSystemName)+"'";
			}
			if(ehrLab.UsiIDAlt != oldEhrLab.UsiIDAlt) {
				if(command!=""){ command+=",";}
				command+="UsiIDAlt = '"+POut.String(ehrLab.UsiIDAlt)+"'";
			}
			if(ehrLab.UsiTextAlt != oldEhrLab.UsiTextAlt) {
				if(command!=""){ command+=",";}
				command+="UsiTextAlt = '"+POut.String(ehrLab.UsiTextAlt)+"'";
			}
			if(ehrLab.UsiCodeSystemNameAlt != oldEhrLab.UsiCodeSystemNameAlt) {
				if(command!=""){ command+=",";}
				command+="UsiCodeSystemNameAlt = '"+POut.String(ehrLab.UsiCodeSystemNameAlt)+"'";
			}
			if(ehrLab.UsiTextOriginal != oldEhrLab.UsiTextOriginal) {
				if(command!=""){ command+=",";}
				command+="UsiTextOriginal = '"+POut.String(ehrLab.UsiTextOriginal)+"'";
			}
			if(ehrLab.ObservationDateTimeStart != oldEhrLab.ObservationDateTimeStart) {
				if(command!=""){ command+=",";}
				command+="ObservationDateTimeStart = '"+POut.String(ehrLab.ObservationDateTimeStart)+"'";
			}
			if(ehrLab.ObservationDateTimeEnd != oldEhrLab.ObservationDateTimeEnd) {
				if(command!=""){ command+=",";}
				command+="ObservationDateTimeEnd = '"+POut.String(ehrLab.ObservationDateTimeEnd)+"'";
			}
			if(ehrLab.SpecimenActionCode != oldEhrLab.SpecimenActionCode) {
				if(command!=""){ command+=",";}
				command+="SpecimenActionCode = '"+POut.String(ehrLab.SpecimenActionCode.ToString())+"'";
			}
			if(ehrLab.ResultDateTime != oldEhrLab.ResultDateTime) {
				if(command!=""){ command+=",";}
				command+="ResultDateTime = '"+POut.String(ehrLab.ResultDateTime)+"'";
			}
			if(ehrLab.ResultStatus != oldEhrLab.ResultStatus) {
				if(command!=""){ command+=",";}
				command+="ResultStatus = '"+POut.String(ehrLab.ResultStatus.ToString())+"'";
			}
			if(ehrLab.ParentObservationID != oldEhrLab.ParentObservationID) {
				if(command!=""){ command+=",";}
				command+="ParentObservationID = '"+POut.String(ehrLab.ParentObservationID)+"'";
			}
			if(ehrLab.ParentObservationText != oldEhrLab.ParentObservationText) {
				if(command!=""){ command+=",";}
				command+="ParentObservationText = '"+POut.String(ehrLab.ParentObservationText)+"'";
			}
			if(ehrLab.ParentObservationCodeSystemName != oldEhrLab.ParentObservationCodeSystemName) {
				if(command!=""){ command+=",";}
				command+="ParentObservationCodeSystemName = '"+POut.String(ehrLab.ParentObservationCodeSystemName)+"'";
			}
			if(ehrLab.ParentObservationIDAlt != oldEhrLab.ParentObservationIDAlt) {
				if(command!=""){ command+=",";}
				command+="ParentObservationIDAlt = '"+POut.String(ehrLab.ParentObservationIDAlt)+"'";
			}
			if(ehrLab.ParentObservationTextAlt != oldEhrLab.ParentObservationTextAlt) {
				if(command!=""){ command+=",";}
				command+="ParentObservationTextAlt = '"+POut.String(ehrLab.ParentObservationTextAlt)+"'";
			}
			if(ehrLab.ParentObservationCodeSystemNameAlt != oldEhrLab.ParentObservationCodeSystemNameAlt) {
				if(command!=""){ command+=",";}
				command+="ParentObservationCodeSystemNameAlt = '"+POut.String(ehrLab.ParentObservationCodeSystemNameAlt)+"'";
			}
			if(ehrLab.ParentObservationTextOriginal != oldEhrLab.ParentObservationTextOriginal) {
				if(command!=""){ command+=",";}
				command+="ParentObservationTextOriginal = '"+POut.String(ehrLab.ParentObservationTextOriginal)+"'";
			}
			if(ehrLab.ParentObservationSubID != oldEhrLab.ParentObservationSubID) {
				if(command!=""){ command+=",";}
				command+="ParentObservationSubID = '"+POut.String(ehrLab.ParentObservationSubID)+"'";
			}
			if(ehrLab.ParentPlacerOrderNum != oldEhrLab.ParentPlacerOrderNum) {
				if(command!=""){ command+=",";}
				command+="ParentPlacerOrderNum = '"+POut.String(ehrLab.ParentPlacerOrderNum)+"'";
			}
			if(ehrLab.ParentPlacerOrderNamespace != oldEhrLab.ParentPlacerOrderNamespace) {
				if(command!=""){ command+=",";}
				command+="ParentPlacerOrderNamespace = '"+POut.String(ehrLab.ParentPlacerOrderNamespace)+"'";
			}
			if(ehrLab.ParentPlacerOrderUniversalID != oldEhrLab.ParentPlacerOrderUniversalID) {
				if(command!=""){ command+=",";}
				command+="ParentPlacerOrderUniversalID = '"+POut.String(ehrLab.ParentPlacerOrderUniversalID)+"'";
			}
			if(ehrLab.ParentPlacerOrderUniversalIDType != oldEhrLab.ParentPlacerOrderUniversalIDType) {
				if(command!=""){ command+=",";}
				command+="ParentPlacerOrderUniversalIDType = '"+POut.String(ehrLab.ParentPlacerOrderUniversalIDType)+"'";
			}
			if(ehrLab.ParentFillerOrderNum != oldEhrLab.ParentFillerOrderNum) {
				if(command!=""){ command+=",";}
				command+="ParentFillerOrderNum = '"+POut.String(ehrLab.ParentFillerOrderNum)+"'";
			}
			if(ehrLab.ParentFillerOrderNamespace != oldEhrLab.ParentFillerOrderNamespace) {
				if(command!=""){ command+=",";}
				command+="ParentFillerOrderNamespace = '"+POut.String(ehrLab.ParentFillerOrderNamespace)+"'";
			}
			if(ehrLab.ParentFillerOrderUniversalID != oldEhrLab.ParentFillerOrderUniversalID) {
				if(command!=""){ command+=",";}
				command+="ParentFillerOrderUniversalID = '"+POut.String(ehrLab.ParentFillerOrderUniversalID)+"'";
			}
			if(ehrLab.ParentFillerOrderUniversalIDType != oldEhrLab.ParentFillerOrderUniversalIDType) {
				if(command!=""){ command+=",";}
				command+="ParentFillerOrderUniversalIDType = '"+POut.String(ehrLab.ParentFillerOrderUniversalIDType)+"'";
			}
			if(ehrLab.ListEhrLabResultsHandlingF != oldEhrLab.ListEhrLabResultsHandlingF) {
				if(command!=""){ command+=",";}
				command+="ListEhrLabResultsHandlingF = "+POut.Bool(ehrLab.ListEhrLabResultsHandlingF)+"";
			}
			if(ehrLab.ListEhrLabResultsHandlingN != oldEhrLab.ListEhrLabResultsHandlingN) {
				if(command!=""){ command+=",";}
				command+="ListEhrLabResultsHandlingN = "+POut.Bool(ehrLab.ListEhrLabResultsHandlingN)+"";
			}
			if(ehrLab.TQ1SetId != oldEhrLab.TQ1SetId) {
				if(command!=""){ command+=",";}
				command+="TQ1SetId = "+POut.Long(ehrLab.TQ1SetId)+"";
			}
			if(ehrLab.TQ1DateTimeStart != oldEhrLab.TQ1DateTimeStart) {
				if(command!=""){ command+=",";}
				command+="TQ1DateTimeStart = '"+POut.String(ehrLab.TQ1DateTimeStart)+"'";
			}
			if(ehrLab.TQ1DateTimeEnd != oldEhrLab.TQ1DateTimeEnd) {
				if(command!=""){ command+=",";}
				command+="TQ1DateTimeEnd = '"+POut.String(ehrLab.TQ1DateTimeEnd)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE ehrlab SET "+command
				+" WHERE EhrLabNum = "+POut.Long(ehrLab.EhrLabNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one EhrLab from the database.</summary>
		public static void Delete(long ehrLabNum){
			string command="DELETE FROM ehrlab "
				+"WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			Db.NonQ(command);
		}

	}
}