using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans835Print:Form {

		private X835 _x835;

		public FormEtrans835Print(X835 x835) {
			InitializeComponent();
			Lan.F(this);
			_x835=x835;
		}

		private void FormEtrans835Print_Load(object sender,EventArgs e) {
			FillGridMain();
		}

		private void FormEtrans835Print_Resize(object sender,EventArgs e) {
			ResizeGridMain();
		}

		private void ResizeGridMain() {
			int colWidthDate=80;
			int colWidthCodeBilled=80;
			int colWidthBilled=80;
			int colWidthDeduct=80;
			int colWidthInsPay=80;
			int colWidthVariable=gridMain.Width-10-colWidthDate-colWidthCodeBilled-colWidthBilled-colWidthDeduct-colWidthInsPay;
			if(gridMain.Columns.Count==0) {
				gridMain.Columns.Add(new UI.ODGridColumn("Patient",colWidthVariable,HorizontalAlignment.Left));
				gridMain.Columns.Add(new UI.ODGridColumn("Date",colWidthDate,HorizontalAlignment.Center));
				gridMain.Columns.Add(new UI.ODGridColumn("CodeBilled",colWidthCodeBilled,HorizontalAlignment.Center));
				gridMain.Columns.Add(new UI.ODGridColumn("Billed",colWidthBilled,HorizontalAlignment.Right));
				gridMain.Columns.Add(new UI.ODGridColumn("Deduct",colWidthDeduct,HorizontalAlignment.Right));
				gridMain.Columns.Add(new UI.ODGridColumn("InsPay",colWidthInsPay,HorizontalAlignment.Right));
			}
			else {
				gridMain.Columns[0].ColWidth=colWidthVariable;
			}
		}

		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ResizeGridMain();
			gridMain.Rows.Clear();
			for(int i=0;i<_x835.ListClaimsPaid.Count;i++) {
				Hx835_Claim claimPaid=_x835.ListClaimsPaid[i];
				for(int j=0;j<claimPaid.ListProcs.Count;j++) {
					Hx835_Proc proc=claimPaid.ListProcs[j];
					UI.ODGridRow row=new UI.ODGridRow();
					row.Cells.Add(new UI.ODGridCell(claimPaid.PatientName));//Patient
					string strDateProc=proc.DateServiceStart.ToShortDateString();
					if(proc.DateServiceEnd.Year>1880) {
						strDateProc+=" to "+proc.DateServiceEnd.ToShortDateString();
					}
					row.Cells.Add(new UI.ODGridCell(strDateProc));//Date
					row.Cells.Add(new UI.ODGridCell(proc.ProcCodeBilled));//CodeBilled
					row.Cells.Add(new UI.ODGridCell(proc.ProcFee.ToString("f2")));//Billed
					row.Cells.Add(new UI.ODGridCell("TODO"));//Deduct
					row.Cells.Add(new UI.ODGridCell(proc.InsPaid.ToString("f2")));//InsPay
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
		}

		private void butPrint_Click(object sender,EventArgs e) {

		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}