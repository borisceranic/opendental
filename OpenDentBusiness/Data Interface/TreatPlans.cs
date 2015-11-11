using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TreatPlans {

		///<summary>Gets all Saved TreatPlans for a given Patient, ordered by date.</summary>
		public static List<TreatPlan> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan "
			  +"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND TPStatus=0 "//Saved
				+"ORDER BY DateTP";
			return Crud.TreatPlanCrud.SelectMany(command);
		}

		public static List<TreatPlan> GetAllForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan "
				+"WHERE PatNum="+POut.Long(patNum)+" ";
			return Crud.TreatPlanCrud.SelectMany(command);
		}

		///<summary>A single treatplan from the DB.</summary>
		public static TreatPlan GetOne(long treatPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TreatPlan>(MethodBase.GetCurrentMethod(),treatPlanNum);
			}
			return Crud.TreatPlanCrud.SelectOne(treatPlanNum);
		}

		///<summary></summary>
		public static void Update(TreatPlan tp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),tp);
				return;
			}
			Crud.TreatPlanCrud.Update(tp);
		}

		///<summary></summary>
		public static long Insert(TreatPlan tp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				tp.TreatPlanNum=Meth.GetLong(MethodBase.GetCurrentMethod(),tp);
				return tp.TreatPlanNum;
			}
			return Crud.TreatPlanCrud.Insert(tp);
		}

		///<summary>Dependencies checked first and throws an exception if any found. So surround by try catch</summary>
		public static void Delete(TreatPlan tp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),tp);
				return;
			}
			//check proctp for dependencies
			string command="SELECT * FROM proctp WHERE TreatPlanNum ="+POut.Long(tp.TreatPlanNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				//this should never happen
				throw new InvalidProgramException(Lans.g("TreatPlans","Cannot delete treatment plan because it has ProcTP's attached"));
			}
			command= "DELETE from treatplan WHERE TreatPlanNum = '"+POut.Long(tp.TreatPlanNum)+"'";
 			Db.NonQ(command);
		}

		public static string GetHashString(TreatPlan tp,List<ProcTP> proclist) {
			//No need to check RemotingRole; no call to db.
			//the key data is a concatenation of the following:
			//tp: Note, DateTP
			//each proctp: Descript,PatAmt
			//The procedures MUST be in the correct order, and we'll use ItemOrder to order them.
			StringBuilder strb=new StringBuilder();
			strb.Append(tp.Note);
			strb.Append(tp.DateTP.ToString("yyyyMMdd"));
			for(int i=0;i<proclist.Count;i++){
				strb.Append(proclist[i].Descript);
				strb.Append(proclist[i].PatAmt.ToString("F2"));
			}
			byte[] textbytes=Encoding.UTF8.GetBytes(strb.ToString());
			//byte[] filebytes = GetBytes(doc);
			//int fileLength = filebytes.Length;
			//byte[] buffer = new byte[textbytes.Length + filebytes.Length];
			//Array.Clone(filebytes,0,buffer,0,fileLength);
			//Array.Clone(textbytes,0,buffer,fileLength,textbytes.Length);
			HashAlgorithm algorithm = MD5.Create();
			byte[] hash = algorithm.ComputeHash(textbytes);//always results in length of 16.
			return Encoding.ASCII.GetString(hash);
		}

		///<summary>Finds orphaned TP and TPi procedures and attaches them to new or current TP. Many calls to DB, consider optimizing or calling sparingly.</summary>
		public static void AuditPlans(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			List<Procedure> listProcsAll=Procedures.GetProcsByStatusForPat(patNum,new[] {ProcStat.TP,ProcStat.TPi});
			//Psuedo# Code:
			//find all TP procedures.
			//Find procedures marked TP
			//  Find/Make active plan
			//    Add missing TP procs to active plan
			//delete orphaned treatplanattaches
			//select all treatplanattaches for patient
			//find orphaned TPi procedures
			//Find/Create Unassigned Procedures TP
			//Assign orphaned procedures to unassigned procedures TP.
			//Remove procedures from the orphaned plan if they are attached to other plans.
			List<Procedure> listProceduresActiveTP=listProcsAll.FindAll(x => x.ProcStatus==ProcStat.TP || x.AptNum>0 || x.PlannedAptNum>0);
			List<TreatPlan> listTreatPlans=TreatPlans.GetAllForPat(patNum);
			#region Active Treatment Plan
			TreatPlan activePlan=listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Active);
			if(listProceduresActiveTP.Count>0) {
				if(activePlan==null) { //there is no active plan, and there should be
					activePlan=new TreatPlan() {
						Heading=Lans.g("TreatPlans","Active Treatment Plan"),
						Note=PrefC.GetString(PrefName.TreatmentPlanNote),
						TPStatus=TreatPlanStatus.Active,
						PatNum=patNum
					};
					activePlan.TreatPlanNum=TreatPlans.Insert(activePlan);
					listTreatPlans.Add(activePlan);
				}
				List<TreatPlanAttach> listActiveTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(activePlan.TreatPlanNum);
				//Add TreatPlanAttaches for procedures that need it.
				foreach(Procedure tpProc in listProceduresActiveTP) {
					if(listActiveTreatPlanAttaches.Any(x => x.ProcNum==tpProc.ProcNum)) {
						continue; //TP procedure is already attached to the active plan.
					}
					TreatPlanAttaches.Insert(new TreatPlanAttach() {ProcNum=tpProc.ProcNum,TreatPlanNum=activePlan.TreatPlanNum,Priority=tpProc.Priority});
				}
				//Delete TreatPlanAttaches for procedures that are no longer TP status.
				foreach(TreatPlanAttach tpa in listActiveTreatPlanAttaches) {
					Procedure proc=listProceduresActiveTP.FirstOrDefault(x => x.ProcNum==tpa.ProcNum);
					if(proc!=null && proc.ProcStatus==ProcStat.TPi) {
						Procedure procClone=proc.Copy();
						proc.ProcStatus=ProcStat.TP;
						Procedures.Update(proc,procClone);
						continue;
					}
					TreatPlanAttaches.Delete(tpa.TreatPlanAttachNum);
				}
			}
			#endregion
			#region Orphans and Unassigned TP
			TreatPlanAttaches.DeleteOrphaned();
			List<TreatPlanAttach> listTPAttaches=TreatPlanAttaches.GetAllForPatNum(patNum);
			//Find all TPi Procedures that do not have a treatplanattach
			List<Procedure> listProcTPiOrphans=Procedures.GetProcsByStatusForPat(patNum,new[] { ProcStat.TPi }).FindAll(x => listTPAttaches.All(y => y.ProcNum!=x.ProcNum));
			TreatPlan unassignedPlan=listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Inactive && x.Heading==Lans.g("TreatPlans","Unassigned"));
			if(listProcTPiOrphans.Count>0) {
				if(unassignedPlan==null) {
					unassignedPlan=new TreatPlan() {
						Heading=Lans.g("TreatPlans","Unassigned"),
						Note=PrefC.GetString(PrefName.TreatmentPlanNote),
						TPStatus=TreatPlanStatus.Inactive,
						PatNum=patNum
					};
					unassignedPlan.TreatPlanNum=TreatPlans.Insert(unassignedPlan);
					listTreatPlans.Add(unassignedPlan);
				}
				listProcTPiOrphans
					.ForEach(x => TreatPlanAttaches.Insert(new TreatPlanAttach() {TreatPlanNum=unassignedPlan.TreatPlanNum,ProcNum=x.ProcNum,Priority=x.Priority}));
				return;
			}
			if(unassignedPlan==null) {
				return;
			} 
			//remove procedures from the orphaned treatment plan if they are attached to other TPs
			List<TreatPlanAttach> listTPAOrphans=listTPAttaches.FindAll(x => x.TreatPlanNum==unassignedPlan.TreatPlanNum);
			TreatPlanAttaches.DeleteMany(listTPAOrphans.FindAll(x => listTPAttaches.FindAll(y => x.ProcNum==y.ProcNum).Count>1));
			if(TreatPlanAttaches.GetAllForTreatPlan(unassignedPlan.TreatPlanNum).Count==0) {
				Crud.TreatPlanCrud.Delete(unassignedPlan.TreatPlanNum);
			}
			#endregion
		}

		public static TreatPlan GetUnassigned() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TreatPlan>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM treatplan WHERE TPStatus="+POut.Int((int)TreatPlanStatus.Inactive)+" AND Heading='"+POut.String(Lans.g("TreatPlan","Unassigned"))+"'";
			return Crud.TreatPlanCrud.SelectOne(command)??new TreatPlan();
		}

		public static void SetActive(TreatPlan treatPlanCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),treatPlanCur);
				return;
			}
			//Should only ever update a single TP.
			string command="UPDATE treatplan SET TPStatus="+POut.Int((int)TreatPlanStatus.Inactive)+" "+
			  "WHERE TPStatus="+POut.Int((int)TreatPlanStatus.Active)+" "+
			  "AND TreatPlanNum!="+POut.Long(treatPlanCur.TreatPlanNum);
			Db.NonQ(command);
			//Should only ever update a single TP.
			command="UPDATE treatplan SET TPStatus="+POut.Int((int)TreatPlanStatus.Active)+" "+
			 "WHERE TreatPlanNum="+POut.Long(treatPlanCur.TreatPlanNum);
			Db.NonQ(command);

		}







	}

	

	


}




















