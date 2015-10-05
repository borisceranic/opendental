using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormProcBroken:Form {
		public bool IsNew;
		private Procedure _procCur;
		private Procedure _procOld;
		private long _provNumSelected;
		private List<Provider> _listProvs;
		private Clinic[] _listClinics;

		public FormProcBroken(Procedure proc) {
			_procCur=proc;
			_procOld=proc.Copy();
			InitializeComponent();
			Lan.F(this);
		}

		private void FormProcBroken_Load(object sender,EventArgs e) {
			textDateEntry.Text=_procCur.DateEntryC.ToShortDateString();
			textProcDate.Text=_procCur.ProcDate.ToShortDateString();
			textAmount.Text=_procCur.ProcFee.ToString("f");
			_listProvs=ProviderC.GetListShort();
			_provNumSelected=_procCur.ProvNum;
			comboProvNum.Items.Clear();
			for(int i=0;i<_listProvs.Count;i++) {
				comboProvNum.Items.Add(_listProvs[i].GetLongDesc());//Only visible provs added to combobox.
				if(_listProvs[i].ProvNum==_procCur.ProvNum) {
					comboProvNum.SelectedIndex=i;//Sets combo text too.
				}
			}
			if(comboProvNum.SelectedIndex==-1) {//The provider exists but is hidden, or selection is optional.
				comboProvNum.Text=Providers.GetLongDesc(_provNumSelected);//Appends "(hidden)" to the end of the long description.
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Items.Add("none");
				comboClinic.SelectedIndex=0;
				_listClinics=Clinics.List;
				for(int i=0;i<_listClinics.Length;i++) {
					comboClinic.Items.Add(_listClinics[i].Description);
					if(_listClinics[i].ClinicNum==_procCur.ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			else {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			textUser.Text=Userods.GetName(_procCur.UserNum);
			textChartNotes.Text=_procCur.Note;
			textAccountNotes.Text=_procCur.BillingNote;
		}

		private void comboProvNum_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelected=_listProvs[comboProvNum.SelectedIndex].ProvNum;
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormP=new FormProviderPick();
			if(comboProvNum.SelectedIndex>-1) {//Initial FormP selection if selected prov is not hidden.
				FormP.SelectedProvNum=_provNumSelected;
			}
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProvNum.SelectedIndex=Providers.GetIndexLong(FormP.SelectedProvNum,_listProvs);
			_provNumSelected=FormP.SelectedProvNum;
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex==0) {//User selected "none"
				_procCur.ClinicNum=0;
			}
			else {
				_procCur.ClinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
		}

		private void butAutoNoteChart_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textChartNotes.AppendText(FormA.CompletedNote);
			}
		}

		private void butAutoNoteAccount_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textAccountNotes.AppendText(FormA.CompletedNote);
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(textProcDate.errorProvider1.GetError(textProcDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textProcDate.Text=="") {
				MsgBox.Show(this,"Please enter a date first.");
				return;
			}
			if(textAmount.Text=="") {
				MsgBox.Show(this,"Please enter an amount.");
				return;
			}
			_procCur.ProcDate=PIn.Date(textProcDate.Text);
			_procCur.ProcFee=PIn.Double(textAmount.Text);
			_procCur.Note=textChartNotes.Text;
			_procCur.BillingNote=textAccountNotes.Text;
			_procCur.ProvNum=_provNumSelected;
			Procedures.Update(_procCur,_procOld);
			ProcedureCode procedureCode=ProcedureCodes.GetProcCode(_procCur.CodeNum);
			string logText=procedureCode.ProcCode+", "+Lan.g(this,"Fee")+": "+_procCur.ProcFee.ToString("c")+", "+procedureCode.Descript;
			SecurityLogs.MakeLogEntry(Permissions.ProcComplEdit,_procCur.PatNum,logText);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			if(IsNew) {
				Procedures.Delete(_procCur.ProcNum);
			}
			DialogResult=DialogResult.Cancel;
		}

	}
}