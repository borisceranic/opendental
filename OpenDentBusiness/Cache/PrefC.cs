using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness {
	public class PrefC {
		private static Dictionary<string,Pref> _dict;
		private static object _lock=new object();

		///<summary>Key is prefName.  Can't use the enum, because prefs are allowed to be added by outside programmers, and this framework will support those prefs, too.</summary>
		internal static Dictionary<string,Pref> Dict {
			get {
				return GetDict();
			}
			set {
				lock(_lock) {
					_dict=value;
				}
			}
		}

		///<summary>Key is prefName.  Can't use the enum, because prefs are allowed to be added by outside programmers, and this framework will support those prefs, too.</summary>
		private static Dictionary<string,Pref> GetDict() {
			bool isDictNull=false;
			lock(_lock) {
				if(_dict==null) {
					isDictNull=true;
				}
			}
			if(isDictNull) {
				Prefs.RefreshCache();
			}
			Dictionary<string,Pref> dictPrefs;
			lock(_lock) {
				dictPrefs=new Dictionary<string,Pref>(_dict);
			}
			return _dict;
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
			/*  The following is the code exactly as it was:
			if(Dict==null){
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Long(Dict[prefName.ToString()].ValueString);
			*/
			Dictionary<string,Pref> dictPrefs=GetDict();
			if(!dictPrefs.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Long(dictPrefs[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type int32.  Used when the pref is an enumeration, itemorder, etc.  Also used for historical queries in ConvertDatabase.</summary>
		public static int GetInt(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Int(Dict[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type double.</summary>
		public static double GetDouble(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Double(Dict[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type bool.</summary>
		public static bool GetBool(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Bool(Dict[prefName.ToString()].ValueString);
		}

		///<Summary>Gets a pref of type bool, but will not throw an exception if null or not found.  Indicate whether the silent default is true or false.</Summary>
		public static bool GetBoolSilent(PrefName prefName,bool silentDefault) {
			if(Dict==null) {
				return silentDefault;
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				return silentDefault;
			}
			return PIn.Bool(Dict[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type string.</summary>
		public static string GetString(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return Dict[prefName.ToString()].ValueString;
		}

		///<summary>Gets a pref of type string.  Will not throw an exception if null or not found.</summary>
		public static string GetStringSilent(PrefName prefName) {
			if(Dict==null) {
				return "";
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				return "";
			}
			return Dict[prefName.ToString()].ValueString;
		}

		///<summary>Gets a pref of type date.</summary>
		public static DateTime GetDate(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.Date(Dict[prefName.ToString()].ValueString);
		}

		///<summary>Gets a pref of type datetime.</summary>
		public static DateTime GetDateT(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return PIn.DateT(Dict[prefName.ToString()].ValueString);
		}

		///<summary>Gets a color from an int32 pref.</summary>
		public static Color GetColor(PrefName prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName.ToString())) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return Color.FromArgb(PIn.Int(Dict[prefName.ToString()].ValueString));
		}

		///<summary>Used sometimes for prefs that are not part of the enum, especially for outside developers.</summary>
		public static string GetRaw(string prefName) {
			if(Dict==null) {
				Prefs.RefreshCache();
			}
			if(!Dict.ContainsKey(prefName)) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return Dict[prefName].ValueString;
		}

		///<summary>Used by an outside developer.</summary>
		public static bool ContainsKey(string prefName) {
			return Dict.ContainsKey(prefName);
		}

		///<summary>Used by an outside developer.</summary>
		public static bool HListIsNull() {
			bool isDictNull=false;
			lock(_lock) {
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
