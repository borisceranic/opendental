using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCachePaySplit:DashboardCacheWithQuery<PaySplit> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="DatePay>"+POut.Date(filter.DateFrom)+" AND DatePay<"+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProvNum,DatePay,SUM(SplitAmt) AS GrossSplit "
				+"FROM paysplit "
				+"WHERE "+where+"IsDiscount=0 "
				+"GROUP BY ProvNum,DatePay ";
		}

		protected override PaySplit GetInstanceFromDataRow(DataRow x) {
			return new PaySplit() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("DatePay"),
				Val=x.Field<double>("GrossSplit"),
			};
		}
	}

	public class PaySplit:GraphQuantityOverTime.GraphDataPointProv { }
}