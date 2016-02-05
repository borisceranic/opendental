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
			FillGridInsPlans();
		}

		void FillGridInsPlans() {
			int sortedByColumnIdx=gridInsPlans.SortedByColumnIdx;
			bool isSortAscending=gridInsPlans.SortedIsAscending;
			gridInsPlans.BeginUpdate();
			if(gridInsPlans.Columns.Count==0) {	
				gridInsPlans.Columns.Clear();
				gridInsPlans.Columns.Add(new UI.ODGridColumn("LName",150,HorizontalAlignment.Left));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("FName",100,HorizontalAlignment.Left));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Middle",50,HorizontalAlignment.Left));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Birthdate",80,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SSN",80,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Pat Maint",80,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Cov Maint",80,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Begin",84,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Term",84,HorizontalAlignment.Center));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Relation",80,HorizontalAlignment.Left));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SubscriberID",100,HorizontalAlignment.Left));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("GroupNum",100,HorizontalAlignment.Left));
				//TODO: Contact information?
				//TODO: Carrier information.
				gridInsPlans.Columns.Add(new UI.ODGridColumn("",1));//Spacer
				sortedByColumnIdx=0;//Sort by Patient Last Name by default.
				isSortAscending=true;//Start with A and progress to Z.
			}
			gridInsPlans.Rows.Clear();
			for(int i=0;i<_listX834s.Count;i++) {
				X834 x834=_listX834s[i];
				for(int j=0;j<x834.ListMembers.Count;j++) {
					Hx834_Member member=x834.ListMembers[j];
					UI.ODGridRow row;
					if(member.ListHealthCoverage.Count==0) {
						Hx834Ins eIns=new Hx834Ins(member,null);
						row=new UI.ODGridRow();
						row.Tag=eIns;
						gridInsPlans.Rows.Add(row);
						row.Cells.Add(eIns.Pat.LName);
						row.Cells.Add(eIns.Pat.FName);
						row.Cells.Add(eIns.Pat.MiddleI);
						row.Cells.Add(eIns.Pat.Birthdate.ToShortDateString());
						row.Cells.Add(eIns.Pat.SSN);
						row.Cells.Add(eIns.GetPatMaintTypeDescript());
						row.Cells.Add("");
						row.Cells.Add(eIns.Sub.DateEffective.ToShortDateString());
						row.Cells.Add(eIns.Sub.DateTerm.ToShortDateString());
						row.Cells.Add(eIns.Plan.Relationship.ToString());
						row.Cells.Add(eIns.Sub.SubscriberID);
						row.Cells.Add(eIns.Ins.GroupNum);
						row.Cells.Add("");//Spacer
					}
					else {
						for(int k=0;k<member.ListHealthCoverage.Count;k++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[k];
							Hx834Ins eIns=new Hx834Ins(member,healthCoverage);
							row=new UI.ODGridRow();
							row.Tag=eIns;
							gridInsPlans.Rows.Add(row);
							row.Cells.Add(eIns.Pat.LName);
							row.Cells.Add(eIns.Pat.FName);
							row.Cells.Add(eIns.Pat.MiddleI);
							row.Cells.Add(eIns.Pat.Birthdate.ToShortDateString());
							row.Cells.Add(eIns.Pat.SSN);
							row.Cells.Add(eIns.GetPatMaintTypeDescript());
							row.Cells.Add(eIns.GetCoverageMaintTypeDescript());
							row.Cells.Add(eIns.Sub.DateEffective.ToShortDateString());
							row.Cells.Add(eIns.Sub.DateTerm.ToShortDateString());
							row.Cells.Add(eIns.Plan.Relationship.ToString());
							row.Cells.Add(eIns.Sub.SubscriberID);
							row.Cells.Add(eIns.Ins.GroupNum);
							row.Cells.Add("");//Spacer
						}
					}
				}
			}
			gridInsPlans.SortForced(sortedByColumnIdx,isSortAscending);
			gridInsPlans.EndUpdate();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

}