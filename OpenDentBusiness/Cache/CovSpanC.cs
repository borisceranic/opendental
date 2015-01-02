using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class CovSpanC {
		///<summary></summary>
		private static CovSpan[] _list;
		private static object _lockObj=new object();

		public static CovSpan[] List {
			get {
				return GetList();
			}
			set {
				lock(_lockObj) {
					_list=value;
				}
			}
		}
		
		///<summary></summary>
		public static CovSpan[] GetList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				CovSpans.RefreshCache();
			}
			CovSpan[] arrayCovSpans;
			lock(_lockObj) {
				arrayCovSpans=new CovSpan[_list.Length];
				for(int i=0;i<_list.Length;i++) {
					arrayCovSpans[i]=_list[i].Copy();
				}
			}
			return arrayCovSpans;
		}

	}
}
