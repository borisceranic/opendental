using System.Collections.Generic;
using System.Linq;
using OpenDentBusiness;

namespace OpenDental {
	public class ProcedureL {
		///<summary>Sets all procedures for apt (or in listProcsForAppt) complete.  Flags procedures as CPOE as needed (when prov logged in).  Makes a log
		///entry for each completed proc.  Then fires the CompleteProcedure automation trigger.
		///<para>If the procs to be set complete already exist in the db and are associated to apt, do not set listProcsForAppt, they will be pulled
		///from/updated to the db.</para>
		///<para>If procs are not already associated with the appointment in the db, e.g. from FormApptEdit since they have not been synched yet, set
		///listProcsForAppt equal to the list of procedures that are selected in the grid in FormApptEdit.  The proc changes will not be saved to the db,
		///but will be reflected in the returned list of procs for synching later.</para></summary>
		public static List<Procedure> SetCompleteInAppt(Appointment apt,List<InsPlan> PlanList,List<PatPlan> patPlans,long siteNum,
			int patientAge,List<InsSub> subList,List<Procedure> listProcsInAppt=null)
		{
			bool isDbUpdate=listProcsInAppt==null;
			if(isDbUpdate) {
				listProcsInAppt=Procedures.GetProcsForSingle(apt.AptNum,false);
			}
			if(listProcsInAppt.Count==0) {
				return listProcsInAppt;//Nothing to do.
			}
			//Flag all the procedures as CPOE if the provider of the procedure is currently logged in.  
			//We have to do this here and not within SetCompleteInAppt() due to RemotingRole checks.
			if(Userods.IsUserCpoe(Security.CurUser)) {
				//Only change the status of IsCpoe to true.  Never set it back to false for any reason.  Once true, always true.
				listProcsInAppt.ForEach(x => x.IsCpoe=true);
			}
			listProcsInAppt=Procedures.SetCompleteInAppt(apt,PlanList,patPlans,siteNum,patientAge,listProcsInAppt,subList,isDbUpdate);
			listProcsInAppt.ForEach(x => LogProcComplCreate(apt.PatNum,x,x.ToothNum));
			if(Programs.UsingOrion) {
				OrionProcs.SetCompleteInAppt(listProcsInAppt);
			}
			//automation
			AutomationL.Trigger(AutomationTrigger.CompleteProcedure,listProcsInAppt.Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).ToList(),apt.PatNum);
			return listProcsInAppt;
		}

		///<summary>Returns empty string if no duplicates, otherwise returns duplicate procedure information.  In all places where this is called, we are guaranteed to have the eCW bridge turned on.  So this is an eCW peculiarity rather than an HL7 restriction.  Other HL7 interfaces will not be checking for duplicate procedures unless we intentionally add that as a feature later.</summary>
		public static string ProcsContainDuplicates(List<Procedure> procs) {
			bool hasLongDCodes=false;
			HL7Def defCur=HL7Defs.GetOneDeepEnabled();
			if(defCur!=null) {
				hasLongDCodes=defCur.HasLongDCodes;
			}
			string info="";
			List<Procedure> procsChecked=new List<Procedure>();
			for(int i=0;i<procs.Count;i++) {
				Procedure proc=procs[i];
				ProcedureCode procCode=ProcedureCodes.GetProcCode(procs[i].CodeNum);
				string procCodeStr=procCode.ProcCode;
				if(procCodeStr.Length>5
					&& procCodeStr.StartsWith("D")
					&& !hasLongDCodes)
				{
					procCodeStr=procCodeStr.Substring(0,5);
				}
				for(int j=0;j<procsChecked.Count;j++) {
					Procedure procDup=procsChecked[j];
					ProcedureCode procCodeDup=ProcedureCodes.GetProcCode(procsChecked[j].CodeNum);
					string procCodeDupStr=procCodeDup.ProcCode;
					if(procCodeDupStr.Length>5
						&& procCodeDupStr.StartsWith("D")
						&& !hasLongDCodes)
					{
						procCodeDupStr=procCodeDupStr.Substring(0,5);
					}
					if(procCodeDupStr!=procCodeStr) {
						continue;
					}
					if(procDup.ToothNum!=proc.ToothNum) {
						continue;
					}
					if(procDup.ToothRange!=proc.ToothRange) {
						continue;
					}
					if(procDup.ProcFee!=proc.ProcFee) {
						continue;
					}
					if(procDup.Surf!=proc.Surf) {
						continue;
					}
					if(info!="") {
						info+=", ";
					}
					info+=procCodeDupStr;
				}
				procsChecked.Add(proc);
			}
			if(info!="") {
				info=Lan.g("ProcedureL","Duplicate procedures")+": "+info;
			}
			return info;
		}

		///<summary>Creates securitylog entry for a completed procedure.  Set toothNum to empty string and it will be omitted from the log entry. toothNums can be null or empty.</summary>
		public static void LogProcComplCreate(long patNum,Procedure procCur,string toothNums) {
			//No need to check RemotingRole; no call to db.
			if(procCur==null) {
				return;//Nothing to do.  Should never happen.
			}
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procCur.CodeNum);
			string logText=procCode.ProcCode+", ";
			if(toothNums!=null && toothNums.Trim()!="") {
				logText+=Lans.g("Procedures","Teeth")+": "+toothNums+", ";
			}
			logText+=Lans.g("Procedures","Fee")+": "+procCur.ProcFee.ToString("F")+", "+procCode.Descript;
			SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,patNum,logText);
		}

	}
}
