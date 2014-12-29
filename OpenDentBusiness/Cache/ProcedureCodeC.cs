using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProcedureCodeC {	
		private static List<ProcedureCode> _list;
		private static Hashtable _hList;
		private static object _lock=new object();

		public static List<ProcedureCode> Listt {
			get {
				return GetListLong();
			}
			set {
				lock(_lock) {
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
				lock(_lock) {
					_hList=value;
				}
			}
		}

		///<summary></summary>
		public static List<ProcedureCode> GetListLong() {
			bool isListNull=false;
			lock(_lock) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				ProcedureCodes.RefreshCache();
			}
			List<ProcedureCode> listProcCodes=new List<ProcedureCode>();
			lock(_lock) {
				listProcCodes.AddRange(_list);
			}
			return listProcCodes;
		}

		///<summary>key:ProcCode, value:ProcedureCode</summary>
		public static Hashtable GetHList() {
			bool isListNull=false;
			lock(_lock) {
				if(_hList==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				ProcedureCodes.RefreshCache();
			}
			Hashtable hashProcCodes;
			lock(_lock) {
				hashProcCodes=new Hashtable(_hList);
			}
			return hashProcCodes;
		}

		
	}
}
