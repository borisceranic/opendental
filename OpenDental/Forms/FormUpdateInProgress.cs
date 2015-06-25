using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormUpdateInProgress:Form {
		private string _updateComputerName;

		public FormUpdateInProgress(string updateComputerName) {
			InitializeComponent();
			Lan.F(this);
			_updateComputerName=updateComputerName;
		}

		private void FormUpdaeInProgress_Load(object sender,System.EventArgs e) {
			labelWarning.Text=Lan.g("FormUpdateInProgress","An update is in progress on workstation")+": '"+_updateComputerName+"'.\r\n\r\n"
			+Lan.g(this,"Not allowed to start")+" "+PrefC.GetString(PrefName.SoftwareName)+" "+Lan.g(this,"while an update is in progress.")+"\r\n"
			+Lan.g(this,"If you are the person who started the update and you wish to override this message because an update is not in progress, click 'Override'.")+"\r\n\r\n"
			+Lan.g(this,"Otherwise, please wait and 'Try Again'.");
		}

		private void butTryAgain_Click(object sender,EventArgs e) {
			Prefs.RefreshCache();
			if(PrefC.GetString(PrefName.UpdateInProgressOnComputerName)!="") {
				MessageBox.Show(Lan.g(this,"Workstation")+": '"+_updateComputerName+"' "+Lan.g(this,"is still updating.  Please wait and 'Try Again'"));
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOverride_Click(object sender,EventArgs e) {
			Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");
			MsgBox.Show(this,"You will be allowed access when you restart.");
			DialogResult=DialogResult.Cancel;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}