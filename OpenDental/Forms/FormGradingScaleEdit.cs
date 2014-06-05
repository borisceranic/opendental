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
		///<summary>False when grading scale is in use by an evaluation.</summary>
		private bool _isEditable=true;

		public FormGradingScaleEdit(GradingScale gradingScaleCur) {
			InitializeComponent();
			Lan.F(this);
			_gradingScaleCur=gradingScaleCur;
		}

		private void FormGradingScaleEdit_Load(object sender,EventArgs e) {
			if(_gradingScaleCur.IsNew) {
				return;
			}
			textDescription.Text=_gradingScaleCur.Description;
			for(int i=0;i<Enum.GetNames(typeof(ScaleType)).Length;i++) {
				comboScaleType.Items.Add(Enum.GetNames(typeof(ScaleType))[i]);
			}
			comboScaleType.SelectedIndex=0;
			if(GradingScales.IsInUseByEvaluation(_gradingScaleCur)) {
				labelIsPercentage.Text=Lan.g(this,"Grading scale is not editable.  It is currently in use by an evaluation.");
				labelIsPercentage.Visible=true;
				_isEditable=false;
				butAdd.Visible=false;
				butOK.Visible=false;
				butCancel.Text="Close";
				textDescription.ReadOnly=true;
			}
			FillGrid();
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
			if(!_isEditable) {
				return;
			}
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
			//if(checkIsPercentage.Checked) {
			//	labelIsPercentage.Visible=true;
			//	butAdd.Enabled=false;
			//}
			//else {
			//	labelIsPercentage.Visible=false;
			//	butAdd.Enabled=true;
			//}
		}

		private void butOK_Click(object sender,EventArgs e) {
			_gradingScaleCur.Description=textDescription.Text;
			//_gradingScaleCur.IsPercentage=checkIsPercentage.Checked;
			if(GradingScales.IsDupicateDescription(_gradingScaleCur)) {
				MsgBox.Show(this,"The selected grading scale description is already used by another grading scale.  Please input a unique description.");
				return;
			}
			//if(checkIsPercentage.Checked) {
			//	GradingScaleItems.DeleteAllFromGradingScale(_gradingScaleCur.GradingScaleNum);
			//}
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