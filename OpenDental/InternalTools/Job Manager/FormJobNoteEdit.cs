using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobNoteEdit:Form {
		public JobNote _jobNote;
		///<summary>Only called when DialogResult is OK (for OK button and sometimes delete button).</summary>
		public delegate void DelegateEditComplete(object sender);

		public FormJobNoteEdit(JobNote jobNote) {
			_jobNote=jobNote;
			InitializeComponent();
			Lan.F(this);
		}

		public JobNote JobNoteCur {
			get {
				return _jobNote;
			}
		}

		private void FormJobNoteEdit_Load(object sender,EventArgs e) {
			if(_jobNote.IsNew) {
				textDateTime.Text=DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString();
			}
			else {
				textDateTime.Text=_jobNote.DateTimeNote.ToString();
			}
			textUser.Text=Userods.GetName(_jobNote.UserNum);
			textNote.Text=_jobNote.Note;
			textDateTime.ReadOnly=true;
			if(Security.CurUser.UserNum!=_jobNote.UserNum) {
				textNote.ReadOnly=true;
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			if(_jobNote.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			JobNotes.Delete(_jobNote.JobNoteNum);
			_jobNote=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textNote.Text=="") {
				MsgBox.Show(this,"Please enter a note, or delete this entry.");
				return;
			}
			try {
				_jobNote.DateTimeNote=DateTime.Parse(textDateTime.Text);
			}
			catch{
				MsgBox.Show(this,"Please fix date.");
				return;
			}
			_jobNote.Note=textNote.Text;
			if(_jobNote.IsNew) {
				JobNotes.Insert(_jobNote);
			}
			else {
				JobNotes.Update(_jobNote);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	
	}
}