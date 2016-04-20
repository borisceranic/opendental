using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClaimSnapshots{

		///<summary></summary>
		public static List<ClaimSnapshot> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimSnapshot>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM claimsnapshot WHERE PatNum = "+POut.Long(patNum);
			return Crud.ClaimSnapshotCrud.SelectMany(command);
		}

		///<summary>Gets one ClaimSnapshot from the db.</summary>
		public static ClaimSnapshot GetOne(long claimSnapshotNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ClaimSnapshot>(MethodBase.GetCurrentMethod(),claimSnapshotNum);
			}
			return Crud.ClaimSnapshotCrud.SelectOne(claimSnapshotNum);
		}

		///<summary></summary>
		public static long Insert(ClaimSnapshot claimSnapshot){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				claimSnapshot.ClaimSnapshotNum=Meth.GetLong(MethodBase.GetCurrentMethod(),claimSnapshot);
				return claimSnapshot.ClaimSnapshotNum;
			}
			string command="SELECT COUNT(*) FROM claimsnapshot WHERE ProcNum="+POut.Long(claimSnapshot.ProcNum)+" AND ClaimType='"+claimSnapshot.ClaimType+"'";
			if(Db.GetCount(command)!="0") {
				return 0;//Do nothing.
			}
			return Crud.ClaimSnapshotCrud.Insert(claimSnapshot);
		}

		///<summary></summary>
		public static void Update(ClaimSnapshot claimSnapshot){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimSnapshot);
				return;
			}
			Crud.ClaimSnapshotCrud.Update(claimSnapshot);
		}

		///<summary></summary>
		public static void Delete(long claimSnapshotNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimSnapshotNum);
				return;
			}
			Crud.ClaimSnapshotCrud.Delete(claimSnapshotNum);
		}

		///<summary>Creates a snapshot for the claimprocs passed in.  Used for reporting purposes.
		///If used from the eConnector, ignore passed in claimprocs and make snapshots for the entire day of completed procedures.
		///When passing in claimprocs, the implementor will need to ensure that only primary claimprocs are being saved.
		///Only creates snapshots if the feature is enabled and if the claimproc is of certain statuses.</summary>
		public static void CreateClaimSnapshot(List<ClaimProc> listClaimProcs,ClaimSnapshotTrigger triggerType) {
			if(!PrefC.GetBool(PrefName.ClaimSnapshotEnabled)
				|| PIn.Enum<ClaimSnapshotTrigger>(PrefC.GetString(PrefName.ClaimSnapshotTriggerType),true)!=triggerType) 
			{
				return;
			}
			List<Procedure> listCompletedProcs=new List<Procedure>();
			if(triggerType==ClaimSnapshotTrigger.EConnector) {//Don't use the passed in list of claimprocs because they should be null.
				listCompletedProcs=Procedures.GetCompletedForDateRange(DateTime.Today,DateTime.Today);
				listClaimProcs=ClaimProcs.GetForProcsWithOrdinal(listCompletedProcs.Select(x => x.ProcNum).ToList(),1);
			}
			else {
				listCompletedProcs=Procedures.GetProcsFromClaimProcs(listClaimProcs);
			}
			//Loop through all the claimprocs and create a claimsnapshot entry for each.
			for(int i=0;i<listClaimProcs.Count;i++) {
				if(listClaimProcs[i].Status==ClaimProcStatus.CapClaim
					|| listClaimProcs[i].Status==ClaimProcStatus.CapComplete
					|| listClaimProcs[i].Status==ClaimProcStatus.CapEstimate
					|| listClaimProcs[i].Status==ClaimProcStatus.Preauth)
				{
					continue;
				}
				double procFee=0;
				if(listCompletedProcs.Find(x => x.ProcNum==listClaimProcs[i].ProcNum)!=null) {
					procFee=listCompletedProcs.Find(x => x.ProcNum==listClaimProcs[i].ProcNum).ProcFee;
				}
				ClaimSnapshot snapshot=new ClaimSnapshot();
				snapshot.ProcNum=listClaimProcs[i].ProcNum;
				double writeoffAmt=listClaimProcs[i].WriteOff;
				//For the eConnector, only use the WriteOff amount on the claimproc if the claimproc is associated to a claim, as this means that value has been set.
				if(triggerType==ClaimSnapshotTrigger.EConnector 
					&& (listClaimProcs[i].Status!=ClaimProcStatus.NotReceived && listClaimProcs[i].Status!=ClaimProcStatus.Received)) 
				{
					if(listClaimProcs[i].WriteOffEstOverride!=-1) {
						writeoffAmt=listClaimProcs[i].WriteOffEstOverride;
					}
					else {
						writeoffAmt=listClaimProcs[i].WriteOffEst;
					}
				}
				snapshot.Writeoff=writeoffAmt;
				snapshot.InsPayEst=listClaimProcs[i].InsEstTotal;
				snapshot.Fee=procFee;
				snapshot.ClaimProcNum=listClaimProcs[i].ClaimProcNum;
				snapshot.SnapshotTrigger=triggerType;
				ClaimSnapshots.Insert(snapshot);
			}
		}

	}
}