using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Text.RegularExpressions;

namespace OpenDental {
	public partial class FormWikiListEdit:Form {
		///<summary>Name of the wiki list being manipulated. This does not include the "wikilist" prefix. i.e. "networkdevices" not "wikilistnetworkdevices"</summary>
		public string WikiListCurName;
		public bool IsNew;
		private DataTable _table;
		private WikiListHist _wikiListOld;
		private bool _isEdited;

		public FormWikiListEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWikiListEdit_Load(object sender,EventArgs e) {
			if(!WikiLists.CheckExists(WikiListCurName)) {
				IsNew=true;
				WikiLists.CreateNewWikiList(WikiListCurName);
			}
			_table=WikiLists.GetByName(WikiListCurName);
			_wikiListOld=WikiListHists.GenerateFromName(WikiListCurName,Security.CurUser.UserNum);
			if(_wikiListOld==null) {
				_wikiListOld=new WikiListHist();
			}
			FillGrid();
			ActiveControl=textSearch;//start in search box.
		}

		/// <summary></summary>
		private void FillGrid() {
			List<WikiListHeaderWidth> colHeaderWidths = WikiListHeaderWidths.GetForList(WikiListCurName);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			for(int c=0;c<_table.Columns.Count;c++){
				int colWidth = 100;//100 = default value in case something is malformed in the database.
				foreach(WikiListHeaderWidth colHead in colHeaderWidths) {
					if(colHead.ColName==_table.Columns[c].ColumnName) {
						colWidth=colHead.ColWidth;
						break;
					}
				}
				col=new ODGridColumn(_table.Columns[c].ColumnName,colWidth,false);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_table.Rows.Count;i++){
				row=new ODGridRow();
				for(int c=0;c<_table.Columns.Count;c++) {
					row.Cells.Add(_table.Rows[i][c].ToString());
				}
				gridMain.Rows.Add(row);
				gridMain.Rows[i].Tag=i;
			}
			gridMain.EndUpdate();
			gridMain.Title=WikiListCurName;
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormWikiListItemEdit FormWLIE = new FormWikiListItemEdit();
			FormWLIE.WikiListCurName=WikiListCurName;
			FormWLIE.ItemNum=PIn.Long(_table.Rows[(int)gridMain.Rows[e.Row].Tag][0].ToString());
			FormWLIE.ShowDialog();
			//saving occurs from within the form.
			if(FormWLIE.DialogResult!=DialogResult.OK) {
				return;
			}
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
			
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			/*
			Table.Rows[e.Row][e.Col]=gridMain.Rows[e.Row].Cells[e.Col].Text;
			Point cellSelected=new Point(gridMain.SelectedCell.X,gridMain.SelectedCell.Y);
			FillGrid();//gridMain.SelectedCell gets cleared.
			gridMain.SetSelected(cellSelected);*/
		}

		/*No longer necessary because gridMain_CellLeave does this as text is changed.
		///<summary>This is done before generating markup, when adding or removing rows or columns, and when changing from "none" view to another view.  FillGrid can't be done until this is done.</summary>
		private void PumpGridIntoTable() {
			//table and grid will only have the same numbers of rows and columns if the view is none.
			//Otherwise, table may have more columns
			//So this is only allowed when switching from the none view to some other view.
			if(ViewShowing!=0) {
				return;
			}
			for(int i=0;i<Table.Rows.Count;i++) {
				for(int c=0;c<Table.Columns.Count;c++) {
					Table.Rows[i][c]=gridMain.Rows[i].Cells[c].Text;
				}
			}
		}*/

		private void butColumnLeft_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1){
				return;
			}
			SetIsEdited();
			Point pointNewSelectedCell=gridMain.SelectedCell;
			pointNewSelectedCell.X=Math.Max(1,pointNewSelectedCell.X-1);
			WikiLists.ShiftColumnLeft(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
			gridMain.SetSelected(pointNewSelectedCell);
		}

