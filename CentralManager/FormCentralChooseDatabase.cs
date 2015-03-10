using CentralManager;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralChooseDatabase:Form {
		public List<CentralConnection> ListCentralConn;
		private List<CentralConnection> _listAllCentralConns;

		public FormCentralChooseDatabase() {
			InitializeComponent();
		}

		private void FormCentralChooseDatabase_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			_listAllCentralConns=CentralConnections.Refresh(textSearch.Text);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("#",40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Database",300);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Note",260);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listAllCentralConns.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listAllCentralConns[i].ItemOrder.ToString());
				if(_listAllCentralConns[i].DatabaseName=="") {//uri
					row.Cells.Add(_listAllCentralConns[i].ServiceURI);
				}
				else {
					row.Cells.Add(_listAllCentralConns[i].ServerName+", "+_listAllCentralConns[i].DatabaseName);
				}
				row.Cells.Add(_listAllCentralConns[i].Note);
				row.Tag=_listAllCentralConns[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butOK_Click(object sender,EventArgs e) {
			ListCentralConn=new List<CentralConnection>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				CentralConnection conn=(CentralConnection)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				ListCentralConn.Add(conn);
			}
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}
	}
}