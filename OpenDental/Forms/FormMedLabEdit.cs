using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;

namespace OpenDental {
	public partial class FormMedLabEdit:Form {
		///<summary>Passed in, or set here if selecting a new patient for medlab. Can be null.</summary>
		public Patient PatCur;
		///<summary>List of all MedLabs linked to this patient and specimen. Passed in from calling class, or set here if newly attached to patient.</summary>
		public List<MedLab> ListMedLabs;
		///<summary>Aggregated final results from all of the med lab orders in ListMedLabs.</summary>
		private List<MedLabResult> _listResults;
		///<summary>True when PatCur.PatNum is different than MedLab.PatNum</summary>
		private bool _isNewPat;
		///<summary>Usually the first MedLab in ListMedLabs. Used for convenience instead of continuously referencing ListMedLabs[0]. 
		///Since all MedLabs in ListMedLabs have the same SpecimenID and SpecimenIDFiller, it is safe to assume all MedLab objects have
		///the same value for some of the fields and we will just pull from the first MedLab in the list.</summary>
		private MedLab _medLabCur;

		public FormMedLabEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabEdit_Load(object sender,EventArgs e) {
			_medLabCur=ListMedLabs[0];
			SetFields();
		}

		///<summary>Used to set all of the fields on the form.
		///Called on load and when a HL7 message is reprocessed to refresh the fields with the new object information.</summary>
		private void SetFields() {
			#region Patient and Patient Address Group Box
			RefreshPatientData();
			textSpecimenNumber.Text=_medLabCur.PatIDLab;
			textFasting.Text=_medLabCur.PatFasting.ToString();
			if(_medLabCur.PatFasting==YN.Unknown) {
				textFasting.Text="";
			}
			#endregion Patient and Patient Address Group Box
			textDateTCollect.Text=_medLabCur.DateTimeCollected.ToString("MM/dd/yyyy hh:mm tt");
			textDateEntered.Text=_medLabCur.DateTimeEntered.ToShortDateString();
			textDateTReport.Text=_medLabCur.DateTimeReported.ToString("MM/dd/yyyy hh:mm tt");
			textTotVol.Text=_medLabCur.TotalVolume;
			#region Odering Physician Group Box
			textPhysicianName.Text=_medLabCur.OrderingProvLName;
			if(_medLabCur.OrderingProvLName!="") {
				textPhysicianName.Text+=", ";
			}
			textPhysicianName.Text+=_medLabCur.OrderingProvFName;
			textPhysicianNPI.Text=_medLabCur.OrderingProvNPI;
			textPhysicianID.Text=_medLabCur.OrderingProvLocalID;
			#endregion Odering Physician Group Box
			string patNote="";
			string labNote="";
			string clinicalInfo="";
			List<string> listTestIds=new List<string>();
			string testsOrdered="";
			for(int i=0;i<ListMedLabs.Count;i++) {
				//Each NotePat may be repeated and we do not want to display multiple copies of the same note. Only add unique notes to PatNote.
				if(!Regex.IsMatch(patNote,Regex.Escape(ListMedLabs[i].NotePat),RegexOptions.IgnoreCase)) {
					if(patNote!="") {
						patNote+="\r\n";
					}
					patNote+=ListMedLabs[i].NotePat;
				}
				//Each NoteLab may be repeated and we do not want to display multiple copies of the same note. Only add unique notes to NoteLab.
				if(!Regex.IsMatch(labNote,Regex.Escape(ListMedLabs[i].NoteLab),RegexOptions.IgnoreCase)) {
					if(labNote!="") {
						labNote+="\r\n";
					}
					labNote+=ListMedLabs[i].NoteLab;
				}
				//Each Clinical Info may be repeated and we do not want to display multiple copies of the same note. Only add unique information to Clinical Info.
				if(!Regex.IsMatch(clinicalInfo,Regex.Escape(ListMedLabs[i].ClinicalInfo),RegexOptions.IgnoreCase)) {
					if(clinicalInfo!="") {
						clinicalInfo+="\r\n";
					}
					clinicalInfo+=ListMedLabs[i].ClinicalInfo;
				}
				//Build list of ordered tests
				if(!listTestIds.Contains(ListMedLabs[i].ObsTestID)
					&& ListMedLabs[i].ActionCode!=ResultAction.G)//"G" indicates a reflex test, not a test originally ordered, so skip these
				{
					listTestIds.Add(ListMedLabs[i].ObsTestID);
					if(testsOrdered!="") {
						testsOrdered+="\r\n";
					}
					testsOrdered+=ListMedLabs[i].ObsTestID+" - "+ListMedLabs[i].ObsTestDescript;
				}
			}
			if(patNote!="" && labNote!="") {
				patNote+="\r\n";
			}
			//concatenate all notes together for display in the same textbox
			patNote+=labNote;
			textGenComments.Text=patNote;
			textAddlInfo.Text=clinicalInfo;
			textTestsOrd.Text=testsOrdered;
			FillGridResults();
			FillGridFacilities();
		}

