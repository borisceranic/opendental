using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSubscriberMove:ODForm {

		private InsPlan _intoInsPlan;
		private InsPlan _fromInsPlan;

		public FormSubscriberMove() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSubscriberMove_Load(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanChangeSubsc)) {
				DialogResult=DialogResult.Cancel;
				return;
			}
		}

		private void butChangePatientInto_Click(object sender,EventArgs e) {
			FormInsPlans formIP=new FormInsPlans();
			formIP.IsSelectMode=true;
			if(formIP.ShowDialog()==DialogResult.OK) {
				_intoInsPlan=formIP.SelectedPlan;
				textCarrierNameInto.Text=Carriers.GetName(_intoInsPlan.CarrierNum);
			}
		}

		private void butChangePatientFrom_Click(object sender,EventArgs e) {
			FormInsPlans formIP=new FormInsPlans();
			formIP.IsSelectMode=true;
			if(formIP.ShowDialog()==DialogResult.OK) {
				_fromInsPlan=formIP.SelectedPlan;
				textCarrierNameFrom.Text=Carriers.GetName(_fromInsPlan.CarrierNum);
			}
		}

		private void butViewInsPlanInto_Click(object sender,EventArgs e) {
			if(_intoInsPlan==null) {
				MsgBox.Show(this,"Insurance plan not selected.\r\nPlease select an insurance plan using the picker button.");
				return;
			}
			FormInsPlan formIP=new FormInsPlan(_intoInsPlan,null,null);
			formIP.ShowDialog();
		}

		private void butViewInsPlanFrom_Click(object sender,EventArgs e) {
			if(_fromInsPlan==null) {
				MsgBox.Show(this,"Insurance plan not selected.\r\nPlease select an insurance plan using the picker button.");
				return;
			}
			FormInsPlan formIP=new FormInsPlan(_fromInsPlan,null,null);
			formIP.ShowDialog();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_fromInsPlan==null) {
				MsgBox.Show(this,"Please pick a carrier to move subscribers from.");
				return;
			}
			if(_intoInsPlan==null) {
				MsgBox.Show(this,"Please pick a carrier to move subscribers to.");
				return;
			}
			if(_fromInsPlan.PlanNum==_intoInsPlan.PlanNum) {
				MsgBox.Show(this,"Can not move a plan into itself.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Globally move all subscribers of the plan at the bottom over to the plan at the top and hide the plan at the bottom?  This will also remove any benefits which are patient specific from the plan on botom.  Patient specific benefits are not commonly used.  This will not copy the subscriber notes or the benefit notes, and will also not copy the effective dates.  This tool is database intensive and can take several minutes to run.  Consider running this tool after business hours or when network usage is typically low.  The changes made will be irreversible.  Please make a full backup before continuing.\r\n\r\nClick OK to continue, or click Cancel if you are not sure.")) {
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				long insSubModifiedCount=InsSubs.MoveSubscribers(_fromInsPlan.PlanNum,_intoInsPlan.PlanNum);
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Count of Subscribers Moved")+": "+insSubModifiedCount);
			}
			catch(ApplicationException ex) {//The tool was blocked due to validation failure.
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(ex.Message);//No translaion here, because translation was done in the business layer.
				msgBox.ShowDialog();
				return;//Since this exception is due to validation failure, do not close the form.  Let the user manually click Cancel so they know what happened.
			}
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeSubsc,0,"Subscriber Move");
			DialogResult=DialogResult.OK;//Closes the form.
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//probably don't need this log entry, but here to maintain old behavior
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeSubsc,0,"Subscriber Move Cancel");
			DialogResult=DialogResult.Cancel;//Closes the form.
		}

	}
}