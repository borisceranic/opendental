using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

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



	}
}