using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormInvoiceItemSelect:ODForm {
		private DataTable _tableSuperFamAcct;
		private ODGrid gridMain;
		private long _patNum;
		///<summary>This dictionary contains all selected items from the grid when OK is pressed.
		///The string will either be "Adj" or "Proc" and the long will be the corresponding primary key.</summary>
		public Dictionary<string,List<long>> DictSelectedItems=new Dictionary<string,List<long>>();

		public FormInvoiceItemSelect(long patNum) {
			_patNum=patNum;
			InitializeComponent();
		}

		private void FormInvoiceItemSelect_Load(object sender, System.EventArgs e) {
			_tableSuperFamAcct=Patients.GetSuperFamProcAdjusts(_patNum);
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableInvoiceItems","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","PatName"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Prov"),55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Code"),55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Tooth"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Description"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableInvoiceItems","Fee"),60,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			List<ProcedureCode> listProcCodes=ProcedureCodes.GetAllCodes();
			Def[][] arrayDefs=DefC.GetArrayLong();
			foreach(DataRow tableRow in _tableSuperFamAcct.Rows) {
				row=new ODGridRow();
				row.Cells.Add(PIn.DateT(tableRow["Date"].ToString()).ToShortDateString());
				row.Cells.Add(tableRow["PatName"].ToString());
				row.Cells.Add(Providers.GetAbbr(PIn.Long(tableRow["Prov"].ToString())));
				ProcedureCode procCode=ProcedureCodes.GetProcCode(PIn.Long(tableRow["Code"].ToString()),listProcCodes);
				if(procCode.CodeNum==0) {
					row.Cells.Add(Lan.g(this,"Adjust"));//Adjustment
				}
				else {
					row.Cells.Add(procCode.ProcCode);
				}
				row.Cells.Add(Tooth.ToInternat(tableRow["Tooth"].ToString()));
				if(procCode.CodeNum==0) {
					row.Cells.Add(DefC.GetName(DefCat.AdjTypes,PIn.Long(tableRow["AdjType"].ToString()),arrayDefs));//Adjustment type
				}
				else {
					row.Cells.Add(procCode.Descript);
				}
				row.Cells.Add(PIn.Double(tableRow["Amount"].ToString()).ToString("F"));
				row.Tag=tableRow;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DataRow row=(DataRow)gridMain.Rows[e.Row].Tag;
			string type="Proc";
			if(row["Code"].ToString()=="") {
				type="Adj";
			}
			DictSelectedItems.Clear();
			DictSelectedItems.Add(type,new List<long>() { PIn.Long(row["PriKey"].ToString()) });//Add the clicked-on entry
			DialogResult=DialogResult.OK;
		}

		private void butAll_Click(object sender,System.EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butNone_Click(object sender,System.EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				DataRow row=(DataRow)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				string type="Proc";
				if(row["Code"].ToString()=="") {
					type="Adj";
				}
				long priKey=PIn.Long(row["PriKey"].ToString());
				List<long> listPriKeys;
				if(DictSelectedItems.TryGetValue(type,out listPriKeys)) {//If an entry with Proc or Adj already exists, grab its list
					listPriKeys.Add(priKey);//Add the primary key to the list
				}
				else {//No entry with Proc or Adj
					DictSelectedItems.Add(type,new List<long>() { priKey });//Make a new dict entry
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}