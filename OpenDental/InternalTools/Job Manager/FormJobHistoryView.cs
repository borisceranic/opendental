using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormJobHistoryView:Form {
		private JobEvent _jobEvent;

		///<summary>Opens with links to the passed in JobNum.</summary>
		public FormJobHistoryView(long jobEventNum) {
			_jobEvent=JobEvents.GetOne(jobEventNum);
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobHistoryView_Load(object sender,EventArgs e) {
			textDateEntry.Text=_jobEvent.DateTimeEntry.ToShortDateString()+" "+_jobEvent.DateTimeEntry.ToShortTimeString();
			textJobNum.Text=_jobEvent.JobNum.ToString();
			textOwner.Text=Userods.GetName(_jobEvent.Owner);
			textStatus.Text=Enum.GetName(typeof(JobStatus),(int)_jobEvent.Status);
			textDescription.Text=_jobEvent.Description;
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}

	}
}