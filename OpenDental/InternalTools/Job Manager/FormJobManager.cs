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
			this.Size=System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
			ToolBarMain.Width=this.Size.Width;
			flowPanel.Width=this.Width-15;
			flowPanel.Height=this.Height-25;
			LayoutToolBar();
		}

		///<summary>Causes the toolbar to be laid out.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Project"),0,"","Add Project"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Job"),0,"","Add Job"));
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Add Project":
					AddProject_Click();
					break;
				case "Add Job":
					AddJob_Click();
					break;
			}
		}

		private void AddProject_Click() {
			JobContainerControl JCControl=new JobContainerControl();
			JCControl.butLeft.Click+=new System.EventHandler(butLeft_Click);
			JCControl.butRight.Click+=new System.EventHandler(butRight_Click);
			JCControl.butMerge.Click+=new System.EventHandler(butMerge_Click);
			UserControlProjects UCProjects=new UserControlProjects();
			JCControl.Controls.Add(UCProjects);
			UCProjects.SetBounds(0,10,UCProjects.Width,UCProjects.Height);
			flowPanel.Controls.Add(JCControl);
		}

		private void AddJob_Click() {
			JobContainerControl JCControl=new JobContainerControl();
			JCControl.butLeft.Click+=new System.EventHandler(butLeft_Click);
			JCControl.butRight.Click+=new System.EventHandler(butRight_Click);
			JCControl.butMerge.Click+=new System.EventHandler(butMerge_Click);
			UserControlJobs UCJobs=new UserControlJobs();			
			JCControl.Controls.Add(UCJobs);
			UCJobs.SetBounds(0,10,UCJobs.Width,UCJobs.Height);
			flowPanel.Controls.Add(JCControl);
		}

		private void FormJobManager_Resize(object sender,EventArgs e) {
			flowPanel.Height=this.Height-25;
			flowPanel.Width=this.Width-15;
		}

		public void butLeft_Click(object sender,EventArgs e) {
			JobContainerControl controlFirst=(JobContainerControl)((System.Windows.Forms.Button)sender).Parent;
			int idxA=flowPanel.Controls.GetChildIndex(controlFirst);
			if(idxA==0) {//Control is already the first one.
				return;
			}
			JobContainerControl controlSecond=(JobContainerControl)flowPanel.Controls[idxA-1];
			int idxB=idxA-1;
			flowPanel.Controls.SetChildIndex(controlFirst,idxB);
			flowPanel.Controls.SetChildIndex(controlSecond,idxA);
		}

		public void butRight_Click(object sender,EventArgs e) {
			JobContainerControl controlFirst=(JobContainerControl)((System.Windows.Forms.Button)sender).Parent;
			int idxA=flowPanel.Controls.GetChildIndex(controlFirst);
			if(idxA==flowPanel.Controls.Count-1) {//Control is already the last one.
				return;
			}
			JobContainerControl controlSecond=(JobContainerControl)flowPanel.Controls[idxA+1];
			int idxB=idxA+1;
			flowPanel.Controls.SetChildIndex(controlFirst,idxB);
			flowPanel.Controls.SetChildIndex(controlSecond,idxA);
		}

		public void butMerge_Click(object sender,EventArgs e) {//Event's sender is always the button pressed
			JobContainerControl control=(JobContainerControl)((System.Windows.Forms.Button)sender).Parent;
			//control.butMerge.Image.
			//FormTest formT=new FormTest(control);
			//formT.Show();
		}

		private void flowPanel_DragEnter(object sender,DragEventArgs e) {
			if(e.Data.GetDataPresent(typeof(JobContainerControl))) {
				e.Effect=DragDropEffects.Move;
			}
		}

		private void flowPanel_DragDrop(object sender,DragEventArgs e) {
			JobContainerControl sourceControl=(JobContainerControl)e.Data.GetData(typeof(JobContainerControl));
			JobContainerControl destControl=(JobContainerControl)flowPanel.GetChildAtPoint(new Point(e.X,e.Y));
			if(sourceControl==null || destControl==null) {
				return;
			}
			int sourceIdx=flowPanel.Controls.GetChildIndex(sourceControl);
			int destIdx=flowPanel.Controls.GetChildIndex(destControl);
			flowPanel.Controls.SetChildIndex(sourceControl,destIdx);
			flowPanel.Controls.SetChildIndex(destControl,sourceIdx);
			//flowPanel.Invalidate();
		}

	}
}
