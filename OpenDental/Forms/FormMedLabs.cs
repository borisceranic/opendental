using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMedLabs:Form {
		public Patient PatCur;
		///<summary>Used to show the labs for a specific patient.  May be the same as PatCur or a different selected patient or null for all patients.</summary>
		private Patient _selectedPat;

		public FormMedLabs() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabs_Load(object sender,EventArgs e) {
			_selectedPat=PatCur;
			if(_selectedPat==null) {
				checkIncludeNoPat.Checked=true;
			}
			FillGrid();
		}

		private void FillGrid() {
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!="")
			{
				return;
			}
			textPatient.Text="";
			if(_selectedPat!=null) {
				textPatient.Text=_selectedPat.GetNameLF();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Date & Time Reported",135);//most recent date and time a result came in
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date & Time Entered",135);
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",75);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Patient",180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Provider",70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Specimen ID",100);//should be the ID sent on the specimen container to lab
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Test(s) Description",230);//description of the test ordered
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			DateTime dateEnd=PIn.Date(textDateEnd.Text);
			if(dateEnd==DateTime.MinValue) {
				dateEnd=DateTime.MaxValue;
			}
			List<MedLab> listMedLabs=MedLabs.GetOrdersForPatient(_selectedPat,checkIncludeNoPat.Checked,PIn.Date(textDateStart.Text),dateEnd);
			for(int i=0;i<listMedLabs.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listMedLabs[i].DateTimeReported.ToString("MM/dd/yyyy hh:mm tt"));
				row.Cells.Add(listMedLabs[i].DateTimeEntered.ToString("MM/dd/yyyy hh:mm tt"));
				if(listMedLabs[i].IsPreliminaryResult) {//check whether the test or any of the most recent results for the test is marked as preliminary
					row.Cells.Add(MedLabs.GetStatusDescript(ResultStatus.P));
				}
				else {
					row.Cells.Add(MedLabs.GetStatusDescript(listMedLabs[i].ResultStatus));
				}
				string nameFL="";
				if(listMedLabs[i].PatNum>0) {
					nameFL=Patients.GetLim(listMedLabs[i].PatNum).GetNameFLnoPref();
				}
				row.Cells.Add(nameFL);
				row.Cells.Add(Providers.GetAbbr(listMedLabs[i].ProvNum));//will be blank if ProvNum=0
				row.Cells.Add(listMedLabs[i].SpecimenID);
				row.Cells.Add(listMedLabs[i].ObsTestDescript);
				row.Tag=listMedLabs[i].PatNum.ToString()+","+listMedLabs[i].SpecimenID+","+listMedLabs[i].SpecimenIDFiller;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedLabEdit FormLE=new FormMedLabEdit();
			long patNum=0;
			string[] patSpecimenIds=gridMain.Rows[e.Row].Tag.ToString().Split(new string[] { "," },StringSplitOptions.None);
			if(patSpecimenIds.Length>0) {
				patNum=PIn.Long(patSpecimenIds[0]);//if PatNum portion of the tag is an empty string, patNum will remain 0
			}
			FormLE.PatCur=Patients.GetPat(patNum);//could be null if PatNum=0
			string specimenId="";
			string specimenIdFiller="";
			if(patSpecimenIds.Length>1) {
				specimenId=patSpecimenIds[1];
			}
			if(patSpecimenIds.Length>2) {
				specimenIdFiller=patSpecimenIds[2];
			}
			FormLE.ListMedLabs=MedLabs.GetForPatAndSpecimen(patNum,specimenId,specimenIdFiller);//patNum could be 0 if this MedLab is not attached to a pat
			FormLE.ShowDialog();
			FillGrid();
		}

		private void checkIncludeNoPat_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkGroupBySpec_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butCurrent_Click(object sender,EventArgs e) {
			_selectedPat=PatCur;
			FillGrid();
		}

		private void butFind_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedPat=Patients.GetPat(FormPS.SelectedPatNum);
			FillGrid();
		}

		private void butAll_Click(object sender,EventArgs e) {
			_selectedPat=null;
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}
		
	}
}
