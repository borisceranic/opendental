using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormGradingScaleEdit:Form {
		List<GradingScaleItem> _listGradingScaleItems;
		private GradingScale _gradingScaleCur;

		public FormGradingScaleEdit(GradingScale gradingScaleCur) {
			InitializeComponent();
			Lan.F(this);
			_gradingScaleCur=gradingScaleCur;
		}

		private void FormGradingScaleEdit_Load(object sender,EventArgs e) {
			if(!_gradingScaleCur.IsNew) {
				textDescription.Text=_gradingScaleCur.Description;
				checkIsPercentage.Enabled=false;
				checkIsPercentage.Checked=_gradingScaleCur.IsPercentage;
				if(checkIsPercentage.Checked) {
					butAdd.Visible=false;
				}
				FillGrid();
			}
		}

		private void FillGrid() {
			_listGradingScaleItems=GradingScaleItems.Refresh(_gradingScaleCur.GradingScaleNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormGradingScaleEdit","Shown"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormGradingScaleEdit","Number"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormGradingScaleEdit","Description"),160);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listGradingScaleItems.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listGradingScaleItems[i].GradeShowing);
				row.Cells.Add(_listGradingScaleItems[i].GradeNumber.ToString());
				row.Cells.Add(_listGradingScaleItems[i].Description);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}
		private void gridMain_DoubleClick(object sender,EventArgs e) {
			FormGradingScaleItemEdit FormGSIE=new FormGradingScaleItemEdit(_listGradingScaleItems[gridMain.GetSelectedIndex()]);
			FormGSIE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			GradingScaleItem gradingScaleItemNew=new GradingScaleItem();
			gradingScaleItemNew.GradingScaleNum=_gradingScaleCur.GradingScaleNum;//Must be set prior to edit window being open if a new item.
			gradingScaleItemNew.IsNew=true;
			FormGradingScaleItemEdit FormGSIE=new FormGradingScaleItemEdit(gradingScaleItemNew);
			FormGSIE.ShowDialog();
			FillGrid();
		}

		private void checkIsPercentage_Click(object sender,EventArgs e) {
			if(checkIsPercentage.Checked) {
				MsgBox.Show(this,"Leaving this checked will delete any grading scale items created for this grading scale when it is saved.  Grading scale items are not needed for percentage grading scales.");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			_gradingScaleCur.Description=textDescription.Text;
			_gradingScaleCur.IsPercentage=checkIsPercentage.Checked;
			if(GradingScales.IsDupicateDescription(_gradingScaleCur)) {
				MsgBox.Show(this,"The selected grading scale description is already used by another grading scale.  Please input a unique description.");
				return;
			}
			if(checkIsPercentage.Checked) {
				GradingScaleItems.DeleteAllFromGradingScale(_gradingScaleCur.GradingScaleNum);
			}
			GradingScales.Update(_gradingScaleCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(_gradingScaleCur.IsNew) {
				GradingScaleItems.DeleteAllFromGradingScale(_gradingScaleCur.GradingScaleNum);
				GradingScales.Delete(_gradingScaleCur.GradingScaleNum);
			}
			DialogResult=DialogResult.Cancel;
		}



	}
}