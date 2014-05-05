using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormGradingScaleItemEdit:Form {
		private GradingScaleItem _gradingScaleItemCur;

		public FormGradingScaleItemEdit(GradingScaleItem gradingScaleItemCur) {
			InitializeComponent();
			Lan.F(this);
			_gradingScaleItemCur=gradingScaleItemCur;
		}

		private void FormGradingScaleItemEdit_Load(object sender,EventArgs e) {
			textGradeShowing.Text=_gradingScaleItemCur.GradeShowing;
			textGradeNumber.Text=_gradingScaleItemCur.GradeNumber.ToString();
			textDescription.Text=_gradingScaleItemCur.Description;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_gradingScaleItemCur.IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			GradingScaleItems.Delete(_gradingScaleItemCur.GradingScaleItemNum);
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textGradeShowing.Text=="") {
				MsgBox.Show(this,"Grade Showing is a required field and cannot be empty.");
				return;
			}
			if(textGradeNumber.Text=="") {
				MsgBox.Show(this,"Grade Number is a required field and cannot be empty.");
				return;
			}
			_gradingScaleItemCur.GradeShowing=textGradeShowing.Text;
			_gradingScaleItemCur.GradeNumber=PIn.Float(textGradeNumber.Text);
			_gradingScaleItemCur.Description=textDescription.Text;
			if(_gradingScaleItemCur.IsNew) {
				GradingScaleItems.Insert(_gradingScaleItemCur);
			}
			else {
				GradingScaleItems.Update(_gradingScaleItemCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}