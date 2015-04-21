using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

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

		///<summary>Gets all payplan charges for the payplans passed in such that the Guarantor is the specified patient.  Based on today's date.  The patNum is for the current patient.</summary>
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

		///<summary></summary>
		public static void DeleteAllInPlan(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payPlanNum);
				return;
			}
			string command="DELETE FROM payplancharge WHERE PayPlanNum="+payPlanNum.ToString();
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