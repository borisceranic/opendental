using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans834Preview:Form {

		X834 _x834;
		List<Patient> _listPatients;
		private int _patNumCol;

		public FormEtrans834Preview(X834 x834) {
			InitializeComponent();
			Lan.F(this);
			_x834=x834;
		}

		private void FormEtrans834Preview_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			ShowStatus("Loading patient information");
			checkIsPatientCreate.Checked=PrefC.GetBool(PrefName.Ins834IsPatientCreate);
			_listPatients=Patients.GetAllPatients();//Testing this on an average sized database took about 1 second to run on a dev machine.
			_listPatients.Sort();
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
				gridInsPlans.Columns.Add(new UI.ODGridColumn("PatNum",68,HorizontalAlignment.Center,UI.GridSortingStrategy.StringCompare));
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
			Application.DoEvents();//To show empty grid while the window is loading.
			gridInsPlans.BeginUpdate();
			gridInsPlans.Rows.Clear();
			int rowCount=0;
			for(int i=0;i<_x834.ListTransactions.Count;i++) {
				Hx834_Tran tran=_x834.ListTransactions[i];
				for(int k=0;k<tran.ListMembers.Count;k++) {
					rowCount++;
				}
			}
			for(int i=0;i<_x834.ListTransactions.Count;i++) {
				Hx834_Tran tran=_x834.ListTransactions[i];
				for(int j=0;j<tran.ListMembers.Count;j++) {
					Hx834_Member member=tran.ListMembers[j];
					ShowStatus("Loading "+(gridInsPlans.Rows.Count+1).ToString().PadLeft(6)+"/"+rowCount.ToString().PadLeft(6)
						+"  Patient "+member.Pat.GetNameLF());
					if(member.ListHealthCoverage.Count==0) {
						UI.ODGridRow row=new UI.ODGridRow();
						gridInsPlans.Rows.Add(row);
						FillGridRow(row,member,null);
					}
					else {//There is at least one insurance plan.
						for(int a=0;a<member.ListHealthCoverage.Count;a++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[a];
							UI.ODGridRow row=new UI.ODGridRow();
							gridInsPlans.Rows.Add(row);
							FillGridRow(row,null,healthCoverage);
						}
					}
				}
			}
			gridInsPlans.SortForced(sortedByColumnIdx,isSortAscending);
			gridInsPlans.EndUpdate();
			ShowStatus("");
		}

		///<summary>The healthCoverage variable can be null.</summary>
		private void FillGridRow(UI.ODGridRow row,Hx834_Member member,Hx834_HealthCoverage healthCoverage) {
			row.Cells.Clear();
			if(healthCoverage==null) {
				row.Tag=member;
			}
			else {
				row.Tag=healthCoverage;
				member=healthCoverage.Member;
			}
			row.Cells.Add(member.Pat.GetNameLF());//Name
			if(member.Pat.Birthdate.Year > 1880) {
				row.Cells.Add(member.Pat.Birthdate.ToShortDateString());//Birthdate
			}
			else {
				row.Cells.Add("");//Birthdate
			}
			row.Cells.Add(member.Pat.SSN);//SSN
			List <Patient> listPatientMatches=Patients.GetPatientsByNameAndBirthday(member.Pat,_listPatients);
			if(member.Pat.PatNum==0 && listPatientMatches.Count==1) {
				member.Pat.PatNum=listPatientMatches[0].PatNum;
			}
			if(member.Pat.PatNum==0 && listPatientMatches.Count==0) {
				row.Cells.Add("");//PatNum
			}
			else if(member.Pat.PatNum==0 && listPatientMatches.Count > 1) {
				row.Cells.Add("Multiple");//PatNum
			}
			else {//Either the patient was matched perfectly or the user chose the correct patient already.
				row.Cells.Add(member.Pat.PatNum.ToString());//PatNum
			}
			if(healthCoverage!=null && healthCoverage.Sub.DateEffective.Year > 1880) {
				row.Cells.Add(healthCoverage.Sub.DateEffective.ToShortDateString());//Date Begin
			}
			else {
				row.Cells.Add("");//Date Begin
			}
			if(healthCoverage!=null && healthCoverage.Sub.DateTerm.Year > 1880) {
				row.Cells.Add(healthCoverage.Sub.DateTerm.ToShortDateString());//Date Term
			}
			else {
				row.Cells.Add("");//Date Term
			}
			row.Cells.Add(member.PlanRelat.ToString());//Relation
			row.Cells.Add(member.SubscriberId);//SubscriberID
			row.Cells.Add(member.GroupNum);//GroupNum
			row.Cells.Add(member.Tran.Payer.Name);//Payer
		}

		private void gridInsPlans_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			Hx834_Member member=null;
			Hx834_HealthCoverage healthCoverage=null;
			if(gridInsPlans.Rows[e.Row].Tag is Hx834_Member) {
				member=(Hx834_Member)gridInsPlans.Rows[e.Row].Tag;
			}
			else {
				healthCoverage=(Hx834_HealthCoverage)gridInsPlans.Rows[e.Row].Tag;
				member=healthCoverage.Member;
			}
			FormPatientSelect FormPS=new FormPatientSelect(member.Pat);
			if(FormPS.ShowDialog()==DialogResult.OK) {
				member.Pat.PatNum=FormPS.SelectedPatNum;
				gridInsPlans.BeginUpdate();
				//Refresh all rows for this member to show the newly selected PatNum.
				//There will be multiple rows if there are multiple insurance plans for the member.
				for(int i=0;i<gridInsPlans.Rows.Count;i++) {
					Hx834_Member memberRefresh=null;
					if(gridInsPlans.Rows[i].Tag is Hx834_Member) {
						memberRefresh=(Hx834_Member)gridInsPlans.Rows[i].Tag;
					}
					else {
						memberRefresh=((Hx834_HealthCoverage)gridInsPlans.Rows[i].Tag).Member;
					}
					if(memberRefresh==member) {
						FillGridRow(gridInsPlans.Rows[e.Row],member,healthCoverage);
					}
				}
				gridInsPlans.EndUpdate();
			}
		}

		private bool MoveFileToArchiveFolder() {
			try {
				string dir=Path.GetDirectoryName(_x834.FilePath);
				string dirArchive=ODFileUtils.CombinePaths(dir,"Archive");
				if(!Directory.Exists(dirArchive)) {
					Directory.CreateDirectory(dirArchive);
				}
				string destPathBasic=ODFileUtils.CombinePaths(dirArchive,Path.GetFileName(_x834.FilePath));
				string destPathExt=Path.GetExtension(destPathBasic);
				string destPathBasicRoot=destPathBasic.Substring(0,destPathBasic.Length-destPathExt.Length);
				string destPath=destPathBasic;
				int attemptCount=1;
				while(File.Exists(destPath)) {
					attemptCount++;
					destPath=destPathBasicRoot+"_"+attemptCount+destPathExt;
				}
				File.Move(_x834.FilePath,destPath);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Failed to move file")+" '"+_x834.FilePath+"' "
					+Lan.g(this,"to archive, probably due to a permission issue.")+"  "+ex.Message);
				return false;
			}
			return true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			Prefs.UpdateBool(PrefName.Ins834IsPatientCreate,checkIsPatientCreate.Checked);
			int rowIndex=1;
			int createdPatsCount=0;
			int updatedPatsCount=0;
			int skippedPatsCount=0;
			StringBuilder sbErrorMessages=new StringBuilder();
			List <int> listImportedSegments=new List<int> ();//Used to reconstruct the 834 with imported patients removed.
			for(int i=0;i<_x834.ListTransactions.Count;i++) {
				Hx834_Tran tran=_x834.ListTransactions[i];
				for(int j=0;j<tran.ListMembers.Count;j++) {
					Hx834_Member member=tran.ListMembers[j];
					ShowStatus("Progress "+(rowIndex).ToString().PadLeft(6)+"/"+gridInsPlans.Rows.Count.ToString().PadLeft(6)
						+"  Importing plans for patient "+member.Pat.GetNameLF());
					//The patient's status is not affected by the maintenance code.  After reviewing all of the possible maintenance codes in 
					//member.MemberLevelDetail.MaintenanceTypeCode, we believe that all statuses suggest either insert or update, except for "Cancel".
					//Nathan and Derek feel that archiving the patinet in response to a Cancel request is a bit drastic.
					//Thus we ignore the patient maintenance code and always assume insert/update.
					//Even if the status was "Cancel", then updating the patient does not hurt.
					bool isMemberImported=false;
					bool isMultiMatch=false;
					if(member.Pat.PatNum==0) {
						//The patient may need to be created below.  However, the patient may have already been inserted in a pervious iteration of this loop
						//in following scenario: Two different 834s include updates for the same patient and both documents are being imported at the same time.
						//If the patient was already inserted, then they would show up in _listPatients and also in the database.  Attempt to locate the patient
						//in _listPatients again before inserting.
						List <Patient> listPatientMatches=Patients.GetPatientsByNameAndBirthday(member.Pat,_listPatients);
						if(listPatientMatches.Count==1) {
							member.Pat.PatNum=listPatientMatches[0].PatNum;
						}
						else if(listPatientMatches.Count > 1) {
							isMultiMatch=true;
						}
					}
					if(isMultiMatch) {
						skippedPatsCount++;
					}
					else if(member.Pat.PatNum==0 && checkIsPatientCreate.Checked) {
						Patients.Insert(member.Pat,false);
						int patIdx=_listPatients.BinarySearch(member.Pat);//Preserve sort order by locating the index in which to insert the newly added patient.
						int insertIdx=~patIdx;//According to MSDN, the index returned by BinarySearch() is a "bitwise compliment"
						_listPatients.Insert(insertIdx,member.Pat);
						SecurityLogs.MakeLogEntry(Permissions.PatientCreate,member.Pat.PatNum,"Created from Import Ins Plans 834.",LogSources.InsPlanImport834);
						isMemberImported=true;
						createdPatsCount++;
					}
					else if(member.Pat.PatNum==0 && !checkIsPatientCreate.Checked) {
						skippedPatsCount++;
					}
					else {//member.Pat.PatNum!=0
						Patient patDb=_listPatients.FirstOrDefault(x => x.PatNum==member.Pat.PatNum);//Locate by PatNum, in case user selected manually.
						member.MergePatientIntoDbPatient(patDb);
						_listPatients.Remove(patDb);//Remove patient from list so we can add it back in the correct location (in case name or bday changed).
						int patIdx=_listPatients.BinarySearch(patDb);//Preserve sort order by locating the index in which to insert the newly added patient.
						//patIdx could be positive if the user manually selected a patient when there were multiple matches found.
						//If patIdx is negative, then according to MSDN, the index returned by BinarySearch() is a "bitwise compliment".
						int insertIdx=(patIdx > 0)?patIdx:~patIdx;
						_listPatients.Insert(insertIdx,patDb);
						isMemberImported=true;
						updatedPatsCount++;
					}
					if(isMemberImported) {
						//Import insurance changes for patient.
						for(int k=0;k<member.ListHealthCoverage.Count;k++) {
							Hx834_HealthCoverage healthCoverage=member.ListHealthCoverage[k];
							if(k > 0) {
								rowIndex++;
							}
							//TODO: Import insurance plans.
						}//end loop k
						//Remove the member from the X834.
						int endSegIndex=0;
						if(j < tran.ListMembers.Count-1) {
							endSegIndex=tran.ListMembers[j+1].MemberLevelDetail.SegmentIndex-1;
						}
						else {
							X12Segment segSe=_x834.GetNextSegmentById(member.MemberLevelDetail.SegmentIndex+1,"SE");//SE segment is required.
							endSegIndex=segSe.SegmentIndex-1;
						}
						for(int s=member.MemberLevelDetail.SegmentIndex;s<=endSegIndex;s++) {
							listImportedSegments.Add(s);
						}

					}
					rowIndex++;
				}//end loop j
			}//end loop i
			if(listImportedSegments.Count > 0 && skippedPatsCount > 0) {//Some patients imported, while others did not.
				if(MoveFileToArchiveFolder()) {
					//Save the unprocessed members back to the import directory, so the user can try to import them again.
					File.WriteAllText(_x834.FilePath,_x834.ReconstructRaw(listImportedSegments));
				}
			}
			else if(listImportedSegments.Count > 0) {//All patinets imported.
				MoveFileToArchiveFolder();
			}
			else if(skippedPatsCount > 0) {//No patients imported.  All patients were skipped.
				//Leave the raw file unaltered and where it is, so it can be processed again.
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