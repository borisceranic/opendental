using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormJobLinks:Form {
		private long _jobNum;
		///<summary>A list of bugs, features, and tasks related to this job.</summary>
		private List<JobLink> _jobLinks;
		private bool _hasChanged=false;

		///<summary>Opens with links to the passed in JobNum.</summary>
		public FormJobLinks(long jobNum) {
			_jobNum=jobNum;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobLinks_Load(object sender,EventArgs e) {
			_jobLinks=JobLinks.GetJobLinks(_jobNum);
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Type",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Description",200);
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_jobLinks.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_jobLinks[i].LinkType.ToString());
				if(_jobLinks[i].LinkType==JobLinkType.Task) {
					Task task=Tasks.GetOne(_jobLinks[i].FKey);
					if(task.Descript.Length>=80) {
						row.Cells.Add(task.Descript.Substring(0,80)+"...");
					}
					else {
						row.Cells.Add(task.Descript);
					}
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Bug) {
					row.Cells.Add("Under Construction");
				}
				else if(_jobLinks[i].LinkType==JobLinkType.Request) {
					row.Cells.Add("Under Construction");
				}
				row.Tag=_jobLinks[i].JobLinkNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			switch (_jobLinks[gridMain.SelectedIndices[0]].LinkType) {
				case JobLinkType.Task:
					Task task=Tasks.GetOne(_jobLinks[gridMain.SelectedIndices[0]].FKey);
					FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
					FormTE.ShowDialog();
					FillGrid();
					break;
				case JobLinkType.Request:
					break;
				case JobLinkType.Bug:
					break;
			}
		}

		private void butLinkBug_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butLinkTask_Click(object sender,EventArgs e) {
			FormTaskSearch FormTS=new FormTaskSearch();
			FormTS.IsSelectionMode=true;
			if(FormTS.ShowDialog()==DialogResult.OK) {
				JobLink jobLink=new JobLink();
				jobLink.JobNum=_jobNum;
				jobLink.FKey=FormTS.SelectedTaskNum;
				jobLink.LinkType=JobLinkType.Task;
				JobLinks.Insert(jobLink);
				_jobLinks=JobLinks.GetJobLinks(_jobNum);
				_hasChanged=true;
				FillGrid();
			}
		}

		private void butLinkFeatReq_Click(object sender,EventArgs e) {
			//FormFeatureRequest FormFR=new FormFeatureRequest();
			MsgBox.Show(this,"This functionality is not yet implemented. Stay tuned for updates.");
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Select a link to remove first.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to remove this link?")) {
				return;
			}
			JobLinks.Delete((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			_hasChanged=true;
			_jobLinks=JobLinks.GetJobLinks(_jobNum);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void FormJobLinks_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				Signalods.SetInvalid(InvalidType.Job);
			}
		}

	}
}