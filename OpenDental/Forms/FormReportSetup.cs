using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormReportSetup:ODForm {
		private bool changed;
		private List<DisplayReport> _listDisplayReportsAll;
		private Point _selectedCell=new Point(-1,-1); //X:Col, Y:Row.
		private ODGrid _selectedGrid=null;

		public FormReportSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormReportSetup_Load(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				checkReportPIClinic.Visible=false;
        checkReportPIClinicInfo.Visible=false;
			}
			checkReportsProcDate.Checked=PrefC.GetBool(PrefName.ReportsPPOwriteoffDefaultToProcDate);
			checkReportsShowPatNum.Checked=PrefC.GetBool(PrefName.ReportsShowPatNum);
			checkReportProdWO.Checked=PrefC.GetBool(PrefName.ReportPandIschedProdSubtractsWO);
      checkReportPIClinicInfo.Checked=PrefC.GetBool(PrefName.ReportPandIhasClinicInfo);
			checkReportPIClinic.Checked=PrefC.GetBool(PrefName.ReportPandIhasClinicBreakdown);
			checkReportPrintWrapColumns.Checked=PrefC.GetBool(PrefName.ReportsWrapColumns);
			_listDisplayReportsAll=DisplayReports.GetAll(true);
			//set tags so that we know what grids are associated to what categories.
			gridProdInc.Tag=DisplayReportCategory.ProdInc;
			gridDaily.Tag=DisplayReportCategory.Daily;
			gridMonthly.Tag=DisplayReportCategory.Monthly;
			gridLists.Tag=DisplayReportCategory.Lists;
			gridPublicHealth.Tag=DisplayReportCategory.PublicHealth;
			gridProdInc.SelectedRowColor=Color.FromArgb(30,55,55,55); //set a transparent colour so you see which cell is selected.
			gridDaily.SelectedRowColor=Color.FromArgb(30,55,55,55); //set a transparent colour so you see which cell is selected.
			gridMonthly.SelectedRowColor=Color.FromArgb(30,55,55,55); //set a transparent colour so you see which cell is selected.
			gridLists.SelectedRowColor=Color.FromArgb(30,55,55,55); //set a transparent colour so you see which cell is selected.
			gridPublicHealth.SelectedRowColor=Color.FromArgb(30,55,55,55); //set a transparent colour so you see which cell is selected.

			FillGrids();
		}

		///<summary>If any columns are reordered or added to this grid, they will need to be considered in the GridCell_Click event below.
		///This refreshes every grid on the form.</summary>
		private void FillGrids() {
			_listDisplayReportsAll=_listDisplayReportsAll.OrderBy(x => x.ItemOrder).ToList();
			//Begin Update
			gridProdInc.BeginUpdate();
			gridDaily.BeginUpdate();
			gridMonthly.BeginUpdate();
			gridLists.BeginUpdate();
			gridPublicHealth.BeginUpdate();
			//Add Columns
			ODGridColumn col;
			gridProdInc.Columns.Clear();
			col=new ODGridColumn("Display Name",190,true);
			gridProdInc.Columns.Add(col);
			col=new ODGridColumn("Hidden",0,HorizontalAlignment.Center);
			gridProdInc.Columns.Add(col);
			gridDaily.Columns.Clear();
			col=new ODGridColumn("Display Name",190,true);
			gridDaily.Columns.Add(col);
			col=new ODGridColumn("Hidden",0,HorizontalAlignment.Center);
			gridDaily.Columns.Add(col);
			gridMonthly.Columns.Clear();
			col=new ODGridColumn("Display Name",190,true);
			gridMonthly.Columns.Add(col);
			col=new ODGridColumn("Hidden",0,HorizontalAlignment.Center);
			gridMonthly.Columns.Add(col);
			gridLists.Columns.Clear();
			col=new ODGridColumn("Display Name",190,true);
			gridLists.Columns.Add(col);
			col=new ODGridColumn("Hidden",0,HorizontalAlignment.Center);
			gridLists.Columns.Add(col);
			gridPublicHealth.Columns.Clear();
			col=new ODGridColumn("Display Name",190,true);
			gridPublicHealth.Columns.Add(col);
			col=new ODGridColumn("Hidden",0,HorizontalAlignment.Center);
			gridPublicHealth.Columns.Add(col);
			//Add Rows
			gridProdInc.Rows.Clear();
			gridDaily.Rows.Clear();
			gridMonthly.Rows.Clear();
			gridLists.Rows.Clear();
			gridPublicHealth.Rows.Clear();
			foreach(DisplayReport reportCur in _listDisplayReportsAll) {
				ODGridRow row= new ODGridRow();
				row.Cells.Add(reportCur.Description);
				row.Cells.Add(reportCur.IsHidden ? "X" : "");
				row.Tag=reportCur.InternalName;
				switch(reportCur.Category) {
					case DisplayReportCategory.ProdInc:
						gridProdInc.Rows.Add(row);
						break;
					case DisplayReportCategory.Daily:
						gridDaily.Rows.Add(row);
						break;
					case DisplayReportCategory.Monthly:
						gridMonthly.Rows.Add(row);
						break;
					case DisplayReportCategory.Lists:
						gridLists.Rows.Add(row);
						break;
					case DisplayReportCategory.PublicHealth:
						gridPublicHealth.Rows.Add(row);
						break;
					case DisplayReportCategory.ArizonaPrimaryCare:
					default:
						break;
				}
			}
			//End Update
			gridProdInc.EndUpdate();
			gridDaily.EndUpdate();
			gridMonthly.EndUpdate();
			gridLists.EndUpdate();
			gridPublicHealth.EndUpdate();
			if(_selectedGrid != null && _selectedCell.Y != -1) {
				_selectedGrid.Rows[_selectedCell.Y].ColorBackG=Color.LightCyan;
				_selectedGrid.SetSelected(_selectedCell);
			}
		}
		
		///<summary>This method is used by all grids in this form. If any new grids are added, they will need to be added to this method.</summary>
		private void grid_CellClick(object sender,ODGridClickEventArgs e) {
			if(_selectedCell.Y != -1 && _selectedGrid != null) {
				//commit change before the new cell is selected to save the old cell's changes.
				CommitChange();
			}
			_selectedCell.X=e.Col;
			_selectedCell.Y=e.Row;
			_selectedGrid=(ODGrid)sender;
			//this label makes sure the user always has some idea of what the selected report is, even if the DisplayName might be incomprehensible.
			labelODInternal.Text=_selectedGrid.Rows[_selectedCell.Y].Tag.ToString();
			DisplayReportCategory selectedCat=(DisplayReportCategory)_selectedGrid.Tag;
			//de-select all but the currently selected grid
			if(selectedCat!=DisplayReportCategory.ProdInc) { gridProdInc.SetSelected(-1,true); }
			if(selectedCat!=DisplayReportCategory.Daily) { gridDaily.SetSelected(-1,true); }
			if(selectedCat!=DisplayReportCategory.Monthly) { gridMonthly.SetSelected(-1,true); }
			if(selectedCat!=DisplayReportCategory.Lists) { gridLists.SetSelected(-1,true); }
			if(selectedCat!=DisplayReportCategory.PublicHealth) { gridPublicHealth.SetSelected(-1,true); }
			DisplayReport clicked=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y);
			if(_selectedCell.X==1) {
				clicked.IsHidden = !clicked.IsHidden;
			}
			FillGrids();
		}

		///<summary>Commit changes that the user might have made to the display name.</summary>
		private void CommitChange() {
			DisplayReportCategory selectedCat=(DisplayReportCategory)_selectedGrid.Tag;
			DisplayReport clicked=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y);
			clicked.Description=_selectedGrid.Rows[_selectedCell.Y].Cells[0].Text;
		}
	
		private void grid_CellLeave(object sender,ODGridClickEventArgs e) {
			CommitChange();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(_selectedCell.Y == -1 || _selectedGrid == null) {
				MsgBox.Show(this,"Please select a report first.");
				return;
			}
			DisplayReportCategory selectedCat = (DisplayReportCategory)_selectedGrid.Tag;
			DisplayReport selectedReport=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y);
			if(selectedReport.ItemOrder==0) {
				return; //the item is already the first in the list and cannot go up anymore.
			}
			DisplayReport switchReport=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y-1);
			selectedReport.ItemOrder--;
			_selectedCell.Y--;
			switchReport.ItemOrder++;
			FillGrids();
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(_selectedCell.Y == -1 || _selectedGrid == null) {
				MsgBox.Show(this,"Please select a report first.");
				return;
			}
			DisplayReportCategory selectedCat = (DisplayReportCategory)_selectedGrid.Tag;
			DisplayReport selectedReport=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y);
			if(selectedReport.ItemOrder==_selectedGrid.Rows.Count-1) {
				return; //the item is already the last in the list and cannot go down anymore.
			}
			DisplayReport switchReport=_listDisplayReportsAll.Find(x => x.Category == selectedCat && x.ItemOrder == _selectedCell.Y+1);
			selectedReport.ItemOrder++;
			_selectedCell.Y++;
			switchReport.ItemOrder--;
			FillGrids();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.ReportsPPOwriteoffDefaultToProcDate,checkReportsProcDate.Checked)
				| Prefs.UpdateBool(PrefName.ReportsShowPatNum,checkReportsShowPatNum.Checked)
				| Prefs.UpdateBool(PrefName.ReportPandIschedProdSubtractsWO,checkReportProdWO.Checked)
				| Prefs.UpdateBool(PrefName.ReportPandIhasClinicInfo,checkReportPIClinicInfo.Checked)
				| Prefs.UpdateBool(PrefName.ReportPandIhasClinicBreakdown,checkReportPIClinic.Checked)
				| Prefs.UpdateBool(PrefName.ReportsWrapColumns,checkReportPrintWrapColumns.Checked)
				) {
				changed=true;
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DisplayReports.Sync(_listDisplayReportsAll);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}