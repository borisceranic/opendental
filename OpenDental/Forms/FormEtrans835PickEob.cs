using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans835PickEob:Form {

		public List<string> ListEobTranIds;

		public string SelectedTranId {
			get { return ListEobTranIds[gridEobs.SelectedIndices[0]]; }
		}

		public FormEtrans835PickEob() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormEtrans835PickEob_Load(object sender,EventArgs e) {
			FillGridEobs();
		}

		private void FillGridEobs() {
			gridEobs.BeginUpdate();
			gridEobs.Columns.Clear();
			gridEobs.Columns.Add(new UI.ODGridColumn("",0));
			gridEobs.Rows.Clear();
			for(int i=0;i<ListEobTranIds.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				row.Cells.Add(ListEobTranIds[i]);
				gridEobs.Rows.Add(row);
			}
			gridEobs.EndUpdate();
		}

		private void gridEobs_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			PressOK();
		}

		private void PressOK() {
			if(gridEobs.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Select an EOB before continuing.");
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			PressOK();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}