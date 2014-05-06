using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormOrthoChart:Form {
		private PatField[] listPatientFields;
		private List<DisplayField> listOrthDisplayFields;
		private List<OrthoChart> listOrthoCharts;
		private Patient _patCur;
		private List<string> _listDisplayFieldNames;
		///<summary>Each row in this table has a date as the first cell.  There will be additional rows that are not yet in the db.  Each blank cell will be an empty string.  It will also store changes made by the user prior to closing the form.  When the form is closed, this table will be compared with the original listOrthoCharts and a synch process will take place to save to db.  An empty string in a cell will result in no db row or a deletion of existing db row.</summary>
		DataTable table;

		public FormOrthoChart(Patient patCur) {
			_patCur = patCur;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormOrthoChart_Load(object sender,EventArgs e) {
			//define the table----------------------------------------------------------------------------------------------------------
			table=new DataTable("OrthoChartForPatient");
			//define columns----------------------------------------------------------------------------------------------------------
			table.Columns.Add("Date",typeof(DateTime));
			listOrthDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.OrthoChart);
			for(int i=0;i<listOrthDisplayFields.Count;i++) {
				table.Columns.Add((i+1).ToString());//named by number, but probably refer to by index
			}
			//define rows------------------------------------------------------------------------------------------------------------
			listOrthoCharts=OrthoCharts.GetAllForPatient(_patCur.PatNum);
			List<DateTime> datesShowing=new List<DateTime>();
			_listDisplayFieldNames=new List<string>();
			for(int i=0;i<listOrthDisplayFields.Count;i++) {//fill listDisplayFieldNames to be used in comparison
				_listDisplayFieldNames.Add(listOrthDisplayFields[i].Description);
			}
			//start adding dates starting with today's date
			datesShowing.Add(DateTime.Today);
			for(int i=0;i<listOrthoCharts.Count;i++) {
				if(!_listDisplayFieldNames.Contains(listOrthoCharts[i].FieldName)) {//skip rows not in display fields
					continue;
				}
				if(!datesShowing.Contains(listOrthoCharts[i].DateService)) {//add dates not already in date list
					datesShowing.Add(listOrthoCharts[i].DateService);
				}
			}
			datesShowing.Sort();
			//We now have a list of dates.
			//add all blank cells to each row except for the date.
			DataRow row;
			//create and add row for each date in date showing
			for(int i=0;i<datesShowing.Count;i++) {
				row=table.NewRow();
				row["Date"]=datesShowing[i];
				for(int j=0;j<listOrthDisplayFields.Count;j++) {
					row[j+1]="";//j+1 because first row is date field.
				}
				table.Rows.Add(row);
			}
			//We now have a table with all empty strings in cells except dates.
			//Fill with data as necessary.
			for(int i=0;i<listOrthoCharts.Count;i++) {//loop
				if(!datesShowing.Contains(listOrthoCharts[i].DateService)){
					continue;
				}
				if(!_listDisplayFieldNames.Contains(listOrthoCharts[i].FieldName)){
					continue;
				}
				for(int j=0;j<table.Rows.Count;j++) {
					if(listOrthoCharts[i].DateService==(DateTime)table.Rows[j]["Date"]) {
						table.Rows[j][_listDisplayFieldNames.IndexOf(listOrthoCharts[i].FieldName)+1]=listOrthoCharts[i].FieldValue;
					}
				}
			}
			FillGrid();
			FillGridPat();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			//First column will always be the date.  gridMain_CellLeave() depends on this fact.
			col=new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			for(int i=0;i<listOrthDisplayFields.Count;i++) {
				col=new ODGridColumn(listOrthDisplayFields[i].Description,listOrthDisplayFields[i].ColumnWidth,true);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				//First column will always be the date.  gridMain_CellLeave() depends on this fact.
				DateTime tempDate=(DateTime)table.Rows[i]["Date"];
				row.Cells.Add(tempDate.ToShortDateString());
				row.Tag=tempDate;
				for(int j=0;j<listOrthDisplayFields.Count;j++) {
					row.Cells.Add(table.Rows[i][j+1].ToString());
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridPat() {
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Field",150);
			gridPat.Columns.Add(col);
			col=new ODGridColumn("Value",200);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			listPatientFields=PatFields.Refresh(_patCur.PatNum);
			PatFieldDefs.RefreshCache();
			ODGridRow row;
			//define and fill rows in grid at the same time.
			for(int i=0;i<PatFieldDefs.List.Length;i++) {
				row=new ODGridRow();
				row.Cells.Add(PatFieldDefs.List[i].FieldName);
				for(int j=0;j<=listPatientFields.Length;j++) {
					if(j==listPatientFields.Length) {//no matches in the list
						row.Cells.Add("");
						break;
					}
					if(listPatientFields[j].FieldName==PatFieldDefs.List[i].FieldName) {
						if(PatFieldDefs.List[i].FieldType==PatFieldType.Checkbox) {
							row.Cells.Add("X");
						}
						else if(PatFieldDefs.List[i].FieldType==PatFieldType.Currency) {
							row.Cells.Add(PIn.Double(listPatientFields[j].FieldValue).ToString("c"));
						}
						else {
							row.Cells.Add(listPatientFields[j].FieldValue);
						}
						break;
					}
				}
				gridPat.Rows.Add(row);
			}
			gridPat.EndUpdate();
		}

		///<summary>Gets all the OrthoChartNums for each field in the selected ortho chart row (if they exists in the database).  Returns list with 0 as an entry if none found.</summary>
		private List<long> GetOrthoChartNumsForRow(int row) {
			List<long> orthoChartNums=new List<long>() { 0 };
			for(int i=0;i<listOrthoCharts.Count;i++) {
				if(listOrthoCharts[i].DateService!=(DateTime)table.Rows[row]["Date"]) {
					continue;
				}
				if(!_listDisplayFieldNames.Contains(listOrthoCharts[i].FieldName)) {
					continue;//No need to audit columns that are not showing in the UI.
				}
				//This is a cell in the corresponding row that they want to see an audit trail for.
				orthoChartNums.Add(listOrthoCharts[i].OrthoChartNum);
			}
			return orthoChartNums;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			/*
			if(e.Col==0){//cannot edit a date
				FormOrthoChartAddDate FormOCAD = new FormOrthoChartAddDate();
				FormOCAD.ShowDialog();
				if(FormOCAD.DialogResult!=DialogResult.OK) {
					return;
				}
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(FormOCAD.SelectedDate.ToShortDateString()==gridMain.Rows[i].Cells[0].Text) {
						MsgBox.Show(this,"That date already exists.");
						return;
					}
				}
				listDatesAdditional.Add(FormOCAD.SelectedDate);
				FillGrid();
				return;
			}
			////create an orthoChart that has this date and this type
			//FormOrthoChartEdit FormOCE = new FormOrthoChartEdit();
			//if(gridMain.Rows[e.Row].Cells[e.Col].Text==" ") {//new ortho chart
			//  FormOCE.OrthoCur.DateService = DateTime.Parse(gridMain.Rows[e.Row].Cells[0].Text);
			//  FormOCE.OrthoCur.FieldName = gridMain.Columns[e.Col].Heading;
			//  FormOCE.IsNew=true;
			//}
			//else {//existing ortho chart
			//  for(int i=0;i<listOrthoCharts.Count;i++) {
			//    if(listOrthoCharts[i].DateService.ToShortDateString()==gridMain.Rows[e.Row].Cells[0].Text
			//    && listOrthoCharts[i].FieldName==gridMain.Columns[e.Col].Heading) {
			//      FormOCE.OrthoCur=listOrthoCharts[i];
			//      break;
			//    }
			//  }
			//}
			//FormOCE.ShowDialog();
			//if(FormOCE.DialogResult!=DialogResult.OK) {
			//  return;
			//}
			//if(FormOCE.IsNew) {
			//  OrthoCharts.Insert(FormOCE.OrthoCur);
			//}
			//else {
			//  OrthoCharts.Update(FormOCE.OrthoCur);
			//}*/
			//FillGrid();
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			//Get the the date for the ortho chart that was just edited.
			DateTime orthoDate=PIn.Date(gridMain.Rows[e.Row].Cells[0].Text);//First column will always be the date.
			//Suppress the security message because it's crazy annoying if the user is simply clicking around in cells.  They might be copying a cell and not changing it.
			if(Security.IsAuthorized(Permissions.OrthoChartEdit,orthoDate,true)) {
				return;//The user has permission.  No need to waste time doing logic below.
			}
			//User is not authorized to edit this cell.  Check if they changed the old value and if they did, put it back to the way it was and warn them about security.
			string oldText="";//If the selected cell is not in listOrthoCharts then it started out blank.  This will put it back to an empty string.
			for(int i=0;i<listOrthoCharts.Count;i++) {
				if(listOrthoCharts[i].DateService!=orthoDate) {
					continue;
				}
				if(listOrthoCharts[i].FieldName!=gridMain.Columns[e.Col].Heading) {
					continue;
				}
				//This is the cell that the user was editing and it had an entry in the database.  Put it back to the way it was.
				oldText=listOrthoCharts[i].FieldValue;
				break;
			}
			if(gridMain.Rows[e.Row].Cells[e.Col].Text!=oldText) {
				//The user actually changed the cell's value and we need to change it back and warn them that they don't have permission.
				gridMain.Rows[e.Row].Cells[e.Col].Text=oldText;
				gridMain.Invalidate();
				Security.IsAuthorized(Permissions.OrthoChartEdit,orthoDate);//This will pop up the message.
			}
			return;
		}

		private void gridPat_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PatField field=PatFields.GetByName(PatFieldDefs.List[e.Row].FieldName,listPatientFields);
			if(field==null) {
				field=new PatField();
				field.PatNum=_patCur.PatNum;
				field.FieldName=PatFieldDefs.List[e.Row].FieldName;
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Text) {
					FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
					FormPF.IsLaunchedFromOrtho=true;
					FormPF.IsNew=true;
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.PickList) {
					FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(field);
					FormPF.IsNew=true;
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Date) {
					FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(field);
					FormPF.IsNew=true;
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Checkbox) {
					FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(field);
					FormPF.IsNew=true;
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Currency) {
					FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(field);
					FormPF.IsNew=true;
					FormPF.ShowDialog();
				}
			}
			else {
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Text) {
					FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.PickList) {
					FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(field);
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Date) {
					FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(field);
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Checkbox) {
					FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(field);
					FormPF.ShowDialog();
				}
				if(PatFieldDefs.List[e.Row].FieldType==PatFieldType.Currency) {
					FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(field);
					FormPF.ShowDialog();
				}
			}
			FillGridPat();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormOrthoChartAddDate FormOCAD = new FormOrthoChartAddDate();
			FormOCAD.ShowDialog();
			if(FormOCAD.DialogResult!=DialogResult.OK) {
				return;
			}
			for(int i=0;i<table.Rows.Count;i++) {
				if(FormOCAD.SelectedDate==(DateTime)table.Rows[i]["Date"]) {
					MsgBox.Show(this,"That date already exists.");
					return;
				}
			}
			//listDatesAdditional.Add(FormOCAD.SelectedDate);
			//Move data from grid to table, add new date row to datatable, then fill grid from table.
			for(int i=0;i<gridMain.Rows.Count;i++) {
				table.Rows[i]["Date"]=gridMain.Rows[i].Tag;//store date
				for(int j=0;j<listOrthDisplayFields.Count;j++) {
					table.Rows[i][j+1]=gridMain.Rows[i].Cells[j+1].Text;
				}
			}
			DataRow row;
			row=table.NewRow();
			row["Date"]=FormOCAD.SelectedDate;
			for(int i=0;i<listOrthDisplayFields.Count;i++) {
				row[i+1]="";//j+1 because first row is date field.
			}
			//insert new row in proper ascending datetime order to dataTable
			for(int i=0;i<=table.Rows.Count;i++){
				if(i==table.Rows.Count){
					table.Rows.InsertAt(row,i);
					break;
				}
				if((DateTime)row["Date"]>(DateTime)table.Rows[i]["Date"]){
					continue;
				}
				table.Rows.InsertAt(row,i);
				break;
			}
			FillGrid();
		}

		private void butUseAutoNote_Click(object sender,EventArgs e) {
			if(gridMain.SelectedCell.X==-1 || gridMain.SelectedCell.X==0) {
				MsgBox.Show(this,"Please select an editable Ortho Chart cell first.");
				return;
			}
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				//Add text to current focused cell				
				gridMain.Rows[gridMain.SelectedCell.Y].Cells[gridMain.SelectedCell.X].Text=FormA.CompletedNote;
				//Move data from grid to table
				table.Rows[gridMain.SelectedCell.Y][gridMain.SelectedCell.X]=FormA.CompletedNote;
				//Refresh grid
				FillGrid();
			}
		}

		private void butAudit_Click(object sender,EventArgs e) {
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Please select an ortho chart field or date first.");
				return;
			}
			//We cannot show audit trails for deleted ortho charts because we delete entries in the ortho chart table.
			//So we have to look and see if the ortho chart entry is in the db still and then use that PK to show the unique audit trail.
			List<long> orthoChartNums=GetOrthoChartNumsForRow(gridMain.SelectedCell.Y);
			List<Permissions> perms=new List<Permissions>();
			perms.Add(Permissions.OrthoChartEdit);
			FormAuditOneType FormA=new FormAuditOneType(_patCur.PatNum,perms,Lan.g(this,"Audit Trail for Ortho Chart"),0);
			SecurityLog[] orthoChartLogs=SecurityLogs.Refresh(_patCur.PatNum,new List<Permissions> { Permissions.OrthoChartEdit },orthoChartNums);
			SecurityLog[] patientFieldLogs=SecurityLogs.Refresh(new DateTime(1,1,1),DateTime.Today,Permissions.PatientFieldEdit,_patCur.PatNum,0);
			List<SecurityLog> listLogs=new List<SecurityLog>();
			listLogs.AddRange(orthoChartLogs);//Show the ortho chart logs first.  There might be a lot of patient field logs.
			listLogs.AddRange(patientFieldLogs);
			FormA.LogList=listLogs.ToArray();
			FormA.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormOrthoChart_FormClosing(object sender,FormClosingEventArgs e) {
			//Save data from grid to table
			for(int i=0;i<gridMain.Rows.Count;i++) {
				table.Rows[i]["Date"]=gridMain.Rows[i].Tag;//store date
				for(int j=0;j<listOrthDisplayFields.Count;j++) {
					table.Rows[i][j+1]=gridMain.Rows[i].Cells[j+1].Text;
				}
			} 
			List<OrthoChart> tempOrthoChartsFromDB=OrthoCharts.GetAllForPatient(_patCur.PatNum);
			List<OrthoChart> tempOrthoChartsFromTable=new List<OrthoChart>();
			for(int r=0;r<table.Rows.Count;r++) {
				for(int c=1;c<table.Columns.Count;c++) {//skip col 0
					OrthoChart tempChart = new OrthoChart();
					tempChart.DateService=(DateTime)table.Rows[r]["Date"];
					tempChart.FieldName=listOrthDisplayFields[c-1].Description;
					tempChart.FieldValue=table.Rows[r][c].ToString();
					tempChart.PatNum=_patCur.PatNum;
					tempOrthoChartsFromTable.Add(tempChart);
				}
			}
			//Check table list vs DB list for inserts and updates.
			for(int i=0;i<tempOrthoChartsFromTable.Count;i++) {
				//Update the Record if it already exists or Insert if it's new.
				for(int j=0;j<=tempOrthoChartsFromDB.Count;j++) {
					//Insert if you've made it through the whole list.
					if(j==tempOrthoChartsFromDB.Count) {
						OrthoCharts.Insert(tempOrthoChartsFromTable[i]);
						break;
					}
					//Update if type and date match
					if(tempOrthoChartsFromDB[j].DateService==tempOrthoChartsFromTable[i].DateService 
							&& tempOrthoChartsFromDB[j].FieldName==tempOrthoChartsFromTable[i].FieldName) 
					{
						tempOrthoChartsFromTable[i].OrthoChartNum=tempOrthoChartsFromDB[j].OrthoChartNum;
						//Make a security log if the user has changed anything.
						if(tempOrthoChartsFromTable[i].FieldValue!=tempOrthoChartsFromDB[j].FieldValue) {
							SecurityLogs.MakeLogEntry(Permissions.OrthoChartEdit,_patCur.PatNum
								,Lan.g(this,"Ortho chart field edited.  Field date")+": "+tempOrthoChartsFromDB[j].DateService.ToShortDateString()+"  "
									+Lan.g(this,"Field name")+": "+tempOrthoChartsFromDB[j].FieldName+"\r\n"
									+Lan.g(this,"Old value")+": \""+tempOrthoChartsFromDB[j].FieldValue+"\"  "
									+Lan.g(this,"New value")+": \""+tempOrthoChartsFromTable[i].FieldValue+"\""
								,tempOrthoChartsFromDB[j].OrthoChartNum);
						}
						OrthoCharts.Update(tempOrthoChartsFromTable[i]);
						break;
					}
				}
			}
		}

		
	}
}