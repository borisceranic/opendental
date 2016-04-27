using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPopupsForFam:ODForm {
		public Patient PatCur;
		private List<PopupEvent> _listPopEvents;
		private List<Popup> _listPopups;

		public FormPopupsForFam(List<PopupEvent> listPopEvents) {
			InitializeComponent();
			Lan.F(this);
			_listPopEvents=listPopEvents;
		}

		private void FormPopupsForFam_Load(object sender,EventArgs e) {
			gridMain.AllowSortingByColumn=true;
			FillGrid();
		}

		private void FillGrid() {
			if(checkDeleted.Checked) {
				_listPopups=Popups.GetDeletedForFamily(PatCur);
			}
			else {
				_listPopups=Popups.GetForFamily(PatCur);
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePopupsForFamily","Patient"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Level"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Disabled"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Last Viewed"),80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(checkDeleted.Checked) {
				col=new ODGridColumn(Lan.g("TablePopupsForFamily","Deleted"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Popup Message"),120);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listPopups.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Patients.GetPat(_listPopups[i].PatNum).GetNameLF());
				row.Cells.Add(Lan.g("enumEnumPopupLevel",_listPopups[i].PopupLevel.ToString()));
				row.Cells.Add(_listPopups[i].IsDisabled?"X":"");
				PopupEvent popEvent=_listPopEvents.FirstOrDefault(x => x.PopupNum==_listPopups[i].PopupNum);
				if(popEvent!=null && popEvent.LastViewed.Year>1880) {
					row.Cells.Add(popEvent.LastViewed.ToShortTimeString());
				}
				else {
					row.Cells.Add("");
				}
				if(checkDeleted.Checked) {
					row.Cells.Add(_listPopups[i].IsArchived?"X":"");
				}
				row.Cells.Add(_listPopups[i].Description);
				row.Tag=i;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPopupEdit FormPE=new FormPopupEdit();
			int rowIndex=(int)gridMain.Rows[e.Row].Tag;
			FormPE.PopupCur=_listPopups[rowIndex];
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}
		
		private void checkDeleted_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormPopupEdit FormPE=new FormPopupEdit();
			Popup popup=new Popup();
			popup.PatNum=PatCur.PatNum;
			popup.PopupLevel=EnumPopupLevel.Patient;
			popup.IsNew=true;
			FormPE.PopupCur=popup;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
	}
}