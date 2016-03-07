using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPhoneEmpDefaultEscalationEdit:ODForm {

		///<summary>Master list of employees. Only get this once.</summary>
		List<PhoneEmpDefault> _listPED=PhoneEmpDefaults.Refresh();
			
		public FormPhoneEmpDefaultEscalationEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEscalationTeamHQ_Load(object sender,System.EventArgs e) {
			//Get all employees.
			_listPED=PhoneEmpDefaults.Refresh();
			//Sort by name.
			_listPED.Sort(new PhoneEmpDefaults.PhoneEmpDefaultComparer(PhoneEmpDefaults.PhoneEmpDefaultComparer.SortBy.name));
			//Create escalation list from employees.
			List<PhoneEmpDefault> listEsc=new List<PhoneEmpDefault>();
			for(int i=0;i<_listPED.Count;i++) {
				PhoneEmpDefault ped=_listPED[i];
				if(ped.EscalationOrder>=1) {
					listEsc.Add(ped);
				}
			}
			//Sort escalation list.
			listEsc.Sort(new PhoneEmpDefaults.PhoneEmpDefaultComparer(PhoneEmpDefaults.PhoneEmpDefaultComparer.SortBy.escalation));
			//Fill the grids.
			FillGrids(listEsc);
		}

		private void FillGrids(List<PhoneEmpDefault> listEsc) {
			//Fill escalation grid.
			gridEscalation.BeginUpdate();
			gridEscalation.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Employee",gridEscalation.Width);
			gridEscalation.Columns.Add(col);
			gridEscalation.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listEsc.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listEsc[i].EmpName.ToString());
				row.Tag=listEsc[i];
				gridEscalation.Rows.Add(row);
				//Set escalation order for this employee.
				//Must happen after the add in order to keep the Escalation order 1-based.
				listEsc[i].EscalationOrder=gridEscalation.Rows.Count;
			}
			gridEscalation.EndUpdate();
			//Fill employee grid.
			gridEmployees.BeginUpdate();
			gridEmployees.Columns.Clear();
			col=new ODGridColumn("Employee",gridEmployees.Width);
			gridEmployees.Columns.Add(col);
			gridEmployees.Rows.Clear();
			for(int i=0;i<_listPED.Count;i++) {
				row=new ODGridRow();
				//Omit employee who are already included in escalation grid.
				if(PhoneEmpDefaults.GetEmpDefaultFromList(_listPED[i].EmployeeNum,listEsc)!=null) {
					continue;
				}
				row.Cells.Add(_listPED[i].EmpName.ToString());
				row.Tag=_listPED[i];
				gridEmployees.Rows.Add(row);
			}
			gridEmployees.EndUpdate();	
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridEmployees.SelectedIndices.Length<=0) {
				return;
			}
			List<PhoneEmpDefault> listKeep=new List<PhoneEmpDefault>();
			//Add existing escalation to top of keepers.
			for(int i=0;i<gridEscalation.Rows.Count;i++) {
				listKeep.Add((PhoneEmpDefault)gridEscalation.Rows[i].Tag);
			}
			List<int> selectedIndices=new List<int>(gridEmployees.SelectedIndices);
			//Add selected employees to bottom of keepers.
			for(int i=0;i<gridEmployees.Rows.Count;i++) {
				if(!selectedIndices.Contains(i)) {
					continue;
				}
				PhoneEmpDefault pedKeep=(PhoneEmpDefault)gridEmployees.Rows[i].Tag;
				if(PhoneEmpDefaults.GetEmpDefaultFromList(pedKeep.EmployeeNum,listKeep)!=null) { //Already a keeper.
					continue;
				}
				listKeep.Add((PhoneEmpDefault)gridEmployees.Rows[i].Tag);
			}
			FillGrids(listKeep);
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(gridEscalation.SelectedIndices.Length<=0) {
				return;
			}
			List<PhoneEmpDefault> listKeep=new List<PhoneEmpDefault>();
			List<int> selectedIndices=new List<int>(gridEscalation.SelectedIndices);
			for(int i=0;i<gridEscalation.Rows.Count;i++) {
				if(selectedIndices.Contains(i)) { //Only remove selected escalation employees.
					continue;
				}
				listKeep.Add((PhoneEmpDefault)gridEscalation.Rows[i].Tag);
			}
			FillGrids(listKeep);
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridEscalation.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Select 1 item from escalation list");
				return;
			}
			if(gridEscalation.SelectedIndices[0]==0) {
				return;
			}
			//Retain current selection.
			int curSelectedIndex=Math.Max(gridEscalation.SelectedIndices[0]-1,0);
			List<PhoneEmpDefault> listKeep=new List<PhoneEmpDefault>();
			List<int> selectedIndices=new List<int>(gridEscalation.SelectedIndices);
			for(int i=0;i<gridEscalation.Rows.Count;i++) {
				PhoneEmpDefault ped=(PhoneEmpDefault)gridEscalation.Rows[i].Tag;
				if(selectedIndices[0]==i+1) {
					ped.EscalationOrder++;
				}
				else if(selectedIndices[0]==i) {
					ped.EscalationOrder--;
				}
				listKeep.Add((PhoneEmpDefault)gridEscalation.Rows[i].Tag);
			}
			listKeep.Sort(new PhoneEmpDefaults.PhoneEmpDefaultComparer(PhoneEmpDefaults.PhoneEmpDefaultComparer.SortBy.escalation));
			FillGrids(listKeep);
			//Reset selection so moving up the list rapidly is easier.
			gridEscalation.SetSelected(curSelectedIndex,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridEscalation.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Select 1 item from escalation list");
				return;
			}
			if(gridEscalation.SelectedIndices[0]>=(gridEscalation.Rows.Count-1)) {
				return;
			}
			//Retain current selection.
			int curSelectedIndex=Math.Min(gridEscalation.SelectedIndices[0]+1,(gridEscalation.Rows.Count-1));
			List<PhoneEmpDefault> listKeep=new List<PhoneEmpDefault>();
			List<int> selectedIndices=new List<int>(gridEscalation.SelectedIndices);
			for(int i=0;i<gridEscalation.Rows.Count;i++) {
				PhoneEmpDefault ped=(PhoneEmpDefault)gridEscalation.Rows[i].Tag;
				if(selectedIndices[0]==i) {
					ped.EscalationOrder++;
				}
				else if(selectedIndices[0]==i-1) {
					ped.EscalationOrder--;
				}
				listKeep.Add((PhoneEmpDefault)gridEscalation.Rows[i].Tag);
			}
			listKeep.Sort(new PhoneEmpDefaults.PhoneEmpDefaultComparer(PhoneEmpDefaults.PhoneEmpDefaultComparer.SortBy.escalation));
			FillGrids(listKeep);
			//Reset selection so moving down the list rapidly is easier.
			gridEscalation.SetSelected(curSelectedIndex,true);
		}

		private void butOK_Click(object sender,System.EventArgs e) {			
			//Build list of escalation employees.
			List<PhoneEmpDefault> listKeep=new List<PhoneEmpDefault>();
			for(int i=0;i<gridEscalation.Rows.Count;i++) {
				PhoneEmpDefault ped=(PhoneEmpDefault)gridEscalation.Rows[i].Tag;
				listKeep.Add(ped);
				ped.EscalationOrder=listKeep.Count;
			}
			//Update the db.
			PhoneEmpDefaults.UpdateEscalationOrder(listKeep);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			this.DialogResult=DialogResult.Cancel;
		}

		private void gridEmployees_CellDoubleClick(object sender,ODGridClickEventArgs e) {

		}
	}
}
