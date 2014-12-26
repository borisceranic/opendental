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

		///<summary>Thread-safe.  Returns a copy of the currently cached long lists of objects.</summary>
		public static List<ProcedureCode> GetListLong() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_list==null;
			}
			if(hasNullList) {
				ProcedureCodes.RefreshCache();
			}
			List<ProcedureCode> listProcedureCodes=new List<ProcedureCode>();
			lock(_lock) {
				if(_list!=null) {
					listProcedureCodes.AddRange(_list);
				}
			}
			return listProcedureCodes;
		}

		///<summary>Thread-safe.  Returns a copy of the currently cached long list of objects.</summary>
		public static Hashtable GetHList() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_hList==null;
			}
			if(hasNullList) {
				ProcedureCodes.RefreshCache();
			}
			Hashtable hashProcedureCodes=new Hashtable();
			List<ProcedureCode> listProcedureCodes=GetListLong();
			lock(_lock) {
				if(_list!=null) {
					for(int i=0;i<listProcedureCodes.Count;i++) {
						try {//Reconstruct the hash table from listProcedureCodes because I don't know how to deep copy a hash table.
							hashProcedureCodes.Add(listProcedureCodes[i].ProcCode,listProcedureCodes[i].Copy());
						}
						catch {//in case of duplicate in db
						}
					}
				}
			}
			return hashProcedureCodes;
		}

		
	}
}
