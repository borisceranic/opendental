using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEvaluationDefEdit:Form {
		private EvaluationDef _evalDefCur;
		private List<EvaluationCriterionDef> _criterionDefsAvailable;
		private List<EvaluationCriterionDef> _criterionDefsForEval;

		public FormEvaluationDefEdit(EvaluationDef evalDefCur) {
			InitializeComponent();
			Lan.F(this);
			_evalDefCur=evalDefCur;
		}

		private void FormEvaluationDefEdit_Load(object sender,EventArgs e) {
			listCriterion.Height=412;
			_criterionDefsForEval=EvaluationCriterionDefs.Refresh(_evalDefCur.EvaluationDefNum);
			FillGrids();
		}

		private void FillGrids() {
			_criterionDefsAvailable=EvaluationCriterionDefs.Refresh();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//TODO: Discuss and brainstorm the correct columns for both of these grids. For now just using placeholders to test.
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationDefEdit","Description"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormDisplayFields","Grading Scale"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_criterionDefsForEval.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_criterionDefsForEval[i].EvaluationDefNum.ToString());
				row.Cells.Add(_criterionDefsForEval[i].GradingScaleNum.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			//Remove things from AvailList that are in the ListShowing.
			for(int i=0;i<_criterionDefsForEval.Count;i++) {
				for(int j=0;j<_criterionDefsAvailable.Count;j++) {
					//Only removing one item from _criterionDefsAvailable per iteration of i, so RemoveAt() is safe without going backwards.
					if(_criterionDefsForEval[i].EvaluationCriterionDefNum==_criterionDefsAvailable[j].EvaluationCriterionDefNum) {
						_criterionDefsAvailable.RemoveAt(j);
						break;
					}
				}
			}
			listCriterion.Items.Clear();
			for(int i=0;i<_criterionDefsAvailable.Count;i++) {
				listCriterion.Items.Add(_criterionDefsAvailable[i]);
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {//TODO: Have this go to FormEvaluationCriterionDefEdit
			//FormDisplayFieldEdit formD=new FormDisplayFieldEdit();
			//formD.FieldCur=ListShowing[e.Row];
			//DisplayField tempField=ListShowing[e.Row].Copy();
			//formD.ShowDialog();
			//if(formD.DialogResult!=DialogResult.OK) {
			//	ListShowing[e.Row]=tempField.Copy();
			//	return;
			//}
			//if(category==DisplayFieldCategory.OrthoChart) {
			//	if(ListShowing[e.Row].Description=="") {
			//		ListShowing[e.Row]=tempField.Copy();
			//		MsgBox.Show(this,"Description cannot be blank.");
			//		return;
			//	}
			//	for(int i=0;i<ListShowing.Count;i++) {//Check against ListShowing only
			//		if(i==e.Row) {
			//			continue;
			//		}
			//		if(ListShowing[e.Row].Description==ListShowing[i].Description) {
			//			ListShowing[e.Row]=tempField;
			//			MsgBox.Show(this,"That field name already exists.");
			//			return;
			//		}
			//	}
			//	for(int i=0;i<AvailList.Count;i++) {//check against AvailList only
			//		if(ListShowing[e.Row].Description==AvailList[i].Description) {
			//			ListShowing[e.Row]=tempField;
			//			MsgBox.Show(this,"That field name already exists.");
			//			return;
			//		}
			//	}
			//}
			//FillGrids();
			//changed=true;
		}

		private void butGradingScale_Click(object sender,EventArgs e) {
			FormGradingScales FormGS=new FormGradingScales();
			FormGS.ShowDialog();
			textGradeScaleName.Text=FormGS.SelectedGradingScale.Description;
			_evalDefCur.GradingScaleNum=FormGS.SelectedGradingScale.GradingScaleNum;
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(listCriterion.SelectedItems.Count==0) {
				MsgBox.Show(this,"Please select an item in the list on the right first.");
				return;
			}
			EvaluationCriterionDef field;
			for(int i=0;i<listCriterion.SelectedItems.Count;i++) {
				field=(EvaluationCriterionDef)listCriterion.SelectedItems[i];
				_criterionDefsForEval.Add(field);
			}
			FillGrids();
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid on the left first.");
				return;
			}
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {//go backwards
				_criterionDefsForEval.RemoveAt(gridMain.SelectedIndices[i]);
			}
			FillGrids();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[0]==0) {
				return;
			}
			for(int i=0;i<selected.Length;i++) {
				_criterionDefsForEval.Reverse(selected[i]-1,2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]-1,true);
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[selected.Length-1]==_criterionDefsForEval.Count-1) {
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				_criterionDefsForEval.Reverse(selected[i],2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			EvaluationDefs.SaveListForDef(_criterionDefsForEval,_evalDefCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}