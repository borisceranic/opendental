using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Schedules {		
		///<summary>Gets a list of Schedule items for one date.</summary>
		public static List<Schedule> GetDayList(DateTime date){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),date);
			}
			string command="SELECT * FROM schedule "
				+"WHERE SchedDate = "+POut.Date(date)+" "
				+"ORDER BY StartTime";
			return RefreshAndFill(command);
		}

		///<summary>Used in the Schedules edit window to get a filtered list of schedule items in preparation for paste or repeat.</summary>
		public static List<Schedule> RefreshPeriod(DateTime dateStart,DateTime dateEnd,List<long> provNums,List<long> empNums,bool includePNotes,
			bool includeCNotes,long clinicNum)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,provNums,empNums,includePNotes,includeCNotes,clinicNum);
			}
			if(provNums.Count==0 && empNums.Count==0 && !includeCNotes && !includePNotes) {
				return new List<Schedule>();
			}
			List<string> listOrClauses=new List<string>();
			if(includePNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum=0)");
			}
			//if the user has the HQ clinic selected and checks the show clinic holidays and notes, this will show holidays and notes for all clinics
			//if any other clinic is selected, this will show those holidays and notes for the selected clinic
			if(includeCNotes) {
				//if HQ, include notes and holidays for all non-HQ clinics, otherwise only include for the selected clinic
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum"+(clinicNum==0?">0":("="+POut.Long(clinicNum)))+")");
			}
			if(provNums.Count>0) {
				listOrClauses.Add("schedule.ProvNum IN ("+string.Join(",",provNums.Select(x => POut.Long(x)))+")");
			}
			if(empNums.Count>0) {
				listOrClauses.Add("schedule.EmployeeNum IN ("+string.Join(",",empNums.Select(x => POut.Long(x)))+")");
			}
			string command="SELECT * FROM schedule "
				+"WHERE SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND ("+string.Join(" OR ",listOrClauses)+")";
			return RefreshAndFill(command);
		}

		///<summary></summary>
		public static List<Schedule> RefreshPeriodBlockouts(DateTime dateStart,DateTime dateEnd,List<long> opNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,opNums);
			}
			if(opNums.Count==0){
				return new List<Schedule>();
			}
			string command="SELECT schedule.ScheduleNum,SchedDate,StartTime,StopTime,SchedType,ProvNum,BlockoutType,Note,Status,EmployeeNum,DateTStamp "
				+"FROM schedule "
				+"WHERE SchedType="+POut.Int((int)ScheduleType.Blockout)+" "
				+"AND SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND ScheduleNum IN (SELECT ScheduleNum FROM scheduleop WHERE OperatoryNum IN("+string.Join(",",opNums.Select(x => POut.Long(x)))+"))";
			return RefreshAndFill(command);
		}

		///<summary></summary>
		public static List<Schedule> RefreshDayEdit(DateTime dateSched){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),dateSched);
			}
			string command="SELECT schedule.* "
				+"FROM schedule "
				+"WHERE SchedDate="+POut.Date(dateSched)+" "
				+"AND SchedType IN (0,1,3)";//Practice or Provider or Employee
			return RefreshAndFill(command);
		}

		///<summary>Gets a list of Schedule items for one date filtered by providers and employees.
		///Also option to include practice and clinic holidays and practice notes.</summary>
		public static List<Schedule> RefreshDayEditForPracticeProvsEmps(DateTime dateSched,List<long> listProvNums,List<long> listEmpNums,
			bool includePNotes,bool includeCNotes,long clinicNum)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),dateSched,listProvNums,listEmpNums,includePNotes,includeCNotes,clinicNum);
			}
			if(listProvNums.Count==0 && listEmpNums.Count==0 && !includeCNotes && !includePNotes) {
				return new List<Schedule>();
			}
			List<string> listOrClauses=new List<string>();
			if(listProvNums.Count>0) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Provider)+" "
					+"AND ProvNum IN ("+string.Join(",",listProvNums.Select(x => POut.Long(x)))+"))");
			}
			if(listEmpNums.Count>0) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Employee)+" "
					+"AND EmployeeNum IN ("+string.Join(",",listEmpNums.Select(x => POut.Long(x)))+"))");
			}
			//Only notes with clinicNum==0
			if(includePNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum=0)");
			}
			//Only notes with clinicNum!=0; Treats HQ/ClinicNum==0 as show all non-practice notes.
			if(includeCNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum"+(clinicNum==0?">0":("="+POut.Long(clinicNum)))+")");
			}
			string command="SELECT schedule.* "
				+"FROM schedule "
				+"WHERE SchedDate="+POut.Date(dateSched)+" "
				+"AND ("+string.Join(" OR ",listOrClauses)+")";
			return RefreshAndFill(command);
		}

		///<summary></summary>
		public static List<Schedule> GetTwoYearPeriod(DateTime startDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),startDate);
			}
			string command="SELECT schedule.* "
				+"FROM schedule "
				+"WHERE SchedDate BETWEEN "+POut.Date(startDate)+" AND "+POut.Date(startDate.AddYears(2))+" "
				+"AND SchedType IN (0,1,3)";//Practice or Provider or Employee
			return RefreshAndFill(command);
		}

		///<summary>Used in the check database integrity tool.</summary>
		public static Schedule[] RefreshAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Schedule[]>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM schedule";
			return RefreshAndFill(command).ToArray();
		}

		public static List<Schedule> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * schedule WHERE DateTStamp>"+POut.DateT(changedSince)+" AND SchedType="+POut.Int((int)ScheduleType.Provider);
			return RefreshAndFill(command);
		}

		///<summary>This is only allowed because it's PRIVATE.  Retrieves all schedules from the db using command, then retrieves all scheduleops for the
		///schedules and fills the schedule.Ops list with OperatoryNums for the schedule.  Does NOT use GROUP_CONCAT since there is a max lenght for
		///GROUP_CONCAT and data may be incorrect or truncated, especially with random primary keys.</summary>
		private static List<Schedule> RefreshAndFill(string command) {
			//Not a typical refreshandfill, as it contains a query.
			List<Schedule> listScheds=Crud.ScheduleCrud.SelectMany(command);
			if(listScheds.Count==0) {
				return listScheds;
			}
			command="SELECT ScheduleNum,OperatoryNum FROM scheduleop WHERE ScheduleNum IN("+string.Join(",",listScheds.Select(x => x.ScheduleNum))+")";
			DataTable tableSO=Db.GetTable(command);
			if(tableSO.Rows.Count==0) {
				return listScheds;
			}
			Dictionary<long,List<long>> dictSchedOps=tableSO.Rows.OfType<DataRow>()
				.GroupBy(x => PIn.Long(x["ScheduleNum"].ToString()),x => PIn.Long(x["OperatoryNum"].ToString()))
				.ToDictionary(x => x.Key,x => x.ToList());
			listScheds.FindAll(x => dictSchedOps.ContainsKey(x.ScheduleNum))//find schedules that have 1+ scheduleops.
				.ForEach(x => x.Ops=dictSchedOps[x.ScheduleNum]);
			return listScheds;
		}

		public static List<Schedule> ConvertTableToList(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Schedule> retVal=Crud.ScheduleCrud.TableToList(table);
			if(!table.Columns.Contains("ops")) {
				return retVal;
			}
			for(int i = 0;i<retVal.Count;i++) {
				retVal[i].Ops=table.Rows[i]["ops"].ToString().Split(',').Select(x => PIn.Long(x)).ToList();
			}
			return retVal;
		}

		///<summary></summary>
		public static void Update(Schedule sched){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sched);
				return;
			}
			Validate(sched);
			Crud.ScheduleCrud.Update(sched);
			string command="DELETE FROM scheduleop WHERE ScheduleNum="+POut.Long(sched.ScheduleNum);
			Db.NonQ(command);
			sched.Ops.ForEach(x => ScheduleOps.Insert(new ScheduleOp { ScheduleNum=sched.ScheduleNum,OperatoryNum=x }));
		}

		///<summary>Similar to Crud.ScheduleCrud.Update except this also handles ScheduleOps.</summary>
		public static void Update(Schedule schedNew,Schedule schedOld,bool validate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),schedNew,schedOld,validate);
				return;
			}
			if(validate) {
				Validate(schedNew);
			}
			Crud.ScheduleCrud.Update(schedNew,schedOld); //may not cause an update, but we still need to check for updates to ScheduleOps below.
			//Sort Ops for SequenceEqual call below.
			schedNew.Ops.Sort();
			schedOld.Ops.Sort();
			if(schedNew.Ops.SequenceEqual(schedOld.Ops)) {  //If both lists contain exactly the same ops.
				return;//no updates to ScheduleOps needed
			}
			string command="DELETE FROM scheduleop WHERE ScheduleNum="+POut.Long(schedNew.ScheduleNum);
			Db.NonQ(command);
			//re-insert ScheduleOps based on the list of opnums in schedNew.Ops
			schedNew.Ops.ForEach(x => ScheduleOps.Insert(new ScheduleOp { ScheduleNum=schedNew.ScheduleNum,OperatoryNum=x }));
		}

		///<summary>Set validate to true to throw an exception if start and stop times need to be validated.  If validate is set to false, then the calling code is responsible for the validation.  Also inserts necessary scheduleop enteries.</summary>
		public static long Insert(Schedule sched,bool validate){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sched.ScheduleNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sched,validate);
				return sched.ScheduleNum;
			}
			if(validate) {
				Validate(sched);
			}
			Crud.ScheduleCrud.Insert(sched);
			sched.Ops.ForEach(x => ScheduleOps.Insert(new ScheduleOp { ScheduleNum=sched.ScheduleNum,OperatoryNum=x }));
			return sched.ScheduleNum;
		}

		///<summary></summary>
		private static void Validate(Schedule sched){
			if(sched.StopTime>TimeSpan.FromDays(1)) {//if pasting to late afternoon, the stop time might be calculated as early the next morning.
				throw new Exception(Lans.g("Schedule","Stop time must be later than start time."));
			}
			if(sched.StartTime>sched.StopTime) {
				throw new Exception(Lans.g("Schedule","Stop time must be later than start time."));
			}
			if(sched.StartTime+TimeSpan.FromMinutes(5)>sched.StopTime	&& sched.Status==SchedStatus.Open) {
				throw new Exception(Lans.g("Schedule","Stop time cannot be the same as the start time."));
			}
		}

		///<summary>Goes to the db to look for overlaps.  Implemented for blockouts, but should work for other types, too.</summary>
		public static bool Overlaps(Schedule sched){
			//No need to check RemotingRole; no call to db.
			return Schedules.GetForType(Schedules.GetDayList(sched.SchedDate),sched.SchedType,sched.ProvNum)//blockouts but should work for any SchedType
				.Any(x => x.ScheduleNum!=sched.ScheduleNum //not the same schedule
					&& (x.SchedType!=ScheduleType.Blockout || sched.Ops.Any(y => x.Ops.Contains(y)))//blockout that shares at least one op
					&& (sched.StopTime>x.StartTime && sched.StartTime<x.StopTime));//time overlap
		}

		///<summary></summary>
		public static void Delete(Schedule sched){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sched);
				return;
			}
			string command= "DELETE from schedule WHERE schedulenum='"+POut.Long(sched.ScheduleNum)+"'";
 			Db.NonQ(command);
			command="DELETE FROM scheduleop WHERE ScheduleNum="+POut.Long(sched.ScheduleNum);
			Db.NonQ(command);
			if(sched.SchedType==ScheduleType.Provider){
				DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv,sched.ScheduleNum);
			}
		}
	
		///<summary>Supply a list of all Schedule for one day. Then, this filters out for one type.</summary>
		public static Schedule[] GetForType(List<Schedule> ListDay,ScheduleType schedType,long provNum){
			//No need to check RemotingRole; no call to db.
			return ListDay.FindAll(x => x.SchedType==schedType && x.ProvNum==provNum).ToArray();
		}

		///<summary>Supply a list of Schedule . Then, this filters out for an employee.</summary>
		public static List<Schedule> GetForEmployee(List<Schedule> ListDay,long employeeNum) {
			//No need to check RemotingRole; no call to db.
			return ListDay.FindAll(x => x.SchedType==ScheduleType.Employee && x.EmployeeNum==employeeNum);
		}

		///<summary>Returns schedules with SchedType.Provider for a specific op.  This overload is for when the listForPeriod includes multiple days.</summary>
		public static List<Schedule> GetSchedsForOp(List<Schedule> listForPeriod,DayOfWeek dayOfWeek,Operatory op){
			//No need to check RemotingRole; no call to db.
			return GetSchedsForOp(listForPeriod.FindAll(x => x.SchedDate.DayOfWeek==dayOfWeek).Select(x => x.Copy()).ToList(),op);
		}

		///<summary>Returns schedules with SchedType.Provider for a specific op.  This overload is for when the listForPeriod includes only one day.</summary>
		public static List<Schedule> GetSchedsForOp(List<Schedule> listForPeriod,Operatory op){
			//No need to check RemotingRole; no call to db.
			List<Schedule> retVal=new List<Schedule>();
			foreach(Schedule schedCur in listForPeriod.FindAll(x => x.SchedType==ScheduleType.Provider)) {//only schedules for provs
				if(schedCur.Ops.Count>0) { //the schedule is for specific op(s), add if it is for this op
					if(schedCur.Ops.Contains(op.OperatoryNum)) {
						retVal.Add(schedCur.Copy());
					}
					continue;
				}
				//the schedule is not for specific op(s), check op settings to see whether to add it
				if(op.ProvDentist>0 && !op.IsHygiene) {//op uses dentist
					if(schedCur.ProvNum==op.ProvDentist) {
						retVal.Add(schedCur.Copy());
					}
				}
				else if(op.ProvHygienist>0 && op.IsHygiene) {//op uses hygienist
					if(schedCur.ProvNum==op.ProvHygienist) {
						retVal.Add(schedCur.Copy());
					}
				}
				else {//op is either a hygiene op with no hygienist set or not a hygiene op with no provider set
					if(schedCur.ProvNum==PrefC.GetLong(PrefName.ScheduleProvUnassigned)) {//use the provider for unassigned ops
						retVal.Add(schedCur.Copy());
					}
				}
			}
			return retVal;
		}

		///<summary>If no provider is found for spot then the operatory provider is returned.</summary>
		public static long GetAssignedProvNumForSpot(List<Schedule> listForPeriod,Operatory op,bool isSecondary,DateTime aptDateTime) {
			//No need to check RemotingRole; no call to db.
			//first, look for a sched assigned specifically to that spot
			for(int i=0;i<listForPeriod.Count;i++){
				if(listForPeriod[i].SchedType!=ScheduleType.Provider){
					continue;
				}
				if(aptDateTime.Date!=listForPeriod[i].SchedDate){
					continue;
				}
				if(!listForPeriod[i].Ops.Contains(op.OperatoryNum)){
					continue;
				}
				if(isSecondary && !Providers.GetIsSec(listForPeriod[i].ProvNum)){
					continue;
				}
				if(!isSecondary && Providers.GetIsSec(listForPeriod[i].ProvNum)){
					continue;
				}
				//for the time, if the sched starts later than the apt starts
				if(listForPeriod[i].StartTime > aptDateTime.TimeOfDay){
					continue;
				}
				//or if the sched ends (before or at same time) as the apt starts
				if(listForPeriod[i].StopTime <= aptDateTime.TimeOfDay){
					continue;
				}
				//matching sched found
				Plugins.HookAddCode(null,"Schedules.GetAssignedProvNumForSpot_Found",isSecondary);
				return listForPeriod[i].ProvNum;
			}
			//if no matching sched found, then use the operatory
			Plugins.HookAddCode(null,"Schedules.GetAssignedProvNumForSpot_None",isSecondary);
			if(isSecondary){
				return op.ProvHygienist;
			}
			else{
				return op.ProvDentist;
			}
			//return 0;//none
		}

		///<summary>Comma delimits multiple schedules and creates a nice clean sting for screen ledgibility</summary>
		public static string GetCommaDelimStringForScheds(List<Schedule> listSchedules) {
			return string.Join(",",listSchedules.Select(x => " "+x.StartTime.ToShortTimeString()+" - "+x.StopTime.ToShortTimeString()));
		}

		///<summary>Clears all blockouts for day.</summary>
		public static void ClearBlockoutsForDay(DateTime date){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),date);
				return;
			}
			//Get ScheduleNums that are to be deleted so we can delete scheduleops
			string command="SELECT ScheduleNum FROM schedule WHERE SchedDate="+POut.Date(date)+" AND SchedType="+POut.Int((int)ScheduleType.Blockout);
			List<long> listSchedNums=Db.GetListLong(command);
			if(listSchedNums.Count==0) {
				return;//nothing to delete
			}
			string schedNumStr=string.Join(",",listSchedNums.Select(x => POut.Long(x)));
			//first delete schedules
			command="DELETE FROM schedule WHERE ScheduleNum IN("+schedNumStr+")";
			Db.NonQ(command);
			//then delete scheduleops for the deleted schedules.
			command="DELETE FROM scheduleop WHERE ScheduleNum IN("+schedNumStr+")";
			Db.NonQ(command);
		}

		public static void ClearBlockoutsForOp(long opNum, DateTime dateClear) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),opNum,dateClear);
				return;
			}
			//A schedule may be attached to more than one operatory.
			List<Schedule> listSchedules = Schedules.GetForDate(dateClear);
			listSchedules.RemoveAll(x => x.SchedType!=ScheduleType.Blockout);
			//Find the sched ops that we want to delete.
			List<ScheduleOp> listSchedOps = ScheduleOps.GetForSchedList(listSchedules);
			listSchedOps.RemoveAll(x => x.OperatoryNum!=opNum);
			ScheduleOps.DeleteBatch(listSchedOps.Select(x => x.ScheduleOpNum).ToList());
			//If deleting the sched op above caused the schedule to be orphaned it should be deleted.
			Schedules.DeleteOrphanedBlockouts();
		}

		public static void ClearBlockoutsForClinic(long clinicNum,DateTime dateClear) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinicNum,dateClear);
				return;
			}
			//A schedule may be attached to more than one operatory.
			List<Schedule> listSchedules = Schedules.GetForDate(dateClear);
			listSchedules.RemoveAll(x => x.SchedType!=ScheduleType.Blockout);
			//Find the sched ops that we want to delete.
			List<long> listOpNums = Operatories.GetOpsForClinic(clinicNum).Select(x=>x.OperatoryNum).ToList();
			List<ScheduleOp> listSchedOps = ScheduleOps.GetForSchedList(listSchedules);
			listSchedOps.RemoveAll(x => !listOpNums.Contains(x.OperatoryNum));
			ScheduleOps.DeleteBatch(listSchedOps.Select(x => x.ScheduleOpNum).ToList());
			//If deleting the sched op above caused the schedule to be orphaned it should be deleted.
			Schedules.DeleteOrphanedBlockouts();
		}

		///<summary>Delete any blockout schedules that don't have any scheduleop rows.</summary>
		private static void DeleteOrphanedBlockouts() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command = "DELETE FROM schedule WHERE SchedType="+POut.Int((int)ScheduleType.Blockout)+" "
				+"AND ScheduleNum NOT IN (SELECT ScheduleNum FROM ScheduleOp)";
			Db.NonQ(command);
		}

		///<summary>Similar to GetDayList but uses Crud pattern and classes.  No need to call RefreshAndFill since this is only used for the ScheduleNums</summary>
		private static List<Schedule> GetForDate(DateTime dateClear) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),dateClear);
			}
			string command="SELECT * FROM Schedule Where SchedDate="+POut.Date(dateClear);
			return Crud.ScheduleCrud.SelectMany(command);
		}
		
		public static bool DateIsHoliday(DateTime date){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),date);
			}
			string command="SELECT COUNT(*) FROM schedule "
				//only count holiday schedules for the entire practice or for the currently selected clinic
				+"WHERE (ClinicNum=0 OR ClinicNum="+POut.Long(Clinics.ClinicNum)+") "
				+"AND Status="+POut.Int((int)SchedStatus.Holiday)+" "
				+"AND SchedType="+POut.Int((int)ScheduleType.Practice)+" "
				+"AND SchedDate="+POut.Date(date);
			string result=Db.GetCount(command);
			return result!="0";
		}

		public static List<Schedule> GetAllForDateAndType(DateTime date,ScheduleType schedType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),date,schedType);
			}
			string command="SELECT * FROM schedule WHERE SchedDate="+POut.Date(date)+" AND SchedType="+POut.Int((int)ScheduleType.Practice);
			return Crud.ScheduleCrud.SelectMany(command);
		}

		///<summary>Returns a 7 column data table in a calendar layout so all you have to do is draw it on the screen.  If includePractice is true,
		///then practice notes and holidays will be included.  If listOps is not empty, then includePractice will include only those practice notes and
		///holidays assigned to the ops in the list.  If listOps is empty, includePractice will include all practice type schedules that are not op/clinic
		///specific.</summary>
		public static DataTable GetPeriod(DateTime dateStart,DateTime dateEnd,List<long> provNums,List<long> empNums,bool includePNotes,
			bool includeCNotes,long clinicNum)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,provNums,empNums,includePNotes,includeCNotes,clinicNum);
			}
			DataTable table=new DataTable();
			DataRow row;
			table.Columns.Add("sun");
			table.Columns.Add("mon");
			table.Columns.Add("tues");
			table.Columns.Add("wed");
			table.Columns.Add("thurs");
			table.Columns.Add("fri");
			table.Columns.Add("sat");
			if(provNums.Count==0 && empNums.Count==0 && !includeCNotes && !includePNotes) {
				return table;
			}
			List<string> listOrClauses=new List<string>();
			//Only notes with clinicNum==0
			if(includePNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND schedule.ClinicNum=0)");
			}
			//Only notes with clinicNum!=0; Treats HQ/ClinicNum==0 as show all non-practice notes.
			if(includeCNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND schedule.ClinicNum"+(clinicNum==0?">0":("="+POut.Long(clinicNum)))+")");
			}
			if(provNums.Count>0) {
				listOrClauses.Add("schedule.ProvNum IN("+string.Join(",",provNums.Select(x => POut.Long(x)))+")");
			}
			if(empNums.Count>0) {
				listOrClauses.Add("schedule.EmployeeNum IN("+string.Join(",",empNums.Select(x => POut.Long(x)))+")");
			}
			string command="SELECT Abbr,employee.FName,Note,SchedDate,SchedType,Status,StartTime,StopTime,schedule.ClinicNum "
				+"FROM schedule "
				+"LEFT JOIN provider ON schedule.ProvNum=provider.ProvNum "
				+"LEFT JOIN employee ON schedule.EmployeeNum=employee.EmployeeNum "
				+"WHERE SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND ("+string.Join(" OR ",listOrClauses)+") "
				+"ORDER BY SchedDate,employee.FName,provider.ItemOrder,StartTime";
			DataTable raw=Db.GetTable(command);
			DateTime dateSched;
			DateTime startTime;
			DateTime stopTime;
			int rowsInGrid=GetRowCal(dateStart,dateEnd)+1;//because 0-based
			for(int i=0;i<rowsInGrid;i++){
				row=table.NewRow();
				table.Rows.Add(row);
			}
			dateSched=dateStart;
			while(dateSched<=dateEnd){
				table.Rows[GetRowCal(dateStart,dateSched)][(int)dateSched.DayOfWeek]=
					dateSched.ToString("MMM d, yyyy");
				dateSched=dateSched.AddDays(1);
			}
			int rowI;
			for(int i=0;i<raw.Rows.Count;i++){
				dateSched=PIn.Date(raw.Rows[i]["SchedDate"].ToString());
				startTime=PIn.DateT(raw.Rows[i]["StartTime"].ToString());
				stopTime=PIn.DateT(raw.Rows[i]["StopTime"].ToString());
				rowI=GetRowCal(dateStart,dateSched);
				if(i!=0//not first row
					&& raw.Rows[i-1]["Abbr"].ToString()==raw.Rows[i]["Abbr"].ToString()//same provider as previous row
					&& raw.Rows[i-1]["FName"].ToString()==raw.Rows[i]["FName"].ToString()//same employee as previous row
					&& raw.Rows[i-1]["SchedDate"].ToString()==raw.Rows[i]["SchedDate"].ToString())//and same date as previous row
				{
					if(startTime.TimeOfDay==PIn.DateT("12 AM").TimeOfDay
						&& stopTime.TimeOfDay==PIn.DateT("12 AM").TimeOfDay)
					{
						table.Rows[rowI][(int)dateSched.DayOfWeek]+="\r\n";//notes and holidays start on new lines
						if(raw.Rows[i]["Status"].ToString()=="2") {//if holiday
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=Lans.g("Schedules","Holiday");
						}
						else {
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=Lans.g("Schedules","Note");
						}
						if(PrefC.HasClinicsEnabled) {
							string clinicDesc=Clinics.GetDesc(PIn.Long(raw.Rows[i]["ClinicNum"].ToString()));
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=" ("+(string.IsNullOrEmpty(clinicDesc)?"Headquarters":clinicDesc)+")";
						}
						table.Rows[rowI][(int)dateSched.DayOfWeek]+=":";
					}
					else{
						table.Rows[rowI][(int)dateSched.DayOfWeek]+=", ";//other than notes and holidays, if same emp or same prov and same date separate by commas
						table.Rows[rowI][(int)dateSched.DayOfWeek]+=startTime.ToString("h:mm")+"-"+stopTime.ToString("h:mm");
					}
				}
				else{
					table.Rows[rowI][(int)dateSched.DayOfWeek]+="\r\n";
					if(startTime.TimeOfDay==PIn.DateT("12 AM").TimeOfDay
						&& stopTime.TimeOfDay==PIn.DateT("12 AM").TimeOfDay)
					{
						if(raw.Rows[i]["Status"].ToString()=="2"){//if holiday
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=Lans.g("Schedules","Holiday");
						}
						else {//note
							if(raw.Rows[i]["Abbr"].ToString()!=""){
								table.Rows[rowI][(int)dateSched.DayOfWeek]+=raw.Rows[i]["Abbr"].ToString()+" ";
							}
							if(raw.Rows[i]["FName"].ToString()!="") {
								table.Rows[rowI][(int)dateSched.DayOfWeek]+=raw.Rows[i]["FName"].ToString()+" ";
							}
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=Lans.g("Schedules","Note");
						}
						if(PrefC.HasClinicsEnabled) {
							string clinicDesc=Clinics.GetDesc(PIn.Long(raw.Rows[i]["ClinicNum"].ToString()));
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=" ("+(string.IsNullOrEmpty(clinicDesc)?"Headquarters":clinicDesc)+")";
						}
						table.Rows[rowI][(int)dateSched.DayOfWeek]+=":";
					}
					else{
						if(raw.Rows[i]["Abbr"].ToString()!="") {
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=raw.Rows[i]["Abbr"].ToString()+" ";
						}
						if(raw.Rows[i]["FName"].ToString()!="") {
							table.Rows[rowI][(int)dateSched.DayOfWeek]+=raw.Rows[i]["FName"].ToString()+" ";
						}
						table.Rows[rowI][(int)dateSched.DayOfWeek]+=
							startTime.ToString("h:mm")+"-"+stopTime.ToString("h:mm");
					}
				}
				if(raw.Rows[i]["Note"].ToString()!="") {
					table.Rows[rowI][(int)dateSched.DayOfWeek]+=" "+raw.Rows[i]["Note"].ToString();
				}
			}
			return table;
		}

		///<summary>Gets a data table that contains all schedules and blockouts that meet our Web Sched requirements.  See comments inside for more detail.</summary>
		public static DataTable GetSchedulesAndBlockoutsForWebSched(List<Provider> listProviders,DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listProviders,dateStart,dateEnd);
			}
			List<long> listProvNums=new List<long>();
			if(listProviders!=null) {
				listProvNums=listProviders.Select(x => x.ProvNum).Distinct().ToList();
			}
			if(!listProvNums.Contains(0)) {
				listProvNums.Add(0);//Always add 0 so that blockouts can be returned.
			}
			//Grab all schedules and blockouts within operatories flagged for Web Sched.
			string command=@"-- First, get all schedules associated to operatories.
				(SELECT schedule.*,operatory.OperatoryNum,operatory.ItemOrder
					FROM schedule 
					INNER JOIN scheduleop ON schedule.ScheduleNum=scheduleop.ScheduleNum
					INNER JOIN operatory ON operatory.OperatoryNum=scheduleop.OperatoryNum
					WHERE operatory.IsWebSched=1
					AND schedule.ProvNum IN ("+String.Join(",",listProvNums)+@")
					AND schedule.BlockoutType > -1 -- We need to include all blockouts and non-blockouts.
					AND "+DbHelper.DtimeToDate("schedule.SchedDate")+">="+POut.Date(dateStart)+" "
					+"AND "+DbHelper.DtimeToDate("schedule.SchedDate")+"<="+POut.Date(dateEnd)+") "
				+@"UNION -- Using UNION instead of UNION ALL because we want duplicate entries to be removed.
				-- Next, get all schedules that are not associated to any operatories
				(SELECT schedule.*,operatory.OperatoryNum,operatory.ItemOrder 
					FROM schedule
					INNER JOIN provider ON schedule.ProvNum=provider.ProvNum
					INNER JOIN operatory ON schedule.ProvNum=operatory.ProvDentist OR schedule.ProvNum=operatory.ProvHygienist
					LEFT JOIN scheduleop ON schedule.ScheduleNum=scheduleop.ScheduleNum
					WHERE provider.IsHidden!=1
					AND provider.ProvNum IN ("+String.Join(",",listProvNums)+@")
					AND operatory.IsWebSched=1
					AND schedule.BlockoutType > -1 -- We need to include all blockouts and non-blockouts.
					AND scheduleop.OperatoryNum IS NULL -- Only consider schedules that are NOT assigned to any operatories (dynamic schedules)
					AND "+DbHelper.DtimeToDate("schedule.SchedDate")+">="+POut.Date(dateStart)+" "
					+"AND "+DbHelper.DtimeToDate("schedule.SchedDate")+"<="+POut.Date(dateEnd)+") "
				+"ORDER BY SchedDate,ItemOrder";//Order the entire result set by SchedDate then by operatory ItemOrder.  This is important for speed when filtering out results and when choosing a time slot.
			return DataCore.GetTable(command);
		}

		///<summary>Returns the 0-based row where endDate will fall in a calendar grid.  It is not necessary to have a function to retrieve the column,
		///because that is simply (int)myDate.DayOfWeek</summary>
		public static int GetRowCal(DateTime startDate,DateTime endDate){
			//No need to check RemotingRole; no call to db.
			TimeSpan span=endDate-startDate;
			int dayInterval=span.Days;
			int daysFirstWeek=7-(int)startDate.DayOfWeek;//eg Monday=7-1=6.  or Sat=7-6=1.
			dayInterval=dayInterval-daysFirstWeek;
			if(dayInterval<0){
				return 0;
			}
			return (int)Math.Ceiling((dayInterval+1)/7d);
		}

		///<summary>When click on a calendar grid, this is used to calculate the date clicked on.  StartDate is the first date in the Calendar, which does
		///not have to be Sun.</summary>
		public static DateTime GetDateCal(DateTime startDate,int row,int col){
			//No need to check RemotingRole; no call to db.
			DateTime dateFirstRow;//the first date of row 0. Typically a few days before startDate. Always a Sun.
			dateFirstRow=startDate.AddDays(-(int)startDate.DayOfWeek);//example: (Tues,May 9).AddDays(-2)=Sun,May 7.
			int days=row*7+col;
			//peculiar bug.  When days=211 (startDate=4/1/10, row=30, col=1
			//and dateFirstRow=3/28/2010 and the current computer date is 4/14/10, and OS is Win7(possibly others),
			//dateFirstRow.AddDays(days)=10/24/10 00:59:58 (off by two seconds)
			//Spent hours trying to duplicate in isolated environment, but it behaves fine outside of this program.
			//Ticks are same, but result is different.
			//Commenting out the CultureInfo changes in FormOpenDental_Load did not help.
			//Not worth further debugging, so:
			DateTime retVal=dateFirstRow.AddDays(days).AddSeconds(5);
			return retVal.Date;
		}

		///<summary>Surround with try/catch.  Uses Sync to update the database with the changes made to listScheds from the stale listSchedsOld.</summary>
		public static void SetForDay(List<Schedule> listScheds,List<Schedule> listSchedsOld) {
			if(listScheds.Any(x=>x.StartTime>x.StopTime)) {
				throw new Exception(Lans.g("Schedule","Stop time must be later than start time."));
			}
			Sync(listScheds,listSchedsOld);
		}

		///<summary>Inserts, updates, or deletes the passed in listNew against the stale listOld.  Returns true if db changes were made.
		///This does not call the normal crud.Sync due to the special cases of DeletedObject and ScheduleOps.
		///This sends less data across middle teir for update logic, which is why remoting role occurs after we have filtered both lists.</summary>
		public static bool Sync(List<Schedule> listNew,List<Schedule> listOld) {
			//No call to DB yet, remoting role to be checked later.
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<Schedule> listIns = new List<Schedule>();
			List<Schedule> listUpdNew = new List<Schedule>();
			List<Schedule> listUpdDB = new List<Schedule>();
			List<Schedule> listDel = new List<Schedule>();
			listNew.Sort((Schedule x,Schedule y) => { return x.ScheduleNum.CompareTo(y.ScheduleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listOld.Sort((Schedule x,Schedule y) => { return x.ScheduleNum.CompareTo(y.ScheduleNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew = 0;
			int idxDB = 0;
			Schedule fieldNew;
			Schedule fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listOld.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listOld.Count) {
					fieldDB=listOld[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.ScheduleNum<fieldDB.ScheduleNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ScheduleNum>fieldDB.ScheduleNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//This filters out schedules that do not need to be updated, instead of relying on the update new/old pattern to filter.
				//Everything past this point needs to increment idxNew and idxDB.
				else if(Crud.ScheduleCrud.UpdateComparison(fieldNew,fieldDB) || !fieldNew.Ops.OrderBy(x=>x).SequenceEqual(fieldDB.Ops.OrderBy(x=>x)))  //if the two lists are not identical
				{
					//Both lists contain the 'next' item, update required
					listUpdNew.Add(fieldNew);
					listUpdDB.Add(fieldDB);
				}
				idxNew++;
				idxDB++;
				//There is nothing to do with this schedule?
			}
			if(listIns.Count==0 && listUpdNew.Count==0 && listUpdDB.Count==0 && listDel.Count==0) {
				return false;//No need to go through remoting role check and following code because it will do nothing
			}
			//This sync logic was split up from the typical sync logic in order to restrict payload sizes that are sent over middle tier.
			//If this method starts having issues in the future we will need to serialize the lists into DataTables to further save size.
			return SyncToDbHelper(listIns,listUpdNew,listUpdDB,listDel);
		}

		///<summary>Inserts, updates, or deletes database rows sepcified in the supplied lists.  Returns true if db changes were made.
		///This was split from the list building logic to limit the payload that needed to be sent over middle tier.</summary>
		public static bool SyncToDbHelper(List<Schedule> listIns,List<Schedule> listUpdNew,List<Schedule> listUpdDB,List<Schedule> listDel) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listIns,listUpdNew,listUpdDB,listDel);
			}
			//Commit changes to DB 
			//to foreach loops
			for(int i = 0;i<listIns.Count;i++) {
				Insert(listIns[i],false);
			}
			for(int i = 0;i<listUpdNew.Count;i++) {
				Update(listUpdNew[i],listUpdDB[i],false);
			}
			for(int i = 0;i<listDel.Count;i++) {
				Delete(listDel[i]);
			}
			if(listIns.Count>0 || listUpdNew.Count>0 || listDel.Count>0) {
				//Unlike base Sync pattern, we already know that anything in the listUpdNew should be updated.
				//filtering for update should have already been performed, otherwise the return value may be a false positive.
				return true;
			}
			return false;
		}

		///<summary>Clears all schedule entries for the given date range and the given providers, employees, and practice.</summary>
		public static void Clear(DateTime dateStart,DateTime dateEnd,List<long> provNums,List<long> empNums,bool includePNotes,bool includeCNotes,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dateStart,dateEnd,provNums,empNums,includePNotes,includeCNotes,clinicNum);
				return;
			}
			if(provNums.Count==0 && empNums.Count==0 && !includeCNotes && !includePNotes) {
				return;
			}
			List<string> listOrClauses=new List<string>();
			//Only notes with clinicNum==0
			if(includePNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum=0)");
			}
			//Only notes with clinicNum!=0; Treats HQ/ClinicNum==0 as show all non-practice notes.
			if(includeCNotes) {
				listOrClauses.Add("(SchedType="+POut.Int((int)ScheduleType.Practice)+" AND ClinicNum"+(clinicNum==0?">0":("="+POut.Long(clinicNum)))+")");
			}
			if(provNums.Count>0) {
				listOrClauses.Add("schedule.ProvNum IN("+string.Join(",",provNums.Select(x => POut.Long(x)))+")");
			}
			if(empNums.Count>0) {
				listOrClauses.Add("schedule.EmployeeNum IN("+string.Join(",",empNums.Select(x => POut.Long(x)))+")");
			}
			string command="SELECT schedule.ScheduleNum FROM schedule "
				+"WHERE SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND ("+string.Join(" OR ",listOrClauses)+")";
			List<long> listScheduleNums=Db.GetListLong(command);
			if(listScheduleNums.Count==0) {//no schedules to clear
				return;
			}
			//make deleted entries for synch purposes
			listScheduleNums.ForEach(x => DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv,x));
			command="DELETE FROM schedule WHERE ScheduleNum IN("+string.Join(",",listScheduleNums)+")";
			Db.NonQ(command);
			command="DELETE FROM scheduleop WHERE ScheduleNum IN("+string.Join(",",listScheduleNums)+")";
			Db.NonQ(command);
		}

		///<summary>Clears all Blockout schedule entries for the given date range and the given ops.</summary>
		public static void ClearBlockouts(DateTime dateStart,DateTime dateEnd,List<long> opNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dateStart,dateEnd,opNums);
				return;
			}
			string command="SELECT * FROM schedule "
				+"WHERE SchedType="+POut.Int((int)ScheduleType.Blockout)+" "
				+"AND SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd);
			List<Schedule> listScheds=RefreshAndFill(command).FindAll(x => x.Ops.Any(y => opNums.Contains(y)));
			if(listScheds.Count==0) {
				return;
			}
			List<string> listOrs=new List<string>();
			listScheds.ForEach(x => opNums.FindAll(y => x.Ops.Contains(y))//for each sched find all ops that are in the Ops list and the list to clear
				.ForEach(y => listOrs.Add("(ScheduleNum="+POut.Long(x.ScheduleNum)+" AND OperatoryNum="+POut.Long(y)+")")));//then build string list to OR
			//first delete scheduleops
			if(listOrs.Count>0) {
				command="DELETE FROM scheduleop WHERE "+string.Join(" OR ",listOrs);
				Db.NonQ(command);
			}
			//Delete any blockouts where all of the Ops for the blockout are in the list of opNums whose scheduleops we just deleted.
			DeleteOrphanedBlockouts();
		}

		public static int GetDuplicateBlockoutCount() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			System.Diagnostics.Stopwatch s=new System.Diagnostics.Stopwatch();
			s.Start();
			string command="SELECT * FROM schedule WHERE SchedType="+POut.Int((int)ScheduleType.Blockout);
			int retval=RefreshAndFill(command)
				.GroupBy(x=> new { x.SchedDate,x.StartTime,x.StopTime,ops=string.Join(",",x.Ops.OrderBy(y=>y)) })//group by duplicates
				.Sum(x=>x.Count()-1);//count duplicates, except the one original per group.
			s.Stop();
			Console.WriteLine("GetDup RefreshAndFill with group and sum, count="+retval+", elapsed time: "+s.Elapsed.TotalSeconds+" sec");
			return retval;
		}

		///<summary></summary>
		public static void ClearDuplicates() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="SELECT * FROM schedule WHERE SchedType="+POut.Int((int)ScheduleType.Blockout);
			List<long> listDupSchedNums=RefreshAndFill(command)
				.GroupBy(x => new { x.SchedDate,x.StartTime,x.StopTime,ops=string.Join(",",x.Ops.OrderBy(y=>y)) })//group by duplicates
				.Select(x => x.Skip(1)) //each group represents a set of duplicates. First time in group is the "non-duplicate"
				.SelectMany(x=>x.Select(y=>y.ScheduleNum)).ToList(); //get those with dupes
			command="DELETE FROM schedule WHERE ScheduleNum IN("+string.Join(",",listDupSchedNums)+")";
			long schedDel=Db.NonQ(command);
			command="DELETE FROM scheduleop WHERE ScheduleNum IN("+string.Join(",",listDupSchedNums)+")";
			long schedOpsDel=Db.NonQ(command);
		}

		///<summary>Set clinicNum to 0 to return 'unassigned' clinics.  Otherwise, filters the data set on the clinic num passed in.
		///Added to the DataSet in Appointments.RefreshPeriod.</summary>
		public static DataTable GetPeriodEmployeeSchedTable(DateTime dateStart,DateTime dateEnd,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,clinicNum);
			}
			DataTable table=new DataTable("EmpSched");
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("EmployeeNum");//only used to compare rows when there are multiple rows for one employee
			table.Columns.Add("empName");
			table.Columns.Add("schedule");
			table.Columns.Add("Note");
			if(dateStart!=dateEnd) {
				table.Columns.Remove("EmployeeNum");
				return table;
			}
			//Consider re-writing to use employee cache
			string command="SELECT employee.EmployeeNum,StartTime,StopTime,FName,Note,schedule.ScheduleNum "
				+"FROM employee "
				+"INNER JOIN schedule ON schedule.EmployeeNum=employee.EmployeeNum "
				+"WHERE SchedType="+POut.Int((int)ScheduleType.Employee)+" "
				+"AND SchedDate="+POut.Date(dateStart)+" "
				+"AND employee.IsHidden=0 ";
			if(PrefC.HasClinicsEnabled) {//Using clinics.
				command+="AND employee.EmployeeNum IN ("+string.Join(",",Employees.GetEmpsForClinic(clinicNum).Select(x => x.EmployeeNum))+") ";
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY schedule.ScheduleNum ";
			}
			else {
				command+="GROUP BY employee.EmployeeNum,StartTime,StopTime,FName,Note,schedule.ScheduleNum ";
			}
			//Sort by Emp num so that sort is deterministic
			command+="ORDER BY FName,LName,employee.EmployeeNum,StartTime";//order by FName for display, LName and EmployeeNum for emps with same FName
			DataTable raw=Db.GetTable(command);
			DataRow row;
			DateTime startTime;
			DateTime stopTime;
			foreach(DataRow rawRow in raw.Rows) {
				row=table.NewRow();
				row["EmployeeNum"]=rawRow["EmployeeNum"].ToString();
				if(table.Rows.Count==0 || rawRow["EmployeeNum"].ToString()!=table.Rows[table.Rows.Count-1]["EmployeeNum"].ToString()) {
					row["empName"]=rawRow["FName"].ToString();
				}
				startTime=PIn.DateT(rawRow["StartTime"].ToString());
				stopTime=PIn.DateT(rawRow["StopTime"].ToString());
				row["schedule"]=startTime.ToString("h:mm")+"-"+stopTime.ToString("h:mm");
				row["Note"]=rawRow["Note"].ToString();
				table.Rows.Add(row);
			}
			table.Columns.Remove("EmployeeNum");//Not necessary to drop this column, but it was not part of the original table when this code was refactored
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
			table.Columns.Add("ClinicNum");
			string command="SELECT schedule.ScheduleNum,SchedDate,StartTime,StopTime,SchedType,ProvNum,BlockoutType,Note,"
				+"Status,EmployeeNum,DateTStamp,schedule.ClinicNum "
				+"FROM schedule "
				+"WHERE SchedDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"ORDER BY StartTime";
			DataTable raw=Db.GetTable(command);
			Dictionary<long,string> dictSchedOps=new Dictionary<long, string>();
			if(raw.Rows.Count>0) {
				List<long> listSchedNums=raw.Rows.OfType<DataRow>().Select(x => PIn.Long(x["ScheduleNum"].ToString())).ToList();
				command="SELECT ScheduleNum,OperatoryNum FROM scheduleop WHERE ScheduleNum IN("+string.Join(",",listSchedNums)+")";
				dictSchedOps=Db.GetTable(command).Rows.OfType<DataRow>()
					.GroupBy(x => PIn.Long(x["ScheduleNum"].ToString()),x => x["OperatoryNum"].ToString())
					.ToDictionary(x => x.Key,x => string.Join(",",x.ToArray()));
			}
			DataRow row;
			foreach(DataRow rawRow in raw.Rows) {
				row=table.NewRow();
				long schedNum=PIn.Long(rawRow["ScheduleNum"].ToString());
				row["ScheduleNum"]=schedNum;
				row["SchedDate"]=POut.Date(PIn.Date(rawRow["SchedDate"].ToString()),false);
				row["StartTime"]=POut.Time(PIn.Time(rawRow["StartTime"].ToString()),false);
				row["StopTime"]=POut.Time(PIn.Time(rawRow["StopTime"].ToString()),false);
				row["SchedType"]=rawRow["SchedType"].ToString();
				row["ProvNum"]=rawRow["ProvNum"].ToString();
				row["BlockoutType"]=rawRow["BlockoutType"].ToString();
				row["Note"]=rawRow["Note"].ToString();
				row["Status"]=rawRow["Status"].ToString();
				if(dictSchedOps.ContainsKey(schedNum)) {
					row["ops"]=dictSchedOps[schedNum];
				}
				row["EmployeeNum"]=rawRow["EmployeeNum"].ToString();
				row["DateTStamp"]=POut.Date(PIn.Date(rawRow["DateTStamp"].ToString()),false);
				row["ClinicNum"]=PIn.Long(rawRow["ClinicNum"].ToString());
				table.Rows.Add(row);
			}
			return table;
		}

	}
}













