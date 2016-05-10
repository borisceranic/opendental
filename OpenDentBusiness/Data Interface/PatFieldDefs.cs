using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace OpenDentBusiness {
	///<summary></summary>
	public class PatFieldDefs {
		#region CachePattern
		private static List<PatFieldDef> _listLong;
		private static List<PatFieldDef> _listShort;

		///<summary>A list of all allowable patFields.</summary>
		public static List<PatFieldDef> ListLong {
			//No need to check RemotingRole; no call to db.
			get {
				if(_listLong==null) {
					RefreshCache();
				}
				return _listLong;
			}
			set {
				_listLong=value;
			}
		}

		///<summary>A list of patFields that are not hidden.</summary>
		public static List<PatFieldDef> ListShort {
			//No need to check RemotingRole; no call to db.
			get {
				if(_listShort==null) {
					RefreshCache();
				}
				return _listShort;
			}
			set {
				_listShort=value;
			}
		}

		///<summary>Gets a deep copy of patFields that are not hidden.</summary>
		public static List<PatFieldDef> GetListShort() {
			List<PatFieldDef> listPatFieldDefs=new List<PatFieldDef>();
			for(int i=0;i<PatFieldDefs.ListShort.Count;i++) {
				listPatFieldDefs.Add(PatFieldDefs.ListShort[i].Copy());
			}
			return listPatFieldDefs;
		}

		///<summary>Gets a deep copy of all allowable patFields.</summary>
		public static List<PatFieldDef> GetListLong() {
			List<PatFieldDef>  listPatFieldDefs=new List<PatFieldDef>();
			for(int i=0;i<PatFieldDefs.ListLong.Count;i++) {
				listPatFieldDefs.Add(PatFieldDefs.ListLong[i].Copy());
			}
			return listPatFieldDefs;
		}

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM patfielddef ORDER BY ItemOrder";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="PatFieldDef";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<PatFieldDef> listShort=new List<PatFieldDef>();
			List<PatFieldDef> listLong=Crud.PatFieldDefCrud.TableToList(table);
			for(int i=0;i<listLong.Count;i++) {
				if(!listLong[i].IsHidden) {
					listShort.Add(listLong[i]);
				}
			}
			ListLong=listLong;
			ListShort=listShort;
		}
		#endregion

		///<summary>Must supply the old field name so that the patient lists can be updated.</summary>
		public static void Update(PatFieldDef patFieldDef, string oldFieldName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patFieldDef,oldFieldName);
				return;
			}
			Crud.PatFieldDefCrud.Update(patFieldDef);
			string command="UPDATE patfield SET FieldName='"+POut.String(patFieldDef.FieldName)+"' "
				+"WHERE FieldName='"+POut.String(oldFieldName)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(PatFieldDef patFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				patFieldDef.PatFieldDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),patFieldDef);
				return patFieldDef.PatFieldDefNum;
			}
			return Crud.PatFieldDefCrud.Insert(patFieldDef);
		}

		///<summary>Surround with try/catch, because it will throw an exception if any patient is using this def.</summary>
		public static void Delete(PatFieldDef patFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patFieldDef);
				return;
			}
			string command="SELECT LName,FName FROM patient,patfield WHERE "
				+"patient.PatNum=patfield.PatNum "
				+"AND FieldName='"+POut.String(patFieldDef.FieldName)+"'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string s=Lans.g("PatFieldDef","Not allowed to delete. Already in use by ")+table.Rows.Count.ToString()
					+" "+Lans.g("PatFieldDef","patients, including")+" \r\n";
				for(int i=0;i<table.Rows.Count;i++){
					if(i>5){
						break;
					}
					s+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString()+"\r\n";
				}
				throw new ApplicationException(s);
			}
			command="DELETE FROM patfielddef WHERE PatFieldDefNum ="+POut.Long(patFieldDef.PatFieldDefNum);
			Db.NonQ(command);
		}
				
		/// <summary>GetFieldName returns the field name identified by the field definition number passed as a parameter.</summary>
		public static string GetFieldName(long patFieldDefNum) {
			//No need to check RemotingRole; no call to db.
			List<PatFieldDef> listPatDefs=GetListShort();
			for(int i=0;i<listPatDefs.Count;i++) {
				if(listPatDefs[i].PatFieldDefNum==patFieldDefNum) {
					return listPatDefs[i].FieldName;
				}
			}
			return "";
		}

		/// <summary>GetPickListByFieldName returns the pick list identified by the field name passed as a parameter.</summary>
		public static string GetPickListByFieldName(string FieldName) {
			//No need to check RemotingRole; no call to db.
			List<PatFieldDef> listPatDefs=GetListShort();
			for(int i=0;i<listPatDefs.Count;i++) {
				if(listPatDefs[i].FieldName==FieldName) {
					return listPatDefs[i].PickList;
				}
			}
			return "";
		}

		///<summary>Sync pattern, must sync entire table. Probably only to be used in the master problem list window.</summary>
		public static void Sync(List<PatFieldDef> listDefs,List<PatFieldDef> listDefsOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listDefs,listDefsOld);
				return;
			}
			Crud.PatFieldDefCrud.Sync(listDefs,listDefsOld);
		}
	}
}