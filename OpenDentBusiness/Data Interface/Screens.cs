using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
  ///<summary></summary>
	public class Screens {

		///<summary>Gets one Screen from the db.</summary>
		public static Screen GetOne(long screenNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Screen>(MethodBase.GetCurrentMethod(),screenNum);
			}
			return Crud.ScreenCrud.SelectOne(screenNum);
		}

		///<summary>After taking multiple screenings using a sheets, this method will import all sheets as screens and insert them into the db.
		///The goal of this method is that the office will fill out a bunch of sheets (in the web?).
		///Then after they get back to their office (with connection to their db) they will push a button to upload / insert a batch.</summary>
		public static List<Screen> CreateScreensFromSheets(List<Sheet> listSheets) {
			//No need to check RemotingRole; no call to db.
			List<Screen> listScreens=new List<Screen>();
			foreach(Sheet sheet in listSheets) {
				listScreens.Add(CreateScreenFromSheet(sheet));
			}
			return listScreens;
		}

		///<summary>After taking a screening using a sheet, this method will import the sheet as a screen and insert it into the db.
		///Returns null if the sheet passed in is not a Screening sheet type or if the sheet is missing the required ScreenGroupNum param.
		///Optionally supply a screen if you want to preset some values.  E.g. ScreenGroupOrder is often preset before calling this method.</summary>
		public static Screen CreateScreenFromSheet(Sheet sheet,Screen screen=null) {
			//No need to check RemotingRole; no call to db.
			//Make sure that the sheet passed in is a screening and contains the required ScreenGroupNum parameter.
			if(sheet.SheetType!=SheetTypeEnum.Screening || SheetParameter.GetParamByName(sheet.Parameters,"ScreenGroupNum")==null) {
				return null;
			}
			if(screen==null) {
				screen=new Screen();
				screen.ScreenGroupNum=(long)SheetParameter.GetParamByName(sheet.Parameters,"ScreenGroupNum").ParamValue;
			}
			screen.SheetNum=sheet.SheetNum;
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "Gender":
						if(field.FieldValue.Trim().ToLower().StartsWith("m")) {
							screen.Gender=PatientGender.Male;
						}
						else if(field.FieldValue.Trim().ToLower().StartsWith("f")) {
							screen.Gender=PatientGender.Female;
						}
						else {
							screen.Gender=PatientGender.Unknown;
						}
						break;
					case "Race/Ethnicity":
						PatientRaceOld patientRace=PatientRaceOld.Unknown;
						Enum.TryParse<PatientRaceOld>(field.FieldValue,out patientRace);
						screen.RaceOld=patientRace;
						break;
					case "GradeLevel":
						PatientGrade patientGrade=PatientGrade.Unknown;
						Enum.TryParse<PatientGrade>(field.FieldValue,out patientGrade);
						screen.GradeLevel=patientGrade;
						break;
					case "Age":
						if(screen.Age!=0) {
							break;//Already calculated via Birthdate.
						}
						byte age=0;
						byte.TryParse(field.FieldValue,out age);
						screen.Age=age;
						break;
					case "Urgency":
						TreatmentUrgency treatmentUrgency=TreatmentUrgency.Unknown;
						Enum.TryParse<TreatmentUrgency>(field.FieldValue,out treatmentUrgency);
						screen.Urgency=treatmentUrgency;
						break;
					case "HasCaries":
						screen.HasCaries=GetYN(field.FieldValue);
						break;
					case "NeedsSealants":
						screen.NeedsSealants=GetYN(field.FieldValue);
						break;
					case "CariesExperience":
						screen.CariesExperience=GetYN(field.FieldValue);
						break;
					case "EarlyChildCaries":
						screen.EarlyChildCaries=GetYN(field.FieldValue);
						break;
					case "ExistingSealants":
						screen.ExistingSealants=GetYN(field.FieldValue);
						break;
					case "MissingAllTeeth":
						screen.MissingAllTeeth=GetYN(field.FieldValue);
						break;
					case "Birthdate":
						DateTime birthdate=new DateTime(1,1,1);
						DateTime.TryParse(field.FieldValue,out birthdate);
						screen.Birthdate=birthdate;
						//Check to see if the sheet has Age manually filled out.  
						//If Age was not manually set, automatically calculate the age based on the birthdate entered.
						//This matches screening functionality.
						SheetField sheetFieldAge=sheet.SheetFields.FirstOrDefault(x => x.FieldName=="Age");
						if(sheetFieldAge!=null && string.IsNullOrEmpty(sheetFieldAge.FieldValue)) {
							screen.Age=PIn.Byte(Patients.DateToAge(birthdate).ToString());
						}
						break;
					case "Comments":
						screen.Comments=field.FieldValue;
						break;
					case "ChartSealantTreatment":
						if(sheet.PatNum!=0) {
							//Digest any TP'd sealants here
							ProcessScreenChart(sheet,new List<string>() { "ChartSealantTreatment" });
						}
						break;
					case "ChartSealantComplete"://Do nothing.
						break;
				}
			}
			if(screen.ScreenNum==0) {
				Insert(screen);
			}
			else {
				Update(screen);
			}
			return screen;
		}

		///<summary>Takes a screening sheet that is associated to a patient and processes any corresponding ScreenChart found.
		///Processing will create treatment planned or completed procedures for the patient.
		///Supply the sheet and then a list of the toothcharts to digest.  This way it can be done one at a time or all at once as desired.</summary>
		public static void ProcessScreenChart(Sheet sheet,List<string> listScreenChartNames) {
			//No need to check RemotingRole; no call to db.
			if(sheet==null || sheet.PatNum==0) {
				return;//An invalid screening sheet was passed in.
			}
			foreach(string chartName in listScreenChartNames) {//For each chart specified for digestion
				string[] toothValues=null;
				foreach(SheetField field in sheet.SheetFields) {//Go through the supplied sheet's fields and find that field.
					if(field.FieldName!=chartName) {
						continue;
					}
					toothValues=field.FieldValue.Split(';');//When found, start digesting it.
					break;				
				}
				if(toothValues==null) {
					continue;
				}
				for(int i=0;i<toothValues.Length;i++) {//toothValues is in the order from low to high tooth number in the chart
					if(!toothValues[i].Contains("S")) {
						continue;
					}
					string surf="";
					#region Parse ScreenChart FieldValues
					int toothNum=0;
					bool isMolar=false;
					bool isRight=false;
					bool isLing=false;
					if(i<=1) {//Top left quadrant of toothchart
						toothNum=i+2;
						isMolar=true;
						isRight=true;
						isLing=true;
					}
					else if(i>1 && i<=3) {//Top middle-left quadrant of toothchart
						toothNum=i+2;
						isMolar=false;
						isRight=true;
						isLing=true;
					}
					else if(i>3 && i<=5) {//Top middle-right quadrant of toothchart
						toothNum=i+8;
						isMolar=false;
						isRight=false;
						isLing=true;
					}
					else if(i>5 && i<=7) {//Top right quadrant of toothchart
						toothNum=i+8;
						isMolar=true;
						isRight=false;
						isLing=true;
					}
					else if(i>7 && i<=9) {//Lower right quadrant of toothchart
						toothNum=i+10;
						isMolar=true;
						isRight=false;
						isLing=false;
					}
					else if(i>9 && i<=11) {//Lower middle-right quadrant of toothchart
						toothNum=i+10;
						isMolar=false;
						isRight=false;
						isLing=false;
					}
					else if(i>11 && i<=13) {//Lower middle-left quadrant of toothchart
						toothNum=i+16;
						isMolar=false;
						isRight=true;
						isLing=false;
					}
					else if(i>13) {//Lower left quadrant of toothchart
						toothNum=i+16;
						isMolar=true;
						isRight=true;
						isLing=false;
					}
					string[] surfaces=toothValues[i].Split(',');
					if(isMolar) {
						if(isRight) {
							if(surfaces[0]=="S") {
								surf+="D";
							}
							if(surfaces[1]=="S") {
								surf+="M";
							}
						}
						else {//Is Left side
							if(surfaces[0]=="S") {
								surf+="M";
							}
							if(surfaces[1]=="S") {
								surf+="D";
							}
						}
						if(isLing && surfaces[2]=="S") {
							surf+="L";
						}
						if(!isLing && surfaces[2]=="S") {
							surf+="B";
						}
					}
					else {//Front teeth, only look at 3rd surface position in control as that's the only one the user can see.
						if(surfaces[2]=="S") {
							surf="O";//NOTE: Not sure what surface to enter here... This is just a placeholder for now until we figure it out...
						}
					}
					#endregion Parse Toothchart FieldValues
					surf=Tooth.SurfTidyForDisplay(surf,toothNum.ToString());
					if(chartName=="ChartSealantTreatment") {//Create TP'd sealant procs if they don't already exist for this patient.
						if(Procedures.GetProcForPatByToothSurfStat(sheet.PatNum,toothNum,surf,ProcStat.TP)!=null) {
							continue;
						}
						Procedures.CreateProcForPat(sheet.PatNum,ProcedureCodes.GetCodeNum("D1351"),surf,toothNum,ProcStat.TP);
					}
					else {//Sealant chart, create complete sealants.
						Procedure proc=Procedures.GetProcForPatByToothSurfStat(sheet.PatNum,toothNum,surf,ProcStat.TP);
						if(proc==null) {//A TP procedure does not already exist.
							Procedures.CreateProcForPat(sheet.PatNum,ProcedureCodes.GetCodeNum("D1351"),surf,toothNum,ProcStat.C);
						}
						else {//TP proc already exists, set it complete.
							Procedure procOld=proc.Copy();
							proc.ProcStatus=ProcStat.C;
							proc.DateEntryC=DateTime.Now;
							Procedures.Update(proc,procOld);
						}
						Recalls.Synch(sheet.PatNum);
					}
				}
			}
		}

		///<summary>Helper method to quickly get the YN value from a string.  Returns Yes if str starts with y, No if n, Unknown by default.</summary>
		private static YN GetYN(string fieldValue) {
			//No need to check RemotingRole; no call to db.
			if(fieldValue.Trim().ToLower().StartsWith("y")) {
				return YN.Yes;
			}
			else if(fieldValue.Trim().ToLower().StartsWith("n")) {
				return YN.No;
			}
			return YN.Unknown;
		}

		///<summary>Gets all screens associated to the screen group passed in.</summary>
		public static List<Screen> GetScreensForGroup(long screenGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Screen>>(MethodBase.GetCurrentMethod(),screenGroupNum);
			}
			string command="SELECT * FROM screen "
				+"WHERE ScreenGroupNum = '"+POut.Long(screenGroupNum)+"' "
				+"ORDER BY ScreenGroupOrder";
			return Crud.ScreenCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(OpenDentBusiness.Screen Cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.ScreenNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.ScreenNum;
			}
			return Crud.ScreenCrud.Insert(Cur);
		}

		///<summary></summary>
		public static void Update(OpenDentBusiness.Screen Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			Crud.ScreenCrud.Update(Cur);
		}

		///<summary></summary>
		public static void Delete(OpenDentBusiness.Screen Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "DELETE from screen WHERE ScreenNum = '"+POut.Long(Cur.ScreenNum)+"'";
			Db.NonQ(command);
		}


	}

	

}













