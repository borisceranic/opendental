using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProviderC {
		private static List<Provider> _listLong;
		private static List<Provider> _listShort;
		private static object _lock=new object();

		public static List<Provider> ListLong {
			get {
				return GetListLong();
			}
			set {
				lock(_lock) {
					_listLong=value;
				}
			}
		}

		public static List<Provider> ListShort {
			get {
				if(_listShort==null) {
					Providers.RefreshCache();
				}
				return _listShort;
			}
			set {
				lock(_lock) {
					_listShort=value;
				}
			}
		}

		///<summary>Thread-safe. Returns a copy of the currently cached long list of objects.</summary>
		public static List<Provider> GetListLong() {
			//If this is first-time access then the cache will be null. Check for null do the initial RefreshCache when necessary.
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listLong==null;
			}
			if(hasNullList) {
				Providers.RefreshCache();
			}
			List<Provider> listProvs=new List<Provider>();
			lock(_lock) {
				if(_listLong!=null) {
					listProvs.AddRange(_listLong);
				}
			}
			return listProvs;
		}

		///<summary>Thread-safe. Returns a copy of the currently cached short list of objects.</summary>
		public static List<Provider> GetListShort() {
			//If this is first-time access then the cache will be null. Check for null do the initial RefreshCache when necessary.
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listShort==null;
			}
			if(hasNullList) {
				Providers.RefreshCache();
			}
			List<Provider> listProvs=new List<Provider>();
			lock(_lock) {
				if(_listShort!=null) {
					listProvs.AddRange(_listShort);
				}
			}
			return listProvs;
		}
	}
}
