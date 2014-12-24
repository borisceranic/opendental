using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProcApptColors{
		#region CachePattern

		///<summary>A list of all ProcApptColors.</summary>
		private static List<ProcApptColor> _listt;
		private static object _lock=new object();

		///<summary>A list of all ProcApptColors.</summary>
		public static List<ProcApptColor> Listt {
			get {
				return GetListLong();
			}
			set {
				lock(_lock) {
					_listt=value;
				}
			}
		}

		///<summary>Thread-safe.  Returns a copy of the currently cached long list of objects.</summary>
		public static List<ProcApptColor> GetListLong() {
			bool hasNullList=false;
			lock(_lock) {
				hasNullList=_listt==null;
			}
			if(hasNullList) {
				ProcApptColors.RefreshCache();
			}
			List<ProcApptColor> listProcApptColors=new List<ProcApptColor>();
			lock(_lock){
				if(_listt!=null) {
					listProcApptColors.AddRange(_listt);
				}
			}
			return listProcApptColors;
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM procapptcolor ORDER BY CodeRange";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ProcApptColor";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			_listt=Crud.ProcApptColorCrud.TableToList(table);
		}
		#endregion

		///<summary></summary>
		public static List<ProcApptColor> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcApptColor>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM procapptcolor WHERE PatNum = "+POut.Long(patNum);
			return Crud.ProcApptColorCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ProcApptColor procApptColor){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				procApptColor.ProcApptColorNum=Meth.GetLong(MethodBase.GetCurrentMethod(),procApptColor);
				return procApptColor.ProcApptColorNum;
			}
			return Crud.ProcApptColorCrud.Insert(procApptColor);
		}

		///<summary></summary>
		public static void Update(ProcApptColor procApptColor){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procApptColor);
				return;
			}
			Crud.ProcApptColorCrud.Update(procApptColor);
		}

		///<summary></summary>
		public static void Delete(long procApptColorNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procApptColorNum);
				return;
			}
			string command= "DELETE FROM procapptcolor WHERE ProcApptColorNum = "+POut.Long(procApptColorNum);
			Db.NonQ(command);
		}

		/*
		///<summary>Gets one ProcApptColor from the db.</summary>
		public static ProcApptColor GetOne(long procApptColorNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ProcApptColor>(MethodBase.GetCurrentMethod(),procApptColorNum);
			}
			return Crud.ProcApptColorCrud.SelectOne(procApptColorNum);
		}*/

		///<summary>Supply code such as D####.  Returns null if no match</summary>
		public static ProcApptColor GetMatch(string procCode) {
			string code1="";
			string code2="";
			List<ProcApptColor> listProcApptColors=ProcApptColors.GetListLong();
			for(int i=0;i<listProcApptColors.Count;i++) {//using public property to trigger refresh if needed.
				if(listProcApptColors[i].CodeRange.Contains("-")) {
					string[] codeSplit=listProcApptColors[i].CodeRange.Split('-');
					code1=codeSplit[0].Trim();
					code2=codeSplit[1].Trim();
				}
				else{
					code1=listProcApptColors[i].CodeRange.Trim();
					code2=listProcApptColors[i].CodeRange.Trim();
				}
				if(procCode.CompareTo(code1)<0 || procCode.CompareTo(code2)>0) {
					continue;
				}
				return listProcApptColors[i];
			}
			return null;
		}




	}
}