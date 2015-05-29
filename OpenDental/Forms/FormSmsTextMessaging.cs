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
			listStatus.Items.Add("Junk");//3
			FillGridTextMessages();
		}

		private void FillGridTextMessages() {
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<_listClinics.Count;i++) {
				listClinicNums.Add(_listClinics[i].ClinicNum);
			}
			SmsFromStatus[] listSmsFromStatuses=new SmsFromStatus[listStatus.SelectedIndices.Count];
			for(int i=0;i<listStatus.SelectedIndices.Count;i++) {
				int index=listStatus.SelectedIndices[i];
				if(index==0) {
					//sent
				}
				else if(index==1) {
					listSmsFromStatuses[i]=SmsFromStatus.ReceivedUnread;
				}
				else if(index==2) {
					listSmsFromStatuses[i]=SmsFromStatus.ReceivedRead;
				}
				else if(index==3) {
					listSmsFromStatuses[i]=SmsFromStatus.Junk;
				}
			}
			List <SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text),
				listClinicNums,listSmsFromStatuses);

		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}