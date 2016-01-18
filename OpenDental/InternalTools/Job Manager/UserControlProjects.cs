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
using CodeBase;

namespace OpenDental {
	public partial class UserControlProjects:UserControl {
		///<summary></summary>
		private List<JobProject> _listJobProjects;
		///<summary></summary>
		private List<Job> _listJobs;
		///<summary></summary>
		private List<JobProject> _listJobProjectHistory;
		///<summary>The index of the last clicked item in the main list.</summary>
		private int _clickedIdx;
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;

		public UserControlProjects() {
			InitializeComponent();
			//this.listMain.ContextMenu = this.menuEdit;
			//Lan.F(this);
			Lan.C(this,menuEdit);
			this.gridMain.ContextMenu=this.menuEdit;
			JobHandler.JobFired+=ODEvent_Fired;
		}

		///<summary>And resets the tabs if the user changes.</summary>
		public void InitializeOnStartup(){
			LayoutToolBar();
			_listJobProjectHistory=new List<JobProject>();
			FillTree();
			FillGrid();
		}

		public void ClearLogOff() {
			_listJobProjectHistory=new List<JobProject>();
			FillTree();
			gridMain.Rows.Clear();
			gridMain.Invalidate();
		}

		//NOT BEING CALLED WITH CURRENT CONFIG
		private void UserControlProjects_Load(object sender,System.EventArgs e) {
			LayoutToolBar();
			_listJobProjectHistory=new List<JobProject>();
			FillTree();
			FillGrid();
		}

		///<summary></summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Project"),0,"","AddProject"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Job"),1,"","AddJob"));
			ToolBarMain.Invalidate();
		}

		///<summary>Refreshes the projects grid.</summary>
		public void RefreshProjects() {
			FillGrid();
		}

		private void FillTree() {
			tree.Nodes.Clear();
			TreeNode node;
			//TreeNode lastNode=null;
			string nodedesc;
			for(int i=0;i<_listJobProjectHistory.Count;i++) {
				nodedesc=_listJobProjectHistory[i].Title;
				node=new TreeNode(nodedesc);
				node.Tag=_listJobProjectHistory[i].JobProjectNum;
				if(tree.SelectedNode==null) {
					tree.Nodes.Add(node);
				}
				else {
					tree.SelectedNode.Nodes.Add(node);
				}
				tree.SelectedNode=node;
			}
			//layout
			if(tabContr.SelectedTab==tabMain) {
				tree.Top=tabContr.Bottom;
			}
			tree.Height=_listJobProjectHistory.Count*tree.ItemHeight+8;
			tree.Refresh();
			gridMain.Top=tree.Bottom;
			checkShowFinished.Top=gridMain.Top+1;
		}

