using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TaskHists{

		///<summary>Gets one TaskHist from the db.</summary>
		public static TaskHist GetOne(long taskHistNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<TaskHist>(MethodBase.GetCurrentMethod(),taskHistNum);
			}
			return Crud.TaskHistCrud.SelectOne(taskHistNum);
		}

		public static string GetChangesDescription(TaskHist taskOld,TaskHist taskCur) {
			if(taskOld.DateTimeEntry==DateTime.MinValue) {
				return "New task.";
			}
			StringBuilder strb=new StringBuilder();
			strb.Append("");
			if(taskCur.TaskListNum!=taskOld.TaskListNum){
				strb.Append(Lans.g("TaskHists","Task list changed from ")
					+TaskLists.GetOne(taskOld.TaskListNum).Descript+Lans.g("TaskHists"," to ")+TaskLists.GetOne(taskCur.TaskListNum).Descript+".\r\n");
			}
			if(taskCur.ObjectType!=taskOld.ObjectType){
				strb.Append(Lans.g("TaskHists","Task attachment changed from ")
					+taskOld.ObjectType.ToString()+Lans.g("TaskHists"," to ")+taskCur.ObjectType.ToString()+".\r\n");
			}
			if(taskCur.KeyNum!=taskOld.KeyNum){
				strb.Append(Lans.g("TaskHists","Task account attachment changed.\r\n"));
			}
			if(taskCur.Descript!=taskOld.Descript){
				strb.Append(Lans.g("TaskHists","Task description changed.\r\n"));
			}
			if(taskCur.TaskStatus!=taskOld.TaskStatus){
				strb.Append(Lans.g("TaskHists","Task status changed from ")+taskOld.TaskStatus.ToString()+Lans.g("TaskHists"," to ")+taskCur.TaskStatus.ToString()+".\r\n");
			}
			if(taskCur.DateTimeEntry!=taskOld.DateTimeEntry){
				strb.Append(Lans.g("TaskHists","Task date added changed from ")
					+taskOld.DateTimeEntry.ToString()
					+Lans.g("TaskHists"," to ")
					+taskCur.DateTimeEntry.ToString()+".\r\n");
			}
			if(taskCur.UserNum!=taskOld.UserNum){
				strb.Append(Lans.g("TaskHists","Task author changed from ")
					+Userods.GetUser(taskOld.UserNum).UserName
					+Lans.g("TaskHists"," to ")
					+Userods.GetUser(taskCur.UserNum).UserName+".\r\n");
			}
			if(taskCur.DateTimeFinished!=taskOld.DateTimeFinished){
				strb.Append(Lans.g("TaskHists","Task date finished changed from ")
					+taskOld.DateTimeFinished.ToString()
					+Lans.g("TaskHists"," to ")
					+taskCur.DateTimeFinished.ToString()+".\r\n");
			}
			if(taskCur.PriorityDefNum!=taskOld.PriorityDefNum){
				strb.Append(Lans.g("TaskHists","Task priority changed from ")
					+DefC.GetDef(DefCat.TaskPriorities,taskOld.PriorityDefNum).ItemName
					+Lans.g("TaskHists"," to ")
					+DefC.GetDef(DefCat.TaskPriorities,taskCur.PriorityDefNum).ItemName+".\r\n");
			}
			if(taskOld.IsNoteChange) { //Using taskOld because the notes changed from the old one to the new one.
				strb.Append(Lans.g("TaskHists","Task notes changed."));
			}
			return strb.ToString();
		}

		///<summary></summary>
		public static long Insert(TaskHist taskHist){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				taskHist.TaskHistNum=Meth.GetLong(MethodBase.GetCurrentMethod(),taskHist);
				return taskHist.TaskHistNum;
			}
			return Crud.TaskHistCrud.Insert(taskHist);
		}

			///<summary></summary>
		public static void Update(TaskHist taskHist){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskHist);
				return;
			}
			Crud.TaskHistCrud.Update(taskHist);
		}

		///<summary>Gets a list of task histories for a given taskNum.</summary>
		public static List<TaskHist> GetArchivesForTask(long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TaskHist>>(MethodBase.GetCurrentMethod(),taskNum);
			}
			string command="SELECT * FROM taskhist WHERE TaskNum="+POut.Long(taskNum)+" ORDER BY DateTStamp";
			return Crud.TaskHistCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Delete(long taskHistNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskHistNum);
				return;
			}
			Crud.TaskHistCrud.Delete(taskHistNum);
		}




	}
}