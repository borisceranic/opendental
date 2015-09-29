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

		///<summary>Creates a draggable control containing the control passed in that will be added to the flow panel passed in.</summary>
		public JobContainerControl(Control jobControl,FlowLayoutPanel flowPanel) {
			InitializeComponent();
			butMerge.ImageIndex=1;
			_flowPanel=flowPanel;
			this.Controls.Add(jobControl);
			jobControl.SetBounds(0,10,jobControl.Width,jobControl.Height);//10 height for the buttons at the top.
			jobControl.Anchor=(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
			_flowPanel.Controls.Add(this);
		}

		private void JobContainerControl_MouseDown(object sender,MouseEventArgs e) {
			if(!_isDocked) {//No dragging when it's in its own form window.
				return;
			}
			this.DoDragDrop(this,DragDropEffects.Move);
		}

		private void JobContainerControl_DragEnter(object sender,DragEventArgs e) {
			if(e.Data.GetDataPresent(typeof(JobContainerControl))) {
				this.BackColor=Color.Blue;
				e.Effect=DragDropEffects.Move;
			}
		}

		private void JobContainerControl_DragLeave(object sender,EventArgs e) {
			this.BackColor=Color.Transparent;
		}

		private void JobContainerControl_DragDrop(object sender,DragEventArgs e) {
			JobContainerControl sourceControl=(JobContainerControl)e.Data.GetData(typeof(JobContainerControl));
			JobContainerControl destControl=(JobContainerControl)_flowPanel.GetChildAtPoint(new Point(e.X,e.Y));
			if(sourceControl==null || destControl==null) {
				return;
			}
			if(sourceControl==destControl){
				sourceControl.BackColor=Color.Transparent;
			}
			sourceControl.BackColor=Color.Transparent;
			destControl.BackColor=Color.Transparent;
			int destIdx=_flowPanel.Controls.GetChildIndex(destControl);
			_flowPanel.Controls.SetChildIndex(sourceControl,destIdx);
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
			_flowPanel.Controls.SetChildIndex(controlSecond,idxA);
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
			_flowPanel.Controls.SetChildIndex(controlSecond,idxA);
		}

		private void butMerge_Click(object sender,EventArgs e) {
			if(_isDocked) {
				butLeft.Visible=false;
				butRight.Visible=false;
				butMerge.ImageIndex=0;
				_flowPanel.Controls.Remove(this);
				FormTest formT=new FormTest(this);
				formT.Show(_flowPanel.Parent);
				_isDocked=false;
			}
			else {//Control is currently in its own form.
				if(this.Parent.GetType()!=typeof(FormTest)) {
					return;//Should never happen...
				}
				FormTest parent=(FormTest)this.Parent;
				this.Anchor=AnchorStyles.None;
				_flowPanel.Controls.Add(this);
				butMerge.ImageIndex=1;
				butLeft.Visible=true;
				butRight.Visible=true;
				parent.Close();
				_isDocked=true;
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
