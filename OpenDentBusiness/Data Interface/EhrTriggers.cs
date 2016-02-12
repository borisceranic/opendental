using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrTriggers{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all AutoTriggers.</summary>
		private static List<AutoTrigger> listt;

		///<summary>A list of all AutoTriggers.</summary>
		public static List<AutoTrigger> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM autotrigger ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="AutoTrigger";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.AutoTriggerCrud.TableToList(table);
		}
		#endregion
		*/

		public static List<EhrTrigger> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrTrigger>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM ehrtrigger";
			return Crud.EhrTriggerCrud.SelectMany(command);
		}

		///<summary>This is the first step of automation, this checks to see if the new object matches one of the trigger conditions. </summary>
		/// <param name="triggerObject">Can be DiseaseDef, ICD9, Icd10, Snomed, Medication, RxNorm, Cvx, AllerfyDef, EHRLabResult, Patient, or VitalSign.</param>
		/// <param name="patCur">Triggers and intervention are currently always dependant on current patient. </param>
		/// <returns>Returns a list of CDSInterventions. Should be used to generate CDS intervention message and later be passed to FormInfobutton for knowledge request.</returns>
		public static List<CDSIntervention> TriggerMatch(object triggerObject,Patient patCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CDSIntervention>>(MethodBase.GetCurrentMethod(),triggerObject,patCur);
			}
			string triggerObjectMessage;//Human readable description of trigers that were met.
			List<CDSIntervention> listInterventions=new List<CDSIntervention>();//return value
			List<EhrTrigger> listEhrTriggers=new List<EhrTrigger>();
			#region Construct EHR trigger list
			//Define objects to be used in matching triggers.
			DiseaseDef diseaseDef;
			ICD9 icd9;
			Icd10 icd10;
			Snomed snomed;
			Medication medication;
			RxNorm rxNorm;
			Cvx cvx;
			AllergyDef allergyDef;
			EhrLabResult ehrLabResult;
			Patient pat;
			Vitalsign vitalsign;
			triggerObjectMessage="";
			string command="";
			switch(triggerObject.GetType().Name) {
				case "DiseaseDef":
					diseaseDef=(DiseaseDef)triggerObject;
					command="SELECT * FROM ehrtrigger"
					+" WHERE ProblemDefNumList LIKE '% "+POut.String(diseaseDef.DiseaseDefNum.ToString())+" %'";// '% <code> %' so we get exact matches.
					if(diseaseDef.ICD9Code!="") {
						command+=" OR ProblemIcd9List LIKE '% "+POut.String(diseaseDef.ICD9Code)+" %'";
						ICD9 icd10Cur=ICD9s.GetByCode(diseaseDef.ICD9Code);
						if(icd10Cur!=null) {
							triggerObjectMessage+="  -"+diseaseDef.ICD9Code+"(Icd9)  "+icd10Cur.Description+"\r\n";
						}
					}
					if(diseaseDef.Icd10Code!="") {
						command+=" OR ProblemIcd10List LIKE '% "+POut.String(diseaseDef.Icd10Code)+" %'";
						Icd10 icd10Cur=Icd10s.GetByCode(diseaseDef.Icd10Code);
						if(icd10Cur!=null) {
							triggerObjectMessage+="  -"+diseaseDef.Icd10Code+"(Icd10)  "+icd10Cur.Description+"\r\n";
						}
					}
					if(diseaseDef.SnomedCode!="") {
						command+=" OR ProblemSnomedList LIKE '% "+POut.String(diseaseDef.SnomedCode)+" %'";
						Snomed snomedCur=Snomeds.GetByCode(diseaseDef.SnomedCode);
						if(snomedCur!=null) {
							triggerObjectMessage+="  -"+diseaseDef.SnomedCode+"(Snomed)  "+snomedCur.Description+"\r\n";
						}
					}
					break;
				case "ICD9":
					icd9=(ICD9)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger"
					+" WHERE Icd9List LIKE '% "+POut.String(icd9.ICD9Code)+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "Icd10":
					icd10=(Icd10)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger"
					+" WHERE Icd10List LIKE '% "+POut.String(icd10.Icd10Code)+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "Snomed":
					snomed=(Snomed)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger"
					+" WHERE SnomedList LIKE '% "+POut.String(snomed.SnomedCode)+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "Medication":
					medication=(Medication)triggerObject;
					RxNorm rxNormCur=RxNorms.GetByRxCUI(medication.RxCui.ToString());
					if(rxNormCur!=null) {
						triggerObjectMessage="  - "+medication.MedName+(medication.RxCui==0?"":" (RxCui:"+rxNormCur.RxCui+")")+"\r\n";
					}
					command="SELECT * FROM ehrtrigger"
					+" WHERE MedicationNumList LIKE '% "+POut.String(medication.MedicationNum.ToString())+" %'";// '% <code> %' so that we can get exact matches.
					if(medication.RxCui!=0) {
						command+=" OR RxCuiList LIKE '% "+POut.String(medication.RxCui.ToString())+" %'";// '% <code> %' so that we can get exact matches.
					}
					break;
				case "RxNorm":
					rxNorm=(RxNorm)triggerObject;
					triggerObjectMessage="  - "+rxNorm.Description+"(RxCui:"+rxNorm.RxCui+")\r\n";
					command="SELECT * FROM ehrtrigger"
					+" WHERE RxCuiList LIKE '% "+POut.String(rxNorm.RxCui)+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "Cvx":
					cvx=(Cvx)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger"
					+" WHERE CvxList LIKE '% "+POut.String(cvx.CvxCode)+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "AllergyDef":
					allergyDef=(AllergyDef)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger"
					+" WHERE AllergyDefNumList LIKE '% "+POut.String(allergyDef.AllergyDefNum.ToString())+" %'";// '% <code> %' so that we can get exact matches.
					break;
				case "EhrLabResult"://match loinc only, no longer 
					ehrLabResult=(EhrLabResult)triggerObject;
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger WHERE "
						+"(LabLoincList LIKE '% "+ehrLabResult.ObservationIdentifierID+" %'" //LOINC may be in one of two fields
						+"OR LabLoincList LIKE '% "+ehrLabResult.ObservationIdentifierIDAlt+" %')"; //LOINC may be in one of two fields
					break;
				case "Patient":
					pat=(Patient)triggerObject;
					List<string> triggerNums=new List<string>();
					//TODO: TriggerObjectMessage
					command="SELECT * FROM ehrtrigger WHERE DemographicsList !=''";
					List<EhrTrigger> triggers=Crud.EhrTriggerCrud.SelectMany(command);
					for(int i=0;i<triggers.Count;i++) {
						string[] arrayDemoItems=triggers[i].DemographicsList.ToLower().Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
						for(int j=0;j<arrayDemoItems.Length;j++) {
							switch(arrayDemoItems[j].Split(',')[0]) {
								case "age":
									int val=PIn.Int(Regex.Match(arrayDemoItems[j],@"\d+").Value);
									if(arrayDemoItems[j].Contains("=")) {//=, >=, or <=
										if(val==pat.Age) {
											triggerNums.Add(triggers[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayDemoItems[j].Contains("<")) {
										if(pat.Age<val) {
											triggerNums.Add(triggers[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayDemoItems[j].Contains(">")) {
										if(pat.Age>val) {
											triggerNums.Add(triggers[i].EhrTriggerNum.ToString());
											break;
										}
									}
									//should never happen, age element didn't contain a comparator
									break;
								case "gender":
									if(arrayDemoItems[j].Split(',').Contains(patCur.Gender.ToString().ToLower())) {
										triggerNums.Add(triggers[i].EhrTriggerNum.ToString());
									}
									break;
								default:
									break;//should never happen
							}
						}
					}
					triggerNums.Add("-1");//to ensure the querry is valid.
					command="SELECT * FROM ehrTrigger WHERE EhrTriggerNum IN ("+String.Join(",",triggerNums)+")";
					break;
				case "Vitalsign":
					List<string> trigNums=new List<string>();
					vitalsign=(Vitalsign)triggerObject;
					command="SELECT * FROM ehrtrigger WHERE VitalLoincList !=''";
					List<EhrTrigger> triggersVit=Crud.EhrTriggerCrud.SelectMany(command);
					for(int i=0;i<triggersVit.Count;i++) {
						string[] arrayVitalItems=triggersVit[i].VitalLoincList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
						for(int j=0;j<arrayVitalItems.Length;j++) {
							double val=PIn.Double(Regex.Match(arrayVitalItems[j],@"\d+(.(\d+))*").Value);//decimal value w or w/o decimal.
							switch(arrayVitalItems[j].Split(',')[0]) {
								case "height":
									if(arrayVitalItems[j].Contains("=")) {//=, >=, or <=
										if(vitalsign.Height==val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains("<")) {
										if(vitalsign.Height<val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains(">")) {
										if(vitalsign.Height>val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									//should never happen, Height element didn't contain a comparator
									break;
								case "weight":
									if(arrayVitalItems[j].Contains("=")) {//=, >=, or <=
										if(vitalsign.Weight==val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains("<")) {
										if(vitalsign.Weight<val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains(">")) {
										if(vitalsign.Weight>val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									break;
								case "BMI":
									float BMI=Vitalsigns.CalcBMI(vitalsign.Weight,vitalsign.Height);
									if(arrayVitalItems[j].Contains("=")) {//=, >=, or <=
										if(BMI==val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains("<")) {
										if(BMI<val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									if(arrayVitalItems[j].Contains(">")) {
										if(BMI>val) {
											trigNums.Add(triggersVit[i].EhrTriggerNum.ToString());
											break;
										}
									}
									break;
								case "BP":
									//TODO
									break;
							}//end switch
						}
					}//End Triggers Vit
					trigNums.Add("-1");//to ensure the querry is valid.
					command="SELECT * FROM ehrTrigger WHERE EhrTriggerNum IN ("+String.Join(",",trigNums)+")";
					break;
				default:
					//command="SELECT * FROM ehrtrigger WHERE false";//should not return any results.
					return null;
#if DEBUG
					throw new Exception(triggerObject.GetType().ToString()+" object not implemented as intervention trigger yet. Add to the list above to handle.");
#endif
					//break;
			}
			listEhrTriggers=Crud.EhrTriggerCrud.SelectMany(command);
			#endregion
			if(listEhrTriggers.Count==0){
				return null;//no triggers matched.
			}
			//Check for MatchCardinality.One type triggers.-------------------------------------------------------------------------------------------------
			for(int i=0;i<listEhrTriggers.Count;i++) {
				if(listEhrTriggers[i].Cardinality!=MatchCardinality.One) {
					continue;
				}
				string triggerMessage=listEhrTriggers[i].Description+":\r\n";//Example:"Patient over 55:\r\n"
				triggerMessage+=triggerObjectMessage;//Example:"  -Patient Age 67\r\n"
				List<object> ListObjectMatches=new List<object>();
				ListObjectMatches.Add(triggerObject);
				CDSIntervention cdsi=new CDSIntervention();
				cdsi.EhrTrigger=listEhrTriggers[i];
				cdsi.InterventionMessage=triggerMessage;
				cdsi.TriggerObjects=ListObjectMatches;
				listInterventions.Add(cdsi);
			}
			//Fill object lists to be checked---------------------------------------------------------------------------------------------------------------
			List<Allergy> listAllergies=Allergies.GetAll(patCur.PatNum,false);
			List<Disease> listDiseases=Diseases.Refresh(patCur.PatNum,true);
			List<DiseaseDef> listDiseaseDefs=new List<DiseaseDef>();
			List<EhrLab> listEhrLabs=EhrLabs.GetAllForPat(patCur.PatNum);
			//List<EhrLabResult> ListEhrLabResults=null;//Lab results are stored in a list in the EhrLab object.
			List<MedicationPat> listMedicationPats=MedicationPats.Refresh(patCur.PatNum,false);
			List<AllergyDef> listAllergyDefs=new List<AllergyDef>();
			for(int i=0;i<listAllergies.Count;i++){
				listAllergyDefs.Add(AllergyDefs.GetOne(listAllergies[i].AllergyDefNum));
			}
			for(int i=0;i<listDiseases.Count;i++){
				listDiseaseDefs.Add(DiseaseDefs.GetItem(listDiseases[i].DiseaseDefNum));
			}
			for(int i=0;i<listEhrTriggers.Count;i++) {
				if(listEhrTriggers[i].Cardinality==MatchCardinality.One) {
					continue;//we handled these above.
				}
				string triggerMessage=listEhrTriggers[i].Description+":\r\n";
				AddCDSIforOneOfEachTwoOrMoreAll(triggerObject,triggerMessage,patCur,listEhrTriggers[i],ref listInterventions,listMedicationPats,
					listAllergies,listDiseaseDefs,listEhrLabs);
			}//end triggers
			return listInterventions;
		}
		
		///<summary>Adds interventions for all cardinalities except "One".</summary>
		private static void AddCDSIforOneOfEachTwoOrMoreAll(object triggerObject,string triggerMessage,Patient patCur,EhrTrigger ehrTrig,
			ref List<CDSIntervention> listInterventions, List<MedicationPat> listMedicationPats,List<Allergy> listAllergies,
			List<DiseaseDef> listDiseaseDefs,List<EhrLab> listEhrLabs) 
		{
			//No remoting call; already checked before calling this method
			List<object> listObjectMatches=new List<object>();//Allergy, Disease, EhrLabResult, MedicationPat, Patient, VaccinePat, Demographic			
			//Problem
			for(int d=0;d<listDiseaseDefs.Count;d++) {
				if(listDiseaseDefs[d].ICD9Code!=""
					&& ehrTrig.ProblemIcd9List.Contains(" "+listDiseaseDefs[d].ICD9Code+" ")) {
					ICD9 currentICD9=ICD9s.GetByCode(listDiseaseDefs[d].ICD9Code);
					if(currentICD9!=null) {
						listObjectMatches.Add(listDiseaseDefs[d]);
						triggerMessage+="  -(ICD9) "+currentICD9.Description+"\r\n";
					}
					continue;
				}
				if(listDiseaseDefs[d].Icd10Code!=""
					&& ehrTrig.ProblemIcd10List.Contains(" "+listDiseaseDefs[d].Icd10Code+" ")) {
					Icd10 icd10Cur=Icd10s.GetByCode(listDiseaseDefs[d].Icd10Code);
					if(icd10Cur!=null) {
						listObjectMatches.Add(listDiseaseDefs[d]);
						triggerMessage+="  -(Icd10) "+icd10Cur.Description+"\r\n";
					}
					continue;
				}
				if(listDiseaseDefs[d].SnomedCode!=""
					&& ehrTrig.ProblemSnomedList.Contains(" "+listDiseaseDefs[d].SnomedCode+" ")) {
					Snomed snomedCur=Snomeds.GetByCode(listDiseaseDefs[d].SnomedCode);
					if(snomedCur!=null) {
						listObjectMatches.Add(listDiseaseDefs[d]);
						triggerMessage+="  -(Snomed) "+snomedCur.Description+"\r\n";
					}
					continue;
				}
				if(ehrTrig.ProblemDefNumList.Contains(" "+listDiseaseDefs[d].DiseaseDefNum+" ")) {
					listObjectMatches.Add(listDiseaseDefs[d]);
					triggerMessage+="  -(Problem Def) "+listDiseaseDefs[d].DiseaseName+"\r\n";
					continue;
				}
			}
			//Medication
			for(int m=0;m<listMedicationPats.Count;m++) {
				if(ehrTrig.MedicationNumList.Contains(" "+listMedicationPats[m].MedicationNum+" ")) {
					listObjectMatches.Add(listMedicationPats[m]);
					Medication medCur=Medications.GetMedication(listMedicationPats[m].MedicationNum);
					if(medCur==null) {
						continue;
					}
					triggerMessage+="  - "+medCur.MedName;
					RxNorm rxNormCur=RxNorms.GetByRxCUI(medCur.RxCui.ToString());
					if(rxNormCur==null) {
						continue;
					}
					triggerMessage+=(medCur.RxCui==0 ? "" : " (RxCui:"+rxNormCur.RxCui+")")+"\r\n";
					continue;
				}
				if(listMedicationPats[m].RxCui!=0
					&& ehrTrig.RxCuiList.Contains(" "+listMedicationPats[m].RxCui+" ")) {
					listObjectMatches.Add(listMedicationPats[m]);
					Medication medCur=Medications.GetMedication(listMedicationPats[m].MedicationNum);
					if(medCur==null) {
						continue;
					}
					triggerMessage+="  - "+medCur.MedName;
					RxNorm rxNormCur=RxNorms.GetByRxCUI(medCur.RxCui.ToString());
					if(rxNormCur==null) {
						continue;
					}
					triggerMessage+=(medCur.RxCui==0 ? "" : " (RxCui:"+rxNormCur.RxCui+")")+"\r\n";
					continue;
				}
				//Cvx 
				//TODO - Cvx triggers are not yet enabled.
			}
			//Allergy
			for(int a=0;a<listAllergies.Count;a++) {
				if(ehrTrig.AllergyDefNumList.Contains(" "+listAllergies[a].AllergyDefNum+" ")) {
					AllergyDef allergyDefCur=AllergyDefs.GetOne(listAllergies[a].AllergyDefNum);
					if(allergyDefCur!=null) {
						listObjectMatches.Add(allergyDefCur);
						triggerMessage+="  -(Allergy) "+allergyDefCur.Description+"\r\n";
					}
					continue;
				}
			}
			//Demographics
			string[] arrayDemoItems=ehrTrig.DemographicsList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int j=0;j<arrayDemoItems.Length;j++) {
				switch(arrayDemoItems[j].Split(',')[0]) {
					case "age":
						int val=PIn.Int(Regex.Match(arrayDemoItems[j],@"\d+").Value);
						if(arrayDemoItems[j].Contains("=")) {//=, >=, or <=
							if(patCur.Age==val) {
								listObjectMatches.Add("Demographic");
								triggerMessage+="  -(Demographic) "+arrayDemoItems[j]+"\r\n";
								break;
							}
						}
						if(arrayDemoItems[j].Contains("<")) {//< or <=
							if(patCur.Age<val) {
								listObjectMatches.Add("Demographic");
								triggerMessage+="  -(Demographic) "+arrayDemoItems[j]+"\r\n";
								break;
							}
						}
						if(arrayDemoItems[j].Contains(">")) {//< or <=
							if(patCur.Age>val) {
								listObjectMatches.Add("Demographic");
								triggerMessage+="  -(Demographic) "+arrayDemoItems[j]+"\r\n";
								break;
							}
						}
						//should never happen, age element didn't contain a comparator
						break;
					case "gender":
						if(arrayDemoItems[j].Split(new char[] { ',' }).Contains(patCur.Gender.ToString().ToLower())) {
							listObjectMatches.Add("Demographic");
							triggerMessage+="  -(Demographic) "+arrayDemoItems[j]+"\r\n";
						}
						break;
					default:
						break;//should never happen
				}
			}
			//Lab Result
			for(int l=0;l<listEhrLabs.Count;l++) {
				for(int r=0;r<listEhrLabs[l].ListEhrLabResults.Count;r++) {
					if(ehrTrig.LabLoincList.Contains(" "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierID+" ")
						|| ehrTrig.LabLoincList.Contains(" "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierIDAlt+" ")) 
					{
						listObjectMatches.Add(listEhrLabs[l].ListEhrLabResults[r]);
						if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierID!="") {//should almost always be the case.
							Loinc loincCur=Loincs.GetByCode(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierID);
							triggerMessage+="  -(LOINC) "+loincCur.NameShort+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierIDAlt!="") {
							Loinc loincCur=Loincs.GetByCode(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierIDAlt);
							triggerMessage+="  -(LOINC) "+loincCur.NameShort+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierText!="") {
							triggerMessage+="  -(LOINC) "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierText+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierTextAlt!="") {
							triggerMessage+="  -(LOINC) "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierTextAlt+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierID!="") {
							triggerMessage+="  -(LOINC) "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierID+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierIDAlt!="") {
							triggerMessage+="  -(LOINC) "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierIDAlt+"\r\n";
						}
						else if(listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierTextOriginal!="") {
							triggerMessage+="  -(LOINC) "+listEhrLabs[l].ListEhrLabResults[r].ObservationIdentifierTextOriginal+"\r\n";
						}
						else {
							triggerMessage+="  -(LOINC) Unknown code.\r\n";//should never happen.
						}
						continue;
					}
				}
			}
			//Vital 
			if(triggerObject is Vitalsign) {
				Vitalsign vitalsign=(Vitalsign)triggerObject;
				//Vital signs are stored at ' height,>60 BMI,<27.0 BMI,>22% '
				string[] arrayVitalItems=ehrTrig.VitalLoincList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
				for(int v=0;v<arrayVitalItems.Length;v++) {
					double val=PIn.Double(Regex.Match(arrayVitalItems[v],@"\d+(.(\d+))*").Value);//decimal value w/ or w/o decimal.
					switch(arrayVitalItems[v].Split(',')[0]) {
						case "height":
							if(arrayVitalItems[v].Contains("=")) {//=, >=, or <=
								if(vitalsign.Height==val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains("<")) {//< or <=
								if(vitalsign.Height<val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains(">")) {//> or >=
								if(vitalsign.Height>val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							break;
						case "weight":
							if(arrayVitalItems[v].Contains("=")) {//=, >=, or <=
								if(vitalsign.Weight==val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains("<")) {//< or <=
								if(vitalsign.Weight<val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains(">")) {//> or >=
								if(vitalsign.Weight>val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							break;
						case "BMI":
							float BMI=Vitalsigns.CalcBMI(vitalsign.Weight,vitalsign.Height);
							if(arrayVitalItems[v].Contains("=")) {//=, >=, or <=
								if(BMI==val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains("<")) {//< or <=
								if(BMI<val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							if(arrayVitalItems[v].Contains(">")) {//> or >=
								if(BMI>val) {
									listObjectMatches.Add(vitalsign);
									triggerMessage+="  -(Vitalsign) "+arrayVitalItems[v]+"\r\n";
									break;
								}
							}
							break;
						case "BP":
							//TODO
							break;
					}//end switch
				}
			}
			if(ehrTrig.Cardinality==MatchCardinality.TwoOrMore && listObjectMatches.Count<2) {
				return;//next trigger, do not add to listInterventions
			}
			if(ehrTrig.Cardinality==MatchCardinality.OneOfEachCategory && !OneOfEachCategoryHelper(ehrTrig,listObjectMatches)) {
				return;
			}
			if(ehrTrig.Cardinality==MatchCardinality.All && !AllCategoryHelper(ehrTrig,listObjectMatches,patCur,triggerObject)) {
				return;
			}
			CDSIntervention cdsi=new CDSIntervention();
			cdsi.EhrTrigger=ehrTrig;
			cdsi.InterventionMessage=triggerMessage;
			cdsi.TriggerObjects=listObjectMatches;
			listInterventions.Add(cdsi);
			//These are all potential match sources for CDSI triggers. Some of these have been implemented, some have not. 2016_02_08
			//Allergy-----------------------------------------------------------------------------------------------------------------------
			//allergy.snomedreaction
			//allergy.AllergyDefNum>>AllergyDef.SnomedType
			//allergy.AllergyDefNum>>AllergyDef.SnomedAllergyTo
			//allergy.AllergyDefNum>>AllergyDef.MedicationNum>>Medication.RxCui
			//Disease-----------------------------------------------------------------------------------------------------------------------
			//Disease.DiseaseDefNum>>DiseaseDef.ICD9Code
			//Disease.DiseaseDefNum>>DiseaseDef.SnomedCode
			//Disease.DiseaseDefNum>>DiseaseDef.Icd10Code
			//LabPanels---------------------------------------------------------------------------------------------------------------------
			//LabPanel.LabPanelNum<<LabResult.TestId (Loinc)
			//LabPanel.LabPanelNum<<LabResult.ObsValue (Loinc)
			//LabPanel.LabPanelNum<<LabResult.ObsRange (Loinc)
			//MedicationPat-----------------------------------------------------------------------------------------------------------------
			//MedicationPat.RxCui
			//MedicationPat.MedicationNum>>Medication.RxCui
			//Patient>>Demographics---------------------------------------------------------------------------------------------------------
			//Patient.Gender
			//Patient.Birthdate (Loinc age?)
			//Patient.SmokingSnoMed
			//RxPat-------------------------------------------------------------------------------------------------------------------------
			//Do not check RxPat. It is useless.
			//VaccinePat--------------------------------------------------------------------------------------------------------------------
			//VaccinePat.VaccineDefNum>>VaccineDef.CVXCode
			//VitalSign---------------------------------------------------------------------------------------------------------------------
			//VitalSign.Height (Loinc)
			//VitalSign.Weight (Loinc)
			//VitalSign.BpSystolic (Loinc)
			//VitalSign.BpDiastolic (Loinc)
			//VitalSign.WeightCode (Snomed)
			//VitalSign.PregDiseaseNum (Snomed)
		}

		private static bool AllCategoryHelper(EhrTrigger ehrTrig,List<object> listObjectMatches,Patient patCur,object triggerObject) {
			//No remoting call; already checked before calling this method
			//Match all Icd9Codes---------------------------------------------------------------------------------------------------------------------------
			string[] arrayIcd9Codes=ehrTrig.ProblemIcd9List.Split(new string[] {" "},StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayIcd9Codes.Length;c++) {
				if(listObjectMatches.FindAll(x => x is DiseaseDef)
					.Exists(x => ((DiseaseDef)x).ICD9Code==arrayIcd9Codes[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all Icd10Codes--------------------------------------------------------------------------------------------------------------------------
			string[] arrayIcd10Codes=ehrTrig.ProblemIcd10List.Split(new string[] {" "},StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayIcd10Codes.Length;c++) {
				if(listObjectMatches.FindAll(x => x is DiseaseDef)
					.Exists(x => ((DiseaseDef)x).Icd10Code==arrayIcd10Codes[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all SnomedCodes-------------------------------------------------------------------------------------------------------------------------
			string[] arraySnomedCodes=ehrTrig.ProblemSnomedList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arraySnomedCodes.Length;c++) {
				if(listObjectMatches.FindAll(x => x is DiseaseDef)
					.Exists(x => ((DiseaseDef)x).SnomedCode==arraySnomedCodes[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all Medications-------------------------------------------------------------------------------------------------------------------------
			string[] arrayMedicationNums=ehrTrig.MedicationNumList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c = 0;c<arrayMedicationNums.Length;c++) {
				if(listObjectMatches.FindAll(x => x is MedicationPat)
					.Exists(x => ((MedicationPat)x).MedicationNum.ToString()==arrayMedicationNums[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all RxNorm------------------------------------------------------------------------------------------------------------------------------
			string[] arrayRxCuiCodes=ehrTrig.RxCuiList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayRxCuiCodes.Length;c++) {
					if(listObjectMatches.FindAll(x => x is MedicationPat)
						.Exists(x => ((MedicationPat)x).RxCui.ToString()==arrayRxCuiCodes[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all CvxCodes----------------------------------------------------------------------------------------------------------------------------
			string[] arrayCvxCodes=ehrTrig.CvxList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayCvxCodes.Length;c++) {
				//TODO - Cvx is not yet enabled.
			}
			//Match all Allergies---------------------------------------------------------------------------------------------------------------------------
			string[] arrayAllergyDefNums=ehrTrig.AllergyDefNumList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayAllergyDefNums.Length;c++) {
				if(listObjectMatches.FindAll(x => x is AllergyDef)
					.Exists(x => ((AllergyDef)x).AllergyDefNum.ToString()==arrayAllergyDefNums[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all Demographics------------------------------------------------------------------------------------------------------------------------
			//Demographics are stored as ' age,>=3  age,<6  gender,female '
			string[] arrayDemographicsList=ehrTrig.DemographicsList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			if(arrayDemographicsList.Length != listObjectMatches.FindAll(x => x.ToString()=="Demographic").Count) {
				return false;//All demographic conditions have not been met
			}			
			//Match all Lab Result LoincCodes---------------------------------------------------------------------------------------------------------------
			string[] arrayLoincCodes=ehrTrig.LabLoincList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
			for(int c=0;c<arrayLoincCodes.Length;c++) {
				if(listObjectMatches.FindAll(x => x is EhrLabResult)
					.Exists(x => ((EhrLabResult)x).ObservationIdentifierID==arrayLoincCodes[c])) 
				{
					continue;//found required code
				}
				//required code not found, return false and continue to next trigger
				return false;
			}
			//Match all Vitals------------------------------------------------------------------------------------------------------------------------------
			if(triggerObject is Vitalsign) {
				//Vital signs are stored as ' height,>60 BMI,<27.0 BMI,>22% '
				string[] arrayVitalItems=ehrTrig.VitalLoincList.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);
				if(arrayVitalItems.Length != listObjectMatches.Count(x => x is Vitalsign)) {
					return false;//All vital sign conditions have not been met
				}
			}
			return true;//All conditions have been met.
		}
		
		///<summary>Returns true if ListObjectMatches satisfies trigger conditions.</summary>
		private static bool OneOfEachCategoryHelper(EhrTrigger ehrTrigger,List<object> listObjectMatches) {
			//No remoting call; already checked before calling this method
			//problems
			if(ehrTrigger.ProblemDefNumList.Trim()!=""
				|| ehrTrigger.ProblemIcd9List.Trim()!=""
				|| ehrTrigger.ProblemIcd10List.Trim()!=""
				|| ehrTrigger.ProblemSnomedList.Trim()!="") 
			{
				//problem condition exists
				if(!listObjectMatches.Any(x=>x is DiseaseDef)) {
					return false;
				}
			}//end problem
			//medication
			if(ehrTrigger.MedicationNumList.Trim()!=""
				|| ehrTrigger.RxCuiList.Trim()!=""
				|| ehrTrigger.CvxList.Trim()!="") 
			{
				//Medication condition exists
				if(!listObjectMatches.Any(x=>x is MedicationPat || x is VaccineDef)){
					return false;
				}
			}//end medication
			//allergy
			if(ehrTrigger.AllergyDefNumList.Trim()!="") {
				//Allergy condition exists
				if(!listObjectMatches.Any(x=>x is AllergyDef)) {
					return false;
				}
			}//end allergy
			//lab
			if(ehrTrigger.LabLoincList.Trim()!="") {
				//Lab result condition exists
				if(!listObjectMatches.Any(x => x is EhrLabResult)) {
					return false;
				}
			}
			//demographics
			if(ehrTrigger.DemographicsList.Trim()!="") {
				//Demographic condition exists
				if(!listObjectMatches.Any(x => x.ToString()=="Demographic")) {
					return false;
				}
			}
			//vitalsign
			if(ehrTrigger.VitalLoincList.Trim()!="") {
				//Demographic condition exists
				if(!listObjectMatches.Any(x => x is Vitalsign)) {
					return false;
				}
			}
			return true;//One from each defined category was found.
		}		

		///<summary></summary>
		public static long Insert(EhrTrigger ehrTrigger) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				ehrTrigger.EhrTriggerNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ehrTrigger);
				return ehrTrigger.EhrTriggerNum;
			}
			return Crud.EhrTriggerCrud.Insert(ehrTrigger);
		}

		///<summary></summary>
		public static void Update(EhrTrigger ehrTrigger) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrTrigger);
				return;
			}
			Crud.EhrTriggerCrud.Update(ehrTrigger);
		}

		///<summary></summary>
		public static void Delete(long ehrTriggerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrTriggerNum);
				return;
			}
			string command= "DELETE FROM ehrtrigger WHERE EhrTriggerNum = "+POut.Long(ehrTriggerNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<AutoTrigger> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<AutoTrigger>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM autotrigger WHERE PatNum = "+POut.Long(patNum);
			return Crud.AutoTriggerCrud.SelectMany(command);
		}

		///<summary>Gets one AutoTrigger from the db.</summary>
		public static AutoTrigger GetOne(long automationTriggerNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<AutoTrigger>(MethodBase.GetCurrentMethod(),automationTriggerNum);
			}
			return Crud.AutoTriggerCrud.SelectOne(automationTriggerNum);
		}

		///<summary></summary>
		public static long Insert(AutoTrigger autoTrigger){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				autoTrigger.AutomationTriggerNum=Meth.GetLong(MethodBase.GetCurrentMethod(),autoTrigger);
				return autoTrigger.AutomationTriggerNum;
			}
			return Crud.AutoTriggerCrud.Insert(autoTrigger);
		}

		///<summary></summary>
		public static void Update(AutoTrigger autoTrigger){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autoTrigger);
				return;
			}
			Crud.AutoTriggerCrud.Update(autoTrigger);
		}
		*/



	}
}