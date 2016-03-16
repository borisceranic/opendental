using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheCompletedProc:DashboardCacheWithQuery<CompletedProc> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="WHERE procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" ";
			if(filter.UseDateFilter) {
				where+="AND procedurelog.ProcDate BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT procedurelog.ProcDate,procedurelog.ProvNum, "
				+"SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)) AS GrossProd, "
				+"COUNT(procedurelog.ProcNum) AS ProcCount, ClinicNum "
				+"FROM procedurelog "
				+where
				+"GROUP BY procedurelog.ProcDate,procedurelog.ProvNum ";
		}

		protected override CompletedProc GetInstanceFromDataRow(DataRow x) {
			//long provNum=x.Field<long>("ProvNum");
			//string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new CompletedProc() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=x.Field<double>("GrossProd"),
				Count=x.Field<long>("ProcCount"),
				//SeriesName=seriesName,
				ClinicNum=x.Field<long>("ClinicNum"),
			};
		}
	}

	public class CompletedProc:GraphQuantityOverTime.GraphDataPointClinic { }
}