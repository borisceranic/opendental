using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProviderC {
		private static List<Provider> _listLong;
		private static List<Provider> _listShort;
		///<summary>Thread safe lock object.  Any time you access _listShort or _listLong you MUST wrap the code block with lock(_lock).  Failing to lock will result in a potential for unsafe access by multiple threads at the same time.</summary>
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
				return GetListShort();
			}
			set {
				lock(_lock) {
					_listShort=value;
				}
			}
		}

		///<summary>Rarely used. Includes all providers, even if hidden.</summary>
		public static List<Provider> GetListLong() {
			bool isListNull=false;
			lock(_lock) {
				if(_listLong==null) {
					isListNull=true;
				}
			}
			//If this is first-time access then the cache will be null.  Only do the initial RefreshCache when necessary.
			if(isListNull) {
				//RefreshCache should never be locked because it contains database I/O.
				Providers.RefreshCache();//Eventually calls ListLong's setter which is thread safe.
			}
			List<Provider> listProvs=new List<Provider>();
			lock(_lock) {
				for(int i=0;i<_listLong.Count;i++) {
					listProvs.Add(_listLong[i].Copy());
				}
			}
			return listProvs;
		}

		///<summary>This is the list used most often. It does not include hidden providers.</summary>
		public static List<Provider> GetListShort() {
			bool isListNull=false;
			lock(_lock) {
				if(_listShort==null) {
					isListNull=true;
				}
			}
			//If this is first-time access then the cache will be null.  Only do the initial RefreshCache when necessary.
			if(isListNull) {
				//RefreshCache should never be locked because it contains database I/O.
				Providers.RefreshCache();//Eventually calls ListShort's setter which is thread safe.
			}
			List<Provider> listProvs=new List<Provider>();
			lock(_lock) {
				for(int i=0;i<_listShort.Count;i++) {
					listProvs.Add(_listShort[i].Copy());
				}
			}
			return listProvs;
		}
	}
}
