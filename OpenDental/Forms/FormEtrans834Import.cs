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
			ShowStatus("Loading...");
			textImportPath.Text=PrefC.GetString(PrefName.Ins834ImportPath);
			FillGridInsPlanFiles();
			ShowStatus("");
		}

		private void butImportPathPick_Click(object sender,EventArgs e) {
			if(folderBrowserImportPath.ShowDialog()==DialogResult.OK) {
				textImportPath.Text=folderBrowserImportPath.SelectedPath;
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridInsPlanFiles();
		}

		///<summary>Shows current status to user in title bar.  Useful for when processing for a few seconds or more.</summary>
		private void ShowStatus(string message) {
			int index=Text.IndexOf(" - ");
			if(index >= 0) {
				Text=Text.Substring(0,index+3);//Remove old status, but keep the dash and spaces.
			}
			else {
				Text+=" - ";//Ensure there is a separating dash with spaces.
			}
			if(message.Trim()=="") {
				index=Text.IndexOf(" - ");
				if(index >= 0) {
					Text=Text.Substring(0,index);//Remove dash a separating spaces if message is empty.
				}
			}
			else {
				Text+=message;
			}
		}

		private void FillGridInsPlanFiles() {
			int sortedByColumnIdx=gridInsPlanFiles.SortedByColumnIdx;
			bool isSortAscending=gridInsPlanFiles.SortedIsAscending;
			gridInsPlanFiles.BeginUpdate();
			if(gridInsPlanFiles.Columns.Count==0) {
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("FileName",300,UI.GridSortingStrategy.StringCompare));
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("Date",80,UI.GridSortingStrategy.StringCompare));
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PatCount",80,UI.GridSortingStrategy.AmountParse));
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PlanCount",80,UI.GridSortingStrategy.AmountParse));
				_colErrorIndex=gridInsPlanFiles.Columns.Count;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("Errors",0,UI.GridSortingStrategy.StringCompare));
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
				string fileName=Path.GetFileName(filePath);
				ShowStatus(Lan.g(this,"Parsing file")+" "+fileName);
				row.Cells.Add(fileName);
				string messageText=File.ReadAllText(filePath);
				if(!X12object.IsX12(messageText)) {
					row.Cells.Add("");//Date
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add("Is not in X12 format.");//Errors
					continue;
				}
				try {
					X12object xobj=new X12object(messageText);
					if(!X834.Is834(xobj)) {
						row.Cells.Add(xobj.DateInterchange.ToString());//Date
						row.Cells.Add("");//PatCount
						row.Cells.Add("");//PlanCount
						row.Cells.Add("Is in X12 format, but is not an 834 document.");//Errors
						continue;
					}
					X834 x834=new X834(messageText);
					x834.FilePath=filePath;
					row.Tag=x834;
					row.Cells.Add(x834.DateInterchange.ToString());//Date
					int memberCount=0;
					int planCount=0;
					for(int j=0;j<x834.ListTransactions.Count;j++) {
						Hx834_Tran tran=x834.ListTransactions[j];
						memberCount+=tran.ListMembers.Count;
						for(int k=0;k<tran.ListMembers.Count;k++) {
							planCount+=tran.ListMembers[k].ListHealthCoverage.Count;
						}
					}
					row.Cells.Add(memberCount.ToString());//PatCount
					row.Cells.Add(planCount.ToString());//PlanCount
					row.Cells.Add("");//File was parsed successfully.  No errors.
				}
				catch(ApplicationException aex) {
					row.Cells.Add("");//Date
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add(aex.Message);//Errors
				}
				catch(Exception ex) {
					row.Cells.Add("");//Date
					row.Cells.Add("");//PatCount
					row.Cells.Add("");//PlanCount
					row.Cells.Add(ex.ToString());//Errors
				}
			}
			gridInsPlanFiles.SortForced(sortedByColumnIdx,isSortAscending);
			gridInsPlanFiles.EndUpdate();
			int selectedIndex=GetNext834Index();
			if(selectedIndex >= 0) {
				gridInsPlanFiles.Rows[selectedIndex].ColorBackG=Color.LightYellow;
			}
			ShowStatus("");
			Cursor=Cursors.Default;
		}

		private int GetNext834Index() {
			//Select all the oldest file which does not have an error.
			int selectedIndex=-1;
			for(int i=0;i<gridInsPlanFiles.Rows.Count;i++) {
				if(gridInsPlanFiles.Rows[i].Cells[_colErrorIndex].Text!="") {//File Error
					continue;
				}
				X834 x834=(X834)gridInsPlanFiles.Rows[i].Tag;
				if(selectedIndex==-1 || ((X834)gridInsPlanFiles.Rows[selectedIndex].Tag).DateInterchange > x834.DateInterchange) {
					selectedIndex=i;
				}
			}
			return selectedIndex;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!Directory.Exists(textImportPath.Text)) {
				MsgBox.Show(this,"Invalid import path.");
				return;
			}
			Prefs.UpdateString(PrefName.Ins834ImportPath,textImportPath.Text);
			int x834Index=GetNext834Index();
			if(x834Index==-1) {
				MsgBox.Show(this,"No files to import.");
				return;
			}
			X834 x834=(X834)gridInsPlanFiles.Rows[x834Index].Tag;
			FormEtrans834Preview FormEP=new FormEtrans834Preview(x834);
			FormEP.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}