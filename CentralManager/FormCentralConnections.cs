using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralConnections:Form {
		///<summary>Initially blank or should be filled with connections that should be filtered out of the list of available conns.
		///When OK is clicked, this list will contain all filtered connections along with selected connections.</summary>
		public List<CentralConnection> ListConns;
		///<summary>A filtered list of connections.  Gets refilled every time FillGrid is called.</summary>
		private List<CentralConnection> _listConnsDisplay;
		private List<ConnectionGroup> _listConnectionGroups;

		public FormCentralConnections() {
			InitializeComponent();
			ListConns=new List<CentralConnection>();
		}

		private void FormCentralConnections_Load(object sender,EventArgs e) {
			_listConnectionGroups=ConnectionGroups.GetListt();
			comboConnectionGroups.Items.Add("All");
			comboConnectionGroups.SelectedIndex=0;
			for(int i=0;i<_listConnectionGroups.Count;i++) {
				comboConnectionGroups.Items.Add(_listConnectionGroups[i].Description);
			}
			FillGrid();
		}

		private void FillGrid() {
			_listConnsDisplay=CentralConnections.Refresh(textSearch.Text);
			if(comboConnectionGroups.SelectedIndex>0) {
				_listConnsDisplay=ConnectionGroups.FilterConnsByGroup(_listConnsDisplay,_listConnectionGroups[comboConnectionGroups.SelectedIndex-1]);
			}
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
			for(int i=0;i<_listConnsDisplay.Count;i++) {
				bool isFound=false;
				for(int j=0;j<ListConns.Count;j++) {
					if(_listConnsDisplay[i].CentralConnectionNum==ListConns[j].CentralConnectionNum) {
						isFound=true;
						break;
					}
				}
				if(isFound) {
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(_listConnsDisplay[i].ItemOrder.ToString());
				if(_listConnsDisplay[i].DatabaseName=="") {//uri
					row.Cells.Add(_listConnsDisplay[i].ServiceURI);
				}
				else {
					row.Cells.Add(_listConnsDisplay[i].ServerName+", "+_listConnsDisplay[i].DatabaseName);
				}
				row.Cells.Add(_listConnsDisplay[i].Note);
				row.Tag=_listConnsDisplay[i];
				gridMain.Rows.Add(row); 
			}
			gridMain.EndUpdate();
		}

		private void comboConnectionGroups_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormCentralConnectionEdit FormCCE=new FormCentralConnectionEdit();
			FormCCE.CentralConnectionCur=(CentralConnection)gridMain.Rows[e.Row].Tag;
			FormCCE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			CentralConnection conn=new CentralConnection();
			conn.IsNew=true;
			FormCentralConnectionEdit FormCCS=new FormCentralConnectionEdit();
			FormCCS.CentralConnectionCur=conn;
			FormCCS.ShowDialog();//Will insert conn on OK.
			FillGrid();//Refreshing the grid will show any new connections added.
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Add any selected connections to ListConns so that forms outside know which connections to add.
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				CentralConnection conn=(CentralConnection)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				ListConns.Add(conn);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
	}
}