using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Fees {
		private static List<Fee> _listt;
		private static object _lockObj=new object();

		///<summary>A list of non-hidden Fees.  A hidden fee is considered a fee that is associated to a hidden fee schedule.</summary>
		public static List<Fee> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>A list of non-hidden Fees.  A hidden fee is considered a fee that is associated to a hidden fee schedule.</summary>
		public static List<Fee> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Fees.RefreshCache();
			}
			List<Fee> listFees=new List<Fee>();
			//Get a deep copy of the visible fee schedules to help figure out which fees are 'hidden'.
			List<FeeSched> listFeeSchedules=FeeSchedC.GetListShort();
			lock(_lockObj) {
				//This is not common at all.  Typically there will be a "GetListShort" that will return all cached items that are not hidden.
				//However, there is no such thing as hiding a fee.  There is however such a thing as hiding a fee schedule.
				//Hiding a fee schedule is just a good as hiding the fees it is associated to.
				//For speed purposes, we need to grab deep copies of fees that are not associated to a hidden fee schedule.
				//There are places in the program (e.g. computing estimates) that call GetListt many times in a loop.
				//Those loops are extremely slow when getting a deep copy of the entire fee table (e.g. 359,421 fees) when only a portion of them are used.
				//An example is when you change nothing and leave the Ins Plan Edit window by clicking OK.
				//That window was taking an office several seconds to close.  By not grabbing a deep copy of hidden fees, the time was cut in half.
				List<Fee> listNonHiddenFees=_listt.FindAll(x => listFeeSchedules.Exists(y => y.FeeSchedNum==x.FeeSched));//Get all non-hidden fees.
				for(int i=0;i<listNonHiddenFees.Count;i++) {
					listFees.Add(listNonHiddenFees[i].Copy());
				}
			}
			return listFees;
		}

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM fee";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Fee";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			Listt=Crud.FeeCrud.TableToList(table);
			/*
			Dict=new Dictionary<FeeKey,Fee>();
			FeeKey key;
			for(int i=0;i<Listt.Count;i++) {
				key=new FeeKey();
				key.codeNum=Listt[i].CodeNum;
				key.feeSchedNum=Listt[i].FeeSched;
				if(Dict.ContainsKey(key)) {
					//Older versions used to delete duplicates here
				}
				else {
					Dict.Add(key,Listt[i]);
				}
			}*/
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

		///<summary>Inserts, updates, or deletes the passed in list against the current cached rows.</summary>
		public static void Sync(List<Fee> listNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew);
				return;
			}
			List<Fee> listDB=Fees.GetListt();
			Crud.FeeCrud.Sync(listNew,listDB);
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.
		///Attempts to find the most accurate fee based on the clinic and provider passed in.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			return GetFee(codeNum,FeeScheds.GetOne(feeSchedNum,listFeeScheds),clinicNum,provNum,GetListt());
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.
		///Attempts to find the most accurate fee based on the clinic and provider passed in.</summary>
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

		///<summary>Returns null if there is no perfect match, returns a fee if there is.  Used when you need to be more specific about matching search criteria.</summary>
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

		///<summary>Clears all fees from one fee schedule.  Supply the list of all fees for all fee schedules.</summary>
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
			for(int i=0;i<listFees.Count;i++){
				if(listFees[i].FeeSched!=fromFeeSched){
					continue;
				}
				if(listFees[i].ClinicNum!=fromClinicNum) {
					continue;
				}
				if(listFees[i].ProvNum!=fromProvNum) {
					continue;
				}
				fee=listFees[i].Copy();
				fee.ProvNum=toProvNum;
				fee.ClinicNum=toClinicNum;
				fee.FeeNum=0;//Set 0 to insert
				fee.FeeSched=toFeeSched;
				listFees.Add(fee);
			}
			return listFees;
		}

		///<summary>Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///Supply the list of all fees for all fee schedules.
		///Returns listFees back after increasing the fees from the passed in fee schedule information.</summary>
		public static List<Fee> Increase(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched feeSched=FeeScheds.GetOne(feeSchedNum,listFeeScheds);
			List<long> listCodeNums=new List<long>(); //Contains only the fee codeNums that have been increased.  Used for keeping track.
			for(int i=0;i<listFees.Count;i++) { //Contains all fees for all scheds.
				Fee feeCur=listFees[i];
				if(listCodeNums.Contains(feeCur.CodeNum)) {
					continue; //Skip the fee if it's already been increased.
				}
				Fee feeForCode=GetFee(feeCur.CodeNum,feeSched,clinicNum,provNum,listFees);
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

	}

	public struct FeeKey{
		public long codeNum;
		public long feeSchedNum;
	}

}