using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTaskNoteEdit:Form {
		public TaskNote TaskNoteCur;
		///<summary>Only called when DialogResult is OK (for OK button and sometimes delete button).</summary>
		public delegate void DelegateEditComplete(object sender);
		///<summary>Called when this form is closed.</summary>
		public DelegateEditComplete EditComplete;

		public FormTaskNoteEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTaskNoteEdit_Load(object sender,EventArgs e) {
			textDateTime.Text=TaskNoteCur.DateTimeNote.ToString();
			textUser.Text=Userods.GetName(TaskNoteCur.UserNum);
			textNote.Text=TaskNoteCur.Note;
			this.Top+=150;
			if(TaskNoteCur.IsNew) {
				textDateTime.ReadOnly=true;
			}
			else if(!Security.IsAuthorized(Permissions.TaskNoteEdit)) {//Tasknotes are not editable unless user has TaskNoteEdit permission.
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void OnEditComplete() {
			if(EditComplete!=null) {
				EditComplete(this);
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			if(TaskNoteCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				Close();//Needed because the window is called as a non-modal window.
				return;
			}
			TaskNotes.Delete(TaskNoteCur.TaskNoteNum);
			DialogResult=DialogResult.OK;
			OnEditComplete();
			Tasks.TaskEditCreateLog(Lan.g(this,"Deleted note from task"),Tasks.GetOne(TaskNoteCur.TaskNum));
			Close();//Needed because the window is called as a non-modal window.
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textNote.Text=="") {
				MsgBox.Show(this,"Please enter a note, or delete this entry.");
				return;
			}
			try {
				TaskNoteCur.DateTimeNote=DateTime.Parse(textDateTime.Text);
			}
			catch{
				MsgBox.Show(this,"Please fix date.");
				return;
			}
			TaskNoteCur.Note=textNote.Text;
			if(TaskNoteCur.IsNew) {
				TaskNotes.Insert(TaskNoteCur);
				Tasks.TaskEditCreateLog(Lan.g(this,"Added task note"),Tasks.GetOne(TaskNoteCur.TaskNum));
			}
			else {
				TaskNotes.Update(TaskNoteCur);
				Tasks.TaskEditCreateLog(Lan.g(this,"Task note changed"),Tasks.GetOne(TaskNoteCur.TaskNum));
			}
			DialogResult=DialogResult.OK;
			OnEditComplete();
			Close();//Needed because the window is called as a non-modal window.
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();//Needed because the window is called as a non-modal window.
		}

		
	
	}
}