using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class OperatoryC {
		///<summary></summary>
		private static List<Operatory> _listt;
		///<summary>A list of only those operatories that are visible.</summary>
		private static List<Operatory> _listShort;
		private static object _lockObj=new object();

		public static List<Operatory> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>A list of only those operatories that are visible.</summary>
		public static List<Operatory> ListShort {
			get {
				return GetListShort();
			}
			set {
				lock(_lockObj) {
					_listShort=value;
				}
			}
		}

		public static List<Operatory> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Operatories.RefreshCache();
			}
			List<Operatory> listOperatories=new List<Operatory>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listOperatories.Add(_listt[i].Copy());
				}
			}
			return listOperatories;
		}

		///<summary>A list of only those operatories that are visible.</summary>
		public static List<Operatory> GetListShort() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listShort==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Operatories.RefreshCache();
			}
			List<Operatory> listOperatories=new List<Operatory>();
			lock(_lockObj) {
				for(int i=0;i<_listShort.Count;i++) {
					listOperatories.Add(_listShort[i].Copy());
				}
			}
			return listOperatories;
		}

		

	}
}