using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEvaluationDefs:Form {

		public FormEvaluationDefs() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEvaluationDefs_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			DataTable table=EvaluationDefs.RefreshByCourse(textCourseDescript.Text);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableEvaluationSetup","Course"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluationSetup","Title"),90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["CourseDescript"].ToString());
				row.Cells.Add(table.Rows[i]["EvalTitle"].ToString());
				row.Tag=table.Rows[i]["EvaluationDefNum"].ToString();
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void textCourseDescript_TextChanged(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void timer1_Tick(object sender,EventArgs e) {
			timer1.Stop();
			FillGrid();
		}

		private void butDuplicate_Click(object sender,EventArgs e) {
			EvaluationDef evalDefNew=EvaluationDefs.GetOne(PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString())).Copy();
			evalDefNew.EvalTitle+="-copy";
			EvaluationDefs.Insert(evalDefNew);
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormEvaluationDefEdit FormEDE=new FormEvaluationDefEdit(new EvaluationDef());
			FormEDE.ShowDialog();
			if(FormEDE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}