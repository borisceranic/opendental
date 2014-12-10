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
	public partial class FormSheetFieldGrid:Form {
		public SheetDef SheetDefCur;
		public SheetFieldDef SheetFieldDefCur;
		public bool IsReadOnly;
		///<summary>Always contains all columns available for this grid type. Never remove items from this list.</summary>
		private List<SheetGridColDef> _listColumnsAvailable;
		//private SheetGridDef gridDefCur;

		public FormSheetFieldGrid() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldGrid_Load(object sender,EventArgs e) {
			if(IsReadOnly) {
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			if(SheetFieldDefCur.GridDef==null) {
				SheetFieldDefCur.GridDef=SheetGridDefs.GetOne(SheetFieldDefCur.FKey);
			}
			if(SheetFieldDefCur.GridDef==null) {
				SheetFieldDefCur.GridDef=new SheetGridDef();
			}
			if(SheetFieldDefCur.GridDef.Columns==null) {
				SheetFieldDefCur.GridDef.Columns=SheetGridDefs.GetColumnsAvailable(SheetFieldDefCur.GridDef.GridType);
			}
			textGridType.Text=SheetGridDefs.GetName(SheetFieldDefCur.GridDef);
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			UI.ODGrid odGrid=new ODGrid();
			using(Graphics g=Graphics.FromImage(new Bitmap(100,100))) {
				bool hasTitle=SheetGridDefs.gridHasDefaultTitle(SheetFieldDefCur.GridDef.GridType);
				textHeight.Text=(odGrid.HeaderHeight+(hasTitle?odGrid.TitleHeight:0)+(int)g.MeasureString("test",odGrid.Font,100,StringFormat.GenericDefault).Height+3).ToString();//SheetFieldDefCur.Height.ToString();
			}
			checkPmtOpt.Checked=SheetFieldDefCur.IsPaymentOption;
			fillComboGrowthBehavior();
			fillColumnNames();
			fillGridMain();
			setGridSpecific();
		}

		private void setGridSpecific() {
			switch(SheetFieldDefCur.GridDef.GridType) {
				case SheetGridType.StatementMain:
					listColumns.Enabled=false;//no new columns
					butLeft.Enabled=false;//cannot alter columns showing
					butRight.Enabled=false;//cannot alter columns showing
					comboGrowthBehavior.SelectedIndex=(int)GrowthBehaviorEnum.DownGlobal;//always down global.
					comboGrowthBehavior.Enabled=false;//cannot change growth behavior.
					break;
				default:
					//nothing to do here
					break;
			}
		}

		private void fillComboGrowthBehavior() {
			for(int i=0;i<Enum.GetNames(typeof(GrowthBehaviorEnum)).Length;i++) {
				comboGrowthBehavior.Items.Add(Enum.GetNames(typeof(GrowthBehaviorEnum))[i]);
				if((int)SheetFieldDefCur.GrowthBehavior==i) {
					comboGrowthBehavior.SelectedIndex=i;
				}
			}
		}

		private void fillColumnNames() {
			listColumns.Items.Clear();
			_listColumnsAvailable=SheetGridDefs.GetColumnsAvailable(SheetFieldDefCur.GridDef.GridType);
			for(int i=0;i<_listColumnsAvailable.Count;i++) {
				if(SheetFieldDefCur.GridDef.Columns.Any(c => c.ColName==_listColumnsAvailable[i].ColName)) {
					continue;//skip fields that are already in the display column list.
				}
				listColumns.Items.Add(_listColumnsAvailable[i].ColName);
			}
		}

		private void fillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Name"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Width"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Align"),60);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			SheetFieldDefCur.GridDef.Columns.Sort(SheetGridColDefs.SortItemOrder);//(SheetGridColDef x,SheetGridColDef y) => { return x.ItemOrder.CompareTo(y.ItemOrder); });//sort item orders
			SheetFieldDefCur.GridDef.Columns.ForEach(c => c.ItemOrder=SheetFieldDefCur.GridDef.Columns.IndexOf(c));//renumber item orders
			for(int i=0;i<SheetFieldDefCur.GridDef.Columns.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(SheetFieldDefCur.GridDef.Columns[i].DisplayName);
				row.Cells.Add(SheetFieldDefCur.GridDef.Columns[i].Width.ToString());
				row.Cells.Add(SheetFieldDefCur.GridDef.Columns[i].TextAlign.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			textWidth.Text=SheetFieldDefCur.GridDef.Columns.Sum(c => c.Width).ToString();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormSheetGridColDefEdit FormCDE=new FormSheetGridColDefEdit();
			FormCDE.ColDef=SheetFieldDefCur.GridDef.Columns[e.Row];
			FormCDE.ShowDialog();
			if(FormCDE.DialogResult!=DialogResult.OK) {
				return;
			}
			fillGridMain();
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(listColumns.SelectedIndex<0) {
				return;
			}
			string selectedColName=listColumns.Items[listColumns.SelectedIndex].ToString();
			//_listSelectedColumnNames.Add(selectedColName);
			SheetFieldDefCur.GridDef.Columns.Add(_listColumnsAvailable.Find(c=>c.ColName==selectedColName));
			fillColumnNames();
			fillGridMain();
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()<0) {
				return;
			}
			//_listSelectedColumnNames.Remove(gridMain.Rows[gridMain.GetSelectedIndex()].Cells[0].Text);
			SheetFieldDefCur.GridDef.Columns.RemoveAll(c => c.ColName==gridMain.Rows[gridMain.GetSelectedIndex()].Cells[0].Text);
			fillColumnNames();
			fillGridMain();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()<1) {
				return;
			}
			int idx=gridMain.GetSelectedIndex();
			SheetFieldDefCur.GridDef.Columns[idx].ItemOrder--;
			SheetFieldDefCur.GridDef.Columns[idx-1].ItemOrder++;
			fillGridMain();
			gridMain.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()>gridMain.Rows.Count-2) {
				return;
			}
			int idx=gridMain.GetSelectedIndex();
			SheetFieldDefCur.GridDef.Columns[idx].ItemOrder++;
			SheetFieldDefCur.GridDef.Columns[idx+1].ItemOrder--;
			fillGridMain();
			gridMain.SetSelected(idx+1,true);
		}

		private void butDelete_Click(object sender,EventArgs e) {
			SheetFieldDefCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//don't save to database here.
			SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
			SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
			SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
			SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
			SheetFieldDefCur.IsPaymentOption=checkPmtOpt.Checked;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}