using CodeBase;
using System;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>Launch this window in a separate thread so that the progress bar can smoothly spin without waiting on the main thread.
	///Send the phrase "DEFCON 1" in order to have the window gracefully close (as to not rely on thread abort).</summary>
	public partial class FormProgressStatus:Form {
		private string _odEventName;

		///<summary>Launches a progress window that will constantly spin and display status updates for global ODEvents with corresponding name.</summary>
		public FormProgressStatus(string odEventName) {
			InitializeComponent();
			Lan.F(this);
			_odEventName=odEventName;
			//Registers this form for any progress status updates that happen throughout the entire program.
			ODEvent.Fired+=ODEvent_Fired;
		}

		private void ODEvent_Fired(ODEventArgs e) {
			//We don't know what thread will cause a progress status change, so invoke this method as a delegate if necessary.
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate() { ODEvent_Fired(e); });
				return;
			}
			//Make sure that this ODEvent is for FormProgressStatus and that the Tag is not null and is a string.
			if(e.Name!=_odEventName || e.Tag==null || e.Tag.GetType()!=typeof(string)) {
				return;
			}
			string status=((string)e.Tag);
			//When the developer wants to close the window, they will send an ODEvent with "DEFCON 1" to signal this form to shut everything down.
			if(status.ToUpper()=="DEFCON 1") {
				DialogResult=DialogResult.OK;
				Close();
				return;
			}
			labelMsg.Text=status;
			Application.DoEvents();//So that the label updates with the new status.
		}

		private void FormProgressStatus_FormClosing(object sender,FormClosingEventArgs e) {
			ODEvent.Fired-=ODEvent_Fired;
		}
	}
}