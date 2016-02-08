using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	///<summary>If using selection mode, call FormMessageReplacements modally.  If not in selection mode, non-modal instances are fine.</summary>
	public partial class FormMessageReplacements:Form {

		private MessageReplaceType _replaceTypes;
		public bool IsSelectionMode;

		///<summary>Returns empty string if there is no Replacement String selected in the grid.</summary>
		public string Replacement {
			get {
				if(gridMain==null || gridMain.IsDisposed || gridMain.GetSelectedIndex()==-1) {
					return "";
				}
				return gridMain.Rows[gridMain.GetSelectedIndex()].Cells[1].Text;
			}
		}

		public FormMessageReplacements(MessageReplaceType replaceTypes) {
			InitializeComponent();
			Lan.F(this);
			_replaceTypes=replaceTypes;
		}

		private void FormMessageReplacements_Load(object sender,EventArgs e) {
			if(IsSelectionMode) {
				butClose.Text=Lan.g(this,"Cancel");
				butOK.Visible=true;
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Replacement"),155);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();

			#region Patient Replacement Rows
			AddReplacementRow("[FName]","The patient's first name.",MessageReplaceType.Patient);
			AddReplacementRow("[LName]","The patient's last name.",MessageReplaceType.Patient);
			AddReplacementRow("[LNameLetter]","The first letter of the patient's last name, capitalized.",MessageReplaceType.Patient);
			AddReplacementRow("[NameF]","The patient's first name.  Same as FName.",MessageReplaceType.Patient);
			AddReplacementRow("[NameFL]","The patient's first name, a space, then the patient's last name.",MessageReplaceType.Patient);
			AddReplacementRow("[PatNum]","The patient's account number.",MessageReplaceType.Patient);
			AddReplacementRow("[ChartNumber]","The patient's chart number.",MessageReplaceType.Patient);
			AddReplacementRow("[WirelessPhone]","The patient's wireless phone number.",MessageReplaceType.Patient);
			AddReplacementRow("[HmPhone]","The patient's home phone number.",MessageReplaceType.Patient);
			AddReplacementRow("[WkPhone]","The patient's work phone number.",MessageReplaceType.Patient);
			AddReplacementRow("[Birthdate]","The patient's birthdate.",MessageReplaceType.Patient);
			AddReplacementRow("[SSN]","The patient's social security number.",MessageReplaceType.Patient);
			AddReplacementRow("[Address]","The patient's address.",MessageReplaceType.Patient);
			AddReplacementRow("[City]","The patient's city.",MessageReplaceType.Patient);
			AddReplacementRow("[State]","The patient's state.",MessageReplaceType.Patient);
			AddReplacementRow("[Zip]","The patient's zip code.",MessageReplaceType.Patient);
			AddReplacementRow("[ReferredFromProvNameFL]","The first and last name of the provider that referred the patient.",MessageReplaceType.Patient);
			AddReplacementRow("[MonthlyCardsOnFile]","Masked list of the patient's monthly credit cards on file.",MessageReplaceType.Patient);
			#endregion
			#region Family Replacement Rows
			//family replacement rows
			AddReplacementRow("[FamilyList]","List of the patient's family members, one per line.",MessageReplaceType.Family);
			#endregion
			#region Appointment Replacement Rows
			//appointment replacement rows
			AddReplacementRow("[ApptDate]","The appointment date.",MessageReplaceType.Appointment);
			AddReplacementRow("[ApptTime]","The appointment time.",MessageReplaceType.Appointment);
			AddReplacementRow("[ApptDayOfWeek]","The day of the week the appointment falls on.",MessageReplaceType.Appointment);
			AddReplacementRow("[ApptProcsList]","The procedures attached to the appointment, one per line, including procedure date and layman's term.",
				MessageReplaceType.Appointment);
			AddReplacementRow("[date]","The appointment date.  Synonym of ApptDate.",MessageReplaceType.Appointment);
			AddReplacementRow("[time]","The appointment time.  Synonym of ApptTime.",MessageReplaceType.Appointment);
			#endregion
			#region Recall Replacement Rows
			//recall replacement rows
			AddReplacementRow("[DueDate]","Max selected recall date for the patient.",MessageReplaceType.Recall);
			AddReplacementRow("[URL]","The link where a patient can go to schedule a recall from the web.",MessageReplaceType.Recall);
			#endregion
			#region User Replacement Rows
			//user replacement rows
			AddReplacementRow("[UserNameF]","The first name of the person who is currently logged in.",MessageReplaceType.User);
			AddReplacementRow("[UserNameL]","The last name of the person who is currently logged in.",MessageReplaceType.User);
			AddReplacementRow("[UserNameFL]","The first name, a space, then the last name of the person who is currently logged in.",
				MessageReplaceType.User);
			#endregion
			#region Office Replacement Rows
			//office replacement rows
			AddReplacementRow("[OfficePhone]","The practice or clinic phone number in standard format.",MessageReplaceType.Office);
			AddReplacementRow("[OfficeFax]","The practice or clinic fax number in standard format.",MessageReplaceType.Office);
			AddReplacementRow("[OfficeName]","The practice or clinic fax number in standard format.",MessageReplaceType.Office);
			#endregion
			#region Miscellaneous Replacement Rows
			//misc replacement rows
			AddReplacementRow("[CurrentMonth]","The text description of the current month (ex December).",MessageReplaceType.Misc);
			#endregion
			gridMain.EndUpdate();
		}

		///<summary>Builds and inserts a replacement row into the grid using the passed in field name, description, and replacement type.</summary>
		private void AddReplacementRow(String fieldName,String descript,MessageReplaceType replacementTypeCur) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(Lan.g("enumMessageReplaceType",replacementTypeCur.ToString()));
			row.Cells.Add(fieldName);
			row.Cells.Add(Lan.g(this,descript));
			if((_replaceTypes & replacementTypeCur)!=replacementTypeCur) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
		}

		///<summary>Replaces all patient fields in the given message with the given patient's information.  Returns the resulting string.
		///Replaces: [FName], [LName], [LNameLetter], [NameF], [NameFL], [PatNum], 
		///[ChartNumber], [HmPhone], [WkPhone], [WirelessPhone], [ReferredFromProvNameFL], etc.</summary>
		public static string ReplacePatient(string message,Patient pat) {
			if(pat==null) {
				return message;
			}
			string retVal=message;
			retVal=retVal.Replace("[FName]",pat.FName);
			retVal=retVal.Replace("[LName]",pat.LName);
			retVal=retVal.Replace("[LNameLetter]",pat.LName.Substring(0,1).ToUpper());
			retVal=retVal.Replace("[NameF]",pat.FName);
			retVal=retVal.Replace("[NameFL]",Patients.GetNameFL(pat.LName,pat.FName,"",""));
			retVal=retVal.Replace("[PatNum]",pat.PatNum.ToString());
			retVal=retVal.Replace("[ChartNumber]",pat.ChartNumber);
			retVal=retVal.Replace("[HmPhone]",pat.HmPhone);
			retVal=retVal.Replace("[WkPhone]",pat.WkPhone);
			retVal=retVal.Replace("[WirelessPhone]",pat.WirelessPhone);
			retVal=retVal.Replace("[Birthdate]",pat.Birthdate.ToShortDateString());
			retVal=retVal.Replace("[SSN]",pat.SSN);
			retVal=retVal.Replace("[Address]",pat.Address);
			retVal=retVal.Replace("[City]",pat.City);
			retVal=retVal.Replace("[State]",pat.State);
			retVal=retVal.Replace("[Zip]",pat.Zip);
			retVal=retVal.Replace("[MonthlyCardsOnFile]",CreditCards.GetMonthlyCardsOnFile(pat.PatNum));
			Referral patRef=Referrals.GetReferralForPat(pat.PatNum);
			if(patRef!=null) {
				retVal=retVal.Replace("[ReferredFromProvNameFL]",Patients.GetNameFL(patRef.LName,patRef.FName,"",""));
			}
			else {
				retVal=retVal.Replace("[ReferredFromProvNameFL]","");
			}
			return retVal;
		}

		///<summary>Replaces all family fields in the given message with the given family's information.  Returns the resulting string.
		///Will Replace: [FamilyList], currently does nothing. </summary>
		public static string ReplaceFamily(string message,Family fam) {
			string retVal=message;
			//TODO: mimic pattern in Recalls.GetAddrTable
			return retVal;
		}

		///<summary>Replaces all appointment fields in the given message with the given appointment's information.  Returns the resulting string.
		///If apt is null, replaces fields with blanks.
		///Replaces: [ApptDate], [ApptTime], [ApptDayOfWeek], [ApptProcList], [date], [time]. </summary>
		public static string ReplaceAppointment(string message,Appointment apt) {
			string retVal=message;
			if(apt==null) {
				retVal=retVal.Replace("[ApptDate]","");
				retVal=retVal.Replace("[date]","");
				retVal=retVal.Replace("[ApptTime]","");
				retVal=retVal.Replace("[time]","");
				retVal=retVal.Replace("[ApptDayOfWeek]","");
				retVal=retVal.Replace("[ApptProcsList]","");
				return retVal;
			}
			retVal=retVal.Replace("[ApptDate]",apt.AptDateTime.ToShortDateString());
			retVal=retVal.Replace("[date]",apt.AptDateTime.ToShortDateString());
			retVal=retVal.Replace("[ApptTime]",apt.AptDateTime.ToShortTimeString());
			retVal=retVal.Replace("[time]",apt.AptDateTime.ToShortTimeString());
			retVal=retVal.Replace("[ApptDayOfWeek]",apt.AptDateTime.DayOfWeek.ToString());
			if(retVal.Contains("[ApptProcsList]")) {
				bool isPlanned=false;
				if(apt.AptStatus==ApptStatus.Planned) {
					isPlanned=true;
				}
				List<Procedure> listProcs=Procedures.GetProcsForSingle(apt.AptNum,isPlanned);
				List<ProcedureCode> listProcCodes=new List<ProcedureCode>();
				ProcedureCode procCode=new ProcedureCode();
				StringBuilder strProcs=new StringBuilder();
				string procDescript="";
				List<ProcedureCode> listAllProcedureCodes=ProcedureCodeC.GetListLong();
				for(int i=0;i<listProcs.Count;i++) {
					procCode=ProcedureCodes.GetProcCode(listAllProcedureCodes,listProcs[i].CodeNum);
					if(procCode.LaymanTerm=="") {
						procDescript=procCode.Descript;
					}
					else {
						procDescript=procCode.LaymanTerm;
					}
					if(i>0) {
						strProcs.Append("\n");
					}
					strProcs.Append(listProcs[i].ProcDate.ToShortDateString()+" "+procCode.ProcCode+" "+procDescript);
				}
				retVal=retVal.Replace("[ApptProcsList]",strProcs.ToString());
			}
			return retVal;
		}

		///<summary>Replaces all recall fields in the given message with the given recall list's information.  Returns the resulting string.
		///Will replace: [DueDate], [URL], currently does nothing.</summary>
		public static string ReplaceRecall(string message,List<Recall> listRecallsForPat) {
			string retVal=message;
			//TODO: these replacements are a lot of work. 
			//When we decide to implement, mimic the pattern regarding the other areas of the program where these replacements are already used.
			return retVal;
		}

		///<summary>Replaces all user fields in the given message with the supplied userod's information.  Returns the resulting string.
		///Only works if the current user has a linked provider or employee, otherwise the replacements will be blank.
		///Replaces: [UserNameF], [UserNameL], [UserNameFL]. </summary>
		public static string ReplaceUser(string message,Userod userod) {
			string retVal=message;
			string userNameF="";
			string userNameL="";
			if(userod.ProvNum!=0) {
				Provider prov=Providers.GetProv(userod.ProvNum);
				userNameF=prov.FName;
				userNameL=prov.LName;
			}
			else if(userod.EmployeeNum!=0) {
				Employee emp=Employees.GetEmp(userod.EmployeeNum);
				userNameF=emp.FName;
				userNameL=emp.LName;
			}
			retVal=retVal.Replace("[UserNameF]",userNameF);
			retVal=retVal.Replace("[UserNameL]",userNameL);
			retVal=retVal.Replace("[UserNameFL]",Patients.GetNameFL(userNameL,userNameF,"",""));
			return retVal;
		}

		///<summary>Replaces all clinic fields in the given message with the supplied clinic's information.  Returns the resulting string.
		///Will use clinic information when available, otherwise defaults to practice info.
		///Replaces: [OfficePhone], [OfficeFax], [OfficeName]. </summary>
		public static string ReplaceOffice(string message,Clinic clinic) {
			string retVal=message;
			string officePhone=PrefC.GetString(PrefName.PracticePhone);
			string officeFax=PrefC.GetString(PrefName.PracticeFax);
			string officeName=PrefC.GetString(PrefName.PracticeTitle);
			if(clinic!=null && !String.IsNullOrEmpty(clinic.Phone)) {
				officePhone=clinic.Phone;
			}
			if(clinic!=null && !String.IsNullOrEmpty(clinic.Fax)) {
				officeFax=clinic.Fax;
			}
			if(clinic!=null && !String.IsNullOrEmpty(clinic.Description)) {
				officeName=clinic.Description;
			}
			if(CultureInfo.CurrentCulture.Name=="en-US" && officePhone.Length==10) {
				officePhone="("+officePhone.Substring(0,3)+")"+officePhone.Substring(3,3)+"-"+officePhone.Substring(6);
			}
			if(CultureInfo.CurrentCulture.Name=="en-US" && officeFax.Length==10) {
				officeFax="("+officeFax.Substring(0,3)+")"+officeFax.Substring(3,3)+"-"+officeFax.Substring(6);
			}
			retVal=retVal.Replace("[OfficePhone]",officePhone);
			retVal=retVal.Replace("[OfficeFax]",officeFax);
			retVal=retVal.Replace("[OfficeName]",officeName);
			return retVal;
		}

		///<summary>Replaces all miscellaneous fields in the given message.  Returns the resulting string.
		///Replaces: [CurrentMonth]</summary>
		public static string ReplaceMisc(string message) {
			string retVal=message;
			retVal=retVal.Replace("[CurrentMonth]",DateTimeOD.Today.ToString("MMMM"));
			return retVal;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();//Because we want the option to open this window non-modal.
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PressOK(e.Row);
		}

		///<summary>Only visible if IsSelectionMode is true.</summary>
		private void butOK_Click(object sender,EventArgs e) {
			PressOK(gridMain.GetSelectedIndex());
		}

		private void PressOK(int index) {
			if(index<0) {
				MsgBox.Show(this,"Please select a field.");
				return;
			}
			if(gridMain.Rows[index].ColorText==Color.Red) {
				MsgBox.Show(this,"The selected field is not supported.");
				return;
			}
			DialogResult=DialogResult.OK;
			Close();//Because we want the option to open this window non-modal.
		}

	}

	///<summary>Flags to specify which replacements are supported from the calling code.</summary>
	public enum MessageReplaceType {
		Patient=1,
		Family=2,
		Appointment=4,
		Recall=8,
		User=16,
		Office=32,
		Misc=64,
	}

}