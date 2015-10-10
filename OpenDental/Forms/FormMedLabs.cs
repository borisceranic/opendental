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
				checkOnlyNoPat.Checked=true;
			}
			textDateStart.Text=DateTime.Today.AddMonths(-3).ToShortDateString();//default list to start with showing the last three months
			//One time reconcile may need to be run to create embedded PDFs for MedLabs that are not attached to a patient.
			if(!PrefC.GetBool(PrefName.MedLabReconcileDone) && PrefC.AtoZfolderUsed) {
				int countMedLabs=MedLabs.GetCountForPatient(0);
				if(MessageBox.Show(this,Lan.g(this,"There are MedLabs in the database that have not been associated with a patient.\r\nA one time "
					+"reconciliation must be performed that will reprocess the HL7 messages for these MedLabs.  This can take some time.\r\nDo you want to "
					+"continue?\r\nNumber of MedLabs not associated with a patient")+": "+countMedLabs+".","",MessageBoxButtons.YesNo)==DialogResult.No)
				{
					Close();
					return;
				}
				Cursor=Cursors.WaitCursor;
				int reconcileFailedCount=MedLabs.Reconcile();
				Cursor=Cursors.Default;
				if(reconcileFailedCount>0) {
					MessageBox.Show(this,Lan.g(this,"Some of the MedLab objects in the database could not be reconciled.\r\nThis may be due to an issue "
						+"processing the original HL7 message text file.\r\nNumber failed")+": "+reconcileFailedCount);
				}
				Prefs.UpdateBool(PrefName.MedLabReconcileDone,true);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void FillGrid() {
			if(IsDisposed) {//This can happen if an auto logoff happens with FormMedLabEdit open
				return;
			}
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!="")
			{
				return;
			}
			textPatient.Text="";
			if(_selectedPat!=null) {
				textPatient.Text=_selectedPat.GetNameLF();
				checkOnlyNoPat.Checked=false;
			}
			Application.DoEvents();
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
			Cursor=Cursors.WaitCursor;
			List<MedLab> listMedLabs=MedLabs.GetOrdersForPatient(_selectedPat,checkIncludeNoPat.Checked,checkOnlyNoPat.Checked,PIn.Date(textDateStart.Text),dateEnd);
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
			Cursor=Cursors.Default;
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
			if(!checkIncludeNoPat.Checked) {
				checkOnlyNoPat.Checked=false;
			}
			FillGrid();
		}

		private void checkOnlyNoPat_Click(object sender,EventArgs e) {
			if(checkOnlyNoPat.Checked) {
				checkIncludeNoPat.Checked=true;
				_selectedPat=null;
			}
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
