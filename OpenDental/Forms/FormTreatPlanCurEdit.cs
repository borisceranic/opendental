using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTreatPlanCurEdit:Form {
		public TreatPlan TreatPlanCur;
		private TreatPlan _treatPlanUnassigned;
		private List<TreatPlanAttach> _listTreatPlanAttaches;
		private List<Procedure> _listProceduresTPAll;
		private List<Procedure> _listProceduresTPCur;
		private List<TreatPlanAttach> _listAllAttaches;
		private long _apptNum;
		private long _apptNumPlanned;

		public FormTreatPlanCurEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTreatPlanCurEdit_Load(object sender,EventArgs e) {
			if(TreatPlanCur==null || (TreatPlanCur.TPStatus!=TreatPlanStatus.Active && TreatPlanCur.TPStatus!=TreatPlanStatus.Inactive)) {
				throw new Exception("No treatment plan loaded.");
			}
			_treatPlanUnassigned=TreatPlans.GetUnassigned();
			this.Text=TreatPlanCur.Heading+" - {"+Lans.g(this,TreatPlanCur.TPStatus.ToString())+"}";
			_listProceduresTPAll=Procedures.GetProcsByStatusForPat(TreatPlanCur.PatNum,new[] {ProcStat.TP,ProcStat.TPi});
			_listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(TreatPlanCur.TreatPlanNum);
			_listProceduresTPCur=_listProceduresTPAll.FindAll(x => _listTreatPlanAttaches.Any(y => x.ProcNum==y.ProcNum) 
				|| (TreatPlanCur.TPStatus==TreatPlanStatus.Active && (x.AptNum>0 || x.PlannedAptNum>0)));
			_listAllAttaches=TreatPlanAttaches.GetAllForPatNum(TreatPlanCur.PatNum);
			textHeading.Text=TreatPlanCur.Heading;
			textNote.Text=TreatPlanCur.Note;
			FillGrids();
			if(TreatPlanCur.TPStatus==TreatPlanStatus.Inactive && TreatPlanCur.Heading==Lan.g("TreatPlan","Unassigned")) {
				gridTP.Title=Lan.g("TreatPlan","Unassigned Procedures");
				labelHeading.Visible=false;
				textHeading.Visible=false;
				labelNote.Visible=false;
				textNote.Visible=false;
				gridAll.Visible=false;
				butLeft.Visible=false;
				butRight.Visible=false;
				butOK.Enabled=false;
				butDelete.Visible=false;
			}
			if(TreatPlanCur.TPStatus==TreatPlanStatus.Active) {
				butMakeActive.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void FillGrids() {
			FillGrid(ref gridTP);
			FillGrid(ref gridAll);
		}

		///<summary>Both grid s should be filled with the same columns. This method prevents the need for two nearly identical fill grid patterns.</summary>
		private void FillGrid(ref ODGrid grid) {
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Status"),40));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Tth"),30));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Surf"),40));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Code"),40));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Description"),200));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"TPs"),40));
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Apt"),40));
			grid.Rows.Clear();
			ODGridRow row;
			List<Procedure> procs;
			if(grid==gridAll) {
				procs=_listProceduresTPAll.FindAll(x => _listProceduresTPCur.All(y => x.ProcNum!=y.ProcNum)).OrderBy(x => PIn.Int(x.ToothNum)).ToList();
			}
			else {
				procs=_listProceduresTPCur.OrderBy(x => PIn.Int(x.ToothNum)).ToList();
			}
			foreach(Procedure proc in procs) {
				row=new ODGridRow();
				ProcedureCode proccode=ProcedureCodes.GetProcCode(proc.CodeNum);
				row.Cells.Add(proc.ProcStatus.ToString());
				row.Cells.Add(proc.ToothNum);
				row.Cells.Add(proc.Surf);
				row.Cells.Add(proccode.ProcCode);
				row.Cells.Add(proccode.LaymanTerm);
				row.Cells.Add(_listAllAttaches.FindAll(x => x.ProcNum==proc.ProcNum && x.TreatPlanNum!=_treatPlanUnassigned.TreatPlanNum).Count.ToString());
				row.Cells.Add(proc.AptNum>0 || proc.PlannedAptNum>0?"X":"");
				row.Tag=proc;
				grid.Rows.Add(row);
			}
			grid.EndUpdate();
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(gridAll.GetSelectedIndex()<0) {
				return;
			}
			foreach(int idx in gridAll.SelectedIndices) {
				_listProceduresTPCur.Add((Procedure)gridAll.Rows[idx].Tag);
			}
			FillGrids();
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridTP.GetSelectedIndex()<0) {
				return;
			}
			foreach(int idx in gridTP.SelectedIndices) {
				if(TreatPlanCur.TPStatus==TreatPlanStatus.Active //Active TP
					&&(((Procedure)gridTP.Rows[idx].Tag).AptNum>0||((Procedure)gridTP.Rows[idx].Tag).PlannedAptNum>0)) //Proc is attached to appointment
				{//if active TP, don't allow scheduled procedures to me moved off the TP.
					continue;
				}
				_listProceduresTPCur.Remove((Procedure)gridTP.Rows[idx].Tag);
			}
			FillGrids();
		}

		private void grids_MouseMove(object sender,MouseEventArgs e) {
			contextMenuProcs.Items.Clear();
			gridAll.ContextMenu=null;
			gridTP.ContextMenu=null;
			ODGrid grid=(ODGrid)sender;
			int row=grid.PointToRow(e.Y);
			if(row<0 || row>=grid.Rows.Count) {
				return;
			}
			Procedure proc=(Procedure)grid.Rows[row].Tag;
			if(proc.AptNum>0) {
				contextMenuProcs.Items.Add(menuItemGotToAppt);
				_apptNum=proc.AptNum;
			}
			if(proc.PlannedAptNum>0) {
				contextMenuProcs.Items.Add(menuItemGoToPlanned);
				_apptNumPlanned=proc.PlannedAptNum;
			}
			if(contextMenuProcs.Items.Count!=0) {
				grid.ContextMenuStrip=contextMenuProcs;
			}
		}

		private void menuItemGotToAppt_Click(object sender,EventArgs e) {
			FormApptEdit FormAPT=new FormApptEdit(_apptNum);
			FormAPT.ShowDialog();
			//consider refreshing data
		}

		private void menuItemGoToPlanned_Click(object sender,EventArgs e) {
			FormApptEdit FormAPT=new FormApptEdit(_apptNumPlanned);
			FormAPT.ShowDialog();
			//consider refreshing data
		}

		private void butMakeActive_Click(object sender,EventArgs e) {
			if(TreatPlanCur.TPStatus==TreatPlanStatus.Inactive && TreatPlanCur.Heading==Lan.g("TreatPlan","Unassigned")) {
				_treatPlanUnassigned=new TreatPlan();
				TreatPlanCur.Heading=Lan.g("TreatPlan","Active Treatment Plan");
				textHeading.Text=TreatPlanCur.Heading;
			}
			TreatPlanCur.TPStatus=TreatPlanStatus.Active;
			butMakeActive.Enabled=false;
			gridTP.Title=Lan.g("TreatPlan","Treatment Planned Procedures");
			labelHeading.Visible=true;
			textHeading.Visible=true;
			labelNote.Visible=true;
			textNote.Visible=true;
			gridAll.Visible=true;
			butLeft.Visible=true;
			butRight.Visible=true;
			butOK.Enabled=true;
			butDelete.Visible=true;
			butDelete.Enabled=false;
			_listProceduresTPCur.RemoveAll(x => x.AptNum>0 || x.PlannedAptNum>0);//to prevent duplicate additions
			_listProceduresTPCur.AddRange(_listProceduresTPAll.FindAll(x => x.AptNum>0 || x.PlannedAptNum>0));
			FillGrids();
		}

		private void butOK_Click(object sender,EventArgs e) {
			TreatPlanCur.Heading=textHeading.Text;
			TreatPlanCur.Note=textNote.Text;
			if(TreatPlanCur.TreatPlanNum==0) {
				TreatPlanCur.TreatPlanNum=TreatPlans.Insert(TreatPlanCur);
			}
			else {
				TreatPlans.Update(TreatPlanCur);
			}
			List<TreatPlanAttach> listNew=_listTreatPlanAttaches.FindAll(x => _listProceduresTPCur.Any(y => x.ProcNum==y.ProcNum));
			_listProceduresTPCur.FindAll(x => listNew.All(y => x.ProcNum!=y.ProcNum))
				.ForEach(x => listNew.Add(new TreatPlanAttach() {TreatPlanNum=TreatPlanCur.TreatPlanNum,ProcNum=x.ProcNum,Priority=x.Priority}));
			TreatPlanAttaches.Sync(listNew,TreatPlanCur.TreatPlanNum);
			if(TreatPlanCur.TPStatus==TreatPlanStatus.Active) {
				TreatPlans.SetActive(TreatPlanCur);
				Procedures.SetTPActive(TreatPlanCur.PatNum,listNew.Select(x => x.ProcNum).ToList());
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(TreatPlanCur.TPStatus==TreatPlanStatus.Active) {
				MsgBox.Show(this,"Cannot delete active treatment plan.");//Should never happen.
				return;
			}
			if(TreatPlanCur.TreatPlanNum!=0) {
				try {
					TreatPlans.Delete(TreatPlanCur);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			DialogResult=DialogResult.OK;
		}

	}
}