using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class FeeSchedC {
		///<summary>A list of all feescheds.</summary>
		private static List<FeeSched> _listLong;
		///<summary>A list of feescheds that are not hidden.</summary>
		private static List<FeeSched> _listShort;
		private static object _lock=new object();

		public static List<FeeSched> ListLong {
			get {
				return GetListLong();
			}
			set {
				lock(_lock) {
					_listLong=value;
				}
			}
		}

		public static List<FeeSched> ListShort {
			get {
				return GetListShort();
			}
			set {
				lock(_lock) {
					_listShort=value;
				}
			}
		}

		///<summary>A list of all feescheds.</summary>
		public static List<FeeSched> GetListLong() {
			bool isListNull=false;
			lock(_lock) {
				if(_listLong==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				FeeScheds.RefreshCache();
			}
			List<FeeSched> listFeeScheds=new List<FeeSched>();
			lock(_lock) {
				for(int i=0;i<_listLong.Count;i++) {
					listFeeScheds.Add(_listLong[i].Copy());
				}
			}
			return listFeeScheds;
		}

		///<summary>A list of feescheds that are not hidden.</summary>
		public static List<FeeSched> GetListShort() {
			bool isListNull=false;
			lock(_lock) {
				if(_listShort==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				FeeScheds.RefreshCache();
			}
			List<FeeSched> listFeeScheds=new List<FeeSched>();
			lock(_lock) {
				for(int i=0;i<_listShort.Count;i++) {
					listFeeScheds.Add(_listShort[i].Copy());
				}
			}
			return listFeeScheds;
		}
		
	}
}
