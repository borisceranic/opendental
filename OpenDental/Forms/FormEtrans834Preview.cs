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
			gridInsPlans.BeginUpdate();
			gridInsPlans.Rows.Clear();
			gridInsPlans.Columns.Clear();
			gridInsPlans.Columns.Add(new UI.ODGridColumn("Patient",200));
			gridInsPlans.Columns.Add(new UI.ODGridColumn("Carrier",200));
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
						row.Cells.Add(eIns.Pat.GetNameFL());
						row.Cells.Add("");
					}
					else {
						for(int k=0;k<member.ListHealthCoverage.Count;k++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[k];
							Hx834Ins eIns=new Hx834Ins(member,healthCoverage);
							row=new UI.ODGridRow();
							row.Tag=eIns;
							gridInsPlans.Rows.Add(row);
							row.Cells.Add(eIns.Pat.GetNameFL());
							if(healthCoverage.HealthCoverage!=null) {
								row.Cells.Add(healthCoverage.HealthCoverage.PlanCoverageDescription);
							}
							else {
								row.Cells.Add("");
							}
						}
					}
				}
			}
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