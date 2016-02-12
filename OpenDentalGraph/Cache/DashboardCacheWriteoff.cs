using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheWriteoff:DashboardCacheWithQuery<Writeoff> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="ProcDate BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProcDate,ProvNum,SUM(WriteOff) AS WriteOffs "
				+"FROM claimproc "
				+"WHERE "+where+"claimproc.Status IN ("
				+POut.Int((int)ClaimProcStatus.Received)+","
				+POut.Int((int)ClaimProcStatus.Supplemental)+","
				+POut.Int((int)ClaimProcStatus.NotReceived)+") "
				+"GROUP BY ProcDate,ProvNum "
				+"HAVING WriteOffs<>0 ";
		}

		protected override Writeoff GetInstanceFromDataRow(DataRow x) {
			long provNum=x.Field<long>("ProvNum");
			string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new Writeoff() {
				ProvNum=provNum,
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=-x.Field<double>("WriteOffs"),
				Count=0, //count procedures, not writeoffs.
				SeriesName=seriesName,
			};
		}
	}

	public class Writeoff:GraphQuantityOverTime.GraphDataPointProv { }
}