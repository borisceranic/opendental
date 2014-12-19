using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProviderC {
		//The comments in ProviderC are relevant for all cache classes. However, it is not necessary to add such comments to each cache class.
		//See wiki [[Programming Pattern - Cache]] for more details on the cache pattern.
		///<summary>Typically holds the "full" list of cached objects. See comments for _lock.</summary>
		private static List<Provider> _listLong;
		///<summary>Typically holds the "short" list of cached objects. See comments for _lock.</summary>
		private static List<Provider> _listShort;
		///<summary>Thread safe lock object. Any time you access _listShort or _listLong you MUST wrap the code block with lock(_lock). Failing to lock will result in a potential for unsafe access by multiple threads at the same time.</summary>
		private static object _lock=new object();

		///<summary>Rarely used. Includes all providers, even if hidden. Thread-safe. Use GetCacheLong() to get the long list of cached objects. This "getter" is only here for backwards compatibility.</summary>
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

		///<summary>This is the list used most often. It does not include hidden providers. Thread-safe. Use GetCacheLong() to get the long list of cached objects. This "getter" is only here for backwards compatibility.</summary>
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
			List<Provider> list=new List<Provider>();
			lock(_lock) {
				if(_listLong!=null) {
					list.AddRange(_listLong);
				}
			}
			return list;
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