		private void FillGrid(){
			long selectedJobNum=0;
			if(gridMain.GetSelectedIndex()!=-1) {
				selectedJobNum=long.Parse(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			}
			if(Security.CurUser==null) {
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				return;
			}
			long parent;
			if(_listJobProjectHistory==null) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(_listJobProjectHistory.Count>0) {//not on main trunk
				parent=_listJobProjectHistory[_listJobProjectHistory.Count-1].JobProjectNum;
			}
			else {//one of the main trunks
				parent=0;
			}
			gridMain.Height=this.ClientSize.Height-gridMain.Top-3;
			RefreshMainLists(parent);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",17);
			col.ImageList=imageListTree;
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableJobProjects","Title"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableJobProjects","Owner"),80);//any width
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			int imageIdx;
			for(int j=0;j<_listJobProjects.Count;j++) {
				if(_listJobProjects[j].ProjectStatus==JobProjectStatus.Done && !checkShowFinished.Checked) {
					continue;
				}
				row=new ODGridRow();
				imageIdx=0;
				row.Cells.Add(imageIdx.ToString());
				row.Cells.Add(_listJobProjects[j].Title);
				row.Cells.Add("");
				row.Tag=-1;//can't select projects
				gridMain.Rows.Add(row);
			}
			for(int i=0;i<_listJobs.Count;i++) {
				if(_listJobs[i].JobStatus==JobStat.Complete && !checkShowFinished.Checked) {
					continue;
				}
				row=new ODGridRow();
				imageIdx=-1;
				row.Cells.Add(imageIdx.ToString());
				row.Cells.Add(_listJobs[i].Title);
				row.Cells.Add(Userods.GetName(_listJobs[i].OwnerNum));
				string[] arrayDescriptionLines=_listJobs[i].Description.Split('\n');
				row.Tag=_listJobs[i].JobNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(long.Parse(gridMain.Rows[i].Tag.ToString())==selectedJobNum) {
					gridMain.SetSelected(i,true);
				}
			}
			gridMain.ScrollValue=gridMain.ScrollValue;//this forces scroll value to reset if it's > allowed max.
			Cursor=Cursors.Default;
		}

		private void checkShowFinished_Click(object sender,EventArgs e) {
			FillGrid();
		}

		///<summary>If parent=0, then this is a trunk.</summary>
		private void RefreshMainLists(long parentProjectNum) {
			if(tabContr.SelectedTab==tabMain) {
				_listJobProjects=JobProjects.GetByParentProject(parentProjectNum,checkShowFinished.Checked);
				_listJobs=Jobs.GetForProject(parentProjectNum,checkShowFinished.Checked);
			}
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//Make sure that this ODEvent is for the Job Manager and that the Tag is not null and is a string.
			if(e.Name!="Job Manager" || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			FillGrid();
		}

		private void tabContr_Click(object sender,System.EventArgs e) {
			_listJobProjectHistory=new List<JobProject>();//clear the tree no matter which tab clicked.
			FillTree();
			FillGrid();
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "AddProject":
					AddJobProject_Clicked();
					break;
				case "AddJob":
					AddJob_Clicked();
					break;
			}
		}

		private void AddJobProject_Clicked() {
			JobProject cur=new JobProject();
			if(_listJobProjectHistory.Count>0) {
				cur.ParentProjectNum=_listJobProjectHistory[_listJobProjectHistory.Count-1].JobProjectNum;
				cur.RootProjectNum=_listJobProjectHistory[0].JobProjectNum;
			}
			else {
				cur.ParentProjectNum=0;
			}
			cur.IsNew=true;
			FormJobProjectEdit FormJPE=new FormJobProjectEdit(cur);
			FormJPE.ShowDialog();
			if(FormJPE.DialogResult==DialogResult.OK) {//Since FillGrid calls the DB, we want to avoid unnecessary DB calls.
				FillGrid();
			}			
		}

		private void AddJob_Clicked() {
			if(JobPermissions.IsAuthorized(JobPerm.Concept)) {
				long projectNum=0;
				if(_listJobProjectHistory.Count>0) {
					projectNum=_listJobProjectHistory[_listJobProjectHistory.Count-1].JobProjectNum;
				}
				FormJobEdit FormJPE=new FormJobEdit(0,projectNum);
				FormJPE.ShowDialog();
				if(FormJPE.DialogResult==DialogResult.OK) {//Since FillGrid calls the DB, we want to avoid unnecessary DB calls.
					FillGrid();
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==0){
				return;
			}
			if(e.Row>=_listJobProjects.Count) {
				Job cur=_listJobs[e.Row-_listJobProjects.Count];
				FormJobEdit FormJPE=new FormJobEdit(cur.JobNum);
				FormJPE.ShowDialog();
				if(FormJPE.DialogResult==DialogResult.OK) {//Since FillGrid calls the DB, we want to avoid unnecessary DB calls.
					FillGrid();
				}
			}
		}

		private void gridMain_MouseDown(object sender,MouseEventArgs e) {
			_clickedIdx=gridMain.PointToRow(e.Y);//e.Row;
			int clickedCol=gridMain.PointToCol(e.X);
			if(_clickedIdx==-1){
				return;
			}
			gridMain.SetSelected(_clickedIdx,true);
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			if(_clickedIdx<_listJobProjects.Count) {
				_listJobProjectHistory.Add(_listJobProjects[_clickedIdx]);
				FillTree();
				FillGrid();
				return;
			}
		}

		private void menuItemDone_Click(object sender,EventArgs e) {
			if(_clickedIdx<_listJobProjects.Count) {//Clicked on a JobProject.  We know this because the grid is ordered by JobProjects before jobs.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you would like to mark this project as complete?")) {
					return;
				}
				int projCount=JobProjects.GetByParentProject(_listJobProjects[_clickedIdx].JobProjectNum,false).Count;
				if(projCount>0) {
					MsgBox.Show(this,"Can not mark as complete while there are active projects in it");
					return;
				}
				int jobCount=Jobs.GetForProject(_listJobProjects[_clickedIdx].JobProjectNum,false).Count;
				if(jobCount>0) {
					MsgBox.Show(this,"Can not mark as complete while there are active jobs in it");
					return;
				}
				try {
					JobProject jobProjectCur=_listJobProjects[_clickedIdx];
					jobProjectCur.ProjectStatus=JobProjectStatus.Done;
					JobProjects.Update(jobProjectCur);
					DataValid.SetInvalid(InvalidType.Jobs);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				FillGrid();
			}
			else {//Clicked on a job.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you would like to mark this job as complete?")) {
					return;
				}
				Job jobCur=_listJobs[_clickedIdx-_listJobProjects.Count];
				jobCur.JobStatus=JobStat.Complete;
				try {
					Jobs.Update(jobCur);
					DataValid.SetInvalid(InvalidType.Jobs);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
			FillGrid();
		}

		private void menuItemEdit_Click(object sender,System.EventArgs e) {
			if(_clickedIdx<_listJobProjects.Count) {//Clicked on a JobProject.  We know this because the grid is ordered by JobProjects before jobs.
				FormJobProjectEdit FormJPE=new FormJobProjectEdit(_listJobProjects[_clickedIdx]);
				FormJPE.ShowDialog();
				if(FormJPE.DialogResult==DialogResult.OK) {//Since FillGrid calls the DB, we want to avoid unnecessary DB calls.
					FillGrid();
				}
			}
			else {
				Job cur=_listJobs[_clickedIdx-_listJobProjects.Count];
				FormJobEdit FormJPE=new FormJobEdit(cur.JobNum);
				FormJPE.ShowDialog();
				if(FormJPE.DialogResult==DialogResult.OK) {//Since FillGrid calls the DB, we want to avoid unnecessary DB calls.
					FillGrid();
				}
			}
		}

		private void menuItemDelete_Click(object sender,System.EventArgs e) {
			if(_clickedIdx<_listJobProjects.Count) {//Clicked on a JobProject.  We know this because the grid is ordered by JobProjects before jobs.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you would like to delete this project?")) {
					return;
				}
				int projCount=JobProjects.GetByParentProject(_listJobProjects[_clickedIdx].JobProjectNum,false).Count;
				if(projCount>0) {
					MsgBox.Show(this,"Can not delete project while there are projects in it");
					return;
				}
				int jobCount=Jobs.GetForProject(_listJobProjects[_clickedIdx].JobProjectNum,false).Count;
				if(jobCount>0) {
					MsgBox.Show(this,"Can not delete project while there are jobs in it");
					return;
				}
				try {
					JobProjects.Delete(_listJobProjects[_clickedIdx].JobProjectNum);
					DataValid.SetInvalid(InvalidType.Jobs);
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			else {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you would like to delete this job?")) {
					return;
				}
				try {
					Jobs.Delete(_listJobs[_clickedIdx-_listJobProjects.Count].JobNum);
					DataValid.SetInvalid(InvalidType.Jobs);
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			FillGrid();
		}

		private void tree_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			for(int i=_listJobProjectHistory.Count-1;i>0;i--) {
				try {
					if(_listJobProjectHistory[i].JobProjectNum==(long)tree.GetNodeAt(e.X,e.Y).Tag) {
						break;//don't remove the node click on or any higher node
					}
					_listJobProjectHistory.RemoveAt(i);
				}
				catch {//Harmless to return here because the user could have clicked near the node
					return;
				}
			}
			FillTree();
			FillGrid();
		}

		

		

		

		

	

		

		

		




	}
}
