using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	///<summary>We use the same cache to track regular writeoffs and capitation writeoffs for efficiency.</summary>
	public class DashboardCacheWriteoff:DashboardCacheWithQuery<Writeoff> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="WHERE TRUE ";
			if(filter.UseDateFilter) {
				where+="AND ProcDate BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT ProcDate,ProvNum,SUM(WriteOff) AS WriteOffs, IF(claimproc.Status="+(int)ClaimProcStatus.CapComplete+",'1','0') AS IsCap, ClinicNum "
				+"FROM claimproc "
				+where
				+"AND claimproc.Status IN ("
				+POut.Int((int)ClaimProcStatus.Received)+","
				+POut.Int((int)ClaimProcStatus.Supplemental)+","
				+POut.Int((int)ClaimProcStatus.NotReceived)+","
				+POut.Int((int)ClaimProcStatus.CapComplete)+") "
				+"GROUP BY ProcDate,ProvNum,(claimproc.Status="+(int)ClaimProcStatus.CapComplete+") "
				+"HAVING WriteOffs<>0 ";
		}

		protected override Writeoff GetInstanceFromDataRow(DataRow x) {
			//long provNum=x.Field<long>("ProvNum");
			//string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new Writeoff() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=-x.Field<double>("WriteOffs"),
				Count=0, //count procedures, not writeoffs.
								 //SeriesName=seriesName,
				ClinicNum=x.Field<long>("ClinicNum"),
				IsCap=PIn.Bool(x.Field<string>("IsCap")),

			};
		}
	}

	public class Writeoff:GraphQuantityOverTime.GraphDataPointClinic {
		public bool IsCap;
	}
}