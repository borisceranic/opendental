using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SecurityLogs {

		///<summary>Used when viewing securityLog from the security admin window.  PermTypes can be length 0 to get all types.</summary>
		public static SecurityLog[] Refresh(DateTime dateFrom,DateTime dateTo,Permissions permType,long patNum,long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SecurityLog[]>(MethodBase.GetCurrentMethod(),dateFrom,dateTo,permType,patNum,userNum);
			}
			string command="SELECT securitylog.*,LName,FName,Preferred,MiddleI,LogHash FROM securitylog "
				+"LEFT JOIN patient ON patient.PatNum=securitylog.PatNum "
				+"LEFT JOIN securityloghash ON securityloghash.SecurityLogNum=securitylog.SecurityLogNum "
				+"WHERE LogDateTime >= "+POut.Date(dateFrom)+" "
				+"AND LogDateTime <= "+POut.Date(dateTo.AddDays(1));
			if(patNum !=0) {
				command+=" AND securitylog.PatNum= '"+POut.Long(patNum)+"'";
			}
			if(permType!=Permissions.None) {
				command+=" AND PermType="+POut.Long((int)permType);
			}
			if(userNum!=0) {
				command+=" AND UserNum="+POut.Long(userNum);
			}
			command+=" ORDER BY LogDateTime";
			DataTable table=Db.GetTable(command);
			List<SecurityLog> list=Crud.SecurityLogCrud.TableToList(table);
			for(int i=0;i<list.Count;i++) {
				if(table.Rows[i]["PatNum"].ToString()=="0") {
					list[i].PatientName="";
				}
				else {
					list[i].PatientName=table.Rows[i]["PatNum"].ToString()+"-"
						+Patients.GetNameLF(table.Rows[i]["LName"].ToString()
						,table.Rows[i]["FName"].ToString()
						,table.Rows[i]["Preferred"].ToString()
						,table.Rows[i]["MiddleI"].ToString());
				}
				list[i].LogHash=table.Rows[i]["LogHash"].ToString();
			}
			return list.ToArray();
		}

		///<summary></summary>
		public static long Insert(SecurityLog log){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				log.SecurityLogNum=Meth.GetLong(MethodBase.GetCurrentMethod(),log);
				return log.SecurityLogNum;
			}
			return Crud.SecurityLogCrud.Insert(log);
		}

		//there are no methods for deleting or changing log entries because that will never be allowed.

		///<summary>Used when viewing various audit trails of specific types.  Only implemented Appointments,ProcFeeEdit,InsPlanChangeCarrierName so far. patNum only used for Appointments.  The other two are zero.</summary>
		public static SecurityLog[] Refresh(long patNum,List<Permissions> permTypes,long fKey) {
			//No need to check RemotingRole; no call to db.
			return Refresh(patNum,permTypes,new List<long>(){ fKey });
		}

		///<summary>Used when viewing various audit trails of specific types.  This overload will return security logs for multiple objects (or fKeys).  Typically you will only need a specific type audit log for one type.  However, for things like ortho charts, each row (FK) in the database represents just one part of a larger ortho chart "object".  Thus, to get the full experience of a specific type audit trail window, we need to get security logs for multiple objects (FKs) that comprise the larger object (what the user sees).  Only implemented with ortho chart so far.  FKeys can be null.</summary>
		public static SecurityLog[] Refresh(long patNum,List<Permissions> permTypes,List<long> fKeys) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SecurityLog[]>(MethodBase.GetCurrentMethod(),patNum,permTypes,fKeys);
			}
			string types="";
			for(int i=0;i<permTypes.Count;i++) {
				if(i>0) {
					types+=" OR";
				}
				types+=" PermType="+POut.Long((int)permTypes[i]);
			}
			string command="SELECT * FROM securitylog "
				+"WHERE ("+types+") ";
			if(fKeys!=null && fKeys.Count > 0) {
				command+="AND FKey IN ("+String.Join(",",fKeys)+") ";
			}
			if(patNum!=0) {//appointments
				command+=" AND PatNum="+POut.Long(patNum)+" ";
			}
			command+="ORDER BY LogDateTime";
			return Crud.SecurityLogCrud.SelectMany(command).ToArray();
		}

		///<summary>Returns one SecurityLog from the db.  Called from SecurityLogHashs.CreateSecurityLogHash()</summary>
		public static SecurityLog GetOne(long securityLogNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SecurityLog>(MethodBase.GetCurrentMethod(),securityLogNum);
			}
			return Crud.SecurityLogCrud.SelectOne(securityLogNum);
		}

		///<summary>PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText) {
			//No need to check RemotingRole; no call to db.
			MakeLogEntry(permType,patNum,logText,0,LogSources.None);
		}

		///<summary>Used when the security log needs to be identified by a particular source.  PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText,LogSources logSource) {
			//No need to check RemotingRole; no call to db.
			MakeLogEntry(permType,patNum,logText,0,logSource);
		}

		///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText,long fKey) {
			//No need to check RemotingRole; no call to db.
			MakeLogEntry(permType,patNum,logText,fKey,LogSources.None);
		}

		///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText,long fKey,LogSources logSource) {
			MakeLogEntry(permType,patNum,logText,fKey,logSource,0);
		}

		///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText,long fKey,LogSources logSource,long defNum) {
			//No need to check RemotingRole; no call to db.
			SecurityLog securityLog=new SecurityLog();
			securityLog.PermType=permType;
			if(Security.CurUser!=null) { //if this is generated by Patient Portal web service then we won't have a CurUser set
				securityLog.UserNum=Security.CurUser.UserNum;
			}
			securityLog.LogText=logText;//"From: "+Environment.MachineName+" - "+logText;
			securityLog.CompName=Environment.MachineName;
			securityLog.PatNum=patNum;
			securityLog.FKey=fKey;
			securityLog.LogSource=logSource;
			securityLog.DefNum=defNum;
			securityLog.SecurityLogNum=SecurityLogs.Insert(securityLog);
			//Create a hash of the security log.
			SecurityLogHashes.InsertSecurityLogHash(securityLog.SecurityLogNum);//uses db date/time
		}

		///<summary>Used when making a security log from a remote server, possibly with multithreaded connections.</summary>
		public static void MakeLogEntryNoCache(Permissions permType,long patnum,string logText) {
			SecurityLog securityLog=new SecurityLog();
			securityLog.PermType=permType;
			securityLog.LogText=logText;
			securityLog.CompName=Environment.MachineName;
			securityLog.PatNum=patnum;
			securityLog.FKey=0;
			securityLog.LogSource=LogSources.None;
			securityLog.SecurityLogNum=SecurityLogs.InsertNoCache(securityLog);
			SecurityLogHashes.InsertSecurityLogHashNoCache(securityLog.SecurityLogNum);
		}

		///<summary>Insertion logic that doesn't use the cache. Has special cases for generating random PK's and handling Oracle insertions.</summary>
		public static long InsertNoCache(SecurityLog securityLog) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetLong(MethodBase.GetCurrentMethod(),securityLog);
			}
			return Crud.SecurityLogCrud.InsertNoCache(securityLog);
		}

	}
}