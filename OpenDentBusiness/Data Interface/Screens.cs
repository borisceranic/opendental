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
		public static List<Screen> ImportScreensFromSheets(List<Sheet> listSheets) {
			//No need to check RemotingRole; no call to db.
			List<Screen> listScreens=new List<Screen>();
			foreach(Sheet sheet in listSheets) {
				listScreens.Add(ImportScreenFromSheet(sheet));
			}
			return listScreens;
		}

		///<summary>After taking a screening using a sheet, this method will import the sheet as a screen and insert it into the db.
		///Returns null if the sheet passed in is not a Screening sheet type or if the sheet is missing the required ScreenGroupNum param.
		///Optionally supply a screen if you want to preset some values.  E.g. ScreenGroupOrder is often preset before calling this method.</summary>
		public static Screen ImportScreenFromSheet(Sheet sheet,Screen screen=null) {
			//No need to check RemotingRole; no call to db.
			//Make sure that the sheet passed in is a screening and contains the required ScreenGroupNum parameter.
			if(sheet.SheetType!=SheetTypeEnum.Screening || SheetParameter.GetParamByName(sheet.Parameters,"ScreenGroupNum")==null) {
				return null;
			}
			if(screen==null) {
				screen=new Screen();
			}
			screen.ScreenGroupNum=(long)SheetParameter.GetParamByName(sheet.Parameters,"ScreenGroupNum").ParamValue;
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
				}
			}
			Insert(screen);
			return screen;
		}

		///<summary>Helper method to quickly get the YN value from a string.  Returns Yes if str starts with y, No if n, Unknown by default.</summary>
		private static YN GetYN(string fieldValue) {
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













