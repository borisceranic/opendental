using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormPaySplitSelect:ODForm {
		private List<PaySplit> _listPaySplits;
		private ODGrid gridMain;
		private long _patNum;
		public bool IsPrePay;
		public long SplitCur;
		///<summary>This list will contain all selected PaySplits when OK is pressed.</summary>
		public List<PaySplit> ListSelectedSplits;

		public FormPaySplitSelect(long patNum) {
			_patNum=patNum;
			InitializeComponent();
		}

		private void FormPaySplitSelect_Load(object sender, System.EventArgs e) {
			ListSelectedSplits=new List<PaySplit>();
			if(IsPrePay) {
				_listPaySplits=PaySplits.GetPrepayForFam(Patients.GetFamily(_patNum));
				//_listPaySplits=PaySplits.GetPrePayments(_patNum);
				for(int i=_listPaySplits.Count-1;i>=0;i--) {
					PaySplit split=_listPaySplits[i];
					//List<PaySplit> listSplitsForSplit=PaySplits.GetSplitsForPrePayment(split.SplitNum);
					List<PaySplit> listSplitsForSplit=PaySplits.GetSplitsForPrepay(_listPaySplits);
					decimal splitTotal=0;
					foreach(PaySplit paySplit in listSplitsForSplit) {
						if(paySplit.SplitNum==SplitCur) {
							continue;
						}
						splitTotal+=(decimal)paySplit.SplitAmt;
					}
					decimal leftOverAmt=(decimal)split.SplitAmt+splitTotal; //splitTotal should be negative.
					if(leftOverAmt<=0) {
						_listPaySplits.Remove(split);
					}
					else {
						split.SplitAmt=(double)leftOverAmt;//This will cause the left over amount to show up in the grid.  We don't do any saving in this form.
					}
				}
			}
			else {
				Family fam=Patients.GetFamily(_patNum);
				List<long> listPatNums=new List<long>();
				foreach(Patient pat in fam.ListPats) {
					listPatNums.Add(pat.PatNum);
				}
				_listPaySplits=PaySplits.GetForPats(listPatNums);//For whole family?  Maybe?  I'm doing it but I'm not sure if that's what we really want.
			}
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableInvoiceItems","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","PatName"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Prov"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","UnearnedType"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Amt Left"),60,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			Def[][] arrayDefs=DefC.GetArrayLong();
			foreach(PaySplit paySplit in _listPaySplits) {
				row=new ODGridRow();
				row.Cells.Add(paySplit.ProcDate.ToShortDateString());
				row.Cells.Add(Patients.GetNameLF(paySplit.PatNum));
				row.Cells.Add(Providers.GetAbbr(paySplit.ProvNum));
				row.Cells.Add(DefC.GetName(DefCat.PaySplitUnearnedType,paySplit.UnearnedType,arrayDefs));
				row.Cells.Add(paySplit.SplitAmt.ToString("F"));
				row.Tag=paySplit;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplit=(PaySplit)gridMain.Rows[e.Row].Tag;
			ListSelectedSplits.Clear();
			ListSelectedSplits.Add(paySplit);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			ListSelectedSplits.Clear();
			PaySplit paySplit=(PaySplit)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			ListSelectedSplits.Add(paySplit);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}