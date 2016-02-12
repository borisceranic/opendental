using System;
using System.Linq;
using System.Collections.Generic;
using OpenDentBusiness;
using System.Drawing;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheProvider:DashboardCacheBase<Provider> {
		private Dictionary<long,string> _dictProvNames=new Dictionary<long, string>();
		private Dictionary<string,Color> _dictProvColors=new Dictionary<string, Color>();
		protected override List<Provider> GetCache(DashboardFilter filter) {
			List<Provider> list=ProviderC.GetListLong();
			_dictProvNames=list.ToDictionary(x => x.ProvNum,x => string.IsNullOrEmpty(x.Abbr)?x.ProvNum.ToString():x.Abbr);
			_dictProvColors=list.GroupBy(x => x.Abbr).ToDictionary(x => string.IsNullOrEmpty(x.Key) ? x.First().ProvNum.ToString() : x.Key ,x => x.First().ProvColor);
			return list;
		}

		protected override bool AllowQueryDateFilter() {
			return false;
		}

		public string GetProvName(long provNum) {
			string provName;
			if(!_dictProvNames.TryGetValue(provNum,out provName)) {
				return provNum.ToString();
			}
			return provName;
		}

		public Color GetProvColor(string provAbbr) {
			Color provColor;
			if(!_dictProvColors.TryGetValue(provAbbr,out provColor)) {
				return Color.DarkBlue;
			}
			return provColor;
		}
	}
}