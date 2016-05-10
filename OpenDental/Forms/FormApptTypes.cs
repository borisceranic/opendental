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
	public partial class FormApptTypes:ODForm {
		private List<AppointmentType> _listApptTypes;
		///<summary>Stale deep copy of _listApptTypes to use with sync.</summary>
		private List<AppointmentType> _listApptTypesOld;
		private bool _isChanged=false;

		public FormApptTypes() {
			InitializeComponent();
			Lan.F(this);
			_listApptTypes=new List<AppointmentType>();
		}

		private void FormApptTypes_Load(object sender,EventArgs e) {
			_listApptTypes=AppointmentTypes.GetListt();
			_listApptTypesOld=_listApptTypes.Select(x => x.Clone()).ToList();
			FillMain();
		}

		private void FillMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptTypes","Name"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Color"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Hidden"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listApptTypes.Sort(AppointmentTypes.SortItemOrder);
			for(int i=0;i<_listApptTypes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listApptTypes[i].AppointmentTypeName);
				//TODO: more elegantly display color. possibly by row (already supported), or color the cell (enhancement).
				//The text color is always black in the grid, but text is also always black on every appointment displayed in the appointment module.
				//If the user chooses a color that does makes the black text hard to read, then we want them to see that in this window immediately.
				row.Cells.Add(_listApptTypes[i].AppointmentTypeColor.Name);
				row.ColorBackG=_listApptTypes[i].AppointmentTypeColor;
				row.Cells.Add(_listApptTypes[i].IsHidden?"X":"");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			if(gridMain.GetSelectedIndex()==0) {
				//Do nothing, the item is at the top of the list.
				return;
			}
			int index=gridMain.GetSelectedIndex();
			_isChanged=true;
			_listApptTypes[index-1].ItemOrder+=1;
			_listApptTypes[index].ItemOrder-=1;
			FillMain();
			index-=1;
			gridMain.SetSelected(index,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			if(gridMain.GetSelectedIndex()==_listApptTypes.Count-1) {
				//Do nothing, the item is at the bottom of the list.
				return;
			}
			int index=gridMain.GetSelectedIndex();
			_isChanged=true;
			_listApptTypes[index+1].ItemOrder-=1;
			_listApptTypes[index].ItemOrder+=1;
			FillMain();
			index+=1;
			gridMain.SetSelected(index,true);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormApptTypeEdit FormATE=new FormApptTypeEdit();
			FormATE.AppointmentTypeCur=_listApptTypes[e.Row];
			FormATE.ShowDialog();
			if(FormATE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormATE.AppointmentTypeCur==null) {
				_listApptTypes.RemoveAt(e.Row);
			}
			else {
				_listApptTypes[e.Row]=FormATE.AppointmentTypeCur;
			}
			_isChanged=true;
			FillMain();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormApptTypeEdit FormATE=new FormApptTypeEdit();
			FormATE.AppointmentTypeCur=new AppointmentType();
			FormATE.AppointmentTypeCur.ItemOrder=_listApptTypes.Count-1;
			FormATE.AppointmentTypeCur.IsNew=true;
			FormATE.ShowDialog();
			if(FormATE.DialogResult!=DialogResult.OK) {
				return;
			}
			_listApptTypes.Add(FormATE.AppointmentTypeCur);
			_isChanged=true;
			FillMain();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormApptTypes_FormClosing(object sender,FormClosingEventArgs e) {
			if(_isChanged) {
				for(int i=0;i<_listApptTypes.Count;i++) {
					_listApptTypes[i].ItemOrder=i;
				}
				AppointmentTypes.Sync(_listApptTypes,_listApptTypesOld);
				DataValid.SetInvalid(InvalidType.AppointmentTypes);
			}
			DialogResult=DialogResult.OK;
		}

	}
}