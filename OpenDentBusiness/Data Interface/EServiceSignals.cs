using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EServiceSignals {
		
		///<summary>returns all eServiceSignals for a given service within the date range, inclusive.</summary>
		public static List<EServiceSignal> GetServiceHistory(eServiceCode serviceCode,DateTime dateStart,DateTime dateStop) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EServiceSignal>>(MethodBase.GetCurrentMethod(),serviceCode,dateStart,dateStop);
			}
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)
				+" AND TimeStamp BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateStop.Date.AddDays(1))+" ORDER BY TimeStamp DESC";
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Ignores eServiceStatus.Info. Returns the last known status for the given eService.</summary>
		public static eServiceSignalSeverity GetServiceStatus(eServiceCode serviceCode) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<eServiceSignalSeverity>(MethodBase.GetCurrentMethod(),serviceCode);
			}
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)
				+" AND Severity!=1 ORDER BY TimeStamp DESC "+DbHelper.LimitWhere(1);//ignore "info" statuses.
			List<EServiceSignal> listSignal=Crud.EServiceSignalCrud.SelectMany(command);
			if(listSignal.Count==0) {
				//NoSignals exist for this service.
				return eServiceSignalSeverity.None;
			}
			//The listener service is considered down and in a critical state if there hasn't been a heartbeat in the last 5 minutes.
			if(serviceCode==eServiceCode.ListenerService && listSignal[0].TimeStamp<DateTime.Now.AddMinutes(-5)) {
				return eServiceSignalSeverity.Critical;
			}
			return listSignal[0].Severity;
		}

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