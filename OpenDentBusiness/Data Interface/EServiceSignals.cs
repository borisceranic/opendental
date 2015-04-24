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
			if(serviceCode==eServiceCode.ListenerService && listSignal[0].SigDateTime<DateTime.Now.AddMinutes(-5)) {
				return eServiceSignalSeverity.Critical;
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

		///<summary></summary>
		public static void Update(EServiceSignal eServiceSignal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eServiceSignal);
				return;
			}
			Crud.EServiceSignalCrud.Update(eServiceSignal);
		}

		///<summary>Inserts an info signal. This will be entered as IsProcessed=true to no user intervention is required.</summary>
		public static void InsertInfoFromListener(string desc,string tag) {
			//No remoting role check necessary.
			InsertSignalFromListener(eServiceSignalSeverity.Info,desc,tag);
		}

		///<summary>Inserts a warning signal. This will be entered as IsProcessed=true to no user intervention is required.</summary>
		public static void InsertWarningFromListener(string desc,string tag) {
			//No remoting role check necessary.
			InsertSignalFromListener(eServiceSignalSeverity.Warning,desc,tag);
		}

		///<summary>Inserts an error signal. This will be entered as IsProcessed=true to no user intervention is required.</summary>
		public static void InsertErrorFromListener(string desc,string tag) {
			//No remoting role check necessary.
			InsertSignalFromListener(eServiceSignalSeverity.Error,desc,tag);
		}

		///<summary>Only used internally for Info, Warning, Error. Working and Critical should use InsertHeartbeatFromListener.</summary>
		private static void InsertSignalFromListener(eServiceSignalSeverity status,string desc,string tag) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),status,desc,tag);
				return;
			}
			//Create a signal with the given inputs. We will make copies of this and insert/update below depending on status.
			EServiceSignal newSignal=new EServiceSignal();
			newSignal.Description=desc;
			newSignal.IsProcessed=true;
			newSignal.ReasonCategory=0;
			newSignal.ReasonCode=0;
			newSignal.ServiceCode=(int)eServiceCode.ListenerService;
			newSignal.Severity=status;
			newSignal.Tag=tag;
			newSignal.SigDateTime=DateTime.Now;
			Insert(newSignal);
		}

		///<summary>Updates local listener heartbeat to a valid working state. Any previous alert indicators in OD will be silenced automatically.</summary>
		public static void SetListenerWorking(string desc) {
			//No remoting role check necessary.
			InsertHeartbeatFromListener(eServiceSignalSeverity.Working,desc);
		}

		///<summary>Updates local listener heartbeat to an invalid critical state. OD will instantly begin seeing alert indicators.</summary>
		public static void SetListenerCritical(string desc) {
			//No remoting role check necessary.
			InsertHeartbeatFromListener(eServiceSignalSeverity.Critical,desc);
		}

		///<summary></summary>
		private static void InsertHeartbeatFromListener(eServiceSignalSeverity status,string desc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),status,desc);
				return;
			}
			//Create a signal with the given inputs. We will make copies of this and insert/update below depending on status.
			EServiceSignal newSignal=new EServiceSignal();
			newSignal.Description=desc;
			newSignal.IsProcessed=status!=eServiceSignalSeverity.Critical;
			newSignal.ReasonCategory=0;
			newSignal.ReasonCode=0;
			newSignal.ServiceCode=(int)eServiceCode.ListenerService;
			newSignal.Severity=status;
			newSignal.Tag="";
			newSignal.SigDateTime=DateTime.Now;
			//Get the previous signal so we can compare it with our new status.
			EServiceSignal lastSignal=Crud.EServiceSignalCrud.SelectOne(
				"SELECT * FROM eservicesignal "+
				"WHERE "+
				//Working or critical only.
				"(Severity="+POut.Int((int)eServiceSignalSeverity.Critical)+" OR Severity="+POut.Int((int)eServiceSignalSeverity.Working)+") "+
				//Listener service only.
				"AND ServiceCode="+POut.Int((int)eServiceCode.ListenerService)+" "+
				//Latest.
				"ORDER BY TimeStamp DESC "+
				"LIMIT 1");
			if(lastSignal==null) { //First ever signal for this service.
				//Insert the original, this will be frozen in time.
				Insert(newSignal);
				//Insert a copy, this will be updated with each subsequent timestamp.
				EServiceSignal currentSignal=newSignal.Copy();
				currentSignal.SigDateTime=newSignal.SigDateTime.AddSeconds(1);
				Insert(currentSignal);
				return;
			}
			if(status==eServiceSignalSeverity.Working) {
				if(lastSignal.Severity==eServiceSignalSeverity.Working) {
					//We were working before and we are still working so just update last working signal with current timestamp.
					lastSignal.SigDateTime=DateTime.Now;
					Update(lastSignal);
					return;
				}
				//We were NOT working before but now we are working.

				//todo: we are now officially working so should we set all previous IsProcessed=true??? 

				//Insert a working signal which will NOT get updated in the future. It will be frozen in time.
				Insert(newSignal);
				//Insert a copy, this will be updated with each subsequent timestamp.
				EServiceSignal currentSignal=newSignal.Copy();
				currentSignal.SigDateTime=newSignal.SigDateTime.AddSeconds(1);
				Insert(currentSignal);

				return;
			}
			else if(status==eServiceSignalSeverity.Critical) {
				if(lastSignal.Severity==eServiceSignalSeverity.Critical) {
					//We were critical before and we are still critical so just update last critical signal with current timestamp.
					lastSignal.SigDateTime=DateTime.Now;
					Update(lastSignal);
					return;
				}
				//We were NOT critical before but now we are critical.
				//Insert a critical signal which will NOT get updated in the future. It will be frozen in time.
				Insert(newSignal);
				//Insert a copy, this will be updated with each subsequent timestamp.
				EServiceSignal currentSignal=newSignal.Copy();
				currentSignal.SigDateTime=newSignal.SigDateTime.AddSeconds(1);
				Insert(currentSignal);

				//todo: set all previous criticals to IsProcessed=true				
				return;
			}
			else { //Some other status so just insert it. Won't affect working/critical flags.
				Insert(newSignal);
			}
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