using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans834Import:Form {

		private string[] _arrayImportFilePaths;
		private int _colErrorIndex;

		public FormEtrans834Import() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans834Import_Load(object sender,EventArgs e) {
			textImportPath.Text=PrefC.GetString(PrefName.Ins834ImportPath);
			checkIsPatientCreate.Checked=PrefC.GetBool(PrefName.Ins834IsPatientCreate);
			FillGridInsPlanFiles();
		}

		private void butImportPathPick_Click(object sender,EventArgs e) {
			if(folderBrowserImportPath.ShowDialog()==DialogResult.OK) {
				textImportPath.Text=folderBrowserImportPath.SelectedPath;
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridInsPlanFiles();
		}

		private void butImport_Click(object sender,EventArgs e) {
			//Deselect files which have errors.
			int deselectedCount=0;
			List <int> listSelectedIndicies=new List<int>(gridInsPlanFiles.SelectedIndices);//Since we will be modifying the selection array dynamically.
			for(int i=0;i<listSelectedIndicies.Count;i++) {
				int index=listSelectedIndicies[i];
				if(gridInsPlanFiles.Rows[index].Cells[_colErrorIndex].Text!="") {
					gridInsPlanFiles.SetSelected(index,false);
					deselectedCount++;
				}
			}
			if(deselectedCount > 0) {
				MessageBox.Show(Lan.g(this,"Number of files containing errors deselected")+": "+deselectedCount);
			}
			if(gridInsPlanFiles.SelectedIndices.Length==0) {
				MsgBox.Show(this,"No files selected for import.");
				return;
			}
			//Import all selected, valid files at the same time.
			List <X834> listX834s=new List<X834>();
			for(int i=0;i<gridInsPlanFiles.SelectedIndices.Length;i++) {
				int index=gridInsPlanFiles.SelectedIndices[i];
				X834 x834=(X834)gridInsPlanFiles.Rows[index].Tag;
				listX834s.Add(x834);
			}
			FormEtrans834Preview FormEP=new FormEtrans834Preview(listX834s);
			FormEP.ShowDialog();
		}

		private void FillGridInsPlanFiles() {
			int sortedByColumnIdx=gridInsPlanFiles.SortedByColumnIdx;
			bool isSortAscending=gridInsPlanFiles.SortedIsAscending;
			gridInsPlanFiles.BeginUpdate();
			if(gridInsPlanFiles.Columns.Count==0) {
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("FileName",300));
				gridInsPlanFiles.Columns[gridInsPlanFiles.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PatCount",80));
				gridInsPlanFiles.Columns[gridInsPlanFiles.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.AmountParse;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PlanCount",80));
				gridInsPlanFiles.Columns[gridInsPlanFiles.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.AmountParse;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("Errors",0));
				gridInsPlanFiles.Columns[gridInsPlanFiles.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
				_colErrorIndex=gridInsPlanFiles.Columns.Count-1;
				sortedByColumnIdx=_colErrorIndex;//Sort by error messages when first loaded.
				isSortAscending=true;//This will push files without errors to the top.
			}			
			gridInsPlanFiles.Rows.Clear();
			gridInsPlanFiles.EndUpdate();
			if(!Directory.Exists(textImportPath.Text)) {
				return;
			}
			Application.DoEvents();//To show empty grid while the window is loading.
			Cursor=Cursors.WaitCursor;
			gridInsPlanFiles.BeginUpdate();
			_arrayImportFilePaths=Directory.GetFiles(textImportPath.Text);
			for(int i=0;i<_arrayImportFilePaths.Length;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				gridInsPlanFiles.Rows.Add(row);
				string filePath=_arrayImportFilePaths[i];
				row.Cells.Add(Path.GetFileName(filePath));
				string messageText=File.ReadAllText(filePath);
				if(!X12object.IsX12(messageText)) {
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add("Is not in X12 format.");//Errors
					continue;
				}
				try {
					X12object xobj=new X12object(messageText);
					if(!X834.Is834(xobj)) {
						row.Cells.Add("");//PatCount
						row.Cells.Add("");//PlanCount
						row.Cells.Add("Is in X12 format, but is not an 834 document.");//Errors
						continue;
					}
					X834 x834=new X834(messageText);
					string errors=Validate834(x834);
					if(errors!="") {
						row.Cells.Add("");//PatCount
						row.Cells.Add("");//PlanCount
						row.Cells.Add(errors);//Errors
						continue;
					}
					row.Tag=x834;
					row.Cells.Add(x834.ListMembers.Count.ToString());//PatCount
					int planCount=0;
					for(int j=0;j<x834.ListMembers.Count;j++) {
						planCount+=x834.ListMembers[j].ListHealthCoverage.Count;
					}
					row.Cells.Add(planCount.ToString());//PlanCount
					row.Cells.Add("");//File was parsed successfully.  No errors.
				}
				catch(ApplicationException aex) {
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add(aex.Message);//Errors
				}
				catch(Exception ex) {
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add(ex.ToString());//Errors
				}
			}
			gridInsPlanFiles.SortForced(sortedByColumnIdx,isSortAscending);
			gridInsPlanFiles.EndUpdate();
			//Select all files which do not have an error.
			for(int i=0;i<gridInsPlanFiles.Rows.Count;i++) {
				if(gridInsPlanFiles.Rows[i].Cells[_colErrorIndex].Text=="") {//No error
					gridInsPlanFiles.SetSelected(i,true);
				}
			}
			Cursor=Cursors.Default;
		}

		private string Validate834(X834 x834) {
			return "";
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!Directory.Exists(textImportPath.Text)) {
				MsgBox.Show(this,"Invalid import path.");
				return;
			}
			Prefs.UpdateString(PrefName.Ins834ImportPath,textImportPath.Text);
			Prefs.UpdateBool(PrefName.Ins834IsPatientCreate,checkIsPatientCreate.Checked);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}