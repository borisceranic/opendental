using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpClaimNotSent {

		public static DataTable GetClaimsNotSent(bool dateRange,string fromDate,string toDate,List<long> listClinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateRange,fromDate,toDate,listClinicNums);
			}
			string whereClin="";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				whereClin+=" AND claim.ClinicNum IN  (" + string.Join(",",listClinicNums)+")";
			}

			string command="SELECT claim.DateService,(CASE WHEN claim.ClaimType='P' THEN 'Primary' WHEN claim.ClaimType='S' THEN 'Secondary' "
				+"WHEN claim.ClaimType='Cap' THEN 'Capitation' ELSE claim.ClaimType END) AS ClaimType,(CASE WHEN claim.ClaimStatus='U' THEN 'Unsent' WHEN "
				+"claim.ClaimStatus='H' THEN 'Hold' WHEN claim.ClaimStatus='W' THEN 'WaitQ' ELSE claim.ClaimStatus END) AS ClaimStatus, "
				+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),carrier.CarrierName,claim.ClaimFee "
				+"FROM patient,claim,insplan,carrier "
				+"WHERE patient.PatNum=claim.PatNum AND insplan.PlanNum=claim.PlanNum "
				+whereClin
				+"AND insplan.CarrierNum=carrier.CarrierNum "	
				+"AND (claim.ClaimStatus = 'U' OR claim.ClaimStatus = 'H' OR  claim.ClaimStatus = 'W')";
			if(dateRange==true) {
				command+=" AND claim.DateService >= '"+fromDate+"' "
					+"AND claim.DateService <= '"+toDate+"'";
			}
			else {
				command+=" AND claim.DateService = '"+fromDate+"'";
			}
			command+=" ORDER BY claim.DateService";

			DataTable _retVal=Db.GetTable(command);
			return _retVal;
		}


	}
}
