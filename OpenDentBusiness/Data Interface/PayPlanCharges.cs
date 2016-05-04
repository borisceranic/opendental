using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayPlanCharges {
		///<summary>Gets all PayPlanCharges for a guarantor or patient, ordered by date.</summary>
		public static List<PayPlanCharge> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanCharge>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE Guarantor='"+POut.Long(patNum)+"' "
				+"OR PatNum='"+POut.Long(patNum)+"' "
				+"ORDER BY ChargeDate";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<PayPlanCharge> GetForPayPlan(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanCharge>>(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE PayPlanNum="+POut.Long(payPlanNum)
				+" ORDER BY ChargeDate";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Gets all payplan charges for the payplans passed in such that the Guarantor is the specified patient.  Based on today's date.  
		///The patNum is for the current patient.  Will return both credits and debits.</summary>
		public static List<PayPlanCharge> GetDueForPayPlans(List<PayPlan> listPayPlans,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanCharge>>(MethodBase.GetCurrentMethod(),listPayPlans,patNum);
			}
			List<string> listPlanNums=new List<string>();
			for(int i=0;i<listPayPlans.Count;i++) {
				listPlanNums.Add(listPayPlans[i].PayPlanNum.ToString());
			}
			string command= "SELECT * FROM payplancharge "
				+"WHERE Guarantor="+POut.Long(patNum)+" AND PayPlanNum IN("+String.Join(", ",listPlanNums)+") "
				+"AND ChargeDate <= DATE(NOW()) ";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Takes a procNum and updates all of the dates of the payment plan charge credits associated to it.
		///If a completed procedure is passed in, it will update all of the payment plan charges associated to it to the ProcDate. 
		///If a non-complete procedure is passed in, it will update the charges associated to MaxValue.
		///Does nothing if there are no charges attached to the passed-in procedure.</summary>
		public static void UpdateAttachedPayPlanCharges(Procedure proc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),proc);
				return;
			}
			if(PrefC.GetInt(PrefName.PayPlansVersion)==1) {
				return;
			}
			List<PayPlanCharge> listCharges=GetFromProc(proc.ProcNum);
			foreach(PayPlanCharge chargeCur in listCharges) {
				chargeCur.ChargeDate=DateTime.MaxValue;
				if(proc.ProcStatus==ProcStat.C) {
					chargeCur.ChargeDate=proc.ProcDate;
				}
				Update(chargeCur); //one update statement for each payplancharge.
			}
			List<PayPlan> listPayPlans=PayPlans.GetAllForCharges(listCharges);
			PayPlans.UpdateTreatmentCompletedAmt(listPayPlans);
		}

		///<summary>Returns a list of procedures attached to the passed-in payment plan.  Returns an empty list if none.</summary>
		public static List<Procedure> GetPayPlanProcs(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Procedure>>(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			string command="SELECT procedurelog.* FROM procedurelog "
				+"INNER JOIN payplancharge ON payplancharge.ProcNum=procedurelog.ProcNum "
				+"AND payplancharge.PayPlanNum="+payPlanNum+" "
				+"GROUP BY procedurelog.ProcNum";
			return Crud.ProcedureCrud.SelectMany(command);
		}

		///<summary>Takes a procNum and returns a list of all payment plan charge credits associated to the procedure.
		///Returns an empty list if there are none.</summary>
		public static List<PayPlanCharge> GetFromProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanCharge>>(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT * FROM payplancharge WHERE payplancharge.ProcNum="+POut.Long(procNum);
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static PayPlanCharge GetOne(long payPlanChargeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PayPlanCharge>(MethodBase.GetCurrentMethod(),payPlanChargeNum);
			}
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE PayPlanChargeNum="+POut.Long(payPlanChargeNum);
			return Crud.PayPlanChargeCrud.SelectOne(command);
		}

		///<summary></summary>
		public static void Update(PayPlanCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			Crud.PayPlanChargeCrud.Update(charge);
		}

		///<summary></summary>
		public static long Insert(PayPlanCharge charge) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				charge.PayPlanChargeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),charge);
				return charge.PayPlanChargeNum;
			}
			return Crud.PayPlanChargeCrud.Insert(charge);
		}

		///<summary>Will delete all PayPlanCharges associated to the passed-in procNum from the database.  Does nothing if the procNum = 0.</summary>
		public static void DeleteForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procNum);
				return;
			}
			if(PrefC.GetInt(PrefName.PayPlansVersion) == 1 || procNum == 0) {
				return;
			}
			List<PayPlan> listPayPlans=PayPlans.GetAllForCharges(GetFromProc(procNum));
			string command="DELETE FROM payplancharge WHERE ProcNum="+POut.Long(procNum);
			Db.NonQ(command);
			PayPlans.UpdateTreatmentCompletedAmt(listPayPlans);
		}

		///<summary></summary>
		public static void Delete(PayPlanCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			string command= "DELETE from payplancharge WHERE PayPlanChargeNum = '"
				+POut.Long(charge.PayPlanChargeNum)+"'";
 			Db.NonQ(command);
		}	

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Must always pass in payPlanNum.</summary>
		public static void Sync(List<PayPlanCharge> listPayPlanCharges,long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPayPlanCharges,payPlanNum);
				return;
			}
			List<PayPlanCharge> listDB=PayPlanCharges.GetForPayPlan(payPlanNum);
			Crud.PayPlanChargeCrud.Sync(listPayPlanCharges,listDB);
		}
	}
}