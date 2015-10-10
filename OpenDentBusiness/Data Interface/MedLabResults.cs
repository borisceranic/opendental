using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabResults{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabResults.</summary>
		private static List<MedLabResult> listt;

		///<summary>A list of all MedLabResults.</summary>
		public static List<MedLabResult> Listt{
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
			string command="SELECT * FROM medlabresult ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabResult";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabResultCrud.TableToList(table);
		}
		#endregion
		*/



		///<summary>Returns a list of all MedLabResults from the db for a given MedLab.  Ordered by ObsID,ObsIDSub,ResultStatus,DateTimeObs DESC.
		///Corrected (ResultStatus=0) will be first in the list then 1=Final, 2=Incomplete, 3=Preliminary, and 4=Cancelled.
		///Then ordered by DateTimeObs DESC, most recent of each status comes first in the list.
		///If there are no results for the lab (or medLabNum=0), this will return an empty list.</summary>
		public static List<MedLabResult> GetForLab(long medLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),medLabNum);
			}
			string command="SELECT * FROM medlabresult WHERE MedLabNum="+POut.Long(medLabNum)+" ORDER BY ObsID,ObsIDSub,ResultStatus,DateTimeObs DESC";
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary>Gets a list of all MedLabResult object from the database for all of the MedLab objects sent in.  The MedLabResults are ordered by
		///ObsID,ObsIDSub,ResultStatus, and DateTimeObs DESC to make it easier to find the most recent and up to date result for a given ObsID and
		///optional ObsIDSub result.  The result statuses are 0=Corrected, 1=Final, 2=Incomplete, 3=Preliminary, and 4=Cancelled.
		///Corrected will be first in the list for each ObsID/ObsIDSub, then Final, etc.
		///If there is more than one result with the same ObsID/ObsIDSub and status, the most recent DateTimeObs will be first.</summary>
		public static List<MedLabResult> GetAllForLabs(List<MedLab> listMedLabs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),listMedLabs);
			}
			List<MedLabResult> retval=new List<MedLabResult>();
			if(listMedLabs.Count==0) {
				return retval;
			}
			List<long> listMedLabNums=new List<long>();
			for(int i=0;i<listMedLabs.Count;i++) {
				listMedLabNums.Add(listMedLabs[i].MedLabNum);
			}
			string command="SELECT * FROM medlabresult WHERE MedLabNum IN("+String.Join(",",listMedLabNums)+") "
				+"ORDER BY ObsID,ObsIDSub,ResultStatus,DateTimeObs DESC,MedLabResultNum DESC";
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary>Gets a list of all MedLabResult objects for this patient with the same ObsID and ObsIDSub as the supplied medLabResult,
		///and for the same SpecimenID and SpecimenIDFiller.  Ordered by ResultStatus,DateTimeObs descending, MedLabResultNum descending.
		///Used to display the history of a result as many statuses may be received.</summary>
		public static List<MedLabResult> GetResultHist(MedLabResult medLabResult,long patNum,string specimenID,string specimenIDFiller) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),medLabResult,patNum,specimenID,specimenIDFiller);
			}
			List<MedLabResult> retval=new List<MedLabResult>();
			if(medLabResult==null) {
				return retval;
			}
			string command="SELECT medlabresult.* FROM medlabresult "
				+"INNER JOIN medlab ON medlab.MedLabNum=medlabresult.MedLabNum "
					+"AND medlab.PatNum="+POut.Long(patNum)+" "
					+"AND medlab.SpecimenID='"+POut.String(specimenID)+"' "
					+"AND medlab.SpecimenIDFiller='"+POut.String(specimenIDFiller)+"' "
				+"WHERE medlabresult.ObsID='"+POut.String(medLabResult.ObsID)+"' "
				+"AND medlabresult.ObsIDSub='"+POut.String(medLabResult.ObsIDSub)+"' "
				+"ORDER BY medlabresult.ResultStatus,medlabresult.DateTimeObs DESC,medlabresult.MedLabResultNum DESC";
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(MedLabResult medLabResult) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				medLabResult.MedLabResultNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabResult);
				return medLabResult.MedLabResultNum;
			}
			return Crud.MedLabResultCrud.Insert(medLabResult);
		}

		///<summary></summary>
		public static void Update(MedLabResult medLabResult) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabResult);
				return;
			}
			Crud.MedLabResultCrud.Update(medLabResult);
		}

		///<summary>Delete all of the MedLabResult objects by MedLabResultNum.</summary>
		public static void DeleteAll(List<long> listResultNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listResultNums);
				return;
			}
			string command= "DELETE FROM medlabresult WHERE MedLabResultNum IN("+String.Join(",",listResultNums)+")";
			Db.NonQ(command);
		}

		///<summary>Delete all of the MedLabResult objects by MedLabNum.</summary>
		public static void DeleteAllForMedLabs(List<long> listMedLabNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listMedLabNums);
				return;
			}
			if(listMedLabNums==null || listMedLabNums.Count<1) {
				return;
			}
			string command= "DELETE FROM medlabresult WHERE MedLabNum IN("+String.Join(",",listMedLabNums)+")";
			Db.NonQ(command);
		}

		public static string GetAbnormalFlagDescript(AbnormalFlag abnormalFlag) {
			//No need to check RemotingRole; no call to db.
			switch(abnormalFlag) {
				case AbnormalFlag._gt:
					return "Panic High";
				case AbnormalFlag._lt:
					return "Panic Low";
				case AbnormalFlag.A:
					return "Abnormal";
				case AbnormalFlag.AA:
					return "Critical Abnormal";
				case AbnormalFlag.H:
					return "Above High Normal";
				case AbnormalFlag.HH:
					return "Alert High";
				case AbnormalFlag.I:
					return "Intermediate";
				case AbnormalFlag.L:
					return "Below Low Normal";
				case AbnormalFlag.LL:
					return "Alert Low";
				case AbnormalFlag.NEG:
					return "Negative";
				case AbnormalFlag.POS:
					return "Positive";
				case AbnormalFlag.R:
					return "Resistant";
				case AbnormalFlag.S:
					return "Susceptible";
				case AbnormalFlag.None:
				default:
					return "";
			}
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabResult> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabResult>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabresult WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabResultCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabResult from the db.</summary>
		public static MedLabResult GetOne(long medLabResultNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabResult>(MethodBase.GetCurrentMethod(),medLabResultNum);
			}
			return Crud.MedLabResultCrud.SelectOne(medLabResultNum);
		}

		///<summary></summary>
		public static void Delete(long medLabResultNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabResultNum);
				return;
			}
			string command= "DELETE FROM medlabresult WHERE MedLabResultNum = "+POut.Long(medLabResultNum);
			Db.NonQ(command);
		}
		*/



	}
}