		///<summary>Formatting for fields in this grid designed to emulate as accurately as possible the sample provided by LabCorp.</summary>
		private void FillGridResults() {
			gridResults.BeginUpdate();
			gridResults.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Test / Result",500);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Flag",115);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Units",110);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Reference Interval",145);
			gridResults.Columns.Add(col);
			col=new ODGridColumn("Lab",60);
			gridResults.Columns.Add(col);
			gridResults.Rows.Clear();
			ODGridRow row;
			MedLabs.GetListFacNums(ListMedLabs,out _listResults);//fills the classwide variable _listResults as well as returning the list of lab facility nums
			string obsDescriptPrev="";
			for(int i=0;i<_listResults.Count;i++) {
				//LabCorp requested that these non-performance results not be displayed on the report
				if((_listResults[i].ResultStatus==ResultStatus.F || _listResults[i].ResultStatus==ResultStatus.X)
					&& _listResults[i].ObsValue==""
					&& _listResults[i].Note=="")
				{
					continue;
				}
				string obsDescript="";
				MedLab medLabCur=MedLabs.GetOne(_listResults[i].MedLabNum);
				//Only dipslay full medLabCur.ObsTestDescript if different than previous Descript.
				if(i==0 || _listResults[i].MedLabNum!=_listResults[i-1].MedLabNum) {
					if(medLabCur.ActionCode!=ResultAction.G) {
						if(obsDescriptPrev==medLabCur.ObsTestDescript) {
							obsDescript=".";
						}
						else {
							obsDescript=medLabCur.ObsTestDescript;
							obsDescriptPrev=obsDescript;
						}
					}
				}
				//Set tabs using spaces and spaces2, can be changed further down in the code
				string spaces="    ";
				string spaces2="        ";
				string obsVal="";
				int padR=78;
				string newLine="";
				if(obsDescript!="") {
					if(obsDescript==_listResults[i].ObsText) {
						spaces="";
						spaces2="    ";
						padR=80;
					}
					else {
						obsVal+=obsDescript+"\r\n";
						newLine+="\r\n";
					}
				}
				if(_listResults[i].ObsValue=="Test Not Performed") {
					obsVal+=spaces+_listResults[i].ObsText;
				}
				else if(_listResults[i].ObsText=="."
					|| _listResults[i].ObsValue.Contains(":")
					|| _listResults[i].ObsValue.Length>20
					|| medLabCur.ActionCode==ResultAction.G)
				{
					obsVal+=spaces+_listResults[i].ObsText+"\r\n"+spaces2+_listResults[i].ObsValue.Replace("\r\n","\r\n"+spaces2);
					newLine+="\r\n";
				}
				else {
					obsVal+=spaces+_listResults[i].ObsText.PadRight(padR,' ')+_listResults[i].ObsValue;
				}
				if(_listResults[i].Note!="") {
					obsVal+="\r\n"+spaces2+_listResults[i].Note.Replace("\r\n","\r\n"+spaces2);
				}
				row=new ODGridRow();
				row.Cells.Add(obsVal);
				row.Cells.Add(newLine+MedLabResults.GetAbnormalFlagDescript(_listResults[i].AbnormalFlag));
				row.Cells.Add(newLine+_listResults[i].ObsUnits);
				row.Cells.Add(newLine+_listResults[i].ReferenceRange);
				row.Cells.Add(newLine+_listResults[i].FacilityID);
				row.Tag=_listResults[i];
				gridResults.Rows.Add(row);
			}
			gridResults.EndUpdate();
		}

		private void FillGridFacilities() {
			gridFacilities.BeginUpdate();
			gridFacilities.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("ID",40);//Facility ID from the MedLabResult
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Name",200);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Address",165);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("City",90);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("State",35);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Zip",70);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Phone",130);
			gridFacilities.Columns.Add(col);
			col=new ODGridColumn("Director",200);//FName LName, Title
			gridFacilities.Columns.Add(col);
			gridFacilities.Rows.Clear();
			ODGridRow row;
			//list of MedLabFacilityNums used by all results, the position in the list will be the facility id
			List<long> listFacNums=MedLabs.GetListFacNums(ListMedLabs,out _listResults);
			for(int i=0;i<listFacNums.Count;i++) {
				MedLabFacility facilityCur=MedLabFacilities.GetOne(listFacNums[i]);
				row=new ODGridRow();
				row.Cells.Add((i+1).ToString().PadLeft(2,'0'));//Actually more of a local renumbering of labs referenced by each Lab Result Row.
				row.Cells.Add(facilityCur.FacilityName);
				row.Cells.Add(facilityCur.Address);
				row.Cells.Add(facilityCur.City);
				row.Cells.Add(facilityCur.State);
				row.Cells.Add(facilityCur.Zip);
				row.Cells.Add(facilityCur.Phone);
				string directorName=facilityCur.DirectorFName;
				if(facilityCur.DirectorFName!="" && facilityCur.DirectorLName!="") {
					directorName+=" ";
				}
				directorName+=facilityCur.DirectorLName;
				if(directorName!="" && facilityCur.DirectorTitle!="") {
					directorName+=", "+facilityCur.DirectorTitle;
				}
				row.Cells.Add(directorName);//could be blank
				gridFacilities.Rows.Add(row);
			}
			gridFacilities.EndUpdate();
		}

		///<summary>Shows result history. Example: Show that a Corrected result had a Final and a Preliminary result in the past.</summary>
		private void gridResults_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedLabResultHist FormRH=new FormMedLabResultHist();
			FormRH.PatCur=PatCur;
			FormRH.ResultCur=(MedLabResult)gridResults.Rows[e.Row].Tag;
			FormRH.ShowDialog();
		}

		///<summary>Allows lab to be reassociated with a new patient.</summary>
		private void butPatSelect_Click(object sender,EventArgs e) {
			FormMedLabPatSelect FormPS=new FormMedLabPatSelect();
			FormPS.PatCur=PatCur;
			FormPS.ListMedLabs=ListMedLabs;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(PatCur==null || PatCur.PatNum!=FormPS.PatCur.PatNum) {
				PatCur=FormPS.PatCur;
				_isNewPat=true;//used to indicate that all MedLab objects need to be attached to the selected patient
			}
			RefreshPatientData();
		}

		private void RefreshPatientData() {
			if(PatCur==null) {//the MedLab object(s) are not attached to a patient, clear all pat fields
				textPatID.Text="";
				textPatLName.Text="";
				textPatFName.Text="";
				textPatMiddleI.Text="";
				textPatSSN.Text="";
				textBirthdate.Text="";
				textGender.Text="";
				return;
			}
			textPatID.Text=PatCur.PatNum.ToString();
			textPatLName.Text=PatCur.LName;
			textPatFName.Text=PatCur.FName;
			textPatMiddleI.Text=PatCur.MiddleI;
			textPatSSN.Text="****-**-"+PatCur.SSN.PadLeft(4,' ').Substring(PatCur.SSN.PadLeft(4,' ').Length-4,4);//mask all but the last 4 digits. Ex: ****-**-1234
			textBirthdate.Text=PatCur.Birthdate.ToShortDateString();
			if(PatCur.Birthdate.Year < 1880) {
				textBirthdate.Text="";
			}
			textGender.Text=PatCur.Gender.ToString();
		}

		private void butProvSelect_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormPP.SelectedProvNum!=_medLabCur.ProvNum) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Update all lab tests and results for this specimen with the selected ordering provider?")) {
					return;
				}
			}
			Provider prov=Providers.GetProv(FormPP.SelectedProvNum);
			for(int i=0;i<ListMedLabs.Count;i++) {
				ListMedLabs[i].OrderingProvLName=prov.LName;
				ListMedLabs[i].OrderingProvFName=prov.FName;
				ListMedLabs[i].OrderingProvNPI=prov.NationalProvID;
				ListMedLabs[i].OrderingProvLocalID=prov.ProvNum.ToString();
				ListMedLabs[i].ProvNum=prov.ProvNum;
				MedLabs.Update(ListMedLabs[i]);
			}
			string provName=prov.LName;
			if(provName!="" && prov.FName!="") {
				provName+=", ";
			}
			provName+=prov.FName;
			textPhysicianName.Text=provName;
			textPhysicianNPI.Text=prov.NationalProvID;
			textPhysicianID.Text=prov.ProvNum.ToString();
		}

		///<summary>Uses sheet framework to generate a PDF file, save it to patient's image folder, and attempt to launch file with defualt reader.
		///If using ImagesStoredInDB it will not launch PDF. If no valid patient is selected you cannot perform this action.</summary>
		private void butPDF_Click(object sender,EventArgs e) {
			if(PatCur==null) {//not attached to a patient when form loaded and they haven't selected a patient to attach to yet
				MsgBox.Show(this,"The Medical Lab must be attached to a patient before the PDF can be saved.");
				return;
			}
			if(_isNewPat) {//save the current patient attached to the MedLab if it has been changed
				MoveLabsAndImagesHelper();
			}
			Cursor=Cursors.WaitCursor;
			SheetDef sheetDef=SheetUtil.GetMedLabResultsSheetDef();
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,_medLabCur.PatNum);
			SheetFiller.FillFields(sheet,null,_medLabCur);
			SheetUtil.CalculateHeights(sheet,Graphics.FromImage(new Bitmap(sheet.HeightPage,sheet.WidthPage)),null,true,120,60,_medLabCur);
			//create the file in the temp folder location, then import so it works when storing images in the db
			string tempPath=ODFileUtils.CombinePaths(PrefL.GetTempFolderPath(),_medLabCur.PatNum.ToString()+".pdf");
			SheetPrinting.CreatePdf(sheet,tempPath,null,_medLabCur);
			HL7Def defCur=HL7Defs.GetOneDeepEnabled(true);
			long category=defCur.LabResultImageCat;
			if(category==0) {
				category=DefC.Short[(int)DefCat.ImageCats][0].DefNum;//put it in the first category.
			}
			//create doc--------------------------------------------------------------------------------------
			OpenDentBusiness.Document docc=null;
			try {
				docc=ImageStore.Import(tempPath,category,Patients.GetPat(_medLabCur.PatNum));
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Error saving document.");
				return;
			}
			finally {
				//Delete the temp file since we don't need it anymore.
				try {
					File.Delete(tempPath);
				}
				catch {
					//Do nothing.  This file will likely get cleaned up later.
				}
			}
			docc.Description=Lan.g(this,"MedLab Result");
			docc.DateCreated=DateTime.Now;
			Documents.Update(docc);
			string filePathAndName="";
			if(PrefC.AtoZfolderUsed) {
				string patFolder=ImageStore.GetPatientFolder(Patients.GetPat(_medLabCur.PatNum),ImageStore.GetPreferredAtoZpath());
				filePathAndName=ODFileUtils.CombinePaths(patFolder,docc.FileName);
			}
			Cursor=Cursors.Default;
			if(filePathAndName!="") {
				Process.Start(filePathAndName);
			}
			SecurityLogs.MakeLogEntry(Permissions.SheetEdit,sheet.PatNum,sheet.Description+" from "+sheet.DateTimeSheet.ToShortDateString()+" pdf was created");
			DialogResult=DialogResult.OK;
		}


		private void butShowHL7_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			List<string[]> listFileNamesDateMod=new List<string[]>();
			for(int i=0;i<ListMedLabs.Count;i++) {
				string filePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),ListMedLabs[i].FileName);
				bool isFileAdded=false;
				for(int j=0;j<listFileNamesDateMod.Count;j++) {
					if(listFileNamesDateMod[j][0]==filePath) {
						isFileAdded=true;
						break;
					}
				}
				if(isFileAdded) {
					continue;
				}
				string dateModified=DateTime.MinValue.ToString();
				try {
					dateModified=File.GetLastWriteTime(filePath).ToString();
				}
				catch(Exception ex) {
					//dateModified will be min value, do nothing?
				}
				listFileNamesDateMod.Add(new string[] { filePath,dateModified });
			}
			FormMedLabHL7MsgText FormMsgText=new FormMedLabHL7MsgText();
			FormMsgText.ListFileNamesDatesMod=listFileNamesDateMod;
			Cursor=Cursors.Default;
			FormMsgText.ShowDialog();
		}

		///<summary>Used to revert any changes made to the MedLab object 
		///OR create embedded files attached to the currently selected patient.
		///This will re-process the original HL7 message(s) from the archived file(s).
		///Since there may be more than one HL7 message that comprises the information on the form, all messages for this specimen will be re-processed.
		///The old MedLab object(s) and any MedLabResult, MedLabSpecimen, or MedLabFacAttach objects linked will be deleted and the form will fill with
		///the new object based on the original file data.  Right now users are only allowed to change the patient and provider, but more changes may be
		///possible in the future that will be reverted.</summary>
		private bool ReprocessMessages() {
			Dictionary<string,string> dictFileNameFileText=new Dictionary<string,string>();//Key=Full file path to message file, value=HL7 message content
			for(int i=0;i<ListMedLabs.Count;i++) {
				string filePath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),ListMedLabs[i].FileName);
				try { //File IO, surround with try catch
					if(!dictFileNameFileText.ContainsKey(filePath)) {
						string fileTextCur=File.ReadAllText(filePath);
						dictFileNameFileText.Add(filePath,fileTextCur);
					}
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Could not read the MedLab HL7 message text file located at")+" "+ListMedLabs[i].FileName+".");
					return false;
				}
			}
			List<long> listMedLabNumsNew=new List<long>();
			foreach(KeyValuePair<string,string> fileCur in dictFileNameFileText) {
				MessageHL7 msg=new MessageHL7(fileCur.Value);
				List<long> listMedLabNumsCur=MessageParserMedLab.Process(msg,fileCur.Key,false,PatCur);//re-creates the documents from the ZEF segments
				if(listMedLabNumsCur==null || listMedLabNumsCur.Count<1) {
					MsgBox.Show(this,"The HL7 message processed did not produce any MedLab objects.");
					return false;
				}
				listMedLabNumsNew.AddRange(listMedLabNumsCur);
			}
			//Delete all records except the ones just created
			DeleteLabsAndResults(_medLabCur, listMedLabNumsNew);
			ListMedLabs=MedLabs.GetForPatAndSpecimen(PatCur.PatNum,_medLabCur.SpecimenID,_medLabCur.SpecimenIDFiller);//handles PatNum=0
			_medLabCur=ListMedLabs[0];
			SetFields();
			return true;
		}

		///<summary>Cascading delete that deletes all MedLab, MedLabResult, MedLabSpecimen, and MedLabFacAttach.
		///Also deletes any embedded PDFs that are linked to by the MedLabResults.
		///The MedLabs and all associated results, specimens, and FacAttaches referenced by the MedLabNums in listExcludeMedLabNums will not be deleted.
		///Used for deleting old entries and keeping new ones.  The list may be empty and then all will be deleted.</summary>
		private void DeleteLabsAndResults(MedLab medLab,List<long> listExcludeMedLabNums=null) {
			List<MedLab> listLabsOld=MedLabs.GetForPatAndSpecimen(medLab.PatNum,medLab.SpecimenID,medLab.SpecimenIDFiller);//patNum could be 0
			List<long> listLabNumsOld=new List<long>();
			for(int i=listLabsOld.Count-1;i>-1;i--) {//backwards to remove any that are in the exclude list so it's filtered before filling the result list
				if(listExcludeMedLabNums!=null && listExcludeMedLabNums.Contains(listLabsOld[i].MedLabNum)) {
					listLabsOld.RemoveAt(i);
					continue;
				}
				listLabNumsOld.Add(listLabsOld[i].MedLabNum);
			}
			List<MedLabResult> listResultsOld=MedLabResults.GetAllForLabs(listLabsOld);
			List<long> listResultNumsOld=new List<long>();
			List<Document> listDocs=new List<Document>();
			for(int i=0;i<listResultsOld.Count;i++) {
				listResultNumsOld.Add(listResultsOld[i].MedLabResultNum);
				if(listResultsOld[i].DocNum==0) {
					continue;
				}
				//if DocNum is invalid, a new document object will be added to the list, but it will be skipped when trying to delete since the file won't exist
				listDocs.Add(Documents.GetByNum(listResultsOld[i].DocNum));
			}
			if(listDocs.Count>0) {
				Patient labPat=Patients.GetPat(medLab.PatNum);
				if(labPat!=null) {
					try {
						ImageStore.DeleteDocuments(listDocs,ImageStore.GetPatientFolder(labPat,ImageStore.GetPreferredAtoZpath()));
					}
					catch(Exception ex) {
						MsgBox.Show(this,"Some images referenced by the MedLabResults could not be deleted and will have to be removed manually.");
					}
				}
			}
			MedLabSpecimens.DeleteAllForLabs(listLabNumsOld);
			MedLabFacAttaches.DeleteAllForLabsOrResults(listLabNumsOld,listResultNumsOld);
			MedLabResults.DeleteAll(listResultNumsOld);
			MedLabs.DeleteAll(listLabNumsOld);
		}

		///<summary>Moves all MedLab objects and any embedded PDFs tied to the MedLabResults to the PatCur.
		///Assumes PatCur and the MedLab.PatNum is not the same patient.
		///If the MedLab objects were not originally attached to a patient, the message text will be re-processed so any embedded PDFs will be created.</summary>
		private void MoveLabsAndImagesHelper() {
			//if the MedLab object(s) were attached to a patient and they are being moved to another patient, move the associated documents
			Patient patOld=Patients.GetPat(_medLabCur.PatNum);
			if(patOld==null) {
				//the MedLab objects were not originally attached to a patient due to the patient not being located when processing the message
				//reprocess using PatCur so the MedLab objects will be attached to PatCur and the ZEF segments can be converted into the embedded PDF file(s) 
				ReprocessMessages();
				_isNewPat=false;
				return;
			}
			for(int i=0;i<ListMedLabs.Count;i++) {
				ListMedLabs[i].PatNum=PatCur.PatNum;
				MedLabs.Update(ListMedLabs[i]);
			}
			List<MedLabResult> listResults=MedLabResults.GetAllForLabs(ListMedLabs);
			string atozPath="";
			string atozFrom="";
			string atozTo="";
			if(PrefC.AtoZfolderUsed) {
				atozPath=ImageStore.GetPreferredAtoZpath();
				atozFrom=ImageStore.GetPatientFolder(patOld,atozPath);
				atozTo=ImageStore.GetPatientFolder(PatCur,atozPath);
			}
			int fileMoveFailures=0;
			for(int i=0;i<listResults.Count;i++) {
				if(listResults[i].DocNum==0) {
					continue;
				}
				Document fromDoc=Documents.GetByNum(listResults[i].DocNum);
				if(fromDoc.DocNum==0) {//DocNum could be 0 if document is not found, GetByNum returns a new doc, not null, if invalid DocNum
					continue;
				}
				if(!PrefC.AtoZfolderUsed) {
					//storing docs in the db, so simply update the PatNum, doc is stored in the RawBase64 column and file name doesn't need to be unique
					fromDoc.PatNum=PatCur.PatNum;
					Documents.Update(fromDoc);
					continue;
				}
				string fromFilePath=ODFileUtils.CombinePaths(atozFrom,fromDoc.FileName);
				if(!File.Exists(fromFilePath)) {
					//the DocNum in the MedLabResults table is pointing to a file that either doesn't exist or is not accessible, can't move/copy it
					fileMoveFailures++;
					continue;
				}
				string destFileName=fromDoc.FileName;
				string destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
				if(File.Exists(destFilePath)) {
					//The file being copied has the same name as a file that exists in the destination folder, use a unique file name and update document table
					destFileName=patOld.PatNum.ToString()+"_"+fromDoc.FileName;//try to prepend patient's PatNum to the original file name
					destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
					while(File.Exists(destFilePath)) {
						//if still not unique, try appending date/time to seconds precision until the file name is unique
						destFileName=patOld.PatNum.ToString()+"_"+Path.GetFileNameWithoutExtension(fromDoc.FileName)
							+"_"+DateTime.Now.ToString("yyyyMMddhhmmss")+Path.GetExtension(fromDoc.FileName);
						destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
					}
				}
				bool isCopied=true;
				try {
					File.Copy(fromFilePath,destFilePath);
				}
				catch(Exception ex) {
					isCopied=false;
					fileMoveFailures++;
				}
				if(isCopied) {//if the file is copied successfully, try to delete the original file
					//Safe to update the document FileName and PatNum to PatCur and new file name
					Documents.MergePatientDocument(patOld.PatNum,PatCur.PatNum,fromDoc.FileName,destFileName);
					try {
						File.Delete(fromFilePath);
					}
					catch(Exception ex) {
						//If we cannot delete the file, could be a permission issue or someone has the file open currently
						//Just skip deleting the file, which means there could be an image in the old pat's folder that may need to be deleted manually
						fileMoveFailures++;
					}
				}
			}
			if(fileMoveFailures>0) {
				MessageBox.Show(Lan.g(this,"Some files attached to the MedLab objects could not be moved.")+"\r\n"
					+Lan.g(this,"This could be due to a missing file, a file being open, or a permission issue on the file which is preventing the move.")+"\r\n"
					+Lan.g(this,"The file(s) will have to be moved manually from the Image module.")+"\r\n"
					+Lan.g(this,"Number of files not moved")+": "+fileMoveFailures);
			}
			_isNewPat=false;
		}

		///<summary>This will delete all MedLab objects for the specimen referenced by _medLabCur and all MedLabResult, MedLabSpecimen,
		///and MedLabFacAttach objects, as well as any documents referenced by the results.  The original HL7 message will remain in the image folder,
		///but this MedLab will not point to it.  We won't remove the HL7 message since there may be other MedLab rows that point to it.</summary>
		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will delete all orders, results, and specimens for this MedLab as well as any associated pdf files.")) {
				return;
			}
			DeleteLabsAndResults(_medLabCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_isNewPat && PatCur!=null) {
				MoveLabsAndImagesHelper();
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}