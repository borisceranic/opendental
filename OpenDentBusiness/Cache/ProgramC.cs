using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProgramC {
		///<summary>key:ProgName, value:Program</summary>
		private static Hashtable _hList;
		///<summary>A list of all Program links.</summary>
		private static List<Program> _listt;
		private static object _lockObj=new object();

		///<summary>key:ProgName, value:Program</summary>
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

		///<summary>A list of all Program links.</summary>
		public static List<Program> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>key:ProgName, value:Program</summary>
		public static Hashtable GetHList() {
			bool isHashNull=false;
			lock(_lockObj) {
				if(_hList==null) {
					isHashNull=true;
				}
			}
			if(isHashNull) {
				Programs.RefreshCache();
			}
			Hashtable hashPrograms=new Hashtable();
			lock(_lockObj) {
				//Jordan approved foreach loop for speed purposes.  Looping through a hashtable of 38,000 items using a for loop took ~22,840ms whereas a foreach loop takes ~10ms.
				foreach(DictionaryEntry entry in _hList) {
					hashPrograms.Add(entry.Key,((Program)entry.Value).Copy());
				}
			}
			return hashPrograms;
		}

		///<summary>A list of all Program links.</summary>
		public static List<Program> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Programs.RefreshCache();
			}
			List<Program> listPrograms=new List<Program>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listPrograms.Add(_listt[i].Copy());
				}
			}
			return listPrograms;
		}

		///<summary></summary>
		public static bool HListIsNull() {
			lock(_lockObj) {
				return _hList==null;
			}
		}
		
	}
}
