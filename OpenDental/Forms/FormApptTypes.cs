using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormApptTypes:Form {
		private List<AppointmentType> _listApptTypes;

		public FormApptTypes() {
			InitializeComponent();
			Lan.F(this);
			_listApptTypes=new List<AppointmentType>();
		}

		private void FormApptTypes_Load(object sender,EventArgs e) {
			FillMain();
		}

		private void FillMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptTypes","Name"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Color"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Hidden"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listApptTypes=AppointmentTypes.Listt;
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

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormApptTypeEdit FormATE=new FormApptTypeEdit();
			FormATE.AppointmentTypeCur=_listApptTypes[e.Row];
			FormATE.ShowDialog();
			if(FormATE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillMain();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormApptTypeEdit FormATE=new FormApptTypeEdit();
			FormATE.AppointmentTypeCur=new AppointmentType();
			FormATE.IsNew=true;
			FormATE.ShowDialog();
			if(FormATE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillMain();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

	}
}