using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheCompletedProc:DashboardCacheWithQuery<CompletedProc> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="ProcDate>"+POut.Date(filter.DateFrom)+" AND ProcDate<"+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProcDate,ProvNum,SUM(ProcFee*(UnitQty+BaseUnits)) AS GrossProd "
				+"FROM procedurelog USE INDEX(indexPNPD) "
				+"WHERE "+where+"ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"GROUP BY ProcDate,ProvNum "
				+"HAVING GrossProd<>0 ";
		}

		protected override CompletedProc GetInstanceFromDataRow(DataRow x) {
			long provNum=x.Field<long>("ProvNum");
			string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new CompletedProc() {
				ProvNum=provNum,
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=x.Field<double>("GrossProd"),
				SeriesName=seriesName,
			};
		}
	}

	public class CompletedProc:GraphQuantityOverTime.GraphDataPointProv { }
}