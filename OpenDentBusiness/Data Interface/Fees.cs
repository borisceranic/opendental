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
		///<summary>Access _Cache instead. This is a unique cache class used for caching and manipulating fees.</summary>
		private static FeeCache _cache;

		///<summary>This is a verry unique cache class. Not generally available for use, instead either get a copy of the cache for local use or use some of the 
		///functions in the S class.</summary>
		private static FeeCache _Cache {
			get {
				bool isNull;
				lock(_lockObj) {
					isNull=_cache==null;
				}
				if(isNull) {
					FeeCache cache=new FeeCache();
					cache.Initialize();
					lock(_lockObj) {
						_cache=cache;
					}
				}
				return _cache;
			}
			set {
				lock(_lockObj) {
					_cache=value;
				}
			}
		}

		//The entire dictionary cache section is very rare.  
		//It is only present because there are several large offices that have called in complaining about slowness.
		//The slowness is caused by our new thread safe cache pattern coupled with an old looping pattern throughout the Open Dental project.
		//The slowness only shows its face when there is an astronomical (e.g. 490K) amount of fees present.
		//This new dictionary will break up the fees first by a dictionary keyed on FeeSchedNum and then by a dictionary keyed on CodeNum.

		///<summary>Returns all fees for the provided FeeSchedNum in one big unsorted list. Used to fill itemized cache.</summary>
		public static List<Fee> GetByFeeSchedNumDB(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod(),feeSchedNum);
			}
			string command = "SELECT * FROM fee WHERE FeeSched="+POut.Long(feeSchedNum);
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Returns all fees for the provided FeeSchedNums in one big unsorted list. 
		///Used to fill itemized cache, should not be called directly from anything other than cache.</summary>
		public static List<Fee> GetByFeeSchedNumsDB(List<long> feeSchedNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod(),feeSchedNums);
			}
			if(feeSchedNums==null || feeSchedNums.Count==0) {
				return new List<Fee>();
			}
			string command = "SELECT * FROM fee WHERE FeeSched IN ("+string.Join(",",feeSchedNums)+")";
			return Crud.FeeCrud.SelectMany(command);
		}

		public static List<Fee> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod());
			}
			string command = "SELECT * FROM fee";
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Gets a copy of the cache for local use. Pass fee sched nums to reduce the size of the cache, otherwise returns copy of entire cache.</summary>
		public static FeeCache GetCache() {
			lock(_lockObj) {
				return _Cache.GetCopy();
			}
		}

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command = "SELECT * FROM fee";//safe to get hidden fees.
			DataTable table = Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Fee";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			FeeCache cache= new FeeCache(Crud.FeeCrud.TableToList(table));
			//Add the fees by code nums dictionary to the dictionary of fee schedules but use a lock for thread safety.
			_Cache=cache;//_Cache locks if neccesary
		}

		///<summary>Returns all fees associated to the procedure code passed in.</summary>
		public static List<Fee> GetFeesForCode(long codeNum) {
			//No need to check RemotingRole; no call to db.
			FeeCache cache;
			lock(_lockObj) {
				cache = _Cache.GetCopy();
			}
			return cache.Dict.SelectMany(x => x.Value.Where(y => y.Key==codeNum).SelectMany(y=>y.Value)).ToList();
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

		///<summary>Returns null if no fee exists, returns a fee based on feeSched and fee localization settings.
		///Attempts to find the most accurate fee based on the clinic and provider passed in.
		///Uses the passed in list of fees instead of the cached fees.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0) {
			//No need to check RemotingRole; no call to db.
			lock(_lockObj) {
				return _Cache.GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			}
		}

		public static void InvalidateFeeSchedules(List<long> listFeeScheduleNums) {
			lock(_lockObj) {
				listFeeScheduleNums.ForEach(x => _Cache.Invalidate(x));
			}
			//if we add a preference to remove lazy loading, it would put a refreshcache call right here.
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.  Not usually used directly.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  
		///Prefers local clinic fees over HQ fees.  
		///Returns 0 if code can't be found.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static double GetAmount0(long codeNum,long feeSched,long clinicNum=0,long provNum=0) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched,clinicNum,provNum);
			if(retVal==-1) {
				return 0;
			}
			return retVal;
		}

		///<summary>Gets the fee sched from the first insplan, the patient, or the provider in that order.  Uses procProvNum if>0, otherwise pat.PriProv.
		///Either returns a fee schedule (fk to definition.DefNum) or 0.</summary>
		public static long GetFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
			//No need to check RemotingRole; no call to db.
			//there's not really a good place to put this function, so it's here.
			long priPlanFeeSched=0;
			PatPlan patPlanPri = patPlans.FirstOrDefault(x => x.Ordinal==1);
			if(patPlanPri!=null) {
				InsPlan planCur=InsPlans.GetPlan(InsSubs.GetSub(patPlanPri.InsSubNum,subList).PlanNum,planList);
				if(planCur!=null) {
					priPlanFeeSched=planCur.FeeSched;
				}
			}
			return GetFeeSched(priPlanFeeSched,pat.FeeSched,procProvNum!=0?procProvNum:pat.PriProv);//use procProvNum, but if 0 then default to pat.PriProv
		}

		///<summary>A simpler version of the same function above.  The required numbers can be obtained in a fairly simple query.
		///Might return a 0 if the primary provider does not have a fee schedule set.</summary>
		public static long GetFeeSched(long priPlanFeeSched,long patFeeSched,long provNum) {
			//No need to check RemotingRole; no call to db.
			long provFeeSched=(ProviderC.GetListLong().FirstOrDefault(x => x.ProvNum==provNum)??new Provider()).FeeSched;//defaults to 0
			return new[] { priPlanFeeSched,patFeeSched,provFeeSched }.FirstOrDefault(x => x>0);//defaults to 0 if all fee scheds are 0
		}

		///<summary>Gets the fee schedule from the primary MEDICAL insurance plan, the first insurance plan, the patient, or the provider in that order.</summary>
		public static long GetMedFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
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
					return GetFeeSched(pat,planList,patPlans,subList,procProvNum);  //Use dental insurance fee schedule
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

		///<summary>Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///Returns listFees back after increasing the fees from the passed in fee schedule information.</summary>
		public static List<Fee> Increase(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			FeeCache feeCache = new FeeCache(listFees.FindAll(x=>x.FeeSched==feeSchedNum));//for looking up Fees quickly/cached.
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
				Fee feeForCode=feeCache.GetFee(feeCur.CodeNum,feeSched.FeeSchedNum,clinicNum,provNum);
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
			long codeNum = ProcedureCodes.GetCodeNum(codeText);
			Fee fee = listFees.FirstOrDefault(x => x.CodeNum==codeNum && x.FeeSched==feeSchedNum && x.ClinicNum==clinicNum && x.ProvNum==provNum);
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
		/// <returns>List of feeSchedNums for any fee that might have been changed. Returns empty list if nothing changed.</returns>
		public static List<long> Sync(List<Fee> listNew,List<Fee> listOld) {
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
				return new List<long>();//No need to go through remoting role check and following code because it will do nothing.
			}
			//This sync logic was split up from the typical sync logic in order to restrict payload sizes that are sent over middle tier.
			//Without first making the lists of fees as small as possible, some fee lists were so large that the maximum SOAP payload size was getting met.
			//If this method starts having issues in the future we will need to serialize the lists of fees into DataTables to further save size.
			return SyncToDbHelper(listIns,listUpdNew,listUpdDB,listDel);
		}

		///<summary>Inserts, updates, or deletes database rows sepcified in the supplied lists.  Returns true if db changes were made.
		///Supply Security.CurUser.UserNum, used to set the SecUserNumEntry field for Inserts.
		///This was split from the list building logic to limit the payload that needed to be sent over middle tier.</summary>
		public static List<long> SyncToDbHelper(List<Fee> listIns,List<Fee> listUpdNew,List<Fee> listUpdDB,List<Fee> listDel,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),listIns,listUpdNew,listUpdDB,listDel,userNum);
			}
			List<long> feeSchedNums = new List<long>();
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
			foreach(Fee fee in listIns) {
				fee.SecUserNumEntry=userNum;
				Crud.FeeCrud.Insert(fee);
			}
			feeSchedNums.AddRange(listIns.Select(x => x.FeeSched).Distinct());
			for(int i = 0;i<listUpdNew.Count;i++) {
				if(Crud.FeeCrud.Update(listUpdNew[i],listUpdDB[i])) {
					feeSchedNums.Add(listUpdNew[i].FeeSched);
					feeSchedNums.Add(listUpdDB[i].FeeSched);
				}
			}
			for(int i = 0;i<listDel.Count;i++) {
				Crud.FeeCrud.Delete(listDel[i].FeeNum);
			}
			feeSchedNums.AddRange(listDel.Select(x => x.FeeSched).Distinct());
			feeSchedNums=feeSchedNums.Distinct().ToList();//remove additional duplicates that might have been placed there by the update function.
			return feeSchedNums;
		}
	}

	public struct FeeKey{
		public long codeNum;
		public long feeSchedNum;
	}

	///<summary>Extreamely a-typical cache pattern.
	///Contains a Dictionary in the format: Dictionary&lt;long,Dictionary&lt;long,&lt;List&lt;Fee>>> where the long keys are FeeSchedNum and code num respectively.</summary>
	public class FeeCache {
		private Dictionary<long,Dictionary<long,List<Fee>>> _dict;

		///<summary>All Fees organized by FeeSchedNum, CodeNum, followed by a list of Fees including the default fee, provider overrides, clinic overrides, etc.</summary>
		public Dictionary<long,Dictionary<long,List<Fee>>> Dict {
			get {
				if(_dict==null) {
					Initialize();
				}
				return _dict;
			}
			set {
				_dict=value;
			}
		}

		public FeeCache() {
			_dict=new Dictionary<long,Dictionary<long,List<Fee>>>();
		}

		///<summary>Construct a cache from the list of provided fees.</summary>
		public FeeCache(List<Fee> listFees) {
			_dict=listFees.GroupBy(x => x.FeeSched).ToDictionary(x => x.Key,x => x.GroupBy(y => y.CodeNum).ToDictionary(y => y.Key,y => y.ToList()));
		}

		///<summary>Fill Dictionary with ALL Fees from DB.</summary>
		public void Initialize() {
			_dict=Fees.GetAll().GroupBy(x => x.FeeSched).ToDictionary(x => x.Key,x => x.GroupBy(y => y.CodeNum).ToDictionary(y => y.Key,y => y.ToList()));
		}

		///<summary>Only refreshes cache items that have been invalidated. Can be called as frequently as one would like.</summary>
		public void RefreshCache() {
			if(_dict==null) {//using _dict instead of Dict so that we do not try refresh individual items in the cache if we are refreshing the whole thing.
				Initialize();
				return;
			}
			List<long> invalidFeeSchedNums = _dict.ToList().FindAll(x => x.Value==null).Select(x => x.Key).ToList();
			List<Fee> listFees=Fees.GetByFeeSchedNumsDB(invalidFeeSchedNums);
			Dictionary<long,Dictionary<long,List<Fee>>> tempDict = listFees.GroupBy(x => x.FeeSched).ToDictionary(x => x.Key,x => x.GroupBy(y => y.CodeNum).ToDictionary(y => y.Key,y => y.ToList()));
			tempDict.ToList().ForEach(x => _dict[x.Key]=x.Value);
		}

		///<summary>Creates copy of fee cache. Pass in a list of feeschednums to reduce the potential amount of work done and data fetched.</summary>
		/// <param name="listFeeSchedNums">If null, copies entire full cache. If set will return a cache containing just the supplied feeSchedNums.</param>
		/// <returns>A copy of the entire cache object which contains a dictionary of fees.</returns>
		public FeeCache GetCopy() {
			RefreshCache();
			FeeCache retVal = new FeeCache();
			retVal._dict=_dict.Where(x=>x.Value!=null && !x.Value.Any(y => y.Value==null || y.Value.Contains(null)))//do not copy any fee schedule that contains a null fee.
				.ToDictionary(x => x.Key,x => x.Value.ToDictionary(y => y.Key,y => y.Value.Select(z=>z.Copy()).ToList()));
			return retVal;
		}

		public void Add(Fee fee) {
			if(!_dict.ContainsKey(fee.FeeSched)) {
				_dict[fee.FeeSched]=new Dictionary<long,List<Fee>>();
			}
			if(!_dict[fee.FeeSched].ContainsKey(fee.CodeNum)) {
				_dict[fee.FeeSched][fee.CodeNum]=new List<Fee>();
			}
			_dict[fee.FeeSched][fee.CodeNum].Add(fee);
		}

		public void Remove(Fee fee) {
			if(!_dict.ContainsKey(fee.FeeSched)) {
				_dict[fee.FeeSched]=new Dictionary<long,List<Fee>>();
			}
			if(!_dict[fee.FeeSched].ContainsKey(fee.CodeNum)) {
				_dict[fee.FeeSched][fee.CodeNum]=new List<Fee>();
			}
			_dict[fee.FeeSched][fee.CodeNum].RemoveAll(x=>x.ProvNum==fee.ProvNum && x.ClinicNum==fee.ClinicNum);
		}

		public void Invalidate(long feeSchedNum) {
			_dict[feeSchedNum]=null;//this either nulled an existing fee schedule or added a new one with no information.
		}

		/////<summary>Used to refresh multiple fee schedules from the DB.</summary>
		/////<param name="feeSchedNums">The feeScheduleNum of the feeschedule to be refreshed.</param>
		//public void RefreshFeeScheds(List<long> feeSchedNums) {
		//	List<Fee> listFees = Fees.GetByFeeSchedNumsDB(feeSchedNums);
		//	Dictionary<long,Dictionary<long,List<Fee>>> tempDict=listFees.GroupBy(x => x.FeeSched).ToDictionary(x => x.Key,x => x.GroupBy(y => y.CodeNum).ToDictionary(y => y.Key,y => y.ToList()));
		//	tempDict.ToList().ForEach(x => _dict[x.Key]=x.Value);
		//}

		/////<summary>Used to refresh a single fee schedule.</summary>
		///// <param name="feeSchedNum">The feeScheduleNum of the feeschedule to be refreshed.</param>
		//public void RefreshFeeSched(long feeSchedNum) {
		//	List<Fee> listFees = Fees.GetByFeeSchedNumDB(feeSchedNum);
		//	_dict[feeSchedNum]=listFees.GroupBy(x => x.CodeNum).ToDictionary(x => x.Key,x => x.ToList());
		//}

		public Fee GetFee(long codeNum,long feeSchedNum,long clinicNum = 0,long provNum = 0) {
			//No need to check RemotingRole; no call to db.
			if(!_dict.ContainsKey(feeSchedNum)) {
				return null;
			}
			if(_dict[feeSchedNum]==null) {
				RefreshCache();
			}
			if(!_dict[feeSchedNum].ContainsKey(codeNum)) {
				return null;
			}
			//Ordering this list by provNum, then Clinic num, ensures the best match will always apear first in the list.
			return _dict[feeSchedNum][codeNum].OrderBy(x => x.ProvNum==provNum).ThenBy(x => x.ClinicNum==clinicNum).FirstOrDefault();
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.  Not usually used directly.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee = GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  
		///Prefers local clinic fees over HQ fees.  
		///Returns 0 if code can't be found.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public double GetAmount0(long codeNum,long feeSched,long clinicNum = 0,long provNum = 0) {
			//No need to check RemotingRole; no call to db.
			double retVal = GetAmount(codeNum,feeSched,clinicNum,provNum);
			if(retVal==-1) {
				return 0;
			}
			return retVal;
		}

		public List<Fee> ToList() {
			return _dict.SelectMany(x => x.Value.SelectMany(y => y.Value)).ToList();
		}

		//public List<Fee> ToListDeep() {
		//	return _dict.SelectMany(x => x.Value.SelectMany(y => y.Value.Select(z => z.Copy()))).ToList();
		//}

	}

}