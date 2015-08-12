using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabs.</summary>
		private static List<MedLab> listt;

		///<summary>A list of all MedLabs.</summary>
		public static List<MedLab> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM medlab ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLab";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary>Gets one MedLab from the db.</summary>
		public static MedLab GetOne(long medLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<MedLab>(MethodBase.GetCurrentMethod(),medLabNum);
			}
			return Crud.MedLabCrud.SelectOne(medLabNum);
		}

		///<summary>Get all MedLab objects for a specific patient from the database.  Can return an empty list.</summary>
		public static List<MedLab> GetForPatient(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLab>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlab WHERE PatNum="+POut.Long(patNum);
			return Crud.MedLabCrud.SelectMany(command);
		}

		///<summary>Get unique MedLab orders, grouped by PatNum, ProvNum, and SpecimenID.  Also returns the most recent DateTime the results
		///were released from the lab and a list of test descriptions ordered.
		///If includeNoPat==true, the lab orders not attached to a patient will be included.</summary>
		public static List<MedLab> GetOrdersForPatient(Patient pat,bool includeNoPat,bool onlyNoPat,DateTime dateReportedStart,DateTime dateReportedEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLab>>(MethodBase.GetCurrentMethod(),pat,includeNoPat,onlyNoPat,dateReportedStart,dateReportedEnd);
			}
			//include all patients unless a patient is specified.
			string patNumClause="medlab.PatNum>0";
			if(pat!=null) {
				patNumClause="medlab.PatNum="+POut.Long(pat.PatNum);
			}
			//do not include patnum=0 unless specified.
			if(includeNoPat) {
				patNumClause+=" OR medlab.PatNum=0";
			}
			if(onlyNoPat) {
				patNumClause="medlab.PatNum=0";
			}
			string command="SELECT MAX(CASE WHEN medlab.DateTimeReported=maxDate.DateTimeReported THEN MedLabNum ELSE 0 END) AS MedLabNum,"
				+"SendingApp,SendingFacility,medlab.PatNum,medlab.ProvNum,PatIDLab,PatIDAlt,PatAge,PatAccountNum,PatFasting,medlab.SpecimenID,"
				+"SpecimenIDFiller,ObsTestID,ObsTestLoinc,ObsTestLoincText,DateTimeCollected,TotalVolume,ActionCode,ClinicalInfo,"
				+"MIN(DateTimeEntered) AS DateTimeEntered,OrderingProvNPI,OrderingProvLocalID,OrderingProvLName,OrderingProvFName,SpecimenIDAlt,"
				+"maxdate.DateTimeReported,MIN(CASE WHEN medlab.DateTimeReported=maxDate.DateTimeReported THEN ResultStatus ELSE NULL END) AS ResultStatus,"
				+"ParentObsID,ParentObsTestID,NotePat,NoteLab,FileName,"
				+"MIN(CASE WHEN medlab.DateTimeReported=maxDate.DateTimeReported THEN OriginalPIDSegment ELSE NULL END) AS OriginalPIDSegment,"
				+DbHelper.GroupConcat("ObsTestDescript",distinct:true,separator:"\r\n")+" AS ObsTestDescript "
				+"FROM medlab "
				+"INNER JOIN ("
					+"SELECT PatNum,ProvNum,SpecimenID,MAX(DateTimeReported) AS DateTimeReported "
					+"FROM medlab "
					+"WHERE ("+patNumClause+") " //Ex: WHERE (medlab.PatNum>0 OR medlab.Patnum=0)
					+"GROUP BY PatNum,ProvNum,SpecimenID "
					+"HAVING "+DbHelper.DtimeToDate("MAX(DateTimeReported)")+" BETWEEN "+POut.Date(dateReportedStart)+" AND "+POut.Date(dateReportedEnd)
				+") maxDate ON maxDate.PatNum=medlab.PatNum AND maxDate.ProvNum=medlab.ProvNum AND maxDate.SpecimenID=medlab.SpecimenID "
				+"GROUP BY medlab.PatNum,medlab.ProvNum,medlab.SpecimenID "
				+"ORDER BY maxdate.DateTimeReported DESC,medlab.SpecimenID,MedLabNum";//most recently received lab on top, with all for a specific specimen together
			return Crud.MedLabCrud.SelectMany(command);
		}

		///<summary>Get MedLabs for a specific patient and a specific SpecimenID, SpecimenIDFiller combination.
		///Ordered by DateTimeReported descending, MedLabNum descending so the most recently reported/processed message is first in the list.
		///If using random primary keys, this information may be incorectly ordered, but that is only an annoyance and this function should still work.</summary>
		public static List<MedLab> GetForPatAndSpecimen(long patNum,string specimenID,string specimenIDFiller) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLab>>(MethodBase.GetCurrentMethod(),patNum,specimenID,specimenIDFiller);
			}
			string command="SELECT * FROM medlab WHERE PatNum="+POut.Long(patNum)+" "
				+"AND SpecimenID='"+POut.String(specimenID)+"' "
				+"AND SpecimenIDFiller='"+POut.String(specimenIDFiller)+"' "
				+"ORDER BY DateTimeReported DESC,MedLabNum DESC";
			return Crud.MedLabCrud.SelectMany(command);
		}

		public static void UpdateFileNames(List<long> medLabNumList,string fileNameNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabNumList,fileNameNew);
				return;
			}
			string command="UPDATE medlab SET FileName='"+POut.String(fileNameNew)+"' WHERE MedLabNum IN("+string.Join(",",medLabNumList)+")";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(MedLab medLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				medLab.MedLabNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLab);
				return medLab.MedLabNum;
			}
			return Crud.MedLabCrud.Insert(medLab);
		}

		///<summary></summary>
		public static void Update(MedLab medLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLab);
				return;
			}
			Crud.MedLabCrud.Update(medLab);
		}

		///<summary>Translates enum values into human readable strings.</summary>
		public static string GetStatusDescript(ResultStatus resultStatus) {
			//No need to check RemotingRole; no call to db.
			switch(resultStatus) {
				case ResultStatus.C:
					return "Corrected";
				case ResultStatus.F:
					return "Final";
				case ResultStatus.I:
					return "Incomplete";
				case ResultStatus.P:
					return "Preliminary";
				case ResultStatus.X:
					return "Canceled";
				default:
					return "";
			}
		}

		///<summary>Delete all of the MedLab objects by MedLabNum.</summary>
		public static void DeleteAll(List<long> listLabNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listLabNums);
				return;
			}
			string command= "DELETE FROM medlab WHERE MedLabNum IN("+String.Join(",",listLabNums)+")";
			Db.NonQ(command);
		}

		///<summary>Returns a list of MedLabFacilityNums, the order in the list will be the facility ID on the report.  Basically a local re-numbering.
		///Each message has a facility or facilities with footnote IDs, e.g. 01, 02, etc.  The results each link to the facility that performed the test.
		///But if there are multiple messages for a test order, e.g. when there is a final result for a subset of the original test results,
		///the additional message may have a facility with footnote ID of 01 that is different than the original message facility with ID 01.
		///So each ID could link to multiple facilities.  We will re-number the facilities so that each will have a unique number for this report.</summary>
		public static List<long> GetListFacNums(List<MedLab> listMedLabs,out List<MedLabResult> listResults) {
			//No need to check RemotingRole; no call to db.
			listResults=MedLabResults.GetAllForLabs(listMedLabs);//use the classwide variable so we can use the list to create the data table
			for(int i=listResults.Count-1;i>-1;i--) {//loop through backward and only keep the most final/most recent result
				if(i==0) {
					break;
				}
				if(listResults[i].ObsID==listResults[i-1].ObsID && listResults[i].ObsIDSub==listResults[i-1].ObsIDSub) {
					listResults.RemoveAt(i);
				}
			}
			listResults.Sort(SortByMedLabNum);
			//listResults will now only contain the most recent or most final/corrected results, sorted by the order inserted in the db
			List<long> listMedLabFacilityNums=new List<long>();
			for(int i=0;i<listResults.Count;i++) {
				List<MedLabFacAttach> listFacAttaches=MedLabFacAttaches.GetAllForLabOrResult(0,listResults[i].MedLabResultNum);
				if(listFacAttaches.Count==0) {
					continue;
				}
				if(!listMedLabFacilityNums.Contains(listFacAttaches[0].MedLabFacilityNum)) {
					listMedLabFacilityNums.Add(listFacAttaches[0].MedLabFacilityNum);
				}
				listResults[i].FacilityID=(listMedLabFacilityNums.IndexOf(listFacAttaches[0].MedLabFacilityNum)+1).ToString().PadLeft(2,'0');
			}
			return listMedLabFacilityNums;
		}

		///<summary>Sort by MedLabResult.MedLabResultNum.</summary>
		private static int SortByMedLabNum(MedLabResult medLabResultX,MedLabResult medLabResultY) {
			if(medLabResultX.MedLabNum!=medLabResultY.MedLabNum) {
				return medLabResultX.MedLabNum.CompareTo(medLabResultY.MedLabNum);
			}
			return medLabResultX.MedLabResultNum.CompareTo(medLabResultY.MedLabResultNum);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.


		///<summary></summary>
		public static void Delete(long medLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabNum);
				return;
			}
			string command= "DELETE FROM medlab WHERE MedLabNum = "+POut.Long(medLabNum);
			Db.NonQ(command);
		}
		*/



	}
}