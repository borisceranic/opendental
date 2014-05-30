using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEvaluationEdit:Form {
		private Evaluation _evalCur;
		private Provider _provInstructor;
		private Provider _provStudent;
		private List<EvaluationCriterion> _evalCrits;


		public FormEvaluationEdit(Evaluation evalCur) {
			InitializeComponent();
			Lan.F(this);
			_evalCur=evalCur;
		}

		private void FormEvaluationEdit_Load(object sender,EventArgs e) {
			textDate.Text=_evalCur.DateEval.ToShortDateString();
			textTitle.Text=_evalCur.EvalTitle;
			textGradeScaleName.Text=GradingScales.GetOne(_evalCur.GradingScaleNum).Description;
			_provInstructor=Providers.GetProv(_evalCur.InstructNum);
			textInstructor.Text=_provInstructor.GetLongDesc();
			_provStudent=Providers.GetProv(_evalCur.StudentNum);
			if(_provStudent!=null) {
				textStudent.Text=_provStudent.GetLongDesc();
			}
			textCourse.Text=SchoolCourses.GetDescript(_evalCur.SchoolCourseNum);
			textGradeNumberOverride.Text=_evalCur.OverallGradeNumber.ToString();
			textGradeShowingOverride.Text=_evalCur.OverallGradeShowing;
			FillGrid();
			RecalculateGrades();
		}

		private void FillGrid() {
			_evalCrits=EvaluationCriterions.Refresh(_evalCur.EvaluationNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationEdit","Description"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationEdit","Grading Scale"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationEdit","Grade Showing"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationEdit","Grade Number"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationEdit","Note"),120);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_evalCrits.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_evalCrits[i].CriterionDescript);
				if(_evalCrits[i].IsCategoryName) {
					row.Bold=true;
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(GradingScales.GetOne(_evalCrits[i].GradingScaleNum).Description);
					row.Cells.Add(_evalCrits[i].GradeShowing);
					row.Cells.Add(_evalCrits[i].GradeNumber.ToString());
					row.Cells.Add(_evalCrits[i].Notes);
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_evalCrits[gridMain.GetSelectedIndex()].IsCategoryName) {
				return;
			}
			FormEvaluationCriterionEdit FormECE=new FormEvaluationCriterionEdit(_evalCrits[gridMain.GetSelectedIndex()]);
			FormECE.ShowDialog();
			if(FormECE.DialogResult==DialogResult.OK) {
				FillGrid();
				RecalculateGrades();
			}
		}

		private void RecalculateGrades() {
			float gradeNumber=0;
			int count=0;
			for(int i=0;i<_evalCrits.Count;i++) {
				if(_evalCrits[i].IsCategoryName) {
					continue;
				}
				if(_evalCrits[i].GradingScaleNum==_evalCur.GradingScaleNum) {
					gradeNumber+=_evalCrits[i].GradeNumber;
					count++;
				}
			}
			if(count>0) {
				gradeNumber=gradeNumber/count;
			}
			if(GradingScales.GetOne(_evalCur.GradingScaleNum).IsPercentage) {
				textGradeNumber.Text=gradeNumber.ToString();
				textGradeShowing.Text=gradeNumber.ToString();
			}
			else {
				float dif=float.MaxValue;
				float closestNumber=0;
				string closestShowing="";
				List<GradingScaleItem> listGradingScaleItem=GradingScaleItems.Refresh(_evalCur.GradingScaleNum);
				for(int i=0;i<listGradingScaleItem.Count;i++) {
					if(Math.Abs(listGradingScaleItem[i].GradeNumber-gradeNumber) < dif) {
						dif=Math.Abs(listGradingScaleItem[i].GradeNumber-gradeNumber);
						closestNumber=listGradingScaleItem[i].GradeNumber;
						closestShowing=listGradingScaleItem[i].GradeShowing;
					}
				}
				textGradeNumber.Text=closestNumber.ToString();
				textGradeShowing.Text=closestShowing;
			}
		}

		private void butStudentPicker_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			FormPP.IsStudentPicker=true;
			FormPP.ShowDialog();
			if(FormPP.DialogResult==DialogResult.OK) {
				_provStudent=Providers.GetProv(FormPP.SelectedProvNum);
				_evalCur.StudentNum=_provStudent.ProvNum;
				textStudent.Text=_provStudent.GetLongDesc();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDate.Text=="") {
				MsgBox.Show(this,"Please enter a date.");
				return;
			}
			if(_provStudent==null) {
				MsgBox.Show(this,"The evaluation must be attached to a student to save grades.");
				return;
			}
			_evalCur.DateEval=DateTime.Parse(textDate.Text);
			_evalCur.StudentNum=_provStudent.ProvNum;
			_evalCur.OverallGradeShowing=textGradeShowing.Text;
			_evalCur.OverallGradeNumber=float.Parse(textGradeNumber.Text);
			if(!String.IsNullOrWhiteSpace(textGradeShowingOverride.Text)) {
				_evalCur.OverallGradeShowing=textGradeShowingOverride.Text;
			}
			if(!String.IsNullOrWhiteSpace(textGradeNumberOverride.Text)) {
				float parsed=0;
				if(!float.TryParse(textGradeNumberOverride.Text,out parsed)) {
					MsgBox.Show(this,"The override for Overall Grade Number is not a valid number.  Please input a valid number to save the evaluation.");
					return;
				}
				_evalCur.OverallGradeNumber=parsed;
			}
			Evaluations.Update(_evalCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}