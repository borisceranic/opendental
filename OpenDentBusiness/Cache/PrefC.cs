using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class PrefC {
		private static Dictionary<string,Pref> _dict;
		private static object _lockObj=new object();

		///<summary>Key is prefName.  Can't use the enum, because prefs are allowed to be added by outside programmers, and this framework will support those prefs, too.</summary>
		internal static Dictionary<string,Pref> Dict {
			get {
				return GetDict();
			}
			set {
				lock(_lockObj) {
					_dict=value;
				}
			}
		}

		///<summary>Key is prefName.  Can't use the enum, because prefs are allowed to be added by outside programmers, and this framework will support those prefs, too.</summary>
		public static Dictionary<string,Pref> GetDict() {
			bool isDictNull=false;
			lock(_lockObj) {
				if(_dict==null) {
					isDictNull=true;
				}
			}
			if(isDictNull) {
				Prefs.RefreshCache();
			}
			Dictionary<string,Pref> dictPrefs=new Dictionary<string,Pref>();
			lock(_lockObj) {
				//Jordan approved foreach loop for speed purposes.  Looping through a dictionary of 38,000 items using a for loop took ~22,840ms whereas a foreach loop takes ~10ms.
				foreach(KeyValuePair<string,Pref> kv in _dict) {
					dictPrefs.Add(kv.Key,((Pref)kv.Value).Copy());
				}
			}
			return dictPrefs;
		}

		///<summary>This property is just a shortcut to this pref to make typing faster.  This pref is used a lot.</summary>
		public static bool RandomKeys {
			get {
				return GetBool(PrefName.RandomPrimaryKeys);
			}
		}

		///<summary>This property is just a shortcut to this pref to make typing faster.  This pref is used a lot.</summary>
		public static bool AtoZfolderUsed {
			get {
				return GetBool(PrefName.AtoZfolderUsed);
			}
		}

		///<summary>Gets a pref of type long.</summary>
		public static long GetLong(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Long(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type int32.  Used when the pref is an enumeration, itemorder, etc.  Also used for historical queries in ConvertDatabase.</summary>
		public static int GetInt(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Int(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type byte.  Used when the pref is a very small integer (0-255).</summary>
		public static byte GetByte(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Byte(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type double.</summary>
		public static double GetDouble(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Double(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type bool.</summary>
		public static bool GetBool(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Bool(dictPrefs[prefName.ToString()].ValueString);
		}

		///<Summary>Gets a pref of type bool, but will not throw an exception if null or not found.  Indicate whether the silent default is true or false.</Summary>
		public static bool GetBoolSilent(PrefName prefName,bool silentDefault) {
			if(HListIsNull()) {
				return silentDefault;
			}
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				return silentDefault;
			}
			return PIn.Bool(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type string.</summary>
		public static string GetString(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return dictPrefs[prefName.ToString()].ValueString;
		}

		///<summary>Gets a pref of type string without using the cache.</summary>
		public static string GetStringNoCache(PrefName prefName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),prefName);
			}
			string command="SELECT ValueString FROM preference WHERE PrefName='"+POut.String(prefName.ToString())+"'";
			return Db.GetScalar(command);
		}

		///<summary>Gets a pref of type string.  Will not throw an exception if null or not found.</summary>
		public static string GetStringSilent(PrefName prefName) {
			if(HListIsNull()) {
				return "";
			}
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				return "";
			}
			return dictPrefs[prefName.ToString()].ValueString;
		}

		///<summary>Gets a pref of type date.</summary>
		public static DateTime GetDate(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Date(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type datetime.</summary>
		public static DateTime GetDateT(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.DateT(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a color from an int32 pref.</summary>
		public static Color GetColor(PrefName prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return Color.FromArgb(PIn.Int(dictPrefs[prefName.ToString()].ValueString));
		}

		///<summary>Used sometimes for prefs that are not part of the enum, especially for outside developers.</summary>
		public static string GetRaw(string prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName)) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return dictPrefs[prefName].ValueString;
		}

		///<summary>Used by an outside developer.</summary>
		public static bool ContainsKey(string prefName) {
			Dictionary<string,Pref> dictPrefs=GetDict();
			return dictPrefs.ContainsKey(prefName);
		}

		///<summary>Used by an outside developer.</summary>
		public static bool HListIsNull() {
			bool isDictNull=false;
			lock(_lockObj) {
				if(_dict==null) {
					isDictNull=true;
				}
			}
			return isDictNull;
		}

		///<summary>Only used in the unit tests.  This quick hack has not been tested.</summary>
		public static Dictionary<string,Pref> DictRef{
			get {
				return Dict;
			}
			set {
				Dict=value;
			}
		}

	}
}
