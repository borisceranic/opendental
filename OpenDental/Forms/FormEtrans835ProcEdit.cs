using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans835ProcEdit:Form {

		private X835 _x835;
		private int _segSvcIndex;
		private string _strProcNum;
		private string _strProcCode;
		private string _strProcFee;
		private string _strInsPaid;
		private List<string> _listProcedureAdjustments;
		private List<string> _listRemarks;
		private decimal _procAdjAmtSum;

		public FormEtrans835ProcEdit(X835 x835,int segSvcIndex,string strProcNum,string strProcCode,string strProcFee,string strInsPaid) {
			InitializeComponent();
			Lan.F(this);
			_x835=x835;
			_segSvcIndex=segSvcIndex;
			_strProcNum=strProcNum;
			_strProcCode=strProcCode;
			_strProcFee=strProcFee;
			_strInsPaid=strInsPaid;
		}

		private void FormEtrans835ClaimEdit_Load(object sender,EventArgs e) {
			FillAll();
		}

		private void FormEtrans835ClaimEdit_Resize(object sender,EventArgs e) {
			FillProcedureAdjustments();//Because the grid columns change size depending on the form size.
			FillRemarks();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillProcedureAdjustments();
			FillHeader();
			FillRemarks();
		}

		private void FillHeader() {
			textProcDescript.Text="";
			if(ProcedureCodes.IsValidCode(_strProcCode)) {
				textProcDescript.Text=ProcedureCodes.GetProcCode(_strProcCode).AbbrDesc;
			}
			textProcNum.Text=_strProcNum;
			textProcCode.Text=_strProcCode;
			textProcFee.Text=_strProcFee;
			textInsPaid.Text=_strInsPaid;
			textInsPaidCalc.Text=(PIn.Decimal(_strProcFee)-_procAdjAmtSum).ToString("f2");
		}

		private void FillProcedureAdjustments() {
			_listProcedureAdjustments=_x835.GetProcAdjustmentInfo(_segSvcIndex);
			if(_listProcedureAdjustments.Count==0) {
				gridProcedureAdjustments.Title="Procedure Adjustments (None Reported)";
			}
			else {
				gridProcedureAdjustments.Title="Procedure Adjustments";
			}
			gridProcedureAdjustments.BeginUpdate();
			gridProcedureAdjustments.Columns.Clear();
			const int colWidthDescription=200;
			const int colWidthAdjAmt=80;
			int colWidthVariable=gridProcedureAdjustments.Width-10-colWidthDescription-colWidthAdjAmt;
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("Description",colWidthDescription,HorizontalAlignment.Left));
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureAdjustments.Rows.Clear();
			_procAdjAmtSum=0;
			for(int i=0;i<_listProcedureAdjustments.Count;i+=3) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(_listProcedureAdjustments[i]));//Description
				row.Cells.Add(new ODGridCell(_listProcedureAdjustments[i+1]));//Reason
				row.Cells.Add(new ODGridCell(_listProcedureAdjustments[i+2]));//AdjAmt
				decimal adjAmount=PIn.Decimal(_listProcedureAdjustments[i+2]);
				_procAdjAmtSum+=adjAmount;
				gridProcedureAdjustments.Rows.Add(row);
			}
			gridProcedureAdjustments.EndUpdate();
			textProcAdjAmtSum.Text=_procAdjAmtSum.ToString("f2");
		}

		private void FillRemarks() {
			_listRemarks=_x835.GetProcRemarks(_segSvcIndex);
			if(_listRemarks.Count==0) {
				gridRemarks.Title="Remarks (None Reported)";
			}
			else {
				gridRemarks.Title="Remarks";
			}
			gridRemarks.BeginUpdate();
			gridRemarks.Columns.Clear();
			gridRemarks.Columns.Add(new UI.ODGridColumn("",gridRemarks.Width,HorizontalAlignment.Left));
			gridRemarks.Rows.Clear();
			for(int i=0;i<_listRemarks.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(new UI.ODGridCell(_listRemarks[i]));
				gridRemarks.Rows.Add(row);
			}
			gridRemarks.EndUpdate();
		}


		private void gridRemarks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_listRemarks[e.Row]);
			msgbox.Show(this);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}