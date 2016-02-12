using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheAdjustment:DashboardCacheWithQuery<Adjustment> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="WHERE AdjDate BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT AdjDate,ProvNum,SUM(AdjAmt) AdjTotal "
				+"FROM adjustment "
				+where
				+"GROUP BY AdjDate,ProvNum "
				+"HAVING AdjTotal<>0 "
				+"ORDER BY AdjDate,ProvNum ";
		}

		protected override Adjustment GetInstanceFromDataRow(DataRow x) {
			long provNum=x.Field<long>("ProvNum");
			string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new Adjustment() {
				ProvNum=provNum,
				DateStamp=x.Field<DateTime>("AdjDate"),
				Val=x.Field<double>("AdjTotal"),	
				Count=0, //count procedures, not adjustments.			
				SeriesName=seriesName,
			};
		}
	}

	public class Adjustment:GraphQuantityOverTime.GraphDataPointProv {
	}
}