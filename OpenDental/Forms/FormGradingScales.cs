using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormGradingScales:Form {
		private List<GradingScale> _listGradingScales;
		public GradingScale SelectedGradingScale;

		public FormGradingScales() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormGradingScales_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			_listGradingScales=GradingScales.Refresh();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormGradingScales","Description"),160);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listGradingScales.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listGradingScales[i].Description);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}
		private void gridMain_DoubleClick(object sender,EventArgs e) {
			SelectedGradingScale=_listGradingScales[gridMain.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}


		private void butEdit_Click(object sender,EventArgs e) {
			FormGradingScaleEdit FormGSE=new FormGradingScaleEdit(_listGradingScales[gridMain.GetSelectedIndex()]);
			FormGSE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			GradingScale gradingScaleNew=new GradingScale();
			gradingScaleNew.GradingScaleNum=GradingScales.Insert(gradingScaleNew);
			FormGradingScaleEdit FormGSE=new FormGradingScaleEdit(gradingScaleNew);
			FormGSE.ShowDialog();
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Select a grading scale for the evaluation.");
				return;
			}
			SelectedGradingScale=_listGradingScales[gridMain.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}