using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralConnectionGroupEdit:Form {
		///<summary>Global list of all connections.  Never changes, used for filling filtered connection list and limiting database calls.</summary>
		private List<CentralConnection> _listConns;
		private List<CentralConnection> _listConnsCur;
		private List<ConnGroupAttach> _listConnGroupAttaches;
		///<summary>ConnectionGroupCur must be passed in from outside.</summary>
		public ConnectionGroup ConnectionGroupCur;
		public bool IsNew;

		public FormCentralConnectionGroupEdit() {
			InitializeComponent();
		}

		private void FormCentralConnectionGroupEdit_Load(object sender,EventArgs e) {
			_listConns=CentralConnections.Refresh("");
			_listConnsCur=new List<CentralConnection>();
			if(IsNew) {
				_listConnGroupAttaches=new List<ConnGroupAttach>();
			}
			else {//Take full list and filter out
				_listConnGroupAttaches=ConnGroupAttaches.GetForGroup(ConnectionGroupCur.ConnectionGroupNum);
				for(int i=0;i<_listConns.Count;i++) {
					for(int j=0;j<_listConnGroupAttaches.Count;j++) {
						if(_listConnGroupAttaches[j].CentralConnectionNum==_listConns[i].CentralConnectionNum) {//Match found
							_listConnsCur.Add(_listConns[i]);
							break;
						}
					}
				}
			}
			textDescription.Text=ConnectionGroupCur.Description;
			FillGrid();
		}

		private void FillGrid() {//Only shows connections in the grid of the currently selected connection group.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Database",320);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Note",300);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listConnsCur.Count;i++) {
				row=new ODGridRow();
				if(_listConnsCur[i].DatabaseName=="") {//uri
					row.Cells.Add(_listConnsCur[i].ServiceURI);
				}
				else {
					row.Cells.Add(_listConnsCur[i].ServerName+", "+_listConnsCur[i].DatabaseName);
				}
				row.Cells.Add(_listConnsCur[i].Note);
				row.Tag=_listConnsCur[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormCentralConnections FormCC=new FormCentralConnections();
			FormCC.ListConns=_listConnsCur;
			FormCC.LabelText.Text=Lans.g(this,"Select connections then click OK to add them to the currently edited group.");
			FormCC.Text=Lans.g(this,"Group Connections");
			FormCC.ShowDialog();
			FillGrid();
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show(Lans.g(this,"Please select a connection first to remove it."));
				return;
			}
			//Remove highlighted connections
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {//SelectedIndices is a sorted list, so we go backwards to chop off the highest to lowest values
				_listConnsCur.RemoveAt(gridMain.SelectedIndices[i]);
			}
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Sync groupattaches for this group prior to leaving this window.  Updating the ConnectionGroup list in parent window is taken care of there.
			if(textDescription.Text=="") {
				MessageBox.Show(Lans.g(this,"Please enter something for the description."));
				return;
			}
			ConnectionGroupCur.Description=textDescription.Text;
			//Does each connection have a group attach?  If not, create one.
			for(int i=0;i<_listConnsCur.Count;i++) {
				bool isFound=false;
				for(int j=0;j<_listConnGroupAttaches.Count;j++) {
					if(_listConnsCur[i].CentralConnectionNum==_listConnGroupAttaches[j].CentralConnectionNum) {
						isFound=true;
						break;
					}
				}
				if(!isFound) {
					ConnGroupAttach connGA=new ConnGroupAttach();
					connGA.CentralConnectionNum=_listConnsCur[i].CentralConnectionNum;
					connGA.ConnectionGroupNum=ConnectionGroupCur.ConnectionGroupNum;
					_listConnGroupAttaches.Add(connGA);
				}
			}
			//Does each group attach have a connection?  If not, remove the attach.
			for(int i=_listConnGroupAttaches.Count-1;i>=0;i--) {
				bool isFound=false;
				for(int j=0;j<_listConnsCur.Count;j++) {
					if(_listConnGroupAttaches[i].CentralConnectionNum==_listConnsCur[j].CentralConnectionNum) {
						isFound=true;
						break;
					}
				}
				if(!isFound) {
					_listConnGroupAttaches.RemoveAt(i);
				}
			}
			//_listConnGroupAttaches now directly reflects what is shown in the UI, without creating duplicates.
			ConnGroupAttaches.Sync(_listConnGroupAttaches,ConnectionGroupCur.ConnectionGroupNum);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;//Do nothing, parent form forgets all changes.
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {//Do nothing
				DialogResult=DialogResult.Cancel;
			}
			//Deletion is permanent.  Remove all groupattaches for this group then Set ConnectionGroupCur to null so parent form knows to remove it.
			if(MessageBox.Show(this,Lans.g(this,"Delete this entire connection group?"),"",MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			for(int i=0;i<_listConnGroupAttaches.Count;i++) {
				ConnGroupAttaches.Delete(_listConnGroupAttaches[i].ConnGroupAttachNum);
			}
			ConnectionGroupCur=null;
			DialogResult=DialogResult.OK;
		}

	}
}
