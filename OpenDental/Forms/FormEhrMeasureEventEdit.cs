using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental;
using OpenDental.UI;

namespace OpenDental {
	///<summary>Only used for editing smoking documentation.</summary>
	public partial class FormEhrMeasureEventEdit:Form {
		private EhrMeasureEvent _measureEventCur;

		public FormEhrMeasureEventEdit(EhrMeasureEvent measureEventCur) {
			InitializeComponent();
			_measureEventCur=measureEventCur;
		}

		private void FormEhrMeasureEventEdit_Load(object sender,EventArgs e) {
			textDateTime.Text=_measureEventCur.DateTEvent.ToString();
			Patient patCur=Patients.GetPat(_measureEventCur.PatNum);
			if(patCur!=null) {
				textPatient.Text=patCur.GetNameFL();
			}
			if(_measureEventCur.EventType==EhrMeasureEventType.TobaccoUseAssessed) {
				Loinc lCur=Loincs.GetByCode(_measureEventCur.CodeValueEvent);//TobaccoUseAssessed events can be one of three types, all LOINC codes
				if(lCur!=null) {
					textType.Text=lCur.NameLongCommon;//Example: History of tobacco use Narrative
				}
				Snomed sCur=Snomeds.GetByCode(_measureEventCur.CodeValueResult);//TobaccoUseAssessed results can be any SNOMEDCT code, we recommend one of 8 codes, but the CQM measure allows 54 codes and we let the user select any SNOMEDCT they want
				if(sCur!=null) {
					textResult.Text=sCur.Description;//Examples: Non-smoker (finding) or Smoker (finding)
				}
			}
			else {
				//Currently, the TobaccoUseAssessed events are the only ones that can be deleted.
				butDelete.Enabled=false;
			}
			if(textType.Text==""){//if not set by LOINC name above, then either not a TobaccoUseAssessed event or the code was not in the LOINC table, fill with EventType
				textType.Text=_measureEventCur.EventType.ToString();
			}
			textMoreInfo.Text=_measureEventCur.MoreInfo;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			string logEntry=Lan.g(this,"Ehr Measure Event was deleted.")+"  "
				+Lan.g(this,"Date")+": "+PIn.DateT(textDateTime.Text)+"  "
				+Lan.g(this,"Type")+": "+_measureEventCur.EventType.ToString()+"  "
				+Lan.g(this,"Patient")+": "+textPatient.Text;
			SecurityLogs.MakeLogEntry(Permissions.EhrMeasureEventEdit,_measureEventCur.PatNum,logEntry);
			EhrMeasureEvents.Delete(_measureEventCur.EhrMeasureEventNum);
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//inserts never happen here.  Only updates.
			if(textDateTime.errorProvider1.GetError(textDateTime)!="") {
				MsgBox.Show(this,"Please enter a valid date time.");
				return;
			}
			string logEntry="";
			if(_measureEventCur.MoreInfo!=textMoreInfo.Text) {
				logEntry+=Lan.g(this,"EHR Measure Event was edited.  More Info was changed.")+"  ";
			}
			DateTime dateTEvent=PIn.DateT(textDateTime.Text);
			if(_measureEventCur.DateTEvent!=dateTEvent) {
				if(logEntry=="") {
					logEntry+=Lan.g(this,"EHR Measure Event was edited.")+"  ";
				}
				logEntry+=Lan.g(this,"Date was changed from")+": "+_measureEventCur.DateTEvent.ToString()+" "
					+Lan.g(this,"to")+": "+dateTEvent.ToString();
			}
			if(logEntry=="") {//No changes occurred.
				DialogResult=DialogResult.OK;
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.EhrMeasureEventEdit,_measureEventCur.PatNum,logEntry);
			_measureEventCur.MoreInfo=textMoreInfo.Text;
			_measureEventCur.DateTEvent=dateTEvent;
			EhrMeasureEvents.Update(_measureEventCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}




	


	}
}
