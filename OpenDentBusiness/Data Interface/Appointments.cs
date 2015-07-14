using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	public class Appointments {
		
		///<summary>Gets a list of appointments for a period of time in the schedule, whether hidden or not.</summary>
		public static Appointment[] GetForPeriod(DateTime startDate,DateTime endDate){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Appointment[]>(MethodBase.GetCurrentMethod(),startDate,endDate);
			}
			//DateSelected = thisDay;
			string command=
				"SELECT * from appointment "
				+"WHERE AptDateTime BETWEEN "+POut.Date(startDate)+" AND "+POut.Date(endDate.AddDays(1))+" "
				+"AND aptstatus != '"+(int)ApptStatus.UnschedList+"' "
				+"AND aptstatus != '"+(int)ApptStatus.Planned+"'";
			return Crud.AppointmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets a List&lt;Appointment&gt; of appointments for a period of time in the schedule, whether hidden or not.</summary>
		public static List<Appointment> GetForPeriodList(DateTime startDate,DateTime endDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),startDate,endDate);
			}
			//DateSelected = thisDay;
			string command=
				"SELECT * from appointment "
				+"WHERE AptDateTime BETWEEN "+POut.Date(startDate)+" AND "+POut.Date(endDate.AddDays(1))+" "
				+"AND aptstatus != '"+(int)ApptStatus.UnschedList+"' "
				+"AND aptstatus != '"+(int)ApptStatus.Planned+"'";
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Gets list of unscheduled appointments.  Allowed orderby: status, alph, date</summary>
		public static Appointment[] RefreshUnsched(string orderby,long provNum,long siteNum,bool includeBrokenAppts,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Appointment[]>(MethodBase.GetCurrentMethod(),orderby,provNum,siteNum,includeBrokenAppts,clinicNum);
			}
			string command="SELECT * FROM appointment "
				+"LEFT JOIN patient ON patient.PatNum=appointment.PatNum "
				+"WHERE ";
			if(includeBrokenAppts) {
				command+="(AptStatus = "+POut.Long((int)ApptStatus.UnschedList)+" OR AptStatus = "+POut.Long((int)ApptStatus.Broken)+") ";
			}
			else {
				command+="AptStatus = "+POut.Long((int)ApptStatus.UnschedList)+" ";
			}
			if(provNum>0) {
				command+="AND (appointment.ProvNum="+POut.Long(provNum)+" OR appointment.ProvHyg="+POut.Long(provNum)+") ";
			}
			if(siteNum>0) {
				command+="AND patient.SiteNum="+POut.Long(siteNum)+" ";
			}
			if(clinicNum>0) {
				command+="AND appointment.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			command+="HAVING patient.PatStatus= "+POut.Long((int)PatientStatus.Patient)+" "
				+" OR patient.PatStatus= "+POut.Long((int)PatientStatus.Prospective)+" ";	
			if(orderby=="status") {
				command+="ORDER BY UnschedStatus,AptDateTime";
			}
			else if(orderby=="alph") {
				command+="ORDER BY LName,FName";
			}
			else { //if(orderby=="date"){
				command+="ORDER BY AptDateTime";
			}
			return Crud.AppointmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets list of asap appointments.</summary>
		public static List<Appointment> RefreshASAP(long provNum,long siteNum,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),provNum,siteNum,clinicNum);
			}
			string command="SELECT * FROM appointment ";
			//if(orderby=="alph" || siteNum>0) {
			//command+="LEFT JOIN patient ON patient.PatNum=appointment.PatNum ";
			//}
			if(siteNum>0) {
				command+="LEFT JOIN patient ON patient.PatNum=appointment.PatNum ";
			}
			command+="WHERE AptStatus = "+POut.Long((int)ApptStatus.ASAP)+" ";
			if(provNum>0) {
				command+="AND (appointment.ProvNum="+POut.Long(provNum)+" OR appointment.ProvHyg="+POut.Long(provNum)+") ";
			}
			if(siteNum>0) {
				command+="AND patient.SiteNum="+POut.Long(siteNum)+" ";
			}
			if(clinicNum>0) {
				command+="AND appointment.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			/*if(orderby=="status") {
				command+="ORDER BY UnschedStatus,AptDateTime";
			}
			else if(orderby=="alph") {
				command+="ORDER BY LName,FName";
			}
			else { //if(orderby=="date"){
				command+="ORDER BY AptDateTime";
			}*/
			command+="ORDER BY AptDateTime";
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Allowed orderby: status, alph, date</summary>
		public static List<Appointment> RefreshPlannedTracker(string orderby,long provNum,long siteNum,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),orderby,provNum,siteNum,clinicNum);
			}
			//We create a in-memory temporary table by joining the appointment and patient
			//tables to get a list of planned appointments for active paients, then we
			//perform a left join on that temporary table against the appointment table
			//to exclude any appointments in the temporary table which are already refereced
			//by the NextAptNum column by any other appointment within the appointment table.
			//Using an in-memory temporary table reduces the number of row comparisons performed for
			//this query overall as compared to left joining the appointment table onto itself,
			//because the in-memory temporary table has many fewer rows than the appointment table
			//on average.
			string command="SELECT tplanned.*,tregular.AptNum "
				+"FROM (SELECT a.* FROM appointment a,patient p "
					+"WHERE a.AptStatus="+POut.Long((int)ApptStatus.Planned)
					+" AND p.PatStatus="+POut.Long((int)PatientStatus.Patient)+" AND a.PatNum=p.PatNum ";
			if(provNum>0) {
				command+="AND (a.ProvNum="+POut.Long(provNum)+" OR a.ProvHyg="+POut.Long(provNum)+") ";
			}
			if(siteNum>0) {
				command+="AND p.SiteNum="+POut.Long(siteNum)+" ";
			}
			if(clinicNum>0) {
				command+="AND a.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			if(orderby=="status") {
				command+="ORDER BY a.UnschedStatus,a.AptDateTime";
			} 
			else if(orderby=="alph") {
				command+="ORDER BY p.LName,p.FName";
			} 
			else { //if(orderby=="date"){
				command+="ORDER BY a.AptDateTime";
			}
			command+=") tplanned "
				+"LEFT JOIN appointment tregular ON tplanned.AptNum=tregular.NextAptNum "
				+"WHERE tregular.AptNum IS NULL";
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Returns all appointments for the given patient, ordered from earliest to latest.  Used in statements, appt cards, OtherAppts window, etc.</summary>
		public static Appointment[] GetForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Appointment[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM appointment "
				+"WHERE PatNum = '"+POut.Long(patNum)+"' "
				+"AND NOT (AptDateTime < "+POut.Date(new DateTime(1880,1,1))+" AND AptStatus="+POut.Int((int)ApptStatus.UnschedList)+") "//AND NOT (on the pinboard)
				+"ORDER BY AptDateTime";
			return Crud.AppointmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets all appointments for a single patient ordered by AptDateTime.</summary>
		public static List<Appointment> GetListForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM appointment "
				+"WHERE patnum = '"+patNum.ToString()+"' "
				+"ORDER BY AptDateTime";
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Gets one appointment from db.  Returns null if not found.</summary>
		public static Appointment GetOneApt(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Appointment>(MethodBase.GetCurrentMethod(),aptNum);
			}
			if(aptNum==0) {
				return null;
			}
			string command="SELECT * FROM appointment "
				+"WHERE AptNum = '"+POut.Long(aptNum)+"'";
			return Crud.AppointmentCrud.SelectOne(command);
		}

		///<summary></summary>
		public static Appointment GetScheduledPlannedApt(long nextAptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Appointment>(MethodBase.GetCurrentMethod(),nextAptNum);
			}
			if(nextAptNum==0) {
				return null;
			}
			string command="SELECT * FROM appointment "
				+"WHERE NextAptNum = '"+POut.Long(nextAptNum)+"'";
			return Crud.AppointmentCrud.SelectOne(command);
		}

		///<summary>Gets a list of all future appointments which are either sched or ASAP.  Ordered by dateTime</summary>
		public static List<Appointment> GetFutureSchedApts(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM appointment "
				+"WHERE PatNum = "+POut.Long(patNum)+" "
				+"AND AptDateTime > "+DbHelper.Now()+" "
				+"AND (aptstatus = "+(int)ApptStatus.Scheduled+" "
				+"OR aptstatus = "+(int)ApptStatus.ASAP+") "
				+"ORDER BY AptDateTime";
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Gets a list of aptNums for one day in the schedule for a given set of providers.</summary>
		public static List<long> GetRouting(DateTime date,List<long> provNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),date,provNums);
			}
			string command=
				"SELECT AptNum FROM appointment "
				+"WHERE AptDateTime LIKE '"+POut.Date(date,false)+"%' "
				+"AND aptstatus != '"+(int)ApptStatus.UnschedList+"' "
				+"AND aptstatus != '"+(int)ApptStatus.Planned+"' "
				+"AND (";
			for(int i=0;i<provNums.Count;i++) {
				if(i>0) {
					command+=" OR";
				}
				command+=" ProvNum="+POut.Long(provNums[i])
					+" OR ProvHyg="+POut.Long(provNums[i]);
			}
			command+=") ORDER BY AptDateTime";
			DataTable table=Db.GetTable(command);
			List<long> retVal=new List<long>();
			for(int i=0;i<table.Rows.Count;i++) {
				retVal.Add(PIn.Long(table.Rows[i][0].ToString()));
			}
			return retVal;
			//return TableToObjects(table).ToArray();
		}

		public static List<Appointment> GetChangedSince(DateTime changedSince,DateTime excludeOlderThan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),changedSince,excludeOlderThan);
			}
			string command="SELECT * FROM appointment WHERE DateTStamp > "+POut.DateT(changedSince)
				+" AND AptDateTime > "+POut.DateT(excludeOlderThan);
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Used if the number of records are very large, in which case using GetChangedSince is not the preffered route due to memory problems caused by large recordsets. </summary>
		public static List<long> GetChangedSinceAptNums(DateTime changedSince,DateTime excludeOlderThan) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince,excludeOlderThan);
			}
			string command="SELECT AptNum FROM appointment WHERE DateTStamp > "+POut.DateT(changedSince)
				+" AND AptDateTime > "+POut.DateT(excludeOlderThan);
			DataTable dt=Db.GetTable(command);
			List<long> aptnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				aptnums.Add(PIn.Long(dt.Rows[i]["AptNum"].ToString()));
			}
			return aptnums;
		}

		///<summary>Used along with GetChangedSinceAptNums</summary>
		public static List<Appointment> GetMultApts(List<long> aptNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),aptNums);
			}
			//MessageBox.Show(patNums.Length.ToString());
			string strAptNums="";
			DataTable table;
			if(aptNums.Count>0) {
				for(int i=0;i<aptNums.Count;i++) {
					if(i>0) {
						strAptNums+="OR ";
					}
					strAptNums+="AptNum='"+aptNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM appointment WHERE "+strAptNums;
				//MessageBox.Show(string command);
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			Appointment[] multApts=Crud.AppointmentCrud.TableToList(table).ToArray();
			List<Appointment> aptList=new List<Appointment>(multApts);
			return aptList;
		}

		///<summary>A list of strings.  Each string corresponds to one appointment in the supplied list.  Each string is a comma delimited list of codenums of the procedures attached to the appointment.</summary>
		public static List<string> GetUAppointProcs(List<Appointment> appts){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),appts);
			}
			List<string> retVal=new List<string>();
			if(appts.Count==0){
				return retVal;
			}
			string command="SELECT AptNum,CodeNum FROM procedurelog WHERE AptNum IN(";
			for(int i=0;i<appts.Count;i++){
				if(i>0){
					command+=",";
				}
				command+=POut.Long(appts[i].AptNum);
			}
			command+=")";
			DataTable table=Db.GetTable(command);
			string str;
			for(int i=0;i<appts.Count;i++){
				str="";
				for(int p=0;p<table.Rows.Count;p++){
					if(table.Rows[p]["AptNum"].ToString()==appts[i].AptNum.ToString()){
						if(str!=""){
							str+=",";
						}
						str+=table.Rows[p]["CodeNum"].ToString();
					}
				}
				retVal.Add(str);
			}
			return retVal;
		}

		public static void Insert(Appointment appt) {
			if(DataConnection.DBtype==DatabaseType.MySql) {
				InsertIncludeAptNum(appt,false);
			}
			else {//Oracle must always have a valid PK.
				appt.AptNum=DbHelper.GetNextOracleKey("appointment","AptNum");
				InsertIncludeAptNum(appt,true);
			}
		}

		///<summary>Set includeAptNum to true only in rare situations.  Like when we are inserting for eCW.</summary>
		public static long InsertIncludeAptNum(Appointment appt,bool useExistingPK) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				appt.AptNum=Meth.GetLong(MethodBase.GetCurrentMethod(),appt,useExistingPK);
				return appt.AptNum;
			}
			//make sure all fields are properly filled:
			if(appt.Confirmed==0){
				appt.Confirmed=DefC.GetList(DefCat.ApptConfirmed)[0].DefNum;
			}
			if(appt.ProvNum==0){
				List<Provider> listProvs=ProviderC.GetListShort();
				appt.ProvNum=listProvs[0].ProvNum;
			}
			return Crud.AppointmentCrud.Insert(appt,useExistingPK);
		}

		///<summary>Updates only the changed columns and returns the number of rows affected.  Supply an oldApt for comparison.</summary>
		public static void Update(Appointment appointment,Appointment oldAppointment) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),appointment,oldAppointment);
				return;
			}
			Crud.AppointmentCrud.Update(appointment,oldAppointment);
		}

		///<summary>Updates InsPlan1 and InsPlan2 for every appointment that isn't completed, broken, or a patient note for the patient passed in.</summary>
		public static void UpdateInsPlansForPat(long patNum,long planNum1,long planNum2) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,planNum1,planNum2);
				return;
			}
			string command="UPDATE appointment SET appointment.InsPlan1="+planNum1+",appointment.InsPlan2="+planNum2
				+" WHERE appointment.AptStatus NOT IN ("+POut.Int((int)ApptStatus.Complete)
					+","+POut.Int((int)ApptStatus.Broken)
					+","+POut.Int((int)ApptStatus.PtNote)
					+","+POut.Int((int)ApptStatus.PtNoteCompleted)+")"
				+" AND appointment.PatNum="+patNum;
			Db.NonQ(command);
		}

		///<summary>Used in Chart module to test whether a procedure is attached to an appointment with today's date. The procedure might have a different date if still TP status.  ApptList should include all appointments for this patient. Does not make a call to db.</summary>
		public static bool ProcIsToday(Appointment[] apptList,Procedure proc){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<apptList.Length;i++){
				if(apptList[i].AptDateTime.Date==DateTime.Today
					&& apptList[i].AptNum==proc.AptNum
					&& (apptList[i].AptStatus==ApptStatus.Scheduled
					|| apptList[i].AptStatus==ApptStatus.ASAP
					|| apptList[i].AptStatus==ApptStatus.Broken
					|| apptList[i].AptStatus==ApptStatus.Complete))
				{
					return true;
				}
			}
			return false;
		}


		///<summary>Used in FormConfirmList.  The assumption is made that showRecall and showNonRecall will not both be false.</summary>
		public static DataTable GetConfirmList(DateTime dateFrom,DateTime dateTo,long provNum,long clinicNum,bool showRecall,bool showNonRecall,bool showHygPresched) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,provNum,clinicNum,showRecall,showNonRecall,showHygPresched);
			}
			DataTable table=new DataTable();
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("AddrNote");
			table.Columns.Add("AptNum");
			table.Columns.Add("age");
			table.Columns.Add("AptDateTime",typeof(DateTime));//This will actually be DateTimeAskedToArrive
			table.Columns.Add("aptDateTime");//This will actually be DateTimeAskedToArrive
			table.Columns.Add("ClinicNum");//patient.ClinicNum
			table.Columns.Add("confirmed");
			table.Columns.Add("contactMethod");
			table.Columns.Add("dateSched");
			table.Columns.Add("email");//could be patient or guarantor email.
			table.Columns.Add("Guarantor");
			table.Columns.Add("medNotes");
			table.Columns.Add("nameF");//or preferred.
			table.Columns.Add("nameFL");
			table.Columns.Add("Note");
			table.Columns.Add("patientName");
			table.Columns.Add("PatNum");
			table.Columns.Add("PreferConfirmMethod");
			table.Columns.Add("ProcDescript");
			table.Columns.Add("TxtMsgOk");
			table.Columns.Add("WirelessPhone");
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT patient.PatNum,patient.LName,patient.FName,patient.Preferred,patient.LName,patient.Guarantor,"
				+"AptDateTime,patient.Birthdate,patient.ClinicNum,patient.HmPhone,patient.TxtMsgOk,patient.WkPhone,"
				+"patient.WirelessPhone,ProcDescript,Confirmed,Note,patient.AddrNote,AptNum,patient.MedUrgNote,"
				+"patient.PreferConfirmMethod,guar.Email guarEmail,patient.Email,patient.Premed,DateTimeAskedToArrive,LogDateTime "
				+"FROM patient "
				+"INNER JOIN appointment ON appointment.PatNum=patient.PatNum "
				+"INNER JOIN patient guar ON guar.PatNum=patient.Guarantor "
				+"LEFT JOIN securitylog ON securitylog.PatNum=appointment.PatNum AND securitylog.PermType=25 AND securitylog.FKey=appointment.AptNum "
				+"WHERE AptDateTime > "+POut.Date(dateFrom)+" "
				//Example: AptDateTime="2014-11-26 13:00".  Filter is 11-26, giving "2014-11-27 00:00" to compare against.  This captures all times.
				+"AND AptDateTime < "+POut.Date(dateTo.AddDays(1))+" "
				+"AND AptStatus IN(1,4) ";//scheduled,ASAP
			if(provNum>0){
				command+="AND ((appointment.ProvNum="+POut.Long(provNum)+" AND appointment.IsHygiene=0) "//only include doc if it's not a hyg appt
					//"AND (appointment.ProvNum="+POut.Long(provNum)
					//+" OR appointment.ProvHyg="+POut.Long(provNum)+") ";
					+" OR (appointment.ProvHyg="+POut.Long(provNum)+" AND appointment.IsHygiene=1)) ";//only include hygienists if it's a hygiene appt
			}
			if(clinicNum>0) {
				command+="AND appointment.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			if(showRecall && !showNonRecall && !showHygPresched) {//Show recall only (the All option was not selected)
				command+="AND appointment.AptNum IN ("
					+"SELECT procedurelog.AptNum FROM procedurelog "
					+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
					+"AND procedurecode.IsHygiene=1) "//recall appt if there is 1 or more procedure on the appt that is marked IsHygiene
					+"AND patient.PatNum IN ("
					+"SELECT DISTINCT procedurelog.PatNum "
					+"FROM procedurelog "
					+"WHERE procedurelog.ProcStatus=2) ";//and the patient has had a procedure completed in the office (i.e. not the patient's first appt)
			}
			else if(!showRecall && showNonRecall && !showHygPresched) {//Show non-recall only (the All option was not selected)
				command+="AND (appointment.AptNum NOT IN ("
					+"SELECT AptNum FROM procedurelog "
					+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
					+"AND procedurecode.IsHygiene=1) "//include if the appointment does not have a procedure marked IsHygiene
					+"OR patient.PatNum NOT IN ("
					+"SELECT DISTINCT procedurelog.PatNum "
					+"FROM procedurelog "
					+"WHERE procedurelog.ProcStatus=2)) ";//or if the patient has never had a completed procedure (new patient appts)
			}
			else if(!showRecall && !showNonRecall && showHygPresched) {//Show hygiene prescheduled only (the All option was not selected)
				//Example: LogDateTime="2014-11-26 13:00".  Filter is 11-26, giving "2014-11-27 00:00" to compare against.  This captures all times for 11-26.
				string aptDateSql="";
				if(DataConnection.DBtype==DatabaseType.MySql) {
					aptDateSql="DATE(appointment.AptDateTime-INTERVAL 2 MONTH)";
				}
				else {
					aptDateSql="ADD_MONTHS(TO_CHAR(appointment.AptDateTime,'MM/DD/YYYY %HH24:%MI:%SS'),-2)";
				}
				command+="AND (securitylog.PatNum IS NULL OR securitylog.LogDateTime < "+aptDateSql+") ";
			}
			command+="ORDER BY AptDateTime";
			DataTable rawtable=Db.GetTable(command);
			DateTime dateT;
			DateTime timeAskedToArrive;
			Patient pat;
			ContactMethod contmeth;
			for(int i=0;i<rawtable.Rows.Count;i++) {
				row=table.NewRow();
				row["AddrNote"]=rawtable.Rows[i]["AddrNote"].ToString();
				row["AptNum"]=rawtable.Rows[i]["AptNum"].ToString();
				row["age"]=Patients.DateToAge(PIn.Date(rawtable.Rows[i]["Birthdate"].ToString())).ToString();//we don't care about m/y.
				dateT=PIn.DateT(rawtable.Rows[i]["AptDateTime"].ToString());
				timeAskedToArrive=PIn.DateT(rawtable.Rows[i]["DateTimeAskedToArrive"].ToString());
				if(timeAskedToArrive.Year>1880) {
					dateT=timeAskedToArrive;
				}
				row["AptDateTime"]=dateT;
				row["aptDateTime"]=dateT.ToShortDateString()+"\r\n"+dateT.ToShortTimeString();
				row["ClinicNum"]=rawtable.Rows[i]["ClinicNum"].ToString();
				row["confirmed"]=DefC.GetName(DefCat.ApptConfirmed,PIn.Long(rawtable.Rows[i]["Confirmed"].ToString()));
				contmeth=(ContactMethod)PIn.Int(rawtable.Rows[i]["PreferConfirmMethod"].ToString());
				if(contmeth==ContactMethod.None || contmeth==ContactMethod.HmPhone) {
					row["contactMethod"]=Lans.g("FormConfirmList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
				}
				if(contmeth==ContactMethod.WkPhone) {
					row["contactMethod"]=Lans.g("FormConfirmList","Wk:")+rawtable.Rows[i]["WkPhone"].ToString();
				}
				if(contmeth==ContactMethod.WirelessPh) {
					row["contactMethod"]=Lans.g("FormConfirmList","Cell:")+rawtable.Rows[i]["WirelessPhone"].ToString();
				}
				if(contmeth==ContactMethod.TextMessage) {
					row["contactMethod"]=Lans.g("FormConfirmList","Text:")+rawtable.Rows[i]["WirelessPhone"].ToString();
				}
				if(contmeth==ContactMethod.Email) {
					row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
				}
				if(contmeth==ContactMethod.DoNotCall || contmeth==ContactMethod.SeeNotes) {
					row["contactMethod"]=Lans.g("enumContactMethod",contmeth.ToString());
				}
				row["dateSched"]="Unknown";
				if(rawtable.Rows[i]["LogDateTime"].ToString().Length>0) {
					row["dateSched"]=rawtable.Rows[i]["LogDateTime"].ToString();
				}
				if(rawtable.Rows[i]["Email"].ToString()=="" && rawtable.Rows[i]["guarEmail"].ToString()!="") {
					row["email"]=rawtable.Rows[i]["guarEmail"].ToString();
				}
				else {
					row["email"]=rawtable.Rows[i]["Email"].ToString();
				}
				row["Guarantor"]=rawtable.Rows[i]["Guarantor"].ToString();
				row["medNotes"]="";
				if(rawtable.Rows[i]["Premed"].ToString()=="1"){
					row["medNotes"]=Lans.g("FormConfirmList","Premedicate");
				}
				if(rawtable.Rows[i]["MedUrgNote"].ToString()!=""){
					if(row["medNotes"].ToString()!="") {
						row["medNotes"]+="\r\n";
					}
					row["medNotes"]+=rawtable.Rows[i]["MedUrgNote"].ToString();
				}
				pat=new Patient();
				pat.LName=rawtable.Rows[i]["LName"].ToString();
				pat.FName=rawtable.Rows[i]["FName"].ToString();
				pat.Preferred=rawtable.Rows[i]["Preferred"].ToString();
				row["nameF"]=pat.GetNameFirstOrPreferred();
				row["nameFL"]=pat.GetNameFirstOrPrefL();
				row["Note"]=rawtable.Rows[i]["Note"].ToString();
				row["patientName"]=	pat.LName+"\r\n";
				if(pat.Preferred!=""){
					row["patientName"]+="'"+pat.Preferred+"'";
				}
				else{
					row["patientName"]+=pat.FName;
				}
				row["PatNum"]=rawtable.Rows[i]["PatNum"].ToString();
				row["PreferConfirmMethod"]=rawtable.Rows[i]["PreferConfirmMethod"].ToString();
				row["ProcDescript"]=rawtable.Rows[i]["ProcDescript"].ToString();
				row["TxtMsgOk"]=rawtable.Rows[i]["TxtMsgOk"].ToString();
				row["WirelessPhone"]=rawtable.Rows[i]["WirelessPhone"].ToString();
				rows.Add(row);
			}
			//Array.Sort(orderDate,RecallList);
			//return RecallList;
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Used in Confirm list to just get addresses.</summary>
		public static DataTable GetAddrTable(List<long> aptNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptNums);
			}
			string command="SELECT patient.LName,patient.FName,patient.MiddleI,patient.Preferred,"
				+"patient.Address,patient.Address2,patient.City,patient.State,patient.Zip,appointment.AptDateTime,appointment.ClinicNum "
				+"FROM patient,appointment "
				+"WHERE patient.PatNum=appointment.PatNum "
				+"AND (FALSE";//simplifies the remaining OR clauses
			for(int i=0;i<aptNums.Count;i++){
				command+=" OR appointment.AptNum="+aptNums[i].ToString();
			}
			command+=") ORDER BY appointment.AptDateTime";
			return Db.GetTable(command);
		}

		///<summary>The newStatus will be a DefNum or 0.</summary>
		public static void SetConfirmed(long aptNum,long newStatus) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),aptNum,newStatus);
				return;
			}
			string command="UPDATE appointment SET Confirmed="+POut.Long(newStatus);
			if(PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger)==newStatus){
				command+=",DateTimeArrived="+DbHelper.Now();
			}
			else if(PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger)==newStatus){
				command+=",DateTimeSeated="+DbHelper.Now();
			}
			else if(PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger)==newStatus){
				command+=",DateTimeDismissed="+DbHelper.Now();
			}
			command+=" WHERE AptNum="+POut.Long(aptNum);
			Db.NonQ(command);
			Plugins.HookAddCode(null, "Appointments.SetConfirmed_end", aptNum, newStatus); 
		}

		///<summary>Sets the new pattern for an appointment.  This is how resizing is done.  Must contain only / and X, with each char representing 5 minutes.</summary>
		public static void SetPattern(long aptNum,string newPattern) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),aptNum,newPattern);
				return;
			}
			string command="UPDATE appointment SET Pattern='"+POut.String(newPattern)+"' WHERE AptNum="+POut.Long(aptNum);
			Db.NonQ(command);
		}

		///<summary>Use to send to unscheduled list, to set broken, etc.  Do not use to set complete.</summary>
		public static void SetAptStatus(long aptNum,ApptStatus newStatus) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),aptNum,newStatus);
				return;
			}
			string command="UPDATE appointment SET AptStatus="+POut.Long((int)newStatus)
				+" WHERE AptNum="+POut.Long(aptNum);
			Db.NonQ(command);
		}

		///<summary>The plan nums that are passed in are simply saved in columns in the appt.  Those two fields are used by approximately one office right now.</summary>
		public static void SetAptStatusComplete(long aptNum,long planNum1,long planNum2) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),aptNum,planNum1,planNum2);
				return;
			}
			string command="UPDATE appointment SET "
				+"AptStatus="+POut.Long((int)ApptStatus.Complete)+", "
				+"InsPlan1="+POut.Long(planNum1)+", "
				+"InsPlan2="+POut.Long(planNum2)+" "
				+"WHERE AptNum="+POut.Long(aptNum);
			Db.NonQ(command);
		}

		public static void SetAptTimeLocked() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="UPDATE appointment SET TimeLocked="+POut.Bool(true);
			Db.NonQ(command);
		}
		
		public static Appointment TableToObject(DataTable table) {
			//No need to check RemotingRole; no call to db.
			if(table.Rows.Count==0) {
				return null;
			}
			return Crud.AppointmentCrud.TableToList(table)[0];
		}

		///<summary></summary>
		public static DataSet RefreshPeriod(DateTime dateStart,DateTime dateEnd) {
			return RefreshPeriod(dateStart,dateEnd,0);
		}

		///<summary>Set clinicNum to 0 to return 'all' clinics.  Otherwise, filters the data set on the clinic num passed in.  Currently only filters GetPeriodEmployeeSchedTable()</summary>
		public static DataSet RefreshPeriod(DateTime dateStart,DateTime dateEnd,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateStart,dateEnd,clinicNum);
			} 
			DataSet retVal=new DataSet();
			DataTable tableAppt=GetPeriodApptsTable(dateStart,dateEnd,0,false);
			retVal.Tables.Add(tableAppt);
			retVal.Tables.Add(GetPeriodEmployeeSchedTable(dateStart,dateEnd,clinicNum));
			//retVal.Tables.Add(GetPeriodWaitingRoomTable(clinicNum));
			retVal.Tables.Add(GetPeriodWaitingRoomTable());
			retVal.Tables.Add(GetPeriodSchedule(dateStart,dateEnd));
			retVal.Tables.Add(GetApptFields(tableAppt));
			retVal.Tables.Add(GetPatFields(tableAppt));
			return retVal;
		}

		///<summary></summary>
		public static DataSet RefreshOneApt(long aptNum,bool isPlanned) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),aptNum,isPlanned);
			} 
			DataSet retVal=new DataSet();
			retVal.Tables.Add(GetPeriodApptsTable(DateTime.MinValue,DateTime.MinValue,aptNum,isPlanned));
			return retVal;
		}

		///<summary>If aptnum is specified, then the dates are ignored.  If getting data for one planned appt, then pass isPlanned=1.  The times of the dateStart and dateEnd are ignored.  This changes which procedures are retrieved.</summary>
		public static DataTable GetPeriodApptsTable(DateTime dateStart,DateTime dateEnd,long aptNum,bool isPlanned) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,aptNum,isPlanned);
			} 
			DataConnection dcon=new DataConnection();
			DataTable table=new DataTable("Appointments");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("age");
			table.Columns.Add("address");
			table.Columns.Add("addrNote");
			table.Columns.Add("apptModNote");
			table.Columns.Add("AppointmentTypeNum");
			table.Columns.Add("aptDate");
			table.Columns.Add("aptDay");
			table.Columns.Add("aptLength");
			table.Columns.Add("aptTime");
			table.Columns.Add("AptDateTime");
			table.Columns.Add("AptDateTimeArrived");
			table.Columns.Add("AptNum");
			table.Columns.Add("AptStatus");
			table.Columns.Add("Assistant");
			table.Columns.Add("assistantAbbr");
			table.Columns.Add("billingType");
			table.Columns.Add("chartNumber");
			table.Columns.Add("chartNumAndName");
			table.Columns.Add("ColorOverride");
			table.Columns.Add("confirmed");
			table.Columns.Add("Confirmed");
			table.Columns.Add("contactMethods");
			//table.Columns.Add("creditIns");
			table.Columns.Add("CreditType");
			table.Columns.Add("Email");
			table.Columns.Add("famFinUrgNote");
			table.Columns.Add("guardians");
			table.Columns.Add("hasIns[I]");
			table.Columns.Add("hmPhone");
			table.Columns.Add("ImageFolder");
			table.Columns.Add("insurance");
			table.Columns.Add("insToSend[!]");
			table.Columns.Add("IsHygiene");
			table.Columns.Add("lab");
			table.Columns.Add("language");
			table.Columns.Add("medOrPremed[+]");
			table.Columns.Add("MedUrgNote");
			table.Columns.Add("Note");
			table.Columns.Add("Op");
			table.Columns.Add("patientName");
			table.Columns.Add("patientNameF");
			table.Columns.Add("PatNum");
			table.Columns.Add("patNum");
			table.Columns.Add("GuarNum");
			table.Columns.Add("patNumAndName");
			table.Columns.Add("Pattern");
			table.Columns.Add("preMedFlag");
			table.Columns.Add("procs");
			table.Columns.Add("procsColored");
			table.Columns.Add("production");
			table.Columns.Add("productionVal");
			table.Columns.Add("provider");
			table.Columns.Add("ProvHyg");
			table.Columns.Add("ProvNum");
			table.Columns.Add("referralFrom");
			table.Columns.Add("referralTo");
			table.Columns.Add("timeAskedToArrive");
			table.Columns.Add("wkPhone");
			table.Columns.Add("wirelessPhone");
			table.Columns.Add("writeoffPPO");
			string command="SELECT p1.Abbr ProvAbbr,p2.Abbr HygAbbr,patient.Address patAddress1,patient.Address2 patAddress2,patient.AddrNote patAddrNote,"
				+"patient.ApptModNote patApptModNote,appointment.AppointmentTypeNum,appointment.AptDateTime apptAptDateTime,appointment.DateTimeArrived apptAptDateTimeArrived,appointment.AptNum apptAptNum,"
			  +"appointment.AptStatus apptAptStatus,appointment.Assistant apptAssistant,"
				+"patient.BillingType patBillingType,patient.BirthDate patBirthDate,patient.DateTimeDeceased patDateTimeDeceased,"
				+"carrier1.CarrierName carrierName1,carrier2.CarrierName carrierName2,"
				+"patient.ChartNumber patChartNumber,patient.City patCity,appointment.ColorOverride apptColorOverride,appointment.Confirmed apptConfirmed,"
				+"patient.CreditType patCreditType,labcase.DateTimeChecked labcaseDateTimeChecked,"
				+"labcase.DateTimeDue labcaseDateTimeDue,labcase.DateTimeRecd labcaseDateTimeRecd,labcase.DateTimeSent labcaseDateTimeSent,appointment.DateTimeAskedToArrive apptDateTimeAskedToArrive,"
				+"patient.Email patEmail,guar.FamFinUrgNote guarFamFinUrgNote,patient.FName patFName,patient.Guarantor patGuarantor,"
				+"COUNT(AllergyNum) hasAllergy,"
				+"COUNT(DiseaseNum) hasDisease,"
				+"patient.HmPhone patHmPhone,patient.ImageFolder patImageFolder,appointment.IsHygiene apptIsHygiene,appointment.IsNewPatient apptIsNewPatient,"
				+"labcase.LabCaseNum labcaseLabCaseNum,patient.Language patLanguage,patient.LName patLName,patient.MedUrgNote patMedUrgNote,"
				+"patient.MiddleI patMiddleI,appointment.Note apptNote,appointment.Op apptOp,appointment.PatNum apptPatNum,"
				+"appointment.Pattern apptPattern,COUNT(patplan.InsSubNum) hasIns,patient.PreferConfirmMethod patPreferConfirmMethod,"
				+"patient.PreferContactMethod patPreferContactMethod,patient.Preferred patPreferred,"
				+"patient.PreferRecallMethod patPreferRecallMethod,patient.Premed patPremed,"
				+"appointment.ProcDescript apptProcDescript,appointment.ProcsColored apptProcsColored,appointment.ProvHyg apptProvHyg,appointment.ProvNum apptProvNum,"
				+"patient.State patState,patient.WirelessPhone patWirelessPhone,patient.WkPhone patWkPhone,patient.Zip patZip "
				+"FROM appointment "
				+"LEFT JOIN patient ON patient.PatNum=appointment.PatNum "
				+"LEFT JOIN provider p1 ON p1.ProvNum=appointment.ProvNum "
				+"LEFT JOIN provider p2 ON p2.ProvNum=appointment.ProvHyg ";
			if(isPlanned){
				command+="LEFT JOIN labcase ON labcase.PlannedAptNum=appointment.AptNum AND labcase.PlannedAptNum!=0 ";
			}
			else{
				command+="LEFT JOIN labcase ON labcase.AptNum=appointment.AptNum AND labcase.AptNum!=0 ";
			}
			command+="LEFT JOIN patient guar ON guar.PatNum=patient.Guarantor "
				+"LEFT JOIN patplan ON patplan.PatNum=patient.PatNum AND patplan.Ordinal=1 "
				//these four lines are very rarely made use of. They depend on the appointment.InsPlan1/2 being filled, which is unreliable.
				+"LEFT JOIN insplan plan1 ON InsPlan1=plan1.PlanNum "
				+"LEFT JOIN insplan plan2 ON InsPlan2=plan2.PlanNum "
				+"LEFT JOIN carrier carrier1 ON plan1.CarrierNum=carrier1.CarrierNum "
				+"LEFT JOIN carrier carrier2 ON plan2.CarrierNum=carrier2.CarrierNum "
				+"LEFT JOIN disease ON patient.PatNum=disease.PatNum AND disease.DiseaseDefNum <> "+POut.Long(PrefC.GetLong(PrefName.ProblemsIndicateNone))+" "
				+"LEFT JOIN allergy ON patient.PatNum=allergy.PatNum AND allergy.AllergyDefNum <> "+POut.Long(PrefC.GetLong(PrefName.AllergiesIndicateNone))+" ";
			if(aptNum==0){
				command+="WHERE AptDateTime >= "+POut.Date(dateStart)+" "
					+"AND AptDateTime < "+POut.Date(dateEnd.AddDays(1))+" "
					+ "AND AptStatus IN (1, 2, 4, 5, 7, 8) ";
			}
			else{
				command+="WHERE appointment.AptNum="+POut.Long(aptNum);
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+=" GROUP BY appointment.AptNum";
			}
			else {//Oracle
				command+=" GROUP BY p1.Abbr,p2.Abbr,patient.Address,patient.Address2,patient.AddrNote,"
				+"patient.ApptModNote,appointment.AppointmentTypeNum,AptDateTime,appointment.DateTimeArrived,appointment.AptNum,appointment.AptStatus,appointment.Assistant,"
				+"patient.BillingType,patient.BirthDate,patient.DateTimeDeceased,"
				+"carrier1.CarrierName,carrier2.CarrierName,"
				+"patient.ChartNumber,patient.City,appointment.ColorOverride,appointment.Confirmed,patient.CreditType,"
				+"labcase.DateTimeChecked,labcase.DateTimeDue,labcase.DateTimeRecd,labcase.DateTimeSent,appointment.DateTimeAskedToArrive,"
				+"patient.Email,guar.FamFinUrgNote,patient.FName,patient.Guarantor,patient.HmPhone,patient.ImageFolder,appointment.IsHygiene,appointment.IsNewPatient,"
				+"labcase.LabCaseNum,patient.Language,patient.LName,patient.MedUrgNote,patient.MiddleI,appointment.Note,appointment.Op,appointment.PatNum,"
				+"Pattern,patient.PreferConfirmMethod,patient.PreferContactMethod,patient.Preferred,"
				+"patient.PreferRecallMethod,patient.Premed,"
				+"appointment.ProcDescript,appointment.ProcsColored,appointment.ProvHyg,appointment.ProvNum,"
				+"patient.State,patient.WirelessPhone,patient.WkPhone,patient.Zip ";
			}
			DataTable raw=dcon.GetTable(command);
			//rawProc table was historically used for other purposes.  It is currently only used for production--------------------------
			//rawProcLab table is only used for Canada and goes hand in hand with the rawProc table, also only used for production.
			DataTable rawProc;
			DataTable rawProcLab=null;
			if(raw.Rows.Count==0){
				rawProc=new DataTable();
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					rawProcLab=new DataTable();
				}
			}
			else{
				command="SELECT AptNum,PlannedAptNum,"//AbbrDesc,procedurecode.CodeNum
					+"ProcFee,";
				if(dateStart.Date<DateTime.Now.Date) {//Use the actual writeoff if looking at a date in the past, otherwise writeoff estimates will be used.
					command+="SUM(WriteOff) writeoffPPO,";
				}
				else {
					command+="SUM(CASE WHEN WriteOffEstOverride!=-1 THEN WriteOffEstOverride ELSE WriteOffEst END) writeoffPPO,";
				}
				command+="procedurelog.ProcNum "
					+"FROM procedurelog "
					+"LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum "
					+"AND (claimproc.WriteOffEst != -1 "
					+"OR claimproc.WriteOffEstOverride != -1) "
					+"WHERE ProcNumLab=0 AND ";
				if(isPlanned) {
					command+="PlannedAptNum!=0 AND PlannedAptNum ";
				} 
				else {
					command+="AptNum!=0 AND AptNum ";
				}
				command+="IN(";//this was far too slow:SELECT a.AptNum FROM appointment a WHERE ";
				if(aptNum==0) {
					for(int a=0;a<raw.Rows.Count;a++){
						if(a>0){
							command+=",";
						}
						command+=raw.Rows[a]["apptAptNum"].ToString();
					}
				}
				else {
					command+=POut.Long(aptNum);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command+=") GROUP BY procedurelog.ProcNum";
				}
				else {//Oracle
					command+=") GROUP BY procedurelog.ProcNum,AptNum,PlannedAptNum,ProcFee";
				}
				rawProc=dcon.GetTable(command);
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && rawProc.Rows.Count>0) {//Canadian. en-CA or fr-CA
					command="SELECT procedurelog.ProcNum,ProcNumLab,ProcFee,SUM(CASE WHEN WriteOffEstOverride!=-1 THEN WriteOffEstOverride ELSE WriteOffEst END) writeoffPPO "
						+"FROM procedurelog "
						+"LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum "
						+"AND (claimproc.WriteOffEst != -1 "
						+"OR claimproc.WriteOffEstOverride != -1) "
						+"WHERE ProcNumLab IN (";
					for(int i=0;i<rawProc.Rows.Count;i++) {
						if(i>0) {
							command+=",";
						}
						command+=rawProc.Rows[i]["ProcNum"].ToString();
					}
					if(DataConnection.DBtype==DatabaseType.MySql) {
						command+=") GROUP BY procedurelog.ProcNum";
					}
					else {//Oracle
						command+=") GROUP BY procedurelog.ProcNum,ProcNumLab,ProcFee";
					}
					rawProcLab=dcon.GetTable(command);
				}
			}
			//rawInsProc table is usually skipped. Too slow------------------------------------------------------------------------------
			DataTable rawInsProc=null;
			if(PrefC.GetBool(PrefName.ApptExclamationShowForUnsentIns)){
				//procs for flag, InsNotSent
				command ="SELECT patient.PatNum, patient.Guarantor "
					+"FROM patient,procedurecode,procedurelog,claimproc "
					+"WHERE claimproc.procnum=procedurelog.procnum "
					+"AND patient.PatNum=procedurelog.PatNum "
					+"AND procedurelog.CodeNum=procedurecode.CodeNum "
					+"AND claimproc.NoBillIns=0 "
					+"AND procedurelog.ProcFee>0 "
					+"AND claimproc.Status=6 "//estimate
					+"AND ((CASE WHEN claimproc.InsEstTotalOverride>-1 THEN claimproc.InsEstTotalOverride ELSE claimproc.InsEstTotal END) > 0) "
					+"AND procedurelog.procstatus=2 "
					+"AND procedurelog.ProcDate >= "+POut.Date(DateTime.Now.AddYears(-1))+" "//I'm sure this is the slow part.  Should be easy to make faster with less range
					+"AND procedurelog.ProcDate <= "+POut.Date(DateTime.Now)+ " "
					+"GROUP BY patient.PatNum, patient.Guarantor"; 
				rawInsProc=dcon.GetTable(command);
			}
			//Guardians-------------------------------------------------------------------------------------------------------------------
			command="SELECT PatNumChild,PatNumGuardian,Relationship,patient.FName,patient.Preferred "
				+"FROM guardian "
				+"LEFT JOIN patient ON patient.PatNum=guardian.PatNumGuardian "
				+"WHERE IsGuardian<>0 AND PatNumChild IN (";
			if(raw.Rows.Count==0){
				command+="0";
			}
			else for(int i=0;i<raw.Rows.Count;i++) {
				if(i>0) {
					command+=",";
				}
				command+=raw.Rows[i]["apptPatNum"].ToString();
			}
			command+=") ORDER BY Relationship";
			DataTable rawGuardians=dcon.GetTable(command);
			Dictionary<long,string> dictRefFromPatNums=new Dictionary<long,string>();//Only contains FROM referrals 
			Dictionary<long,string> dictRefToPatNums=new Dictionary<long,string>();//Only contains TO referrals
			List<long> listPatNums=new List<long>();
			List<long> listRefNums=new List<long>();
			for(int i=0;i<raw.Rows.Count;i++) {
				if(!listPatNums.Contains((long)raw.Rows[i]["apptPatNum"])) {
					listPatNums.Add((long)raw.Rows[i]["apptPatNum"]);
				}
			}
			List<RefAttach> listRefAttaches=RefAttaches.GetRefAttaches(listPatNums);
			for(int i=0;i<listRefAttaches.Count;i++) {
				if(!listRefNums.Contains(listRefAttaches[i].ReferralNum)) {
					listRefNums.Add(listRefAttaches[i].ReferralNum);
				}
			}
			List<Referral> listReferrals=Referrals.GetReferrals(listRefNums);
			for(int j=0;j<listRefAttaches.Count;j++) {
				string nameLF="";
				for(int k=0;k<listReferrals.Count;k++) {
					if(listRefAttaches[j].ReferralNum==listReferrals[k].ReferralNum) {
						nameLF=listReferrals[k].LName+", "+listReferrals[k].FName;
					}
				}
				if(listRefAttaches[j].IsFrom) {
					if(!dictRefFromPatNums.ContainsKey(listRefAttaches[j].PatNum)) {//New entry
						dictRefFromPatNums.Add(listRefAttaches[j].PatNum,Lans.g("Appointments","Referred From")+":");
					}
					dictRefFromPatNums[listRefAttaches[j].PatNum]+=("\r\n"+nameLF);//Concatenate all refFrom nameLF's to the refFrom dict
				}
				else {
					if(!dictRefToPatNums.ContainsKey(listRefAttaches[j].PatNum)) {
						dictRefToPatNums.Add(listRefAttaches[j].PatNum,Lans.g("Appointments","Referred To")+":");//New entry
					}
					dictRefToPatNums[listRefAttaches[j].PatNum]+=("\r\n"+nameLF);//Concatenate all refTo nameLF's to the refTo dict
				}
			}
			DateTime aptDate;
			DateTime aptDateArrived;
			TimeSpan span;
			int hours;
			int minutes;
			DateTime labDate;
			DateTime labDueDate;
			DateTime birthdate;
			DateTime timeAskedToArrive;
			decimal production;
			decimal writeoffPPO;
			for(int i=0;i<raw.Rows.Count;i++) {
				row=table.NewRow();
				row["address"]=Patients.GetAddressFull(raw.Rows[i]["patAddress1"].ToString(),raw.Rows[i]["patAddress2"].ToString(),
					raw.Rows[i]["patCity"].ToString(),raw.Rows[i]["patState"].ToString(),raw.Rows[i]["patZip"].ToString());
				row["addrNote"]="";
				if(raw.Rows[i]["patAddrNote"].ToString()!=""){
					row["addrNote"]=Lans.g("Appointments","AddrNote: ")+raw.Rows[i]["patAddrNote"].ToString();
				}
				aptDate=PIn.DateT(raw.Rows[i]["apptAptDateTime"].ToString());
				aptDateArrived=PIn.DateT(raw.Rows[i]["apptAptDateTimeArrived"].ToString());
				row["AptDateTime"]=aptDate;
				row["AptDateTimeArrived"]=aptDateArrived;
				birthdate=PIn.Date(raw.Rows[i]["patBirthdate"].ToString());
				DateTime dateTimeDeceased=PIn.Date(raw.Rows[i]["patDateTimeDeceased"].ToString());
				DateTime dateTimeTo=DateTime.Now;
				if(dateTimeDeceased.Year>1880) {
					dateTimeTo=dateTimeDeceased;
				}
				row["age"]="";
				if(birthdate.AddYears(18)<dateTimeTo) {
					row["age"]=Lans.g("Appointments","Age: ");//only show if older than 18
				}
				if(birthdate.Year>1880){
					row["age"]+=PatientLogic.DateToAgeString(birthdate,dateTimeTo);
				}
				else{
					row["age"]+="?";
				}
				row["apptModNote"]="";
				if(raw.Rows[i]["patApptModNote"].ToString()!="") {
					row["apptModNote"]=Lans.g("Appointments","ApptModNote: ")+raw.Rows[i]["patApptModNote"].ToString();
				}
				row["aptDate"]=aptDate.ToShortDateString();
				row["aptDay"]=aptDate.ToString("dddd");
				span=TimeSpan.FromMinutes(raw.Rows[i]["apptPattern"].ToString().Length*5);
				hours=span.Hours;
				minutes=span.Minutes;
				if(hours==0){
					row["aptLength"]=minutes.ToString()+Lans.g("Appointments"," Min");
				}
				else if(hours==1){
					row["aptLength"]=hours.ToString()+Lans.g("Appointments"," Hr, ")
						+minutes.ToString()+Lans.g("Appointments"," Min");
				}
				else{
					row["aptLength"]=hours.ToString()+Lans.g("Appointments"," Hrs, ")
						+minutes.ToString()+Lans.g("Appointments"," Min");
				}
				row["aptTime"]=aptDate.ToShortTimeString();
				row["AptNum"]=raw.Rows[i]["apptAptNum"].ToString();
				row["AptStatus"]=raw.Rows[i]["apptAptStatus"].ToString();
				row["Assistant"]=raw.Rows[i]["apptAssistant"].ToString();
				row["assistantAbbr"]="";
				if(row["Assistant"].ToString()!="0") {
					row["assistantAbbr"]=Employees.GetAbbr(PIn.Long(raw.Rows[i]["apptAssistant"].ToString()));
				}
				row["AppointmentTypeNum"]=raw.Rows[i]["AppointmentTypeNum"].ToString();
				row["billingType"]=DefC.GetName(DefCat.BillingTypes,PIn.Long(raw.Rows[i]["patBillingType"].ToString()));
				row["chartNumber"]=raw.Rows[i]["patChartNumber"].ToString();
				row["chartNumAndName"]="";
				if(raw.Rows[i]["apptIsNewPatient"].ToString()=="1") {
					row["chartNumAndName"]="NP-";
				}
				row["chartNumAndName"]+=raw.Rows[i]["patChartNumber"].ToString()+" "
					+PatientLogic.GetNameLF(raw.Rows[i]["patLName"].ToString(),raw.Rows[i]["patFName"].ToString(),
					raw.Rows[i]["patPreferred"].ToString(),raw.Rows[i]["patMiddleI"].ToString());
				row["ColorOverride"]=raw.Rows[i]["apptColorOverride"].ToString();
				row["confirmed"]=DefC.GetName(DefCat.ApptConfirmed,PIn.Long(raw.Rows[i]["apptConfirmed"].ToString()));
				row["Confirmed"]=raw.Rows[i]["apptConfirmed"].ToString();
				row["contactMethods"]="";
				if(raw.Rows[i]["patPreferConfirmMethod"].ToString()!="0"){
					row["contactMethods"]+=Lans.g("Appointments","Confirm Method: ")
						+((ContactMethod)PIn.Long(raw.Rows[i]["patPreferConfirmMethod"].ToString())).ToString();
				}
				if(raw.Rows[i]["patPreferContactMethod"].ToString()!="0"){
					if(row["contactMethods"].ToString()!="") {
						row["contactMethods"]+="\r\n";
					}
					row["contactMethods"]+=Lans.g("Appointments","Contact Method: ")
						+((ContactMethod)PIn.Long(raw.Rows[i]["patPreferContactMethod"].ToString())).ToString();
				}
				if(raw.Rows[i]["patPreferRecallMethod"].ToString()!="0"){
					if(row["contactMethods"].ToString()!="") {
						row["contactMethods"]+="\r\n";
					}
					row["contactMethods"]+=Lans.g("Appointments","Recall Method: ")
						+((ContactMethod)PIn.Long(raw.Rows[i]["patPreferRecallMethod"].ToString())).ToString();
				}
				bool InsToSend=false;
				if(rawInsProc!=null){
					//figure out if pt's family has ins claims that need to be created
					for(int j=0;j<rawInsProc.Rows.Count;j++){
						if(raw.Rows[i]["hasIns"].ToString()!="0") {
							if (raw.Rows[i]["patGuarantor"].ToString()==rawInsProc.Rows[j]["Guarantor"].ToString() 
								|| raw.Rows[i]["patGuarantor"].ToString()==rawInsProc.Rows[j]["PatNum"].ToString())
							{
								InsToSend=true;
							}
						}
					}
				}
				row["CreditType"]=raw.Rows[i]["patCreditType"].ToString();
				row["Email"]=raw.Rows[i]["patEmail"].ToString();
				row["famFinUrgNote"]="";
				if(raw.Rows[i]["guarFamFinUrgNote"].ToString()!="") {
					row["famFinUrgNote"]=Lans.g("Appointments","FamFinUrgNote: ")+raw.Rows[i]["guarFamFinUrgNote"].ToString();
				}
				row["guardians"]="";
				GuardianRelationship guardRelat;
				for(int g=0;g<rawGuardians.Rows.Count;g++) {
					if(raw.Rows[i]["apptPatNum"].ToString()==rawGuardians.Rows[g]["PatNumChild"].ToString()) {
						if(row["guardians"].ToString()!="") {
							row["guardians"]+=",";
						}
						guardRelat=(GuardianRelationship)PIn.Int(rawGuardians.Rows[g]["Relationship"].ToString());
						row["guardians"]+=Patients.GetNameFirstOrPreferred(rawGuardians.Rows[g]["FName"].ToString(),rawGuardians.Rows[g]["Preferred"].ToString())
							+Guardians.GetGuardianRelationshipStr(guardRelat);
					}
				}
				row["hasIns[I]"]="";
				if(raw.Rows[i]["hasIns"].ToString()!="0") {
					row["hasIns[I]"]+="I";
				}
				row["hmPhone"]=Lans.g("Appointments","Hm: ")+raw.Rows[i]["patHmPhone"].ToString();
				row["ImageFolder"]=raw.Rows[i]["patImageFolder"].ToString();
				row["insurance"]="";
				if(raw.Rows[i]["carrierName1"].ToString()!="") {
					row["insurance"]+="Ins1: "+raw.Rows[i]["carrierName1"].ToString();
					if(raw.Rows[i]["carrierName2"].ToString()!="") {
						//if(row["insurance"].ToString()!="") {
						row["insurance"]+="\r\n";
						//}
						row["insurance"]+="Ins2: "+raw.Rows[i]["carrierName2"].ToString();
					}
				}
				else if(raw.Rows[i]["hasIns"].ToString()!="0") {
					row["insurance"]=Lans.g("Appointments","Insured");
				}
				row["insToSend[!]"]="";
				if(InsToSend) {
					row["insToSend[!]"]="!";
				}
				row["IsHygiene"]=raw.Rows[i]["apptIsHygiene"].ToString();
				row["lab"]="";
				if(raw.Rows[i]["labcaseLabCaseNum"].ToString()!=""){
					labDate=PIn.DateT(raw.Rows[i]["labcaseDateTimeChecked"].ToString());
					if(labDate.Year>1880) {
						row["lab"]=Lans.g("Appointments","Lab Quality Checked");
					}
					else {
						labDate=PIn.DateT(raw.Rows[i]["labcaseDateTimeRecd"].ToString());
						if(labDate.Year>1880) {
							row["lab"]=Lans.g("Appointments","Lab Received");
						}
						else {
							labDate=PIn.DateT(raw.Rows[i]["labcaseDateTimeSent"].ToString());
							if(labDate.Year>1880) {
								row["lab"]=Lans.g("Appointments","Lab Sent");//sent but not received
							}
							else {
								row["lab"]=Lans.g("Appointments","Lab Not Sent");
							}
							labDueDate=PIn.DateT(raw.Rows[i]["labcaseDateTimeDue"].ToString());
							if(labDueDate.Year>1880) {
								row["lab"]+=", "+Lans.g("Appointments","Due: ")//+dateDue.ToString("ddd")+" "
									+labDueDate.ToShortDateString();//+" "+dateDue.ToShortTimeString();
							}
						}
					}
				}
				CultureInfo culture=CodeBase.MiscUtils.GetCultureFromThreeLetter(raw.Rows[i]["patLanguage"].ToString());
				if(culture==null) {//custom language
					row["language"]=raw.Rows[i]["patLanguage"].ToString();
				}
				else {
					row["language"]=culture.DisplayName;
				}
				row["medOrPremed[+]"]="";
				if(raw.Rows[i]["patMedUrgNote"].ToString()!="" || raw.Rows[i]["patPremed"].ToString()=="1" || raw.Rows[i]["hasDisease"].ToString()!="0" || raw.Rows[i]["hasAllergy"].ToString()!="0") {
					row["medOrPremed[+]"]="+";
				}
				row["MedUrgNote"]=raw.Rows[i]["patMedUrgNote"].ToString();
				row["Note"]=raw.Rows[i]["apptNote"].ToString();
				row["Op"]=raw.Rows[i]["apptOp"].ToString();
				if(raw.Rows[i]["apptIsNewPatient"].ToString()=="1"){
					row["patientName"]="NP-";
				}
				row["patientName"]+=PatientLogic.GetNameLF(raw.Rows[i]["patLName"].ToString(),raw.Rows[i]["patFName"].ToString(),
					raw.Rows[i]["patPreferred"].ToString(),raw.Rows[i]["patMiddleI"].ToString());
				row["patientNameF"]=raw.Rows[i]["patFName"].ToString();
				row["PatNum"]=raw.Rows[i]["apptPatNum"].ToString();
				row["patNum"]="PatNum: "+raw.Rows[i]["apptPatNum"].ToString();
				row["GuarNum"]=raw.Rows[i]["patGuarantor"].ToString();
				row["patNumAndName"]="";
				if(raw.Rows[i]["apptIsNewPatient"].ToString()=="1") {
					row["patNumAndName"]="NP-";
				}
				row["patNumAndName"]+=raw.Rows[i]["apptPatNum"].ToString()+" "
					+PatientLogic.GetNameLF(raw.Rows[i]["patLName"].ToString(),raw.Rows[i]["patFName"].ToString(),
					raw.Rows[i]["patPreferred"].ToString(),raw.Rows[i]["patMiddleI"].ToString());
				row["Pattern"]=raw.Rows[i]["apptPattern"].ToString();
				row["preMedFlag"]="";
				if(raw.Rows[i]["patPremed"].ToString()=="1") {
					row["preMedFlag"]=Lans.g("Appointments","Premedicate");
				}
				row["procs"]=raw.Rows[i]["apptProcDescript"].ToString();
				row["procsColored"]+=raw.Rows[i]["apptProcsColored"].ToString();
				production=0;
				writeoffPPO=0;
				if(rawProc!=null) {
					for(int p=0;p<rawProc.Rows.Count;p++) {
						if(isPlanned && raw.Rows[i]["apptAptNum"].ToString()!=rawProc.Rows[p]["PlannedAptNum"].ToString()) {
							continue;
						}
						else if(!isPlanned && raw.Rows[i]["apptAptNum"].ToString()!=rawProc.Rows[p]["AptNum"].ToString()) {
							continue;
						}
						production+=PIn.Decimal(rawProc.Rows[p]["ProcFee"].ToString());
						//WriteOffEst -1 and WriteOffEstOverride -1 already excluded
						//production-=
						writeoffPPO+=PIn.Decimal(rawProc.Rows[p]["writeoffPPO"].ToString());//frequently zero
						if(rawProcLab!=null) { //Will be null if not Canada.
							for(int a=0;a<rawProcLab.Rows.Count;a++) {
								if(rawProcLab.Rows[a]["ProcNumLab"].ToString()==rawProc.Rows[p]["ProcNum"].ToString()) {
									production+=PIn.Decimal(rawProcLab.Rows[a]["ProcFee"].ToString());
									writeoffPPO+=PIn.Decimal(rawProcLab.Rows[a]["writeoffPPO"].ToString());//frequently zero
								}
							}
						}
					}
				}
				row["production"]=production.ToString("c");//PIn.Double(raw.Rows[i]["Production"].ToString()).ToString("c");
				row["productionVal"]=production.ToString();//raw.Rows[i]["Production"].ToString();
				if(raw.Rows[i]["apptIsHygiene"].ToString()=="1"){
					row["provider"]=raw.Rows[i]["HygAbbr"].ToString();
					if(raw.Rows[i]["ProvAbbr"].ToString()!=""){
						row["provider"]+=" ("+raw.Rows[i]["ProvAbbr"].ToString()+")";
					}
				}
				else{
					row["provider"]=raw.Rows[i]["ProvAbbr"].ToString();
					if(raw.Rows[i]["HygAbbr"].ToString()!="") {
						row["provider"]+=" ("+raw.Rows[i]["HygAbbr"].ToString()+")";
					}
				}
				row["ProvNum"]=raw.Rows[i]["apptProvNum"].ToString();
				row["ProvHyg"]=raw.Rows[i]["apptProvHyg"].ToString();
				long apptPatNum=PIn.Long(raw.Rows[i]["apptPatNum"].ToString());
				if(dictRefFromPatNums.ContainsKey(apptPatNum)){//Add this patient's "from" referrals
					row["referralFrom"]=dictRefFromPatNums[apptPatNum];
				}
				if(dictRefToPatNums.ContainsKey(apptPatNum)){//Add this patient's "to" referrals
					row["referralTo"]=dictRefToPatNums[apptPatNum];
				}
				row["timeAskedToArrive"]="";
				timeAskedToArrive=PIn.DateT(raw.Rows[i]["apptDateTimeAskedToArrive"].ToString());
				if(timeAskedToArrive.Year>1880) {
					row["timeAskedToArrive"]=timeAskedToArrive.ToString("H:mm");
				}
				row["wirelessPhone"]=Lans.g("Appointments","Cell: ")+raw.Rows[i]["patWirelessPhone"].ToString();
				row["wkPhone"]=Lans.g("Appointments","Wk: ")+raw.Rows[i]["patWkPhone"].ToString();
				row["writeoffPPO"]=writeoffPPO.ToString();
				table.Rows.Add(row);
			}
			return table;
		}

		///<summary>Pass in the appointments table so that we can search based on appointments.</summary>
		public static DataTable GetApptFields(DataTable tableAppts) {
			//No need to check RemotingRole; no call to db.
			List<long> aptNums=new List<long>();
			for(int i=0;i<tableAppts.Rows.Count;i++) {
				aptNums.Add(PIn.Long(tableAppts.Rows[i]["AptNum"].ToString()));
			}
			return GetApptFieldsByApptNums(aptNums);
		}

		/// <summary>Only called from above method, but must be public for remoting.</summary>
		public static DataTable GetApptFieldsByApptNums(List<long> aptNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptNums);
			}
			string command="SELECT AptNum,FieldName,FieldValue "
				+"FROM apptfield "
				+"WHERE AptNum IN (";
			if(aptNums.Count==0) {
				command+="0";
			}
			else for(int i=0;i<aptNums.Count;i++) {
					if(i>0) {
						command+=",";
					}
					command+=POut.Long(aptNums[i]);
				}
			command+=")";
			DataConnection dcon=new DataConnection();
			DataTable table= dcon.GetTable(command);
			table.TableName="ApptFields";
			return table;
		}

		///<summary>Pass in the appointments table so that we can search based on appointments.</summary>
		public static DataTable GetPatFields(DataTable tableAppts) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),tableAppts);
			}
			string command="SELECT PatNum,FieldName,FieldValue "
				+"FROM patfield "
				+"WHERE PatNum IN (";
			if(tableAppts.Rows.Count==0) {
				command+="0";
			}
			else{
				for(int i=0;i<tableAppts.Rows.Count;i++) {
					if(i>0) {
						command+=",";
					}
					command+=tableAppts.Rows[i]["PatNum"].ToString();
				}
			}
			command+=")";
			DataConnection dcon=new DataConnection();
			DataTable table= dcon.GetTable(command);
			table.TableName="PatFields";
			return table;
		}

		///<summary>Pass in one aptNum</summary>
		public static DataTable GetApptFields(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptNum);
			}
			string command="SELECT ApptFieldNum,apptfielddef.FieldName,FieldValue "
				+"FROM apptfielddef "
				+"LEFT JOIN apptfield ON apptfielddef.FieldName=apptfield.FieldName "
				+"AND AptNum = "+POut.Long(aptNum)+" "
				+"ORDER BY apptfielddef.FieldName";
			DataConnection dcon=new DataConnection();
			DataTable table= dcon.GetTable(command);
			table.TableName="ApptFields";
			return table;
		}

		public static DataTable GetPeriodEmployeeSchedTable(DateTime dateStart,DateTime dateEnd) {
			return GetPeriodEmployeeSchedTable(dateStart,dateEnd,0);
		}

		///<summary>Set clinicNum to 0 to return 'unassigned' clinics.  Otherwise, filters the data set on the clinic num passed in.</summary>
		public static DataTable GetPeriodEmployeeSchedTable(DateTime dateStart,DateTime dateEnd,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,clinicNum);
			}
			DataConnection dcon=new DataConnection();
			DataTable table=new DataTable("EmpSched");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("empName");
			table.Columns.Add("schedule");
			table.Columns.Add("Note");
			if(dateStart!=dateEnd) {
				return table;
			}
			string clinicJoin="";
			string clinicWhere="";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics.
				clinicJoin="LEFT JOIN userod ON employee.EmployeeNum=userod.EmployeeNum ";
				if(clinicNum==0) {
					clinicWhere="AND (userod.ClinicNum IS NULL OR userod.ClinicNum=0) ";
				}
				else {
					clinicWhere="AND userod.ClinicNum="+POut.Long(clinicNum)+" ";
				}
			}
			string command="SELECT StartTime,StopTime,FName,employee.EmployeeNum,Note,schedule.ScheduleNum "
				+"FROM employee "
				+"INNER JOIN schedule ON schedule.EmployeeNum=employee.EmployeeNum "
				+clinicJoin
				+"WHERE SchedType="+POut.Int((int)ScheduleType.Employee)+" "
				+"AND SchedDate = "+POut.Date(dateStart)+" "
				+"AND employee.IsHidden = 0 "
				+clinicWhere;
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY schedule.ScheduleNum ";
			}
			else {
				command+="GROUP BY employee.EmployeeNum,StartTime,StopTime,FName,Note,schedule.ScheduleNum ";
			}
			command+="ORDER BY FName,StartTime";
			DataTable raw=dcon.GetTable(command);
			DateTime startTime;
			DateTime stopTime;
			for(int i=0;i<raw.Rows.Count;i++) {
				row=table.NewRow();
				if(i==0 || raw.Rows[i]["EmployeeNum"].ToString()!=raw.Rows[i-1]["EmployeeNum"].ToString()){
					row["empName"]=raw.Rows[i]["FName"].ToString();
				}
				else{
					row["empName"]="";
				}
				if(row["schedule"].ToString()!=""){
					row["schedule"]+=",";
				}
				startTime=PIn.DateT(raw.Rows[i]["StartTime"].ToString());
				stopTime=PIn.DateT(raw.Rows[i]["StopTime"].ToString());
				row["schedule"]+=startTime.ToString("h:mm")+"-"+stopTime.ToString("h:mm");
				row["Note"]=raw.Rows[i]["Note"].ToString();
				table.Rows.Add(row);
			}
			return table;
		}

		public static DataTable GetPeriodWaitingRoomTable() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			//DateTime dateStart=PIn.PDate(strDateStart);
			//DateTime dateEnd=PIn.PDate(strDateEnd);
			DataConnection dcon=new DataConnection();
			DataTable table=new DataTable("WaitingRoom");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("patName");
			table.Columns.Add("waitTime");
			table.Columns.Add("OpNum");
			string command="SELECT DateTimeArrived,DateTimeSeated,LName,FName,Preferred,"+DbHelper.Now()+" dateTimeNow,Op "
				+"FROM appointment "
				+"JOIN patient ON appointment.PatNum=patient.PatNum "
				+"WHERE "+DbHelper.DtimeToDate("AptDateTime")+" = "+POut.Date(DateTime.Now)+" "
				+"AND DateTimeArrived > "+POut.Date(DateTime.Now)+" "//midnight earlier today
				+"AND DateTimeArrived < "+DbHelper.Now()+" "
				+"AND "+DbHelper.DtimeToDate("DateTimeArrived")+"="+DbHelper.DtimeToDate("AptDateTime")+" ";//prevents people from getting "stuck" in waiting room.
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				command+="AND TO_NUMBER(TO_CHAR(DateTimeSeated,'SSSSS')) = 0 ";
			}
			else{
				command+="AND TIME(DateTimeSeated) = 0 ";
			}
			command+="AND AptStatus IN ("+POut.Int((int)ApptStatus.Complete)+","
																	 +POut.Int((int)ApptStatus.Scheduled)+","
																	 +POut.Int((int)ApptStatus.ASAP)+") "//None of the other statuses
				+"ORDER BY AptDateTime";
			DataTable raw=dcon.GetTable(command);
			TimeSpan timeArrived;
			//DateTime timeSeated;
			DateTime waitTime;
			Patient pat;
			DateTime dateTimeNow;
			//int minutes;
			for(int i=0;i<raw.Rows.Count;i++) {
				row=table.NewRow();
				pat=new Patient();
				pat.LName=raw.Rows[i]["LName"].ToString();
				pat.FName=raw.Rows[i]["FName"].ToString();
				pat.Preferred=raw.Rows[i]["Preferred"].ToString();
				row["patName"]=pat.GetNameLF();
				dateTimeNow=PIn.DateT(raw.Rows[i]["dateTimeNow"].ToString());
				timeArrived=(PIn.DateT(raw.Rows[i]["DateTimeArrived"].ToString())).TimeOfDay;
				waitTime=dateTimeNow-timeArrived;
				row["waitTime"]=waitTime.ToString("H:mm:ss");
				//minutes=waitTime.Minutes;
				//if(waitTime.Hours>0){
				//	row["waitTime"]+=waitTime.Hours.ToString()+"h ";
					//minutes-=60*waitTime.Hours;
				//}
				//row["waitTime"]+=waitTime.Minutes.ToString()+"m";
				row["OpNum"]=raw.Rows[i]["Op"].ToString();
				table.Rows.Add(row);
			}
			return table;
		}

		public static DataTable GetPeriodSchedule(DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd);
			}
			DataTable table=new DataTable("Schedule");
			table.Columns.Add("ScheduleNum");
			table.Columns.Add("SchedDate");
			table.Columns.Add("StartTime");
			table.Columns.Add("StopTime");
			table.Columns.Add("SchedType");
			table.Columns.Add("ProvNum");
			table.Columns.Add("BlockoutType");
			table.Columns.Add("Note");
			table.Columns.Add("Status");
			table.Columns.Add("ops");
			table.Columns.Add("EmployeeNum");
			table.Columns.Add("DateTStamp");
			string command="SELECT schedule.ScheduleNum,SchedDate,StartTime,StopTime,SchedType,ProvNum,BlockoutType,Note,Status,EmployeeNum,DateTStamp,"+DbHelper.GroupConcat("scheduleop.OperatoryNum")+" ops_ "
				+"FROM schedule "
				+"LEFT JOIN scheduleop ON schedule.ScheduleNum=scheduleop.ScheduleNum "
				+"WHERE SchedDate >= "+POut.Date(dateStart)+" "
				+"AND SchedDate <= "+POut.Date(dateEnd)+" ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY schedule.ScheduleNum ";
			}
			else {//Oracle
				command+="GROUP BY schedule.ScheduleNum,SchedDate,StartTime,StopTime,SchedType,ProvNum,BlockoutType,Note,Status,EmployeeNum,DateTStamp ";
			}
			command+="ORDER BY StartTime";
			DataTable raw=Db.GetTable(command);
			DataRow row;
			for(int i=0;i<raw.Rows.Count;i++) {
				row=table.NewRow();
				row["ScheduleNum"]=raw.Rows[i]["ScheduleNum"].ToString();
				row["SchedDate"]=POut.Date(PIn.Date(raw.Rows[i]["SchedDate"].ToString()),false);
				row["StartTime"]=POut.Time(PIn.Time(raw.Rows[i]["StartTime"].ToString()),false);
				row["StopTime"]=POut.Time(PIn.Time(raw.Rows[i]["StopTime"].ToString()),false);
				row["SchedType"]=raw.Rows[i]["SchedType"].ToString();
				row["ProvNum"]=raw.Rows[i]["ProvNum"].ToString();
				row["BlockoutType"]=raw.Rows[i]["BlockoutType"].ToString();
				row["Note"]=raw.Rows[i]["Note"].ToString();
				row["Status"]=raw.Rows[i]["Status"].ToString();
				row["ops"]=PIn.ByteArray(raw.Rows[i]["ops_"]);
				row["EmployeeNum"]=raw.Rows[i]["EmployeeNum"].ToString();
				row["DateTStamp"]=POut.Date(PIn.Date(raw.Rows[i]["DateTStamp"].ToString()),false);
				table.Rows.Add(row);
			}
			return table;
		}

		//Get DS for one appointment in Edit window--------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------------------

		///<summary></summary>
		public static DataSet GetApptEdit(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),aptNum);
			}
			DataSet retVal=new DataSet();
			retVal.Tables.Add(GetApptTable(aptNum));
			retVal.Tables.Add(GetPatTable(retVal.Tables["Appointment"].Rows[0]["PatNum"].ToString()));
			retVal.Tables.Add(GetProcTable(retVal.Tables["Appointment"].Rows[0]["PatNum"].ToString(),aptNum.ToString(),
				retVal.Tables["Appointment"].Rows[0]["AptStatus"].ToString(),
				retVal.Tables["Appointment"].Rows[0]["AptDateTime"].ToString()
				));
			retVal.Tables.Add(GetCommTable(retVal.Tables["Appointment"].Rows[0]["PatNum"].ToString()));
			bool isPlanned=false;
			if(retVal.Tables["Appointment"].Rows[0]["AptStatus"].ToString()=="6"){
				isPlanned=true;
			}
			retVal.Tables.Add(GetMiscTable(aptNum.ToString(),isPlanned));
			retVal.Tables.Add(GetApptFields(aptNum));
			return retVal;
		}

		public static DataTable GetApptTable(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptNum);
			}
			string command="SELECT * FROM appointment WHERE AptNum="+aptNum.ToString();
			DataTable table=Db.GetTable(command);
			table.TableName="Appointment";
			return table;
		}

		public static DataTable GetPatTable(string patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum);
			}
			DataTable table=new DataTable("Patient");
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("field");
			table.Columns.Add("value");
			string command="SELECT * FROM patient WHERE PatNum="+patNum;
			DataTable rawPat=Db.GetTable(command);
			DataRow row;
			//Patient Name--------------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Name");
			row["value"]=PatientLogic.GetNameLF(rawPat.Rows[0]["LName"].ToString(),rawPat.Rows[0]["FName"].ToString(),
				rawPat.Rows[0]["Preferred"].ToString(),rawPat.Rows[0]["MiddleI"].ToString());
			table.Rows.Add(row);
			//Patient First Name--------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","First Name");
			row["value"]=rawPat.Rows[0]["FName"];
			table.Rows.Add(row);
			//Patient Last name---------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Last Name");
			row["value"]=rawPat.Rows[0]["LName"];
			table.Rows.Add(row);
			//Patient middle initial----------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Middle Initial");
			row["value"]=rawPat.Rows[0]["MiddleI"];
			table.Rows.Add(row);
			//Patient birthdate----------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Birthdate");
			row["value"]=PIn.Date(rawPat.Rows[0]["Birthdate"].ToString()).ToShortDateString();
			table.Rows.Add(row);
			//Patient home phone--------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Home Phone");
			row["value"]=rawPat.Rows[0]["HmPhone"];
			table.Rows.Add(row);
			//Patient work phone--------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Work Phone");
			row["value"]=rawPat.Rows[0]["WkPhone"];
			table.Rows.Add(row);
			//Patient wireless phone----------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Wireless Phone");
			row["value"]=rawPat.Rows[0]["WirelessPhone"];
			table.Rows.Add(row);
			//Patient credit type-------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Credit Type");
			row["value"]=rawPat.Rows[0]["CreditType"];
			table.Rows.Add(row);
			//Patient billing type------------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Billing Type");
			row["value"]=DefC.GetName(DefCat.BillingTypes,PIn.Long(rawPat.Rows[0]["BillingType"].ToString()));
			table.Rows.Add(row);
			//Patient total balance-----------------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Total Balance");
			double totalBalance=PIn.Double(rawPat.Rows[0]["EstBalance"].ToString());
			row["value"]=totalBalance.ToString("F");
			table.Rows.Add(row);
			//Patient address and phone notes-------------------------------------------------------
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Address and Phone Notes");
			row["value"]=rawPat.Rows[0]["AddrNote"];
			table.Rows.Add(row);
			//Patient family balance----------------------------------------------------------------
			command="SELECT BalTotal,InsEst FROM patient WHERE Guarantor='"
				+rawPat.Rows[0]["Guarantor"].ToString()+"'";
			DataTable familyBalance=Db.GetTable(command);
			row=table.NewRow();
			row["field"]=Lans.g("FormApptEdit","Family Balance");
			double balance=PIn.Double(familyBalance.Rows[0]["BalTotal"].ToString())
				-PIn.Double(familyBalance.Rows[0]["InsEst"].ToString());
			row["value"]=balance.ToString("F");
			table.Rows.Add(row);
			//Site----------------------------------------------------------------------------------
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				row=table.NewRow();
				row["field"]=Lans.g("FormApptEdit","Site");
				row["value"]=Sites.GetDescription(PIn.Long(rawPat.Rows[0]["SiteNum"].ToString()));
				table.Rows.Add(row);
			}
			return table;
		}

		public static DataTable GetProcTable(string patNum,string aptNum,string apptStatus,string aptDateTime) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum,aptNum,apptStatus,aptDateTime);
			}
			DataTable table=new DataTable("Procedure");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("AbbrDesc");
			table.Columns.Add("attached");//0 or 1
			table.Columns.Add("CodeNum");
			table.Columns.Add("descript");
			table.Columns.Add("fee");
			table.Columns.Add("priority");
			table.Columns.Add("Priority");
			table.Columns.Add("ProcCode");
			table.Columns.Add("ProcDate");
			table.Columns.Add("ProcNum");
			table.Columns.Add("ProcStatus");
			table.Columns.Add("ProvNum");
			table.Columns.Add("status");
			table.Columns.Add("Surf");
			table.Columns.Add("toothNum");
			table.Columns.Add("ToothNum");
			table.Columns.Add("ToothRange");
			table.Columns.Add("TreatArea");
			//but we won't actually fill this table with rows until the very end.  It's more useful to use a List<> for now.
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT AbbrDesc,procedurecode.ProcCode,AptNum,LaymanTerm,"
				+"PlannedAptNum,Priority,ProcFee,ProcNum,ProcStatus, "
				+"procedurecode.Descript,procedurelog.CodeNum,ProcDate,procedurelog.ProvNum,Surf,ToothNum,ToothRange,TreatArea "
				+"FROM procedurelog LEFT JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+"WHERE PatNum="+patNum//sort later
			//1. All TP procs
				+" AND (ProcStatus=1 OR ";//tp
			//2. All attached procs
				//+" AND ";
			if(apptStatus=="6"){//planned
				command+="PlannedAptNum="+aptNum;
			}
			else{
				command+="AptNum="+aptNum;//exclude procs attached to other appts.
			}
			//3. All unattached completed procs with same date as appt.
			//but only if one of these types
			if(apptStatus=="1" || apptStatus=="2" || apptStatus=="4" || apptStatus=="5"){//sched,C,ASAP,broken
				DateTime aptDate=PIn.DateT(aptDateTime);
				command+=" OR (AptNum=0 "//unattached
					+"AND ProcStatus=2 "//complete
					+"AND "+DbHelper.DtimeToDate("ProcDate")+"="+POut.Date(aptDate)+")";//same date
			}
			command+=") "
				+"AND ProcStatus<>6 "//Not deleted.
				+"AND IsCanadianLab=0";
			DataTable rawProc=Db.GetTable(command);
			for(int i=0;i<rawProc.Rows.Count;i++) {
				row=table.NewRow();
				row["AbbrDesc"]=rawProc.Rows[i]["AbbrDesc"].ToString();
				if(apptStatus=="6"){//planned
					row["attached"]=(rawProc.Rows[i]["PlannedAptNum"].ToString()==aptNum) ? "1" : "0";
				}
				else{
					row["attached"]=(rawProc.Rows[i]["AptNum"].ToString()==aptNum) ? "1" : "0";
				}
				row["CodeNum"]=rawProc.Rows[i]["CodeNum"].ToString();
				row["descript"]="";
				if(apptStatus=="6") {//planned
					if(rawProc.Rows[i]["PlannedAptNum"].ToString()!="0" && rawProc.Rows[i]["PlannedAptNum"].ToString()!=aptNum) {
						row["descript"]=Lans.g("FormApptEdit","(other appt)");
					}
				}
				else {
					if(rawProc.Rows[i]["AptNum"].ToString()!="0" && rawProc.Rows[i]["AptNum"].ToString()!=aptNum) {
						row["descript"]=Lans.g("FormApptEdit","(other appt)");
					}
				}
				if(rawProc.Rows[i]["LaymanTerm"].ToString()==""){
					row["descript"]+=rawProc.Rows[i]["Descript"].ToString();
				}
				else{
					row["descript"]+=rawProc.Rows[i]["LaymanTerm"].ToString();
				}
				if(rawProc.Rows[i]["ToothRange"].ToString()!=""){
					row["descript"]+=" #"+Tooth.FormatRangeForDisplay(rawProc.Rows[i]["ToothRange"].ToString());
				}
				row["fee"]=PIn.Double(rawProc.Rows[i]["ProcFee"].ToString()).ToString("F");
				row["priority"]=DefC.GetName(DefCat.TxPriorities,PIn.Long(rawProc.Rows[i]["Priority"].ToString()));
				row["Priority"]=rawProc.Rows[i]["Priority"].ToString();
				row["ProcCode"]=rawProc.Rows[i]["ProcCode"].ToString();
				row["ProcDate"]=rawProc.Rows[i]["ProcDate"].ToString();//eg 2012-02-19
				row["ProcNum"]=rawProc.Rows[i]["ProcNum"].ToString();
				row["ProcStatus"]=rawProc.Rows[i]["ProcStatus"].ToString();
				row["ProvNum"]=rawProc.Rows[i]["ProvNum"].ToString();
				row["status"]=((ProcStat)PIn.Long(rawProc.Rows[i]["ProcStatus"].ToString())).ToString();
				row["Surf"]=rawProc.Rows[i]["Surf"].ToString();
				row["toothNum"]=Tooth.GetToothLabel(rawProc.Rows[i]["ToothNum"].ToString());
				row["ToothNum"]=rawProc.Rows[i]["ToothNum"].ToString();
				row["ToothRange"]=rawProc.Rows[i]["ToothRange"].ToString();
				row["TreatArea"]=rawProc.Rows[i]["TreatArea"].ToString();
				rows.Add(row);
			}
			//Sorting
			rows.Sort(CompareRows);
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>The supplied DataRows must include the following columns: attached,Priority,ToothRange,ToothNum,ProcCode. This sorts all objects in Chart module based on their dates, times, priority, and toothnum.  For time comparisons, procs are not included.  But if other types such as comm have a time component in ProcDate, then they will be sorted by time as well.</summary>
		public static int CompareRows(DataRow x,DataRow y) {
			//No need to check RemotingRole; no call to db.
			/*if(x["attached"].ToString()!=y["attached"].ToString()){//if one is attached and the other is not
				if(x["attached"].ToString()=="1"){
					return -1;
				}
				else{
					return 1;
				}
			}*/
			return ProcedureLogic.CompareProcedures(x,y);//sort by priority, toothnum, procCode
		}

		public static DataTable GetCommTable(string patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum);
			}
			DataTable table=new DataTable("Comm");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("commDateTime");
			table.Columns.Add("CommlogNum");
			table.Columns.Add("CommType");
			table.Columns.Add("Note");
			string command="SELECT * FROM commlog WHERE PatNum="+patNum//+" AND IsStatementSent=0 "//don't include StatementSent
				+" ORDER BY CommDateTime";
			DataTable rawComm=Db.GetTable(command);
			for(int i=0;i<rawComm.Rows.Count;i++) {
				row=table.NewRow();
				row["commDateTime"]=PIn.DateT(rawComm.Rows[i]["commDateTime"].ToString()).ToShortDateString();
				row["CommlogNum"]=rawComm.Rows[i]["CommlogNum"].ToString();
				row["CommType"]=rawComm.Rows[i]["CommType"].ToString();
				row["Note"]=rawComm.Rows[i]["Note"].ToString();
				table.Rows.Add(row);
			}
			return table;
		}

		public static DataTable GetMiscTable(string aptNum,bool isPlanned) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptNum,isPlanned);
			}
			DataTable table=new DataTable("Misc");
			DataRow row;
			table.Columns.Add("LabCaseNum");
			table.Columns.Add("labDescript");
			table.Columns.Add("requirements");
			string command="SELECT LabCaseNum,DateTimeDue,DateTimeChecked,DateTimeRecd,DateTimeSent,"
				+"laboratory.Description FROM labcase,laboratory "
				+"WHERE labcase.LaboratoryNum=laboratory.LaboratoryNum AND ";
			if(isPlanned){
				command+="labcase.PlannedAptNum="+aptNum;
			}
			else {
				command+="labcase.AptNum="+aptNum;
			}
			DataTable raw=Db.GetTable(command);
			DateTime date;
			DateTime dateDue;
			//for(int i=0;i<raw.Rows.Count;i++) {//always return one row:
			row=table.NewRow();
			row["LabCaseNum"]="0";
			row["labDescript"]="";
			if(raw.Rows.Count>0){
				row["LabCaseNum"]=raw.Rows[0]["LabCaseNum"].ToString();
				row["labDescript"]=raw.Rows[0]["Description"].ToString();
				date=PIn.DateT(raw.Rows[0]["DateTimeChecked"].ToString());
				if(date.Year>1880){
					row["labDescript"]+=", "+Lans.g("FormApptEdit","Quality Checked");
				}
				else{
					date=PIn.DateT(raw.Rows[0]["DateTimeRecd"].ToString());
					if(date.Year>1880){
						row["labDescript"]+=", "+Lans.g("FormApptEdit","Received");
					}
					else{
						date=PIn.DateT(raw.Rows[0]["DateTimeSent"].ToString());
						if(date.Year>1880){
							row["labDescript"]+=", "+Lans.g("FormApptEdit","Sent");//sent but not received
						}
						else{
							row["labDescript"]+=", "+Lans.g("FormApptEdit","Not Sent");
						}
						dateDue=PIn.DateT(raw.Rows[0]["DateTimeDue"].ToString());
						if(dateDue.Year>1880) {
							row["labDescript"]+=", "+Lans.g("FormAppEdit","Due: ")+dateDue.ToString("ddd")+" "
								+dateDue.ToShortDateString()+" "+dateDue.ToShortTimeString();
						}
					}
				}
			}
			//requirements-------------------------------------------------------------------------------------------
			command="SELECT "
				+"reqstudent.Descript,LName,FName "
				+"FROM reqstudent,provider "//schoolcourse "
				+"WHERE reqstudent.ProvNum=provider.ProvNum "
				+"AND reqstudent.AptNum="+aptNum;
			raw=Db.GetTable(command);
			row["requirements"]="";
			for(int i=0;i<raw.Rows.Count;i++){
				if(i!=0){
					row["requirements"]+="\r\n";
				}
				row["requirements"]+=raw.Rows[i]["LName"].ToString()+", "+raw.Rows[i]["FName"].ToString()
					+": "+raw.Rows[i]["Descript"].ToString();
			}
			table.Rows.Add(row);
			return table;
		}

		//private static DataRow GetRowFromTable(
		///<summary></summary>
		public static void Delete(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),aptNum);
				return;
			}
			string command;
			command="SELECT PatNum,IsNewPatient,AptStatus FROM appointment WHERE AptNum="+POut.Long(aptNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count<1){
				return;//Already deleted or did not exist.
			}
			Patient pat=Patients.GetPat(PIn.Long(table.Rows[0]["PatNum"].ToString()));
			if(table.Rows[0]["IsNewPatient"].ToString()=="1") {
				Procedures.SetDateFirstVisit(DateTime.MinValue,3,pat);
			}
			//procs
			if(table.Rows[0]["AptStatus"].ToString()=="6") {//planned
				command="UPDATE procedurelog SET PlannedAptNum =0 WHERE PlannedAptNum = "+POut.Long(aptNum);
			}
			else {
				command="UPDATE procedurelog SET AptNum =0 WHERE AptNum = "+POut.Long(aptNum);
			}
			Db.NonQ(command);
			//labcases
			if(table.Rows[0]["AptStatus"].ToString()=="6") {//planned
				command="UPDATE labcase SET PlannedAptNum =0 WHERE PlannedAptNum = "+POut.Long(aptNum);
			}
			else {
				command="UPDATE labcase SET AptNum =0 WHERE AptNum = "+POut.Long(aptNum);
			}
			Db.NonQ(command);
			//plannedappt
			command="DELETE FROM plannedappt WHERE AptNum="+POut.Long(aptNum);
			Db.NonQ(command);
			//we will not reset item orders here
			command="DELETE FROM appointment WHERE AptNum = "+POut.Long(aptNum);
			Db.NonQ(command);
			//apptfield
			command="DELETE FROM apptfield WHERE AptNum = "+POut.Long(aptNum);
			Db.NonQ(command);
			DeletedObjects.SetDeleted(DeletedObjectType.Appointment,aptNum);
		}

		/// <summary>If make5minute is false, then result will be in 10 or 15 minutes blocks and will need a later conversion step before going to db.</summary>
		public static string CalculatePattern(long provDent,long provHyg,List<long> codeNums,bool make5minute) {
			StringBuilder strBTime=new StringBuilder("");
			string procTime="";
			ProcedureCode procCode;
			if(codeNums.Count==1) {
				procCode=ProcedureCodes.GetProcCode(codeNums[0]);
				if(procCode.IsHygiene) {//hygiene proc
					procTime=ProcCodeNotes.GetTimePattern(provHyg,codeNums[0]);
				}
				else {//dentist proc
					procTime=ProcCodeNotes.GetTimePattern(provDent,codeNums[0]);
				}
				strBTime.Append(procTime);
			}
			else {//multiple procs or no procs
				for(int i=0;i<codeNums.Count;i++) {
					procCode=ProcedureCodes.GetProcCode(codeNums[i]);
					if(procCode.IsHygiene) {//hygiene proc
						procTime=ProcCodeNotes.GetTimePattern(provHyg,codeNums[i]);
					}
					else {//dentist proc
						procTime=ProcCodeNotes.GetTimePattern(provDent,codeNums[i]);
					}
					if(procTime.Length<2) {
						continue;
					}
					for(int n=1;n<procTime.Length-1;n++) {
						if(procTime.Substring(n,1)=="/") {
							strBTime.Append("/");
						}
						else {
							strBTime.Insert(0,"X");
						}
					}
				}
			}
			if(codeNums.Count>1) {//multiple procs
				strBTime.Insert(0,"/");
				strBTime.Append("/");
			}
			else if(codeNums.Count==0) {//0 procs
				strBTime.Append("/");
			}
			if(strBTime.Length>39) {
				strBTime.Remove(39,strBTime.Length-39);
			}
			Plugins.HookAddCode(null,"Appointments.CalculatePattern_end",strBTime,provDent,provHyg,codeNums);
			string pattern=strBTime.ToString();
			if(make5minute) {
				return ConvertPatternTo5(pattern);
			}
			return pattern;
		}

		public static string ConvertPatternTo5(string pattern) {
			StringBuilder savePattern=new StringBuilder();
			for(int i=0;i<pattern.Length;i++) {
				savePattern.Append(pattern.Substring(i,1));
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10) {
					savePattern.Append(pattern.Substring(i,1));
				}
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15) {
					savePattern.Append(pattern.Substring(i,1));
					savePattern.Append(pattern.Substring(i,1));
				}
			}
			if(savePattern.Length==0) {
				savePattern=new StringBuilder("/");
			}
			return savePattern.ToString();
		}

		///<summary>Only called from the mobile server, not from any workstation.  Pass in an apptViewNum of 0 for now.  We might use that parameter later.</summary>
		public static string GetMobileBitmap(DateTime date,long apptViewNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),date,apptViewNum);
			}
			//For testing pass a resource image.
			return POut.Bitmap(Properties.Resources.ApptBackTest,ImageFormat.Gif);
		}

		///<summary>Returns a list of appointments that are scheduled between start date and end date. This takes into account the length of the appointments.</summary>
		public static List<Appointment> GetAppointmentsForPeriod(DateTime start,DateTime end) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),start,end);
			}
			string command="SELECT * FROM appointment WHERE ";
			command+="AptDateTime >= "+POut.DateT(start.Date);//to check to see if an appointment scheduled beforehand overlaps this time segment
			command+="AND AptDateTime <= "+POut.DateT(end);
			List<Appointment> retVal = Crud.AppointmentCrud.TableToList(Db.GetTable(command));
			for(int i=retVal.Count-1;i>=0;i--) {
				if(retVal[i].AptDateTime.AddMinutes(retVal[i].Pattern.Length*PrefC.GetInt(PrefName.AppointmentTimeIncrement))>start) {
					retVal.RemoveAt(i);
				}
			}
			return retVal;
		}

		///<summary>Gets all appointments scheduled in the operatories passed in that fall within the start and end dates.
		///Does not currently consider the time portion of the DateTimes passed in.</summary>
		public static List<Appointment> GetAppointmentsForOpsByPeriod(List<long> opNums,DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Appointment>>(MethodBase.GetCurrentMethod(),opNums,dateStart,dateEnd);
			}
			string command="SELECT * FROM appointment WHERE Op > 0 ";
			if(opNums!=null && opNums.Count > 0) {
				command+="AND Op IN("+String.Join(",",opNums)+") ";
			}
			command+="AND "+DbHelper.DtimeToDate("AptDateTime")+">="+POut.Date(dateStart)+" "
				+"AND "+DbHelper.DtimeToDate("AptDateTime")+"<="+POut.Date(dateEnd)+" "
				+"ORDER BY AptDateTime,Op";//Ordering by AptDateTime then Op is important for speed when checking for collisions in Web Sched.
			return Crud.AppointmentCrud.SelectMany(command);
		}

		///<summary>Gets the ProvNum for the last completed or scheduled appointment for a patient. If none, returns 0.</summary>
		public static long GetProvNumFromLastApptForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT ProvNum FROM appointment WHERE AptStatus IN ("+(int)ApptStatus.Complete+","+(int)ApptStatus.Scheduled+")"
				+" AND AptDateTime<="+POut.DateT(System.DateTime.Now)
				+" AND PatNum="+POut.Long(patNum)
				+" ORDER BY AptDateTime DESC LIMIT 1";
			string result=Db.GetScalar(command);
			if(String.IsNullOrWhiteSpace(result)) {
				return 0;
			}
			return PIn.Long(result);
		}

		///<summary>Uses the input parameters to construct a List&lt;ApptSearchProviderSchedule&gt;. It is written to reduce the number of queries to the database.</summary>
		/// <param name="listProvNums">PrimaryKeys to Provider.</param>
		/// <param name="dateScheduleStart">The date that will start looking for provider schedule information.</param>
		/// <param name="dateScheduleStop">The date that will stop looking for provider schedule information.</param>
		/// <param name="listSchedules">A List of Schedules containing all of the schedules for the given day, or possibly more. 
		/// Intended to be all schedules between search start date and search start date plus 2 years. This is to reduce queries to DB.</param>
		/// <param name="listAppointments">A List of Appointments containing all of the schedules for the given day, or possibly more. 
		/// Intended to be all Appointments between search start date and search start date plus 2 years. This is to reduce queries to DB.</param>
		public static Dictionary<DateTime,List<ApptSearchProviderSchedule>> GetApptSearchProviderScheduleForProvidersAndDate(List<long> listProvNums,DateTime dateScheduleStart,DateTime dateScheduleStop,List<Schedule> listSchedules,List<Appointment> listAppointments) {//Not working properly when scheduled but no ops are set.
			//No need to check RemotingRole; no call to db.
			Dictionary<DateTime,List<ApptSearchProviderSchedule>> dictProviderSchedulesByDate=new Dictionary<DateTime,List<ApptSearchProviderSchedule>>();
			List<ApptSearchProviderSchedule> listProviderSchedules=new List<ApptSearchProviderSchedule>();
			if(dateScheduleStart.Date<=dateScheduleStop.Date) {
				listProviderSchedules=GetApptSearchProviderScheduleForProvidersAndDate(listProvNums,dateScheduleStart,listSchedules,listAppointments);
				dictProviderSchedulesByDate.Add(dateScheduleStart.Date,listProviderSchedules);
				return dictProviderSchedulesByDate;
			}
			//Loop through all the days between the start and stop date and return the ApptSearchProviderSchedule's for all days.
			for(int i=0;i<(dateScheduleStop-dateScheduleStart).Days;i++) {
				listProviderSchedules=GetApptSearchProviderScheduleForProvidersAndDate(listProvNums,dateScheduleStart.AddDays(i),listSchedules,listAppointments);
				if(dictProviderSchedulesByDate.ContainsKey(dateScheduleStart.AddDays(i))) {//Just in case.
					dictProviderSchedulesByDate[dateScheduleStart.AddDays(i)]=listProviderSchedules;
				}
				else {
					dictProviderSchedulesByDate.Add(dateScheduleStart.AddDays(i),listProviderSchedules);
				}
			}
			return dictProviderSchedulesByDate;
		}

		///<summary>Uses the input parameters to construct a List&lt;ApptSearchProviderSchedule&gt;. It is written to reduce the number of queries to the database.</summary>
		/// <param name="ProviderNums">PrimaryKeys to Provider.</param>
		/// <param name="ScheduleDate">The date to construct the schedule for.</param>
		/// <param name="ScheduleList">A List of Schedules containing all of the schedules for the given day, or possibly more. 
		/// Intended to be all schedules between search start date and search start date plus 2 years. This is to reduce queries to DB.</param>
		/// <param name="AppointmentList">A List of Appointments containing all of the schedules for the given day, or possibly more. 
		/// Intended to be all Appointments between search start date and search start date plus 2 years. This is to reduce queries to DB.</param>
		public static List<ApptSearchProviderSchedule> GetApptSearchProviderScheduleForProvidersAndDate(List<long> ProviderNums,DateTime ScheduleDate,List<Schedule> ScheduleList,List<Appointment> AppointmentList) {//Not working properly when scheduled but no ops are set.
			//No need to check RemotingRole; no call to db.
			List<ApptSearchProviderSchedule> retVal=new List<ApptSearchProviderSchedule>();
			ScheduleDate=ScheduleDate.Date;
			for(int i=0;i<ProviderNums.Count;i++) {
				retVal.Add(new ApptSearchProviderSchedule());
				retVal[i].ProviderNum=ProviderNums[i];
				retVal[i].SchedDate=ScheduleDate;
			}
			for(int s=0;s<ScheduleList.Count;s++) {
				if(ScheduleList[s].SchedDate.Date!=ScheduleDate) {//ignore schedules for different dates.
					continue;
				}
				if(ProviderNums.Contains(ScheduleList[s].ProvNum)) {//schedule applies to one of the selected providers
					int indexOfProvider = ProviderNums.IndexOf(ScheduleList[s].ProvNum);//cache the provider index
					int scheduleStartBlock = (int)ScheduleList[s].StartTime.TotalMinutes/5;//cache the start time of the schedule
					int scheduleLengthInBlocks = (int)(ScheduleList[s].StopTime-ScheduleList[s].StartTime).TotalMinutes/5;//cache the length of the schedule
					for(int i=0;i<scheduleLengthInBlocks;i++) {
						retVal[indexOfProvider].ProvBar[scheduleStartBlock+i]=true;//provider may have an appointment here
						retVal[indexOfProvider].ProvSchedule[scheduleStartBlock+i]=true;//provider is scheduled today
					}
				}
			}
			for(int a=0;a<AppointmentList.Count;a++) {
				if(AppointmentList[a].AptDateTime.Date!=ScheduleDate) {
					continue;
				}
				if(!AppointmentList[a].IsHygiene && ProviderNums.Contains(AppointmentList[a].ProvNum)) {//Not hygiene Modify provider bar based on ProvNum
					int indexOfProvider = ProviderNums.IndexOf(AppointmentList[a].ProvNum);
					int appointmentCurStartBlock = (int)AppointmentList[a].AptDateTime.TimeOfDay.TotalMinutes/5;
					for(int i=0;i<AppointmentList[a].Pattern.Length;i++) {
						if(AppointmentList[a].Pattern[i]=='X') {
							retVal[indexOfProvider].ProvBar[appointmentCurStartBlock+i]=false;
						}
					}
				}
				else if(AppointmentList[a].IsHygiene && ProviderNums.Contains(AppointmentList[a].ProvHyg)) {//Modify provider bar based on ProvHyg
					int indexOfProvider = ProviderNums.IndexOf(AppointmentList[a].ProvHyg);
					int appointmentStartBlock = (int)AppointmentList[a].AptDateTime.TimeOfDay.TotalMinutes/5;
					for(int i=0;i<AppointmentList[a].Pattern.Length;i++) {
						if(AppointmentList[a].Pattern[i]=='X') {
							retVal[indexOfProvider].ProvBar[appointmentStartBlock+i]=false;
						}
					}
				}
			}
			return retVal;
		}

		///<summary>Returns true if the patient has any broken appointments, future appointments, unscheduled appointments, or unsched planned appointments.  This adds intelligence when user attempts to schedule an appointment by only showing the appointments for the patient when needed rather than always.</summary>
		public static bool HasPlannedEtc(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT COUNT(*) FROM appointment "
				+"WHERE PatNum='"+POut.Long(patNum)+"' "
				+"AND (AptStatus='"+POut.Long((int)ApptStatus.Broken)+"' "
				+"OR AptStatus='"+POut.Long((int)ApptStatus.UnschedList)+"' "
				+"OR (AptStatus='"+POut.Long((int)ApptStatus.Scheduled)+"' AND AptDateTime > "+DbHelper.Curdate()+" ) "//future scheduled
				//planned appts that are already scheduled will also show because they are caught on the line above rather then on the next line
				+"OR (AptStatus='"+POut.Long((int)ApptStatus.Planned)+"' "//planned, not sched
				+"AND NOT EXISTS(SELECT * FROM appointment a2 WHERE a2.PatNum='"+POut.Long(patNum)+"' AND a2.NextAptNum=appointment.AptNum)) "
				+")";
			if(Db.GetScalar(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Returns a dictionary containing the last completed appointment date of each patient.</summary>
		public static Dictionary<long,DateTime> GetDateLastVisit() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<long,DateTime>>(MethodBase.GetCurrentMethod());
			}
			Dictionary<long,DateTime> retVal=new Dictionary<long,DateTime>();
			string command="SELECT PatNum,MAX(AptDateTime) DateLastAppt "
					+"FROM appointment "
					+"WHERE "+DbHelper.DtimeToDate("AptDateTime")+"<="+DbHelper.Curdate()+" "
					+"GROUP BY PatNum";
			DataTable tableLastVisit=Db.GetTable(command);
			for(int i=0;i<tableLastVisit.Rows.Count;i++) {
				long patNum=PIn.Long(tableLastVisit.Rows[i]["PatNum"].ToString());
				DateTime dateLastAppt=PIn.DateT(tableLastVisit.Rows[i]["DateLastAppt"].ToString());
				retVal.Add(patNum,dateLastAppt);
			}
			return retVal;
		}

		///<summary>Returns a dictionary containing all information of every scheduled, completed, and ASAP appointment made from all non-deleted patients.  Usually used for bridges.</summary>
		public static Dictionary<long,List<Appointment>> GetAptsForPats(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<long,List<Appointment>>>(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			Dictionary<long,List<Appointment>> retVal=new Dictionary<long,List<Appointment>>();
			string command="SELECT * "
					+"FROM appointment "
					+"WHERE AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.Complete)+","+POut.Int((int)ApptStatus.ASAP)+") "
					+"AND "+DbHelper.DtimeToDate("AptDateTime")+">="+POut.Date(dateFrom)+" AND "+DbHelper.DtimeToDate("AptDateTime")+"<="+POut.Date(dateTo);
			List<Appointment> listApts=Crud.AppointmentCrud.SelectMany(command);
			for(int i=0;i<listApts.Count;i++) {
				if(retVal.ContainsKey(listApts[i].PatNum)) {
					retVal[listApts[i].PatNum].Add(listApts[i]);//Add the current appointment to the list of appointments for the patient.
				}
				else {
					retVal.Add(listApts[i].PatNum,new List<Appointment> { listApts[i] });//Initialize the list of appointments for the current patient and include the current appoinment.
				}
			}
			return retVal;
		}

		/// <summary>Get a dictionary of all procedure codes for all scheduled, ASAP, and completed appointments</summary>
		public static Dictionary<long,List<long>> GetCodeNumsAllApts() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Dictionary<long,List<long>>>(MethodBase.GetCurrentMethod());
			}
			Dictionary<long,List<long>> retVal=new Dictionary<long,List<long>>();
			string command="SELECT appointment.AptNum,procedurelog.CodeNum "
				+"FROM appointment "
				+"LEFT JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				long aptNum=PIn.Long(table.Rows[i]["AptNum"].ToString());
				long codeNum=PIn.Long(table.Rows[i]["CodeNum"].ToString());
				if(retVal.ContainsKey(aptNum)) {
					retVal[aptNum].Add(codeNum);//Add the current CodeNum to the list of CodeNums for the appointment.
				}
				else {
					retVal.Add(aptNum,new List<long> { codeNum });//Initialize the list of CodeNums for the current appointment and include the current CodeNum.
				}
			}
			return retVal;
		}

		///<summary>Fills an appointment passed in with all appropriate procedures for the recall passed in.  It's up to the calling class to then place the appointment on the pinboard or schedule.  The appointment will be inserted into the database in this method so it's important to delete it if the appointment doesn't get scheduled.  Returns the list of procedures that were created for the appointment so that they can be displayed to Orion users.</summary>
		public static List<Procedure> FillAppointmentForRecall(Appointment aptCur,Recall recallCur,List<Recall> listRecalls,Patient patCur,List<string> listProcStrs,List<InsPlan> listPlans,List<InsSub> listSubs) {
			//No need to check RemotingRole; no call to db.
			aptCur.PatNum=patCur.PatNum;
			aptCur.AptStatus=ApptStatus.UnschedList;//In all places where this is used, the unsched status with no aptDateTime will cause the appt to be deleted when the pinboard is cleared.
			if(patCur.PriProv==0) {
				aptCur.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			}
			else {
				aptCur.ProvNum=patCur.PriProv;
			}
			aptCur.ProvHyg=patCur.SecProv;
			if(aptCur.ProvHyg!=0) {
				aptCur.IsHygiene=true;
			}
			aptCur.ClinicNum=patCur.ClinicNum;
			//whether perio or prophy:
			string recallPattern=RecallTypes.GetTimePattern(recallCur.RecallTypeNum);
			if(RecallTypes.IsSpecialRecallType(recallCur.RecallTypeNum)
				&& patCur.Birthdate.AddYears(PrefC.GetInt(PrefName.RecallAgeAdult)) > ((recallCur.DateDue>DateTime.Today)?recallCur.DateDue:DateTime.Today)) //For example, if pt's 12th birthday falls after recall date.
			{
				List<RecallType> listRecallTypes=RecallTypeC.GetListt();
				for(int i=0;i<listRecallTypes.Count;i++) {
					if(listRecallTypes[i].RecallTypeNum==RecallTypes.ChildProphyType) {
						List<string> childprocs=RecallTypes.GetProcs(listRecallTypes[i].RecallTypeNum);
						if(childprocs.Count>0) {
							listProcStrs=childprocs;//overrides adult procs.
						}
						string childpattern=RecallTypes.GetTimePattern(listRecallTypes[i].RecallTypeNum);
						if(childpattern!="") {
							recallPattern=childpattern;//overrides adult pattern.
						}
					}
				}
			}
			aptCur.Pattern=RecallTypes.ConvertTimePattern(recallPattern);
			//Add films------------------------------------------------------------------------------------------------------
			if(RecallTypes.IsSpecialRecallType(recallCur.RecallTypeNum)) {//if this is a prophy or perio
				for(int i=0;i<listRecalls.Count;i++) {
					if(recallCur.RecallNum==listRecalls[i].RecallNum) {
						continue;//already handled.
					}
					if(listRecalls[i].IsDisabled) {
						continue;
					}
					if(listRecalls[i].DateDue.Year<1880) {
						continue;
					}
					if(listRecalls[i].DateDue>recallCur.DateDue//if film due date is after prophy due date
						&& listRecalls[i].DateDue>DateTime.Today)//and not overdue
					{
						continue;
					}
					//incomplete: exclude manual recall types
					listProcStrs.AddRange(RecallTypes.GetProcs(listRecalls[i].RecallTypeNum));
				}
			}
			aptCur.ProcDescript="";
			aptCur.ProcsColored="";
			for(int i=0;i<listProcStrs.Count;i++) {
				string procDescOne="";
				if(i>0) {
					aptCur.ProcDescript+=", ";
				}
				procDescOne+=ProcedureCodes.GetProcCode(listProcStrs[i]).AbbrDesc;
				aptCur.ProcDescript+=procDescOne;
				//Color and previous date are determined by ProcApptColor object
				ProcApptColor pac=ProcApptColors.GetMatch(listProcStrs[i]);
				System.Drawing.Color pColor=System.Drawing.Color.Black;
				string prevDateString="";
				if(pac!=null) {
					pColor=pac.ColorText;
					if(pac.ShowPreviousDate) {
						prevDateString=Procedures.GetRecentProcDateString(aptCur.PatNum,aptCur.AptDateTime,pac.CodeRange);
						if(prevDateString!="") {
							prevDateString=" ("+prevDateString+")";
						}
					}
				}
				aptCur.ProcsColored+="<span color=\""+pColor.ToArgb().ToString()+"\">"+procDescOne+prevDateString+"</span>";
			}
			aptCur.TimeLocked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			Appointments.Insert(aptCur);
			Procedure procCur;
			List<PatPlan> listPatPlans=PatPlans.Refresh(patCur.PatNum);
			List<Benefit> listBenifits=Benefits.Refresh(listPatPlans,listSubs);
			InsPlan priplan=null;
			InsSub prisub=null;
			if(listPatPlans.Count>0) {
				prisub=InsSubs.GetSub(listPatPlans[0].InsSubNum,listSubs);
				priplan=InsPlans.GetPlan(prisub.PlanNum,listPlans);
			}
			double insfee;
			double standardfee;
			List<Procedure> listProcs=new List<Procedure>();
			for(int i=0;i<listProcStrs.Count;i++) {
				procCur=new Procedure();//this will be an insert
				//procnum
				procCur.PatNum=patCur.PatNum;
				procCur.AptNum=aptCur.AptNum;
				procCur.CodeNum=ProcedureCodes.GetCodeNum(listProcStrs[i]);
				procCur.ProcDate=DateTime.Now;
				procCur.DateTP=DateTime.Now;
				//Check if it's a medical procedure.
				bool isMed = false;
				procCur.MedicalCode=ProcedureCodes.GetProcCode(procCur.CodeNum).MedicalCode;
				if(procCur.MedicalCode != null && procCur.MedicalCode != "") {
					isMed = true;
				}
				//Get fee schedule for medical or dental.
				long feeSch;
				if(isMed) {
					feeSch=Fees.GetMedFeeSched(patCur,listPlans,listPatPlans,listSubs);
				}
				else {
					feeSch=Fees.GetFeeSched(patCur,listPlans,listPatPlans,listSubs);
				}
				procCur.ProvNum=patCur.PriProv;
				//Procedures.Cur.Dx=
				procCur.ClinicNum=patCur.ClinicNum;
				//Get the fee amount for medical or dental.
				if(PrefC.GetBool(PrefName.MedicalFeeUsedForNewProcs) && isMed) {
					insfee=Fees.GetAmount0(ProcedureCodes.GetProcCode(procCur.MedicalCode).CodeNum,feeSch,procCur.ClinicNum,procCur.ProvNum);
				}
				else {
					insfee=Fees.GetAmount0(procCur.CodeNum,feeSch,procCur.ClinicNum,procCur.ProvNum);
				}
				if(priplan!=null && priplan.PlanType=="p") {//PPO
					standardfee=Fees.GetAmount0(procCur.CodeNum,Providers.GetProv(Patients.GetProvNum(patCur)).FeeSched,procCur.ClinicNum,procCur.ProvNum);
					if(standardfee>insfee) {
						procCur.ProcFee=standardfee;
					}
					else {
						procCur.ProcFee=insfee;
					}
				}
				else {
					procCur.ProcFee=insfee;
				}
				//surf
				//toothnum
				//Procedures.Cur.ToothRange="";
				//ProcCur.NoBillIns=ProcedureCodes.GetProcCode(ProcCur.CodeNum).NoBillIns;
				//priority
				procCur.ProcStatus=ProcStat.TP;
				procCur.Note="";
				//Procedures.Cur.PriEstim=
				//Procedures.Cur.SecEstim=
				//claimnum
				//nextaptnum
				procCur.BaseUnits = ProcedureCodes.GetProcCode(procCur.CodeNum).BaseUnits;
				procCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
				Procedures.Insert(procCur);//no recall synch required
				Procedures.ComputeEstimates(procCur,patCur.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBenifits,patCur.Age,listSubs);
				listProcs.Add(procCur);
			}
			return listProcs;
		}

	}

	///<summary>Holds information about a provider's Schedule. Not actual database table.</summary>
	public class ApptSearchProviderSchedule {
		///<summary>FK to Provider</summary>
		public long ProviderNum;
		///<summary>Date of the ProviderSchedule.</summary>
		public DateTime SchedDate;
		///<summary>This contains a bool for each 5 minute block throughout the day. True means provider is scheduled to work, False means provider is not scheduled to work.</summary>
		public bool[] ProvSchedule;
		///<summary>This contains a bool for each 5 minute block throughout the day. True means available, False means something is scheduled there or the provider is not scheduled to work.</summary>
		public bool[] ProvBar;

		///<summary>Constructor.</summary>
		public ApptSearchProviderSchedule() {
			ProvSchedule=new bool[288];
			ProvBar=new bool[288];
		}
	}

	///<summary>Holds information about a operatory's Schedule. Not actual database table.</summary>
	public class ApptSearchOperatorySchedule {
		///<summary>FK to Operatory</summary>
		public long OperatoryNum;
		///<summary>Date of the OperatorySchedule.</summary>
		public DateTime SchedDate;
		///<summary>This contains a bool for each 5 minute block throughout the day. True means operatory is open, False means operatory is in use.</summary>
		public bool[] OperatorySched;
		///<summary>List of providers 'allowed' to work in this operatory.</summary>
		public List<long> ProviderNums;

	}
}
