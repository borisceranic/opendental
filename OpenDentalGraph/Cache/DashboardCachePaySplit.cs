using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCachePaySplit:DashboardCacheWithQuery<PaySplit> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="DatePay BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProvNum,DatePay,SUM(SplitAmt) AS GrossSplit,ClinicNum "
				+"FROM paysplit "
				+"WHERE "+where+"IsDiscount=0 "
				+"GROUP BY ProvNum,DatePay,ClinicNum ";
		}

		protected override PaySplit GetInstanceFromDataRow(DataRow x) {
			//long provNum=x.Field<long>("ProvNum");
			//string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new PaySplit() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("DatePay"),
				Val=x.Field<double>("GrossSplit"),
				Count=0, //counting paysplits is not useful
								 //SeriesName=seriesName,
				ClinicNum=x.Field<long>("ClinicNum"),
			};
		}
	}

	public class PaySplit:GraphQuantityOverTime.GraphDataPointClinic { }
}