using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormProcCodeEditMore:Form {
		public List<Fee> ListFees;
		private ProcedureCode _procCode;
		private List<FeeSched> _listFeeScheds;

		public FormProcCodeEditMore(ProcedureCode procCode) {
			InitializeComponent();
			Lan.F(this);
			_procCode=procCode;
		}

		private void FormProcCodeEditMore_Load(object sender,EventArgs e) {
			if(ListFees==null) {
				ListFees=Fees.GetListt();
			}
			_listFeeScheds=FeeSchedC.GetListShort();
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Schedule"),200);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Provider"),135);
				gridMain.Columns.Add(col);
			}
			else {//Using clinics.
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Schedule"),130);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Clinic"),130);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Provider"),75);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Amount"),100,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			List<Fee> listFees=new List<Fee>();
			for(int i=0;i<_listFeeScheds.Count;i++) {
				listFees.AddRange(ListFees.FindAll(fee => fee.FeeSched==_listFeeScheds[i].FeeSchedNum && fee.CodeNum==_procCode.CodeNum)
								.OrderBy(fee => fee.ClinicNum)
								.ThenBy(fee => fee.ProvNum));
			}
			ODGridRow row;
			long lastFeeSched=0;
			for(int i=0;i<listFees.Count;i++) {
				row=new ODGridRow();
				if(listFees[i].FeeSched!=lastFeeSched) {
					row.Cells.Add(FeeScheds.GetDescription(listFees[i].FeeSched));
					row.Bold=true;
					lastFeeSched=listFees[i].FeeSched;
					row.ColorBackG=Color.LightBlue;
					if(listFees[i].ClinicNum!=0 || listFees[i].ProvNum!=0) { //FeeSched change, but not with a default fee. Insert placeholder row.
						if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
							row.Cells.Add("");
						}
						row.Cells.Add("");
						row.Cells.Add("");
						Fee fee=new Fee();
						fee.FeeSched=listFees[i].FeeSched;
						row.Tag=fee;
						gridMain.Rows.Add(row);
						//Now that we have a placeholder for the default fee (none was found), go about adding the next row (non-default fee).
						row=new ODGridRow();
						row.Cells.Add("");
					}
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=listFees[i];
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) { //Using clinics
					row.Cells.Add(Clinics.GetDesc(listFees[i].ClinicNum)); //Returns "" if invalid clinicnum (ie. 0)
				}
				row.Cells.Add(Providers.GetAbbr(listFees[i].ProvNum)); //Returns "" if invalid provnum (ie. 0)
				row.Cells.Add(listFees[i].Amount.ToString("n"));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			Fee fee=(Fee)gridMain.Rows[e.Row].Tag;
			FormFeeEdit FormFE=new FormFeeEdit();
			if(fee.FeeNum==0) {
				FormFE.IsNew=true;
				fee.CodeNum=_procCode.CodeNum;
				Fees.Insert(fee);//Pre-insert the fee before opening the edit window.
			}
			FormFE.FeeCur=fee;
			FormFE.ShowDialog();
			//FormFE could have updated or deleted the fee.  Refresh our cache regardless of what happened in the edit window.
			Fees.RefreshCache();
			ListFees=Fees.GetListt();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}