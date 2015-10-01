using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProviderErxs{
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all ProviderErxs.</summary>
		private static List<ProviderErx> listt;

		///<summary>A list of all ProviderErxs.</summary>
		public static List<ProviderErx> Listt{
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
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM providererx ORDER BY NationalProviderID";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ProviderErx";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.ProviderErxCrud.TableToList(table);
		}
		#endregion

		///<summary>Gets from db.  Used from FormErxAccess at HQ only.</summary>
		public static List<ProviderErx> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProviderErx>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM providererx WHERE PatNum = "+POut.Long(patNum)+" ORDER BY NationalProviderID";
			return Crud.ProviderErxCrud.SelectMany(command);
		}

		///<summary>Gets one ProviderErx from the cache.</summary>
		public static ProviderErx GetOneForNpi(string npi) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<Listt.Count;i++) {
				if(Listt[i].NationalProviderID==npi) {
					return Listt[i];
				}
			}
			return null;
		}

		///<summary>Gets all ProviderErx which have not yet been sent to HQ.</summary>
		public static List<ProviderErx> GetAllUnsent() {
			//No need to check RemotingRole; no call to db.
			List<ProviderErx> listProvErxUnsent=new List<ProviderErx>();
			for(int i=0;i<Listt.Count;i++) {
				if(!Listt[i].IsSentToHq) {
					listProvErxUnsent.Add(Listt[i]);
				}
			}
			return listProvErxUnsent;
		}

		///<summary></summary>
		public static long Insert(ProviderErx providerErx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				providerErx.ProviderErxNum=Meth.GetLong(MethodBase.GetCurrentMethod(),providerErx);
				return providerErx.ProviderErxNum;
			}
			return Crud.ProviderErxCrud.Insert(providerErx);
		}

		///<summary></summary>
		public static bool Update(ProviderErx providerErx,ProviderErx oldProviderErx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),providerErx,oldProviderErx);
			}
			return Crud.ProviderErxCrud.Update(providerErx,oldProviderErx);
		}

		///<summary>Inserts, updates, or deletes the passed in list against the current cached rows.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ProviderErx> listNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
					return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew);
			}
			return Crud.ProviderErxCrud.Sync(listNew,Listt);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		 
		///<summary></summary>
		public static void Update(ProviderErx providerErx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),providerErx);
				return;
			}
			Crud.ProviderErxCrud.Update(providerErx);
		}

		///<summary>Gets one ProviderErx from the db.</summary>
		public static ProviderErx GetOne(long providerErxNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ProviderErx>(MethodBase.GetCurrentMethod(),providerErxNum);
			}
			return Crud.ProviderErxCrud.SelectOne(providerErxNum);
		}

		///<summary></summary>
		public static void Delete(long providerErxNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),providerErxNum);
				return;
			}
			Crud.ProviderErxCrud.Delete(providerErxNum);
		}
		*/



	}
}