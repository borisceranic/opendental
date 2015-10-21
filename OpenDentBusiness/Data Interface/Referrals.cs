using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
///<summary></summary>
	public class Referrals{
		///<summary>All referrals for all patients.  Just as needed.  Cache refresh could be more intelligent and faster.</summary>
		private static Referral[] list;

		public static Referral[] List {
			//No need to check RemotingRole; no call to db.
			get {
				if(list==null) {
					RefreshCache();
				}
				return list;
			}
			set {
				list=value;
			}
		}

		///<summary>Refreshes all referrals for all patients.  Need to rework at some point so less memory is consumed.  Also refreshes dynamically, so no need to invalidate local data.</summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM referral ORDER BY lname";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Referral";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			list=Crud.ReferralCrud.TableToList(table).ToArray();
		}

		///<summary></summary>
		public static void Update(Referral refer) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),refer);
				return;
			}
			Crud.ReferralCrud.Update(refer);
		}

		///<summary></summary>
		public static long Insert(Referral refer) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				refer.ReferralNum=Meth.GetLong(MethodBase.GetCurrentMethod(),refer);
				return refer.ReferralNum;
			}
			return Crud.ReferralCrud.Insert(refer);
		}

		///<summary></summary>
		public static void Delete(Referral refer) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),refer);
				return;
			}
			string command= "DELETE FROM referral "
				+"WHERE referralnum = '"+refer.ReferralNum+"'";
			Db.NonQ(command);
		}

		///<summary>Get all matching rows where input email is found in the Email column.</summary>
		public static List<Referral> GetEmailMatch(string email) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Referral>>(MethodBase.GetCurrentMethod(),email);
			}
			string command= "SELECT * FROM referral "
				+"WHERE IsDoctor=1 AND UPPER(EMail) LIKE '%"+email.ToUpper()+"%'";
			return Crud.ReferralCrud.SelectMany(command);
		}

		private static Referral GetFromList(long referralNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++) {
				if(List[i].ReferralNum==referralNum) {
					return List[i];
				}
			}
			//couldn't find it, so refresh list and try again
			Referrals.RefreshCache();
			for(int i=0;i<List.Length;i++) {
				if(List[i].ReferralNum==referralNum) {
					return List[i];
				}
			}
			return null;
		}

		///<summary>Includes title like DMD on the end.</summary>
		public static string GetNameLF(long referralNum) {
			//No need to check RemotingRole; no call to db.
			if(referralNum==0) {
				return "";
			}
			Referral referral=GetFromList(referralNum);
			if(referral==null) {
				return "";
			}
			string retVal=referral.LName;
			if(referral.FName!="") {
				retVal+=", "+referral.FName;
			}
			if(referral.MName!="") {
				retVal+=" "+referral.MName;
			}
			if(referral.Title !="") {
				retVal+=", "+referral.Title;
			}
			//specialty seems to wordy to add here
			return retVal;
		}

		///<summary>Includes title, such as DMD.</summary>
		public static string GetNameFL(long referralNum) {
			//No need to check RemotingRole; no call to db.
			if(referralNum==0) {
				return "";
			}
			Referral referral=GetFromList(referralNum);
			if(referral==null) {
				return "";
			}
			return referral.GetNameFL();
		}

		///<summary></summary>
		public static string GetPhone(long referralNum) {
			//No need to check RemotingRole; no call to db.
			if(referralNum==0)
				return "";
			for(int i=0;i<List.Length;i++) {
				if(List[i].ReferralNum==referralNum) {
					if(List[i].Telephone.Length==10) {
						return List[i].Telephone.Substring(0,3)+"-"+List[i].Telephone.Substring(3,3)+"-"+List[i].Telephone.Substring(6);
					}
					return List[i].Telephone;
				}
			}
			return "";
		}

		///<summary>Returns a list of Referrals with names similar to the supplied string.  Used in dropdown list from referral field in FormPatientAddAll for faster entry.</summary>
		public static List<Referral> GetSimilarNames(string referralLName){
			//No need to check RemotingRole; no call to db.
			List<Referral> retVal=new List<Referral>();
			for(int i=0;i<List.Length;i++){
				if(List[i].LName.ToUpper().IndexOf(referralLName.ToUpper())==0){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		///<summary>Gets Referral info from memory. Does not make a call to the database unless needed.</summary>
		public static Referral GetReferral(long referralNum) {
			//No need to check RemotingRole; no call to db.
			if(referralNum==0) {
				return null;
			}
			for(int i=0;i<List.Length;i++) {
				if(List[i].ReferralNum==referralNum) {
					return List[i].Copy();
				}
			}
			RefreshCache();//must be outdated
			for(int i=0;i<List.Length;i++) {
				if(List[i].ReferralNum==referralNum) {
					return List[i].Copy();
				}
			}
			throw new ApplicationException("Error.  Referral not found: "+referralNum.ToString());
		}

		///<summary>Gets the first referral "from" for the given patient.  Will return null if no "from" found for patient.</summary>
		public static Referral GetReferralForPat(long patNum) {
			//No need to check RemotingRole; no call to db.
			List<RefAttach> RefAttachList=RefAttaches.Refresh(patNum);
			for(int i=0;i<RefAttachList.Count;i++) {
				if(RefAttachList[i].IsFrom) {
					return GetReferral(RefAttachList[i].ReferralNum);
				}
			}
			return null;
		}

		///<summary>Gets all referrals by RefNum.  Returns an empty list if no matches.</summary>
		public static List<Referral> GetReferrals(List<long> listRefNums) {
			//No need to check RemotingRole; no call to db.
			List<Referral> listRefs=new List<Referral>();
			for(int i=0;i<listRefNums.Count;i++) {
				for(int j=0;j<List.Length;j++) {
					if(listRefNums[i]==List[j].ReferralNum) {
						listRefs.Add(List[j]);
						break;
					}
				}
			}
			return listRefs;
		}

		///<summary>Merges two referrals into a single referral. Returns false if both referrals are the same.</summary>
		public static bool MergeReferrals(long refNumInto,long refNumFrom) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),refNumInto,refNumFrom);
			}
			if(refNumInto==refNumFrom) {
				//Do not merge the same referral onto itself.
				return false;
			}
			string command="UPDATE claim "
				+"SET ReferringProv="+POut.Long(refNumInto)+" "
				+"WHERE ReferringProv="+POut.Long(refNumFrom);
			Db.NonQ(command);
			command="UPDATE refattach "
				+"SET ReferralNum="+POut.Long(refNumInto)+" "
				+"WHERE ReferralNum="+POut.Long(refNumFrom);
			Db.NonQ(command);
			Crud.ReferralCrud.Delete(refNumFrom);
			return true;
		}

		///<summary>Returns the number of refattaches that this referral has.</summary>
		public static int CountReferralAttach(long referralNum) {
			string command="SELECT COUNT(*) FROM refattach "
				+"WHERE ReferralNum="+POut.Long(referralNum);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Used to check if a specialty is in use when user is trying to hide it.</summary>
		public static bool IsSpecialtyInUse(long defNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),defNum);
			}
			string command="SELECT COUNT(*) FROM referral WHERE Specialty="+POut.Long(defNum);
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}
	}
}