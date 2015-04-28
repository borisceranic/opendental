using System;
using System.Collections.Generic;
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
		}

		private void FormMedLabs_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Prov",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Placer Specimen ID",130);//should be the ID sent on the specimen container to lab
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Filler Specimen ID",130);//lab assigned specimen ID
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date & Time Entered",140);//earliest date and time entered into the lab system
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date & Time Reported",140);//most recent date and time a result came in
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Tests Ordered",240);//comma delimeted list of test IDs ordered, will include reflex tests
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_tableMedLabs=MedLabs.GetOrdersForPatient(PatCur.PatNum);
			for(int i=0;i<_tableMedLabs.Rows.Count;i++) {
				row=new ODGridRow();
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
			FormLE.PatCur=PatCur;
			FormLE.ListMedLabs=MedLabs.GetForPatAndSpecimen(PatCur.PatNum,_tableMedLabs.Rows[e.Row]["SpecimenID"].ToString(),
				_tableMedLabs.Rows[e.Row]["SpecimenIDFiller"].ToString());
			FormLE.ShowDialog();
			if(FormLE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void butMove_Click(object sender,EventArgs e) {

		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}
		
	}
}
