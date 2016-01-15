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

		public FormEtrans834Import() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans834Import_Load(object sender,EventArgs e) {
			textImportPath.Text=PrefC.GetString(PrefName.Ins834ImportPath);
			checkIsPatientCreate.Checked=PrefC.GetBool(PrefName.Ins834IsPatientCreate);
			FillGridInsPlans();
		}

		private void butImportPathPick_Click(object sender,EventArgs e) {
			if(folderBrowserImportPath.ShowDialog()==DialogResult.OK) {
				textImportPath.Text=folderBrowserImportPath.SelectedPath;
				FillGridInsPlans();
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridInsPlans();
		}

		private void FillGridInsPlans() {
			gridInsPlans.BeginUpdate();
			if(gridInsPlans.Columns.Count==0) {
				gridInsPlans.Columns.Add(new UI.ODGridColumn("FileName",100));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("CarrierName",150));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("PatientName",150));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Errors",0));
			}
			gridInsPlans.Rows.Clear();
			if(!Directory.Exists(textImportPath.Text)) {
				gridInsPlans.EndUpdate();
				return;
			}
			string[] arrayImportFilePaths=Directory.GetFiles(textImportPath.Text);
			for(int i=0;i<arrayImportFilePaths.Length;i++) {
				string filePath=arrayImportFilePaths[i];
				string messageText=File.ReadAllText(filePath);
				if(!X12object.IsX12(messageText)) {
					continue;
				}
				X12object xobj=new X12object(messageText);
				if(!X834.Is834(xobj)) {
					continue;
				}
				try {
					X834 x834=new X834(messageText);
				}
				catch(ApplicationException aex) {
					MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste("Failed to load file '"+filePath+"'. "+aex.Message);
					msgBox.ShowDialog();
				}

			}
			gridInsPlans.EndUpdate();
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