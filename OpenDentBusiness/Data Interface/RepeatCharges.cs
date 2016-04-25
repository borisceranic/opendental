using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class RepeatCharges {
		///<summary>Gets a list of all RepeatCharges for a given patient.  Supply 0 to get a list for all patients.</summary>
		public static RepeatCharge[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RepeatCharge[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM repeatcharge";
			if(patNum!=0) {
				command+=" WHERE PatNum = "+POut.Long(patNum);
			}
			command+=" ORDER BY DateStart";
			return Crud.RepeatChargeCrud.SelectMany(command).ToArray();
		}	

		///<summary></summary>
		public static void Update(RepeatCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			Crud.RepeatChargeCrud.Update(charge);
		}

		///<summary>HQ only. Called from WebServiceMainHQ.SmsSignAgreement and internal Broadcast Monitor too.
		///Create a monthly access repeating charge or update if already exists. 
		///Provided vlnMonthlyChargeUSD will only be applied if the RepeatCharge does not already exist.</summary>
		public static void UpsertForTextingAccess(long patNum,float vlnMonthlyChargeUSD) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,vlnMonthlyChargeUSD);
				return;
			}
			RepeatCharge[] repeateCharges=RepeatCharges.Refresh(patNum);
			RepeatCharge repeatCharge=null;
			if(repeateCharges!=null) {
				repeatCharge=repeateCharges.FirstOrDefault(x => x.ProcCode=="038");
			}
			if(repeatCharge==null) {
				repeatCharge=new RepeatCharge();
				repeatCharge.ProcCode="038";
				repeatCharge.PatNum=patNum;
				repeatCharge.IsEnabled=true;
				repeatCharge.DateStart=System.DateTime.Today; //post start date of today. Billing Cycle day will determine when charges are posted.
				repeatCharge.CreatesClaim=false;
				repeatCharge.CopyNoteToProc=false;
				//Previously, the charge was set to the default Fee.Amount for code 038. 
				//Now the charge will come from a static rate set in SMSNexmoRate table for this country code.
				repeatCharge.ChargeAmt=vlnMonthlyChargeUSD;
				//repeatCharge.ChargeAmt=Fees.GetFeesAllShallow(Fees.GetDict()).FirstOrDefault(f => f.CodeNum==ProcedureCodes.GetProcCode("038").CodeNum).Amount;
				repeatCharge.RepeatChargeNum=RepeatCharges.Insert(repeatCharge);
			}
			repeatCharge.IsEnabled=true;
			repeatCharge.DateStop=System.DateTime.MinValue;
			RepeatCharges.Update(repeatCharge);
		}

		///<summary></summary>
		public static long Insert(RepeatCharge charge) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				charge.RepeatChargeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),charge);
				return charge.RepeatChargeNum;
			}
			return Crud.RepeatChargeCrud.Insert(charge);
		}

		///<summary>Called from FormRepeatCharge.</summary>
		public static void Delete(RepeatCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			string command="DELETE FROM repeatcharge WHERE RepeatChargeNum ="+POut.Long(charge.RepeatChargeNum);
			Db.NonQ(command);
		}

		///<summary>For internal use only.  Returns all eRx repeating charges for all customers.</summary>
		public static List<RepeatCharge> GetForErx() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RepeatCharge>>(MethodBase.GetCurrentMethod());
			}
			//Does not need to be Oracle compatible because this is an internal tool only.
			string command="SELECT * FROM repeatcharge WHERE ProcCode REGEXP '^Z[0-9]{3,}$'";
			return Crud.RepeatChargeCrud.SelectMany(command);
		}

		///<summary>Get the list of all RepeatCharge rows. DO NOT REMOVE! Used by OD WebApps solution.</summary>
		// ReSharper disable once UnusedMember.Global
		public static List<RepeatCharge> GetAll() {
			//No need to check RemotingRole; no call to db.
			return Refresh(0).ToList();			
		}

		///<summary>Returns true if there are any active repeating charges on the patient's account, false if there are not.</summary>
		public static bool ActiveRepeatChargeExists(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum);
			}
			//Counts the number of repeat charges that a patient has with a valid start date in the past and no stop date or a stop date in the future
			string command="SELECT COUNT(*) FROM repeatcharge "
				+"WHERE PatNum="+POut.Long(patNum)+" AND DateStart BETWEEN '1880-01-01' AND "+DbHelper.Curdate()+" "
				+"AND (DateStop='0001-01-01' OR DateStop>="+DbHelper.Curdate()+")";
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}


	}

	

	


}










