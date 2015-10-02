using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpInsOverpaid {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetInsuranceOverpaid(List<long> listClinicNums,bool groupByProc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,groupByProc);
			}
			string query=@"SELECT "+DbHelper.Concat("patient.LName","', '","patient.FName")+@" patname,
				procedurelog.ProcDate,
				SUM(procedurelog.ProcFee) ""$sumfee"",
				SUM((SELECT SUM(claimproc.InsPayAmt + claimproc.Writeoff) FROM claimproc WHERE claimproc.ProcNum=procedurelog.ProcNum)) AS
				""$PaidAndWriteoff""
				FROM procedurelog
				LEFT JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum
				LEFT JOIN patient ON patient.PatNum=procedurelog.PatNum
				WHERE procedurelog.ProcStatus=2/*complete*/
				AND procedurelog.ProcFee>0 ";//Negative proc fees should not show up on this report.  
																	   //We have one office that uses negative proc fees as internal adjustments.
			if(listClinicNums.Count>0) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(groupByProc) {
				query+="GROUP BY procedurelog.ProcNum ";
			}
			else {//Group by patient
				query+="GROUP BY procedurelog.PatNum,procedurelog.ProcDate ";
			}
			query+=@"HAVING ROUND($sumfee,3) < ROUND($PaidAndWriteoff,3)
				ORDER BY patname,ProcDate ";
			return Db.GetTable(query);
		}
		
	}
}
