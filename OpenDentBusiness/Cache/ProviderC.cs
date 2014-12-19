using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProviderC {
		private static List<Provider> _listLong;
		private static List<Provider> _listShort;
		private static object _lock=new object();

		///<summary>Rarely used. Includes all providers, even if hidden.</summary>
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

		///<summary>This is the list used most often. It does not include hidden providers.</summary>
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

		public static List<Provider> GetListLong() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listLong==null;
			}
			if(hasNullList) {
				Providers.RefreshCache();
			}
			List<Provider> list=new List<Provider>();
			lock(_lock) {
				if(_listLong!=null) {
					list.AddRange(_listLong);
				}
			}
			return list;
		}

		public static List<Provider> GetListShort() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listShort==null;
			}
			if(hasNullList) {
				Providers.RefreshCache();
			}
			List<Provider> list=new List<Provider>();
			lock(_lock) {
				if(_listShort!=null) {
					list.AddRange(_listShort);
				}
			}
			return list;
		}
	}
}
