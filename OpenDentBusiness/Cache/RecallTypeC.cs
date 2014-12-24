using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class RecallTypeC {
		private static List<RecallType> _listt;
		private static object _lock=new object();

		///<summary>A list of all recall Types.</summary>
		public static List<RecallType> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lock) {
					_listt=value;
				}
			}
		}

		///<summary>Thread-safe.  Returns a copy of the currently cached long list of objects.</summary>
		public static List<RecallType> GetListt() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listt==null;
			}
			if(hasNullList) {
				RecallTypes.RefreshCache();
			}
			List<RecallType> listRecallTypes=new List<RecallType>();
			lock(_lock) {
				if(_listt!=null) {
					listRecallTypes.AddRange(_listt);
				}
			}
			return listRecallTypes;
		}

	}
}
