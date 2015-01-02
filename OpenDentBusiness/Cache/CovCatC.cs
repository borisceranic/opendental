using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class CovCatC {
		private static List<CovCat> _listt;
		private static List<CovCat> _listShort;
		private static object _lock=new object();

		///<summary>All CovCats</summary>
		public static List<CovCat> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lock) {
					_listt=value;
				}
			}
		}

		///<summary>Only CovCats that are not hidden.</summary>
		public static List<CovCat> ListShort {
			get {
				return GetListShort();
			}
			set {
				lock(_lock) {
					_listShort=value;
				}
			}
		}

		///<summary>All CovCats</summary>
		public static List<CovCat> GetListt() {
			bool isListNull=false;
			lock(_lock) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				CovCats.RefreshCache();
			}
			List<CovCat> listCovCats=new List<CovCat>();
			lock(_lock) {
				for(int i=0;i<_listt.Count;i++) {
					listCovCats.Add(_listt[i].Copy());
				}
			}
			return listCovCats;
		}

		///<summary>Only CovCats that are not hidden.</summary>
		public static List<CovCat> GetListShort() {
			bool isListNull=false;
			lock(_lock) {
				if(_listShort==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				CovCats.RefreshCache();
			}
			List<CovCat> listCovCats=new List<CovCat>();
			lock(_lock) {
				for(int i=0;i<_listShort.Count;i++) {
					listCovCats.Add(_listShort[i].Copy());
				}
			}
			return listCovCats;
		}

		///<summary></summary>
		public static int GetOrderLong(long covCatNum) {
			List<CovCat> listCovCats=GetListt();
			for(int i=0;i<listCovCats.Count;i++) {
				if(covCatNum==listCovCats[i].CovCatNum) {
					return (byte)i;
				}
			}
			return -1;
		}	

	}
}
