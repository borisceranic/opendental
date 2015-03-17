using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralConnectionGroups:Form {
		private List<ConnectionGroup> _listCentralConnGroups;

		public FormCentralConnectionGroups() {
			InitializeComponent();
		}

		private void FormCentralConnectionGroups_Load(object sender,EventArgs e) {
			_listCentralConnGroups=ConnectionGroups.GetListt();
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lans.g(this,"Group Name"),280);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Conns"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listCentralConnGroups.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listCentralConnGroups[i].Description);
				row.Cells.Add(ConnGroupAttaches.GetCountByGroup(_listCentralConnGroups[i].ConnectionGroupNum).ToString());
				row.Tag=_listCentralConnGroups[i];//Not really used currently, but we may want to add filtering later.
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			ConnectionGroup connGroup=new ConnectionGroup();
			connGroup.Description="";
			connGroup.ConnectionGroupNum=ConnectionGroups.Insert(connGroup);
			FormCentralConnectionGroupEdit FormCCGE=new FormCentralConnectionGroupEdit();
			FormCCGE.ConnectionGroupCur=connGroup;
			FormCCGE.IsNew=true;
			FormCCGE.ShowDialog();
			if(FormCCGE.DialogResult==DialogResult.OK) {
				if(FormCCGE.ConnectionGroupCur==null) {
					ConnectionGroups.Delete(connGroup.ConnectionGroupNum);
				}
				else {
					_listCentralConnGroups.Add(connGroup);
				}
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			ConnectionGroup connGroup=_listCentralConnGroups[gridMain.SelectedIndices[0]].Copy();//Making copy so if they press cancel any changes won't persist.
			FormCentralConnectionGroupEdit FormCCGE=new FormCentralConnectionGroupEdit();
			FormCCGE.ConnectionGroupCur=connGroup;
			FormCCGE.ShowDialog();
			if(FormCCGE.DialogResult==DialogResult.OK) {
				if(FormCCGE.ConnectionGroupCur==null) {//Group was deleted in child window, remove it from list. (Deletion is also DialogResult.OK)
					_listCentralConnGroups.RemoveAt(gridMain.SelectedIndices[0]);
				}
				else{//Child window potentially updated the connection group, replace old version in list with current version.
					_listCentralConnGroups[gridMain.SelectedIndices[0]]=FormCCGE.ConnectionGroupCur;
				}
			}
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void FormCentralConnectionGroups_FormClosing(object sender,FormClosingEventArgs e) {
			ConnectionGroups.Sync(_listCentralConnGroups);//Reflect all changes in the database.
		}

	}
}
