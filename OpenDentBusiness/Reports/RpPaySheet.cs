using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpPaySheet {

		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetInsTable(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,List<long> listInsuranceTypes,bool hasAllProvs,bool hasAllClinics,bool hasInsuranceTypes,bool isGroupedByPatient) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvNums,listClinicNums,listInsuranceTypes,hasAllProvs,hasAllClinics,hasInsuranceTypes,isGroupedByPatient);
			}
			string whereProv="";
			if(!hasAllProvs) {
				whereProv+=" AND claimproc.ProvNum IN(";
				for(int i=0;i<listProvNums.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+=POut.Long(listProvNums[i]);
				}
				whereProv+=") ";
			}
			string whereClin="";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				whereClin+=" AND claimproc.ClinicNum IN(";
				for(int i=0;i<listClinicNums.Count;i++) {
					if(i>0) {
						whereClin+=",";
					}
					whereClin+=POut.Long(listClinicNums[i]);
				}
				whereClin+=") ";
			}
			string queryIns=
				@"SELECT claimproc.DateCP,carrier.CarrierName,MAX("
+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+@") lfname,
provider.Abbr, ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryIns+="clinic.Description clinicDesc, ";
			}
			queryIns+=@"claimpayment.CheckNum,FORMAT(SUM(claimproc.InsPayAmt),2) amt,claimproc.ClaimNum,claimpayment.PayType 
				FROM claimproc
				LEFT JOIN insplan ON claimproc.PlanNum = insplan.PlanNum 
				LEFT JOIN patient ON claimproc.PatNum = patient.PatNum
				LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum
				LEFT JOIN provider ON provider.ProvNum=claimproc.ProvNum
				LEFT JOIN claimpayment ON claimproc.ClaimPaymentNum = claimpayment.ClaimPaymentNum ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryIns+="LEFT JOIN clinic ON clinic.ClinicNum=claimproc.ClinicNum ";
			}
			queryIns+="WHERE (claimproc.Status=1 OR claimproc.Status=4) "//received or supplemental
				+whereProv
				+whereClin
				+"AND claimpayment.CheckDate >= "+POut.Date(dateFrom)+" "
				+"AND claimpayment.CheckDate <= "+POut.Date(dateTo)+" ";
			if(!hasInsuranceTypes && listInsuranceTypes.Count>0) {
				queryIns+="AND claimpayment.PayType IN (";
				for(int i=0;i<listInsuranceTypes.Count;i++) {
					if(i>0) {
						queryIns+=",";
					}
					queryIns+=POut.Long(listInsuranceTypes[i]);
				}
				queryIns+=") ";
			}
			queryIns+=@"GROUP BY claimproc.DateCP,"
				+"claimproc.ClaimPaymentNum,provider.ProvNum,";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryIns+="claimproc.ClinicNum,clinic.Description,";
			}
			queryIns+="carrier.CarrierName,provider.Abbr,claimpayment.CheckNum";
			if(isGroupedByPatient) {
				queryIns+=",patient.PatNum";
			}
			queryIns+=" ORDER BY claimpayment.PayType,claimproc.DateCP,lfname";
			if(!hasInsuranceTypes && listInsuranceTypes.Count==0) {
				queryIns=DbHelper.LimitOrderBy(queryIns,0);
			}
			return Db.GetTable(queryIns);
		}

		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetPatTable(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,List<long> listPatientTypes,bool hasAllProvs,bool hasAllClinics,bool hasPatientTypes,bool isGroupedByPatient) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvNums,listClinicNums,listPatientTypes,hasAllProvs,hasAllClinics,hasPatientTypes,isGroupedByPatient);
			}
			//patient payments-----------------------------------------------------------------------------------------
			string whereProv="";
			if(!hasAllProvs) {
				whereProv+=" AND paysplit.ProvNum IN(";
				for(int i=0;i<listProvNums.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+=POut.Long(listProvNums[i]);
				}
				whereProv+=") ";
			}
			string whereClin="";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				whereClin+=" AND payment.ClinicNum IN(";
				for(int i=0;i<listClinicNums.Count;i++) {
					if(i>0) {
						whereClin+=",";
					}
					whereClin+=POut.Long(listClinicNums[i]);
				}
				whereClin+=") ";
			}
			string queryPat=
				@"SELECT payment.PayDate DatePay,MAX("
+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+@") lfname,provider.Abbr, ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryPat+="clinic.Description clinicDesc, ";
			}
			queryPat+=@"payment.CheckNum,
				FORMAT(SUM(paysplit.SplitAmt),2) amt, payment.PayNum,ItemName,payment.PayType 
				FROM payment
				LEFT JOIN paysplit ON payment.PayNum=paysplit.PayNum
				LEFT JOIN patient ON payment.PatNum=patient.PatNum
				LEFT JOIN provider ON paysplit.ProvNum=provider.ProvNum
				LEFT JOIN definition ON payment.PayType=definition.DefNum ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryPat+="LEFT JOIN clinic ON clinic.ClinicNum=payment.ClinicNum ";
			}
			queryPat+="WHERE 1 "
				+whereProv
				+whereClin
				+"AND paysplit.DatePay >= "+POut.Date(dateFrom)+" "
				+"AND paysplit.DatePay <= "+POut.Date(dateTo)+" ";
			if(!hasPatientTypes && listPatientTypes.Count>0) {
				queryPat+="AND (";
				for(int i=0;i<listPatientTypes.Count;i++) {
					if(i>0) {
						queryPat+="OR ";
					}
					queryPat+="payment.PayType = "+POut.Long(listPatientTypes[i])+" ";
				}
				queryPat+=") ";
			}
			queryPat+=@"GROUP BY payment.PayNum,payment.PayDate,provider.ProvNum,";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				queryPat+="payment.ClinicNum,clinic.Description,";
			}
			queryPat+="provider.Abbr,payment.CheckNum,definition.ItemName";
			if(isGroupedByPatient) {
				queryPat+=",patient.PatNum";
			}
			queryPat+=" ORDER BY payment.PayType,paysplit.DatePay,lfname";
			if(!hasPatientTypes && listPatientTypes.Count==0) {
				queryPat=DbHelper.LimitOrderBy(queryPat,0);
			}
			return Db.GetTable(queryPat);
		}

	}
}
