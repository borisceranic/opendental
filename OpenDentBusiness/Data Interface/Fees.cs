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

		///<summary>A list of all Fees.</summary>
		public static List<Fee> Listt{
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>A list of all Fees.</summary>
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
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listFees.Add(_listt[i].Copy());
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

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			return GetFee(codeNum,GetListt(),FeeScheds.GetOne(feeSchedNum,listFeeScheds));
		}

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.</summary>
		public static Fee GetFee(long codeNum,List<Fee> listFees,FeeSched feeSched) {
			//No need to check RemotingRole; no call to db.
			if(codeNum==0){
				return null;
			}
			if(feeSched==null || feeSched.FeeSchedNum==0){
				return null;
			}
			if(!feeSched.IsGlobal) {//Clinic fee schedule.
				for(int i=0;i<listFees.Count;i++) {
					if(listFees[i].CodeNum==codeNum && listFees[i].FeeSched==feeSched.FeeSchedNum && listFees[i].ClinicNum==Clinics.ClinicNum) {
						return listFees[i];
					}
				}
			}
			//This fee schedule is a HQ fee schedule, or the clinic fee was not found.
			for(int i=0;i<listFees.Count;i++) {//Search for the HQ fee schedule.
				if(listFees[i].CodeNum==codeNum && listFees[i].ClinicNum==0 && listFees[i].FeeSched==feeSched.FeeSchedNum) {
					return listFees[i];
				}
			}
			return null;
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.  Not usually used directly.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)){
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Returns 0 if code can't be found.
		///TODO: =js 6/19/13 There are many places where this is used to get the fee for a proc.  This results in approx 12 identical chunks of code throughout the program.
		///We need to build a method to eliminate all those identical sections.  This will prevent bugs from cropping up when these sections get out of synch.</summary>
		public static double GetAmount0(long codeNum,long feeSched) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched);
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
			//No need to check RemotingRole; no call to db. ??
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

		///<summary>Clears all fees from one fee schedule.  Supply the FeeSchedNum of the feeSchedule.</summary>
		public static void ClearFeeSched(long schedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),schedNum);
				return;
			}
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched feeSched=FeeScheds.GetOne(schedNum,listFeeScheds);
			string command="DELETE FROM fee WHERE FeeSched="+schedNum;
			if(!feeSched.IsGlobal) {
				command+=" AND ClinicNum="+Clinics.ClinicNum;
			}
			Db.NonQ(command);
		}

		///<summary>Copies any fee objects over to the new fee schedule.  Usually run ClearFeeSched first.  Be careful exactly which int's you supply.  Only copies the currently active clinic's fees to the new schedule.</summary>
		public static void CopyFees(long fromFeeSched,long toFeeSched) {
			//No need to check RemotingRole; no call to db.
			List<Fee> listFees=GetListt();
			if(listFees==null) {
				RefreshCache();
			}
			Fee fee;
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched toSched=FeeScheds.GetOne(toFeeSched,listFeeScheds);
			for(int i=0;i<listFees.Count;i++){
				if(listFees[i].FeeSched!=fromFeeSched){
					continue;
				}
				if(listFees[i].ClinicNum!=Clinics.ClinicNum) {
					continue;
				}
				fee=listFees[i].Copy();
				if(toSched.IsGlobal) {
					fee.ClinicNum=0;
				}
				else {
					fee.ClinicNum=Clinics.ClinicNum;
				}
				fee.FeeNum=0;//Set 0 to insert
				fee.FeeSched=toFeeSched;
				Fees.Insert(fee);
			}
		}

		///<summary>Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.</summary>
		public static void Increase(long feeSchedNum,int percent,int round) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched feeSched=FeeScheds.GetOne(feeSchedNum,listFeeScheds);
			List<Fee> listFees=GetListt();
			if(listFees==null) {
				RefreshCache();
			}
			for(int i=listFees.Count-1;i>=0;i--) { //Contains all fees for all scheds.
				Fee feeCur=listFees[i];
				Fee feeForCode=GetFee(feeCur.CodeNum,listFees,feeSched);
				if(feeForCode!=null && feeForCode.Amount!=0) {
					double newVal=(double)feeForCode.Amount*(1+(double)percent/100);
					if(round>0) {
						feeForCode.Amount=Math.Round(newVal,round);
					}
					else {
						feeForCode.Amount=Math.Round(newVal,MidpointRounding.AwayFromZero);
					}
					if(!feeSched.IsGlobal && feeForCode.ClinicNum!=Clinics.ClinicNum) {//The fee showing in the clinic fee schedule is the HQ fee.
						feeForCode.ClinicNum=Clinics.ClinicNum;
						feeForCode.FeeNum=0;//Reset the primary key to zero, so that a new record will be inserted.
						Fees.Insert(feeForCode);
					}
					else {
						Fees.Update(feeForCode);
					}
				}
				for(int j=listFees.Count-1;j>=0;j--) {
					if(listFees[j].CodeNum==feeCur.CodeNum) {
						listFees.RemoveAt(j);
					}
				}
				i=listFees.Count;
			}
		}

		///<summary>schedI is the currently displayed index of the fee schedule to save to.  If an amt of -1 is passed in, then it indicates a "blank" entry which will cause deletion of any existing fee.</summary>
		public static void Import(string codeText,double amt,long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			if(!ProcedureCodes.IsValidCode(codeText)){
				return;//skip for now. Possibly insert a code in a future version.
			}
			long feeNum=GetFeeNum(ProcedureCodes.GetCodeNum(codeText),feeSchedNum);//feeNum will be for the currently active clinic.
			if(feeNum>0) {
				Delete(feeNum);
			}
			if(amt==-1) {
				//RefreshCache();
				return;
			}
			Fee fee=new Fee();
			fee.Amount=amt;
			fee.FeeSched=feeSchedNum;
			fee.CodeNum=ProcedureCodes.GetCodeNum(codeText);
			fee.ClinicNum=0;//Either 0 because you're importing on an HQ schedule or local clinic because the feesched is localizable.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			if(!FeeScheds.GetOne(feeSchedNum,listFeeScheds).IsGlobal) {//FeeSched is localizable.
				fee.ClinicNum=Clinics.ClinicNum;
			}
			Insert(fee);//Insert new fee specific to the active clinic.
			//RefreshCache();//moved this outside the loop
		}

		///<summary>Gets the FeeNum from the database based on FeeSched.IsGlobal and currently active clinic, returns 0 if none found.</summary>
		public static long GetFeeNum(long codeNum,long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),codeNum,feeSchedNum);
			}
			long clinicNum=0;
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			if(!FeeScheds.GetOne(feeSchedNum,listFeeScheds).IsGlobal) {//FeeSched is localizable.
				clinicNum=Clinics.ClinicNum;
			}
			string command="SELECT FeeNum FROM fee WHERE CodeNum="+POut.Long(codeNum)+" AND FeeSched="+POut.Long(feeSchedNum)+" AND ClinicNum="+POut.Long(clinicNum);
			return PIn.Long(Db.GetScalar(command));
		}

	}

	public struct FeeKey{
		public long codeNum;
		public long feeSchedNum;
	}

}