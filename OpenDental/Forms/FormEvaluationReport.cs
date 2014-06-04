using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEvaluationReport:Form {
		private bool _isCourseSelected=false;
		private bool _isInstructorSelected=false;
		private List<SchoolCourse> _schoolCourses;
		private List<Provider> _provInstructors;

		public FormEvaluationReport() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEvaluationReport_Load(object sender,EventArgs e) {
			//dateStart.SelectionStart=DateTime.Today.AddMonths(-4);
			//dateStart.SelectionEnd=DateTime.Today.AddMonths(-4);
			//dateEnd.SelectionStart=DateTime.Today;
			FillCourses();
		}

		private void FillCourses() {
			_schoolCourses=new List<SchoolCourse>(SchoolCourses.List);
			gridCourses.BeginUpdate();
			gridCourses.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationReport - Courses","CourseID"),60);
			gridCourses.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationReport - Courses","Description"),90);
			gridCourses.Columns.Add(col);
			gridCourses.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_schoolCourses.Count;i++) {
				row=new ODGridRow();
				row.Tag=_schoolCourses[i].SchoolCourseNum.ToString();
				row.Cells.Add(_schoolCourses[i].CourseID);
				row.Cells.Add(_schoolCourses[i].Descript);
				gridCourses.Rows.Add(row);
			}
			gridCourses.EndUpdate();
		}

		private void FillInstructors() {
			_provInstructors=Providers.GetInstructors();
			gridInstructors.BeginUpdate();
			gridInstructors.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationReport - Instructors","ProvNum"),50);
			gridInstructors.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationReport - Instructors","Last Name"),80);
			gridInstructors.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationReport - Instructors","First Name"),80);
			gridInstructors.Columns.Add(col);
			gridInstructors.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_provInstructors.Count;i++) {
				row=new ODGridRow();
				row.Tag=(_provInstructors[i].ProvNum.ToString());
				row.Cells.Add(_provInstructors[i].ProvNum.ToString());
				row.Cells.Add(_provInstructors[i].LName);
				row.Cells.Add(_provInstructors[i].FName);
				gridInstructors.Rows.Add(row);
			}
			gridInstructors.EndUpdate();
		}

		private void FillStudents() {
			List<long> schoolCourseNums=new List<long>();
			List<long> instructorProvNums=new List<long>();
			for(int i=0;i<gridCourses.SelectedIndices.Length;i++) {
				int index=gridCourses.SelectedIndices[i];
				schoolCourseNums.Add(PIn.Long(gridCourses.Rows[index].Tag.ToString()));
			}
			for(int i=0;i<gridInstructors.SelectedIndices.Length;i++) {
				int index=gridInstructors.SelectedIndices[i];
				instructorProvNums.Add(PIn.Long(gridInstructors.Rows[index].Tag.ToString()));
			}
			DataTable table=Evaluations.GetFilteredList(schoolCourseNums,instructorProvNums);
			gridStudent.BeginUpdate();
			gridStudent.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormEvaluationReport - Students","ProvNum"),60);
			gridStudent.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationReport - Students","Last Name"),90);
			gridStudent.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationReport - Students","First Name"),90);
			gridStudent.Columns.Add(col);
			gridStudent.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["StudentNum"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				row.Tag=table.Rows[i]["StudentNum"].ToString();
				gridStudent.Rows.Add(row);
			}
			gridStudent.EndUpdate();
		}

		private void checkAllStudents_CheckedChanged(object sender,EventArgs e) {
			if(checkAllInstructors.Checked) {
				gridStudent.SetSelected(true);
				return;
			}
			gridStudent.SetSelected(false);
		}

		private void checkAllInstructors_CheckedChanged(object sender,EventArgs e) {
			if(checkAllInstructors.Checked) {
				gridInstructors.SetSelected(true);
				gridInstructors.Visible=false;
				_isInstructorSelected=true;
				return;
			}
			gridInstructors.SetSelected(false);
			gridInstructors.Visible=true;
		}

		private void checkAllCourses_CheckedChanged(object sender,EventArgs e) {
			if(checkAllCourses.Checked) {
				if(!_isCourseSelected) {
					FillInstructors();
					FillStudents();
					_isCourseSelected=true;
				}
				gridCourses.SetSelected(true);
				gridCourses.Visible=false;
				return;
			}
			gridCourses.SetSelected(false);
			gridCourses.Visible=true;
		}

		private void gridCourses_CellClick(object sender,UI.ODGridClickEventArgs e) {
			if(!_isCourseSelected) {
				FillInstructors();
				_isCourseSelected=true;
			}
			if(_isInstructorSelected) {
				FillStudents();
			}
		}

		private void gridInstructors_CellClick(object sender,UI.ODGridClickEventArgs e) {
			FillStudents();
			_isInstructorSelected=true;
		}

		private void butAverage_Click(object sender,EventArgs e) {
			//DataTable TableReport=new DataTable();
			//ReportSimpleGrid report=new ReportSimpleGrid();
			//List<long> schoolCourseNums=new List<long>();
			//List<long> instructorProvNums=new List<long>();
			//for(int i=0;i<gridCourses.SelectedIndices.Length;i++) {
			//	int index=gridCourses.SelectedIndices[i];
			//	schoolCourseNums.Add(PIn.Long(gridCourses.Rows[index].Tag.ToString()));
			//}
			//for(int i=0;i<gridInstructors.SelectedIndices.Length;i++) {
			//	int index=gridInstructors.SelectedIndices[i];
			//	instructorProvNums.Add(PIn.Long(gridInstructors.Rows[index].Tag.ToString()));
			//}
			////TODO: Change this query to work with the average once we have decided how to quantify different grading scales
			//string command="SELECT schoolcourse.CourseID,ins.ProvNum,ins.LName,ins.FName,"
			//+"stu.ProvNum,stu.LName,stu.FName,gradingscale.Description,evaluation.OverallGradeShowing,evaluation.OverallGradeNumber FROM evaluation "
			//	+"INNER JOIN provider ins ON ins.ProvNum=evaluation.InstructNum "
			//	+"INNER JOIN provider stu ON stu.ProvNum=evaluation.StudentNum "
			//	+"INNER JOIN schoolcourse ON schoolcourse.SchoolCourseNum=evaluation.SchoolCourseNum "
			//	+"INNER JOIN gradingscale ON gradingscale.GradingScaleNum=evaluation.GradingScaleNum "
			//	+"WHERE TRUE";
			//if(schoolCourseNums!=null && schoolCourseNums.Count!=0) {
			//	command+=" AND schoolcourse.SchoolCourseNum IN (";
			//	for(int i=0;i<schoolCourseNums.Count;i++) {
			//		command+="'"+POut.Long(schoolCourseNums[i])+"'";
			//		if(i!=schoolCourseNums.Count-1) {
			//			command+=",";
			//			continue;
			//		}
			//		command+=")";
			//	}
			//}
			//if(instructorProvNums!=null && instructorProvNums.Count!=0) {
			//	command+=" AND ins.ProvNum IN (";
			//	for(int i=0;i<instructorProvNums.Count;i++) {
			//		command+="'"+POut.Long(instructorProvNums[i])+"'";
			//		if(i!=instructorProvNums.Count-1) {
			//			command+=",";
			//			continue;
			//		}
			//		command+=")";
			//	}
			//}
			//command+=" AND evaluation.DateEval BETWEEN "+POut.DateT(dateStart.SelectionStart)+" AND "+POut.DateT(dateEnd.SelectionStart);
			//command+=" ORDER BY CourseID,ins.ProvNum";
			//report.Query=command;
			//FormQuery FormQuery2=new FormQuery(report);
			//FormQuery2.IsReport=true;
			//TableReport=report.GetTempTable();
			//report.TableQ=new DataTable(null);//new table with 10 columns
			//for(int i=0;i<10;i++) { //add columns
			//	report.TableQ.Columns.Add(new System.Data.DataColumn());//blank columns
			//}
			//report.InitializeColumns();
			//float gradeNumber=0;
			//for(int i=0;i<TableReport.Rows.Count;i++) {
			//	DataRow row = report.TableQ.NewRow();
			//	row[0]=TableReport.Rows[i][0];
			//	row[1]=TableReport.Rows[i][1];
			//	row[2]=TableReport.Rows[i][2];
			//	row[3]=TableReport.Rows[i][3];
			//	row[4]=TableReport.Rows[i][4];
			//	row[5]=TableReport.Rows[i][5];
			//	row[6]=TableReport.Rows[i][6];
			//	row[7]=TableReport.Rows[i][7];
			//	row[8]=TableReport.Rows[i][8];
			//	row[9]=TableReport.Rows[i][9];
			//	gradeNumber+=PIn.Float(TableReport.Rows[i][9].ToString());
			//	report.TableQ.Rows.Add(row);  //adds row to table Q
			//}
			//report.ColTotal[9]=(decimal)gradeNumber/TableReport.Rows.Count;
			//report.Title="Evaluation Average for Course";
			//report.SubTitle.Add(PrefC.GetString(PrefName.PracticeTitle));
			//report.SubTitle.Add(dateStart.SelectionStart.ToShortDateString()+" - "+dateEnd.SelectionStart.ToShortDateString());
			//FormQuery2.ResetGrid();//necessary won't work without
			//report.ColPos[0]=10;
			//report.ColPos[1]=80;
			//report.ColPos[2]=160;
			//report.ColPos[3]=260;
			//report.ColPos[4]=370;
			//report.ColPos[5]=440;
			//report.ColPos[6]=530;
			//report.ColPos[7]=620;
			//report.ColPos[8]=710;
			//report.ColPos[9]=770;
			//report.ColPos[10]=800;
			//report.ColCaption[0]=Lan.g(this,"Course");
			//report.ColCaption[1]=Lan.g(this,"InstNum");
			//report.ColCaption[2]=Lan.g(this,"InstLName");
			//report.ColCaption[3]=Lan.g(this,"InstFName");
			//report.ColCaption[4]=Lan.g(this,"StuNum");
			//report.ColCaption[5]=Lan.g(this,"StuLName");
			//report.ColCaption[6]=Lan.g(this,"StuFName");
			//report.ColCaption[7]=Lan.g(this,"GradScale");
			//report.ColCaption[8]=Lan.g(this,"Grade");
			//report.ColCaption[8]=Lan.g(this,"Number");
			//report.ColAlign[0]=HorizontalAlignment.Center;
			//report.ColAlign[1]=HorizontalAlignment.Center;
			//report.ColAlign[2]=HorizontalAlignment.Center;
			//report.ColAlign[3]=HorizontalAlignment.Center;
			//report.ColAlign[4]=HorizontalAlignment.Center;
			//report.ColAlign[5]=HorizontalAlignment.Center;
			//report.ColAlign[6]=HorizontalAlignment.Center;
			//report.ColAlign[7]=HorizontalAlignment.Center;
			//report.ColAlign[8]=HorizontalAlignment.Center;
			//report.ColAlign[8]=HorizontalAlignment.Right;
			//FormQuery2.ShowDialog();

			////Now to fill Table Q from the temp tables
			//report.TableQ=new DataTable(null);//new table with 9 columns
			//for(int i=0;i<9;i++) { //add columns
			//	report.TableQ.Columns.Add(new System.Data.DataColumn());//blank columns
			//}
			//report.InitializeColumns();
			//for(int i=0;i<TableReport.Rows.Count;i++) {
			//	DataRow row = report.TableQ.NewRow();
			//	row[0]=TableReport.Rows[i];
			//	row[1]=dates[i].DayOfWeek.ToString();
			//	row[2]=production.ToString("n");
			//	row[3]=scheduled.ToString("n");
			//	row[4]=adjust.ToString("n");
			//	row[5]=inswriteoff.ToString("n"); //spk 5/19/05
			//	row[6]=totalproduction.ToString("n");
			//	row[7]=ptincome.ToString("n");				// spk
			//	row[8]=insincome.ToString("n");
			//	report.TableQ.Rows.Add(row);  //adds row to table Q
			//}
			//FormQuery FormQuery2=new FormQuery(report);
			//FormQuery2.IsReport=true;
			//FormQuery2.ResetGrid();//necessary won't work without
			//report.Title="Evaluation Average for Course";
			//report.SubTitle.Add(PrefC.GetString(PrefName.PracticeTitle));
			//report.SubTitle.Add(dateStart.SelectionStart.ToShortDateString()+" - "+dateEnd.SelectionStart.ToShortDateString());
			////if(checkAllInstructors.Checked) {
			////	report.SubTitle.Add(Lan.g(this,"All Instructors"));
			////}
			////else {
			////	string str="";
			////	for(int i=0;i<listProv.SelectedIndices.Count;i++) {
			////		if(i>0) {
			////			str+=", ";
			////		}
			////		str+=ProviderC.ListShort[listProv.SelectedIndices[i]].Abbr;
			////	}
			////	report.SubTitle.Add(str);
			////}
			////report.Summary.Add(
			////	//=Lan.g(this,"Total Production (Production + Scheduled + Adjustments):")+" "
			////	//+(colTotals[2]+colTotals[3]
			////	//+colTotals[4]).ToString("c"); //spk 5/19/05
			////	Lan.g(this,"Total Production (Production + Scheduled + Adj - Writeoff):")+" "
			////	+(colTotals[2]+colTotals[3]+colTotals[4]
			////	+colTotals[5]).ToString("c"));
			////report.Summary.Add("");
			////report.Summary.Add(
			////	Lan.g(this,"Total Income (Pt Income + Ins Income):")+" "
			////	+(colTotals[7]+colTotals[8]).ToString("c"));
			//report.ColPos[0]=20;
			//report.ColPos[1]=110;
			//report.ColPos[2]=190;
			//report.ColPos[3]=270;
			//report.ColPos[4]=350;
			//report.ColPos[5]=420;
			//report.ColPos[6]=490;
			//report.ColPos[7]=560;
			//report.ColPos[8]=630;
			//report.ColCaption[0]=Lan.g(this,"Course");
			//report.ColCaption[1]=Lan.g(this,"InstructorNum");
			//report.ColCaption[2]=Lan.g(this,"Instructor LName");
			//report.ColCaption[3]=Lan.g(this,"Instructor FName");
			//report.ColCaption[4]=Lan.g(this,"StudentNum");
			//report.ColCaption[5]=Lan.g(this,"Student LName");		//spk 5/19/05
			//report.ColCaption[6]=Lan.g(this,"Student FName");
			//report.ColCaption[7]=Lan.g(this,"Grading Scale");		// spk
			//report.ColCaption[8]=Lan.g(this,"Grade");		// spk
			//report.ColAlign[2]=HorizontalAlignment.Right;
			//report.ColAlign[3]=HorizontalAlignment.Right;
			//report.ColAlign[4]=HorizontalAlignment.Right;
			//report.ColAlign[5]=HorizontalAlignment.Right;
			//report.ColAlign[6]=HorizontalAlignment.Right;
			//report.ColAlign[7]=HorizontalAlignment.Right;
			//report.ColAlign[8]=HorizontalAlignment.Right;
			//FormQuery2.ShowDialog();
		}

		private void butStudent_Click(object sender,EventArgs e) {

		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}