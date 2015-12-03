using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpProcSheet {

		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetIndividualTable(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,string procCode,
			bool isAnyClinicMedical) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvNums,listClinicNums,procCode,isAnyClinicMedical);
			};
			string query="SELECT procedurelog.ProcDate,"
			  +DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+" "
			  +"AS plfname, procedurecode.ProcCode,";
			if(!isAnyClinicMedical) {
				query+="procedurelog.ToothNum,";
			}
			query+="procedurecode.Descript,provider.Abbr,";
			if(PrefC.HasClinicsEnabled) {
				query+="COALESCE(clinic.Description,\"Unassigned\") Clinic,";
			}
			query+="procedurelog.ProcFee*(CASE procedurelog.UnitQty+procedurelog.BaseUnits WHEN 0 THEN 1 ELSE procedurelog.UnitQty+procedurelog.BaseUnits END)"
				+"-COALESCE(SUM(claimproc.WriteOff),0) ";//\"$fee\" "  //if no writeoff, then subtract 0
			if(DataConnection.DBtype==DatabaseType.MySql) {
				query+="$fee ";
			}
			else {//Oracle needs quotes.
				query+="\"$fee\" ";
			}
			query+="FROM patient "
				+"INNER JOIN procedurelog ON procedurelog.PatNum=patient.PatNum "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
				+"INNER JOIN provider ON provider.ProvNum=procedurelog.ProvNum ";
			if(PrefC.HasClinicsEnabled) {
				query+="LEFT JOIN clinic ON clinic.ClinicNum=procedurelog.ClinicNum ";
			}
			query+="LEFT JOIN claimproc ON procedurelog.ProcNum=claimproc.ProcNum "
				+"AND claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+" "//only CapComplete writeoffs are subtracted here.
				+"WHERE procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"AND procedurelog.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			if(PrefC.HasClinicsEnabled) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			query+="AND procedurecode.ProcCode LIKE '%"+POut.String(procCode)+"%' "
				+"AND "+DbHelper.DtimeToDate("procedurelog.ProcDate")+" >= " +POut.Date(dateFrom)+" "
				+"AND "+DbHelper.DtimeToDate("procedurelog.ProcDate")+" <= " +POut.Date(dateTo)+" "
				+"GROUP BY procedurelog.ProcNum "
				+"ORDER BY "+DbHelper.DtimeToDate("procedurelog.ProcDate")+",plfname,procedurecode.ProcCode,ToothNum";
			return Db.GetTable(query);
		}

		public static DataTable GetGroupedTable(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,string procCode) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvNums,listClinicNums,procCode);
			}
			string query="SELECT procs.ItemName,procs.ProcCode,procs.Descript,";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				query+="Count(*),FORMAT(ROUND(AVG(procs.fee),2),2) $AvgFee,SUM(procs.fee) AS $TotFee ";
			}
			else {//Oracle needs quotes.
				query+="Count(*),AVG(procs.fee) \"$AvgFee\",SUM(procs.fee) AS \"$TotFee\" ";
			}
			query+="FROM ( "
				+"SELECT procedurelog.ProcFee*(CASE procedurelog.UnitQty+procedurelog.BaseUnits WHEN 0 THEN 1 ELSE "
				+"procedurelog.UnitQty+procedurelog.BaseUnits END) -COALESCE(SUM(claimproc.WriteOff),0) fee, "
				+"procedurecode.ProcCode,	procedurecode.Descript,	definition.ItemName, definition.ItemOrder "
				+"FROM procedurelog "
				+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+"INNER JOIN definition ON definition.DefNum=procedurecode.ProcCat "
				+"LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum AND claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+" "
				+"WHERE procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"AND procedurelog.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
				query+="AND procedurecode.ProcCode LIKE '%"+POut.String(procCode)+"%' "
				+"AND "+DbHelper.DtimeToDate("procedurelog.ProcDate")+" >= " +POut.Date(dateFrom)+" "
				+"AND "+DbHelper.DtimeToDate("procedurelog.ProcDate")+" <= " +POut.Date(dateTo)+" "
				+"GROUP BY procedurelog.ProcNum ) procs "
				+"GROUP BY procs.ProcCode "
				+"ORDER BY procs.ItemOrder,procs.ProcCode";
			return Db.GetTable(query);
		}

	}
}
