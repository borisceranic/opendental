using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans834Preview:Form {

		List <X834> _listX834s;

		public FormEtrans834Preview(List <X834> listX834s) {
			InitializeComponent();
			Lan.F(this);
			_listX834s=listX834s;
		}

		private void FormEtrans834Preview_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillGridInsPlans();
			Cursor=Cursors.Default;
		}

		void FillGridInsPlans() {
			int sortedByColumnIdx=gridInsPlans.SortedByColumnIdx;
			bool isSortAscending=gridInsPlans.SortedIsAscending;
			gridInsPlans.BeginUpdate();
			if(gridInsPlans.Columns.Count==0) {
				gridInsPlans.Columns.Clear();
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Name",200,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Birthdate",74,HorizontalAlignment.Center,UI.GridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SSN",66,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Maint",54,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Begin",84,HorizontalAlignment.Center,UI.GridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Term",84,HorizontalAlignment.Center,UI.GridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Relation",70,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SubscriberID",96,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("GroupNum",100,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Payer",0,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				sortedByColumnIdx=0;//Sort by Patient Last Name by default.
				isSortAscending=true;//Start with A and progress to Z.
			}
			gridInsPlans.Rows.Clear();
			for(int i=0;i<_listX834s.Count;i++) {
				X834 x834=_listX834s[i];
				for(int j=0;j<x834.ListTransactions.Count;j++) {
					Hx834_Tran tran=x834.ListTransactions[j];
					for(int k=0;k<tran.ListMembers.Count;k++) {
						Hx834_Member member=tran.ListMembers[k];						
						if(member.ListHealthCoverage.Count==0) {
							UI.ODGridRow row=new UI.ODGridRow();
							row.Tag=member;
							gridInsPlans.Rows.Add(row);
							row.Cells.Add(member.Pat.GetNameLF());//Name
							row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
							row.Cells.Add(member.Pat.SSN);//SSN
							row.Cells.Add(member.GetPatMaintTypeDescript());//Maint
							row.Cells.Add("");//Date Begin
							row.Cells.Add("");//Date Term
							row.Cells.Add(member.PlanRelat.ToString());//Relation
							row.Cells.Add(member.SubscriberId);//SubscriberID
							row.Cells.Add(member.GroupNum);//GroupNum
							row.Cells.Add(tran.Payer.Name);//Payer
						}
						else {
							for(int a=0;a<member.ListHealthCoverage.Count;a++) {
								Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[a];
								UI.ODGridRow row=new UI.ODGridRow();
								row.Tag=healthCoverage;
								gridInsPlans.Rows.Add(row);
								row.Cells.Add(member.Pat.GetNameLF());//Name
								row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
								row.Cells.Add(member.Pat.SSN);//SSN
								row.Cells.Add(healthCoverage.GetCoverageMaintTypeDescript());//Maint
								row.Cells.Add(healthCoverage.Sub.DateEffective.ToShortDateString());//Date Begin
								row.Cells.Add(healthCoverage.Sub.DateTerm.ToShortDateString());//Date Term
								row.Cells.Add(member.PlanRelat.ToString());//Relation
								row.Cells.Add(member.SubscriberId);//SubscriberID
								row.Cells.Add(member.GroupNum);//GroupNum
								row.Cells.Add(tran.Payer.Name);//Payer
							}
						}
					}
				}
			}
			gridInsPlans.SortForced(sortedByColumnIdx,isSortAscending);
			gridInsPlans.EndUpdate();
		}

		private void butOK_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			//TODO: Get a cache of all patients in order to save processing time.
			for(int i=0;i<gridInsPlans.Rows.Count;i++) {
				Hx834_Member member;
				if(gridInsPlans.Rows[i].Tag is Hx834_Member) {
					member=(Hx834_Member)gridInsPlans.Rows[i].Tag;
				}
				//TODO: Match the member based first on SSN, then on (last name) + (birthdate) + (partial first name)
			}
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

}