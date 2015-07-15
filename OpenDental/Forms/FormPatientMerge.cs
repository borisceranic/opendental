using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPatientMerge:Form {

		public FormPatientMerge() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPatientMerge_Load(object sender,EventArgs e) {
		}

		private void butChangePatientInto_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK){
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				this.textPatientIDInto.Text=selectedPatNum.ToString();
				Patient pat=Patients.GetPat(selectedPatNum);
				this.textPatientNameInto.Text=pat.GetNameFLFormal();
				this.textPatToBirthdate.Text=pat.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void butChangePatientFrom_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK) {
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				this.textPatientIDFrom.Text=selectedPatNum.ToString();
				Patient pat=Patients.GetPat(selectedPatNum);
				this.textPatientNameFrom.Text=pat.GetNameFLFormal();
				this.textPatFromBirthdate.Text=pat.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void CheckUIState(){
			this.butMerge.Enabled=(this.textPatientIDInto.Text.Trim()!="" && this.textPatientIDFrom.Text.Trim()!="");
		}

		private void butMerge_Click(object sender,EventArgs e) {
			long patTo=Convert.ToInt64(this.textPatientIDInto.Text.Trim());
			long patFrom=Convert.ToInt64(this.textPatientIDFrom.Text.Trim());
			if(patTo==patFrom){
				MsgBox.Show(this,"Cannot merge a patient account into itself. Please select a different patient to merge from.");
				return;
			}
			Patient patientFrom=Patients.GetPat(patFrom);
			Patient patientTo=Patients.GetPat(patTo);
			if(patientFrom.FName.Trim().ToLower()!=patientTo.FName.Trim().ToLower() ||
					patientFrom.LName.Trim().ToLower()!=patientTo.LName.Trim().ToLower() ||
					patientFrom.Birthdate!=patientTo.Birthdate) 
			{//mismatch
				if(Programs.UsingEcwTightOrFullMode()) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,@"The two selected patients do not have the same first name, last name, and date of birth.  The patients must first be merged from within eCW, then immediately merged in the same order in Open Dental.  If the patients are not merged in this manner, some information may not properly bridge between eCW and Open Dental.
Into patient name: "+patientTo.FName+" "+patientTo.LName+", Into patient birthdate: "+patientTo.Birthdate.ToShortDateString()+@".
From patient name: "+patientFrom.FName+" "+patientFrom.LName+", From paient birthdate: "+patientFrom.Birthdate.ToShortDateString()+@".
Merge the patient at the bottom into the patient shown at the top?")) 
					{
						return;//The user chose not to merge
					}
				}
				else {//not eCW
					MsgBox.Show(this,"The two selected patients do not have the same first name, last name, and date of birth.  You must set all of those the same before merge is allowed.");
					return;//Do not merge.
				}
			}
			else {//name and bd match
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Merge the patient at the bottom into the patient shown at the top?")) {
					return;//The user chose not to merge.
				}
			}
			this.Cursor=Cursors.WaitCursor;
			if(patientFrom.PatNum==patientFrom.Guarantor){
				Family fam=Patients.GetFamily(patFrom);
				if(fam.ListPats.Length>1){
					this.Cursor=Cursors.Default;
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
						"The patient you have chosen to merge from is a guarantor. Merging this patient into another account will "
						+"cause all familiy members of the patient being merged from to be moved into the same family as the "
						+"patient account being merged into. Do you wish to continue with the merge?")) {
						return;//The user chose not to merge.
					}
					this.Cursor=Cursors.WaitCursor;
				}
			}
			if(Patients.MergeTwoPatients(patTo,patFrom)) {
				//The patient has been successfully merged.
				//Now copy the physical images from the old patient to the new if they are using an AtoZ image share.
				//This has to happen in the UI because the middle tier server might not have access to the image share.
				//If the users are storing images within the database, those images have already been taken care of in the merge method above.
				if(PrefC.AtoZfolderUsed) {
					#region Copy AtoZ Documents
					//Move the patient documents within the 'patFrom' A to Z folder to the 'patTo' A to Z folder.
					//We have to be careful here of documents with the same name. We have to rename such documents
					//so that no documents are overwritten/lost.
					string atoZpath=ImageStore.GetPreferredAtoZpath();
					string atozFrom=ImageStore.GetPatientFolder(patientFrom,atoZpath);
					string atozTo=ImageStore.GetPatientFolder(patientTo,atoZpath);
					int fileCopyFailures=0;
					string[] fromFiles=Directory.GetFiles(atozFrom);
					for(int i=0;i<fromFiles.Length;i++) {
						string fileName=Path.GetFileName(fromFiles[i]);
						string destFileName=fileName;
						string destFilePath=ODFileUtils.CombinePaths(atozTo,fileName);
						if(File.Exists(destFilePath)) {
							//The file being copied has the same name as a possibly different file within the destination a to z folder.
							//We need to copy the file under a unique file name and then make sure to update the document table to reflect
							//the change.
							destFileName=patientFrom.PatNum.ToString()+"_"+fileName;
							destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
							while(File.Exists(destFilePath)) {
								destFileName=patientFrom.PatNum.ToString()+"_"+fileName+"_"+DateTime.Now.ToString("yyyyMMddhhmmss");
								destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
							}
						}
						bool isCopied=true;
						try {
							File.Copy(fromFiles[i],destFilePath); //Will throw exception if file already exists.
						}
						catch {
							isCopied=false;
							fileCopyFailures++;
						}
						if(isCopied) {//If the copy did not fail, try to delete the old file.
							//We can now safely update the document FileName and PatNum to the "to" patient.
							Documents.MergePatientDocument(patFrom,patTo,fileName,destFileName);
							try {
								File.Delete(fromFiles[i]);
							}
							catch {
								//If we were unable to delete the file then it is probably because someone has the document open currently.
								//Just skip deleting the file. This means that occasionally there will be an extra file in their backup
								//which is just clutter but at least the merge is guaranteed this way.
							}
						}
					}
					if(fileCopyFailures>0) {
						MessageBox.Show(Lan.g(this,"Some files belonging to the from patient were not copied.")+"\r\n"
						                +Lan.g(this,"Number of files not copied")+": "+fileCopyFailures);
					}
					#endregion
				}
				this.textPatientIDFrom.Text="";
				this.textPatientNameFrom.Text="";
				this.textPatFromBirthdate.Text="";
				CheckUIState();
				MsgBox.Show(this,"Patients merged successfully.");
				//Make log entry here not in parent form because we can merge multiple patients at a time.
				SecurityLogs.MakeLogEntry(Permissions.PatientMerge,patientTo.PatNum,"Patient: "+patientFrom.GetNameFL()+"\r\nPatNum From: "+patientFrom.PatNum+"\r\nPatNum To: "+patientTo.PatNum);
			}
			this.Cursor=Cursors.Default;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}