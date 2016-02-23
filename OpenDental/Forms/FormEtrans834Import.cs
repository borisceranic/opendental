using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans834Import:Form {

		private ODThread _odThread=null;
		private delegate void Update834Delegate();
		private string[] _arrayImportFilePaths;
		private int _colDateIndex;
		private int _colPatCountIndex;
		private int _colPlanCountIndex;
		private int _colErrorIndex;

		public FormEtrans834Import() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans834Import_Load(object sender,EventArgs e) {
			textImportPath.Text=PrefC.GetString(PrefName.Ins834ImportPath);
			FillGridInsPlanFiles();
		}

		private void butImportPathPick_Click(object sender,EventArgs e) {
			if(folderBrowserImportPath.ShowDialog()==DialogResult.OK) {
				textImportPath.Text=folderBrowserImportPath.SelectedPath;
				FillGridInsPlanFiles();
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
			gridInsPlanFiles.BeginUpdate();
			if(gridInsPlanFiles.Columns.Count==0) {
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("FileName",300,UI.GridSortingStrategy.StringCompare));
				_colDateIndex=gridInsPlanFiles.Columns.Count;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("Date",80,UI.GridSortingStrategy.StringCompare));
				_colPatCountIndex=gridInsPlanFiles.Columns.Count;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PatCount",80,UI.GridSortingStrategy.AmountParse));
				_colPlanCountIndex=gridInsPlanFiles.Columns.Count;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("PlanCount",80,UI.GridSortingStrategy.AmountParse));
				_colErrorIndex=gridInsPlanFiles.Columns.Count;
				gridInsPlanFiles.Columns.Add(new UI.ODGridColumn("Errors",0,UI.GridSortingStrategy.StringCompare));
			}			
			gridInsPlanFiles.Rows.Clear();
			gridInsPlanFiles.EndUpdate();
			if(!Directory.Exists(textImportPath.Text)) {
				return;
			}
			gridInsPlanFiles.BeginUpdate();
			_arrayImportFilePaths=Directory.GetFiles(textImportPath.Text);
			for(int i=0;i<_arrayImportFilePaths.Length;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				gridInsPlanFiles.Rows.Add(row);
				string filePath=_arrayImportFilePaths[i];
				row.Tag=filePath;				
				string fileName=Path.GetFileName(filePath);
				row.Cells.Add(fileName);//FileName
				row.Cells.Add("");//Date - This value will be filled in when WorkerParse834() runs below.
				row.Cells.Add("");//PatCount - This value will be filled in when WorkerParse834() runs below.
				row.Cells.Add("");//PlanCount - This value will be filled in when WorkerParse834() runs below.
				row.Cells.Add("Loading file...");//Errors - This value will be filled in when WorkerParse834() runs below.
			}
			gridInsPlanFiles.EndUpdate();
			Application.DoEvents();
			if(_odThread!=null) {
				_odThread.QuitSync(0);
			}
			_odThread=new ODThread(WorkerParse834);
			_odThread.Start();
		}

		private void WorkerParse834(ODThread odThread) {
			Load834_Safe();
			odThread.QuitAsync();
		}

		///<summary>Call this from external thread. Invokes to main thread to avoid cross-thread collision.</summary>
		private void Load834_Safe() {
			try {
				this.BeginInvoke(new Update834Delegate(Load834_Unsafe),new object[] { });
			}
			//most likely because form is no longer available to invoke to
			catch { }			
		}

		private void Load834_Unsafe() {
			Cursor=Cursors.WaitCursor;
			ShowStatus("Loading...");
			Application.DoEvents();
			const int previewLimitCount=40;
			for(int i=0;i<gridInsPlanFiles.Rows.Count;i++) {
				UI.ODGridRow row=gridInsPlanFiles.Rows[i];
				if(i < previewLimitCount) {
					gridInsPlanFiles.BeginUpdate();
				}
				string filePath=(string)row.Tag;
				ShowStatus(Lan.g(this,"Parsing file")+" "+Path.GetFileName(filePath));
				string messageText=File.ReadAllText(filePath);
				if(!X12object.IsX12(messageText)) {
					row.Cells[_colErrorIndex].Text="Is not in X12 format.";
					continue;
				}
				try {
					X12object xobj=new X12object(messageText);
					if(!X834.Is834(xobj)) {
						row.Cells[_colErrorIndex].Text="Is in X12 format, but is not an 834 document.";
						continue;
					}
					X834 x834=new X834(messageText);
					x834.FilePath=filePath;
					row.Tag=x834;
					row.Cells[_colDateIndex].Text=x834.DateInterchange.ToString();
					int memberCount=0;
					int planCount=0;
					for(int j=0;j<x834.ListTransactions.Count;j++) {
						Hx834_Tran tran=x834.ListTransactions[j];
						memberCount+=tran.ListMembers.Count;
						for(int k=0;k<tran.ListMembers.Count;k++) {
							planCount+=tran.ListMembers[k].ListHealthCoverage.Count;
						}
					}
					row.Cells[_colPatCountIndex].Text=memberCount.ToString();
					row.Cells[_colPlanCountIndex].Text=planCount.ToString();
					row.Cells[_colErrorIndex].Text="";
				}
				catch(ApplicationException aex) {
					row.Cells[_colErrorIndex].Text=aex.Message;
				}
				catch(Exception ex) {
					row.Cells[_colErrorIndex].Text=ex.ToString();
				}
				if(i < previewLimitCount) {
					gridInsPlanFiles.EndUpdate();//Also invalidates grid.  Update required in case there was large error text.
					Application.DoEvents();
				}
			}
			gridInsPlanFiles.BeginUpdate();
			int selectedIndex=GetNext834Index();
			if(selectedIndex >= 0) {
				gridInsPlanFiles.Rows[selectedIndex].ColorBackG=Color.LightYellow;
			}
			gridInsPlanFiles.EndUpdate();//Also invalidates grid.  Update required in case there was large error text.
			ShowStatus("");
			Cursor=Cursors.Default;
			Application.DoEvents();
		}

		private int GetNext834Index() {
			//Select the oldest file which does not have an error.
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
			if(_odThread!=null && !_odThread.HasQuit) {
				return;//Force the user to wait until the window has finished loading/refreshing, because we need to know which file to import.
			}
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
			_odThread.QuitAsync();
			DialogResult=DialogResult.Cancel;
		}

	}
}