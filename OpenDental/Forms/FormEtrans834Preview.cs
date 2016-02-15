using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans834Preview:Form {

		List <X834> _listX834s;
		List<Patient> _listPatients;
		private int _patNumCol;

		public FormEtrans834Preview(List <X834> listX834s) {
			InitializeComponent();
			Lan.F(this);
			_listX834s=listX834s;
		}

		private void FormEtrans834Preview_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			ShowStatus("Loading patient information");
			checkIsPatientCreate.Checked=PrefC.GetBool(PrefName.Ins834IsPatientCreate);
			_listPatients=Patients.GetAllPatients();//Testing this on an average sized database took about 1 second to run on a dev machine.
			FillGridInsPlans();
			ShowStatus("");
			Cursor=Cursors.Default;
		}

		///<summary>Shows current status to user in title bar.  Useful for when processing for a few seconds or more.</summary>
		private void ShowStatus(string message) {
			int index=Text.IndexOf(" - ");
			if(index >= 0) {
				Text=Text.Substring(0,index+3);//Remove old status, but keep the dash and spaces.
			}
			else {
				Text+=" - ";//Ensure there is a separating dash with spaces.
			}
			if(message.Trim()=="") {
				index=Text.IndexOf(" - ");
				if(index >= 0) {
					Text=Text.Substring(0,index);//Remove dash a separating spaces if message is empty.
				}
			}
			else {
				Text+=message;
			}
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
				_patNumCol=gridInsPlans.Columns.Count;
				gridInsPlans.Columns.Add(new UI.ODGridColumn("PatNum",54,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Begin",84,HorizontalAlignment.Center,UI.GridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Date Term",84,HorizontalAlignment.Center,UI.GridSortingStrategy.DateParse));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Relation",70,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("SubscriberID",96,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("GroupNum",100,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				gridInsPlans.Columns.Add(new UI.ODGridColumn("Payer",0,HorizontalAlignment.Left,UI.GridSortingStrategy.StringCompare));
				sortedByColumnIdx=0;//Sort by Patient Last Name by default.
				isSortAscending=true;//Start with A and progress to Z.
			}
			gridInsPlans.EndUpdate();
			Application.DoEvents();//To show the blank grid.
			gridInsPlans.BeginUpdate();
			gridInsPlans.Rows.Clear();
			int rowCount=0;
			for(int i=0;i<_listX834s.Count;i++) {
				X834 x834=_listX834s[i];
				for(int j=0;j<x834.ListTransactions.Count;j++) {
					Hx834_Tran tran=x834.ListTransactions[j];
					for(int k=0;k<tran.ListMembers.Count;k++) {
						rowCount++;
					}
				}
			}
			for(int i=0;i<_listX834s.Count;i++) {
				X834 x834=_listX834s[i];
				for(int j=0;j<x834.ListTransactions.Count;j++) {
					Hx834_Tran tran=x834.ListTransactions[j];
					for(int k=0;k<tran.ListMembers.Count;k++) {
						Hx834_Member member=tran.ListMembers[k];
						ShowStatus("Loading "+(gridInsPlans.Rows.Count+1).ToString().PadLeft(6)+"/"+rowCount.ToString().PadLeft(6)
							+"  Patient "+member.Pat.GetNameLF());						
						//Locate a match within the database based on first name, last name, and birthdate.
						List <Patient> listPatientMatches=Patients.GetPatientsByNameAndBirthday(member.Pat.LName,member.Pat.FName,member.Pat.Birthdate,_listPatients);
						if(member.Pat.PatNum==0 && listPatientMatches.Count==1) {
							member.Pat.PatNum=listPatientMatches[0].PatNum;
						}
						if(member.ListHealthCoverage.Count==0) {
							UI.ODGridRow row=new UI.ODGridRow();
							row.Tag=member;
							gridInsPlans.Rows.Add(row);
							row.Cells.Add(member.Pat.GetNameLF());//Name
							if(member.Pat.Birthdate.Year > 1880) {
								row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
							}
							else {
								row.Cells.Add("");//Birthdate
							}
							row.Cells.Add(member.Pat.SSN);//SSN
							if(member.Pat.PatNum==0 && listPatientMatches.Count==0) {
								row.Cells.Add("");//PatNum
							}
							else if(member.Pat.PatNum==0 && listPatientMatches.Count > 1) {
								row.Cells.Add("Multiple");//PatNum
							}
							else {//Either the patient was matched perfectly or the user chose the correct patient already.
								row.Cells.Add(member.Pat.PatNum.ToString());//PatNum
							}
							row.Cells.Add("");//Date Begin
							row.Cells.Add("");//Date Term
							row.Cells.Add(member.PlanRelat.ToString());//Relation
							row.Cells.Add(member.SubscriberId);//SubscriberID
							row.Cells.Add(member.GroupNum);//GroupNum
							row.Cells.Add(tran.Payer.Name);//Payer
						}
						else {//There is at least one insurance plan.
							for(int a=0;a<member.ListHealthCoverage.Count;a++) {
								Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[a];
								UI.ODGridRow row=new UI.ODGridRow();
								row.Tag=healthCoverage;
								gridInsPlans.Rows.Add(row);
								row.Cells.Add(member.Pat.GetNameLF());//Name
								if(member.Pat.Birthdate.Year > 1880) {
									row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
								}
								else {
									row.Cells.Add("");//Birthdate
								}
								row.Cells.Add(member.Pat.SSN);//SSN
								if(member.Pat.PatNum==0 && listPatientMatches.Count==0) {
									row.Cells.Add("");//PatNum
								}
								else if(member.Pat.PatNum==0 && listPatientMatches.Count > 1) {
									row.Cells.Add("Multiple");//PatNum
								}
								else {//Either the patient was matched perfectly or the user chose the correct patient already.
									row.Cells.Add(member.Pat.PatNum.ToString());//PatNum
								}
								if(healthCoverage.Sub.DateEffective.Year > 1880) {
									row.Cells.Add(healthCoverage.Sub.DateEffective.ToShortDateString());//Date Begin
								}
								else {
									row.Cells.Add("");//Date Begin
								}
								if(healthCoverage.Sub.DateTerm.Year > 1880) {
									row.Cells.Add(healthCoverage.Sub.DateTerm.ToShortDateString());//Date Term
								}
								else {
									row.Cells.Add("");//Date Term
								}
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
			ShowStatus("");
		}

		private void butOK_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			Prefs.UpdateBool(PrefName.Ins834IsPatientCreate,checkIsPatientCreate.Checked);
			int createdPatsCount=0;
			int updatedPatsCount=0;
			int skippedPatsCount=0;
			StringBuilder sbErrorMessages=new StringBuilder();
			for(int i=0;i<gridInsPlans.Rows.Count;i++) {
				Hx834_Member member=null;
				Hx834_HealthCoverage healthCoverage=null;
				if(gridInsPlans.Rows[i].Tag is Hx834_Member) {
					member=(Hx834_Member)gridInsPlans.Rows[i].Tag;
				}
				else if(gridInsPlans.Rows[i].Tag is Hx834_HealthCoverage) {
					healthCoverage=(Hx834_HealthCoverage)gridInsPlans.Rows[i].Tag;
					member=healthCoverage.Member;
				}
				ShowStatus("Progress "+(i+1).ToString().PadLeft(6)+"/"+gridInsPlans.Rows.Count.ToString().PadLeft(6)
					+"  Importing plans for patient "+member.Pat.GetNameLF());
				//The patient's status is not affected by the maintenance code.  After reviewing all of the possible maintenance codes in 
				//member.MemberLevelDetail.MaintenanceTypeCode, we believe that all statuses suggest either insert or update, except for "Cancel".
				//Nathan and Derek feel that archiving the patinet in response to a Cancel request is a bit drastic.
				//Thus we ignore the patient maintenance code and always assume insert/update.
				//Even if the status was "Cancel", then updating the patient does not hurt.
				if(member.Pat.PatNum==0 && gridInsPlans.Rows[i].Cells[_patNumCol].Text!="") {//Multiple matches
					skippedPatsCount++;
				}
				else if(member.Pat.PatNum==0 && checkIsPatientCreate.Checked) {
					//TODO: How are patinets being duplicated?  Probably due to multiple plans for the same patient.
					Patients.Insert(member.Pat,false);
					SecurityLogs.MakeLogEntry(Permissions.PatientCreate,member.Pat.PatNum,"Created from Import Ins Plans 834.",LogSources.InsPlanImport834);
					createdPatsCount++;
				}
				else if(member.Pat.PatNum==0 && !checkIsPatientCreate.Checked) {
					skippedPatsCount++;
				}
				else {//member.Pat.PatNum!=0
					Patient patDb=_listPatients.FirstOrDefault(x => x.PatNum==member.Pat.PatNum);
					member.MergePatientIntoDbPatient(patDb);
					updatedPatsCount++;
				}
				if(healthCoverage==null) {
					continue;
				}

			}
			Cursor=Cursors.Default;
			string msg=Lan.g(this,"Done.");
			if(createdPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients created:")+" "+createdPatsCount;
			}
			if(updatedPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients updated:")+" "+updatedPatsCount;
			}
			if(skippedPatsCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of patients skipped:")+" "+skippedPatsCount;
				msg+=sbErrorMessages.ToString();
			}
			MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(msg);
			msgBox.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}

}