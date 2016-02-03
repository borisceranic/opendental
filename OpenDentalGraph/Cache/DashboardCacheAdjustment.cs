using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheAdjustment:DashboardCacheWithQuery<Adjustment> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="WHERE AdjDate>"+POut.Date(filter.DateFrom)+" AND AdjDate<"+POut.Date(filter.DateTo)+" ";
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
			return new Adjustment() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("AdjDate"),
				Val=x.Field<double>("AdjTotal"),
			};
		}
	}

	public class Adjustment:GraphQuantityOverTime.GraphDataPointProv {
		public override bool IncludeInCountMode() {
			return false;
		}
	}
}