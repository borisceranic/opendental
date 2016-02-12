using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheClaimPayment:DashboardCacheWithQuery<ClaimPayment> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="DateCP BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProvNum,DateCP,SUM(InsPayAmt) AS GrossIncome "
				+"FROM claimproc "
				+"WHERE "+where+"ClaimPaymentNum<>0 AND InsPayAmt<>0 "
				+"GROUP BY ProvNum,DateCP ";
		}

		protected override ClaimPayment GetInstanceFromDataRow(DataRow x) {
			long provNum=x.Field<long>("ProvNum");
			string provName=DashboardCache.Providers.GetProvName(provNum);
			return new ClaimPayment() {
				ProvNum=provNum,
				DateStamp=x.Field<DateTime>("DateCP"),
				Val=x.Field<double>("GrossIncome"),
				Count=0, //nothing to count for income.
				SeriesName=provName,
			};
		}
	}

	public class ClaimPayment:GraphQuantityOverTime.GraphDataPointProv { }
}