		private void butColumnRight_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				return;
			}
			SetIsEdited();
			Point pointNewSelectedCell=gridMain.SelectedCell;
			pointNewSelectedCell.X=Math.Min(gridMain.Columns.Count-1,pointNewSelectedCell.X+1);
			WikiLists.ShiftColumnRight(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
			gridMain.SetSelected(pointNewSelectedCell);
		}

		private void butHeaders_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			FormWikiListHeaders FormWLH = new FormWikiListHeaders();
			FormWLH.WikiListCurName=WikiListCurName;
			FormWLH.ShowDialog();
			if(FormWLH.DialogResult!=DialogResult.OK) {
				return;
			}
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
		}

		private void butColumnAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			SetIsEdited();
			WikiLists.AddColumn(WikiListCurName);
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
		}

		private void butColumnDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Select cell in column to be deleted first.");
				return;
			}
			if(!WikiLists.CheckColumnEmpty(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName)){
				MsgBox.Show(this,"Column cannot be deleted because it conatins data.");
				return;
			}
			SetIsEdited();
			WikiLists.DeleteColumn(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
		}

		private void butAddItem_Click(object sender,EventArgs e) {
			FormWikiListItemEdit FormWLIE = new FormWikiListItemEdit();
			FormWLIE.WikiListCurName=WikiListCurName;
			FormWLIE.ItemNum=WikiLists.AddItem(WikiListCurName);
			FormWLIE.ShowDialog();
			if(FormWLIE.DialogResult!=DialogResult.OK) {
				WikiLists.DeleteItem(FormWLIE.WikiListCurName,FormWLIE.ItemNum);//delete new item because dialog was not OK'ed.
				return;
			}
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(gridMain.Rows[i].Cells[0].Text==FormWLIE.ItemNum.ToString()) {
					gridMain.Rows[i].ColorBackG=Color.FromArgb(255,255,128);
					gridMain.ScrollToIndex(i);
				}
			}
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			bool isScrollSet=false;
			for(int i=0;i<gridMain.Rows.Count;i++) {
				gridMain.Rows[i].ColorBackG=Color.White;//set all rows back to white.
				if(textSearch.Text=="") {
					continue;
				}
				for(int j=0;j<gridMain.Columns.Count;j++) {
					if(gridMain.Rows[i].Cells[j].Text.ToUpper().Contains(textSearch.Text.ToUpper())) {
						gridMain.Rows[i].ColorBackG=Color.FromArgb(255,255,128);
						if(!isScrollSet) {//scroll to the first match in the list.
							gridMain.ScrollToIndex(i);
							isScrollSet=true;
						}
						break;//next row
					}
				}//end i
			}//end i
			gridMain.Invalidate();
		}

		private void butRenameList_Click(object sender,EventArgs e) {
			//Logic copied from FormWikiLists.butAdd_Click()---------------------
			InputBox inputListName = new InputBox("New List Name");
			inputListName.ShowDialog();
			if(inputListName.DialogResult!=DialogResult.OK) {
				return;
			}
			//Format input as it would be saved in the database--------------------------------------------
			inputListName.textResult.Text=inputListName.textResult.Text.ToLower().Replace(" ","");
			//Validate list name---------------------------------------------------------------------------
			if(DbHelper.isMySQLReservedWord(inputListName.textResult.Text)) {
				//Can become an issue when retrieving column header names.
				MsgBox.Show(this,"List name is a reserved word in MySQL.");
				return;
			}
			if(inputListName.textResult.Text=="") {
				MsgBox.Show(this,"List name cannot be blank.");
				return;
			}
			if(WikiLists.CheckExists(inputListName.textResult.Text)) {
				MsgBox.Show(this,"List name already exists.");
				return;
			}
			try {
				WikiLists.Rename(WikiListCurName,inputListName.textResult.Text);
				SetIsEdited();
				WikiListHists.Rename(WikiListCurName,inputListName.textResult.Text);
				WikiListCurName=inputListName.textResult.Text;
				_table=WikiLists.GetByName(WikiListCurName);
				FillGrid();
			}
			catch(Exception ex) {
				MessageBox.Show(this,ex.Message);
			}
		}

		///<summary>Set the _isEdited bool to true and saves a copy of the list. This only happens once. This prevents spamming of updates.</summary>
		private void SetIsEdited() {
			if(_isEdited || IsNew) {//Dont save a wikiListHist entry if this is a new list, or we have already saved an entry prior to a previous edit.
				return;
			}
			_wikiListOld.WikiListHistNum=WikiListHists.Insert(_wikiListOld);
			_isEdited=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.Rows.Count>0) {
				MsgBox.Show(this,"Cannot delete a non-empty list.  Remove all items first and try again.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this entire list and all references to it?")) {
				return;
			}
			SetIsEdited();
			WikiLists.DeleteList(WikiListCurName);
			//Someday, if we have links to lists, then this is where we would update all the wikipages containing those links.  Remove links to data that was contained in the table.
			DialogResult=DialogResult.OK;
		}

		private void butHistory_Click(object sender,EventArgs e) {
			FormWikiListHistory FormWLH=new FormWikiListHistory();
			FormWLH.ListNameCur=WikiListCurName;
			FormWLH.ShowDialog();
			if(!FormWLH.IsReverted) {
				return;
			}
			//Reversion has already saved a copy of the current revision.
			_wikiListOld=WikiListHists.GenerateFromName(WikiListCurName,Security.CurUser.UserNum);
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
			_isEdited=false;
			IsNew=false;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		

		
	

	

		

		

		

	

	}
}