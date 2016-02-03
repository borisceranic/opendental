using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheNewPatient:DashboardCacheWithQuery<NewPatient> {
		protected override string GetCommand(DashboardFilter filter) {
			//todo: this should use patient.DateFirstVisit instead. Much faster but this would require some development and a convert script.
			return
				"SELECT PatNum, MIN(ProcDate) FirstProc "
				+"FROM procedurelog USE INDEX(indexPNPSCN) "
				+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"GROUP BY PatNum";
		}

		protected override NewPatient GetInstanceFromDataRow(DataRow x) {
			return new NewPatient() {
				DateStamp=x.Field<DateTime>("FirstProc"),
				Val=1, //Each row counts as 1.
				SeriesName="All", //Only 1 series.
			};
		}

		protected override bool AllowQueryDateFilter() {
			return false;
		}
	}

	public class NewPatient:GraphQuantityOverTime.GraphPointBase { }
}