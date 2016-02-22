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
			if(healthCoverage!=null && healthCoverage.DateEffective.Year > 1880) {
				row.Cells.Add(healthCoverage.DateEffective.ToShortDateString());//Date Begin
			}
			else {
				row.Cells.Add("");//Date Begin
			}
			if(healthCoverage!=null && healthCoverage.DateTerm.Year > 1880) {
				row.Cells.Add(healthCoverage.DateTerm.ToShortDateString());//Date Term
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
			if(!MsgBox.Show(this,true,"Importing insurance plans is a database intensive operation and can take 10 minutes or more to run.  "
				+"It is best to import insurance plans after hours or during another time period when database usage is otherwise low.\r\n"
				+"Click OK to import insurance plans now, or click Cancel."))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			Prefs.UpdateBool(PrefName.Ins834IsPatientCreate,checkIsPatientCreate.Checked);
			int rowIndex=1;
			int createdPatsCount=0;
			int updatedPatsCount=0;
			int skippedPatsCount=0;
			int createdCarrierCount=0;
			int createdPlanCount=0;
			int droppedPlanCount=0;
			int updatedPlanCount=0;
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
						//The code here mimcs the behavior of FormPatientSelect.butAddPt_Click().
						Patients.Insert(member.Pat,false);
						Patient memberPatOld=member.Pat.Copy();
						member.Pat.PatStatus=PatientStatus.Patient;
						member.Pat.BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType);
						if(!PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
							//Set the patients primary provider to the practice default provider.
							member.Pat.PriProv=Providers.GetDefaultProvider().ProvNum;
						}
						member.Pat.ClinicNum=Clinics.ClinicNum;
						member.Pat.Guarantor=member.Pat.PatNum;
						Patients.Update(member.Pat,memberPatOld);
						int patIdx=_listPatients.BinarySearch(member.Pat);//Preserve sort order by locating the index in which to insert the newly added patient.
						int insertIdx=~patIdx;//According to MSDN, the index returned by BinarySearch() is a "bitwise compliment", since not currently in list.
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
							List <Carrier> listCarriers=Carriers.GetByNameAndElectId(tran.Payer.Name,tran.Payer.IdentificationCode);							
							if(listCarriers.Count==0) {
								Carrier carrier=new Carrier();
								carrier.CarrierName=tran.Payer.Name;
								carrier.ElectID=tran.Payer.IdentificationCode;
								Carriers.Insert(carrier);
								DataValid.SetInvalid(InvalidType.Carriers);
								listCarriers.Add(carrier);
								createdCarrierCount++;
							}
							//Update insurance plans.  Match based on carrier and SubscriberId.
							bool isDropping=false;
							if(healthCoverage.HealthCoverage.MaintenanceTypeCode=="002") {
								isDropping=true;
							}
							Family fam=Patients.GetFamily(member.Pat.PatNum);
							List<InsSub> listInsSubs=InsSubs.RefreshForFam(fam);
							List <InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
							List <PatPlan> listPatPlans=PatPlans.Refresh(member.Pat.PatNum);
							InsSub insSubMatch=null;
							InsPlan insPlanMatch=null;
							PatPlan patPlanMatch=null;
							for(int p=0;p<listInsSubs.Count;p++) {
								InsSub insSub=listInsSubs[p];
								//According to section 1.4.3 of the standard, the preferred method of matching a dependent to a subscriber is to use the subscriberId.
								if(insSub.SubscriberID.Trim()!=member.SubscriberId.Trim()) {
									continue;
								}
								InsPlan insPlan=InsPlans.GetPlan(insSub.PlanNum,listInsPlans);
								//We also require a carrier match, because we expect the sender to give us
								//the same carrier information for the insurance plans of both the subscriber and dependent.
								Carrier carrier=listCarriers.FirstOrDefault(x => x.CarrierNum==insPlan.CarrierNum);
								if(carrier==null) {
									continue;
								}
								insSubMatch=insSub;
								insPlanMatch=insPlan;
								patPlanMatch=PatPlans.GetFromList(listPatPlans,insSub.InsSubNum);
								break;
							}							
							if(patPlanMatch==null && isDropping) {//No plan match and dropping plan.
								//Nothing to do.  The plan either never existed or is already dropped.
							}
							else if(patPlanMatch==null && !isDropping) {//No plan match and not dropping plan.  Create the plan.
								//The code below mimics how insurance plans are created in ContrFamily.ToolButIns_Click().
								if(insPlanMatch==null) {
									insPlanMatch=new InsPlan();
									if(member.InsFiling!=null) {
										insPlanMatch.FilingCode=member.InsFiling.InsFilingCodeNum;
									}
									insPlanMatch.GroupNum=member.GroupNum;
									insPlanMatch.CarrierNum=listCarriers[0].CarrierNum;
									insPlanMatch.ClaimFormNum=PrefC.GetLong(PrefName.DefaultClaimForm);
									InsPlans.Insert(insPlanMatch);
								}
								else {
									if(member.InsFiling!=null) {
										insPlanMatch.FilingCode=member.InsFiling.InsFilingCodeNum;
									}
									insPlanMatch.GroupNum=member.GroupNum;
									InsPlans.Update(insPlanMatch);
								}
								if(insSubMatch==null) {
									insSubMatch=new InsSub();
									insSubMatch.PlanNum=insPlanMatch.PlanNum;
									//According to section 1.4.3 of the 834 standard, subscribers must be sent in the 834 before dependents.
									//This requirement facilitates easier linking of dependents to their subscribers.
									//Since we were not able to locate a subscriber within the family above, we can safely assume that the insurance plan in the 834
									//is for the subscriber.  Thus we can set the Subscriber PatNum to the same PatNum as the patient.
									insSubMatch.Subscriber=member.Pat.PatNum;
									insSubMatch.SubscriberID=member.SubscriberId;
									insSubMatch.ReleaseInfo=member.IsReleaseInfo;
									insSubMatch.AssignBen=true;
									insSubMatch.DateEffective=healthCoverage.DateEffective;
									insSubMatch.DateTerm=healthCoverage.DateTerm;
									InsSubs.Insert(insSubMatch);
								}
								else {
									insSubMatch.DateEffective=healthCoverage.DateEffective;
									insSubMatch.DateTerm=healthCoverage.DateTerm;
									insSubMatch.ReleaseInfo=member.IsReleaseInfo;
									InsSubs.Update(insSubMatch);
								}
								patPlanMatch=new PatPlan();
								patPlanMatch.Ordinal=0;
								for(int p=0;p<listPatPlans.Count;p++) {
									if(listPatPlans[p].Ordinal > patPlanMatch.Ordinal) {
										patPlanMatch.Ordinal=listPatPlans[p].Ordinal;
									}
								}
								patPlanMatch.Ordinal++;//Greatest ordinal for patient.
								patPlanMatch.PatNum=member.Pat.PatNum;
								patPlanMatch.InsSubNum=insSubMatch.InsSubNum;
								patPlanMatch.Relationship=member.PlanRelat;
								if(member.PlanRelat!=Relat.Self) {
									member.Pat.Guarantor=insSubMatch.Subscriber;
									Patient memberPatOld=member.Pat.Copy();
									Patients.Update(member.Pat,memberPatOld);
								}
								PatPlans.Insert(patPlanMatch);
								createdPlanCount++;
							}
							else if(patPlanMatch!=null && isDropping) {//Plan matched and dropping plan.  Drop the plan.
								//This code mimics the behavior of FormInsPlan.butDrop_Click(), except here we do not care if there are claims for this plan today.
								//We need this feature to be as streamlined as possible so that it might become an automated process later.
								//Testing for claims on today's date does not seem that useful anyway, or at least not as useful as checking for any claims
								//associated to the plan, instead of just today's date.
								PatPlans.Delete(patPlanMatch.PatPlanNum);//Estimates recomputed within Delete()
								droppedPlanCount++;
							}
							else if(patPlanMatch!=null && !isDropping)  {//Plan matched and not dropping plan.  Update the plan.
								patPlanMatch.Relationship=member.PlanRelat;
								PatPlans.Update(patPlanMatch);
								insSubMatch.DateEffective=healthCoverage.DateEffective;
								insSubMatch.DateTerm=healthCoverage.DateTerm;
								insSubMatch.ReleaseInfo=member.IsReleaseInfo;
								InsSubs.Update(insSubMatch);
								if(member.InsFiling!=null) {
									insPlanMatch.FilingCode=member.InsFiling.InsFilingCodeNum;
								}
								insPlanMatch.GroupNum=member.GroupNum;
								InsPlans.Update(insPlanMatch);
								updatedPlanCount++;
							}
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
			if(createdCarrierCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of carriers created:")+" "+createdCarrierCount;
				msg+=sbErrorMessages.ToString();
			}
			if(createdPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of plans created:")+" "+createdPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(droppedPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of plans dropped:")+" "+droppedPlanCount;
				msg+=sbErrorMessages.ToString();
			}
			if(updatedPlanCount > 0) {
				msg+="\r\n"+Lan.g(this,"Number of plans updated:")+" "+updatedPlanCount;
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