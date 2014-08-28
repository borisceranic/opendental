using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormDispensary:Form {

		public FormDispensary() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormDispensary_Load(object sender,EventArgs e) {
			comboClass.Items.Add(Lan.g(this,"All"));
			comboClass.SelectedIndex=0;
			for(int i=0;i<SchoolClasses.List.Length;i++) {
				comboClass.Items.Add(SchoolClasses.GetDescript(SchoolClasses.List[i]));
			}
			FillStudents();
		}

		private void FillStudents() {
			long selectedProvNum=0;
			long schoolClass=0;
			if(comboClass.SelectedIndex>0) {
				schoolClass=SchoolClasses.List[comboClass.SelectedIndex-1].SchoolClassNum;
			}
			DataTable table=Providers.RefreshForDentalSchool(schoolClass,"","",textProvNum.Text,false,false);
			gridStudents.BeginUpdate();
			gridStudents.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableProviderSetup","ProvNum"),60);
			gridStudents.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","Last Name"),90);
			gridStudents.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","First Name"),90);
			gridStudents.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","Class"),100);
			gridStudents.Columns.Add(col);
			gridStudents.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(table.Rows[i]["ProvNum"].ToString());
				}
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				if(table.Rows[i]["GradYear"].ToString()!="") {
					row.Cells.Add(table.Rows[i]["GradYear"].ToString()+"-"+table.Rows[i]["Descript"].ToString());
				}
				else {
					row.Cells.Add("");
				}

				gridStudents.Rows.Add(row);
			}
			gridStudents.EndUpdate();
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["ProvNum"].ToString()==selectedProvNum.ToString()) {
					gridStudents.SetSelected(i,true);
					break;
				}
			}
		}

		private void menuItemClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}