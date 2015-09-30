using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpRouting {
		///<summary>Gets a list of aptNums for one day in the schedule for a given set of providers and clinics.</summary>
		public static List<long> GetRouting(DateTime date,List<long> listProvNums,List<long> listClinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),date,listProvNums);
			}
			string command=
				"SELECT AptNum FROM appointment "
				+"WHERE AptDateTime LIKE '"+POut.Date(date,false)+"%' "
				+"AND aptstatus != '"+(int)ApptStatus.UnschedList+"' "
				+"AND aptstatus != '"+(int)ApptStatus.Planned+"' "
				+"AND ProvNum IN ("+String.Join(",",listProvNums)+") ";
			if(listClinicNums.Count>0) {
				command+="AND ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command+="ORDER BY AptDateTime";
			DataTable table=Db.GetTable(command);
			List<long> retVal=new List<long>();
			for(int i=0;i<table.Rows.Count;i++) {
				retVal.Add(PIn.Long(table.Rows[i][0].ToString()));
			}
			return retVal;
		}
	}
}
