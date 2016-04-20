using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpProcNotBilledIns {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetProcsNotBilled(List<long> listClinicNums,bool includeMedProcs,DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,includeMedProcs,dateStart,dateEnd);
			}
			string query="SELECT ";
			if(PrefC.GetBool(PrefName.ReportsShowPatNum)) {
				query+=DbHelper.Concat("CAST(patient.PatNum AS CHAR)","'-'","patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			else {
				query+=DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			query+=" AS 'PatientName',procedurelog.ProcDate,procedurecode.Descript,procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits),procedurelog.ProcNum "
				+"FROM patient,procedurecode,procedurelog,claimproc,insplan "
				+"WHERE claimproc.procnum=procedurelog.procnum "
				+"AND patient.PatNum=procedurelog.PatNum "
				+"AND procedurelog.CodeNum=procedurecode.CodeNum "
				+"AND claimproc.PlanNum=insplan.PlanNum ";
			if(!includeMedProcs) {
				query+="AND insplan.IsMedical=0 ";
			}
			query+="AND claimproc.NoBillIns=0 "
				+"AND procedurelog.ProcFee>0 "
				+"AND claimproc.Status="+(int)ClaimProcStatus.Estimate+" "
				+"AND procedurelog.procstatus="+(int)ProcStat.C+" ";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				query+="AND procedurecode.IsCanadianLab=0 ";//ignore Canadian labs
			}
			query+="AND procedurelog.ProcDate	BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" ";
			if(listClinicNums.Count>0) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			query+="GROUP BY procedurelog.ProcNum "
				+"ORDER BY patient.LName,patient.FName,patient.PatNum,procedurelog.ProcDate";
			return Db.GetTable(query);
		}

	}
}
