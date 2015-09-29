using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using System.Text;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormJobManager:Form {

		public FormJobManager() {
			InitializeComponent();
		}

		private void FormJobManager_Load(object sender,EventArgs e) {
			LayoutToolBar();
		}

		///<summary>Causes the toolbar to be laid out.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Project Window"),0,"","Add Project Window"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Job Window"),0,"","Add Job Window"));
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Add Project Window":
					AddJobControl(new UserControlProjects());
					break;
				case "Add Job Window":
					AddJobControl(new UserControlJobs());
					break;
			}
		}

		///<summary>Adds a JobContainerControl to flowPanel that will be filled with the control that was passed in.</summary>
		private void AddJobControl(Control jobControl) {
			JobContainerControl jobContainerControl=new JobContainerControl(jobControl,flowPanel);
		}

	}
}
