using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenDentBusiness{
	
	///<summary></summary>
	public class Recalls {
		private const string WEB_SCHED_SIGN_UP_URL="http://www.patientviewer.com/WebSchedSignUp.html";

		///<summary>http://www.patientviewer.com/WebSchedSignUp.html</summary>
		public static string GetWebSchedPromoURL() {
			//No need to check RemotingRole; no call to db.
			return WEB_SCHED_SIGN_UP_URL;
		}

		///<summary>Gets all recalls for the supplied patients, usually a family or single pat.  Result might have a length of zero.  Each recall will also have the DateScheduled filled by pulling that info from other tables.</summary>
		public static List<Recall> GetList(List<long> patNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Recall>>(MethodBase.GetCurrentMethod(),patNums);
			} 
			string wherePats="";
			for(int i=0;i<patNums.Count;i++){
				if(i!=0){
					wherePats+=" OR ";
				}
				wherePats+="PatNum="+patNums[i].ToString();
			}
			string command="SELECT * FROM recall WHERE "+wherePats;
			return Crud.RecallCrud.SelectMany(command);
		}

		public static List<Recall> GetList(long patNum) {
			//No need to check RemotingRole; no call to db.
			List<long> patNums=new List<long>();
			patNums.Add(patNum);
			return GetList(patNums);
		}

		/// <summary></summary>
		public static List<Recall> GetList(List<Patient> patients){
			//No need to check RemotingRole; no call to db.
			List<long> patNums=new List<long>();
			for(int i=0;i<patients.Count;i++){
				patNums.Add(patients[i].PatNum);
			}
			return GetList(patNums);
		}

		public static Recall GetRecall(long recallNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Recall>(MethodBase.GetCurrentMethod(),recallNum);
			}
			return Crud.RecallCrud.SelectOne(recallNum);
		}

		///<summary>Will return a recall or null.</summary>
		public static Recall GetRecallProphyOrPerio(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Recall>(MethodBase.GetCurrentMethod(),patNum);
			} 
			string command="SELECT * FROM recall WHERE PatNum="+POut.Long(patNum)
				+" AND (RecallTypeNum="+RecallTypes.ProphyType+" OR RecallTypeNum="+RecallTypes.PerioType+")";
			return Crud.RecallCrud.SelectOne(command);
		}

		public static List<Recall> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Recall>>(MethodBase.GetCurrentMethod(),changedSince);
			} 
			string command="SELECT * FROM recall WHERE DateTStamp > "+POut.DateT(changedSince);
			return Crud.RecallCrud.SelectMany(command);
		}

		///<summary>Only used in FormRecallList to get a list of patients with recall.  Supply a date range, using min and max values if user left blank.  If provNum=0, then it will get all provnums.  It looks for both provider match in either PriProv or SecProv.</summary>
		public static DataTable GetRecallList(DateTime fromDate,DateTime toDate,bool groupByFamilies,long provNum,long clinicNum,
			long siteNum,RecallListSort sortBy,RecallListShowNumberReminders showReminders,List<long> excludePatNums)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),fromDate,toDate,groupByFamilies,provNum,clinicNum,siteNum,sortBy,showReminders,excludePatNums);
			}
			DataTable table=new DataTable();
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("age");
			table.Columns.Add("billingType");
			table.Columns.Add("contactMethod");//text representation for display
			table.Columns.Add("ClinicNum");
			table.Columns.Add("dateLastReminder");
			table.Columns.Add("DateDue",typeof(DateTime));
			table.Columns.Add("dueDate");//blank if minVal
			table.Columns.Add("Email");
			table.Columns.Add("FName");
			table.Columns.Add("Guarantor");
			table.Columns.Add("guarFName");
			table.Columns.Add("guarLName");
			table.Columns.Add("LName");
			table.Columns.Add("maxDateDue",typeof(DateTime));
			table.Columns.Add("Note");
			table.Columns.Add("numberOfReminders");
			table.Columns.Add("patientName");
			table.Columns.Add("PatNum");
			table.Columns.Add("PreferRecallMethod");
			table.Columns.Add("recallInterval");
			table.Columns.Add("RecallNum");
			table.Columns.Add("recallType");
			table.Columns.Add("status");
			List<DataRow> rows=new List<DataRow>();
			string command;
			command=@"SELECT patguar.BalTotal,patient.BillingType,patient.Birthdate,recall.DateDue,patient.ClinicNum,MAX(CommDateTime) ""_dateLastReminder"",
				DisableUntilBalance,DisableUntilDate,
				patient.Email,patguar.Email ""_guarEmail"",patguar.FName ""_guarFName"",
				patguar.LName ""_guarLName"",patient.FName,
				patient.Guarantor,patient.HmPhone,patguar.InsEst,patient.LName,recall.Note,
				COUNT(commlog.CommlogNum) ""_numberOfReminders"",
				recall.PatNum,patient.PreferRecallMethod,patient.Preferred,
				recall.RecallInterval,recall.RecallNum,recall.RecallStatus,
				recalltype.Description ""_recalltype"",patient.WirelessPhone,patient.WkPhone
				FROM recall
				LEFT JOIN patient ON recall.PatNum=patient.PatNum
				LEFT JOIN patient patguar ON patient.Guarantor=patguar.PatNum
				LEFT JOIN recalltype ON recall.RecallTypeNum=recalltype.RecallTypeNum
				LEFT JOIN commlog ON commlog.PatNum=recall.PatNum
				AND CommType="+POut.Long(Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL))+" "
				+"AND CommDateTime > recall.DatePrevious "
				//We need to make commlog more restrictive for situations where a manually added recall has no date previous,
				+"WHERE patient.patstatus=0 ";
			if(provNum>0){
				command+="AND (patient.PriProv="+POut.Long(provNum)+" "
					+"OR patient.SecProv="+POut.Long(provNum)+") ";
			}
			if(clinicNum>0) {
				command+="AND patient.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			if(siteNum>0) {
				command+="AND patient.SiteNum="+POut.Long(siteNum)+" ";
			}
			command+="AND recall.DateDue >= "+POut.Date(fromDate)+" "
				+"AND recall.DateDue <= "+POut.Date(toDate)+" "
				+"AND recall.IsDisabled = 0 ";
			if(PrefC.GetString(PrefName.RecallTypesShowingInList)!="") {
				command+="AND recall.RecallTypeNum IN("+PrefC.GetString(PrefName.RecallTypesShowingInList)+") ";
			}
			if(PrefC.GetBool(PrefName.RecallExcludeIfAnyFutureAppt)) {
				string datesql="CURDATE()";
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					datesql="(SELECT CURRENT_DATE FROM dual)";
				}
				command+=@"AND NOT EXISTS(SELECT * FROM appointment WHERE
					appointment.PatNum=recall.PatNum AND appointment.AptDateTime > "+datesql//early this morning
					+" AND appointment.AptStatus IN(1,4)) ";//scheduled,ASAP
			}
			else{
				command+="AND recall.DateScheduled='0001-01-01' "; //Only show rows where no future recall appointment.
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY recall.PatNum,recall.RecallTypeNum ";//GROUP BY RecallTypeNum forces both manual and prophy types to show independently.
			}
			else {
				command+=@"GROUP BY  patguar.BalTotal,patient.BillingType,
					patient.Birthdate,recall.DateDue,
					DisableUntilBalance,DisableUntilDate,
					patient.Email,patguar.Email,patguar.FName,
					patguar.LName,patient.FName,
					patient.Guarantor,patient.HmPhone,patguar.InsEst,patient.LName,recall.Note,
					recall.PatNum,patient.PreferRecallMethod,patient.Preferred,
					recall.RecallInterval,recall.RecallNum,recall.RecallStatus,
					recalltype.Description,patient.WirelessPhone,patient.WkPhone,recall.RecallTypeNum ";
			}
 			DataTable rawtable=Db.GetTable(command);
			DateTime dateDue;
			DateTime dateRemind;
			Interval interv;
			Patient pat;
			ContactMethod contmeth;
			int numberOfReminders;
			int maxNumberReminders=(int)PrefC.GetLong(PrefName.RecallMaxNumberReminders);
			long patNum;
			DateTime disableUntilDate;
			double disableUntilBalance;
			double familyBalance;
			for(int i=0;i<rawtable.Rows.Count;i++){
				patNum=PIn.Long(rawtable.Rows[i]["PatNum"].ToString());
				dateDue=PIn.Date(rawtable.Rows[i]["DateDue"].ToString());
				dateRemind=PIn.Date(rawtable.Rows[i]["_dateLastReminder"].ToString());
				numberOfReminders=PIn.Int(rawtable.Rows[i]["_numberOfReminders"].ToString());
				if(numberOfReminders==0) {
					//always show
				}
				else if(numberOfReminders==1) {
					if(PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder)==-1) {
						continue;
					}
					if(dateRemind.AddDays(PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder)).Date > DateTime.Today){ //> toDate
						//|| dateRemind.AddDays(PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder)) < fromDate)
						continue;
					}
				}
				else{//2 or more reminders
					if(PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder)==-1) {
						continue;
					}
					if(dateRemind.AddDays(PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder)).Date > DateTime.Today){ //> toDate
						//|| dateRemind.AddDays(PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder)) < fromDate)
						continue;
					}
				}
				if(maxNumberReminders != -1 && numberOfReminders > maxNumberReminders) {
					continue;
				}
				if(showReminders==RecallListShowNumberReminders.Zero) {
					if(numberOfReminders != 0) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.One) {
					if(numberOfReminders != 1) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.Two) {
					if(numberOfReminders != 2) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.Three) {
					if(numberOfReminders != 3) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.Four) {
					if(numberOfReminders != 4) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.Five) {
					if(numberOfReminders != 5) {
						continue;
					}
				}
				else if(showReminders==RecallListShowNumberReminders.SixPlus) {
					if(numberOfReminders < 6 ) {
						continue;
					}
				}
				if(excludePatNums.Contains(patNum)){
					continue;
				}
				disableUntilDate=PIn.Date(rawtable.Rows[i]["DisableUntilDate"].ToString());
				if(disableUntilDate.Year>1880 && disableUntilDate > DateTime.Today) {
					continue;
				}
				disableUntilBalance=PIn.Double(rawtable.Rows[i]["DisableUntilBalance"].ToString());
				if(disableUntilBalance>0) {
					familyBalance=PIn.Double(rawtable.Rows[i]["BalTotal"].ToString());
					if(!PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {//typical
						familyBalance-=PIn.Double(rawtable.Rows[i]["InsEst"].ToString());
					}
					if(familyBalance > disableUntilBalance) {
						continue;
					}
				}
				row=table.NewRow();
				row["age"]=Patients.DateToAge(PIn.Date(rawtable.Rows[i]["Birthdate"].ToString())).ToString();//we don't care about m/y.
				row["billingType"]=DefC.GetName(DefCat.BillingTypes,PIn.Long(rawtable.Rows[i]["BillingType"].ToString()));
				row["ClinicNum"]=PIn.Long(rawtable.Rows[i]["ClinicNum"].ToString());
				contmeth=(ContactMethod)PIn.Long(rawtable.Rows[i]["PreferRecallMethod"].ToString());
				if(contmeth==ContactMethod.None){
					if(!PrefC.GetBool(PrefName.RecallUseEmailIfHasEmailAddress)){//if user only wants to use email if contact method is email (it isn't for this patient)
						row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
					}
					else{//if user wants to use email if there is an email address
						if(groupByFamilies){
							if(rawtable.Rows[i]["_guarEmail"].ToString() != "") {//since there is an email,
								row["contactMethod"]=rawtable.Rows[i]["_guarEmail"].ToString();
							}
							else{
								row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
							}
						}
						else{
							if(rawtable.Rows[i]["Email"].ToString() != "") {//since there is an email,
								row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
							}
							else{
								row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
							}
						}
					}
				}
				if(contmeth==ContactMethod.HmPhone){
					row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
				}
				if(contmeth==ContactMethod.WkPhone) {
					row["contactMethod"]=Lans.g("FormRecallList","Wk:")+rawtable.Rows[i]["WkPhone"].ToString();
				}
				if(contmeth==ContactMethod.WirelessPh) {
					row["contactMethod"]=Lans.g("FormRecallList","Cell:")+rawtable.Rows[i]["WirelessPhone"].ToString();
				}
				if(contmeth==ContactMethod.Email) {
					if(groupByFamilies) {
						//always use guarantor email
						row["contactMethod"]=rawtable.Rows[i]["_guarEmail"].ToString();
					}
					else {
						row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
					}
				}
				if(contmeth==ContactMethod.Mail) {
					row["contactMethod"]=Lans.g("FormRecallList","Mail");
				}
				if(contmeth==ContactMethod.DoNotCall || contmeth==ContactMethod.SeeNotes) {
					row["contactMethod"]=Lans.g("enumContactMethod",contmeth.ToString());
				}
				if(dateRemind.Year<1880) {
					row["dateLastReminder"]="";
				}
				else {
					row["dateLastReminder"]=dateRemind.ToShortDateString();
				}
				row["DateDue"]=dateDue;
				if(dateDue.Year<1880) {
					row["dueDate"]="";
				}
				else {
					row["dueDate"]=dateDue.ToShortDateString();
				}
				if(groupByFamilies) {
					row["Email"]=rawtable.Rows[i]["_guarEmail"].ToString();
				}
				else {
					row["Email"]=rawtable.Rows[i]["Email"].ToString();
				}
				row["FName"]=rawtable.Rows[i]["FName"].ToString();
				row["Guarantor"]=rawtable.Rows[i]["Guarantor"].ToString();
				row["guarFName"]=rawtable.Rows[i]["_guarFName"].ToString();
				row["guarLName"]=rawtable.Rows[i]["_guarLName"].ToString();
				row["LName"]=rawtable.Rows[i]["LName"].ToString();
				row["maxDateDue"]=DateTime.MinValue;//we'll set the actual value in a subsequent loop
				row["Note"]=rawtable.Rows[i]["Note"].ToString();
				if(numberOfReminders==0) {
					row["numberOfReminders"]="";
				}
				else {
					row["numberOfReminders"]=numberOfReminders.ToString();
				}
				pat=new Patient();
				pat.LName=rawtable.Rows[i]["LName"].ToString();
				pat.FName=rawtable.Rows[i]["FName"].ToString();
				pat.Preferred=rawtable.Rows[i]["Preferred"].ToString();
				row["patientName"]=pat.GetNameLF();
				row["PatNum"]=rawtable.Rows[i]["PatNum"].ToString();
				/*if(contmeth==ContactMethod.None){
					if(groupByFamilies) {
						if(rawtable.Rows[i]["_guarEmail"].ToString() != "") {//since there is an email,
							row["PreferRecallMethod"]=((int)ContactMethod.Email).ToString();
						}
						else {
							row["PreferRecallMethod"]=((int)ContactMethod.None).ToString();
						}
					}
					else {
						if(rawtable.Rows[i]["Email"].ToString() != "") {//since there is an email,
							row["PreferRecallMethod"]=((int)ContactMethod.Email).ToString();
						}
						else {
							row["PreferRecallMethod"]=((int)ContactMethod.None).ToString();
						}
					}
				}
				else{*/
				row["PreferRecallMethod"]=rawtable.Rows[i]["PreferRecallMethod"].ToString();
				//}
				interv=new Interval(PIn.Int(rawtable.Rows[i]["RecallInterval"].ToString()));
				row["recallInterval"]=interv.ToString();
				row["RecallNum"]=rawtable.Rows[i]["RecallNum"].ToString();
				row["recallType"]=rawtable.Rows[i]["_recalltype"].ToString();
				row["status"]=DefC.GetName(DefCat.RecallUnschedStatus,PIn.Long(rawtable.Rows[i]["RecallStatus"].ToString()));
				rows.Add(row);
			}
			//Now that we have eliminated some rows, this next section calculates the maxDateDue date for each family
			//key=guarantor, value=maxDateDue
			Dictionary<long,DateTime> dictMaxDateDue=new Dictionary<long,DateTime>();
			long guarNum;
			for(int i=0;i<rows.Count;i++) {
				guarNum=PIn.Long(rows[i]["Guarantor"].ToString());
				dateDue=(DateTime)rows[i]["DateDue"];
				if(dictMaxDateDue.ContainsKey(guarNum)) {
					if(dateDue > dictMaxDateDue[guarNum]) {
						dictMaxDateDue[guarNum]=dateDue;
					}
				}
				else {//no decision necessary
					dictMaxDateDue.Add(guarNum,dateDue);
				}
			}
			for(int i=0;i<rows.Count;i++) {
				guarNum=PIn.Long(rows[i]["Guarantor"].ToString());
				if(dictMaxDateDue.ContainsKey(guarNum)) {
					rows[i]["maxDateDue"]=dictMaxDateDue[guarNum];
				}
				else {
					rows[i]["maxDateDue"]=DateTime.MinValue;//should never happen
				}
			}
			RecallComparer comparer=new RecallComparer();
			comparer.GroupByFamilies=groupByFamilies;
			comparer.SortBy=sortBy;
			rows.Sort(comparer);
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary></summary>
		public static long Insert(Recall recall) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				recall.RecallNum=Meth.GetLong(MethodBase.GetCurrentMethod(),recall);
				return recall.RecallNum;
			}
			return Crud.RecallCrud.Insert(recall);
		}

		///<summary></summary>
		public static void Update(Recall recall) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),recall);
				return;
			}
			Crud.RecallCrud.Update(recall);
		}

		///<summary></summary>
		public static void Delete(Recall recall) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),recall);
				return;
			}
			string command= "DELETE from recall WHERE RecallNum = "+POut.Long(recall.RecallNum);
			Db.NonQ(command);
			DeletedObjects.SetDeleted(DeletedObjectType.RecallPatNum,recall.PatNum);
		}

		///<summary>Synchronizes all recalls for one patient. If datePrevious has changed, then it completely deletes the old status and note information and sets a new DatePrevious and dateDueCalc.  Also updates dateDue to match dateDueCalc if not disabled.  Creates any recalls as necessary.  Recalls will never get automatically deleted except when all triggers are removed.  Otherwise, the dateDueCalc just gets cleared.</summary>
		public static void Synch(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			List<RecallType> typeListActive=RecallTypes.GetActive();
			List<RecallType> typeList=new List<RecallType>(typeListActive);
			string command="SELECT * FROM recall WHERE PatNum="+POut.Long(patNum);
			List<Recall> recallList=Crud.RecallCrud.SelectMany(command);
			//determine if this patient is a perio patient.
			bool isPerio=false;
			for(int i=0;i<recallList.Count;i++){
				if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==recallList[i].RecallTypeNum){
					isPerio=true;
					break;
				}
			}
			//remove types from the list which do not apply to this patient.
			for(int i=0;i<typeList.Count;i++){//it's ok to not go backwards because we immediately break.
				if(isPerio) {
					if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==typeList[i].RecallTypeNum) {
						typeList.RemoveAt(i);
						break;
					}
				}
				else {
					if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==typeList[i].RecallTypeNum) {
						typeList.RemoveAt(i);
						break;
					}
				}
			}
			//get previous dates for all types at once.
			//Because of the inner join, this will not include recall types with no trigger.
			command="SELECT RecallTypeNum,MAX(ProcDate) procDate_ "
				+"FROM procedurelog,recalltrigger "
				+"WHERE PatNum="+POut.Long(patNum)
				+" AND procedurelog.CodeNum=recalltrigger.CodeNum "
				+"AND (";
			if(typeListActive.Count>0) {//This will include both prophy and perio, regardless of whether this is a prophy or perio patient.
				for(int i=0;i<typeListActive.Count;i++) {
					if(i>0) {
						command+=" OR";
					}
					command+=" RecallTypeNum="+POut.Long(typeListActive[i].RecallTypeNum);
				}
			} 
			else {
				command+=" RecallTypeNum=0";//Effectively forces an empty result set, without changing the returned table structure.
			}
			command+=") AND (ProcStatus = "+POut.Long((int)ProcStat.C)+" "
				+"OR ProcStatus = "+POut.Long((int)ProcStat.EC)+" "
				+"OR ProcStatus = "+POut.Long((int)ProcStat.EO)+") "
				+"GROUP BY RecallTypeNum";
			DataTable tableDates=Db.GetTable(command);
			//Go through the type list and either update recalls, or create new recalls.
			//Recalls that are no longer active because their type has no triggers will be ignored.
			//It is assumed that there are no duplicate recall types for a patient.
			DateTime prevDate;
			Recall matchingRecall;
			Recall recallNew;
			DateTime prevDateProphy=DateTime.MinValue;
			DateTime dateProphyTesting;
			for(int i=0;i<typeListActive.Count;i++) {
				if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)!=typeListActive[i].RecallTypeNum
					&& PrefC.GetLong(PrefName.RecallTypeSpecialPerio)!=typeListActive[i].RecallTypeNum) 
				{
					//we are only working with prophy and perio in this loop.
					continue;
				}
				for(int d=0;d<tableDates.Rows.Count;d++) {//procs for patient
					if(tableDates.Rows[d]["RecallTypeNum"].ToString()==typeListActive[i].RecallTypeNum.ToString()) {
						dateProphyTesting=PIn.Date(tableDates.Rows[d]["procDate_"].ToString());
						//but patient could have both perio and prophy.
						//So must test to see if the date is newer
						if(dateProphyTesting>prevDateProphy) {
							prevDateProphy=dateProphyTesting;
						}
						break;
					}
				}
			}
			for(int i=0;i<typeList.Count;i++){//active types for this patient.
				if(RecallTriggers.GetForType(typeList[i].RecallTypeNum).Count==0) {
					//if no triggers for this recall type, then skip it.  Don't try to add or alter.
					continue;
				}
				//set prevDate:
				if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==typeList[i].RecallTypeNum
					|| PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==typeList[i].RecallTypeNum) 
				{
					prevDate=prevDateProphy;
				}
				else {
					prevDate=DateTime.MinValue;
					for(int d=0;d<tableDates.Rows.Count;d++) {//procs for patient
						if(tableDates.Rows[d]["RecallTypeNum"].ToString()==typeList[i].RecallTypeNum.ToString()) {
							prevDate=PIn.Date(tableDates.Rows[d]["procDate_"].ToString());
							break;
						}
					}
				}
				matchingRecall=null;
				for(int r=0;r<recallList.Count;r++){//recalls for patient
					if(recallList[r].RecallTypeNum==typeList[i].RecallTypeNum){
						matchingRecall=recallList[r];
					}
				}
				if(matchingRecall==null){//if there is no existing recall,
					if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==typeList[i].RecallTypeNum
						|| PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==typeList[i].RecallTypeNum
						|| prevDate.Year>1880)//for other types, if date is not minVal, then add a recall
					{
						//add a recall
						recallNew=new Recall();
						recallNew.RecallTypeNum=typeList[i].RecallTypeNum;
						recallNew.PatNum=patNum;
						recallNew.DatePrevious=prevDate;//will be min val for prophy/perio with no previous procs
						recallNew.RecallInterval=typeList[i].DefaultInterval;
						if(prevDate.Year<1880) {
							recallNew.DateDueCalc=DateTime.MinValue;
						}
						else {
							recallNew.DateDueCalc=prevDate+recallNew.RecallInterval;
						}
						recallNew.DateDue=recallNew.DateDueCalc;
						Recalls.Insert(recallNew);
					}
				}
				else{//alter the existing recall
					if(!matchingRecall.IsDisabled
						&& matchingRecall.DisableUntilBalance==0
						&& matchingRecall.DisableUntilDate.Year<1880
						&& prevDate.Year>1880//this protects recalls that were manually added as part of a conversion
						&& prevDate != matchingRecall.DatePrevious) 
					{//if datePrevious has changed, reset
						matchingRecall.RecallStatus=0;
						matchingRecall.Note="";
						matchingRecall.DateDue=matchingRecall.DateDueCalc;//now it is allowed to be changed in the steps below
					}
					if(prevDate.Year<1880){//if no previous date
						matchingRecall.DatePrevious=DateTime.MinValue;
						if(matchingRecall.DateDue==matchingRecall.DateDueCalc){//user did not enter a DateDue
							matchingRecall.DateDue=DateTime.MinValue;
						}
						matchingRecall.DateDueCalc=DateTime.MinValue;
						Recalls.Update(matchingRecall);
					}
					else{//if previous date is a valid date
						matchingRecall.DatePrevious=prevDate;
						if(matchingRecall.IsDisabled){//if the existing recall is disabled 
							matchingRecall.DateDue=DateTime.MinValue;//DateDue is always blank
						}
						else{//but if not disabled
							if(matchingRecall.DateDue==matchingRecall.DateDueCalc//if user did not enter a DateDue
								|| matchingRecall.DateDue.Year<1880)//or DateDue was blank
							{
								matchingRecall.DateDue=matchingRecall.DatePrevious+matchingRecall.RecallInterval;//set same as DateDueCalc
							}
						}
						matchingRecall.DateDueCalc=matchingRecall.DatePrevious+matchingRecall.RecallInterval;
						Recalls.Update(matchingRecall);
					}
				}
			}
			//now, we need to loop through all the inactive recall types and clear the DateDueCalc
			//We don't do this anymore. User must explicitly delete recalls, either one-by-one, or from the recall type window.
			/*
			List<RecallType> typeListInactive=RecallTypes.GetInactive();
			for(int i=0;i<typeListInactive.Count;i++){
				matchingRecall=null;
				for(int r=0;r<recallList.Count;r++){
					if(recallList[r].RecallTypeNum==typeListInactive[i].RecallTypeNum){
						matchingRecall=recallList[r];
					}
				}
				if(matchingRecall==null){//if there is no existing recall,
					continue;
				}
				Recalls.Delete(matchingRecall);//we'll just delete it
				//There is an existing recall, so alter it if certain conditions are met
				//matchingRecall.DatePrevious=DateTime.MinValue;
				//if(matchingRecall.DateDue==matchingRecall.DateDueCalc){//if user did not enter a DateDue
					//we can safely alter the DateDue
				//	matchingRecall.DateDue=DateTime.MinValue;
				//}
				//matchingRecall.DateDueCalc=DateTime.MinValue;
				//Recalls.Update(matchingRecall);
			}*/
		}

		/// <summary>Synchronizes DateScheduled column in recall table for one patient.  This must be used instead of lazy synch in RecallsForPatient, when deleting an appointment, when sending to unscheduled list, setting an appointment complete, etc.  This is fast, but it would be inefficient to call it too much.</summary>
		public static void SynchScheduledApptFull(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			//Clear out DateScheduled column for this pat before changing
			string command="UPDATE recall "
				+"SET recall.DateScheduled="+POut.Date(DateTime.MinValue)+" "
				+"WHERE recall.PatNum="+POut.Long(patNum);
			Db.NonQ(command);
			//Get table of future appointments dates with recall type for this patient, where a procedure is attached that is a recall trigger procedure
			command="SELECT recalltrigger.RecallTypeNum,MIN("+DbHelper.DtimeToDate("appointment.AptDateTime")+") AS AptDateTime "
				+"FROM procedurelog "
				+"INNER JOIN recalltrigger ON procedurelog.CodeNum=recalltrigger.CodeNum "
				+"INNER JOIN recall ON recalltrigger.RecallTypeNum=recall.RecallTypeNum "
					+"AND recall.PatNum="+POut.Long(patNum)+" "
				+"INNER JOIN appointment ON appointment.AptNum=procedurelog.AptNum "
					+"AND appointment.PatNum="+POut.Long(patNum)+" "
					+"AND (appointment.AptStatus="+POut.Int((int)ApptStatus.Scheduled)+" OR appointment.AptStatus="+POut.Int((int)ApptStatus.ASAP)+") "
					+"AND appointment.AptDateTime > "+DbHelper.Curdate()+" "//early this morning
				+"WHERE procedurelog.PatNum="+POut.Long(patNum)+" "
				+"GROUP BY recalltrigger.RecallTypeNum";
			DataTable table=Db.GetTable(command);
			//Update the recalls for this patient with DATE(AptDateTime) where there is a future appointment with recall proc on it
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["RecallTypeNum"].ToString()=="") {
					continue;
				}
				command=@"UPDATE recall	SET recall.DateScheduled="+POut.Date(PIn.Date(table.Rows[i]["AptDateTime"].ToString()))+" " 
					+"WHERE recall.RecallTypeNum="+POut.Long(PIn.Long(table.Rows[i]["RecallTypeNum"].ToString()))+" "
					+"AND recall.PatNum="+POut.Long(patNum)+" ";
				Db.NonQ(command);
			}
		}

		///<summary>Updates RecallInterval and DueDate for all patients that have the recallTypeNum and defaultIntervalOld to use the defaultIntervalNew.</summary>
		public static void UpdateDefaultIntervalForPatients(long recallTypeNum,Interval defaultIntervalOld,Interval defaultIntervalNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),recallTypeNum,defaultIntervalOld,defaultIntervalNew);
				return;
			}
			string command="SELECT * FROM recall WHERE IsDisabled=0 AND RecallTypeNum="+POut.Long(recallTypeNum)+" AND RecallInterval="+POut.Int(defaultIntervalOld.ToInt());
			List<Recall> recallList=Crud.RecallCrud.SelectMany(command);
			for(int i=0;i<recallList.Count;i++) {
				if(recallList[i].DateDue!=recallList[i].DateDueCalc) {//User entered a DueDate.
					//Don't change the DateDue since user already overrode it
				}
				else{
					recallList[i].DateDue=recallList[i].DatePrevious+defaultIntervalNew;
				}
				recallList[i].DateDueCalc=recallList[i].DatePrevious+defaultIntervalNew;
				recallList[i].RecallInterval=defaultIntervalNew;
				Update(recallList[i]);
			}
		}

		public static void DeleteAllOfType(long recallTypeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),recallTypeNum);
				return;
			}
			string command="DELETE FROM recall WHERE RecallTypeNum= "+POut.Long(recallTypeNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void SynchAllPatients(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//get all active patients
			string command="SELECT PatNum "
				+"FROM patient "
				+"WHERE PatStatus=0";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				Synch(PIn.Long(table.Rows[i][0].ToString()));
			}
			//get all active patients with future scheduled appointments that have a procedure attached which is a recall trigger procedure
			command="SELECT DISTINCT patient.PatNum "
						+"FROM patient "
						+"INNER JOIN appointment ON appointment.PatNum=patient.PatNum AND AptDateTime>CURDATE() AND AptStatus IN (1,4,5) "//Scheduled,ASAP, or Broken
						//Broken is only included to fix a bug that existed between versions 12.4 and 13.2.  It clears out the datesched
						//if broken future appt is the only appt with a recall trigger on it so the patient will be on the recall list again.
						+"INNER JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum "
						+"INNER JOIN recalltrigger ON recalltrigger.CodeNum=procedurelog.CodeNum "
						+"WHERE PatStatus=0";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				SynchScheduledApptFull(PIn.Long(table.Rows[i][0].ToString()));
			}
		}

		/// <summary></summary>
		public static DataTable GetAddrTable(List<long> recallNums,bool groupByFamily,RecallListSort sortBy) {
			//No need to check RemotingRole; no call to db.
			DataTable rawTable=GetAddrTableRaw(recallNums);
			List<DataRow> rawRows=new List<DataRow>();
			for(int i=0;i<rawTable.Rows.Count;i++){
				rawRows.Add(rawTable.Rows[i]);
			}
			RecallComparer comparer=new RecallComparer();
			comparer.GroupByFamilies=groupByFamily;
			comparer.SortBy=sortBy;
			rawRows.Sort(comparer);
			DataTable table=new DataTable();
			table.Columns.Add("address");//includes address2. Can be guar.
			table.Columns.Add("City");//Can be guar.
			table.Columns.Add("clinicNum");//will be the guar clinicNum if grouped.
			table.Columns.Add("dateDue");
			table.Columns.Add("email");//Will be guar if grouped by family
			table.Columns.Add("emailPatNum");//Will be guar if grouped by family
			table.Columns.Add("famList");
			table.Columns.Add("guarLName");
			table.Columns.Add("numberOfReminders");//for a family, this will be the max for the family
			table.Columns.Add("patientNameF");//Only used when single email
			table.Columns.Add("patientNameFL");
			table.Columns.Add("patNums");//Comma delimited.  Used in email.
			table.Columns.Add("recallNums");//Comma delimited.  Used during e-mail and eCards
			table.Columns.Add("State");//Can be guar.
			table.Columns.Add("Zip");//Can be guar.
			string familyAptList="";
			string recallNumStr="";
			string patNumStr="";
			DataRow row;
			List<DataRow> rows=new List<DataRow>();
			int maxNumReminders=0;
			int maxRemindersThisPat;
			Patient pat;
			for(int i=0;i<rawRows.Count;i++) {
				if(!groupByFamily) {
					row=table.NewRow();
					row["address"]=rawRows[i]["Address"].ToString();
					if(rawRows[i]["Address2"].ToString()!="") {
						row["address"]+="\r\n"+rawRows[i]["Address2"].ToString();
					}
					row["City"]=rawRows[i]["City"].ToString();
					row["clinicNum"]=rawRows[i]["ClinicNum"].ToString();
					row["dateDue"]=PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
					//since not grouping by family, this is always just the patient email
					row["email"]=rawRows[i]["Email"].ToString();
					row["emailPatNum"]=rawRows[i]["PatNum"].ToString();
					row["famList"]="";
					row["guarLName"]=rawRows[i]["guarLName"].ToString();//even though we won't use it.
					row["numberOfReminders"]=PIn.Long(rawRows[i]["numberOfReminders"].ToString()).ToString();
					pat=new Patient();
					pat.LName=rawRows[i]["LName"].ToString();
					pat.FName=rawRows[i]["FName"].ToString();
					pat.Preferred=rawRows[i]["Preferred"].ToString();
					row["patientNameF"]=pat.GetNameFirstOrPreferred();
					row["patientNameFL"]=pat.GetNameFLnoPref();// GetNameFirstOrPrefL();
					row["patNums"]=rawRows[i]["PatNum"].ToString();
					row["recallNums"]=rawRows[i]["RecallNum"].ToString();
					row["State"]=rawRows[i]["State"].ToString();
					row["Zip"]=rawRows[i]["Zip"].ToString();
					rows.Add(row);
					continue;
				}
				//groupByFamily----------------------------------------------------------------------
				if(familyAptList==""){//if this is the first patient in the family
					maxNumReminders=0;
					//loop through the whole family, and determine the maximum number of reminders
					for(int f=i;f<rawRows.Count;f++) {
						maxRemindersThisPat=PIn.Int(rawRows[f]["numberOfReminders"].ToString());
						if(maxRemindersThisPat>maxNumReminders) {
							maxNumReminders=maxRemindersThisPat;
						}
						if(f==rawRows.Count-1//if this is the last row
							|| rawRows[i]["Guarantor"].ToString()!=rawRows[f+1]["Guarantor"].ToString())//or if the guarantor on next line is different
						{
							break;
						}
					}
					//now we know the max number of reminders for the family
					if(i==rawRows.Count-1//if this is the last row
						|| rawRows[i]["Guarantor"].ToString()!=rawRows[i+1]["Guarantor"].ToString())//or if the guarantor on next line is different
					{
						//then this is a single patient, and there are no other family members in the list.
						row=table.NewRow();
						row["address"]=rawRows[i]["Address"].ToString();
						if(rawRows[i]["Address2"].ToString()!="") {
							row["address"]+="\r\n"+rawRows[i]["Address2"].ToString();
						}
						row["City"]=rawRows[i]["City"].ToString();
						row["State"]=rawRows[i]["State"].ToString();
						row["Zip"]=rawRows[i]["Zip"].ToString();
						row["clinicNum"]=rawRows[i]["ClinicNum"].ToString();
						row["dateDue"]=PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
						//this will always be the guarantor email
						row["email"]=rawRows[i]["guarEmail"].ToString();
						row["emailPatNum"]=rawRows[i]["Guarantor"].ToString();
						row["famList"]="";
						row["guarLName"]=rawRows[i]["guarLName"].ToString();//even though we won't use it.
						row["numberOfReminders"]=maxNumReminders.ToString();
						//if(rawRows[i]["Preferred"].ToString()=="") {
						row["patientNameF"]=rawRows[i]["FName"].ToString();
						//}
						//else {
						//	row["patientNameF"]=rawRows[i]["Preferred"].ToString();
						//}
						row["patientNameFL"]=rawRows[i]["FName"].ToString()+" "
							+rawRows[i]["MiddleI"].ToString()+" "
							+rawRows[i]["LName"].ToString();
						row["patNums"]=rawRows[i]["PatNum"].ToString();
						row["recallNums"]=rawRows[i]["RecallNum"].ToString();
						rows.Add(row);
						continue;
					}
					else{//this is the first patient of a family with multiple family members
						familyAptList=rawRows[i]["FName"].ToString()+":  "
							+PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
						patNumStr=rawRows[i]["PatNum"].ToString();
						recallNumStr=rawRows[i]["RecallNum"].ToString();
						continue;
					}
				}
				else{//not the first patient
					familyAptList+="\r\n"+rawRows[i]["FName"].ToString()+":  "
						+PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
					patNumStr+=","+rawRows[i]["PatNum"].ToString();
					recallNumStr+=","+rawRows[i]["RecallNum"].ToString();
				}
				if(i==rawRows.Count-1//if this is the last row
					|| rawRows[i]["Guarantor"].ToString()!=rawRows[i+1]["Guarantor"].ToString())//or if the guarantor on next line is different
				{
					//This part only happens for the last family member of a grouped family
					row=table.NewRow();
					row["address"]=rawRows[i]["guarAddress"].ToString();
					if(rawRows[i]["guarAddress2"].ToString()!="") {
						row["address"]+="\r\n"+rawRows[i]["guarAddress2"].ToString();
					}
					row["City"]=rawRows[i]["guarCity"].ToString();
					row["State"]=rawRows[i]["guarState"].ToString();
					row["Zip"]=rawRows[i]["guarZip"].ToString();
					row["clinicNum"]=rawRows[i]["guarClinicNum"].ToString();
					row["dateDue"]=PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
					row["email"]=rawRows[i]["guarEmail"].ToString();
					row["emailPatNum"]=rawRows[i]["Guarantor"].ToString();
					row["famList"]=familyAptList;
					row["guarLName"]=rawRows[i]["guarLName"].ToString();
					row["numberOfReminders"]=maxNumReminders.ToString();
					row["patientNameF"]="";//not used here
					row["patientNameFL"]="";//we won't use this
					row["patNums"]=patNumStr;
					row["recallNums"]=recallNumStr;
					rows.Add(row);
					familyAptList="";
				}	
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary></summary>
		public static DataTable GetAddrTableForWebSched(List<long> recallNums,bool groupByFamily,RecallListSort sortBy) {
			//No need to check RemotingRole; no call to db.
			DataTable rawTable=GetAddrTableRaw(recallNums);
			List<DataRow> rawRows=new List<DataRow>();
			for(int i=0;i<rawTable.Rows.Count;i++) {
				rawRows.Add(rawTable.Rows[i]);
			}
			RecallComparer comparer=new RecallComparer();
			comparer.GroupByFamilies=groupByFamily;
			comparer.SortBy=sortBy;
			rawRows.Sort(comparer);
			DataTable table=new DataTable();
			table.Columns.Add("clinicNum");//will be the guar clinicNum if grouped.
			table.Columns.Add("dateDue");
			table.Columns.Add("email");//will be guar if grouped by family
			table.Columns.Add("emailPatNum");//will be guar if grouped by family
			table.Columns.Add("numberOfReminders");//for a family, this will be the max for the family
			table.Columns.Add("patientNameF");
			table.Columns.Add("patientNameFL");
			table.Columns.Add("PatNum");
			table.Columns.Add("RecallNum");
			DataRow row;
			List<DataRow> rows=new List<DataRow>();
			Patient pat;
			for(int i=0;i<rawRows.Count;i++) {
				row=table.NewRow();
				if(groupByFamily) {
					//Use guarantors clinic and email for all notifications.
					row["clinicNum"]=rawRows[i]["guarClinicNum"].ToString();
					row["email"]=rawRows[i]["guarEmail"].ToString();
					row["emailPatNum"]=rawRows[i]["Guarantor"].ToString();
				}
				else {
					row["clinicNum"]=rawRows[i]["ClinicNum"].ToString();
					row["email"]=rawRows[i]["Email"].ToString();
					row["emailPatNum"]=rawRows[i]["PatNum"].ToString();
				}
				row["dateDue"]=PIn.Date(rawRows[i]["DateDue"].ToString()).ToShortDateString();
				row["numberOfReminders"]=PIn.Long(rawRows[i]["numberOfReminders"].ToString()).ToString();
				row["PatNum"]=rawRows[i]["PatNum"].ToString();
				pat=new Patient();
				pat.LName=rawRows[i]["LName"].ToString();
				pat.FName=rawRows[i]["FName"].ToString();
				pat.Preferred=rawRows[i]["Preferred"].ToString();
				row["patientNameF"]=pat.GetNameFirstOrPreferred();
				row["patientNameFL"]=pat.GetNameFLnoPref();
				row["RecallNum"]=rawRows[i]["RecallNum"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Gets a base table used for creating </summary>
		public static DataTable GetAddrTableRaw(List<long> recallNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),recallNums);
			}
			//get maxDateDue for each family.
			string command=@"DROP TABLE IF EXISTS temprecallmaxdate;
				CREATE table temprecallmaxdate(
					Guarantor bigint NOT NULL,
					MaxDateDue date NOT NULL,
					PRIMARY KEY (Guarantor)
				);
				INSERT INTO temprecallmaxdate 
				SELECT patient.Guarantor,MAX(recall.DateDue) maxDateDue
				FROM patient
				LEFT JOIN recall ON patient.PatNum=recall.PatNum
				AND (";
			for(int i=0;i<recallNums.Count;i++) {
				if(i>0) {
					command+=" OR ";
				}
				command+="recall.RecallNum="+POut.Long(recallNums[i]);
			}
			command+=") GROUP BY patient.Guarantor";
			Db.NonQ(command);
			command=@"SELECT patient.Address,patguar.Address guarAddress,
				patient.Address2,patguar.Address2 guarAddress2,
				patient.City,patguar.City guarCity,patient.ClinicNum,patguar.ClinicNum guarClinicNum,
				recall.DateDue,patient.Email,patguar.Email guarEmail,
				patient.FName,patguar.FName guarFName,patient.Guarantor,
				patient.LName,patguar.LName guarLName,temprecallmaxdate.MaxDateDue maxDateDue,
				patient.MiddleI,
				COUNT(commlog.CommlogNum) numberOfReminders,
				patient.PatNum,patient.Preferred,recall.RecallNum,
				patient.State,patguar.State guarState,patient.Zip,patguar.Zip guarZip
				FROM recall 
				LEFT JOIN patient ON patient.PatNum=recall.PatNum 
				LEFT JOIN patient patguar ON patient.Guarantor=patguar.PatNum
				LEFT JOIN commlog ON commlog.PatNum=recall.PatNum
				AND CommType="+POut.Long(Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL))+" "
				//+"AND SentOrReceived = "+POut.Long((int)CommSentOrReceived.Sent)+" "
				+"AND CommDateTime > recall.DatePrevious "
				+"LEFT JOIN temprecallmaxdate ON temprecallmaxdate.Guarantor=patient.Guarantor "
				+"WHERE ";
			for(int i=0;i<recallNums.Count;i++) {
				if(i>0) {
					command+=" OR ";
				}
				command+="recall.RecallNum="+POut.Long(recallNums[i]);
			}
			command+=@" GROUP BY patient.Address,patguar.Address,
				patient.Address2,patguar.Address2,
				patient.City,patguar.City,patient.ClinicNum,patguar.ClinicNum,
				recall.DateDue,patient.Email,patguar.Email,
				patient.FName,patguar.FName,patient.Guarantor,
				patient.LName,patguar.LName,temprecallmaxdate.MaxDateDue,
				patient.MiddleI,patient.PatNum,patient.Preferred,recall.RecallNum,
				patient.State,patguar.State,patient.Zip,patguar.Zip";
			DataTable rawTable=Db.GetTable(command);
			command="DROP TABLE IF EXISTS temprecallmaxdate";
			Db.NonQ(command);
			return rawTable;
		}

		/// <summary></summary>
		public static void UpdateStatus(long recallNum,long newStatus) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),recallNum,newStatus);
				return;
			}
			string command="UPDATE recall SET RecallStatus="+newStatus.ToString()
				+" WHERE RecallNum="+recallNum.ToString();
			Db.NonQ(command);
		}

		public static int GetCountForType(long recallTypeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),recallTypeNum);
			}
			string command="SELECT COUNT(*) FROM recall "
				+"JOIN recalltype ON recall.RecallTypeNum=recalltype.RecallTypeNum "
				+"WHERE recalltype.recallTypeNum="+POut.Long(recallTypeNum);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Return RecallNums that have changed since a paticular time. </summary>
		public static List<long> GetChangedSinceRecallNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT RecallNum FROM recall WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> recallnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				recallnums.Add(PIn.Long(dt.Rows[i]["RecallNum"].ToString()));
			}
			return recallnums;
		}

		///<summary>Returns recalls with given list of RecallNums. Used along with GetChangedSinceRecallNums.</summary>
		public static List<Recall> GetMultRecalls(List<long> recallNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Recall>>(MethodBase.GetCurrentMethod(),recallNums);
			}
			string strRecallNums="";
			DataTable table;
			if(recallNums.Count>0) {
				for(int i=0;i<recallNums.Count;i++) {
					if(i>0) {
						strRecallNums+="OR ";
					}
					strRecallNums+="RecallNum='"+recallNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM recall WHERE "+strRecallNums;
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			Recall[] multRecalls=Crud.RecallCrud.TableToList(table).ToArray();
			List<Recall> recallList=new List<Recall>(multRecalls);
			return recallList;
		}

		#region Web Sched
		///<summary>Makes a web service call to WebServiceCustomerUpdates to make sure this customer is signed up for Web Sched.
		///Throws exceptions if the user is not valid.
		///An ODException will have an error code; 0=no errors. 110=No Web Sched repeating charge. 120=Invalid web service response. 190=All other errors.</summary>
		public static void ValidateWebSched() {
			//Either the Web Sched service was enabled or they just enabled it.
			//Send off a web request to  WebServiceCustomersUpdates to verify that the office is still valid and is currently paying for the eService.  
			StringBuilder strbuild=new StringBuilder();
			#region Web Service Call
#if DEBUG
			OpenDentBusiness.localhost.Service1 updateService=new OpenDentBusiness.localhost.Service1();
#else
			OpenDentBusiness.customerUpdates.Service1 updateService=new OpenDentBusiness.customerUpdates.Service1();
			updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress) !="") {
				IWebProxy proxy = new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				updateService.Proxy=proxy;
			}
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
			}
			#endregion
			string result="";
			try {
				result=updateService.ValidateWebSched(strbuild.ToString());
			}
			catch {
				//Do nothing.  Leaving result empty will display correct error messages later on.
			}
			ValidateWebSchedResponse(result);
		}

		///<summary>Validates the results of our ValidateWebSched web service call.
		///Returns true if they are valid otherwise throws an exception if the office is not allowed to use the Web Sched or other errors.
		///An ODException will have an error code; 0=no errors. 110=No Web Sched repeating charge. 120=Invalid web service response. 190=All other errors.</summary>
		///<param name="response">This should be the result string that was received from WebServiceCustomerUpdates.ValidateWebSched()</param>
		///<returns>True if user is an active customer and they have an active WebSched repeating charge.</returns>
		private static void ValidateWebSchedResponse(string response) {
			//No need to check RemotingRole; no call to db.
			XmlDocument doc=new XmlDocument();
			XmlNode node=null;
			try {
				doc.LoadXml(response);
				node=doc.SelectSingleNode("//ValidateWebSchedResponse");
			}
			catch {
				//Invalid web service response passed in.  Node will be null and will return false correctly.
			}
			if(node==null) {
				//There should always be a ValidateWebSchedResponse node.  If there isn't, something went wrong.
				throw new ODException(Lans.g("Recalls","Invalid web service response.  Please try again or give us a call."),120);
			}
			if(node.InnerText=="Valid") {
				return;
			}
			#region Specific Error Handling
			//At this point we know something went wrong.  So we need to give the user a hint as to why they can't enable the Web Sched.
			XmlNode nodeError=doc.SelectSingleNode("//Error");
			XmlNode nodeErrorCode=doc.SelectSingleNode("//ErrorCode");
			if(nodeError==null || nodeErrorCode==null) {
				//Something went wronger than wrong.
				throw new ODException(Lans.g("Recalls","Invalid web service response.  Please try again or give us a call."),120);
			}
			//Typical error messages will say something like: "Registration key period has ended", "Customer not registered for WebSched monthly service", etc.
			if(nodeErrorCode.InnerText=="110") {//Customer not registered for WebSched monthly service
				throw new ODException(Lans.g("Recalls","Please give us a call or visit our web page to see more information about signing up for this service."
					+"\r\n"+Recalls.GetWebSchedPromoURL()),110);
			}
			//For every other error message returned, we'll simply show it to the user.
			//Inner text can be exception text if something goes very wrong.  Do not translate.
			throw new ODException(Lans.g("Recalls","Error")+": "+nodeError.InnerText,190);
			#endregion
		}

		///<summary>Gets up to 30 days of open time slots based on the recall passed in.
		///Open time slots are found by looping through operatories flagged for Web Sched and finding openings that can hold the recall.
		///The amount of time required to be considered "available" is dictated by the RecallType associated to the recall passed in.</summary>
		///<returns>DataTable with 4 columns: SchedDate (date), TimeStart (DateTime), TimeStop (DateTime), OperatoryNum (long)</returns>
		public static DataTable GetAvailableWebSchedTimeSlots(long recallNum,DateTime dateStart,DateTime dateEnd) {
			//No need to check RemotingRole; no call to db.
			Clinic clinic=Clinics.GetClinicForRecall(recallNum);
			Recall recall=Recalls.GetRecall(recallNum);
			List<Provider> listProviders=Providers.GetProvidersForWebSched(recall.PatNum);
			if(recall==null) {
				throw new ODException(Lans.g("WebSched","The recall appointment you are trying to schedule is no longer available.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			List<RecallType> listRecallTypes=RecallTypeC.GetListt();
			RecallType recallType=listRecallTypes.FirstOrDefault(x => x.RecallTypeNum==recall.RecallTypeNum);
			return GetAvailableWebSchedTimeSlots(recallType,listProviders,clinic,dateStart,dateEnd);
		}

		///<summary>Gets up to 30 days of open time slots based on the RecallType passed in.
		///Open time slots are found by looping through operatories flagged for Web Sched and finding openings that can hold the RecallType.
		///The RecallType passed in must be a valid recall type.
		///Providers passed in will be the only providers considered when looking for available time slots.
		///Passing in a null clinic will only consider clinics set to 0 (unassigned) if clinics are enabled.
		///The timeslots on / or between the Start and End dates passed in will be considered.</summary>
		///<returns>DataTable with 4 columns: SchedDate (date), TimeStart (DateTime), TimeStop (DateTime), OperatoryNum (long)</returns>
		public static DataTable GetAvailableWebSchedTimeSlots(RecallType recallType,List<Provider> listProviders,Clinic clinic,DateTime dateStart,DateTime dateEnd) {
			//No need to check RemotingRole; no call to db.
			if(recallType==null) {//Validate that recallType is not null.
				throw new ODException(Lans.g("WebSched","The recall appointment you are trying to schedule is no longer available.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			//Get all the Operatories that are flagged for Web Sched.
			List<Operatory> listWebSchedOps=Operatories.GetOpsForWebSched();
			List<long> listWebSchedOpNums=listWebSchedOps.Select(x => x.OperatoryNum).Distinct().ToList();
			if(listWebSchedOpNums.Count < 1) {//This is very possible for offices that aren't set up the way that we expect them to be.
				throw new ODException(Lans.g("WebSched","There are no operatories set up for Web Sched.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			DataTable tableSchedules=Schedules.GetSchedulesAndBlockoutsForWebSched(listProviders,dateStart,dateEnd);
			//Convert the custom data table (includes operatory info) into a list of schedules so that other methods can have access to the info.
			List<Schedule> listAllSchedules=Crud.ScheduleCrud.TableToList(tableSchedules);//Does not call the db.
			List<Schedule> listProviderSchedules=listAllSchedules.FindAll(x => x.BlockoutType==0);//Ignore blockouts.
			DataTable tableBlockouts=GetBlockoutsFromSchedule(tableSchedules);//Make a custom table that will store all blockouts with op info.
			List<Appointment> listApptsForOps=Appointments.GetAppointmentsForOpsByPeriod(listWebSchedOpNums,dateStart,dateEnd);
			List<long> listProvNums=listProviders.Select(x => x.ProvNum).Distinct().ToList();
			//We need to be conscious of double booking possibilities.  Go get provider schedule information for the date range passed in.
			Dictionary<DateTime,List<ApptSearchProviderSchedule>> dictProvSchedules=Appointments.GetApptSearchProviderScheduleForProvidersAndDate(
				listProvNums,dateStart,dateEnd,listProviderSchedules,listApptsForOps);
			//Create the custom DataTable of available time slots which we will be returning.
			DataTable tableAvailableTimes=new DataTable();
			tableAvailableTimes.Columns.Add("SchedDate");
			tableAvailableTimes.Columns.Add("TimeStart");
			tableAvailableTimes.Columns.Add("TimeStop");
			tableAvailableTimes.Columns.Add("OperatoryNum");//Needed for when a time slot has been chosen.
			List<TimeSlot> listTimeSlots=new List<TimeSlot>();//Create a list of available time slots.
			DateTime dateLastTimeSlot=new DateTime(1800,1,1);//Keeps track of what date the last time slot was found.
			//Figure out how large of a time slot we need to find in order to consider this time slot "available".
			int apptLengthMins=RecallTypes.ConvertTimePattern(recallType.TimePattern).Length * 5;
			List<DateTime> listUniqueDays=new List<DateTime>();
			List<TimeSlot> listAvailableTimeSlots=new List<TimeSlot>();
			//Loop through all schedules five minutes at a time to find time slots large enough that have no appointments and no blockouts within them.
			for(int i=0;i<tableSchedules.Rows.Count;i++) {
				if(PIn.Long(tableSchedules.Rows[i]["BlockoutType"].ToString()) > 0) {
					continue;//Blockouts should not have appointments scheduled on top of them.
				}
				DateTime dateSched=PIn.Date(tableSchedules.Rows[i]["SchedDate"].ToString());
				if(!listUniqueDays.Contains(dateSched)) {
					listUniqueDays.Add(dateSched);
				}
				//If dateLastTimeSlot has a valid year, that means we have at least 30 days of time slots that will be sent back to the Web Sched app.
				if(dateLastTimeSlot.Year > 1880 && dateSched.Date!=dateLastTimeSlot.Date) {
					//We only want to break out once we are finished sending all available time slots for the 30th (last) day.
					//This makes the logic easier for the mobile app when they want to get "more dates".
					break;
				}
				long schedOpNum=PIn.Long(tableSchedules.Rows[i]["OperatoryNum"].ToString());
				TimeSpan timeSchedStart=PIn.Time(tableSchedules.Rows[i]["StartTime"].ToString());
				TimeSpan timeSchedStop=PIn.Time(tableSchedules.Rows[i]["StopTime"].ToString());
				//Skip this schedule entry if the operatory's clinic does not match the patient's clinic.
				Operatory op=listWebSchedOps.Find(x => x.OperatoryNum==schedOpNum);
				if(op==null) {
					continue;
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
					//If a clinic was not passed in, ONLY consider unassigned operatories
					//Also, if a valid clinic was passed in, make sure the operatory has a matching clinic.
					if((clinic==null && op.ClinicNum!=0)
						|| (clinic!=null && op.ClinicNum!=clinic.ClinicNum)) 
					{
						continue;
					}
				}
				//Straight up ignore schedules in the past.  This should not be possible but is just in case.
				if(dateSched.Date < DateTime.Today) {
					continue;
				}
				int timeIncrement=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
				//Check to see if we are currently looking at today's date. 
				if(dateSched.Date==DateTime.Now.Date) {
					//We need to make sure that we are looking for openings AFTER right now.
					//First, check the stop time of this schedule to make sure that right now isn't past the stop time of the schedule.
					if(DateTime.Now.TimeOfDay>=timeSchedStop) {
						continue;//The current time is or has passed the ending time for the schedule.
					}
					//Next, make sure that the start time is after right now.  If it isn't, set timeSchedStart to right now.
					if(DateTime.Now.TimeOfDay > timeSchedStart) {
						timeSchedStart=DateTime.Now.TimeOfDay;
					}
				}
				//Now, make sure that the start time is set to a starting time that makes sense with the appointment time increment preference.
				int minsOver=(timeSchedStart.Minutes)%timeIncrement;
				if(minsOver>0) {
					int minsToAdd=timeIncrement-minsOver;
					timeSchedStart=timeSchedStart.Add(new TimeSpan(0,minsToAdd,0));
				}
				//Double check that we haven't pushed the start time past the stop time.
				if(timeSchedStart>=timeSchedStop) {
					continue;
				}
				//At this point, we know that timeSchedStart is set to a valid time that we need to start looking for openings.
				//Start going through this operatories schedule according to the time increment, looking for a gap that can handle apptLengthMins.
				TimeSpan timeSlotStart=new TimeSpan(timeSchedStart.Ticks);
				//Start looking for collisions AFTER the start time.
				//Stop as soon as the slots stop time meets or passes the sched stop time.
				//Iterate through the schedule via the time increment preference.
				for(TimeSpan timeSlotStop=timeSchedStart.Add(new TimeSpan(0,timeIncrement,0))
					;timeSlotStop<=timeSchedStop
					;timeSlotStop=timeSlotStop.Add(new TimeSpan(0,timeIncrement,0)))
				{
					//Check to see if we've found an opening.
					TimeSpan timeSpanCur=timeSlotStop-timeSlotStart;
					if(timeSpanCur.TotalMinutes==apptLengthMins) {
						//We just found an opening.  Make sure we don't already have this time slot available.
						DateTime dateTimeSlotStart=new DateTime(dateSched.Year,dateSched.Month,dateSched.Day,timeSlotStart.Hours,timeSlotStart.Minutes,0);
						DateTime dateTimeSlotStop=new DateTime(dateSched.Year,dateSched.Month,dateSched.Day,timeSlotStop.Hours,timeSlotStop.Minutes,0);
						TimeSlot timeSlot=new TimeSlot(dateTimeSlotStart,dateTimeSlotStop,PIn.Long(tableSchedules.Rows[i]["OperatoryNum"].ToString()));
						bool isTimeSlotAlreadyAvailable=false;
						bool hasDoubleBookingConflict=false;
						for(int j=0;j<listAvailableTimeSlots.Count;j++) {
							if(listAvailableTimeSlots[j].DateTimeStart==dateTimeSlotStart
								&& listAvailableTimeSlots[j].DateTimeStop==dateTimeSlotStop) 
							{
								isTimeSlotAlreadyAvailable=true;
								break;
							}
						}
						if(!isTimeSlotAlreadyAvailable) {
							//Check for double booking.
							if(dictProvSchedules.ContainsKey(dateSched.Date)) {
								long recallProvNum=PIn.Long(tableSchedules.Rows[i]["ProvNum"].ToString());
								if(IsWebSchedDoubleBooked(dictProvSchedules[dateSched.Date],recallProvNum,RecallTypes.ConvertTimePattern(recallType.TimePattern),dateTimeSlotStart)) {
									//There is a double booking conflict.  Do not add this time slot as a possibility and move on.
									hasDoubleBookingConflict=true;
								}
							}
							if(!hasDoubleBookingConflict) {
								//There are no collisions with this provider's schedule, add it to our list of available time slots.
								listAvailableTimeSlots.Add(timeSlot);
							}
						}
						//Continue looking for more open slots starting at the end of this time slot.
						//E.g. we just found 9:30 AM to 10:00 AM.  We need to continue from 10:00 AM.
						timeSlotStart=timeSlotStop;
						continue;
					}
					//Check to see if there is an appointment or a blockout that collides with this blockout.
					bool isOverlapping=false;
					//First we'll look at blockouts because it should be quicker than looking at the appointments
					for(int k=0;k<tableBlockouts.Rows.Count;k++) {
						if(schedOpNum!=PIn.Long(tableBlockouts.Rows[k]["OperatoryNum"].ToString())) {
							continue;
						}
						DateTime dateBlockout=PIn.Date(tableBlockouts.Rows[k]["SchedDate"].ToString());
						if(dateSched.Date!=dateBlockout.Date) {
							continue;//Block out is not on the same day that we are looking at.
						}
						//Same operatory and day, check if the times overlap.
						TimeSpan timeBlockoutStart=PIn.Time(tableBlockouts.Rows[k]["StartTime"].ToString());
						TimeSpan timeBlockoutStop=PIn.Time(tableBlockouts.Rows[k]["StopTime"].ToString());
						//Remove the date portion from the blockouts:
						timeBlockoutStart=new TimeSpan(timeBlockoutStart.Hours,timeBlockoutStart.Minutes,0);
						timeBlockoutStop=new TimeSpan(timeBlockoutStop.Hours,timeBlockoutStop.Minutes,0);
						if(IsTimeOverlapping(timeSlotStart,timeSlotStop,timeBlockoutStart,timeBlockoutStop)) {
							isOverlapping=true;
							break;
						}
					}
					if(isOverlapping) {//This check is here so that we don't waste time looping through appointments if we don't need to.
						//There was a collision, set the time slot start time to the stop time and continue from there.
						timeSlotStart=timeSlotStop;
						continue;
					}
					//Next we'll look for overlapping appointments
					for(int k=0;k<listApptsForOps.Count;k++) {
						if(schedOpNum!=listApptsForOps[k].Op) {
							continue;
						}
						if(dateSched.Date!=listApptsForOps[k].AptDateTime.Date) {
							continue;//Appt is not on the same day that we are looking at.
						}
						//Same operatory and day, check if the times overlap.
						TimeSpan timeApptStart=listApptsForOps[k].AptDateTime.TimeOfDay;
						TimeSpan timeApptStop=listApptsForOps[k].AptDateTime.AddMinutes(listApptsForOps[k].Pattern.Length*5).TimeOfDay;
						if(IsTimeOverlapping(timeSlotStart,timeSlotStop,timeApptStart,timeApptStop)) {
							isOverlapping=true;
							break;
						}
					}
					if(isOverlapping) {
						//There was a collision, set the time slot start time to the stop time and continue from there.
						timeSlotStart=timeSlotStop;
						continue;
					}
				}
				if(listUniqueDays.Count>=30) {//Once we hit at least 30 days of time slots, we want to finish getting the rest for that day then kick out.
					dateLastTimeSlot=dateSched;
				}
			}
			//Order the entire list of available time slots so that they are displayed to the user in sequential order.
			//We need to do this because we loop through each provider's schedule one at a time and add openings as we find them.
			listAvailableTimeSlots=listAvailableTimeSlots.OrderBy(x => x.DateTimeStart).ToList();
			//Turn the list of available times into a DataTable.
			for(int i=0;i<listAvailableTimeSlots.Count;i++) {
				tableAvailableTimes.Rows.Add(listAvailableTimeSlots[i].DateTimeStart.ToShortDateString()
						,listAvailableTimeSlots[i].DateTimeStart.ToString("yyyy-MM-dd HH:mm:ss")
						,listAvailableTimeSlots[i].DateTimeStop.ToString("yyyy-MM-dd HH:mm:ss")
						,listAvailableTimeSlots[i].OperatoryNum.ToString());
			}
			return tableAvailableTimes;
		}

		///<summary>Returns a datatable that matches the structure of the table passed in where the column BlockoutType is > 0.
		///Returns an empty DataTable if no blockouts found.</summary>
		private static DataTable GetBlockoutsFromSchedule(DataTable tableSchedules) {
			//No need to check RemotingRole; no call to db.
			DataTable tableBlockouts=tableSchedules.Clone();
			for(int i=0;i<tableSchedules.Rows.Count;i++) {
				if(PIn.Long(tableSchedules.Rows[i]["BlockoutType"].ToString()) > 0) {
					tableBlockouts.Rows.Add(tableSchedules.Rows[i].ItemArray);//This is a blockout so add it to our table of blockouts.
					continue;
				}
			}
			return tableBlockouts;
		}

		///<summary>Checks to see if the recallProvNum provider has any double booking issues with the recall pattern passed in.
		///timePattern must be a time pattern in 5 minute increments.</summary>
		private static bool IsWebSchedDoubleBooked(List<ApptSearchProviderSchedule> listProviderSchedules,long recallProvNum,string timePattern,DateTime dateTimeRecallStart) {
			List<ApptSearchProviderSchedule> listProviderSchedulesForProv=listProviderSchedules.FindAll(x => x.ProviderNum==recallProvNum);
			//Figure out what 5 min increment the dateTimeRecallStart passed in starts on.
			int startingIncrement=(int)dateTimeRecallStart.TimeOfDay.TotalMinutes/5;
			for(int i=0;i<listProviderSchedulesForProv.Count;i++) {//There should only be one.
				//Check to make sure the ProvBar does not have any conflicts with the recallPattern passed in.
				for(int j=0;j<timePattern.Length;j++) {
					if(timePattern[j]=='/') {//Don't worry about HYG conflicts.
						continue;
					}
					if(!listProviderSchedulesForProv[i].ProvBar[startingIncrement+j]) {//False means there is a collision in the providers schedule.
						return true;
					}
				}
			}
			return false;//No double booking collision.
		}

		///<summary>Checks if the two times passed in overlap.</summary>
		private static bool IsTimeOverlapping(TimeSpan timeStartBegin,TimeSpan timeStartEnd,TimeSpan timeStopBegin,TimeSpan timeStopEnd) {
			//No need to check RemotingRole; no call to db.
			//Test start times
			if(timeStartBegin >= timeStopBegin && timeStartBegin < timeStopEnd) {
				return true;
			}
			//Test end times
			if(timeStartEnd > timeStopBegin && timeStartEnd <= timeStopEnd) {
				return true;
			}
			//Test engulf
			if(timeStartBegin <= timeStopBegin && timeStartEnd >= timeStopEnd) {
				return true;
			}
			return false;
		}

		///<summary>Used in the eConnector service.</summary>
		public static string SendWebSchedNotifications() {
			try {
				ValidateWebSched();
			}
			catch(Exception ex) {
				return ex.Message;
			}
			DateTime fromDate=DateTime.MinValue;
			DateTime toDate=DateTime.MinValue;
			int daysPast=PrefC.GetInt(PrefName.RecallDaysPast);
			int daysFuture=PrefC.GetInt(PrefName.RecallDaysFuture);
			if(daysPast==-1) {
				fromDate=DateTime.MinValue;
			}
			else {
				fromDate=DateTime.MinValue;
				//fromDate=DateTime.Today.AddDays(-daysPast);
			}
			if(daysFuture==-1) {
				toDate=DateTime.MaxValue;
			}
			else {
				toDate=DateTime.Today.AddDays(daysFuture);
			}
			long provNum=0;
			long clinicNum=0;
			long siteNum=0;
			List<long> recallNums=new List<long>();
			DataTable table=Recalls.GetRecallList(fromDate,toDate,PrefC.GetBool(PrefName.RecallGroupByFamily),provNum,clinicNum,siteNum,RecallListSort.Alphabetical,RecallListShowNumberReminders.All,recallNums);
			for(int i=0;i<table.Rows.Count;i++) {
				recallNums.Add(PIn.Long(table.Rows[i]["RecallNum"].ToString()));
			}
			if(recallNums.Count==0) {
				return "Did not send recall notifications for Web Sched.  There are no recalls to send notifications for.";
			}
			return SendWebSchedNotifications(recallNums,PrefC.GetBool(PrefName.RecallGroupByFamily),RecallListSort.Alphabetical);
		}

		public static string SendWebSchedNotifications(List<long> recallNums,bool isGroupFamily,RecallListSort sortBy) {
			//No need to check RemotingRole; no call to db.
			string response="";
			Dictionary<long,string> dictWebSchedParameters=new Dictionary<long,string>();
			//Send off a web request to WebServiceCustomersUpdates to get the obfuscated URLs for the selected patients.
			#region Send Web Service Request For URLs
#if DEBUG
			OpenDentBusiness.localhost.Service1 updateService=new OpenDentBusiness.localhost.Service1();
#else
			OpenDentBusiness.customerUpdates.Service1 updateService=new OpenDentBusiness.customerUpdates.Service1();
			updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress)!="") {
				IWebProxy proxy=new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				updateService.Proxy=proxy;
			}
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("RSData");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("RecallNums");
				writer.WriteString(String.Join("|",recallNums));//A pipe delimited list of recall nums. E.g. 3|2|1|4
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			try {
				response=updateService.GetWebSchedURLs(strbuild.ToString());
			}
			catch {
				//Do nothing.  Leaving result empty will display correct error messages later on.
			}
			#endregion
			#region Parse Response
			XmlDocument doc=new XmlDocument();
			XmlNode nodeError=null;
			XmlNode nodeResponse=null;
			XmlNodeList nodeURLs=null;
			try {
				doc.LoadXml(response);
				nodeError=doc.SelectSingleNode("//Error");
				nodeResponse=doc.SelectSingleNode("//GetWebSchedURLsResponse");
			}
			catch {
				//Invalid web service response passed in.  Node will be null and will return false correctly.
			}
			#region Error Handling
			if(nodeError!=null || nodeResponse==null) {
				string error=Lans.g("WebSched","There was an error with the web request.  Please try again or give us a call.");
				//Either something went wrong or someone tried to get cute and use our Web Sched service when they weren't supposed to.
				if(nodeError!=null) {
					error+="\r\n"+Lans.g("WebSched","Error Details")+":\r\n" +nodeError.InnerText;
				}
				return error;
			}
			#endregion
			//At this point we know we got a valid response from our web service.
			dictWebSchedParameters.Clear();
			nodeURLs=doc.GetElementsByTagName("URL");
			if(nodeURLs!=null) {
				//Loop through all the URL nodes that were returned.
				//Each URL node will contain an RN attribute which will be the corresponding recall num.
				for(int i=0;i<nodeURLs.Count;i++) {
					long recallNum=0;
					XmlAttribute attributeRecallNum=nodeURLs[i].Attributes["RN"];
					if(attributeRecallNum!=null) {
						recallNum=PIn.Long(attributeRecallNum.Value);
					}
					dictWebSchedParameters.Add(recallNum,nodeURLs[i].InnerText);
				}
			}
			#endregion
			//Now that the web service response has been validated, parsed, and our dictionary filled, we now can loop through the selected patients and send off the emails.
			DataTable addrTable=Recalls.GetAddrTableForWebSched(recallNums,isGroupFamily,sortBy);
			EmailMessage emailMessage;
			EmailAddress emailAddress;
			for(int i=0;i<addrTable.Rows.Count;i++) {
				#region Send Email Notification
				string emailBody="";
				string emailSubject="";
				emailMessage=new EmailMessage();
				emailMessage.PatNum=PIn.Long(addrTable.Rows[i]["emailPatNum"].ToString());
				emailMessage.ToAddress=PIn.String(addrTable.Rows[i]["email"].ToString());//might be guarantor email
				emailAddress=EmailAddresses.GetByClinic(PIn.Long(addrTable.Rows[i]["ClinicNum"].ToString()));
				emailMessage.FromAddress=emailAddress.SenderAddress;
				if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
					emailSubject=PrefC.GetString(PrefName.WebSchedSubject);
					emailBody=PrefC.GetString(PrefName.WebSchedMessage);
				}
				else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
					emailSubject=PrefC.GetString(PrefName.WebSchedSubject2);
					emailBody=PrefC.GetString(PrefName.WebSchedMessage2);
				}
				else {
					emailSubject=PrefC.GetString(PrefName.WebSchedSubject3);
					emailBody=PrefC.GetString(PrefName.WebSchedMessage3);
				}
				emailSubject=emailSubject.Replace("[NameF]",addrTable.Rows[i]["patientNameF"].ToString());
				//It is common for offices to have paitents with a blank recall date (they've never had a recall performed at the office).
				//Instead of showing 01/01/0001 in the email, we will simply show today's date because that is what the Web Sched time slots will start showing.
				DateTime dateDue=PIn.Date(addrTable.Rows[i]["dateDue"].ToString());
				if(dateDue.Year < 1880) {
					dateDue=DateTime.Today;
				}
				emailBody=emailBody.Replace("[DueDate]",dateDue.ToShortDateString());
				emailBody=emailBody.Replace("[NameF]",addrTable.Rows[i]["patientNameF"].ToString());
				string URL="";
				try {
					dictWebSchedParameters.TryGetValue(PIn.Long(addrTable.Rows[i]["RecallNum"].ToString()),out URL);
				}
				catch(Exception ex) {
					string error=ex.Message+"\r\n";
					return error+Lans.g("WebSched","Problem getting Web Sched URL for patient")+": "+addrTable.Rows[i]["patientNameFL"].ToString();
				}
				emailBody=emailBody.Replace("[URL]",URL);
				string officePhone=PrefC.GetString(PrefName.PracticePhone);
				Clinic clinic=Clinics.GetClinic(PIn.Long(addrTable.Rows[i]["clinicNum"].ToString()));
				if(clinic!=null && !String.IsNullOrEmpty(clinic.Phone)) {
					officePhone=clinic.Phone;
				}
				if(CultureInfo.CurrentCulture.Name=="en-US" && officePhone.Length==10) {
					officePhone="("+officePhone.Substring(0,3)+")"+officePhone.Substring(3,3)+"-"+officePhone.Substring(6);
				}
				emailBody=emailBody.Replace("[OfficePhone]",officePhone);
				emailMessage.Subject=emailSubject;
				emailMessage.BodyText=emailBody;
				try {
					EmailMessages.SendEmailUnsecure(emailMessage,emailAddress);
				}
				catch(Exception ex) {
					string error=ex.Message+"\r\n";
					if(ex.GetType()==typeof(System.ArgumentException)) {
						error+=Lans.g("WebSched","Go to Setup | Appointments | Recall.  The subject for WebSched notifications must not span multiple lines.")+"\r\n";
					}
					return error+Lans.g("WebSched","Patient")+": "+addrTable.Rows[i]["patientNameFL"].ToString();
				}
				emailMessage.MsgDateTime=DateTime.Now;
				emailMessage.SentOrReceived=EmailSentOrReceived.Sent;
				EmailMessages.Insert(emailMessage);
				#endregion
				#region Insert Commlog
				long userNum=0;//For Web Sched
				if(Security.CurUser!=null) {
					userNum=Security.CurUser.UserNum;//Middle tier should not be using this method, it does not call the db.
				}
				Commlogs.InsertForRecall(PIn.Long(addrTable.Rows[i]["PatNum"].ToString()),CommItemMode.Email,PIn.Int(addrTable.Rows[i]["numberOfReminders"].ToString()),
					PrefC.GetLong(PrefName.RecallStatusEmailed),CommItemSource.WebSched,userNum);
				Recalls.UpdateStatus(PIn.Long(addrTable.Rows[i]["RecallNum"].ToString()),PrefC.GetLong(PrefName.RecallStatusEmailed));
				#endregion
			}
			return "";
		}

		private struct TimeSlot {
			public DateTime DateTimeStart;
			public DateTime DateTimeStop;
			public long OperatoryNum;

			public TimeSlot(DateTime dateTimeStart,DateTime dateTimeStop,long operatoryNum) {
				DateTimeStart=dateTimeStart;
				DateTimeStop=dateTimeStop;
				OperatoryNum=operatoryNum;
			}
		}
		#endregion
	}

	///<summary>The supplied DataRows must include the following columns: Guarantor, PatNum, guarLName, guarFName, LName, FName, DateDue, maxDateDue, billingType.  maxDateDue is the most recent DateDue for all family members in the list and needs to be the same for all family members.  This date will be used for better grouping.</summary>
	class RecallComparer:IComparer<DataRow> {
		public bool GroupByFamilies;
		///<summary>rather than by the ordinary DueDate.</summary>
		public RecallListSort SortBy;

		///<summary></summary>
		public int Compare(DataRow x,DataRow y) {
			//NOTE: Even if grouping by families, each family is not necessarily going to have a guarantor.
			if(GroupByFamilies) {
				if(SortBy==RecallListSort.Alphabetical) {
					//if guarantors are different, sort by guarantor name
					if(x["Guarantor"].ToString() != y["Guarantor"].ToString()) {
						if(x["guarLName"].ToString() != y["guarLName"].ToString()) {
							return x["guarLName"].ToString().CompareTo(y["guarLName"].ToString());
						}
						return x["guarFName"].ToString().CompareTo(y["guarFName"].ToString());
					}
					return 0;//order within family does not matter
				}
				else if(SortBy==RecallListSort.DueDate) {
					DateTime xD=PIn.Date(x["maxDateDue"].ToString());
					DateTime yD=PIn.Date(y["maxDateDue"].ToString());
					if(xD != yD) {
						return (xD.CompareTo(yD));
					}
					//if dates are same, sort/group by guarantor
					if(x["Guarantor"].ToString() != y["Guarantor"].ToString()) {
						return (x["Guarantor"].ToString().CompareTo(y["Guarantor"].ToString()));
					}
					//within the same family, sort by actual DueDate
					xD=PIn.Date(x["DateDue"].ToString());
					yD=PIn.Date(y["DateDue"].ToString());
					return (xD.CompareTo(yD));
					//return 0;
				}
				else if(SortBy==RecallListSort.BillingType){
					if(x["billingType"].ToString()!=y["billingType"].ToString()){
						return x["billingType"].ToString().CompareTo(y["billingType"].ToString());
					}
					//if billing types are the same, sort by dueDate
					DateTime xD=PIn.Date(x["maxDateDue"].ToString());
					DateTime yD=PIn.Date(y["maxDateDue"].ToString());
					if(xD != yD) {
						return (xD.CompareTo(yD));
					}
					//if dates are same, sort/group by guarantor
					if(x["Guarantor"].ToString() != y["Guarantor"].ToString()) {
						return (x["Guarantor"].ToString().CompareTo(y["Guarantor"].ToString()));
					}
				}
			}
			else {//individual patients
				if(SortBy==RecallListSort.Alphabetical) {
					if(x["LName"].ToString() != y["LName"].ToString()) {
						return x["LName"].ToString().CompareTo(y["LName"].ToString());
					}
					return x["FName"].ToString().CompareTo(y["FName"].ToString());
				}
				else if(SortBy==RecallListSort.DueDate) {
					if((DateTime)x["DateDue"] != (DateTime)y["DateDue"]) {
						return ((DateTime)x["DateDue"]).CompareTo(((DateTime)y["DateDue"]));
					}
					//if duedates are the same, sort by LName
					return x["LName"].ToString().CompareTo(y["LName"].ToString());
				}
				else if(SortBy==RecallListSort.BillingType){
					if(x["billingType"].ToString()!=y["billingType"].ToString()){
						return x["billingType"].ToString().CompareTo(y["billingType"].ToString());
					}
					//if billing types are the same, sort by dueDate
					if((DateTime)x["DateDue"] != (DateTime)y["DateDue"]) {
						return ((DateTime)x["DateDue"]).CompareTo(((DateTime)y["DateDue"]));
					}
					//if duedates are the same, sort by LName
					return x["LName"].ToString().CompareTo(y["LName"].ToString());
				}
			}
			return 0;
		}




	}

	public enum RecallListShowNumberReminders {
		All,
		Zero,
		One,
		Two,
		Three,
		Four,
		Five,
		SixPlus
	}

	public enum RecallListSort{
		DueDate,
		Alphabetical,
		BillingType
	}
	

}









