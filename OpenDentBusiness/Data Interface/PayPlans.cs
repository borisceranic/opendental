using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayPlans {
		///<summary>Gets a list of all payplans for a given patient, whether they are the patient or the guarantor.  This is only used in one place, when deleting a patient to check dependencies.</summary>
		public static int GetDependencyCount(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT COUNT(*) FROM payplan"
				+" WHERE PatNum = "+POut.Long(patNum)
				+" OR Guarantor = "+POut.Long(patNum);
			return PIn.Int(Db.GetScalar(command));
		}

		public static PayPlan GetOne(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PayPlan>(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			return Crud.PayPlanCrud.SelectOne(payPlanNum);
		}

		///<summary>The patNum is the current patient.</summary>
		public static List<PayPlan> GetForPats(List<long> listPatNums,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlan>>(MethodBase.GetCurrentMethod(),listPatNums,patNum);
			}
			//We have to check for guarantor separately in case the payment plan belongs to a patient in another family.
			string command="SELECT * FROM payplan "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+") OR Guarantor="+POut.Long(patNum);
			return Crud.PayPlanCrud.SelectMany(command);
		}

		///<summary>Determines if there are any valid plans with that patient as the guarantor.</summary>
		public static List<PayPlan> GetValidPlansNoIns(long guarNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlan>>(MethodBase.GetCurrentMethod(),guarNum);
			}
			string command="SELECT * FROM payplan"
				+" WHERE Guarantor = "+POut.Long(guarNum)
				+" AND PlanNum = 0"
				+" ORDER BY payplandate";
			return Crud.PayPlanCrud.SelectMany(command);
		}

		///<summary>Returns true if the patient passed in has any outstanding non-ins payment plans with them as the guarantor.</summary>
		public static bool HasOutstandingPayPlansNoIns(long guarNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),guarNum);
			}
			string command="SELECT SUM(paysplit.SplitAmt) FROM paysplit "
				+"INNER JOIN payplan ON paysplit.PayPlanNum=payplan.PayPlanNum "
				+"WHERE payplan.PlanNum=0 "
				+"AND payplan.Guarantor="+POut.Long(guarNum);
			double amtPaid=PIn.Double(Db.GetScalar(command));
			command="SELECT SUM(payplancharge.Principal+payplancharge.Interest) FROM payplancharge "
				+"INNER JOIN payplan ON payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"WHERE payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" AND payplan.PlanNum=0 "
				+"AND payplan.Guarantor="+POut.Long(guarNum);
			double totalCost=PIn.Double(Db.GetScalar(command));
			if(totalCost-amtPaid < .01) {
				return false;
			}
			return true;
		}

		///<summary>Get all payment plans for this patient with the insurance plan identified by PlanNum and InsSubNum attached (marked used for tracking expected insurance payments) that have not been paid in full.  Only returns plans with no claimprocs currently attached or claimprocs from the claim identified by the claimNum sent in attached.  If claimNum is 0 all payment plans with planNum, insSubNum, and patNum not paid in full will be returned.</summary>
		public static List<PayPlan> GetValidInsPayPlans(long patNum,long planNum,long insSubNum,long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlan>>(MethodBase.GetCurrentMethod(),patNum,planNum,insSubNum,claimNum);
			}
			string command="";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="SELECT payplan.*,MAX(claimproc.ClaimNum) ClaimNum";
			}
			else {
				command+="SELECT payplan.PayPlanNum,payplan.PatNum,payplan.Guarantor,payplan.PayPlanDate,"
					+"payplan.APR,payplan.Note,payplan.PlanNum,payplan.CompletedAmt,payplan.InsSubNum,MAX(claimproc.ClaimNum) ClaimNum";
			}
			command+=" FROM payplan"
				+" LEFT JOIN claimproc ON claimproc.PayPlanNum=payplan.PayPlanNum"
				+" WHERE payplan.PatNum="+POut.Long(patNum)
				+" AND payplan.PlanNum="+POut.Long(planNum)
				+" AND payplan.InsSubNum="+POut.Long(insSubNum);
			if(claimNum>0) {
				command+=" AND (claimproc.ClaimNum IS NULL OR claimproc.ClaimNum="+POut.Long(claimNum)+")";//payplans with no claimprocs attached or only claimprocs from the same claim
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+=" GROUP BY payplan.PayPlanNum";
			}
			else {
				command+=" GROUP BY payplan.PayPlanNum,payplan.PatNum,payplan.Guarantor,payplan.PayPlanDate,"
					+"payplan.APR,payplan.Note,payplan.PlanNum,payplan.CompletedAmt,payplan.InsSubNum";
			}
			command+=" HAVING payplan.CompletedAmt>SUM(COALESCE(claimproc.InsPayAmt,0))";//has not been paid in full yet
			if(claimNum==0) {//if current claimproc is not attached to a claim, do not return payplans with claimprocs from existing claims already attached
				command+=" AND (MAX(claimproc.ClaimNum) IS NULL OR MAX(claimproc.ClaimNum)=0)";
			}
			command+=" ORDER BY payplan.PayPlanDate";
			DataTable payPlansWithClaimNum=Db.GetTable(command);
			List<PayPlan> retval=new List<PayPlan>();
			for(int i=0;i<payPlansWithClaimNum.Rows.Count;i++) {
				PayPlan planCur=new PayPlan();
				planCur.PayPlanNum=PIn.Long(payPlansWithClaimNum.Rows[i]["PayPlanNum"].ToString());
				planCur.PatNum=PIn.Long(payPlansWithClaimNum.Rows[i]["PatNum"].ToString());
				planCur.Guarantor=PIn.Long(payPlansWithClaimNum.Rows[i]["Guarantor"].ToString());
				planCur.PayPlanDate=PIn.Date(payPlansWithClaimNum.Rows[i]["PayPlanDate"].ToString());
				planCur.APR=PIn.Double(payPlansWithClaimNum.Rows[i]["APR"].ToString());
				planCur.Note=payPlansWithClaimNum.Rows[i]["Note"].ToString();
				planCur.PlanNum=PIn.Long(payPlansWithClaimNum.Rows[i]["PlanNum"].ToString());
				planCur.CompletedAmt=PIn.Double(payPlansWithClaimNum.Rows[i]["CompletedAmt"].ToString());
				planCur.InsSubNum=PIn.Long(payPlansWithClaimNum.Rows[i]["InsSubNum"].ToString());
				if(claimNum>0 && payPlansWithClaimNum.Rows[i]["ClaimNum"].ToString()==claimNum.ToString()) {
					//if a payplan exists with claimprocs from the same claim as the current claimproc attached, always only return that one payplan
					//claimprocs from one claim are not allowed to be attached to different payplans
					retval.Clear();
					retval.Add(planCur);
					break;
				}
				retval.Add(planCur);
			}
			return retval;
		}

		///<summary>Updates the TreatmentCompletedAmt field of the passed in payplans in the database.  
		///Used when a procedure attached to a payment plan charge is set complete or deleted.
		///The treatment completed amount only takes into account payplancharge credits that have already occured
		///(no charges attached to TP'd procs).</summary>
		public static void UpdateTreatmentCompletedAmt(List<PayPlan> listPayPlans) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPayPlans);
				return;
			}
			foreach(PayPlan payPlanCur in listPayPlans) {
				double completedAmt=0;
				List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payPlanCur.PayPlanNum);
				completedAmt=listCharges.Where(x => x.ChargeType==PayPlanChargeType.Credit)
					.Where(x => x.ChargeDate.Date <= DateTime.Today.Date)
					.Select(x => x.Principal)
					.Sum();
				payPlanCur.CompletedAmt=completedAmt;
				Update(payPlanCur);
			}
		}

		///<summary>Executes a LINQ statement that returns the total amount of tx that is both completed and planned for the passed in payment plan.
		///Only used for payplans v2.  Different from the TxCompletedAmt, which looks ONLY at PayPlanCharge credits that have already occurred. 
		///Does not update or make any calls to the database, as TxTotalAmt is not a db column.</summary>
		public static double GetTxTotalAmt(List<PayPlanCharge> listCharges) {
			//no need to check RemotingRole; no call to db.
			return listCharges.Where(x => x.ChargeType==PayPlanChargeType.Credit)
				.Select(x => x.Principal)
				.Sum();
		}

		///<summary>Updates the database preference "PayPlansVerion" and puts a payment plan credit into the ledger for all currently existing.
		///Pass in the version to update to.  Currently you can only update from version 1 to version 2.
		///Does NOT perform aging or generate the post-update changelog PDF.  (See FormPayPlanUpdate)</summary>
		public static bool UpdatePayPlanVersion(int versionNumber) {
			//No need to check RemotingRole; no call to db.
			switch(versionNumber) {
				case 2:					
					List<PayPlan> listPayPlanAll=GetAll();//get all payment plans
					foreach(PayPlan payPlanCur in listPayPlanAll) {//for each plan, create a new credit of the txCompletedAmt.
						//it should not be possible to have an existing payment plan without at least one payplancharge.
						List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(payPlanCur.PayPlanNum);
						//add a credit to the list of payplancharges for each payplan
						PayPlanCharge creditNew=new PayPlanCharge();
						creditNew.ChargeDate=payPlanCur.PayPlanDate;
						creditNew.ChargeType=PayPlanChargeType.Credit;
						creditNew.ClinicNum=listPayPlanCharges[0].ClinicNum;//Because all payplancharges for a payment plan have the same ClinicNum.
						creditNew.Guarantor=payPlanCur.Guarantor;
						creditNew.Note=Lans.g("AccountModule","Payment Plan Credit");
						creditNew.PatNum=payPlanCur.PatNum;
						creditNew.PayPlanNum=payPlanCur.PayPlanNum;
						creditNew.Principal=payPlanCur.CompletedAmt;
						creditNew.ProvNum=listPayPlanCharges[0].ProvNum;//Because all payplancharges for a payment plan have the same ProvNum.
						PayPlanCharges.Insert(creditNew);
					}
					Prefs.RefreshCache();//Because the prefs might not yet been refreshed in FormOpenDental if we are in the middle of updating.
					Prefs.UpdateInt(PrefName.PayPlansVersion,2);
					Prefs.RefreshCache();
					break;
				default:
					throw new Exception("Version 2 is the highest Payment Plan version currently available.");
		}
			return true;
		}

		public static List<PayPlan> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlan>>(MethodBase.GetCurrentMethod());
			}
			return Crud.PayPlanCrud.SelectMany("SELECT * FROM payplan");
		}

		///<summary></summary>
		public static long Insert(PayPlan payPlan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				payPlan.PayPlanNum=Meth.GetLong(MethodBase.GetCurrentMethod(),payPlan);
				return payPlan.PayPlanNum;
			}
			return Crud.PayPlanCrud.Insert(payPlan);
		}

		///<summary></summary>
		public static void Update(PayPlan payPlan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payPlan);
				return;
			}
			Crud.PayPlanCrud.Update(payPlan);
		}

		///<summary>Called from FormPayPlan.  Also deletes all attached payplancharges.  Throws exception if there are any paysplits attached.</summary>
		public static void Delete(PayPlan plan){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),plan);
				return;
			}
			string command;
			if(plan.PlanNum==0) {  //Patient payment plan
				command="SELECT COUNT(*) FROM paysplit WHERE PayPlanNum="+plan.PayPlanNum.ToString();
				if(Db.GetCount(command)!="0") {
					throw new ApplicationException
						(Lans.g("PayPlans","You cannot delete a payment plan with patient payments attached.  Unattach the payments first."));
				}
			}
			else {  //Insurance payment plan
				command="SELECT COUNT(*) FROM claimproc WHERE PayPlanNum="+plan.PayPlanNum.ToString();
				if(Db.GetCount(command)!="0") {
					throw new ApplicationException
						(Lans.g("PayPlans","You cannot delete a payment plan with insurance payments attached.  Unattach the payments first."));
				}
			}
			command="DELETE FROM payplancharge WHERE PayPlanNum="+plan.PayPlanNum.ToString();
			Db.NonQ(command);
			command="DELETE FROM payplan WHERE PayPlanNum ="+plan.PayPlanNum.ToString();
			Db.NonQ(command);
		}

		/// <summary>Gets info directly from database. Used from PayPlan and Account windows to get the amount paid so far on one payment plan.</summary>
		public static double GetAmtPaid(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command="SELECT SUM(paysplit.SplitAmt) FROM paysplit "
				+"WHERE paysplit.PayPlanNum = '"+payPlanNum.ToString()+"' "
				+"GROUP BY paysplit.PayPlanNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return 0;
			}
			return PIn.Double(table.Rows[0][0].ToString());
		}

		/// <summary>Gets info directly from database. Used when adding a payment.</summary>
		public static bool PlanIsPaidOff(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command="SELECT SUM(paysplit.SplitAmt) FROM paysplit "
				+"WHERE PayPlanNum = "+POut.Long(payPlanNum);// +"' "
				//+" GROUP BY paysplit.PayPlanNum";
			double amtPaid=PIn.Double(Db.GetScalar(command));
			command="SELECT SUM(Principal+Interest) FROM payplancharge "
				+"WHERE ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" AND PayPlanNum="+POut.Long(payPlanNum);
			double totalCost=PIn.Double(Db.GetScalar(command));
			if(totalCost-amtPaid < .01) {
				return true;
			}
			return false;
		}

		///<summary>Used from FormPayPlan, Account, and ComputeBal to get the accumulated amount due for a payment plan based on today's date.  
		///Includes interest, but does not include payments made so far.  The chargelist must include all charges for this payplan, 
		///but it can include more as well.</summary>
		public static double GetAccumDue(long payPlanNum, List<PayPlanCharge> chargeList){
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<chargeList.Count;i++){
				if(chargeList[i].PayPlanNum!=payPlanNum){
					continue;
				}
				if(chargeList[i].ChargeDate>DateTime.Today){//not due yet
					continue;
				}
				if(chargeList[i].ChargeType!=PayPlanChargeType.Debit) { //for v1, debits are the only ChargeType.
					continue;
				}
				retVal+=chargeList[i].Principal+chargeList[i].Interest;
			}
			return retVal;
		}

		public static List<PayPlan> GetAllForCharges(List<PayPlanCharge> listCharge) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlan>>(MethodBase.GetCurrentMethod(),listCharge);
			}
			if(listCharge.Count==0) {
				return new List<PayPlan>();
			}
			string command="SELECT * FROM payplan "
				+"WHERE PayPlanNum IN ("+String.Join(",",listCharge.Select(x => x.PayPlanNum))+")";
			return Crud.PayPlanCrud.SelectMany(command);
		}

		/// <summary>Used from Account window to get the amount paid so far on one payment plan.  
		/// Must pass in the total amount paid and the returned value will not be more than this.  
		/// The chargelist must include all charges for this payplan, but it can include more as well.  
		/// It will loop sequentially through the charges to get just the principal portion.</summary>
		public static double GetPrincPaid(double amtPaid,long payPlanNum,List<PayPlanCharge> chargeList) {
			//No need to check RemotingRole; no call to db.
			//amtPaid gets reduced to 0 throughout this loop.
			double retVal=0;
			for(int i=0;i<chargeList.Count;i++){
				if(chargeList[i].PayPlanNum!=payPlanNum){
					continue;
				}
				if(chargeList[i].ChargeType!=PayPlanChargeType.Debit) { //for v1, debits are the only ChargeType.
					continue;
				}
				//For this charge, first apply payment to interest
				if(amtPaid>chargeList[i].Interest){
					amtPaid-=chargeList[i].Interest;
				}
				else{//interest will eat up the remainder of the payment
					amtPaid=0;
					break;
				}
				//Then, apply payment to principal
				if(amtPaid>chargeList[i].Principal){
					retVal+=chargeList[i].Principal;
					amtPaid-=chargeList[i].Principal;
				}
				else{//principal will eat up the remainder of the payment
					retVal+=amtPaid;
					amtPaid=0;
					break;
				}
			}
			return retVal;
		}

		/// <summary>Used from Account and ComputeBal to get the total amount of the original principal for one payment plan.
		/// Does not include any interest. The chargelist must include all charges for this payplan, but it can include more as well.</summary>
		public static double GetTotalPrinc(long payPlanNum,List<PayPlanCharge> chargeList) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<chargeList.Count;i++){
				if(chargeList[i].PayPlanNum!=payPlanNum){
					continue;
				}
				if(chargeList[i].ChargeType!=PayPlanChargeType.Debit) { //for v1, debits are the only ChargeType.
					continue;
				}
				retVal+=chargeList[i].Principal;
			}
			return retVal;
		}

		///<summary>Returns the sum of all payment plan entries for guarantor and/or patient.</summary>
		public static double ComputeBal(long patNum,PayPlan[] list,List<PayPlanCharge> chargeList) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<list.Length;i++){
				//one or both of these conditions may be met:
				if(list[i].Guarantor==patNum){
					retVal+=GetAccumDue(list[i].PayPlanNum,chargeList);//Internally considers ChargeType
				}
				if(list[i].PatNum==patNum){
					retVal-=GetTotalPrinc(list[i].PayPlanNum,chargeList);//Internally considers ChargeType
				}
			}
			return retVal;
		}

		


	}

	

	


}










