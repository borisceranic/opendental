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
				butClose.Text=Lans.g(this,"Cancel");
				butOK.Visible=true;
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Type",100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Replacement",155);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[FName]");
			row.Cells.Add("The patient's first name.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[LName]");
			row.Cells.Add("The patient's last name.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[LNameLetter]");
			row.Cells.Add("The first letter of the patient's last name, capitalized.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[NameF]");
			row.Cells.Add("The patient's first name.  Same as FName.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[NameFL]");
			row.Cells.Add("The patient's first name, a space, then the patient's last name.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[PatNum]");
			row.Cells.Add("The patient's account number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[ChartNumber]");
			row.Cells.Add("The patient's chart number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[WirelessPhone]");
			row.Cells.Add("The patient's wireless phone number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[HmPhone]");
			row.Cells.Add("The patient's home phone number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[WkPhone]");
			row.Cells.Add("The patient's work phone number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[Birthdate]");
			row.Cells.Add("The patient's birthdate.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[SSN]");
			row.Cells.Add("The patient's social security number.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[Address]");
			row.Cells.Add("The patient's address.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[City]");
			row.Cells.Add("The patient's city.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[State]");
			row.Cells.Add("The patient's state.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[Zip]");
			row.Cells.Add("The patient's zip code.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Patient");
			row.Cells.Add("[ReferredFromProvNameFL]");
			row.Cells.Add("The first and last name of the provider that referred the patient.");
			if((_replaceTypes & MessageReplaceType.Patient)!=MessageReplaceType.Patient) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Family");
			row.Cells.Add("[FamilyList]");
			row.Cells.Add("List of the patient's family members, one per line.");
			if((_replaceTypes & MessageReplaceType.Family)!=MessageReplaceType.Family) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[ApptDate]");
			row.Cells.Add("The appointment date.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[ApptTime]");
			row.Cells.Add("The appointment time.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[ApptDayOfWeek]");
			row.Cells.Add("The day of the week the appointment falls on.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[ApptProcsList]");
			row.Cells.Add("The procedures attached to the appointment, one per line, including procedure date and layman's term.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[date]");
			row.Cells.Add("The appointment date.  Synonym of ApptDate.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Appointment");
			row.Cells.Add("[time]");
			row.Cells.Add("The appointment time.  Synonym of ApptTime.");
			if((_replaceTypes & MessageReplaceType.Appointment)!=MessageReplaceType.Appointment) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Recall");
			row.Cells.Add("[DueDate]");
			row.Cells.Add("Max selected recall date for the patient.");
			if((_replaceTypes & MessageReplaceType.Recall)!=MessageReplaceType.Recall) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Recall");
			row.Cells.Add("[URL]");
			row.Cells.Add("The link where a patient can go to schedule a recall from the web.");
			if((_replaceTypes & MessageReplaceType.Recall)!=MessageReplaceType.Recall) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("User");
			row.Cells.Add("[UserNameF]");
			row.Cells.Add("The first name of the person who is currently logged in.");
			if((_replaceTypes & MessageReplaceType.User)!=MessageReplaceType.User) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("User");
			row.Cells.Add("[UserNameL]");
			row.Cells.Add("The last name of the person who is currently logged in.");
			if((_replaceTypes & MessageReplaceType.User)!=MessageReplaceType.User) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("User");
			row.Cells.Add("[UserNameFL]");
			row.Cells.Add("The first name, a space, then the last name of the person who is currently logged in.");
			if((_replaceTypes & MessageReplaceType.User)!=MessageReplaceType.User) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Office");
			row.Cells.Add("[OfficePhone]");
			row.Cells.Add("The practice or clinic phone number in standard format.");
			if((_replaceTypes & MessageReplaceType.Office)!=MessageReplaceType.Office) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Office");
			row.Cells.Add("[OfficeFax]");
			row.Cells.Add("The practice or clinic fax number in standard format.");
			if((_replaceTypes & MessageReplaceType.Office)!=MessageReplaceType.Office) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			row=new ODGridRow();
			row.Cells.Add("Office");
			row.Cells.Add("[OfficeName]");
			row.Cells.Add("The practice or clinic name.");
			if((_replaceTypes & MessageReplaceType.Office)!=MessageReplaceType.Office) {
				row.ColorText=Color.Red;
			}
			gridMain.Rows.Add(row);
			gridMain.EndUpdate();
		}

		///<summary>Replaces all patient fields in the given message with the given patient's information.  Returns the resulting string.
		///Replaces: [FName], [LName], [LNameLetter], [NameF], [NameFL], [PatNum], 
		///[ChartNumber], [HmPhone], [WkPhone], [WirelessPhone], [ReferredFromProvNameFL].</summary>
		public static string ReplacePatient(string message,Patient pat) {
			string retVal=message;
			retVal=retVal.Replace("[FName]",pat.FName);
			retVal=retVal.Replace("[LName]",pat.LName);
			retVal=retVal.Replace("[LNameLetter]",pat.LName.Substring(0,1).ToUpper());
			retVal=retVal.Replace("[NameF]",pat.FName);
			retVal=retVal.Replace("[NameFL]",pat.FName+" "+pat.LName);
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
			Referral patRef=Referrals.GetReferralForPat(pat.PatNum);
			if(patRef!=null) {
				retVal=retVal.Replace("[ReferredFromProvNameFL]",patRef.FName+" "+patRef.LName);
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
			retVal=retVal.Replace("[UserNameFL]",userNameF+" "+userNameL);
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
	}

}