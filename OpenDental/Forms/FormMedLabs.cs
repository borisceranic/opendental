using System;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Data;

namespace OpenDental {
	public partial class FormMedLabs:Form {
		public Patient PatCur;
		private DataTable _tableMedLabs;

		public FormMedLabs() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabs_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Patient",130);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Provider",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Placer Specimen ID",120);//should be the ID sent on the specimen container to lab
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Filler Specimen ID",120);//lab assigned specimen ID
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date & Time Entered",135);//earliest date and time entered into the lab system
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date & Time Reported",135);//most recent date and time a result came in
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Tests Ordered",140);//comma delimeted list of test IDs ordered, will include reflex tests
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_tableMedLabs=MedLabs.GetOrdersForPatient(PatCur.PatNum,checkIncludeNoPat.Checked);
			for(int i=0;i<_tableMedLabs.Rows.Count;i++) {
				row=new ODGridRow();
				if(_tableMedLabs.Rows[i]["PatNum"].ToString()==PatCur.PatNum.ToString()) {
					row.Cells.Add(PatCur.GetNameFLnoPref());
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
				row.Cells.Add(_tableMedLabs.Rows[i]["SpecimenIDFiller"].ToString());
				row.Cells.Add(_tableMedLabs.Rows[i]["DateTimeEntered"].ToString());
				row.Cells.Add(_tableMedLabs.Rows[i]["DateTimeReported"].ToString());
				row.Cells.Add(_tableMedLabs.Rows[i]["TestsOrdered"].ToString().Replace(",",", "));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedLabEdit FormLE=new FormMedLabEdit();
			long patNum=0;
			if(_tableMedLabs.Rows[e.Row]["PatNum"].ToString()==PatCur.PatNum.ToString()) {
				FormLE.PatCur=PatCur;
				patNum=PatCur.PatNum;
			}
			FormLE.ListMedLabs=MedLabs.GetForPatAndSpecimen(patNum,_tableMedLabs.Rows[e.Row]["SpecimenID"].ToString(),
				_tableMedLabs.Rows[e.Row]["SpecimenIDFiller"].ToString());//patNum could be 0 if this MedLab is not attached to a patient
			FormLE.ShowDialog();
			FillGrid();
		}

		private void checkIncludeNoPat_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}

	}
}
