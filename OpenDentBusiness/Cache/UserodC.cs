using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class UserodC {
		private static List<Userod> _listt;
		private static List<Userod> _listShort;
		private static object _lockObj=new object();

		///<summary>A list of all users.</summary>
		public static List<Userod> Listt{
			get{
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}
		
		///<summary>A list of users.  Does not include hidden users.</summary>
		public static List<Userod> ShortList {
			get {
				return GetListShort();
			}
			set {
				lock(_lockObj) {
					_listShort=value;
				}
			}
		}

		///<summary>A list of all users.</summary>
		public static List<Userod> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Userods.RefreshCache();
			}
			List<Userod> listUserods=new List<Userod>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listUserods.Add(_listt[i].Copy());
				}
			}
			return listUserods;
		}
		
		///<summary>A list of users.  Does not include hidden users.</summary>
		public static List<Userod> GetListShort() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listShort==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Userods.RefreshCache();
			}
			List<Userod> listUserods=new List<Userod>();
			lock(_lockObj) {
				for(int i=0;i<_listShort.Count;i++) {
					listUserods.Add(_listShort[i].Copy());
				}
			}
			return listUserods;
		}
		
	}
}
