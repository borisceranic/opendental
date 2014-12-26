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
				//TODO: Add the following comment to the summary if we decide to not use GetListLong() here:
				//Use GetListLong() instead of this getter when needing access to this list from the OpenDentBusiness project.
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
				//TODO: Add the following comment to the summary if we decide to not use GetListShort() here:
				//Use GetListShort() instead of this getter when needing access to this list from the OpenDentBusiness project.
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
				listProvs.AddRange(_listLong);
			}
			return listProvs;
		}

		///<summary>This is the list used most often. It does not include hidden providers.</summary>
		public static List<Provider> GetListShort() {
			bool hasNullList=false;
			lock(_lock) {
				if(_listShort==null) {
					hasNullList=true;
				}
			}
			//If this is first-time access then the cache will be null.  Only do the initial RefreshCache when necessary.
			if(hasNullList) {
				//RefreshCache should never be locked because it contains database I/O.
				Providers.RefreshCache();//Eventually calls ListShort's setter which is thread safe.
			}
			List<Provider> listProvs=new List<Provider>();
			lock(_lock) {
				listProvs.AddRange(_listShort);
			}
			return listProvs;
		}
	}
}
