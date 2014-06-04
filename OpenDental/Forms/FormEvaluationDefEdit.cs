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
		private List<EvaluationCriterionDef> _criterionDefsForEval;
		private Dictionary<long,GradingScale> _gradingScales;
		private List<long> _itemOrder;

		public FormEvaluationDefEdit(EvaluationDef evalDefCur) {
			InitializeComponent();
			Lan.F(this);
			_evalDefCur=evalDefCur;
		}

		private void FormEvaluationDefEdit_Load(object sender,EventArgs e) {
			if(!_evalDefCur.IsNew) {
				textTitle.Text=_evalDefCur.EvalTitle;
				textCourse.Text=SchoolCourses.GetDescript(_evalDefCur.SchoolCourseNum);
				textGradeScaleName.Text=GradingScales.GetOne(_evalDefCur.GradingScaleNum).Description;
			}
			_criterionDefsForEval=EvaluationCriterionDefs.GetAllForEvaluationDef(_evalDefCur.EvaluationDefNum);
			_itemOrder=new List<long>();
			for(int j=0;j<_criterionDefsForEval.Count;j++) {
				_itemOrder.Add(_criterionDefsForEval[j].EvaluationCriterionDefNum);
			}
			List<GradingScale> gradingScales=GradingScales.Refresh();
			_gradingScales=new Dictionary<long,GradingScale>();
			for(int i=0;i<gradingScales.Count;i++) {
				_gradingScales.Add(gradingScales[i].GradingScaleNum,gradingScales[i]);
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationDefEdit","Description"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationDefEdit","Grading Scale"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_criterionDefsForEval.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_criterionDefsForEval[i].CriterionDescript);
				if(_criterionDefsForEval[i].IsCategoryName) {
					row.Bold=true;
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_gradingScales[_criterionDefsForEval[i].GradingScaleNum].Description);
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormEvaluationCriterionDefEdit FormECDE=new FormEvaluationCriterionDefEdit(_criterionDefsForEval[gridMain.GetSelectedIndex()]);
			FormECDE.ShowDialog();
			if(FormECDE.DialogResult==DialogResult.OK) {
				_criterionDefsForEval=EvaluationCriterionDefs.GetAllForEvaluationDef(_evalDefCur.EvaluationDefNum);
				FillGrid();
			}
		}

		private void butCriterionAdd_Click(object sender,EventArgs e) {
			if(_evalDefCur.GradingScaleNum==0) {
				MsgBox.Show(this,"Please select a grading scale before adding criterion.");
				return;
			}
			EvaluationCriterionDef evalCritDef=new EvaluationCriterionDef();
			evalCritDef.EvaluationDefNum=_evalDefCur.EvaluationDefNum;
			evalCritDef.GradingScaleNum=_evalDefCur.GradingScaleNum;
			evalCritDef.IsNew=true;
			FormEvaluationCriterionDefEdit FormECDE=new FormEvaluationCriterionDefEdit(evalCritDef);
			FormECDE.ShowDialog();
			if(FormECDE.DialogResult==DialogResult.OK) {
				_criterionDefsForEval=EvaluationCriterionDefs.GetAllForEvaluationDef(_evalDefCur.EvaluationDefNum);
				FillGrid();
			}
		}

		/// <summary>Used after adding or deleting an EvaluationCriterionDef.  Enables item order to persist.</summary>
		private void SynchItemOrder() {
			
		}

		private void butGradingScale_Click(object sender,EventArgs e) {
			FormGradingScales FormGS=new FormGradingScales();
			FormGS.IsSelectionMode=true;
			FormGS.ShowDialog();
			if(FormGS.DialogResult==DialogResult.OK) {
				textGradeScaleName.Text=FormGS.SelectedGradingScale.Description;
				_evalDefCur.GradingScaleNum=FormGS.SelectedGradingScale.GradingScaleNum;
			}
		}

		private void butCoursePicker_Click(object sender,EventArgs e) {
			FormSchoolCourses FormSC=new FormSchoolCourses();
			FormSC.IsSelectionMode=true;
			FormSC.ShowDialog();
			if(FormSC.DialogResult==DialogResult.OK) {
				_evalDefCur.SchoolCourseNum=FormSC.CourseSelected.SchoolCourseNum;
				textCourse.Text=FormSC.CourseSelected.CourseID;
			}
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
				_itemOrder.Reverse(selected[i]-1,2);
			}
			FillGrid();
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
				_itemOrder.Reverse(selected[i],2);
			}
			FillGrid();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_evalDefCur.IsNew || MsgBox.Show(this,MsgBoxButtons.YesNo,"This will delete the evaluation def.  Continue?")) {
				EvaluationDefs.Delete(_evalDefCur.EvaluationDefNum);
				DialogResult=DialogResult.Cancel;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_evalDefCur.SchoolCourseNum==0) {
				MsgBox.Show(this,"A school course must be selected for this evaluation def before it can be saved.");
				return;
			}
			if(_evalDefCur.GradingScaleNum==0) {
				MsgBox.Show(this,"A grading scale must be selected for this evaluation def before it can be saved.");
				return;
			}
			if(!String.IsNullOrWhiteSpace(_evalDefCur.EvalTitle) 
				&& _evalDefCur.EvalTitle!=textTitle.Text 
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"Changing the EvaluationDef titles during a term could interfere with grading reports.  Continue?")) 
			{
				return;
			}
			_evalDefCur.EvalTitle=textTitle.Text;
			EvaluationDefs.Update(_evalDefCur);
			for(int i=0;i<_criterionDefsForEval.Count;i++) {
				_criterionDefsForEval[i].ItemOrder=i;
				EvaluationCriterionDefs.Update(_criterionDefsForEval[i]);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(_evalDefCur.IsNew) {
				EvaluationDefs.Delete(_evalDefCur.EvaluationDefNum);
			}
			DialogResult=DialogResult.Cancel;
		}

		


	}
}