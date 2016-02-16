using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Fees {
		///<summary></summary>
		private static object _lockObj=new object();
		///<summary>This is a dictionary of FeeSchedNums that are linked to a dictionary of CodeNums linked to a list of Fees.
		///This methodology is in place because there are offices with an astronomical amount of fees (e.g. 490K) which takes too long to loop through.
		///The typical cache pattern is to use lists of objects.  See ProviderC.cs for the typical cache pattern.</summary>
		private static Dictionary<long,Dictionary<long,List<Fee>>> _dictFeesByFeeSchedNumsAndCodeNums;
		
		//The entire dictionary cache section is very rare.  
		//It is only present because there are several large offices that have called in complaining about slowness.
		//The slowness is caused by our new thread safe cache pattern coupled with an old looping pattern throughout the Open Dental project.
		//The slowness only shows its face when there is an astronomical (e.g. 490K) amount of fees present.
		//This new dictionary will break up the fees first by a dictionary keyed on FeeSchedNum and then by a dictionary keyed on CodeNum.

		///<summary>Returns a deep copy of the global dictionary of all fees.</summary>
		public static Dictionary<long,Dictionary<long,List<Fee>>> GetDict() {
			//No need to check RemotingRole; no call to db.
			bool isDictNull=false;
			lock(_lockObj) {
				if(_dictFeesByFeeSchedNumsAndCodeNums==null) {
					isDictNull=true;
				}
			}
			if(isDictNull) {
				Fees.RefreshCache();
			}
			Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums=new Dictionary<long,Dictionary<long,List<Fee>>>();
			lock(_lockObj) {
				foreach(KeyValuePair<long,Dictionary<long,List<Fee>>> dictByFeeSchedNum in _dictFeesByFeeSchedNumsAndCodeNums) {
					Dictionary<long,List<Fee>> dictFeesByCodeNums=new Dictionary<long,List<Fee>>();
					foreach(KeyValuePair<long,List<Fee>> kv in dictByFeeSchedNum.Value) {
						List<Fee> listFees=((List<Fee>)kv.Value).Select(x => x.Copy()).ToList();
						dictFeesByCodeNums[kv.Key]=listFees;
					}
					dictFeesByFeeSchedNumsAndCodeNums[dictByFeeSchedNum.Key]=dictFeesByCodeNums;
				}
			}
			return dictFeesByFeeSchedNumsAndCodeNums;
		}

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT fee.* FROM fee INNER JOIN feesched ON feesched.FeeSchedNum=fee.FeeSched WHERE feesched.IsHidden=0";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Fee";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=Crud.FeeCrud.TableToList(table);
			//Organize the list of fees into a complex dictionary by fee schedules then by codes.
			Dictionary<long,Dictionary<long,List<Fee>>> dictFeesBySchedNumsAndCodeNums=GetDictForList(listFees);
			//Add the fees by code nums dictionary to the dictionary of fee schedules but use a lock for thread safety.
			lock(_lockObj) {
				_dictFeesByFeeSchedNumsAndCodeNums=dictFeesBySchedNumsAndCodeNums;
			}
		}

		///<summary>Returns a complex dictionary that is separated exactly like the cached dictionary of fees except with the fees passed in.</summary>
		public static Dictionary<long,Dictionary<long,List<Fee>>> GetDictForList(List<Fee> listFees) {
			Dictionary<long,Dictionary<long,List<Fee>>> dictFeesBySchedNumsAndCodeNums=new Dictionary<long,Dictionary<long,List<Fee>>>();
			//Fill the complex dictionary of fees that is first broken up by fee schedules and then by procedure codes.
			Dictionary<long,List<Fee>> dictFeesForSchedNums=listFees.GroupBy(x => x.FeeSched)
				.ToDictionary(x => x.Key,y => y.ToList());
			//Break up all the fees within each fee schedule into a sub dictionary that is keyed off of CodeNum.
			foreach(KeyValuePair<long,List<Fee>> dictFeesBySchedNum in dictFeesForSchedNums){
				//Get all fees associated to this fee schedule.
				List<Fee> listFeesForSched;
				if(!dictFeesForSchedNums.TryGetValue(dictFeesBySchedNum.Key,out listFeesForSched)) {
					continue;
				}
				//Create a dictionary of Fees associated to this fee schedule for each unique procedure code.
				Dictionary<long,List<Fee>> dictFeesByCodeNums=new Dictionary<long,List<Fee>>();
				//The following linq statement is three times faster than using a for loop.
				dictFeesByCodeNums=listFeesForSched.GroupBy(x => x.CodeNum)
					.ToDictionary(x => x.Key,y => y.ToList());
				//Set the dictionary of all fees for this fee schedule broken up by code num to the outer dictionary's key value (FeeSchedNum).
				dictFeesBySchedNumsAndCodeNums[dictFeesBySchedNum.Key]=dictFeesByCodeNums;
			}
			return dictFeesBySchedNumsAndCodeNums;
		}

		///<summary>Returns a shallow copy of all fees from the given dictionary.</summary>
		public static List<Fee> GetFeesAllShallow(Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=new List<Fee>();
			foreach(KeyValuePair<long,Dictionary<long,List<Fee>>> dictByFeeSchedNums in dictFeesByFeeSchedNumsAndCodeNums) {
				foreach(KeyValuePair<long,List<Fee>> dictFeesByCodeNums in dictByFeeSchedNums.Value) {
					listFees.AddRange((List<Fee>)dictFeesByCodeNums.Value);
				}
			}
			return listFees;
		}

		///<summary>Returns a deep copy of every fee from the given dictionary.</summary>
		public static List<Fee> GetFeesAllDeepCopy(Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees = new List<Fee>();
			foreach(KeyValuePair<long,Dictionary<long,List<Fee>>> dictByFeeSchedNums in dictFeesByFeeSchedNumsAndCodeNums) {
				foreach(KeyValuePair<long,List<Fee>> dictFeesByCodeNums in dictByFeeSchedNums.Value) {
					foreach(Fee fee in (List<Fee>)dictFeesByCodeNums.Value) {
						listFees.Add(fee.Copy());
					}
				}
			}
			return listFees;
		}

		///<summary>Returns all fees associated to the procedure code passed in.</summary>
		public static List<Fee> GetFeesForCode(long codeNum) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=new List<Fee>();
			Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums=GetDict();
			//Loop through each fee schedule and get all fees associated to the passed in procedure code.
			foreach(KeyValuePair<long,Dictionary<long,List<Fee>>> dictByFeeSchedNums in dictFeesByFeeSchedNumsAndCodeNums) {
				Dictionary<long,List<Fee>> dictFeesByCodeNums=dictByFeeSchedNums.Value;
				List<Fee> listFeesForCodeNum;
				if(dictFeesByCodeNums.TryGetValue(codeNum,out listFeesForCodeNum)) {
					//Add all fees to our return value that are associated to this code num.
					listFees.AddRange(listFeesForCodeNum);
				}
			}
			return listFees;
		}

		///<summary>Gets a deep copy of the list of fees associated to the fee schedule and procedure code passed in.
		///Uses the passed dictionary passed in instead of the globally static dictionary.</summary>
		public static List<Fee> GetFeesBySchedAndCode(long feeSchedNum,long codeNum
			,Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) 
		{
			//No need to check RemotingRole; no call to db.
			//Use the dictionary to find a small list of fees associated to this fee schedule and code num.
			Dictionary<long,List<Fee>> dictFeesByCodeNum;
			if(!dictFeesByFeeSchedNumsAndCodeNums.TryGetValue(feeSchedNum,out dictFeesByCodeNum)) {
				return new List<Fee>();
			}
			List<Fee> listFees;
			if(!dictFeesByCodeNum.TryGetValue(codeNum,out listFees)) {
				return new List<Fee>();
			}
			return listFees;
		}

		public static void AddFeeToDict(Fee fee,Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) {
			//No need to check RemotingRole; no call to db.
			if(fee==null || fee.FeeSched==0 || fee.CodeNum==0) {
				return;
			}
			Dictionary<long,List<Fee>> dictFeesByCodeNum;
			if(!dictFeesByFeeSchedNumsAndCodeNums.TryGetValue(fee.FeeSched,out dictFeesByCodeNum)) {
				dictFeesByCodeNum=new Dictionary<long,List<Fee>>();
				dictFeesByFeeSchedNumsAndCodeNums[fee.FeeSched]=dictFeesByCodeNum;
			}
			List<Fee> listFees;
			if(!dictFeesByCodeNum.TryGetValue(fee.CodeNum,out listFees)) {
				listFees=new List<Fee>();
			}
			listFees.Add(fee);
			dictFeesByCodeNum[fee.CodeNum]=listFees;
		}

		public static void RemoveFeeFromDict(Fee fee,Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) {
			//No need to check RemotingRole; no call to db.
			if(fee==null || fee.FeeSched==0 || fee.CodeNum==0) {
				return;
			}
			Dictionary<long,List<Fee>> dictFeesByCodeNum;
			if(!dictFeesByFeeSchedNumsAndCodeNums.TryGetValue(fee.FeeSched,out dictFeesByCodeNum)) {
				return;
			}
			List<Fee> listFees;
			if(!dictFeesByCodeNum.TryGetValue(fee.CodeNum,out listFees)) {
				return;
			}
			listFees.Remove(fee);
			dictFeesByCodeNum[fee.CodeNum]=listFees;
		}

		///<summary></summary>
		public static void Update(Fee fee){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),fee);
				return;
			}
			Crud.FeeCrud.Update(fee);
		}

		///<summary></summary>
		public static long Insert(Fee fee) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				fee.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				fee.FeeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),fee);
				return fee.FeeNum;
			}
			return Crud.FeeCrud.Insert(fee);
		}

		///<summary></summary>
		public static void Delete(Fee fee){
			//No need to check RemotingRole; no call to db.
			Delete(fee.FeeNum);
		}

		///<summary></summary>
		public static void Delete(long feeNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeNum);
				return;
			}
			string command="DELETE FROM fee WHERE FeeNum="+feeNum;
			Db.NonQ(command);
		}

		///<summary>Deletes all fees for the supplied FeeSched that aren't for the HQ clinic.</summary>
		public static void DeleteNonHQFeesForSched(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSchedNum);
				return;
			}
			string command="DELETE FROM fee WHERE FeeSched="+POut.Long(feeSchedNum)+" AND ClinicNum!=0";
			Db.NonQ(command);
		}

		///<summary>Returns null if no fee exists, returns a default fee for the passed in feeSchedNum.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			return GetFee(codeNum,feeSchedNum,0,0);
		}

		///<summary>Returns null if no fee exists, returns a default fee for the passed in feeSchedNum.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			return GetFee(codeNum,FeeScheds.GetOne(feeSchedNum,listFeeScheds),clinicNum,provNum);
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.</summary>
		public static Fee GetFee(long codeNum,FeeSched feeSched,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=new List<Fee>();
			//We do not want to make a deep copy here because that takes a lot of time if the calling method is calling this method in a loop.
			//Locking this next helper method call will guard against the dictionary itself (not its content) from being modified.
			bool isDictNull=false;
			lock(_lockObj) {
				if(_dictFeesByFeeSchedNumsAndCodeNums==null) {
					isDictNull=true;
				}
			}
			if(isDictNull) {
				Fees.RefreshCache();
			}
			lock(_lockObj) {
				listFees=GetFeesBySchedAndCode(feeSched.FeeSchedNum,codeNum,_dictFeesByFeeSchedNumsAndCodeNums);
			}
			return GetFee(codeNum,feeSched,clinicNum,provNum,listFees);
		}

		///<summary>Returns null if no fee exists, returns a default fee for the passed in feeSchedNum.
		///Uses the dictionary of fees passed in instead of the cached dictionary.</summary>
		public static Fee GetFee(long codeNum,FeeSched feeSched,long clinicNum,long provNum
			,Dictionary<long,Dictionary<long,List<Fee>>> dictFeesByFeeSchedNumsAndCodeNums) 
		{
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=GetFeesBySchedAndCode(feeSched.FeeSchedNum,codeNum,dictFeesByFeeSchedNumsAndCodeNums);
			return GetFee(codeNum,feeSched,clinicNum,provNum,listFees);
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.
		///Attempts to find the most accurate fee based on the clinic and provider passed in.
		///Uses the passed in list of fees instead of the cached fees.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			return GetFee(codeNum,FeeScheds.GetOne(feeSchedNum,listFeeScheds),clinicNum,provNum,listFees);
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.
		///Attempts to find the most accurate fee based on the clinic and provider passed in.
		///Uses the passed in list of fees instead of the cached fees.</summary>
		public static Fee GetFee(long codeNum,FeeSched feeSched,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			if(codeNum==0) {
				return null;
			}
			if(feeSched==null || feeSched.FeeSchedNum==0) {
				return null;
			}
			Fee bestFee;
			if(!feeSched.IsGlobal) {//Localized fee schedule.
				//Try to find a best match
				bestFee=listFees.Find(fee => fee.FeeSched==feeSched.FeeSchedNum && fee.CodeNum==codeNum && fee.ClinicNum==clinicNum && fee.ProvNum==provNum);
				if(bestFee!=null) {
					return bestFee;
				}
				//Try to find a provider match
				bestFee=listFees.Find(fee => fee.FeeSched==feeSched.FeeSchedNum && fee.CodeNum==codeNum && fee.ProvNum==provNum && fee.ClinicNum==0);
				if(bestFee!=null) {
					return bestFee;
				}
				//Try to find a clinic match
				bestFee=listFees.Find(fee => fee.FeeSched==feeSched.FeeSchedNum && fee.CodeNum==codeNum && fee.ClinicNum==clinicNum && fee.ProvNum==0);
				if(bestFee!=null) {
					return bestFee;
				}
				//If a localized fee schedule was not found, search for a default fee schedule.
			}
			//Default fee schedules will always have ClinicNum and ProvNum set to 0.
			bestFee=listFees.Find(fee => fee.FeeSched==feeSched.FeeSchedNum && fee.CodeNum==codeNum && fee.ClinicNum==0 && fee.ProvNum==0);
			if(bestFee!=null) {
				return bestFee;
			}
			return null; //No match found at all for HQ fee.
		}

		///<summary>Returns null if there is no perfect match, returns a fee if there is.  
		///Used when you need to be more specific about matching search criteria.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static Fee GetMatch(long codeNum,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees) {
			for(int i=0;i<listFees.Count;i++) {
				if(listFees[i].CodeNum==codeNum 
					&& listFees[i].FeeSched==feeSchedNum
					&& listFees[i].ClinicNum==clinicNum
					&& listFees[i].ProvNum==provNum) 
				{
					return listFees[i];
				}
			}
			return null;
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.  Not usually used directly.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)){
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.  Not usually used directly.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum,clinicNum,provNum,listFees);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		public static double GetAmount0(long codeNum,long feeSched) {
			//No need to check RemotingRole; no call to db.
			return GetAmount0(codeNum,feeSched,0,0);													 
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Returns 0 if code can't be found.
		///TODO: =js 6/19/13 There are many places where this is used to get the fee for a proc.  This results in approx 12 identical chunks of code throughout the program.
		///We need to build a method to eliminate all those identical sections.  This will prevent bugs from cropping up when these sections get out of synch.</summary>
		public static double GetAmount0(long codeNum,long feeSched,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched,clinicNum,provNum);
			if(retVal==-1){
				return 0;
			}
			return retVal;															 
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  
		///Prefers local clinic fees over HQ fees.  
		///Returns 0 if code can't be found.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static double GetAmount0(long codeNum,long feeSched,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched,clinicNum,provNum,listFees);
			if(retVal==-1) {
				return 0;
			}
			return retVal;
		}

		///<summary>Gets the fee schedule from the first insplan, the patient, or the provider in that order.  Either returns a fee schedule (fk to definition.DefNum) or 0.</summary>
		public static long GetFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList) {
			//No need to check RemotingRole; no call to db.
			//there's not really a good place to put this function, so it's here.
			long retVal=0;
			//First, try getting the fee schedule from the insplan.
			if(PatPlans.GetInsSubNum(patPlans,1)!=0){
				InsSub SubCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlans,1),subList);
				InsPlan PlanCur=InsPlans.GetPlan(SubCur.PlanNum,planList);
				if(PlanCur==null){
					retVal=0;
				}
				else{
					retVal=PlanCur.FeeSched;
				}
			}
			if(retVal==0){//Ins plan did not have a fee sched, check the patient.
				if(pat.FeeSched!=0){
					retVal=pat.FeeSched;
				}
				else {//Patient did not have a fee sched, check the provider.
					List<Provider> listProvs=ProviderC.GetListLong();
					if(pat.PriProv!=0 && listProvs.Count>0) {
						retVal=listProvs[Providers.GetIndexLong(pat.PriProv,listProvs)].FeeSched;//Guaranteed to work because ProviderC.ListLong has at least one provider in the list.
					}
				}
			}
			return retVal;
		}

		///<summary>A simpler version of the same function above.  The required numbers can be obtained in a fairly simple query.  Might return a 0 if the primary provider does not have a fee schedule set.</summary>
		public static long GetFeeSched(long priPlanFeeSched,long patFeeSched,long patPriProvNum) {
			//No need to check RemotingRole; no call to db.
			if(priPlanFeeSched!=0){
				return priPlanFeeSched;
			}
			if(patFeeSched!=0){
				return patFeeSched;
			}
			List<Provider> listProvs=ProviderC.GetListLong();
			return listProvs[Providers.GetIndexLong(patPriProvNum,listProvs)].FeeSched;
		}

		///<summary>Gets the fee schedule from the primary MEDICAL insurance plan, the first insurance plan, the patient, or the provider in that order.</summary>
		public static long GetMedFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList) {
			//No need to check RemotingRole; no call to db.
			long retVal = 0;
			if(PatPlans.GetInsSubNum(patPlans,1) != 0){
				//Pick the medinsplan with the ordinal closest to zero
				int planOrdinal=10; //This is a hack, but I doubt anyone would have more than 10 plans
				bool hasMedIns=false; //Keep track of whether we found a medical insurance plan, if not use dental insurance fee schedule.
				InsSub subCur;
				foreach(PatPlan patplan in patPlans){
					subCur=InsSubs.GetSub(patplan.InsSubNum,subList);
					if(patplan.Ordinal<planOrdinal && InsPlans.GetPlan(subCur.PlanNum,planList).IsMedical) {
						planOrdinal=patplan.Ordinal;
						hasMedIns=true;
					}
				}
				if(!hasMedIns) { //If this patient doesn't have medical insurance (under ordinal 10)
					return GetFeeSched(pat,planList,patPlans,subList);  //Use dental insurance fee schedule
				}
				subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlans,planOrdinal),subList);
				InsPlan PlanCur=InsPlans.GetPlan(subCur.PlanNum, planList);
				if (PlanCur==null){
					retVal=0;
				} 
				else {
					retVal=PlanCur.FeeSched;
				}
			}
			if (retVal==0){
				if (pat.FeeSched!=0){
					retVal=pat.FeeSched;
				} 
				else {
					if (pat.PriProv==0){
						List<Provider> listProvs=ProviderC.GetListShort();
						retVal=listProvs[0].FeeSched;
					} 
					else {
						//MessageBox.Show(Providers.GetIndex(Patients.Cur.PriProv).ToString());   
						List<Provider> listProvs=ProviderC.GetListLong();
						retVal=listProvs[Providers.GetIndexLong(pat.PriProv,listProvs)].FeeSched;
					}
				}
			}
			return retVal;
		}

		///<summary>Removes (clears) all fees matching the schedule, clinic, and provider passed in from the provided list of fees.
		///Returns the list that has had the corresponding fees removed from it.</summary>
		public static List<Fee> ClearFeeSched(long schedNum,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			for(int i=listFees.Count-1;i>=0;i--) {
				if(listFees[i].FeeSched==schedNum && listFees[i].ClinicNum==clinicNum && listFees[i].ProvNum==provNum) {
					listFees.RemoveAt(i);
				}
			}
			return listFees;
		}

		///<summary>Copies any fee objects over to the fee schedule passed in.  The user will typically have cleared the fee schedule first.
		///Supply the list of all fees for all fee schedules.
		///Returns listFees back after copying the fees from the passed in fee schedule information.</summary>
		public static List<Fee> CopyFees(long fromFeeSched,long fromClinicNum,long fromProvNum,long toFeeSched,long toClinicNum,long toProvNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			Fee fee;
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched toSched=FeeScheds.GetOne(toFeeSched,listFeeScheds);
			List<Fee> listFeesForSched=listFees.FindAll(x => x.FeeSched==fromFeeSched);
			foreach(Fee feeCur in listFeesForSched) {
				if(listFees.Exists(x => x.CodeNum==feeCur.CodeNum 
						&& x.ClinicNum==toClinicNum 
						&& x.ProvNum==toProvNum
						&& x.FeeSched==toFeeSched)) 
				{
					continue;
				}
				Fee feeBestMatch=GetFee(feeCur.CodeNum,fromFeeSched,fromClinicNum,fromProvNum,listFeesForSched);
				if(feeBestMatch==null) {//The source fee schedule doesn't have a fee for that
					continue;
				}
				fee=feeBestMatch.Copy();
				fee.ProvNum=toProvNum;
				fee.ClinicNum=toClinicNum;
				fee.FeeNum=0;//Set 0 to insert
				fee.FeeSched=toFeeSched;
				listFees.Add(fee);
			}
			return listFees;
		}

		///<summary>Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///Returns listFees back after increasing the fees from the passed in fee schedule information.</summary>
		public static List<Fee> Increase(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			//Get all fees associated to the fee schedule passed in.
			List<Fee> listFeesForSched=listFees.FindAll(x => x.FeeSched==feeSchedNum);
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched feeSched=FeeScheds.GetOne(feeSchedNum,listFeeScheds);
			List<long> listCodeNums=new List<long>(); //Contains only the fee codeNums that have been increased.  Used for keeping track.
			foreach(Fee feeCur in listFeesForSched) {
				if(listCodeNums.Contains(feeCur.CodeNum)) {
					continue; //Skip the fee if it's associated to a procedure code that has already been increased / added.
				}
				//Find the fee with the best match for this procedure code with the additional settings passed in.
				Fee feeForCode=GetFee(feeCur.CodeNum,feeSched,clinicNum,provNum,listFeesForSched);
				//The best match isn't 0, and we haven't already done this CodeNum
				if(feeForCode!=null && feeForCode.Amount!=0) {
					double newVal=(double)feeForCode.Amount*(1+(double)percent/100);
					if(round>0) {
						newVal=Math.Round(newVal,round);
					}
					else {
						newVal=Math.Round(newVal,MidpointRounding.AwayFromZero);
					}
					//The fee showing in the fee schedule is not a perfect match.  Make a new one that is.
					//E.g. We are increasing all fees for clinicNum of 1 and provNum of 5 and the best match found was for clinicNum of 3 and provNum of 7.
					//We would then need to make a copy of that fee, increase it, and then associate it to the clinicNum and provNum passed in (1 and 5).
					if(!feeSched.IsGlobal && (feeForCode.ClinicNum!=clinicNum || feeForCode.ProvNum!=provNum)) {
						Fee fee=new Fee();
						fee.Amount=newVal;
						fee.CodeNum=feeCur.CodeNum;
						fee.ClinicNum=clinicNum;
						fee.ProvNum=provNum;
						fee.FeeSched=feeSchedNum;
						listFees.Add(fee);
					}
					else { //Just update the match found.
						feeForCode.Amount=newVal;
					}
				}
				listCodeNums.Add(feeCur.CodeNum);
			}
			return listFees;
		}

		///<summary>This method will remove and/or add a fee for the fee information passed in.
		///codeText will typically be one valid procedure code.  E.g. D1240
		///If an amt of -1 is passed in, then it indicates a "blank" entry which will cause deletion of any existing fee.
		///Returns listFees back after importing the passed in fee information.
		///Does not make any database calls.  This is left up to the user to take action on the list of fees returned.
		///Also, makes security log entries based on how the fee changed.  Does not make a log for codes that were removed (user already warned)</summary>
		public static List<Fee> Import(string codeText,double amt,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			if(!ProcedureCodes.IsValidCode(codeText)){
				return listFees;//skip for now. Possibly insert a code in a future version.
			}
			string feeOldStr="";
			Fee fee=GetMatch(ProcedureCodes.GetCodeNum(codeText),feeSchedNum,clinicNum,provNum,listFees);
			if(fee!=null) {
				feeOldStr=Lans.g("FormFeeSchedTools","Old Fee")+": "+fee.Amount.ToString("c")+", ";
				listFees.Remove(fee);
			}
			if(amt==-1) {
				return listFees;
			}
			fee=new Fee();
			fee.Amount=amt;
			fee.FeeSched=feeSchedNum;
			fee.CodeNum=ProcedureCodes.GetCodeNum(codeText);
			fee.ClinicNum=clinicNum;//Either 0 because you're importing on an HQ schedule or local clinic because the feesched is localizable.
			fee.ProvNum=provNum;
			listFees.Add(fee);//Insert new fee specific to the active clinic.
			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lans.g("FormFeeSchedTools","Procedure")+": "+codeText+", "+feeOldStr
				+Lans.g("FormFeeSchedTools","New Fee")+": "+amt.ToString("c")+", "
				+Lans.g("FormFeeSchedTools","Fee Schedule")+": "+FeeScheds.GetDescription(feeSchedNum)+". "
				+Lans.g("FormFeeSchedTools","Fee changed using the Import button in the Fee Tools window."),ProcedureCodes.GetCodeNum(codeText));
			return listFees;
		}

		///<summary>Inserts, updates, or deletes the passed in listNew against the stale listOld.  Returns true if db changes were made.
		///This does not call the normal crud.Sync due to the special case of making sure we do not insert a duplicate fee.</summary>
		public static bool Sync(List<Fee> listNew,List<Fee> listOld) {
			//No call to DB yet, remoting role to be checked later.
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Fee> listIns = new List<Fee>();
			List<Fee> listUpdNew = new List<Fee>();
			List<Fee> listUpdDB = new List<Fee>();
			List<Fee> listDel = new List<Fee>();
			listNew.Sort((Fee x,Fee y) => { return x.FeeNum.CompareTo(y.FeeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listOld.Sort((Fee x,Fee y) => { return x.FeeNum.CompareTo(y.FeeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew = 0;
			int idxDB = 0;
			Fee fieldNew;
			Fee fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listOld.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listOld.Count) {
					fieldDB=listOld[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.FeeNum<fieldDB.FeeNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.FeeNum>fieldDB.FeeNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Everything past this point needs to increment idxNew and idxDB.
				else if(Crud.FeeCrud.UpdateComparison(fieldNew,fieldDB)) {
					//Both lists contain the 'next' item, update required
					listUpdNew.Add(fieldNew);
					listUpdDB.Add(fieldDB);
				}
				idxNew++;
				idxDB++;
				//There is nothing to do with this fee?
			}
			if(listIns.Count==0 && listUpdNew.Count==0 && listUpdDB.Count==0 && listDel.Count==0) {
				return false;//No need to go through remoting role check and following code because it will do nothing.
			}
			//This sync logic was split up from the typical sync logic in order to restrict payload sizes that are sent over middle tier.
			//Without first making the lists of fees as small as possible, some fee lists were so large that the maximum SOAP payload size was getting met.
			//If this method starts having issues in the future we will need to serialize the lists of fees into DataTables to further save size.
			return SyncToDbHelper(listIns,listUpdNew,listUpdDB,listDel);
		}

		///<summary>Inserts, updates, or deletes database rows sepcified in the supplied lists.  Returns true if db changes were made.
		///Supply Security.CurUser.UserNum, used to set the SecUserNumEntry field for Inserts.
		///This was split from the list building logic to limit the payload that needed to be sent over middle tier.</summary>
		public static bool SyncToDbHelper(List<Fee> listIns,List<Fee> listUpdNew,List<Fee> listUpdDB,List<Fee> listDel,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listIns,listUpdNew,listUpdDB,listDel,userNum);
			}
			int rowsUpdatedCount = 0;
			//Commit changes to DB
			//Delete any potential duplicate fees before inserting new ones.
			if(listIns.Count > 0) {
				//A duplicate fee is one that has the exact same FeeSchedNum, CodeNum, ClinicNum, and ProvNum.  ClinicNum and ProvNum may be 0.
				//Future TODO:  Find a safer way to do this.  Transaction the deletes with the inserts?
				string command = "DELETE FROM fee WHERE ";
				for(int i = 0;i<listIns.Count;i++) {
					if(i>0) {
						command+="OR ";
					}
					command += "(fee.FeeSched="+POut.Long(listIns[i].FeeSched)+" "
						+"AND fee.CodeNum="+POut.Long(listIns[i].CodeNum)+" "
						+"AND fee.ClinicNum="+POut.Long(listIns[i].ClinicNum)+" "
						+"AND fee.ProvNum="+POut.Long(listIns[i].ProvNum)+") ";
				}
				Db.NonQ(command);
			}
			for(int i = 0;i<listIns.Count;i++) {
				listIns[i].SecUserNumEntry=userNum;
				Crud.FeeCrud.Insert(listIns[i]);
			}
			for(int i = 0;i<listUpdNew.Count;i++) {
				if(Crud.FeeCrud.Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			for(int i = 0;i<listDel.Count;i++) {
				Crud.FeeCrud.Delete(listDel[i].FeeNum);
			}
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}
	}

	public struct FeeKey{
		public long codeNum;
		public long feeSchedNum;
	}

}