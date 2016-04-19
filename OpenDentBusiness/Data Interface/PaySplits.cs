using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PaySplits {
		///<summary>Returns all paySplits for the given patNum, organized by procDate.  WARNING! Also includes related paysplits that aren't actually attached to patient.  Includes any split where payment is for this patient.</summary>
		public static PaySplit[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PaySplit[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			/*This query was too slow
			string command=
				"SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum=payment.PayNum "
				+"AND (paysplit.PatNum = '"+POut.Long(patNum)+"' OR payment.PatNum = '"+POut.Long(patNum)+"') "
				+"ORDER BY ProcDate";*/
			//this query goes 10 times faster for very large databases
			string command=@"select DISTINCT paysplitunion.* FROM "
				+"(SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum=payment.PayNum and payment.PatNum='"+POut.Long(patNum)+"' "
				+"UNION "
				+"SELECT DISTINCT paysplit.* FROM paysplit,payment "
				+"WHERE paysplit.PayNum = payment.PayNum AND paysplit.PatNum='"+POut.Long(patNum)+"') paysplitunion "
				+"ORDER BY paysplitunion.ProcDate";
			return Crud.PaySplitCrud.SelectMany(command).ToArray();
		}

		///<summary>Used from payment window to get all paysplits for the payment.</summary>
		public static List<PaySplit> GetForPayment(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),payNum);
			}
			string command=
				"SELECT * FROM paysplit "
				+"WHERE PayNum="+POut.Long(payNum);
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(PaySplit split){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),split);
				return;
			}
			Crud.PaySplitCrud.Update(split);
		}

		///<summary></summary>
		public static long Insert(PaySplit split) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				split.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				split.SplitNum=Meth.GetLong(MethodBase.GetCurrentMethod(),split);
				return split.SplitNum;
			}
			return Crud.PaySplitCrud.Insert(split);
		}

		///<summary>Deletes the paysplit.</summary>
		public static void Delete(PaySplit split){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),split);
				return;
			}
			string command= "DELETE from paysplit WHERE SplitNum = "+POut.Long(split.SplitNum);
 			Db.NonQ(command);
		}

		///<summary>Used from payment window AutoSplit button to delete paysplits when clicking AutoSplit more than once.</summary>
		public static void DeleteForPayment(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payNum);
				return;
			}
			string command="DELETE FROM paysplit"
				+" WHERE PayNum="+POut.Long(payNum);
			Db.NonQ(command);
		}
		
		///<summary>Gets one paysplit using the specified SplitNum.</summary>
		public static PaySplit GetOne(long splitNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PaySplit>(MethodBase.GetCurrentMethod(),splitNum);
			}
			string command="SELECT * FROM paysplit WHERE SplitNum="+POut.Long(splitNum);
			return Crud.PaySplitCrud.SelectOne(command);
		}

		///<summary>Used from FormPayment to return the total payments for a procedure without requiring a supplied list.</summary>
		public static string GetTotForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetString(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT SUM(paysplit.SplitAmt) FROM paysplit "
				+"WHERE paysplit.ProcNum="+POut.Long(procNum);
			return Db.GetScalar(command);

		}

		///<summary>Returns all paySplits for the given procNum. Must supply a list of all paysplits for the patient.</summary>
		public static ArrayList GetForProc(long procNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		///<summary>Used from ContrAccount and ProcEdit to display and calculate payments attached to procs. Used once in FormProcEdit</summary>
		public static double GetTotForProc(long procNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal+=List[i].SplitAmt;
				}
			}
			return retVal;
		}

		///<summary>Used from FormPaySplitEdit.  Returns total payments for a procedure for all paysplits other than the supplied excluded paysplit.</summary>
		public static double GetTotForProc(long procNum,PaySplit[] List,long excludeSplitNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].SplitNum==excludeSplitNum){
					continue;
				}
				if(List[i].ProcNum==procNum){
					retVal+=List[i].SplitAmt;
				}
			}
			return retVal;
		}

		///<summary>Used once in ContrAccount.  WARNING!  The returned list of 'paysplits' are not real paysplits.  They are actually grouped by patient and date.  Only the ProcDate, SplitAmt, PatNum, and ProcNum(one of many) are filled. Must supply a list which would include all paysplits for this payment.</summary>
		public static ArrayList GetGroupedForPayment(long payNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			int matchI;
			for(int i=0;i<List.Length;i++){
				if(List[i].PayNum==payNum){
					//find a 'paysplit' with matching procdate and patnum
					matchI=-1;
					for(int j=0;j<retVal.Count;j++){
						if(((PaySplit)retVal[j]).ProcDate==List[i].ProcDate && ((PaySplit)retVal[j]).PatNum==List[i].PatNum){
							matchI=j;
							break;
						}
					}
					if(matchI==-1){
						retVal.Add(new PaySplit());
						matchI=retVal.Count-1;
						((PaySplit)retVal[matchI]).ProcDate=List[i].ProcDate;
						((PaySplit)retVal[matchI]).PatNum=List[i].PatNum;
					}
					if(((PaySplit)retVal[matchI]).ProcNum==0 && List[i].ProcNum!=0){
						((PaySplit)retVal[matchI]).ProcNum=List[i].ProcNum;
					}
					((PaySplit)retVal[matchI]).SplitAmt+=List[i].SplitAmt;
				}
			}
			return retVal;
		}

		///<summary>Only those amounts that have the same paynum, procDate, and patNum as the payment, and are not attached to procedures.</summary>
		public static double GetAmountForPayment(long payNum,DateTime payDate,long patNum,PaySplit[] paySplitList) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<paySplitList.Length;i++){
				if(paySplitList[i].PayNum!=payNum) {
					continue;
				}
				if(paySplitList[i].PatNum!=patNum){
					continue;
				}
				if(paySplitList[i].ProcDate!=payDate){
					continue;
				}
				if(paySplitList[i].ProcNum!=0){
					continue;
				}
				retVal+=paySplitList[i].SplitAmt;
			}
			return retVal;
		}

		///<summary>Used in Payment window to get all paysplits for a single patient without using a supplied list.</summary>
		public static List<PaySplit> GetForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * FROM paysplit "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Used once in ContrAccount to just get the splits for a single patient.  The supplied list also contains splits that are not necessarily for this one patient.</summary>
		public static PaySplit[] GetForPatient(long patNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].PatNum==patNum){
					retVal.Add(List[i]);
				}
			}
			PaySplit[] retList=new PaySplit[retVal.Count];
			retVal.CopyTo(retList);
			return retList;
		}

		///<summary>For a given PayPlan, returns a table of PaySplits with additional payment information.</summary>
		public static DataTable GetForPayPlan(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command="SELECT paysplit.*,payment.CheckNum,SUM(payment.PayAmt) AS 'PayAmt',payment.PayType "
					+"FROM paysplit "
					+"LEFT JOIN payment ON paysplit.PayNum=payment.PayNum "
					+"WHERE paysplit.PayPlanNum="+POut.Long(payPlanNum)+" "
					+"GROUP BY paysplit.ProcDate,paysplit.PayNum,paysplit.ProvNum "
					+"ORDER BY ProcDate"; 
			DataTable tableSplits=Db.GetTable(command);
			return tableSplits;
		}

		///<summary>Gets paysplits from a provided datatable.  This was originally part of GetForPayPlan but can't be because it's passed through the Middle Tier.</summary>
		public static List<PaySplit> GetFromBundled(DataTable dataTable) {
			return Crud.PaySplitCrud.TableToList(dataTable);
		}

		///<summary>Used once in ContrAccount.  Usually returns 0 unless there is a payplan for this payment and patient.</summary>
		public static long GetPayPlanNum(long payNum,long patNum,PaySplit[] List) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].PayNum==payNum && List[i].PatNum==patNum && List[i].PayPlanNum!=0){
					return List[i].PayPlanNum;
				}
			}
			return 0;
		}

		///<summary>Gets all paysplits that have are designated as prepayments for the patient's family.</summary>
		public static List<PaySplit> GetPrepayForFam(Family fam) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),fam);
			}
			List<long> listFamPatNums=fam.ListPats.Select(x => x.PatNum).Distinct().ToList();
			string command="SELECT * FROM paysplit "
				+"WHERE ProvNum=0 "
				+"AND UnearnedType!=0 "
				+"AND PrePaymentNum=0 "
				+"AND PatNum IN ("+String.Join(",",listFamPatNums)+") "
				+"ORDER BY ProcDate";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Gets all paysplits that are attached to the prepayment paysplits specified.</summary>
		public static List<PaySplit> GetSplitsForPrepay(List<PaySplit> listPrepaymentSplits) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PaySplit>>(MethodBase.GetCurrentMethod(),listPrepaymentSplits);
			}
			if(listPrepaymentSplits==null || listPrepaymentSplits.Count < 1) {
				return new List<PaySplit>();
			}
			List<long> listSplitNums=listPrepaymentSplits.Select(x => x.SplitNum).Distinct().ToList();
			string command="SELECT * FROM paysplit WHERE PrePaymentNum IN ("+String.Join(",",listSplitNums)+")";
			return Crud.PaySplitCrud.SelectMany(command);
		}

		///<summary>Returns the total amount of prepayments for the entire family.</summary>
		public static decimal GetUnearnedForFam(Family fam) {
			//No need to check RemotingRole; no call to db.
			//Find all paysplits for this account with provnum=0
			//Foreach paysplit find all other paysplits with paysplitnum == provnum0 paysplit
			//Sum paysplit amounts, see if it covers provnum0 split.
			//Any money left over sum and show as "Unallocated" aka unearned
			decimal unearnedTotal=0;
			List<PaySplit> listPrePayments=PaySplits.GetPrepayForFam(fam);
			if(listPrePayments.Count>0) { 
				foreach(PaySplit split in listPrePayments) {
					unearnedTotal+=(decimal)split.SplitAmt;
				}
				List<PaySplit> listSplitsForPrePayment=PaySplits.GetSplitsForPrepay(listPrePayments);
				foreach(PaySplit split in listSplitsForPrePayment) {
					unearnedTotal+=(decimal)split.SplitAmt;//Splits for prepayments are generally negative.
				}
			}
			return unearnedTotal;
		}

		///<summary>Used in FormPayment to sych database with changes user made to the paySplit list for a payment.  Must supply an old list for comparison.  Only the differences are saved.</summary>
		public static void UpdateList(List<PaySplit> oldSplitList,List<PaySplit> newSplitList) {
			//No need to check RemotingRole; no call to db.
			PaySplit newPaySplit;
			for(int i=0;i<oldSplitList.Count;i++) {//loop through the old list
				newPaySplit=null;
				for(int j=0;j<newSplitList.Count;j++) {
					if(newSplitList[j]==null || newSplitList[j].SplitNum==0) {
						continue;
					}
					if(((PaySplit)oldSplitList[i]).SplitNum==((PaySplit)newSplitList[j]).SplitNum) {
						newPaySplit=newSplitList[j];
						break;
					}
				}
				if(newPaySplit==null) {
					//PaySplit with matching SplitNum was not found, so it must have been deleted
					Delete(oldSplitList[i]);
					continue;
				}
				//PaySplit was found with matching SplitNum, so check for changes
				if(newPaySplit.DateEntry != oldSplitList[i].DateEntry
					|| newPaySplit.DatePay != oldSplitList[i].DatePay
					|| newPaySplit.PatNum != oldSplitList[i].PatNum
					|| newPaySplit.PayNum != oldSplitList[i].PayNum
					|| newPaySplit.PayPlanNum != oldSplitList[i].PayPlanNum
					|| newPaySplit.ProcDate != oldSplitList[i].ProcDate
					|| newPaySplit.ProcNum != oldSplitList[i].ProcNum
					|| newPaySplit.ProvNum != oldSplitList[i].ProvNum
					|| newPaySplit.SplitAmt != oldSplitList[i].SplitAmt
					|| newPaySplit.UnearnedType != oldSplitList[i].UnearnedType
					|| newPaySplit.ClinicNum != oldSplitList[i].ClinicNum) 
				{
					Update(newPaySplit);
				}
			}
			for(int i=0;i<newSplitList.Count;i++) {//loop through the new list
				if(newSplitList[i]==null) {
					continue;
				}
				if(newSplitList[i].SplitNum!=0) {
					continue;
				}
				//entry with SplitNum=0, so it's new
				Insert(newSplitList[i]);
			}
		}

		

	}

	public enum SplitManagerPromptType {
		///<summary>0</summary>
		DoNotUse,
		///<summary>1</summary>
		Prompt,
		///<summary>2</summary>
		Force
	}

	


}










