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
			string command="SELECT * FROM eservicesignal "
				+"WHERE ServiceCode="+POut.Int((int)serviceCode)+" "
				+"AND SigDateTime BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateStop.Date.AddDays(1))+" "
				+"ORDER BY SigDateTime DESC, Severity DESC";
			return Crud.EServiceSignalCrud.SelectMany(command);
		}

		///<summary>Returns the last known status for the given eService.</summary>
		public static eServiceSignalSeverity GetServiceStatus(eServiceCode serviceCode) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<eServiceSignalSeverity>(MethodBase.GetCurrentMethod(),serviceCode);
			}
			//The only statuses within the eServiceSignalSeverity enum are NotEnabled, Working, and Critical.
			//All other statuses are used for logging purposes and should not be considered within this method.
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)+" "
				+"ORDER BY SigDateTime DESC, Severity DESC "+DbHelper.LimitWhere(1);
			List<EServiceSignal> listSignal=Crud.EServiceSignalCrud.SelectMany(command);
			if(listSignal.Count==0) {
				//NoSignals exist for this service.
				return eServiceSignalSeverity.None;
			}
			return listSignal[0].Severity;
		}

		///<summary>Returns the last known status for the Listener Service.  
		///Returns Critical if a signal has not been entered in the last 5 minutes.
		///Returns Error if there are ANY error signals that have not been processed.</summary>
		public static eServiceSignalSeverity GetListenerServiceStatus() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<eServiceSignalSeverity>(MethodBase.GetCurrentMethod());
			}
			//Additionally, this query will run a subselect to get the count of all unprocessed errors.
			//Running that query as a subselect here simply saves an extra call to the database.
			//This subselect should be fine to run here since the query is limited to one result and the count of unprocessed errors should be small.
			string command="SELECT eservicesignal.*," //eservicesignal.* is required because we will manually call TableToList() later.
					+"(SELECT COUNT(*) FROM eservicesignal WHERE Severity="+POut.Int((int)eServiceSignalSeverity.Error)+" AND IsProcessed=0) PendingErrors "
				+"FROM eservicesignal WHERE ServiceCode="+POut.Int((int)eServiceCode.ListenerService)+" "
				+"AND Severity IN("+POut.Int((int)eServiceSignalSeverity.NotEnabled)+","
					+POut.Int((int)eServiceSignalSeverity.Working)+","
					+POut.Int((int)eServiceSignalSeverity.Error)+","
					+POut.Int((int)eServiceSignalSeverity.Critical)+") "
				+"ORDER BY SigDateTime DESC, Severity DESC "+DbHelper.LimitWhere(1);
			DataTable table=Db.GetTable(command);
			List<EServiceSignal> listSignal=Crud.EServiceSignalCrud.TableToList(table);
			if(listSignal.Count==0) {
				//NoSignals exist for this service.
				return eServiceSignalSeverity.None;
			}
			//The listener service is considered down and in a critical state if there hasn't been a heartbeat in the last 5 minutes.
			//An eSignal severity of "Not Enabled" means the office no longer wants to monitor the status of the service.
			if(listSignal[0].Severity!=eServiceSignalSeverity.NotEnabled && listSignal[0].SigDateTime<DateTime.Now.AddMinutes(-5)) {
				//Office has not disabled the monitoring of the listener service and there hasn't been a heartbeat in the last 5 minutes.
				return eServiceSignalSeverity.Critical;
			}
			//We need to flag the service monitor as Error if there are ANY pending errors.
			if(table.Rows[0]["PendingErrors"].ToString()!="0") {
				return eServiceSignalSeverity.Error;
			}
			return listSignal[0].Severity;
		}

		///<summary></summary>
		public static long Insert(EServiceSignal eServiceSignal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				eServiceSignal.EServiceSignalNum=Meth.GetLong(MethodBase.GetCurrentMethod(),eServiceSignal);
				return eServiceSignal.EServiceSignalNum;
			}
			return Crud.EServiceSignalCrud.Insert(eServiceSignal);
		}

		///<summary>Inserts a healthy heartbeat.</summary>
		public static void InsertHeartbeatForService(eServiceCode serviceCode) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),serviceCode);
				return;
			}
			string command="SELECT * FROM eservicesignal WHERE ServiceCode="+POut.Int((int)serviceCode)
				+" AND Severity IN ("
				+POut.Int((int)eServiceSignalSeverity.NotEnabled)+","
				+POut.Int((int)eServiceSignalSeverity.Working)+","
				+POut.Int((int)eServiceSignalSeverity.Critical)
				+") ORDER BY SigDateTime DESC "+DbHelper.LimitWhere(1);//only select not enabled, working, and critical statuses.
			EServiceSignal eServiceSignalLast=Crud.EServiceSignalCrud.SelectOne(command);
			//If initializing or changing state to working from not working, insert two signals; An anchor and a rolling timestamp.
			if(eServiceSignalLast==null || eServiceSignalLast.Severity!=eServiceSignalSeverity.Working) { //First ever heartbeat or critical which was not previously critical.
				Crud.EServiceSignalCrud.Insert(new EServiceSignal() { ServiceCode=(int)serviceCode,Severity=eServiceSignalSeverity.Working,SigDateTime=DateTime.Now,IsProcessed=true,Description="Heartbeat Anchor" });//anchor heartbeat
				Crud.EServiceSignalCrud.Insert(new EServiceSignal() { ServiceCode=(int)serviceCode,Severity=eServiceSignalSeverity.Working,SigDateTime=DateTime.Now.AddSeconds(1),IsProcessed=true,Description="Heartbeat" });//rolling heartbeat
				return;
			}
			eServiceSignalLast.SigDateTime=DateTime.Now;//succeptible to system clock being different than server time. But since this code is being run from the server, this should never happen.
			Crud.EServiceSignalCrud.Update(eServiceSignalLast);
		}

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignal);
				return;
			}
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary>Sets IsProcessed to true on all eService signals of the passed in severity.</summary>
		public static void ProcessSignalsForSeverity(eServiceSignalSeverity severity) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),severity);
				return;
			}
			string command="UPDATE eservicesignal SET IsProcessed=1 WHERE Severity="+POut.Int((int)severity);
			Db.NonQ(command);
		}

		///<summary>Sets IsProcessed to true on eService signals of Error severity that are within 15 minutes of the passed in DateTime.</summary>
		public static void ProcessErrorSignalsAroundTime(DateTime dateTime) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dateTime);
				return;
			}
			if(dateTime.Year<1880) {
				return;//Nothing to do.
			}
			string command="UPDATE eservicesignal SET IsProcessed=1 "
				+"WHERE Severity="+POut.Int((int)eServiceSignalSeverity.Error)+" "
				+"AND SigDateTime BETWEEN "+POut.DateT(dateTime.AddMinutes(-15))+" AND "+POut.DateT(dateTime.AddMinutes(15));
			Db.NonQ(command);
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