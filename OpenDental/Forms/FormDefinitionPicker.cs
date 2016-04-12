using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormDefinitionPicker:ODForm {
		///<summary>The passed-in list of Defs.</summary>
		private List<Def> _listDefInitial=new List<Def>();
		///<summary>The selected defs at any given point. Initially, this is the same as _listDefInitial.</summary>
		public List<Def> ListSelectedDefs=new List<Def>();
		///<summary>List of all defs of the passed-in category type.</summary>
		private List<Def> _listDefs;

		///<summary>Passing in a list of Defs will make those defs pre-selected and highlighted when this window loads.</summary>
		public FormDefinitionPicker(DefCat cat,List<Def> listDefs = null) {
			InitializeComponent();
			Lan.F(this);
			if(listDefs!=null) {
				ListSelectedDefs=listDefs; //initially, selected defs and list defs are the same. However, ListSelectedDefs changes while _listDefInitial doesn't.
				_listDefInitial=new List<Def>(listDefs);
			}
			_listDefs=DefC.GetListLong(cat).ToList();

		}

		private void FormDefinitionPicker_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Definition",200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("ItemValue",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Hidden",0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			foreach(Def defCur in _listDefs) {
				//even if "Show Hidden" is not checked, show hidden defs if they were passed in in the initial list.
				if(defCur.IsHidden && !checkShowHidden.Checked && !_listDefInitial.Any(x => defCur.DefNum == x.DefNum)) {
					continue;
				}
				ODGridRow row=new ODGridRow();
				row.Cells.Add(defCur.ItemName);
				row.Cells.Add(defCur.ItemValue);
				row.Cells.Add(defCur.IsHidden ? "X" : "");
				row.Tag=defCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i < gridMain.Rows.Count;i++) {
				ODGridRow rowCur=gridMain.Rows[i];
				//if the row was previously selected, it should stay selected.
				if(ListSelectedDefs.Any(x => ((Def)rowCur.Tag).DefNum == x.DefNum)) {
					gridMain.SetSelected(i,true);
				}
				//if the row is in the list of initial defs, it should be highlighted blue.
				if(_listDefInitial.Any(x => ((Def)rowCur.Tag).DefNum == x.DefNum)) {
					rowCur.ColorBackG=Color.LightBlue;
				}
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			ListSelectedDefs.Clear();
			for(int i=0;i < gridMain.SelectedIndices.Length;i++) {
				ListSelectedDefs.Add((Def)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
			}
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}