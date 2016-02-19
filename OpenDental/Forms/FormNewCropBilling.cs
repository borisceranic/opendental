using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental {
	public partial class FormNewCropBilling:Form {

		public FormNewCropBilling() {
			InitializeComponent();
		}

		private void FormBillingList_Resize(object sender,EventArgs e) {
			RefreshGridColumns();
		}

		private void butBrowse_Click(object sender,EventArgs e) {
			if(openFileDialog1.ShowDialog()==DialogResult.OK) {
				textBillingXmlPath.Text=openFileDialog1.FileName;
			}
		}

		private void butLoad_Click(object sender,EventArgs e) {
			if(!File.Exists(textBillingXmlPath.Text)) {
				MessageBox.Show("File does not exist or could not be accessed. Make sure the file is not open in another program and try again.");
				return;
			}
			FillGrid();
		}

		private void RefreshGridColumns() {
			gridBillingList.BeginUpdate();
			gridBillingList.Columns.Clear();
			int gridWidth=this.Width-50;
			int patNumWidth=54;//fixed width
			int npiWidth=70;//fixed width
			int yearMonthAddedWidth=104;//fixed width
			int isNewWidth=46;//fixed width
			int typeWidth=40;//fixed width
			int variableWidth=gridWidth-patNumWidth-npiWidth-yearMonthAddedWidth-isNewWidth-typeWidth;
			int practiceTitleWidth=variableWidth/2;//variable width
			int firstLastNameWidth=variableWidth-practiceTitleWidth;//variable width
			gridBillingList.Columns.Add(new ODGridColumn("PatNum",patNumWidth,HorizontalAlignment.Center));//0
			gridBillingList.Columns.Add(new ODGridColumn("NPI",npiWidth,HorizontalAlignment.Center));//1
			gridBillingList.Columns.Add(new ODGridColumn("YearMonthBilling",yearMonthAddedWidth,HorizontalAlignment.Center));//2
			gridBillingList.Columns.Add(new ODGridColumn("Type",typeWidth,HorizontalAlignment.Center));//3
			gridBillingList.Columns.Add(new ODGridColumn("IsNew",isNewWidth,HorizontalAlignment.Center));//4
			gridBillingList.Columns.Add(new ODGridColumn("PracticeTitle",practiceTitleWidth,HorizontalAlignment.Left));//5
			gridBillingList.Columns.Add(new ODGridColumn("FirstLastName",firstLastNameWidth,HorizontalAlignment.Left));//6
			gridBillingList.EndUpdate();
		}

		private void FillGrid() {
			try {
				string xmlData=File.ReadAllText(textBillingXmlPath.Text);
				xmlData=xmlData.Replace("&nbsp;","");
				XmlDocument xml=new XmlDocument();
				xml.LoadXml(xmlData);
				XmlNode divNode=xml.FirstChild;
				XmlNode tableNode=divNode.FirstChild;
				RefreshGridColumns();
				gridBillingList.BeginUpdate();
				gridBillingList.Rows.Clear();
				for(int i=1;i<tableNode.ChildNodes.Count;i++) { //Skip the first row, because it contains the column names.
					ODGridRow gr=new ODGridRow();
					XmlNode trNode=tableNode.ChildNodes[i];
					//0 PatNum
					string shortName=trNode.ChildNodes[1].InnerText;
					int accountIdStartIndex=shortName.IndexOf("-")+1;
					int accountIdLength=shortName.Substring(accountIdStartIndex).LastIndexOf("-");
					string accountId=shortName.Substring(accountIdStartIndex,accountIdLength);
					int patNumLength=accountId.IndexOf("-");
					string patNumStr=PIn.String(accountId.Substring(0,patNumLength));
					if(patNumStr=="6566") {//Account 6566 corresponds to our software key in the training database. These accounts are test accounts.
						continue;//Do not show OD test accounts.
					}
					long patNum=PIn.Long(patNumStr);
					gr.Cells.Add(new ODGridCell(patNumStr));
					//1 NPI
					string npi=PIn.String(trNode.ChildNodes[8].InnerText);
					gr.Cells.Add(new ODGridCell(npi));
					//2 YearMonthAdded
					gr.Cells.Add(new ODGridCell(trNode.ChildNodes[9].InnerText));
					//3 Type (N=New customer who began using eRx in the report month, U=Customer who used eRx one month prior to report month, B=Both N and U.)
					string billType=trNode.ChildNodes[10].InnerText;
					gr.Cells.Add(new ODGridCell(billType));
					//4 IsNew
					List<RepeatCharge> listRepeatChargesCur=RepeatCharges.GetForErx(patNum);
					RepeatCharge repeatChargeForNpi=GetRepeatChargeForNPI(listRepeatChargesCur,npi);
					bool isNew=false;
					//If billType=="U", then we will not post a repeating charge, since it should have been posted in the previous reporting month already.
					//Not posting repeating charges where billType="U" allows our techs to delete repeating charges for customers who have cancelled.
					//The repeating charge tool will not create any charges older than 50 days, thus adding this repeating charge is useless anyway.
					if(repeatChargeForNpi==null && billType!="U") {
						isNew=true;
					}
					gr.Cells.Add(new ODGridCell(isNew?"X":""));
					//5 PracticeTitle
					gr.Cells.Add(new ODGridCell(trNode.ChildNodes[0].InnerText));
					//6 FirstLastName
					gr.Cells.Add(new ODGridCell(trNode.ChildNodes[2].InnerText));
					gridBillingList.Rows.Add(gr);
				}
				gridBillingList.EndUpdate();
			}
			catch(Exception ex) {
				MessageBox.Show("There is something wrong with the input file. Try again. If issue persists, then contact a programmer: "+ex.Message);
			}
		}

		///<summary>Searches the repeatChargesCur list for the eRx repeating charge related to the given npi.
		///A repeating charge is a match if the note beings with "NPIs=" followed by the given npi, or if the note simply starts with the npi.
		///Returns null if no match found.</summary>
		private RepeatCharge GetRepeatChargeForNPI(List <RepeatCharge> repeatChargesCur,string npi) {
			for(int i=0;i<repeatChargesCur.Count;i++) {
				RepeatCharge rc=repeatChargesCur[i];
				string note=rc.Note.Trim();
				if(note.ToUpper().StartsWith("NPI=")) {//Case insensitive check
					note=note.Substring(4);//Remove the leading NPI=
				}
				if(note.StartsWith(npi)) {
					return rc;
				}
			}
			return null;
		}

		///<summary>Returns a code in format Z###, depending on which codes are already in use for the current patnum.
		///The returned code is guaranteed to exist in the database, because codes are created if they do not exist.</summary>
		private string GetProcCodeForNewCharge(List<RepeatCharge> repeatChargesCur) {
			//Locate a proc code for eRx which is not already in use.
			string procCode="Z000";
			int attempts=0;
			bool procCodeInUse;
			do {
				procCodeInUse=false;
				for(int i=0;i<repeatChargesCur.Count;i++) {
					if(repeatChargesCur[i].ProcCode==procCode) {
						procCodeInUse=true;
						break;
					}
				}
				if(procCodeInUse) {
					attempts++;//Should start at 2. The Codes will be "Z001", "Z002", "Z003", etc...
					if(attempts>999) {
						throw new Exception("Cannot add more than 999 Z-codes yet.  Ask programmer to increase.");
					}
					procCode="Z"+(attempts.ToString().PadLeft(3,'0'));
				}
			} while(procCodeInUse);
			//If the selected code is not in the database already, then add it automatically.
			long codeNum=ProcedureCodes.GetCodeNum(procCode);
			if(codeNum==0) {//The selected code does not exist, so we must add it.
				ProcedureCode code=new ProcedureCode();
				code.ProcCode=procCode;
				code.Descript="Electronic Rx";
				code.AbbrDesc="eRx";
				code.ProcTime="/X/";
				code.ProcCat=162;//Software
				code.TreatArea=TreatmentArea.Mouth;
				ProcedureCodes.Insert(code);
				ProcedureCodes.RefreshCache();
			}
			return procCode;
		}

		private int GetChargeDayOfMonth(long patNum) {
			//Match the day of the month for the eRx repeating charge to their existing monthly support charge (even if the monthly support is disabled).
			int day=15;//Day 15 will be used if they do not have any existing repeating charges.
			RepeatCharge[] chargesForPat=RepeatCharges.Refresh(patNum);
			bool hasMaintCharge=false;
			for(int j=0;j<chargesForPat.Length;j++) {
				if(chargesForPat[j].ProcCode=="001") {//Monthly maintenance repeating charge
					hasMaintCharge=true;
					day=chargesForPat[j].DateStart.Day;
					break;
				}
			}
			//The customer is not on monthly support, so use any other existing repeating charge day (example EHR Monthly and Mobile).
			if(!hasMaintCharge && chargesForPat.Length>0) {
				day=chargesForPat[0].DateStart.Day;
			}
			return day;
		}

		private void butProcess_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will add a new repeating charge for each provider in the list above"
				+" who is new (does not already have a repeating charge), based on PatNum and NPI.  Continue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			int numChargesAdded=0;
			int numSkipped=0;
			StringBuilder strBldArchivedPats=new StringBuilder();
			for(int i=0;i<gridBillingList.Rows.Count;i++) {
				long patNum=PIn.Long(gridBillingList.Rows[i].Cells[0].Text);
				string npi=PIn.String(gridBillingList.Rows[i].Cells[1].Text);
				string billingType=gridBillingList.Rows[i].Cells[3].Text;
				List<RepeatCharge> listErxRepeatCharges=RepeatCharges.GetForErx(patNum);
				RepeatCharge repeatCur=GetRepeatChargeForNPI(listErxRepeatCharges,npi);
				if(repeatCur==null) {//No such repeating charge exists yet for the given npi.
					if(gridBillingList.Rows[i].Cells[4].Text!="X") {
						continue;//Only create a charge for rows marked new.
					}
					//We consider the provider a new provider and create a new repeating charge.
					string yearMonth=gridBillingList.Rows[i].Cells[2].Text;
					int yearBilling=PIn.Int(yearMonth.Substring(0,4));//The year chosen by the OD employee when running the eRx Billing report.
					int monthBilling=PIn.Int(yearMonth.Substring(4));//The month chosen by the OD employee when running the eRx Billing report.
					int dayOtherCharges=GetChargeDayOfMonth(patNum);//The day of the month that the customer already has other repeating charges. Keeps their billing simple (one bill per month for all charges).
					int daysInMonth=DateTime.DaysInMonth(yearBilling,monthBilling);
					if(dayOtherCharges>daysInMonth) {
						//The day that the user used eRx (signed up) was in a month that does not have the day of the other monthly charges in it.
						//E.g.  dayOtherCharges = 31 and the user started a new eRx account in a month without 31 days.
						//Therefore, we have to use the last day of the month that they started.
						//This can introduce multiple statements being sent out which can potentially delay us (HQ) from getting paid in a timely fashion.
						//A workaround for this would be to train our techs to never run billing after the 28th of every month that way incomplete statements are not sent.
						dayOtherCharges=daysInMonth;
					}
					DateTime dateErxCharge=new DateTime(yearBilling,monthBilling,dayOtherCharges);
					if(dateErxCharge<DateTime.Today.AddMonths(-3)) {//Just in case the user runs an older report.
						numSkipped++;
						continue;
					}
					repeatCur=new RepeatCharge();
					repeatCur.IsNew=true;
					repeatCur.PatNum=patNum;
					repeatCur.ProcCode=GetProcCodeForNewCharge(listErxRepeatCharges);
					repeatCur.ChargeAmt=15;//15$/month
					repeatCur.DateStart=dateErxCharge;
					repeatCur.Note="NPI="+npi;
					repeatCur.IsEnabled=true;
					repeatCur.CopyNoteToProc=true;//Copy the billing note to the procedure note by default so that the customer can see the NPI the charge corresponds to. Can be unchecked by user if a private note is added later (rare).
					if(!RepeatCharges.ActiveRepeatChargeExists(repeatCur.PatNum)) { 
						//Set the patient's billing day to the start day on the repeat charge
						Patient patOld=Patients.GetPat(repeatCur.PatNum);
						Patient patNew=patOld.Copy();
						//Check the patients status and move them to Archived if they are currently deleted.
						if(patOld.PatStatus==PatientStatus.Deleted) {
							patNew.PatStatus=PatientStatus.Archived;
						}
						//Notify the user about any deleted or archived patients that were just given a new repeating charge.
						if(patOld.PatStatus==PatientStatus.Archived || patOld.PatStatus==PatientStatus.Deleted) {
							strBldArchivedPats.AppendLine("#"+patOld.PatNum+" - "+patOld.GetNameLF());
						}
						patNew.BillingCycleDay=repeatCur.DateStart.Day;
						Patients.Update(patNew,patOld);
					}
					RepeatCharges.Insert(repeatCur);
					numChargesAdded++;
				}
				else { //The repeating charge for eRx billing already exists for the given npi.
					DateTime dateEndLastMonth=(new DateTime(DateTime.Today.Year,DateTime.Today.Month,1)).AddDays(-1);
					if(billingType=="B" || billingType=="N") {//The provider sent eRx last month.
						if(repeatCur.DateStop.Year>2010) {//eRx support for this provider was disabled at one point, but has been used since.
							if(repeatCur.DateStop<dateEndLastMonth) {//If the stop date is in the future or already at the end of the month, then we cannot presume that there will be a charge next month.
								repeatCur.DateStop=dateEndLastMonth;//Make sure the recent use is reflected in the end date.
								RepeatCharges.Update(repeatCur);
							}
						}
					}
					else if(billingType=="U") {//The provider did not send eRx last month, but did send eRx two months ago.
						//Customers must call in to disable repeating charges, they are not disabled automatically.
					}
					else {
						throw new Exception("Unknown eRx Billing type "+billingType);
					}
				}
			}
			FillGrid();
			Cursor=Cursors.Default;
			string msg="Done. Number of provider charges added: "+numChargesAdded;
			if(numSkipped>0) {
				msg+=Environment.NewLine+"Number skipped due to old DateBilling (over 3 months ago): "+numSkipped;
			}
			if(strBldArchivedPats.Length > 0) {
				msg+=Environment.NewLine+"Archived patients that had a repeating charge created:"
					+Environment.NewLine+strBldArchivedPats.ToString();
			}
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(msg);
			msgBoxCP.Show();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
