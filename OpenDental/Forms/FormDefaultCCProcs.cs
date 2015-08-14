using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormDefaultCCProcs:Form {
		private string _defaultCCProcs;

		public FormDefaultCCProcs() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDefaultCCProcs_Load(object sender,EventArgs e) {
			_defaultCCProcs=PrefC.GetString(PrefName.DefaultCCProcs);
			FillProcs();
		}

		private void FillProcs() {
			listProcs.Items.Clear();
			if(String.IsNullOrEmpty(_defaultCCProcs)) {
				return;
			}
			string[] arrayProcCodes=_defaultCCProcs.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<arrayProcCodes.Length;i++) {
				listProcs.Items.Add(arrayProcCodes[i]+"- "+ProcedureCodes.GetLaymanTerm(ProcedureCodes.GetProcCode(arrayProcCodes[i]).CodeNum));
			}
		}

		private void butAddProc_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			string procCode=ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum);
			List<string> procsForCards=_defaultCCProcs.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).ToList();
			if(procsForCards.Exists(x => x==procCode)) {
				return;
			}
			procsForCards.Add(procCode);
			_defaultCCProcs=String.Join(",",procsForCards);
			FillProcs();
		}

		private void butRemoveProc_Click(object sender,EventArgs e) {
			if(listProcs.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a procedure first.");
				return;
			}
			List<string> procsForCards=_defaultCCProcs.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).ToList();
			procsForCards.RemoveAt(listProcs.SelectedIndex);
			_defaultCCProcs=String.Join(",",procsForCards);
			FillProcs();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Prefs.UpdateString(PrefName.DefaultCCProcs,_defaultCCProcs)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}