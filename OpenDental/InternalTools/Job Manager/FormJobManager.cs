using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using System.Text;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormJobManager:Form {
		///<summary>A simple list that keeps track of any job container control that has ever been added to the flow panel.
		///We keep them around so that when this window closes, we have access to each containers "CloseForm" method.</summary>
		private List<JobContainerControl> _listJobContainerControls=new List<JobContainerControl>();

		public FormJobManager() {
			InitializeComponent();
		}

		private void FormJobManager_Load(object sender,EventArgs e) {
			LayoutToolBar();
			AddJobControl(new UserControlManage(),"Manage");
		}

		///<summary>Causes the toolbar to be laid out.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Project Window"),0,"","Add Project Window"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Job Window"),0,"","Add Job Window"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Review Window"),0,"","Add Review Window"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Manage Window"),0,"","Add Manage Window"));
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Add Project Window":
					AddJobControl(new UserControlProjects(),"Projects");
					break;
				case "Add Job Window":
					AddJobControl(new UserControlJobs(),"Jobs");
					break;
				case "Add Review Window":
					AddJobControl(new UserControlReviews(),"Reviews");
					break;
				case "Add Manage Window":
					AddJobControl(new UserControlManage(),"Manage");
					break;
			}
		}

		///<summary>Adds a JobContainerControl to flowPanel that will be filled with the control that was passed in.</summary>
		private void AddJobControl(Control jobControl,string title) {
			JobContainerControl jobContainerControl=new JobContainerControl(jobControl,flowPanel,title);
			_listJobContainerControls.Add(jobContainerControl);
		}

		private void FormJobManager_FormClosing(object sender,FormClosingEventArgs e) {
			//When the job manager window is closed, we want to focefully close all our "children" forms that could be floating on their own.
			//When job container controls are "undocked", they will be spawned in their own forms and will no longer be associated to this form.
			//If we were to forcefully make this job manager window the parent of those spawned forms, the children forms would always be on top.
			for(int i=0;i<_listJobContainerControls.Count;i++) {
				if(_listJobContainerControls[i]!=null && !_listJobContainerControls[i].IsDisposed) {
					_listJobContainerControls[i].CloseForm();
				}
			}
		}

	}
}
