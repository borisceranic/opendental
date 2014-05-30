using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEvaluationCriterionDefEdit:Form {
		EvaluationCriterionDef _evalCritDef;

		public FormEvaluationCriterionDefEdit(EvaluationCriterionDef evalCritDef) {
			InitializeComponent();
			Lan.F(this);
			_evalCritDef=evalCritDef;
		}

		private void FormEvaluationCriterionDefEdit_Load(object sender,EventArgs e) {
			textDescript.Text=_evalCritDef.CriterionDescript;
			GradingScale gradeScale=GradingScales.GetOne(_evalCritDef.GradingScaleNum);
			textGradeScaleName.Text=gradeScale.Description;
			checkIsCategoryName.Checked=_evalCritDef.IsCategoryName;
		}

		private void butGradingScale_Click(object sender,EventArgs e) {
			FormGradingScales FormGS=new FormGradingScales();
			FormGS.IsSelectionMode=true;
			FormGS.ShowDialog();
			if(FormGS.DialogResult==DialogResult.OK) {
				textGradeScaleName.Text=FormGS.SelectedGradingScale.Description;
				_evalCritDef.GradingScaleNum=FormGS.SelectedGradingScale.GradingScaleNum;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_evalCritDef.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This will delete the criterion def.  Continue?")) {
				EvaluationCriterionDefs.Delete(_evalCritDef.EvaluationCriterionDefNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDescript.Text=="") {
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			_evalCritDef.CriterionDescript=textDescript.Text;
			_evalCritDef.IsCategoryName=checkIsCategoryName.Checked;
			if(_evalCritDef.IsNew) {
				EvaluationCriterionDefs.Insert(_evalCritDef);
			}
			else {
				EvaluationCriterionDefs.Update(_evalCritDef);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}