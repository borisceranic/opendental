using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class JobContainerControl:DraggableControl {
		///<summary>A reference to the flow panel that contains this JobContainerControl.
		///If this becomes a problem in the future, we can easily move code for moving this JobContainerControl around in the flow panel 
		///into FormJobManager.cs which is where the flow panel really lives.  This could be done with eventing, public static methods, etc.</summary>
		private FlowLayoutPanel _flowPanel;
		private bool _isDocked=true;
		private string _title;

		///<summary>Creates a draggable control containing the control passed in that will be added to the flow panel passed in.</summary>
		public JobContainerControl(Control controlJob,FlowLayoutPanel flowPanel,string title) {
			InitializeComponent();
			_flowPanel=flowPanel;
			_title=title;
			this.Controls.Add(controlJob);
			if(controlJob.GetType()==typeof(UserControlManage)) {
				this.Height=flowPanel.Height-10;
				this.Width=flowPanel.Width-20;
				controlJob.SetBounds(0,20,this.Width,this.Height-20);
				controlJob.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
				butDock.ImageIndex=1;
				panelHighlight.Height=this.Height;
				panelHighlight.Width=this.Width;
				_flowPanel.Controls.Add(this);
			}
			else {
				this.Height=controlJob.Height+20;
				this.Width=controlJob.Width;
				panelHighlight.Height=this.Height;
				panelHighlight.Width=this.Width;
				butLeft.Visible=false;
				butRight.Visible=false;
				butDock.ImageIndex=0;
				controlJob.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
				FormJobContainer FormJC=new FormJobContainer(this,_title);
				FormJC.Show();
				_isDocked=false;
				AllowDragging=false;
			}
		}

		private void JobContainerControl_MouseDown(object sender,MouseEventArgs e) {
			if(!_isDocked) {//No dragging when it's in its own form window.
				return;
			}
			this.DoDragDrop(this,DragDropEffects.Move);
		}

		private void JobContainerControl_DragEnter(object sender,DragEventArgs e) {
			if(e.Data.GetDataPresent(typeof(JobContainerControl))) {
				panelHighlight.BringToFront();
				panelHighlight.Visible=true;
				e.Effect=DragDropEffects.Move;
			}
		}

		private void JobContainerControl_DragLeave(object sender,EventArgs e) {
			panelHighlight.SendToBack();
			panelHighlight.Visible=false;
		}

		private void JobContainerControl_DragDrop(object sender,DragEventArgs e) {
			JobContainerControl controlSource=(JobContainerControl)e.Data.GetData(typeof(JobContainerControl));
			//There was a glitch where the client point was different from the form point so PointToClient has to be used when detecting control locations.
			JobContainerControl controlDest=(JobContainerControl)_flowPanel.GetChildAtPoint(_flowPanel.PointToClient(new Point(e.X,e.Y)));
			if(controlSource==null || controlDest==null) {
				return;
			}
			if(controlSource==controlDest){
				controlSource.panelHighlight.SendToBack();
				controlSource.panelHighlight.Visible=false;
				return;
			}
			controlSource.panelHighlight.SendToBack();
			controlSource.panelHighlight.Visible=false;
			controlDest.panelHighlight.SendToBack();
			controlDest.panelHighlight.Visible=false;
			int destIdx=_flowPanel.Controls.GetChildIndex(controlDest);
			_flowPanel.Controls.SetChildIndex(controlSource,destIdx);
		}

		///<summary>Closes the parent form of this control if it is currently showing in a separate form (not docked).</summary>
		public void CloseForm() {
			if(this.Parent.GetType()==typeof(FormJobContainer)) {
				FormJobContainer FormJC=((FormJobContainer)this.Parent);
				if(!FormJC.IsDisposed) {
					FormJC.Close();
				}
			}
		}

		private void butLeft_Click(object sender,EventArgs e) {
			JobContainerControl controlFirst=(JobContainerControl)((System.Windows.Forms.Button)sender).Parent;
			int idxA=_flowPanel.Controls.GetChildIndex(controlFirst);
			if(idxA==0) {//Control is already the first one.
				return;
			}
			JobContainerControl controlSecond=(JobContainerControl)_flowPanel.Controls[idxA-1];
			int idxB=idxA-1;
			_flowPanel.Controls.SetChildIndex(controlFirst,idxB);
		}

		private void butRight_Click(object sender,EventArgs e) {
			JobContainerControl controlFirst=(JobContainerControl)((System.Windows.Forms.Button)sender).Parent;
			int idxA=_flowPanel.Controls.GetChildIndex(controlFirst);
			if(idxA==_flowPanel.Controls.Count-1) {//Control is already the last one.
				return;
			}
			JobContainerControl controlSecond=(JobContainerControl)_flowPanel.Controls[idxA+1];
			int idxB=idxA+1;
			_flowPanel.Controls.SetChildIndex(controlFirst,idxB);
		}

		private void butDock_Click(object sender,EventArgs e) {
			if(_isDocked) {
				butLeft.Visible=false;
				butRight.Visible=false;
				butDock.ImageIndex=0;
				_flowPanel.Controls.Remove(this);
				FormJobContainer FormJC=new FormJobContainer(this,_title);
				this.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
				FormJC.Show();
				_isDocked=false;
				AllowDragging=false;
			}
			else {//Control is currently in its own form.
				if(this.Parent.GetType()!=typeof(FormJobContainer)) {
					return;//Should never happen...
				}
				FormJobContainer parent=(FormJobContainer)this.Parent;
				this.Anchor=AnchorStyles.None;
				_flowPanel.Controls.Add(this);
				butDock.ImageIndex=1;
				butLeft.Visible=true;
				butRight.Visible=true;
				parent.Close();
				_isDocked=true;
				AllowDragging=true;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			if(!_isDocked) {
				this.Parent.Dispose();//Close the parent window if it's "popped out"
			}
			else {//Close the control if it's "popped in"
				this.Dispose();
			}
		}

	}
}
