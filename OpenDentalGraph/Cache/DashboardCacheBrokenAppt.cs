using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheBrokenAppt:DashboardCacheWithQuery<BrokenAppt> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				//where="WHERE AdjDate>"+POut.Date(filter.DateFrom)+" AND AdjDate<"+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT ProcDate,ProvNum,ClinicNum,COUNT(ProcNum) ApptCount "
				+"FROM procedurelog "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum AND procedurecode.ProcCode='D9986' "
				+where
				+"GROUP BY ProcDate,ProvNum,ClinicNum ";
		}

		protected override BrokenAppt GetInstanceFromDataRow(DataRow x) {
			return new BrokenAppt() {
				ProvNum=x.Field<long>("ProvNum"),
				ClinicNum=x.Field<long>("ClinicNum"),
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=x.Field<long>("ApptCount"),
				//Should be set at runtime in GraphQuantityOverTimeFilter.FilterData().
				//SeriesName="",
			};
		}
	}

	public class BrokenAppt:GraphQuantityOverTime.GraphDataPointClinic {
	}	
}
