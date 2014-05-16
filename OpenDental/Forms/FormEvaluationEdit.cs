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
		private bool _isAdminMode;	
		private Provider _provInstructor;
		private Provider _provStudent;
		private List<EvaluationCriterion> _evalCrits;


		public FormEvaluationEdit(Evaluation evalCur, bool isAdminMode) {
			InitializeComponent();
			Lan.F(this);
			_evalCur=evalCur;
			_isAdminMode=isAdminMode;
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
			textGradeShowing.Text=_evalCur.OverallGradeShowing;
			FillGrid();
		}

		private void FillGrid() {
			_evalCrits=EvaluationCriterions.Refresh(_evalCur.EvaluationNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//TODO: Add more columns
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationEdit","Description"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationEdit","Grading Scale"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_evalCrits.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_evalCrits[i].CriterionDescript);
				row.Cells.Add(GradingScales.GetOne(_evalCrits[i].GradingScaleNum).Description);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormEvaluationCriterionEdit FormECE=new FormEvaluationCriterionEdit(_evalCrits[gridMain.GetSelectedIndex()]);
			FormECE.ShowDialog();
			if(FormECE.DialogResult==DialogResult.OK) {
				FillGrid();
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
			Evaluations.Update(_evalCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}