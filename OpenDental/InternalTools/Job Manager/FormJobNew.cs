using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;
using System.Linq;

namespace OpenDental {
	public partial class FormJobNew:ODForm {
		public Job JobCur=new Job();

		public FormJobNew() {
			InitializeComponent();
		}

		private void FormJobNew_Load(object sender,EventArgs e) {
			if(JobCur==null) {
				JobCur=new Job();
			}
			JobCur.IsNew=true;
			Text="New Job"+(JobCur.Title.Length>0?" - "+JobCur.Title:"");
			controlJobEdit.LoadJob(JobCur);
		}

		private void userControlJobEdit1_SaveClick(object sender,EventArgs e) {
			JobCur=controlJobEdit.GetJob();
			DialogResult=DialogResult.OK;
		}

		private void controlJobEdit_TitleChanged(object sender,string title) {
			Text="New Job"+(title.Length>0?" - "+title:"")+(controlJobEdit.IsChanged?"*":"");
		}

		private void FormJobNew_FormClosing(object sender,FormClosingEventArgs e) {
			if(!controlJobEdit.IsChanged) {
				return;
			}
			if(controlJobEdit.IsChanged) {
				switch(MessageBox.Show("Save changes to current job?","",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.OK:
					case DialogResult.Yes:
						controlJobEdit.ForceSave();
						DialogResult=DialogResult.OK;
						break;
					case DialogResult.No:
						DialogResult=DialogResult.Cancel;
						break;
					case DialogResult.Cancel:
						//do not load or navigate to new job.
						//no dialog result
						e.Cancel=true;
						return;
				}
			}
		}

	}
}