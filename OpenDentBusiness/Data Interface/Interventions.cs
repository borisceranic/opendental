using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Interventions{
		//If this table type will exist as cached data, uncomment the CachePattern region below.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all Interventions.</summary>
		private static List<Intervention> listt;

		///<summary>A list of all Interventions.</summary>
		public static List<Intervention> Listt{
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
			string command="SELECT * FROM intervention ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Intervention";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.InterventionCrud.TableToList(table);
		}
		#endregion
		*/
		
		///<summary></summary>
		public static long Insert(Intervention intervention) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				intervention.InterventionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),intervention);
				return intervention.InterventionNum;
			}
			return Crud.InterventionCrud.Insert(intervention);
		}

		///<summary></summary>
		public static void Update(Intervention intervention) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),intervention);
				return;
			}
			Crud.InterventionCrud.Update(intervention);
		}

		///<summary></summary>
		public static void Delete(long interventionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),interventionNum);
				return;
			}
			string command= "DELETE FROM intervention WHERE InterventionNum = "+POut.Long(interventionNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static List<Intervention> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Intervention>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM intervention WHERE PatNum = "+POut.Long(patNum);
			return Crud.InterventionCrud.SelectMany(command);
		}

		public static List<Intervention> Refresh(long patNum,InterventionCodeSet codeSet) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Intervention>>(MethodBase.GetCurrentMethod(),patNum,codeSet);
			}
			string command="SELECT * FROM intervention WHERE PatNum = "+POut.Long(patNum)+" AND CodeSet = "+POut.Int((int)codeSet);
			return Crud.InterventionCrud.SelectMany(command);
		}

		///<summary>Gets list of CodeValue strings from interventions with DateEntry in the last year and CodeSet equal to the supplied codeSet.
		///Result list is grouped by CodeValue, CodeSystem even though we only return the list of CodeValues.  However, there are no codes in the
		///EHR intervention code list that conflict between code systems, so we should never have a duplicate code in the returned list.</summary>
		public static List<string> GetAllForCodeSet(InterventionCodeSet codeSet) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),codeSet);
			}
			string command="SELECT CodeValue FROM intervention WHERE CodeSet="+POut.Int((int)codeSet)+" "
				+"AND "+DbHelper.DtimeToDate("DateEntry")+">="+POut.Date(MiscData.GetNowDateTime().AddYears(-1))+" "
				+"GROUP BY CodeValue,CodeSystem";
			return Db.GetListString(command);
		}

		///<summary>Gets one Intervention from the db.</summary>
		public static Intervention GetOne(long interventionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Intervention>(MethodBase.GetCurrentMethod(),interventionNum);
			}
			return Crud.InterventionCrud.SelectOne(interventionNum);
		}

	}
}