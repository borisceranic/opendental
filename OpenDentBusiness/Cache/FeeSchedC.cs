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

		///<summary>Thread-safe.  Returns a copy of the currently cached long list of objects.</summary>
		public static List<FeeSched> GetListLong() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listLong==null;
			}
			if(hasNullList) {
				FeeScheds.RefreshCache();
			}
			List<FeeSched> listFeeScheds=new List<FeeSched>();
			lock(_lock) {
				if(_listLong!=null) {
					listFeeScheds.AddRange(_listLong);
				}
			}
			return listFeeScheds;
		}

		///<summary>Thread-safe.  Returns a copy of the currently cached long list of objects.</summary>
		public static List<FeeSched> GetListShort() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listShort==null;
			}
			if(hasNullList) {
				FeeScheds.RefreshCache();
			}
			List<FeeSched> listFeeScheds=new List<FeeSched>();
			lock(_lock) {
				if(_listShort!=null) {
					listFeeScheds.AddRange(_listShort);
				}
			}
			return listFeeScheds;
		}
		
	}
}
