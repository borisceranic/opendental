using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSmsTextMessaging:Form {

		private List<Clinic> _listClinics;
		///<summary>Set before showing this form.  If true, will initially show sent messages.  Otherwise will initially show received messages.</summary>
		public bool IsSent;

		public FormSmsTextMessaging() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSmsTextMessaging_Load(object sender,EventArgs e) {
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboClinic.Items.Clear();
			for(int i=0;i<_listClinics.Count;i++) {
				comboClinic.Items.Add(_listClinics[i].Description);
				comboClinic.SetSelected(i,true);
			}
			textDateFrom.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTimeOD.Today.ToShortDateString();
			listStatus.Items.Clear();
			listStatus.Items.Add("Sent");//0
			if(IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Unread");//1
			if(!IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Read");//2
			if(!IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Junk");//3
			FillGridTextMessages();
		}

		private void FillGridTextMessages() {
			int sortByColIdx=gridMessages.SortedByColumnIdx;
			bool isSortAsc=gridMessages.SortedIsAscending;
			if(sortByColIdx==-1) {
				//Default to sorting by Date descending.
				sortByColIdx=0;
				isSortAsc=false;
			}
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("Date",65,HorizontalAlignment.Center));
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left));
			gridMessages.Columns.Add(new UI.ODGridColumn("Type",80,HorizontalAlignment.Center));
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",80,HorizontalAlignment.Center));
			gridMessages.Columns.Add(new UI.ODGridColumn("Message",0,HorizontalAlignment.Left));
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<_listClinics.Count;i++) {
				listClinicNums.Add(_listClinics[i].ClinicNum);
			}
			bool isSent=false;
			List <SmsFromStatus> listSmsFromStatuses=new List<SmsFromStatus>();
			for(int i=0;i<listStatus.SelectedIndices.Count;i++) {
				int index=listStatus.SelectedIndices[i];
				if(index==0) {
					isSent=true;
				}
				else if(index==1) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedUnread);
				}
				else if(index==2) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedRead);
				}
				else if(index==3) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedJunk);
				}
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			Dictionary<long,string> dictPatNames=Patients.GetAllPatientNames();
			if(listSmsFromStatuses.Count>0) {
				List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(dateFrom,dateTo,listClinicNums,listSmsFromStatuses.ToArray());				
				for(int i=0;i<listSmsFromMobile.Count;i++) {
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsFromMobile[i];
					row.Cells.Add(listSmsFromMobile[i].DateTimeReceived.ToShortDateString());//Date
					if(listSmsFromMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsFromMobile[i].PatNum]);//Patient
					}
					row.Cells.Add(Lan.g(this,"Received"));//Type
					row.Cells.Add(SmsFromMobiles.GetSmsFromStatusDescript(listSmsFromMobile[i].SmsStatus));//Status
					row.Cells.Add(listSmsFromMobile[i].MsgText);//Message
					gridMessages.Rows.Add(row);
				}
			}
			if(isSent) {
				List<SmsToMobile> listSmsToMobile=SmsToMobiles.GetMessages(dateFrom,dateTo,listClinicNums);
				for(int i=0;i<listSmsToMobile.Count;i++) {
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsToMobile[i];
					row.Cells.Add(listSmsToMobile[i].DateTimeSent.ToShortDateString());//Date
					if(listSmsToMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsToMobile[i].PatNum]);//Patient
					}
					row.Cells.Add(Lan.g(this,"Sent"));//Type
					row.Cells.Add(listSmsToMobile[i].Status.ToString());//Status
					row.Cells.Add(listSmsToMobile[i].MsgText);//Message
					gridMessages.Rows.Add(row);
				}
			}
			gridMessages.EndUpdate();
			gridMessages.SortForced(sortByColIdx,isSortAsc);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridTextMessages();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}