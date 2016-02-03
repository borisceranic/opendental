using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheWriteoff:DashboardCacheWithQuery<Writeoff> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="ProcDate>"+POut.Date(filter.DateFrom)+" AND ProcDate<"+POut.Date(filter.DateTo)+" AND ";
			}
			return
				"SELECT ProcDate,ProvNum,SUM(WriteOff) AS WriteOffs "
				+"FROM claimproc "
				+"WHERE "+where+"claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+" "
				+"GROUP BY ProcDate,ProvNum "
				+"HAVING WriteOffs<>0 ";
		}

		protected override Writeoff GetInstanceFromDataRow(DataRow x) {
			return new Writeoff() {
				ProvNum=x.Field<long>("ProvNum"),
				DateStamp=x.Field<DateTime>("ProcDate"),
				Val=x.Field<double>("WriteOffs"),
			};
		}
	}

	public class Writeoff:GraphQuantityOverTime.GraphDataPointProv { }
}