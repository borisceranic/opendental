using System;
using System.Collections.Generic;
using OpenDentBusiness;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheBrokenProcedure:DashboardCacheWithQuery<BrokenProc> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="WHERE ProcStatus="+(int)ProcStat.C+" ";
			if(filter.UseDateFilter) {
				where="AND DATE(ProcDate) BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT ProcDate,ProvNum,ClinicNum,COUNT(ProcNum) ProcCount, SUM(ProcFee) ProcFee "
				+"FROM procedurelog "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
				+"AND procedurecode.ProcCode='D9986' "
				+where
				+"GROUP BY ProcDate,ProvNum,ClinicNum ";
		}

		protected override BrokenProc GetInstanceFromDataRow(DataRow x) {
			return new BrokenProc() {
				ProvNum=x.Field<long>("ProvNum"),
				ClinicNum=x.Field<long>("ClinicNum"),
				DateStamp=x.Field<DateTime>("ProcDate"),
				Count=x.Field<long>("ProcCount"),
				Val=x.Field<double>("ProcFee")
			};
		}
	}

	public class BrokenProc:GraphQuantityOverTime.GraphDataPointClinic {
	}	
}