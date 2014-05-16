using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEvaluationCriterionEdit:Form {
		private EvaluationCriterion _evalCritCur;
		private List<GradingScaleItem> _listGradeItems;
		private GradingScale _gradingScale;

		public FormEvaluationCriterionEdit(EvaluationCriterion evalCritCur) {
			InitializeComponent();
			Lan.F(this);
			_evalCritCur=evalCritCur;
		}

		private void FormEvaluationCriterionEdit_Load(object sender,EventArgs e) {
			//There is always going to be an EvaluationCriterion when coming into this window.
			_gradingScale=GradingScales.GetOne(_evalCritCur.GradingScaleNum);
			textCriterionDescript.Text=_evalCritCur.CriterionDescript;
			textGradingScale.Text=_gradingScale.Description;
			textNote.Text=_evalCritCur.Notes;
			_listGradeItems=GradingScaleItems.Refresh(_evalCritCur.GradingScaleNum);
			if(!_gradingScale.IsPercentage) {
				textGradeNumberPercent.Visible=false;
				textGradeShowing.ReadOnly=true;
				for(int i=0;i<_listGradeItems.Count;i++) {
					comboGradeNumber.Items.Add(_listGradeItems[i].GradeNumber);
					if(_listGradeItems[i].GradeNumber==_evalCritCur.GradeNumber) {
						comboGradeNumber.SelectedIndex=i;
						textGradeShowing.Text=_evalCritCur.GradeShowing;
					}
				}
			}
			else {
				comboGradeNumber.Visible=false;
				textGradeNumberPercent.Text=_evalCritCur.GradeNumber.ToString();
				textGradeShowing.Text=_evalCritCur.GradeShowing;
			}
		}

		private void comboGradeNumber_SelectionChangeCommitted(object sender,EventArgs e) {
			_evalCritCur.GradeNumber=_listGradeItems[comboGradeNumber.SelectedIndex].GradeNumber;
			_evalCritCur.GradeShowing=_listGradeItems[comboGradeNumber.SelectedIndex].GradeShowing;
			textGradeShowing.Text=_evalCritCur.GradeShowing;
		}

		private void butOK_Click(object sender,EventArgs e) {
			float result;
			if(_gradingScale.IsPercentage) {
				if(!float.TryParse(textGradeNumberPercent.Text,out result)) {
					MsgBox.Show(this,"Grade number must be a valid percentage. Do not include '%' in the value.");
				}
				_evalCritCur.GradeNumber=result;
				_evalCritCur.GradeShowing=textGradeShowing.Text;
			}
			_evalCritCur.Notes=textNote.Text;
			EvaluationCriterions.Update(_evalCritCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}