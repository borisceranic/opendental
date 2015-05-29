using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMedLabs:Form {
		public Patient PatCur;
		private DataTable _tableMedLabs;
		///<summary>Used to show the labs for a specific patient.  May be the same as PatCur or a different selected patient or null for all patients.</summary>
		private Patient _selectedPat;

		public FormMedLabs() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabs_Load(object sender,EventArgs e) {
			_selectedPat=PatCur;
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
			col=new ODGridColumn("Date & Time Reported",150);//most recent date and time a result came in
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Patient",170);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Provider",120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Specimen ID",100);//should be the ID sent on the specimen container to lab
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Test(s) Description",200);//description of the test ordered
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Result Count",110);//count of results received for this test
			col.SortingStrategy=GridSortingStrategy.AmountParse;
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			DateTime dateEnd=PIn.Date(textDateEnd.Text);
			if(dateEnd==DateTime.MinValue) {
				dateEnd=DateTime.MaxValue;
			}
			_tableMedLabs=MedLabs.GetOrdersForPatient(_selectedPat,checkIncludeNoPat.Checked,checkGroupBySpec.Checked,PIn.Date(textDateStart.Text),dateEnd);
			for(int i=0;i<_tableMedLabs.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_tableMedLabs.Rows[i]["DateTimeReported"].ToString());
				long patNum=PIn.Long(_tableMedLabs.Rows[i]["PatNum"].ToString());
				if(patNum>0) {
					row.Cells.Add(Patients.GetLim(patNum).GetNameFL());
				}
				else {
					row.Cells.Add("");
				}
				long provNum=0;
				try {
					provNum=PIn.Long(_tableMedLabs.Rows[i]["ProvNum"].ToString());
				}
				catch(Exception ex) {
					//do nothing, provNum will remain 0
				}
				row.Cells.Add(Providers.GetAbbr(provNum));
				row.Cells.Add(_tableMedLabs.Rows[i]["SpecimenID"].ToString());
				row.Cells.Add(_tableMedLabs.Rows[i]["ObsTestDescript"].ToString());
				row.Cells.Add(_tableMedLabs.Rows[i]["ResultCount"].ToString());
				row.Tag=_tableMedLabs.Rows[i]["PatNum"].ToString()+","+_tableMedLabs.Rows[i]["SpecimenID"].ToString()+","
					+_tableMedLabs.Rows[i]["SpecimenIDFiller"].ToString();
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(checkGroupBySpec.Checked) {
				return;
			}
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(gridMain.Rows[i].Tag.ToString()==gridMain.Rows[e.Row].Tag.ToString()) {
					gridMain.Rows[i].ColorText=Color.Red;
				}
				else {
					gridMain.Rows[i].ColorText=Color.Black;
				}
			}
			gridMain.Invalidate();
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
