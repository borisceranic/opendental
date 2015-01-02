using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProcedureCodeC {	
		private static List<ProcedureCode> _list;
		private static Hashtable _hList;
		private static object _lockObj=new object();

		public static List<ProcedureCode> Listt {
			get {
				return GetListLong();
			}
			set {
				lock(_lockObj) {
					_list=value;
				}
			}
		}

		///<summary>key:ProcCode, value:ProcedureCode</summary>
		public static Hashtable HList {
			get {
				return GetHList();
			}
			set {
				lock(_lockObj) {
					_hList=value;
				}
			}
		}

		///<summary></summary>
		public static List<ProcedureCode> GetListLong() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				ProcedureCodes.RefreshCache();
			}
			List<ProcedureCode> listProcCodes=new List<ProcedureCode>();
			lock(_lockObj) {
				for(int i=0;i<_list.Count;i++) {
					listProcCodes.Add(_list[i].Copy());
				}
			}
			return listProcCodes;
		}

		///<summary>key:ProcCode, value:ProcedureCode</summary>
		public static Hashtable GetHList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_hList==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				ProcedureCodes.RefreshCache();
			}
			Hashtable hashProcCodes=new Hashtable();
			lock(_lockObj) {
				//Jordan approved foreach loop for speed purposes.  Looping through a hashtable of 38,000 items using a for loop took ~22,840ms whereas a foreach loop takes ~10ms.
				foreach(DictionaryEntry entry in _hList) {
					hashProcCodes.Add(entry.Key,((ProcedureCode)entry.Value).Copy());
				}
			}
			return hashProcCodes;
		}

		
	}
}
