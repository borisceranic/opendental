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
			if(!Security.IsAuthorized(Permissions.SecurityAdmin,true)) {//TODO: Add a new preference named AdminDentalEvaluations
				groupAdmin.Visible=false;
				_isInstructorMode=true;
			}
			else {
				_isAdminMode=true;
				butAdd.Visible=false;
			}
			comboCourse.Items.Add("All");
			for(int i=0;i<SchoolCourses.List.Length;i++) {
				comboCourse.Items.Add(SchoolCourses.List[i].Descript);
			}
			comboCourse.SelectedIndex=0;
			_listInstructor=Providers.GetInstructors();
			comboInstructor.Items.Add("All");
			for(int i=0;i<_listInstructor.Count;i++) {
				comboInstructor.Items.Add(_listInstructor[i].GetLongDesc());
			}
			comboInstructor.SelectedIndex=0;
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			FillGrid();
		}

		private void FillGrid() {
			long course=(comboCourse.SelectedIndex==0) ? 0:SchoolCourses.List[comboCourse.SelectedIndex-1].SchoolCourseNum;
			long instructor=(comboInstructor.SelectedIndex==0) ? 0:_listInstructor[comboInstructor.SelectedIndex-1].ProvNum;
			DataTable table=Evaluations.GetFilteredList(DateTime.Parse(textDateStart.Text),DateTime.Parse(textDateEnd.Text),textLastName.Text,textFirstName.Text,PIn.Long(textProvNum.Text),course,instructor);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableEvaluations","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Title"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Instructor"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","ProvNum"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Last Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","First Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Course"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Grade"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEvaluations","Grading Scale"),90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(DateTime.Parse(table.Rows[i]["DateEval"].ToString()).ToShortDateString());
				row.Cells.Add(table.Rows[i]["EvalTitle"].ToString());
				row.Cells.Add(Providers.GetLongDesc(PIn.Long(table.Rows[i]["InstructNum"].ToString())));
				row.Cells.Add(table.Rows[i]["StudentNum"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				row.Cells.Add(table.Rows[i]["Descript"].ToString());
				row.Cells.Add(table.Rows[i]["OverallgradeShowing"].ToString());
				row.Cells.Add(table.Rows[i]["Description"].ToString());
				row.Tag=table.Rows[i]["EvaluationNum"].ToString();//To keep the correct reference to the Evaluation even when filtering the list.
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//TODO: Set false to check for the actual Admin setting once it is enabled
			FormEvaluationEdit FormEE=new FormEvaluationEdit(Evaluations.GetOne(PIn.Long(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString())),false);
			FormEE.ShowDialog();
			FillGrid();
		}

		private void comboCourse_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
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
			FormEvaluationDefs FormED=new FormEvaluationDefs();
			FormED.IsSelectionMode=true;
			FormED.ShowDialog();
			if(FormED.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}