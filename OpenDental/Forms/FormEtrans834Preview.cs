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
						Etrans834Ins eIns=new Etrans834Ins(member,null);
						row=new UI.ODGridRow();
						row.Tag=eIns;
						gridInsPlans.Rows.Add(row);
						row.Cells.Add(eIns.Pat.GetNameFL());
						row.Cells.Add("");
					}
					else {
						for(int k=0;k<member.ListHealthCoverage.Count;k++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[k];
							Etrans834Ins eIns=new Etrans834Ins(member,healthCoverage);
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

	public class Etrans834Ins {

		public Hx834_Member Member;
		public Patient Pat;
		public Hx834_HealthCoverage HealthCoverage;

		public Etrans834Ins(Hx834_Member member,Hx834_HealthCoverage healthCoverage) {
			Member=member;
			Pat=new Patient();
			Pat.FName=member.MemberName.NameFirst;
			Pat.MiddleI=member.MemberName.NameMiddle;
			Pat.LName=member.MemberName.NameLast;
			if(member.MemberMailingAddress!=null) {
				Pat.Address=member.MemberMailStreetAddress.AddressInformation1;
				Pat.Address2=member.MemberMailStreetAddress.AddressInformation2;
				Pat.City=member.MemberMailCityStateZipCode.CityName;
				Pat.State=member.MemberMailCityStateZipCode.StateOrProvinceCode;
				Pat.Zip=member.MemberMailCityStateZipCode.PostalCode;
			}
			Pat.StudentStatus="N";
			if(member.ListMemberSchools.Count > 0) {
				Pat.SchoolName=member.ListMemberSchools[0].MemberSchool.NameLast;
				Pat.StudentStatus="F";//There is no way to tell if the student is part time or full-time from this data.
			}
			HealthCoverage=healthCoverage;
			if(healthCoverage!=null) {
				Pat.HasIns="I";
			}
		}
	}

}