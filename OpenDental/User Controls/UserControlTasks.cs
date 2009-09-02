using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class UserControlTasks:UserControl {
		///<summary>List of all TastLists that are to be displayed in the main window. Combine with TasksList.</summary>
		private List<TaskList> TaskListsList;
		///<summary>List of all Tasks that are to be displayed in the main window.  Combine with TaskListsList.</summary>
		private List<Task> TasksList;
		//<Summary>Only used if viewing user tab.  This is a list of all task lists in the general tab.  It is used to generate full path heirarchy info for each task list in the user tab.</Summary>
		//private List<TaskList> TaskListsAllGeneral;
		///<summary>An arraylist of TaskLists beginning from the trunk and adding on branches.  If the count is 0, then we are in the trunk of one of the five categories.  The last TaskList in the TreeHistory is the one that is open in the main window.</summary>
		private List<TaskList> TreeHistory;
		///<summary>A TaskList that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private TaskList ClipTaskList;
		///<summary>A Task that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private Task ClipTask;
		///<summary>If there is an item on our 'clipboard', this tracks whether it was cut.</summary>
		private bool WasCut;
		///<summary>The index of the last clicked item in the main list.</summary>
		private int clickedI;
		///<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		public TaskObjectType GotoType;
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;

		public UserControlTasks() {
			InitializeComponent();
			//this.listMain.ContextMenu = this.menuEdit;
			//Lan.F(this);
			for(int i=0;i<menuEdit.MenuItems.Count;i++) {
				Lan.C(this,menuEdit.MenuItems[i]);
			}
			this.gridMain.ContextMenu=this.menuEdit;
		}

		///<summary>The parent might call this if it gets a signal that a relevant task was added from another workstation.  The parent should only call this if it has been verified that there is a change to tasks.</summary>
		public void RefreshTasks(){
			FillGrid();
		}

		protected void OnGoToChanged() {
			if(GoToChanged!=null) {
				GoToChanged(this,new EventArgs());
			}
		}

		public void InitializeOnStartup(){
			if(Security.CurUser==null) {
				return;
			}
			tabUser.Text=Lan.g(this,"for ")+Security.CurUser.UserName;
			LayoutToolBar();
			if(Tasks.LastOpenList==null) {//first time openning
				TreeHistory=new List<TaskList>();
				cal.SelectionStart=DateTime.Today;
			}
			else {//reopening
				tabContr.SelectedIndex=Tasks.LastOpenGroup;
				TreeHistory=new List<TaskList>();
				for(int i=0;i<Tasks.LastOpenList.Count;i++) {
					TreeHistory.Add(((TaskList)Tasks.LastOpenList[i]).Copy());
				}
				cal.SelectionStart=Tasks.LastOpenDate;
			}
			FillTree();
			//FillMain();
			FillGrid();
			SetMenusEnabled();
		}

		public void ResetUser(){
			//tabContr.TabPages[
			tabUser.Text=Lan.g(this,"for ")+Security.CurUser.UserName;
		}

		private void UserControlTasks_Load(object sender,System.EventArgs e) {
			if(this.DesignMode){
				return;
			}
			if(!PrefC.GetBool("TaskAncestorsAllSetInVersion55")) {
				if(!MsgBox.Show(this,true,"A one-time routine needs to be run.  It will take a few minutes.  Do you have time right now?")){
					return;
				}
				Cursor=Cursors.WaitCursor;
				TaskAncestors.SynchAll();
				Prefs.UpdateBool("TaskAncestorsAllSetInVersion55",true);
				DataValid.SetInvalid(InvalidType.Prefs);
				Cursor=Cursors.Default;
			}
		}

		///<summary></summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Setup"),-1,"","Setup"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add TaskList"),0,"","AddList"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Task"),1,"","AddTask"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Exit"),-1,"","Exit"));
			ToolBarMain.Invalidate();
		}

		private void FillTree() {
			tree.Nodes.Clear();
			TreeNode node;
			//TreeNode lastNode=null;
			string nodedesc;
			for(int i=0;i<TreeHistory.Count;i++) {
				nodedesc=TreeHistory[i].Descript;
				if(tabContr.SelectedIndex==0){//user tab
					nodedesc=TreeHistory[i].ParentDesc+nodedesc;
				}
				node=new TreeNode(nodedesc);
				node.Tag=TreeHistory[i].TaskListNum;
				if(tree.SelectedNode==null) {
					tree.Nodes.Add(node);
				}
				else {
					tree.SelectedNode.Nodes.Add(node);
				}
				tree.SelectedNode=node;
			}
			//remember this position for the next time we open tasks
			Tasks.LastOpenList=new ArrayList();
			for(int i=0;i<TreeHistory.Count;i++) {
				Tasks.LastOpenList.Add(TreeHistory[i].Copy());
			}
			Tasks.LastOpenGroup=tabContr.SelectedIndex;
			Tasks.LastOpenDate=cal.SelectionStart;
			//layout

			if(tabContr.SelectedIndex==0) {//user
				tree.Top=tabContr.Bottom;
			}
			else if(tabContr.SelectedIndex==1) {//main
				tree.Top=tabContr.Bottom;
			}
			else if(tabContr.SelectedIndex==2) {//repeating
				tree.Top=tabContr.Bottom;
			}
			else {//by date
				tree.Top=cal.Bottom+1;
			}
			tree.Height=TreeHistory.Count*tree.ItemHeight+8;
			tree.Refresh();
			gridMain.Top=tree.Bottom;
			checkShowFinished.Top=gridMain.Top+1;
			textStartDate.Top=gridMain.Top;
			labelStartDate.Top=gridMain.Top+1;
			gridMain.Height=this.ClientSize.Height-gridMain.Top-3;
		}

		private void textStartDate_KeyPress(object sender,KeyPressEventArgs e) {
			//FillGrid();
		}

		private void textStartDate_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			long parent;
			DateTime date;
			if(TreeHistory==null){
				return;
			}
			if(TreeHistory.Count>0) {//not on main trunk
				parent=TreeHistory[TreeHistory.Count-1].TaskListNum;
				date=DateTime.MinValue;
			}
			else {//one of the main trunks
				parent=0;
				date=cal.SelectionStart;
			}
			RefreshMainLists(parent,date);
			#region dated trunk automation
			//dated trunk automation-----------------------------------------------------------------------------
			if(TreeHistory.Count==0//main trunk
				&& (tabContr.SelectedIndex==3	|| tabContr.SelectedIndex==4 || tabContr.SelectedIndex==5))//any of the dated groups
			{
				//clear any lists which are derived from a repeating list and which do not have any itmes checked off
				bool changeMade=false;
				for(int i=0;i<TaskListsList.Count;i++) {
					if(TaskListsList[i].FromNum==0) {//ignore because not derived from a repeating list
						continue;
					}
					if(!AnyAreMarkedComplete(TaskListsList[i])) {
						DeleteEntireList(TaskListsList[i]);
						changeMade=true;
					}
				}
				//clear any tasks which are derived from a repeating task 
				//and which are still new (not marked viewed or done)
				for(int i=0;i<TasksList.Count;i++) {
					if(TasksList[i].FromNum==0) {
						continue;
					}
					if(TasksList[i].TaskStatus==TaskStatusEnum.New) {
						Tasks.Delete(TasksList[i]);
						changeMade=true;
					}
				}
				if(changeMade) {
					RefreshMainLists(parent,date);
				}
				//now add back any repeating lists and tasks that meet the criteria
				//Get lists of all repeating lists and tasks of one type.  We will pick items from these two lists.
				List<TaskList> repeatingLists=new List<TaskList>();
				List<Task> repeatingTasks=new List<Task>();
				switch(tabContr.SelectedIndex) {
					case 3:
						repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Day);
						repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Day);
						break;
					case 4:
						repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Week);
						repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Week);
						break;
					case 5:
						repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Month);
						repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Month);
						break;
				}
				//loop through list and add back any that meet criteria.
				changeMade=false;
				bool alreadyExists;
				for(int i=0;i<repeatingLists.Count;i++) {
					//if already exists, skip
					alreadyExists=false;
					for(int j=0;j<TaskListsList.Count;j++) {//loop through Main list
						if(TaskListsList[j].FromNum==repeatingLists[i].TaskListNum) {
							alreadyExists=true;
							break;
						}
					}
					if(alreadyExists) {
						continue;
					}
					//otherwise, duplicate the list
					repeatingLists[i].DateTL=date;
					repeatingLists[i].FromNum=repeatingLists[i].TaskListNum;
					repeatingLists[i].IsRepeating=false;
					repeatingLists[i].Parent=0;
					repeatingLists[i].ObjectType=0;//user will have to set explicitly
					DuplicateExistingList(repeatingLists[i],true);
					changeMade=true;
				}
				for(int i=0;i<repeatingTasks.Count;i++) {
					//if already exists, skip
					alreadyExists=false;
					for(int j=0;j<TasksList.Count;j++) {//loop through Main list
						if(TasksList[j].FromNum==repeatingTasks[i].TaskNum) {
							alreadyExists=true;
							break;
						}
					}
					if(alreadyExists) {
						continue;
					}
					//otherwise, duplicate the task
					repeatingTasks[i].DateTask=date;
					repeatingTasks[i].FromNum=repeatingTasks[i].TaskNum;
					repeatingTasks[i].IsRepeating=false;
					repeatingTasks[i].TaskListNum=0;
					//repeatingTasks[i].UserNum//repeating tasks shouldn't get a usernum
					Tasks.Insert(repeatingTasks[i]);
					changeMade=true;
				}
				if(changeMade) {
					RefreshMainLists(parent,date);
				}
			}//End of dated trunk automation--------------------------------------------------------------------------
			#endregion dated trunk automation
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",17);
			col.ImageList=imageListTree;
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTasks","Description"),200);//any width
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			string dateStr="";
			string objDesc="";
			string tasklistdescript="";
			int imageindex;
			for(int i=0;i<TaskListsList.Count;i++) {
				dateStr="";
				if(TaskListsList[i].DateTL.Year>1880
					&& (tabContr.SelectedIndex==0 || tabContr.SelectedIndex==1))//user or main
				{
					if(TaskListsList[i].DateType==TaskDateType.Day) {
						dateStr=TaskListsList[i].DateTL.ToShortDateString()+" - ";
					}
					else if(TaskListsList[i].DateType==TaskDateType.Week) {
						dateStr=Lan.g(this,"Week of")+" "+TaskListsList[i].DateTL.ToShortDateString()+" - ";
					}
					else if(TaskListsList[i].DateType==TaskDateType.Month) {
						dateStr=TaskListsList[i].DateTL.ToString("MMMM")+" - ";
					}
				}
				objDesc="";
				if(tabContr.SelectedIndex==0){//user tab
					objDesc=TaskListsList[i].ParentDesc;
				}
				tasklistdescript=TaskListsList[i].Descript;
				imageindex=0;
				if(TaskListsList[i].NewTaskCount>0){
					imageindex=3;//orange
					tasklistdescript=tasklistdescript+"("+TaskListsList[i].NewTaskCount.ToString()+")";
				}
				row=new ODGridRow();
				row.Cells.Add(imageindex.ToString());
				row.Cells.Add(dateStr+objDesc+tasklistdescript);
				gridMain.Rows.Add(row);
			}
			for(int i=0;i<TasksList.Count;i++) {
				dateStr="";
				if(tabContr.SelectedIndex==0 || tabContr.SelectedIndex==1) {//user or main
					if(TasksList[i].DateTask.Year>1880) {
						if(TasksList[i].DateType==TaskDateType.Day) {
							dateStr=TasksList[i].DateTask.ToShortDateString()+" - ";
						}
						else if(TasksList[i].DateType==TaskDateType.Week) {
							dateStr=Lan.g(this,"Week of")+" "+TasksList[i].DateTask.ToShortDateString()+" - ";
						}
						else if(TasksList[i].DateType==TaskDateType.Month) {
							dateStr=TasksList[i].DateTask.ToString("MMMM")+" - ";
						}
					}
					else if(TasksList[i].DateTimeEntry.Year>1880) {
						dateStr=TasksList[i].DateTimeEntry.ToShortDateString()+" - ";
					}
				}
				objDesc="";
				if(TasksList[i].TaskStatus==TaskStatusEnum.Done){
					objDesc=Lan.g(this,"Done:")+TasksList[i].DateTimeFinished.ToShortDateString()+" - ";
				}
				if(TasksList[i].ObjectType==TaskObjectType.Patient) {
					if(TasksList[i].KeyNum!=0) {
						objDesc=Patients.GetPat(TasksList[i].KeyNum).GetNameLF()+" - ";
					}
				}
				else if(TasksList[i].ObjectType==TaskObjectType.Appointment) {
					if(TasksList[i].KeyNum!=0) {
						Appointment AptCur=Appointments.GetOneApt(TasksList[i].KeyNum);
						if(AptCur!=null) {
							objDesc=Patients.GetPat(AptCur.PatNum).GetNameLF()
								+"  "+AptCur.AptDateTime.ToString()
								+"  "+AptCur.ProcDescript
								+"  "+AptCur.Note
								+" - ";
						}
					}
				}
				row=new ODGridRow();
				switch(TasksList[i].TaskStatus) {
					case TaskStatusEnum.New:
						row.Cells.Add("4");
						break;
					case TaskStatusEnum.Viewed:
						row.Cells.Add("2");
						break;
					case TaskStatusEnum.Done:
						row.Cells.Add("1");
						break;
				}
				row.Cells.Add(dateStr+objDesc+TasksList[i].Descript);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void checkShowFinished_Click(object sender,EventArgs e) {
			if(checkShowFinished.Checked){
				labelStartDate.Visible=true;
				textStartDate.Visible=true;
				textStartDate.Text=DateTime.Now.AddDays(-7).ToShortDateString();
			}
			else{
				labelStartDate.Visible=false;
				textStartDate.Visible=false;
			}
			FillGrid();
		}

		///<summary>A recursive function that checks every child in a list IsFromRepeating.  If any are marked complete, then it returns true, signifying that this list should be immune from being deleted since it's already in use.</summary>
		private bool AnyAreMarkedComplete(TaskList list) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(list.TaskListNum);
			List<Task> childTasks=Tasks.RefreshChildren(list.TaskListNum,true,DateTime.MinValue);
			for(int i=0;i<childLists.Count;i++) {
				if(AnyAreMarkedComplete(childLists[i])) {
					return true;
				}
			}
			for(int i=0;i<childTasks.Count;i++) {
				if(childTasks[i].TaskStatus==TaskStatusEnum.Done) {
					return true;
				}
			}
			return false;
		}

		///<summary>If parent=0, then this is a trunk.</summary>
		private void RefreshMainLists(long parent,DateTime date) {
			DateTime startDate=DateTime.MinValue;
			if(textStartDate.Visible && textStartDate.Text!=""){
				if(textStartDate.errorProvider1.GetError(textStartDate)==""){
					startDate=PIn.PDate(textStartDate.Text);
				}
				else{//invalid date
					startDate=DateTime.Today.AddDays(-7);
				}
			}
			if(this.DesignMode){
				TaskListsList=new List<TaskList>();
				TasksList=new List<Task>();
				return;
			}
			if(tabContr.SelectedIndex==0){//user
				//TaskListsAllGeneral=TaskLists.GetAllGeneral();
			}
			if(parent!=0){//not a trunk
				TaskListsList=TaskLists.RefreshChildren(parent);
				TasksList=Tasks.RefreshChildren(parent,checkShowFinished.Checked,startDate);
			}
			else if(tabContr.SelectedIndex==0) {//user
				TaskListsList=TaskLists.RefreshUserTrunk(Security.CurUser.UserNum);
				TasksList=new List<Task>();//no tasks in the user trunk
					//Tasks.RefreshUserTrunk(Security.CurUser.UserNum);
			}
			else if(tabContr.SelectedIndex==1) {//main
				TaskListsList=TaskLists.RefreshMainTrunk();
				TasksList=Tasks.RefreshMainTrunk(checkShowFinished.Checked,startDate);
			}
			else if(tabContr.SelectedIndex==2) {//repeating
				TaskListsList=TaskLists.RefreshRepeatingTrunk();
				TasksList=Tasks.RefreshRepeatingTrunk();
			}
			else if(tabContr.SelectedIndex==3) {//date
				TaskListsList=TaskLists.RefreshDatedTrunk(date,TaskDateType.Day);
				TasksList=Tasks.RefreshDatedTrunk(date,TaskDateType.Day,checkShowFinished.Checked,startDate);
			}
			else if(tabContr.SelectedIndex==4) {//week
				TaskListsList=TaskLists.RefreshDatedTrunk(date,TaskDateType.Week);
				TasksList=Tasks.RefreshDatedTrunk(date,TaskDateType.Week,checkShowFinished.Checked,startDate);
			}
			else if(tabContr.SelectedIndex==5) {//month
				TaskListsList=TaskLists.RefreshDatedTrunk(date,TaskDateType.Month);
				TasksList=Tasks.RefreshDatedTrunk(date,TaskDateType.Month,checkShowFinished.Checked,startDate);
			}
		}

		private void tabContr_Click(object sender,System.EventArgs e) {
			TreeHistory=new List<TaskList>();//clear the tree no matter which tab clicked.
			FillTree();
			FillGrid();
		}

		private void cal_DateSelected(object sender,System.Windows.Forms.DateRangeEventArgs e) {
			TreeHistory=new List<TaskList>();//clear the tree
			FillTree();
			FillGrid();
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			//if(e.Button.Tag.GetType()==typeof(string)){
			//standard predefined button
			switch(e.Button.Tag.ToString()) {
				case "Setup":
					OnSetup_Click();
					break;
				case "AddList":
					OnAddList_Click();
					break;
				case "AddTask":
					OnAddTask_Click();
					break;
			}
		}

		private void OnSetup_Click() {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTaskSetup FormT=new FormTaskSetup();
			FormT.ShowDialog();
		}

		private void OnAddList_Click() {
			if(tabContr.SelectedIndex==0 && TreeHistory.Count==0){//trunk of user tab
				MsgBox.Show(this,"Not allowed to add a task list to the trunk of the user tab.  Either use the subscription feature, or add it to a child list.");
				return;
			}
			TaskList cur=new TaskList();
			//if this is a child of any other taskList
			if(TreeHistory.Count>0) {
				cur.Parent=TreeHistory[TreeHistory.Count-1].TaskListNum;
			}
			else {
				cur.Parent=0;
				if(tabContr.SelectedIndex==3) {//by date
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Day;
				}
				else if(tabContr.SelectedIndex==4) {//by week
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Week;
				}
				else if(tabContr.SelectedIndex==5) {//by month
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Month;
				}
			}
			if(tabContr.SelectedIndex==2) {//repeating
				cur.IsRepeating=true;
			}
			FormTaskListEdit FormT=new FormTaskListEdit(cur);
			FormT.IsNew=true;
			FormT.ShowDialog();
			FillGrid();
		}

		private void OnAddTask_Click() {
			if(tabContr.SelectedIndex==0 && TreeHistory.Count==0) {//trunk of user tab
				MsgBox.Show(this,"Not allowed to add a task to the trunk of the user tab.  Add it to a child list instead.");
				return;
			}
			Task cur=new Task();
			//if this is a child of any taskList
			if(TreeHistory.Count>0) {
				cur.TaskListNum=TreeHistory[TreeHistory.Count-1].TaskListNum;
			}
			else {
				cur.TaskListNum=0;
				if(tabContr.SelectedIndex==3) {//by date
					cur.DateTask=cal.SelectionStart;
					cur.DateType=TaskDateType.Day;
				}
				else if(tabContr.SelectedIndex==4) {//by week
					cur.DateTask=cal.SelectionStart;
					cur.DateType=TaskDateType.Week;
				}
				else if(tabContr.SelectedIndex==5) {//by month
					cur.DateTask=cal.SelectionStart;
					cur.DateType=TaskDateType.Month;
				}
			}
			if(tabContr.SelectedIndex==2) {//repeating
				cur.IsRepeating=true;
			}
			cur.UserNum=Security.CurUser.UserNum;
			FormTaskEdit FormT=new FormTaskEdit(cur);
			FormT.IsNew=true;
			FormT.ShowDialog();
			//moved into the task edit window.
			//if(FormT.DialogResult==DialogResult.OK){
			//	DataValid.SetInvalidTask(cur.TaskNum,true);
			//}
			if(FormT.GotoType!=TaskObjectType.None) {
				GotoType=FormT.GotoType;
				GotoKeyNum=FormT.GotoKeyNum;
				OnGoToChanged();
				//DialogResult=DialogResult.OK;
				return;
			}
			FillGrid();
		}

		private void OnEdit_Click() {
			if(clickedI < TaskListsList.Count) {//is list
				FormTaskListEdit FormT=new FormTaskListEdit(TaskListsList[clickedI]);
				FormT.ShowDialog();
			}
			else {//task
				FormTaskEdit FormT=new FormTaskEdit(TasksList[clickedI-TaskListsList.Count]);
				FormT.ShowDialog();
				if(FormT.GotoType!=TaskObjectType.None) {
					GotoType=FormT.GotoType;
					GotoKeyNum=FormT.GotoKeyNum;
					OnGoToChanged();
					//DialogResult=DialogResult.OK;
					return;
				}
			}
			FillGrid();
		}

		private void OnCut_Click() {
			if(clickedI < TaskListsList.Count) {//is list
				ClipTaskList=TaskListsList[clickedI].Copy();
				ClipTask=null;
			}
			else {//task
				ClipTaskList=null;
				ClipTask=TasksList[clickedI-TaskListsList.Count].Copy();
			}
			WasCut=true;
		}

		private void OnCopy_Click() {
			if(clickedI < TaskListsList.Count) {//is list
				ClipTaskList=TaskListsList[clickedI].Copy();
				ClipTask=null;
			}
			else {//task
				ClipTaskList=null;
				ClipTask=TasksList[clickedI-TaskListsList.Count].Copy();
			}
			WasCut=false;
		}

		private void OnPaste_Click() {
			if(ClipTaskList!=null) {//a taskList is on the clipboard
				TaskList newTL=ClipTaskList.Copy();
				if(TreeHistory.Count>0) {//not on main trunk
					newTL.Parent=TreeHistory[TreeHistory.Count-1].TaskListNum;
					switch(tabContr.SelectedIndex) {
						case 0://user
							//treat pasting just like it's the main tab, because not on the trunk.
							break;
						case 1://main
							//even though usually only trunks are dated, we will leave the date alone in main
							//category since user may wish to preserve it. All other children get date cleared.
							break;
						case 2://repeating
							newTL.DateTL=DateTime.MinValue;//never a date
							//leave dateType alone, since that affects how it repeats
							break;
						case 3://day
						case 4://week
						case 5://month
							newTL.DateTL=DateTime.MinValue;//children do not get dated
							newTL.DateType=TaskDateType.None;//this doesn't matter either for children
							break;
					}
				}
				else {//one of the main trunks
					newTL.Parent=0;
					switch(tabContr.SelectedIndex) {
						case 0://user
							//maybe we should treat this like a subscription rather than a paste.  Implement later.  For now:
							MsgBox.Show(this,"Not allowed to paste directly to the trunk of this tab.  Try using the subscription feature instead.");
							return;
						case 1://main
							newTL.DateTL=DateTime.MinValue;
							newTL.DateType=TaskDateType.None;
							break;
						case 2://repeating
							newTL.DateTL=DateTime.MinValue;//never a date
							//newTL.DateType=TaskDateType.None;//leave alone
							break;
						case 3://day
							newTL.DateTL=cal.SelectionStart;
							newTL.DateType=TaskDateType.Day;
							break;
						case 4://week
							newTL.DateTL=cal.SelectionStart;
							newTL.DateType=TaskDateType.Week;
							break;
						case 5://month
							newTL.DateTL=cal.SelectionStart;
							newTL.DateType=TaskDateType.Month;
							break;
					}
				}
				if(tabContr.SelectedIndex==2) {//repeating
					newTL.IsRepeating=true;
				}
				else {
					newTL.IsRepeating=false;
				}
				newTL.FromNum=0;//always
				if(tabContr.SelectedIndex==0 || tabContr.SelectedIndex==1) {//user or main
					DuplicateExistingList(newTL,true);
				}
				else {
					DuplicateExistingList(newTL,false);
				}
			}
			if(ClipTask!=null) {//a task is on the clipboard
				Task newT=ClipTask.Copy();
				if(TreeHistory.Count>0) {//not on main trunk
					newT.TaskListNum=TreeHistory[TreeHistory.Count-1].TaskListNum;
					switch(tabContr.SelectedIndex) {
						case 0://user
							//treat pasting just like it's the main tab, because not on the trunk.
							break;
						case 1://main
							//even though usually only trunks are dated, we will leave the date alone in main
							//category since user may wish to preserve it. All other children get date cleared.
							break;
						case 2://repeating
							newT.DateTask=DateTime.MinValue;//never a date
							//leave dateType alone, since that affects how it repeats
							break;
						case 3://day
						case 4://week
						case 5://month
							newT.DateTask=DateTime.MinValue;//children do not get dated
							newT.DateType=TaskDateType.None;//this doesn't matter either for children
							break;
					}
				}
				else {//one of the main trunks
					newT.TaskListNum=0;
					switch(tabContr.SelectedIndex) {
						case 0://user
							//never allowed to have a task on the user trunk.
							MsgBox.Show(this,"Tasks may not be pasted directly to the trunk of this tab.  Try pasting within a list instead.");
							return;
						case 1://main
							newT.DateTask=DateTime.MinValue;
							newT.DateType=TaskDateType.None;
							break;
						case 2://repeating
							newT.DateTask=DateTime.MinValue;//never a date
							//newTL.DateType=TaskDateType.None;//leave alone
							break;
						case 3://day
							newT.DateTask=cal.SelectionStart;
							newT.DateType=TaskDateType.Day;
							break;
						case 4://week
							newT.DateTask=cal.SelectionStart;
							newT.DateType=TaskDateType.Week;
							break;
						case 5://month
							newT.DateTask=cal.SelectionStart;
							newT.DateType=TaskDateType.Month;
							break;
					}
				}
				if(tabContr.SelectedIndex==2) {//repeating
					newT.IsRepeating=true;
				}
				else {
					newT.IsRepeating=false;
				}
				newT.FromNum=0;//always
				if(WasCut && Tasks.WasTaskAltered(ClipTask)){
					MsgBox.Show("Tasks","Not allowed to move because the task has been altered by someone else.");
					FillGrid();
					return;
				}
				Tasks.Insert(newT);
				DataValid.SetInvalidTask(newT.TaskNum,true);
			}
			if(WasCut) {
				if(ClipTaskList!=null) {
					DeleteEntireList(ClipTaskList);
				}
				else if(ClipTask!=null) {
					Tasks.Delete(ClipTask);
				}
			}
			FillGrid();
		}

		private void OnGoto_Click() {
			//not even allowed to get to this point unless a valid task
			Task task=TasksList[clickedI-TaskListsList.Count];
			GotoType=task.ObjectType;
			GotoKeyNum=task.KeyNum;
			OnGoToChanged();
		}

		///<summary>A recursive function that duplicates an entire existing TaskList.  For the initial loop, make changes to the original taskList before passing it in.  That way, Date and type are only set in initial loop.  All children preserve original dates and types.  The isRepeating value will be applied in all loops.  Also, make sure to change the parent num to the new one before calling this function.  The taskListNum will always change, because we are inserting new record into database.</summary>
		private void DuplicateExistingList(TaskList newList,bool isInMainOrUser) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(newList.TaskListNum);
			List<Task> childTasks=Tasks.RefreshChildren(newList.TaskListNum,true,DateTime.MinValue);
			TaskLists.InsertOrUpdate(newList,true);
			//now we have a new taskListNum to work with
			for(int i=0;i<childLists.Count;i++) {
				childLists[i].Parent=newList.TaskListNum;
				if(newList.IsRepeating) {
					childLists[i].IsRepeating=true;
					childLists[i].DateTL=DateTime.MinValue;//never a date
				}
				else {
					childLists[i].IsRepeating=false;
				}
				childLists[i].FromNum=0;
				if(!isInMainOrUser) {
					childLists[i].DateTL=DateTime.MinValue;
					childLists[i].DateType=TaskDateType.None;
				}
				DuplicateExistingList(childLists[i],isInMainOrUser);
			}
			for(int i=0;i<childTasks.Count;i++) {
				childTasks[i].TaskListNum=newList.TaskListNum;
				if(newList.IsRepeating) {
					childTasks[i].IsRepeating=true;
					childTasks[i].DateTask=DateTime.MinValue;//never a date
				}
				else {
					childTasks[i].IsRepeating=false;
				}
				childTasks[i].FromNum=0;
				if(!isInMainOrUser) {
					childTasks[i].DateTask=DateTime.MinValue;
					childTasks[i].DateType=TaskDateType.None;
				}
				Tasks.Insert(childTasks[i]);
			}
		}

		private void OnDelete_Click() {
			if(clickedI < TaskListsList.Count) {//is list
				//check to make sure the list is empty.
				List<Task> tsks=Tasks.RefreshChildren(TaskListsList[clickedI].TaskListNum,true,DateTime.MinValue);
				List<TaskList> tsklsts=TaskLists.RefreshChildren(TaskListsList[clickedI].TaskListNum);
				if(tsks.Count>0 || tsklsts.Count>0){
					MsgBox.Show(this,"Not allowed to delete a list unless it's empty.");
					return;
				}
				if(!MsgBox.Show(this,true,"Delete list including all sublists and tasks?")) {
					return;
				}
				DeleteEntireList(TaskListsList[clickedI]);
			}
			else {//Is task
				if(!MsgBox.Show(this,true,"Delete?")) {
					return;
				}
				Tasks.Delete(TasksList[clickedI-TaskListsList.Count]);
				DataValid.SetInvalidTask(TasksList[clickedI-TaskListsList.Count].TaskNum,false);
			}
			FillGrid();
		}

		///<summary>A recursive function that deletes the specified list and all children.</summary>
		private void DeleteEntireList(TaskList list) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(list.TaskListNum);
			List<Task> childTasks=Tasks.RefreshChildren(list.TaskListNum,true,DateTime.MinValue);
			for(int i=0;i<childLists.Count;i++) {
				DeleteEntireList(childLists[i]);
			}
			for(int i=0;i<childTasks.Count;i++) {
				Tasks.Delete(childTasks[i]);
			}
			try {
				TaskLists.Delete(list);
			}
			catch(Exception e) {
				MessageBox.Show(e.Message);
			}
		}

		//private void listMain_Click(object sender,System.EventArgs e) {
			
		//}

		/*private void listMain_DoubleClick(object sender,System.EventArgs e) {
			if(clickedI==-1) {
				return;
			}
			if(clickedI >= TaskListsList.Count) {//is task
				FormTaskEdit FormT=new FormTaskEdit(TasksList[clickedI-TaskListsList.Count]);
				FormT.ShowDialog();
				if(FormT.GotoType!=TaskObjectType.None) {
					GotoType=FormT.GotoType;
					GotoKeyNum=FormT.GotoKeyNum;
					OnGoToChanged();
					//DialogResult=DialogResult.OK;
					return;
				}
			}
			FillMain();
			FillGrid();
		}*/

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//if(clickedI==-1) {
			//	return;
			//}
			if(e.Col==0){
				//no longer allow double click on checkbox, because it's annoying.
				return;
			}
			if(e.Row >= TaskListsList.Count) {//is task
				FormTaskEdit FormT=new FormTaskEdit(TasksList[e.Row-TaskListsList.Count]);
				FormT.ShowDialog();
				if(FormT.GotoType!=TaskObjectType.None) {
					GotoType=FormT.GotoType;
					GotoKeyNum=FormT.GotoKeyNum;
					OnGoToChanged();
					return;
				}
			}
			FillGrid();
		}

		private void gridMain_MouseDown(object sender,MouseEventArgs e) {
			clickedI=gridMain.PointToRow(e.Y);//e.Row;
			int clickedCol=gridMain.PointToCol(e.X);
			if(clickedI==-1){
				return;
			}
			gridMain.SetSelected(clickedI,true);//if right click.
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			if(clickedI < TaskListsList.Count) {//is list
				TreeHistory.Add(TaskListsList[clickedI]);
				FillTree();
				FillGrid();
				return;
			}
			//check tasks off
			if(clickedCol!=0){//e.X>ClickedItem.Position.X+16) {
				return;
			}
			Task task=TasksList[clickedI-TaskListsList.Count].Copy();
			Task taskOld=task.Copy();
			if(task.TaskStatus==TaskStatusEnum.New){
				task.TaskStatus=TaskStatusEnum.Viewed;
			}
			else if(task.TaskStatus==TaskStatusEnum.Viewed){
				task.TaskStatus=TaskStatusEnum.Done;
				task.DateTimeFinished=DateTime.Now;
			}
			else if(task.TaskStatus==TaskStatusEnum.Done){
				//I guess just leave the date finished in place. It will reset if they mark it complete again.
				task.TaskStatus=TaskStatusEnum.New;
			}
			try {
				Tasks.Update(task,taskOld);
				DataValid.SetInvalidTask(task.TaskNum,false);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FillGrid();
		}

		private void menuEdit_Popup(object sender,System.EventArgs e) {
			SetMenusEnabled();
		}

		private void SetMenusEnabled() {
			if(gridMain.SelectedIndices.Length==0) {
				menuItemEdit.Enabled=false;
				menuItemCut.Enabled=false;
				menuItemCopy.Enabled=false;
				menuItemDelete.Enabled=false;
			}
			else {
				menuItemEdit.Enabled=true;
				menuItemCut.Enabled=true;
				menuItemCopy.Enabled=true;
				menuItemDelete.Enabled=true;
			}
			if(ClipTaskList==null && ClipTask==null) {
				menuItemPaste.Enabled=false;
			}
			else {//there is an item on our clipboard
				menuItemPaste.Enabled=true;
			}
			//Subscriptions----------------------------------------------------------
			if(gridMain.SelectedIndices.Length==0) {
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=false;
			}
			else if(tabContr.SelectedIndex==0){//user
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=true;
			}
			else if(tabContr.SelectedIndex==1 && clickedI < TaskListsList.Count) {//main and tasklist
				menuItemSubscribe.Enabled=true;
				menuItemUnsubscribe.Enabled=false;
			}
			else{//either any other tab, or a task on the main list
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=false;
			}
			//Goto---------------------------------------------------------------
			if(gridMain.SelectedIndices.Length>0
				&& clickedI >= TaskListsList.Count)//is task
			{
				Task task=TasksList[clickedI-TaskListsList.Count];
				if(task.ObjectType==TaskObjectType.None) {
					menuItemGoto.Enabled=false;
				}
				else {
					menuItemGoto.Enabled=true;
				}
			}
			else {
				menuItemGoto.Enabled=false;//not a task
			}
		}

		private void OnSubscribe_Click(){
			//won't even get to this point unless it is a list
			try{
				TaskSubscriptions.SubscList(TaskListsList[clickedI].TaskListNum,Security.CurUser.UserNum);
			}
			catch(ApplicationException ex){//for example, if already subscribed.
				MessageBox.Show(ex.Message);
				return;
			}
			MsgBox.Show(this,"Done");
		}

		private void OnUnsubscribe_Click() {
			TaskSubscriptions.UnsubscList(TaskListsList[clickedI].TaskListNum,Security.CurUser.UserNum);
			//FillMain();
			FillGrid();
		}

		private void menuItemEdit_Click(object sender,System.EventArgs e) {
			OnEdit_Click();
		}

		private void menuItemCut_Click(object sender,System.EventArgs e) {
			OnCut_Click();
		}

		private void menuItemCopy_Click(object sender,System.EventArgs e) {
			OnCopy_Click();
		}

		private void menuItemPaste_Click(object sender,System.EventArgs e) {
			OnPaste_Click();
		}

		private void menuItemDelete_Click(object sender,System.EventArgs e) {
			OnDelete_Click();
		}

		private void menuItemSubscribe_Click(object sender,EventArgs e) {
			OnSubscribe_Click();
		}

		private void menuItemUnsubscribe_Click(object sender,EventArgs e) {
			OnUnsubscribe_Click();
		}

		private void menuItemGoto_Click(object sender,System.EventArgs e) {
			OnGoto_Click();
		}

		//private void listMain_SelectedIndexChanged(object sender,System.EventArgs e) {
		//	SetMenusEnabled();
		//}

		private void tree_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			for(int i=TreeHistory.Count-1;i>0;i--) {
				if(TreeHistory[i].TaskListNum==(int)tree.GetNodeAt(e.X,e.Y).Tag) {
					break;//don't remove the node click on or any higher node
				}
				TreeHistory.RemoveAt(i);
			}
			FillTree();
			//FillMain();
			FillGrid();
		}

		

		

		

		

	

		

		

		




	}
}
