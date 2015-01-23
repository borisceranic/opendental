using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class AppointmentTypes {

		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all AppointmentTypes.</summary>
		private static List<AppointmentType> listt;

		///<summary>A list of all AppointmentTypes.</summary>
		public static List<AppointmentType> Listt {
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM appointmenttype ORDER BY ItemOrder";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="AppointmentType";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			listt=Crud.AppointmentTypeCrud.TableToList(table);
		}

		///<summary>Returns a deep copy of Listt.</summary>
		public static List<AppointmentType> GetListt() {
			List<AppointmentType> listApptTypes=new List<AppointmentType>();
			for(int i=0;i<AppointmentTypes.Listt.Count;i++) {//Gets a deep copy of the cache.
				listApptTypes.Add(AppointmentTypes.Listt[i].Clone());
			}
			return listApptTypes;
		}

		#endregion

		#region Sync Pattern

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static void Sync(List<AppointmentType> listNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew);//never pass DB list through the web service
				return;
			}
			string command="SELECT * FROM appointmenttype ORDER BY ItemOrder";
			List<AppointmentType> listApptTypes=Crud.AppointmentTypeCrud.SelectMany(command);
			Crud.AppointmentTypeCrud.Sync(listNew,listApptTypes);
		}

		#endregion

		///<summary>Gets one AppointmentType from the cache.</summary>
		public static AppointmentType GetOne(long appointmentTypeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<AppointmentType>(MethodBase.GetCurrentMethod(),appointmentTypeNum);
			}
			for(int i=0;i<Listt.Count;i++) {
				if(Listt[i].AppointmentTypeNum==appointmentTypeNum) {
					return Listt[i];
				}
			}
			return null;
		}

		///<summary></summary>
		public static long Insert(AppointmentType appointmentType){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				appointmentType.AppointmentTypeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),appointmentType);
				return appointmentType.AppointmentTypeNum;
			}
			return Crud.AppointmentTypeCrud.Insert(appointmentType);
		}

		///<summary></summary>
		public static void Update(AppointmentType appointmentType){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),appointmentType);
				return;
			}
			Crud.AppointmentTypeCrud.Update(appointmentType);
		}

		///<summary>Surround with try catch.</summary>
		public static void Delete(long appointmentTypeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),appointmentTypeNum);
				return;
			}
			string s=AppointmentTypes.CheckInUse(GetOne(appointmentTypeNum));
			if(s!="") {
				throw new ApplicationException(Lans.g("AppointmentTypes",s));
			}
			string command="DELETE FROM appointmenttype WHERE AppointmentTypeNum = "+POut.Long(appointmentTypeNum);
			Db.NonQ(command);
		}

		///<summary>Used when attempting to delete.</summary>
		public static string CheckInUse(AppointmentType appointmentType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),appointmentType);
			}
			string command="SELECT COUNT(*) FROM appointment WHERE AppointmentTypeNum = "+POut.Long(appointmentType.AppointmentTypeNum);
			if(PIn.Int(Db.GetCount(command))>0) {
				return "Not allowed to delete appointment types that are in use on an appointment.";
			};
			return "";
		}

		public static int SortItemOrder(AppointmentType a1,AppointmentType a2) {
			if(a1.ItemOrder!=a2.ItemOrder){
				return a1.ItemOrder.CompareTo(a2.ItemOrder);
			}
			return a1.AppointmentTypeNum.CompareTo(a2.AppointmentTypeNum);
		}

		///<summary>Returns true if all members are the same.</summary>
		public static bool Compare(AppointmentType a1,AppointmentType a2) {
			if(a1.AppointmentTypeColor==a2.AppointmentTypeColor
				&& a1.AppointmentTypeName==a2.AppointmentTypeName
				&& a1.IsHidden==a2.IsHidden
				&& a1.ItemOrder==a2.ItemOrder)
			{
				return true;
			}
			return false;
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/**/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		*/



	}
}