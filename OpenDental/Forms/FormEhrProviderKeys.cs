using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrProviderKeys:Form {
		private List<EhrProvKey> _listKeys;
		private Provider _provSelected;

		public FormEhrProviderKeys() {
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>Used from FormProvEdit.  Sets the selected provider in the combo box and disables the combo box selector.</summary>
		public FormEhrProviderKeys(Provider provCur) {
			InitializeComponent();
			Lan.F(this);
			_provSelected=provCur;
		}

		private void FormEhrProviderKeys_Load(object sender,EventArgs e) {
			if(_provSelected==null) {
				for(int i=0;i<ProviderC.ListShort.Count;i++) {
					comboProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
					if(Security.CurUser.ProvNum==ProviderC.ListShort[i].ProvNum) {
						comboProv.SelectedIndex=i;
						_provSelected=Providers.GetProv(Security.CurUser.ProvNum);
					}
				}
			}
			else {
				comboProv.Items.Add(_provSelected.GetLongDesc());
				comboProv.SelectedIndex=0;
				comboProv.Enabled=false;
			}
			FillGrid();
		}

		private void comboProv_SelectionChangeCommitted(object sender,EventArgs e) {
			_provSelected=ProviderC.ListShort[comboProv.SelectedIndex];
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Year",50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Key",100);
			gridMain.Columns.Add(col);
			_listKeys=new List<EhrProvKey>();
			if(_provSelected!=null) {
				_listKeys=EhrProvKeys.GetKeysForProv(_provSelected.ProvNum);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listKeys.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listKeys[i].YearValue.ToString());
				row.Cells.Add(_listKeys[i].ProvKey);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			EhrProvKey keycur=_listKeys[e.Row];
			keycur.IsNew=false;
			FormEhrProviderKeyEdit formE=new FormEhrProviderKeyEdit(Providers.GetProv(ProviderC.ListShort[comboProv.SelectedIndex].ProvNum),keycur);
			formE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(_provSelected==null) {
				MsgBox.Show(this, "Please select a provider.");
				return;
			}
			EhrProvKey keycur=new EhrProvKey();
			keycur.IsNew=true;
			FormEhrProviderKeyEdit formE=new FormEhrProviderKeyEdit(Providers.GetProv(ProviderC.ListShort[comboProv.SelectedIndex].ProvNum),keycur);
			formE.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	

		
	}
}