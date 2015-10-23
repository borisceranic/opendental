using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Ebills{
		///<summary></summary>
		private static List<Ebill> _listEbills;
		private static object _lockObj=new object();

		public static List<Ebill> List{
			//No need to check RemotingRole; no call to db.
			get {
				return GetList();
			}
			set {
				lock(_lockObj) {
					_listEbills=value;
				}
			}
		}

		public static List<Ebill> GetList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listEbills==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			List<Ebill> listEbills=new List<Ebill>();
			lock(_lockObj) {
				for(int i=0;i<_listEbills.Count;i++) {
					listEbills.Add(_listEbills[i].Copy());
				}
			}
			return listEbills;
		}

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM ebill";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Ebill";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List=Crud.EbillCrud.TableToList(table);
		}

		/*
		 
		///<summary></summary>
		public static List<Ebill> GetForPat(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Ebill>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM ebill WHERE PatNum = "+POut.Long(patNum);
			return Crud.EbillCrud.SelectMany(command);
		}

		///<summary>Gets one Ebill from the db.</summary>
		public static Ebill GetOne(long ebillNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<Ebill>(MethodBase.GetCurrentMethod(),ebillNum);
			}
			return Crud.EbillCrud.SelectOne(ebillNum);
		}

		///<summary></summary>
		public static long Insert(Ebill ebill){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				ebill.EbillNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ebill);
				return ebill.EbillNum;
			}
			return Crud.EbillCrud.Insert(ebill);
		}

		///<summary></summary>
		public static void Update(Ebill ebill){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ebill);
				return;
			}
			Crud.EbillCrud.Update(ebill);
		}
		
		 ///<summary></summary>
		public static void Delete(long ebillNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ebillNum);
				return;
			}
			Crud.EbillCrud.Delete(ebillNum);
		} 
		 
		*/

		public static Ebill GetForClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			List<Ebill> listEbills=GetList();
			for(int i=0;i<listEbills.Count;i++) {
				if(clinicNum==listEbills[i].ClinicNum) {
					return listEbills[i];
				}
			}
			return null;
		}

		public static bool Sync(List<Ebill> listNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew);
			}
			List<Ebill> listDB=Ebills.GetList();
			return Crud.EbillCrud.Sync(listNew,listDB);
		}

	}
}