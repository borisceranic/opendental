using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class CovSpanC {
		///<summary></summary>
		private static CovSpan[] _list;
		private static object _lock=new object();

		public static CovSpan[] List {
			get {
				return GetList();
			}
			set {
				lock(_lock) {
					_list=value;
				}
			}
		}
		
		///<summary>thread-safe.  Returns a copy of the currently cached list of objects.</summary>
		public static CovSpan[] GetList() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_list==null;
			}
			if(hasNullList) {
				CovSpans.RefreshCache();
			}
			CovSpan[] arrayCovSpans=null;
			lock(_lock) {
				if(_list!=null) {
					arrayCovSpans=new CovSpan[_list.Length];
					_list.CopyTo(arrayCovSpans,0);
				}
			}
			return arrayCovSpans;
		}

	}
}
