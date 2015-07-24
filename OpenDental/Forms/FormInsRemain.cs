using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormInsRemain:Form {

		private Patient _patCur;
		private Family _famCur;

		public FormInsRemain(long selectedPatNum) {
			InitializeComponent();
			Lan.F(this);
			_famCur=Patients.GetFamily(selectedPatNum);
			_patCur=_famCur.GetPatient(selectedPatNum);
		}

		private void FormInsRemain_Load(object sender,EventArgs e) {
			SetGridCols();
			FillGrid();
			FillSummary();
		}

		private void SetGridCols() { //sizes can be changed as needed
			ODGridColumn col;
			gridRemainTimeUnits.BeginUpdate();
			gridRemainTimeUnits.Columns.Clear();
			col=new ODGridColumn("Category",165);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Qty",60);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Used",60);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Remaining",60);
			gridRemainTimeUnits.Columns.Add(col);
			gridRemainTimeUnits.EndUpdate();
		}

		private void FillGrid() {
			gridRemainTimeUnits.BeginUpdate();
			gridRemainTimeUnits.Rows.Clear();
			List<Benefit> listPatBenefits=Benefits.Refresh(PatPlans.Refresh(_patCur.PatNum),InsSubs.GetListForSubscriber(_patCur.PatNum));
			ODGridRow gridRow; 
			List<Procedure> listProcs;
			double amtUsed;
			int monthRenew;
			if(listPatBenefits.Count>0) {
				for(int i=0;i<listPatBenefits.Count;i++) {
					if(
						listPatBenefits[i].CovCatNum==0 //no category
						|| listPatBenefits[i].BenefitType!=InsBenefitType.Limitations //benefit type is not limitations
						|| (listPatBenefits[i].TimePeriod!=BenefitTimePeriod.CalendarYear && listPatBenefits[i].TimePeriod!=BenefitTimePeriod.ServiceYear) //neither calendar year nor serviceyear
						|| listPatBenefits[i].Quantity<=0 //quantity is 0 (maybe we should still show these benefits?)
						|| listPatBenefits[i].QuantityQualifier != BenefitQuantity.NumberOfServices //qualifier us not the number of services
						|| (listPatBenefits[i].CoverageLevel != BenefitCoverageLevel.Family && listPatBenefits[i].CoverageLevel != BenefitCoverageLevel.Individual)) //neither individual nor family coverage level
					{
						continue;
					}
					if(listPatBenefits[i].TimePeriod==BenefitTimePeriod.CalendarYear) { //for calendar year, get completed procs from January.01.CurYear ~ Curdate
						listProcs=Procedures.GetCompletedForDateRange(new DateTime(DateTime.Today.Year,1,1),DateTime.Today); //01/01/CurYear. is there a better way?
					}
					else { //if not calendar year, then it must be service year
						monthRenew=InsPlans.RefreshOne(listPatBenefits[i].PlanNum).MonthRenew; //monthrenew only stores the month as an int.
						//ternary op: if the current month >= month renew, use this year. otherwise, use last year. I can break this into another if-else if necessary.
						listProcs=Procedures.GetCompletedForDateRange(new DateTime(DateTime.Today.Month>=monthRenew?DateTime.Today.Year:DateTime.Today.Year-1,monthRenew,1),DateTime.Today);
					}
					amtUsed=GetAmtUsedForCat(listProcs,CovCats.GetCovCat(listPatBenefits[i].CovCatNum)); //calculate the amount used for one benefit. this is slightly inefficient. O(n^2)
					gridRow=new ODGridRow();
					gridRow.Cells.Add(CovCats.GetCovCat(listPatBenefits[i].CovCatNum).EbenefitCat.ToString()); //Coverage Category
					gridRow.Cells.Add(listPatBenefits[i].Quantity.ToString()); //Quantity	
					gridRow.Cells.Add(amtUsed.ToString()); //Used
					gridRow.Cells.Add((listPatBenefits[i].Quantity-amtUsed>0?listPatBenefits[i].Quantity-amtUsed:0).ToString()); //Remaining: if (Quantity - Used) < 0, then 0. I can change this to a non-ternary op if need be
					gridRemainTimeUnits.Rows.Add(gridRow);
				}
			}
			gridRemainTimeUnits.EndUpdate();
		}
		
		/// <summary>Pass in a list of procedures and a covCat, return the sum of all CanadaTimeUnits of the procedures in that covCat as a double. Iterates through the list of procedures to make a list of procedurecodes.</summary>
		private double GetAmtUsedForCat(List<Procedure> listProcs,CovCat covCat) {
			List<ProcedureCode> listProcCodes = new List<ProcedureCode>();
			for(int i=0;i<listProcs.Count;i++) {
				listProcCodes.Add(ProcedureCodes.GetProcCode(listProcs[i].CodeNum));	//turn list of procedures into list of procedurecodes.
			}
			double total=0;//CanadaTimeUnits can be decimal numbers, like 0.5.
			for(int i=0;i<listProcCodes.Count;i++) { //for every procedurecode
				if(CovCats.GetCovCat(CovSpans.GetCat(listProcCodes[i].ProcCode)).EbenefitCat==covCat.EbenefitCat) { //if the covCat of that code is the same as the passed-in covCat
					total+=listProcCodes[i].CanadaTimeUnits; //add the Canada time units to the total.
				}
			}
			return total;
		}

		//all of the code below is copied directly from the account module, ContrAccount.FillSummary(). line 2459
		private void FillSummary() {
			textFamPriMax.Text="";
			textFamPriDed.Text="";
			textFamSecMax.Text="";
			textFamSecDed.Text="";
			textPriMax.Text="";
			textPriDed.Text="";
			textPriDedRem.Text="";
			textPriUsed.Text="";
			textPriPend.Text="";
			textPriRem.Text="";
			textSecMax.Text="";
			textSecDed.Text="";
			textSecDedRem.Text="";
			textSecUsed.Text="";
			textSecPend.Text="";
			textSecRem.Text="";
			if(_patCur==null) {
				return;
			}
			double maxFam=0;
			double maxInd=0;
			double ded=0;
			double dedFam=0;
			double dedRem=0;
			double remain=0;
			double pend=0;
			double used=0;
			InsPlan PlanCur;
			InsSub SubCur;
			List<InsSub> subList=InsSubs.RefreshForFam(_famCur);
			List<InsPlan> InsPlanList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> PatPlanList=PatPlans.Refresh(_patCur.PatNum);
			List<Benefit> BenefitList=Benefits.Refresh(PatPlanList,subList);
			List<Claim> ClaimList=Claims.Refresh(_patCur.PatNum);
			List<ClaimProcHist> HistList=ClaimProcs.GetHistList(_patCur.PatNum,BenefitList,PatPlanList,InsPlanList,DateTimeOD.Today,subList);
			if(PatPlanList.Count>0) {
				SubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[0].PatPlanNum,-1,_patCur.PatNum,PatPlanList[0].InsSubNum,BenefitList);
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,BenefitList,_patCur.PatNum,PatPlanList[0].InsSubNum);
				textPriPend.Text=pend.ToString("F");
				textPriUsed.Text=used.ToString("F");
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,false);
				if(maxFam==-1) {
					textFamPriMax.Text="";
				}
				else {
					textFamPriMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textPriMax.Text="";
					textPriRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamPriMax.Text=max.ToString("F");
					textPriMax.Text=maxInd.ToString("F");
					textPriRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textPriDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,_patCur.PatNum,ded,dedFam);
					textPriDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamPriDed.Text=dedFam.ToString("F");
				}
			}
			if(PatPlanList.Count>1) {
				SubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[1].PatPlanNum,-1,_patCur.PatNum,PatPlanList[1].InsSubNum,BenefitList);
				textSecPend.Text=pend.ToString("F");
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,BenefitList,_patCur.PatNum,PatPlanList[1].InsSubNum);
				textSecUsed.Text=used.ToString("F");
				//max=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum);
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,false);
				if(maxFam==-1) {
					textFamSecMax.Text="";
				}
				else {
					textFamSecMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textSecMax.Text="";
					textSecRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamSecMax.Text=max.ToString("F");
					textSecMax.Text=maxInd.ToString("F");
					textSecRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textSecDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,_patCur.PatNum,ded,dedFam);
					textSecDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamSecDed.Text=dedFam.ToString("F");
				}
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}