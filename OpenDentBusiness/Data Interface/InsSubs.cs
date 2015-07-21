using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class InsSubs{
		///<summary>It's fastest if you supply a sub list that contains the sub, but it also works just fine if it can't initally locate the sub in the list.  You can supply an empty list.  If still not found, returns a new InsSub. The reason for the new InsSub is because it is common to immediately get an insplan using inssub.InsSubNum.  And, of course, that would fail if inssub was null.</summary>
		public static InsSub GetSub(long insSubNum,List<InsSub> subList) {
			//No need to check RemotingRole; no call to db.
			InsSub retVal=new InsSub();
			if(insSubNum==0) {
				return new InsSub();
			}
			if(subList==null) {
				subList=new List<InsSub>();
			}
			bool found=false;
			for(int i=0;i<subList.Count;i++) {
				if(subList[i].InsSubNum==insSubNum) {
					found=true;
					retVal=subList[i];
				}
			}
			if(!found) {
				retVal=GetOne(insSubNum);//retVal will now be null if not found
			}
			if(retVal==null) {
				//MessageBox.Show(Lans.g("InsPlans","Database is inconsistent.  Please run the database maintenance tool."));
				return new InsSub();
			}
			return retVal;
		}

		///<summary>Gets one InsSub from the db.</summary>
		public static InsSub GetOne(long insSubNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<InsSub>(MethodBase.GetCurrentMethod(),insSubNum);
			}
			return Crud.InsSubCrud.SelectOne(insSubNum);
		}

		///<summary>Gets new List for the specified family.  The only insSubs it misses are for claims with no current coverage.  These are handled as needed.</summary>
		public static List<InsSub> RefreshForFam(Family Fam) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsSub>>(MethodBase.GetCurrentMethod(),Fam);
			}
			//The command is written in a nested fashion in order to be compatible with both MySQL and Oracle.
			string command=
				"SELECT D.* FROM inssub D,"+
				"((SELECT A.InsSubNum FROM inssub A WHERE";
			//subscribers in family
			for(int i=0;i<Fam.ListPats.Length;i++) {
				if(i>0) {
					command+=" OR";
				}
				command+=" A.Subscriber="+POut.Long(Fam.ListPats[i].PatNum);
			}
			//in union, distinct is implied
			command+=") UNION (SELECT B.InsSubNum FROM inssub B,patplan P WHERE B.InsSubNum=P.InsSubNum AND (";
			for(int i=0;i<Fam.ListPats.Length;i++) {
				if(i>0) {
					command+=" OR";
				}
				command+=" P.PatNum="+POut.Long(Fam.ListPats[i].PatNum);
			}
			command+="))) C "
				+"WHERE D.InsSubNum=C.InsSubNum "
				+"ORDER BY "+DbHelper.UnionOrderBy("DateEffective",4);
			return Crud.InsSubCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(InsSub insSub){
			return Insert(insSub,false);
		}

		///<summary></summary>
		public static long Insert(InsSub insSub,bool useExistingPK){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				insSub.InsSubNum=Meth.GetLong(MethodBase.GetCurrentMethod(),insSub,useExistingPK);
				return insSub.InsSubNum;
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Crud.InsSubCrud.Insert(insSub);//Oracle ALWAYS uses existing PKs because they do not support auto-incrementing.
			}
			return Crud.InsSubCrud.Insert(insSub,useExistingPK);
		}

		///<summary></summary>
		public static void Update(InsSub insSub) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insSub);
				return;
			}
			Crud.InsSubCrud.Update(insSub);
		}

		///<summary>Throws exception if dependencies.  Doesn't delete anything else.</summary>
		public static void Delete(long insSubNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insSubNum);
				return;
			}
			try {
				ValidateNoKeys(insSubNum,true);
			}
			catch(ApplicationException ex) {
				throw new ApplicationException(Lans.g("FormInsPlan","Not allowed to delete: ")+ex.Message);
			}
			string command;
			DataTable table;
			//Remove from the patplan table just in case it is still there.
			command="SELECT PatPlanNum FROM patplan WHERE InsSubNum = "+POut.Long(insSubNum);
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				//benefits with this PatPlanNum are also deleted here
				PatPlans.Delete(PIn.Long(table.Rows[i]["PatPlanNum"].ToString()));
			}
			command="DELETE FROM claimproc WHERE InsSubNum = "+POut.Long(insSubNum);
			Db.NonQ(command);
			Crud.InsSubCrud.Delete(insSubNum);
		}

		/// <summary>Will throw an exception if this InsSub is being used anywhere. Set strict true to test against every check.</summary>
		public static void ValidateNoKeys(long subNum, bool strict){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),subNum,strict);
				return;
			}
			string command;
			string result;
			//claim.InsSubNum/2
			command="SELECT COUNT(*) FROM claim WHERE InsSubNum = "+POut.Long(subNum);
			result=Db.GetScalar(command);
			if(result!="0") {
				throw new ApplicationException(Lans.g("FormInsPlan","Subscriber has existing claims and so the subscriber cannot be deleted."));
			}
			command="SELECT COUNT(*) FROM claim WHERE InsSubNum2 = "+POut.Long(subNum);
			result=Db.GetScalar(command);
			if(result!="0") {
				throw new ApplicationException(Lans.g("FormInsPlan","Subscriber has existing claims and so the subscriber cannot be deleted."));
			}
			//claimproc.InsSubNum
			if(strict) {
				command="SELECT COUNT(*) FROM claimproc WHERE InsSubNum = "+POut.Long(subNum)+" AND Status != "+POut.Int((int)ClaimProcStatus.Estimate);//ignore estimates
				result=Db.GetScalar(command);
				if(result!="0") {
					throw new ApplicationException(Lans.g("FormInsPlan","Subscriber has existing claim procedures and so the subscriber cannot be deleted."));
				}
			}
			//etrans.InsSubNum
			command="SELECT COUNT(*) FROM etrans WHERE InsSubNum = "+POut.Long(subNum);
			result=Db.GetScalar(command);
			if(result!="0") {
				throw new ApplicationException(Lans.g("FormInsPlan","Subscriber has existing etrans entry and so the subscriber cannot be deleted."));
			}
			//payplan.InsSubNum
			command="SELECT COUNT(*) FROM payplan WHERE InsSubNum = "+POut.Long(subNum);
			result=Db.GetScalar(command);
			if(result!="0") {
				throw new ApplicationException(Lans.g("FormInsPlan","Subscriber has existing insurance linked payment plans and so the subscriber cannot be deleted."));
			}
		}

		/* jsalmon (11/15/2013) Depricated because inssubs should not be blindly deleted.
		///<summary>A quick delete that is only used when cancelling out of a new edit window.</summary>
		public static void Delete(long insSubNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insSubNum);
				return;
			}
			Crud.InsSubCrud.Delete(insSubNum);
		}
		 */

		///<summary>Used in FormInsSelectSubscr to get a list of insplans for one subscriber directly from the database.</summary>
		public static List<InsSub> GetListForSubscriber(long subscriber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsSub>>(MethodBase.GetCurrentMethod(),subscriber);
			}
			string command="SELECT * FROM inssub WHERE Subscriber="+POut.Long(subscriber);
			return Crud.InsSubCrud.SelectMany(command);
		}

		public static List<InsSub> GetListForPlanNum(long planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsSub>>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT * FROM inssub WHERE PlanNum="+POut.Long(planNum);
			return Crud.InsSubCrud.SelectMany(command);
		}

		///<summary>Only used once.  Gets a list of subscriber names from the database that have the specified plan. Used to display in the insplan window.  The returned list never includes the inssub that we're viewing.</summary>
		public static string[] GetSubscribersForPlan(long planNum,long excludeSub) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod(),planNum,excludeSub);
			}
			string command="SELECT CONCAT(CONCAT(LName,', '),FName) "
				+"FROM inssub LEFT JOIN patient ON patient.PatNum=inssub.Subscriber "
				+"WHERE inssub.PlanNum="+POut.Long(planNum)+" "
				+"AND inssub.InsSubNum !="+POut.Long(excludeSub)+" "
				+" ORDER BY LName,FName";
			DataTable table=Db.GetTable(command);
			string[] retStr=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				retStr[i]=PIn.String(table.Rows[i][0].ToString());
			}
			return retStr;
		}

		///<summary>Called from FormInsPlan when user wants to view a benefit note for other subscribers on a plan.  Should never include the current subscriber that the user is editing.  This function will get one note from the database, not including blank notes.  If no note can be found, then it returns empty string.</summary>
		public static string GetBenefitNotes(long planNum,long subNumExclude) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),planNum,subNumExclude);
			}
			string command="SELECT BenefitNotes FROM inssub WHERE BenefitNotes != '' AND PlanNum="+POut.Long(planNum)+" AND InsSubNum !="+POut.Long(subNumExclude)+" "+DbHelper.LimitAnd(1);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return "";
			}
			return PIn.String(table.Rows[0][0].ToString());
		}

		///<summary>Returns the number of subs affected.</summary>
		public static long SetAllSubsAssignBen() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command="UPDATE inssub SET AssignBen=0 WHERE AssignBen<>0";
			return Db.NonQ(command);
		}

		///<summary>This will assign all PlanNums to new value when Create New Plan If Needed is selected and there are multiple subscribers to a plan and an inssub object has been updated to point at a new PlanNum.  The PlanNum values need to be reflected in the claim, claimproc, payplan, and etrans tables, since those all both store inssub.InsSubNum and insplan.PlanNum.</summary>
		public static void SynchPlanNumsForNewPlan(InsSub SubCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),SubCur);
				return;
			}
			//claim.PlanNum
			string command="UPDATE claim SET claim.PlanNum="+POut.Long(SubCur.PlanNum)+" "
				+"WHERE claim.InsSubNum="+POut.Long(SubCur.InsSubNum)+" AND claim.PlanNum!="+POut.Long(SubCur.PlanNum);
			Db.NonQ(command);
			//claim.PlanNum2
			command="UPDATE claim SET claim.PlanNum2="+POut.Long(SubCur.PlanNum)+" "
				+"WHERE claim.InsSubNum2="+POut.Long(SubCur.InsSubNum)+" AND claim.PlanNum2!="+POut.Long(SubCur.PlanNum);
			Db.NonQ(command);
			//claimproc.PlanNum
			command="UPDATE claimproc SET claimproc.PlanNum="+POut.Long(SubCur.PlanNum)+" "
				+"WHERE claimproc.InsSubNum="+POut.Long(SubCur.InsSubNum)+" AND claimproc.PlanNum!="+POut.Long(SubCur.PlanNum);
			Db.NonQ(command);
			//payplan.PlanNum
			command="UPDATE payplan SET payplan.PlanNum="+POut.Long(SubCur.PlanNum)+" "
				+"WHERE payplan.InsSubNum="+POut.Long(SubCur.InsSubNum)+" AND payplan.PlanNum!="+POut.Long(SubCur.PlanNum);
			Db.NonQ(command);
			//etrans.PlanNum, only used if EtransType.BenefitInquiry270 and BenefitResponse271 and Eligibility_CA.
			command="UPDATE etrans SET etrans.PlanNum="+POut.Long(SubCur.PlanNum)+" "
				+"WHERE etrans.InsSubNum!=0 AND etrans.InsSubNum="+POut.Long(SubCur.InsSubNum)+" AND etrans.PlanNum!="+POut.Long(SubCur.PlanNum);
			Db.NonQ(command);
		}

		///<summary>Returns the number of subscribers moved.</summary>
		public static long MoveSubscribers(long insPlanNumFrom,long insPlanNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),insPlanNumFrom,insPlanNumTo);
			}
			List<InsSub> listInsSubsFrom=GetListForPlanNum(insPlanNumFrom);
			List<long> listBlockedPatNums=new List<long>();
			//Perform the same validation as when the user manually drops insplans from FormInsPlan using the Drop button.
			for(int i=0;i<listInsSubsFrom.Count;i++){
				InsSub insSubFrom=listInsSubsFrom[i];
				List<PatPlan> listPatPlanFrom=PatPlans.Refresh(insSubFrom.Subscriber);
				for(int j=0;j<listPatPlanFrom.Count;j++) {
					PatPlan patPlanFrom=listPatPlanFrom[j];
					//The following comments and logic are copied from the FormInsPlan Drop button...
					//If they have a claim for this ins with today's date, don't let them drop.
					//We already have code in place to delete claimprocs when we drop ins, but the claimprocs attached to claims are protected.
					//The claim clearly needs to be deleted if they are dropping.  We need the user to delete the claim before they drop the plan.
					//We also have code in place to add new claimprocs when they add the correct insurance.
					List<Claim> listClaims=Claims.Refresh(patPlanFrom.PatNum);//Get all claims for patient.
					for(int k=0;k<listClaims.Count;k++) {
						if(listClaims[k].PlanNum!=insPlanNumFrom) {//Make sure the claim is for the insurance plan we are about to change, not any other plans the patient might have.
							continue;
						}
						if(listClaims[k].DateService!=DateTime.Today) {//not today
							continue;
						}
						//Patient currently has a claim for the insplan they are trying to drop.
						if(!listBlockedPatNums.Contains(patPlanFrom.PatNum)) {
							listBlockedPatNums.Add(patPlanFrom.PatNum);
						}
					}
				}
			}
			if(listBlockedPatNums.Count>0) {
				StringBuilder sb=new StringBuilder();
				for(int i=0;i<listBlockedPatNums.Count;i++) {
					sb.Append("\r\n");
					Patient pat=Patients.GetPat(listBlockedPatNums[i]);
					sb.Append("#"+listBlockedPatNums[i]+" "+pat.GetNameFLFormal());
				}
				throw new ApplicationException(Lans.g("InsSubs","Before changing the subscribers on the insurance plan being moved from, please delete all of today's claims related to the insurance plan being moved from for the following patients")+":"+sb.ToString());
			}
			//This loop mimics some of the logic in PatPlans.Delete().
			int insSubMovedCount=0;
			for(int i=0;i<listInsSubsFrom.Count;i++){
				InsSub inssub=listInsSubsFrom[i];
				long oldInsSubNum=inssub.InsSubNum;
				inssub.InsSubNum=0;//This will allow us to insert a new record.
				inssub.PlanNum=insPlanNumTo;
				inssub.DateEffective=DateTime.MinValue;
				inssub.BenefitNotes="";
				inssub.SubscNote="";
				long insSubNumNew=InsSubs.Insert(inssub);
				string command="SELECT PatNum FROM patplan WHERE InsSubNum="+POut.Long(oldInsSubNum);
				DataTable tablePatsForInsSub=Db.GetTable(command);
				if(tablePatsForInsSub.Rows.Count==0) {
					continue;
				}
				insSubMovedCount++;
				for(int j=0;j<tablePatsForInsSub.Rows.Count;j++) {
					long patNum=PIn.Long(tablePatsForInsSub.Rows[j]["PatNum"].ToString());
					List<PatPlan> listPatPlans=PatPlans.Refresh(patNum);
					for(int k=0;k<listPatPlans.Count;k++) {
						PatPlan patPlan=listPatPlans[k];
						if(patPlan.InsSubNum==oldInsSubNum) {
							command="DELETE FROM benefit WHERE PatPlanNum=" +POut.Long(patPlan.PatPlanNum);//Delete patient specific benefits (rare).
							Db.NonQ(command);
							patPlan.InsSubNum=insSubNumNew;
							PatPlans.Update(patPlan);
						}
					}
					//Now that the plan has changed for the current subscriber, recalculate estimates.
					Family fam=Patients.GetFamily(patNum);
					Patient pat=fam.GetPatient(patNum);
					List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(patNum);
					List<Procedure> listProcs=Procedures.Refresh(patNum);
					listPatPlans=PatPlans.Refresh(patNum);
					List<InsSub> listInsSubs=InsSubs.RefreshForFam(fam);
					List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
					List<Benefit> listBenefits=Benefits.Refresh(listPatPlans,listInsSubs);
					Procedures.ComputeEstimatesForAll(patNum,listClaimProcs,listProcs,listInsPlans,listPatPlans,listBenefits,pat.Age,listInsSubs);
				}
			}
			InsPlan insPlanFrom=InsPlans.RefreshOne(insPlanNumFrom);
			insPlanFrom.IsHidden=true;
			InsPlans.Update(insPlanFrom);
			return insSubMovedCount;
		}

	}
}