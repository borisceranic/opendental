using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormMedLabs:Form {
		public List<MedLab> ListMedLabs;
		public Patient PatCur;

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
			col=new ODGridColumn("Date Time",80,HorizontalAlignment.Center);//Formatted yyyyMMdd
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Placer Order Number",130,HorizontalAlignment.Center);//Should be PK but might not be. Instead use Placer Order Num.
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Filler Order Number",130,HorizontalAlignment.Center);//Should be PK but might not be. Instead use Placer Order Num.
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Test Performed",430);//Should be PK but might not be. Instead use Placer Order Num.
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Results In",80,HorizontalAlignment.Center);//Or date of latest result? or both?
			gridMain.Columns.Add(col);
			//ListMedLabs = EhrLabs.GetAllForPat(PatCur.PatNum);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListMedLabs.Count;i++) {
				row=new ODGridRow();
				//string dateSt=ListMedLabs[i].ResultDateTime.PadRight(8,'0').Substring(0,8);//stored in DB as yyyyMMddhhmmss-zzzz
				//DateTime dateT=PIn.Date(dateSt.Substring(4,2)+"/"+dateSt.Substring(6,2)+"/"+dateSt.Substring(0,4));
				//row.Cells.Add(dateT.ToShortDateString());//date only
				//row.Cells.Add(ListMedLabs[i].PlacerOrderNum);
				//row.Cells.Add(ListMedLabs[i].FillerOrderNum);
				//row.Cells.Add(ListMedLabs[i].UsiText);
				//row.Cells.Add(ListMedLabs[i].ListEhrLabResults.Count.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butRetrieve_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedLabEdit FormLE=new FormMedLabEdit();
			FormLE.MedLabCur=ListMedLabs[e.Row];
			FormLE.ShowDialog();
			if(FormLE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
			//TODO:maybe add more code here for when we come back from form... In case we delete a lab from the form.
		}

		private void butMove_Click(object sender,EventArgs e) {

		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
	}
}
