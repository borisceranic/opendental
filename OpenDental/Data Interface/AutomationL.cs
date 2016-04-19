using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental {
	public class AutomationL {
		///<summary>ProcCodes will be null unless trigger is CompleteProcedure.
		///This routine will generally fail silently.  Will return true if a trigger happened.</summary>
		public static bool Trigger(AutomationTrigger trigger,List<string> procCodes,long patNum,long aptNum=0) {
			if(patNum==0) {//Could happen for OpenPatient trigger
				return false;
			}
			bool automationHappened=false;
			for(int i=0;i<Automations.Listt.Count;i++) {
				if(Automations.Listt[i].Autotrigger!=trigger) {
					continue;
				}
				if(trigger==AutomationTrigger.CompleteProcedure) {
					if(procCodes==null || procCodes.Count==0) {
						continue;//fail silently
					}
					string[] arrayCodes=Automations.Listt[i].ProcCodes.Split(',');
					if(procCodes.All(x => !arrayCodes.Contains(x))) {
						continue;
					}
				}
				//matching automation item has been found
				//Get possible list of conditions that exist for this automation item
				List<AutomationCondition> autoConditionsList=AutomationConditions.GetListByAutomationNum(Automations.Listt[i].AutomationNum);
				if(autoConditionsList.Count>0 && !CheckAutomationConditions(autoConditionsList,patNum)) {
					continue;
				}
				SheetDef sheetDef;
				Sheet sheet;
				FormSheetFillEdit FormSF;
				switch(Automations.Listt[i].AutoAction) {
					case AutomationAction.CreateCommlog:
						Commlog commlogCur=new Commlog();
						commlogCur.PatNum=patNum;
						commlogCur.CommDateTime=DateTime.Now;
						commlogCur.CommType=Automations.Listt[i].CommType;
						commlogCur.Note=Automations.Listt[i].MessageContent;
						commlogCur.Mode_=CommItemMode.None;
						commlogCur.UserNum=Security.CurUser.UserNum;
						FormCommItem FormCI=new FormCommItem(commlogCur);
						FormCI.IsNew=true;
						FormCI.ShowDialog();
						automationHappened=true;
						continue;
					case AutomationAction.PopUp:
						MessageBox.Show(Automations.Listt[i].MessageContent);
						automationHappened=true;
						continue;
					case AutomationAction.PrintPatientLetter:
					case AutomationAction.ShowExamSheet:
						sheetDef=SheetDefs.GetSheetDef(Automations.Listt[i].SheetDefNum);
						sheet=SheetUtil.CreateSheet(sheetDef,patNum);
						SheetParameter.SetParameter(sheet,"PatNum",patNum);
						SheetFiller.FillFields(sheet);
						using(Bitmap bmp=new Bitmap(100,100))//a dummy bitmap for the graphics object
						using(Graphics g=Graphics.FromImage(bmp)) {
							SheetUtil.CalculateHeights(sheet,g);
						}
						FormSF=new FormSheetFillEdit(sheet);
						FormSF.ShowDialog();
						automationHappened=true;
						continue;
					case AutomationAction.PrintReferralLetter:
						long referralNum=RefAttaches.GetReferralNum(patNum);
						if(referralNum==0) {
							MsgBox.Show("Automations","This patient has no referral source entered.");
							automationHappened=true;
							continue;
						}
						sheetDef=SheetDefs.GetSheetDef(Automations.Listt[i].SheetDefNum);
						sheet=SheetUtil.CreateSheet(sheetDef,patNum);
						SheetParameter.SetParameter(sheet,"PatNum",patNum);
						SheetParameter.SetParameter(sheet,"ReferralNum",referralNum);
						SheetFiller.FillFields(sheet);
						using(Bitmap bmp=new Bitmap(100,100))//a dummy bitmap for the graphics object
						using(Graphics g=Graphics.FromImage(bmp)) {
							SheetUtil.CalculateHeights(sheet,g);
						}
						FormSF=new FormSheetFillEdit(sheet);
						FormSF.ShowDialog();
						automationHappened=true;
						continue;
					case AutomationAction.SetApptStatus:
						if(Automations.Listt[i].AptStatus==ApptStatus.None) {
							MsgBox.Show("Automations","Invalid appointment status set in automation.");
							automationHappened=true;
							continue;
						}
						Appointment aptNew=Appointments.GetOneApt(aptNum);
						if(aptNew==null) {
							MsgBox.Show("Automations","Invalid appointment for automation.");
							automationHappened=true;
							continue;
						}
						Appointment aptOld=aptNew.Clone();
						aptNew.AptStatus=Automations.Listt[i].AptStatus;
						Appointments.Update(aptNew,aptOld);
						continue;
				}
			}
			return automationHappened;
		}

		private static bool CheckAutomationConditions(List<AutomationCondition> autoConditionsList,long patNum) {
			//Make sure every condition returns true
			for(int i=0;i<autoConditionsList.Count;i++) {
				switch(autoConditionsList[i].CompareField) {
					case AutoCondField.NeedsSheet:
						if(NeedsSheet(autoConditionsList[i],patNum)) {
							return false;
						}
						break;
					case AutoCondField.Problem:
						if(!ProblemComparison(autoConditionsList[i],patNum))	{
							return false;
						}
						break;
					case AutoCondField.Medication:
						if(!MedicationComparison(autoConditionsList[i],patNum))	{
							return false;
						}
						break;
					case AutoCondField.Allergy:
						if(!AllergyComparison(autoConditionsList[i],patNum))	{
							return false;
						}
						break;
					case AutoCondField.Age:
						if(!AgeComparison(autoConditionsList[i],patNum)) {
							return false;
						}
						break;
					case AutoCondField.Gender:
						if(!GenderComparison(autoConditionsList[i],patNum)) {
							return false;
						}
						break;
					case AutoCondField.Labresult:
						if(!LabresultComparison(autoConditionsList[i],patNum)) {
							return false;
						}
						break;
					case AutoCondField.InsuranceNotEffective:
						if(!InsuranceNotEffectiveComparison(autoConditionsList[i],patNum)) {
							return false;
						}
						break;
				}
			}
			return true;
		}

		#region Comparisons
		private static bool NeedsSheet(AutomationCondition autoCond, long patNum) {
			List<Sheet> sheetList=Sheets.GetForPatientForToday(patNum);
			switch(autoCond.Comparison) {//Find out what operand to use.
				case AutoCondComparison.Equals:
					//Loop through every sheet to find one that matches the condition.
					for(int i=0;i<sheetList.Count;i++) {
						if(sheetList[i].Description==autoCond.CompareString) {//Operand based on AutoCondComparison.
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for(int i=0;i<sheetList.Count;i++) {
						if(sheetList[i].Description.ToLower().Contains(autoCond.CompareString.ToLower())) {
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool ProblemComparison(AutomationCondition autoCond,long patNum) {
			List<Disease> problemList=Diseases.Refresh(patNum,true);
			switch(autoCond.Comparison) {//Find out what operand to use.
				case AutoCondComparison.Equals:
					for(int i=0;i<problemList.Count;i++) {//Includes hidden
						if(DiseaseDefs.GetName(problemList[i].DiseaseDefNum)==autoCond.CompareString) {
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for(int i=0;i<problemList.Count;i++) {
						if(DiseaseDefs.GetName(problemList[i].DiseaseDefNum).ToLower().Contains(autoCond.CompareString.ToLower())) {
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool MedicationComparison(AutomationCondition autoCond,long patNum) {
			List<Medication> medList=Medications.GetMedicationsByPat(patNum);
			switch(autoCond.Comparison) {
				case AutoCondComparison.Equals:
					for(int i=0;i<medList.Count;i++) {
						if(medList[i].MedName==autoCond.CompareString) {
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for(int i=0;i<medList.Count;i++) {
						if(medList[i].MedName.ToLower().Contains(autoCond.CompareString.ToLower())) {
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool AllergyComparison(AutomationCondition autoCond,long patNum) {
			List<Allergy> allergyList=Allergies.GetAll(patNum,false);
			switch(autoCond.Comparison) {
				case AutoCondComparison.Equals:
					for(int i=0;i<allergyList.Count;i++) {
						if(AllergyDefs.GetOne(allergyList[i].AllergyDefNum).Description==autoCond.CompareString) {
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for(int i=0;i<allergyList.Count;i++) {
						if(AllergyDefs.GetOne(allergyList[i].AllergyDefNum).Description.ToLower().Contains(autoCond.CompareString.ToLower())) {
							return true;
						}
					}
					break;
			}
			return false;
		}

		private static bool AgeComparison(AutomationCondition autoCond,long patNum) {
			Patient pat=Patients.GetPat(patNum);
			int age=pat.Age;
			int ageTrigger=0;
			if(!int.TryParse(autoCond.CompareString,out ageTrigger)){
				return false;//This is only possible due to an old bug that was fixed.
			}
			switch(autoCond.Comparison) {
				case AutoCondComparison.Equals:
					return (age==ageTrigger);
				case AutoCondComparison.Contains:
					return (age.ToString().Contains(autoCond.CompareString));
				case AutoCondComparison.GreaterThan:
					return (age>ageTrigger);
				case AutoCondComparison.LessThan:
					return (age<ageTrigger);
				default:
					return false;
			}
		}

		private static bool GenderComparison(AutomationCondition autoCond,long patNum) {
			Patient pat=Patients.GetPat(patNum);
			switch(autoCond.Comparison) {
				case AutoCondComparison.Equals:
					return (pat.Gender.ToString().Substring(0,1).ToLower()==autoCond.CompareString.ToLower());
				case AutoCondComparison.Contains:
					return (pat.Gender.ToString().Substring(0,1).ToLower().Contains(autoCond.CompareString.ToLower()));
				default:
					return false;
			}
		}

		private static bool LabresultComparison(AutomationCondition autoCond,long patNum) {
			List<LabResult> listResults=LabResults.GetAllForPatient(patNum);
			switch(autoCond.Comparison) {
				case AutoCondComparison.Equals:
					for(int i=0;i<listResults.Count;i++) {
						if(listResults[i].TestName==autoCond.CompareString) {
							return true;
						}
					}
					break;
				case AutoCondComparison.Contains:
					for(int i=0;i<listResults.Count;i++) {
						if(listResults[i].TestName.ToLower().Contains(autoCond.CompareString.ToLower())) {
							return true;
						}
					}
					break;
			}
			return false;
		}

		///<summary>Returns false if the insurance plan is effictve.  True if today is outside of the insurance effective date range.</summary>
		private static bool InsuranceNotEffectiveComparison(AutomationCondition autoCond,long patNum) {
			PatPlan patPlanCur=PatPlans.GetPatPlan(patNum,1);
			if(patPlanCur==null) {
				return true;
			}
			InsSub insSubCur=InsSubs.GetOne(patPlanCur.InsSubNum);
			if(insSubCur==null) {
				return true;
			}
			if(DateTime.Today>=insSubCur.DateEffective && DateTime.Today<=insSubCur.DateTerm) {
				return false;//Allen - Not not effective
			}
			return true;
		}
		#endregion




	}
}
