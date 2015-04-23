using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EServiceSignals{

		///<summary>Returns dictionary for each service</summary>
		private static Dictionary<eServiceCode,eServiceStatus> GetServiceStatuses() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<eServiceCode,eServiceStatus>>(MethodBase.GetCurrentMethod());
			}
			Dictionary<eServiceCode,eServiceStatus> retVal=new Dictionary<eServiceCode,eServiceStatus>();
			string command="SELECT * FROM eservicesignal WHERE IsProcessed = 0";
			List<EServiceSignal> listSignals=Crud.EServiceSignalCrud.SelectMany(command);
			foreach(eServiceCode sc in Enum.GetValues(typeof(eServiceCode))) {
				retVal.Add(sc,eServiceStatus.Working);
			}
			for(int i=0;i<listSignals.Count;i++) {
				eServiceCode sc;
				if(!Enum.TryParse<eServiceCode>(listSignals[i].ServiceCode.ToString(),out sc)) {
					continue;//must be a signal from a new service not supported by this version of OD.
				}
				retVal[sc]=(eServiceStatus)Math.Max((int)retVal[sc],(int)listSignals[i].Severity);
			}
			return retVal;
		}

		//private static List<EServiceSignal> GetAllForService(eServiceCode serviceCode) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetObject<List<EServiceSignal>>(MethodBase.GetCurrentMethod(),serviceCode);
		//	}
		//	EServiceSignal e=new EServiceSignal();
		//	e.ServiceCode
		//	string command="SELECT * FROM eservicesignal WHERE ServiceCode = "+POut.Int((int)serviceCode);
		//	return Crud.EServiceSignalCrud.SelectMany(command);
		//}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EServiceSignal> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EServiceSignal>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM eservicesignal WHERE PatNum = "+POut.Long(patNum);
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Gets one EServiceSignal from the db.</summary>
		public static EServiceSignal GetOne(long eServiceSignalNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EServiceSignal>(MethodBase.GetCurrentMethod(),eServiceSignalNum);
			}
			return Crud.EServiceSignalCrud.SelectOne(eServiceSignalNum);
		}

		///<summary></summary>
		public static long Insert(EServiceSignal eServiceSignal){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				eServiceSignal.EServiceSignalNum=Meth.GetLong(MethodBase.GetCurrentMethod(),eServiceSignal);
				return eServiceSignal.EServiceSignalNum;
			}
			return Crud.EServiceSignalCrud.Insert(eServiceSignal);
		}

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignal);
				return;
			}
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary></summary>
		public static void Delete(long eServiceSignalNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignalNum);
				return;
			}
			string command= "DELETE FROM eservicesignal WHERE EServiceSignalNum = "+POut.Long(eServiceSignalNum);
			Db.NonQ(command);
		}
		*/



	}
}