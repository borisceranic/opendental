using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>Not part of cache refresh.</summary>
	public class Tasks {
		private const long _triageTaskListNum=1697;
		private const long _redTaskDefNum=501;
		private static bool _isHQ;
		private static long _defaultTaskPriorityDefNum;

		///<summary>Only used from UI.</summary>
		public static ArrayList LastOpenList;
		///<summary>Only used from UI.  The index of the last open tab.</summary>
		public static int LastOpenGroup;
		///<summary>Only used from UI.</summary>
		public static DateTime LastOpenDate;

		///<summary>This is needed because of the extra column that is not part of the database.</summary>
		private static List<Task> TableToList(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<Task> retVal=Crud.TaskCrud.TableToList(table);
			for(int i=0;i<retVal.Count;i++) {
				if(table.Columns.Contains("IsUnread")) {
					retVal[i].IsUnread=PIn.Bool(table.Rows[i]["IsUnread"].ToString());//1 or more will result in true.
				}
				if(table.Columns.Contains("ParentDesc")) {
					retVal[i].ParentDesc=PIn.String(table.Rows[i]["ParentDesc"].ToString());
				}
				if(table.Columns.Contains("LName")
					&& table.Columns.Contains("FName")
					&& table.Columns.Contains("Preferred")
					) 
				{
					string lname=PIn.String(table.Rows[i]["LName"].ToString());
					string fname=PIn.String(table.Rows[i]["FName"].ToString());
					string preferred=PIn.String(table.Rows[i]["Preferred"].ToString());
					retVal[i].PatientName=Patients.GetNameLF(lname,fname,preferred,"");
				}
			}
			return retVal;
		}

		/*
		///<summary>There are NO tasks on the user trunk, so this is not needed.</summary>
		public static List<Task> RefreshUserTrunk(int userNum) {
			string command="SELECT task.* FROM tasksubscription "
				+"LEFT JOIN task ON task.TaskNum=tasksubscription.TaskNum "
				+"WHERE tasksubscription.UserNum="+POut.PInt(userNum)
				+" AND tasksubscription.TaskNum!=0 "
				+"ORDER BY DateTimeEntry";
			return RefreshAndFill(command);
		}*/

		///<summary>Gets one Task from database.</summary>
		public static Task GetOne(long TaskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Task>(MethodBase.GetCurrentMethod(),TaskNum);
			}
			string command="SELECT * FROM task WHERE TaskNum = "+POut.Long(TaskNum);
			return Crud.TaskCrud.SelectOne(command);
		}

		///<summary>Gets all tasks for the Task Search function, limited to 50 by default.</summary>
		public static DataTable GetDataSet(long userNum,List<long> listTaskListNums,long taskNum,string taskDateCreatedFrom,string taskDateCreatedTo,
			string taskDateCompletedFrom,string taskDateCompletedTo,string taskDescription,long taskPriorityNum,long patNum,bool limit)
		{
			List<string> listWhereClauses=new List<string>();
			List<string> listWhereNoteClauses=new List<string>();
			DateTime dateCreatedFrom=PIn.Date(taskDateCreatedFrom);//will be DateTime.MinValue if not set, i.e. if " "
			DateTime dateCreatedTo=PIn.Date(taskDateCreatedTo);//will be DateTime.MinValue if not set, i.e. if " "
			DateTime dateCompletedFrom=PIn.Date(taskDateCompletedFrom);//will be DateTime.MinValue if not set, i.e. if " "
			DateTime dateCompletedTo=PIn.Date(taskDateCompletedTo);//will be DateTime.MinValue if not set, i.e. if " "
			if(userNum!=0) {
				listWhereClauses.Add("task.UserNum="+POut.Long(userNum));
				listWhereNoteClauses.Add("tasknote.UserNum="+POut.Long(userNum));
			}
			if(listTaskListNums.Count>0) {
				listWhereClauses.Add("task.TaskListNum IN("+string.Join(",",listTaskListNums)+")");
			}
			if(taskNum!=0) {
				listWhereClauses.Add("task.TaskNum="+POut.Long(taskNum));
				listWhereNoteClauses.Add("tasknote.TaskNum="+POut.Long(taskNum));
			}
			//Note: DateTime strings that are empty actually are " " due to how the empty datetime control behaves.
			if(dateCreatedFrom>DateTime.MinValue) {
				listWhereClauses.Add("DATE(task.DateTimeEntry)>="+POut.Date(dateCreatedFrom));
				listWhereNoteClauses.Add("DATE(tasknote.DateTimeNote)>="+POut.Date(dateCreatedFrom));
			}
			if(dateCreatedTo>DateTime.MinValue) {
				listWhereClauses.Add("DATE(task.DateTimeEntry)<="+POut.Date(dateCreatedTo));
				listWhereNoteClauses.Add("DATE(tasknote.DateTimeNote)<="+POut.Date(dateCreatedTo));
			}
			if(dateCompletedFrom>DateTime.MinValue) {
				listWhereClauses.Add("DATE(task.DateTimeFinished)>="+POut.Date(dateCompletedFrom));
			}
			if(dateCompletedTo>DateTime.MinValue) {
				listWhereClauses.Add("DATE(task.DateTimeFinished)<="+POut.Date(dateCompletedTo));
			}
			if(taskDescription!="") {
				listWhereClauses.Add("task.Descript LIKE '%"+POut.String(taskDescription)+"%'");
				listWhereNoteClauses.Add("tasknote.Note LIKE '%"+POut.String(taskDescription)+"%'");
			}
			if(taskPriorityNum!=0) {
				listWhereClauses.Add("task.PriorityDefNum="+POut.Long(taskPriorityNum));
			}
			if(patNum!=0) {
				listWhereClauses.Add("task.ObjectType="+POut.Int((int)TaskObjectType.Patient));
				listWhereClauses.Add("task.KeyNum="+POut.Long(patNum));
			}
			string whereClause="";
			if(listWhereClauses.Count>0) {
				whereClause="WHERE "+string.Join(" AND ",listWhereClauses)+" ";
			}
			string whereNoteClause="";
			if(listWhereNoteClauses.Count>0) {
				whereNoteClause="WHERE "+string.Join(" AND ",listWhereNoteClauses)+" ";
			}
			//First Data set from Task, Unioned with...
			string command="(SELECT task.TaskNum AS TaskNum "
				+"FROM task "
				+whereClause
				+"ORDER BY task.DateTimeEntry DESC";
			if(limit) {
				command+=" LIMIT 50";
			}
			command+=")";
			//Second Data set from TaskNote
			if((whereClause!="" && whereNoteClause!="") || (whereClause=="" && whereNoteClause=="")) {
				command+=" UNION "
				+"(SELECT taskNote.TaskNum AS TaskNum "
				+"FROM tasknote "
				+whereNoteClause
				+"ORDER BY tasknote.DateTimeNote DESC";
				if(limit) {
					command+=" LIMIT 50";
				}
				command+=")";
			}
			List<long> listTaskNums=Db.GetListLong(command);
			DataTable table=new DataTable();
			table.Columns.Add(new DataColumn("description"));
			table.Columns.Add(new DataColumn("note"));
			table.Columns.Add(new DataColumn("PatNum"));
			table.Columns.Add(new DataColumn("procTime"));
			table.Columns.Add(new DataColumn("dateCreate"));
			table.Columns.Add(new DataColumn("dateComplete"));
			table.Columns.Add(new DataColumn("TaskNum"));
			table.Columns.Add(new DataColumn("color"));
			if(listTaskNums.Count==0) {
				return table;//empty table with correct structure.
			}
			//listTaskNums contains too many items. Tasks found from matching task notes must be filtered too. (This prevents a costly join in the query.)
			List<Task> listTasks=Tasks.GetMany(listTaskNums)//All tasks for the notes and tasks
				.FindAll(x => listTaskListNums.Count==0 || listTaskListNums.Contains(x.TaskListNum))//filter by TaskListNum, if neccesary
				.FindAll(x => taskPriorityNum==0 || taskPriorityNum==x.PriorityDefNum)//filter by priority, if neccesary
				.FindAll(x => patNum==0 || (x.ObjectType==TaskObjectType.Patient && x.KeyNum==patNum))//filter by patnum, if neccesary
				.FindAll(x => dateCompletedFrom==DateTime.MinValue || x.DateTimeFinished.Date>=dateCompletedFrom.Date)//filter by dateFrom, if neccesary
				.FindAll(x => dateCompletedTo==DateTime.MinValue || x.DateTimeFinished.Date<=dateCompletedTo.Date)
				.OrderByDescending(x=> x.DateTimeEntry).ToList();//Order results
			List<TaskNote> listTaskNotes=TaskNotes.RefreshForTasks(listTaskNums);//All notes for the tasks.	(Ordered by dateTime)		
			int textColor=DefC.GetColor(DefCat.ProgNoteColors,DefC.GetList(DefCat.ProgNoteColors)[18].DefNum).ToArgb();//18="Patient Note Text"
			int textCompletedColor=DefC.GetColor(DefCat.ProgNoteColors,DefC.GetList(DefCat.ProgNoteColors)[20].DefNum).ToArgb();//20="Completed Pt Note Text"
			string txt;
			DataRow row;
			foreach(Task taskCur in listTasks) {
				txt="";
				row=table.NewRow();
				//Build data row
				if(taskCur.TaskStatus==TaskStatusEnum.Done) {
					row["color"]=textCompletedColor;
					txt+=Lans.g("TaskSearch","Completed")+" ";
				}
				else {
					row["color"]=textColor;
				}
				if(taskCur.TaskListNum!=0) {
					row["description"]=txt+Lans.g("TaskSearch","In List")+": "+TaskLists.GetFullPath(taskCur.TaskListNum);
				}
				else {
					row["description"]=txt+Lans.g("TaskSearch","Not in list");
				}
				txt="";
				if(!taskCur.Descript.StartsWith("==") && taskCur.UserNum!=0) {
					txt+=Userods.GetName(PIn.Long(taskCur.UserNum.ToString()))+" - ";
				}
				txt+=taskCur.Descript;
				listTaskNotes.FindAll(x => x.TaskNum==taskCur.TaskNum)
					.ForEach(x => txt+="\r\n"//even on the first loop
						+"=="+Userods.GetName(x.UserNum)+" - "
						+x.DateTimeNote.ToShortDateString()+" "
						+x.DateTimeNote.ToShortTimeString()
						+" - "+x.Note);
				row["note"]=txt;
				if(taskCur.ObjectType==TaskObjectType.Patient) {
					row["PatNum"]=taskCur.KeyNum;
				}
				if(taskCur.DateTask.Year>1880) {//check if due date set for task or note
					row["dateCreate"]=taskCur.DateTask.ToString(Lans.GetShortDateTimeFormat());
				}
				else if(taskCur.DateTimeEntry.Year>1880) {//since dateT was just redefined, check it now
					row["dateCreate"]=taskCur.DateTimeEntry.ToShortDateString();
				}
				if(taskCur.DateTask.TimeOfDay!=TimeSpan.Zero) {
					row["procTime"]=taskCur.DateTask.ToString("h:mm")+taskCur.DateTask.ToString("%t").ToLower();
				}
				else if(taskCur.DateTimeEntry.TimeOfDay!=TimeSpan.Zero) {
					row["procTime"]=taskCur.DateTimeEntry.ToString("h:mm")+taskCur.DateTimeEntry.ToString("%t").ToLower();
				}
				if(taskCur.DateTimeFinished.Year>1880) {
					row["dateComplete"]=taskCur.DateTimeFinished.ToString(Lans.GetShortDateTimeFormat());
				}
				row["TaskNum"]=taskCur.TaskNum;
				table.Rows.Add(row);
			}
			return table;
		}

		///<summary>Gets all tasks for a supplied list of task nums.</summary>
		public static List<Task> GetMany(List<long> listTaskNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),listTaskNums);
			}
			string command="SELECT * FROM task WHERE TaskNum IN("+String.Join(",",listTaskNums)+") ORDER BY DateTimeEntry";
			return Crud.TaskCrud.SelectMany(command);
		}

		///<summary>Gets all tasks for the main trunk.</summary>
		public static List<Task> RefreshMainTrunk(bool showDone,DateTime startDate,long currentUserNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),showDone,startDate,currentUserNum);
			}
			//startDate only applies if showing Done tasks.
			string command="SELECT task.*,"
					+"(SELECT COUNT(*) FROM taskunread WHERE task.TaskNum=taskunread.TaskNum AND taskunread.UserNum="+POut.Long(currentUserNum)+") IsUnread, "
					+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"WHERE TaskListNum=0 "
				+"AND DateTask < "+POut.Date(new DateTime(1880,01,01))+" "
				+"AND IsRepeating=0";
			if(showDone){
				command+=" AND (TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")";//of if done, then restrict date
			}
			else{
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			return TableToList(table);
		}

		///<summary>Gets all 'new' tasks for a user.</summary>
		public static List<Task> RefreshUserNew(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command="SELECT task.*,1 AS IsUnread,";//we fill the IsUnread column with 1's because we already know that they are all unread
			}
			else {//Oracle
				//Since this statement has a GROUP BY clause and the table has a clob column, we have to do some Oracle magic with the descript column.
				command="SELECT task.TaskNum,task.TaskListNum,task.DateTask,task.KeyNum,(SELECT Descript FROM task taskdesc WHERE task.TaskNum=taskdesc.TaskNum) Descript,task.TaskStatus"
					+",task.IsRepeating,task.DateType,task.FromNum,task.ObjectType,task.DateTimeEntry,task.UserNum,task.DateTimeFinished"
					+",1 AS IsUnread,";//we fill the IsUnread column with 1's because we already know that they are all unread
			}
			command+="tasklist.Descript ParentDesc, "	/*Renamed to keep same column name as old query*/
					+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"INNER JOIN taskunread ON task.TaskNum=taskunread.TaskNum "
					+"AND taskunread.UserNum = "+POut.Long(userNum)+" "
				+"LEFT JOIN tasklist ON task.TaskListNum=tasklist.TaskListNum "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum "
					+"AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY task.TaskNum ";//in case there are duplicate unreads
			}
			else {//Oracle
				//in case there are duplicate unreads
				command+="GROUP BY task.TaskNum,task.TaskListNum,task.DateTask,task.KeyNum,task.TaskStatus,task.IsRepeating"
					+",task.DateType,task.FromNum,task.ObjectType,task.DateTimeEntry,task.UserNum,task.DateTimeFinished "
					+",tasklist.Descript, patient.LName, patient.FName, patient.Preferred ";
			}
			command+="ORDER BY task.DateTimeEntry";
			DataTable table=Db.GetTable(command);
			List<DataRow> listRows=new List<DataRow>();
			for(int i=0;i<table.Rows.Count;i++) {
				listRows.Add(table.Rows[i]);
			}
			#region Set Sort Variables. This greatly increases sort speed.
			_isHQ=PrefC.GetBool(PrefName.DockPhonePanelShow);//increases speed of the sort function performed below.
			List<Def> listTaskPriorities=new List<Def>();
			listTaskPriorities.AddRange(DefC.GetList(DefCat.TaskPriorities));
			for(int i=0;i<listTaskPriorities.Count;i++) {
				if(listTaskPriorities[i].ItemValue.ToUpper()=="D") {
					_defaultTaskPriorityDefNum=listTaskPriorities[i].DefNum;
					break;
				}
			}
			#endregion
			listRows.Sort(TaskComparer);
			DataTable tableSorted=table.Clone();//Easy way to copy the columns.
			tableSorted.Rows.Clear();
			for(int i=0;i<listRows.Count;i++) {
				tableSorted.Rows.Add(listRows[i].ItemArray);
			}
			List<Task> listTasks=TableToList(tableSorted);
			return listTasks;
		}

		///<summary>Gets all 'open ticket' tasks for a user.  An open ticket is a task that was created by this user, is attached to a patient, and is not done.</summary>
		public static List<Task> RefreshOpenTickets(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT task.*, "
				+"(SELECT COUNT(*) FROM taskunread WHERE task.TaskNum=taskunread.TaskNum "
				+"AND taskunread.UserNum="+POut.Long(userNum)+") AS IsUnread, "
				+"tasklist.Descript AS ParentDesc, "
				+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"LEFT JOIN tasklist ON task.TaskListNum=tasklist.TaskListNum "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum "
					+"AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"WHERE NOT EXISTS( "
					+"SELECT * FROM taskancestor "
					+"LEFT JOIN tasklist ON tasklist.TaskListNum=taskancestor.TaskListNum "
					+"WHERE taskancestor.TaskNum=task.TaskNum "
					+"AND tasklist.DateType!=0) "//if any ancestor is a dated list, then we don't want that task
				+"AND task.DateType=0 "//this only handles tasks directly in the dated trunks
				+"AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"AND task.IsRepeating=0 "
				+"AND task.UserNum="+POut.Long(userNum)+" "
				+"AND TaskStatus!="+POut.Int((int)TaskStatusEnum.Done)+" "
				+"ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			return TableToList(table);
		}

		///<summary>Gets all tasks for the repeating trunk.  Always includes "done".</summary>
		public static List<Task> RefreshRepeatingTrunk() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT task.*, "
				+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"WHERE TaskListNum=0 "
				+"AND DateTask < "+POut.Date(new DateTime(1880,01,01))+" "
				+"AND IsRepeating=1 "
				+"ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			return TableToList(table);
		}

		///<summary>0 is not allowed, because that would be a trunk.  Also, if this is in someone's inbox, then pass in the userNum whose inbox it is in.  If not in an inbox, pass in 0.</summary>
		public static List<Task> RefreshChildren(long listNum,bool showDone,DateTime startDate,long currentUserNum,long userNumInbox) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),listNum,showDone,startDate,currentUserNum,userNumInbox);
			}
			//startDate only applies if showing Done tasks.
			string command=
				"SELECT task.*, "
				+"(SELECT COUNT(*) FROM taskunread WHERE task.TaskNum=taskunread.TaskNum ";//the count turns into a bool
			//if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//we don't bother with this.  Always get IsUnread
			//if a task is someone's inbox,
			if(userNumInbox>0) {
				//then restrict by that user
				command+="AND taskunread.UserNum="+POut.Long(userNumInbox)+") IsUnread, ";
			}
			else {
				//otherwise, restrict by current user
				command+="AND taskunread.UserNum="+POut.Long(currentUserNum)+") IsUnread, ";
			}
			command+="patient.LName,patient.FName,patient.Preferred, "
							+"COALESCE(MAX(tasknote.DateTimeNote),task.DateTimeEntry) AS 'LastUpdated',"
							+"CASE WHEN tasknote.TaskNoteNum IS NULL THEN 0 ELSE 1 END AS 'HasNotes' "
							+"FROM task "
							+"LEFT JOIN patient ON task.KeyNum=patient.PatNum AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
							+"LEFT JOIN tasknote ON task.TaskNum=tasknote.TaskNum "
							+"WHERE TaskListNum="+POut.Long(listNum);
			if(showDone) {
				command+=" AND ((TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")" //or if done, then restrict date
					+" OR DateTimeFinished = '0001-01-01 00:00:00')"; //Include tasks that have a finished date time as MinValue so they can be edited.
			}
			else {
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" GROUP BY task.TaskNum "//Sorting happens below
							+" ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			List<Task> taskList=new List<Task>();
			//Note: Only used for HQ, Oracle does not matter.
			List<DataRow> listRows=new List<DataRow>();
			for(int i=0;i<table.Rows.Count;i++) {
				listRows.Add(table.Rows[i]);
			}
			#region Set Sort Variables. This greatly increases sort speed.
			_isHQ=PrefC.GetBool(PrefName.DockPhonePanelShow);//increases speed of the sort function performed below.
			List<Def> listTaskPriorities=new List<Def>();
			listTaskPriorities.AddRange(DefC.GetList(DefCat.TaskPriorities));
			for(int i=0;i<listTaskPriorities.Count;i++) {
				if(listTaskPriorities[i].ItemValue.ToUpper()=="D") {
					_defaultTaskPriorityDefNum=listTaskPriorities[i].DefNum;
					break;
				}
			}
			#endregion
			listRows.Sort(TaskComparer);
			DataTable tableSorted=table.Clone();//Easy way to copy the columns.
			tableSorted.Rows.Clear();
			for(int i=0;i<listRows.Count;i++) {
				tableSorted.Rows.Add(listRows[i].ItemArray);
			}
			taskList=TableToList(tableSorted);
			return taskList;
		}

		///<summary>All repeating items for one date type with no heirarchy.</summary>
		public static List<Task> RefreshRepeating(TaskDateType dateType,long currentUserNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),dateType,currentUserNum);
			}
			string command=
				"SELECT task.*, "
				+"(SELECT COUNT(*) FROM taskunread WHERE task.TaskNum=taskunread.TaskNum "
					+"AND taskunread.UserNum="+POut.Long(currentUserNum)+") IsUnread, "//Not sure if this makes sense here
				+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"WHERE IsRepeating=1 "
				+"AND DateType="+POut.Long((int)dateType)+" "
				+"ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			return TableToList(table);
		}

		///<summary>Gets all tasks for one of the 3 dated trunks. startDate only applies if showing Done.</summary>
		public static List<Task> RefreshDatedTrunk(DateTime date,TaskDateType dateType,bool showDone,DateTime startDate,long currentUserNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),date,dateType,showDone,startDate,currentUserNum);
			}
			DateTime dateFrom=DateTime.MinValue;
			DateTime dateTo=DateTime.MaxValue;
			if(dateType==TaskDateType.Day) {
				dateFrom=date;
				dateTo=date;
			}
			else if(dateType==TaskDateType.Week) {
				dateFrom=date.AddDays(-(int)date.DayOfWeek);
				dateTo=dateFrom.AddDays(6);
			}
			else if(dateType==TaskDateType.Month) {
				dateFrom=new DateTime(date.Year,date.Month,1);
				dateTo=dateFrom.AddMonths(1).AddDays(-1);
			}
			string command=
				"SELECT task.*, "
				+"(SELECT COUNT(*) FROM taskunread WHERE task.TaskNum=taskunread.TaskNum "
					+"AND taskunread.UserNum="+POut.Long(currentUserNum)+") IsUnread, "//Not sure if this makes sense here
				+"patient.LName,patient.FName,patient.Preferred "
				+"FROM task "
				+"LEFT JOIN patient ON task.KeyNum=patient.PatNum AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"WHERE DateTask >= "+POut.Date(dateFrom)
				+" AND DateTask <= "+POut.Date(dateTo)
				+" AND DateType="+POut.Long((int)dateType);
			if(showDone){
				command+=" AND (TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")";//of if done, then restrict date
			}
			else{
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" ORDER BY DateTimeEntry";
			DataTable table=Db.GetTable(command);
			return TableToList(table);
		}

		///<summary>The full refresh is only used once when first synching all the tasks for taskAncestors.</summary>
		public static List<Task> RefreshAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM task WHERE TaskListNum != 0";
			return Crud.TaskCrud.SelectMany(command);
		}

		/*
		public static List<Task> RefreshAndFill(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Task> retVal=new List<Task>();
			Task task;
			for(int i=0;i<table.Rows.Count;i++) {
				task=new Task();
				task.TaskNum        = PIn.Long(table.Rows[i][0].ToString());
				task.TaskListNum    = PIn.Long(table.Rows[i][1].ToString());
				task.DateTask       = PIn.Date(table.Rows[i][2].ToString());
				task.KeyNum         = PIn.Long(table.Rows[i][3].ToString());
				task.Descript       = PIn.String(table.Rows[i][4].ToString());
				task.TaskStatus     = (TaskStatusEnum)PIn.Long(table.Rows[i][5].ToString());
				task.IsRepeating    = PIn.Bool(table.Rows[i][6].ToString());
				task.DateType       = (TaskDateType)PIn.Long(table.Rows[i][7].ToString());
				task.FromNum        = PIn.Long(table.Rows[i][8].ToString());
				task.ObjectType     = (TaskObjectType)PIn.Long(table.Rows[i][9].ToString());
				task.DateTimeEntry  = PIn.DateT(table.Rows[i][10].ToString());
				task.UserNum        = PIn.Long(table.Rows[i][11].ToString());
				task.DateTimeFinished= PIn.DateT(table.Rows[i][12].ToString());
				retVal.Add(task);
			}
			return retVal;
		}*/

		///<summary>Surround with try/catch.  Must supply the supposedly unaltered oldTask.  Will throw an exception if oldTask does not exactly match the database state.  Keeps users from overwriting each other's changes.</summary>
		public static void Update(Task task,Task oldTask){
			//No need to check RemotingRole; no call to db.
			Validate(task,oldTask);//No try/catch here, we want the exception to be thrown back to the calling form.
			Update(task);
			if(task.TaskListNum!=oldTask.TaskListNum) {
				TaskAncestors.Synch(task);
			}
		}

		public static void Validate(Task task,Task oldTask) {
			//No need to check RemotingRole; no call to db.
			if(task.IsRepeating && task.DateTask.Year>1880) {
				throw new Exception(Lans.g("Tasks","Task cannot be tagged repeating and also have a date."));
			}
			if(task.IsRepeating && task.TaskStatus!=TaskStatusEnum.New) {//and any status but new
				throw new Exception(Lans.g("Tasks","Tasks that are repeating must have a status of New."));
			}
			if(task.IsRepeating && task.TaskListNum!=0 && task.DateType!=TaskDateType.None) {//In repeating, children not allowed to repeat.
				throw new Exception(Lans.g("Tasks","In repeating tasks, only the main parents can have a task status."));
			}
			if(WasTaskAltered(oldTask)){
				throw new Exception(Lans.g("Tasks","Not allowed to save changes because the task has been altered by someone else."));
			}
			if(task.IsNew) {
				TaskEditCreateLog(Lans.g("Tasks","New task added"),task);
				task.IsNew=false;
			}
			else {
				if(task.TaskStatus!=oldTask.TaskStatus) {
					if(task.TaskStatus==TaskStatusEnum.Done) {
						TaskEditCreateLog(Lans.g("Tasks","Task marked done"),task);
					}
					if(task.TaskStatus==TaskStatusEnum.New) {
						TaskEditCreateLog(Lans.g("Tasks","Task marked new"),task);
					}
					//Nothing for case when Not New and Not Done. Put here in future is wanted
				}
				if(task.Descript!=oldTask.Descript) {
					TaskEditCreateLog(Lans.g("Tasks","Task description edited"),task);
				}
				if(task.UserNum!=oldTask.UserNum) {
					TaskEditCreateLog(Lans.g("Tasks","Changed user from")+" "+Userods.GetName(oldTask.UserNum),task);//+" To "+Userods.GetName(task.UserNum)),task);
				}
				if(task.KeyNum!=oldTask.KeyNum) {//We know at this point that SOMETHING with the task association changed.
					Patient patOld=null;
					Patient patNew=null;
					string log="";
					#region Old Task Object Type
					if(oldTask.KeyNum > 0) {//Old task had a patient/appointment
						if(oldTask.ObjectType==TaskObjectType.Patient) {//It was a patient
							patOld=Patients.GetLim(oldTask.KeyNum);
							log+=Lans.g("Tasks","Task object type changed from patient")+" "+patOld.GetNameFL()+" ";
						}
						else {//It was an appointment
							log+=Lans.g("Tasks","Task object type changed from appointment for")+" ";
							Appointment aptOld=Appointments.GetOneApt(oldTask.KeyNum);
							patOld=Patients.GetLim(aptOld.PatNum);
							if(aptOld==null) {
								log+=Lans.g("Tasks","(appointment deleted)")+" ";
							}
							else {
								log+=Lans.g("Tasks",patOld.GetNameLF()
									+"  "+aptOld.AptDateTime.ToString()
									+"  "+aptOld.ProcDescript+" ");
							}
						}
					}
					else {//Old task had "None"
						log+=Lans.g("Tasks","Task object type changed from none")+" ";
					}
					#endregion
					#region New Task Object Type
					if(task.KeyNum > 0) {//New task has a patient/appointment
						if(task.ObjectType==TaskObjectType.Patient) {//It was a patient
							patNew=Patients.GetLim(task.KeyNum);
							log+=Lans.g("Tasks","to object type patient")+" "+patNew.GetNameFL();
						}
						else {//It was an appointment
							log+=Lans.g("Tasks","to object type appointment for")+" ";
							Appointment aptNew=Appointments.GetOneApt(task.KeyNum);
							patNew=Patients.GetLim(aptNew.PatNum);
							if(aptNew==null) {
								log+=Lans.g("Tasks","(appointment deleted)");
							}
							else {
								log+=Lans.g("Tasks",patNew.GetNameLF()
									+"  "+aptNew.AptDateTime.ToString()
									+"  "+aptNew.ProcDescript);
							}
						}
					}
					else {
						log+=Lans.g("Tasks","to object type none.");
					}
					#endregion
					//Make a log depending on what happened with the object type association.
					TaskEditCreateLog(log,task);
				}
				if(task.TaskListNum!=oldTask.TaskListNum && oldTask.TaskListNum==0) {
					TaskEditCreateLog(Lans.g("Tasks","Task moved from Main"),task);
				}
				else if(task.TaskListNum!=oldTask.TaskListNum) {
					TaskEditCreateLog(Lans.g("Tasks","Task moved from")+" "+TaskLists.GetOne(oldTask.TaskListNum).Descript,task);
				}
			}
		}

		///<summary>This update method doesn't do any of the typical checks for the Task update.  Not used normally.</summary>
		public static void Update(Task task) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),task);
				return;
			}
			Crud.TaskCrud.Update(task);
		}

		///<summary></summary>
		public static long Insert(Task task) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				task.TaskNum=Meth.GetLong(MethodBase.GetCurrentMethod(),task);
				return task.TaskNum;
			}
			if(task.IsRepeating && task.DateTask.Year>1880) {
				throw new Exception(Lans.g("Tasks","Task cannot be tagged repeating and also have a date."));
			}
			if(task.IsRepeating && task.TaskStatus!=TaskStatusEnum.New) {//and any status but new
				throw new Exception(Lans.g("Tasks","Tasks that are repeating must have a status of New."));
			}
			if(task.IsRepeating && task.TaskListNum!=0 && task.DateType!=TaskDateType.None) {//In repeating, children not allowed to repeat.
				throw new Exception(Lans.g("Tasks","In repeating tasks, only the main parents can have a task status."));
			}
			Crud.TaskCrud.Insert(task);
			TaskAncestors.Synch(task);
			return task.TaskNum;
		}

		///<summary></summary>
		public static bool WasTaskAltered(Task task){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),task);
			}
			string command="SELECT * FROM task WHERE TaskNum="+POut.Long(task.TaskNum);
			Task oldtask=Crud.TaskCrud.SelectOne(command);
			if(oldtask==null
				|| oldtask.DateTask!=task.DateTask
				|| oldtask.DateType!=task.DateType
				|| oldtask.Descript!=task.Descript
				|| oldtask.FromNum!=task.FromNum
				|| oldtask.IsRepeating!=task.IsRepeating
				|| oldtask.KeyNum!=task.KeyNum
				|| oldtask.ObjectType!=task.ObjectType
				|| oldtask.TaskListNum!=task.TaskListNum
				|| oldtask.TaskStatus!=task.TaskStatus
				|| oldtask.UserNum!=task.UserNum
				|| oldtask.DateTimeEntry!=task.DateTimeEntry
				|| oldtask.DateTimeFinished!=task.DateTimeFinished)
			{
				return true;
			}
			return false;
		}

		///<summary>Deleting a task never causes a problem, so no dependencies are checked.</summary>
		public static void Delete(long taskNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum);
				return;
			}
			Tasks.ClearFkey(taskNum);//Zero securitylog FKey column for rows to be deleted.
			string command= "DELETE FROM task WHERE TaskNum = "+POut.Long(taskNum);
 			Db.NonQ(command);
			command="DELETE FROM taskancestor WHERE TaskNum = "+POut.Long(taskNum);
			Db.NonQ(command);
			command="DELETE FROM tasknote WHERE TaskNum = "+POut.Long(taskNum);
			Db.NonQ(command);
			command="DELETE FROM taskunread WHERE TaskNum = "+POut.Long(taskNum);
			Db.NonQ(command);
			//Remove all references from the joblink table for HQ only.
			if(Prefs.GetBoolNoCache(PrefName.DockPhonePanelShow)) {
				command="DELETE FROM joblink "
					+"WHERE FKey = "+POut.Long(taskNum)+" "
					+"AND LinkType = "+POut.Int((int)JobLinkType.Task);
				Db.NonQ(command);
			}
		}

		///<summary>Gets a count of unread tasks to notify user when first logging in.</summary>
		public static int UserTasksCount(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT task.TaskNum FROM task,taskunread "
				+"WHERE task.TaskNum=taskunread.TaskNum "
				+"AND taskunread.UserNum = "+POut.Long(userNum)+" "
				+"GROUP BY task.TaskNum";//this handles duplicate taskunread entries.
			/*
			string command="SELECT COUNT(*) FROM taskancestor,task,tasklist,tasksubscription "
				+"WHERE taskancestor.TaskListNum=tasklist.TaskListNum "
				+"AND task.TaskNum=taskancestor.TaskNum "
				+"AND tasksubscription.TaskListNum=tasklist.TaskListNum "
				+"AND tasksubscription.UserNum="+POut.Long(userNum)
				+" AND task.TaskStatus="+POut.Long((int)TaskStatusEnum.New);*/
			DataTable table=Db.GetTable(command);
			return table.Rows.Count;
			//return PIn.Int(Db.GetScalar(command));//GetCount failed if no new tasks.
		}

		/*
		///<summary>Appends a carriage return as well as the text to any task.  If a taskListNum is specified, then it also changes the taskList.</summary>
		public static void Append(long taskNum,string text) {
			//No need to check RemotingRole; no call to db.
			Append(taskNum,text,-1);
		}

		///<summary>Appends a carriage return as well as the text to any task.  If a taskListNum is specified, then it also changes the taskList.    Must call TaskAncestors.Synch after this.</summary>
		public static void Append(long taskNum,string text,long taskListNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum,text,taskListNum);
				return;
			}
			string command;
			if(taskListNum==-1) {
				command="UPDATE task SET Descript=CONCAT(Descript,'"+POut.String("\r\n"+text)+"') WHERE TaskNum="+POut.Long(taskNum);
			}
			else {
				command="UPDATE task SET Descript=CONCAT(Descript,'"+POut.String("\r\n"+text)+"'), "
					+"TaskListNum="+POut.Long(taskListNum)+" "
					+"WHERE TaskNum="+POut.Long(taskNum);
			}
			Db.NonQ(command);
		}*/

		public static int GetCountOpenTickets(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT COUNT(*) "
				+"FROM task "
				+"WHERE NOT EXISTS(SELECT * FROM taskancestor,tasklist "
				+"WHERE taskancestor.TaskNum=task.TaskNum "
				+"AND tasklist.TaskListNum=taskancestor.TaskListNum "
				+"AND tasklist.DateType!=0) "//if any ancestor is a dated list, then we don't want that task
				+"AND task.DateType=0 "//this only handles tasks directly in the dated trunks
				+"AND task.ObjectType="+POut.Int((int)TaskObjectType.Patient)+" "
				+"AND task.IsRepeating=0 "
				+"AND task.UserNum="+POut.Long(userNum)+" "
				+"AND TaskStatus != "+POut.Int((int)TaskStatusEnum.Done);
			return PIn.Int(Db.GetCount(command));
		}

		public static void TaskEditCreateLog(string logText,Task task) {
			TaskEditCreateLog(Permissions.TaskEdit,logText,task);
		}

		///<summary>Makes audit trail entry for the task passed in.
		///If this task has an object type set, the log will show up under the corresponding patient for the selected object type.
		///Used for both TaskEdit and TaskNoteEdit permissions.</summary>
		public static void TaskEditCreateLog(Permissions perm,string logText,Task task) {
			if(task==null) {  //Something went wrong before calling this function, and somehow task wasn't passed in
				//Do nothing.  This was added because in very intermittent situations this function would throw a UE and crash OD.
				//	this is just a simple securitylog entry so it is fine to skip it in this case.  We should try to solve the issues
				//	causing null to be passed in, but we should not let this throw a UE.
				return;
			}
			long patNum=0;//Task type of none defaults to 0.
			if(task.KeyNum!=0) {  //Either no object attached, or object hasn't been commited to db yet (Changed the object but haven't clicked OK on TaskEdit).
				if(task.ObjectType==TaskObjectType.Patient) {//Task type of patient we can use the task.KeyNum for patNum
					patNum=task.KeyNum;
				}
				else if(task.ObjectType==TaskObjectType.Appointment) {//Task type of appointment we have to look up the patient from the apt.
					Appointment AptCur=Appointments.GetOneApt(task.KeyNum);
					patNum=AptCur.PatNum;
				}
			}
			SecurityLogs.MakeLogEntry(perm,patNum,logText,task.TaskNum);
		}

		///<summary>Sorted in Ascending order: Unread/Read, </summary>
		public static int TaskComparer(DataRow x,DataRow y) {
			//1)Sort by IsUnread status
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
				if(x["IsUnread"].ToString()!=y["IsUnread"].ToString()) {
					//Note: we are returning the negative of x.CompareTo(y)
					return -(PIn.Long(x["IsUnread"].ToString()).CompareTo(PIn.Long(y["IsUnread"].ToString())));//sort unread to top.
				}
			}
			//2)Sort by Task Priority
			if(x["PriorityDefNum"].ToString()!=y["PriorityDefNum"].ToString()) {//we only care about task priority if they are different
				long xTaskPriorityDefNum=PIn.Long(x["PriorityDefNum"].ToString());
				long yTaskPriorityDefNum=PIn.Long(y["PriorityDefNum"].ToString());
				//0 will always be considered like the default task priority.
				if(xTaskPriorityDefNum==0) {
					xTaskPriorityDefNum=_defaultTaskPriorityDefNum;
				}
				if(yTaskPriorityDefNum==0) {
					yTaskPriorityDefNum=_defaultTaskPriorityDefNum;
				}
				//x.ItemOrder.CompareTo(y.ItemOrder)
				return DefC.GetDef(DefCat.TaskPriorities,xTaskPriorityDefNum).ItemOrder.CompareTo(DefC.GetDef(DefCat.TaskPriorities,yTaskPriorityDefNum).ItemOrder);
			}
			//3)Sort by Date Time
			return CompareTimes(x,y);
		}

		///<summary>Compares the most recent times of the task or task notes associated to the tasks passed in.  Most recently updated tasks will be farther down in the list.</summary>
		public static int CompareTimes(DataRow x,DataRow y) {
			if(_isHQ
				&& PIn.Long(x["TaskListNum"].ToString())==_triageTaskListNum
				&& PIn.Long(x["PriorityDefNum"].ToString())==_redTaskDefNum)//Red tasks in triage only, sort by lastUpdated
			{
				DateTime xMaxDateTime=PIn.DateT(x["LastUpdated"].ToString());
				DateTime yMaxDateTime=PIn.DateT(y["LastUpdated"].ToString());
				return xMaxDateTime.CompareTo(yMaxDateTime);
			}
			else {//Sort everything else based on task creation date
				DateTime xMaxDateTime=PIn.DateT(x["DateTimeEntry"].ToString());
				DateTime yMaxDateTime=PIn.DateT(y["DateTimeEntry"].ToString());
				return xMaxDateTime.CompareTo(yMaxDateTime);
			}
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching taskNum as FKey and are related to Task.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Task table type.</summary>
		public static void ClearFkey(long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum);
				return;
			}
			Crud.TaskCrud.ClearFkey(taskNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching taskNums as FKey and are related to Task.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Task table type.</summary>
		public static void ClearFkey(List<long> listTaskNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listTaskNums);
				return;
			}
			Crud.TaskCrud.ClearFkey(listTaskNums);
		}
	}


}
