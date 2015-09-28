using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpPayPlan {

		///<summary>If not using clinics then supply an empty list of clinicNums.</summary>
		public static DataTable GetPayPlanTable(DateTime dateStart,DateTime dateEnd,List<long> listProvNums,List<long> listClinicNums,
			bool hasAllProvs,DisplayPayPlanType displayPayPlanType,bool hideCompletedPlans,bool showFamilyBalance,bool hasDateRange) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listProvNums,listClinicNums,hasAllProvs,displayPayPlanType,
					hideCompletedPlans,showFamilyBalance,hasDateRange);
			}
			string whereProv="";
			if(!hasAllProvs) {
				whereProv+=" AND payplancharge.ProvNum IN(";
				for(int i=0;i<listProvNums.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+=POut.Long(listProvNums[i]);
				}
				whereProv+=") ";
			}
			string whereClin="";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				whereClin+=" AND payplancharge.ClinicNum IN(";
				for(int i=0;i<listClinicNums.Count;i++) {
					if(i>0) {
						whereClin+=",";
					}
					whereClin+=POut.Long(listClinicNums[i]);
				}
				whereClin+=") ";
			}
			DataTable table=new DataTable();
			table.Columns.Add("provider");
			table.Columns.Add("guarantor");
			table.Columns.Add("ins");
			table.Columns.Add("princ");
			table.Columns.Add("paid");
			table.Columns.Add("due");
			table.Columns.Add("balance");
			table.Columns.Add("clinicname");
			DataRow row;
			string datesql="CURDATE()";//This is used to find out how much people owe currently and has nothing to do with the selected range
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				datesql="(SELECT CURRENT_DATE FROM dual)";
			}
			//Oracle TODO:  Either put entire query without GROUP BY in SUBSELECT and then GROUP BY outside, or rewrite query to use joins instead of subselects.
			string command="SELECT FName,LName,MiddleI,PlanNum,Preferred,PlanNum, "
				+"COALESCE((SELECT SUM(Principal+Interest) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
					+"AND ChargeDate <= "+datesql+@"),0) '_accumDue', ";
			command+="COALESCE((SELECT SUM(SplitAmt) FROM paysplit WHERE paysplit.PayPlanNum=payplan.PayPlanNum AND paysplit.PayPlanNum!=0),0) '_paid', ";
			command+="COALESCE((SELECT SUM(InsPayAmt) FROM claimproc WHERE claimproc.PayPlanNum=payplan.PayPlanNum "
					+"AND claimproc.Status IN("
					+POut.Int((int)ClaimProcStatus.Received)+","
					+POut.Int((int)ClaimProcStatus.Supplemental)+","
					+POut.Int((int)ClaimProcStatus.CapClaim)
					+") AND claimproc.PayPlanNum!=0),0) '_insPaid', ";
			command+="COALESCE((SELECT SUM(Principal) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum),0) '_principal', "
				+"patient.PatNum PatNum, "
				+"payplancharge.ProvNum ProvNum ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				command+=", payplancharge.ClinicNum ClinicNum ";
			}
			//In order to determine if the patient has completely paid off their payment plan we need to get the total amount of interest as of today.
			//Then, after the query has run, we'll add the interest up until today with the total principal for the entire payment plan.
			//For this reason, we cannot use _accumDue which only gets the principle up until today and not the entire payment plan principle.
			command+=",COALESCE((SELECT SUM(Interest) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
					+"AND ChargeDate <= "+datesql+@"),0) '_interest' "
				+"FROM payplan "
				+"LEFT JOIN patient ON patient.PatNum=payplan.Guarantor "
				+"LEFT JOIN payplancharge ON payplan.PayPlanNum=payplancharge.PayPlanNum "
				+"WHERE TRUE ";//Always include true, so that the WHERE clause may always be present.
			if(hasDateRange) {
				command+="AND payplan.PayPlanDate >= "+POut.Date(dateStart)+" "
				+"AND payplan.PayPlanDate <= "+POut.Date(dateEnd)+" ";
			}
			command+=whereProv
				+whereClin;
			if(displayPayPlanType==DisplayPayPlanType.Insurance) {
				command+="AND payplan.PlanNum!=0 ";
			}
			else if(displayPayPlanType==DisplayPayPlanType.Patient) {
				command+="AND payplan.PlanNum=0 ";
			}
			else if(displayPayPlanType==DisplayPayPlanType.Both) {
				//Do not filter the query at all which will show both insurance and patient payment plan types.
			}
			command+="GROUP BY FName,LName,MiddleI,Preferred,payplan.PayPlanNum ";
			if(hideCompletedPlans) {
				command+="HAVING _paid+_insPaid < _principal ";
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				command+="ORDER BY ClinicNum,LName,FName";
			}
			else {
				command+="ORDER BY LName,FName";
			}
			DataTable raw=ReportsComplex.GetTable(command);
			//DateTime payplanDate;
			Patient pat;
			double princ;
			double paid;
			double interest;
			double accumDue;
			for(int i=0;i<raw.Rows.Count;i++) {
				princ=PIn.Double(raw.Rows[i]["_principal"].ToString());
				interest=PIn.Double(raw.Rows[i]["_interest"].ToString());
				if(raw.Rows[i]["PlanNum"].ToString()=="0") {//pat payplan
					paid=PIn.Double(raw.Rows[i]["_paid"].ToString());
				}
				else {//ins payplan
					paid=PIn.Double(raw.Rows[i]["_insPaid"].ToString());
				}
				accumDue=PIn.Double(raw.Rows[i]["_accumDue"].ToString());
				if(hideCompletedPlans) {
					//We store all monetary amounts in our database as data type "double".  
					//Doubles cannot do precision math so some payment plans that are already paid off will get returned.
					//We need to skip any payment plans that are off by a fraction of a penny.
					//We cannot use _accumDue (which is principle (up to today) + interest (up to today)) because we need principle for the entire payment plan.
					double amountRemain=(princ+interest)-paid;
					if(amountRemain < 0.005) {
						continue;
					}
				}
				row=table.NewRow();
				//payplanDate=PIn.PDate(raw.Rows[i]["PayPlanDate"].ToString());
				//row["date"]=raw.Rows[i]["PayPlanDate"].ToString();//payplanDate.ToShortDateString();
				pat=new Patient();
				pat.LName=raw.Rows[i]["LName"].ToString();
				pat.FName=raw.Rows[i]["FName"].ToString();
				pat.MiddleI=raw.Rows[i]["MiddleI"].ToString();
				pat.Preferred=raw.Rows[i]["Preferred"].ToString();
				row["provider"]=Providers.GetLName(PIn.Long(raw.Rows[i]["ProvNum"].ToString()));
				row["guarantor"]=pat.GetNameLF();
				if(raw.Rows[i]["PlanNum"].ToString()=="0") {
					row["ins"]="";
				}
				else {
					row["ins"]="X";
				}
				row["princ"]=princ.ToString("f");
				row["paid"]=paid.ToString("f");
				row["due"]=(accumDue-paid).ToString("f");
				if(showFamilyBalance) {
					Family famCur=Patients.GetFamily(PIn.Long(raw.Rows[i]["PatNum"].ToString()));
					Decimal total=(decimal)famCur.ListPats[0].BalTotal;
					row["balance"]=(total - (decimal)famCur.ListPats[0].InsEst).ToString("F");
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
					string clinicDesc=Clinics.GetDesc(PIn.Long(raw.Rows[i]["ClinicNum"].ToString()));
					row["clinicname"]=(clinicDesc=="")?Lans.g("FormRpPayPlans","Unassigned"):clinicDesc;
				}
				table.Rows.Add(row);
			}
			return table;
		}

	}

	///<summary>Used to dictate which payment plan types are shown in the payment plan report.</summary>
	public enum DisplayPayPlanType {
		///<summary>0</summary>
		Patient,
		///<summary>1</summary>
		Insurance,
		///<summary>2</summary>
		Both
	}
}
