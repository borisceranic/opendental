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
			bool hasNullList=false;
			lock(_lock) {
				if(_hList==null) {
					hasNullList=true;
				}
			}
			if(hasNullList) {
				ProcedureCodes.RefreshCache();
			}
			List<ProcedureCode> listProcedureCodes=new List<ProcedureCode>();
			lock(_lock) {
				listProcedureCodes.AddRange(_list);
			}
			return listProcedureCodes;
		}

		///<summary>key:ProcCode, value:ProcedureCode</summary>
		public static Hashtable GetHList() {
			bool hasNullList=false;
			lock(_lock) {
				if(_hList==null) {
					hasNullList=true;
				}
			}
			if(hasNullList) {
				ProcedureCodes.RefreshCache();
			}
			Hashtable hashProcedureCodes;
			lock(_lock) {
				hashProcedureCodes=new Hashtable(_hList);
			}
			return hashProcedureCodes;
		}

		
	}
}
