using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEvaluations:Form {
		private Provider _userProv;
		private List<Provider> _listInstructor;
		private bool _isInstructorMode=false;
		private bool _isAdminMode=false;

		public FormEvaluations() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEvaluations_Load(object sender,EventArgs e) {
			_userProv=Providers.GetProv(Security.CurUser.ProvNum);
			//_UserProv will only be allowed to be null if the user is an admin. Checking for null in this block is not necessary.
			if(!Security.IsAuthorized(Permissions.SecurityAdmin,true)) {//TODO: Change this to whatever Jordan thinks is best
				groupAdmin.Visible=false;
				_isInstructorMode=true;
			}
			else {
				_isAdminMode=true;
			}
			comboCourse.Items.Add("All");
			for(int i=0;i<SchoolCourses.List.Length;i++) {
				comboCourse.Items.Add(SchoolCourses.List[i].Descript);
			}
			_listInstructor=Providers.GetInstructors();
			comboInstructor.Items.Add("All");
			for(int i=0;i<SchoolCourses.List.Length;i++) {
				comboInstructor.Items.Add(_listInstructor[i].GetLongDesc());
			}
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.AddMonths(-6).ToShortDateString();
			FillGrid();
		}

		private void FillGrid() {
			long course=(comboCourse.SelectedIndex==0) ? 0:SchoolCourses.List[comboCourse.SelectedIndex-1].SchoolCourseNum;
			long instructor=(comboInstructor.SelectedIndex==0) ? 0:_listInstructor[comboInstructor.SelectedIndex-1].ProvNum;
			DataTable table=Evaluations.GetFilteredList(DateTime.Parse(textDateStart.Text),DateTime.Parse(textDateEnd.Text),textLastName.Text,textFirstName.Text,PIn.Long(textUniqueID.Text),course,comboInstructor.SelectedIndex);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableEvaluations","Date"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Title"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Instructor"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","UID"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Last Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","First Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Course"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Grading Scale"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Grade"),90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["DateEval"].ToString());
				row.Cells.Add(table.Rows[i]["EvalTitle"].ToString());
				row.Cells.Add(table.Rows[i]["InstructorNum"].ToString());
				row.Cells.Add(table.Rows[i]["StudentNum"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				row.Cells.Add(table.Rows[i]["Descript"].ToString());
				row.Cells.Add(table.Rows[i]["Description"].ToString());
				row.Cells.Add(table.Rows[i]["OverallgradeShowing"].ToString());
				row.Tag=table.Rows[i]["EvaluationNum"].ToString();
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateStart.errorProvider1.GetError(textDateStart)!="" || textDateEnd.errorProvider1.GetError(textDateEnd)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDateStart.Text=="" || textDateEnd.Text=="") {
				MsgBox.Show(this,"Please enter a date.");
				return;
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {

		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}