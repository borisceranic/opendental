using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpOutstandingIns {

		///<summary>Called from FormRpOutstandingIns. Gets outstanding insurance claims. Requires all fields. provNumList may be empty (but will return null if isAllProv is false).  listClinicNums may be empty.  dateMin and dateMax will not be used if they are set to DateTime.MinValue() (01/01/0001). If isPreauth is true only claims of type preauth will be returned.</summary>
		public static DataTable GetOutInsClaims(bool isAllProv,List<long> listProvNums,DateTime dateMin,DateTime dateMax,bool isPreauth,List<long> listClinicNums){ 
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) { 
				return Meth.GetTable(MethodBase.GetCurrentMethod(),isAllProv,listProvNums,dateMin,dateMax,isPreauth,listClinicNums); 
			} 
			string command;
			command = "SELECT carrier.CarrierName,carrier.Phone,claim.ClaimType,patient.FName,patient.LName,patient.MiddleI,patient.PatNum,"
				+"claim.DateService,claim.DateSent,claim.ClaimFee,claim.ClaimNum,claim.ClinicNum,"
				+"definition.ItemValue DaysSuppressed,"+DbHelper.DtimeToDate("securitylog.LogDateTime")+" DateLog,"
				+"definition.DefNum DefNum "
				+"FROM carrier "
				+"INNER JOIN insplan ON insplan.CarrierNum=carrier.CarrierNum "
				+"INNER JOIN claim ON claim.PlanNum=insplan.PlanNum "
				+"AND claim.ClaimStatus='S' ";
			if(dateMin!=DateTime.MinValue) {
				command+="AND claim.DateSent <= "+POut.Date(dateMin)+" ";
			}
			if(dateMax!=DateTime.MinValue) {
				command+="AND claim.DateSent >= "+POut.Date(dateMax)+" ";
			}
			if(!isAllProv) {
				if(listProvNums.Count>0) {
					command+="AND claim.ProvTreat IN ("+String.Join(",",listProvNums)+") ";
				}
			}
			if(listClinicNums.Count>0) {
				command+="AND claim.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(!isPreauth) {
				command+="AND claim.ClaimType!='Preauth' ";
			}
			command+="LEFT JOIN definition ON definition.DefNum=claim.CustomTracking "
				+"LEFT JOIN securitylog ON securitylog.FKey=claim.ClaimNum " //FKey matches ClaimNum
					+"AND securitylog.DefNum=definition.DefNum "	//DefNum only used for claim custom history edits
					+"AND securitylog.LogDateTime=(SELECT MAX(sl.LogDateTime) FROM securitylog sl WHERE sl.FKey=claim.ClaimNum AND sl.DefNum!=0) "
					+"AND securitylog.PermType="+(int)Permissions.ClaimHistoryEdit+" "
				+"INNER JOIN patient ON patient.PatNum=claim.PatNum ";
			command+="ORDER BY carrier.Phone,insplan.PlanNum";
			object[] parameters={command};
			Plugins.HookAddCode(null,"Claims.GetOutInsClaims_beforequeryrun",parameters);//Moved entire method from Claims.cs
			command=(string)parameters[0];
			DataTable table=Db.GetTable(command);
			return table;
		}

	}
}
